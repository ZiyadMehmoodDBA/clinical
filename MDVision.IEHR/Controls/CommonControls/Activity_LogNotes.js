Activity_LogNotes = {

    params: [],

    Load: function (params) {

        Activity_LogNotes.params = params;

        Activity_LogNotes.LoadActivityLogsNotesComments();

        $("#pnlLogNotes #lblPatientName").html($("#hfPatientFullNameOnly").val());
    },

    LoadActivityLogsNotesComments: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Activity_LogNotes.ActivityLogsNotesLoad(Activity_LogNotes.params.patientID, "0", 'Patients').done(function (response) {
                    if (response.status != false) {
                        
                        Activity_LogNotes.NotesGridLoad(response);
                    }
                    else if (response.Message != "") {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ActivityLogsNotesLoad: function (patientId, visitId, tableName) {
        data = "PatientID=" + patientId + "&VisitID=" + visitId + "&DBTableName=" + tableName;
        return MDVisionService.defaultService(data, "PATIENT_ACTIVITY_LOG", "LOAD_NOTE_COMMENTS_AUDIT");
    },

    NotesGridLoad: function (response) {

        $("#pnlLogNotes #dgvLogAccountNotes").dataTable().fnDestroy();
        $("#dgvLogAccountNotes tbody").find("tr").remove();
        if (response.LogCount > 0) {
            var LogLoad_JSONData = JSON.parse(response.LogLoad_JSON);
            $.each(LogLoad_JSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td style="display:none;">' + item.DBAuditId + '</td><td>' + item.CreatedDate + '</td><td>' + item.UserName + '</td> <td>' + item.CurrentValue + '</td>');
                $("#dgvLogAccountNotes tbody").last().append($row);
            });
        }
        else {
            $('#dgvLogAccountNotes').DataTable({
                "language": {
                    "emptyTable": "No Notes Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvLogAccountNotes'))
            ;
        else
            $("#dgvLogAccountNotes").DataTable({ "bSort": false, "pageLength": 5, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


    },

    UnLoad: function () {
        if (Activity_LogNotes.params != null && Activity_LogNotes.params.ParentCtrl != null) {
            UnloadActionPan(Activity_LogNotes.params.ParentCtrl, 'Activity_LogNotes');
        }
        else
            UnloadActionPan(null, 'Activity_LogNotes');
    },
}