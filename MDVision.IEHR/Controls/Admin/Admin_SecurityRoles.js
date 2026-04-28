
Admin_SecurityRoles = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_SecurityRoles.bIsFirstLoad) {
            Admin_SecurityRoles.bIsFirstLoad = false;
            AppPrivileges.GetFormPrivileges("Security Roles", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_SecurityRoles.SecurityRoleSearch();
                }
            });
        }
    },

    SecurityRoleAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Roles", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["SecurityRoleId"] = "-1";
                params["mode"] = "Add";
                params["IsAdmin"] = "False";
                LoadActionPan('securityRoleDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SecurityRoleEdit: function (SecurityRoleId, IsAdmin,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvSecurityRole_row' + SecurityRoleId));
        AppPrivileges.GetFormPrivileges("Security Roles", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = SecurityRoleId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["SecurityRoleId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["IsAdmin"] = IsAdmin;
                    LoadActionPan('securityRoleDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SecurityRoleDelete: function (SecurityRoleId, IsAdmin,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvSecurityRole_row' + SecurityRoleId));
        AppPrivileges.GetFormPrivileges("Security Roles", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (IsAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser) {
                    utility.DisplayMessages("Admin Security Role can't be Delete !", 3);
                }
                else {
                    utility.myConfirm('1', function () {
                        var selectedValue = SecurityRoleId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Admin_SecurityRoles.DeleteSecurityRole(selectedValue).done(function (response) {
                                if (response.status != false) {

                                    CacheManager.BindCodes('GetRoles', true);

                                    var table1 = $('#dgvSecurityRoles').DataTable();
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

    SecurityRoleActiveInactive: function (SecurityRoleId, IsActive, IsAdmin,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Security Roles", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (IsAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser) {
                    utility.DisplayMessages("Admin Security Role can't be Active / Inactive !", 3);
                }
                else {
                    utility.myConfirm('3', function () {
                        var selectedValue = SecurityRoleId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            securityRoleDetail.SecurityRoleActiveInactive(selectedValue, IsActive).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    Admin_SecurityRoles.SecurityRoleSearch('0');
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

    SecurityRoleSearch: function (SecurityRoleId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Security Roles", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminSecurityRoles #pnlSecurityRoles_Result").css("display") == "none") {
                    $("#pnlAdminSecurityRoles #pnlSecurityRoles_Result").show();
                }

                var self = $("#pnlSecurityRole_Search");
                var myJSON = self.getMyJSON();

                Admin_SecurityRoles.SearchSecurityRole(myJSON, SecurityRoleId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_SecurityRoles.SecurityRoleGridLoad(response);
                        var TableControl = "pnlAdminSecurityRoles #dgvSecurityRoles";
                        var PagingPanelControlID = "pnlAdminSecurityRoles #divRolesPaging";
                        var ClassControlName = "Admin_SecurityRoles";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.SecurityRoleCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_SecurityRoles.SecurityRoleSearch(PrimaryID, PageNumber, ResultPerPage);
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

    SecurityRoleGridLoad: function (response) {
        $("#dgvSecurityRoles").dataTable().fnDestroy();
        $("#pnlSecurityRoles_Result #dgvSecurityRoles tbody").find("tr").remove();
        if (response.SecurityRoleCount > 0) {
            var roleType = "";
            var SecurityRoleLoadJSONData = JSON.parse(response.SecurityRoleLoad_JSON);
            $.each(SecurityRoleLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_SecurityRoles.SecurityRoleEdit('" + item.RoleId + "','" + item.IsAdmin + "',event );");
                $row.attr("id", "gvSecurityRole_row" + item.RoleId);
                $row.attr("SecurityRoleId", item.RoleId);

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
                //Start || 13 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 
                if (item.RoleType == "True") {
                    roleType = "Emergency Access";
                }
                else {
                    roleType = "Regular";
                }
                //End   || 13 April, 2016 || ZeeshanAK || Changes for DOC 33- Emergency Access 

                $row.append('<td style="display:none;">' + item.RoleId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_SecurityRoles.SecurityRoleDelete(' + item.RoleId + ",'" + item.IsAdmin + "'" + ',event  );" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_SecurityRoles.SecurityRoleEdit(' + item.RoleId + ",'" + item.IsAdmin + "'" + ',event );"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_SecurityRoles.SecurityRoleActiveInactive(' + item.RoleId + ', ' + isactive + ',' + "'" + item.IsAdmin + "'" + ',event );" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.RoleName + '</td><td>' + roleType + '</td><td>' + item.Description + '</td>');


                $("#pnlSecurityRoles_Result #dgvSecurityRoles tbody").last().append($row);
            });
        }
        else {
            $('#dgvSecurityRoles').DataTable({
                "language": {
                    "emptyTable": "No Security Role Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvSecurityRoles'))
            ;
        else
            $("#pnlSecurityRoles_Result #dgvSecurityRoles").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
           // $("#pnlSecurityRoles_Result #dgvSecurityRoles").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchSecurityRole: function (SecurityRoleData, SecurityRoleId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "SecurityRoleData=" + SecurityRoleData + "&SecurityRoleID=" + SecurityRoleId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE", "SEARCH_SECURITY_ROLE");
    },

    DeleteSecurityRole: function (SecurityRoleId) {
        var data = "SecurityRoleID=" + SecurityRoleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SECURITY_ROLE_DETAIL", "DELETE_SECURITY_ROLE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}