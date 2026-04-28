providerDetail = {
    params: [],
    CPTData: [],
    imgwithoutwatermark: "",
    ispicnotchange: false,
    Load: function (params) {
        BackgroundLoaderShow(true);
        providerDetail.params = params;
        providerDetail.CPTData = [];
        var self = $('#tblproviderDetail');
        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            $("#" + providerDetail.params["PanelID"] + " #tblproviderDetail #ddlEntity").attr('disabled', 'disabled');
        }

        // Start initializing color picker for New/Established Patient Type
        $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divNewPatColor").colorpicker();
        $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divEstPatColor").colorpicker();
        $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divNewPatColor").colorpicker('setValue', '#08c');
        $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divEstPatColor").colorpicker('setValue', '#08c');
        $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divNewPatColor #txtNewPatColor").val('#0088cc');
        $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divEstPatColor #txtEstPatColor").val('#0088cc');
        //End initializing color picker for New/Established Patient Type

        self.loadDropDowns(true).done(function () {
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#providerDetail #divProvider_Entity").css("display", "none");
            //    $("#providerDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            var SelectedEntityId = "";
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                $("#" + providerDetail.params["PanelID"] + " #tblproviderDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                SelectedEntityId = globalAppdata["SeletedEntityId"];
                //PMS#379
                //setTimeout(function () {
                //    providerDetail.LoadSpecialty(globalAppdata["SeletedEntityId"]);
                //}, 100);
                //end PMS#379
            }
            else {

                if (providerDetail.params["mode"] == "Edit") {
                    SelectedEntityId = providerDetail.params["EntityId"];
                }
                else {
                    SelectedEntityId = globalAppdata["SeletedEntityId"];
                }
            }
            $("#providerDetail #tblproviderDetail #txtWebSite").change(function () {
                if ((this.value).match(/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/)) {
                    if (this.value != '' && !/^(http|https):\/\//.test(this.value)) {
                        this.value = "http://" + this.value;
                    }
                }
            });
            $.when(providerDetail.LoadSpecialty(SelectedEntityId)).done(function () {
                providerDetail.LoadProvider();
            });
            $('#' + providerDetail.params.PanelID + ' #divProviderFacility #ddlProviderDiagnosticImagingFacility').find('option:contains(' + "- Select -" + ')').remove();
            $('#' + providerDetail.params.PanelID + ' #divBulkSignException #ddlBulkSignException').find('option:contains(' + "- Select -" + ')').remove();
            $('#' + providerDetail.params.PanelID + ' #divProviderFacility .multiselect-container').find("li").eq(1).remove();
            $('#' + providerDetail.params.PanelID + ' #divBulkSignException .multiselect-container').find("li").eq(1).remove()
            providerDetail.IntializeMultiSelectDropDownFacility();
            providerDetail.IntializeMultiSelectDropDownBulkSignException();
            $('#frmProviderDetail').data('serialize', $('#frmProviderDetail').serialize());
        });

        // Set Title Explicitly if it's passed as Parameter
        if (providerDetail.params.Title != null)
            $("#" + providerDetail.params["PanelID"] + " #headingTitle").text(providerDetail.params.Title);


        // Enable Scrub Claim check box for MDVision User only
        if (globalAppdata["AppUserName"].toLowerCase() == "mdvision")
            $("#div_ScrubClaim").removeClass("hidden");


        providerDetail.loadReportHeaderddl();
        providerDetail.FillNPIProviderData();
    },
    IntializeMultiSelectDropDownFacility: function () {
        $('#' + providerDetail.params.PanelID + ' #divProviderFacility #ddlProviderDiagnosticImagingFacility').multiselect('destroy');
        $('#' + providerDetail.params.PanelID + ' #divProviderFacility #ddlProviderDiagnosticImagingFacility').multiselect({
            includeSelectAllOption: false,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            onChange: function (element, checked) {
            }
        });
        $('#' + providerDetail.params.PanelID + " #ddlProviderDiagnosticImagingFacility").val("");
    },
    IntializeMultiSelectDropDownBulkSignException: function () {
        $('#' + providerDetail.params.PanelID + ' #divBulkSignException #ddlBulkSignException').multiselect('destroy');
        $('#' + providerDetail.params.PanelID + ' #divBulkSignException #ddlBulkSignException').multiselect({
            includeSelectAllOption: false,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            onChange: function (element, checked) {
            }
        });
        $('#' + providerDetail.params.PanelID + " #ddlBulkSignException").val("");
    },
    //CallCityState: function (control, field1, field2) {
    //    var zipcode = $('#providerDetail ' + control).val();
    //    var cityname = null;
    //    var statename = null;
    //    providerDetail.FillCityState(zipcode, cityname, statename).done(function (response) {
    //        if (response.status != false) {
    //            var citystate = JSON.parse(response.CITYSTATE_JSON);
    //            $('#providerDetail ' + field1).val(citystate.txtCity);
    //            $('#providerDetail ' + field2).val(citystate.txtState);
    //            //var self = $("#providerDetail");
    //            ////self.bindMyJSON(true, citystate, true);
    //            //utility.bindMyJSON(true, citystate, true, self);
    //            //providerDetail.ValidateProvider();
    //        }
    //        else {
    //            $('#providerDetail ' + field1).val('');
    //            $('#providerDetail ' + field2).val('');
    //        }
    //    });
    //},

    LoadProvider: function () {
        $("#providerDetail #pnlLicenseDetail").removeClass('disableAll');
        if (providerDetail.params.mode == "Add") {
            $('#providerDetail #txtShortName').attr("enabled", "enabled");

            $("#providerDetail #pnlLicenseDetail").addClass('disableAll');
            providerDetail.ValidateProvider();

            //serialize Data after all controls loaded.
            $('#frmProviderDetail').data('serialize', $('#frmProviderDetail').serialize());

        } else if (providerDetail.params.mode == "Edit") {
            $('#providerDetail #txtShortName').attr("disabled", "disabled");
            providerDetail.LoadProviderLicense().done(function (response) {
                if (response.status != false) {

                    providerDetail.ProviderLicenseGridLoad(response);
                    $('#frmProviderDetail').data('serialize', $('#frmProviderDetail').serialize());
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

            //providerDetail.LoadEntityBasedData($("#providerDetail #ddlEntity").val());
            var IsWaterMarkApplieded = 1;
            providerDetail.FillProvider(providerDetail.params.ProviderId, IsWaterMarkApplieded).done(function (response) {
                if (response.status != false) {
                    providerDetail.LoadProviderData(response);
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }
    },
    LoadProviderData: function (response)
    {
        var provider_detail = JSON.parse(response.ProviderFill_JSON);
        var providerCPTsList_JSON = JSON.parse(response.ProviderCPTs_JSON);
        var self = $("#providerDetail");
        utility.bindMyJSON(true, provider_detail, false, self).done(function () {
            var dfd = jQuery.Deferred();

            // PMS-4767 Start initializing color picker for New/Established Patient Type
            $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divNewPatColor").colorpicker('setValue', provider_detail.txtNewPatColor != "" ? provider_detail.txtNewPatColor : '#0088cc');
            $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divEstPatColor").colorpicker('setValue', provider_detail.txtEstPatColor != "" ? provider_detail.txtEstPatColor : '#0088cc');
            if (provider_detail.txtNewPatColor == "")
                $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divNewPatColor #txtNewPatColor").val('#0088cc');
            if (provider_detail.txtEstPatColor == "")
                $("#" + providerDetail.params["PanelID"] + "  #divPatientTypeColor #divEstPatColor #txtEstPatColor").val('#0088cc');
            // PMS-4767 End initializing color picker for New/Established Patient Type

            $.each(providerCPTsList_JSON, function (i, item) {
                var li = "<li procedureListId = " + item.CPTId + "  id=" + item.CPTId + " cptCode=\"" + item.CPTCode + "\" cptDesc=\"" + item.CPTCodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMED_Description + "\"><a href='#'>" + item.CPTCode + " - " + item.CPTCodeDescription + "<span class='removeIconListHover' onclick='providerDetail.deleteCPTFromCPTData(this,\"" + item.CPTCode + "\",\"" + item.CPTCodeDescription + "\",\"" + item.SNOMEDID + "\",\"" + item.SNOMED_Description + "\");'><i class='fa fa-close'></i></span></a></li>";
                $('#' + providerDetail.params.PanelID + ' #ulProceduresList').addClass('panel-body p-none height-max150 height150 overflowY own-scroll');
                $('#' + providerDetail.params.PanelID + ' #ulProceduresList').append(li);
                var objData = {};
                objData["CPTCode"] = item.CPTCode;
                objData["CPTCodeDescription"] = item.CPTCodeDescription;
                objData["SNOMED_Description"] = item.SNOMED_Description;
                objData["SNOMEDID"] = item.SNOMEDID;
                providerDetail.CPTData.push(objData);
            });
            if (provider_detail.chkActive == 'True')
                $("#providerDetail #chkActive").attr("checked", true);
            else
                $("#providerDetail #chkActive").attr("checked", false);

            if (provider_detail.chkSpecialist == 'True')
                $("#providerDetail #chkSpecialist").attr("checked", true);
            else
                $("#providerDetail #chkSpecialist").attr("checked", false);


            //start  ||  added by Talha Tanweer 22 july 2016
            if (provider_detail.chkIs_eSignatured == 'True') {
                $("#providerDetail #chkeSignature").attr("checked", true);
            } else {
                $("#providerDetail #chkeSignature").attr("checked", false);
            }

            if (provider_detail.imgeSignature != "") {
                $("#frmProviderDetail #eSignatureImageWrapperID").removeClass("hidden");
                providerDetail.imgwithoutwatermark = provider_detail.OrignalimgeSignature;
                providerDetail.ispicnotchange = true;
                var isBrowserIE = providerDetail.GetIEVersion() > 0;
                if (!isBrowserIE) {
                    $('#frmProviderDetail  #img_eSignature_Admin_Provider_Detail').attr('src', provider_detail.imgeSignature);
                } else {
                    var srcForIE = provider_detail.imgeSignature.replace("System.Byte[]", "image/gif");
                    $('#frmProviderDetail  #img_eSignature_Admin_Provider_Detail').attr('src', srcForIE);
                }

            }

            {
                //var imageSrc = $('#frmProviderDetail  #img_eSignature_Admin_Provider_Detail').attr('src');
                //if (imageSrc == 'Content/images/default_male_profile.gif' || imageSrc == 'Content/images/default_female_profile.gif') {
                //    $('#' + providerDetail.params.PanelID + " #btnRemoveUploadedImage").hide();
                //}
                //else {
                //    $('#' + providerDetail.params.PanelID + " #btnRemoveUploadedImage").show();
                //}
                //$('#frmProviderDetail').data('serialize', $('#frmProviderDetail').serialize());
                ////Patient_Demographic.FillPatientInfo(Patient_Demographic.params.patientID);
                ////Patient_Demographic.params["DemographicId"] = Patient_Demographic.params.patientID;
            }


            //End    ||  added by Talha Tanweer 22 july 2016


            $("#providerDetail #pnlLicenseDetail").removeClass('disableAll');

            providerDetail.ValidateProvider();
            if (provider_detail.strDiagnosticImagingFacilities) {
                var arrstrDiagnosticImagingFacilityIds = provider_detail.strDiagnosticImagingFacilities.split(',');
                $('#' + providerDetail.params.PanelID + ' #divProviderFacility #ddlProviderDiagnosticImagingFacility').val(arrstrDiagnosticImagingFacilityIds);
                $('#' + providerDetail.params.PanelID + ' #divProviderFacility #ddlProviderDiagnosticImagingFacility').multiselect("refresh");
            }
            else {
                $('#' + providerDetail.params.PanelID + ' #divProviderFacility #ddlProviderDiagnosticImagingFacility').find("option:selected").removeAttr("selected");
                $('#' + providerDetail.params.PanelID + ' #divProviderFacility #ddlProviderDiagnosticImagingFacility').multiselect("refresh");
            }
            if (provider_detail.strBulkSignException) {
                var arrstrBulkSignExceptionIds = provider_detail.strBulkSignException.trim().split(',');
                $('#' + providerDetail.params.PanelID + ' #divBulkSignException #ddlBulkSignException').val(arrstrBulkSignExceptionIds);
                $('#' + providerDetail.params.PanelID + ' #divBulkSignException #ddlBulkSignException').multiselect("refresh");
            }
            else {
                $('#' + providerDetail.params.PanelID + ' #divBulkSignException #ddlBulkSignException').find("option:selected").removeAttr("selected");
                $('#' + providerDetail.params.PanelID + ' #divBulkSignException #ddlBulkSignException').multiselect("refresh");
            }
            dfd.resolve();
            dfd.then(function () {
                //serialize Data after all controls loaded.
                $('#frmProviderDetail').data('serialize', $('#frmProviderDetail').serialize());
                return dfd.promise();
            });
        });

    },
    LoadEntityBasedData: function (entityID) {

        providerDetail.LoadBasicFeeGroup(entityID).done(function () {

        });
        providerDetail.LoadSupervisingProvider(entityID).done(function () {

        });
        providerDetail.LoadSpecialty(entityID).done(function () {
            $('#frmProviderDetail').bootstrapValidator('revalidateField', $('#frmProviderDetail #ddlSpecialty').attr('name'));
        });
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#tblproviderDetail #ddlFeeGroup', 'GetFeeGroup', false, entityID);
        } else {
            CacheManager.BindDropDownsByEntityID('#tblproviderDetail #ddlFeeGroup', 'GetFeeGroup', true, null);
        }
    },

    LoadBasicFeeGroup: function (entityID) {
        // Loads Entity Based Basic Fee Group
        return providerDetail.FillBasicFeeGroup(entityID).done(function (response) {
            if (response.status != false) {
                var basicfeegroup_detail = JSON.parse(response.BasicFeeGroupLoad_JSON);
                $("#providerDetail #ddlBasicFeeGroup").empty();
                $("#providerDetail #ddlBasicFeeGroup").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(basicfeegroup_detail, function (i, item) {
                    $("#providerDetail #ddlBasicFeeGroup").append(
                        $('<option/>', {
                            value: item.BasicFeeGroupId,
                            html: item.EntityName + " - " + item.ShortName
                        })
                    );
                });
            }

        });
    },

    LoadSupervisingProvider: function (entityID) {
        // Loads Entity Based Supervising Provider
        return providerDetail.FillSupervisingProvider(entityID).done(function (response) {
            if (response.status != false) {
                var feegroup_detail = JSON.parse(response.SupervisingProviderLoad_JSON);
                $("#providerDetail #ddlSupervisingProvider").empty();
                $("#providerDetail #ddlSupervisingProvider").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(feegroup_detail, function (i, item) {
                    $("#providerDetail #ddlSupervisingProvider").append(
                        $('<option/>', {
                            value: item.ProviderId,
                            html: item.ShortName
                        })
                    );
                });
            }

        });
    },

    LoadSpecialty: function (entityID) {
        // Loads Entity Based Specialty
        return providerDetail.FillSpecialty(entityID).done(function (response) {
            if (response.status != false) {
                var feegroup_detail = JSON.parse(response.SpecialtyLoad_JSON);
                $("#providerDetail #ddlSpecialty").empty();
                $("#providerDetail #ddlSpecialty").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(feegroup_detail, function (i, item) {
                    $("#providerDetail #ddlSpecialty").append(
                        $('<option/>', {
                            value: item.SpecialtyId,
                            html: item.ShortName
                        })
                    );
                });
                $('#frmProviderDetail').data('serialize', $('#frmProviderDetail').serialize());
            }

        });
    },

    ValidateProvider: function () {
        $('#' + providerDetail.params["PanelID"] + '  #frmProviderDetail')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    shortname: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    lastname: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    firstname: {
                        group: '.size60per',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    sex: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    speciality: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    qualification: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    npi: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                            integer: {
                                message: ' '
                            }
                        }
                    },
                    profiletype: {
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    entity: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    providertype: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Address: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    city: {
                        group: '.size60per',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    state: {
                        group: '.size40per',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    zip: {
                        group: '.size60per',
                        validators: {
                            notEmpty: {
                                message: ''
                            },
                        }
                    },
                    'email': {
                        group: '.col-sm-2',
                        enabled: false,
                        validators: {
                            regexp: {
                                regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                                message: 'Email not Valid'
                            }

                        }
                    },
                    'Website': {
                        group: '.col-sm-2',
                        enabled: false,
                        validators: {
                            uri: {
                                message: 'Format not Valid'
                            }
                        }
                    },
                    'gender': {
                        feedbackIcons: false,
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    }
                }
            })
            .on('success.form.bv', function (e) {
                e.preventDefault();
                if ($('#frmProviderDetail  #img_eSignature_Admin_Provider_Detail').attr('src') === ""
                    && $("#" + providerDetail.params["PanelID"] + " #" + providerDetail.checkbox_eSignatureId).prop("checked") === true) {
                    utility.DisplayMessages("Please Upload Signature or uncheck eSignature!", 3);
                } else {
                    providerDetail.ProviderSave();
                }
            });
    },

    EnableValidation: function (obj) {

        var objprovider = $('#providerDetail #frmProviderDetail');
        var formValidation = objprovider.data("bootstrapValidator");
        if ($(obj).val() != "") {
            formValidation.enableFieldValidators($(obj).attr("name"), true);
        } else {
            formValidation.enableFieldValidators($(obj).attr("name"), false);
        }

    },

    ProviderSave: function () {
        $('#frmProviderDetail').data('serialize', $('#frmProviderDetail').serialize());
        var strMessage = "";
        var self = $("#providerDetail");
        var myJSON = self.getMyJSON();
        var objData = JSON.parse(myJSON);
        var FacilityIds = self.find('#divProviderFacility ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            return this.value;
        }).get().join(',');
        var BulkSignExceptionIds = self.find('#divBulkSignException ul.multiselect-container li input[type=checkbox]:checked').map(function () {
            return this.value;
        }).get().join(',');
        objData["ListProviderCPTs"] = providerDetail.CPTData;
        objData["strFacilityIds"] = FacilityIds;
        objData["strBulkSignExceptionIds"] = BulkSignExceptionIds;
        myJSON = JSON.stringify(objData);
        if (providerDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (providerDetail.params.ProviderId == "-1") {
                        providerDetail.SaveProvider(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#providerDetail #pnlLicenseDetail").removeClass('disableAll');
                                //  providerDetail.params.ProviderId = response.ProviderId;

                                //Editable Grid

                                var PanelGrid = "#" + providerDetail.params["PanelID"] + " #pnlLicenseDetail";
                                var GridId = "#" + providerDetail.params["PanelID"] + " #dgvStateLicense";
                                if (providerDetail.params["IsFromEncounter"] != 1) {
                                    utility.MakeEditableGrid(PanelGrid, GridId, providerDetail);
                                }
                                //    Admin_Provider.ProviderSearch(response.ProviderId);

                                utility.DisplayMessages(response.message, 1);
                                $('#frmProviderDetail').data('serialize', $('#frmProviderDetail').serialize());
                                CacheManager.BindCodes('GetProvider', true);
                                CacheManager.BindCodes('GetAllProviders', true);
                                var parentCTRL = providerDetail.params["ParentCtrl"] != null ? providerDetail.params["ParentCtrl"] : "Admin_Provider";

                                if (parentCTRL == "EncounterChargeCapture")
                                {
                                    EncounterChargeCapture.LoadProvider();
                                }
                                UnloadActionPan(parentCTRL, "providerDetail");
                                Admin_Provider.ProviderSearch();
                            } else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    } else if (providerDetail.params.ProviderId != "-1" && providerDetail.params.ProviderId != "" && providerDetail.params.ProviderId != "0") {
                        providerDetail.UpdateProvider(myJSON, providerDetail.params.ProviderId, 1).done(function (response) {
                            if (response.status != false) {
                                Admin_Provider.ProviderSearch(providerDetail.params.ProviderId);
                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetProvider', true);
                            } else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                } else
                    utility.DisplayMessages(strMessage, 2);
            });
        } else if (providerDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    providerDetail.UpdateProvider(myJSON, providerDetail.params.ProviderId, 1).done(function (response) {
                        if (response.status != false) {
                            var parentCTRL = providerDetail.params["ParentCtrl"] != null ? providerDetail.params["ParentCtrl"] : "Admin_Provider";
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetProvider', true);
                            CacheManager.BindCodes('GetAllProviders', true);
                            if (globalAppdata.DefaultProviderId == providerDetail.params.ProviderId) {
                                globalAppdata.IsProviderBulkSign = $('#providerDetail #frmProviderDetail #ddlBulkSign').val() == "1" ? "True" : "False";
                            }
                            if (providerDetail.params.RefCtrl != null && providerDetail.params.RefCtrl != "") {
                                var loadedEncounters = $(document).find('div[id*="pnlEncounterChargeCapture"]:last');
                                if (providerDetail.params.ParentCtrl && providerDetail.params.ParentCtrl.indexOf("EncounterChargeCapture") > -1 || loadedEncounters && EncounterChargeCapture.params && EncounterChargeCapture.params.PanelID) {
                                    var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
                                    var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
                                    EncounterChargeCapture.LoadProvider();
                                    EditableGrid.initialize(ChargeGridId, EncounterChargeCapture, "0", false, false, false, false, undefined, false, true);
                                }
                                
                                UnloadActionPan(parentCTRL, "providerDetail");

                            } else {
                                Admin_Provider.ProviderSearch(providerDetail.params.ProviderId);
                                if (providerDetail.params.ParentCtrl && providerDetail.params.ParentCtrl.indexOf("EncounterChargeCapture") > -1) {
                                    EncounterChargeCapture.LoadProvider();
                                }
                                UnloadActionPan(parentCTRL, "providerDetail");
                            }
                        } else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                } else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    // ------------ Provider License Region (Detail Grid)
    ProviderLicenseGridLoad: function (response) {

        if (response.ProviderLicenseCount > 0) {
            var ProviderLicenseJSON = JSON.parse(response.ProviderLicense_JSON);

            // get Actions
            var actions = "";
            $("#" + providerDetail.params["PanelID"] + " #dgvStateLicense tr th").each(function () {
                if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                    var arrActionType = [];
                    if ($(this).attr("ActionType") != null) {
                        arrActionType = $(this).attr("ActionType").split(',');
                        actions = EditableGrid.GetActions(arrActionType);
                    }
                }
            });

            $.each(ProviderLicenseJSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", item.ProviderLicenseId);

                $row.append('<td class="actions" id="' + item.ProviderLicenseId + '" >' + actions + '</td><td style="text-transform:uppercase">' + item.State + '</td><td>' + item.LicenseNo + '</td>');

                $("#" + providerDetail.params["PanelID"] + " #dgvStateLicense tbody").last().append($row);
            });
        }

        //Editable Grid
        var PanelGrid = "#" + providerDetail.params["PanelID"] + " #pnlLicenseDetail";
        var GridId = "#" + providerDetail.params["PanelID"] + " #dgvStateLicense";
        if (providerDetail.params["IsFromEncounter"] != 1) {
            utility.MakeEditableGrid(PanelGrid, GridId, providerDetail,false);
        }
    },

    SaveProviderLicense: function (ProviderLicenseData, RowId) {
        var data = "ProviderLicenseData=" + ProviderLicenseData + "&ProviderId=" + providerDetail.params.ProviderId + "&RowId=" + RowId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "SAVE_LICENSE_INFO");
    },

    UpdateProviderLicense: function (ProviderLicenseData, ProviderLicenseID) {

        var data = "ProviderLicenseData=" + ProviderLicenseData + "&ProviderLicenseID=" + ProviderLicenseID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "UPDATE_LICENSE_INFO");
    },

    //FillProviderLicense: function (ProviderLicenseID) {
    //    var data = "ProviderLicenseID=" + ProviderLicenseID;
    //    // serach parameter , class name, command name of class
    //    return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_LICENSE_INFO");
    //},

    FillBasicFeeGroup: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_BASIC_FEE_GROUP");
    },

    FillSupervisingProvider: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_SUPERVISING_PROVIDER");
    },

    FillSpecialty: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_SPECIALTY");
    },

    LoadProviderLicense: function () {
        var data = "ProviderId=" + providerDetail.params.ProviderId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "LOAD_LICENSE_INFO");
    },

    DeleteProviderLicense: function (ProviderLicenseId) {
        var data = "ProviderLicenseId=" + ProviderLicenseId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "DELETE_LICENSE_INFO");
    },

    UnLoadPlan: function () {

        if ($('#frmProviderLicense').serialize() != $('#frmProviderLicense').data('serialize')) {
            utility.myConfirm('2', function () {
                $('#ProviderLicenseDetailGrid').modal('hide');
            }, function () { },
                '2'
            );
        } else {
            $('#ProviderLicenseDetailGrid').modal('hide');
        }


    },
    //----------------------------------------------------------------

    SaveProvider: function (ProviderData) {
        var data = "ProviderData=" + ProviderData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "SAVE_PROVIDER");
    },

    UpdateProvider: function (ProviderData, ProviderID, IsActive) {
        if (providerDetail.ispicnotchange) {
            var modifiedjson = JSON.parse(ProviderData);
            modifiedjson.img_eSignature_Admin_Provider_Detail = providerDetail.imgwithoutwatermark;
            ProviderData = JSON.stringify(modifiedjson);
        }
        var data = "ProviderData=" + ProviderData + "&ProviderID=" + ProviderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "UPDATE_PROVIDER");
    },

    FillProvider: function (ProviderID, IsWaterMarkApplied, NIPNumber) {
        var IsWaterMark = 0;
        if (IsWaterMarkApplied === null || IsWaterMarkApplied === undefined || IsWaterMarkApplied === "null") {

        }
        else {
            IsWaterMark = IsWaterMarkApplied;
        }

        var data = "ProviderID=" + ProviderID + "&IsWaterMark=" + IsWaterMark + "&NIPNumber=" + NIPNumber;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "FILL_PROVIDER");
    },

    FillProviderApiData: function ( NIPNumber) {
        var data ="&NIPNumber=" + NIPNumber;
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "LOAD_PROVIDER_NPI_APIDATA");
    },

    GetMultipleProviderInfo_DBCall: function (ProviderIDs) {

        var data = "ProviderIDs=" + ProviderIDs;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "GET_MULTIPLE_PROVIDER_INFO");
    },


    UpdateProviderActiveInactive: function (ProviderID, IsActive) {
        var data = "ProviderID=" + ProviderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "UPDATE_PROVIDER_ACTIVE_INACTIVE");
    },

    //FillCityState: function (zipcode, cityname, statename) {
    //    var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
    //    // serach parameter , class name, command name of class
    //    return MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE");
    //},

    UnLoad: function () {
        if (EMRUtility.compareFormDataWithSerialized(providerDetail.params.PanelID + ' #frmProviderDetail') || $('#' + providerDetail.params.PanelID + ' #ulProceduresList').find('li[id*="-"]').length > 0) {
            utility.myConfirm('2', function () {
                UnloadActionPan(providerDetail.params["ParentCtrl"], "providerDetail");
            }, function () {
            },'2');
        }
        else
            UnloadActionPan(providerDetail.params["ParentCtrl"], "providerDetail");

        var loadedEncounters = $(document).find('div[id*="pnlEncounterChargeCapture"]:last');
        if (providerDetail.params.ParentCtrl && providerDetail.params.ParentCtrl.indexOf("EncounterChargeCapture") > -1 || loadedEncounters && typeof EncounterChargeCapture !="undefined" && EncounterChargeCapture.params && EncounterChargeCapture.params.PanelID) {
            var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
            var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
            EditableGrid.initialize(ChargeGridId, EncounterChargeCapture, "0", false, false, false, false, undefined, false, true);
        }
        providerDetail.imgwithoutwatermark = "";
        providerDetail.ispicnotchange = false;
    },

    //-------------------Editable Grid Methods Starts---

    rowSave: function ($row, obj) {

        if (obj.rowValidate($row)) {

            var _self = obj,
                $actions,
                values = [];

            if ($row.hasClass('adding')) {
                $row.removeClass('adding');
            }

            values = $row.find('td').map(function () {

                var $this = $(this);

                if ($this.hasClass('expand')) {
                    return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
                } else if ($this.hasClass('actions')) {

                    return _self.datatable.cell(this).data();
                } else if ($this.hasClass('ddl')) {
                    return $.trim($this.find('select').val());

                } else {
                    return $.trim($this.find('input').val());
                }
            });

            var id = $row.attr("id");
            var myJSON = $row.getMyJSON();


            if (id && id > 0) {
                //Edit Record
                var strMessage = "";
                AppPrivileges.GetFormPrivileges("Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        providerDetail.UpdateProviderLicense(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                utility.DisplayMessages(response.Message, 1);
                                providerDetail.rowDraw($row, _self, values);
                            } else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                });
            } else {
                //Add Record

                AppPrivileges.GetFormPrivileges("Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        providerDetail.SaveProviderLicense(myJSON, id).done(function (response) {
                            if (response.status != false) {

                                $row.attr("id", response.ProviderLicenseInfoId);
                                $row.attr("onclick", "utility.SelectGridRow($(this))");
                                utility.DisplayMessages(response.Message, 1);
                                providerDetail.rowDraw($row, _self, values);
                            } else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    } else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
        }
    },

    rowAdd: function () {

        AppPrivileges.GetFormPrivileges("Provider", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
            } else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {

        var id = $row.attr("id");
        AppPrivileges.GetFormPrivileges("Provider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = id;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    } else {
                        providerDetail.DeleteProviderLicense(selectedValue).done(function (response) {
                            if (response.status != false) {

                                if ($row.hasClass('adding')) {
                                }
                                var _self = obj;
                                _self.datatable.row($row.get(0)).remove().draw();

                                //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                    utility.removePaginationFromGrid($('#' + providerDetail.params.PanelID + ' #pnlLicenseDetail'));
                                //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
                                utility.DisplayMessages(response.Message, 1);
                            } else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            } else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    rowCancel: function ($row, obj) {


        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },
    validateLength: function (ev) {
        var maxLength = $(ev).attr('maxlength');
        maxLength = maxLength == null ? 10 : maxLength;
        if ($(ev).val().length != maxLength) {
            $(ev).val('');
        }

    },
    //-------------------Editable Grid Methods Ends---

    //-----------------------------------------------------------------------------------------------------------------------
    //--------------------------------------      eSignature Work Started      ----------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------

    checkbox_eSignatureId: "chkeSignature",

    //Author: Talha Tanweer
    //Date  : 22/07/2016
    change_eSignatured: function () {

        var isChecked = $("#" + providerDetail.params["PanelID"] + " #" + providerDetail.checkbox_eSignatureId).prop("checked");

        if (isChecked === true) {

            var params = [];
            params["ParentCtrl"] = 'providerDetail';
            var controlId = "Admin_Provider_eSignature";
            LoadActionPan(controlId, params);
        }
    },

    //Author: Talha Tanweer
    //Date  : 22/07/2016
    setImageSource: function (sourceString) {
        if (sourceString !== "") {
            $("#frmProviderDetail #eSignatureImageWrapperID").removeClass("hidden");
            $('#frmProviderDetail  #img_eSignature_Admin_Provider_Detail').attr('src', sourceString);
            providerDetail.ispicnotchange = false;
        } else {

        }
    },



    //Author: Talha Tanweer
    //Date  : 22/07/2016
    RemoveUploadedImage: function () {
        utility.myConfirm('Are you sure to delete the signature?', function () {
            $('#' + providerDetail.params.PanelID + " #frmProviderDetail #img_eSignature_Admin_Provider_Detail").attr("src", "");
            $("#frmProviderDetail #eSignatureImageWrapperID").addClass("hidden");
            $("#providerDetail #frmProviderDetail #chkeSignature").attr("checked", false);
            //var self = $('#' + providerDetail.params["PanelID"]);
            //var myJSON = self.getMyJSONByName();
            var self = $('#' + providerDetail.params["PanelID"] + " #frmProviderDetail");
            var myJSON = self.getMyJSON();
            providerDetail.ispicnotchange = false;

            providerDetail.UpdateProvider(myJSON, providerDetail.params.ProviderId, 1).done(function (response) {
                //---------- Remove Bug EMR-609-------------------   providerDetail.LoadProvider();

                utility.DisplayMessages("Successfully Deleted", 1);
            });
        }
          , function () {
              //NO CALLBACK
          },
   'Confirm Delete'

   //
);

    },


    //Author: Talha Tanweer
    //Date  : 22/07/2016
    UpdateDemographic: function (DemographicData, PatientID, strRaceIds) {

        var objData = JSON.parse(DemographicData);
        objData["PatientID"] = PatientID;
        objData["CommandType"] = "update_patient_demographic";
        objData["imgPatient"] = $('#pnlDemographic #imgPatient').attr('src');
        objData["strRaceIds"] = strRaceIds;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },



    //Author: Talha Tanweer
    //Date  : 03/August/2016

    GetIEVersion: function () {
        var sAgent = window.navigator.userAgent;
        var Idx = sAgent.indexOf("MSIE");

        // If IE, return version number.
        if (Idx > 0)
            return parseInt(sAgent.substring(Idx + 5, sAgent.indexOf(".", Idx)));

            // If IE 11 then look for Updated user agent string.
        else if (!!navigator.userAgent.match(/Trident\/7\./))
            return 11;

        else if (navigator.userAgent.match(/Edge\/\d./i)) {
            return 12;//12 for edge
        }
        else
            return 0; //It is not IE
    },



    //-----------------------------------------------------------------------------------------------------------------------
    //--------------------------------------       eSignature Work Ended       ----------------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------

    loadReportHeaderddl: function () {

        var data = "ID=" + providerDetail.params.ProviderId;

        MDVisionService.lookups('GetReportHeaders', true, data).done(function (result) {
            var options = JSON.parse(result['GetReportHeaders']);
            var $headerddl = $('#' + providerDetail.params.PanelID + ' #ddlHeaderTemplate');

            $headerddl.empty();
            $headerddl.append(
                      $('<option/>', {
                          value: "",
                          html: "Blank",
                      })
                  );

            $.each(options, function (i, item) {
                if (item.Value != "" && typeof item.Value != 'undefined') {
                    $headerddl.append(
                        $('<option/>', {
                            value: item.Value,
                            html: item.Name,
                        })
                    );
                }
            });
            $('#frmProviderDetail').data('serialize', $('#frmProviderDetail').serialize());
        });
    },

    ShowHistory: function () {
        var PanelID = 'providerDetail';
        var ParentCtrl = 'providerDetail';
        var ProfileName = 'Provider';
        var DBTableName = 'Provider';
        var ColumnKeyId = providerDetail.params.ProviderId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
        //    .done(function (response) {
        //});

    },
    /*********** Procedures Begin ***********/
    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "providerDetail";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtProcedures";
        params["ParentCtrlPanelID"] = providerDetail.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, providerDetail.params.PanelID);
    },

    bindAutoComplete: function (element) {
        var hiddenCrtl = $('#' + providerDetail.params.PanelID + ' #txtProcedures');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "providerDetail", null, true);

    },

    deleteCPTFromCPTData: function (obj, cptCode, cptDescription, snomedCode, snomedDescription) {
        cptDescription = typeof cptDescription == "undefined" ? "" : cptDescription.toString();
        $.each(providerDetail.CPTData, function (index, item) {
            if (item["CPTCode"] == cptCode.toString() && item["CPTCodeDescription"] == cptDescription) {
                providerDetail.CPTData.splice(index, 1);
                if ($(obj).parent().parent().attr('procedureListId') > -1) {
                    providerDetail.deleteCPT_DB_Call($(obj).parent().parent().attr('procedureListId')).done(function (response) {
                        if(response.status != false)
                            utility.DisplayMessages(response.Message, 1);
                        else
                            utility.DisplayMessages(response.Message, 3);
                    });
                }
                if ($('#' + providerDetail.params.PanelID + ' #ulProceduresList li').length == 1) {
                    $('#' + providerDetail.params.PanelID + ' #ulProceduresList').removeClass('panel-body p-none height-max150 height150 overflowY own-scroll');
                }
                $(obj).parent().parent().remove();
                return false;
            }
        });
    },

    deleteCPT_DB_Call: function (procedureListId) {
        var data = "ProcedureListId=" + procedureListId;
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER_DETAIL", "DELETE_PROCEDURELIST_CPT");
    },
    /*********** Procedures End ***********/


    //Fill provider data against NPI number by calling NPI API
    FillNPIProviderData: function () {
        var ctrl = $("#" + providerDetail.params.PanelID + ' #frmProviderDetail #txtNPI');
        ctrl.focusout(function () {
            var inputData = ctrl.val();
            if (!(inputData == "" || inputData == "__________" || ctrl.data("ProviderData") == inputData || inputData.replace(/[^0-9]/g, "").length != 10)) {


                providerDetail.FillProvider(0, 0, inputData).done(function (response) {
                    if (response.status != false) {
                        providerDetail.LoadProviderData(response);
                    }
                    else {
                        providerDetail.FillProviderApiData(inputData).done(function (response) {
                            providerDetail.PrivderNPIsuccess(response);
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
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtFirstName').val("");
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtLastName').val("");
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtTaxonomyCode').val("");
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtOfficeAddress').val("");
          //$("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtAddress2').val("");
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtTelephone').val("");
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtCity').val("");
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtState').val("");
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtZip').val("");
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtZipExt').val("");
            $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtFax').val("");
            if (data.result_count > 0 && data.results[0].enumeration_type == "NPI-1") {
                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtFirstName').val(data.results[0].basic.first_name);
                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtLastName').val(data.results[0].basic.last_name);
                if (data.results[0].basic.gender == 'M')
                    $("#" + providerDetail.params.PanelID + ' #frmProviderDetail #ddlSex').val(0);
                else if (data.results[0].basic.gender == 'F')
                    $("#" + providerDetail.params.PanelID + ' #frmProviderDetail #ddlSex').val(1);
                else
                    $("#" + providerDetail.params.PanelID + ' #frmProviderDetail #ddlSex').val(2);

                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtTaxonomyCode').val(data.results[0].taxonomies[0].code);
                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtOfficeAddress').val(data.results[0].addresses[1].address_1 + ' ' + data.results[0].addresses[1].address_2);
                // $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtAddress2').val(data.results[0].addresses[1].address_2);
                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtTelephone').val(data.results[0].addresses[1].telephone_number);
                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtCity').val(data.results[0].addresses[1].city);
                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtState').val(data.results[0].addresses[1].state);
                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtZip').val(data.results[0].addresses[1].postal_code.substring(0, 5));
                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtZipExt').val(data.results[0].addresses[1].postal_code.substring(5, 9));
                $("#" + providerDetail.params.PanelID + ' #frmProviderDetail  #txtFax').val(data.results[0].addresses[1].fax_number);
            } else if (data.result_count > 0 && data.results[0].enumeration_type != "NPI-1") {
                utility.DisplayMessages("Provided NPI is an Organization NPI.", 2);
            } else if (data.result_count == 0) {
                utility.DisplayMessages("Record not found for Provided NPI", 2);
            }
        } catch (ex) {
            console.log(ex);
            BackgroundLoaderShow(false);
            utility.DisplayMessages("Record not found for given NPI", 2);
        }
    },
}
