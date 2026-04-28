schcheckout = {
    bIsFirstLoad: true,
    params: [],
    NavigationEvent: null,
    scheduledSlots: [],
    Load: function (params) {

        schcheckout.params = params;

        if (schcheckout.bIsFirstLoad) {
            schcheckout.bIsFirstLoad = false;
        }

        var self = $('#schcheckout');
        self.loadDropDowns(true).done(function () {

            schcheckout.LoadAllAutocomplete();
            schcheckout.LoadAppointment();
            $('#schcheckout #divAppointmentsAvailableSlots #Provider').val(schcheckout.params.ProviderId);
            $('#schcheckout #divAppointmentsAvailableSlots #Facility').val(schcheckout.params.FacilityId);
        });

        $('#schcheckout #dpDOS').datepicker("setDate", schcheckout.params.DayDate);
        $('#schcheckout #divAppointmentsAvailableSlots #fromDate').datepicker("setDate", schcheckout.params.DayDate);
        //schcheckout.params.DayDate;

        utility.callbackAfterAllDOMLoaded(function () {
            $('#schcheckout #frmcheckout').data('serialize', $('#schcheckout #frmcheckout').serialize());
        });
    },

    LoadAppointment: function () {
        BackgroundLoaderShow(true);


        schcheckout.FillPatientData(schcheckout.params.PatientId, schcheckout.params.AppointmentId).done(function (response) {
            if (response.status != false) {


                var appointment_detail = JSON.parse(response.AppointmentFill_JSON);
                var patientappointments = JSON.parse(response.PatientAppointment_JSON);
                 var appointmentCheckout = JSON.parse(response.CheckoutAppointment_JSON);
                var patinsurance = JSON.parse(response.PatientInsuranceDetail_JSON);
                var copayments = JSON.parse(response.PatientCopayment_JSON);
                var patient_balance = JSON.parse(response.PatientBalance_JSON);

                if (patient_balance.length > 0) {
                    $('#schcheckout #txtPatientBalance').val(parseFloat(Number(patient_balance[0].PatientBalance)).toFixed(2));
                    //$('#schcheckin #txtAdvanceBalance').val(patient_balance[0].AdvanceBalance);
                    $('#schcheckout #txtPlanBalance').val(parseFloat(Number(patient_balance[0].InsuranceBalance)).toFixed(2));
                }

                if (copayments.length != '0' || copayments.length != 0) {

                    $.each(copayments, function (i, item) {
                        if (copayments[i].PaidAmountDr != "" && copayments[i].IsRefund != 'True') {

                            $('#schcheckout #chkPaid').attr('checked', true);

                            return false;
                        }
                    });

                }

                else if (copayments.length == '0' || copayments.length == 0) {

                    $('#schcheckout #chkPaid').attr('checked', false);
                }
                if (patientappointments.length >= 0) {

                    $('#frmcheckout #dpDOS').val(patientappointments[0].AppointmentDate.split(' ')[0]);
                    $('#schcheckout #hfRefProvider').val(patientappointments[0].RefProviderId);
                    $('#schcheckout #txtRefProvider').val(patientappointments[0].RefProviderName);

                    $('#schcheckout #hfProviderId').val(patientappointments[0].ProviderId);
                    $('#schcheckout #txtProvider').val(patientappointments[0].ProviderName);

                    $('#schcheckout #hfpatientaccount').val(patientappointments[0].AccountNumber);
                    $('#schcheckout #hfpatientid').val(patientappointments[0].PatientId);
                    $('#schcheckout #hfpatientname').val(patientappointments[0].PatientName);

                    $('#schcheckout #hfFacilityId').val(patientappointments[0].FacilityId);
                    schcheckout.params.FacilityId = patientappointments[0].FacilityId;
                    $('#schcheckout #txtFacility').val(patientappointments[0].FacilityName);

                    //$('#schcheckout #hfSchReasonId').val(patientappointments[0].SchReasonId);
                    //$('#schcheckout #txtReason').val(patientappointments[0].SchReasonId);

                    $('#schcheckout #hfSchStatusId').val(patientappointments[0].SchStatusId);

                    $('#schcheckout #txtReason').val(patientappointments[0].ReasonComments);
                    //Start 22-08-2016 Humaira Yousaf for referral Id
                    $('#schcheckout #hfReferralId').val(patientappointments[0].ReferralId);
                    if (patientappointments[0].ReferralId != null && patientappointments[0].ReferralId != '') {
                        $('#schcheckout #btnAddReferral').text('View Referral');
                    }
                    else {
                        $('#schcheckout #btnAddReferral').text('Add Referral');
                    }
                    //End 22-08-2016 Humaira Yousaf for referral Id
                    if (patientappointments[0].IsSpecialist == 'True')
                        $('#schcheckout #rdSpecialist').attr("checked", "checked");
                    else
                        $('#schcheckout #rdPCP').attr("checked", "checked");

                    var insuranceid = patientappointments[0].PatientInsuranceId;

                    $('#schcheckout #txtCopayment').val(parseFloat(Number(patientappointments[0].Copayment)).toFixed(2));
                }

                var self = $("#schcheckout");

                utility.bindMyJSON(true, appointment_detail, false, self).done(function () {

                    utility.AutoEnableAutoCompleteLink($('#schcheckout #frmcheckout'));
                    schcheckout.ValidatePatientCheckOut();

                });

                schcheckout.BindInsurancePlans(response.PatientInsuranceDetail_JSON, insuranceid);

                //set FollowUp Text
                if (appointmentCheckout.length > 0 &&  appointmentCheckout[0].isFollowUpAppointmentCreated != 1) {
                    if (response.NotesFollowUp_Count > 0) {
                        var FollowUp = JSON.parse(response.NotesFollowUp_JSON);
                        $("#frmcheckout #FollowUpText").html(FollowUp.SOAPText);
                        $("#frmcheckout #FollowUpText li").css("list-style", "none");
                        $("#frmcheckout #FollowUpText a").attr("disabled", "disabled");

                        if ($('#frmcheckout #FollowUpText').find(".haveText").length <= 0)
                            $("#frmcheckout #FollowUpText").css("display", "none");
                        utility.callbackAfterAllDOMLoaded(function () {
                            $("#frmcheckout #FollowUpText a").removeAttr("disabled");
                            $("#frmcheckout #FollowUpText a:first-child").text('Follow Up Appointment');
                            $("#frmcheckout #FollowUpText a:first-child").attr("onClick", "schcheckout.ShowAppointmentSlots()");
                            $("#frmcheckout #FollowUpText a:nth-child(2)").remove();
                        });
                    }
                }
            }

            else {
                utility.DisplayMessages(response.Message, 3);
                BackgroundLoaderShow(false);
            }

        });
    },
    ValidateVisitDate: function () {
        var DOS = new Date($('#frmcheckout #dpDOS').val());
        var selectedDate = new Date($('#schcheckout #divAppointmentsAvailableSlots #fromDate').val());
        if (selectedDate < DOS) {
            $("#schcheckout #divAppointmentsAvailableSlots #fromDate").datepicker("setDate", new Date($('#schcheckout #dpDOS').val()));
            utility.DisplayMessages('Please select date greater than visit date', 3);
            return false;
        }
        else {
            schcheckout.AvailableAppointmentFill();
        }
    },
    ShowAppointmentSlots: function () {
        $("#frmcheckout #divAppointmentsAvailableSlots").removeClass("hidden");
        //utility.CreateDatePicker('frmcheckout #divAppointmentsAvailableSlots #fromDate', function () {
        //    schcheckout.ValidateVisitDate();
        //}, true);
        var dayWeekYear = $("#frmcheckout #FollowUpText section div.haveText:first").attr("ctype");
        var count = $("#frmcheckout #FollowUpText section div.haveText:first").attr("cval");
        if (dayWeekYear) {
            if (count) {
                var dat = new Date($('#schcheckout #dpDOS').val());
                if ((dayWeekYear.toLowerCase() == "day" || dayWeekYear.toLowerCase() == "days") && count.toLowerCase() != '2-3') {
                    dat.setDate(dat.getDate() + parseInt(count) * 1);
                    $("#schcheckout #divAppointmentsAvailableSlots #fromDate").datepicker("setDate", dat);
                }
                else if (dayWeekYear.toLowerCase() == "week" || dayWeekYear.toLowerCase() == "weeks") {
                    dat.setDate(dat.getDate() + parseInt(count) * 7);
                    $("#schcheckout #divAppointmentsAvailableSlots #fromDate").datepicker("setDate", dat);
                }
                else if (dayWeekYear.toLowerCase() == "month" || dayWeekYear.toLowerCase() == "months") {
                    dat.setMonth(dat.getMonth() + parseInt(count));
                    $("#schcheckout #divAppointmentsAvailableSlots #fromDate").datepicker("setDate", dat);
                }
                else if (dayWeekYear.toLowerCase() == "year" || dayWeekYear.toLowerCase() == "years") {
                    dat.setFullYear(dat.getFullYear() + parseInt(count));
                    $("#schcheckout #divAppointmentsAvailableSlots #fromDate").datepicker("setDate", dat);
                }
                else if (dayWeekYear.toLowerCase() == 'prn' || count == '2-3')
                    $("#schcheckout #divAppointmentsAvailableSlots #fromDate").datepicker("setDate", new Date());
            }
            else
                $("#schcheckout #divAppointmentsAvailableSlots #fromDate").datepicker("setDate", new Date());
        }
        else
            $("#schcheckout #divAppointmentsAvailableSlots #fromDate").datepicker("setDate", new Date());
        var sch = $("#schcheckout #dvSlots");
        var schedulerData = sch.data("kendoScheduler");
        if (schedulerData) {
            schedulerData.destroy();
            sch.html("");
        }

        $("#chkOutProvider").kendoDropDownList({
            dataSource: PMSScheduler.providerDataSource,
            dataTextField: "name",
            dataValueField: "id",
            change: function (e) {
                $("#schcheckout #dvSlots").data("kendoScheduler").dataSource.read();
            },
        });
        $("#chkOutProvider").data("kendoDropDownList").select(function (dataItem) {
            return dataItem.id == schcheckout.params.ProviderId;
        });

        $("#chkOutFacility").kendoDropDownList({
            dataSource: PMSScheduler.facilityDataSource,
            dataTextField: "name",
            dataValueField: "id",
            change: function (e) {
                $("#schcheckout #dvSlots").data("kendoScheduler").dataSource.read();
            },
        });
        $("#chkOutFacility").data("kendoDropDownList").select(function (dataItem) {
            return dataItem.id == schcheckout.params.FacilityId;
        });

        var todayDate = schcheckout.setCustomHour("0", "0", new Date($("#schcheckout #divAppointmentsAvailableSlots #fromDate").val()));
        schcheckout.CanScheduler = $("#schcheckout #dvSlots").kendoScheduler({
            //resources: schcheckout.InitializeResources(),
            date: todayDate,
            dateHeaderTemplate: kendo.template("<strong>#=kendo.toString(date, 'd')#</strong>"),
            currentTimeMarker: {
                updateInterval: 100
            },
            minorTickCount: 1,
            minorTick: 15,
            majorTick: 15,
            allDaySlot: false,
            startTime: todayDate,
            height: 690,
            navigate: function (e) {
                schcheckout.NavigationEvent = e;
                $("#schcheckout #dvSlots").data("kendoScheduler").dataSource.read();
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
                setTimeout(function () {
                    $('.k-si-close').removeClass('k-icon');
                    $('.k-si-close').addClass('fa fa-times');
                }, 500);
                schcheckout.AvailableAppointmentFill();
            },
            eventTemplate: $("#event-template-waitlist ").html(),
            dataSource: {
                transport: {
                    read: function (e) {
                        schcheckout.SchedulerDefaultEvent = e;
                        schcheckout.LoadScheduler(e);
                        var schedulerData = $("#schcheckout #dvSlots").data("kendoScheduler");
                        if (schedulerData)
                            schedulerData.refresh();
                    },
                    //create: function (e) {
                    //    Scheduling_RescheduleSearch.SearchScheduleProviderDayView().done(function (response) {
                    //        if (response.status != false) {
                    //            e.data = response.ProviderScheduleFill_JSON;
                    //        }
                    //    });
                    //},
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
                var minutes = Scheduling_RescheduleSearch.diff_minutes(e.event.start, e.event.end);
                var providerId = $("#chkOutProvider").data("kendoDropDownList").value();
                var providerName = $("#chkOutProvider").data("kendoDropDownList").text();
                var FacilityId = $("#chkOutFacility").data("kendoDropDownList").value();
                var FacilityName = $("#chkOutFacility").data("kendoDropDownList").text();
                schcheckout.OpenAppointmentDetail("Add", null, providerId, providerName, null, null, FacilityId, FacilityName, null, Scheduling_RescheduleSearch.formatAMPM(e.event.start), Scheduling_RescheduleSearch.formatAMPM(e.event.end), minutes, null, $.datepicker.formatDate('mm/dd/yy', e.event.start), false, 0);
            },
            group: {
                resources: ["providers"]
            },
        }).data("kendoScheduler");
    },
    setCustomHour: function (hour, minut, datt) {
        datt.setHours(hour);
        datt.setMinutes(minut);
        datt.setSeconds(0);
        datt.setMilliseconds(0);
        return datt;
    },
    AvailableAppointmentFill: function () {
        if ($("#schcheckout #divAppointmentsAvailableSlots #Provider").val() == "" || $("#schcheckout #divAppointmentsAvailableSlots #Provider").val() == "0") {
            utility.DisplayMessages("Provider is required.", 3);
            return false;
        }
        else if ($("#schcheckout #divAppointmentsAvailableSlots #fromDate").val() == "") {
            utility.DisplayMessages("Date is required.", 3);
            return false;
        }
        schcheckout.loadAvailableAppointments().done(function (response) {
            var response = JSON.parse(response);
            if (response.AvailableAppointmentsCount > 0) {
                var ProviderId = null;
                var FacilityId = null;
                var Date = null;
                var AppointmentDetail = JSON.parse(response.AvailableAppointmentsLoad_JSON);
                $('#schcheckout #dvisPatientScheduled').removeClass('hidden');
                $("#schcheckout #dvisPatientScheduled").empty();
                $.each(AppointmentDetail, function (i, item) {
                    var labeltext = 'Patient is scheduled for ' + item.AppointmentDate + ", " + item.TimeFrom;

                    ProviderId = item.ProviderId;
                    FacilityId = schcheckout.params.FacilityId = item.FacilityId;
                    Date = item.Date;
                    var dt = item.Date; // AppointmentDetail[0].Date;
                    var year = dt.split('-')[0];
                    var month = dt.split('-')[1];
                    var day = dt.split('-')[2];
                    var finalDt = month + '/' + day + '/' + year;
                    $("#schcheckout #dvisPatientScheduled").append('<div><label class="blue" id="lblisPatientScheduled">' + labeltext + '</label></div>');
                    $("#pnlClinicalFollowUpTCM #chkisPatientScheduled" + i).val(item.TimeFrom);
                    schcheckout.scheduledSlots.push(item.TimeFrom);
                });
                // utility.callbackAfterAllDOMLoaded(function () { schcheckout.AvailableSlotsFill(ProviderId, FacilityId, Date); });
            } else {
                $('#schcheckout #dvisPatientScheduled').addClass('hidden');
                schcheckout.scheduledSlots = [];
                schcheckout.AvailableSlotsFill(null, null, null);
            }
        });
    },
    loadAvailableAppointments: function () {
        var scheduler = $("#schcheckout #dvSlots").data("kendoScheduler");
        var date;
        if (schcheckout.NavigationEvent) {
            date = kendo.toString(schcheckout.NavigationEvent.date, "d");
        } else {
            date = kendo.toString(scheduler.date(), "d");
        }

        var objData = {};
        objData["Date"] = date;
        objData["PatientId"] = schcheckout.params.PatientId
        objData["Provider"] = $("#chkOutProvider").data("kendoDropDownList").value();
        objData["Facility"] = $("#chkOutFacility").data("kendoDropDownList").value();
        objData["commandType"] = "load_patient_appointment";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FOLLOWUP", "ClinicalFollowUp");
    },
    AvailableSlotsFill: function (providerId, facilityId, Date) {
        schcheckout.loadAvailableSlots(providerId, facilityId, Date).done(function (response) {
            var response = JSON.parse(response);
            $("#" + schcheckout.params.PanelID + " #ulSlots tbody tr").remove();
            if (response.AvailableSlotsCount > 0) {
                var SlotsDetail = JSON.parse(response.AvailableSlotsLoad_JSON);
                if (SlotsDetail[0].FreeSlots < 1) {
                    $("#schcheckout #lblError").css("display", "block");
                    $("#schcheckout #lblError").text('SLOTS NOT AVAILABLE');
                    $("#schcheckout #dvisPatientScheduled").addClass('hidden');
                    $("#schcheckout #btnSchedule").addClass('hidden');
                    return;
                } else {
                    $("#schcheckout #lblError").css("display", "none");
                    var finalTr = '';
                    var counter = 2;
                    $.each(SlotsDetail, function (i, item) {
                        var isAlreadyScheduled = false;
                        $.each(schcheckout.scheduledSlots, function (ind, val) {
                            if (item.SlotTime == val)
                                isAlreadyScheduled = true;
                        });
                        if (!isAlreadyScheduled) {

                            if ($(finalTr).find('input').length % 4 == 0)
                                finalTr = finalTr + '<tr>';
                            finalTr = finalTr + '<td><div class="col-xs-6 p-xs"><div class="checkbox-custom">';
                            finalTr = finalTr + '<input  type="checkbox" onclick="schcheckout.changeTime(this);" name="Slot" value="' + item.SlotTime + '"  >';
                            finalTr = finalTr + '   <label class="control-label">' + item.SlotTime + '</label></div></div></td>';

                            if ($(finalTr).find('input').length % 4 == 0)
                                finalTr = finalTr + '</tr>';
                            if ($("#schcheckout #Duration").val() == "")
                                $("#schcheckout #Duration").val(item.SlotMinutes);
                        }
                    });
                    $("#" + schcheckout.params.PanelID + " #ulSlots tbody").append(finalTr);
                    $("#schcheckout #btnSchedule").removeClass('hidden');
                }
            } else
                $("#schcheckout #lblError").css("display", "block");
        });
    },
    loadAvailableSlots: function (providerId, facilityId, dt) {
        var objData = {};
        if (providerId != null)
            objData["Provider"] = providerId;
        else
            objData["Provider"] = $("#schcheckout #Provider").val();
        if (facilityId != null)
            objData["Facility"] = facilityId;
        else
            objData["Facility"] = $("#schcheckout #Facility").val();
        if (dt != null)
            objData["Date"] = dt;
        else {
            var dat = new Date();
            var frmDate = (dat.getMonth() + 1) + "/" + dat.getDate() + "/" + dat.getFullYear();
            objData["Date"] = $("#schcheckout #fromDate").val() == "" ? frmDate : $("#schcheckout #fromDate").val();
            if ($("#schcheckout #fromDate").val() == "") {
                $("#schcheckout #fromDate").datepicker('setDate', objData["Date"]);
            }
        }
        objData["commandType"] = "load_provider_available_slots";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FOLLOWUP", "ClinicalFollowUp");
    },
    changeTime: function (obj) {
        $("#schcheckout #ChooseSchedule").addClass('hidden');
        $("#schcheckout #fromTime").val($(obj).val());
        $('#dvSlotsDetail input').each(function () {
            if ($(this).val() != $(obj).val())
                $(this).prop('checked', false);
        });
    },
    FollowUpAppointmentSchedule: function () {


        if (schcheckout.validateSlotDetail()) {
            var dfd = $.Deferred();
            var self = $("#schcheckout #divAppointmentsAvailableSlots");
            var myJSON = self.getMyJSONByName();
            schcheckout.FollowUpAppointmentSchedule_DBCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    PMSScheduler.CanScheduler.dataSource.read();
                    //schcheckout.RefreshSchduler();
                    if (response.AppointmentId)
                        schcheckout.params.appid = response.AppointmentId;
                    utility.DisplayMessages(response.message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve();
                }
            });
            return dfd;
        }



    },
    InitializeResources: function () {
        var SchedulerResource = [];

        var facility_ = { field: "FacilityId", name: "facilities", dataSource: [] };
        var FacilityId = $('#schcheckout #divAppointmentsAvailableSlots #Facility').val();
        var FacilityName = $('#schcheckout #divAppointmentsAvailableSlots #Facility option:selected').text();
        facility_.dataSource.push({ text: FacilityName, value: FacilityId, color: null });
        SchedulerResource.push(facility_);

        var provider_ = { field: "ProviderId", name: "providers", dataSource: [] };
        var ProviderId = $('#schcheckout #divAppointmentsAvailableSlots #Provider').val();
        var ProviderName = $('#schcheckout #divAppointmentsAvailableSlots #Provider option:selected').text();
        provider_.dataSource.push({ text: ProviderName, value: ProviderId, groupType: 'Provider' });
        SchedulerResource.push(provider_);

        return SchedulerResource;
    },
    LoadScheduler: function (e) {
        schcheckout.LoadScheduler_DBCall().done(function (response) {
            if (response.status != false) {
                e.success(response.ProviderScheduleFill_JSON);
                var names = $('#schcheckout #divAppointmentsAvailableSlots .k-scheduler-header-wrap th');
                if (response.ProviderScheduleFill_JSON.length > 0) {
                    var providerName = response.ProviderScheduleFill_JSON[0].ProviderName;
                    var count = response.ProviderScheduleFill_JSON.length;
                    if (count && count > 0) {
                        $(names).html(providerName + " <span class='badge style='color:white;background: #D2312D;'>" + count + "</span>");
                    }
                }
                else {
                    var providerName = $("#chkOutProvider").data("kendoDropDownList").dataItem().name;
                    $(names).html(providerName + " <span class='badge style='color:white;background: #D2312D;'></span>");
                }
                schcheckout.LoadSchBlockHours();
            }
        });
    },
    LoadSchBlockHours: function () {
        var scheduler = $("#schcheckout #dvSlots").data("kendoScheduler");
        if (scheduler && scheduler._selectedViewName && scheduler._selectedViewName != "month") {
            schcheckout.LoadSchBlockHours_DBCall(scheduler._selectedViewName, scheduler).done(function (response) {
                if (response.status != false) {
                    if (response.BlockHoursCount > 0) {
                        PMSScheduler.ResetAllSlotSlotsColor(scheduler);
                        var BlockHoursLoadJSONData = JSON.parse(response.BlockHoursLoad_JSON);
                        var resourceArry = scheduler.resources[1].dataSource.options.data;
                        $.each(BlockHoursLoadJSONData, function (ind, item) {
                            var contentDiv = scheduler.element.find("div.k-scheduler-content");
                            var rows = contentDiv.find("tr");
                            if (scheduler._selectedViewName == "day") {
                                for (var i = 0; i < rows.length; i++) {
                                    var slot = scheduler.slotByElement(rows[i]);
                                    PMSScheduler.SetSlotColor(slot, resourceArry, item);
                                };
                            }
                            else {
                                for (var i = 0; i < rows.length; i++) {
                                    var tds = $(rows[i]).find('td');
                                    for (var s = 0; s < tds.length; s++) {
                                        var slot = scheduler.slotByElement(tds[s]);
                                        PMSScheduler.SetSlotColor(slot, resourceArry, item);
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
    LoadSchBlockHours_DBCall: function (ViewTypeName, scheduler) {
        var self = $("#schcheckout #divAppointmentsAvailableSlots");
        var myJSON = JSON.parse(self.getMyJSON());
        var objData = new Object();
        objData["ProviderIds"] = myJSON.Provider;
        objData["ResourceIds"] = "0";
        var StartDate = kendo.toString(new Date(), "d");
        if (schcheckout.NavigationEvent)
            StartDate = kendo.toString(schcheckout.NavigationEvent.date, "d");
        else if (scheduler && scheduler.date())
            StartDate = kendo.toString(scheduler.date(), "d");
        objData["start"] = StartDate;
        objData["SchViewType"] = ViewTypeName;
        objData["CommandType"] = "Load_Sch_BlockHours";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");
    },
    LoadScheduler_DBCall: function () {
        var scheduler = $("#schcheckout #dvSlots").data("kendoScheduler");
        var StartDate = kendo.toString(new Date(), "d");
        if (schcheckout.NavigationEvent)
            StartDate = kendo.toString(schcheckout.NavigationEvent.date, "d");
        else if (scheduler && scheduler.date())
            StartDate = kendo.toString(scheduler.date(), "d");
        var self = $("#schcheckout #divAppointmentsAvailableSlots");
        var myJSON = JSON.parse(self.getMyJSON());
        var objData = new Object();
        objData["ProviderId"] = $("#chkOutProvider").data("kendoDropDownList").value();
        objData["FacilityId"] = $("#chkOutFacility").data("kendoDropDownList").value();
        objData["TimeFrom"] = StartDate;
        objData["IsProvider"] = true;
        objData["CommandType"] = "SEARCH_WAITLIST_SCHEDULE";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Scheduler", "PMSScheduler");
    },
    OpenAppointmentDetail: function (mode, AppointmentId, ProviderId, ProviderName, ResourceId, ResourceName, FacilityId, FacilityName, ScheduleReasonId, Time, ToTime, SlotMinutes, ScheduleReason, ScheduleDate, isNoteCreated, checkin) {
        if (ScheduleDate)
            Scheduling_Calendar.disableAppointmentDate = false;
        else
            Scheduling_Calendar.disableAppointmentDate = true;
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["AppointmentId"] = AppointmentId;
                params["ProviderId"] = ProviderId;
                params["ProviderName"] = ProviderName;
                params["ResourceId"] = ResourceId;
                params["ResourceName"] = ResourceName;
                params["FacilityId"] = FacilityId;
                params["FacilityName"] = FacilityName;
                params["ScheduleReasonId"] = ScheduleReasonId;
                params["isNoteCreated"] = isNoteCreated;
                params["checkin"] = checkin;
                params["Time"] = Time;
                params["ToTime"] = ToTime;
                params["SlotMinutes"] = SlotMinutes;
                params["ScheduleReason"] = ScheduleReason;
                params["mode"] = mode;
                params["ScheduleDate"] = ScheduleDate;
                params["PatientId"] = schcheckout.params.PatientId;
                params["ParentCtrl"] = "schcheckout";
                LoadActionPan('appointmentDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    FollowUpAppointmentSchedule_DBCall: function (appointmentData) {
        var objData = JSON.parse(appointmentData);
        objData["commandType"] = "save_followup_appointment";
        objData["PatientId"] = schcheckout.params.PatientId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FOLLOWUP", "ClinicalFollowUp");
    },
    validateSlotDetail: function () {

        var IsTrue = true;
        var DOS = new Date();
        DOS.setHours(0, 0, 0, 0);
        var selectedDate = new Date($('#schcheckout #divAppointmentsAvailableSlots #fromDate').val());
        selectedDate.setHours(0, 0, 0, 0);

        if (!$("#schcheckout #fromDate").val()) {
            utility.DisplayMessages("Date is required.", 3);
            IsTrue = false;
        }
        else if (!$("#schcheckout #Facility").val()) {
            utility.DisplayMessages("Facility is required.", 3);
            IsTrue = false;
        }
        else if (!$("#schcheckout #Provider").val()) {
            utility.DisplayMessages("Provider is required.", 3);
            IsTrue = false;
        }
        else if (!$("#schcheckout #Duration").val()) {
            utility.DisplayMessages("Duration is required.", 3);
            IsTrue = false;
        }
        else if (selectedDate < DOS) {
            utility.DisplayMessages('you cannot schedule appointment on past date', 3);
            IsTrue = false;
        }
        else if ($('#dvSlotsDetail').find('input[name="Slot"]:checked').length > 0) {
            IsTrue = true;
            $("#schcheckout #ChooseSchedule").addClass('hidden');
        }
        else {
            IsTrue = false;
            $("#schcheckout #ChooseSchedule").removeClass('hidden');
        }
        return IsTrue;
    },
    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
            $("input#txtRefProvider").autocomplete({
                autoFocus: true,
                source: RefProviders, // pass an array
                select: function (event, ui) {
                    setTimeout(function () {
                        $("#schcheckout #hfRefProvider").val(ui.item.id);
                        if ($("#schcheckout #lnkRefProviderEdit").css("display") == "none") {
                            $("#schcheckout #lnkRefProviderEdit").css("display", "inline");
                            $("#schcheckout #lblRefProvider").css("display", "none");
                        }
                    }, 100);

                }
            });
        });
    },

    BindInsurancePlans: function (InsuranceJSON, InsuranceID) {
        var InsuranceJSON_detail = JSON.parse(InsuranceJSON);
        $("#frmcheckout #ddlInsurancePlan").empty();
        $("#frmcheckout #ddlInsurancePlan").append($('<option/>', {
            value: "",
            html: "- SELECT -",
            priority: "",
            coPayment: ""
        }));
        $.each(InsuranceJSON_detail, function (i, item) {
            $('#schcheckout #frmcheckout #ddlInsurancePlan').append(
                $('<option />', {
                    value: item.InsuranceId,
                    html: item.InsurancePlanName,
                    priority: item.PlanPriority,
                    coPayment: item.AmtCopay,
                    specialistCopay: item.SpecialistCopay
                })
            );

        });

        for (var i = 0; i < InsuranceJSON_detail.length; i++) {

            if (InsuranceID == InsuranceJSON_detail[i].InsuranceId) {

                $('#schcheckout #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                //$('#schcheckout #txtCopayment').val(InsuranceJSON_detail[i].AmtCopay);


                $('#schcheckout #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                if ($('#schcheckout #rdPCP').is(':checked')) {
                    //$('#schcheckout #txtCopayment').val(InsuranceJSON_detail[i].AmtCopay);
                }
                else if ($('#schcheckout #rdSpecialist').is(':checked')) {
                    //$('#schcheckout #txtCopayment').val(InsuranceJSON_detail[i].SpecialistCopay);
                }
            }

        }



    },

    FillInsuranceData: function (control, controlToChange) {

        var coPayment = $("#frmcheckout #ddlInsurancePlan option:selected").attr("coPayment");
        var specialistCopay = $("#frmcheckout #ddlInsurancePlan option:selected").attr("specialistCopay");

        if ($('#schcheckout #rdPCP').is(':checked')) {
            $("#frmcheckout #txtCopayment").val(coPayment);
        }
        else if ($('#schcheckout #rdSpecialist').is(':checked')) {
            $("#frmcheckout #txtCopayment").val(specialistCopay);
        }

        //$("#frmcheckout #txtCopayment").val(coPayment);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";

        params["ParentCtrl"] = "schcheckout";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    ResetRefProvider: function () {

        $('#schcheckout #hfRefProvider').val(null);

    },

    OpenRefProviderDetail: function () {
        //Admin_ReferringProvider.ReferringProviderEdit($('#pnlDemographic #hfRefProvider').val(), "patTabDemographic", "txtRefProvider");
        var params = [];
        params["ReferringProviderId"] = $('#schcheckout #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "schcheckout";

        LoadActionPan('referringproviderDetail', params);
    },

    ValidatePatientCheckOut: function () {
        $('#frmcheckout')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   //name: {
                   //    group: '.col-sm-4',
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
            schcheckout.PatientCheckOutSave();
        });
    },

    RefreshSchduler: function () {
        UnloadActionPan(schcheckout.params["ParentCtrl"], "actionPanCheckOut");
        if (Scheduling_Calendar.params.PanelID == "pnlScheduleMuliView") {
            Scheduling_MuliView.LoadMultipleViewCalendar();
        }

        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        //expression for week range
        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        //Month Regular Expression
        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

        if (schcheckout.params.PanelID == "pnlScheduleMuliView") {

            var providerid = schcheckout.params.ProviderId;
            var facilityid = schcheckout.params.FacilityId;
            var resourceid = schcheckout.params.ResourceId;
            var date = schcheckout.params.DayDate;
            var dateid = schcheckout.params.DateId;

            Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);


        }
        else if (schcheckout.params.PanelID == "schEditSlot") {
            schEditSlot.SelectSlotDetail(schcheckout.params.TimeslotDetailid, schcheckout.params.ProviderId, schcheckout.params.ResourceId);

            if (schcheckout.params.MultipleView == 1) {

                var providerid = schcheckout.params.ProviderId;
                var facilityid = schcheckout.params.FacilityId;
                var resourceid = schcheckout.params.ResourceId;
                var date = schcheckout.params.DayDate;
                var dateid = schcheckout.params.DateId;

                Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);

            }

            else if (schcheckout.params.DayDate.match(weekrg) && schcheckout.params.DayDate.length > 15) {
                var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                Scheduling_Calendar.ClearTable();

                var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                var week = Scheduling_Calendar.GetWeek(date1);
                $('#pnlScheduleCalendar #daydate span').html(week);
                //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
            }
            else {
                var statusslots = Scheduling_Calendar.FilterCriteria();
                if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
                    Scheduling_Calendar.DayCalendar(schcheckout.params.ProviderId, null, schcheckout.params.FacilityId, schcheckout.params.DayDate, statusslots);
                }
                else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                    Scheduling_Calendar.DayCalendar(null, schcheckout.params.ResourceId, schcheckout.params.FacilityId, schcheckout.params.DayDate, statusslots);
                }
                //Scheduling_Calendar.DayCalendar(schcheckout.params.ProviderId, schcheckout.params.ResourceId, schcheckout.params.FacilityId, schcheckout.params.DayDate, statusslots);
                if (schcheckout.params.ProviderId != "")
                    $("#pnlScheduleCalendar #Provider option[value=" + schcheckout.params.ProviderId + "]").attr('selected', 'selected');
                if (schcheckout.params.ResourceId != "")
                    $("#pnlScheduleCalendar #Resource option[value=" + schcheckout.params.ResourceId + "]").attr('selected', 'selected');
                if (schcheckout.params.FacilityId != "")
                    $("#pnlScheduleCalendar #Facility option[value=" + schcheckout.params.FacilityId + "]").attr('selected', 'selected');
                if (schcheckout.params.DayDate != "")
                    $('#pnlScheduleCalendar #daydate span').html(schcheckout.params.DayDate);
            }


        }

        else if (schcheckout.params.DayDate.match(weekrg) && schcheckout.params.DayDate.length > 15) {
            var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
            Scheduling_Calendar.ClearTable();

            var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
            var week = Scheduling_Calendar.GetWeek(date1);
            $('#daydate span').html(week);
            //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
            var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
        }

        else {
            var statusslots = Scheduling_Calendar.FilterCriteria();
            if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
                Scheduling_Calendar.DayCalendar(schcheckout.params.ProviderId, null, schcheckout.params.FacilityId, schcheckout.params.DayDate, statusslots);
            }
            else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                Scheduling_Calendar.DayCalendar(null, schcheckout.params.ResourceId, schcheckout.params.FacilityId, schcheckout.params.DayDate, statusslots);
            }
            //Scheduling_Calendar.DayCalendar(schcheckout.params.ProviderId, schcheckout.params.ResourceId, schcheckout.params.FacilityId, schcheckout.params.DayDate, statusslots);
            if (schcheckout.params.ProviderId != "")
                $("#Provider option[value=" + schcheckout.params.ProviderId + "]").attr('selected', 'selected');
            if (schcheckout.params.ResourceId != "")
                $("#Resource option[value=" + schcheckout.params.ResourceId + "]").attr('selected', 'selected');
            if (schcheckout.params.FacilityId != "")
                $("#Facility option[value=" + schcheckout.params.FacilityId + "]").attr('selected', 'selected');
            if (schcheckout.params.DayDate != "")
                $('#daydate span').html(schcheckout.params.DayDate);

        }
    },

    PatientCheckOutSave: function () {
        var strMessage = "";
        var self = $("#schcheckout");
        var myJSON = self.getMyJSON();

        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                schcheckout.SavePatientCheckOut(myJSON, schcheckout.params.PatientVisitId).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);

                        PMSScheduler.CanScheduler.dataSource.read();
                        UnloadActionPan(schcheckout.params["ParentCtrl"], "actionPanCheckOut");
                        //schcheckout.RefreshSchduler();
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

    FillPatientData: function (PatientId, AppointmentId) {
        var data = "PatientId=" + PatientId + "&AppointmentId=" + AppointmentId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_CHECKOUT", "FILL_PATIENT_DATA");
    },

    SavePatientCheckOut: function (CheckOutData, VisitId) {
        var data = "CheckOutData=" + CheckOutData + "&VisitId=" + VisitId + "&ReferralId=" + $('#schcheckout #hfReferralId').val();;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_CHECKOUT", "SAVE_PATIENT_CHECKOUT");
    },

    PrintLetter: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'schcheckout';
        LoadActionPan('designLetterPrinting', params);

    },

    UnLoad: function () {
        schcheckout.NavigationEvent = null;
        utility.UnLoadDialog('frmcheckout', function () {
            UnloadActionPan(schcheckout.params["ParentCtrl"], "actionPanCheckOut");
        }, function () {
            UnloadActionPan(schcheckout.params["ParentCtrl"], "actionPanCheckOut");
        });


    },
    //Function Name: addReferral
    //Author: Humaira Yousaf
    //Date :  22-08-2016
    //Description: Adds Referral
    addReferral: function () {
        var params = [];
        params["PatientId"] = schcheckout.params.PatientId;
        params["AccountNumber"] = $('#schcheckout #hfpatientaccount').val();
        params["PatientName"] = $('#schcheckout #hfpatientname').val();
        //params["ReferralToId"] = schcheckout.params.ProviderId;
        //params["Reason"] = $('#schcheckout #txtReason').val();
        //params["Status"] = 1; // Not Started    
        params["ParentCtrl"] = "schcheckout";

        if ($('#schcheckout #btnAddReferral').text() == 'Add Referral') {
            params["mode"] = "Add";
        }
        else {
            params["mode"] = "Edit";
            params["ReferralId"] = $('#schcheckout #hfReferralId').val();
        }
        LoadActionPan('Patient_Referrals_Outgoing_Detail', params);
    },
    //Function Name: SavePatientReferral
    //Author: Humaira Yousaf
    //Date :  05-10-2016
    //Description: Save Patient Referral
    SavePatientReferral: function () {
        var data = "AppointmentId=" + schcheckout.params.AppointmentId + "&ReferralId=" + $('#schcheckout #hfReferralId').val();
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_CHECKOUT", "UPDATE_APPOINTMENT_REFERRAL");
    },
}