Admin_ResourceSchedule = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_ResourceSchedule.bIsFirstLoad) {
            Admin_ResourceSchedule.bIsFirstLoad = false;
            var self = $('#pnlAdminResourceSchedule');
            self.loadDropDowns(true);

            AppPrivileges.GetFormPrivileges("Resource Schedule", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_ResourceSchedule.ResourceScheduleSearch();
                }
            });
        }
    },

    ResourceScheduleAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Resource Schedule", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ResScheduleId"] = null;
                params["mode"] = "Add";

                LoadActionPan('resourcescheduleDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ResourceScheduleEdit: function (ResScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvResourceSchedule_row' + ResScheduleId));
        AppPrivileges.GetFormPrivileges("Resource Schedule", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ResScheduleId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ResScheduleId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('resourcescheduleDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ResourceScheduleDelete: function (ResScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvResourceSchedule_row' + ResScheduleId));
        AppPrivileges.GetFormPrivileges("Resource Schedule", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ResScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ResourceSchedule.DeleteResourceSchedule(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#pnlAdminResourceSchedule #dgvResourceSchedule').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_ResourceSchedule.ResourceScheduleSearch('0');
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

    ResourceScheduleActiveInactive: function (ResScheduleId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Resource Schedule", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ResScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        resourcescheduleDetail.UpdateResourceScheduleActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_ResourceSchedule.ResourceScheduleSearch('0');
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

    ResourceScheduleSearch: function (ResScheduleId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Resource Schedule", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminResourceSchedule #pnlResourceSchedule_Result").css("display") == "none") {
                    $("#pnlAdminResourceSchedule #pnlResourceSchedule_Result").show();
                }

                var self = $("#pnlResourceSchedule_Search");
                var myJSON = self.getMyJSON();

                Admin_ResourceSchedule.SearchResourceSchedule(myJSON, ResScheduleId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_ResourceSchedule.ResourceScheduleGridLoad(response);
                        var TableControl = "pnlAdminResourceSchedule #dgvResourceSchedule";
                        var PagingPanelControlID = "pnlAdminResourceSchedule #divResSchedulePaging";
                        var ClassControlName = "Admin_ResourceSchedule";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ResourceScheduleCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_ResourceSchedule.ResourceScheduleSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ResourceScheduleGridLoad: function (response) {
        $("#dgvResourceSchedule").dataTable().fnDestroy();
        $("#pnlResourceSchedule_Result #dgvResourceSchedule tbody").find("tr").remove();
        if (response.ResourceScheduleCount > 0) {
            var ResourceScheduleLoadJSONData = JSON.parse(response.ResourceScheduleLoad_JSON);
            $.each(ResourceScheduleLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_ResourceSchedule.ResourceScheduleEdit('" + item.ResScheduleId + "',event);");
                $row.attr("id", "gvResourceSchedule_row" + item.ResScheduleId);
                $row.attr("ResScheduleId", item.ResScheduleId);

                if (item.isActive == "True") {
                    isactive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    isactive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }

                $row.append('<td style="display:none;">' + item.ResScheduleId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_ResourceSchedule.ResourceScheduleDelete(\'' + item.ResScheduleId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ResourceSchedule.ResourceScheduleEdit(\'' + item.ResScheduleId + '\',event);" title="View Record"><i class="fa fa-eye black"></i></a>&nbsp;</td><td>' + item.ResourceName + '</td><td>' + item.FacilityName + '</td><td>' + item.FromDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ToDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.FromTime + '</td><td>' + item.ToTime + '</td><td>'  + item.CreatedOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.CreatedBy + '</td>' );

                $("#pnlResourceSchedule_Result #dgvResourceSchedule tbody").last().append($row);
            });
        }
        else {
            $('#pnlResourceSchedule_Result #dgvResourceSchedule').DataTable({
                "language": {
                    "emptyTable": "No Resource Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlResourceSchedule_Result #dgvResourceSchedule'))
            ;
        else {
            $("#pnlResourceSchedule_Result #dgvResourceSchedule").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
           // $("#pnlResourceSchedule_Result #dgvResourceSchedule").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        }
    },

    SearchResourceSchedule: function (ResourceScheduleData, ResScheduleId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ResourceScheduleData=" + ResourceScheduleData + "&ResScheduleID=" + ResScheduleId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCESCHEDULE", "SEARCH_RESOURCESCHEDULE");
    },

    DeleteResourceSchedule: function (ResScheduleId) {
        var data = "ResScheduleID=" + ResScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCESCHEDULE_DETAIL", "DELETE_RESOURCESCHEDULE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}