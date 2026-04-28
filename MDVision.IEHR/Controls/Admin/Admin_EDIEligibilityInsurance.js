Admin_EDIEligibilityInsurance = {
    bIsFirstLoad: true,
    Load: function (params) {

        Admin_EDIEligibilityInsurance.params = params;

        if (Admin_EDIEligibilityInsurance.bIsFirstLoad) {
            Admin_EDIEligibilityInsurance.bIsFirstLoad = false;

            var self = "";
            if (Admin_EDIEligibilityInsurance.params["PanelID"] != "pnlAdminEDIEligibilityInsurance") {
                self = $("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " #pnlAdminEDIEligibilityInsurance");
            } else {

                var self = $('#pnlAdminEDIEligibilityInsurance');
            }
            //self.loadDropDowns(true);
            //AppPrivileges.GetFormPrivileges("EDI Eligibility Insurance", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            //        Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceSearch();
            //    }
            //});
            self.loadDropDowns(true).done(function (response) {
                Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceSearch();
            });
            //if (Admin_EDIEligibilityInsurance.params.ParentCtrl != "Admin_InsurancePlan_Detail") {
            //    Admin_EDIEligibilityInsurance.removeDialogClasses();
            //}
        }
        if (Admin_EDIEligibilityInsurance.params.TabID == "adminTabEDIEligibilityInsurance") {
            $('#' + Admin_EDIEligibilityInsurance.params.PanelID + ' #modaldialog').removeAttr('class');
        }
        if (Admin_EDIEligibilityInsurance.params.ParentCtrl != "insurancePlanDetail") {
            Admin_EDIEligibilityInsurance.removeDialogClasses();
        }
    },
    removeDialogClasses: function () {
        //$('#' + Admin_EDISubmitInsurance.params.PanelID + ' .modal-header').hide();
        //$('#' + Admin_EDISubmitInsurance.params.PanelID + ' #pnlEDISubmitInsurance_Search').removeClass('panel-body');

        $('#' + Admin_EDIEligibilityInsurance.params.PanelID + ' #modalBody').removeClass('modal-body');
        $('#' + Admin_EDIEligibilityInsurance.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Admin_EDIEligibilityInsurance.params.PanelID + ' .modal-dialog').removeAttr('class');
    },
    EDIEligibilityInsuranceAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Eligibility Insurance", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["EDIEligibilityInsuranceId"] = null;
                params["mode"] = "Add";
                params["ParentCtrl"] = 'Admin_EDIEligibilityInsurance';
                params["FromAdmin"] = "0";
                LoadActionPan('EDIEligibilityInsuranceDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDIEligibilityInsuranceEdit: function (EDIEligibilityInsuranceId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDIEligibilityInsurance_row' + EDIEligibilityInsuranceId));
        AppPrivileges.GetFormPrivileges("EDI Eligibility Insurance", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = EDIEligibilityInsuranceId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["EDIEligibilityInsuranceId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["ParentCtrl"] = 'Admin_EDIEligibilityInsurance';
                    params["FromAdmin"] = "0";
                    LoadActionPan('EDIEligibilityInsuranceDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    EDIEligibilityInsuranceDelete: function (EDIEligibilityInsuranceId,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvEDIEligibilityInsurance_row' + EDIEligibilityInsuranceId));
        AppPrivileges.GetFormPrivileges("EDI Eligibility Insurance", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = EDIEligibilityInsuranceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_EDIEligibilityInsurance.DeleteEDIEligibilityInsurance(selectedValue).done(function (response) {
                            if (response.status != false) {
                                //var table1 = $('#dgvEDIEligibilityInsurance').DataTable();
                                //table1.row('.active').remove().draw(false);
                                Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceSearch();
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

    EDIEligibilityInsuranceActiveInactive: function (EDIEligibilityInsuranceId, IsActive,event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("EDI Eligibility Insurance", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = EDIEligibilityInsuranceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        EDIEligibilityInsuranceDetail.UpdateEDIEligibilityInsuranceActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceSearch('0');
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

    EDIEligibilityInsuranceSearch: function (EDIEligibilityInsuranceId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("EDI Eligibility Insurance", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " #pnlEDIEligibilityInsurance_Result").css("display") == "none") {
                    $("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " #pnlEDIEligibilityInsurance_Result").show();
                }
                
                var self = $("#pnlEDIEligibilityInsurance_Search");
                var myJSON = self.getMyJSON();

                Admin_EDIEligibilityInsurance.SearchEDIEligibilityInsurance(myJSON, EDIEligibilityInsuranceId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceGridLoad(response);
                        var TableControl = Admin_EDIEligibilityInsurance.params["PanelID"]+" #dgvEDIEligibilityInsurance";
                        var PagingPanelControlID = Admin_EDIEligibilityInsurance.params["PanelID"]+" #divEDIEligibilityInsurancePaging";
                        var ClassControlName = "Admin_EDIEligibilityInsurance";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.EDIEligibilityInsuranceCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceSearch(PrimaryID, PageNumber, ResultPerPage);
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

    EDIEligibilityInsuranceGridLoad: function (response) {
        $("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " #dgvEDIEligibilityInsurance").dataTable().fnDestroy();
        $("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " #dgvEDIEligibilityInsurance tbody").find("tr").remove();
        if (response.EDIEligibilityInsuranceCount > 0) {
            var EDIEligibilityInsuranceLoadJSONData = JSON.parse(response.EDIEligibilityInsuranceLoad_JSON);
            $.each(EDIEligibilityInsuranceLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceEdit('" + item.EDIEligibilityID + "',event);");
                $row.attr("id", "gvEDIEligibilityInsurance_row" + item.EDIEligibilityID);
                $row.attr("EDIEligibilityInsuranceId", item.EDIEligibilityID);
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
                var EDIEligibilityInsurance = "";
                if (Admin_EDIEligibilityInsurance.params["FromAdmin"] == "0" && Admin_EDIEligibilityInsurance.params["ParentCtrl"] == "insurancePlanDetail") {
                     var EDIEligibilityInsuranceEvent = "insurancePlanDetail.FILLEDIEligibilityInsurance('" + item.EDIEligibilityID + "','" + item.EligibilityInsuranceName + "');"
                     $row.attr("onclick", EDIEligibilityInsuranceEvent);

                   
                     if (item.IsActive == "False") {
                         $row.attr("onclick", "");
                         EDIEligibilityInsuranceEvent = '';
                     }
                     EDIEligibilityInsurance = '&nbsp;<a class="btn  btn-xs" href="#" onclick=' + EDIEligibilityInsuranceEvent + ' title="Select Record"><i class="fa fa-check"></i></a>';
                    $row.append('<td style="display:none;">' + item.EDIEligibilityID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceDelete(' + item.EDIEligibilityID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceEdit(' + item.EDIEligibilityID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceActiveInactive(' + item.EDIEligibilityID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + EDIEligibilityInsurance + '</td><td>' + item.EligibilityInsuranceName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.PayorId + '</td><td>' + item.PhoneNo + '</td>');
                }
                else {
                    //
                    $row.attr("onclick", "Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceEdit('" + item.EDIEligibilityID + "',event);");
                    $row.append('<td style="display:none;">' + item.EDIEligibilityID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceDelete(' + item.EDIEligibilityID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceEdit(' + item.EDIEligibilityID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceActiveInactive(' + item.EDIEligibilityID + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a></td><td>' + item.EligibilityInsuranceName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.PayorId + '</td><td>' + item.PhoneNo + '</td>');
                }
                //
               // $row.append('<td style="display:none;">' + item.EDIEligibilityID + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceDelete(' + item.EDIEligibilityID + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceEdit(' + item.EDIEligibilityID + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceActiveInactive(' + item.EDIEligibilityID + ', ' + isactive + ',event);" title="'+activeTitle+'"><i class="' + tglclass + '"></i></a></td><td>' + item.EligibilityInsuranceName + '</td><td>' + item.ClearingHouseName + '</td><td>' + item.PayorId + '</td><td>' + item.PhoneNo + '</td>');

                $("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " #dgvEDIEligibilityInsurance tbody").last().append($row);
            });
        }
        else {
            $("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " #dgvEDIEligibilityInsurance").DataTable({
                "language": {
                    "emptyTable": "No EDI Eligibility Insurance Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " dgvEDIEligibilityInsurance") || $("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " #dgvEDIEligibilityInsurance").parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            $("#" + Admin_EDIEligibilityInsurance.params["PanelID"] + " #dgvEDIEligibilityInsurance").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchEDIEligibilityInsurance: function (EDIEligibilityInsuranceData, EDIEligibilityInsuranceId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "EDIEligibilityInsuranceData=" + EDIEligibilityInsuranceData + "&EDIEligibilityInsuranceID=" + EDIEligibilityInsuranceId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_ELIGIBILITY_INSURANCE", "SEARCH_EDI_ELIGIBILITY_INSURANCE");
    },

    DeleteEDIEligibilityInsurance: function (EDIEligibilityInsuranceId) {
        var data = "EDIEligibilityInsuranceID=" + EDIEligibilityInsuranceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_ELIGIBILITY_INSURANCE_DETAIL", "DELETE_EDI_ELIGIBILITY_INSURANCE");
    },

    //UnLoadTab: function (Tab) {
    //    //RemoveAdminTab(Tab);
    //    utility.UnLoadDialog("pnlEDISubmitInsurance_Search", function () {
    //        UnloadActionPan(Admin_EDIEligibilityInsurance.params["ParentCtrl"]);
    //    }, function () {
    //        UnloadActionPan(Admin_EDIEligibilityInsurance.params["ParentCtrl"]);
    //    });
    //},
    UnLoadTab: function (Tab) {
        if (Admin_EDIEligibilityInsurance.params["FromAdmin"] == "0") {


            if (Admin_EDIEligibilityInsurance.params != null && Admin_EDIEligibilityInsurance.params.ParentCtrl != null && Admin_EDIEligibilityInsurance.params.PanelID != 'pnlAdminEDIEligibilityInsurance') {
                UnloadActionPan(Admin_EDIEligibilityInsurance.params.ParentCtrl, 'Admin_EDIEligibilityInsurance', null, Admin_EDIEligibilityInsurance.params.PanelID);
            }

            else if (Admin_EDIEligibilityInsurance.params != null && Admin_EDIEligibilityInsurance.params.ParentCtrl != null) {
                UnloadActionPan(Admin_EDIEligibilityInsurance.params.ParentCtrl, 'Admin_EDIEligibilityInsurance');
            }

            else
                UnloadActionPan(null, 'Admin_EDIEligibilityInsurance');
        }
        else {
            RemoveAdminTab();
        }
    },
    
}