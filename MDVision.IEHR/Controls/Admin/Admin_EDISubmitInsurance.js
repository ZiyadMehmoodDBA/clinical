
Admin_EDISubmitInsurance = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Admin_EDISubmitInsurance.params = params;

        if (Admin_EDISubmitInsurance.bIsFirstLoad) {
            Admin_EDISubmitInsurance.bIsFirstLoad = false;

            var self = "";
            if (Admin_EDISubmitInsurance.params["PanelID"] != "pnlAdminEDISubmitInsurance")
                self = $("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #pnlAdminEDISubmitInsurance");
            else
                self = $('#pnlAdminEDISubmitInsurance');

            //if (Admin_EDISubmitInsurance.params["PanelID"] != "pnlAdminEDISubmitInsurance") {
            //    self = $("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #pnlAdminEDISubmitInsurance");
            //}

            self.loadDropDowns(true).done(function (response) {
                Admin_EDISubmitInsurance.EDISubmitInsuranceSearch();
            });

        }
        if (Admin_EDISubmitInsurance.params.TabID == "adminTabEDISubmitInsurance") {
            $('#' + Admin_EDISubmitInsurance.params.PanelID + ' #modaldialog').removeAttr('class');
        }
        if (Admin_EDISubmitInsurance.params.ParentCtrl != "insurancePlanDetail") {
            Admin_EDISubmitInsurance.removeDialogClasses();
        }

    },
    removeDialogClasses: function () {
        //$('#' + Admin_EDISubmitInsurance.params.PanelID + ' .modal-header').hide();
        //$('#' + Admin_EDISubmitInsurance.params.PanelID + ' #pnlEDISubmitInsurance_Search').removeClass('panel-body');

        $('#' + Admin_EDISubmitInsurance.params.PanelID + ' #modalBody').removeClass('modal-body');
        $('#' + Admin_EDISubmitInsurance.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Admin_EDISubmitInsurance.params.PanelID + ' .modal-dialog').removeAttr('class');
    },
    EDISubmitInsuranceAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Submit Insurance", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["EDISubmitInsuranceId"] = null;
                params["ParentCtrl"] = 'Admin_EDISubmitInsurance';
                params["FromAdmin"] = "0";
                params["mode"] = "Add";
                LoadActionPan('EDISubmitInsuranceDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDISubmitInsuranceEdit: function (EDISubmitInsuranceId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDISubmitInsurance_row' + EDISubmitInsuranceId));
        AppPrivileges.GetFormPrivileges("EDI Submit Insurance", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = EDISubmitInsuranceId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["EDISubmitInsuranceId"] = selectedValue;
                    params["ParentCtrl"] = 'Admin_EDISubmitInsurance';
                    params["FromAdmin"] = "0";
                    params["mode"] = "Edit";
                    LoadActionPan('EDISubmitInsuranceDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDISubmitInsuranceDelete: function (EDISubmitInsuranceId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDISubmitInsurance_row' + EDISubmitInsuranceId));
        AppPrivileges.GetFormPrivileges("EDI Submit Insurance", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = EDISubmitInsuranceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_EDISubmitInsurance.DeleteEDISubmitInsurance(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvEDISubmitInsurance').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_EDISubmitInsurance.EDISubmitInsuranceSearch();
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

    EDISubmitInsuranceActiveInactive: function (EDISubmitInsuranceId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("EDI Submit Insurance", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = EDISubmitInsuranceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        EDISubmitInsuranceDetail.UpdateEDISubmitInsuranceActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_EDISubmitInsurance.EDISubmitInsuranceSearch('0');
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

    EDISubmitInsuranceSearch: function (EDISubmitInsuranceId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Submit Insurance", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #pnlEDISubmitInsurance_Result").css("display") == "none") {
                    $("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #pnlEDISubmitInsurance_Result").show();
                }

                var self = $("#pnlEDISubmitInsurance_Search");
                var myJSON = self.getMyJSON();

                Admin_EDISubmitInsurance.SearchEDISubmitInsurance(myJSON, EDISubmitInsuranceId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_EDISubmitInsurance.EDISubmitInsuranceGridLoad(response);
                        var TableControl = Admin_EDISubmitInsurance.params["PanelID"] + " #dgvEDISubmitInsurance";
                        var PagingPanelControlID = Admin_EDISubmitInsurance.params["PanelID"] + " #divEDISubmitInsurancePaging";
                        var ClassControlName = "Admin_EDISubmitInsurance";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.EDISubmitInsuranceCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_EDISubmitInsurance.EDISubmitInsuranceSearch(PrimaryID, PageNumber, ResultPerPage);
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

    EDISubmitInsuranceGridLoad: function (response) {
        $("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #dgvEDISubmitInsurance").dataTable().fnDestroy();
        $("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #dgvEDISubmitInsurance tbody").find("tr").remove();
        if (response.EDISubmitInsuranceCount > 0) {
            var EDISubmitInsuranceLoadJSONData = JSON.parse(response.EDISubmitInsuranceLoad_JSON);
            $.each(EDISubmitInsuranceLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

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
                //
               var selectSubmitInsurance = "";
               if (Admin_EDISubmitInsurance.params["FromAdmin"] == "0" && Admin_EDISubmitInsurance.params["ParentCtrl"] == "insurancePlanDetail") {
                   var selectSubmitInsurance = "insurancePlanDetail.FillEDISubmitInsurance('" + item.EDISubmitID + "','" + item.ClearingHouseName + "','" + item.SubmitInsuranceName + "','" + item.PayorId + "');"
                   $row.attr("onclick", selectSubmitInsurance);
               
                   selectSubmitInsurance = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + selectSubmitInsurance + ' title="Select Record"><i class="fa fa-check"></i></a>';
                   //$row.attr("onclick", selectSubmitInsurance);
                   if (item.IsActive == "False") {
                       $row.attr("onclick", "");
                       selectSubmitInsurance = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + "" + ' title="Select Record"><i class="fa fa-check"></i></a>';
                   }
                   $row.append('<td style="display:none;">' + item.EDISubmitID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDISubmitInsurance.EDISubmitInsuranceDelete(' + item.EDISubmitID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDISubmitInsurance.EDISubmitInsuranceEdit(' + item.EDISubmitID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDISubmitInsurance.EDISubmitInsuranceActiveInactive(' + item.EDISubmitID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectSubmitInsurance + '</td><td>' + item.SubmitInsuranceName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.PayorId + '</td><td>' + item.PhoneNo + '</td>');
               }
                else{
                   //
                   $row.attr("onclick", "Admin_EDISubmitInsurance.EDISubmitInsuranceEdit('" + item.EDISubmitID + "',event);");
                   $row.attr("id", "gvEDISubmitInsurance_row" + item.EDISubmitID);
                   $row.attr("EDISubmitInsuranceId", item.EDISubmitID);
                $row.append('<td style="display:none;">' + item.EDISubmitID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDISubmitInsurance.EDISubmitInsuranceDelete(' + item.EDISubmitID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDISubmitInsurance.EDISubmitInsuranceEdit(' + item.EDISubmitID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDISubmitInsurance.EDISubmitInsuranceActiveInactive(' + item.EDISubmitID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.SubmitInsuranceName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.PayorId + '</td><td>' + item.PhoneNo + '</td>');
                   }
               $("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #dgvEDISubmitInsurance tbody").last().append($row);
            });
        }
        else {
            $("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #dgvEDISubmitInsurance").DataTable({
                "language": {
                    "emptyTable": "No EDI Submit Insurance Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #dgvEDISubmitInsurance") || $("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #dgvEDISubmitInsurance").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Admin_EDISubmitInsurance.params["PanelID"] + " #dgvEDISubmitInsurance").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchEDISubmitInsurance: function (EDISubmitInsuranceData, EDISubmitInsuranceId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "EDISubmitInsuranceData=" + EDISubmitInsuranceData + "&EDISubmitInsuranceID=" + EDISubmitInsuranceId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SUBMIT_INSURANCE", "SEARCH_EDI_SUBMIT_INSURANCE");
    },

    DeleteEDISubmitInsurance: function (EDISubmitInsuranceId) {
        var data = "EDISubmitInsuranceID=" + EDISubmitInsuranceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SUBMIT_INSURANCE_DETAIL", "DELETE_EDI_SUBMIT_INSURANCE");
    },

    //UnLoadTab: function (Tab) {
    //    //RemoveAdminTab(Tab);
    //    utility.UnLoadDialog("pnlEDISubmitInsurance_Search", function () {
    //        UnloadActionPan(Admin_EDISubmitInsurance.params["ParentCtrl"]);
    //    }, function () {
    //        UnloadActionPan(Admin_EDISubmitInsurance.params["ParentCtrl"]);
    //    });
    //},
    UnLoadTab: function (Tab) {
        if (Admin_EDISubmitInsurance.params["FromAdmin"] == "0") {


            if (Admin_EDISubmitInsurance.params != null && Admin_EDISubmitInsurance.params.ParentCtrl != null && Admin_EDISubmitInsurance.params.PanelID != 'pnlAdminEDISubmitInsurance') {
                UnloadActionPan(Admin_EDISubmitInsurance.params.ParentCtrl, 'Admin_EDISubmitInsurance', null, Admin_EDISubmitInsurance.params.PanelID);
            }

            else if (Admin_EDISubmitInsurance.params != null && Admin_EDISubmitInsurance.params.ParentCtrl != null) {
                UnloadActionPan(Admin_EDISubmitInsurance.params.ParentCtrl, 'Admin_EDISubmitInsurance');
            }

            else
                UnloadActionPan(null, 'Admin_EDISubmitInsurance');
        }
        else {
            RemoveAdminTab();
        }
    },
}