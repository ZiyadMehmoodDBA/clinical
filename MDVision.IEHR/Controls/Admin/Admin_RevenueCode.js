
Admin_RevenueCode = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_RevenueCode.bIsFirstLoad) {
            Admin_RevenueCode.bIsFirstLoad = false;
            var self = $('#pnlAdminRevenueCode');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlRevenueCode_Search #divRevenueCode_Entity").css("display", "none");
                //    $("#pnlRevenueCode_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_RevenueCode.RevenueCodeSearch();
            });
        }
    },

    RevenueCodeAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Revenue Code", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["RevenueCodeId"] = "-1";
                params["mode"] = "Add";

                LoadActionPan('revenuecodeDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    RevenueCodeEdit: function (RevenueCodeId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvRevenueCode_row' + RevenueCodeId));
        AppPrivileges.GetFormPrivileges("Revenue Code", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = RevenueCodeId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["RevenueCodeId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('revenuecodeDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    RevenueCodeDelete: function (RevenueCodeId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvRevenueCode_row' + RevenueCodeId));
        AppPrivileges.GetFormPrivileges("Revenue Code", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = RevenueCodeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_RevenueCode.DeleteRevenueCode(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvRevenueCode').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetRevenueCode', true);
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

    RevenueCodeActiveInactive: function (RevenueCodeId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Revenue Code", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = RevenueCodeId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        revenuecodeDetail.UpdateRevenueCodeActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_RevenueCode.RevenueCodeSearch('0');
                                CacheManager.BindCodes('GetRevenueCode', true);
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

    RevenueCodeSearch: function (RevenueCodeId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Revenue Code", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminRevenueCode #pnlRevenueCode_Result").css("display") == "none") {
                    $("#pnlAdminRevenueCode #pnlRevenueCode_Result").show();
                }

                var self = $("#pnlRevenueCode_Search");
                var myJSON = self.getMyJSON();

                Admin_RevenueCode.SearchRevenueCode(myJSON, RevenueCodeId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_RevenueCode.RevenueCodeGridLoad(response);
                        var TableControl = "pnlAdminRevenueCode #dgvRevenueCode";
                        var PagingPanelControlID = "pnlAdminRevenueCode #divRevenueCodePaging";
                        var ClassControlName = "Admin_RevenueCode";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.RevenueCodeCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_RevenueCode.RevenueCodeSearch(PrimaryID, PageNumber, ResultPerPage);
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

    RevenueCodeGridLoad: function (response) {
        $("#dgvRevenueCode").dataTable().fnDestroy();
        $("#pnlRevenueCode_Result #dgvRevenueCode tbody").find("tr").remove();
        if (response.RevenueCodeCount > 0) {
            var RevenueCodeLoadJSONData = JSON.parse(response.RevenueCodeLoad_JSON);
            $.each(RevenueCodeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_RevenueCode.RevenueCodeEdit('" + item.RevenueCodeId + "',event);");
                $row.attr("id", "gvRevenueCode_row" + item.RevenueCodeId);
                $row.attr("RevenueCodeId", item.RevenueCodeId);

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
                $row.append('<td style="display:none;">' + item.RevenueCodeId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_RevenueCode.RevenueCodeDelete(\'' + item.RevenueCodeId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_RevenueCode.RevenueCodeEdit(\'' + item.RevenueCodeId + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_RevenueCode.RevenueCodeActiveInactive(\'' + item.RevenueCodeId + '\', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.RevenueCode + '</td><td>' + item.Description + '</td>');

                $("#pnlRevenueCode_Result #dgvRevenueCode tbody").last().append($row);
            });
        }
        else {
            $('#dgvRevenueCode').DataTable({
                "language": {
                    "emptyTable": "No Revenue Code Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvRevenueCode'))
            ;
        else
            $("#pnlRevenueCode_Result #dgvRevenueCode").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
           // $("#pnlRevenueCode_Result #dgvRevenueCode").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchRevenueCode: function (RevenueCodeData, RevenueCodeId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "RevenueCodeData=" + RevenueCodeData + "&RevenueCodeID=" + RevenueCodeId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE", "SEARCH_REVENUE_CODE");
    },

    DeleteRevenueCode: function (RevenueCodeId) {
        var data = "RevenueCodeID=" + RevenueCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_REVENUE_CODE_DETAIL", "DELETE_REVENUE_CODE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}