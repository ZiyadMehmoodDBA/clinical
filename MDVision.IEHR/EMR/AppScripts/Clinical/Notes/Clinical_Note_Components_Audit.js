/*
    Author: Arsalan Javed
    Creation Date: April 28,2017
    OverView:This File Is created for Clinical Notes Components Audit
*/

Clinical_Note_Components_Audit = {
    bIsFirstLoad: true,
    params: [],
    AuditData: [],
    Load: function (params) {

        Clinical_Note_Components_Audit.params = params;
        Clinical_Note_Components_Audit.AuditData = [];


        if (Clinical_Note_Components_Audit.bIsFirstLoad) {

            Clinical_Note_Components_Audit.bIsFirstLoad = false;
            Clinical_Note_Components_Audit.LoadComponentAudit();
            if (Clinical_Note_Components_Audit.params.VisitDate)
                $('#pnlNote_Components_Audit #sp_Date').text(Clinical_Note_Components_Audit.params.VisitDate);
            if (Clinical_Note_Components_Audit.params.VisitReasonComments)
                $('#pnlNote_Components_Audit #sp_Reason').text(Clinical_Note_Components_Audit.params.VisitReasonComments);
            if (Clinical_Note_Components_Audit.params.NoteType)
                $('#pnlNote_Components_Audit #sp_Type').text(Clinical_Note_Components_Audit.params.NoteType);
        }

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
    LoadComponentAudit: function (PageNo, rpp) {

        Clinical_Note_Components_Audit.LoadComponentAudit_DBCall(0,PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                //-----------------Pagination------------

                if (response.ComponentAuditCount > 0) {
                    $('#' + Clinical_Note_Components_Audit.params["PanelID"] + " #divComponentHistory").css("display", "inline");
                    //Showing 1 to 15 of 15 entries
                    var RecordsPerPage = rpp != null ? rpp : 15;
                    var CurrentPage = PageNo != null ? PageNo : 1;

                    var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                    var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                    if (PageNo == null) {
                        utility.GetCustomPaging("divComponentHistory", response.iTotalDisplayRecords, 5, "Clinical_Note_Components_Audit", CurrentPage, RecordsPerPage);
                    }
                    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                    $('#' + Clinical_Note_Components_Audit.params["PanelID"] + " #divComponentHistory #divShowingEntries").text(showingText);
                    $('#' + Clinical_Note_Components_Audit.params["PanelID"] + " #pnlActivityLog_User li").each(function () {
                        if ($(this).text() == CurrentPage) {
                            $(this).attr("class", "active");
                        }
                        else
                            $(this).removeAttr("class");
                    });
                }
                else {
                    $('#' + Clinical_Note_Components_Audit.params["PanelID"] + " #divComponentHistory").css("display", "none");
                }

                //--------------------End Pagination-------------------

                Clinical_Note_Components_Audit.LoadComponentAuditGrid(response);
            }
            else {

            }

        });
    },

    LoadComponentAuditGrid: function (response) {

        //Bind Data in Table
        $("#pnlNote_Components_Audit #dgvUserActions").dataTable().fnDestroy();
        $("#" + Clinical_Note_Components_Audit.params["PanelID"] + " #pnlActivityLog_User #dgvUserActions tbody").find("tr").remove();
        Clinical_Note_Components_Audit.AuditData = [];

        if (response.ComponentAuditCount > 0) {

            $.each(response.ComponentAudit_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this));Clinical_Note_Components_Audit.ShowDetails('" + item.NoteComponentAuditId + "')");
                $row.attr("NoteComponentAuditId", item.NoteComponentAuditId);
                if (item.ModifiedOn == null || item.ModifiedOn == undefined)
                    item.ModifiedOn = ""
                $row.append('<td>' + item.ComponentName
                    + '</td><td>' + item.ModifiedByName + '</td><td>'
                    + item.ModifiedOn.slice(0, 10) + " " + Clinical_Note_Components_Audit.tConvert(item.ModifiedOn.slice(11, 16)) + '</td><td>'
                    + item.DBAction + '</td>');

                $("#pnlNote_Components_Audit #dgvUserActions tbody").last().append($row);
                Clinical_Note_Components_Audit.AuditData.push(item);
            });
        }
        else {
            $('#' + Clinical_Note_Components_Audit.params["PanelID"] + " #divComponentHistory").css("display", "none");
            $("#pnlNote_Components_Audit #dgvUserActions").dataTable().fnDestroy();
            $("#pnlNote_Components_Audit #dgvUserActions").DataTable({
                "language": {
                    "emptyTable": "No record  found."
                }, "autoWidth": false, "bLengthChange": false, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false
            });
        }
        if ($.fn.dataTable.isDataTable("#pnlNote_Components_Audit #dgvUserActions"))
            ;
        else
            $("#pnlNote_Components_Audit #dgvUserActions").DataTable({ "bLengthChange": false, "autoWidth": false, "bFilter": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false });

    },

    //-------Pagination Functions Starts----------

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlActivityLog_User li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Clinical_Note_Components_Audit.LoadComponentAudit(PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlActivityLog_User li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Clinical_Note_Components_Audit.LoadComponentAudit(currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlActivityLog_User li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Clinical_Note_Components_Audit.LoadComponentAudit(currentPageNo, 15);
        }
    },

    //-------Pagination Functions End----------

    ShowDetails: function (NoteComponentAuditId) {

        var current_item = $.grep(Clinical_Note_Components_Audit.AuditData, function (n, i) {
            return (parseInt(n.NoteComponentAuditId) == parseInt(NoteComponentAuditId));
        });

        $("#pnlNote_Components_Audit #dgvComponentHistory tbody").find("tr").remove();

        if (current_item.length > 0) {
            $.each(current_item, function (i, item) {
                var $row = $('<tr class="disableAll  list-unstyled"/>');
                $row.append('<td>' + item.OldSOAPText
                    + '</td><td>' + item.NewSOAPText + '</td>');
                //+ '<td>' + "" + '</td>');

                $("#pnlNote_Components_Audit #dgvComponentHistory tbody").last().append($row);
            });

        }
    },

    LoadComponentAudit_DBCall: function (NoteComponentAuditId, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 15 : RowsPerPage;

        var objData = {};
        objData["NotesId"] = Clinical_Note_Components_Audit.params.NotesId ? Clinical_Note_Components_Audit.params.NotesId : 0;
        objData["NoteComponentAuditId"] = NoteComponentAuditId ? NoteComponentAuditId : 0;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PageNumber"] = PageNumber;
        objData["commandType"] = "load_note_component_audit";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "ClinicalNotes", "NotesComponentAudit");
    },

    UnLoad: function () {

        UnloadActionPan(Clinical_Note_Components_Audit.params["ParentCtrl"]);
    },

}
