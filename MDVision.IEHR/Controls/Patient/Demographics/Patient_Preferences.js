Patient_Preferences = {
    params: [],
    Enable: false,
    Load: function (params) {
        Patient_Preferences.params = params;
        var self = $('#pnlPatientPreferences');
        //self.loadDropDowns(true);

        self.loadDropDowns(true).done(function () {

            Patient_Preferences.LoadPreferences();
            Patient_Preferences.LoadAllAutocomplete();
        });


    },
    checkPreference: function (val) {
        var WorkTel = Patient_Preferences.params.PatWorkTel;
        var Cell = Patient_Preferences.params.PatCell;
        var HomeTel = Patient_Preferences.params.PatHomeTel;
        var ddlID = $(val).attr("id");
        //WorkTel == "" && Cell == "" && HomeTel == ""
        if (($("#" + ddlID + " option:selected").text() == "Text" || $("#" + ddlID + " option:selected").text() == "Phone") && (WorkTel == "" && Cell == "" && HomeTel == "")) {
            //if ($(val).val()) {
            //    if ($('#pnlPatientPreferences #ddl1stPreference').val() == $('#pnlPatientPreferences #ddl2ndPreference').val()) {
            //        $(val).val("");
            //        utility.DisplayMessages("Preferences can not be same", 3);
            //    }
            //}
            utility.DisplayMessages("Phone number missing", 2);
            $(val).val("");
        } else {
            if ($(val).val()) {
                if ($('#pnlPatientPreferences #ddl1stPreference').val() == $('#pnlPatientPreferences #ddl2ndPreference').val()) {
                    $(val).val("");
                    utility.DisplayMessages("Preferences can not be same", 3);
                }
            }
        }

    },
    LoadAllAutocomplete: function () {
        CacheManager.BindCodes('GetSchool', true).done(function (result) {
            $("#pnlPatientPreferences input#txtSchool").autocomplete({
                autoFocus: true,
                source: Schools, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#pnlPatientPreferences #hfSchool").val(ui.item.id); // add the selected id
                        if ($("#pnlPatientPreferences #lnkSchoolDetail").css("display") == "none") {
                            $("#pnlPatientPreferences #lnkSchoolDetail").css("display", "inline");
                            $("#pnlPatientPreferences #lblSchool").css("display", "none");
                        }
                    }, 100);
                }
            });
        });

       // utility.CreateDatePicker('pnlPatientPreferences #frmPatientPreferences #dtpDateOfDeath', null, false);
    },
    EnableDisableoptions: function (val) {

        if ($(val).attr("val") == "true") {
            $(val).attr("val", "false");
            $("#pnlPatientPreferences #ddl1stPreference,#ddl2ndPreference,#chkcommnwithgrntr").prop("disabled", true);
            $('#pnlPatientPreferences #ddl1stPreference,#ddl2ndPreference').val("");
            $('#pnlPatientPreferences #chkcommnwithgrntr').prop('checked', false);
        } else {
            $(val).attr("val", "true");
            $("#pnlPatientPreferences #ddl1stPreference,#ddl2ndPreference,#chkcommnwithgrntr").prop("disabled", false);
        }
    },
    LoadPreferences: function () {
        AppPrivileges.GetFormPrivileges("Preferences", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Preferences.FillPatientPreferences().done(function (response) {
                    if (response.status != false) {
                        var preferences_detail = JSON.parse(response.PreferencesFill_JSON);
                        var self = $("#frmPatientPreferences");
                        utility.bindMyJSON(true, preferences_detail, false, self).done(function () {

                            if (preferences_detail.chkPatientStatement == 'True')
                                $("#pnlPatientPreferences #chkPatientStatement").attr("checked", true);
                            else
                                $("#pnlPatientPreferences #chkPatientStatement").attr("checked", false);

                            if (preferences_detail.chkcommnwithgrntr == 'True')
                                $("#pnlPatientPreferences #chkcommnwithgrntr").attr("checked", true);
                            else
                                $("#pnlPatientPreferences #chkcommnwithgrntr").attr("checked", false);

                            if (preferences_detail.chkcommnoptout == 'True') {
                                $("#pnlPatientPreferences #chkcommnoptout").attr("checked", true);
                                $("#pnlPatientPreferences #ddl1stPreference,#ddl2ndPreference,#chkcommnwithgrntr").prop("disabled", true);
                                $('#pnlPatientPreferences #ddl1stPreference,#ddl2ndPreference').val("");
                                $("#pnlPatientPreferences #chkcommnoptout").attr("val", "false");
                            }

                            else
                                $("#pnlPatientPreferences #chkcommnoptout").attr("checked", false);

                            if (preferences_detail.txtSchool != "") {
                                if ($("#pnlPatientPreferences #lnkSchoolDetail").css("display") == "none") {
                                    $("#pnlPatientPreferences #lnkSchoolDetail").css("display", "inline");
                                    $("#pnlPatientPreferences #lblSchool").css("display", "none");
                                }
                            }

                            //utility.RemoveTimeFromDate("#pnlPatientPreferences #dtpDateOfDeath", preferences_detail.dtpDateOfDeath);
                            //Patient_Preferences.ValidatePatientPreferences();

                            //serialize Data after all controls loaded.
                            $('#frmPatientPreferences').data('serialize', $('#frmPatientPreferences').serialize());

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

    ValidatePatientPreferences: function () {
        $('#frmPatientPreferences')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Patient_Preferences.PatientPreferencesSave();
       });
    },

    PatientPreferencesSave: function () {
        var strMessage = "";
        var self = $("#frmPatientPreferences");
        var myJSON = self.getMyJSON();

        //if (Patient_Preferences.params.mode == "Add") {
        AppPrivileges.GetFormPrivileges("Demographic", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlPatientPreferences #hfSchool").val() == "-1") {
                    utility.DisplayMessages("School not Valid", 2);
                }
                else {
                    Patient_Preferences.UpdatePatientPreferences(myJSON).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Patient_Preferences.UnLoadTab('Patient_Preferences');
                            Patient_Demographic.LoadPatientDemogrphic();
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
        //}
    },

    // -------------- School ---------------------
    FillSchoolName: function (SchoolId, SchoolName) {
        Patient_Preferences.LoadAllAutocomplete();
        $("#pnlPatientPreferences #txtSchool").val(SchoolName).focus();
        $("#pnlPatientPreferences #hfSchool").val(SchoolId);
        UnloadActionPan(Patient_School.params["ParentCtrl"]);
    },

    OpenSchool: function () {
        var params = [];
        params["SchoolId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_Preferences';

        LoadActionPan('Patient_School', params);
    },

    OpenSchoolDetail: function () {
        //Patient_School.SchoolEdit($("#pnlPatientPreferences #hfSchool").val());

        var params = [];
        params["SchoolId"] = $("#pnlPatientPreferences #hfSchool").val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        //params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'Patient_Preferences';
        LoadActionPan('schoolDetail', params);
    },

    HideSchoolLink: function () {
        $("#pnlPatientPreferences #hfSchool").val("-1");
        $("#pnlPatientPreferences #lnkSchoolDetail").hide();
    },
    // -------------- End School -----------------

    //SavePatientPreferences: function (PatientPreferencesData) {
    //    var data = "PatientPreferencesData=" + PatientPreferencesData + "&PatientID=" + Patient_Preferences.params.patientID;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "PATIENT_PREFERENCES", "SAVE_PATIENT_PREFERENCES");
    //},

    UpdatePatientPreferences: function (PatientPreferencesData) {
        var data = "PatientPreferencesData=" + PatientPreferencesData + "&PatientID=" + Patient_Preferences.params.patientID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_PREFERENCES", "UPDATE_PATIENT_PREFERENCES");
    },

    FillPatientPreferences: function () {
        var data = "PatientID=" + Patient_Preferences.params.patientID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_PREFERENCES", "FILL_PATIENT_PREFERENCES");
    },

    UnLoadTab: function (Tab) {

        if (Tab) {

            if (Patient_Preferences.params.FromAdmin != "1") {
                if (Patient_Preferences.params != null && Patient_Preferences.params.ParentCtrl != null) {
                    UnloadActionPan(Patient_Preferences.params.ParentCtrl, 'Patient_Preferences');
                }
                else
                    UnloadActionPan(null, 'Patient_Preferences');
            }
            else
                RemoveAdminTab(Tab);

        }
        else {
            utility.UnLoadDialog('frmPatientPreferences', function () {
                if (Patient_Preferences.params.FromAdmin != "1") {
                    if (Patient_Preferences.params != null && Patient_Preferences.params.ParentCtrl != null) {
                        UnloadActionPan(Patient_Preferences.params.ParentCtrl, 'Patient_Preferences');
                    }
                    else
                        UnloadActionPan(null, 'Patient_Preferences');
                }
                else
                    RemoveAdminTab(Tab);
            }, function () {
                UnloadActionPan(Patient_Preferences.params.ParentCtrl, 'Patient_Preferences');
            });
        }
    },

    IsPatientHasGuarantor: function (obj) {
        if (obj.checked) {
            //alert("checked");
            if (Patient_Preferences.params.GuarantorID == "") {
                utility.DisplayMessages("Guarantor Missing", 2);
                $(obj).prop('checked', false);
            } else {
                guarantorDetail.FillGuarantor(Patient_Preferences.params.GuarantorID).done(function (response) {
                    if (response.status != false) {
                        var guarantor_detail = JSON.parse(response.GuarantorFill_JSON);
                        if (guarantor_detail.txtTelephone.replace(/[_\W]+/g, "") == "") {
                            utility.DisplayMessages("Guarantor’s Phone number missing", 2);
                            $(obj).prop('checked', false);
                        } else {

                        }
                    }
                    else {
                        //utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        } else {
            //alert("unchecked");
        }
    },
}
