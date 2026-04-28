lawyerDetail = {
    params: [],
    Load: function (params) {
        lawyerDetail.params = params;
        lawyerDetail.LoadLawyer();
    },

    LoadLawyer: function () {
        AppPrivileges.GetFormPrivileges("Lawyer", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (lawyerDetail.params.mode == "Add") {

                    //serialize data
                    $('#frmLawyerDetail').data('serialize', $('#frmLawyerDetail').serialize());
                    lawyerDetail.ValidateLawyer();
                }
                else if (lawyerDetail.params.mode == "Edit") {
                    lawyerDetail.FillLawyer(lawyerDetail.params.LawyerId).done(function (response) {
                        if (response.status != false) {
                            var lawyer_detail = JSON.parse(response.LawyerFill_JSON);
                            var self = $("#lawyerDetail");
                            utility.bindMyJSON(true, lawyer_detail, false, self).done(function () {

                                if (lawyer_detail.chkActive == 'True')
                                    $("#lawyerDetail #chkActive").attr("checked", true);
                                else
                                    $("#lawyerDetail #chkActive").attr("checked", false);

                                lawyerDetail.ValidateLawyer();
                                //serialize data
                                $('#frmLawyerDetail').data('serialize', $('#frmLawyerDetail').serialize());

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

    LawyerSave: function () {
        var strMessage = "";
        var self = $("#lawyerDetail");
        var myJSON = self.getMyJSON();
        if (lawyerDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Lawyer", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    lawyerDetail.SaveLawyer(myJSON).done(function (response) {
                        if (response.status != false) {
                            Patient_Lawyer.LawyerSearch(response.LawyerId);
                            CacheManager.BindCodes('GetLawyer', true);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan(lawyerDetail.params["ParentCtrl"]);
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
        else if (lawyerDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Lawyer", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    lawyerDetail.UpdateLawyer(myJSON, lawyerDetail.params.LawyerId).done(function (response) {
                        if (response.status != false) {
                            if (lawyerDetail.params["ParentCtrl"] == "Patient_Lawyer") {
                                Patient_Lawyer.LawyerSearch(lawyerDetail.params.LawyerId);
                            }
                            CacheManager.BindCodes('GetLawyer', true);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan(lawyerDetail.params["ParentCtrl"]);
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

    ValidateLawyer: function () {
        $('#frmLawyerDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   LawyerName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LawyerName: {
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
            lawyerDetail.LawyerSave();
        });
    },
    FillCityState: function (control, zipctrl, cityctr, statectrl) {
        var zipcode = $(control + ' ' + zipctrl).val();
        var cityname = null;
        var statename = null;
        if (zipcode != "" && $(control + ' ' + zipctrl).inputmask().val().indexOf('_') == -1) {
            var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
            // serach parameter , class name, command name of class
            MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE").done(function (response) {
                if (response.status != false) {
                    var citystate = JSON.parse(response.CITYSTATE_JSON);
                    $(control + ' ' + cityctr).val(citystate.txtCity);
                    $(control + ' ' + statectrl).val(citystate.txtState);

           
                    //
                }
                else {
                    ////Practice Management System PMS-816
                    ////patient Entire Module : When user entered invalid Zip code , States and City data disappeared
                    //$(control + ' ' + cityctr).val('');
                    //$(control + ' ' + statectrl).val('');
                }
            });
        }

    },
    EnableValidation: function (obj) {
        var objprovider = $('#lawyerDetail #frmLawyerDetail');
        var formValidation = objprovider.data("bootstrapValidator");
        if ($(obj).val() != "") {
            formValidation.enableFieldValidators($(obj).attr("name"), true);
        } else {
            formValidation.enableFieldValidators($(obj).attr("name"), false);
        }
    },

    SaveLawyer: function (LawyerData) {
        var data = "LawyerData=" + LawyerData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_LAWYER_DETAIL", "SAVE_LAWYER");
    },

    UpdateLawyer: function (LawyerData, LawyerID) {
        var data = "LawyerData=" + LawyerData + "&LawyerID=" + LawyerID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_LAWYER_DETAIL", "UPDATE_LAWYER");
    },

    FillLawyer: function (LawyerID) {
        var data = "LawyerID=" + LawyerID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_LAWYER_DETAIL", "FILL_LAWYER");
    },

    UpdateLawyerActiveInactive: function (LawyerID, IsActive) {
        var data = "LawyerID=" + LawyerID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_LAWYER_DETAIL", "UPDATE_LAWYER_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmLawyerDetail', function () {
            UnloadActionPan(lawyerDetail.params["ParentCtrl"]);
        }, function () {
            UnloadActionPan(lawyerDetail.params["ParentCtrl"]);
        });
    },
}