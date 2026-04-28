Admin_AppointmentStatus = {
    bIsFirstLoad: true,
    Load: function (params) {
        if (Admin_AppointmentStatus.bIsFirstLoad) {
            Admin_AppointmentStatus.bIsFirstLoad = false;
            AppPrivileges.GetFormPrivileges("Appointment Status", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_AppointmentStatus.AppointmentStatusSearch();
                }
            });
        }
    },

    AppointmentStatusAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment Status", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["AppointmentId"] = null;
                params["mode"] = "Add";
                LoadActionPan('appointmentStatusDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    AppointmentStatusEdit: function (AppointmentId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvAppointmentStatus_row' + AppointmentId));
        AppPrivileges.GetFormPrivileges("Appointment Status", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = AppointmentId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["AppointmentId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('appointmentStatusDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    AppointmentStatusDelete: function (AppointmentId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvAppointmentStatus_row' + AppointmentId));
        AppPrivileges.GetFormPrivileges("Appointment Status", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = AppointmentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_AppointmentStatus.DeleteAppointmentStatus(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvAppointmentStatus').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_AppointmentStatus.AppointmentStatusSearch('0');
                                utility.DisplayMessages(response.Message, 1);

                                CacheManager.BindCodes('GetAppointmentStatus', true);
                                MDVisionService.lookups("GetAppointmentStatus", true);
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

    AppointmentStatusActiveInactive: function (AppointmentId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = AppointmentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        appointmentStatusDetail.UpdateScheduleAppointmentStatusActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_AppointmentStatus.AppointmentStatusSearch('0');

                                CacheManager.BindCodes('GetAppointmentStatus', true);
                                MDVisionService.lookups("GetAppointmentStatus", true);
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

    AppointmentStatusSearch: function (AppointmentId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Appointment Status", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminAppointmentStatus #pnlAppointmentStatus_Result").css("display") == "none") {
                    $("#pnlAdminAppointmentStatus #pnlAppointmentStatus_Result").show();
                }

                var self = $("#pnlAppointmentStatus_Search");
                var myJSON = self.getMyJSON();

                Admin_AppointmentStatus.SearchAppointmentStatus(myJSON, AppointmentId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_AppointmentStatus.AppointmentStatusGridLoad(response);
                        var TableControl = "pnlAdminAppointmentStatus #dgvAppointmentStatus";
                        var PagingPanelControlID = "pnlAdminAppointmentStatus #divAppointmentStatusPaging";
                        var ClassControlName = "Admin_AppointmentStatus";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.AppointmentStatusCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_AppointmentStatus.AppointmentStatusSearch(PrimaryID, PageNumber, ResultPerPage);
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

    AppointmentStatusGridLoad: function (response) {
        $("#dgvAppointmentStatus").dataTable().fnDestroy();
        $("#pnlAppointmentStatus_Result #dgvAppointmentStatus tbody").find("tr").remove();
        if (response.AppointmentStatusCount > 0) {
            var AppointmentStatusLoadJSONData = JSON.parse(response.AppointmentStatusLoad_JSON);
            $.each(AppointmentStatusLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_AppointmentStatus.AppointmentStatusEdit('" + item.AppointmentId + "',event);");
                $row.attr("id", "gvAppointmentStatus_row" + item.AppointmentId);
                $row.attr("AppointmentId", item.AppointmentId);

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
                var actionBar = "";

                var ClassDisabled = item.IsSystem == "True" ? "disabled" : "";
                if (!(item.IsSystem == "True")) {
                    actionBar = '<td><a class="btn  btn-xs"  href="#" onclick="Admin_AppointmentStatus.AppointmentStatusDelete(' + item.AppointmentId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs"  href="#" onclick="Admin_AppointmentStatus.AppointmentStatusEdit(' + item.AppointmentId + ',event);"  title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_AppointmentStatus.AppointmentStatusActiveInactive(' + item.AppointmentId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td>';
                } else {
                    actionBar = '<td title="System generated, cannot modify!" data-toggle="tooltip"></td>';
                }

                $row.append('<td style="display:none;">' + item.AppointmentId + '</td>' + actionBar + '<td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.ShortName + '">' + item.ShortName + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.Description + '">' + item.Description + '</td><td><div style="width:12px;height:12px; margin:2px 0 0 2px;background:' + item.Color + '"></td><td>' + item.IsActive + '</td>');


                $("#pnlAppointmentStatus_Result #dgvAppointmentStatus tbody").last().append($row);
            });
        }
        else {
            $('#dgvAppointmentStatus').DataTable({
                "language": {
                    "emptyTable": "No Appointment Status Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#dgvAppointmentStatus'))
            ;
        else
            $("#pnlAppointmentStatus_Result #dgvAppointmentStatus").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //  $("#pnlAppointmentStatus_Result #dgvAppointmentStatus").DataTable({ "bLengthChange": false, "order": [[ 0, "desc" ]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchAppointmentStatus: function (AppointmentStatusData, AppointmentId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "AppointmentStatusData=" + AppointmentStatusData + "&AppointmentID=" + AppointmentId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_APPOINTMENT_STATUS", "SEARCH_APPOINTMENT_STATUS");
    },

    DeleteAppointmentStatus: function (AppointmentId) {
        var data = "AppointmentID=" + AppointmentId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_APPOINTMENT_STATUS_DETAIL", "DELETE_APPOINTMENT_STATUS");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}