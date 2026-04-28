Bill_ERACharge_Detail = {
    params: null,
    bIsFirstLoad: true,
    self: null,
    Load: function (params) {
        Bill_ERACharge_Detail.params = params;

        if (Bill_ERACharge_Detail.params.PanelID != "pnlBillERAChargeDetailSearch") {
            Bill_ERACharge_Detail.self = $('#' + Bill_ERACharge_Detail.params.PanelID + ' #pnlBillERAChargeDetailSearch');
            Bill_ERACharge_Detail.params.PanelID = Bill_ERACharge_Detail.params.PanelID + ' #pnlBillERAChargeDetailSearch';
        }
        else
            Bill_ERACharge_Detail.self = $('#' + Bill_ERACharge_Detail.params.PanelID);
        Bill_ERACharge_Detail.ActiveControls(false);
        Bill_ERACharge_Detail.ERAChargeDetailLoad();

        if (Bill_ERACharge_Detail.params['ParentCtrl'] == 'Bill_ERA_Summary')
            $("#" + Bill_ERACharge_Detail.params.PanelID + " #btnLinkChange").css("display", "none");


    },
    OpenERAChargeSearch: function () {
        var params = [];
        var data = new Object();
        data['ClaimNumber'] = "";
        data['CPT'] = Bill_ERACharge_Detail.self.find('#txtCPT').val();
        data['AccountNo'] = "";
        data['FirstName'] = Bill_ERACharge_Detail.self.find('#txtFirstName').val();
        data['LastName'] = Bill_ERACharge_Detail.self.find('#txtLastName').val();
        data["DOSFrom"] = utility.RemoveTimeFromDate(null, Bill_ERACharge_Detail.self.find('#dtpDOSFrom').val());
        data["DOSTo"] = utility.RemoveTimeFromDate(null, Bill_ERACharge_Detail.self.find('#dtpDOSTo').val());
        params["ParentCtrl"] = "Bill_ERACharge_Detail";
        params["data"] = data;
        LoadActionPan('ERA_ChargeSearch', params);

    },

    OpenERALinkedChargeHistory: function () {
        var params = [];
        params["ParentCtrl"] = "Bill_ERACharge_Detail";
        params["ERADetailID"] = Bill_ERACharge_Detail.params.ERADtId;
        LoadActionPan('Bill_ERALinkedCharge_History', params);

    },

    ERAChargeDetailLoad: function () {

        AppPrivileges.GetFormPrivileges("ERA", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Bill_ERACharge_Detail.LoadERAChargeDetail(Bill_ERACharge_Detail.params.ERADtId, Bill_ERACharge_Detail.params.ERAId).done(function (response) {
                    if (response.status != false) {

                        var detailJSON = JSON.parse(response.ERA_Detail_Fill_JSON);
                        utility.bindMyJSONByName(true, detailJSON, false, Bill_ERACharge_Detail.self);
                        Bill_ERACharge_Detail.BillERAClaimAdjCodeGridLoad(response.ERA_Detail_AdjCode_JSON);
                        Bill_ERACharge_Detail.BillERALinkedChargeDetailGridLoad(response.ERA_LinkedCharge_Detail_JSON);

                        if (detailJSON.txtPaymentStatus == "Posted")
                            $("#btnLinkChange").addClass("disableAll");

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
    LoadERAChargeDetail: function (ERADtId, ERAId) {

        var objData = new JSON.constructor();
        objData["ERAID"] = ERAId;
        objData["ERADetailID"] = ERADtId;
        objData["CommandType"] = "load_eracharge_detail";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");
    },
    BillERAClaimAdjCodeGridLoad: function (AdjCodeJSON) {
        $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #dgvERAClaimAdjCode").dataTable().fnDestroy();
        $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #pnlERAAdjCode_Result #dgvERAClaimAdjCode tbody").find("tr").remove();
        var AdjCodeObject = JSON.parse(AdjCodeJSON);
        if (AdjCodeObject.length > 0) {

            $.each(AdjCodeObject, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#gvAdjCode_row" + i + "'))");
                $row.attr("id", "gvAdjCode_row" + i);

                $row.append('<td>' + item.AdjGroupCode + '</td> <td class="size-max150 ellipses" data-toggle="tooltip" data-placement="right" title="' + item.AdjGroupDescription + '">' + item.AdjGroupDescription + '</td>  <td>' + item.AdjReasonCode + '</td>  <td data-toggle="tooltip" data-placement="left" title="' + item.AdjReasonDescription + '" class="size-max150 ellipses">' + item.AdjReasonDescription + '</td><td>' + item.Amount + '</td>');

                $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #pnlERAAdjCode_Result #dgvERAClaimAdjCode tbody").last().append($row);
            });
        }
        else {
            $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #dgvERAClaimAdjCode").DataTable({
                "language": {
                    "emptyTable": "No Claim Adjustment Code Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + Bill_ERACharge_Detail.params["PanelID"] + " #dgvERAClaimAdjCode"))
            ;
        else
            $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #pnlERAAdjCode_Result #dgvERAClaimAdjCode").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        $("#pnlERAAdjCode_Result div.table-responsive").css("overflow", "auto");
    },
    BillERALinkedChargeDetailGridLoad: function (LinkedChargeJSON) {
        $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #dgvLinkedChargeDetail").dataTable().fnDestroy();
        $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #pnlERAlinkedChargeDetail_Result #dgvLinkedChargeDetail tbody").find("tr").remove();
        if (LinkedChargeJSON != null) {
            var LinkedChargeObject = JSON.parse(LinkedChargeJSON);
            if (LinkedChargeObject.length > 0) {

                $.each(LinkedChargeObject, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#gvBillChargeDetail_row" + i + "'))");
                    $row.attr("id", "gvBillChargeDetail_row" + i);

                    $row.append('<td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td> <td>' + item.CPTCode + '</td>  <td>' + item.Modifier1 + '</td>  <td>' + item.InsurancePlanName + '</td><td>' + item.ProviderName + '</td><td>' + item.FacilityName + '</td><td>' + item.InsCharges + '</td><td>' + item.InsBalance + '</td>');
                    $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #pnlERAlinkedChargeDetail_Result #dgvLinkedChargeDetail tbody").last().append($row);
                });
            }
            else {
                $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #dgvLinkedChargeDetail").DataTable({
                    "language": {
                        "emptyTable": "No Linked Charges Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
        }
        else {
            $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #dgvLinkedChargeDetail").DataTable({
                "language": {
                    "emptyTable": "No Linked Charges Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable("#" + Bill_ERACharge_Detail.params["PanelID"] + " #dgvLinkedChargeDetail"))
            ;
        else
            $("#" + Bill_ERACharge_Detail.params["PanelID"] + " #pnlERAlinkedChargeDetail_Result #dgvLinkedChargeDetail").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        $("#pnlERAlinkedChargeDetail_Result div.table-responsive").css("overflow", "auto");
    },
    ActiveControls: function (active) {
        if (!active) {
            $(Bill_ERACharge_Detail.self).find('#dtpDOSFrom, #dtpDOSTo, #txtClaimNumber, #txtFirstName ,#txtLastName , #txtDOB,#txtERAClaimNumber, #txtChargeNumber,#txtERAChargeNumber,#txtSubscriberID,#txtCPT,#txtModifiers,#txtUnits,#txtPOS,#txtStatus,#txtICN,#txtSecondaryInsPerERA,#txtSecondarySubscriberID,#txtCrossedOver,#txtPaymentStatus,#txtChargedAmount,#txtAllowedAmount,#txtPaidAmount,#txtCoInsuranceAmount,#txtDeductibleAmount,#txtCopayment,#txtLateFilingCharges,#txtInterestAmount,#txtLinkedBy,#txtLinkedDate').each(function () {
                $(this).prop('disabled', true);
            });
        }
        else {
            $(Bill_ERACharge_Detail.self).find('#dtpDOSFrom, #dtpDOSTo, #txtClaimNumber, #txtFirstName ,#txtLastName , #txtDOB,#txtERAClaimNumber, #txtChargeNumber,#txtERAChargeNumber,#txtSubscriberID,#txtCPT,#txtModifiers,#txtUnits,#txtPOS,#txtStatus,#txtICN,#txtSecondaryInsPerERA,#txtSecondarySubscriberID,#txtCrossedOver,#txtPaymentStatus,#txtChargedAmount,#txtAllowedAmount,#txtPaidAmount,#txtCoInsuranceAmount,#txtDeductibleAmount,#txtCopayment,#txtLateFilingCharges,#txtInterestAmount,#txtLinkedBy,#txtLinkedDate').each(function () {
                $(this).prop('disabled', false);
            });
        }
    },
    UpdateERADetail: function (ERADtId, ChargeID, ClaimNumber) {

        var objData = new JSON.constructor();
        objData["ChargeID"] = ChargeID;
        objData["ERADetailID"] = ERADtId;
        objData["ClaimNumber"] = ClaimNumber;
        objData["CommandType"] = "update_era_detail";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "ERA", "ERADetail");
    },
    LinkERACharge: function (ChargeID, IsLink, event, PatientID) {
        if (event != null) {
            event.stopPropagation();
        }


        if (IsLink == "true") {

            var params = [];
            params["ParentCtrl"] = "ERA_ChargeSearch";
            params["ScreenName"] = "ERAChargeDetail";
            params["ERADtlID"] = Bill_ERACharge_Detail.params.ERADtId;
            params["ChargeID"] = ChargeID;
            params["PatientID"] = PatientID;
            params["IsLink"] = IsLink;
            LoadActionPan('Bill_ERA_Charge_Link_Wizard', params);
        }
        //else {

        //    AppPrivileges.GetFormPrivileges("ERA", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //        if (strMessage == "") {
        //            ERADetail.ERALink(Bill_ERACharge_Detail.params.ERADtId, ChargeID, IsLink).done(function (response) {
        //                if (response.status != false) {
        //                    utility.DisplayMessages(response.Message, 1);
        //                    ERA_ChargeSearch.UnLoad();
        //                    Bill_ERACharge_Detail.ERAChargeDetailLoad();
        //                }
        //                else {
        //                    utility.DisplayMessages(response.Message, 3);
        //                }

        //            });
        //        }
        //        else
        //            utility.DisplayMessages(strMessage, 2);

        //    });
        //}

        
    },
    UnLoad: function () {
        //if ($('#frmERAdetail').serialize() != $('#frmERAdetail').data('serialize')) {
        //    utility.myConfirm('2', function () {
        //        if (Bill_ERACharge_Detail.params != null && Bill_ERACharge_Detail.params.ParentCtrl != null) {
        //            UnloadActionPan(Bill_ERACharge_Detail.params.ParentCtrl, "Bill_ERACharge_Detail");
        //        }
        //    }, function () { },
        //            '2'
        //    );
        //}
        //else {
        UnloadActionPan(Bill_ERACharge_Detail.params.ParentCtrl, "Bill_ERACharge_Detail");
        //}
    },
}