Clinical_Section = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Clinical_Section.bIsFirstLoad) {
            Clinical_Section.bIsFirstLoad = false;

            var self = $('#pnlClinicalSection');
            self.loadDropDowns(true);
            AppPrivileges.GetFormPrivileges("Section", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Clinical_Section.SectionSearch();
                }
            });
        }
    },

    SectionAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Section", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                params = [];
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

    SectionSearch: function (UserId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Section", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlClinicalSection #pnlSection_Result").css("display") == "none") {
                    $("#pnlClinicalSection #pnlSection_Result").show();
                }

                var self = $("#pnlSection_Search");
                var myJSON = self.getMyJSON();

                Clinical_Section.SearchSection(myJSON, UserId).done(function (response) {
                    if (response.status != false) {
                        Clinical_Section.SectionGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(strMessage, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchSection: function (UserData, userID) {
        var data = "UserData=" + UserData + "&userID=" + userID;

        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "SEARCH_TEMPLATE");
    },

    SectionGridLoad: function (response) {
        $("#pnlClinicalSection #dgvSection").dataTable().fnDestroy();
        $("#pnlClinicalSection #pnlSection_Result #dgvSection tbody").find("tr").remove();
        if (response.UserCount > 0) {
            var UserLoadJSONData = JSON.parse(response.UserLoad_JSON);
            $.each(UserLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvSection_row" + item.UserId + "'))");
                $row.attr("id", "gvSection_row" + item.UserId);
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
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    tglclass = "fa fa-toggle-on red";
                }
                $row.append('<td style="display:none;">' + item.UserId + '</td><td><a class="btn  btn-xs" href="#" onclick="Clinical_Section.SectionDelete(' + item.UserId + ",'" + item.IsAdmin + "'" + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Section.SectionEdit(' + item.UserId + ",'" + item.IsAdmin + "'" + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_Section.SectionActiveInactive(' + item.UserId + "," + isactive + ",'" + item.IsAdmin + "'" + ');" title="Inactive Record"><i class="' + tglclass + '"></i></a></td><td>' + item.UserName + '</td><td>' + item.FirstName + '</td><td>' + item.LastName + '</td><td>' + item.EntityName + '</td><td>' + admin + '</td>');

                $("#pnlSection_Result #dgvSection tbody").last().append($row);
            });
        }
        else {
            $('#pnlClinicalSection #dgvSection').DataTable({
                "language": {
                    "emptyTable": "No User Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#pnlClinicalSection #dgvSection'))
            ;
        else
            $("#pnlClinicalSection #pnlSection_Result #dgvSection").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SectionActiveInactive: function (UserId, IsActive, IsUAdmin) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Section", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
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
                                    Clinical_Section.SectionSearch('0');
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

    SectionEdit: function (UserId, IsUAdmin) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Section", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = UserId;
                var selectedAdmin = IsUAdmin;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    params = [];
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

    SectionDelete: function (UserId, IsUAdmin) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Section", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (IsUAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser)
                { utility.DisplayMessages("Admin User can't be Delete !", 3); }
                else
                {
                    utility.myConfirm('1', function () {
                        var selectedValue = UserId;
                        var oTable = $('#dgvSection').DataTable();
                        var ind = $(this).index();
                        var idx = oTable.row(this).index();
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Clinical_Section.DeleteUser(selectedValue).done(function (response) {
                                if (response.status != false) {
                                    var table1 = $('#dgvSection').DataTable();
                                    table1.row('.active').remove().draw(false);
                                    utility.DisplayMessages(response.Message, 1);
                                    //utility.DisplayMessages("Record Deleted Successfully.", 1);
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

    DeleteSection: function (userID) {
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