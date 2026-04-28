Admin_BillingProvider = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_BillingProvider.params = params;
        if (Admin_BillingProvider.params["FromAdmin"] == "0" && Admin_BillingProvider.params["PanelID"] == 'pnlAdminBillingProvider')
            Admin_BillingProvider.params["FromAdmin"] = "1";
        if (Admin_BillingProvider.bIsFirstLoad) {
            Admin_BillingProvider.bIsFirstLoad = false;
            var self = "";
            if (Admin_BillingProvider.params["PanelID"] != "pnlAdminBillingProvider")
                self = $('#' + Admin_BillingProvider.params["PanelID"] + ' #pnlAdminBillingProvider')
            else
                self = $('#pnlAdminBillingProvider');

            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    $("#pnlBillingProvider_Search #ddlEntity").attr('disabled', 'disabled');
                    $("#pnlBillingProvider_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
            });

            AppPrivileges.GetFormPrivileges("Billing Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_BillingProvider.BillingProviderSearch();
                }
            });
            if (Admin_BillingProvider.params.Title != null)
                self.find("#headingTitle").text(Admin_BillingProvider.params.Title);
        }
    },

    BillingProviderAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Billing Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var param = [];
                param["BillingProviderId"] = null;
                param["mode"] = "Add";
                param["FromAdmin"] = Admin_BillingProvider.params["FromAdmin"];
                // params["ParentCtrl"] = Admin_BillingProvider.params["ParentCtrl"];
                if (Admin_BillingProvider.params["FromAdmin"] == "0") {
                    param["ParentCtrl"] = 'Admin_BillingProvider';
                }
                if (Admin_BillingProvider.params["PanelID"] == "pnlEncounterCreateClaim") {
                    param["unload"] = "CreateClaim"
                    LoadActionPan('Admin_BillingProvider_Detail', param, "pnlEncounterCreateClaim #pnlAdminBillingProvider");
                }
                else {
                    LoadActionPan('Admin_BillingProvider_Detail', param)
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BillingProviderEdit: function (BillingProviderId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvBillingProvider_row' + BillingProviderId));
        AppPrivileges.GetFormPrivileges("Billing Provider", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = BillingProviderId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var param = [];
                    param["BillingProviderId"] = selectedValue;
                    param["mode"] = "Edit";
                    param["FromAdmin"] = Admin_BillingProvider.params["FromAdmin"];
                    // params["ParentCtrl"] = Admin_BillingProvider.params["ParentCtrl"];
                    if (Admin_BillingProvider.params["FromAdmin"] == "0") {
                        param["ParentCtrl"] = 'Admin_BillingProvider';
                    }
                    if (Admin_BillingProvider.params["PanelID"] == "pnlEncounterCreateClaim") {
                        param["unload"] = "CreateClaim"
                        LoadActionPan('Admin_BillingProvider_Detail', param, "pnlEncounterCreateClaim #pnlAdminBillingProvider");
                    }
                    else {
                        LoadActionPan('Admin_BillingProvider_Detail', param)
                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BillingProviderDelete: function (BillingProviderId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#' + Admin_BillingProvider.params["PanelID"] + ' #gvBillingProvider_row' + BillingProviderId));
        AppPrivileges.GetFormPrivileges("Billing Provider", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = BillingProviderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_BillingProvider.DeleteBillingProvider(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#' + Admin_BillingProvider.params["PanelID"] + ' #dgvBillingProvider').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetBillingProviders', true);
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

    BillingProviderActiveInactive: function (BillingProviderId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Billing Provider", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = BillingProviderId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_BillingProvider.UpdateBillingProviderActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                CacheManager.BindCodes('GetBillingProviders', true);
                                Admin_BillingProvider.BillingProviderSearch('0');
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

    BillingProviderSearch: function (BillingProviderId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Billing Provider", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_BillingProvider.params["PanelID"] + " #pnlBillingProvider_Result").css("display") == "none") {
                    $("#" + Admin_BillingProvider.params["PanelID"] + " #pnlBillingProvider_Result").show();
                }

                var self = $("#pnlBillingProvider_Search");
                var myJSON = self.getMyJSON();

                Admin_BillingProvider.SearchBillingProvider(myJSON, BillingProviderId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_BillingProvider.BillingProviderGridLoad(response);
                        var TableControl = Admin_BillingProvider.params["PanelID"] + " #dgvBillingProvider";
                        var PagingPanelControlID = Admin_BillingProvider.params["PanelID"] + " #divBillingProviderPaging";
                        var ClassControlName = "Admin_BillingProvider";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.BillingProviderCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_BillingProvider.BillingProviderSearch(PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 2);
                    }

                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BillingProviderGridLoad: function (response) {
        $("#" + Admin_BillingProvider.params["PanelID"] + " #dgvBillingProvider").dataTable().fnDestroy();
        $("#" + Admin_BillingProvider.params["PanelID"] + " #pnlBillingProvider_Result #dgvBillingProvider tbody").find("tr").remove();
        if (response.BillingProviderCount > 0) {
            var BillingProviderSettingsLoadJSONData = JSON.parse(response.BillingProviderLoad_JSON);
            $.each(BillingProviderSettingsLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Admin_BillingProvider.BillingProviderEdit('" + item.BillingProviderId + "',event);");
                $row.attr("id", "gvBillingProvider_row" + item.BillingProviderId);
                $row.attr("BillingProviderId", item.BillingProviderId);

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

                var billto = "";
                if (item.ISEIN.toLowerCase() == 'true')
                    billto = "EIN";
                else
                    billto = "SSN";

                var selectProvider = "";//disabled
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                if (Admin_BillingProvider.params["FromAdmin"] == "0") {
                    //var FullName = item.LastName + ', ' + item.FirstName + ' ' + item.MI;
                    var selectMethod = "Admin_BillingProvider.FillProviderName('" + item.BillingProviderId + "','" + item.ShortName + "','" + item.EntityName + "',event);"
                    selectProvider = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';

                    if (ClassDisabled != "disabled")
                        $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_BillingProvider.ProviderEdit('" + item.ProviderId + "',event);");
                }
                if (Admin_BillingProvider.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    $row.append('<td style="display:none;">' + item.ProviderId + '</td><td>' + selectProvider + '</td><td>' + item.ShortName + '</td><td>' + item.LastName + '</td><td>' + item.FirstName + '</td><td>' + item.SpecialtyName + '</td><td>' + item.NPI + '</td><td>' + item.EntityName + '</td>');

                } else {
                    $('#btn-add').show();
                    $row.append('<td style="display:none;">' + item.BillingProviderId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_BillingProvider.BillingProviderDelete(' + item.BillingProviderId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_BillingProvider.BillingProviderEdit(' + item.BillingProviderId + ',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_BillingProvider.BillingProviderActiveInactive(' + item.BillingProviderId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectProvider + '</td><td>' + item.ShortName + '</td><td>' + item.EntityName + '</td><td>' + item.EIN + '</td><td>' + item.LastName + ', ' + item.FirstName + ' ' + item.MI + '</td><td>' + item.NPI + '</td><td>' + billto + '</td>');
                }

                $("#" + Admin_BillingProvider.params["PanelID"] + " #pnlBillingProvider_Result #dgvBillingProvider tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_BillingProvider.params["PanelID"] + ' #dgvBillingProvider').DataTable({
                "language": {
                    "emptyTable": "No Billing Provider Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_BillingProvider.params["PanelID"] + ' #dgvBillingProvider'))
            ;
        else
            $("#" + Admin_BillingProvider.params["PanelID"] + " #pnlBillingProvider_Result #dgvBillingProvider").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Admin_BillingProvider.params["PanelID"] + " #pnlBillingProvider_Result #dgvBillingProvider").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillProviderName: function (ProviderId, ProviderName, EntityName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtProvider";
        var RefCtrlHidden = " #hfProvider";
        var RefCtrlLabel = " #lblProvider";
        var RefCtrlLink = " #lnkProviderEdit";
        if (Admin_BillingProvider.params["RefCtrl"] != null)
            RefCtrl = " #" + Admin_BillingProvider.params["RefCtrl"];
        if (Admin_BillingProvider.params["RefCtrlHidden"] != null)
            RefCtrlHidden = " #" + Admin_BillingProvider.params["RefCtrlHidden"];
        if (Admin_BillingProvider.params["RefCtrlLabel"] != null)
            RefCtrlLabel = " #" + Admin_BillingProvider.params["RefCtrlLabel"];
        if (Admin_BillingProvider.params["RefCtrlLink"] != null)
            RefCtrlLink = " #" + Admin_BillingProvider.params["RefCtrlLink"];

        if (Admin_BillingProvider.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrl).val(ProviderName + ' - ' + EntityName).focus();
            else
                $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrl).val(ProviderName).focus();
        }
        else {
            $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrl).val(ProviderName).focus();
        }

        //$('#' + Admin_BillingProvider.params["PanelID"] + RefCtrl).val(ProviderName).focus();
        $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrlHidden).val(ProviderId);
        $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrlLabel).css("display", "none");
        $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrlLink).css("display", "inline");
        if (Admin_BillingProvider.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_BillingProvider.params["ProviderId"] = ProviderId;
            $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrlLabel).css("display", "inline");
        } else
            if (Admin_BillingProvider.params["IsOptional"] != null && Admin_BillingProvider.params["RefForm"] != null && Admin_BillingProvider.params["IsOptional"] == false && Admin_BillingProvider.params.ParentCtrl != "ERADetail") {
                $('#' + Admin_BillingProvider.params["PanelID"] + ' #' + Admin_BillingProvider.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrl).attr("name"));
            }
        // Start 02/02/2016 Adnan Maqbool Khan for bug # 3247
        //if ($("#" + Admin_BillingProvider.params["PanelID"] + " #" + Admin_BillingProvider.params["RefForm"]).data('bootstrapValidator') != null && typeof $("#" + Admin_BillingProvider.params["PanelID"] + " #" + Admin_BillingProvider.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
        //    $('#' + Admin_BillingProvider.params["PanelID"] + ' #' + Admin_BillingProvider.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrl).attr("name"));
        //}
        //end
        UnloadActionPan(Admin_BillingProvider.params["ParentCtrl"], "Admin_BillingProvider");
        $('#' + Admin_BillingProvider.params["PanelID"] + RefCtrl).focus();
    },
    SearchBillingProvider: function (BillingProviderData, BillingProviderId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "BillingProviderData=" + BillingProviderData + "&BillingProviderID=" + BillingProviderId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER", "SEARCH_BILLING_PROVIDER");
    },

    DeleteBillingProvider: function (BillingProviderId) {
        var data = "BillingProviderID=" + BillingProviderId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_DETAIL", "DELETE_BILLING_PROVIDER");
    },

    UpdateBillingProviderActiveInactive: function (BillingProviderID, IsActive) {
        var data = "BillingProviderID=" + BillingProviderID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_BILLING_PROVIDER_DETAIL", "UPDATE_BILLING_PROVIDER_ACTIVE_INACTIVE");
    },

    /*UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },*/
    UnLoadTab: function () {
        if (Admin_BillingProvider.params["FromAdmin"] == "0") {

            if (Admin_BillingProvider.params != null && Admin_BillingProvider.params.ParentCtrl != null && Admin_BillingProvider.params.PanelID != 'pnlAdminProvider') {
                UnloadActionPan(Admin_BillingProvider.params.ParentCtrl, 'Admin_BillingProvider', null, Admin_BillingProvider.params.PanelID);
            }
            else if (Admin_BillingProvider.params != null && Admin_BillingProvider.params.ParentCtrl != null) {
                UnloadActionPan(Admin_BillingProvider.params.ParentCtrl, 'Admin_BillingProvider');
            }
            else
                UnloadActionPan(null, 'Admin_BillingProvider');

        }
        else {
            RemoveAdminTab();
        }
    },
}