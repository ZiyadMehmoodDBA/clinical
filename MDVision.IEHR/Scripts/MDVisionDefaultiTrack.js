var DefaultMenuSelected = "MDVisioniTrack";

function LoadApplication() {
    //fix PMS-2706
    if (localStorage.SelectPatientEntityId != globalAppdata.SeletedEntityId) {
        localStorage.removeItem('SelectedPatientId');
        localStorage.removeItem('SelectPatientEntityId');
    }
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
    DashBoard.MessageCount();
    Batch_Fax_QuickLink.faxAccess().done(
         function (resp) {

             Batch_Fax_QuickLink.access = resp.status;
             Batch_Fax_QuickLink.ShowFaxCount();
         });
		 
	initSession();
}

/******* This function is responsible for making hash section of to be used in the application for specific module loading ***********/

function LoadHomeTab() {

    var PatientId = localStorage.getItem("SelectedPatientId");
    var hashSection = window.location.hash;

    tabName = "mstrTabiTrack";
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
                    setTimeout(function () {
                        SelectTab(innerScreen, 'false');
                    }, 300);
                });
            } else {
                $.when(SelectTab(tabName, 'false')).then(function () {
                    var iTrackSelectedPage = localStorage.getItem("iTrackSelectedPage");
                    if (iTrackSelectedPage && iTrackSelectedPage != "") {
                        setTimeout(function () {
                            SelectTab(iTrackSelectedPage, 'false');
                        }, 300);
                    }
                });
            }
        }
    }, "html");

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
            if (tab.TabID != 'mstrTabDashBoard' && (tab.TabID == "schTabSearch"))
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
    //if (params.DemographicAutoUpdateActiveTab) {
    //    GetAutoUpdateDemographicScreen(tabId, IsMenu, Reload, UniqueId, TabTitle, event);
    //}
    //else {    // all tabs other than Patient Demographic/Insurance
    SelectApplicationTab(tabId, IsMenu, Reload, UniqueId, TabTitle, event);
    //}

    $('.content-body').removeClass('pb-none');
    $('.content-body').removeClass('pr-none');
    $('#btnDemographicLabel').show();

    var hashSection = window.location.hash;
    if (tabId != "mstrTabiTrack") {
        localStorage.setItem("iTrackSelectedPage", tabId);
        if (hashSection != "#iTrack/" + tabId) {
            window.location.hash = "#iTrack/" + tabId;
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
    if (UniqueId && tabId.indexOf('Encounter') > -1) {
        params['VisitId'] = UniqueId;
    }
    var PatientId = localStorage.getItem("SelectedPatientId");
    if (tabId == "iTrack" + tabId.replace(/iTrack/gi, "")) {
        if (tabId == 'mstrTabiTrack') {
            SelectCurrentTab(tabId, IsMenu, Reload);
        }

        else {
            LoadiTrackTab(tabId, IsMenu);

        }
        return;
    }
    else
        SelectCurrentTab(tabId, IsMenu, Reload);

}

function LoadiTrackTab(tabId, IsMenu) {
    var menuTab = tabId.replace(/Tab/gi, "Menu");
    var SelectedmenuObj = $(document.getElementById(menuTab));
    var menuTitle = $(SelectedmenuObj).find("a").attr('title');
    var ElementName = tabId.replace(/iTrackTab/gi, "");

    AddiTrackTab(ElementName, IsMenu);
    //var Tab = GetTab(tabId);
    var nodes = document.getElementById("mstrDiviTrack").getElementsByTagName('*');
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
        if (iTrackTabs == null) {
            $("#" + "mstrDiviTrack").append(Element);
            iTrackTabs = $("#" + "mstrDiviTrack").scrollTabs();
        }
        else {
            if (Element)
                iTrackTabs.addTab($(Element)[0].outerHTML, $("#mstrDiviTrack .btn").length);
        }
        $("#mstrDiviTrack .btn").removeClass('tab_selected');
        $($("#mstrDiviTrack .btn")[$("#mstrDiviTrack .btn").length - 1]).addClass('tab_selected');
    }
}

function SelectCurrentTab(tabID, IsMenu, Reload) {
    var Tab = GetTab(tabID);

    $("ul li[id*=mstrMenu]").show();
    $("#mstrMenuClinical").show();
    $("#mstrMenuClinical > a > Span").text('Clinical');
    $("#ClinicalUL").hide();
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

    SelectedMenuAndTab(Tab, IsMenu);
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
    }, 500);

    Tab["Selected"] = true;
    Tab["Active"] = true;

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

    if (Tab != null && Tab.TabID == "mstrTabiTrack" && Tab.MasterTabID == "") {
        iTrackFirstLoad = false;
        SelectTab('iTrackTabDashboard', 'false');
    }
    if (Tab.TabID != 'mstrTabReports') {
        setTimeout(function () {
            if ($(SelectedpnlObj).attr('id') != null && $(SelectedpnlObj).attr('id').toLowerCase().indexOf("paymentposting") < 0)
                SelectedpnlObj.find("form :input:not(button):not(hidden):not([data-plugin-timepicker]):not([data-plugin-datepicker]):not([data-plugin-keyboard-numpad]):not([id*='date']):not([id*='Date']):not([id*='dtp']):enabled:visible:first").focus();
        }, 400);
    }

    if (Tab != null && Tab.MasterTabID != null && Tab.MasterTabID != "" && Tab.MasterTabID != 'mstrTabClinical') {
        setParentChildMenuId("mainUL");
    }
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

function UnloadTabParams(MasterTabID) {
    var TabsPatientList = jQuery.grep(TabsArray, function (obj) {
        return obj.MasterTabID === MasterTabID;
    });
    $.each(TabsPatientList, function (i, item) {
        try {
            if (item.ContainerControlID != "" && item.ContainerControlID != null && typeof item.ContainerControlID != "undefined" && eval(item.ContainerControlID) != null) {
                eval(item.ContainerControlID).bIsFirstLoad = true;
                eval(item.ContainerControlID).params = [];
            }
        } catch (ex) {
            console.log(ex);
        }

    });
}


function AddiTrackTab(ElementName, IsMenu) {
    var cmd = [];
    cmd.TabID = "iTrackTab" + ElementName;
    cmd.PanelID = "pnliTrack" + ElementName;//panal id from opening html control
    cmd.MasterTabID = "mstrTabiTrack";
    cmd.ParentTabID = "mstrTabiTrack";
    cmd.ParentChildTabID = "mstrMenuiTrack";
    cmd.ContainerControlID = "iTrack_" + ElementName;
    cmd.Selected = true;
    cmd.Container = "ctrlPaniTrack";// in which container it will be open
    cmd.ActionPanContainer = "actionPaniTrack" + ElementName;//action pan of opening panal
 
    if (ElementName == "MIPSummary") {
        cmd.Path = "./EMR/HTML/iTrack/iTrack_MIPSummary.html";
    }
    else if (ElementName == "Dashboard") {
        cmd.Path = "./EMR/HTML/iTrack/iTrack_Dashboard.html";
    }
    else if (ElementName == "Quality") {
        cmd.Path = "./EMR/HTML/iTrack/iTrack_Quality.html";
    } else if (ElementName == "PromotingInteroperability") {
        cmd.Path = "./EMR/HTML/iTrack/iTrack_PromotingInteroperability.html";
    }
    else if (ElementName == "ImprovementActivities") {
        cmd.Path = "./EMR/HTML/iTrack/iTrack_ImprovementActivities.html";
    }
    else if (ElementName == "eCQMs") {
        cmd.Path = "./EMR/HTML/iTrack/iTrack_eCQMs.html";
    } else if (ElementName == "Cost") {
        cmd.Path = "./EMR/HTML/iTrack/iTrack_Cost.html";
    } else if (ElementName == "Submission") {
        cmd.Path = "./EMR/HTML/iTrack/iTrack_Submission.html";
    }
    else if (ElementName == "MUReport") {
        cmd.Path = "./EMR/HTML/iTrack/iTrack_MUReport.html";
        cmd.TabID = "iTrackTab_MUReport";
        cmd.PanelID = "pnliTrack_MUReport";//panal id from opening html control
        cmd.MasterTabID = "mstrTabiTrack";
        cmd.ParentTabID = "mstrTabiTrack";
        cmd.ParentChildTabID = "mstrMenuiTrack";
        cmd.ContainerControlID = "iTrack_MUReport";
        cmd.Selected = true;
        cmd.Container = "ctrlPaniTrack";// in which container it will be open
        cmd.ActionPanContainer = "actionPaniTrack_MUReport";//action pan of opening panal
    }
    var Tab = GetTab(cmd.TabID);
    var iTrackTab = cmd;
    if (typeof Tab != "undefined") {
        SelectCurrentTab(iTrackTab.TabID, IsMenu);
        //AddAndSelectTab(iTrackTab, IsMenu);
    }
    else {
        AddAndSelectTab(iTrackTab, IsMenu);
    }
}


// Patient Select , Get and Close Function
function SelectPatient(patientID, FormName) {
    patientID = String(patientID);
    ClosePatient(patientID, true);

    var dfd = new $.Deferred();
    if (IsPatientExist(patientID) == true) {
        if (GetSelectedPatientID() != patientID) {
            // params: [];
            params['patientID'] = patientID;
            params['mode'] = "Edit";
            params['FromAdmin'] = "0";
            params['PanelID'] = "pnlDemographic";
            params['SelectedPatientFormName'] = FormName;
            if ($('#PatientProfile').css('display') == 'none') {
                $('#PatientProfile').show('blind', 200);
            }
            SetSelectedPatient(patientID);
        }
        dfd.resolve('ok');
    }
    else {
        if (patientID != "-1")
            AddPatientArray(patientID);

        SetSelectedPatient(patientID);
        params['patientID'] = patientID;
        params['mode'] = "Edit";
        params['FromAdmin'] = "0";
        params['PanelID'] = "pnlDemographic";
        params['SelectedPatientFormName'] = FormName;

        $('#PatientProfile').css('display', 'none');
        if ($('#PatientProfile').css('display') == 'none') {
            $('#PatientProfile').show('blind', 200);
        }
        //Start//Abid Ali// patient banner load on Out of office visit selection
        $.when(setPatientBanner(patientID)).then(function () {
            //******************************
            params['mode'] = "Edit";
            //******************************
            if (localStorage.getItem("iTrackSelectedPage") != null && localStorage.getItem("iTrackSelectedPage") == "billTabOutOfOfficeVisits") {
                LoadiTrackTab('billTabOutOfOfficeVisits', true);
                $('#billMenuOutOfOfficeVisits a:first').trigger("click");
                $('#billMenuOutOfOfficeVisits').addClass('nav-active');
            }
        });
        dfd.resolve('ok');
    }
    setTimeout(function () {
        $("#frmDemographic :input:not(button):not(:hidden):enabled:visible:first").focus();
    }, 400);
    return dfd.promise();
}


function setPatientBanner(PatientID, isOpenNotes, CCMterminationReason) {
    // temp change to get previous version

    var dfd = new $.Deferred();
    Patient_Demographic.FillDemographic(PatientID).done(function (response) {
        if (response.status != false) {
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
            localStorage.setItem("SelectedPatientId", PatientID);
            localStorage.setItem("SelectedAccountNumber", demographic_detail.AccountNo);
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
            dfd.resolve();
        }
        else {
            utility.DisplayMessages(response.Message, 3);
            dfd.resolve();
        }
    });
    return dfd;
}


function ClosePatient(patientID, bit) {
    $("#btnPatientAlerts").css("display", "none");
    var selectedPatientID = GetSelectedPatientID();

    if (typeof selectedPatientID == 'undefined')
        return

    if (selectedPatientID == '')
        return

    for (var i = 0; i < PatientArray.length; i++) {
        if (PatientArray[i].PatientID == selectedPatientID) {
            PatientArray.splice(i, 1);
        }
    }
    store.saveSession('selectedPatientArray', PatientArray);
    store.clearAllBySetName(selectedPatientID);

    if (PatientArray.length <= 0) {
        //unload the html
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
        //$('#mstrDivPatient [id*="EncounterVisit_Detail"]').each(function () {
        //    RemoveEncounterTab($(this).attr("id"));
        //    $(this).remove();
        //});
        //$('[id*="pnlEncounterVisit_Detail"]').remove();//.resetAllControls();
        //$('[id*="pnlEncounterChargeCapture"]').remove();//.find('#frmEncounterChargeCapture').resetAllControls("#dgvICD,#dgvCPT,#dgvModifier,#dgvVisitCharge");
        //$("#pnlDemographic #divDemographicPicture #imgPatient").attr("src", "Content/images/default_male_profile.gif");
        //$('#pnlPatientInsurance #frmPatientInsurance #scanImage #image').attr("src", "Content/images/idcard1.png")

        //this code will get all the  jquery classes
        //var TabsPatientList = UnloadTabParams("mstrTabPatient");

        if (typeof params != 'undefined' && params != null) {
            params = [];
            params['patientID'] = "-1";
            params["FromAdmin"] = "1";
            params["PreviousTab"] = GetSelectedTab();
        }
        //if (typeof bit == 'undefined') {
        //    var tab = GetCurrentSelectedTab();
        //    var CurrentTabID = "mstrTabDashBoard";
        //    if (tab.TabID != 'patTabDemographic' && tab.ParentTabID != "mstrTabPatient") {
        //        CurrentTabID = tab.TabID;
        //    }
        //    SelectCurrentTab(CurrentTabID, 'false');
        //}
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

    //var TabsPatientList = UnloadTabParams("mstrTabPatient");

    if (typeof params != 'undefined' && params != null) {
        params = [];
        params['patientID'] = "-1";
        params["FromAdmin"] = "1";
    }
    //if (typeof bit == 'undefined') {
    //    var tab = GetCurrentSelectedTab();

    //    var CurrentTabID = "mstrTabDashBoard";
    //    if (tab.TabID != 'patTabDemographic' && tab.TabID != "mstrTabClinical" && tab.ParentTabID != "mstrTabPatient" && tab.ParentTabID != "mstrTabClinical") {
    //        CurrentTabID = tab.TabID;
    //    }
    //    SelectCurrentTab('mstrTabDashBoard', 'false');
    //}
    var PatientTabs = new Array();
    TabsArray.filter(function (obj) {
        //if (obj.ParentTabID == "mstrTabPatient")
        //    PatientTabs.push(obj)
        //else if (obj.MasterTabID == "mstrTabClinical")
        //    PatientTabs.push(obj)
        //else
        if (obj.MasterTabID == "mstrTabiTrack" && obj.ContainerControlID == "OutOfOfficeVisits")
            PatientTabs.push(obj)
    });
    $.each(PatientTabs, function (i, tab) {
        //if (tab.Container == "ctrlPanClinical") {
        //    $("#" + tab.Container).html("");
        //    UnloadTabParams('mstrTabClinical');
        //    $("#hfClinicalMenuParentLiId").val('');
        //    $("#hfClinicalMenuChildLiId").val('');
        //}
        if (tab.Container == "ctrlPaniTrack") {
            //SelectTab('mstrTabDashBoard', 'false');
            if ($("#pnlBillOutOfOfficeVisits").length > 0) {
                $("div[id*=pnlBillOutOfOfficeVisits]").remove();
            }
        }
        RemoveTab(tab);
    });
    $('#mstrDivPatient').empty();
}


function MainClosePatient() {
    // if user is on demographic/Insurance tab 
    //if (params.DemographicAutoUpdateActiveTab) {
    //    var formId = "#pnlDemographic #frmDemographic";
    //    if (params.DemographicAutoUpdateActiveTab == "Insurance")
    //        formId = "#pnlPatientInsurance #frmPatientInsurance";
    //    var formCtr = $(formId);
    //    if (formCtr.serialize() != params.defaultDemographicSerailizeForm && params.IsDemographicInfoGlobalyUpdated == false) {

    //        var ScreenName = params.DemographicAutoUpdateActiveTab;
    //        if (params.DemographicAutoUpdateActiveTab == "Insurance")
    //            ScreenName = "Patient Insurances";
    //        utility.myConfirm('Are you sure you want to save ' + ScreenName + ' change?', function () {
    //            params.IsDemographicInfoGlobalyUpdated = true;
    //            delete params.DemographicAutoUpdateActiveTab;
    //            if (formId == "#pnlPatientInsurance #frmPatientInsurance")
    //                $("#pnlPatientInsurance #frmPatientInsurance").trigger("submit");
    //            else
    //                $("#pnlDemographic #frmDemographic").trigger("submit");

    //            // confirm data post request is successfully updated
    //            if (params.IsDemographicInfoGlobalyUpdated)
    //            { ApplicationPatientClose(); }

    //        }, function () {  // reset active tab screen to its default state
    //            if (params.DemographicAutoUpdateActiveTab == "Demographic")
    //                Patient_Demographic.LoadPatientDemogrphic();
    //            else Patient_Insurance.LoadInsuranceList();
    //            //  reset flag to its default value 
    //            if (params.IsDemographicInfoGlobalyUpdated)
    //            { params.IsDemographicInfoGlobalyUpdated = false; }

    //            ApplicationPatientClose();

    //        },
    //                       'Confirm Change'
    //                   );
    //    }
    //    else {
    //        ApplicationPatientClose();
    //    }
    //}
    //else {
    ApplicationPatientClose();
    //}
}

function ApplicationPatientClose() {
    var SelectedTabID = selectedtab.TabID.toLowerCase();

    ClosePatientNew();


};

