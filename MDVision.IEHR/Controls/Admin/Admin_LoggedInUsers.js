Admin_LoggedInUsers = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_LoggedInUsers.bIsFirstLoad) {
            Admin_LoggedInUsers.bIsFirstLoad = false;

            var self = $('#pnlAdminLoggedInUsers');

            self.loadDropDowns(true).done(function () {

                Admin_LoggedInUsers.UserSearch();
            });
        }
    },

    UserEdit: function (UserId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvUser_row' + UserId));
        //AppPrivileges.GetFormPrivileges("Users", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        var selectedValue = UserId;
        // var selectedAdmin = IsUAdmin;
        if (selectedValue) {

            var params = [];
            params["UserId"] = selectedValue;
            params["RefCtrl"] = "LoggedInUsers";
            params["mode"] = "Edit";
            LoadActionPan('userDetail', params);
        }
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    SuspendUserSession: function (SessionId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('Are you sure you want to Suspend this User\'s Session', function () {
            //var SessionId = SessionId1;
            if (SessionId) {

                Admin_LoggedInUsers.UserSessionSuspend(SessionId).done(function (response) {
                    if (response.status != false) {

                        Admin_LoggedInUsers.UserSearch();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () {

        },
            'Confirm Suspend'
        );

    },

    UserSearch: function () {

        if ($("#pnlAdminLoggedInUsers #pnlUser_Result").css("display") == "none") {
            $("#pnlAdminLoggedInUsers #pnlUser_Result").show();
        }

        var self = $("#pnlLoggedInUsers_Search");
        var myJSON = self.getMyJSON();

        Admin_LoggedInUsers.SearchUser(myJSON).done(function (response) {
            if (response.status == true) {
                Admin_LoggedInUsers.UserGridLoad(response);
                if (response.status != false) {
                    var TableControl = "pnlAdminLoggedInUsers #dgvLoggedinUsers";
                    var PagingPanelControlID = "pnlAdminLoggedInUsers #divUsersPaging";
                    var ClassControlName = "AdminLoggedInUsers";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout(CreatePagination(response.UserCount, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function () {
                        Admin_LoggedInUsers.UserSearch();
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
    },

    UserGridLoad: function (response) {
        $("#pnlAdminLoggedInUsers #dgvLoggedinUsers").dataTable().fnDestroy();
        $("#pnlAdminLoggedInUsers #pnlUser_Result #dgvLoggedinUsers tbody").find("tr").remove();
        if (response.loggedInUsersCount > 0) {
            var UserLoadJSONData = JSON.parse(response.LoggedInusers_JSON);
            $.each(UserLoadJSONData, function (i, item) {

                var sessionStatusMessage = "";
                if (item.IsSuspended) {
                    sessionStatusMessage = "Suspended";
                }
                else if (item.IsLogin) {
                    sessionStatusMessage = "Active";
                }
                else {
                    sessionStatusMessage = "Logged out";
                }
                var $row = $('<tr/>');
                //   $row.attr("onclick", "Admin_LoggedInUsers.UserEdit('" + item.AppUserId + "',event);");
                $row.attr("id", "gvUser_row" + item.SessionId);

                var EditMethod = '<a class="btn btn-xs" href="#" onclick="Admin_LoggedInUsers.UserEdit(\'' + item.AppUserId + '\',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>';
                var SuspendMethod = '<a class="btn btn-danger btn-xs mr-xs" onclick="Admin_LoggedInUsers.SuspendUserSession(\'' + item.SessionId + '\',event);">Suspend</a>';

                $row.append(
                    '<td style="display:none;">' + item.SessionId + '</td>' +
                    '<td>' + EditMethod + '</td>' +
                    '<td>' + item.AppUsername + '</td>' +
                    '<td>' + $("#lstEntityId option[value='" + item.EntityId + "']").text() + '</td>' +
                    '<td>' + item.BrowserName + '</td>' +
                    '<td>' + item.BrowserVersion + '</td>' +
                    '<td>' + item.IP + '</td>' +
                    '<td>' + item.Platform + '</td>' +
                    '<td>' + item.MachineInfo + '</td>' +
                    '<td>' + item.LastActivity + '</td>' +
                    '<td>' + sessionStatusMessage + '</td>' +
                    '<td>' + SuspendMethod + '</td>'
                    );

                $("#pnlUser_Result #dgvLoggedinUsers tbody").last().append($row);
            });
        }
        else {
            $('#pnlAdminLoggedInUsers #divUsersPaging').css("display", "none");
            $('#pnlAdminLoggedInUsers #dgvLoggedinUsers').DataTable({
                "language": {
                    "emptyTable": "No Records"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if (!$.fn.dataTable.isDataTable('#pnlAdminLoggedInUsers #dgvLoggedinUsers'))
            $("#pnlAdminLoggedInUsers #pnlUser_Result #dgvLoggedinUsers").DataTable({ "bInfo": false, "bPaginate": false, "bSort": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchUser: function (UserData) {

        var data = "UserData=" + UserData;

        return MDVisionService.defaultService(data, "ADMIN_USER", "SEARCH_LOGGED_IN_USER");
    },

    UserSessionSuspend: function (SessionId) {
        var data = "SessionId=" + SessionId;
        return MDVisionService.defaultService(data, "ADMIN_USER", "SUSPEND_SESSION");
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

