CCMProgressUpdateHistory = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        CCMProgressUpdateHistory.params = params;

        if (CCMProgressUpdateHistory.bIsFirstLoad) {
            CCMProgressUpdateHistory.bIsFirstLoad = false;
            if (CCMProgressUpdateHistory.params != null && CCMProgressUpdateHistory.params.PanelID != "pnlCCMProgressUpdateHistory") {
                CCMProgressUpdateHistory.params["PanelID"] = CCMProgressUpdateHistory.params["PanelID"] + ' #pnlCCMProgressUpdateHistory';
            }
            else {
                CCMProgressUpdateHistory.params["PanelID"] = "pnlCCMProgressUpdateHistory";
            }
            var self = $('#' + CCMProgressUpdateHistory.params["PanelID"]);
            CCMProgressUpdateHistory.setPageTitle();


            // self.loadDropDowns(true).done(function () {

            CCMProgressUpdateHistory.CCMProgressUpdateHistorySearch();
            //});


        }
    },


    setPageTitle: function () {

        var self = $('#' + CCMProgressUpdateHistory.params["PanelID"]);

        var PageTitle = "";


        var ProgressCategoryId = CCMProgressUpdateHistory.params.ProgressCategoryId

        switch (ProgressCategoryId) {

            case 1:
                PageTitle = "Goals Important to Patient";
                break;
            case 2:
                PageTitle = "Targets";
                break;
            case 3:
                PageTitle = "Potential Barriers";
                break;
            case 4:
                PageTitle = "Expected Outcomes";
                break;
            case 5:
                PageTitle = "Goals/Targets Achieved";
                break;
            case 6:
                PageTitle = "Progress reduction barriers";
                break;
            case 7:
                PageTitle = "Progress towards expected outcomes";
                break;
            case 8:
                PageTitle = "Other Information";
                break;
            default:
                PageTitle = "Progress Update History";
                break;
        }

        self.find("#pageTitle").text(PageTitle);

    },

    CCMProgressUpdateHistorySearch: function (PageNo, rpp) {
        var strMessage = "";
        //   AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + CCMProgressUpdateHistory.params["PanelID"] + ' #pnlCCMProgressUpdateHistory_Result').css("display") == "none") {
                $('#' + CCMProgressUpdateHistory.params["PanelID"] + ' #pnlCCMProgressUpdateHistory_Result').show();
            }
            CCMProgressUpdateHistory.SearchCCMProgressUpdateHistory(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    CCMProgressUpdateHistory.CCMProgressUpdateHistoryGridLoad(response);
                    var TableControl = CCMProgressUpdateHistory.params["PanelID"] + " #dgvCCMProgressUpdateHistory";
                    var PagingPanelControlID = CCMProgressUpdateHistory.params["PanelID"] + " #divPracticePaging";
                    var ClassControlName = "CCMProgressUpdateHistory";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.PracticeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        //CCMProgressUpdateHistory.PracticeSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    CCMProgressUpdateHistoryGridLoad: function (response) {
        $('#' + CCMProgressUpdateHistory.params["PanelID"] + ' #dgvCCMProgressUpdateHistory').dataTable().fnDestroy();
        $('#' + CCMProgressUpdateHistory.params["PanelID"] + ' #pnlCCMProgressUpdateHistory_Result #dgvCCMProgressUpdateHistory tbody').find("tr").remove();
        if (response.CCMProgressUpdateListCount > 0) {
            var CCMProgressUpdateHistoryJSONData = response.CCMProgressUpdateList_JSON;
            $.each(CCMProgressUpdateHistoryJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvCCMProgressUpdateHistoryJSONData_row" + item.ProgressUpdateId);

                var createdDate = "";

                if (item.CreatedOn != null && item.CreatedOn != "")
                {
                    createdDate = item.CreatedOn.split(' ')[0];
                }

                if (createdDate == "01/01/1900") {
                    createdDate = "";
                }

                $row.append('<td style="display:none;">' + item.ProgressUpdateId + '</td><td>' + item.CreatedByName + '</td><td>' + item.Value + '</td><td>' + createdDate + '</td><td>' + item.CreatedTime + '</td>');

                $('#' + CCMProgressUpdateHistory.params["PanelID"] + ' #pnlCCMProgressUpdateHistory_Result #dgvCCMProgressUpdateHistory tbody').last().append($row);
            });
        }
        else {
            $('#' + CCMProgressUpdateHistory.params["PanelID"] + ' #dgvCCMProgressUpdateHistory').DataTable({
                "language": {
                    "emptyTable": "No Progress History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + CCMProgressUpdateHistory.params["PanelID"] + ' #dgvCCMProgressUpdateHistory'))
            ;
        else
            $('#' + CCMProgressUpdateHistory.params["PanelID"] + ' #pnlCCMProgressUpdateHistory_Result #dgvCCMProgressUpdateHistory').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchCCMProgressUpdateHistory: function (PageNumber, RowsPerPage) {

        var objData = new Object();

        objData["EnrollmentInfoId"] = CCMProgressUpdateHistory.params.EnrollmentInfoId;
        objData["PatientId"] = CCMProgressUpdateHistory.params.PatientId;
        objData["ProgressCategoryId"] = CCMProgressUpdateHistory.params.ProgressCategoryId;

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "LoadProgressUpdate");
    },

    UnLoad: function () {

        if (CCMProgressUpdateHistory.params != null && CCMProgressUpdateHistory.params.ParentCtrl && CCMProgressUpdateHistory.params.ParentCtrlPanelID) {
            UnloadActionPan(CCMProgressUpdateHistory.params.ParentCtrl, "CCMProgressUpdateHistory", null, CCMProgressUpdateHistory.params.ParentCtrlPanelID);
        }
        else if (CCMProgressUpdateHistory.params != null && CCMProgressUpdateHistory.params.ParentCtrl) {
            UnloadActionPan(CCMProgressUpdateHistory.params.ParentCtrl, "CCMProgressUpdateHistory");
        }
        else {
            UnloadActionPan(null, "CCMProgressUpdateHistory");
        }

    },
}

