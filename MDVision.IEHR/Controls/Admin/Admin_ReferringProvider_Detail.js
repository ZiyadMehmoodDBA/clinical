referringproviderDetail = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {
        referringproviderDetail.params = params;
        if (referringproviderDetail.params.PanelID != "referringproviderDetail") {
            referringproviderDetail.params.PanelID += " #referringproviderDetail";
        }
        var self = $('#' + referringproviderDetail.params.PanelID + ' #tblreferringproviderDetail');
        if (referringproviderDetail.params.Title != null)
            $("#" + referringproviderDetail.params.PanelID).find("#headingTitle").text(referringproviderDetail.params.Title);

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $('#' + referringproviderDetail.params.PanelID + " #tblreferringproviderDetail #divReferringProvider_Entity").css("display", "none");
            //    $('#' + referringproviderDetail.params.PanelID + " #tblreferringproviderDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            referringproviderDetail.LoadReferringProvider();

            //serialize Data after all controls loaded.
            $('#frmdemographicDetail').data('serialize', $('#frmdemographicDetail').serialize());

        });
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail').find('[data-mask]').on('blur', function () {
            // this.$element.trigger('input');
            if ($(this).parent().find('label span').length > 1 && $(this).parent().find('label span').text() =="*")
                $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail').bootstrapValidator('revalidateField', $(this).attr("name"));
        });

        referringproviderDetail.AddCustomValidations();
       
        referringproviderDetail.FillNPIReferringProviderData();
           },

    AddCustomValidations: function()
    {
        $("#tblreferringproviderDetail #txtCity").keypress(function (event) {
            var regex = new RegExp("^[a-zA-Z0-9\-\ ]+$");
            var key = String.fromCharCode(!event.charCode ? event.which : event.charCode);
            if (!regex.test(key)) {
                event.preventDefault();
                return false;
            }
        });
    },

    LoadReferringProvider: function () {
        
        if (referringproviderDetail.params.mode == "Add") {
            $('#frmReferringProviderDetail').data('serialize', $('#frmReferringProviderDetail').serialize());
            referringproviderDetail.ValidateReferringProvider();
        }
        else if (referringproviderDetail.params.mode == "Edit") {
            referringproviderDetail.FillReferringProvider(referringproviderDetail.params.ReferringProviderId, referringproviderDetail.params.EntityId).done(function (response) {
                if (response.status != false) {
                    var ReferringProvider_detail = JSON.parse(response.ReferringProviderFill_JSON);
                    var self = $("#referringproviderDetail");

                    utility.bindMyJSON(true, ReferringProvider_detail, false, self).done(function () {


                        referringproviderDetail.ValidateReferringProvider();
                        if (ReferringProvider_detail.chkActive == 'True')
                            $('#' + referringproviderDetail.params.PanelID + " #chkActive").attr("checked", true);
                        else
                            $('#' + referringproviderDetail.params.PanelID + " #chkActive").attr("checked", false);


                        //serialize Data after all controls loaded.
                        $('#frmdemographicDetail').data('serialize', $('#frmdemographicDetail').serialize());

                    });


                }

                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                $('#frmReferringProviderDetail').data('serialize', $('#frmReferringProviderDetail').serialize());
            });
        }
    },

    ValidateReferringProvider: function () {
        $('#' + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   FirstName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   LastName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   NPI: {
                       group: '.col-sm-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           integer: {
                                 message: '',
                               // The default separators
                                 thousandsSeparator: ''
                           }
                       }
                   },
                   City: {
                       group: '.col-xs-8',
                       enabled: false,
                       validators: {
                           //notEmpty: {
                           //    message: ''
                           //},
                       }
                   },
                   state: {
                       group: '.col-xs-4',
                       validators: {
                           //notEmpty: {
                           //    message: ''
                           //},
                       }
                   },
                   ZipCode: {
                       group: 'col-xs-8',
                       validators: {
                       ////    notEmpty: {
                       //        message: ''
                       //    },
                       }
                   },
                   Entity: {
                       group: '.col-sm-3',
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
                           //emailAddress: {
                           //    message: 'Email not Valid'
                           regexp: {
                               regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                               message: 'Email not Valid'
                           }
                       }
                   }
               }
           }).on('success.form.bv', function (e) {
               e.preventDefault();
               referringproviderDetail.ReferringProviderSave();
           });
    },

    EnableValidation: function (obj) {

        var objprovider = $('#referringproviderDetail #frmReferringProviderDetail');
        var formValidation = objprovider.data("bootstrapValidator");
        if ($(obj).val() != "") {
            formValidation.enableFieldValidators($(obj).attr("name"), true);
        }
        else {
            formValidation.enableFieldValidators($(obj).attr("name"), false);
        }

    },

    ReferringProviderSave: function () {
        var strMessage = "";

        var self = $('#' + referringproviderDetail.params.PanelID);
        var myJSON = self.getMyJSON();
        if (referringproviderDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Referring Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    referringproviderDetail.SaveReferringProvider(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_ReferringProvider.ReferringProviderSearch(response.ReferringProviderId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetRefProviders', true);
                            UnloadActionPan(referringproviderDetail.params["ParentCtrl"], 'referringproviderDetail');
                            //UnloadActionPan();
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
        else if (referringproviderDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Referring Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    referringproviderDetail.UpdateReferringProvider(myJSON, referringproviderDetail.params.ReferringProviderId, 1).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            //CacheManager.BindCodes('GetRefProviders', true);
                            CacheManager.BindCodes('GetRefProviders', true).done(function (result) {
                                if (referringproviderDetail.params.FromAdmin == "0") {
                                    var RefPrvFullName = $('#' + referringproviderDetail.params.PanelID + " #txtLastName").val() + ", " + $('#' + referringproviderDetail.params.PanelID + " #txtFirstName").val();
                                    if (referringproviderDetail.params.RefCtrl == "txtRefProvider")
                                        referringproviderDetail.FillRefProviderName(referringproviderDetail.params.ReferringProviderId, RefPrvFullName);
                                    else if (referringproviderDetail.params.RefCtrl == "txtPCP")
                                        referringproviderDetail.FillPCPName(referringproviderDetail.params.ReferringProviderId, RefPrvFullName);
                                    else {
                                        if (referringproviderDetail.params.ParentCtrl=="Admin_ReferringProvider") {
                                            Admin_ReferringProvider.ReferringProviderSearch(referringproviderDetail.params.ReferringProviderId);
                                        }
                                        UnloadActionPan(referringproviderDetail.params["ParentCtrl"], 'referringproviderDetail');
                                        

                                    }
                                }
                                else {
                                    UnloadActionPan(referringproviderDetail.params["ParentCtrl"], 'referringproviderDetail');
                                    Admin_ReferringProvider.ReferringProviderSearch(referringproviderDetail.params.ReferringProviderId);
                                }
                            });



                            //UnloadActionPan();
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

    //CallCityState: function () {
    //    var zipcode = $('#referringproviderDetail #txtZipCode').val();
    //    var cityname = null;
    //    var statename = null;
    //    // '00601', null
    //    referringproviderDetail.FillCityState(zipcode, cityname, statename).done(function (response) {
    //        if (response.status != false) {
    //            var citystate = JSON.parse(response.CITYSTATE_JSON);
    //            var self = $("#referringproviderDetail");
    //            self.bindMyJSON(true, citystate);
    //            referringproviderDetail.ValidateReferringProvider();
    //        }
    //    });
    //},

    FillPCPName: function (PCPId, PCPName) {
        var PanelID = referringproviderDetail.params["PanelID"];
        if (referringproviderDetail.params["PanelID"].indexOf("referringproviderDetail")>-1) {
            PanelID = referringproviderDetail.params["PanelID"].replace("#referringproviderDetail", "");
        }
        $('#' + PanelID + ' #txtPCP').val(PCPName);
        $('#' + PanelID + ' #hfPCP').val(PCPId);
        $('#' + PanelID + ' #lblPCP').css("display", "none");
        $('#' + PanelID + ' #lnkPCPEdit').css("display", "inline");
        UnloadActionPan(referringproviderDetail.params["ParentCtrl"], "referringproviderDetail");
    },

    FillRefProviderName: function (RefProviderId, RefProviderName) {
        var PanelID = referringproviderDetail.params["PanelID"];
        if (referringproviderDetail.params["PanelID"].indexOf("referringproviderDetail") > -1) {
            PanelID = referringproviderDetail.params["PanelID"].replace("#referringproviderDetail", "");
        }
        $('#' + PanelID + ' #txtRefProvider').val(RefProviderName);
        $('#' + PanelID + ' #hfRefProvider').val(RefProviderId);
        utility.SetKendoAutoCompleteSourceforValidate($('#' + PanelID + ' #txtRefProvider'), $('#' + PanelID + ' #txtRefProvider').val(), $('#' + PanelID + ' #hfRefProvider'), $('#' + PanelID + ' #hfRefProvider').val());

        $('#' + PanelID + ' #lblRefProvider').css("display", "none");
        $('#' + PanelID + ' #lnkRefProviderEdit').css("display", "inline");
        UnloadActionPan(referringproviderDetail.params["ParentCtrl"], "referringproviderDetail");
    },

    SaveReferringProvider: function (ReferringProviderData) {
        var data = "ReferringProviderData=" + ReferringProviderData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER_DETAIL", "SAVE_REFERRING_PROVIDER");
    },

    UpdateReferringProvider: function (ReferringProviderData, ReferringProviderID, IsActive) {
        var data = "ReferringProviderData=" + ReferringProviderData + "&ReferringProviderID=" + ReferringProviderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER_DETAIL", "UPDATE_REFERRING_PROVIDER");
    },

    FillReferringProvider: function (ReferringProviderID, EntityId) {
        var data = "ReferringProviderID=" + ReferringProviderID + "&EntityId="+EntityId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER_DETAIL", "FILL_REFERRING_PROVIDER");
    },

    UpdateReferringProviderActiveInactive: function (ReferringProviderID, IsActive) {
        var data = "ReferringProviderID=" + ReferringProviderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER_DETAIL", "UPDATE_REFERRING_PROVIDER_ACTIVE_INACTIVE");
    },

    //FillCityState: function (zipcode, cityname, statename) {
    //    var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE");
    //},

    UnLoad: function () {

        utility.UnLoadDialog("frmReferringProviderDetail", function () {
            UnloadActionPan(referringproviderDetail.params["ParentCtrl"], 'referringproviderDetail');
        }, function () {
            UnloadActionPan(referringproviderDetail.params["ParentCtrl"], 'referringproviderDetail');
        });

  
    },

    LoadRefProvidersDBCall: function (name) {
        var data = "RefProName=" + name;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER_DETAIL", "LOAD_REFERRING_PROVIDER_AUTOCOMPLETE");
    },

    ShowHistory: function () {
        var PanelID = 'referringproviderDetail';
        var ParentCtrl = 'referringproviderDetail';
        var ProfileName = 'Referring Provider';
        var DBTableName = 'ReferringProvider';
        var ColumnKeyId = specialtyDetail.params.SpecialtyId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },

    //Fill provider data against NPI number by calling NPI API
    FillNPIReferringProviderData:function()
    {
        var ctrl = $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtNPI');
        ctrl.focusout(function () {
            var inputData = ctrl.val();
            if (!(inputData == "" || inputData == "__________" || ctrl.data("ProviderData") == inputData || inputData.replace(/[^0-9]/g, "").length != 10)) {
                providerDetail.FillProviderApiData(inputData).done(function (response) {
                    referringproviderDetail.ReferringPrivdersuccess(response);
                });



            }
        });

        ctrl.focusin(function () {
            ctrl.data("ProviderData", ctrl.val());
        });
    },

    ReferringPrivdersuccess: function (data) {
        try {
        BackgroundLoaderShow(false);
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtFirstName').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtLastName').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtTaxonomyCode').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtAddress').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtAddress2').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtTelephone').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtCity').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtState').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtZipCode').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtZipCodeExt').val("");
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtFax').val("");

        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtFirstName').val(data.results[0].basic.first_name);
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtLastName').val(data.results[0].basic.last_name);
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtTaxonomyCode').val(data.results[0].taxonomies[0].code);
        
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtAddress').val(data.results[0].addresses[1].address_1);
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtAddress2').val(data.results[0].addresses[1].address_2);
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtTelephone').val(data.results[0].addresses[1].telephone_number);
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtCity').val(data.results[0].addresses[1].city);
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtState').val(data.results[0].addresses[1].state);
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtZipCode').val(data.results[0].addresses[1].postal_code.substring(0, 5));
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtZipCodeExt').val(data.results[0].addresses[1].postal_code.substring(5, 9));
        $("#" + referringproviderDetail.params.PanelID + ' #frmReferringProviderDetail #txtFax').val(data.results[0].addresses[1].fax_number);
        } catch (ex) {
            console.log(ex);
            BackgroundLoaderShow(false);
           utility.DisplayMessages("Record not found for given NPI", 2);
        }
        },

    

}









