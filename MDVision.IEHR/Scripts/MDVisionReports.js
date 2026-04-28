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
var DefaultMenuSelected = "MDVisionDefault";

function DocumentCustomViewMode() {
    setTimeout(function () {
        customViewMode();
    }, 520);

}

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

function OpenVisitDetail(VisitId, patientID, IsFromDetail) {
    var strMessage = "";
    AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var params = [];
            params["FromAdmin"] = 0;
            if (ReportsViewer_Detail.params.FormName == "Charges and Payments Reports/ClaimStatusDashboardDetail" || IsFromDetail == "1") {
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

function OpenPatientDemographics(patientid, IsFromDetail) {
    var strMessage = "";
    AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var params = [];
            params["mode"] = 'Edit';
            params["PatBanner"] = true;
            params["patientID"] = patientid;
            params["IsFill"] = false;
            params["FromAdmin"] = "0";
            if (ReportsViewer_Detail.params.FormName == "Charges and Payments Reports/ClaimStatusDashboardDetail" || IsFromDetail == "1") {
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
   
    params["ParentCtrl"] = "mstrTabReports";
  
   
    LoadActionPan('Clinical_NotesView', params);

}

function SetGlobalAppData(id, data) {
    globalAppdata[id] = data;
}

function setDefaultValuesForScanCanvas(w, h) {
    localStorage.DWT_height = ($(window).height() - h) < 500 ? $(window).height() - (h + 100) : $(window).height() - h;
    localStorage.DWT_width = $(window).width() - w;
}
function reSetDefaultValuesForScanCanvas() {
    localStorage.DWT_height = $(window).height() - 250;
    localStorage.DWT_width = $(window).width() - 500;
}
function LoadApplication() {
    //fix PMS-2706
    if (localStorage.SelectPatientEntityId != globalAppdata.SeletedEntityId) {
        localStorage.removeItem('SelectedPatientId');
        localStorage.removeItem('SelectPatientEntityId');
    }

    //set Default height and Width for Scanner canvas. 
    setDefaultValuesForScanCanvas(500, 250);

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


    }
    else {
        LoadHomeTab();
    }
    Batch_Fax_QuickLink.faxAccess().done(
         function (resp) {

             Batch_Fax_QuickLink.access = resp.status;
             Batch_Fax_QuickLink.ShowFaxCount();
         });

	initSession();
}

/******* This function is responsible for making hash section of to be used in the application for specific module loading ***********/
function RefreshWindowOnEntityChange() {
    if (localStorage.GlobalEntityIdForMultiWindow != globalAppdata.SeletedEntityId) {
        localStorage.setItem("GlobalEntityIdForMultiWindow", globalAppdata.SeletedEntityId);
        location.reload();
    }
}

function makeHashSection() {

    var hashSection = "";
    if (globalAppdata["PreferredScreenName"] != null && globalAppdata["PreferredScreenName"] != "") {
        switch (globalAppdata["PreferredScreenName"]) {

            case "Patient":
                hashSection = "Patient";
                break;
            case "Billing":
                hashSection = "Billing";
                break;
            case "Admin":
                hashSection = "Admin";
                break;
            case "Batch":
                hashSection = "Batch";
                break;
            case "Clinical":
                hashSection = "Clinical";
                break;
            case "Scheduler":
                hashSection = "Schedule";
                if (globalAppdata["PreferredSchScreenName"] != null && globalAppdata["PreferredSchScreenName"] != "") {
                    switch (globalAppdata["PreferredSchScreenName"]) {
                        case 'Wait List':
                            hashSection += "/WaitList";
                            break;
                        case 'Scheduler Search':
                            hashSection += "/Search";
                            break;
                        case 'Schedule Group':

                            hashSection += "/MultipleView";
                            break;
                        default:
                            break;
                            //  SelectTab('schTabCalendar', 'false');
                            //  hashSection += "/MultipleView"; break;
                    }
                }
                break;
            case "AuditbleEvents":
                hashSection = "AuditbleEvents";
                break;
            default:
                if (globalAppdata["PreferredScreenName"].toLowerCase().indexOf('reports') > -1) {
                    hashSection = "Reports";
                } else {
                    //
                }
                break;
        }
    }
    if (window.location.hash == "") {
        window.location.hash = hashSection;
    }
}

function LoadHomeTab() {

    makeHashSection();


    var hashSection = window.location.hash;
    var tabName = null;
    var subModuleName = null;
    var PatientId = localStorage.getItem("SelectedPatientId");

    if (hashSection && hashSection != "" && hashSection.length > 1) {
        hashSection = hashSection.split("#")[1];

        tabName = "mstrTab" + hashSection.split("/")[0];
        subModuleName = hashSection.split("/")[1];

    }
    else {
        tabName = "mstrTabReports";
    }

    AppCommands.Load();
    var homeTab = GetTab('mstrTabDashBoard');
    
        var ajax_get = $.get(homeTab.Path, { cache: false }, function (content) {
            html = content;
            if (homeTab.Container) {
                $("#" + homeTab.Container).append(content);
                LoadCommands().done(function () { 
                //if (tabName == "mstrTabDashBoard") {
                //    LoadControl(GetTab("mstrTabDashBoard"), null);
                //}

                if (globalAppdata.AppUserName.toLowerCase() == 'mdvision') {
                    SelectTab('mstrTabAdmin', 'false');
                }
                else {
                    
                    if (PatientId != null && PatientId != "") {
                        $.when(setPatientBanner(PatientId, true)).then(function () {
                            //set Immunization Alert 
                            //Start 7/5/17
                            // M Ahmad Imran

                            //var responseGetFormHaveRights = "";
                            //$.when(responseGetFormHaveRights = utility.GetFormHaveRights("Medical_Immunization")).then(function () {
                            //    if (responseGetFormHaveRights.response) {
                            //        IsBackgroundLoaderShow = false;
                            //        Immunization_AlertConfiguration.SetImmunizationAlertCount(PatientId, false);
                            //    }
                            //});

                            //set Immunization Alert 
                            //End 7/5/17
                            // M Ahmad Imran
                                $.when(SelectTab('mstrTabDashBoard', 'false')).then(function () {
                                    setTimeout(function () {
                                        if (tabName == "mstrTabSchedule" && subModuleName != null && subModuleName != "") {

                                            $.when(SelectTab(tabName, 'false')).then(function () {
                                                SelectTab("schTab" + subModuleName, 'false');
                                            });


                                        } else if (tabName != "mstrTabDashBoard" && subModuleName != null && subModuleName != "") {
                                            $.when(SelectTab(tabName, 'false')).then(function () {
                                                SelectTab(subModuleName, 'false');
                                            });
                                        } else {
                                            SelectTab(tabName, 'false');
                                        }
                                    }, 500);
                                });
                        });

                    }
                    else {
                        setTimeout(function () {

                            if (tabName == "mstrTabSchedule" && subModuleName != null && subModuleName != "") {

                                $.when(SelectTab(tabName, 'false')).then(function () {
                                    SelectTab("schTab" + subModuleName, 'false');
                                });


                            } else if (tabName != "mstrTabDashBoard" && subModuleName != null && subModuleName != "") {
                                $.when(SelectTab(tabName, 'false')).then(function () {
                                    SelectTab(subModuleName, 'false');
                                });
                            } else {
                                SelectTab(tabName, 'false');
                            }
                        }, 500);
                    }
                }
                });
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
            if (tab.TabID != 'mstrTabDashBoard' && (tab.TabID == "mstrTabReports"))
                return LoadHTMLControl(tab);
        }

    });
    var dfd = new $.Deferred();

    $.when.apply(null, defferedarray).then(function () {
        //$(document).loadDropDowns(false);
        MDVisionService.reloadLookups = false;
        BackgroundLoaderShow(false);
        dfd.resolve(true);
    });

    return dfd.promise();

}

function LoadHTMLControl(tab) {

        var ajax_get = $.get(tab.Path, {
            cache: false
        }, function (content) {
            html = content;
          
            if (tab.Container) {
                $("#" + tab.Container).append(content);
            }

        }, "html");

        return ajax_get.promise();

}

function SelectTab(tabId, IsMenu, Reload, UniqueId, TabTitle, event) {
    // if User Come from Patient Demographic/Insurance Tab
    if (params.DemographicAutoUpdateActiveTab) {
        GetAutoUpdateDemographicScreen(tabId, IsMenu, Reload, UniqueId, TabTitle, event);
    }
    else {    // all tabs other than Patient Demographic/Insurance
        SelectApplicationTab(tabId, IsMenu, Reload, UniqueId, TabTitle, event);
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
    if (!(tabId.trim() == ("clinicaltab" + (tabId.replace(/clinicaltab/gi, "").trim())).trim() || tabId == "clinicalTab" + tabId.replace(/clinicalTab/gi, ""))) {
        deleteNotesGlobalParams();
    }

    SelectCurrentTab(tabId, IsMenu, Reload);
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

function deleteNotesGlobalParams() {
    delete params["PatientId"];;
    delete params["ParentCtrl"];
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

var isInTimeOut = 0;
function SelectCurrentTab(tabID, IsMenu, Reload) {
    var Tab = GetTab(tabID);


        $("ul li[id*=mstrMenu]").show();
        $("#mstrMenuClinical").show();
        $("#mstrMenuClinical > a > Span").text('Clinical');
        $("#ClinicalUL").hide();
        //$("#ctrlPanClinical").html("")
        $("#anchorMainMenu").hide();
        $("#mstrDivFaceSheet, #mstrDivMedical, #mstrDivHistroy, #mstrDivNotes, #mstrDivOrders, #mstrDivSpecialities, #mstrDivTemplateBuilder, #mstrDivMiscellaneous, #mstrDivWomenHealth").hide();


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


            if ((!$('#PatientProfile').css('display') || $('#PatientProfile').css('display') == 'none') && PatientArray.length > 0) {
                $('#PatientProfile').show('blind', 200);
            }

                LoadControl(Tab, params);

    }
    else
        LoadControl(Tab, param);

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
    else if (SelectedmenuObj.length > 0 && Tab.ParentTabID != "" && IsMenu == 'true') {
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

    if (Ctrl == "Document_Scan" || Ctrl == "uploadImage")
        reSetDefaultValuesForScanCanvas();
}

function setPatientBanner(PatientID, isOpenNotes, CCMterminationReason) {
    // temp change to get previous version

    var dfd = new $.Deferred();
    Patient_Demographic.FillDemographic(PatientID).done(function (response) {
        if (response.status != false) {

            try {
                $("#mainForm  li#CDSAlert").show();
                $.when(ClinicalCDSDetail.showCDSAlert("", PatientID)).then(function () {
                    dfd.resolve();
                });
            }
            catch (err) {
                dfd.resolve();
            }

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
                if (demographic_detail.Sex == "Female")
                    $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_female_profile.gif");
                else if (demographic_detail.Sex == "Male" || demographic_detail.Sex == "Other")
                    $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_male_profile.gif");
                else 
                    $("#PatientProfile #imgPatientProfile").attr("src", "Content/images/default_male_profile.gif");
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

    $('#btnDemographicLabel').show();
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
        else {
            SelectCurrentTab('mstrTabPatient', 'true');
        }

     
        if (CurrentSelectedTab.MasterTabID == 'mstrTabReports')
            SelectCurrentTab(CurrentSelectedTab.MasterTabID, 'true');

        dfd.resolve('ok');


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
    Clinical_ProgressNote.disconnectUserFromSignalR();
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

function CheckDemographicDetailOnRedirect(SelectedPage, event) {
    if (params.DemographicAutoUpdateActiveTab) {
        if (params.defaultDemographicSerailizeForm) {
            var formId = "";
            if (params.DemographicAutoUpdateActiveTab == "Demographic") {
                if ($("#pnlDemographic #frmDemographic").length > 0) {
                    formId = "#pnlDemographic #frmDemographic";
                }
            }
            else if (params.DemographicAutoUpdateActiveTab == "Insurance") {
                if ($("#pnlPatientInsurance #frmPatientInsurance").length > 0) {
                    formId = "#pnlPatientInsurance #frmPatientInsurance";
                }
            }
            var formCtr = $(formId);
            if (formCtr.serialize() != params.defaultDemographicSerailizeForm) {
                if (!params.IsDemographicInfoGlobalyUpdated) {
                    event.preventDefault();
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
                        $.when(drfdAutoSave).then(function () {
                            window.location = SelectedPage;
                        });

                    }, function () {
                        window.location = SelectedPage;
                    },
                  'Confirm Change');
                }
            }
        }
    }
}

