
Admin_InsurancePlanAddress = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_InsurancePlanAddress.params = params;
        if (Admin_InsurancePlanAddress.bIsFirstLoad) {
            Admin_InsurancePlanAddress.bIsFirstLoad = false;
            var self = "";
            if (Admin_InsurancePlanAddress.params["PanelID"] != "pnlAdminInsurancePlanAddress")
                self = $('#' + Admin_InsurancePlanAddress.params["PanelID"] + ' #pnlAdminInsurancePlanAddress');
            else
                self = $('#pnlAdminInsurancePlanAddress');
       
                Admin_InsurancePlanAddress.InsurancePlanAddressSearch();

        }
        if (Admin_InsurancePlanAddress.params.TabID == "adminTabInsurancePlan") {
            $('#' + Admin_InsurancePlanAddress.params.PanelID + ' #modaldialog').removeAttr('class');
        }
    },

    InsurancePlanAddressSearch: function (InsurancePlanId, PageNo, rpp) {
        var strMessage = "";
        //if (Admin_InsurancePlanAddress.params["MatchedInsurancePlan"] != undefined && Admin_InsurancePlanAddress.params["MatchedInsurancePlan"] != "" && Admin_InsurancePlanAddress.params["MatchedInsurancePlan"] != null) {
        //    Admin_InsurancePlanAddress.InsurancePlanGridLoad(Admin_InsurancePlanAddress.params["MatchedInsurancePlan"]);
        //    $("#" + Admin_InsurancePlanAddress.params["PanelID"] + " #pnlInsurancePlan_Result").show();
        //    Admin_InsurancePlanAddress.params["MatchedInsurancePlan"] = "";
        //} else {
        AppPrivileges.GetFormPrivileges("Insurance Plan", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if ($("#" + Admin_InsurancePlanAddress.params["PanelID"] + " #pnlInsurancePlan_Result").css("display") == "none") {
                //    $("#" + Admin_InsurancePlanAddress.params["PanelID"] + " #pnlInsurancePlan_Result").show();
                //}

                var self = $("#" + Admin_InsurancePlanAddress.params["PanelID"] + " #pnlInsurancePlanAddress_Search");
                var myJSON = self.getMyJSON();

                Admin_InsurancePlanAddress.LoadInsurancePlanAddress(myJSON, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        Admin_InsurancePlanAddress.InsurancePlanAddressGridLoad(response);
                        var TableControl = Admin_InsurancePlanAddress.params["PanelID"] + " #dgvPlanAddress";
                        var PagingPanelControlID = Admin_InsurancePlanAddress.params["PanelID"] + " #divInsurancePlanAddressPaging";
                        var ClassControlName = "Admin_InsurancePlanAddress";
                        var PagesToDisplay = 5;
                        var iTotalDisplayRecords = response.iTotalDisplayRecords;
                        setTimeout(CreatePagination(response.InsurancePlanAddressCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Admin_InsurancePlanAddress.InsurancePlanAddressSearch(PrimaryID, PageNumber, ResultPerPage);
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
    //}
    },

    InsurancePlanAddressGridLoad: function (response) {
        $("#" + Admin_InsurancePlanAddress.params.PanelID + " #pnlInsurancePlanAddres_Result #dgvPlanAddress").dataTable().fnDestroy();
        $("#" + Admin_InsurancePlanAddress.params.PanelID + " #pnlInsurancePlanAddres_Result #dgvPlanAddress tbody").find("tr").remove();
        if (response.InsurancePlanAddressCount > 0) {
            var InsurancePlanAddressJSON = JSON.parse(response.InsurancePlanAddress_JSON);
            $.each(InsurancePlanAddressJSON, function (i, item) {
                var $row = $('<tr/>');

                MethodMode = "Admin_InsurancePlanAddress.FillInsurancePlanAddress('" + item.InsurancePlanId + "', '" + item.InsurancePlan + "', '" + item.InsurancePlanAddressId + "', '" + item.Description + "', '" + item.SearchPattern + "')";
                $row.attr("onclick", "utility.SelectGridRow($('#gvInsurancePlanAddress_row" + item.InsurancePlanAddressId + "'))");
                $row.attr("id", "gvInsurancePlanAddress_row" + item.InsurancePlanAddressId);
                $row.attr("InsurancePlanAddressId", item.InsurancePlanAddressId);
                $row.attr("onclick", MethodMode);

                $row.append('<td style="display:none;">' + item.InsurancePlanAddressId + '</td><td>' + item.InsurancePlan + '</td><td>' + item.Description + '</td><td>' + item.Address + '</td><td>' + item.City + '</td><td>' + item.State + '</td><td>' + item.ZipCode + '</td><td>' + item.PhoneNo + '</td>');

                $("#" + Admin_InsurancePlanAddress.params.PanelID + " #pnlInsurancePlanAddres_Result #dgvPlanAddress tbody").last().append($row);
            });
        }
        else {
            $("#" + Admin_InsurancePlanAddress.params.PanelID + " #pnlInsurancePlanAddres_Result #dgvPlanAddress").DataTable({
                "language": {
                    "emptyTable": "No Plan Address Found"
                }, "autoWidth": false, "bLengthChange": false
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Admin_InsurancePlanAddress.params.PanelID + " #pnlInsurancePlanAddres_Result #dgvPlanAddress"))
            ;
        else
            $("#" + Admin_InsurancePlanAddress.params.PanelID + " #pnlInsurancePlanAddres_Result #dgvPlanAddress").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    LoadInsurancePlanAddress: function (InsurancePlanAddressData, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "InsurancePlanAddressData=" + InsurancePlanAddressData + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "LOAD_PLAN_ADDRESS");
    },

    FillInsurancePlanAddress: function (InsurancePlanId, InsurancePlanName, InsurancePlanAddressId, InsuranceDescription, SearchPattern) {

        var RefCtrl = "";
        var RefHiddenIdCtrl = "";
        var RefSearchPattern = "";
        if (Admin_InsurancePlanAddress.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_InsurancePlanAddress.params["RefCtrl"];
        }
        if (Admin_InsurancePlanAddress.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_InsurancePlanAddress.params["RefHiddenIdCtrl"];
        }
        //if (Admin_InsurancePlanAddress.params["Address"] != null) {
        //    Address = " #" + Admin_InsurancePlanAddress.params["Address"];
        //}
        if (Admin_InsurancePlanAddress.params["SearchPattern"] != null) {
            RefSearchPattern = " #" + Admin_InsurancePlanAddress.params["SearchPattern"];
        }

        $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefCtrl).val(InsuranceDescription);
        $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefHiddenIdCtrl).val(InsurancePlanId);
        $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefSearchPattern).val(SearchPattern);
        $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefCtrl).removeAttr("data-original-title");
        $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefCtrl).attr("data-original-title", InsuranceDescription);
        $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefCtrl).attr("data-toggle", "tooltip");

        $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefCtrl).attr('data-placement', "top");
        $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefCtrl).attr('data-originalval', InsuranceDescription);
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ contents: InsuranceDescription });
        //$('#' + Admin_InsurancePlanAddress.params["PanelID"] + Address).val(AddressVal);

        $Ctr = $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefCtrl);
        $hfCtr = $('#' + Admin_InsurancePlanAddress.params["PanelID"] + RefHiddenIdCtrl);

        var obj_ = { id: InsurancePlanId, value: InsurancePlanName, searchPattern: SearchPattern, IPDescription: InsuranceDescription, Isreferral: "" };
        utility.SetAutoCompleteSource($Ctr, $hfCtr, obj_);

        Admin_InsurancePlanAddress.UnLoadTab();// UnloadActionPan(Admin_InsurancePlanAddress.params["ParentCtrl"]);
        Patient_Insurance.FillInsurancePlanAddress(InsurancePlanId, InsurancePlanAddressId, true);
    },

    UnLoadTab: function (Tab) {
        if (Admin_InsurancePlanAddress.params["FromAdmin"] == "0") {
            if (Admin_InsurancePlanAddress.params != null && Admin_InsurancePlanAddress.params.ParentCtrl != null) {
                UnloadActionPan(Admin_InsurancePlanAddress.params.ParentCtrl, 'Admin_InsurancePlanAddress', null, Admin_InsurancePlanAddress.params.PanelID);
            }
            else
                UnloadActionPan(null, 'Admin_InsurancePlanAddress');
        }
        else {
            RemoveAdminTab(Tab);
        }
    },
}