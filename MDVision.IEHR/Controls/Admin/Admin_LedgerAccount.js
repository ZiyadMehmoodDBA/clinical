
Admin_LedgerAccount = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_LedgerAccount.bIsFirstLoad) {
            Admin_LedgerAccount.bIsFirstLoad = false;
            var self = $('#pnlAdminLedgerAccount');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {                
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlLedgerAccount_Search #divLedgerAccount_Entity").css("display", "none");
                //    $("#pnlLedgerAccount_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_LedgerAccount.LedgerAccountSearch();
            });
        }
    },

    LedgerAccountAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Ledger Account", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["LedgerAccountId"] = null;
                params["mode"] = "Add";

                LoadActionPan('ledgeraccountDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LedgerAccountEdit: function (LedgerAccountId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvLedgerAccount_row' + LedgerAccountId));
        AppPrivileges.GetFormPrivileges("Ledger Account", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = LedgerAccountId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["LedgerAccountId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('ledgeraccountDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LedgerAccountDelete: function (LedgerAccountId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvLedgerAccount_row' + LedgerAccountId));
        AppPrivileges.GetFormPrivileges("Ledger Account", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = LedgerAccountId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_LedgerAccount.DeleteLedgerAccount(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvLedgerAccount').DataTable();
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
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LedgerAccountActiveInactive: function (LedgerAccountId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Ledger Account", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = LedgerAccountId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        ledgeraccountDetail.UpdateLedgerAccountActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_LedgerAccount.LedgerAccountSearch('0');
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

    LedgerAccountSearch: function (LedgerAccountId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Ledger Account", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminLedgerAccount #pnlLedgerAccount_Result").css("display") == "none") {
                    $("#pnlAdminLedgerAccount #pnlLedgerAccount_Result").show();
                }

                var self = $("#pnlLedgerAccount_Search");
                var myJSON = self.getMyJSON();
                Admin_LedgerAccount.SearchLedgerAccount(myJSON, LedgerAccountId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_LedgerAccount.LedgerAccountGridLoad(response);
                        var TableControl = "pnlAdminLedgerAccount #dgvLedgerAccount";
                        var PagingPanelControlID = "pnlAdminLedgerAccount #divLedgerAccountPaging";
                        var ClassControlName = "Admin_LedgerAccount";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.LedgerAccountCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_LedgerAccount.LedgerAccountSearch(PrimaryID, PageNumber, ResultPerPage);
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

    LedgerAccountGridLoad: function (response) {
        $("#dgvLedgerAccount").dataTable().fnDestroy();
        $("#pnlLedgerAccount_Result #dgvLedgerAccount tbody").find("tr").remove();
        if (response.LedgerAccountCount > 0) {
            var LedgerAccountLoadJSONData = JSON.parse(response.LedgerAccountLoad_JSON);
            $.each(LedgerAccountLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_LedgerAccount.LedgerAccountEdit('" + item.LedgerAccountId + "',event);");
                $row.attr("id", "gvLedgerAccount_row" + item.LedgerAccountId);
                $row.attr("LedgerAccountId", item.LedgerAccountId);

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
                $row.append('<td style="display:none;">' + item.LedgerAccountId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_LedgerAccount.LedgerAccountDelete(\'' + item.LedgerAccountId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_LedgerAccount.LedgerAccountEdit(\'' + item.LedgerAccountId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_LedgerAccount.LedgerAccountActiveInactive(\'' + item.LedgerAccountId + '\', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td>');

                $("#pnlLedgerAccount_Result #dgvLedgerAccount tbody").last().append($row);
            });
        }
        else {
            $('#dgvLedgerAccount').DataTable({
                "language": {
                    "emptyTable": "No Ledger Account Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvLedgerAccount'))
            ;
        else
            $("#pnlLedgerAccount_Result #dgvLedgerAccount").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchLedgerAccount: function (LedgerAccountData, LedgerAccountId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "LedgerAccountData=" + LedgerAccountData + "&LedgerAccountID=" + LedgerAccountId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_LEDGER_ACCOUNT", "SEARCH_LEDGER_ACCOUNT");
    },

    DeleteLedgerAccount: function (LedgerAccountId) {
        var data = "LedgerAccountID=" + LedgerAccountId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_LEDGER_ACCOUNT_DETAIL", "DELETE_LEDGER_ACCOUNT");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}