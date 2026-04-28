OCR_Scanner = {
    params: [],
    myinterval: '',
    isProcessed: '0',
    isScanConnect: '0',
    scanCount: 0,
    imgStringFirst: '',
    isScanOnly: '0',
    isScanningComplete: '0',
    isNewData: '0',
    Load: function (params) {
        OCR_Scanner.params = params;
        OCR_Scanner.scanCount = 0;
        OCR_Scanner.imgStringFirst = '';
        OCR_Scanner.isScanOnly = '0';
        // OCR_Scanner.IsScannerInUse();


        //if (OCR_Scanner.isScanConnect == 1) {
        //    OCR_Scanner.InitLibrary().done(function () {
        //        OCR_Scanner.AutoDetectCard();
        //    });
        //}
        BackgroundLoaderShow(true);
        setTimeout(function () {
            OCR_Scanner.InitLibrary();
            BackgroundLoaderShow(false);
        }, 2000)

    },

    AutoDetectCard: function () {
        OCR_Scanner.CheckCardInsertion();
        //OCR_Scanner.CheckCardInsertion();
    },
    //function to uninitialize library
    UnInitLibrary: function () {
        var CSSNLibJ = OCR_Scanner.getCSSNLibJObj();//get cssn sdk object
        CSSNLibJ.UnInitSDK();//uninitialize cssn lib.
    },
    //get CSSN SDK Object embedded in Applet Tag.
    getCSSNLibJObj: function () {
        if (document.getElementById)//if element with id supported in browser
        {
            return document.getElementById("CSSNLibJ");//return cssn object embedded with id as CSSNLibJ in Applet Tag.
        }
        else {//otherwise
            return null;//return null object
        }
    },
    CheckCardInsertion: function () {
        var CSSNLibJ = OCR_Scanner.getCSSNLibJObj();
        if (CSSNLibJ.IsPaperOn() == 1)
        { OCR_Scanner.ScanCard(); }
        if (CSSNLibJ.GetPressedButton() >= 1)
        { OCR_Scanner.ScanCard(); }
    },
    IsScannerInUse: function () {

        try {
            var SDKLicenseKey = GetSDKLicenseKey();//set license key
            var CSSNLibJ = OCR_Scanner.getCSSNLibJObj();//get cssn sdk object
            //CSSNLibJ.DefaultScanner(13);
            var isScannerConnected = $("#pnlOCRScannerViewer #hdnIsScannerConnected").val();
            if (isScannerConnected == 0) {
                try {
                    var result = CSSNLibJ.InitScanLib(SDKLicenseKey);//initialize scan lib sdk with license key
                    if (result < 0 && result != -13) {//if error
                        if (result == -19) {
                            utility.DisplayMessages("Scanner already in use by other application. ", 3);
                        }
                        else
                            utility.DisplayMessages("Scanner Status: Not Connected", 3);
                    }
                    else {
                        $("#pnlOCRScannerViewer #btnConnect").addClass("disableAll");
                        OCR_Scanner.isScanConnect = 1;

                    }
                } catch (ex) {
                    utility.DisplayMessages("Scanner Status: Not Connected", 3);
                    //utility.DisplayMessages(ex, 3);
                    console.log(ex);
                }
            }
        }
        catch (ex) {
            console.log(ex);
        }
    },
    //function to initialize library
    InitLibrary: function () {
        $("#pnlOCRScannerViewer #errorMsg").hide();
        $("#pnlOCRScannerViewer #successMsg").hide();
        var SDKLicenseKey = GetSDKLicenseKey();//set license key
        var CSSNLibJ = OCR_Scanner.getCSSNLibJObj()//.done(function () { //get cssn sdk object
        //CSSNLibJ.DefaultScanner(13);
        var isScannerConnected = $("#pnlOCRScannerViewer #hdnIsScannerConnected").val();

        if (isScannerConnected == 0) {
            try {
                var result = CSSNLibJ.InitScanLib(SDKLicenseKey);//initialize scan lib sdk with license key
                if (result < 0) {//if error
                    if (result == -19) {
                        utility.DisplayMessages("Scanner already in use by other application. ", 3);
                    }
                    else if (result == -4) {
                        utility.DisplayMessages("Scanner Not Found", 3);
                    }
                    else if (result == -13) {
                        if (CSSNLibJ.IsNeedCalibration() == 1) {
                            if (CSSNLibJ.Calibrate() == 1) {
                                utility.DisplayMessages("Scanner is calibrated successfully", 1);
                            }
                            else {
                                utility.DisplayMessages("Calibration is required. Please insert calibration page.", 3);
                            }
                        }
                        else {
                            utility.DisplayMessages("Scanner Not Found", 3);
                        }
                    }
                    else {
                        utility.DisplayMessages("Scanner Status: Not Connected", 3);
                    }
                }
                else//if everything ok
                {
                    if (CSSNLibJ.IsNeedCalibration() == 1) {
                        if (CSSNLibJ.Calibrate() == 1) {
                            utility.DisplayMessages("Scanner is calibrated successfully", 1);
                        }
                        else {
                            utility.DisplayMessages("Calibration is required. Please insert calibration page.", 3);
                        }
                    }
                    else {
                        scannerName = GetScannerNameByType(CSSNLibJ.GetScannerType());
                        $("#pnlOCRScannerViewer #btnConnect").addClass("disableAll");

                        utility.DisplayMessages("Scanner Status: Connected", 1);

                        try {
                            result = CSSNLibJ.InitImageLib(SDKLicenseKey);//initialize image lib. with license kay
                            if (result < 0) {
                                utility.DisplayMessages("Scanner already in use by other application. ", 3);

                            }
                            else {
                                $("#pnlOCRScannerViewer #hdnIsScannerConnected").val(1);
                            }
                        } catch (ex) {
                            console.log(ex);
                        }
                        try {
                            result = CSSNLibJ.InitMedSdk(SDKLicenseKey);//initialize medical lib. with license key
                            if (result < 0) {
                                utility.DisplayMessages("Scanner already in use by other application. ", 3);

                            }
                        } catch (ex) {
                            console.log(ex);
                        }

                        $("#pnlOCRScannerViewer #btnScanCard").removeClass("btn btn-default disableAll").addClass("btn btn-default");
                        $("#pnlOCRScannerViewer #btnProcessCard").removeClass("btn btn-default disableAll").addClass("btn btn-default");
                        $("#pnlOCRScannerViewer #btnProcess").removeClass("btn btn-default disableAll").addClass("btn btn-default");
                        $("#pnlOCRScannerViewer #btnProcessCardNew").removeClass("btn btn-default disableAll").addClass("btn btn-default");
                        OCR_Scanner._ScannerTypeDetection();
                        var dfd = new $.Deferred();
                        dfd.resolve("1");
                        return dfd.promise();
                    }

                }
            } catch (ex) {
                console.log(ex);
                //if (ex.message == "CSSNLibJ.InitScanLib is not a function" || ex.message == "undefined is not a function" || ex.message == "Object doesn't support property or method 'InitScanLib'") {
                $('#pnlOCRScannerViewer #downloadPlugInContainer').removeClass('hidden');
                utility.DisplayMessages("Please install Following plugin.", 3);
                //}
                //else {
                //    utility.DisplayMessages("Please install CSSN SDK", 3);
                //}
            }
        }
        else {
            utility.DisplayMessages("Scanner Status: Already is Connected", 1);
        }
        // });

    },

    Calibrate: function () {
        var CSSNLibJ = OCR_Scanner.getCSSNLibJObj();
        CSSNLibJ.CalibrateEx();
    },

    _ScannerTypeDetection: function () {
        var CSSNLibJ = OCR_Scanner.getCSSNLibJObj();//get cssn sdk object
        var scanner_type = CSSNLibJ.GetScannerType();
        //For duplex scanning
        if (IsScannerDuplex(scanner_type) == true) {
            $('#pnlOCRScannerViewer #scanBothSideContainer').show();
            $('#pnlOCRScannerViewer #lbOneSide').show();
            $('#pnlOCRScannerViewer #lbBothSides').show();
            $('#pnlOCRScannerViewer #btnProcess').hide();
            $('#pnlOCRScannerViewer #btnProcessCard').show();

            document.getElementById("ProcessOneSide").disabled = false;
            document.getElementById("ProcessBothSides").disabled = false;
            document.getElementById("ProcessOneSide").checked = true;

            CSSNLibJ.SetDuplex(1);//set mode as duplex
            document.getElementById("cbscanBothSides").disabled = false;
            document.getElementById("cbscanBothSides").checked = true;
        }
            // For simplex scanning
        else {
            CSSNLibJ.SetDuplex(0);
            $('#pnlOCRScannerViewer #scanBothSideContainer').hide();
            document.getElementById("cbscanBothSides").checked = false;
            document.getElementById("cbscanBothSides").disabled = true;
            $('#pnlOCRScannerViewer #btnProcess').show();
            $('#pnlOCRScannerViewer #btnProcessCard').hide();
        }

        // Display a message if the scanner is non OCR
        if (IsScannerOCR(scanner_type) == false) {
            utility.DisplayMessages("You are using a Non-OCR scanner", 3);
            // alert("You are using a Non-OCR scanner. Images from a Non-OCR scanners are not being processed by the SDK.");
        }
    },

    //called when scan card button is clicked
    ScanCard: function () {
        $("#pnlOCRScannerViewer #errorMsg").hide();
        $("#pnlOCRScannerViewer #successMsg").hide();
        var CSSNLibJ = OCR_Scanner.getCSSNLibJObj();//get cssn sdk object
        if (CSSNLibJ.IsScannerValid() == 1) {
            if (CSSNLibJ.IsNeedCalibration() == 1) {
                CSSNLibJ.CalibrateEx();
            }

            var scanner_type = CSSNLibJ.GetScannerType();
            if (scanner_type != 14) {
                if (CSSNLibJ.GetPressedButton() >= 1) {
                    //document.getElementById("ProcessBothSides").disabled = true;
                    OCR_Scanner.scanCount++;
                }
                else {
                    utility.DisplayMessages("Please press scan button.", 3);
                    return;
                }
            }
            //else {
            //    document.getElementById("ProcessBothSides").disabled = false;
            //}
            if (OCR_Scanner.isScanOnly == '0')
                OCR_Scanner.Scanning();
            else {
                OCR_Scanner.ScanningAndProcessing();
            }
        }
        else {
            utility.DisplayMessages("Scanner not valid, please check connection to the scanner", 3);
        }

    },

    Scanning: function () {
        var CSSNLibJ = OCR_Scanner.getCSSNLibJObj();
        if (CSSNLibJ.IsPaperOn() > 0) {

            CSSNLibJ.SetResolution(300);//set scanner resolution to 300 dpi.
            CSSNLibJ.SetScanSize(-1, -1);//auto detect card size
            var result;
            if (OCR_Scanner.scanCount == 2)
                result = CSSNLibJ.ScanToFileBackJ("");//scan file to buffer & disk
            else
                result = CSSNLibJ.ScanToFileJ("");//scan file to buffer & disk
            if (result < 0)//if error
            {
                utility.DisplayMessages("There was no paper in the scanner.", 3);
                return;
            }
            else {
                OCR_Scanner.isProcessed = '1';
                utility.DisplayMessages("Card is scanned successfully", 1);
                //document.getElementById("lbOneSide").style.display = "block";
                //document.getElementById("lbBothSides").style.display = "block";

                // If the scanner is simplex disable the sides options

                //document.getElementById("ProcessOneSide").disabled = false;
                //document.getElementById("ProcessOneSide").checked = true;
                setTimeout(function () { OCR_Scanner.DrawImageOnCanvas(); }, 700);
                var scanner_type = CSSNLibJ.GetScannerType();
                if (scanner_type != 14 && OCR_Scanner.scanCount == 1) {
                    utility.myConfirm('8', function () {
                        if (CSSNLibJ.GetPressedButton() >= 1) {
                            //document.getElementById("ProcessBothSides").disabled = true;
                            OCR_Scanner.scanCount++;
                            result = CSSNLibJ.ScanToFileBackJ("");
                            setTimeout(function () { OCR_Scanner.DrawImageOnCanvas(); }, 700);
                            OCR_Scanner.UnLoadTab();
                        }
                        else {
                            utility.DisplayMessages("Please press scan button.", 3);
                            return;
                        }
                    }, function () { OCR_Scanner.UnLoadTab(); }, '8');
                }
            }
        }
        else {
            utility.DisplayMessages("There was no paper in the scanner.", 3);
        }
    },

    DrawImageOnCanvas: function () {

        var nxtHeight = 10;
        var drawingCanvas = document.getElementById("myDrawingCanvas");
        // Initaliase a 2-dimensional drawing context
        var context = drawingCanvas.getContext('2d');
        //Canvas commands go here
        var imgObj = new Image();
        // Draw the front image on the canvas
        if (OCR_Scanner.scanCount == 1) {
            drawingCanvas.height = 700;
            OCR_Scanner.imgStringFirst = "data:image/jpg;base64," + CSSNLibJ.GetImageBufferDataBase64("jpg");
            imgObj.src = "data:image/jpg;base64," + CSSNLibJ.GetImageBufferDataBase64("jpg");
            context.save();
            context.drawImage(imgObj, 100, nxtHeight);
            context.restore();
        }
        else {
            imgObj.src = "data:image/jpg;base64," + CSSNLibJ.GetImageBufferDataBase64("jpg");
            context.save();
            context.drawImage(imgObj, 100, nxtHeight);
            context.restore();
        }
        //if (OCR_Scanner.scanCount == 2) {

        //}

        if (document.getElementById("cbscanBothSides").checked == true || OCR_Scanner.scanCount == 2) {
            if (OCR_Scanner.scanCount == 2) {
                drawingCanvas.height = 1429;
                imgObj.src = OCR_Scanner.imgStringFirst;
                context.save();
                context.drawImage(imgObj, 100, nxtHeight);
                context.restore();
            }
            imgObj.src = $('#pnlOCRScannerViewer #cardlineImage').attr('src');
            nxtHeight = nxtHeight + 680;
            context.save();
            context.drawImage(imgObj, 100, nxtHeight);
            context.restore();
            // Draw the Back image on the canvas
            if (OCR_Scanner.scanCount == 2)
                imgObj.src = "data:image/jpg;base64," + CSSNLibJ.GetImageBufferBackDataBase64("jpg");
            else
                imgObj.src = "data:image/jpg;base64," + CSSNLibJ.GetImageBufferBackDataBase64("jpg");
            nxtHeight = nxtHeight + 40;
            context.save();
            context.drawImage(imgObj, 100, nxtHeight);
            context.restore();
        }
        $("#pnlPatientInsurance #scanImage").html("<img class=\"img-responsive img-center \" src=\"" + drawingCanvas.toDataURL() + "\"></img>");
        document.getElementById('scanImage').innerHTML = "<img class=\"img-responsive img-center\" src= \"" + drawingCanvas.toDataURL() + "\"></img>";
        //alert(drawingCanvas.toDataURL());

    },

    ScanningAndProcessing: function () {

        var CSSNLibJ = OCR_Scanner.getCSSNLibJObj(); //get cssn lib object
        if (CSSNLibJ.IsPaperOn() > 0) {

            CSSNLibJ.SetResolution(300);//set scanner resolution to 300 dpi.
            CSSNLibJ.SetScanSize(-1, -1);//auto detect card size
            var result;
            if (OCR_Scanner.scanCount == 2)
                result = CSSNLibJ.ScanToFileBackJ("");//scan file to buffer & disk
            else
                result = CSSNLibJ.ScanToFileJ("");//scan file to buffer & disk
            if (result < 0)//if error
            {
                utility.DisplayMessages("There was no paper in the scanner.", 3);
                return;
            }
            else {
                OCR_Scanner.isProcessed = '1';
                utility.DisplayMessages("Card is scanned successfully", 1);
                //document.getElementById("lbOneSide").style.display = "block";
                //document.getElementById("lbBothSides").style.display = "block";

                // If the scanner is simplex disable the sides options

                //document.getElementById("ProcessOneSide").disabled = false;
                //document.getElementById("ProcessOneSide").checked = true;
                setTimeout(function () { OCR_Scanner.DrawImageOnCanvas(); }, 700);
                var scanner_type = CSSNLibJ.GetScannerType();
                if (scanner_type != 14 && OCR_Scanner.scanCount == 1) {
                    utility.myConfirm('8', function () {
                        if (CSSNLibJ.GetPressedButton() >= 1) {
                            //document.getElementById("ProcessBothSides").disabled = true;
                            OCR_Scanner.scanCount++;
                            result = CSSNLibJ.ScanToFileBackJ("");
                            setTimeout(function () { OCR_Scanner.DrawImageOnCanvas(); }, 700);
                            OCR_Scanner.ProcessingCard();
                        }
                        else {
                            utility.DisplayMessages("Please press scan button.", 3);
                            return;
                        }
                    }, function () {
                        OCR_Scanner.ProcessingCard();
                    });
                }
                else {
                    OCR_Scanner.ProcessingCard();
                }
            }
        }
        else {
            utility.DisplayMessages("There was no paper in the scanner.", 3);
        }
        var result;

    },

    ProcessingCard: function () {
        if (CSSNLibJ.GetDuplex() == 0) {
            // For simplex scanners
            //result = CSSNLibJ.ProcessMedical("", "", 0);//process medical card
            result = CSSNLibJ.ProcessMedicalSide(0, "", 0);
        }
        else {
            // For duplex scanners
            if (document.getElementById("ProcessOneSide").checked == true) {
                //If only one side is to be processed
                result = CSSNLibJ.ProcessMedicalSide(0, "", 0);
            }
            else {
                //If both sides are to be processed
                result = CSSNLibJ.ProcessMedical("", "", 0);//process medical card
            }
        }

        if (result >= 0)//if everything ok
        {

            utility.DisplayMessages("Card is processed successfully.", 1);
            // Extract all the fields whose count may be more than 1
            $("#pnlPatientInsurance #txtSubscriberID").val(CSSNLibJ.getMedMemberID());
            if (OCR_Scanner.isNewData == '1')
                $("#pnlPatientInsurance #txtInsurancePlan").val(CSSNLibJ.getMedPlanProvider());
            $("#pnlPatientInsurance #txtSubscriberGroupID").val(CSSNLibJ.getMedGroupNumber());
            $("#pnlPatientInsurance #txtVisitCopayment").val(CSSNLibJ.getMedCopayOV());
            $("#pnlPatientInsurance #txtFirstName").val(CSSNLibJ.getMedNameFirst());
            $("#pnlPatientInsurance #txtLastName").val(CSSNLibJ.getMedNameLast());
            $("#pnlPatientInsurance #txtMiddleInitial").val(CSSNLibJ.getMedNameMiddle());
            $("#pnlPatientInsurance #dtpCoverageDateFrom").val(CSSNLibJ.getMedEffectiveDate());
            $("#pnlPatientInsurance #dtpCoverageDateTo").val(CSSNLibJ.getMedDateOfBirth());
            OCR_Scanner.isProcessed = '1';
            // OCR_Scanner.UnLoadTab();
        }
        else {
            utility.DisplayMessages("There was an error while processing card.", 3);

        }

    },

    ProcessNewMedCard: function () {
        Patient_Insurance.AddInsurancePlan();
        OCR_Scanner.isNewData = '1';
        OCR_Scanner.ProcessMedCard();
    },
    //called when extract data button is clicked
    ProcessMedCard: function () {
        OCR_Scanner.isScanOnly = '1';
        OCR_Scanner.ScanCard();
    },

    //auto rotate image by angle 'angle'

    S4: function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    },
    guid: function () {
        return (OCR_Scanner.S4() + OCR_Scanner.S4() + "-" + OCR_Scanner.S4() + "-" + OCR_Scanner.S4() + "-" + OCR_Scanner.S4() + "-" + OCR_Scanner.S4() + OCR_Scanner.S4() + OCR_Scanner.S4());
    },
    UnLoadTab: function () {
        if (OCR_Scanner.params != null && OCR_Scanner.params.ParentCtrl != null) {
            try {
                OCR_Scanner.UnInitLibrary();
            } catch (ex) {
                console.log(ex);
            }
            UnloadActionPan(OCR_Scanner.params.ParentCtrl, 'OCR_Scanner');
        }
        else {
            try {
                OCR_Scanner.UnInitLibrary();
            } catch (ex) {
                console.log(ex);
            }
            UnloadActionPan(null, 'OCR_Scanner');
        }
    },
}

