
Admin_PrivilegeGroup = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_PrivilegeGroup.bIsFirstLoad) {
            Admin_PrivilegeGroup.bIsFirstLoad = false;

            AppPrivileges.GetFormPrivileges("Security Entity Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_PrivilegeGroup.PrivilegeGroupSearch();
                }
            });
        }
    },

    PrivilegeGroupAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Entity Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PrivilegeGroupId"] = "-1";
                params["mode"] = "Add";
                params["IsAdmin"] = "False";
                LoadActionPan('privilegeGroupDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PrivilegeGroupEdit: function (SecurityRoleId, IsAdmin,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Security Entity Group", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = SecurityRoleId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["PrivilegeGroupId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["IsAdmin"] = IsAdmin;
                    LoadActionPan('privilegeGroupDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PrivilegeGroupDelete: function (PrivilegeGroupId, IsAdmin,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPrivilegeGroup_row' + PrivilegeGroupId));
        AppPrivileges.GetFormPrivileges("Security Entity Group", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (IsAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser) {
                    utility.DisplayMessages("Admin Privilege Group can't be Delete !", 3);
                }
                else {
                    utility.myConfirm('1', function () {
                        var isAdmin = $("#privilegeGroupDetail #gvPrivilegeGroup_row" + PrivilegeGroupId).attr("IsAdmin");
                        var selectedValue = PrivilegeGroupId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Admin_PrivilegeGroup.DeletePrivilegeGroup(selectedValue).done(function (response) {
                                if (response.status != false) {
                                    var table1 = $('#dgvPrivilegeGroup').DataTable();
                                    table1.row('.active').remove().draw(false);
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
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PrivilegeGroupActiveInactive: function (PrivilegeGroupId, IsActive, IsAdmin,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Security Entity Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (IsAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser) {
                    utility.DisplayMessages("Admin Privilege Group can't be Active / Inactive !", 3);
                }
                else {
                    utility.myConfirm('3', function () {
                        var selectedValue = PrivilegeGroupId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            privilegeGroupDetail.UpdatePrivilegeGroupActiveInactive(selectedValue, IsActive).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    Admin_PrivilegeGroup.PrivilegeGroupSearch('0');
                                    //UnloadActionPan();
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
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PrivilegeGroupSearch: function (PrivilegeGroupId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Entity Group", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminPrivilegeGroup #pnlPrivilegeGroup_Result").css("display") == "none") {
                    $("#pnlAdminPrivilegeGroup #pnlPrivilegeGroup_Result").show();
                }

                var self = $("#pnlPrivilegeGroup_Search");
                var myJSON = self.getMyJSON();

                Admin_PrivilegeGroup.SearchPrivilegeGroup(myJSON, PrivilegeGroupId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_PrivilegeGroup.PrivilegeGroupGridLoad(response);
                        var TableControl = "pnlAdminPrivilegeGroup #dgvPrivilegeGroup";
                        var PagingPanelControlID = "pnlAdminPrivilegeGroup #divPrivilegeGroupPaging";
                        var ClassControlName = "Admin_PrivilegeGroup";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.PrivilegeGroupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_PrivilegeGroup.PrivilegeGroupSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);

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

    PrivilegeGroupGridLoad: function (response) {
        $("#dgvPrivilegeGroup").dataTable().fnDestroy();
        $("#pnlPrivilegeGroup_Result #dgvPrivilegeGroup tbody").find("tr").remove();
        if (response.PrivilegeGroupCount > 0) {
            var PrivilegeGroupLoadJSONData = JSON.parse(response.PrivilegeGroupLoad_JSON);
            $.each(PrivilegeGroupLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_PrivilegeGroup.PrivilegeGroupEdit('" + item.SecGroupId + "','" + item.IsAdmin + "'" + ",event);");
                $row.attr("id", "gvPrivilegeGroup_row" + item.SecGroupId);
                $row.attr("PrivilegeGroupId", item.SecGroupId);
                $row.attr("IsAdmin", item.IsAdmin);

                if (item.IsAdmin == "True")
                    admin = 'Yes';
                else
                    admin = 'No';

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

                $row.append('<td style="display:none;">' + item.SecGroupId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_PrivilegeGroup.PrivilegeGroupDelete(' + item.SecGroupId + ",'" + item.IsAdmin + "'" + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_PrivilegeGroup.PrivilegeGroupEdit(' + item.SecGroupId + ",'" + item.IsAdmin + "'" + ',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_PrivilegeGroup.PrivilegeGroupActiveInactive(' + item.SecGroupId + ', ' + isactive + ',' + "'" + item.IsAdmin + "'" + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + admin + '</td>');

                $("#pnlPrivilegeGroup_Result #dgvPrivilegeGroup tbody").last().append($row);
            });
        }
        else {
            $('#dgvPrivilegeGroup').DataTable({
                "language": {
                    "emptyTable": "No Security Entity Group Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvPrivilegeGroup'))
            ;
        else
            $("#pnlPrivilegeGroup_Result #dgvPrivilegeGroup").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            // $("#pnlPrivilegeGroup_Result #dgvPrivilegeGroup").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchPrivilegeGroup: function (PrivilegeGroupData, PrivilegeGroupId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "PrivilegeGroupData=" + PrivilegeGroupData + "&PrivilegeGroupID=" + PrivilegeGroupId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP", "SEARCH_PRIVILEGE_GROUP");
    },

    DeletePrivilegeGroup: function (PrivilegeGroupId) {
        var data = "PrivilegeGroupID=" + PrivilegeGroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PRIVILEGE_GROUP_DETAIL", "DELETE_PRIVILEGE_GROUP");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}