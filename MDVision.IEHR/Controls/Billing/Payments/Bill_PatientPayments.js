Bill_PatientPayments = {
    bIsFirstLoad: true,
    params: [],
    pdf: '',
    PrntTtlAmt: 0,
    PrntRefunds: 0,
    Load: function (params) {
        Bill_PatientPayments.params = params;
        if (Bill_PatientPayments.bIsFirstLoad) {
            Bill_PatientPayments.bIsFirstLoad = false;
            Bill_PatientPayments.PatientPaymentSearch(1, 15);
        }
        Bill_PatientPayments.PrntTtlAmt = 0;
    },
    //------Pagination Functions------
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlPatientPayment_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Bill_PatientPayments.PatientPaymentSearch(PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlPatientPayment_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Bill_PatientPayments.PatientPaymentSearch(currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlPatientPayment_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Bill_PatientPayments.PatientPaymentSearch(currentPageNo, 15);
        }
    },

    PatientPaymentSearch: function (PageNo, rpp) {

        Bill_PatientPayments.SearchPatientPayments(Bill_PatientPayments.params.PatientId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                $("#" + Bill_PatientPayments.params["PanelID"] + " #pnlPatientPayment_Result #dgvPatientPayment").dataTable().fnDestroy();
                $("#" + Bill_PatientPayments.params["PanelID"] + " #pnlPatientPayment_Result #dgvPatientPayment tbody").find("tr").remove();
                var totalAmount = 0;
                var totalPayment = 0;
                var totalRefund = 0;
                if (response.PatientPaymentLoadJSON_Count > 0) {
                    var BillPatientPaymentJSONData = JSON.parse(response.PatientPaymentLoad_JSON);
                    $.each(BillPatientPaymentJSONData, function (i, item) {
                        var $row = $('<tr id="' + item.PaymentId + '"/>');
                        var paymentType = item.PaymentType;
                        if (item.PaymentType && item.PaymentType.trim() == "Credit Card") {
                            if (item.CardType) {
                                paymentType = item.PaymentType + " - " + item.CardType;
                            }
                        }
                        totalAmount = totalAmount + Number(item.Amount);
                        var icone = "<a class='btn btn-xs' href='#' onclick='Bill_PatientPayments.OpenRecivedPayment(" + item.PaymentId + ");'   title='Edit Record'><i class='fa fa-edit black'></i></a>";
                        var CheckBox = '<input type="checkbox" onclick="Bill_PatientPayments.CheckMarkPayments(this);" name="checkbox" id="' + item.PaymentId + '">';
                        //Handling of refund
                        if (item.Amount.indexOf("-") > -1) {
                            item.Amount = utility.convertToFigure(Number(item.Amount) * -1, true, true)
                            icone = "";
                            //CheckBox = "";
                            totalRefund++;
                        }
                        else {
                            item.Amount = utility.convertToFigure(item.Amount, true)
                            totalPayment++;
                        }

                        if (!item.CardNo) {
                            item.CardNo = "--";
                        }

                        $row.append('<td>' + CheckBox + '</td><td>' + icone + '</td><td>' + utility.RemoveTimeFromDate(null, item.PaymentDate) + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + item.ClaimNumber + '</td> <td>' + paymentType + '</td> <td>' + item.CardNo + '</td> <td>' + item.Amount + '</td>');
                        $("#" + Bill_PatientPayments.params["PanelID"] + " #pnlPatientPayment_Result #dgvPatientPayment tbody").last().append($row);

                    });


                }
                else {
                    $('#' + Bill_PatientPayments.params["PanelID"] + " #divPatientPaymentsPaging").css("display", "none");
                    $("#" + Bill_PatientPayments.params["PanelID"] + " #pnlPatientPayment_Result #dgvPatientPayment").DataTable({
                        "language": {
                            "emptyTable": "No Patient Payment Found"
                        }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
                //Set ToolTip for Comments.
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                if ($.fn.dataTable.isDataTable("#" + Bill_PatientPayments.params["PanelID"] + " #pnlPatientPayment_Result #dgvPatientPayment") || $("#" + Bill_PatientPayments.params["PanelID"] + " #pnlPatientPayment_Result").find("div").hasClass("dataTables_wrapper"))
                    ;
                else
                    $("#" + Bill_PatientPayments.params["PanelID"] + " #pnlPatientPayment_Result #dgvPatientPayment").DataTable({ "bInfo": false, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown


                $("#" + Bill_PatientPayments.params["PanelID"] + " #totalPayment").text(totalPayment);
                $("#" + Bill_PatientPayments.params["PanelID"] + " #totalRefund").text(totalRefund);
                $("#" + Bill_PatientPayments.params["PanelID"] + " #totalAmount").text(utility.convertToFigure(totalAmount, true));

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    UnLoadTab: function (Tab) {
        UnloadActionPan(Bill_PatientPayments.params["ParentCtrl"]);
    },
    SearchPatientPayments: function (patientId, PageNumber, RowsPerPage) {
        var data = "PatientId=" + patientId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        return MDVisionService.defaultService(data, "PATIENT_LEDGER", "SEARCH_PATIENT_PAYMENTS");
    },
    OpenRecivedPayment: function (PaymentID) {
        var params = [];
        params["PaymentID"] = PaymentID;
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Bill_PatientPayments';
        LoadActionPan('Bill_ReceivedPatientPayments', params);
    },

    CheckMarkPayments: function (obj) {
        if ($(obj).attr("id") == "chkAllPayments") {
            if ($(obj).prop("checked")) {
                //$("#PrintPatientPayments #popDiv #dgvPatientPaymentPrint tbody tr").remove();
                $.each($("#dgvPatientPayment").find('tr td input[name="checkbox"]'), function (i, item) {
                    if ($(item).prop("checked") == false) {
                        $(item).prop("checked", true);
                        var row = $(item).parent().parent().clone();
                        row.children()[0].remove();
                        row.children()[0].remove();
                        $("#PrintPatientPayments #popDiv #dgvPatientPaymentPrint tbody").last().append(row);
                        var amt = parseFloat(row.find("td").last().text().replace("$", "").replace("(", "-").replace(")", ""));
                        Bill_PatientPayments.PrntTtlAmt = Number(parseFloat(Bill_PatientPayments.PrntTtlAmt)) + Number(parseFloat(amt));
                        if (Number(parseFloat(row.find("td").last().text().replace("$", "").replace("(", "-").replace(")", ""))) < 0) {
                            Bill_PatientPayments.PrntRefunds = Number(parseFloat(Bill_PatientPayments.PrntRefunds)) + 1;
                        }
                    }
                });
            } else {
                $("#dgvPatientPayment").find('tr td input[name="checkbox"]').prop("checked", false);
                $("#PrintPatientPayments #popDiv #dgvPatientPaymentPrint tbody tr").remove();
                Bill_PatientPayments.PrntTtlAmt = 0;
                Bill_PatientPayments.PrntRefunds = 0;
            }
        } else {
            if ($(obj).prop("checked")) {
                $(obj).prop("checked", true);
                var row = $(obj).parent().parent().clone();
                row.children()[0].remove();
                row.children()[0].remove();
                var amt = parseFloat(row.find("td").last().text().replace("$", "").replace("(", "-").replace(")", ""));
                $("#PrintPatientPayments #popDiv #dgvPatientPaymentPrint tbody").last().append(row);
                Bill_PatientPayments.PrntTtlAmt = Number(parseFloat(Bill_PatientPayments.PrntTtlAmt)) + Number(parseFloat(amt));
                if (Number(parseFloat($(obj).parent().parent().find("td").last().text().replace("$", "").replace("(", "-").replace(")", ""))) < 0) {
                    Bill_PatientPayments.PrntRefunds = Number(parseFloat(Bill_PatientPayments.PrntRefunds)) + 1;
                }
                if ($("#dgvPatientPayment tbody").find("input[type='checkbox']:checked").length == $("#dgvPatientPayment tbody").find("input[type='checkbox']").length) {
                    $("#dgvPatientPayment thead").find("input[id='chkAllPayments']").prop("checked", true);
                } else {
                    $("#dgvPatientPayment thead").find("input[id='chkAllPayments']").prop("checked", false);
                }
            } else {
                $(obj).prop("checked", false);
                var removeItem = $(obj).attr("id");
                $("#PrintPatientPayments #popDiv #dgvPatientPaymentPrint tbody").find("tr[id=" + removeItem + "]").remove();
                Bill_PatientPayments.PrntTtlAmt = Number(parseFloat(Bill_PatientPayments.PrntTtlAmt)) - Number(parseFloat($(obj).parent().parent().find("td").last().text().replace("$", "").replace("(", "-").replace(")", "")));
                if (Number(parseFloat($(obj).parent().parent().find("td").last().text().replace("$", "").replace("(", "-").replace(")", ""))) < 0) {
                    Bill_PatientPayments.PrntRefunds = Number(parseFloat(Bill_PatientPayments.PrntRefunds)) - 1;
                }
                $("#dgvPatientPayment thead").find("input[id='chkAllPayments']").prop("checked", false)
            }
        }
    },

    PrintPatientPayments: function (obj) {
        Bill_PatientPayments.LoadPrintPractice(Patient_Ledger.params.PracticeId).done(function (response) {
            if (response.status != false) {
                if ($("#PrintPatientPayments #PrintDiv #dgvPatientPaymentPrint tbody").find('tr').length > 0) {
                    var PracticeJSon = JSON.parse(response.ReceivedPatientPaymentLoad_JSON)

                    $("#PrintPatientPayments #PrintDiv #PracticeName").text(PracticeJSon[0].Description);
                    $("#PrintPatientPayments #PrintDiv #PracticeAddress").text(PracticeJSon[0].Address);
                    $("#PrintPatientPayments #PrintDiv #PracticeCity").text(PracticeJSon[0].City + ', ' + PracticeJSon[0].State + ', ' + PracticeJSon[0].ZIPCode);
                    $("#PrintPatientPayments #PrintDiv #PracticeContact").text('Phone: ' + PracticeJSon[0].PhoneNo + ' Fax: ' + PracticeJSon[0].Fax);

                    $("#PrintPatientPayments #PrintDiv #SpnAccNum").text($("#PatientProfile #hfAccountNo").val());
                    $("#PrintPatientPayments #PrintDiv #SpnPatName").text($("#PatientProfile #hfPatientFullNameOnly").val());
                    $("#PrintPatientPayments #PrintDiv #DivPrintFooter #SpnttlPymnt").text($("#PrintPatientPayments #PrintDiv #dgvPatientPaymentPrint tbody").find('tr').length);
                    $("#PrintPatientPayments #PrintDiv #DivPrintFooter #SpnttlAmt").text(Bill_PatientPayments.PrntTtlAmt);
                    $("#PrintPatientPayments #PrintDiv #DivPrintFooter #SpnttlRefunds").text(Bill_PatientPayments.PrntRefunds);

                    var d = new Date($('#userCurrentTime').text());
                    if (d == "Invalid Date")
                        d = new Date(Date($('#userCurrentTime').text()))

                    $("#PrintPatientPayments #PrintDiv #DivPrintDate").html(utility.getFullDate(d));

                    var params = [];
                    params["mode"] = "Edit";
                    params["FromAdmin"] = "0";
                    params["ParentCtrl"] = 'Bill_PatientPayments';
                    params["ArrPayments"] = Bill_PatientPayments.ArrPayments;

                    LoadActionPan('Bill_PatientPaymentsPrint', params);
                } else {
                    utility.DisplayMessages("Please select a payment!", 3);
                }
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadPrintPractice: function (PracticeId) {
        var data = "PracticeId=" + PracticeId;
        return MDVisionService.defaultService(data, "PATIENT_LEDGER", "LOAD_PRINT_PRACTICE");
    },
}

