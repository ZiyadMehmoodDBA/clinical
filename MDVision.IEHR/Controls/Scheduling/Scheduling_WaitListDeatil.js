schwaitlistdetail = {
    bIsFirstLoad: true,
    params: [],
    SchedulerMinorTickCount: 4,
    Load: function (params) {
        schwaitlistdetail.params = params;


        var self = $('#schwaitlistdetail');
        if (schwaitlistdetail.bIsFirstLoad) {
            self.loadDropDowns(true).done(function () {

                //serialize Data.
                $('#frmSchedulingWaitListDetail').data('serialize', $('#frmSchedulingWaitListDetail').serialize());
                $("#schwaitlistdetail #ddlPreferredTime option[value='']").remove();
                schwaitlistdetail.FillDDLStatus();
                schwaitlistdetail.LoadAllAutocomplete();
                schwaitlistdetail.LoadWaitList();
                $("#schwaitlistdetail #ddlPreferredTime").val("1");
                schwaitlistdetail.SetSchedulerMinorTickCount();

            });
            //schwaitlistdetail.LoadAllAutocomplete();
        }
        //schwaitlistdetail.LoadWaitList();
        //$("#schwaitlistdetail #ddlPreferredTime").val("1");

    },


    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#schwaitlistdetail #frmSchedulingWaitListDetail input#txtProvider");
            var hfCtrl = $("#schwaitlistdetail #frmSchedulingWaitListDetail #hfProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
        });
        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $("#schwaitlistdetail #frmSchedulingWaitListDetail input#txtFacility");
            var hfCtrl = $('#schwaitlistdetail #frmSchedulingWaitListDetail #hfFacility');
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });
        CacheManager.BindCodes('GetResources', false).done(function (result) {
            var Ctrl = $("#schwaitlistdetail #frmSchedulingWaitListDetail input#txtResource");
            var hfCtrl = $("#schwaitlistdetail #frmSchedulingWaitListDetail #hfResource");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Resources, null, hfCtrl);
        });
        CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
            var Ctrl = $("#schwaitlistdetail #frmSchedulingWaitListDetail input#txtRefProvider");
            var hfCtrl = $("#schwaitlistdetail #frmSchedulingWaitListDetail #hfRefProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", RefProviders, null, hfCtrl);
        });

        var AppointmentDuration = [
                                   { id: 12, name: '5 mint', color: '' },
                                   { id: 6, name: '10 mint', color: '' },
                                   { id: 4, name: '15 mint', color: '' },
                                   { id: 3, name: '20 mint', color: '' },
                                   { id: 2, name: '30 mint', color: '' },
                                   { id: 1, name: '60 mint', color: '' },
        ];

        $("#schwaitlistdetail #appointmentDurationselectWaitlist").kendoDropDownList({
            dataSource: AppointmentDuration,
            dataTextField: "name",
            dataValueField: "id",
            index: 0,
            change: function (e) {
                var widget = $("#waitListScheduler").data("kendoScheduler");
                if (widget) {
                    widget.setOptions({
                        minorTickCount: 1,
                        minorTick: PMSScheduler.getCurrentTick(this.dataItem().id),
                        majorTick: PMSScheduler.getCurrentTick(this.dataItem().id)
                    });
                    widget.view(widget.view().name);
                }
            },
        });

        schwaitlistdetail.BindPatientName();
        schwaitlistdetail.BindPatientAccount();
    },
    SetSchedulerMinorTickCount: function () {
        var dropdownlistDuration = $("#schwaitlistdetail #appointmentDurationselectWaitlist").data("kendoDropDownList");
        if (dropdownlistDuration) {
            dropdownlistDuration.value(schwaitlistdetail.SchedulerMinorTickCount);
        }
    },

    FillDDLStatus: function () {
        CacheManager.BindCodes('GetWaitListStatus', false).done(function (result) {
            var WaitlistStatus = JSON.parse(result.GetWaitListStatus)
            for (var i = 0; i < WaitlistStatus.length; i++) {
                if (WaitlistStatus[i].Name == "Waiting" || WaitlistStatus[i].Name == "WAITING") {

                    $('#schwaitlistdetail #ddlStatus').val(WaitlistStatus[i].Value);
                    $("#schwaitlistdetail #ddlStatus option[value=" + WaitlistStatus[i].Value + "]").attr('selected', 'selected');
                }
            }
        });
    },

    LoadWaitList: function () {
        if (schwaitlistdetail.params.mode == "Add") {

            schwaitlistdetail.ValidateWaitList();
            $('#schwaitlistdetail #btnsearch').attr("disabled", "disabled");
            $('#schwaitlistdetail #DivSearch').css("display", "none");
            $('#schwaitlistdetail #ddlStatus').attr("disabled", "disabled");

            schwaitlistdetail.FillDDLStatus();

            //******* Start Setting default values

            var DefaultProviderId = globalAppdata['DefaultProviderId'];
            var DefaultFacilityId = globalAppdata['DefaultFacilityId'];
            var DefaultProviderName = globalAppdata['DefaultProviderName'];
            var DefaultFacilityName = globalAppdata['DefaultFacilityName'];

            if ($("#PatientProfile #hfPatientId").val() != "") {
                schwaitlistdetail.FillPatientAccount($("#PatientProfile #hfPatientId").val());
            }

            if (DefaultFacilityId != "") {
                $('#schwaitlistdetail #frmSchedulingWaitListDetail #hfFacility').val(DefaultFacilityId);
                $("#schwaitlistdetail #frmSchedulingWaitListDetail input#txtFacility").val(DefaultFacilityName);
                if ($("#schwaitlistdetail #lnkFacilityEdit").css("display") == "none") {
                    $("#schwaitlistdetail #lnkFacilityEdit").css("display", "inline");
                    $("#schwaitlistdetail #lblFacility").css("display", "none");
                }
            } else {
                $('#schwaitlistdetail #frmSchedulingWaitListDetail #hfFacility').val('');
                $("#schwaitlistdetail #frmSchedulingWaitListDetail input#txtFacility").val('');
                $("#schwaitlistdetail #lnkFacilityEdit").css("display", "none");
                $("#schwaitlistdetail #lblFacility").css("display", "inline");
            }

            if (DefaultProviderId != "") {
                $("#schwaitlistdetail #frmSchedulingWaitListDetail input#txtProvider").val(DefaultProviderName);
                $("#schwaitlistdetail #frmSchedulingWaitListDetail #hfProvider").val(DefaultProviderId);
                if ($("#schwaitlistdetail #lnkProviderEdit").css("display") == "none") {
                    $("#schwaitlistdetail #lnkProviderEdit").css("display", "inline");
                    $("#schwaitlistdetail #lblProvider").css("display", "none");
                }
            } else {
                $("#schwaitlistdetail #lnkProviderEdit").css("display", "none");
                $("#schwaitlistdetail #lblProvider").css("display", "inline");
            }

            //******* End Setting default values

            //serialize Data after all controls loaded.
            $('#frmSchedulingWaitListDetail').data('serialize', $('#frmSchedulingWaitListDetail').serialize());
            schwaitlistdetail.bIsFirstLoad = false;
        }
        else if (schwaitlistdetail.params.mode == "Edit") {
            $('#schwaitlistdetail #txtAccount').attr("disabled", "disabled");
            $('#schwaitlistdetail #txtFullName').attr("disabled", "disabled");

            $('#schwaitlistdetail #lnkPatientName').attr("disabled", "disabled");
            $('#schwaitlistdetail #lnkPatientAccount').attr("disabled", "disabled");

            $('#schwaitlistdetail #DivSearch').css("display", "");
            //$('#schwaitlistdetail #btnSaveWaitList').attr("disabled", "disabled");
            schwaitlistdetail.FillWaitList(schwaitlistdetail.params.WaitListId).done(function (response) {
                if (response.status != false) {
                    $('#schwaitlistdetail #btnSaveWaitList').html('Save & Exit');
                    var waitList_detail = JSON.parse(response.WaitListFill_JSON);
                    var waitList_detail_data = JSON.parse(response.WaitListDetail_JSON);
                    var self = $("#schwaitlistdetail");

                    if (waitList_detail_data[0].RefProviderId != "") {
                        if ($("#schwaitlistdetail #lnkRefProviderEdit").css("display") == "none") {
                            $("#schwaitlistdetail #lnkRefProviderEdit").css("display", "inline");
                            $("#schwaitlistdetail #lblRefProvider").css("display", "none");
                        }
                    }

                    if (waitList_detail_data[0].ResourceId == "") {

                        if ($("#schwaitlistdetail #lnkProviderEdit").css("display") == "none") {
                            $("#schwaitlistdetail #lnkProviderEdit").css("display", "inline");
                            $("#schwaitlistdetail #lblProvider").css("display", "none");
                        }

                        $('#schwaitlistdetail #rdProvider').attr("checked", "checked");

                        $('#schwaitlistdetail #divResource').hide();
                        $('#schwaitlistdetail #divProvider').show();
                        $('#schwaitlistdetail #hfResource').val(null);

                    }
                    else if (waitList_detail_data[0].ProviderId == "") {

                        if ($("#schwaitlistdetail #lnkResourceEdit").css("display") == "none") {
                            $("#schwaitlistdetail #lnkResourceEdit").css("display", "inline");
                            $("#schwaitlistdetail #lblResource").css("display", "none");
                        }

                        $('#schwaitlistdetail #rdResource').attr("checked", "checked");

                        $('#schwaitlistdetail #divResource').show();
                        $('#schwaitlistdetail #divProvider').hide();
                        $('#schwaitlistdetail #hfProvider').val(null);

                    }

                    if ($("#schwaitlistdetail #lnkFacilityEdit").css("display") == "none") {
                        $("#schwaitlistdetail #lnkFacilityEdit").css("display", "inline");
                        $("#schwaitlistdetail #lblFacility").css("display", "none");
                    }

                    utility.bindMyJSON(true, waitList_detail, false, self).done(function () {
                        schwaitlistdetail.bIsFirstLoad = false;
                        $('#schwaitlistdetail #txtAccount').val(waitList_detail_data[0].AccountNumber);
                        $('#schwaitlistdetail #txtFullName').val(waitList_detail_data[0].PatientName);
                        if (schwaitlistdetail.params.Status == "Canceled" || schwaitlistdetail.params.Status == "Canceled By Patient" || schwaitlistdetail.params.Status == "Not Available" || schwaitlistdetail.params.Status == "Booked") {
                            $('#schwaitlistdetail #btnsearch').attr("disabled", "disabled");
                            $('#schwaitlistdetail #DivSearch').css("display", "none");
                        }
                        if (schwaitlistdetail.params.Status == "Booked") {

                            $("#schwaitlistdetail #frmSchedulingWaitListDetail :input").attr("disabled", true);

                            //$('#schwaitlistdetail #DivSearch').css("display", "none");
                            //$('#schwaitlistdetail #btnsearch').attr("disabled", "disabled");
                            //$('#schwaitlistdetail #ddlStatus').attr("disabled", "disabled");
                            //$('#schwaitlistdetail #btnSaveWaitList').attr("disabled", "disabled");

                            if ($("#schwaitlistdetail #pnlBookedApp_Result").css("display") == "none") {
                                $("#schwaitlistdetail #pnlBookedApp_Result").show();
                            }

                            $("#schwaitlistdetail #buttonDiv").show();
                            schwaitlistdetail.BookedAppGridLoad(response);

                        }
                        if (waitList_detail_data[0].IsPreferredDay == "1") {

                            $('#schwaitlistdetail #rdWeekDay').attr('checked', true);
                            $("#schwaitlistdetail #chkSaturday").attr("disabled", false);
                            $("#schwaitlistdetail #chkSunday").attr("disabled", false);
                            $("#schwaitlistdetail #chkMonday").attr("disabled", false);
                            $("#schwaitlistdetail #chkTuesday").attr("disabled", false);
                            $("#schwaitlistdetail #chkWednesday").attr("disabled", false);
                            $("#schwaitlistdetail #chkThursday").attr("disabled", false);
                            $("#schwaitlistdetail #chkFriday").attr("disabled", false);
                            if (waitList_detail_data[0].PreferredDay != "") {
                                var s = waitList_detail_data[0].PreferredDay;
                                var match = s.split(',')
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#schwaitlistdetail #chkSaturday').attr('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#schwaitlistdetail #chkSaturday').attr('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#schwaitlistdetail #chkSunday').attr('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#schwaitlistdetail #chkSunday').attr('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#schwaitlistdetail #chkMonday').attr('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#schwaitlistdetail #chkMonday').attr('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#schwaitlistdetail #chkTuesday').attr('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#schwaitlistdetail #chkTuesday').attr('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#schwaitlistdetail #chkWednesday').attr('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#schwaitlistdetail #chkWednesday').attr('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#schwaitlistdetail #chkThursday').attr('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#schwaitlistdetail #chkThursday').attr('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#schwaitlistdetail #chkFriday').attr('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#schwaitlistdetail #chkFriday').attr('checked', false);
                                    }
                                    x++;

                                }
                            }
                        }

                        else if (waitList_detail_data[0].IsPreferredDay == "2") {
                            $('#schwaitlistdetail #rdAnyDay').attr('checked', true);

                        }
                        else if (waitList_detail_data[0].IsPreferredDay == "3") {
                            $('#schwaitlistdetail #rdCustom').attr('checked', true);
                            $("#schwaitlistdetail #chkSaturday").attr("disabled", false);
                            $("#schwaitlistdetail #chkSunday").attr("disabled", false);
                            $("#schwaitlistdetail #chkMonday").attr("disabled", false);
                            $("#schwaitlistdetail #chkTuesday").attr("disabled", false);
                            $("#schwaitlistdetail #chkWednesday").attr("disabled", false);
                            $("#schwaitlistdetail #chkThursday").attr("disabled", false);
                            $("#schwaitlistdetail #chkFriday").attr("disabled", false);

                            if (waitList_detail_data[0].PreferredDay != "") {
                                var s = waitList_detail_data[0].PreferredDay;
                                var match = s.split(',')
                                var x = 0;
                                for (var a in match) {
                                    var variable = match[a];

                                    if (x == 0 && variable == "1") {
                                        $('#schwaitlistdetail #chkSaturday').attr('checked', true);
                                    }
                                    if (x == 0 && variable == "0") {
                                        $('#schwaitlistdetail #chkSaturday').attr('checked', false);
                                    }
                                    if (x == 1 && variable == "1") {
                                        $('#schwaitlistdetail #chkSunday').attr('checked', true);
                                    }
                                    if (x == 1 && variable == "0") {
                                        $('#schwaitlistdetail #chkSunday').attr('checked', false);
                                    }
                                    if (x == 2 && variable == "1") {
                                        $('#schwaitlistdetail #chkMonday').attr('checked', true);
                                    }
                                    if (x == 2 && variable == "0") {
                                        $('#schwaitlistdetail #chkMonday').attr('checked', false);
                                    }
                                    if (x == 3 && variable == "1") {
                                        $('#schwaitlistdetail #chkTuesday').attr('checked', true);
                                    }
                                    if (x == 3 && variable == "0") {
                                        $('#schwaitlistdetail #chkTuesday').attr('checked', false);
                                    }
                                    if (x == 4 && variable == "1") {
                                        $('#schwaitlistdetail #chkWednesday').attr('checked', true);
                                    }
                                    if (x == 4 && variable == "0") {
                                        $('#schwaitlistdetail #chkWednesday').attr('checked', false);
                                    }
                                    if (x == 5 && variable == "1") {
                                        $('#schwaitlistdetail #chkThursday').attr('checked', true);
                                    }
                                    if (x == 5 && variable == "0") {
                                        $('#schwaitlistdetail #chkThursday').attr('checked', false);
                                    }
                                    if (x == 6 && variable == "1") {
                                        $('#schwaitlistdetail #chkFriday').attr('checked', true);
                                    }
                                    if (x == 6 && variable == "0") {
                                        $('#schwaitlistdetail #chkFriday').attr('checked', false);
                                    }
                                    x++;

                                }
                            }
                        }

                        if (waitList_detail.preferredDate == "") {

                            $("#schwaitlistdetail #rdPrefDate").attr("disabled", true);
                            $("#schwaitlistdetail #rdFrmToDate").attr('checked', true);

                        }
                        else if (waitList_detail.preferredDate != "") {

                            $("#schwaitlistdetail #rdPrefDate").attr("disabled", false);
                            $("#schwaitlistdetail #rdPrefDate").attr('checked', true);

                        }

                        $('#frmSchedulingWaitListDetail').data('serialize', $('#frmSchedulingWaitListDetail').serialize());

                    });


                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            schwaitlistdetail.ValidateWaitList();
        }
    },

    ValidateWaitList: function () {
        $('#frmSchedulingWaitListDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   FullName: {
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
                   Resource: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //Reason: {
                   //    group: '.col-sm-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   fromdate: {
                       group: '.col-sm-3',
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
                   todate: {
                       group: '.col-sm-3',
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
                   Status: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   'theDays[]': {
                       feedbackIcons: 'false',
                       group: '.col-md-8',
                       enable: false,
                       validators: {
                           choice: {
                               min: 1,
                               message: 'Choose atleast one checkbox.'
                           }
                       }
                   },
                   //rdPreferredDay: {
                   //    group: '.col-sm-3',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //}
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            schwaitlistdetail.WaitListSave();
        });
    },

    WaitListSave: function () {
        //Start 28/01/2016 Muhammad Irfan for bug # PMS-3440
        if ($('#schwaitlistdetail #preferredDate').val() != "") {
            if ($('#schwaitlistdetail #preferredDate').val() != "")
                var preferddate = new Date($('#schwaitlistdetail #preferredDate').val());
            else
                var preferddate = $('#schwaitlistdetail #preferredDate').datepicker('getDate');
            var todate = new Date($('#schwaitlistdetail #toDate').val());
            var fromdate = new Date($('#schwaitlistdetail #fromDate').val());

            if (preferddate != 'Invalid Date') {
                if ((preferddate >= fromdate) && (preferddate <= todate)) {
                    var strMessage = "";
                    var self = $("#schwaitlistdetail");
                    var myJSON = self.getMyJSON();
                    if (schwaitlistdetail.params.mode == "Add") {
                        AppPrivileges.GetFormPrivileges("Wait List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                schwaitlistdetail.SaveWaitList(myJSON).done(function (response) {
                                    if (response.status != false) {
                                        $("#pnlScheduleWaitList #ddlPreferredTime").val("");
                                        Scheduling_WaitList.WaitListSearch(response.WaitListId);
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
                    else if (schwaitlistdetail.params.mode == "Edit") {
                        AppPrivileges.GetFormPrivileges("Wait List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                schwaitlistdetail.UpdateWaitList(myJSON, schwaitlistdetail.params.WaitListId).done(function (response) {
                                    if (response.status != false) {
                                        $("#pnlScheduleWaitList #ddlPreferredTime").val("");
                                        Scheduling_WaitList.WaitListSearch(schwaitlistdetail.params.WaitListId);
                                        utility.DisplayMessages(response.message, 1);

                                        UnloadActionPan();

                                        var value = $("#schwaitlistdetail #ddlStatus").val();
                                        if (value != '3' && value != '2' && value != '1' && value != '5') {
                                            $('#schwaitlistdetail #btnsearch').prop("disabled", false);
                                            $('#schwaitlistdetail #DivSearch').css("display", "block");
                                        }



                                        //var h = ($("#schwaitlistdetail #preferredDate").text());
                                        //var g = ($("#schwaitlistdetail #preferredDate").val());

                                        if ($("#schwaitlistdetail #preferredDate").val() == "") {

                                            $("#schwaitlistdetail #rdPrefDate").attr("disabled", true);
                                            $("#schwaitlistdetail #rdPrefDate").attr('checked', false);
                                            $("#schwaitlistdetail #rdFrmToDate").attr('checked', true);

                                        }
                                        else if ($("#schwaitlistdetail #preferredDate").val() != "") {

                                            $("#schwaitlistdetail #rdPrefDate").attr("disabled", false);
                                            $("#schwaitlistdetail #rdPrefDate").attr('checked', true);

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
                }
                else
                    utility.DisplayMessages('Preferred Date must be between From and To Date.', 3);

            }
        }
        else {
            var strMessage = "";
            var self = $("#schwaitlistdetail");
            var myJSON = self.getMyJSON();
            if (schwaitlistdetail.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("Wait List", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        schwaitlistdetail.SaveWaitList(myJSON).done(function (response) {
                            if (response.status != false) {
                                $("#pnlScheduleWaitList #ddlPreferredTime").val("");
                                Scheduling_WaitList.WaitListSearch(response.WaitListId);
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
            else if (schwaitlistdetail.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Wait List", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        schwaitlistdetail.UpdateWaitList(myJSON, schwaitlistdetail.params.WaitListId).done(function (response) {
                            if (response.status != false) {
                                $("#pnlScheduleWaitList #ddlPreferredTime").val("");
                                Scheduling_WaitList.WaitListSearch(schwaitlistdetail.params.WaitListId);
                                utility.DisplayMessages(response.message, 1);
                                UnloadActionPan();
                                var value = $("#schwaitlistdetail #ddlStatus").val();
                                if (value != '3' && value != '2' && value != '1' && value != '5') {
                                    $('#schwaitlistdetail #btnsearch').prop("disabled", false);
                                    $('#schwaitlistdetail #DivSearch').css("display", "block");
                                }


                                //var h = ($("#schwaitlistdetail #preferredDate").text());
                                //var g = ($("#schwaitlistdetail #preferredDate").val());

                                if ($("#schwaitlistdetail #preferredDate").val() == "") {

                                    $("#schwaitlistdetail #rdPrefDate").attr("disabled", true);
                                    $("#schwaitlistdetail #rdPrefDate").attr('checked', false);
                                    $("#schwaitlistdetail #rdFrmToDate").attr('checked', true);

                                }
                                else if ($("#schwaitlistdetail #preferredDate").val() != "") {

                                    $("#schwaitlistdetail #rdPrefDate").attr("disabled", false);
                                    $("#schwaitlistdetail #rdPrefDate").attr('checked', true);

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
        }

    },

    // -------------- Patient ---------------------

    ResetPatientValue: function () {
        $('#schwaitlistdetail #hfPatientid').val("0");
    },

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'schwaitlistdetail';
        LoadActionPan('Patient_Search', params);
    },

    BindPatientAccount: function () {
        var valid = false;
        var Ctrl = $("#schwaitlistdetail input#txtAccount");
        var hfCtrl = $('#schwaitlistdetail #hfPatientid');
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.AccountNumber && obj.AccountNumber.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.AccountNumber;
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
            }
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            Ctrl.val(dataItem.AccountNumber);
            $('#schwaitlistdetail #txtFullName').val(dataItem.FullName);
            utility.InsertRecentPatient(dataItem.id);
            utility.SetKendoAutoCompleteSourceforValidate($('#schwaitlistdetail #txtFullName'), dataItem.FullName, $('#schwaitlistdetail #hfPatientid'), dataItem.id, "FullName");
        }
        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 3,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        utility.GetPatientArray(Ctrl.val(), 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },
    BindPatientName: function () {
        var valid = false;
        var Ctrl = $("#schwaitlistdetail input#txtFullName");
        var hfCtrl = $('#schwaitlistdetail #hfPatientid');
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.FullName && obj.FullName.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.FullName;
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
            }
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            var patientid = dataItem.id;
            Ctrl.val(dataItem.FullName);
            $('#schwaitlistdetail #txtAccount').val(dataItem.AccountNumber);
            utility.InsertRecentPatient(dataItem.id);
            Patient_Demographic.FillPatientAlertsCount('1', patientid, 'schwaitlistdetail').done(function () {
                if (patientid != $('#PatientProfile #hfPatientId').val()) {
                    Patient_Demographic.isFinanicialAlert = true;
                }
            });
            utility.SetKendoAutoCompleteSourceforValidate($('#schwaitlistdetail #txtAccount'), dataItem.AccountNumber, $('#schwaitlistdetail #hfPatientid'), dataItem.id, "AccountNumber");
        }
        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 3,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        utility.GetPatientArrayByName(Ctrl.val(), 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },

    FillPatientInfoFromSearch: function (PatientId, event) {
        if (event != null) {
            event.stopPropagation();
            if (event.target.type == "checkbox") {
                $(':checkbox', this).trigger('click');
                return;
            }
        }
        setTimeout(function () { schwaitlistdetail.FillPatientAccount(PatientId); }, 200);
        UnloadActionPan("schwaitlistdetail");
        $('#frmSchedulingWaitListDetail').bootstrapValidator('revalidateField', 'FullName');
        utility.InsertRecentPatient(PatientId);
    },

    FillPatientAccount: function (PatientId) {
        var dfd = new $.Deferred();
        schwaitlistdetail.FillPatient(PatientId).done(function (response) {
            if (response.status != false) {
                Patient_Demographic.FillPatientAlertsCount('1', PatientId, 'schwaitlistdetail').done(function () {
                    if (PatientId != $('#PatientProfile #hfPatientId').val()) {
                        Patient_Demographic.isFinanicialAlert = true;
                    }
                });
                $('#schwaitlistdetail #txtRefProvider').val("");
                var patient_detail = JSON.parse(response.PatientFill_JSON);
                var patient = JSON.parse(response.Patient_JSON);

                var self = $("#schwaitlistdetail");
                utility.bindMyJSON(true, patient_detail, false, self);
                $('#schwaitlistdetail #txtAccount').val(patient[0].AccountNumber);
                $('#schwaitlistdetail #txtFullName').val(patient[0].FullName);
                utility.SetKendoAutoCompleteSourceforValidate($('#schwaitlistdetail #txtAccount'), patient[0].AccountNumber, $('#schwaitlistdetail #hfPatientid'), patient[0].PatientId, "AccountNumber");
                utility.SetKendoAutoCompleteSourceforValidate($('#schwaitlistdetail #txtFullName'), patient[0].FullName, $('#schwaitlistdetail #hfPatientid'), patient[0].PatientId, "FullName");
                CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].Value == patient[0].ReferringProviderId) {

                            $('#schwaitlistdetail #txtRefProvider').val(patient[0].ReferringProviderName);
                            $('#schwaitlistdetail #hfRefProvider').val(patient[0].ReferringProviderId);
                        }
                    }
                });
                CacheManager.BindCodes('GetWaitListStatus', false).done(function (result) {
                    for (var i = 0; i < result.length; i++) {
                        if (result[i].Name == "Waiting" || result[i].Name == "WAITING") {

                            $('#schwaitlistdetail #ddlStatus').val(result[i].Value);
                            $("#schwaitlistdetail #ddlStatus option[value=" + result[i].Value + "]").attr('selected', 'selected');
                        }
                    }
                });
                $('#frmSchedulingWaitListDetail').bootstrapValidator('revalidateField', 'FullName');
                $("#ddlPreferredTime option[value='']").remove();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd.promise();
    },
    //----------------Provider--------------------
    ResetProviderValue: function () {
        $('#schwaitlistdetail #hfProvider').val("0");
    },

    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmSchedulingWaitListDetail";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schwaitlistdetail";
        LoadActionPan('Admin_Provider', params);
    },

    //----------------Resource--------------------
    OpenResource: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmSchedulingWaitListDetail";
        params["ResourceId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schwaitlistdetail";
        LoadActionPan('Admin_Resources', params);
    },

    ResetResourceValue: function () {
        $('#schwaitlistdetail #hfResource').val("0");
    },
    //----------------Facility--------------------

    ResetFacilityValue: function () {
        $('#schwaitlistdetail #hfFacility').val("0");
    },

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmSchedulingWaitListDetail";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schwaitlistdetail";
        LoadActionPan('Admin_Facility', params);
    },
    //----------------Referring Provider--------------------
    ResetRefProvider: function () {

        $('#schwaitlistdetail #frmSchedulingWaitListDetail #hfRefProvider').val('0');

    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";

        params["ParentCtrl"] = "schwaitlistdetail";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    //----------------Schedule Search-----------------------

    ScheduleSearch: function (PageNo, rpp) {

        var schDate = $("#rdPrefDate").prop('checked') ? $("#preferredDate").val() : $("#fromDate").val();
        schwaitlistdetail.InitilizeScheduler(schDate);
    },

    BatchScheduleGridLoad: function (response) {
        $("#dgvSchSearch").dataTable().fnDestroy();
        $("#pnlSchSearch_Result #dgvSchSearch tbody").find("tr").remove();
        if (response.ScheduleSearchCount > 0) {
            var ScheduleSearchJSONData = JSON.parse(response.ScheduleSearch_JSON);
            $.each(ScheduleSearchJSONData, function (i, item) {
                var $row = $('<tr/>');
                var Reasons = item.Reasons;
                if ($("#frmSchedulingWaitListDetail #txtSchReason").val() != "") {
                    Reasons = $("#frmSchedulingWaitListDetail #txtSchReason").val();
                }

                if (ScheduleSearchJSONData[i].SlotStatusId == '1') {
                    //blocked
                    $row.append('<td>Blocked</td><td>' + item.PatientAllowed + '</td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="""" width="63">' + item.FromTimeSlots + '</td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');
                    $row.css('color', 'white');
                    $row.css('background-color', '#f88379');
                }
                else {


                    if (ScheduleSearchJSONData[i].AppCounts == 0) {
                        //open
                        $row.append('<td><button class="btn btn-link btn-xs" type="button" name="checkbox" id="' + item.TimeSlotDtlId + '" onclick="schwaitlistdetail.OpenAppointmentDetail(' + item.TimeSlotId + ',' + item.TimeSlotDtlId + ',\'' + item.ProviderId + '\',\'' + item.Provider + '\',\'' + item.Resources + '\',\'' + item.ResourceId + '\',\'' + item.FacilityId + '\',\'' + item.Facility + '\',\'' + item.SchReasonId + '\',\'' + item.PatientAllowed + '\',\'' + item.AppCounts + '\',\'' + item.OverBooked + '\', \'' + Reasons + '\', \'' + item.Minutes + '\', \'' + item.FromTimeSlots + '\', \'' + item.ToTimeSlots + '\', \'' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '\');"><i title="Add Appointment" class="fa fa-calendar "></i></button></td><td>' + item.PatientAllowed + '</td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="""" width="63">' + item.FromTimeSlots + '</td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');
                        $row.css('background-color', '#fff');
                    }
                    else {

                        if (ScheduleSearchJSONData[i].AppCounts > ScheduleSearchJSONData[i].PatientAllowed) {

                            //Over Booked
                            $row.append('<td><button class="btn btn-link btn-xs" type="button" name="checkbox" id="' + item.TimeSlotDtlId + '" onclick="schwaitlistdetail.OpenAppointmentDetail(' + item.TimeSlotId + ',' + item.TimeSlotDtlId + ',\'' + item.ProviderId + '\',\'' + item.Provider + '\',\'' + item.Resources + '\',\'' + item.ResourceId + '\',\'' + item.FacilityId + '\',\'' + item.Facility + '\',\'' + item.SchReasonId + '\',\'' + item.PatientAllowed + '\',\'' + item.AppCounts + '\',\'' + item.OverBooked + '\', \'' + Reasons + '\', \'' + item.Minutes + '\', \'' + item.FromTimeSlots + '\', \'' + item.ToTimeSlots + '\', \'' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '\');"><i title="Add Appointment" class="fa fa-calendar white"></i></button></td><td>' + item.PatientAllowed + '</td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="""" width="63">' + item.FromTimeSlots + '</td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');
                            $row.css('color', 'white');
                            $row.css('background-color', '#CC6666');
                        }
                        else if (ScheduleSearchJSONData[i].AppCounts < ScheduleSearchJSONData[i].PatientAllowed) {
                            //Booked
                            $row.append('<td><button class="btn btn-link btn-xs" type="button" name="checkbox" id="' + item.TimeSlotDtlId + '" onclick="schwaitlistdetail.OpenAppointmentDetail(' + item.TimeSlotId + ',' + item.TimeSlotDtlId + ',\'' + item.ProviderId + '\',\'' + item.Provider + '\',\'' + item.Resources + '\',\'' + item.ResourceId + '\',\'' + item.FacilityId + '\',\'' + item.Facility + '\',\'' + item.SchReasonId + '\',\'' + item.PatientAllowed + '\',\'' + item.AppCounts + '\',\'' + item.OverBooked + '\', \'' + Reasons + '\', \'' + item.Minutes + '\', \'' + item.FromTimeSlots + '\', \'' + item.ToTimeSlots + '\', \'' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '\');"><i title="Add Appointment" class="fa fa-calendar white"></i></button></td><td>' + item.PatientAllowed + '</td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="""" width="63">' + item.FromTimeSlots + '</td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');
                            $row.css('color', 'white');
                            $row.css('background-color', '#76B007');
                        }
                        else if (ScheduleSearchJSONData[i].PatientAllowed == ScheduleSearchJSONData[i].AppCounts) {

                            //full
                            $row.append('<td><button class="btn btn-link btn-xs" type="button" name="checkbox" id="' + item.TimeSlotDtlId + '" onclick="schwaitlistdetail.OpenAppointmentDetail(' + item.TimeSlotId + ',' + item.TimeSlotDtlId + ',\'' + item.ProviderId + '\',\'' + item.Provider + '\',\'' + item.Resources + '\',\'' + item.ResourceId + '\',\'' + item.FacilityId + '\',\'' + item.Facility + '\',\'' + item.SchReasonId + '\',\'' + item.PatientAllowed + '\',\'' + item.AppCounts + '\',\'' + item.OverBooked + '\', \'' + Reasons + '\', \'' + item.Minutes + '\', \'' + item.FromTimeSlots + '\', \'' + item.ToTimeSlots + '\', \'' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '\');"><i title="Add Appointment" class="fa fa-calendar white"></i></button></td><td>' + item.PatientAllowed + '</td><td>' + item.Date.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td onclick="""" width="63">' + item.FromTimeSlots + '</td><td>' + item.Day + '</td><td>' + item.Minutes + '</td><td>' + item.BookedPatients + '</td><td>' + item.Provider + '</td><td>' + item.Resources + '</td><td>' + item.Reasons + '</td><td>' + item.OverBooked + '</td><td>' + item.Facility + '</td>');
                            $row.css('color', 'white');
                            $row.css('background-color', '#FE7510');
                        }

                    }
                }


                $("#pnlSchSearch_Result #dgvSchSearch tbody").last().append($row);
            });
        }
        else {
            $("#schwaitlistdetail #divSchPaging").css("display", "none");
            $('#dgvSchSearch').DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvSchSearch'))
            ;
        else
            $("#pnlSchSearch_Result #dgvSchSearch").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    //----------------End Schedule Search-------------------

    //----------------Add Appointment-------------------
    OpenAppointmentDetail: function (TimeSlotId, TimeSlotDtlId, ProviderId, Provider, Resources, ResourceId, FacilityId, Facility, ReasonId, PatientAllowed, PatientBooked, OverBooked, Reasons, SlotMinutes, FromTime, ToTime, scheduleDate) {


        AppPrivileges.GetFormPrivileges("Appointment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {



                params: [];
                params["SlotMinutes"] = SlotMinutes;
                params["ProviderId"] = ProviderId;
                params["ProviderName"] = Provider;
                params["ResourceId"] = ResourceId;
                params["ResourceName"] = Resources;
                params["FacilityId"] = FacilityId;
                params["FacilityName"] = Facility;
                params["ScheduleReasonId"] = ReasonId;
                params["ScheduleReason"] = Reasons;
                params["Time"] = FromTime;
                params["ToTime"] = ToTime;
                params["ScheduleDate"] = scheduleDate;
                params["PatientId"] = $("#schwaitlistdetail #hfPatientid").val();
                params["WaitListId"] = schwaitlistdetail.params.WaitListId;
                params["mode"] = "Add";
                params["ParentCtrl"] = "schwaitlistdetail";
                LoadActionPan('appointmentDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    BookedAppGridLoad: function (response) {

        if ($("#schwaitlistdetail #pnlBookedApp_Result").css("display") == "none") {
            $("#schwaitlistdetail #pnlBookedApp_Result").show();
        }

        $("#dgvBookedApp").dataTable().fnDestroy();
        $("#pnlBookedApp_Result #dgvBookedApp tbody").find("tr").remove();
        // if (response.AppointmentStatusCount > 0) {
        var AppointmentJSONData = JSON.parse(response.WaitListDetail_JSON);
        $.each(AppointmentJSONData, function (i, item) {
            var $row = $('<tr/>');
            $row.attr("onclick", "utility.SelectGridRow($('#gvAppointment_row" + item.PatientId + "'))");
            $row.attr("id", "gvAppointment_row" + item.PatientId);
            $row.attr("PatientId", item.PatientId);

            if (item.IsActive == "True") {
                isactive = 0;
                activeTitle = "Active Record";
                tglclass = "fa fa-toggle-on green";
            }
            else {
                isactive = 1;
                activeTitle = "Inactive Record";
                tglclass = "fa fa-toggle-on red";
            }
            $row.append('<td style="display:none;">' + item.PatientId + '</td><td>' + item.PatientName + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.ResourceName + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.ReasonName + '">' + item.ReasonName + '</td>');


            $("#pnlBookedApp_Result #dgvBookedApp tbody").last().append($row);
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        });
        //}
        //else {
        //    $('#dgvBookedApp').DataTable({
        //        "language": {
        //            "emptyTable": "No Appointment Found"
        //        }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
        //    });
        //}
        if ($.fn.dataTable.isDataTable('#dgvBookedApp'))
            ;
        else {
            $("#pnlBookedApp_Result #dgvBookedApp").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            //Start 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
            utility.removePaginationFromGrid($('#pnlBookedApp_Result'));
            //End 06-07-2018 TahreemMalik If no record found and data table is empty, pagination will not show. MA-537
        }
    },
    //----------------End Appointment-------------------

    LoadActivePatients: function (AccountNo) {
        var data = "AccountNo=" + AccountNo;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST_DETAIL", "SEARCH_PATIENT");
    },

    FillPatient: function (PatientID) {
        var data = "PatientID=" + PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST_DETAIL", "FILL_PATIENT");
    },

    SaveWaitList: function (WaitListData) {
        var data = "WaitListData=" + WaitListData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST_DETAIL", "SAVE_WAITLIST");
    },

    FillWaitList: function (WaitListId) {
        var data = "WaitListId=" + WaitListId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST_DETAIL", "FILL_WAITLIST");
    },

    UpdateWaitList: function (WaitListData, WaitListId) {
        var data = "WaitListData=" + WaitListData + "&WaitListId=" + WaitListId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST_DETAIL", "UPDATE_WAITLIST");
    },

    UpdateWaitListActiveInactive: function (WaitListId, IsActive) {
        var data = "WaitListId=" + WaitListId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST_DETAIL", "UPDATE_WAITLIST_ACTIVE_INACTIVE");
    },

    SearchBatchSchedule: function (SchedulingSearchData, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var data = "SchedulingSearchData=" + SchedulingSearchData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_WAITLIST_DETAIL", "SEARCH_BATCH_SCHEDULING");
    },

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlSchSearch_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        schwaitlistdetail.ScheduleSearch(PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#schwaitlistdetail #pnlSchSearch_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            schwaitlistdetail.ScheduleSearch(currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#schwaitlistdetail #pnlSchSearch_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            schwaitlistdetail.ScheduleSearch(currentPageNo, 15);
        }
    },

    UnLoad: function () {
        UnloadActionPan(schwaitlistdetail.params["ParentCtrl"], "schwaitlistdetail");
    },

    //-----------------------
    OpenRefProviderDetail: function () {
        var params = [];
        params["ReferringProviderId"] = $('#schwaitlistdetail #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "schwaitlistdetail";

        LoadActionPan('referringproviderDetail', params);
    },

    OpenProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#schwaitlistdetail #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = 'schwaitlistdetail';
        LoadActionPan('providerDetail', params);
    },

    OpenResourceDetail: function () {
        var params = [];
        params["ResourceId"] = $('#schwaitlistdetail #hfResource').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtResource";
        params["ParentCtrl"] = 'schwaitlistdetail';
        LoadActionPan('resourcesDetail', params);
    },

    OpenFacilityDetail: function () {
        var params = [];
        params["FacilityId"] = $('#schwaitlistdetail #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        params["ParentCtrl"] = 'schwaitlistdetail';
        LoadActionPan('facilityDetail', params);
    },

    ResetLink: function () {
        if ($("#schwaitlistdetail #hfProvider").val() == "") {
            $("#schwaitlistdetail #lblProvider").css("display", "inline");
            $("#schwaitlistdetail #lnkProviderEdit").css("display", "none");
        }

        if ($("#schwaitlistdetail #hfResource").val() == "") {
            $("#schwaitlistdetail #lblResource").css("display", "inline");
            $("#schwaitlistdetail #lnkResourceEdit").css("display", "none");
        }
    },

    PrefferDayRadioButtonClick: function () {
        var formValidation = $("#schwaitlistdetail #frmSchedulingWaitListDetail").data("bootstrapValidator");
        if ($('#schwaitlistdetail input[name="rdPreferredDay"]:radio:checked').val() == "1") {

            $("#schwaitlistdetail #divPreferedDay input[type='checkbox']").filter(':checkbox').prop('checked', false);
            formValidation.enableFieldValidators('theDays[]', true);
            $("#schwaitlistdetail #divPreferedDay input[type='checkbox']").attr("disabled", false);
            $("#schwaitlistdetail #chkSaturday").attr("disabled", true);
            $("#schwaitlistdetail #chkSunday").attr("disabled", true);

        } else if ($('#schwaitlistdetail input[name="rdPreferredDay"]:radio:checked').val() == "3") {
            $("#schwaitlistdetail #divPreferedDay input[type='checkbox']").filter(':checkbox').prop('checked', false);
            formValidation.enableFieldValidators('theDays[]', true);
            $("#schwaitlistdetail #divPreferedDay input[type='checkbox']").attr("disabled", false);

        } else if ($('#schwaitlistdetail input[name="rdPreferredDay"]:radio:checked').val() == "2") {
            $("#schwaitlistdetail #divPreferedDay input[type='checkbox']").filter(':checkbox').prop('checked', false);
            formValidation.enableFieldValidators('theDays[]', false);
            $('#schwaitlistdetail #frmSchedulingWaitListDetail').bootstrapValidator('revalidateField', 'theDays[]');
            $("#schwaitlistdetail #divPreferedDay input[type='checkbox']").attr("disabled", true);

        }

    },

    //-----------------------------------------

    BindScheduleReasons: function () {
        var SchReason = $('#schwaitlistdetail #txtSchReason').val();
        var AllSchReasons = [];
        var dfd = new $.Deferred();
        if (SchReason.length > 2) {
            utility.Keyupdelay(function () {
                blckreasonDetail.LoadScheduleReasons(SchReason).done(function (responseData) {
                    if (responseData.status != false) {
                        if (responseData.SchReasonCount > 0) {
                            var ScheduleReasons = JSON.parse(responseData.ScheduleReasonsLoad_JSON);

                            $.each(ScheduleReasons, function (i, item) {
                                AllSchReasons.push({ id: item.ScheduleReasonId, value: item.ShortName, Duration: item.Duration });
                            });

                        }
                    }
                    dfd.resolve(AllSchReasons);
                });
            });
        } else {
            dfd.resolve(AllSchReasons);
        }

        //---------------
        dfd.then(function () {

            $('#schwaitlistdetail #txtSchReason').autocomplete({
                autoFocus: true,
                source: AllSchReasons,
                open: function (event, ui) { disable = true },
                close: function (event, ui) {
                    disable = false; $(this).focus();
                },
                select: function (event, ui) {
                    setTimeout(function () {
                        $('#schwaitlistdetail #txtSchReason').val(ui.item.value);
                        $('#schwaitlistdetail #hfSchReasonId').val(ui.item.id);
                    }, 100);

                }
            }).blur(function () {
                setTimeout(function () {
                    utility.ValidateAutoComplete($('#schwaitlistdetail #txtSchReason'), 'schwaitlistdetail #hfSchReasonId', false, null, null, null);
                }, 200);
            });
            $('#schwaitlistdetail #txtSchReason').autocomplete("search");

        });

        //--------------------
    },

    OpenScheduleReason: function () {

        var params = [];
        params["ScheduleReasonId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schwaitlistdetail";
        LoadActionPan('Admin_ScheduleReason', params);

    },

    FillScheduleReason: function (ScheduleReasonId, ShortName, Duration, event) {

        if (event != null) {
            event.stopPropagation();
        }

        UnloadActionPan("schwaitlistdetail");

        $('#schwaitlistdetail #txtSchReason').val(ShortName);
        $('#schwaitlistdetail #hfSchReasonId').val(ScheduleReasonId);
        $('#frmSchedulingWaitListDetail').bootstrapValidator('revalidateField', 'Reason');

    },

    InitilizeScheduler: function (schDate) {
        var ele = $("#waitListScheduler");
        var schedulerData = ele.data("kendoScheduler");
        if (schedulerData) {
            schedulerData.destroy();
            ele.html("");
        }
        var todayDate = schwaitlistdetail.setCustomHour("24", "0");
        todayDate.setDate(todayDate.getDate() - 1);

        schwaitlistdetail.CanScheduler = $("#waitListScheduler").kendoScheduler({
            //resources: PMSScheduler.InitializeResources(),
            date: schDate,
            dateHeaderTemplate: kendo.template("<strong>#=kendo.toString(date, 'd')#</strong>"),
            currentTimeMarker: {
                updateInterval: 100
            },
            minorTickCount: 1,
            minorTick: PMSScheduler.getCurrentTick($("#appointmentDurationselectWaitlist").val()),
            majorTick: PMSScheduler.getCurrentTick($("#appointmentDurationselectWaitlist").val()),
            allDaySlot: false,
            startTime: todayDate,
            height: 690,
            navigate: function (e) {
                console.log("navigate", e.date);
                $("#waitListScheduler").data("kendoScheduler").dataSource.read();
            },
            views: [
                 { type: "day", selected: true }, ],
            editable: {
                template: $("#customEditorTemplate").html(),
                destroy: false
            },
            dataBound: function () {
                $('table.k-scheduler-dayview .k-scheduler-header-wrap tr:first').next().hide();
                $('table.k-scheduler-dayview div.k-scheduler-times table.k-scheduler-table tr:first').next().hide();
                $('.k-scheduler-header-wrap .k-scheduler-table tr:first th').addClass('text-center bg-primary');
                $('.k-scheduler-header-wrap .k-scheduler-table tr:first th').removeClass('k-today');
                $('#waitListScheduler ').children('div:first').children('a').remove();

                setTimeout(function () {

                    $('.k-si-close').removeClass('k-icon');
                    $('.k-si-close').addClass('fa fa-times');

                }, 500);
            },
            eventTemplate: $("#event-template-waitlist ").html(),
            dataSource: {
                transport: {
                    read: function (e) {
                        schwaitlistdetail.SchedulerDefaultEvent = e;
                        schwaitlistdetail.LoadScheduler(e);
                    },
                    create: function (e) {
                        schwaitlistdetail.SearchScheduleProviderDayView().done(function (response) {
                            if (response.status != false) {
                                e.data = response.ProviderScheduleFill_JSON;
                            }
                        });
                    },
                },
            },

            schema: {
                model: {
                    id: "AppointmentId",
                    fields: {
                        AppointmentId: { type: "number" },

                        start: {
                            type: "date", field: "start"
                        },
                        end: {
                            type: "date", field: "end"
                        },
                        FacilityColor: {
                            field: "FacilityColor", defaultValue: "#ff8f50 ",
                        },
                        PatientId: {
                            field: "PatientId", defaultValue: "0",
                        },
                        PatientType: {
                            field: "PatientType"
                        },
                        PatientName: {
                            field: "PatientName"
                        },
                        Comments: {
                            field: "Comments"
                        },
                        ResonComments: {
                            field: "ResonComments"
                        },
                        VisitType: {
                            field: "VisitType"
                        },
                        AppointmentStatus: {
                            field: "AppointmentStatus"
                        },
                        ProviderId: {
                            field: "ProviderId"
                        },
                        FacilityId: {
                            field: "FacilityId"
                        },
                        StatusColor: {
                            field: "StatusColor", defaultValue: "transparent",
                        },
                        CopayBal: {
                            field: "CopayBal", defaultValue: "0",
                        },
                        AmtCopay: {
                            field: "AmtCopay", defaultValue: "0",
                        },
                        CopayClass: {
                            field: "CopayClass", defaultValue: "Black",
                        },
                        EligibilityStatus: {
                            from: "EligibilityStatus", defaultValue: "",
                        }

                    },
                }
            },
            add: function (e) {
                e.preventDefault();
                var monutes = PMSScheduler.diff_minutes(e.event.start, e.event.end);
                var providerId = $("#schwaitlistdetail #hfProvider").val();
                var providerName = $("#schwaitlistdetail #txtProvider").val();
                var facilityId = $("#schwaitlistdetail #hfFacility").val();
                var facilityName = $("#schwaitlistdetail #txtFacility").val();
                var resouceId = $("#schwaitlistdetail #hfResource").val();
                var resouceName = $("#schwaitlistdetail #txtResource").val();

                schwaitlistdetail.OpenAppointmentDetail(null, null, providerId, providerName, resouceName, resouceId, facilityId, facilityName, null, null, null, null, null, monutes, PMSScheduler.formatAMPM(e.event.start), PMSScheduler.formatAMPM(e.event.end), moment(e.event.start).format("MM/DD/YYYY"));
            },
            edit: function (e) {
                e.preventDefault();
                //var mode = "";
                //if (e.event.AppointmentId > 0) {
                //    mode = "Edit"
                //    Scheduling_Calendar.AppointmentAddNew(mode, e.event.AppointmentId, e.event.ProviderId, e.event.ProviderName, null, null, e.event.FacilityId, e.event.FacilityName, null, e.event.TimeFrom, e.event.TimeTo, null, null, e.event.AppointmentDate);
                //} else {
                //    mode = "Add";
                //    var monutes = PMSScheduler.diff_minutes(e.event.start, e.event.end);
                //    var provider_obj = $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").dataSource.data();
                //    var provider = provider_obj.filter(f => f.id == e.event.ProviderId)[0];
                //    Scheduling_Calendar.AppointmentAddNew(mode, null, e.event.ProviderId, provider.name, null, null, e.event.FacilityId, e.event.FacilityName, null, PMSScheduler.formatAMPM(e.event.start), PMSScheduler.formatAMPM(e.event.end), monutes, null, e.event.AppointmentDate);
                //}
            },
            group: {
                resources: ["providers"]
            },
        }).data("kendoScheduler");

    },

    LoadScheduler: function (e) {
        schwaitlistdetail.LoadScheduler_DBCall().done(function (response) {
            if (response.status != false) {
                $("#divAppointmentDurationselectWaitlist").removeClass("disabled");
                e.success(response.ProviderScheduleFill_JSON);
                var names = $('#schwaitlistdetail .k-scheduler-header-wrap th');
                if (response.ProviderScheduleFill_JSON.length > 0) {

                    var providerName = response.ProviderScheduleFill_JSON[0].ProviderName;
                    var count = response.ProviderScheduleFill_JSON.length;

                    if (count && count > 0) {
                        $(names).html(providerName + " <span class='badge style='color:white;background: #D2312D;'>" + count + "</span>");
                    }
                }
                else {
                    var providerName;
                    if ($('#rdProvider').is(':checked')) {
                        providerName = $('#frmSchedulingWaitListDetail #txtProvider').val();
                    } else {
                        providerName = $('#frmSchedulingWaitListDetail #txtResource').val();
                    }
                    $(names).html(providerName + " <span class='badge style='color:white;background: #D2312D;'></span>");

                }
                $('#waitListScheduler .k-scheduler-toolbar .k-nav-current').css("margin-left", "32vw");
            }
        });
    },
    LoadScheduler_DBCall: function () {

        var self = $("#schwaitlistdetail");
        var myJSON = JSON.parse(self.getMyJSON());

        var objData = new Object();
        if (myJSON.rdResource) {
            objData["ResourceId"] = myJSON.hfResource;
        }
        if (myJSON.rdProvider) {
            objData["ProviderId"] = myJSON.hfProvider;
        }
        objData["FacilityId"] = myJSON.hfFacility;
        objData["TimeFrom"] = $("#fromDate").val();
        objData["TimeTo"] = $("#toDate").val();
        if (myJSON.rdPrefDate) {
            objData["PreferredDate"] = $("#preferredDate").val();
        }
        objData["IsProvider"] = myJSON.rdProvider;

        objData["CommandType"] = "SEARCH_WAITLIST_SCHEDULE";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");
    },
    setCustomHour: function (hour, minut) {
        var datt = new Date();
        datt.setHours(hour);
        datt.setMinutes(minut);
        datt.setSeconds(0);
        datt.setMilliseconds(0);
        return datt;
    },
}