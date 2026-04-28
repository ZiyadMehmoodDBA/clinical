Admin_PasswordConfiguration = {
    bIsFirstLoad: true,
    Load: function (params) {
        var self = $('#frmPasswordConfiguration');
        if (Admin_PasswordConfiguration.bIsFirstLoad) {
            Admin_PasswordConfiguration.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {
                //serialize Data.
                $('#frmPasswordConfiguration').data('serialize', $('#frmPasswordConfiguration').serialize());
                Admin_PasswordConfiguration.ValidatePasswordConfiguration();

                Admin_PasswordConfiguration.EntityChanged($("#frmPasswordConfiguration #lstEntityId"));
            });
        }
    },

    PasswordConfigurationFill: function (PasswordConfigurationID) {
        Admin_PasswordConfiguration.FillPasswordConfiguration(PasswordConfigurationID).done(function (response) {
            if (response.status != false) {

                var Admin_PasswordConfigurationData = JSON.parse(response.PasswordConfigurationFill_JSON);
                var self = $("#frmPasswordConfiguration");
                utility.bindMyJSON(true, Admin_PasswordConfigurationData, false, self).done(function () {
                    //serialize Data.
                    $('#frmPasswordConfiguration').data('serialize', $('#frmPasswordConfiguration').serialize());
                });
            }
            else {
                Admin_PasswordConfiguration.setDefaultConfiguration();
                //                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    FillPasswordConfiguration: function (PasswordConfigurationID) {
        var data = "PasswordConfigurationID=" + PasswordConfigurationID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PASSWORD_CONFIGURATION", "FILL_PASSWORD_CONFIGURATION");
    },

    ValidatePasswordConfiguration: function () {
        $('#frmPasswordConfiguration')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {

                    Entity: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },

                    MinPasswordLength: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    MinSpecialCharacter: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    MinAlphaCharacter: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    MinNumericCharacter: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    MinUppercaseCharacter: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    MaxPasswordAge: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    PasswordHistory: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },

                    AccountLockoutThreshold: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },

                    IdleSessionTimeout: {
                        group: '.col-sm-2',
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
            Admin_PasswordConfiguration.PasswordConfigurationUdpate();
        });
    },

    PasswordConfigurationUdpate: function () {
        var self = $("#frmPasswordConfiguration");
        var myJSON = self.getMyJSON();
        //password configuration id and entityid would be the same
        var EntityId = $("#frmPasswordConfiguration #lstEntityId").val();
        if (EntityId != "") {
            Admin_PasswordConfiguration.UpdatePasswordConfiguration(myJSON, EntityId).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    UpdatePasswordConfiguration: function (PasswordConfigurationData, PasswordConfigurationID) {
        var data = "PasswordConfigurationData=" + PasswordConfigurationData + "&PasswordConfigurationID=" + PasswordConfigurationID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PASSWORD_CONFIGURATION", "UPDATE_PASSWORD_CONFIGURATION");
    },

    EntityChanged: function (Ctrl) {

        var EntityId = $(Ctrl).val();
        if (EntityId != "") {
            Admin_PasswordConfiguration.PasswordConfigurationFill(EntityId);
        } else {


        }
    },

    setDefaultConfiguration: function () {
        $("#frmPasswordConfiguration #txtMinPasswordLength").val(8);
        $("#frmPasswordConfiguration #txtMinSpecialCharacter").val(1);
        $("#frmPasswordConfiguration #txtMinAlphaCharacter").val(1);
        $("#frmPasswordConfiguration #txtMinNumericCharacter").val(1);
        $("#frmPasswordConfiguration #txtMinUppercaseCharacter").val(1);
        $("#frmPasswordConfiguration #txtMaxPasswordAge").val(60);
        $("#frmPasswordConfiguration #txtPasswordHistory").val(24);
        $("#frmPasswordConfiguration #txtAccountLockoutThreshold").val(0);
        $("#frmPasswordConfiguration #txtIdleSessionTimeout").val(10);
    },


    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}