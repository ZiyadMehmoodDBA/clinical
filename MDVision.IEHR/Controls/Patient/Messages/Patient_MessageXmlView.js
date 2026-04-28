Patient_MessageXmlView = {
    bIsFirstLoad: true,
    params: [],
    TemplateContent: "",

    Load: function (params) {
        Patient_MessageXmlView.params = params;
        
        Patient_MessageXmlView.SetIframePath(Patient_MessageXmlView.params['filePath']);
        //Patient_MessageCreate.Documentready();
    },

    UnLoad: function () {

        if (Patient_MessageXmlView.params != null && Patient_MessageXmlView.params.ParentCtrl) {
            UnloadActionPan(Patient_MessageXmlView.params.ParentCtrl);
            Patient_MessageXmlView.params = null;
        }
        else {
            UnloadActionPan();
        }
    },

    SetIframePath: function (url) {
        $("#pnlPatientMessageXmlView #IframXML").attr('src', url)
    },

 
}