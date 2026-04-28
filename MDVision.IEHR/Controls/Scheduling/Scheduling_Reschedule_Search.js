Scheduling_RescheduleSearch = {
    bIsFirstLoad: true,
    params: [],
    SchedulerMinorTickCount: 4,
    Load: function (params) {
        Scheduling_RescheduleSearch.params = params;

        var self = $('#pnlRescheduleSearch');


        if (Scheduling_RescheduleSearch.params.appointmenttModel.GroupType == "Resource") {
            $("#pnlRescheduleSearch #frmRescheduleSearch #divProvider").css("display", "none");
            $("#pnlRescheduleSearch #frmRescheduleSearch #divResource").css("display", "");
            $("#pnlRescheduleSearch #frmRescheduleSearch #txtResource").val(Scheduling_RescheduleSearch.params.ProviderName);
            $('#frmRescheduleSearch #divResource').removeClass('disableAll');
            $("#pnlRescheduleSearch #frmRescheduleSearch #hfResource").val(Scheduling_RescheduleSearch.params.ProviderId);
            CacheManager.BindCodes('GetResources', false).done(function (result) {
                $("#frmRescheduleSearch #txtResource").autocomplete({
                    autoFocus: true,
                    source: Resources, // pass an array
                    select: function (event, ui) {

                        setTimeout(function () {
                            $("#frmRescheduleSearch #hfResource").val(ui.item.id);

                        }, 100);
                    }
                });
            });
        } else {
            $("#pnlRescheduleSearch #frmRescheduleSearch #divProvider").css("display", "");
            $("#pnlRescheduleSearch #frmRescheduleSearch #divResource").css("display", "none");
            $("#pnlRescheduleSearch #frmRescheduleSearch #txtProvider").val(Scheduling_RescheduleSearch.params.ProviderName);
            $('#frmRescheduleSearch #divProvider').removeClass('disableAll');
            $("#pnlRescheduleSearch #frmRescheduleSearch #hfProvider").val(Scheduling_RescheduleSearch.params.ProviderId);
            CacheManager.BindCodes('GetProvider', false).done(function (result) {
                $('#frmRescheduleSearch #txtProvider').autocomplete({
                    autoFocus: true,
                    source: Providers, // pass an array
                    select: function (event, ui) {

                        setTimeout(function () {
                            $("#frmRescheduleSearch #hfProvider").val(ui.item.id);

                        }, 100);
                    }
                });
            });

        }

        $("#pnlRescheduleSearch #frmRescheduleSearch #hfFacilityId").val(Scheduling_RescheduleSearch.params.FacilityId);
        $("#pnlRescheduleSearch #frmRescheduleSearch #txtSearchFacility").val(Scheduling_RescheduleSearch.params.FacilityName);

        $('#frmRescheduleSearch #dpfromDate').on('change', function () {
            if ($('#frmRescheduleSearch').data('bootstrapValidator') != null && typeof $('#frmRescheduleSearch').data('bootstrapValidator') != 'undefined') {
                $('#frmRescheduleSearch').bootstrapValidator('revalidateField', 'fromDate');
            }
        });
        if (Scheduling_RescheduleSearch.params.DayDate)
            $('#pnlRescheduleSearch #frmRescheduleSearch #dpfromDate').datepicker("setDate", Clinical_Notes.getdatetime(new Date(Scheduling_RescheduleSearch.params.DayDate), false));
        else
            $('#pnlRescheduleSearch #frmRescheduleSearch #dpfromDate').datepicker("setDate", new Date());


        Scheduling_RescheduleSearch.ValidateRecheduleSearch();

        var AppointmentDuration = [
                                   { id: 12, name: '5 mint', color: '' },
                                   { id: 6, name: '10 mint', color: '' },
                                   { id: 4, name: '15 mint', color: '' },
                                   { id: 3, name: '20 mint', color: '' },
                                   { id: 2, name: '30 mint', color: '' },
                                   { id: 1, name: '60 mint', color: '' },
        ];

        $("#frmRescheduleSearch #appointmentDurationselectResch").kendoDropDownList({
            dataSource: AppointmentDuration,
            dataTextField: "name",
            dataValueField: "id",
            index: 0,
            change: function (e) {
                var widget = $("#forRescheduler").data("kendoScheduler");
                var provider = $('#pnlRescheduleSearch .k-scheduler-header-wrap th').html();
                if (widget) {
                    widget.setOptions({
                        minorTickCount: 1,
                        minorTick: PMSScheduler.getCurrentTick(this.dataItem().id),
                        majorTick: PMSScheduler.getCurrentTick(this.dataItem().id)
                    });
                    widget.view(widget.view().name);
                }
                $('#pnlRescheduleSearch .k-scheduler-header-wrap th').html(provider);
            },
        });
        Scheduling_RescheduleSearch.SetSchedulerMinorTickCount();

    },

    LoadSchBlockHours: function () {
        var scheduler = $("#forRescheduler").data("kendoScheduler");
        if (scheduler && scheduler._selectedViewName && scheduler._selectedViewName != "month") {
            Scheduling_RescheduleSearch.LoadSchBlockHours_DBCall(scheduler._selectedViewName, scheduler).done(function (response) {
                if (response.status != false) {
                    if (response.BlockHoursCount > 0) {
                        PMSScheduler.ResetAllSlotSlotsColor(scheduler);
                        var BlockHoursLoadJSONData = JSON.parse(response.BlockHoursLoad_JSON);
                        //var resourceArry = scheduler._data;
                        $.each(BlockHoursLoadJSONData, function (ind, item) {
                            var contentDiv = scheduler.element.find("div.k-scheduler-content");
                            var rows = contentDiv.find("tr");
                            if (scheduler._selectedViewName == "day") {
                                for (var i = 0; i < rows.length; i++) {
                                    var slot = scheduler.slotByElement(rows[i]);
                                    Scheduling_RescheduleSearch.SetSlotColor(slot, BlockHoursLoadJSONData[0].ProviderId, item);
                                };
                            }
                            else {
                                for (var i = 0; i < rows.length; i++) {
                                    var tds = $(rows[i]).find('td');
                                    for (var s = 0; s < tds.length; s++) {
                                        var slot = scheduler.slotByElement(tds[s]);
                                        Scheduling_RescheduleSearch.SetSlotColor(slot, BlockHoursLoadJSONData[0].ProviderId, item);
                                    }
                                };
                            }
                        });
                    }
                    else {
                        PMSScheduler.ResetAllSlotSlotsColor(scheduler);
                    }
                }
            });
        }
    },

    SetSlotColor: function (slot, ProviderId, item) {
        if (slot) {
            var blockFrom = kendo.toString(new Date(item.SchDateFrom), "s");
            var blockTo = kendo.toString(new Date(item.SchDateTo), "s");
            var slotStart = kendo.toString(slot.startDate, "s");
            if (Date.parse(slotStart) >= Date.parse(blockFrom) && Date.parse(slotStart) < Date.parse(blockTo) && ProviderId == item.ProviderId) {
                $(slot.element).css('background-color', item.Color);
                $(slot.element).css('color', "White");
                var description = "";

                if (item.Description && item.Description.length > 170) {
                    description = item.Description.substring(0, 170) + "..";
                } else {
                    description = item.Description;
                }

                $(slot.element).attr('title', item.Description);
                $(slot.element).text("Blocked:-" + description);
                $(slot.element).attr('isBlock', "true");
            }
            else if (!$(slot.element).attr('isBlock')) {
                $(slot.element).css('background-color', "");
                $(slot.element).css('color', "");
                $(slot.element).text("");
                $(slot.element).removeAttr('isBlock');
            }
        }
    },

    LoadSchBlockHours_DBCall: function (ViewTypeName, scheduler) {
        var objData = new Object();
        var ProviderIds = $("#pnlRescheduleSearch #hfProvider").val();
        var ResourceIds = $("#pnlRescheduleSearch #hfResource").val();
        objData["ProviderIds"] = ProviderIds;
        objData["ResourceIds"] = ResourceIds;
        var StartDate = kendo.toString(new Date(), "d");


        StartDate = kendo.toString(scheduler.date(), "d");

        objData["start"] = StartDate;
        objData["SchViewType"] = ViewTypeName;
        objData["CommandType"] = "Load_Sch_BlockHours";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");
    },

    toDateRequired: function () {


        var formValidation = $('#pnlRescheduleSearch #frmRescheduleSearch').data("bootstrapValidator");
        if ($('#pnlRescheduleSearch #frmRescheduleSearch').data('bootstrapValidator') != null && typeof $('#pnlRescheduleSearch #frmRescheduleSearch').data('bootstrapValidator') != 'undefined') {
            if ($('#pnlRescheduleSearch #frmRescheduleSearch #dpfromDate').val() != "") {
                formValidation.enableFieldValidators('toDate', true);
            }
            else {
                formValidation.enableFieldValidators('toDate', false);
            }
        }
    },

    ValidateRecheduleSearch: function () {
        $('#pnlRescheduleSearch #frmRescheduleSearch')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },

              fields: {
                  fromDate: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
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
                  Facility: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Resource: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Scheduling_RescheduleSearch.ScheduleSearch();
       });
    },

    //==== Start Search Function ====== \\

    ScheduleSearch: function (PageNo, rpp) {

        var schDate = $("#frmRescheduleSearch #dpfromDate").val();
        Scheduling_RescheduleSearch.InitilizeScheduler(schDate);
    },

    SearchBatchSchedule: function (SchedulingSearchData, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var data = "SchedulingSearchData=" + SchedulingSearchData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "SCHEDULING_BLOCK_UNBLOCK", "RESCHEDULE_SEARCH");
    },

    //==== End Search Function ======== \\

    //==== Start Grid Load ==== \\


    //==== Ebd Grid Load ====\\

    UnLoad: function () {

        UnloadActionPan(Scheduling_RescheduleSearch.params["ParentCtrl"], "pnlRescheduleSearch");
        PMSScheduler.CanScheduler.dataSource.read();
        //Scheduling_Calendar.LoadScheduleFill();
    },

    //==== Open Appointment Screen ==== \\
    OpenAppointmentDetail: function (ProviderId, Provider, Resources, ResourceId, FacilityId, Facility, SlotMinutes, FromTime, ToTime, appointmentId, scheduleDate, isNoteCreated, isResource) {


        AppPrivileges.GetFormPrivileges("Appointment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["ProviderId"] = ProviderId;
                params["ProviderName"] = Provider;
                params["ResourceId"] = ResourceId;
                params["ResourceName"] = Resources;
                params["FacilityId"] = $('#pnlRescheduleSearch #frmRescheduleSearch #hfFacilityId').val();
                params["FacilityName"] = $('#pnlRescheduleSearch #frmRescheduleSearch #txtSearchFacility').val();
                params["ScheduleDate"] = scheduleDate;
                params["Time"] = FromTime;
                params["ToTime"] = ToTime;
                params["PatientId"] = $("#Scheduling_RescheduleSearch #hfPatientid").val();
                params["DateId"] = Scheduling_RescheduleSearch.params.DateId;
                params["isNoteCreated"] = isNoteCreated;
                params["AppointmentId"] = appointmentId;
                params["mode"] = "Edit";
                params["isResource"] = isResource;
                params["ParentCtrl"] = "Scheduling_RescheduleSearch";
                LoadActionPan('appointmentDetail', params);


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    OpenFacility: function () {
        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefHiddenIdCtrl"] = "hfFacilityId";
        params["RefCtrl"] = "txtSearchFacility";
        params["ParentCtrl"] = "Scheduling_RescheduleSearch";
        LoadActionPan('Admin_Facility', params);
    },
    BindFacility: function () {

        var shortName = $("#" + "pnlRescheduleSearch #frmRescheduleSearch #txtSearchFacility").val();
        utility.GetFacilityArray(shortName).done(function (response) {

            $("#pnlRescheduleSearch #frmRescheduleSearch #txtSearchFacility").autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#" + "pnlRescheduleSearch #frmRescheduleSearch #txtSearchFacility").val(ui.item.value);
                        $("#" + "pnlRescheduleSearch #frmRescheduleSearch #hfFacilityId").val(ui.item.id);
                        if ($("#" + "pnlRescheduleSearch #frmRescheduleSearch #lnkFacility").css("display") == "none") {
                            $("#" + "pnlRescheduleSearch #frmRescheduleSearch #lnkFacility").css("display", "inline");

                        }

                    }, 100);

                }
            });
            //$("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtFacility").autocomplete("search");
        });
    },
    OpenFacilityDetail: function () {

        var params = [];
        params["FacilityId"] = $('#' + 'pnlRescheduleSearch #frmRescheduleSearch #hfFacility').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtSearchFacility";
        params["ParentCtrl"] = 'Scheduling_RescheduleSearch';
        LoadActionPan('facilityDetail', params);
    },
    ForceBookingAppointment: function () {


        var params = {};

        params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        params["ProviderName"] = $('#pnlScheduleCalendar #Provider option:selected').text();
        params["FacilityId"] = $('#pnlRescheduleSearch #frmRescheduleSearch #hfFacilityId').val();
        params["FacilityName"] = $('#pnlRescheduleSearch #frmRescheduleSearch #txtSearchFacility').val();
        params["ParentCtrl"] = 'Scheduling_RescheduleSearch';
        params["isSaveReceiptDoc"] = false;
        params["mode"] = "Add";
        params["SlotMinutes"] = 15;
        params["DayDate"] = $('#pnlScheduleCalendar #daydate span').html();
        params["PatientId"] = Scheduling_RescheduleSearch.params.PatientId;
        LoadActionPan('Scheduling_Force_Booking', params);

    },

    InitilizeScheduler: function (schDate) {
        var ele = $("#forRescheduler");
        var schedulerData = ele.data("kendoScheduler");
        if (schedulerData) {
            schedulerData.destroy();
            ele.html("");
        }
        var todayDate = Scheduling_RescheduleSearch.setCustomHour("24", "0");
        todayDate.setDate(todayDate.getDate() - 1);

        Scheduling_RescheduleSearch.CanScheduler = $("#forRescheduler").kendoScheduler({

            date: schDate,
            dateHeaderTemplate: kendo.template("<strong>#=kendo.toString(date, 'd')#</strong>"),
            currentTimeMarker: {
                updateInterval: 100
            },
            minorTickCount: 1,
            minorTick: PMSScheduler.getCurrentTick(Scheduling_RescheduleSearch.SchedulerMinorTickCount),
            majorTick: PMSScheduler.getCurrentTick(Scheduling_RescheduleSearch.SchedulerMinorTickCount),
            allDaySlot: false,
            startTime: todayDate,
            height: 690,
            navigate: function (e) {
                console.log("navigate", e.date);
                $("#forRescheduler").data("kendoScheduler").dataSource.read()
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
                $('.k-scheduler-header-wrap .k-scheduler-table tr:first th').css('background-color', '#0088cc');
                $('#forRescheduler .k-scheduler-toolbar').addClass('hidden');
                Scheduling_RescheduleSearch.LoadSchBlockHours();
                setTimeout(function () {
                    $('.k-si-close').removeClass('k-icon');
                    $('.k-si-close').addClass('fa fa-times');
                }, 500);
            },
            eventTemplate: $("#event-template-waitlist ").html(),
            dataSource: {
                transport: {
                    read: function (e) {
                        Scheduling_RescheduleSearch.SchedulerDefaultEvent = e;
                        Scheduling_RescheduleSearch.LoadScheduler(e);
                    },
                    create: function (e) {
                        Scheduling_RescheduleSearch.SearchScheduleProviderDayView().done(function (response) {
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
                if (Scheduling_RescheduleSearch.params.appointmenttModel.GroupType == "Resource") {
                    var minutes = Scheduling_RescheduleSearch.diff_minutes(e.event.start, e.event.end);
                    var resourceId = $("#frmRescheduleSearch #hfResource").val();
                    var resourceName = $("#frmRescheduleSearch #txtResource").val();
                    var model = Scheduling_RescheduleSearch.params.appointmenttModel;
                    Scheduling_RescheduleSearch.OpenAppointmentDetail(null, null, resourceName, resourceId, model.FacilityId, model.FacilityName, minutes, Scheduling_RescheduleSearch.formatAMPM(e.event.start), Scheduling_RescheduleSearch.formatAMPM(e.event.end), model.AppointmentId, moment(e.event.start).format("MM/DD/YYYY"), model.isNoteCreated, true);
                }
                else {
                    var minutes = Scheduling_RescheduleSearch.diff_minutes(e.event.start, e.event.end);
                    var providerId = $("#frmRescheduleSearch #hfProvider").val();
                    var providerName = $("#frmRescheduleSearch #txtProvider").val();
                    var model = Scheduling_RescheduleSearch.params.appointmenttModel;
                    Scheduling_RescheduleSearch.OpenAppointmentDetail(providerId, providerName, null, null, model.FacilityId, model.FacilityName, minutes, Scheduling_RescheduleSearch.formatAMPM(e.event.start), Scheduling_RescheduleSearch.formatAMPM(e.event.end), model.AppointmentId, moment(e.event.start).format("MM/DD/YYYY"), model.isNoteCreated, false);
                }
            },
            edit: function (e) {
                e.preventDefault();
            },
            group: {
                resources: ["providers"]
            },
        }).data("kendoScheduler");

    },
    SetSchedulerMinorTickCount: function () {
        var dropdownlistDuration = $("#frmRescheduleSearch #appointmentDurationselectResch").data("kendoDropDownList");
        if (dropdownlistDuration) {
            dropdownlistDuration.value(Scheduling_RescheduleSearch.SchedulerMinorTickCount);
        }
    },
    LoadScheduler: function (e) {
        var viewType = "day";
        Scheduling_RescheduleSearch.LoadScheduler_DBCall().done(function (response) {
            if (response.status != false) {
                e.success(response.ProviderScheduleFill_JSON);
                var names = $('#pnlRescheduleSearch .k-scheduler-header-wrap th');
                if (response.ProviderScheduleFill_JSON.length > 0) {

                    var providerName = response.ProviderScheduleFill_JSON[0].ProviderName;
                    var count = response.ProviderScheduleFill_JSON.length;

                    if (count && count > 0) {
                        $(names).html(providerName + " <span class='badge style='color:white;background: #D2312D;'>" + count + "</span>");
                    }
                }
                else {
                    var providerName;
                    if (Scheduling_RescheduleSearch.params.appointmenttModel.GroupType == "Resource") {
                        providerName = $('#pnlReschedule_Search #txtResource').val();
                    } else {
                        providerName = $('#pnlReschedule_Search #txtProvider').val();
                    }
                    $(names).html(providerName + " <span class='badge style='color:white;background: #D2312D;'></span>");
                }
            }
        });
    },
    LoadScheduler_DBCall: function () {

        var self = $("#pnlRescheduleSearch");
        var myJSON = JSON.parse(self.getMyJSON());

        var objData = new Object();

        if (Scheduling_RescheduleSearch.params.appointmenttModel.GroupType == "Resource") {
            objData["ResourceId"] = myJSON.hfResource;
            objData["IsProvider"] = false;
        } else {
            objData["ProviderId"] = myJSON.hfProvider;
            objData["IsProvider"] = true;
        }
        objData["FacilityId"] = myJSON.hfFacilityId;
        objData["TimeFrom"] = $("#pnlRescheduleSearch #dpfromDate").val();

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
    diff_minutes: function (dt2, dt1) {

        var diff = (dt2.getTime() - dt1.getTime()) / 1000;
        diff /= 60;
        return Math.abs(Math.round(diff));

    },
    formatAMPM: function (date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return strTime;
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmRescheduleSearch";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Scheduling_RescheduleSearch";
        LoadActionPan('Admin_Provider', params);
    },

    OpenResource: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = "frmRescheduleSearch";
        params["ResourceId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Scheduling_RescheduleSearch";
        LoadActionPan('Admin_Resources', params);
    },
}