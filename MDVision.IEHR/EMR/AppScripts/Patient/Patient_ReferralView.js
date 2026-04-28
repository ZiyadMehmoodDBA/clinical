//Author: Humaira Yousaf
//Date: 23-03-2016
//This file will handle all actions performed to view PDF
Patient_ReferralsView = {
    bIsFirstLoad: true,
    params: [],
    pdf: "",

    Load: function (params) {
        Patient_ReferralsView.params = params;
        if (Patient_ReferralsView.params != null && Patient_ReferralsView.params.PanelID != "pnlPatient_ReferralView") {
            Patient_ReferralsView.params["PanelID"] = Patient_ReferralsView.params["PanelID"] + ' #pnlPatient_ReferralView';
        }
        else {
            Patient_ReferralsView.params = [];
            Patient_ReferralsView.params["PanelID"] = "pnlPatient_ReferralView"
        }

        if (Patient_ReferralsView.bIsFirstLoad) {
            Patient_ReferralsView.bIsFirstLoad = false;
            var self = $('#' + Patient_ReferralsView.params["PanelID"]);
            self.loadDropDowns(true).done(function () {

                Patient_ReferralsView.ReferralPreview(Patient_ReferralsView.params.PatientId, Patient_ReferralsView.params.UserId, Patient_ReferralsView.params.ReferralId);
            });
        }
    },
    //Function Name: ReferralPreview
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Creates PDF to view Consultation Order
    ReferralPreview: function (patientID, userID, ReferralId) {

        Patient_ReferralsView.previewReferral(patientID, userID, ReferralId).done(function (response) {
            try {
                response = JSON.parse(response);
                Patient_ReferralsView.pdf = response.ReferralHTML;
                //utility.PDFViewer(Patient_ReferralsView.pdf, false, 'pnlPatient_ReferralView #PreviewReferralForm', true);
                utility.documentPrint(response.ReferralHTML);
            }
            catch (ex) {
                console.log(ex);
            }

            return false;
            // utility.PDFViewer(response.ReferralHTML, false, 'pnlPatient_ReferralView #PreviewReferralForm', true);
        });
    },
    //Function Name: printConsultationOrder
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: Prints PDF
    printReferral: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referrals", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            strMessage = "";
            if (strMessage == "") {

                var raw = atob($('#helperPDF').val());

                var uint8Array = new Uint8Array(raw.length);
                for (var i = 0; i < raw.length; i++) {
                    uint8Array[i] = raw.charCodeAt(i);
                }
                var byteArray = new Uint8Array(uint8Array);

                var blob = new Blob([byteArray], { type: 'application/pdf' });
                var url = URL.createObjectURL(blob);
                //window.focus();
                var width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
                var height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;
                var myWindow = window.open(url, "MsgWindow", 'width=' + width + ', height=' + height);

                return true;
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    //Function Name: previewReferral
    //Author Name: Humaira Yousaf
    //Created Date: 23-03-2016
    //Description: DB call to view PDF
    previewReferral: function (patientID, userID, ReferralId) {

        var objData = {};
        objData["PatientId"] = patientID;
        objData["ReferralId"] = ReferralId;
        objData["commandType"] = "preview_referral";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PatientList", "IncomingOrOutgoingRefferal");
    },

    UnLoad: function () {

        var PanelID = '';
        if (Patient_ReferralsView.params.ParentCtrl == "clinicalTabFaceSheet") {

            params["ParentCtrl"] = 'Patient_Referrals';
            PanelID = 'pnlClinicalFaceSheet #pnlPatientReferrals';
        }
        else if (Patient_ReferralsView.params.ParentCtrl == "Patient_Referrals") {
            //Start 22-12-2016 Edit By Humaira Yousaf for EMR-2358
            params["ParentCtrl"] = 'Patient_Referrals';
            if (Patient_Referrals != null && Patient_Referrals.params.ParentCtrl != null && Patient_Referrals.params.ParentCtrl == "clinicalTabProgressNote") {
                PanelID = "pnlClinicalProgressNote #pnlPatientReferrals";
            }
            else {
                PanelID = 'pnlClinicalFaceSheet #pnlPatientReferrals';
            }
            //End 22-12-2016 Edit By Humaira Yousaf for EMR-2358
        }
        else {
            params["ParentCtrl"] = 'Patient_Referrals';
            PanelID = 'pnlPatientReferrals';
        }

        if (Patient_ReferralsView.params.ParentCtrl == "OrderSet_Patient_Referrals_Outgoing_Detail") {

            UnloadActionPan(Patient_ReferralsView.params.ParentCtrl);
        }
        else
            UnloadActionPan(Patient_ReferralsView.params.ParentCtrl, 'Patient_ReferralsView', null, PanelID);
    },
    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = "data:application/pdf;base64," + Patient_ReferralsView.pdf;
        params["ParentCtrl"] = "Patient_ReferralsView";
        LoadActionPan("Batch_FaxSend", params);
    }
}
