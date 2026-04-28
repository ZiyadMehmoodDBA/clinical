Patient_Information_Import = {
    params: [],

    Load: function (params) {
        Patient_Information_Import.params = params;
        if (Patient_Information_Import.params.PanelID != "pnlPatientInformationImport")
            Patient_Information_Import.params.PanelID = Patient_Information_Import.params.PanelID + " #pnlPatientInformationImport";
        else
            Patient_Information_Import.params.PanelID = "pnlPatientInformationImport";

        var self = $('#' + Patient_Information_Import.params.PanelID);
        
        $.when(Patient_Information_Import.LoadDocumentProviderDropDown(), self.loadDropDowns(true)).then(function () {
            $("#" + Patient_Information_Import.params.PanelID + " #ddlFolder option").filter(function () { return $(this).val() == Patient_Information_Import.params.FolderId; }).prop("selected", true);
            $("#" + Patient_Information_Import.params.PanelID + " #ddlDocumentProvider option").filter(function () { return $(this).text().toLowerCase() == "self"; }).prop("selected", true);
            $("#" + Patient_Information_Import.params.PanelID + " #ddlDocumentSource option").filter(function () { return $(this).text().toLowerCase() == "patient portal"; }).prop("selected", true);
            var PatientId = Patient_Information_Import.params.PatientId;
            $("#" + Patient_Information_Import.params.PanelID + " #frmDocumentImport #hfPatientId").val(PatientId);
            Patient_Information_Import.ValidateImport();
            Patient_Information_Import.LoadPatientCase(PatientId);
            Patient_Information_Import.LoadPatientInformationDetail();

            $("#" + Patient_Information_Import.params["PanelID"] + " #frmDocumentImport #ddlDocumentSource").on("change", function (e) {
                if ($("#" + Patient_Information_Import.params["PanelID"] + " #frmDocumentImport #ddlDocumentSource option:selected").text().toLowerCase() == "other") {
                    $('#' + Patient_Information_Import.params["PanelID"] + " #frmDocumentImport #divOtherDocumentSource").removeClass('hidden');
                    $('#' + Patient_Information_Import.params["PanelID"] + " #frmDocumentImport #txtOtherDocumentSource").focus();
                }
                else {
                    $('#' + Patient_Information_Import.params["PanelID"] + " #frmDocumentImport #divOtherDocumentSource").addClass('hidden');
                    $('#' + Patient_Information_Import.params["PanelID"] + " #frmDocumentImport #txtOtherDocumentSource").val('');
                }

            });
            //initialization of date-picker.
            utility.CreateDatePicker(Patient_Information_Import.params.PanelID + ' #dtpDOS', function () {
                //on-change callback method
            });
            utility.CreateDatePicker(Patient_Information_Import.params.PanelID + ' #dtpReceivedOn',
                 function (ev) {
                     if ($('#' + Patient_Information_Import.params.PanelID + ' #frmDocumentImport').data("bootstrapValidator") != null && typeof $('#frmDocumentImport').data('bootstrapValidator') != 'undefined') {
                         $('#' + Patient_Information_Import.params.PanelID + ' #frmDocumentImport').bootstrapValidator('revalidateField', 'ReceivedOn');
                     }
                 }, false);
        });

    },

    LoadPatientInformationDetail: function () {
        Patient_Information_Import.LoadPatientInformationDetail_DBCall(Patient_Information_Import.params.PatPortalDocId).done(function (response) {
            if (response.status != false) {
                var familyContact_detail = JSON.parse(response.PatientInformationSubmissionFill_JSON);
                var self = $('#' + Patient_Information_Import.params.PanelID);
                utility.bindMyJSON(true, familyContact_detail, false, self).done(function () { });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadPatientInformationDetail_DBCall: function (Data) {
        var data = "Id=" + Data;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "FILL_PATIENT_INFORMATION_SUBMISSION");
    },

    LoadDocumentProviderDropDown: function () {
        var data = "IsActive=&ID=" + Patient_Information_Import.params.PatientId;
        return MDVisionService.lookups('GetDocumentProvider', true, data).done(function (result) {
            if (result['GetDocumentProvider']) {
                var options = JSON.parse(result['GetDocumentProvider']);
                var $documentProviderDDL = $('#' + Patient_Information_Import.params.PanelID + ' #ddlDocumentProvider');
                $documentProviderDDL.empty();
                $.each(options, function (i, item) {
                    if (item.Value != null) {
                        $documentProviderDDL.append(
                            $('<option/>', {
                                value: item.Value,
                                html: item.Name,
                                refname: item.RefName,
                                refvalue: item.RefValue

                            })
                        );
                    }
                });
                $documentProviderDDL.append(
                    $('<option/>', {
                        value: 0,
                        html: 'Self',
                        refname: '',
                        refvalue: ''

                    })
                );
            }
        })
    },

    ValidateImport: function () {
        $('#frmDocumentImport')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    fileupload: {
                        group: '.col-sm-9',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Folder: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    DocumentProvider: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    DocumentSource: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    ReceivedOn: {
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
            Patient_Information_Import.ImportSave();
        });
    },

    LoadPatientCase: function (PatientId) {
        $('#' + Patient_Information_Import.params.PanelID + " input#txtCaseNumber").val('');
        $("#" + Patient_Information_Import.params.PanelID + " #hfCaseId").val('');
        CacheManager.BindPatientData('GetPatientCase', true, PatientId).done(function (result) {
            $('#' + Patient_Information_Import.params.PanelID + " input#txtCaseNumber").autocomplete({
                autoFocus: true,
                source: PatientCase, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#" + Patient_Information_Import.params.PanelID + " #hfCaseId").val(ui.item.id); // add the selected id
                        if ($("#" + Patient_Information_Import.params.PanelID + " #lnkCaseNumberEdit").css("display") == "none") {
                            $("#" + Patient_Information_Import.params.PanelID + " #lnkCaseNumberEdit").css("display", "inline");
                            $("#" + Patient_Information_Import.params.PanelID + " #lblCaseNumber").css("display", "none");
                        }
                    }, 100);
                }
            });
            if (Patient_Information_Import.params.ParentCtrl == "Patient_Document" && Patient_Information_Import.params.PatientDetail == "1") {
                if (Patient_Information_Import.params["CaseNo"] != null && Patient_Information_Import.params["CaseNo"] != "") {
                    $("#" + Patient_Information_Import.params["PanelID"] + " #frmDocumentImport #txtCaseNumber").val(Patient_Information_Import.params.CaseNo);
                    $("#" + Patient_Information_Import.params["PanelID"] + " #frmDocumentImport #hfCaseId").val(Patient_Information_Import.params.CaseId);
                }
            }
        });

    },

    ImportSave: function () {
        var self = $("#" + Patient_Information_Import.params.PanelID + " #frmDocumentImport");
        var myJSON = self.getMyJSON();
        Patient_Information_Import.ImportSave_DBCall(myJSON).done(function (response) {
            if (response.status != false) {
                Patient_Information_Submission.InformationSearch();
                Patient_Document.DocumentSearch();
                Patient_Document.LoadFolders();
                var ListItemId = Patient_Information_Import.params.FolderId;
                Patient_Document.LoadFoldersAfterDeleteOrUpdateRecords().done(function () {
                    ListItemHighLight(ListItemId);
                });
                utility.DisplayMessages(response.Message, 1);
                Patient_Information_Import.UnLoad();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ImportSave_DBCall: function (PatientDocumentData) {
        var data = "PatientDocumentData=" + PatientDocumentData + "&PatientId=" + Patient_Information_Import.params.PatientId + "&PatPortalDocId=" + Patient_Information_Import.params.PatPortalDocId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "SAVE_PATIENT_INFORMATION_SUBMISSION");
    },

    BindClaimNumber: function () {
        $("#" + Patient_Information_Import.params.PanelID + " #txtClaimNumber").autocomplete({
            autoFocus: true,
            source: function (request, response) {
                var ClaimNumber = $('#' + Patient_Information_Import.params.PanelID + ' #txtClaimNumber').val();
                var patientId = $("#" + Patient_Information_Import.params.PanelID + " #hfPatientId").val();
                if (ClaimNumber.length > 2) {
                    Patient_Information_Import.LoadClaimNumers(ClaimNumber, patientId).done(function (responseData) {
                        if (responseData.status != false) {
                            if (responseData.ClaimsCount > 0) {
                                var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                                var AllClaimsVisits = [];
                                $.each(Claims, function (i, item) {
                                    AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                                });
                                response(AllClaimsVisits);
                            }
                        }
                    });
                }
            },

            select: function (event, ui) {
                setTimeout(function () {
                    $("#" + Patient_Information_Import.params.PanelID + " #hfVisitId").val(ui.item.id);
                    $("#" + Patient_Information_Import.params.PanelID + " #hfPatientId").val(ui.item.PatientId);
                    $("#" + Patient_Information_Import.params.PanelID + " #txtAccountNumber").val(ui.item.PatientName);
                    $("#" + Patient_Information_Import.params.PanelID + " #txtClaimNumber").val(ui.item.ClaimNumber);
                    Patient_Information_Import.LoadPatientCase(ui.item.PatientId);
                }, 100);
            }
        });
    },

    LoadClaimNumers: function (claimNumber, patientID) {
        var data = "ClaimNumber=" + claimNumber + "&PatientID=" + patientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_BATCHCHARGE_DETAIL", "SEARCH_VISIT_CLAIM");
    },

    OpenCaseDetail: function (HiddenCtrl) {
        var params = [];
        params["CaseId"] = parseInt($('#' + Patient_Information_Import.params["PanelID"] + ' #' + HiddenCtrl).val());
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["PatientId"] = $("#" + Patient_Information_Import.params["PanelID"] + " #hfPatientId").val();
        params["RefCtrl"] = "txtCaseNumber";
        params["ParentCtrl"] = "Patient_Information_Import";
        LoadActionPan('Patient_Case_Detail', params);
    },

    OpenCase: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        var PanelID = Patient_Information_Import.params["TabID"];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["CaseId"] = "-1";
        params["patientID"] = $("#" + Patient_Information_Import.params.PanelID + " #hfPatientId").val();
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Information_Import";
        LoadActionPan('Patient_Case', params);
    },

    UnLoad: function () {
        if (Patient_Information_Import.params && Patient_Information_Import.params.ParentCtrl) {
            UnloadActionPan(Patient_Information_Import.params.ParentCtrl, 'Patient_Information_Import');
        }
        else
            UnloadActionPan(null, 'Patient_Information_Import');
    },

}