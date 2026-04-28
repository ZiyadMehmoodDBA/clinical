Patient_Reminder_Log = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Patient_Reminder_Log.bIsFirstLoad == true) {
            Patient_Reminder_Log.params = params;
            Patient_Reminder_Log.params.PatientId = $('#PatientProfile #hfPatientId').val();
            if(!Patient_Reminder_Log.params.PatientId){
                Patient_Reminder_Log.params.PatientId=0;
            }
            Patient_Reminder_Log.ReminderLogSearch(Patient_Reminder_Log.params.PatientId);
        }
    },

    ReminderLogSearch:function(PatientId){
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Reminder_Log.SearchReminderLog(Patient_Reminder_Log.params.PatientId).done(function (response) {
                    Patient_Reminder_Log.LoadReminderLogGrid(response);
                });

            }
        });
    },
    SearchReminderLog: function (PatientId) {
        var data = null;
        data = "PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "PATIENT_REMINDER_LOG", "LOAD_REMINDER_LOG");
    },
    UnLoad: function () {

    },
    LoadReminderLogGrid: function (response) {
        $("#dgvReminderLog").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#pnlReminderLog_Result #dgvReminderLog tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.ReminderLogCount > 0) {
            $.each(response.ReminderLogData, function (i, item) {
                var $row = $('<tr/>');
                $row.append('<td>' + item.AppointmentDate + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.ReminderType + '</td><td>' + item.AppointmentStatus + '</td><td>' + item.Status + '</td><td>' + item.ReminderResponse + '</td>');

                $("#pnlReminderLog_Result #dgvReminderLog tbody").last().append($row);
            });
        }
        else {
            $('#pnlReminderLog_Result #dgvReminderLog').DataTable({
                "language": {
                    "emptyTable": "No Reminder Log Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false }]
            });
        }
        //Creating Data Table Instance
        if ($.fn.dataTable.isDataTable('#pnlReminderLog_Result #dgvReminderLog'))
            ;
        else {
            $("#pnlReminderLog_Result #dgvReminderLog").DataTable({ "bInfo": true, "bSort": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false }] }); // to remove records per page dropdown
            

        }
    },
}