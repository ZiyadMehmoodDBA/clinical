billingprovidersettingsDetail = {
    params: [],
    Enable: false,
    AddressEnalbed: false,
    Load: function (params) {
        billingprovidersettingsDetail.params = params;

        var self = $('#tblbillingprovidersettingsDetail');
        self.loadDropDowns(true).done(function () {

            billingprovidersettingsDetail.LoadBillingProviderSettings();
            var isBillToEINChecked = self.find("#chkBillToEIN").attr("checked");
            if (isBillToEINChecked != "checked") {
                self.find("#pnlEIN").addClass('disableAll');
            }
            else {
                self.find("#pnlEIN").removeClass('disableAll');

            }
           
        });

    },

    LoadBillingProviderSettings: function () {
        if (billingprovidersettingsDetail.params.mode == "Add") {


            //serialize Data after all controls loaded.
            $('#frmBillingProviderSettingsDetail').data('serialize', $('#frmBillingProviderSettingsDetail').serialize());
            billingprovidersettingsDetail.ValidateBillingProviderSettings();

            billingprovidersettingsDetail.EnableEIN_Div();
            billingprovidersettingsDetail.PayAddress($("#billingprovidersettingsDetail #chkIsPayToAddress"));

        }
        else if (billingprovidersettingsDetail.params.mode == "Edit") {
            billingprovidersettingsDetail.FillBillingProviderSettings(billingprovidersettingsDetail.params.BillingProviderSettingsId).done(function (response) {
                if (response.status != false) {
                    var billingprovidersettings_detail = JSON.parse(response.BillingProviderSettingsFill_JSON);
                    var self = $("#billingprovidersettingsDetail");

                    utility.bindMyJSON(true, billingprovidersettings_detail, false, self).done(function () {

                        if (billingprovidersettings_detail.chkBillToEIN == 'True') {
                           
                            $("#billingprovidersettingsDetail #chkBillToEIN").attr("checked", true);
                            $("#pnlEIN").removeClass('disableAll');
                            $("#billingprovidersettingsDetail #chkBillToEIN").attr("checked", true);
                        }
                        else {
                            $("#pnlEIN").addClass('disableAll');
                            $("#billingprovidersettingsDetail #chkBillToEIN").attr("checked", false);
                        }
                        if (billingprovidersettings_detail.chkBillToProviderSSN == 'True') {
                            $("#billingprovidersettingsDetail #chkBillToProviderSSN").attr("checked", true);
                        }
                        else {
                            $("#billingprovidersettingsDetail #chkBillToProviderSSN").attr("checked", false);
                        }
                        if (billingprovidersettings_detail.chkAcceptAssignment == 'True') {
                            $("#billingprovidersettingsDetail #chkAcceptAssignment").attr("checked", true);
                        }
                        else {
                            $("#billingprovidersettingsDetail #chkAcceptAssignment").attr("checked", false);
                        }
                        if (billingprovidersettings_detail.chkIsActive == 'True') {
                            $("#billingprovidersettingsDetail #chkIsActive").attr("checked", true);
                        }
                        else {
                            $("#billingprovidersettingsDetail #chkIsActive").attr("checked", false);
                        }

                        billingprovidersettingsDetail.ValidateBillingProviderSettings();

                        //set is to pay validation
                        billingprovidersettingsDetail.PayAddress($("#billingprovidersettingsDetail #chkIsPayToAddress"));
                        billingprovidersettingsDetail.EnableEIN_Div();
                        //serialize Data after all controls loaded.
                        $('#frmBillingProviderSettingsDetail').data('serialize', $('#frmBillingProviderSettingsDetail').serialize());



                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    CallCityState: function (control, field1, field2) {
        var zipcode = $('#billingprovidersettingsDetail ' + control).val();
        var cityname = null;
        var statename = null;
        billingprovidersettingsDetail.FillCityState(zipcode, cityname, statename).done(function (response) {
            if (response.status != false) {
                var citystate = JSON.parse(response.CITYSTATE_JSON);
                $('#billingprovidersettingsDetail ' + field1).val(citystate.txtCity);
                $('#billingprovidersettingsDetail ' + field2).val(citystate.txtState);
                //billingprovidersettingsDetail.ValidateBillingProviderSettings();
            }
            else {
                $('#billingprovidersettingsDetail ' + field1).val('');
                $('#billingprovidersettingsDetail ' + field2).val('');
            }
        });
    },

    ValidateBillingProviderSettings: function () {
        $('#frmBillingProviderSettingsDetail')
           .bootstrapValidator({
               live: 'disabled',
               excluded: ':disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Insurance: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Facility: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Provider: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   EIN: {
                       group: '.col-sm-7',
                       enabled: billingprovidersettingsDetail.Enable,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   EINName: {
                       group: '.col-sm-3',
                       enabled: billingprovidersettingsDetail.Enable,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   NPI: {
                       group: '.col-xs-5',
                       enabled: billingprovidersettingsDetail.Enable,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           integer: {
                               message: ' ',
                           }
                       }
                   },
                   Address1: {
                       group: '.col-sm-6',
                       enabled: billingprovidersettingsDetail.Enable,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   City: {
                       group: '.col-sm-3',
                       enabled: billingprovidersettingsDetail.Enable,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   State: {
                       group: '.col-sm-3',
                       enabled: billingprovidersettingsDetail.Enable,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ZipCode: {
                       group: '.col-sm-8',
                       enabled: billingprovidersettingsDetail.Enable,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           integer: {
                               message: ' ',
                           }
                       }
                   },
                   PayToAddress1: {
                       group: '.col-sm-6',
                       enabled: billingprovidersettingsDetail.AddressEnalbed,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PayToAddress2: {
                       group: '.col-sm-6',
                       enabled: billingprovidersettingsDetail.AddressEnalbed,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PayToCity: {
                       group: '.col-sm-3',
                       enabled: billingprovidersettingsDetail.AddressEnalbed,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PayToState: {
                       group: '.col-sm-3',
                       enabled: billingprovidersettingsDetail.AddressEnalbed,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PayToZipCode: {
                       group: '.col-sm-3',
                       enabled: billingprovidersettingsDetail.AddressEnalbed,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           integer: {
                               message: ' ',
                           }
                       }
                   },
               }
           })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           billingprovidersettingsDetail.BillingProviderSettingsSave();
       });
    },

    BillingProviderSettingsSave: function () {
        var strMessage = "";
        var self = $("#billingprovidersettingsDetail");
        var myJSON = self.getMyJSON();
        if (billingprovidersettingsDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Additional Billing Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    billingprovidersettingsDetail.SaveBillingProviderSettings(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_BillingProviderSettings.BillingProviderSettingsSearch(response.BillingProviderSettingsId);
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
        else if (billingprovidersettingsDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Additional Billing Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    billingprovidersettingsDetail.UpdateBillingProviderSettings(myJSON, billingprovidersettingsDetail.params.BillingProviderSettingsId, 1).done(function (response) {
                        if (response.status != false) {
                            Admin_BillingProviderSettings.BillingProviderSettingsSearch(billingprovidersettingsDetail.params.BillingProviderSettingsId);
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
    },

    EnableEIN_Div: function () {
        var bootstrapValidator = $('#frmBillingProviderSettingsDetail').data('bootstrapValidator');
        if ($("#billingprovidersettingsDetail #chkBillToEIN").is(':checked')) {
            $("#billingprovidersettingsDetail #chkBillToProviderSSN").prop('checked', false);
            $("#billingprovidersettingsDetail #pnlEIN").removeClass('disableAll');
            billingprovidersettingsDetail.Enable = true;
            bootstrapValidator.enableFieldValidators('EIN', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('EINName', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('NPI', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('Address1', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('City', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('State', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('ZipCode', billingprovidersettingsDetail.Enable);

        }
        else {
            $("#billingprovidersettingsDetail #pnlEIN").addClass('disableAll');
            billingprovidersettingsDetail.Enable = false;
            bootstrapValidator.enableFieldValidators('EIN', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('EINName', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('NPI', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('Address1', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('City', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('State', billingprovidersettingsDetail.Enable);
            bootstrapValidator.enableFieldValidators('ZipCode', billingprovidersettingsDetail.Enable);
        }

    },

    ToggleBill_To_EIN: function (obj) {
       
        var bootstrapValidator = $('#frmBillingProviderSettingsDetail').data('bootstrapValidator');

        if ($(obj).is(':checked')) {

            var ProviderId = $("#billingprovidersettingsDetail #lstProvider").val();

            if (ProviderId != "") {
                billingprovidersettingsDetail.IsProviderHasSSN(ProviderId).done(function (response) {

                    if (response.status != false) {

                        $("#billingprovidersettingsDetail #chkBillToEIN").prop('checked', false);
                        $("#billingprovidersettingsDetail #pnlEIN").addClass('disableAll');
                        bootstrapValidator.enableFieldValidators('EIN', false);
                        bootstrapValidator.enableFieldValidators('EINName', false);
                        bootstrapValidator.enableFieldValidators('NPI', false);
                        bootstrapValidator.enableFieldValidators('Address1', false);
                        bootstrapValidator.enableFieldValidators('City', false);
                        bootstrapValidator.enableFieldValidators('State', false);
                        bootstrapValidator.enableFieldValidators('ZipCode', false);

                    }
                    else {
                        utility.DisplayMessages(response.Message, 2);
                        $(obj).prop('checked', false);
                        $("#billingprovidersettingsDetail #pnlEIN").removeClass('disableAll');

                        bootstrapValidator.enableFieldValidators('EIN', true);
                        bootstrapValidator.enableFieldValidators('EINName', true);
                        bootstrapValidator.enableFieldValidators('NPI', true);
                        bootstrapValidator.enableFieldValidators('Address1', true);
                        bootstrapValidator.enableFieldValidators('City', true);
                        bootstrapValidator.enableFieldValidators('State', true);
                        bootstrapValidator.enableFieldValidators('ZipCode', true);

                    }

                });
            }
            else {
                utility.DisplayMessages("Please select provider.", 2);
                $(obj).prop('checked', false);
            }
        }
        else {
            $("#billingprovidersettingsDetail #pnlEIN").removeClass('disableAll');
            bootstrapValidator.enableFieldValidators('EIN', true);
            bootstrapValidator.enableFieldValidators('EINName', true);
            bootstrapValidator.enableFieldValidators('NPI', true);
            bootstrapValidator.enableFieldValidators('Address1', true);
            bootstrapValidator.enableFieldValidators('City', true);
            bootstrapValidator.enableFieldValidators('State', true);
            bootstrapValidator.enableFieldValidators('ZipCode', true);

        }
    },

    IsProviderHasSSN: function (ProviderId) {

        var data = "ProviderId=" + ProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_SETTINGS_DETAIL", "CHECK_PROVIDER_SSN");

    },

    SaveBillingProviderSettings: function (BillingProviderSettingsData) {
        var data = "BillingProviderSettingsData=" + BillingProviderSettingsData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_SETTINGS_DETAIL", "SAVE_BILLING_PROVIDER_SETTINGS");
    },

    UpdateBillingProviderSettings: function (BillingProviderSettingsData, BillingProviderSettingsID, IsActive) {
        var data = "BillingProviderSettingsData=" + BillingProviderSettingsData + "&BillingProviderSettingsID=" + BillingProviderSettingsID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_SETTINGS_DETAIL", "UPDATE_BILLING_PROVIDER_SETTINGS");
    },

    FillBillingProviderSettings: function (BillingProviderSettingsID) {
        var data = "BillingProviderSettingsID=" + BillingProviderSettingsID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_SETTINGS_DETAIL", "FILL_BILLING_PROVIDER_SETTINGS");
    },

    UpdateBillingProviderSettingsActiveInactive: function (BillingProviderSettingsID, IsActive) {
        var data = "BillingProviderSettingsID=" + BillingProviderSettingsID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_SETTINGS_DETAIL", "UPDATE_BILLING_PROVIDER_SETTINGS_ACTIVE_INACTIVE");
    },

    FillCityState: function (zipcode, cityname, statename) {
        var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmBillingProviderSettingsDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    PayAddress: function (obj) {

        var bootstrapValidator = $('#frmBillingProviderSettingsDetail').data('bootstrapValidator');

        if ($(obj).is(":checked")) {
            //make fields required.
            billingprovidersettingsDetail.AddressEnalbed = true;
            $("#billingprovidersettingsDetail #pnlEIN #pay_to_address").removeClass('disableAll');
            $("#frmBillingProviderSettingsDetail #pay_to_address span").css('display', 'inline-block');
        }
        else {
            //not required.
            billingprovidersettingsDetail.AddressEnalbed = false;
            $("#billingprovidersettingsDetail #pnlEIN #pay_to_address").addClass('disableAll');
            $("#frmBillingProviderSettingsDetail #pay_to_address span").css('display', 'none');
        }

        bootstrapValidator.enableFieldValidators('PayToZipCode', billingprovidersettingsDetail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayToState', billingprovidersettingsDetail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayToCity', billingprovidersettingsDetail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayToAddress2', billingprovidersettingsDetail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayToAddress1', billingprovidersettingsDetail.AddressEnalbed);

    },

    ShowHistory: function () {
        var PanelID = 'billingprovidersettingsDetail';
        var ParentCtrl = 'billingprovidersettingsDetail';
        var ProfileName = 'Additional Billing Provider';
        var DBTableName = 'BillingProviderSettings';
        var ColumnKeyId = billingprovidersettingsDetail.params.BillingProviderSettingsId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}