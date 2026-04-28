
Admin_Insur = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_Insur.params = params;
        if (Admin_Insur.bIsFirstLoad) {
            Admin_Insur.bIsFirstLoad = false;
            var self = $('#pnlAdminInsur');

            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").attr('disabled', 'disabled');
            }
            self.loadDropDowns(true).done(function () {
                //if (globalAppdata['IsAdmin'] != "True") {
                //    $("#pnlInsur_Search #divInsurance_Entity").css("display", "none");
                //    $("#pnlInsur_Search #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                //}
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
                Admin_Insur.InsuranceSearch();
            });
        }
    },

    InsuranceAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Insurance", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["InsuranceId"] = null;
                params["mode"] = "Add";
                params["ParentCtrl"] = "Admin_Insur";
                LoadActionPan('insurDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    InsuranceEdit: function (InsuranceId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvInsur_row' + InsuranceId));
        AppPrivileges.GetFormPrivileges("Insurance", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = InsuranceId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["InsuranceId"] = selectedValue;
                    params["ParentCtrl"] = "Admin_Insur";
                    params["mode"] = "Edit";
                    LoadActionPan('insurDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    InsuranceDelete: function (InsuranceId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvInsur_row' + InsuranceId));
        AppPrivileges.GetFormPrivileges("Insurance", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = InsuranceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_Insur.DeleteInsurance(selectedValue).done(function (response) {
                            if (response.status != false) {
                               
                                var table1 = $("#" + Admin_Insur.params["PanelID"] + ' #dgvInsur').DataTable();
                                table1.row('.active').remove().draw(false);
                                utility.DisplayMessages(response.Message, 1);
                                CacheManager.BindCodes('GetInsurance', true);
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

    InsuranceActiveInactive: function (InsuranceId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Insurance", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = InsuranceId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        insurDetail.UpdateInsuranceActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_Insur.InsuranceSearch('0');
                                CacheManager.BindCodes('GetInsurance', true);
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

    InsuranceSearch: function (InsuranceId, PageNo, rpp) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Insurance", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlAdminInsur #pnlInsur_Result").css("display") == "none") {
                    $("#pnlAdminInsur #pnlInsur_Result").show();
                }

                var self = $("#pnlInsur_Search");
                var myJSON = self.getMyJSON();

                Admin_Insur.SearchInsurance(myJSON, InsuranceId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_Insur.InsuranceGridLoad(response);

                        var TableControl = "pnlAdminInsur #dgvInsur";
                        var PagingPanelControlID = "pnlAdminInsur #divInsurancePaging";
                        var ClassControlName = "Admin_Insur";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.InsuranceCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_Insur.InsuranceSearch(PrimaryID, PageNumber, ResultPerPage);
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

    InsuranceGridLoad: function (response) {
        $("#" + Admin_Insur.params["PanelID"] + " #dgvInsur").dataTable().fnDestroy();
        $("#" + Admin_Insur.params["PanelID"] + " #pnlInsur_Result #dgvInsur tbody").find("tr").remove();
        if (response.InsuranceCount > 0) {
            var InsuranceLoadJSONData = JSON.parse(response.InsuranceLoad_JSON);
            $.each(InsuranceLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
              
                $row.attr("id", "gvInsur_row" + item.InsuranceId);
                $row.attr("InsurId", item.InsuranceId);

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
                var MethodMode = "";
                if (Admin_Insur.params["ParentCtrl"] == "Admin_InsurancePlan") {
                    MethodMode = "Admin_InsurancePlan.FillInsuranceName('" + item.InsuranceId + "', '" + item.ShortName + "')";
                }
                var selectInsurPlan = "";
                if (Admin_Insur.params["FromAdmin"] == "0") {
                    selectInsurPlan = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", MethodMode);
                }
                else {
                    $row.attr("onclick", "Admin_Insur.InsuranceEdit('" + item.InsuranceId + "',event);");
                }

                $row.append('<td style="display:none;">' + item.InsuranceId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_Insur.InsuranceDelete(' + item.InsuranceId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_Insur.InsuranceEdit(' + item.InsuranceId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_Insur.InsuranceActiveInactive(' + item.InsuranceId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectInsurPlan + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.EmailAddress + '</td>');
                $("#" + Admin_Insur.params["PanelID"] + " #pnlInsur_Result #dgvInsur tbody").last().append($row);
            });
        }
        else {
            $('#dgvInsur').DataTable({
                "language": {
                    "emptyTable": "No Insurance Found"
                },
                "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Admin_Insur.params["PanelID"] + ' #dgvInsur'))
            ;
        else
            $("#" + Admin_Insur.params["PanelID"] + " #pnlInsur_Result #dgvInsur").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    SearchInsurance: function (InsuranceData, InsuranceId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "InsuranceData=" + InsuranceData + "&InsuranceID=" + InsuranceId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE", "SEARCH_INSURANCE");
    },

    DeleteInsurance: function (InsuranceId) {
        var data = "InsuranceID=" + InsuranceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_DETAIL", "DELETE_INSURANCE");
    },

    UnLoadTab: function (Tab) {
        if (Admin_Insur.params["FromAdmin"] == "0") {
            if (Admin_Insur.params != null && Admin_Insur.params.ParentCtrl != null) {
                UnloadActionPan(Admin_Insur.params.ParentCtrl, 'Admin_Insur', null, Admin_Insur.params.PanelID);
            }
            else
                UnloadActionPan(null, 'Admin_Insur');
        }
        else {
            RemoveAdminTab(Tab);
        }
    },
}