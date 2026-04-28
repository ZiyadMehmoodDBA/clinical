Admin_FaxSettingsDetail = {
    params: [],
    Image_eSignature_div_via_Device_Id: "imageBox_forDevice_eSignature",

    // ------- eSignature Global Variables -----------
    wgssSignatureSDK: null,
    sigObj: null,
    sigCtl: null,
    dynCapt: null,
    signaturePad: null,
    SignatureType: 'Mouse',
    MyData: "",
    eSignatureimg: "",
    IsCustomCoverPage: false,

    Load: function (params) {

        Admin_FaxSettingsDetail.params = params;
        EMRUtility.SwicthWidgetInializatoin();

        var tick = $('#switchActive');
        $(tick).attr('IsActive', '0');

        if (Admin_FaxSettingsDetail.params.type == "Provider") {
            // show provider forms
            $("#txtTimeZone option[timeZoneId=15]").attr("selected", "selected");
            $('#fileToUpload').on('change', function () {
                var files = $('#fileToUpload').get(0).files;
                if (files.length > 0) {
                    var filename = files[0].name;
                    var extension = filename.replace(/^.*\./, '');
                    if (extension == filename) {
                        extension = '';
                    } else {
                        extension = extension.toLowerCase();
                    }
                    if (extension == 'pdf') {
                        $('#FileProviderMessage').text(files[0].name);
                        files = $('#fileToUpload').get(0).files;
                    } else {
                        utility.DisplayMessages("Please attach PDF files only.", 4);
                    }
                }
            })
            var checker = $('#switchActiveProvider');
            Admin_FaxSettingsDetail.changeSwitch(checker);
            var ddr = $("#pnlProvider_FaxSetting");
            ddr.show();
            ddr.find("#providerUserDropdown").loadDropDowns(true, "IsActive=true&ID=1", Admin_FaxSettingsDetail.params["PanelID"]);
            ddr.find("#providerDropdown").loadDropDowns(true, "IsActive=", Admin_FaxSettingsDetail.params["PanelID"]).done(function () {
                $('#chkUser').on('change', function () {
                    Admin_FaxSettingsDetail.AddUser().done(function () {
                        Admin_FaxSettingsDetail.LoadUsers();
                    });
                });
                $('#chkProvider').on('change', function () {
                    Admin_FaxSettingsDetail.LoadUsers();
                });
                if (Admin_FaxSettingsDetail.params.mode == "Add") {
                    Admin_FaxSettingsDetail.ValidateProvider();
                    $('#chkProvider').on('change', function () {
                        if ($('#pnlAdminFaxSettingsDetail #chkProvider').children(':selected').text() == "- Select -") {
                            $('#pnlAdminFaxSettingsDetail #txtFirstName').val("");
                            $('#pnlAdminFaxSettingsDetail #txtFirstName').attr('disabled', false);
                            $('#pnlAdminFaxSettingsDetail #txtMiddleName').val("");
                            $('#pnlAdminFaxSettingsDetail #txtMiddleName').attr('disabled', false);
                            $('#pnlAdminFaxSettingsDetail #txtLastName').val("");
                            $('#pnlAdminFaxSettingsDetail #txtLastName').attr('disabled', false);
                        }
                        else {
                            var providerId = $("#frmProviderId").getMyJSON();
                            providerId = JSON.parse(providerId);
                            var ProviderData = null;
                            var data = "ProviderData=" + ProviderData + "&ProviderID=" + providerId.chkProvider + "&PageNumber=1&RowsPerPage=15";
                            MDVisionService.defaultService(data, "ADMIN_PROVIDER", "SEARCH_PROVIDER").done(function (resp) {
                                resp = JSON.parse(resp.ProviderLoad_JSON);
                                Admin_FaxSettingsDetail.fillForms(resp);
                            });
                        }
                    });
                }

                if (Admin_FaxSettingsDetail.params.mode == "Edit") {

                    $("#pnlAdminFaxSettingsDetail #chkProvider option[value='" + Admin_FaxSettingsDetail.params.Id + "']").attr("selected", "selected");
                    $('#pnlAdminFaxSettingsDetail #chkProvider').prop("disabled", "disabled");

                    //   $('#eSignatureSaveButton').attr('onclick', 'Admin_FaxSettingsDetail.saveProvider();');
                    // $('.required').hide();
                    Admin_FaxSettingsDetail.ValidateProvider();
                    Admin_FaxSettingsDetail.LoadUsers();
                    var providerDataStr = $("#frmProviderDetails").getMyJSON();

                    var providerData = JSON.parse(providerDataStr);
                    providerData["ProviderId"] = Admin_FaxSettingsDetail.params.Id;
                    Admin_FaxSettings.loadProvidersFaxSettings(providerData).done(function (response) {
                        var faxSettingJSON = JSON.parse(response.ProviderFaxFill_JSON);
                        Admin_FaxSettingsDetail.params.FaxSettingId = faxSettingJSON[0].FaxSettingId;
                        Admin_FaxSettingsDetail.fillForms(faxSettingJSON);
                    });
                }
            });
        }
        else if (Admin_FaxSettingsDetail.params.type == "Facility") {
            $("#txtTimeZoneFacility option[timeZoneId=15]").attr("selected", "selected");

            $('#fileToUploadFacility').on('change', function () {
                var files = $('#fileToUploadFacility').get(0).files;
                //  var files = $('#fileToUpload').get(0).files;
                if (files.length > 0) {
                    var filename = files[0].name;
                    var extension = filename.replace(/^.*\./, '');
                    if (extension == filename) {
                        extension = '';
                    } else {
                        extension = extension.toLowerCase();
                    }
                    if (extension == 'pdf') {
                        $('#FileMessage').text(files[0].name);
                        files = $('#fileToUploadFacility').get(0).files;
                    } else {
                        utility.DisplayMessages("Please attach PDF files only.", 4);
                    }
                }
            })
            var checker = $('#switchActiveFacility');
            Admin_FaxSettingsDetail.changeSwitch(checker);

            // show facility forms
            var ddr = $("#pnlFacility_FaxSetting");
            ddr.show();
            ddr.find("#facilityUserDropdown").loadDropDowns(true, "IsActive=true&ID=1", Admin_FaxSettingsDetail.params["PanelID"]);
            ddr.find("#facilityDropdown").loadDropDowns(true, "IsActive=", Admin_FaxSettingsDetail.params["PanelID"]).done(function () {
                $('#chkFacilityUser').on('change', function () {
                    if ($('#chkFacility').val()) {
                        Admin_FaxSettingsDetail.FacilityAddUser().done(function () {
                            Admin_FaxSettingsDetail.FacilityLoadUsers();
                        });
                    }
                    else {
                        utility.DisplayMessages("Facility Not Selected!", 2);
                    }
                });
                $('#chkFacility').on('change', function () {
                    Admin_FaxSettingsDetail.FacilityLoadUsers();
                });
                if (Admin_FaxSettingsDetail.params.mode == "Add") {
                    Admin_FaxSettingsDetail.ValidateFacility();
                    $('#chkFacility').on('change', function () {
                        if ($('#chkFacility').children(':selected').text() == "- Select -") {
                            $('#frmFacilityDetails :input#txtShortName').val("");
                            $('#frmFacilityDetails :input#txtShortName').attr('disabled', false);
                        }
                        else {
                            var FacilityId = $("#frmFacilityId").getMyJSON();
                            FacilityId = JSON.parse(FacilityId);
                            var FacilityData = null;
                            var data = "FacilityData=" + FacilityData + "&FacilityID=" + FacilityId.chkFacility + "&PageNumber=1&RowsPerPage=15";
                            MDVisionService.defaultService(data, "ADMIN_FACILITY", "SEARCH_FACILITY").done(function (resp) {
                                resp = JSON.parse(resp.FacilityLoad_JSON);
                                Admin_FaxSettingsDetail.fillFormsFacility(resp);
                            });
                        }

                    });
                }

                if (Admin_FaxSettingsDetail.params.mode == "Edit") {

                    $("#chkFacility option[value='" + Admin_FaxSettingsDetail.params.Id + "']").attr("selected", "selected");
                    $('#chkFacility').prop("disabled", "disabled");

                    //     $('#eSignatureSaveButtonFacility').attr('onclick', 'Admin_FaxSettingsDetail.saveFacility();');

                    // $('.required').hide();
                    Admin_FaxSettingsDetail.ValidateFacility();
                    Admin_FaxSettingsDetail.FacilityLoadUsers();

                    var FacilityDataStr = $("#frmFacilityDetails").getMyJSON();

                    var FacilityData = JSON.parse(FacilityDataStr);
                    FacilityData["FacilityId"] = Admin_FaxSettingsDetail.params.Id;
                    Admin_FaxSettings.loadFacilitysFaxSettings(FacilityData).done(function (response) {
                        var faxSettingJSON = JSON.parse(response.FacilityFaxFill_JSON);
                        Admin_FaxSettingsDetail.params.FaxSettingId = faxSettingJSON[0].FaxSettingId;
                        Admin_FaxSettingsDetail.fillFormsFacility(faxSettingJSON);
                    });
                }
            });
        }

        Admin_FaxSettingsDetail.OnLoad();
        Admin_FaxSettingsDetail.setSavedImage_on_eSignatureForm();
        Admin_FaxSettingsDetail.InitializeCanvas();

        $("#txtUsers").keypress(function () {
            Admin_FaxSettingsDetail.searchUsers();
        });
    },

    ValidateProvider: function () {
        $('#frmProviderDetails')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    displayname: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    firstname: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    lastname: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    companyname: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    faxno: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    timezone: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    }

                }
            }).on('success.form.bv', function (e) {
                e.preventDefault();

                if ($('#chkProvider').val()) {

                    Admin_FaxSettingsDetail.saveProvider();
                }
                else {
                    utility.DisplayMessages("Please select a provider first", 3);
                }

            });

    },

    saveProviderFaxSettings: function () {

        var uploadData = new FormData();
        var files = $('#fileToUpload').get(0).files;

        for (var i = 0; i < files.length; i++) {
            uploadData.append("file_" + i, files[i]);
        }

        var providerId = $("#frmProviderId").getMyJSON();
        providerId = JSON.parse(providerId);
        var providerDataStr = $("#frmProviderDetails").getMyJSON();
        var providerData = JSON.parse(providerDataStr);

        if (Admin_FaxSettingsDetail.params.mode == "Add") {
            providerData["ProviderId"] = providerId.chkProvider;
            // Check if settings already exists against this provider 

            Admin_FaxSettings.loadProvidersFaxSettings(providerData).done(function (resp) {
                if (resp.status == false) {  // means it does not exist already
                    providerData["IsCustomCoverPage"] = Admin_FaxSettingsDetail.IsCustomCoverPage;
                    providerData["Is_esignatured"] = true;
                    providerData["eSignature"] = Admin_FaxSettingsDetail.eSignatureimg;
                    providerData["CreatedBy"] = globalAppdata['AppUserName'];
                    providerData["ModifiedBy"] = globalAppdata['AppUserName'];

                    if (Admin_FaxSettingsDetail.IsCustomCoverPage == true) {
                        var reader = new FileReader();
                        if (files.length > 0) {
                            reader.readAsDataURL(files[0]);
                            reader.onload = function () {
                                //   var base64 = 
                                //  base64 = base64.substring(28, base64.length);
                                providerData["CoverPage"] = reader.result;

                                Admin_FaxSettingsDetail.faxSaveProviderSettings(providerData).done(function (response) {

                                    utility.DisplayMessages("Settings have been saved.", 1);
                                    Admin_FaxSettingsDetail.UnLoadTab();

                                }
                                );

                            };
                            reader.onerror = function (error) {
                                utility.DisplayMessages("Files were not uploaded", 4)
                            };
                        }
                        else {
                            utility.DisplayMessages("Please upload a file to continue.", 4)
                        }
                    }
                    else {

                        providerData["CoverPage"] = null;
                        Admin_FaxSettingsDetail.faxSaveProviderSettings(providerData).done(function () {
                            utility.DisplayMessages("Settings have been Saved.", 1);
                            Admin_FaxSettingsDetail.UnLoadTab();
                        });
                    }
                }
                else {
                    utility.DisplayMessages("Settings already Exist against this provider.", 4);
                }
            });
        }
        else {
            providerData["ProviderId"] = Admin_FaxSettingsDetail.params.Id; // Id will be locked
            providerData["IsCustomCoverPage"] = Admin_FaxSettingsDetail.IsCustomCoverPage;
            providerData["Is_esignatured"] = true;
            providerData["eSignature"] = Admin_FaxSettingsDetail.eSignatureimg;
            providerData["CreatedBy"] = globalAppdata['AppUserName'];
            providerData["ModifiedBy"] = globalAppdata['AppUserName'];

            if (Admin_FaxSettingsDetail.IsCustomCoverPage == true) {
                var reader = new FileReader();
                if (files.length > 0) {
                    reader.readAsDataURL(files[0]);
                    reader.onload = function () {
                        //var base64 = 
                        //  base64 = base64.substring(28, base64.length);
                        providerData["CoverPage"] = reader.result;

                        Admin_FaxSettingsDetail.faxSaveProviderSettings(providerData).done(function () {



                            utility.DisplayMessages("Settings have been updated.", 1);
                            Admin_FaxSettingsDetail.UnLoadTab();


                        });
                    };
                    reader.onerror = function (error) {
                    };
                }
                else {
                    utility.DisplayMessages("Please upload a file to continue.", 4);
                }
            }
            else {
                providerData["CoverPage"] = "";
                Admin_FaxSettingsDetail.faxSaveProviderSettings(providerData).done(function () {
                    utility.DisplayMessages("Settings have been updated.", 1);
                    Admin_FaxSettingsDetail.UnLoadTab();
                });
            }
        }
    },

    faxSaveProviderSettings: function (data) {
        var strdata = JSON.stringify(data);
        strdata = "ProviderFaxData=" + strdata;
        if (Admin_FaxSettingsDetail.params.mode == "Add") {
            resp = MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "SAVE_PROVIDER");
        }
        else {
            resp = MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "UPDATE_PROVIDER");
        }
        return resp; //ionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "UPDATE_PROVIDER");
    },

    saveProvider: function () {
        Admin_FaxSettingsDetail.SavePictureNew();
        Admin_FaxSettingsDetail.saveProviderFaxSettings();
    },

    AddUser: function () {
        var users = $('#frmUserId').getMyJSON();

        var providerData = JSON.parse(users);
        var providerId = $("#frmProviderId").getMyJSON();

        providerId = JSON.parse(providerId);
        providerData["ProviderId"] = providerId.chkProvider;
        providerData["UserId"] = providerData["chkUser"];
        if (providerData["ProviderId"] == "" || providerData["ProviderId"] == null) {
            utility.DisplayMessages("Please select a provider first", 2);
        }
        else {
            var strdata = JSON.stringify(providerData);
            strdata = "ProviderUserData=" + strdata;
            var x = MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "SAVE_PROVIDER_USER");
        }
        return x;
    },

    LoadUsers: function () {

        var users = $("#frmUserId").getMyJSON();
        var providerId = $("#frmProviderId").getMyJSON();
        var providerData = JSON.parse(users);
        providerId = JSON.parse(providerId);
        providerData["ProviderId"] = providerId.chkProvider;
        if (providerData["ProviderId"] == "" || providerData["ProviderId"] == null) {
            //   utility.DisplayMessages("Please select a provider", 2)
        }
        else {
            Admin_FaxSettingsDetail.loadUsersProvider(providerData).done(function (resp) {
                Admin_FaxSettingsDetail.UserGridLoad(resp);
            });
        }
    },

    UserGridLoad: function (resp) {
        var rows = "";
        var userTable = $('#dgvUserFaxSettings');
        $('#dgvUserFaxSettings').dataTable().fnDestroy();
        $("#dgvUserFaxSettings tbody").find("tr").remove();
        if (resp.ProviderFaxFill_JSON != null || resp.ProviderFaxFill_JSON != undefined) {
            var data = JSON.parse(resp.ProviderFaxFill_JSON);

            if (data.length > 0) {

                $.each(data, function (i, item) {
                    rows += '<tr><td style="display:none;">' + item.UserId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_FaxSettingsDetail.DeleteUsers(' + item.UserId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td><td>' + item.UserName + '</td></tr>';
                });
            }
            else {
                rows += '<tr><td style="display:none;">' + item.UserId + '</td><td></td>No User Found.<td></td></tr>';
            }
        }
        else {

            rows += '<tr><td style="display:none;"></td><td colspan="2"><center>No User Found.</td></center><td style="display:none;"></td></tr>';
        }

        userTable.append(rows);
    },

    DeleteUsers: function (Id) {
        var users = $("#frmUserId").getMyJSON();
        var providerId = $("#frmProviderId").getMyJSON();
        var providerData = JSON.parse(users);
        providerId = JSON.parse(providerId);
        providerData["ProviderId"] = providerId.chkProvider;
        providerData["UserId"] = Id;

        var strdata = JSON.stringify(providerData);
        strdata = "ProviderUserData=" + strdata;


        MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "DELETE_PROVIDER_USER").done(function () {
            Admin_FaxSettingsDetail.LoadUsers();
        });


    },

    loadUsersProvider: function (providerData) {
        var strdata = JSON.stringify(providerData);
        strdata = "ProviderUserData=" + strdata;
        return MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "LOAD_PROVIDER_USER");
    },

    // Facility 

    ValidateFacility: function () {
        $('#frmFacilityDetails')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    displayname: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    shortname: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    faxno: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    timezone: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    }
                }
            }).on('success.form.bv', function (e) {
                e.preventDefault();

                if ($('#chkFacility').val()) {


                    Admin_FaxSettingsDetail.saveFacility();
                }


            });

    },
    saveFacilityFaxSettings: function () {

        var files = $('#fileToUploadFacility').get(0).files;

        var FacilityId = $("#frmFacilityId").getMyJSON();

        FacilityId = JSON.parse(FacilityId);
        var FacilityDataStr = $("#frmFacilityDetails").getMyJSON();

        var FacilityData = JSON.parse(FacilityDataStr);
        if (Admin_FaxSettingsDetail.params.mode == "Add") {
            FacilityData["FacilityId"] = FacilityId.chkFacility;
            // Check if settings already exists against this Facility 
            Admin_FaxSettings.loadFacilitysFaxSettings(FacilityData).done(function (resp) {

                if (resp.status == false) {  // means it exists already
                    FacilityData["IsCustomCoverPage"] = Admin_FaxSettingsDetail.IsCustomCoverPage;
                    FacilityData["Is_esignatured"] = true;
                    FacilityData["eSignature"] = Admin_FaxSettingsDetail.eSignatureimg;
                    FacilityData["CreatedBy"] = globalAppdata['AppUserName'];
                    FacilityData["ModifiedBy"] = globalAppdata['AppUserName'];
                    FacilityData["CoverPage"] = "";
                    if (Admin_FaxSettingsDetail.IsCustomCoverPage == true) {
                        var reader = new FileReader();

                        if (files.length > 0) {
                            reader.readAsDataURL(files[0]);
                            reader.onload = function () {

                                // var base64 = reader.result;
                                //  base64 = base64.substring(28, base64.length);
                                FacilityData["CoverPage"] = reader.result;
                                $('#FileMessage').html("File is uploaded");
                                Admin_FaxSettingsDetail.faxSaveFacilitySettings(FacilityData).done(function () {

                                    utility.DisplayMessages("Settings have been saved!", 1);
                                    Admin_FaxSettingsDetail.UnLoadTab();
                                });
                            };
                            reader.onerror = function (error) {
                                utility.DisplayMessages("File was not uploaded.", 4);
                            };
                        }
                        else {
                            utility.DisplayMessages("Please upload a file to continue.", 4);
                        }
                    }
                    else {
                        if (Admin_FaxSettingsDetail.IsCustomCoverPage == false) {
                            FacilityData["CoverPage"] = null;
                            Admin_FaxSettingsDetail.faxSaveFacilitySettings(FacilityData).done(function () {
                                utility.DisplayMessages("Settings have been saved!", 1);
                                Admin_FaxSettingsDetail.UnLoadTab();
                            });
                        }
                        else {
                            utility.DisplayMessages("Please upload a file to continue.", 4);
                        }
                    }
                }
                else {
                    utility.DisplayMessages("Settings already Exist against this Facility.", 4);
                }
            });
        }
        else {
            FacilityData["FacilityId"] = Admin_FaxSettingsDetail.params.Id; // Id will be locked
            FacilityData["IsCustomCoverPage"] = Admin_FaxSettingsDetail.IsCustomCoverPage;
            FacilityData["CreatedBy"] = globalAppdata['AppUserName'];
            FacilityData["ModifiedBy"] = globalAppdata['AppUserName'];
            FacilityData["CoverPage"] = "";
            if (Admin_FaxSettingsDetail.IsCustomCoverPage == true) {
                var reader = new FileReader();
                if (files.length > 0) {
                    reader.readAsDataURL(files[0]);
                    reader.onload = function () {

                        // var base64 = reader.result;
                        // base64 = base64.substring(28, base64.length);
                        FacilityData["CoverPage"] = reader.result;
                        $('#FileMessage').html("File has been uploaded.");
                        Admin_FaxSettingsDetail.faxSaveFacilitySettings(FacilityData).done(function () {
                            utility.DisplayMessages("Settings have been Updated!", 1);
                            Admin_FaxSettingsDetail.UnLoadTab();
                        });
                    };
                    reader.onerror = function (error) {
                        utility.DisplayMessages("File was not uploaded.", 4);
                    };
                }
                else {
                    utility.DisplayMessages("Please upload a file to continue.", 4);
                }
            }
            else {
                if (FacilityData["CoverPage"] != "") {
                    Admin_FaxSettingsDetail.faxSaveFacilitySettings(FacilityData);
                    Admin_FaxSettingsDetail.UnLoadTab();
                }
                Admin_FaxSettingsDetail.faxSaveFacilitySettings(FacilityData).done(function () {
                    utility.DisplayMessages("Settings have been updated!", 1);
                    Admin_FaxSettingsDetail.UnLoadTab();
                });
            }
        }
    },
    faxSaveFacilitySettings: function (data) {
        var strdata = JSON.stringify(data);
        strdata = "FacilityFaxData=" + strdata;

        if (Admin_FaxSettingsDetail.params.mode == "Add") {
            resp = MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "SAVE_FACILITY");
        }
        else {
            resp = MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "UPDATE_FACILITY");
        }
        return resp;
    },
    saveFacility: function () {

        Admin_FaxSettingsDetail.saveFacilityFaxSettings();
    },

    FacilityAddUser: function () {
        var users = $('#frmFacilityUserId').getMyJSON();

        var FacilityData = JSON.parse(users);
        var FacilityId = $("#frmFacilityId").getMyJSON();

        FacilityId = JSON.parse(FacilityId);
        FacilityData["FacilityId"] = FacilityId.chkFacility;
        FacilityData["UserId"] = FacilityData["chkFacilityUser"];
        if (FacilityData["FacilityId"] == "" || FacilityData["FacilityId"] == null) {
            utility.DisplayMessages("Please select a Facility first", 2);
        }
        else {

            var strdata = JSON.stringify(FacilityData);
            strdata = "FacilityUserData=" + strdata;


            var x = MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "SAVE_FACILITY_USER");
        }
        return x;
    },
    GetFacilityFieldsUsers: function () {
        var users = $("#frmFacilityUserId").getMyJSON();
        var FacilityId = $("#frmFacilityId").getMyJSON();
        var FacilityData = JSON.parse(users);
        FacilityId = JSON.parse(FacilityId);
        FacilityData["FacilityId"] = FacilityId.chkFacility;
        FacilityData["UserId"] = FacilityData["chkFacilityUser"];
        //if (FacilityData["FacilityId"] == "" || FacilityData["FacilityId"] == null) {

        //}
        return FacilityData;
    },
    FacilityLoadUsers: function () {


        var users = $("#frmUserId").getMyJSON();
        var FacilityId = $("#frmFacilityId").getMyJSON();
        var FacilityData = JSON.parse(users);
        FacilityId = JSON.parse(FacilityId);
        FacilityData["FacilityId"] = FacilityId.chkFacility;
        if (FacilityData["FacilityId"] == "" || FacilityData["FacilityId"] == null) {
            utility.DisplayMessages("Please select a Facility", 2)
        }
        else {
            Admin_FaxSettingsDetail.loadUsersFacility(FacilityData).done(function (resp) {
                Admin_FaxSettingsDetail.FacilityUserGridLoad(resp);

            });
        }
    },
    FacilityUserGridLoad: function (resp) {
        var rows = "";
        var userTable = $('#dgvUserFacilityFaxSettings');

        $('#dgvUserFacilityFaxSettings').dataTable().fnDestroy();
        $("#dgvUserFacilityFaxSettings tbody").find("tr").remove();

        if (resp.FacilityFaxFill_JSON != null || resp.FacilityFaxFill_JSON != undefined) {
            var data = JSON.parse(resp.FacilityFaxFill_JSON);

            if (data.length > 0) {

                $.each(data, function (i, item) {
                    rows += '<tr><td style="display:none;">' + item.UserId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_FaxSettingsDetail.FacilityDeleteUsers(' + item.UserId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;</td><td>' + item.UserName + '</td></tr>';
                });
            }
            else {
                rows += '<tr><td style="display:none;">' + item.UserId + '</td><td></td>No User Found.<td></td></tr>';
            }
        }
        else {
            rows += '<tr><td style="display:none;"></td><td colspan="2"><center>No User Found.</td></center><td style="display:none;"></td></tr>';
        }
        userTable.append(rows);
    },
    FacilityDeleteUsers: function (Id) {
        var users = $("#frmFacilityUserId").getMyJSON();
        var FacilityId = $("#frmFacilityId").getMyJSON();
        var FacilityData = JSON.parse(users);
        FacilityId = JSON.parse(FacilityId);
        FacilityData["FacilityId"] = FacilityId.chkFacility;
        FacilityData["UserId"] = Id;

        var strdata = JSON.stringify(FacilityData);
        strdata = "FacilityUserData=" + strdata;


        MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "DELETE_FACILITY_USER").done(function () {
            Admin_FaxSettingsDetail.FacilityLoadUsers();
        });


    },
    loadUsersFacility: function (FacilityData) {

        var strdata = JSON.stringify(FacilityData);
        strdata = "FacilityUserData=" + strdata;


        return MDVisionService.defaultService(strdata, "ADMIN_PROVIDER_FAXSETTINGS", "LOAD_FACILITY_USER");

    },

    fillForms: function (resp) {
        var item = resp[0];
        if (Admin_FaxSettingsDetail.params.mode == "Add") {

            $('#pnlAdminFaxSettingsDetail #txtFirstName').val(item.FirstName);
            $('#pnlAdminFaxSettingsDetail #txtFirstName').attr('disabled', 'disabled');
            $('#pnlAdminFaxSettingsDetail #txtMiddleName').val(item.MiddleName);
            $('#pnlAdminFaxSettingsDetail #txtMiddleName').attr('disabled', 'disabled');
            $('#pnlAdminFaxSettingsDetail #txtLastName').val(item.LastName);
            $('#pnlAdminFaxSettingsDetail #txtLastName').attr('disabled', 'disabled');

        }
        if (Admin_FaxSettingsDetail.params.mode == "Edit") {

            $('#pnlAdminFaxSettingsDetail #txtDisplayName').val(item.DisplayName);
            $('#pnlAdminFaxSettingsDetail #txtFirstName').val(item.FirstName);
            $('#pnlAdminFaxSettingsDetail #txtFirstName').attr('disabled', 'disabled');
            $('#pnlAdminFaxSettingsDetail #txtMiddleName').val(item.MiddleName);
            $('#pnlAdminFaxSettingsDetail #txtMiddleName').attr('disabled', 'disabled');
            $('#pnlAdminFaxSettingsDetail #txtLastName').val(item.LastName);
            $('#pnlAdminFaxSettingsDetail #txtLastName').attr('disabled', 'disabled');
            $('#pnlAdminFaxSettingsDetail #txtCompanyName').val(item.CompanyName);
            $('#pnlAdminFaxSettingsDetail #txtPhoneNo').val(item.PhoneNo);
            $('#pnlAdminFaxSettingsDetail #txtFaxNo').val(item.FaxNo);
            if (item.HasCoverPage == "True" || item.HasCoverPage == "true") {
                $('#txtCover').show();
                $('#txtCover').text("You have already added a cover page.");
            }
            else if (item.HasCoverPage == "False" || item.HasCoverPage == "false") {
                $('#txtCover').show();
                $('#txtCover').text("You are currently using default cover page.");
            }
        }

    },

    fillFormsFacility: function (resp) {

        var item = resp[0];

        if (Admin_FaxSettingsDetail.params.mode == "Add") {

            $('#frmFacilityDetails :input#txtShortName').val(item.ShortName);
            $('#frmFacilityDetails :input#txtShortName').attr('disabled', 'disabled');
        } else {

            $('#frmFacilityDetails :input#txtDisplayName').val(item.DisplayName);
            $('#frmFacilityDetails :input#txtShortName').val(item.ShortName);
            $('#frmFacilityDetails :input#txtShortName').attr('disabled', 'disabled');
            $('#frmFacilityDetails :input#txtCompanyName').val(item.CompanyName);
            $('#frmFacilityDetails :input#txtPhoneNo').val(item.PhoneNo);
            $('#frmFacilityDetails :input#txtFaxNo').val(item.FaxNo);
            if (item.HasCoverPage == "True" || item.HasCoverPage == "true") {
                $('#txtCoverFacility').show();
                $('#txtCoverFacility').text("You have already added a cover page.");
            }
            else if (item.HasCoverPage == "False" || item.HasCoverPage == "false") {
                $('#txtCoverFacility').show();
                $('#txtCoverFacility').text("You are currently using default cover page.");
            }
        }
    },
    UnLoadTab: function () {
        if (Admin_FaxSettingsDetail.params.type == "Provider") {

            var switcher = $('#pnlAdminFaxSettings #switchActive');
            $(switcher).attr('IsActive', '0');
            Admin_FaxSettings.changeSwitch(switcher[0]);
        }
        else {


            var switcher = $('#pnlAdminFaxSettings #switchActive');
            $(switcher).attr('IsActive', '1');
            Admin_FaxSettings.changeSwitch(switcher[0]);

        }
        if (Admin_FaxSettingsDetail.params != null && Admin_FaxSettingsDetail.params.ParentCtrl) {
            UnloadActionPan(Admin_FaxSettingsDetail.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }

        Admin_FaxSettings.loadProviders();
        Admin_FaxSettings.loadFacilities();

    },

    changeSwitch: function (objThis) {
        var IsActive = $(objThis).attr('IsActive');

        if (IsActive == '1') {
            $('#uploadButton').hide("slow", function () {
            });
            $('#uploadFacilityButton').hide("slow", function () {
            });
            Admin_FaxSettingsDetail.IsCustomCoverPage = false;
            $(objThis).attr('IsActive', '0');
        }
        else if (IsActive == '0') {
            $('#uploadButton').show("slow", function () {
            });

            $('#uploadFacilityButton').show("slow", function () {
            });

            Admin_FaxSettingsDetail.IsCustomCoverPage = true;
            $(objThis).attr('IsActive', '1');

        }
    },

    // Signatures

    InitializeCanvas: function () {
        var wrapper1 = document.getElementById("eSignatureMousepad"),
           canvas = wrapper1.querySelector("canvas");

        // Adjust canvas coordinate space taking into account pixel ratio,
        // to make it look crisp on mobile devices.
        // This also causes canvas to be cleared.
        function resizeCanvas() {

            // When zoomed out to less than 100%, for some very strange reason,
            // some browsers report devicePixelRatio as less than 1
            // and only part of the canvas is cleared then.

            var ratio = Math.max(window.devicePixelRatio || 1, 1);
            canvas.width = (canvas.offsetWidth > 314 ? canvas.offsetWidth : 315) * ratio;
            canvas.height = 125;// (canvas.offsetHeight > 124 ? canvas.offsetWidth : 125) * ratio;
            canvas.getContext("2d").scale(ratio, ratio);
        }

        window.onresize = resizeCanvas;
        resizeCanvas();

        Admin_Provider_eSignature.signaturePad = new SignaturePad(canvas, { minWidth: 0.7, maxWidth: 1.4 });
    },

    Create_eSignatureImage_via_Device: function () {
        Admin_Provider_eSignature.Capture();
    },

    BrowseScanned_eSignatureImage: function () {

    },

    Undo_eSignature: function () {
        if (Admin_Provider_eSignature.SignatureType == "Picture") {

            Admin_Provider_eSignature.InitializeCanvas();

            $(Admin_Provider_eSignature.params.PanelID + ' #Upload_Image_file').val('');
        }
        else if (Admin_Provider_eSignature.SignatureType == "Mouse") {
            Admin_Provider_eSignature.signaturePad.clear();
        }
    },

    Save_eSignature: function () { },

    BufferFile: function (input) {

        Admin_Provider_eSignature.SignatureType = "Picture";
        Admin_Provider_eSignature.InitializeCanvas();

        if (input.files) {
            if (Document_Import.ValidateFileSize(input.files) > 2) {

                utility.DisplayMessages("Maximum 2MB  is allowed", 4);
                $(Admin_Provider_eSignature.params.PanelID + ' #Upload_Image_file').val('');
                return false;
            }
            else {
                var fileType = input.files[0].type.toLowerCase();

                if (fileType == "image/jpeg" || fileType == "image/png" || fileType == "image/jpg" || fileType == "image/gif" || fileType == "image/bmp") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        Admin_Provider_eSignature.drawImagetoCanvas(e.target.result);
                    }

                    Admin_Provider_eSignature.SignatureType = "Picture";
                    reader.readAsDataURL(input.files[0]);
                }
                else {
                    utility.DisplayMessages("Only JPG, PNG, BMP, JPEG, GIF files are allowed.", 2);
                    $(Admin_Provider_eSignature.params.PanelID + ' #Upload_Image_file').val(null);

                    Admin_Provider_eSignature.drawImagetoCanvas("");
                }
            }
        }
    },

    SavePictureNew: function () {

        var chkValue = $('#pnleSignatureDetail input:radio[name=03]:checked').val();
        chkValue = "Picture";

        var base64 = null;
        var Image_placer_ID = "#pnleSignatureDetail #imgUploadImage";
        //var IS_Image_Exists = !($(Image_placer_ID).attr('src') === "" || $(Image_placer_ID).attr('src') === undefined
        //    || $(Image_placer_ID).attr('src') === null || $(Image_placer_ID).attr('src') === "null");
        //   var eSignature_via_BrowsePicture = $('#Upload_Image_file').val() != "";

        if (Admin_Provider_eSignature.params.ParentCtrl == "patTabInsurance" || Admin_Provider_eSignature.params.ParentCtrl == "Patient_Insurance") {
            //if (chkValue == "Picture") {
            //    if ($('#Upload_Image_file').val() != "") {
            //        base64 = $('#pnleSignatureDetail #imgUploadImage').attr('src');
            //        Patient_Insurance.setImageSource(base64);
            //    }
            //    else {
            //        utility.DisplayMessages("Please Upload File First", 3);
            //        return false;
            //    }
            //}
        }
        else {

            if (Admin_Provider_eSignature.SignatureType == "Picture") {

                base64 = document.getElementById('eSignatureMousepadCanvas').toDataURL();

                if (base64 == null) {
                    utility.DisplayMessages("Please Upload File First", 3);
                    return false;
                }
            }
            else if (Admin_Provider_eSignature.SignatureType == "Mouse") {
                base64 = Admin_Provider_eSignature.signaturePad.toDataURL('image/png');
                if (base64 == null) {
                    utility.DisplayMessages("Please add signature using mouse.", 3);
                    return false;
                }
            }

            if (Admin_Provider_eSignature.params.ParentCtrl == "providerDetail") {
                providerDetail.setImageSource(base64);
            }

        }

        //$("#webcam").scriptcam({
        //    disconnected: true,
        //});
        Admin_FaxSettingsDetail.eSignatureimg = base64;
    },

    setSavedImage_on_eSignatureForm: function () {

        var savedImg_Src = $('#frmProviderDetail  #img_eSignature_Admin_Provider_Detail').attr('src');

        var IsImageExists = !(savedImg_Src === "" || savedImg_Src === undefined || savedImg_Src === null || savedImg_Src === "null");

        if (IsImageExists) {
        }
    },

    //-----------------------------------------------------------------------------------------------------------------------
    //---------------------------       eSignature Device Integration Work Started       ------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------

    print: function (txt) {
        utility.DisplayMessages(txt, 2);
    },

    OnLoad: function (callback) {
        // Admin_Provider_eSignature.print("CLEAR");
        Admin_Provider_eSignature.restartSession(callback);
    },

    restartSession: function (callback) {

        Admin_Provider_eSignature.wgssSignatureSDK = null;
        Admin_Provider_eSignature.sigObj = null;
        Admin_Provider_eSignature.sigCtl = null;
        Admin_Provider_eSignature.dynCapt = null;
        var imageBox = document.getElementById("imageBox");

        if (null != imageBox.firstChild) {
            imageBox.removeChild(imageBox.firstChild);
        }

        var timeout = setTimeout(timedDetect, 1500);
        // pass the starting service port  number as configured in the registry
        Admin_Provider_eSignature.wgssSignatureSDK = new Signaturesdk.WacomGSS_SignatureSDK(onDetectRunning, 8000);

        function timedDetect() {
            if (Admin_Provider_eSignature.wgssSignatureSDK.running) {
                start();
            }
            else {
                //    Admin_Provider_eSignature.print("Signature SDK Service not detected.");
            }
        }

        function onDetectRunning() {
            if (Admin_Provider_eSignature.wgssSignatureSDK.running) {
                clearTimeout(timeout);
                start();
            }
            else {
                //   Admin_Provider_eSignature.print("Signature SDK Service not detected.");
            }
        }

        function start() {
            if (Admin_Provider_eSignature.wgssSignatureSDK.running) {
                Admin_Provider_eSignature.sigCtl = new Admin_Provider_eSignature.wgssSignatureSDK.SigCtl(onSigCtlConstructor);
            }
        }

        function onSigCtlConstructor(sigCtlV, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                Admin_Provider_eSignature.dynCapt = new Admin_Provider_eSignature.wgssSignatureSDK.DynamicCapture(onDynCaptConstructor);
            }
            else {
                Admin_Provider_eSignature.print("SigCtl constructor error: " + status);
            }
        }

        function onDynCaptConstructor(dynCaptV, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                Admin_Provider_eSignature.sigCtl.GetSignature(onGetSignature);
            }
            else {
                Admin_Provider_eSignature.print("DynCapt constructor error: " + status);
            }
        }

        function onGetSignature(sigCtlV, sigObjV, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                Admin_Provider_eSignature.sigObj = sigObjV;
                Admin_Provider_eSignature.sigCtl.GetProperty("Component_FileVersion", onSigCtlGetProperty);
            }
            else {
                Admin_Provider_eSignature.print("SigCapt GetSignature error: " + status);
            }
        }

        function onSigCtlGetProperty(sigCtlV, property, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                //Admin_Provider_eSignature.print("DLL: flSigCOM.dll  v" + property.text);
                Admin_Provider_eSignature.dynCapt.GetProperty("Component_FileVersion", onDynCaptGetProperty);
            }
            else {
                Admin_Provider_eSignature.print("SigCtl GetProperty error: " + status);
            }
        }

        function onDynCaptGetProperty(dynCaptV, property, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                //Admin_Provider_eSignature.print("DLL: flSigCapt.dll v" + property.text);
                //Admin_Provider_eSignature.print("Test application ready.");
                //Admin_Provider_eSignature.print("Press 'Start' to capture a signature.");
                if ('function' === typeof callback) {
                    callback();
                }
            }
            else {
                Admin_Provider_eSignature.print("DynCapt GetProperty error: " + status);
            }
        }
    },

    Capture: function () {
        if (!Admin_Provider_eSignature.wgssSignatureSDK.running || null == Admin_Provider_eSignature.dynCapt) {
            //Admin_Provider_eSignature.print("Session error. Restarting the session.");
            Admin_Provider_eSignature.print("Device not Detected!");
            Admin_Provider_eSignature.restartSession(Admin_Provider_eSignature.Capture);
            return;
        }

        Admin_Provider_eSignature.dynCapt.Capture(Admin_Provider_eSignature.sigCtl, "who", "why", null, null, onDynCaptCapture);

        function onDynCaptCapture(dynCaptV, SigObjV, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.INVALID_SESSION == status) {
                //Admin_Provider_eSignature.print("Error: invalid session. Restarting the session.");
                Admin_Provider_eSignature.restartSession(Admin_Provider_eSignature.Capture);
            }
            else {
                if (Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptOK != status) {
                    // Admin_Provider_eSignature.print("Capture returned: " + status);
                }
                switch (status) {
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptOK:
                        Admin_Provider_eSignature.sigObj = SigObjV;
                        //Admin_Provider_eSignature.print("Signature captured successfully");
                        utility.DisplayMessages("Signature captured successfully", 1);
                        Admin_Provider_eSignature.SignatureType = "Picture";
                        var flags = Admin_Provider_eSignature.wgssSignatureSDK.RBFlags.RenderOutputBase64 |
                                    Admin_Provider_eSignature.wgssSignatureSDK.RBFlags.RenderColor24BPP;
                        var imageBox = document.getElementById("imageBox");
                        //   Admin_Provider_eSignature.sigObj.RenderBitmap("bmp", imageBox.clientWidth, imageBox.clientHeight, 0.7, 0x00000000, 0x00FFFFFF, flags, 0, 0, onRenderBitmap);
                        Admin_Provider_eSignature.sigObj.RenderBitmap("bmp", "315px", "125px", 0.7, 0x00000000, 0x00FFFFFF, flags, 0, 0, onRenderBitmap);
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptCancel:
                        Admin_Provider_eSignature.print("Signature capture cancelled");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptPadError:
                        Admin_Provider_eSignature.print("No capture service available");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptError:
                        Admin_Provider_eSignature.print("Tablet Error");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptIntegrityKeyInvalid:
                        Admin_Provider_eSignature.print("The integrity key parameter is invalid (obsolete)");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptNotLicensed:
                        Admin_Provider_eSignature.print("No valid Signature Capture licence found");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptAbort:
                        Admin_Provider_eSignature.print("Error - unable to parse document contents");
                        break;
                    default:
                        Admin_Provider_eSignature.print("Capture Error " + status);
                        break;
                }
            }
        }

        function onRenderBitmap(sigObjV, bmpObj, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {

                Admin_Provider_eSignature.drawImagetoCanvas(bmpObj.image.src);
            }
            else {
                Admin_Provider_eSignature.print("Signature Render Bitmap error: " + status);
            }
        }

    },

    drawImagetoCanvas: function (source) {

        var ctx = document.getElementById('eSignatureMousepadCanvas').getContext('2d');
        var img = new Image;

        img.onload = function () {
            ctx.drawImage(img, 0, 0, 315, 125); // Or at whatever offset you like
        };
        img.width = 315;
        img.height = 125;
        img.src = source;

        ctx.height = 125;
        ctx.width = 315;
    },

    ShowHistory: function () {
        var PanelID = 'pnlAdminFaxSettingsDetail';
        var ParentCtrl = 'Admin_FaxSettingsDetail';
        var ProfileName = 'Electronic Fax Setup';
        var DBTableName = 'ProviderFaxSettings';
        var ColumnKeyId = Admin_FaxSettingsDetail.params.FaxSettingId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

    ShowHistoryFacility: function () {
        var PanelID = 'pnlAdminFaxSettingsDetail';
        var ParentCtrl = 'Admin_FaxSettingsDetail';
        var ProfileName = 'Electronic Fax Setup Facility';
        var DBTableName = 'FacilityFaxSettings';
        var ColumnKeyId = Admin_FaxSettingsDetail.params.FaxSettingId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}
