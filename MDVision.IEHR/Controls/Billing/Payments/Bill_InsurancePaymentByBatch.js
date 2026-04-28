Bill_InsurancePaymentByBatch = {
    params: [],
    Load: function (params) {
        Bill_InsurancePaymentByBatch.params = params;
        Bill_InsurancePaymentByBatch.LoadInsurancePaymentByBatch(Bill_InsurancePaymentByBatch.params.PaymentBatchID);
    },
    LoadInsurancePaymentByBatch: function (BatchId) {
        Bill_InsurancePaymentByBatch.PaymentByBatchGridLoad(BatchId);
    },
    PaymentByBatchGridLoad: function (BatchId) {

        Bill_InsurancePaymentByBatch.InsurancePaymentByBatchLoad(BatchId).done(function (response) {
            $("#dgvBillInsurancePaymentByBatch").dataTable().fnDestroy();
            if (response.PaymentByBatchCount > 0) {
                var parentctrl = 'Bill_InsurancePaymentByBatch';
                var PaymentByBatchJSONData = JSON.parse(response.PaymentByBatchLoad_JSON);
                $.each(PaymentByBatchJSONData, function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr("id", "dgvBillInsurancePaymentByBatch_row" + i);
                    $row.attr("onclick", "utility.SelectGridRow($('#dgvBillInsurancePaymentByBatch_row" + i + "'));");
                    $row.append('<td> <a href="#" onclick="Bill_InsurancePaymentByBatch.LoadVisitDetail(' + item.VisitId + ',' + item.PatientId + ',event);"  title="View Claim Detail">' + item.ClaimNumber + '</a></td><td class="ellip150" data-toggle="tooltip" data-placement="right" title="' + item.PatientName + '">' + item.PatientName + '</td><td><a href="#" onclick="utility.PatientDemographics(' + item.PatientId + ', \'' + parentctrl + '\',event);"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td><td>' + item.CPTCode + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number((Number(item.Amount))).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + item.EntryBy + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.EntryDate + '">' + item.EntryDate + '</td>');
                    $("#" + Bill_InsurancePaymentByBatch.params.PanelID + " #dgvBillInsurancePaymentByBatch tbody").last().append($row);
                });
            }
            else {
                $("#" + Bill_InsurancePaymentByBatch.params.PanelID + " #dgvBillInsurancePaymentByBatch").DataTable({
                    "language": {
                        "emptyTable": "No Payment Found against this Batch"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
                });
            }
            //Set ToolTip for Comments.
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            if ($.fn.dataTable.isDataTable('#dgvBillInsurancePaymentByBatch'))
                ;
            else
                $("#" + Bill_InsurancePaymentByBatch.params.PanelID + " #pnlBillInsurancePaymentByBatch_Result #dgvBillInsurancePaymentByBatch").DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

            $("#" + Bill_InsurancePaymentByBatch.params.PanelID + " #pnlBillInsurancePaymentByBatch_Result div.table-responsive").css("overflow", "auto");
        });
    },

    InsurancePaymentByBatchLoad: function (BatchId) {
        var data = "BatchId=" + BatchId;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "SEARCH_INSURANCE_PAYMENT_BY_BATCH");
    },

    LoadVisitDetail: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        var params = [];
        params["FromAdmin"] = 0;
        params["ParentCtrl"] = 'Bill_InsurancePaymentByBatch';
        //pnlBillPaymentByBatch
        params["VisitId"] = VisitId;
        params["patientID"] = PatientId;

        LoadActionPan('EncounterChargeCapture', params, Bill_InsurancePaymentByBatch.params.PanelID);
    },
    UnLoad: function () {
        UnloadActionPan(Bill_InsurancePaymentByBatch.params.ParentCtrl, "Bill_InsurancePaymentByBatch");
    },
}