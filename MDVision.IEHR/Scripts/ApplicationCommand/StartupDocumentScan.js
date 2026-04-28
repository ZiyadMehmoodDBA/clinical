function OpenDocumentScan() {

    if (globalAppdata.DefaultThemeName == "Gray") {
        $("html").css("background-color", "#eee");
    }

    ScannerLoaded = "ScanShell 800DX";
    var pid = getParameterByName("pid");
    practiceDetail.DemographicPractice(pid).done(function (response) {
        if (response.status != false) {
            var medication_detail = JSON.parse(response.PracticeFill_JSON);
            ScanPrivilige = medication_detail.chkScan;
            OCRPrivilige = medication_detail.chkOCR;
            if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                Patient_Demographic.ScanPrivilige = medication_detail.chkScan;
                Patient_Demographic.OCRPrivilige = medication_detail.chkOCR;
            }
        }
        else {
            ScanPrivilige = false;
            OCRPrivilige = false;
            if (Patient_Demographic.params.PanelID == "pnlDemographic"); {
                Patient_Demographic.ScanPrivilige = false;
                Patient_Demographic.OCRPrivilige = false;
            }
        }
        if (ScanPrivilige == "True") {
            var param = [];
            var PanelID = getParameterByName("PanelID");
            param["ParentCtrl"] = getParameterByName("ParentCtrl");
            param["RefCtrl"] = getParameterByName("RefCtrl");// "patTabInsurance";
            param["RefFill"] = getParameterByName("RefFill");
            param["IsFromIfram"] = true;
            param["IsFromUploadImage"] = getParameterByName("IsFromUploadImage");

            //set Values
            $("#pnlDemographic #frmDemographic #hfPractice").val(pid);


            LoadActionPan('Document_Scan', param, PanelID);
        }
        else {
            utility.DisplayMessages("Practice does not have the privileges to Scan document. Please contact your administrator", 2);
        }
    });
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}


var TabsArray = [];
var params = [];
params['patientID'] = "-1";
params["FromAdmin"] = "1";
params["PreviousTab"] = null;
var IsActionPanPatientLoaded = false
var PatientArray = [];
var globalAppdata = [];
var DefaultUser = "MDVISION";
var LoadedPatientControlsArray = [];
var date_format = "dd/mm/yyyy";
var adminTabs = null;
var billingTabs = null;
var patientTabs = null;
var auditbleEventsTab = null;
var IsBackgroundLoaderShow = true;
var LoadedEncounterTabs = [];
var CurrentParentmenu = null;
var xhrPool = []; // to keep Ajax calls.
var session = null;
var LastSocialHx = null;
var ScanPrivilige = false;
var OCRPrivilige = false;
var SchFirstLoad = true;
var isPopUpfax = false;
var isFileCompressed = false;
var ScannerLoaded = "ScanShell 800DX";
var UserRecentAccessedNoteComponent = "";
var IsRemoveNoteComponent = true;
var BillingFirstLoad = true;
var drfdAutoSave = '';
var DefaultMenuSelected = "MDVisionDefault";

function RefreshWindowOnEntityChange() {

}
function sess_Reset(TimeOUt) {
    sess_lastActivity = new Date();
}

document.onkeypress = sess_Reset;
document.onmousedown = sess_Reset;

(function ($) {
    //var xhrPool = [];
    $(document).ajaxSend(function (e, jqXHR, options) {
        if (IsBackgroundLoaderShow == true) {
            xhrPool.push(jqXHR);
            BackgroundLoaderShow(true);
            //  console.log( xhrPool.length);
        }
        jqXHR.then(function () {
            xhrPool = $.grep(xhrPool, function (x) { return x != jqXHR });
            BackgroundLoaderShow(false);
        });

    });
    $(document).ajaxComplete(function (e, jqXHR, options) {
        xhrPool = $.grep(xhrPool, function (x) { return x != jqXHR });
        BackgroundLoaderShow(false);
    });
    $(document).ajaxError(function (e, jqXHR, options) {
        xhrPool = $.grep(xhrPool, function (x) { return x != jqXHR });
        BackgroundLoaderShow(false);
    });
    $(document).ajaxSuccess(function (e, jqXHR, options) {
        xhrPool = $.grep(xhrPool, function (x) { return x != jqXHR });
        BackgroundLoaderShow(false);
    });

})(jQuery);

function BackgroundLoaderShow(bShow) {

    // finish loader only when there is no more Ajax call.
    if (xhrPool.length <= 0)
        bShow = false;
    else
        bShow = true;

    if (bShow) {

        $('#BackgroundLoader').show();
    } else {
        $('#BackgroundLoader').hide();

    }
    //  $('#BackgroundLoader').hide();
}

function SetGlobalAppData(id, data) {
    globalAppdata[id] = data;
}

function LoadActionPan(ctrl, param, ParentCtrlPanelID, isIndependentControl) {

    var CurrentTab;
    var FromAdmin;

    if (param != null) {
        if (typeof param["ParentCtrl"] != 'undefined' && param["ParentCtrl"] != null) {
            CurrentTab = GetTab(param["ParentCtrl"]);
            FromAdmin = param["FromAdmin"];
        }
        else {
            CurrentTab = GetCurrentSelectedTab();
        }
    }
    else {
        CurrentTab = GetCurrentSelectedTab();
    }


    var html;
    html = utility.getTabHtml(ctrl);
    var dfd = new $.Deferred();
    if (html) {
        dfd.resolve(html);
    }
    else {
        IsBackgroundLoaderShow = false;
        $.get(GetTab(ctrl).Path, {
            cache: false
        }, function (content) {
            html = content;
            IsBackgroundLoaderShow = true;
            dfd.resolve(html);
        });
    }

    $.when(dfd).then(function () {
        //  debugger;

        if (params["PreviousTab"] == null)
            params["PreviousTab"] = GetTab(ctrl);
        else
            params["PreviousTab"] = GetSelectedTab();

        eval(ctrl + '.bIsFirstLoad=true');


        $("#" + CurrentTab.ActionPanContainer).prepend(html);
        if (param != null) {
            param["PanelID"] = CurrentTab.PanelID;
            param["TabID"] = CurrentTab.TabID;
        }

        eval(ctrl + '.Load')(param);


        return dfd.promise();

    });
}

function GetCurrentSelectedTab() {
    var CurrentMasterTab = GetCurrentMasterTab();
    return GetSelectedTab(CurrentMasterTab.MasterTabID);
}

function GetCurrentMasterTab() {
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i]["MasterTabID"] == "" && TabsArray[i]["ParentTabID"] == "" && TabsArray[i]["Selected"] == true) return TabsArray[i];
    }
}
function GetSelectedTab(ParentTabID) {
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i]["ParentTabID"] == ParentTabID && TabsArray[i]["Selected"] == true) {
            selectedtab = TabsArray[i];
            return GetSelectedTab(TabsArray[i].TabID);
        }
    }
    return selectedtab;
}
function GetTab(tabID) {
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].TabID == tabID) return TabsArray[i];
    }
}
function LoadApplication() {

    //set Default height and Width for Scanner canvas. 
    setDefaultValuesForScanCanvas(500, 300);

    var TabsArr = store.fetchSession('TabsArray');

    if (TabsArr) {

        TabsArray = TabsArr;
        var PatArray = store.fetchSession('selectedPatientArray');

        if (PatArray)
            PatientArray = PatArray;
    }
    else {
        AppCommands.Load();
    }
}

function DocumentCustomViewMode() {
    setTimeout(function () {
        customViewMode();
    }, 520);

}

function setDefaultValuesForScanCanvas(w, h) {
    localStorage.DWT_height = ($(window).height() - h) < 500 ? $(window).height() - (h + 100) : $(window).height() - h;
    localStorage.DWT_width = $(window).width() - w;
}

function reSetDefaultValuesForScanCanvas() {
    localStorage.DWT_height = $(window).height() - 250;
    localStorage.DWT_width = $(window).width() - 500;

}
LoadApplication();