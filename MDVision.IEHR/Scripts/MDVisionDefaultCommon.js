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
var iTrackTabs = null;
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

var isInTimeOut = 0;

function AmendmentNotesOpen(NotesId, Amendment) {
    var params = [];
    params["ParentCtrl"] = "mstrTabReports";
    params["FromAdmin"] = 0;
    params["mode"] = "Add";
    params["NotesId"] = NotesId;
    if (Amendment.toLowerCase() == "true") {
        params["Amendment"] = true;
    } else {
        params["Amendment"] = false;
    }

    LoadActionPan("Clinical_NotesAmendment", params);
}

function OpenVisitDetail(VisitId, patientID) {
    var strMessage = "";
    AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var params = [];
            params["FromAdmin"] = 0;
            if (ReportsViewer_Detail.params.FormName == "Charges and Payments Reports/ClaimStatusDashboardDetail") {
                params["ParentCtrl"] = "ReportsViewer_Detail";
            } else {
                params["ParentCtrl"] = "mstrTabReports";
            }
            params["VisitId"] = VisitId;
            params["patientID"] = patientID;
            LoadActionPan('EncounterChargeCapture', params);
        }
        else
            utility.DisplayMessages(strMessage, 2);
    });
}

function OpenPatientDemographics(patientid) {
    var strMessage = "";
    AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var params = [];
            params["mode"] = 'Edit';
            params["PatBanner"] = true;
            params["patientID"] = patientid;
            params["IsFill"] = false;
            params["FromAdmin"] = "0";
            if (ReportsViewer_Detail.params.FormName == "Charges and Payments Reports/ClaimStatusDashboardDetail") {
                params["ParentCtrl"] = "ReportsViewer_Detail";
            } else {
                params["ParentCtrl"] = "mstrTabReports";
            }
            LoadActionPan('demographicDetail', params);
        }
        else
            utility.DisplayMessages(strMessage, 2);
    });

}

function OpenMonthlyPaymentTrendDetail(ProviderId, ProviderName, ClaimDate, TotalPayment) {
    var params = [];
    params["FromAdmin"] = 0;
    params["ParentCtrl"] = "mstrTabReports";
    params["ProviderId"] = ProviderId;
    params["ProviderName"] = ProviderName;
    params["ClaimDate"] = ClaimDate;
    params["TotalPayment"] = TotalPayment;
    LoadActionPan('MonthlyPaymentTrend_Detail', params);
}

function NotesPreview(NotesId, PatientId) {
    var params = [];
    params["FromAdmin"] = "0";
    params["NotesId"] = NotesId;
    params["PatientId"] = PatientId;
    params["ParentCtrl"] = "mstrTabReports";
    LoadActionPan('Clinical_NotesView', params);
}

function SetGlobalAppData(id, data) {
    globalAppdata[id] = data;
}

function RefreshWindowOnEntityChange() {
    if (localStorage.GlobalEntityIdForMultiWindow != globalAppdata.SeletedEntityId) {
        localStorage.setItem("GlobalEntityIdForMultiWindow", globalAppdata.SeletedEntityId);
        location.reload();
    }
}

function clearUserSession() {
    for (var i = 0; i < TabsArray.length; i++) {
        var key = TabsArray[i].TabID;
        store.clear(key);
    }
}

function setParentChildMenuId(MenuToShow) {
    if (MenuToShow == "mainUL") {
        var selectedParentMenu = $("#" + MenuToShow + " li.nav-expanded");
        var selectedParentLiId = selectedParentMenu.attr("id");
        var selectedChildLiId;
        if (selectedParentMenu.find("ul li.nav-active").length > 0) {
            selectedChildLiId = selectedParentMenu.find("ul li.nav-active").attr("id");
        }
        if (selectedParentLiId != null && selectedParentLiId != "") {
            $("#hfMainMenuParentLiId").val(selectedParentLiId);
        }
        if (selectedChildLiId != null && selectedChildLiId != "") {
            $("#hfMainMenuChildLiId").val(selectedChildLiId);
        }
    }
}

function ToggleMenu(MenuToShow, CurrentulMenuId, CtrlCurrentParentLiId, CtrlCurrentChildLiId, CtrlPrevParentLiId, CtrlPrevChildLiId, CurrentTabId) {

    if (MenuToShow == "MainMenu") {
        if ($("#hfMainMenuParentLiId").val() != "" && $("#hfMainMenuChildLiId").val() != "") {
            $("li#" + $("#hfMainMenuParentLiId").val() + " a:first").trigger("click");
            setTimeout(function () {
                if ($("li#" + $("#hfMainMenuParentLiId").val()).hasClass("nav-expanded") == false) {
                    $("li#" + $("#hfMainMenuParentLiId").val()).addClass("nav-expanded")
                }
                $("li#" + $("#hfMainMenuChildLiId").val() + " a:first").trigger("click");
            }, 100);
        }
        else {
            SelectTab('mstrTabDashBoard', 'false');
        }

    }
}

function SelectCurrentItem(objButtonId, ButtonType) {
    if (objButtonId != null && objButtonId != "undefined" && ButtonType != null) {
        var CurrentButton = $(ButtonType + "#" + String(objButtonId));
        var CurrentButtonType = CurrentButton.attr("type") != null ? CurrentButton.attr("type") : "li";
        var Activeclass = CurrentButtonType == "li" ? "nav-active" : "active";
        var CurrentButtonParent = null;
        if (CurrentButton != null) {
            if (ButtonType == "button")
                CurrentButtonParent = $(ButtonType + "#" + String(objButtonId)).parent().parent().parent();
            else
                CurrentButtonParent = $(ButtonType + "#" + String(objButtonId)).parent();
            CurrentButtonParent.find(CurrentButtonType).each(function () {
                if (ButtonType == "button") {
                    if ($(this).attr("id") == String(objButtonId)) {
                        if ($(this).parent().hasClass("tab_selected") == false)
                            $(this).parent().addClass("tab_selected");
                    }
                    else
                        $(this).parent().removeClass("tab_selected");
                } else {
                    if ($(this).attr("id") == String(objButtonId)) {
                        if ($(this).hasClass(Activeclass) == false) {
                            $(this).addClass(Activeclass)
                        }
                    }
                    else {
                        $(this).removeClass(Activeclass)
                    }
                }
            });
        }
    }

}

function AddAndSelectTab(cmd, IsMenu, UniqueId, selected) {
    AddTab(cmd);

    var html = utility.getTabHtml(cmd.TabID);
    if (html) {
        var dfd = new $.Deferred();
        dfd.resolve(html);
        if (cmd.Container) {
            $("#" + cmd.Container).append(html);
            if (selected != false)
                SelectCurrentTab(cmd.TabID, IsMenu);
        }
        return dfd.promise();
    }
    else {
        var ajax_get = $.get(cmd.Path, {
            cache: false
        }, function (content) {

            var myDiv = $("<div></div>").append(content);
            if (UniqueId != null) {
                myDiv.find("div#" + cmd.PanelID.replace(String(UniqueId), "")).attr("id", cmd.PanelID).attr("VisitId", UniqueId);
                myDiv.find("div#" + cmd.ActionPanContainer.replace(String(UniqueId), "")).attr("id", cmd.ActionPanContainer);
            }

            html = String(myDiv.first("div").html());
            //html = content;

            utility.saveTabHtml(cmd.TabID, html);
            if (cmd.Container) {
                $("#" + cmd.Container).append(html);
                if (selected != false)
                    SelectCurrentTab(cmd.TabID, IsMenu);
            }
        }, "html");
    }
}

function RemoveCurrentTab(tabID) {
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].TabID == tabID) return TabsArray.splice(i, 1);
    }
}

function CheckPatientDemoMissingDetails() {
    var SelectedPatientSex = $('#PatientProfile #hfPatientSex').val();
    var SelectedPatientMaritalStatus = $('#PatientProfile #hfPatientMaritalStatus').val();
    var SelectedPatientEthnicityIds = $('#PatientProfile #hfPatientEthnicityIds').val();
    var SelectedPatientRaceIds = $('#PatientProfile #hfPatientRaceIds').val();
    var SelectedPatientAddress1 = $('#PatientProfile #hfPatientAddress1').val();
    var SelectedPatientCity = $('#PatientProfile #hfPatientCity').val();
    var SelectedPatientState = $('#PatientProfile #hfPatientState').val();
    var SelectedPatientZip = $('#PatientProfile #hfPatientZip').val();
    var SelectedPatientHomeTel = $('#PatientProfile #hfPatientHomeTel').val();

    if (SelectedPatientSex != "" && SelectedPatientMaritalStatus != "" && SelectedPatientEthnicityIds != "" && SelectedPatientRaceIds != "" && SelectedPatientAddress1 != "" &&
        SelectedPatientCity != "" && SelectedPatientState != "" && SelectedPatientZip != "" && SelectedPatientHomeTel != "") {
        return false;
    }
    else {
        return true;
    }
}

function UnSelectedMenuAndTab(Tab, IsMenu) {
    var UnselectedtabObj = $(document.getElementById(Tab.TabID));
    var Unselectedstr = Tab.TabID;
    var UnselectedmenuTab = Unselectedstr.replace(/Tab/gi, "Menu");
    var UnselectedmenuObj = $(document.getElementById(UnselectedmenuTab));
    if (Tab.MasterTabID == "" && IsMenu == 'false')
        $(UnselectedmenuObj).removeClass("nav-parent nav-expanded").addClass("nav-parent");

    if (Tab.MasterTabID != "" && IsMenu == 'false') {
        if (typeof Tab.ParentChildTabID != "undefined") {
            var strParentChild = Tab.ParentChildTabID;
            var menuParentChildTab = strParentChild.replace(/Tab/gi, "Menu");

            var UnSelectedParentChildObj = $(document.getElementById(menuParentChildTab));
            // -----------------------------------------------------------
            if (Tab.MasterTabID == "mstrTabClinical") {
                if ($(UnSelectedParentChildObj).hasClass("nav-parent nav-expanded") == true)
                    $(UnSelectedParentChildObj).removeClass("nav-parent nav-expanded").addClass("nav-parent");
                else
                    $(UnSelectedParentChildObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");
            } else {
                if ($(UnSelectedParentChildObj).hasClass("nav-expanded nav-parent") == true || $(UnSelectedParentChildObj).hasClass("nav-parent nav-expanded") == true)
                    $(UnSelectedParentChildObj).removeClass("nav-parent nav-expanded").addClass("nav-parent");
                if (Tab.ParentChildTabID == "mstrMenuBilling")
                    $(UnSelectedParentChildObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");

                if (Tab.ParentChildTabID == CurrentParentmenu) {
                    $(UnSelectedParentChildObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");
                }
            }
            //PreParent InActive
            var parentParentModule = $('#' + menuParentChildTab).parents('li').attr('id');
            if (parentParentModule != undefined) {
                var SelectedMenuparentParentObjId = parentParentModule.replace("Menu", "Tab");
                if (SelectedMenuparentParentObjId != Tab.MasterTabID) {
                    var SelectedMenuparentParentObj = $(document.getElementById(parentParentModule));
                    if ($(SelectedMenuparentParentObj).hasClass("nav-parent nav-expanded"))
                        $(SelectedMenuparentParentObj).removeClass("nav-parent nav-expanded").addClass("nav-parent");
                    else if ($(SelectedMenuparentParentObj).hasClass("nav-expanded nav-parent"))
                        $(SelectedMenuparentParentObj).removeClass("nav-expanded nav-parent").addClass("nav-parent");
                    else
                        $(SelectedMenuparentParentObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");
                }
            }
        }
        UnselectedmenuObj.removeClass("nav-active");
    }
    else if (Tab.MasterTabID != "" && IsMenu == 'true')
        UnselectedmenuObj.removeClass("nav-active");


    if (Tab.MasterTabID == "") {
        if ($(UnselectedtabObj).hasClass("active") == true)
            $(UnselectedtabObj).removeClass("active");
    }
    else {
        if ($(UnselectedtabObj).hasClass("btn btn-default btn-sm active") == true)
            UnselectedtabObj.removeClass("btn btn-default btn-sm active").addClass("btn btn-default btn-sm");
        else if ((UnselectedtabObj.parent().attr("class")) == "btn btn-default btn-sm tab_selected") {
            if (Tab.MasterTabID != "mstrTabSchedule")
                UnselectedtabObj.parent().attr("class", "btn btn-default btn-sm tab_space");
        }
    }
    if (CurrentParentmenu == "adminMenuFavorites") {
        if (Tab.ContainerControlID == "Favorite_FamilyHistory" || Tab.ContainerControlID == "Favorite_MedicalHistory" || Tab.ContainerControlID == "Favorite_SurgicalHistory" || Tab.ContainerControlID == "Favorite_HospitalizationHistory") {
            $("#adminMenuClinical").removeClass("nav-parent").addClass("nav-parent nav-expanded");
        }
    }
}

function GetTab(tabID) {
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].TabID == tabID) return TabsArray[i];
    }
}

function OpenDemographicLabel() {
    var params = [];
    params["PatientID"] = GetSelectedPatientID();
    LoadActionPan('DemographicLabel', params);
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

function showCurrentTab() {
    _tempCurrenttab.show('fade', 500);
}

function LoadControl(Tab, param, Reload) {
    var patientID = null;
    if (isPatientTab(Tab) || Reload) {
        patientID = GetSelectedPatientID();
        if (Tab.ContainerControlID != "") {
            if (param != null) {
                if (patientID != null) {
                    param['patientID'] = patientID;
                    param['mode'] = "Edit";
                }
                if (selectedtab != null) {
                    param["PanelID"] = selectedtab.PanelID;
                    param["ActionPanContainer"] = selectedtab.ActionPanContainer;
                    param["TabID"] = selectedtab.TabID;
                }
                else {
                    param["PanelID"] = Tab.PanelID;
                    param["ActionPanContainer"] = Tab.ActionPanContainer;
                    param["TabID"] = Tab.TabID;
                }
            }
            if (!store.fetch(Tab.ContainerControlID, patientID) || Reload) {
                var b = eval(Tab.ContainerControlID + '.Load')(param);
                store.save(Tab.ContainerControlID, true, patientID)
            }
            else {
                var b = eval(Tab.ContainerControlID + '.Load')(param);
            }
        }
    }
    else {
        if (Tab.ContainerControlID != "") {
            if (param != null) {
                if (patientID != null) {
                    param['patientID'] = patientID;
                    param['mode'] = "Edit";
                }
                if (selectedtab != null) {
                    param["PanelID"] = selectedtab.PanelID;
                    param["ActionPanContainer"] = selectedtab.ActionPanContainer;
                    param["TabID"] = selectedtab.TabID;
                }
                else {
                    param["PanelID"] = Tab.PanelID;
                    param["ActionPanContainer"] = Tab.ActionPanContainer;
                    param["TabID"] = Tab.TabID;
                }
            }
            eval(Tab.ContainerControlID + '.Load')(param);
        }
    }
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

function LoadActionPan(ctrl, param, ParentCtrlPanelID, isIndependentControl) {
    DisposeOrEnableDragablePatientSearch(null, param, 1);
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

    var ActionPanContainer = null;
    if (param != null && param.flag != undefined && param.flag == true)
        ActionPanContainer = CurrentTab.ParallelActionPanContainer;
    else {
        if (ParentCtrlPanelID != null)
            ActionPanContainer = ParentCtrlPanelID + ' #' + CurrentTab.ActionPanContainer;
        else
            ActionPanContainer = CurrentTab.ActionPanContainer;
        if (isIndependentControl != null && isIndependentControl == true)
            ActionPanContainer = ParentCtrlPanelID;
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
        if ($('#' + ActionPanContainer).find('div#modaldialog').length == 0) {

            $('#' + ActionPanContainer).empty();

            if (params["PreviousTab"] == null)
                params["PreviousTab"] = GetTab(ctrl);
            else
                params["PreviousTab"] = GetSelectedTab();


            if ($('#' + ActionPanContainer).find('div').first().attr("id") != ctrl) {

                eval(ctrl + '.bIsFirstLoad=true');


                $("#" + ActionPanContainer).prepend(html);
                if (param != null) {
                    param["PanelID"] = CurrentTab.PanelID;
                    param["ActionPanContainer"] = CurrentTab.ActionPanContainer;
                    param["TabID"] = CurrentTab.TabID;
                }
                if (FromAdmin != 1) {

                    $('#' + ActionPanContainer).find('div#formpanelheading').empty();
                    $('#' + ActionPanContainer).find('div#modalheader').css("display", "");
                    $('#' + ActionPanContainer).find('div#modaldialog').addClass('modal-dialog modal-dialog-lg');
                    $('#' + ActionPanContainer).find('div#modalcontent').addClass('modal-content');
                    $('#' + ActionPanContainer).find('div#modalheader').addClass('modal-header');
                    $('#' + ActionPanContainer).find('div#modalbody').addClass('modal-body');
                }

                eval(ctrl + '.Load')(param);


                if (!$('#' + ActionPanContainer).find('div').first().css('display') || $('#' + ActionPanContainer).find('div').first().css('display') == 'none') {
                    $('#' + ActionPanContainer).find('div').first().css("display", "");
                }
            }
            $('#' + ActionPanContainer).modal({
                show: 'true',
                backdrop: 'static',
                keyboard: false

            }).on('shown.bs.modal', function () {

                if ($(this).find("form").is('[id]') && $(this).find("form").attr('id') != undefined) {
                    if ($(this).find("form").attr('id').toLowerCase().indexOf("paymentposting") < 0) {
                        $(this).find("form :input:not(button):not(hidden):not([data-plugin-timepicker]):not([data-plugin-datepicker]):not([data-plugin-keyboard-numpad]):not([id*='date']):not([id*='Date']):not([id*='dtp']):enabled:visible:first").focus();
                    }
                }


            }).on('hidden.bs.modal', function () {
                if ($('body').find('.modal-backdrop').length > 0) {

                    $('body').addClass('modal-open');
                }
                else
                    $('body').removeClass('modal-open');

                //remove cache access for Note component on component close, IMP-724, by Arsalan javed
                if ($(this).attr("id") == "actionPanClinicalProgressNote"
                    && $(this).hasClass('fade')
                    && !($(this).hasClass('in'))
                    && UserRecentAccessedNoteComponent != ""
                    ) {

                    if (IsRemoveNoteComponent == true) {
                        Clinical_ProgressNote.remove_UserNoteAccess(true, UserRecentAccessedNoteComponent).done(function (res) {
                            UserRecentAccessedNoteComponent = "";
                        });
                    }

                }

            });

            return dfd.promise();
        }
    });
}

function UnloadActionPan(ParentCtrl, Ctrl, ParallelActionPan, ParentCtrlPanelID) {

    DisposeOrEnableDragablePatientSearch(ParentCtrl, null, 0);

    var CurrentSelectedTab;
    if (ParentCtrl != null) {
        CurrentSelectedTab = GetTab(ParentCtrl);
        //if (typeof Tab == "undefined")
    }
    else {
        CurrentSelectedTab = GetCurrentSelectedTab();
    }

    var CurrentActionPanContainer = null;
    if (ParallelActionPan != null)
        CurrentActionPanContainer = CurrentSelectedTab.ParallelActionPanContainer;
    else {
        if (ParentCtrlPanelID != null)
            CurrentActionPanContainer = ParentCtrlPanelID + ' #' + CurrentSelectedTab.ActionPanContainer;
        else
            CurrentActionPanContainer = CurrentSelectedTab.ActionPanContainer;
    }
    //if (CurrentSelectedTab.MasterTabID == "mstrTabPatient" && (PatientArray.length <= 0 || GetSelectedPatientID() == 'undefined'))
    //    CurrentActionPanContainer = "actionPanPatient";

    //Reverted by Azeem Raza Tayyab on 15-Mar-2016, changes done by Adnan on 8722.
    /*
    $CurrentActionPanContainer =$(document).find('div[id="' + CurrentActionPanContainer + '"]:last');
    if ($CurrentActionPanContainer.length <= 0)
        $CurrentActionPanContainer = $('#' + CurrentActionPanContainer);

    $CurrentActionPanContainer.modal('hide');
    $CurrentActionPanContainer.find('div').first().hide('blind', 500, function () {
        $(this).remove();
    });*/
    //QAC2-319 fix by Azhar Shahzad | the issue is, when same screen open more then once in child action controls
    if ($('#' + CurrentActionPanContainer).length <= 0) {
        CurrentSelectedTab = GetCurrentSelectedTab();
        CurrentActionPanContainer = CurrentSelectedTab.ActionPanContainer;
    }
    //QAC2-319 end fix
    $('#' + CurrentActionPanContainer).modal('hide');

    if (CurrentActionPanContainer == "actionPanReportsSSRSDashboard") {
        $('#' + CurrentActionPanContainer).find('div').first().remove();
    }

    else {
        //bug#PMS-1147
        if (Ctrl == "Document_Scan") {
            //animation with hide option not work mostly in ie browser it run infinit loop and browser stop responding.
            //its a jquery defact.so remove animation only in this case.
            $('#' + CurrentActionPanContainer).find('div').first().hide();
            $('#' + CurrentActionPanContainer).find('div').first().remove();
        }
        else {
            $('#' + CurrentActionPanContainer).find('div').first().hide('blind', 500, function () {
                $(this).remove();
            });
        }
    }

    // ctrl load two Different module at same time
    if (Ctrl) {
        eval(Ctrl + '.bIsFirstLoad=true');
    }
    params["ParentCtrl"] = null;
    params["FromAdmin"] = "1";

    //This code will clear the params of loaded jquery classes for tabs of controls
    /*if (CurrentSelectedTab != "" && CurrentSelectedTab != null && typeof CurrentSelectedTab!="undefined") {
        eval(CurrentSelectedTab.ContainerControlID).bIsFirstLoad = true;
        eval(CurrentSelectedTab.ContainerControlID).params = [];
    }*/
    if ($('#' + GetCurrentSelectedTab().PanelID).find('div').first().attr('aria-hidden') == "false" || $('#' + GetCurrentSelectedTab().PanelID).find('div').first().prop('id') == "actionPanBillUnClaimedAppointment") {
        $("body").addClass("modal-open");
    }
    else {
        $("body").removeClass("modal-open");
    }
}

function OpenMU3Alerts(SelectedTab) {
    var params = [];
    params["PatientID"] = GetSelectedPatientID();
    params["SelectedTab"] = SelectedTab ? SelectedTab : "";
    LoadActionPan('MU_Alerts', params);
}

function ShowPatientProfile() {
    $('#PatientProfile').css('display', 'none');

    if ($('#PatientProfile').css('display') == 'none') {
        $('#PatientProfile').show('blind', 200);
    }
}

function GetSelectedPatientID() {
    for (var i = 0; i < PatientArray.length; i++) {
        if (PatientArray[i].Selected == true) return String(PatientArray[i].PatientID);
    }
}

function IsPatientExist(PatientID) {
    var breturn = false;
    for (var i = 0; i < PatientArray.length; i++) {
        if (PatientArray[i].PatientID == PatientID) {
            breturn = true;
            break;
        }
    }
    return breturn;
}

function SetSelectedPatient(patientID) {
    for (var i = 0; i < PatientArray.length; i++) {
        if (PatientArray[i].PatientID == patientID)
            PatientArray[i].Selected = true;
        else
            PatientArray[i].Selected = false;
    }
}

function AddPatientArray(PatientID) {
    for (var i = 0; i < PatientArray.length; i++) {
        PatientArray.splice(i, 1);
    }
    var Patient = new Object();
    Patient["Selected"] = true;
    Patient["PatientID"] = PatientID;
    PatientArray.push(Patient);

    for (var i = 0; i < PatientArray.length; i++) {
        if (PatientArray[i].PatientID != PatientID)
            PatientArray[i].Selected = false;
    }

    store.saveSession('selectedPatientArray', PatientArray);
}


(function ($) {
    $(document).ajaxSend(function (e, jqXHR, options) {
        if (IsBackgroundLoaderShow == true) {
            xhrPool.push(jqXHR);
            BackgroundLoaderShow(true);
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
}

function OpenDashBoardSetting() {
    var params = [];
    var tab = GetCurrentSelectedTab();
    if (tab.TabID != 'mstrTabDashBoard') {
        params["ParentCtrl"] = tab.TabID;
    }
    else
        params["ParentCtrl"] = "mstrTabDashBoard";
    LoadActionPan('DashBoardSetting', params)
}

function OpenDashBoardChgPwd() {
    var params = [];
    var tab = GetCurrentSelectedTab();
    if (tab.TabID != 'mstrTabDashBoard') {
        params["ParentCtrl"] = tab.TabID;
    }
    else
        params["ParentCtrl"] = "mstrTabDashBoard";
    LoadActionPan('DashBoardChangePwd', params)
}

function OpenUserMessages() {
    var params = [];
    var tab = GetCurrentSelectedTab();
    if (tab.TabID != 'mstrTabDashBoard') {
        params["ParentCtrl"] = tab.TabID;
    }
    else
        params["ParentCtrl"] = "mstrTabDashBoard";
    LoadActionPan('Patient_UserMessagesQuickLink', params)
}

function OpenFaxes() {
    var params = [];
    params["AssignedToId"] = globalAppdata['AppUserId'];
    params["IsUserMessages"] = "1";
    LoadActionPan('Batch_Fax_QuickLink', params)
}

function OpenUserTasks() {
    var params = [];
    params["AssignedToId"] = globalAppdata['AppUserId'];
    params["IsUserMessages"] = "1";
    LoadActionPan('User_Task', params)
}

function SelectDefaultPatientTab(tabId, selectedChildTabID, Reload) {
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].ParentTabID == tabId) {
            if (TabsArray[i].TabID == selectedChildTabID) {
                TabsArray[i].Selected = true;
                $('#' + TabsArray[i].PanelID).show();
            }
            else {
                TabsArray[i].Selected = false;
                $('#' + TabsArray[i].PanelID).hide();
            }
        }
    }
    SetActiveTab(selectedChildTabID);
    SelectCurrentTab(selectedChildTabID, 'false', Reload);
}

function SetActiveTab(parentTabID) {
    var selectedTab = null;
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].ParentTabID == parentTabID) {
            if (TabsArray[i]["Selected"] == true) {
                selectedTab = TabsArray[i];
                //Select Active Tab
                var SelectedtabObj = $(document.getElementById(selectedTab["TabID"]));
                var SelectedpnlObj //document.getElementById(selectedTab["PanelID"]);

                if (TabsArray[i]["PanelID"] != "" && TabsArray[i]["MasterTabID"] != "")
                    SelectedpnlObj = $('#' + TabsArray[i]["Container"] + ' #' + TabsArray[i]["PanelID"]);
                else
                    SelectedpnlObj = $(document.getElementById(TabsArray[i]["PanelID"]));

                SelectedpnlObj.css("display", "block");
                SelectedMenuAndTab(selectedTab, 'false');
            }
            else {
                TabsArray[i]["Selected"] = false;
                TabsArray[i]["Active"] = false;

                var UnselectedtabObj = $(document.getElementById(TabsArray[i]["TabID"]));
                var UnselectedpnlObj

                if (TabsArray[i]["PanelID"] != "" && TabsArray[i]["MasterTabID"] != "")
                    UnselectedpnlObj = $('#' + TabsArray[i]["Container"] + ' #' + TabsArray[i]["PanelID"]);
                else
                    UnselectedpnlObj = $(document.getElementById(TabsArray[i]["PanelID"]));

                UnselectedpnlObj.css("display", "none");
                UnSelectedMenuAndTab(TabsArray[i], 'false');
            }
        }
    }
}

function TopButtonsHideShow() {

    if (globalAppdata["IsDocumentsAlert"] == "False") {
        $('.notifications #PendingDocuments').css('display', 'none');
    }
    else
        $('.notifications #PendingDocuments').css('display', 'inline');

    if (globalAppdata["IsSearchPatient"] == "False") {
        $('.notifications #PatientSearch').css('display', 'none');
    }
    else
        $('.notifications #PatientSearch').css('display', 'inline');

    if (globalAppdata["IsQuickAddPatient"] == "False") {
        $('.notifications #AddPatient').css('display', 'none');
    }
    else
        $('.notifications #AddPatient').css('display', 'inline');

    if (globalAppdata["IsNote"] == "False") {
        $('.notifications #Notes').css('display', 'none');
    }
    else
        $('.notifications #Notes').css('display', 'inline');

    if (globalAppdata["IsAppointment"] == "False") {
        $('.notifications #Appointments').css('display', 'none');
    }
    else
        $('.notifications #Appointments').css('display', 'inline');

    if (globalAppdata["IsTask"] == "False") {
        $('.notifications #Tasks').css('display', 'none');
    }
    else
        $('.notifications #Tasks').css('display', 'inline');

    if (globalAppdata["IsFax"] == "False") {
        $('.notifications #Faxes').css('display', 'none');
    }
    else
        $('.notifications #Faxes').css('display', 'inline');

    if (globalAppdata["IsMessage"] == "False") {
        $('.notifications #Messages').css('display', 'none');
    }
    else
        $('.notifications #Messages').css('display', 'inline');

    /**/

    if (globalAppdata["IsPrescriptionsRefill"] == "False") {
        $('.notifications #PrescriptionsRefill').css('display', 'none');
    }
    else
        $('.notifications #PrescriptionsRefill').css('display', 'inline');

    if (globalAppdata["IsPendingPrescriptions"] == "False") {
        $('.notifications #PendingPrescriptions').css('display', 'none');
    }
    else
        $('.notifications #PendingPrescriptions').css('display', 'inline');

    if (globalAppdata["IsImmunizationAlert"] == "False") {
        $('.notifications #ImmunizationAlert').css('display', 'none');
    }
    else
        $('.notifications #ImmunizationAlert').css('display', 'inline');

    if (globalAppdata["IsDocumentsAlert"] == "False") {
        $('.notifications #PendingDocuments').css('display', 'none');
    }
    else
        $('.notifications #PendingDocuments').css('display', 'inline');
    /**/
    if (globalAppdata["IsDocument"] == "False") {
        $('.notifications #Documents').css('display', 'none');
    }
    else
        $('.notifications #Documents').css('display', 'inline');
}

function OpenHelpScreen() {
    if (!$('#containerHelpDocument').hasClass('modal')) {
        var parameters = [];
        $('#containerHelpDocument').addClass('modal fade')
        LoadActionPan('HelpScreen', parameters, 'containerHelpDocument', true);
    }
}

function OpenAboutScreen() {
    if (!$('#containerAbout').hasClass('modal')) {
        var parameters = [];
        $('#containerAbout').addClass('modal fade')
        LoadActionPan('AboutScreen', parameters, 'containerAbout', true);
    }
}

function globallyDateTime() {
    var d = new Date($('#userCurrentTime').text());

    if (d == "Invalid Date")
        d = new Date(Date($('#userCurrentTime').text()))
    d.setSeconds(d.getSeconds() + 1);
    var t = d.toLocaleTimeString();

    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    var dayName = dayNames[d.getDay()]
    var monthName = monthNames[d.getMonth()]
    var dateOfMonth = d.getDate();
    var year = d.getFullYear()

    var date = dayName + ", " + monthName + " " + dateOfMonth + ", " + year;
    document.getElementById("userCurrentTime").innerHTML = date + " " + t;
}

function deleteNotesGlobalParams() {
    delete params["PatientId"];;
    delete params["ParentCtrl"];
}

function GetAutoUpdateDemographicScreen(tabId, IsMenu, Reload, UniqueId, TabTitle, event) {
    if (params.DemographicAutoUpdateActiveTab == "Demographic") {
        if ($("#pnlDemographic #frmDemographic").length > 0) {
            PatientDetailsAutoUpdate("#pnlDemographic #frmDemographic", params.defaultDemographicSerailizeForm, tabId, IsMenu, Reload, UniqueId, TabTitle, event);
        }
    }
    else if (params.DemographicAutoUpdateActiveTab == "Insurance") {
        if ($("#pnlPatientInsurance #frmPatientInsurance").length > 0)
            PatientDetailsAutoUpdate("#pnlPatientInsurance #frmPatientInsurance", params.defaultDemographicSerailizeForm, tabId, IsMenu, Reload, UniqueId, TabTitle, event);
    }
}

function PatientDetailsAutoUpdate(formId, defaultSerailizeForm, tabId, IsMenu, Reload, UniqueId, TabTitle, event) {
    if (formId.indexOf("#") == 0)
        formId = formId;
    else
        formId = "#" + formId;

    var formCtr = $(formId);
    if (defaultSerailizeForm)     // check it contain serialize data at Load time of Demographic/Insurance (active tab)
    {
        if (formCtr.serialize() != defaultSerailizeForm) {
            if (!params.IsDemographicInfoGlobalyUpdated)     // check active tab changes already not updated 
            {
                var ScreenName = params.DemographicAutoUpdateActiveTab;
                if (params.DemographicAutoUpdateActiveTab == "Insurance")
                    ScreenName = "Patient Insurance";

                utility.myConfirm('Are you sure you want to save ' + ScreenName + ' change?', function () {
                    drfdAutoSave = $.Deferred();
                    params.IsDemographicInfoGlobalyUpdated = true;          //confirm active tab is updated
                    delete params.DemographicAutoUpdateActiveTab;
                    if (formId == "#pnlPatientInsurance #frmPatientInsurance")
                        $("#pnlPatientInsurance #frmPatientInsurance").trigger("submit");
                    else
                        $("#pnlDemographic #frmDemographic").trigger("submit");
                    // deffered  object is used to complete all save/update call on data submitt of active tab before go into other tab
                    $.when(drfdAutoSave).then(function () {
                        { SelectApplicationTab(tabId, IsMenu, Reload, UniqueId, TabTitle, event); }
                    });

                }, function () {
                    //  set active tab is inactive and its data is  updated without any changes and open other click tab 
                    params.IsDemographicInfoGlobalyUpdated = true;
                    delete params.DemographicAutoUpdateActiveTab;
                    SelectApplicationTab(tabId, IsMenu, Reload, UniqueId, TabTitle, event);
                },
                                   'Confirm Change'
                               );
            }
        }
        else {    //if user does not do any change in active tab and move to other tab 
            SelectApplicationTab(tabId, IsMenu, Reload, UniqueId, TabTitle, event);
            delete params.DemographicAutoUpdateActiveTab;
        }
    }
}

function DisposeOrEnableDragablePatientSearch(ParentCtrl, params, Dispose) {
    if (Dispose) {
        if (params && params.ParentCtrl && params.ParentCtrl == 'Patient_Search') {
            Patient_Search.DisposeDraggable();
        }
    }
    else {
        if (ParentCtrl && ParentCtrl == 'Patient_Search') {
            $('.Patient_Search:last').parent().draggable({
                handle: ".Patient_Search:last .modal-content"
            });
            $(".Patient_Search:last #modaldialog").attr("class", "mt-none mr-none mb-none modal-dialog modal-dialog-full");
        }
    }
}

function removeTabAndClickSibling(e) {
    var leftTab = $(e.parent()).prev().hasClass("scroll_tab_left_finisher");
    var rightTab = $(e.parent()).next().hasClass("scroll_tab_right_finisher");
    if ((leftTab === false && rightTab === false) || (leftTab === false && rightTab === true)) {
        $(e.parent()).prev().find('button').click();
    }

    else {
        $(e.parent()).next().find('button').click();
    }
}

function CloseWindow() {
    window.close();
}

function getNotificationsCounts() {
    var data = "PatientID=" + params.patientID;
    // serach parameter , class name, command name of class
    return MDVisionService.defaultService(data, "DASHBOARD", "SEARCH_NOTIFICATIONS_COUNTS");
}

function updateNotificationsCounts() {
    var NotesCount;
    var AppCount;
    var AlertCount;
    var MessageCount;
    var UserTasksCount;
    IsBackgroundLoaderShow = false;
    getNotificationsCounts().done(function (response) {
        IsBackgroundLoaderShow = true;
        if (response.status != false) {
            var parsedJson = JSON.parse(response.NotificationsCounts_JSON);
            AlertCount = parsedJson.spnAlertCount;
            //setting the values in respective spans
            $("#spnAlertCount").text(AlertCount);
        }
    });
}

function ClinicalMenuClick(event, CallBackFunction, IsRootElement, ObjRootElementId, objButtonId, ButtonType) {
    if (event) {
        event.stopPropagation();
    }
    var objDeffered = $.Deferred();
    if (IsRootElement == 1) {
        // if user wants to collapsed a menu. EMR-1044| change by Azhar Shahzad on May 09,2016
        var $anchor = $(ObjRootElementId),
                    $prev = $anchor.closest('ul.nav').find('> li.nav-expanded'),
                    $next = $anchor.closest('li');
        if (ObjRootElementId != null && $prev.get(0) !== $next.get(0)) {
            var parentId = $(ObjRootElementId).parent().attr("id");
            $("#hfClinicalMenuParentLiId").val(parentId);
            $(ObjRootElementId).parent().find("ul li:first a:first").click();
            var firstLiId = $(ObjRootElementId).parent().find("ul li:first").attr("id");
            if (firstLiId != null) {
                objButtonId = firstLiId;
                $("#hfClinicalMenuChildLiId").val(firstLiId);
            }
        }
    }
    else if (IsRootElement == 0) {
        if (objButtonId != null) {
            $("#hfClinicalMenuChildLiId").val(objButtonId);
        }
    }

    SelectCurrentItem(objButtonId, ButtonType);
    if (ButtonType == "li") {
        SelectCurrentItem(objButtonId, "button");
    }
    else if (ButtonType == "button") {
        SelectCurrentItem(objButtonId, "li");
    }
    var SelectedPatientId = $("#PatientProfile #hfPatientId").val();
    var SelectedPatientAccount = $("#PatientProfile #hfAccountNo").val();
    if (SelectedPatientId != null && SelectedPatientId != "") {
        if (CheckPatientDemoMissingDetails() == false) {
            if (CallBackFunction != null && typeof (CallBackFunction) == 'function') {
                Clinical_ProgressNote.disconnectUserFromSignalR();
                setTimeout(CallBackFunction, 20);
            }
            objDeffered.resolve();
        }
        else {
            params["mode"] = "Edit";
            params["PatBanner"] = true;
            params["IsFill"] = true;
            LoadActionPan('demographicDetail', params);
        }
    }
    else {
        LoadActionPan('Patient_Search', null);
        objDeffered.resolve();
    }
    return objDeffered;
}

function RemoveTabContainerControlID(cmd) {
    var newArray = TabsArray.filter(function (TabIndex) {
        if (cmd.ContainerControlID == TabIndex.ContainerControlID) {
            TabIndex.ContainerControlID = "";
            return TabIndex;
        }
    });
}

function RemoveTab(cmd) {
    var newArray = TabsArray.filter(function (TabIndex) {
        if (cmd.TabID != TabIndex.TabID) {
            return TabIndex;
        }
    });

    TabsArray = newArray;
    store.saveSession('TabsArray', TabsArray);
}

function AddTab(cmd) {
    var Tab = new Object();
    Tab["Selected"] = cmd.Selected;
    Tab["TabID"] = cmd.TabID;
    Tab["MasterTabID"] = cmd.MasterTabID;
    Tab["ParentTabID"] = cmd.ParentTabID;
    Tab["ParentChildTabID"] = cmd.ParentChildTabID;
    Tab["PanelID"] = cmd.PanelID;
    Tab["Active"] = false;
    Tab["ContainerControlID"] = cmd.ContainerControlID;
    Tab["Container"] = cmd.Container;
    Tab["Path"] = cmd.Path;
    Tab["isActionPan"] = cmd.isActionPan;
    Tab["defaultSelectedTabID"] = cmd.defaultSelectedTabID;
    Tab["ActionPanContainer"] = cmd.ActionPanContainer;
    Tab["ParallelActionPanContainer"] = cmd.ParallelActionPanContainer;
    TabsArray.push(Tab);
}

function setDefaultValuesForScanCanvas(w, h) {
    localStorage.DWT_height = ($(window).height() - h) < 500 ? $(window).height() - (h + 100) : $(window).height() - h;
    localStorage.DWT_width = $(window).width() - w;
}

function reSetDefaultValuesForScanCanvas() {
    localStorage.DWT_height = $(window).height() - 250;
    localStorage.DWT_width = $(window).width() - 500;
}

function RemoveAdminTab(TabID, TabType) {
    var Tab;
    // if user pass the tab id than it should must get tab informatoin against that tab id
    // Change by: Muhammad Azhar Shahzad, Date: April 08,2016
    if (TabType == "Encounter" || TabID != null) {
        Tab = GetTab(TabID);
    }
    else
        Tab = GetCurrentSelectedTab();
    if (typeof Tab == "undefined")
        return;

    var SelectedtabObj = $(document.getElementById(Tab.TabID));
    var SelectedActionPanContObj = $(document.getElementById(Tab.ActionPanContainer));
    var SelectedpnlObj = $(document.getElementById(Tab.PanelID));

    if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "")
        SelectedpnlObj = $('#' + Tab["Container"] + ' #' + Tab["PanelID"]);
    else
        SelectedpnlObj = $(document.getElementById(Tab["PanelID"]));


    if (SelectedtabObj != null || SelectedtabObj != "undefined") {

        removeTabAndClickSibling(SelectedtabObj);
        //adnan maqbool, PMS-5193
        if (SelectedtabObj.attr('id')) {
            if (SelectedtabObj.attr('id').split('_')[0] == "EncounterVisit") {
                SelectedtabObj.remove();
            } else {
                SelectedtabObj.parent().remove();
            }
        } else {
            SelectedtabObj.parent().remove();
        }
        //
    }

    if (SelectedpnlObj != null || SelectedpnlObj != "undefined") {
        SelectedpnlObj.remove();
    }
    if (SelectedActionPanContObj != null || SelectedActionPanContObj != "undefined") {

        SelectedActionPanContObj.remove();
    }

    //remove tab from array
    RemoveTab(Tab);
    localStorage.setItem("BillingSelectedPage", "");

    //reset the fistload of contorl
    //adnan maqbool, PMS-5193
    if (Tab.ContainerControlID)
        eval(Tab.ContainerControlID + '.bIsFirstLoad=true');

    //Unselected Previous menu
    UnSelectedMenuAndTab(Tab, 'false');

    //if other tab exist to select current tab
    var mstrTab = Tab.MasterTabID;
    var TabDiv = mstrTab.replace(/Tab/gi, "Div");
    var nodes = document.getElementById(TabDiv).getElementsByTagName('*');
    if (nodes.length != 0) {
        SelectCurrentTab(nodes[0].id, 'false');
    }

    // Remove Tab ContainerControlID from other App Command Objects. PMS-3431
    RemoveTabContainerControlID(Tab);
}