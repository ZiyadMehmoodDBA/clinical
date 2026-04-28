Patient_MedText_Referrals = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Patient_MedText_Referrals.params = params;
        Patient_MedText_Referrals.LoadMedText();
    },

    LoadMedText: function () {

        $("#pnlPatientMedTextReferrals #IFMedtext").attr('src', Patient_MedText_Referrals.params["MedTextUrl"]);
    },

    UnLoadTab: function () {

        if (Patient_MedText_Referrals.params != null && Patient_MedText_Referrals.params.ParentCtrl != null) {
            UnloadActionPan(Patient_MedText_Referrals.params.ParentCtrl, 'Patient_MedText_Referrals');
        }
        else
            UnloadActionPan(null, 'Patient_MedText_Referrals');

        if (Patient_MedText_Referrals.params.ParentCtrl == "Patient_Referrals_Outgoing_Detail")
            Patient_Referrals_Outgoing_Detail.UnLoadTab();
    },
}