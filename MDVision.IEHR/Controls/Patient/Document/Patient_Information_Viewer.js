Patient_Information_Viewer = {
    params: [],
    scale: 1.0,
    needCanvasReset: false,
    interval: null,

    Load: function (params) {
        Patient_Information_Viewer.params = params;
        var self = $('#' + params.PanelID + ' #pnlPatientInformationViewer');
        Patient_Information_Viewer.DocumentFill();
    },

    ZoomIn: function () {
        var canvas = $("#canvas");
        canvas.css("width", (canvas.width() * 2) + "px");
        //document_Viewer.drawCanvas(2);
    },

    ZoomOut: function () {
        var canvas = $("#canvas");
        canvas.css("width", (canvas.width() / 2) + "px");
        //Patient_Information_Viewer.drawCanvas(0.5);
    },

    drawCanvas: function (scale) {
        var canvas = document.getElementById('canvas');
        var context = canvas.getContext('2d');
        if (Patient_Information_Viewer.needCanvasReset) {
            context.setTransform(1, 0, 0, 1, 0.5, 0.5);
            Patient_Information_Viewer.needCanvasReset = false;
        }
        context.scale(scale, scale);
        context.beginPath();
        context.restore();
    },

    navigateCanvas: function (navigation) {
        var canvas = document.getElementById("canvas");
        var context = canvas.getContext("2d");
        needCanvasReset = true;
        switch (navigation) {
            case "up":
                context.translate(0, -10);
                break;
            case "down":
                context.translate(0, 10);
                break;
            case "left":
                context.translate(-10, 0);
                break;
            case "right":
                context.translate(10, 0);
                break;
            case "reset":
                $("#canvas").removeAttr("style");
                //context.setTransform(1, 0, 0, 1, 0.5, 0.5);
                break;
        }
    },

    FillDocument: function (Url, FileType) {
        var data = "Url=" + Url + "&FileType=" + FileType;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "FILL_PATIENT_INFORMATION_VIEWER");
    },

    DocumentFill: function () {

        var self = $('#' + Patient_Information_Viewer.params.PanelID + ' #CtrDocumentViewer');
        var documentCall = Patient_Information_Viewer.FillDocument(Patient_Information_Viewer.params.Url);
        $.when(documentCall).done(function (response) {
            // documentCall.done(function (response) {
            if (response.status != false) {
                var document_details = JSON.parse(response.DocumentLoad_JSON);
                var FileType = Patient_Information_Viewer.params.FileType;

                if (FileType.indexOf("pdf") > -1) {
                    $('#progressnotehtmldoc').remove();
                    $('#pnlPatientInformationViewer #canvasContainer').hide();
                    $('#pnlPatientInformationViewer #imagesControls').hide();
                    $('#pnlPatientInformationViewer #OpenDocumentIF').show();
                    $('#pnlPatientInformationViewer #OpenHTMLDocument').hide();
                    $('#pnlPatientInformationViewer #extraContorls').hide();
                    Patient_Information_Viewer.params['DocumentType'] = "PDF";
                    $('#progressnotehtmldoc').remove();

                    utility.PDFViewer(document_details.Base64FileStream, false, 'pnlPatientInformationViewer #OpenDocumentIF');
                }
                else if (FileType == null || FileType == "") {

                    //Patient_Information_Viewer.LoadImagesData(document_details.Base64FileStream, document_details.FileType);
                    $('#progressnotehtmldoc').remove();
                    $('#pnlPatientInformationViewer #canvasContainer').hide();
                    $('#pnlPatientInformationViewer #imagesControls').hide();
                    $('#pnlPatientInformationViewer #OpenDocumentIF').show();
                    $('#pnlPatientInformationViewer #OpenHTMLDocument').hide();
                    $('#pnlPatientInformationViewer #extraContorls').hide();
                    Patient_Information_Viewer.params['DocumentType'] = "PDF";
                    $('#progressnotehtmldoc').remove();

                    utility.PDFViewer(document_details.Base64FileStream, false, 'pnlPatientInformationViewer #OpenDocumentIF');
                }
                else if (FileType.indexOf("image") > -1) {
                    try {
                        $('#pnlPatientInformationViewer #OpenDocumentIF').hide();
                        $('#pnlPatientInformationViewer #OpenHTMLDocument').hide();
                        $('#pnlPatientInformationViewer #canvasContainer').show();
                        $('#pnlPatientInformationViewer #imagesControls').show();
                        $('#pnlPatientInformationViewer #extraContorls').show();
                        Patient_Information_Viewer.params['DocumentType'] = "IMAGE";
                        var imageObj = new Image();
                        //for IE
                        //  imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;

                        imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;
                        var canvas = document.getElementById("canvas");
                        setTimeout(function () {
                            canvas.width = imageObj.width;
                            canvas.height = imageObj.height;
                        }, 1000);

                        var context = canvas.getContext("2d");


                        function draw() {
                            var scale = 1;
                            var originx = 0;
                            var originy = 0;
                            context.save();
                            context.setTransform(1, 0, 0, 1, 0, 0);
                            context.clearRect(0, 0, canvas.width, canvas.height);
                            context.restore();
                            context.drawImage(imageObj, 0, 0);
                        }

                        setTimeout(function () { draw(); }, 1000);
                        Patient_Information_Viewer.interval = setInterval(function () { draw(); }, 1000);
                    }
                    catch (ex) {
                        utility.DisplayMessages(ex, 2);
                        console.log(ex);
                    }
                }
                else if (FileType.indexOf("pdf") > -1) {
                    if (document_details.NoteHtml && document_details.NoteHtml != null && document_details.NoteHtml != "undefined") {
                        var ProgressNotehtml = $("<div id='progressnotehtmldoc' ></div>").append(document_details.NoteHtml);
                        $("#pnlPatientInformationViewer #Notedoc").append(ProgressNotehtml);
                        kendo.drawing.drawDOM("#pnlPatientInformationViewer #Notedoc", {
                            landscape: false,
                            scale: 0.6,
                            paperSize: "A4",
                            // margin: "2cm 3cm ",
                            margin: {
                                left: "10mm",
                                //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                top: "3mm",
                                //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                right: "10mm",
                                bottom: "15mm"
                            },
                            rowtemplate: $('#' + Patient_Information_Viewer.params["PanelID"] + " #page-templateLegacy").html()
                            // template: kendo.template( $('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html())
                        }).then(function (group) {

                            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                                var pdfurl = dataURL.split('data:application/pdf;base64,').join('');
                                $('#progressnotehtmldoc').remove();
                                $('#pnlPatientInformationViewer #canvasContainer').hide();
                                $('#pnlPatientInformationViewer #imagesControls').hide();
                                $('#pnlPatientInformationViewer #OpenDocumentIF').show();
                                $('#pnlPatientInformationViewer #OpenHTMLDocument').hide();
                                $('#pnlPatientInformationViewer #extraContorls').hide();
                                Patient_Information_Viewer.params['DocumentType'] = "PDF";
                                $('#progressnotehtmldoc').remove();
                                utility.PDFViewer(pdfurl, false, 'pnlPatientInformationViewer #OpenDocumentIF');
                            });

                        });
                    } else {
                        $('#pnlPatientInformationViewer #canvasContainer').hide();
                        $('#pnlPatientInformationViewer #imagesControls').hide();
                        $('#pnlPatientInformationViewer #OpenDocumentIF').show();
                        $('#pnlPatientInformationViewer #OpenHTMLDocument').hide();
                        $('#pnlPatientInformationViewer #extraContorls').hide();
                        Patient_Information_Viewer.params['DocumentType'] = "PDF";

                        // window.open = ContentUri;

                        //PDF Viewer
                        utility.PDFViewer(document_details.Base64FileStream, false, 'pnlPatientInformationViewer #OpenDocumentIF');
                    }

                }

                else if (FileType.indexOf("html") > -1) {
                    try {
                        $('#pnlPatientInformationViewer #OpenHTMLDocument').show();
                        $('#pnlPatientInformationViewer #OpenDocumentIF').hide();
                        $('#pnlPatientInformationViewer #canvasContainer').hide();
                        $('#pnlPatientInformationViewer #imagesControls').hide();
                        $('#pnlPatientInformationViewer #extraContorls').hide();
                        Patient_Information_Viewer.params['DocumentType'] = "HTML";

                        $("#pnlPatientInformationViewer #OpenHTMLDocument").contents().find('html').html(atob(document_details.Base64FileStream));


                    }
                    catch (ex) {
                        utility.DisplayMessages(ex, 2);
                        console.log(ex);
                    }
                }

                var table = $('#' + Patient_Information_Viewer.params.PanelID + ' ' + $('#' + Patient_Information_Viewer.params.PanelID + ' .tabs-custom li.active a').attr('href')).find('table');
                table.find('tbody tr[documentid=' + Patient_Information_Viewer.params.PatDocID + ']').addClass('active');
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    PrintDocument: function () {
        var DocType = Patient_Information_Viewer.params.DocumentType.toLowerCase();
        if (DocType == 'image') {
            var canvasObj = document.getElementById("pnlPatientInformationViewer").querySelector('#canvas');
            var canvasContext = canvasObj.getContext("2d");
            $('#pnlPatientInformationViewer #printHelper').append('<img src="' + canvasObj.toDataURL() + '"/>');
            $('#pnlPatientInformationViewer #printHelper').printMe();


            $('#pnlPatientInformationViewer #printHelper').empty();
        }
    },

    UnLoadTab: function () {
        if (Patient_Information_Viewer.params && Patient_Information_Viewer.params.ParentCtrl) {
            UnloadActionPan(Patient_Information_Viewer.params.ParentCtrl, 'Patient_Information_Viewer');
        }
        else
            UnloadActionPan(null, 'Patient_Information_Viewer');
    },

    LoadImagesData: function (base64, filetype) {
        var byteCharacters = atob(base64);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        var objDef2 = $.Deferred();
        var zip = new JSZip();
        var filesData = "";
        zip.loadAsync(byteArray).then(function (response) {
            Object.keys(zip.files).forEach(function (filename) {
                zip.files[filename].async('base64').then(function (fileData) {
                    filesData = fileData;
                    objDef2.resolve("ok")
                })
            })
        });
        objDef2.then(function () {
            Patient_Information_Viewer.MergeImageInViewr(filetype, filesData);
        });
    },

    MergeImageInViewr: function (FileType, filesData) {
        try {
            $('#pnlPatientInformationViewer #OpenDocumentIF').hide();
            $('#pnlPatientInformationViewer #OpenHTMLDocument').hide();
            $('#pnlPatientInformationViewer #canvasContainer').show();
            $('#pnlPatientInformationViewer #imagesControls').show();
            $('#pnlPatientInformationViewer #extraContorls').show();
            Patient_Information_Viewer.params['DocumentType'] = "IMAGE";
            var imageObj = new Image();
            //for IE
            //  imageObj.src = "data:" + document_details.FileType + ";base64," + document_details.Base64FileStream;

            imageObj.src = "data:" + FileType + ";base64," + filesData;
            var canvas = document.getElementById("canvas");
            setTimeout(function () {
                canvas.width = imageObj.width;
                canvas.height = imageObj.height;
            }, 1000);

            var context = canvas.getContext("2d");


            function draw() {
                var scale = 1;
                var originx = 0;
                var originy = 0;
                context.save();
                context.setTransform(1, 0, 0, 1, 0, 0);
                context.clearRect(0, 0, canvas.width, canvas.height);
                context.restore();
                context.drawImage(imageObj, 0, 0);
            }

            setTimeout(function () { draw(); }, 1000);
            Patient_Information_Viewer.interval = setInterval(function () { draw(); }, 1000);
            Patient_Information_Viewer.params.t1 = performance.now();
            console.log("Call to compression took " + (Patient_Information_Viewer.params.t1 - Patient_Information_Viewer.params.t0) + " milliseconds.")
        }
        catch (ex) {
            utility.DisplayMessages(ex, 2);
            console.log(ex);
        }
    },

}