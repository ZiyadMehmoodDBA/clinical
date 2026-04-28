Batch_FaxSendAnnotate = {
    bIsFirstLoad: true,
    params: [],
    scale: 1,
    height: 0,
    width: 0,
    ParentCtrlId: null,
    // Programmer: Ahsan Nasir

    Load: function (params) {
        Batch_FaxSendAnnotate.params = params;
        Batch_FaxSendAnnotate.ParentCtrlId = Batch_FaxSendAnnotate.params.ParentCtrl;

        //Batch_FaxSendAnnotate.canvas = document.getElementById('canvas');
        //Batch_FaxSendAnnotate.context = canvas.getContext('2d');
        //Batch_FaxSendAnnotate.elementID = "pdf-thumbnail";

        Batch_FaxSendAnnotate.LoadPDF();

        $("#pdf-thumbnail").on("click", 'li', function () {

            $(".active img").attr("src", lc.getImage().toDataURL('image/jpeg', 1.0));

            lc.clear();


            var backgroundImage = new Image()
            backgroundImage.src = $(this.children).attr("src");
            lc.saveShape(LC.createShape('Image', { x: 0, y: 0, image: backgroundImage, scale: Batch_FaxSendAnnotate.scale }));
        });

        $("#save").click(function (e) {
            e.preventDefault();
            $(".active img").attr("src", lc.getImage().toDataURL('image/jpeg', 1.0));
            var doc = new jsPDF("p", "mm", "a4", true);
            var totalPages = $("#pdf-thumbnail li").length;
            $("#pdf-thumbnail li").each(function () {

                var html = $(this).children().attr("src");
                doc.addImage(html, 'JPEG', 10, 10, 180, 290); //EMR-7104

                if (totalPages > 1) {
                    doc.addPage();
                    totalPages--;
                }
            });

            var datauri = doc.save("download.pdf");
            //  utility.DisplayMessages("File successfully downloaded.", 1);

        });

    },
    loadClassifyPages: function () {

        $(".active img").attr("src", lc.getImage().toDataURL('image/jpeg', 1.0));
        var doc = new jsPDF("p", "in", "a4", true);
        var totalPages = $("#pdf-thumbnail li").length;
        $("#pdf-thumbnail li").each(function () {

            var html = $(this).children().attr("src");
            doc.addImage(html, 'JPEG', 0, 0, Batch_FaxSendAnnotate.width, Batch_FaxSendAnnotate.height);

            if (totalPages > 1) {
                doc.addPage();
                totalPages--;
            }
        });

        var datauri = doc.output('datauristring');

        var params = [];
        params["pdfBase64"] = datauri;
        params["FaxId"] = Batch_FaxSendAnnotate.params["FaxId"];
        params["ParentCtrl"] = "Batch_FaxSendAnnotate";
        LoadActionPan("Batch_FaxClassifyPages", params);
    },
    LoadPDF: function () {

        var canvas = document.getElementById('canvas');
        var context = canvas.getContext('2d');

        var pdfBase64 = Batch_FaxSend.AnnotateArray["base64"];
        var binaryPdf = Batch_FaxSendAnnotate.ConvertBase64ToBinary(pdfBase64);
        var currentPage = 1;
        var globalPdf = null;
        ////--var container = document.getElementById('pdf-thumbnail');
        function renderPage(page) {
            //
            // Prepare canvas using PDF page dimensions
            //
            ////--  var canvas = document.createElement('canvas');
            // Link: http://stackoverflow.com/a/13039183/1577396
            // Canvas width should be set to the window's width for appropriate
            // scaling factor of the document with respect to the canvas width
            var viewport = page.getViewport(1.2);
            // append the created canvas to the container
            ////--  container.appendChild(canvas);
            // Get context of the canvas
            ////-- var context = canvas.getContext('2d');
            canvas.height = viewport.height;
            canvas.width = viewport.width;
            //
            // Render PDF page into canvas context
            //
            var renderContext = {
                canvasContext: context,
                viewport: viewport
            };

            page.render(renderContext).then(function () {

                var img = canvas.toDataURL('image/jpeg', 1.0);
                $("#pdf-thumbnail").append('<li data-position="' + currentPage + '"><img src="' + img + '"/></li>');

                if (currentPage < globalPdf.numPages) {
                    currentPage++;
                    globalPdf.getPage(currentPage).then(renderPage);
                } else {
                    // Callback function here, which will trigger when all pages are loaded
                    Batch_FaxSendAnnotate.CreateScrollSlider();
                    Batch_FaxSendAnnotate.CreateAnnotateLiterallyEditor();

                }
            }, function (error) {
                utility.DisplayMessages(error.message, 2);
                Batch_FaxSendAnnotate.UnLoad();
            });

        }
        PDFJS.getDocument(binaryPdf).then(function (pdf) {
            if (!globalPdf) {
                globalPdf = pdf;
            }
            pdf.getPage(currentPage).then(renderPage);
        }, function (error) {
            utility.DisplayMessages(error.message, 2);
            Batch_FaxSendAnnotate.UnLoad();
        });
    },
    CreateScrollSlider: function () {

        var $frame = $('#smart-pdf-thumbnail');
        var $slidee = $frame.children('ul').eq(0);
        var $wrap = $frame.parent();

        if (Sly.getInstance($frame[0])) {
            Sly.getInstance($frame[0]).destroy();
        }

        var options = ({
            itemNav: 'basic',
            smart: 1,
            activateOn: 'click',
            mouseDragging: 1,
            touchDragging: 1,
            releaseSwing: 1,
            startAt: 3,
            scrollBar: $wrap.find('.scrollbar'),
            scrollBy: 1,
            pagesBar: $wrap.find('.pages'),
            activatePageOn: 'click',
            speed: 300,
            elasticBounds: 1,
            easing: 'easeOutExpo',
            dragHandle: 1,
            dynamicHandle: 1,
            clickBar: 1,
        });

        var sly = new Sly($frame, options).init();
        sly.activate(0);
    },
    CreateAnnotateLiterallyEditor: function () {
        var backgroundImage = new Image()
        backgroundImage.src = $(".active img").attr("src");

        lc = LC.init(document.getElementById('editor'), {
            imageSize: { width: 720, height: 950 },
            keyboardShortcuts: false,
            tools: [LC.tools.Pan, LC.tools.Text, LC.tools.Pencil, LC.tools.Eraser,
                    LC.tools.Rectangle, LC.tools.Polygon, LC.tools.Ellipse, LC.tools.Line]
        });

        lc.saveShape(LC.createShape('Image', { x: 0, y: 0, image: backgroundImage, scale: Batch_FaxSendAnnotate.scale }));

        $(".lc-clear").hide();
    },
    ConvertBase64ToBinary: function (dataURI) {
        var BASE64_MARKER = ';base64,';
        var base64Index = dataURI.indexOf(BASE64_MARKER) + BASE64_MARKER.length;
        var base64 = dataURI.substring(base64Index);
        base64 = base64.replace(/\n/g, '');
        var raw = window.atob(base64);
        var rawLength = raw.length;
        var array = new Uint8Array(new ArrayBuffer(rawLength));

        for (var i = 0; i < rawLength; i++) {
            array[i] = raw.charCodeAt(i);
        }
        return array;
    },
    UnLoad: function () {
        //Batch_Fax.getInbox(Batch_Fax.client_id, Batch_Fax.client_secret, '1001');
        //Batch_Fax.getOutbox(Batch_Fax.client_id, Batch_Fax.client_secret);

        Batch_FaxSend.AnnotateArray["base64"] = "";
        if (Batch_FaxSendAnnotate.ParentCtrlId == "Batch_Fax")
        {
            Batch_FaxSendAnnotate.params.ParentCtrl = Batch_FaxSendAnnotate.ParentCtrlId;
            Batch_FaxSendAnnotate.ParentCtrlId = null;
        }
        if (Batch_FaxSendAnnotate.params != null && Batch_FaxSendAnnotate.params.ParentCtrl) {

            UnloadActionPan(Batch_FaxSendAnnotate.params.ParentCtrl);
        }
        else {


            UnloadActionPan();
        }
    },
    Reset: function () {
        $('#smart-pdf-thumbnail li').remove();
        Batch_FaxSendAnnotate.LoadPDF();
    }

}
