EncounterClaimSummary = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        EncounterClaimSummary.params = params;

        if (EncounterClaimSummary.bIsFirstLoad) {
            EncounterClaimSummary.bIsFirstLoad = false;
            EncounterClaimSummary.LoadClaimHistorySummary(EncounterClaimSummary.params["VisitId"]);
            //EncounterClaimSummary.FillVisitData(EncounterClaimSummary.params["VisitId"], EncounterClaimSummary.params.PatientId);
            //EncounterClaimSummary.LoadPatientInsurances(EncounterClaimSummary.params.PatientId);
        }
    },

    LoadClaimHistorySummary: function (VisitId) {
        EncounterClaimSummary.ClaimHistoryFill(VisitId).done(function (response) {
            var PatientDetail_JSON = JSON.parse(response.PatientDetail_JSON);
            var ClaimDetail_JSON = JSON.parse(response.ClaimDetail_JSON);
            var ICDDetail_JSON = JSON.parse(response.ICDDetail_JSON);
            var CPTDetail_JSON = JSON.parse(response.CPTDetail_JSON);
            var InsuranceDetail_JSON = JSON.parse(response.InsuranceDetail_JSON);
            var PaymentDetail_JSON = JSON.parse(response.PaymentDetail_JSON);

            $.each(PatientDetail_JSON, function (i, item) {
                $('#' + EncounterClaimSummary.params.PanelID + " #SpnPatientName").text(item.Patient);
                $('#' + EncounterClaimSummary.params.PanelID + " #SpnDOB").text(utility.RemoveTimeFromDate(null, item.DOB));
                $('#' + EncounterClaimSummary.params.PanelID + " #SpnPhoneNo").text(item.Phone);
                $('#' + EncounterClaimSummary.params.PanelID + " #SpnPatientAddress").text(item.Address);
                $('#' + EncounterClaimSummary.params.PanelID + " #SpnClaimDate").text(utility.RemoveTimeFromDate(null, item.ClaimDate));
                $('#' + EncounterClaimSummary.params.PanelID + " #SpnDOSFrom").text(utility.RemoveTimeFromDate(null, item.DOS));
                $('#' + EncounterClaimSummary.params.PanelID + " #SpnProvider").text(item.Provider);
            });

            $.each(ClaimDetail_JSON, function (i, item) {
                $('#' + EncounterClaimSummary.params.PanelID + " #lblTotalAmount").text(globalAppdata.DefaultCurrency + Number(item.TotalAmount).toFixed(Number(globalAppdata.DecimalPlaces)));
                $('#' + EncounterClaimSummary.params.PanelID + " #lblPaymentAdjustment").text(globalAppdata.DefaultCurrency + Number(item.PmtAdj).toFixed(Number(globalAppdata.DecimalPlaces)));
                $('#' + EncounterClaimSummary.params.PanelID + " #lblBalance").text(globalAppdata.DefaultCurrency + Number(item.TotalBalance).toFixed(Number(globalAppdata.DecimalPlaces)));
                $('#' + EncounterClaimSummary.params.PanelID + " #lblClaimNumber").text(item.ClaimNumber);
                $('#' + EncounterClaimSummary.params.PanelID + " #lblSubmitStatus").text(item.SubmitStatus);
            });

            var strHtml = "";
            $.each(ICDDetail_JSON, function (i, item) {
                strHtml += "<label><b>" + item.ICDCode + "</b> " + item.ICDCodeDescription + " </label></br>";
            });

            $("#divICDDescription").html(strHtml);
            $.each(CPTDetail_JSON, function (i, item) {
                var $row = $('<tr />');
                var strModifier = "";
                if (item.Modifier1) {
                    strModifier += item.Modifier1
                }
                if (item.Modifier2) {
                    strModifier += "," + item.Modifier2
                }
                if (item.Modifier3) {
                    strModifier += "," + item.Modifier3
                }
                if (item.Modifier4) {
                    strModifier += "," + item.Modifier4
                }

                $row.append('<td>' + item.Code + '</td><td>' + strModifier + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSTo) + '</td><td>' + item.POS + '</td><td>' + item.TOS + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.Fee).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + item.units + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.BilledFee).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td>');
                $("#" + EncounterClaimSummary.params["PanelID"] + " #pnlEncounterClaimSummary_Result #dgvCPTDetail tbody").last().append($row);
            });

            $.each(InsuranceDetail_JSON, function (i, item) {
                var $row = $('<tr />');
                $row.append('<td>' + item.InsurancePlanName + '</td><td>' + item.GroupId + '</td><td>' + item.SubscriberId + '</td><td>' + item.Code + '</td><td>' + item.FileStatus + '</td>');
                $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvPatientInsurances_Result #dgvPatientInsurances tbody").last().append($row);
            });

            $.each(PaymentDetail_JSON, function (i, item) {
                var $row = $('<tr/>');
                $row.append('<td>' + item.From + '</td><td>' + utility.RemoveTimeFromDate(null, item.Date) + '</td><td>' + item.Type + '</td><td>' + item.CheckNo + '</td><td class="text-right">' + globalAppdata.DefaultCurrency + Number(item.Payments).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td>');
                $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvPayments_Result #dgvPayments tbody").last().append($row);
            });

        });
    },

    ClaimHistoryFill: function (VisitId) {
        var data = "VisitID=" + VisitId;
        return MDVisionService.defaultService(data, "PATIENT_CLAIM_SUMMARY", "GET_CLAIM_SUMMARY");
    },



    UnLoad: function () {
        UnloadActionPan(EncounterClaimSummary.params.ParentCtrl, 'EncounterClaimSummary');
    },

    PrintReport: function () {
        AppPrivileges.GetFormPrivileges("Charges", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + EncounterClaimSummary.params["PanelID"] + " #PrintReport").hide();
                $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvCPTDetail tr td").css("text-align", "center");
                $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvPatientInsurances tr td").css("text-align", "center");
                $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvPayments tr td").css("text-align", "center");
                var ua = window.navigator.userAgent;
                var msie = ua.indexOf("MSIE ");
                if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
                    var strcontents = $("#" + EncounterClaimSummary.params["PanelID"] + " #mainScrollPan").html();

                    var ReportName = "Claim Summary";
                    var windowUrl = 'Claim Summary';
                    var uniqueName = new Date();
                    var windowName = 'Print' + uniqueName.getTime();
                    var printWindow = window.open('', 'printwindow');
                    printWindow.document.write('<html><head><title>' + ReportName + ' | PrintReport</title>');
                    printWindow.document.write('</head><body>');
                    //Append the external CSS file.
                    printWindow.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
                    //Append the DIV contents.
                    printWindow.document.write(strcontents);
                    printWindow.document.write('</body></html>');
                    printWindow.document.close();
                    printWindow.focus();
                    printWindow.print();
                    printWindow.close();

                    setTimeout(function () {
                        $("#" + EncounterClaimSummary.params["PanelID"] + " #PrintReport").show();
                        $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvCPTDetail tr td").css("text-align", "");
                        $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvPatientInsurances tr td").css("text-align", "");
                        $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvPayments tr td").css("text-align", "");
                    }, 200);

                }
                else {
                    $("#" + EncounterClaimSummary.params["PanelID"] + " #PrintReport").hide();
                    var ReportName = "Claim Summary";
                    setTimeout(function () {
                        var contents = $("#" + EncounterClaimSummary.params["PanelID"] + " #mainScrollPan").html();
                        var frame1 = $('<iframe />');
                        frame1[0].name = ReportName.toLowerCase().trim();
                        frame1.attr("scrolling", "no");
                        frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden" });
                        $("body").append(frame1);
                        var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                        frameDoc.document.open();
                        //Create a new HTML document.
                        frameDoc.document.write('<html><head><title>' + ReportName + ' | PrintReport</title>');
                        frameDoc.document.write('</head><body>');
                        //Append the external CSS file.
                        frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
                        //Append the DIV contents.
                        frameDoc.document.write(contents);
                        frameDoc.document.write('</body></html>');
                        frameDoc.document.close();
                        setTimeout(function () {
                            window.frames[ReportName.toLowerCase().trim()].focus();
                            window.frames[ReportName.toLowerCase().trim()].print();
                            frame1.remove();
                            $("#" + EncounterClaimSummary.params["PanelID"] + " #PrintReport").show();
                            $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvCPTDetail tr td").css("text-align", "");
                            $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvPatientInsurances tr td").css("text-align", "");
                            $("#" + EncounterClaimSummary.params["PanelID"] + " #dgvPayments tr td").css("text-align", "");
                        }, 200);
                    }, 100);
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
}