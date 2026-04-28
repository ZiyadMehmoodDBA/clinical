//Author: M Ahmad Imran
//Date: 23-08-2016
//This file will handle all actions performed to view PDF
Immunization_AlertPrint = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Immunization_AlertPrint.params = params;
        if (Immunization_AlertPrint.params != null && Immunization_AlertPrint.params.PanelID != "pnlIImmunization_AlertPrint") {
            Immunization_AlertPrint.params["PanelID"] = Immunization_AlertPrint.params["PanelID"] + ' #pnlIImmunization_AlertPrint';
        }
        else {
            Immunization_AlertPrint.params = [];
            Immunization_AlertPrint.params["PanelID"] = "pnlIImmunization_AlertPrint"
        }

        if (Immunization_AlertPrint.bIsFirstLoad) {
            Immunization_AlertPrint.bIsFirstLoad = false;
            var self = $('#' + Immunization_AlertPrint.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                Immunization_AlertPrint.GetImmunizationAlertForPrint(Immunization_AlertPrint.params.PatientId);
            });
        }
    },
    //Function Name: GetImmunizationAlertForPrint
    //Author Name: M Ahmad Imran
    //Created Date: 23-08-2016
    //Description: Creates PDF to view Consultation Order
    GetImmunizationAlertForPrint: function (patientID, userID, ReferralId) {
        Immunization_AlertPrint.GetImmunizationAlertForPrint_DB_CALL(patientID).done(function (response) {
            response = JSON.parse(response);
            utility.PDFViewer(response.ImmunizationAlertHTML, false, 'pnlIImmunization_AlertPrint #PreviewImmunizationAlertForm', true);
        });
    },
    //Function Name: printConsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Prints PDF
    printImmunizationAlert: function () {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Referrals", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + Immunization_AlertPrint.params["PanelID"] + " #PreviewImmunizationAlertForm")[0].contentWindow.focus();
                $("#" + Immunization_AlertPrint.params["PanelID"] + " #PreviewImmunizationAlertForm")[0].contentWindow.print();
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        //});
    },
    //Function Name: previewReferral
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: DB call to view PDF
    GetImmunizationAlertForPrint_DB_CALL: function (patientID) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["commandType"] = "GetImmunizationAlertForPrint";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Immunization");
    },

    UnLoad: function () {
        if (Immunization_AlertPrint.params["FromAdmin"] == "0") {
            if (Immunization_AlertPrint.params != null && Immunization_AlertPrint.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_AlertPrint.params.ParentCtrl, 'Immunization_AlertPrint');
            } else
                UnloadActionPan(null, 'Immunization_AlertPrint');
        } else {
            RemoveAdminTab();
        }
    }
}
