User_Task = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        User_Task.params = params;
        if (User_Task.bIsFirstLoad) {
            User_Task.bIsFirstLoad = false;
            User_Task.SearchUserTask(null, User_Task.params.AssignedToId, "Task", 2);
        }
    },

    SearchUserTask: function (MessageId, AssignedToUserId, MsgType, MsgStatusId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Tasks", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + User_Task.params["PanelID"] + " #pnlUserTask_Result").css("display") == "none") {
                    $("#" + User_Task.params["PanelID"] + " #pnlUserTask_Result").show();
                }

                User_Task.UserTaskSearch(MessageId, AssignedToUserId, MsgType, MsgStatusId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        if (response.UserTaskCount > 0) {

                        }
                        else {

                            $("#pnlUserTask #divUserTaskPaging").css("display", "none");
                        }

                        User_Task.UserTaskGridLoad(response, PageNo, rpp);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    UserTaskGridLoad: function (response, PageNo, rpp) {
        if ($("#pnlUserTask #pnlUserTask_Result").css("display") == "none") {
            $("#pnlUserTask #pnlUserTask_Result").show();
        }
        var gridId = "dgvUserTask";
        var PanelgridId = "#pnlUserTask" + " #pnlUserTask_Result #" + gridId;
        $(PanelgridId).dataTable().fnDestroy();
        $(PanelgridId + " tbody").find("tr").remove();
        if (response.UserTaskCount > 0) {
            // Set latest Task Count on Main Page
            $("#spnUserTasksCount").text(response.UserTaskCount);
            var UserTaskLoadJSONData = JSON.parse(response.UserTaskLoad_JSON);
            $.each(UserTaskLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", gridId + "_row" + item.PatMsgId);
                $row.attr("MessageId", item.PatMsgId);

                if (item.IsActive == "True") {
                    isactive = 0;
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }

                var EditMethod = "User_Task.UserTaskAddEdit('" + item.PatMsgId.trim() + "','Edit',null,null,event);";
                $row.attr("onclick", EditMethod);
                var ActiveInacvtiveMethod = "User_Task.ActiveInactiveUserTask('" + item.PatMsgId.trim() + "','" + isactive + "',event);";
                var AddMessageReplyMethod = "User_Task.UserTaskAddEdit('" + item.PatMsgId.trim() + "','Reply','" + item.AssignedToId + "','" + item.MsgStatusId + "',event);";
                if (item.MessageStatus == "Unresolved")
                    $row.append('<td style="display:none;">' + item.PatMsgId + '</td><td><a class="btn btn-xs" href="#" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="Inactive Record"><i class="' + tglclass + '"></i></a><a class="btn  btn-xs" href="#" onclick="' + AddMessageReplyMethod + '" title="Reply"><i class="fa fa-reply"></i></a>&nbsp;</td><td class="size-max250 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.MsgDetail + '">' + item.MsgDetail + '</td><td>' + item.MessageStatus + '</td><td>' + item.MessageType + '</td><td>' + item.AssigneeName + '</td><td>' + item.CreatedBy + '</td><td>' + item.EntryDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td>');

                $(PanelgridId + " tbody").last().append($row);
            });
            //Set ToolTip for Comments.
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });


            //----------------- User Task Paging----
            $("#pnlUserTask #divUserTaskPaging").css("display", "inline");
            //Showing 1 to 15 of 15 Record(s)
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;

            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divUserTaskPaging", response.iTotalDisplayRecords, 5, "User_Task", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
            $("#pnlUserTask #divUserTaskPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlUserTask li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
            //---------------End paging
        }
        else {
            $("#spnUserTasksCount").text("");
            $("#pnlUserTask #divUserTaskPaging").css("display", "none");
            $(PanelgridId).DataTable({
                "language": {
                    "emptyTable": "No Task Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false,}]
            });
        }
        if ($.fn.dataTable.isDataTable(PanelgridId))
            ;
        else
            $(PanelgridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting":[], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    UserTaskAddEdit: function (MessageId, mode, AssignedToId, StatusId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        if (mode == "Edit" || mode == "Reply") {
            utility.SelectGridRow($('#dgvUserTask_row' + MessageId));
        }
        AppPrivileges.GetFormPrivileges("Tasks", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["MessageId"] = MessageId;
                params["mode"] = mode;
                params["ParentCtrl"] = "User_Task";
                params["AssignedToId"] = User_Task.params.AssignedToId;
                if (mode == "Add") {
                    LoadActionPan('Patient_MessageAdd', params);
                    //LoadActionPan('Patient_MessageReply', params);
                }
                else if (mode == "Edit") {
                    LoadActionPan('Patient_MessageEdit', params);
                }
                else if (mode == "Reply") {
                    params["StatusId"] = StatusId;
                    LoadActionPan('Patient_MessageReply', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    UserTaskSearch: function (MessageId, AssignedToId, MsgType, MsgStatusId, PageNumber, RowsPerPage) {

        if (MessageId == null) {
            MessageId = 0;
        }
        if (AssignedToId == null) {
            AssignedToId = 0;
        }
        if (MsgType == null) {
            MsgType = "Task";
        }
        if (MsgStatusId == null) {
            MsgStatusId = 2;// 2 stands for Task
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "MessageId=" + MessageId + "&AssignedToId=" + AssignedToId + "&MsgType=" + MsgType + "&MsgStatusId=" + MsgStatusId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "USER_TASK", "SEARCH_USER_TASK");
    },
    //PMS-4856
    ActiveInactiveUserTask: function (MessageId, IsActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('3', function () {
            var selectedValue = MessageId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Patient_Message.MessageUpdateActiveInactive(selectedValue, IsActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        User_Task.SearchUserTask(null, User_Task.params.AssignedToId, "Task", 2);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { }, '3', null, null, null, IsActive);
    },
    UnLoad: function () {
        if (User_Task.params != null && User_Task.params.ParentCtrl != null) {
            UnloadActionPan(User_Task.params.ParentCtrl, 'User_Task');
        }
        else
            UnloadActionPan(null, 'User_Task');

        if (User_Task.params != null && User_Task.params.TabID == 'mstrTabDashBoard') {
            DashBoard.DashBoardTasksSearch(null, null, null);
        }
    },

    //-----------Pagination Functions--------------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlUserTask_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        User_Task.SearchUserTask(null, User_Task.params.AssignedToId, "Task", 2, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlUserTask_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            User_Task.SearchUserTask(null, User_Task.params.AssignedToId, "Task", 2, currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlUserTask_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            User_Task.SearchUserTask(null, User_Task.params.AssignedToId, "Task", 2, currentPageNo, 15);
        }
    },
}