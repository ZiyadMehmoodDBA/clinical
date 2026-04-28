User_Message = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        User_Message.params = params;
        if (User_Message.bIsFirstLoad) {
            User_Message.bIsFirstLoad = false;
            var self = $('#pnlUserMessage #msgTypeContainer')
            self.loadDropDowns(true).done(function () {
                $("#pnlUserMessage #msgType option:contains('Unresolved')").attr('selected', 'selected');
                Patient_Message.SearchPatientMessage(null, 1, User_Message.params.AssignedToId,null,null,$("#pnlUserMessage #msgType").val());
            });
            
            
        }
    },

    UserMessageGridLoad: function (response, PageNo, rpp) {
        if ($("#pnlUserMessage #pnlUserMessage_Result").css("display") == "none") {
            $("#pnlUserMessage #pnlUserMessage_Result").show();
        }
        var gridId = "dgvUserMessage";
        pnlUserMessage
        var PanelgridId = "#pnlUserMessage" + " #pnlUserMessage_Result #" + gridId;
        //var PanelgridId = "#" + User_Message.params["PanelID"] + " #pnlUserMessage_Result #" + gridId;
        $(PanelgridId).dataTable().fnDestroy();
        $(PanelgridId + " tbody").find("tr").remove();
        if (response.MessageCount > 0) {
            var totalUserMesagesCount = 0;
            var MessageLoadJSONData = JSON.parse(response.MessageLoad_JSON);
            $.each(MessageLoadJSONData, function (i, item) {
                if (item.MessageType.toLowerCase() != "task") {
                    totalUserMesagesCount++;
                    var $row = $('<tr/>');
                    
                    $row.attr("id", gridId + "_row" + item.PatMsgId);
                    $row.attr("MessageId", item.PatMsgId);

                    if (item.IsActive == "True") {
                        isactive = 0;
                        tglclass = "fa fa-toggle-on green";
                    } else {
                        isactive = 1;
                        tglclass = "fa fa-toggle-on red";
                    }

                    var EditMethod = "User_Message.UserMessageAddEdit('" + item.PatMsgId.trim() + "','Edit',null,null,event);";
                    $row.attr("onclick", EditMethod);
                    var ActiveInacvtiveMethod = "User_Message.ActiveInactivePatientMessage('" + item.PatMsgId.trim() + "'," + isactive + ",event);";
                    var AddMessageReplyMethod = "User_Message.UserMessageAddEdit('" + item.PatMsgId.trim() + "','Reply'," + item.AssignedToId + ",'" + item.MsgStatusId + "',event);";
                    //if (item.MessageStatus == "Unresolved")
                    $row.append('<td style="display:none;">' + item.PatMsgId + '</td><td><a class="btn btn-xs" href="#" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs" href="#" onclick="' + ActiveInacvtiveMethod + '" title="Inactive Record"><i class="' + tglclass + '"></i></a><a class="btn  btn-xs" href="#" onclick="' + AddMessageReplyMethod + '" title="Reply"><i class="fa fa-reply"></i></a>&nbsp;</td><td class="size-max250 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.MsgDetail + '">' + item.MsgDetail + '</td><td>' + item.MessageStatus + '</td><td>' + item.MessageType + '</td><td>' + item.AssigneeName + '</td><td>' + item.UserName + '</td><td>' + item.EntryDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td>');

                    $(PanelgridId + " tbody").last().append($row);
                } 
            });
            //Set ToolTip for Comments.
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

            //----------------- Patient Messages Paging----
            $("#pnlUserMessage #divUserMsgsPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 15;
            var CurrentPage = PageNo != null ? PageNo : 1;
           // $("#spnMessageCount").text(totalUserMesagesCount);
            var PagesToShow = Math.ceil(totalUserMesagesCount / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                utility.GetCustomPaging("divUserMsgsPaging", totalUserMesagesCount, 5, "User_Message", CurrentPage, RecordsPerPage);
            }
            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < totalUserMesagesCount ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : totalUserMesagesCount;
            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + totalUserMesagesCount + " Record(s)";
            $("#pnlUserMessage #divUserMsgsPaging #divShowingEntries").text(showingText);
            // Change Background Color to Black for selected page
            $("#pnlUserMessage li").each(function () {
                if ($(this).text() == CurrentPage) {
                    $(this).attr("class", "active");
                }
                else
                    $(this).removeAttr("class");
            });
            //--------------------------------------------

            if ($(PanelgridId + " tbody tr").length <= 0) {

                $("#pnlUserMessage #divUserMsgsPaging").css("display", "none");
                $(PanelgridId).DataTable({
                    "language": {
                        "emptyTable": "No Message Found"
                    }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });

            }

        }
        else {
            $("#pnlUserMessage #divUserMsgsPaging").css("display", "none");
            $(PanelgridId).DataTable({
                "language": {
                    "emptyTable": "No Message Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable(PanelgridId))
            ;
        else
            $(PanelgridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    UserMessageAddEdit: function (MessageId, mode, AssignedToId, StatusId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Tasks", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["MessageId"] = MessageId;
                params["mode"] = mode;
                params["ParentCtrl"] = "User_Message";
                params["AssignedToId"] = User_Message.params.AssignedToId;
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

    UnLoad: function () {
        if (User_Message.params != null && User_Message.params.ParentCtrl != null) {
            UnloadActionPan(User_Message.params.ParentCtrl, 'User_Message');
        }
        else
            UnloadActionPan(null, 'User_Message');
    },

    //-----------Pagination Functions--------------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlUserMessage_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Patient_Message.SearchPatientMessage(null, 1, User_Message.params.AssignedToId, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlUserMessage_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Patient_Message.SearchPatientMessage(null, 1, User_Message.params.AssignedToId, currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlUserMessage_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Patient_Message.SearchPatientMessage(null, 1, User_Message.params.AssignedToId, currentPageNo, 15);
        }
    },

    filterMessage: function (ev) {
        Patient_Message.SearchPatientMessage(null, 1, User_Message.params.AssignedToId,null,null,$(ev).val());
    }
}