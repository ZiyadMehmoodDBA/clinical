Bill_PatientPaymentsPrint = {
    bIsFirstLoad: true,
    params: [],
    pdf: '',
    Load: function (params) {
        Bill_PatientPaymentsPrint.params = params;
        if (Bill_PatientPaymentsPrint.params != null && Bill_PatientPaymentsPrint.params.PanelID != "pnlPreviewPatientPayments" && Bill_PatientPaymentsPrint.params.IsFromActivityLog != true) {
            Bill_PatientPaymentsPrint.params["PanelID"] = Bill_PatientPaymentsPrint.params["PanelID"] + ' #pnlPreviewPatientPayments';
        }
        else {
            Bill_PatientPaymentsPrint.params["PanelID"] = "pnlPreviewPatientPayments"
        }


        if (Bill_PatientPaymentsPrint.bIsFirstLoad) {
            Bill_PatientPaymentsPrint.bIsFirstLoad = false;
            var self = $('#' + Bill_PatientPaymentsPrint.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                if (Bill_PatientPaymentsPrint.params.IsFromActivityLog == true) {
                    Bill_PatientPaymentsPrint.PrintNotesLog();
                    $("#" + Bill_PatientPaymentsPrint.params.PanelID + " #ModalTitle").text("Print Claims Detail-Notes Log");
                } else {
                    Bill_PatientPaymentsPrint.PrintPatientPayments(Bill_PatientPaymentsPrint.params.ArrPayments);
                    $("#" + Bill_PatientPaymentsPrint.params.PanelID + " #ModalTitle").text("Print Preview Patient Payments");
                }
            });
        }
    },

    PrintPatientPayments: function (ArrPayments) {
        var rows = "";
            var def = $.Deferred();
            setTimeout(function () {
                obj = $("#pnlPatientPayments #PrintPatientPayments")
                $("#pnlPatientPayments #PrintPatientPayments").removeClass("hidden");
                $("#pnlPatientPayments #PrintPatientPayments").css("display", "inline");

                kendo.drawing.drawDOM($("#pnlPatientPayments #PrintPatientPayments #PrintDiv"), {
                    landscape: false,
                    scale: 0.6,
                    paperSize: "A4",
                    margin: {
                        left: "3mm",
                        top: "3mm",
                        right: "3mm",
                        bottom: "3mm"
                    },
                }).then(function (group) {
                    kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                        var params = [];
                        params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                        params["PreviewPdf"] = true;
                        Bill_PatientPaymentsPrint.pdf = params["PrintPDFDataURL"];
                        //if (isPreview)

                        utility.PDFViewer(params["PrintPDFDataURL"], false, 'pnlPreviewPatientPayments #frmPreviewPatientPayments #PreviewPrintReceipt', true);

                        def.resolve();
                        $("#pnlPatientPayments #PrintPatientPayments").addClass("hidden");
                    });
                });
            }, 500);

            return def.promise();
    },

    PrintNotesLog: function () {
        var def = $.Deferred();
        setTimeout(function () {
            $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog").removeClass("hidden");
            $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog").css("display", "inline");

            kendo.drawing.drawDOM($("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog"), {
                landscape: false,
                scale: 0.6,
                paperSize: "A4",
                margin: {
                    left: "3mm",
                    top: "3mm",
                    right: "3mm",
                    bottom: "3mm"
                },
            }).then(function (group) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    var params = [];
                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                    params["PreviewPdf"] = true;
                    Bill_PatientPaymentsPrint.pdf = params["PrintPDFDataURL"];
                    //if (isPreview)

                    utility.PDFViewer(params["PrintPDFDataURL"], false, 'pnlPreviewPatientPayments #frmPreviewPatientPayments #PreviewPrintReceipt', true);

                    def.resolve();
                    $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog").addClass("hidden");
                    $("#pnlActivityLog #pnlActivityLog_NotesLog #PrintNotesLog").css("display", "none");
                });
            });
        }, 500);

        return def.promise();
    },

    printPatientPayments: function () {
        $("#" + Bill_PatientPaymentsPrint.params["PanelID"] + " #PreviewPrintReceipt")[0].contentWindow.focus();
        $("#" + Bill_PatientPaymentsPrint.params["PanelID"] + " #PreviewPrintReceipt")[0].contentWindow.print();
    },

    UnLoad: function () {

        Bill_PatientPaymentsPrint.pdf = "";
        UnloadActionPan(Bill_PatientPaymentsPrint.params["ParentCtrl"], "Bill_PatientPaymentsPrint");

    },
}