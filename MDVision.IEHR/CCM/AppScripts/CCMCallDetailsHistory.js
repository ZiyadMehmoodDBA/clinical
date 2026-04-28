CCMCallDetailsHistory = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        CCMCallDetailsHistory.params = params;

        if (CCMCallDetailsHistory.bIsFirstLoad) {
            CCMCallDetailsHistory.bIsFirstLoad = false;
            if (CCMCallDetailsHistory.params != null && CCMCallDetailsHistory.params.PanelID != "pnlCCMCallDetailsHistory") {
                CCMCallDetailsHistory.params["PanelID"] = CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsHistory';
            }
            else {
                CCMCallDetailsHistory.params["PanelID"] = "pnlCCMCallDetailsHistory";
            }
            var self = $('#' + CCMCallDetailsHistory.params["PanelID"]);

            // self.loadDropDowns(true).done(function () {
          
            CCMCallDetailsHistory.CCMCallDetailsHistorySearch();
            //});
        }
    },

    monthChange: function (obj) {
        var currMonth = $(obj).val();
        var currYear = $("#TaskTimerYear").val();
        CCMCallDetailsHistory.CCMCallDetailsHistorySearch(1, 1500, currMonth, currYear);
    },

    yearChange: function (obj) {
        var currMonth = $("#TaskTimerMonth").val();
        var currYear = $(obj).val();
        CCMCallDetailsHistory.CCMCallDetailsHistorySearch(1, 1500, currMonth, currYear);
    },

    CCMCallDetailsHistorySearch: function (PageNo, rpp, currMonth, currYear) {
        var strMessage = "";
        //   AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsHistory_Result').css("display") == "none") {
                $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsHistory_Result').show();
            }
            CCMCallDetailsHistory.SearchCCMCallDetailsHistory(PageNo, rpp, currMonth, currYear).done(function (response) {

                var d = new Date(), month = d.getMonth(), year = d.getFullYear();

                if (!currMonth)
                    $('#TaskTimerMonth option')[month].selected = true;
                else
                    $('#TaskTimerMonth option')[currMonth - 1].selected = true;

                for (i = new Date().getFullYear() ; i > 1900; i--)
                    $('#TaskTimerYear').append($('<option />').val(i).html(i));

                if (response.status != false)
                {

                    CCMCallDetailsHistory.CCMCallDetailsHistoryGridLoad(response);
                    var TableControl = CCMCallDetailsHistory.params["PanelID"] + " #dgvCCMCallDetailsHistory";
                    var PagingPanelControlID = CCMCallDetailsHistory.params["PanelID"] + " #divPracticePaging";
                    var ClassControlName = "CCMCallDetailsHistory";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.PracticeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        //CCMCallDetailsHistory.PracticeSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                }
                else
                {
                    utility.DisplayMessages(response.Message, 3);
                    $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsHistory').dataTable().fnDestroy();
                    $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsHistory_Result #dgvCCMCallDetailsHistory tbody').find("tr").remove();
                    $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsHistory').DataTable({
                        "language": {
                            "emptyTable": "No Log Found"
                        }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    CCMCallDetailsHistoryGridLoad: function (response) {

        var actions = "";
        $("#" + CCMCallDetailsHistory.params.PanelID + " #dgvCCMCallDetailsHistory tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, "#" + CCMCallDetailsHistory.params.PanelID + " #pnlCCMCallDetailsHistory_Result");
                }
            }
        });

        $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsHistory').dataTable().fnDestroy();
        $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsHistory_Result #dgvCCMCallDetailsHistory tbody').find("tr").remove();
        if (response.CCMCallDetailsCount > 0) {
            var CCMCallDetailsHistoryJSONData = response.CCMTaskTimer_JSON;
            $.each(CCMCallDetailsHistoryJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", item.TaskTimerAmalgamatedId);
                $row.attr("EnrollmentInfoId", CCMCallDetailsHistory.params.EnrollmentInfoId);

                var Task_Date = utility.RemoveTimeFromDate(null, item.TaskDate);

                if (Task_Date == "01/01/1900")
                    Task_Date = "";


                $row.append('<td style="display:none;">' + item.TaskTimerAmalgamatedId + '</td><td>' + actions + '</td><td>' + item.CreatedByName + '</td><td>' + item.ModifiedByName + '</td><td>' + (item.TaskReason == "- Select -" ? "" : item.TaskReason) + '</td><td>' + item.Comments + '</td><td>' + item.TaskDuration + '</td><td>' + item.Caller + '</td><td>' + item.ReceiverName + '</td><td>' + Task_Date + '</td><td>' + item.TaskTime + '</td>');

                $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsHistory_Result #dgvCCMCallDetailsHistory tbody').last().append($row);
            });
        }
        else {
            $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsHistory').DataTable({
                "language": {
                    "emptyTable": "No Calls History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsHistory'))
            ;
        else
            EMRUtility.MakeEditableGrid('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsHistory_Result', '#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsHistory', CCMCallDetailsHistory, 0, false, true, false, true, false, null);

        //  $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsHistory_Result #dgvCCMCallDetailsHistory').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchCCMCallDetailsHistory: function (PageNumber, RowsPerPage, currMonth, currYear) {

        var objData = new Object();
        if (CCMCallDetailsHistory.params.ParentCtrl == "patTabDemographic") {
            objData["EnrollmentInfoId"] = CCMCallDetailsHistory.params.EnrollmentInfoId;
            objData["PatientId"] = CCMCallDetailsHistory.params.PatientId;
        }
        else {
            objData["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;
            objData["PatientId"] = CCMProgramUpdate.params.PatientId;
        }
        objData["Action"] = "false";

        var d = new Date();

        if (currMonth == null || currMonth == "" || currMonth == undefined || currMonth == 'undefined')
            currMonth = d.getMonth() + 1;

        if (currYear == null || currYear == "" || currYear == undefined || currYear == 'undefined')
            currYear = d.getFullYear();

        objData["Month"] = currMonth;
        objData["Year"] = currYear;

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "LoadCCMTaskTimerDetails");
    },

    rowSave: function ($row, obj) {
        //if (obj.rowValidate($row)) 
        //{
        var _self = obj,
        $actions,
        values = [];

        if ($row.hasClass('adding'))
            $row.removeClass('adding');

        values = $row.find('td').map(function () {

            var $this = $(this);

            //if ($this.hasClass('expand')) {
            //    return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
            //}
            //else
            if ($this.hasClass('actions')) {

                return _self.datatable.cell(this).data();
            }
            else if ($this.hasClass("ddl")) {
                //if ($this.find("select").attr("id") == "ddlRelation" + $($row).attr("id"))
                return $.trim($this.find('select option:selected').text());

            } else {
                return $.trim($this.find('input').val());
            }
        });


        var taskReasonId = $row.find('td:eq(2) select option:selected').val();

        var id = $row.attr("id");
        var myJSON = $row.getMyJSONByName();

        if (id && id > 0) {
            //Edit Record
            var strMessage = "";
            //AppPrivileges.GetFormPrivileges("Type Of Service", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {

            CCMCallDetailsHistory.CCMCallDetailsHistoryUpdate_DbCall(id, myJSON).done(function (response) {
                response = JSON.parse(response);
                var responseModel = JSON.parse(response.ResponseModel);
                if (responseModel.status != false) {
                    CCMCallDetailsHistory.CCMCallDetailsHistorySearch();
                    CCM_Patient_Hub.ShowPanel('ProgramUpdates');
                    utility.DisplayMessages(responseModel.Message, 1);
                    CCMCallDetailsHistory.rowDraw($row, _self, values);
                    //Patient_AccountManager.PatientRepresentativeSearch();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            //    }
            //});
        }
        //}
    },

    rowRemove: function ($row, event) {
        var CallDetailsId = $row.attr("id");

        if (CallDetailsId > 0) {
            utility.myConfirm('1', function () {
                CCMCallDetailsHistory.CCMCallDetailsHistoryDelete_DbCall(CallDetailsId).done(function (response) {
                    response = JSON.parse(response);
                    var responseModel = JSON.parse(response.ResponseModel);
                    if (responseModel.status != false)
                    {
                        CCMCallDetailsHistory.CCMCallDetailsHistorySearch();
                        utility.DisplayMessages(responseModel.Message, 1);
                        CCM_Patient_Hub.ShowPanel('ProgramUpdates');
                    }
                    else {
                        utility.DisplayMessages(responseModel.Message, 3);
                    }
                });

            });
        }
    },

    rowCancel: function ($row, obj) {
        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },

    CCMCallDetailsHistoryUpdate_DbCall: function (CallDetailsId, data) {
        var objData = {};
        var obj = JSON.parse(data);
        objData["CallId"] = CallDetailsId;
        objData["TaskTimerAmalgamatedId"] = CallDetailsId;
        objData["ReceiverName"] = obj.ReceiverName;
        objData["CallDate"] = obj.CallDate;
        objData["CallTime"] = obj.CallTime;
        objData["TaskDate"] = obj.CallDate;
        objData["TaskTime"] = obj.CallTime;
        objData["Caller"] = obj.Caller;
        objData["Caller_text"] = obj.Caller_text;
        objData["Comments"] = obj.Comments;

        objData["CallerType"] = obj.Caller_RefValue;
        objData["ReasonId"] = obj.CallReason;
        objData["CallReason"] = obj.CallReason_text;
        objData["TaskReason"] = obj.CallReason_text;
        objData["CallReason_RefValue"] = obj.CallReason_RefValue;

        objData["TaskDuration"] = obj.Duration;
        objData["DurationUnit"] = obj.DurationUnit;
        objData["DurationUnit_text"] = obj.DurationUnit_text;
        objData["EnrollmentInfoId"] = CCMCallDetailsHistory.params.EnrollmentInfoId;
        objData["PatientId"] = CCMCallDetailsHistory.params.PatientId;
        var jsondata = JSON.stringify(objData);
        return MDVisionService.APIService(jsondata, "CCMProgramUpdate", "UpdateCCMCallDetails");
    },

    CCMCallDetailsHistoryDelete_DbCall: function (CallDetailsId) {
        var objData = {};
        objData["CallId"] = CallDetailsId;
        objData["commandType"] = "DELETE_CCM_CALL_DETAILS";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "CCMProgramUpdate", "DeleteCCMCallDetails");
    },

    UnLoad: function () {

        if (CCMCallDetailsHistory.params != null && CCMCallDetailsHistory.params.ParentCtrl && CCMCallDetailsHistory.params.ParentCtrlPanelID) {
            UnloadActionPan(CCMCallDetailsHistory.params.ParentCtrl, "CCMCallDetailsHistory", null, CCMCallDetailsHistory.params.ParentCtrlPanelID);
        }
        else if (CCMCallDetailsHistory.params != null && CCMCallDetailsHistory.params.ParentCtrl) {
            UnloadActionPan(CCMCallDetailsHistory.params.ParentCtrl, "CCMCallDetailsHistory");
        }
        else {
            UnloadActionPan(null, "CCMCallDetailsHistory");
        }

    },

    CCMCallDetailsPreviousHistorySearch: function (PageNo, rpp) {
        var strMessage = "";
        //   AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsPreviousHistory_Result').css("display") == "none") {
                $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsPreviousHistory_Result').show();
            }
            CCMCallDetailsHistory.SearchCCMCallDetailsPreviousHistory(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    CCMCallDetailsHistory.CCMCallDetailsPreviousHistoryGridLoad(response);
                    var TableControl = CCMCallDetailsHistory.params["PanelID"] + " #dgvCCMCallDetailsPreviousHistory";
                    var PagingPanelControlID = CCMCallDetailsHistory.params["PanelID"] + " #divPracticePaging";
                    var ClassControlName = "CCMCallDetailsHistory";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.PracticeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        //CCMCallDetailsHistory.PracticeSearch(PrimaryID, PageNumber, ResultPerPage);
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

    CCMCallDetailsPreviousHistoryGridLoad: function (response) {

        $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsPreviousHistory').dataTable().fnDestroy();
        $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsPreviousHistory_Result #dgvCCMCallDetailsPreviousHistory tbody').find("tr").remove();
        if (response.CCMCallDetailsCount > 0) {
            var CCMCallDetailsHistoryJSONData = response.CCMTaskTimer_JSON;
            $.each(CCMCallDetailsHistoryJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", item.TaskTimerAmalgamatedId);
                $row.attr("EnrollmentInfoId", CCMCallDetailsHistory.params.EnrollmentInfoId);

                var Task_Date = utility.RemoveTimeFromDate(null, item.TaskDate);

                if (Task_Date == "01/01/1900")
                    Task_Date = "";


                $row.append('<td style="display:none;">' + item.TaskTimerAmalgamatedId + '</td><td>' + item.CreatedByName + '</td><td>' + item.ModifiedByName + '</td><td>' + item.TaskReason + '</td><td>' + item.Comments + '</td><td>' + item.TaskDuration + '</td><td>' + item.Caller + '</td><td>' + item.ReceiverName + '</td><td>' + Task_Date + '</td><td>' + item.TaskTime + '</td>');

                $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsPreviousHistory_Result #dgvCCMCallDetailsPreviousHistory tbody').last().append($row);
            });
        }
        else {
            $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsPreviousHistory').DataTable({
                "language": {
                    "emptyTable": "No Calls History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsPreviousHistory'))
            ;
        else
            //  EMRUtility.MakeEditableGrid('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsHistory_Result', '#' + CCMCallDetailsHistory.params["PanelID"] + ' #dgvCCMCallDetailsHistory', CCMCallDetailsHistory, 0, false, true, false, true, false, null);

            $('#' + CCMCallDetailsHistory.params["PanelID"] + ' #pnlCCMCallDetailsPreviousHistory_Result #dgvCCMCallDetailsPreviousHistory').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchCCMCallDetailsPreviousHistory: function (PageNumber, RowsPerPage) {

        var objData = new Object();

        if (CCMCallDetailsHistory.params.ParentCtrl == "patTabDemographic") {
            objData["EnrollmentInfoId"] = CCMCallDetailsHistory.params.EnrollmentInfoId;
            objData["PatientId"] = CCMCallDetailsHistory.params.PatientId;
        }
        else {
            objData["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;
            objData["PatientId"] = CCMProgramUpdate.params.PatientId;
        }
        objData["Action"] = "true";

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "LoadCCMTaskTimerDetails");
    },
}

