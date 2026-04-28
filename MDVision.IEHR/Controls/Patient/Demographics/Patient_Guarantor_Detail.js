guarantorDetail = {
    params: [],
    PatientRelation:'',
    Load: function (params) {
        guarantorDetail.params = params;
        var self = $('#tblguarantorDetail');
        self.loadDropDowns(true).done(function () {
            guarantorDetail.LoadGuarantor();
            if (guarantorDetail.params.PatientId) {
                $("#guarantorDetail #frmGuarantorDetail #hfPatientId").val(guarantorDetail.params.PatientId);
            }
        });

    },
    SetAddressSameAsPatient: function () {
        if (guarantorDetail.params.Address1 != null && typeof guarantorDetail.params.Address1 != "undefined") {
            $("#guarantorDetail #frmGuarantorDetail #txtAddress1").val(guarantorDetail.params.Address1);
            $("#guarantorDetail #frmGuarantorDetail").bootstrapValidator('revalidateField', 'Address1');
        }
        if (guarantorDetail.params.Zip != null && typeof guarantorDetail.params.Zip != "undefined") {
            $("#guarantorDetail #frmGuarantorDetail #txtZip").val(guarantorDetail.params.Zip);
        }
        if (guarantorDetail.params.City != null && typeof guarantorDetail.params.City != "undefined") {
            $("#guarantorDetail #frmGuarantorDetail #txtCity").val(guarantorDetail.params.City);
            $("#guarantorDetail #frmGuarantorDetail").bootstrapValidator('revalidateField', 'City');
        }
        if (guarantorDetail.params.State != null && typeof guarantorDetail.params.State != "undefined") {
            $("#guarantorDetail #frmGuarantorDetail #txtState").val(guarantorDetail.params.State);
        }
    },
    LoadGuarantor: function () {
        AppPrivileges.GetFormPrivileges("Guarantor", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (guarantorDetail.params.mode == "Add") {

                    guarantorDetail.ValidateGuarantor();
                    $('#frmGuarantorDetail').data('serialize', $('#frmGuarantorDetail').serialize());

                }
                else if (guarantorDetail.params.mode == "Edit") {
                    guarantorDetail.FillGuarantor(guarantorDetail.params.GuarantorId).done(function (response) {
                        if (response.status != false) {
                            var guarantor_detail = JSON.parse(response.GuarantorFill_JSON);
                            guarantorDetail.ValidateGuarantor();

                            var self = $("#guarantorDetail");
                            utility.bindMyJSON(true, guarantor_detail, false, self).done(function () {

                                if (guarantor_detail.chkActive == 'True')
                                    $("#guarantorDetail #chkActive").attr("checked", true);
                                else
                                    $("#guarantorDetail #chkActive").attr("checked", false);

                                $('#frmGuarantorDetail').data('serialize', $('#frmGuarantorDetail').serialize());
                                guarantorDetail.PatientRelation = $("#guarantorDetail #frmGuarantorDetail #ddlRelation option:selected").text().trim();
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

    GuarantorSave: function () {

        var strMessage = "";
        var self = $("#guarantorDetail");
        var myJSON = self.getMyJSON();
        if (guarantorDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Guarantor", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    guarantorDetail.SaveGuarantor(myJSON).done(function (response) {
                        if (response.status != false) {
                            Patient_Guarantor.GuarantorSearch(response.GuarantorId);
                            utility.DisplayMessages(response.message, 1);
                            //CacheManager.BindCodes('GetGuarantor', true);
                            $('#frmGuarantorDetail').data('serialize', $('#frmGuarantorDetail').serialize());
                            UnloadActionPan(guarantorDetail.params["ParentCtrl"]);
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
        else if (guarantorDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Guarantor", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    guarantorDetail.UpdateGuarantor(myJSON, guarantorDetail.params.GuarantorId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            //CacheManager.BindCodes('GetGuarantor', true).done(function (result) {
                            var GurantorFullName = $("#guarantorDetail #txtLastName").val() + ", " + $("#guarantorDetail #txtFirstName").val();
                            if (guarantorDetail.params.RefCtrl != null) {
                                guarantorDetail.FillGuarantorName(guarantorDetail.params.GuarantorId, GurantorFullName);
                                //Patient_Guarantor.GuarantorSearch(guarantorDetail.params.GuarantorId);
                            }
                            else {
                                Patient_Guarantor.GuarantorSearch(guarantorDetail.params.GuarantorId);

                                $('#frmGuarantorDetail').data('serialize', $('#frmGuarantorDetail').serialize());
                                UnloadActionPan(guarantorDetail.params["ParentCtrl"]);
                            }
                            //});
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

    ValidateGuarantor: function () {
        $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Relation: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LastName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   FirstName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Address1: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   City: {
                       group: '.size60per',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //State: {
                   //    group: '.size40per',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   //Zip: {
                   //    group: '.size60per',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   Email: {
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
            guarantorDetail.GuarantorSave();
        }).on('keyup', 'input#txtEmail', function (e) {
            var formValidation = $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').data("bootstrapValidator");
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

    FillGuarantorName: function (GuarantorId, GuarantorName) {
        $('#' + guarantorDetail.params["PanelID"] + ' #txtGuarantor').val(GuarantorName);
        $('#' + guarantorDetail.params["PanelID"] + ' #hfGuarantor').val(GuarantorId);
        $('#' + guarantorDetail.params["PanelID"] + ' #lblGuarantor').css("display", "none");
        $('#' + guarantorDetail.params["PanelID"] + ' #lnkGuarantorEdit').css("display", "inline");
        utility.SetKendoAutoCompleteSourceforValidate($('#' + guarantorDetail.params["PanelID"] + ' #txtGuarantor'), GuarantorName, $('#' + guarantorDetail.params["PanelID"] + ' #hfGuarantor'), GuarantorId);
        UnloadActionPan(guarantorDetail.params["ParentCtrl"], "Patient_Guarantor");
    },

    SaveGuarantor: function (GuarantorData) {
        var data = "GuarantorData=" + GuarantorData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_GUARANTOR_DETAIL", "SAVE_GUARANTOR");
    },

    UpdateGuarantor: function (GuarantorData, GuarantorID) {
        var PatientId = 0;
        if (guarantorDetail.params.PatientId) {
            PatientId = guarantorDetail.params.PatientId;
        }
        var data = "GuarantorData=" + GuarantorData + "&GuarantorID=" + GuarantorID + "&PatientID=" + PatientId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_GUARANTOR_DETAIL", "UPDATE_GUARANTOR");
    },

    FillGuarantor: function (GuarantorID) {
        var PatientId=0;
        if(guarantorDetail.params.PatientId){
            PatientId=guarantorDetail.params.PatientId;
        }
        var data = "GuarantorID=" + GuarantorID + "&PatientID=" + PatientId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_GUARANTOR_DETAIL", "FILL_GUARANTOR");
    },

    UpdateGuarantorActiveInactive: function (GuarantorID, IsActive) {
        var data = "GuarantorID=" + GuarantorID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_GUARANTOR_DETAIL", "UPDATE_GUARANTOR_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmGuarantorDetail', function () {
            UnloadActionPan(guarantorDetail.params["ParentCtrl"]);
        }, function () {
            UnloadActionPan(guarantorDetail.params["ParentCtrl"]);
        });

    },
    SetSelfRelation: function (text) {
        if (text != "") {
            var CurrentOptionText = $("#guarantorDetail #frmGuarantorDetail #ddlRelation option[value='" + text + "']").text();
            if (CurrentOptionText.toLowerCase() == "self") {
                guarantorDetail.CopyPatient();
                guarantorDetail.PatientRelation = CurrentOptionText;
                var formValidation = $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').data("bootstrapValidator");
                if (formValidation) {
                    formValidation.enableFieldValidators('LastName', true);
                    formValidation.enableFieldValidators('FirstName', true);
                    formValidation.enableFieldValidators('Address1', true);
                    formValidation.enableFieldValidators('City', true);
                    $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').bootstrapValidator('revalidateField', 'LastName');
                    $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').bootstrapValidator('revalidateField', 'FirstName');
                    $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').bootstrapValidator('revalidateField', 'Address1');
                    $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').bootstrapValidator('revalidateField', 'City');
                }

            } else if (guarantorDetail.PatientRelation.toLowerCase()=="self" && CurrentOptionText.toLowerCase() !="self") {
                $("#guarantorDetail #frmGuarantorDetail").find("input[type=text]").val("");
                $("#guarantorDetail #frmGuarantorDetail #dtpDOB").datepicker("setDate", "");
                guarantorDetail.PatientRelation = CurrentOptionText;
                var formValidation = $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').data("bootstrapValidator");
                if (formValidation) {
                    formValidation.enableFieldValidators('LastName', true);
                    formValidation.enableFieldValidators('FirstName', true);
                    formValidation.enableFieldValidators('Address1', true);
                    formValidation.enableFieldValidators('City', true);
                    $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').bootstrapValidator('revalidateField', 'LastName');
                    $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').bootstrapValidator('revalidateField', 'FirstName');
                    $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').bootstrapValidator('revalidateField', 'Address1');
                    $('#' + guarantorDetail.params.PanelID + ' #frmGuarantorDetail').bootstrapValidator('revalidateField', 'City');
                }
            }
        }

        
    },
    CopyPatient: function () {
        $("#guarantorDetail #frmGuarantorDetail #txtLastName").val($('#pnlDemographic #frmDemographic #txtLastName').val());
        $("#guarantorDetail #frmGuarantorDetail #txtFirstName").val($('#pnlDemographic #frmDemographic #txtFirstName').val());

        if ($("#pnlDemographic #frmDemographic #dtpDOB").val() != "")
            $("#guarantorDetail #frmGuarantorDetail #dtpDOB").datepicker("setDate", $('#pnlDemographic #frmDemographic #dtpDOB').val());

        $("#guarantorDetail #frmGuarantorDetail #txtTelephone").val($('#pnlDemographic #frmDemographic #txtHomeTel').val());
        $("#guarantorDetail #frmGuarantorDetail #txtEmail").val($('#pnlDemographic #frmDemographic #txtEmail').val());
        $("#guarantorDetail #frmGuarantorDetail #txtAddress1").val($('#pnlDemographic #frmDemographic #txtAddress1').val());
        $("#guarantorDetail #frmGuarantorDetail #txtCity").val($('#pnlDemographic #frmDemographic #txtCity').val());
        $("#guarantorDetail #frmGuarantorDetail #txtState").val($('#pnlDemographic #frmDemographic #txtState').val());
        $("#guarantorDetail #frmGuarantorDetail #txtZip").val($('#pnlDemographic #frmDemographic #txtZip').val());
        $("#guarantorDetail #frmGuarantorDetail #txtZipExt").val($('#pnlDemographic #frmDemographic #txtZipExt').val());

    },
    LoadActiveGuarantors: function (name) {
        var data = "name=" + name;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_GUARANTOR_DETAIL", "SEARCH_GUARANTOR");
    }
}