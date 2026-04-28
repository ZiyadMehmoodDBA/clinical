emergencyContactDetail = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {
        emergencyContactDetail.params = params;
        //utility.ValidateDOB('frmEmergencyContactDetail', 'emergencyContactDetail #dtpDOB', new Date('1800-01-01'), new Date(), true);


        if (emergencyContactDetail.bIsFirstLoad) {
            emergencyContactDetail.bIsFirstLoad = false;
            var self = null
            if (emergencyContactDetail.params.PanelID != "emergencyContactDetail") {
                self = $('#' + emergencyContactDetail.params.PanelID + ' #emergencyContactDetail');
            }
            else {
                self = $('#' + emergencyContactDetail.params.PanelID);
            }
            self.loadDropDowns(true).done(function () {
                self.find("#ddlRelation option[value='8']").remove();
            });
            emergencyContactDetail.ValidateEmergencyContactDetail();
        }
        emergencyContactDetail.LoadEmergencyContactDetail();
    },

    LoadEmergencyContactDetail: function () {
        AppPrivileges.GetFormPrivileges("Emergency Contact", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (emergencyContactDetail.params.mode == "Add") {

                    //serialize Data after all controls loaded.
                    $('#frmEmergencyContactDetail').data('serialize', $('#frmEmergencyContactDetail').serialize());
                 //   emergencyContactDetail.ValidateEmergencyContactDetail();
                }
                else if (emergencyContactDetail.params.mode == "Edit") {
                    emergencyContactDetail.EmergencyContactFill(emergencyContactDetail.params.PatientID, emergencyContactDetail.params.ContactId).done(function (response) {
                        if (response.status != false) {
                            var emergencyContact_detail = JSON.parse(response.EmergencyContactsFill_JSON);
                            var self = $("#emergencyContactDetail");
                            utility.bindMyJSON(true, emergencyContact_detail, false, self).done(function () {
                                //utility.RemoveTimeFromDate("#emergencyContactDetail #dtpDOB", emergencyContact_detail.dtpDOB);
                            //    emergencyContactDetail.ValidateEmergencyContactDetail();
                                //serialize Data after all controls loaded.
                                $('#frmEmergencyContactDetail').data('serialize', $('#frmEmergencyContactDetail').serialize());
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
        params["ParentCtrl"] = 'emergencyContactDetail';
        LoadActionPan('Patient_Search', params);
    },

    ValidateEmergencyContactDetail: function () {
        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];
       date_format=date_format.toUpperCase()

        $('#frmEmergencyContactDetail')
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
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   FirstName: {
                       group: '.col-xs-8',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
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
                   //DOB: {
                   //    group: '.col-sm-3',
                   //    validators: {
                   //        date: {
                   //            format: date_format,
                   //            message: ''
                   //        }
                   //    }
                   //}
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            emergencyContactDetail.SaveEmergencyContact();
        }).on('keyup', 'input#txtEmail', function (e) {
            var formValidation = $('#' + emergencyContactDetail.params.PanelID + ' #frmEmergencyContactDetail').data("bootstrapValidator");
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

    FillPatientInfoFromSearch: function (PatientId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        emergencyContactDetail.FillPatientInfo(PatientId, 1);
        UnloadActionPan(Patient_Search.params["ParentCtrl"]);
        utility.InsertRecentPatient(PatientId);
    },

    FillEmergencyContact: function (PatientID,ContactID) {
        emergencyContactDetail.params["EmergencyContactId"] = ContactID;
        AppPrivileges.GetFormPrivileges("Emergency Contact", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                emergencyContactDetail.EmergencyContactFill(PatientID, ContactID).done(function (response) {
                    if (response.status != false) {
                        LoadActionPan('emergencyContactDetail', null);
                        //UnloadActionPan(Patient_EmergencyContact.params["ParentCtrl"]);
                        var emergencyContact_detail = JSON.parse(response.EmergencyContactsFill_JSON);
                        var self = $("#emergencyContactDetail");
                        utility.bindMyJSON(true, emergencyContact_detail, false, self);
                        //utility.RemoveTimeFromDate("#emergencyContactDetail #dtpDOB", $("#emergencyContactDetail #dtpDOB").val());
                        emergencyContactDetail.params["mode"] = "Edit";

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
        emergencyContactDetail.FillPatientInfo(emergencyContactDetail.params.PatientID, 0);
    },

    FillPatientInfo: function (PatientId, IsFromSearch) {
        AppPrivileges.GetFormPrivileges("Emergency Contact", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                emergencyContactDetail.PatientInfoFill(PatientId, IsFromSearch).done(function (response) {
                    if (response.status != false) {
                        var patientinfo_detail = JSON.parse(response.PatientInfoFill_JSON);
                        var self = $("#emergencyContactDetail");
                        utility.bindMyJSON(true, patientinfo_detail, false, self);
                        /*Patient_EmergencyContact.params["mode"] = "Edit";
                        Patient_EmergencyContact.ValidateEmergencyContact();*/
                        $('#frmEmergencyContactDetail').bootstrapValidator('revalidateField', 'LastName');
                        $('#frmEmergencyContactDetail').bootstrapValidator('revalidateField', 'FirstName');

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

    SaveEmergencyContact: function () {
        var strMessage = "";
        if (emergencyContactDetail.params.EmergencyContactsCount == "0") {
            $("#hfIsPrimary").val("True");
        }
        if (emergencyContactDetail.params.mode == "Edit") {
            if (emergencyContactDetail.params.IsPrimary == "True") {
                $("#hfIsPrimary").val("True");
            } else {
                $("#hfIsPrimary").val("False");
            }
        }
        var self = $("#emergencyContactDetail");
        var myJSON = self.getMyJSON();

        if (emergencyContactDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Emergency Contact", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    emergencyContactDetail.EmergencyContactSave(myJSON, emergencyContactDetail.params.PatientID).done(function (response) {
                        if (response.status != false) {
                            emergencyContactDetail.params["EmergencyContactId"] = response.EmergencyContactID;
                            emergencyContactDetail.params["mode"] = "Edit";
                            emergencyContactDetail.UpdateCareGiversDropDown();
                            Patient_EmergencyContact.LoadEmergencyContacts();
                            UnloadActionPan(emergencyContactDetail.params["ParentCtrl"]);
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
        else if (emergencyContactDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Emergency Contact", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    emergencyContactDetail.EmergencyContactUpdate(myJSON, emergencyContactDetail.params.ContactId).done(function (response) {
                        if (response.status != false) {

                            utility.DisplayMessages(response.message, 1);
                            emergencyContactDetail.UpdateCareGiversDropDown();
                            Patient_EmergencyContact.LoadEmergencyContacts();
                            UnloadActionPan(emergencyContactDetail.params["ParentCtrl"]);
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

    EmergencyContactUpdate: function (EmergencyContactData, EmergencyContactID) {
        var data = "EmergencyContactData=" + EmergencyContactData + "&PatientID=" + emergencyContactDetail.params.PatientID + "&EmergencyContactID=" + EmergencyContactID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_EMERGENCYCONTACT_DETAIL", "UPDATE_EMERGENCYCONTACT");
    },

    EmergencyContactSave: function (EmergencyContactData, patientID) {
        var data = "EmergencyContactData=" + EmergencyContactData + "&PatientID=" + patientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_EMERGENCYCONTACT_DETAIL", "SAVE_EMERGENCYCONTACT");
    },

    EmergencyContactFill: function (patientID,ContactID) {
        var data = "PatientID=" + patientID + "&EmergencyContactID=" + ContactID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_EMERGENCYCONTACT_DETAIL", "FILL_EMERGENCYCONTACT");
    },

    PatientInfoFill: function (PatientId, IsFromSearch) {
        var data = "PatientID=" + PatientId + "&IsFromSearch=" + IsFromSearch;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_EMERGENCYCONTACT_DETAIL", "FILL_PATIENT_INFO");
    },

    UnLoad: function (Tab) {

        utility.UnLoadDialog('frmEmergencyContactDetail', function () {
            UnloadActionPan(emergencyContactDetail.params["ParentCtrl"]);
        }, function () {
            UnloadActionPan(emergencyContactDetail.params["ParentCtrl"]);
        });

    },
    UpdateCareGiversDropDown: function () {
        $.when(Patient_Demographic.LoadCareGiverDropDowns('pnlDemographic').then(function () {
            if (Patient_Demographic.careGiverIds) {
                $('#' + Patient_Demographic.params.PanelID + " #ddlCareGiver").val(Patient_Demographic.careGiverIds.split(','));
            }          
            Patient_Demographic.IntializeMultiSelectDropDownCareGiver();
        }));
    },
}
