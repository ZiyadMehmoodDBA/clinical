Admin_CoWorkersGroup = {
    bIsFirstLoad: true,
    Load: function (params) {
        Admin_CoWorkersGroup.params = params;
        if (Admin_CoWorkersGroup.bIsFirstLoad) {
            Admin_CoWorkersGroup.bIsFirstLoad = false;

            var self = $('#pnlAdminCoWorkersGroup');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#lstEntityId").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {

                Admin_CoWorkersGroup.CoWorkersGroupSearch();

            });
        }
    },
    CoWorkersGroupAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Co-workers Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["CoWorkersGroupId"] = "-1";
                params["mode"] = "Add";
                params["IsUAdmin"] = "False";
                LoadActionPan('Admin_CoWorkersGroupDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    CoWorkerGroupEdit: function (CoWorkersGroupID, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Co-workers Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.SelectGridRow($('#gvCoWorkersGroup_row' + CoWorkersGroupID));
                var selectedValue = CoWorkersGroupID;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["CoWorkersGroupID"] = selectedValue;
                    params["mode"] = "Edit";
                    //params["ParentCtrl"] = "Admin_CoWorkersGroup";
                    LoadActionPan('Admin_CoWorkersGroupDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    CoWorkersGroupSearch: function (CoWorkersGroupID, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Co-workers Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminCoWorkersGroup #pnlCoWorkersGroup_Result").css("display") == "none") {
                    $("#pnlAdminCoWorkersGroup #pnlCoWorkersGroup_Result").show();
                }

                var self = $("#pnlCoWorkersGroup_Search");
                var myJSON = self.getMyJSON();

                Admin_CoWorkersGroup.SearchCoWorkersGroup(myJSON, CoWorkersGroupID, PageNo, rpp).done(function (response) {
                    if (response.status == true) {
                        Admin_CoWorkersGroup.CoWorkersGroupGridLoad(response);
                        var TableControl = "pnlAdminCoWorkersGroup #dgvCoWorkersGroup";
                        var PagingPanelControlID = "pnlAdminCoWorkersGroup #divCoWorkersGroupPaging";
                        var ClassControlName = "Admin_CoWorkersGroup";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.CoWorkersCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_CoWorkersGroup.CoWorkersGroupSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                    }
                    else {
                        Admin_CoWorkersGroup.CoWorkersGroupGridLoad(response);
                        var TableControl = "pnlAdminCoWorkersGroup #dgvCoWorkersGroup";
                        var PagingPanelControlID = "pnlAdminCoWorkersGroup #divCoWorkersGroupPaging";
                        var ClassControlName = "Admin_CoWorkersGroup";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.CoWorkersCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_CoWorkersGroup.CoWorkersGroupSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                        if (response.Message) {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    CoWorkersGroupGridLoad: function (response) {
        $("#dgvCoWorkersGroup").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#pnlCoWorkersGroup_Result #dgvCoWorkersGroup tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.CoWorkersCount > 0) {
            var CoWorkersGroupJSONData = JSON.parse(response.CoWorkersGroup_JSON); //Parsing array to JSON
            $.each(CoWorkersGroupJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_CoWorkersGroup.CoWorkerGroupEdit('" + item.CoWorkersGroupId + "',event);");
                $row.attr("id", "gvCoWorkersGroup_row" + item.CoWorkersGroupId);

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
                $row.append('<td style="display:none;">' + item.CoWorkersGroupId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_CoWorkersGroup.CoWorkerGroupDelete(\'' + item.CoWorkersGroupId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_CoWorkersGroup.CoWorkerGroupEdit(\'' + item.CoWorkersGroupId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_CoWorkersGroup.CoWorkerGroupActiveInactive(\'' + item.CoWorkersGroupId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + '</td><td>' + item.Name + '</td>');

                $("#pnlCoWorkersGroup_Result #dgvCoWorkersGroup tbody").last().append($row);
            });
        }
        else {
            $('#pnlCoWorkersGroup_Result #dgvCoWorkersGroup').DataTable({
                "language": {
                    "emptyTable": "No Co-Worker Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlCoWorkersGroup_Result #dgvCoWorkersGroup'))
            ;
        else {
            $("#pnlCoWorkersGroup_Result #dgvCoWorkersGroup").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        }
    },
    SearchCoWorkersGroup: function (UserData, CoWorkersGroupID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "Data=" + UserData + "&CoWorkersGroupID=" + CoWorkersGroupID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;

        return MDVisionService.defaultService(data, "ADMIN_COWORKERGROUP", "SEARCH_COWORKERSGROUP");
    },
    CoWorkerGroupDelete: function (GroupId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Co-workers Group", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = GroupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_CoWorkersGroup.DeleteCoWorkerGroup(selectedValue).done(function (response) {
                            if (response.status != false) {
                                Admin_CoWorkersGroup.CoWorkersGroupSearch();
                                utility.DisplayMessages(response.Message, 1);
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
        });
    },
    DeleteCoWorkerGroup: function (GroupId) {
        var data = "CoWorkerGroupIdID=" + GroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_COWORKERGROUP", "DELETE_COWORKERSGROUP");
    },
    CoWorkerGroupActiveInactive: function (CoWorkerGroupId, IsActive, event) {
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Co-workers Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = CoWorkerGroupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_CoWorkersGroup.UpdateCoWorkerGroupActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.Message, 1);
                                Admin_CoWorkersGroup.CoWorkersGroupSearch();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () {
                },
                  '3', null, null, null, IsActive
                );
            }
        });
    },
    UpdateCoWorkerGroupActiveInactive: function (CoWorkerGroupId, IsActive) {
        var data = "CoWorkerGroupId=" + CoWorkerGroupId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_COWORKERGROUP", "UPDATE_COWORKERSGROUP_ACTIVE_INACTIVE");
    },
    //AST-332
    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);

    }
}