Admin_OccupationStatus = {

    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Admin_OccupationStatus.params = params;

        if (Admin_OccupationStatus.params["FromAdmin"] == "0" && Admin_OccupationStatus.params["PanelID"] == 'pnlAdminOccupationStatus')
            Admin_OccupationStatus.params["FromAdmin"] = "1";

        if (Admin_OccupationStatus.bIsFirstLoad) {
            Admin_OccupationStatus.bIsFirstLoad = false;

        }
        Admin_OccupationStatus.OccupationStatusSearch();
    },

    OccupationStatusAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Occupation Status", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["StatusId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_OccupationStatus.params["FromAdmin"];
                LoadActionPan('Admin_OccupationStatusDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OccupationStatusSearch: function (StatusID, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Occupation Status", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminOccupationStatus #pnlOccupationStatus_Result").css("display") == "none") {
                    $("#pnlAdminOccupationStatus #pnlOccupationStatus_Result").show();
                }

                var self = $("#pnlOccupationStatus_Search");
                var myJSON = self.getMyJSON();

                Admin_OccupationStatus.SearchOccupationStatus(myJSON, StatusID, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_OccupationStatus.OccupationStatusGridLoad(response);
                        var TableControl = "pnlAdminOccupationStatus #dgvOccupationStatus";
                        var PagingPanelControlID = "pnlAdminOccupationStatus #divOccupationStatusPaging";
                        var ClassControlName = "Admin_OccupationStatus";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.OccupationStatusCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_OccupationStatus.OccupationStatusSearch(PrimaryID, PageNumber, ResultPerPage);
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

    OccupationStatusEdit: function (StatusId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvOccupationStatus_row' + StatusId));
        AppPrivileges.GetFormPrivileges("Occupation Status", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = StatusId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["StatusId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('Admin_OccupationStatusDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    SearchOccupationStatus: function (OccupationStatusData, StatusID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "OccupationStatusData=" + OccupationStatusData + "&StatusID=" + StatusID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_OCCUPATION_STATUS_DETAIL", "SEARCH_OCCUPATIONSTATUS_CODE");
    },

    OccupationStatusGridLoad: function (response) {
        $("#dgvOccupationStatus").dataTable().fnDestroy();
        $("#pnlOccupationStatus_Result #dgvOccupationStatus tbody").find("tr").remove();
        if (response.OccupationStatusCount > 0) {
            var OccupationStatusLoad_JSON = JSON.parse(response.OccupationStatusLoad_JSON);
            $.each(OccupationStatusLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_OccupationStatus.OccupationStatusEdit('" + item.StatusId + "',event);");
                $row.attr("id", "gvOccupationStatus_row" + item.StatusId);
                $row.attr("StatusId", item.StatusId);
                var IsOccupationType = "";
                if (item.IsOccupation == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                    IsOccupationType = "Occupation";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                    IsOccupationType = "Industry";
                }
                $row.append('<td style="display:none;">' + item.StatusId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_OccupationStatus.OccupationStatusDelete(\'' + item.StatusId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_OccupationStatus.OccupationStatusEdit(\'' + item.StatusId + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>' + IsOccupationType + '</td><td>' + item.ConceptCode + '</td><td>' + item.Description + '</td>');

                $("#pnlOccupationStatus_Result #dgvOccupationStatus tbody").last().append($row);
            });
        }
        else {
            $('#dgvOccupationStatus').DataTable({
                "language": {
                    "emptyTable": "No Occupation Status Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvOccupationStatus'))
            ;
        else
            $("#pnlOccupationStatus_Result #dgvOccupationStatus").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); 
    },

    OccupationStatusDelete: function (StatusId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvOccupationStatus_row' + StatusId));
        AppPrivileges.GetFormPrivileges("Occupation Status", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = StatusId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_OccupationStatus.DeleteOccupationStatus(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvOccupationStatus').DataTable();
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

    DeleteOccupationStatus: function (StatusId) {
        var data = "StatusId=" + StatusId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_OCCUPATION_STATUS_DETAIL", "DELETE_OCCUPATION_STATUS");
    },

    UnLoadTab: function () {
        if (Admin_OccupationStatus.params["FromAdmin"] == "0") {
            if (Admin_OccupationStatus.params != null && Admin_OccupationStatus.params.ParentCtrl != null) {
                UnloadActionPan(Admin_OccupationStatus.params.ParentCtrl, 'Admin_OccupationStatus');
            }
            else
                UnloadActionPan(null, 'Admin_OccupationStatus');
        }
        else {
            RemoveAdminTab();
        }
    },
}