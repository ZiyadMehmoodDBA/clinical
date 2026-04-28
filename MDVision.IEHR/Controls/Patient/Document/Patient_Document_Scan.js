Document_Scan = {
    bIsFirstLoad: true,
    params: [],
    counter: 1,
    AttachedDocsArray: [],
    PasswordJSON: "",
    IsValidationRequired: false,
    Load: function (params) {
        $("#dwtcontrolContainerAttach").addClass("hide");
        Document_Scan.AttachedDocsArray = [];
        Document_Scan.params = params;

        if (Document_Scan.params["IsFromUploadImage"] && Document_Scan.params["IsFromUploadImage"].toLowerCase() == "true") {
            Document_Scan.params["IsFromUploadImage"] = true;
        }
        else {
            Document_Scan.params["IsFromUploadImage"] = false;
        }

        var self = $('#' + params.PanelID + ' #pnlDocumentScan');
        self.loadDropDowns(true).done(function () {
            $("#" + Document_Scan.params["PanelID"]).find('#chkSelectAllScnImgs').prop('checked', true);
            if (Document_Scan.params.RefCtrl && Document_Scan.params.RefCtrl == "chargeBatchDetail") {
                $('#' + Document_Scan.params.PanelID + ' #ddlFolder').html("<option value='1' selected>Encounters</option>");
                $('#' + Document_Scan.params.PanelID + ' #ddlFolder').attr("disabled", true);
                $('#' + Document_Scan.params.PanelID + ' #ddlFolder').css("pointer-events", "none");
            } else if (Document_Scan.params.RefCtrl && Document_Scan.params.RefCtrl == "paymentBatchDetail") {
                //$('#' + Document_Scan.params.PanelID + ' #ddlFolder').html("<option value='1' selected>Check</option>");
                //$('#' + Document_Scan.params.PanelID + ' #ddlFolder').attr("disabled", true);
                //$('#' + Document_Scan.params.PanelID + ' #ddlFolder').css("pointer-events", "none");
            }

            // Priority
            $("#" + Document_Scan.params.PanelID + " #frmFilters #ddlDocumentPriority").html("");
            CacheManager.BindCodes('GetDocumentPriority', false).done(function (result) {
                var priorities = JSON.parse(result.GetDocumentPriority)
                $.each(priorities, function (k, obj) {
                    var color = "";
                    if (obj.Name.toLowerCase().trim() == "low")
                        color = "green bold";
                    else if (obj.Name.toLowerCase().trim() == "medium")
                        color = "dark-yellow bold";
                    else if (obj.Name.toLowerCase().trim() == "high")
                        color = "red bold";
                    $("#" + Document_Scan.params.PanelID + " #frmFilters #ddlDocumentPriority").append("<option value='" + obj.Value + "' class='" + color + "' " + ">" + obj.Name + "</option>");
                });

                // Set Default Value of Document Prority
                if (globalAppdata.DefaultDocumentPriorityId != "") {
                    $("#" + Document_Scan.params["PanelID"] + " #frmFilters #ddlDocumentPriority").val(globalAppdata.DefaultDocumentPriorityId);
                    $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #ddlDocumentPriority").val(globalAppdata.DefaultDocumentPriorityId);
                }

            });


            Document_Scan.IntializeMultiSelectDropDownFolder();
            Document_Scan.LoadPatientVisitDOS();
            //initialization of date-picker.
            utility.CreateDatePicker(Document_Scan.params["PanelID"] + ' #dtpExpirtyDate', function () {
                //on-change callback method
            });
            utility.CreateDatePicker(Document_Scan.params["PanelID"] + ' #dtpDOS', function () {
                //on-change callback method
                $(Document_Scan.params["PanelID"] + ' #frmDocumentScan').bootstrapValidator('revalidateField', 'DOS');
            });
            Document_Scan.PasswordJSON = "";
        });
        // var dfd = new $.Deferred();
        self.find("#dwtcontrolContainerAttach").addClass("hide");
        pageonload();
        Document_Scan.ValidateScanner(Document_Scan.params.PanelID + ' #frmDocumentScan');
        //if (Document_Scan.params.RefCtrl == "chargeBatchDetail" || Document_Scan.params.RefCtrl == "paymentBatchDetail") {
        //    Document_Scan.CustomSettings();

        //    //  Dynamsoft.WebTwainEnv.RegisterEvent('OnWebTwainReady', function () {

        //    //     DWObject.RegisterEvent('OnPostTransfer', function () {
        //    //alert(DWObject.CurrentImageIndexInBuffer);
        //    //  });
        //    //     alert('Scanner is ready to use');
        //    //  });

        //}
        //else
        if (Document_Scan.params.RefCtrl == "patTabInsurance") {
            Document_Scan.InsuranceCustomSettings();
            //AST-3 Start
            $("#" + Document_Scan.params.PanelID + " #div_controls").addClass("hidden");
            //AST-3 END
        }
        else if (Document_Scan.params.RefCtrl == "patientDemographic") {
            Document_Scan.DemographicCustomSettings();
        }
        else if (Document_Scan.params.RefCtrl == "imageupload") {
            Document_Scan.UploadimageCustomSettings();
        }
        if (Document_Scan.params["PanelID"] == 'pnlPatientDocument' || Document_Scan.params["PanelID"] == 'pnlBatchDocuments') {
            if (localStorage.SelectedAccountNumber) {
                var $ctr = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan');
                var $hfctr = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId');
                // set value from Patient->document if patient is selected (patient->document)  field.
                if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullNameScan').val()) {
                    $ctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #txtFullNameScan').val());
                    $hfctr.val($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val());
                }
                else {
                    $ctr.val($('#PatientProfile #hfPatientFullNameOnly').val());
                    $hfctr.val(localStorage.SelectedPatientId);

                    // Load folders if user empty Patient->document fiedl.
                    if ($('#' + Document_Scan.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val() != localStorage.SelectedPatientId) {
                        Patient_Document.LoadFolders(true);
                    }
                }



            }
        }
        //AST-148 by:MAHMAD
        if (Document_Scan.params["PanelID"] == "pnldemographicDetail" || Document_Scan.params["PanelID"] == "pnlDemographicQuick") {
            $("#" + Document_Scan.params["PanelID"] + ' #btnScan').hide();
        }
        
        //AST-148 by:MAHMAD
        if (Document_Scan.params["IsFromIfram"] == true) {
            $("#" + Document_Scan.params["PanelID"] + " #doc_model").removeClass("modal-dialog");
            $("#" + Document_Scan.params["PanelID"] + " #doc_header").addClass("hidden");
            $("#" + Document_Scan.params["PanelID"] + " #actionBtn").addClass("hidden");
            $("#doc_model .modal-content").css({
                'box-shadow': 'none',
                'border': 'none',
                'border-radius': '0px'
            });
            if (Document_Scan.params["IsFromUploadImage"]== true) {
                $("#" + Document_Scan.params["PanelID"] + " #divCustomScan").addClass("hidden");
                $("#" + Document_Scan.params["PanelID"] + " #divCustomScanAttach").addClass("hidden");
                $("#" + Document_Scan.params["PanelID"] + " #ScanWrapper #lstFileName").addClass("hidden");
                $("#" + Document_Scan.params["PanelID"] + " #ScanWrapper #btnScanProcess").css("display", "block");
                $("#" + Document_Scan.params["PanelID"] + " #ScanWrapper #btnScan").css("display", "block");
                $("#" + Document_Scan.params["PanelID"] + " #ScanWrapper #Duplex").click();
            }
        }

        Document_Scan.BindTagAutocomplete();
        Document_Scan.BindPatientNameAutocomplete();
        Document_Scan.SetDocumentScanner();
        Document_Scan.SetCollapseExpandPanelPatientDocument();
        Document_Scan.OnLoadResources();

    },

    OnLoadResources: function () {

        //offCanvas
        $('#' + Document_Scan.params.PanelID + " #menu-toggle").click(function (e) {
            e.preventDefault();

            Document_Scan.SetParamPanalID();

            if ($('#' + Document_Scan.params.PanelID + ' #asideBtn').hasClass('asideBtn'))
                $('#' + Document_Scan.params.PanelID + ' #asideBtn').removeClass('asideBtn').addClass('asideBtn-toggled');
            else if ($('#' + Document_Scan.params.PanelID + ' #asideBtn').hasClass('asideBtn-toggled'))
                $('#' + Document_Scan.params.PanelID + ' #asideBtn').removeClass('asideBtn-toggled').addClass('asideBtn');

            $('#' + Document_Scan.params.PanelID + " #wrapper").toggleClass("toggled");
        });

        //DWTcontainer
        $("#" + Document_Scan.params["PanelID"] + " #DWTcontainer").hover(function () {
            $(document).bind('mousewheel DOMMouseScroll', function (event) {
                stopWheel(event);
            });
        }, function () {
            $(document).unbind('mousewheel DOMMouseScroll');
        });
        $("#" + Document_Scan.params["PanelID"] + " ul.PCollapse li>div").click(function () {
            if ($(this).next().css("display") == "none") {
                $(".divType").next().hide("normal");
                $(".divType").children(".mark_arrow").removeClass("expanded");
                $(".divType").children(".mark_arrow").addClass("collapsed");
                $(this).next().show("normal");
                $(this).children(".mark_arrow").removeClass("collapsed");
                $(this).children(".mark_arrow").addClass("expanded");
            }
        });

        $(function () {
            $('[data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};
                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;
                $this.themePluginToggle(opts);
            });
        });

    },

    SetCollapseExpandPanelPatientDocument: function () {

        //1- Initialization
        $('#' + Document_Scan.params.PanelID + ' .splitterBtn').html('<a class="btnspliter"></a>');
        EMRUtility.changeIcon($('#' + Document_Scan.params.PanelID + ' .splitterBtn a'));

        $('#' + Document_Scan.params.PanelID + ' .splitterBtn a').click(function (e) {
            $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
            var a = $(this);
            EMRUtility.changeIcon(a);
        });

        //2- Set Default
        $('#' + Document_Scan.params.PanelID + ' #splitterScanbody').removeClass('splitterBody active');
        $('#' + Document_Scan.params.PanelID + ' #splitterScanbody').hide();


        utility.CreateDatePicker(Document_Scan.params["PanelID"] + " #frmFilters #dtpDcanDOS", function () {
        });
        utility.CreateDatePicker(Document_Scan.params["PanelID"] + " #frmFilters #dtpReceivedOn", function () {
        });
        utility.CreateDatePicker(Document_Scan.params["PanelID"] + " #frmFilters #dtpScanExpiry", function () {
        });

    },

    SetDocumentScanner: function () {
        $("#" + Document_Scan.params["PanelID"] + " #divCustomScanAttach").removeClass("hide");
        //$("#DW_btnRemoveAllImages").addClass("pull-left");
        $("#" + Document_Scan.params["PanelID"] + " #DW_btnRemoveAllImages").parent().prepend("<button type='button' onclick='Document_Scan.btnExportScanDocumen_onclick()' id='btnExportScanDocument' class='btn btn-link btn-xs'><i class='glyphicon glyphicon-import fa-flip-vertical text-default'></i> Export</button>");
        $("#" + Document_Scan.params["PanelID"] + " #DW_btnRemoveAllImages").parent().prepend("<button type='button' onclick='return btnLoad_onclick();' id='btnImportScanDocument' class='btn btn-link btn-xs'><i class='glyphicon glyphicon-import text-success'></i> Import</button>");
        $("#" + Document_Scan.params["PanelID"] + " #DW_btnRemoveAllImages").parent().prepend("<button type='button' onclick='Document_Scan.btnViewScanDocumen_onclick()' id='btnViewScanDocument' class='btn btn-link btn-xs'><i class='fa fa-eye text-warning'></i>  View</button>");

        if (Document_Scan.params["TabID"] == "patTabDocuments" || Document_Scan.params["TabID"] == "batchTabDocuments" || Document_Scan.params["TabID"] == "chargeBatchDetail" || Document_Scan.params["TabID"] == "patTabDemographic") {
            $("#" + Document_Scan.params["PanelID"] + " #divcomments").addClass("col-xs-12");
            //$("#" + Document_Scan.params["PanelID"] + " #divbtn").addClass("col-xs-2");
            $("#" + Document_Scan.params["PanelID"] + " #btnDocumentScan").html("Add");
            if (Document_Scan.params["TabID"] == "chargeBatchDetail" || Document_Scan.params["TabID"] == "patTabDemographic") {
                $('#' + Document_Scan.params["PanelID"] + ' #btnDocumentScanExit').removeClass('hidden');
            }
        }
        else {
            $("#" + Document_Scan.params["PanelID"] + " #divcomments").addClass("col-xs-8");
            $("#" + Document_Scan.params["PanelID"] + " #divbtn").addClass("pull-right");
            $("#" + Document_Scan.params["PanelID"] + " #btnDocumentScan").html("Save & Exit");
        }
    },

    btnExportScanDocumen_onclick: function () {

        var actionpan = $("#" + Document_Scan.params["PanelID"] + " #actionPanSelectType");
        if (actionpan.length > 0) {
            $(actionpan).html($('#divImageTypes').html());
            $(actionpan).modal({
                show: 'true',
                backdrop: 'static',
                keyboard: false

            }).on('shown.bs.modal', function () {


                var _chkMultiPagePDF_save = document.getElementById("MultiPagePDF_save");
                _chkMultiPagePDF_save.checked = false;
                _chkMultiPagePDF_save.disabled = true;

            }).on('hidden.bs.modal', function () {


            });
        }


        // Show Types in Popup
        // As Save Image button

        //btnSave_onclick();  return false; // on Save Button Click
        //rdPDFsave_onclick(); // on PDF RadioButton Click
        //rdTIFFsave_onclick(); // on TIFF RadioButton Click
        //rdsave_onclick(); // on rest of all RadioButton's Click

    },

    UnLoadImageType: function () {
        var actionpan = $("#" + Document_Scan.params["PanelID"] + " #actionPanSelectType");
        if (actionpan.length > 0) {
            $(actionpan).modal('hide');
            $(actionpan).css('display', "none");
            $(actionpan).find('div').first().remove();
            $("body").removeClass("modal-open");
        }
    },

    btnViewScanDocumen_onclick: function () {


        if (Document_Scan.AttachedDocsArray.length > 0) {
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #dwtcontrolContainer").parent().removeClass("hide");
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #dwtcontrolContainerAttach").addClass("hide");
        }
        else if ($("#" + Document_Scan.params["PanelID"] + " #PreviewContainer #DW_PreviewMode").val() != "0") {
            $("#" + Document_Scan.params["PanelID"] + " #PreviewContainer #DW_PreviewMode").val("0");
            $("#" + Document_Scan.params["PanelID"] + " #PreviewContainer #DW_PreviewMode").trigger("change");
        }
        customViewMode();
    },

    IntializeMultiSelectDropDownFolder: function () {
        $('#' + Document_Scan.params.PanelID + ' #ddlFolder').multiselect('destroy');
        $('#' + Document_Scan.params.PanelID + ' #ddlFolder').multiselect({
            //includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            onChange: function (option, checked) {
                var options = $(option).parent().find('option');
                var Selectedoptions = $(option).parent().find('option:selected');
                if (option.length > 0) {
                    var optionText = $(option)[0].outerText;
                    var optionVal = $($(option)[0]).val();
                    if (checked) {
                        $('#' + Document_Scan.params.PanelID + " #ddlFolder").multiselect('refresh');
                    }
                    else {
                        options.each(function () {
                            var input = $('#' + Document_Scan.params.PanelID + ' #ddlFolder input[type=checkbox][value="' + $(this).val() + '"]');
                            input.prop('disabled', false);
                        });
                    }
                }
                //var value = $(option).val();
            },
        });
        $('#' + Document_Scan.params.PanelID + " #ddlFolder").val("");
        Document_Scan.validateFolder(3);
    },

    checkFolderSelection: function () {
        var self = $("#" + Document_Scan.params["PanelID"]);
        var FolderIds = self.find('#divFolder ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            return this.value;
        }).get().join(',');
        if (FolderIds != "" && FolderIds != ",") {

            if ($('#' + Document_Scan.params.PanelID + ' #ddlFolder option:selected').length <= 0)
                $('#' + Document_Scan.params.PanelID + " #ddlFolder").val(FolderIds.split(","));

            Document_Scan.validateFolder(2);
        } else {
            Document_Scan.validateFolder(1);
        }


    },

    saveScanDocument: function () {

        var imgData = null;
        var fileType = null;
        if (!checkIfImagesInBuffer()) {
            utility.DisplayMessages("There is no scanned document available", 3);
            return;
        }
        var i, strimgType_save;
        var NM_imgType_save = document.getElementsByName("imgType_save");
        for (i = 0; i < 5; i++) {
            if (NM_imgType_save.item(i).checked == true) {
                strimgType_save = NM_imgType_save.item(i).value;
                break;
            }
        }
        DWObject.IfShowFileDialog = true;
        var _txtFileNameforSave = document.getElementById("txt_fileNameforSave");
        var bSave = false;

        var _chkMultiPageTIFF_save = document.getElementById("MultiPageTIFF_save");
        var vAsyn = false;
        if (strimgType_save == "tif" && _chkMultiPageTIFF_save.checked) {
            vAsyn = true;
            DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
            //for (var i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
            //    DWObject.SetSelectedImageIndex(i, i);
            //}
            var size = DWObject.GetSelectedImagesSize(2); // Calculate the size of selected images in pdf format

            //imgData = "data:image/tiff;base64," + DWObject.SaveSelectedImagesToBase64Binary();
            imgData = DWObject.SaveSelectedImagesToBase64Binary();
            fileType = "image/tiff";
            var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
            var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
            Document_Scan.DocumentSave(imgData, fileType, FileName);
            Document_Scan.counter++;
        }
        else if (strimgType_save == "pdf" && document.getElementById("MultiPagePDF_save").checked) {
            vAsyn = true;
            DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
            //for (var i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
            //    DWObject.SetSelectedImageIndex(i, i);
            //}
            var size = DWObject.GetSelectedImagesSize(4); // Calculate the size of selected images in pdf formatixes related ot 

            //imgData = "data:application/pdf;base64," + DWObject.SaveSelectedImagesToBase64Binary();
            imgData = DWObject.SaveSelectedImagesToBase64Binary();
            fileType = "application/pdf";
            if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "chargeBatchDetail") {
                var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                chargeBatchDetail.GetScannedDocuments(imgData, fileType, FileName);
                Document_Scan.counter++;
            }
            else if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "paymentBatchDetail") {
                var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                paymentBatchDetail.GetScannedDocuments(imgData, fileType, FileName + ".pdf");
                Document_Scan.counter++;
            }
            else {
                var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                Document_Scan.DocumentSave(imgData, fileType, FileName + ".pdf");
                Document_Scan.counter++;
            }

        }
        else {
            for (var imgIndex = 0; imgIndex < DWObject._HowManyImagesInBuffer; imgIndex++) {
                //DWObject.__OnRefreshUI(imgIndex)
                DWObject.CurrentImageIndexInBuffer = imgIndex;
                //updatePageInfo();
                DWObject.SelectedImagesCount = 1;
                DWObject.SetSelectedImageIndex(0, DWObject.CurrentImageIndexInBuffer);
                switch (i) {
                    case 0:
                        // bitmap
                        var size = DWObject.GetSelectedImagesSize(0); // Calculate the size of selected images in BMP format
                        //imgData = "data:image/bmp;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "image/bmp";

                        break;
                    case 1:
                        //jpeg
                        var size = DWObject.GetSelectedImagesSize(1); // Calculate the size of selected images in JPG format
                        //imgData = "data:image/jpg;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "image/jpg";
                        break;
                    case 2:
                        //Tiff
                        var size = DWObject.GetSelectedImagesSize(2); // Calculate the size of selected images in Tiff format
                        //imgData = "data:image/tiff;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "image/tiff";
                        break;
                    case 3:
                        //PNG
                        var size = DWObject.GetSelectedImagesSize(3); // Calculate the size of selected images in PNG format
                        //imgData = "data:image/png;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "image/png";
                        break;
                    case 4:
                        //pdf
                        var size = DWObject.GetSelectedImagesSize(4); // Calculate the size of selected images in PDF format
                        //imgData = "data:application/pdf;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "application/pdf";
                        break;


                }
                if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "chargeBatchDetail") {
                    var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                    var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                    chargeBatchDetail.GetScannedDocuments(imgData, fileType, FileName + "." + strimgType_save);
                    Document_Scan.counter++;

                }
                else if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "paymentBatchDetail") {
                    var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                    var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                    paymentBatchDetail.GetScannedDocuments(imgData, fileType, FileName + "." + strimgType_save);
                    Document_Scan.counter++;
                }
                else {
                    var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                    var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                    Document_Scan.DocumentSave(imgData, fileType, FileName + "." + strimgType_save);
                    Document_Scan.counter++;
                }
            }
        }

        if (vAsyn == false) {
            if (bSave)
                appendMessage('<b>Save Image: </b>');
            if (checkErrorString()) {
                return;
            }
        }
    },

    OpenPatientCharges: function (name) {
        if (name == "ddlClaim")
            Document_Scan.params["ClaimControlName"] = name;
        else if (name == "txtClaimNumber")
            Document_Scan.params["ClaimControlName"] = name;


        utility.OpenPatientCharges('Document_Scan', true);
    },

    FillVisitFromSearch: function (accountnumber, patientid, claimnumber, visitid) {

        if (Document_Scan.params["ClaimControlName"] == "ddlClaim") {
            $("#" + Document_Scan.params["PanelID"] + "  #frmDocumentScan #ddlClaim").val(claimnumber);
            $("#" + Document_Scan.params["PanelID"] + "  #frmDocumentScan #hfVisitId").val(visitid);
            $("#" + Document_Scan.params["PanelID"] + "  #frmDocumentScan #txtAccountNumber").val(accountnumber);
        }
        else if (Document_Scan.params["ClaimControlName"] == "txtClaimNumber") {
            $("#" + Document_Scan.params["PanelID"] + " #frmFilters #txtClaimNumber").val(claimnumber);
            $("#" + Document_Scan.params["PanelID"] + " #frmFilters #hfPatientVisitId").val(visitid);

        }

        Document_Scan.params["patientID"] = patientid;
        Bill_ChargeSearch.UnLoad();

    },

    DocumentSave: function (strFiles, strFileType, fileName) {
        var data = new FormData();
        var objDef = $.Deferred();
        if (isFileCompressed) {
            var zip = new JSZip();
            zip.file(fileName, strFiles, {
                base64: true
            });
            zip.generateAsync({
                type: "blob", compression: "DEFLATE", compressionOptions: {
                    level: 9
                }
            }).then(function (blob) {
                data.append(0, blob);
                objDef.resolve("ok")
            });
        } else {
            objDef.resolve("ok")
        }
        objDef.then(function () {
            var self = $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan");
            data.append("scanFile", strFiles);
            data.append("PatientID", self.find("#hfPatientId").val() ? self.find("#hfPatientId").val() : Document_Scan.params.patientID);
            data.append("fileType", strFileType);
            data.append("FileName", fileName);
            if (Document_Scan.AttachedDocsArray && Document_Scan.AttachedDocsArray.length > 0) {
                //data.append("AttachedDocs", Array.prototype.map.call(Document_Scan.AttachedDocsArray, s => s.PatDocId).toString());

                data.append("AttachedDocs", Array.prototype.map.call(Document_Scan.AttachedDocsArray, function (s) {
                    return s.PatDocId;
                }).toString());

            } else {
                data.append("AttachedDocs", "");
            }
            //var AttachedDocs = [];
            //if (Document_Scan.AttachedDocsArray && Document_Scan.AttachedDocsArray.length > 0) {
            //    $.each(Document_Scan.AttachedDocsArray, function (i, item) {
            //        var objectDoc = new Object();
            //        objectDoc.id = item.PatDocId;
            //        AttachedDocs.push(objectDoc);
            //    });
            //}
            //data.append("AttachedDocs", JSON.stringify(AttachedDocs));

            if (Document_Scan.params['RefCtrl'] == "advancePayment")
                data.append('advancePaymentId', Document_Scan.params['AdvancePaymentId']);
            else if (Document_Scan.params['RefCtrl'] == "LabResult" || Document_Scan.params['RefCtrl'] == "RadiologyResult" || Document_Scan.params['RefCtrl'] == "IncomingReferral" || Document_Scan.params['RefCtrl'] == "OutgoingReferral" || Document_Scan.params['RefCtrl'] == "OrderSetOutgoingReferral") {
                data.append('RefModuleName', Document_Scan.params['RefModuleName']);
                data.append('TransitionId', Document_Scan.params['TransitionId']);
                data.append('OrderSetReferralId', Document_Scan.params['OrderSetReferralId']);
            }
            var markAsReview = false;
            if (Document_Scan.params.RefCtrl == "patTabInsurance" || Document_Scan.params.RefCtrl == "patientDemographic" || Document_Scan.params.RefCtrl == "imageupload") {
                markAsReview = true;
            }
            if ($("#" + Document_Scan.params.PanelID + " #frmDocumentScan #source > option").length > 0) {
                if ($("#" + Document_Scan.params.PanelID + " #frmDocumentScan #source > option:selected").text() == "ScanShell 800DX" && markAsReview) {
                    $("#" + Document_Scan.params.PanelID + " #frmDocumentScan #chkReviewed").prop('checked', true);
                }
            }
            var myJSON = self.getMyJSON();
            var MyJsonParse = JSON.parse(myJSON)
            data.append("PatientDocumentData", myJSON);
            var FolderList = [];
            self.find('#divFolder ul.multiselect-container li input[type=checkbox]:checked').map(function () {
                var objectFolder = new Object();
                objectFolder.Value = this.value;
                objectFolder.Name = this.nextSibling.data.trim();
                FolderList.push(objectFolder);
            });
            if (FolderList.length > 0) {
                if (Document_Scan.params.mode == "Scan") {
                    data.append("FolderList", JSON.stringify(FolderList));
                    if (Document_Scan.PasswordJSON != "") {
                        data.append("PasswordJSON", Document_Scan.PasswordJSON)
                    } else {
                        data.append("PasswordJSON", "{'SetPassword':'','ConfirmPassword':'','UserId':null}")
                    }
                    Document_Scan.SaveDocument(data).done(function (response) {
                        if (response.status != false) {
                            if (MyJsonParse.ddlAssignUserto == globalAppdata['AppUserId']) {
                                Document_AssignedTo.PendingCountSearchDoc();
                            }
                            utility.DisplayMessages(response.message, 1);
                            Patient_Document.LoadFolders(true);
                            if (Document_Scan.params['RefCtrl'] == "LabResult") {
                                if (ClinicalLabOrderDetail != null) {
                                    $('#' + ClinicalLabResultDetail.params.PanelID + " #ddlStatus").val("Final");
                                    ClinicalLabResultDetail.params.ScanDocument = true;
                                    ClinicalLabResultDetail.LabResultSave('');
                                }
                            }

                            if (Document_Scan.params["PanelID"] == 'pnlPatientDocument' || Document_Scan.params["PanelID"] == 'pnlBatchDocuments') {
                                Patient_Document.LoadFolders(true);
                                utility.myConfirm('The document has been saved successfully. Do you want to delete this document?', function () {
                                    btnRemoveCurrentImage_onclick();
                                }, function () { Patient_Document.ShowHideMoveButton(); }, 'Confirm Delete');
                            } else if (Document_Scan.params["PanelID"] == 'pnlPatientAdvancePayment') {
                                $("#" + Document_Scan.params["PanelID"] + " #btnDocumentScan").addClass('disableAll');
                                advancePaymentDetail.DocumentSearch();
                                Dynamsoft.WebTwainEnv = null;
                                // DWObject.CloseSource();
                                UnloadActionPan(Document_Scan.params.ParentCtrl, 'Document_Scan');
                            }
                            //Patient_Document.DocumentSearch();
                            //UnloadActionPan(folderDetail.params["ParentCtrl"], "Document_Import");

                            ScannerLoaded = "ScanShell 800DX";
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    utility.DisplayMessages("Please select a valid file to upload.", 3);
                }
            } else {
                Document_Scan.validateFolder(1);
            }
        });
    },

    LoadDocumentsForAttach: function () {
        AppPrivileges.GetFormPrivileges("Documents", "LINK", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PanelID"] = "pnlPatientDocument_Search";
                params["ParentCtrl"] = 'Document_Scan';
                params["PatientId"] = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId').val();
                LoadActionPan('Patient_Document_Search', params);
            } else
                utility.DisplayMessages(strMessage, 2);
            //$("#" + Patient_Document.params.PanelID + " #btnAttach").addClass('hidden');
        });
    },

    validateFolder: function (operationid) {
        $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divFolder label").find("i").remove();
        if (operationid == 1) {
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divFolder .multiselect").css("border-color", "#cc2724");
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divFolder").find(".control-label").css("color", "#cc2724");
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divFolder").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-remove" data-bv-icon-for="Race" style="display: block;"></i>');
        } else if (operationid == 2) {
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divFolder .multiselect").css("border-color", "#3c763d");
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divFolder").find(".control-label").css("color", "#3c763d");
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divFolder").find(".control-label").prepend('<i class="form-control-feedback glyphicon glyphicon-ok" data-bv-icon-for="Race" style="display: block;"></i>');
        } else {
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divFolder .multiselect").css("border-color", "#ccc");
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divFolder").find(".control-label").css("color", "#000000");
        }
    },


    SaveDocument: function (data, PatientDocumentData) {
        return MDVisionService.fileService(data, "PATIENT_DOCUMENT", "SAVE_PATIENT_DOCUMENT");
    },
    ValidateScanner: function (FormId) {

        $('#' + FormId).bootstrapValidator('destroy');
        $('#' + FormId).unbind("success.form.bv");

        $('#' + FormId)
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
                        group: '.col-xs-6',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    FullName: {
                        group: '.col-xs-6',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    }
                }
            })
            .on('success.form.bv', function (e) {
                e.preventDefault();
                Document_Scan.saveScanDocument();
                Document_Scan.IsValidationRequired = true;

            });
    },
    SaveScannerClicked: function (btn) {
        setTimeout(function () {
            if (!Document_Scan.IsValidationRequired) {
                Document_Scan.ValidateScanner(Document_Scan.params.PanelID + ' #frmDocumentScan');
                Document_Scan.IsValidationRequired = true;
                $(btn).trigger('click');
                return true;
            } Document_Scan.IsValidationRequired = false;
        }, 200);
    },
    CustomSettings: function () {
        $("#" + Document_Scan.params["PanelID"] + ' .formFields').hide();
        $("#" + Document_Scan.params["PanelID"] + ' .editImage').hide();
        $("#" + Document_Scan.params["PanelID"] + ' .localImage').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #DWTemessageContainer').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #dwtcontrolContainer').css('height', '400px');
        $("#" + Document_Scan.params["PanelID"] + ' #dwtcontrolContainer').css('overflow', 'auto');
        $("#" + Document_Scan.params["PanelID"] + ' #MultiPagePDF_save').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #btnScanAndExit').show();
        $("#" + Document_Scan.params["PanelID"] + ' #btnSave').show();

        // $('#dwtcontrolContainer canvas').attr('height', '390');
        // $('.ds-dwt-container-box').css('height', '400px');
        // $('.ds-dwt-container-box div').css('height', '400px');

    },
    CustomSettingsforImageLoad: function () {
        $("#" + Document_Scan.params["PanelID"] + ' .formFields').hide();
        $("#" + Document_Scan.params["PanelID"] + ' .editImage').hide();
        $("#" + Document_Scan.params["PanelID"] + ' .localImage').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #DWTemessageContainer').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #MultiPagePDF_save').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #btnScanAndExit').show();
        $("#" + Document_Scan.params["PanelID"] + ' #btnSave').show();

    },
    UnLoadTab: function () {



        //if (Document_Scan.params != null && Document_Scan.params.ParentCtrl != null) {
        //    UnloadActionPan(Document_Scan.params.ParentCtrl, "Document_Scan");
        //    //UnloadActionPan(Document_Viewer.params.ParentCtrl);

        //    if (Document_Scan.params.ParentCtrl == "advancePaymentDetail")
        //        advancePaymentDetail.DocumentSearch();
        //}

        if (Document_Scan.params["PanelID"] == 'pnlPatientDocument' || Document_Scan.params["PanelID"] == 'pnlBatchDocuments') {
            try {
                var size = DWObject.HowManyImagesInBuffer;
                if (parseFloat(size) > 0) {
                    utility.myConfirm('2', function () {
                        if (Document_Scan.params != null && Document_Scan.params.ParentCtrl != null) {
                            Dynamsoft.WebTwainEnv = null;
                            // DWObject.CloseSource();
                            UnloadActionPan(Document_Scan.params.ParentCtrl, "Document_Scan");
                            //UnloadActionPan(Document_Viewer.params.ParentCtrl);

                            if (Document_Scan.params.ParentCtrl == "advancePaymentDetail")
                                advancePaymentDetail.DocumentSearch();
                        }
                        else {
                            Dynamsoft.WebTwainEnv = null;
                            // DWObject.CloseSource();
                            UnloadActionPan(null, 'Document_Scan');
                        }

                    }, function () {
                    },
                                            '2'
                        );
                } else {
                    if (Document_Scan.params["PatientDetail"] == "1") {
                        DWObject.RemoveAllImages();
                        Dynamsoft.WebTwainEnv = null;
                        // DWObject.CloseSource();
                        UnloadActionPan(Document_Scan.params.ParentCtrl, 'Document_Scan');
                    } else {
                        Dynamsoft.WebTwainEnv = null;
                        // DWObject.CloseSource();
                        UnloadActionPan(null, 'Document_Scan');
                    }
                }
            }
            catch (ex) {
                if (Document_Scan.params["PatientDetail"] == "1") {
                    Dynamsoft.WebTwainEnv = null;
                    // DWObject.CloseSource();
                    UnloadActionPan(Document_Scan.params.ParentCtrl, 'Document_Scan');
                } else {
                    Dynamsoft.WebTwainEnv = null;
                    // DWObject.CloseSource();
                    UnloadActionPan(null, 'Document_Scan');
                }
                console.log(ex);

            }
        } else {
            Dynamsoft.WebTwainEnv = null;
            // DWObject.CloseSource();
            UnloadActionPan(Document_Scan.params.ParentCtrl, 'Document_Scan');
        }
        ScannerLoaded = "ScanShell 800DX";
        if (Document_Scan.params["PanelID"] == "pnlDemographicQuick" || Document_Scan.params.DocPrivate == "1") {
            if ($('.modal-backdrop').length > 0) {
                $('.modal-backdrop').remove();
            }
        }
    },


    ScanAndExit: function (isExit) {
        var fileType = "";
        var base64 = "";
        var type;
        DWObject.RegisterEvent('OnPostTransfer', function () {
            var controls = document.getElementsByName("imgType_save");
            for (i = 0; i < 5; i++) {
                if (controls.item(i).checked == true) {
                    type = controls.item(i).value;
                    break;
                }
            }
            DWObject.SetSelectedImageIndex(0, DWObject.CurrentImageIndexInBuffer);
            switch (type) {
                case "bmp":
                    var size = DWObject.GetSelectedImagesSize(0); // Calculate the size of selected images in BMP format
                    base64 = DWObject.SaveSelectedImagesToBase64Binary();
                    fileType = "image/bmp";
                    break;

                case "jpg":
                    var size = DWObject.GetSelectedImagesSize(1); // Calculate the size of selected images in JPG format
                    base64 = DWObject.SaveSelectedImagesToBase64Binary();
                    fileType = "image/jpg";
                    break;

                case "png":
                    var size = DWObject.GetSelectedImagesSize(3); // Calculate the size of selected images in PNG format
                    base64 = DWObject.SaveSelectedImagesToBase64Binary();
                    fileType = "image/png";
                    break;
                case "pdf":
                    var size = DWObject.GetSelectedImagesSize(4); // Calculate the size of selected images in PDF format
                    base64 = DWObject.SaveSelectedImagesToBase64Binary();
                    fileType = "application/pdf";
                    break;
            }
            if (!document.getElementById("MultiPagePDF_save").checked) {
                var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "chargeBatchDetail") {
                    chargeBatchDetail.GetScannedDocuments(base64, fileType, FileName);
                    Document_Scan.counter++;
                }
                else if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "paymentBatchDetail") {
                    paymentBatchDetail.GetScannedDocuments(base64, fileType, FileName);
                    Document_Scan.counter++;
                }
            }
            DWObject.CloseSource();
        });
        if (isExit) {
            DWObject.RegisterEvent('OnPostAllTransfers', function () {
                if (type == "pdf" && document.getElementById("MultiPagePDF_save").checked) {
                    DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
                    //for (var i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
                    //    DWObject.SetSelectedImageIndex(i, i);
                    //}
                    var size = DWObject.GetSelectedImagesSize(4); // Calculate the size of selected images in pdf format

                    //imgData = "data:application/pdf;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                    var imgData = DWObject.SaveSelectedImagesToBase64Binary();
                    fileType = "application/pdf";
                    var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                    var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                    if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "chargeBatchDetail") {
                        chargeBatchDetail.GetScannedDocuments(imgData, fileType, FileName);
                        Document_Scan.counter++;
                    }
                    else if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "paymentBatchDetail") {
                        paymentBatchDetail.GetScannedDocuments(imgData, fileType, FileName);
                        Document_Scan.counter++;
                    }

                }
                DWObject.CloseSource();
                Document_Scan.UnLoadTab();
                DWObject.UnregisterEvent('OnPostTransfer', function () { });
                DWObject.UnregisterEvent('OnPostAllTransfers', function () { });
            });
        }
    },
    InsuranceCustomSettings: function () {
        Document_Scan.CustomSettings();
        $("#" + Document_Scan.params["PanelID"] + ' #lstFileName').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #btnSave').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #btnScanAndExit').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #imgTypejpeg').prop('checked', true);
        $("#" + Document_Scan.params["PanelID"] + ' #RGB').prop('checked', true);
        $("#" + Document_Scan.params["PanelID"] + ' #Duplex').prop('checked', true);
        $("#" + Document_Scan.params["PanelID"] + ' #btnScanProcess').show();
        $("#" + Document_Scan.params["PanelID"] + ' #btnScanProcessWithInsurance').show();
        $("#" + Document_Scan.params["PanelID"] + ' #divCustomScanAttach').hide();
        $("#" + Document_Scan.params["PanelID"] + ' .label-strip-modal').removeClass("label-strip-modal");
    },
    DemographicCustomSettings: function () {
        Document_Scan.CustomSettingsforImageLoad();
        $("#" + Document_Scan.params["PanelID"] + ' #lstFileName').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #btnSave').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #btnScanAndExit').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #btnDocumentScan').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #divSaveimage').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #imgTypejpeg').prop('checked', true);
        $("#" + Document_Scan.params["PanelID"] + ' #RGB').prop('checked', true);
        $("#" + Document_Scan.params["PanelID"] + ' #Duplex').prop('checked', true);
        $("#" + Document_Scan.params["PanelID"] + ' #btnScanProcessDemographic').show();
        $("#" + Document_Scan.params["PanelID"] + ' #divCustomScanAttach').hide();
        $("#" + Document_Scan.params["PanelID"] + ' .label-strip-modal').addClass("modal-dialog-full");
        $("#" + Document_Scan.params["PanelID"] + ' .label-strip-modal').removeClass("label-strip-modal");
        $("#" + Document_Scan.params["PanelID"] + ' #div_controls').addClass("hidden");

    },
    UploadimageCustomSettings: function () {
        Document_Scan.CustomSettingsforImageLoad();
        $("#" + Document_Scan.params["PanelID"] + ' #lstFileName').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #btnSave').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #btnScanAndExit').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #imgTypejpeg').prop('checked', true);
        $("#" + Document_Scan.params["PanelID"] + ' #RGB').prop('checked', true);
        $("#" + Document_Scan.params["PanelID"] + ' #Duplex').prop('checked', true);
        $("#" + Document_Scan.params["PanelID"] + ' #btnScanProcessDemographic').show();
        $("#" + Document_Scan.params["PanelID"] + ' #divSaveimage').hide();
        $("#" + Document_Scan.params["PanelID"] + ' #divCustomScanAttach').hide();

    },
    UploadImage: function () {

        Document_Scan.ScanDrivingLicense();

    },

    SetCanvasForUploadImage: function (frontImage, backImage) {
        var PatientDocumentImag = "";
        var c = document.createElement('canvas');
        if (c.getContext) {
            var img = "";
            var ctx = c.getContext("2d");
            var imageObj1 = new Image();
            var imageObj2 = new Image();
            imageObj1.onload = function () {
                c.width = imageObj1.width;
                c.height = imageObj1.height * 2;
                ctx.drawImage(imageObj1, 0, 0);
                imageObj2.onload = function () {
                    ctx.drawImage(imageObj2, 0, imageObj1.height);
                    var PatientDocumentImage = c.toDataURL("image/jpeg");
                    var img = c.toDataURL("image/png");
                    $(window.parent.document).find('#frmDemographic #myCanvasUploadImg').attr('src', PatientDocumentImage);
                }
                imageObj2.src = frontImage;
            }
            imageObj1.src = backImage;
        }
    },

    SavePatientDemographicPic: function () {
        acquireImage();
        var dfd2 = new $.Deferred();
        var base64 = null;

        DWObject.RegisterEvent('OnPostAllTransfers', function () {
            if (checkIfImagesInBuffer()) {
                var frontImage;
                var fileType;
                var backImage;
                var refscanner = false;
                DWObject.CurrentImageIndexInBuffer = 0;
                DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
                DWObject.SetSelectedImageIndex(0, DWObject.CurrentImageIndexInBuffer);
                DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_JPG);
                backImage = "data:image/jpeg;base64," + DWObject.SaveSelectedImagesToBase64Binary();

                DWObject.CurrentImageIndexInBuffer = 1;
                DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
                DWObject.SetSelectedImageIndex(0, DWObject.CurrentImageIndexInBuffer);
                DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_JPG);
                frontImage = "data:image/jpeg;base64," + DWObject.SaveSelectedImagesToBase64Binary();

                fileType = "image/jpeg";

                Document_Scan.SetCanvasForUploadImage(frontImage, backImage);
                var frontImageBlob;
                var backImagerBlob;
                var frmImageToProcess = new FormData();
                frontImageBlob = utility.basr64URLtoBlob(frontImage);
                backImagerBlob = utility.basr64URLtoBlob(backImage);

                frmImageToProcess.append("backImage", frontImageBlob);
                frmImageToProcess.append("frontImage", backImagerBlob);
                //Accesing the web service..
                $.ajax({
                    type: "POST",
                    url: "https://cssnwebservices.com/CSSNService/CardProcessor/ProcessDLDuplexEX/0/true/-1/true/true/true/0/150/false/false",
                    data: frmImageToProcess,
                    cache: false,
                    contentType: 'application/octet-stream; charset=utf-8;',
                    dataType: "json",
                    processData: false,
                    beforeSend: function (xhr) {
                        BackgroundLoaderShow(true);
                        xhr.setRequestHeader("Authorization", "LicenseKey " + $.base64.encode("B3E4DBF19BED"));
                    },
                    success: function (data) {
                        try {
                            base64 = utility.arrayBufferToBase64(data.FaceImage);
                            refscanner = true;
                            dfd2.resolve('ok');
                        } catch (ex) {
                            console.log(ex);
                            BackgroundLoaderShow(false);
                            utility.DisplayMessages("Please scan card properly.", 3);
                        }
                    },
                    error: function (e) {
                        BackgroundLoaderShow(false);
                        utility.DisplayMessages("Error: " + e, 3);
                    }
                });

                DWObject.UnregisterEvent('OnPostAllTransfers', function () { });
            }
            else {
                utility.DisplayMessages("There is no scanned image available", 3);
                return false;
            }
        });
        dfd2.then(function () {

            $(window.parent.document).find('#pnlUploadImage #frmUploadImage #scan_Imge').attr('src', base64);
            $(window.parent.document).find('#pnlUploadImage #frmUploadImage #btn_close_uploadimage').click();
        });

    },

    ScanInsuranceCard: function (isProcess, isNewInsurancePlan) {
      //  Patient_Demographic.params["isFromePictureMode"] = false;
        if (Document_Scan.params["IsFromUploadImage"] == true) {
          
            if (isProcess == true && isNewInsurancePlan == "scan") {
                Document_Scan.SavePatientDemographicPic();
            }
            else {
                Document_Scan.UploadImage();
            }

        }
        else {
            var objDefferedScan = $.Deferred();
            var PracticeId = "0";
            if (Patient_Document.params["PanelID"] == "pnldemographicDetail #pnlPatientDocument") {
                PracticeId = $("#pnldemographicDetail #frmdemographicDetail #hfPractice").val();
            } else if (Patient_Document.params["PanelID"] == "ctrlPanPatient #pnlPatientDocument") {
                PracticeId = $("#pnlDemographic #frmDemographic #hfPractice").val();
            } else if (Patient_Document.params["PanelID"] == "pnlBatchDocuments") {
                PracticeId = $("#pnlBatchDocuments #hfPractice").val();
            } else if (Document_Scan.params["PanelID"] == "pnlPaymentBatchDetail") {
                PracticeId = $("#pnlPaymentBatchDetail #frmPaymentBatchDetail #hfPractice").val();
            } else if (Document_Scan.params["PanelID"] == "pnlChargeBatchDetail") {
                PracticeId = $("#pnlChargeBatchDetail #frmChargeBatchDetail #hfPractice").val();
            } else if (Patient_Document.params["PanelID"] == "pnlPatientInsurance" || Document_Scan.params["PanelID"] == "pnlPatientInsurance" || Document_Scan.params["PanelID"] == "pnlPatientAdvancePayment") {
                PracticeId = $("#pnlDemographic #frmDemographic #hfPractice").val();
                if (!PracticeId) {
                    PracticeId = $('#' + demographicDetail.params.PanelID + ' #frmdemographicDetail #hfPractice').val()
                }
            } else {
                PracticeId = globalAppdata['DefaultPracticeId'];
            }

            practiceDetail.DemographicPractice(PracticeId).done(function (response) {
                if (response.status != false) {
                    var medication_detail = JSON.parse(response.PracticeFill_JSON);
                    ScanPrivilige = medication_detail.chkScan;
                    OCRPrivilige = medication_detail.chkOCR;
                    if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                        Patient_Demographic.ScanPrivilige = medication_detail.chkScan;
                        Patient_Demographic.OCRPrivilige = medication_detail.chkOCR;
                    }
                }
                else {
                    ScanPrivilige = false;
                    OCRPrivilige = false;
                    if (Patient_Demographic.params.PanelID == "pnlDemographic") {
                        Patient_Demographic.ScanPrivilige = false;
                        Patient_Demographic.OCRPrivilige = false;
                    }
                    if (Document_Scan.params.PanelID == "pnlBatchDocuments") {
                        ScanPrivilige = "True";
                        OCRPrivilige = "True";
                    }
                }
                objDefferedScan.resolve('ok');
            });

            objDefferedScan.then(function () {
                if (OCRPrivilige == "True") {
                    //BackgroundLoaderShow(true);
                    var strCount = DWObject.HowManyImagesInBuffer;
                    if (strCount > 0) {
                        if (Document_Scan.params.PanelID == "pnlPatientDocument" || Document_Scan.params.PanelID == "pnlBatchDocuments") {
                            Document_Scan.ProcessScanning(isProcess, isNewInsurancePlan);
                        }
                        else {
                            utility.myConfirm('Unsaved document(s) will be removed. Do you want to continue?', function () {
                                DWObject.RemoveAllImages();
                                Document_Scan.ProcessScanning(isProcess, isNewInsurancePlan);
                            }, function () { },
                                                                'Confirmation Message'
                                            );
                        }

                    }
                    else {
                        DWObject.RemoveAllImages();
                        Document_Scan.ProcessScanning(isProcess, isNewInsurancePlan);
                    }


                }
                else {
                    utility.DisplayMessages("Practice is not selected or doesnot have priviliges to OCR. Please contact your administrator.", 2);
                }
            });
        }
    },
    ProcessScanning: function (isProcess, isNewInsurancePlan) {
        acquireImage();
        var frontImage;
        var fileType;
        var backImage;
        // var imgData = null;
        // var filetype = null;
        DWObject.RegisterEvent('OnPostAllTransfers', function () {
            if (DWObject.HowManyImagesInBuffer && DWObject.HowManyImagesInBuffer > 0) {
                $("#" + Patient_Document.params["PanelID"]).find('.btn-docMove').removeClass("hidden");
            }
            //DWObject.__OnRefreshUI(0)
            DWObject.CurrentImageIndexInBuffer = 0;
            DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
            DWObject.SetSelectedImageIndex(0, DWObject.CurrentImageIndexInBuffer);
            DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_JPG);
            backImage = "data:image/jpeg;base64," + DWObject.SaveSelectedImagesToBase64Binary();

            //DWObject.__OnRefreshUI(1);
            if ($('#source :selected').text().includes("PaperStream IP fi-7160"))
                DWObject.CurrentImageIndexInBuffer = 0;
            else
                DWObject.CurrentImageIndexInBuffer = 1;
            DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
            if (Document_Scan.params.RefCtrl == "patTabInsurance" || Document_Scan.params.RefCtrl == "patientDemographic" || Document_Scan.params.RefCtrl == "imageupload") {
                DWObject.SetSelectedImageIndex(0, DWObject.CurrentImageIndexInBuffer);
            }
            else {
                DWObject.SetSelectedImageIndex(0, 0);
            }
            DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_JPG);
            frontImage = "data:image/jpeg;base64," + DWObject.SaveSelectedImagesToBase64Binary();

            if (isNewInsurancePlan != "scan" && Document_Scan.params.PanelID != "pnlPatientDocument" && Document_Scan.params.PanelID != "pnlBatchDocuments" && Document_Scan.params.PanelID != "pnlPatientAdvancePayment") {
                fileType = "image/jpeg";
                if (Document_Scan.params["IsFromIfram"] == true) {
                    $(window.parent.document).find('#pnlDocument_Scanner #hfIsNewInsurancePlan').val(isNewInsurancePlan);
                    $(window.parent.document).find('#pnlDocument_Scanner #img_frontImge').attr('src', frontImage);
                    $(window.parent.document).find('#pnlDocument_Scanner #img_backImge').attr('src', backImage);
                    DWObject.UnregisterEvent('OnPostAllTransfers', function () { });
                    $(window.parent.document).find('#pnlDocument_Scanner #btn_close_with_processing').click();
                }
                else {
                    Patient_Insurance.fillProcessedDataWithAPI(isNewInsurancePlan, frontImage, backImage);
                    DWObject.UnregisterEvent('OnPostAllTransfers', function () { });
                    if (Document_Scan.params.PanelID == "pnlPatientInsurance") {
                        Patient_Insurance.InsuranceCardChanged = "true";
                    }
                }

            }
            else {
                if (Patient_Insurance.params.PanelID) {
                    Document_Scan.LoadScannedImages(frontImage, backImage);
                }
                else if (Document_Scan.params["IsFromIfram"] == true) {
                    $(window.parent.document).find('#pnlDocument_Scanner #img_frontImge').attr('src', frontImage);
                    $(window.parent.document).find('#pnlDocument_Scanner #img_backImge').attr('src', backImage);
                    $(window.parent.document).find('#pnlDocument_Scanner #btn_close_with_scan').click();
                }
            }
            /*// Reverted as per discussion with Kazi/Bilal sab
            if (Document_Scan.params.PanelID == "pnlPatientDocument" || Document_Scan.params.PanelID == "pnlBatchDocuments") {
                DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
                for (var i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
                    DWObject.SetSelectedImageIndex(i, i);
                }
            }*/
            DWObject.CloseSource();
        });

    },

    LoadScannedImages: function (frontImage, backImage) {

        $('#' + Patient_Insurance.params.PanelID + ' #image').attr('src', "");
        $('#' + Patient_Insurance.params.PanelID + ' #backimage').attr('src', "");
        $('#' + Patient_Insurance.params.PanelID + ' #backimage').hide();

        var c = document.getElementById("myCanvas");
        if (c && c.getContext) {
            var ctx = c.getContext("2d");
            var imageObj1 = new Image();
            var imageObj2 = new Image();
            imageObj1.onload = function () {
                c.width = imageObj1.width;
                c.height = imageObj1.height * 2;
                ctx.drawImage(imageObj1, 0, 0);
                imageObj2.onload = function () {
                    ctx.drawImage(imageObj2, 0, imageObj1.height);

                    var img = c.toDataURL("image/png");

                    $('#' + Patient_Insurance.params.PanelID + ' #image').attr('src', img);
                    $('#' + Patient_Insurance.params.PanelID + ' #scanImage #image').css({ "cursor": "pointer" });

                    if (Document_Scan.params.PanelID != "pnlPatientDocument" && Document_Scan.params.PanelID != "pnlBatchDocuments" && Document_Scan.params.PanelID != 'pnlPatientAdvancePayment') {
                        if (Document_Scan.params.PanelID.indexOf("pnlPatientInsurance") > -1) {
                            $('#' + Document_Scan.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("true");
                        }
                        Document_Scan.UnloadDialogue();
                    }
                }
                imageObj2.src = frontImage;
            }

            imageObj1.src = backImage;

        }
        if (Document_Scan.params.PanelID == "pnlPatientInsurance") {
            Patient_Insurance.InsuranceCardChanged = "true";
        }
    },

    UnloadDialogue: function () {
        Patient_Insurance.isProcessed = '1';

        if (Document_Scan.params["IsFromIfram"] = true) {
            Document_Scanner.UnLoadTab();
        }
        else {
            Document_Scan.UnLoadTab();
        }

        BackgroundLoaderShow(false);
        //Patient_Insurance.ProcessedData = medicalCard;
        //  $("#" + Patient_Insurance.params.PanelID + " #txtSubscriberID").trigger('blur');
        var downKeyEvent = $.Event("keydown");
        downKeyEvent.keyCode = $.ui.keyCode.DOWN;  // event for pressing "down" key
        var enterKeyEvent = $.Event("keydown");
        enterKeyEvent.keyCode = $.ui.keyCode.ENTER;
        //$("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan").trigger(downKeyEvent);  // First downkey invokes search
        //$("#" + Patient_Insurance.params.PanelID + " #txtInsurancePlan").trigger(enterKeyEvent);

    },
    OpenCaseDetail: function (HiddenCtrl) {
        var params = [];
        params["CaseId"] = parseInt($('#' + Document_Scan.params["PanelID"] + ' #' + HiddenCtrl).val());
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        //  params["PatientId"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfPatientId").val();
        params["RefCtrl"] = "ddlCase";
        params["ParentCtrl"] = 'Document_Scan';

        LoadActionPan('Patient_Case_Detail', params);
    },
    OpenCase: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["CaseId"] = "-1";
        // params["patientID"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfPatientId").val();
        params["patientID"] = "";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Document_Scan';

        LoadActionPan('Patient_Case', params);
    },

    ScanDrivingLicense: function () {
        //if ($('#ddlFolder').val() == "") {
        //    $('#' + Document_Scan.params.PanelID + ' #frmDocumentScan').bootstrapValidator('revalidateField', 'Folder');
        //    utility.DisplayMessages("Please select Document Type", 3);
        //} else {
        if (OCRPrivilige == "True") {
            DWObject.RemoveAllImages();
            acquireImage();
            var frontImage;
            var fileType;
            var backImage;
            var imgData = null;
            var filetype = null;

            DWObject.RegisterEvent('OnPostAllTransfers', function () {

                //
                if (!checkIfImagesInBuffer()) {
                    utility.DisplayMessages("There is no scanned document available", 3);
                    return;
                }
            //    Patient_Demographic.params["isFromePictureMode"] = false;
                var i, strimgType_save;
                var NM_imgType_save = document.getElementsByName("imgType_save");
                for (i = 0; i < 5; i++) {
                    if (NM_imgType_save.item(i).checked == true) {
                        strimgType_save = NM_imgType_save.item(i).value;
                        break;
                    }
                }
                DWObject.IfShowFileDialog = true;
                var _txtFileNameforSave = document.getElementById("txt_fileNameforSave");
                var bSave = false;

                var _chkMultiPageTIFF_save = document.getElementById("MultiPageTIFF_save");
                var vAsyn = false;
                if (strimgType_save == "tif" && _chkMultiPageTIFF_save.checked) {
                    vAsyn = true;
                    DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
                    //for (var i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
                    //    DWObject.SetSelectedImageIndex(i, i);
                    //}
                    var size = DWObject.GetSelectedImagesSize(2); // Calculate the size of selected images in pdf format

                    //imgData = "data:image/tiff;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                    imgData = DWObject.SaveSelectedImagesToBase64Binary();
                    fileType = "image/tiff";
                    var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                    var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                    Document_Scan.DocumentSave(imgData, fileType, FileName);
                    Document_Scan.counter++;
                }
                else if (strimgType_save == "pdf" && document.getElementById("MultiPagePDF_save").checked) {
                    vAsyn = true;
                    DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
                    //for (var i = 0; i < DWObject.HowManyImagesInBuffer; i++) {
                    //    DWObject.SetSelectedImageIndex(i, i);
                    //}
                    var size = DWObject.GetSelectedImagesSize(4); // Calculate the size of selected images in pdf format

                    //imgData = "data:application/pdf;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                    imgData = DWObject.SaveSelectedImagesToBase64Binary();
                    filetype = "application/pdf";

                    var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                    var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();

                }
                // var self = $("#pnlDocumentScan");
                // var myJSON = self.getMyJSON();


                //
                //DWObject.__OnRefreshUI(0)
                DWObject.CurrentImageIndexInBuffer = 0;
                DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
                DWObject.SetSelectedImageIndex(0, DWObject.CurrentImageIndexInBuffer);
                DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_JPG);
                backImage = "data:image/jpeg;base64," + DWObject.SaveSelectedImagesToBase64Binary();

                //DWObject.__OnRefreshUI(1);
                DWObject.CurrentImageIndexInBuffer = 1;
                DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
                DWObject.SetSelectedImageIndex(0, DWObject.CurrentImageIndexInBuffer);
                DWObject.GetSelectedImagesSize(EnumDWT_ImageType.IT_JPG);
                frontImage = "data:image/jpeg;base64," + DWObject.SaveSelectedImagesToBase64Binary();


                if (Document_Scan.params.RefFill == "QuickpatientDemographic") {
                    if (imgData != "") {

                        Patient_DemographicQuick.imagedata = imgData;
                        Document_Scan.SetCanvasImage(frontImage, backImage,false);
                    }

                    // Patient_DemographicQuick.frontimage = backImage;
                    Patient_DemographicQuick.filetype = filetype;
                    if (FileName != "")
                        Patient_DemographicQuick.filename = FileName;

                    //Patient_DemographicQuick.scannerjson = JSON.parse(myJSON);
                } else {

                    if (imgData != "") {
                        demographicDetail.imagedata = imgData;
                        Document_Scan.SetCanvasImage(frontImage, backImage,true);


                        if (Document_Scan.params["IsFromUploadImage"] == true)
                            $(window.parent.document).find('#pnlUploadImage #btn_setkey_uploadimage').click();

                        Patient_Demographic.IsImageUpdated = 'True';
                        Patient_Demographic.params["isFromPictureMode"] = false;
                    }


                    demographicDetail.filetype = filetype;
                    if (FileName != "")
                        demographicDetail.filename = FileName;

                    //  demographicDetail.scannerjson = JSON.parse(myJSON);

                }

                demographicDetail.fillDrivingLicenseDataWithAPI(frontImage, backImage, Document_Scan.params.RefFill);


                // Document_Scan.saveScanDocument();
                DWObject.UnregisterEvent('OnPostAllTransfers', function () { });




                DWObject.CloseSource();
            });


        }
        else {
            utility.DisplayMessages("Practice is not selected or doesnot have priviliges to OCR. Please contact your administrator.", 2);
        }



    },
    SetCanvasImage: function (frontImage, backImage,isFromDemograhpicDetail) {
      //  Patient_Demographic.params["isFromePictureMode"] = false;
        if (Document_Scan.params["IsFromUploadImage"] == true) {
            Document_Scan.SetCanvasForUploadImage(frontImage, backImage);
        }
        else {
            var PatientDocumentImag = "";
            var c = document.createElement('canvas');
            if (c.getContext) {
                var img = "";
                var ctx = c.getContext("2d");
                var imageObj1 = new Image();
                var imageObj2 = new Image();
                imageObj1.onload = function () {
                    c.width = imageObj1.width;
                    c.height = imageObj1.height * 2;
                    ctx.drawImage(imageObj1, 0, 0);
                    imageObj2.onload = function () {
                        ctx.drawImage(imageObj2, 0, imageObj1.height);
                        var PatientDocumentImage = c.toDataURL("image/jpeg");
                        var img = c.toDataURL("image/png");
                        //demographicDetail.imagedata = PatientDocumentImage;
                        
                        if (isFromDemograhpicDetail == true)
                        {
                            $(window.parent.document).find('#frmdemographicDetail #imagedatabase64').val(PatientDocumentImage);
                            $(window.parent.document).find('#frmdemographicDetail #btn_set_imagedata').click();
                        }

                        $(window.parent.document).find('#frmDemographicQuick #myQuickCanvasUploadImg').attr('src', PatientDocumentImage);
                    }
                    imageObj2.src = frontImage;
                }
                imageObj1.src = backImage;
            }
        }
    },
    OpenPatientAccount: function (RefCtrl) {
        var params = [];
        params["FromAdmin"] = "0";
        params["RefCtrl"] = RefCtrl;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Document_Scan";
        LoadActionPan('Patient_Search', params);
    },
    BindPatientNameAutocomplete: function () {
        var Ctrl = $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #txtFullNameScan");
        var hfCtrl = $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #hfPatientId");
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.FullName && obj.FullName.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.FullName;
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
            }
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            Ctrl.val(dataItem.FullName);
            utility.InsertRecentPatient(dataItem.id);
            $("#" + Patient_Document.params["PanelID"] + " #hfPatientId").val(dataItem.id);
            // set patient name in Patient->document.
            $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtFullName').val(dataItem.FullName)
            $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val(dataItem.id);
            $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtAccountNumber').val(dataItem.AccountNo);
            Patient_Document.LoadFolders(true);
            Document_Scan.ResetControls();
        };
        if (Ctrl.data("kendoAutoComplete"))
            Ctrl.data("kendoAutoComplete").destroy();
        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 4,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        utility.GetPatientArrayByName(Ctrl.val(), 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },
    FillPatientInfoFromSearch: function (PatientId, AccountNo, FirstName, LastName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan').val(LastName + ", " + FirstName + " ");
        if ($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan').data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtFullNameScan'), LastName + ", " + FirstName + " ", $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #hfPatientId'), PatientId, "FullName");

        Patient_Search.UnLoad();
        // set patient name in Patient->document.
        $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtFullName').val(LastName + ", " + FirstName + " ")
        $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #hfPatientId').val(PatientId);
        $('#' + Patient_Document.params["PanelID"] + ' #frmPatientDocument #txtAccountNumber').val(AccountNo);
        utility.InsertRecentPatient(PatientId);
        Patient_Document.LoadFolders(true);
        Document_Scan.LoadPatientVisitDOS(PatientId);
        $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan').bootstrapValidator('revalidateField', 'FullName');
        Document_Scan.ResetControls();
    },

    ResetControls: function () {
        $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txt_fileNameforSave').val('');
        $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #dtpDOS').val('');

        $('#' + Document_Scan.params.PanelID + " #frmDocumentScan #ddlFolder").val('');
        $('#' + Document_Scan.params.PanelID + " #frmDocumentScan #ddlFolder").multiselect('refresh');

        if (globalAppdata.DefaultDocumentPriorityId != "") {
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #ddlDocumentPriority").val(globalAppdata.DefaultDocumentPriorityId);
        }
    },

    BindClaimNumber: function (obj, fhcontrol) {
        $(obj).autocomplete({
            autoFocus: true,
            source: function (request, response) {

                utility.Keyupdelay(function () {
                    var ClaimNumber = $(obj).val();
                    if (ClaimNumber.length > 2) {
                        Document_Scan.LoadClaimNumers(ClaimNumber).done(function (responseData) {
                            if (responseData.status != false) {
                                if (responseData.ClaimsCount > 0) {
                                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                                    var AllClaimsVisits = [];
                                    $.each(Claims, function (i, item) {
                                        AllClaimsVisits.push({
                                            id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber
                                        });
                                    });
                                    response(AllClaimsVisits);
                                }
                            }
                        });
                    }
                });
            },
            select: function (event, ui) {
                setTimeout(function () {
                    $("#" + Document_Scan.params["PanelID"] + " #" + fhcontrol).val(ui.item.id);
                    $(obj).val(ui.item.ClaimNumber);

                }, 100);
            }
        });
        $(obj).autocomplete("search");

    },
    LoadClaimNumers: function (claimNumber) {
        var data = "ClaimNumber=" + claimNumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },
    CheckClaimOnChange: function (obj, hfcontrol) {
        var ClaimNumber = $(obj).val();
        if (ClaimNumber.length <= 0) {
            $("#" + Document_Scan.params["PanelID"] + " #" + hfcontrol).val("");
            $(obj).val("");
        }
    },
    LoadPatientVisitDOS: function (PatientId) {
        var patientId = null;
        if (!PatientId) {
            patientId = $('#PatientProfile #hfPatientId').val();
            if (patientId == "") {
                patientId = $("#frmPatientDocument #hfPatientId").val();
            }
        }
        else {
            patientId = PatientId;
        }
        if (patientId) {
            Document_Scan.LoadVisitDOS(patientId).done(function (responce) {
                $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divddlDOS #ddlDOS").empty();
                $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divddlDOS #ddlDOS").append($("<option/>", {
                    value: "",
                    text: "-Select-"
                }));
                if (responce.TotalDOS > 0) {
                    $.each(responce.PatientDOS, function (k, item) {
                        $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #divddlDOS #ddlDOS").append($("<option/>", {
                            value: utility.RemoveTimeFromDate(null, item.DOSFrom),
                            text: utility.RemoveTimeFromDate(null, item.DOSFrom)
                        }));
                    });
                }
            });
        }

    },
    LoadVisitDOS: function (PatientID) {

        var data = "PatientID=" + PatientID;
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "GET_VISIT_DOS");
    },

    SelectedDOSControl: function (obj) {
        if ($(obj).val() == "Calendar") {
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #dtpDOS").removeAttr("disabled");
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #ddlDOS").attr("disabled", "disabled");
            //  $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #ddlDOS").val("");
        }
        else {
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #dtpDOS").attr("disabled", "disabled");
            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #ddlDOS").removeAttr("disabled");
            // $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #dtpDOS").val("");
        }
    },

    PrivateDocument: function (obj, ParentFlow) {
        AppPrivileges.GetFormPrivileges("Documents", "PRIVACY CHECK", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            var strMessage;
            if (strMessage == "") {
                //$('#' + params.PanelID + ' #chkPrivate').attr('checked', true);
                if ($(obj).prop("checked") || ParentFlow == "Document_Viewer") {
                    if ($('body').find('#modal-from-dom-DocumentPassword').length < 1) {
                        Document_Scan.myConfirm(ParentFlow);
                    } else {
                        $('body').find('#modal-from-dom-DocumentPassword').modal("show");
                        $('#popDiv #TxtDocPassword').val("");
                        $('#popDiv #TxtDocConfirmPassword').val("");
                        $('#popDiv #btnDocumentScan').attr("onclick", ParentFlow + ".SaveDocPassword('" + ParentFlow + "');");
                    }
                } else {
                    if (ParentFlow == "Document_Import")
                        Document_Import.PasswordJSON = "";
                    else if (ParentFlow == "Document_Scan") {
                        Document_Scan.PasswordJSON = "";
                    } else if (ParentFlow == "Document_Viewer") {
                        Document_Viewer.PasswordJSON = "";
                    }
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
                $('#' + params.PanelID + ' #chkPrivate').attr('checked', false);
            }
            //$("#" + Document_Scan.params.PanelID + " #chkPrivate").addClass('hidden');
        });
    },

    SetDocumentPassword: function () {
        //AppPrivileges.GetFormPrivileges("Encounter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //if (strMessage == "") {
        //if (visits.length > 0) {

        //}
        //else {
        //    utility.DisplayMessages("Please select claim.", 2);
        //}
        //} else {
        //    utility.DisplayMessages(strMessage, 2);
        //}
        //});
    },

    myConfirm: function (ParentFlow) {
        var DivFormGroup = '<div class="form-group">';
        var DivEnd = '</div>'
        var Password = '<input type="password" name="DocPassword" id="TxtDocPassword" class="form-control">';
        var ConfirmPassword = '<input type="password" name="DocConfirmPassword" id="TxtDocConfirmPassword" class="form-control">';
        var dialogTitle = "Confirm Privacy";
        var Required = '<span class="required">*</span>';
        var Clearfix = '<div class="clearfix"></div>';
        var Spacer20 = '<div class="spacer20"></div>'
        var ShareAccessCaption = 'Share access of this document by selecting the user(s) below. User(s) selected will receive system generated message containing password.';
        var ClickMethod = "";
        if (ParentFlow == "Document_Scan") {
            var ClickMethod = "Document_Scan.cancelConfirmDialog('1')";
            var SaveDocMethod = ParentFlow + ".SaveDocPassword('" + ParentFlow + "');";
        } else {
            var ClickMethod = "Document_Scan.cancelConfirmDialog('0')";
            var SaveDocMethod = ParentFlow + ".SaveDocPassword('" + ParentFlow + "');";
        }

        var markUp = '';
        markUp = '<div id="modal-from-dom-DocumentPassword" class="modal fade">' +
                        '<div class="modal-dialog modal-dialog-smd modal-top-adjust">' +
                            '<div class="modal-content">'
                            + '<div class="modal-header">' + '<button type="button" onclick="' + ClickMethod + '"  class="close" "></button>'
                                + '<h4 class="modal-title">' + dialogTitle + ' </h4>'
                            + DivEnd
                                + '<div class="modal-body bg-white" id="popDiv">'
                                    + '<div class="col-xs-6"><label class="control-label">Set Password' + Required + '</label>' + Password + DivEnd + Clearfix
                                    + '<div class="col-xs-6"><label class="control-label">Confirm Password' + Required + '</label>' + ConfirmPassword + DivEnd
                                    + DivFormGroup
                                        + '<div class="col-xs-12 pad-a-labelsize-btn">'
                                            + '<a href="#" id="btnShareAccess" onclick="Document_Scan.ShowShareAccess();" class="">Share Access</a>'
                                        + DivEnd
                                    + DivEnd + Spacer20
                                        + DivFormGroup
                                        + '<div class="col-xs-12 hidden" id="DivShareAccess">'
                                            + '<div class="col-xs-9"><label class="control-label"><b>' + ShareAccessCaption + '</b></label>' + DivEnd + Spacer20
                                                + '<div class="col-xs-6"><label class="control-label">Select User(s)</label>'
                                                + '<select class="form-control" id="ddlUsers" name="Users" multiple="multiple" ddlist="GetUsers"></select></div>'
                                            + DivEnd
                                            + '<div class="col-xs-12 pad-a-labelsize-btn">'
                                            + '<button id="btnDocumentScan" class="btn btn-primary btn-sm  rightbtn" type="button" onclick="' + SaveDocMethod + '">Save</button>'
                                        + DivEnd
                                        + DivEnd
                                 + DivEnd
                        + DivEnd
                    + DivEnd
                + '</div><div></div>';
        $(markUp).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false
        }).on('shown.bs.modal', function () {

        }).on('hidden.bs.modal', function () {
            if ($('body').find('.modal-backdrop').length > 0) {
                if ($('body').css('overflow').toLowerCase() != "scroll") {
                    $('body').addClass('modal-open');
                }
                else {
                    $('body').addClass('modal-open');
                }

            }
        });

        if (ParentFlow == "Document_Import") {
            Document_Import.params.DocPrivate = "1";
        } else if (ParentFlow == "Document_Scan") {
            Document_Scan.params.DocPrivate = "1";
        } else if (ParentFlow == "Document_Viewer") {
            Document_Viewer.params.DocPrivate = "1";
        }

        $('#popDiv #TxtDocPassword').val("");
        $('#popDiv #TxtDocConfirmPassword').val("");

    },

    ShowShareAccess: function () {
        if ($('#popDiv #DivShareAccess').hasClass('hidden')) {
            $('#popDiv #DivShareAccess').removeClass('hidden');
            if ($("#popDiv").find("#DivShareAccess select option").length <= 0) {
                $("#popDiv").find("#DivShareAccess").loadDropDowns().done(function () {
                    $("#popDiv").find("#DivShareAccess #ddlUsers option")[0].remove();
                    $("#popDiv").find("#DivShareAccess #ddlUsers").multiselect({
                        includeSelectAllOption: true,
                        enableFiltering: true,
                        enableCaseInsensitiveFiltering: true,
                        maxHeight: 200,
                    });
                });
            }
        } else {
            $('#popDiv #DivShareAccess').addClass('hidden');
        }
    },

    SaveDocPassword: function (ParentFlow) {
        var DocPassword = {
        };
        if ($("#popDiv #TxtDocPassword").val() != "") {
            var PasswordMatch = $("#popDiv #TxtDocPassword").val() == $("#popDiv #TxtDocConfirmPassword").val() ? true : false;
            if (PasswordMatch) {
                var Selectedvalues = $("#popDiv").find("#DivShareAccess #ddlUsers").val();
                Selectedvalues = $.isArray(Selectedvalues) ? Selectedvalues.join() : Selectedvalues;
                DocPassword["SetPassword"] = $("#popDiv #TxtDocPassword").val();
                DocPassword["ConfirmPassword"] = $("#popDiv #TxtDocConfirmPassword").val();
                DocPassword["UserId"] = Selectedvalues;

                Document_Scan.PasswordJSON = JSON.stringify(DocPassword);
                if (ParentFlow == "Document_Scan") {
                    Document_Scan.cancelConfirmDialog('1');
                } else {
                    Document_Scan.cancelConfirmDialog('0');
                }
            } else {
                utility.DisplayMessages("Passwords entered do not match", 3);
            }
        } else {
            utility.DisplayMessages("Please enter Password.", 3);
        }

    },

    cancelConfirmDialog: function (ScanClose) {
        $("#modal-from-dom-DocumentPassword").modal('hide');
        if (ScanClose == "1" && $("body").find(".modal-backdrop").length > 0) {
            $("body").find(".modal-backdrop").removeClass("modal-backdrop");
        }
    },

    AddTags: function () {
        var params = [];
        params["ParentCtrl"] = "Document_Scan";
        params["TabId"] = "patTabDocuments";
        LoadActionPan('Patient_DocumentTag', params);
    },
    BindTagAutocomplete: function () {

        var TagName = $('#' + Document_Scan.params["PanelID"] + ' #frmDocumentScan #txtTagName').val();

        utility.Keyupdelay(function () {
            var AllTags = utility.GetDocumentTagArray(TagName, 0).done(function (response) {

                $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #txtTagName").autocomplete({
                    autoFocus: true,
                    //source: AllPatients, // pass an array (without a comma)
                    source: response,
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #hfDocumentTagId").val(ui.item.id);
                            $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #txtTagName").val(ui.item.value);

                        }, 100);
                    }
                }).blur(function () {

                    setTimeout(function () {
                        utility.ValidateAutoComplete($("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #txtTagName"), "frmDocumentScan #hfDocumentTagId", false, 1, null, null);
                    }, 200);

                });

                $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #txtTagName").autocomplete("search");
            });
        });

    },
    FillTagName: function (hfTagId, txtTagName) {
        $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #txtTagName").val(txtTagName);
        $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #hfDocumentTagId").val(hfTagId);
        Patient_DocumentTag.UnLoad();
        // $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan #txtTagName").focus();
    },

    MoveSelectDocs: function (FolderId, FolderName) {

        var imgData = null;
        var fileType = null;
        if (!checkIfImagesInBuffer()) {
            utility.DisplayMessages("There is no scanned document available", 3);
            return;
        }
        var i, strimgType_save;
        var NM_imgType_save = document.getElementsByName("imgType_save");
        for (i = 0; i < 5; i++) {
            if (NM_imgType_save.item(i).checked == true) {
                strimgType_save = NM_imgType_save.item(i).value;
                break;
            }
        }
        DWObject.IfShowFileDialog = true;
        var _txtFileNameforSave = document.getElementById("txt_fileNameforSave");
        var bSave = false;

        var _chkMultiPageTIFF_save = document.getElementById("MultiPageTIFF_save");
        var vAsyn = false;
        if (strimgType_save == "tif" && _chkMultiPageTIFF_save.checked) {
            vAsyn = true;
            DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
            var size = DWObject.GetSelectedImagesSize(2); // Calculate the size of selected images in pdf format

            //imgData = "data:image/tiff;base64," + DWObject.SaveSelectedImagesToBase64Binary();
            imgData = DWObject.SaveSelectedImagesToBase64Binary();
            fileType = "image/tiff";
            var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
            var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
            Document_Scan.DocumentSave_ForMoveDoc(imgData, fileType, FileName, FolderId, FolderName);
            Document_Scan.counter++;
        }
        else if (strimgType_save == "pdf" && document.getElementById("MultiPagePDF_save").checked) {
            vAsyn = true;
            DWObject.SelectedImagesCount = DWObject.HowManyImagesInBuffer;
            var size = DWObject.GetSelectedImagesSize(4); // Calculate the size of selected images in pdf format

            //imgData = "data:application/pdf;base64," + DWObject.SaveSelectedImagesToBase64Binary();
            imgData = DWObject.SaveSelectedImagesToBase64Binary();
            fileType = "application/pdf";
            if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "chargeBatchDetail") {
                var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                chargeBatchDetail.GetScannedDocuments(imgData, fileType, FileName);
                Document_Scan.counter++;
            }
            else if (Document_Scan.params.RefCtrl != undefined && Document_Scan.params.RefCtrl == "paymentBatchDetail") {
                var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                paymentBatchDetail.GetScannedDocuments(imgData, fileType, FileName + ".pdf");
                Document_Scan.counter++;
            }
            else {
                var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                Document_Scan.DocumentSave_ForMoveDoc(imgData, fileType, FileName + ".pdf", FolderId, FolderName);
                Document_Scan.counter++;
            }

        }
        else {
            for (var imgIndex = 0 ; imgIndex < DWObject._HowManyImagesInBuffer; imgIndex++) {
                //DWObject.__OnRefreshUI(imgIndex)
                DWObject.CurrentImageIndexInBuffer = imgIndex;
                //updatePageInfo();
                DWObject.SelectedImagesCount = 1;
                DWObject.SetSelectedImageIndex(0, DWObject.CurrentImageIndexInBuffer);
                switch (i) {
                    case 0:
                        // bitmap
                        var size = DWObject.GetSelectedImagesSize(0); // Calculate the size of selected images in BMP format
                        //imgData = "data:image/bmp;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "image/bmp";

                        break;
                    case 1:
                        //jpeg
                        var size = DWObject.GetSelectedImagesSize(1); // Calculate the size of selected images in JPG format
                        //imgData = "data:image/jpg;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "image/jpg";
                        break;
                    case 2:
                        //Tiff
                        var size = DWObject.GetSelectedImagesSize(2); // Calculate the size of selected images in Tiff format
                        //imgData = "data:image/tiff;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "image/tiff";
                        break;
                    case 3:
                        //PNG
                        var size = DWObject.GetSelectedImagesSize(3); // Calculate the size of selected images in PNG format
                        //imgData = "data:image/png;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "image/png";
                        break;
                    case 4:
                        //pdf
                        var size = DWObject.GetSelectedImagesSize(4); // Calculate the size of selected images in PDF format
                        //imgData = "data:application/pdf;base64," + DWObject.SaveSelectedImagesToBase64Binary();
                        imgData = DWObject.SaveSelectedImagesToBase64Binary();
                        fileType = "application/pdf";
                        break;


                }
                var input = $('#' + Document_Scan.params.PanelID + " #txt_fileNameforSave").val();
                var FileName = input != "" ? (Document_Scan.GetUDate() + '_' + input) : Document_Scan.GetUDate();
                Document_Scan.DocumentSave_ForMoveDoc(imgData, fileType, FileName + "." + strimgType_save, FolderId, FolderName);
                Document_Scan.counter++;
            }
        }

        if (vAsyn == false) {
            if (bSave)
                appendMessage('<b>Save Image: </b>');
            if (checkErrorString()) {
                return;
            }
        }
    },
    DocumentSave_ForMoveDoc: function (strFiles, strFileType, fileName, FolderId, FolderName) {
        var data = new FormData();
        var objDef = $.Deferred();
        if (isFileCompressed) {
            var zip = new JSZip();
            zip.file(fileName, strFiles, { base64: true });
            zip.generateAsync({ type: "blob", compression: "DEFLATE", compressionOptions: { level: 9 } }).then(function (blob) {
                data.append(0, blob);
                objDef.resolve("ok")
            });
        } else {
            objDef.resolve("ok")
        }
        objDef.then(function () {
            var self = $("#" + Document_Scan.params["PanelID"] + " #frmDocumentScan");
            data.append("scanFile", strFiles);
            data.append("PatientID", self.find("#hfPatientId").val() ? self.find("#hfPatientId").val() : Document_Scan.params.patientID);
            data.append("fileType", strFileType);
            data.append("FileName", fileName);
            if (Document_Scan.AttachedDocsArray && Document_Scan.AttachedDocsArray.length > 0) {
                //data.append("AttachedDocs", Array.prototype.map.call(Document_Scan.AttachedDocsArray, s => s.PatDocId).toString());
                data.append("AttachedDocs", Array.prototype.map.call(Document_Scan.AttachedDocsArray, function (s) {
                    return s.PatDocId;
                }).toString());
            } else {
                data.append("AttachedDocs", "");
            }
            var markAsReview = false;
            if (Document_Scan.params.RefCtrl == "patTabInsurance" || Document_Scan.params.RefCtrl == "patientDemographic" || Document_Scan.params.RefCtrl == "imageupload") {
                markAsReview = true;
            }
            if ($("#" + Document_Scan.params.PanelID + " #frmDocumentScan #source > option").length > 0) {
                if ($("#" + Document_Scan.params.PanelID + " #frmDocumentScan #source > option:selected").text() == "ScanShell 800DX" && markAsReview) {
                    $("#" + Document_Scan.params.PanelID + " #frmDocumentScan #chkReviewed").prop('checked', true);
                }
            }
            var myJSON = self.getMyJSON();
            data.append("PatientDocumentData", myJSON);
            var FolderList = [];
            FolderList.push({
                Value: FolderId,
                Name: FolderName.trim()
            });
            if (FolderList.length > 0) {
                if (Document_Scan.params.mode == "Scan") {
                    data.append("FolderList", JSON.stringify(FolderList));
                    if (Document_Scan.PasswordJSON != "") {
                        data.append("PasswordJSON", Document_Scan.PasswordJSON)
                    } else {
                        data.append("PasswordJSON", "{'SetPassword':'','ConfirmPassword':'','UserId':null}")
                    }
                    Document_Scan.SaveDocument(data).done(function (response) {
                        if (response.status != false) {
                            if (Document_Scan.params["PanelID"] == 'pnlPatientDocument' || Document_Scan.params["PanelID"] == 'pnlBatchDocuments') {
                                Patient_Document.LoadFolders(true).done(function () {

                                    var selectedScanImage = $('#DWTcontainer .imgTag_Selected').length
                                    if (selectedScanImage > 1) {
                                        utility.myConfirm('The documents have been moved/transferred successfully. Do you want to delete these documents?', function () {
                                            btnRemoveCurrentImage_onclick();
                                        }, function () {
                                            Patient_Document.ShowHideMoveButton();
                                        }, 'Confirm Delete');
                                    }
                                    else {
                                        utility.myConfirm('The document has been moved/transferred successfully. Do you want to delete this document?', function () {
                                            btnRemoveCurrentImage_onclick();
                                        }, function () {
                                            Patient_Document.ShowHideMoveButton();
                                        }, 'Confirm Delete');
                                    }
                                });
                            }
                            ScannerLoaded = "ScanShell 800DX";
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    utility.DisplayMessages("Please select a valid file to upload.", 3);
                }
            } else {
                Document_Scan.validateFolder(1);
            }
        });
    },
    GetUDate: function () {
        var dNow = new Date();
        //PMS-4449 Start
        if ($('#' + Document_Scan.params.PanelID + " #dtpDOS").is(":visible") && $('#' + Document_Scan.params.PanelID + " #dtpDOS").val()) {
            dNow = new Date($('#' + Document_Scan.params.PanelID + " #dtpDOS").val());
        }
        //PMS-4449 End
        var hours = dNow.getHours();
        var minutes = dNow.getMinutes();
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = (hours < 10 ? '0' + hours : hours) + '' + minutes + '' + ampm;
        var month = (dNow.getMonth() + 1);
        var dayDate = dNow.getDate();
        //var localFullDate = (month < 10 ? '0' + month : month) + '-' + (dayDate < 10 ? '0' + dayDate : dayDate) + '-' + dNow.getFullYear() + ' ' + strTime;
        var localFullDate = (month < 10 ? '0' + month : month) + '.' + (dayDate < 10 ? '0' + dayDate : dayDate) + '.' + dNow.getFullYear();
        return localFullDate;
    },

    SetParamPanalID: function () {

        if ($("#mstrTabBatch").hasClass("active")) {
            Document_Scan.params.PanelID = "pnlBatchDocuments";
        } else if ($("#mstrTabPatient").hasClass("active")) {
            Document_Scan.params.PanelID = "pnlPatientDocument";
        }

    }
}
