HelpScreen = {
    params: [],
    Load: function (params) {
        HelpScreen.params = params;
        HelpScreen.GetPdfHelperDocument();

        //$.get('./Resources/HelpDocument/HelpPDFBase64.txt', function (resp) {
        //    utility.PDFViewer(resp, false, 'pnlHelpScreen #OpenDocumentIF');
        //});
        
    },
    UnLoad: function () {
        //UnloadActionPan(HelpScreen.params["ParentCtrl"], "HelpScreen");
        $('#containerHelpDocument').modal('hide');
        $('#containerHelpDocument').removeClass('modal fade')
        $('#containerHelpDocument').empty();
    },
    GetPdfHelperDocument: function () {
        HelpScreen.LoadHelperPDF().done(function (response) {
            if (response.status != false) {
                var base64String = response.pdfHelperBase64;
                utility.PDFViewer(base64String, false, 'pnlHelpScreen #OpenDocumentIF');           
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadHelperPDF: function () {
        var data = "";
        return MDVisionService.defaultService(data, "DashBoard", "MDVision_Training_Manual");
    }
}
