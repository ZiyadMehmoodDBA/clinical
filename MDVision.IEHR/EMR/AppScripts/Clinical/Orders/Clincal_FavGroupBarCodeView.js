ClincalFavGroupBarCodeView = {
    params: [],
    bIsFirstLoad: true,
    LabOrderList: [],
    Load: function (params) {

        ClincalFavGroupBarCodeView.params = params;

        if (ClincalFavGroupBarCodeView.params.PanelID != 'pnlClincalFavGroupBarCodeView') {
            ClincalFavGroupBarCodeView.params.PanelID = ClincalFavGroupBarCodeView.params.PanelID + ' #pnlClincalFavGroupBarCodeView';
        } else {
            ClincalFavGroupBarCodeView.params.PanelID = 'pnlClincalFavGroupBarCodeView';
        }

        ClincalFavGroupBarCodeView.fillLabOrder(ClincalFavGroupBarCodeView.params.LabOrderIDs).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    ClincalFavGroupBarCodeView.LabOrderList = response.LabOderFill_JSON;
                    if (ClincalFavGroupBarCodeView.LabOrderList.length > 0) {
                        var htmlStr = "";
                        for (var i = 0; i < ClincalFavGroupBarCodeView.LabOrderList.length; i++) {

                            var PatientDOB = ClincalFavGroupBarCodeView.LabOrderList[i].PatientDOB.split(' ')[0];
                            var splitDOB = PatientDOB.split('/');
                            var month = splitDOB[0].length > 1 ? splitDOB[0] : '0' + splitDOB[0];
                            var day = splitDOB[1].length > 1 ? splitDOB[1] : '0' + splitDOB[1];
                            var year = splitDOB[2];
                            PatientDOB = month + "/" + day + "/" + year;
                            ClincalFavGroupBarCodeView.LabOrderList[i].PatientDOB = PatientDOB;
                            ClincalFavGroupBarCodeView.LabOrderList[i].TxtNoOfPrintName = '#txtNoOfPrints' + i;
                            if (i == 0 || i % 2 == 0) {
                                htmlStr += '<div class="row">';
                            }
                            htmlStr += '<div class="col-md-4">';
                            htmlStr += '<div id="divNoOfPrints' + i + '" class="col-md-12">';
                            htmlStr += '  <label class="control-label">No of Prints </label>';
                            htmlStr += '  <input class="form-control" name="NoOfPrints' + i + '" id="txtNoOfPrints' + i + '" onchange="ClincalFavGroupBarCodeView.generateNoOfPrints(this)" onkeydown="ClincalFavGroupBarCodeView.maxLegthCheck(e,this)" value="1" type="number" maxlength="3" />';
                            htmlStr += '</div>';
                            htmlStr += '<div class="spacer15"></div>';
                            htmlStr += '<div id="divPrintSpecimenLabel' + i + '" class="">';
                            htmlStr += '    <div class="col-md-12">';
                            htmlStr += '        <label class="control-label">Lab:</label>';
                            htmlStr += '        <label class="control-label" id="lblLabName' + i + '">' + ClincalFavGroupBarCodeView.LabOrderList[i].LabName + '</label>';
                            htmlStr += '    </div>';
                            htmlStr += '    <div class="col-md-12">';
                            htmlStr += '        <label class="control-label">Client #:</label>';
                            htmlStr += '        <label class="control-label" id="lblClientNo' + i + '">' + ClincalFavGroupBarCodeView.LabOrderList[i].ClientNo + '</label>';
                            htmlStr += '    </div>';
                            htmlStr += '    <div class="col-md-12">';
                            htmlStr += '        <label class="control-label">Ref. #:</label>';
                            htmlStr += '        <label class="control-label" id="lblOrderNumber' + i + '">' + ClincalFavGroupBarCodeView.LabOrderList[i].OrderNo + '</label>';
                            htmlStr += '    </div>';
                            htmlStr += '    <div class="col-md-12">';
                            htmlStr += '        <label class="control-label">Name:</label>';
                            htmlStr += '        <label class="control-label" id="lblPatName' + i + '">' + ClincalFavGroupBarCodeView.LabOrderList[i].FullName + '</label>';
                            htmlStr += '    </div>';
                            htmlStr += '    <div class="col-md-12">';
                            htmlStr += '        <label class="control-label">DOB:</label>';
                            htmlStr += '        <label class="control-label" id="lblPatDOB' + i + '">' + PatientDOB + '</label>';
                            htmlStr += '    </div>';
                            htmlStr += '    <div class="col-md-12 testname">';
                            htmlStr += '        <label class="control-label">' + ClincalFavGroupBarCodeView.LabOrderList[i].TestName + '</label>';
                            htmlStr += '    </div>';
                            htmlStr += '    <div class="spacer15"></div>';
                            htmlStr += '</div>';
                            htmlStr += '<div class="spacer15"></div>';
                            htmlStr += '</div>';

                            if (i != 0 && i % 2 == 0) {
                                htmlStr += '</div>';
                            }
                        }
                        if ((i-1) % 2 != 0) {
                            htmlStr += '</div>';
                        }
                        htmlStr += '<div class="row">';
                        htmlStr += '<div class="col-md-12">';
                        htmlStr += '<div class="pull-right pt-md pr-default">';
                        htmlStr += '<button type="button" class="btn btn-sm btn-primary" id="btnPrint" onclick="ClincalFavGroupBarCodeView.PrintBarCode()">Print</button>';
                        htmlStr += '</div>';
                        htmlStr += '</div>';
                        htmlStr += '</div>';

                        $('#frmClincalFavGroupBarCodeView').append(htmlStr);

                    }

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });
    },

    maxLegthCheck: function (e, txt) {
        var keycode = e.keyCode;

    },

    generateNoOfPrints: function (obj) {
        if (obj != null) {
            var noOfPrints = $(obj).val();
            if (noOfPrints > 9) {
                noOfPrints = 9;
                $(obj).val(noOfPrints);
            }
            noOfPrints = noOfPrints != "" && parseInt(noOfPrints) > 1 ? parseInt(noOfPrints) : 1;
            $(obj).val(noOfPrints)
            //var divAllPrint = $('#' + ClincalFavGroupBarCodeView.params.PanelID + " #printAllSpecimen");
            //divAllPrint.empty();
            //var htmlToAppend = "";
            //for (var i = 0; i < noOfPrints; i++) {
            //    var clone = $('#' + ClincalFavGroupBarCodeView.params.PanelID + " #divPrintSpecimenLabel").clone();
            //    clone.removeClass("hidden");
            //    htmlToAppend += clone.html();
            //}
            //divAllPrint.html(htmlToAppend);
        }
    },

    fillLabOrder: function (LabOrderIDs) {

        var objData = {};
        objData["commandType"] = "fill_favgrouplaborder";
        objData["LabOrderIDs"] = LabOrderIDs;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "LabOrder", "LabOrder");
    },


    PrintBarCode: function () {
        var printerFind = true;
        try {
            // open label
            var labelXml = '<?xml version="1.0" encoding="utf-8"?>\
    <DieCutLabel Version="8.0" Units="twips">\
        <PaperOrientation>Landscape</PaperOrientation>\
        <Id>Address</Id>\
        <PaperName>30252 Address</PaperName>\
        <DrawCommands/>\
        <ObjectInfo>\
            <TextObject>\
                <Name>LabName</Name>\
                <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
                <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
                <LinkedObjectName></LinkedObjectName>\
                <Rotation>Rotation0</Rotation>\
                <IsMirrored>False</IsMirrored>\
                <IsVariable>True</IsVariable>\
                <HorizontalAlignment>Left</HorizontalAlignment>\
                <VerticalAlignment>Middle</VerticalAlignment>\
                <TextFitMode>ShrinkToFit</TextFitMode>\
                <UseFullFontHeight>True</UseFullFontHeight>\
                <Verticalized>False</Verticalized>\
                <StyledText/>\
            </TextObject>\
            <Bounds X="332" Y="150" Width="2210" Height="250" />\
        </ObjectInfo>\
        <ObjectInfo>\
            <TextObject>\
                <Name>ClientNo</Name>\
                <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
                <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
                <LinkedObjectName></LinkedObjectName>\
                <Rotation>Rotation0</Rotation>\
                <IsMirrored>False</IsMirrored>\
                <IsVariable>True</IsVariable>\
                <HorizontalAlignment>Left</HorizontalAlignment>\
                <VerticalAlignment>Middle</VerticalAlignment>\
                <TextFitMode>ShrinkToFit</TextFitMode>\
                <UseFullFontHeight>True</UseFullFontHeight>\
                <Verticalized>False</Verticalized>\
                <StyledText/>\
            </TextObject>\
            <Bounds X="332" Y="400" Width="2210" Height="250" />\
        </ObjectInfo>\
        <ObjectInfo>\
            <TextObject>\
                <Name>OrderNo</Name>\
                <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
                <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
                <LinkedObjectName></LinkedObjectName>\
                <Rotation>Rotation0</Rotation>\
                <IsMirrored>False</IsMirrored>\
                <IsVariable>True</IsVariable>\
                <HorizontalAlignment>Left</HorizontalAlignment>\
                <VerticalAlignment>Middle</VerticalAlignment>\
                <TextFitMode>ShrinkToFit</TextFitMode>\
                <UseFullFontHeight>True</UseFullFontHeight>\
                <Verticalized>False</Verticalized>\
                <StyledText/>\
            </TextObject>\
            <Bounds X="332" Y="650" Width="2210" Height="250" />\
        </ObjectInfo>\
        <ObjectInfo>\
            <TextObject>\
                <Name>FullName</Name>\
                <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
                <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
                <LinkedObjectName></LinkedObjectName>\
                <Rotation>Rotation0</Rotation>\
                <IsMirrored>False</IsMirrored>\
                <IsVariable>True</IsVariable>\
                <HorizontalAlignment>Left</HorizontalAlignment>\
                <VerticalAlignment>Middle</VerticalAlignment>\
                <TextFitMode>AlwaysFit</TextFitMode>\
                <UseFullFontHeight>True</UseFullFontHeight>\
                <Verticalized>False</Verticalized>\
                <StyledText/>\
            </TextObject>\
            <Bounds X="332" Y="900" Width="2210" Height="250" />\
        </ObjectInfo>\
        <ObjectInfo>\
            <TextObject>\
                <Name>DOB</Name>\
                <ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
                <BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
                <LinkedObjectName></LinkedObjectName>\
                <Rotation>Rotation0</Rotation>\
                <IsMirrored>False</IsMirrored>\
                <IsVariable>True</IsVariable>\
                <HorizontalAlignment>Left</HorizontalAlignment>\
                <VerticalAlignment>Middle</VerticalAlignment>\
                <TextFitMode>AlwaysFit</TextFitMode>\
                <UseFullFontHeight>True</UseFullFontHeight>\
                <Verticalized>False</Verticalized>\
			   <StyledText/>\
            </TextObject>\
            <Bounds X="332" Y="1150" Width="2210" Height="250" />\
        </ObjectInfo>\
    </DieCutLabel>';


            var label = dymo.label.framework.openLabelXml(labelXml);

            // select printer to print on
            // for simplicity sake just use the first LabelWriter printer
            var printers = dymo.label.framework.getPrinters();
            if (printers.length == 0) {
                printerFind = false;
                //   utility.DisplayMessages("No DYMO printers are installed. Install DYMO printers.",3);
            }

            var printerName = "";
            for (var i = 0; i < printers.length; ++i) {
                var printer = printers[i];
                if (printer.printerType == "LabelWriterPrinter") {
                    printerName = printer.name;
                    break;
                }
            }

            if (printerName == "") {
                printerFind = false;
                //     utility.DisplayMessages("No LabelWriter printers found. Install LabelWriter printer",3);

            }

            for (var j = 0; j < ClincalFavGroupBarCodeView.LabOrderList.length; j++) {

                // set label text
                label.setObjectText("LabName", "Lab: " + ClincalFavGroupBarCodeView.LabOrderList[j].LabName);
                label.setObjectText("ClientNo", "Client #: " + ClincalFavGroupBarCodeView.LabOrderList[j].ClientNo);
                label.setObjectText("OrderNo", "Ref.#: " + ClincalFavGroupBarCodeView.LabOrderList[j].OrderNo);
                label.setObjectText("FullName", "Name: " + ClincalFavGroupBarCodeView.LabOrderList[j].FullName);
                label.setObjectText("DOB", "DOB: " + ClincalFavGroupBarCodeView.LabOrderList[j].PatientDOB);

                var count = parseInt($(ClincalFavGroupBarCodeView.LabOrderList[j].TxtNoOfPrintName).val());

                for (var i = 0; i < count; i++) {
                    label.print(printerName);
                }
            }

        }
        catch (ex) {
            printerFind = false;
            utility.DisplayMessages("No label writer printer found.", 3);
            console.log(ex);
            //  utility.DisplayMessages(ex.message || ex,4);
        }
        if (!printerFind) {
            printerFind = true;
            var docType = '<!doctype html>';
            var docCnt = $('#' + ClincalFavGroupBarCodeView.params.PanelID + " #frmClincalFavGroupBarCodeView").clone();
            docCnt.find("[id^=divNoOfPrints]").css("display", "none");
            docCnt.find(".testname").css("display", "none");
            docCnt = docCnt.html();

            var docHead = '<head> <script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
         + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
         + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
         + '</script>'
         + '</head>';
            var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=865, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
            var newWin = window.open("", "_blank", winAttr);
            writeDoc = newWin.document;
            writeDoc.open();
            writeDoc.write(docType + '<html>' + docHead + '<body>' + docCnt + '</body></html>');
            //writeDoc.close();

            //newWin.focus();
            ////   Our changes End
            setTimeout(function () {
                newWin.focus();
                newWin.print();
                newWin.close();
            }, 1000);
        }
    },



    UnLoad: function (caller) {

        ClincalFavGroupBarCodeView.checkedProblems = [];
        ClincalFavGroupBarCodeView.CPTCodeQA = [];
        var form = '#' + ClincalFavGroupBarCodeView.params.PanelID + " #frmClinicalLabOrderDetail";
        var saveButtonisHidden = $('#' + ClincalFavGroupBarCodeView.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder").hasClass("hidden");

        if (caller == 'saveExit' || saveButtonisHidden == true) {
            if (ClincalFavGroupBarCodeView.params["ParentCtrl"] == "Clinical_LabOrder") {
                UnloadActionPan(ClincalFavGroupBarCodeView.params["ParentCtrl"], "ClincalFavGroupBarCodeView", null, ClincalFavGroupBarCodeView.params["ParentCtrlPanelID"]);
            }
            else {
                UnloadActionPan(ClincalFavGroupBarCodeView.params["ParentCtrl"], "ClincalFavGroupBarCodeView");

            }
        }
        else {

            if (ClincalFavGroupBarCodeView.params["ParentCtrl"] == "Clinical_LabOrder") {
                UnloadActionPan(ClincalFavGroupBarCodeView.params["ParentCtrl"], "ClincalFavGroupBarCodeView", null, ClincalFavGroupBarCodeView.params["ParentCtrlPanelID"]);
            }
            else if (ClincalFavGroupBarCodeView.params["ParentCtrl"] == "Clinical_LabOrderView") {
                UnloadActionPan(ClincalFavGroupBarCodeView.params["ParentCtrl"]);
            }
            else {
                UnloadActionPan(ClincalFavGroupBarCodeView.params["ParentCtrl"], "ClincalFavGroupBarCodeView");

            }

        }
    },
}