
Admin_ClearingHouse = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {

        Admin_ClearingHouse.params = params;

        if (Admin_ClearingHouse.params["FromAdmin"] == "0" && Admin_ClearingHouse.params["PanelID"] == 'pnlAdminClearingHouse')
            Admin_ClearingHouse.params["FromAdmin"] = "1";

        if (Admin_ClearingHouse.bIsFirstLoad) {
            Admin_ClearingHouse.bIsFirstLoad = false;

            var self = "";//$('#pnlAdminClearingHouse');
            if (Admin_ClearingHouse.params["PanelID"] != 'pnlAdminClearingHouse')
                self = $('#' + Admin_ClearingHouse.params["PanelID"] + ' #pnlAdminClearingHouse');
            else
                self = $('#pnlAdminClearingHouse');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }

            self.loadDropDowns(true).done(function () {                
                //if (globalAppdata['IsAdmin'] != "True"){
                //    $("#" + Admin_ClearingHouse.params["PanelID"] + " #pnlClearingHouse_Search #divClearingHouse_Entity").css("display", "none");
                //    $("#" + Admin_ClearingHouse.params["PanelID"] + " #pnlClearingHouse_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_ClearingHouse.ClearingHouseSearch();
            });
        }
    },

    ClearingHouseAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clearinghouse", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ClearingHouseId"] = null;
                params["mode"] = "Add";
                LoadActionPan('clearingHouseDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ClearingHouseEdit: function (ClearingHouseId,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($('#gvClearingHouse_row' + ClearingHouseId));
        AppPrivileges.GetFormPrivileges("Clearinghouse", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ClearingHouseId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ClearingHouseId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('clearingHouseDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ClearingHouseDelete: function (ClearingHouseId,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($('#gvClearingHouse_row' + ClearingHouseId));
        AppPrivileges.GetFormPrivileges("Clearinghouse", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ClearingHouseId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ClearingHouse.DeleteClearingHouse(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $("#" + Admin_ClearingHouse.params["PanelID"] + " #dgvClearingHouse").DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_ClearingHouse.ClearingHouseSearch();
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

    ClearingHouseActiveInactive: function (ClearingHouseId, IsActive,event) {
        var strMessage = "";
	if (event != null) {
            event.stopPropagation();
	}
	utility.SelectGridRow($('#gvClearingHouse_row' + ClearingHouseId));
        AppPrivileges.GetFormPrivileges("Clearinghouse", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ClearingHouseId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        clearingHouseDetail.UpdateClearingHouseActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_ClearingHouse.ClearingHouseSearch('0');
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

    ClearingHouseSearch: function (ClearingHouseId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clearinghouse", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_ClearingHouse.params["PanelID"] + " #pnlClearingHouse_Result").css("display") == "none") {
                    $("#" + Admin_ClearingHouse.params["PanelID"] + " #pnlClearingHouse_Result").show();
                }

                var self = $("#" + Admin_ClearingHouse.params["PanelID"] + " #pnlClearingHouse_Search");
                var myJSON = self.getMyJSON();

                Admin_ClearingHouse.SearchClearingHouse(myJSON, ClearingHouseId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_ClearingHouse.ClearingHouseGridLoad(response);
                        var TableControl = "pnlAdminClearingHouse #dgvClearingHouse";
                        var PagingPanelControlID = "pnlAdminClearingHouse #divClearingHousePaging";
                        var ClassControlName = "Admin_ClearingHouse";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ClearingHouseCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_ClearingHouse.ClearingHouseSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ClearingHouseGridLoad: function (response) {
        $("#" + Admin_ClearingHouse.params["PanelID"] + " #dgvClearingHouse").dataTable().fnDestroy();
        $("#" + Admin_ClearingHouse.params["PanelID"] + " #pnlClearingHouse_Result #dgvClearingHouse tbody").find("tr").remove();
        if (response.ClearingHouseCount > 0) {
            var ClearingHouseLoadJSONData = JSON.parse(response.ClearingHouseLoad_JSON);
            $.each(ClearingHouseLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                
                $row.attr("id", "gvClearingHouse_row" + item.ClearingHouseId);
                $row.attr("ClearingHouseId", item.ClearingHouseId);
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

                var selectClearingHouse = "";
                if (Admin_ClearingHouse.params["FromAdmin"] == "0") {
                    selectClearingHouse = '&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_ClearingHouse.FillClearingHouse(' + item.ClearingHouseId + ', \'' + item.ShortName + '\');" title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", "Admin_ClearingHouse.FillClearingHouse(' + item.ClearingHouseId + ', \'' + item.ShortName + '\');");
                }
                else
                    $row.attr("onclick", "Admin_ClearingHouse.ClearingHouseEdit('" + item.ClearingHouseId + "',event);");
                $row.append('<td style="display:none;">' + item.ClearingHouseId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_ClearingHouse.ClearingHouseDelete(' + item.ClearingHouseId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ClearingHouse.ClearingHouseEdit(' + item.ClearingHouseId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_ClearingHouse.ClearingHouseActiveInactive(' + item.ClearingHouseId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectClearingHouse + '</td><td>' + item.ShortName + '</td><td>' + item.TypeName + '</td><td>' + item.ClaimSubmitAllowed + '</td><td>' + item.ClaimStatusAllowed + '</td><td>' + item.EligibilityAllowed + '</td>');

                $("#" + Admin_ClearingHouse.params["PanelID"] + " #pnlClearingHouse_Result #dgvClearingHouse tbody").last().append($row);
            });
        }
        else {
            $("#" + Admin_ClearingHouse.params["PanelID"] + " #dgvClearingHouse").DataTable({
                "language": {
                    "emptyTable": "No Clearing House Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Admin_ClearingHouse.params["PanelID"] + " #dgvClearingHouse"))
            ;
        else
            $("#" + Admin_ClearingHouse.params["PanelID"] + " #pnlClearingHouse_Result #dgvClearingHouse").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillClearingHouse: function (ClearingHouseId, ShortName) {

        var RefCtrl = "txtClearingHouse";
        var RefHiddenIdCtrl = "hfClearingHouse";

        if (Admin_ClearingHouse.params.RefCtrl != null)
            RefCtrl = Admin_ClearingHouse.params.RefCtrl;

        if (Admin_ClearingHouse.params.RefHiddenIdCtrl != null)
            RefHiddenIdCtrl = Admin_ClearingHouse.params.RefHiddenIdCtrl;

        $('#' + Admin_ClearingHouse.params.PanelID + ' #' + RefCtrl).val(ShortName);
        $('#' + Admin_ClearingHouse.params.PanelID + ' #' + RefHiddenIdCtrl).val(ClearingHouseId);

        UnloadActionPan(Admin_ClearingHouse.params["ParentCtrl"], "Admin_ClearingHouse");
    },

    SearchClearingHouse: function (ClearingHouseData, ClearingHouseId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ClearingHouseData=" + ClearingHouseData + "&ClearingHouseID=" + ClearingHouseId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_CLEARING_HOUSE", "SEARCH_CLEARING_HOUSE");
    },

    DeleteClearingHouse: function (ClearingHouseId) {
        var data = "ClearingHouseID=" + ClearingHouseId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_CLEARING_HOUSE_DETAIL", "DELETE_CLEARING_HOUSE");
    },

    UnLoadTab: function (Tab) {

        if (Admin_ClearingHouse.params["FromAdmin"] == "0") {
            if (Admin_ClearingHouse.params != null && Admin_ClearingHouse.params.ParentCtrl != null) {
                UnloadActionPan(Admin_ClearingHouse.params.ParentCtrl, 'Admin_ClearingHouse');
            }
            else
                UnloadActionPan(null, 'Admin_Facility');
        }
        else {
            RemoveAdminTab();
        }
    },
}