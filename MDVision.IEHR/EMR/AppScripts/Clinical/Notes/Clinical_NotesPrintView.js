NotesPrintView = {
    params: [],
    bIsFirstLoad: true,
    Load: function (params) {

    },
    PrintReport: function () {

        Clinical_NotesView.PreviewNotes(Clinical_NotesView.params.NotesId).done(function (response) {
            $("#" + NotesPrintView.params["PanelID"] + " #NotesPrintView #NotePrintTitle").html("Notes");
            if (response.status == true) {

                var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);
                if (NotesLoad_JSON[0].NoteText != '' && NotesLoad_JSON[0].NoteText != null) {
                    HTMLNotes = NotesLoad_JSON[0].NoteText;
                    HTMLNotes = HTMLNotes.replace(/&quot;/g, '"');
                    HTMLNotes = HTMLNotes.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                }
            }

            $("#" + NotesPrintView.params["PanelID"] + " #NotesPrintView  #reportTable").removeClass("Of-a");
            $("#" + NotesPrintView.params["PanelID"] + " #NotesPrintView #printMe").hide();
            setTimeout(function () {
                var contents = HTMLNotes;
                var frame1 = $('<iframe />');
                frame1[0].name = "Notes";
                frame1.attr("scrolling", "no");
                frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden" });
                $("body").append(frame1);
                var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
                frameDoc.document.open();
                //Create a new HTML document.
                frameDoc.document.write('<html><head><title> Notes </title>');
                frameDoc.document.write('</head><body>');
                //Append the external CSS file.
                frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Default/print-media.css" />');
                //Append the DIV contents.
                frameDoc.document.write(contents);
                frameDoc.document.write('</body></html>');
                frameDoc.document.close();

                setTimeout(function () {
                    window.frames["Notes"].focus();
                    window.frames["Notes"].print();
                    frame1.remove();
                    $("#" + NotesPrintView.params["PanelID"] + " #NotesPrintView  #reportTable").addClass("Of-a");
                    $("#" + NotesPrintView.params["PanelID"] + " #NotesPrintView #printMe").show();
                }, 200);

            }, 100);

        });
    },



    PrintReports: function () {

        Clinical_NotesView.PreviewNotes(Clinical_NotesView.params.NotesId).done(function (response) {
            utility.PDFViewer(response.NotesHTML, false, 'NotesPrintView #PreviewClaimFormIF');

        });
    },
    UnLoad: function (Tab) {
        UnloadActionPan(NotesPrintView.params["ParentCtrl"]);
    }

}