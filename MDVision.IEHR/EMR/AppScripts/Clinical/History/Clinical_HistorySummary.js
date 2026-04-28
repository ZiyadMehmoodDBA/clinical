Clinical_HistorySummary = {
    //Author: Muhammad Azhar Shahzad
    //Date: Dec 16,2015
    //This file will handle all actions performed for History Summary and it's child handling
    bIsFirstLoad: true,
    HistoryCacheList: {
        SocialHx: null,
        MedicalHx: null,
        FamilyHx: null,
        SurgicalHx: null,
        BirthHx: null,
        HospitalizationHx: null,
        SocPsyandBehaviorHx: null
    },
    params: [],
    //Author: Muhammad  Azhar Shahzad
    //Date: Dec 15,2015
    //This function will be called once tab is clicked, it expects parameters to be used for SocialHx
    Load: function (params) {
        Clinical_HistorySummary.params = params;
        //emr-1325 adnan maqbool
        //  $('#pnlClinicalSocialHx').remove();
        //  $('#pnlClinicalFamilyHx').remove();
        //$('#pnlClinicalMedicalHx').remove();
        // $('#pnlClinicalSurgicalHx').remove();
        // $('#pnlClinicalBirthHx').remove();
        // $('#pnlClinicalHospitalizationHx').remove();
        if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
            $('#' + Clinical_HistorySummary.params.PanelID + ' #modaldialog').addClass('modal-dialog modal-dialog-full');
            $('#' + Clinical_HistorySummary.params.PanelID + ' #mstrDivHistroy1').removeClass('hidden');
        }
        else {
            Clinical_HistorySummary.HistoryCacheList = {
                SocialHx: null,
                MedicalHx: null,
                FamilyHx: null,
                SurgicalHx: null,
                BirthHx: null,
                HospitalizationHx: null,
                SocPsyandBehaviorHx: null
            };
        }

        if (Clinical_HistorySummary.params.PanelID.indexOf('pnlClinicalHistorySummary') == -1) {
            Clinical_HistorySummary.params.PanelID = Clinical_HistorySummary.params.PanelID + ' #pnlClinicalHistorySummary';
        }


        try {
            if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {

                if (Clinical_HistorySummary.params["HxtabOrder"] != null && Clinical_HistorySummary.params["HxtabOrder"] != '') {
                    var btnNoteHistoryTabsUl = $('#' + Clinical_HistorySummary.params.PanelID + ' #btnNoteHistoryTabs ul').clone();
                    var Arrange = Clinical_HistorySummary.params["HxtabOrder"];
                    $('#' + Clinical_HistorySummary.params.PanelID + ' #btnNoteHistoryTabs ul').html('');
                    Arrange = Arrange.split(',');
                    var ulObject = $('<ul class="list-unstyled"> </ul>');
                    var lengthOfArray = Arrange.length - 1;
                    for (var index = lengthOfArray; index > -1; index--) {
                        var nameHx = Arrange[index];
                        var lito = $(btnNoteHistoryTabsUl).find("li[name*='" + nameHx + "']").clone();
                        $(ulObject).append(lito);
                    }
                    $('#' + Clinical_HistorySummary.params.PanelID + ' #btnNoteHistoryTabs ').append(ulObject);
                }

                $('#pnlClinicalHistorySummary #btnNoteHistoryTabs ul').sortable({
                    handle: 'button',
                    cancel: '',
                    update: function (evt) {
                        var obj = $('#pnlClinicalHistorySummary #btnNoteHistoryTabs ul').sortable('toArray');
                        var HxtabOrder = '';
                        $('#pnlClinicalHistorySummary #btnNoteHistoryTabs ul li').each(function () {
                            HxtabOrder = $(this).attr("name") + ',' + HxtabOrder;
                        });
                        if (HxtabOrder.length > 0) {
                            HxtabOrder = HxtabOrder.substring(0, HxtabOrder.length - 1);
                            if (Clinical_ProgressNote != null && Clinical_ProgressNote.params != null)
                                Clinical_ProgressNote.params["HxtabOrder"] = HxtabOrder;
                            Clinical_HistorySummary.updateHxtabOrder(HxtabOrder).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    if (Clinical_ProgressNote != null && Clinical_ProgressNote.params != null)
                                        Clinical_ProgressNote.params["HxtabOrder"] = HxtabOrder;
                                    Clinical_HistorySummary.params["HxtabOrder"] = HxtabOrder;
                                }
                            });
                        }
                    }
                });


            }

        }
        catch (ex) {
            console.log(ex);
        }
        $('#' + Clinical_HistorySummary.params.PanelID + ' #btnNoteHistoryTabs ul li').css('float', 'left');
        if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes') {
            $(' #pnlClinicalHistorySummary').removeClass('row');
        }
        if (Clinical_HistorySummary.params.ParentCtrl == null) {
            $('#divViewHistorySummary').removeClass('hidden');
        }


        if (params["TabToSelected"] == "SocialHx") {
            EMRUtility.AddHistoryTabOnNote('SocialHx', false, Clinical_HistorySummary.params, $('#' + Clinical_HistorySummary.params.PanelID + ' #clinicalMenu_History_SocialHx'));
        }
        else if (params["TabToSelected"] == "MedicalHx") {
            EMRUtility.AddHistoryTabOnNote('MedicalHx', false, Clinical_HistorySummary.params, $('#' + Clinical_HistorySummary.params.PanelID + ' #clinicalMenu_History_MedicalHx'));
        }
        else if (params["TabToSelected"] == "FamilyHx") {
            EMRUtility.AddHistoryTabOnNote('FamilyHx', false, Clinical_HistorySummary.params, $('#' + Clinical_HistorySummary.params.PanelID + ' #clinicalMenu_History_FamilyHx'));
        }
        else if (params["TabToSelected"] == "SurgicalHx") {
            EMRUtility.AddHistoryTabOnNote('SurgicalHx', false, Clinical_HistorySummary.params, $('#' + Clinical_HistorySummary.params.PanelID + ' #clinicalMenu_History_SurgicalHx'));
        }
        else if (params["TabToSelected"] == "BirthHx") {
            EMRUtility.AddHistoryTabOnNote('BirthHx', false, Clinical_HistorySummary.params, $('#' + Clinical_HistorySummary.params.PanelID + ' #clinicalMenu_History_BirthHx'));
        }
        else if (params["TabToSelected"] == "HospitalizationHx") {
            EMRUtility.AddHistoryTabOnNote('HospitalizationHx', false, Clinical_HistorySummary.params, $('#' + Clinical_HistorySummary.params.PanelID + ' #clinicalMenu_History_HospitalizationHx'));
        }
        else if (params["TabToSelected"] == "SocPsyandBehaviorHx") {
            EMRUtility.AddHistoryTabOnNote('SocPsyandBehaviorHx', false, Clinical_HistorySummary.params, $('#' + Clinical_HistorySummary.params.PanelID + ' #clinicalMenu_History_SocPsyandBehaviorHx'));
        }
        else {
            Clinical_HistorySummary.loadHistorySummarySoap();
        }
        if (globalAppdata["isMU3SocPsycBehaviourHx"] && globalAppdata["isMU3SocPsycBehaviourHx"].toLowerCase() == "false")
            $('#' + Clinical_HistorySummary.params.PanelID + ' #clinicalMenu_History_SocPsyandBehaviorHx').addClass("hidden");

    },

    updateHxtabOrder: function (HxtabOrder) {
        var objData = new Object();
        objData["NoteId"] = Clinical_HistorySummary.params.NotesId;
        objData["HxtabOrder"] = HxtabOrder;
        objData["commandType"] = "updatenoteshxtaborder";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");
    },

    //Author: Muhammad Azhar Shahzad
    //Date: Dec 16,2015
    //This function will get Soap Text for HistorySummary from its all childs and concatenate to show in Form

    loadHistorySummarySoap: function () {
        // Start 31/12/2015 Muhammad Irfan for View Privilege of history summary

        Clinical_HistorySummary.CacheActiveHistoryData();
        $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').empty();
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("History_Hx Summary", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_HistorySummary.getHistorySummarySoap_DbCall(Clinical_HistorySummary.params.patientID).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var SoapTextHtml = "";
                        if (response.HistorySummarySoapCount > 0) {


                            var HistorySummarySoap_JSON = JSON.parse(response.HistorySummarySoap_JSON);
                            $.each(HistorySummarySoap_JSON, function (index, element) {
                                if (element.SocialHxSoapText != "") {
                                    var onclickAttribute = 'Clinical_HistorySummary.openHistoryComponent(\"SocialHx\",event);';
                                    
                                    if (Clinical_HistorySummary.params.ActionPanContainer == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                        onclickAttribute = "EMRUtility.AddHistoryTabOnNote(\"SocialHx\", false, Clinical_HistorySummary.params, $(\"#pnlClinicalHistorySummary #clinicalMenu_History_SocialHx\"));";
                                    }
                                    SoapTextHtml += "<div id='History_Social_Hx'><a href='javascript:void(0);' onclick='" + onclickAttribute + "'><strong>Social Hx</strong></a></div>";
                                    SoapTextHtml += element.SocialHxSoapText;
                                }
                                if (element.FamilyHxSoapText != "") {
                                    var onclickAttribute = 'Clinical_HistorySummary.openHistoryComponent(\"FamilyHx\",event);';
                                    if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                        onclickAttribute = "EMRUtility.AddHistoryTabOnNote(\"FamilyHx\", false, Clinical_HistorySummary.params, $(\"#pnlClinicalHistorySummary #clinicalMenu_History_FamilyHx\"));";
                                    }
                                    SoapTextHtml += "<div id='History_FamilyHx'><a href='javascript:void(0);' onclick='" + onclickAttribute + "'><strong>Family Hx</strong></a></div>";
                                    var res = element.FamilyHxSoapText.split('<br/>').join('<div class="spacer10"></div>')
                                    SoapTextHtml += res;
                                }
                                if (element.MedicalHxSoapText != "") {
                                    var onclickAttribute = 'Clinical_HistorySummary.openHistoryComponent(\"MedicalHx\",event);';
                                    if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                        onclickAttribute = "EMRUtility.AddHistoryTabOnNote(\"MedicalHx\", false, Clinical_HistorySummary.params, $(\"#pnlClinicalHistorySummary #clinicalMenu_History_MedicalHx\"));";
                                    }
                                    SoapTextHtml += "<div id='History_MedicalHx'><a href='javascript:void(0);' onclick='" + onclickAttribute + "'><strong>Medical Hx</strong></a></div>";

                                    element.MedicalHxSoapText = element.MedicalHxSoapText.replace(/&quot;/g, '"');
                                    element.MedicalHxSoapText = element.MedicalHxSoapText.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                                    element.MedicalHxSoapText = element.MedicalHxSoapText.replace(/&nbsp;/g, '');

                                    var finalSoapText = '<p>';

                                    var soapText = document.createElement('div');
                                    $(soapText).html(element.MedicalHxSoapText);

                                    $(soapText).find('div').each(function (index, item) {
                                        var soap = $(this).html();
                                        var nextDisease = $($(soapText).find('div')[index + 1]).html();

                                        if (soap != "") {
                                            if (nextDisease != null) {
                                                if (soap.indexOf('The patient underwent') == -1 && soap.indexOf('The patient also underwent') == -1 && soap.indexOf('Status') == -1 && soap.indexOf('Test Result') == -1 && soap.indexOf('Onset') == -1 && soap.indexOf('Duration') == -1 && soap.indexOf('Severity') == -1
                                                     && soap.indexOf('Pattern') == -1 && soap.indexOf('Aggravated by') == -1 && soap.indexOf('Location') == -1 &&
                                                    nextDisease.indexOf('The patient underwent') == -1 && nextDisease.indexOf('The patient also underwent') == -1 && nextDisease.indexOf('Status') == -1 && nextDisease.indexOf('Test Result') == -1 && nextDisease.indexOf('Onset') == -1 && nextDisease.indexOf('Duration') == -1 && nextDisease.indexOf('Severity') == -1
                                                     && nextDisease.indexOf('Pattern') == -1 && nextDisease.indexOf('Aggravated by') == -1 && nextDisease.indexOf('Location') == -1) {

                                                    finalSoapText += soap[soap.length - 1] == ":" || soap[soap.length - 1] == "." ? soap.replace(soap[soap.length - 1], ', ') : soap + ", ";
                                                }
                                                else {
                                                    finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                                                }
                                            }
                                            else {
                                                finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                                            }
                                        }
                                    });
                                    $(soapText).find('div').remove();
                                    if (finalSoapText == '<p>') {
                                        finalSoapText += $(soapText).html() + "</p>"
                                    }
                                    else {
                                        finalSoapText += '<br>' + $(soapText).html() + "</p>"
                                    }


                                    SoapTextHtml += finalSoapText;
                                }
                                if (element.SurgicalHxSoapText != "") {
                                    var onclickAttribute = 'Clinical_HistorySummary.openHistoryComponent(\"SurgicalHx\",event);';
                                    if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                        onclickAttribute = "EMRUtility.AddHistoryTabOnNote(\"SurgicalHx\", false, Clinical_HistorySummary.params, $(\"#pnlClinicalHistorySummary #clinicalMenu_History_SurgicalHx\"));";
                                    }
                                    SoapTextHtml += "<div id='History_SurgicalHx'><a href='javascript:void(0);' onclick='" + onclickAttribute + "'><strong>Surgical Hx</strong></a></div>";
                                    SoapTextHtml += element.SurgicalHxSoapText;
                                }
                                if (element.EnvironmentalHxSoapText != "") {
                                    var onclickAttribute = 'Clinical_HistorySummary.openHistoryComponent(\"EnvironmentalHx\",event);';
                                    if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                        onclickAttribute = "EMRUtility.AddHistoryTabOnNote(\"EnvironmentalHx\", false, Clinical_HistorySummary.params, $(\"#pnlClinicalHistorySummary #clinicalMenu_History_EnvironmentalHx\"));";
                                    }
                                    SoapTextHtml += "<div id='History_EnvironmentalHx'><a href='javascript:void(0);' onclick='" + onclickAttribute + "'><strong>Environmental Hx</strong></a></div>";
                                    SoapTextHtml += element.EnvironmentalHxSoapText;
                                }
                                if (element.BirthHxSoapText != "") {
                                    var onclickAttribute = 'Clinical_HistorySummary.openHistoryComponent(\"BirthHx\",event);';
                                    if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                        onclickAttribute = "EMRUtility.AddHistoryTabOnNote(\"BirthHx\", false, Clinical_HistorySummary.params, $(\"#pnlClinicalHistorySummary #clinicalMenu_History_BirthHx\"));";
                                    }
                                    SoapTextHtml += "<div id='History_BirthHx'><a href='javascript:void(0);' onclick='" + onclickAttribute + "'><strong>Birth Hx</strong></a></div>";
                                    SoapTextHtml += element.BirthHxSoapText;
                                }
                                if (element.HospitalizationHxSoapText != "") {
                                    var onclickAttribute = 'Clinical_HistorySummary.openHistoryComponent(\"HospitalizationHx\",event);';
                                    if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                        onclickAttribute = "EMRUtility.AddHistoryTabOnNote(\"HospitalizationHx\", false, Clinical_HistorySummary.params, $(\"#pnlClinicalHistorySummary #clinicalMenu_History_HospitalizationHx\"));";
                                    }
                                    SoapTextHtml += "<div id='History_HospitalizationHx'><a href='javascript:void(0);' onclick='" + onclickAttribute + "'><strong>Hospitalization Hx</strong></a></div>";
                                    SoapTextHtml += element.HospitalizationHxSoapText;
                                }
                                if (element.SocPsyandBehaviorHxSoapText != "") {
                                    var onclickAttribute = 'Clinical_HistorySummary.openHistoryComponent(\"SocPsyandBehaviorHx\",event);';
                                    if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                        onclickAttribute = "EMRUtility.AddHistoryTabOnNote(\"SocPsyandBehaviorHx\", false, Clinical_HistorySummary.params, $(\"#pnlClinicalHistorySummary #clinicalMenu_History_SocPsyandBehaviorHx\"));";
                                    }
                                    if (globalAppdata["isMU3SocPsycBehaviourHx"] && globalAppdata["isMU3SocPsycBehaviourHx"].toLowerCase() == "false")
                                        onclickAttribute = "";
                                    SoapTextHtml += "<div id='History_SocPsyandBehaviorHx'><a href='javascript:void(0);' onclick='" + onclickAttribute + "'><strong>Social, Psychological and Behavior Hx</strong></a></div>";
                                    SoapTextHtml += element.SocPsyandBehaviorHxSoapText;
                                }
                                if (SoapTextHtml != '') {
                                    SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                                    SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');

                                    SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                                    if (Clinical_HistorySummary.params.PanelID != "pnlClinicalHistorySummary") {
                                        Clinical_HistorySummary.params.PanelID = "pnlClinicalHistorySummary";
                                    }
                                    if (Clinical_HistorySummary.params.PanelID.indexOf("HistroySummary") > -1 || Clinical_HistorySummary.params.PanelID.indexOf("pnlClinicalHistorySummary") > -1) {

                                        if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                            //var section = $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('section:first');
                                            //Start Bug fix EMR-4943 By:M Ahmad Imran
                                            var progressNotePanel = "#pnlClinicalProgressNote ";
                                            if (Clinical_HistorySummary.params.PanelID.indexOf("pnlClinicalProgressNote") > 0) {
                                                progressNotePanel = "";
                                            }
                                            var section = $(progressNotePanel + '#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('section#sectionSoapTexts');
                                            //End Bug fix EMR-4943 By:M Ahmad Imran
                                            if ($(section).length > 0) {
                                                $(section).css('display', 'block');
                                                $(section).find('div.panel-body').html(SoapTextHtml);
                                            }
                                            else {
                                                $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').prepend('<section id="sectionSoapTexts" class="panel panel-featured"><header class="panel-heading"><h2 class="panel-title">SOAP Text </h2></header><div class="panel-body">' + SoapTextHtml + '</div></section>');
                                            }

                                            Clinical_HistorySummary.HideActiveHistory();
                                        }
                                        else
                                            $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').html(SoapTextHtml);

                                    }
                                    else {

                                        if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabNotes' || Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                            var section = $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('section:first');
                                            if ($(section).length > 0) {
                                                $(section).css('display', 'block');
                                                $(section).find('div.panel-body').html(SoapTextHtml);
                                            }
                                            else {
                                                $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').prepend('<section class="panel panel-featured"><header class="panel-heading"><h2 class="panel-title">SOAP Text </h2></header><div class="panel-body">' + SoapTextHtml + '</div></section>');
                                            }

                                            Clinical_HistorySummary.HideActiveHistory();
                                        }
                                        else
                                            $('#' + Clinical_HistorySummary.params.PanelID).closest("#divViewHistorySummary").html(SoapTextHtml);

                                        panelId = $('#' + Clinical_HistorySummary.params.PanelID).closest("#pnlClinicalHistorySummary").attr('id');
                                        if (panelId == null)
                                            panelId = "pnlClinicalHistorySummary";
                                        Clinical_HistorySummary.params.PanelID = panelId;
                                    }

                                    $.each($('#btnNoteHistoryTabs').find("button"), function (index, item) {
                                        if ($(item).attr("id") == 'clinicalMenu_History_HistorySummary') {
                                            $(item).addClass("active");
                                        }
                                        else {
                                            $(item).removeClass("active");
                                        }
                                    });


                                } else {
                                    // If there is no SOAP yet created for Patient than next history tab will be shown
                                    //Clinical_HistorySummary.unLoad();
                                    if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                        $('#' + Clinical_HistorySummary.params.PanelID + ' #mstrDivHistroy1 .active').next().trigger('click');
                                    } else {
                                        $('#' + Clinical_HistorySummary.params.PanelID + ' #mstrDivHistroy .active').next().trigger('click');
                                    }
                                }
                            });
                        } else {
                            // If there is no SOAP yet created for Patient than next history tab will be shown
                            if (Clinical_HistorySummary.params.ParentCtrl == 'clinicalTabProgressNote') {
                                //$('#' + Clinical_HistorySummary.params.PanelID + ' #mstrDivHistroy1 .active').parent().next().find("button").trigger('click');
                            } else {
                                //$('#pnlTab3 #mstrDivHistroy .active').next().trigger('click');
                            }
                        }
                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    HideActiveHistory: function () {

        var socialHx = $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalSocialHx').length;
        if (socialHx > 0) {
            $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalSocialHx').css('display', 'none');
        }

        var medicalHx = $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalMedicalHx').length;
        if (medicalHx > 0) {
            $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalMedicalHx').css('display', 'none');
        }

        var familyHx = $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalFamilyHx').length;
        if (familyHx > 0) {
            $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalFamilyHx').css('display', 'none');
        }

        var surgicalHx = $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalSurgicalHx').length;
        if (surgicalHx > 0) {
            $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalSurgicalHx').css('display', 'none');
        }

        var birthHx = $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalBirthHx').length;
        if (birthHx > 0) {
            $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalBirthHx').css('display', 'none');
        }

        var hospitalizationHx = $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalHospitalizationHx').length;
        if (hospitalizationHx > 0) {
            $('#' + Clinical_HistorySummary.params.PanelID + ' #divViewHistorySummary').find('div#pnlClinicalHospitalizationHx').css('display', 'none');
        }

    },
    //This Function will make server side call to get SOAP text
    //Author: Muhammad Azhar Shahzad, Date: Dec 16,2015
    getHistorySummarySoap_DbCall: function () {
        var objData = new Object();
        if (Clinical_HistorySummary.params.patientID == "" || Clinical_HistorySummary.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_HistorySummary.params.patientID;
        }
        if (Clinical_HistorySummary.params.ParentCtrl == "clinicalTabNotes") {
            objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        }
        objData["commandType"] = "getHistorySummarySoapByPatientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");
    },

    checkForGlobalDetail: function (Type, HxList) {
        var cacheJSON = $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').data('serialize').split("&");
        var data = $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').serialize().split("&");

        var cacheJSON = {};
        for (var key in data) {
            cacheJSON[data[key].split("=")[0]] = decodeURIComponent(data[key].split("=")[1]);
        }
        var result = false;
        if (HxList[Type + "Comments"] && cacheJSON[Type + "Comments"] != HxList[Type + "Comments"]) {
            result = true;
        }
        //if (cacheJSON[Type + "Unremarkable"] != HxList[Type + "Unremarkable"]) {
        //    result = true;
        //}
        if (HxList[Type + "Date"] && cacheJSON[Type + "Date"] != HxList[Type + "Date"]) {
            result = true;
        }

        return result;
    },
    unLoad: function () {
        if (Clinical_HistorySummary.params.ParentCtrl == "clinicalTabNotes" || Clinical_HistorySummary.params.ParentCtrl == null) {
            Clinical_HistorySummary.params.ParentCtrl = "clinicalTabProgressNote";
        }
        Clinical_BirthHx.bIsFirstLoad = true;
        Clinical_FamilyHx.bIsFirstLoad = true;
        Clinical_HospitalizationHx.bIsFirstLoad = true;
        Clinical_MedicalHx.bIsFirstLoad = true;
        Clinical_SocialHx.bIsFirstLoad = true;
        Clinical_SurgicalHx.bIsFirstLoad = true;
        if (Clinical_HistorySummary.params.ParentCtrl == "clinicalTabProgressNote") {

            Clinical_HistorySummary.CacheActiveHistoryData();

            var socialChanged = false;
            var medicalChanged = false;
            var familyChanged = false;
            var surgicalChanged = false;
            var birthChanged = false;
            var hospitalizationChanged = false;
            var SocPsyanBehaviorChanged = false;

            var detailExists = false;

            if (Clinical_HistorySummary.HistoryCacheList.SocialHx) {
                var date = Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxDate;
                var unremarkable = Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable;
                var comments = Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxComments;

                var alcohol = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel.length;
                var caffeine = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel.length;
                var drugs = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel.length;
                var exercise = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel.length;
                var housing = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel.length;
                var travel = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel.length;
                var occupation = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel.length
                var sexual = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel.length;
                var sleep = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel.length;
                var tobacco = Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel.length

                var detail = alcohol || caffeine || drugs || exercise || housing || occupation || sexual || sleep || tobacco || travel;

                if (Clinical_SocialHx.date != date || Clinical_SocialHx.unremarkable != unremarkable || Clinical_SocialHx.overallComments != comments
                   || detail > 0) {
                    socialChanged = true;
                }
                else {
                    socialChanged = false;
                }
            }

            if (Clinical_HistorySummary.HistoryCacheList.MedicalHx) {
                var date = Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxDate;
                var unremarkable = Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxUnremarkable;
                var comments = Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxComments;
                var diseases = Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList.length;

                if (Clinical_MedicalHx.medicalDate != date || Clinical_MedicalHx.unremarkable != unremarkable || Clinical_MedicalHx.overallComments != comments
                   || diseases > 0) {
                    medicalChanged = true;
                }
                else {
                    medicalChanged = false;
                }
            }

            if (Clinical_HistorySummary.HistoryCacheList.FamilyHx) {
                var date = Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyHxDate;
                var unremarkable = Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyHxUnremarkable;
                var comments = Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyOverallComments;
                var diseases = Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease.length;
                var diseasesDetail = Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail.length;

                if (Clinical_FamilyHx.familyDate != date || Clinical_FamilyHx.unremarkable != unremarkable || Clinical_FamilyHx.overallComments != comments
                   || diseases > 0 || diseasesDetail > 0) {
                    familyChanged = true;
                }
                else {
                    familyChanged = false;
                }
            }

            if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
                var date = Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxDate;
                var unremarkable = Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxUnremarkable;
                var comments = Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxComments;
                var diseases = Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList.length;

                if (Clinical_SurgicalHx.surgicalDate != date || Clinical_SurgicalHx.unremarkable != unremarkable || Clinical_SurgicalHx.overallComments != comments
                   || diseases > 0) {
                    surgicalChanged = true;
                }
                else {
                    surgicalChanged = false;
                }
            }

            if (Clinical_HistorySummary.HistoryCacheList.BirthHx != null) {

                var date = Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxDate;
                var unremarkable = Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxUnremarkable;
                var comments = Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxComments;
                var general = Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxGeneral;
                var maternal = Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxMaternalDelivery;
                var newborn = Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxNewborn;

                if (Clinical_BirthHx.date != date || Clinical_BirthHx.unremarkable != unremarkable || Clinical_BirthHx.overallComments != comments
                   || general != null || maternal != null || newborn != null) {
                    birthChanged = true;
                }
                else {
                    birthChanged = false;
                }
            }
            if (Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx != null) {
                CurrentJson = $('#' + Clinical_SocPsyandBehaviorHx.params.PanelID + " #frmClinicalSocPsyandBehaviorHx").getMyJSONByName();

                if (Clinical_SocPsyandBehaviorHx.SocPsyandBehaviorJSON != CurrentJson) {
                    SocPsyanBehaviorChanged = true;
                }
                else {
                    SocPsyanBehaviorChanged = false;
                }
            }
            if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null) {
                var date = Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxDate;
                var unremarkable = Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxUnremarkable;
                var comments = Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxComments;
                var diseases = Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList.length;

                if (Clinical_HospitalizationHx.hospitalizationDate != date || Clinical_HospitalizationHx.unremarkable != unremarkable || Clinical_HospitalizationHx.overallComments != comments
                   || diseases > 0) {
                    hospitalizationChanged = true;
                }
                else {
                    hospitalizationChanged = false;
                }
            }

            detailExists = socialChanged || medicalChanged || familyChanged || surgicalChanged || birthChanged || hospitalizationChanged || SocPsyanBehaviorChanged;

            if (detailExists == true) {
                utility.myConfirmNote('1', function () {
                    Clinical_HistorySummary.addHistoriesToNote(true);

                }, function () {
                    Clinical_HistorySummary.addHistoriesToNote(false);
                }, function () {
                    UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                    Clinical_HistorySummary.HistoryCacheList = {
                        SocialHx: null,
                        MedicalHx: null,
                        FamilyHx: null,
                        SurgicalHx: null,
                        BirthHx: null,
                        HospitalizationHx: null,
                        SocPsyandBehaviorHx: null
                    };
                },
              '1', false
              );
            }
            else {
                UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                Clinical_HistorySummary.HistoryCacheList = {
                    SocialHx: null,
                    MedicalHx: null,
                    FamilyHx: null,
                    SurgicalHx: null,
                    BirthHx: null,
                    HospitalizationHx: null,
                    SocPsyandBehaviorHx: null
                };
                //Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabBirthHx','BirthHx');
                //Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabFamilyHx','FamilyHx');
                //Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabHospitalizationHx', 'HospitalizationHx');
                //Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabMedicalHx', 'MedicalHx');
                //Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSocialHx', 'SocialHx');
                //Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSurgicalHx', 'SurgicalHx');
            }
        }

    },



    RemoveTabFromTabArray: function (tabID, tabName) {
        for (var i = 0; i < TabsArray.length; i++) {
            if (TabsArray[i].TabID == tabID) {
                TabsArray.splice(i, 1);
            }
        }

        $('#ctrlPanClinical #pnlClinical' + tabName).remove();
    },

    openHistoryComponent: function (ComponentName, event) {
        javascript: ClinicalMenuClick(event, function () {
            javascript: SelectTab('clinicalTab' + ComponentName, 'true');
        }, 0, null, 'clinicalMenu_History_' + ComponentName, 'li');
    },

    CacheActiveHistoryData: function () {

        var dfd = $.Deferred();
        var activeHistory = $('#pnlClinicalProgressNote #pnlClinicalHistorySummary #btnNoteHistoryTabs > ul > li > button.active').attr('title');
        if (activeHistory == "Medical Hx" && Clinical_MedicalHx.medicalHxJSON != '') {
            var dataChanged = EMRUtility.compareFormDataWithSerialized(Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx');
            if (Clinical_MedicalHx.medicalHxJSON != $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").getMyJSONByName() || dataChanged) {
                var diseaseId = $('#' + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease > li.active").attr('id');
                var diseaseData = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").getMyJSONByName();
                $.when(Clinical_MedicalHx.cacheMedicalHxJSON(diseaseId, diseaseData)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }
        }
        else if (activeHistory == "Birth Hx") {
            $.when(Clinical_BirthHx.CacheSectionsData(false)).then(function () {
                dfd.resolve();
            });
        }
        else if (activeHistory == "Social, Psychological and Behavior Hx") {
            $.when(Clinical_SocPsyandBehaviorHx.cacheSocPsyandBehaviorHxJSON()).then(function () {
                dfd.resolve();
            });
        }
        else if (activeHistory == "Family Hx" && Clinical_FamilyHx.familyHxJSON != '') {
            var memberId = $('#' + Clinical_FamilyHx.params.PanelID + " ul#ulFamilyMember li.active").attr('id');
            var diseaseId = $('#' + Clinical_FamilyHx.params.PanelID + " #FamilyDisease ul#ulFamilyHxDisease li.active").attr('id');
            var updatedJSON = $('#' + Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx #FamilyMemberDetails').getMyJSONByName();
            // var dataChanged = EMRUtility.compareFormDataWithSerialized(Clinical_FamilyHx.params.PanelID + ' #frmClinicalFamilyHx');
            var date = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #dtFamilyHxDate").val();
            var unremarkable = $('#' + Clinical_FamilyHx.params.PanelID + " #frmClinicalFamilyHx #chkFamilyHxUnremarkable").prop('checked');
            var comments = $('#' + Clinical_FamilyHx.params.PanelID + " #txtFamilyOverallComments").val();
            var dataChanged = Clinical_FamilyHx.familyDate != date || Clinical_FamilyHx.unremarkable != unremarkable || Clinical_FamilyHx.overallComments != comments;

            if (Clinical_FamilyHx.familyHxJSON != updatedJSON || dataChanged) {
                $.when(Clinical_FamilyHx.cacheFamilyHxJSON(memberId, diseaseId, updatedJSON)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }
        }
        else if (activeHistory == "Surgical Hx" && Clinical_SurgicalHx.surgicalHxJSON != '') {
            var dataChanged = EMRUtility.compareFormDataWithSerialized(Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx');

            var date = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtSurgicalHxDate").val();
            var unremarkable = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop('checked');
            var comments = $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOverallComments").val();

            var dataChanged = Clinical_SurgicalHx.surgicalDate != date || Clinical_SurgicalHx.unremarkable != unremarkable || Clinical_SurgicalHx.overallComments != comments;

            if (Clinical_SurgicalHx.surgicalHxJSON != $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").getMyJSONByName() || dataChanged) {
                var diseaseId = $('#' + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease > li.active").attr('id');
                var diseaseData = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").getMyJSONByName();
                $.when(Clinical_SurgicalHx.cacheSurgicalHxJSON(diseaseId, diseaseData)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }
        }
        else if (activeHistory == "Hospitalization Hx" && Clinical_HospitalizationHx.hospitalizationHxJSON != '') {

            var date = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #dtHospitalizationHxDate").val();
            var unremarkable = $('#' + Clinical_HospitalizationHx.params.PanelID + " #frmClinicalHospitalizationHx #chkHospitalizationHxUnremarkable").prop('checked');
            var comments = $('#' + Clinical_HospitalizationHx.params.PanelID + " #txtHospitalizationOverallComments").val();

            var dataChanged = Clinical_HospitalizationHx.hospitalizationDate != date || Clinical_HospitalizationHx.unremarkable != unremarkable || Clinical_HospitalizationHx.overallComments != comments;

            if (Clinical_HospitalizationHx.hospitalizationHxJSON != $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName() || dataChanged) {
                var diseaseId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li.active").attr('id');
                var diseaseData = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();
                $.when(Clinical_HospitalizationHx.cacheHospitalizationHxJSON(diseaseId, diseaseData)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                dfd.resolve();
            }
        }

        else if (activeHistory == "Social Hx" && Clinical_SocialHx.socialHxJSON != '') {
            if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked")) {
                var patientId;
                if (Clinical_SocialHx.params.patientID == null) {
                    patientId = $('#PatientProfile #hfPatientId').val();
                } else {
                    patientId = Clinical_SocialHx.params.patientID;
                }
                var SocialHxData = {
                    SocialHxId: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(),
                    PatientId: patientId,
                    SocialHxDate: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val(),
                    SocialHxUnremarkable: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked"),
                    SocialComments: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val(),
                    NotesId: Clinical_ProgressNote.params.NotesId,
                    lstTobaccoModel: [],
                    lstAlcoholModel: [],
                    lstDrugAbuseModel: [],
                    lstSexualHxModel: [],
                    lstOccupationHxModel: [],
                    lstSleepHxModel: [],
                    lstExercisesHxModel: [],
                    lstHousingHxModel: [],
                    lstTravelHxModel: [],
                    lstCaffeineIntakHxModel: [],
                }
                Clinical_HistorySummary.HistoryCacheList.SocialHx = SocialHxData;
                dfd.resolve();
            }
            else {

                var activeTab = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems > li.active > a").text();

                if (activeTab.toLowerCase() != 'tobacco' && activeTab.toLowerCase() != 'alcohol' && activeTab.toLowerCase() != 'drug' && activeTab.toLowerCase() != 'drug abuse' && activeTab.toLowerCase() != 'sexual' && activeTab.toLowerCase() != 'sexual hx') {
                    activeTab += "_" + $('#' + Clinical_SocialHx.params.PanelID + " #ulMiscStatus > li.active > a").text().replace(" ", "").trim();
                }
                $.when(Clinical_SocialHx.cachePrevTabData(activeTab.toLowerCase())).then(function () {
                    dfd.resolve();
                });
            }
        }
        else {
            dfd.resolve();
        }

        return dfd;
    },

    addHistoriesToNote: function (addToNote) {

        //Clinical_HistorySummary.CacheActiveHistoryData();
        $.when(Clinical_HistorySummary.CacheActiveHistoryData()).then(function () {
            Clinical_HistorySummary.aaddHistoriesToNote_DBCall(addToNote).done(function (response) {
                $.when(Clinical_HistorySummary.SaveFavListStatusAndValue()).then(function () {
                    Clinical_HistorySummary.HistoryCacheList.SurgicalHx = null;
                    Clinical_HistorySummary.HistoryCacheList.MedicalHx = null;
                    Clinical_HistorySummary.HistoryCacheList.BirthHx = null;
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx = null;
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx = null;
                    Clinical_HistorySummary.HistoryCacheList.SocialHx = null;
                    Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx = null;


                    response = JSON.parse(response);
                    if (response.status != false) {
                        var SoapTextHtml = "";
                        if (response.HistorySummarySoapCount > 0 && addToNote == true) {
                            var hideAlertMessage = true;

                            var HistorySummarySoap_JSON = JSON.parse(response.HistorySummarySoap_JSON);
                            $.each(HistorySummarySoap_JSON, function (index, element) {
                                if (element.SocialHxSoapText != "") {
                                    var SoapTextHtml = element.SocialHxSoapText;
                                    SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                                    SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                                    SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                                    element.SocialHxSoapText = SoapTextHtml;
                                    Clinical_SocialHx.createSocialHxBodyHTMLFromNote(element, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage).done(function () {
                                        EMRUtility.scrollToPNcomponent('clinical_socialhx');
                                        Clinical_ProgressNote.saveComponentSOAPText("SocialHx", true);
                                    });
                                }
                                if (element.FamilyHxSoapText != "") {
                                    var SoapTextHtml = element.FamilyHxSoapText;
                                    SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                                    SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');

                                    SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                                    element.FamilyHxSoapText = SoapTextHtml;
                                    Clinical_FamilyHx.createFamilyHxBodyHTMLFromNote(element, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage).done(function () {
                                        EMRUtility.scrollToPNcomponent('clinical_familyhx');
                                        Clinical_ProgressNote.saveComponentSOAPText("FamilyHx", true);
                                    });
                                }
                                if (element.MedicalHxSoapText != "") {

                                    var SoapTextHtml = element.MedicalHxSoapText;
                                    SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                                    SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');

                                    SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                                    element.MedicalHxSoapText = SoapTextHtml;
                                    Clinical_MedicalHx.createMedicalHxBodyHTMLFromNote(element, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage).done(function () {
                                        EMRUtility.scrollToPNcomponent('clinical_medicalhx');
                                        Clinical_ProgressNote.saveComponentSOAPText("MedicalHx", true);
                                    });

                                }
                                if (element.SurgicalHxSoapText != "") {
                                    var SoapTextHtml = element.SurgicalHxSoapText;
                                    SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                                    SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');

                                    SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                                    element.SurgicalHxSoapText = SoapTextHtml;
                                    Clinical_SurgicalHx.createSurgicalHxBodyHTMLFromNote(element, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage).done(function () {
                                        EMRUtility.scrollToPNcomponent('clinical_surgicalhx');
                                        Clinical_ProgressNote.saveComponentSOAPText("SurgicalHx", true);
                                    });
                                }
                                if (element.EnvironmentalHxSoapText != "") {
                                }
                                if (element.BirthHxSoapText != "") {
                                    var SoapTextHtml = element.BirthHxSoapText;
                                    SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                                    SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');

                                    SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                                    element.BirthHxSoapText = SoapTextHtml;
                                    Clinical_BirthHx.createBirthHxBodyHTMLFromNote(element, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage).done(function () {
                                        EMRUtility.scrollToPNcomponent('clinical_birthhx');
                                        Clinical_ProgressNote.saveComponentSOAPText("BirthHx", true);
                                    });
                                }
                                if (element.HospitalizationHxSoapText != "") {
                                    var SoapTextHtml = element.HospitalizationHxSoapText;
                                    SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                                    SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');

                                    SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                                    element.HospitalizationHxSoapText = SoapTextHtml;
                                    Clinical_HospitalizationHx.createHospitalizationHxBodyHTMLFromNote(element, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage).done(function () {
                                        EMRUtility.scrollToPNcomponent('clinical_hospitalizationhx');
                                        Clinical_ProgressNote.saveComponentSOAPText("HospitalizationHx", true);
                                    });
                                }
                                if (element.SocPsyandBehaviorHxSoapText != "") {
                                    var SoapTextHtml = element.SocPsyandBehaviorHxSoapText;
                                    SoapTextHtml = SoapTextHtml.replace(/&quot;/g, '"');
                                    SoapTextHtml = SoapTextHtml.replace(/&lt;/g, '<').replace(/&gt;/g, '>');

                                    SoapTextHtml = SoapTextHtml.replace(/&nbsp;/g, '');
                                    element.SocPsyandBehaviorHxSoapText = SoapTextHtml;
                                    Clinical_SocPsyandBehaviorHx.createSocPsyandBehaviorHxBodyHTMLFromNote(element, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage).done(function () {
                                        EMRUtility.scrollToPNcomponent('clinical_socpsyandbehaviorhx');
                                        Clinical_ProgressNote.saveComponentSOAPText("SocPsyandBehaviorHx", true);
                                    });
                                }
                                if (SoapTextHtml != '') {

                                } else {
                                }
                                //commented by mary
                                //Clinical_ProgressNote.updateProgressNoteHTML(null, null, false);
                                Clinical_ProgressNote.ShowHideComponetsHeaders();
                                //Clinical_ProgressNote.saveComponentSOAPText("Hx",true);
                            });
                        } else {
                        }
                        Clinical_BirthHx.bIsFirstLoad = true;
                        Clinical_FamilyHx.bIsFirstLoad = true;
                        Clinical_HospitalizationHx.bIsFirstLoad = true;
                        Clinical_MedicalHx.bIsFirstLoad = true;
                        Clinical_SocialHx.bIsFirstLoad = true;
                        Clinical_SurgicalHx.bIsFirstLoad = true;
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        utility.DisplayMessages("Successfully Saved", 1);
                    }
                    else {
                        utility.DisplayMessages(response.message, 3);
                    }
                });
            });
        });
    },

    SaveFavListStatusAndValue: function () {
        var dfd = $.Deferred();
        var def = [];
        if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null) {
            def.push($.when(EMRUtility.insertUpdateFavListStatus(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FavListName, Clinical_HistorySummary.HistoryCacheList.FamilyHx.isFavListOpened)).then(function () {
                def.push($.when(EMRUtility.insertUpdateFavListVal(Clinical_HistorySummary.HistoryCacheList.FamilyHx.FavListName, Clinical_HistorySummary.HistoryCacheList.FamilyHx.FavListVal)).then(function () {

                })
                );
            })
            );
        }

        if (Clinical_HistorySummary.HistoryCacheList.MedicalHx != null) {
            def.push($.when(EMRUtility.insertUpdateFavListStatus(Clinical_HistorySummary.HistoryCacheList.MedicalHx.FavListName, Clinical_HistorySummary.HistoryCacheList.MedicalHx.isFavListOpened)).then(function () {
                def.push($.when(EMRUtility.insertUpdateFavListVal(Clinical_HistorySummary.HistoryCacheList.MedicalHx.FavListName, Clinical_HistorySummary.HistoryCacheList.MedicalHx.FavListVal)).then(function () {

                })
                );
            })
            );
        }

        if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
            def.push($.when(EMRUtility.insertUpdateFavListStatus(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.FavListName, Clinical_HistorySummary.HistoryCacheList.SurgicalHx.isFavListOpened)).then(function () {
                def.push($.when(EMRUtility.insertUpdateFavListVal(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.FavListName, Clinical_HistorySummary.HistoryCacheList.SurgicalHx.FavListVal)).then(function () {

                })
                );
            })
            );
        }

        $.when.apply($, def).done(function ($n) {
            dfd.resolve();
        });
        return dfd;
    },
    //addHistoriesToNote: function (addToNote) {

    //    Clinical_HistorySummary.CacheActiveHistoryData();
    //    Clinical_HistorySummary.aaddHistoriesToNote_DBCall().done(function (response) {

    //        response = JSON.parse(response);
    //        if (response.status != false) {
    //            var NoteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
    //            var MedicalHxId = response.MedicalHxId;

    //            if (MedicalHxId > 0) {

    //                var $mainDivMedicalHx = $(document.createElement('div'));

    //                if ($(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId).length == 0) {
    //                    $mainDivMedicalHx.append(response.MedicalHxSoapText);

    //                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().addClass('initialVisitBody');
    //                    if ($mainDivMedicalHx.html() != '') {
    //                        $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().append($mainDivMedicalHx.html());
    //                    }
    //                    var Edithandler = function (e) {
    //                        Clinical_ProgressNote.EditNotesComments_ComponentAttached($(this).parent().next());
    //                    };
    //                    $(NoteHTMLCtrl + ' #Cli_MedicalHx_Main' + MedicalHxId + ' .btnPNC_Edit').unbind("click", Edithandler).bind("click", Edithandler);

    //                    var Removehandler = function (e) {
    //                        Clinical_ProgressNote.RemoveComponentAttached($(this).attr("name"));
    //                    };
    //                    $(NoteHTMLCtrl + ' #Cli_MedicalHx_Main' + MedicalHxId + ' .btnPNC_Remove').unbind("click", Removehandler).bind("click", Removehandler);

    //                } else {

    //                    var CommentHTML = "";
    //                    var CommentsID = $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul li:Last').attr('id');
    //                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
    //                        CommentHTML = $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul li:Last').get(0).outerHTML;
    //                    }
    //                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId).html(response.MedicalHxSoapText);
    //                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul').append(CommentHTML);
    //                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().addClass('initialVisitBody');
    //                    if ($mainDivMedicalHx.html() != '') {
    //                        $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().append($mainDivMedicalHx.html());
    //                    }
    //                    var Edithandler = function (e) {
    //                        Clinical_ProgressNote.EditNotesComments_ComponentAttached($(this).parent().next());
    //                    };
    //                    $(NoteHTMLCtrl + ' #Cli_MedicalHx_Main' + MedicalHxId + ' .btnPNC_Edit').unbind("click", Edithandler).bind("click", Edithandler);

    //                    var Removehandler = function (e) {
    //                        Clinical_ProgressNote.RemoveComponentAttached($(this).attr("name"));
    //                    };
    //                    $(NoteHTMLCtrl + ' #Cli_MedicalHx_Main' + MedicalHxId + ' .btnPNC_Remove').unbind("click", Removehandler).bind("click", Removehandler);
    //                }
    //            }

    //            Clinical_ProgressNote.updateProgressNoteHTML(null, null, true).done(function () {
    //                utility.DisplayMessages('Saved',1);

    //            });
    //        }
    //    });

    //    var objData = {};
    //    objData["hospitalModel"]
    //    //if (Clinical_HistorySummary.HistoryCacheList.MedicalHx != null) {
    //    //    Clinical_MedicalHx.CacheMedicalHxSave('Disease', true, addToNote).done(function () {
    //    //        if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
    //    //            Clinical_SurgicalHx.CacheSurgicalHxSave('Disease', true, addToNote).done(function () {
    //    //                dfd.resolve();
    //    //            });
    //    //        }
    //    //    });
    //    //}



    //    //if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null && Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.length > 0) {
    //    //    Clinical_HospitalizationHx.CacheHospitalizationHxSave('Disease', true, addToNote);
    //    //}

    //    //if (Clinical_HistorySummary.HistoryCacheList.BirthHx != null && Clinical_HistorySummary.HistoryCacheList.BirthHx.length > 0) {
    //    //    Clinical_BirthHx.CacheSaveBirthHx(true, addToNote);
    //    //}

    //    //if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null && Clinical_HistorySummary.HistoryCacheList.FamilyHx.length > 0) {
    //    //    Clinical_FamilyHx.CacheFamilyHxSave('', true, addToNote);
    //    //}

    //    //if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null && Clinical_HistorySummary.HistoryCacheList.SocialHx.length > 0) {
    //    //    Clinical_SocialHx.CacheSocialHxSave('', true, addToNote);
    //    //}

    //    //dfd.done(function () {
    //    //    var a = 10;
    //    //});


    //    //var dfd = new $.Deferred();
    //    //var isSaved = false;

    //    //Clinical_HistorySummary.CacheActiveHistoryData();

    //    //if (Clinical_HistorySummary.HistoryCacheList.MedicalHx != null && Clinical_HistorySummary.HistoryCacheList.MedicalHx.length > 0) {
    //    //    Clinical_MedicalHx.CacheMedicalHxSave('Disease', true, addToNote).done(function () {
    //    //        isSaved = true;
    //    //        dfd.resolve();
    //    //    });
    //    //}

    //    //if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null && Clinical_HistorySummary.HistoryCacheList.SurgicalHx.length > 0) {
    //    //    Clinical_SurgicalHx.CacheSurgicalHxSave('Disease', true, addToNote);
    //    //}

    //    //if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null && Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.length > 0) {
    //    //    Clinical_HospitalizationHx.CacheHospitalizationHxSave('Disease',true, addToNote);
    //    //}

    //    //if (Clinical_HistorySummary.HistoryCacheList.BirthHx != null && Clinical_HistorySummary.HistoryCacheList.BirthHx.length > 0) {
    //    //    Clinical_BirthHx.CacheSaveBirthHx(true, addToNote);
    //    //}

    //    //if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null && Clinical_HistorySummary.HistoryCacheList.FamilyHx.length > 0) {
    //    //    Clinical_FamilyHx.CacheFamilyHxSave('', true, addToNote);
    //    //}

    //    //if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null && Clinical_HistorySummary.HistoryCacheList.SocialHx.length > 0) {
    //    //    Clinical_SocialHx.CacheSocialHxSave('', true, addToNote);
    //    //}

    //    //dfd.done(function () {
    //    //    var a = 10;
    //    //});

    //},

    resetListAccordingToUnremarkAble: function () {

        if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
            if (Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable) {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel = [];
            }
            else {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel == [] &&
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel == [] &&
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel == [] &&
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel == [] &&
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel == [] &&
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel == [] &&
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel == [] &&
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel == [] &&
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel == []) {
                    Clinical_HistorySummary.HistoryCacheList.SocialHx = null;
                }
            }
        }
        if (Clinical_HistorySummary.HistoryCacheList.MedicalHx != null) {
            if (Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxUnremarkable) {
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList = [];
            }
            else {
                if (Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList == []) {
                    Clinical_HistorySummary.HistoryCacheList.MedicalHx = null;
                }
            }
        }
        if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
            if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxUnremarkable) {
                Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList = [];
            }
            else {
                if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList == []) {
                    Clinical_HistorySummary.HistoryCacheList.SurgicalHx = null;
                }
            }
        }
        if (Clinical_HistorySummary.HistoryCacheList.FamilyHx != null) {
            if (Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyHxUnremarkable) {
                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease = [];
                Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail = [];
            }
            else {
                if (Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDisease == [] && Clinical_HistorySummary.HistoryCacheList.FamilyHx.FamilyMemberDiseaseDetail == []) {
                    Clinical_HistorySummary.HistoryCacheList.FamilyHx = null;
                }
            }
        }
        if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx != null) {
            if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationHxUnremarkable) {
                Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList = [];
            }
            else {
                if (Clinical_HistorySummary.HistoryCacheList.HospitalizationHx.HospitalizationDiseaseList == []) {
                    Clinical_HistorySummary.HistoryCacheList.HospitalizationHx = null;
                }
            }
        }
        if (Clinical_HistorySummary.HistoryCacheList.BirthHx != null && Clinical_HistorySummary.HistoryCacheList.BirthHx.length > 0) {
            if (Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxUnremarkable) {
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxGeneral = {};
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxMaternalDelivery = {};
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxNewborn = {};
            }
            else {
                if (Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxGeneral == {} && Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxMaternalDelivery == {} && Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxNewborn == {}) {
                    Clinical_HistorySummary.HistoryCacheList.BirthHx = null;
                }
            }
        }
    },

    aaddHistoriesToNote_DBCall: function (addToNote) {

        Clinical_HistorySummary.resetListAccordingToUnremarkAble();

        var objData = {};
        objData["HistoriesList"] = Clinical_HistorySummary.HistoryCacheList;

        objData["SocialHxList"] = Clinical_HistorySummary.HistoryCacheList.SocialHx;
        objData["MedicalHxList"] = Clinical_HistorySummary.HistoryCacheList.MedicalHx;
        objData["SurgicalHxList"] = Clinical_HistorySummary.HistoryCacheList.SurgicalHx;
        objData["FamilyHxList"] = Clinical_HistorySummary.HistoryCacheList.FamilyHx;
        objData["HospitalizationHxList"] = Clinical_HistorySummary.HistoryCacheList.HospitalizationHx;
        objData["SocPsyandBehaviorHx"] = Clinical_HistorySummary.HistoryCacheList.SocPsyandBehaviorHx;
        if (Clinical_HistorySummary.HistoryCacheList.BirthHx != null && Clinical_HistorySummary.HistoryCacheList.BirthHx.length > 0) {
            objData["BirthHxList"] = Clinical_HistorySummary.HistoryCacheList.BirthHx[0];
        }
        else {
            objData["BirthHxList"] = Clinical_HistorySummary.HistoryCacheList.BirthHx;
        }

        if (Clinical_HistorySummary.params.patientID == "" || Clinical_HistorySummary.params.patientID == "undefined") {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_HistorySummary.params.patientID;
        }
        if (addToNote == true) {
            objData["NoteId"] = Clinical_HistorySummary.params.NotesId;
        }
        else {
            objData["NoteId"] = 0;
        }
        objData["commandType"] = "savecachehistories";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");
    },

    closeHistories: function () {

        Clinical_HistorySummary.CacheActiveHistoryData();
        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
    },
}