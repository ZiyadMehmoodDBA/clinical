PatientEligibilityServiceDetail = {
    params: [],
    Load: function (params) {
        PatientEligibilityServiceDetail.params = params;
        $('#divInterval').hide();
        var self = null;
        if (PatientEligibilityServiceDetail.params.PanelID == "PatientEligibilityServiceDetail")
            self = $('#PatientEligibilityServiceDetail');
        else
            self = $('#' + PatientEligibilityServiceDetail.params.PanelID + ' #PatientEligibilityServiceDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {

            PatientEligibilityServiceDetail.LoadPatientEligibilityService();

        });

    },

    UnLoad: function () {

        utility.UnLoadDialog("frmPatientEligibilityServiceDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ModeChanged: function () {
        var txtMode = $('#ddlInterval :selected').text();
        var txtModeTrimed = $('#ddlInterval :selected').text().replace(/ /g, '');

        if (txtModeTrimed === 'Interval') {
            $('#divInterval').show();
        }
        else {
            $('#divInterval').hide();
        }

    },

    LoadPatientEligibilityService: function () {
        if (PatientEligibilityServiceDetail.params.mode == "Add") {
            $('#PatientEligibilityServiceDetail #txtShortName').attr("enabled", "enabled");

            $('input#txtClearingHoueUserPassword').removeAttr("type");
            $('input#txtClearingHoueUserPassword').prop('type', 'password');

              if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                  $("#PatientEligibilityServiceDetail #frmPatientEligibilityServiceDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                    }

              PatientEligibilityServiceDetail.ValidatePatientEligibilityService();

            //Serialize data
            setTimeout(function () {
                $('#frmPatientEligibilityServiceDetail').data('serialize', $('#frmPatientEligibilityServiceDetail').serialize());
                    }, "100");

        }
        else if (PatientEligibilityServiceDetail.params.mode == "Edit") {
            PatientEligibilityServiceDetail.FillPatientEligibilityService(PatientEligibilityServiceDetail.params.PatientEligibilityServiceID).done(function (response) {
                if (response.status != false) {


                    var PatientEligibilityService_detail = JSON.parse(response.PatientEligibilityServiceFill_JSON);
                    var self = $("#PatientEligibilityServiceDetail");

                    if (PatientEligibilityService_detail.ddlInterval == 'Interval')
                        $('#divInterval').show();
                    else
                        $('#divInterval').hide();

                    utility.bindMyJSON(true, PatientEligibilityService_detail, false, self).done(function () {

                        PatientEligibilityServiceDetail.ValidatePatientEligibilityService();
                        $('#frmPatientEligibilityServiceDetail').data('serialize', $('#frmPatientEligibilityServiceDetail').serialize());

                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidatePatientEligibilityService: function () {
        $('#frmPatientEligibilityServiceDetail')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    ClearingHouse: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Entity: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    ScheduleDays: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Mode: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Time: {
                        group: '.col-sm-4',
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
            PatientEligibilityServiceDetail.PatientEligibilityServiceSave();
        });
    },

    PatientEligibilityServiceSave: function () {
        var strMessage = "";
        var self = $("#PatientEligibilityServiceDetail");
        var myJSON = self.getMyJSON();
        if (PatientEligibilityServiceDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Patient Eligibility Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    PatientEligibilityServiceDetail.SavePatientEligibilityService(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_PatientEligibilityService.PatientEligibilityServiceSearch(response.PatientEligibilityServiceID);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetPatientEligibilityService', true);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (PatientEligibilityServiceDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Patient Eligibility Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    PatientEligibilityServiceDetail.UpdatePatientEligibilityService(myJSON, PatientEligibilityServiceDetail.params.PatientEligibilityServiceID).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Admin_PatientEligibilityService.PatientEligibilityServiceSearch(PatientEligibilityServiceDetail.params.PatientEligibilityServiceID);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetPatientEligibilityService', true);
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    SavePatientEligibilityService: function (PatientEligibilityServiceData) {
        var data = "PatientEligibilityServiceData=" + PatientEligibilityServiceData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PATIENT_ELIGIBILITY_SERVICE", "SAVE_PATIENT_ELIGIBILITY_SERVICE");
    },

    UpdatePatientEligibilityService: function (PatientEligibilityServiceData, PatientEligibilityServiceID) {
        var data = "PatientEligibilityServiceData=" + PatientEligibilityServiceData + "&PatientEligibilityServiceID=" + PatientEligibilityServiceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PATIENT_ELIGIBILITY_SERVICE", "UPDATE_PATIENT_ELIGIBILITY_SERVICE");
    },

    FillPatientEligibilityService: function (PatientEligibilityServiceID) {
        var data = "PatientEligibilityServiceID=" + PatientEligibilityServiceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PATIENT_ELIGIBILITY_SERVICE", "FILL_PATIENT_ELIGIBILITY_SERVICE");
    },

    UpdatePatientEligibilityServiceActiveInactive: function (PatientEligibilityServiceID, IsActive) {
        var data = "PatientEligibilityServiceID=" + PatientEligibilityServiceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PATIENT_ELIGIBILITY_SERVICE", "UPDATE_PATIENT_ELIGIBILITY_SERVICE_ACTIVE_INACTIVE");
    },

    ResetTimeDropDown: function () {
        var from = $('#PatientEligibilityServiceDetail #txtTime').val();

        var fromRes = from.substring(6, 8);

        if (fromRes == 'AM') {
            $("#PatientEligibilityServiceDetail #time1").val("AM");
        }
        else if (fromRes == 'PM') {
            $("#PatientEligibilityServiceDetail #time1").val("PM");
        }
        else {
            $("#PatientEligibilityServiceDetail #time1").val("ALL");
        }
    },
    ShowHistory: function () {
        var PanelID = 'PatientEligibilityServiceDetail';
        var ParentCtrl = 'PatientEligibilityServiceDetail';
        var ProfileName = 'Patient Eligibility Service';
        var DBTableName = 'PatientEligibilityService';
        var ColumnKeyId = PatientEligibilityServiceDetail.params.PatientEligibilityServiceID

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}