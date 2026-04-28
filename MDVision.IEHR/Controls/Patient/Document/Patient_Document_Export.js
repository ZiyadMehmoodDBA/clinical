Document_Export = {
    bIsFirstLoad: true,
    params: [],
    responseForExportDoc: null,

    Load: function (params) {
        Document_Export.params = params;
        Document_Export.params.PanelID = "pnlDocumentExport";

        var self = $('#' + Document_Export.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Document_Export.IntializeMultiSelectDropDownFolder();
        });

        Document_Export.DocumentSearch(Document_Export.params.PatDocIDs);
    },
    UnLoadTab: function () {
        if (Patient_Document.params["ParentCtrl"] == "demographicDetail") {
            var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
            UnloadActionPan(Document_Export.params.ParentCtrl, 'Document_Export', null, parentPanelId);
        } else {
            if (Document_Export.params != null && Document_Export.params.ParentCtrl != null) {
                UnloadActionPan(Document_Export.params.ParentCtrl, 'Document_Export');
            }
            else
                UnloadActionPan(null, 'Document_Export');
        }
    },
    ExportFile: function (evt) {
        Document_Export.ValidateExport();

    },
    DocumentSearch: function (patientDocID) {
        Document_Export.SearchDocument(null, Document_Export.params.patientId, patientDocID).done(function (response) {
            if (response.status != false) {
                Document_Export.responseForExportDoc = $.parseJSON(response.DocumentLoad_JSON);

                var imageExt = ["image/jpeg", "image/png", "image/jpg", "image/gif", "image/bmp", "jpeg", "png", "jpg", "gif", "bmp"];
                for (var i = 0; i < Document_Export.responseForExportDoc.length; i++) {
                    var file = Document_Export.responseForExportDoc[i];
                    if (file.FileType && file.FileType.length > 0) {
                        if ($.inArray(file.FileType, imageExt) == -1) {
                            $("#" + Document_Export.params.PanelID + " #fileConverstion").css("display", "none");
                            break;
                        }
                    } else if (file.FilePath) {
                        var ext = file.FilePath.substr(file.FilePath.lastIndexOf(".") + 1);
                        if ($.inArray(ext, imageExt) == -1) {
                            $("#" + Document_Export.params.PanelID + " #fileConverstion").css("display", "none");
                            break;
                        }
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SearchDocument: function (DocumentData, PatientID, PatDocIDs) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var data = "PatientDocumentData=" + DocumentData + "&PatientID=" + PatientID + "&PatDocIds=" + PatDocIDs + "&bFileStream=0";
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "SEARCH_PATIENT_DOCUMENT");
    },
    ValidateExport: function () {
        $('#pnlDocumentExport')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    FolderName: {
                        group: '.col-xs-8',
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
            Document_Export.ExportDocuments();
            e.stopImmediatePropagation();
        });
    },
    ExportDocuments: function () {

        Document_Export.DownloadDocuments(Document_Export.params.patientId, Document_Export.params.PatDocIDs).done(function (response) {

            if (response.status) {
                var files = JSON.parse(response.Documents);
                var zip = new JSZip();
                for (var fileIndex = 0 ; fileIndex < files.length; fileIndex++) {
                    if (files[fileIndex].Base64FileStream && files[fileIndex].Base64FileStream != "") {
                        if (!zip.files[files[fileIndex].FilePath])
                            zip.file(files[fileIndex].FilePath, files[fileIndex].Base64FileStream, { base64: true });
                        else
                            zip.file("(" + fileIndex + ")" + files[fileIndex].FilePath, files[fileIndex].Base64FileStream, { base64: true });

                    }

                }

                setTimeout(function () {
                    var content = zip.generateAsync({ type: "blob" })
                                         .then(function (content) {
                                             saveAs(content, $('#pnlDocumentExport #txtFolderName').val() + ".zip");

                                         });
                    Document_Export.UnLoadTab();
                }, 20);




                utility.DisplayMessages("Exported Successfully", 1);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    DownloadDocuments: function (PatientID, PatDocIDs) {
        if (PatientID == null) {
            PatientID = 0;
        }
        var converttype = $("input[name='converttype']:checked").val();
        if (!converttype) {
            converttype = "";
        }
        var data = "PatientID=" + PatientID + "&PatDocIds=" + PatDocIDs + "&ConvertType=" + converttype;// + "&bFileStream=1";
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "Download_PATIENT_DOCUMENT");
    },

    IntializeMultiSelectDropDownFolder: function () {
        $('#' + Document_Export.params.PanelID + ' #ddlFolder').multiselect('destroy');
        $('#' + Document_Export.params.PanelID + ' #ddlFolder').multiselect({
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
                        $('#' + Document_Export.params.PanelID + " #ddlFolder").multiselect('refresh');
                    }
                    else {
                        options.each(function () {
                            var input = $('#' + Document_Export.params.PanelID + ' #ddlFolder input[type=checkbox][value="' + $(this).val() + '"]');
                            input.prop('disabled', false);
                        });
                    }
                }
            },
        });
        //$('#' + Patient_Demographic.params.PanelID + " #ddlFolder").val("");
    },
    BindPatientAccount: function () {

        //var AccountNo = $('#' + Document_Export.params.PanelID + ' #txtAccountNumber').val();
        //utility.Keyupdelay(function () {
        //    utility.GetPatientArray(AccountNo, 0).done(function (response) {
        //        $('#' + Document_Export.params.PanelID + ' #txtAccountNumber').autocomplete({
        //            autoFocus: true,
        //            //source: AllPatients, // pass an array (without a comma)
        //            source: response,
        //            select: function (event, ui) {
        //                setTimeout(function () {
        //                    $("#" + Document_Export.params.PanelID + " #hfPatientId").val(ui.item.id);
        //                    utility.InsertRecentPatient(ui.item.id);

        //                }, 300);
        //            }
        //        });
        //        $('#' + Document_Export.params.PanelID + ' #txtAccountNumber').autocomplete("search");
        //        $('#' + Document_Export.params.PanelID + ' #txtAccountNumber').focus();
        //    });
        //});



        var AllPatients = [];
        var AccountNo = $('#' + Document_Export.params.PanelID + ' #txtAccountNumber').val();
        // Start 08/03/2016 Muhammad Irfan for bug # PMS-4361
        utility.Keyupdelay(function () {
            var AllPatients = utility.GetPatientArrayByName(AccountNo, 1).done(function (response) {
                $.each(response, function (i, item) {
                    item.value = item.FullName;
                });

                $("#" + Document_Export.params.PanelID + " #txtAccountNumber").autocomplete({
                    //source: AllPatients, // pass an array (without a comma)
                    autoFocus: true,
                    source: response,
                    open: function (event, ui) {
                        disable = true
                    },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            setTimeout(function () {
                                $("#" + Document_Export.params.PanelID + " #frmDocumentInternalExport #hfPatientId").val(ui.item.id);
                                utility.InsertRecentPatient(ui.item.id);
                            }, 100);

                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                        utility.ValidateAutoCompletePatientName($("#" + Document_Export.params.PanelID + " #txtAccountNumber"), Document_Export.params.PanelID + " #frmDocumentInternalExport #hfPatientId", false, 0, null, null);
                    }, 200);
                });
                $('#' + Document_Export.params.PanelID + ' #txtAccountNumber').autocomplete("search");
                $("#" + Document_Export.params.PanelID + " #txtAccountNumber").focus();
            });
        });
        // End 08/03/2016 Muhammad Irfan for bug # PMS-4361
        //................
        //var AllPatients = [];
        //var IsAccountWithFullName = "0";
        //var AccountNo = $('#' + Document_Export.params.PanelID + ' #txtAccountNumber').val();
        //if (AccountNo.length > 2) {
        //    utility.Keyupdelay(function () {
        //        appointmentDetail.LoadActivePatients(AccountNo).done(function (responseData) {
        //            if (responseData.status != false) {
        //                if (responseData.PatientCount > 0) {
        //                    var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
        //                    $.each(PatientLoadJSONData, function (i, item) {
        //                        if (IsAccountWithFullName != null && IsAccountWithFullName == "1") {
        //                            AllPatients.push({ id: item.PatientId, value: item.AccountNumber + " - " + item.FullName, AccountNumber: item.AccountNumber, FullName: item.FullName });
        //                        }
        //                        else {
        //                            AllPatients.push({ id: item.PatientId, value: item.AccountNumber, AccountNumber: item.AccountNumber, FullName: item.FullName });
        //                        }
        //                    });
        //                    $("#" + Document_Export.params.PanelID + " #txtAccountNumber").autocomplete({
        //                        autoFocus: true,
        //                        source: AllPatients,
        //                        select: function (event, ui) {
        //                            setTimeout(function () {
        //                                $("#" + Document_Export.params.PanelID + " #frmDocumentInternalExport #hfPatientId").val(ui.item.id);
        //                                //Document_Export.LoadPatientCase(ui.item.id);
        //                                utility.InsertRecentPatient(ui.item.id);
        //                                //Document_Export.LoadPatientVisitDOS(ui.item.id);
        //                            }, 100);
        //                        }
        //                    });
        //                    $('#' + Document_Export.params.PanelID + ' #txtAccountNumber').autocomplete("search");
        //                    $("#" + Document_Export.params.PanelID + " #txtAccountNumber").focus();
        //                }
        //            }
        //        });

        //        //var AllPatients = utility.GetPatientArray(AccountNo, 1);
        //    });
        //}
    },
    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Document_Export';
        LoadActionPan('Patient_Search', params);
    },
    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Document_Export.params.PanelID + " #frmDocumentInternalExport #hfPatientId").val(PatientId);
        $("#" + Document_Export.params.PanelID + " #frmDocumentInternalExport #txtAccountNumber").val(patFullName.split(' - ')[1].trim());
        //Document_Export.LoadPatientCase(Number(PatientId));
        UnloadActionPan("Document_Export");
        utility.InsertRecentPatient(PatientId);
        utility.SetAutoCompleteSource($("#" + Document_Export.params.PanelID + " #frmDocumentInternalExport #txtAccountNumber"), $("#" + Document_Export.params.PanelID + " #frmDocumentInternalExport #hfPatientId"));
        //Document_Export.BindPatientAccount();

        //$('#frmDocumentImport').bootstrapValidator('revalidateField', 'AccountNumber');
        //Document_Export.LoadPatientVisitDOS(PatientId);


    },
    InternalExportFiles: function (e) {
        e.preventDefault();
        var self = $("#pnlDocumentExport #frmDocumentInternalExport");


        var FolderList = [];
        self.find('ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            var objectFolder = new Object();
            objectFolder.Value = this.value;
            objectFolder.Name = this.nextSibling.data.trim();
            FolderList.push(objectFolder);
        });
        var patientId = $("#" + Document_Export.params.PanelID + " #frmDocumentInternalExport #hfPatientId").val();
        patientId = patientId.length > 0 ? patientId : null;

        if (FolderList.length <= 0) {
            utility.DisplayMessages("Please select folder", 3);
            return;
        }
        if (patientId <= 0) {
            utility.DisplayMessages("Please select patient account no.", 3);
            return;
        }
        var data = {
            PatientId: patientId,
            CurrentPatientId: $("#PatientProfile #hfPatientId").val(),
            FolderList: JSON.stringify(FolderList),
            DocumentIds: Document_Export.params.PatDocIDs,
            ConvertType: ""
        };

        Document_Export.SaveInternalExport(data).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
                Document_Export.UnLoadTab();
                $("#" + Patient_Document.params["PanelID"] + " #hfPatientId").val($("#PatientProfile #hfPatientId").val());
                Patient_Document.LoadFolders(true);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    SaveInternalExport: function (data) {
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "INTERNAL_EXPORT_PATIENT_DOCUMENT");
    }
}
