
Admin_PlanFeeLink = {
    bIsFirstLoad: true,
    params:null,
    Load: function (params) {
        Admin_PlanFeeLink.params = params;
        if (Admin_PlanFeeLink.bIsFirstLoad) {
            Admin_PlanFeeLink.bIsFirstLoad = false;
            
            var self = null;
            if (Admin_PlanFeeLink.params.PanelID == "pnlAdminPlanFeeLink")
                self = $('#pnlAdminPlanFeeLink');
            else
                self = $('#' + Admin_PlanFeeLink.params.PanelID + ' #pnlAdminPlanFeeLink');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {                
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlPlanFeeLink_Search #divPlanFeeLink_Entity").css("display", "none");
                //    $("#pnlPlanFeeLink_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_PlanFeeLink.PlanFeeLinkSearch();
            });
        }
    },

    PlanFeeLinkAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Plan Fee Link", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PlanFeeLinkId"] = null;
                params["mode"] = "Add";

                LoadActionPan('planfeelinkDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PlanFeeLinkEdit: function (PlanFeeLinkId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPlanFeeLink_row' + PlanFeeLinkId));
        AppPrivileges.GetFormPrivileges("Plan Fee Link", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = PlanFeeLinkId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["PlanFeeLinkId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('planfeelinkDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    PlanFeeLinkDelete: function (PlanFeeLinkId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPlanFeeLink_row' + PlanFeeLinkId));
        AppPrivileges.GetFormPrivileges("Plan Fee Link", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = PlanFeeLinkId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_PlanFeeLink.DeletePlanFeeLink(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvPlanFeeLink').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_PlanFeeLink.PlanFeeLinkSearch();
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

    PlanFeeLinkActiveInactive: function (PlanFeeLinkId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Plan Fee Link", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = PlanFeeLinkId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        planfeelinkDetail.UpdatePlanFeeLinkActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_PlanFeeLink.PlanFeeLinkSearch('0');
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

    PlanFeeLinkSearch: function (PlanFeeLinkId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Plan Fee Link", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminPlanFeeLink #pnlPlanFeeLink_Result").css("display") == "none") {
                    $("#pnlAdminPlanFeeLink #pnlPlanFeeLink_Result").show();
                }

                var self = $("#pnlPlanFeeLink_Search");
                var myJSON = self.getMyJSON();

                Admin_PlanFeeLink.SearchPlanFeeLink(myJSON, PlanFeeLinkId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_PlanFeeLink.PlanFeeLinkGridLoad(response);
                        var TableControl = "pnlAdminPlanFeeLink #dgvPlanFeeLink";
                        var PagingPanelControlID = "pnlAdminPlanFeeLink #divPlanFeeLinkPaging";
                        var ClassControlName = "Admin_PlanFeeLink";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.PlanFeeLinkCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_PlanFeeLink.PlanFeeLinkSearch(PrimaryID, PageNumber, ResultPerPage);
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

    PlanFeeLinkGridLoad: function (response) {
        $("#dgvPlanFeeLink").dataTable().fnDestroy();
        $("#pnlPlanFeeLink_Result #dgvPlanFeeLink tbody").find("tr").remove();
        if (response.PlanFeeLinkCount > 0) {
            var PlanFeeLinkLoadJSONData = JSON.parse(response.PlanFeeLinkLoad_JSON);
            $.each(PlanFeeLinkLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_PlanFeeLink.PlanFeeLinkEdit('" + item.PlanFeeLinkId + "',event);");
                $row.attr("id", "gvPlanFeeLink_row" + item.PlanFeeLinkId);
                $row.attr("PlanFeeLinkId", item.PlanFeeLinkId);

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
                $row.append('<td style="display:none;">' + item.PlanFeeLinkId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_PlanFeeLink.PlanFeeLinkDelete(\'' + item.PlanFeeLinkId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_PlanFeeLink.PlanFeeLinkEdit(\'' + item.PlanFeeLinkId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_PlanFeeLink.PlanFeeLinkActiveInactive(\'' + item.PlanFeeLinkId + '\', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.Name + '</td><td>' + item.Description + '</td>');

                $("#pnlPlanFeeLink_Result #dgvPlanFeeLink tbody").last().append($row);
            });
        }
        else {
            $('#dgvPlanFeeLink').DataTable({
                "language": {
                    "emptyTable": "No Plan Fee Link Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvPlanFeeLink'))
            ;
        else
            $("#pnlPlanFeeLink_Result #dgvPlanFeeLink").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchPlanFeeLink: function (PlanFeeLinkData, PlanFeeLinkId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "PlanFeeLinkData=" + PlanFeeLinkData + "&PlanFeeLinkID=" + PlanFeeLinkId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_FEE_LINK", "SEARCH_PLAN_FEE_LINK");
    },

    DeletePlanFeeLink: function (PlanFeeLinkId) {
        var data = "PlanFeeLinkID=" + PlanFeeLinkId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_FEE_LINK_DETAIL", "DELETE_PLAN_FEE_LINK");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}