
Admin_ProcedureFeeSchedule = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_ProcedureFeeSchedule.bIsFirstLoad) {
            Admin_ProcedureFeeSchedule.bIsFirstLoad = false;
            var self = $('#pnlAdminProcedureFeeSchedule');
            self.loadDropDowns(true).done(function () {
                AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Admin_ProcedureFeeSchedule.ProcedureFeeScheduleSearch();
                    }
                });
            });

            
        }
    },

    BindAutoComplete: function (element) {

        var entityId = $('#pnlAdminProcedureFeeSchedule #ddlFeeGroup option:selected').attr('refvalue');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', null, entityId, true, -1, "CPT", true, "Admin_ProcedureFeeSchedule", null, true);


        //var entityId = $('#pnlAdminProcedureFeeSchedule #ddlFeeGroup option:selected').attr('refvalue');
        //if (globalAppdata['IMO_ID'] == "") {
        //    CacheManager.BindAutoCompleteText('#pnlAdminProcedureFeeSchedule #txtCPTCode', 'GetCPTCode', true, null, entityId);
        //}
        //else {
        //    utility.BindAutoCompleteText('#pnlAdminProcedureFeeSchedule #txtCPTCode', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', null, entityId);
        //}

    },

    ProcedureFeeScheduleAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ProcedureFeeScheduleId"] = "-1";
                params["mode"] = "Add";
                LoadActionPan('procedureFeeScheduleDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ProcedureFeeScheduleEdit: function (ProcedureFeeScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvProcedureFeeSchedule_row' + ProcedureFeeScheduleId));
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ProcedureFeeScheduleId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ProcedureFeeScheduleId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('procedureFeeScheduleDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ProcedureFeeScheduleDelete: function (ProcedureFeeScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvProcedureFeeSchedule_row' + ProcedureFeeScheduleId));
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ProcedureFeeScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ProcedureFeeSchedule.DeleteProcedureFeeSchedule(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvProcedureFeeSchedule').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_ProcedureFeeSchedule.ProcedureFeeScheduleSearch();
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

    ProcedureFeeScheduleActiveInactive: function (ProcedureFeeScheduleId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ProcedureFeeScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        procedureFeeScheduleDetail.UpdateProcedureFeeScheduleActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_ProcedureFeeSchedule.ProcedureFeeScheduleSearch('0');
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

    ProcedureFeeScheduleSearch: function (ProcedureFeeScheduleId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminProcedureFeeSchedule #pnlProcedureFeeSchedule_Result").css("display") == "none") {
                    $("#pnlAdminProcedureFeeSchedule #pnlProcedureFeeSchedule_Result").show();
                }

                var self = $("#pnlProcedureFeeSchedule_Search");
                var myJSON = self.getMyJSON();

                Admin_ProcedureFeeSchedule.SearchProcedureFeeSchedule(myJSON, ProcedureFeeScheduleId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_ProcedureFeeSchedule.ProcedureFeeScheduleGridLoad(response);
                        var TableControl = "pnlAdminProcedureFeeSchedule #dgvProcedureFeeSchedule";
                        var PagingPanelControlID = "pnlAdminProcedureFeeSchedule #DivProcedureFeeSchedulePaging";
                        var ClassControlName = "Admin_ProcedureFeeSchedule";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ProcedureFeeScheduleCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_ProcedureFeeSchedule.ProcedureFeeScheduleSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ProcedureFeeScheduleGridLoad: function (response) {
        $("#dgvProcedureFeeSchedule").dataTable().fnDestroy();
        $("#pnlProcedureFeeSchedule_Result #dgvProcedureFeeSchedule tbody").find("tr").remove();
        if (response.ProcedureFeeScheduleCount > 0) {
            var ProcedureFeeScheduleLoadJSONData = JSON.parse(response.ProcedureFeeScheduleLoad_JSON);
            $.each(ProcedureFeeScheduleLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_ProcedureFeeSchedule.ProcedureFeeScheduleEdit('" + item.FeeGroupProcId + "',event);");
                $row.attr("id", "gvProcedureFeeSchedule_row" + item.FeeGroupProcId);
                $row.attr("ProcedureFeeScheduleId", item.FeeGroupProcId);

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

                $row.append('<td style="display:none;">' + item.FeeGroupProcId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_ProcedureFeeSchedule.ProcedureFeeScheduleDelete(\'' + item.FeeGroupProcId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ProcedureFeeSchedule.ProcedureFeeScheduleEdit(\'' + item.FeeGroupProcId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_ProcedureFeeSchedule.ProcedureFeeScheduleActiveInactive(\'' + item.FeeGroupProcId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.FeeGroupName + '</td><td>' + item.PlanFeeLinkName + '</td><td>' + item.CPTCode + '</td><td>' + utility.convertToFigure(item.Fee, true) + '</td><td>' + utility.convertToFigure(item.ExpectedFee, true) + '</td>');

                $("#pnlProcedureFeeSchedule_Result #dgvProcedureFeeSchedule tbody").last().append($row);
            });
        }
        else {
            $('#dgvProcedureFeeSchedule').DataTable({
                "language": {
                    "emptyTable": "No Fee Group Plan CPT Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvProcedureFeeSchedule'))
            ;
        else
            $("#pnlProcedureFeeSchedule_Result #dgvProcedureFeeSchedule").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },
    LoadEntityBasedData: function () {
        var entityID = $('#pnlAdminProcedureFeeSchedule #ddlFeeGroup option:selected').attr("refvalue");
        $('#pnlAdminProcedureFeeSchedule #txtCPTCode').val("");
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#pnlAdminProcedureFeeSchedule #ddlPlanFeeLink', 'GetPlanFeeLink', false, entityID);
            // CacheManager.BindDropDownsByEntityID(ControlPanel + ' #ddlCPTCode', 'GetCPTCode', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#pnlAdminProcedureFeeSchedule #ddlPlanFeeLink', 'GetPlanFeeLink', true, null);
            //  CacheManager.BindDropDownsByEntityID(ControlPanel + ' #ddlCPTCode', 'GetCPTCode', true, null);
        }
       
    },
    SearchProcedureFeeSchedule: function (ProcedureFeeScheduleData, ProcedureFeeScheduleId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ProcedureFeeScheduleData=" + ProcedureFeeScheduleData + "&ProcedureFeeScheduleID=" + ProcedureFeeScheduleId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE", "SEARCH_PROCEDURE_FEE_SCHEDULE");
    },

    DeleteProcedureFeeSchedule: function (ProcedureFeeScheduleId) {
        var data = "ProcedureFeeScheduleID=" + ProcedureFeeScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_FEE_SCHEDULE_DETAIL", "DELETE_PROCEDURE_FEE_SCHEDULE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}