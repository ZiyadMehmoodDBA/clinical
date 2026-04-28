blockHoursDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        blockHoursDetail.params = params;

        var self = $('#blockHoursDetail');

       
        // Start initializing color picker
        $('#blockHoursDetail #divBlockHoursColor').colorpicker();

        $('#blockHoursDetail #divBlockHoursColor').colorpicker().on('changeColor.colorpicker', function (event) {
        });
        self.loadDropDowns(true).done(function () {
            blockHoursDetail.ValidateBlockHours();
            blockHoursDetail.LoadAllAutocomplete().done(function () {
                if (blockHoursDetail.params.mode == "Add" && blockHoursDetail.params.OpenFromSchedulerSlot) {

                    blockHoursDetail.SetValuesFromSchedulerSlot(blockHoursDetail.params);
                }

            });
            blockHoursDetail.LoadBlockHours();

        });
    },
    
    LoadBlockHours: function () {

        if (blockHoursDetail.params.mode == "Add") {
           
            //serialize Data after all controls loaded.
            $('#frmBlockHoursDetail').data('serialize', $('#frmBlockHoursDetail').serialize());

        }
        else if (blockHoursDetail.params.mode == "Edit") {
            blockHoursDetail.FillBlockHours(blockHoursDetail.params.BlockHoursId).done(function (response) {
                if (response.status != false) {
                    var blockHours_detail = JSON.parse(response.BlockHoursFill_JSON);
                    var self = $("#blockHoursDetail");
                    utility.bindMyJSON(true, blockHours_detail, false, self).done(function () {


                        //----------------

                        if (globalAppdata.AppUserName != "MDVISION") {

                            $('#blockHoursDetail #txtFacility').val(blockHours_detail.txtFacility1);
                            $('#blockHoursDetail #txtProvider').val(blockHours_detail.txtProvider1);
                            $('#blockHoursDetail #txtResource').val(blockHours_detail.txtResource1);

                        }

                        //----------------

                        if (blockHours_detail.hfProvider == "0") {

                            $('#blockHoursDetail #rdResource').attr("checked", "checked");

                            $('#blockHoursDetail #divResource').show();
                            $('#blockHoursDetail #divProvider').hide();
                            $('#blockHoursDetail #hfProvider').val(null);
                        }

                        else if (blockHours_detail.hfResource == "0") {

                            $('#blockHoursDetail #rdProvider').attr("checked", "checked");

                            $('#blockHoursDetail #divResource').hide();
                            $('#blockHoursDetail #divProvider').show();
                            $('#blockHoursDetail #hfResource').val(null);
                        }

                        //if (blockHours_detail.chkActive == 'True')
                        //    $("#blockHoursDetail #chkActive").attr("checked", true);
                        //else
                        //    $("#blockHoursDetail #chkActive").attr("checked", false);
                        
                        
                            $('#frmBlockHoursDetail #blockHoursFromTime').timepicker("setTime", blockHours_detail.blockHoursFromTime);
                            $('#frmBlockHoursDetail #blockHoursToTime').timepicker("setTime", blockHours_detail.blockHoursToTime);
                        
                        
                        
                        $('#blockHoursDetail #divBlockHoursColor').colorpicker('setValue', blockHours_detail.txtColor);
                        if (blockHours_detail.chkOverLappingAllowed == 'True')
                            $("#blockHoursDetail #chkOverLappingAllowed").attr("checked", true);
                        else
                            $("#blockHoursDetail #chkOverLappingAllowed").attr("checked", false);
                       
                        //serialize Data after all controls loaded.
                        $('#frmBlockHoursDetail').data('serialize', $('#frmBlockHoursDetail').serialize());

                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    LoadAllAutocomplete: function () {
        var objDeffered = $.Deferred();
        var FacilityDeffered = $.Deferred();
        var ProviderDeffered = $.Deferred();
        var ResourceDeffered = $.Deferred();
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#blockHoursDetail #frmBlockHoursDetail input#txtProvider");
            var hfCtrl = $("#blockHoursDetail #frmBlockHoursDetail #hfProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
            ProviderDeffered.resolve('ok');
        });

        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#blockHoursDetail #frmBlockHoursDetail input#txtFacility");
            var hfCtrl = $('#blockHoursDetail #frmBlockHoursDetail #hfFacility');
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
            FacilityDeffered.resolve('ok');
        });

        CacheManager.BindCodes('GetResources', false).done(function (result) {
            var Ctrl = $("#blockHoursDetail #frmBlockHoursDetail input#txtResource");
            var hfCtrl = $("#blockHoursDetail #frmBlockHoursDetail #hfResource");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Resources, null, hfCtrl);
            ResourceDeffered.resolve('ok');
        });
        $.when(FacilityDeffered, ProviderDeffered, ResourceDeffered).then(function () {

            objDeffered.resolve('ok');
        });

        return objDeffered.promise();
    },

    ValidateBlockHours: function () {
        $('#frmBlockHoursDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   //Facility: {
                   //    group: '.col-sm-6',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   blockReason: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   fromDate: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           date: {
                               format: date_format.toUpperCase(),
                               message: ' '
                           }
                       }
                   },
                   fromTime: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   toDate: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           date: {
                               format: date_format.toUpperCase(),
                               message: ' '
                           }
                       }
                   },
                   toTime: {
                       group: '.col-xs-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Provider: {
                       group: '.col-sm-6',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Resource: {
                       group: '.col-sm-6',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Description: {
                       group: '.col-sm-12',
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
            blockHoursDetail.BlockHoursSave();
        });
    },

    BlockHoursSave: function () {
        var criteria;
        var start = blockHoursDetail.ConvertTimeformat("24", $("#blockHoursFromTime").val().trim());
        var end = blockHoursDetail.ConvertTimeformat("24", $("#blockHoursToTime").val().trim());

        var array = start.split(":");
        var array1 = end.split(":");
        var x = array[0];
        var y = array1[0];

        if (parseInt(x) < parseInt(y))
            criteria = 'true';
        else if (parseInt(x) == parseInt(y)) {
            var w = array[1];
            var n = array1[1];
            if (parseInt(w) == parseInt(n))
                criteria = 'false';
            if (parseInt(w) < parseInt(n))
                criteria = 'true';
            if (parseInt(w) > parseInt(n))
                criteria = 'false';

        }
        else if (parseInt(x) > parseInt(y))
            criteria = 'false';

        if (criteria == 'true') {
            var strMessage = "";
            var self = $("#blockHoursDetail");
            var myJSON = self.getMyJSON();
            if (blockHoursDetail.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("Block Hours", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        blockHoursDetail.SaveBlockHours(myJSON).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                blockHoursDetail.UnLoad();
                                //UnloadActionPan(blockHoursDetail.params.ParentCtrl, 'blockHoursDetail');
                                if (blockHoursDetail.params.OpenFromSchedulerSlot != undefined && blockHoursDetail.params.OpenFromSchedulerSlot) { 
                                    //PMSScheduler.refreshSchBlockHours();
                                    var scheduler = $("#scheduler").data("kendoScheduler");
                                    if (scheduler) {
                                        scheduler.dataSource.read();
                                    }
                                }
                                else
                                    Admin_BlockHours.BlockHoursSearch(response.BlockHoursId);
                                //MDVisionService.reloadLookups = true;
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
            else if (blockHoursDetail.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Block Hours", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        blockHoursDetail.UpdateBlockHours(myJSON, blockHoursDetail.params.BlockHoursId).done(function (response) {
                            if (response.status != false) {
                                if (blockHoursDetail.params.OpenFromSchedulerSlot != undefined && blockHoursDetail.params.OpenFromSchedulerSlot) {
                                    // Do nothing
                                }
                                else
                                    Admin_BlockHours.BlockHoursSearch(blockHoursDetail.params.BlockHoursId);
                                utility.DisplayMessages(response.message, 1);
                                blockHoursDetail.UnLoad();
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
        }
        else
            utility.DisplayMessages('From Time Criteria must be greater then To Time Criteria.', 3);

    },

    ConvertTimeformat: function (format, str) {
        var time = str;
        var hours = Number(time.match(/^(\d+)/)[1]);
        var minutes = Number(time.match(/:(\d+)/)[1]);
        var AMPM = time.match(/\s(.*)$/)[1];
        if (AMPM == "PM" && hours < 12) hours = hours + 12;
        if (AMPM == "AM" && hours == 12) hours = hours - 12;
        var sHours = hours.toString();
        var sMinutes = minutes.toString();
        if (hours < 10) sHours = "0" + sHours;
        if (minutes < 10) sMinutes = "0" + sMinutes;
        // alert(sHours + ":" + sMinutes);
        var x = sHours + ":" + sMinutes;
        return x;

    },

    //----------------Provider--------------------   

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmBlockHoursDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "blockHoursDetail";
        LoadActionPan('Admin_Provider', params);
    },

    //----------------Resource--------------------
    OpenResource: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmBlockHoursDetail";
        params["ResourceId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "blockHoursDetail";
        LoadActionPan('Admin_Resources', params);
    },
    //----------------Facility--------------------
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmBlockHoursDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "blockHoursDetail";
        LoadActionPan('Admin_Facility', params);
    },

    SaveBlockHours: function (BlockHoursData) {
        var data = "BlockHoursData=" + BlockHoursData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BLOCK_HOURS_DETAIL", "SAVE_BLOCK_HOURS");
    },

    UpdateBlockHours: function (BlockHoursData, BlockHoursID) {
        var data = "BlockHoursData=" + BlockHoursData + "&BlockHoursID=" + BlockHoursID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BLOCK_HOURS_DETAIL", "UPDATE_BLOCK_HOURS");
    },

    FillBlockHours: function (BlockHoursID) {
        var data = "BlockHoursID=" + BlockHoursID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BLOCK_HOURS_DETAIL", "FILL_BLOCK_HOURS");
    },

    UpdateBlockHoursActiveInactive: function (BlockHoursID, IsActive) {
        var data = "BlockHoursID=" + BlockHoursID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BLOCK_HOURS_DETAIL", "UPDATE_BLOCK_HOURS_ACTIVE_INACTIVE");
    },

    UnLoad: function () {
        if (blockHoursDetail.params["ParentCtrl"] == "schTabCalendar") {
            UnloadActionPan(blockHoursDetail.params.ParentCtrl, 'blockHoursDetail');
            
            var scheduler = $("#scheduler").data("kendoScheduler");
            if (scheduler) {
                scheduler.dataSource.read();
            }
        }
        else if (blockHoursDetail.params["ParentCtrl"] == "Admin_BlockHours") {
            UnloadActionPan(blockHoursDetail.params.ParentCtrl, 'blockHoursDetail');
        }
        else {
            utility.UnLoadDialog("frmBlockHoursDetail", function () {
                UnloadActionPan();
            }, function () {
                UnloadActionPan();
            });
        }
    },

    BindBlockReason: function () {

        var shortName = $('#frmBlockHoursDetail #lstBlockReasonType').val();
        blockHoursDetail.GetBlockHours(shortName).done(function (response) {

            $('#frmBlockHoursDetail #lstBlockReasonType').autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#pnlDemographic #hfRefProvider").val(ui.item.id); // add the selected id
                        if ($("#pnlDemographic #lnkRefProviderEdit").css("display") == "none") {
                            $("#pnlDemographic #lnkRefProviderEdit").css("display", "inline");
                            $("#pnlDemographic #lblRefProvider").css("display", "none");
                        }
                    }, 100);

                }
            });
            $('#frmBlockHoursDetail #lstBlockReasonType').autocomplete("search");
        });


    },


    GetBlockHours: function (name, IsGetAll) {
        var AllBlockReasons = [];
        var dfd = new $.Deferred();
        blockHoursDetail.LoadBlockhoursDBCall(name).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.ResonsCount > 0) {
                    var Resons = JSON.parse(responseData.ResonsLoad_JSON);
                    $.each(Resons, function (i, item) {

                        AllBlockReasons.push({ id: item.ScheduleReasonId, value: item.ShortName });
                    });
                }
            }
            dfd.resolve(AllBlockReasons);
        });
        return dfd.promise();
    },

    LoadBlockhoursDBCall: function (name) {
        var data = "BlockHours=" + name;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BLOCK_HOURS_DETAIL", "LOAD_BLOCK_HOURS_AUTOCOMPLETE");
    },

    ShowHistory: function () {
        var PanelID = 'blockHoursDetail';
        var ParentCtrl = 'blockHoursDetail';
        var ProfileName = 'Block Hours';
        var DBTableName = 'SchBlockHours';
        var ColumnKeyId = blockHoursDetail.params.BlockHoursId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
    SetValuesFromSchedulerSlot: function (SlotData) {


        var FacilityId = SlotData.FaclilityId;
        var ProviderId = SlotData.ProviderId;
        var ResourceId = SlotData.ResourceId;
        //var FacilityId = 97;
        //var ProviderId = 50;
        //var ResourceId = 49;
        var FromTime = SlotData.FromTime;
        var ToTime = SlotData.ToTime;
        var FromDate = SlotData.FromDate;
        var ToDate = SlotData.EndDate;
        // set facility here
        if (FacilityId) {

            if (Facilities) {
                var Facility = $.grep(Facilities, function (v) {
                    return v.id == FacilityId;
                });
                var facid = Facility[0].id;
                $('#blockHoursDetail #frmBlockHoursDetail #hfFacility').val(facid);
                $('#blockHoursDetail #txtFacility').val(Facility[0].value);

            }
        }


        // set Provider here
        if (ProviderId) {

            if (Providers) {
                var Provider = $.grep(Providers, function (v) {
                    return v.id == ProviderId;
                });

                $('#blockHoursDetail #frmBlockHoursDetail #hfProvider').val(ProviderId);
                $('#blockHoursDetail #txtProvider').val(Provider[0].value);

            }
        }
        // Set Resource here.
        if (ResourceId) {

            if (Resources) {
                var Resource = $.grep(Resources, function (v) {
                    return v.id == ResourceId;
                });

                $('#blockHoursDetail #frmBlockHoursDetail #hfResource').val(ResourceId);
                $('#blockHoursDetail #txtResource').val(Resource[0].value);

            }
        }













        //----------------

        if (ResourceId) {

            $('#blockHoursDetail #rdResource').attr("checked", "checked");

            $('#blockHoursDetail #divResource').show();
            $('#blockHoursDetail #divProvider').hide();
            $('#blockHoursDetail #hfProvider').val(null);
        }

        else if (ProviderId) {

            $('#blockHoursDetail #rdProvider').attr("checked", "checked");

            $('#blockHoursDetail #divResource').hide();
            $('#blockHoursDetail #divProvider').show();
            $('#blockHoursDetail #hfResource').val(null);
        }
        $('#frmBlockHoursDetail #blockHoursFromTime').timepicker("setTime", FromTime);
        $('#frmBlockHoursDetail #blockHoursToTime').timepicker("setTime", ToTime);
        $('#frmBlockHoursDetail #blockHoursFromDate').val(FromDate);
        $('#frmBlockHoursDetail #blockHoursToDate').val(ToDate);

        $("#blockHoursDetail #chkOverLappingAllowed").attr("checked", true);



    },
}