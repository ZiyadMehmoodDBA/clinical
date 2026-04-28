appointmentSummary = {
    params: [],

    Load: function (params) {

        appointmentSummary.params = params;

        utility.ValidateFromToDate('pnlAppointmentSummary #frmAppointmentSummarySearch', 'dpfromDate', 'dptoDate', true);

        $("#frmAppointmentSummarySearch #hdnFacilityId").val(appointmentSummary.params.FacilityId);
        $("#frmAppointmentSummarySearch #hdnProviderId").val(appointmentSummary.params.ProviderId);
        $("#frmAppointmentSummarySearch #hdnResourceId").val(appointmentSummary.params.ResourseId);

        appointmentSummary.AppointmentSummary();
    },

    UnLoad: function () {

        UnloadActionPan();

    },

    AppointmentSummary: function () {
        var data = appointmentSummary.params;
        var fromDate = "";
        var toDate = "";
        var facilityId = 0;
        var providerId = "";
        var resourceId = "";
        if (data != undefined && data != null) {
            facilityId = data["FacilityId"];
            providerId = data["ProviderId"];
            resourceId = data["ResourceId"];
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
                var dateRang = appointmentSummary.CheckDateRangeForMonth(mont[0].toUpperCase(), mont[1]);
                if (dateRang.indexOf("-") >= 0) {
                    var dateStr = dateRang.split('-');
                    fromDate = dateStr[0].trim();
                    toDate = dateStr[1].trim();
                }
            }
        }
        var jsonData = '{"FacilityId" :"' + facilityId + '", "ProviderId" :"' + providerId + '", "FromDate" : "' + fromDate + '", "ToDate" :"' + toDate + '", "ResourceId" :"' + resourceId + '"}';
        appointmentSummary.AppointmentSummaryDetails(jsonData);
    },

    SearchAppointmentSummary: function (data) {
        var data_ = "SearchSummaryData=" + data;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data_, "SCHEDULING_APPOINTMENT_SUMMARY", "SEARCH_APPOINTMENT_SUMMARY");
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

    AppointmentSummaryDetails: function (jsonData) {
        var appointmentSummaryData = "";
        $('#pnlAppointmentSummary #tblAppointment_Summary tbody tr').remove();
        appointmentSummary.SearchAppointmentSummary(jsonData).done(function (response) {
            if (response.status != false) {
                if (response.AppointmentCount > 0 && response.AppointmentSummaryLoad_JSON != undefined && response.AppointmentSummaryLoad_JSON != "") {
                    var appointSum = JSON.parse(response.AppointmentSummaryLoad_JSON);
                   
                    for (var i = 0; i < appointSum.length; i++) {
                        appointmentSummaryData += "<tr><td>" + appointSum[i].PatientType + "</td><td>" + appointSum[i].VisitType + "</td><td>" + appointSum[i].AppointmentCount + "</td>";
                    }

                    $('#pnlAppointmentSummary #tblAppointment_Summary').css("display", "block");
                    $('#pnlAppointmentSummary #tblAppointment_Summary tbody').append(appointmentSummaryData);



                }
                else {
                    utility.DisplayMessages(response.Message, 2);
                }

            } else {
                utility.DisplayMessages(response.Message, 3);
            }
            var jsonjObj = JSON.parse(jsonData);
            $('#pnlAppointmentSummary_Search #dpfromDate').datepicker('update', jsonjObj.FromDate);
            $('#pnlAppointmentSummary_Search #dptoDate').datepicker('update', jsonjObj.ToDate);
        });
    },

    SearchAppointmentWithDatePicker: function () {
        var fromDate = $('#pnlAppointmentSummary_Search #dpfromDate').val();
        var toDate = $('#pnlAppointmentSummary_Search #dptoDate').val();
        var data = appointmentSummary.params;
        var facilityId = data["FacilityId"];
        var providerId = data["ProviderId"];
        var resourceId = data["ResourceId"];
        
        if (fromDate != "" && toDate != "" && facilityId != undefined && providerId != undefined) {
            var jsonData = '{"FacilityId" :"' + facilityId + '", "ProviderId" :"' + providerId + '", "FromDate" : "' + fromDate + '", "ToDate" :"' + toDate + '", "ResourceId" :"' + resourceId + '"}';
            appointmentSummary.AppointmentSummaryDetails(jsonData);
        }
        else if (fromDate == "" || fromDate == null || fromDate == undefined) {
            utility.DisplayMessages("Select From Date", 3);
        } else if (toDate == "" || toDate == null || toDate == undefined) {
            utility.DisplayMessages("Select To Date", 3);
        }
    },

};