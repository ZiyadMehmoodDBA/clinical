
Admin_POSFeeSchedule = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_POSFeeSchedule.bIsFirstLoad) {
            Admin_POSFeeSchedule.bIsFirstLoad = false;
            
            var self = $('#pnlAdminPOSFeeSchedule');
            self.loadDropDowns(true);
            AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_POSFeeSchedule.POSFeeScheduleSearch(); 
                }
            });
        }
    },

    BindAutoComplete: function (element) {

        var entityId = $('#pnlAdminPOSFeeSchedule #ddlFeeGroup option:selected').attr('refvalue');
        //var hiddenCrtl = $("#pnlAdminPOSFeeSchedule #hfCPTCode");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', null, entityId, true, -1, "CPT", true, "Admin_POSFeeSchedule", null, true);

        //if (globalAppdata['IMO_ID'] == "") {
        //    CacheManager.BindAutoCompleteText('#pnlAdminPOSFeeSchedule #txtCPTCode', 'GetCPTCode', true, '#pnlAdminPOSFeeSchedule #hfCPTCode', entityId);
        //}
        //else {
        //    utility.BindAutoCompleteText('#pnlAdminPOSFeeSchedule #txtCPTCode', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#pnlAdminPOSFeeSchedule #hfCPTCode', entityId);
        //}

    },

    POSFeeScheduleAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["POSFeeScheduleId"] = "-1";
                params["mode"] = "Add";
                LoadActionPan('POSFeeScheduleDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    POSFeeScheduleEdit: function (POSFeeScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPOSFeeSchedule_row' + POSFeeScheduleId));
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = POSFeeScheduleId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["POSFeeScheduleId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('POSFeeScheduleDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    POSFeeScheduleDelete: function (POSFeeScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvPOSFeeSchedule_row' + POSFeeScheduleId));
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = POSFeeScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_POSFeeSchedule.DeletePOSFeeSchedule(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvPOSFeeSchedule').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_POSFeeSchedule.POSFeeScheduleSearch();
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

    POSFeeScheduleActiveInactive: function (POSFeeScheduleId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = POSFeeScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        POSFeeScheduleDetail.UpdatePOSFeeScheduleActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_POSFeeSchedule.POSFeeScheduleSearch('0');
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

    POSFeeScheduleSearch: function (POSFeeScheduleId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Fee Group Plan CPT POS", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminPOSFeeSchedule #pnlPOSFeeSchedule_Result").css("display") == "none") {
                    $("#pnlAdminPOSFeeSchedule #pnlPOSFeeSchedule_Result").show();
                }

                var self = $("#pnlPOSFeeSchedule_Search");
                var myJSON = self.getMyJSON();

                Admin_POSFeeSchedule.SearchPOSFeeSchedule(myJSON, POSFeeScheduleId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_POSFeeSchedule.POSFeeScheduleGridLoad(response);
                        var TableControl = "pnlAdminPOSFeeSchedule #dgvPOSFeeSchedule";
                        var PagingPanelControlID = "pnlAdminPOSFeeSchedule #DivPOSFeeSchedulePaging";
                        var ClassControlName = "Admin_POSFeeSchedule";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.POSFeeScheduleCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_POSFeeSchedule.POSFeeScheduleSearch(PrimaryID, PageNumber, ResultPerPage);
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

    POSFeeScheduleGridLoad: function (response) {
        $("#dgvPOSFeeSchedule").dataTable().fnDestroy();
        $("#pnlPOSFeeSchedule_Result #dgvPOSFeeSchedule tbody").find("tr").remove();
        if (response.POSFeeScheduleCount > 0) {
            var POSFeeScheduleLoadJSONData = JSON.parse(response.POSFeeScheduleLoad_JSON);
            $.each(POSFeeScheduleLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_POSFeeSchedule.POSFeeScheduleEdit('" + item.FeeGroupPOSId + "',event);");
                $row.attr("id", "gvPOSFeeSchedule_row" + item.FeeGroupPOSId);
                $row.attr("POSFeeScheduleId", item.FeeGroupPOSId);

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

                $row.append('<td style="display:none;">' + item.FeeGroupPOSId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_POSFeeSchedule.POSFeeScheduleDelete(\'' + item.FeeGroupPOSId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_POSFeeSchedule.POSFeeScheduleEdit(\'' + item.FeeGroupPOSId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_POSFeeSchedule.POSFeeScheduleActiveInactive(\'' + item.FeeGroupPOSId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.FeeGroupName + '</td><td>' + item.PlanFeeLinkName + '</td><td>' + item.CPTCode + '</td><td>' + item.POSCode + '</td><td>' + utility.convertToFigure(item.Fee, true) + '</td><td>' + utility.convertToFigure(item.ExpectedFee, true) + '</td>');

                $("#pnlPOSFeeSchedule_Result #dgvPOSFeeSchedule tbody").last().append($row);
            });
        }
        else {
            $('#dgvPOSFeeSchedule').DataTable({
                "language": {
                    "emptyTable": "No Fee Group Plan CPT POS Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvPOSFeeSchedule'))
            ;
        else
            $("#pnlPOSFeeSchedule_Result #dgvPOSFeeSchedule").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },
    LoadEntityBasedData: function () {
        var entityID = $('#pnlAdminPOSFeeSchedule #ddlFeeGroup option:selected').attr("refvalue");
        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#pnlAdminPOSFeeSchedule #ddlPlanFeeLink', 'GetPlanFeeLink', false, entityID);
            // CacheManager.BindDropDownsByEntityID(ControlPanel + ' #ddlCPTCode', 'GetCPTCode', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#pnlAdminPOSFeeSchedule #ddlPlanFeeLink', 'GetPlanFeeLink', true, null);
            //  CacheManager.BindDropDownsByEntityID(ControlPanel + ' #ddlCPTCode', 'GetCPTCode', true, null);
        }
        $('#pnlAdminPOSFeeSchedule #txtCPTCode').val("");
    },
    SearchPOSFeeSchedule: function (POSFeeScheduleData, POSFeeScheduleId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "POSFeeScheduleData=" + POSFeeScheduleData + "&POSFeeScheduleID=" + POSFeeScheduleId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE", "SEARCH_POS_FEE_SCHEDULE");
    },

    DeletePOSFeeSchedule: function (POSFeeScheduleId) {
        var data = "POSFeeScheduleID=" + POSFeeScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_POS_FEE_SCHEDULE_DETAIL", "DELETE_POS_FEE_SCHEDULE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}