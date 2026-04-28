Document_Scanner = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Document_Scanner.params = params;
        Document_Scanner.LoadDocumentScan();
    },

    LoadDocumentScan: function () {

        $("#pnlDocument_Scanner #IFDocument").attr('src', 
            "DocumentScan.aspx?pid=" + Document_Scanner.params["PracticeId"] + 
            "&PanelID=" + Document_Scanner.params["PanelID"] + 
            "&ParentCtrl=" + Document_Scanner.params["ParentCtrl"] +
            "&RefFill=" + Document_Scanner.params["RefFill"] +
            "&RefCtrl=" + Document_Scanner.params["RefCtrl"]);
    },

    UnLoadWithScanner: function () {

        var frontImage = $('#pnlDocument_Scanner #img_frontImge').attr('src');
        var backImage = $('#pnlDocument_Scanner #img_backImge').attr('src');

        if (Patient_Insurance.params.PanelID) {
            Document_Scan.params["IsFromIfram"] = true;
            Document_Scan.LoadScannedImages(frontImage, backImage);
        }
    },

    UnLoadWithProcessing: function () {

        Patient_Insurance.params["IsFromIfram"] = true;
        var frontImage = $('#pnlDocument_Scanner #img_frontImge').attr('src');
        var backImage = $('#pnlDocument_Scanner #img_backImge').attr('src');
        var IsNewInsurancePlan = $('#pnlDocument_Scanner #hfIsNewInsurancePlan').val() == "true" ? true : false;
        Patient_Insurance.fillProcessedDataWithAPI(IsNewInsurancePlan, frontImage, backImage);
        if (Document_Scan.params.PanelID == "pnlPatientInsurance") {
            Patient_Insurance.InsuranceCardChanged = "true";
        }
    },

    UnLoadTab: function () {

        if (Document_Scanner.params != null && Document_Scanner.params.PanelID == "pnlDemographicQuick") {
            Patient_DemographicQuick.frontimage = $("#imageFromScanner").attr("src");
        }

        if (Document_Scanner.params != null && Document_Scanner.params.ParentCtrl != null) {
            UnloadActionPan(Document_Scanner.params.ParentCtrl, 'Document_Scanner');
        }
        else
            UnloadActionPan(null, 'Document_Scanner');

        if (Patient_Insurance.params["PreDocument_ScanParams"])
            Document_Scan.params = Patient_Insurance.params["PreDocument_ScanParams"];

        reSetDefaultValuesForScanCanvas();
    },
}