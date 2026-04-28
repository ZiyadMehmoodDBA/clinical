var slotid;
var slotdetailid;
var calvalue;
Scheduling_Calendar = {
    bIsFirstLoad: true,
    params: [],
    slotHeight: 24,
    slotHeightWeek: 20,
    padding: -5,
    buttonHeightAdj: 7,
    zIndexDraggable: 1017,
    appoinmentSoltDtl: [],
    timeSlotDtlId: '',
    eSuperbillInfo: [],
    eSuperbillAddPermission: false,
    eSuperbillEditPermission: false,
    eSuperbillViewPermission: false,
    isVisitUpdForAppointment: false,
    Appcount: [],
    disableAppointmentDate:false,
    Load: function (params) {

        Scheduling_Calendar.params = params;

        //alert(utility.UserBrowser());
        $("#fromdate div:first").hide();
        if (Scheduling_Calendar.bIsFirstLoad) {
            Scheduling_Calendar.bIsFirstLoad = false;
            var self = $('#pnlScheduleCalendar');
            var events;
            self.loadDropDowns(true).done(function () {
                Scheduling_Calendar.LoadSchedulingDetail();
                Scheduling_Calendar.LoadScheduleFill();
                Scheduling_Calendar.HideShowDropDownLink();
                $("#pnlScheduleCalendar #ddlVisitTypeSc option[value='']").text("ALL");
                $("#pnlScheduleCalendar #ddlPatientTypeSc option[value='']").text("ALL");

            });
            Scheduling_Calendar.closeDropDownOnHtmlClick();
        } else if (Scheduling_Calendar.isVisitUpdForAppointment) {
            Scheduling_Calendar.LoadScheduleFill();
        } else {
            Scheduling_Calendar.LoadScheduleFill();
            Scheduling_Calendar.closeDropDownOnHtmlClick();
        }

        if ($("html").hasClass("sidebar-left-collapsed")) {
            $("html").removeClass("sidebar-left-collapsed");
        }
        if (globalAppdata['DefaultProviderId'] == "")
        { $('#pnlScheduleCalendar #btnForceBooking').attr("disabled", "disabled"); }
        else {
            $('#pnlScheduleCalendar #btnForceBooking').removeAttr("disabled")
        }
    },

    LoadScheduleFill: function () {
        $('#pnlScheduleCalendar #statusColorPanel').empty();
        Scheduling_Calendar.enableSlimScroll();


        MDVisionService.lookups("GetAppointmentStatus").done(function (result) {
            result = JSON.parse(result["GetAppointmentStatus"]);
            if (!Scheduling_Calendar.isVisitUpdForAppointment) {
                Scheduling_Calendar.ClearTable();
                Scheduling_Calendar.isVisitUpdForAppointment = false;
            }
            switch (globalAppdata["PreferredSchScreenName"]) {
                case 'CalendarDay':
                    $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date()));
                    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #daydate').text().trim());
                    $('#btnday').addClass('active');
                    $('#btnweek').removeClass('active');
                    $('#btnmnth').removeClass('active');
                    break;
                case 'CalendarWeek':
                    var a = $.datepicker.formatDate('mm/dd/yy', new Date());//$('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                    var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                    var week = Scheduling_Calendar.GetWeek(date1);
                    $('#pnlScheduleCalendar #daydate span').html(week);
                    $("#pnlScheduleCalendar #WeekCal").css("display", "");
                    $('#btnday').removeClass('active');
                    $('#btnweek').addClass('active');
                    $('#btnmnth').removeClass('active');
                    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #daydate').text().trim());
                    break;
                case 'CalendarMonth':
                    var month = $.datepicker.formatDate('MM/yy', new Date());
                    // Startcurrent month set
                    $('#pnlScheduleCalendar #daydate span').html(month); $("#pnlScheduleCalendar #MonthCal").css("display", "");
                    $('#btnday').removeClass('active');
                    $('#btnweek').removeClass('active');
                    $('#btnmnth').addClass('active');
                    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #daydate').text().trim());
                    break;
                default:
                    if ($('#pnlScheduleCalendar #daydate span').html() == "") {
                        $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date()));
                    }
                    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #daydate').text().trim());
                    $('#btnday').addClass('active');
                    $('#btnweek').removeClass('active');
                    $('#btnmnth').removeClass('active');
                    //   Scheduling_Calendar.DayClick(true);
                    break;
            }
            //$('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date()));
            //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #daydate').text().trim());
            if (result != false) {
                var classCol;
                $.each(result, function (key, value) {
                    if (result[key].Name.length < 13) {
                        classCol = "col-xs-6 pl-xxs pr-none";
                    }
                    else {
                        classCol = "col-xs-12 pl-xxs pr-none";
                    }

                    if (result[key].Name.toUpperCase() != '- SELECT -')
                        if (result[key].Name.toUpperCase() != 'CANCEL')

                            $('#pnlScheduleCalendar #statusColorPanel').append('<div id="' + result[key].Value + '" class="' + classCol + '">  <input id="' + result[key].Value + '" type="checkbox" class="cb-element" checked></input>                      <span class="color-reference-box" style="background-color:' + result[key].RefValue + ';"></span>                        <label class="control-label " for="color-reference-box">' + result[key].Name + '</label>                    </div>');
                    if (result[key].Name.toUpperCase() == 'CANCEL')
                        $('#pnlScheduleCalendar #statusColorPanel').append('<div id="' + result[key].Value + '" class="' + classCol + '">  <input id="' + result[key].Value + '" type="checkbox" class="cb-element"></input>                      <span class="color-reference-box" style="background-color:' + result[key].RefValue + ';"></span>                        <label class="control-label " for="color-reference-box">' + result[key].Name + '</label>                    </div>');
                });

            }

        });

    },

    AppointmentAddNew: function (mode, AppointmentId, ProviderId, ProviderName, ResourceId, ResourceName, FacilityId, FacilityName, ScheduleReasonId, Time, ToTime, SlotMinutes, ScheduleReason, ScheduleDate, isNoteCreated, checkin) {

        //var senderElement = event.target.tagName;
        //if (senderElement == 'TD') {
        if (ScheduleDate == null || ScheduleDate == "") {
            Scheduling_Calendar.disableAppointmentDate = false;
            ScheduleDate = $.datepicker.formatDate('mm/dd/yy', new Date());
        } else {
            Scheduling_Calendar.disableAppointmentDate = true;
        }
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                //params["SlotId"] = Slotid;
                //params["SlotdetailId"] = TimeSlotDtlId;
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

                //params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
                params["Time"] = Time;
                params["ToTime"] = ToTime;
                params["SlotMinutes"] = SlotMinutes;
                //params["IsSpecialist"] = IsSpecialist;
                params["ScheduleReason"] = ScheduleReason;
                params["mode"] = mode;
                params["ScheduleDate"] = ScheduleDate;
                params["ParentCtrl"] = "schTabCalendar";
                LoadActionPan('appointmentDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //}
    },

    AppointmentAdd: function (Slotid, TimeSlotDtlId, ProviderId, ProviderName, ResourceId, ResourceName, FacilityId, FacilityName, ScheduleReasonId, event, Time, ToTime, IsSpecialist, SlotMinutes, ScheduleReason, ScheduleDate) {

        var senderElement = event.target.tagName;
        if (senderElement == 'TD') {

            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {

                    if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {


                        var params = [];
                        params["SlotId"] = Slotid;
                        params["SlotdetailId"] = TimeSlotDtlId;

                        params["ProviderId"] = ProviderId;
                        params["ProviderName"] = ProviderName;
                        params["ResourceId"] = null;
                        params["ResourceName"] = ResourceName;
                        params["FacilityId"] = FacilityId;
                        params["FacilityName"] = FacilityName;
                        params["ScheduleReasonId"] = ScheduleReasonId;
                        params["mode"] = "Add";
                        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();

                        params["Time"] = Time;
                        params["ToTime"] = ToTime;
                        params["SlotMinutes"] = SlotMinutes;
                        params["IsSpecialist"] = IsSpecialist;
                        params["ScheduleReason"] = ScheduleReason;

                        params["ScheduleDate"] = ScheduleDate;

                        params["ParentCtrl"] = "schTabCalendar";
                        LoadActionPan('appointmentDetail', params);
                    }
                    else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                        var params = [];
                        params["SlotId"] = Slotid;
                        params["SlotdetailId"] = TimeSlotDtlId;

                        params["ProviderId"] = null;
                        params["ProviderName"] = null;
                        params["ResourceId"] = ResourceId;
                        params["ResourceName"] = ResourceName;
                        params["FacilityId"] = FacilityId;
                        params["FacilityName"] = FacilityName;
                        params["ScheduleReasonId"] = ScheduleReasonId;

                        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
                        params["Time"] = Time;
                        params["ToTime"] = ToTime;
                        params["SlotMinutes"] = SlotMinutes;
                        params["IsSpecialist"] = IsSpecialist;
                        params["ScheduleReason"] = ScheduleReason;
                        params["mode"] = "Add";
                        params["ScheduleDate"] = ScheduleDate;
                        params["ParentCtrl"] = "schTabCalendar";
                        LoadActionPan('appointmentDetail', params);

                    }

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    AppointmentEdit: function (appid, visitid, checkin, duration, noteid) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var isNoteCreated = false;
                isNoteCreated = noteid == "0" ? false : true;

                if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
                    var params = [];
                    params["checkin"] = checkin;
                    params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
                    params["ResourceId"] = null;
                    params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
                    params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
                    params["AppointmentId"] = appid;
                    params["PatientVisitId"] = visitid;
                    params["SlotMinutes"] = duration;
                    params["isNoteCreated"] = isNoteCreated;
                    params["mode"] = "Edit";
                    if (Scheduling_Calendar.params.PanelID == "pnlScheduleCalendar") {
                        params["ParentCtrl"] = 'schTabCalendar';
                    }
                    else {
                        params["ParentCtrl"] = 'schTabMultipleView';
                    }
                    LoadActionPan('appointmentDetail', params);

                }


                else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                    var params = [];
                    params["checkin"] = checkin;
                    params["ProviderId"] = null;
                    params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
                    params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
                    params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
                    params["AppointmentId"] = appid;
                    params["PatientVisitId"] = visitid;
                    params["SlotMinutes"] = duration;
                    params["isNoteCreated"] = isNoteCreated;
                    params["mode"] = "Edit";
                    params["ParentCtrl"] = "schTabCalendar";
                    LoadActionPan('appointmentDetail', params);

                }


            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    AppointmentStatusUpdate: function (appid, statusid) {

        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Scheduling_Calendar.UpdateAppointmentStatus(appid, statusid).done(function (response) {
                    if (response.status != false) {



                        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                        //expression for week range
                        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                        //Month Regular Expression
                        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

                        var date = $('#daydate').text().trim();

                        if (date.match(weekrg) && date.length > 15) {
                            var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                            Scheduling_Calendar.ClearTable();

                            var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                            var week = Scheduling_Calendar.GetWeek(date1);
                            $('#pnlScheduleCalendar #daydate span').html(week);

                            var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                        }

                        else {

                            var providerid = $('#pnlScheduleCalendar #Provider').val();
                            var resourceid = $('#pnlScheduleCalendar #Resource').val();
                            var facilityid = $('#pnlScheduleCalendar #Facility').val();

                            var statusslots = Scheduling_Calendar.FilterCriteria();

                            if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                                Scheduling_Calendar.DayCalendar(providerid, null, facilityid, date, statusslots);
                            }

                            else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                                Scheduling_Calendar.DayCalendar(null, resourceid, facilityid, date, statusslots);

                            }

                            if (providerid != "")
                                $("#pnlScheduleCalendar #Provider option[value=" + providerid + "]").attr('selected', 'selected');
                            if (resourceid != "")
                                $("#pnlScheduleCalendar #Resource option[value=" + resourceid + "]").attr('selected', 'selected');
                            if (facilityid != "")
                                $("#pnlScheduleCalendar #Facility option[value=" + facilityid + "]").attr('selected', 'selected');
                            if (date != "")
                                $('#pnlScheduleCalendar #daydate span').html(date);

                        }

                        utility.DisplayMessages(response.Message, 1);

                    }
                    else {
                        utility.DisplayMessages(response.Message, 2);
                    }
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    AppointmentByStatus: function (ProviderId, FacilityId, SlotDate, color, ResourceId) {


        if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

            var params = [];
            params["ParentCtrl"] = "schTabCalendar";
            params["ProviderId"] = ProviderId;
            params["FacilityId"] = FacilityId;
            params["SlotDate"] = SlotDate;
            params["color"] = color;
            params["ResourceId"] = null;
            if (color != '#f6f6f6')
                LoadActionPan('schAppointmentStatus', params);

        }

        else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

            var params = [];
            params["ParentCtrl"] = "schTabCalendar";
            params["ProviderId"] = null;
            params["FacilityId"] = FacilityId;
            params["SlotDate"] = SlotDate;
            params["color"] = color;
            params["ResourceId"] = ResourceId;
            if (color != '#f6f6f6')
                LoadActionPan('schAppointmentStatus', params);

        }

    },

    DayClick: function (isLoadData) {
        $('#pnlScheduleCalendar #txtgoto').val('');
        $("#pnlScheduleCalendar #btngoto").addClass('disableAll');
        $('#btnday').addClass('active');
        $('#btnweek').removeClass('active');
        $('#btnmnth').removeClass('active');
        $("#pnlScheduleCalendar #checkboxpanel").css("display", "none");
        $("#pnlScheduleCalendar #yeardiv").css("display", "none");
        $("#pnlScheduleCalendar #monthdiv").css("display", "none");
        $("#pnlScheduleCalendar #fromdate").css("display", "");
        $('#pnlScheduleCalendar #agenda').val('P1');
        $("#pnlScheduleCalendar #btnblock").css("display", "");
        $("#pnlScheduleCalendar #btnunblock").css("display", "");
        $("#pnlScheduleCalendar #facilityheader").css("display", "");
        $("#pnlScheduleCalendar #proresheader").css("display", "");
        $("#pnlScheduleCalendar #MonthCal").css("display", "none");
        $("#pnlScheduleCalendar #WeekCal").css("display", "none");
        $('#pnlScheduleCalendar #btnProviderPrint').removeAttr("disabled");

        //Bug # PMS-2816 By Mohsin Nasir

        var currentDate = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
        if (currentDate == 'Invalid Date') {
            currentDate = new Date();
        }
        Scheduling_Calendar.ClearTable();
        $('#divDayName').removeAttr('style');
        $('#divDayName').html($.datepicker.formatDate('DD', new Date(currentDate)) + ", ");
        $('#daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date(currentDate)));
        if (isLoadData)
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", currentDate);




        //var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        ////expression for week range
        //var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        ////Month Regular Expression
        //var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
        //var criteria = $('#pnlScheduleCalendar #daydate').text().trim();
        //if (criteria.match(dayrgx) && criteria.length < 15) {
        //    var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
        //    if (a == undefined || a == 'Invalid Date') {
        //        var x = $('#pnlScheduleCalendar #daydate').text().trim();
        //        Scheduling_Calendar.ClearTable();
        //        $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date(x)));

        //        $('#pnlScheduleCalendar #fromdate').datepicker("setDate", x);
        //    }
        //    else {
        //        Scheduling_Calendar.ClearTable();
        //        $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date(a)));

        //        $('#pnlScheduleCalendar #fromdate').datepicker("setDate", a);
        //    }

        //}

        //else if (criteria.match(weekrg)) {
        //    var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
        //    if (a == 'Invalid Date') {
        //        a = new Date();
        //    }
        //    Scheduling_Calendar.ClearTable();
        //    $('#daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date(a)));

        //    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", a);
        //}
        //else if (criteria.match(monthreg)) {
        //    var a = $('#pnlScheduleCalendar #calmonth').val();
        //    if (a == 'Invalid Date') {
        //        a = new Date();
        //    }

        //    var b = $.datepicker.formatDate('mm/dd/yy', new Date(Scheduling_Calendar.FormatDate(a)));
        //    Scheduling_Calendar.ClearTable();
        //    $('#pnlScheduleCalendar #daydate span').html(b);
        //    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", b);

        //}
        //END Bug # PMS-2816 By Mohsin Nasir

    },

    Initializationtooltip: function () {

        $('.helllo').each(function (i, obj) {

            $(obj).attr('id');

            var clickid = $(obj).attr('id');
            var arr = clickid.split('-');
            var patientid = arr[0];
            var appid = arr[1];
            Scheduling_Calendar.PatientDetail(patientid, appid, clickid);


        });
    },

    DayCalendar: function (ProviderId, ResourceId, FacilityId, CriteriaDate, statusid, patientTypeId, visitTypeId) {

        Scheduling_Calendar.Appcount = [];
        $("#pnlScheduleCalendar #checkboxpanel").css("display", "none");
        $("#pnlScheduleCalendar #yeardiv").css("display", "none");
        $("#pnlScheduleCalendar #monthdiv").css("display", "none");
        $("#pnlScheduleCalendar #fromdate").css("display", "");
        $('#pnlScheduleCalendar #agenda').val('P1');
        $("#pnlScheduleCalendar #btnblock").css("display", "");
        $("#pnlScheduleCalendar #btnunblock").css("display", "");
        if (CriteriaDate == undefined)
            CriteriaDate = $('#pnlScheduleCalendar #daydate span').text();
        Scheduling_Calendar.ClearTable();
        $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date()));
        var appstatuses;
        var appstatus;
        var reminderType;
        Scheduling_Calendar.appoinmentSoltDtl = [];
        Scheduling_Calendar.timeSlotDtlId = '';
        if (patientTypeId == undefined || patientTypeId == null) {
            patientTypeId = $('#pnlScheduleCalendar #ddlPatientTypeSc').val();
        }
        if (visitTypeId == undefined || visitTypeId == null) {
            visitTypeId = $('#pnlScheduleCalendar #ddlVisitTypeSc').val();
        }
        MDVisionService.lookups("GetAppointmentStatus").done(function (result) {
            result = JSON.parse(result["GetAppointmentStatus"]);
            appstatus = result;
            Scheduling_Calendar.SearchDaySlotSchedule(ProviderId, ResourceId, FacilityId, CriteriaDate, statusid, patientTypeId, visitTypeId).done(function (response) {
                if (response.status != false) {
                    var Slot_Detail = JSON.parse(response.ProviderScheduleFill_JSON);
                    //Try to get tbody first with jquery children. works faster!
                    var providername = $('#pnlScheduleCalendar #Provider option:selected').text();
                    var resourcename = $('#pnlScheduleCalendar #Resource option:selected').text();
                    var facilityname = $('#pnlScheduleCalendar #Facility option:selected').text();
                    Scheduling_Calendar.eSuperbillAddPermission = response.addPermission == "" ? true : false;
                    Scheduling_Calendar.eSuperbillEditPermission = response.editPermission == "" ? true : false;
                    Scheduling_Calendar.eSuperbillViewPermission = response.viewPermission == "" ? true : false;

                    var tbody = $('#pnlScheduleCalendar #myTable').children('tbody');
                    //Then if no tbody just select your table
                    var table = tbody.length ? tbody : $('#pnlScheduleCalendar #myTable');

                    if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

                        if (resourcename.toUpperCase() == '- SELECT -') {

                            resourcename = "";

                            $('#proresheader').text('Resource: ' + resourcename);

                        }
                        else {
                            $('#proresheader').text('Resource: ' + resourcename);
                        }
                        if (facilityname.toUpperCase() == '- SELECT -') {


                            facilityname = "";
                            $('#facilityheader').text('Facility: ' + facilityname);

                        }
                        else {
                            $('#facilityheader').text('Facility: ' + facilityname);
                        }

                    }

                    if ($('#pnlScheduleCalendar #rdprovider').is(':checked')) {
                        if (providername.toUpperCase() == '- SELECT -') {
                            providername = "";

                            $('#proresheader').text('Provider: ' + providername);

                        }
                        else {
                            $('#proresheader').text('Provider: ' + providername);
                        }
                        if (facilityname.toUpperCase() == '- SELECT -') {

                            facilityname = "";
                            $('#facilityheader').text('Facility: ' + facilityname);

                        }
                        else {
                            $('#facilityheader').text('Facility: ' + facilityname);
                        }
                    }
                    var appids = [];
                    var slotappcount;
                    for (var i = 0; i < Slot_Detail.length; i++) {
                        var appointment;
                        var margin = Slot_Detail[i].MaxCountApp;


                        var provid = Slot_Detail[i].ProviderId;
                        if (provid == '' || provid == 'undefined')
                            provid = null;


                        var refprovid = Slot_Detail[i].RefProviderId;
                        if (refprovid == '' || refprovid == 'undefined')
                            refprovid = null;

                        var provname = Slot_Detail[i].ProviderName;
                        if (provname == '' || provname == 'undefined')
                            provname = "";

                        var resid = Slot_Detail[i].ResourceId;
                        if (resid == '' || resid == 'undefined')
                            resid = null;
                        var resname = Slot_Detail[i].ResourceName;
                        if (resname == '' || resname == 'undefined')
                            resname = "";
                        var facid = Slot_Detail[i].FacilityId;
                        if (facid == '' || facid == 'undefined')
                            facid = null;
                        var facname = Slot_Detail[i].FacilityName;
                        if (facname == '' || facname == 'undefined')
                            facname = "";

                        var facphoneno = Slot_Detail[i].FacilityPhoneNo;
                        if (!facphoneno)
                            facphoneno = "";

                        facphoneno = facphoneno.replace(/[_\W]+/g, "");

                        var resonid = Slot_Detail[i].ScheduleReasonId;
                        if (resonid == '' || resonid == 'undefined')
                            resonid = null;

                        var color = "";

                        if (Slot_Detail[i].FacilityColor == "" || Slot_Detail[i].FacilityColor == "undefined" || Slot_Detail[i].FacilityColor == "null" || Slot_Detail[i].FacilityColor == null) {
                            color = '';
                        } else {
                            color = 'bgcolor = \'' + Slot_Detail[i].FacilityColor + '\'';
                        }


                        var appointments = '<div class="pull-left">&nbsp;</div>';
                        if (Slot_Detail[i].AppDtl != "") {
                            var app;
                            var test = 0;

                            var data = Slot_Detail[i].AppDtl;
                            var arr = data.split('|');

                            for (var j = 0; j < arr.length; j++) {

                                // Index 19 is added for non billable bit

                                slotappcount = (arr.length) - 1;
                                var split = arr[j].split(',')
                                if (split.length == 22 || split.length == 24) {
                                    var appid = split[0];

                                    // MK
                                    //if (!Scheduling_Calendar.Appcount.indexOf(appid) > -1)
                                    //    Scheduling_Calendar.Appcount.push(appid);

                                    var hasMatch = Scheduling_Calendar.Appcount.some(function (_appointmentid) {
                                        return _appointmentid === appid;
                                    });
                                    if (!hasMatch) {
                                        Scheduling_Calendar.Appcount.push(appid);
                                    }


                                    var removeItem = '0';
                                    Scheduling_Calendar.Appcount = jQuery.grep(Scheduling_Calendar.Appcount, function (value) {
                                        return value != removeItem;
                                    });

                                    var patientid = split[1];
                                    var patientname = split[2].replace(/#@#/g, ',');

                                    patientname = patientname.replace('-', ', ');

                                    var appointmentstatus = split[3];
                                    var appcolor = split[4];
                                    var appcopay = split[6];
                                    var appreason = split[3];
                                    var appcount = split[7];
                                    var appcomments = split[8];

                                    var appschreason = split[5];
                                    appschreason = appschreason.replace(/#@#/g, ',');

                                    var patientvisitid = split[9];
                                    var patvisitstatusid = split[10];
                                    var patvisitname = split[11];
                                    var patEligibility = split[12];

                                    //start clinical notes check
                                    var noteid = split[13];

                                    //For last Status
                                    var lastStatusId = split[14];
                                    var lastStatusName = split[15];

                                    // Start 01/12/2015 Muhammad Irfan Bug # EMR-9

                                    var noteStatus = split[16];
                                    var patientType = split[17];
                                    var visitType = split[18];
                                    // End 01/12/2015 Muhammad Irfan Bug # EMR-9

                                    var isNonBillable = split[19];

                                    var billInfoId = split[20];
                                    var billInfoStatus = split[21];

                                    if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {
                                        if (split.length == 24) {
                                            provid = split[22];
                                            provname = split[23].replace('-', ', ');
                                        }
                                    }
                                    var noteTitle = "";
                                    var isNoteCreated = false;

                                    if (noteid == "0") {
                                        noteTitle = "Create Note";
                                        isNoteCreated = false;
                                    } else {
                                        noteTitle = "Edit Note";
                                        isNoteCreated = true;
                                    }
                                    var isViewNote = false;
                                    if (patvisitname.toUpperCase() == "CHECK OUT") {


                                        if (noteStatus == "Signed") {
                                            noteTitle = "View Note";
                                            isNoteCreated = true;
                                            isViewNote = true;
                                        }
                                    }
                                    //end clinical notes check

                                    //Edit By Mohsin Nasir Bug # 2931
                                    //Add New Legend(blue circle) for Eligibility.

                                    var eligibilityIcon;

                                    //Edit By Mohsin Nasir Bug # 2931
                                    //Add New Legend(blue circle) for Eligibility.
                                    //if (arr.length > 2 && appcount != undefined) {

                                    //    Scheduling_Calendar.timeSlotDtlId = '{"TimeSlotDtlId"' + ': ' + '"checkbox' + Slot_Detail[i].TimeSlotDtlId + '",' + '"SlotCount"' + ':' + appcount + ', "AppointmentId" :' + appid + ', "PatientId" :' + patientid + '}';
                                    //    var jsonRes = JSON.parse(Scheduling_Calendar.timeSlotDtlId);
                                    //    var resultArray = $.grep(Scheduling_Calendar.appoinmentSoltDtl, function (e) {
                                    //        return e.AppointmentId == appid && e.PatientId == patientid;

                                    //    });
                                    //    if (resultArray.length == 0) {
                                    //        Scheduling_Calendar.appoinmentSoltDtl.push(jsonRes);
                                    //    }

                                    //}
                                    if (patEligibility == "Active") {
                                        eligibilityIcon = '<i class="fa fa-check-circle green"></i>';
                                    }
                                    else if (patEligibility == "Inactive") {
                                        eligibilityIcon = '<i class="fa fa-times-circle red"></i>';
                                    }
                                    else if (patEligibility.toLowerCase() == "waiting") {
                                        eligibilityIcon = '<i class="fa fa-circle black"></i>';
                                    }
                                    else {
                                        eligibilityIcon = '<i class="fa fa-circle blue"></i>';
                                    }
                                    //END Edit By Mohsin Nasir Bug # 2931

                                    appcopay = appcopay.split('-');
                                    var copaycolor;

                                    var appcopayamt = parseFloat(appcopay[0]);
                                    //azam aftab dated 2/12/2016 PMS-3940
                                    if (appcopay.length == 3) {
                                        var appcopayamtpaid = -Math.abs(parseFloat(appcopay[2]));
                                    } else {
                                        var appcopayamtpaid = parseFloat(appcopay[1]);
                                    }
                                    //end dated 2/12/2016 PMS-3940

                                    //if (appcopayamtpaid == appcopayamt) {
                                    //    copaycolor = 'Green';//copay blance
                                    //}

                                    //if (appcopayamtpaid > 0 && appcopayamtpaid < appcopayamt) {
                                    //    copaycolor = 'Black';//copay blance
                                    //}

                                    //if (appcopayamtpaid == 0 && appcopayamt > appcopayamtpaid) {
                                    //    appcopay = appcopayamt;
                                    //    copaycolor = 'Red'; // amount copay
                                    //}



                                    if (appcopayamtpaid == 0) {
                                        copaycolor = 'Green'; //copay paid
                                    }
                                    if (appcopayamtpaid > 0) {
                                        copaycolor = 'Black';//copay blance
                                    }
                                    if (appcopayamtpaid == appcopayamt && appcopayamtpaid > 0) {
                                        appcopay = appcopayamt;
                                        copaycolor = 'Red'; // amount copay
                                    }

                                    appids.push(appid);

                                    for (var w = 0; w < appstatus.length; w++) {
                                        if (appstatus[w].Name.toUpperCase() != "- SELECT -" && appstatus[w].Name != appointmentstatus && appstatus[w].Name.toUpperCase() != "CHECK IN" && appstatus[w].Name.toUpperCase() != "CHECK OUT")
                                            appstatuses += '<li data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id=' + appstatus[w].Value + ' ><a onclick="Scheduling_Calendar.AppointmentStatusUpdate(' + appid + ',' + appstatus[w].Value + ');" href="#" >' + appstatus[w].Name + '</a></li> ';
                                    }
                                    appstatuses = appstatuses.replace('undefined', '');
                                    var showapp = 0;

                                    for (z = 0; z < appids.length; z++) {
                                        if (appids[z] == appid && appid != 0) {
                                            showapp = showapp + 1;
                                        }

                                    }
                                    /* Start Code for Reminder Call */
                                    var sms = "SMS";
                                    var voice = "VOICE";
                                    var email = "EMAIL";
                                    var Patient_email = "";
                                    var Appointmentdatetime = "";
                                    var NotesVisitTypeId = Slot_Detail[i].PatientDetail.split(',')[17];
                                    var NotePatientTypeId = Slot_Detail[i].PatientDetail.split('|')[0].split(',')[18];
                                    try {
                                        Patient_email = Slot_Detail[i].PatientDetail.split(',')[10];
                                        Appointmentdatetime = $('#pnlScheduleCalendar #daydate span').text() + " " + Slot_Detail[i].FromTimeSlots;
                                    } catch (ex) {
                                        console.log(ex);
                                    }
                                    reminderType = '<li id=' + appid + ' ><a onclick="Scheduling_Calendar.SendQuickReminder(' + appid + ',\'' + sms + '\',' + patientid + ',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + facphoneno + '\');" href="#" >SMS</a></li> <li id=' + appid + ' ><a onclick="Scheduling_Calendar.SendQuickReminder(' + appid + ',\'' + voice + '\',' + patientid + ',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + facphoneno + '\');" href="#" >Voice</a></li> <li id=' + appid + ' ><a onclick="Scheduling_Calendar.SendQuickReminder(' + appid + ',\'' + email + '\',' + patientid + ',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + facphoneno + '\');" href="#" >Email</a></li> ';
                                    var appointmentReminder = '';
                                    if (appointmentstatus.toUpperCase() == 'PENDING' || appointmentstatus.toUpperCase() == 'SCHEDULED') {
                                        appointmentReminder = '<li class="dropdown-submenu"><a tabindex="-1" href="#" onclick="">Appointment Reminder</a><ul> ' + reminderType + '   </ul> </li>';
                                    }
                                    /* End Code for Reminder Call*/
                                    if ($.trim(appcount) > '1' && showapp == 1) {
                                        // alert(appcount * Scheduling_Calendar.slotHeight)

                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL' && appointmentstatus.toUpperCase() != "NO SHOW")
                                            appointments = appointments + '<div class="appdrag apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a  href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>        <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li>' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                            appointments = appointments + '<div class="appdrag apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>  <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            if (isNonBillable == "1") {
                                                eSuperbillLink = '';
                                            }
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',null,' + provid + ',\'' + provname + '\',\'' + NotesVisitTypeId + '\'' + ',\'' + NotePatientTypeId + '\'' + ');">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> ' + appointmentReminder + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>' + eSuperbillLink + '</ul>  </div></div> ';
                                        }
                                        //------------------ Start 01/12/2015 Muhammad Irfan For EMR-9
                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED") {


                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            if (isNonBillable == "1") {
                                                eSuperbillLink = '';
                                            }
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> ' + appointmentReminder + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>' + eSuperbillLink + '     </ul>  </div></div> ';

                                        }
                                        //------------------ End 01/12/2015 Muhammad Irfan For EMR-9

                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL') {
                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLinkForCheckOut(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            //if (isNonBillable == "1") {
                                            //    eSuperbillLink = '';
                                            //}
                                            var etemp = eSuperbillLink;
                                            eSuperbillLink = "";
                                            if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> ' + eSuperbillLink + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + ',\'' + provid + '\'' + "," + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ',' + patientvisitid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + '  <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li></ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ',' + patientvisitid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + ' <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li>  </ul>  </div></div> ';
                                            }
                                        }
                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + facid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>    </ul>  </div></div> ';
                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK OUT' && patvisitname.toUpperCase() != 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL')
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + facid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> ' + appointmentReminder + '</li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                    }

                                    if ($.trim(appcount) == '1' && showapp == 1) {

                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL' && appointmentstatus.toUpperCase() != "NO SHOW")
                                            appointments = appointments + '<div class="appdrag apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span> </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li id="' + appid + '"><a  href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>              <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                            appointments = appointments + '<div class="appdrag apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span> </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a  href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li>         <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                        if (patientvisitid != "" && (patvisitname.toUpperCase() == 'CHECK IN') && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            if (isNonBillable == "1") {
                                                eSuperbillLink = '';
                                            }
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',null,' + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> ' + appointmentReminder + '   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> ' + eSuperbillLink + '   </ul>  </div></div> ';
                                        }
                                        //------------------ Start 01/12/2015 Muhammad Irfan For EMR-9
                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED") {

                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            if (isNonBillable == "1") {
                                                eSuperbillLink = '';
                                            }
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> ' + appointmentReminder + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>' + eSuperbillLink + '   </ul>  </div></div> ';
                                        }
                                        //------------------ End 01/12/2015 Muhammad Irfan For EMR-9
                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL') {


                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLinkForCheckOut(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            //if (isNonBillable == "1") {
                                            //    eSuperbillLink = '';
                                            //}
                                            var etemp = eSuperbillLink;
                                            eSuperbillLink = "";

                                            if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> ' + eSuperbillLink + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + '  <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li></ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + ' <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li>  </ul>  </div></div> ';
                                            }


                                            //if ($(eSuperbillLink).find('a').text() == "View eSuperbill") {
                                            //    appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1);">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li>  ' + eSuperbillLink + ' <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>  </ul>  </div></div> ';
                                            //} else {
                                            //    appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1);">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li>' + eSuperbillLink + '   </ul>  </div></div> ';
                                            //}
                                        }
                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>    </ul>  </div></div> ';
                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK OUT' && patvisitname.toUpperCase() != 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL')
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                    }
                                    if (showapp != 1) {

                                    }
                                    appointments = appointments.replace("undefined", "");
                                    appstatuses = '';
                                    test++;
                                }
                            }

                        }

                        if (Slot_Detail[i].BlockUnblock == "Blocked") {
                            table.append('<tr id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '"><td onclick="Scheduling_Calendar.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ');" width="63"><a href="#">' + Slot_Detail[i].FromTimeSlots + '</a></td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td width="20"><input type="checkbox" name="checkbox" id="checkbox' + Slot_Detail[i].TimeSlotDtlId + '"></td><td BGCOLOR="#f88379" data-blocked="true" style="color:#fff;" data-FromTime="' + Slot_Detail[i].FromTimeSlots + '" data-ToTime="' + Slot_Detail[i].ToTimeSlots + '" data-slotId="Slotid' + Slot_Detail[i].TimeSlotDtlId + '" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '" >Blocked:' + Slot_Detail[i].BlockReason + '-' + Slot_Detail[i].Comments + ' </td></tr>');
                        }
                        else if (appointments == '<div class="pull-left">&nbsp;</div>' && Slot_Detail[i].IsAppointmetExist != 1) {

                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_Calendar.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ');" width="63"><a href="#">' + Slot_Detail[i].FromTimeSlots + '</a></td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td width="20"><input type="checkbox" name="checkbox"  id="checkbox' + Slot_Detail[i].TimeSlotDtlId + '"></td><td class="appdrop slot" ' + color + ' style="position:relative;" onclick="Scheduling_Calendar.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" data-FromTime="' + Slot_Detail[i].FromTimeSlots + '" data-ToTime="' + Slot_Detail[i].ToTimeSlots + '" data-slotId="Slotid' + Slot_Detail[i].TimeSlotDtlId + '" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',' + Slot_Detail[i].FromTimeSlots + '">' + appointments + '</td></tr>');
                        }
                        else {
                            var chkBoxByid = "checkbox" + Slot_Detail[i].TimeSlotDtlId;
                            //var resultArr = $.grep(Scheduling_Calendar.appoinmentSoltDtl, function (e) {

                            //    return e.AppointmentId == appid && e.PatientId == patientid;
                            //    //Scheduling_Calendar.appoinmentSoltDtl
                            //});
                            //if (appcount != undefined && resultArr.length == 0) {
                            //    Scheduling_Calendar.timeSlotDtlId = '{"TimeSlotDtlId"' + ': ' + '"checkbox' + Slot_Detail[i].TimeSlotDtlId + '",' + '"SlotCount"' + ':' + appcount + ', "AppointmentId" :' + appid + ', "PatientId" :' + patientid + '}';
                            //    var jsonRes = JSON.parse(Scheduling_Calendar.timeSlotDtlId);
                            //    Scheduling_Calendar.appoinmentSoltDtl.push(jsonRes);
                            //}
                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_Calendar.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ');" width="63"><a href="#"> ' + Slot_Detail[i].FromTimeSlots + '</a></td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td width="20"><input type="checkbox" name="checkbox" id="checkbox' + Slot_Detail[i].TimeSlotDtlId + '"></td><td class="appdrop slot" style="position:relative;" ' + color + '  onclick="Scheduling_Calendar.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" data-FromTime="' + Slot_Detail[i].FromTimeSlots + '" data-ToTime="' + Slot_Detail[i].ToTimeSlots + '" data-slotId="Slotid' + Slot_Detail[i].TimeSlotDtlId + '" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',' + Slot_Detail[i].FromTimeSlots + '"  >' + appointments + '</td></tr>');
                        }
                        appointments = '';
                        Scheduling_Calendar.PatientDetails(Slot_Detail[i].PatientDetail, Slot_Detail[i].AppDtl);
                    }
                    $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                    Scheduling_Calendar.MovePatientAppointment();

                }
                else {

                    Scheduling_Calendar.ClearTable();
                    var tbody = $('#pnlScheduleCalendar #myTable').children('tbody');
                    var providername = $('#pnlScheduleCalendar #Provider option:selected').text();
                    var resourcename = $('#pnlScheduleCalendar #Resource option:selected').text();
                    var facilityname = $('#pnlScheduleCalendar #Facility option:selected').text();
                    //Then if no tbody just select your table
                    var table = tbody.length ? tbody : $('#pnlScheduleCalendar #myTable');


                    if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

                        if (resourcename.toUpperCase() == '- SELECT -') {

                            resourcename = "";

                            $('#proresheader').text('Resource: ' + resourcename);

                        }
                        else {
                            $('#proresheader').text('Resource: ' + resourcename);
                        }
                        if (facilityname.toUpperCase() == '- SELECT -') {


                            facilityname = "";
                            $('#facilityheader').text('Facility: ' + facilityname);

                        }
                        else {
                            $('#facilityheader').text('Facility: ' + facilityname);
                        }

                    }

                    if ($('#pnlScheduleCalendar #rdprovider').is(':checked')) {
                        if (providername.toUpperCase() == '- SELECT -') {
                            providername = "";

                            $('#proresheader').text('Provider: ' + providername);
                        }
                        else {
                            $('#proresheader').text('Provider: ' + providername);
                        }
                        if (facilityname.toUpperCase() == '- SELECT -') {

                            facilityname = "";
                            $('#facilityheader').text('Facility: ' + facilityname);

                        }
                        else {
                            $('#facilityheader').text('Facility: ' + facilityname);
                        }
                    }



                    table.append('<tr align="center"><th style="text-align: center;" colspan="5">No Schedule Found.</th>  </tr>');
                }
                //Scheduling_Calendar.Initializationtooltip();


                Scheduling_Calendar.dropDownMenuClick(Scheduling_Calendar.params.PanelID);
                Scheduling_Calendar.subDropdownMenuMouseEnter();

                if (Scheduling_Calendar.Appcount.length > 0) {
                    //$("#pnlScheduleCalendar #proresheader #spnAppCount").removeClass('hidden');
                    $('#proresheader').append('<span id="spnAppCount" class="badge" style="display: inline;"></span>');
                    $("#pnlScheduleCalendar #proresheader #spnAppCount").text(Scheduling_Calendar.Appcount.length);
                }
                else
                    $("#pnlScheduleCalendar #proresheader span").remove();

                //Scheduling_Calendar.disableCheckBoxControl();

            });
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

    DayCalendarbystatus: function (ProviderId, ResourceId, FacilityId, CriteriaDate, higlight, patientTypeId, visitTypeId) {
        Scheduling_Calendar.Appcount = [];
        $("#pnlScheduleCalendar #facilityheader").css("display", "");
        $("#pnlScheduleCalendar #proresheader").css("display", "");
        $("#pnlScheduleCalendar #checkboxpanel").css("display", "none");
        $("#pnlScheduleCalendar #yeardiv").css("display", "none");
        $("#pnlScheduleCalendar #monthdiv").css("display", "none");
        $("#pnlScheduleCalendar #fromdate").css("display", "");
        $('#pnlScheduleCalendar #agenda').val('P1');
        $("#pnlScheduleCalendar #btnblock").css("display", "");
        $("#pnlScheduleCalendar #btnunblock").css("display", "");
        $('#btnday').addClass('active');
        $('#btnweek').removeClass('active');
        $('#btnmnth').removeClass('active');
        $("#pnlScheduleCalendar #MonthCal").css("display", "none");
        $("#pnlScheduleCalendar #WeekCal").css("display", "none");
        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        Scheduling_Calendar.ClearTable();
        $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date(CriteriaDate)));
        var appstatuses;
        var appstatus;
        var reminderType;
        Scheduling_Calendar.appoinmentSoltDtl = [];
        Scheduling_Calendar.timeSlotDtlId = '';
        if (patientTypeId == undefined || patientTypeId == null) {
            patientTypeId = $('#pnlScheduleCalendar #ddlPatientTypeSc').val();
        }
        if (visitTypeId == undefined || visitTypeId == null) {
            visitTypeId = $('#pnlScheduleCalendar #ddlVisitTypeSc').val();
        }
        MDVisionService.lookups("GetAppointmentStatus").done(function (result) {
            result = JSON.parse(result["GetAppointmentStatus"]);
            appstatus = result;
            var statusslots = Scheduling_Calendar.FilterCriteria();
            Scheduling_Calendar.SearchDaySlotSchedule(ProviderId, ResourceId, FacilityId, CriteriaDate, statusslots, patientTypeId, visitTypeId).done(function (response) {
                if (response.status != false) {
                    var Slot_Detail = JSON.parse(response.ProviderScheduleFill_JSON);
                    //Try to get tbody first with jquery children. works faster!
                    var providername = $('#pnlScheduleCalendar #Provider option:selected').text();
                    var resourcename = $('#pnlScheduleCalendar #Resource option:selected').text();
                    var facilityname = $('#pnlScheduleCalendar #Facility option:selected').text();
                    Scheduling_Calendar.eSuperbillAddPermission = response.addPermission == "" ? true : false;
                    Scheduling_Calendar.eSuperbillEditPermission = response.editPermission == "" ? true : false;
                    Scheduling_Calendar.eSuperbillViewPermission = response.viewPermission == "" ? true : false;

                    var tbody = $('#pnlScheduleCalendar #myTable').children('tbody');
                    //Then if no tbody just select your table
                    var table = tbody.length ? tbody : $('#pnlScheduleCalendar #myTable');
                    if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

                        if (resourcename.toUpperCase() == '- SELECT -') {

                            resourcename = "";

                            $('#proresheader').text('Resource: ' + resourcename);

                        }
                        else {
                            $('#proresheader').text('Resource: ' + resourcename);
                        }
                        if (facilityname.toUpperCase() == '- SELECT -') {


                            facilityname = "";
                            $('#facilityheader').text('Facility: ' + facilityname);

                        }
                        else {
                            $('#facilityheader').text('Facility: ' + facilityname);
                        }

                    }

                    if ($('#pnlScheduleCalendar #rdprovider').is(':checked')) {
                        if (providername.toUpperCase() == '- SELECT -') {
                            providername = "";

                            $('#proresheader').text('Provider: ' + providername);
                        }
                        else {
                            $('#proresheader').text('Provider: ' + providername);
                        }
                        if (facilityname.toUpperCase() == '- SELECT -') {

                            facilityname = "";
                            $('#facilityheader').text('Facility: ' + facilityname);

                        }
                        else {
                            $('#facilityheader').text('Facility: ' + facilityname);
                        }
                    }
                    var appids = [];
                    var slotappcount;
                    for (var i = 0; i < Slot_Detail.length; i++) {
                        var appointment;
                        var margin = Slot_Detail[i].MaxCountApp;

                        var provid = Slot_Detail[i].ProviderId;
                        if (provid == '' || provid == 'undefined')
                            provid = null;

                        var refprovid = Slot_Detail[i].RefProviderId;
                        if (refprovid == '' || refprovid == 'undefined')
                            refprovid = null;

                        var provname = Slot_Detail[i].ProviderName;
                        if (provname == '' || provname == 'undefined')
                            provname = "";

                        var resid = Slot_Detail[i].ResourceId;
                        if (resid == '' || resid == 'undefined')
                            resid = null;
                        var resname = Slot_Detail[i].ResourceName;
                        if (resname == '' || resname == 'undefined')
                            resname = "";
                        var facid = Slot_Detail[i].FacilityId;
                        if (facid == '' || facid == 'undefined')
                            facid = null;
                        var facname = Slot_Detail[i].FacilityName;
                        if (facname == '' || facname == 'undefined')
                            facname = "";

                        var facphoneno = Slot_Detail[i].FacilityPhoneNo;
                        if (!facphoneno)
                            facphoneno = "";

                        facphoneno = facphoneno.replace(/[_\W]+/g, "");

                        var resonid = Slot_Detail[i].ScheduleReasonId;
                        if (resonid == '' || resonid == 'undefined')
                            resonid = null;

                        var color = "";

                        if (Slot_Detail[i].FacilityColor == "" || Slot_Detail[i].FacilityColor == "undefined" || Slot_Detail[i].FacilityColor == "null" || Slot_Detail[i].FacilityColor == null) {
                            color = '';
                        } else {
                            color = 'bgcolor = \'' + Slot_Detail[i].FacilityColor + '\'';
                        }

                        var appointments = '<div class="pull-left">&nbsp;</div>';
                        if (Slot_Detail[i].AppDtl != "") {
                            var app;
                            var test = 0;

                            var data = Slot_Detail[i].AppDtl;
                            var arr = data.split('|');

                            for (var j = 0; j < arr.length; j++) {
                                slotappcount = (arr.length) - 1;
                                var split = arr[j].split(',')
                                if (split.length == 22 || split.length == 24) {

                                    var appid = split[0];
                                    //For appointment count
                                    //if (!Scheduling_Calendar.Appcount.includes(appid))
                                    //    Scheduling_Calendar.Appcount.push(appid);

                                    var hasMatch = Scheduling_Calendar.Appcount.some(function (_appointmentid) {
                                        return _appointmentid === appid;
                                    });
                                    if (!hasMatch) {
                                        Scheduling_Calendar.Appcount.push(appid);
                                    }

                                    var removeItem = '0';
                                    Scheduling_Calendar.Appcount = jQuery.grep(Scheduling_Calendar.Appcount, function (value) {
                                        return value != removeItem;
                                    });
                                    var patientid = split[1];
                                    var patientname = split[2].replace(/#@#/g, ',');
                                    patientname = patientname.replace('-', ', ');
                                    var appointmentstatus = split[3];
                                    var appcolor = split[4];
                                    var appschreason = split[5];
                                    appschreason = appschreason.replace(/#@#/g, ',');
                                    var appcopay = split[6];
                                    var appreason = split[3];
                                    var appcount = split[7];
                                    var appcomments = split[8];
                                    var patientvisitid = split[9];
                                    var patvisitstatusid = split[10];
                                    var patvisitname = split[11];
                                    var patEligibility = split[12];

                                    //For last Status
                                    var noteid = split[13];
                                    var lastStatusId = split[14];
                                    var lastStatusName = split[15];
                                    var patientType = split[17];
                                    var visitType = split[18];
                                    var isNonBillable = split[19];
                                    var eligibilityIcon;


                                    var noteStatus = split[16];
                                    var noteTitle = "";
                                    var isNoteCreated = false;
                                    var billInfoId = split[20];
                                    var billInfoStatus = split[21];

                                    if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {
                                        if (split.length == 24) {
                                            provid = split[22];
                                            provname = split[23].replace('-', ', ');
                                        }
                                    }

                                    if (noteid == "0") {
                                        noteTitle = "Create Note";
                                        isNoteCreated = false;
                                    } else {
                                        noteTitle = "Edit Note";
                                        isNoteCreated = true;
                                    }
                                    var isViewNote = false;
                                    if (patvisitname.toUpperCase() == "CHECK OUT") {


                                        if (noteStatus == "Signed") {
                                            noteTitle = "View Note";
                                            isNoteCreated = true;
                                            isViewNote = true;
                                        }
                                    }



                                    //Edit By Mohsin Nasir Bug # 2931
                                    //Add New Legend(blue circle) for Eligibility.

                                    //if (arr.length > 2 && appcount != undefined) {

                                    //    Scheduling_Calendar.timeSlotDtlId = '{"TimeSlotDtlId"' + ': ' + '"checkbox' + Slot_Detail[i].TimeSlotDtlId + '",' + '"SlotCount"' + ':' + appcount + ', "AppointmentId" :' + appid + ', "PatientId" :' + patientid + '}';
                                    //    var jsonRes = JSON.parse(Scheduling_Calendar.timeSlotDtlId);
                                    //    var resultArray = $.grep(Scheduling_Calendar.appoinmentSoltDtl, function (e) {
                                    //        return e.AppointmentId == appid && e.PatientId == patientid;

                                    //    });
                                    //    if (resultArray.length == 0) {
                                    //        Scheduling_Calendar.appoinmentSoltDtl.push(jsonRes);
                                    //    }

                                    //}
                                    if (patEligibility == "Active") {
                                        eligibilityIcon = '<i class="fa fa-check-circle green"></i>';
                                    }
                                    else if (patEligibility == "Inactive") {
                                        eligibilityIcon = '<i class="fa fa-times-circle red"></i>';
                                    }
                                    else if (patEligibility.toLowerCase() == "waiting") {
                                        eligibilityIcon = '<i class="fa fa-circle black"></i>';
                                    }
                                    else {
                                        eligibilityIcon = '<i class="fa fa-circle blue"></i>';
                                    }
                                    //END Edit By Mohsin Nasir Bug # 2931

                                    appcopay = appcopay.split('-');
                                    var copaycolor;

                                    var appcopayamt = parseFloat(appcopay[0]);
                                    //azam aftab dated 2/12/2016 PMS-3940
                                    if (appcopay.length == 3) {
                                        var appcopayamtpaid = -Math.abs(parseFloat(appcopay[2]));
                                    } else {
                                        var appcopayamtpaid = parseFloat(appcopay[1]);
                                    }
                                    //end dated 2/12/2016 PMS-3940

                                    if (appcopayamtpaid == 0) {
                                        copaycolor = 'Green'; //copay paid
                                    }
                                    if (appcopayamtpaid > 0) {
                                        copaycolor = 'Black';//copay blance
                                    }
                                    if (appcopayamtpaid == appcopayamt && appcopayamtpaid > 0) {
                                        appcopay = appcopayamt;
                                        copaycolor = 'Red'; // amount copay
                                    }

                                    appids.push(appid);

                                    for (var w = 0; w < appstatus.length; w++) {
                                        if (appstatus[w].Name.toUpperCase() != "- SELECT -" && appstatus[w].Name != appointmentstatus && appstatus[w].Name.toUpperCase() != "CHECK IN" && appstatus[w].Name.toUpperCase() != "CHECK OUT")
                                            appstatuses += '<li data-lastStatusId = "' + lastStatusId + '" data-lastStatusName = "' + lastStatusName + '" id=' + appstatus[w].Value + ' ><a onclick="Scheduling_Calendar.AppointmentStatusUpdate(' + appid + ',' + appstatus[w].Value + ');" href="#" >' + appstatus[w].Name + '</a></li> ';
                                    }
                                    appstatuses = appstatuses.replace('undefined', '');
                                    var showapp = 0;

                                    for (z = 0; z < appids.length; z++) {
                                        if (appids[z] == appid && appid != 0) {
                                            showapp = showapp + 1;
                                        }

                                    }
                                    /* Start Code for Reminder Call */
                                    var sms = "SMS";
                                    var voice = "VOICE";
                                    var email = "EMAIL";
                                    var Patient_email = "";
                                    var Appointmentdatetime = "";

                                    try {
                                        Patient_email = Slot_Detail[i].PatientDetail.split(',')[10];
                                        Appointmentdatetime = $('#pnlScheduleCalendar #daydate span').text() + " " + Slot_Detail[i].FromTimeSlots;
                                    } catch (ex) {
                                        console.log(ex);
                                    }

                                    reminderType = '<li id=' + appid + ' ><a onclick="Scheduling_Calendar.SendQuickReminder(' + appid + ',\'' + sms + '\',' + patientid + ',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + facphoneno + '\');" href="#" >SMS</a></li> <li id=' + appid + ' ><a onclick="Scheduling_Calendar.SendQuickReminder(' + appid + ',\'' + voice + '\',' + patientid + ',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + facphoneno + '\');" href="#" >Voice</a></li> <li id=' + appid + ' ><a onclick="Scheduling_Calendar.SendQuickReminder(' + appid + ',\'' + email + '\',' + patientid + ',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + facphoneno + '\');" href="#" >Email</a></li> ';
                                    var appointmentReminder = '';
                                    if (appointmentstatus.toUpperCase() == 'PENDING' || appointmentstatus.toUpperCase() == 'SCHEDULED') {
                                        appointmentReminder = '<li class="dropdown-submenu"><a tabindex="-1" href="#" onclick="">Appointment Reminder</a><ul> ' + reminderType + '   </ul> </li>';
                                    }
                                    /* End Code for Reminder Call*/
                                    // provider appointment
                                    if ($.trim(appcount) > '1' && showapp == 1) {
                                        // alert(appcount * Scheduling_Calendar.slotHeight)

                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL' && appointmentstatus.toUpperCase() != "NO SHOW")
                                            appointments = appointments + '<div class="appdrag apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a  href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>        <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                            appointments = appointments + '<div class="appdrag apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>  <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            if (isNonBillable == "1") {
                                                eSuperbillLink = '';
                                            }
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',null,' + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>' + eSuperbillLink + '</ul>  </div></div> ';
                                        }
                                        //------------------ Start 01/12/2015 Muhammad Irfan For EMR-9
                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED") {


                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            if (isNonBillable == "1") {
                                                eSuperbillLink = '';
                                            }
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> ' + appointmentReminder + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>' + eSuperbillLink + '     </ul>  </div></div> ';

                                        }
                                        //------------------ End 01/12/2015 Muhammad Irfan For EMR-9

                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL') {
                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLinkForCheckOut(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            //if (isNonBillable == "1") {
                                            //    eSuperbillLink = '';
                                            //}

                                            var etemp = eSuperbillLink;
                                            eSuperbillLink = "";

                                            if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> ' + eSuperbillLink + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + '  <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li></ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + ' <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li>  </ul>  </div></div> ';
                                            }
                                        }
                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>    </ul>  </div></div> ';
                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK OUT' && patvisitname.toUpperCase() != 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL')
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                    }

                                    if ($.trim(appcount) == '1' && showapp == 1) {

                                        if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL' && appointmentstatus.toUpperCase() != "NO SHOW")
                                            appointments = appointments + '<div class="appdrag apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span> </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li id="' + appid + '"><a  href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>              <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                        if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                            appointments = appointments + '<div class="appdrag apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span> </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">    <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a  href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li>         <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                        if (patientvisitid != "" && (patvisitname.toUpperCase() == 'CHECK IN') && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() != "SIGNED") {

                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            if (isNonBillable == "1") {
                                                eSuperbillLink = '';
                                            }
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',null,' + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> ' + appointmentReminder + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> ' + eSuperbillLink + '   </ul>  </div></div> ';
                                        }
                                        //------------------ Start 01/12/2015 Muhammad Irfan For EMR-9
                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL' && noteStatus.toUpperCase() == "SIGNED") {

                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            if (isNonBillable == "1") {
                                                eSuperbillLink = '';
                                            }
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo"  onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>      <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li> ' + appointmentReminder + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>' + eSuperbillLink + '   </ul>  </div></div> ';
                                        }
                                        //------------------ End 01/12/2015 Muhammad Irfan For EMR-9
                                        if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL') {


                                            var eSuperbillLink = Scheduling_Calendar.getESuperBillLinkForCheckOut(patientid, patientvisitid, noteid, "day", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                            //if (isNonBillable == "1") {
                                            //    eSuperbillLink = '';
                                            //}

                                            var etemp = eSuperbillLink;
                                            eSuperbillLink = "";

                                            if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> ' + eSuperbillLink + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + '  <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li></ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                            }
                                            else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + ' <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li>  </ul>  </div></div> ';
                                            }


                                            //if ($(eSuperbillLink).find('a').text() == "View eSuperbill") {
                                            //    appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1);">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li>  ' + eSuperbillLink + ' <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>  </ul>  </div></div> ';
                                            //} else {
                                            //    appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1);">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\');">' + noteTitle + '</a></li>' + eSuperbillLink + '   </ul>  </div></div> ';
                                            //}
                                        }
                                        if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul> ' + appstatuses + '   </ul>  </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>    </ul>  </div></div> ';
                                        if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK OUT' && patvisitname.toUpperCase() != 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL')
                                            appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> ' + appointmentReminder + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                    }

                                    if (showapp != 1) {

                                    }
                                    appointments = appointments.replace("undefined", "");
                                    appstatuses = '';
                                    test++;
                                }
                            }

                        }
                        if (Slot_Detail[i].BlockUnblock == "Blocked") {
                            table.append('<tr id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '"><td onclick="Scheduling_Calendar.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ');" width="63"><a href="#">' + Slot_Detail[i].FromTimeSlots + '</a></td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td width="20"><input type="checkbox" name="checkbox" id="checkbox' + Slot_Detail[i].TimeSlotDtlId + '"></td><td BGCOLOR="#f88379" data-blocked="true" style="color:#fff;" data-FromTime="' + Slot_Detail[i].FromTimeSlots + '" data-ToTime="' + Slot_Detail[i].ToTimeSlots + '" data-slotId="Slotid' + Slot_Detail[i].TimeSlotDtlId + '" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '" >' + Slot_Detail[i].BlockReason + '</td></tr>');
                        }
                        else if (appointments == '<div class="pull-left">&nbsp;</div>' && Slot_Detail[i].IsAppointmetExist != 1) {
                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_Calendar.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ');" width="63"><a href="#">' + Slot_Detail[i].FromTimeSlots + '</a></td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td width="20"><input type="checkbox" name="checkbox"  id="checkbox' + Slot_Detail[i].TimeSlotDtlId + '"></td><td class="appdrop slot" ' + color + ' style="position:relative;" data-FromTime="' + Slot_Detail[i].FromTimeSlots + '" data-ToTime="' + Slot_Detail[i].ToTimeSlots + '" data-slotId="Slotid' + Slot_Detail[i].TimeSlotDtlId + '" onclick="Scheduling_Calendar.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + '">' + appointments + '</td></tr>');
                        }
                        else {
                            var chkBoxByid = "checkbox" + Slot_Detail[i].TimeSlotDtlId;
                            //var resultArr = $.grep(Scheduling_Calendar.appoinmentSoltDtl, function (e) {
                            //    return e.AppointmentId == appid && e.PatientId == patientid;
                            //});
                            //if (appcount != undefined && resultArr.length == 0) {
                            //    Scheduling_Calendar.timeSlotDtlId = '{"TimeSlotDtlId"' + ': ' + '"checkbox' + Slot_Detail[i].TimeSlotDtlId + '",' + '"SlotCount"' + ':' + appcount + ', "AppointmentId" :' + appid + ', "PatientId" :' + patientid + '}';
                            //    var jsonRes = JSON.parse(Scheduling_Calendar.timeSlotDtlId);
                            //    Scheduling_Calendar.appoinmentSoltDtl.push(jsonRes);
                            //}
                            table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_Calendar.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ');" width="63"><a href="#">' + Slot_Detail[i].FromTimeSlots + '</a></td><td id="' + Slot_Detail[i].TimeSlotDtlId + '"  width="25" align="center">' + Slot_Detail[i].PatientBooked + '</td><td width="20"><input type="checkbox" name="checkbox" id="checkbox' + Slot_Detail[i].TimeSlotDtlId + '"></td><td class="appdrop slot" ' + color + ' style="position:relative;"   onclick="Scheduling_Calendar.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" data-FromTime="' + Slot_Detail[i].FromTimeSlots + '" data-ToTime="' + Slot_Detail[i].ToTimeSlots + '" data-slotId="Slotid' + Slot_Detail[i].TimeSlotDtlId + '" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + '"  >' + appointments + '</td></tr>');
                        }
                        appointments = '';
                        Scheduling_Calendar.PatientDetails(Slot_Detail[i].PatientDetail, Slot_Detail[i].AppDtl);
                    }
                    Scheduling_Calendar.HiglightSlot(higlight);
                    Scheduling_Calendar.MovePatientAppointment();
                    $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });



                }
                else {

                    Scheduling_Calendar.ClearTable();
                    var tbody = $('#pnlScheduleCalendar #myTable').children('tbody');
                    var providername = $('#pnlScheduleCalendar #Provider option:selected').text();
                    var resourcename = $('#pnlScheduleCalendar #Resource option:selected').text();
                    var facilityname = $('#pnlScheduleCalendar #Facility option:selected').text();
                    //Then if no tbody just select your table
                    var table = tbody.length ? tbody : $('#pnlScheduleCalendar #myTable');
                    if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

                        if (resourcename.toUpperCase() == '- SELECT -') {

                            resourcename = "";

                            $('#proresheader').text('Resource: ' + resourcename);

                        }
                        else {
                            $('#proresheader').text('Resource: ' + resourcename);
                        }
                        if (facilityname.toUpperCase() == '- SELECT -') {


                            facilityname = "";
                            $('#facilityheader').text('Facility: ' + facilityname);

                        }
                        else {
                            $('#facilityheader').text('Facility: ' + facilityname);
                        }

                    }

                    if ($('#pnlScheduleCalendar #rdprovider').is(':checked')) {
                        if (providername.toUpperCase() == '- SELECT -') {
                            providername = "";

                            $('#proresheader').text('Provider: ' + providername);
                        }
                        else {
                            $('#proresheader').text('Provider: ' + providername);
                        }
                        if (facilityname.toUpperCase() == '- SELECT -') {

                            facilityname = "";
                            $('#facilityheader').text('Facility: ' + facilityname);

                        }
                        else {
                            $('#facilityheader').text('Facility: ' + facilityname);
                        }
                    }
                    table.append('<tr align="center"><td style="text-align: center;" colspan="5">No Schedule Found.</td>  </tr>');
                }
                //Scheduling_Calendar.Initializationtooltip();
                Scheduling_Calendar.dropDownMenuClick(Scheduling_Calendar.params.PanelID);
                Scheduling_Calendar.subDropdownMenuMouseEnter();
                //Scheduling_Calendar.disableCheckBoxControl();
                if (Scheduling_Calendar.Appcount.length > 0) {
                    $('#proresheader').append('<span id="spnAppCount" class="badge" style="display: inline;"></span>');
                    $("#pnlScheduleCalendar #proresheader #spnAppCount").text(Scheduling_Calendar.Appcount.length);
                }
            });
        });

    },

    HiglightSlot: function (higlight) {
        var table = $("#pnlScheduleCalendar #myTable tbody");
        var i = 0;
        table.find('tr').each(function (i) {
            var $tds = $(this).find('td'),
                productId = $tds.eq(0).text();
            i++;
            if (productId == higlight) {

                $("#pnlScheduleCalendar #myTable tr:nth-child(" + i + ") td:nth-child(1)").css("background-color", "#FFFEC5");
            }

        });
    },
    WeekClick: function () {
        $('#pnlScheduleCalendar #txtgoto').val('');
        $("#pnlScheduleCalendar #btngoto").addClass('disableAll');
        $('#btnday').removeClass('active');
        $('#btnweek').addClass('active');
        $('#btnmnth').removeClass('active');
        $("#pnlScheduleCalendar #checkboxpanel").css("display", "");
        $("#pnlScheduleCalendar #yeardiv").css("display", "none");
        $("#pnlScheduleCalendar #monthdiv").css("display", "none");
        $("#pnlScheduleCalendar #fromdate").css("display", "");
        $('#pnlScheduleCalendar #agenda').val('P2');
        $("#pnlScheduleCalendar #btnblock").css("display", "none");
        $("#pnlScheduleCalendar #btnunblock").css("display", "none");
        $("#pnlScheduleCalendar #facilityheader").css("display", "none");
        $("#pnlScheduleCalendar #proresheader").css("display", "none");
        $("#pnlScheduleCalendar #MonthCal").css("display", "none");
        $("#pnlScheduleCalendar #WeekCal").css("display", "");
        $('#pnlScheduleCalendar #divDayName').html("");
        $('#pnlScheduleCalendar #divDayName').css('display', 'none');
        $('#pnlScheduleCalendar #btnProviderPrint').attr("disabled", "disabled");
        Scheduling_Calendar.ClearTable();

        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        //expression for week range
        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        //Month Regular Expression
        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
        var criteria = $('#pnlScheduleCalendar #daydate').text().trim();
        if (criteria.match(dayrgx) && criteria.length < 15) {
            var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
            if (a == 'Invalid Date') {
                a = new Date();
            }
            Scheduling_Calendar.ClearTable();

            var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
            var week = Scheduling_Calendar.GetWeek(date1);
            $('#pnlScheduleCalendar #daydate span').html(week);
            var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
        }

        else if (criteria.match(weekrg)) {
            var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
            if (a == 'Invalid Date') {
                a = new Date();
            }
            Scheduling_Calendar.ClearTable();

            var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
            var week = Scheduling_Calendar.GetWeek(date1);
            $('#pnlScheduleCalendar #daydate span').html(week);
            var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);


        }
        else if (criteria.match(monthreg)) {
            var a = $('#pnlScheduleCalendar #calmonth').val();
            if (a == 'Invalid Date') {
                a = new Date();
            }
            var b = $.datepicker.formatDate('mm/dd/yy', new Date(Scheduling_Calendar.FormatDate(a)));
            Scheduling_Calendar.ClearTable();
            var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(b));
            var week = Scheduling_Calendar.GetWeek(date1);
            $('#pnlScheduleCalendar #daydate span').html(week);
            var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(Scheduling_Calendar.FormatDate(a)));
            //Bug # PMS-2965 By Mohsin Nasir

            //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #fromdate').datepicker('getDate'));

            //END Bug # PMS-2965 By Mohsin Nasir

            //if (utility.UserBrowser() == "Firefox" || utility.UserBrowser().toUpperCase() == "IE") {
            //    $('#pnlScheduleCalendar #fromdate').datepicker("update", monthDatetxt);
            //}
            //else {
            //    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", monthDatetxt);
            //}

        }
    },

    WeekCalendar: function (ProviderId, ResourceId, FacilityId, checkeddays, DatesString, StatusId, PatientTypeId, VisitTypeId) {
        Scheduling_Calendar.ClearTable();
        $("#weektable").empty();
        var appstatuses;
        var appstatus;
        if (PatientTypeId == undefined || PatientTypeId == null) {
            PatientTypeId = $('#pnlScheduleCalendar #ddlPatientTypeSc').val();
        }
        if (VisitTypeId == undefined || VisitTypeId == null) {
            VisitTypeId = $('#pnlScheduleCalendar #ddlVisitTypeSc').val();
        }
        MDVisionService.lookups("GetAppointmentStatus").done(function (result) {
            result = JSON.parse(result["GetAppointmentStatus"]);
            appstatus = result;
            Scheduling_Calendar.SelectWeeklySlotAppointment(ProviderId, ResourceId, FacilityId, checkeddays, DatesString, StatusId, PatientTypeId, VisitTypeId).done(function (response) {
                if (response != false) {

                    $('#weekcontainer').empty();
                    var tablewidth = 100 / parseInt(response.length);
                    var PatientsDtl = [];
                    var AppointmentsDtl = [];
                    for (var wc = 0; wc < response.length; wc++) {
                        var weekdetail = JSON.parse(response[wc]);
                        var Slot_Detail = JSON.parse(weekdetail.ProviderScheduleFill_JSON);
                        Scheduling_Calendar.eSuperbillAddPermission = weekdetail.addPermission == "" ? true : false;
                        Scheduling_Calendar.eSuperbillEditPermission = weekdetail.editPermission == "" ? true : false;
                        Scheduling_Calendar.eSuperbillViewPermission = weekdetail.viewPermission == "" ? true : false;
                        $("#weektable").append('<th class="multiple-heading center" width=' + tablewidth + '%>' + weekdetail.tableday + '</th>');
                        $("#weekcontainer").append('<div style="width:' + tablewidth + '%; float:left; " class="th-text-center " id="' + weekdetail.tableday + '' + FacilityId + '"></div>');
                        $('#WeekCal').css('width', $("#weekcontainer").css('width'));
                        if (Slot_Detail.length != 0) {
                            var table = $('<table class="table table-responsive table-bordered table-condensed" id="table' + weekdetail.tableday + '' + FacilityId + '"><tbody></tbody></table>');
                            $('#table' + weekdetail.tableday + '' + FacilityId + '').children('tbody');
                            var appids = [];
                            var slotappcount;
                            for (var i = 0; i < Slot_Detail.length; i++) {
                                var appointment;
                                var margin = Slot_Detail[i].MaxCountApp;

                                var provid = Slot_Detail[i].ProviderId;
                                if (provid == '' || provid == 'undefined')
                                    provid = null;

                                var refprovid = Slot_Detail[i].RefProviderId;
                                if (refprovid == '' || refprovid == 'undefined')
                                    refprovid = null;

                                var provname = Slot_Detail[i].ProviderName;
                                if (provname == '' || provname == 'undefined')
                                    provname = "";

                                var resid = Slot_Detail[i].ResourceId;
                                if (resid == '' || resid == 'undefined')
                                    resid = null;
                                var resname = Slot_Detail[i].ResourceName;
                                if (resname == '' || resname == 'undefined')
                                    resname = "";
                                var facid = Slot_Detail[i].FacilityId;
                                if (facid == '' || facid == 'undefined')
                                    facid = null;
                                var facname = Slot_Detail[i].FacilityName;
                                if (facname == '' || facname == 'undefined')
                                    facname = "";

                                var facphoneno = Slot_Detail[i].FacilityPhoneNo;
                                if (!facphoneno)
                                    facphoneno = "";

                                facphoneno = facphoneno.replace(/[_\W]+/g, "");

                                var resonid = Slot_Detail[i].ScheduleReasonId;
                                if (resonid == '' || resonid == 'undefined')
                                    resonid = null;

                                var color = "";

                                if (Slot_Detail[i].FacilityColor == "" || Slot_Detail[i].FacilityColor == "undefined" || Slot_Detail[i].FacilityColor == "null" || Slot_Detail[i].FacilityColor == null) {
                                    color = '';
                                } else {
                                    color = 'bgcolor = \'' + Slot_Detail[i].FacilityColor + '\'';
                                }

                                var appointments = '<div class="pull-left">&nbsp;</div>';
                                if (Slot_Detail[i].AppDtl != "") {
                                    var app;
                                    var test = 0;

                                    var data = Slot_Detail[i].AppDtl;
                                    var arr = data.split('|');

                                    for (var j = 0; j < arr.length; j++) {
                                        slotappcount = (arr.length) - 1;
                                        var split = arr[j].split(',')
                                        if (split.length == 22 || split.length == 24) {

                                            var appid = split[0];
                                            var patientid = split[1];
                                            var patientname = split[2].replace(/#@#/g, ',');
                                            patientname = patientname.replace('-', ', ');
                                            var appointmentstatus = split[3];
                                            var appcolor = split[4];
                                            var appcopay = split[6];
                                            var appreason = split[3];
                                            var appcount = split[7];
                                            var appcomments = split[8];
                                            var appschreason = split[5];
                                            appschreason = appschreason.replace(/#@#/g, ',');
                                            var patientvisitid = split[9];
                                            var patvisitstatusid = split[10];
                                            var patvisitname = split[11];
                                            var patEligibility = split[12];

                                            //For last Status
                                            var lastStatusId = split[14];
                                            var lastStatusName = split[15];
                                            var patientType = split[17];
                                            var visitType = split[18];
                                            var isNonBillable = split[19];
                                            var billInfoId = split[20];
                                            var billInfoStatus = split[21];
                                            if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {
                                                if (split.length == 24) {
                                                    provid = split[22];
                                                    provname = split[23].replace("-", ", ");
                                                }
                                            }
                                            var eligibilityIcon;


                                            //Start//28-07-2016// Added by Abid =================
                                            var noteid = split[13];
                                            var noteStatus = split[16];
                                            var noteTitle = "";
                                            var isNoteCreated = false;
                                            if (noteid == "0") {
                                                noteTitle = "Create Note";
                                                isNoteCreated = false;
                                            } else {
                                                noteTitle = "Edit Note";
                                                isNoteCreated = true;
                                            }
                                            var isViewNote = false;
                                            if (patvisitname.toUpperCase() == "CHECK OUT") {


                                                if (noteStatus == "Signed") {
                                                    noteTitle = "View Note";
                                                    isNoteCreated = true;
                                                    isViewNote = true;
                                                }
                                            }
                                            //End//28-07-2016// Added by Abid =================



                                            //Edit By Mohsin Nasir Bug # 2931
                                            //Add New Legend(blue circle) for Eligibility.
                                            if (patEligibility == "Active") {
                                                eligibilityIcon = '<i class="fa fa-check-circle green"></i>';
                                            }
                                            else if (patEligibility == "Inactive") {
                                                eligibilityIcon = '<i class="fa fa-times-circle red"></i>';
                                            }
                                            else if (patEligibility.toLowerCase() == "waiting") {
                                                eligibilityIcon = '<i class="fa fa-circle black"></i>';
                                            }
                                            else {
                                                eligibilityIcon = '<i class="fa fa-circle blue"></i>';
                                            }
                                            //END Edit By Mohsin Nasir Bug # 2931

                                            appcopay = appcopay.split('-');
                                            var copaycolor;

                                            var appcopayamt = parseFloat(appcopay[0]);
                                            //azam aftab dated 2/12/2016 PMS-3940
                                            if (appcopay.length == 3) {
                                                var appcopayamtpaid = -Math.abs(parseFloat(appcopay[2]));
                                            } else {
                                                var appcopayamtpaid = parseFloat(appcopay[1]);
                                            }
                                            //end dated 2/12/2016 PMS-3940

                                            if (appcopayamtpaid == 0) {
                                                copaycolor = 'Green'; //copay paid
                                            }
                                            if (appcopayamtpaid > 0) {
                                                copaycolor = 'Black';//copay blance
                                            }
                                            if (appcopayamtpaid == appcopayamt && appcopayamtpaid > 0) {
                                                appcopay = appcopayamt;
                                                copaycolor = 'Red'; // amount copay
                                            }
                                            appids.push(appid);

                                            for (var w = 0; w < appstatus.length; w++) {
                                                if (appstatus[w].Name.toUpperCase() != "- SELECT -" && appstatus[w].Name != appointmentstatus && appstatus[w].Name.toUpperCase() != "CHECK IN" && appstatus[w].Name.toUpperCase() != "CHECK OUT")
                                                    appstatuses += '<li data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id=' + appstatus[w].Value + ' ><a onclick="Scheduling_Calendar.AppointmentStatusUpdate(' + appid + ',' + appstatus[w].Value + ');" href="#" >' + appstatus[w].Name + '</a></li> ';
                                            }
                                            appstatuses = appstatuses.replace('undefined', '');
                                            var showapp = 0;

                                            for (z = 0; z < appids.length; z++) {
                                                if (appids[z] == appid && appid != 0) {
                                                    showapp = showapp + 1;
                                                }

                                            }

                                            /* Start Code for Reminder Call */
                                            var sms = "SMS";
                                            var voice = "VOICE";
                                            var email = "EMAIL";
                                            var Patient_email = "";
                                            var Appointmentdatetime = "";

                                            try {
                                                Patient_email = Slot_Detail[i].PatientDetail.split(',')[10];
                                                Appointmentdatetime = Slot_Detail[i].ScheduleDate.split(" ")[0] + " " + Slot_Detail[i].FromTimeSlots;
                                            } catch (ex) {
                                                console.log(ex);
                                            }
                                            reminderType = '<li id=' + appid + ' ><a onclick="Scheduling_Calendar.SendQuickReminder(' + appid + ',\'' + sms + '\',' + patientid + ',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + facphoneno + '\');" href="#" >SMS</a></li> <li id=' + appid + ' ><a onclick="Scheduling_Calendar.SendQuickReminder(' + appid + ',\'' + voice + '\',' + patientid + ',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + facphoneno + '\');" href="#" >Voice</a></li> <li id=' + appid + ' ><a onclick="Scheduling_Calendar.SendQuickReminder(' + appid + ',\'' + email + '\',' + patientid + ',\'' + Patient_email + '\',\'' + Appointmentdatetime + '\',\'' + facphoneno + '\');" href="#" >Email</a></li> ';
                                            /* End Code for Reminder Call*/
                                            var appointmentReminder = '';
                                            if (appointmentstatus.toUpperCase() == 'PENDING' || appointmentstatus.toUpperCase() == 'SCHEDULED') {
                                                appointmentReminder = '<li class="dropdown-submenu"><a tabindex="-1" href="#" onclick="">Appointment Reminder</a><ul> ' + reminderType + '   </ul> </li>';
                                            }
                                            //week slots
                                            if ($.trim(appcount) > '1' && showapp == 1) {

                                                if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL' && appointmentstatus.toUpperCase() != "NO SHOW")
                                                    appointments = appointments + '<div class="appdrag apm-block zIndex"  style="width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + ',' + weekdetail.tableday + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a  href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>      <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul > ' + appstatuses + '   </ul>  </li> </li> ' + appointmentReminder + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                    appointments = appointments + '<div class="appdrag apm-block zIndex"  style="width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + ',' + weekdetail.tableday + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul > ' + appstatuses + '   </ul>  </li> </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';

                                                if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL') {

                                                    var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "week", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                    if (isNonBillable == "1") {
                                                        eSuperbillLink = '';
                                                    }
                                                    appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + ',' + weekdetail.tableday + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',null,' + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li>' + appointmentReminder + '<li  id="' + appid + '"><a  tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li>' + eSuperbillLink + ' </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul></div></div> ';
                                                }
                                                if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL') {
                                                    var eSuperbillLink = Scheduling_Calendar.getESuperBillLinkForCheckOut(patientid, patientvisitid, noteid, "week", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                    //if (isNonBillable == "1") {
                                                    //    eSuperbillLink = '';
                                                    //}

                                                    var etemp = eSuperbillLink;
                                                    eSuperbillLink = "";

                                                    if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> ' + eSuperbillLink + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + '  <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li></ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + ' <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li>  </ul>  </div></div> ';
                                                    }
                                                }
                                                //appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + ',' + weekdetail.tableday + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\');">Edit Appointment</a></li>  </li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                                if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                    appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * 21) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>   <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul > ' + appstatuses + '   </ul>  </li>    </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>   </ul>  </div></div> ';
                                                if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL' && patvisitname.toUpperCase() != 'CHECK IN')
                                                    appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * 21) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>  </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                            }

                                            if ($.trim(appcount) == '1' && showapp == 1) {

                                                if (appointmentstatus.toUpperCase() != 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL' && appointmentstatus.toUpperCase() != "NO SHOW")
                                                    appointments = appointments + '<div class="appdrag apm-block zIndex"  style="height:17px; width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + ',' + weekdetail.tableday + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span> </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a  href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>           <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul > ' + appstatuses + '   </ul>  </li> </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';
                                                if (appointmentstatus.toUpperCase() == 'CONFIRM' && patientvisitid == "" && appointmentstatus.toUpperCase() != 'CANCEL')
                                                    appointments = appointments + '<div class="appdrag apm-block zIndex"  style="height:17px; width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + ',' + weekdetail.tableday + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span> </div><div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',0,0);">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',0,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>  <li id="' + appid + '"><a class="disabled" href="#" onclick="Scheduling_Calendar.LoadCheckIn(' + appid + ',' + patientid + ',\'' + Slot_Detail[i].ScheduleDate + '\');">Check in</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.RescheduleAppointmentSearch(\'' + facid + '\',\'' + facname + '\',\'' + provid + '\',\'' + provname + '\',\'' + resid + '\',\'' + resname + '\',\'' + appid + '\',\'' + patientid + '\');">Reschedule</a></li>        <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul > ' + appstatuses + '   </ul>  </li> </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li> </ul>  </div></div> ';

                                                if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK IN' && appointmentstatus.toUpperCase() != 'CANCEL') {

                                                    var eSuperbillLink = Scheduling_Calendar.getESuperBillLink(patientid, patientvisitid, noteid, "week", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                    if (isNonBillable == "1") {
                                                        eSuperbillLink = '';
                                                    }
                                                    appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + ',' + weekdetail.tableday + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');" id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>    <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCheckOut(' + appid + ',' + patientid + ',' + patientvisitid + ');">Check Out</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CancelPatientCheckIn(' + patientvisitid + ',' + appid + ',this);">Cancel CheckIn</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\',null,' + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#" >Create Charge</a></li><li  id="' + appid + '"><a  tabindex="-1" href="#" onclick="Scheduling_Calendar.PrintLetter(' + patientid + ',\'' + patientvisitid + '\',\'' + appid + '\');" >Print Super Bill</a></li>' + eSuperbillLink + '</li> ' + appointmentReminder + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li></ul></div></div> ';
                                                }

                                                if (patientvisitid != "" && patvisitname.toUpperCase() == 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL') {

                                                    var eSuperbillLink = Scheduling_Calendar.getESuperBillLinkForCheckOut(patientid, patientvisitid, noteid, "week", patientType, provid, refprovid, facid, appid, billInfoId, billInfoStatus);
                                                    //if (isNonBillable == "1") {
                                                    //    eSuperbillLink = '';
                                                    //}

                                                    var etemp = eSuperbillLink;
                                                    eSuperbillLink = "";

                                                    if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "create note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + ' </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "view esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li  id="' + appid + '"><a onclick="Scheduling_Calendar.CreateVisitCharge(\'' + appid + '\',\'' + patientid + '\',\'' + patientvisitid + '\');" href="#">View Charge</a></li>' + eSuperbillLink + '  <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li></ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "create esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li>' + eSuperbillLink + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li>  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "view note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + '  <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li><li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li>  </ul>  </div></div> ';
                                                    }
                                                    else if ($(etemp).find('a').text().toLowerCase() == "edit esuperbill" && noteTitle.toLowerCase() == "edit note") {
                                                        appointments = appointments + '<div class="apm-block zIndex"  style=" width:' + 98 / parseInt(margin) + '%; margin-left:' + test * (98 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"> <div class="apm-block-content" style="padding:2px;"> ' + eligibilityIcon + ' <a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div><div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeight) - Scheduling_Calendar.buttonHeightAdj) + 'px;">   <ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu"> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadCopayment(' + appid + ',' + patientid + ',' + facid + ',' + patientvisitid + ',\'' + patvisitname + '\');">Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',1,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">Edit Appointment</a></li> ' + eSuperbillLink + ' <li  id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadClinicalNote(\'' + Slot_Detail[i].PatientDetail + '\',' + appid + ',' + patientid + ',\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + patientvisitid + '\',\'' + appschreason + '\',\'' + Slot_Detail[i].RefProviderName + '\',\'' + Slot_Detail[i].RefProviderId + '\',\'' + isNoteCreated + '\',\'' + noteid + '\' , \'' + isViewNote + '\'' + "," + provid + ',\'' + provname + '\'' + ');">' + noteTitle + '</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li>  </ul>  </div></div> ';
                                                    }
                                                }
                                                if (appointmentstatus.toUpperCase() == 'CANCEL' || appointmentstatus.toUpperCase() == "NO SHOW")
                                                    appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * 21) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + appreason + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li>    <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientAppointmentDelete(' + appid + ');">Delete Appointment</a></li>    <li class="dropdown-submenu">    <a tabindex="-1" href="#" onclick="Scheduling_Calendar.AppointmentStatusUpdate();">Change Appointment Status</a>   <ul > ' + appstatuses + '   </ul>  </li>   </li>' + appointmentReminder + ' <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>   </ul>  </div></div> ';
                                                if (patientvisitid != "" && patvisitname.toUpperCase() != 'CHECK OUT' && appointmentstatus.toUpperCase() != 'CANCEL' && patvisitname.toUpperCase() != 'CHECK IN')
                                                    appointments = appointments + '<div class="apm-block zIndex"  style="width:' + 94 / parseInt(margin) + '%; margin-left:' + test * (94 / parseInt(margin)) + '%; height:' + ((parseInt(appcount) * 21) + Scheduling_Calendar.padding) + 'px; border-color:' + appcolor + ';  "  id="' + appid + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + patientid + ',' + appcount + ',' + Slot_Detail[i].FromTimeSlots + '"><div class="apm-block-content" style="padding:2px;">' + eligibilityIcon + '<a href="#" class="helllo" onclick=" Scheduling_Calendar.PatientDemographics(' + patientid + ');"  id=' + patientid + '-' + appid + '>' + patientname + '</a>[' + patvisitname + ']</div> <div class="apm-copayment" style="font-weight: bold; color:' + copaycolor + ';"> $' + appcopayamtpaid + '&nbsp;<span class="ellipses inlineBlock size80per" data-toggle="tooltip" data-placement="right" title="' + appcomments + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason + "-" + visitType + '" style="color:gray;">' + appcomments.split(/\s+/).slice(0, 1).join(" ") + (appcomments != "" ? "-" : "") + patientType + "-" + appschreason.split(/\s+/).slice(0, 1).join(" ") + "-" + visitType + '</span></div> <div class="buttonDrop dropdown" style="height:' + ((parseInt(appcount) * Scheduling_Calendar.slotHeightWeek) - Scheduling_Calendar.buttonHeightAdj) + 'px;"><ul data-lastStatusId = ' + lastStatusId + ' data-lastStatusName = "' + lastStatusName + '" id="myid" class="dropdown-menu multi-level" role="menu" aria-labelledby="dropdownMenu">   <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.PatientDemographics(' + patientid + ');">Demographics</a></li><li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.LoadUnallocatedCopayment(' + appid + ',' + patientid + ',' + facid + ',' + provid + ',' + patientvisitid + ');">Unallocated Copayment</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.OpenPatientEligibility(' + appid + ');">Patient Eligibility</a></li> <li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.AppointmentEdit(' + appid + ',\'' + patientvisitid + '\',2,\'' + Slot_Detail[i].SlotMinutes + '\',\'' + noteid + '\');">View Appointment</a></li> </li> ' + appointmentReminder + '<li id="' + appid + '"><a href="#" onclick="Scheduling_Calendar.CreateLetter(' + patientid + ');">Create Letter</a></li>  </ul>  </div></div> ';
                                            }
                                            if (showapp != 1) {

                                            }

                                            appointments = appointments.replace("undefined", "");
                                            appstatuses = '';
                                            test++;
                                        }
                                    }

                                }
                                if (Slot_Detail[i].BlockUnblock == "Blocked") {
                                    table.append('<tr id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '"><td onclick="Scheduling_Calendar.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ');" width="63"><a href="#">' + Slot_Detail[i].FromTimeSlots + '</a></td><td BGCOLOR="#f88379" data-blocked="true" style="color:#fff;position:relative;" data-FromTime="' + Slot_Detail[i].FromTimeSlots + '" data-ToTime="' + Slot_Detail[i].ToTimeSlots + '" data-slotId="Slotid' + Slot_Detail[i].TimeSlotDtlId + '" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + '" ><div class="apm-block-status">Blocked:' + Slot_Detail[i].BlockReason + '</div></td></tr>');
                                }
                                else if (appointments == '<div class="pull-left">&nbsp;</div>') {
                                    table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_Calendar.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ');" width="63"><a href="#">' + Slot_Detail[i].FromTimeSlots + '</a></td><td class="appdrop slot" ' + color + ' style="position:relative;" onclick="Scheduling_Calendar.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',' + Slot_Detail[i].FromTimeSlots + ',' + weekdetail.tableday + '" data-FromTime="' + Slot_Detail[i].FromTimeSlots + '" data-ToTime="' + Slot_Detail[i].ToTimeSlots + '" data-slotId="Slotid' + Slot_Detail[i].TimeSlotDtlId + '">' + appointments + '</td></tr>');
                                }
                                else {

                                    table.append('<tr id="' + Slot_Detail[i].Patients + '"><td onclick="Scheduling_Calendar.EditSlot(' + Slot_Detail[i].TimeSlotDtlId + ');" width="63"><a href="#"> ' + Slot_Detail[i].FromTimeSlots + '</a></td><td class="appdrop slot" ' + color + ' style="position:relative;"   onclick="Scheduling_Calendar.AppointmentAdd(' + Slot_Detail[i].TimeSlotId + ',' + Slot_Detail[i].TimeSlotDtlId + ',' + provid + ', \'' + provname + '\',' + resid + ',\'' + resname + '\',' + facid + ',\'' + facname + '\',' + resonid + ',event,\'' + Slot_Detail[i].FromTimeSlots + '\',\'' + Slot_Detail[i].ToTimeSlots + '\',\'' + Slot_Detail[i].IsSpecialist + '\',\'' + Slot_Detail[i].SlotMinutes + '\', \'' + Slot_Detail[i].BlockReason + '\',\'' + Slot_Detail[i].ScheduleDate + '\');" id="Slotid' + Slot_Detail[i].TimeSlotId + ',TimeSlotDtlId' + Slot_Detail[i].TimeSlotDtlId + ',' + Slot_Detail[i].Patients + ',' + Slot_Detail[i].FromTimeSlots + ',' + weekdetail.tableday + '" data-FromTime="' + Slot_Detail[i].FromTimeSlots + '" data-ToTime="' + Slot_Detail[i].ToTimeSlots + '" data-slotId="Slotid' + Slot_Detail[i].TimeSlotDtlId + '"  >' + appointments + '</td></tr>');
                                }
                                appointments = '';
                                PatientsDtl.push(Slot_Detail[i].PatientDetail);
                                AppointmentsDtl.push(Slot_Detail[i].AppDtl);
                            }
                            //Scheduling_Calendar.MovePatientAppointment();
                            $('#' + weekdetail.tableday + '' + FacilityId + '').append(table);
                            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                            if (PatientsDtl.length == AppointmentsDtl.length) {
                                for (var ini = 0; ini < PatientsDtl.length; ini++) {
                                    Scheduling_Calendar.PatientDetails(PatientsDtl[ini], AppointmentsDtl[ini]);
                                }
                                PatientsDtl = [];
                                AppointmentsDtl = [];
                            }
                        }
                        else {

                            var table = $('<table class="table table-responsive table-bordered table-condensed" id="table' + weekdetail.tableday + '' + FacilityId + '"><tbody></tbody></table>');
                            $('#table' + weekdetail.tableday + '' + FacilityId + '').children('tbody');
                            table.append('<tr align="center"><td style="text-align: center;" >No Schedule Found.</td>  </tr>');
                            $('#' + weekdetail.tableday + '' + FacilityId + '').append(table);
                        }

                    }

                }
                Scheduling_Calendar.MovePatientAppointment();
                //Scheduling_Calendar.Initializationtooltip();
                Scheduling_Calendar.dropDownMenuClick(Scheduling_Calendar.params.PanelID);
                Scheduling_Calendar.subDropdownMenuMouseEnter();
                Scheduling_Calendar.EnableWeekCheckBox();
            });
        });

    },

    MonthCalendar: function (ProviderId, FacilityId, ResourceId, CriteriaMonth, StatusId, PatientTypeId, VisitTypeId) {

        // Start left search panel setup
        $("#pnlScheduleCalendar #checkboxpanel").css("display", "none");
        $("#pnlScheduleCalendar #fromdate").css("display", "none");
        $("#pnlScheduleCalendar #yeardiv").css("display", "");
        $("#pnlScheduleCalendar #monthdiv").css("display", "");
        $('#pnlScheduleCalendar #agenda').val('P3');
        $("#pnlScheduleCalendar #btnblock").css("display", "none");
        $("#pnlScheduleCalendar #btnunblock").css("display", "none");
        // End left search panel setup
        var monthDays = 0;
        var month = $.datepicker.formatDate('MM/yy', new Date((Scheduling_Calendar.FormatDate($('#pnlScheduleCalendar #daydate span').text().trim()))));
        // Startcurrent month set
        $('#pnlScheduleCalendar #daydate span').html(month);
        // End current month set

        // Start of Monthly view calendar
        Scheduling_Calendar.ClearTable();
        if (PatientTypeId == undefined || PatientTypeId == null) {
            PatientTypeId = $('#pnlScheduleCalendar #ddlPatientTypeSc').val();
        }
        if (VisitTypeId == undefined || VisitTypeId == null) {
            VisitTypeId = $('#pnlScheduleCalendar #ddlVisitTypeSc').val();
        }


        Scheduling_Calendar.SelectMonthlyAppointment(ProviderId, FacilityId, ResourceId, CriteriaMonth, StatusId, PatientTypeId, VisitTypeId).done(function (response) {

            $('#pnlScheduleCalendar #btnday').removeClass("active");
            $('#pnlScheduleCalendar #btnweek').removeClass("active");
            $('#pnlScheduleCalendar #btnmnth').addClass("active");
            if (response.status != false) {

                var Month_Detail = JSON.parse(response.MonthlyAppointmentFill_JSON);

                var date = new Date('' + CriteriaMonth.substr(0, 2) + '/01/' + CriteriaMonth.substr(3, 4) + '');


                var day = date.getDate();

                var month = date.getMonth();
                //Bug # PMS-2970 By Mohsin Nasir
                //if (month == 0)
                // month = 1;
                //END Bug # PMS-2970 By Mohsin Nasir
                var year = date.getFullYear();

                //if (year <= 200) {
                //    year += 1900;
                //}
                months = new Array('January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');
                days_in_month = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
                if (utility.LeapYear(year)) {
                    days_in_month[1] = 29;
                }
                total = days_in_month[month];
                var date_today = day + ' ' + months[month] + ' ' + year;
                var month_today = months[month] + ' ' + year;
                beg_j = date;
                beg_j.setDate(1);
                if (beg_j.getDate() == 2) {
                    beg_j = setDate(0);
                }
                beg_j = beg_j.getDay();

                //Try to get tbody first with jquery children. works faster!
                var tbody = $('#pnlScheduleCalendar #monthTable').children('tbody');

                //Then if no tbody just select your table
                var table = tbody.length ? tbody : $('#pnlScheduleCalendar #monthTable');
                var monthhtml = '<tr></tr>';


                monthhtml += ('<tr ><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th></tr><tr>');
                week = 0;
                // previous month days
                for (i = 1; i <= beg_j; i++) {

                    if (days_in_month[month - 1] == undefined) {
                        monthDays = 31;
                    } else {
                        monthDays = days_in_month[month - 1];
                    }
                    //original line
                    //monthhtml += ('<td class="deactive1"><div class="td-hight100"><label class="block">' + (days_in_month[month - 1] - beg_j + i) + '</label></div></td>');
                    monthhtml += ('<td class="deactive1"><div class="td-hight100"><label class="block">' + (monthDays - beg_j + i) + '</label></div></td>');
                    week++;
                }
                for (i = 1; i <= total; i++) {
                    if (week == 0) {
                        monthhtml += ('<tr>');
                    }

                    // current day fill

                    if (day == i) {
                        var execute = false;
                        for (var j = 0; j < Month_Detail.length; j++) {
                            if (Month_Detail[j].AppCount != "") {

                                var currentDay = Month_Detail[j].Days;
                                var data = Month_Detail[j].AppCount;
                                var arr = data.split('|');

                                var appointments = '<div class="clearfix"></div><hr class="m-xs" >';
                                var appointments1;
                                if (currentDay == i) {
                                    execute = true;

                                    for (var k = 0; k < arr.length; k++) {
                                        var split = arr[k].split(', ')

                                        if (split.length == 2) {
                                            var numberCount = split[0];
                                            var bgcolor = split[1];
                                            if (numberCount != "" && bgcolor != '#f6f6f6') {
                                                var date = '\'' + CriteriaMonth.substr(3, 4) + '-' + CriteriaMonth.substr(0, 2) + '-' + i + '\'';
                                                var providerid = '\'' + $('#pnlScheduleCalendar #Provider').val() + '\'';
                                                var facilityid = '\'' + $('#pnlScheduleCalendar #Facility').val() + '\'';
                                                var resourceid = '\'' + $('#pnlScheduleCalendar #Resource').val() + '\'';
                                                var color = '\'' + bgcolor + '\'';
                                                appointments += '<div  onclick="Scheduling_Calendar.AppointmentByStatus(' + providerid + ',' + facilityid + ', ' + date + ', ' + color + ', ' + resourceid + ');" class="cell-box" style="cursor:pointer;border-color:' + bgcolor + '">' + numberCount + '</div>';
                                            }
                                            if (numberCount != "" && bgcolor == '#f6f6f6') {
                                                var date = '\'' + CriteriaMonth.substr(3, 4) + '-' + CriteriaMonth.substr(0, 2) + '-' + i + '\'';
                                                var providerid = '\'' + $('#pnlScheduleCalendar #Provider').val() + '\'';
                                                var facilityid = '\'' + $('#pnlScheduleCalendar #Facility').val() + '\'';
                                                var resourceid = '\'' + $('#pnlScheduleCalendar #Resource').val() + '\'';
                                                var color = '\'' + bgcolor + '\'';
                                                appointments1 = '<div  onclick="Scheduling_Calendar.AppointmentByStatus(' + providerid + ',' + facilityid + ', ' + date + ', ' + color + ', ' + resourceid + ');" class="cell-box1" style="cursor:pointer;border-color:' + bgcolor + '">' + numberCount + '</div>';
                                            }
                                        }
                                    }
                                    monthhtml += ('<td><div class="td-hight100"><label class="block pull-left">' + i + '</label><div style="float:right;">' + appointments1 + '</div><label class="text-bold pull-right" style="margin-top:8px;">Total:</label>' + appointments + '</div></td>');

                                }


                            }

                        }
                        if (execute == false) {
                            monthhtml += ('<td><div class="td-hight100"><label class="block">' + i + '</label></div></td>');
                        }
                    }
                        //normal days fill
                    else {
                        var execute = false;
                        for (var j = 0; j < Month_Detail.length; j++) {
                            if (Month_Detail[j].AppCount != "") {

                                var currentDay = Month_Detail[j].Days;
                                var data = Month_Detail[j].AppCount;
                                var arr = data.split('|');

                                var appointments = '<div class="clearfix"></div><hr class="m-xs" >';
                                var appointments1;
                                if (currentDay == i) {
                                    execute = true;

                                    for (var k = 0; k < arr.length; k++) {
                                        var split = arr[k].split(', ')

                                        if (split.length == 2) {
                                            var numberCount = split[0];
                                            var bgcolor = split[1];
                                            if (numberCount != "" && bgcolor != '#f6f6f6') {
                                                var date = '\'' + CriteriaMonth.substr(3, 4) + '-' + CriteriaMonth.substr(0, 2) + '-' + i + '\'';
                                                var providerid = '\'' + $('#pnlScheduleCalendar #Provider').val() + '\'';
                                                var facilityid = '\'' + $('#pnlScheduleCalendar #Facility').val() + '\'';
                                                var resourceid = '\'' + $('#pnlScheduleCalendar #Resource').val() + '\'';
                                                var color = '\'' + bgcolor + '\'';
                                                appointments += '<div  onclick="Scheduling_Calendar.AppointmentByStatus(' + providerid + ',' + facilityid + ', ' + date + ', ' + color + ', ' + resourceid + ');" class="cell-box" style="cursor:pointer;border-color:' + bgcolor + '">' + numberCount + '</div>';
                                            }
                                            if (numberCount != "" && bgcolor == '#f6f6f6') {
                                                var date = '\'' + CriteriaMonth.substr(3, 4) + '-' + CriteriaMonth.substr(0, 2) + '-' + i + '\'';
                                                var providerid = '\'' + $('#pnlScheduleCalendar #Provider').val() + '\'';
                                                var facilityid = '\'' + $('#pnlScheduleCalendar #Facility').val() + '\'';
                                                var resourceid = '\'' + $('#pnlScheduleCalendar #Resource').val() + '\'';
                                                var color = '\'' + bgcolor + '\'';
                                                appointments1 = '<div  onclick="Scheduling_Calendar.AppointmentByStatus(' + providerid + ',' + facilityid + ', ' + date + ', ' + color + ', ' + resourceid + ');" class="cell-box1" style="cursor:pointer;border-color:' + bgcolor + '">' + numberCount + '</div>';
                                            }
                                        }
                                    }
                                    monthhtml += ('<td><div class="td-hight100"><label class="block pull-left">' + i + '</label><div style="float:right;">' + appointments1 + '</div><label class="text-bold pull-right" style="margin-top:8px;">Total:</label>' + appointments + '</div></td>');

                                }


                            }

                        }
                        if (execute == false) {
                            monthhtml += ('<td><div class="td-hight100"><label class="block">' + i + '</label></div></td>');
                        }
                    }

                    week++;
                    if (week == 7) {
                        monthhtml += ('</tr>');
                        week = 0;
                    }
                }
                for (i = 1; week != 0; i++) {
                    // next month days fill
                    monthhtml += ('<td class="deactive1"><div class="td-hight100 deactive1"><label class="block">' + i + '</label></div></td>');
                    week++;
                    if (week == 7) {
                        monthhtml += ('</tr>');
                        week = 0;
                    }
                }
            }
            else {


                var date = new Date('' + CriteriaMonth.substr(0, 2) + '/01/' + CriteriaMonth.substr(3, 4) + '');

                var day = date.getDate();

                var month = date.getMonth();
                //Bug # PMS-2970 By Mohsin Nasir
                //if (month == 0)
                //    month = 1;
                //END Bug # PMS-2970 By Mohsin Nasir
                var year = date.getFullYear();

                //if (year <= 200) {
                //    year += 1900;
                //}
                months = new Array('January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December');
                days_in_month = new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31);
                if (utility.LeapYear(year)) {
                    days_in_month[1] = 29;
                }
                total = days_in_month[month];
                var date_today = day + ' ' + months[month] + ' ' + year;
                var month_today = months[month] + ' ' + year;
                beg_j = date;
                beg_j.setDate(1);
                if (beg_j.getDate() == 2) {
                    beg_j = setDate(0);
                }
                beg_j = beg_j.getDay();

                //Try to get tbody first with jquery children. works faster!
                var tbody = $('#pnlScheduleCalendar #monthTable').children('tbody');

                //Then if no tbody just select your table
                var table = tbody.length ? tbody : $('#pnlScheduleCalendar #monthTable');
                var monthhtml = '<tr></tr>';
                //   monthhtml+=('<tr>   <th colspan="7">' + month_today + '</th>     </tr>');

                monthhtml += ('<tr ><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th><th class="p-none" width="14.28%"></th></tr><tr>');
                week = 0;
                // previous month days
                for (i = 1; i <= beg_j; i++) {
                    if (days_in_month[month - 1] == undefined) {
                        monthDays = 31;
                    } else {
                        monthDays = days_in_month[month - 1];
                    }
                    //original line
                    // monthhtml += ('<td><div class="td-hight100"><label class="block">' + (days_in_month[month - 1] - beg_j + i) + '</label></div></td>');
                    monthhtml += ('<td><div class="td-hight100"><label class="block">' + (monthDays - beg_j + i) + '</label></div></td>');
                    week++;
                }
                for (i = 1; i <= total; i++) {
                    if (week == 0) {
                        monthhtml += ('<tr>');
                    }

                    // current day fill

                    if (day == i) {

                        monthhtml += ('<td><div class="td-hight100"><label class="block">' + i + '</label></div></td>');


                    }
                        //normal days fill
                    else {
                        monthhtml += ('<td><div class="td-hight100"><label class="block">' + i + '</label></div></td>');




                    }

                    week++;
                    if (week == 7) {
                        monthhtml += ('</tr>');
                        week = 0;
                    }
                }
                for (i = 1; week != 0; i++) {
                    // next month days fill
                    monthhtml += ('<td><div class="td-hight100"><label class="block">' + i + '</label></div></td>');
                    week++;
                    if (week == 7) {
                        monthhtml += ('</tr>');
                        week = 0;
                    }
                }
            }
            table.append(monthhtml);
        });
        // End Start of Monthly view calendar
    },

    MonthClick: function () {
        $('#pnlScheduleCalendar #txtgoto').val('');
        $("#pnlScheduleCalendar #btngoto").addClass('disableAll');
        $('#btnday').removeClass('active');
        $('#btnweek').removeClass('active');
        $('#btnmnth').addClass('active');
        $("#pnlScheduleCalendar #checkboxpanel").css("display", "none");
        $("#pnlScheduleCalendar #yeardiv").css("display", "none");
        $("#pnlScheduleCalendar #monthdiv").css("display", "none");
        $("#pnlScheduleCalendar #fromdate").css("display", "");
        $('#pnlScheduleCalendar #agenda').val('P1');
        $("#pnlScheduleCalendar #btnblock").css("display", "");
        $("#pnlScheduleCalendar #btnunblock").css("display", "");
        $("#pnlScheduleCalendar #facilityheader").css("display", "none");
        $("#pnlScheduleCalendar #proresheader").css("display", "none");
        $("#pnlScheduleCalendar #MonthCal").css("display", "");
        $("#pnlScheduleCalendar #WeekCal").css("display", "none");
        $('#pnlScheduleCalendar #divDayName').html("");
        $('#pnlScheduleCalendar #divDayName').css('display', 'none');
        $('#pnlScheduleCalendar #btnProviderPrint').attr("disabled", "disabled");
        Scheduling_Calendar.ClearTable();

        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        //expression for week range
        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        //Month Regular Expression
        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
        var criteria = $('#pnlScheduleCalendar #daydate').text().trim();
        if (criteria.match(dayrgx) && criteria.length < 15) {

            var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
            if (a == 'Invalid Date') {
                a = new Date();
            }
            Scheduling_Calendar.ClearTable();
            var statusslots = Scheduling_Calendar.FilterCriteria();
            var month = $.datepicker.formatDate('mm/yy', new Date(a));

            if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), null, month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
            }

            else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                Scheduling_Calendar.MonthCalendar(null, $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());

            }

            var monthDatetxt = $.datepicker.formatDate('MM/yy', new Date(a));

            $('#pnlScheduleCalendar #calmonth').val(monthDatetxt);

            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', new Date(a)));
            //$('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);

            //if (utility.UserBrowser() == "Firefox" || utility.UserBrowser().toUpperCase() == "IE") {
            //    $('#pnlScheduleCalendar #calmonth').datepicker("update", monthDatetxt);
            //}
            //else {
            //    $('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);
            //}

        }

        else if (criteria.match(weekrg)) {
            var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
            if (a == 'Invalid Date') {
                a = new Date();
            }
            Scheduling_Calendar.ClearTable();
            var statusslots = Scheduling_Calendar.FilterCriteria();
            var month = $.datepicker.formatDate('mm/yy', new Date(a));

            if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), null, month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());

            }
            else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                Scheduling_Calendar.MonthCalendar(null, $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
            }

            var monthDatetxt = $.datepicker.formatDate('MM/yy', new Date(a));
            $('#pnlScheduleCalendar #calmonth').val(monthDatetxt);
            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', new Date(a)));
            //$('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);
            //if (utility.UserBrowser() == "Firefox" || utility.UserBrowser().toUpperCase() == "IE") {
            //    $('#pnlScheduleCalendar #calmonth').datepicker("update", monthDatetxt);
            //}
            //else {
            //    $('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);
            //}
        }
        else if (criteria.match(monthreg)) {

            var a = $.datepicker.formatDate('mm/dd/yy', new Date(Scheduling_Calendar.FormatDate(criteria)));
            //var a = $.datepicker.formatDate('yy/mm/dd', new Date(criteria));
            var month = $.datepicker.formatDate('mm/yy', new Date(a));
            Scheduling_Calendar.ClearTable();
            var statusslots = Scheduling_Calendar.FilterCriteria();

            if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), null, month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());

            }
            else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                Scheduling_Calendar.MonthCalendar(null, $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());

            }

            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', new Date(a)));
            //$('#pnlScheduleCalendar #calmonth').datepicker("setDate", $('#pnlScheduleCalendar #daydate span').html());

            //if (utility.UserBrowser() == "Firefox" || utility.UserBrowser().toUpperCase() == "IE") {
            //    $('#pnlScheduleCalendar #calmonth').datepicker("update", monthDatetxt);
            //}
            //else {
            //    $('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);
            //}
        }


    },

    MovePatientAppointment: function () {

        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment", "UPDATE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        $('a[data-toggle="tooltip"]').tooltip({
            animated: 'fade',
            placement: 'bottom',
            container: 'body',
            html: true,

        });
        $(".appdrag").draggable({
            revert: true,
            appendTo: 'body',
            stack: '.appdrop',

            start: function () {

                $('#myTable').find(".open").removeClass("open");
                $(this).parent().find('.appdrag').css("z-index", '');
                $(this).parent().css("z-index", Scheduling_Calendar.zIndexDraggable);

                $(this).css("z-index", '1018');
                $('#tabs').css('z-index', '9999');
                $('.tooltip').remove();
            },
            stop: function () {
                // $('#myTable').find(".open");
                $(this).css("z-index", "");
                $('#tabs').css('z-index', '0');
                $('.tooltip').remove();
            }
        });


        $(".appdrop").droppable({
            accept: ".appdrag",
            activeClass: 'droppable-active',
            hoverClass: 'droppable-hover',
            tolerance: 'pointer',
            drop: function (ev, ui) {
                $(this).css("z-index", "");
                $('.zIndex').parent().css("z-index", "");
                //$(".zIndexActive").parent().css("z-index", "");
                //alert(ui.draggable.attr('id'));
                //alert($(this).attr('id'));
                var test = $(this);
                var exist = 0;
                var a = $(this).attr('id');
                var b = ui.draggable.attr('id');
                var arr = a.split(',');
                var arr1 = b.split(',');
                var dragslotdetailid = arr1[1];
                CutAppointmentId = arr1[0];
                var apppatientid = arr1[2];
                var appcount = arr1[3];

                var againexist = 0;

                //Mohsin

                //var droptime = arr[3];
                //var dragtime = arr1[4];

                var droptime = $(this).attr('data-fromtime');
                var dragtime = arr1.length == 5 ? arr1[arr1.length - 1] : arr1[arr1.length - 2];

                var dragday = '';
                var dropday = '';
                if (arr1[5] != undefined)
                    dragday = arr1[5];
                else {
                    if (arr1[4] != undefined)
                        dragday = arr1[4];
                }
                if (arr[4] != undefined)
                    dropday = arr[4];
                slotid = arr[0].replace('Slotid', '');
                slotdetailid = arr[1].replace('TimeSlotDtlId', '');
                var isDayCal = !(dragday != null && (dragday.toLowerCase().indexOf('am') > 0 || dragday.toLowerCase().indexOf('pm') > 0) ? true : false);
                //var genericMessage = false;
                //var currentTr = "";
                if (dragday != dropday || isDayCal) {
                    // same slot movement
                    for (var x1 = 2; x1 <= arr.length; x1++) {
                        if ($.trim(arr[x1]) == $.trim(apppatientid) && $.trim(dragslotdetailid) == $.trim(slotdetailid)) {

                            exist = 1;
                        }

                    }
                    //for (var ini = 0; ini < appcount - 1; ini++) {
                    //    var nextTr = $(this).closest("tr").next("tr");

                    //    if (currentTr == "") currentTr = nextTr;
                    //    var nextTrId = currentTr.attr("id").split(',');
                    //    if (!(nextTrId == "")) {
                    //        for (var init = 0; init < nextTrId.length; init++) {
                    //            if (apppatientid == nextTrId[init]) {
                    //                exist = 1;
                    //                genericMessage = true;
                    //            }
                    //        }
                    //    }
                    //    currentTr = currentTr.closest("tr").next("tr");
                    //}
                    if (appcount > 1) {
                        if (dragslotdetailid < slotdetailid) {
                            for (var dr = 0; dr < appcount; dr++) {
                                var sameappmoveid = parseInt(dragslotdetailid) + parseInt(dr);
                                if (slotdetailid == sameappmoveid) {
                                    againexist = 1;
                                }
                            }
                            //if ($(this).parent().attr('id') != undefined && againexist == 0) {
                            //    for (var l = 1; l <= appcount - 1; l++) {
                            //        if (l == 1)
                            //            var nextrow = $(this).parent().attr('id').split(',');
                            //        else
                            //            if ($(this).parent().next().attr('id') != undefined)
                            //                var nextrow = $(this).parent().next().attr('id').split(',');
                            //        //var nextrow = $(this).parent().next().attr('id').split(',');
                            //        for (var x1 = 0; x1 <= nextrow.length; x1++) {
                            //            if ($.trim(nextrow[x1]) == $.trim(apppatientid)) {

                            //                exist = 1;
                            //            }

                            //        }
                            //    }
                            //}
                        }
                        if (dragslotdetailid > slotdetailid) {
                            if ($(this).parent().attr('id') != undefined) {
                                //for (var l = 1; l <= appcount - 1; l++) {

                                //    if (l == 1)
                                //        var nextrow = $(this).parent().attr('id').split(',');
                                //    else
                                //        if ($(this).parent().prev().attr('id') != undefined)
                                //            var nextrow = $(this).parent().prev().attr('id').split(',');
                                //    //var nextrow = $(this).parent().next().attr('id').split(',');
                                //    for (var x1 = 0; x1 <= nextrow.length; x1++) {
                                //        if ($.trim(nextrow[x1]) == $.trim(apppatientid)) {

                                //            exist = 1;
                                //        }

                                //    }
                                //}
                            }
                        }
                    }
                    //params["TimeSlotDtlId"] = slotdetailid;
                    //params["CutAppointmentId"] = CutAppointmentId;

                    var drag_element = ui.draggable[0];

                    if ($.trim(dragslotdetailid) != $.trim(slotdetailid) && exist != 1) {
                        AppPrivileges.GetFormPrivileges("Appointment", "UPDATE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                utility.myConfirm('Are you sure want to move appointment from ' + dragtime + ' to ' + droptime + '', function () {

                                    Scheduling_Calendar.MoveAppointment(CutAppointmentId, slotdetailid).done(function (response) {
                                        if (response.status != false) {
                                            utility.DisplayMessages(response.Message, 1);
                                            ui.draggable.detach().appendTo(test);



                                            var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                            //expression for week range
                                            var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                                            //Month Regular Expression
                                            var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

                                            var date = $('#daydate').text().trim();

                                            if (date.match(weekrg) && date.length > 15) {
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

                                                var providerid = $('#pnlScheduleCalendar #Provider').val();
                                                var resourceid = $('#pnlScheduleCalendar #Resource').val();
                                                var facilityid = $('#pnlScheduleCalendar #Facility').val();
                                                //var date = $('#daydate').text().trim();
                                                var statusslots = Scheduling_Calendar.FilterCriteria();
                                                if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                                                    Scheduling_Calendar.DayCalendar(providerid, null, facilityid, date, statusslots);
                                                }

                                                else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                                                    Scheduling_Calendar.DayCalendar(null, resourceid, facilityid, date, statusslots);

                                                }
                                                if (providerid != "")
                                                    $("#pnlScheduleCalendar #Provider option[value=" + providerid + "]").attr('selected', 'selected');
                                                if (resourceid != "")
                                                    $("#pnlScheduleCalendar #Resource option[value=" + resourceid + "]").attr('selected', 'selected');
                                                if (facilityid != "")
                                                    $("#pnlScheduleCalendar #Facility option[value=" + facilityid + "]").attr('selected', 'selected');
                                                if (date != "")
                                                    $('#pnlScheduleCalendar #daydate span').html(date);

                                            }

                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                            var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                            //expression for week range
                                            var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                                            //Month Regular Expression
                                            var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

                                            var date = $('#daydate').text().trim();

                                            if (date.match(weekrg) && date.length > 15) {
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

                                                var providerid = $('#pnlScheduleCalendar #Provider').val();
                                                var resourceid = $('#pnlScheduleCalendar #Resource').val();
                                                var facilityid = $('#pnlScheduleCalendar #Facility').val();
                                                //var date = $('#daydate').text().trim();
                                                var statusslots = Scheduling_Calendar.FilterCriteria();

                                                if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                                                    Scheduling_Calendar.DayCalendar(providerid, null, facilityid, date, statusslots);
                                                }

                                                else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                                                    Scheduling_Calendar.DayCalendar(null, resourceid, facilityid, date, statusslots);

                                                }


                                                if (providerid != "")
                                                    $("#pnlScheduleCalendar #Provider option[value=" + providerid + "]").attr('selected', 'selected');
                                                if (resourceid != "")
                                                    $("#pnlScheduleCalendar #Resource option[value=" + resourceid + "]").attr('selected', 'selected');
                                                if (facilityid != "")
                                                    $("#pnlScheduleCalendar #Facility option[value=" + facilityid + "]").attr('selected', 'selected');
                                                if (date != "")
                                                    $('#pnlScheduleCalendar #daydate span').html(date);

                                            }
                                        }
                                    });

                                }, function () { },
                       'Move Appointment'
                   );
                            }
                            else
                                utility.DisplayMessages(strMessage, 2);
                        });
                    }


                    else if ($.trim(dragslotdetailid) != $.trim(slotdetailid) && exist == 1) {
                        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                        //expression for week range
                        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                        //Month Regular Expression
                        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

                        var date = $('#daydate').text().trim();

                        if (date.match(weekrg) && date.length > 15) {
                            var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                            //Scheduling_Calendar.ClearTable();

                            var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                            var week = Scheduling_Calendar.GetWeek(date1);
                            $('#pnlScheduleCalendar #daydate span').html(week);
                            //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                            var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                        }

                        else {

                            var providerid = $('#pnlScheduleCalendar #Provider').val();
                            var resourceid = $('#pnlScheduleCalendar #Resource').val();
                            var facilityid = $('#pnlScheduleCalendar #Facility').val();
                            //var date = $('#daydate').text().trim();
                            var statusslots = Scheduling_Calendar.FilterCriteria();
                            if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                                Scheduling_Calendar.DayCalendar(providerid, null, facilityid, date, statusslots);
                            }

                            else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                                Scheduling_Calendar.DayCalendar(null, resourceid, facilityid, date, statusslots);

                            }
                            if (providerid != "")
                                $("#pnlScheduleCalendar #Provider option[value=" + providerid + "]").attr('selected', 'selected');
                            if (resourceid != "")
                                $("#pnlScheduleCalendar #Resource option[value=" + resourceid + "]").attr('selected', 'selected');
                            if (facilityid != "")
                                $("#pnlScheduleCalendar #Facility option[value=" + facilityid + "]").attr('selected', 'selected');
                            if (date != "")
                                $('#pnlScheduleCalendar #daydate span').html(date);

                        }
                        //if (genericMessage) {
                        //    utility.DisplayMessages('Patient is already booked in current time range.', 3);
                        //} else {
                        utility.DisplayMessages('Patient is already booked on ' + droptime + ' slot.', 3);
                        //}


                    }
                    //  ui.draggable.detach().appendTo($(this));
                }

                var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                //expression for week range
                var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                //Month Regular Expression
                var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

                var date = $('#daydate').text().trim();

                if (date.match(weekrg) && date.length > 15) {
                    var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                    // Scheduling_Calendar.ClearTable();

                    var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                    var week = Scheduling_Calendar.GetWeek(date1);
                    $('#pnlScheduleCalendar #daydate span').html(week);
                    //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                    var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                    //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                }

                else {

                    var providerid = $('#pnlScheduleCalendar #Provider').val();
                    var resourceid = $('#pnlScheduleCalendar #Resource').val();
                    var facilityid = $('#pnlScheduleCalendar #Facility').val();
                    //var date = $('#daydate').text().trim();
                    var statusslots = Scheduling_Calendar.FilterCriteria();
                    //if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                    //    Scheduling_Calendar.DayCalendar(providerid, null, facilityid, date, statusslots);

                    //}

                    //else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                    //    Scheduling_Calendar.DayCalendar(null, resourceid, facilityid, date, statusslots);


                    //}
                    if (providerid != "")
                        $("#pnlScheduleCalendar #Provider option[value=" + providerid + "]").attr('selected', 'selected');
                    if (resourceid != "")
                        $("#pnlScheduleCalendar #Resource option[value=" + resourceid + "]").attr('selected', 'selected');
                    if (facilityid != "")
                        $("#pnlScheduleCalendar #Facility option[value=" + facilityid + "]").attr('selected', 'selected');
                    if (date != "")
                        $('#pnlScheduleCalendar #daydate span').html(date);

                }


            }


        });

        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    EditSlot: function (TimeslotDetailid) {

        if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
            var criteria = $('#pnlScheduleCalendar #daydate').text().trim();

            var params = [];
            params["ParentCtrl"] = "schTabCalendar";
            params["TimeslotDetailid"] = TimeslotDetailid;
            params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
            params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
            params["ResourceId"] = null;
            params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();

            LoadActionPan('schEditSlot', params);
        }

        else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
            var criteria = $('#pnlScheduleCalendar #daydate').text().trim();

            var params = [];
            params["ParentCtrl"] = "schTabCalendar";
            params["TimeslotDetailid"] = TimeslotDetailid;
            params["ProviderId"] = null;
            params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
            params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
            params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();

            LoadActionPan('schEditSlot', params);
        }

    },

    GetWeek: function (weekdate) {
        var formatted_date = function (date) {
            var m = ("0" + (date.getMonth() + 1)).slice(-2); // in javascript month start from 0.
            var d = ("0" + date.getDate()).slice(-2); // add leading zero
            var y = date.getFullYear();
            return m + '/' + d + '/' + y;
        }

        var curr_date = new Date(weekdate);

        var day = curr_date.getDay();

        var diff = curr_date.getDate() - day + (day == 0 ? -6 : 1); // 0 for sunday

        var week_start_tstmp = curr_date.setDate(diff);

        var week_start = new Date(week_start_tstmp);

        var week_start_date = formatted_date(week_start);

        var week_end = new Date(week_start_tstmp);  // first day of week

        week_end = new Date(week_end.setDate(week_end.getDate() + 6));

        var week_end_date = formatted_date(week_end);

        var sd = new Date(week_start_date);
        var newdatesd = Scheduling_Calendar.AddSubDays(sd, -1);
        newdatesd = formatted_date(newdatesd);

        var ed = new Date(week_end_date);
        var newdateed = Scheduling_Calendar.AddSubDays(ed, -1);
        newdateed = formatted_date(newdateed);

        date = newdatesd + ' - ' + newdateed;    // date range for current week

        /*
          var week_end_date =formatted_date(new Date()); // limit current week date range upto current day.
        */
        return date;
    },

    TodayDate: function () {
        //String format yyyy/mm/dd
        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        //expression for week range
        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        //Month Regular Expression
        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
        var criteria = $('#pnlScheduleCalendar #daydate').text().trim();
        if (criteria.match(dayrgx)) {
            var curdte = $.datepicker.formatDate('yy/mm/dd', new Date());
            $('#divDayName').removeAttr('style');
            $('#pnlScheduleCalendar #divDayName').html($.datepicker.formatDate('DD', new Date(curdte)) + ", ");

            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', new Date()));
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #daydate').text().trim());
        }
        else if (criteria.match(monthreg)) {

            var month = $.datepicker.formatDate('mm/yy', new Date());
            var statusslots = Scheduling_Calendar.FilterCriteria();
            //if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

            //    Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), null, month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
            //}

            //else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

            //    Scheduling_Calendar.MonthCalendar(null, $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());

            //}

            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', new Date()));
            var monthDatetxt = $.datepicker.formatDate('MM/yy', new Date());
            $('#pnlScheduleCalendar #calmonth').val(monthDatetxt);

            $('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);
        }
        else if (criteria.match(weekrg)) {
            var date1 = $.datepicker.formatDate('yy/mm/dd', new Date());
            var week = Scheduling_Calendar.GetWeek(date1);
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", week);
            $('#pnlScheduleCalendar #daydate span').html(week);
        }
    },

    BackDate: function () {
        //String format yyyy/mm/dd
        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        //expression for week range
        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        //Month Regular Expression
        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

        var criteria = $('#pnlScheduleCalendar #daydate').text().trim();

        if (criteria.match(dayrgx) && criteria.length < 15) {
            var d = new Date(criteria);
            var newdate = Scheduling_Calendar.AddSubDays(d, -1);
            var curdte = $.datepicker.formatDate('yy/mm/dd', newdate);

            $('#pnlScheduleCalendar #divDayName').removeAttr('style');
            $('#divDayName').html($.datepicker.formatDate('DD', new Date(curdte)) + ", ");
            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', newdate));
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #daydate').text().trim());
        }
        else if (criteria.match(weekrg)) {
            var getdate = new Date(criteria.substr(13, 10));
            var newdate = Scheduling_Calendar.AddSubDays(getdate, -7);

            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", newdate);

            var formatted_date = function (date) {
                var m = ("0" + (date.getMonth() + 1)).slice(-2); // in javascript month start from 0.
                var d = ("0" + date.getDate()).slice(-2); // add leading zero
                var y = date.getFullYear();
                return m + '/' + d + '/' + y;
            }

            var curr_date = new Date(newdate);

            var day = curr_date.getDay();

            var diff = curr_date.getDate() - day + (day == 0 ? -6 : 1); // 0 for sunday

            var week_start_tstmp = curr_date.setDate(diff);

            var week_start = new Date(week_start_tstmp);

            var week_start_date = formatted_date(week_start);

            var week_end = new Date(week_start_tstmp);  // first day of week

            week_end = new Date(week_end.setDate(week_end.getDate() + 6));

            var week_end_date = formatted_date(week_end);

            var sd = new Date(week_start_date);
            var newdatesd = Scheduling_Calendar.AddSubDays(sd, -1);
            newdatesd = formatted_date(newdatesd);

            var ed = new Date(week_end_date);
            var newdateed = Scheduling_Calendar.AddSubDays(ed, -1);
            newdateed = formatted_date(newdateed);

            date = newdatesd + ' - ' + newdateed;    // date range for current week    // date range for current week

            $('#pnlScheduleCalendar #daydate span').html(date);
        }
        else if (criteria.match(monthreg)) {

            //var d = new Date($.datepicker.formatDate('mm/dd/yy', new Date(Scheduling_Calendar.FormatDate(criteria))));
            ////var d = new Date(criteria);
            //var newdate = Scheduling_Calendar.AddSubMonth(d, -1);

            //var month = $.datepicker.formatDate('mm/yy', new Date(newdate));

            //var statusslots = Scheduling_Calendar.FilterCriteria();

            ////if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

            ////    Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), null, month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
            ////}

            ////else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

            ////    Scheduling_Calendar.MonthCalendar(null, $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());

            ////}

            //$('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', newdate));
            ////Scheduling_Calendar.FormatDate($('#pnlScheduleCalendar #daydate').text().trim())
            //var temp = $.datepicker.formatDate('mm/dd/yy', new Date(Scheduling_Calendar.FormatDate($('#pnlScheduleCalendar #daydate').text().trim())));
            //var monthDatetxt = $.datepicker.formatDate('MM/yy', new Date(temp));
            ////var monthDatetxt = $.datepicker.formatDate('MM/yy', new Date($('#pnlScheduleCalendar #daydate').text().trim()));
            //$('#pnlScheduleCalendar #calmonth').val(monthDatetxt);

            //$('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);



            //--------------- new
            var d = new Date(Scheduling_Calendar.FormatDate(criteria));

            var txtgot = parseInt($('#pnlScheduleCalendar #txtgoto').val());
            var newdate = Scheduling_Calendar.AddSubMonth(d, -1);
            var month = $.datepicker.formatDate('mm/yy', newdate);

            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', newdate));
            var monthDatetxt = $.datepicker.formatDate('MM/yy', newdate);
            $('#pnlScheduleCalendar #calmonth').val(monthDatetxt);

            $('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);

        }
    },

    NextDate: function () {
        //String format yyyy/mm/dd
        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        //expression for week range
        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        //Month Regular Expression
        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
        var criteria = $('#pnlScheduleCalendar #daydate').text().trim();

        //var re1 = '((?:Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday|Tues|Thur|Thurs|Sun|Mon|Tue|Wed|Thu|Fri|Sat))';	// Day Of Week 1
        //var re2 = '(\\s+)';
        //var re3 = '((?:[0]?[1-9]|[1][012])[-:\\/.](?:(?:[0-2]?\\d{1})|(?:[3][01]{1}))[-:\\/.](?:(?:[1]{1}\\d{1}\\d{1}\\d{1})|(?:[2]{1}\\d{3})))(?![\\d])';	// MMDDYYYY 1
        //&& criteria.match(new RegExp(re1 + re2 + re3, ["i"]))
        if (criteria.match(dayrgx) && criteria.length < 15) {

            var d = new Date(criteria);
            var newdate = Scheduling_Calendar.AddSubDays(d, 1);
            var curdte = $.datepicker.formatDate('yy/mm/dd', newdate);

            $('#pnlScheduleCalendar #divDayName').removeAttr('style');
            $('#divDayName').html($.datepicker.formatDate('DD', new Date(curdte)) + ", ");
            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', newdate));
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #daydate').text().trim());
        }
        else if (criteria.match(weekrg)) {
            var getdate = new Date(criteria.substr(0, 10));
            var newdate = Scheduling_Calendar.AddSubDays(getdate, 8);

            $('#pnlScheduleCalendar #divDayName').html("");
            $('#pnlScheduleCalendar #divDayName').css('display', 'none');
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", newdate);

            var formatted_date = function (date) {
                var m = ("0" + (date.getMonth() + 1)).slice(-2); // in javascript month start from 0.
                var d = ("0" + date.getDate()).slice(-2); // add leading zero
                var y = date.getFullYear();
                return m + '/' + d + '/' + y;
            }

            var curr_date = new Date(newdate);

            var day = curr_date.getDay();

            var diff = curr_date.getDate() - day + (day == 0 ? -6 : 1); // 0 for sunday

            var week_start_tstmp = curr_date.setDate(diff);

            var week_start = new Date(week_start_tstmp);

            var week_start_date = formatted_date(week_start);

            var week_end = new Date(week_start_tstmp);  // first day of week

            week_end = new Date(week_end.setDate(week_end.getDate() + 6));

            var week_end_date = formatted_date(week_end);


            var sd = new Date(week_start_date);
            var newdatesd = Scheduling_Calendar.AddSubDays(sd, -1);
            newdatesd = formatted_date(newdatesd);

            var ed = new Date(week_end_date);
            var newdateed = Scheduling_Calendar.AddSubDays(ed, -1);
            newdateed = formatted_date(newdateed);

            date = newdatesd + ' - ' + newdateed;    // date range for current week    // date range for current week



            $('#pnlScheduleCalendar #daydate span').html(date);
        }
        else if (criteria.match(monthreg)) {

            //var d = new Date($.datepicker.formatDate('mm/dd/yy', new Date(Scheduling_Calendar.FormatDate(criteria))));

            ////var d = new Date(criteria);
            //var newdate = Scheduling_Calendar.AddSubMonth(d, 1);
            //var month = $.datepicker.formatDate('mm/yy', new Date(newdate));



            //var statusslots = Scheduling_Calendar.FilterCriteria();

            ////if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

            ////    Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), null, month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
            ////}

            ////else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

            ////    Scheduling_Calendar.MonthCalendar(null, $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
            ////}

            //$('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', newdate));

            ////Scheduling_Calendar.FormatDate($('#pnlScheduleCalendar #daydate').text().trim())
            //var temp = $.datepicker.formatDate('mm/dd/yy', new Date(Scheduling_Calendar.FormatDate($('#pnlScheduleCalendar #daydate').text().trim())));
            //var monthDatetxt = $.datepicker.formatDate('MM/yy', new Date(temp));
            //$('#pnlScheduleCalendar #calmonth').val(monthDatetxt);

            //$('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);




            //----------- new


            var d = new Date(Scheduling_Calendar.FormatDate(criteria));

            var txtgot = parseInt($('#pnlScheduleCalendar #txtgoto').val());
            var newdate = Scheduling_Calendar.AddSubMonth(d, 1);
            var month = $.datepicker.formatDate('mm/yy', newdate);
            $('#pnlScheduleCalendar #divDayName').html("");
            $('#pnlScheduleCalendar #divDayName').css('display', 'none');
            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', newdate));
            var monthDatetxt = $.datepicker.formatDate('MM/yy', newdate);
            $('#pnlScheduleCalendar #calmonth').val(monthDatetxt);

            $('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);
        }
    },

    GoTo: function () {


        //String format yyyy/mm/dd
        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        //expression for week range
        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        //Month Regular Expression
        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
        var criteria = $('#daydate').text().trim();

        if (criteria.match(dayrgx) && criteria.length < 15) {
            var txtgot = $('#pnlScheduleCalendar #txtgoto').val();
            var d = new Date(criteria);
            var newdate = Scheduling_Calendar.AddSubDays(d, txtgot);
            var curdte = $.datepicker.formatDate('yy/mm/dd', newdate);
            var statusslots = Scheduling_Calendar.FilterCriteria();

            var weekdays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $('#pnlScheduleCalendar #divDayName').removeAttr('style');
            //$('#pnlScheduleCalendar #divDayName span').html(weekdays[new Date(curdte).getDay()]);
            $('#divDayName').html($.datepicker.formatDate('DD', new Date(curdte)) + ", ");
            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', newdate));
            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#pnlScheduleCalendar #daydate').text().trim());
        }
        else if (criteria.match(weekrg)) {
            var getdate = new Date(criteria.substr(0, 10));

            var txtgot = $('#pnlScheduleCalendar #txtgoto').val();
            var newdate = Scheduling_Calendar.AddSubDays(getdate, txtgot * 8);
            $('#pnlScheduleCalendar #divDayName').html("");
            $('#pnlScheduleCalendar #divDayName').css('display', 'none');

            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", newdate);

            var formatted_date = function (date) {
                var m = ("0" + (date.getMonth() + 1)).slice(-2); // in javascript month start from 0.
                var d = ("0" + date.getDate()).slice(-2); // add leading zero
                var y = date.getFullYear();
                return m + '/' + d + '/' + y;
            }

            var curr_date = new Date(newdate);

            var day = curr_date.getDay();

            var diff = curr_date.getDate() - day + (day == 0 ? -6 : 1); // 0 for sunday

            var week_start_tstmp = curr_date.setDate(diff);

            var week_start = new Date(week_start_tstmp);

            var week_start_date = formatted_date(week_start);

            var week_end = new Date(week_start_tstmp);  // first day of week

            week_end = new Date(week_end.setDate(week_end.getDate() + 6));

            var week_end_date = formatted_date(week_end);


            var sd = new Date(week_start_date);
            var newdatesd = Scheduling_Calendar.AddSubDays(sd, -1);
            newdatesd = formatted_date(newdatesd);

            var ed = new Date(week_end_date);
            var newdateed = Scheduling_Calendar.AddSubDays(ed, -1);
            newdateed = formatted_date(newdateed);

            date = newdatesd + ' - ' + newdateed;    // date range for current week    // date range for current week



            $('#pnlScheduleCalendar #daydate span').html(date);
        }
        else if (criteria.match(monthreg)) {
            var d = new Date(Scheduling_Calendar.FormatDate(criteria));

            var txtgot = parseInt($('#pnlScheduleCalendar #txtgoto').val());
            var newdate = Scheduling_Calendar.AddSubMonth(d, txtgot);
            var month = $.datepicker.formatDate('mm/yy', newdate);

            //if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

            //    Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), null, month, "", $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());

            //}

            //else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

            //    Scheduling_Calendar.MonthCalendar(null, $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, "", $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
            //}
            $('#pnlScheduleCalendar #divDayName').html("");
            $('#pnlScheduleCalendar #divDayName').css('display', 'none');

            $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', newdate));
            var monthDatetxt = $.datepicker.formatDate('MM/yy', newdate);
            $('#pnlScheduleCalendar #calmonth').val(monthDatetxt);

            $('#pnlScheduleCalendar #calmonth').datepicker("setDate", monthDatetxt);
        }

    },

    PatientDemographics: function (patientid) {

        //var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {

        //        var params = [];
        //        params["mode"] = 'Edit';
        //        params["patientID"] = patientid;
        //        params["FromAdmin"] = "0";
        //        params["ParentCtrl"] = "schTabCalendar";
        //        LoadActionPan('demographicDetail', params);

        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
        params["QuickAddPatient"] = true;
        SelectTab('mstrTabPatient', 'false');
        setTimeout(function () {
            Patient_Search.params.FormName = "DayCalendar";
            Patient_Search.SelectPatient(patientid, null);
            $('#patTabDemographic').click();
            $("body").removeClass("modal-open").removeAttr("style");
        }, 200);


    },

    LoadCopayment: function (appid, patientid, facilityId, patientvisitid, patvisitname) {

        //if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

        //    utility.DisplayMessages("Copayment can't be collected for Resource Appointmnet", 3);
        //}

        //else {

        var params = [];
        params["FromAdmin"] = "0";
        params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
        params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
        if (params["FacilityId"] == "")
            params["FacilityId"] = facilityId;
        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        params["AppointmentId"] = appid;
        params["PatientId"] = patientid;

        params["PatientVisitId"] = patientvisitid;
        params["PatientVisitName"] = patvisitname;
        if (Scheduling_Calendar.params.PanelID == "pnlScheduleCalendar") {
            params["ParentCtrl"] = 'schTabCalendar';
        }
        else {
            params["ParentCtrl"] = 'schTabMultipleView';
        }
        LoadActionPan('schcopayment', params);
        //}
    },

    LoadCheckIn: function (appid, patientid, scheduleDate) {

        //if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

        //    utility.DisplayMessages("Resource Appointment can't CheckedIn", 3);
        //}
        // else {
        var selectedDate = $('#pnlScheduleCalendar #daydate').text().trim();
        var currentDate = ($.datepicker.formatDate('mm/dd/yy', new Date()));

        if (new Date(scheduleDate) <= new Date(currentDate)) {
            var params = [];
            params["FromAdmin"] = "0";
            params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
            params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
            params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
            params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
            params["AppointmentId"] = appid;
            params["PatientId"] = patientid;
            if (Scheduling_Calendar.params.PanelID == "pnlScheduleCalendar") {
                params["ParentCtrl"] = 'schTabCalendar';
            }
            else {
                params["ParentCtrl"] = 'schTabMultipleView';
            }
            LoadActionPan('schcheckin', params);
        }
        else {
            utility.DisplayMessages("Future Appointment can't CheckedIn", 3);
        }
        //var params = [];
        //params["FromAdmin"] = "0";
        //params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        //params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
        //params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
        //params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        //params["AppointmentId"] = appid;
        //params["PatientId"] = patientid;
        //params["ParentCtrl"] = 'schTabCalendar';
        //LoadActionPan('schcheckin', params);

        //}


    },

    LoadCheckOut: function (appid, patientid, patientvisitid) {

        //if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

        //    utility.DisplayMessages("Resource Appointment can't CheckedOut", 3);
        //}

        //   else {
        var params = [];
        params["FromAdmin"] = "0";
        params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
        params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        params["AppointmentId"] = appid;
        params["PatientId"] = patientid;
        params["PatientVisitId"] = patientvisitid;
        if (Scheduling_Calendar.params.PanelID == "pnlScheduleCalendar") {
            params["ParentCtrl"] = 'schTabCalendar';
        }
        else {
            params["ParentCtrl"] = 'schTabMultipleView';
        }
        LoadActionPan('schcheckout', params);

        // }
    },
    CreateVisitCharge: function (appid, patientid, VisitId) {
        var params = [];
        if (Scheduling_Calendar.params.PanelID == "pnlScheduleCalendar") {
            params["ParentCtrl"] = 'schTabCalendar';
        }
        else {
            params["ParentCtrl"] = 'schTabMultipleView';
        }
        params["AppointmentId"] = appid;
        params["VisitId"] = VisitId;
        params["patientID"] = patientid;
        params["AppointmentDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        LoadActionPan('EncounterChargeCapture', params);


        //***** Start New Code

        //var params = [];
        //params["FromAdmin"] = 0;
        //params["ParentCtrl"] = 'Bill_ChargeSearch';
        //params['mode'] = "Add";
        //params["PatientId"] = Patient_Demographic.params.patientID;
        //params["patFullName"] = Patient_Demographic.params.patFullName;
        //params["RefProviderId"] = Patient_Demographic.params.RefProviderId;
        //params["RefProviderName"] = Patient_Demographic.params.RefProviderName;
        //params["ProviderId"] = Patient_Demographic.params.PatientProviderId;
        //params["ProviderName"] = Patient_Demographic.params.PatientProvider;
        //params["FacilityId"] = Patient_Demographic.params.PatientFacilityId;
        //params["FaciltyName"] = Patient_Demographic.params.PatientFacility;
        //params["SelfPay"] = Patient_Demographic.params.SelfPay;
        //LoadActionPan('Encounter_CreateClaim', params);

        //***** End New Code


    },
    DeletePatientAppointment: function (AppointmentId) {
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

                                appointmentDetail.DeleteAppointmentRow(selectedValue);
                                utility.DisplayMessages(response.Message, 1);

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
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
                                //var table1 = $('#dgvSlotEdit').DataTable();
                                //table1.row('.active').remove().draw(false);


                                var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                //expression for week range
                                var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                                //Month Regular Expression
                                var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

                                var date = $('#pnlScheduleCalendar #daydate').text().trim();

                                if (date.match(weekrg) && date.length > 15) {
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
                                    var providerid = $('#pnlScheduleCalendar #Provider').val();
                                    var resourceid = $('#pnlScheduleCalendar #Resource').val();
                                    var facilityid = $('#pnlScheduleCalendar #Facility').val();
                                    //var date = $('#daydate').text().trim();

                                    if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                                        Scheduling_Calendar.DayCalendar(providerid, null, facilityid, date, statusslots);
                                    }

                                    else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                                        Scheduling_Calendar.DayCalendar(null, resourceid, facilityid, date, statusslots);

                                    }



                                    if (providerid != "")
                                        $("#pnlScheduleCalendar #Provider option[value=" + providerid + "]").attr('selected', 'selected');
                                    if (resourceid != "")
                                        $("#pnlScheduleCalendar #Resource option[value=" + resourceid + "]").attr('selected', 'selected');
                                    if (facilityid != "")
                                        $("#pnlScheduleCalendar #Facility option[value=" + facilityid + "]").attr('selected', 'selected');
                                    if (date != "")
                                        $('#pnlScheduleCalendar #daydate span').html(date);

                                }


                                utility.DisplayMessages(response.Message, 1);
                                //Scheduling_Calendar.DayCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Resource').val(), $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #daydate').text().trim());
                                //$('#daydate span').html($.datepicker.formatDate('yy/mm/dd', $('#pnlScheduleCalendar #daydate').text().trim()));

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '1'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    CancelPatientCheckIn: function (VisitId, AppointmentId, refObj) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Are you sure want to Cancel CheckIn?', function () {
                    var selectedValue = VisitId;
                    if (selectedValue == "" || selectedValue == "undefined" || AppointmentId == "" || AppointmentId == "undefined") {
                    }
                    else {
                        var lastStatusName = $(refObj).parent().parent().attr('data-laststatusname');
                        var lastStatusId = $(refObj).parent().parent().attr('data-laststatusid');
                        schcheckin.CancelPatCheckIn(selectedValue, AppointmentId, lastStatusId, lastStatusName).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvSlotEdit').DataTable();
                                //table1.row('.active').remove().draw(false);
                                if (Scheduling_Calendar.params.PanelID == "pnlScheduleMuliView") {
                                    Scheduling_MuliView.LoadMultipleViewCalendar();
                                }

                                var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                //expression for week range
                                var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                                //Month Regular Expression
                                var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;

                                var date = $('#pnlScheduleCalendar #daydate').text().trim();

                                if (date.match(weekrg) && date.length > 15) {
                                    var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                                    Scheduling_Calendar.ClearTable();

                                    var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                                    var week = Scheduling_Calendar.GetWeek(date1);
                                    $('#pnlScheduleCalendar #daydate span').html(week);
                                    // $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                                    var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                                    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                                }

                                else {
                                    var statusslots = Scheduling_Calendar.FilterCriteria();
                                    var providerid = $('#pnlScheduleCalendar #Provider').val();
                                    var resourceid = $('#pnlScheduleCalendar #Resource').val();
                                    var facilityid = $('#pnlScheduleCalendar #Facility').val();
                                    //var date = $('#daydate').text().trim();
                                    if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
                                        Scheduling_Calendar.DayCalendar(providerid, null, facilityid, date, statusslots);
                                    }
                                    else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                                        Scheduling_Calendar.DayCalendar(null, resourceid, facilityid, date, statusslots);
                                    }
                                    //Scheduling_Calendar.DayCalendar(providerid, resourceid, facilityid, date, statusslots);
                                    if (providerid != "")
                                        $("#pnlScheduleCalendar #Provider option[value=" + providerid + "]").attr('selected', 'selected');
                                    if (resourceid != "")
                                        $("#pnlScheduleCalendar #Resource option[value=" + resourceid + "]").attr('selected', 'selected');
                                    if (facilityid != "")
                                        $("#pnlScheduleCalendar #Facility option[value=" + facilityid + "]").attr('selected', 'selected');
                                    if (date != "")
                                        $('#pnlScheduleCalendar #daydate span').html(date);

                                }


                                utility.DisplayMessages(response.Message, 1);
                                //Scheduling_Calendar.DayCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Resource').val(), $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #daydate').text().trim());
                                //$('#daydate span').html($.datepicker.formatDate('yy/mm/dd', $('#pnlScheduleCalendar #daydate').text().trim()));

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    'Confirm Cancel CheckIn'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PrintLetter: function (patientID, visitid, appid) {

        //var params = [];
        //params["FromAdmin"] = "0";
        //params["ParentCtrl"] = 'schTabCalendar';
        //LoadActionPan('designLetterPrinting', params);

        var params = [];
        params["FromAdmin"] = "0";
        //params["NoteId"] = BillingInformation.params.NotesId;
        // params["PreviousTab"] = GetSelectedTab();
        if (Scheduling_Calendar.params.PanelID == "pnlScheduleCalendar") {
            params["ParentCtrl"] = 'schTabCalendar';
        }
        else {
            params["ParentCtrl"] = 'schTabMultipleView';
        }
        params["PatientId"] = patientID.toString();
        params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        params["VisitId"] = visitid;
        params["appid"] = appid;
        LoadActionPan('Clinical_SuperBillTemplate', params);
    },

    FilterCriteria: function () {
        var stausid = "";
        $("#pnlScheduleCalendar #statusColorPanel").find("div").find('input[type=checkbox]').each(function () {
            var id = $(this).attr('id');

            var id = $(this).attr('id');

            if ($(this).is(':checked')) {
                stausid += id + ',';

            } else {

            }
        });
        return stausid;
    },

    ChangeDate: function (IsUpdate) {



        //String format yyyy/mm/dd
        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        //expression for week range
        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        //Month Regular Expression
        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
        var criteria = $('#pnlScheduleCalendar #daydate').text().trim();

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Calendar", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {


                if (criteria.match(dayrgx) && criteria.length < 15) {
                    $('#pnlScheduleCalendar #btnmnth').removeClass("active");
                    $('#pnlScheduleCalendar #btnweek').removeClass("active");
                    $('#pnlScheduleCalendar #btnday').addClass("active");

                    $("#pnlScheduleCalendar #proresheader").css("display", "");
                    $("#pnlScheduleCalendar #MonthCal").css("display", "none");
                    $("#pnlScheduleCalendar #WeekCal").css("display", "none");

                    var d = new Date(criteria);
                    var newdate = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                    if (newdate == 'Invalid Date') {
                        newdate = $.datepicker.formatDate('mm/dd/yy', new Date());
                    }
                    var curdte = $.datepicker.formatDate('yy/mm/dd', newdate);
                    var statusslots = Scheduling_Calendar.FilterCriteria();

                    if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked"))
                        Scheduling_Calendar.DayCalendar($('#pnlScheduleCalendar #Provider').val(), 0, $('#pnlScheduleCalendar #Facility').val(), curdte.trim(), statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
                    else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked"))
                        Scheduling_Calendar.DayCalendar(0, $('#pnlScheduleCalendar #Resource').val(), $('#pnlScheduleCalendar #Facility').val(), curdte.trim(), statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSC').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
                    $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('mm/dd/yy', newdate));
                    $('#divDayName').removeAttr('style');
                    $('#pnlScheduleCalendar #divDayName').html($.datepicker.formatDate('DD', new Date(newdate)) + ", ");
                }
                else if (criteria.match(weekrg)) {
                    $('#pnlScheduleCalendar #btnmnth').removeClass("active");
                    $('#pnlScheduleCalendar #btnday').removeClass("active");
                    $('#pnlScheduleCalendar #btnweek').addClass("active");
                    var date = $('#pnlScheduleCalendar #daydate span').html();
                    // var criteria = $('#pnlScheduleCalendar #daydate').text().trim();

                    //=======================================================================
                    // Week Calendar Day settings in user entity options \\


                    var ObjDeferred = $.Deferred();
                    if (IsUpdate == true) {
                        //update call.
                        Scheduling_Calendar.DefaultSettingSave().done(function (response) {

                            if (response.status != false) {
                                ObjDeferred.resolve();
                            }
                            else {
                                //utility.DisplayMessages(response.Message, 3);
                                ObjDeferred.resolve();
                            }
                        });

                    }
                    else
                        ObjDeferred.resolve();

                    $.when(ObjDeferred).then(function () {

                        Scheduling_Calendar.FillDefaultSettings(globalAppdata.AppUserName).done(function (response) {

                            var date = $('#pnlScheduleCalendar #daydate span').html();
                            if (response.status == true) {

                                var EntityGroupSttings = JSON.parse(response.EntityGroupSttings_JSON);

                                if (EntityGroupSttings == "") {
                                    var s = "";
                                }
                                else {
                                    var s = EntityGroupSttings[0].SchedulePattern;
                                }
                                var match = s.split(',')
                                var x = 0;

                                if (s == "") {
                                    $('#pnlScheduleCalendar #chkcalMonday').prop('checked', true);
                                    $('#pnlScheduleCalendar #chkcalTuesday').prop('checked', true);
                                    $('#pnlScheduleCalendar #chkcalWednesday').prop('checked', true);
                                    $('#pnlScheduleCalendar #chkcalThursday').prop('checked', true);
                                    $('#pnlScheduleCalendar #chkcalFriday').prop('checked', true);
                                    $('#pnlScheduleCalendar #chkcalSaturday').prop('checked', true);
                                    $('#pnlScheduleCalendar #chkcalSunday').prop('checked', true);
                                }
                                else if (s != "") {
                                    for (var a in match) {
                                        var variable = match[a];

                                        if (x == 0 && variable == "1") {
                                            $('#pnlScheduleCalendar #chkcalMonday').prop('checked', true);
                                        }
                                        if (x == 0 && variable == "0") {
                                            $('#pnlScheduleCalendar #chkcalMonday').prop('checked', false);
                                        }
                                        if (x == 1 && variable == "1") {
                                            $('#pnlScheduleCalendar #chkcalTuesday').prop('checked', true);
                                        }
                                        if (x == 1 && variable == "0") {
                                            $('#pnlScheduleCalendar #chkcalTuesday').prop('checked', false);
                                        }
                                        if (x == 2 && variable == "1") {
                                            $('#pnlScheduleCalendar #chkcalWednesday').prop('checked', true);
                                        }
                                        if (x == 2 && variable == "0") {
                                            $('#pnlScheduleCalendar #chkcalWednesday').prop('checked', false);
                                        }
                                        if (x == 3 && variable == "1") {
                                            $('#pnlScheduleCalendar #chkcalThursday').prop('checked', true);
                                        }
                                        if (x == 3 && variable == "0") {
                                            $('#pnlScheduleCalendar #chkcalThursday').prop('checked', false);
                                        }
                                        if (x == 4 && variable == "1") {
                                            $('#pnlScheduleCalendar #chkcalFriday').prop('checked', true);
                                        }
                                        if (x == 4 && variable == "0") {
                                            $('#pnlScheduleCalendar #chkcalFriday').prop('checked', false);
                                        }
                                        if (x == 5 && variable == "1") {
                                            $('#pnlScheduleCalendar #chkcalSaturday').prop('checked', true);
                                        }
                                        if (x == 5 && variable == "0") {
                                            $('#pnlScheduleCalendar #chkcalSaturday').prop('checked', false);
                                        }
                                        if (x == 6 && variable == "1") {
                                            $('#pnlScheduleCalendar #chkcalSunday').prop('checked', true);
                                        }
                                        if (x == 6 && variable == "0") {
                                            $('#pnlScheduleCalendar #chkcalSunday').prop('checked', false);
                                        }
                                        x++;

                                    }
                                }
                                //-------------- Calendar Code --------------------

                                var checkeddays = '';

                                if ($('#pnlScheduleCalendar #chkcalMonday').is(':checked')) {

                                    checkeddays += '1';
                                }
                                else {
                                    checkeddays += '0';
                                }
                                if ($('#pnlScheduleCalendar #chkcalTuesday').is(':checked')) {

                                    checkeddays += '1';
                                }
                                else {
                                    checkeddays += '0';
                                } if ($('#pnlScheduleCalendar #chkcalWednesday').is(':checked')) {

                                    checkeddays += '1';
                                }
                                else {
                                    checkeddays += '0';
                                } if ($('#pnlScheduleCalendar #chkcalThursday').is(':checked')) {

                                    checkeddays += '1';
                                }
                                else {
                                    checkeddays += '0';
                                } if ($('#pnlScheduleCalendar #chkcalFriday').is(':checked')) {

                                    checkeddays += '1';
                                }
                                else {
                                    checkeddays += '0';
                                } if ($('#pnlScheduleCalendar #chkcalSaturday').is(':checked')) {

                                    checkeddays += '1';
                                }
                                else {
                                    checkeddays += '0';
                                } if ($('#pnlScheduleCalendar #chkcalSunday').is(':checked')) {

                                    checkeddays += '1';
                                }
                                else {
                                    checkeddays += '0';
                                }

                                var DatesString = '';

                                var startdate = new Date(date.substr(0, 10));
                                var enddate = new Date(date.substr(13, 10));
                                var j = 0;
                                for (var i = 0; i < 7; i++) {
                                    var d = new Date(startdate);
                                    var newdate = Scheduling_Calendar.AddSubDays(d, j);
                                    var passingdate = $.datepicker.formatDate('yy/mm/dd', newdate);
                                    var date = new Date(newdate);
                                    var weekdays = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];

                                    var weekday = weekdays[date.getDay()];
                                    if (weekday == "Sunday") {
                                        if ($('#pnlScheduleCalendar #chkcalSunday').is(':checked')) {
                                            var str = weekday + ',' + passingdate + '|';
                                            DatesString += str;
                                        }
                                    }
                                    if (weekday == "Monday") {
                                        if ($('#pnlScheduleCalendar #chkcalMonday').is(':checked')) {
                                            var str = weekday + ',' + passingdate + '|';
                                            DatesString += str;
                                        }
                                    }
                                    if (weekday == "Tuesday") {
                                        if ($('#pnlScheduleCalendar #chkcalTuesday').is(':checked')) {
                                            var str = weekday + ',' + passingdate + '|';
                                            DatesString += str;
                                        }
                                    }
                                    if (weekday == "Wednesday") {
                                        if ($('#pnlScheduleCalendar #chkcalWednesday').is(':checked')) {
                                            var str = weekday + ',' + passingdate + '|';
                                            DatesString += str;
                                        }
                                    }
                                    if (weekday == "Thursday") {
                                        if ($('#pnlScheduleCalendar #chkcalThursday').is(':checked')) {
                                            var str = weekday + ',' + passingdate + '|';
                                            DatesString += str;
                                        }
                                    }
                                    if (weekday == "Friday") {

                                        if ($('#pnlScheduleCalendar #chkcalFriday').is(':checked')) {
                                            var str = weekday + ',' + passingdate + '|';
                                            DatesString += str;
                                        }
                                    }
                                    if (weekday == "Saturday") {
                                        if ($('#pnlScheduleCalendar #chkcalSaturday').is(':checked')) {
                                            var str = weekday + ',' + passingdate + '|';
                                            DatesString += str;
                                        }
                                    }
                                    j++;
                                }
                                var statusslots = Scheduling_Calendar.FilterCriteria();
                                if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked"))
                                    Scheduling_Calendar.WeekCalendar($('#pnlScheduleCalendar #Provider').val(), 0, $('#pnlScheduleCalendar #Facility').val(), checkeddays, DatesString, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
                                else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked"))
                                    Scheduling_Calendar.WeekCalendar(0, $('#pnlScheduleCalendar #Resource').val(), $('#pnlScheduleCalendar #Facility').val(), checkeddays, DatesString, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());

                                //-------------- End Calendar Code ------------------
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }

                        });

                    });

                    //Scheduling_Calendar.DefaultSettingFill();

                    //========================================================================


                }
                else if (criteria.match(monthreg)) {


                    $('#pnlScheduleCalendar #btnweek').removeClass("active");
                    $('#pnlScheduleCalendar #btnday').removeClass("active");
                    $('#pnlScheduleCalendar #btnmnth').addClass("active");
                    //var a = $.datepicker.formatDate('yy/mm/dd', new Date(criteria));
                    //var month = $.datepicker.formatDate('mm/yy', new Date(a));
                    //Scheduling_Calendar.ClearTable();
                    //var statusslots = Scheduling_Calendar.FilterCriteria();
                    //Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, statusslots);

                    //$('#pnlScheduleCalendar #calmonth').val($('#pnlScheduleCalendar #daydate span').html());
                    //$('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', new Date(a)));
                    //calvalue = $('#pnlScheduleCalendar #calmonth').val();
                    //var d = new Date(criteria);
                    //var newdate = Scheduling_Calendar.AddSubMonth(d, 1);
                    //$('#daydate span').html($.datepicker.formatDate('MM/yy', newdate));
                    var a = $.datepicker.formatDate('mm/dd/yy', new Date(Scheduling_Calendar.FormatDate(criteria)));
                    var month = $.datepicker.formatDate('mm/yy', new Date(a));
                    Scheduling_Calendar.ClearTable();
                    var statusslots = Scheduling_Calendar.FilterCriteria();


                    if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked"))
                        Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), 0, month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
                    else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked"))
                        Scheduling_Calendar.MonthCalendar(0, $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());


                    $('#pnlScheduleCalendar #daydate span').html($.datepicker.formatDate('MM/yy', new Date(a)));
                    $('#pnlScheduleCalendar #calmonth').val($('#pnlScheduleCalendar #daydate span').html());
                    calvalue = $('#pnlScheduleCalendar #calmonth').val();
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });


    },

    AddSubDays: function (theDate, days) {
        //return new Date(theDate.getTime() + days * 24 * 60 * 60 * 1000);
        return new Date(theDate.setDate(theDate.getDate() + parseInt(days)));
    },

    AddSubMonth: function (theDate, month) {

        return new Date(theDate.setMonth(theDate.getMonth() + month));

    },

    ClearTable: function () {
        $("#pnlScheduleCalendar #myTable").empty();
        $("#pnlScheduleCalendar #monthTable").empty();
        $("#pnlScheduleCalendar #weekTable1").empty();
        $("#pnlScheduleCalendar #weekTable2").empty();
        $("#pnlScheduleCalendar #weekTable3").empty();
        $("#pnlScheduleCalendar #weekTable4").empty();
        $("#pnlScheduleCalendar #weekTable5").empty();
        $("#pnlScheduleCalendar #weekTable6").empty();
        $("#pnlScheduleCalendar #weekTable7").empty();
        $('#weekcontainer').empty();
    },

    BlockSlots: function () {
        var params = [];
        params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
        params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();



        var selected = [];
        var slotids;
        $('#myTable input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('id').replace('checkbox', ''));
            }
        });

        for (var w = 0; w <= selected.length; w++) {
            slotids += selected[w] + ',';
        }
        slotids = slotids.replace('undefined,', '');
        slotids = slotids.replace('undefined', '');
        if (slotids != "") {
            params["ParentCtrl"] = "schTabCalendar";
            params["slotids"] = slotids;
            params["Status"] = "Blocked";
            LoadActionPan('blckreasonDetail', params);


        }
        else {
            utility.DisplayMessages("Please Select a slot.", 3);
        }
    },

    UnBlockSlots: function () {
        var params = [];
        params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
        params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();

        var selected = [];
        var slotids;
        $('#myTable input[type=checkbox]').each(function () {
            if ($(this).is(":checked")) {
                selected.push($(this).attr('id').replace('checkbox', ''));
            }
        });

        for (var w = 0; w <= selected.length; w++) {
            slotids += selected[w] + ',';
        }
        slotids = slotids.replace('undefined,', '');
        slotids = slotids.replace('undefined', '');
        if (slotids != "") {
            params["ParentCtrl"] = "schTabCalendar";
            params["slotids"] = slotids;
            params["Status"] = "UnBlocked";
            LoadActionPan('blckreasonDetail', params);
        }
        else {
            utility.DisplayMessages("Please Select a slot.", 3);
        }
    },
    getAge: function (dateString) {
        var today = new Date();

        var birthDate = dateString;//new Date(tempDate);
        /**/

        var age = today.getFullYear() - birthDate.getFullYear();
        var m = today.getMonth() - birthDate.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        return age;
    },
    PatientDetails: function (responsePatients, responseAppDtl) {



        var resultPat = responsePatients.split('|');

        var resultApp = responseAppDtl.split('|');
        if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
            var provName = $('#pnlScheduleCalendar #Provider option:selected').text();
        } else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
            var provName = $('#pnlScheduleCalendar #Resource option:selected').text();
        }


        if (resultApp.length == resultPat.length) {



            for (var i = 0; i < resultPat.length - 1; i++) {

                var patientDtl = resultPat[i].split(',');


                var fullDate = new Date(patientDtl[2].replace(/\-/g, "/"));

                var DOB = fullDate.toLocaleDateString();



                var appDtl = resultApp[i].split(',');

                var patientname = appDtl[2].replace('-', ', ').replace(/#@#/g, ',');

                var ctrl = appDtl[1] + '-' + appDtl[0]

                var age = Scheduling_Calendar.getAge(fullDate);
                var appcopay = appDtl[6];
                var appschreason = appDtl[5];
                appschreason = appschreason.replace(/#@#/g, ',');
                appcopay = appcopay.split('-');
                var appcopayamtpaid = 0;
                if (appcopay.length == 3) {
                    appcopayamtpaid = -Math.abs(parseFloat(appcopay[2]));
                } else {
                    appcopayamtpaid = parseFloat(appcopay[1]);
                }


                var reminder_delivery_status = patientDtl[13] == (undefined) ? "" : patientDtl[13];
                var reminder_delivery_response = patientDtl[14] == (undefined) ? "" : patientDtl[14];
                var reminder_delivery_response_message = patientDtl[16] == (undefined) ? "" : patientDtl[16];
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
                    status_style_color = 'style = "color:green"';
                }

                var facName = "";
                if (patientDtl[15] == undefined || patientDtl[15] == "undefined") {
                    facName = patientDtl[13];
                } else {
                    facName = patientDtl[15];
                }

                popover = '<h5 class="pull-left m-none pr-xxs">' + patientname + '</h5> <span>' + age + ' Y, ' + patientDtl[4] + '; '
                               + '<b>Account:</b> ' + patientDtl[1] + '; '
                               + '<b>DOB: </b> ' + patientDtl[2].replace(/\-/g, "/") + '</span>'
                               + '<hr class="stooltip " >'

                               + '<ul class="list-unstyled pl-none">'

                               + '<li><b>Mobile:</b> ' + patientDtl[3] + '</li>'
                               + '<li><b>Reason:</b> ' + appschreason + '</li>'
                               + '<li><b>Primary Plan:</b> ' + patientDtl[12] + '</li>'
                               + '<li><b>Provider/Resource:</b> ' + provName + '</li>'
                               + '<li><b>Copay:</b> ' + appcopayamtpaid + '</li>'
                               + '<li><b>Status:</b> ' + appDtl[3] + '</li>'
                               + '<li><b>Reminder Delivery Status:</b> ' + reminder_delivery_status + '</li>'
                               + '<li><b>Reminder Response:</b> <span ' + status_style_color + '>' + reminder_delivery_response + '</span></li>'
                               + '<li><b>Time:</b> ' + patientDtl[5] + '(Minutes)</li>'
                               + '<li><b>Patient Type:</b> ' + appDtl[17] + '</li>'
                               + '<li><b>Visit Type:</b> ' + appDtl[18] + '</li>'
                               + '<li><b>Comments:</b> ' + appDtl[8] + '</li>'
                               + '<li><b>Created by:</b> ' + patientDtl[6].replace('_', ', ') + '</li>'
                               + '<li><b>Created on:</b> ' + patientDtl[7] + '</li>'
                               + '<li><b>Last Modified by:</b> ' + patientDtl[8].replace('_', ', ') + '</li>'
                               + '<li><b>Last Modified on:</b> ' + patientDtl[9] + '</li>'

                               + '<li><b>E.mail:</b>' + patientDtl[10] + '</li>'
                               + '<li><b>Facility:</b>' + facName + '</li>';
                +'</ul>'

                //if (PatientInsurance_Detail == []) {

                //    + 'Primary Plan: ' + PatientInsurance_Detail[0].InsurancePlanName + '<br>'
                //    + 'Copay: ' + PatientInsurance_Detail[0].AmtCopay + '<br>'

                //}
                //else {
                //    + 'Primary Plan: No Plan Found<br>'
                //}
                $('#' + ctrl).tooltipster({
                    theme: 'tooltipster-shadow',
                    content: $(popover),
                    functionReady: function (instance, helper) {
                        var posTop = $(helper)[0].getBoundingClientRect().top;
                        var anchorBottom = $(this)[0].getBoundingClientRect().bottom;
                        if (posTop < 0) {
                            $('.tooltipster-base').css("top", (anchorBottom + 13) + "px");
                            $('.tooltipster-arrow').removeClass("tooltipster-arrow-top").addClass("tooltipster-arrow-bottom");
                        }
                    }

                });

            }

        }


    },
    PatientDetail: function (patientid, appointmentid, ctrl) {

        Scheduling_Calendar.GetPatientAppDetail(patientid, appointmentid).done(function (response) {
            var popover;
            if (response.status != false) {
                var Patient_Detail = JSON.parse(response.PatientDetail_JSON);
                var PatientInsurance_Detail = JSON.parse(response.PatientInsuranceDetail_JSON);
                var PatientAppointment_Detail = JSON.parse(response.PatientAppointment_JSON);
                var age = Scheduling_Calendar.getAge(Patient_Detail[0].DOB.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, ''));
                popover = '<h5>' + Patient_Detail[0].LastName + ', ' + Patient_Detail[0].FirstName + '</h5> ' + age + ' Y, ' + Patient_Detail[0].Gender + '<br>'
                               + 'Account:' + Patient_Detail[0].AccountNumber + '<br>'
                               + 'DOB: ' + Patient_Detail[0].DOB.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + ''
                               + '<hr class="stooltip " >'
                               + 'Cell: ' + Patient_Detail[0].CellNo + '<br>'
                               + 'Reason: ' + PatientAppointment_Detail[0].Reason + '<br>'
                               + 'Provider: ' + PatientAppointment_Detail[0].ProviderName + ' <br>'

                               + 'Status: ' + PatientAppointment_Detail[0].Status + '<br>'
                               + 'Time: ' + PatientAppointment_Detail[0].Duration + '(Minutes)<br>'
                               + 'Patient Type: ' + PatientAppointment_Detail[0].PatientType + '<br>'
                               + 'Visit Type: ' + PatientAppointment_Detail[0].VisitType + '<br>'
                               + 'Comments: ' + PatientAppointment_Detail[0].Comments + '<br>'
                               + 'Created by: ' + Patient_Detail[0].CreatedBy.split('_', ', ') + '<br>'
                               + 'Created on: ' + Patient_Detail[0].CreatedOn + '<br>'
                               + 'Last Modified by: ' + Patient_Detail[0].ModifiedBy.split('_', ', ') + '<br>'
                               + 'Last Modified on: ' + Patient_Detail[0].ModifiedOn + '<br>'

                               + '<p>E.mail:' + Patient_Detail[0].EmailAddress + '</p>';


                if (PatientInsurance_Detail == []) {

                    + 'Primary Plan: ' + PatientInsurance_Detail[0].InsurancePlanName + '<br>'
                    + 'Copay: ' + PatientInsurance_Detail[0].AmtCopay + '<br>'

                }
                else {
                    + 'Primary Plan: No Plan Found<br>'
                }

            }
            $('#' + ctrl).tooltipster({
                theme: 'tooltipster-shadow',
                content: $(popover),
                functionReady: function (instance, helper) {
                    var posTop = $(helper)[0].getBoundingClientRect().top;
                    var anchorBottom = $(this)[0].getBoundingClientRect().bottom;
                    if (posTop < 0) {
                        $('.tooltipster-base').css("top", (anchorBottom + 13) + "px");
                        $('.tooltipster-arrow').removeClass("tooltipster-arrow-top").addClass("tooltipster-arrow-bottom");
                    }
                }

            });
        });

    },

    LoadSchedulingDetail: function () {

        var DefaultProviderId = globalAppdata['DefaultProviderId'];
        var DefaultFacilityId = globalAppdata['DefaultFacilityId'];
        var DefaultResourceId = globalAppdata['DefaultResourceId'];
        var pexist = 0;
        var fexist = 0;
        var rexist = 0;
        $('#Provider option').each(function () {
            if (this.value == DefaultProviderId) {
                pexist = 1;
            }
        });
        $('#Facility option').each(function () {
            if (this.value == DefaultFacilityId) {
                fexist = 1;
            }
        });
        $('#Resource option').each(function () {
            if (this.value == DefaultResourceId) {
                rexist = 1;
            }
        });

        if (pexist == 1)
            $('#pnlScheduleCalendar #Provider').val(globalAppdata['DefaultProviderId']);
        else
            $("#pnlScheduleCalendar #Provider option[value='']").attr('selected', true);

        if (fexist == 1)
            $('#pnlScheduleCalendar #Facility').val(globalAppdata['DefaultFacilityId']);
        else
            $("#pnlScheduleCalendar #Facility option[value='']").attr('selected', true);

        if (rexist == 1)
            $('#pnlScheduleCalendar #Resource').val(globalAppdata['DefaultResourceId']);
        else
            $("#pnlScheduleCalendar #Resource option[value='']").attr('selected', true);
        //to hide show label and links of dropdowns
        if ($('#pnlScheduleCalendar #Provider').val()) {
            $("#pnlScheduleCalendar #lnkProviderEdit").css("display", "inline");
            $("#pnlScheduleCalendar #lblProvider").css("display", "none");
        } else {
            $("#pnlScheduleCalendar #lblProvider").css("display", "inline");
            $("#pnlScheduleCalendar #lnkProviderEdit").css("display", "none");
        }
        if ($('#pnlScheduleCalendar #Facility').val()) {
            $("#pnlScheduleCalendar #lnkFacilityEdit").css("display", "inline");
            $("#pnlScheduleCalendar #lblFacility").css("display", "none");
        } else {
            $("#pnlScheduleCalendar #lblFacility").css("display", "inline");
            $("#pnlScheduleCalendar #lnkFacilityEdit").css("display", "none");
        }
        if ($('#pnlScheduleCalendar #Resource').val()) {
            $("#pnlScheduleCalendar #lnkResourceEdit").css("display", "inline");
            $("#pnlScheduleCalendar #lblResource").css("display", "none");
        } else {
            $("#pnlScheduleCalendar #lblResource").css("display", "inline");
            $("#pnlScheduleCalendar #lnkResourceEdit").css("display", "none");
        }
        //
    },

    SearchDaySlotSchedule: function (ProviderID, ResourceID, FacilityID, SlotDate, StatusId, PatientTypeId, VisitTypeId) {
        var data = "ProviderID=" + ProviderID + "&ResourceID=" + ResourceID + "&FacilityID=" + FacilityID + "&SlotDate=" + SlotDate + "&StatusId=" + StatusId + "&PatientTypeId=" + PatientTypeId + "&VisitTypeId=" + VisitTypeId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "SEARCH_PROVIDERSCHEDULE");
    },

    MoveAppointment: function (AppointmentID, TimeSlotDtlId) {
        //  var strMessage = "";
        //  AppPrivileges.GetFormPrivileges("Appointment", "UPDATE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //     if (strMessage == "") {

        var data = "AppointmentID=" + AppointmentID + "&TimeSlotDtlId=" + TimeSlotDtlId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "MOVE_APPOINTMENT");

        //   }
        //   else
        //        utility.DisplayMessages(strMessage, 2);
        //  });

    },

    UpdateAppointmentStatus: function (AppointmentID, SchStatusID, CancelReason) {

        if (CancelReason != null) {
            var object = new Object();
            object["CancelReason"] = CancelReason;
            var stream = JSON.stringify(object);
            var data = "AppointmentID=" + AppointmentID + "&SchStatusID=" + SchStatusID + "&AppointmentData=" + stream;
        }
        else {
            var data = "AppointmentID=" + AppointmentID + "&SchStatusID=" + SchStatusID;
        }
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "UPDATE_APPOINTMENT_STATUS");


    },

    SelectMonthlyAppointment: function (ProviderID, FacilityID, ResourceID, MonthYear, StatusId, PatientTypeId, VisitTypeId) {
        var data = "ProviderID=" + ProviderID + "&FacilityID=" + FacilityID + "&ResourceID=" + ResourceID + "&MonthYear=" + MonthYear + "&StatusId=" + StatusId + "&PatientTypeId=" + PatientTypeId + "&VisitTypeId=" + VisitTypeId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "SELECT_MONTH_APPOINTMENT");
    },

    SelectWeeklyAppointment: function (ProviderID, FacilityID, ResourceID, StartDate, EndDate, DaysOfWeek) {
        var data = "ProviderID=" + ProviderID + "&FacilityID=" + FacilityID + "&ResourceID=" + ResourceID + "&StartDate=" + StartDate + "&EndDate=" + EndDate + "&DaysOfWeek=" + DaysOfWeek;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "SELECT_WEEKLY_APPOINTMENT");
    },

    GetPatientAppDetail: function (PatientID, AppointmentID) {
        var data = "PatientID=" + PatientID + "&AppointmentID=" + AppointmentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "GET_PATIENT_DETAILS");
    },

    GetAppointmentStatuses: function () {
        var data = "";
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "SELECT_APPOINTMENT_STATUSES");
    },

    SelectWeeklySlotAppointment: function (ProviderID, ResourceID, FacilityID, checkeddays, DatesString, StatusId, PatientTypeId, VisitTypeId) {
        var data = "ProviderID=" + ProviderID + "&ResourceID=" + ResourceID + "&FacilityID=" + FacilityID + "&checkeddays=" + checkeddays + "&DatesString=" + DatesString + "&StatusId=" + StatusId + "&PatientTypeId=" + PatientTypeId + "&VisitTypeId=" + VisitTypeId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "SELECT_WEEKLY_SLOT_APP");
    },

    //-------Open Provider/Resource/Facility------
    OpenProviderDetail: function () {
        //Admin_Provider.ProviderEdit($('#' + demographicDetail.params.PanelID + ' #hfProvider').val(), "demographicDetail");
        var params = [];
        params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "Provider";
        params["ParentCtrl"] = 'schTabCalendar';
        LoadActionPan('providerDetail', params);
    },

    HideShowProviderLink: function () {

        if ($('#pnlScheduleCalendar #Provider').val() != "") {

            if ($("#pnlScheduleCalendar #lnkProviderEdit").css("display") == "none") {
                $("#pnlScheduleCalendar #lnkProviderEdit").css("display", "inline");
                $("#pnlScheduleCalendar #lblProvider").css("display", "none");
            }
            $('#pnlScheduleCalendar #btnForceBooking').removeAttr("disabled");

        }
        else {
            $("#pnlScheduleCalendar #lnkProviderEdit").css("display", "none");
            $("#pnlScheduleCalendar #lblProvider").css("display", "inline");
            $('#pnlScheduleCalendar #btnForceBooking').attr("disabled", "disabled");
        }
    },

    OpenFacilityDetail: function () {
        var statusslots = Scheduling_Calendar.FilterCriteria();
        var params = [];
        params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
        params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        params["statusslots"] = statusslots;
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "Facility";
        params["ParentCtrl"] = 'schTabCalendar';
        LoadActionPan('facilityDetail', params);
    },

    HideShowFacilityLink: function () {
        if ($('#pnlScheduleCalendar #Facility').val() != "") {

            if ($("#pnlScheduleCalendar #lnkFacilityEdit").css("display") == "none") {
                $("#pnlScheduleCalendar #lnkFacilityEdit").css("display", "inline");
                $("#pnlScheduleCalendar #lblFacility").css("display", "none");
            }
        }
        else {
            $("#pnlScheduleCalendar #lnkFacilityEdit").css("display", "none");
            $("#pnlScheduleCalendar #lblFacility").css("display", "inline");
        }
    },

    HideShowAndLoadVisitType: function (control) {
        $("#pnlScheduleCalendar #ddlVisitTypeSc option:selected").prop("selected", false);
        if ($('#pnlScheduleCalendar #ddlPatientTypeSc').val() != "") {
            $("#pnlScheduleCalendar #ddlVisitTypeSc").prop("disabled", false);
        }
        else {
            $("#pnlScheduleCalendar #ddlVisitTypeSc").prop("disabled", true);
        }
        var selectedPateintType = $(control).val();
        $("#pnlScheduleCalendar #ddlPatientTypeSc option").each(function (i, item) {
            if (item.value == selectedPateintType) {
                Scheduling_Calendar.ShowHideVisitTypeOptions(item.value, true);
            }
            else if (item.value == "") {
                Scheduling_Calendar.ShowHideVisitTypeOptions('', true);
            }
            else
                Scheduling_Calendar.ShowHideVisitTypeOptions(item.value, false);
        });

    },
    ShowHideVisitTypeOptions: function (value, canShowOption) {
        $('#pnlScheduleCalendar #ddlVisitTypeSc').find('option[refvalue="' + value + '"]').map(function () {
            return $(this).parent('span').length === 0 ? this : null;
        }).wrap('<span>').hide();

        if (canShowOption) {
            $('#pnlScheduleCalendar #ddlVisitTypeSc').find('option[refvalue="' + value + '"]').unwrap().show();
        }
        else
            $('#pnlScheduleCalendar #ddlVisitTypeSc').find('option[refvalue="' + value + '"]').hide();

    },

    OpenResourceDetail: function () {
        var params = [];
        params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "Resource";
        params["ParentCtrl"] = 'schTabCalendar';
        LoadActionPan('resourcesDetail', params);
    },

    HideShowResourceLink: function () {
        if ($('#pnlScheduleCalendar #Resource').val() != "") {

            if ($("#pnlScheduleCalendar #lnkResourceEdit").css("display") == "none") {
                $("#pnlScheduleCalendar #lnkResourceEdit").css("display", "inline");
                $("#pnlScheduleCalendar #lblResource").css("display", "none");
            }
        }
        else {
            $("#pnlScheduleCalendar #lnkResourceEdit").css("display", "none");
            $("#pnlScheduleCalendar #lblResource").css("display", "inline");
        }
    },

    HideShowDropDownLink: function () {
        //------Provider--------------
        var DefaultProviderId = globalAppdata['DefaultProviderId'];
        var pexist = 0;
        $('#pnlScheduleCalendar #Provider option').each(function () {
            if (this.value == DefaultProviderId && DefaultProviderId != "") {
                pexist = 1;
            }
        });

        if (pexist == 1) {
            if ($("#pnlScheduleCalendar #lnkProviderEdit").css("display") == "none") {
                $("#pnlScheduleCalendar #lnkProviderEdit").css("display", "inline");
                $("#pnlScheduleCalendar #lblProvider").css("display", "none");
            }
        }
        else {
            $("#pnlScheduleCalendar #lnkProviderEdit").css("display", "none");
            $("#pnlScheduleCalendar #lblProvider").css("display", "inline");
        }
        //-------- End Provider------------

        //-------- Facility----------------
        var DefaultFacilityId = globalAppdata['DefaultFacilityId'];
        var fexist = 0;

        $('#pnlScheduleCalendar #Facility option').each(function () {
            if (this.value == DefaultFacilityId && DefaultFacilityId != "") {
                fexist = 1;
            }
        });

        if (fexist == 1) {
            if ($("#pnlScheduleCalendar #lnkFacilityEdit").css("display") == "none") {
                $("#pnlScheduleCalendar #lnkFacilityEdit").css("display", "inline");
                $("#pnlScheduleCalendar #lblFacility").css("display", "none");
            }
        }
        else {
            $("#pnlScheduleCalendar #lnkFacilityEdit").css("display", "none");
            $("#pnlScheduleCalendar #lblFacility").css("display", "inline");
        }

        //-------- End Facility-------------

        //-------- Resource-----------------
        var DefaultResourceId = globalAppdata['DefaultResourceId'];
        var rexist = 0;
        $('#pnlScheduleCalendar #Resource option').each(function () {
            if (this.value == DefaultResourceId && DefaultResourceId != "") {
                rexist = 1;
            }
        });

        if (rexist == 1) {
            if ($("#pnlScheduleCalendar #lnkResourceEdit").css("display") == "none") {
                $("#pnlScheduleCalendar #lnkResourceEdit").css("display", "inline");
                $("#pnlScheduleCalendar #lblResource").css("display", "none");
            }
        }
        else {
            $("#pnlScheduleCalendar #lnkResourceEdit").css("display", "none");
            $("#pnlScheduleCalendar #lblResource").css("display", "inline");
        }

        //-------- End Resource-------------
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
                        if (Scheduling_Calendar.params.PanelID == "pnlScheduleCalendar") {
                            params["ParentCtrl"] = 'schTabCalendar';
                        }
                        else {
                            params["ParentCtrl"] = 'schTabMultipleView';
                        }
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

    FormatDate: function (date) {

        var obj = { January: 1, February: 2, March: 3, April: 4, May: 5, June: 6, July: 7, August: 8, September: 9, October: 10, November: 11, December: 12 };
        var temp = date.split('/');


        //alert(date + " :" + obj[temp[0]] + "/1/" + "/" + temp[1]);
        return obj[temp[0]] + "/1" + "/" + temp[1];
    },

    // Week Calendar Day settings in user entity options \\

    FillDefaultSettings: function (userName) {
        var data = "userName=" + userName;
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "FILL_DEFAULT_SETTINGS");
    },


    DefaultSettingSave: function () {

        var self = $("#pnlScheduleCalendar #checkboxpanel");
        var myJSON = self.getMyJSON();
        if (globalAppdata['EntityUserOptionId'] == "" || globalAppdata['EntityUserOptionId'] == undefined) {

            return Scheduling_Calendar.SaveDefaultSetting(myJSON);
        }
        else if (globalAppdata['EntityUserOptionId'] != "" || globalAppdata['EntityUserOptionId'] != undefined) {

            return Scheduling_Calendar.UpdateDefaultSetting(myJSON, globalAppdata['EntityUserOptionId']);

        }
        else
            return false;

    },


    UpdateDefaultSetting: function (DefaultSettingData, EntityUserOptionId) {
        var data = "DefaultSettingData=" + DefaultSettingData + "&EntityUserOptionId=" + EntityUserOptionId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "UPDATE_DEFAULT_SETTING");
    },

    SaveDefaultSetting: function (DefaultSettingData) {
        var data = "DefaultSettingData=" + DefaultSettingData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "SAVE_DEFAULT_SETTING");
    },

    LoadClinicalNote: function (resultPat, AppointmentId, PatientId, AppTime, VisitId, AppReason, RefProviderName, RefProviderId, isNoteCreated, noteid, status, provid, providerName, VisitTypeId, PatientTypeId) {


        //params["QuickAddPatient"] = true;
        //SelectTab('mstrTabPatient', 'false');
        //setTimeout(function () { SelectPatient(PatientId, ""); }, 10);
        //setTimeout(function () { SelectTab('mstrTabClinical', 'false'); }, 200);


        //var paramsNotes = [];
        //paramsNotes["AppointmentId"] = AppointmentId;
        //paramsNotes["PatientId"] = PatientId;
        //paramsNotes["NotesVisitTime"] = AppTime;
        //paramsNotes["NotesVisitId"] = VisitId;
        //paramsNotes["NotesVisitDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        //paramsNotes["NotesFacilityId"] = $('#pnlScheduleCalendar #Facility').val();
        //paramsNotes["NotesProviderId"] = $('#pnlScheduleCalendar #Provider').val();

        //paramsNotes["NotesFacilityName"] = $('#pnlScheduleCalendar #Facility option:selected').text();
        //paramsNotes["NotesProviderName"] = $('#pnlScheduleCalendar #Provider option:selected').text();

        //paramsNotes["RefProviderName"] = RefProviderName;
        //paramsNotes["RefProviderId"] = RefProviderId;

        //paramsNotes["ScheduleReason"] = Reason;
        //setTimeout(function () { ClinicalMenuSettings.TopButtons('clinicalMenuNotes', paramsNotes); }, 250);
        //      Clinical_ProgressNote.params.AppointmentVisitId = VisitId;


        //------------------------------------
        var appFacName = "";
        var appFacId = "";
        var _resultPat = resultPat.split('|');
        var patientAppDtl = _resultPat[0].split(',');
        appFacName = patientAppDtl[patientAppDtl.length - 1];
        $('#pnlScheduleCalendar #Facility option').each(function () {
            if ($(this).text() == appFacName.trim()) {
                appFacId = $(this).val();
            }
        });
        //------------------------------------

        Clinical_Notes.params.NotesId = null;
        if (status != 'true') {

            if (isNoteCreated == "true") {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        params["QuickAddPatient"] = true;

                        var AppointmentTime = AppTime;
                        var VisitDate = $('#pnlScheduleCalendar #daydate').text().trim();
                        var FacilityId = appFacId;//$('#pnlScheduleCalendar #Facility').val();
                        var ProviderId = $('#pnlScheduleCalendar #Provider').val();
                        if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                            var ResourceId = $('#pnlScheduleCalendar #Resource').val();
                            var ResourceName = $('#pnlScheduleCalendar #Resource option:selected').text();
                            if ($('#pnlScheduleCalendar #Resource option:selected').attr('refname') != "-") {
                                var ResourceproviderName = $('#pnlScheduleCalendar #Resource option:selected').attr('refname').split('-')[1]
                                var ResourceproviderId = $('#pnlScheduleCalendar #Resource option:selected').attr('refname').split('-')[0]
                            } else {
                                var ResourceproviderName = null;
                                var ResourceproviderId = null;
                            }
                        } else {
                            var ResourceId = null;
                            var ResourceName = null;
                        }
                        var ParentCntrlLoadid = "Schedular"
                        var FacilityName = appFacName;//$('#pnlScheduleCalendar #Facility option:selected').text();
                        var ProviderName = $('#pnlScheduleCalendar #Provider option:selected').text();
                        if (($('#pnlScheduleCalendar #rdresource').is(':checked'))) {
                            ProviderId = provid;
                            ProviderName = providerName;
                        }
                        var RefProviderName = (RefProviderName == "undefined" || RefProviderName == null) ? null : RefProviderName;
                        var RefProviderId = (RefProviderId == "undefined" || RefProviderId == null) ? null : RefProviderId;
                        var Reason = (AppReason == "undefined" || AppReason == null) ? null : AppReason;
                        var ForProgressNote = false;
                        var NotesId = noteid != null ? noteid : '';
                        var Room = "";
                        EMRUtility.CreateNote("Edit", PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName,
                            FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, ResourceId, ResourceName, ResourceproviderId, ResourceproviderName, VisitTypeId, PatientTypeId);

                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
            else {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        params["QuickAddPatient"] = true;

                        var AppointmentTime = AppTime;
                        var VisitDate = $('#pnlScheduleCalendar #daydate').text().trim();
                        var FacilityId = appFacId;//$('#pnlScheduleCalendar #Facility').val();
                        var ProviderId = $('#pnlScheduleCalendar #Provider').val();
                        if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                            var ResourceId = $('#pnlScheduleCalendar #Resource').val();
                            var ResourceName = $('#pnlScheduleCalendar #Resource option:selected').text();
                            if ($('#pnlScheduleCalendar #Resource option:selected').attr('refname') != "-") {
                                var ResourceproviderName = $('#pnlScheduleCalendar #Resource option:selected').attr('refname').split('-')[1]
                                var ResourceproviderId = $('#pnlScheduleCalendar #Resource option:selected').attr('refname').split('-')[0]
                            } else {
                                var ResourceproviderName = null;
                                var ResourceproviderId = null;
                            }
                        } else {
                            var ResourceId = null;
                            var ResourceName = null;
                        }
                        var ParentCntrlLoadid = "Schedular"
                        var FacilityName = appFacName;//$('#pnlScheduleCalendar #Facility option:selected').text();
                        var ProviderName = $('#pnlScheduleCalendar #Provider option:selected').text();
                        if (($('#pnlScheduleCalendar #rdresource').is(':checked'))) {
                            ProviderId = provid;
                            ProviderName = providerName;
                        }
                        var RefProviderName = (RefProviderName == "undefined" || RefProviderName == null) ? null : RefProviderName;
                        var RefProviderId = (RefProviderId == "undefined" || RefProviderId == null) ? null : RefProviderId;
                        var Reason = (AppReason == "undefined" || AppReason == null) ? null : AppReason;
                        var ForProgressNote = false;
                        var NotesId = '';
                        var Room = "";
                        EMRUtility.CreateNote("Add", PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName,
                            FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, ResourceId, ResourceName, ResourceproviderId, ResourceproviderName, VisitTypeId, PatientTypeId);
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
        } else {
            AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    params["QuickAddPatient"] = true;

                    var AppointmentTime = AppTime;
                    var VisitDate = $('#pnlScheduleCalendar #daydate').text().trim();
                    var FacilityId = appFacId;//$('#pnlScheduleCalendar #Facility').val();
                    var ProviderId = $('#pnlScheduleCalendar #Provider').val();
                    var ParentCntrlLoadid = "Schedular"
                    var FacilityName = appFacName;//$('#pnlScheduleCalendar #Facility option:selected').text();
                    var ProviderName = $('#pnlScheduleCalendar #Provider option:selected').text();
                    if (($('#pnlScheduleCalendar #rdresource').is(':checked'))) {
                        ProviderId = provid;
                        ProviderName = providerName;
                    }
                    var RefProviderName = (RefProviderName == "undefined" || RefProviderName == null) ? null : RefProviderName;
                    var RefProviderId = (RefProviderId == "undefined" || RefProviderId == null) ? null : RefProviderId;
                    var Reason = (AppReason == "undefined" || AppReason == null) ? null : AppReason;
                    var ForProgressNote = false;
                    var NotesId = noteid != null ? noteid : '';
                    var Room = "";
                    EMRUtility.CreateNote("View", PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName,
                        FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId, VisitTypeId, PatientTypeId);

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    viewClinicalNote: function (AppointmentId, PatientId, AppTime, VisitId, AppReason, RefProviderName, RefProviderId, isNoteCreated, noteid) {

        Clinical_Notes.params.NotesId = null;
        if (isNoteCreated == "true") {
            AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    params["QuickAddPatient"] = true;

                    var AppointmentTime = AppTime;
                    var VisitDate = $('#pnlScheduleCalendar #daydate').text().trim();
                    var FacilityId = $('#pnlScheduleCalendar #Facility').val();
                    var ProviderId = $('#pnlScheduleCalendar #Provider').val();
                    var ParentCntrlLoadid = "Schedular"
                    var FacilityName = $('#pnlScheduleCalendar #Facility option:selected').text();
                    var ProviderName = $('#pnlScheduleCalendar #Provider option:selected').text();
                    var RefProviderName = (RefProviderName == "undefined" || RefProviderName == null) ? null : RefProviderName;
                    var RefProviderId = (RefProviderId == "undefined" || RefProviderId == null) ? null : RefProviderId;
                    var Reason = (AppReason == "undefined" || AppReason == null) ? null : AppReason;
                    var ForProgressNote = false;
                    var NotesId = noteid != null ? noteid : '';
                    var Room = "";
                    EMRUtility.CreateNote("View", PatientId, AppointmentId, ProviderId, ProviderName, AppointmentTime, VisitId, VisitDate, Reason, FacilityName,
                        FacilityId, Room, NotesId, ForProgressNote, ParentCntrlLoadid, RefProviderName, RefProviderId);

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }


    },


    enableSlimScroll: function () {
        $('#pnlScheduleCalendar #sidebarWrapperInner').slimScroll({ height: '95%' });
    },

    // Begin Date January 04, 2015 Edit By Azam Aftab
    //Parent Dropdown(.dropdown-menu.multi-level) position
    parentMenuPosition: function (e) {
        e.stopPropagation();
        //to close all existing dropdown
        if ($(this).hasClass("open") != true) {

            var tableName = $(this).closest('table').attr('id');
            $(".buttonDrop.dropdown").removeClass("open");
            $(this).addClass("open");
            $("#" + tableName).find('.zIndexActive').removeClass('zIndexActive').addClass('zIndex');
            $(this).parent().removeClass("zIndex").addClass("zIndexActive");

            $(".zIndexActive").parent().css("z-index", "");

        }
        else {
            $(this).removeClass("open");
            $(this).parent().removeClass("zIndexActive").addClass("zIndex");

        }
        var menu = $(this).children("ul");
        var subMenu = menu.find('ul');
        if (subMenu.hasClass("hide") === false) {
            subMenu.addClass("hide");
        }
        var menuwidth = menu.outerWidth();
        var menuHeight = menu.outerHeight();

        var posTop = $(this)[0].getBoundingClientRect().top;
        var posBottom = $(window).height() - $(this)[0].getBoundingClientRect().bottom;

        //To calculate the height of header M.Azhar Hussain

        //check multiple View height
        var multiView = $("#frmSchedulingMuliView").outerHeight();
        if (multiView > 0) {
            //add margin-bottom
            multiView = multiView + 25;
        }

        var headerHeight = $('.header').outerHeight() + $('#pnlTab3 .tab-content').outerHeight();
        var posTopWithHeader = posTop - (headerHeight + multiView);
        //setting .dropdown-menu.multi-level position
        if (menuHeight < posBottom && menuHeight > posTopWithHeader) {
            //show on bottom
            menu.css("top", "");
        }
        else if (menuHeight > posBottom && menuHeight < posTopWithHeader) {
            //show on top
            menu.css("top", "-" + menuHeight + "px");
        }
        else if (menuHeight > posBottom && menuHeight > posTopWithHeader) {
            //show on bottom default possition
            //if grater on top and bottom
            var tableHeight = $(this).parents("#tablediv").height();
            var headingHeight = headerHeight + $("#StickyCalButton").outerHeight();
            if (tableHeight < menuHeight) {
                //ref 1.1
                $(this).parents("#tablediv").css('padding-bottom', menuHeight + 'px');
                //show on bottom
                menu.css("top", "");
            }
            else if (tableHeight > menuHeight && (scrollY - headingHeight) > menuHeight) {
                //show on top
                menu.css("top", "-" + menuHeight + "px");
            }
            else {
                //show on bottom
                menu.css("top", "");
            }

        }

    },
    LoadUnallocatedCopayment: function (appid, patientid, facid, provid, visitid) {

        var params = [];
        params["FromAdmin"] = "0";
        params["ProviderId"] = provid;
        params["ResourceId"] = $('#pnlScheduleCalendar #Resource').val();
        params["FacilityId"] = facid;
        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        params["AppointmentId"] = appid;
        params["PatientId"] = patientid;
        if (Scheduling_Calendar.params.PanelID == "pnlScheduleCalendar") {
            params["ParentCtrl"] = 'schTabCalendar';
        }
        else {
            params["ParentCtrl"] = 'schTabMultipleView';
        }
        params["VisitId"] = visitid ? visitid : 0;
        LoadActionPan('Scheduling_UnallocatedCopayment', params);
    },
    dropDownMenuClick: function (parentId) {
        $("#" + parentId + " .buttonDrop.dropdown").on("click", Scheduling_Calendar.parentMenuPosition);
    },

    subDropdownMenuMouseEnter: function () {
        $("#myid li.dropdown-submenu").on("mouseenter", function () {

            var ul = $(this).find("ul");
            ul.removeClass("hide");
            ul.slimScroll({ height: 'inherit', railVisible: true, railColor: '#222', railOpacity: 0.3 });

            var currentMenu = $(this).find(".slimScrollDiv");
            var posLeft = $(this).parent()[0].getBoundingClientRect().left;
            var posRight = $(window).width() - $(this).parent()[0].getBoundingClientRect().right;
            var menuPosLeft = "menuPosLeft";
            var menuPosRight = "menuPosRight";
            var menuPosBottom = "menuPosBottom";
            var resetMenuPos = menuPosLeft + " " + menuPosRight + " " + menuPosBottom;
            var width = currentMenu.width();

            var items = currentMenu.children('ul').children("li").length;
            var c1 = "columnN1";
            var c2 = "columnN2";
            var c3 = "columnN3";
            var c123 = c1 + " " + c2 + " " + c3;
            var isC123 = "." + c1 + ", ." + c2 + ", ." + c3;
            var initialClass = "dropdown-menu multi-column-dropdown";
            currentMenu.css("position", "absolute");

            if (items <= 7) {
                currentMenu.removeClass(c123).addClass(initialClass + " " + c1);


            }
            else if (items > 7 && items <= 14) {
                currentMenu.removeClass(c123).addClass(initialClass + " " + c2);
            }
            else if (items > 14) {
                currentMenu.removeClass(c123).addClass(initialClass + " " + c3);
            }
            width = currentMenu.outerWidth();

            subMenuPosition(width);
            function subMenuPosition(width) {

                if (width > posLeft && width < posRight) {
                    //for right
                    currentMenu.removeClass(resetMenuPos).addClass(menuPosRight);
                }
                else if (width < posLeft && width > posRight) {
                    //for left
                    currentMenu.removeClass(resetMenuPos).addClass(menuPosLeft);
                }
                else if (width < posLeft && width < posRight) {
                    currentMenu.removeClass(resetMenuPos).addClass(menuPosLeft);//default behaviour
                }
                else if (width > posLeft && width > posRight && currentMenu.hasClass(c1)) {
                    currentMenu.removeClass(resetMenuPos).addClass(menuPosBottom);
                }
                else if (width > posLeft && width > posRight && currentMenu.hasClass(c3) || currentMenu.hasClass(c2)) {
                    var count = 2;

                    do {
                        currentMenu.removeClass(c123);
                        currentMenu.addClass("columnN" + count);
                        width = currentMenu.width();
                        if (width < posRight || width < posLeft) {
                            break;
                        }
                        count--;
                    } while (width > posLeft && count > 0 || width > posRight && count > 0);


                    if (currentMenu.hasClass(c1)) {
                        currentMenu.removeClass(menuPosLeft + " " + menuPosRight).addClass(menuPosBottom);
                    }

                    subMenuPosition(width);
                }//end of condition

            }//end of function subMenuPosition
        });
    },

    closeDropDownOnHtmlClick: function () {
        $('html').click(function () {
            $(".buttonDrop.dropdown").removeClass("open");
            $('#tablediv').find('.zIndexActive').removeClass('zIndexActive').addClass("zIndex");
            //remove padding-bottom on dropdown close; ref 1.1
            $("#tablediv").css("padding-bottom", "");
        });
    },

    disableCheckBoxControl: function () {
        var appoinmentSoltDtl = Scheduling_Calendar.appoinmentSoltDtl;
        for (var i = 0; i < appoinmentSoltDtl.length; i++) {
            var counter = Scheduling_Calendar.appoinmentSoltDtl[i].SlotCount;
            var slotId = appoinmentSoltDtl[i].TimeSlotDtlId;
            var flag = false;
            $('#myTable input:checkbox').each(function (index) {
                var currentChkid = $(this).attr('id');
                if (slotId == currentChkid) {
                    $(this).attr("disabled", true);
                    counter--;
                    flag = true;
                }
                else if (counter > 0 && flag) {
                    $(this).attr("disabled", true);
                    counter--;
                }
                //else if (counter == 0 && flag) {
                //    return false;
                //}
            });
        }

    },

    // End Date January 04, 2015 Edit By Azam Aftab

    //Azam Aftab Dated January 20, 2015  PMS-3479
    ShowAppointmentSummary: function () {
        //if ($("#pnlScheduleCalendar #frmSchedulingCalendar #rdprovider").prop("checked") == true) {
        //    if ($('#pnlScheduleCalendar #Facility').val() == "" || $('#pnlScheduleCalendar #Provider').val() == "") {
        //        utility.DisplayMessages("Select provider or facility from dropdown", 2);
        //        return false;
        //    }
        //} else if ($("#pnlScheduleCalendar #frmSchedulingCalendar #rdresource").prop("checked") == true) {
        //    if ($('#pnlScheduleCalendar #Facility').val() == "" || $('#pnlScheduleCalendar #Resource').val() == "") {
        //        utility.DisplayMessages("Select resource or facility from dropdown", 2);
        //        return false;
        //    }
        //}

        if ($("#pnlScheduleCalendar #frmSchedulingCalendar #rdprovider").prop("checked") == true) {
            if ($('#pnlScheduleCalendar #Provider').val() == "") {
                utility.DisplayMessages("Please select Provider or Resource", 2);
                return false;
            }
        } else if ($("#pnlScheduleCalendar #frmSchedulingCalendar #rdresource").prop("checked") == true) {
            if ($('#pnlScheduleCalendar #Resource').val() == "") {
                utility.DisplayMessages("Please select Provider or Resource", 2);
                return false;
            }
        }

        var params = [];
        var facilityId = $('#pnlScheduleCalendar #Facility').val() != "" ? $('#pnlScheduleCalendar #Facility').val() : "0";
        var providerId = $("#pnlScheduleCalendar #frmSchedulingCalendar #rdprovider").prop("checked") == true ? $('#pnlScheduleCalendar #Provider').val() : "0";
        var resourceId = $("#pnlScheduleCalendar #frmSchedulingCalendar #rdresource").prop("checked") == true ? $('#pnlScheduleCalendar #Resource').val() : "0";
        var datedDate = $('#pnlScheduleCalendar #daydate').text().trim();
        if (datedDate != '' && datedDate != undefined && datedDate != null) {
            params["ProviderId"] = providerId;
            params["ResourceId"] = resourceId;
            params["FacilityId"] = facilityId;
            params["DayDate"] = datedDate;
            params["ParentCtrl"] = "schTabCalendar";
            if ($('#actionPanScheduleCalendar #pnlAppointmentSummary').length > 0) {
                $('#actionPanScheduleCalendar #pnlAppointmentSummary').remove();
            }
            LoadActionPan('appointmentSummary', params);
        } else {
            utility.DisplayMessages("Some thing wrong! Please try again", 2);
        }


    },
    // End Date January 20, 2015 Edit By Azam Aftab PMS-3479


    ReloadSchedulerOnDDLChange: function () {
        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
        //expression for week range
        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
        //Month Regular Expression
        var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
        var criteria = $('#pnlScheduleCalendar #daydate').text().trim();
        if (criteria.match(dayrgx) || criteria.match(weekrg)) {
            Scheduling_Calendar.ChangeDate();
        }
        else if (criteria.match(monthreg)) {
            Scheduling_Calendar.ClearTable();
            var month = $.datepicker.formatDate('mm/yy', new Date(Scheduling_Calendar.FormatDate(criteria)));
            var statusslots = Scheduling_Calendar.FilterCriteria();
            if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {

                Scheduling_Calendar.MonthCalendar($('#pnlScheduleCalendar #Provider').val(), $('#pnlScheduleCalendar #Facility').val(), null, month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
            }
            else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {

                Scheduling_Calendar.MonthCalendar(null, $('#pnlScheduleCalendar #Facility').val(), $('#pnlScheduleCalendar #Resource').val(), month, statusslots, $('#pnlScheduleCalendar #ddlPatientTypeSc').val(), $('#pnlScheduleCalendar #ddlVisitTypeSc').val());
            }

        }
    },
    DisableWeekCheckBox: function () {
        $('#pnlScheduleCalendar #checkboxpanel input[type=checkbox]').prop("disabled", true);
    },
    EnableWeekCheckBox: function () {
        $('#pnlScheduleCalendar #checkboxpanel input[type=checkbox]').prop("disabled", false);
    },
    WeekCheckBoxClick: function () {
        Scheduling_Calendar.DisableWeekCheckBox();
        Scheduling_Calendar.ChangeDate(true);
    },

    ShowBlockAppSummary: function () {

        //if ($("#pnlScheduleCalendar #frmSchedulingCalendar #rdprovider").prop("checked") == true) {
        //    if ($('#pnlScheduleCalendar #Facility').val() == "" || $('#pnlScheduleCalendar #Provider').val() == "") {
        //        utility.DisplayMessages("Select provider or facility from dropdown", 2);
        //        return false;
        //    }
        //} else if ($("#pnlScheduleCalendar #frmSchedulingCalendar #rdresource").prop("checked") == true) {
        //    if ($('#pnlScheduleCalendar #Facility').val() == "" || $('#pnlScheduleCalendar #Resource').val() == "") {
        //        utility.DisplayMessages("Select resource or facility from dropdown", 2);
        //        return false;
        //    }
        //}
        var params = [];
        var facilityId = $('#pnlScheduleCalendar #Facility').val();
        var providerId = $("#pnlScheduleCalendar #frmSchedulingCalendar #rdprovider").prop("checked") == true ? $('#pnlScheduleCalendar #Provider').val() : "0";
        var resourceId = $("#pnlScheduleCalendar #frmSchedulingCalendar #rdresource").prop("checked") == true ? $('#pnlScheduleCalendar #Resource').val() : "0";
        var datedDate = $('#pnlScheduleCalendar #daydate').text().trim();
        if (datedDate != '' && datedDate != undefined && datedDate != null) {
            params["ProviderId"] = providerId;
            params["ResourceId"] = resourceId;
            params["FacilityId"] = facilityId;
            params["DayDate"] = datedDate;
            params["ParentCtrl"] = "schTabCalendar";
            //if ($('#actionPanScheduleCalendar #pnlAppointmentSummary').length > 0) {
            //    $('#actionPanScheduleCalendar #pnlAppointmentSummary').remove();
            //}
            LoadActionPan('Scheduling_BlockAppointment_Summary', params);
        } else {
            utility.DisplayMessages("Some thing wrong! Please try again", 2);
        }

    },

    SendQuickReminder: function (appId, type, patientId, PatientEmail, Appointmentdate, FacilityPhoneNo) {
        Scheduling_Calendar.FillPatientPreferences(patientId).done(function (response) {
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
                var facilityPhoneNo = FacilityPhoneNo.replace(/[_\W]+/g, "");

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
                    params["AppointmentId"] = appId;
                    params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
                    params["PatientId"] = patientId;
                    params["ScreenType"] = type;
                    params["patientNumber"] = patientNumber;
                    params["FacilityPhoneNo"] = facilityPhoneNo;
                    params["patientGuarantorId"] = patientGuarantorId;
                    params["PatientName"] = patientName;
                    params["PatientEmail"] = PatientEmail;
                    params["ProviderName"] = $("#pnlScheduleCalendar #Provider option:selected").text();
                    params["AppointmentDate"] = Appointmentdate;
                    params["patientGuarantorNumber"] = guarantorNumber;
                    params["mode"] = "Add";
                    LoadActionPan('remindersDetail', params);
                }

            }

        });

    },


    //Start//Abid Ali//26-07-2016//eSuperbill section

    //Load Popup of eSuperbill creation.
    createSuperbill: function (patientId, vistId, noteId, billInfoId, btnClicked, patientType, providerId, refProviderId, FacilityId, appId) {
        if (billInfoId == -1) {
            var Obj = new Object();
            Obj["ENMTypeId"] = null;
            Obj["BillingInfoId"] = '-1'
            Obj["commandType"] = "BILLING_INFORMATION_SAVE";
            Obj["NotesId"] = noteId;
            Obj["PatientId"] = patientId;
            Obj["ProviderId"] = providerId
            Obj["VisitId"] = vistId;
            Obj["Status"] = 'Draft';
            Obj["VisitDate"] = $('#' + Scheduling_Calendar.params.PanelID + ' #daydate span').html();
            Scheduling_Calendar.BillingInfoSave(Obj).done(function (response) {
                response = JSON.parse(response);
                if (response.status) {
                    var params = [];
                    noteId = noteId == null ? 0 : noteId;
                    providerId = providerId != null ? providerId : "";
                    refProviderId = refProviderId != null ? refProviderId : providerId;
                    FacilityId = FacilityId != null ? FacilityId : "";
                    billInfoId = billInfoId == null ? -1 : billInfoId;
                    params["FromAdmin"] = "0";
                    params["ParentCtrl"] = Scheduling_Calendar.params.TabID;
                    params["ProviderId"] = providerId;
                    //params["AppointmentVisitId"] = vistId;
                    params["PatientId"] = patientId;
                    params["NotesId"] = parseInt(noteId);
                    params["VisitId"] = vistId;
                    params["BillingInfoId"] = response.BillingInfoId;
                    params["BtnClicked"] = btnClicked;
                    params["appId"] = appId;

                    LoadActionPan('BillingInformation', params);
                }
                else {

                }
            });
        }
        else {
            var params = [];
            noteId = noteId == null ? 0 : noteId;
            providerId = providerId != null ? providerId : "";
            refProviderId = refProviderId != null ? refProviderId : providerId;
            FacilityId = FacilityId != null ? FacilityId : "";
            billInfoId = billInfoId == null ? -1 : billInfoId;

            params["FromAdmin"] = "0";
            params["ParentCtrl"] = Scheduling_Calendar.params.TabID;
            params["ProviderId"] = providerId;
            params["AppointmentVisitId"] = vistId;
            params["PatientId"] = patientId;
            params["NotesId"] = parseInt(noteId);
            params["VisitId"] = vistId;
            params["BillingInfoId"] = parseInt(billInfoId);
            params["BtnClicked"] = btnClicked;
            params["appId"] = appId;
            params["PatientType"] = patientType.indexOf("New") > -1 ? 1 : 2;

            LoadActionPan('BillingInformation', params);
        }
    },
    BillingInfoSave: function (objData) {
        objData["commandType"] = "BILLING_INFORMATION_SAVE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    viewSuperbill: function (patientId, visitId, noteId, billInfoId, btnClicked, patientType, providerId, refProviderId, FacilityId, parentCtrl) {

        //providerId = providerId != null ? providerId : "";
        //refProviderId = refProviderId != null ? refProviderId : providerId;
        //FacilityId = FacilityId != null ? FacilityId : "";

        //var params = [];
        //params["FromAdmin"] = 0;

        //if (parentCtrl == "pnlScheduleMuliView") {
        //    params["ParentCtrl"] = 'schTabMultipleView';
        //}
        //else {
        //    params["ParentCtrl"] = 'schTabCalendar';
        //}

        //params["VisitId"] = visitId;
        //params["patientID"] = patientId;

        //LoadActionPan('EncounterChargeCapture', params);


        var params = [];
        if (parentCtrl == "pnlScheduleMuliView") {
            params["ParentCtrl"] = 'schTabMultipleView';
        }
        else {
            params["ParentCtrl"] = 'schTabCalendar';
        }
        params["FromAdmin"] = 0;
        params["NotesId"] = noteId;
        params["VisitId"] = visitId;
        //        params["NoteDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #dtpVisitDate').val();
        params["BillingInfoId"] = parseInt(billInfoId);
        //       params["VisitDate"] = Clinical_ProgressNote.params.VisitDateForFollowUp;
        params["PatientId"] = patientId;
        params["ProviderId"] = providerId;
        params["PatientTypeId"] = patientType;
        //    params["AppointmentDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #txtLinkedAppointment').val();

        //       params["NoteStatus"] = $('#pnlClinicalProgressNote #hfNoteStatus').val();
        LoadActionPan("BillingInformation", params);

    },

    //Gets eSuperbill
    getESuperBillLink: function (patientId, visitId, noteId, btnClicked, patientType, providerId, refProviderId, FacilityId, appid, billInfoId, billInfoStatus) {

        var link = '';
        return link; //EMR-5167

        //if (Scheduling_Calendar.eSuperbillAddPermission) {
        //    link = '<li  id="' + visitId + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientId.trim() + '\',\'' + visitId.trim() + '\',\'' + noteId.trim() + '\',\'-1\',\'' + btnClicked.trim() + '\',\'' + patientType.trim() + '\',\'' + providerId + '\',\'' + refProviderId + '\',\'' + FacilityId + '\',\'' + appid + '\');" >Create eSuperbill</a></li>';
        //}
        //if (Scheduling_Calendar.eSuperbillEditPermission) {
        //    if (billInfoId && billInfoStatus && billInfoStatus == "Draft") {
        //        link = '<li  id="' + visitId + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientId.trim() + '\',\'' + visitId.trim() + '\',\'' + noteId.trim() + '\',\'' + billInfoId.trim() + '\',\'' + btnClicked.trim() + '\',\'' + patientType.trim() + '\',\'' + providerId + '\',\'' + refProviderId + '\',\'' + FacilityId + '\',\'' + appid + '\');">Edit eSuperbill</a></li>';
        //    }
        //}
        //if (Scheduling_Calendar.eSuperbillViewPermission) {
        //    if (billInfoId && billInfoStatus && billInfoStatus == "Signed") {
        //        link = '<li  id="' + visitId + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.viewSuperbill(\'' + patientId.trim() + '\',\'' + visitId.trim() + '\',\'' + noteId.trim() + '\',\'' + billInfoId.trim() + '\',\'' + btnClicked.trim() + '\',\'' + patientType.trim() + '\',\'' + providerId + '\',\'' + refProviderId + '\',\'' + FacilityId + '\',\'' + Scheduling_Calendar.params.PanelID.trim() + '\');" >View eSuperbill</a></li>';
        //    }
        //}
        //return link;



    },

    getESuperBillLinkForCheckOut: function (patientId, visitId, noteId, btnClicked, patientType, providerId, refProviderId, FacilityId, appid, billInfoId, billInfoStatus) {

        var link = '';

        if (Scheduling_Calendar.eSuperbillAddPermission) {
            link = '<li  id="' + visitId + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientId.trim() + '\',\'' + visitId.trim() + '\',\'' + noteId.trim() + '\',\'-1\',\'' + btnClicked.trim() + '\',\'' + patientType.trim() + '\',\'' + providerId + '\',\'' + refProviderId + '\',\'' + FacilityId + '\',\'' + appid + '\');" >Create eSuperbill</a></li>';
        }
        if (Scheduling_Calendar.eSuperbillEditPermission) {
            if (billInfoId && billInfoStatus && billInfoStatus == "Draft") {
                link = '<li  id="' + visitId + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.createSuperbill(\'' + patientId.trim() + '\',\'' + visitId.trim() + '\',\'' + noteId.trim() + '\',\'' + billInfoId.trim() + '\',\'' + btnClicked.trim() + '\',\'' + patientType.trim() + '\',\'' + providerId + '\',\'' + refProviderId + '\',\'' + FacilityId + '\',\'' + appid + '\');">Edit eSuperbill</a></li>';
            }
        }
        if (Scheduling_Calendar.eSuperbillViewPermission) {
            if (billInfoId && billInfoStatus && billInfoStatus == "Signed") {
                link = '<li  id="' + visitId + '"><a tabindex="-1" href="#" onclick="Scheduling_Calendar.viewSuperbill(\'' + patientId.trim() + '\',\'' + visitId.trim() + '\',\'' + noteId.trim() + '\',\'' + billInfoId.trim() + '\',\'' + btnClicked.trim() + '\',\'' + patientType.trim() + '\',\'' + providerId + '\',\'' + refProviderId + '\',\'' + FacilityId + '\',\'' + Scheduling_Calendar.params.PanelID.trim() + '\');" >View eSuperbill</a></li>';
            }
        }
        return link;



    },

    getAllESuperbills: function () {
        var dfd = new $.Deferred();
        Scheduling_Calendar.getAllESuperbillsDbCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                Scheduling_Calendar.eSuperbillAddPermission = response.addPermission == "" ? true : false;
                Scheduling_Calendar.eSuperbillEditPermission = response.editPermission == "" ? true : false;
                Scheduling_Calendar.eSuperbillViewPermission = response.viewPermission == "" ? true : false;

                response.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFill_JSON);

                $.each(response.BillingInfoFill_JSON, function () {

                    Scheduling_Calendar.eSuperbillInfo.push($(this));

                });
                dfd.resolve('ok');
                return dfd.promise();
            }
            else {
                //utility.DisplayMessages(response.Message, 3);
                Scheduling_Calendar.eSuperbillInfo = [];
            }
        });
        return dfd.promise();
    },

    //Start//======Db Calls======

    getAllESuperbillsDbCall: function () {
        var objData = new Object();
        //objData["BillingInfoId"] = BillingInformation.params.BillingInfoId;
        objData["commandType"] = "BILLING_INFORMATION_SELECT_Customized";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },

    // End //======Db Calls======

    //End//Abid Ali//26-07-2016//eSuperbill section

    FillPatientPreferences: function (patientID) {
        var data = "PatientID=" + patientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_PREFERENCES", "FILL_PATIENT_PREFERENCES");
    },

    // Start //===== Reschedule Appointment

    RescheduleAppointmentSearch: function (facilityId, facilityName, providerId, providerName, resourceId, resourceName, appointmentId, patientId) {

        var params = [];
        //params["FromAdmin"] = "0";
        params["FacilityId"] = facilityId;
        params["FacilityName"] = facilityName;
        params["ProviderId"] = providerId;
        params["ProviderName"] = providerName;
        params["ResourceId"] = resourceId;
        params["ResourceName"] = resourceName;
        params["AppointmentId"] = appointmentId;
        params["PatientId"] = patientId;
        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();
        params["ParentCtrl"] = 'schTabCalendar';
        LoadActionPan('Scheduling_RescheduleSearch', params);

    },


    // End //===== Reschedule Appointment
    ProviderAppointmentPrint: function () {
        if ($("#pnlPMSScheduler #hfProviderIds").val() == "" && $("#pnlPMSScheduler #hfResourceIds").val() == "") {
            utility.DisplayMessages("Please select Provider/Resource", 2);
            return false;
        }

        var IsDaySechedule = $("#" + Scheduling_Calendar.params.PanelID + " #btnday").hasClass("active");
        var ProviderId = $('#hfProviderIds').val();
        var FacilityId = $('#hfFacilityIds').val();
        var ResourceId = $("#hfResourceIds").val();
        var scheduler = $("#scheduler").data("kendoScheduler");

        if ((ProviderId != "" || FacilityId != "") || ResourceId) {
            var params = {};
            params["ProviderId"] = ProviderId;
            params["FacilityId"] = FacilityId;
            params["ResourceId"] = ResourceId;
            params["AppointmentDate"] = scheduler._model.formattedShortDate;
            params["ParentCtrl"] = 'schTabCalendar';
            params["isSaveReceiptDoc"] = false;
            if (ResourceId != "")
                params["IsResourceSelected"] = 1;
            else
                params["IsResourceSelected"] = 0;
            LoadActionPan('Scheduling_ProviderAppointmentPrint', params);
        }
    },
    ForceBookingAppointment: function () {


        var params = {};

        params["ProviderId"] = $('#pnlScheduleCalendar #Provider').val();
        params["ProviderName"] = $('#pnlScheduleCalendar #Provider option:selected').text();
        if ($('#pnlScheduleCalendar #Facility').val() != "") {
            params["FacilityId"] = $('#pnlScheduleCalendar #Facility').val();
            params["FacilityName"] = $('#pnlScheduleCalendar #Facility option:selected').text();
        }
        params["DayDate"] = $('#pnlScheduleCalendar #daydate span').html();
        params["ParentCtrl"] = 'schTabCalendar';
        params["isSaveReceiptDoc"] = false;
        params["mode"] = "Add";
        params["SlotMinutes"] = 15;

        LoadActionPan('Scheduling_Force_Booking', params);

    }
};