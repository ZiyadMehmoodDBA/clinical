Bill_FollowUpARHistory = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Bill_FollowUpARHistory.params = params;

        if (Bill_FollowUpARHistory.params.PanelID != "pnlBillFollowUpARHistory")
            Bill_FollowUpARHistory.params.PanelID = Bill_FollowUpARHistory.params.PanelID + ' #pnlBillFollowUpARHistory';

        if (Bill_FollowUpARHistory.bIsFirstLoad) {
            Bill_FollowUpARHistory.bIsFirstLoad = false;

            $('#' + Bill_FollowUpARHistory.params.PanelID).loadDropDowns(true).done(function () {

                if (Bill_FollowUpARHistory.params.FollowUpARType != null && Bill_FollowUpARHistory.params.FollowUpARType != "" && Bill_FollowUpARHistory.params.FollowUpARID != null && Bill_FollowUpARHistory.params.FollowUpARType != "") {
                    Bill_FollowUpARHistory.ARHistorySearch(Bill_FollowUpARHistory.params.FollowUpARType, Bill_FollowUpARHistory.params.FollowUpARID);
                }
            });
        }

    },





    ARHistorySearch: function (FollowUpARType, FollowUpARID, PageNo, rpp) {
        var self = "";
        self = $('#' + Bill_FollowUpARHistory.params.PanelID);
        var myJSON = self.getMyJSON();
        Bill_FollowUpARHistory.SearchARHistory(myJSON, FollowUpARType, FollowUpARID, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                if ($("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARGroupHistory_Result").css("display") == "none") {
                    $("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARGroupHistory_Result").show();
                }
                if ($("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARActionHistory_Result").css("display") == "none") {
                    $("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARActionHistory_Result").show();
                }
                if ($("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARReasonHistory_Result").css("display") == "none") {
                    $("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARReasonHistory_Result").show();
                }

                if ($("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARRemitCodeHistory_Result").css("display") == "none") {
                    $("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARRemitCodeHistory_Result").show();
                }
                   
                Bill_FollowUpARHistory.GridLoad(response.GroupHistory_JSON, response.GroupHistoryCount, "GroupHistory", "#" + Bill_FollowUpARHistory.params["PanelID"] + " #frmBillFollowUpARHistory #pnlARGroupHistory_Result #dgvARGroupHistory");
                Bill_FollowUpARHistory.GridLoad(response.ActionHistory_JSON, response.GroupHistoryCount, "ActionHistory", "#" + Bill_FollowUpARHistory.params["PanelID"] + " #frmBillFollowUpARHistory #pnlARActionHistory_Result #dgvARActionHistory");
                Bill_FollowUpARHistory.GridLoad(response.ReasonHistory_JSON, response.ReasonHistoryCount, "ReasonHistory", "#" + Bill_FollowUpARHistory.params["PanelID"] + " #frmBillFollowUpARHistory #pnlARReasonHistory_Result #dgvARReasonHistory");
                Bill_FollowUpARHistory.GridLoad(response.RemitCodeHistory_JSON, response.RemitCodeHistoryCount, "RemitCodeHistory", "#" + Bill_FollowUpARHistory.params["PanelID"] + " #frmBillFollowUpARHistory #pnlARRemitCodeHistory_Result #dgvARRemitCodeHistory");
                // Bill_FollowUpARHistory.ARGroupHistoryGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        Bill_FollowUpARHistory.SearchARCallHistory(FollowUpARID,FollowUpARType, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                Bill_FollowUpARHistory.CallHistoryGridLoad(response.FollowupARCall_JSON, response.FollowupARCallCount, "CallHistory", "#" + Bill_FollowUpARHistory.params["PanelID"] + " #frmBillFollowUpARHistory #pnlARCallHistory_Result #dgvARCallHistory");
                if (response.FollowupARCallCount > 0) {
                    $('#' + Bill_FollowUpARHistory.params.PanelID + " #divARCallHistoryPaging").css("display", "inline");
                    //Showing 1 to 15 of 15 entries
                    var RecordsPerPage = rpp != null ? rpp : 5;
                    var CurrentPage = PageNo != null ? PageNo : 1;

                    var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                    var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                    if (PageNo == null) {
                        utility.GetCustomPaging(Bill_FollowUpARHistory.params.PanelID + " #divARCallHistoryPaging", response.iTotalDisplayRecords, 5, "Bill_FollowUpARHistory", CurrentPage, RecordsPerPage);
                    }
                    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                    $("#" + Patient_Search.params["PanelID"] + " #divARCallHistoryPaging #divShowingEntries").text(showingText);
                    // Change Background Color to Black for selected page
                    $("#" + Patient_Search.params["PanelID"] + " li").each(function () {
                        if ($(this).text() == CurrentPage) {
                            $(this).attr("class", "active");
                        }
                        else
                            $(this).removeAttr("class");
                    });
                }
                else
                {
                    $('#' + Bill_FollowUpARHistory.params.PanelID + " #divARCallHistoryPaging").css("display", "none");
                }
             
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },



    ARGroupHistoryGridLoad: function (response) {
        $("#" + Bill_FollowUpARHistory.params.PanelID + " #pnlARGroupHistory_Result #dgvARGroupHistory").dataTable().fnDestroy();
        $("#" + Bill_FollowUpARHistory.params.PanelID + " #pnlARGroupHistory_Result #dgvARGroupHistory tbody").find("tr").remove();
        if (response.ARVisitCount > 0) {
            var PatientARJSONData = JSON.parse(response.ARVisitLoad_JSON);
            $.each(PatientARJSONData, function (i, item) {
                var $row = $('<tr/>');
                
                $row.attr("id", "gvPatientAR_row" + item.FolUpARDtlId);
                $row.attr("PatARDetailId", item.FolUpARDtlId);
                if (item.IsActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                var deleteActionMethod = '';

                var editMethod = "Bill_FollowUpPatientAR.OpenPatientARDetail('" + item.VisitId + "','" + item.FolUpARDtlId + "','" + item.PatientId + "','" + item.AccountNumber + "','" + item.LastName + "','" + item.FirstName + "','" + item.InsurancePlanId + "','" + item.ProviderName + "','" + item.ProviderId + "',event)";
                var editActionMarkup = '<a class="btn btn-xs" href="#" onclick="' + editMethod + '" title="AR Detail"><i class="fa fa-book"></i></a>';
                $row.attr("onclick", editMethod);
                // var editActionMethod = '<a class="btn btn-xs" href="#" onclick="Bill_FollowUpPatientAR.OpenPatientARDetail(' + item.VisitId + ',' + item.FolUpARDtlId + ');" title="AR Detail"><i class="fa fa-book"></i></a>';
                var selectActionMethod = '';

                $row.append('<td style="display:none;">' + item.FolUpARDtlId + '</td><td>' + deleteActionMethod + '&nbsp;' + editActionMarkup + selectActionMethod + '</td> <td>' + item.PracticeName + '</td> <td>' + item.FacilityName + '</td> <td>' + item.ProviderName + '</td><td>' + item.InsurancePlanName + '</td><td>' + item.AccountNumber + '</td><td>' + item.ClaimNumber + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.ARGroupName + '</td><td>' + item.FollowupActionName + '</td><td>' + item.FollowupReasonName + '</td><td>' + item.Suspended + '</td><td>' + utility.RemoveTimeFromDate(null, item.FirstStatementDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.StatementDate) + '</td><td>' + item.AdditionalComments + '</td>');

                $("#" + Bill_FollowUpPatientAR.params.PanelID + " #pnlARGroupHistory_Result #dgvARGroupHistory tbody").last().append($row);
            });
        }
        else {
            $('#' + Bill_FollowUpARHistory.params.PanelID + " #"+divARGroupHistoryPaging).css("display", "none");
            $("#" + Bill_FollowUpARHistory.params.PanelID + " #pnlARGroupHistory_Result #dgvARGroupHistory").DataTable({
                "language": {
                    "emptyTable": "No AR Group History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + Bill_FollowUpARHistory.params.PanelID + " #pnlBillFollowUpPatientAR_Result #dgvFollowUpPatientAR"))
            ;
        else
            $("#" + Bill_FollowUpARHistory.params.PanelID + " #pnlARGroupHistory_Result #dgvARGroupHistory").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },



    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {
        Bill_FollowUpARHistory.ARHistorySearch(Bill_FollowUpARHistory.params.FollowUpARType, Bill_FollowUpARHistory.params.FollowUpARID, PageNo, 5);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $('#' + ARGroupHistory.params.PanelID + " #pnlARGroupHistory_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_FollowUpARHistory.ARHistorySearch(Bill_FollowUpARHistory.params.FollowUpARType, Bill_FollowUpARHistory.params.FollowUpARID, PageNo, 15);
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $('#' + Bill_FollowUpARHistory.params["PanelID"] + " #pnlARGroupHistory_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_FollowUpARHistory.ARHistorySearch(Bill_FollowUpARHistory.params.FollowUpARType, Bill_FollowUpARHistory.params.FollowUpARID, PageNo, 15);
        }
    },



    // _____________________DB Calls___________________________________//
    SearchARHistory: function (searchData, FollowUpARType, FollowUpARID, pageNo, recordPerPage) {
        if (pageNo == null) {
            pageNo = 1;
        }
        if (recordPerPage == null) {
            recordPerPage = 15;
        }
        var data;
        Data = data = "searchData=" + searchData + "&FollowUpARType=" + FollowUpARType + "&FollowUpARID=" + FollowUpARID + "&pageNo=" + pageNo + "&recordPerPage=" + recordPerPage;
        if (Bill_FollowUpARHistory.params.VisitId != null && Bill_FollowUpARHistory.params.VisitId!="")
            var data = "searchData=" + searchData + "&FollowUpARType=" + FollowUpARType + "&FollowUpARID=" + FollowUpARID + "&pageNo=" + pageNo + "&recordPerPage=" + recordPerPage + "&VisitId=" + Bill_FollowUpARHistory.params.VisitId;
        return MDVisionService.defaultService(data, "BILLING_FOLLOWUP_HISTORY", "SEARCH_FOLLOWUP_HISTORY");
    },

    SearchARCallHistory: function (FollowUpARID, FollowUpARType, pageNo, recordPerPage) {
        if (pageNo == null || pageNo == undefined) {
            pageNo = 1;
        }
        if (recordPerPage == null || recordPerPage == undefined) {
            recordPerPage = 5;
        }
        var data;
        Data = data = "FollowUpARType=" + FollowUpARType + "&FollowUpARID=" + FollowUpARID + "&PageNumber=" + pageNo + "&RowsPerPage=" + recordPerPage;
  
        return MDVisionService.defaultService(data, "BILLING_FOLLOWUP_AR_CALL", "LOAD_FOLLOWUP_AR_CALL");
    },
    // ______________________End DB Calls__________________________//





    // ______________________UNLOADING__________________________//
    UnLoad: function () {

        if (Bill_FollowUpARHistory.params != null && Bill_FollowUpARHistory.params.ParentCtrl) {
            UnloadActionPan(Bill_FollowUpARHistory.params.ParentCtrl);
        }
        else
            UnloadActionPan();
    },

    GridLoad: function (response, count, gridType, gridId) {
        if ($.fn.dataTable.isDataTable(gridId))
            $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();

        var emptyTableMsg = "";
        if (gridType == "GroupHistory") {
            if ($("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARGroupHistory_Result").css("display") == "none") {
                $("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARGroupHistory_Result").show();
            }
            emptyTableMsg = "No Group History Found";
        }
        else if (gridType == "ActionHistory") {
            if ($("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARActionHistory_Result").css("display") == "none") {
                $("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARActionHistory_Result").show();
            }
            emptyTableMsg = "No Action History Found";
        }
        else if (gridType == "ReasonHistory") {
            if ($("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARReasonHistory_Result").css("display") == "none") {
                $("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARReasonHistory_Result").show();
            }
            emptyTableMsg = "No Reason History Found";
        }
        else if (gridType == "RemitCodeHistory") {
            if ($("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARRemitCodeHistory_Result").css("display") == "none") {
                $("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARRemitCodeHistory_Result").show();
            }
            emptyTableMsg = "No Remit Code History Found";
        }
        if (count > 0) {
            var firstRowId = "";
            var PatientLoadJSONData = JSON.parse(response);
            $.each(PatientLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                var _rowId = "";
                var emptyTableMsg = "";
                if (gridType == "GroupHistory") {
                    _rowId = "dgvARGroupHistory_row" + i;
                    $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.EntryDate + '</td><td>' + item.EnteredBy + '</td><td>' + item.OriginalGroup + '</td><td>' + item.CurrentGroup + '</td><td>' + item.GroupAge + '</td>');
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); ");
                    //-----------------Pagination------------
                }

                    //--------------------End Pagination-------------------

                else if (gridType == "ActionHistory") {
                    _rowId = "dgvARActionHistory_row" + i;
                    $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.EntryDate + '</td><td>' + item.EnteredBy + '</td><td>' + item.OriginalAction + '</td><td>' + item.CurrentAction + '</td><td>' + item.ActionAge + '</td>');
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); ");

                } else if (gridType == "ReasonHistory") {
                    _rowId = "dgvARReasonHistory_row" + i;
                    $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.EntryDate + '</td><td>' + item.EnteredBy + '</td><td>' + item.OriginalReason + '</td><td>' + item.CurrentReason + '</td><td>' + item.ReasonAge + '</td>');
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); ");
                }
                else if (gridType == "RemitCodeHistory") {
                    _rowId = "dgvARRemitCodeHistory_row" + i;
                    $row.append('<td style="display:none;">' + item.ActivityLogId + '</td><td>' + item.EntryDate + '</td><td>' + item.EnteredBy + '</td><td>' + item.OriginalRemitCode + '</td><td>' + item.CurrentRemitCode + '</td><td>' + item.CodeAge + '</td>');
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); ");

                }
                $row.attr("id", _rowId);

                $(gridId + " tbody").last().append($row);
            });
           
                if ($.fn.dataTable.isDataTable(gridId) || $(gridId).parent().parent().hasClass("dataTables_wrapper"))
                    ;
                else
                    $(gridId).DataTable({ bDestroy: true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            
        }
        else {
            if (!$(gridId).parent().parent().hasClass("dataTables_wrapper")) {
                $(gridId).DataTable({
                    bDestroy: true,
                    "language": {
                        "emptyTable": emptyTableMsg
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
                $(gridId + " tbody").last().append($row);
            }
       }
    },
    CallHistoryGridLoad: function (response, count, gridType, gridId) {

        if ($.fn.dataTable.isDataTable(gridId))
            $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();

        var emptyTableMsg = "";
        if (gridType == "CallHistory") {
            if ($("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARCallHistory_Result").css("display") == "none") {
                $("#" + Bill_FollowUpARHistory.params["PanelID"] + " #Bill_FollowUpARHistory #pnlARCallHistory_Result").show();
            }
            emptyTableMsg = "No Call History Found";
        }
        if (count > 0) {
            var firstRowId = "";
            var CallHistoryJSONData = JSON.parse(response);
            $.each(CallHistoryJSONData, function (i, item) {
                var $row = $('<tr/>');
                var _rowId = "";
                var emptyTableMsg = "";
                var details = 'Call: ' + item.CreatedBy + '  ' + item.CreatedOn + '  Duration ' + item.Duration + ' Min ' + item.Comments;
                if (gridType == "CallHistory") {
                    _rowId = "dgvARCallHistory_row" + i;
                    $row.append('<td style="display:none;">' + item.ARCallId + '</td><td>' + item.CreatedOn + '</td><td>' + item.CreatedBy + '</td><td>' + item.FollowupAction + '</td><td>' + item.FollowupReason + '</td><td>' + item.ARType + '</td><td>' + item.CStatus + '</td><td>' + item.Duration + ' Min</td><td>' + details + '</td>');
                    $row.attr("onclick", "utility.SelectGridRow($('#" + _rowId + "')); ");
                }
                if (firstRowId == "") {
                    firstRowId = _rowId;
                }
                $row.attr("id", _rowId);

                $(gridId + " tbody").last().append($row);
            });
        }
        else {
            $('#' + Bill_FollowUpARHistory.params.PanelID + " #divARCallHistoryPaging").css("display", "none");
            if (!$(gridId).parent().parent().hasClass("dataTables_wrapper")) {
                $(gridId).DataTable({
                    bDestroy: true,
                    "language": {
                        "emptyTable": emptyTableMsg
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
                $(gridId + " tbody").last().append($row);
            }
        }

    },
}