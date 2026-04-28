//Author: Humaira Yousaf
//Date: 02-05-2016
//This file will handle all actions performed to view PDF
Clinical_RadiologyResultView = {
    bIsFirstLoad: true,
    params: [],
    pdf: "",

    Load: function (params) {
        Clinical_RadiologyResultView.params = params;
        if (Clinical_RadiologyResultView.params != null && Clinical_RadiologyResultView.params.PanelID != "Clinical_RadiologyResultView") {
            Clinical_RadiologyResultView.params["PanelID"] = Clinical_RadiologyResultView.params["PanelID"] + ' #Clinical_RadiologyResultView';
        }
        else {
            Clinical_RadiologyResultView.params = [];
            Clinical_RadiologyResultView.params["PanelID"] = "Clinical_RadiologyResultView"
        }

        if (Clinical_RadiologyResultView.bIsFirstLoad) {
            Clinical_RadiologyResultView.bIsFirstLoad = false;
            var self = $('#' + Clinical_RadiologyResultView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                if (params["Caller"] != null && params["Caller"] == "viewpdf")
                    Clinical_RadiologyResultView.radiologyResultPdfPreview(Clinical_RadiologyResultView.params.PatientId, Clinical_RadiologyResultView.params.UserId, Clinical_RadiologyResultView.params.RadiologyOrderId, Clinical_RadiologyResultView.params.RadiologyResultId);                   
                else                   
                    Clinical_RadiologyResultView.RadiologyResultPreview(Clinical_RadiologyResultView.params.PatientId, Clinical_RadiologyResultView.params.RadiologyResultId, Clinical_RadiologyResultView.params.RadiologyOrderId);
            });
        }
    },
    //Function Name: RadiologyResultPreview
    //Author Name: Humaira Yousaf
    //Created Date: 02-05-2016
    //Description: Creates PDF to view Radiology Result
    RadiologyResultPreview: function (patientID, RadiologyResultId, RadiologyOrderId) {
        Clinical_RadiologyResultView.previewRadiologyResult(patientID, RadiologyResultId, RadiologyOrderId).done(function (response) {
            response = JSON.parse(response);
            Clinical_RadiologyResultView.pdf = response.RadiologyResultHTML;
            utility.PDFViewer(response.RadiologyResultHTML, false, 'Clinical_RadiologyResultView #PreviewRadiologyResultForm', true);
            if (Clinical_RadiologyOrder.params.ParentCtrl && Clinical_RadiologyOrder.params.ParentCtrl == 'clinicalTabProgressNote')
                Clinical_NotesView.SaveDiagnosticResultInPatDocs(response.RadiologyResultHTML, RadiologyResultId);
            
        });
    },
    //Function Name: printRadiologyResult
    //Author Name: Humaira Yousaf
    //Created Date: 02-05-2016
    //Description: Prints PDF
    printRadiologyResult: function () {
        AppPrivileges.GetFormPrivileges(" Face Sheet", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + Clinical_RadiologyResultView.params["PanelID"] + " #PreviewRadiologyResultForm")[0].contentWindow.focus();
                $("#" + Clinical_RadiologyResultView.params["PanelID"] + " #PreviewRadiologyResultForm")[0].contentWindow.print();

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Function Name: previewRadiologyResult
    //Author Name: Humaira Yousaf
    //Created Date: 02-05-2016
    //Description: DB call to view PDF
    previewRadiologyResult: function (patientID, RadiologyResultId, RadiologyOderId) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["RadiologyResultId"] = RadiologyResultId;
        objData["RadiologyOrderId"] = RadiologyOderId;
        objData["commandType"] = "preview_RadiologyResult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },


    UnLoad: function () {

        if (Clinical_RadiologyResultView.params["FromAdmin"] == "0") {
            if (Clinical_RadiologyResultView.params != null && Clinical_RadiologyResultView.params.ParentCtrl != null) {
                if (Clinical_RadiologyResultView.params.ParentCtrl == 'clinicalTabLabOrder') {
                    UnloadActionPan(Clinical_RadiologyResultView.params["ParentCtrl"], "Clinical_RadiologyResultView");
                } else {
                    Clinical_RadiologyResultView.params.PanelID = Clinical_RadiologyResultView.params.PanelID.replace(" #Clinical_RadiologyResultView", "");
                    UnloadActionPan(Clinical_RadiologyResultView.params.ParentCtrl, 'Clinical_RadiologyResultView', null, Clinical_RadiologyResultView.params.PrPanelID);
                }
            }
            else
                UnloadActionPan(null, 'Clinical_RadiologyResultView');
        }
        else {
            RemoveAdminTab();
        }
    },

    //Function Name: radiologyOrderResultPreview
    //Author Name: Humaira Yousaf
    //Created Date: 09-05-2016
    //Description: Views PDF

    radiologyResultPdfPreview: function (patientID, userID, radiologyOrderId, radiologyResultId) {
        Clinical_RadiologyResultView.previewRadiologyResultpdf(patientID, userID, radiologyOrderId, radiologyResultId).done(function (response) {
            response = JSON.parse(response);
            Clinical_RadiologyResultView.pdf = response.RadiologyOrderHTML;
            utility.PDFViewer(response.RadiologyOrderHTML, false, 'Clinical_RadiologyResultView #PreviewRadiologyResultForm', true);

        });
    },

    //Function Name: previewRadiologyResult
    //Author Name: Humaira Yousaf
    //Created Date: 09-05-2016
    //Description: Views PDF
    previewRadiologyResultpdf: function (patientID, userID, radiologyOrderId, radiologyResultId) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["RadiologyOrderId"] = radiologyOrderId;
        objData["RadiologyResultId"] = radiologyResultId;
        objData["commandType"] = "viewpdfradiologyresult";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "RadiologyResult", "RadiologyResult");
    },

    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = "data:application/pdf;base64," + Clinical_RadiologyResultView.pdf;
        params["ParentCtrl"] = "Clinical_RadiologyResultView";
        LoadActionPan("Batch_FaxSend", params);
    }
}
