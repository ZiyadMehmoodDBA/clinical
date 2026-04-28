
Admin_BillingProviderSettings = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_BillingProviderSettings.bIsFirstLoad) {
            Admin_BillingProviderSettings.bIsFirstLoad = false;
            var self = $('#pnlAdminBillingProviderSettings');
            self.loadDropDowns(true);

            AppPrivileges.GetFormPrivileges("Additional Billing Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_BillingProviderSettings.BillingProviderSettingsSearch();
                }
            });
        }
    },

    BillingProviderSettingsAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Additional Billing Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["BillingProviderSettingsId"] = null;
                params["mode"] = "Add";

                LoadActionPan('billingprovidersettingsDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BillingProviderSettingsEdit: function (BillingProviderSettingsId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvBillingProviderSettings_row' + BillingProviderSettingsId));
        AppPrivileges.GetFormPrivileges("Additional Billing Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = BillingProviderSettingsId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["BillingProviderSettingsId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('billingprovidersettingsDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BillingProviderSettingsDelete: function (BillingProviderSettingsId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvBillingProviderSettings_row' + BillingProviderSettingsId));
        AppPrivileges.GetFormPrivileges("Additional Billing Provider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = BillingProviderSettingsId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_BillingProviderSettings.DeleteBillingProviderSettings(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvBillingProviderSettings').DataTable();
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

    BillingProviderSettingsActiveInactive: function (BillingProviderSettingsId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Additional Billing Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = BillingProviderSettingsId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        billingprovidersettingsDetail.UpdateBillingProviderSettingsActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_BillingProviderSettings.BillingProviderSettingsSearch('0');
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

    BillingProviderSettingsSearch: function (BillingProviderSettingsId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Additional Billing Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminBillingProviderSettings #pnlBillingProviderSettings_Result").css("display") == "none") {
                    $("#pnlAdminBillingProviderSettings #pnlBillingProviderSettings_Result").show();
                }

                var self = $("#pnlBillingProviderSettings_Search");
                var myJSON = self.getMyJSON();

                Admin_BillingProviderSettings.SearchBillingProviderSettings(myJSON, BillingProviderSettingsId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_BillingProviderSettings.BillingProviderSettingsGridLoad(response);
                        var TableControl = "pnlAdminBillingProviderSettings #dgvBillingProviderSettings";
                        var PagingPanelControlID = "pnlAdminBillingProviderSettings #divBillingProviderPaging";
                        var ClassControlName = "Admin_BillingProviderSettings";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.BillingProviderSettingsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_BillingProviderSettings.BillingProviderSettingsSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                    }
                    else {
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BillingProviderSettingsGridLoad: function (response) {
        $("#dgvBillingProviderSettings").dataTable().fnDestroy();
        $("#pnlBillingProviderSettings_Result #dgvBillingProviderSettings tbody").find("tr").remove();
        if (response.BillingProviderSettingsCount > 0) {
            var BillingProviderSettingsLoadJSONData = JSON.parse(response.BillingProviderSettingsLoad_JSON);
            $.each(BillingProviderSettingsLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_BillingProviderSettings.BillingProviderSettingsEdit('" + item.BillingProviderId + "',event);");
                $row.attr("id", "gvBillingProviderSettings_row" + item.BillingProviderId);
                $row.attr("BillingProviderSettingsId", item.BillingProviderId);

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
                $row.append('<td style="display:none;">' + item.BillingProviderId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_BillingProviderSettings.BillingProviderSettingsDelete(' + item.BillingProviderId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_BillingProviderSettings.BillingProviderSettingsEdit(' + item.BillingProviderId + ',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_BillingProviderSettings.BillingProviderSettingsActiveInactive(' + item.BillingProviderId + ', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.InsuranceName + '</td><td>' + item.FacilityName + '</td><td>' + item.ProviderName + '</td>');


                $("#pnlBillingProviderSettings_Result #dgvBillingProviderSettings tbody").last().append($row);
            });
        }
        else {
            $('#dgvBillingProviderSettings').DataTable({
                "language": {
                    "emptyTable": "No Billing Provider Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvBillingProviderSettings'))
            ;
        else
            $("#pnlBillingProviderSettings_Result #dgvBillingProviderSettings").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            //$("#pnlBillingProviderSettings_Result #dgvBillingProviderSettings").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchBillingProviderSettings: function (BillingProviderSettingsData, BillingProviderSettingsId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "BillingProviderSettingsData=" + BillingProviderSettingsData + "&BillingProviderSettingsID=" + BillingProviderSettingsId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_SETTINGS", "SEARCH_BILLING_PROVIDER_SETTINGS");
    },

    DeleteBillingProviderSettings: function (BillingProviderSettingsId) {
        var data = "BillingProviderSettingsID=" + BillingProviderSettingsId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_SETTINGS_DETAIL", "DELETE_BILLING_PROVIDER_SETTINGS");
    },
    UpdateBillingProviderSettingsActiveInactive: function (BillingProviderSettingsID, IsActive) {
        var data = "BillingProviderSettingsID=" + BillingProviderSettingsID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_SETTINGS_DETAIL", "UPDATE_BILLING_PROVIDER_SETTINGS_ACTIVE_INACTIVE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}