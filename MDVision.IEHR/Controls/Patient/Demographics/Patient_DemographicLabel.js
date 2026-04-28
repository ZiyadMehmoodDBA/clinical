DemographicLabel = {
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
    PatInfo: "",
    PatProvAccount: "",
    LabName: "",
    PatAddress: "",
    DOB: "",


    Load: function (params) {

        DemographicLabel.params = params;

        if (DemographicLabel.params.PanelID != 'pnlDemographicLabel') {
            DemographicLabel.params.PanelID = DemographicLabel.params.PanelID + ' #pnlDemographicLabel';
        } else {
            DemographicLabel.params.PanelID = 'pnlDemographicLabel';
        }




        DemographicLabel.fillDemographics(DemographicLabel.params.PatientID).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    var demographicData = JSON.parse(response.ResponseModel).DemographicData;
                    if (demographicData != null) {

                        var labOrder = demographicData

                        var PatientName = "";
                        var PatientDOB = "";
                        var PatientAddress = "";
                        var PatientInsurance = "";
                        var SubscriberID = "";
                        var Provider = "";
                        var AccountNumber = "";

                        if (demographicData.Settings.IsPatientName.toLowerCase() == "true" && demographicData.PatientName != null) {
                            PatientName = demographicData.PatientName;
                        }
                        if (demographicData.Settings.IsPatientDOB.toLowerCase() == "true" && demographicData.PatientDOB != null) {
                            PatientDOB = ' ' + demographicData.PatientDOB.split(' ')[0];
                         //   var PatientDOB = labOrder.PatientDOB.split(' ')[0];
                            var splitDOB = PatientDOB.split('/');
                            var month = splitDOB[0].length > 1 ? splitDOB[0] : '0' + splitDOB[0];
                            var day = splitDOB[1].length > 1 ? splitDOB[1] : '0' + splitDOB[1];
                            var year = splitDOB[2];
                            PatientDOB = ' ' + month + "/" + day + "/" + year;
                        }
                        if (demographicData.Settings.IsPatientAddress.toLowerCase() == "true" && demographicData.PatientAddress != null) {
                            PatientAddress = demographicData.PatientAddress;
                        }
                        if (demographicData.Settings.IsInsurancePlan.toLowerCase() == "true" && demographicData.InsurancePlan != null) {
                            PatientInsurance = demographicData.InsurancePlan;
                        }
                        if (demographicData.Settings.IsSubscriberID.toLowerCase() == "true" && demographicData.SubscriberID != null) {
                            SubscriberID = ' ' + demographicData.SubscriberID;
                        }
                        if (demographicData.Settings.IsPatientAccountNo.toLowerCase() == "true" && demographicData.AccountNumber != null) {
                            AccountNumber = demographicData.AccountNumber;
                        }
                        if (demographicData.Settings.IsProvider.toLowerCase() == "true" && demographicData.ProviderName != null) {
                            Provider = ' ' + demographicData.ProviderName;
                        }
                        
                       
                     

                        var txtNoOfPrints = $('#pnlDemographicLabel #txtNoOfPrints');
                        txtNoOfPrints.val(1);
                        DemographicLabel.generateNoOfPrints(txtNoOfPrints);
                        $('#pnlDemographicLabel #lblPatInfo').text(PatientName + PatientDOB);
                        $('#pnlDemographicLabel #lblPatAddress').text(PatientAddress);
                        $('#pnlDemographicLabel #lblPatInsuranceInfo').text(PatientInsurance + SubscriberID);
                        $('#pnlDemographicLabel #lblPatProvAccount').text(AccountNumber + Provider);
                        // $('#pnlDemographicLabel #lblPatDOB").text(PatientDOB);

                        if ($('#pnlDemographicLabel #lblPatInfo').text().trim().length == 0) {
                            $('#pnlDemographicLabel #lblPatInfo').remove();
                        }
                        if ($('#pnlDemographicLabel #lblPatAddress').text().trim().length == 0) {
                            $('#pnlDemographicLabel #lblPatAddress').remove();
                        }

                        if ($('#pnlDemographicLabel #lblPatInsuranceInfo').text().trim().length == 0) {
                            $('#pnlDemographicLabel #lblPatInsuranceInfo').remove();
                        }
                        if ($('#pnlDemographicLabel #lblPatProvAccount').text().trim().length == 0) {
                            $('#pnlDemographicLabel #lblPatProvAccount').remove();
                        }
                        DemographicLabel.PatInfo = PatientName + PatientDOB;
                        DemographicLabel.PatAddress = PatientAddress;
                        DemographicLabel.PatInsuranceInfo = PatientInsurance  + SubscriberID;
                        DemographicLabel.PatProvAccount = AccountNumber + Provider;
                     //   DemographicLabel.DOB = PatientDOB;
                        //var output = utility.RemoveTimeFromDate(null, labOrder.OrderDate) + ' ' + labOrder.OrderTime
                        //$('#pnlDemographicLabel #lblOrderDateTime").text(output);
                        //$('#pnlDemographicLabel #lblAccountNumberAndName").text(labOrder.AccountNumber + ' ' + PatientName);
                        //$('#pnlDemographicLabel #lblGenderAge").text(PatientSex + ' ' + PatientAge);

                        //DemographicLabel.generateBarcode(labOrder.PatAddress);
                    }



                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });

        //DemographicLabel.domReadyFunction();
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
            var divAllPrint = $('#pnlDemographicLabel #printAllSpecimen');
            divAllPrint.empty();
            var htmlToAppend = "";
            for (var i = 0; i < noOfPrints; i++) {
                var clone = $('#pnlDemographicLabel #divPrintSpecimenLabel').clone();
                clone.removeClass("hidden");
                htmlToAppend += clone.html();
            }
            divAllPrint.html(htmlToAppend);
        }
    },

    generateBarcode: function (PatAddress) {
        var value = PatAddress;
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

    fillDemographics: function (PatientID) {

        var objData = {};
        objData["CommandType"] = "load_demographic_label_data";
        objData["PatientId"] = PatientID;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "DashBoard", "LoadDashBoard");
    },


    PrintBarCode: function () {
        var printerFind = true;
        try {
            var currenPixels = 70;
            var PatInfo = "";
            var PatAddress = "";
            var PatInsuranceInfo = "";
            var PatProvAcc = "";
            if ($('#pnlDemographicLabel #lblPatInfo').text().trim().length != 0) {



                PatInfo = '<ObjectInfo>\
<TextObject>\
<Name>PatInfo</Name>\
<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
<LinkedObjectName />\
<Rotation>Rotation0</Rotation>\
<IsMirrored>False</IsMirrored>\
<IsVariable>False</IsVariable>\
<GroupID>-1</GroupID>\
<IsOutlined>False</IsOutlined>\
<HorizontalAlignment>Left</HorizontalAlignment>\
<VerticalAlignment>Middle</VerticalAlignment>\
<TextFitMode>ShrinkToFit</TextFitMode>\
<UseFullFontHeight>True</UseFullFontHeight>\
<Verticalized>False</Verticalized>\
<StyledText>\
<Element>\
<Attributes>\
<Font Family="Arial" Size="11" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
<ForeColor Alpha="255" Red="0" Green="0" Blue="0" HueScale="100" />\
</Attributes>\
</Element>\
</StyledText>\
</TextObject>\
<Bounds X="330" Y="' + currenPixels + '" Width="2210" Height="288" />\
</ObjectInfo>';
            }
            if ($('#pnlDemographicLabel #lblPatAddress').text().trim().length != 0) {
                currenPixels += 290;
                PatAddress = '<ObjectInfo>\
<TextObject>\
<Name>PatAddress</Name>\
<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
<LinkedObjectName />\
<Rotation>Rotation0</Rotation>\
<IsMirrored>False</IsMirrored>\
<IsVariable>False</IsVariable>\
<GroupID>-1</GroupID>\
<IsOutlined>False</IsOutlined>\
<HorizontalAlignment>Left</HorizontalAlignment>\
<VerticalAlignment>Middle</VerticalAlignment>\
<TextFitMode>ShrinkToFit</TextFitMode>\
<UseFullFontHeight>True</UseFullFontHeight>\
<Verticalized>False</Verticalized>\
<StyledText>\
<Element>\
<Attributes>\
<Font Family="Arial" Size="11" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
<ForeColor Alpha="255" Red="0" Green="0" Blue="0" HueScale="100" />\
</Attributes>\
</Element>\
</StyledText>\
</TextObject>\
<Bounds X="330" Y="' + currenPixels + '" Width="2210" Height="288" />\
</ObjectInfo>';

            }



            if ($('#pnlDemographicLabel #lblPatInsuranceInfo').text().trim().length != 0) {
                currenPixels += 290;
                PatInsuranceInfo = '<ObjectInfo>\
<TextObject>\
<Name>PatInsuranceInfo</Name>\
<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
<LinkedObjectName />\
<Rotation>Rotation0</Rotation>\
<IsMirrored>False</IsMirrored>\
<IsVariable>False</IsVariable>\
<GroupID>-1</GroupID>\
<IsOutlined>False</IsOutlined>\
<HorizontalAlignment>Left</HorizontalAlignment>\
<VerticalAlignment>Middle</VerticalAlignment>\
<TextFitMode>ShrinkToFit</TextFitMode>\
<UseFullFontHeight>True</UseFullFontHeight>\
<Verticalized>False</Verticalized>\
<StyledText>\
<Element>\
<Attributes>\
<Font Family="Arial" Size="11" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
<ForeColor Alpha="255" Red="0" Green="0" Blue="0" HueScale="100" />\
</Attributes>\
</Element>\
</StyledText>\
</TextObject>\
<Bounds X="330" Y="' + currenPixels + '" Width="2210" Height="288" />\
</ObjectInfo>';
            }
            if ($('#pnlDemographicLabel #lblPatProvAccount').text().trim().length != 0) {
                currenPixels += 290;
                PatProvAcc = ' <ObjectInfo>\
<TextObject>\
<Name>PatProvAccount</Name>\
<ForeColor Alpha="255" Red="0" Green="0" Blue="0" />\
<BackColor Alpha="0" Red="255" Green="255" Blue="255" />\
<LinkedObjectName />\
<Rotation>Rotation0</Rotation>\
<IsMirrored>False</IsMirrored>\
<IsVariable>False</IsVariable>\
<GroupID>-1</GroupID>\
<IsOutlined>False</IsOutlined>\
<HorizontalAlignment>Left</HorizontalAlignment>\
<VerticalAlignment>Middle</VerticalAlignment>\
<TextFitMode>ShrinkToFit</TextFitMode>\
<UseFullFontHeight>True</UseFullFontHeight>\
<Verticalized>False</Verticalized>\
<StyledText>\
<Element>\
<String xml:space="preserve">Click here to enter text</String>\
<Attributes>\
<Font Family="Arial" Size="11" Bold="False" Italic="False" Underline="False" Strikeout="False" />\
<ForeColor Alpha="255" Red="0" Green="0" Blue="0" HueScale="100" />\
</Attributes>\
</Element>\
</StyledText>\
</TextObject>\
<Bounds X="330" Y="' + currenPixels + '" Width="2210" Height="288" />\
</ObjectInfo>';
            }
            // open label
            var labelXml = '<?xml version="1.0" encoding="utf-8"?>\
<DieCutLabel Version="8.0" Units="twips">\
<PaperOrientation>Landscape</PaperOrientation>\
<Id>LW_DURABLE_25X89mm</Id>\
<IsOutlined>false</IsOutlined>\
<PaperName>1933081 Drbl 1 x 3-1/2 in</PaperName>\
<DrawCommands>\
<RoundRectangle X="0" Y="0" Width="1440" Height="5040" Rx="90.708661417" Ry="90.708661417" />\
</DrawCommands>\
' + PatInfo + PatAddress + PatInsuranceInfo + PatProvAcc + ' </DieCutLabel>';

            var label = dymo.label.framework.openLabelXml(labelXml);

            // set label text
            // label.setObjectText("PatInfo", DemographicLabelPatInsuranceInfo);
            if ($('#pnlDemographicLabel #lblPatInfo').text().trim().length != 0) {
                label.setObjectText("PatInfo", DemographicLabel.PatInfo);
            }
            if ($('#pnlDemographicLabel #lblPatAddress').text().trim().length != 0) {
                label.setObjectText("PatAddress", DemographicLabel.PatAddress.trim());
            }
            if ($('#pnlDemographicLabel #lblPatInsuranceInfo').text().trim().length != 0) {
                label.setObjectText("PatInsuranceInfo", DemographicLabel.PatInsuranceInfo);
            }
            if ($('#pnlDemographicLabel #lblPatProvAccount').text().trim().length != 0) {
                label.setObjectText("PatProvAccount", DemographicLabel.PatProvAccount);
            }
         //   label.setObjectText("DOB", "DOB: " + DemographicLabel.DOB);
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
            var docCnt = $('#pnlDemographicLabel #frmDemographicLabel').clone();
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

        DemographicLabel.checkedProblems = [];
        DemographicLabel.CPTCodeQA = [];
        var form = '#pnlDemographicLabel #frmClinicalLabOrderDetail';
        var saveButtonisHidden = $('#pnlDemographicLabel #frmClinicalLabOrderDetail #btnSaveOrder').hasClass("hidden");

        if (caller == 'saveExit' || saveButtonisHidden == true) {
            if (DemographicLabel.params["ParentCtrl"] == "Clinical_LabOrder") {
                UnloadActionPan(DemographicLabel.params["ParentCtrl"], "DemographicLabel", null, DemographicLabel.params["ParentCtrlPanelID"]);
            }
            else {
                UnloadActionPan(DemographicLabel.params["ParentCtrl"], "DemographicLabel");

            }
        }
        else {

            if (DemographicLabel.params["ParentCtrl"] == "Clinical_LabOrder") {
                UnloadActionPan(DemographicLabel.params["ParentCtrl"], "DemographicLabel", null, DemographicLabel.params["ParentCtrlPanelID"]);
            }
            else if (DemographicLabel.params["ParentCtrl"] == "Clinical_LabOrderView") {
                UnloadActionPan(DemographicLabel.params["ParentCtrl"]);
            }
            else {
                UnloadActionPan(DemographicLabel.params["ParentCtrl"], "DemographicLabel");

            }

        }
    },
}