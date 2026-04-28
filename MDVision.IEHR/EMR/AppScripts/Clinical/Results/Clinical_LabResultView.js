//Author: Humaira Yousaf
//Date: 25-04-2016
//This file will handle all actions performed to view PDF
Clinical_LabResultView = {
    bIsFirstLoad: true,
    params: [],
    faxPDF: "",

    Load: function (params) {
        Clinical_LabResultView.params = params;
        if (Clinical_LabResultView.params != null && Clinical_LabResultView.params.PanelID != "Clinical_LabResultView") {
            Clinical_LabResultView.params["PanelID"] = Clinical_LabResultView.params["PanelID"] + ' #Clinical_LabResultView';
        }
        else {
            Clinical_LabResultView.params = [];
            Clinical_LabResultView.params["PanelID"] = "Clinical_LabResultView"
        }

        if (Clinical_LabResultView.bIsFirstLoad) {
            Clinical_LabResultView.bIsFirstLoad = false;
            var self = $('#' + Clinical_LabResultView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                if (params["Caller"] != null && params["Caller"] == "viewpdf")
                {
                    //Start 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1022
                    Clinical_LabResultView.labResultPDFPreview(Clinical_LabResultView.params.PatientId, Clinical_LabResultView.params.UserId, Clinical_LabResultView.params.LabOrderId, Clinical_LabResultView.params.LabResultId);
                    //End 11-05-2016 Edit By Humaira Yousaf Bug# EMR-1022
                }

                else if (Clinical_LabResultView.params["IsExternalPDF"])
                {
                    Clinical_LabResultView.labResultExternalPDFView(Clinical_LabResultView.params.LabOrderResultExternalPDFId);
                }
                else {
                 Clinical_LabResultView.LabResultPreview(Clinical_LabResultView.params.PatientId, Clinical_LabResultView.params.LabResultId, Clinical_LabResultView.params.LabOrderId);  
               }
               
            });
        }
    },
    //Function Name: LabResultPreview
    //Author Name: Humaira Yousaf
    //Created Date: 25-04-2016
    //Description: Creates PDF to view Lab Result
    LabResultPreview: function (patientID, labResultId, labOrderId) {
        Clinical_LabResultView.previewLabResult(patientID, labResultId, labOrderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                Clinical_LabResultView.faxPDF = response.LabResultHTML;
                utility.PDFViewer(response.LabResultHTML, false, 'Clinical_LabResultView #PreviewLabResultForm', true);
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
            
        });
    },
    labResultExternalPDFView: function (LabOrderResultExternalPDFId) {
        Clinical_LabResultView.previewLabResultExternalPDF(LabOrderResultExternalPDFId).done(function (response) {
            response = JSON.parse(response);
            Clinical_LabResultView.faxPDF = response.PDFData.FileBase64;
            utility.PDFViewer(response.PDFData.FileBase64, false, 'Clinical_LabResultView #PreviewLabResultForm', true);
        });
    },

    //Function Name: printLabResult
    //Author Name: Humaira Yousaf
    //Created Date: 25-04-2016
    //Description: Prints PDF
    printLabResult: function () {
        AppPrivileges.GetFormPrivileges(" Face Sheet", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + Clinical_LabResultView.params["PanelID"] + " #PreviewLabResultForm")[0].contentWindow.focus();
                $("#" + Clinical_LabResultView.params["PanelID"] + " #PreviewLabResultForm")[0].contentWindow.print();

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
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
        objData["BarCodeHtml"] = Clinical_LabResultView.params.BarCodeHtml;
        objData["commandType"] = "preview_LabResult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabResult", "LabResult");
    },
    UnLoad: function () {

        //if (Clinical_LabResultView.params != null && Clinical_LabResultView.params.ParentCtrl) {
        //    UnloadActionPan(Clinical_LabResultView.params.ParentCtrl);
        //}
        //else {
        //    UnloadActionPan();
        //}

        //if (Clinical_LabResultView.params["FromAdmin"] == "0") {
        //    if (Clinical_LabResultView.params != null && Clinical_LabResultView.params.ParentCtrl != null) {
        //        UnloadActionPan(Clinical_LabResultView.params.ParentCtrl, 'Clinical_LabResultView', null, Clinical_LabResultView.params.PanelID);
        //    }
        //    else
        //        UnloadActionPan(null, 'Clinical_LabResultView');
        //}
        //else {
        //    RemoveAdminTab();
        //}


        if (Clinical_LabResultView.params["FromAdmin"] == "0") {
            if (Clinical_LabResultView.params != null && Clinical_LabResultView.params.ParentCtrl != null) {
                if (Clinical_LabResultView.params.ParentCtrl == 'clinicalTabLabOrder') {
                    UnloadActionPan(Clinical_LabResultView.params["ParentCtrl"], "Clinical_LabResultView");
                } else {
                    Clinical_LabResultView.params.PanelID = Clinical_LabResultView.params.PanelID.replace(" #Clinical_LabResultView", "");
                    UnloadActionPan(Clinical_LabResultView.params.ParentCtrl, 'Clinical_LabResultView', null, Clinical_LabResultView.params.PrPanelID);
                }
            }
            else
                UnloadActionPan(null, 'Clinical_LabResultView');
        }
        else {
            RemoveAdminTab();
        }
    },
    //Function Name: labOrderResultPreview
    //Author Name: Humaira Yousaf
    //Created Date: 06-05-2016
    //Description: View PDF
    labResultPDFPreview: function (patientID, userID, labOrderId, labResultId) {
        Clinical_LabResultView.previewLabResultpdf(patientID, userID, labOrderId, labResultId).done(function (response) {
            response = JSON.parse(response);
            Clinical_LabResultView.faxPDF = response.LabResultHTML
            utility.PDFViewer(response.LabOrderHTML, false, 'Clinical_LabResultView #PreviewLabResultForm', true);

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
        params["PDFBase64"] = "data:application/pdf;base64," + Clinical_LabResultView.faxPDF;
        params["ParentCtrl"] = "Clinical_LabResultView";
        LoadActionPan("Batch_FaxSend", params);
        
    }
}
