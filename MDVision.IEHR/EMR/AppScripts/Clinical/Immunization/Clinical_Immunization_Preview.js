//Author: M Ahmad Imran
//Date: 15-8-2016
//This file will handle all actions performed to view PDF
Clinical_Immunization_Preview = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Clinical_Immunization_Preview.params = params;
        if (Clinical_Immunization_Preview.params != null && Clinical_Immunization_Preview.params.PanelID != "pnlClinical_Immunization_Preview") {
            Clinical_Immunization_Preview.params["PanelID"] = Clinical_Immunization_Preview.params["PanelID"] + ' #pnlClinical_Immunization_Preview';
        }
        else {
            
            Clinical_Immunization_Preview.params["PanelID"] = "pnlClinical_Immunization_Preview"
        }

        if (Clinical_Immunization_Preview.bIsFirstLoad) {
            Clinical_Immunization_Preview.bIsFirstLoad = false;
            var self = $('#' + Clinical_Immunization_Preview.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                Clinical_Immunization_Preview.ImmunizationPreview(Clinical_Immunization_Preview.params.PatientId, Clinical_Immunization_Preview.params.UserId, Clinical_Immunization_Preview.params.ImmunizationIds);
            });
        }
    },
    //Function Name: ReferralPreview
    //Author Name: M Ahmad Imran
    //Created Date: 15-08-2016
    //Description: Creates PDF to view Immunization
    ImmunizationPreview: function (patientID, userID, ImmunizationIds) {
        Clinical_Immunization_Preview.previewImmunization(patientID, userID, ImmunizationIds).done(function (response) {
            response = JSON.parse(response);
            utility.PDFViewer(response.ImmunizationHTML, false, 'pnlClinical_Immunization_Preview #Clinical_Immunization_PreviewDiv', true);
        });
    },
    //Function Name: printImmunization
    //Author Name: M Ahmad Imran
    //Created Date: 15-08-2016
    //Description: Prints PDF
    printImmunization: function () {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Referrals", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                $("#" + Clinical_Immunization_Preview.params["PanelID"] + " #Clinical_Immunization_PreviewDiv")[0].contentWindow.focus();
                $("#" + Clinical_Immunization_Preview.params["PanelID"] + " #Clinical_Immunization_PreviewDiv")[0].contentWindow.print();
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        //});
    },
    //Function Name: previewReferral
    //Author Name: M Ahmad Imran
    //Created Date: 15-08-2016
    //Description: DB call to view PDF
    previewImmunization: function (patientID, userID, ImmunizationIds) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["commandType"] = "preview_Immunization";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Immunization");
    },

    UnLoad: function () {

        if (Clinical_Immunization_Preview.params != null && Clinical_Immunization_Preview.params.ParentCtrl) {

            if (Clinical_Immunization_Preview.params.ParentCtrl == 'clinicalTabImmunization') {
                UnloadActionPan(Clinical_Immunization_Preview.params["ParentCtrl"], "Clinical_Immunization_Preview");
            } else {
                Clinical_Immunization_Preview.params.PanelID = Clinical_Immunization_Preview.params.PanelID.replace(" #pnlClinical_Immunization_Preview", "");
                UnloadActionPan(Clinical_Immunization_Preview.params.ParentCtrl, 'Clinical_Immunization_Preview', null, Clinical_Immunization_Preview.params.PanelID);
            }
        }
        else {
            UnloadActionPan();
        }
    }
}
