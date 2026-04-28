MonthlyPaymentTrend_Detail = {
    params: [],
    Load: function (params) {
        MonthlyPaymentTrend_Detail.params = params;
        $("#pnlMonthlyPaymentTrendDetail" + " .selectedProvider").html(params.ProviderName + "-" + params.ClaimDate);
        MonthlyPaymentTrend_Detail.MonthlyPaymentTrendDetailSearch(params.ProviderId, params.ProviderName, params.ClaimDate);

        
    },
    MonthlyPaymentTrendDetailSearch: function (ProviderId,ProviderName,ClaimDate) {
        MonthlyPaymentTrend_Detail.SearchMonthlyPaymentTrend(ProviderId, ProviderName, ClaimDate).done(function (response) {
            if (response.status != false) {
                MonthlyPaymentTrend_Detail.PaymentTrendGridLoad(response);    //this will append table data in table body and create datatables instance
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    PaymentTrendGridLoad: function (response) {
        $("#dgvPaymentTrend").dataTable().fnDestroy(); //making sure all the instances created of datatables should be destroyed before recreating it
        $("#pnlMonthlyPaymentTrendDetail #dgvPaymentTrend tbody").find("tr").remove(); //Removing all the table data from table body
        if (response.MonthlyPaymentTrendListCount > 0) {
           
            $.each(response.MonthlyPaymentTrendListInfo_JSON, function (i, item) {
                if (item.Insurance == null)
                    item.Insurance = "N/A";
                var $row = $('<tr/>');
                $row.append('<td>' + item.FaciltyName + '</td><td>' + item.Insurance + '</td><td style="text-align:center">' + item.CPTCode + '</td><td>' + item.FullName + '</td><td style="text-align:center">' + item.AccountNumber + '</td><td>' + utility.RemoveTimeFromDate(null, item.ServiceDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.PaidDate) + '</td><td style="text-align:center">' + item.ClaimNumber + '</td><td style="text-align:right;">' + utility.convertToFigure(item.Fee, true) + '</td><td style="text-align:right;">' + utility.convertToFigure(item.PaymentPaid, true) + '</td><td style="text-align:right;">' + utility.convertToFigure(item.PatientPaid, true) + '</td><td style="text-align:right;">' + utility.convertToFigure(item.InsurancePaidAmt, true) + '</td><td style="text-align:right;">' + utility.convertToFigure(item.TotalAdj, true) + '</td>');

                $("#pnlMonthlyPaymentTrendDetail #dgvPaymentTrend tbody").last().append($row);
            });
            $("#pnlMonthlyPaymentTrendDetail #dgvPaymentTrend tfoot .totalPayment").append("$"+utility.convertToFigure(MonthlyPaymentTrend_Detail.params["TotalPayment"]));
        }
        else {
            $('#pnlMonthlyPaymentTrendDetail #dgvPaymentTrend').DataTable({
                "language": {
                    "emptyTable": "No Provider Payment Trend Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Creating Data Table Instance
        if ($.fn.dataTable.isDataTable('#pnlMonthlyPaymentTrendDetail #dgvPaymentTrend'))
            ;
        else {
            $("#pnlMonthlyPaymentTrendDetail #dgvPaymentTrend").DataTable({ "bInfo": false, "bSort": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        }
    },
    UnLoad: function (Tab) {
        UnloadActionPan(ReportsSSRSPrintView.params["ParentCtrl"]);
    },
    SearchMonthlyPaymentTrend: function (ProviderId, ProviderName, ClaimDate) {
       
        var data = "ProviderId=" + ProviderId + "&ProviderName='" + ProviderName + "'&ClaimDate='" + ClaimDate+"'";
        // search parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "REPORTS_MONTHLY_PAYMENT_TREND_DETAIL", "SEARCH_MONTHTLY_PAYMENT_TREND");
    },
}