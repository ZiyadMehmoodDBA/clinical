BillClaimSubmitHistoryPrint = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        BillClaimSubmitHistoryPrint.params = params;
        BillClaimSubmitHistoryPrint.ClaimSubmitHistory_Print(BillClaimSubmitHistoryPrint.params.JSON);
    },

    ClaimSubmitHistory_Print: function (myJSON, PageNo, rpp) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Claim Submission", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                BillClaimSubmitHistoryPrint.SearchClaimSubmitHistory(myJSON, PageNo, rpp).done(function (response) {

                    if (response.status != false) {
                        BillClaimSubmitHistoryPrint.LoadClaimSubmitHistoryGrid(response, PageNo, rpp);
                    }
                    else {
                        //utility.DisplayMessages(response.Message, 3);
                    }
                });

            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    LoadClaimSubmitHistoryGrid: function (response, PageNo, rpp) {

        $("#BillClaimSubmitHistoryPrint #dgvClaimSubmitHistoryPrint").dataTable().fnDestroy();
        $("#BillClaimSubmitHistoryPrint #dgvClaimSubmitHistoryPrint tbody").find("tr").remove();
        if (response.ClaimSubmitHistoryCount > 0) {
            $("#BillClaimSubmitHistoryPrint #pnlClaimSubmitHistory_Result").css("display", "");

            var ClaimSubmitHistoryLoadJSONData = JSON.parse(response.ClaimSubmitHistoryLoad_JSON);
            $.each(ClaimSubmitHistoryLoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this));");
                var submittype = "";
                var mark_as_submitted = "";
                if (item.SubmitType.toLowerCase() == "true")
                    submittype = "Electronic";
                else if (item.SubmitType.toLowerCase() == "false")
                    submittype = "Paper";

                if (item.SubmitType.toLowerCase() == "false" && item.IsMarkSubmitted.toLowerCase() == "true")
                    mark_as_submitted = "/Mark as Submitted";
                else if (item.SubmitType.toLowerCase() == "true" && item.IsMarkSubmitted.toLowerCase() == "true")
                    mark_as_submitted = "/Mark as Transmitted";


                $row.append(
                        '<td>' + item.AccountNumber + '</td>' +
                        '<td>' + item.PatientName + '</td>' +
                        '<td>' + item.InsuranceName + '</td>' +
                        '<td>' + item.ClaimNumber + '</td>' +
                        '<td>' + item.BatchNumber + '</td>' +
                        '<td>' + submittype + mark_as_submitted + '</td>' +
                        '<td>' + item.SubmittedBy + '</td>' +
                        '<td>' + item.SubmittedTo + '</td>' +
                        '<td>' + item.SubmittedOn + '</td>');

                $("#BillClaimSubmitHistoryPrint #dgvClaimSubmitHistoryPrint tbody").last().append($row);
            });
        }
        else {

            $("#BillClaimSubmitHistoryPrint #pnlClaimSubmitHistory_Result").css("display", "");
            $("#BillClaimSubmitHistoryPrint #dgvClaimSubmitHistoryPrint").dataTable().fnDestroy();
            $("#BillClaimSubmitHistoryPrint #dgvClaimSubmitHistoryPrint").DataTable({
                "language": {
                    "emptyTable": "No Record  Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false, bFilter: false,
            });
        }

        if ($.fn.dataTable.isDataTable("#BillClaimSubmitHistoryPrint #dgvClaimSubmitHistoryPrint"))
            ;
        else
            $("#BillClaimSubmitHistoryPrint #dgvClaimSubmitHistoryPrint").DataTable({ "bLengthChange": false, "autoWidth": false, "aaSorting": [[8, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bPaginate": false, "bInfo": false, bFilter: false, });

    },

    SearchClaimSubmitHistory: function (ClaimData, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 1000 : RowsPerPage;

        var objData = JSON.parse(ClaimData);

        objData["PageNo"] = PageNumber;
        objData["CommandType"] = "Search";
        objData["rpp"] = RowsPerPage;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Billing", "SearchClaimSubmitionHistory");
    },
    PrintReport: function () {
        $("#BillClaimSubmitHistoryPrint #printMe").hide();
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            var strcontents = $("#BillClaimSubmitHistoryPrint #PrintDetails").html();

            var ReportName = "Claim Submit History";
            var windowUrl = 'Claim Submit History';
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
                $("#BillClaimSubmitHistoryPrint #printMe").show();
                //$("#" + Patient_Ledger.params["PanelID"] + " #PrintReport").show();
                //$("#" + Patient_Ledger.params["PanelID"] + " #divSearchCharges").show();
                //$("#" + Patient_Ledger.params["PanelID"] + " .txtPatientInfo").show();
                //$("#" + Patient_Ledger.params["PanelID"] + " .lblPatientInfo").hide();
                //$("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientOutstanding tr td").css("text-align", "");
                //$("#" + Patient_Ledger.params["PanelID"] + " #dgvPatientLedger tr td").css("text-align", "");
            }, 200);
        }
        else {
            setTimeout(function () {
                var contents = $("#BillClaimSubmitHistoryPrint #PrintDetails").html();
                var frame1 = $('<iframe />');
                frame1[0].name = "Claim Submit History";
                frame1.attr("scrolling", "no");
                frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden" });
                $("body").append(frame1);
                var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                frameDoc.document.open();
                //Create a new HTML document.
                frameDoc.document.write('<html><head><title>Claim Submit History | Print</title>');
                frameDoc.document.write('</head><body>');
                //Append the external CSS file.
                frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
                //Append the DIV contents.
                frameDoc.document.write(contents);
                frameDoc.document.write('</body></html>');
                frameDoc.document.close();

                setTimeout(function () {
                    window.frames[frame1[0].name].focus();
                    window.frames[frame1[0].name].print();
                    frame1.remove();
                    $("#" + BillClaimSubmitHistoryPrint.params["PanelID"] + " #BillClaimSubmitHistoryPrint #printMe").show();
                }, 200);

            }, 100);
        }
    },

    UnLoad: function (Tab) {
        UnloadActionPan(BillClaimSubmitHistoryPrint.params["ParentCtrl"]);
    },
}