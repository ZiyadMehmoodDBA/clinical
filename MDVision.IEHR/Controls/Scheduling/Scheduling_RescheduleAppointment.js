Scheduling_RescheduleAppointment = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Scheduling_RescheduleAppointment.params = params;

        var self = $('#pnlRescheduleAppointment');
        //self.loadDropDowns(true).done(function () {
        Scheduling_RescheduleAppointment.ValidateRescheduleAppointment();
        //Scheduling_RescheduleAppointment.LoadSlotBlockUnblock();
        $('#pnlRescheduleAppointment #frmRescheduleAppointment  #ddlToTime').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false;
            if ($('#pnlRescheduleAppointment #frmRescheduleAppointment').data("bootstrapValidator") != null && typeof $('#pnlRescheduleAppointment #frmRescheduleAppointment').data('bootstrapValidator') != 'undefined') {
                $('#pnlRescheduleAppointment #frmRescheduleAppointment').bootstrapValidator('revalidateField', 'ToTime');
            }

        });
        //serialize Data.
        //$('#frmblckreason').data('serialize', $('#frmblckreason').serialize());
        utility.CreateDatePicker('pnlRescheduleAppointment #frmRescheduleAppointment #dpToDate',
    function (ev) {
       // Scheduling_RescheduleAppointment.fillToTimeDDL();
        if ($('#pnlRescheduleAppointment #frmRescheduleAppointment').data("bootstrapValidator") != null) {
            $('#pnlRescheduleAppointment #frmRescheduleAppointment').bootstrapValidator('revalidateField', 'ToDate');
        }
        //on-change callback method 
    },
    false);
        //$('#pnlRescheduleAppointment #frmRescheduleAppointment #dpToDate').datepicker("setDate", Scheduling_RescheduleAppointment.params.DayDate);
        if (Scheduling_RescheduleAppointment.params.AppointmentDate) {
            $('#pnlRescheduleAppointment #frmRescheduleAppointment #dpToDate').datepicker("setDate", new Date(Scheduling_RescheduleAppointment.params.AppointmentDate));
        }
        else {
            $('#pnlRescheduleAppointment #frmRescheduleAppointment #dpToDate').datepicker("setDate", new Date());
        }
        
        //});


        //$('#pnlBatchExportCCDA  #MultiTime').timepicker({
        //    defaultTime: 'now',
        //    minuteStep: 5//,
        //});

        // $('#pnlBatchExportCCDA  #ddlToTime').timepicker('setTime', new Date());
        $('#pnlRescheduleAppointment #frmRescheduleAppointment').bootstrapValidator('revalidateField', 'ToTime');
    },

    fillToTimeDDL: function () {


        var facilityID = Scheduling_RescheduleAppointment.params.FacilityId;
        var providerID = Scheduling_RescheduleAppointment.params.ProviderId;
        var resourceID = Scheduling_RescheduleAppointment.params.ResourceId;
        var scheduleDate = $('#pnlRescheduleAppointment #frmRescheduleAppointment #dpToDate').val();

        Scheduling_RescheduleAppointment.BindDropDownsFromTime(facilityID, providerID, resourceID, scheduleDate).done(function (response) {
            var objDDLToTime = $('#pnlRescheduleAppointment #frmRescheduleAppointment #ddlToTime');
            var l = $(objDDLToTime);
            l.empty();

            if (response.status != false) {

                var DDLLoad_JSON = JSON.parse(response.DDLLoad_JSON);

                $.each(DDLLoad_JSON, function (i, result) {

                    l.append($("<option />").val(result.TimeSlotDtlId).text(result.FromTimeSlots).attr("TimeSlotId", result.TimeSlotId).attr("TimeSlotDtlId", result.TimeSlotDtlId));

                });

            } else {

            }
        });

    },

    BindDropDownsFromTime: function (facilityID, providerID, resourceID, scheduleDate) {

        var data = "facilityID=" + facilityID + "&providerID=" + providerID + "&resourceID=" + resourceID + "&scheduleDate=" + scheduleDate;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_BLOCK_UNBLOCK", "FILL_FROMTIME_DDL");


    },

    ValidateRescheduleAppointment: function () {
        $('#frmRescheduleAppointment')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               excluded: ':disabled',
               fields: {

                   ToDate: {
                       group: '.col-sm-6',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ToTime: {
                       group: '.col-sm-6',
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
            Scheduling_RescheduleAppointment.RescheduleAppointmentSave();
        });
    },

    RescheduleAppointmentSave: function (status, slotids) {


        var AppointmentId = Scheduling_RescheduleAppointment.params.AppointmentId;
        // var slotdetailid = $('#pnlRescheduleAppointment #frmRescheduleAppointment #ddlToTime option:selected').attr('timeslotdtlid');
        var NewTime = $('#pnlRescheduleAppointment #frmRescheduleAppointment #ddlToTime').val();
        var NewDate = $('#pnlRescheduleAppointment #frmRescheduleAppointment #dpToDate').val();

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Scheduling_RescheduleAppointment.MoveAppointment(AppointmentId, NewDate, NewTime).done(function (response) {
                    if (response.status != false) {
                        Scheduling_RescheduleAppointment.UnLoad();
                        utility.DisplayMessages(response.Message, 1);
                        var table1 = $('#tblBlockAppointment_Summary #dgvBlockAppointment').DataTable();
                        table1.row('.active').remove().draw(false);

                        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                        var calendarDate = $('#pnlScheduleCalendar #daydate').text().trim();

                        var provId = $("#pnlScheduleCalendar #Provider").val();
                        var facId = $("#pnlScheduleCalendar #Facility").val();
                        var resId = $("#pnlScheduleCalendar #Resource").val();


                        if (calendarDate.match(weekrg) && calendarDate.length > 15) {
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
                            Scheduling_Calendar.DayCalendar(provId, resId, facId, schcheckin.params.DayDate, statusslots);
                            if (provId != "")
                                $("#pnlScheduleCalendar #Provider option[value=" + provId + "]").attr('selected', 'selected');
                            if (resId != "")
                                $("#pnlScheduleCalendar #Resource option[value=" + resId + "]").attr('selected', 'selected');
                            if (facId != "")
                                $("#pnlScheduleCalendar #Facility option[value=" + facId + "]").attr('selected', 'selected');
                            if (schcheckin.params.DayDate != "")
                                $('#pnlScheduleCalendar #daydate span').html(calendarDate);
                        }
                        PMSScheduler.ReloadScheduler();
                    } else {
                        utility.DisplayMessages(response.Message, 2);
                    }
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    MoveAppointment: function (AppointmentID, NewDate, NewTime) {
        var data = "AppointmentID=" + AppointmentID + "&NewDate=" + NewDate + "&NewTime=" + NewTime;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_CALENDAR", "RESCHEDULE_APPOINTMENT");
    },

    UnLoad: function () {

        UnloadActionPan(Scheduling_RescheduleAppointment.params["ParentCtrl"], "pnlRescheduleAppointment");
    },
}