Admin_Provider_eSignature = {
    params: [],

    Image_eSignature_div_via_Device_Id: "imageBox_forDevice_eSignature",

    // ------- eSignature Global Variables -----------
    wgssSignatureSDK: null,
    sigObj: null,
    sigCtl: null,
    dynCapt: null,
    signaturePad: null,
    SignatureType: 'Mouse',
    // ------- eSignature Global Variables -----------

    //Author: Talha Tanweer
    //Date  : 22/07/2016
    Load: function (params) {
        //Admin_Provider_eSignature.resizeCanvas();
        Admin_Provider_eSignature.params = params;

        Admin_Provider_eSignature.params.PanelID = " #" + Admin_Provider_eSignature.params.PanelID + ' #pnleSignatureDetail';
        Admin_Provider_eSignature.OnLoad();

        Admin_Provider_eSignature.setSavedImage_on_eSignatureForm();

        Admin_Provider_eSignature.InitializeCanvas();
    },

    InitializeCanvas: function () {
        var wrapper1 = document.getElementById("eSignatureMousepad"),
           canvas = wrapper1.querySelector("canvas");

        // Adjust canvas coordinate space taking into account pixel ratio,
        // to make it look crisp on mobile devices.
        // This also causes canvas to be cleared.
        function resizeCanvas() {

            // When zoomed out to less than 100%, for some very strange reason,
            // some browsers report devicePixelRatio as less than 1
            // and only part of the canvas is cleared then.

            var ratio = Math.max(window.devicePixelRatio || 1, 1);
            canvas.width = (canvas.offsetWidth > 314 ? canvas.offsetWidth : 315) * ratio;
            canvas.height = 125;// (canvas.offsetHeight > 124 ? canvas.offsetWidth : 125) * ratio;
            canvas.getContext("2d").scale(ratio, ratio);
        }

        window.onresize = resizeCanvas;
        resizeCanvas();

        Admin_Provider_eSignature.signaturePad = new SignaturePad(canvas, { minWidth: 0.7, maxWidth: 1.4 });
    },

    //Author: Talha Tanweer
    //Date  : 22/07/2016
    UnLoad: function (caller) {


        var parentContolId = "providerDetail";
        var controlId = "Admin_Provider_eSignature";
        //UnloadActionPan(parentContolId, controlId);

        UnloadActionPan(Admin_Provider_eSignature.params.ParentCtrl, controlId);

    },

    //Author: Talha Tanweer
    //Date  : 22/07/2016
    Create_eSignatureImage_via_Device: function () {
        Admin_Provider_eSignature.Capture();
    },

    //Author: Talha Tanweer
    //Date  : 22/07/2016
    BrowseScanned_eSignatureImage: function () {

    },

    //Author: Talha Tanweer
    //Date  : 22/07/2016
    Undo_eSignature: function () {
        if (Admin_Provider_eSignature.SignatureType == "Picture") {

            Admin_Provider_eSignature.InitializeCanvas();

            $(Admin_Provider_eSignature.params.PanelID + ' #Upload_Image_file').val('');
        }
        else if (Admin_Provider_eSignature.SignatureType == "Mouse") {
            Admin_Provider_eSignature.signaturePad.clear();
        }
    },

    //Author: Talha Tanweer
    //Date  : 22/07/2016
    Save_eSignature: function () { },

    //Author: Talha Tanweer
    //Date  : 22/07/2016
    BufferFile: function (input) {

        Admin_Provider_eSignature.SignatureType = "Picture";
        Admin_Provider_eSignature.InitializeCanvas();

        if (input.files) {
            if (utility.ValidateFileSize(input.files) > 2) {

                utility.DisplayMessages("Maximum 2MB  is allowed", 4);
                $(Admin_Provider_eSignature.params.PanelID + ' #Upload_Image_file').val('');
                return false;
            }
            else {
                var fileType = input.files[0].type.toLowerCase();

                if (fileType == "image/jpeg" || fileType == "image/png" || fileType == "image/jpg" || fileType == "image/gif" || fileType == "image/bmp") {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        Admin_Provider_eSignature.drawImagetoCanvas(e.target.result);
                    }

                    Admin_Provider_eSignature.SignatureType = "Picture";
                    reader.readAsDataURL(input.files[0]);
                }
                else {
                    utility.DisplayMessages("Only JPG, PNG, BMP, JPEG, GIF files are allowed.", 2);
                    $(Admin_Provider_eSignature.params.PanelID + ' #Upload_Image_file').val(null);

                    Admin_Provider_eSignature.drawImagetoCanvas("");
                }
            }
        }
    },

    //Author: Talha Tanweer
    //Date  : 26/07/2016
    SavePictureNew: function () {

        var chkValue = $('#pnleSignatureDetail input:radio[name=03]:checked').val();
        chkValue = "Picture";

        var base64 = null;
        var Image_placer_ID = "#pnleSignatureDetail #imgUploadImage";
        //var IS_Image_Exists = !($(Image_placer_ID).attr('src') === "" || $(Image_placer_ID).attr('src') === undefined
        //    || $(Image_placer_ID).attr('src') === null || $(Image_placer_ID).attr('src') === "null");
        //   var eSignature_via_BrowsePicture = $('#Upload_Image_file').val() != "";

        if (Admin_Provider_eSignature.params.ParentCtrl == "patTabInsurance" || Admin_Provider_eSignature.params.ParentCtrl == "Patient_Insurance") {
            //if (chkValue == "Picture") {
            //    if ($('#Upload_Image_file').val() != "") {
            //        base64 = $('#pnleSignatureDetail #imgUploadImage').attr('src');
            //        Patient_Insurance.setImageSource(base64);
            //    }
            //    else {
            //        utility.DisplayMessages("Please Upload File First", 3);
            //        return false;
            //    }
            //}
        }
        else {

            if (Admin_Provider_eSignature.SignatureType == "Picture") {

                base64 = document.getElementById('eSignatureMousepadCanvas').toDataURL();

                if (base64 == null) {
                    utility.DisplayMessages("Please Upload File First", 3);
                    return false;
                }
            }
            else if (Admin_Provider_eSignature.SignatureType == "Mouse") {
                base64 = Admin_Provider_eSignature.signaturePad.toDataURL('image/png');
                if (base64 == null) {
                    utility.DisplayMessages("Please add signature using mouse.", 3);
                    return false;
                }
            }

            /////////////start commented by humaira/////////

            //if (chkValue == "Picture") {
            //    if (IS_Image_Exists) {
            //        base64 = $(Image_placer_ID).attr('src');
            //    }
            //        //else if (IS_Image_Exists) {
            //        //    base64 = $(Image_placer_ID).attr('src');
            //        //}
            //    else {
            //        utility.DisplayMessages("Please Upload File First", 3);
            //        return false;
            //    }
            //}
            //else if (chkValue == "Webcam") {
            //    if ($('#pnleSignatureDetail #webCamImage').attr('src') != "") {
            //        base64 = $('#pnleSignatureDetail #webCamImage').attr('src');


            //    }
            //    else {
            //        utility.DisplayMessages("Please Capture Image First", 3);
            //        return false;
            //    }

            //}

            /////////////end commented by humaira/////////

            //if (Admin_Provider_eSignature.params.ParentCtrl == "demographicDetail") {
            //    demographicDetail.setImageSource(base64);
            //}
            //else if (Admin_Provider_eSignature.params.ParentCtrl == "patTabDemographic") {
            //    Patient_Demographic.setImageSource(base64);
            //}
            //else if (Admin_Provider_eSignature.params.ParentCtrl == "StatementGroupDetail") {
            //    StatementGroupDetail.setImageSource(base64);
            //}

            //else
            if (Admin_Provider_eSignature.params.ParentCtrl == "providerDetail") {
                providerDetail.setImageSource(base64);
            }
            else if (Admin_Provider_eSignature.params.ParentCtrl == "CCMAgreement") {
                CCMAgreement.setImageSource(base64);
            }
        }

        //////$("#webcam").scriptcam({
        //////    disconnected: true,
        //////});
        Admin_Provider_eSignature.UnLoad();

    },

    //Author: Talha Tanweer
    //Date  : 26/07/2016
    setSavedImage_on_eSignatureForm: function () {

        var savedImg_Src = $('#frmProviderDetail  #img_eSignature_Admin_Provider_Detail').attr('src');

        var IsImageExists = !(savedImg_Src === "" || savedImg_Src === undefined || savedImg_Src === null || savedImg_Src === "null");

        if (IsImageExists) {
        }
    },

    //-----------------------------------------------------------------------------------------------------------------------
    //---------------------------       eSignature Device Integration Work Started       ------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------


    //Author: Talha Tanweer
    //Date  : 26/07/2016
    print: function (txt) {
        utility.DisplayMessages(txt, 2);
    },

    //Author: Talha Tanweer
    //Date  : 26/07/2016
    OnLoad: function (callback) {
        // Admin_Provider_eSignature.print("CLEAR");
        Admin_Provider_eSignature.restartSession(callback);
    },

    //Author: Talha Tanweer
    //Date  : 26/07/2016
    restartSession: function (callback) {

        Admin_Provider_eSignature.wgssSignatureSDK = null;
        Admin_Provider_eSignature.sigObj = null;
        Admin_Provider_eSignature.sigCtl = null;
        Admin_Provider_eSignature.dynCapt = null;
        var imageBox = document.getElementById("imageBox");

        if (null != imageBox.firstChild) {
            imageBox.removeChild(imageBox.firstChild);
        }

        var timeout = setTimeout(timedDetect, 1500);
        // pass the starting service port  number as configured in the registry
        Admin_Provider_eSignature.wgssSignatureSDK = new Signaturesdk.WacomGSS_SignatureSDK(onDetectRunning, 8000);

        function timedDetect() {
            if (Admin_Provider_eSignature.wgssSignatureSDK.running) {
                start();
            }
            else {
                //    Admin_Provider_eSignature.print("Signature SDK Service not detected.");
            }
        }

        function onDetectRunning() {
            if (Admin_Provider_eSignature.wgssSignatureSDK.running) {
                clearTimeout(timeout);
                start();
            }
            else {
                //   Admin_Provider_eSignature.print("Signature SDK Service not detected.");
            }
        }

        function start() {
            if (Admin_Provider_eSignature.wgssSignatureSDK.running) {
                Admin_Provider_eSignature.sigCtl = new Admin_Provider_eSignature.wgssSignatureSDK.SigCtl(onSigCtlConstructor);
            }
        }

        function onSigCtlConstructor(sigCtlV, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                Admin_Provider_eSignature.dynCapt = new Admin_Provider_eSignature.wgssSignatureSDK.DynamicCapture(onDynCaptConstructor);
            }
            else {
                Admin_Provider_eSignature.print("SigCtl constructor error: " + status);
            }
        }

        function onDynCaptConstructor(dynCaptV, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                Admin_Provider_eSignature.sigCtl.GetSignature(onGetSignature);
            }
            else {
                Admin_Provider_eSignature.print("DynCapt constructor error: " + status);
            }
        }

        function onGetSignature(sigCtlV, sigObjV, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                Admin_Provider_eSignature.sigObj = sigObjV;
                Admin_Provider_eSignature.sigCtl.GetProperty("Component_FileVersion", onSigCtlGetProperty);
            }
            else {
                Admin_Provider_eSignature.print("SigCapt GetSignature error: " + status);
            }
        }

        function onSigCtlGetProperty(sigCtlV, property, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                //Admin_Provider_eSignature.print("DLL: flSigCOM.dll  v" + property.text);
                Admin_Provider_eSignature.dynCapt.GetProperty("Component_FileVersion", onDynCaptGetProperty);
            }
            else {
                Admin_Provider_eSignature.print("SigCtl GetProperty error: " + status);
            }
        }

        function onDynCaptGetProperty(dynCaptV, property, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {
                //Admin_Provider_eSignature.print("DLL: flSigCapt.dll v" + property.text);
                //Admin_Provider_eSignature.print("Test application ready.");
                //Admin_Provider_eSignature.print("Press 'Start' to capture a signature.");
                if ('function' === typeof callback) {
                    callback();
                }
            }
            else {
                Admin_Provider_eSignature.print("DynCapt GetProperty error: " + status);
            }
        }
    },


    //Author: Talha Tanweer
    //Date  : 26/07/2016
    Capture: function () {
        if (!Admin_Provider_eSignature.wgssSignatureSDK.running || null == Admin_Provider_eSignature.dynCapt) {
            //Admin_Provider_eSignature.print("Session error. Restarting the session.");
            Admin_Provider_eSignature.print("Device not Detected!");
            Admin_Provider_eSignature.restartSession(Admin_Provider_eSignature.Capture);
            return;
        }

        Admin_Provider_eSignature.dynCapt.Capture(Admin_Provider_eSignature.sigCtl, "who", "why", null, null, onDynCaptCapture);

        function onDynCaptCapture(dynCaptV, SigObjV, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.INVALID_SESSION == status) {
                //Admin_Provider_eSignature.print("Error: invalid session. Restarting the session.");
                Admin_Provider_eSignature.restartSession(Admin_Provider_eSignature.Capture);
            }
            else {
                if (Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptOK != status) {
                    // Admin_Provider_eSignature.print("Capture returned: " + status);
                }
                switch (status) {
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptOK:
                        Admin_Provider_eSignature.sigObj = SigObjV;
                        //Admin_Provider_eSignature.print("Signature captured successfully");
                        utility.DisplayMessages("Signature captured successfully", 1);
                        Admin_Provider_eSignature.SignatureType = "Picture";
                        var flags = Admin_Provider_eSignature.wgssSignatureSDK.RBFlags.RenderOutputBase64 |
                                    Admin_Provider_eSignature.wgssSignatureSDK.RBFlags.RenderColor24BPP;
                        var imageBox = document.getElementById("imageBox");
                        //   Admin_Provider_eSignature.sigObj.RenderBitmap("bmp", imageBox.clientWidth, imageBox.clientHeight, 0.7, 0x00000000, 0x00FFFFFF, flags, 0, 0, onRenderBitmap);
                        Admin_Provider_eSignature.sigObj.RenderBitmap("bmp", "315px", "125px", 0.7, 0x00000000, 0x00FFFFFF, flags, 0, 0, onRenderBitmap);
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptCancel:
                        Admin_Provider_eSignature.print("Signature capture cancelled");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptPadError:
                        Admin_Provider_eSignature.print("No capture service available");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptError:
                        Admin_Provider_eSignature.print("Tablet Error");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptIntegrityKeyInvalid:
                        Admin_Provider_eSignature.print("The integrity key parameter is invalid (obsolete)");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptNotLicensed:
                        Admin_Provider_eSignature.print("No valid Signature Capture licence found");
                        break;
                    case Admin_Provider_eSignature.wgssSignatureSDK.DynamicCaptureResult.DynCaptAbort:
                        Admin_Provider_eSignature.print("Error - unable to parse document contents");
                        break;
                    default:
                        Admin_Provider_eSignature.print("Capture Error " + status);
                        break;
                }
            }
        }

        function onRenderBitmap(sigObjV, bmpObj, status) {
            if (Admin_Provider_eSignature.wgssSignatureSDK.ResponseStatus.OK == status) {

                Admin_Provider_eSignature.drawImagetoCanvas(bmpObj.image.src);
            }
            else {
                Admin_Provider_eSignature.print("Signature Render Bitmap error: " + status);
            }
        }

    },

    drawImagetoCanvas: function (source) {

        var ctx = document.getElementById('eSignatureMousepadCanvas').getContext('2d');
        var img = new Image;

        img.onload = function () {
            ctx.drawImage(img, 0, 0, 315, 125); // Or at whatever offset you like
        };
        img.width = 315;
        img.height = 125;
        img.src = source;

        ctx.height = 125;
        ctx.width = 315;
    },

    //-----------------------------------------------------------------------------------------------------------------------
    //----------------------------       eSignature Device Integration Work Ended       -------------------------------------
    //-----------------------------------------------------------------------------------------------------------------------
}
