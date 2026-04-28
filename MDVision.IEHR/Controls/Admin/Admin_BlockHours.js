Admin_BlockHours = {
    bIsFirstLoad: true,
    Load: function (params) {
        Admin_BlockHours.params = params;
        if (params["ParentCtrl"] == "mstrTabSchedule") {
            $("#modalBlockHourdialog").addClass("modal-dialog  modal-dialog-full");
            $("#modalcontent").addClass("modal-content");
            $("#SearchBlockHoursBtn").show();
            $("#btnad").css('display', 'none')
            $("#btnad2").css('display', 'block')
            Admin_BlockHours.parentActionPanDiv = GetTab(Admin_BlockHours.params.ParentCtrl).ActionPanContainer;
           // if ($("#pnlBlockHours_Result #dgvBlockHours tbody").length == 2)
            //{ $("#pnlBlockHours_Result #dgvBlockHours").eq(1).remove();  }
        }
        else {
            $("#modalBlockHourdialog").removeClass("modal-dialog  modal-dialog-full");
            $("#modalcontent").removeClass("modal-dialog");
            $("#modalcontent").removeClass("modal-content");
            // AST-332 clode button did not hide.
           // $("#SearchBlockHoursBtn").hide();
            $("#btnad").css('display', 'block')
            $("#btnad2").css('display', 'none')
     
            Admin_BlockHours.parentActionPanDiv = GetCurrentSelectedTab().Container;
        }
        
       
        if (Admin_BlockHours.bIsFirstLoad) {
            Admin_BlockHours.bIsFirstLoad = false;
            var self = $('#' + Admin_BlockHours.parentActionPanDiv + ' #pnlAdminBlockHours');
            self.loadDropDowns(true).done(function () {
                Admin_BlockHours.LoadAllControls();


                AppPrivileges.GetFormPrivileges("Block Hours", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Admin_BlockHours.BlockHoursSearch();
                    }
                });
            });
        }

    },

    BlockHoursAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Block Hours", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                if (Admin_BlockHours.params["ParentCtrl"] == "mstrTabSchedule") {
                    params['FromAdmin'] = "0";
                }
                params["ParentCtrl"] = "Admin_BlockHours";
                params["BlockHoursId"] = null;
                params["mode"] = "Add";

                LoadActionPan('blockHoursDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BlockHoursEdit: function (BlockHoursId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvBlockHours_row' + BlockHoursId));
        AppPrivileges.GetFormPrivileges("Block Hours", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = BlockHoursId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    if (Admin_BlockHours.params["ParentCtrl"] == "mstrTabSchedule") {
                        params['FromAdmin'] = 0;
                    }
                    params["ParentCtrl"] = "Admin_BlockHours";
                    params["BlockHoursId"] = selectedValue;
                    params["mode"] = "Edit";
                    LoadActionPan('blockHoursDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BlockHoursDelete: function (BlockHoursId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvBlockHours_row' + BlockHoursId));
        AppPrivileges.GetFormPrivileges("Block Hours", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirmDetail('Are you sure you want to delete the appointment block \'' + event.srcElement.parentElement.parentElement.parentElement.getAttribute('data') + ' \' ?', function () {
                    var selectedValue = BlockHoursId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_BlockHours.DeleteBlockHours(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvBlockHours').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_BlockHours.BlockHoursSearch('0');
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    'Confirm Delete'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    BlockHoursActiveInactive: function (BlockHoursId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Block Hours", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = BlockHoursId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        blockHoursDetail.UpdateBlockHoursActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_BlockHours.BlockHoursSearch('0');
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

    BlockHoursSearch: function (BlockHoursId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Block Hours", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_BlockHours.parentActionPanDiv + " #pnlAdminBlockHours #pnlBlockHours_Result").css("display") == "none") {
                    $("#" + Admin_BlockHours.parentActionPanDiv + " #pnlAdminBlockHours #pnlBlockHours_Result").show();
                }

                var self = $("#" + Admin_BlockHours.parentActionPanDiv + " #pnlBlockHours_Search");
                var myJSON = self.getMyJSON();

                Admin_BlockHours.SearchBlockHours(myJSON, BlockHoursId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_BlockHours.BlockHoursGridLoad(response);
                        var TableControl = "pnlAdminBlockHours #dgvBlockHours";
                        var PagingPanelControlID = "pnlAdminBlockHours #divBlockHoursPaging";
                        var ClassControlName = "Admin_BlockHours";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.BlockHoursCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_BlockHours.BlockHoursSearch(PrimaryID, PageNumber, ResultPerPage);
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

    BlockHoursGridLoad: function (response) {
        $("#" + Admin_BlockHours.parentActionPanDiv + " #dgvBlockHours").dataTable().fnDestroy();
        $("#" + Admin_BlockHours.parentActionPanDiv + " #pnlBlockHours_Result #dgvBlockHours tbody").find("tr").remove();
        if (response.BlockHoursCount > 0) {
            var BlockHoursLoadJSONData = JSON.parse(response.BlockHoursLoad_JSON);
            $.each(BlockHoursLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_BlockHours.BlockHoursEdit('" + item.BlockHoursId + "',event);");
                $row.attr("id", "gvBlockHours_row" + item.BlockHoursId);
                $row.attr("BlockHoursId", item.BlockHoursId);
                $row.attr("data", item.Description);
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

                $row.append('<td style="display:none;">' + item.BlockHoursId + '</td><td><a class="btn btn-xs" href="#" onclick="Admin_BlockHours.BlockHoursDelete(' + item.BlockHoursId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_BlockHours.BlockHoursEdit(' + item.BlockHoursId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>' + item.FromDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.ToDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td>' + item.FromTime + '</td><td>' + item.ToTime + '</td><td>' + item.ProviderName + '</td><td>' + item.ResourceName + '</td><td>' + item.FacilityName + '</td><td>' + item.Description + '</td>');

                $("#" + Admin_BlockHours.parentActionPanDiv + " #pnlBlockHours_Result #dgvBlockHours tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_BlockHours.parentActionPanDiv + ' #pnlBlockHours_Result #dgvBlockHours').DataTable({
                "language": {
                    "emptyTable": "No Block Hours Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_BlockHours.parentActionPanDiv + '#pnlBlockHours_Result #dgvBlockHours'))
            ;
        else {
            $("#" + Admin_BlockHours.parentActionPanDiv + "#pnlBlockHours_Result #dgvBlockHours").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            //  $("#pnlBlockHours_Result #dgvBlockHours").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        }
    },

    SearchBlockHours: function (BlockHoursData, BlockHoursId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "BlockHoursData=" + BlockHoursData + "&BlockHoursID=" + BlockHoursId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BLOCK_HOURS", "SEARCH_BLOCK_HOURS");
    },

    DeleteBlockHours: function (BlockHoursId) {
        var data = "BlockHoursID=" + BlockHoursId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BLOCK_HOURS_DETAIL", "DELETE_BLOCK_HOURS");
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "pnlBlockHours_Search";
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Admin_BlockHours";
        LoadActionPan('Admin_Facility', params);
    },
    OpenProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "pnlBlockHours_Search";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Admin_BlockHours";
        LoadActionPan('Admin_Provider', params);
    },
    OpenResource: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "pnlBlockHours_Search";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Admin_BlockHours";
        LoadActionPan('Admin_Resources', params);
    },
    LoadAllControls: function () {
        var self = $('#pnlAdminBlockHours');


        CacheManager.BindCodes('GetFacility', false).done(function (result) {
            var Ctrl = $('#pnlAdminBlockHours #pnlBlockHours_Search #txtFacility');
            var hfCtrl = $("#pnlBlockHours_Search #hfFacility");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl);
        });
        CacheManager.BindCodes('GetResources', false).done(function (result) {
            var Ctrl = $('#pnlAdminBlockHours #pnlBlockHours_Search #txtResource');
            var hfCtrl = $("#pnlBlockHours_Search #hfResource");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Resources, null, hfCtrl);
        });
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#pnlAdminBlockHours #pnlBlockHours_Search #txtProvider');
            var hfCtrl = $("#pnlBlockHours_Search #hfProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
        });


    },
    UnloadPan: function () {
        if (Admin_BlockHours.params != null && Admin_BlockHours.params.ParentCtrl != null) {
            if (Admin_BlockHours.params.ParentCtrl == "mstrTabSchedule") {
                var scheduler = $("#scheduler").data("kendoScheduler");
                if (scheduler) {
                    scheduler.dataSource.read();
                }
            }
            else {
                // unload Tab.AST-332
                if (Admin_BlockHours.params["TabID"] == "adminTabBlockHours") {
                    Admin_BlockHours.UnLoadTab();
                }
            }
            UnloadActionPan(Admin_BlockHours.params.ParentCtrl, 'Admin_BlockHours');
            Admin_BlockHours.bIsFirstLoad = true;

        }
        else {
            UnloadActionPan();
            if (Admin_BlockHours.params["TabID"] == "adminTabBlockHours") {
                Admin_BlockHours.UnLoadTab();
            }
        }
           
    },

}