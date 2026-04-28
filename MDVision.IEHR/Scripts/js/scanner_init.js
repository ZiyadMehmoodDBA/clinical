var _iLeft, _iTop, _iRight, _iBottom; //These variables are used to remember the selected area

function pageonload() {

    InitMessageBody();
    initMessageBox(false);  //Messagebox
    initCustomScan();       //CustomScan

    initiateInputs();
}


function InitMessageBody() {

    //var MessageBody = document.getElementById("divNoteMessage");
    //if (MessageBody) {
    //    var ObjString = "<div class='divinput' style='line-height: 18px;background-color: #C1E5F0;border-radius: 4px;color: #30768D;'><b style='color: #1E586B;'>Note:</b> This online demo application uses JavaScript + ASP.NET (C#). If you use other server-side language (JSP, PHP, ASP.NET (VB.NET), ASP etc.), you can visit the ";
    //    ObjString += "<a href='http://www.dynamsoft.com/Downloads/WebTWAIN-Sample-Download.aspx' target='_blank' >sample gallery page";
    //    ObjString += "</a>";
    //    ObjString += ".</div>";

    //    MessageBody.style.display = "";
    //    MessageBody.innerHTML = ObjString;
    //}
}

function initMessageBox(bNeebBack) {
    var objString = "";

    // The container for navigator, view mode and remove button
    //Start 25-08-2016 Humaira Yousaf to prevent document from saving on pages navigation
    objString += "<div style='text-align:center; background-color:#FFFFFF;'>";
    objString += "<div class='col-xs-12'><div class='row'><div class='inline-block'style='width: 300px'><div class='row'>";
    objString += "<div class='col-xs-4 pr-none mt-lg'><button type='button' id='DW_btnFirstImage' onclick='btnFirstImage_onclick()' class='btn btn-default btn-sm mr-xs'><i class='fa fa-fast-backward'></i></button>";
    objString += "<button type='button' onclick='btnPreImage_onclick()' type='button' id='DW_btnPreImage' class='btn btn-default btn-sm'><i class='fa fa-play fa-flip-horizontal'></i></button></div>";
    objString += "<div class='col-xs-6 pl-xs pr-xs size120 pad-a-labelsize'><div class='col-xs-5 p-none'><input type='text' readonly='readonly' id='DW_CurrentImage' class='form-control' style='font-size: 14px; height: 30px; margin-top: -3px'></div><div class='col-xs-2 p-none'><p class='lead'>/</p></div>";
    objString += "<div class='col-xs-5 p-none'><input type='text' class='form-control' id='DW_TotalImage' readonly='readonly' style='height: 30px;font-size: 14px; margin-top: -3px'></div></div>";
    objString += "<div class='col-xs-3 pl-none pr-none mt-lg'><button type='button' class='btn btn-default btn-sm pull-left' id='DW_btnNextImage' onclick='btnNextImage_onclick()'><i class='fa fa-play'></i></button>";
    objString += "<button type='button' class='btn btn-default btn-sm' type='button' id='DW_btnLastImage' onclick='btnLastImage_onclick()'><i class='fa fa-fast-forward'></i></button></div></div></div>";

    objString += "<div class='clearfix'></div><div class='inline-block'><div class='pull-left size90 mr-sm'>Preview Mode";
    objString += "<select class='form-control' id='DW_PreviewMode' onchange ='setlPreviewMode();' style='height: 30px;margin-top: -1px;font-size: 14px;margin-left: 5px;'>";
    objString += "    <option value='0'>1X1</option>";
    objString += "</select><br /></div>";
    objString += "<div style='width: 400px' class='mt-lg'>";
    //objString += "<div style='width: 400px' class='mt-lg'><button  onclick='btnRemoveCurrentImage_onclick()' id='DW_btnRemoveCurrentImage' class='btn btn-sm btn-default pull-left'>Remove Selected Images</button>";
    //End 25-08-2016 Humaira Yousaf to prevent document from saving on pages navigation
    if (bNeebBack) {
        objString += "<button type='button' onclick='btnRemoveAllImages_onclick()' id='DW_btnRemoveAllImages' class='btn btn-sm btn-default'>Remove All</button>";
        objString += "<span style=\"font-size:larger\"><a href =\"online_demo_list.aspx\"><b>Back</b></a></span></div>";
    }
    else {
        objString += "<button type='button' onclick='btnRemoveAllImages_onclick()' id='DW_btnRemoveAllImages' class='btn btn-sm btn-default'>Remove All</button></div>";
    }
    objString += "</div></div></div>";

    //objString += "<div style='text-align:center; width:580px; background-color:#FFFFFF;display:block'>";
    //objString += "<div style='position:relative; background:white; float:left; width:430px; height:35px;'>";
    //objString += "<input id='DW_btnFirstImage' onclick='btnFirstImage_onclick()' type='button' value=' |&lt; '/>&nbsp;";
    //objString += "<input id='DW_btnPreImage' onclick='btnPreImage_onclick()' type='button' value=' &lt; '/>&nbsp;&nbsp;";
    //objString += "<input type='text' size='2' id='DW_CurrentImage' readonly='readonly'/>/";
    //objString += "<input type='text' size='2' id='DW_TotalImage' readonly='readonly'/>&nbsp;&nbsp;";
    //objString += "<input id='DW_btnNextImage' onclick='btnNextImage_onclick()' type='button' value=' &gt; '/>&nbsp;";
    //objString += "<input id='DW_btnLastImage' onclick='btnLastImage_onclick()' type='button' value=' &gt;| '/></div>";
    //objString += "<div style='position:relative; background:white; float:left; width:150px; height:35px;'>Preview Mode";
    //objString += "<select size='1' id='DW_PreviewMode' onchange ='setlPreviewMode();'>";
    //objString += "    <option value='0'>1X1</option>";
    //objString += "</select><br /></div>";
    //objString += "<div><input id='DW_btnRemoveCurrentImage' onclick='btnRemoveCurrentImage_onclick()' type='button' value='Remove Selected Images'/>";
    //if (bNeebBack) {
    //    objString += "<input id='DW_btnRemoveAllImages' onclick='btnRemoveAllImages_onclick()' type='button' value='Remove All'/><br /><br />";
    //    objString += "<span style=\"font-size:larger\"><a href =\"online_demo_list.aspx\"><b>Back</b></a></span><br /></div>";
    //}
    //else {
    //    objString += "<input id='DW_btnRemoveAllImages' onclick='btnRemoveAllImages_onclick()' type='button' value='Remove All'/><br /></div>";
    //}
    //objString += "</div>";

    // The container for the error message
    //objString += "<div id='DWTdivMsg'>";
    //objString += "Message:<br />"
    //objString += "<div id='DWTemessage' style='width:580px;height:80px; overflow:scroll; background-color:#ffffff; border:1px #303030; border-style:solid; text-align:left; position:relative' >";
    //objString += "</div></div>";

    var DWTemessageContainer = document.getElementById("DWTemessageContainer");
    DWTemessageContainer.innerHTML = objString;

    // Fill the init data for preview mode selection
    var varPreviewMode = document.getElementById("DW_PreviewMode");
    varPreviewMode.options.length = 0;
    varPreviewMode.options.add(new Option("1X1", 0));
    varPreviewMode.options.add(new Option("2X2", 1));
    varPreviewMode.options.add(new Option("3X3", 2));
    varPreviewMode.options.add(new Option("4X4", 3));
    varPreviewMode.options.add(new Option("5X5", 4));
    varPreviewMode.selectedIndex = 0;

    //var _divMessageContainer = document.getElementById("DWTemessage");
    //_divMessageContainer.ondblclick = function () {
    //    this.innerHTML = "";
    //    _strTempStr = "";
    //}

}

function initCustomScan() {
    var ObjString = "<div class='clearfix'></div><ul id='divTwainType' class='list-unstyled pt-xs'> ";
    ObjString += "<li style=''>";
    ObjString += "<div class='pull-left pl-default pr-default'><div class='checkbox-custom checkboxTiny' >  <input class='form-control' type='checkbox' id='ShowUI'><label class='control-label' for='ShowUI' id ='lblShowUI'>Show UI</label>  </div> </div>  ";
    ObjString += "<div class='pull-left pl-default pr-default'><div class='checkbox-custom checkboxTiny' >  <input class='form-control' type='checkbox' id='ADF'><label class='control-label' for='ADF' >AutoFeeder</label>  </div> </div>  ";
    ObjString += "<div class='pull-left pl-default pr-default'><div class='checkbox-custom checkboxTiny' >  <input class='form-control' type='checkbox' id='Duplex'><label class='control-label' for='Duplex' >Duplex</label>  </div> </div>  ";

    ObjString += "<li class='hidden'><div class='col-xs-12 mt-sm'>Pixel Type:</div>";
    ObjString += "<div class='col-xs-3'><div class='radio-custom'> <input type='radio' id='BW' name='PixelType'><label for='BW'>B&amp;W</label></div></div>";
    ObjString += "<div class='col-xs-3'><div class='radio-custom'> <input type='radio' id='Gray' name='PixelType'><label for='Gray'>Gray</label></div></div>";
    ObjString += "<div class='col-xs-3'><div class='radio-custom'> <input type='radio' id='RGB' name='PixelType'><label for='RGB'>Color</label></div></div></li>";

    //ObjString += "<label for='BW'><input type='radio' id='BW' name='PixelType'/>B&amp;W </label>";
    //ObjString += "<label for='Gray'><input type='radio' id='Gray' name='PixelType'/>Gray</label>";
    //ObjString += "<label for='RGB'><input type='radio' id='RGB' name='PixelType'/>Color</label></li>";

    //ObjString += "<li style='padding-left:15px;margin-top:20px;'>";
    //ObjString += "<label for='Resolution'>Resolution:<select size='1' id='Resolution'><option value = ''></option></select></label></li>";

    ObjString += "<li>";
    ObjString += "<div class='col-xs-12 hidden'><label class='control-label' for='Resolution'>Resolution</label><select id='Resolution' class='form-control'><option value=''></option></select></div></li>";
    ObjString += "</ul>";


    //var ObjString = "<div class='clearfix'></div><ul id='divTwainType' class='list-unstyled mt-sm'> ";
    //ObjString += "<li style=''>";
    //

    //ObjString += "<label id ='lblShowUI' for = 'ShowUI'><input type='checkbox' id='ShowUI' />Show UI&nbsp;</label>";
    //ObjString += "<label for = 'ADF'><input type='checkbox' id='ADF' />AutoFeeder&nbsp;</label>";
    //ObjString += "<label for = 'Duplex'><input type='checkbox' id='Duplex'/>Duplex</label></li>";
    //ObjString += "<li style='padding-left:15px;'>Pixel Type:";
    //ObjString += "<label for='BW'><input type='radio' id='BW' name='PixelType'/>B&amp;W </label>";
    //ObjString += "<label for='Gray'><input type='radio' id='Gray' name='PixelType'/>Gray</label>";
    //ObjString += "<label for='RGB'><input type='radio' id='RGB' name='PixelType'/>Color</label></li>";
    //ObjString += "<li style='padding-left:15px;'>";
    //ObjString += "<label for='Resolution'>Resolution:<select size='1' id='Resolution'><option value = ''></option></select></label></li>";
    //ObjString += "</ul>";

    document.getElementById("divProductDetail").innerHTML = ObjString;

    var vResolution = document.getElementById("Resolution");
    vResolution.options.length = 0;
    vResolution.options.add(new Option("100", 100));
    vResolution.options.add(new Option("150", 150));
    vResolution.options.add(new Option("200", 200));
    vResolution.options.selectedIndex = 2;
    //vResolution.options.add(new Option("300", 300));
}

function initiateInputs() {

    var allinputs = document.getElementById("pnlDocumentScan").getElementsByTagName("input");
    for (var i = 0; i < allinputs.length; i++) {
        if (allinputs[i].type == "checkbox") {
            allinputs[i].checked = false;
        }
        else if (allinputs[i].type == "text") {
            allinputs[i].value = "";
        }
    }

    if (!Dynamsoft.Lib.env.bWin) {
        document.getElementById("btnEditor").style.display = "none";
        document.getElementById("tblLoadImage").style.height = "170";
        document.getElementById("notformac1").style.display = "none";
    }

    if (Dynamsoft.Lib.env.bIE == true && Dynamsoft.Lib.env.bWin64 == true) {
        document.getElementById("samplesource64bit").style.display = "inline";
        document.getElementById("samplesource32bit").style.display = "none";
    }
}



function initDllForChangeImageSize() {

    var vInterpolationMethod = document.getElementById("InterpolationMethod");
    vInterpolationMethod.options.length = 0;
    vInterpolationMethod.options.add(new Option("NearestNeighbor", 1));
    vInterpolationMethod.options.add(new Option("Bilinear", 2));
    vInterpolationMethod.options.add(new Option("Bicubic", 3));

}

function setDefaultValue() {
    document.getElementById("RGB").checked = true;

    var varImgTypejpeg2 = document.getElementById("imgTypejpeg2");
    if (varImgTypejpeg2)
        varImgTypejpeg2.checked = true;

    var _strDefaultSaveImageName = "";
    var _txtFileNameforSave = document.getElementById("txt_fileNameforSave");
    _txtFileNameforSave.value = _strDefaultSaveImageName;

    var _txtFileName = document.getElementById("txt_fileName");
    _txtFileName.value = _strDefaultSaveImageName;

    var _chkMultiPageTIFF_save = document.getElementById("MultiPageTIFF_save");
    _chkMultiPageTIFF_save.disabled = true;
    var _chkMultiPagePDF_save = document.getElementById("MultiPagePDF_save");
    _chkMultiPagePDF_save.disabled = true;
    var _chkMultiPageTIFF = document.getElementById("MultiPageTIFF");
    _chkMultiPageTIFF.disabled = true;
    var _chkMultiPagePDF = document.getElementById("MultiPagePDF");
    _chkMultiPagePDF.disabled = true;

    var varImgTypepdf = document.getElementById("imgTypepdf");
    if (varImgTypepdf) {
        varImgTypepdf.checked = true;
        rdPDFsave_onclick();
        $('#MultiPagePDF_save')[0].checked = true;
    }
}


var DWObject;            // The DWT Object
// Check if the control is fully loaded.
function Dynamsoft_OnReady() {

    //Set Canvas Height
    var h1_ = 250;
    var h2_ = 237;
    if (Document_Scan.params && Document_Scan.params["RefCtrl"] == "patientDemographic") {
        h1_ = 360;
        h2_ = 347;
    }
    if (Document_Scan.params && (Document_Scan.params["TabID"] == "Patient_DemographicQuick" || Document_Scan.params["IsScanFrom"] == "true")) {
        h1_ = 260;
        h2_ = 247;
    }
    else if (Document_Scan.params && Document_Scan.params["RefCtrl"] == "imageupload") {
        h1_ = 360;
        h2_ = 347;
    }
    else if (Document_Scan.params && Document_Scan.params["RefCtrl"] == "patTabInsurance") {
        h1_ = 200;
    }

    if (Document_Scan.params["IsFromIfram"] == true) {
        h1_ = (h1_ - h1_) + 40;
    }


    $("#frmDocumentScan #page-content-wrapper").children().first("div").css("height", $(window).height() - h1_);
    $("#frmDocumentScan #dwtcontrolContainer").css("height", $(window).height() - h2_);
    $("#frmDocumentScan #dwtcontrolContainer").children().first("div").children().first("div").css("height", $(window).height() - h1_);
    $("#frmDocumentScan #dwtcontrolContainer").css("width", "100%");
    $("#frmDocumentScan #dwtcontrolContainer").parents(".dwtcontrolMain").css("width", "100%");
    $("#frmDocumentScan #dwtcontrolContainer").children().first("div").children().first("div").css("width", "100%");

    var liNoScanner = document.getElementById("pNoScanner");
    // If the ErrorCode is 0, it means everything is fine for the control. It is fully loaded.
    DWObject = Dynamsoft.WebTwainEnv.GetWebTwain('dwtcontrolContainer');
    if (DWObject) {
        if (DWObject.ErrorCode == 0) {
            DWObject.LogLevel = 1;
            DWObject.IfAllowLocalCache = true;

            if (!document.getElementById("source"))
                return;

            document.getElementById("source").options.length = 0;
            var vCount = DWObject.SourceCount;
            // If source list need to be displayed, fill in the source items.
            if (vCount == 0) {

                if (Dynamsoft.Lib.env.bWin) {
                    liNoScanner.style.display = "block";
                    liNoScanner.style.textAlign = "center";
                }
                else
                    liNoScanner.style.display = "none";
            }


            for (var i = 0; i < vCount; i++) {
                document.getElementById("source").options.add(new Option(DWObject.GetSourceNameItems(i), i));
            }


            if (vCount > 0) {
                source_onchange();
                var optVal = "";
                if (ScannerLoaded == "WIA-fi-7160") {
                    optVal = $("#source option:contains('PaperStream')").attr('value');
                    if (!optVal) {
                        optVal = $("#source option:contains('" + ScannerLoaded + "')").attr('value');
                    }
                }
                else if (ScannerLoaded == "ScanShell 800DX") {
                    //look into source if exact matched value exist
                    $("#source option").each(function () {
                        if ($(this).text() == 'ScanShell 800DX') {
                            optVal = $(this).val();
                        }
                    });
                    //if not exact match the name then use contain 
                    if (!optVal) {
                        optVal = $("#source option:contains('" + ScannerLoaded + "')").attr('value');
                    }

                }

                //var optVal = $("#source option:contains('" + ScannerLoaded + "')").attr('value');
                if (typeof optVal == "undefined" || optVal == "") {
                    optVal = 1;
                }
                $("#source").val(optVal);
            }

            if (Dynamsoft.Lib.env.bWin)
                DWObject.MouseShape = false;

            if (vCount == 0) {
                document.getElementById("btnScan").disabled = true;
                if (document.getElementById("btnScanProcessData"))
                    document.getElementById("btnScanProcessData").disabled = true;
                if (document.getElementById("btnScanProcess"))
                    document.getElementById("btnScanProcess").disabled = true;
                if (document.getElementById("btnScanProcessWithInsurance"))
                    document.getElementById("btnScanProcessWithInsurance").disabled = true;
                if (document.getElementById("btnScanProcessDemographic"))
                    document.getElementById("btnScanProcessDemographic").disabled = true;
            }
            else {
                document.getElementById("btnScan").disabled = false;
                document.getElementById("btnScan").style.color = "#FE8E14";
                if (document.getElementById("btnScanProcessData")) {
                    document.getElementById("btnScanProcessData").disabled = false;
                    document.getElementById("btnScanProcessData").style.color = "#FE8E14";
                }
                if (document.getElementById("btnScanProcess")) {
                    document.getElementById("btnScanProcess").disabled = false;
                    document.getElementById("btnScanProcess").style.color = "#FE8E14";
                }
                if (document.getElementById("btnScanProcessWithInsurance")) {
                    document.getElementById("btnScanProcessWithInsurance").disabled = false;
                    document.getElementById("btnScanProcessWithInsurance").style.color = "#FE8E14";
                }
                if (document.getElementById("btnScanProcessDemographic")) {
                    document.getElementById("btnScanProcessDemographic").disabled = false;
                    document.getElementById("btnScanProcessDemographic").style.color = "#FE8E14";
                }
            }

            if (!Dynamsoft.Lib.env.bWin && DWObject.ImageCaptureDriverType != 0) {
                if (document.getElementById("lblShowUI"))
                    document.getElementById("lblShowUI").style.display = "none";
                if (document.getElementById("ShowUI"))
                    document.getElementById("ShowUI").style.display = "none";
            }
            else {
                if (document.getElementById("lblShowUI"))
                    document.getElementById("lblShowUI").style.display = "";
                if (document.getElementById("ShowUI"))
                    document.getElementById("ShowUI").style.display = "";
            }

            initDllForChangeImageSize();
            //set default
            setTimeout(function () {
                setDefaultValue();
            }, 200);


            re = /^\d+$/;
            strre = /^[\s\w]+$/;
            refloat = /^\d+\.*\d*$/i;

            _iLeft = 0;
            _iTop = 0;
            _iRight = 0;
            _iBottom = 0;

            for (var i = 0; i < document.links.length; i++) {
                if (document.links[i].className == "ShowtblLoadImage") {
                    document.links[i].onclick = showtblLoadImage_onclick;
                }
                if (document.links[i].className == "ClosetblLoadImage") {
                    document.links[i].onclick = closetblLoadImage_onclick;
                }
            }
            if (vCount == 0) {
                if (Dynamsoft.Lib.env.bWin) {
                    document.getElementById("aNoScanner").style.color = "Red";
                    document.getElementById("aNoScanner").innerHTML = "<b>No TWAIN compatible drivers detected:<b/>";
                    document.getElementById("Resolution").style.display = "none";
                    showtblLoadImage_onclick();
                }

            }
            //else
            //  document.getElementById("divBlank").style.display = "none";

            updatePageInfo();
            ua = (navigator.userAgent.toLowerCase());
            if (!ua.indexOf("msie 6.0")) {
                ShowSiteTour();
            }

            DWObject.RegisterEvent("OnPostTransfer", Dynamsoft_OnPostTransfer);
            DWObject.RegisterEvent("OnPostLoad", Dynamsoft_OnPostLoadfunction);
            DWObject.RegisterEvent("OnPostAllTransfers", Dynamsoft_OnPostAllTransfers);
            DWObject.RegisterEvent("OnMouseClick", Dynamsoft_OnMouseClick);
            DWObject.RegisterEvent("OnImageAreaSelected", Dynamsoft_OnImageAreaSelected);
            DWObject.RegisterEvent("OnImageAreaDeSelected", Dynamsoft_OnImageAreaDeselected);
            DWObject.RegisterEvent("OnTopImageInTheViewChanged", Dynamsoft_OnTopImageInTheViewChanged);
            DWObject.ShowPageNumber = true;
            DWObject.AllowMultiSelect = true;
        }
    }
    customViewMode();
}


function showtblLoadImage_onclick() {
    switch (document.getElementById("tblLoadImage").style.display) {
        case "hidden": document.getElementById("tblLoadImage").style.display = "block";
            document.getElementById("Resolution").style.visibility = "hidden";
            break;
        case "visible":
            document.getElementById("tblLoadImage").style.display = "none";
            document.getElementById("Resolution").style.visibility = "visible";
            break;
        default: break;
    }
    document.getElementById("tblLoadImage").style.top = ds_gettop(document.getElementById("pNoScanner")) + pNoScanner.offsetHeight + "px";
    document.getElementById("tblLoadImage").style.left = ds_getleft(document.getElementById("pNoScanner")) + 0 + "px";
    return false;
}

function closetblLoadImage_onclick() {
    document.getElementById("tblLoadImage").style.dispaly = "none";
    document.getElementById("Resolution").style.visibility = "visible";
    return false;
}

//--------------------------------------------------------------------------------------
//************************** Used a lot *****************************
//--------------------------------------------------------------------------------------
function updatePageInfo() {

    try {
        document.getElementById("DW_TotalImage").value = DWObject.HowManyImagesInBuffer;
        var currentIndex = DWObject.CurrentImageIndexInBuffer + 1;
        if (DWObject._UIManager._UIView.aryImageControls.length > 1) {
            $(DWObject._UIManager._UIView.aryImageControls).each(function (index) {
                if ($(this)[0].bSelect)
                    currentIndex = $(this)[0].cIndex + 1;
            });
        }
        else if (DWObject._UIManager._UIView.aryImageControls.length == 1) {
            if (DWObject.HowManyImagesInBuffer < currentIndex) {
                currentIndex = currentIndex;
            }
            else
                currentIndex = DWObject._UIManager._UIView.aryImageControls[0].cIndex + 1;
        }
        document.getElementById("DW_CurrentImage").value = currentIndex;
    }
    catch (ex) {
        console.log(ex);
    }


    shoHideDocMoveBtn();
}


var _strTempStr = "";       // Store the temp string for display
function appendMessage(strMessage) {
    _strTempStr += strMessage;
    var _divMessageContainer = document.getElementById("DWTemessageContainer");
    if (_divMessageContainer) {
        _divMessageContainer.innerHTML = _strTempStr;
        _divMessageContainer.scrollTop = _divMessageContainer.scrollHeight;
    }
}

function checkIfImagesInBuffer() {
    if (DWObject.HowManyImagesInBuffer == 0) {
        appendMessage("There is no image in buffer.<br />")
        return false;
    }
    else
        return true;
}
function getErrorString() {
    return DWObject.ErrorString;
}
function checkErrorString() {
    return checkErrorStringWithErrorCode(DWObject.ErrorCode, DWObject.ErrorString);
}

function checkErrorStringWithErrorCode(errorCode, errorString, responseString) {
    if (errorCode == 0) {
        appendMessage("<span style='color:#cE5E04'><b>" + errorString + "</b></span><br />");

        return true;
    }
    if (errorCode == -2115) //Cancel file dialog
        return true;
    else {
        if (errorCode == -2003) {
            var ErrorMessageWin = window.open("", "ErrorMessage", "height=500,width=750,top=0,left=0,toolbar=no,menubar=no,scrollbars=no, resizable=no,location=no, status=no");
            ErrorMessageWin.document.writeln(responseString); //DWObject.HTTPPostResponseString);
        }
        appendMessage("<span style='color:#cE5E04'><b>" + errorString + "</b></span><br />");
        return false;
    }
}


//--------------------------------------------------------------------------------------
//************************** Used a lot *****************************
//--------------------------------------------------------------------------------------
function ds_getleft(el) {
    var tmp = el.offsetLeft;
    el = el.offsetParent
    while (el) {
        tmp += el.offsetLeft;
        el = el.offsetParent;
    }
    return tmp;
}
function ds_gettop(el) {
    var tmp = el.offsetTop;
    el = el.offsetParent
    while (el) {
        tmp += el.offsetTop;
        el = el.offsetParent;
    }
    return tmp;
}

function Over_Out_DemoImage(obj, url) {
    obj.src = url;
}


//window.onload = function() {
//    document.onscroll = MouseScroll;
//}

function MouseScroll(evt) {
    var mousewheelevt = (/Firefox/i.test(navigator.userAgent)) ? "DOMMouseScroll" : "mousewheel";
    if (document.attachEvent)
        document.attachEvent("on" + mousewheelevt, NavigateImages);
    else if (document.addEventListener);
    document.addEventListener(mousewheelevt, NavigateImages, false)
}

function NavigateImages(e) {
    evt = window.event || e;
    var delta = evt.detail ? evt.detail * (-120) : evt.wheelDelta;
    if (delta < 0)
        btnNextImage_wheel();
    else if (delta > 0)
        btnPreImage_wheel();
}

function stopWheel(evt) {
    if (!evt) { /* IE7, IE8, Chrome, Safari */
        evt = window.event;
    }
    if (evt.preventDefault) { /* Chrome, Safari, Firefox */
        var ret = evt.preventDefault();
    }
    evt.returnValue = false; /* IE7, IE8 */
}
function resetPageInfo() {
    try {
        document.getElementById("DW_TotalImage").value = DWObject.HowManyImagesInBuffer;
        document.getElementById("DW_CurrentImage").value = DWObject.CurrentImageIndexInBuffer + 1;
    }
    catch (ex) {
        console.log(ex);
    }

    shoHideDocMoveBtn();
}