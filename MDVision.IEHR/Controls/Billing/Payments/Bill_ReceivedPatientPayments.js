Bill_ReceivedPatientPayments = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Bill_ReceivedPatientPayments.params = params;
        if (Bill_ReceivedPatientPayments.bIsFirstLoad) {
            Bill_ReceivedPatientPayments.bIsFirstLoad = false;
            Bill_ReceivedPatientPayments.RecivedPatPaymentSearch(1, 15);
        }
    },
    //------Pagination Functions------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlReceivedPatientPayments_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Bill_ReceivedPatientPayments.RecivedPatPaymentSearch(PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlReceivedPatientPayments_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_ReceivedPatientPayments.RecivedPatPaymentSearch(currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlReceivedPatientPayments_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Bill_ReceivedPatientPayments.RecivedPatPaymentSearch(currentPageNo, 15);
        }
    },

    RecivedPatPaymentSearch: function (PageNo, rpp) {

        Bill_ReceivedPatientPayments.SearchRecivedPatPayment(Bill_ReceivedPatientPayments.params.PaymentID, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatientPayments_Result #dgvReceivedPatientPayments").dataTable().fnDestroy();
                $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatientPayments_Result #dgvReceivedPatientPayments tbody").find("tr").remove();
                var self = $("#" + Bill_ReceivedPatientPayments.params.PanelID + ' #pnlReceivedPatientPayments');
                var BillReceivedPatientPaymentJSONData = JSON.parse(response.ReceivedPatientPaymentLoad_JSON);
                var paymentType = BillReceivedPatientPaymentJSONData[0]["PaymentType"];
                if (BillReceivedPatientPaymentJSONData[0]["PaymentType"]  && BillReceivedPatientPaymentJSONData[0]["PaymentType"].trim() == "Credit Card")
                {
                    if (BillReceivedPatientPaymentJSONData[0]["CardType"]) {
                        paymentType = BillReceivedPatientPaymentJSONData[0]["PaymentType"] + " - " + BillReceivedPatientPaymentJSONData[0]["CardType"];
                    }
                    else {
                        paymentType = BillReceivedPatientPaymentJSONData[0]["PaymentType"];
                    }
                }
                $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #txtPaymentType").val(paymentType);
                $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #txtPaymentDate").val(utility.RemoveTimeFromDate(null, BillReceivedPatientPaymentJSONData[0]["PaymentDate"]));
                $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #txtCardNo").val(BillReceivedPatientPaymentJSONData[0]["CardNo"]);
                $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #txtAmount").val(utility.convertToFigure(BillReceivedPatientPaymentJSONData[0]["Amount"], true));
                Bill_ReceivedPatientPayments.LoadPatReceivedInsuranceGrid(response);
                Bill_ReceivedPatientPayments.LoadPatReceivedPayments(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    LoadPatReceivedInsuranceGrid: function (response)
    {
        var BillReceivedPatientPaymentJSONData = JSON.parse(response.ReceivedPatientPaymentLoad_JSON);
        if (response.ReceivedPatientPaymentLoadJSON_Count > 0) {
           
            $.each(BillReceivedPatientPaymentJSONData, function (i, item) {
                if (item.PatientInsurance) {
                    var $row = $('<tr/>');
                    $row.append('<td>' + item.PatientInsurance + '</td> <td>' + utility.convertToFigure(item.copay, true) + '</td>');
                    $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatInsurance_Result #dgvReceivedPatInsurance tbody").last().append($row);
                }
               
            });
        }
        else {
            $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatInsurance_Result #dgvReceivedPatInsurance").DataTable({
                "language": {
                    "emptyTable": "No record found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        ////Set ToolTip for Comments.
        //$('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        //if ($.fn.dataTable.isDataTable("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatInsurance_Result #dgvReceivedPatInsurance") || $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatInsurance_Result").find("div").hasClass("dataTables_wrapper"))
        //    ;
        //else
        //    $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatInsurance_Result #dgvReceivedPatInsurance").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    
    },
    LoadPatReceivedPayments: function (response)
    {
        var BillReceivedPatientPaymentJSONData = JSON.parse(response.ReceivedPatientPaymentLoad_JSON);
        var totalPaid = 0;
        if (response.ReceivedPatientPaymentLoadJSON_Count > 0) {
            $.each(BillReceivedPatientPaymentJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.append('<td>' + item.ClaimNumber + '</td> <td>' + item.PatientName + '</td> <td>' + utility.RemoveTimeFromDate(null, item.DOS) + '</td> <td>' + item.Provider + '</td><td>' + item.Facility + '</td><td>' + utility.convertToFigure(item.InsBalance, true) + '</td><td>' + utility.convertToFigure(item.PatBalance, true) + '</td><td>' + utility.convertToFigure(item.Amount, true) + '</td>');
                $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatientPayments_Result #dgvReceivedPatientPayments tbody").last().append($row);

                totalPaid = totalPaid + Number(item.Amount);
            });
        }
        else {
            $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatientPayments_Result #dgvReceivedPatientPayments").DataTable({
                "language": {
                    "emptyTable": "No record found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        $("#" + Bill_PatientPayments.params["PanelID"] + " #totalPaid").text(utility.convertToFigure(totalPaid, true));
        ////Set ToolTip for Comments.
        //$('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        //if ($.fn.dataTable.isDataTable("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatientPayments_Result #dgvReceivedPatientPayments") || $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatientPayments_Result").find("div").hasClass("dataTables_wrapper"))
        //    ;
        //else
        //    $("#" + Bill_ReceivedPatientPayments.params["PanelID"] + " #pnlReceivedPatientPayments_Result #dgvReceivedPatientPayments").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    
    },
    UnLoadTab: function (Tab) {
        UnloadActionPan(Bill_ReceivedPatientPayments.params["ParentCtrl"]);
    },
    SearchRecivedPatPayment: function (pmtId) {
        var data = "PmtId=" + pmtId ;
        return MDVisionService.defaultService(data, "PATIENT_LEDGER", "RECEVIED_PATIENT_PAYMENT");
    }

}

