uploadImage = {
    params: [],

    Load: function (params) {

        uploadImage.params = params;
        if (uploadImage.params.ParentCtrl.toLowerCase() == "pattabinsurance" || uploadImage.params.ParentCtrl.toLowerCase() == "patient_insurance") {
            $('#' + uploadImage.params.PanelID + ' #headerTitle').text("Upload Insurance Card");
            $('#' + uploadImage.params.PanelID + ' #containerWebcam').hide();
            $('#' + uploadImage.params.PanelID + ' #containerscaner').hide();

        }
        uploadImage.CheckPictureMode();
    },
    CheckPictureMode: function () {

        if (uploadImage.params.ParentCtrl.toLowerCase() == "pattabinsurance" || uploadImage.params.ParentCtrl.toLowerCase() == "patient_insurance") {
            $("#pnlUploadImage #Scanner").prop("checked", false);
            $("#pnlUploadImage #Picuture").prop("checked", true);
        }
        var chkValue = $('#pnlUploadImage input:radio[name=03]:checked').val();
        if (chkValue == "Picture") {
            $('#pnlUploadImage #uploadContainer').show();
            $('#pnlUploadImage #scanerContainer').hide();
            $('#pnlUploadImage #webcamContainer').hide();

        }
        else if (chkValue == "Webcam") {
            $('#pnlUploadImage #uploadContainer').hide();
            $('#pnlUploadImage #scanerContainer').hide();
            $('#pnlUploadImage #webcamContainer').show();
            uploadImage.LoadWebCam();
        }
        else if (chkValue == "Scanner") {
            if (uploadImage.params.ParentCtrl == "patTabDemographic" && Patient_Demographic.ScanPrivilige.toLowerCase() == "false") {
                utility.DisplayMessages("Practice is not selected or doesn't have privileges to Scan. Please contact your administrator.", 2);
            }
            else {
                $('#pnlUploadImage #uploadContainer').hide();
                $('#pnlUploadImage #webcamContainer').hide();
                $('#pnlUploadImage #scanerContainer').show();

                if (uploadImage.params.ParentCtrl == "patTabDemographic" || uploadImage.params.ParentCtrl == "demographicDetail")
                    $('#pnlUploadImage #btnScanProcessData').css("display", "");

                //$('#pnlUploadImage #modaldialog').attr("class", "modal-dialog-full modal-dialog");
                //var scannerparam = [];
                //var PanelID = null;
                //scannerparam["PanelID"] = "pnlUploadImage";
                //scannerparam["RefCtrl"] = "imageupload";
                //eval("Document_Scan.Load")(scannerparam);
                var param = [];
                var RefCtrl = null;
                var PanelID = uploadImage.params["PanelID"];

                if (uploadImage.params.ParentCtrl == "patTabDemographic") {
                    param["ParentCtrl"] = 'patTabDemographic';
                    param["RefCtrl"] = "patTabDemographic";
                }
                else if (uploadImage.params.ParentCtrl == "demographicDetail") {
                    param["ParentCtrl"] = 'demographicDetail';
                    param["RefCtrl"] = "demographicDetail";
                }
                //uploadImage.params["PreDocument_ScanParams"] = uploadImage.params;
                //Document_Scan.params = param;
                //LoadActionPan('Document_Scanner', param, PanelID);
                
                $("#pnlUploadImage #scanerContainer #IFDocumentScan").attr('src', "DocumentScan.aspx?pid=" + uploadImage.params["PracticeId"] +
                    "&PanelID=" + uploadImage.params["PanelID"] + "&ParentCtrl=" + param["ParentCtrl"] +
                    "&RefFill=" + param["ParentCtrl"] + "&RefCtrl=" + param["RefCtrl"] + "&IsFromUploadImage=true");
            }
        }

    },

    UnLoadUploadImage: function () {
        var base64 = $('#pnlUploadImage #frmUploadImage #scan_Imge').attr('src');
        uploadImage.setPatientNewImage(base64);
        uploadImage.UnLoad();

    },

    setImageUploadKey: function () {
        Patient_Demographic.IsImageUpdated = 'True';
    },

    BufferFile: function (input) {

        if (input.files.length > 0) {
            if (input.files) {
                if (Document_Import.ValidateFileSize(input.files) > 2) {
                    utility.DisplayMessages("Maximum 2MB  is allowed", 4);
                    $('#pnlUploadImage #Upload_Image_file').val('');
                    return false;
                }
                else {
                    var fileType = input.files[0].type.toLowerCase();
                    if (fileType == "image/jpeg" || fileType == "image/png" || fileType == "image/jpg" || fileType == "image/gif" || fileType == "image/bmp") {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $('#pnlUploadImage #imgUploadImage').attr('src', e.target.result);
                            $('#pnlUploadImage #imgUploadImage').show();
                        }

                        reader.readAsDataURL(input.files[0]);

                    }
                    else {
                        utility.DisplayMessages("only JPG,PNG,BMP,JPEG,GIF file is allowed", 2);
                        $('#pnlUploadImage #Upload_Image_file').val(null);
                        $('#pnlUploadImage #imgUploadImage').attr('src', "");
                        $('#pnlUploadImage #imgUploadImage').hide();
                    }
                }
            }

        }

    },
    SavePictureNew: function () {
      
        var dfd = new $.Deferred();
        var chkValue = $('#pnlUploadImage input:radio[name=03]:checked').val();
        var base64 = null;
        if (uploadImage.params.ParentCtrl == "patTabInsurance" || uploadImage.params.ParentCtrl == "Patient_Insurance") {
            if (chkValue == "Picture") {
                if ($('#Upload_Image_file').val() != "") {
                    base64 = $('#pnlUploadImage #imgUploadImage').attr('src');
                    Patient_Insurance.setImageSource(base64);
                    Patient_Insurance.InsuranceCardChanged = "true";
                    Patient_Demographic.params["isFromPictureMode"] = true;
                    $('#' + uploadImage.params.PanelID + ' #frmPatientInsurance  #hfImageChange').val("true");
                    dfd.resolve('ok');
                }
                else {
                    utility.DisplayMessages("Please Upload File First", 3);
                    return false;
                }
            }
        }
        else {
            if (chkValue == "Picture") {
                if ($('#Upload_Image_file').val() != "" || $('#pnlUploadImage #imgUploadImage').attr('src') !== "") {
                    base64 = $('#pnlUploadImage #imgUploadImage').attr('src');
                    Patient_Demographic.params["isFromPictureMode"] = true;
                    dfd.resolve('ok');
                }

                else {
                    utility.DisplayMessages("Please Upload File First", 3);
                    return false;
                }

            }
            else if (chkValue == "Webcam") {

                if ($('#pnlUploadImage #webCamImage').attr('src') != "") {
                    base64 = $('#pnlUploadImage #webCamImage').attr('src');
                    dfd.resolve('ok');
                    Patient_Demographic.params["isFromPictureMode"] = false;

                }
                else {
                    utility.DisplayMessages("Please Capture Image First", 3);
                    return false;
                }

            }

            dfd.then(function () {
                uploadImage.setPatientNewImage(base64);
            });

        }
        // Apply try catch for the bug no AST-28
        try {
            $("#webcam").scriptcam({
                disconnected: true,
            });
        }
        catch (ex) {
            console.log(ex);
        }
        
        uploadImage.UnLoad();

    },

    // PMS-4550 set Uploaded picture 
    setPatientNewImage: function (base64) {

        if (uploadImage.params.ParentCtrl == "demographicDetail") {
            demographicDetail.setImageSource(base64);
            Patient_Demographic.IsImageUpdated = 'True';
            if (demographicDetail.params["mode"] == "Edit") {
                demographicDetail.DemographicSave();
            }
        }
        else if (uploadImage.params.ParentCtrl == "patTabDemographic" || refscanner == true) {
            Patient_Demographic.setImageSource(base64);
            Patient_Demographic.IsImageUpdated = 'True';
            Patient_Demographic.DemographicSave();
        }
        else if (uploadImage.params.ParentCtrl == "StatementGroupDetail") {
            StatementGroupDetail.setImageSource(base64);
        }
    },

    UnLoad: function () {

        if (uploadImage.params != null && uploadImage.params.ParentCtrl != null) {
            var selectedTab = GetCurrentSelectedTab();
            var parentPanelID = selectedTab.PanelID;
            //Start 18-10-2016 Edit By Humaira Yousaf Bug# QAC2-374
            if (parentPanelID == 'mstrDivClinical') {
                UnloadActionPan(uploadImage.params.ParentCtrl, 'uploadImage');
            }
            else if (parentPanelID == 'mstrDivBilling') {
                UnloadActionPan(uploadImage.params.ParentCtrl, 'uploadImage');
            }
            else {
                UnloadActionPan(uploadImage.params.ParentCtrl, 'uploadImage', null, parentPanelID);
            }
            //End 18-10-2016 Edit By Humaira Yousaf Bug# QAC2-374
        }
        else
            UnloadActionPan(null);
    },

    UploadImage: function () {

        Document_Scan.params["RefFill"] = uploadImage.params["ParentCtrl"];
        Document_Scan.ScanDrivingLicense();

    },

    // __________________________________WebCam Code_________________________________________________________//
    LoadWebCam: function () {
        $("#webcam").scriptcam({
            showMicrophoneErrors: false,
            onError: uploadImage.onError,
            cornerRadius: 20,
            disableHardwareAcceleration: 1,
            cornerColor: 'e3e5e2',
            onWebcamReady: uploadImage.onWebcamReady,
            // uploadImage: 'upload.gif',
            onPictureAsBase64: uploadImage.base64_tofield_and_image
        });
        $('#actionPanDemographic').removeClass('fade in');

    },
    base64_toimage: function () {
        $('#webCamImage').attr("src", "data:image/png;base64," + $.scriptcam.getFrameAsBase64());
    },
    base64_tofield_and_image: function (b64) {

        $('#webCamImage').attr("src", "data:image/png;base64," + b64);
    },
    changeCamera: function () {
        $.scriptcam.changeCamera($('#cameraNames').val());
    },
    onError: function (errorId, errorMsg) {
        $("#btn1").attr("disabled", true);
        $("#btn2").attr("disabled", true);
        alert(errorMsg);
    },
    onWebcamReady: function (cameraNames, camera, microphoneNames, microphone, volume) {
        $.each(cameraNames, function (index, text) {
            $('#cameraNames').append($('<option></option>').val(index).html(text))
        });
        $('#cameraNames').val(camera);
    }

    // __________________________________END WebCam Code_________________________________________________________//
}

