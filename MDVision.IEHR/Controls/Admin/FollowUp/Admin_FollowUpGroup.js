Admin_FollowUpGroup = {
    params: [],
    isFirstLoad: true,
    Load: function (params) {
        Admin_FollowUpGroup.params = params;
        if (Admin_FollowUpGroup.isFirstLoad) {
            Admin_FollowUpGroup.GroupSearch();
        }

    },
    GroupSearch: function (groupId, PageNo, rpp) {
        if ($("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result").css("display") == "none") {
            $("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result").show();
        }
        if ($("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result").css("display") == "none") {
            $("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result").show();
        }

        var self = "";
        self = $('#' + Admin_FollowUpGroup.params.PanelID);
        var myJSON = self.getMyJSON();

        Admin_FollowUpGroup.SearchGroup(myJSON, groupId, PageNo, rpp).done(function (response) {
            if (response.status != false) {

                //-----------------Pagination------------
                Admin_FollowUpGroup.FollowUpGroupGridLoad(response);
                if (response.GroupCount > 0) {
                    $('#' + Admin_FollowUpGroup.params.PanelID + " #divFollowUpGroupPaging").css("display", "inline");
                    var TableControl = Admin_FollowUpGroup.params["PanelID"] + " #dgvFollowUpGroup";
                    var PagingPanelControlID = Admin_FollowUpGroup.params["PanelID"] + " #divFollowUpGroupPaging";
                    var ClassControlName = "Admin_FollowUpGroup";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.GroupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                        Admin_FollowUpGroup.GroupSearch(PrimaryID, PageNumber, ResultPerPage);
                    }), 10);

                    //Showing 1 to 15 of 15 entries
                    //var RecordsPerPage = rpp != null ? rpp : 15;
                    //var CurrentPage = PageNo != null ? PageNo : 1;

                    //var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                    //var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                    //if (PageNo == null) {
                    //    utility.GetCustomPaging("divFollowUpGroupPaging", response.iTotalDisplayRecords, 5, "Admin_FollowUpGroup", CurrentPage, RecordsPerPage);
                    //}
                    //var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                    //var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                    //$('#' + Admin_FollowUpGroup.params.PanelID + " #divFollowUpGroupPaging #divShowingEntries").text(showingText);
                    //// Change Background Color to Black for selected page
                    //$('#' + Admin_FollowUpGroup.params.PanelID + " li").each(function () {
                    //    if ($(this).text() == CurrentPage) {
                    //        $(this).attr("class", "active");
                    //    }
                    //    else
                    //        $(this).removeAttr("class");
                    //});
                }
                else {
                    $('#' + Admin_FollowUpGroup.params.PanelID + " #divFollowUpGroupPaging").css("display", "none");
                }

                //--------------------End Pagination-------------------


            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    FollowUpGroupGridLoad: function (response) {
        $("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result #dgvFollowUpGroup").dataTable().fnDestroy();
        $("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result #dgvFollowUpGroup tbody").find("tr").remove();
        if (response.GroupCount > 0) {
            var LoadGroupJSONData = JSON.parse(response.FollowUpGroupLoad_JSON);
            $.each(LoadGroupJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_FollowUpGroup.EditFollowUpGroup('" + item.ARGroupId + "',event);");
                $row.attr("id", "gvGroup_row" + item.ARGroupId);
                $row.attr("GroupId", item.ARGroupId);
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


                $row.append('<td><a class="btn  btn-xs" href="#" onclick="Admin_FollowUpGroup.FollowGroupDelete(' + item.ARGroupId + ',event);" title="Delete FollowUp Group"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_FollowUpGroup.EditFollowUpGroup(' + item.ARGroupId + ',event);" title="Edit"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_FollowUpGroup.GroupActiveInactive(' + item.ARGroupId + "," + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectAction + '</td> <td class="max150 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.ShortName + '">' + item.ShortName + '</td> <td class="max250 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.Description + '">' + item.Description + '</td><td>' + utility.RemoveTimeFromDate(null, item.CreatedOn) + '</td><td>' + item.CreatedBy + '</td>');

                $("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result #dgvFollowUpGroup tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_FollowUpGroup.params.PanelID + " #divFollowUpGroupPaging").css("display", "none");
            $("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result #dgvFollowUpGroup").DataTable({
                "language": {
                    "emptyTable": "No FollowUp AR Group Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result #dgvFollowUpGroup"))
            ;
        else
            $("#" + Admin_FollowUpGroup.params.PanelID + " #pnlAdminFollowUpGroup_Result #dgvFollowUpGroup").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown



    },
    SearchGroup: function (groupData, groupId, pageNo, recordPerPage) {
        if (pageNo == null) {
            pageNo = 1;
        }
        if (recordPerPage == null) {
            recordPerPage = 15;
        }
        var data = "groupData=" + groupData + "&groupId=" + groupId + "&pageNo=" + pageNo + "&recordPerPage=" + recordPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "SEARCH_GROUP");
    },
    FollowUpGroupAdd: function () {
        var params = [];
        params["mode"] = "Add";
        params["ParentCtrl"] = 'adminTabFollowUpGroup';
        LoadActionPan('Admin_FollowUpGroup_Detail', params);
    },
    EditFollowUpGroup: function (ARGroupID, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvGroup_row' + ARGroupID));
        var params = [];
        params["mode"] = "edit";
        params["ParentCtrl"] = 'adminTabFollowUpGroup';
        params["GroupId"] = ARGroupID;
        LoadActionPan('Admin_FollowUpGroup_Detail', params);
    },

    FollowGroupDelete: function (groupId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvGroup_row' + groupId));
        utility.myConfirm('1', function () {
            var selectedValue = groupId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Admin_FollowUpGroup.DeleteGroup(selectedValue).done(function (response) {
                    if (response.status != false) {

                        Admin_FollowUpGroup.GroupSearch();
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

    },
    DeleteGroup: function (groupId) {
        var data = "groupId=" + groupId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "DELETE_GROUP");
    },

    GroupActiveInactive: function (ARGroupId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        // AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //  if (strMessage == "") {
        utility.myConfirm('3', function () {
            var selectedValue = ARGroupId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Admin_FollowUpGroup.GroupUpdateActiveInactive(selectedValue, IsActive).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Admin_FollowUpGroup.GroupSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '3', null, null, null, IsActive
        );
        // }
        //  else
        //   utility.DisplayMessages(strMessage, 2);
        // });
    },

    GroupUpdateActiveInactive: function (ARGroupId, IsActive) {
        var data = "ARGroupId=" + ARGroupId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_GROUP", "UPDATE_GROUP_ACTIVE_INACTIVE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}