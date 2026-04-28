
Admin_ProcedureCategory = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_ProcedureCategory.bIsFirstLoad) {
            Admin_ProcedureCategory.bIsFirstLoad = false;

            AppPrivileges.GetFormPrivileges("Procedure Category", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_ProcedureCategory.ProcedureCategorySearch();
                }
            });
        }
    },

    ProcedureCategoryAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Procedure Category", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ProcedureCategoryId"] = null;
                params["mode"] = "Add";

                LoadActionPan('procedurecategoryDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ProcedureCategoryEdit: function (ProcedureCategoryId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvProcedureCategory_row' + ProcedureCategoryId));
        AppPrivileges.GetFormPrivileges("Procedure Category", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ProcedureCategoryId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ProcedureCategoryId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('procedurecategoryDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ProcedureCategoryDelete: function (ProcedureCategoryId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvProcedureCategory_row' + ProcedureCategoryId));
        AppPrivileges.GetFormPrivileges("Procedure Category", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ProcedureCategoryId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ProcedureCategory.DeleteProcedureCategory(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvProcedureCategory').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetProcedureCategory', true);
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

    ProcedureCategoryActiveInactive: function (ProcedureCategoryId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Procedure Category", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ProcedureCategoryId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        procedurecategoryDetail.UpdateProcedureCategoryActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_ProcedureCategory.ProcedureCategorySearch('0');
                                CacheManager.BindCodes('GetProcedureCategory', true);
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

    ProcedureCategorySearch: function (ProcedureCategoryId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Procedure Category", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminProcedureCategory #pnlProcedureCategory_Result").css("display") == "none") {
                    $("#pnlAdminProcedureCategory #pnlProcedureCategory_Result").show();
                }

                var self = $("#pnlProcedureCategory_Search");
                var myJSON = self.getMyJSON();

                Admin_ProcedureCategory.SearchProcedureCategory(myJSON, ProcedureCategoryId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_ProcedureCategory.ProcedureCategoryGridLoad(response);
                        var TableControl = "pnlAdminProcedureCategory #divProcCategoryePaging";
                        var PagingPanelControlID = "pnlAdminProcedureCategory #divProcCategoryePaging";
                        var ClassControlName = "Admin_ProcedureCategory";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ProcedureCategoryCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_ProcedureCategory.ProcedureCategorySearch(PrimaryID, PageNumber, ResultPerPage);
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

    ProcedureCategoryGridLoad: function (response) {
        $("#dgvProcedureCategory").dataTable().fnDestroy();
        $("#pnlProcedureCategory_Result #dgvProcedureCategory tbody").find("tr").remove();
        if (response.ProcedureCategoryCount > 0) {
            var ProcedureCategoryLoadJSONData = JSON.parse(response.ProcedureCategoryLoad_JSON);
            $.each(ProcedureCategoryLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_ProcedureCategory.ProcedureCategoryEdit('" + item.ProcCategoryId + "',event);");
                $row.attr("id", "gvProcedureCategory_row" + item.ProcCategoryId);
                $row.attr("ProcCategoryId", item.ProcCategoryId);

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
                $row.append('<td style="display:none;">' + item.ProcCategoryId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_ProcedureCategory.ProcedureCategoryDelete(\'' + item.ProcCategoryId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ProcedureCategory.ProcedureCategoryEdit(\'' + item.ProcCategoryId + '\',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_ProcedureCategory.ProcedureCategoryActiveInactive(\'' + item.ProcCategoryId + '\', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.Name + '</td><td>' + item.Description + '</td>');

                $("#pnlProcedureCategory_Result #dgvProcedureCategory tbody").last().append($row);
            });
        }
        else {
            $('#dgvProcedureCategory').DataTable({
                "language": {
                    "emptyTable": "No Procedure Category Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvProcedureCategory'))
            ;
        else
            $("#pnlProcedureCategory_Result #dgvProcedureCategory").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchProcedureCategory: function (ProcedureCategoryData, ProcedureCategoryId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ProcedureCategoryData=" + ProcedureCategoryData + "&ProcedureCategoryID=" + ProcedureCategoryId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_CATEGORY", "SEARCH_PROCEDURE_CATEGORY");
    },

    DeleteProcedureCategory: function (ProcedureCategoryId) {
        var data = "ProcedureCategoryID=" + ProcedureCategoryId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_CATEGORY_DETAIL", "DELETE_PROCEDURE_CATEGORY");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}