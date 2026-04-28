Batch_FaxView = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Batch_FaxView.params = params;
        if (Batch_FaxView.params != null && Batch_FaxView.params.PanelID != "Batch_FaxView") {
            Batch_FaxView.params["PanelID"] = Batch_FaxView.params["PanelID"] + ' #Batch_FaxView';
        }
        else {
            Batch_FaxView.params = [];
            Batch_FaxView.params["PanelID"] = "Batch_FaxView"
        }


        if ((Batch_FaxView.params.ParentCtrl && Batch_FaxView.params.ParentCtrl == "Patient_Document") || (Batch_FaxView.params.TabID == "batchTabDocuments"
            || Batch_FaxView.params.TabID == "patTabDocuments")
            ) {
            $("#" + Batch_FaxView.params.PanelID + " .modal-title").html("");
            $("#" + Batch_FaxView.params.PanelID + " .modal-title").html("Preview Document");
        }
        Batch_FaxView.FaxPreview(Batch_FaxView.params.FaxHtml);


    },
    FaxPreview: function (laborderhtml) {
        //  laborderhtml = laborderhtml.substring(0, (laborderhtml.length - 6));

        // laborderhtml = laborderhtml.substring(28, laborderhtml.length); // for fax send
        utility.PDFViewer(laborderhtml, false, 'Batch_FaxView #PreviewLabOrderForm', true);


    },

    printFax: function () {
        $("#Batch_FaxView #PreviewLabOrderForm")[0].contentWindow.focus();
        $("#Batch_FaxView #PreviewLabOrderForm")[0].contentWindow.print();
    },
    UnLoad: function () {
        if (Patient_Document.params["ParentCtrl"] == "demographicDetail") {
            var parentPanelId = GetTab(Patient_Document.params["ParentCtrl"]).PanelID;
            UnloadActionPan(Batch_FaxView.params["ParentCtrl"], 'Batch_FaxView', null, parentPanelId);
            delete Batch_FaxView.UnloadParent;
        } else {
            if (Batch_FaxView.UnloadParent && Batch_FaxView.UnloadParent == 'ParentUnload') {
                var parentPanelId = GetTab(Batch_FaxView.params["ParentCtrl"]).PanelID;
                UnloadActionPan(Batch_FaxView.params["ParentCtrl"], 'Batch_FaxView', null, parentPanelId);
                delete Batch_FaxView.UnloadParent;
            }

            else if (Batch_FaxView.params != null && Batch_FaxView.params.ParentCtrl) {
                UnloadActionPan(Batch_FaxView.params.ParentCtrl);
            }
            else {
                UnloadActionPan();
            }
        }
    }
}