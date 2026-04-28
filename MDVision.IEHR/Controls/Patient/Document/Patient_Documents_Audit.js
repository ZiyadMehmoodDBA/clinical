/*
    Author: Balawal Khan
    Creation Date: Feb 27,2018
    OverView:This File Is created for Documents Audit Component
*/

Patient_Documents_Audit = {
    bIsFirstLoad: true,
    params: [],
    AuditData: [],
    Load: function (params) {

        Patient_Documents_Audit.params = params;
        Patient_Documents_Audit.AuditData = [];

        Patient_Documents_Audit.DocumentAuditLoad(Patient_Documents_Audit.params.DocID);
        if (Patient_Documents_Audit.bIsFirstLoad) {

           
        }

    },

    DocumentAuditLoad: function (DocID, PageNumber, ResultPerPage) {

        Patient_Documents_Audit.LoadComponentAudit_DBCall(DocID, PageNumber, ResultPerPage).done(function (response) {
           // response = JSON.parse(response);
            if (response.status != false) {
                Patient_Documents_Audit.LoadComponentAuditGrid(response);

                var TableControl = $("#pnlPatientDocument #dgvUserActions");
                var PagingPanelControlID = Patient_Documents_Audit.params["PanelID"] + " #dvgPatientDocumentsAudit_Paging";
                var ClassControlName = "Patient_Documents_Audit";
                var PagesToDisplay = 15;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.iTotalDisplayRecords, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (DocID, PageNumber, ResultPerPage) {
                        Patient_Documents_Audit.DocumentAuditLoad(Patient_Documents_Audit.params.DocID, PageNumber, ResultPerPage);
                    }), 10);
            }
        });
    },

    LoadComponentAuditGrid: function (response) {

        //Bind Data in Table
        $("#pnlPatient_Documents_Audit #dgvUserActions").dataTable().fnDestroy();
        $("#" + Patient_Documents_Audit.params["PanelID"] + " #pnlActivityLog_User #dgvUserActions tbody").find("tr").remove();
        Patient_Documents_Audit.AuditData = [];

        if (response.DocumentActivity.length > 0) {
            $.each(response.DocumentActivity, function (i, item) {
                var $row = $('<tr/>');
              
                $row.append('<td>' + item.UserName
                    + '</td><td>' + item.ActionName + '</td><td>'
                    + item.CreatedOn + '</td>');

                $("#pnlPatient_Documents_Audit #dgvUserActions tbody").last().append($row);
            });

        }
        else {
            
            $('#pnlPatient_Documents_Audit #dgvUserActions').DataTable({
                "language": {
                    "emptyTable": "No Record Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable('#pnlPatient_Documents_Audit #dgvUserActions'))
            ;
        else
            $("#pnlPatient_Documents_Audit #dgvUserActions").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": true, "bPaginate": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },


    LoadComponentAudit_DBCall: function (PatDocID, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var data = "PatDocId=" + PatDocID+"&pageNumber=" + PageNumber + "&rowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DOCUMENT_ACTIVITY");
    },

    UnLoad: function () {
        UnloadActionPan(Patient_Documents_Audit.params["ParentCtrl"], "Patient_Documents_Audit", null, Patient_Documents_Audit.params["ParentCtrlPanelID"]);       
    },

}
