
Admin_User = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_User.bIsFirstLoad) {
            Admin_User.bIsFirstLoad = false;

            var self = $('#pnlAdminUser');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#lstEntityId").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#lstEntityId").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_User.UserSearch('0');
            });
        }
    },

    UserAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Users", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["UserId"] = "-1";
                params["mode"] = "Add";
                params["IsUAdmin"] = "False";
                LoadActionPan('userDetail', params);
                //LoadActionPan('userDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    UserEdit: function (UserId, IsUAdmin, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvUser_row' + UserId));
        AppPrivileges.GetFormPrivileges("Users", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = UserId;
                var selectedAdmin = IsUAdmin;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["UserId"] = selectedValue;
                    params["IsUAdmin"] = selectedAdmin;
                    params["mode"] = "Edit";
                    LoadActionPan('userDetail', params);
                    //LoadActionPan('userDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    UserDelete: function (UserId, IsUAdmin, PageNo, rpp, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Users", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (IsUAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser)
                { utility.DisplayMessages("Admin User can't be Delete !", 3); }
                else
                {
                    utility.myConfirm('1', function () {
                        var selectedValue = UserId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Admin_User.DeleteUser(selectedValue).done(function (response) {
                                if (response.status != false) {
                                    //var table1 = $('#pnlAdminUser #dgvUsers').DataTable();
                                    //table1.row('.active').remove().draw(false);
                                    Admin_User.UserSearch('0', PageNo, rpp);
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
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    UserActiveInactive: function (UserId, IsActive, IsUAdmin, PageNo, rpp, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Users", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (IsUAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser)
                { utility.DisplayMessages("Admin User can't be Active / In Active !", 3); }
                else
                {
                    utility.myConfirm('3', function () {
                        var selectedValue = UserId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {

                            userDetail.UpdateUserActiveInactive(selectedValue, IsActive).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    Admin_User.UserSearch('0', '1', '15');
                                    UnloadActionPan();
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

    UserSearch: function (UserId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Users", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminUser #pnlUser_Result").css("display") == "none") {
                    $("#pnlAdminUser #pnlUser_Result").show();
                }

                var self = $("#pnlUser_Search");
                var myJSON = self.getMyJSON();

                Admin_User.SearchUser(myJSON, UserId, PageNo, rpp).done(function (response) {
                    if (response.status == true) {
                        Admin_User.UserGridLoad(response, PageNo, rpp);
                        if (response.status != false) {
                            var TableControl = "pnlAdminUser #dgvUsers";
                            var PagingPanelControlID = "pnlAdminUser #divUsersPaging";
                            var ClassControlName = "Admin_User";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.UserCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_User.UserSearch(PrimaryID, PageNumber, ResultPerPage);
                            }), 10);
                        }
                        else {
                            utility.DisplayMessages(strMessage, 3);
                        }
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



    UserGridLoad: function (response, PageNo, rpp) {
        //$('#pnlAdminUser #dgvUser_wrapper').find('.datatables-footer').remove();
        //$('#pnlAdminUser #dgvUser_wrapper').find('.datatables-header').remove();
        $("#pnlAdminUser #dgvUsers").dataTable().fnDestroy();
        $("#pnlAdminUser #pnlUser_Result #dgvUsers tbody").find("tr").remove();
        if (response.UserCount > 0) {
            var UserLoadJSONData = JSON.parse(response.UserLoad_JSON);
            $.each(UserLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_User.UserEdit('" + item.UserId + "','" + item.IsAdmin + "',event);");
                $row.attr("id", "gvUser_row" + item.UserId);
                $row.attr("UserId", item.UserId);
                $row.attr("UserName", item.UserName);
                $row.attr("FirstName", item.FirstName);
                $row.attr("LastName", item.LastName);
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
                $row.append('<td style="display:none;">' + item.UserId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_User.UserDelete(\'' + item.UserId + "','" + item.IsAdmin + "','" + PageNo + "','" + rpp + "'" + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_User.UserEdit(\'' + item.UserId + "','" + item.IsAdmin + "'" + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_User.UserActiveInactive(\'' + item.UserId + "'," + isactive + ",'" + item.IsAdmin + "','" + PageNo + "','" + rpp + "'" + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.UserName + '</td><td>' + item.FirstName + '</td><td>' + item.LastName + '</td><td>' + item.EntityName + '</td><td>' + admin + '</td>');

                $("#pnlUser_Result #dgvUsers tbody").last().append($row);
            });
        }
        else {
            $('#pnlAdminUser #divUsersPaging').css("display", "none");
            $('#pnlAdminUser #dgvUsers').DataTable({
                "language": {
                    "emptyTable": "No User Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#pnlAdminUser #dgvUsers'))
            ;
        else
            $("#pnlAdminUser #pnlUser_Result #dgvUsers").DataTable({ "bInfo": false, "bPaginate": false, "bSort": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $("#pnlAdminUser #pnlUser_Result #dgvUsers").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchUser: function (UserData, userID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "UserData=" + UserData + "&userID=" + userID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;

        return MDVisionService.defaultService(data, "ADMIN_USER", "SEARCH_USER");
    },

    DeleteUser: function (userID) {
        var data = "UserID=" + userID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_USER_DETAIL", "DELETE_USER");
    },

    SelectGridRow: function (obj) {
        if (obj.className != 'active') {
            $(obj).parent().children().removeClass('active');
            $(obj).addClass('active');
        }
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}

