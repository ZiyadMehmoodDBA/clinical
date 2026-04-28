ClinicalMenuSettings = {
    bIsFirstLoad: true,
    params: [],
    menuArray: [],
    Load: function (params) {
        ClinicalMenuSettings.params = params;
        ClinicalMenuSettings.menuArray = [];
        ClinicalMenuSettings.ClinicalMenuSettingsSearch();

    },

    ClinicalMenuSettingsSearch: function (moduleId, tabId) {
        var strMessage = "";

        var self = $("#pnlClinicalClinicalMenuSettings");
        var myJSON = self.getMyJSON();
        var objDeffered = $.Deferred();
        return ClinicalMenuSettings.SearchClinicalMenuSettings(myJSON, moduleId).done(function (response) {


            var screeningMenu = "<li class='nav-parent' id='clinicalMenuScreening' style='display:block;'><a data-toggle='tab' title='Screening' href='#mstrTabScreening' onclick=ClinicalMenuClick(event,function(){ClinicalMenuSettings.TopButtons('clinicalMenuScreening');ClinicalMenuSettings.selectClinicalMenu('clinicalMenuScreening');},1,this,'clinicalMenu_Screening_Depression','li');><i class='fa fa-list-ol' aria-hidden='true'></i><span>Screenings</span></a><ul id='sortableScreening' class='nav nav-children'><li class='ui-state-default' id='clinicalMenu_Screening_Depression'><a href='#mstrTabScreening' onclick=javascript:ClinicalMenuClick(event,function(){javascript:SelectTab('clinicalTabDepression','true');},0,null,'clinicalMenu_Screening_Depression','li'); title='Depression'>Depression</a></li> <li class='ui-state-default' id='clinicalMenu_Screening_Tobacco'><a href='#mstrTabScreening' onclick=javascript:ClinicalMenuClick(event,function(){javascript:SelectTab('clinicalTabTobacco','true');},0,null,'clinicalMenu_Screening_Tobacco','li'); title='Tobacco'>Tobacco</a></li></ul></li>";
           // var screeningMenu = "<li class='nav-parent' id='clinicalMenuScreening' style='display:block;'><a data-toggle='tab' title='Screening' href='#mstrTabScreening' onclick=ClinicalMenuClick(event,function(){ClinicalMenuSettings.TopButtons('clinicalMenuScreening');ClinicalMenuSettings.selectClinicalMenu('clinicalMenuScreening');},1,this,'VBP_MissingDataAlert','li')><i class='fa fa-list-ol' aria-hidden='true'></i><span>Screenings</span></a><ul id='sortableScreening' class='nav nav-children'><li class='ui-state-default' id='clinicalMenu_Screening_Depression'><a href='#mstrTabScreening' onclick=javascript:ClinicalMenuClick(event,function(){javascript:SelectTab('clinicalTabDepression','true');},0,null,'clinicalMenu_Screening_Depression','li'); title='Depression'>Depression</a></li></ul></li>";
            if (response.status != false && response.html != '') {
                if (!$("#clinicalMenuTemplateBuilder").hasClass("nav-parent active nav-expanded")) {
                    if (response.MenuSettingsCount > 0) {
                        var ClinicalMenuSettingsLoad_JSON = JSON.parse(response.ClinicalMenuSettingsLoad_JSON);
                        var ClinicalMenuHTML = response.html;
                        ClinicalMenuHTML = ClinicalMenuHTML.replace(/Face Sheet/g, 'Clinical Summary');
                        ClinicalMenuHTML = ClinicalMenuHTML.replace(/&quot;/g, '"');
                        ClinicalMenuHTML = ClinicalMenuHTML.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                        ClinicalMenuHTML = ClinicalMenuHTML.replace(/&#39;/g, "'");
                        ClinicalMenuHTML = ClinicalMenuHTML.replace('[[and]]', "&");
                        ClinicalMenuHTML = ClinicalMenuHTML.replace(/href="javascript:SelectTab/g, "href='javascript:SelectTab").replace(/;"="">/g, ";'>");
                        var desfaulHTML = $("#ClinicalUL").html();
                        desfaulHTML = desfaulHTML.replace(/Face Sheet/g, 'Clinical Summary');

                        var SelectedMenuId = null;
                        if ($("#ClinicalUL").find(".nav-active").length > 0 && $("#ClinicalUL").find(".nav-active").hasClass("nav-expanded")) {
                            SelectedMenuId = $("#ClinicalUL").find(".nav-active").attr("id");
                        }
                        if (ClinicalMenuSettingsLoad_JSON != null && ClinicalMenuSettingsLoad_JSON.length > 0 && ClinicalMenuSettingsLoad_JSON[0].IsChanged == "True") {
                            $("#ClinicalUL").html(ClinicalMenuHTML);
                            $("#ClinicalUL").show();
                            ClinicalMenuSettings.LoadMenu();
                            $("ul[id*=sortable]").each(function () {
                                ClinicalMenuSettings.menuArray[this.id] = $(this).sortable('toArray');
                            });
                            ClinicalMenuSettings.loadDefaultMenu(desfaulHTML);
                            ClinicalMenuSettings.LoadMenuwithMU3Privilages();
                            ClinicalMenuSettings.reSortMenu(desfaulHTML, ClinicalMenuHTML);
                        } else {
                            $("#ClinicalUL").html(ClinicalMenuHTML);
                            $("#ClinicalUL").show();
                            ClinicalMenuSettings.LoadMenu();
                            ClinicalMenuSettings.LoadMenuwithMU3Privilages();
                            var clinicalMenuHtml = $("#ClinicalUL").html();
                            ClinicalMenuSettings.UpdateClinicalMenuSettings(null, clinicalMenuHtml, null);
                        }

                        if (SelectedMenuId) {
                            $("#ClinicalUL").children("li").each(function () {
                                $(this).hasClass("nav-expanded") ? $(this).removeClass("nav-expanded") : "";
                                $(this).hasClass("nav-active") ? $(this).removeClass("nav-active") : "";
                                $(this).attr("display") == "block" ? $(this).attr("display", "none") : "";
                            });
                            $("#" + SelectedMenuId).addClass("nav-active nav-expanded");
                            $("#" + SelectedMenuId).attr("display", "block");
                        }
                        if ($("#ClinicalUL").find("#clinicalMenuScreening").length) {
                            $("#ClinicalUL").find("#clinicalMenuScreening").remove();
                        }
                        $(screeningMenu).insertBefore("#ClinicalUL #clinicalMenuNotes");
                        objDeffered.resolve();
                    }
                }
                else {
                    $("#ClinicalUL").html($("#ClinicalUL").html());
                    $("#ClinicalUL").show();
                    if ($("#ClinicalUL").find("#clinicalMenuScreening").length) {
                        $("#ClinicalUL").find("#clinicalMenuScreening").remove();
                    }
                    $(screeningMenu).insertBefore("#ClinicalUL #clinicalMenuNotes");
                    objDeffered.resolve()
                    ClinicalMenuSettings.LoadMenu();
                }
            }
            else {
                //Begin 29-12-15 Muhammad Arshad Remove hardcoding of clinical menu
                ClinicalMenuSettings.loadDefaultMenu();
                ClinicalMenuSettings.LoadMenuwithMU3Privilages();
                var clinicalMenuHtml = $("#ClinicalUL").html();
                ClinicalMenuSettings.UpdateClinicalMenuSettings(null, clinicalMenuHtml, null);
                if ($("#ClinicalUL").find("#clinicalMenuScreening").length) {
                    $("#ClinicalUL").find("#clinicalMenuScreening").remove();
                }
             
                $(screeningMenu).insertBefore("#ClinicalUL #clinicalMenuNotes");
                objDeffered.resolve()
                //}
            }
            return objDeffered.promise();
            
        });
    },
    /*
        Author: Muhammad Azhar Shahzad
        Created on: March 30,2016
        purpose: Load menu against rights and sorted order saved for that user
    */
    reSortMenu: function (desfaulHTML, ClinicalMenuHTML) {
        var updatedHTML = $('#ClinicalUL').html();
        $("#ClinicalUL ul[id*=sortable]").each(function () {
            var menuOrdering = ClinicalMenuSettings.menuArray[this.id];
            // Get your list items
            var items = $(this).find('li');
            // Map the existing items to their new positions            
            var remainingLI = "";
            var orderedItems = '';
            if (menuOrdering) {
                menuOrdering = jQuery.unique(menuOrdering);
                if (menuOrdering != null && menuOrdering.length > 0) {
                    $.each(menuOrdering, function (index, item) {
                        var objIndex = null;
                        var existingItem = $.each(items, function (ind, cntrl) {
                            if (item == $(cntrl).attr('id')) {
                                objIndex = ind; existingItem = cntrl;
                            }
                        });
                        if (existingItem != null && existingItem != '' && items != null && objIndex != null) {
                            orderedItems += existingItem[objIndex].outerHTML;
                            items.splice(objIndex, 1);
                        }
                    });

                    // check the ordered list items has the item remaining or not
                    $.each(items, function (index, element) {
                        var alreadyExists = $.grep($(orderedItems), function (cntrl, index) {
                            return ($(cntrl).attr('id') == element.id);
                        });
                        if (alreadyExists == null || alreadyExists.length == 0) {
                            remainingLI += element.outerHTML;
                        }
                    });
                    // Clear the old list items and insert the newly ordered ones
                    $(this).empty().html(orderedItems + remainingLI);
                }
            }
        });
        ClinicalMenuSettings.LoadMenu();
        var clinicalMenuHtml = $("#ClinicalUL").html();
        ClinicalMenuSettings.UpdateClinicalMenuSettings(null, clinicalMenuHtml, null);
    },
    /*
        Author: Muhammad Azhar Shahzad
        Created on: March 30,2016
        purpose: Load menu against rights
    */
    loadDefaultMenu: function (desfaulHTML) {
        desfaulHTML = desfaulHTML == null ? $('#ClinicalUL').html() : desfaulHTML; //'<ul class="nav nav-main clinicalMenu" id="ClinicalUL">'
        //End 29-12-15 Muhammad Arshad Remove hardcoding of clinical menu
        $("#ClinicalUL").html(desfaulHTML);
        $("#ClinicalUL li.nav-parent a").each(function () {
            var ImmediatParent = $(this).parent();
            if (ImmediatParent != null && $(ImmediatParent).hasClass("nav-parent")) {
                var attrOnclick = $(this).attr("onclick");
                var childHref = "";
                if ($(this).parent().attr("id") != "clinicalMenuNotes") {
                    childHref = $(this).parent().find("ul li:first a:first").attr("onclick");
                }
                var currentChildId = $(this).parent().find("ul li:first").attr("id");
                //Start 16-12-2015 Muhammad Arshad if Menu does'nt have child li item, then it's also child menu
                if (!(currentChildId != null)) {
                    currentChildId = $(this).parent().attr("id")
                }
                //Start 16-12-2015 Muhammad Arshad if Menu does'nt have child li item, then it's also child menu
                var childId = $(this).parent().find("ul li:first a:first").parent().attr("id");
                //Start 21-12-2015 Muhammad Arshad Fixing clinical menu issue
                if (currentChildId == "clinicalMenuFaceSheet" || currentChildId == "clinicalMenuNotes") {
                    attrOnclick += ";ClinicalMenuSettings.selectClinicalMenu('" + currentChildId + "');";
                    $(this).attr("onclick", "javascript:ClinicalMenuClick(event,function(){" + attrOnclick + "},0,this,'" + currentChildId + "','li');");
                }
                else {
                    attrOnclick += ";ClinicalMenuSettings.selectClinicalMenu('" + $(this).parent().attr("id") + "');";
                    $(this).attr("onclick", "javascript:ClinicalMenuClick(event,function(){" + attrOnclick + "},1,this,'" + currentChildId + "','li');");
                }
                //End 21-12-2015 Muhammad Arshad Fixing clinical menu issue
            }
        });

        $("#ClinicalUL li.ui-state-default a").each(function () {
            var attrHref = $(this).attr("onclick");
            $(this).attr("onclick", "javascript:ClinicalMenuClick(event,function(){" + attrHref + "},0,null,'" + $(this).parent().attr("id") + "','li');")
        });
        $("#ClinicalUL").show();
        ClinicalMenuSettings.LoadMenu();
    },
    LoadMenuwithMU3Privilages: function () {
        $("#ClinicalUL li.nav-parent a").each(function () {
            var ImmediatParent = $(this).parent();
            var ImmediatParentId = $(ImmediatParent).attr("id");
            if (ImmediatParentId && ImmediatParentId == "clinicalMenu_History_SocPsyandBehaviorHx" && globalAppdata["isMU3SocPsycBehaviourHx"] && globalAppdata["isMU3SocPsycBehaviourHx"].toLowerCase() == "false")
                $("#" + ImmediatParentId).remove();
            else if (ImmediatParentId && ImmediatParentId == "clinicalMenu_Medical_Implantable" && globalAppdata["isImplantableDevices"] && globalAppdata["isImplantableDevices"].toLowerCase() == "false")
                $("#" + ImmediatParentId).remove();
            else if (ImmediatParentId && ImmediatParentId == "clinicalMenu_Medical_Cognitive" && globalAppdata["isConsolidatedCDACreationPreformance"] && globalAppdata["isConsolidatedCDACreationPreformance"].toLowerCase() == "false")
                $("#" + ImmediatParentId).remove();
            else if (ImmediatParentId && ImmediatParentId == "clinicalMenu_Medical_CarePlan" && globalAppdata["isCarePlan"] && globalAppdata["isCarePlan"].toLowerCase() == "false")
                $("#" + ImmediatParentId).remove();
        });
    },
    //Start 21-12-2015 Muhammad Arshad Fixing clinical menu issue
    selectClinicalMenu: function (currentLiId) {
        if (currentLiId != null && currentLiId != "") {
            $("#ClinicalUL li.nav-parent:not(#clinicalMenuScreening),li#clinicalMenuFaceSheet,li#clinicalMenuNotes ").each(function () {
                if ($(this).attr("id") == "clinicalMenuFaceSheet" || $(this).attr("id") == "clinicalMenuNotes") {
                    $(this).removeClass("nav-parent");
                }
                if ($(this).attr("id") != null && $(this).attr("id") != currentLiId) {
                    $(this).removeClass("nav-active");
                    $(this).removeClass("nav-expanded");
                }
                else if ($(this).attr("id") == currentLiId) {
                    $(this).addClass("nav-active");
                    $(this).addClass("nav-expanded");

                }
            });
            if (currentLiId == "clinicalMenuScreening") {
                if ($("#clinicalMenuScreening").hasClass("nav-expanded")) {
                    $("#clinicalMenuScreening").removeClass("nav-expanded");
                    $("#clinicalMenuScreening").removeClass("nav-active");
                }
                else {
                    $("#clinicalMenuScreening").addClass("nav-expanded");
                    $("#clinicalMenuScreening").addClass("nav-active");
                }
            }
        }
        else {
            $("#ClinicalUL li#clinicalMenuFaceSheet,li#clinicalMenuNotes").each(function () {
               
                $(this).removeClass("nav-active");
                $(this).removeClass("nav-expanded");
            });
        }
    },
    //End 21-12-2015 Muhammad Arshad Fixing clinical menu issue  

    createTopButton: function (mstrDivId, TabId, TabText, ActiveClass, FormParentHTMLId) {
       
        if ($("#" + mstrDivId).length == 0) {
            onlyDivId = ''
            if (mstrDivId.indexOf('#') > -1) {
                onlyDivId = mstrDivId.split('#')[1]
            }
            else {
                onlyDivId = mstrDivId;
            }
            $('#pnlTab3 div ').append('<div class="tab-pane" id="' + onlyDivId + '"></div>');

        }

        if (TabText != "Phone Encounter") {

            $("#" + mstrDivId).html('');
        }
        var id = TabId;//"clinicalTabNotes";
        var txt = TabText;//"Notes";
        var Tab = TabId;//"clinicalTabNotes";
        //Start 21-12-2015 Muhammad Arshad Fixing clinical menu issue
        var attrOnClick = "ClinicalMenuSettings.selectClinicalMenu('" + FormParentHTMLId + "');";

        var selectedTabClass = "";
        if (TabText != "Phone Encounter") {
            selectedTabClass = "tab_selected";
        }
        //EMR - 832 fix, need span encloused on button
        var Element = $('<span class="btn btn-default btn-sm ' + selectedTabClass + ' tab_space"><button type="button" class="btn btn-default btn-sm tab_space' + ActiveClass + '" title="' + txt
           + '" id="' + id + '" onclick=SelectTab("' + Tab + '","false");ClinicalMenuClick(event,function(){' + attrOnClick + '},0,null,\'' + id + '\',\'' + 'button' + '\');>' + txt + '</button></span>');

        $("#" + mstrDivId).append(Element);//$("#mstrDivNotes").append(Element);
        $("#" + mstrDivId).css({ display: "block" });;//$("#mstrDivNotes").css({ display: "block" });

        $("#mstrMenuClinical").removeClass('nav-expanded nav-parent active');

        //   $("#mstrMenuClinical").addClass('nav-parent active');
        $("div[id*=mstrDiv]").hide();
        $("#" + mstrDivId).show();
        //SelectTab(Tab, 'false');
        if (TabText != "Phone Encounter") {
            $("#" + id).trigger("click");
        }
        //End 21-12-2015 Muhammad Arshad Fixing clinical menu issue
    },

    TopButtons: function (FormParentHTMLId, paramsNotes, fromDemographics) {

        var objDeffered = $.Deferred();
        var menuTab = FormParentHTMLId.replace(/clinicalMenu/gi, "");

        var MstrDiv = "mstrDivNotes"

        if ($("#" + "mstrDiv" + menuTab).length == 0) {
            $('#pnlTab3 div ').append('<div class="tab-pane" id="mstrDiv' + menuTab + '"></div>');
            MstrDiv = "pnlTab3 #mstrDivNotes";
        }

        $("#" + "mstrDiv" + menuTab).html('');

        //if (menuTab == "Notes" || menuTab == "ProgressNote") {
        //console.log(menuTab);
        if (menuTab.indexOf("Notes") > -1 || menuTab == "ProgressNote" || menuTab.indexOf("FaceSheet") > -1) {
            if (paramsNotes != null) {
                params["AppointmentId"] = paramsNotes["AppointmentId"];
                params["PatientId"] = paramsNotes["PatientId"];
                params["NotesVisitTime"] = paramsNotes["NotesVisitTime"];
                params["NotesVisitId"] = paramsNotes["NotesVisitId"];
                params["NotesVisitDate"] = paramsNotes["NotesVisitDate"];
                params["NotesFacilityId"] = paramsNotes["NotesFacilityId"];
                params["NotesProviderId"] = paramsNotes["NotesProviderId"];
                params["ParentCntrlLoadid"] = "Schedular";
                params["NotesFacilityName"] = paramsNotes["NotesFacilityName"];
                params["NotesProviderName"] = paramsNotes["NotesProviderName"];
                params["ScheduleReason"] = paramsNotes["ScheduleReason"];
                params["menuTab"] = "ProgressNote";
            }
            var isFirstButton = 1;
            if (menuTab.indexOf("Notes") > -1) {

                var ActiveClass = "";
                if (isFirstButton == 1) {
                    ActiveClass = " active";
                    isFirstButton = 0;
                }
                else {
                    ActiveClass = "";
                }
                ClinicalMenuSettings.createTopButton(MstrDiv, "clinicalTabNotes", "Notes", ActiveClass, FormParentHTMLId);

                if (MstrDiv == "mstrDivNotes") {
                    $('#pnlTab3 #mstrDivClinical').hide();
                    $('#pnlTab3 #mstrDivNotes').show();
                }
                AppPrivileges.GetFormPrivileges("Notes_Phone Encounter", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        ClinicalMenuSettings.createTopButton(MstrDiv, "clinicalTabPhoneEncounter", "Phone Encounter", "", FormParentHTMLId);
                    }
                    objDeffered.resolve('done');
                });
            }
            else if (menuTab.indexOf("FaceSheet") > -1) {
                var ActiveClass = "";
                if (isFirstButton == 1) {
                    ActiveClass = " active";
                    isFirstButton = 0;
                }
                else {
                    ActiveClass = "";
                }
                ClinicalMenuSettings.createTopButton("mstrDivFaceSheet", "clinicalTabFaceSheet", "Clinical Summary", ActiveClass, FormParentHTMLId);
                objDeffered.resolve('done');
            }
            //  Clinical_Notes.LoadNotesTab(menuTab, params);
            // SelectCurrentTab('mstrTabClinical', 'true');
            //  SelectCurrentTab('mstrTabClinical', 'true');
            //SelectTab('clinicalTabNotes', 'false');
            //   SelectTab('mstrTabClinical', 'false');

            //   SelectTab('clinicalTabNotes', 'false');
            // }
        } else {
            //$("#ClinicalUL li").each(function () {
            //    $(this).on("click", function () {
            //        ClinicalMenuClick()
            //    });
            //});
            var isFirstButton = 1;
            $("#ClinicalUL #" + FormParentHTMLId + " ul li").each(function (index) {
                var Node = $(this).text().trim();
                var Tab = "clinicalTab" + Node;
                var id = $(this).attr("id");
                var txt = ($(this).find('a').text()).trim();
                if (Tab == "clinicalTabQuestion Group")
                    Tab = "clinicalTabQuestion_Group";
                else if (Tab == "clinicalTabDesign Letter")
                    Tab = "clinicalTabLetter";
                else if (Tab == "clinicalTabRadiology")
                    Tab = "clinicalTabRadiologyOrder";
                else if (Tab == "clinicalTabProcedure")
                    Tab = "clinicalTabProcedureOrder";
                else if (Tab == "clinicalTabConsultation")
                    Tab = "clinicalTabConsultationOrder";
                else if (Tab == "clinicalTabLab")
                    Tab = "clinicalTabLabOrder";
                else if (Tab == "clinicalTabImplantable Devices")
                    Tab = "clinicalTabImplantable";
                else if (Tab == "clinicalTabSocial, Psychological and Behavior Hx") {
                    Tab = "clinicalTabSocPsyandBehaviorHx";
                }
                else if (Tab == "clinicalTabFunctional Cognitive and  Mental Status") {
                    Tab = "clinicalTabCognitive";
                }
                else if (Tab == "clinicalTabDiagnostic Imaging") {
                    Tab = "clinicalTabRadiologyOrder";
                }
                else
                    Tab = Tab.replace(/\s+/g, '');
                ;
                var ActiveClass = "";
                if (isFirstButton == 1) {
                    ActiveClass = " active";
                    isFirstButton = 0;
                }
                else {
                    ActiveClass = "";
                }
                var attrOnClick = "ClinicalMenuSettings.selectClinicalMenu('" + FormParentHTMLId + "');";
                var Element = $('<span class="btn btn-default btn-sm tab_space" id="'+ id + '"><button type="button" title="' + txt
                    + '" id="' + id + '" onclick=SelectTab("' + Tab + '","false");ClinicalMenuClick(event,function(){' + attrOnClick + '},0,null,\'' + id + '\',\'' + 'button' + '\');>' + txt + '</button></span>');
                //To avoid duplication of tab buttons
                if ( id != undefined) {
                    if ($("#mstrDiv" + menuTab + " button#" + id).length < 1) {
                        $("#mstrDiv" + menuTab).append(Element);
                    }
                }

                $("#mstrDiv" + menuTab).css({ display: "block" });
            });
            if (fromDemographics != null) {
                $("#mstrMenuClinical").removeClass('nav-expanded nav-parent active');
                $("div[id*=mstrDiv]").hide();
                $("#" + "mstrDiv" + menuTab).show();
                $("#mstrDiv" + menuTab).scrollTabs();
                $("#mstrDiv" + menuTab).removeClass('tab_selected');
                $("#mstrDiv" + menuTab + " #" + fromDemographics).addClass('tab_selected');
            }
            else {
                //var FirstChildHref = $("#" + FormParentHTMLId + " ul li:first a:first").attr("href");
                //$("#" + FormParentHTMLId + " ul li a:first").trigger('click');
                //$("#clinicalMenu" + menuTab + "ul li:first").addClass('nav-active');
                $("#mstrMenuClinical").removeClass('nav-expanded nav-parent active');
                //   $("#mstrMenuClinical").addClass('nav-parent active');
                $("div[id*=mstrDiv]").hide();
                $("#" + "mstrDiv" + menuTab).show();
                $("#mstrDiv" + menuTab).scrollTabs();
                //$("#" + "mstrDiv" + menuTab + " button:first").addClass('active');
                $("#mstrDiv" + menuTab).removeClass('tab_selected');
                if ($("#hfClinicalMenuChildLiId").val() != "" && $("#mstrDiv" + menuTab + " span button[id*='" + $("#hfClinicalMenuChildLiId").val() + "']").length > 0) {
                    $("#mstrDiv" + menuTab + " span button[id*='" + $("#hfClinicalMenuChildLiId").val() + "']").parent("span").addClass('tab_selected');
                }
                else {
                    $("#mstrDiv" + menuTab + " .btn").first().addClass('tab_selected');
                }
            }
            objDeffered.resolve('done');
        }
        return objDeffered;

    },

    GetClinicalSubMenus: function (ClinicalModulesData, FormParentHTMLId) {

        var data = "ClinicalModulesData=" + ClinicalModulesData + "&FormParentHTMLId=" + FormParentHTMLId;
        return MDVisionService.defaultService(data, "ClinicalMenuSettings", "GET_CLINICAL_SUB_MENU");
    },

    LoadMenu: function () {
        (function ($) {

            'use strict';

            var $items = $('.nav-main li.nav-parent');

            function expand($li) {
                $li.children('ul.nav-children').slideDown('fast', function () {
                    $li.addClass('nav-expanded');
                    $(this).css('display', '');
                    ensureVisible($li);
                });
            }

            function collapse($li) {
                $li.children('ul.nav-children').slideUp('fast', function () {
                    $(this).css('display', '');
                    $li.removeClass('nav-expanded');
                });
            }

            function ensureVisible($li) {
                var scroller = $li.offsetParent();
                if (!scroller.get(0)) {
                    return false;
                }

                var top = $li.position().top;
                if (top < 0) {
                    scroller.animate({
                        scrollTop: scroller.scrollTop() + top
                    }, 'fast');
                }
            }

            $items.find('> a').on('click', function (ev) {

                var $anchor = $(this),
                    $prev = $anchor.closest('ul.nav').find('> li.nav-expanded'),
                    $next = $anchor.closest('li');

                if ($anchor.prop('href')) {
                    var arrowWidth = parseInt(window.getComputedStyle($anchor.get(0), ':after').width, 10) || 0;
                    if (ev.offsetX > $anchor.get(0).offsetWidth - arrowWidth) {
                        ev.preventDefault();
                    }
                }

                if ($prev.get(0) !== $next.get(0)) {
                    collapse($prev);
                    expand($next);
                } else {
                    collapse($prev);
                }
            });

            $("ul[id*=sortable]").sortable({
                placeholder: "ui-sortable-placeholder"

            });
            $("ul[id*=sortable]").sortable({
                out: function () {
                    //setTimeout(function () {
                    var clinicalMenuHtml = $("#ClinicalUL").html();
                    //$('#ClinicalUL')[0].outerHTML
                    var activeMenu = $(this).parent().attr('id');
                    ClinicalMenuSettings.UpdateClinicalMenuSettings(null, clinicalMenuHtml, activeMenu);
                    //}, 1000);

                }
            });

        }).apply(this, [jQuery]);
    },

    SearchClinicalMenuSettings: function (ClinicalModulesData, moduleID, tabID) {

        var data = "ClinicalModulesData=" + ClinicalModulesData + "&moduleID=" + moduleID + "&tabID=" + tabID;
        return MDVisionService.defaultService(data, "ClinicalMenuSettings", "SEARCH_CLINICAL_MENU_SETTINGS");
    },

    ClinicalMenuSettingsLoad: function (response, tabId) {
        if (response.MenuSettingsCount > 0) {
            var ClinicalMenuSettingsLoadJSONData = JSON.parse(response.ClinicalMenuSettingsLoad_JSON);
            var Header_ClinicalMenuSettings_LoadJSONData = JSON.parse(response.Header_ClinicalMenuSettings_JSON);

            if (response.status != false) {
                $('#mstrMenuClinical ul').remove();
                var ClinicalMenuSettingsList = "";

                var moduleName = "";
                var mainHeading = "";
                for (var indexHeader = 0; indexHeader < response.Header_ClinicalMenuSettings_Count; indexHeader++) {
                    //if (Header_ClinicalMenuSettings_LoadJSONData[index] == ) {
                    //}
                    moduleName = Header_ClinicalMenuSettings_LoadJSONData[indexHeader].FormParentHTMLId.replace("clinicalMenu", "");
                    mainHeading = "<li class='nav-parent' id='mstrMenuMedical' runat='server' visible='false'>" +
                                      "<a href='mstrTabMedical' data-toggle='tab' title='Medical' onclick='SelectTab(\"mstrTabMedical\",\"true\");'>" +
                                      "<i class='fa fa-calendar' aria-hidden='true'></i><span>Medical</span> </a>";

                    ClinicalMenuSettingsList += mainHeading + '<ul class="nav nav-children">';
                    for (var indexComponents = 0; indexComponents < ClinicalMenuSettingsLoadJSONData.length; indexComponents++) {
                        //if (ClinicalMenuSettingsLoadJSONData.length > 0) {

                        if (Header_ClinicalMenuSettings_LoadJSONData[indexHeader].FormParentHTMLId == ClinicalMenuSettingsLoadJSONData[indexComponents].FormParentHTMLId) {
                            ClinicalMenuSettingsList += '<li id=' + ClinicalMenuSettingsLoadJSONData[indexComponents].FormHTMLId + ' visible="false"><a href="javascript:void(0);">' + ClinicalMenuSettingsLoadJSONData[indexComponents].Name.split('_')[1] + '</a></li>';
                        }

                    }

                    ClinicalMenuSettingsList += "</ul></li>";
                    //$('#mstrMenuClinical ul').remove();

                }
                $('#mainUL').append(ClinicalMenuSettingsList);
            }

            //$.each(ClinicalMenuSettingsLoadJSONData, function (i, item) {

            //   alert( item.ModuleId + "_" + item.FormId)

            //});
        }
    },

    UpdateClinicalMenuSettings: function (userId, clinicalMenuHTML, activeMenuID) {
        //clinicalMenuHTML = clinicalMenuHTML.replace(/"/g, "'");
        if (activeMenuID != null) {
            ClinicalMenuSettings.TopButtons(activeMenuID);
        }
        clinicalMenuHTML = clinicalMenuHTML.replace(/&quot;/g, '"');
        clinicalMenuHTML = clinicalMenuHTML.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
        clinicalMenuHTML = clinicalMenuHTML.replace(/&#39;/g, "'");
        clinicalMenuHTML = clinicalMenuHTML.replace(/&amp;/g, "[[and]]");
        //ui-state-default nav-active ui-sortable-handle
        clinicalMenuHTML = clinicalMenuHTML.replace("ui-state-default nav-active ui-sortable-handle", "ui-state-default ui-sortable-handle");
        clinicalMenuHTML = clinicalMenuHTML.replace("nav-parent active nav-expanded", "nav-parent");
      
      
       
    

        var data = "userId=" + userId + "&clinicalMenuHTML=" + clinicalMenuHTML;
        // serach parameter , class name, command name of class 
        if (clinicalMenuHTML.indexOf("absolute") == -1) {
            return MDVisionService.defaultService(data, "ClinicalMenuSettings", "UPDATE_CLINICAL_MENU_SETTINGS");
        }
        else
        {
            ClinicalMenuSettings.ClinicalMenuSettingsSearch();
        }
    },

    UnLoadTab: function (Tab) {
        RemoveAdminTab(Tab);
    },
}