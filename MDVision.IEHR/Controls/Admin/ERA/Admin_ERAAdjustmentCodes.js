
Admin_ERAAdjustmentCodes = {
    bIsFirstLoad: true,
    Load: function (params) {
        Admin_ERAAdjustmentCodes.params = params;
        if (Admin_ERAAdjustmentCodes.bIsFirstLoad) {
            Admin_ERAAdjustmentCodes.bIsFirstLoad = false;
          //  var self = $('#pnlAdminERAAdjustmentCodes');
            if (Admin_ERAAdjustmentCodes.params["PanelID"] != 'pnlAdminERAAdjustmentCodes') {
                Admin_ERAAdjustmentCodes.params["PanelID"] = Admin_ERAAdjustmentCodes.params["PanelID"] + " #pnlAdminERAAdjustmentCodes";
            }
           var  self = $('#' + Admin_ERAAdjustmentCodes.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                AppPrivileges.GetFormPrivileges("ERA Adjustment Codes", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Admin_ERAAdjustmentCodes.ERAAdjustmentCodesSearch();
                    }
                });
            });

           
        }
    },

    ERAAdjustmentCodesAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ERA Adjustment Codes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ERAAdjCodeId"] = null;
                params["mode"] = "Add";

                LoadActionPan('Admin_ERAAdjustmentCodesDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ERAAdjustmentCodesEdit: function (ERAAdjCodeId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvERAAdjustmentCodes_row' + ERAAdjCodeId));
        AppPrivileges.GetFormPrivileges("ERA Adjustment Codes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ERAAdjCodeId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ERAAdjCodeId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('Admin_ERAAdjustmentCodesDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ERAAdjustmentCodesDelete: function (ERAAdjCodeId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvERAAdjustmentCodes_row' + ERAAdjCodeId));
        AppPrivileges.GetFormPrivileges("ERA Adjustment Codes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ERAAdjCodeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ERAAdjustmentCodes.DeleteERAAdjustmentCodes(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $("#"+ Admin_ERAAdjustmentCodes.params["PanelID"]+' #dgvERAAdjustmentCodes').DataTable();
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

    ERAAdjustmentCodesActiveInactive: function (ERAAdjCodeId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("ERA Adjustment Codes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ERAAdjCodeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ERAAdjustmentCodesDetail.UpdateERAAdjustmentCodesActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_ERAAdjustmentCodes.ERAAdjustmentCodesSearch('0');
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

    ERAAdjustmentCodesSearch: function (ERAAdjCodeId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ERA Adjustment Codes", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_ERAAdjustmentCodes.params["PanelID"] + " #pnlERAAdjustmentCodes_Result").css("display") == "none") {
                    $("#" + Admin_ERAAdjustmentCodes.params["PanelID"] + " #pnlERAAdjustmentCodes_Result").show();
                }

                var self =$("#"+ Admin_ERAAdjustmentCodes.params["PanelID"]+" #pnlERAAdjustmentCodes_Search");
                var myJSON = self.getMyJSON();
                Admin_ERAAdjustmentCodes.SearchERAAdjustmentCodes(myJSON, ERAAdjCodeId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_ERAAdjustmentCodes.ERAAdjustmentCodesGridLoad(response);
                        var TableControl = Admin_ERAAdjustmentCodes.params["PanelID"] + " #dgvERAAdjustmentCodes";
                        var PagingPanelControlID = Admin_ERAAdjustmentCodes.params["PanelID"] + " #divERAAdjustmentCodesPaging";
                        var ClassControlName = "Admin_ERAAdjustmentCodes";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ERAAdjustmentCodesCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_ERAAdjustmentCodes.ERAAdjustmentCodesSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ERAAdjustmentCodesGridLoad: function (response) {
        $("#" + Admin_ERAAdjustmentCodes.params["PanelID"] + " #dgvERAAdjustmentCodes").dataTable().fnDestroy();
        $("#" + Admin_ERAAdjustmentCodes.params["PanelID"] + " #pnlERAAdjustmentCodes_Result #dgvERAAdjustmentCodes tbody").find("tr").remove();
        if (response.ERAAdjustmentCodesCount > 0) {
            var ERAAdjustmentCodesLoadJSONData = JSON.parse(response.ERAAdjustmentCodesLoad_JSON);
            $.each(ERAAdjustmentCodesLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_ERAAdjustmentCodes.ERAAdjustmentCodesEdit('" + item.ERAAdjCodeId + "',event);");
                $row.attr("id", "gvERAAdjustmentCodes_row" + item.ERAAdjCodeId);
                $row.attr("ERAAdjCodeId", item.ERAAdjCodeId);

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
                $row.append('<td style="display:none;">' + item.ERAAdjCodeId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_ERAAdjustmentCodes.ERAAdjustmentCodesDelete(' + item.ERAAdjCodeId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ERAAdjustmentCodes.ERAAdjustmentCodesEdit(' + item.ERAAdjCodeId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_ERAAdjustmentCodes.ERAAdjustmentCodesActiveInactive(' + item.ERAAdjCodeId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.GroupCode + '</td><td>' + item.ReasonCode + '</td><td>' + item.PracticeName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.LedgerAccountName + '</td><td>' + item.ERAActionName + '</td><td>' + item.IsActive + '</td>');

                $("#" + Admin_ERAAdjustmentCodes.params["PanelID"] + " #pnlERAAdjustmentCodes_Result #dgvERAAdjustmentCodes tbody").last().append($row);
            });
        }
        else {
            $("#"+ Admin_ERAAdjustmentCodes.params["PanelID"]+" #dgvERAAdjustmentCodes").DataTable({
                "language": {
                    "emptyTable": "No ERA Adjustment Codes Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvERAAdjustmentCodes'))
            ;
        else
            $("#" + Admin_ERAAdjustmentCodes.params["PanelID"] + " #pnlERAAdjustmentCodes_Result #dgvERAAdjustmentCodes").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchERAAdjustmentCodes: function (ERAAdjustmentCodesData, ERAAdjCodeId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ERAAdjustmentCodesData=" + ERAAdjustmentCodesData + "&ERAAdjCodeId=" + ERAAdjCodeId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ERA_ADJUSTMENT_CODES", "SEARCH_ERA_ADJUSTMENT_CODES");
    },

    DeleteERAAdjustmentCodes: function (ERAAdjCodeId) {
        var data = "ERAAdjCodeId=" + ERAAdjCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ERAADJUSTMENTCODES_DETAIL", "DELETE_ERAADJUSTMENT_CODES");
    },

    

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}