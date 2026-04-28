Clinical_NotesComponentSelection = {
    bIsFirstLoad: true,
    params: [],
    DocumentIds: [],
    PatEduDocumentIds: [],
    PrintPDFDataURL:'',
    Load: function (params) {
        Clinical_NotesComponentSelection.params = params;
        if (Clinical_NotesComponentSelection.params != null && Clinical_NotesComponentSelection.params.PanelID != "Clinical_NotesComponentSelection") {
            Clinical_NotesComponentSelection.params["PanelID"] = Clinical_NotesComponentSelection.params["PanelID"] + ' #Clinical_NotesComponentSelection';
        }
        else {
            Clinical_NotesComponentSelection.params = [];
            Clinical_NotesComponentSelection.params["PanelID"] = "Clinical_NotesComponentSelection"
        }

        if (Clinical_NotesComponentSelection.bIsFirstLoad) {
            Clinical_NotesComponentSelection.bIsFirstLoad = false
            Clinical_NotesView.SelectedComponent = [];
            if (Clinical_NotesComponentSelection.params.CheckBoxes != null) {
                $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #chkBoxes').append('<div class="col-sm-12 col-md-12 col-lg-12 mb-md"><div class="checkbox-custom"><input type="checkbox" id="chkComponentAll" onclick="Clinical_NotesComponentSelection.AddToListAll(this);" name="Component" checked><label class="control-label"> Select All</label></div>');
                for (var chk = 0; chk < Clinical_NotesComponentSelection.params.CheckBoxes.length; chk++) {

                    if (Clinical_NotesComponentSelection.params["ParentCtrl"] = "Clinical_NotesView") {
                        Clinical_NotesView.SelectedComponent.push(Clinical_NotesComponentSelection.params.CheckBoxes[chk]);
                    }
                    $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #chkBoxes').append('<div class="col-sm-6 col-md-6 col-lg-4 mb-md"><div class="checkbox-custom"><input type="checkbox" id="chkComponent" onclick="Clinical_NotesComponentSelection.AddToList(this);" name="Component" checked><label class="control-label">' + Clinical_NotesComponentSelection.params.CheckBoxes[chk] + '</label></div>');
                }
            }
        }

        Clinical_NotesComponentSelection.hideShowComponentSelection();
    },
    AddToListAll:function(obj)
    {
        Clinical_NotesView.SelectedComponent = [];
        if ($(obj).prop('checked')) {
            $('#Clinical_NotesComponentSelection #chkComponent').prop('checked', true);
            $.each($('#Clinical_NotesComponentSelection #chkComponent'), function (i, item) {
                Clinical_NotesView.SelectedComponent.push($(item).siblings().text());
            });
        } else {
            $('#Clinical_NotesComponentSelection #chkComponent').prop('checked', false);
           
        }
    },
    AddToList: function (chk) {
        if ($('#Clinical_NotesComponentSelection #chkComponent:checked').length == $('#Clinical_NotesComponentSelection #chkComponent').length) {
            $('#Clinical_NotesComponentSelection #chkComponentAll').prop('checked', true);
        } else {
            $('#Clinical_NotesComponentSelection #chkComponentAll').prop('checked', false);
        }
        if (Clinical_NotesComponentSelection.params["ParentCtrl"] == "Clinical_NotesView") {
            var ComponentName = $(chk).parent().text();
            if ($(chk).prop("checked") == true) {
                if (Clinical_NotesView.SelectedComponent.indexOf(ComponentName) == -1) {
                    Clinical_NotesView.SelectedComponent.push(ComponentName);
                }
            }
            else {
                var charIndex = Clinical_NotesView.SelectedComponent.indexOf(ComponentName)
                if (charIndex > -1) {
                    Clinical_NotesView.SelectedComponent.splice(charIndex, 1);
                }
            }
        }
    },

    UnLoad: function () {
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_NotesComponentSelection.params.ParentCtrl == "clinicalTabFaceSheet") {
            if (Clinical_NotesComponentSelection.params["FromAdmin"] == "0") {
                if (Clinical_NotesComponentSelection.params != null && Clinical_NotesComponentSelection.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_NotesComponentSelection.params.ParentCtrl, 'Clinical_NotesComponentSelection');
                }
                else
                    UnloadActionPan(null, 'Clinical_NotesComponentSelection');
            }
            else {
                RemoveAdminTab();
            }
            objDeffered.resolve();
            Clinical_FaceSheet.loadFaceSheet();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else {
            if (Clinical_NotesComponentSelection.params != null && Clinical_NotesComponentSelection.params.ParentCtrl) {
                UnloadActionPan(Clinical_NotesComponentSelection.params.ParentCtrl);
                // Clinical_NotesComponentSelection.params = null;
            }
            else {
                UnloadActionPan();
            }
            objDeffered.resolve();
        }

        Clinical_NotesComponentSelection.PrintComponent();
        return objDeffered;
    },

    PrintComponent: function () {
        var printType = globalAppdata.NotePrevieStyle;
        Clinical_NotesView.params["PrintType"] = printType;

        Clinical_NotesView.ExcludeImageIds = Clinical_NotesComponentSelection.getImagesNotToPrint();

        var saveDiagnosticResult = false;
        if (Clinical_NotesView.SelectedComponent && Clinical_NotesView.SelectedComponent.length == 1) {
            var component = $.trim(Clinical_NotesView.SelectedComponent[0]);
            if (component.indexOf("Diagnostic Imaging Results") != -1) {
                saveDiagnosticResult = true;
            }
            else {
                saveDiagnosticResult = false;
            }
        }

        if (printType && printType == 'Classic Style' && Clinical_NotesView.params["IsPhoneEncounter"] != true) {
            Clinical_NotesComponentSelection.LoadLegacyComponents(saveDiagnosticResult).done(function (response) {
                response = JSON.parse(response);

                utility.PDFViewer(response.Message, true, null, null, true);

                $("#printcall").hide();
                if (Clinical_NotesComponentSelection.params["NoteStatus"] != "Signed") {
                    $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #noteButtonCoSign").attr('disabled', true);
                    $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #noteButtonAmendment").attr('disabled', true);
                }
                else {
                    $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #noteButtonCoSign").attr('disabled', false);
                    $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #noteButtonAmendment").attr('disabled', false);
                }
                Clinical_NotesView.SelectedComponent = null;
                if (saveDiagnosticResult) {
                    Clinical_NotesView.SaveDiagnosticResultInPatDocs(response.Message);
                }
            });
        }
        else {
            $('#' + Clinical_NotesView.params["PanelID"] + " #printcallClone").html($('#' + Clinical_NotesView.params["PanelID"] + " #printcall")[0].innerHTML);
            if (Clinical_NotesView.SelectedComponent && Clinical_NotesView.SelectedComponent.length == 1) {
                $('#' + Clinical_NotesView.params["PanelID"] + ' #printcallClone #ulContent').find(".CustomComponent").remove();
            }
            $('#' + Clinical_NotesView.params["PanelID"] + ' #printcallClone #ulContent').find('li.initialVisitBody').each(function () {
                if (Clinical_NotesView.SelectedComponent != null)

                    if ($(this).hasClass("CustomFormsComponent") && Clinical_NotesView.SelectedComponent.indexOf("Custom Forms")==-1) {
                        $(this).remove();
                    }
                else{
                    if (!$(this).hasClass("CustomFormsComponent")) {
                        if (Clinical_NotesView.SelectedComponent.indexOf($(this).find('header:first').text()) == -1) {
                            if ($(this).find('header').text().trim() == 'Images') {
                                if (Clinical_NotesView.ExcludeImageIds && Clinical_NotesView.ExcludeImageIds.length > 0) {
                                    for (var i = 0; i < Clinical_NotesView.ExcludeImageIds.length; i++) {
                                        var image = $(this).find('#Cli_Images_Main' + Clinical_NotesView.ExcludeImageIds[i]);
                                        if (image) {
                                            $(image).remove();
                                        }
                                    }
                                }
                            }
                            else {
                                $(this).remove();
                            }
                        }
                    }
                  }

            });
            if (saveDiagnosticResult) {
                var userName = globalAppdata["AppUserLastName"] + ', ' + globalAppdata["AppUserFirstName"];
                var signeDateTime = moment().format("dddd, LL") + ' at ' + moment().format("h:mm:ss a");
                $('#' + Clinical_NotesView.params["PanelID"] + " #printcallClone").append('<p>e-Signed By: ' + userName + ' on ' + signeDateTime + '</p>');
            }
            Clinical_NotesView.printNote(saveDiagnosticResult);
        }

        Clinical_NotesComponentSelection.printDocuments();
        //Clinical_NotesView.NotesPreview(Clinical_NotesView.params.NotesId, Clinical_NotesView.params.PatientId, Clinical_NotesView.params.ProviderId)
    },

    LoadLegacyComponents: function (IsSaveDiagnosticResult) {
        var noteID = Clinical_NotesComponentSelection.params["NotesId"];
        var patientID = Clinical_NotesComponentSelection.params["PatientId"];
        var providerId = Clinical_NotesComponentSelection.params["ProviderId"];
        var objNotesComponentViewModel = new Object();
        objNotesComponentViewModel.NotesComponents = new Array();
        $.each(Clinical_NotesView.SelectedComponent, function (index, value) {
            var obj = new Object();
            obj.NotesId = noteID;
            obj.PatientId = patientID;
            obj.ProviderId = providerId;
            obj.Component = value.trim().split(" ").join("");
            objNotesComponentViewModel.NotesComponents.push(obj);
        });
        var obj = new Object();
        obj.NotesId = noteID;
        obj.PatientId = patientID;
        obj.ProviderId = providerId;
        obj.Component = "NotesComponent";
        objNotesComponentViewModel.NotesComponents.push(obj);
        objNotesComponentViewModel.ExcludedImages = new Array();
        objNotesComponentViewModel.ExcludedImages = Clinical_NotesComponentSelection.getImagesNotToPrint();
        objNotesComponentViewModel.IsSaveDiagnosticResult = IsSaveDiagnosticResult;
        return MDVisionService.APIServiceComplex(objNotesComponentViewModel, "ClinicalNotes", "LoadLegacyNoteAndRenderTemplate");
    },

    hideShowComponentSelection: function () {

        if (globalAppdata["IsSelectNoteComponent"] == "True") {
            if (Clinical_NotesComponentSelection.params.CheckBoxes.length > 0) {
                $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #pHeading').removeClass('hidden');
                $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #chkBoxes').removeClass('hidden');
            }
            else {
                $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #pHeading').addClass('hidden');
                $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #chkBoxes').addClass('hidden');
            }
        }
        else {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #pHeading').addClass('hidden');
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #chkBoxes').addClass('hidden');
        }

        Clinical_NotesComponentSelection.hideShowAttachments();
    },

    hideShowAttachments: function () {
        if (Clinical_NotesComponentSelection.params.DocAttached == true) {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #divHeading').removeClass('hidden');
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv').removeClass('hidden');
            Clinical_NotesComponentSelection.showAssociatedAttachmentsOfNote();
        }
        else {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #divHeading').addClass('hidden');
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv').addClass('hidden');
        }
    },
    showAssociatedAttachmentsOfNote: function () {
        Clinical_NotesComponentSelection.getAssociatedAttachmentsOfNote_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.AttachedDocsCount > 0) {
                    $(response.AttachedDocs).each(function (ind, item) { item.checked = true; });
                    Clinical_NotesComponentSelection.createAssocialtedAttachmentsSections(response.AttachedDocs);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    getAssociatedAttachmentsOfNote_DBCall: function () {
        var objData = {};
        objData["NotesId"] = Clinical_NotesComponentSelection.params["NotesId"];
        objData["commandType"] = "getAssociatedAttachmentsOfNote";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "ClinicalNotes");
    },

    createAssocialtedAttachmentsSections: function (attachments) {
        var images = $.grep(attachments, function (item, index) {
            return item.Type == "Med" && item.FileType.indexOf('image') >= 0;
        });

        var documnents = $.grep(attachments, function (item, index) {
            return item.Type == "Med" && (item.FileType.indexOf('pdf') >= 0 || item.FileType.indexOf('html') >= 0);
        });

        var patEdu = $.grep(attachments, function (item, index) {
            return item.Type == "Edu";
        });

        var letters = $.grep(attachments, function (item, index) {
            return item.Type == "Let";
        });

        Clinical_NotesComponentSelection.initializeTreeViews(images, documnents, patEdu, letters);
    },

    initializeTreeViews: function (images, documnents, patEdu, letters) {
        if (images && images.length > 0) {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #imagesTree').kendoTreeView({
                checkboxes: {
                    checkChildren: true,
                    template:
                      "# if (item.level() > 0) { #" +
                          '<div class="checkbox-custom"><input type="checkbox" checked="checked"><label> &nbsp;</label></div>' +
                      "# } #"
                },

                dataSource: [{
                    DocumentName: "Images",
                    items: images
                }],
                dataTextField: "DocumentName",
                dataIdField: "PatDocId",
                loadOnDemand: false
            }).data("kendoTreeView");

            if (images.length == 1) {
                var imgTree = $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #imagesTree');
                $(imgTree).find('li:first > div:first').remove();
                $(imgTree).find('li:first > ul').css('display', 'block');
            }
        }
        else {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #imagesTree').remove();
        }

        if (documnents && documnents.length > 0) {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #documentsTree').kendoTreeView({
                checkboxes: {
                    checkChildren: true,
                    template:
                       "# if (item.level() > 0) { #" +
                          '<div class="checkbox-custom"><input type="checkbox" checked="checked"><label> &nbsp;</label></div>' +
                      "# } #"
                },
                dataSource: [{
                    DocumentName: "Documents",
                    items: documnents
                }],
                dataTextField: "DocumentName",
                dataIdField: "PatDocId",
                loadOnDemand: false
            }).data("kendoTreeView");

            if (documnents.length == 1) {
                var docsTree = $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #documentsTree');
                $(docsTree).find('li:first > div:first').remove();
                $(docsTree).find('li:first > ul').css('display', 'block');
            }
        }
        else {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #documentsTree').remove();
        }

        if (patEdu && patEdu.length > 0) {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #patientEducationTree').kendoTreeView({
                checkboxes: {
                    checkChildren: true,
                    template:
                      "# if (item.level() > 0) { #" +
                          '<div class="checkbox-custom"><input type="checkbox" checked="checked"><label> &nbsp;</label></div>' +
                      "# } #"
                },
                dataSource: [{
                    DocumentName: "Patient Education",
                    items: patEdu
                }],
                dataTextField: "DocumentName",
                dataIdField: "PatDocId",
                loadOnDemand: false
            }).data("kendoTreeView");

            if (patEdu.length == 1) {
                var eduTree = $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #patientEducationTree');
                $(eduTree).find('li:first > div:first').remove();
                $(eduTree).find('li:first > ul').css('display', 'block');
            }
        }
        else {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #patientEducationTree').remove();
        }

        if (letters && letters.length > 0) {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #lettersTree').kendoTreeView({
                checkboxes: {
                    checkChildren: true,
                    template:
                      "# if (item.level() > 0) { #" +
                          '<div class="checkbox-custom"><input type="checkbox" checked="checked"><label> &nbsp;</label></div>' +
                      "# } #"
                },
                dataSource: [{
                    DocumentName: "Letters",
                    items: letters
                }],
                dataTextField: "DocumentName",
                dataIdField: "PatDocId",
                loadOnDemand: false
            }).data("kendoTreeView");

            if (letters.length == 1) {
                var ltrTree = $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #lettersTree');
                $(ltrTree).find('li:first > div:first').remove();
                $(ltrTree).find('li:first > ul').css('display', 'block');
            }
        }
        else {
            $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #lettersTree').remove();
        }
    },

    getImagesNotToPrint: function () {
        var checkedNodes = [],
            treeView = $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #imagesTree').data("kendoTreeView");
        if (treeView) {
            var nodes = treeView.dataSource.view()[0].items;

            for (var i = 0; i < nodes.length; i++) {
                if (!nodes[i].checked) {
                    checkedNodes.push(nodes[i].PatDocId);
                }
            }
        }

        return checkedNodes;
    },
    printDocuments: function () {

        var treeView = $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #documentsTree').data("kendoTreeView");
        if (treeView) {
            var nodes = treeView.dataSource.view()[0].items;

            for (var i = 0; i < nodes.length; i++) {
                if (nodes[i].checked) {
                    Clinical_NotesComponentSelection.DocumentIds.push(nodes[i].PatDocId);
                }
            }
        }

        var treeViewEdu = $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #patientEducationTree').data("kendoTreeView");
        if (treeViewEdu) {
            var nodesEdu = treeViewEdu.dataSource.view()[0].items;

            for (var i = 0; i < nodesEdu.length; i++) {
                if (nodesEdu[i].checked) {
                    Clinical_NotesComponentSelection.DocumentIds.push(nodesEdu[i].PatDocId);
                }
            }
        }

        var treeViewLtr = $('#' + Clinical_NotesComponentSelection.params["PanelID"] + ' #associatedAttachmentsDiv #lettersTree').data("kendoTreeView");
        if (treeViewLtr) {
            var nodesLtr = treeViewLtr.dataSource.view()[0].items;

            for (var i = 0; i < nodesLtr.length; i++) {
                if (nodesLtr[i].checked) {
                    Clinical_NotesComponentSelection.DocumentIds.push(nodesLtr[i].PatDocId);
                }
            }
        }

        if (Clinical_NotesComponentSelection.DocumentIds && Clinical_NotesComponentSelection.DocumentIds.length > 0) {
            var documentCall = Patient_Document.FillDocumentsMerged(null, Clinical_ProgressNote.params.patientID, Clinical_NotesComponentSelection.DocumentIds.join(','));
            $.when(documentCall).done(function (response) {
                Clinical_NotesComponentSelection.DocumentIds = [];

                if (response.status != false) {
                    utility.documentPrint(response.MergedContent);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
}
