
Admin_PlanCategory = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_PlanCategory.bIsFirstLoad) {
            Admin_PlanCategory.bIsFirstLoad = false;
            var self = $('#pnlAdminPlanCategory');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {                
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlPlanCategory_Search #divPlanCategory_Entity").css("display", "none");
                //    $("#pnlPlanCategory_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_PlanCategory.PlanCategorySearch();
            }); 
        }
    },

    PlanCategoryAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Plan Category", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PlanCategoryId"] = null;
                params["mode"] = "Add";

                LoadActionPan('planCategoryDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PlanCategoryEdit: function (PlanCategoryId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPlanCategory_row' + PlanCategoryId));
        AppPrivileges.GetFormPrivileges("Plan Category", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = PlanCategoryId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["PlanCategoryId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('planCategoryDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PlanCategoryDelete: function (PlanCategoryId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPlanCategory_row' + PlanCategoryId));
        AppPrivileges.GetFormPrivileges("Plan Category", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = PlanCategoryId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_PlanCategory.DeletePlanCategory(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#dgvPlanCategory').DataTable();
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

    PlanCategoryActiveInactive: function (PlanCategoryId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Plan Category", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = PlanCategoryId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        planCategoryDetail.UpdatePlanCategoryActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_PlanCategory.PlanCategorySearch('0');
                                //UnloadActionPan();
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

    PlanCategorySearch: function (PlanCategoryId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Plan Category", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminPlanCategory #pnlPlanCategory_Result").css("display") == "none") {
                    $("#pnlAdminPlanCategory #pnlPlanCategory_Result").show();
                }

                var self = $("#pnlPlanCategory_Search");
                var myJSON = self.getMyJSON();

                Admin_PlanCategory.SearchPlanCategory(myJSON, PlanCategoryId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_PlanCategory.PlanCategoryGridLoad(response);
                        var TableControl = "pnlAdminPlanCategory #dgvPlanCategory";
                        var PagingPanelControlID = "pnlAdminPlanCategory #divPlanCategoryPaging";
                        var ClassControlName = "Admin_PlanCategory";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.PlanCategoryCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_PlanCategory.PlanCategorySearch(PrimaryID, PageNumber, ResultPerPage);
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

    PlanCategoryGridLoad: function (response) {
        $("#dgvPlanCategory").dataTable().fnDestroy();
        $("#pnlPlanCategory_Result #dgvPlanCategory tbody").find("tr").remove();
        if (response.PlanCategoryCount > 0) {
            var PlanCategoryLoadJSONData = JSON.parse(response.PlanCategoryLoad_JSON);
            $.each(PlanCategoryLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_PlanCategory.PlanCategoryEdit('" + item.PlanId + "',event);");
                $row.attr("id", "gvPlanCategory_row" + item.PlanId);
                $row.attr("PlanCategoryId", item.PlanId);

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

                $row.append('<td style="display:none;">' + item.PlanId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_PlanCategory.PlanCategoryDelete(\'' + item.PlanId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_PlanCategory.PlanCategoryEdit(\'' + item.PlanId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_PlanCategory.PlanCategoryActiveInactive(\'' + item.PlanId + '\',' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.ShortName + '</td><td>' + item.Description + '</td>');

                $("#pnlPlanCategory_Result #dgvPlanCategory tbody").last().append($row);
            });
        }
        else {
            $('#dgvPlanCategory').DataTable({
                "language": {
                    "emptyTable": "No Plan Category Found"
                },
                "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvPlanCategory'))
            ;
        else
            $("#pnlPlanCategory_Result #dgvPlanCategory").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchPlanCategory: function (PlanCategoryData, PlanCategoryId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "PlanCategoryData=" + PlanCategoryData + "&PlanCategoryID=" + PlanCategoryId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_CATEGORY", "SEARCH_PLAN_CATEGORY");
    },

    DeletePlanCategory: function (PlanCategoryId) {
        //if (PageNumber == null) {
        //    PageNumber = 1;
        //}
        //if (RowsPerPage == null) {
        //    RowsPerPage = 15;
        //}
        var data = "PlanCategoryID=" + PlanCategoryId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_CATEGORY_DETAIL", "DELETE_PLAN_CATEGORY");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}