Admin_ScheduleReason = {
    bIsFirstLoad: true,
    params: null,
    Load: function (params) {
        Admin_ScheduleReason.params = params;


        if (Admin_ScheduleReason.params["FromAdmin"] == "0" && Admin_ScheduleReason.params["PanelID"] == 'pnlAdminScheduleReason')
            Admin_ScheduleReason.params["FromAdmin"] = "1";

        if (Admin_ScheduleReason.bIsFirstLoad) {
            Admin_ScheduleReason.bIsFirstLoad = false;
            var self = $('#pnlAdminScheduleReason');
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                $("#" + Admin_ScheduleReason.params["PanelID"] + " #ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlScheduleReason_Search #divReason_Entity").css("display", "none");
                //    $("#pnlScheduleReason_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    $("#" + Admin_ScheduleReason.params["PanelID"] + " #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_ScheduleReason.ScheduleReasonSearch();
            });
        }
    },

    ScheduleReasonAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Schedule Reason", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ScheduleReasonId"] = null;
                params["ParentCtrl"] = 'Admin_ScheduleReason';
                params["mode"] = "Add";

                LoadActionPan('scheduleReasonDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ScheduleReasonEdit: function (ScheduleReasonId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Admin_ScheduleReason.params["PanelID"] + ' #gvScheduleReason_row' + ScheduleReasonId));
        AppPrivileges.GetFormPrivileges("Schedule Reason", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = ScheduleReasonId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["ScheduleReasonId"] = selectedValue;
                    params["ParentCtrl"] = 'Admin_ScheduleReason';
                    params["mode"] = "Edit";
                    LoadActionPan('scheduleReasonDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ScheduleReasonDelete: function (ScheduleReasonId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($("#" + Admin_ScheduleReason.params["PanelID"] + ' #gvScheduleReason_row' + ScheduleReasonId));
        AppPrivileges.GetFormPrivileges("Schedule Reason", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = ScheduleReasonId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_ScheduleReason.DeleteScheduleReason(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $("#" + Admin_ScheduleReason.params["PanelID"] + ' #dgvScheduleReason').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_ScheduleReason.ScheduleReasonSearch('0');
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetBlockReasons', true);
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

    ScheduleReasonActiveInactive: function (ScheduleReasonId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Schedule Reason", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = ScheduleReasonId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        scheduleReasonDetail.UpdateScheduleReasonActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_ScheduleReason.ScheduleReasonSearch('0');
                                CacheManager.BindCodes('GetBlockReasons', true);
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

    ScheduleReasonSearch: function (ScheduleReasonId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Schedule Reason", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_ScheduleReason.params["PanelID"] + " #pnlScheduleReason_Result").css("display") == "none") {
                    $("#" + Admin_ScheduleReason.params["PanelID"] + " #pnlScheduleReason_Result").show();
                }

                var self = $("#" + Admin_ScheduleReason.params["PanelID"] + " #pnlScheduleReason_Search");
                var myJSON = self.getMyJSON();

                Admin_ScheduleReason.SearchScheduleReason(myJSON, ScheduleReasonId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_ScheduleReason.ScheduleReasonGridLoad(response);
                        var TableControl = "pnlAdminScheduleReason #dgvScheduleReason";
                        var PagingPanelControlID = "pnlAdminScheduleReason #divScheduleReasonPaging";
                        var ClassControlName = "Admin_ScheduleReason";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.ScheduleReasonCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_ScheduleReason.ScheduleReasonSearch(PrimaryID, PageNumber, ResultPerPage);
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

    ScheduleReasonGridLoad: function (response) {
        $("#" + Admin_ScheduleReason.params["PanelID"] + " #dgvScheduleReason").dataTable().fnDestroy();
        $("#" + Admin_ScheduleReason.params["PanelID"] + " #pnlScheduleReason_Result #dgvScheduleReason tbody").find("tr").remove();
        if (response.ScheduleReasonCount > 0) {
            var ScheduleReasonLoadJSONData = JSON.parse(response.ScheduleReasonLoad_JSON);
            $.each(ScheduleReasonLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                //$row.attr("onclick", "Admin_ScheduleReason.ScheduleReasonEdit('" + item.ScheduleReasonId + "',event);");
                $row.attr("id", "gvScheduleReason_row" + item.ScheduleReasonId);
                $row.attr("ScheduleReasonId", item.ScheduleReasonId);

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

                // ----------------- New Code

                var selectReason = "";
                var selectMethod;
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                if (Admin_ScheduleReason.params["FromAdmin"] == "0") {

                    if (Admin_ScheduleReason.params.ParentCtrl == 'blckreasonDetail') {
                        selectMethod = "blckreasonDetail.FillScheduleReason('" + item.ScheduleReasonId + "','" + item.ShortName + "','" + item.Duration + "',event);"
                    } else if (Admin_ScheduleReason.params.ParentCtrl == 'appointmentDetail') {
                        selectMethod = "appointmentDetail.FillScheduleReason('" + item.ScheduleReasonId + "','" + item.ShortName + "','" + item.Duration + "',event);"
                    } else if (Admin_ScheduleReason.params.ParentCtrl == 'schEditSlot') {
                        selectMethod = "schEditSlot.FillScheduleReason('" + item.ScheduleReasonId + "','" + item.ShortName + "','" + item.Duration + "',event);"
                    } else if (Admin_ScheduleReason.params.ParentCtrl == 'schwaitlistdetail') {
                        selectMethod = "schwaitlistdetail.FillScheduleReason('" + item.ScheduleReasonId + "','" + item.ShortName + "','" + item.Duration + "',event);"
                    } else if (Admin_ScheduleReason.params.ParentCtrl == 'providerscheduleDetail') {
                        if (Admin_ScheduleReason.params.IsBlockReason == true) {
                            selectMethod = "providerscheduleDetail.FillBlockReason('" + item.ScheduleReasonId + "','" + item.ShortName + "','" + item.Duration + "',event);"
                        } else {
                            selectMethod = "providerscheduleDetail.FillScheduleReason('" + item.ScheduleReasonId + "','" + item.ShortName + "','" + item.Duration + "',event);"
                        }
                    } else if (Admin_ScheduleReason.params.ParentCtrl == 'resourcescheduleDetail') {
                        if (Admin_ScheduleReason.params.IsBlockReason == true) {
                            selectMethod = "resourcescheduleDetail.FillBlockReason('" + item.ScheduleReasonId + "','" + item.ShortName + "','" + item.Duration + "',event);"
                        } else {
                            selectMethod = "resourcescheduleDetail.FillScheduleReason('" + item.ScheduleReasonId + "','" + item.ShortName + "','" + item.Duration + "',event);"
                        }
                    }


                    selectReason = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                    if (ClassDisabled != "disabled")
                        $row.attr("onclick", selectMethod);
                }
                else {
                    $row.attr("onclick", "Admin_ScheduleReason.ScheduleReasonEdit('" + item.ScheduleReasonId + "',event);");
                }

                // ------------- End New Code

                //bug #PMS-2015
                $row.append('<td style="display:none;">' + item.ScheduleReasonId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_ScheduleReason.ScheduleReasonDelete(\'' + item.ScheduleReasonId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ScheduleReason.ScheduleReasonEdit(\'' + item.ScheduleReasonId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_ScheduleReason.ScheduleReasonActiveInactive(\'' + item.ScheduleReasonId + '\', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectReason + '</td><td>' + item.ShortName + '</td><td class="ellip100" title="' + item.Description + '" data-toggle="tooltip" data-placement="left">' + item.Description + '</td> <td>' + item.Duration + '</td><td>' + item.EntityName + '</td>');
                //bug #PMS-2015
                $("#pnlScheduleReason_Result #dgvScheduleReason tbody").last().append($row);
            });
        }
        else {
            $("#" + Admin_ScheduleReason.params["PanelID"] + " #pnlScheduleReason_Result #dgvScheduleReason").DataTable({
                "language": {
                    "emptyTable": "No Schedule Reason Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Admin_ScheduleReason.params["PanelID"] + " #pnlScheduleReason_Result #dgvScheduleReason"))
            ;
        else {
            $("#" + Admin_ScheduleReason.params["PanelID"] + " #pnlScheduleReason_Result #dgvScheduleReason").DataTable({ "bInfo": false, "bPaginate": false, "bSort": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            //$("#pnlScheduleReason_Result #dgvScheduleReason").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        }
        //bug #PMS-2015
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        //bug #PMS-2015
    },

    SearchScheduleReason: function (ScheduleReasonData, ScheduleReasonId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "ScheduleReasonData=" + ScheduleReasonData + "&ScheduleReasonID=" + ScheduleReasonId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SCHEDULE_REASON", "SEARCH_SCHEDULE_REASON");
    },

    DeleteScheduleReason: function (ScheduleReasonId) {
        var data = "ScheduleReasonID=" + ScheduleReasonId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_SCHEDULE_REASON_DETAIL", "DELETE_SCHEDULE_REASON");
    },

    UnLoadTab: function (Tab) {
        //RemoveAdminTab(Tab);

        if (Admin_ScheduleReason.params["FromAdmin"] == "0") {


            if (Admin_ScheduleReason.params != null && Admin_ScheduleReason.params.ParentCtrl != null && Admin_ScheduleReason.params.PanelID != 'pnlAdminScheduleReason') {
                UnloadActionPan(Admin_ScheduleReason.params.ParentCtrl, 'Admin_ScheduleReason', null, Admin_ScheduleReason.params.PanelID);
            }

            else if (Admin_ScheduleReason.params != null && Admin_ScheduleReason.params.ParentCtrl != null) {
                UnloadActionPan(Admin_ScheduleReason.params.ParentCtrl, 'Admin_ScheduleReason');
            }

            else
                UnloadActionPan(null, 'Admin_ScheduleReason');
        }
        else {
            RemoveAdminTab();
        }

    },

}
