//Author: Humaira Yousaf
//Date: 23-03-2016
//This file will handle all actions performed to view PDF
Clinical_ConsultationOrderView = {
    bIsFirstLoad: true,
    params: [],
    pdf: "",

    Load: function (params) {
        Clinical_ConsultationOrderView.params = params;
        if (Clinical_ConsultationOrderView.params != null && Clinical_ConsultationOrderView.params.PanelID != "Clinical_ConsultationOrderView") {
            Clinical_ConsultationOrderView.params["PanelID"] = Clinical_ConsultationOrderView.params["PanelID"] + ' #Clinical_ConsultationOrderView';
        }
        else {
            Clinical_ConsultationOrderView.params = [];
            Clinical_ConsultationOrderView.params["PanelID"] = "Clinical_ConsultationOrderView"
        }

        if (Clinical_ConsultationOrderView.bIsFirstLoad) {
            Clinical_ConsultationOrderView.bIsFirstLoad = false;
            var self = $('#' + Clinical_ConsultationOrderView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                Clinical_ConsultationOrderView.consultationOrderPreview(Clinical_ConsultationOrderView.params.PatientId, Clinical_ConsultationOrderView.params.UserId, Clinical_ConsultationOrderView.params.ConsultationOrderId);
            });
        }
    },
    //Function Name: consultationOrderPreview
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Creates PDF to view Consultation Order
    consultationOrderPreview: function (patientID, userID, consultationOrderId) {
        Clinical_ConsultationOrderView.previewConsultationOrder(patientID, userID, consultationOrderId).done(function (response) {
            response = JSON.parse(response);
            Clinical_ConsultationOrderView.pdf = response.ConsultationOrderHTML;
            utility.PDFViewer(response.ConsultationOrderHTML, false, 'Clinical_ConsultationOrderView #PreviewConsultationOrderForm', true);
        });
    },
    //Function Name: printConsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Prints PDF
    printConsultationOrder: function () {
        AppPrivileges.GetFormPrivileges(" Face Sheet", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + Clinical_ConsultationOrderView.params["PanelID"] + " #PreviewConsultationOrderForm")[0].contentWindow.focus();
                $("#" + Clinical_ConsultationOrderView.params["PanelID"] + " #PreviewConsultationOrderForm")[0].contentWindow.print();
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    //Function Name: previewConsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: DB call to view PDF
    previewConsultationOrder: function (patientID, userID, consultationOrderId) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["ConsultationOrderId"] = consultationOrderId;
        objData["commandType"] = "preview_consultationorder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ConsultationOrder", "ConsultationOrder");
    },

    UnLoad: function () {

        if (Clinical_ConsultationOrderView.params != null && Clinical_ConsultationOrderView.params.ParentCtrl) {
            if (Clinical_ConsultationOrderView.params.ParentCtrlPanelID != "undefined" && Clinical_ConsultationOrderView.params.ParentCtrlPanelID !="")
                UnloadActionPan(Clinical_ConsultationOrderView.params.ParentCtrl, null, null, Clinical_ConsultationOrderView.params.ParentCtrlPanelID);
            else
                UnloadActionPan(Clinical_ConsultationOrderView.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    },
    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = "data:application/pdf;base64," + Clinical_ConsultationOrderView.pdf;
        params["ParentCtrl"] = "Clinical_ConsultationOrderView";
        LoadActionPan("Batch_FaxSend", params);
    }
}
