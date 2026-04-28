iTrack_MIPSPrintPreview = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        iTrack_MIPSPrintPreview.params = params;
        iTrack_MIPSPrintPreview.params.UlContent = params["UlContent"];
        iTrack_MIPSPrintPreview.params.Measure = params["Measure"];
        if (iTrack_MIPSPrintPreview.params == null) {
            iTrack_MIPSPrintPreview.params = [];
        }
        if (iTrack_MIPSPrintPreview.params.PanelID != "iTrack_MIPSPrintPreview") {

            iTrack_MIPSPrintPreview.params["PanelID"] = "iTrack_MIPSPrintPreview";
        }
        if (iTrack_MIPSPrintPreview.params != null) {
            iTrack_MIPSPrintPreview.params["PanelID"] = "iTrack_MIPSPrintPreview";
        }
        if (iTrack_MIPSPrintPreview.bIsFirstLoad) {
            iTrack_MIPSPrintPreview.bIsFirstLoad = false;
            var self = $('#' + iTrack_MIPSPrintPreview.params["PanelID"]);

            self.loadDropDowns(true).done(function () {
                $('#' + iTrack_MIPSPrintPreview.params.PanelID + " #frmNotesView #MeasureName").text(iTrack_MIPSPrintPreview.params.Measure);
                $('#' + iTrack_MIPSPrintPreview.params.PanelID + " #frmNotesView #printcall #ulContent").append(iTrack_MIPSPrintPreview.params.UlContent);

                iTrack_MIPSPrintPreview.PrintReports();
               
            });
        }
    },
    PrintReports: function(){
        kendo.drawing.drawDOM('#' + iTrack_MIPSPrintPreview.params["PanelID"] + " #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: kendo.template($('#' + iTrack_MIPSPrintPreview.params["PanelID"] + " #page-templateLegacy").html())
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                params["PrintPDFDataURL"] = PrintPDFDataURL;
                params["PreviewPdf"] = true;
                utility.PDFViewer(params["PrintPDFDataURL"], true, null, null, true);
               // $('#' + iTrack_MIPSPrintPreview.params["PanelID"] + " #printcall").hide();
            });

        });
    },
    UnLoad: function () {

        if (iTrack_MIPSPrintPreview.params != null && iTrack_MIPSPrintPreview.params.ParentCtrl && iTrack_MIPSPrintPreview.params.ParentCtrlPanelID) {
            if (Clinical_Notes.params.ParentCntrlLoadid == "Schedular") {
                Clinical_Notes.params.mode = "Add";
                Clinical_Notes.params.ParentCntrlLoadid = '';
            }
            UnloadActionPan(iTrack_MIPSPrintPreview.params.ParentCtrl, "iTrack_MIPSPrintPreview", null, iTrack_MIPSPrintPreview.params.ParentCtrlPanelID);
            // iTrack_MIPSPrintPreview.params = null;
        }
        else if (iTrack_MIPSPrintPreview.params != null && iTrack_MIPSPrintPreview.params.ParentCtrl) {
            if (iTrack_MIPSPrintPreview.params.ParentCtrl == "iTrack_MIPSPrintPreview") {
                UnloadActionPan("EncounterChargeCapture", "iTrack_MIPSPrintPreview");
            }
            UnloadActionPan(iTrack_MIPSPrintPreview.params.ParentCtrl, "iTrack_MIPSPrintPreview");
        }
        else {
            UnloadActionPan(null, "iTrack_MIPSPrintPreview");
        }
        if (iTrack_MIPSPrintPreview.params.Grid != null && typeof iTrack_MIPSPrintPreview.params.Grid != typeof undefined) {
            if (iTrack_MIPSPrintPreview.params.Grid == "ModifiedNote") {
                DashBoard.DashBoardModifiedNotesSearch();
            }
        }
    },
}