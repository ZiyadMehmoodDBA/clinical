Admin_ProviderSchedule = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_ProviderSchedule.bIsFirstLoad) {
            Admin_ProviderSchedule.bIsFirstLoad = false;
            var self = $('#pnlAdminProviderSchedule');
            self.loadDropDowns(true);
            AppPrivileges.GetFormPrivileges("Provider Schedule", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_ProviderSchedule.ProviderScheduleSearch();
                }
            });
        }
    },

    ProviderScheduleAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Provider Schedule", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ScheduleId"] = null;
                params["mode"] = "Add";

                LoadActionPan('providerscheduleDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ProviderScheduleEdit: function (ScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvProviderSchedule_row' + ScheduleId));
        AppPrivileges.GetFormPrivileges("Provider Schedule", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ScheduleId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ScheduleId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('providerscheduleDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ProviderScheduleDelete: function (ScheduleId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvProviderSchedule_row' + ScheduleId));
        AppPrivileges.GetFormPrivileges("Provider Schedule", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ProviderSchedule.DeleteProviderSchedule(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#pnlAdminProviderSchedule #dgvProviderSchedule').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_ProviderSchedule.ProviderScheduleSearch('0');
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

    ProviderScheduleActiveInactive: function (ScheduleId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Provider Schedule", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ScheduleId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        providerscheduleDetail.UpdateProviderScheduleActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_ProviderSchedule.ProviderScheduleSearch('0');
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

    ProviderScheduleSearch: function (ScheduleId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Provider Schedule", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminProviderSchedule #pnlProviderSchedule_Result").css("display") == "none") {
                    $("#pnlAdminProviderSchedule #pnlProviderSchedule_Result").show();
                }

                var self = $("#pnlProviderSchedule_Search");
                var myJSON = self.getMyJSON();

                Admin_ProviderSchedule.SearchProviderSchedule(myJSON, ScheduleId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_ProviderSchedule.ProviderScheduleGridLoad(response);
                        var TableControl =  "pnlAdminProviderSchedule #dgvProviderSchedule";
                        var PagingPanelControlID = "pnlAdminProviderSchedule #divProviderSchedulePaging";
                        var ClassControlName = "Admin_ProviderSchedule";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ProviderScheduleCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords,
                            function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_ProviderSchedule.ProviderScheduleSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ProviderScheduleGridLoad: function (response) {
        $("#dgvProviderSchedule").dataTable().fnDestroy();
        $("#pnlProviderSchedule_Result #dgvProviderSchedule tbody").find("tr").remove();
        if (response.ProviderScheduleCount > 0) {
            var ProviderScheduleLoadJSONData = JSON.parse(response.ProviderScheduleLoad_JSON);
            $.each(ProviderScheduleLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_ProviderSchedule.ProviderScheduleEdit('" + item.ScheduleId + "',event);");
                $row.attr("id", "gvProviderSchedule_row" + item.ScheduleId);
                $row.attr("ScheduleId", item.ScheduleId);

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
                
                $row.append('<td style="display:none;">' + item.ScheduleId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_ProviderSchedule.ProviderScheduleDelete(\'' + item.ScheduleId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ProviderSchedule.ProviderScheduleEdit(\'' + item.ScheduleId + '\',event);" title="View Record"><i class="fa fa-eye black"></i></a>&nbsp;</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.FromDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ToDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.FromTime + '</td><td>' + item.ToTime + '</td><td>'+ item.CreatedOn.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.CreatedBy + '</td>');

                $("#pnlProviderSchedule_Result #dgvProviderSchedule tbody").last().append($row);
            });
        }
        else {
            $('#pnlProviderSchedule_Result #dgvProviderSchedule').DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlProviderSchedule_Result #dgvProviderSchedule'))
            ;
        else {
            $("#pnlProviderSchedule_Result #dgvProviderSchedule").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
           // $("#pnlProviderSchedule_Result #dgvProviderSchedule").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        }
    },

    SearchProviderSchedule: function (ProviderScheduleData, ScheduleId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ProviderScheduleData=" + ProviderScheduleData + "&ScheduleID=" + ScheduleId+ "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDERSCHEDULE", "SEARCH_PROVIDERSCHEDULE");
    },

    DeleteProviderSchedule: function (ScheduleId) {
        var data = "ScheduleID=" + ScheduleId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDERSCHEDULE_DETAIL", "DELETE_PROVIDERSCHEDULE");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}