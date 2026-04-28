Admin_StatementMessage = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Admin_StatementMessage.params = params;
        if (Admin_StatementMessage.bIsFirstLoad) {
            Admin_StatementMessage.bIsFirstLoad = false;

            AppPrivileges.GetFormPrivileges("Statement Message", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_StatementMessage.SearchStatementMessage();
                  //  utility.bindEnterKey("#pnlAdminStatementMessage #btnUserSearch");
                }
            });


        }
    },

    StatementMessageAdd: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Statement Message", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["MessageId"] = "-1";
                params["mode"] = "Add";
                LoadActionPan('StatementMessageDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    SearchStatementMessage: function (StatementMessageId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Statement Message", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if ($("#pnlAdminStatementMessage #pnlStatementMessage_Result").css("display") == "none") {
                    $("#pnlAdminStatementMessage #pnlStatementMessage_Result").show();
                }

              //  var StatementMessageId = null;

                var self = $("#pnlAdminStatementMessage");
                var myJSON = self.getMyJSON();


                Admin_StatementMessage.StatementMessageSearch(myJSON, StatementMessageId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {


                        //-----------------Pagination------------

                        if (response.MessageCount > 0) {
                            $('#' + Admin_StatementMessage.params.PanelID + " #divStatementMessagePaging").css("display", "inline");
                            var TableControl = Admin_StatementMessage.params.PanelID + " #dgvStatementMessage";
                            var PagingPanelControlID = Admin_StatementMessage.params.PanelID + " #divStatementMessagePaging";
                            var ClassControlName = "Admin_StatementMessage";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.MessageCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_StatementMessage.SearchStatementMessage(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
                            //Showing 1 to 15 of 15 entries
                            //var RecordsPerPage = rpp != null ? rpp : 15;
                            //var CurrentPage = PageNo != null ? PageNo : 1;

                            //var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            //var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            //if (PageNo == null) {
                            //    utility.GetCustomPaging("divStatementMessagePaging", response.iTotalDisplayRecords, 5, "Admin_StatementMessage", CurrentPage, RecordsPerPage);
                            //}
                            //var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            //var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            //$('#' + Admin_StatementMessage.params.PanelID + " #divStatementMessagePaging #divShowingEntries").text(showingText);
                            //// Change Background Color to Black for selected page
                            //$('#' + Admin_StatementMessage.params.PanelID + " li").each(function () {
                            //    if ($(this).text() == CurrentPage) {
                            //        $(this).attr("class", "active");
                            //    }
                            //    else
                            //        $(this).removeAttr("class");
                            //});
                        }
                        else {
                            $('#' + Admin_StatementMessage.params.PanelID + " #divStatementMessagePaging").css("display", "none");
                        }

                        //--------------------End Pagination-------------------








                        Admin_StatementMessage.StatementMessageGridLoad(response);
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

    StatementMessageSearch: function (JSONstr, StatementMessageID, pageNo, recordPerPage) {


        if (JSONstr == null) {
            JSONstr = "";
        }

        if (StatementMessageID == null) {
            StatementMessageID = 0;
        }

        if (pageNo == null) {
            pageNo = 1;
        }
        if (recordPerPage == null) {
            recordPerPage = 15;
        }
        //FIXME
        var data = "StatementMessageData=" + JSONstr + "&StatementMessageID=" + StatementMessageID + "&pageNo=" + pageNo + "&recordPerPage=" + recordPerPage;

        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_MESSAGE", "SEARCH_STATEMENT_MESSAGE");
    },

    StatementMessageGridLoad: function (response) {
        $("#pnlAdminStatementMessage #pnlStatementMessage_Result #dgvStatementMessage").dataTable().fnDestroy();
        $("#pnlAdminStatementMessage #pnlStatementMessage_Result #dgvStatementMessage tbody").find("tr").remove();
        if (response.MessageCount > 0) {
            var StatementMessageLoadJSONData = JSON.parse(response.StatementMessageLoad_JSON);


            $.each(StatementMessageLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_StatementMessage.StatementMessageEdit('" + item.StmtMsgId + "',event);");
                $row.attr("id", "dgvStatementMessage_row" + item.StmtMsgId);
                $row.attr("StmtMsgId", item.StmtMsgId);

                var selectMethod = "";

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


                $row.append('<td style="display:none;">' + item.StmtMsgId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_StatementMessage.StatementMessageDelete(' + item.StmtMsgId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_StatementMessage.StatementMessageEdit(' + item.StmtMsgId + ',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_StatementMessage.StatementMessageActiveInactive(' + item.StmtMsgId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.ShortName + '">' + item.ShortName + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Message + '">' + item.Message + '</td>');

                $("#pnlAdminStatementMessage #pnlStatementMessage_Result #dgvStatementMessage tbody").last().append($row);




            });



        }
        else {

            //FIXME
            if ($("#pnlAdminStatementMessage #pnlStatementMessage_Result").css("display") == "none") {
                $("#pnlAdminStatementMessage #pnlStatementMessage_Result").show();
            }

            $("#pnlAdminStatementMessage #pnlStatementMessage_Result #dgvStatementMessage").DataTable({
                "language": {
                    "emptyTable": "No Statement Messages Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });


        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#pnlAdminStatementMessage #pnlStatementMessage_Result #dgvStatementMessage"));
        else
            $("#pnlAdminStatementMessage #pnlStatementMessage_Result #dgvStatementMessage").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    StatementMessageActiveInactive: function (StatementMessageID, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Statement Message", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = StatementMessageID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {

                        Admin_StatementMessage.UpdateStatementMessageActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_StatementMessage.SearchStatementMessage();

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                   '3', null, null, null, IsActive
                );

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    UpdateStatementMessageActiveInactive: function (StatementMessageID, IsActive) {
        var data = "StatementMessageID=" + StatementMessageID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_MESSAGE", "UPDATE_STATEMENT_MESSAGE_ACTIVE_INACTIVE");
    },

    StatementMessageDelete: function (StatementMessageId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvStatementMessage_row' + StatementMessageId));
        AppPrivileges.GetFormPrivileges("Statement Message", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                utility.myConfirm('1', function () {
                    var selectedValue = StatementMessageId;
                    //var oTable = $('#dgvSupperBill').DataTable();
                    //var ind = $(this).index();
                    //var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_StatementMessage.DeleteStatementMessage(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvSupperBill').DataTable();
                                //table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                Admin_StatementMessage.SearchStatementMessage();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () {

                },
                    '1'
                );

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DeleteStatementMessage: function (StatementMessageId) {
        var data = "StatementMessageId=" + StatementMessageId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_MESSAGE", "DELETE_STATEMENT_MESSAGE");
    },

    StatementMessageEdit: function (StatementMessageId,event) {

        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#dgvStatementMessage_row' + StatementMessageId));
        AppPrivileges.GetFormPrivileges("Statement Message", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = StatementMessageId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["StatementMessageId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('StatementMessageDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },



    
    /************PAGINATION FUNCTIONS**************/

    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {
        Admin_StatementMessage.SearchStatementMessage(0, PageNo, 15);
    },
    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $('#' + Admin_StatementMessage.params.PanelID + " #pnlStatementMessage_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_StatementMessage.SearchStatementMessage(0, currentPageNo, 15);
        }
    },
    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $('#' + Admin_StatementMessage.params["PanelID"] + " #pnlStatementMessage_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_StatementMessage.SearchStatementMessage(0, currentPageNo, 15);
        }
    },
    /************************************/




    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },

};

