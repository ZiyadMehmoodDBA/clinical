Patient_Document_Image_Annotation = {
    bIsFirstLoad: true,
    params: [],
    scale: 1,
    Load: function (params) {

        Patient_Document_Image_Annotation.params = params;
        if (Patient_Document_Image_Annotation.bIsFirstLoad) {

            var self = $('#pnlImageAnnotation #frmImage_Annotation');

            self.loadDropDowns(true).done(function () {

                Patient_Document_Image_Annotation.ValidateImage_Annotation();
                Patient_Document_Image_Annotation.FillData();
                $("#frmImage_Annotation").data('serialize', $("#frmImage_Annotation").serialize());
            });
        }

        utility.callbackAfterAllDOMLoaded(function () {
            $("#frmImage_Annotation").data('serialize', $("#frmImage_Annotation").serialize());
        });

    },

    FillData: function () {

        var self = $('#pnlImageAnnotation #frmImage_Annotation');
        var documentCall = Document_Viewer.FillDocument(null, Patient_Document_Image_Annotation.params.PatientID, Patient_Document_Image_Annotation.params.PatDocID);
        $.when(documentCall).done(function (response) {
            // documentCall.done(function (response) {
            if (response.status != false) {
                var document_details = JSON.parse(response.DocumentLoad_JSON);
                var FileType = document_details.FileType;
                var LoadPrevious = document_details.txtLoadPrevious;

                self.find("#txtFullName").val(document_details.txtPatientName)
                self.find("#txtFileName").val(document_details.txtFileName);
                self.find("#lnkFileNameExt").val(document_details.lnkFileNameExt);
                self.find("#txtComments").val(document_details.txtComments);
                self.find("#ddlFolder").val(document_details.ddlFolder);

                //utility.bindMyJSON(true, document_details, false, self);

                if (FileType.indexOf("image") > -1) {


                    try {

                        Patient_Document_Image_Annotation.params["Res_FileType"] = document_details.FileType;
                        Patient_Document_Image_Annotation.params["Res_Base64FileStream"] = document_details.Base64FileStream;

                        Patient_Document_Image_Annotation.CreateAnnotateLiterallyEditor(document_details.FileType, document_details.Base64FileStream);

                    }
                    catch (ex) {
                        utility.DisplayMessages(ex, 2);
                        console.log(ex);
                    }
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            $("#frmImage_Annotation").data('serialize', $("#frmImage_Annotation").serialize());
        });


    },

    CreateAnnotateLiterallyEditor: function (FileType, Base64FileStream) {

        var imageObj = new Image();
        imageObj.src = "data:" + FileType + ";base64," + Base64FileStream;

        setTimeout(function () {
          
            Patient_Document_Image_Annotation.params.lc = LC.init(document.getElementById('annotation_editor'), {
                imageSize: { width: imageObj.width, height: imageObj.height },
                keyboardShortcuts: false,
                tools: [LC.tools.Pan, LC.tools.Text, LC.tools.Pencil, LC.tools.Eraser,
                        LC.tools.Rectangle, LC.tools.Polygon, LC.tools.Ellipse, LC.tools.Line]
            });
            Patient_Document_Image_Annotation.params.lc.saveShape(LC.createShape('Image', { x: 0, y: 0, image: imageObj }));
            $("#frmImage_Annotation").data('serialize', $("#frmImage_Annotation").serialize());
        }, 500);      
    },

    ValidateImage_Annotation: function () {
        $('#pnlImageAnnotation #frmImage_Annotation')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    Folder: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    FileName: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Patient_Document_Image_Annotation.updatePatientDocument();
        });

    },

    SavePatientDocument: function (IsToAddNote) {
        Patient_Document_Image_Annotation.params["IsToAddNote"] = IsToAddNote;
        $('#pnlImageAnnotation #frmImage_Annotation #btnSubmit').click();
    },

    updatePatientDocument: function () {

        console.log(Patient_Document_Image_Annotation.params["IsToAddNote"]);

        var file_base64 = Patient_Document_Image_Annotation.params.lc.getImage().toDataURL();
        var file_temp = file_base64.split("base64,");
        var file_ = file_temp[1];

        var BASE64_MARKER = ';base64,';
        var base64Index = file_base64.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
        var base64 = file_base64.substring(base64Index);

        var self = $("#pnlImageAnnotation #frmImage_Annotation");
        var myJSON = self.getMyJSONByName();
        Patient_Document_Image_Annotation.updatePatientDocument_DBCall(myJSON, base64).done(function (response) {

            if (response.status != false) {

                //utility.DisplayMessages(response.Message, 1);
                if (Patient_Document_Image_Annotation.params["IsToAddNote"] == true) {
                    $("#" + Clinical_Notes.params.PanelID + " #noteatch_i").addClass("fa-paperclip");
                    var docId = Patient_Document_Image_Annotation.params.PatDocID;
                    if (docId) {
                        var $row = $("#" + Patient_Document.params.PanelID + ' #dgvPatientDocument tbody [documentid="' + docId + '"]');
                        $row.find('td:eq(2)').append('&nbsp;<a class="btn  btn-xs" href="#" onclick="" title=""><i class="fa fa-paperclip"></i></a>');
                        $row.find('input:checkbox#chkPatDoc' + docId).prop('checked', true);
                    }
                    Patient_Document_Image_Annotation.CreateHTMLProgressNoteImages(docId)
                }
                else {
                    utility.DisplayMessages(response.Message, 1);
                    var isAttached = Patient_Document_Image_Annotation.params["IsAttached"];
                    if (isAttached) {
                        Clinical_ProgressNote.AttachedNoteComponentIds.push(Patient_Document_Image_Annotation.params.PatDocID);
                        if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0) {
                            $("#" + Patient_Document.params.PanelID + "  #btnAddToNote").prop('disabled', false);
                        } else {
                            $("#" + Patient_Document.params.PanelID + "  #btnAddToNote").prop('disabled', true);
                        }
                    }
                }
                $("#frmImage_Annotation").data('serialize', $("#frmImage_Annotation").serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },

    //Add to Note START

    checkImagesExists: function (NoteDocumentId) {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_images').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="ImagesComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_images title="Images"  id="' + NoteDocumentId + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Images\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Images">Images</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Images\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_images> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_images').parent().parent().removeClass('hidden');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    CreateHTMLProgressNoteImages: function (docId) {
        
        Patient_Document_Image_Annotation.checkImagesExists(docId);

        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        var file_base64 = Patient_Document_Image_Annotation.params.lc.getImage().toDataURL();

        var $image_ = document.createElement("IMG");
        $image_.setAttribute("src", file_base64);
        $image_.setAttribute("width", "175");
        $image_.setAttribute("height", "150");
        $image_.setAttribute("alt", "Note attchment image");

        var $mainDivImages = $(document.createElement('div'));
        var $sectionBodyImages = $(document.createElement('section'));
        var $detailsDiv = $(document.createElement('div'));
        var $listImages = $(document.createElement('ul'));
        $detailsDiv.attr('id', "Cli_Images_" + docId);

        $sectionBodyImages.attr('id', "Cli_Images_Main" + docId);
        $sectionBodyImages.attr('id_', "" + docId + "");
        $sectionBodyImages.css('margin-bottom', "5px");


        $sectionBodyImages.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Images_" + docId + '"><i class="fa fa-edit"></i></a>' +
                   '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Images_Main" + docId + '"  ><i class="fa fa-times"></i></a></div>');

        $listImages.append($image_);

        $detailsDiv.append($listImages);

        $sectionBodyImages.append($detailsDiv);

        $mainDivImages.append($sectionBodyImages);
        $mainDivImages.html()

        $(noteHTMLCtrl + ' clinical_images').parent().parent().addClass('initialVisitBody');
       
        if ($(noteHTMLCtrl).find("#Cli_Images_Main" + docId).length > 0) {
            $(noteHTMLCtrl).find("#Cli_Images_Main" + docId).html($sectionBodyImages.html());
        }
        else {
            $(noteHTMLCtrl + ' clinical_images').parent().parent().append($mainDivImages.html());
        }

        Clinical_ProgressNote.saveComponentSOAPText('Images').done(function () {
            $.when(UnloadActionPan(Patient_Document_Image_Annotation.params["ParentCtrl"], "Patient_Document_Image_Annotation", null, Patient_Document_Image_Annotation.params["ParentCtrlPanelID"])).then(function () {
                utility.callbackAfterAllDOMLoaded(function () {
                    if (Patient_Document_Image_Annotation.params.ParentCtrl == "Patient_Document") {
                        UnloadActionPan(null, 'Patient_Document'); //unloads Patient Document pop-up
                    }
                });
            });
            
        });
    },

    //Add to Note END



    updatePatientDocument_DBCall: function (data_, filebase64) {

        var PatientID = Patient_Document_Image_Annotation.params.PatientID;
        var PatDocID = Patient_Document_Image_Annotation.params.PatDocID;
        var NotesId = Patient_Document_Image_Annotation.params.NotesId;
        var IsToAddNote = Patient_Document_Image_Annotation.params.IsToAddNote;

        var obj = new Object();
        obj["DocumentID"] = PatDocID;
        obj["NotesId"] = NotesId;
        obj["PatientID"] = PatientID;
        obj["IsToAddNote"] = IsToAddNote;
        obj["FileBase64"] = filebase64;

        var strdata = JSON.stringify(obj);

        var data = "PatientDocumentData=" + data_ + "&ObjectData=" + strdata;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT_IMAGE_ANNOTATION", "UPDATE_PATIENT_DOCUMENT_AND_ATTACH_NOTE");

    },

    Reset: function () {

        Patient_Document_Image_Annotation.CreateAnnotateLiterallyEditor(
            Patient_Document_Image_Annotation.params["Res_FileType"],
            Patient_Document_Image_Annotation.params["Res_Base64FileStream"]
            );

    },

    UnLoad: function () {

        utility.UnLoadDialog('frmImage_Annotation', function () {
            UnloadActionPan(Patient_Document_Image_Annotation.params["ParentCtrl"], "Patient_Document_Image_Annotation", null, Patient_Document_Image_Annotation.params["ParentCtrlPanelID"]);
        }, function () {
            UnloadActionPan(Patient_Document_Image_Annotation.params["ParentCtrl"], "Patient_Document_Image_Annotation", null, Patient_Document_Image_Annotation.params["ParentCtrlPanelID"]);

        });
        
    },

};