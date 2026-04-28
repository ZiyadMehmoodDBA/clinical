
submitterSetupDetail = {
    params: [],
    Load: function (params) {
        submitterSetupDetail.params = params;
        
        var self = null;
        if (submitterSetupDetail.params.PanelID == "submitterSetupDetail")
            self = $('#submitterSetupDetail');
        else
            self = $('#' + submitterSetupDetail.params.PanelID + ' #submitterSetupDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            submitterSetupDetail.LoadSubmitterSetup();
        });
    },

    LoadSubmitterSetup: function () {
        if (submitterSetupDetail.params.mode == "Add") {
            $('#submitterSetupDetail #txtShortName').attr("enabled", "enabled");

            //serialize data
            $('#frmSubmitterSetupDetail').data('serialize', $('#frmSubmitterSetupDetail').serialize());
            submitterSetupDetail.ValidateSubmitterSetup();
        }
        else if (submitterSetupDetail.params.mode == "Edit") {
            $('#submitterSetupDetail #txtShortName').attr("disabled", "disabled");
            submitterSetupDetail.FillSubmitterSetup(submitterSetupDetail.params.SubmitterSetupId).done(function (response) {
                if (response.status != false) {
                    var submitterSetup_detail = JSON.parse(response.SubmitterSetupFill_JSON);
                    var self = $("#submitterSetupDetail");
                    utility.bindMyJSON(true, submitterSetup_detail, false, self).done(function () {

                        submitterSetupDetail.ValidateSubmitterSetup();
                        //serialize data
                        $('#frmSubmitterSetupDetail').data('serialize', $('#frmSubmitterSetupDetail').serialize());
                    });
                    
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateSubmitterSetup: function () {
        $('#frmSubmitterSetupDetail')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    ShortName: {
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
                    OrganizationLastName: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    SubmitterAddress1: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    City: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    State: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Zipcode: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    ContactName: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    ContactTelephone: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    //EntityIdentifier: {
                    //    group: '.col-sm-4',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //EntityTypeQualifier: {
                    //    group: '.col-sm-4',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //SubmitterFirstName: {
                    //    group: '.col-sm-4',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    //SubmitterMiddleName: {
                    //    group: '.col-sm-4',
                    //    validators: {
                    //        notEmpty: {
                    //            message: ''
                    //        }
                    //    }
                    //},
                    IdentificationCodeQualifier: {
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
            submitterSetupDetail.SubmitterSetupSave();
        });
    },

    SubmitterSetupSave: function () {
        var strMessage = "";
        var self = $("#submitterSetupDetail");
        var myJSON = self.getMyJSON();
        if (submitterSetupDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Submitter Setup", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    submitterSetupDetail.SaveSubmitterSetup(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_SubmitterSetup.SubmitterSetupSearch(response.SubmitterSetupId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
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
        else if (submitterSetupDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Submitter Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    submitterSetupDetail.UpdateSubmitterSetup(myJSON, submitterSetupDetail.params.SubmitterSetupId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Admin_SubmitterSetup.SubmitterSetupSearch(submitterSetupDetail.params.SubmitterSetupId);
                            UnloadActionPan();
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

    SaveSubmitterSetup: function (SubmitterSetupData) {
        var data = "SubmitterSetupData=" + SubmitterSetupData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SUBMITTER_SETUP_DETAIL", "SAVE_SUBMITTER_SETUP");
    },

    UpdateSubmitterSetup: function (SubmitterSetupData, SubmitterSetupID) {
        var data = "SubmitterSetupData=" + SubmitterSetupData + "&SubmitterSetupID=" + SubmitterSetupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SUBMITTER_SETUP_DETAIL", "UPDATE_SUBMITTER_SETUP");
    },

    FillSubmitterSetup: function (SubmitterSetupID) {
        var data = "SubmitterSetupID=" + SubmitterSetupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SUBMITTER_SETUP_DETAIL", "FILL_SUBMITTER_SETUP");
    },

    UpdateSubmitterSetupActiveInactive: function (SubmitterSetupID, IsActive) {
        var data = "SubmitterSetupID=" + SubmitterSetupID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SUBMITTER_SETUP_DETAIL", "UPDATE_SUBMITTER_SETUP_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmSubmitterSetupDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },
    ShowHistory: function () {
        var PanelID = 'submitterSetupDetail';
        var ParentCtrl = 'submitterSetupDetail';
        var ProfileName = 'Submitter Setup';
        var DBTableName = 'SubmitterSetup';
        var ColumnKeyId = submitterSetupDetail.params.SubmitterSetupId

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}