facilityDetail = {
    params: [],
    Locations: [],
    Load: function (params) {
        facilityDetail.params = params;

        //initialization of date-pickers.
        utility.CreateDatePicker('facilityDetail #tblfacilityDetail #dtpStartDate', function () {
            //on-change callback method 
        });

        // Start initializing color picker
        $('#facilityDetail #divFacilityColor').colorpicker();

        $('#facilityDetail #divFacilityColor').colorpicker().on('changeColor.colorpicker', function (event) {
            //$('#frmFacilityDetail').bootstrapValidator('revalidateField', 'Color');
            //$('#frmFacilityDetail').bootstrapValidator('updateStatus', 'Color', 'NOT_VALIDATED');
        });
        //End initializing color picker

        var self = $('#facilityDetail #tblfacilityDetail');
        self.loadDropDowns(true).done(function () {
            facilityDetail.saveAllLocations();
            //serialize Data.
            $('#frmFacilityDetail').data('serialize', $('#frmFacilityDetail').serialize());
            facilityDetail.ValidateFacility();
            facilityDetail.LoadFacility();
            $("#facilityDetail #tblfacilityDetail #txtWebSite").change(function () {
                if ((this.value).match(/[a-zA-Z0-9-\.]+\.[a-z]{2,4}/)) {
                    if (this.value != '' && !/^(http|https):\/\//.test(this.value)) {
                        this.value = "http://" + this.value;
                    }
                }
            });
        });

    },
    saveAllLocations: function () {

        facilityDetail.Locations = [];
        facilityDetail.Locations = $('#frmFacilityDetail #ddlLocation').children('option').map(function () {
            return {
                "Id": this.value,
                "Location": this.text,
                "facilityTypeId": $(this).attr("refvalue")

            }
        });
        $('#frmFacilityDetail #ddlLocation').empty();
    },
    getLocationByFacilityType: function (facilityTypeId) {
        var locations = $.grep(facilityDetail.Locations, function (v) {
            return v.facilityTypeId == facilityTypeId;
        });
        return locations;
    },
    facilityTypeChange: function () {
        var locationCtrl = $('#frmFacilityDetail #ddlLocation');

        var facilityTypeId = $("#frmFacilityDetail #ddlFacilityType").val()

        locationCtrl.empty().append($('<option />', { value: "", html: "- Select -", refvalue: "" }));
        $.each(facilityDetail.getLocationByFacilityType(facilityTypeId), function (i, item) {
            locationCtrl.append($('<option />', { value: item.Id, html: item.Location, refvalue: item.facilityTypeId }));
        });
    },

    LoadFacility: function () {
        if (facilityDetail.params.mode == "Add") {
            $('#facilityDetail #txtShortName').attr("enabled", "enabled");
        }
        else if (facilityDetail.params.mode == "Edit") {
            $('#facilityDetail #txtShortName').attr("disabled", "disabled");
            facilityDetail.FillFacility(facilityDetail.params.FacilityId).done(function (response) {
                if (response.status != false) {

                    var facility_detail = JSON.parse(response.FacilityFill_JSON);
                    var self = $("#facilityDetail");

                    utility.bindMyJSON(true, facility_detail, false, self).done(function () {

                        if (facility_detail.chkBillToPractice == 'True')
                            $("#facilityDetail #chkBillToPractice").attr("checked", true);
                        else
                            $("#facilityDetail #chkBillToPractice").attr("checked", false);

                        $('#facilityDetail #divFacilityColor').colorpicker('setValue', facility_detail.txtColor);

                        $("#facilityDetail #ddlFacilityType").trigger("change");

                        $("#facilityDetail #ddlLocation").val(facility_detail.ddlLocation);

                        //serialize Data.
                        $('#frmFacilityDetail').data('serialize', $('#frmFacilityDetail').serialize());

                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //CallCityState: function (control, field1, field2) {
    //    var zipcode = $('#facilityDetail ' + control).val();
    //    var cityname = null;
    //    var statename = null;
    //    facilityDetail.FillCityState(zipcode, cityname, statename).done(function (response) {
    //        if (response.status != false) {
    //            var citystate = JSON.parse(response.CITYSTATE_JSON);
    //            $('#facilityDetail ' + field1).val(citystate.txtCity);
    //            $('#facilityDetail ' + field2).val(citystate.txtState);
    //            //var self = $("#facilityDetail");
    //            ////self.bindMyJSON(true, citystate, true);
    //            //utility.bindMyJSON(true, citystate, true, self);
    //            //facilityDetail.ValidateFacility();
    //        }
    //        else {
    //            $('#facilityDetail ' + field1).val('');
    //            $('#facilityDetail ' + field2).val('');
    //        }
    //    });
    //},

    ValidateFacility: function () {
        $('#frmFacilityDetail')
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
                        group: '.col-sm-2',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Description: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Practice: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    POS: {
                        group: '.col-sm-2',
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
                            }
                        }
                    },
                    Address: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    City: {
                        group: '.size65per',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },

                    State: {
                        group: '.size35per',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },

                    Zip: {
                        group: '.size65per',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    Email: {
                        group: '.col-sm-5',
                        enabled: false,
                        validators: {
                            regexp: {
                                regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                                message: 'Email not Valid'
                            }
                        }
                    },
                    WebSite: {
                        group: '.col-sm-3',
                        enabled: false,
                        validators: {
                            uri: {
                                message: 'Format not Valid'
                            }
                        }
                    },
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            facilityDetail.FacilitySave();
        });
    },

    EnableValidation: function(obj) {

        var objfacility = $('#' + facilityDetail.params.PanelID +' #frmFacilityDetail');
        var formValidation = objfacility.data("bootstrapValidator");
        if ($(obj).val() != "")
        {
            formValidation.enableFieldValidators($(obj).attr("name"), true);
        }
        else {
            formValidation.enableFieldValidators($(obj).attr("name"), false);
        }

    },


    FacilitySave: function () {
        var strMessage = "";
        var self = $("#facilityDetail");
        var myJSON = self.getMyJSON();
        if (facilityDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Facility", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    facilityDetail.SaveFacility(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_Facility.FacilitySearch(response.FacilityId);
                            utility.DisplayMessages(response.Message, 1);
                            if (providerDetail.params.ParentCtrl && providerDetail.params.ParentCtrl.indexOf("EncounterChargeCapture") > -1) {
                                EncounterChargeCapture.Loadfacility();
                            }
                            UnloadActionPan(facilityDetail.params["ParentCtrl"]);
                            CacheManager.BindCodes('GetFacility', true);
                            CacheManager.BindCodes('GetFacilityOutgoingReferral', true);
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
        else if (facilityDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Facility", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    facilityDetail.UpdateFacility(myJSON, facilityDetail.params.FacilityId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            CacheManager.BindCodes('GetFacility', true);
                            CacheManager.BindCodes('GetFacilityOutgoingReferral', true);
                            if (facilityDetail.params.RefCtrl != null) {
                                UnloadActionPan(facilityDetail.params["ParentCtrl"], "facilityDetail");
                            }
                            else {
                                Admin_Facility.FacilitySearch(facilityDetail.params.FacilityId);
                                UnloadActionPan(facilityDetail.params["ParentCtrl"], "facilityDetail");
                            }
                            //To reload Schedule
                            if (providerDetail.params.ParentCtrl && providerDetail.params.ParentCtrl.indexOf("EncounterChargeCapture") > -1) {
                                EncounterChargeCapture.Loadfacility();
                            }
                            if (facilityDetail.params.ParentCtrl == "schTabCalendar") {
                                //Scheduling_Calendar.DayCalendar(facilityDetail.params.ProviderId, facilityDetail.params.ResourceId, facilityDetail.params.FacilityId, facilityDetail.params.DayDate, facilityDetail.params.statusslots);
                                $("#pnlScheduleCalendar #containerScheduleMode .active").trigger('click')
                            }

                            //


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

    SaveFacility: function (FacilityData) {
        var data = "FacilityData=" + FacilityData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FACILITY_DETAIL", "SAVE_FACILITY");
    },

    UpdateFacility: function (FacilityData, FacilityID) {
        var data = "FacilityData=" + FacilityData + "&FacilityID=" + FacilityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FACILITY_DETAIL", "UPDATE_FACILITY");
    },

    FillFacility: function (FacilityID) {
        var data = "FacilityID=" + FacilityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FACILITY_DETAIL", "FILL_FACILITY");
    },

    UpdateFacilityActiveInactive: function (FacilityID, IsActive) {
        var data = "FacilityID=" + FacilityID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FACILITY_DETAIL", "UPDATE_FACILITY_ACTIVE_INACTIVE");
    },

    //FillCityState: function (zipcode, cityname, statename) {
    //    var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE");
    //},

    UnLoad: function () {

        if (facilityDetail.params != null && facilityDetail.params.ParentCtrl != null && facilityDetail.params.PanelID != 'tblfacilityDetail') {
            UnloadActionPan(facilityDetail.params.ParentCtrl, 'facilityDetail', null, facilityDetail.params.PanelID);
        }
        else {
            utility.UnLoadDialog("frmFacilityDetail", function () {
                UnloadActionPan(facilityDetail.params["ParentCtrl"], "facilityDetail");
            }, function () {
                UnloadActionPan(facilityDetail.params["ParentCtrl"], "facilityDetail");
            });
        }
    },

    ShowHistory: function () {
        var PanelID = 'facilityDetail';
        var ParentCtrl = 'facilityDetail';
        var ProfileName = 'Facility';
        var DBTableName = 'Facility';
        var ColumnKeyId = facilityDetail.params.FacilityId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}

