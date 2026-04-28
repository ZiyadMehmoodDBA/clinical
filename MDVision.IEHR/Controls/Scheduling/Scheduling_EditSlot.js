schEditSlot = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        schEditSlot.params = params;
        var self = $('#schEditSlot');
        self.loadDropDowns(true).done(function () {

            schEditSlot.SelectSlotDetail(schEditSlot.params.TimeslotDetailid, schEditSlot.params.ProviderId, schEditSlot.params.ResourceId);
            schEditSlot.ValidateSlotEdit();
        });



    },

    SelectSlotDetail: function (TimeslotDetailid, ProviderId, ResourceId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Slot", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#schEditSlot #pnlSlotEdit_Result").css("display") == "none") {
                    $("#schEditSlot #pnlSlotEdit_Result").show();
                }

                schEditSlot.SelectSchSlotDetail(TimeslotDetailid, ProviderId, ResourceId).done(function (response) {

                    if (response.status != false) {
                        var editslotdetail = JSON.parse(response.SchSlotDetailFill_JSON);
                        var editslotdetail1 = JSON.parse(response.SchSlotDetail_JSON);
                        var slotDetail = JSON.parse(response.SchSlot_JSON);
                        var self = $("#schEditSlot");
                        utility.bindMyJSON(true, editslotdetail, false, self).done(function () {

                            //$('#schEditSlot #txtProvider').val($("#pnlScheduleCalendar #Provider option:selected").text());
                            //$('#schEditSlot #txtResource').val($("#pnlScheduleCalendar #Resource option:selected").text());
                            //$('#schEditSlot #txtFacility').val($("#pnlScheduleCalendar #Facility option:selected").text());

                            // $("#frmSchSlotEdit #ddlReason option[value=" + slotDetail[0].ScheduleReasonId + "]").attr('selected', 'selected');

                            if (slotDetail[0].BlockStatus == 'Blocked') {


                                $('#schEditSlot #btnBlockUnblock').html('Unblock');

                                if (slotDetail[0].BlockReasonId != "")
                                    $("#frmSchSlotEdit #ddlReason option[value=" + slotDetail[0].BlockReasonId + "]").attr('selected', 'selected');

                            }
                            else if ((slotDetail[0].BlockStatus == 'Unblocked')) {

                                $('#schEditSlot #btnBlockUnblock').html('Block');
                                if (slotDetail[0].BlockReasonId != "")
                                    $("#frmSchSlotEdit #ddlReason option[value=" + slotDetail[0].BlockReasonId + "]").attr('selected', 'selected');
                            }

                            if (slotDetail[0].OverBookAllowed == 'True') {


                                $('#schEditSlot #chkOverbookAllowed').attr('checked', true);

                            }
                            else if ((slotDetail[0].OverBookAllowed == 'False')) {
                                $('#schEditSlot #chkOverbookAllowed').attr('checked', false);
                            }
                            schEditSlot.SchSlotEditGridLoad(response);

                            //serialize Data after all controls loaded.
                            $('#frmSchSlotEdit').data('serialize', $('#frmSchSlotEdit').serialize());
                        });


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

    SchSlotEditGridLoad: function (response) {
        $("#dgvSlotEdit").dataTable().fnDestroy();
        $("#pnlSlotEdit_Result #dgvSlotEdit tbody").find("tr").remove();
        if (response.SchSlotDetailsCount > 0) {
            var SchSlotDetailJSONData = JSON.parse(response.SchSlotDetail_JSON);
            if (SchSlotDetailJSONData[0].AccountNumber != "") {
                $.each(SchSlotDetailJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#dgvSlotEdit_row" + item.AppointmentId + "'))");
                    $row.attr("id", "dgvSlotEdit_row" + item.AppointmentId);
                    $row.attr("AppointmentId", item.AppointmentId);

                    var strAction = "";

                    var classDisabled = "disabled";
                    //----Full String--------
                    //strAction = '<a class="btn  btn-xs" href="#" onclick="schEditSlot.PatientAppointmentDelete(' + item.AppointmentId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="schEditSlot.AppointmentEdit(' + item.AppointmentId + ');"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="schEditSlot.LoadCheckIn(' + item.AppointmentId + ', ' + item.PatientId + ');"  title="Check In"><i class="fa fa-check-square-o black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="schEditSlot.LoadCheckOut(' + item.AppointmentId + ',' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Check Out"><i class="fa fa-external-link black"></i></a><a class="btn btn-xs" href="#" onclick="schEditSlot.OpenCoPayment(' + item.AppointmentId + ', ' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Copayment"><i class="fa fa-money black"></i></a>&nbsp;';

                    //---- After Confirm------
                    if (item.Status.toUpperCase() == 'CONFIRM') {
                        strAction = '<a class="btn btn-xs" href="#" onclick="schEditSlot.PatientAppointmentDelete(' + item.AppointmentId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="schEditSlot.AppointmentEdit(' + item.AppointmentId + ',\'' + item.VisitId + '\',0);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="schEditSlot.LoadCheckIn(' + item.AppointmentId + ', ' + item.PatientId + ');"  title="Check In"><i class="fa fa-check-square-o black"></i></a>&nbsp;<a class="btn btn-xs ' + classDisabled + ' " href="#" onclick="schEditSlot.LoadCheckOut(' + item.AppointmentId + ',' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Check Out"><i class="fa fa-external-link black"></i></a><a class="btn btn-xs" href="#" onclick="schEditSlot.OpenCoPayment(' + item.AppointmentId + ', ' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Copayment"><i class="fa fa-money black"></i></a>&nbsp;';
                    }
                        //---- After CheckIn------
                    else if (item.Status.toUpperCase() == 'CHECKIN' || item.Status.toUpperCase() == 'SIGNED' || item.Status.toUpperCase() == 'ROOMED') {
                        strAction = '<a class="btn btn-xs ' + classDisabled + '" href="#" onclick="schEditSlot.PatientAppointmentDelete(' + item.AppointmentId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="schEditSlot.AppointmentEdit(' + item.AppointmentId + ',\'' + item.VisitId + '\',1);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs ' + classDisabled + '" href="#" onclick="schEditSlot.LoadCheckIn(' + item.AppointmentId + ', ' + item.PatientId + ');"  title="Check In"><i class="fa fa-check-square-o black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="schEditSlot.LoadCheckOut(' + item.AppointmentId + ',' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Check Out"><i class="fa fa-external-link black"></i></a><a class="btn btn-xs" href="#" onclick="schEditSlot.OpenCoPayment(' + item.AppointmentId + ', ' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Copayment"><i class="fa fa-money black"></i></a>&nbsp;';
                    }
                        //---- After CheckOut-----
                    else if (item.Status.toUpperCase() == 'CHECKOUT') {
                        strAction = '<a class="btn btn-xs ' + classDisabled + '" href="#" onclick="schEditSlot.PatientAppointmentDelete(' + item.AppointmentId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="schEditSlot.AppointmentEdit(' + item.AppointmentId + ',\'' + item.VisitId + '\',2);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs ' + classDisabled + '" href="#" onclick="schEditSlot.LoadCheckIn(' + item.AppointmentId + ', ' + item.PatientId + ');"  title="Check In"><i class="fa fa-check-square-o black"></i></a>&nbsp;<a class="btn btn-xs ' + classDisabled + '" href="#" onclick="schEditSlot.LoadCheckOut(' + item.AppointmentId + ',' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Check Out"><i class="fa fa-external-link black"></i></a><a class="btn btn-xs" href="#" onclick="schEditSlot.OpenCoPayment(' + item.AppointmentId + ', ' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Copayment"><i class="fa fa-money black"></i></a>&nbsp;';
                    }
                    else {
                        //----Before Confirm------
                        strAction = '<a class="btn  btn-xs" href="#" onclick="schEditSlot.PatientAppointmentDelete(' + item.AppointmentId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="schEditSlot.AppointmentEdit(' + item.AppointmentId + ',\'' + item.VisitId + '\',0);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs ' + classDisabled + '" href="#" onclick="schEditSlot.LoadCheckIn(' + item.AppointmentId + ', ' + item.PatientId + ');"  title="Check In"><i class="fa fa-check-square-o black"></i></a>&nbsp;<a class="btn btn-xs ' + classDisabled + '" href="#" onclick="schEditSlot.LoadCheckOut(' + item.AppointmentId + ',' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Check Out"><i class="fa fa-external-link black"></i></a><a class="btn btn-xs" href="#" onclick="schEditSlot.OpenCoPayment(' + item.AppointmentId + ', ' + item.PatientId + ',\'' + item.VisitId + '\');"  title="Copayment"><i class="fa fa-money black"></i></a>&nbsp;';
                    }

                    $row.append('<td style="display:none;">' + item.AppointmentId + '</td><td>' + strAction + '</td><td>' + item.AccountNumber + '</td><td>' + item.FirstName + '</td><td>' + item.LastName + '</td><td>' + item.DOB.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.PatientType + '</td><td>' + item.VisitType + '</td><td>' + item.Reason + '</td><td>' + item.Comments + '</td><td>' + item.Status + '</td>');


                    $("#pnlSlotEdit_Result #dgvSlotEdit tbody").last().append($row);
                });

                $('#schEditSlot #btnBlockUnblock').hide();
            }


        }
        else {
            $('#dgvSlotEdit').DataTable({
                "language": {
                    "emptyTable": "No Appointment Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvSlotEdit'))
            ;
        else
            $("#pnlSlotEdit_Result #dgvSlotEdit").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    ValidateSlotEdit: function () {
        $('#frmSchSlotEdit')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   patientAllowed: {
                       group: '.col-sm-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   //Reason: {
                   //    group: '.col-sm-2',
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
            schEditSlot.UpdateSlotInfo();
        });
    },

    BlockUnblockSlot: function () {


        if ($('#schEditSlot #btnBlockUnblock').html() == 'Unblock') {

            if (schEditSlot.params.PanelID == "pnlScheduleMuliView") {

                var params = [];
                params["ParentCtrl"] = "schEditSlot";
                params["slotids"] = schEditSlot.params.TimeslotDetailid;
                params["Status"] = "UnBlocked";
                params["PanelID"] = schEditSlot.params.PanelID;
                params["ProviderId"] = schEditSlot.params.ProviderId;
                params["FacilityId"] = schEditSlot.params.FacilityId;
                params["ResourceId"] = schEditSlot.params.ResourceId;
                //params["DayDate"] = schEditSlot.params.DayDate;
                params["DateId"] = schEditSlot.params.DateId;
                params["MultipleView"] = "1";


                LoadActionPan('blckreasonDetail', params);


            }
            else if (schEditSlot.params.PanelID == "pnlScheduleSearch") {
                var params = [];
                params["ParentCtrl"] = "schEditSlot";
                params["slotids"] = schEditSlot.params.TimeslotDetailid;
                params["Status"] = "UnBlocked";
                params["PanelID"] = schEditSlot.params.PanelID;
                params["CurrentPage"] = schEditSlot.params.CurrentPage;
                params["RecordsPerPage"] = schEditSlot.params.RecordsPerPage;
                params["ScheduleSearch"] = "1";
                LoadActionPan('blckreasonDetail', params);

            }
            else {
                var params = [];
                params["ParentCtrl"] = "schEditSlot";
                params["slotids"] = schEditSlot.params.TimeslotDetailid;
                params["Status"] = "UnBlocked";
                params["PanelID"] = schEditSlot.params.PanelID;
                params["ProviderId"] = schEditSlot.params.ProviderId;
                params["FacilityId"] = schEditSlot.params.FacilityId;
                params["ResourceId"] = schEditSlot.params.ResourceId;
                params["DayDate"] = schEditSlot.params.DayDate;
                //params["DateId"] = schEditSlot.params.DateId;
                LoadActionPan('blckreasonDetail', params);
            }


        }
        else if ($('#schEditSlot #btnBlockUnblock').html() == 'Block') {
            if (schEditSlot.params.PanelID == "pnlScheduleMuliView") {
                var params = [];
                params["ParentCtrl"] = "schEditSlot";
                params["slotids"] = schEditSlot.params.TimeslotDetailid;
                params["PanelID"] = schEditSlot.params.PanelID;
                params["ProviderId"] = schEditSlot.params.ProviderId;
                params["FacilityId"] = schEditSlot.params.FacilityId;
                params["ResourceId"] = schEditSlot.params.ResourceId;
                //params["DayDate"] = schEditSlot.params.DayDate;
                params["DateId"] = schEditSlot.params.DateId;
                params["MultipleView"] = "1";



                params["Status"] = "Blocked";

                LoadActionPan('blckreasonDetail', params);
            }
            else if (schEditSlot.params.PanelID == "pnlScheduleSearch") {
                var params = [];
                params["ParentCtrl"] = "schEditSlot";
                params["slotids"] = schEditSlot.params.TimeslotDetailid;
                params["Status"] = "Blocked";
                params["PanelID"] = schEditSlot.params.PanelID;
                params["CurrentPage"] = schEditSlot.params.CurrentPage;
                params["RecordsPerPage"] = schEditSlot.params.RecordsPerPage;
                params["ScheduleSearch"] = "1";
                LoadActionPan('blckreasonDetail', params);

            }
            else {

                var params = [];
                params["ParentCtrl"] = "schEditSlot";
                params["slotids"] = schEditSlot.params.TimeslotDetailid;
                params["PanelID"] = schEditSlot.params.PanelID;
                params["ProviderId"] = schEditSlot.params.ProviderId;
                params["FacilityId"] = schEditSlot.params.FacilityId;
                params["ResourceId"] = schEditSlot.params.ResourceId;
                params["DayDate"] = schEditSlot.params.DayDate;
                // params["DateId"] = schEditSlot.params.DateId;
                params["Status"] = "Blocked";

                LoadActionPan('blckreasonDetail', params);
            }
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
                                var table1 = $('#dgvSlotEdit').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);

                                schEditSlot.SelectSlotDetail(schEditSlot.params.TimeslotDetailid, schEditSlot.params.ProviderId, schEditSlot.params.ResourceId);

                                //expression for day range
                                var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                //expression for week range
                                var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;

                                if (schEditSlot.params.PanelID == "pnlScheduleMuliView") {

                                    var providerid = schEditSlot.params.ProviderId;
                                    var facilityid = schEditSlot.params.FacilityId;
                                    var resourceid = schEditSlot.params.ResourceId;
                                    var date = schEditSlot.params.DayDate;
                                    var dateid = schEditSlot.params.DateId;

                                    Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);

                                }

                                else if (schEditSlot.params.DayDate.match(weekrg) && schEditSlot.params.DayDate.length > 15) {
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
                                    Scheduling_Calendar.DayCalendar(schEditSlot.params.ProviderId, schEditSlot.params.ResourceId, schEditSlot.params.FacilityId, schEditSlot.params.DayDate, statusslots);
                                    if (schEditSlot.params.ProviderId != "")
                                        $("#pnlScheduleCalendar #Provider option[value=" + schEditSlot.params.ProviderId + "]").attr('selected', 'selected');
                                    if (schEditSlot.params.ResourceId != "")
                                        $("#pnlScheduleCalendar #Resource option[value=" + schEditSlot.params.ResourceId + "]").attr('selected', 'selected');
                                    if (schEditSlot.params.FacilityId != "")
                                        $("#pnlScheduleCalendar #Facility option[value=" + schEditSlot.params.FacilityId + "]").attr('selected', 'selected');
                                    if (schEditSlot.params.DayDate != "")
                                        $('#pnlScheduleCalendar #daydate span').html(schEditSlot.params.DayDate);
                                }
                                //------------------------------------------------                               
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

    UpdateSlotInfo: function () {

        var strMessage = "";
        var self = $("#schEditSlot");
        var myJSON = self.getMyJSON();
        var patternJSON;

        AppPrivileges.GetFormPrivileges("Slot", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                schEditSlot.UpdateScheduleSlot(myJSON, schEditSlot.params.TimeslotDetailid).done(function (response) {
                    if (response.status != false) {
                        //Admin_ProviderSchedule.ProviderScheduleSearch(appointmentDetail.params.AppointmentId);
                        utility.DisplayMessages(response.message, 1);
                        UnloadActionPan();

                        var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                        //expression for week range
                        var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;

                        if (schEditSlot.params.PanelID == "pnlScheduleMuliView") {

                            var providerid = schEditSlot.params.ProviderId;
                            var facilityid = schEditSlot.params.FacilityId;
                            var resourceid = schEditSlot.params.ResourceId;
                            var date = schEditSlot.params.DayDate;
                            var dateid = schEditSlot.params.DateId;

                            Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0, null);


                        }
                        else if (schEditSlot.params.DayDate.match(weekrg) && schEditSlot.params.DayDate.length > 15) {
                            var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                            Scheduling_Calendar.ClearTable();

                            var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                            var week = Scheduling_Calendar.GetWeek(date1);
                            $('#pnlScheduleCalendar #daydate span').html(week);
                            //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                            var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                            $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                        }
                        else if (schEditSlot.params.DayDate.match(dayrgx) && schEditSlot.params.DayDate.length < 15) {
                            var statusslots = Scheduling_Calendar.FilterCriteria();
                            Scheduling_Calendar.DayCalendar(schEditSlot.params.ProviderId, schEditSlot.params.ResourceId, schEditSlot.params.FacilityId, schEditSlot.params.DayDate, statusslots);
                            if (schEditSlot.params.ProviderId != "")
                                $("#pnlScheduleCalendar #Provider option[value=" + schEditSlot.params.ProviderId + "]").attr('selected', 'selected');
                            if (schEditSlot.params.ResourceId != "")
                                $("#pnlScheduleCalendar #Resource option[value=" + schEditSlot.params.ResourceId + "]").attr('selected', 'selected');
                            if (schEditSlot.params.FacilityId != "")
                                $("#pnlScheduleCalendar #Facility option[value=" + schEditSlot.params.FacilityId + "]").attr('selected', 'selected');
                            if (schEditSlot.params.DayDate != "")
                                $('#pnlScheduleCalendar #daydate span').html(schEditSlot.params.DayDate);
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
    },

    AppointmentEdit: function (appid, visitid, checkin) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                var params = [];
                params["AppointmentId"] = appid;
                params["checkin"] = checkin;
                params["ProviderId"] = schEditSlot.params.ProviderId;
                params["ResourceId"] = schEditSlot.params.ResourceId;
                params["FacilityId"] = schEditSlot.params.FacilityId;
                params["DateId"] = schEditSlot.params.DateId;
                params["DayDate"] = schEditSlot.params.DayDate;
                params["TimeslotDetailid"] = schEditSlot.params.TimeslotDetailid;
                params["MultipleView"] = schEditSlot.params.MultipleView;
                params["PatientVisitId"] = visitid;
                params["mode"] = "Edit";
                params["ParentCtrl"] = "schEditSlot";
                LoadActionPan('appointmentDetail', params);

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    SelectSchSlotDetail: function (TimeslotDetailid, ProviderId, ResourceId) {
        var data = "TimeslotDetailid=" + TimeslotDetailid + "&ProviderId=" + ProviderId + "&ResourceId=" + ResourceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_SELECT_SLOT_DETAIL", "SELECT_SLOT_DETAIL");
    },

    UpdateScheduleSlot: function (SlotData, TimeSlotDtlId) {

        var ProviderId;
        var ResourceId;

        ProviderId = schEditSlot.params.ProviderId;
        ResourceId = schEditSlot.params.ResourceId;
        var data = "SlotData=" + SlotData + "&TimeSlotDtlId=" + TimeSlotDtlId + "&ProviderId=" + ProviderId + "&ResourceId=" + ResourceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_SELECT_SLOT_DETAIL", "UPDATE_SCH_SLOT");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmSchSlotEdit', function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },

    //-------------------------------------------------

    OpenCoPayment: function (AppId, PatId, VisitId) {

        if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {
            utility.DisplayMessages("Copayment can't be collected for Resource Appointmnet", 3);
        }
        else {
            var params = [];
            params["FromAdmin"] = "0";
            params["AppointmentId"] = AppId;
            //params["PatientVisitId"] = appointmentDetail.params.PatientVisitId;
            params["ProviderId"] = schEditSlot.params.ProviderId;
            params["FacilityId"] = schEditSlot.params.FacilityId;
            //params["TimeslotDetailid"] = schEditSlot.params.TimeslotDetailid;
            params["PatientVisitId"] = VisitId;
            params["PatientId"] = PatId;
            params["ParentCtrl"] = 'schEditSlot';
            params["DayDate"] = schEditSlot.params.DayDate;
            LoadActionPan('schcopayment', params);
        }

    },

    LoadCheckIn: function (appid, patientid) {

        if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

            utility.DisplayMessages("Resource Appointment can't CheckedIn", 3);
        }
        else {

            var params = [];
            params["FromAdmin"] = "0";
            params["ProviderId"] = schEditSlot.params.ProviderId;
            params["ResourceId"] = null;
            params["FacilityId"] = schEditSlot.params.FacilityId;
            params["DayDate"] = schEditSlot.params.DayDate;
            params["TimeslotDetailid"] = schEditSlot.params.TimeslotDetailid;
            params["MultipleView"] = schEditSlot.params.MultipleView;
            params["DateId"] = schEditSlot.params.DateId;
            params["AppointmentId"] = appid;
            params["PatientId"] = patientid;
            params["ParentCtrl"] = 'schEditSlot';
            LoadActionPan('schcheckin', params);

        }


    },

    LoadCheckOut: function (appid, patientid, patientvisitid) {

        if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

            utility.DisplayMessages("Resource Appointment can't CheckedOut", 3);
        }

        else {
            var params = [];
            params["FromAdmin"] = "0";
            params["ProviderId"] = schEditSlot.params.ProviderId;
            params["ResourceId"] = null;
            params["FacilityId"] = schEditSlot.params.FacilityId;
            params["DayDate"] = schEditSlot.params.DayDate;
            params["TimeslotDetailid"] = schEditSlot.params.TimeslotDetailid;
            params["MultipleView"] = schEditSlot.params.MultipleView;
            params["DateId"] = schEditSlot.params.DateId;
            params["AppointmentId"] = appid;
            params["PatientId"] = patientid;
            params["PatientVisitId"] = patientvisitid;
            params["ParentCtrl"] = 'schEditSlot';
            LoadActionPan('schcheckout', params);

        }
    },

    //--------------------------------------------------

    BindScheduleReasons: function () {
        var SchReason = $('#schEditSlot #txtSchReason').val();
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

            $('#schEditSlot #txtSchReason').autocomplete({
                autoFocus: true,
                source: AllSchReasons,
                open: function (event, ui) { disable = true },
                close: function (event, ui) {
                    disable = false; $(this).focus();
                },
                select: function (event, ui) {
                    setTimeout(function () {
                        $('#schEditSlot #txtSchReason').val(ui.item.value);
                        $('#schEditSlot #hfSchReasonId').val(ui.item.id);
                    }, 100);

                }
            }).blur(function () {
                setTimeout(function () {
                    utility.ValidateAutoComplete($('#schEditSlot #txtSchReason'), 'schEditSlot #hfSchReasonId', false, null, null, null);
                }, 200);
            });
            $('#schEditSlot #txtSchReason').autocomplete("search");

        });

        //--------------------
    },

    OpenScheduleReason: function () {

        var params = [];
        params["ScheduleReasonId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schEditSlot";
        LoadActionPan('Admin_ScheduleReason', params);

    },

    FillScheduleReason: function (ScheduleReasonId, ShortName, Duration, event) {

        if (event != null) {
            event.stopPropagation();
        }

        UnloadActionPan("schEditSlot");

        $('#schEditSlot #txtSchReason').val(ShortName);
        $('#schEditSlot #hfSchReasonId').val(ScheduleReasonId);

    },
}