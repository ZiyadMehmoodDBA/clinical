
Admin_EDITaxIDSetup = {
    bIsFirstLoad: true,
    params:null,
    Load: function (params) {
        Admin_EDITaxIDSetup.params = params;
        if (Admin_EDITaxIDSetup.bIsFirstLoad) {
            Admin_EDITaxIDSetup.bIsFirstLoad = false;
            var self = $('#pnlAdminEDITaxIDSetup');

            var self = null;
            if (Admin_EDITaxIDSetup.params.PanelID == "pnlAdminEDITaxIDSetup")
                self = $('#pnlAdminEDITaxIDSetup');
            else
                self = $('#' + Admin_EDITaxIDSetup.params.PanelID + ' #pnlAdminEDITaxIDSetup');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                    Admin_EDITaxIDSetup.EDITaxIDSetupSearch();
            });
        }
    },

    EDITaxIDSetupAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Tax ID Setup", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["EDITaxIDSetupId"] = null;
                params["mode"] = "Add";
                LoadActionPan('editaxidsetupDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDITaxIDSetupEdit: function (EDITaxIDSetupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDITaxIDSetup_row' + EDITaxIDSetupId));
        AppPrivileges.GetFormPrivileges("EDI Tax ID Setup", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = EDITaxIDSetupId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["EDITaxIDSetupId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('editaxidsetupDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDITaxIDSetupDelete: function (EDITaxIDSetupId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDITaxIDSetup_row' + EDITaxIDSetupId));
        AppPrivileges.GetFormPrivileges("EDI Tax ID Setup", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = EDITaxIDSetupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_EDITaxIDSetup.DeleteEDITaxIDSetup(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvEDITaxIDSetup').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_EDITaxIDSetup.EDITaxIDSetupSearch();
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

    LoadEntityBasedData: function (entityID) {

        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#pnlAdminEDITaxIDSetup #ddlClearinghouse', 'GetClearingHouse', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#pnlAdminEDITaxIDSetup #ddlClearinghouse', 'GetClearingHouse', false, null);
        }

    },

    EDITaxIDSetupActiveInactive: function (EDITaxIDSetupId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("EDI Tax ID Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = EDITaxIDSetupId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        editaxidsetupDetail.UpdateEDITaxIDSetupActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_EDITaxIDSetup.EDITaxIDSetupSearch('0');
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

    EDITaxIDSetupSearch: function (EDITaxIDSetupId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Tax ID Setup", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminEDITaxIDSetup #pnlEDITaxIDSetup_Result").css("display") == "none") {
                    $("#pnlAdminEDITaxIDSetup #pnlEDITaxIDSetup_Result").show();
                }

                var self = $("#pnlEDITaxIDSetup_Search");
                var myJSON = self.getMyJSON();

                Admin_EDITaxIDSetup.SearchEDITaxIDSetup(myJSON, EDITaxIDSetupId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_EDITaxIDSetup.EDITaxIDSetupGridLoad(response);
                        var TableControl = "pnlAdminEDITaxIDSetup #dgvEDITaxIDSetup";
                        var PagingPanelControlID = "pnlAdminEDITaxIDSetup #divEDITaxIDSetupPaging";
                        var ClassControlName = "Admin_EDITaxIDSetup";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.EDITaxIDSetupCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_EDITaxIDSetup.ReceiverSetupSearch(PrimaryID, PageNumber, ResultPerPage);
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

    EDITaxIDSetupGridLoad: function (response) {
        $("#dgvEDITaxIDSetup").dataTable().fnDestroy();
        $("#pnlEDITaxIDSetup_Result #dgvEDITaxIDSetup tbody").find("tr").remove();
        if (response.EDITaxIDSetupCount > 0) {
            var EDITaxIDSetupLoadJSONData = JSON.parse(response.EDITaxIDSetupLoad_JSON);
            $.each(EDITaxIDSetupLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_EDITaxIDSetup.EDITaxIDSetupEdit('" + item.EDITaxIDSetupId + "',event);");
                $row.attr("id", "gvEDITaxIDSetup_row" + item.EDITaxIDSetupId);
                $row.attr("EDITaxIDSetupId", item.EDITaxIDSetupId);

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

                $row.append('<td style="display:none;">' + item.EDITaxIDSetupId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDITaxIDSetup.EDITaxIDSetupDelete(' + item.EDITaxIDSetupId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDITaxIDSetup.EDITaxIDSetupEdit(' + item.EDITaxIDSetupId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDITaxIDSetup.EDITaxIDSetupActiveInactive(' + item.EDITaxIDSetupId + ', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.TaxID + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.EntityName + '</td><td>' + item.SubmitterName + '</td><td>' + item.ReveiverName + '</td>');

                $("#pnlEDITaxIDSetup_Result #dgvEDITaxIDSetup tbody").last().append($row);
            });
        }
        else {
            $('#dgvEDITaxIDSetup').DataTable({
                "language": {
                    "emptyTable": "No EDI Tax ID Setup Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvEDITaxIDSetup'))
            ;
        else
            $("#pnlEDITaxIDSetup_Result #dgvEDITaxIDSetup").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchEDITaxIDSetup: function (EDITaxIDSetupData, EDITaxIDSetupId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "EDITaxIDSetupData=" + EDITaxIDSetupData + "&EDITaxIDSetupID=" + EDITaxIDSetupId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_TAX_ID_SETUP", "SEARCH_EDI_TAX_ID_SETUP");
    },

    DeleteEDITaxIDSetup: function (EDITaxIDSetupId) {
        var data = "EDITaxIDSetupID=" + EDITaxIDSetupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_TAX_ID_SETUP_DETAIL", "DELETE_EDI_TAX_ID_SETUP");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}