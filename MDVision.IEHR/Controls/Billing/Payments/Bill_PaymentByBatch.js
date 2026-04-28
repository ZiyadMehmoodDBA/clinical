Bill_PaymentByBatch = {
    params:[],
    Load: function (params) {
        Bill_PaymentByBatch.params = params;
        Bill_PaymentByBatch.LoadPaymentByBatch(Bill_PaymentByBatch.params.PaymentBatchID);
    },
    LoadPaymentByBatch: function (BatchId) {
        Bill_PaymentByBatch.PaymentByBatchLoad(BatchId).done(function (response) {
            if (response.status != false) {
                Bill_PaymentByBatch.PaymentByBatchGridLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        
    },
    PaymentByBatchGridLoad: function (response) {
        $("#dgvBillPaymentByBatch").dataTable().fnDestroy();
        if (response.PaymentByBatchCount > 0) {
            var parentctrl = 'Bill_PaymentByBatch';
            var PaymentByBatchJSONData = JSON.parse(response.PaymentByBatchLoad_JSON);
            $.each(PaymentByBatchJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvPaymentByBatch_row" + i);
                $row.attr("onclick", "utility.SelectGridRow($('#gvPaymentByBatch_row" + i + "'));");
                $row.append('<td>' + item.PaymentType + '</td><td class="ellip150" data-toggle="tooltip" data-placement="right" title="' + item.Description + '">' + item.Description + '</td><td>' + item.PlanName + '</td><td class="ellip150" data-toggle="tooltip" data-placement="right" title="' + item.ProviderName + '">' + item.ProviderName + '</td><td> <a href="#" onclick="Bill_PaymentByBatch.LoadVisitDetail(' + item.VisitId + ',' + item.PatientId + ',event);"  title="View Claim Detail">' + item.ClaimNumber + '</a></td><td>' + item.CPTCode + '</td><td><a href="#" onclick="utility.PatientDemographics(' + item.PatientId + ', \'' + parentctrl + '\',event);"  title="View Patient">' + item.AccountNumber + '</a></td><td class="ellip150" data-toggle="tooltip" data-placement="right" title="' + item.PatientName + '">' + item.PatientName + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + "" + Number((Number(item.Amount))).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + utility.RemoveTimeFromDate(null, item.DatePaid) + '</td><td>' + item.EntryBy + '</td><td class="ellip100" data-toggle="tooltip" data-placement="left" title="' + item.EntryDate + '">' + item.EntryDate + '</td>');
                $("#"+Bill_PaymentByBatch.params.PanelID+" #dgvBillPaymentByBatch tbody").last().append($row);
            });
        }
        else {
            $("#"+Bill_PaymentByBatch.params.PanelID+" #dgvBillPaymentByBatch").DataTable({
                "language": {
                    "emptyTable": "No Payment Found against this Batch"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }]
            });
        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if ($.fn.dataTable.isDataTable('#dgvBillPaymentByBatch'))
            ;
        else
            $("#" + Bill_PaymentByBatch.params.PanelID + " #pnlBillPaymentByBatch_Result #dgvBillPaymentByBatch").DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        $("#" + Bill_PaymentByBatch.params.PanelID + " #pnlBillPaymentByBatch_Result div.table-responsive").css("overflow", "auto");
    },
    PaymentByBatchLoad: function (BatchId) {
        var data = "BatchId=" + BatchId;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_BATCH_DETAIL", "SEARCH_PAYMENT_BY_BATCH");
    },
    LoadVisitDetail: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_PaymentByBatch';
        //pnlBillPaymentByBatch
                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;

                LoadActionPan('EncounterChargeCapture', params, Bill_PaymentByBatch.params.PanelID);
          
      
    },
    UnLoad: function () {
        UnloadActionPan(Bill_PaymentByBatch.params.ParentCtrl, "Bill_PaymentByBatch");
    },
}