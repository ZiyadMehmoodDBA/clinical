Bill_FollowUpClaimSplit = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Bill_FollowUpClaimSplit.params = params;
        if (Bill_FollowUpClaimSplit.params.PanelID != "pnlBillFollowUpClaimSplit")
            Bill_FollowUpClaimSplit.params.PanelID = Bill_FollowUpClaimSplit.params.PanelID + ' #pnlBillFollowUpClaimSplit';

        if (Bill_FollowUpClaimSplit.bIsFirstLoad) {

            $('#' + Bill_FollowUpClaimSplit.params.PanelID).loadDropDowns(true).done(function () {
                Bill_FollowUpClaimSplit.LoadARCharges(Bill_FollowUpClaimSplit.params.VisitId, null, 1, 10000, null);
                //$('#' + Bill_FollowUpClaimSplit.params.PanelID + ' #frmBillFollowUpClaimSplit').data('serialize', $('#' + Bill_FollowUpClaimSplit.params.PanelID + ' #frmBillFollowUpClaimSplit').serialize());
            });
        }

    },

    checkUncheckAll: function (obj) {
        //  var selectedDiv = $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlPatientDocument_Result li.active a").attr("href");
        var GridId = " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit";
        //  $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + ' input[id*="chkClaimCharge"]').prop("checked", $(obj).prop("checked"));


        if ($(obj).prop('checked'))
            $(obj).attr('title', 'Unselect all');
        else
            $(obj).attr('title', 'Select all');


        $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId).find("input[type='checkbox']")
            .prop('checked', obj.checked);

        if (obj.checked == true)
            $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + " tbody").find("input[type='checkbox']:first")
     .prop('checked', false);

    },

    SplitSelectedCharges: function (event) {
        if (event != null) {
            event.stopPropagation();
        }
        var GridId = " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit";
        var allChargesChkbox = $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + ' input[id*="chkClaimCharge"]');
        var SelectedChargesChkbx = $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + ' input[id*="chkClaimCharge"]:checked');
        var ClaimChargeIds = $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + ' input[id*="chkClaimCharge"]:checked').map(function () {
            return this.id.replace("chkClaimCharge", "");
        }).get().join(',');
        if (ClaimChargeIds == "") {
            utility.DisplayMessages("Please select any charge to split.", 4);
            return false;
        }
        else if ((allChargesChkbox.length > 0 && SelectedChargesChkbx.length > 0) && (allChargesChkbox.length == SelectedChargesChkbx.length)) {
            utility.DisplayMessages("Please Unselect atleast one charge.", 4);
            return false;
        }
        else {
            Bill_FollowUpClaimSplit.SplitCharges(Bill_FollowUpClaimSplit.params.VisitId, ClaimChargeIds);
            //utility.DisplayMessages("Selected ChargeIds are" + ClaimChargeIds, 4);
        }
    },

    SplitCharges: function (VisitId, ChargesId) {
        AppPrivileges.GetFormPrivileges("Encounter", "Edit", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Bill_FollowUpClaimSplit.ChargesSplit(VisitId, ChargesId).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages("Claims are splitted successfully.", 1);
                        Bill_FollowUpClaimSplit.LoadARCharges(Bill_FollowUpClaimSplit.params.VisitId, null, 1, 10000, null);
                        Bill_FollowUpInsuranceAR_Detail.LoadARCharges(Bill_FollowUpClaimSplit.params.VisitId);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        });
    },

    LoadARCharges: function (VisitId, ChargeId, PageNo, rpp, RowId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Encounter", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Bill_FollowUpClaimSplit.params.PanelID + " #pnlBillFollowUpClaimSplit_Result").css("display") == "none") {
                    $('#' + Bill_FollowUpClaimSplit.params.PanelID + " #pnlBillFollowUpClaimSplit_Result").show();
                }

                //var self = $("#" + Bill_PaymentPosting.params["PanelID"]);
                //var myJSON = self.getMyJSON();

                Bill_PaymentPosting.SearchCharges("", VisitId, ChargeId, PageNo, rpp).done(function (response) {
                    if (response.status != false) {
                        if (response.ChargeCount > 0) {

                            //------------Pagination-----------
                            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #divBillFollowUpClaimSplitPaging").css("display", "inline");
                            //Showing 1 to 15 of 15 entries
                            var RecordsPerPage = rpp != null ? rpp : 5;
                            var CurrentPage = PageNo != null ? PageNo : 1;

                            //params["myJSON"] = myJSON;

                            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                            //if (PageNo == null) {
                            utility.GetCustomPaging("divBillFollowUpClaimSplitPaging", response.iTotalDisplayRecords, 5, "Bill_FollowUpClaimSplit", CurrentPage, RecordsPerPage);
                            //}
                            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                            var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #divBillFollowUpClaimSplitPaging #divShowingEntries").text(showingText);
                            // Change Background Color to Black for selected page
                            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " li").each(function () {
                                if ($(this).text() == CurrentPage) {
                                    $(this).attr("class", "active");
                                }
                                else

                                    $(this).removeAttr("class");
                            });
                            //------------End Pagination-------
                        }
                        else {
                            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #divBillFollowUpClaimSplitPaging").css("display", "none");
                        }
                        Bill_FollowUpClaimSplit.ChargesGridLoad(response, RowId);
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

    ChargesGridLoad: function (response, RowId) {
        $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit").dataTable().fnDestroy();
        $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit tbody").find("tr").remove();
        if (response.ChargeCount > 0) {//response.ChargeCount
            var ChargeLoadJSONData = JSON.parse(response.ChargeLoad_JSON);
            $.each(ChargeLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvBillFollowUpClaimSplit_row" + item.ChargeCapId + "'));");
                $row.attr("id", "gvBillFollowUpClaimSplit_row" + item.ChargeCapId);
                $row.attr("ChargeId", item.ChargeCapId);

                //if (item.IsActive == "True") {
                //    isactive = 0;
                //    activeRecord = "Active Record";
                //    tglclass = "fa fa-toggle-on green";
                //}
                //else {
                //    isactive = 1;
                //    activeRecord = "Inactive Record";
                //    tglclass = "fa fa-toggle-on red";
                //}

                //var MethodMode = "";
                //var ActionBit = false;

                //var EditMethod = "Bill_PaymentPosting.ChargePaymentAdd(" + item.ChargeCapId.trim() + ",'Add','" + item.PatientId + "','" + item.PatientInsuranceId + "','" + item.InsurancePlanName + "','" + item.VisitId + "','" + item.FacilityId + "','" + item.FacilityName + "','" + item.ProviderId + "','" + item.ProviderName + "','" + item.TotalBal + "','" + item.InsCharges + "','" + item.PatCharges + "','" + item.Copay + "','" + item.InsBalance + "','" + item.PatBalance + "','" + item.CopayBalance + "','" + item.Fee + "');";

                //var ActiveInacvtiveMethod = "";//"Patient_Search.ActiveInactivePatient(" + item.PatientId.trim() + "," + isactive + ");";
                //var strAction = "";
                //strAction = '<a class="btn btn-xs" href="#sectionChargeDetail" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs mr-xs" href="#sectionChargeDetail" onclick="' + ActiveInacvtiveMethod + '" title="' + activeRecord + '"><i class="' + tglclass + '"></i></a>';

                // $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + strAction + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + item.ClaimNumber + '</td><td>' + item.CPTCode + '</td><td>' + item.InsurancePlanName + '</td><td>' + item.Fee + '</td><td>' + item.TotalBal + '</td><td>0</td><td>0</td><td>0</td><td>' + item.PatChargeAmt + '</td><td>0</td><td>0</td><td>0</td><td>' + item.Copay + '</td><td>0</td><td>0</td><td>0</td><td></td>');

                $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td><input type="checkbox" onclick="Bill_FollowUpClaimSplit.CheckUncheckCharge(this,event)" id="chkClaimCharge' + item.ChargeCapId + '" /></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + item.ClaimNumber + '</td><td>' + item.CPTCode + '</td><td>' + item.InsurancePlanName + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.Fee).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + parseFloat(item.Units).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(parseFloat(item.Units) * parseFloat(item.Fee)).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.TotalBal).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.InsCharges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.InsPaid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.InsWriteOff).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.InsBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.PatCharges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.PatPaid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.PatDiscount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.PatBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.Copay).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.CopayPaid).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.CopayDiscount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + parseFloat(item.CopayBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td>');
                $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit tbody").last().append($row);
            });


        }
        else {
            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #divBillFollowUpClaimSplitPaging").css("display", "none");
            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit").DataTable({
                "language": {
                    "emptyTable": "No Charges Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit"))
            ;
        else
            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "iDisplayLength": 5, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        if (Bill_FollowUpClaimSplit.params.ChargeId != null) {
            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit #gvBillFollowUpClaimSplit_row" + Bill_FollowUpClaimSplit.params.ChargeId).trigger("click");
        }
        else if (RowId != null) {
            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit tbody tr#" + RowId).trigger("click");
        }
        else
            $("#" + Bill_FollowUpClaimSplit.params["PanelID"] + " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit tbody tr:eq(0) ").click();


        //// setting cash for payment type
        //$('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(1);

        //utility.CreateDatePicker(Bill_PaymentPosting.params.PanelID + ' #dtpDatePaid', function () {

        //    // on-change callback method 
        //}, true);

    },

    ChargesSplit: function (VisitId, ChargesIds) {
        if (VisitId == null) {
            VisitId = -1;
        }
        if (ChargesIds == null) {
            ChargesIds = -1;
        }
        //utility.DisplayMessages("It's under construction", 1);
        var data = "VisitId=" + VisitId + "&ChargeIDs=" + ChargesIds;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_FOLLOWUP_CLAIM_SPLIT", "SPLIT_CLAIM_CHARGES");
    },

    UnLoad: function () {

        utility.UnLoadDialog(Bill_FollowUpClaimSplit.params.PanelID + ' #frmFollowUpPatientARDetail', function () {
            UnloadActionPan(Bill_FollowUpClaimSplit.params["ParentCtrl"], "Bill_FollowUpClaimSplit");
        }, function () {
            UnloadActionPan(Bill_FollowUpClaimSplit.params["ParentCtrl"], "Bill_FollowUpClaimSplit");
        });

    },


    CheckUncheckCharge: function (obj,event) {
        if (event != null) {
            event.stopPropagation();
        }

        var GridId = " #pnlBillFollowUpClaimSplit_Result #dgvBillFollowUpClaimSplit";

        if (!$(obj).prop('checked')) {
            $("#pnlBillFollowUpClaimSplit #chkMasterClaimCharge").prop('checked', false);
            $("#pnlBillFollowUpClaimSplit #chkMasterClaimCharge").attr('title', 'Select all');
            //$("#pnlBillClaimSubmission #dgvSubmission").find("input[type='checkbox']").prop('disabled', false);
        }
        else {

            var selected = [];
            $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + " tbody").find("input[type='checkbox']").each(function () {

                if (!$(this).is(":checked")) {
                    selected.push(this);
                }
            });

            if (selected.length > 0) {
                $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + " #chkMasterClaimCharge").prop('checked', false);
                $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + " #chkMasterClaimCharge").attr('title', 'Select all');

            }
            else {
                $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + " tbody").find("input[type='checkbox']:first").prop('checked', false);
                $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + " #chkMasterClaimCharge").prop('checked', true);
                $('#' + Bill_FollowUpClaimSplit.params["PanelID"] + GridId + " #chkMasterClaimCharge").attr('title', 'Unselect all');
            }
        }
    },

}