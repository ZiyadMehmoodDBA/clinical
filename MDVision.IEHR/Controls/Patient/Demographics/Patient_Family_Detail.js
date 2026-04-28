Patient_Family_Detail = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        Patient_Family_Detail.params = params;

        if (Patient_Family_Detail.bIsFirstLoad) {
            Patient_Family_Detail.bIsFirstLoad = false;
           // var self = $('#pnlFamilyDetail');
            var self = null;

            if (Patient_Family_Detail.params.PanelID != "pnlFamilyDetail")
                self = $('#' + Patient_Family_Detail.params.PanelID + ' #pnlFamilyDetail');
            else
                self = $('#pnlFamilyDetail');
            self.loadDropDowns(true).done(function () {
                $('#pnlFamilyDetail #ddlRelation option:contains(Self)').remove();
                $('#pnlFamilyDetail #hfLinkedPatientId').val('-1');
                $('#pnlFamilyDetail #txtLastName').keypress(function (e) {
                    var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                    if (keycode >= 48 && keycode <= 57) {
                        e.preventDefault();
                    }
                });
                $('#pnlFamilyDetail #txtFirstName').keypress(function (e) {
                    var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                    if (keycode >= 48 && keycode <= 57) {
                        e.preventDefault();
                    }
                });
                Patient_Family_Detail.ValidateFamily();
                utility.ValidateDOB(Patient_Family_Detail.params.PanelID + ' #frmFamilyDetail', Patient_Family_Detail.params.PanelID + ' #frmFamilyDetail #dtpDOB', new Date('1800-01-01'), new Date(), false);
                Patient_Family_Detail.BindProvider();
                Patient_Family_Detail.BindFacility();
                Patient_Family_Detail.LoadFamilyDetail();
            });
        }
    },

    LoadFamilyDetail: function () {
        AppPrivileges.GetFormPrivileges("Patient Family", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (Patient_Family_Detail.params.mode == "Add") {

                  //  Patient_Family_Detail.ValidateFamily();

                    //serialize Data after all controls loaded.
                    $('#frmFamilyDetail').data('serialize', $('#frmFamilyDetail').serialize());
                }
                else if (Patient_Family_Detail.params.mode == "Edit") {
                    Patient_Family_Detail.FamilyFill(Patient_Family_Detail.params.FamilyId).done(function (response) {
                        if (response.status != false) {
                            var familyContact_detail = JSON.parse(response.PatientFamilyFill_JSON);
                            var self = $("#pnlFamilyDetail");
                            if (familyContact_detail.hfRelationName.toLowerCase() == "other relationship")
                                $('#pnlFamilyDetail #divOtherRelationship').css('display', 'inline');
                            else
                                $('#pnlFamilyDetail #divOtherRelationship').css('display', 'none');
                            utility.bindMyJSON(true, familyContact_detail, false, self).done(function () {
                                utility.RemoveTimeFromDate("#pnlFamilyDetail #dtpDOB", familyContact_detail.dtpDOB);
                                utility.RemoveTimeFromDate("#pnlFamilyDetail #dtpDOD", familyContact_detail.dtpDOD);
                                if (familyContact_detail.chkAddAsPatient == "True") {
                                    Patient_Family_Detail.DisableAddAsPatientPanel(self);
                                }

                                //serialize Data after all controls loaded.
                                $('#frmFamilyDetail').data('serialize', $('#frmFamilyDetail').serialize());
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

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_Family_Detail';
        LoadActionPan('Patient_Search', params);
    },

    ValidateFamily: function () {
        $('#' + Patient_Family_Detail.params.PanelID + ' #frmFamilyDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  LastName: {
                      group: '.col-sm-3',
                      enabled: true,
                      validators: {

                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  FirstName: {
                      group: '.col-xs-8',
                      enabled: true,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Relation: {
                      group: '.col-sm-3',
                      enabled: true,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  DOB: {
                      group: '.col-sm-3',
                      enabled: true,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Email: {
                      group: '.col-sm-3',
                      enabled: false,
                      validators: {
                          regexp: {
                              regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                              message: 'Email not Valid'
                          }

                      }
                  },
                  OtherRelationship: {
                      group: '.col-sm-3',
                      enabled: true,
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Patient_Family_Detail.SaveFamily();
       }).on('keyup', 'input#txtEmail', function (e) {
           var formValidation = $('#' + Patient_Family_Detail.params.PanelID + ' #frmFamilyDetail').data("bootstrapValidator");
           switch ($(this).attr("name")) {
               case 'Email':
                   var email = $("input#txtEmail").val();
                   if (email != "") {
                       formValidation.enableFieldValidators('Email', true);
                   }
                   else
                       formValidation.enableFieldValidators('Email', false);
                   break;
           }
       });
    },

    SaveFamily: function () {
        var strMessage = "";
        var self = $("#pnlFamilyDetail");
        var myJSON = self.getMyJSON();

        if (Patient_Family_Detail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Patient Family", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_Family_Detail.FamilySave(myJSON).done(function (response) {
                        if (response.status != false) {
                            Patient_Family_Detail.params["FamilyId"] = response.FamilyID;
                            Patient_Family_Detail.params["mode"] = "Edit";

                            Patient_Family.LoadFamilies();
                            UnloadActionPan(Patient_Family_Detail.params["ParentCtrl"]);
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
        else if (Patient_Family_Detail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Patient Family", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Patient_Family_Detail.FamilyUpdate(myJSON, Patient_Family_Detail.params.FamilyId).done(function (response) {
                        if (response.status != false) {
                            Patient_Family.LoadFamilies();
                            UnloadActionPan(Patient_Family_Detail.params["ParentCtrl"]);
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

    FillFamily: function (FamilyId) {
        Patient_Family_Detail.params["FamilyId"] = FamilyId;
        AppPrivileges.GetFormPrivileges("Patient Family", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Family_Detail.FamilyFill(FamilyId).done(function (response) {
                    if (response.status != false) {
                        var Family_detail = JSON.parse(response.PatientFamilyFill_JSON);
                        var self = $("#pnlFamilyDetail");
                        utility.bindMyJSON(true, Patient_Family_Detail, false, self);
                        utility.RemoveTimeFromDate("#pnlFamilyDetail #dtpDOB", $("#pnlFamilyDetail #dtpDOB").val());
                        Patient_Family_Detail.params["mode"] = "Edit";
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

    FillPatientInfo: function (PatientId, IsFromSearch) {
        AppPrivileges.GetFormPrivileges("Patient Family", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Family_Detail.PatientInfoFill(PatientId, IsFromSearch).done(function (response) {
                    if (response.status != false) {
                        var patientinfo_detail = JSON.parse(response.PatientInfoFill_JSON);
                        var self = $("#pnlFamilyDetail");
                        utility.bindMyJSON(true, patientinfo_detail, false, self);
                        $('#' + Patient_Family_Detail.params.PanelID + ' #frmFamilyDetail').bootstrapValidator('revalidateField', 'LastName');
                        $('#' + Patient_Family_Detail.params.PanelID + ' #frmFamilyDetail').bootstrapValidator('revalidateField', 'FirstName');
                        $('#' + Patient_Family_Detail.params.PanelID + ' #frmFamilyDetail').bootstrapValidator('revalidateField', 'Relation');
                        if (IsFromSearch == 1) {
                            self.find("#chkAddAsPatient").prop('checked', true);
                            Patient_Family_Detail.setProviderFacilityDetails(PatientId);
                            Patient_Family_Detail.DisableAddAsPatientPanel(self);
                        }
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

    FillSameAsPatientInfo: function () {
        Patient_Family_Detail.FillPatientInfo(Patient_Family_Detail.params.PatientID, 0)
    },

    FillPatientInfoFromSearch: function (PatientId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        Patient_Family_Detail.FillPatientInfo(PatientId, 1);
        UnloadActionPan(Patient_Search.params["ParentCtrl"]);
        utility.InsertRecentPatient(PatientId);
    },

    FamilySave: function (FamilyData) {
        var data = "FamilyData=" + FamilyData + "&PatientID=" + Patient_Family_Detail.params.PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_FAMILY_DETAIL", "SAVE_FAMILY");
    },

    FamilyUpdate: function (FamilyData, FamilyID) {
        var data = "FamilyData=" + FamilyData + "&PatientID=" + Patient_Family_Detail.params.PatientID + "&FamilyID=" + FamilyID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_FAMILY_DETAIL", "UPDATE_FAMILY");
    },

    FamilyFill: function (FamilyID) {
        var data = "PatientID=" + Patient_Family_Detail.params.PatientID + "&FamilyID=" + FamilyID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_FAMILY_DETAIL", "FILL_FAMILY");
    },

    PatientInfoFill: function (PatientId, IsFromSearch) {
        var data = "PatientID=" + PatientId + "&IsFromSearch=" + IsFromSearch;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_FAMILY_DETAIL", "FILL_PATIENT_INFO");
    },

    UnLoad: function (Tab) {

        utility.UnLoadDialog('frmFamilyDetail', function () {
            UnloadActionPan(Patient_Family_Detail.params["ParentCtrl"]);
        }, function () {
            UnloadActionPan(Patient_Family_Detail.params["ParentCtrl"]);
        });



    },

    GetSelectedRelationship: function (ev) {
        if ($(ev.selectedOptions).text().toLowerCase() == "other relationship") {
            $('#pnlFamilyDetail #divOtherRelationship').css('display', 'inline');
            $('#pnlFamilyDetail #txtOtherRelation').val('');
            $('#pnlFamilyDetail #txtOtherRelation').text('');
        }
        else {
            $('#pnlFamilyDetail #divOtherRelationship').css('display', 'none');
            $('#pnlFamilyDetail #txtOtherRelation').val('');
            $('#pnlFamilyDetail #txtOtherRelation').text('');
        }
    },

    // -------------- Provider ---------------------
    OpenProvider: function () {
        var params = [];
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Family_Detail";
        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#pnlFamilyDetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'Patient_Family_Detail';
        LoadActionPan('providerDetail', params);
    },
    BindProvider: function (isFullName, shortName) {
        var Ctrl = $("#" + Patient_Family_Detail.params.PanelID + " #frmFamilyDetail #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Family_Detail.params.PanelID + " #frmFamilyDetail #hfProvider");
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },
    // -------------- End Provider -----------------

    // -------------- Facility ---------------------
    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Patient_Family_Detail";
        LoadActionPan('Admin_Facility', params);
    },
    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#pnlFamilyDetail #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Patient_Family_Detail';
        params["RefCtrl"] = "txtFacility";
        LoadActionPan('facilityDetail', params);
    },
    BindFacility: function () {
        var Ctrl = $("#" + Patient_Family_Detail.params.PanelID + " #frmFamilyDetail #txtFacility");
        var func = function () { return utility.GetFacilityDescriptionArray(Ctrl.val()) };
        var hfCtrl = $("#" + Patient_Family_Detail.params.PanelID + " #frmFamilyDetail #hfFacility");
        var onSelect = function (e) {
            $("#" + Patient_Family_Detail.params.PanelID + " #frmFamilyDetail #txtPractice").val(e.Practice);
            $("#" + Patient_Family_Detail.params.PanelID + " #frmFamilyDetail #hfPractice").val(e.PracticeId);
        }
        var onChange = function (valid) {
            if (Ctrl.val() != "" && $("#" + Patient_Family_Detail.params.PanelID + " #txtPractice").val() == "") {
                if (!valid) {
                    $("#" + Patient_Family_Detail.params.PanelID + " #txtPractice").val("");
                    $("#" + Patient_Family_Detail.params.PanelID + " #hfPractice").val("");
                }
            }
            if ($(Ctrl).val() == "") {
                $("#" + Patient_Family_Detail.params.PanelID + " #txtPractice").val("");
                $("#" + Patient_Family_Detail.params.PanelID + " #hfPractice").val("");
                $("#" + Patient_Family_Detail.params.PanelID + " #lnkFacilityEdit").css("display", "none");
                $("#" + Patient_Family_Detail.params.PanelID + " #lblFacility").css("display", "inline");
            }
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },
    // -------------- End Facility -----------------
    HideShowProviderFacilityPanel: function (obj) {
        if ($(obj).prop("checked")) {
            $("#" + Patient_Family_Detail.params.PanelID + " #DisplayAddAsPatient").css("display", "inline");
            Patient_Family_Detail.setProviderFacilityDetails(Patient_Family_Detail.params.PatientID);
        }
        else {
            $("#" + Patient_Family_Detail.params.PanelID + " #DisplayAddAsPatient").css("display", "none");
            var self = $("#" + Patient_Family_Detail.params.PanelID + " #frmFamilyDetail");
            self.find("#txtFacility").val('');
            self.find("#hfFacility").val('');
            self.find("#txtProvider").val('');
            self.find("#hfProvider").val('');
            self.find("#txtPractice").val('');
            self.find("#hfPractice").val('');
        }
    },
    setProviderFacilityDetails: function (PatientId) {
        //sets Provider, Facility and Practice of patient in Family while adding/updating. It only sets if AddAsPatient is checked.
        Patient_Demographic.FillDemographic(PatientId).done(function (response) {
            if (response.status != false) {
                var demographic_detail = JSON.parse(response.DemographicFill_JSON);
                var self = $("#" + Patient_Family_Detail.params.PanelID + " #frmFamilyDetail");

                //Facility
                self.find("#txtFacility").val(demographic_detail.Facility);
                self.find("#hfFacility").val(demographic_detail.FacilityID);
                $Ctrl = self.find("#txtFacility");
                $hfCtrl = self.find("#hfFacility");
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl, $Ctrl.val(), $hfCtrl, $hfCtrl.val());
                self.find("#txtFacility").focus();

                //Provider
                self.find("#txtProvider").val(demographic_detail.Provider);
                self.find("#hfProvider").val(demographic_detail.ProviderID);
                $Ctrl_p = self.find("#txtProvider");
                $hfCtrl_p = self.find("#hfProvider");
                utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());
                self.find("#txtProvider").focus();

                //Practice
                self.find("#txtPractice").val(demographic_detail.Practice);
                self.find("#hfPractice").val(demographic_detail.PracticeID);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    DisableAddAsPatientPanel: function (self) {
        $("#" + Patient_Family_Detail.params.PanelID + " #DisplayAddAsPatient").css("display", "inline");
        self.find("#chkAddAsPatient").attr('disabled', true);

        //Provider
        self.find("#lnkProvider").addClass('disableAll');
        self.find("#txtProvider").addClass('disableAll');
        $Ctrl_p = self.find("#txtProvider");
        $hfCtrl_p = self.find("#hfProvider");
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl_p, $Ctrl_p.val(), $hfCtrl_p, $hfCtrl_p.val());
        self.find("#txtProvider").focus();

        //Facility
        self.find("#txtFacility").addClass('disableAll');
        self.find("#lnkFacility").addClass('disableAll');
        $Ctrl = self.find("#txtFacility");
        $hfCtrl = self.find("#hfFacility");
        utility.SetKendoAutoCompleteSourceforValidate($Ctrl, $Ctrl.val(), $hfCtrl, $hfCtrl.val());
        self.find("#txtFacility").focus();
    },
}
