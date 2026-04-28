
Admin_EDIServiceHandle = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Admin_EDIServiceHandle.params = params;

        if (Admin_EDIServiceHandle.params["FromAdmin"] == "0" && Admin_EDIServiceHandle.params["PanelID"] == 'pnlAdminEDIServiceHandle')
            Admin_EDIServiceHandle.params["FromAdmin"] = "1";

        if (Admin_EDIServiceHandle.bIsFirstLoad) {
            Admin_EDIServiceHandle.bIsFirstLoad = false;

            var self = "";
            if (Admin_EDIServiceHandle.params["PanelID"] != 'pnlAdminEDIServiceHandle')
                self = $('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #pnlAdminEDIServiceHandle');
            else
                self = $('#pnlAdminEDIServiceHandle');

            self.loadDropDowns(true).done(function () {
                Admin_EDIServiceHandle.EDIServiceHandleSearch();
            });
        }
    },

    EDIServiceHandleAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Service", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["EDIServiceHandleID"] = null;
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_EDIServiceHandle.params["FromAdmin"];
                if (Admin_EDIServiceHandle.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_EDIServiceHandle';
                }
                LoadActionPan('EDIServiceHandleDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDIServiceHandleEdit: function (EDIServiceHandleID,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDIServiceHandle_row' + EDIServiceHandleID));
        AppPrivileges.GetFormPrivileges("EDI Service", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = EDIServiceHandleID;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["EDIServiceHandleID"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_EDIServiceHandle.params["FromAdmin"];
                    if (Admin_EDIServiceHandle.params["FromAdmin"] == "0") {
                        params["ParentCtrl"] = 'Admin_EDIServiceHandle';
                    }
                    LoadActionPan('EDIServiceHandleDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDIServiceHandleDelete: function (EDIServiceHandleID,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDIServiceHandle_row' + EDIServiceHandleID));
        utility.SelectGridRow($("#" + Admin_EDIServiceHandle.params["PanelID"] + ' #gvEDIServiceHandle_row' + EDIServiceHandleID));
        AppPrivileges.GetFormPrivileges("EDI Service", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = EDIServiceHandleID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_EDIServiceHandle.DeleteEDIServiceHandle(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #dgvEDIServiceHandle').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                //CacheManager.BindCodes('GetEDIServiceHandle', true);
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

    EDIServiceHandleActiveInactive: function (EDIServiceHandleID, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("EDI Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = EDIServiceHandleID;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        EDIServiceHandleDetail.UpdateEDIServiceHandleActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_EDIServiceHandle.EDIServiceHandleSearch('0');
                                //CacheManager.BindCodes('GetEDIServiceHandle', true);
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

    EDIServiceHandleSearch: function (EDIServiceHandleID, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Service", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_EDIServiceHandle.params["PanelID"] + " #pnlEDIServiceHandle_Result").css("display") == "none") {
                    $("#" + Admin_EDIServiceHandle.params["PanelID"] + " #pnlEDIServiceHandle_Result").show();
                }

                var self = $("#" + Admin_EDIServiceHandle.params["PanelID"] + " #pnlAdminEDIServiceHandle_Search");
                var myJSON = self.getMyJSON();

                Admin_EDIServiceHandle.SearchEDIServiceHandle(myJSON, EDIServiceHandleID, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_EDIServiceHandle.EDIServiceHandleGridLoad(response);
                        var TableControl = Admin_EDIServiceHandle.params["PanelID"] + " #dgvEDIServiceHandle";
                        var PagingPanelControlID = Admin_EDIServiceHandle.params["PanelID"] + " #divEDIServiceHandlePaging";
                        var ClassControlName = "Admin_EDIServiceHandle";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.EDIServiceHandleCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_EDIServiceHandle.EDIServiceHandleSearch(PrimaryID, PageNumber, ResultPerPage);
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

    EDIServiceHandleGridLoad: function (response) {
        $("#" + Admin_EDIServiceHandle.params["PanelID"] + " #dgvEDIServiceHandle").dataTable().fnDestroy();
        $("#" + Admin_EDIServiceHandle.params["PanelID"] + " #pnlEDIServiceHandle_Result #dgvEDIServiceHandle tbody").find("tr").remove();
        if (response.EDIServiceHandleCount > 0) {
            var EDIServiceHandleLoad_JSON = JSON.parse(response.EDIServiceHandleLoad_JSON);
            $.each(EDIServiceHandleLoad_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_EDIServiceHandle.EDIServiceHandleEdit('" + item.EDIServiceHandleID + "',event);utility.SelectGridRow($(this));");
                $row.attr("id", "gvEDIServiceHandle_row" + item.EDIServiceHandleID);
                $row.attr("EDIServiceHandleID", item.EDIServiceHandleID);
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

                var selectEDIServiceHandle = "";
                var ClassDisabled = item.IsActive == "True" ? "" : "disabled";
                //if (Admin_EDIServiceHandle.params["FromAdmin"] == "0") {
                //    var selectMethod = "Admin_EDIServiceHandle.FillEDIServiceHandleName('" + item.EDIServiceHandleID + "','" + item.ShortName + "','" + item.PracticeId + "','" + item.PracticeName + "','" + item.EntityName + "',event);"
                //    selectEDIServiceHandle = '&nbsp;<a class="btn  btn-xs ' + ClassDisabled + '" href="#" onclick=' + selectMethod + ' title="Select Record"><i class="fa fa-check black"></i></a>';
                //    $row.attr("onclick", selectMethod);
                //}
                //else {
                //    $row.attr("onclick", "Admin_EDIServiceHandle.EDIServiceHandleEdit('" + item.EDIServiceHandleID + "',event);");
                //}
                if (Admin_EDIServiceHandle.params["PanelID"] == "pnlReportsSSRSDashboard") {
                    $('#btn-add').hide();
                    //$row.append('<td style="display:none;">' + item.EDIServiceHandleID + '</td><td>' + selectEDIServiceHandle + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    $row.append('<td style="display:none;">' + item.EDIServiceHandleID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDIServiceHandle.EDIServiceHandleDelete(' + item.EDIServiceHandleID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDIServiceHandle.EDIServiceHandleEdit(' + item.EDIServiceHandleID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDIServiceHandle.EDIServiceHandleActiveInactive(' + item.EDIServiceHandleID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + item.EDIServiceHandleID + '</td><td>' + item.EntityName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.Case + '</td><td>' + item.Mode + '</td><td>' + item.Time + '</td><td>' + item.IntervalHours + '</td><td>' + item.IntervalMinutes + '</td><td>' + item.IsActive + '</td>');

                } else {
                    $('#btn-add').show();
                    //$row.append('<td style="display:none;">' + item.EDIServiceHandleID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDIServiceHandle.EDIServiceHandleDelete(' + item.EDIServiceHandleID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDIServiceHandle.EDIServiceHandleEdit(' + item.EDIServiceHandleID + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDIServiceHandle.EDIServiceHandleActiveInactive(' + item.EDIServiceHandleID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectEDIServiceHandle + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PracticeName + '</td><td>' + item.PhoneNo + '</td><td>' + item.City + '</td><td>' + item.POSName + '</td><td>' + item.NPI + '</td>');
                    $row.append('<td style="display:none;">' + item.EDIServiceHandleID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDIServiceHandle.EDIServiceHandleDelete(' + item.EDIServiceHandleID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDIServiceHandle.EDIServiceHandleEdit(' + item.EDIServiceHandleID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDIServiceHandle.EDIServiceHandleActiveInactive(' + item.EDIServiceHandleID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.EntityName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.Case + '</td><td>' + item.Mode + '</td><td>' + item.Time + '</td><td>' + item.IntervalHours + '</td><td>' + item.IntervalMinutes + '</td><td>' + item.IsActive + '</td>');

                }
                $("#" + Admin_EDIServiceHandle.params["PanelID"] + " #pnlEDIServiceHandle_Result #dgvEDIServiceHandle tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #dgvEDIServiceHandle').DataTable({
                "language": {
                    "emptyTable": "No EDIServiceHandle Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #dgvEDIServiceHandle'))
            ;
        else
            $("#" + Admin_EDIServiceHandle.params["PanelID"] + " #pnlEDIServiceHandle_Result #dgvEDIServiceHandle").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        //$("#" + Admin_EDIServiceHandle.params["PanelID"] + " #pnlEDIServiceHandle_Result #dgvEDIServiceHandle").DataTable({ "bLengthChange": false, "order": [[0, "desc"]], "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillEDIServiceHandleName: function (EDIServiceHandleID, EDIServiceHandleName, PracticeId, PracticeName, EntityName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var RefCtrl = " #txtEDIServiceHandle";
        var RefHiddenIdCtrl = " #hfEDIServiceHandle";
        if (Admin_EDIServiceHandle.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_EDIServiceHandle.params["RefCtrl"];
        }
        if (Admin_EDIServiceHandle.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_EDIServiceHandle.params["RefHiddenIdCtrl"];
        }

        if (Admin_Provider.params["PanelID"] == "blockHoursDetail") {
            if (globalAppdata.AppUserName == "MDVISION")
                $('#' + Admin_EDIServiceHandle.params["PanelID"] + RefCtrl).val(EDIServiceHandleName + ' - ' + EntityName).focus();
            else
                $('#' + Admin_EDIServiceHandle.params["PanelID"] + RefCtrl).val(EDIServiceHandleName).focus();
        }
        else {
            $('#' + Admin_EDIServiceHandle.params["PanelID"] + RefCtrl).val(EDIServiceHandleName).focus();
        }
        //$('#' + Admin_EDIServiceHandle.params["PanelID"] + RefCtrl).val(EDIServiceHandleName).focus();
        $('#' + Admin_EDIServiceHandle.params["PanelID"] + RefHiddenIdCtrl).val(EDIServiceHandleID);


        if (Admin_EDIServiceHandle.params["PanelID"] == "pnlReportsSSRSDashboard") {
            Admin_EDIServiceHandle.params["EDIServiceHandleID"] = EDIServiceHandleID;
            Admin_EDIServiceHandle.params["PracticeId"] = PracticeId;
            $('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #lblEDIServiceHandle').css("display", "inline");
        } else {
            if (Admin_EDIServiceHandle.params["IsOptional"] != null && Admin_EDIServiceHandle.params["RefForm"] != null && Admin_EDIServiceHandle.params["IsOptional"] == false) {
                if ($('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #' + Admin_EDIServiceHandle.params["RefForm"]).data('bootstrapValidator') != null && typeof $('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #' + Admin_EDIServiceHandle.params["RefForm"]).data('bootstrapValidator') != 'undefined') {
                    $('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #' + Admin_EDIServiceHandle.params["RefForm"]).bootstrapValidator('revalidateField', $('#' + Admin_EDIServiceHandle.params.PanelID + RefCtrl).attr("name"));
                }
            }
            $('#' + Admin_EDIServiceHandle.params.PanelID + ' #txtPractice').val(PracticeName);
            $('#' + Admin_EDIServiceHandle.params.PanelID + ' #hfPractice').val(PracticeId);
            $('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #lblEDIServiceHandle').css("display", "none");
            $('#' + Admin_EDIServiceHandle.params["PanelID"] + ' #lnkEDIServiceHandleEdit').css("display", "inline");
        }

        if (Admin_EDIServiceHandle.params != null && Admin_EDIServiceHandle.params.ParentCtrl != null && Admin_EDIServiceHandle.params.PanelID != 'pnlAdminEDIServiceHandle') {
            UnloadActionPan(Admin_EDIServiceHandle.params.ParentCtrl, 'Admin_EDIServiceHandle', null, Admin_EDIServiceHandle.params.PanelID);
        }
        else
            UnloadActionPan(Admin_EDIServiceHandle.params["ParentCtrl"], "Admin_EDIServiceHandle");

        $('#' + Admin_EDIServiceHandle.params["PanelID"] + RefCtrl).focus();
    },

    SearchEDIServiceHandle: function (EDIServiceHandleData, EDIServiceHandleID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "EDIServiceHandleData=" + EDIServiceHandleData + "&EDIServiceHandleID=" + EDIServiceHandleID + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SERVICE_HANDLE", "SEARCH_EDI_SERVICE_HANDLE");
    },

    DeleteEDIServiceHandle: function (EDIServiceHandleID) {
        var data = "EDIServiceHandleID=" + EDIServiceHandleID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SERVICE_HANDLE", "DELETE_EDI_SERVICE_HANDLE");
    },

    UnLoadTab: function (Tab) {
        if (Admin_EDIServiceHandle.params["FromAdmin"] == "0") {


            if (Admin_EDIServiceHandle.params != null && Admin_EDIServiceHandle.params.ParentCtrl != null && Admin_EDIServiceHandle.params.PanelID != 'pnlAdminEDIServiceHandle') {
                UnloadActionPan(Admin_EDIServiceHandle.params.ParentCtrl, 'Admin_EDIServiceHandle', null, Admin_EDIServiceHandle.params.PanelID);
            }

            else if (Admin_EDIServiceHandle.params != null && Admin_EDIServiceHandle.params.ParentCtrl != null) {
                UnloadActionPan(Admin_EDIServiceHandle.params.ParentCtrl, 'Admin_EDIServiceHandle');
            }

            else
                UnloadActionPan(null, 'Admin_EDIServiceHandle');
        }
        else {
            RemoveAdminTab();
        }
    },
}