Patient_Fax_Log = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Patient_Fax_Log.bIsFirstLoad == true) {
            Patient_Fax_Log.params = params;
            Patient_Fax_Log.params.PatientId = $('#PatientProfile #hfPatientId').val();
            if (!Patient_Fax_Log.params.PatientId) {
                Patient_Fax_Log.params.PatientId = 0;
            }
            Patient_Fax_Log.FaxLogSearch(Patient_Fax_Log.params.PatientId);
        }
    },

    FaxLogSearch: function (PatientId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Patient_Fax_Log.SearchFaxLog(Patient_Fax_Log.params.PatientId).done(function (response) {
                    Patient_Fax_Log.LoadFaxLogGrid(response);
                });

            }
        });
    },
    SearchFaxLog: function (PatientId) {
        var data = null;
        data = "PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "Patient_Fax_Log", "LOAD_FAX_LOG");
    },
    UnLoad: function () {

    },
    LoadFaxLogGrid: function (response) {
        $("#dgvFaxLog").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#pnlPatientFaxLog #dgvFaxLog tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.FaxLogCount > 0) {
            $.each(response.FaxLogData, function (i, item) {
                var $row = $('<tr/>');
                $row.append('<td>' + item.ToFaxNumber + '</td><td>' + item.SentStatus + '</td><td>' + item.DateAndTime + '</td><td>' + item.RecipientName + '</td><td>' + item.Pages + '</td><td>' + item.SenderName + '</td><td>' + item.Subject + '</td>');

                $("#pnlFaxLog_Result #dgvFaxLog tbody").last().append($row);
            });
        }
        else {
            $('#pnlFaxLog_Result #dgvFaxLog').DataTable({
                "language": {
                    "emptyTable": "No Fax Log Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false }]
            });
        }
        //Creating Data Table Instance
        if ($.fn.dataTable.isDataTable('#pnlFaxLog_Result #dgvFaxLog'))
            ;
        else {
            $("#pnlFaxLog_Result #dgvFaxLog").DataTable({ "bInfo": true, "bSort": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false }] }); // to remove records per page dropdown


        }
    },
};