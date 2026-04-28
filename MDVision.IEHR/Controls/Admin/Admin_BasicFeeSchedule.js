
Admin_BasicFeeSchedule = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_BasicFeeSchedule.bIsFirstLoad) {
            Admin_BasicFeeSchedule.bIsFirstLoad = false;
            var self = $('#pnlAdminBasicFeeSchedule');
            self.loadDropDowns(true).done(function () {

                AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Admin_BasicFeeSchedule.BasicFeeScheduleSearch();
                    }
                });
            });
        }
    },

    BindAutoComplete: function (element) {

        var entityId = $('#pnlAdminBasicFeeSchedule #ddlBasicFeeGroup option:selected').attr('refvalue');
        //var hiddenCrtl = $("#pnlAdminBasicFeeSchedule #hfCPTCode");
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', null, entityId, false, -1, "CPT", true, "Admin_BasicFeeSchedule", null, true);

        //if (globalAppdata['IMO_ID'] == "") {
        //    CacheManager.BindAutoCompleteText('#pnlAdminBasicFeeSchedule #txtCPTCode', 'GetCPTCode', true, '#pnlAdminBasicFeeSchedule #hfCPTCode', entityId);
        //}
        //else {
        //    utility.BindAutoCompleteText('#pnlAdminBasicFeeSchedule #txtCPTCode', 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', '#pnlAdminBasicFeeSchedule #hfCPTCode', entityId);
        //}

    },

    BasicFeeScheduleAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["BasicFeeScheduleId"] = "-1";
                params["mode"] = "Add";
                LoadActionPan('basicFeeScheduleDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BasicFeeScheduleEdit: function (BasicFeeScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvBasicFeeSchedule_row' + BasicFeeScheduleId));
        AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = BasicFeeScheduleId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["BasicFeeScheduleId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('basicFeeScheduleDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BasicFeeScheduleDelete: function (BasicFeeScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvBasicFeeSchedule_row' + BasicFeeScheduleId));
        AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = BasicFeeScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_BasicFeeSchedule.DeleteBasicFeeSchedule(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvBasicFeeSchedule').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_BasicFeeSchedule.BasicFeeScheduleSearch();
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

    BasicFeeScheduleActiveInactive: function (BasicFeeScheduleId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = BasicFeeScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        basicFeeScheduleDetail.UpdateBasicFeeScheduleActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_BasicFeeSchedule.BasicFeeScheduleSearch('0');
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

    BasicFeeScheduleSearch: function (BasicFeeScheduleId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Basic Fee Schedule", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminBasicFeeSchedule #pnlBasicFeeSchedule_Result").css("display") == "none") {
                    $("#pnlAdminBasicFeeSchedule #pnlBasicFeeSchedule_Result").show();
                }

                var self = $("#pnlBasicFeeSchedule_Search");
                var myJSON = self.getMyJSON();

                Admin_BasicFeeSchedule.SearchBasicFeeSchedule(myJSON, BasicFeeScheduleId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_BasicFeeSchedule.BasicFeeScheduleGridLoad(response);
                        var TableControl = "pnlAdminBasicFeeSchedule #dgvBasicFeeSchedule";
                        var PagingPanelControlID = "pnlAdminBasicFeeSchedule #DivBasicFeeSchedulePaging";
                        var ClassControlName = "Admin_BasicFeeSchedule";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.BasicFeeScheduleCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_BasicFeeSchedule.BasicFeeScheduleSearch(PrimaryID, PageNumber, ResultPerPage);
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

    BasicFeeScheduleGridLoad: function (response) {
        $("#dgvBasicFeeSchedule").dataTable().fnDestroy();
        $("#pnlBasicFeeSchedule_Result #dgvBasicFeeSchedule tbody").find("tr").remove();
        if (response.BasicFeeScheduleCount > 0) {
            var BasicFeeScheduleLoadJSONData = JSON.parse(response.BasicFeeScheduleLoad_JSON);
            $.each(BasicFeeScheduleLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_BasicFeeSchedule.BasicFeeScheduleEdit('" + item.BasicFeeSchId + "',event);");
                $row.attr("id", "gvBasicFeeSchedule_row" + item.BasicFeeSchId);
                $row.attr("BasicFeeScheduleId", item.BasicFeeSchId);

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

                $row.append('<td style="display:none;">' + item.BasicFeeSchId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_BasicFeeSchedule.BasicFeeScheduleDelete(' + item.BasicFeeSchId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_BasicFeeSchedule.BasicFeeScheduleEdit(' + item.BasicFeeSchId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_BasicFeeSchedule.BasicFeeScheduleActiveInactive(' + item.BasicFeeSchId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.BasicFeeGroupName + '</td><td>' + item.CPTCode + '</td><td>' + utility.convertToFigure(item.Fee, true) + '</td><td>' + utility.convertToFigure(item.ExpectedFee,true) + '</td>');

                $("#pnlBasicFeeSchedule_Result #dgvBasicFeeSchedule tbody").last().append($row);
            });
        }
        else {
            $('#dgvBasicFeeSchedule').DataTable({
                "language": {
                    "emptyTable": "No Basic Fee Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#dgvBasicFeeSchedule'))
            ;
        else
            $("#pnlBasicFeeSchedule_Result #dgvBasicFeeSchedule").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },
   
    LoadEntityBasedData: function () {
        $('#pnlAdminBasicFeeSchedule #txtCPTCode').val("");
    },
    SearchBasicFeeSchedule: function (BasicFeeScheduleData, BasicFeeScheduleId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "BasicFeeScheduleData=" + BasicFeeScheduleData + "&BasicFeeScheduleID=" + BasicFeeScheduleId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE", "SEARCH_BASIC_FEE_SCHEDULE");
    },

    DeleteBasicFeeSchedule: function (BasicFeeScheduleId) {
        var data = "BasicFeeScheduleID=" + BasicFeeScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_SCHEDULE_DETAIL", "DELETE_BASIC_FEE_SCHEDULE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}