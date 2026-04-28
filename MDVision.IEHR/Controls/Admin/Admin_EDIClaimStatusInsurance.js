
Admin_EDIClaimStatusInsurance = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_EDIClaimStatusInsurance.bIsFirstLoad) {
            Admin_EDIClaimStatusInsurance.bIsFirstLoad = false;
            var self = $('#pnlAdminEDIClaimStatusInsurance');
            self.loadDropDowns(true);
            AppPrivileges.GetFormPrivileges("EDI Claim Status Insurance", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceSearch();
                }
            });
        }
    },

    EDIClaimStatusInsuranceAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Claim Status Insurance", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["EDIClaimStatusInsuranceId"] = null;
                params["mode"] = "Add";
                LoadActionPan('EDIClaimStatusInsuranceDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDIClaimStatusInsuranceEdit: function (EDIClaimStatusInsuranceId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDIClaimStatusInsurance_row' + EDIClaimStatusInsuranceId));
        AppPrivileges.GetFormPrivileges("EDI Claim Status Insurance", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = EDIClaimStatusInsuranceId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["EDIClaimStatusInsuranceId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('EDIClaimStatusInsuranceDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDIClaimStatusInsuranceDelete: function (EDIClaimStatusInsuranceId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDIClaimStatusInsurance_row' + EDIClaimStatusInsuranceId));
        AppPrivileges.GetFormPrivileges("EDI Claim Status Insurance", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = EDIClaimStatusInsuranceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_EDIClaimStatusInsurance.DeleteEDIClaimStatusInsurance(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvEDIClaimStatusInsurance').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceSearch();
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

    EDIClaimStatusInsuranceActiveInactive: function (EDIClaimStatusInsuranceId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("EDI Claim Status Insurance", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = EDIClaimStatusInsuranceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        EDIClaimStatusInsuranceDetail.UpdateEDIClaimStatusInsuranceActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceSearch('0');
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

    EDIClaimStatusInsuranceSearch: function (EDIClaimStatusInsuranceId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Claim Status Insurance", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminEDIClaimStatusInsurance #pnlEDIClaimStatusInsurance_Result").css("display") == "none") {
                    $("#pnlAdminEDIClaimStatusInsurance #pnlEDIClaimStatusInsurance_Result").show();
                }

                var self = $("#pnlEDIClaimStatusInsurance_Search");
                var myJSON = self.getMyJSON();

                Admin_EDIClaimStatusInsurance.SearchEDIClaimStatusInsurance(myJSON, EDIClaimStatusInsuranceId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceGridLoad(response);
                        var TableControl = "pnlAdminEDIClaimStatusInsurance #dgvEDIClaimStatusInsurance";
                        var PagingPanelControlID = "pnlAdminEDIClaimStatusInsurance #divEDIClaimStatusInsurancePaging";
                        var ClassControlName = "Admin_EDIClaimStatusInsurance";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.EDIClaimStatusInsuranceCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceSearch(PrimaryID, PageNumber, ResultPerPage);
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

    EDIClaimStatusInsuranceGridLoad: function (response) {
        $("#dgvEDIClaimStatusInsurance").dataTable().fnDestroy();
        $("#pnlEDIClaimStatusInsurance_Result #dgvEDIClaimStatusInsurance tbody").find("tr").remove();
        if (response.EDIClaimStatusInsuranceCount > 0) {
            var EDIClaimStatusInsuranceLoadJSONData = JSON.parse(response.EDIClaimStatusInsuranceLoad_JSON);
            $.each(EDIClaimStatusInsuranceLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceEdit('" + item.EDIClaimStatusID + "',event);");
                $row.attr("id", "gvEDIClaimStatusInsurance_row" + item.EDIClaimStatusID);
                $row.attr("EDIClaimStatusInsuranceId", item.EDIClaimStatusID);
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
                $row.append('<td style="display:none;">' + item.EDIClaimStatusID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceDelete(' + item.EDIClaimStatusID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceEdit(' + item.EDIClaimStatusID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceActiveInactive(' + item.EDIClaimStatusID + ', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.EDIStatusInsurance + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.PayorId + '</td><td>' + item.PhoneNo + '</td>');

                $("#pnlEDIClaimStatusInsurance_Result #dgvEDIClaimStatusInsurance tbody").last().append($row);
            });
        }
        else {
            $('#dgvEDIClaimStatusInsurance').DataTable({
                "language": {
                    "emptyTable": "No EDI Claim Status Insurance Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvEDIClaimStatusInsurance'))
            ;
        else
            $("#pnlEDIClaimStatusInsurance_Result #dgvEDIClaimStatusInsurance").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchEDIClaimStatusInsurance: function (EDIClaimStatusInsuranceData, EDIClaimStatusInsuranceId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "EDIClaimStatusInsuranceData=" + EDIClaimStatusInsuranceData + "&EDIClaimStatusInsuranceID=" + EDIClaimStatusInsuranceId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_CLAIM_STATUS_INSURANCE", "SEARCH_EDI_CLAIM_STATUS_INSURANCE");
    },

    DeleteEDIClaimStatusInsurance: function (EDIClaimStatusInsuranceId) {
        var data = "EDIClaimStatusInsuranceID=" + EDIClaimStatusInsuranceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_CLAIM_STATUS_INSURANCE_DETAIL", "DELETE_EDI_CLAIM_STATUS_INSURANCE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}