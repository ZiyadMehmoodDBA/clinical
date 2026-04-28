Clinical_Template = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Clinical_Template.bIsFirstLoad) {
            Clinical_Template.bIsFirstLoad = false;

            var self = $('#pnlClinicalTemplate');
            self.loadDropDowns(true);
            AppPrivileges.GetFormPrivileges("Template", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Clinical_Template.TemplateSearch();
                }
            });
        }
    },

    TemplateAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
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

    TemplateSearch: function (UserId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlClinicalTemplate #pnlTemplate_Result").css("display") == "none") {
                    $("#pnlClinicalTemplate #pnlTemplate_Result").show();
                }

                var self = $("#pnlTemplate_Search");
                var myJSON = self.getMyJSON();

                Clinical_Template.SearchTemplate(myJSON, UserId).done(function (response) {
                    if (response.status != false) {
                        Clinical_Template.TemplateGridLoad(response);
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

    SearchTemplate: function (UserData, userID) {
        var data = "UserData=" + UserData + "&userID=" + userID;

        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "SEARCH_TEMPLATE");
    },

    TemplateGridLoad: function (response) {
        $("#pnlClinicalTemplate #dgvTemplate").dataTable().fnDestroy();
        $("#pnlClinicalTemplate #pnlTemplate_Result #dgvTemplate tbody").find("tr").remove();
        if (response.UserCount > 0) {
            var UserLoadJSONData = JSON.parse(response.UserLoad_JSON);
            $.each(UserLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvTemplate_row" + item.UserId + "'))");
                $row.attr("id", "gvTemplate_row" + item.UserId);
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
                $row.append('<td style="display:none;">' + item.UserId + '</td><td><a class="btn  btn-xs" href="#" onclick="Clinical_Template.TemplateDelete(' + item.UserId + ",'" + item.IsAdmin + "'" + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_Template.TemplateEdit(' + item.UserId + ",'" + item.IsAdmin + "'" + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_Template.TemplateActiveInactive(' + item.UserId + "," + isactive + ",'" + item.IsAdmin + "'" + ');" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.UserName + '</td><td>' + item.FirstName + '</td><td>' + item.LastName + '</td><td>' + item.EntityName + '</td><td>' + admin + '</td>');

                $("#pnlTemplate_Result #dgvTemplate tbody").last().append($row);
            });
        }
        else {
            $('#pnlClinicalTemplate #dgvTemplate').DataTable({
                "language": {
                    "emptyTable": "No User Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }


        if ($.fn.dataTable.isDataTable('#pnlClinicalTemplate #dgvTemplate'))
            ;
        else
            $("#pnlClinicalTemplate #pnlTemplate_Result #dgvTemplate").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    TemplateActiveInactive: function (UserId, IsActive, IsUAdmin) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
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
                                    Clinical_Template.TemplateSearch('0');
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

    TemplateEdit: function (UserId, IsUAdmin) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
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

    TemplateDelete: function (UserId, IsUAdmin) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Template", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                if (IsUAdmin == "True" && globalAppdata['AppUserName'] != DefaultUser)
                { utility.DisplayMessages("Admin User can't be Delete !", 3); }
                else
                {
                    utility.myConfirm('1', function () {
                        var selectedValue = UserId;
                        var oTable = $('#dgvTemplate').DataTable();
                        var ind = $(this).index();
                        var idx = oTable.row(this).index();
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Clinical_Template.DeleteUser(selectedValue).done(function (response) {
                                if (response.status != false) {
                                    var table1 = $('#dgvTemplate').DataTable();
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

    DeleteTemplate: function (userID) {
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