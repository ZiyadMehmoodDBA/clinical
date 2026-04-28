ClinicalBarCodeView = {
    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    LabOrderProblems: [],
    FavListName: "LabOrderDetail",
    checkedProblems: [],
    CPTCodeQA: [],
    ArrayValidation: [],
    selectedTestCode: null,
    selectedTestDescription: null,
    ClientNo: "",
    FullName: "",
    LabName: "",
    OrderNo: "",
    DOB: "",


    Load: function (params) {

        ClinicalBarCodeView.params = params;

        if (ClinicalBarCodeView.params.PanelID != 'pnlClinicalBarCodeView') {
            ClinicalBarCodeView.params.PanelID = ClinicalBarCodeView.params.PanelID + ' #pnlClinicalBarCodeView';
        } else {
            ClinicalBarCodeView.params.PanelID = 'pnlClinicalBarCodeView';
        }




        ClinicalBarCodeView.fillLabOrder(ClinicalBarCodeView.params.LabOrderId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    var LabOrderList = JSON.parse(response.LabOderFill_JSON);
                    if (LabOrderList.length > 0) {

                        var labOrder = LabOrderList[0];
                        var PatientName = '', PatientAge = '', PatientSex = '';
                        if (Patient_Demographic != null && Patient_Demographic.params != null) {
                            PatientAge = Patient_Demographic.params.patientAge;
                            PatientSex = Patient_Demographic.params.PatientSex;
                            PatientName = Patient_Demographic.params.PatientFirstName + ' ' + Patient_Demographic.params.PatientLastName;
                        }
                        var PatientDOB = labOrder.PatientDOB.split(' ')[0];
                        var splitDOB = PatientDOB.split('/');
                        var month = splitDOB[0].length > 1 ? splitDOB[0] : '0' + splitDOB[0];
                        var day = splitDOB[1].length > 1 ? splitDOB[1] : '0' + splitDOB[1];
                        var year = splitDOB[2];
                        PatientDOB = month + "/" + day + "/" + year;

                        var txtNoOfPrints = $('#' + ClinicalBarCodeView.params.PanelID + " #txtNoOfPrints");
                        txtNoOfPrints.val(1);
                        ClinicalBarCodeView.generateNoOfPrints(txtNoOfPrints);
                        $('#' + ClinicalBarCodeView.params.PanelID + " #lblClientNo").text(labOrder.ClientNo);
                        $('#' + ClinicalBarCodeView.params.PanelID + " #lblOrderNumber").text(labOrder.OrderNo);
                        $('#' + ClinicalBarCodeView.params.PanelID + " #lblLabName").text(labOrder.LabName);
                        $('#' + ClinicalBarCodeView.params.PanelID + " #lblPatName").text(labOrder.FullName);
                        $('#' + ClinicalBarCodeView.params.PanelID + " #lblPatDOB").text(PatientDOB);
                        ClinicalBarCodeView.ClientNo = labOrder.ClientNo;
                        ClinicalBarCodeView.OrderNo = labOrder.OrderNo;
                        ClinicalBarCodeView.LabName = labOrder.LabName;
                        ClinicalBarCodeView.FullName = labOrder.FullName;
                        ClinicalBarCodeView.DOB = PatientDOB;
                        //var output = utility.RemoveTimeFromDate(null, labOrder.OrderDate) + ' ' + labOrder.OrderTime
                        //$('#' + ClinicalBarCodeView.params.PanelID + " #lblOrderDateTime").text(output);
                        //$('#' + ClinicalBarCodeView.params.PanelID + " #lblAccountNumberAndName").text(labOrder.AccountNumber + ' ' + PatientName);
                        //$('#' + ClinicalBarCodeView.params.PanelID + " #lblGenderAge").text(PatientSex + ' ' + PatientAge);

                        //ClinicalBarCodeView.generateBarcode(labOrder.OrderNo);
                    }



                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });

        //ClinicalBarCodeView.domReadyFunction();
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
            var divAllPrint = $('#' + ClinicalBarCodeView.params.PanelID + " #printAllSpecimen");
            divAllPrint.empty();
            var htmlToAppend = "";
            for (var i = 0; i < noOfPrints; i++) {
                var clone = $('#' + ClinicalBarCodeView.params.PanelID + " #divPrintSpecimenLabel").clone();
                clone.removeClass("hidden");
                htmlToAppend += clone.html();
            }
            divAllPrint.html(htmlToAppend);
        }
    },

    generateBarcode: function (orderNumber) {
        var value = orderNumber;
        var btype = 'code39';
        var renderer = 'css';

        var quietZone = false;


        var settings = {
            output: renderer,
            bgColor: '#FFFFFF',
            color: '#000000',
            barWidth: '1',
            barHeight: '50',
            moduleSize: '5',
            posX: '10',
            posY: '20',
            addQuietZone: '1'
        };



        $("#barcodeTarget").html("").show().barcode(value, btype, settings);

    },

    fillLabOrder: function (LabOrderId) {

        var objData = {};
        objData["commandType"] = "fill_LabOrder";
        objData["LabOrderId"] = LabOrderId;
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

            // set label text
            label.setObjectText("LabName", "Lab: " + ClinicalBarCodeView.LabName);
            label.setObjectText("ClientNo", "Client #: " + ClinicalBarCodeView.ClientNo);
            label.setObjectText("OrderNo", "Ref.#: " + ClinicalBarCodeView.OrderNo);
            label.setObjectText("FullName", "Name: " + ClinicalBarCodeView.FullName);
            label.setObjectText("DOB", "DOB: " + ClinicalBarCodeView.DOB);
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
            var count = parseInt($("#txtNoOfPrints").val());

            for (var i = 0; i < count; i++) {
                label.print(printerName);
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
            var docCnt = $('#' + ClinicalBarCodeView.params.PanelID + " #frmClinicalBarCodeView").clone();
            docCnt.find("#divNoOfPrints").css("display", "none");
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

        ClinicalBarCodeView.checkedProblems = [];
        ClinicalBarCodeView.CPTCodeQA = [];
        var form = '#' + ClinicalBarCodeView.params.PanelID + " #frmClinicalLabOrderDetail";
        var saveButtonisHidden = $('#' + ClinicalBarCodeView.params.PanelID + " #frmClinicalLabOrderDetail #btnSaveOrder").hasClass("hidden");

        if (caller == 'saveExit' || saveButtonisHidden == true) {
            if (ClinicalBarCodeView.params["ParentCtrl"] == "Clinical_LabOrder") {
                UnloadActionPan(ClinicalBarCodeView.params["ParentCtrl"], "ClinicalBarCodeView", null, ClinicalBarCodeView.params["ParentCtrlPanelID"]);
            }
            else {
                UnloadActionPan(ClinicalBarCodeView.params["ParentCtrl"], "ClinicalBarCodeView");

            }
        }
        else {

            if (ClinicalBarCodeView.params["ParentCtrl"] == "Clinical_LabOrder") {
                UnloadActionPan(ClinicalBarCodeView.params["ParentCtrl"], "ClinicalBarCodeView", null, ClinicalBarCodeView.params["ParentCtrlPanelID"]);
            }
            else if (ClinicalBarCodeView.params["ParentCtrl"] == "Clinical_LabOrderView") {
                UnloadActionPan(ClinicalBarCodeView.params["ParentCtrl"]);
            }
            else {
                UnloadActionPan(ClinicalBarCodeView.params["ParentCtrl"], "ClinicalBarCodeView");

            }

        }
    },
}