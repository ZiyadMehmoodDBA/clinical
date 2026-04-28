
Clinical_LabResultTrendsView = {
    bIsFirstLoad: true,
    params: [],
    faxPDF: "",

    Load: function (params) {
        Clinical_LabResultTrendsView.params = params;
        if (Clinical_LabResultTrendsView.params != null && Clinical_LabResultTrendsView.params.PanelID != "Clinical_LabResultTrendsView") {
            Clinical_LabResultTrendsView.params["PanelID"] = Clinical_LabResultTrendsView.params["PanelID"] + ' #Clinical_LabResultTrendsView';
        }
        else {
            Clinical_LabResultTrendsView.params = [];
            Clinical_LabResultTrendsView.params["PanelID"] = "Clinical_LabResultTrendsView"
        }
        
  
        if (Clinical_LabResultTrendsView.bIsFirstLoad) {
            Clinical_LabResultTrendsView.bIsFirstLoad = false;
            var self = $('#' + Clinical_LabResultTrendsView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

            

            });
        }
    },
    printScreen: function (TrendBase64) {
        utility.PDFViewer((TrendBase64.split(',')[1]), false, 'Clinical_LabResultTrendsView #PreviewLabResultForm', true);
    },
    //Function Name: LabResultPreview
    //Author Name: Humaira Yousaf
    //Created Date: 25-04-2016
    //Description: Creates PDF to view Lab Result
    LabResultPreview: function (patientID, labResultId, labOrderId) {
        Clinical_LabResultTrendsView.previewLabResult(patientID, labResultId, labOrderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                Clinical_LabResultTrendsView.faxPDF = response.LabResultHTML;
                utility.PDFViewer(response.LabResultHTML, false, 'Clinical_LabResultTrendsView #PreviewLabResultForm', true);
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }

        });
    },
    labResultExternalPDFView: function (LabOrderResultExternalPDFId) {
        Clinical_LabResultTrendsView.previewLabResultExternalPDF(LabOrderResultExternalPDFId).done(function (response) {
            response = JSON.parse(response);
            Clinical_LabResultTrendsView.faxPDF = response.PDFData.FileBase64;
            utility.PDFViewer(response.PDFData.FileBase64, false, 'Clinical_LabResultTrendsView #PreviewLabResultForm', true);
        });
    },

   
    printLabResult: function () {
 
                $("#" + Clinical_LabResultTrendsView.params["PanelID"] + " #PreviewLabResultForm")[0].contentWindow.focus();
                $("#" + Clinical_LabResultTrendsView.params["PanelID"] + " #PreviewLabResultForm")[0].contentWindow.print();

        
    },
    previewLabResultExternalPDF: function (labOrderResultExternalPDFId) {
        var objData = {};
        objData["LabOrderResultExternalPDFId"] = labOrderResultExternalPDFId;
        objData["commandType"] = "preview_LabResultExternalPDF";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },

    previewLabResult: function (patientID, labResultId, labOderId) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["LabResultId"] = labResultId;
        objData["LabOrderId"] = labOderId;
        objData["BarCodeHtml"] = Clinical_LabResultTrendsView.params.BarCodeHtml;
        objData["commandType"] = "preview_LabResult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    UnLoad: function () {

        //if (Clinical_LabResultTrendsView.params != null && Clinical_LabResultTrendsView.params.ParentCtrl) {
        //    UnloadActionPan(Clinical_LabResultTrendsView.params.ParentCtrl);
        //}
        //else {
        //    UnloadActionPan();
        //}

        //if (Clinical_LabResultTrendsView.params["FromAdmin"] == "0") {
        //    if (Clinical_LabResultTrendsView.params != null && Clinical_LabResultTrendsView.params.ParentCtrl != null) {
        //        UnloadActionPan(Clinical_LabResultTrendsView.params.ParentCtrl, 'Clinical_LabResultTrendsView', null, Clinical_LabResultTrendsView.params.PanelID);
        //    }
        //    else
        //        UnloadActionPan(null, 'Clinical_LabResultTrendsView');
        //}
        //else {
        //    RemoveAdminTab();
        //}


          if (Clinical_LabResultTrendsView.params != null && Clinical_LabResultTrendsView.params.ParentCtrl != null) {
                if (Clinical_LabResultTrendsView.params.ParentCtrl == 'clinicalTabLabOrder') {
                    UnloadActionPan(Clinical_LabResultTrendsView.params["ParentCtrl"], "Clinical_LabResultTrendsView");
                } else {
                    Clinical_LabResultTrendsView.params.PanelID = Clinical_LabResultTrendsView.params.PanelID.replace(" #Clinical_LabResultTrendsView", "");
                    UnloadActionPan(Clinical_LabResultTrendsView.params.ParentCtrl);
                }
            }
            else
                UnloadActionPan(null, 'Clinical_LabResultTrendsView');
    
    },
    //Function Name: labOrderResultPreview
    //Author Name: Humaira Yousaf
    //Created Date: 06-05-2016
    //Description: View PDF
    labResultPDFPreview: function (patientID, userID, labOrderId, labResultId) {
        Clinical_LabResultTrendsView.previewLabResultpdf(patientID, userID, labOrderId, labResultId).done(function (response) {
            response = JSON.parse(response);
            Clinical_LabResultTrendsView.faxPDF = response.LabResultHTML
            utility.PDFViewer(response.LabOrderHTML, false, 'Clinical_LabResultTrendsView #PreviewLabResultForm', true);

        });
    },
    //Function Name: previewLabResult
    //Author Name: Humaira Yousaf
    //Created Date: 06-05-2016
    //Description: View PDF
    previewLabResultpdf: function (patientID, userID, labOrderId, labresultId) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["LabOrderId"] = labOrderId;
        objData["LabResultId"] = labresultId;
        objData["commandType"] = "viewpdflabresult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = Clinical_LabResultTrendsView.faxPDF;
        params["ParentCtrl"] = "Clinical_LabResultTrendsView";
        LoadActionPan("Batch_FaxSend", params);
    },
}
