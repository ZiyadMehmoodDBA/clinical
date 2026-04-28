Admin_FollowUpAction = {
    bIsFirstLoad: true,
    params:[],
    Load: function (params) {

        Admin_FollowUpAction.params = params;
        if (Admin_FollowUpAction.params.PanelID != "pnlAdminFollowUpAction") {
            Admin_FollowUpAction.params.PanelID = Admin_FollowUpAction.params.PanelID + ' #pnlAdminFollowUpAction';
        }
        var self = $('#' + Admin_FollowUpAction.params.PanelID);
        if (Admin_FollowUpAction.bIsFirstLoad) {
            Admin_FollowUpAction.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {
                Admin_FollowUpAction.ActionSearch();
            });
        }
    },
    ActionSearch: function (ActionId, PageNo, rpp) {
        if ($("#pnlAdminFollowUpAction_Result").css("display") == "none") {
            $("#pnlAdminFollowUpAction_Result").show();
        }
        if ($("#" + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result").css("display") == "none") {
            $("#" + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result").show();
        }

        var self = "";
        self = $('#pnlAdminFollowUpAction');
        var myJSON = self.getMyJSON();

        Admin_FollowUpAction.SearchAction(myJSON, ActionId, PageNo, rpp).done(function (response) {
            if (response.status != false) {

                //-----------------Pagination------------

                if (response.ActionCount > 0) {
                    $('#' + Admin_FollowUpAction.params.PanelID + " #divFollowUpActionPaging").css("display", "inline");
                    var TableControl = Admin_FollowUpAction.params["PanelID"] + " #dgvFollowUpAction";
                    var PagingPanelControlID = Admin_FollowUpAction.params["PanelID"] + " #divFollowUpActionPaging";
                    var ClassControlName = "Admin_FollowUpAction";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.ActionCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_FollowUpAction.ActionSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);

                    //Showing 1 to 15 of 15 entries
                    //var RecordsPerPage = rpp != null ? rpp : 15;
                    //var CurrentPage = PageNo != null ? PageNo : 1;

                    //var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                    //var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                    //if (PageNo == null) {
                    //    utility.GetCustomPaging("divFollowUpActionPaging", response.iTotalDisplayRecords, 5, "Admin_FollowUpAction", CurrentPage, RecordsPerPage);
                    //}
                    //var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                    //var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                    //$('#' + Admin_FollowUpAction.params.PanelID + " #divFollowUpActionPaging #divShowingEntries").text(showingText);
                    //// Change Background Color to Black for selected page
                    //$('#' + Admin_FollowUpAction.params.PanelID + " li").each(function () {
                    //    if ($(this).text() == CurrentPage) {
                    //        $(this).attr("class", "active");
                    //    }
                    //    else
                    //        $(this).removeAttr("class");
                    //});
                }
                else {
                    $('#' + Admin_FollowUpAction.params.PanelID + " #divFollowUpActionPaging").css("display", "none");
                }

                //--------------------End Pagination-------------------

                Admin_FollowUpAction.FollowUpActionGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        
    },
    FollowUpActionGridLoad: function (response) {
        $("#" + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result #dgvFollowUpAction").dataTable().fnDestroy();
        $("#" + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result #dgvFollowUpAction tbody").find("tr").remove();
        if (response.ActionCount > 0) {
            var LoadActionJSONData = JSON.parse(response.FollowUpActionLoad_JSON);
            $.each(LoadActionJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FollowUpAction.EditFollowUpAction('" + item.FollowupActionId + "',event);");
                $row.attr("id", "gvAction_row" + item.FollowupActionId);
                $row.attr("ActionId", item.FollowupActionId);
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

                var selectAction = "";
                //if (Admin_FollowUpAction.params["FromAdmin"] == "0") {
                //    selectAction = '&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_FollowUpAction.FillBatchCharge(' + item.BatchNumber + ', \'' + item.ActionId + '\');" title="Select Record"><i class="fa fa-check black"></i></a>';
                //}

                
                $row.append('<td><a class="btn  btn-xs" href="#" onclick="Admin_FollowUpAction.FollowActionDelete(' + item.FollowupActionId + ',event);" title="Delete FollowUp Action"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_FollowUpAction.EditFollowUpAction(' + item.FollowupActionId + ',event);" title="Edit"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_FollowUpAction.ActionActiveInactive(' + item.FollowupActionId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectAction + '</td> <td class="max150 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.ShortName + '">' + item.ShortName + '</td> <td class="max250 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.Description + '">' + item.Description + '</td> <td>' + item.SuspendedDays + '</td><td>' + item.ARTypeName + '</td><td class="max200 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.AutoAction + '">' + item.AutoAction + '</td><td class="max150 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.LedgerAccount + '">' + item.LedgerAccount + '</td><td>' + item.LetterName + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedByName + '</td>');

                $("#" + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result #dgvFollowUpAction tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_FollowUpAction.params.PanelID + " #divFollowUpActionPaging").css("display", "none");
            $("#" + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result #dgvFollowUpAction").DataTable({
                "language": {
                    "emptyTable": "No FollowUp Action Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result #dgvFollowUpAction"))
            ;
        else
            $("#" + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result #dgvFollowUpAction").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            //$("#" + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result #dgvFollowUpAction").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        
    },
    SearchAction: function (actionData, actionId, pageNo, recordPerPage) {
        if (pageNo == null) {
            pageNo = 1;
        }
        if (recordPerPage == null) {
            recordPerPage = 15;
        }
        var data = "actionData=" + actionData + "&actionId=" + actionId + "&pageNo=" + pageNo + "&recordPerPage="+recordPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ACTION", "SEARCH_ACTION");
    },
    FollowActionDelete: function (actionId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvAction_row' + actionId));
        //AppPrivileges.GetFormPrivileges("Patient Family", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        utility.myConfirm('1', function () {
            var selectedValue = actionId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Admin_FollowUpAction.DeleteAction(selectedValue).done(function (response) {
                    if (response.status != false) {

                        Admin_FollowUpAction.ActionSearch();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '1'
        );
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },
    DeleteAction: function (actionId) {
        var data = "actionId=" + actionId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ACTION", "DELETE_ACTION");
    },
    EditFollowUpAction: function (actionId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvAction_row' + actionId));
        var params = [];
        params["ActionId"] = actionId;
        params["mode"] = "Edit";
        LoadActionPan('FollowUp_Action_Detail', params);
    },
    OpenAction: function () {
        var params = [];
        params["ActionId"] = "-1";
        params["mode"] = "Add";
        LoadActionPan('FollowUp_Action_Detail', params);
    },
    SelectedPageClick: function (PageNo, objPage, TotalRecords, rpp, pagingDivId) {
        Admin_FollowUpAction.ActionSearch(0, PageNo, 15);
    },
    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $('#' + Admin_FollowUpAction.params.PanelID + " #pnlAdminFollowUpAction_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_FollowUpAction.ActionSearch(0, currentPageNo, 15);
        }
    },
    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $('#' + Admin_FollowUpAction.params["PanelID"] + " #pnlAdminFollowUpAction_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            Admin_FollowUpAction.ActionSearch(0, currentPageNo, 15);
        }
    },

    ActionActiveInactive: function (FollowupActionId, isActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
 
        utility.myConfirm('3', function () {
            var selectedValue = FollowupActionId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Admin_FollowUpAction.ActionUpdateActiveInactive(selectedValue, isActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Admin_FollowUpAction.ActionSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
           '3', null, null, null, isActive
        );
    },

    ActionUpdateActiveInactive: function (FollowupActionId, isActive) {
        var data = "FollowupActionId=" + FollowupActionId + "&IsActive=" + isActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ACTION", "UPDATE_ACTION_ACTIVE_INACTIVE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}