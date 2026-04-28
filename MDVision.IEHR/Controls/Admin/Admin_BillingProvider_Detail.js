Admin_BillingProvider_Detail = {
    params: [],
    Enable: false,
    AddressEnalbed: false,
    Load: function (params) {
        Admin_BillingProvider_Detail.params = params;
        var self = null;
        if (Admin_BillingProvider_Detail.params.PanelID.indexOf("pnlAdmin_BillingProvider_Detail") < 0)
            self = $('#' + Admin_BillingProvider_Detail.params.PanelID + " #pnlAdmin_BillingProvider_Detail");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        //var self = $('#tblAdmin_BillingProvider_Detail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }

        self.loadDropDowns(true).done(function () {

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            Admin_BillingProvider_Detail.LoadBillingProvider();
            var isBillToEINChecked = self.find("#chkBillToEIN").attr("checked");
            if (isBillToEINChecked != "checked") {
                self.find("#pnlEIN").addClass('disableAll');
            }
            else {
                self.find("#pnlEIN").removeClass('disableAll');

            }

        });

        Admin_BillingProvider_Detail.FillNPIBillingProviderData();
    },

    LoadBillingProvider: function () {
        if (Admin_BillingProvider_Detail.params.mode == "Add") {

            //serialize Data after all controls loaded.
            $('#frmAdmin_BillingProvider_Detail').data('serialize', $('#frmAdmin_BillingProvider_Detail').serialize());
            Admin_BillingProvider_Detail.ValidateBillingProvider();
            Admin_BillingProvider_Detail.PayAddress($("#pnlAdmin_BillingProvider_Detail #chkIsPayToAddress"));

        }
        else if (Admin_BillingProvider_Detail.params.mode == "Edit") {
            Admin_BillingProvider_Detail.FillBillingProvider(Admin_BillingProvider_Detail.params.BillingProviderId).done(function (response) {
                if (response.status != false) {
                    Admin_BillingProvider_Detail.FillBillingProviderData(response);
                    $(self).find("#txtShortName").attr("disabled", "disabled");
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    FillBillingProviderData: function (response) {
        var billingprovider_detail = JSON.parse(response.BillingProviderSettingsFill_JSON);
        var self = $("#pnlAdmin_BillingProvider_Detail");

        utility.bindMyJSON(true, billingprovider_detail, false, self).done(function () {
            Admin_BillingProvider_Detail.ValidateBillingProvider();

            //set is to pay validation
            Admin_BillingProvider_Detail.PayAddress($("#pnlAdmin_BillingProvider_Detail #chkIsPayToAddress"));
            Admin_BillingProvider_Detail.ProfileType($("#pnlAdmin_BillingProvider_Detail #ddlProviderType"));
            //serialize Data after all controls loaded.
            $('#frmAdmin_BillingProvider_Detail').data('serialize', $('#frmAdmin_BillingProvider_Detail').serialize());
        });
    },
    ValidateBillingProvider: function () {
        $('#frmAdmin_BillingProvider_Detail')
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
                   Entity: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ShortName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ProviderType: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   EIN: {
                       group: '.col-sm-7',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LastName: {
                       group: '.col-sm-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   FirstName: {
                       group: '.col-sm-2',
                       enabled: Admin_BillingProvider_Detail.Enable,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   NPI: {
                       group: '.col-xs-5',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Address1: {
                       group: '.col-sm-6',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   City: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   State: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ZipCode: {
                       group: '.col-sm-8',
                       validators: {
                           zipCode: {
                               country: 'US',
                               message: ' '
                           },
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PayToAddress1: {
                       group: '.col-sm-6',
                       enabled: Admin_BillingProvider_Detail.AddressEnalbed,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //PayToAddress2: {
                   //    group: '.col-sm-6',
                   //    enabled: Admin_BillingProvider_Detail.AddressEnalbed,
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   PayToCity: {
                       group: '.col-sm-3',
                       enabled: Admin_BillingProvider_Detail.AddressEnalbed,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PayToState: {
                       group: '.col-sm-3',
                       enabled: Admin_BillingProvider_Detail.AddressEnalbed,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PayToZipCode: {
                       group: '.col-sm-3',
                       enabled: Admin_BillingProvider_Detail.AddressEnalbed,
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
           Admin_BillingProvider_Detail.BillingProviderSave();
       });
    },

    BillingProviderSave: function () {
        var strMessage = "";
        var self = $("#pnlAdmin_BillingProvider_Detail");
        var myJSON = self.getMyJSON();
        if (Admin_BillingProvider_Detail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Billing Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_BillingProvider_Detail.SaveBillingProvider(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_BillingProvider.BillingProviderSearch(response.BillingProviderId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetBillingProviders', true);
                            if (Admin_BillingProvider_Detail.params["unload"] != "CreateClaim") {
                                UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail");
                            } else {
                                UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail", null, "pnlEncounterCreateClaim #pnlAdminBillingProvider");
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
        }
        else if (Admin_BillingProvider_Detail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Billing Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_BillingProvider_Detail.UpdateBillingProvider(myJSON, Admin_BillingProvider_Detail.params.BillingProviderId, 1).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            if (Admin_BillingProvider_Detail.params["ParentCtrl"] && Admin_BillingProvider_Detail.params["ParentCtrl"] == "Admin_BillingProvider") {
                                Admin_BillingProvider.BillingProviderSearch(Admin_BillingProvider_Detail.params.BillingProviderId);
                                CacheManager.BindCodes('GetBillingProviders', true);
                                if (Admin_BillingProvider_Detail.params.RefCtrl != null && Admin_BillingProvider_Detail.params.RefCtrl != "") {
                                    if (Admin_BillingProvider_Detail.params["unload"] != "CreateClaim") {
                                        UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail");
                                    } else {
                                        UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail", null, "pnlEncounterCreateClaim #pnlAdminBillingProvider");
                                    }
                                }
                                else {
                                    Admin_BillingProvider.BillingProviderSearch(Admin_BillingProvider_Detail.params.BillingProviderId);
                                    if (Admin_BillingProvider_Detail.params["unload"] != "CreateClaim") {
                                        UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail");
                                    } else {
                                        UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail", null, "pnlEncounterCreateClaim #pnlAdminBillingProvider");
                                    }
                                }
                            }
                            else {
                                if (Admin_BillingProvider_Detail.params["unload"] != "CreateClaim") {
                                    UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail");
                                } else {
                                    UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail", null, "pnlEncounterCreateClaim #pnlAdminBillingProvider");
                                }
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
        }
    },

    SaveBillingProvider: function (BillingProviderData) {
        var data = "BillingProviderData=" + BillingProviderData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_DETAIL", "SAVE_BILLING_PROVIDER");
    },

    UpdateBillingProvider: function (BillingProviderData, BillingProviderID, IsActive) {
        var data = "BillingProviderData=" + BillingProviderData + "&BillingProviderID=" + BillingProviderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_DETAIL", "UPDATE_BILLING_PROVIDER");
    },

    FillBillingProvider: function (BillingProviderID, NPINumber) {
        var data = "BillingProviderID=" + BillingProviderID + "&NPINumber=" + NPINumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_DETAIL", "FILL_BILLING_PROVIDER");
    },

    UpdateBillingProviderActiveInactive: function (BillingProviderID, IsActive) {
        var data = "BillingProviderID=" + BillingProviderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_DETAIL", "UPDATE_BILLING_PROVIDER_SETTINGS_ACTIVE_INACTIVE");
    },

    FillCityState: function (zipcode, cityname, statename) {
        var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE");
    },

    UnLoad: function () {
        if (Admin_BillingProvider_Detail.params["unload"] != "CreateClaim") {
            utility.UnLoadDialog("frmAdmin_BillingProvider_Detail", function () {
                UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail");
            }, function () {
                UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail");
            });
        } else {
            utility.UnLoadDialog("frmAdmin_BillingProvider_Detail", function () {
                UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail", null, "pnlEncounterCreateClaim #pnlAdminBillingProvider");
            }, function () {
                UnloadActionPan(Admin_BillingProvider_Detail.params["ParentCtrl"], "Admin_BillingProvider_Detail", null, "pnlEncounterCreateClaim #pnlAdminBillingProvider");
            });
        }

    },

    PayAddress: function (obj) {

        var bootstrapValidator = $('#frmAdmin_BillingProvider_Detail').data('bootstrapValidator');

        if ($(obj).is(":checked")) {
            //make fields required.
            Admin_BillingProvider_Detail.AddressEnalbed = true;
            $("#pnlAdmin_BillingProvider_Detail #pnlEIN #pay_to_address").removeClass('disableAll');
            $("#frmAdmin_BillingProvider_Detail #pay_to_address span").css('display', 'inline-block');
        }
        else {
            //not required.
            Admin_BillingProvider_Detail.AddressEnalbed = false;
            $("#pnlAdmin_BillingProvider_Detail #pnlEIN #pay_to_address").addClass('disableAll');
            $("#frmAdmin_BillingProvider_Detail #pay_to_address span").css('display', 'none');
        }

        bootstrapValidator.enableFieldValidators('PayToZipCode', Admin_BillingProvider_Detail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayToState', Admin_BillingProvider_Detail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayToCity', Admin_BillingProvider_Detail.AddressEnalbed);
        //bootstrapValidator.enableFieldValidators('PayToAddress2', Admin_BillingProvider_Detail.AddressEnalbed);
        bootstrapValidator.enableFieldValidators('PayToAddress1', Admin_BillingProvider_Detail.AddressEnalbed);

    },

    ProfileType: function (obj) {
        var bootstrapValidator = $('#frmAdmin_BillingProvider_Detail').data('bootstrapValidator');
        if ($(obj).val() == "1") {
            Admin_BillingProvider_Detail.Enable = true;
            $("#frmAdmin_BillingProvider_Detail #div_fname span").css('display', 'inline-block');
            bootstrapValidator.enableFieldValidators('FirstName', Admin_BillingProvider_Detail.Enable);
        }
        else {
            Admin_BillingProvider_Detail.Enable = false;
            $("#frmAdmin_BillingProvider_Detail #div_fname span").css('display', 'none');
            bootstrapValidator.enableFieldValidators('FirstName', Admin_BillingProvider_Detail.Enable);
        }
    },

    ShowHistory: function () {
        var PanelID = 'Admin_BillingProvider_Detail';
        var ParentCtrl = 'Admin_BillingProvider_Detail';
        var ProfileName = 'Billing Provider';
        var DBTableName = 'BillingProvider';
        var ColumnKeyId = specialtyDetail.params.SpecialtyId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },

    //Fill provider data against NPI number by calling NPI API
    FillNPIBillingProviderData: function () {
        var ctrl = $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail #txtNPI');
        ctrl.focusout(function () {
            var inputData = ctrl.val();
            if (!(inputData == "" || inputData == "__________" || ctrl.data("ProviderData") == inputData || inputData.replace(/[^0-9]/g, "").length != 10))
            {

                Admin_BillingProvider_Detail.FillBillingProvider(0, inputData).done(function (response) {
                    if (response.status != false) {
                        Admin_BillingProvider_Detail.FillBillingProviderData(response);
                    }
                    else {
                        providerDetail.FillProviderApiData(inputData).done(function (response) {
                            Admin_BillingProvider_Detail.PrivderNPIsuccess(response);
                        });
                    }
                });

             
            }
        });

        ctrl.focusin(function () {
            ctrl.data("ProviderData", ctrl.val());
        });
    },

    PrivderNPIsuccess: function (data) {
        try {

            BackgroundLoaderShow(false);
           
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtFirstName').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtLastName').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtTaxonomyCode').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtAddress1').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtAddress2').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtTelephone').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtCity').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #lstState').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtZipCode').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtZipCodeExt').val("");
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtFax').val("");

            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtFirstName').val(data.results[0].basic.first_name);
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtLastName').val(data.results[0].basic.last_name);
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtTaxonomyCode').val(data.results[0].taxonomies[0].code);
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtAddress1').val(data.results[0].addresses[1].address_1);
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtAddress2').val(data.results[0].addresses[1].address_2);
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtTelephone').val(data.results[0].addresses[1].telephone_number);
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtCity').val(data.results[0].addresses[1].city);
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #lstState').val(data.results[0].addresses[1].state);
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtZipCode').val(data.results[0].addresses[1].postal_code.substring(0, 5));
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtZipCodeExt').val(data.results[0].addresses[1].postal_code.substring(5, 9));
            $("#" + Admin_BillingProvider_Detail.params.PanelID + ' #frmAdmin_BillingProvider_Detail  #txtFax').val(data.results[0].addresses[1].fax_number);
        } catch (ex) {
            console.log(ex);
            BackgroundLoaderShow(false);
            utility.DisplayMessages("Record not found for given NPI", 2);
        }
    },

}