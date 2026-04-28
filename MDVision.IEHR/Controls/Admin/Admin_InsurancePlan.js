
Admin_InsurancePlan = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Admin_InsurancePlan.params = params;
        if (Admin_InsurancePlan.bIsFirstLoad) {
            Admin_InsurancePlan.bIsFirstLoad = false;
            Admin_InsurancePlan.LoadAllAutocomplete();
            var self = "";
            if (Admin_InsurancePlan.params["PanelID"] != "pnlAdminInsurancePlan")
                self = $('#' + Admin_InsurancePlan.params["PanelID"] + ' #pnlAdminInsurancePlan');
            else
                self = $('#pnlAdminInsurancePlan');
            if ((Admin_InsurancePlan.params.ParentCtrl == "patTabInsurance" || Admin_InsurancePlan.params.ParentCtrl == "Patient_Insurance") && Admin_InsurancePlan.params.RefCtrl != null) {
                self.find('#txtDescription').val(Admin_InsurancePlan.params.RefCtrl);
            }

            self.loadDropDowns(true).done(function () {
                Admin_InsurancePlan.InsurancePlanSearch();
                if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                    self.find("#ddlEntity").attr('disabled', 'disabled');
                    self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
                }
            });
        }
        if (Admin_InsurancePlan.params.TabID == "adminTabInsurancePlan") {
            $('#' + Admin_InsurancePlan.params.PanelID + ' #modaldialog').removeAttr('class');
        }
    },

    InsurancePlanAdd: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Insurance Plan", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["InsurancePlanId"] = "-1";
                params["mode"] = "Add";
                params["FromAdmin"] = Admin_InsurancePlan.params["FromAdmin"];
                if (Admin_InsurancePlan.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_InsurancePlan';
                }
                LoadActionPan('insurancePlanDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenInsurancePlan: function () {
        var params = [];
        params["FromAdmin"] = "0";
        var PanelID = Admin_InsurancePlan.params["PanelID"];
        params["ParentCtrl"] = 'Admin_InsurancePlan';
        LoadActionPan('Admin_Insur', params, PanelID);
    },
    OpenInsurancePlanDetail: function () {
        var params = [];
        params["ParentCtrl"] = 'Admin_InsurancePlan';
        params["InsuranceId"] = $("#" + Admin_InsurancePlan.params.PanelID + " #hfAdminInsurancePlan").val();
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        LoadActionPan('insurDetail', params);
    },
    FillInsuranceName : function (insuranceId,shorName)
    {
        $("#" + Admin_InsurancePlan.params.PanelID + " #txtAdminInsurancePlan").val(shorName);
        $("#" + Admin_InsurancePlan.params.PanelID + " #hfAdminInsurancePlan").val(insuranceId);
        if (Admin_InsurancePlan.params["PanelID"] == "Admin_InsurancePlan") {
            UnloadActionPan(Admin_Insur.params.ParentCtrl, 'Admin_InsurancePlan', null, Admin_Insur.params.PanelID);
        } else {
            if (Admin_Insur.params["ParentCtrl"]) {
                UnloadActionPan(Admin_Insur.params["ParentCtrl"]);
            }
            else {
                UnloadActionPan(null, "Admin_InsurancePlan");
            }
        }
    },
    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetInsurance', false).done(function (result) {
            var Ctrl = $("#" + Admin_InsurancePlan.params["PanelID"] + " input#txtAdminInsurancePlan");
            var hfCtrl = $("#" + Admin_InsurancePlan.params["PanelID"] + " #hfAdminInsurancePlan");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", arrAdminInsurances, null, hfCtrl);
        });
    },

   

    InsurancePlanEdit: function (InsurancePlanId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvInsurancePlan_row' + InsurancePlanId));
        AppPrivileges.GetFormPrivileges("Insurance Plan", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue = InsurancePlanId;
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    var params = [];
                    params["InsurancePlanId"] = selectedValue;
                    params["mode"] = "Edit";
                    params["FromAdmin"] = Admin_InsurancePlan.params["FromAdmin"];
                    //if (Admin_InsurancePlan.params["FromAdmin"] == "0") {
                    params["ParentCtrl"] = 'Admin_InsurancePlan';
                    //}
                    LoadActionPan('insurancePlanDetail', params);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    InsurancePlanDelete: function (InsurancePlanId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        utility.SelectGridRow($('#gvInsurancePlan_row' + InsurancePlanId));
        AppPrivileges.GetFormPrivileges("Insurance Plan", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('1', function () {
                    var selectedValue = InsurancePlanId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Admin_InsurancePlan.DeleteInsurancePlan(selectedValue).done(function (response) {
                            if (response.status != false) {
                                var table1 = $('#' + Admin_InsurancePlan.params["PanelID"] + ' #dgvInsurancePlan').DataTable();
                                table1.row('.active').remove().draw(false);
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

    InsurancePlanActiveInactive: function (InsurancePlanId, IsActive, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Insurance Plan", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('3', function () {
                    var selectedValue = InsurancePlanId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        insurancePlanDetail.UpdateInsurancePlanActiveInactive(selectedValue, IsActive).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                Admin_InsurancePlan.InsurancePlanSearch('0');
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

    InsurancePlanSearch: function (InsurancePlanId, PageNo, rpp) {
        var strMessage = "";
        if (Admin_InsurancePlan.params["MatchedInsurancePlan"] != undefined && Admin_InsurancePlan.params["MatchedInsurancePlan"] != "" && Admin_InsurancePlan.params["MatchedInsurancePlan"] != null) {
            Admin_InsurancePlan.InsurancePlanGridLoad(Admin_InsurancePlan.params["MatchedInsurancePlan"]);
            $("#" + Admin_InsurancePlan.params["PanelID"] + " #pnlInsurancePlan_Result").show();
            Admin_InsurancePlan.params["MatchedInsurancePlan"] = "";
        } else {
            AppPrivileges.GetFormPrivileges("Insurance Plan", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if ($("#" + Admin_InsurancePlan.params["PanelID"] + " #pnlInsurancePlan_Result").css("display") == "none") {
                        $("#" + Admin_InsurancePlan.params["PanelID"] + " #pnlInsurancePlan_Result").show();
                    }

                    var self = $("#" + Admin_InsurancePlan.params["PanelID"] + " #pnlInsurancePlan_Search");
                    var myJSON = self.getMyJSON();

                    Admin_InsurancePlan.SearchInsurancePlan(myJSON, InsurancePlanId, null, PageNo, rpp).done(function (response) {
                        if (response.status != false) {
                            Admin_InsurancePlan.InsurancePlanGridLoad(response);
                            var TableControl = Admin_InsurancePlan.params["PanelID"] + " #dgvInsurancePlan";
                            var PagingPanelControlID = Admin_InsurancePlan.params["PanelID"] + " #divInsurancePlanPaging";
                            var ClassControlName = "Admin_InsurancePlan";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(CreatePagination(response.InsurancePlanCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                                Admin_InsurancePlan.InsurancePlanSearch(PrimaryID, PageNumber, ResultPerPage);
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
        }
    },

    InsurancePlanGridLoad: function (response) {
        $("#" + Admin_InsurancePlan.params["PanelID"] + " #dgvInsurancePlan").dataTable().fnDestroy();
        $("#" + Admin_InsurancePlan.params["PanelID"] + " #pnlInsurancePlan_Result #dgvInsurancePlan tbody").find("tr").remove();
        if (response.InsurancePlanCount > 0) {
            var InsurancePlanLoadJSONData = JSON.parse(response.InsurancePlanLoad_JSON);
            $.each(InsurancePlanLoadJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.attr("id", "gvInsurancePlan_row" + item.InsurancePlanId);
                $row.attr("InsurancePlanId", item.InsurancePlanId);

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
                if (Admin_InsurancePlan.params["ParentCtrl"] == "patTabCaseManagement") {
                    MethodMode = "Patient_Case.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "Patient_Case_Detail") {
                    MethodMode = "Patient_Case_Detail.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "Patient_Ledger") {
                    MethodMode = "Patient_Ledger.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "Patient_Search") {
                    MethodMode = "Patient_Search.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "chargeSearchDetail") {
                    MethodMode = "chargeSearchDetail.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "revenuecodeDetail" || Admin_InsurancePlan.params["ParentCtrl"] == "placeOfServiceDetail" || Admin_InsurancePlan.params["ParentCtrl"] == "cptcodeDetail" || Admin_InsurancePlan.params["ParentCtrl"] == "typeOfServiceDetail" || Admin_InsurancePlan.params["PanelID"] == "pnlBillClaimSubmission") {
                    MethodMode = "Admin_InsurancePlan.FillInsurancePlan('" + item.InsurancePlanId + "', '" + item.Description + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "billTabUnClaimedAppointment") {
                    MethodMode = "Bill_UnClaimedAppointment.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.Description + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "ERA_ChargeSearch") {
                    MethodMode = "ERA_ChargeSearch.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["TabID"] == "batchTabPatientEligibility") {
                    MethodMode = "Patient_Eligibility.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "Bill_ChargeSearch") {
                    MethodMode = "Bill_ChargeSearch.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.Description + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "Patient_Case") {
                    MethodMode = "Patient_Case.FillInsurancePlanName('" + item.InsuranceId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "insurancePlanDetail") {
                    MethodMode = "insurancePlanDetail.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.Description + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "billTabFollowUpPatientAR") {
                    MethodMode = "Bill_FollowUpPatientAR.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "billTabFollowUpInsuranceAR") {
                    MethodMode = "Bill_FollowUpInsuranceAR.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "mstrTabDashBoard") {
                    MethodMode = "DashBoard.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.ShortName + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "Batch_ClinicalQualityMeasure") {
                    MethodMode = "Batch_ClinicalQualityMeasure.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.Description + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "iTrackeCQMs") {
                    MethodMode = "iTrack_eCQMs.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.Description + "')";
                }
                else if (Admin_InsurancePlan.params["ParentCtrl"] == "Bill_Insurance_Payment_Detail") {
                    MethodMode = "Bill_Insurance_Payment_Detail.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.Description + "')";
                }
                else {
                    MethodMode = "Patient_Insurance.FillInsurancePlanName('" + item.InsurancePlanId + "', '" + item.Description + "', '" + item.SearchPattern + "',event,'" + item.IPDescription + "','" + item.IsReferralRequired + "')";
                }

                var selectInsurPlan = "";
                if (Admin_InsurancePlan.params["FromAdmin"] == "0") {
                    selectInsurPlan = '&nbsp;<a class="btn  btn-xs" href="#" onclick="' + MethodMode + '" title="Select Record"><i class="fa fa-check black"></i></a>';
                    $row.attr("onclick", MethodMode);
                }
                else {
                    $row.attr("onclick", "Admin_InsurancePlan.InsurancePlanEdit('" + item.InsurancePlanId + "',event);");
                }
                $row.append('<td style="display:none;">' + item.InsurancePlanId + '</td><td><a class="btn  btn-xs" href="#" onclick="Admin_InsurancePlan.InsurancePlanDelete(' + item.InsurancePlanId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Admin_InsurancePlan.InsurancePlanEdit(' + item.InsurancePlanId + ',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Admin_InsurancePlan.InsurancePlanActiveInactive(' + item.InsurancePlanId + ', ' + isactive + ',event);" title="' + activeTitle + '"><i class="' + tglclass + '"></i></a>' + selectInsurPlan + '</td><td>' + item.ShortName + '</td><td>' + item.Description + '</td><td>' + item.PlanTypeName + '</td><td>' + item.ClaimFlagName + '</td><td>' + item.ClaimTypeName + '</td><td>' + item.OutstandingDays + '</td><td>' + item.SearchPattern + '</td>');

                $("#" + Admin_InsurancePlan.params["PanelID"] + " #pnlInsurancePlan_Result #dgvInsurancePlan tbody").last().append($row);
            });
        }
        else {
            $('#' + Admin_InsurancePlan.params["PanelID"] + ' #dgvInsurancePlan').DataTable({
                "language": {
                    "emptyTable": "No Insurance Plan Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#' + Admin_InsurancePlan.params["PanelID"] + ' #dgvInsurancePlan'))
            ;
        else
            $("#" + Admin_InsurancePlan.params["PanelID"] + " #pnlInsurancePlan_Result #dgvInsurancePlan").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
    },

    FillInsurancePlan: function (InsurancePlanId, InsurancePlanName) {

        var RefCtrl = " #txtCodePlan";
        var RefHiddenIdCtrl = " #hfPlan";
        if (Admin_InsurancePlan.params["RefCtrl"] != null) {
            RefCtrl = " #" + Admin_InsurancePlan.params["RefCtrl"];
        }
        if (Admin_InsurancePlan.params["RefHiddenIdCtrl"] != null) {
            RefHiddenIdCtrl = " #" + Admin_InsurancePlan.params["RefHiddenIdCtrl"];
        }
        $('#' + Admin_InsurancePlan.params["PanelID"] + RefCtrl).val(InsurancePlanName);
        $('#' + Admin_InsurancePlan.params["PanelID"] + RefHiddenIdCtrl).val(InsurancePlanId);

        UnloadActionPan(Admin_InsurancePlan.params["ParentCtrl"]);
    },

    SearchInsurancePlan: function (InsurancePlanData, InsurancePlanId, SubscriberID, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var SubScrbrId = "";
        if (SubscriberID == null) {
            if (Admin_InsurancePlan.params["SubScriberId"] != null) {
                SubScrbrId = Admin_InsurancePlan.params["SubScriberId"];
            }
        }
        else
            SubScrbrId = SubscriberID;
        SubScrbrId = utility.replaceSymbols(SubScrbrId, "%", "!per");
        var data = "InsurancePlanData=" + InsurancePlanData + "&InsurancePlanID=" + InsurancePlanId + "&SubScriberId=" + SubScrbrId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN", "SEARCH_INSURANCE_PLAN");
    },

    DeleteInsurancePlan: function (InsurancePlanId) {
        var data = "InsurancePlanID=" + InsurancePlanId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN_DETAIL", "DELETE_INSURANCE_PLAN");
    },

    LoadInsuranceDBCall: function (ShortName) {
        var data = "ShortName=" + ShortName;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_INSURANCE_PLAN", "LOAD_INSURANCE_LOOKUP");
    },

    UnLoadTab: function (Tab) {
        if (Admin_InsurancePlan.params["FromAdmin"] == "0") {
            if (Admin_InsurancePlan.params != null && Admin_InsurancePlan.params.ParentCtrl != null) {
                UnloadActionPan(Admin_InsurancePlan.params.ParentCtrl, 'Admin_InsurancePlan', null, Admin_InsurancePlan.params.PanelID);
            }
            else
                UnloadActionPan(null, 'Admin_InsurancePlan');
        }
        else {
            RemoveAdminTab(Tab);
        }
    },

    ShowHistory: function () {
        var PanelID = 'Admin_InsurancePlan';
        var ParentCtrl = 'Admin_InsurancePlan';
        var ProfileName = 'Insurance Plan';
        var DBTableName = 'InsurancePlan';
        var ColumnKeyId = modifierDetail.params.ModifierId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}