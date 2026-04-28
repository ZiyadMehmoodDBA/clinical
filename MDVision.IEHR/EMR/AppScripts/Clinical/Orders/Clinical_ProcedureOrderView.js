//Author: Humaira Yousaf
//Date: 22-03-2016
//This file will handle all actions performed to view PDF
Clinical_ProcedureOrderView = {
    bIsFirstLoad: true,
    params: [],
    pdf: "",

    Load: function (params) {
        Clinical_ProcedureOrderView.params = params;
        if (Clinical_ProcedureOrderView.params != null && Clinical_ProcedureOrderView.params.PanelID != "Clinical_ProcedureOrderView") {
            Clinical_ProcedureOrderView.params["PanelID"] = Clinical_ProcedureOrderView.params["PanelID"] + ' #Clinical_ProcedureOrderView';
        }
        else {
            Clinical_ProcedureOrderView.params = [];
            Clinical_ProcedureOrderView.params["PanelID"] = "Clinical_ProcedureOrderView"
        }

        if (Clinical_ProcedureOrderView.bIsFirstLoad) {
            Clinical_ProcedureOrderView.bIsFirstLoad = false;
            var self = $('#' + Clinical_ProcedureOrderView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                Clinical_ProcedureOrderView.procedureOrderPreview(Clinical_ProcedureOrderView.params.PatientId, Clinical_ProcedureOrderView.params.UserId, Clinical_ProcedureOrderView.params.ProcedureOrderId);
            });
        }
    },
    //Function Name: procedureOrderPreview
    //Author Name: Humaira Yousaf
    //Created Date: 22-03-2016
    //Description: Creates PDF to view Procedure Order
    procedureOrderPreview: function (patientID, userID, procedureOrderId) {
        Clinical_ProcedureOrderView.previewProcedureOrder(patientID, userID, procedureOrderId).done(function (response) {
            response = JSON.parse(response);
            Clinical_ProcedureOrderView.pdf = response.ProcedureOrderHTML;
            utility.PDFViewer(response.ProcedureOrderHTML, false, 'Clinical_ProcedureOrderView #PreviewProcedureOrderForm', true);

        });
    },
    //Function Name: printProcedureOrder
    //Author Name: Humaira Yousaf
    //Created Date: 22-03-2016
    //Description: Prints PDF
    printProcedureOrder: function () {
        AppPrivileges.GetFormPrivileges(" Face Sheet", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
              
                $("#" + Clinical_ProcedureOrderView.params["PanelID"] + " #PreviewProcedureOrderForm")[0].contentWindow.focus();
                $("#" + Clinical_ProcedureOrderView.params["PanelID"] + " #PreviewProcedureOrderForm")[0].contentWindow.print();
             
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Function Name: previewProcedureOrder
    //Author Name: Humaira Yousaf
    //Created Date: 22-03-2016
    //Description: DB call to view PDF
    previewProcedureOrder: function (patientID, userID, procedureOrderId) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["ProcedureOrderId"] = procedureOrderId;
        objData["commandType"] = "preview_procedureOrder";
        var data = JSON.stringify(objData);      
        return MDVisionService.APIService(data, "ProcedureOrder", "ProcedureOrder");
    },

    UnLoad: function () {

        if (Clinical_ProcedureOrderView.params != null && Clinical_ProcedureOrderView.params.ParentCtrl) {
            UnloadActionPan(Clinical_ProcedureOrderView.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    }
    ,
    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = "data:application/pdf;base64," + Clinical_ProcedureOrderView.pdf;
        params["ParentCtrl"] = "Clinical_ProcedureOrderView";
        LoadActionPan("Batch_FaxSend", params);
    }
}
