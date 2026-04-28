NoteTemplatePreview = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        var self = $('#frmPreviewNoteTempPrint');
        var data = "IsActive=&ID=-1&ID2=" + $("#pnlClinicalNotes #hfProvider").val();
        self.loadDropDowns(true, data).done(function () {
            self.find('.NoteTemplate > select').val(params.TemplateId);
        });
        Clinical_Notes.TemplateDetailFromDB(params.TemplateId).done(function (response) {
            response = JSON.parse(response);
            $("#NoteTemplatePreview #NoteHTMLBody").html(response.Message);
            NoteTemplatePreview.LoadPDFViewNotePreview();
        });

    },
    UnLoad: function () {
        UnloadActionPan(null, "NoteTemplatePreview");
    },
    LoadTemplateData: function (ThisCTRL) {
        if ($(ThisCTRL).val() != "" && $(ThisCTRL).val() > 0) {
            Clinical_Notes.TemplateDetailFromDB($(ThisCTRL).val()).done(function (response) {
                response = JSON.parse(response);
                $("#NoteTemplatePreview #NoteHTMLBody").html(response.Message);
                NoteTemplatePreview.LoadPDFViewNotePreview();
            });
        }
    },

    LoadPDFViewNotePreview: function () {
        $("#NoteTemplatePreview #NoteHTMLBody").removeClass("hide");
        var def = $.Deferred();
        kendo.drawing.drawDOM("#NoteTemplatePreview #NoteHTMLBody", {
            landscape: false,
            scale: 0.8,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            }
        }).then(function (group) {
            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');

                utility.PDFViewer(dataURL.split('data:application/pdf;base64,').join(''), false, 'frmPreviewNoteTempPrint #PreviewNoteTempPrint', true);
                def.resolve('ok');
            });
        });
        def.then(function () {
            $("#NoteTemplatePreview #NoteHTMLBody").addClass("hide");
        });
    }
}