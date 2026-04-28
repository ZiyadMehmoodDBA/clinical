PMSScheduler = {
    bIsFirstLoad: true,
    params: [],
    ActionBit: false,
    PatientAddbit: true,
    currentPanelID: "",
    IsRecentPatientSelected: "",
    CanScheduler: null,
    NavigationEvent: null,
    NextDate: null,
    IsResourceSch: false,
    PreviousViewType: "day",
    CopiedEventHolder: null,
    CutEventHolder: null,
    SchEventHolder: null,
    Counter: 0,
    MonthViewCount: 0,
    currentEvent: "",
    DayViewCount: 0,
    SchedulerMinorTickCount: 4,
    contextMenuCounter: 0,
    providerDataSource: [],
    facilityDataSource: [],
    scheduleStatusDataSource: [],
    ZoomINCount: 0,
    ScrollPosition: 0,
    backupArrayProviderIds: [],
    navigationJSON: null,
    WorkWeekSchedulesJSON: null,
    gblockHoursCount: null,
    gblockHoursJSON: null,
    SchdefObject: null,
    SchedulerHeight: 0,
    SchedulerTooltip: null,
    Load: function (params) {

        PMSScheduler.params = params;
        PMSScheduler.DomReadyFunction();
        if (PMSScheduler.bIsFirstLoad == true) {
            PMSScheduler.bIsFirstLoad = false;
            PMSScheduler.Counter = 1;
            PMSScheduler.ZoomINCount = 0;
            PMSScheduler.InitilizeSearchFilters().done(function () {
                PMSScheduler.SetSchedulerMinorTickCount();
                PMSScheduler.InitilizeScheduler();
            });
        } else {
            PMSScheduler.RefreshScheduler();
        }
    },
    DomReadyFunction: function () {
        $(function () {
            $('#menu-toggle').click(function () {
                var checked = $.map($("#schedulerFacility :checked"), function (checkbox) {
                    return parseInt($(checkbox).val());
                });
                setTimeout(function () { PMSScheduler.UpdateFacility(checked); }, 500);
            });

            $("#schedulerFacility :checkbox").change(function (e) {
                var checked = $.map($("#schedulerFacility :checked"), function (checkbox) {
                    return parseInt($(checkbox).val());
                });
                PMSScheduler.UpdateFacility(checked);
            });

            $("#menu-toggle").click(function (e) {
                e.preventDefault();
                $("#wrapper").toggleClass("toggled");
                $("#tablediv").resize(function (e) {

                    $('#StickyCalButton').css('width', $("#myTable").css('width'));
                    $('#StickyCalButton').css('width', $("#monthTable").css('width'));
                    $('#MonthCal').css('width', $("#monthTable").css('width'));

                    $('#StickyCalButton').css('width', $("#weekcontainer").css('width'));
                    $('#WeekCal').css('width', $("#weekcontainer").css('width'));

                });
            });
        });
        PMSScheduler.SchedulerGetMinorTickCount();

    },

    SetCurrentPosition: function () {
        setTimeout(function () {
            $('.k-scheduler-content').animate({
                scrollTop: PMSScheduler.ScrollPosition
            }, 100);
        }, 300);
    },

    SetToDayFocus: function (date, operationId, view, preView) {
        var currentDate = new Date();
        currentDate = currentDate.setHours(0, 0, 0, 0);
        var mydate = new Date();
        if (date != null && typeof date != "undefined") {
            mydate = date.setHours(0, 0, 0, 0);
        }
        if (view != null && typeof view != "undefined" && view == "day") {
            if (date == null || typeof date == "undefined") {
                if (preView == "day") {
                    PMSScheduler.ScrollPosition = $('.k-scheduler-content').scrollTop();
                }
            } else {
                if (mydate == currentDate) {
                    if (operationId == 1) {
                        if (preView == "day") {
                            PMSScheduler.ScrollPosition = $('.k-scheduler-content').scrollTop();
                        }
                    } else {
                        PMSScheduler.SetCurrentPosition();
                    }
                }
            }
        } else if ((view != null && typeof view != "undefined" && view != "day") && preView == "day") {
            if (mydate == currentDate) {
                if (operationId == 1) {
                    if (preView == "day") {
                        PMSScheduler.ScrollPosition = $('.k-scheduler-content').scrollTop();
                    }
                }
            }
        }
    },
    SetSchedulerMinorTickCount: function () {
        var dropdownlistDuration = $("#pnlPMSScheduler #appointmentDurationselect").data("kendoDropDownList");
        if (dropdownlistDuration) {
            dropdownlistDuration.value(PMSScheduler.SchedulerMinorTickCount);
        }

    },

    SetSchedulerMinorTick: function () {
        if ($("#pnlPMSScheduler #appointmentDurationselect").length > 0) {
            PMSScheduler.SetSchedulerMinorTickCount();
            var widget = $("#scheduler").data("kendoScheduler");
            if (widget) {
                widget.setOptions({
                    minorTickCount: PMSScheduler.SchedulerMinorTickCount
                });

                widget.view(widget.view().name);
            }

        }
    },

    SetSchedulerMinorMajorTick: function () {
        if ($("#pnlPMSScheduler #appointmentDurationselect").length > 0) {
            PMSScheduler.SetSchedulerMinorTickCount();

            var widget = $("#scheduler").data("kendoScheduler");
            if (widget) {
                widget.setOptions({
                    minorTickCount: 1,
                    minorTick: PMSScheduler.getCurrentTick(PMSScheduler.SchedulerMinorTickCount),
                    majorTick: PMSScheduler.getCurrentTick(PMSScheduler.SchedulerMinorTickCount)
                });

                widget.view(widget.view().name);
            }

        }
        PMSScheduler.ResetAppointmentCount();
    },

    SchedulerGetMinorTickCount: function () {
        if (globalAppdata.SchedulerTimeInterval && globalAppdata.SchedulerTimeInterval != "" && globalAppdata.SchedulerTimeInterval != "0") {

            if (globalAppdata.SchedulerTimeInterval == "5") {
                PMSScheduler.SchedulerMinorTickCount = 12;
            }
            else if (globalAppdata.SchedulerTimeInterval == "10") {
                PMSScheduler.SchedulerMinorTickCount = 6;
            }
            else if (globalAppdata.SchedulerTimeInterval == "15") {
                PMSScheduler.SchedulerMinorTickCount = 4;
            }
            else if (globalAppdata.SchedulerTimeInterval == "20") {
                PMSScheduler.SchedulerMinorTickCount = 3;
            }
            else if (globalAppdata.SchedulerTimeInterval == "30") {
                PMSScheduler.SchedulerMinorTickCount = 2;
            }
            else if (globalAppdata.SchedulerTimeInterval == "60") {
                PMSScheduler.SchedulerMinorTickCount = 1;
            } else {
                PMSScheduler.SchedulerMinorTickCount = 4;
            }

        } else {
            PMSScheduler.SchedulerMinorTickCount = 4;
        }
    },

    LoadBlockHours: function () {
        var params = [];
        params["ParentCtrl"] = "mstrTabSchedule";
        params["FromAdmin"] = "0";
        LoadActionPan('Admin_BlockHours', params);
    },

    fitSchedulerWidget: function () {
        if ((PMSScheduler.NavigationEvent && PMSScheduler.NavigationEvent.view == "month") || PMSScheduler.PreviousViewType == "month") {
            var widget = $("#scheduler").data("kendoScheduler");
            //size widget to take the whole view
            widget.element.height("1000");
            widget.resize(true);
        }
    },

    fitSchedulerWidgetHeight: function () {
        if (PMSScheduler.NavigationEvent && (PMSScheduler.NavigationEvent.view == "week" || PMSScheduler.NavigationEvent.view == "workWeek")) {
            var widget = $("#scheduler").data("kendoScheduler");
            //size widget to take the whole view
            widget.element.height(PMSScheduler.SchedulerHeight);
            widget.resize(true);
        }
    },

    GetSchedulerHeight: function () {
        if (!PMSScheduler.NavigationEvent || PMSScheduler.NavigationEvent.view != "month") {
            var widget = $("#scheduler").data("kendoScheduler");
            PMSScheduler.SchedulerHeight = widget.element.height();
        }
    },


    InitilizeAppointmentToolTipMonth: function () {
        $("#scheduler").kendoTooltip({
            filter: ".k-more-events",
            position: "top",
            show: PMSScheduler.setMonthViewTooltip,
            width: 212,
            //height: 450,
            autoHide: true,
            content: kendo.template($('#tooltipTemplateMonthView').html()),
        });
    },

    setMonthViewTooltip: function (e) {
        $(this.content).parent().css("background-color", "#E6EEF7");
        $('.k-callout.k-callout-s').addClass('set-Tooltip-Position');
    },

    LoadScheduler_DBCall: function (viewType) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        var ProviderIds = $("#pnlPMSScheduler #hfProviderIds").val();
        var ResourceIds = $("#pnlPMSScheduler #hfResourceIds").val();
        var FacilityIds = $("#pnlPMSScheduler #hfFacilityIds").val();
        var PatientTypeId = $("#pnlPMSScheduler #hfPatientTypeId").val();
        var VisitTypeId = $("#pnlPMSScheduler #hfVisitTypeIds").val();
        var StartDate = kendo.toString(new Date(), "d");

        if (PMSScheduler.NavigationEvent) {
            StartDate = kendo.toString(PMSScheduler.NavigationEvent.date, "d");
        }
        else if (scheduler && scheduler.date())
            StartDate = kendo.toString(scheduler.date(), "d");

        var AppointmentStatusIds = $("#pnlPMSScheduler #hfAppointmentStatusIds").val();

        var objData = new Object();
        objData["ProviderIds"] = ProviderIds;
        objData["ResourceIds"] = ResourceIds;
        objData["FacilityIds"] = FacilityIds;
        objData["PatientTypeId"] = PatientTypeId ? PatientTypeId : 0;
        objData["VisitTypeIds"] = VisitTypeId;
        objData["StartDate"] = StartDate;
        objData["AppointmentStatusIds"] = AppointmentStatusIds;
        objData["IsResourceSch"] = PMSScheduler.IsResourceSch;
        objData["SchViewType"] = viewType;
        if (PMSScheduler.NextDate) {
            objData["Month"] = PMSScheduler.NextDate.getMonth() + 1;
            objData["Year"] = PMSScheduler.NextDate.getFullYear();
        }
        else {
            objData["Month"] = 0;
            objData["Year"] = 0;
        }


        objData["CommandType"] = "Search_Appointment_Schdule";
        if (globalAppdata["WeekWorkDaysIds"] && globalAppdata["WeekWorkDaysIds"].indexOf('1') > -1) {
            objData["WorkWeekDays"] = globalAppdata["WeekWorkDaysIds"];
        }
        else {
            objData["WorkWeekDays"] = "";
        }

        var data = JSON.stringify(objData);
        var response = MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");

        if (response.ProviderScheduleFill_JSON && response.ProviderScheduleFill_JSON.length > 0) {
            $.each(response.ProviderScheduleFill_JSON, function () {
                if (new Date("01/01/2007 " + new Date(this.end).toTimeString()).getTime() == new Date("01/01/2007 00:00:00").getTime()) {
                    this.end = new Date(new Date(this.end) - 1 * 60000);
                    this.TimeTo = "23:59:00";
                }
            });
        }
        return response;
    },
    getCurrentTick: function (minorMajorTick) {
        if (minorMajorTick == 1) {
            minorMajorTick = 60;
        }
        else if (minorMajorTick == 2) {
            minorMajorTick = 30;
        }
        else if (minorMajorTick == 4) {
            minorMajorTick = 15;
        }
        else if (minorMajorTick == 3) {
            minorMajorTick = 20;
        }
        else if (minorMajorTick == 6) {
            minorMajorTick = 10;
        }
        else if (minorMajorTick == 12) {
            minorMajorTick = 5;
        }
        return minorMajorTick;
    },

    GetProviderSchedule_Warper: function (viewType) {

        var deff = $.Deferred();
        var todayDate = PMSScheduler.setCustomHour("24", "0");
        todayDate.setDate(todayDate.getDate() - 1);
        var curntDate = new Date(todayDate);
        var day = PMSScheduler.addZero(curntDate.getDate());
        var month = PMSScheduler.addZero(curntDate.getMonth());
        var year = PMSScheduler.addZero(curntDate.getFullYear());
        var scheduleDate = month + "/" + day + "/" + year;
        PMSScheduler.GetProviderSchedule(viewType).done(function (response) {

            var day = PMSScheduler.addZero(curntDate.getDate());
            var month = PMSScheduler.addZero(curntDate.getMonth());
            var year = PMSScheduler.addZero(curntDate.getFullYear());
            workDayStart_ = new Date(year, month, day, 0, 0, 0);
            workDayEnd_ = new Date(year, month, day, 23, 0, 0);

            if (response.status != false) {
                navigationJSON = response.ScheduleSlotsFill_JSON;
                var scheduledHours = navigationJSON;
                if (scheduledHours != null && scheduledHours.length > 0) {
                    var sDate = scheduledHours[0].AppointmentDate;
                    sDate = utility.RemoveTimeFromDate(null, sDate);
                    var tFrom = scheduledHours[0].BeginningTime;
                    var tTo = scheduledHours[0].EndingTime;
                    if (tFrom && tTo) {
                        workDayStart_ = new Date(sDate + " " + tFrom);
                        workDayEnd_ = new Date(sDate + " " + tTo);
                    }
                }
                else {
                    workDayStart_ = new Date(kendo.toString(curntDate, "yyyy-MM-dd") + " 08:00 AM");
                    workDayEnd_ = new Date(kendo.toString(curntDate, "yyyy-MM-dd") + " 05:00 PM");
                }
            }

            deff.resolve([{ "workDayStart": workDayStart_, "workDayEnd": workDayEnd_ }]);
        });

        return deff;
    },

    InitilizeScheduler: function () {
        var ele = $("#scheduler");
        var todayDateTime = new Date();
        var todayDate = PMSScheduler.setCustomHour("24", "0");
        var viewType = PMSScheduler.PreviousViewType ? PMSScheduler.PreviousViewType : 'day';
        var schedulerData = ele.data("kendoScheduler");
        if (schedulerData) {
            schedulerData.destroy();
            ele.html("");
        }
        PMSScheduler.contextMenuCounter = 0;
        if (PMSScheduler.NextDate && PMSScheduler.NextDate != "") {
            todayDateTime = PMSScheduler.NextDate;
        }
        todayDate.setDate(todayDate.getDate() - 1);
        var curntDate = new Date(todayDate);
        var day = PMSScheduler.addZero(curntDate.getDate());
        var month = PMSScheduler.addZero(curntDate.getMonth());
        var year = PMSScheduler.addZero(curntDate.getFullYear());
        var scheduleDate = month + "/" + day + "/" + year;
        PMSScheduler.GetProviderSchedule_Warper(viewType).done(function (response) {

            workDayStart_ = response[0].workDayStart;
            workDayEnd_ = response[0].workDayEnd;

            PMSScheduler.CanScheduler = $("#scheduler").kendoScheduler({
                resources: PMSScheduler.InitializeResources(),
                date: todayDateTime,
                dateHeaderTemplate: kendo.template("<strong>#=kendo.toString(date, 'ddd MM/dd/yyyy')#</strong>"),
                currentTimeMarker: {
                    updateInterval: 100
                },
                minorTickCount: 1,
                minorTick: PMSScheduler.getCurrentTick(PMSScheduler.SchedulerMinorTickCount),
                majorTick: PMSScheduler.getCurrentTick(PMSScheduler.SchedulerMinorTickCount),
                allDaySlot: false,
                editable: {
                    resize: false
                },
                startTime: todayDate,
                workWeekStart: 0,
                workWeekEnd: 6,
                workDayStart: workDayStart_,
                workDayEnd: workDayEnd_,
                height: 'calc(100vh - 195px)',
                showWorkHours: localStorage.getItem("IsShowWorkHours") == "true" ? true : false,
                navigate: function (e) {

                    if (e.action === "changeWorkDay") {
                        //Show work / full day button is pressed
                        var which_ = e.sender.element.context.lastElementChild.innerText.trim().toLowerCase();
                        if (which_ == "show business hours")
                            localStorage.setItem("IsShowWorkHours", true);
                        else if (which_ == "show full day")
                            localStorage.setItem("IsShowWorkHours", false);
                    }
                    e.sender._workDayMode = localStorage.getItem("IsShowWorkHours") == "true" ? true : false,
                    PMSScheduler.LoadSchedulerForNavigation(e);
                    PMSScheduler.SetSchedulerView(e, true);
                },
                moveEnd: function (e) {
                    e.preventDefault();
                    if (PMSScheduler.CutEventHolder) {
                        var oldDateTime = new Date(PMSScheduler.CutEventHolder.OldStart);
                        var newDateTime = new Date(e.start);
                        if (Date.parse(newDateTime) != Date.parse(oldDateTime) || e.resources.ProviderId != PMSScheduler.CutEventHolder.ProviderId) {
                            PMSScheduler.CutEventHolder.NewStart = new Date(e.start);
                            var fromT = kendo.toString(new Date(PMSScheduler.CutEventHolder.OldStart), "dddd, MMMM dd, yyyy,") + " " + kendo.toString(new Date(PMSScheduler.CutEventHolder.OldStart), "t");
                            var toT = kendo.toString(new Date(PMSScheduler.CutEventHolder.NewStart), "dddd, MMMM dd, yyyy,") + " " + kendo.toString(new Date(PMSScheduler.CutEventHolder.NewStart), "t");
                            var confirmMsg = "Do you want to move the appointment from " + fromT + " to " + toT + "?";
                            var allowMove = true;
                            if (e.resources.ProviderId != PMSScheduler.CutEventHolder.ProviderId) {
                                var scheduler = $("#scheduler").data("kendoScheduler");
                                var resourceArry = scheduler.resources[1].dataSource.options.data;
                                var Provider = $.grep(resourceArry, function (v) {
                                    return v.value == e.resources.ProviderId;
                                });
                                if (Provider) {
                                    confirmMsg = "Do you want to move the appointment from " + PMSScheduler.CutEventHolder.ProviderName + " to " + Provider[0].text + " from " + fromT + " to " + toT + "?"
                                }
                            }
                            else if (Date.parse(newDateTime) == Date.parse(oldDateTime)) {
                                allowMove = false;
                            }
                            if (allowMove) {
                                utility.myConfirm(confirmMsg,
                                function () {
                                    PMSScheduler.CutEventHolder.start = new Date(e.start);
                                    PMSScheduler.CutEventHolder.end = new Date(e.start);
                                    PMSScheduler.CutEventHolder.NewAppointmentDate = kendo.toString(new Date(e.start), "s");
                                    PMSScheduler.CutEventHolder.ProviderId = e.resources.ProviderId;

                                    PMSScheduler.onDroped();
                                }, function () {
                                }, "Move Appointment");
                            }

                        }
                    }

                },
                views: [
                {
                    type: "day", selected: PMSScheduler.PreviousViewType == "day" ? true : false, eventTemplate: $("#event-template").html()
                },
                {
                    type: "workWeek", selected: PMSScheduler.PreviousViewType == "workWeek" ? true : false, eventTemplate: $("#event-template").html()
                },
                {
                    type: "week", selected: PMSScheduler.PreviousViewType == "week" ? true : false, eventTemplate: $("#event-template").html()
                },
                { type: "month", selected: PMSScheduler.PreviousViewType == "month" ? true : false, eventTemplate: $("#event-template-month").html(), }
                ],
                editable: {
                    destroy: false,
                },
                dataBound: function (e) {
                    $('table.k-scheduler-dayview .k-scheduler-header-wrap tr:first').next().hide();
                    $('table.k-scheduler-dayview div.k-scheduler-times table.k-scheduler-table tr:first').next().hide();
                    $('.k-scheduler-header-wrap .k-scheduler-table tr:first th').addClass('text-center bg-primary');
                    $('.k-scheduler-header-wrap .k-scheduler-table tr:last th').addClass('text-center bg-primary');
                    $('.k-scheduler-header-wrap .k-scheduler-table tr:first th').removeClass('k-today');
                    $('.k-scheduler-header-wrap .k-scheduler-table tr:last th').removeClass('k-today');
                    PMSScheduler.scheduler_dataBoundFormatedDate(e);
                    if (e.sender._selectedViewName != "day") {
                        $('.k-scheduler-toolbar .k-nav-current').css("margin-left", "20vw");
                    } else {
                        $('.k-scheduler-toolbar .k-nav-current').css("margin-left", "25vw");
                    }
                    $('p.contextmenuleftclick').click(function (evt) {
                        evt.preventDefault();
                        var contextMenu = $("#SchedulerContextMenu").data("kendoContextMenu");
                        PMSScheduler.currentEvent = $(this).closest('div.k-event.k-event-inverse');
                        if (PMSScheduler.contextMenuCounter < 1) {
                            contextMenu.open(PMSScheduler.currentEvent);
                        }
                        contextMenu.open(evt.clientX, evt.clientY);
                    });

                    var scheduler = this;
                    var customIsWorkDay = scheduler._workDayMode;
                    var curntDate = e.sender._model.formattedDate;
                    curntDate = new Date(curntDate);
                    var day = PMSScheduler.addZero(curntDate.getDate());
                    var month = PMSScheduler.addZero(curntDate.getMonth());
                    var year = PMSScheduler.addZero(curntDate.getFullYear());
                    if (e.sender.view().title == 'Month') {
                        $("#scheduler td span.totalCount").remove();
                        var dates = $.map(scheduler.dataSource._data, function (o) {
                            return new Date(o.AppointmentDate).getDate()
                        });
                        var counts = {};

                        $.each(dates, function (key, value) {
                            if (!counts.hasOwnProperty(value)) {
                                counts[value] = 1;
                            } else {
                                counts[value]++;
                            }
                        });
                        $.each(counts, function (key, value) {
                            $("#scheduler td span").filter(function () {
                                return parseInt($(this).text()) === parseInt(key);
                            }).parent().not('.k-other-month').append("<span class='totalCount pull-left text-bold' style='margin-top: -5px;'>Total: " + value + "</span>")
                        });
                    }
                    var providers = $("#pnlPMSScheduler #hfProviderIds").val();
                    var resources = $("#pnlPMSScheduler #hfResourceIds").val();
                    PMSScheduler.SetDateAlignment();
                },
                dataBinding: function (e) {
                    var scheduler = e.sender;
                    var contentDiv = scheduler.element.find("div.k-scheduler-content");
                    var rows = contentDiv.find("tr");
                    if (e.sender.viewName() == "workWeek") {

                        PMSScheduler.setCustomWorkWeekView(e.sender);
                    }

                },
                remove: function (e) {
                    e.preventDefault();
                },
                dataSource: {
                    batch: true,
                    transport: {
                        read: function (e) {
                            var scheduler = $("#scheduler").data("kendoScheduler");
                            if (scheduler) {
                                scheduler._workDayMode = localStorage.getItem("IsShowWorkHours") == "true" ? true : false;
                            }
                            PMSScheduler.LoadScheduler(e);
                            PMSScheduler.RefreshScheduler();
                        },
                        create: function (e) {

                        },
                        destroy: function (e) {
                            e.success("");
                        },
                        update: function (e) {

                        },
                        parameterMap: function (options, operation) {
                            if (operation !== "read" && options.models) {
                                return {
                                    models: kendo.stringify(options.models)
                                };
                            }
                        }
                    },
                },
                schema: {
                    model: {
                        id: "AppointmentId",
                        fields: {
                            AppointmentId: {
                                type: "number"
                            },

                            start: {
                                type: "date", field: "start"
                            },
                            end: {
                                type: "date", field: "end"
                            },
                            FaciityColor: {
                                field: "FaciityColor"
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
                                field: "CopayClass", defaultValue: "Green",
                            },
                            EligibilityStatus: {
                                from: "EligibilityStatus", defaultValue: "",
                            },
                            GroupType: {
                                field: "GroupType", defaultValue: "Provider",
                            },
                            FirstName: {
                                field: "FirstName", defaultValue: "",
                            },
                            LastName: {
                                field: "LastName", defaultValue: "",
                            },
                            AccountNumber: {
                                field: "AccountNumber", defaultValue: "",
                            },
                            InsurancePlanId: {
                                field: "InsurancePlanId", defaultValue: 0,
                            },
                            InsuranceId: {
                                field: "InsuranceId", defaultValue: 0,
                            },
                            PatientSex: {
                                field: "PatientSex", defaultValue: "",
                            },
                            PatientDOB: {
                                field: "PatientDOB", defaultValue: "",
                            },
                            PatientAddress1: {
                                field: "PatientAddress1", defaultValue: "",
                            },
                            PatientCity: {
                                field: "PatientCity", defaultValue: "",
                            },
                            PatientState: {
                                field: "PatientState", defaultValue: "",
                            },
                            PatientZip: {
                                field: "PatientZip", defaultValue: "",
                            },
                            PatientEthnicityIds: {
                                field: "PatientEthnicityIds", defaultValue: "",
                            },
                            PatientRaceIds: {
                                field: "PatientRaceIds", defaultValue: "",
                            },
                            PatientMaritalStatus: {
                                field: "PatientMaritalStatus", defaultValue: "",
                            },
                            PatientHomeTel: {
                                field: "PatientHomeTel", defaultValue: "",
                            },
                            NewPatientColor: {
                                field: "NewPatientColor", defaultValue: "#0088cc",
                            },
                            EstablishedPatientColor: {
                                field: "EstablishedPatientColor", defaultValue: "#0088cc",
                            },
                        },
                    }
                },

                moveStart: function (e) {
                    PMSScheduler.CutEventHolder = null;
                    var model = e.event;
                    if (model.AppointmentStatus && (model.AppointmentStatus.toLowerCase() == "scheduled" || model.AppointmentStatus.toLowerCase() == "pending") && PMSScheduler.CanScheduler._selectedViewName != "month") {
                        var duration = PMSScheduler.diff_minutes(new Date(model.AppointmentDateFrom), new Date(model.AppointmentDateTo));
                        PMSScheduler.CutEventHolder = model;
                        PMSScheduler.CutEventHolder.OldStart = new Date(model.start);
                        PMSScheduler.CutEventHolder.SchSlotDuration = duration;
                    }
                },
                cancel: function (e) {
                },
                add: function (e) {
                    if (PMSScheduler.PreviousViewType != "month") {
                        e.preventDefault();
                        var monutes = PMSScheduler.diff_minutes(e.event.start, e.event.end);

                        if (!PMSScheduler.IsResourceSch && typeof e.event.ProviderId != "undefined") {
                            var provider_obj = $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").dataSource.data();
                            //var provider = provider_obj.filter(f => f.id == e.event.ProviderId)[0];

                            var provider = provider_obj.filter(function (f) {
                                return f.id == e.event.ProviderId;
                            })[0];

                            Scheduling_Calendar.AppointmentAddNew("Add", null, e.event.ProviderId, provider.name, null, null, e.event.FacilityId, e.event.FacilityName, null, PMSScheduler.formatAMPM(e.event.start), PMSScheduler.formatAMPM(e.event.end), monutes, null, $.datepicker.formatDate('mm/dd/yy', e.event.start), false, 0);
                        } else if (PMSScheduler.IsResourceSch && typeof e.event.ProviderId != "undefined") {
                            var resource_obj = $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect").dataSource.data();
                            //var resource = resource_obj.filter(f => f.id == e.event.ProviderId)[0];
                            var resource = resource_obj.filter(function (f) {
                                return f.id == e.event.ProviderId;
                            })[0];

                            Scheduling_Calendar.AppointmentAddNew("Add", null, null, null, e.event.ProviderId, resource.name, e.event.FacilityId, e.event.FacilityName, null, PMSScheduler.formatAMPM(e.event.start), PMSScheduler.formatAMPM(e.event.end), monutes, null, $.datepicker.formatDate('mm/dd/yy', e.event.start), false, 0);
                        } else {
                            Scheduling_Calendar.AppointmentAddNew("Add", null, null, null, null, null, null, null, null, PMSScheduler.formatAMPM(e.event.start), PMSScheduler.formatAMPM(e.event.end), monutes, null, $.datepicker.formatDate('mm/dd/yy', e.event.start), false, 0);
                        }

                    } else {
                        e.preventDefault();
                    }
                },
                edit: function (e) {
                    var mode = "";
                    var checkin = (e.event.AppointmentStatus.toLocaleLowerCase() == "check in" || e.event.AppointmentStatus.toLocaleLowerCase() == "check out") ? 1 : 0;
                    checkin = (e.event.AppointmentStatus.toLocaleLowerCase() == "cancel" || e.event.AppointmentStatus.toLocaleLowerCase() == "no show") ? 2 : checkin;
                    if (e.event.AppointmentId > 0) {
                        mode = "Edit";
                        Scheduling_Calendar.AppointmentAddNew(mode, e.event.AppointmentId, e.event.ProviderId, e.event.ProviderName, null, null, e.event.FacilityId, e.event.FacilityName, null, e.event.TimeFrom, e.event.TimeTo, null, null, e.event.AppointmentDate, e.event.isNoteCreated, checkin);
                    } else {
                        mode = "Add";
                        var monutes = PMSScheduler.diff_minutes(e.event.start, e.event.end);
                        var provider_obj = $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").dataSource.data();
                        //var provider = provider_obj.filter(f => f.id == e.event.ProviderId)[0];

                        var provider = provider_obj.filter(function (f) {
                            return f.id == e.event.ProviderId;
                        })[0];

                        Scheduling_Calendar.AppointmentAddNew(mode, null, e.event.ProviderId, provider.name, null, null, e.event.FacilityId, e.event.FacilityName, null, PMSScheduler.formatAMPM(e.event.start), PMSScheduler.formatAMPM(e.event.end), monutes, null, e.event.AppointmentDate, e.event.isNoteCreated, checkin);
                    }
                    e.preventDefault();
                },
                group: {
                    resources: ["providers"]//PMSScheduler.IsResourceSch ? [" "] : ["providers"]
                },
            }).data("kendoScheduler");

            var ZoomInIcon = $("<li class='ZoomInli k-state-default k-header'><a class='k-link' onclick='PMSScheduler.ZoomInScheduler(event)'><i  class='fa fa-search-plus'></i></a></li>");
            $(scheduler).find('.k-scheduler-toolbar').find('ul.k-scheduler-navigation').find('li.k-nav-next').after(ZoomInIcon)

            var ZoomOutIcon = $("<li class='ZoomOutli k-state-default k-header'><a class='k-link' onclick='PMSScheduler.ZoomOutScheduler(event)'><i  class='fa fa-search-minus'></i></a></li>");
            $(scheduler).find('.k-scheduler-toolbar').find('ul.k-scheduler-navigation').find('li.ZoomInli').after(ZoomOutIcon)

            PMSScheduler.SetContextMenu();

            PMSScheduler.InitilizeAppointmentToolTip();
            PMSScheduler.InitilizeAppointmentToolTipMonth();
            PMSScheduler.deleteAppointment();
            PMSScheduler.fitSchedulerWidget();
            PMSScheduler.GetSchedulerHeight();

        });
    },

    GetProviderSchedule: function (viewType) {

        var IsProvider = true;
        var scheduler = $("#scheduler").data("kendoScheduler");
        var Ids = $("#pnlPMSScheduler #hfProviderIds").val();
        if ($("#pnlPMSScheduler #hfResourceIds").val()) {
            IsProvider = false;
            Ids = $("#pnlPMSScheduler #hfResourceIds").val();
        }


        var StartDate = kendo.toString(new Date(), "d");

        if (PMSScheduler.NavigationEvent) {
            StartDate = kendo.toString(PMSScheduler.NavigationEvent.date, "d");
        }
        else if (scheduler && scheduler.date())
            StartDate = kendo.toString(scheduler.date(), "d");

        var objData = new Object();
        objData["IsProvider"] = IsProvider;
        objData["ProviderIds"] = Ids;
        objData["StartDate"] = StartDate;
        objData["FacilityIds"] = $("#pnlPMSScheduler #hfFacilityIds").val();
        objData["SchViewType"] = viewType;

        objData["CommandType"] = "get_provider_schdule";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");

    },

    SetSchedulerView: function (e) {

        var deff = $.Deferred();
        var Ids = $("#pnlPMSScheduler #hfProviderIds").val();
        if ($("#pnlPMSScheduler #hfResourceIds").val()) {
            Ids = $("#pnlPMSScheduler #hfResourceIds").val();
        }

        var scheduler = $("#scheduler").data("kendoScheduler");

        if (Ids.split(',').length == 1) {
            PMSScheduler.GetProviderSchedule_Warper(e.view).done(function (response) {
                scheduler.options.workDayStart = response[0].workDayStart;
                scheduler.options.workDayEnd = response[0].workDayEnd;
                deff.resolve('ok');
            });
        }
        else {
            deff.resolve('ok');
        }

        deff.then(function (res) {
            if (PMSScheduler.SchdefObject) {
                $.when(PMSScheduler.SchdefObject).then(function () {

                    if (e.view == "workWeek") {
                        var currDate = new Date;
                        var firstDayofWeek = new Date(currDate.setDate(currDate.getDate() - currDate.getDay()));
                        scheduler.options.start = firstDayofWeek;
                        scheduler.options.workWeekStart = 0;
                        scheduler.options.workWeekEnd = 6;

                    }
                    // else if (e.view == "day") {
                    var scheduledHours = navigationJSON;
                    if (scheduledHours != null && scheduledHours.length > 0) {
                        var sDate = scheduledHours[0].AppointmentDate;
                        sDate = utility.RemoveTimeFromDate(null, sDate);
                        var tFrom = scheduledHours[0].BeginningTime;
                        var tTo = scheduledHours[0].EndingTime;
                        if (tFrom && tTo) {
                            scheduler.options.workDayStart = new Date(sDate + " " + tFrom);
                            scheduler.options.workDayEnd = new Date(sDate + " " + tTo);
                        }
                    }

                    //}
                    scheduler.view(scheduler.view().name);
                    PMSScheduler.ResetAppointmentCount();
                    PMSScheduler.LoadSchBlockHours();
                });
            }
        });
    },

    ZoomInScheduler: function (event) {
        if (event != this) {
            event.stopPropagation();
        }
        if (PMSScheduler.ZoomINCount == 0)
            $('#scheduler').animate({ 'zoom': 1.2 }, 400);
        else if (PMSScheduler.ZoomINCount == 1)
            $('#scheduler').animate({ 'zoom': 1.4 }, 400);
        else if (PMSScheduler.ZoomINCount == 2)
            $('#scheduler').animate({ 'zoom': 1.6 }, 400);
        if (PMSScheduler.ZoomINCount < 3)
            PMSScheduler.ZoomINCount++;

        setTimeout(function () {
            PMSScheduler.RefreshScheduler();
            PMSScheduler.SetDateAlignment();
        }, 400);
    },

    ZoomOutScheduler: function (event) {
        if (event != this) {
            event.stopPropagation();
        }
        if (PMSScheduler.ZoomINCount == 1) {
            $('#scheduler').animate({ 'zoom': 1 }, 400);
        }
        else if (PMSScheduler.ZoomINCount == 2) {
            $('#scheduler').animate({ 'zoom': 1.2 }, 400);
        }
        else if (PMSScheduler.ZoomINCount == 3) {
            $('#scheduler').animate({ 'zoom': 1.4 }, 400);

        }
        if (PMSScheduler.ZoomINCount > 0)
            PMSScheduler.ZoomINCount--;

        setTimeout(function () {
            PMSScheduler.RefreshScheduler();
            PMSScheduler.SetDateAlignment();
        }, 400);
    },

    SetDateAlignment: function () {
        var scheduler = $("#scheduler").data("kendoScheduler");
        if (scheduler._selectedViewName == "day") {
            if (PMSScheduler.ZoomINCount == 1) {
                $(".k-nav-current").css("margin-left", "20vw");
            }
            else if (PMSScheduler.ZoomINCount == 2) {
                $(".k-nav-current").css("margin-left", "13vw");
            }
            else if (PMSScheduler.ZoomINCount == 3) {
                $(".k-nav-current").css("margin-left", "8vw");

            } else {
                $(".k-nav-current").css("margin-left", "25vw");
            }
        } else {
            if (PMSScheduler.ZoomINCount == 1) {
                $(".k-nav-current").css("margin-left", "16vw");
            }
            else if (PMSScheduler.ZoomINCount == 2) {
                $(".k-nav-current").css("margin-left", "10vw");
            }
            else if (PMSScheduler.ZoomINCount == 3) {
                $(".k-nav-current").css("margin-left", "2vw");

            } else {
                $(".k-nav-current").css("margin-left", "20vw");
            }
        }
    },

    scheduler_dataBoundFormatedDate: function (e) {
        if (e.sender.viewName() == "month") {
            var test = e.sender._model.formattedDate;
            var startDate = kendo.toString(kendo.parseDate(test.trim()), 'dddd MMMM dd, yyyy');
            var endDate = kendo.toString(new Date(kendo.parseDate(e.sender._model.formattedDate.trim()).getFullYear(), kendo.parseDate(e.sender._model.formattedDate.trim()).getMonth() + 1, 0), 'dddd MMMM dd, yyyy');
            $(".k-lg-date-format").html(startDate + " - " + endDate);
        }
        else if (e.sender.viewName() == "week" || e.sender.viewName() == "workWeek") {
            var test = e.sender._model.formattedDate.split('-');
            if (test.length > 0) {
                var startDate = kendo.toString(kendo.parseDate(test[0].trim()), 'dddd MMMM dd, yyyy');
                var endDate = kendo.toString(kendo.parseDate(test[1].trim()), 'dddd MMMM dd, yyyy');
                $(".k-lg-date-format").html(startDate + " - " + endDate);
            }
        }
        else if (e.sender.viewName() == "day") {
            var test = e.sender._model.formattedDate;
            var startDate = kendo.toString(kendo.parseDate(test.trim()), 'dddd MMMM dd, yyyy');
            $(".k-lg-date-format").html(startDate);
        }
    },

    SetContextMenu: function () {

        $("#SchedulerContextMenu").kendoContextMenu({
            filter: ".k-event, .k-event-inverse, .k-scheduler-table td",
            target: "#scheduler",
            //trigger: 'left',
            animation: {
                open: {
                    effects: "fadeIn"
                },
                duration: 500
            },
            //showOn: "click",
            select: function (e) {

                var target = $(e.target);
                var scheduler = $("#scheduler").data("kendoScheduler");
                var slot = scheduler.slotByElement(target);
                if (target.hasClass("k-event")) {
                    var model = scheduler.dataSource.getByUid(target.data().uid);
                    var optionName = e.item.getAttribute("name");
                    var optionType = e.item.getAttribute("optiontype");
                    var $uiItem = $(e.item);
                    var duration = PMSScheduler.diff_minutes(new Date(model.AppointmentDateFrom), new Date(model.AppointmentDateTo));
                    switch (optionName) {
                        case "copyAppointment":
                            {
                                PMSScheduler.CopiedEventHolder = model;
                                PMSScheduler.CopiedEventHolder.OldStart = new Date(model.start);
                                PMSScheduler.CopiedEventHolder.SchSlotDuration = duration
                                break;
                            }
                        case "cutAppointment":
                            {
                                PMSScheduler.CutEventHolder = model;
                                PMSScheduler.CutEventHolder.OldStart = new Date(model.start);
                                PMSScheduler.CutEventHolder.SchSlotDuration = duration
                                break;
                            }
                        default:
                            PMSScheduler.OpenSelectedScreen(model, optionType, optionName, $uiItem);
                            break;
                    }
                } else {
                    if ($(e.item).hasClass("pasteAppointment")) {
                        $(e.item).remove();
                        if (PMSScheduler.CopiedEventHolder) {
                            var oldDateTime = new Date(PMSScheduler.CopiedEventHolder.OldStart);
                            var newDateTime = new Date(slot.startDate);
                            PMSScheduler.CopiedEventHolder.oldDateTime = oldDateTime;
                            PMSScheduler.CopiedEventHolder.newDateTime = newDateTime;
                            PMSScheduler.CopiedEventHolder.start = new Date(slot.startDate);
                            PMSScheduler.CopiedEventHolder.end = new Date(slot.startDate);
                            PMSScheduler.CopiedEventHolder.NewStart = new Date(slot.startDate);
                            PMSScheduler.CopiedEventHolder.NewAppointmentDate = kendo.toString(new Date(slot.startDate), "s");
                            PMSScheduler.CopiedEventHolder.NewgroupIndex = slot.groupIndex;
                            PMSScheduler.onCopyPaste(oldDateTime, newDateTime);
                        }
                        else if (PMSScheduler.CutEventHolder) {
                            var oldDateTime = new Date(PMSScheduler.CutEventHolder.OldStart);
                            var newDateTime = new Date(slot.startDate);
                            PMSScheduler.CutEventHolder.oldDateTime = oldDateTime;
                            PMSScheduler.CutEventHolder.newDateTime = newDateTime;
                            PMSScheduler.CutEventHolder.start = new Date(slot.startDate);
                            PMSScheduler.CutEventHolder.end = new Date(slot.startDate);
                            PMSScheduler.CutEventHolder.NewStart = new Date(slot.startDate);
                            PMSScheduler.CutEventHolder.NewAppointmentDate = kendo.toString(new Date(slot.startDate), "s");
                            PMSScheduler.CutEventHolder.NewgroupIndex = slot.groupIndex;
                            PMSScheduler.onCutPaste();
                        }
                    }
                    else if ($(e.item).hasClass("AddBlockHours")) {
                        //  PMSScheduler.CopiedEventHolder = null
                        // PMSScheduler.CutEventHolder = null;
                        var params = [];
                        params["FromAdmin"] = "0";
                        var StartDate = kendo.toString(new Date(slot.startDate), "MM/dd/yyyy");
                        var EndDate = kendo.toString(new Date(slot.endDate), "MM/dd/yyyy");
                        var newDateTime = new Date(slot.startDate);

                        var AppDate = kendo.toString(new Date(slot.startDate), "s");
                        var fromT = kendo.toString(new Date(slot.startDate), "t");
                        var toT = kendo.toString(new Date(slot.endDate), "t");

                        var scheduler = $("#scheduler").data("kendoScheduler");


                        var FacilityId = null;
                        var facilityIds = $("#pnlPMSScheduler #hfFacilityIds").val() ? $("#pnlPMSScheduler #hfFacilityIds").val() : globalAppdata.DefaultFacilityId;


                        var FacililityArry = facilityIds.split(',');

                        if (FacililityArry.length == 1)
                            FacilityId = FacililityArry[0].trim();
                        else
                            FacilityId = null;

                        var ResourceId = null;
                        var ProviderId = null;
                        if (scheduler.resources[1]) {
                            var resourceArry = scheduler.resources[1].dataSource.options.data;

                            var resource = resourceArry[slot.groupIndex];

                            if (resource.groupType == "Resource") {
                                ResourceId = resource.value;
                            }
                            else {
                                ProviderId = resource.value;
                            }
                        }


                        params["FromDate"] = StartDate;
                        params["EndDate"] = EndDate;
                        params["FromTime"] = fromT;
                        params["ProviderId"] = ProviderId;
                        params["ResourceId"] = ResourceId;
                        params["FaclilityId"] = FacilityId;
                        params["ToTime"] = toT;
                        params["ParentCtrl"] = 'schTabCalendar';
                        params["BlockHoursId"] = null;
                        params["mode"] = "Add";
                        params["OpenFromSchedulerSlot"] = true;
                        LoadActionPan('blockHoursDetail', params);

                    }
                    else if ($(e.item).hasClass("EditBlockHours")) {
                        // PMSScheduler.CopiedEventHolder = null
                        //   PMSScheduler.CutEventHolder = null;
                        var params = [];
                        params["FromAdmin"] = "0";
                        var StartDate = slot.startDate;
                        var EndDate = slot.endDate;
                        var newDateTime = new Date(slot.startDate);

                        var APpDate = kendo.toString(new Date(slot.startDate), "s");
                        var fromT = kendo.toString(new Date(slot.startDate), "t");
                        var toT = kendo.toString(new Date(slot.endDate), "t");

                        var BlockHoursId = $(slot.element).attr('BlockHoursId');
                        params["ParentCtrl"] = 'schTabCalendar';
                        params["BlockHoursId"] = BlockHoursId;
                        params["OpenFromSchedulerSlot"] = true;
                        params["mode"] = "Edit";
                        LoadActionPan('blockHoursDetail', params);

                    }
                    else if ($(e.item).hasClass("DeleteBlockHours")) {
                        // PMSScheduler.CopiedEventHolder = null
                        // PMSScheduler.CutEventHolder = null;
                        var BlockHoursId = $(slot.element).attr('BlockHoursId');
                        PMSScheduler.BlockHoursDelete(BlockHoursId, null);

                    }


                }
            },
            open: function (e) {

                // set menu options avalibility accourding to the business rules.
                var scheduler = $("#scheduler").data("kendoScheduler");
                var dataUID = $(e.target).attr("data-uid");

                if (PMSScheduler.currentEvent) {
                    dataUID = $(PMSScheduler.currentEvent).attr('data-uid');
                    PMSScheduler.currentEvent = "";
                }
                PMSScheduler.contextMenuCounter = PMSScheduler.contextMenuCounter + 1;

                var model = scheduler.dataSource.getByUid(dataUID);

                // imp-2905
                $(".sc-tooltip").hide();

                if (model)
                    PMSScheduler.SetContextMenuOptions(model);

                if ($(e.target).hasClass("k-event")) {
                    $(".pasteAppointment").remove();
                    $(e.sender.element.find('.mainli')).removeClass('hidden');

                }
                else if (PMSScheduler.CopiedEventHolder) {
                    var menu = e.sender;
                    menu.remove(".pasteAppointment");
                    var slot = scheduler.slotByElement(e.target);
                    if (!$(e.sender.element.find('.mainli')).hasClass('hidden')) {
                        $(e.sender.element.find('.mainli')).addClass('hidden');
                    }
                    var scheduler = $("#scheduler").data("kendoScheduler");
                    var resourceArry = scheduler.resources[1].dataSource.options.data;
                    var resource = resourceArry[slot.groupIndex];
                    var newProviderId = PMSScheduler.CopiedEventHolder.ProviderId;
                    if (resource) {
                        newProviderId = resource.value;
                    }
                    var startDateTime = PMSScheduler.CopiedEventHolder.end;
                    var toT = slot.startDate;
                    var fromT = PMSScheduler.CopiedEventHolder.OldStart;
                    if (Date.parse(toT) != Date.parse(fromT) || newProviderId != PMSScheduler.CopiedEventHolder.ProviderId) {
                        PMSScheduler.CopiedEventHolder.ProviderId = newProviderId;
                        menu.append('<li class="pasteAppointment"><span>Paste&nbsp;<i class="fa fa-paste" aria-hidden="true"></i></span></li>');
                    }
                    if ($(slot.element).attr('isBlock')) {

                        menu.remove(".AddBlockHours");
                        menu.remove(".EditBlockHours");
                        menu.remove(".DeleteBlockHours");
                        menu.append('<li name="EditBlockHours" class="EditBlockHours"><span>Edit Block Hours&nbsp;<i  aria-hidden="true"></i></span></li>');
                        menu.append('<li name="DeleteBlockHours" class="DeleteBlockHours"><span>Delete Block Hours&nbsp;<i  aria-hidden="true"></i></span></li>');


                    }
                    else {

                        menu.remove(".EditBlockHours");
                        menu.remove(".AddBlockHours");
                        menu.remove(".DeleteBlockHours");
                        if (PMSScheduler.CanScheduler._selectedViewName != "month") {
                            menu.append('<li name="AddBlockHours" class="AddBlockHours"><span>Add Block Hours&nbsp;<i  aria-hidden="true"></i></span></li>');
                        }



                    }
                }
                else if (PMSScheduler.CutEventHolder) {
                    var menu = e.sender;
                    menu.remove(".pasteAppointment");
                    var slot = scheduler.slotByElement(e.target);
                    if (!$(e.sender.element.find('.mainli')).hasClass('hidden')) {
                        $(e.sender.element.find('.mainli')).addClass('hidden');
                    }
                    var scheduler = $("#scheduler").data("kendoScheduler");
                    var resourceArry = scheduler.resources[1].dataSource.options.data;
                    var resource = resourceArry[slot.groupIndex];
                    var newProviderId = PMSScheduler.CutEventHolder.ProviderId;
                    if (resource) {
                        newProviderId = resource.value;
                    }
                    var startDateTime = PMSScheduler.CutEventHolder.end;
                    var toT = slot.startDate;
                    var fromT = PMSScheduler.CutEventHolder.OldStart;
                    if (Date.parse(toT) != Date.parse(fromT) || newProviderId != PMSScheduler.CutEventHolder.ProviderId) {
                        PMSScheduler.CutEventHolder.ProviderId = newProviderId;
                        menu.append('<li class="pasteAppointment"><span>Paste&nbsp;<i class="fa fa-paste" aria-hidden="true"></i></span></li>');

                    }
                    if ($(slot.element).attr('isBlock')) {

                        menu.remove(".AddBlockHours");
                        menu.remove(".EditBlockHours");
                        menu.remove(".DeleteBlockHours");
                        menu.append('<li name="EditBlockHours" class="EditBlockHours"><span>Edit Block Hours&nbsp;<i  aria-hidden="true"></i></span></li>');
                        menu.append('<li name="DeleteBlockHours" class="DeleteBlockHours"><span>Delete Block Hours&nbsp;<i  aria-hidden="true"></i></span></li>');


                    }
                    else {

                        menu.remove(".EditBlockHours");
                        menu.remove(".AddBlockHours");
                        menu.remove(".DeleteBlockHours");
                        if (PMSScheduler.CanScheduler._selectedViewName != "month") {
                            menu.append('<li name="AddBlockHours" class="AddBlockHours"><span>Add Block Hours&nbsp;<i  aria-hidden="true"></i></span></li>');
                        }



                    }
                }
                else {
                    if ($(e.item).attr("role") != "menuitem") {
                        if (!$(e.sender.element.find('.mainli')).hasClass('hidden')) {
                            $(e.sender.element.find('.mainli')).addClass('hidden');
                        }
                    }
                    var scheduler = $("#scheduler").data("kendoScheduler");
                    var menu = e.sender;
                    var slot = scheduler.slotByElement(e.target);
                    if (slot) {
                        if ($(slot.element).attr('isBlock')) {

                            menu.remove(".AddBlockHours");
                            menu.remove(".EditBlockHours");
                            menu.remove(".DeleteBlockHours");
                            menu.append('<li name="EditBlockHours" class="EditBlockHours"><span>Edit Block Hours&nbsp;<i  aria-hidden="true"></i></span></li>');
                            menu.append('<li name="DeleteBlockHours" class="DeleteBlockHours"><span>Delete Block Hours&nbsp;<i  aria-hidden="true"></i></span></li>');


                        }
                        else {

                            menu.remove(".EditBlockHours");
                            menu.remove(".AddBlockHours");
                            menu.remove(".DeleteBlockHours");
                            if (PMSScheduler.CanScheduler._selectedViewName != "month") {
                                menu.append('<li name="AddBlockHours" class="AddBlockHours"><span>Add Block Hours&nbsp;<i  aria-hidden="true"></i></span></li>');
                            }
                            else {
                                setTimeout(function () {
                                    $('#SchedulerContextMenu').css("display", "none");
                                }, 30);
                            }




                        }

                    }
                    // menu.remove(".pasteAppointment");

                }
            }
        });
    },

    SetminorTickCount: function () {
        return 4;
    },
    deleteAppointment: function () {
        $("#scheduler").on("click", ".k-si-close", function (e) {
            //disable the event based on your custom logic
            var scheduler = $("#scheduler").getKendoScheduler();
            var eventUID = $(e.target.closest(".k-event")).data("uid");
            var event = scheduler.occurrenceByUid(eventUID);
            if (event.AppointmentStatus != "Check In" && event.AppointmentStatus != "Check Out") {
                if (!event.isAllDay) {
                    e.stopImmediatePropagation();
                    Scheduling_Calendar.DeletePatientAppointment(event.AppointmentId);
                }
            } else {
                e.stopImmediatePropagation();
                utility.DisplayMessages(event.AppointmentStatus + " appointment can not be deleted.", 3);
            }
        });
    },

    SetContextMenuOptions: function (model) {

        var class_ = "";
        switch (model.AppointmentStatus.toLowerCase()) {
            case "scheduled":
                class_ = "default-op"; break;
            case "check in":
                class_ = "checkin-op"; break;
            case "check out":
                class_ = "checkout-op"; break;
            case "pending":
                class_ = "pending-op"; break;
            case "confirm":
            case "voice":
            case "no response":
                class_ = "cvn-op"; break;
            case "no show":
            case "cancel":
                class_ = "nc-op"; break;
            default:
                class_ = "default-op"; break;

        }

        if (class_) {
            $("#SchedulerContextMenu").find("li").each(function (i, item) {
                if ($(item).hasClass("paste-op") && $(item).hasClass(class_)) {
                    if (PMSScheduler.CanScheduler._selectedViewName != "month") {
                        $(item).css("display", "block");
                    }
                    else {
                        $(item).css("display", "none");
                    }
                }
                else if ($(item).hasClass("mainli") && $(item).hasClass(class_)) {
                    $(item).css("display", "block");
                }
                else if ($(item).hasClass("mainli")) {
                    $(item).css("display", "none");
                }

                if ($(item).attr("name") == "editAppointment" && class_ == "nc-op") {
                    $(item).children("span").html("View Appointment");
                }
                else if ($(item).attr("name") == "editAppointment") {
                    $(item).children("span").html("Edit Appointment");
                }

                if ($(item).attr("name") == "createNote" && model.isNoteCreated == true) {
                    $(item).children("span").html("Edit Note");
                }
                else if ($(item).attr("name") == "createNote" && model.isNoteCreated == false) {
                    $(item).children("span").html("Create Note");
                }

                if ($(item).attr("name") == "createNote" && (model.AppointmentStatus.toLowerCase() == "check out" || model.AppointmentStatus.toLowerCase() == "check in") && model.IsNoteSigned == true) {
                    $(item).css("display", "none");
                }

                if ($(item).attr("name") == "createCharge" && model.AppointmentStatus.toLowerCase() == "check out" && model.IsNoteSigned == true) {
                    $(item).children("span").html("View Charge");
                }
                else if ($(item).attr("name") == "createCharge" && model.IsNoteSigned == false) {
                    $(item).children("span").html("Create Charge");
                }

                if ($(item).attr("name") == "viewNote" && model.AppointmentStatus.toLowerCase() == "check out" && model.IsNoteSigned == true) {
                    $(item).css("display", "block");
                }
                else if ($(item).attr("name") == "viewNote") {
                    $(item).css("display", "none");
                }

                if ($(item).attr("name") == "AddBlockHours") {
                    $(item).css("display", "none");
                }
                else if ($(item).attr("name") == "EditBlockHours") {
                    $(item).css("display", "none");
                }
                else if ($(item).attr("name") == "DeleteBlockHours") {
                    $(item).css("display", "none");
                }
            });
        }

        if (model.AppointmentStatus.toLowerCase().indexOf('reschedule') >= 0) {
            $("#SchedulerContextMenu #reschedule").css("display", "none");
        }


        //if (model.AppointmentStatus.toLowerCase() == "scheduled" || model.AppointmentStatus.toLowerCase() == "pending") {
        //    $("#SchedulerContextMenu #copy-op").css("display", "block");
        //    $("#SchedulerContextMenu #cut-op").css("display", "block");
        //    $("#SchedulerContextMenu #reschedule").css("display", "block");
        //}
        //else {
        //    $("#SchedulerContextMenu #copy-op").css("display", "none");
        //    $("#SchedulerContextMenu #cut-op").css("display", "none");
        //    $("#SchedulerContextMenu #reschedule").css("display", "none");
        //}


        CacheManager.BindAppointmentStatusWorkFlow(model.SchStatusId, model.AppointmentStatus).done(function (optionsList) {
            PMSScheduler.MakeStatusList(optionsList);
            var cIndex = $('.k-animation-container').css('z-index');
            setTimeout(function () { $('#SchedulerContextMenu').parent('div').css('z-index', (parseInt(cIndex) + 100)); }, 500)
        });
    },

    MakeStatusList: function (optionsList) {

        $("#SchedulerContextMenu #ulAppointmentStatus").html("");
        for (var i = 0; i < optionsList.length; i++) {
            $("#SchedulerContextMenu #ulAppointmentStatus").append("<li optiontype='status' name='"
                + optionsList[i].PossibleOption
                + "' class='k-item k-state-default k-first' role='menuitem'"
                + "' id='" + optionsList[i].DestinationStatusId
                + "' status='" + optionsList[i].PossibleOption
                + "' color='" + optionsList[i].Color
                + "'> <span class='k-link'>" + optionsList[i].PossibleOption
                + "</span></li>");
        }
    },

    OpenSelectedScreen: function (model, optionType, optionName, $uiItem) {

        if (optionType == "status") {

            PMSScheduler.AppointmentStatusUpdate(model.AppointmentId, $uiItem.attr("status"), $uiItem.attr("color"), $uiItem.attr("id"));
        }
        else if (optionType == "reminder") {

            PMSScheduler.SendQuickReminder(model, optionName.toLocaleUpperCase());
        }
        else {

            switch (optionName) {
                case "demographics":
                    {
                        //params["QuickAddPatient"] = true;
                        SelectTab('mstrTabPatient', 'false');
                        setTimeout(function () {
                            Patient_Search.params.FormName = "DayCalendar";
                            Patient_Search.SelectPatient(model.PatientId, null);
                            $('#patTabDemographic').click();
                            $("body").removeClass("modal-open").removeAttr("style");
                        }, 200);
                        break;
                    }
                case "copyment":
                    {
                        var params = [];
                        params["FromAdmin"] = "0";
                        params["ProviderId"] = model.ProviderId;
                        params["ResourceId"] = model.ResourceIds; //require crosscheck
                        params["FacilityId"] = model.FacilityId;
                        params["DayDate"] = model.AppointmentDate;
                        params["AppointmentId"] = model.AppointmentId;
                        params["PatientId"] = model.PatientId;

                        params["PatientVisitId"] = model.VisitId;
                        params["PatientVisitName"] = model.VisitTypeName;
                        params["ParentCtrl"] = 'schTabCalendar';
                        LoadActionPan('schcopayment', params);

                        break;
                    }
                case "unAllocatedCopay":
                    {
                        var params = [];
                        params["FromAdmin"] = "0";
                        params["ProviderId"] = model.ProviderId;
                        params["ResourceId"] = model.ResourceIds; //require crosscheck
                        params["FacilityId"] = model.FacilityId;
                        params["DayDate"] = model.AppointmentDate;
                        params["AppointmentId"] = model.AppointmentId;;
                        params["PatientId"] = model.PatientId;
                        params["ParentCtrl"] = 'schTabCalendar';
                        //params["VisitId"] = visitid ? visitid : 0; not avaliable
                        LoadActionPan('Scheduling_UnallocatedCopayment', params);

                        break;
                    }
                case "patientEligibility":
                    {
                        PMSScheduler.OpenPatientEligibility(model.AppointmentId);
                        break;
                    }
                case "editAppointment":
                    {
                        var mode = "Edit";
                        var checkin = (model.AppointmentStatus.toLocaleLowerCase() == "check in" || model.AppointmentStatus.toLocaleLowerCase() == "check out") ? 1 : 0;
                        checkin = (model.AppointmentStatus.toLocaleLowerCase() == "cancel" || model.AppointmentStatus.toLocaleLowerCase() == "no show") ? 2 : checkin;
                        Scheduling_Calendar.AppointmentAddNew(mode, model.AppointmentId, model.ProviderId, model.ProviderName, null, null, model.FacilityId, model.FacilityName, null, model.TimeFrom, model.TimeTo, null, null, model.AppointmentDate, model.isNoteCreated, checkin);
                        break;
                    }
                case "checkIn":
                    {
                        PMSScheduler.LoadCheckIn(model);
                        break;
                    }
                case "reschedule":
                    {
                        PMSScheduler.RescheduleAppointmentSearch(model);
                        break;
                    }
                case "deleteAppointment":
                    {
                        PMSScheduler.PatientAppointmentDelete(model.AppointmentId);
                        break;
                    }
                case "cancelCheckIn":
                    {
                        PMSScheduler.CancelPatientCheckIn(model.VisitId, model.AppointmentId, model.LastScheduleStatusId, model.LastAppointmentStatus);
                        break;
                    }
                case "checkOut":
                    {
                        PMSScheduler.LoadCheckOut(model);
                        break;
                    }
                case "createCharge":
                    {
                        PMSScheduler.CreateVisitCharge(model);
                        break;
                    }
                case "createNote":
                    {
                        PMSScheduler.LoadClinicalNote(model, false);
                        break;
                    }
                case "viewNote":
                    {
                        PMSScheduler.LoadClinicalNote(model, true);
                        break;
                    }
                case "printSupperBill":
                    {
                        PMSScheduler.PrintLetter(model);
                        break;
                    }
                case "letter":
                    {
                        PMSScheduler.CreateLetter(model.PatientId);
                        break;
                    }
                case "History":
                    {
                        var params = [];
                        params["FromAdmin"] = "0";
                        params["AppointmentId"] = model.AppointmentId;;
                        params["PatientId"] = model.PatientId;
                        params["ParentCtrl"] = 'schTabCalendar';
                        LoadActionPan('appointmentHistory', params);
                        break;
                    }
                default:
                    break;
            }
        }

    },

    LoadCheckOut: function (model) {
        var params = [];
        params["FromAdmin"] = "0";
        params["ProviderId"] = model.ProviderId;
        params["ResourceId"] = model.ResourceId;
        params["FacilityId"] = model.FacilityId;
        params["DayDate"] = model.AppointmentDate;
        params["AppointmentId"] = model.AppointmentId;
        params["PatientId"] = model.PatientId;
        params["PatientVisitId"] = model.VisitId;
        params["ParentCtrl"] = 'schTabCalendar';
        LoadActionPan('schcheckout', params);
    },

    CreateVisitCharge: function (model) {
        if (model.PatientSex != "" && model.PatientAddress1 != "" && model.PatientCity != "" && model.PatientState != "" && model.PatientZip != ""
            && model.PatientEthnicityIds != "" && model.PatientRaceIds != "" && model.PatientMaritalStatus != "" && model.PatientHomeTel != "") {
            var params = [];
            params["ParentCtrl"] = 'schTabCalendar';
            params["AppointmentId"] = model.AppointmentId;
            params["VisitId"] = model.VisitId;
            params["patientID"] = model.PatientId;
            params["AppointmentDate"] = model.AppointmentDate.trim();
            LoadActionPan('EncounterChargeCapture', params);
        }
        else {
            model["mode"] = "Edit";
            model["PatBanner"] = true;
            model["IsFill"] = true;
            model["patientID"] = model.PatientId;
            model["FormsTitle"] = "Please complete missing demographics to continue!";
            LoadActionPan('demographicDetail', model);
        }
    },

    PrintLetter: function (model) {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'schTabCalendar';
        params["PatientId"] = model.PatientId;
        params["ProviderId"] = model.ProviderId;
        params["VisitId"] = model.Visitid;
        params["appid"] = model.AppointmentId;
        LoadActionPan('Clinical_SuperBillTemplate', params);
    },

    LoadClinicalNote: function (model, IsView) {
        if (model.PatientSex != "" && model.PatientAddress1 != "" && model.PatientCity != "" && model.PatientState != "" && model.PatientZip != ""
            && model.PatientEthnicityIds != "" && model.PatientRaceIds != "" && model.PatientMaritalStatus != "" && model.PatientHomeTel != "") {
            var AppReason = "";
            var RefProviderName = model.RefProviderName;
            var RefProviderId = model.RefProviderId;
            var noteid = model.Notesid;
            var VisitDate = model.AppointmentDate.split('T')[0]; //$('#pnlScheduleCalendar #daydate').text().trim();
            var AppointmentTime = model.TimeFrom;

            Clinical_Notes.params.NotesId = null;
            if (IsView == false) {

                if (model.isNoteCreated == true) {
                    AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            params["QuickAddPatient"] = true;


                            //if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                            //    if ($('#pnlScheduleCalendar #Resource option:selected').attr('refname') != "-") {
                            //        var ResourceproviderName = $('#pnlScheduleCalendar #Resource option:selected').attr('refname').split('-')[1]
                            //        var ResourceproviderId = $('#pnlScheduleCalendar #Resource option:selected').attr('refname').split('-')[0]
                            //    } else {
                            var ResourceproviderName = null;
                            var ResourceproviderId = null;
                            //    }
                            //}
                            var ParentCntrlLoadid = "Schedular";
                            var Reason = (AppReason == "undefined" || AppReason == null) ? null : AppReason;
                            var ForProgressNote = false;
                            var Room = "";
                            EMRUtility.CreateNote("Edit", model.PatientId, model.AppointmentId, model.ProviderId, model.ProviderName, AppointmentTime, model.VisitId, VisitDate, Reason, model.FacilityName,
                                model.FacilityId, Room, noteid, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, model.ResourceId, model.ResourceName, ResourceproviderId, ResourceproviderName, model.VisitTypeId, model.PatientTypeId, model.TimeFrom, model.TimeTo);

                        }
                        else
                            utility.DisplayMessages(strMessage, 2);
                    });
                }
                else {
                    AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            params["QuickAddPatient"] = true;

                            //if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                            //    if ($('#pnlScheduleCalendar #Resource option:selected').attr('refname') != "-") {
                            //        var ResourceproviderName = $('#pnlScheduleCalendar #Resource option:selected').attr('refname').split('-')[1]
                            //        var ResourceproviderId = $('#pnlScheduleCalendar #Resource option:selected').attr('refname').split('-')[0]
                            //    } else {
                            var ResourceproviderName = null;
                            var ResourceproviderId = null;
                            //    }
                            //}
                            var ParentCntrlLoadid = "Schedular"
                            var Reason = (AppReason == "undefined" || AppReason == null) ? null : AppReason;
                            var ForProgressNote = false;
                            var Room = "";
                            EMRUtility.CreateNote("Add", model.PatientId, model.AppointmentId, model.ProviderId, model.ProviderName, AppointmentTime, model.VisitId, VisitDate, Reason, model.FacilityName,
                                model.FacilityId, Room, noteid, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, model.ResourceId, model.ResourceName, ResourceproviderId, ResourceproviderName, model.VisitTypeId, model.PatientTypeId, model.TimeFrom, model.TimeTo);
                        }
                        else
                            utility.DisplayMessages(strMessage, 2);
                    });
                }
            } else {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        params["QuickAddPatient"] = true;
                        var ParentCntrlLoadid = "Schedular"
                        var Reason = (AppReason == "undefined" || AppReason == null) ? null : AppReason;
                        var ForProgressNote = false;
                        var Room = "";
                        EMRUtility.CreateNote("View", model.PatientId, model.AppointmentId, model.ProviderId, model.ProviderName, AppointmentTime, model.VisitId, VisitDate, Reason, model.FacilityName,
                            model.FacilityId, Room, noteid, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, model.VisitTypeId, model.PatientTypeId, model.TimeFrom, model.TimeTo);

                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
        }
        else {
            var objData = {};
            objData["mode"] = "Edit";
            objData["PatBanner"] = true;
            objData["IsFill"] = true;
            objData["patientID"] = model.PatientId;
            objData["FormsTitle"] = "Please complete missing demographics to continue!";
            objData["GrandParent"] = "PMSScheduler";
            objData["IsView"] = IsView;
            objData["model"] = model;
            LoadActionPan('demographicDetail', objData);
        }
    },

    PatientAppointmentDelete: function (AppointmentId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = AppointmentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        appointmentDetail.DeletePatientAppointment(selectedValue).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                var dataSourceApp = PMSScheduler.CanScheduler.dataSource;
                                //var ap = dataSourceApp._data.filter(f => f.AppointmentId == selectedValue)[0];

                                var ap = dataSourceApp._data.filter(function (f) {
                                    return f.AppointmentId == selectedValue;
                                })[0];

                                PMSScheduler.CanScheduler.dataSource.remove(ap);
                                PMSScheduler.CanScheduler.dataSource.sync();
                                PMSScheduler.ResetAppointmentCount();
                            }
                            else
                                utility.DisplayMessages(response.Message, 3);
                        });
                    }
                }, function () {
                }, '1');
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    AppointmentStatusUpdate: function (appid, AppointmentStatus, color, statusid) {

        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (AppointmentStatus.toLowerCase() == "cancel") {
                    // Ask for the cancellation reason
                    PMSScheduler.OpenCancellationReason(appid, statusid);
                }
                else {
                    Scheduling_Calendar.UpdateAppointmentStatus(appid, statusid).done(function (response) {
                        if (response.status != false) {

                            PMSScheduler.CanScheduler.dataSource.read();
                            //var dataSourceApp = PMSScheduler.CanScheduler.dataSource;
                            //var ap = dataSourceApp._data.filter(f => f.AppointmentId == appid)[0];
                            ////ap.VisitId = visitId;
                            //ap.AppointmentStatus = AppointmentStatus;
                            //ap.SchStatusId = statusid;
                            //ap.StatusColor = color;
                            //PMSScheduler.CanScheduler.dataSource.pushUpdate(ap);

                            utility.DisplayMessages(response.Message, 1);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 2);
                        }
                    });
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OpenCancellationReason: function (appid, statusid) {
        var params = [];
        params["AppointmentId"] = appid;
        params["StatusId"] = statusid;
        LoadActionPan('PMSScheduler_AppointmentCancellation', params);

    },
    CancelPatientCheckIn: function (VisitId, AppointmentId, lastStatusId, lastStatusName) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Are you sure want to Cancel CheckIn?', function () {
                    var selectedValue = VisitId;
                    if (selectedValue == "" || selectedValue == "undefined" || AppointmentId == "" || AppointmentId == "undefined") {
                    }
                    else {
                        schcheckin.CancelPatCheckIn(selectedValue, AppointmentId, lastStatusId, lastStatusName).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);

                                var dataSourceApp = PMSScheduler.CanScheduler.dataSource;
                                //var ap = dataSourceApp._data.filter(f => f.AppointmentId == AppointmentId)[0];
                                var ap = dataSourceApp._data.filter(function (f) {
                                    return f.AppointmentId == AppointmentId;
                                })[0];

                                ap.VisitId = "";
                                ap.AppointmentStatus = lastStatusName;
                                ap.SchStatusId = lastStatusId;
                                //ap.StatusColor = color;
                                PMSScheduler.CanScheduler.dataSource.pushUpdate(ap);

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () {
                },
                    'Confirm Cancel CheckIn'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    CreateLetter: function (PatientId) {
        var params = [];
        params["ParentCtrl"] = "schTabCalendar";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        params["PatientId"] = PatientId;
        LoadActionPan("SelectLetter_Template", params);
    },


    RescheduleAppointmentSearch: function (model) {
        var isProviderEditable;
        if (model.AmtCopay > 0) {
            if (model.CopayBal > 0 && model.AmtCopay != model.CopayBal) {
                isProviderEditable = false;
            }
            else if (model.CopayBal == 0) {
                isProviderEditable = false;
            } else {
                isProviderEditable = true;
            }
        } else {
            isProviderEditable = true;
        }
        var params = [];
        params["FacilityId"] = model.FacilityId;
        params["FacilityName"] = model.FacilityName;
        params["ProviderId"] = model.ProviderId;
        params["ProviderName"] = model.ProviderName;
        params["ResourceId"] = model.ResourceIds;
        //params["ResourceName"] = model.ResourceName;
        params["AppointmentId"] = model.AppointmentId;
        params["PatientId"] = model.PatientId;
        params["DayDate"] = model.AppointmentDate;
        params["ParentCtrl"] = 'schTabCalendar';
        params["isProviderEditable"] = isProviderEditable;
        params["appointmenttModel"] = model;
        LoadActionPan('Scheduling_RescheduleSearch', params);

    },

    LoadCheckIn: function (model) {

        var currentDate = ($.datepicker.formatDate('mm/dd/yy', new Date()));
        var date_ = model.AppointmentDate.split('T');
        if (new Date(date_[0]) <= new Date(currentDate)) {
            var params = [];
            params["FromAdmin"] = "0";
            params["ProviderId"] = model.ProviderId;
            params["ResourceId"] = model.ResourceIds;
            params["FacilityId"] = model.FacilityId;
            params["DayDate"] = model.AppointmentDate;
            params["AppointmentId"] = model.AppointmentId;
            params["PatientId"] = model.PatientId;
            params["ParentCtrl"] = 'schTabCalendar';
            LoadActionPan('schcheckin', params);
        }
        else {
            utility.DisplayMessages("Future Appointment can't CheckedIn", 3);
        }
    },

    OpenPatientEligibility: function (appid) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                appointmentDetail.FillAppointment(appid).done(function (response) {

                    if (response.status != false) {
                        var appointment_detail = JSON.parse(response.AppointmentFill_JSON);

                        var patientname = appointment_detail.PatientName.split(',');

                        var params = [];
                        params["FromAdmin"] = "0";
                        params["patientID"] = appointment_detail.hfpatientid;
                        params["patientAccount"] = appointment_detail.AccountNumber;
                        if (patientname.length > 0) {
                            params["patientLastName"] = patientname[1];
                            params["patientFirstName"] = patientname[0];
                        }
                        params["patientInsurancePlanId"] = appointment_detail.ddlInsurancePlan;
                        params["Provider"] = appointment_detail.txtProvider;
                        params["ProviderId"] = appointment_detail.hfProviderId;
                        params["ParentCtrl"] = 'schTabCalendar';
                        LoadActionPan('Patient_Eligibility', params);


                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    SendQuickReminder: function (model, type) {
        Scheduling_Calendar.FillPatientPreferences(model.PatientId).done(function (response) {
            if (response.status != false) {

                var patData = JSON.parse(response.PreferencesFill_JSON);

                //patientCellNo, patientHomeNo, patientWorkNo, patientGuarantorId, guarantorNumber

                var patientCellNo = patData.patientCellNo;
                var patientHomeNo = patData.patientHomeNo;
                var patientWorkNo = patData.patientWorkNo;
                var patientGuarantorId = patData.patientGuarantorId;
                var guarantorNumber = patData.guarantorNumber;
                var isGuarantorAttached = patData.chkcommnwithgrntr;
                var guarantorRelation = patData.guarantorRelationText;
                var patientName = patData.PatientLName + "," + patData.PatientFName;
                var facilityPhoneNo = model.FacilityPhoneNo.replace(/[_\W]+/g, "");

                if (isGuarantorAttached.toLowerCase() != "true" || guarantorRelation.toLowerCase() == "self") {
                    patientGuarantorId = "";
                }

                guarantorNumber = guarantorNumber.replace(/[_\W]+/g, "");

                var patientNumber = "";
                if (patientCellNo == "" && patientHomeNo == "" && patientWorkNo == "") {
                    patientNumber = "";
                } else if (patientCellNo == "" && patientHomeNo != "") {
                    patientNumber = patientHomeNo.replace(/[_\W]+/g, "");
                } else if (patientCellNo == "" && patientHomeNo == "" && patientWorkNo != "") {
                    patientNumber = patientWorkNo.replace(/[_\W]+/g, "");
                } else if (patientCellNo != "") {
                    patientNumber = patientCellNo.replace(/[_\W]+/g, "");
                }
                if (patData.chkcommnoptout == "True") {
                    utility.DisplayMessages(patientName + " has opted out for appointment reminders", 2);
                } else {
                    var params = [];
                    params["RemindersTemplateId"] = "-1";
                    params["AppointmentId"] = model.AppointmentId;
                    params["ProviderId"] = model.ProviderId;
                    params["PatientId"] = model.PatientId;
                    params["ScreenType"] = type;
                    params["patientNumber"] = patientNumber;
                    params["FacilityPhoneNo"] = facilityPhoneNo;
                    params["patientGuarantorId"] = patientGuarantorId;
                    params["PatientName"] = patientName;
                    params["PatientEmail"] = patData.EmailAddress;
                    params["ProviderName"] = model.ProviderName;
                    params["AppointmentDate"] = model.AppointmentDate.split('T')[0];
                    params["patientGuarantorNumber"] = guarantorNumber;
                    params["mode"] = "Add";
                    LoadActionPan('remindersDetail', params);
                }

            }

        });

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
    diff_minutes: function (dt2, dt1) {

        var diff = (dt2.getTime() - dt1.getTime()) / 1000;
        diff /= 60;
        return Math.abs(Math.round(diff));

    },

    LoadDayViewFromMonthView: function (date) {
        PMSScheduler.NextDate = date;
        PMSScheduler.PreviousViewType = "day";
        PMSScheduler.InitilizeScheduler();
    },

    LoadSchedulerForNavigation: function (e) {
        PMSScheduler.SchedulerMinorTickCount = $("#pnlPMSScheduler #appointmentDurationselect").val();
        PMSScheduler.SetToDayFocus(PMSScheduler.NextDate, 1, e.view, PMSScheduler.PreviousViewType);
        PMSScheduler.NextDate = e.date;
        PMSScheduler.SetToDayFocus(e.date, 2, e.view, e.view);
        PMSScheduler.NavigationEvent = e;
        var viewType = e.view;
        var scheduler = $("#scheduler").data("kendoScheduler");

        switch (viewType) {
            case "workWeek":
                PMSScheduler.MonthViewCount = 0;
                PMSScheduler.DayViewCount = 0;
                PMSScheduler.ResetNavigationFilter(viewType, e.sender._selectedViewName);
                $("#schedulerPrint").addClass("disabled");
                $("#pnlPMSScheduler #divAppointmentDurationselect").removeClass('none-poiner-event');
                PMSScheduler.fitSchedulerWidgetHeight();
                break;
            case "week":
                PMSScheduler.MonthViewCount = 0;
                PMSScheduler.DayViewCount = 0;
                PMSScheduler.ResetNavigationFilter(viewType, e.sender._selectedViewName);
                $("#schedulerPrint").addClass("disabled");
                $("#pnlPMSScheduler #divAppointmentDurationselect").removeClass('none-poiner-event');
                PMSScheduler.fitSchedulerWidgetHeight();
                break;
            case "month":
                PMSScheduler.DayViewCount = 0;
                PMSScheduler.MonthViewCount = PMSScheduler.MonthViewCount + 1;
                $("#schedulerPrint").addClass("disabled");
                $("#pnlPMSScheduler #divAppointmentDurationselect").addClass('none-poiner-event');
                PMSScheduler.ResetNavigationFilter(viewType, e.sender._selectedViewName);
                PMSScheduler.fitSchedulerWidget();
                break;
            default:
                PMSScheduler.MonthViewCount = 0;
                $("#schedulerPrint").removeClass("disabled");
                $("#pnlPMSScheduler #divAppointmentDurationselect").removeClass('none-poiner-event');
                if (PMSScheduler.PreviousViewType != "day") {
                    setTimeout(function () {
                        PMSScheduler.PreviousViewType = viewType;
                        if (PMSScheduler.backupArrayProviderIds.length != 0) {
                            $("#pnlPMSScheduler #hfProviderIds").val(PMSScheduler.backupArrayProviderIds);
                            $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").value(PMSScheduler.backupArrayProviderIds);
                        }
                        PMSScheduler.InitilizeScheduler();
                    }, 200);
                } else {
                    scheduler.dataSource.read();
                }
                break;
        }

        return true;

    },

    ResetNavigationFilter: function (viewType, oldViewtype) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        var ProviderIds = $("#pnlPMSScheduler #hfProviderIds").val();
        var ResourceIds = $("#pnlPMSScheduler #hfResourceIds").val();
        var arrayProviderIds = ProviderIds ? ProviderIds.split(',') : [];
        var arrayResourceIds = ResourceIds ? ResourceIds.split(',') : [];

        if (oldViewtype == "day")
            PMSScheduler.backupArrayProviderIds = arrayProviderIds;

        if (arrayProviderIds.length > 1) {
            $("#pnlPMSScheduler #hfProviderIds").val(arrayProviderIds[arrayProviderIds.length - 1]);
            $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").value(arrayProviderIds[arrayProviderIds.length - 1]);
            PMSScheduler.IsResourceSch = false;
        }
        if (arrayResourceIds.length > 1) {
            $("#pnlPMSScheduler #hfResourceIds").val(arrayResourceIds[arrayResourceIds.length - 1]);
            $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect").value(arrayResourceIds[arrayResourceIds.length - 1]);
            PMSScheduler.IsResourceSch = true;
        }
        if ((arrayProviderIds.length > 1 || arrayResourceIds.length > 1)) {
            PMSScheduler.PreviousViewType = viewType;
            PMSScheduler.InitilizeScheduler();
        }
        else
            if (viewType == 'month') {
                if (PMSScheduler.MonthViewCount <= 1) {
                    setTimeout(function () {
                        PMSScheduler.PreviousViewType = viewType;
                        PMSScheduler.InitilizeScheduler();
                    }, 200);
                } else {
                    if (PMSScheduler.Counter > 0) {
                        setTimeout(function () {
                            scheduler.dataSource.read();
                        }, 200);
                    }
                }
            } else if (viewType == "workWeek") {
                if (PMSScheduler.Counter > 0) {
                    setTimeout(function () {
                        scheduler.dataSource.read();
                    }, 200);
                }
                else {
                    setTimeout(function () {
                        scheduler.dataSource.read();
                    }, 200);
                }
            }
            else if (viewType == "week") {
                if (PMSScheduler.Counter > 0) {
                    setTimeout(function () {
                        scheduler.dataSource.read();
                    }, 200);
                }
                else {
                    setTimeout(function () {
                        scheduler.dataSource.read();
                    }, 200);
                }
            }
    },
    LoadScheduler: function (e) {
        var defObject = $.Deferred();
        PMSScheduler.SchdefObject = defObject;
        var scheduler = $("#scheduler").data("kendoScheduler");
        PMSScheduler.PreviousViewType = scheduler._selectedViewName;
        PMSScheduler.LoadScheduler_DBCall(PMSScheduler.PreviousViewType).done(function (response) {
            if (response.status != false) {
                e.success(response.ProviderScheduleFill_JSON);

                navigationJSON = response.ScheduleSlotsFill_JSON;
                WorkWeekSchedulesJSON = response.WorkWeekSchedules;
                gblockHoursCount = response.BlockHoursCount;
                gblockHoursJSON = response.BlockHoursLoad_JSON != undefined ? JSON.parse(response.BlockHoursLoad_JSON) : null;

                defObject.resolve();
                var allEvents = scheduler.element ? scheduler.element.find("div.k-event") : [];
                if (scheduler._selectedViewName && scheduler._selectedViewName == "month") {
                    if (scheduler && scheduler.dataSource && scheduler.dataSource._data && scheduler.dataSource._data.length > 0) {
                        setTimeout(function () {
                            $('#pnlPMSScheduler .k-scheduler-header-wrap').find('.k-slot-cell').html(scheduler.dataSource._data[0].ProviderName + " <span class='badge' style='color:white;background: #D2312D;'>" + response.ProviderScheduleFill_JSON.length + "</span>");
                        }, 300);
                    } else {
                        $('#pnlPMSScheduler .k-scheduler-header-wrap').find('.k-slot-cell').html(scheduler.resources[1] ? scheduler.resources[1].dataSource.options.data[0].text : "" + " <span class='badge' style='color:white;background: #D2312D;'></span>");
                    }
                } else if (scheduler._selectedViewName && (scheduler._selectedViewName == "week" || scheduler._selectedViewName == "workWeek")) {
                    if (scheduler && scheduler.dataSource && scheduler.dataSource._data && scheduler.dataSource._data.length > 0) {
                        setTimeout(function () {
                            $('#pnlPMSScheduler .k-scheduler-header-wrap').find('.k-slot-cell').html(scheduler.dataSource._data[0].ProviderName + " <span class='badge' style='color:white;background: #D2312D;'>" + allEvents.length + "</span>");
                        }, 300);
                    } else {
                        $('#pnlPMSScheduler .k-scheduler-header-wrap').find('.k-slot-cell').html(scheduler.resources[1] ? scheduler.resources[1].dataSource.options.data[0].text : "" + " <span class='badge' style='color:white;background: #D2312D;'></span>");
                    }
                } else if (scheduler._selectedViewName && scheduler._selectedViewName == "day") {
                    if (scheduler._workDayMode) {
                        var resourceArry = scheduler.resources[1].dataSource.options.data;
                        var names = $('#pnlPMSScheduler .k-scheduler-header-wrap').find('.k-slot-cell');
                        var allEvents = scheduler.element.find("div.k-event");
                        var count = 0;
                        var prvIndex = null;
                        $.each(allEvents, function (r, evt) {
                            var slot = scheduler.slotByElement(allEvents[r]);
                            if (prvIndex == slot.groupIndex || prvIndex == null) {
                                count++;
                            } else if (prvIndex != slot.groupIndex) {
                                $(names[prvIndex]).html(resourceArry[prvIndex].text + " <span class='badge style='color:white;background: #D2312D;'>" + count + "</span>");
                                count = 1;
                            }
                            prvIndex = slot.groupIndex;

                        });
                        if (resourceArry[prvIndex] != null) {
                            $(names[prvIndex]).html(resourceArry[prvIndex].text + " <span class='badge style='color:white;background: #D2312D;'>" + count + "</span>");
                        }
                        PMSScheduler.setProviderScheduleBaseHours(response);
                    }
                    else {
                        var counts = null;
                        var names = $('#pnlPMSScheduler .k-scheduler-header-wrap').find('.k-slot-cell');
                        $.each(names, function (i) {
                            var resourceArry = scheduler.resources[1].dataSource.options.data;
                            var resource = resourceArry[i];
                            if (resource) {
                                counts = $.grep(response.ProviderScheduleFill_JSON, function (v) {
                                    return v.ProviderId == resource.value;
                                });
                                if (counts && counts.length > 0) {
                                    $(names[i]).html(resource.text + " <span class='badge style='color:white;background: #D2312D;'>" + counts.length + "</span>");
                                } else {
                                    $(names[i]).html(resource.text + " <span class='badge style='color:white;background: #D2312D;'></span>");
                                }
                            }

                        });
                        PMSScheduler.setProviderScheduleBaseHours(response);

                    }
                }

                PMSScheduler.LoadSchBlockHours();
                // PMSScheduler.drawBusinessHours(scheduler);
            }

        });
        return defObject;
    },
    setProviderScheduleBaseHours: function (response) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        if (scheduler.resources.length < 2) {
            return;
        }
        if (scheduler && scheduler._selectedViewName) {
            if (scheduler._selectedViewName != "month") {
                PMSScheduler.ResetAllSlotSlotsColor(scheduler);
            }
        }
        var resourceArry = scheduler.resources[1].dataSource.options.data;
        var contentDiv = scheduler.element.find("div.k-scheduler-content");
        var rows = contentDiv.find("tr");
        if (response.status != false) {

            for (var i = 0; i < rows.length; i++) {
                var tds = $(rows[i]).find('td');
                for (var s = 0; s < tds.length; s++) {
                    var slot = scheduler.slotByElement(tds[s]);
                    if (scheduler._selectedViewName == "day" || scheduler._selectedViewName == "workWeek") {
                        slot = $(slot.element)[0];
                        if ($(slot).attr('isBlock') != 'true') {
                            $(slot).addClass('k-nonwork-hour');
                            $(slot).css('background-color', '');
                        }
                    }
                }
            }
            if (response.ScheduleSlotsFill_JSON != null && response.ScheduleSlotsFill_JSON.length > 0) {
                $.each(response.ScheduleSlotsFill_JSON, function (ind, item) {
                    var sDate = item.AppointmentDate;
                    sDate = utility.RemoveTimeFromDate(null, sDate);
                    var tFrom = item.TimeFrom;
                    var tTo = item.TimeTo;
                    var dbProviderId = item.ProviderId;

                    for (var i = 0; i < rows.length; i++) {
                        var tds = $(rows[i]).find('td');
                        for (var s = 0; s < tds.length; s++) {
                            var slot = scheduler.slotByElement(tds[s]);
                            var slotStart = kendo.toString(slot.startDate, "s");
                            var ProviderId = resourceArry[slot.groupIndex].value;
                            if (scheduler._selectedViewName == "day" || scheduler._selectedViewName == "workWeek") {
                                var blockFrom = kendo.toString(new Date(sDate + " " + tFrom), "s");
                                var blockTo = kendo.toString(new Date(sDate + " " + tTo), "s");
                                if (Date.parse(slotStart) >= Date.parse(blockFrom) && Date.parse(slotStart) < Date.parse(blockTo) && ProviderId == item.ProviderId) {
                                    slot = $(slot.element)[0];
                                    if ($(slot).attr('isBlock') != 'true') {
                                        $(slot).css('background-color', 'white');
                                        //$(slot).addClass('k-today');
                                    }

                                }

                            }
                        }
                    }
                });
            } else {
                for (var i = 0; i < rows.length; i++) {
                    var tds = $(rows[i]).find('td');
                    for (var s = 0; s < tds.length; s++) {
                        var slot = scheduler.slotByElement(tds[s]);
                        var slotStart = kendo.toString(slot.startDate, "s");
                        var ProviderId = resourceArry[slot.groupIndex].value;
                        if (scheduler._selectedViewName == "day" || scheduler._selectedViewName == "workWeek") {
                            slot = $(slot.element)[0];
                            if ($(slot).attr('isBlock') != 'true') {
                                $(slot).addClass('k-nonwork-hour');
                            }
                        }
                    }
                }
            }
        }


    },
    ResetAppointmentCount: function () {
        var scheduler = $("#scheduler").data("kendoScheduler");
        if (scheduler && scheduler._selectedViewName && (scheduler._selectedViewName == "month" || scheduler._selectedViewName == "week" || scheduler._selectedViewName == "workWeek")) {
            if (scheduler && scheduler._data && scheduler._data.length > 0) {
                $('#pnlPMSScheduler .k-scheduler-header-wrap').find('.k-slot-cell').html(scheduler._data[0].ProviderName + " <span class='badge style='color:white;background: #D2312D;'>" + scheduler._data.length + "</span>");
            }
            else {
                $('#pnlPMSScheduler .k-scheduler-header-wrap').find('.k-slot-cell').html((scheduler.resources.length > 1 ? scheduler.resources[1].dataSource.options.data[0].text : '') + " <span class='badge style='color:white;background: #D2312D;'></span>");
            }
        } else if (scheduler && scheduler._selectedViewName && scheduler._selectedViewName == "day") {
            PMSScheduler.calculatAppointments(scheduler);
        }
    },

    OpenEligibility: function (AppointmentId, EligibilityStatus, PatientId, ProviderName, ProviderId, PatientAccountNo, PatientLastName, PatientFirstName, PatientInsuranceId, InsurancePlanId) {
        if (EligibilityStatus && EligibilityStatus != null && (EligibilityStatus == "Active" || EligibilityStatus == "Inactive")) {
            PMSScheduler.EligibilityId_DBCall(PatientInsuranceId, AppointmentId, PatientId, EligibilityStatus).done(function (response) {
                if (response.status != false) {
                    if (response.EDIEligibilityId > 0) {
                        Patient_Eligibility.OpenEligibilityDetailSch(response.EDIEligibilityId);
                    } else {
                        utility.DisplayMessages("Record not found.", 3);
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        } else {
            Patient_Insurance.OpenPatientEligibilityFromSch(PatientId, ProviderName, ProviderId, PatientAccountNo, PatientLastName, PatientFirstName, PatientInsuranceId);
        }
    },

    UpdateFacility: function (checked) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        scheduler.dataSource.filter({
            operator: function (task) {
                return $.inArray(task.FacilityId, checked) >= 0;
            }
        });
    },
    UpdateProviders: function (checked) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        scheduler.dataSource.filter({
            operator: function (task) {
                return $.inArray(task.ProviderId, checked) >= 0;
            }
        });
    },
    scrollToHour: function (hour) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        var time = PMSScheduler.setCustomHour(hour, "0");
        if (scheduler) {
            var contentDiv = scheduler.element.find("div.k-scheduler-content");
            var rows = contentDiv.find("tr");

            for (var i = 0; i < rows.length; i++) {
                var slot = scheduler.slotByElement(rows[i]);
                if (slot) {
                    var slotTime = kendo.toString(slot.startDate, "HH:mm");
                    var targetTime = kendo.toString(time, "HH:mm");

                    if (targetTime === slotTime) {
                        scheduler.view()._scrollTo($(rows[i]).find("td:first")[0], contentDiv[0]);
                    }
                }

            };
        }
    },

    SetCurrentHour: function () {
        var datt = new Date();
        var currentHour = datt.getHours();
        setTimeout(function () {
            PMSScheduler.scrollToHour(currentHour)
        }, 1000);
    },
    setCustomHour: function (hour, minut) {
        var datt = new Date();
        datt.setHours(hour);
        datt.setMinutes(minut);
        datt.setSeconds(0);
        datt.setMilliseconds(0);
        return datt;
    },
    InitilizeAppointmentToolTip: function () {
        PMSScheduler.SchedulerTooltip = $("#scheduler").kendoTooltip({
            filter: ".k-event:not(.k-event-drag-hint) > div .PatientNameTooltip, .k-task",
            position: "top",
            width: 426,
            animation: {
                open: {
                    duration: 150
                }
            },
            content: kendo.template($('#tooltipTemplate').html())
        }).data("kendoTooltip");
        var wwidth = "110%";

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        function changeIcon(e) {
            for (var i = 0; i < e.length; i++) {
                if ($(e[i]).parent().prev().hasClass('active') == true) {
                    $(e[i]).html('');
                    $(e[i]).html('<i class="fa fa-angle-up"></i>');
                }
                else {
                    $(e[i]).html('');
                    $(e[i]).html('<i class="fa fa-angle-down"></i>');
                };
            }
        }

        $('.splitterBtn').html('<a></a>');
        changeIcon($('.splitterBtn a'));

        $('.extrnalUrl').click(function (e) {
            $('.extrnalUrl').html("");
            var cokeUrl = "http://cokestudio.com.pk/season8/";
            $.get(cokeUrl, function (respo) {
                alert(respo);
            })
        });



        $(document).ready(function () {

            $('html').click(function () {
                $(".azhr.dropdown").removeClass("open");
            });

            //splitter code
            $('.splitterBtn a').click(function (e) {
                $(this).parent('.splitterBtn').prev().slideToggle(250).toggleClass('active');
                var a = $(this);
                changeIcon(a);
            });



            $(function () {
                $('[data-plugin-toggle]').each(function () {
                    var $this = $(this),
                        opts = {
                        };

                    var pluginOptions = $this.data('plugin-options');
                    if (pluginOptions)
                        opts = pluginOptions;

                    $this.themePluginToggle(opts);
                });
            });


            $('#menu-toggle').click(function (e) {
                e.preventDefault();
                $('#wrapper').toggleClass('toggled');
            });
            $('.initialVisit .editBtn').click(function (e) {
                $(this).parent().next().toggleClass('hidden');
            });



            //initialVisit JS
            $('.initialVisit li section').mouseenter(function (e) {
                $(this).children('div.pull-right').toggleClass('hidden');
            });
            $('.initialVisit li section').mouseleave(function (e) {
                $(this).children('div.pull-right').toggleClass('hidden');
            });

            $('.initialVisit li header').mouseenter(function (e) {
                $(this).children('.closeBtn').toggleClass('hidden');
                $(this).css('background', '#EAF1F8');
            });
            $('.initialVisit li header').mouseleave(function (e) {
                $(this).children('.closeBtn').toggleClass('hidden');
                $(this).css('background', 'none');
            });
            //end of initialVisit JS

        });
    },

    InitilizeSearchFilters: function () {

        var methodName = ['GetProviderWithQualification', 'GetResourcesWithDescription', 'GetFacility', 'GetPatientVisitType', 'GetAppointmentStatus'];
        var dfd = new $.Deferred();
        BackgroundLoaderShow(true);
        MDVisionService.lookups(methodName, false, "").done(function (results) {

            var providers = JSON.parse(results['GetProviderWithQualification']);
            var resourses = JSON.parse(results['GetResourcesWithDescription']);
            var facility = JSON.parse(results['GetFacility']);
            var visitType = JSON.parse(results['GetPatientVisitType']);
            var status = JSON.parse(results['GetAppointmentStatus']);

            var providers_ = [];
            var defaultProvider = null;
            $.each(providers, function (j, result) {
                if (result.Name != '- Select -') {
                    providers_.push({
                        id: result.Value, name: result.Name
                    });
                    if (result.Value == globalAppdata.DefaultProviderId)
                        defaultProvider = {
                            id: result.Value, name: result.Name
                        };
                }

            });

            var resourses_ = [];
            var defaultResource = null;
            $.each(resourses, function (j, result) {
                if (result.Name != '- Select -') {
                    resourses_.push({
                        id: result.Value, name: result.Name
                    });
                    if (result.Value == globalAppdata.DefaultResourceId)
                        defaultResource = {
                            id: result.Value, name: result.Name
                        };
                }

            });
            PMSScheduler.providerDataSource = providers_;
            //ExValue to show Description of facility
            var facility_ = [];
            var defaultFacility = null;
            var AllSelected = null;
            $.each(facility, function (j, result) {

                //var facilityName = "";
                //if (globalAppdata["IsShowFacilityShortName"].toLowerCase() == "true")
                //    facilityName = result.Name + " " + result.ExValue;

                //else
                //    facilityName = result.ExValue;

                if (result.Name == '- Select -') {
                    result.ExValue = "All";
                    AllSelected = {
                        id: result.Value, name: result.ExValue, color: result.Title, refval: result.Name
                    };
                }

                facility_.push({
                    id: result.Value, name: result.ExValue, color: result.Title, refval: (result.Name == '- Select -') ? "" : result.Name
                });

                if (result.ExValue != "All" && result.Value == globalAppdata.DefaultFacilityId)
                    defaultFacility = {
                        id: result.Value, name: result.ExValue, color: result.Title, refval: result.Name
                    };

            });
            PMSScheduler.facilityDataSource = facility_;
            var patientType_ = [];
            $.each(JSON.parse(StaticLookups.GetPatientType), function (j, result) {
                if (result.Name == '- Select -')
                    result.Name = "Select Patient Type";
                patientType_.push({
                    id: result.Value, name: result.Name
                });

            });

            var visitType_ = [];
            $.each(visitType, function (j, result) {
                if (result.Name != '- Select -')
                    visitType_.push({
                        id: result.Value, name: result.Name, type: result.RefValue
                    });
            });

            var status_ = [];
            $.each(status, function (j, result) {
                if (result.Name != '- Select -')
                    status_.push({
                        id: result.Value, name: result.Name, color: result.RefValue
                    });
            });
            if (!(status_[0].name == "All")) {
                status_.splice(0, 0, { id: "0", name: "All", color: "" });
            }
            PMSScheduler.scheduleStatusDataSource = status_;
            //Destroy Existing Controls
            PMSScheduler.DestroyKendoFilters();

            //Provider  
            $("#pnlPMSScheduler #providerMultiselect").kendoMultiSelect({
                dataSource: providers_,
                dataTextField: "name",
                dataValueField: "id",
                noDataTemplate: 'No Data!',
                placeholder: "Select Provider",
                autoClose: false,
                height: 400,
                tagTemplate: kendo.template($("#tagProviderTemplate").html()),
                tagMode: "single",
                change: function (e) {
                    var values_ = $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").value();
                    PMSScheduler.MakeFilertQuery(e, values_, "provider");
                },
                dataBound: function (e) {
                    var ms = $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect");
                    ms.input.attr("disabled", "disabled");
                },
                open: function () {

                    setTimeout(function () {
                        if ($("#providerMultiselect_listbox li").length >= 20) {
                            $("#providerMultiselect_listbox").css("height", "100%");
                            $("#providerMultiselect_listbox").parent().css("height", "100%");
                            $("#providerMultiselect_listbox").parent().parent().css("height", "100%");
                            $("#providerMultiselect-list").css("height", $(window).height() - 210 + "px");
                            $("#providerMultiselect-list").parent().css("height", $(window).height() - 210 + "px");
                        }
                        $("#providerMultiselect-list").parent().addClass("SchdlCustomControls");

                    }, 100);


                },
            });

            //Resource
            $("#pnlPMSScheduler #resourceMultiselect").kendoMultiSelect({
                dataSource: resourses_,
                dataTextField: "name",
                dataValueField: "id",
                placeholder: "Select Resource",
                autoClose: false,
                height: 400,
                tagTemplate: kendo.template($("#tagResourceTemplate").html()),
                tagMode: "single",
                change: function (e) {
                    var values_ = $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect").value();
                    PMSScheduler.MakeFilertQuery(e, values_, "resource");
                },
                dataBound: function (e) {
                    var ms = $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect");
                    ms.input.attr("disabled", "disabled");
                },
                open: function () {

                    setTimeout(function () {
                        if ($("#resourceMultiselect_listbox li").length >= 20) {
                            $("#resourceMultiselect_listbox").css("height", "100%");
                            $("#resourceMultiselect_listbox").parent().css("height", "100%");
                            $("#resourceMultiselect_listbox").parent().parent().css("height", "100%");
                            $("#resourceMultiselect-list").css("height", $(window).height() - 210 + "px");
                            $("#resourceMultiselect-list").parent().css("height", $(window).height() - 210 + "px");
                        }
                        $("#resourceMultiselect-list").parent().addClass("SchdlCustomControls");

                    }, 100);


                }
            });

            //Facility
            $("#pnlPMSScheduler #facilityMultiselect").kendoMultiSelect({
                dataSource: facility_,
                dataTextField: "name",
                dataValueField: "id",
                placeholder: "Facility",
                itemTemplate: (globalAppdata["IsShowFacilityShortName"].toLowerCase() == "true") ? '<span class="k-state-default" style="background-color:#:data.color#"></span>' +
                 '<span class="k-state-default"><p><div class="text-uppercase">#: data.refval #</div><small> #: data.name # </small></p></span>' : '<span class="k-state-default" style="background-color:#:data.color#"></span>' +
                 '<span class="k-state-default"><p> #: data.name # </p></span>',
                footerTemplate: 'Total #: instance.dataSource.total() # items found',
                tagTemplate: kendo.template($("#tagFacilityTemplate").html()),
                tagMode: "single",
                height: 400,
                autoClose: false,
                change: function (e) {
                    var values_ = $("#pnlPMSScheduler #facilityMultiselect").data("kendoMultiSelect").value();
                    var sitem = $.grep(values_, function (itemm) {
                        return itemm == ""
                    });
                    if ((values_.indexOf("") == values_.length - 1) && sitem.length > 0) {
                        //unselect all except All option.
                        values_ = [];
                        setTimeout(function () {
                            $("#facilityMultiselect_taglist span:first").html("All Facility(s) Selected");
                        }, 100);
                        $("#pnlPMSScheduler #facilityMultiselect").data("kendoMultiSelect").value(sitem);
                    }
                    else if (sitem.length > 0) {
                        values_.splice(values_.indexOf(""), 1);
                        $("#pnlPMSScheduler #facilityMultiselect").data("kendoMultiSelect").value(values_);
                    }
                    PMSScheduler.MakeFilertQuery(e, values_, "facility");

                },
                open: function () {

                    setTimeout(function () {
                        if ($("#facilityMultiselect_listbox li").length >= 20) {
                            $("#facilityMultiselect_listbox").css("height", "100%");
                            $("#facilityMultiselect_listbox").parent().css("height", "100%");
                            $("#facilityMultiselect_listbox").parent().parent().css("height", "100%");
                            $("#facilityMultiselect-list").css("height", $(window).height() - 210 + "px");
                            $("#facilityMultiselect-list").parent().css("height", $(window).height() - 210 + "px");
                        }
                        $("#facilityMultiselect-list").parent().addClass("SchdlCustomControls");
                    }, 100);


                },
                dataBound: function (e) {
                    var ms = $("#pnlPMSScheduler #facilityMultiselect").data("kendoMultiSelect");
                    ms.input.attr("disabled", "disabled");
                }
            });

            //Patient Type
            $("#pnlPMSScheduler #patientTypeSingleselect").kendoDropDownList({
                dataSource: {
                    data: patientType_
                },
                dataTextField: "name",
                dataValueField: "id",
                height: 400,
                select: function (e) {

                    var item = e.item;
                    var visitType = $("#pnlPMSScheduler #visitTypeSingleselect").data("kendoMultiSelect");
                    var visitList = visitType.dataSource.data();

                    if (item.text() == "Select Patient Type") {

                        var typeids = [];
                        $.each(visitList, function (i, item) {
                            typeids.push(item.id);
                        });
                        visitType.value(typeids);
                        $("#pnlPMSScheduler #hfVisitTypeIds").val(typeids.join(","));
                        visitType.enable(false);
                    }
                    else {

                        var sitem = $.grep(e.sender.dataSource.data(), function (itemm) {
                            return itemm.name == item.text()
                        });

                        var filtered = [];
                        $.each(visitList, function (i, itemm) {

                            if (sitem[0].id == itemm.type) {
                                filtered.push(itemm);
                            }

                        });

                        $("#visitTypeSingleselect_listbox li").each(function (i, ittem) {

                            var ismatch = $.grep(filtered, function (itemm) {
                                return $(ittem).html() == itemm.name;
                            });
                            if (ismatch.length > 0)
                                $(ittem).css("display", "block");
                            else
                                $(ittem).css("display", "none");

                        });

                        $("#pnlPMSScheduler #visitTypeSingleselect").data("kendoMultiSelect").value([]);
                        $("#pnlPMSScheduler #hfVisitTypeIds").val("");
                        $("#pnlPMSScheduler #visitTypeSingleselect").data("kendoMultiSelect").enable(true);
                    }
                },
                change: function (e) {

                    var value_ = $("#pnlPMSScheduler #patientTypeSingleselect").data("kendoDropDownList").value();
                    PMSScheduler.MakeFilertQuery(e, value_, "patientType");

                },
                open: function () {
                    setTimeout(function () {
                        $("#patientTypeSingleselect-list").parent().addClass("SchdlCustomControls");
                    }, 100);
                }
            });

            //Visit Type
            $("#pnlPMSScheduler #visitTypeSingleselect").kendoMultiSelect({
                dataSource: visitType_,
                dataTextField: "name",
                dataValueField: "id",
                placeholder: "Visit Type",
                height: "auto",
                //maxSelectedItems: 3,
                tagTemplate: kendo.template($("#tagVisitTypeTemplate").html()),
                tagMode: "single",
                enable: false,
                change: function (e) {
                    var values_ = $("#pnlPMSScheduler #visitTypeSingleselect").data("kendoMultiSelect").value();
                    PMSScheduler.MakeFilertQuery(e, values_, "visitType");
                },
                dataBound: function (e) {
                    var ms = $("#pnlPMSScheduler #visitTypeSingleselect").data("kendoMultiSelect");
                    ms.input.attr("disabled", "disabled");
                },
                open: function () {
                    setTimeout(function () {
                        if ($("#visitTypeSingleselect_listbox li[style='display: block;'] ").length >= 20) {
                            $("#visitTypeSingleselect_listbox").css("height", "100%");
                            $("#visitTypeSingleselect_listbox").parent().css("height", "100%");
                            $("#visitTypeSingleselect_listbox").parent().parent().css("height", "100%");
                            $("#visitTypeSingleselect-list").css("height", $(window).height() - 210 + "px");
                            $("#visitTypeSingleselect-list").parent().css("height", $(window).height() - 210 + "px");
                        }
                        $("#visitTypeSingleselect-list").parent().addClass("SchdlCustomControls");
                    }, 100);
                }
            });

            //Appointment Status
            $("#pnlPMSScheduler #statusSingleselect").kendoMultiSelect({
                dataSource: status_,
                dataTextField: "name",
                dataValueField: "id",
                placeholder: "Status",
                height: 400,
                itemTemplate: '<span class="k-state-default"><p>#: data.name #</p></span>'
                + '<span class="k-state-default" style="background-color:#:data.color#"></span>',
                footerTemplate: 'Total #: instance.dataSource.total() # items found',
                tagTemplate: kendo.template($("#tagStatusTemplate").html()),
                tagMode: "single",
                autoClose: false,
                change: function (e) {
                    var values_ = $("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect").value();
                    var sitem = $.grep(values_, function (itemm) {
                        return itemm == "0"
                    });
                    if ((values_.indexOf("0") == values_.length - 1) && sitem.length > 0) {
                        //unselect all except All option.
                        values_ = [];
                        setTimeout(function () {
                            $("#statusSingleselect_taglist span:first").html("All Status(es) Selected");
                        }, 100);
                        $("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect").value(sitem);
                    }
                    else if (sitem.length > 0) {
                        values_.splice(values_.indexOf("0"), 1);
                        $("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect").value(values_);
                    }
                    PMSScheduler.MakeFilertQuery(e, values_, "status");



                    //var values_ = $("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect").value();
                    //PMSScheduler.MakeFilertQuery(e, values_, "status");
                },
                dataBound: function (e) {
                    var ms = $("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect");
                    ms.input.attr("disabled", "disabled");
                },
                open: function () {
                    setTimeout(function () {
                        $("#statusSingleselect-list").parent().addClass("SchdlCustomControls");
                    }, 100);
                }
            });

            //Set Default Values;
            if (defaultProvider) {
                $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").value(defaultProvider);
                $("#pnlPMSScheduler #hfProviderIds").val(defaultProvider.id);
            }
            else if (defaultResource) {
                $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect").value(defaultResource);
                $("#pnlPMSScheduler #hfResourceIds").val(defaultResource.id);
                PMSScheduler.IsResourceSch = true;
            }
            if (defaultFacility) {
                $("#pnlPMSScheduler #facilityMultiselect").data("kendoMultiSelect").value(defaultFacility);
                $("#pnlPMSScheduler #hfFacilityIds").val(defaultFacility.id);
            }
            else {
                $("#pnlPMSScheduler #facilityMultiselect").data("kendoMultiSelect").value(AllSelected);
                $("#facilityMultiselect_taglist span:first").html("All Facility(s) Selected");
                $("#pnlPMSScheduler #hfFacilityIds").val("");
            }
            var ids = [];
            $.each(status_, function (i, item) {
                if (item.name.toLowerCase() != "cancel" && item.name.toLowerCase() != "reschedule" && item.name.toLowerCase() != "all")
                    ids.push(item.id);
            });
            $("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect").value(ids);
            $("#pnlPMSScheduler #hfAppointmentStatusIds").val(ids.join(","));

            // By default select All patient Visit Types
            var typeids = [];
            $.each(visitType_, function (i, item) {
                typeids.push(item.id);
            });
            $("#pnlPMSScheduler #visitTypeSingleselect").data("kendoMultiSelect").value(typeids);
            $("#pnlPMSScheduler #hfVisitTypeIds").val(typeids.join(","));

            BackgroundLoaderShow(false);
            dfd.resolve();
        });

        var AppointmentDuration = [
        {
            id: 12, name: '5 min', color: ''
        },
        {
            id: 6, name: '10 min', color: ''
        },
        {
            id: 4, name: '15 min', color: ''
        },
        {
            id: 3, name: '20 min', color: ''
        },
        {
            id: 2, name: '30 min', color: ''
        },
        {
            id: 1, name: '60 min', color: ''
        },
        ];

        $("#pnlPMSScheduler #appointmentDurationselect").kendoDropDownList({
            dataSource: AppointmentDuration,
            dataTextField: "name",
            dataValueField: "id",
            index: 0,
            change: function (e) {
                var widget = $("#scheduler").data("kendoScheduler");
                widget.setOptions({
                    minorTickCount: 1,
                    minorTick: PMSScheduler.getCurrentTick(this.dataItem().id),
                    majorTick: PMSScheduler.getCurrentTick(this.dataItem().id)
                });
                widget.view(widget.view().name);
                //  $('.k-scheduler-header-wrap .k-scheduler-table tr:first th').addClass('text-center bg-primary');               
                PMSScheduler.LoadSchBlockHours();
                PMSScheduler.ResetAppointmentCount();
                PMSScheduler.ZoomINCount = 0;
            },
            open: function () {
                setTimeout(function () {
                    $("#appointmentDurationselect-list").parent().addClass("SchdlCustomControls");
                }, 100);
            }
        });
        return dfd;

    },

    InitializeResources: function () {

        var hfFacilityIds = $("#pnlPMSScheduler #hfFacilityIds").val() ? $("#pnlPMSScheduler #hfFacilityIds").val().split(",") : null;
        var SchedulerResource = [];
        //if (hfFacilityIds) {
        var facility_ = {
            field: "FacilityId", name: "facilities", dataSource: []
        }
        var facility_obj = $("#pnlPMSScheduler #facilityMultiselect").data("kendoMultiSelect");
        var data_ = facility_obj.dataSource.data();
        for (var i = 0; i < data_.length; i++) {
            facility_.dataSource.push({
                text: data_[i].name, value: data_[i].id, color: data_[i].color
            });
        }
        SchedulerResource.push(facility_);
        //}

        var hfProviderIds = $("#pnlPMSScheduler #hfProviderIds").val() ? $("#pnlPMSScheduler #hfProviderIds").val().split(",") : null;
        var hfResourceIds = $("#pnlPMSScheduler #hfResourceIds").val() ? $("#pnlPMSScheduler #hfResourceIds").val().split(",") : null;

        if (hfProviderIds) {
            var provider_ = {
                field: "ProviderId", name: "providers", dataSource: []
            }
            var provider_obj = $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect");
            for (var i = 0; i < hfProviderIds.length; i++) {
                if (hfProviderIds[i]) {
                    var obj_ = $.grep(provider_obj.dataSource.data(), function (a) {
                        return a.id == hfProviderIds[i]
                    });
                    if (obj_.length > 0)
                        provider_.dataSource.push({
                            text: obj_[0].name, value: obj_[0].id, groupType: 'Provider'
                        });
                }
            }
            SchedulerResource.push(provider_);
            $("#pnlPMSScheduler #hfResourceIds").val("");
        } else if (hfResourceIds) {
            var provider_ = {
                field: "ProviderId", name: "providers", dataSource: []
            }
            var resource_obj = $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect");
            for (var i = 0; i < hfResourceIds.length; i++) {
                if (hfResourceIds[i]) {
                    var obj_ = $.grep(resource_obj.dataSource.data(), function (a) {
                        return a.id == hfResourceIds[i]
                    });
                    if (obj_.length > 0)
                        provider_.dataSource.push({
                            text: obj_[0].name, value: obj_[0].id, groupType: 'Resource'
                        });
                }
            }
            SchedulerResource.push(provider_);
            $("#pnlPMSScheduler #hfProviderIds").val("");
        }

        return SchedulerResource;
    },


    MakeFilertQuery: function (event, filterValues, filterName) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        if (filterName == "provider") {

            if (PMSScheduler.PreviousViewType == "month" || PMSScheduler.PreviousViewType == "week" || PMSScheduler.PreviousViewType == "workWeek") {
                if (filterValues.length > 1) {
                    $("#pnlPMSScheduler #hfProviderIds").val(filterValues[filterValues.length - 1]);
                    $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").value(filterValues[filterValues.length - 1]);
                } else {
                    $("#pnlPMSScheduler #hfProviderIds").val(filterValues.join(','));
                }
            } else {
                $("#pnlPMSScheduler #hfProviderIds").val(filterValues.join(','));
            }
            //$("#pnlPMSScheduler #hfProviderIds").val(filterValues.join(','));
            $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect").value([]);
            $("#pnlPMSScheduler #hfResourceIds").val("");
            PMSScheduler.IsResourceSch = false;
        }
        else if (filterName == "resource") {

            if (PMSScheduler.PreviousViewType == "month" || PMSScheduler.PreviousViewType == "week" || PMSScheduler.PreviousViewType == "workWeek") {
                if (filterValues.length > 1) {
                    $("#pnlPMSScheduler #hfResourceIds").val(filterValues[filterValues.length - 1]);
                    $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect").value(filterValues[filterValues.length - 1]);
                } else {
                    $("#pnlPMSScheduler #hfResourceIds").val(filterValues.join(','));
                }
            } else {
                $("#pnlPMSScheduler #hfResourceIds").val(filterValues.join(','));
            }
            $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").value([]);
            $("#pnlPMSScheduler #hfProviderIds").val("");
            PMSScheduler.IsResourceSch = true;

            //Resource
            PMSScheduler.ResourcesResource;
        }
        else if (filterName == "facility") {
            $("#pnlPMSScheduler #hfFacilityIds").val(filterValues.join(','));

            //Facility
            PMSScheduler.FacilityResource;
        }

        else if (filterName == "patientType")
            $("#pnlPMSScheduler #hfPatientTypeId").val(filterValues);
        else if (filterName == "visitType")
            $("#pnlPMSScheduler #hfVisitTypeIds").val(filterValues.join(','));
        else if (filterName == "status")
            $("#pnlPMSScheduler #hfAppointmentStatusIds").val(filterValues.join(','));


        // if (filterName == "provider" || filterName == "resource")
        PMSScheduler.InitilizeScheduler();
        //else {
        //    scheduler.dataSource.read();
        //    PMSScheduler.ResetAppointmentCount();
        //}

    },

    DestroyKendoFilters: function () {

        try {
            var control_ = null;
            control_ = $("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect");
            if (control_) {
                control_.destroy();
                control_.html("");
            }

            control_ = $("#pnlPMSScheduler #visitTypeSingleselect").data("kendoMultiSelect");
            if (control_) {
                control_.destroy();
                control_.html("");
            }

            control_ = $("#pnlPMSScheduler #patientTypeSingleselect").data("kendoDropDownList");
            if (control_) {
                control_.destroy();
                control_.html("");
            }

            control_ = $("#pnlPMSScheduler #facilityMultiselect").data("kendoMultiSelect");
            if (control_) {
                control_.destroy();
                control_.html("");
            }

            control_ = $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect");
            if (control_) {
                control_.destroy();
                control_.html("");
            }

            control_ = $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect");
            if (control_) {
                control_.destroy();
                control_.html("");
            }

        } catch (e) {

        }

    },
    addAppointment: function () {
        Scheduling_Calendar.AppointmentAddNew("Add", 0);
    },
    addZero: function (i) {
        if (i < 10)
            i = "0" + i;
        return i;
    },
    getDefaultDate: function () {
        var d = new Date();
        var day = PMSScheduler.addZero(d.getDate());
        var month = PMSScheduler.addZero(d.getMonth() + 1);
        var year = PMSScheduler.addZero(d.getFullYear());
        return day + "/" + month + "/" + year;
    },
    getKendoSchDate: function () {
        var date = PMSScheduler.getDefaultDate();
        var scheduler = $("#scheduler").data("kendoScheduler");
        if (scheduler && scheduler._selectedViewName.toLowerCase() == "month")
            date = scheduler._model.formattedShortDate.replace(', ', '/');
        else if (scheduler && scheduler._selectedViewName.toLowerCase() == "day" || scheduler && scheduler._selectedViewName.toLowerCase() == "week" || scheduler && scheduler._selectedViewName.toLowerCase() == "workweek")
            date = scheduler._model.formattedShortDate;
        return date;
    },
    ShowAppointmentSummary: function () {
        if ($("#pnlPMSScheduler #hfProviderIds").val() == "" && $("#pnlPMSScheduler #hfResourceIds").val() == "") {
            utility.DisplayMessages("Please select Provider or Resource", 2);
            return false;
        }
        var date = PMSScheduler.getKendoSchDate();
        var params = [];
        var facilityIds = $("#pnlPMSScheduler #hfFacilityIds").val() ? $("#pnlPMSScheduler #hfFacilityIds").val() : "0";
        var providerIds = $("#pnlPMSScheduler #hfProviderIds").val() ? $("#pnlPMSScheduler #hfProviderIds").val() : "0";
        var resourceIds = $("#pnlPMSScheduler #hfResourceIds").val() ? $("#pnlPMSScheduler #hfResourceIds").val() : "0";
        if (date) {
            params["ProviderId"] = providerIds;
            params["ResourceId"] = resourceIds;
            params["FacilityId"] = facilityIds;
            params["DayDate"] = date;
            params["ParentCtrl"] = "schTabCalendar";
            LoadActionPan('appointmentSummary', params);
        } else {
            utility.DisplayMessages("Some thing wrong! Please try again", 2);
        }
    },
    ShowBlockedAppointment: function () {
        if ($("#pnlPMSScheduler #hfProviderIds").val() == "" && $("#pnlPMSScheduler #hfResourceIds").val() == "") {
            utility.DisplayMessages("Please select Provider or Resource", 2);
            return false;
        }
        var params = [];
        var facilityIds = $("#pnlPMSScheduler #hfFacilityIds").val() ? $("#pnlPMSScheduler #hfFacilityIds").val() : "0";
        var providerIds = $("#pnlPMSScheduler #hfProviderIds").val() ? $("#pnlPMSScheduler #hfProviderIds").val() : "0";
        var resourceIds = $("#pnlPMSScheduler #hfResourceIds").val() ? $("#pnlPMSScheduler #hfResourceIds").val() : "0";
        var datedDate = PMSScheduler.getKendoSchDate();
        if (datedDate) {
            params["ProviderId"] = providerIds;
            params["ResourceId"] = resourceIds;
            params["FacilityId"] = facilityIds;
            params["DayDate"] = datedDate;
            params["ParentCtrl"] = "schTabCalendar";
            LoadActionPan('Scheduling_BlockAppointment_Summary', params);
        } else {
            utility.DisplayMessages("Some thing wrong! Please try again", 2);
        }
    },
    InitializeAutoComplete: function () {
        $("#appointmentDetail #txtFullName").kendoAutoComplete({
            dataTextField: "FullName",
            filter: "contains",
            minLength: 3,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var patName = $("#appointmentDetail #txtFullName").val();
                        var AllPatients = utility.GetPatientArrayByName(patName, 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
            template: '<span> #: AccountNumber # -  #: FullName # </span>',
            change: function (e) {
                appointmentDetail.FillPatientAccount(this.dataItem().id);
            }

        });

        $("#appointmentDetail #txtAccountNo").kendoAutoComplete({
            dataTextField: "ProductName",
            filter: "contains",
            minLength: 3,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        var AccountNo = $("#appointmentDetail #txtAccountNo").val();
                        var AllPatients = utility.GetPatientArray(AccountNo, 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
            template: '<span> #: AccountNumber # -  #: FullName # </span>',
            change: function (e) {
                appointmentDetail.FillPatientAccount(this.dataItem().id);
            }
        });
    },
    PatientDemographics: function (patientid, AccountNumber) {

        var CurrentSelectedTab = GetCurrentSelectedTab();
        $(".sc-tooltip").hide();
        if (CurrentSelectedTab.ActionPanContainer == "actionPanScheduleWaitList" || CurrentSelectedTab.ActionPanContainer == "actionPanPMSScheduler") {
            UnloadActionPan();
        }
        params["QuickAddPatient"] = true;
        $('#patTabDemographic').click();
        SelectTab('mstrTabPatient', 'false');
        setTimeout(function () {
            Patient_Search.params.FormName = "DayCalendar";
            Patient_Search.SelectPatient(patientid, null);
            $('#patTabDemographic').click();
            $("body").removeClass("modal-open").removeAttr("style");
            if (AccountNumber) {
                $("#pnlDocumentScan #frmDocumentScan #txtAccountNumber").val(AccountNumber);
                var $ctr = $('#pnlDocumentScan #frmDocumentScan #txtPateintName');
                var $hfctr = $('#pnlDocumentScan #frmDocumentScan #hfPatientId');
                $ctr.val(AccountNumber);
                $hfctr.val(patientid);
            }
        }, 200);
    },
    LoadToolTipData: function (AppointmentId) {
        var TooltipDetail = "";
        var response = PMSScheduler.ToolTipDataLoad(AppointmentId);//.done(function (response) {
        var Model = JSON.parse(response);
        var ResponseDetail = Model.ResponseModel;
        ResponseDetail = JSON.parse(ResponseDetail);
        var AppointmentDetail = new Object();
        if (ResponseDetail.status != false) {
            TooltipDetail = JSON.parse(ResponseDetail.ToolTipDataFill_JSON);

            var patientAge = "";
            if (TooltipDetail.PatientAge) {

                patientAge = TooltipDetail.PatientAge.split(',');

                if (parseInt((TooltipDetail.PatientAge.split(',')[0]).split(' ')[1]) > 0) {

                    patientAge = TooltipDetail.PatientAge.split(',')[0]; //age in years
                } else if (parseInt((TooltipDetail.PatientAge.split(',')[1]).split(' ')[1]) > 0) {
                    patientAge = TooltipDetail.PatientAge.split(',')[1]; //age in months
                } else {
                    patientAge = TooltipDetail.PatientAge.split(',')[2]; //age in days

                }


            }
            else {
                patientAge = TooltipDetail.Age + ' Year(s)';
            }

            var reminder_delivery_status = TooltipDetail.Status == (undefined) ? "" : TooltipDetail.Status;
            var reminder_delivery_response = TooltipDetail.KeyPress == (undefined) ? "" : TooltipDetail.KeyPress;
            var reminder_delivery_response_message = TooltipDetail.ResponseMessage == (undefined) ? "" : TooltipDetail.ResponseMessage;
            var status_style_color = "";

            if (reminder_delivery_status != "") {
                reminder_delivery_status = reminder_delivery_status.toLocaleLowerCase() == 'new' ? 'Waiting' : reminder_delivery_status;
                reminder_delivery_status = reminder_delivery_status.toLocaleLowerCase() == 'failed' ? 'Not Delivered' : reminder_delivery_status;
                reminder_delivery_status = reminder_delivery_status.toLocaleLowerCase() == 'made' ? 'Successfully Delivered' : reminder_delivery_status;
            }

            if (reminder_delivery_response != "") {
                reminder_delivery_response = reminder_delivery_response.toLocaleLowerCase() == '1' ? 'Confirm' : reminder_delivery_response;
                reminder_delivery_response = reminder_delivery_response.toLocaleLowerCase() == '2' ? 'Cancel' : reminder_delivery_response;
                //reminder_delivery_response = reminder_delivery_response.toLocaleLowerCase() == '0' ? 'Invalid Key Press' : reminder_delivery_response;
                if (reminder_delivery_response.toLocaleLowerCase() == '3' || reminder_delivery_response.toLocaleLowerCase() == '4' || reminder_delivery_response.toLocaleLowerCase() == '5' || reminder_delivery_response.toLocaleLowerCase() == '6' || reminder_delivery_response.toLocaleLowerCase() == '7' || reminder_delivery_response.toLocaleLowerCase() == '8' || reminder_delivery_response.toLocaleLowerCase() == '9') {
                    reminder_delivery_response = 'Invalid Key Press';
                }
                if (reminder_delivery_response.toLocaleLowerCase() == '0') {
                    reminder_delivery_response = 'No Response';
                }
            }

            if (reminder_delivery_response_message != "" && reminder_delivery_response_message.indexOf("answering") >= 0)
                reminder_delivery_response = "Message left on answering machine.";

            if (reminder_delivery_response.toLocaleLowerCase() == "confirm") {
                status_style_color = "green";
            }


            AppointmentDetail["Age"] = patientAge;
            AppointmentDetail["Gender"] = TooltipDetail.Gender;
            AppointmentDetail["AccountNumber"] = TooltipDetail.AccountNumber;
            AppointmentDetail["DOB"] = TooltipDetail.DOB;
            AppointmentDetail["CellNo"] = TooltipDetail.CellNo;
            AppointmentDetail["EmailAddress"] = TooltipDetail.EmailAddress;
            AppointmentDetail["Duration"] = TooltipDetail.Duration;
            AppointmentDetail["PrimaryInsuranceName"] = TooltipDetail.PrimaryInsuranceName;

            AppointmentDetail["ReminderDeliveryStatus"] = reminder_delivery_status;
            AppointmentDetail["ReminderDeliveryResponse"] = reminder_delivery_response;
            // AppointmentDetail["ReminderDeliveryResponse"] = "test Response";
            // AppointmentDetail["StatusStyleColor"]= "green";
            AppointmentDetail["StatusStyleColor"] = status_style_color;
            AppointmentDetail["CancellationReason"] = TooltipDetail.CancellationReason;


            AppointmentDetail["ScheduleDate"] = TooltipDetail.ScheduleDate;
            AppointmentDetail["ScheduleProvider"] = TooltipDetail.ScheduleProvider;
            AppointmentDetail["ScheduleFacility"] = TooltipDetail.ScheduleFacility;


            if (TooltipDetail.RescheduleDate) {
                AppointmentDetail["RescheduleColor"] = "#000000";
                AppointmentDetail["RescheduleDate"] = TooltipDetail.RescheduleDate ? TooltipDetail.RescheduleDate : "";
                AppointmentDetail["RescheduleProvider"] = TooltipDetail.RescheduleProvider ? TooltipDetail.RescheduleProvider : "";
                AppointmentDetail["RescheduleFacility"] = TooltipDetail.RescheduleFacility ? TooltipDetail.RescheduleFacility : "";
                AppointmentDetail["RescheduleText"] = "Rescheduled From:";

            }
            else if (TooltipDetail.ScheduleDate) {
                AppointmentDetail["RescheduleColor"] = "#000000";
                AppointmentDetail["RescheduleDate"] = TooltipDetail.ScheduleDate ? TooltipDetail.ScheduleDate : "";
                AppointmentDetail["RescheduleProvider"] = TooltipDetail.RescheduleProvider ? TooltipDetail.RescheduleProvider : "";
                AppointmentDetail["RescheduleFacility"] = TooltipDetail.RescheduleFacility ? TooltipDetail.RescheduleFacility : "";
                AppointmentDetail["RescheduleText"] = "Rescheduled To:";
            }
            else {
                AppointmentDetail["RescheduleColor"] = "#F5F5F5";
                AppointmentDetail["RescheduleDate"] = "";
                AppointmentDetail["RescheduleProvider"] = "";
                AppointmentDetail["RescheduleFacility"] = "";
                AppointmentDetail["RescheduleText"] = "Rescheduled From:";


            }


            AppointmentDetail["CopayBal"] = TooltipDetail.CopayBal;
            if (TooltipDetail.AppointmentStatus == "Cancel") {

                AppointmentDetail["ReasonComments"] = TooltipDetail.CancellationReason;

            }
            else {
                if (TooltipDetail.ReasonComments)
                    AppointmentDetail["ReasonComments"] = TooltipDetail.ReasonComments.replace(/#@#/g, ',');
                else
                    AppointmentDetail["ReasonComments"] = "";
            }

            if (TooltipDetail.imgPatient)
                AppointmentDetail["imgPatient"] = TooltipDetail.imgPatient;
            else {
                if (TooltipDetail.Gender == "Male")
                    AppointmentDetail["imgPatient"] = "../../Content/images/default_male_profile.gif";
                else
                    AppointmentDetail["imgPatient"] = "../../Content/images/default_female_profile.gif";

            }

            // set modifiedon and modifiedby empty if both are same

            if (TooltipDetail.ModifiedOn == TooltipDetail.CreatedOn) {
                AppointmentDetail["ModifiedOn"] = "";
                AppointmentDetail["ModifiedBy"] = "";
            }
            else {
                AppointmentDetail["ModifiedOn"] = TooltipDetail.ModifiedOn;
                AppointmentDetail["ModifiedBy"] = TooltipDetail.ModifiedBy;
            }
            AppointmentDetail["CreatedBy"] = TooltipDetail.CreatedBy;

            AppointmentDetail["CreatedOn"] = TooltipDetail.CreatedOn;



            return AppointmentDetail;
        }
        else {

            return AppointmentDetail;
        }


    },
    EligibilityId_DBCall: function (PatientInsuranceId, AppointmentId, PatientId, Status) {
        var objData = new Object();
        //if (model.GroupType == "Resource") {
        //    objData["ResourceId"] = ProviderId;
        //}
        //else
        objData["InsuranceId"] = PatientInsuranceId;
        objData["Status"] = Status;
        objData["AppointmentId"] = AppointmentId;

        objData["PatientId"] = PatientId;
        objData["CommandType"] = "load_eligibilityid";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");
    },
    ToolTipDataLoad: function (AppointmentId) {
        var objData = new Object();
        objData["CommandType"] = "fill_tooltip_data";
        objData["AppointmentId"] = AppointmentId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIServiceSyncCall(data, "Scheduler", "PMSScheduler");

    },
    ScheduledSlotsLoad: function (viewType) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        var ProviderIds = $("#pnlPMSScheduler #hfProviderIds").val();
        var ResourceIds = $("#pnlPMSScheduler #hfResourceIds").val();
        var FacilityIds = $("#pnlPMSScheduler #hfFacilityIds").val();
        var PatientTypeId = $("#pnlPMSScheduler #hfPatientTypeId").val();
        var VisitTypeId = $("#pnlPMSScheduler #hfVisitTypeIds").val();
        var StartDate = kendo.toString(new Date(), "d");

        if (PMSScheduler.NavigationEvent) {
            StartDate = kendo.toString(PMSScheduler.NavigationEvent.date, "d");
        }
        else if (scheduler && scheduler.date())
            StartDate = kendo.toString(scheduler.date(), "d");

        var AppointmentStatusIds = $("#pnlPMSScheduler #hfAppointmentStatusIds").val();

        var objData = new Object();
        objData["ProviderIds"] = ProviderIds;
        objData["ResourceIds"] = ResourceIds;
        objData["FacilityIds"] = FacilityIds;
        objData["PatientTypeId"] = PatientTypeId ? PatientTypeId : 0;
        objData["VisitTypeIds"] = VisitTypeId;
        objData["StartDate"] = StartDate;
        objData["AppointmentStatusIds"] = AppointmentStatusIds;
        objData["IsResourceSch"] = PMSScheduler.IsResourceSch;
        objData["SchViewType"] = viewType;
        if (PMSScheduler.NextDate) {
            objData["Month"] = PMSScheduler.NextDate.getMonth() + 1;
            objData["Year"] = PMSScheduler.NextDate.getFullYear();
        }
        else {
            objData["Month"] = 0;
            objData["Year"] = 0;
        }
        objData["CommandType"] = "Search_Appointment_Schdule";

        var data = JSON.stringify(objData);
        return MDVisionService.APIServiceSyncCall(data, "Scheduler", "PMSScheduler");

    },
    // Function for paste appointment for copyied Element
    onCopyPaste: function (fromT, toT) {
        if (PMSScheduler.CopiedEventHolder) {
            //var scheduler = $("#scheduler").data("kendoScheduler");
            //var resourceArry = scheduler.resources[1].dataSource.options.data;
            //var resource = resourceArry[PMSScheduler.CopiedEventHolder.NewgroupIndex];
            //var newProviderId = PMSScheduler.CopiedEventHolder.ProviderId;
            //if (resource) {
            //    newProviderId = resource.value;
            //}
            //var startDateTime = PMSScheduler.CopiedEventHolder.end;
            //if ((Date.parse(toT) != Date.parse(fromT) && newProviderId == PMSScheduler.CopiedEventHolder.ProviderId) || (Date.parse(toT) == Date.parse(fromT) && newProviderId != PMSScheduler.CopiedEventHolder.ProviderId)) {
            //utility.myConfirm("Are you sure, you want to paste appointment?",
            //    function () {
            var startDateTime = PMSScheduler.CopiedEventHolder.end;
            PMSScheduler.CopiedEventHolder.end = new Date(startDateTime.setMinutes(startDateTime.getMinutes() + PMSScheduler.CopiedEventHolder.SchSlotDuration));
            PMSScheduler.CopyPasteScheduler_DBCall(PMSScheduler.CopiedEventHolder).done(function (response) {
                if (response.status != false) {
                    PMSScheduler.CopiedEventHolder.AppointmentId = response.AppointmentId;
                    PMSScheduler.CopiedEventHolder.id = response.AppointmentId;
                    utility.DisplayMessages("Successfully Pasted", 1);
                    //PMSScheduler.LoadCurrentAppointmentInScheduler("Add", PMSScheduler.CopiedEventHolder);
                    var scheduler = $("#scheduler").data("kendoScheduler");
                    scheduler.dataSource.read();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                PMSScheduler.CopiedEventHolder = null;
            });
            //}, function () { }, "Paste Appointment Alert");
        }
        //  }
    },
    // Function for paste appointment for copyied Element
    onCutPaste: function () {
        if (PMSScheduler.CutEventHolder) {
            //var fromT = kendo.toString(new Date(PMSScheduler.CutEventHolder.OldStart), "t");
            //var toT = kendo.toString(new Date(PMSScheduler.CutEventHolder.NewStart), "t");

            //var scheduler = $("#scheduler").data("kendoScheduler");
            //var resourceArry = scheduler.resources[1].dataSource.options.data;
            //var resource = resourceArry[PMSScheduler.CutEventHolder.NewgroupIndex];
            //var newProviderId = PMSScheduler.CutEventHolder.ProviderId;
            //if (resource) {
            //    newProviderId = resource.value;
            //}
            //if ((Date.parse(toT) != Date.parse(fromT) && newProviderId == PMSScheduler.CutEventHolder.ProviderId) || (Date.parse(toT) == Date.parse(fromT) && newProviderId != PMSScheduler.CutEventHolder.ProviderId)) {
            //utility.myConfirm("Are you sure, you want to move appointment from: " + fromT + " to: " + toT + "?",
            //       function () {  
            var startDateTime = PMSScheduler.CutEventHolder.end;
            PMSScheduler.CutEventHolder.end = new Date(startDateTime.setMinutes(startDateTime.getMinutes() + PMSScheduler.CutEventHolder.SchSlotDuration));
            PMSScheduler.CutPasteScheduler_DBCall(PMSScheduler.CutEventHolder).done(function (response) {
                if (response.status != false) {
                    PMSScheduler.CutEventHolder.AppointmentId = response.AppointmentId;
                    PMSScheduler.CutEventHolder.id = response.AppointmentId;
                    utility.DisplayMessages("Successfully Pasted", 1);
                    var scheduler = $("#scheduler").data("kendoScheduler");
                    scheduler.dataSource.read();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                PMSScheduler.CutEventHolder = null;
            });

            //}, function () {
            //}, "Paste Appointment Alert");
        }

        //}

    },
    onDroped: function () {
        if (PMSScheduler.CutEventHolder) {
            var fromT = kendo.toString(new Date(PMSScheduler.CutEventHolder.OldStart), "t");
            var toT = kendo.toString(new Date(PMSScheduler.CutEventHolder.NewStart), "t");
            //utility.myConfirm("Are you sure, you want to move appointment from: " + fromT + " to: " + toT + "?",
            //       function () {
            var startDateTime = PMSScheduler.CutEventHolder.end;
            PMSScheduler.CutEventHolder.end = new Date(startDateTime.setMinutes(startDateTime.getMinutes() + PMSScheduler.CutEventHolder.SchSlotDuration));
            PMSScheduler.CutPasteScheduler_DBCall(PMSScheduler.CutEventHolder).done(function (response) {
                if (response.status != false) {
                    PMSScheduler.CutEventHolder.AppointmentId = response.AppointmentId;
                    PMSScheduler.CutEventHolder.id = response.AppointmentId;
                    utility.DisplayMessages("Moved Successfully", 1);
                    //PMSScheduler.LoadCurrentAppointmentInScheduler("Edit", PMSScheduler.CutEventHolder);
                    var scheduler = $("#scheduler").data("kendoScheduler");
                    scheduler.dataSource.read();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                PMSScheduler.CutEventHolder = null;
            });

            //}, function () {
            //}, "Paste Appointment Alert");
        }

    },
    CopyPasteScheduler_DBCall: function (model) {
        var objData = new Object();
        if (model.GroupType == "Resource") {
            objData["ResourceId"] = model.ProviderId;
        }
        else
            objData["ProviderId"] = model.ProviderId;
        //var scheduler = $("#scheduler").data("kendoScheduler");
        //var resourceArry = scheduler.resources[1].dataSource.options.data;
        //var resource = resourceArry[model.NewgroupIndex];
        //if (resource) {
        //    if (model.GroupType = 'Resources') {
        //        objData["ResourceId"] = resource.value;
        //    }
        //    else
        //        objData["ProviderId"] = resource.value;
        //}

        objData["AppointmentId"] = model.AppointmentId;

        objData["PatientId"] = model.PatientId;
        objData["AppointmentDate"] = kendo.toString(model.NewAppointmentDate, "s");
        objData["end"] = model.end;
        objData["start"] = model.start;

        objData["CommandType"] = "Copy_Paste_Appointment";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");
    },
    CutPasteScheduler_DBCall: function (model) {
        var objData = new Object();
        if (model.GroupType == "Resource") {
            objData["ResourceId"] = model.ProviderId;
        }
        else
            objData["ProviderId"] = model.ProviderId;
        objData["AppointmentId"] = model.AppointmentId;
        objData["PatientId"] = model.PatientId;
        objData["AppointmentDate"] = kendo.toString(model.NewAppointmentDate, "s");
        objData["end"] = model.end;
        objData["start"] = model.start;

        objData["CommandType"] = "Cut_Paste_Appointment";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");
    },
    LoadCurrentAppointmentInScheduler: function (mode, model) {
        var scheduler = $("#scheduler").data("kendoScheduler");
        delete model.SchSlotDuration;
        delete model.OldStart;
        delete model.NewStart;
        delete model.NewAppointmentDate;
        delete model.NewgroupIndex;
        if (mode == "Add") {
            scheduler.dataSource.add({
                AmtCopay: model.CoPayment,
                AppointmentDate: model.AppointmentDate,
                AppointmentDateFrom: new Date(model.start),
                AppointmentDateTo: new Date(model.end),
                AppointmentId: model.AppointmentId,
                AppointmentStatus: model.AppointmentStatus,
                CommandType: model.CommandType,
                Comments: model.Comments,
                CopayBal: model.CopayBal,
                CopayClass: model.CopayClass,
                EligibilityStatus: model.EligibilityStatus,
                EndDate: model.EndDate,
                FacilityColor: model.FacilityColor,
                FacilityId: model.FacilityId,
                FacilityName: model.FacilityName,
                IsNonBilable: model.IsNonBilable,
                Month: new Date(model.AppointmentDate).getMonth() + 1,
                PatientId: model.Patientid,
                PatientName: model.PatientName,
                PatientType: model.PatientType,
                PatientTypeId: model.PatientTypeId,
                ProviderId: model.ProviderId,
                ProviderName: model.ProviderName,
                ReasonComments: model.ReasonComments,
                SchStatusId: model.SchStatusId,
                StatusColor: model.StatusColor,
                TimeFrom: model.TimeFrom,
                TimeTo: model.TimeTo,
                VisitTypeId: model.VisitTypeId,
                VisitTypeName: model.VisitTypeName,
                Year: new Date(model.AppointmentDate).getFullYear(),
                end: new Date(model.end),
                id: model.AppointmentId,
                ownerId: 0,
                start: new Date(mode.start),
                uid: PMSScheduler.generateUID(),
            });
        } else if (mode == "Edit") {
            var dataSourceApp = scheduler.dataSource;
            //var ap = dataSourceApp._data.filter(f => f.AppointmentId == model.AppointmentId)[0];
            var ap = dataSourceApp._data.filter(function (f) {
                return f.AppointmentId == model.AppointmentId;
            })[0];

            ap.AppointmentDate = new Date(model.AppointmentDate);
            ap.AppointmentDateFrom = new Date(model.start);
            ap.AppointmentDateTo = new Date(model.end);
            ap.ProviderId = model.ProviderId,
            ap.end = new Date(model.end);
            ap.start = new Date(model.start);
            scheduler.dataSource.pushUpdate(ap);
        }
    },
    RefreshScheduler: function () {
        var scheduler = $("#scheduler").data("kendoScheduler");
        if (scheduler)
            scheduler.refresh();
    },
    generateUID: function () {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    },
    ReloadScheduler: function () {
        var scheduler = $("#scheduler").data("kendoScheduler");
        if (scheduler) {
            scheduler.dataSource.read();
        }

    },
    ShowTooltipclass: function () {
        $(".sc-tooltip").show();
    },
    LoadSchBlockHours: function () {
        var scheduler = $("#scheduler").data("kendoScheduler");
        if (scheduler && scheduler._selectedViewName) {
            var contentDiv = scheduler.element.find("div.k-scheduler-content");
            var rows = contentDiv.find("tr");
            if (gblockHoursCount > 0) {
                var BlockHoursLoadJSONData = gblockHoursJSON;
                var resourceArry = scheduler.resources[1] ? scheduler.resources[1].dataSource.options.data : [];
                $.each(BlockHoursLoadJSONData, function (ind, item) {

                    for (var i = 0; i < rows.length; i++) {
                        var tds = $(rows[i]).find('td');
                        for (var s = 0; s < tds.length; s++) {
                            var slot = scheduler.slotByElement(tds[s]);
                            PMSScheduler.SetSlotColor(slot, resourceArry, item, scheduler._selectedViewName, tds[s]);
                        }
                    };
                });
                if (!BlockHoursLoadJSONData[0].FromDate == BlockHoursLoadJSONData[0].ToDate) {
                    PMSScheduler.setProviderScheduleBaseHours();
                }
            }

        }
        else {


        }
    },

    refreshSchBlockHours: function () {
        var scheduler = $("#scheduler").data("kendoScheduler");
        if (scheduler && scheduler._selectedViewName) {
            if (scheduler._selectedViewName != "month") {
                PMSScheduler.ResetAllSlotSlotsColor(scheduler);
            }
            var contentDiv = scheduler.element.find("div.k-scheduler-content");
            var rows = contentDiv.find("tr");

            PMSScheduler.LoadSchBlockHours_DBCall(scheduler._selectedViewName, scheduler).done(function (response) {
                if (response.status != false) {
                    if (response.BlockHoursCount > 0) {
                        var BlockHoursLoadJSONData = JSON.parse(response.BlockHoursLoad_JSON);
                        gblockHoursCount = response.BlockHoursCount;
                        gblockHoursJSON = response.BlockHoursLoad_JSON != undefined ? JSON.parse(response.BlockHoursLoad_JSON) : null;
                        var resourceArry = scheduler.resources[1] ? scheduler.resources[1].dataSource.options.data : [];
                        $.each(BlockHoursLoadJSONData, function (ind, item) {

                            for (var i = 0; i < rows.length; i++) {
                                var tds = $(rows[i]).find('td');
                                for (var s = 0; s < tds.length; s++) {
                                    var slot = scheduler.slotByElement(tds[s]);
                                    PMSScheduler.SetSlotColor(slot, resourceArry, item, scheduler._selectedViewName, tds[s]);
                                }
                            };
                        });
                        if (!BlockHoursLoadJSONData[0].FromDate == BlockHoursLoadJSONData[0].ToDate) {
                            PMSScheduler.setProviderScheduleBaseHours();
                        }
                    }

                }
            });
        }
        else {


        }
    },
    SetSlotColor: function (slot, resourceArry, item, viewType, slotElem) {
        if (slot) {
            var blockFrom = kendo.toString(new Date(item.SchDateFrom), "s");
            var blockTo = kendo.toString(new Date(item.SchDateTo), "s");
            var ProviderId = resourceArry.length != 0 ? resourceArry[slot.groupIndex].value : "";
            var slotStart = kendo.toString(slot.startDate, "s");
            if (Date.parse(slotStart) >= Date.parse(blockFrom) && Date.parse(slotStart) < Date.parse(blockTo) && (ProviderId == item.ProviderId || viewType == "month")) {
                $(slotElem).css('background-color', item.Color);
                $(slotElem).css('color', "White");
                var description = "";
                if (viewType == "week") {
                    if (item.Description && item.Description.length > 30) {
                        description = item.Description.substring(0, 30) + "..";
                    } else {
                        description = item.Description;
                    }
                } else {
                    if (item.Description && item.Description.length > 30) {
                        description = item.Description.substring(0, 30) + "..";
                    } else {
                        description = item.Description;
                    }
                }
                if (item.BlockHoursId) {
                    $(slotElem).attr('title', item.Description);
                    $(slotElem).text("Blocked:-" + description);
                    $(slotElem).attr('isBlock', "true");
                    $(slotElem).attr('BlockHoursId', item.BlockHoursId);
                    $(slotElem).css('text-align', "right");
                }
                else {
                    // This Else code will run for Holidays
                    if (viewType != "month") {
                        $(slotElem).attr('title', item.Description);
                        $(slotElem).find('span.blockedText').remove();
                        $(slotElem).append('<span class="blockedText pull-left ellip150">' + "Blocked:-" + description + '</span>');
                        $(slotElem).removeAttr('isBlock');
                        $(slotElem).attr('BlockHoursId', item.BlockHoursId);
                        $(slotElem).css('text-align', "right");
                    }
                    else {
                        $(slotElem).attr('title', item.Description);
                        //$(slot.element).html('<div><span style="float:left">' + "Blocked:-" + description + '</span><span style="float:right;color:black">' + $(slot.element)[0].textContent + '</span></div>');
                        if ($(slotElem).find('span.totalCount').length == 0) {
                            $(slotElem).find('span.blockedText').remove();
                            $(slotElem).append('<span class="blockedText pull-left ellip150">' + "Blocked:-" + description + '</span>');
                        }
                        $(slotElem).removeAttr('isBlock');
                        $(slotElem).attr('BlockHoursId', item.BlockHoursId);
                        //$(slot.element).css('text-align', "right");

                    }

                }
            }
            //else if (!$(slot.element).attr('isBlock')) {
            //    $(slot.element).css('background-color', "");
            //    $(slot.element).css('color', "");
            //    $(slot.element).text("");
            //    $(slot.element).removeAttr('isBlock');
            //}


        }
    },
    ResetAllSlotSlotsColor: function (scheduler) {
        var tds = scheduler.view().content.find('td');
        for (var i = 0; i < tds.length; i++) {
            var slot = scheduler.slotByElement(tds[i]);
            if (slot) {
                $(slot.element).css('background-color', "");
                $(slot.element).css('color', "");
                $(slot.element).text("");
                $(slot.element).removeAttr('isBlock');
            }
        }
    },
    LoadSchBlockHours_DBCall: function (ViewTypeName, scheduler) {
        var objData = new Object();
        var ProviderIds = $("#pnlPMSScheduler #hfProviderIds").val();
        var ResourceIds = $("#pnlPMSScheduler #hfResourceIds").val();
        objData["ProviderIds"] = ProviderIds;
        objData["ResourceIds"] = ResourceIds;
        var StartDate = kendo.toString(new Date(), "d");

        if (PMSScheduler.NavigationEvent) {
            StartDate = kendo.toString(PMSScheduler.NavigationEvent.date, "d");
        }
        else if (scheduler && scheduler.date())
            StartDate = kendo.toString(scheduler.date(), "d");

        objData["start"] = StartDate;
        objData["SchViewType"] = ViewTypeName;
        objData["CommandType"] = "Load_Sch_BlockHours";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");
    },
    BlockHoursDelete: function (BlockHoursId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        AppPrivileges.GetFormPrivileges("Block Hours", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                utility.myConfirm('Do you want to delete the block hours ?', function () {
                    var selectedValue = BlockHoursId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_BlockHours.DeleteBlockHours(selectedValue).done(function (response) {
                            if (response.status != false) {

                                utility.DisplayMessages(response.Message, 1);
                                var scheduler = $("#scheduler").data("kendoScheduler");
                                if (scheduler) {
                                    scheduler.dataSource.read();
                                }
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () {
                },
                    'Confirm Delete'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    ZoomInScheduler2: function (event) {
        //kendo.fx($(scheduler).find('.k-scheduler-content')).zoom("in").startValue(1).endValue(1.2).play(); scheduler
        if (event != this) {
            event.stopPropagation();
        }

        if (PMSScheduler.ZoomINCount == 0)
            kendo.fx($('#scheduler').find('.k-scheduler-content')).zoom("in").startValue(1).endValue(1.1).play();
        else if (PMSScheduler.ZoomINCount == 1)
            kendo.fx($('#scheduler').find('.k-scheduler-content')).zoom("in").startValue(1.1).endValue(1.2).play();

        if (PMSScheduler.ZoomINCount < 2)
            PMSScheduler.ZoomINCount++;

    },
    ZoomOutScheduler2: function (event) {
        if (event != this) {
            event.stopPropagation();
        }
        if (PMSScheduler.ZoomINCount == 0) {
            // Do Nothing
        }

        else if (PMSScheduler.ZoomINCount == 1)
            kendo.fx($('#scheduler').find('.k-scheduler-content')).zoom("out").endValue(1).startValue(1.1).play();
        else if (PMSScheduler.ZoomINCount == 2)
            kendo.fx($('#scheduler').find('.k-scheduler-content')).zoom("out").endValue(1.1).startValue(1.2).play();
        //   kendo.fx($('#scheduler')).zoom("out").endValue(1).startValue(1.1).play();
        if (PMSScheduler.ZoomINCount > 0)
            PMSScheduler.ZoomINCount--;
    },
    setCustomWorkWeekView: function (scheduler) {
        if (scheduler && scheduler.viewName() == "workWeek") {
            var contentDiv = scheduler.element.find("div.k-scheduler-content");
            var rows = contentDiv.find("tr");
            $($($('.k-scheduler-header tr')[headerIndex]).find('th')).removeClass('hidden');
            $(rows).find('td').removeClass('hidden');
            var datesArr = [];
            var customWorkWeekDays = globalAppdata["WeekWorkDaysIds"].split(',');
            var selectedProviders = $("#pnlPMSScheduler #providerMultiselect").data("kendoMultiSelect").value().length;
            var selectedResources = $("#pnlPMSScheduler #resourceMultiselect").data("kendoMultiSelect").value().length;
            var headerIndex = 0;
            if (selectedProviders > 0 || selectedResources > 0) {
                headerIndex = 1;
            }
            if (globalAppdata["WeekWorkDaysIds"] != null && globalAppdata["WeekWorkDaysIds"].indexOf('1') > -1) {
                $($($('.k-scheduler-header tr')[headerIndex]).find('th')).each(function (k, v) {
                    if (customWorkWeekDays[k] == "0") {
                        $(this).addClass('hidden');
                        datesArr.push(k);
                    }

                });
                $(datesArr).each(function (i, v) {
                    $(rows).each(function (ind, val) {

                        var td = $(val).find('td')[v];

                        $(td).addClass('hidden');

                    });
                });
                var workdays = $($($('.k-scheduler-header tr')[1]).find('th.hidden')).length;

                if ($('.k-scheduler-header tr').length > 1 && workdays > 0) {
                    workdays = 7 - workdays;
                    $($($('.k-scheduler-header tr')[0]).find('th')[0]).attr('colspan', workdays);
                }
            }
            else {
                if ($($('.k-scheduler-header tr')[headerIndex]).find('th').length > 6) {
                    $($($('.k-scheduler-header tr')[headerIndex]).find('th')[0]).addClass('hidden');
                    $($($('.k-scheduler-header tr')[headerIndex]).find('th')[6]).addClass('hidden');
                    $($($('.k-scheduler-header tr')[headerIndex]).find('th')[1]).removeClass('hidden');
                    $($($('.k-scheduler-header tr')[headerIndex]).find('th')[2]).removeClass('hidden');
                    $($($('.k-scheduler-header tr')[headerIndex]).find('th')[3]).removeClass('hidden');
                    $($($('.k-scheduler-header tr')[headerIndex]).find('th')[4]).removeClass('hidden');
                    $($($('.k-scheduler-header tr')[headerIndex]).find('th')[5]).removeClass('hidden');
                }
                $(rows).each(function (ind, val) {
                    if ($($(val).find('td')).length > 6) {
                        $($(val).find('td')[0]).addClass('hidden');
                        $($(val).find('td')[6]).addClass('hidden');
                        $($(val).find('td')[1]).removeClass('hidden');
                        $($(val).find('td')[2]).removeClass('hidden');
                        $($(val).find('td')[3]).removeClass('hidden');
                        $($(val).find('td')[4]).removeClass('hidden');
                        $($(val).find('td')[5]).removeClass('hidden');
                    }
                });

                $($($('.k-scheduler-header tr')[0]).find('th')[0]).attr('colspan', 5);

            }
        }

    },
    calculatAppointments: function (scheduler) {
        var counts = null;
        var names = $('#pnlPMSScheduler .k-scheduler-header-wrap').find('.k-slot-cell');
        var allEvents = scheduler.element.find("div.k-event");
        $.each(names, function (i) {
            var resourceArry = scheduler.resources[1].dataSource.options.data;
            var ind = names.length > 1 ? i : 0;
            var resource = resourceArry[ind];
            if (resource) {
                if (scheduler._workDayMode) {
                    if (allEvents.length > 0) {
                        var count = null;
                        $.each(allEvents, function (r, evt) {
                            $.each(scheduler._data, function (ixd, arrObj) {
                                if (arrObj.uid == $(evt).data('uid') && arrObj.ProviderId == resource.value)
                                    count++;
                            });
                        });
                        $(names[i]).html(resource.text + " <span class='badge style='color:white;background: #D2312D;'>" + (count ? count : "") + "</span>");
                    }
                    else {
                        $(names[i]).html(resource.text);
                    }
                }
                else {
                    counts = $.grep(scheduler._data, function (v) {
                        return v.ProviderId == resource.value;
                    });
                    if (counts && counts.length > 0) {
                        $(names[i]).html(resource.text + " <span class='badge style='color:white;background: #D2312D;'>" + (counts.length > 0 ? counts.length : "") + "</span>");
                    } else {
                        $(names[i]).html(resource.text + " <span class='badge style='color:white;background: #D2312D;'></span>");
                    }
                }
            }
        });
    },
}
var wwidth = "110%";