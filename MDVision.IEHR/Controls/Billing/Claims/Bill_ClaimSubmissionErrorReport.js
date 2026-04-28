Bill_ClaimSubmissionErrorReport = {
    params: [],
    bIsFirstLoad: true,
    ErroedClaimsList: [],

    Load: function (params) {

        if (Bill_ClaimSubmissionErrorReport.bIsFirstLoad) {
            Bill_ClaimSubmissionErrorReport.bIsFirstLoad = false;
            Bill_ClaimSubmissionErrorReport.params = params;
            Bill_ClaimSubmissionErrorReport.LoadClaimErrrorReport();
        }

        Bill_ClaimSubmissionErrorReport.CopyToClipBoard();
    },

    LoadClaimErrrorReport: function () {

        $("#" + Bill_ClaimSubmissionErrorReport.params["PanelID"] + " #dgvSubmissionReport").dataTable().fnDestroy();
        $("#" + Bill_ClaimSubmissionErrorReport.params["PanelID"] + " #pnlSubmissionReport_Result #dgvSubmissionReport tbody").find("tr").remove();
        var ClaimsSubmissionJSON = JSON.parse(Bill_ClaimSubmissionErrorReport.params["ClaimsSubmissionResponse"].ClaimsSubmissionJSON);
        var Claims_Array = Bill_ClaimSubmissionErrorReport.params["Claims_Array"];
        var ClaimErrorObject = ClaimsSubmissionJSON.EroredClaims;
        var ErroredClaims_Count = Bill_ClaimSubmissionErrorReport.params["ClaimsSubmissionResponse"].ErroredClaims_Count;
        var Submitted_Claims_Count = Number(Claims_Array.length) - Number(ErroredClaims_Count);

        var statement = Submitted_Claims_Count + " claim(s) out of " + Claims_Array.length + " claim(s) submitted successfully. Following claim(s) contain missing or invalid information.";
        $("#" + Bill_ClaimSubmissionErrorReport.params["PanelID"] + " #pnlSubmissionReport_Message").html(statement);

        var have_values = false;
        for (var i in ClaimErrorObject) {
            if (ClaimErrorObject.hasOwnProperty(i)) {

                have_values = true;
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($(this))");
                $row.attr("id", "gvClaimError_row" + i);

                var current_obj;
                Claims_Array.forEach(function (item, j) {
                    if (i == item.visitid)
                        current_obj = item;
                });

                var claimnumber = "";
                var patientId = 0;
                var visitid = 0;
                if (current_obj) {
                    claimnumber = current_obj.claimnumber;
                    patientId = current_obj.patientid;
                    visitid = current_obj.visitid;
                }

                Bill_ClaimSubmissionErrorReport.ErroedClaimsList.push(visitid);
                $row.append('<td style="display:none;" >' + visitid + '</td> <td> <a href="#" onclick="Bill_ClaimSubmissionErrorReport.LoadVisitDetail(' + visitid + ',' + patientId + ',event);"  title="View Claim Detail" >' + claimnumber + '</a></td><td>' + ClaimErrorObject[i] + '</td>');

                $("#" + Bill_ClaimSubmissionErrorReport.params["PanelID"] + " #pnlSubmissionReport_Result #dgvSubmissionReport tbody").last().append($row);

            }
        }

        if (have_values == false) {
            $("#" + Bill_ClaimSubmissionErrorReport.params["PanelID"] + " #dgvSubmissionReport").DataTable({
                "language": {
                    "emptyTable": "No Claim Error Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + Bill_ClaimSubmissionErrorReport.params["PanelID"] + " #dgvSubmissionReport"))
            ;
        else
            $("#" + Bill_ClaimSubmissionErrorReport.params["PanelID"] + " #pnlSubmissionReport_Result #dgvSubmissionReport").DataTable({ "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        $("#pnlSubmissionReport_Result div.table-responsive").css("overflow", "auto");

    },

    LoadVisitDetail: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_ClaimSubmissionErrorReport';

                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;

                LoadActionPan('EncounterChargeCapture', params, Bill_ClaimSubmission.params.PanelID);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    //Delete_ClaimSubmissionErrors: function () {

    //    var objData = new Object();

    //    objData["Visits"] = Bill_ClaimSubmissionErrorReport.ErroedClaimsList.toString();
    //    var data = JSON.stringify(objData);
    //    return MDVisionService.PMSAPIService(data, "Billing", "DeleteClaimSubmissionErrors");

    //},

    PrintReport: function () {
        var ua = window.navigator.userAgent;
        var msie = ua.indexOf("MSIE ");
        if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
            var strcontents = $("#" + Bill_ClaimSubmissionErrorReport.params["PanelID"] + " #pnlSubmissionReport_Result #dgvSubmissionReport").html();

            var ReportName = "Claim Submission Error Report";
            var windowUrl = 'Claim Submission Error Report';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open('', 'printwindow');
            printWindow.document.write('<html><head><title>' + ReportName + ' | Claim Submission Error Report</title>');
            printWindow.document.write('</head><body><table>');
            //Append the external CSS file.
            printWindow.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" />');
            //Append the DIV contents.
            printWindow.document.write(strcontents);
            printWindow.document.write('</table></body></html>');
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        }
        else {
            var ReportName = "Claim Submission Error Report";
            var contents = $("#" + Bill_ClaimSubmissionErrorReport.params["PanelID"] + " #pnlSubmissionReport_Result #dgvSubmissionReport").html();
            var frame1 = $('<iframe />');
            frame1[0].name = ReportName.toLowerCase().trim();
            frame1.attr("scrolling", "no");
            frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden" });
            $("body").append(frame1);
            var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
            frameDoc.document.open();
            //Create a new HTML document.
            frameDoc.document.write('<html><head><title>' + ReportName + ' | Claim Submission Error Report</title>');
            frameDoc.document.write('</head><body>');
            //Append the external CSS file.
            frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" /><table>');
            //Append the DIV contents.
            frameDoc.document.write(contents);
            frameDoc.document.write('</table></body></html>');
            frameDoc.document.close();
            setTimeout(function () {
                window.frames[ReportName.toLowerCase().trim()].focus();
                window.frames[ReportName.toLowerCase().trim()].print();
                frame1.remove();
            }, 200);
        }
    },

    CopyToClipBoard: function () {

        //Bill_ClaimSubmissionErrorReport.selectElementContents(document.getElementById('dgvSubmissionReport'));
        //var clipboard = new Clipboard('#' + Bill_ClaimSubmissionErrorReport.params["PanelID"] + ' #btnCopyReport', {
        //    target: function () {
        //        return document.querySelector('#' + Bill_ClaimSubmissionErrorReport.params["PanelID"] + ' #dgvSubmissionReport');
        //    }
        //});

        var clipboardSnippets = new Clipboard('#' + Bill_ClaimSubmissionErrorReport.params["PanelID"] + ' #btnCopyReport', {
            target: function (trigger) {
                return document.querySelector('#' + Bill_ClaimSubmissionErrorReport.params["PanelID"] + ' #dgvSubmissionReport');
            },
            text: function (text) {
                return document.querySelector('#' + Bill_ClaimSubmissionErrorReport.params["PanelID"] + ' #dgvSubmissionReport').innerText;
            },
            action: function (action) {
                return "copy"
            }
        });
        clipboardSnippets.on('success', function (e) {
            e.clearSelection();
            utility.DisplayMessages('Copied!', 1);
        });
        clipboardSnippets.on('error', function (e) {
            utility.DisplayMessages('Error while copying.',4);
        });

        //clipboard.destroy();
        //clipboard.on('success', function (e) {
        //    console.log(e);
        //});

        //clipboard.on('error', function (e) {
        //    console.log(e);
        //});

    },

    UnLoad: function () {

        if (Bill_ClaimSubmissionErrorReport.params != null && Bill_ClaimSubmissionErrorReport.params.ParentCtrl != undefined && Bill_ClaimSubmissionErrorReport.params.ParentCtrl != null) {
            UnloadActionPan(Bill_ClaimSubmissionErrorReport.params.ParentCtrl);
        }
        else
            UnloadActionPan();
    },

};