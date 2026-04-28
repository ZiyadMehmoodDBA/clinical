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
$(document).ready(function () {

    //Showing DateTimer

    //EMR-984 fix
    //$(document).idleTimer((parseInt(globalAppdata["SessionTimout"]) * 1000 * 60) - (30 * 1000));
    //$(document).on("idle.idleTimer", function (event, elem, obj) {
    //    // function you want to fire when the user goes idle
    //    if (!$('#ReLogin').hasClass('modal')) {
    //        var parameters = [];
    //        //params['ParentCntrlID'] = ParentCntrlID;
    //        parameters['UserName'] = globalAppdata['AppUserName']
    //        $('#ReLogin').addClass('modal fade')
    //        LoadActionPan('UserReLogin', parameters, 'ReLogin', true);//, GetCurrentSelectedTab().PanelID);
    //        ////idle Users, auto save Before sign out of the user, Dr Hijjar Requirement, Implemeneted By Azhar Shahzad on July 14, 2016
    //        if (params.NotesId != null && params.NotesId != "" && Number(params.NotesId) > 0 && $('#pnlClinicalProgressNote').is(':visible') && Number(globalAppdata["SessionTimout"]) <= 5) {
    //            Clinical_ProgressNote.updateProgressNoteHTML();
    //        }
    //    }
    //});
    //$(document).on("active.idleTimer", function (event, elem, obj, triggerevent) {
    //    // function you want to fire when the user becomes active again
    //});

    //if (localStorage.getItem('VersionNo') == null || localStorage.getItem('VersionNo') != globalAppdata['VersionNo'] || localStorage.getItem('PatchNo') == null || localStorage.getItem('PatchNo') != globalAppdata['PatchNo']) {
    //    store.clearAll();
    //    store.clearAllSession();
    //    localStorage.clear();
    //    localStorage.setItem('VersionNo', globalAppdata['VersionNo']);
    //    localStorage.setItem('PatchNo', globalAppdata['PatchNo']);
    //    html = null;
    //}

    //LoadApplication();

    //if (globalAppdata['DateFormat'])
    //    date_format = globalAppdata['DateFormat'];

    //setInterval(Patient_Message.RefreshCount, globalAppdata['RefreshTime']);
    ////showing date timer
    //setInterval(function () { globallyDateTime() }, 1000);
    ////setInterval(Patient_Message.RefreshCount, 2000);
    //$(document).on("keydown", function (e) {
    //    if (e.which === 8 && !$(e.target).is("input:not([readonly]):not([type=radio]):not([type=checkbox]), textarea, [contentEditable], [contentEditable=true]")) {
    //        e.preventDefault();
    //    }
    //});
    //$(document).mousedown(function (e) {
    //    switch (e.which) {

    //        case 2:
    //            e.preventDefault();
    //            break;

    //    }
    //    return true;
    //});

    ////$(window).on('beforeunload', function () {
    ////    return 'Are you sure you want to leave?';
    ////});

    ////$(window).on('unload', function () {
    ////    logout();
    ////});
    ///*Doc -33 Emergency work flow
    //     4.1.2	Login with emergency access user in application
    //    1.	A user having emergency access login in mdvision application.
    //    2.	The icon with username will be displayed in Red indicating the user will perform actions having emergency access role. This can been seen in given screen.
    //    3.	The Red username icon will be displayed in blinking form and upon hover the text “Emergency Access User” will be displayed.
    //    /removing blinking after 5 mins of user login
    //*/
    //setTimeout(function () {
    //    $('#mainForm #userbox #emergencyUserIconSpan').removeClass('animated infinite flash');
    //}, 60000 * 5);

}


);

//function InitMDVisionServerHub(user, entity, entityRegCode) {
//    ServerHub = $.connection.webEMRServerHub;
//    ServerHub.client.sendMessage = function (message) {
//        if (message.toLowerCase().indexOf("signoff_client") > -1) {
//            console.log(message);
//            //dataservice_patient.SesionLoggOff().done(function (response) {
//            //    $("#DefaultHeadingLiteral").html(globaldata["UserEntityName"] + "\\" + globaldata["AppUserName"]);
//            //    $("#DefaultPopUpExtender").show();
//            //});

//        }
//        else {
//            console.log(message);
//           // DashBoard.refreshPanels(message);
//        }
//    };
//    $.connection.hub.start().done(function () {
//        ServerHub.server.addClient($.connection.hub.id, user, entity, entityRegCode);
//    });
//}

function OpenViewReportDetail(FormName, QueryString) {
    var params = [];
    params["mode"] = "Edit";
    params["ParentCtrl"] = "mstrTabReports";
    params["FormName"] = FormName;
    params["QueryString"] = QueryString;
    params["reportpath"] = 'Controls/Reports/ReportViewer.aspx?reportpath=' + FormName;

    LoadActionPan('ReportsViewer_Detail', params);
}

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
function NotesPreview(NotesId, PatientId) {//, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, createdOn, IsPhoneEncounter) {
    //  Clinical_Notes.canSignNote().done(function (canSign) {
    var params = [];

    params["FromAdmin"] = "0";
    params["NotesId"] = NotesId;
    params["PatientId"] = PatientId;
    //if ($('#pnlNotes_Result #liSignedNotes').hasClass('active')) {
    //    params["RefSearch"] = "SignedSearch";
    //} else {
    //    params["RefSearch"] = "DraftSearch";
    //}

    //params["ProviderId"] = ProviderId;
    //params["VisitDate"] = VisitDate;
    //params["VisitDateWithoutTime"] = createdOn;
    params["ParentCtrl"] = "mstrTabReports";
    //params["ParentCtrlPanelID"] = Clinical_Notes.params.PanelID;
    //params["HasUnSignPermission"] = Clinical_Notes.params.HasUnSignPermission;

    //params["BillingInfoId"] = BillingInfoId;
    //params["AppointmentDate"] = AppointmentDate;
    //params["FacilityId"] = FacilityId;
    //params["RefProviderID"] = RefProviderID;
    //params["VisitId"] = VisitId;
    //params["NoteDate"] = NoteDate;
    //params["PatientTypeId"] = PatientTypeId;
    //params["POS"] = POS;
    //params["IsPhoneEncounter"] = IsPhoneEncounter;

    //   params["CanSign"] = canSign;
    LoadActionPan('Clinical_NotesView', params);
    // });
}

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
    TopButtonsHideShow();

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


        //reload dashboard
        //var html = utility.getTabHtml('mstrTabDashBoard');
        //if (html) {
        //    $("#ctrlPanDashBoard").html(html);
        //    eval('DashBoard.Load')(params);
        //}
        //else {
        //    $.get(GetTab("mstrTabDashBoard").Path, { cache: false }, function (content) {
        //        html = content;
        //        $("#ctrlPanDashBoard").html(html);
        //        eval('DashBoard.Load')(params);
        //    });
        //}

    }
    else {
        LoadHomeTab();
    }
    //////Batch_Fax.faxAccess().done(
    //////     function (resp) {

    //////         Batch_Fax.access = resp.status;
    //////         Batch_Fax.ShowFaxCount();
    //////     });

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
    var tabName = "mstrTabAuditbleEvents";
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

            if (PatientId != null && PatientId != "") {
                setPatientBanner(PatientId, true);
            }
            if (innerScreen.length > 0) {
                $.when(SelectTab(tabName, 'false')).then(function () {
                    SelectTab(innerScreen, 'false');
                });
            } else {
                SelectTab(tabName, "false");
            }

        }
    }, "html");

}

function clearUserSession() {

    //  store.clearAllSession();

    for (var i = 0; i < TabsArray.length; i++) {
        var key = TabsArray[i].TabID;
        store.clear(key);
    }



}



function LoadCommands() {

    var LoginAppUserId = globalAppdata['AppUserId'];
    var stored_AppUserId = store.fetch('LoginAppUserId');



    // BackgroundLoaderShow(true);
    // if (globalAppdata['AppUserName'] != DefaultUser) {
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
    // }
    //else {
    //    store.save('entityid', null);
    //    MDVisionService.reloadLookups = true;
    //}


    //    AppCommands.Load();
    //   var defferedarray = "{}";


    var defferedarray = $.map(TabsArray, function (tab, count) {
        if (tab.Path.length > 0) {
            if (tab.TabID == "schTabSearch")
                return LoadHTMLControl(tab);
        }

    });

    $.when.apply(null, defferedarray).then(function () {
        //$(document).loadDropDowns(false);
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

            //if (tab.Container == "ctrlPan_Home") {
            //    //LoadDashBoard();
            //    LoadControl(tab, null);
            //}
        }
        //else {
        //    $(html).loadDropDowns(false);
        //}
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
            //else { $(html).loadDropDowns(false); }
        }, "html");

        //if (tab.Container == "ctrlPan_Home") {
        //    ajax_get.complete(function () {
        //        //LoadDashBoard();
        //        LoadControl(GetTab("mstrTab_Home"), null);
        //    });

        //}
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
    if (tabId == "mstrTabSchedule" || tabId == "schTabCalendar") {
        PMSScheduler.ReloadScheduler();
        $('.content-body').addClass('pb-none');
        $('.content-body').addClass('pr-none');
    }
    else {
        $('.content-body').removeClass('pb-none');
        $('.content-body').removeClass('pr-none');
    }
    if (tabId == "mstrTabPatient" || tabId == "patTabDemographic" || $('#mstrTabPatient').attr('class') == "active") {
        $('#btnDemographicLabel').show();
    }
    else {
        $('#btnDemographicLabel').hide();
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
        //EncounterChargeCapture.params["VisitId"] = UniqueId;
        //var CurrentTab = GetTab(tabId);
        //if (CurrentTab!=null) {
        //    EncounterChargeCapture.params["PanelID"] = CurrentTab.PanelID;
        //    var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
        //    var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
        //    ////$(ChargeGridId).dataTable().fnDestroy();
        //    //$(ChargeGridId + " tbody tr").remove();
        //    EncounterChargeCapture.EditableGrid = utility.MakeEditableGrid(PanelChargeGrid, ChargeGridId, EncounterChargeCapture, "0", false, false, false, false);
        //}

        params['VisitId'] = UniqueId;
    }
    if (!(tabId.trim() == ("clinicaltab" + (tabId.replace(/clinicaltab/gi, "").trim())).trim() || tabId == "clinicalTab" + tabId.replace(/clinicalTab/gi, ""))) {
        deleteNotesGlobalParams();
    }

    //if (tabId == "adminTabUser" || tabId == "adminTabSecurityRoles" || tabId == "adminTabPrivilegesGroup" || tabId == "adminTabPractice" || tabId == "adminTabFacility" || tabId == "adminTabProvider" || tabId == "adminTabResources" || tabId == "adminTabSpecialty") {
    if (tabId == "adminTab" + tabId.replace(/adminTab/gi, "")) {
        LoadAdminTab(tabId, IsMenu);
        return;
    }
    else if (tabId == "batchTab" + tabId.replace(/batchTab/gi, "")) {
        LoadBatchTab(tabId, IsMenu);
        return;
    }
    else if (tabId == "EncounterTab" + tabId.replace(/EncounterTab/gi, "")) {
        LoadEncounterTab(tabId, IsMenu, UniqueId, TabTitle);
        return;
    }
    else if (tabId.trim() == ("clinicaltab" + (tabId.replace(/clinicaltab/gi, "").trim())).trim() || tabId == "clinicalTab" + tabId.replace(/clinicalTab/gi, "")) {
        /*
            Author Change: Muhammad Azhar Shahzad
            Change Date: April 04,2016
            Purpose: For Clinical Tab, patient should be selected for furthur operations
        */
        if (params["patientID"] == null || params["patientID"] == "" || params["patientID"] == "-1") {
            LoadActionPan('Patient_Search', null);
            if ($('#PatientProfile').css('display') != 'none') {
                $('#PatientProfile').css('display', 'none');
            }
        } else {
            LoadClinicalTab(tabId, IsMenu);
        }

        return;
    }



    else if (tabId.trim() == ("notetab" + (tabId.replace(/notetab/gi, "").trim())).trim() || tabId == "noteTab" + tabId.replace(/noteTab/gi, "")) {
        /*
            Author Change: Muhammad Azhar Shahzad
            Change Date: April 04,2016
            Purpose: For Clinical Tab, patient should be selected for furthur operations
        */
        if (params["patientID"] == null || params["patientID"] == "" || params["patientID"] == "-1") {
            LoadActionPan('Patient_Search', null);
            if ($('#PatientProfile').css('display') != 'none') {
                $('#PatientProfile').css('display', 'none');
            }
        } else {
            LoadNoteTab(tabId, IsMenu);
        }

        return;
    }

    else if (tabId == "billTab" + tabId.replace(/billTab/gi, "")) {
        var isOOOVisitClicked = false;
        if (tabId == 'billTabOutOfOfficeVisits') {
            if (params["patientID"] == null || params["patientID"] == "" || params["patientID"] == "-1") {
                isOOOVisitClicked = true;
                LoadActionPan('Patient_Search', null);
                if ($('#PatientProfile').css('display') != 'none') {
                    $('#PatientProfile').css('display', 'none');
                }
            } else {
                isOOOVisitClicked = true;
                LoadBillingTab(tabId, IsMenu);
            }
        }

        if (!isOOOVisitClicked) {
            LoadBillingTab(tabId, IsMenu);
        }
        return;
    }
    else if (tabId == "auditbleEventsTab" + tabId.replace(/auditbleEventsTab/gi, "")) {
        LoadAuditbleEventsTab(tabId, IsMenu);

        return;
    }
    else if (tabId == "patTab" + tabId.replace(/patTab/gi, "")) {
        LoadPatientTab(tabId, IsMenu, true);
        return;
    }
    else
        SelectCurrentTab(tabId, IsMenu, Reload);

}
function LoadPatientTab(tabId, IsMenu, isLoad) {
    var menuTab = tabId.replace(/Tab/gi, "Menu");
    var SelectedmenuObj = $(document.getElementById(menuTab));
    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    var ElementName = tabId.replace(/patTab/gi, "");
    if (isLoad)
        AddPatientTab(ElementName, IsMenu);

    var nodes = document.getElementById("mstrDivPatient").getElementsByTagName('*');
    var varifyNode = document.getElementById(tabId);
    if (varifyNode == null) {
        if (nodes.length == 0) {
            var Element = $('<span class="btn btn-default btn-sm tab_selected tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></span>');
        }
        else {
            $(nodes).each(function () {
                $(this).removeClass('active');
                $(this).removeClass('tab_selected');
            });
            var Element = $('<span class="btn btn-default btn-sm tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></span>');
            //Add Commands to Array....
        }
        //Append Element
        $("#" + "mstrDivPatient").append(Element);
        //Append Element

        //if (patientTabs == null) {
        //    $("#" + "mstrDivPatient").append(Element);
        //    patientTabs = $("#" + "mstrDivPatient").scrollTabs();
        //}
        //else {
        //    patientTabs.addTab($(Element)[0].outerHTML, $("#mstrDivPatient .btn").length);
        //}
        //$("#mstrDivPatient .btn").removeClass('tab_selected');
        //$($("#mstrDivPatient .btn")[$("#mstrDivPatient .btn").length - 1]).addClass('tab_selected');
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
function LoadAdminTab(tabId, IsMenu) {
    var menuTab = tabId.replace(/Tab/gi, "Menu");
    var SelectedmenuObj = $(document.getElementById(menuTab));
    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    var ElementName = tabId.replace(/adminTab/gi, "");

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

function LoadBillingTab(tabId, IsMenu) {
    var menuTab = tabId.replace(/Tab/gi, "Menu");
    var SelectedmenuObj = $(document.getElementById(menuTab));
    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    var ElementName = tabId.replace(/billTab/gi, "");

    AddBillingTab(ElementName, IsMenu);
    //var Tab = GetTab(tabId);
    var nodes = document.getElementById("mstrDivBilling").getElementsByTagName('*');
    var varifyNode = document.getElementById(tabId);
    var Element = "";
    if (varifyNode == null) {
        if (menuTitle) {
            if (nodes.length == 0) {
                var Element = $('<span class="btn btn-default btn-sm tab_selected tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></span>');
            }
            else {
                var Element = $('<span class="btn btn-default btn-sm tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></span>');
                //Add Commands to Array....
            }
        }
        //Append Element
        if (billingTabs == null) {
            $("#" + "mstrDivBilling").append(Element);
            billingTabs = $("#" + "mstrDivBilling").scrollTabs();
        }
        else {
            billingTabs.addTab($(Element)[0].outerHTML, $("#mstrDivBilling .btn").length);
        }
        $("#mstrDivBilling .btn").removeClass('tab_selected');
        $($("#mstrDivBilling .btn")[$("#mstrDivBilling .btn").length - 1]).addClass('tab_selected');

        //$("#" + "mstrDivBilling").append(Element);
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

function LoadClinicalTab(tabId, IsMenu) {
    var menuTab = tabId.replace(/Tab/gi, "Menu").trim();
    var menuTab_ = menuTab.replace("clinicalMenu", "");
    menuTab_ = menuTab_.trim();
    menuTab_ = menuTab_.charAt(0).toUpperCase() + menuTab_.slice(1);
    menuTab_ == "Question_group" ? menuTab_ = "Question_Group" : menuTab_ = menuTab_;
    var SelectedmenuObj = $(document.getElementById("clinicalMenu" + menuTab_));

    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    if (SelectedmenuObj.length == 0 && menuTitle == null) {
        menuTitle = menuTab_;
    }
    var ElementName = tabId.replace(/clinicalTab/gi, "");
    ElementName = ElementName.trim();
    ElementName = ElementName.charAt(0).toUpperCase() + ElementName.slice(1);
    ElementName == "Question_group" ? ElementName = "Question_Group" : ElementName = ElementName;
    var tab_Id = "clinicalTab" + ElementName;

    AddClinicalTab(ElementName, IsMenu);

}

function LoadNoteTab(tabId, IsMenu) {
    var menuTab = tabId.replace(/Tab/gi, "Menu").trim();
    var menuTab_ = menuTab.replace("clinicalMenu", "");
    menuTab_ = menuTab_.trim();
    menuTab_ = menuTab_.charAt(0).toUpperCase() + menuTab_.slice(1);
    menuTab_ == "Question_group" ? menuTab_ = "Question_Group" : menuTab_ = menuTab_;
    var SelectedmenuObj = $(document.getElementById("clinicalMenu" + menuTab_));

    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    if (SelectedmenuObj.length == 0 && menuTitle == null) {
        menuTitle = menuTab_;
    }
    var ElementName = tabId.replace(/noteTab/gi, "");
    ElementName = ElementName.trim();
    ElementName = ElementName.charAt(0).toUpperCase() + ElementName.slice(1);
    ElementName == "Question_group" ? ElementName = "Question_Group" : ElementName = ElementName;
    var tab_Id = "noteTab" + ElementName;

    AddNoteTab(ElementName, IsMenu);

}

function LoadAuditbleEventsTab(tabId, IsMenu) {
    var menuTab = tabId.replace(/Tab/gi, "Menu");
    var SelectedmenuObj = $(document.getElementById(menuTab));
    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    var ElementName = tabId.replace(/AuditbleEventsTab/gi, "");
    //var menuTab = tabId;
    //var SelectedmenuObj = $(document.getElementById(menuTab));
    //var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    //var ElementName = tabId.replace(/AuditbleEvents/gi, "");

    AddAuditbleEventsTab(ElementName, IsMenu);
    //var Tab = GetTab(tabId);
    var nodes = document.getElementById("mstrDivAuditbleEvents").getElementsByTagName('*');
    var varifyNode = document.getElementById(tabId);
    if (varifyNode == null) {
        if (nodes.length == 0) {
            var Element = $('<span class="btn btn-default btn-sm tab_selected tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></span>');
        }
        else {
            var Element = $('<span class="btn btn-default btn-sm tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></span>');
            //Add Commands to Array....
        }
        //Append Element
        $("#" + "mstrDivAuditbleEvents").append(Element);
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

//#endregion Clinical Related Code
function LoadBatchTab(tabId, IsMenu) {
    var menuTab = tabId.replace(/Tab/gi, "Menu");
    var SelectedmenuObj = $(document.getElementById(menuTab));
    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    var ElementName = tabId.replace(/batchTab/gi, "");

    AddBatchTab(ElementName, IsMenu);
    //var Tab = GetTab(tabId);
    var nodes = document.getElementById("mstrDivBatch").getElementsByTagName('*');
    var varifyNode = document.getElementById(tabId);
    if (varifyNode == null) {
        if (nodes.length == 0) {
            var Element = $('<span class="btn btn-default btn-sm tab_selected tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></span>');
        }
        else {
            var Element = $('<span class="btn btn-default btn-sm tab_space"><button type="button" title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false");>' + menuTitle + '</button></span>');
            //Add Commands to Array....
        }
        //Append Element
        $("#" + "mstrDivBatch").append(Element);
    }
}

function LoadEncounterTab(tabId, IsMenu, UniqueId, title) {
    var menuTab = tabId.replace(/Tab/gi, "Menu");
    var SelectedmenuObj = $(document.getElementById(menuTab));
    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    if (typeof menuTitle == "undefined") {
        menuTitle = title;
    }
    var ElementName = tabId.replace(/EncounterTab/gi, "")//.replace(UniqueId, "");
    if (ElementName.indexOf(UniqueId) > -1) {
        ElementName = ElementName.replace(UniqueId, "");
    }
    tabId = "Encounter" + ElementName + UniqueId;
    var cmd = AddEncounterTab(ElementName, IsMenu, UniqueId);
    tabId = cmd.TabID;
    //var Tab = GetTab(tabId);
    var nodes = document.getElementById("mstrDivPatient").getElementsByTagName('*');
    var varifyNode = document.getElementById(tabId);
    if (varifyNode == null) {
        if (nodes.length == 0) {
            //adnan maqbool, PMS-5141
            var Element = $('<span"  class="btn btn-default btn-sm tab_selected tab_space" id="' + tabId + '"><button type="button" title="' + menuTitle + '" onclick=SelectTab("' + tabId + '","false","true","' + UniqueId + '","' + title + '");$("#ctrlPanPatient").show();>' + menuTitle + '</button><a title="Close Visit" onclick="EncounterVisit_Detail.CloseVisitTab(this);"><i class="fa fa-close"></i></a></span>');
        }
        else {
            var Element = $('<span class="btn btn-default btn-sm tab_selected tab_space" id="' + tabId + '"><button title="' + menuTitle + '" type="button" onclick=SelectTab("' + tabId + '","false","true","' + UniqueId + '","' + title + '");$("#ctrlPanPatient").show();>' + menuTitle + '</button><a title="Close Visit" onclick="EncounterVisit_Detail.CloseVisitTab(this);" ><i class="fa fa-close"></i></a></span>');
            //Add Commands to Array....
        }


        /************/
        //if (nodes.length == 0) {
        //    var Element = $('<span"  class="btn btn-default btn-sm active tab_space"> <button title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false","true","' + UniqueId + '","' + title + '");$("#ctrlPanPatient").show();>' + menuTitle + '</button><a  onclick="EncounterVisit_Detail.CloseVisitTab(this);"><i class="fa fa-times"></i></a></span>');
        //}
        //else {
        //    var Element = $('<span class="btn btn-default btn-sm tab_space" ><button title="' + menuTitle + '" id="' + tabId + '" onclick=SelectTab("' + tabId + '","false","true","' + UniqueId + '","' + title + '");$("#ctrlPanPatient").show();>' + menuTitle + '</button><a onclick="EncounterVisit_Detail.CloseVisitTab(this);" ><i class="fa fa-times"></i></a></span>');
        //    //Add Commands to Array....
        //}
        /************/

        //Append Element
        //$("#" + "mstrDivPatient div:first-child").append(Element);
        $("#" + "mstrDivPatient").append(Element);

        //if (patientTabs == null) {
        //    $("#" + "mstrDivPatient").append(Element);
        //    patientTabs = $("#" + "mstrDivPatient").scrollTabs();
        //}
        //else {
        //    patientTabs.addTab($(Element)[0].outerHTML, $("#mstrDivPatient .btn").length);
        //}
        //$("#mstrDivPatient .btn").removeClass('tab_selected');
        //$($("#mstrDivPatient .btn")[$("#mstrDivPatient .btn").length - 1]).addClass('tab_selected');
    }
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
    else if (ElementName == "FavoritesFamilyHistory" || ElementName == "FavoritesMedicalHistory" || ElementName == "FavoritesSurgicalHistory" || ElementName == "FavoritesComplaint" || ElementName == "FavoritesTherapueticInjection" || ElementName == "FavoritesVaccine" || ElementName == "FavoritesProcedureOrder" || ElementName == "FavoritesConsultationOrder" || ElementName == "FavoritesCustomForms" || ElementName == "FavoritesRadiologyOrder" || ElementName == "FavoritesLabOrder" || ElementName == "FavoritesHistory" || ElementName == "FavoritesProblems") {
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

function AddClinicalTab(ElementName, IsMenu) {
    var cmd = [];

    ElementName = ElementName.trim();
    ElementName = ElementName.charAt(0).toUpperCase() + ElementName.slice(1);
    ElementName == "Question_group" ? ElementName = "Question_Group" : ElementName = ElementName;
    if (ElementName == "Problems") {
        ElementName = "ProblemLists";
    }
    cmd.TabID = "clinicalTab" + ElementName;
    cmd.PanelID = "pnlClinical" + ElementName;//panal id from opening html control
    cmd.MasterTabID = "mstrTabClinical";
    cmd.ParentTabID = "mstrTabClinical";
    /*
    Author: Muhammad Azhar Shahzad
    Date: Jan 07, 2016
    Purpose: Removing these parameters from globar params, if selected Tab is Not Progress Note and notes
    these params are getting set, when we try to creat note or edit note from schedular*/
    if (!(ElementName == "Notes" || ElementName == "ProgressNote")) {
        deleteNotesGlobalParams();
    }
    if (ElementName == "Template" || ElementName == "Section" || ElementName == "Question" || ElementName == "Question_Group") {
        cmd.ParentChildTabID = "clinicalMenuTemplateBuilder";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/TemplateBuilder/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "Letter") {
        cmd.ParentChildTabID = "clinicalMenuDesignLetter";

        cmd.ContainerControlID = "Design_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./Controls/Clinical/LetterDesign/Design_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }

    else if (ElementName == "Notes") {
        cmd.ParentChildTabID = "clinicalMenuNotes";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }

    else if (ElementName == "Implantable") {
        //cmd.ParentChildTabID = "clinicalMenuNotes";
        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }

    else if (ElementName == "PhoneEncounter") {
        cmd.ParentChildTabID = "clinicalMenuPhoneEncounter";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "ProgressNote") {
        cmd.ParentChildTabID = "clinicalMenuNotes";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
        //Start 16/03/2016 Muhammad Arshad Patient Education related changes
    else if (ElementName == "Vitals" || ElementName == "Allergies" || ElementName == "ProblemLists" || ElementName == "Medications" || ElementName == "Procedures" || ElementName == "PatientEducation" || ElementName == "Immunization" || ElementName == "CDSAlerts" || ElementName == "CarePlan" || ElementName == "Cognitive") {
        //End 16/03/2016 Muhammad Arshad Patient Education related changes
        cmd.ParentChildTabID = "clinicalMenuMedical";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal

    }
    else if (ElementName == "SocialHx" || ElementName == "HistorySummary" || ElementName == "BirthHx" || ElementName == "MedicalHx" || ElementName == "FamilyHx" || ElementName == "SurgicalHx" || ElementName == "HospitalizationHx" || ElementName == "SocPsyandBehaviorHx") {
        cmd.ParentChildTabID = "clinicalMenuHistroy";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/History/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "FaceSheet") {
        cmd.ParentChildTabID = "clinicalMenuFaceSheet";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/FaceSheet/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
        //Start 16/03/2016 Muhammad Arshad RadiolgyOrder related changes
    else if (ElementName == "RadiologyOrder" || ElementName == "ProcedureOrder" || ElementName == "ConsultationOrder" || ElementName == "CustomForms" || ElementName == "LabOrder") {
        cmd.ParentChildTabID = "clinicalMenuOrders";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    //End 16/03/2016 Muhammad Arshad RadiolgyOrder related changes
    var Tab = GetTab(cmd.TabID);
    var ClinicalTab = cmd;

    if (typeof Tab != "undefined") {
        SelectCurrentTab(ClinicalTab.TabID, IsMenu);
    }
    else {
        AddAndSelectTab(ClinicalTab, IsMenu);
    }
}

function deleteNotesGlobalParams() {
    delete params["PatientId"];;
    delete params["ParentCtrl"];
}

function AddNoteTab(ElementName, IsMenu) {
    var cmd = [];

    ElementName = ElementName.trim();
    ElementName = ElementName.charAt(0).toUpperCase() + ElementName.slice(1);
    ElementName == "Question_group" ? ElementName = "Question_Group" : ElementName = ElementName;
    cmd.TabID = "noteTab" + ElementName;
    cmd.PanelID = "pnlClinical" + ElementName;//panal id from opening html control
    cmd.MasterTabID = "mstrTabNotes";
    cmd.ParentTabID = "mstrTabNotes";
    if (!(ElementName == "Notes" || ElementName == "ProgressNote")) {
        delete params.NotesRoom;
        delete params.NotesFacilityId;
        delete params.NotesFacilityName;
        delete params.NotesId;
        delete params.NotesProviderId;
        delete params.NotesProviderName;
        delete params.NotesVisitDate;
        delete params.NotesVisitId;
        delete params.NotesVisitTime;
    }
    if (ElementName == "Template" || ElementName == "Section" || ElementName == "Question" || ElementName == "Question_Group") {
        cmd.ParentChildTabID = "clinicalMenuTemplateBuilder";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanNote";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/TemplateBuilder/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "Letter") {
        cmd.ParentChildTabID = "clinicalMenuDesignLetter";

        cmd.ContainerControlID = "Design_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./Controls/Clinical/LetterDesign/Design_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }

    else if (ElementName == "Notes") {
        cmd.ParentChildTabID = "clinicalMenuNotes";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanNote";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }

    else if (ElementName == "PhoneEncounter") {
        cmd.ParentChildTabID = "clinicalMenuPhoneEncounter";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "ProgressNote") {
        cmd.ParentChildTabID = "clinicalMenuNotes";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanNote";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Notes/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
        //Start 16/03/2016 Muhammad Arshad Patient Education related changes
    else if (ElementName == "Vitals" || ElementName == "Allergies" || ElementName == "ProblemLists" || ElementName == "Medications" || ElementName == "Procedures" || ElementName == "PatientEducation" || ElementName == "Immunization" || ElementName == "CarePlan" || ElementName == "Cognitive") {
        //End 16/03/2016 Muhammad Arshad Patient Education related changes
        cmd.ParentChildTabID = "clinicalMenuMedical";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanClinical";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Medical/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal

    }
    else if (ElementName == "SocialHx" || ElementName == "HistorySummary" || ElementName == "BirthHx" || ElementName == "MedicalHx" || ElementName == "FamilyHx" || ElementName == "SurgicalHx" || ElementName == "HospitalizationHx" || ElementName == "SocPsyandBehaviorHx") {
        cmd.ParentChildTabID = "clinicalMenuHistroy";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanNote";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/History/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    else if (ElementName == "FaceSheet") {
        cmd.ParentChildTabID = "clinicalMenuFaceSheet";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanNote";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/FaceSheet/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
        //Start 16/03/2016 Muhammad Arshad RadiolgyOrder related changes
    else if (ElementName == "RadiologyOrder" || ElementName == "ProcedureOrder" || ElementName == "ConsultationOrder" || ElementName == "CustomForms" || ElementName == "LabOrder") {
        cmd.ParentChildTabID = "clinicalMenuOrders";

        cmd.ContainerControlID = "Clinical_" + ElementName;
        cmd.Selected = true;
        cmd.Container = "ctrlPanNote";// in which container it will be open
        cmd.Path = "./EMR/HTML/Clinical/Orders/Clinical_" + ElementName + ".html";
        cmd.ActionPanContainer = "actionPanClinical" + ElementName;//action pan of opening panal
    }
    //End 16/03/2016 Muhammad Arshad RadiolgyOrder related changes

    var Tab = GetTab(cmd.TabID);
    var ClinicalTab = cmd;

    if (typeof Tab != "undefined") {
        SelectCurrentTab(ClinicalTab.TabID, IsMenu);
    }
    else {
        AddAndSelectTab(ClinicalTab, IsMenu);
    }
}

function AddBillingTab(ElementName, IsMenu) {
    var cmd = [];
    cmd.TabID = "billTab" + ElementName;
    cmd.PanelID = "pnlBill" + ElementName;//panal id from opening html control
    cmd.MasterTabID = "mstrTabBilling";
    cmd.ParentTabID = "mstrTabBilling";
    cmd.ParentChildTabID = "mstrMenuBilling";
    cmd.ContainerControlID = "Bill_" + ElementName;
    cmd.Selected = true;
    cmd.Container = "ctrlPanBilling";// in which container it will be open
    cmd.ActionPanContainer = "actionPanBill" + ElementName;//action pan of opening panal

    var searchcharge = ElementName.search('Charge');
    var searclaim = ElementName.search('Claim');
    var searpayment = ElementName.search('Payment');
    var searchera = ElementName.search('ERA');
    var searchPatientAR = ElementName.search('PatientAR');
    var searchInsuranceAR = ElementName.search('InsuranceAR');
    var searchPatientStatement = ElementName.search('PatientStatement');
    var searchOutOfOfficeVisits = ElementName.search('OutOfOfficeVisits');
    var copayreceipt = ElementName.search('CopayReceipt');

    if (searchcharge != -1)
        cmd.Path = "./Controls/Billing/Charges/Bill_" + ElementName + ".html";
    else if (searclaim != -1)
        cmd.Path = "./Controls/Billing/Claims/Bill_" + ElementName + ".html";
    else if (searpayment != -1)
        cmd.Path = "./Controls/Billing/Payments/Bill_" + ElementName + ".html";
    else if (searchera != -1)
        cmd.Path = "./Controls/Billing/ERA/Bill_" + ElementName + ".html";
    else if (searchPatientAR != -1)
        cmd.Path = "./Controls/Billing/FollowUp/Bill_" + ElementName + ".html";
    else if (searchInsuranceAR != -1)
        cmd.Path = "./Controls/Billing/FollowUp/Bill_" + ElementName + ".html";
    else if (searchPatientStatement != -1)
        cmd.Path = "./Controls/Billing/PatientStatement/Bill_" + ElementName + ".html";

        //For Out Of Office Visits
    else if (searchOutOfOfficeVisits != -1) {
        cmd.Path = "./EMR/HTML/Clinical/BillingInformation/" + ElementName + ".html";
        cmd.ContainerControlID = ElementName;
    }
    else if (copayreceipt != -1) {
        cmd.PanelID = "pnlScheduling_UnallocatedCopayment_Search";
        cmd.ContainerControlID = "Scheduling_UnallocatedCopayment_Search";
        cmd.Path = "./Controls/Scheduling/Scheduling_UnallocatedCopayment_Search.html";
        cmd.ActionPanContainer = "actionPanScheduling_UnallocatedCopayment_Search";
    }


    if (ElementName == "EDIReport") {
        cmd.Path = "./Controls/Billing/Charges/Bill_" + ElementName + ".html";
    }
    var Tab = GetTab(cmd.TabID);
    var billTab = cmd;

    if (typeof Tab != "undefined") {
        SelectCurrentTab(billTab.TabID, IsMenu);
    }
    else {
        AddAndSelectTab(billTab, IsMenu);

        //LoadHTMLControl(AdminTab);
        //for (var i = 0; i < TabsArray.length; i++) {
        //    if (isAdminTab(TabsArray[i])) {
        //        if (TabsArray[i].Path.length > 0)
        //            LoadHTMLControl(TabsArray[i]);
        //    }
        //}
    }
}

function AddPatientTab(ElementName, IsMenu) {
    var cmd = [];


    cmd.MasterTabID = "mstrTabPatient";
    cmd.ParentTabID = "mstrTabPatient";
    cmd.Selected = true;
    cmd.Container = "ctrlPanPatient";// in which container it will be open

    if (ElementName == "Demographic") {
        cmd.TabID = "patTab" + ElementName;
        cmd.PanelID = "pnl" + ElementName;//panel id from opening html control
        cmd.ActionPanContainer = "actionPan" + ElementName;//action pan of opening panel
        cmd.ContainerControlID = "Patient_Demographic";
        cmd.Path = "./Controls/Patient/Demographics/Patient_Demographic.html";
    }
    else if (ElementName == "Encounter") {
        cmd.TabID = "patTabEncounter";
        cmd.PanelID = "pnlEncounter";
        cmd.ContainerControlID = "Encounter_Visits";
        cmd.Path = "./Controls/Patient/Encounter/Encounter_Visits.html";
        cmd.ActionPanContainer = "actionPanEncounter";
    }
    else if (ElementName == "Insurance") {
        cmd.TabID = "patTabInsurance";
        cmd.PanelID = "pnlPatientInsurance";
        cmd.Path = "./Controls/Patient/Insurance/Patient_Insurance.html";
        cmd.ActionPanContainer = "actionPanPatientInsurance";
        cmd.ContainerControlID = "Patient_Insurance";
    }
    else if (ElementName == "Documents") {
        cmd.TabID = "patTabDocuments";
        cmd.PanelID = "pnlPatientDocument";
        cmd.ContainerControlID = "Patient_Document";
        cmd.Path = "./Controls/Patient/Document/Patient_Document.html";
        cmd.ActionPanContainer = "actionPanPatientDocument";
    }
    else if (ElementName == "CaseManagement") {
        cmd.TabID = "patTabCaseManagement";
        cmd.PanelID = "pnlPatientCase";
        cmd.ContainerControlID = "Patient_Case";
        cmd.Path = "./Controls/Patient/Case/Patient_Case.html";
        cmd.ActionPanContainer = "actionPanPatientCase";
    }
    else if (ElementName == "Messages") {
        cmd.TabID = "patTabMessages";
        cmd.PanelID = "pnlPatientMessage";
        cmd.ContainerControlID = "Patient_Message";
        cmd.Path = "./Controls/Patient/Messages/Patient_Message.html";
        cmd.ActionPanContainer = "actionPanPatientMessage";

    }
    else if (ElementName == "ActivityLog") {
        cmd.TabID = "patTabActivityLog";
        cmd.PanelID = "pnlActivityLog";
        cmd.ContainerControlID = "Activity_Log";
        cmd.Path = "./Controls/CommonControls/Activity_Log.html";
        cmd.ActionPanContainer = "actionPanActivityLog";
    }
    else if (ElementName == "Letter") {
        cmd.TabID = "patTab" + ElementName;
        cmd.PanelID = "pnl" + ElementName;
        cmd.ActionPanContainer = "actionPan" + ElementName;
        cmd.ContainerControlID = "Patient_Letter";
        cmd.Path = "./EMR/HTML/Patient/Patient_Letter.html";
    }
    else if (ElementName == "UserMessages") {
        cmd.TabID = "patTabUserMessages";
        cmd.PanelID = "pnlPatientUserMessages";
        cmd.ActionPanContainer = "actionPanPatientUserMessages";
        cmd.ContainerControlID = "Patient_UserMessages";
        cmd.Path = "./Controls/Patient/Messages/Patient_UserMessages.html";
    }
    else if (ElementName == "Referrals") {
        cmd.TabID = "patTabReferrals";
        cmd.PanelID = "pnlPatientReferrals";
        cmd.ActionPanContainer = "actionPanPatientReferrals";
        cmd.ContainerControlID = "Patient_Referrals";
        cmd.Path = "./EMR/HTML/Patient/Patient_Referrals.html";
    }
    else if (ElementName == "CustomForm") {
        cmd.TabID = "patTab" + ElementName;
        cmd.PanelID = "pnlPatientCustomForm";
        cmd.ActionPanContainer = "actionPan" + ElementName;
        cmd.ContainerControlID = "Patient_CustomForm";
        cmd.Path = "./EMR/HTML/Patient/Patient_CustomForm.html";
    }

    var Tab = GetTab(cmd.TabID);
    var PatientTab = cmd;

    if (typeof Tab != "undefined") {
        SelectCurrentTab(PatientTab.TabID, IsMenu);
    }
    else {
        AddAndSelectTab(PatientTab, IsMenu);
    }
}

function AddBatchTab(ElementName, IsMenu) {
    var cmd = [];
    cmd.TabID = "batchTab" + ElementName;

    cmd.MasterTabID = "mstrTabBatch";
    cmd.ParentTabID = "mstrTabBatch";
    cmd.Selected = true;
    cmd.Container = "ctrlPanBatch";// in which container it will be open
    cmd.PanelID = "pnlBatch" + ElementName;//panel id from opening html control
    cmd.ActionPanContainer = "actionPanBatch" + ElementName;//action pan of opening panel

    if (ElementName == "Documents") {
        cmd.ContainerControlID = "Patient_Document";
        cmd.Path = "./Controls/Patient/Document/Patient_Document.html";
    }
    else if (ElementName == "Messages") {
        cmd.ContainerControlID = "Patient_Message";
        cmd.Path = "./Controls/Patient/Messages/Patient_Message.html";
    }
    else if (ElementName == "PatientEligibility") {
        cmd.ContainerControlID = "Patient_Eligibility";
        cmd.Path = "./Controls/Patient/Insurance/Patient_Eligibility.html";
    }
    else if (ElementName == "ClinicalQualityMeasure") {
        cmd.ContainerControlID = "Batch_" + ElementName;
        cmd.Path = "./Controls/Batch/Batch_" + ElementName + ".html";
    }
    else if (ElementName == "PatientList") {
        cmd.ContainerControlID = "Batch_" + ElementName;
        cmd.Path = "./EMR/HTML/Batch/PatientList/Batch_" + ElementName + ".html";
    }
    else if (ElementName == "ImportHL7LabResults") {
        cmd.ContainerControlID = "Batch_" + ElementName;
        cmd.Path = "./EMR/HTML/Batch/HL7LabResults/Batch_" + ElementName + ".html";
    }
    else if (ElementName == "ImportHL7ImmunizationBatch") {
        cmd.ContainerControlID = "Batch_" + ElementName;
        cmd.Path = "./Controls/Batch/Batch_" + ElementName + ".html";
    }
    else {
        cmd.ContainerControlID = "Batch_" + ElementName;
        cmd.Path = "./Controls/Batch/Batch_" + ElementName + ".html";
    }
    var Tab = GetTab(cmd.TabID);
    var BatchTab = cmd;

    if (typeof Tab != "undefined") {
        SelectCurrentTab(BatchTab.TabID, IsMenu);
    }
    else {
        AddAndSelectTab(BatchTab, IsMenu);
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
function AddAuditbleEventsTab(ElementName, IsMenu) {
    var cmd = [];
    cmd.TabID = "auditbleEventsTab" + ElementName;
    cmd.PanelID = "pnlAuditbleEvents" + ElementName;//panal id from opening html control
    cmd.MasterTabID = "mstrTabAuditbleEvents";
    cmd.ParentTabID = "mstrTabAuditbleEvents";
    cmd.ParentChildTabID = "mstrMenuAuditbleEvents";
    cmd.ContainerControlID = "AuditbleEvents_" + ElementName;
    cmd.Selected = true;
    cmd.Container = "ctrlPanAuditbleEvents";// in which container it will be open
    cmd.ActionPanContainer = "actionPanAuditbleEvents" + ElementName;//action pan of opening panal

    var activitylog = ElementName.search('ActivityLog');


    if (activitylog != -1) {
        cmd.Path = "./EMR/HTML/AuditbleEvents/AuditbleEvents_" + ElementName + ".html";
    }



    var Tab = GetTab(cmd.TabID);
    var billTab = cmd;

    if (typeof Tab != "undefined") {
        SelectCurrentTab(billTab.TabID, IsMenu);
    }
    else {
        AddAndSelectTab(billTab, IsMenu);

        //LoadHTMLControl(AdminTab);
        //for (var i = 0; i < TabsArray.length; i++) {
        //    if (isAdminTab(TabsArray[i])) {
        //        if (TabsArray[i].Path.length > 0)
        //            LoadHTMLControl(TabsArray[i]);
        //    }
        //}

    }
}
function AddEncounterTab(ElementName, IsMenu, UniqueId) {
    var cmd = GetEncounterCommand("mstrTabPatient", ElementName, UniqueId, true, "ctrlPanPatient");
    var Tab = GetTab(cmd.TabID);
    var EncounterTab = cmd;

    if (typeof Tab != "undefined") {
        SelectCurrentTab(cmd.TabID, IsMenu, true);
        $("#" + cmd.TabID).trigger("click");
    }
    else {

        AddAndSelectTab(EncounterTab, IsMenu, UniqueId, true);
        setTimeout(function () {
            AddEncounterSubTabs(cmd.TabID, "ChargeCapture,ClinicalNote", UniqueId, cmd.PanelID, cmd.PanelID);
        }, 100);
    }
    return cmd;
}

function AddEncounterSubTabs(ParentTabID, SubTabNames, UniqueId, obj, Cotainer) {
    var arrSubTabName = SubTabNames.split(",");
    for (var i = 0; i < arrSubTabName.length; i++) {
        var selected = false;
        if (arrSubTabName[i] == "ChargeCapture")
            selected = true;
        var cmd = GetEncounterCommand(ParentTabID, arrSubTabName[i], UniqueId, selected, Cotainer);
        var menuTitle = arrSubTabName[i];
        var disableClass = "";
        var title = menuTitle;
        if (menuTitle == "ChargeCapture") {
            menuTitle = "Charge Capture";
        }
        else if (menuTitle == "ClinicalNote") {
            menuTitle = "Clinical Note";
            disableClass = 'disableAll';
        }

        var SubTabClick = "SelectTab('" + cmd.TabID + "','false','true','" + UniqueId + "','" + title + "');EncounterChargeCapture.ChangeSubTabStyle('" + cmd.TabID + "');";
        var Element = $('<a href="#" title="' + menuTitle + '" id="' + cmd.TabID + '" onclick= ' + SubTabClick + ' class="' + disableClass + '" >' + menuTitle + '</a>');
        $("#" + obj).find("div#DivEncounterTabs").append(Element);
        AddAndSelectTab(cmd, true, UniqueId, selected);

    }
}

function GetEncounterCommand(ParentTabID, ElementName, UniqueId, Selected, Container) {
    var cmd = [];
    cmd.TabID = "Encounter" + ElementName + UniqueId;
    cmd.PanelID = "pnlEncounter" + ElementName + UniqueId; //+ UniqueId;//panal id from opening html control
    cmd.MasterTabID = ParentTabID;
    cmd.ParentTabID = ParentTabID;
    cmd.ContainerControlID = "Encounter" + ElementName;
    cmd.Selected = Selected;
    cmd.Container = Container;// in which container it will be open
    cmd.Path = "./Controls/Patient/Encounter/Encounter_" + ElementName + ".html";
    cmd.ActionPanContainer = "actionPanEncounter" + ElementName + UniqueId;//action pan of opening panal
    return cmd;
}

function RemoveEncounterTab(TabID) {
    RemoveAdminTab(TabID, "Encounter");
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

function CreateAdminTabs(tabId) {
    //var Tab = GetTab(tabId);
    //var nodes = document.getElementById(Tab.PanelID).getElementsByTagName('*');
    //if (nodes.length===0) {
    //    //Get all Tab Id's of Menu Control
    //    var MenuPanal = Tab.PanelID.replace(/Div/gi, "Menu");
    //    nodes = document.getElementById(MenuPanal).getElementsByTagName('li');
    //        for (var i = 0; i < nodes.length; i++) {
    //            var liName = nodes[i].id;
    //            //Append ButtonControl to AdminTab
    //                var ElementId = liName.replace(/Menu/gi, "Tab");
    //                var ElementTitle = ElementId.replace(/adminTab/gi, "");
    //                if (i == 0)
    //                {
    //                    var Element = $('<button type="button" class="btn btn-default btn-sm active tab_space" title="Admin ' + ElementTitle + '" id="' + ElementId + '" onclick=SelectTab("' + ElementId + '","false");>' + ElementTitle + '</button>');
    //                }
    //                else
    //                {
    //                    var Element = $('<button type="button" class="btn btn-default btn-sm tab_space" title="Admin ' + ElementTitle + '" id="' + ElementId + '" onclick=SelectTab("' + ElementId + '","false");>' + ElementTitle + '</button>');
    //                    //Add Commands to Array....
    //                    CreateTab(ElementTitle);
    //                }
    //            //Append Element
    //                $("#" + Tab.PanelID).append(Element);
    //              }
    //}

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
                ////var selectedParentLiId = $("#ClinicalUL li.nav-expanded").attr("id");
                ////var selectedChildLiId = $("#ClinicalUL li.nav-expanded ul li.nav-active").attr("id");
                //////$("#" + selectedParentLiId).removeClass("nav-expanded");
                //////$("#" + selectedParentLiId).removeClass("nav-expanded");
                ////$("#ClinicalUL li#" + selectedParentLiId + " a:first").trigger("click");
                //////$("#" + selectedParentLiId).addClass("nav-expanded");
                ////$("#ClinicalUL li.nav-expanded ul li#" + selectedChildLiId + " a:first").trigger("click");
                ////$("button#" + selectedChildLiId).trigger("click");
                //////$("#" + $("#hfClinicalMenuChildLiId").val()).find("a:first").trigger("click");
            });

            //$("div[id*=mstrDiv]").hide();
            //$.when(ClinicalMenuSettings.ClinicalMenuSettingsSearch(null, null)).then(function () {
            //    $("#ClinicalUL li#" + selectedParentLiId + " a:first").trigger("click");
            //    $("#ClinicalUL li.nav-expanded ul li#" + selectedChildLiId + " a:first").trigger("click");
            //});
            //$("#ClinicalUL li.nav-expanded a:first").trigger("click");
            //$("#ClinicalUL li.nav-expanded ul li.nav-active")

            //// If menu is already selected then it should not reload menu
            //if ($("#ClinicalUL li.nav-expanded").length > 0 && $("#ClinicalUL li.nav-expanded ul li.nav-active").length > 0) {

            //    //return;
            //}
            //else {
            //    $("div[id*=mstrDiv]").hide();
            //    ClinicalMenuSettings.ClinicalMenuSettingsSearch(null);
            //}



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


    //if (selectedChildTab && IsCreate) {
    if (!IsActionPanPatientLoaded) {
        store.saveSession('TabsArray', TabsArray);

        if ((tabID == "mstrTabPatient" || Tab.MasterTabID == 'mstrTabPatient') && (PatientArray.length <= 0)) {

            if (tabID != 'patTabDemographic') {
                $('#mstrDivPatient').children().remove();
                $.each($('#mstrMenuPatient li a'), function (i, item) {
                    LoadPatientTab($(item).attr('action'), true, false);
                });
                $('#patTabDemographic').click();
                //SelectCurrentTab('patTabDemographic', 'true');
                //$('#mstrDivPatient #patTabDemographic').addClass('active');
                return false;

            }
            if (params.QuickAddPatient != true)
                LoadActionPan('Patient_Search', null);
            delete params.QuickAddPatient;


            if ($('#PatientProfile').css('display') != 'none') {
                $('#PatientProfile').css('display', 'none');
            }

            //if (Tab.Container != 'ctrlPanPatient') {
            //    if ($('#' + Tab.Container).css('display') != 'none') {
            //        $('#' + Tab.Container).hide('slide', 200);
            //    }
            //    //mstrDivPatient
            //    //if ($('#mstrTabPatient').css('display') != 'none') {
            //    //    $('#mstrTabPatient').hide('slide', 200);
            //    //}
            //}

        }
        else if ((tabID == "mstrTabPatient" || Tab.MasterTabID == 'mstrTabPatient') && (PatientArray.length > 0)) {
            var selectedCount = 0;
            $("#mstrDivPatient").children().each(function () {
                if ($(this).hasClass("tab_selected")) {
                    selectedCount = selectedCount + 1;
                }
            });
            if (selectedCount == 0) {
                $('#mstrDivPatient').children().remove();
                $.each($('#mstrMenuPatient li a'), function (i, item) {
                    LoadPatientTab($(item).attr('action'), true, false);
                });
                $('#patTabDemographic').click();
                //SelectTab("patTabDemographic", false);
                return false;
            } else {
                if ((!$('#PatientProfile').css('display') || $('#PatientProfile').css('display') == 'none') && PatientArray.length > 0) {
                    $('#PatientProfile').show('blind', 200);
                }
                LoadControl(Tab, params);
            }

        }
        else if (tabID == "mstrTabClinical" && (params["patientID"] == null || params["patientID"] == "" || params["patientID"] == "-1") && isInTimeOut == 0) {
            LoadActionPan('Patient_Search', null);
            if ($('#PatientProfile').css('display') != 'none') {
                $('#PatientProfile').css('display', 'none');
            }
            // this is the check, if patient is selected from clinical tab, and now naviagte to Patient Tab, than Patient Tab buttons should display and demographic select should trigger
        } else if ((tabID == "mstrTabPatient" || Tab.MasterTabID == 'mstrTabPatient') && tabID != 'patTabDemographic' && (params.TabID != null && (params.TabID.indexOf('clinical') != -1 || params.TabID == "mstrTabDashBoard" || params.TabID == "billTabOutOfOfficeVisits") && params.patientID != "-1")) {
            if (tabID != 'patTabDemographic') {
                $('#mstrDivPatient').children().remove();
                $.each($('#mstrMenuPatient li a'), function (i, item) {
                    LoadPatientTab($(item).attr('action'), true, false);
                });
                if (Tab.MasterTabID != 'mstrTabPatient') {
                    $('#patTabDemographic').click();
                }
                return false;
            }
        } else {

            //if (!$('#' + Tab.Container).css('display') || $('#' + Tab.Container).css('display') == 'none' && PatientArray.length > 0) {
            //        $('#' + Tab.Container).show('fade', 200);
            //    }


            if ((!$('#PatientProfile').css('display') || $('#PatientProfile').css('display') == 'none') && PatientArray.length > 0) {
                $('#PatientProfile').show('blind', 200);
            }
            //if ($('#mstrTabPatient').css('display') == 'none') {
            //    $('#mstrTabPatient').show();
            //}


            if (Tab.TabID.indexOf("EncounterVisit_Detail") > -1) {
                LoadControl(Tab, params, true);
            } else {
                LoadControl(Tab, params);
            }


        }

    }
    else
        LoadControl(Tab, param);

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
            if ($(SelectedpnlObj).attr('id') != null && $(SelectedpnlObj).attr('id').toLowerCase().indexOf("paymentposting") < 0)
                SelectedpnlObj.find("form :input:not(button):not(hidden):not([data-plugin-timepicker]):not([data-plugin-datepicker]):not([data-plugin-keyboard-numpad]):not([id*='date']):not([id*='Date']):not([id*='dtp']):enabled:visible:first").focus();
        }, 400);
    }

    if (Tab != null && Tab.MasterTabID != null && Tab.MasterTabID != "" && Tab.MasterTabID != 'mstrTabClinical') {
        setParentChildMenuId("mainUL");
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


//function showChildTab(Tab, IsCreate) {
//    var selectedChildTab = GetSelectedTab(Tab.TabID);
//    if (Tab.MasterTabID != "") {
//        if (selectedChildTab) {
//            $('#' + selectedChildTab.PanelID).show();
//            Tab.ContainerControlID = selectedChildTab.ContainerControlID;
//            Tab.Container = selectedChildTab.Container;
//        }
//    }

//    //means master tab clicked
//    if (Tab.MasterTabID == "") {
//        if (selectedChildTab) {
//            $('#' + selectedChildTab.PanelID).show();
//            Tab.ContainerControlID = selectedChildTab.ContainerControlID;
//            Tab.Container = selectedChildTab.Container;
//        }
//    }


//    //if (selectedChildTab && IsCreate) {
//    if (!bIsDetailActionPanLoaded) {
//        store.saveSession('TabsArray', TabsArray);
//        LoadControl(Tab, params);
//    }
//    else
//        LoadControl(Tab, param);
//    //}
//    //else { LoadControl(Tab, params); }

//    //return selectedChildTab;
//}

function GetTab(tabID) {
    for (var i = 0; i < TabsArray.length; i++) {
        if (TabsArray[i].TabID == tabID) return TabsArray[i];
    }
}

function OpenDemographicLabel() {
    var params = [];
    params["PatientID"] = GetSelectedPatientID();
    // params["providerid"] = $('#pnlClinicalLabOrder #hfprovider').val();
    // params["ParentCtrl"] = 'Clinical_LabOrder';
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
    //if (!$(SelectedpnlObj).css('display') || $(SelectedpnlObj).css('display') == 'none') {
    //    $(SelectedpnlObj).show('fade', 200);
    //}
    //screenAdjustment();
}

//function SelectStaticTab(tabIds, pnlIds) {
//    var arrTabs = tabIds.split(',');
//    var arrPnls = pnlIds.split(',');

//    for (i = 0; i < arrTabs.length; i++) {
//        var tabObj = document.getElementById(arrTabs[i]);
//        var pnlObj = document.getElementById(arrPnls[i]);
//        if (i == 0) {
//            $(tabObj).find(".pageTabLeft").removeClass("pageTabLeft").addClass("pageTabLeftSelected");
//            $(tabObj).find(".pageTabRight").removeClass("pageTabRight").addClass("pageTabRightSelected");
//            $(tabObj).find("div").removeClass("pageTab").addClass("pageTabSelected");
//            if (arrPnls[i] == "pnlFaxViewer") {
//                $(pnlObj).css("height", "400px");
//                $(pnlObj).css("width", "100%");
//            }
//            else
//                $(pnlObj).css("display", "block");
//        } else {
//            $(tabObj).find(".pageTabLeftSelected").removeClass("pageTabLeftSelected").addClass("pageTabLeft");
//            $(tabObj).find(".pageTabRightSelected").removeClass("pageTabRightSelected").addClass("pageTabRight");
//            $(tabObj).find("div").removeClass("pageTabSelected").addClass("pageTab");

//            if (arrPnls[i] == "pnlFaxViewer") {
//                $(pnlObj).css("height", "0px");
//                $(pnlObj).css("width", "0px");
//            }
//            else
//                $(pnlObj).css("display", "none");
//        }
//    }
//}

function LoadControl(Tab, param, Reload) {
    var patientID = null;
    if (isPatientTab(Tab) || Reload) {
        patientID = GetSelectedPatientID();
        //if (IsActionPanPatientLoaded == true) {
        //    // using for edit mode show only demograpic form
        //    if (!store.fetch(Tab.ContainerControlID, "patientEdit_" + patientID) || Reload) {
        //        var b = eval(Tab.ContainerControlID + '.Load')(param);
        //        store.save(Tab.ContainerControlID, true, "patientEdit_" + patientID)
        //    }
        //}
        //else {
        //if (Tab.ContainerControlID.toLowerCase() == "patientopenvisit" || Tab.ContainerControlID.toLowerCase() == "patientclosedvisit") {
        //    eval(Tab.ContainerControlID + '.Load')(param);
        //}
        //else {
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
    //}
    //if (Tab.TabID == "patTab_Medication" && Reload)
    //{
    //    eval(Tab.ContainerControlID + '.Load')(param);
    //}
    //SetScrollPanHeights();
    // screenAdjustment();
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

//function EnableDisableControls(PanalID, Pram) {
//    if (Pram == 1) {
//        var nodes = document.getElementById(PanalID).getElementsByTagName('*');
//        for (var i = 0; i < nodes.length; i++) {
//            nodes[i].disabled = true;

//        }


//        //$("#cover").css("display", "block");
//        ////$('#logout_box').fadeIn("slow");
//        //$(".main-wrapper").css({ // this is just for style
//        //    "opacity": "0.3"
//        //});
//        //var nodes = document.getElementById("pnlTab1").getElementsByTagName('*');
//        //for (var i = 0; i < nodes.length; i++) {
//        //    $("#" + nodes[i].id).addClass("disabled");
//        //    enableElements(nodes[i]);
//        //    //$("#" + nodes[i].id).prop.attr("disabled", "true");

//        //}
//        //var nodes = document.getElementById("pnlTab2").getElementsByTagName('*');
//        //for (var i = 0; i < nodes.length; i++) {
//        //    $("#" + nodes[i].id).addClass("disabled");
//        //    enableElements(nodes[i]);
//        //}
//        //var nodes = document.getElementById("pnlTab3").getElementsByTagName('*');
//        //for (var i = 0; i < nodes.length; i++) {
//        //    $("#" + nodes[i].id).addClass("disabled");
//        //    enableElements(nodes[i]);
//        //   }

//    }
//    else if (Pram == 2) {
//        var nodes = document.getElementById(PanalID).getElementsByTagName('*');
//        for (var i = 0; i < nodes.length; i++) {
//            nodes[i].disabled = false;

//        }
//        $("#pnlTab1").css("display", "inline");
//        $("#pnlTab2").css("display", "inline");
//        $("#pnlTab3").css("display", "inline");
//    }
//}

function setPatientBanner(PatientID, isOpenNotes, CCMterminationReason) {
    // temp change to get previous version

    var dfd = new $.Deferred();
    Patient_Demographic.FillDemographic(PatientID).done(function (response) {
        if (response.status != false) {

            //try {
            //    $("#mainForm  li#CDSAlert").show();
            //    $.when(ClinicalCDSDetail.showCDSAlert("", PatientID)).then(function () {
            //        dfd.resolve();
            //    });
            //}
            //catch (err) {
            //    dfd.resolve();
            //}

            // This Check is to push patient id to PatientArray, which is used for furthur operations in Patient Demographic information
            if (IsPatientExist(PatientID) == false) {
                if (GetSelectedPatientID() != PatientID) {
                    if (PatientID != "-1")
                        AddPatientArray(PatientID);
                    params['patientID'] = PatientID;
                    params['mode'] = "Edit";
                    params['FromAdmin'] = "0";
                }
            }
            var demographic_detail = JSON.parse(response.DemographicFill_JSON);
            if (demographic_detail.imgPatient != undefined && demographic_detail.imgPatient != "") {
                $("#PatientProfile #imgPatientProfile").attr("src", demographic_detail.imgPatient);
            }
            else {
                if (demographic_detail.Sex == "Female") {
                    $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_female_profile.gif");
                }
                else {
                    $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_male_profile.gif");
                }
            }
            Patient_Demographic.FillPatientBar(response.DemographicFill_JSON, PatientID, isOpenNotes, CCMterminationReason);
            $("#PatientProfile").show();
        }
        else {
            utility.DisplayMessages(response.Message, 3);
            dfd.resolve();
        }
    });
    return dfd;
}

// Patient Select , Get and Close Function
function SelectPatient(patientID, FormName) {


    patientID = String(patientID);
    ClosePatient(patientID, true);
    try {
        ClinicalCDSDetail.showCDSAlert("", patientID);
    }
    catch (ex) {
    }

    var dfd = new $.Deferred();
    if (IsPatientExist(patientID) == true) {
        if (GetSelectedPatientID() != patientID) {
            //    SaveSelectedPatientData();
            //    LoadPatientData(patientid);
            //    MakePatientSelected(patientid);

            // params: [];
            params['patientID'] = patientID;
            params['mode'] = "Edit";
            params['FromAdmin'] = "0";
            params['PanelID'] = "pnlDemographic";
            params['SelectedPatientFormName'] = FormName;
            //Patient_Demographic.FillPatientInfo(params).done(function () {
            if ($('#PatientProfile').css('display') == 'none') {
                $('#PatientProfile').show('blind', 200);

            }
            SetSelectedPatient(patientID);
            //});
        }
        SelectCurrentTab('mstrTabPatient', 'true');
        //if (FormName != "schedule") {
        //    if (patientID == "-1")
        //        SelectDefaultPatientTab('mstrTabPatient', 'patTab_NewPatient');
        //    else
        // SelectDefaultPatientTab('mstrTabPatient', 'patTabDemographic', 'false');
        // }

        dfd.resolve('ok');
        //UpdatePanelAjaxLoading(false);
    }
    else {

        //AppCommands.reloadTab();
        //ClosePatient();
        if (patientID != "-1")
            AddPatientArray(patientID);

        SetSelectedPatient(patientID);
        //params.length = 0;
        //params: [];
        params['patientID'] = patientID;
        params['mode'] = "Edit";
        params['FromAdmin'] = "0";
        params['PanelID'] = "pnlDemographic";
        params['SelectedPatientFormName'] = FormName;
        //delete params["ActionPanContainer"];
        //if (globaldata['eRxOnly'] == "true" || mode == 'patTab_Detail') {
        //    SelectDefaultPatientTab('mstrTab_Patient', 'patTab_Detail');
        //}
        //else {
        //    if (mode != "schedule")
        //        SelectDefaultPatientTab('mstrTab_Patient', 'patTab_Chart');
        //}


        //SelectCurrentTab('mstrTabPatient', 'false');
        $('#PatientProfile').css('display', 'none');

        //Patient_Demographic.FillPatientInfo(params).done(function () {


        if ($('#PatientProfile').css('display') == 'none') {
            //var CurrentSelectedTab = GetCurrentSelectedTab();
            //if (CurrentSelectedTab.TabID == "mstrTabHome" || CurrentSelectedTab.MasterTabID == "mstrTabHome")
            //    $('#PatientProfile').css('display', 'none');
            //else {
            $('#PatientProfile').show('blind', 200);

            //}
        }

        //SelectCurrentTab('patTabDemographic', 'false');
        //SelectDefaultPatientTab('mstrTabPatient','patTabDemographic', 'false');

        //  SelectCurrentTab('mstrTabPatient', 'true');

        var CurrentSelectedTab = GetCurrentSelectedTab();


        if (CurrentSelectedTab.TabID != 'patTabDemographic' && CurrentSelectedTab.MasterTabID == 'mstrTabPatient') {
            SelectCurrentTab('patTabDemographic', 'true');
        }
            // to navigate Patient Selection from Batch patient List
            // Author: Muhammad Azhar Shahzad, Date: April 09,2016
        else if (CurrentSelectedTab.TabID == 'batchTabPatientList' || CurrentSelectedTab.TabID == "mstrTabReports") {
            SelectCurrentTab('mstrTabPatient', 'true');
            setPatientBanner(patientID);
            params['mode'] = "Edit";
            $('#mstrDivPatient').children().remove();
            $.each($('#mstrMenuPatient li a'), function (i, item) {
                LoadPatientTab($(item).attr('action'), true, false);
            });
            SelectCurrentTab('patTabDemographic', 'true');
        }
        else if (CurrentSelectedTab.TabID == 'mstrTabClinical' || CurrentSelectedTab.MasterTabID == 'mstrTabClinical') {

            //setPatientBanner(patientID);
            //EMR-50 fix by Azhar Shahzad on 7/20/2016
            $.when(setPatientBanner(patientID)).then(function () {
                //Begin 23-12-2015 Muhammad Arshad Bug#PMS-2863 Clinical -> Patient -> patient demographics should proper display and other (Insurance,auditlog,encounter,message etc) menu should proper display
                params['mode'] = "Edit";
                $('#mstrDivPatient').children().remove();
                $.each($('#mstrMenuPatient li a'), function (i, item) {
                    LoadPatientTab($(item).attr('action'), true, false);
                });
                /*
                    Author Change: Muhammad Azhar Shahzad
                    Change Date: April 04,2016
                    Purpose: If patient is selected from Clinical Tab, than FaceSheet should be loaded by default else, patient selection is processed by Patient Tab.
                */
                if (CurrentSelectedTab.TabID != 'patTabDemographic' && CurrentSelectedTab.MasterTabID == 'mstrTabPatient') {
                    $('#patTabDemographic').click();
                }
                else if (CurrentSelectedTab.TabID != 'patTabDemographic') {
                    if (CheckPatientDemoMissingDetails() == false) {
                        //Start 16-12-2015 Muhammad Arshad Load FaceSheet while clinical tab is loaded
                        //setTimeout(function () { $("#ClinicalUL li#clinicalMenuNotes a:first").trigger("click"); isInTimeOut = 0; }, 1000);
                        //$("#ClinicalUL li#clinicalMenuFaceSheet a:first").trigger("click")
                        $("#ClinicalUL li#clinicalMenuFaceSheet a:first").trigger("click");
                        //
                        $('#ctrlPanPatient').css('display', 'none');
                        document.getElementById("ctrlPanPatient").style.display = "none !important";
                        $('#ctrlPanPatient').css('display', 'none !important');
                    }
                    else {
                        params["mode"] = "Edit";
                        params["PatBanner"] = true;
                        params["IsFill"] = true;
                        LoadActionPan('demographicDetail', params);
                    }
                    isInTimeOut = 0;
                }
                //setTimeout(function () {
                //    params['mode'] = "Add";
                //    SelectCurrentTab('mstrTabClinical', 'true');
                //}, 50);
            });
            //End 23-12-2015 Muhammad Arshad Bug#PMS-2863 Clinical -> Patient -> patient demographics should proper display and other (Insurance,auditlog,encounter,message etc) menu should proper display
        }
            //Start//Abid Ali// patient banner load on Out of office visit selection
        else if (CurrentSelectedTab.TabID == 'mstrTabBilling' || CurrentSelectedTab.MasterTabID == 'mstrTabBilling') {

            $.when(setPatientBanner(patientID)).then(function () {

                //******************************

                params['mode'] = "Edit";
                $('#mstrDivPatient').children().remove();
                $.each($('#mstrMenuPatient li a'), function (i, item) {
                    LoadPatientTab($(item).attr('action'), true, false);
                });
                if (CurrentSelectedTab.TabID != 'patTabDemographic' && CurrentSelectedTab.MasterTabID == 'mstrTabPatient') {
                    $('#patTabDemographic').click();
                }
                //else if (CurrentSelectedTab.TabID != 'patTabDemographic') {
                //    $("#ClinicalUL li#clinicalMenuFaceSheet a:first").trigger("click");
                //    //
                //    $('#ctrlPanPatient').css('display', 'none');
                //    document.getElementById("ctrlPanPatient").style.display = "none !important";
                //    $('#ctrlPanPatient').css('display', 'none !important');

                //    isInTimeOut = 0;
                //}

                //******************************
                LoadBillingTab('billTabOutOfOfficeVisits', true);
                $('#billMenuOutOfOfficeVisits a:first').trigger("click");
                $('#billMenuOutOfOfficeVisits').addClass('nav-active');

            });
        }
            //End//Abid Ali// patient banner load on Out of office visit selection

        else {
            SelectCurrentTab('mstrTabPatient', 'true');
        }

        if (CurrentSelectedTab.MasterTabID == 'mstrTabSchedule')
            SelectCurrentTab(CurrentSelectedTab.MasterTabID, 'true');

        if (CurrentSelectedTab.MasterTabID == 'mstrTabReports')
            SelectCurrentTab(CurrentSelectedTab.MasterTabID, 'true');

        dfd.resolve('ok');
        //});

        //Patient_Demographic.FillDemographic(patientID)
        //    .done(function (response) {
        //        //addtopatientlist(response);
        //        if (patientID != "-1")
        //        {
        //            store.save('selectPatientData', response.DemographicFill_JSON, patientID);
        //            dfd.resolve('ok');
        //        }

        //        //var CurrentSelectedTab = GetCurrentSelectedTab();
        //        //$('#' + CurrentSelectedTab.ActionPanContainer).html('');

        //        Patient_Demographic.FillPatientInfo(patientID);
        //        if ($('#PatientProfile').css('display') == 'none') {
        //            //var CurrentSelectedTab = GetCurrentSelectedTab();
        //            //if (CurrentSelectedTab.TabID == "mstrTabHome" || CurrentSelectedTab.MasterTabID == "mstrTabHome")
        //            //    $('#PatientProfile').css('display', 'none');
        //            //else {
        //            $('#PatientProfile').show('blind', 200);

        //            //}
        //        }

        //    }


        //)
        //.fail(function (xhr, status) {

        //});

    }

    setTimeout(function () {
        //  $("#frmDemographic :input:not(:hidden)").each(function (i) { $(this).attr('tabindex', i + 1); });
        $("#frmDemographic :input:not(button):not(:hidden):enabled:visible:first").focus();
    }, 400);

    return dfd.promise();
}
function ShowPatientProfile() {
    $('#PatientProfile').css('display', 'none');

    if ($('#PatientProfile').css('display') == 'none') {
        //var CurrentSelectedTab = GetCurrentSelectedTab();
        //if (CurrentSelectedTab.TabID == "mstrTabHome" || CurrentSelectedTab.MasterTabID == "mstrTabHome")
        //    $('#PatientProfile').css('display', 'none');
        //else {
        $('#PatientProfile').show('blind', 200);

        //}
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

function MainClosePatient() {
    // if user is on demographic/Insurance tab 
    if (params.DemographicAutoUpdateActiveTab) {
        var formId = "#pnlDemographic #frmDemographic";
        if (params.DemographicAutoUpdateActiveTab == "Insurance")
            formId = "#pnlPatientInsurance #frmPatientInsurance";
        var formCtr = $(formId);
        if (formCtr.serialize() != params.defaultDemographicSerailizeForm && params.IsDemographicInfoGlobalyUpdated == false) {

            var ScreenName = params.DemographicAutoUpdateActiveTab;
            if (params.DemographicAutoUpdateActiveTab == "Insurance")
                ScreenName = "Patient Insurances";
            utility.myConfirm('Are you sure you want to save ' + ScreenName + ' change?', function () {
                params.IsDemographicInfoGlobalyUpdated = true;
                delete params.DemographicAutoUpdateActiveTab;
                if (formId == "#pnlPatientInsurance #frmPatientInsurance")
                    $("#pnlPatientInsurance #frmPatientInsurance").trigger("submit");
                else
                    $("#pnlDemographic #frmDemographic").trigger("submit");

                // confirm data post request is successfully updated
                if (params.IsDemographicInfoGlobalyUpdated)
                { ApplicationPatientClose(); }

            }, function () {  // reset active tab screen to its default state
                if (params.DemographicAutoUpdateActiveTab == "Demographic")
                    Patient_Demographic.LoadPatientDemogrphic();
                else Patient_Insurance.LoadInsuranceList();
                //  reset flag to its default value 
                if (params.IsDemographicInfoGlobalyUpdated)
                { params.IsDemographicInfoGlobalyUpdated = false; }

                ApplicationPatientClose();

            },
                           'Confirm Change'
                       );


        }
        else {
            ApplicationPatientClose();
        }
    }
    else {
        ApplicationPatientClose();
    }

}
function ApplicationPatientClose() {
    var SelectedTabID = selectedtab.TabID.toLowerCase();
    if (SelectedTabID == "pattabdemographic") {
        if ($('#frmDemographic').serialize() != $('#frmDemographic').data('serialize')) {
            utility.myConfirm('10', function () {
                ClosePatientNew();
                //Start//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
                $(" #mainForm  li#CDSAlert input").val('');
                $(" #mainForm  li#CDSAlert span").text('');
                $(" #mainForm  li#CDSAlert").hide();

                $(" #mainForm  li#ImmunizationAlert input").val('');
                $(" #mainForm  li#ImmunizationAlert span").text('');
                //$(" #mainForm  li#ImmunizationAlert").hide();

                //End//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
                $('#mstrMenuPatient ul li').removeClass("nav-active");


            }, function () {
                //AZAM AFTAB Dated January 11,2015 PMS-3230 Patient Demographic
                // SelectTab('patTabDemographic', 'false');
            });
        }
        else {
            ClosePatientNew();
            $('#mstrMenuPatient ul li').removeClass("nav-active");

            //Start//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
            $(" #mainForm  li#CDSAlert input").val('');
            $(" #mainForm  li#CDSAlert span").text('');
            $(" #mainForm  li#CDSAlert").hide();

            $(" #mainForm  li#ImmunizationAlert input").val('');
            $(" #mainForm  li#ImmunizationAlert span").text('');
            //$(" #mainForm  li#ImmunizationAlert").hide();
            //End//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
        }
    }
    else if (SelectedTabID == "pattabinsurance") {
        if ($('#frmPatientInsurance').serialize() != $('#frmPatientInsurance').data('serialize')) {
            utility.myConfirm('10', function () {
                ClosePatientNew();
                $('#mstrMenuPatient ul li').removeClass("nav-active");

                //Start//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
                $(" #mainForm  li#CDSAlert input").val('');
                $(" #mainForm  li#CDSAlert span").text('');
                $(" #mainForm  li#CDSAlert").hide();

                $(" #mainForm  li#ImmunizationAlert input").val('');
                $(" #mainForm  li#ImmunizationAlert span").text('');
                //$(" #mainForm  li#ImmunizationAlert").hide();
                //End//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value

            }, function () {
                // SelectTab('patTabInsurance', 'false');
            });
        }
        else {
            ClosePatientNew();
            $('#mstrMenuPatient ul li').removeClass("nav-active");

            //Start//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
            $(" #mainForm  li#CDSAlert input").val('');
            $(" #mainForm  li#CDSAlert span").text('');
            $(" #mainForm  li#CDSAlert").hide();

            $(" #mainForm  li#ImmunizationAlert input").val('');
            $(" #mainForm  li#ImmunizationAlert span").text('');
            //$(" #mainForm  li#ImmunizationAlert").hide();
            //End//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
        }
    }
    else if (SelectedTabID.indexOf("clinical") > -1) {
        if ($('#frmClinicalMenuSettings').serialize() != $('#frmClinicalMenuSettings').data('serialize')) {
            utility.myConfirm('10', function () {
                ClosePatientNew();
                $('#mstrMenuPatient ul li').removeClass("nav-active");

                //Start//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
                $(" #mainForm  li#CDSAlert input").val('');
                $(" #mainForm  li#CDSAlert span").text('');
                $(" #mainForm  li#CDSAlert").hide();

                $(" #mainForm  li#ImmunizationAlert input").val('');
                $(" #mainForm  li#ImmunizationAlert span").text('');
                //$(" #mainForm  li#ImmunizationAlert").hide();
                //End//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value

            }, function () {
                // SelectTab('mstrTabClinical', 'false');
                //end PMS-3230 Patient Demographic
            });
        }
        else {
            ClosePatientNew();
            $('#mstrMenuPatient ul li').removeClass("nav-active");

            //Start//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
            $(" #mainForm  li#CDSAlert input").val('');
            $(" #mainForm  li#CDSAlert span").text('');
            $(" #mainForm  li#CDSAlert").hide();

            $(" #mainForm  li#ImmunizationAlert input").val('');
            $(" #mainForm  li#ImmunizationAlert span").text('');
            //$(" #mainForm  li#ImmunizationAlert").hide();
            //End//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
        }
    }
    else {
        ClosePatientNew();
        $('#mstrMenuPatient ul li').removeClass("nav-active");

        //Start//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
        $(" #mainForm  li#CDSAlert input").val('');
        $(" #mainForm  li#CDSAlert span").text('');
        $(" #mainForm  li#CDSAlert").hide();

        $(" #mainForm  li#ImmunizationAlert input").val('');
        $(" #mainForm  li#ImmunizationAlert span").text('');
        //$(" #mainForm  li#ImmunizationAlert").hide();
        //End//15-03-2016//Ahmad Raza//hiding CDSAlert icon on patient close and removing hidden field's value
    }

    //var CurrentTab = GetCurrentSelectedTab();

    //if ($('#frmDemographic').serialize() != $('#frmDemographic').data('serialize')) {
    //    utility.myConfirm('10', function () {

    //        ClosePatient();

    //    }, function () { SelectTab('patTabDemographic', 'false'); });
    //}
    //else if ($('#frmPatientInsurance').serialize() != $('#frmPatientInsurance').data('serialize')) {

    /* utility.myConfirm('10', function () {
    
            ClosePatient();
    
        }, function () {
            if ($('#frmPatientInsurance').is(':visible')) {
                SelectTab('patTabInsurance', 'false');
            } else {
                SelectTab('patTabDemographic', 'false');
            }
        });*/

    //}
    //else {
    //    ClosePatient();
    //}
};
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

function ClosePatient(patientID, bit) {


    $("#btnPatientAlerts").css("display", "none");
    //$("#btnPatientMessages").css("display", "none");
    var selectedPatientID = GetSelectedPatientID();
    //if (typeof patientID == 'undefined')
    //    patientID = $("#PatientProfile #hfPatientId").val()

    if (typeof selectedPatientID == 'undefined')
        return

    if (selectedPatientID == '')
        return

    if (typeof selectedPatientID != 'undefined') {
        if (selectedPatientID != patientID) {
            //if (patientID != -1)
            //    uploadImage.RemovePicture(selectedPatientID);
        }

    }

    for (var i = 0; i < PatientArray.length; i++) {
        if (PatientArray[i].PatientID == selectedPatientID) {
            PatientArray.splice(i, 1);
        }
    }

    store.saveSession('selectedPatientArray', PatientArray);
    store.clearAllBySetName(selectedPatientID);






    if (PatientArray.length <= 0) {
        //unload the html



        //var CurrentActionPanContainer = CurrentSelectedTab.ActionPanContainer;
        //if (CurrentSelectedTab.MasterTabID == "mstrTabPatient")
        //    CurrentActionPanContainer = "actionPanPatient";
        //$('#' + CurrentActionPanContainer).find('div').first().hide('blind', 500, function () { $(this).remove(); });

        // $('#btnPatientList').css("display", "none");

        //if (CurrentSelectedTab.TabID != "mstrTab_Schedule" && CurrentSelectedTab.MasterTabID != "mstrTab_Schedule")
        //    SelectTab('mstrTabHome');
        //else
        //    $('#CurrentPatientInfo').hide('blind', 200);
        $('#PatientProfile').css('display', 'none');
        $('#PatientProfile #lblPatientData').html("");
        $('#PatientProfile #hfPatientId').val("");
        $('#PatientProfile #hfAccountNo').val("");

        $('#pnlDemographic #frmDemographic').resetAllControls();
        $('#pnldemographicDetail #frmdemographicDetail').resetAllControls();
        $('#pnlDemographicQuick #frmDemographicQuick').resetAllControls();
        $('#pnlPatientInsurance #frmPatientInsurance').resetAllControls();
        $('#pnlPatientDocument #frmPatientDocument').resetAllControls("#dgvPatientDocument,#dgvPatientDocumentReviewed");
        $('#pnlPatientMessage').resetAllControls("#dgvPatientMessage");
        $('#pnlPatientCase').resetAllControls("#dgvPatientCase");
        $('#pnlActivityLog #frmActivityLog').resetAllControls("#dgvNewEntry,#dgvUser,#dgvChanges");
        $('#pnlEncounter #frmVisits').resetAllControls("#dgvOpenVisitsDetail,#dgvCloseVisitsDetail");
        // Remove Encounter Tabs
        $('#mstrDivPatient [id*="EncounterVisit_Detail"]').each(function () {
            RemoveEncounterTab($(this).attr("id"));
            $(this).remove();
        });//.remove();
        $('[id*="pnlEncounterVisit_Detail"]').remove();//.resetAllControls();
        $('[id*="pnlEncounterChargeCapture"]').remove();//.find('#frmEncounterChargeCapture').resetAllControls("#dgvICD,#dgvCPT,#dgvModifier,#dgvVisitCharge");
        $("#pnlDemographic #divDemographicPicture #imgPatient").attr("src", "Content/images/default_male_profile.gif");
        $('#pnlPatientInsurance #frmPatientInsurance #scanImage #image').attr("src", "Content/images/idcard1.png")

        // AppCommands.reloadTab();

        //$('#ctrlPanPatient').find('div').first().hide('blind', 500, function () { $(this).remove(); });
        //SelectPatient("-1");

        // /// empty the patient record
        // params['patientID'] = '-1';
        // params['mode'] = "Edit";
        // params['FromAdmin'] = "0";
        // params['PanelID'] = "pnlDemographic";
        // AddPatientArray(params['patientID']);
        //// SelectDefaultPatientTab('mstrTabPatient', 'patTabDemographic', 'false');
        // SelectCurrentTab('patTabDemographic', 'false');
        // for (var i = 0; i < PatientArray.length; i++) {
        //     if (PatientArray[i].PatientID == params['patientID']) {
        //         PatientArray.splice(i, 1);
        //     }
        // }
        // store.saveSession('selectedPatientArray', PatientArray);
        // store.clearAllBySetName(params['patientID']);
        //$('#PatientProfile').hide('blind', 200);

        //SelectCurrentTab('mstrTabPatient', 'false');

        //this code will get all the  jquery classes
        var TabsPatientList = UnloadTabParams("mstrTabPatient");

        if (typeof params != 'undefined' && params != null) {

            params = [];
            params['patientID'] = "-1";
            params["FromAdmin"] = "1";
            params["PreviousTab"] = GetSelectedTab();
            //params.patientID = "-1";
            //params.PatientInsuranceId = "-1";
            //params.PatientProviderId = "-1";
            //params.mode = "";
            //params.PatientFirstName = "";
            //params.PatientLastName = "";
            //params.PatientProvider = "";
            //params.PatientAccountNo = "";
        }
        if (typeof bit == 'undefined') {
            var tab = GetCurrentSelectedTab();
            var CurrentTabID = "mstrTabDashBoard";
            if (tab.TabID != 'patTabDemographic' && tab.ParentTabID != "mstrTabPatient") {
                CurrentTabID = tab.TabID;
            }

            SelectCurrentTab(CurrentTabID, 'false');
        }

        //patTabDemographic


    }



    //else if (selectedPatientID == patientID && typeof bit == 'undefined') {
    //    //if (CurrentSelectedTab.TabID != "mstrTabSchedule" && CurrentSelectedTab.MasterTabID != "mstrTab_Schedule")
    //    //    SelectPatient(PatientArray[PatientArray.length - 1].PatientID);
    //    //else
    //        SelectPatient(PatientArray[PatientArray.length - 1].PatientID, 'schedule');
    //}
}

function ClosePatientNew(patientID, bit) {
    $('#btnDemographicLabel').hide();
    var selectedPatientID = GetSelectedPatientID();

    localStorage.removeItem("SelectedPatientId");
    localStorage.removeItem("SelectedAccountNumber");
    localStorage.removeItem("SelectedPatientInsurance");
    localStorage.removeItem("SelectedPatientSex");
    localStorage.removeItem("SelectedPatientMaritalStatus");
    localStorage.removeItem("SelectedPatientEthnicityIds");
    localStorage.removeItem("SelectedPatientRaceIds");
    localStorage.removeItem("SelectedPatientAddress1");
    localStorage.removeItem("SelectedPatientCity");
    localStorage.removeItem("SelectedPatientState");
    localStorage.removeItem("SelectedPatientZip");
    localStorage.removeItem("SelectedPatientHomeTel");

    store.clearAllSession();
    store.clearAll();
    for (var i = 0; i < PatientArray.length; i++) {
        if (PatientArray[i].PatientID == selectedPatientID) {
            PatientArray.splice(i, 1);
        }
    }
    //if (PatientArray.length > 0) {
    store.saveSession('selectedPatientArray', PatientArray);
    if (selectedPatientID != null)
        store.clearAllBySetName(selectedPatientID);

    Patient_Demographic.isFinanicialAlert = true;
    Patient_Demographic.isDocExpiryAlert = true;
    $('#ctrlPanPatient').empty();
    $('#PatientProfile').css('display', 'none');
    $('#PatientProfile #lblPatientData').html("");
    $('#PatientProfile #hfPatientId').val("");
    $('#PatientProfile #hfAccountNo').val("");
    $('#PatientProfile #hfPatientSex').val("");
    $('#PatientProfile #hfPatientMaritalStatus').val("");
    $('#PatientProfile #hfPatientEthnicityIds').val("");
    $('#PatientProfile #hfPatientRaceIds').val("");
    $('#PatientProfile #hfPatientAddress1').val("");
    $('#PatientProfile #hfPatientCity').val("");
    $('#PatientProfile #hfPatientState').val("");
    $('#PatientProfile #hfPatientZip').val("");
    $('#PatientProfile #hfPatientHomeTel').val("");

    var TabsPatientList = UnloadTabParams("mstrTabPatient");


    if (typeof params != 'undefined' && params != null) {
        params = [];
        params['patientID'] = "-1";
        params["FromAdmin"] = "1";
    }
    if (typeof bit == 'undefined') {
        var tab = GetCurrentSelectedTab();

        var CurrentTabID = "mstrTabDashBoard";
        if (tab.TabID != 'patTabDemographic' && tab.TabID != "mstrTabClinical" && tab.ParentTabID != "mstrTabPatient" && tab.ParentTabID != "mstrTabClinical") {
            CurrentTabID = tab.TabID;
        }
        //else if (tab.ParentTabID == "mstrTabClinical") {
        //    CurrentTabID = tab.ParentTabID;
        //}
        SelectCurrentTab('mstrTabDashBoard', 'false');
    }
    var PatientTabs = new Array();
    TabsArray.filter(function (obj) {
        if (obj.ParentTabID == "mstrTabPatient")
            PatientTabs.push(obj)
        else if (obj.MasterTabID == "mstrTabClinical")
            PatientTabs.push(obj)
        else if (obj.MasterTabID == "mstrTabBilling" && obj.ContainerControlID == "OutOfOfficeVisits")
            PatientTabs.push(obj)
    });
    $.each(PatientTabs, function (i, tab) {

        if (tab.Container == "ctrlPanClinical") {
            $("#" + tab.Container).html("");
            UnloadTabParams('mstrTabClinical');
            $("#hfClinicalMenuParentLiId").val('');
            $("#hfClinicalMenuChildLiId").val('');
        }
        if (tab.Container == "ctrlPanBilling") {
            //$("#" + tab.PanelID).html("");
            //UnloadTabParams('mstrTabBilling');
            // $("#hfClinicalMenuParentLiId").val('');
            // $("#hfClinicalMenuChildLiId").val('');
            // $("#mstrDivBilling button#billTabOutOfOfficeVisits").parent().remove();//.html('');

            //$("#" + OutOfOfficeVisits.params["PanelID"] + " #pnlBillOOOVisits_Result #dgvOOOVisits").remove();
            SelectTab('mstrTabDashBoard', 'false');

            if ($("#pnlBillOutOfOfficeVisits").length > 0) {
                $("div[id*=pnlBillOutOfOfficeVisits]").remove();
            }
        }
        RemoveTab(tab);
    });
    $('#mstrDivPatient').empty();
    //SelectTab('mstrTabDashBoard', 'false');

}



//function PatientLoad(patientID, Mode, ParentCtrl) {



//    //if (patientID != '-1')
//    //    //SaveSelectedPatientData();


//    ////if (IsPatientExist(patientID) != true) {
//    ////    AppCommands.reloadTab();

//    ////    AddPatientArray(patientID);

//    ////}

//    //SelectCurrentTab('mstrTabPatient', 'false');
//    params: [];
//    params["mode"] = Mode;
//    params["patientID"] = patientID;
//    params["FromAdmin"] = "0";
//    params["ParentCtrl"] = ParentCtrl;
//    LoadActionPan('demographicDetail', params);

//    if (Mode != 'Edit') {
//        if (IsPatientExist(patientID) != true) {
//            //AppCommands.reloadTab();
//            AddPatientArray(patientID);
//        }
//        SetSelectedPatient(patientID);
//    }
//   // SelectDefaultPatientTab('mstrTab_Patient', 'patTab_NewPatient');
//    //SelectDefaultPatientTab('', 'patTab_NewPatient');
//   // $("#tabDiv_Patient").hide();
//   // $("#mainScrollPan_PatientNew").show();
//}

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
function OpenUserMessages() {
    var params = [];
    var tab = GetCurrentSelectedTab();
    if (tab.TabID != 'mstrTabDashBoard') {
        params["ParentCtrl"] = tab.TabID;
    }
    else
        params["ParentCtrl"] = "mstrTabDashBoard";
    //if ($("#spnMessageCount").text() > 0) {
    LoadActionPan('Patient_UserMessagesQuickLink', params)
    //}
    //else {
    //    $("#btnPatientMessages").attr("title", "New Message");
    //    params["mode"] = "Add";
    //    //params["ParentCtrl"] = "";
    //    LoadActionPan('Patient_MessageAdd', params);
    //}
}
function OpenFaxes() {
    var params = [];
    params["AssignedToId"] = globalAppdata['AppUserId'];
    params["IsUserMessages"] = "1";
    LoadActionPan('Batch_Fax', params)

}
function OpenUserTasks() {
    var params = [];
    params["AssignedToId"] = globalAppdata['AppUserId'];
    params["IsUserMessages"] = "1";
    //params["ParentCtrl"] = "patTabDemographic";
    //if ($("#spnUserTasksCount").text() > 0) {
    LoadActionPan('User_Task', params)
    //}
    //else {
    //    $("#btnUserTasks").attr("title", "New Task");
    //    params["mode"] = "Add";
    //    params["RefCtrl"] = "User_Task";
    //    LoadActionPan('Patient_MessageAdd', params);
    //}
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


            //initialization
            //   NotesCount = parsedJson.spnNotesCount;
            //  AppCount = parsedJson.spnAppCount;
            AlertCount = parsedJson.spnAlertCount;
            //  MessageCount = parsedJson.spnMessageCount;
            // UserTasksCount = parsedJson.spnUserTasksCount;

            //setting the values in respective spans

            //  $("#spnNotesCount").text(NotesCount);
            //  $("#spnAppCount").text(AppCount);
            $("#spnAlertCount").text(AlertCount);
            //  $("#spnMessageCount").text(MessageCount);
            //   $("#spnUserTasksCount").text(UserTasksCount);

        }

    });
}


function getNotificationsCounts() {
    var data = "PatientID=" + params.patientID;
    // serach parameter , class name, command name of class
    return MDVisionService.defaultService(data, "DASHBOARD", "SEARCH_NOTIFICATIONS_COUNTS");
}


function OpenDashBoardQuickDocuments() {
    // verfiy Dashboard tab is active or not. 
    if (!$("#mstrTabDashBoard").hasClass("active")) {
        $("#mstrTabDashBoard").find('a').click();
    }
    $("#widgetpanel #Documents").click();
    // DashBoard.DashBoardDocumentSearch(null, null, null);

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
                    // if(params.IsDemographicInfoGlobalyUpdated)
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
