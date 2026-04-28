Scheduling_BlockAppointment_Summary = {
    params: [],

    Load: function (params) {

        Scheduling_BlockAppointment_Summary.params = params;

        utility.ValidateFromToDate('pnlBlockAppointmentSummary #frmBlockAppointmentSearch', 'dpfromDate', 'dptoDate', true);

        //$("#frmAppointmentSummarySearch #hdnFacilityId").val(Scheduling_BlockAppointment_Summary.params.FacilityId);
        //$("#frmAppointmentSummarySearch #hdnProviderId").val(Scheduling_BlockAppointment_Summary.params.ProviderId);
        //$("#frmAppointmentSummarySearch #hdnResourceId").val(Scheduling_BlockAppointment_Summary.params.ResourseId);

        Scheduling_BlockAppointment_Summary.LoadBlockAppointment();
    },

    UnLoad: function () {

        UnloadActionPan(Scheduling_BlockAppointment_Summary.params["ParentCtrl"], "pnlBlockAppointmentSummary");

    },


    LoadBlockAppointment: function () {
        var data = Scheduling_BlockAppointment_Summary.params;
        var fromDate = "";
        var toDate = "";
        var facilityId = 0;
        var providerId = "";
        var resourceId = "";
        if (data != undefined && data != null) {
            //facilityId = data["FacilityId"];
            //providerId = data["ProviderId"];
            //resourceId = data["ResourceId"];

            facilityId = Scheduling_BlockAppointment_Summary.params.FacilityId;
            providerId = Scheduling_BlockAppointment_Summary.params.ProviderId;
            resourceId = Scheduling_BlockAppointment_Summary.params.ResourceId;

            if (data["DayDate"].indexOf("-") >= 0) { // for week 
                var dateStr = data["DayDate"].split('-');
                fromDate = dateStr[0].trim();
                toDate = dateStr[1].trim();
            }
            else if ((data["DayDate"].match(/\//g) || []).length == 2) {// for day
                fromDate = data["DayDate"].trim();
                toDate = data["DayDate"].trim();
            }
            else {
                var mont = data["DayDate"].split('/');
                var dateRang = Scheduling_BlockAppointment_Summary.CheckDateRangeForMonth(mont[0].toUpperCase(), mont[1]);
                if (dateRang.indexOf("-") >= 0) {
                    var dateStr = dateRang.split('-');
                    fromDate = dateStr[0].trim();
                    toDate = dateStr[1].trim();
                }
            }
        }
        var jsonData = '{"FacilityId" :"' + facilityId + '", "ProviderId" :"' + providerId + '", "FromDate" : "' + fromDate + '", "ToDate" :"' + toDate + '", "ResourceId" :"' + resourceId + '"}';
        Scheduling_BlockAppointment_Summary.BlockAppointmentLoad(jsonData);
    },

    SearchBlockAppSummary: function (jsonData) {
        var data = "SearchBlockedAppSummaryData=" + jsonData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_SUMMARY", "SCHEDULING_BLOCK_APP_SUMMARY");
    },
    tConvert: function (time) {
        // Check correct time format and split into components
        time = time.toString().match(/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [time];

        if (time.length > 1) { // If time format correct
            time = time.slice(1); // Remove full string match value
            time[5] = +time[0] < 12 ? ' AM' : ' PM'; // Set AM/PM
            time[0] = +time[0] % 12 || 12; // Adjust hours
        }
        return time.join(''); // return adjusted time or original string
    },
    CheckDateRangeForMonth: function (month, year) {
        if (month == "JANUARY" || month == "MARCH" || month == "MAY" || month == "JULY" || month == "AUGUST" || month == "OCTOBER" || month == "DECEMBER") {// month that have 31 day in a month
            if (month == "JANUARY") {
                return '01/01/' + year + ' - 01/31/' + year;
            } else if (month == "MARCH") {
                return '03/01/' + year + ' - 03/31/' + year;
            } else if (month == "MAY") {
                return '05/01/' + year + ' - 05/31/' + year;
            } else if (month == "JULY") {
                return '07/01/' + year + ' - 07/31/' + year;
            } else if (month == "AUGUST") {
                return '08/01/' + year + ' - 08/31/' + year;
            } else if (month == "OCTOBER") {
                return '10/01/' + year + ' - 10/31/' + year;
            } else if (month == "DECEMBER") {
                return '12/01/' + year + ' - 12/31/' + year;
            }

        }
        else if (month == "APRIL" || month == "JUNE" || month == "SEPTEMBER" || month == "NOVEMBER") {
            if (month == "APRIL") {
                return '04/01/' + year + ' - 04/30/' + year;
            } else if (month == "JUNE") {
                return '06/01/' + year + ' - 06/30/' + year;
            } else if (month == "SEPTEMBER") {
                return '09/01/' + year + ' - 09/30/' + year;
            } else if (month == "NOVEMBER") {
                return '11/01/' + year + ' - 11/30/' + year;
            }
        } else if (month == "FEBRUARY") {
            if (utility.LeapYear(year)) {
                return '02/01/' + year + ' - 02/29/' + year;
            } else {
                return '02/01/' + year + ' - 02/28/' + year;
            }
        }
    },

    BlockAppointmentLoad: function (jsonData) {
        //var appointmentSummaryData = "";
        $('#pnlBlockAppointmentSummary #tblBlockAppointment_Summary tbody tr').remove();
        if ($("#pnlBlockAppointmentSummary #tblBlockAppointment_Summary").css("display") == "none") {
            $("#pnlBlockAppointmentSummary #tblBlockAppointment_Summary").show();
        }
        Scheduling_BlockAppointment_Summary.SearchBlockAppSummary(jsonData).done(function (response) {
            if (response.status != false) {
                if (response.AppointmentCount > 0 && response.AppointmentSummaryLoad_JSON != undefined && response.AppointmentSummaryLoad_JSON != "") {

                    Scheduling_BlockAppointment_Summary.BlockAppGridLoad(response);

                    var appointSum = JSON.parse(response.AppointmentSummaryLoad_JSON);


                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
            var jsonjObj = JSON.parse(jsonData);
            $('#pnlBlockAppointmentSummary #dpfromDate').datepicker('update', jsonjObj.FromDate);
            $('#pnlBlockAppointmentSummary #dptoDate').datepicker('update', jsonjObj.ToDate);
        });
    },

    BlockAppGridLoad: function (response) {
        $("#dgvBlockAppointment").dataTable().fnDestroy();
        $("#tblBlockAppointment_Summary #dgvBlockAppointment tbody").find("tr").remove();
        if (response.AppointmentCount > 0) {
            var AppointmentSummaryLoadJSONData = JSON.parse(response.AppointmentSummaryLoad_JSON);
            $.each(AppointmentSummaryLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvWaitList_row" + i);
                $row.attr("onclick", "utility.SelectGridRow($('#gvWaitList_row" + i + "'))");
                if (item.IsActive == "1") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                var parameter = item.AppointmentId + ",'" + item.AppointmentDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + "'";
                var fromtime = Scheduling_BlockAppointment_Summary.tConvert(item.TimeFrom.slice(0, 5));
                var totime = Scheduling_BlockAppointment_Summary.tConvert(item.TimeTo.slice(0, 5));
                $row.append('<td style="display:none;">' + item.AppointmentId + '</td><td><a class="btn btn-xs" href="#" onclick="Scheduling_BlockAppointment_Summary.OpenRescheduleAppointment(' + parameter + ');" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;</td> <td>' + item.PatientName + '</td> <td>' + item.AppointmentDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td> <td>' + fromtime + '</td> <td>' + totime + '</td> <td>' + item.ProviderName + '</td>');
                $("#tblBlockAppointment_Summary #dgvBlockAppointment tbody").last().append($row);
            });
        }
        else {
            $('#dgvBlockAppointment').DataTable({
                "language": {
                    "emptyTable": "No Appointment Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvBlockAppointment'))
            ;
        else
            $("#tblBlockAppointment_Summary #dgvBlockAppointment").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchAppointmentWithDatePicker: function () {
        var fromDate = $('#pnlBlockAppointmentSummary #dpfromDate').val();
        var toDate = $('#pnlBlockAppointmentSummary #dptoDate').val();
        var data = Scheduling_BlockAppointment_Summary.params;
        var facilityId = data["FacilityId"];
        var providerId = data["ProviderId"];
        var resourceId = data["ResourceId"];

        if (fromDate != "" && toDate != "" && facilityId != undefined && providerId != undefined) {
            var jsonData = '{"FacilityId" :"' + facilityId + '", "ProviderId" :"' + providerId + '", "FromDate" : "' + fromDate + '", "ToDate" :"' + toDate + '", "ResourceId" :"' + resourceId + '"}';
            Scheduling_BlockAppointment_Summary.BlockAppointmentLoad(jsonData);
        }
        else if (fromDate == "" || fromDate == null || fromDate == undefined) {
            utility.DisplayMessages("Select From Date", 3);
        } else if (toDate == "" || toDate == null || toDate == undefined) {
            utility.DisplayMessages("Select To Date", 3);
        }
    },

    OpenRescheduleAppointment: function (appID,appDate) {
        var params = [];
        var facilityId = $('#pnlScheduleCalendar #Facility').val();
        var providerId = $("#pnlScheduleCalendar #frmSchedulingCalendar #rdprovider").prop("checked") == true ? $('#pnlScheduleCalendar #Provider').val() : "0";
        var resourceId = $("#pnlScheduleCalendar #frmSchedulingCalendar #rdresource").prop("checked") == true ? $('#pnlScheduleCalendar #Resource').val() : "0";
        var datedDate = $('#pnlScheduleCalendar #daydate').text().trim();

        params["ProviderId"] = providerId;
        params["ResourceId"] = resourceId;
        params["FacilityId"] = facilityId;
        params["DayDate"] = datedDate;
        params["AppointmentId"] = appID;
        params["AppointmentDate"] = appDate;
        params["ParentCtrl"] = "Scheduling_BlockAppointment_Summary";
        LoadActionPan('Scheduling_RescheduleAppointment', params);


    },
};