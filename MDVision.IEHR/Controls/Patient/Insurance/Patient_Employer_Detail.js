employerDetail = {
    params: [],
    Load: function (params) {
        employerDetail.params = params;
        employerDetail.LoadEmployer();
    },

    LoadEmployer: function () {
        AppPrivileges.GetFormPrivileges("Employer", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (employerDetail.params.mode == "Add") {

                    //serialize data
                    $('#frmEmployerDetail').data('serialize', $('#frmEmployerDetail').serialize());
                    employerDetail.ValidateEmployer();
                }
                else if (employerDetail.params.mode == "Edit") {
                    employerDetail.FillEmployer(employerDetail.params.EmployerId).done(function (response) {
                        if (response.status != false) {
                            var employer_detail = JSON.parse(response.EmployerFill_JSON);
                            var self = $("#employerDetail");
                            utility.bindMyJSON(true, employer_detail, false, self).done(function () {


                                if (employer_detail.chkActive == 'True')
                                    $("#employerDetail #chkActive").attr("checked", true);
                                else
                                    $("#employerDetail #chkActive").attr("checked", false);

                                employerDetail.ValidateEmployer();
                                //serialize data
                                $('#frmEmployerDetail').data('serialize', $('#frmEmployerDetail').serialize());

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

    EmployerSave: function () {
        var strMessage = "";
        var self = $("#employerDetail");
        var myJSON = self.getMyJSON();
        if (employerDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Employer", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    employerDetail.SaveEmployer(myJSON).done(function (response) {
                        if (response.status != false) {
                            Patient_Employer.EmployerSearch(response.EmployerId);
                            CacheManager.BindCodes('GetEmployer', true);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan(employerDetail.params["ParentCtrl"]);
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
        else if (employerDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Employer", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    employerDetail.UpdateEmployer(myJSON, employerDetail.params.EmployerId).done(function (response) {
                        if (response.status != false) {
                            if (employerDetail.params["ParentCtrl"] == "Patient_Employer") {
                                Patient_Employer.EmployerSearch(employerDetail.params.EmployerId);
                            }                            
                            CacheManager.BindCodes('GetEmployer', true);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan(employerDetail.params["ParentCtrl"]);
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

    ValidateEmployer: function () {
        $('#frmEmployerDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Name: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   'Email': {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           regexp: {
                               regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                               message: 'Email not Valid'
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            employerDetail.EmployerSave();
        });
    },
    EnableValidation: function (obj) {
        var objprovider = $('#employerDetail #frmEmployerDetail');
        var formValidation = objprovider.data("bootstrapValidator");
        if ($(obj).val() != "") {
            formValidation.enableFieldValidators($(obj).attr("name"), true);
        } else {
            formValidation.enableFieldValidators($(obj).attr("name"), false);
        }
    },
    SaveEmployer: function (EmployerData) {
        var data = "EmployerData=" + EmployerData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMPLOYER_DETAIL", "SAVE_EMPLOYER");
    },

    UpdateEmployer: function (EmployerData, EmployerID) {
        var data = "EmployerData=" + EmployerData + "&EmployerID=" + EmployerID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMPLOYER_DETAIL", "UPDATE_EMPLOYER");
    },

    FillEmployer: function (EmployerID) {
        var data = "EmployerID=" + EmployerID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMPLOYER_DETAIL", "FILL_EMPLOYER");
    },

    UpdateEmployerActiveInactive: function (EmployerID, IsActive) {
        var data = "EmployerID=" + EmployerID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_EMPLOYER_DETAIL", "UPDATE_EMPLOYER_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmEmployerDetail', function () {
            UnloadActionPan(employerDetail.params["ParentCtrl"]);
        }, function () {
            UnloadActionPan(employerDetail.params["ParentCtrl"]);
        });

        
    },
}