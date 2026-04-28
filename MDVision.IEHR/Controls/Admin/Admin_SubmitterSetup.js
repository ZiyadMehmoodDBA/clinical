
Admin_SubmitterSetup = {
    bIsFirstLoad: true,
    params:null,
    Load: function (params) {
        Admin_SubmitterSetup.params = params;
        if (Admin_SubmitterSetup.bIsFirstLoad) {
            Admin_SubmitterSetup.bIsFirstLoad = false;
            var self = $('#pnlAdminSubmitterSetup');

            var self = null;
            if (Admin_SubmitterSetup.params.PanelID == "pnlAdminSubmitterSetup")
                self = $('#pnlAdminSubmitterSetup');
            else
                self = $('#' + Admin_SubmitterSetup.params.PanelID + ' #pnlAdminSubmitterSetup');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }

            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_SubmitterSetup.SubmitterSetupSearch();

            });
        }
    },

    SubmitterSetupAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Submitter Setup", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["SubmitterSetupId"] = null;
                params["mode"] = "Add";
                LoadActionPan('submitterSetupDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SubmitterSetupEdit: function (SubmitterSetupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Submitter Setup", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = SubmitterSetupId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["SubmitterSetupId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('submitterSetupDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SubmitterSetupDelete: function (SubmitterSetupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Submitter Setup", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = SubmitterSetupId;
                    var oTable = $('#dgvSubmitterSetup').DataTable();
                    var ind = $(this).index();
                    var idx = oTable.row(this).index();
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_SubmitterSetup.DeleteSubmitterSetup(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvSubmitterSetup').DataTable();
                                table1.row('.active').remove().draw(false);
                                Admin_SubmitterSetup.SubmitterSetupSearch();
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
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SubmitterSetupActiveInactive: function (SubmitterSetupId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Submitter Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = SubmitterSetupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        submitterSetupDetail.UpdateSubmitterSetupActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_SubmitterSetup.SubmitterSetupSearch('0');
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

    SubmitterSetupSearch: function (SubmitterSetupId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Submitter Setup", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminSubmitterSetup #pnlSubmitterSetup_Result").css("display") == "none") {
                    $("#pnlAdminSubmitterSetup #pnlSubmitterSetup_Result").show();
                }

                var self = $("#pnlSubmitterSetup_Search");
                var myJSON = self.getMyJSON();

                Admin_SubmitterSetup.SearchSubmitterSetup(myJSON, SubmitterSetupId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_SubmitterSetup.SubmitterSetupGridLoad(response);
                        var TableControl = "pnlAdminSubmitterSetup #dgvSubmitterSetup";
                        var PagingPanelControlID = "pnlAdminSubmitterSetup #divSubmitterSetupPaging";
                        var ClassControlName = "Admin_SubmitterSetup";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.SubmitterSetupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_SubmitterSetup.SubmitterSetupSearch(PrimaryID, PageNumber, ResultPerPage);
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

    SubmitterSetupGridLoad: function (response) {
        $("#dgvSubmitterSetup").dataTable().fnDestroy();
        $("#pnlSubmitterSetup_Result #dgvSubmitterSetup tbody").find("tr").remove();
        if (response.SubmitterSetupCount > 0) {
            var SubmitterSetupLoadJSONData = JSON.parse(response.SubmitterSetupLoad_JSON);
            $.each(SubmitterSetupLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_SubmitterSetup.SubmitterSetupEdit('" + item.SubmitterSetupId + "',event);");
                $row.attr("id", "gvSubmitterSetup_row" + item.SubmitterSetupId);
                $row.attr("SubmitterSetupId", item.SubmitterSetupId);
                if (item.isActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                $row.append('<td style="display:none;">' + item.SubmitterSetupId + '</td><td><a class="btn  btn-xs" href="#"onclick="Admin_SubmitterSetup.SubmitterSetupDelete(\'' + item.SubmitterSetupId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_SubmitterSetup.SubmitterSetupEdit(\'' + item.SubmitterSetupId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#"  onclick="Admin_SubmitterSetup.SubmitterSetupActiveInactive(\'' + item.SubmitterSetupId + '\',' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.SS1000ANM103 + '</td><td>' + item.SS1000ANM104 + '</td>');

                $("#pnlSubmitterSetup_Result #dgvSubmitterSetup tbody").last().append($row);
            });
        }
        else {
            $('#dgvSubmitterSetup').DataTable({
                "language": {
                    "emptyTable": "No Submitter Setup found"
                },
                "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvSubmitterSetup'))
            ;
        else
            $("#pnlSubmitterSetup_Result #dgvSubmitterSetup").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchSubmitterSetup: function (SubmitterSetupData, SubmitterSetupID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "SubmitterSetupData=" + SubmitterSetupData + "&SubmitterSetupID=" + SubmitterSetupID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SUBMITTER_SETUP", "SEARCH_SUBMITTER_SETUP");
    },

    DeleteSubmitterSetup: function (SubmitterSetupID) {
        var data = "SubmitterSetupID=" + SubmitterSetupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SUBMITTER_SETUP_DETAIL", "DELETE_SUBMITTER_SETUP");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}

