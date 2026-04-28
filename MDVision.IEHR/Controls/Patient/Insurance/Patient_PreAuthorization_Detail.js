Patient_PreAuthorization_Detail = {
    params: [],

    Load: function (params) {
        Patient_PreAuthorization_Detail.params = params;
        //var self = $('#pnlPreAuthorizationDetail');
        //self.loadDropDowns(true);

        Patient_PreAuthorization_Detail.BindInsurancePlans(Patient_PreAuthorization_Detail.params.PlanResponse.PatientInsuranceLoad_JSON);

        Patient_PreAuthorization_Detail.LoadPreAuthorizationsDetail();

    },

    BindAutoComplete: function (element) {


        var hiddenCrtl = $("#pnlPreAuthorizationDetail #hfcptcode");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Patient_PreAuthorization_Detail", null, true);

        //if (globalAppdata['IMO_ID'] == "") {
        //    CacheManager.BindAutoCompleteText('#pnlPreAuthorizationDetail #txtCPTCode', 'GetCPTCode', true, '#pnlPreAuthorizationDetail #hfcptcode', "");
        //}
        //else {
        //    utility.BindAutoCompleteText('#pnlPreAuthorizationDetail #txtCPTCode', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#pnlPreAuthorizationDetail #hfcptcode', "");
        //}

    },
    LoadPreAuthorizationsDetail: function () {
        AppPrivileges.GetFormPrivileges("Authorization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //Patient_PreAuthorization_Detail.FillPatientAccount(Patient_PreAuthorization_Detail.params.PatientID);
                if (Patient_PreAuthorization_Detail.params.mode == "Add") {

                    // Begin Jan 5th, 2015, Author: Abdur Rehman Latif, No ticket Change for Production
                    $("#frmPreAuthorizationDetail #chkActive").prop("checked", true);
                    if ($('#frmPreAuthorizationDetail #ddlPlan option').length == 2) {
                        $($("#frmPreAuthorizationDetail #ddlPlan option")[1]).prop('selected', true);
                        //azam aftab dated Jan 13,2015 PMS-3381
                        $("#frmPreAuthorizationDetail #ddlPlan").trigger("onchange");
                        //end dated Jan 13,2015 PMS-3381
                    }
                    else {
                        $($("#frmPreAuthorizationDetail #ddlPlan option")[0]).prop('selected', true);

                    }
                    // End Jan 5th, 2015, Author: Abdur Rehman Latif, No ticket Change for Production
                    //serialize data
                    $('#frmPreAuthorizationDetail').data('serialize', $('#frmPreAuthorizationDetail').serialize());
                    Patient_PreAuthorization_Detail.ValidatePreAuthorization();
                }
                else if (Patient_PreAuthorization_Detail.params.mode == "Edit") {
                    Patient_PreAuthorization_Detail.PreAuthorizationFill(Patient_PreAuthorization_Detail.params.PreAuthorizationId).done(function (response) {
                        if (response.status != false) {
                            var preAuthorization_detail = JSON.parse(response.PatientAuthorizationFill_JSON);
                            var self = $("#pnlPreAuthorizationDetail");
                            utility.bindMyJSON(true, preAuthorization_detail, false, self).done(function () {

                                if (preAuthorization_detail.chkActive == 'True')
                                    $("#pnlPreAuthorizationDetail #chkActive").attr("checked", true);
                                else
                                    $("#pnlPreAuthorizationDetail #chkActive").attr("checked", false);
                                var priority = $("#frmPreAuthorizationDetail #ddlPlan option:selected").attr("priority");
                                if (priority != "") {
                                    $("#frmPreAuthorizationDetail #txtProirity").val(priority);
                                }
                                //    utility.RemoveTimeFromDate("#pnlPreAuthorizationDetail #dtpFromDate", $("#pnlPreAuthorizationDetail #dtpFromDate").val());
                                //   utility.RemoveTimeFromDate("#pnlPreAuthorizationDetail #dtpToDate", $("#pnlPreAuthorizationDetail #dtpToDate").val());



                                //serialize data
                                $('#frmPreAuthorizationDetail').data('serialize', $('#frmPreAuthorizationDetail').serialize());
                                Patient_PreAuthorization_Detail.ValidatePreAuthorization();

                            });

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },

    CallCityState: function (control, field1, field2) {
        var zipcode = $('#frmPreAuthorizationDetail ' + control).val();
        var cityname = null;
        var statename = null;
        providerDetail.FillCityState(zipcode, cityname, statename).done(function (response) {
            if (response.status != false) {
                var citystate = JSON.parse(response.CITYSTATE_JSON);
                $('#frmPreAuthorizationDetail ' + field1).val(citystate.txtCity);
                $('#frmPreAuthorizationDetail ' + field2).val(citystate.txtState);
            }
            else {
                $('#frmPreAuthorizationDetail ' + field1).val('');
                $('#frmPreAuthorizationDetail ' + field2).val('');
            }
        });
    },

    ResetHiddenValue: function () {
        var $cptCode = $('#frmPreAuthorizationDetail #txtCPTCode');
        $cptCode.val($cptCode.val().replace(/[^a-zA-Z0-9]/g, function () {
            return '';
        }));
        $('#frmPreAuthorizationDetail #hfcptcode').val("-1");
    },

    ValidatePreAuthorization: function () {
        $('#frmPreAuthorizationDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  InsurancePlan: {
                      group: '.col-sm-3',
                      enabled: true,
                      validators: {

                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  CPTCode: {
                      group: '.col-sm-3',
                      enabled: true,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },

                  PAN: {
                      group: '.col-sm-4',
                      enabled: true,
                      validators: {

                          notEmpty: {
                              message: ''
                          },
                      }
                  }
              }
          })
            .on('blur', 'input#txtCPTCode', function (e) {
                var formValidation = $('#frmPreAuthorizationDetail').data("bootstrapValidator");
                switch ($(this).attr("name")) {
                    case 'CPTCode':
                        var OccurenceCod1Val = $("#frmPreAuthorizationDetail #txtCPTCode").val();
                        //if (OccurenceCod1Val != "") {
                        utility.ValidateAutoComplete(this, 'pnlPreAuthorizationDetail #hfcptcode', false, 1)
                        //   }
                        break;
                    default:
                        break;
                }
            })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Patient_PreAuthorization_Detail.SavePreAuthorization();
       });
    },

    FillPreAuthorization: function (PreAuthorizationId) {
        Patient_PreAuthorization_Detail.params["PreAuthorizationId"] = PreAuthorizationId;
        AppPrivileges.GetFormPrivileges("Authorization", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_PreAuthorization_Detail.PreAuthorizationFill(PreAuthorizationId).done(function (response) {
                    if (response.status != false) {
                        var Authorization_detail = JSON.parse(response.PatientAuthorizationFill_JSON);
                        var self = $("#pnlPreAuthorizationDetail");

                        utility.bindMyJSON(true, Authorization_detail, false, self).done(function () {
                            if (Authorization_detail.chkActive == 'True')
                                $("#pnlPreAuthorizationDetail #chkActive").attr("checked", true);
                            else
                                $("#pnlPreAuthorizationDetail #chkActive").attr("checked", false);
                            var priority = $("#frmPreAuthorizationDetail #ddlPlan option:selected").attr("priority");
                            if (priority != "") {
                                $("#frmPreAuthorizationDetail #txtProirity").val(priority);
                            }
                            utility.RemoveTimeFromDate("#pnlPreAuthorizationDetail #dtpFromDate", $("#pnlPreAuthorizationDetail #dtpFromDate").val());
                            utility.RemoveTimeFromDate("#pnlPreAuthorizationDetail #dtpToDate", $("#pnlPreAuthorizationDetail #dtpToDate").val());
                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FillPatientAccount: function (PatientId) {
        appointmentDetail.FillPatient(PatientId).done(function (response) {
            if (response.status != false) {
                var patient_detail = JSON.parse(response.PatientFill_JSON);

                Patient_PreAuthorization_Detail.BindInsurancePlans(response.PatientInsurance_JSON);
                //appointmentDetail.ValidateAppointment();
                // $('#appointmentDetail').data('serialize', $('#appointmentDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    FillInsuranceData: function (control, controlToChange) {

        var priority = $("#frmPreAuthorizationDetail #ddlPlan option:selected").attr("priority");
        $("#frmPreAuthorizationDetail #txtProirity").val(priority);


    },

    BindInsurancePlans: function (InsuranceJSON) {
        var InsuranceJSON_detail = JSON.parse(InsuranceJSON);
        $("#frmPreAuthorizationDetail #ddlPlan").empty();
        $("#frmPreAuthorizationDetail #ddlPlan").append($('<option/>', {
            value: "",
            html: "- SELECT -"
        }));
        $.each(InsuranceJSON_detail, function (i, item) {
            //Added by Azeem Raza Tayyab on 11-Apr-2016 to Fix bug# PMS-4816
            if (item.IsActive.toLowerCase() == "true") {
                $("#frmPreAuthorizationDetail #ddlPlan").append(
                    $('<option />', {
                        value: item.InsurancePlanId,
                        html: item.InsurancePlanName,
                        priority: item.PlanPriority
                    })
                );
            }
        });

    },

    // -------------- CPT Code -----------------

    OpenCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_PreAuthorization_Detail";
        params["RefHiddenCtrl"] = "hfcptcode";
        params["RefCtrl"] = "txtCPTCode";
        LoadActionPan('Admin_IMOCPT', params);
    },

    // -------------- End CPT Code -------------

    SavePreAuthorization: function () {
        var strMessage = "";
        var self = $("#pnlPreAuthorizationDetail");
        var myJSON = self.getMyJSON();

        if (Patient_PreAuthorization_Detail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Authorization", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var result;
                    result = Patient_PreAuthorization_Detail.ValidateValues();
                    if (result != false) {
                        Patient_PreAuthorization_Detail.PreAuthorizationSave(myJSON).done(function (response) {
                            if (response.status != false) {
                                Patient_PreAuthorization_Detail.params["PreAuthorizationId"] = response.PreAuthorizationId;
                                Patient_PreAuthorization_Detail.params["mode"] = "Edit";
                                Patient_PreAuthorization.LoadPreAuthorizations();
                                UnloadActionPan(Patient_PreAuthorization_Detail.params["ParentCtrl"]);
                                utility.DisplayMessages(response.message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (Patient_PreAuthorization_Detail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Authorization", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_PreAuthorization_Detail.PreAuthorizationUpdate(myJSON, Patient_PreAuthorization_Detail.params.PreAuthorizationId).done(function (response) {
                        if (response.status != false) {
                            if (Patient_PreAuthorization_Detail.params.ParentCtrl && Patient_PreAuthorization_Detail.params.ParentCtrl.indexOf("EncounterChargeCapture") > -1) {
                                UnloadActionPan(Patient_PreAuthorization_Detail.params["ParentCtrl"]);
                            }
                            else {
                                Patient_PreAuthorization.LoadPreAuthorizations();
                                UnloadActionPan(Patient_PreAuthorization_Detail.params["ParentCtrl"]);
                            }
                            utility.DisplayMessages(response.message, 1);
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

    ValidateValues: function () {
        if ($('#pnlPreAuthorizationDetail #hfcptcode').val() == "-1") {
            utility.DisplayMessages("CPT code not Valid", 2);
            return false;
        }
        else
            return true;
    },

    PreAuthorizationFill: function (PreAuthorizationID) {
        var data = "PatientID=" + Patient_PreAuthorization_Detail.params.PatientID + "&PreAuthorizationID=" + PreAuthorizationID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_PREAUTHORIZATION_DETAIL", "FILL_PREAUTHORIZATION");
    },

    PreAuthorizationSave: function (PreAuthorizationData) {
        var data = "PreAuthorizationData=" + PreAuthorizationData + "&PatientID=" + Patient_PreAuthorization_Detail.params.PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_PREAUTHORIZATION_DETAIL", "SAVE_PREAUTHORIZATION");
    },

    PreAuthorizationUpdate: function (PreAuthorizationData, PreAuthorizationID) {
        var data = "PreAuthorizationData=" + PreAuthorizationData + "&PatientID=" + Patient_PreAuthorization_Detail.params.PatientID + "&PreAuthorizationID=" + PreAuthorizationID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_PREAUTHORIZATION_DETAIL", "UPDATE_PREAUTHORIZATION");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmPreAuthorizationDetail', function () {
            if (Patient_PreAuthorization_Detail.params != null && Patient_PreAuthorization_Detail.params.ParentCtrl != null) {
                UnloadActionPan(Patient_PreAuthorization_Detail.params.ParentCtrl, 'Patient_PreAuthorization');
            }
            else
                UnloadActionPan(null, 'Patient_PreAuthorization');
        }, function () {
            if (Patient_PreAuthorization_Detail.params != null && Patient_PreAuthorization_Detail.params.ParentCtrl != null) {
                UnloadActionPan(Patient_PreAuthorization_Detail.params.ParentCtrl, 'Patient_PreAuthorization');
            }
            else
                UnloadActionPan(null, 'Patient_PreAuthorization');
        });


    },


}