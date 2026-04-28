CCMConsent = {
    bIsFirstLoad: true,
    params: [],
    pdf: "",
    Load: function (params) {
        CCMConsent.params = params;
        if (CCMConsent.params != null && CCMConsent.params.PanelID != "CCMConsent") {
            CCMConsent.params["PanelID"] = CCMConsent.params["PanelID"] + ' #CCMConsent';
        }
        else {
            CCMConsent.params = [];
            CCMConsent.params["PanelID"] = "CCMConsent"
        }

        if (CCMConsent.bIsFirstLoad) {
            CCMConsent.bIsFirstLoad = false;
            utility.PDFViewer(CCMConsent.params.ConsentFileStream, false, 'CCMConsent #PreviewConsentForm', true);
        }
    },

    UnLoad: function ()
    {
        UnloadActionPan(CCMConsent.params.ParentCtrl, 'CCMConsent');
    },

}
