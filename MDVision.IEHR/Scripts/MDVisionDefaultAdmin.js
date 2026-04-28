var TabsArray = [];
var params = [];
params['patientID'] = "-1";
params["FromAdmin"] = "1";
params["PreviousTab"] = null;
var IsActionPanPatientLoaded = false
var PatientArray = [];
var globalAppdata = [];
var DefaultUser = "MDVISION";
var iTrackTabs = null;
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

function SetGlobalAppData(id, data) {
    globalAppdata[id] = data;
}

function LoadApplication() {
    //fix PMS-2706
    if (localStorage.SelectPatientEntityId != globalAppdata.SeletedEntityId) {
        localStorage.removeItem('SelectedPatientId');
        localStorage.removeItem('SelectPatientEntityId');
    }

    //fix PMS-2709
    localStorage.setItem("GlobalEntityIdForMultiWindow", globalAppdata.SeletedEntityId);
    //////
    $('#blistSwitchEntity').on('click', 'li', function () {
        localStorage.removeItem('SelectedPatientId');

    });

    DashBoardSetting.ChangeThemeColor(true);

    store.clearAllSession();
    store.clearAll();
    var TabsArr = store.fetchSession('TabsArray');

    if (TabsArr) {

        TabsArray = TabsArr;
        var PatArray = store.fetchSession('selectedPatientArray');

        if (PatArray)
            PatientArray = PatArray;

        var document_html = store.fetchSession('document_html');

        if (document_html) {
            store.clearSession('document_html');
            $(document).find('body').html(document_html);
        }

        //reload current control
        var tab = GetCurrentSelectedTab();
        var html = utility.getTabHtml(tab.TabID);
        if (html) {

            if (tab.TabID != 'mstrTabDashBoard') {
                $("#" + tab.PanelID).remove();
                $("#" + tab.Container).prepend(html);
                $("#" + tab.PanelID).css("display", "");
                eval(tab.ContainerControlID + '.Load')(params);
            }

        }
        else {
            $.get(tab.Path, { cache: false }, function (content) {
                html = content;
                if (tab.TabID != 'mstrTabDashBoard') {
                    $("#" + tab.PanelID).remove();
                    $("#" + tab.Container).prepend(html);
                    $("#" + tab.PanelID).css("display", "");
                    eval(tab.ContainerControlID + '.Load')(params);
                }
            });
        }
    }
    else {
        LoadHomeTab();
    }
	
	 initSession();

}

/******* This function is responsible for making hash section of to be used in the application for specific module loading ***********/
function RefreshWindowOnEntityChange() {
    if (localStorage.GlobalEntityIdForMultiWindow != globalAppdata.SeletedEntityId) {
        localStorage.setItem("GlobalEntityIdForMultiWindow", globalAppdata.SeletedEntityId);
        location.reload();
    }
}

function LoadHomeTab() {

    var PatientId = localStorage.getItem("SelectedPatientId");

    var hashSection = window.location.hash;
    var tabName = "mstrTabAdmin";
    var innerScreen = "";

    if (hashSection.indexOf("/") != -1) {
        splittedHashSection = hashSection.split("/");
        innerScreen = splittedHashSection[splittedHashSection.length - 1];
    }

    AppCommands.Load();
    var homeTab = GetTab('mstrTabDashBoard');

    var ajax_get = $.get(homeTab.Path, { cache: false }, function (content) {

        if (homeTab.Container) {
            $("#" + homeTab.Container).append(content);
            LoadCommands();

            if (innerScreen.length > 0) {
                $.when(SelectTab(tabName, 'false')).then(function () {
                    SelectTab(innerScreen, 'false');
                });
            } else {
                $.when(SelectTab(tabName, 'false')).then(function () {
                    var AdminSelectedPage = localStorage.getItem("AdminSelectedPage");
                    if (AdminSelectedPage && AdminSelectedPage != "") {
                        setTimeout(function () {
                            SelectTab(AdminSelectedPage, 'false');
                        }, 300);
                    }
                });
            }

        }
    }, "html");

}

function clearUserSession() {

    for (var i = 0; i < TabsArray.length; i++) {
        var key = TabsArray[i].TabID;
        store.clear(key);
    }
}

function LoadCommands() {

    var LoginAppUserId = globalAppdata['AppUserId'];
    var stored_AppUserId = store.fetch('LoginAppUserId');

    var SeletedEntityId = globalAppdata['SeletedEntityId'];
    var stored_entityid = store.fetch('entityid');
    if (stored_entityid != SeletedEntityId) {
        store.save('entityid', SeletedEntityId);
        MDVisionService.reloadLookups = true;
    }

    if (stored_AppUserId != LoginAppUserId || stored_entityid != SeletedEntityId) {
        store.save('LoginAppUserId', LoginAppUserId);
        utility.ClearDropdowns();
    }

    var defferedarray = $.map(TabsArray, function (tab, count) {
        if (tab.Path.length > 0) {
            if (tab.TabID == "schTabSearch")
                return LoadHTMLControl(tab);
        }

    });

    $.when.apply(null, defferedarray).then(function () {
        MDVisionService.reloadLookups = false;
        BackgroundLoaderShow(false);
    });
}

function LoadHTMLControl(tab) {

    var html = utility.getTabHtml(tab.TabID);
    if (html) {
        var dfd = new $.Deferred();
        dfd.resolve(html);
        if (tab.Container) {
            $("#" + tab.Container).append(html);
        }
        return dfd.promise();
    }
    else {

        var ajax_get = $.get(tab.Path, {
            cache: false
        }, function (content) {
            html = content;
            utility.saveTabHtml(tab.TabID, content);
            if (tab.Container) {
                $("#" + tab.Container).append(content);
            }
        }, "html");

        return ajax_get.promise();
    }
}

function SelectTab(tabId, IsMenu, Reload, UniqueId, TabTitle, event) {
    // if User Come from Patient Demographic/Insurance Tab
    if (params.DemographicAutoUpdateActiveTab) {
        GetAutoUpdateDemographicScreen(tabId, IsMenu, Reload, UniqueId, TabTitle, event);
    }
    else {    // all tabs other than Patient Demographic/Insurance
        SelectApplicationTab(tabId, IsMenu, Reload, UniqueId, TabTitle, event);
    }

    $('.content-body').removeClass('pb-none');
    $('.content-body').removeClass('pr-none');
    $('#btnDemographicLabel').hide();

    var hashSection = window.location.hash;
    if (tabId != "mstrTabAdmin") {
        localStorage.setItem("AdminSelectedPage", tabId);
        if (hashSection != "#Admin/" + tabId) {
            window.location.hash = "#Admin/" + tabId;
        }
    }

}

function SelectApplicationTab(tabId, IsMenu, Reload, UniqueId, TabTitle, event) {
    if (event != null) {
        event.stopPropagation();
    }
    if (params["PreviousTab"] == null)
        params["PreviousTab"] = GetTab(tabId);
    else
        params["PreviousTab"] = GetSelectedTab();
    if (params.ActionPanContainer == 'actionPanClinicalProgressNote') {
        $("#pnlClinicalProgressNote #track").remove();
        $("#pnlClinicalProgressNote #AudioPlayer").load();
    }
    if (UniqueId && tabId.indexOf('Encounter') > -1) {
        params['VisitId'] = UniqueId;
    }

    if (tabId == "adminTab" + tabId.replace(/adminTab/gi, "")) {
        LoadAdminTab(tabId, IsMenu);
        return;
    }

    else
        SelectCurrentTab(tabId, IsMenu, Reload);

}

function LoadAdminTab(tabId, IsMenu) {
    var menuTab = tabId.replace(/Tab/gi, "Menu");
    var SelectedmenuObj = $(document.getElementById(menuTab));
    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    var ElementName = tabId.replace(/adminTab/gi, "");

    if (SelectedmenuObj.length > 0) {           //MA-666 when user has no rights of form, it'll not display
        AddAdminTab(ElementName, IsMenu);
        //var Tab = GetTab(tabId);
        var nodes = document.getElementById("mstrDivAdmin").getElementsByTagName('*');
        var varifyNode = document.getElementById(tabId);
        if (varifyNode == null) {
            var Element = '';
            if (nodes.length == 0) {
                if (tabId != 'adminTabFavoritesFamilyHistory' && tabId != 'adminTabFavoritesMedicalHistory' && tabId != 'adminTabFavoritesSurgicalHistory' && tabId != 'adminTabFavoritesHospitalizationHistory')
                    Element = $('<span class="btn btn-default btn-sm tab_selected tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></span>');
            }
            else {
                if (tabId != 'adminTabFavoritesFamilyHistory' && tabId != 'adminTabFavoritesMedicalHistory' && tabId != 'adminTabFavoritesSurgicalHistory' && tabId != 'adminTabFavoritesHospitalizationHistory') {
                    $(nodes).each(function () {
                        $(this).removeClass('active');
                        $(this).removeClass('tab_selected');
                        //$(this).switchClass("btn-sm active", "btn-sm");
                        //$(this).removeClass("");
                    });
                    Element = $('<span class="btn btn-default btn-sm tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></sapn>');
                }
                //Add Commands to Array....
            }
            if (tabId != 'adminTabFavoritesFamilyHistory' && tabId != 'adminTabFavoritesMedicalHistory' && tabId != 'adminTabFavoritesSurgicalHistory' && tabId != 'adminTabFavoritesHospitalizationHistory') {
                if (adminTabs == null) {
                    $("#" + "mstrDivAdmin").append(Element);
                    adminTabs = $("#" + "mstrDivAdmin").scrollTabs();
                }
                else {
                    adminTabs.addTab($(Element)[0].outerHTML, $("#mstrDivAdmin .btn").length);
                }
                $("#mstrDivAdmin .btn").removeClass('tab_selected');
                $($("#mstrDivAdmin .btn")[$("#mstrDivAdmin .btn").length - 1]).addClass('tab_selected');
            }
        }
        else {
            $(nodes).each(function () {
                var CurrentID = $(this).attr("id");
                if (CurrentID != null && CurrentID != tabId) {
                    $(this).removeClass('active');
                    $(this).removeClass('tab_selected');
                    //$(this).switchClass("btn-sm active", "btn-sm");
                }

                //$(this).removeClass("");
            });
        }
    }
}

//#region Clinical Related Code

function setParentChildMenuId(MenuToShow) {
    if (MenuToShow == "mainUL") {
        //$("#mainUL li.nav-expanded ul li.nav-active").parent().parent()
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

function AddAdminTab(ElementName, IsMenu) {
    var cmd = [];
    cmd.TabID = "adminTab" + ElementName;
    if (ElementName == "CDS") {
        cmd.PanelID = "pnlClinical" + ElementName;//panal id from opening html control
    }
    else if (ElementName == "Lab") {
        cmd.PanelID = "pnlClinical" + ElementName;//panal id from opening html control
    }
    else if (ElementName == "Macro") {
        cmd.PanelID = "pnlClinical" + ElementName;//panal id from opening html control
    }
    else if (ElementName == "AuditReport") {
        cmd.PanelID = "pnlClinical" + ElementName;//panal id from opening html control
    }
    else if (ElementName == "ReportHeader") {
        cmd.PanelID = "pnlClinical" + ElementName;//panal id from opening html control
    }
    else if (ElementName == "TemplateLetter") {
        cmd.PanelID = "pnlLetterTemplate";
    }
    else if (ElementName == "ProviderNote") {
        cmd.PanelID = "pnlClinicalProviderNoteTemplate";

    }
    else if (ElementName == "FavoritesComplaint") {
        cmd.PanelID = "pnlFavoriteComplaints";
    }
    else if (ElementName == "FavoritesTherapueticInjection") {
        cmd.PanelID = "pnlFavoriteTherapueticInjection";
    }
    else if (ElementName == "FavoritesMedication") {
        cmd.PanelID = "pnlFavoriteMedication";
    }
    else if (ElementName == "FavoritesVaccine") {
        cmd.PanelID = "pnlFavoriteVaccine";
    }
    else if (ElementName == "FavoritesProblems") {
        cmd.PanelID = "pnlFavoriteProblems";
    }
    else if (ElementName == "FavoritesProcedureOrder") {
        cmd.PanelID = "pnlFavoriteProcedureOrder";
    }
    else if (ElementName == "FavoritesProcedure") {
        cmd.PanelID = "pnlFavoriteProcedure";
    }
    else if (ElementName == "FavoritesConsultationOrder") {
        cmd.PanelID = "pnlFavoriteConsultationOrder";
    }
    else if (ElementName == "FavoritesCustomForms") {
        cmd.PanelID = "pnlFavoriteCustomForms";
    }
    else if (ElementName == "FavoritesRadiologyOrder") {
        cmd.PanelID = "pnlFavoriteRadiologyOrder";
    }
    else if (ElementName == "FavoritesLabOrder") {
        cmd.PanelID = "pnlFavoriteLabOrder";
    }
    else if (ElementName == "PhysicalExamTemplate") {
        cmd.PanelID = "pnlPhysicalExamTemplate";
    }
    else if (ElementName == "VisitTypeDurationGroup") {
        cmd.PanelID = "pnlAdminVisitTypeDurationGroup";
    }
    else if (ElementName == "VisitTypeDurationGroup") {
        cmd.PanelID = "pnlAdmin_VisitTypeDurationGroupDetail";
    }
    else if (ElementName == "AOETemplate") {
        cmd.PanelID = "pnlAOETemplate";
    }
    else if (ElementName == "ProcedureTemplate") {
        cmd.PanelID = "pnlProcedureTemplate";
    }
    else if (ElementName == "ReviewOfSystemsTemplate") {
        cmd.PanelID = "pnlReviewOfSystemsTemplate";
    }
        //Start 31-03-2016 Humaira Yousaf for favorite history
    else if (ElementName == "FavoritesFamilyHistory") {
        cmd.PanelID = "pnlFavoriteFamilyHistory";
    }
    else if (ElementName == "FavoritesMedicalHistory") {
        cmd.PanelID = "pnlFavoriteMedicalHistory";
    }
    else if (ElementName == "FavoritesHospitalizationHistory") {
        cmd.PanelID = "pnlFavoriteHospitalizationHistory";
    }
    else if (ElementName == "FavoritesSurgicalHistory") {
        cmd.PanelID = "pnlFavoriteSurgicalHistory";
    }
        //Start 30-03-2016 Humaira Yousaf for Favorite History
    else if (ElementName == "FavoritesHistory") {
        cmd.PanelID = "pnlFavoriteHistory";
    }
        //End 30-03-2016 Humaira Yousaf for Favorite History
        //End 31-03-2016 Humaira Yousaf for favorite history
    else if (ElementName == "ReviewOfSystemsDataTemplate") {
        cmd.PanelID = "pnlReviewOfSystemsDataTemplate";
    }
        //Start 13-6-2016 Abid Ali for Physial Exam Data Template
    else if (ElementName == "PhysicalExamDataTemplate") {
        cmd.PanelID = "pnlPhysicalExamDataTemplate";
    }
        //End 13-6-2016 Abid Ali for Physial Exam Data Template

    else if (ElementName == "LotNumber") {
        cmd.PanelID = "pnlImmunization_LotNumber";
    }
    else if (ElementName == "ImmunizationCategory") {
        cmd.PanelID = "pnlImmunization_Category";
    }
    else if (ElementName == "ImmunizationVaccineCrosswalk") {
        cmd.PanelID = "pnlImmunization_VaccineCrosswalk";
    }
    else if (ElementName == "ImmunizationAddImmInj") {
        cmd.PanelID = "pnlImmunization_ImmunizationAddImmInj";
    }
    else if (ElementName == "ImmunizationManufacturer") {
        cmd.PanelID = "pnlImmunization_Manufacturer";
    }
    else if (ElementName == "ImmunizationScheduleSetup") {
        cmd.PanelID = "pnlImmunization_ScheduleSetup";
    }
    else if (ElementName == "ImmunizationLotManagement") {
        cmd.PanelID = "pnlImmunization_LotNumber";
    }
    else if (ElementName == "ImmunizationRegistery") {
        cmd.PanelID = "pnlImmunization_Registery";
    }
    else if (ElementName == "ImmunizationAlertConfiguration") {
        cmd.PanelID = "pnlImmunization_AlertConfiguration";
    }
    else if (ElementName == "CustomForms") {
        cmd.PanelID = "pnlClinicalCustomForms";
    }
    else if (ElementName == "OrderSets") {
        cmd.PanelID = "pnlClinicalOrderSets";
    }
    else if (ElementName == "HPITemplate") {
        cmd.PanelID = "pnlHPITemplate";
    }
    else if (ElementName == "HPIFindings") {
        cmd.PanelID = "pnlAdminHPIFindings";
    }
    else if (ElementName == "HPISymptoms") {
        cmd.PanelID = "pnlAdminHPISymptoms";
    }
    else {
        cmd.PanelID = "pnlAdmin" + ElementName;//panal id from opening html control
    }

    cmd.MasterTabID = "mstrTabAdmin";
    //if (ElementName == "TemplateLetter") {
    //    cmd.ParentTabID = "adminMenuTemplate";
    //}
    //else {
    //    cmd.ParentTabID = "mstrTabAdmin";
    //}
    cmd.ParentTabID = "mstrTabAdmin";


    if (ElementName == "User" || ElementName == "PrivilegeGroup" || ElementName == "SecurityRoles") {
        cmd.ParentChildTabID = "adminMenuSecurity";
    }
    else if (ElementName == "Provider" || ElementName == "Practice" || ElementName == "Facility" || ElementName == "Resources" || ElementName == "Specialty" || ElementName == "BillingProvider" || ElementName == "BillingProviderSettings" || ElementName == "ReferringProvider" || ElementName == "CDS" || ElementName == "Lab" || ElementName == "AuditReport" || ElementName == "ReportHeader") {
        cmd.ParentChildTabID = "adminMenuProfiles";
    }
    else if (ElementName == "CPTCode" || ElementName == "ICD" || ElementName == "Modifier" || ElementName == "ProcedureCategory" || ElementName == "TypeOfService" || ElementName == "PlaceOfService" || ElementName == "RevenueCode") {
        cmd.ParentChildTabID = "adminMenuCodes";
    }
    else if (ElementName == "Insur" || ElementName == "InsurancePlan" || ElementName == "PlanCategory" || ElementName == "InsurancePlanAddress") {
        cmd.ParentChildTabID = "adminMenuInsurance";
    }
    else if (ElementName == "LedgerAccount" || ElementName == "ERAAdjustmentCodes") {
        cmd.ParentChildTabID = "adminMenuPaymentSetup";
    }
    else if (ElementName == "EDISubmitInsurance" || ElementName == "EDIEligibilityInsurance" || ElementName == "EDIClaimStatusInsurance" || ElementName == "ClearingHouse" || ElementName == "SubmitterSetup" || ElementName == "EDITaxIDSetup" || ElementName == "EDIServiceHandle" || ElementName == "EDIReceiver") {
        cmd.ParentChildTabID = "adminMenuEDI";
    }
    else if (ElementName == "FollowUpReason" || ElementName == "FollowUpType" || ElementName == "FollowUpRemittanceCode" || ElementName == 'FollowUpAction' || ElementName == 'FollowUpClaimStatusCode' || ElementName == 'FollowUpClaimStatusCategoryCode' || ElementName == 'FollowUpCodesMapping' || ElementName == 'FollowUpGroup' || ElementName == "FollowUpAdjustmentCode") {
        cmd.ParentChildTabID = "adminMenuFollowUp";
    }
    else if (ElementName == "BasicFeeGroup" || ElementName == "FeeGroup" || ElementName == "PlanFeeLink" || ElementName == "BasicFeeSchedule" || ElementName == "ProcedureFeeSchedule" || ElementName == "POSFeeSchedule") {
        cmd.ParentChildTabID = "adminMenuFeeSchedule";
    }
    else if (ElementName == "Holidays" || ElementName == "BlockHours" || ElementName == "ScheduleReason" || ElementName == "ProviderSchedule" || ElementName == "ResourceSchedule" || ElementName == "AppointmentStatus") {
        cmd.ParentChildTabID = "adminMenuSchedule";
    }
    else if (ElementName == "SupperBill") {
        cmd.ParentChildTabID = "adminMenuBills";
    }
    else if (ElementName == "StatementMessage" || ElementName == "StatementGroup") {
        cmd.ParentChildTabID = "adminMenuBills";
    }
    else if (ElementName == "Folder" || ElementName == "FolderType") {
        cmd.ParentChildTabID = "adminMenuDocumentSetup";
    }

    else if (ElementName == "RemindersTemplates") {
        cmd.ParentChildTabID = "adminMenuReminders";
    }
        //--------------
        //else if (ElementName == "ERAAdjustmentCodes") {
        //    cmd.ParentChildTabID = "adminMenuERAAdjustmentCodes";
        //}

    else if (ElementName == "HL7ADT" || ElementName == "HL7SIU" || ElementName == "HL7DFT") {
        cmd.ParentChildTabID = "adminMenuHL7";
    }
    else if (ElementName == "CCMTemplates" || ElementName == "CCMICDGroups" || ElementName == "CCMCareTeam") {
        cmd.ParentChildTabID = "adminMenuCCM";
    }
    else if (ElementName == "TemplateLetter" || ElementName == "PhysicalExamTemplate" || ElementName == "AOETemplate"
        || ElementName == "ReviewOfSystemsTemplate" || ElementName == "ProviderNote" || ElementName == "HPITemplate"
        || ElementName == "HPIFindings" || ElementName == "HPISymptoms" || ElementName == "ProcedureTemplate") {
        cmd.ParentChildTabID = "adminMenuTemplate";
    }
    else if (ElementName == "FavoritesFamilyHistory" || ElementName == "FavoritesMedicalHistory" || ElementName == "FavoritesSurgicalHistory" || ElementName == "FavoritesComplaint" || ElementName == "FavoritesTherapueticInjection" || ElementName == "FavoritesVaccine" || ElementName == "FavoritesProcedureOrder" || ElementName == "FavoritesConsultationOrder" || ElementName == "FavoritesCustomForms" || ElementName == "FavoritesRadiologyOrder" || ElementName == "FavoritesLabOrder" || ElementName == "FavoritesHistory" || ElementName == "FavoritesProblems" || ElementName == "FavoritesMedication") {
        cmd.ParentChildTabID = "adminMenuFavorites";
    }
    else if (ElementName == "LotNumber") {
        cmd.ParentChildTabID = "adminMenuLotNumber";
    }
    else if (ElementName == "Category") {
        cmd.ParentChildTabID = "adminMenuCategory";
    }
    else if (ElementName == "ReviewOfSystemsDataTemplate") {
        cmd.ParentChildTabID = "adminMenuDataTemplate";
    }
    else if (ElementName == "PhysicalExamDataTemplate") {
        cmd.ParentChildTabID = "adminMenuDataTemplate";
    }
    else if (ElementName == "CDS") {
        cmd.ParentChildTabID = "adminMenuClinical";
    }
    else if (ElementName == "Lab") {
        cmd.ParentChildTabID = "adminMenuClinical";
    }
    else if (ElementName == "Macro") {
        cmd.ParentChildTabID = "adminMenuClinical";
    }
    else if (ElementName == "AuditReport") {
        cmd.ParentChildTabID = "adminMenuClinical";
    }
    else if (ElementName == "ReportHeader") {
        cmd.ParentChildTabID = "adminMenuClinical";
    }
    //--------------
    //Start 24-02-2016 Muhammad Arshad CDS related changes
    if (ElementName == "CDS") {
        cmd.ContainerControlID = "Clinical_" + ElementName;
    }
    else if (ElementName == "Lab") {
        cmd.ContainerControlID = "Clinical_" + ElementName;
    }
    else if (ElementName == "Macro") {
        cmd.ContainerControlID = "Clinical_" + ElementName;
    }
    else if (ElementName == "AuditReport") {
        cmd.ContainerControlID = "Clinical_" + ElementName;
    }
    else if (ElementName == "ReportHeader") {
        cmd.ContainerControlID = "Clinical_" + ElementName;
    }
    else if (ElementName == "TemplateLetter") {
        cmd.ContainerControlID = "Letter_Template";
    }
    else if (ElementName == "LotNumber") {
        cmd.ContainerControlID = "Immunization_LotNumber";
    }
    else if (ElementName == "Category") {
        cmd.ContainerControlID = "Immunization_Category";
    }
    else if (ElementName == "ProviderNote") {
        cmd.ContainerControlID = "Clinical_Provider_Note_Template";
    }
    else if (ElementName == "FavoritesComplaint") {
        cmd.ContainerControlID = "Favorite_Complaints";
    }
    else if (ElementName == "FavoritesTherapueticInjection") {
        cmd.ContainerControlID = "Favorite_TherapueticInjection";
    }

    else if (ElementName == "FavoritesVaccine") {
        cmd.ContainerControlID = "Favorite_Vaccine";
    }
    else if (ElementName == "FavoritesProblems") {
        cmd.ContainerControlID = "Favorite_Problems";
    }
    else if (ElementName == "FavoritesMedication") {
        cmd.ContainerControlID = "Favorite_Medication";
    }
    else if (ElementName == "FavoritesProcedureOrder") {
        cmd.ContainerControlID = "Favorite_ProcedureOrder";
    }
    else if (ElementName == "FavoritesProcedure") {
        cmd.ContainerControlID = "Favorite_Procedure";
    }
    else if (ElementName == "FavoritesConsultationOrder") {
        cmd.ContainerControlID = "Favorite_ConsultationOrder";
    }
    else if (ElementName == "FavoritesCustomForms") {
        cmd.ContainerControlID = "Favorite_CustomForms";
    }
    else if (ElementName == "FavoritesRadiologyOrder") {
        cmd.ContainerControlID = "Favorite_RadiologyOrder";
    }
    else if (ElementName == "FavoritesLabOrder") {
        cmd.ContainerControlID = "Favorite_LabOrder";
    }
    else if (ElementName == "PhysicalExamTemplate") {
        cmd.ContainerControlID = "PhysicalExamTemplate";
    }
    else if (ElementName == "VisitTypeDurationGroup") {
        cmd.ContainerControlID = "VisitTypeDurationGroup";
    }
    else if (ElementName == "VisitTypeDetail") {
        cmd.ContainerControlID = "VisitTypeDetail";
    }
    else if (ElementName == "AOETemplate") {
        cmd.ContainerControlID = "AOETemplate";
    }
    else if (ElementName == "ProcedureTemplate") {
        cmd.ContainerControlID = "ProcedureTemplate";
    }
    else if (ElementName == "ReviewOfSystemsTemplate") {
        cmd.ContainerControlID = "ReviewOfSystemsTemplate";
    }
        //Start 31-03-2016 Humaira Yousaf for favorite list
    else if (ElementName == "FavoritesFamilyHistory") {
        cmd.ContainerControlID = "Favorite_FamilyHistory";
    }
    else if (ElementName == "FavoritesMedicalHistory") {
        cmd.ContainerControlID = "Favorite_MedicalHistory";
    }
    else if (ElementName == "FavoritesSurgicalHistory") {
        cmd.ContainerControlID = "Favorite_SurgicalHistory";
    }
    else if (ElementName == "FavoritesHospitalizationHistory") {
        cmd.ContainerControlID = "Favorite_HospitalizationHistory";
    }
        //Start 30-03-2016 Humaira Yousaf for Favorite History
    else if (ElementName == "FavoritesHistory") {
        cmd.ContainerControlID = "Favorite_History";
    }
        //End 30-03-2016 Humaira Yousaf for Favorite History
        //End 31-03-2016 Humaira Yousaf for favorite list
    else if (ElementName == "ReviewOfSystemsDataTemplate") {
        cmd.ContainerControlID = "ReviewOfSystemsDataTemplate";
    }

    else if (ElementName == "PhysicalExamDataTemplate") {
        cmd.ContainerControlID = "PhysicalExamDataTemplate";
    }
    else if (ElementName == "ImmunizationCategory") {
        cmd.ContainerControlID = "Immunization_Category";
    }
    else if (ElementName == "ImmunizationVaccineCrosswalk") {
        cmd.ContainerControlID = "Immunization_VaccineCrosswalk";
    }
    else if (ElementName == "ImmunizationAddImmInj") {
        cmd.ContainerControlID = "Immunization_ImmunizationAddImmInj";
    }
    else if (ElementName == "ImmunizationManufacturer") {
        cmd.ContainerControlID = "Immunization_Manufacturer";
    }
    else if (ElementName == "ImmunizationScheduleSetup") {
        cmd.ContainerControlID = "Immunization_ScheduleSetup";
    }
    else if (ElementName == "ImmunizationLotManagement") {
        cmd.ContainerControlID = "Immunization_LotNumber";
    }
    else if (ElementName == "ImmunizationRegistery") {
        cmd.ContainerControlID = "Immunization_Registery";
    }
    else if (ElementName == "ImmunizationAlertConfiguration") {
        cmd.ContainerControlID = "Immunization_AlertConfiguration";
    }
    else if (ElementName == "CustomForms") {
        cmd.ContainerControlID = "Clinical_CustomForms";
    }
    else if (ElementName == "OrderSets") {
        cmd.ContainerControlID = "Clinical_OrderSets";
    }
    else if (ElementName == "HPITemplate") {
        cmd.ContainerControlID = "HPITemplate";
    }
    else if (ElementName == "HPIFindings") {
        cmd.ContainerControlID = "Clinical_" + ElementName;
    }
    else if (ElementName == "HPISymptoms") {
        cmd.ContainerControlID = "Clinical_" + ElementName;
    }
    else {
        cmd.ContainerControlID = "Admin_" + ElementName;
    }
    //End 24-02-2016 Muhammad Arshad CDS related changes
    cmd.Selected = true;
    cmd.Container = "ctrlPanAdmin";// in which container it will be open
    if ((ElementName == "ERAAdjustmentCodes")) {
        cmd.Path = "./Controls/Admin/ERA/Admin_" + ElementName + ".html";
    }
    else if (ElementName == "FollowUpReason" || ElementName == "FollowUpType" || ElementName == "FollowUpRemittanceCode" || ElementName == 'FollowUpAction' || ElementName == 'FollowUpClaimStatusCode' || ElementName == 'FollowUpClaimStatusCategoryCode' || ElementName == 'FollowUpCodesMapping' || ElementName == 'FollowUpGroup' || ElementName == "FollowUpAdjustmentCode") {
        cmd.Path = "./Controls/Admin/FollowUp/Admin_" + ElementName + ".html";
    }
    else if (ElementName == "StatementMessage" || ElementName == "StatementGroup") {
        cmd.Path = "./Controls/Admin/PatientStatement/Admin_" + ElementName + ".html";
    }
    else if (ElementName == "CCMTemplates" || ElementName == "CCMICDGroups" || ElementName == "CCMCareTeam") {
        cmd.Path = "./Controls/Admin/CCM/Admin_" + ElementName + ".html";
    }
        //Start 24-02-2016 Muhammad Arshad CDS related changes
    else if (ElementName == "CDS") {
        cmd.Path = "./EMR/HTML/Clinical/CDS/Clinical_" + ElementName + ".html";
    }
    else if (ElementName == "Lab") {
        cmd.Path = "./EMR/HTML/Clinical/Lab/Clinical_" + ElementName + ".html";
    }
    else if (ElementName == "Macro") {
        cmd.Path = "./EMR/HTML/Clinical/Macros/Clinical_" + ElementName + ".html";
    }
    else if (ElementName == "AuditReport") {
        cmd.Path = "./EMR/HTML/Clinical/AuditReport/Clinical_" + ElementName + ".html";
    }
    else if (ElementName == "ReportHeader") {
        cmd.Path = "./EMR/HTML/Clinical/ReportHeader/Clinical_SearchReportHeaderTemplate.html";
    }
    else if (ElementName == "TemplateLetter") {
        cmd.Path = "./EMR/HTML/Clinical/TemplateBuilderNew/Letter_Template.html";
    }
    else if (ElementName == "LotNumber") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_LotNumber.html";
    }
    else if (ElementName == "Category") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_Category.html";
    }
    else if (ElementName == "ProviderNote") {
        cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_Provider_Note_Template.html";
    }
    else if (ElementName == "ROSTemplateDetailRevamp") {
        cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/ROSTemplateDetailRevamp.html";
    }
    else if (ElementName == "FavoritesComplaint") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_Complaints.html";
    }
    else if (ElementName == "FavoritesTherapueticInjection") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_TherapueticInjection.html";
    }
    else if (ElementName == "FavoritesVaccine") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_Vaccine.html";
    }
    else if (ElementName == "FavoritesProblems") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_Problems.html";
    }
    else if (ElementName == "FavoritesMedication") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_Medication.html";
    }
    else if (ElementName == "FavoritesProcedureOrder") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_ProcedureOrder.html";
    }
    else if (ElementName == "FavoritesProcedure") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_Procedure.html";
    }
    else if (ElementName == "FavoritesConsultationOrder") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_ConsultationOrder.html";
    }
    else if (ElementName == "FavoritesCustomForms") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_CustomForms.html";
    }
    else if (ElementName == "FavoritesRadiologyOrder") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_RadiologyOrder.html";
    }
    else if (ElementName == "FavoritesLabOrder") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_LabOrder.html";
    }
    else if (ElementName == "PhysicalExamTemplate") {
        cmd.Path = "./EMR/HTML/Clinical/Templates/PhysicalExamTemplate.html";
    }
    else if (ElementName == "VisitTypeDurationGroup") {
        cmd.Path = "./Controls/Admin/Admin_VisitTypeDurationGroup.html";
    }
    else if (ElementName == "VisitTypeDetail") {
        cmd.Path = "./Controls/Admin/Admin_VisitTypeDurationGroup_Detail.html";
    }
    else if (ElementName == "AOETemplate") {
        cmd.Path = "./EMR/HTML/Clinical/Templates/AOETemplate.html";
    }
    else if (ElementName == "ProcedureTemplate") {
        cmd.Path = "./EMR/HTML/Clinical/Templates/ProcedureTemplate.html";
    }
    else if (ElementName == "ReviewOfSystemsTemplate") {
        cmd.Path = "./EMR/HTML/Clinical/Templates/ReviewOfSystemsTemplate.html";
    }
        //Start 31-03-2016 Humaira Yousaf for favorite history
    else if (ElementName == "FavoritesFamilyHistory") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_FamilyHistory.html";
    }
    else if (ElementName == "FavoritesMedicalHistory") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_MedicalHistory.html";
    }
    else if (ElementName == "FavoritesSurgicalHistory") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_SurgicalHistory.html";
    }
    else if (ElementName == "FavoritesHospitalizationHistory") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_HospitalizationHistory.html";
    }
        //Start 30-03-2016 Humaira Yousaf for Favorite History
    else if (ElementName == "FavoritesHistory") {
        cmd.Path = "./EMR/HTML/Clinical/Favorites/Favorite_History.html";
    }
        //End 30-03-2016 Humaira Yousaf for Favorite History
        //End 31-03-2016 Humaira Yousaf for favorite history
    else if (ElementName == "ReviewOfSystemsDataTemplate") {
        cmd.Path = "./EMR/HTML/Clinical/DataTemplates/ReviewOfSystemsDataTemplate.html";
    }
    else if (ElementName == "ImmunizationCategory") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_Category.html";
    }
    else if (ElementName == "ImmunizationVaccineCrosswalk") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_VaccineCrosswalk.html";
    }
    else if (ElementName == "ImmunizationAddImmInj") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_ImmunizationAddImmInj.html";
    }
    else if (ElementName == "ImmunizationManufacturer") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_Manufacturer.html";
    }
    else if (ElementName == "ImmunizationScheduleSetup") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_ScheduleSetup.html";
    }
    else if (ElementName == "ImmunizationLotManagement") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_LotNumber.html";
    }
    else if (ElementName == "ImmunizationRegistery") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_Registery.html";
    }
    else if (ElementName == "ImmunizationAlertConfiguration") {
        cmd.Path = "./EMR/HTML/Clinical/Immunization/Immunization_AlertConfiguration.html";
    }
    else if (ElementName == "PhysicalExamDataTemplate") {
        cmd.Path = "./EMR/HTML/Clinical/DataTemplates/PhysicalExamDataTemplate.html";
    }
    else if (ElementName == "CustomForms") {
        cmd.Path = "./EMR/HTML/Clinical/CustomForms/Clinical_CustomForms.html";
    }
    else if (ElementName == "OrderSets") {
        cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_OrderSets.html";
    }
    else if (ElementName == "HPITemplate") {
        cmd.Path = "./EMR/HTML/Clinical/Templates/HPITemplate.html";
    }
    else if (ElementName == "HPIFindings") {
        cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_HPIFindings.html";
    }
    else if (ElementName == "HPISymptoms") {
        cmd.Path = "./EMR/HTML/Clinical/Templates/Clinical_HPISymptoms.html";
    }
        //End 24-02-2016 Muhammad Arshad CDS related changes
    else {
        cmd.Path = "./Controls/Admin/Admin_" + ElementName + ".html";
    }
    //Start 24-02-2016 Muhammad Arshad CDS related changes
    if (ElementName == "CDS") {
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "Lab") {
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "Macro") {
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "AuditReport") {
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "ReportHeader") {
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "ImmunizationCategory") {
        cmd.ActionPanContainer = "actionPanImmunization_Category";
    }
    else if (ElementName == "ImmunizationScheduleSetup") {
        cmd.ActionPanContainer = "actionPanImmunization_ScheduleSetup";
    }
    else if (ElementName == "ImmunizationLotManagement") {
        cmd.ActionPanContainer = "actionPanImmunization_LotNumber";
    }
    else if (ElementName == "ImmunizationRegistery") {
        cmd.ActionPanContainer = "actionPanImmunization_Registery";
    }
    else if (ElementName == "ImmunizationAlertConfiguration") {
        cmd.ActionPanContainer = "actionPanImmunization_AlertConfiguration";
    }
    else if (ElementName == "TemplateLetter") {
        cmd.ActionPanContainer = "actionPanLetterTemplate";
    }
    else if (ElementName == "ProviderNote") {
        cmd.ActionPanContainer = "actionPanClinicalProviderNoteTemplate";
    }
    else if (ElementName == "FavoritesComplaint") {
        cmd.ActionPanContainer = "actionPanFavoriteComplaints";
    }
    else if (ElementName == "FavoritesTherapueticInjection") {
        cmd.ActionPanContainer = "actionPanFavoriteTherapueticInjection";
    }
    else if (ElementName == "FavoritesVaccine") {
        cmd.ActionPanContainer = "actionPanFavoriteVaccine";
    }
    else if (ElementName == "FavoritesProblems") {
        cmd.ActionPanContainer = "actionPanFavoriteProblems";
    }
    else if (ElementName == "FavoritesMedication") {
        cmd.ActionPanContainer = "actionPanFavoriteMedication";
    }
    else if (ElementName == "FavoritesProcedureOrder") {
        cmd.ActionPanContainer = "actionPanFavoriteProcedureOrder";
    }
    else if (ElementName == "FavoritesProcedure") {
        cmd.ActionPanContainer = "actionPanFavoriteProcedure";
    }
    else if (ElementName == "FavoritesConsultationOrder") {
        cmd.ActionPanContainer = "actionPanFavoriteConsultationOrder";
    }
    else if (ElementName == "FavoritesCustomForms") {
        cmd.ActionPanContainer = "actionPanFavoriteCustomForms";
    }
    else if (ElementName == "FavoritesRadiologyOrder") {
        cmd.ActionPanContainer = "actionPanFavoriteRadiologyOrder";
    }
    else if (ElementName == "FavoritesLabOrder") {
        cmd.ActionPanContainer = "actionPanFavoriteLabOrder";
    }
    else if (ElementName == "PhysicalExamTemplate") {
        cmd.ActionPanContainer = "actionPanPhysicalExamTemplate";
    }
    else if (ElementName == "VisitTypeDurationGroup") {
        cmd.ActionPanContainer = "actionPanAdminVisitTypeDurationGroup";
    }
    else if (ElementName == "VisitTypeDetail") {
        cmd.ActionPanContainer = "actionPanAdminVisitTypeDurationGroupDetail";
    }
    else if (ElementName == "AOETemplate") {
        cmd.ActionPanContainer = "actionPanAOETemplate";
    }
    else if (ElementName == "ProcedureTemplate") {
        cmd.ActionPanContainer = "actionPanProcedureTemplate";
    }
    else if (ElementName == "ReviewOfSystemsTemplate") {
        cmd.ActionPanContainer = "actionPanReviewOfSystemsTemplate";
    }
        //Start 31-03-2016 Humaira Yousaf for favorite history
    else if (ElementName == "FavoritesFamilyHistory") {
        cmd.ActionPanContainer = "actionPanFavoriteFamilyHistory";
    }
    else if (ElementName == "FavoritesHospitalizationHistory") {
        cmd.ActionPanContainer = "actionPanFavoriteHospitalizationHistory";
    }
    else if (ElementName == "FavoritesMedicalHistory") {
        cmd.ActionPanContainer = "actionPanFavoriteMedicalHistory";
    }
    else if (ElementName == "FavoritesSurgicalHistory") {
        cmd.ActionPanContainer = "actionPanFavoriteSurgicalHistory";
    }
        //Start 30-03-2016 Humaira Yousaf for Favorite History
    else if (ElementName == "FavoritesHistory") {
        cmd.ActionPanContainer = "actionPanFavoriteHistory";
    }
        //End 30-03-2016 Humaira Yousaf for Favorite History
        //End 31-03-2016 Humaira Yousaf for favorite history
    else if (ElementName == "ReviewOfSystemsDataTemplate") {
        cmd.ActionPanContainer = "actionPanReviewOfSystemsDataTemplate";
    }
    else if (ElementName == "PhysicalExamDataTemplate") {
        cmd.ActionPanContainer = "actionPanPhysicalExamDataTemplate";
    }

    else if (ElementName == "MeasureGroups") {
        cmd.ActionPanContainer = "actionPanPQRSMeasureGroups";
        cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/PQRS_" + ElementName + ".html";
        cmd.ContainerControlID = "PQRS_" + ElementName;
        cmd.ParentChildTabID = "adminMenuPQRS";
        cmd.PanelID = "pnlPQRSMeasureGroups";
    } else if (ElementName == "IndividualReporting") {
        cmd.ActionPanContainer = "actionPanPQRSIndividualReporting";
        cmd.Path = "./EMR/HTML/Clinical/PQRSAdmin/PQRS_" + ElementName + ".html";
        cmd.ParentChildTabID = "adminMenuPQRS";
        cmd.PanelID = "pnlPQRSIndividualReporting";
        cmd.ContainerControlID = "PQRS_" + ElementName;
    } else if (ElementName == "iTrackIndividualReporting") {
        cmd.ActionPanContainer = "actionPanIndividualReporting";
        cmd.Path = "./EMR/HTML/iTrack/iTrack_AdminPreferenceIndividualReporting.html";
        cmd.ParentChildTabID = "adminMenuMIPSSelectMeasures";
        cmd.PanelID = "pnlMIPSAdminPreferenceIndividualReporting";
        cmd.ContainerControlID = "iTrack_AdminPreferenceIndividualReporting";
    }
    else if (ElementName == "MIPSGroupReporting") {
    cmd.ActionPanContainer = "actionPanGroupReporting";
    cmd.Path = "./EMR/HTML/iTrack/iTrack_AdminPreferenceGroupReporting.html";
    cmd.ParentChildTabID = "adminMenuMIPSSelectMeasures";
    cmd.PanelID = "pnlMIPSAdminPreferenceGroupReporting";
    cmd.ContainerControlID = "iTrack_AdminPreferenceGroupReporting";
}
    else if (ElementName == "Systems") {
        cmd.ActionPanContainer = "actionPanAdminPESystems";
        cmd.Path = "./EMR/HTML/Clinical/PhysicalExam/Clinical_PhysicalExam" + ElementName + ".html";
        cmd.ParentChildTabID = "adminMenuClinicalQuestionnaire";
        cmd.PanelID = "pnlAdminPESystems";
        cmd.ContainerControlID = "Clinical_PhysicalExam" + ElementName;
    }

    else if (ElementName == "Observations") {
        cmd.ActionPanContainer = "actionPanAdminPEObservations";
        cmd.Path = "./EMR/HTML/Clinical/PhysicalExam/Clinical_PhysicalExam" + ElementName + ".html";
        cmd.ParentChildTabID = "adminMenuClinicalQuestionnaire";
        cmd.PanelID = "pnlAdminPEObservations";
        cmd.ContainerControlID = "Clinical_PhysicalExam" + ElementName;
    }
    else if (ElementName == "ROSSystems") {
        cmd.ActionPanContainer = "actionPanAdminROSSystems";
        cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/Clinical_" + ElementName + ".html";
        cmd.ParentChildTabID = "adminMenuClinicalQuestionnaire";
        cmd.PanelID = "pnlROSSystems";
        cmd.ContainerControlID = "Clinical_" + ElementName;
    }

    else if (ElementName == "MIPSIndividualProvider") {
        cmd.ActionPanContainer = "actionPanMIPSAdminPreferenceGroup";
        cmd.Path = "./EMR/HTML/iTrack/MIPSPreference_IndividualProvider.html";
        cmd.ParentChildTabID = "adminMenuMIPSPreference";
        cmd.PanelID = "pnlIndividualProvider";
        cmd.ContainerControlID = "MIPSPreference_IndividualProvider";
    }
    else if (ElementName == "MIPSGroupVirtualGroup") {
        cmd.ActionPanContainer = "actionPanIndividualProvider";
        cmd.Path = "./EMR/HTML/iTrack/MIPS_AdminPreferenceGroup.html";
        cmd.ParentChildTabID = "adminMenuMIPSPreference";
        cmd.PanelID = "pnlMIPSAdminPreferenceGroup";
        cmd.ContainerControlID = "MIPS_AdminPreferenceGroup";
    }
    else if (ElementName == "MIPSIndividualReporting") {
        cmd.ActionPanContainer = "actionPanIndividualReporting";
        cmd.Path = "./EMR/HTML/iTrack/iTrack_AdminPreferenceIndividualReporting.html";
        cmd.ParentChildTabID = "adminMenuMIPSPreference";
        cmd.PanelID = "pnlMIPSAdminPreferenceIndividualReporting";
        cmd.ContainerControlID = "iTrack_AdminPreferenceIndividualReporting";
    }
    else if (ElementName == "ROSCharatristics") {
        cmd.ActionPanContainer = "actionPanAdminROSChatristics";
        cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/Clinical_" + ElementName + ".html";
        cmd.ParentChildTabID = "adminMenuClinicalQuestionnaire";
        cmd.PanelID = "pnlAdminROSChatristics";
        cmd.ContainerControlID = "Clinical_" + ElementName;
    }
    else if (ElementName == "ROSTemplateRevamp") {
        cmd.ActionPanContainer = "actionPanROSTemplateRevamp";
        cmd.Path = "./EMR/HTML/Clinical/ReviewOfSystem/" + ElementName + ".html";
        cmd.ParentChildTabID = "adminMenuClinicalTemplate";
        cmd.PanelID = "pnlROSTemplateRevamp";
        cmd.ContainerControlID = "" + ElementName;
    }

    else if (ElementName == "DrugCodeCost") {
        cmd.ActionPanContainer = "actionPanAdminDrugCodeCost";
        cmd.Path = "./Controls/Admin/Admin_Drug_Code_Cost.html";
        cmd.ParentChildTabID = "adminMenuProfiles";
        cmd.PanelID = "pnlAdminDrugCodeCost";
        cmd.ContainerControlID = "Admin_Drug_Code_Cost";
    }
    else if (ElementName == "HPITemplate") {
        cmd.ActionPanContainer = "actionPanHPITemplate";
    }
    else if (ElementName == "HPIFindings") {
        cmd.ActionPanContainer = "actionPanAdminHPIFindings";
    }
    else if (ElementName == "HPISymptoms") {
        cmd.ActionPanContainer = "actionPanAdminHPISymptoms";
    }
    else if (ElementName == "ImmunizationVaccineCrosswalk") {
        cmd.ActionPanContainer = "actionPanImmunization_VaccineCrosswalk";
    }
    else if (ElementName == "CustomForms") {
        cmd.ActionPanContainer = "actionPanClinicalCustomForms";
    }
    else if (ElementName == "OrderSets") {
        cmd.ActionPanContainer = "actionPanClinicalOrderSets";
    }
    else {
        cmd.ActionPanContainer = "actionPanAdmin" + ElementName;//action pan of opening panal
    }
    //End 24-02-2016 Muhammad Arshad CDS related changes

    var Tab = GetTab(cmd.TabID);
    var AdminTab = cmd;

    if (typeof Tab != "undefined") {
        SelectCurrentTab(AdminTab.TabID, IsMenu);
    }
    else {
        AddAndSelectTab(AdminTab, IsMenu);

        //LoadHTMLControl(AdminTab);
        //for (var i = 0; i < TabsArray.length; i++) {
        //    if (isAdminTab(TabsArray[i])) {
        //        if (TabsArray[i].Path.length > 0)
        //            LoadHTMLControl(TabsArray[i]);
        //    }
        //}
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

    //reset the fistload of contorl
    //adnan maqbool, PMS-5193
    if (Tab.ContainerControlID)
        eval(Tab.ContainerControlID + '.bIsFirstLoad=true');


    //Unselected Previous menu
    //var UnselectedmenuTab = Tab.TabID.replace(/Tab/gi, "Menu");
    //var UnselectedmenuObj = $(document.getElementById(UnselectedmenuTab));
    //UnselectedmenuObj.removeClass("nav-active");
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

function RemoveCurrentTab(tabID) {
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].TabID == tabID) return TabsArray.splice(i, 1);
    }
}

var isInTimeOut = 0;
function SelectCurrentTab(tabID, IsMenu, Reload) {
    var Tab = GetTab(tabID);

    if ((tabID == "mstrTabClinical" || tabID.substring(0, tabID.indexOf("Tab")) == "clinical") && (params["patientID"] != null || params["patientID"] != "" || params["patientID"] != "-1")) {
        //SelectTab('mstrTabPatient', 'false');
        $("ul li[id*=mstrMenu]").hide();
        $("ul li[id*=mstrMenuClinical]").hide();
        // $("#mstrMenuClinical > a > Span").text('Face Sheet');
        if ($("html").hasClass("sidebar-left-collapsed")) {
            $("html").removeClass("sidebar-left-collapsed");
        }
        $("#anchorMainMenu").show();

        setTimeout(function () {
            try {
                $('#ctrlPanPatient').css('display', 'none');
                document.getElementById("ctrlPanPatient").style.display = "none !important";
                $('#ctrlPanPatient').css('display', 'none !important');

            }
            catch (ex) {
                console.log(ex);
            }

        }, 50);
        //
        if (tabID == "mstrTabClinical") {
            $("div[id*=mstrDiv]").hide();
            $.when(ClinicalMenuSettings.ClinicalMenuSettingsSearch(null)).then(function () {
                if ($("#mstrDivNotes").css("display") == "none") {
                    isInTimeOut = 1;
                    var selectedClinicalMenuParentLiId = $("#hfClinicalMenuParentLiId").val();
                    var selectedClinicalMenuChildLiId = $("#hfClinicalMenuChildLiId").val();

                    /*
                        Author Change: Muhammad Azhar Shahzad
                        Change Date: April 04,2016
                        Purpose: For Clinical Tab, patient should be selected for furthur operations
                    */
                    if ($('#PatientProfile #hfPatientId').val() != "") {
                        //if no clinical menu is not clicked yet, then to show Notes Screen
                        if (selectedClinicalMenuParentLiId != null && selectedClinicalMenuParentLiId != "" && selectedClinicalMenuChildLiId != null && selectedClinicalMenuChildLiId != "") {
                            if (GetSelectedTab("mstrTabClinical").ContainerControlID != 'Clinical_ProgressNote') {

                                $("li#" + selectedClinicalMenuParentLiId + " a:first").trigger("click");
                                document.getElementById("ctrlPanPatient").style.display = "none !important";
                                setTimeout(function () {
                                    if (GetCurrentMasterTab().TabID == 'mstrTabClinical') {
                                        if ($("li#" + selectedClinicalMenuParentLiId).hasClass("nav-expanded") == false) {
                                            $("li#" + selectedClinicalMenuParentLiId).addClass("nav-expanded")
                                        }
                                        $("li#" + selectedClinicalMenuChildLiId + " a:first").trigger("click");
                                    }
                                    $('#ctrlPanPatient').css('display', 'none');
                                    document.getElementById("ctrlPanPatient").style.display = "none !important";
                                    $('#ctrlPanPatient').css('display', 'none !important');

                                    isInTimeOut = 0;
                                }, 10);
                            } else {
                                Clinical_Notes.AddProgressNoteTab();
                            }
                        }
                        else {
                            //Start 16-12-2015 Muhammad Arshad Load FaceSheet while clinical tab is loaded
                            //setTimeout(function () { $("#ClinicalUL li#clinicalMenuNotes a:first").trigger("click"); isInTimeOut = 0; }, 1000);
                            //$("#ClinicalUL li#clinicalMenuFaceSheet a:first").trigger("click")
                            setTimeout(function () {
                                if (GetCurrentMasterTab().TabID == 'mstrTabClinical' && !(GetSelectedTab("mstrTabClinical").TabID == 'clinicalTabProgressNote')) {
                                    $("#ClinicalUL li#clinicalMenuFaceSheet a:first").trigger("click");
                                }
                                //
                                $('#ctrlPanPatient').css('display', 'none');
                                document.getElementById("ctrlPanPatient").style.display = "none !important";
                                $('#ctrlPanPatient').css('display', 'none !important');

                                isInTimeOut = 0;
                            }, 10);
                            //End 16-12-2015 Muhammad Arshad Load FaceSheet while clinical tab is loaded
                        }


                    }

                    else if (tabID == "mstrTabClinical" && (params["patientID"] == null || params["patientID"] == "" || params["patientID"] == "-1")) {
                        LoadActionPan('Patient_Search', null);
                        if ($('#PatientProfile').css('display') != 'none') {
                            $('#PatientProfile').css('display', 'none');
                        }
                    }
                }
            });

        }
    }
    else {
        $("ul li[id*=mstrMenu]").show();
        $("#mstrMenuClinical").show();
        $("#mstrMenuClinical > a > Span").text('Clinical');
        $("#ClinicalUL").hide();
        //$("#ctrlPanClinical").html("")
        $("#anchorMainMenu").hide();
        $("#mstrDivFaceSheet, #mstrDivMedical, #mstrDivHistroy, #mstrDivNotes, #mstrDivOrders, #mstrDivSpecialities, #mstrDivTemplateBuilder, #mstrDivMiscellaneous, #mstrDivWomenHealth").hide();
    }

    if (typeof Tab == "undefined")
        return;
    var SelectedtabObj = $(document.getElementById(Tab["TabID"]));
    var SelectedpnlObj = $(document.getElementById(Tab["PanelID"]));
    var SelectedContObj = $(document.getElementById(Tab["Container"]));
    if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "")
        SelectedpnlObj = $('#' + Tab["Container"] + ' #' + Tab["PanelID"]);
    else
        SelectedpnlObj = $(document.getElementById(Tab["PanelID"]));

    _tempCurrenttab = SelectedpnlObj;


    selectedtab = null;
    //Select Active Tab
    //if (Tab["PanelID"] != "" && Tab["MasterTabID"] != "")
    //    SelectedpnlObj = $('#' + Tab["Container"] + ' #' + Tab["PanelID"]);
    //else
    //    SelectedpnlObj = $(document.getElementById(Tab["PanelID"]));

    SelectedMenuAndTab(Tab, IsMenu);
    //else if ($(SelectedtabObj).hasClass("active") == false)
    //    $(SelectedtabObj).addClass("active");
    if (Tab.MasterTabID == "mstrTabAdmin") {
        var $listexpended = $("ul li[id*=mstrMenuAdmin]").find("ul:first li");
        $listexpended.each(function (index, li) {
            if ($(this).hasClass("nav-expanded nav-parent") == true || $(this).hasClass("nav-parent nav-expanded") == true) {
                var active = $(this).find("ul li").filter(function () { if ($(this).hasClass("nav-parent nav-active") || $(this).hasClass("nav-active")) return $(this); });
                if (active.length == 0)
                    if (Tab["TabID"] != 'adminTabFavoritesFamilyHistory' && Tab["TabID"] != 'adminTabFavoritesMedicalHistory' && Tab["TabID"] != 'adminTabFavoritesSurgicalHistory' && Tab["TabID"] != 'adminTabFavoritesHospitalizationHistory') {
                        $(this).removeClass("nav-parent nav-expanded").addClass("nav-parent");
                    }


            }
        });
    }
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].MasterTabID == Tab.MasterTabID && TabsArray[i].ParentTabID == Tab.ParentTabID && TabsArray[i].TabID != Tab.TabID) {
            TabsArray[i]["Selected"] = false;
            TabsArray[i]["Active"] = false;

            var UnselectedtabObj = $(document.getElementById(TabsArray[i]["TabID"]));
            var UnselectedContObj = $(document.getElementById(TabsArray[i]["Container"]));

            var UnselectedpnlObj

            if (TabsArray[i]["PanelID"] != "" && TabsArray[i]["MasterTabID"] != "")
                UnselectedpnlObj = $('#' + TabsArray[i]["Container"] + ' #' + TabsArray[i]["PanelID"]);
            else
                UnselectedpnlObj = $(document.getElementById(TabsArray[i]["PanelID"]));


            UnSelectedMenuAndTab(TabsArray[i], IsMenu);


            if (UnselectedpnlObj.css('display') != 'none') {
                UnselectedpnlObj.hide('fade', { direction: 'left', easing: 'easeInOutElastic' }, 200, showCurrentTab);
            }
            $(UnselectedContObj).css("display", "none");
        }
    }



    if (!$(SelectedpnlObj).css('display') || $(SelectedpnlObj).css('display') == 'none') {
        $(SelectedpnlObj).show('fade', 100);
    }

    if (!$(SelectedContObj).css('display') || $(SelectedContObj).css('display') == 'none') {
        $(SelectedContObj).css("display", "");
    }

    setTimeout(function () {
        $.each($('.input-group').find("input[type='text']"), function () {

            if (($(this).attr("onblur") != null && $(this).attr("onblur") != "" && $(this).attr("onblur").indexOf("utility.validateautocomplete" >= 0)) || ($(this).attr("oninput"))) {
                //  if ($(this).attr("onblur").indexOf("utility.validateautocomplete" >= 0)) {
                $(this).on("autocompleteopen", function (event, ui) {
                    if ($(this).closest(".modal-dialog").length == 0) {
                        $(this).autocomplete('widget').zIndex("1018");
                    } else {
                        $(this).autocomplete("option", "appendTo", "#" + $(this).closest('form').attr('id'));

                    }
                });
                //}

            }

        });

        //$(".ui-autocomplete-input").on("autocompleteopen", function (event, ui) {
        //    if ($(this).closest(".modal-dialog").length == 0)
        //        $(this).autocomplete('widget').zIndex("1018");
        //});
    }, 500);

    Tab["Selected"] = true;
    Tab["Active"] = true;

    //showChildTab(Tab);
    var selectedChildTab = GetSelectedTab(Tab.ParentTabID);
    if (Tab.MasterTabID != "") {

        if (selectedChildTab) {

            $('#' + selectedChildTab.PanelID).show();
            Tab.ContainerControlID = selectedChildTab.ContainerControlID;
            Tab.Container = selectedChildTab.Container;
        }
    }

    //means master tab clicked
    if (Tab.MasterTabID == "") {

        if (selectedChildTab) {
            $('#' + selectedChildTab.PanelID).show();
            Tab.ContainerControlID = selectedChildTab.ContainerControlID;
            Tab.Container = selectedChildTab.Container;
        }
    }

    LoadControl(Tab, params);

    if (SchFirstLoad == true && Tab != null && Tab.TabID == "mstrTabSchedule") {
        SchFirstLoad = false;
        switch (globalAppdata["PreferredSchScreenName"]) {
            case 'Wait List':
                SelectTab('schTabWaitList', 'false');
                break;
            case 'Scheduler Search':
                SelectTab('schTabSearch', 'false');
                break;
            case 'Schedule Group':
                SelectTab('schTabMultipleView', 'false');
                break;
            default:
                // SelectTab('schTabCalendar', 'false');
                break;
        }
    }

    if (Tab != null && Tab.TabID == "mstrTabBilling" && Tab.MasterTabID == "") {
        BillingFirstLoad = false;
        switch (globalAppdata["PreferredBillingScreenName"]) {
            case 'Charge Batch':
                SelectTab('billTabChargeBatchSearch', 'true');
                break;
            case 'Charges':
                SelectTab('billTabChargeSearch', 'true');
                break;
            case 'Claim Submission':
                SelectTab('billTabClaimSubmission', 'true');
                break;
            case 'Copay Receipt':
                SelectTab('billTabCopayReceipt', 'true');
                break;
            case 'EDI Report':
                SelectTab('billTabEDIReport', 'true');
                break;
            case 'ERA':
                SelectTab('billTabERA', 'true');
                break;
            case 'FollowUp Insurance AR':
                SelectTab('billTabFollowUpInsuranceAR', 'true');
                break;
            case 'FollowUp Patient AR':
                SelectTab('billTabFollowUpPatientAR', 'true');
                break;
            case 'Patient Statement':
                SelectTab('billTabPatientStatement', 'true');
                break;
            case 'Payment Batch':
                SelectTab('billTabPaymentBatchSearch', 'true');
                break;
            case 'Payment Posting':
                SelectTab('billTabPaymentPosting', 'true');
                break;
            case 'UnClaimed Appointments':
                SelectTab('billTabUnClaimedAppointment', 'true');
                break;
            case 'Out of Office Visits':
                SelectTab('billTabOutOfOfficeVisits', 'true');
                break;
            default:
                // SelectTab('schTabCalendar', 'false');
                break;
        }
    }
    if (Tab.TabID != 'mstrTabReports') {
        setTimeout(function () {
            //    SelectedpnlObj.find('form :input:not(hidden)').each(function (i) { $(this).attr('tabindex', i + 1); });
            if ($(SelectedpnlObj).attr('id') != null && $(SelectedpnlObj).attr('id').toLowerCase().indexOf("paymentposting") < 0 && $(SelectedpnlObj).attr('id').toLowerCase().indexOf("clinicalauditreport") < 0)
                SelectedpnlObj.find("form :input:not(button):not(hidden):not([data-plugin-timepicker]):not([data-plugin-datepicker]):not([data-plugin-keyboard-numpad]):not([id*='date']):not([id*='Date']):not([id*='dtp']):enabled:visible:first").focus();
        }, 400);
    }

    if (Tab != null && Tab.MasterTabID != null && Tab.MasterTabID != "" && Tab.MasterTabID != 'mstrTabClinical') {
        setParentChildMenuId("mainUL");
    }

}

function SelectedMenuAndTab(Tab, IsMenu) {
    var SelectedtabObj = $(document.getElementById(Tab.TabID));

    var str = Tab["TabID"];
    var menuTab = str.replace(/Tab/gi, "Menu");
    var SelectedmenuObj = $(document.getElementById(menuTab));


    //parent menu
    if (Tab.MasterTabID == "" && IsMenu == 'false') {
        if ($(SelectedmenuObj).hasClass("nav-parent active") == true)
            $(SelectedmenuObj).removeClass("nav-parent active").addClass("nav-parent nav-expanded");
        else
            $(SelectedmenuObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");
    }


    //child menu
    if (Tab.MasterTabID != "" && IsMenu == 'false') {
        var strMaster = Tab.MasterTabID;
        var menuMasterTab = strMaster.replace(/Tab/gi, "Menu");

        //"nav-active"

        var SelectedMenuMasterObj = $(document.getElementById(menuMasterTab));
        if ($(SelectedMenuMasterObj).hasClass("nav-parent active") == true)
            $(SelectedMenuMasterObj).removeClass("nav-parent active").addClass("nav-parent nav-expanded");
        else
            $(SelectedMenuMasterObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");

        if (typeof Tab.ParentChildTabID != "undefined") {
            var strParentChild = Tab.ParentChildTabID;
            var menuParentChildTab = strParentChild.replace(/Tab/gi, "Menu");
            CurrentParentmenu = menuParentChildTab;
            var SelectedParentChildObj = $(document.getElementById(menuParentChildTab));
            if ($(SelectedParentChildObj).hasClass("nav-parent active") == true)
                $(SelectedParentChildObj).removeClass("nav-parent active").addClass("nav-parent nav-expanded");
            else
                $(SelectedParentChildObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");
            //PreParent Active
            var parentParentModule = $('#' + menuParentChildTab).parents('li').attr('id');
            if (parentParentModule != undefined) {
                var SelectedMenuparentParentObjId = parentParentModule.replace("Menu", "Tab");
                if (SelectedMenuparentParentObjId != Tab.MasterTabID) {
                    var SelectedMenuparentParentObj = $(document.getElementById(parentParentModule));
                    if ($(SelectedMenuparentParentObj).hasClass("nav-parent active") == true)
                        $(SelectedMenuparentParentObj).removeClass("nav-parent active").addClass("nav-parent nav-expanded");
                    else
                        $(SelectedMenuparentParentObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");
                }
            }

            //.children('.rightclickarea:first')
            //.attr('id')
        }

        $(SelectedmenuObj).addClass("nav-active");
    }
    else if (Tab.ParentTabID != "" && IsMenu == 'true') {
        var SelectedParentMenuObj = $(document.getElementById(Tab.ParentTabID));
        if ($(SelectedParentMenuObj).hasClass("nav-parent active") == true)
            $(SelectedParentMenuObj).removeClass("nav-parent active").addClass("nav-parent nav-expanded");
        else
            $(SelectedParentMenuObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");

        $(SelectedmenuObj).addClass("nav-active");
    }

    if (Tab.MasterTabID == "") {
        if ($(SelectedtabObj).hasClass("active") == false)
            $(SelectedtabObj).addClass("active");

    }
    else if ($(SelectedtabObj).hasClass("btn btn-default btn-sm active") == false) {
        $(SelectedtabObj).parent().click();
        if (Tab.MasterTabID != "mstrTabSchedule") {
            //adnan maqbool, PMS-5193
            if (str.split('_')[0] != "EncounterVisit") {
                SelectedtabObj.closest("span").attr("class", "btn btn-default btn-sm tab_selected");
            }
            else {
                SelectedtabObj.removeClass('tab_space btn btn-default btn-sm nav-active').addClass('tab_space btn btn-default btn-sm active');
            }
            //
        }
        else {
            SelectedtabObj.removeClass("btn btn-default btn-sm").addClass("btn btn-default btn-sm active");
        }

        //       $(SelectedtabObj).removeClass("btn btn-default btn-sm").addClass("btn btn-default btn-sm tab_selected");
    }
    if (Tab["ParentChildTabID"] == "adminMenuFavorites") {


        if (Tab["TabID"] == 'adminTabFavoritesFamilyHistory' || Tab["TabID"] == 'adminTabFavoritesMedicalHistory' || Tab["TabID"] == 'adminTabFavoritesSurgicalHistory' || Tab["TabID"] == 'adminTabFavoritesHospitalizationHistory') {
            $('#adminMenuFavoritesHistory').addClass('nav-active');
        }
        else {
            $('#adminMenuFavoritesHistory').removeClass('nav-active');
        }
    }
}

function UnSelectedMenuAndTab(Tab, IsMenu) {

    var UnselectedtabObj = $(document.getElementById(Tab.TabID));
    var Unselectedstr = Tab.TabID;
    var UnselectedmenuTab = Unselectedstr.replace(/Tab/gi, "Menu");
    var UnselectedmenuObj = $(document.getElementById(UnselectedmenuTab));
    //  if (globalAppdata['IsAdmin'] != 'True') {
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
                //  else
                //  $(UnSelectedParentChildObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");
                if (Tab.ParentChildTabID == "mstrMenuBilling")
                    $(UnSelectedParentChildObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");

                if (Tab.ParentChildTabID == CurrentParentmenu) {
                    $(UnSelectedParentChildObj).removeClass("nav-parent").addClass("nav-parent nav-expanded");
                    //CurrentParentmenu = null;
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
        //if (Tab["TabID"] == 'adminTabFavoritesFamilyHistory' || Tab["TabID"] == 'adminTabFavoritesMedicalHistory' || Tab["TabID"] == 'adminTabFavoritesSurgicalHistory') {
        if (Tab.ContainerControlID == "Favorite_FamilyHistory" || Tab.ContainerControlID == "Favorite_MedicalHistory" || Tab.ContainerControlID == "Favorite_SurgicalHistory" || Tab.ContainerControlID == "Favorite_HospitalizationHistory") {
            $("#adminMenuClinical").removeClass("nav-parent").addClass("nav-parent nav-expanded");
            //$("#adminMenuFavoritesHistory").removeClass("nav-parent").addClass("nav-active");
        }
    }
    //}
    //else
    //    $(UnselectedmenuObj).removeClass("nav-parent nav-expanded").addClass("nav-parent active nav-expanded");

}

function GetTab(tabID) {
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].TabID == tabID) return TabsArray[i];
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

function showCurrentTab() {
    _tempCurrenttab.show('fade', 500);
    //if (!$(SelectedpnlObj).css('display') || $(SelectedpnlObj).css('display') == 'none') {
    //    $(SelectedpnlObj).show('fade', 200);
    //}
    //screenAdjustment();
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

                // var c = eval(Tab.ContainerControlID + '.bIsFirstLoad=true');
                var b = eval(Tab.ContainerControlID + '.Load')(param);
                store.save(Tab.ContainerControlID, true, patientID)
            }
            else {
                var b = eval(Tab.ContainerControlID + '.Load')(param);
            }
        }

        //}
        //}
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

function UnloadTabParams(MasterTabID) {
    var TabsPatientList = jQuery.grep(TabsArray, function (obj) {
        return obj.MasterTabID === MasterTabID;
    });
    $.each(TabsPatientList, function (i, item) {
        if (item.ContainerControlID != "" && item.ContainerControlID != null && typeof item.ContainerControlID != "undefined") {
            eval(item.ContainerControlID).bIsFirstLoad = true;
            eval(item.ContainerControlID).params = [];

        }
    });
    if (MasterTabID == "mstrTabPatient") {
        $('#ctrlPanPatient form').each(function () {
            if (typeof $(this).data('bootstrapValidator') != "undefined") {
                $(this).bootstrapValidator('destroy', true);
            }
        });
    }
    if (MasterTabID == "mstrTabClinical") {
        $('#ctrlPanClinical form').each(function () {
            if (typeof $(this).data('bootstrapValidator') != "undefined") {
                $(this).bootstrapValidator('destroy', true);
            }
        });
    }
}

function SetActiveTab(parentTabID) {
    var selectedTab = null;

    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].ParentTabID == parentTabID) {

            if (TabsArray[i]["Selected"] == true) {


                selectedTab = TabsArray[i];
                //Select Active Tab
                var SelectedtabObj = $(document.getElementById(selectedTab["TabID"]));
                //var SelectedContObj = $(document.getElementById(selectedTab["Container"]));
                var SelectedpnlObj //document.getElementById(selectedTab["PanelID"]);

                if (TabsArray[i]["PanelID"] != "" && TabsArray[i]["MasterTabID"] != "")
                    SelectedpnlObj = $('#' + TabsArray[i]["Container"] + ' #' + TabsArray[i]["PanelID"]);
                else
                    SelectedpnlObj = $(document.getElementById(TabsArray[i]["PanelID"]));

                SelectedpnlObj.css("display", "block");
                //if (!$(SelectedpnlObj).css('display') || $(SelectedpnlObj).css('display') == 'none') {
                //    $(SelectedpnlObj).show('fade', 100);
                //}
                //  SelectedpnlObj.css("display", "block");
                //if (!$(SelectedContObj).css('display') || $(SelectedContObj).css('display') == 'none') {
                //    $(SelectedContObj).css("display", "");
                //}

                SelectedMenuAndTab(selectedTab, 'false');



            }
            else {
                TabsArray[i]["Selected"] = false;
                TabsArray[i]["Active"] = false;

                var UnselectedtabObj = $(document.getElementById(TabsArray[i]["TabID"]));
                //var UnselectedContObj = $(document.getElementById(TabsArray[i]["Container"]));

                var UnselectedpnlObj

                if (TabsArray[i]["PanelID"] != "" && TabsArray[i]["MasterTabID"] != "")
                    UnselectedpnlObj = $('#' + TabsArray[i]["Container"] + ' #' + TabsArray[i]["PanelID"]);
                else
                    UnselectedpnlObj = $(document.getElementById(TabsArray[i]["PanelID"]));


                //$(UnselectedContObj).css("display", "none");

                UnselectedpnlObj.css("display", "none");
                UnSelectedMenuAndTab(TabsArray[i], 'false');


            }
        }
    }
}

// Application background Loader Function
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
        //jqXHR.success(function () {
        //    xhrPool = $.grep(xhrPool, function (x) { return x != jqXHR
        //    });
        //    BackgroundLoaderShow(false);
        //});
        //jqXHR.error(function () {
        //    xhrPool = $.grep(xhrPool, function (x) { return x != jqXHR });
        //    BackgroundLoaderShow(false);
        //});
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
    //var abort = function () {
    //    $.each(xhrPool, function (idx, jqXHR) {
    //        jqXHR.abort();
    //    });
    //    BackgroundLoaderShow(false);
    //};

    //var oldbeforeunload = window.onbeforeunload;
    //window.onbeforeunload = function () {
    //    var r = oldbeforeunload ? oldbeforeunload() : undefined;
    //    if (r == undefined) {
    //        // only cancel requests if there is no prompt to stay on the page
    //        // if there is a prompt, it will likely give the requests enough time to finish
    //        abort();
    //    }
    //    return r;
    //}
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

function OpenDashBoardSetting() {
    var params = [];
    //params["AssignedToId"] = globalAppdata['AppUserId'];
    //params["IsUserMessages"] = "1";
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
    //params["AssignedToId"] = globalAppdata['AppUserId'];
    //params["IsUserMessages"] = "1";
    var tab = GetCurrentSelectedTab();
    if (tab.TabID != 'mstrTabDashBoard') {
        params["ParentCtrl"] = tab.TabID;
    }
    else
        params["ParentCtrl"] = "mstrTabDashBoard";
    LoadActionPan('DashBoardChangePwd', params)


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

function CloseWindow() {
    window.close();
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

function globallyDateTime() {
    var d = new Date($('#userCurrentTime').text());

    if (d == "Invalid Date")
        d = new Date(Date($('#userCurrentTime').text()))
    d.setSeconds(d.getSeconds() + 1);
    var t = d.toLocaleTimeString();
    // var date = d.toDateString();

    var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"]

    var dayName = dayNames[d.getDay()]
    var monthName = monthNames[d.getMonth()]
    var dateOfMonth = d.getDate();
    var year = d.getFullYear()

    var date = dayName + ", " + monthName + " " + dateOfMonth + ", " + year;

    document.getElementById("userCurrentTime").innerHTML = date + " " + t;

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

