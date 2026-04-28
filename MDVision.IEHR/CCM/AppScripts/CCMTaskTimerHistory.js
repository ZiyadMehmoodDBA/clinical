CCMTaskTimerHistory = {
    bIsFirstLoad: true,
    params: [],
    SelectedMonth:0,
    Load: function (params) {
        CCMTaskTimerHistory.params = params;

        if (CCMTaskTimerHistory.bIsFirstLoad) {
            CCMTaskTimerHistory.bIsFirstLoad = false;
            if (CCMTaskTimerHistory.params != null && CCMTaskTimerHistory.params.PanelID != "pnlCCMTaskTimerHistory") {
                CCMTaskTimerHistory.params["PanelID"] = CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory';
            }
            else {
                CCMTaskTimerHistory.params["PanelID"] = "pnlCCMTaskTimerHistory";
            }
            var self = $('#' + CCMTaskTimerHistory.params["PanelID"]);

            // self.loadDropDowns(true).done(function () {
            CCMTaskTimerHistory.SelectedMonth = new Date().getMonth() + 1;
            CCMTaskTimerHistory.CCMTaskTimerHistorySearch();
            //});
            
            $("#TaskTimerMonth").val(CCMTaskTimerHistory.SelectedMonth)
       
            $("#TaskTimerMonth").change(function () {
              
                CCMTaskTimerHistory.SelectedMonth = $("#TaskTimerMonth").val();
               
                CCMTaskTimerHistory.CCMTaskTimerHistorySearch();
            });

        }
    },

    CCMTaskTimerHistorySearch: function (primaryId, PageNo, rpp) {
        var strMessage = "";
        //   AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory_Result').css("display") == "none") {
                $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory_Result').show();
            }

            var TableControl = CCMTaskTimerHistory.params["PanelID"] + " #dgvCCMTaskTimerHistory";
            var PagingPanelControlID = CCMTaskTimerHistory.params["PanelID"] + " #divCCMTaskTimerHistoryPaging";
            var ClassControlName = "CCMTaskTimerHistory";
            var PagesToDisplay = 5;

            CCMTaskTimerHistory.SearchCCMTaskTimerHistory(PageNo, rpp, CCMTaskTimerHistory.SelectedMonth).done(function (response) {
                if (response.status != false) {
                    CCMTaskTimerHistory.CCMTaskTimerHistoryGridLoad(response);
                    
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.CCMTaskTimerCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        CCMTaskTimerHistory.CCMTaskTimerHistorySearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);
                }
                else {
                     $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerHistory').dataTable().fnDestroy();
                        $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory_Result #dgvCCMTaskTimerHistory tbody').find("tr").remove();
                        $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerHistory').DataTable({
                            "language": {
                                "emptyTable": "No History Found"
                            }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                        });
                        $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory_Result #divCCMTaskTimerHistoryPaging').css("visibility", "hidden");
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    CCMTaskTimerHistoryGridLoad: function (response) {

        var actions = "";
        $("#" + CCMTaskTimerHistory.params.PanelID + " #dgvCCMTaskTimerHistory tr th").each(function () {
            if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
                var arrActionType = [];
                if ($(this).attr("ActionType") != null) {
                    arrActionType = $(this).attr("ActionType").split(',');
                    actions = EMREditableGrid.GetActions(arrActionType, "#" + CCMTaskTimerHistory.params.PanelID + " #pnlCCMTaskTimerHistory_Result");
                }
            }
        });

        $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory_Result #divCCMTaskTimerHistoryPaging').css("visibility", "visible");
        $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerHistory').dataTable().fnDestroy();
        $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory_Result #dgvCCMTaskTimerHistory tbody').find("tr").remove();
        if (response.CCMTaskTimerCount > 0) {
            var CCMTaskTimerHistoryJSONData = response.CCMTaskTimer_JSON;
            $.each(CCMTaskTimerHistoryJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", item.TaskTimerId);

                var TaskDate = utility.RemoveTimeFromDate(null, item.TaskDate)

                if (TaskDate == "01/01/1900")
                    TaskDate = "";

                $row.append('<td style="display:none;">' + item.TaskTimerId + '</td><td>' + actions + '</td><td>' + item.CreatedBy + '</td><td>' + item.TaskReason + '</td><td>' + item.Comments + '</td><td>' + Number(item.TaskDuration).toFixed(globalAppdata.DecimalPlaces) + ' Minutes' + '</td><td>' + TaskDate + '</td><td>' + item.TaskTime.split(".")[0] + '</td>');

                $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory_Result #dgvCCMTaskTimerHistory tbody').last().append($row);
            });
        }
        else {
            $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerHistory').DataTable({
                "language": {
                    "emptyTable": "No History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerHistory'))
            ;
        else
            EMRUtility.MakeEditableGrid('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory_Result', '#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerHistory', CCMTaskTimerHistory, 0, false, true, false, true, false, null);
        //$('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerHistory_Result #dgvCCMTaskTimerHistory').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

     //   $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerHistory').dataTable().fnSort([[1, "desc"]]);
    },

    SearchCCMTaskTimerHistory: function (PageNumber, RowsPerPage, selectedMonth) {

        var objData = new Object();

        objData["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;
        objData["PatientId"] = CCMProgramUpdate.params.PatientId;
        objData["Action"] = "false";
        objData["PageNumber"] = typeof PageNumber === "undefined" ? 1 : PageNumber;
        objData["RowsPerPage"] = typeof RowsPerPage === "undefined" ? 15 : RowsPerPage;
        objData["SelectedMonth"] = selectedMonth;
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "LoadCCMTaskTimer");
    },

    //GridActionButtons: function (TaskTimerId) {
    //    return '<a class="btn btn-xs" href="javascript:void(0);" onclick="CCMTaskTimerHistory.TaskTimerHistoryDelete(' + TaskTimerId + ', event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;' +
    //                          '<a class="btn btn-xs" href="javascript:void(0);" onclick="CCMTaskTimerHistory.TaskTimerHistoryEdit(' + TaskTimerId + ', event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;';
    //    //  '<a class="btn  btn-xs" href="javascript:void(0);" onclick="CCM_CarePlan.carePlanActiveInactive(' + TaskTimerId + "," + isEventactive + ', event);" title="' + activeTitle +
    //    //  '"><i class="' + tglclass + '"></i></a>';
    //},
    TaskTimerHistoryEdit: function (TaskTimerId, event) {
        alert(TaskTimerId);
    },
    rowSave: function ($row, obj) {

        if (obj.rowValidate($row)) {

            var _self = obj,
            $actions,
            values = [];

            if ($row.hasClass('adding')) {
                $row.removeClass('adding');
            }

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

                CCMTaskTimerHistory.TaskTimerHistoryUpdate_DbCall(id, myJSON).done(function (response) {
                    response = JSON.parse(response);
                    var responseModel = JSON.parse(response.ResponseModel);
                    if (responseModel.status != false) {
                        CCMTaskTimerHistory.CCMTaskTimerHistorySearch();
                        utility.DisplayMessages(responseModel.Message, 1);
                        CCMProgramUpdate.LoadCCMTaskTime();
                        CCMTaskTimerHistory.rowDraw($row, _self, values);
                        //Patient_AccountManager.PatientRepresentativeSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
                //    }
                //});
            }
        }
    },

    rowRemove: function ($row, event) {
        var TaskTimerId = $row.attr("id");
        //if (event != null) {
        //    event.stopPropagation();
        //}
        if (TaskTimerId > 0) {
            utility.myConfirm('1', function () {
                CCMTaskTimerHistory.TaskTimerHistoryDelete_DbCall(TaskTimerId).done(function (response) {
                    response = JSON.parse(response);
                    var responseModel = JSON.parse(response.ResponseModel);
                    if (responseModel.status != false) {
                        CCMTaskTimerHistory.CCMTaskTimerHistorySearch();
                        utility.DisplayMessages(responseModel.Message, 1);
                        CCMProgramUpdate.LoadCCMTaskTime();
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
    TaskTimerHistoryUpdate_DbCall: function (TaskTimerId, data) {
        var objData = {};
        var obj = JSON.parse(data);
        objData["TaskTimerId"] = TaskTimerId;
        objData["TaskHours"] = obj.TaskHours;
        objData["TaskMinutes"] = obj.TaskMinutes;
        objData["TaskSeconds"] = obj.TaskSeconds;
        objData["Comments"] = obj.Comments;
        objData["TaskReason"] = obj.TaskReason_text;
        objData["commandType"] = "Update_CCM_Task_Timer_Id";
        var jsondata = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(jsondata, "CCMProgramUpdate", "UpdateCCMTaskTimerHistory");
    },
    TaskTimerHistoryDelete_DbCall: function (TaskTimerId) {
        var objData = {};
        objData["TaskTimerId"] = TaskTimerId;
        objData["commandType"] = "DELETE_CCM_Task_Timer_Id";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "CCMProgramUpdate", "DeleteCCMTaskTimerHistory");
    },
    UnLoad: function () {

        if (CCMTaskTimerHistory.params != null && CCMTaskTimerHistory.params.ParentCtrl && CCMTaskTimerHistory.params.ParentCtrlPanelID) {
            UnloadActionPan(CCMTaskTimerHistory.params.ParentCtrl, "CCMTaskTimerHistory", null, CCMTaskTimerHistory.params.ParentCtrlPanelID);
        }
        else if (CCMTaskTimerHistory.params != null && CCMTaskTimerHistory.params.ParentCtrl) {
            UnloadActionPan(CCMTaskTimerHistory.params.ParentCtrl, "CCMTaskTimerHistory");
        }
        else {
            UnloadActionPan(null, "CCMTaskTimerHistory");
        }

    },




    CCMTaskTimerPreviousHistorySearch: function (primaryId, PageNo, rpp) {
        var strMessage = "";
        //   AppPrivileges.GetFormPrivileges("Practice", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            if ($('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerPreviousHistory_Result').css("display") == "none") {
                $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerPreviousHistory_Result').show();
            }
            CCMTaskTimerHistory.SearchCCMTaskTimerPreviousHistory(PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    CCMTaskTimerHistory.CCMTaskTimerPreviousHistoryGridLoad(response);
                    var TableControl = CCMTaskTimerHistory.params["PanelID"] + " #dgvCCMTaskTimerPreviousHistory";
                    var PagingPanelControlID = CCMTaskTimerHistory.params["PanelID"] + " #divCCMTaskTimerHistoryPreviousPaging";
                    var ClassControlName = "CCMTaskTimerPreviousHistory";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.CCMTaskTimerCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        CCMTaskTimerHistory.CCMTaskTimerPreviousHistorySearch(PrimaryID, PageNumber, ResultPerPage);
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

    CCMTaskTimerPreviousHistoryGridLoad: function (response) {

        $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerPreviousHistory').dataTable().fnDestroy();
        $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerPreviousHistory_Result #dgvCCMTaskTimerPreviousHistory tbody').find("tr").remove();
        if (response.CCMTaskTimerCount > 0) {
            var CCMTaskTimerHistoryJSONData = response.CCMTaskTimer_JSON;
            $.each(CCMTaskTimerHistoryJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", item.TaskTimerId);

                var TaskDate = utility.RemoveTimeFromDate(null, item.TaskDate)

                if (TaskDate == "01/01/1900")
                    TaskDate = "";

                $row.append('<td style="display:none;">' + item.TaskTimerId + '</td><td>' + item.Action + '</td><td>' + item.CreatedBy + '</td><td>' + item.TaskReason + '</td><td>' + item.Comments + '</td><td>' + Number(item.TaskDuration).toFixed(globalAppdata.DecimalPlaces) + ' Minutes' + '</td><td>' + TaskDate + '</td><td>' + item.TaskTime.split(".")[0] + '</td>');

                $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerPreviousHistory_Result #dgvCCMTaskTimerPreviousHistory tbody').last().append($row);
            });
        }
        else {
            $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerPreviousHistory').DataTable({
                "language": {
                    "emptyTable": "No History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + CCMTaskTimerHistory.params["PanelID"] + ' #dgvCCMTaskTimerPreviousHistory'))
            ;
        else
            $('#' + CCMTaskTimerHistory.params["PanelID"] + ' #pnlCCMTaskTimerPreviousHistory_Result #dgvCCMTaskTimerPreviousHistory').DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },

    SearchCCMTaskTimerPreviousHistory: function (PageNumber, RowsPerPage) {

        var objData = new Object();

        objData["EnrollmentInfoId"] = CCMProgramUpdate.params.EnrollmentInfoId;
        objData["PatientId"] = CCMProgramUpdate.params.PatientId;
        objData["Action"] = "true";
        objData["PageNumber"] = typeof PageNumber === "undefined" ? 1 : PageNumber;
        objData["RowsPerPage"] = typeof RowsPerPage === "undefined" ? 15 : RowsPerPage;
        objData["SelectedMonth"] = -1;

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMProgramUpdate", "LoadCCMTaskTimer");
    },
}

