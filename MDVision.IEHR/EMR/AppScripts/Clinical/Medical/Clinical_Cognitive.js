/*
Author: Muhammad Arshad
Date: 27/04/2016
Overview: This file is created to show Cognitive And Functional Status
*/

Clinical_Cognitive = {
    bIsFirstLoad: true,
    lastSelectedType: null,
    params: [],
    Load: function (params) {
        Clinical_Cognitive.params = params;

        if (Clinical_Cognitive.params.PanelID != 'pnlClinicalCognitive') {
            Clinical_Cognitive.params.PanelID = Clinical_Cognitive.params.PanelID + ' #pnlClinicalCognitive';
        } else {
            Clinical_Cognitive.params.PanelID = 'pnlClinicalCognitive';
        }
        if (Clinical_Cognitive.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_Cognitive.params.PanelID + " div#FaceSheetPager", Clinical_Cognitive.params.FaceSheetComponents, 'appointments');
        }
        var CognitiveId = "";
        if (Clinical_Cognitive.params.mode == "Add" || Clinical_Cognitive.params.CognitiveId == null || Clinical_Cognitive.params.CognitiveId == "" || Clinical_Cognitive.params.CognitiveId == "-1") {
            CognitiveId = "-1";
        }
        else if (Clinical_Cognitive.params.mode == "Edit") {
            CognitiveId = Clinical_Cognitive.params.CognitiveId;
            //Clinical_Cognitive.FamilyHxEdit(FamilyHxId);
        }

        var self = $('#' + Clinical_Cognitive.params.PanelID);

        self.loadDropDowns(true).done(function () {

            Clinical_Cognitive.loadCognitive(CognitiveId);
            Clinical_Cognitive.EnterEventForFreeTextInput();
        });

        //  Clinical_Cognitive.domReadyFunction();
        //Clinical_Cognitive.appointmentSearch();

        if (Clinical_Cognitive.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_Cognitive.params.PanelID + ' #btnAddToNote').removeClass('hidden');
        }
        else {
            $('#' + Clinical_Cognitive.params.PanelID + ' #btnAddToNote').addClass('hidden');
        }

        utility.callbackAfterAllDOMLoaded(function () {
            $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive').data('serialize', $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive').serialize());
        });

    },

    // Method Name: activeInActive
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle active/inactive of Goal li
    activeInActive: function () {
        $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive #ulCognitiveStatus li').on('click', function () {
            $(this).parent().find('li.active').removeClass('active');
            $(this).addClass('active');
            $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive #CognitiveDetails').removeClass('disableAll');
        });

    },

    // Method Name: loadCognitive
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle load of Cognitive
    loadCognitive: function (CognitiveId) {
        Clinical_Cognitive.fillCognitive(null, CognitiveId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    var goalLoadDetails = JSON.parse(response.StatusLoad_JSON);
                    var FucntionalStatusDetails = JSON.parse(response.FucntionalStatusLoad_JSON);
                    var MentalStatusDetails = JSON.parse(response.MentalStatusLoad_JSON);
                    var CognitiveDetail = JSON.parse(response.CognitiveFill_JSON);
                    Clinical_Cognitive.params["IsAttatchedWithNote"] = CognitiveDetail.IsAttatchedWithNote

                    if (goalLoadDetails.length > 0) {
                        var a = goalLoadDetails[0];
                        if (a.CognitiveId > 0) {
                            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #aHistory").removeClass('hidden');
                            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #aHistory").attr('onclick', 'Clinical_Cognitive.showHistory(' + a.CognitiveId + ')');
                        }
                    }
                    else if (FucntionalStatusDetails.length > 0) {
                        var b = FucntionalStatusDetails[0];
                        if (b.CognitiveId > 0) {
                            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #aHistory").removeClass('hidden');
                            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #aHistory").attr('onclick', 'Clinical_Cognitive.showHistory(' + b.CognitiveId + ')');
                        }

                    }

                    else if (MentalStatusDetails.length > 0) {
                        var b = MentalStatusDetails[0];
                        if (b.CognitiveId > 0) {
                            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #aHistory").removeClass('hidden');
                            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #aHistory").attr('onclick', 'Clinical_Cognitive.showHistory(' + b.CognitiveId + ')');
                        }
                    }

                    Clinical_Cognitive.loadCognitiveStatus('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulCognitiveStatus", goalLoadDetails, "Status");
                    Clinical_Cognitive.loadCognitiveStatus('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulFunctionalStatus", FucntionalStatusDetails, "FunctionalStatus");
                    Clinical_Cognitive.loadCognitiveStatus('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulMentalStatus", MentalStatusDetails, "MentalStatus");

                    
                    var self = $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive");
                    $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #txtCognitiveStatus").val(CognitiveDetail.Instruction);
                    //        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #txtFunctionalStatus").val(CognitiveDetail.FutureAppointments);
                    utility.bindMyJSONByName(true, CognitiveDetail, false, self).done(function () {
                        Clinical_Cognitive.params.mode = "Edit";
                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }

        });

    },

    // Method Name: fillCognitive
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle fill of Cognitive
    fillCognitive: function (CognitiveType, CognitiveId, goalId) {
        var objData = new Object();
        objData["CognitiveId"] = CognitiveId != null ? CognitiveId : 0;

        if (CognitiveType == "Status") {
            objData["CognitiveStatusId"] = goalId;
        }
        else {
            objData["CognitiveStatusId"] = 0;
        }

        if (CognitiveType == "FunctionalStatus") {
            objData["FunctionalStatusId"] = goalId;
        }
        else {
            objData["FunctionalStatusId"] = 0;
        }
        if (CognitiveType == "MentalStatus") {
            objData["MentalStatusId"] = goalId;
        }
        else {
            objData["MentalStatusId"] = 0;
        }

        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["IsFromNote"] = Clinical_Cognitive.params["IsFromNote"] != null ? Clinical_Cognitive.params["IsFromNote"] : false;
        objData["commandType"] = "Fill_Cognitive";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "Cognitive");
    },

    // Method Name: loadCognitiveGoal
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle load of Cognitive goal
    loadCognitiveStatus: function (crtl, result, statusType) {
        var currentLiClass = "";
        var currentLiClick = "";

        if ($(crtl).length > 0)
            l = $(crtl);
        l.empty();
        var isFirstLi = true;
        $.each(result, function (j, item) {
            // var status = statusType == "Status" ? item.StatusId : item.FunctionalStatusId;
            if (statusType == "Status") { status = item.StatusId; }
            else if (statusType == "MentalStatus") { status = item.MentalStatusId; }
            else { status = item.FunctionalStatusId; }
            var li = "<li  id=\"" + status + "\" onclick='Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + statusType + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + item.ICD9Code + "\" icd9Desc=\"" + item.ICD9CodeDescription + "\" icd10Code=\"" + item.ICD10Code + "\" icd10Desc=\"" + item.ICD10CodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\" detailsnote=\"" + item.Instruction + "\"><a>" + item.ICD9CodeDescription + "<span  class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event,\"" + statusType + "\");'><i class='fa fa-times'></i></span></a></li>"
            //var li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_Cognitive.fillMedicalHxDisease(this, event);' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Code=\"" + item.ICD9Code + "\" icd9Desc=\"" + item.ICD9CodeDescription + "\" icd10Code=\"" + item.ICD10Code + "\" icd10Desc=\"" + item.ICD10CodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\"><a href='#'>" + item.ICD9CodeDescription + "<div id='deleteIcon' style='display:none' class='pull-right' onclick='Clinical_Cognitive.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close red'></i></div></a></li>"
            l.append(li);
        });
    },
    // Method Name: resetValues
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle reset control values of Cognitive
    resetValues: function (StatusType) {
        var detailDiv = "";
        if (StatusType == "Status") {
            detailDiv = "CognitiveDetails";
        }
        else if (StatusType == "FunctionalStatus") {
            detailDiv = "FunctionalDetails";
        }
        else if (StatusType == "MentalStatus") {
            detailDiv = "MentalDetails";
        }

        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #" + detailDiv).find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
            $(this).val('');
        });

    },
    bindtextareavalue: function (obj, ev, StatusType) {
        if (ev != null)
            ev.stopPropagation();
        var hiddenField = "";
        var ulCtrl = "";
        var detailsDiv = "";
        if (StatusType == "Status") 
            ulCtrl = "ulCognitiveStatus";
        else if (StatusType == "FunctionalStatus")
            ulCtrl = "ulFunctionalStatus";
        else if (StatusType == "MentalStatus")
            ulCtrl = "ulMentalStatus";
        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive ul#" + ulCtrl).find("li.active").attr("detailsnote", $(obj).val());
    },

    // Method Name: fillCognitiveGoal
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle fill of Cognitive goal
    fillCognitiveGoal: function (obj, ev) {

        ev.stopPropagation();
        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #FamilyMemberDetails").removeClass("disableAll");
        var goalId = $(obj).attr('id');
        Clinical_Cognitive.resetValues();
        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfGoalId").val(goalId);

        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive ul#ulCognitiveGoal li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == goalId) {
                if ($(this).hasClass("active") == false) {
                    $(this).addClass("active");
                    $(this).find('div').css('display', '');
                }
            }
            else {
                $(this).removeClass("active");
                $(this).find('div').css('display', 'none');
            }
        });
        if (goalId > 0) {
            Clinical_Cognitive.loadCognitiveComponent('Status', goalId, true);
        }
    },
    // Method Name: loadCognitiveComponent
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle fill of Cognitive goal data
    loadCognitiveComponent: function (CognitiveType, goalId, isGoalFill, isNewLoad) {
        BackgroundLoaderShow(true);
        Clinical_Cognitive.fillCognitive(CognitiveType, null, goalId).done(function (response) {
            if (isNewLoad == true) {
                if ($('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfSelectedCognitiveStatus").val() != "" && $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfSelectedCognitiveStatus").val() != "undefined") {
                    var list = $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #ulCognitiveStatus li#" + $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfSelectedCognitiveStatus").val());
                    $(list).addClass('active');
                    $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #CognitiveDetails").removeClass("disableAll");
                    list.trigger('click');
                    return;
                }
            }
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var goalLoadDetails = JSON.parse(response.StatusLoad_JSON);
                    var Cognitivedetail = JSON.parse(response.CognitiveFill_JSON);
                    var goaldetail = JSON.parse(response.StatusFill_JSON);
                    var functionalStatusDetail = JSON.parse(response.FucntionalStatusFill_JSON);
                    var MentalStatusDetail = JSON.parse(response.MentalStatusFill_JSON);
                    //if (Cognitivedetail.CognitiveId > 0) {
                    //    $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #aHistory").removeClass('hidden');
                    //    $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #aHistory").attr('onclick', 'Clinical_Cognitive.showHistory(' + Cognitivedetail.CognitiveId + ')');
                    //}
                    var StatusNote = goaldetail.length > 0 ? goaldetail[0].Instruction : "";
                    var FunctionalStatusNote = functionalStatusDetail.length > 0 ? functionalStatusDetail[0].Instruction : "";
                    var MentalStatusNote = MentalStatusDetail.length > 0 ? MentalStatusDetail[0].Instruction : "";
                    if (CognitiveType == "Status") {
                        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #txtCognitiveStatusNote").val(StatusNote);
                    }
                    else if (CognitiveType == "MentalStatus") {
                        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #txtMentalStatusNote").val(MentalStatusNote);
                    }
                    else {
                        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #txtFunctionalStatusNote").val(FunctionalStatusNote);
                    }

                    Clinical_Cognitive.params.mode = "Edit";
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {

            }
            BackgroundLoaderShow(false);
        });
    },

    // Method Name: showIcon
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle show delete icon on Cognitive Goal li's focus
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },

    // Method Name: showIcon
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle show delete icon on Cognitive Goal li's focus out
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },

    // Method Name: deleteCognitiveGoal
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle delete of CognitiveGoal
    deleteCognitiveGoal: function (obj, ev) {

        var dfd = new $.Deferred();
        ev.stopPropagation();
        var diseaseId = $(obj).attr('id');
        if (diseaseId < 0) {
            $(obj).remove();
            $('#' + Clinical_Cognitive.params.PanelID + ' #sectionDiseaseDetails').resetAllControls(null);
            dfd.resolve();
        } else {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        utility.myConfirm('23', function () {
                            var selectedValue = diseaseId;
                            if (selectedValue == "" || selectedValue == "undefined") {
                            }
                            else {
                                Clinical_Cognitive.CognitiveGoalDelete(selectedValue).done(function (response) {

                                    response = JSON.parse(response);
                                    if (response.status != false) {

                                        $(obj).remove();
                                        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #txtInstructions").val('');
                                        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #FamilyMemberDetails").addClass('disableAll');
                                        dfd.resolve();
                                        utility.DisplayMessages(response.Message, 1);

                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                        }, function () { },
                            '23'
                        );
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
        }

        dfd.done(function () {
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #sectionDiseaseDetails").addClass('disableAll');
        });
    },

    // Method Name: CognitiveGoalDelete
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle DBCall for delete of CognitiveGoal
    CognitiveGoalDelete: function (goalId) {
        var objData = new Object();
        objData["GoalId"] = goalId;
        var CognitiveId = $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfCognitiveId").val();
        objData["CognitiveId"] = CognitiveId;
        objData["commandType"] = "DELETE_CognitiveGOAL";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "Cognitive");
    },

    // Method Name: isDetailExists
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will check whether detail exists in plan of care Detail section or not
    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";
        if (TabType != null && TabType != "") {
            if (TabType.toLowerCase() == "cognitive") {
                sectionDetails = "sectionCognitiveDetail";
            }
        }
        if (sectionDetails != "") {
            var self = $('#' + Clinical_Cognitive.params.PanelID + ' section#' + sectionDetails).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if ($(this).prop("disabled") != true) {
                    var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                    if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                        DetailExists = true;
                    }

                }
            });
        }

        return DetailExists;
    },

    isFunctionalDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";
        if (TabType != null && TabType != "") {
            if (TabType.toLowerCase() == "cognitive") {
                sectionDetails = "sectionFunctionalDetail";
            }
        }
        if (sectionDetails != "") {
            var self = $('#' + Clinical_Cognitive.params.PanelID + ' section#' + sectionDetails).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if ($(this).prop("disabled") != true) {
                    var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                    if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                        DetailExists = true;
                    }

                }
            });
        }

        return DetailExists;
    },

    isMentalDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";
        if (TabType != null && TabType != "") {
            if (TabType.toLowerCase() == "cognitive") {
                sectionDetails = "sectionMentalDetail";
            }
        }
        if (sectionDetails != "") {
            var self = $('#' + Clinical_Cognitive.params.PanelID + ' section#' + sectionDetails).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if ($(this).prop("disabled") != true) {
                    var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                    if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                        DetailExists = true;
                    }

                }
            });
        }

        return DetailExists;
    },

    // Method Name: CognitiveSave
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle Cognitive Save
    CognitiveSave: function (IsAddToNote, CognitiveType, UnloadMedicalhx, UnloadOrNot) {
        var dfd = $.Deferred();
        var CognitiveId = $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfCognitiveId").val() != "" ? $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfCognitiveId").val() : "-1";
        if (parseInt(CognitiveId) > 0) {
            Clinical_Cognitive.params.mode = "Edit";
        }
        else {
            Clinical_Cognitive.params.mode = "Add";
        }

        var DetailExists = false;
        DetailExists = Clinical_Cognitive.isDetailExists(CognitiveType.toLowerCase());
        functionalDetailExists = Clinical_Cognitive.isFunctionalDetailExists(CognitiveType.toLowerCase());
        MentalDetailExists = Clinical_Cognitive.isMentalDetailExists(CognitiveType.toLowerCase());

        var functionalLIs = $('#' + Clinical_Cognitive.params.PanelID + " ul#ulFunctionalStatus  li").length;
        var cognitiveLIs = $('#' + Clinical_Cognitive.params.PanelID + " ul#ulCognitiveStatus li").length;
        var MentalLIs = $('#' + Clinical_Cognitive.params.PanelID + " ul#ulMentalStatus li").length;
        if (functionalLIs > 0 || cognitiveLIs > 0 || MentalLIs > 0) {
            var strMessage = "";
            var self = null;
            var self2 = null;
            var Mentalself = null;
            if (CognitiveType.toLowerCase() == "cognitive") {
                self = $('#' + Clinical_Cognitive.params.PanelID + " section#sectionCognitiveDetail");
                self2 = $('#' + Clinical_Cognitive.params.PanelID + " section#sectionFunctionalDetail");
                Mentalself = $('#' + Clinical_Cognitive.params.PanelID + " section#sectionMentalDetail");
            }
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var myJSON2 = self2 != null ? self2.getMyJSONByName() : "{}";
            var MentalmyJSON = Mentalself != null ? Mentalself.getMyJSONByName() : "{}";

            var mergedJSON = utility.MergeJSON(myJSON, myJSON2, MentalmyJSON);

            var objData = JSON.parse(mergedJSON);

            objData["CognitiveStatusModel"] = [];
            objData["FunctionalStatusModel"] = [];
            objData["MentalStatusModel"] = [];

            if (CognitiveType.toLowerCase() == "cognitive") {

                $('#' + Clinical_Cognitive.params.PanelID + " ul#ulCognitiveStatus li").each(function (i, item) {
                    obj = {};
                    obj.CognitiveStatusId = $(this).attr("id");
                    obj.CognitiveStatusText = $(this).text();
                    obj.ICD9Code = $(this).attr("icd9code");
                    obj.ICD9CodeDescription = $(this).attr("icd9desc");
                    obj.ICD10Code = $(this).attr("icd10code");
                    obj.ICD10CodeDescription = $(this).attr("icd10desc");
                    obj.SNOMEDID = $(this).attr("snomedcode");
                    obj.SNOMEDDescription = $(this).attr("snomeddesc");
                    if ($(this).hasClass("active"))
                        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfSelectedCognitive").val($(this).attr("id"));
                    obj.Instruction = $(this).attr("detailsnote");
                    objData["CognitiveStatusModel"].push(obj);
                });

                $('#' + Clinical_Cognitive.params.PanelID + " ul#ulFunctionalStatus li").each(function (i, item) {
                    obj = {};
                    obj.FunctionalStatusId = $(this).attr("id");
                    obj.FunctionalStatusText = $(this).text();
                    obj.FunctionalStatusICD9Code = $(this).attr("icd9code");
                    obj.FunctionalStatusICD9CodeDescription = $(this).attr("icd9desc");
                    obj.FunctionalStatusICD10Code = $(this).attr("icd10code");
                    obj.FunctionalStatusICD10CodeDescription = $(this).attr("icd10desc");
                    obj.FunctionalStatusSNOMEDID = $(this).attr("snomedcode");
                    obj.FunctionalStatusSNOMEDDescription = $(this).attr("snomeddesc");
                    obj.FunctionalStatusInstruction = $(this).attr("detailsnote");;
                    objData["FunctionalStatusModel"].push(obj);
                });

                $('#' + Clinical_Cognitive.params.PanelID + " ul#ulMentalStatus li").each(function (i, item) {
                    obj = {};
                    obj.MentalStatusId = $(this).attr("id");
                    obj.MentalStatusText = $(this).text();
                    obj.MentalStatusICD9Code = $(this).attr("icd9code");
                    obj.MentalStatusICD9CodeDescription = $(this).attr("icd9desc");
                    obj.MentalStatusICD10Code = $(this).attr("icd10code");
                    obj.MentalStatusICD10CodeDescription = $(this).attr("icd10desc");
                    obj.MentalStatusSNOMEDID = $(this).attr("snomedcode");
                    obj.MentalStatusSNOMEDDescription = $(this).attr("snomeddesc");
                    obj.FreeTextICD = $(this).attr("FreeTextICD");
                    obj.MentalStatusInstruction = $(this).attr("detailsnote");

                    objData["MentalStatusModel"].push(obj);
                });
            }

            objData["CognitiveId"] = $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfCognitiveId").val();
            objData["CognitiveType"] = CognitiveType != null ? CognitiveType : "";

            mergedJSON = JSON.stringify(objData);

            if (Clinical_Cognitive.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_Cognitive.saveCognitive(IsAddToNote, mergedJSON).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                Clinical_Cognitive.SetItemIds(response);


                                $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfCognitiveId").val(response.cognitiveId);
                                Clinical_Cognitive.params.mode = "Edit";
                                if (Clinical_Cognitive.params.ParentCtrl == "clinicalTabProgressNote" && IsAddToNote == true) {
                                    $.when(Clinical_Cognitive.getCognitiveInfo(CognitiveType, UnloadOrNot, response.cognitiveId, true)).then(function () {
                                        dfd.resolve();
                                    });
                                }
                                else {
                                    dfd.resolve();
                                }
                                utility.DisplayMessages(response.message, 1);

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
            else if (Clinical_Cognitive.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_Cognitive.updateCognitive(IsAddToNote, mergedJSON, Clinical_Cognitive.params.CognitiveId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                Clinical_Cognitive.SetItemIds(response);
                                $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfCognitiveId").val(response.cognitiveId);

                                if (Clinical_Cognitive.params.ParentCtrl == "clinicalTabProgressNote" && IsAddToNote == true) {
                                    $.when(Clinical_Cognitive.getCognitiveInfo(CognitiveType, UnloadOrNot)).then(function () {
                                        dfd.resolve();
                                    });
                                } else {
                                    utility.DisplayMessages(response.message, 1);
                                    dfd.resolve();
                                }

                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
            }
        }
        else {
            utility.DisplayMessages("Please enter any value", 3);
        }

        return dfd.then(function () {
            $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive').data('serialize', $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive').serialize());
        });
    },

    SetItemIds: function (response) {

        Cognitive_List = JSON.parse(response.Cognitive_Status);
        Functional_List = JSON.parse(response.Functional_Status);
        MentalStatus_List = JSON.parse(response.Mental_Status);

        if (Cognitive_List.length > 0) {
            $.each(Cognitive_List, function (i, item) {

                var $item = $('#' + Clinical_Cognitive.params.PanelID
                     + " ul#ulCognitiveStatus li[icd9code*='" + item.ICD9Code + "'][icd10code*='" + item.ICD10Code + "'][icd9desc*='" + item.ICD9CodeDescription + "'][icd10desc*='" + item.ICD10CodeDescription + "']");
                if ($item.length > 0) {
                    $($item).attr("id", item.StatusId);
                }
            });


        }

        if (Functional_List.length > 0) {
            $.each(Functional_List, function (i, item) {

                var $item = $('#' + Clinical_Cognitive.params.PanelID
                     + " ul#ulFunctionalStatus li[icd9code*='" + item.ICD9Code + "'][icd10code*='" + item.ICD10Code + "'][icd9desc*='" + item.ICD9CodeDescription + "'][icd10desc*='" + item.ICD10CodeDescription + "']");
                if ($item.length > 0) {
                    $($item).attr("id", item.FunctionalStatusId);
                }
            });
        }

        if (MentalStatus_List.length > 0) {
            var $item;
            $.each(MentalStatus_List, function (i, item) {
                if ($('#' + Clinical_Cognitive.params.PanelID + " ul#ulMentalStatus li[icd9desc*='" + item.ICD9CodeDescription + "']").attr("freetexticd"))
                    $item = $('#' + Clinical_Cognitive.params.PanelID + " ul#ulMentalStatus li[icd9desc*='" + item.ICD9CodeDescription + "']");
                else
                    $item = $('#' + Clinical_Cognitive.params.PanelID
                     + " ul#ulMentalStatus li[icd9code*='" + item.ICD9Code + "'][icd10code*='" + item.ICD10Code + "'][icd9desc*='" + item.ICD9CodeDescription + "'][icd10desc*='" + item.ICD10CodeDescription + "']");
                if ($item && $item.length > 0) {
                    $($item).attr("id", item.MentalStatusId);
                }
            });
        }
    },

    // Method Name: updateCognitive
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle DBCall for Cognitive update
    updateCognitive: function (IsAddToNote, CognitiveData, CognitiveId) {

        var objData = JSON.parse(CognitiveData);
        if (Clinical_Cognitive.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_Cognitive.params.patientID;
        }
        objData["IsFromNote"] = IsAddToNote;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "UPDATE_Cognitive";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "Cognitive");
    },

    // Method Name: updateCognitive
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle DBCall for Cognitive save
    saveCognitive: function (IsAddToNote, CognitiveData) {
        var objData = JSON.parse(CognitiveData);
        if (Clinical_Cognitive.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_Cognitive.params.patientID;
        }
        objData["IsFromNote"] = IsAddToNote;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "SAVE_Cognitive";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "Cognitive");
    },

    // Method Name: bindICD9AutoComplete
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle autocomplete of plan of care goal
    bindICD9AutoComplete: function (element) {
        if (element.id == 'txtFunctionalStatus') {
            $('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val('txtFunctionalStatus');
            $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus').attr("data-popupunload", "false");
        }
        else if (element.id == 'txtMentalStatus') {
            $('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val('txtMentalStatus');
            $('#' + Clinical_Cognitive.params.PanelID + ' #txtMentalStatus').attr("data-popupunload", "false");
        }
        else {
            $('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val('txtCognitiveStatus');
            $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus').attr("data-popupunload", "false");
        }



        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_Cognitive", null, false);
    },

    changeProcedureField: function () {

        if ($('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #rdofreetext").prop("checked") == true) {
            Clinical_Cognitive.lastSelectedType = "FreeText";
            //$('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtDisease").addClass('hidden');
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #DivICDAutoComplete").addClass('hidden');
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #btnsearchcpt").addClass('hidden');
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #txtFreeText").removeClass('hidden');
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #DivFreeText").removeClass('hidden');
        } else {
            Clinical_Cognitive.lastSelectedType = "CPT";
            //   $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtDisease").removeClass('hidden');
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #btnsearchcpt").removeClass('hidden');
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #txtFreeText").addClass('hidden');
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #DivFreeText").addClass('hidden');
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #DivICDAutoComplete").removeClass('hidden');
        }

        Clinical_Cognitive.SaveFreeTextStatus();

    },

    SaveFreeTextStatus: function () {
        var panel = "";
        if (Clinical_Cognitive.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #" + Clinical_Cognitive.params.PanelID;
        }
        else {
            panel = "#" + Clinical_Cognitive.params.PanelID;
        }
        var IsFreeText = false;
        $('input[name=rdoprocedure]:checked', panel).val() == 1 ? IsFreeText = true : IsFreeText = false;
        EMRUtility.insertUpdateFreeTextStatus("Clinical_Cognitive", IsFreeText);
    },

    // Method Name: showIcon
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle show delete icon on PlanofCare Goal li's focus
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },

    // Method Name: showIcon
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle show delete icon on PlanofCare Goal li's focus out
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },


    // Method Name: OpenSearchPopup
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will open ICD popup to select
    OpenSearchPopup: function (SearchType, Ctrl, HiddenCtrl, element) {

        if (element.id == 'btnCognitiveStatus') {

            $('#' + Clinical_Cognitive.params.PanelID + ' #txtCognitiveStatus').attr('data-popupunload', 'true');

            $('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val('txtCognitiveStatus');
        }
        else if (element.id == 'btnMentalStatus') {
            $('#' + Clinical_Cognitive.params.PanelID + ' #txtMentalStatus').attr('data-popupunload', 'true');

            $('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val('txtMentalStatus');
        }
        else {
            $('#' + Clinical_Cognitive.params.PanelID + ' #txtFunctionalStatus').attr('data-popupunload', 'true');

            $('#' + Clinical_Cognitive.params.PanelID + ' #hfCtrl').val('txtFunctionalStatus');
        }


        var controlToLoad = "";
        if (SearchType == "ICD") {

            controlToLoad = "Admin_IMOICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (SearchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        var params = [];
        params["FromAdmin"] = "0";
        //params["ParentCtrl"] = Clinical_Cognitive.params["TabID"];
        if (Clinical_Cognitive.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_Cognitive';
        }
        else {
            params["ParentCtrl"] = Clinical_Cognitive.params["TabID"];
        }

        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Clinical_Cognitive.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },

    bindFreeText: function () {
        var freeText = $.trim($('#' + Clinical_Cognitive.params.PanelID + ' #txtFreeText').val());
        var MentalStatus = "MentalStatus";
        if (freeText.length > 0) {
            var currId = -1;
            $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive ul#ulMentalStatus li[id*='-']").each(function (i, item) {
                currId = $(this).attr("id");
            });
            currId = parseInt(currId) + (-1);
            //var li = "<li  id=" + currId + " onclick='Clinical_HospitalizationHx.fillHospitalizationHxDisease(this, event);' onmouseover='Clinical_HospitalizationHx.showIcon(this);' onmouseout='Clinical_HospitalizationHx.hideIcon(this);' icd9Desc=\"" + freeText + "\" freeText=\"" + freeText + "\"><a href='#'>" + freeText + "<span class='removeIconListHover' onclick='Clinical_HospitalizationHx.deleteHospitalizationHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            var li = "<li  id=" + currId + " onclick = 'Clinical_Cognitive.fillCognitiveStatus(this, event,\"" + MentalStatus + "\");' onmouseover='Clinical_Cognitive.showIcon(this);' onmouseout='Clinical_Cognitive.hideIcon(this);' icd9Desc=\"" + freeText +
                "\" FreeTextICD=\"" + freeText + "\">  <a onclick='Clinical_Cognitive.activeInActiveMentalStatus($(this), event);'>" + freeText + "<span class='removeIconListHover' onclick='Clinical_Cognitive.deleteCognitiveStatus($($(this).parent()).parent(), event ,\"" + MentalStatus + "\");'><i class='fa fa-close'></i></span></a></li>"
            var IsAlreadyExist = false;
            $('#' + Clinical_Cognitive.params.PanelID + ' ul#ulMentalStatus li').each(function () {
                if ($(this).attr('icd9Desc').toLowerCase() == freeText.toLowerCase()) {
                    IsAlreadyExist = true;
                }
            });
            if (!IsAlreadyExist) {
                $('#' + Clinical_Cognitive.params.PanelID + ' #ulMentalStatus').append(li);
                $(li).trigger('click');

                //var diseaseId = $('#' + Clinical_HospitalizationHx.params.PanelID + " #ulHospitalizationDisease > li.active").attr('id');
                //var disease = $(li).get(0).outerHTML;
                //var diseaseData = $('#' + Clinical_HospitalizationHx.params.PanelID + " #sectionHospitalizationDetails").getMyJSONByName();
                //Clinical_HospitalizationHx.cacheHospitalizationHxJSON(diseaseId, diseaseData, disease);

            }
            else {
                utility.DisplayMessages('Diagnosis already added', 2);
            }
        }
        $('#' + Clinical_Cognitive.params.PanelID + ' #txtFreeText').val('');
    },

    EnterEventForFreeTextInput: function () {
        $("#" + Clinical_Cognitive.params.PanelID + " #txtFreeText").on('keyup', function (e) {
            if (e.keyCode == 13) {
                Clinical_Cognitive.bindFreeText();

            }
        });
    },

    // Method Name: appointmentSearch
    // Author: Muhammad Arshad
    // Date: 01/04/2016
    // Description: This function will handle appointmentSearch
    appointmentSearch: function (PageNo, rpp) {
        Clinical_Cognitive.searchAppointments(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Cognitive.appointmentGridLoad(response);
                var TableControl = "Clinical_Cognitive #dgvClinical_Cognitive";
                var PagingPanelControlID = "Clinical_Cognitive #divFaceSheetAppointmentPaging";
                var ClassControlName = "Clinical_Cognitive";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AppointmentsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    Clinical_Cognitive.appointmentSearch(PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });
    },

    // Method Name: appointmentGridLoad
    // Author: Muhammad Arshad
    // Date: 01/04/2016
    // Description: This function will handle appointment grid load
    appointmentGridLoad: function (response) {
        $("#" + Clinical_Cognitive.params.PanelID + " #dgvClinical_Cognitive").dataTable().fnDestroy();
        $("#" + Clinical_Cognitive.params.PanelID + " #dgvClinical_Cognitive tbody").find("tr").remove();
        if (response.AppointmentsCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.AppointmentsLoad_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td>' + utility.RemoveTimeFromDate(null, item.ScheduleDate) + '</td><td>' + item.SchReason + '</td><td>' + item.ProviderName + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#" + Clinical_Cognitive.params.PanelID + " #dgvClinical_Cognitive tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_Cognitive.params.PanelID + " #divFaceSheetAppointmentPaging").css("display", "none");
            $("#" + Clinical_Cognitive.params.PanelID + " #dgvClinical_Cognitive").DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_Cognitive.params.PanelID + " #dgvClinical_Cognitive"))
            ;
        else
            $("#" + Clinical_Cognitive.params.PanelID + " #dgvClinical_Cognitive").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $('.table-responsive').css('min-height', '220px');
    },

    // Method Name: searchAppointments
    // Author: Muhammad Arshad
    // Date: 01/04/2016
    // Description: This function will handle DBCall for appointment search
    searchAppointments: function (PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();
        objData["commandType"] = "Search_FaceSheet_Appointments";
        return MDVisionService.APIService(objData, "FaceSheet", "FaceSheet");

    },

    //-----------------Progress Note-------------
    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Method Name: addCognitiveToNotes
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: Call Back function to add component to Progress Note

    //Method Name: getCognitiveInfo
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: this function will get Plan Of Care Soap Text and attach that to Progress note
    getCognitiveInfo: function (CognitiveType, unloadCognitive, CognitiveId, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_Cognitive.fillCognitive(CognitiveType, CognitiveId, null).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    $.when(Clinical_Cognitive.createCognitiveBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', unloadCognitive, hideAlertMessage)).then(function () {
                        UnloadActionPan(Clinical_Cognitive.params.ParentCtrl, 'Clinical_Cognitive');
                        dfd.resolve();
                    });
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(strMessage, 3);
                }
            }
            else {
                dfd.resolve();
            }
        });
        return dfd;
    },
    //Method Name: checkCognitiveExists
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This Function will check, if Medical History Soap is already attached in Progress note, if Medical History is not attached than it will create main divs to attach allergy
    checkCognitiveExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_functionalandcognitive').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #MiscellaneousNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';
            var onClick = 'Clinical_ProgressNote.SelectNotesComponentTab(\'FunctionalAndCognitive\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');';
            (globalAppdata["isConsolidatedCDACreationPreformance"] && globalAppdata["isConsolidatedCDACreationPreformance"].toLowerCase() == "false") ? onClick = "" : "";
            $(CompnentSelector).append(' <li class="FunctionalAndCognitiveComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_functionalandcognitive title="Functional And Cognitive"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onClick=' + onClick + ' title="Functional And Cognitive">Functional And Cognitive</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'FunctionalAndCognitive\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'FunctionalAndCognitive\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_functionalandcognitive> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_functionalandcognitive').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    //Method Name: createCognitiveBodyHTML
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This Function is used to create SOAP html and append it to  Progress note
    createCognitiveBodyHTML: function (response, NoteHTMLCtrl, unloadCognitive, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_Cognitive.checkCognitiveExists();
        if (response.CognitiveFill_JSON != null && response.CognitiveFill_JSON != '') {
            var CognitiveFill_Obj = JSON.parse(response.CognitiveFill_JSON);
            var $mainDivCognitive = $(document.createElement('div'));

            var CognitiveId = CognitiveFill_Obj.CognitiveId;
            if (CognitiveId > 0) {
                var $SectionBodyCognitive = $(document.createElement('section'));
                $SectionBodyCognitive.attr('id', "Cli_Cognitive_Main" + CognitiveId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Cognitive_" + CognitiveId);
                var $ListCognitive = $(document.createElement('ul'));

                $ListCognitive.attr('class', 'list-unstyled')

                $SectionBodyCognitive.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Cognitive_" + CognitiveId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Cognitive_Main" + CognitiveId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListCognitive.append("<li>" + CognitiveFill_Obj.CognitiveSoapText + "</li>");
                $DetailsDiv.append($ListCognitive);
                $SectionBodyCognitive.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_functionalandcognitive').parent().parent().find('#Cli_Cognitive_Main' + CognitiveId).length == 0) {
                    $mainDivCognitive.append($SectionBodyCognitive);
                    $.when(Clinical_Cognitive.updateCognitiveHtml($mainDivCognitive.html(), CognitiveId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                        dfd.resolve();
                    });
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_functionalandcognitive').parent().parent().find('#Cli_Cognitive_Main' + CognitiveId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_functionalandcognitive').parent().parent().find('#Cli_Cognitive_Main' + CognitiveId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_functionalandcognitive').parent().parent().find('#Cli_Cognitive_Main' + CognitiveId).html($SectionBodyCognitive.html());
                    $(NoteHTMLCtrl + ' clinical_functionalandcognitive').parent().parent().find('#Cli_Cognitive_Main' + CognitiveId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText('Functional And Cognitive', hideAlertMessage);
                    $.when(Clinical_Cognitive.updateCognitiveHtml("", CognitiveId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                        dfd.resolve();
                    });

                }

                if (unloadCognitive == true) {
                    Clinical_Cognitive.UnLoad(Clinical_Cognitive.bNextPrev);
                }
            }
            else {
                dfd.resolve();
            }
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    //Method Name: updateCognitiveHtml
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This Function is called by Progress Notes (Fill Cognitive Func, CopyAllNotesCategories)
    updateCognitiveHtml: function (CognitiveHtml, CognitiveId, NoteHTMLCtrl, hideAlertMessage) {
        var dfd = $.Deferred();
        $(NoteHTMLCtrl + ' clinical_functionalandcognitive').parent().parent().addClass('initialVisitBody');
        if (CognitiveHtml != '') {
            $(NoteHTMLCtrl + ' clinical_functionalandcognitive').parent().parent().append(CognitiveHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (CognitiveHtml != '') {
            $.when(Clinical_Cognitive.attachCognitiveFromNotes(CognitiveId, hideAlertMessage)).then(function () {
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    //Method Name: detach_ComponentsCognitive
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This Function detach Medical History From progress note
    detach_ComponentsCognitive: function (ComponentName, IsUpdate, CognitiveComponentRemove) {

        var Clinical_CognitiveIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_functionalandcognitive').parent().parent().find('section[id*="Cli_Cognitive_Main"]').map(function () {
            return this.id.replace("Cli_Cognitive_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .FunctionalAndCognitiveComponent').attr('NoteComponentId');
        if (CognitiveComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_functionalandcognitive').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Functional And Cognitive', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_functionalandcognitive').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Functional And Cognitive']").remove();
                });
            }
            else {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_functionalandcognitive').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Functional And Cognitive', true))
                }
                else {
                    if (NoteComponentId && NoteComponentId != "NCDummyId")
                        promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                    else {
                        var def = $.Deferred();
                        promise.push(def);
                        def.resolve();
                    }
                }
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_functionalandcognitive').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_functionalandcognitive').parent().parent().find('section[id*="Cli_Cognitive_Main"]').remove();
        }

        if (Clinical_CognitiveIds == "" || Clinical_CognitiveIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_functionalandcognitive').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Functional And Cognitive', true))
            }
            else {
                if (NoteComponentId && NoteComponentId != "NCDummyId")
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
            }
            $.when.apply($, promise).done(function () {
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_functionalandcognitive').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                utility.DisplayMessages('Successfully Deleted', 1);
            });
        }
        else {
            Clinical_Cognitive.detachCognitiveFromNotes_DBCall(Clinical_CognitiveIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Functional And Cognitive', true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },


    //Method Name: AttachCognitiveFromNotes
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //This Functions attached Medical Hx to Progress Note for current Patient Selected
    attachCognitiveFromNotes: function (CognitiveId, hideAlertMessage) {
        var dfd = $.Deferred();
        var strMessage = "";
        if (strMessage == "") {
            var selectedValue = CognitiveId;
            if (selectedValue == "" || selectedValue == "undefined") {
                dfd.resolve();
            }
            else {
                Clinical_Cognitive.AttachCognitiveFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $.when(Clinical_ProgressNote.saveComponentSOAPText('Functional And Cognitive', hideAlertMessage)).then(function () {
                            $('#' + CognitiveId).remove();
                            dfd.resolve();
                        });

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        dfd.resolve();
                    }
                });
            }
        }
        else {
            utility.DisplayMessages(strMessage, 2);
            dfd.resolve();
        }
        return dfd;
    },

    //Method Name: getLatestCognitiveByPatientId
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description:If Cognitive Component which is droped in Progress note has no Cognitive attached, than it will call for Latest Cognitive for this patient
    getLatestCognitiveByPatientId: function (hideAlertMessage) {
        var strMessage = '';
        if (strMessage == "") {
            Clinical_Cognitive.getLatestClinical_CognitiveByPatientId_DBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_Cognitive.createCognitiveBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }

            });
        }
        else {
            utility.DisplayMessages(strMessage, 3);
        }
    },


    //Method Name: AttachCognitiveFromNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This function will handle DBCall to attach Plan of Care from Notes
    AttachCognitiveFromNotes_DBCall: function (CognitiveId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["CognitiveId"] = CognitiveId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "attach_cognitive_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ClinicalSummary", "Cognitive");
    },

    //Method Name: getLatestClinical_CognitiveByPatientId_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This function will handle DBCall to get latest Plan of Care by patient Id
    getLatestClinical_CognitiveByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }

        objData["IsFromNote"] = Clinical_Cognitive.params["IsFromNote"] != null ? Clinical_Cognitive.params["IsFromNote"] : false;
        objData["commandType"] = "getlatest_Cognitiveby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "Cognitive");
    },


    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to Unload screen
    */
    UnLoad: function () {

        var objDeffered = $.Deferred();
        if (EMRUtility.compareFormDataWithSerialized(Clinical_Cognitive.params.PanelID + ' #frmCognitive')) {
            utility.myConfirmNote('1', function () {
                $.when(Clinical_Cognitive.CognitiveSave(true, 'cognitive', null, false)).then(function () {
                    UnloadActionPan(Clinical_Cognitive.params.ParentCtrl, 'Clinical_Cognitive');
                    objDeffered.resolve();
                    return objDeffered;
                });
            }, "",
             function () {
                 UnloadActionPan(Clinical_Cognitive.params.ParentCtrl, 'Clinical_Cognitive');
                 objDeffered.resolve();
                 return objDeffered;
             },
            '');
        } else {
            if (Clinical_Cognitive.params.FromAdmin == "0") {
                UnloadActionPan(Clinical_Cognitive.params.ParentCtrl, 'Clinical_Cognitive');
            }
            else {
                RemoveAdminTab();
            }
            objDeffered.resolve();
            return objDeffered;
        }
    },

    //Method Name: showHistory
    //Author Name: Ahmad Raza
    //Created Date: 20-04-2016
    //Description: This function will show Cognitive History
    showHistory: function (CognitiveId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var TabId = Clinical_Cognitive.params.TabID;
        if (Clinical_Cognitive.params.ParentCtrl == "clinicalTabProgressNote") {
            TabId = 'Clinical_Cognitive';
        }
        EMRUtility.showCurrentItemHistory(Clinical_Cognitive.params.PanelID, null, null, "Cognitive,Cognitive_Status,Cognitive_FunctionalStatus,Cognitive_MentalStatus", null, TabId, CognitiveId);
    },

    fillCognitiveStatus: function (obj, ev, StatusType) {

        ev.stopPropagation();

        $("#" + Clinical_Cognitive.params.PanelID + " #frmCognitive").bootstrapValidator('resetForm', true);

        var goalId = $(obj).attr('id');

        var hiddenField = "";
        var ulCtrl = "";
        var detailsDiv = "";
        var detailstxtArea = "";

        if (StatusType == "Status") {
            hiddenField = "hfCognitiveStatusId";
            ulCtrl = "ulCognitiveStatus";
            detailsDiv = "CognitiveDetails";
            detailstxtArea = "txtCognitiveStatusNote";
        }
        else if (StatusType == "FunctionalStatus") {
            hiddenField = "hfFunctionalStatusId";
            ulCtrl = "ulFunctionalStatus";
            detailsDiv = "FunctionalDetails";
            detailstxtArea = "txtFunctionalStatusNote";
        }

        else if (StatusType == "MentalStatus") {
            hiddenField = "hfMentalStatusId";
            ulCtrl = "ulMentalStatus";
            detailsDiv = "MentalDetails";
            detailstxtArea = "txtMentalStatusNote";
        }

        Clinical_Cognitive.resetValues(StatusType);
        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #" + detailsDiv).removeClass("disableAll");
        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #" + hiddenField).val(goalId);

        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive ul#" + ulCtrl + " li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == goalId) {
                if ($(this).hasClass("active") == false) {
                    $(this).addClass("active");
                    $(this).find('div').css('display', '');
                }
            }
            else {
                $(this).removeClass("active");
                $(this).find('div').css('display', 'none');
            }
        });
        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #" + detailstxtArea).val($('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive ul#" + ulCtrl).find("li.active").attr("detailsnote"));
        if (goalId > 0) {
            Clinical_Cognitive.loadCognitiveComponent(StatusType, goalId, true);
        }
    },

    fillFunctionalStatusGoal: function (obj, ev) {

        //ev.stopPropagation();
        //$('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #FamilyMemberDetails").removeClass("disableAll");
        //var goalId = $(obj).attr('id');
        //Clinical_PlanOfCare.resetValues();
        //$('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfGoalId").val(goalId);

        //$('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare ul#ulPlanOfCareGoal li").each(function (i, item) {
        //    if ($(this).attr("id") != null && $(this).attr("id") == goalId) {
        //        if ($(this).hasClass("active") == false) {
        //            $(this).addClass("active");
        //            $(this).find('div').css('display', '');
        //        }
        //    }
        //    else {
        //        $(this).removeClass("active");
        //        $(this).find('div').css('display', 'none');
        //    }
        //});
        //if (goalId > 0) {
        //    Clinical_PlanOfCare.loadPlanOfCareComponent('goal', goalId, true);
        //}
    },

    activeInActiveCognitiveStatus: function () {
        $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive #ulCognitiveStatus li').on('click', function () {
            $(this).parent().find('li.active').removeClass('active');
            $(this).addClass('active');
            $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive #CognitiveDetails').removeClass('disableAll');
        });

    },

    activeInActiveFunctionalStatus: function () {
        $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive #ulFunctionalStatus li').on('click', function () {
            $(this).parent().find('li.active').removeClass('active');
            $(this).addClass('active');
            $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive #FunctionalDetails').removeClass('disableAll');
        });

    },

    activeInActiveMentalStatus: function () {
        $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive #ulMentalStatus li').on('click', function () {
            $(this).parent().find('li.active').removeClass('active');
            $(this).addClass('active');
            $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive #FunctionalDetails').removeClass('disableAll');
        });

    },

    deleteCognitiveStatus: function (obj, ev, StatusType) {
        var divDetails = "";
        if (StatusType == "Status") {
            divDetails = " #sectionCognitiveDetail";
        }
        else if (StatusType == "FunctionalStatus") {
            divDetails = " #sectionFunctionalDetail";
        }
        else if (StatusType == "MentalStatus") {
            divDetails = " #sectionMentalDetail";
        }
        var dfd = new $.Deferred();
        ev.stopPropagation();
        var diseaseId = $(obj).attr('id');
        if (diseaseId < 0) {
            $(obj).remove();
            $('#' + Clinical_Cognitive.params.PanelID + divDetails).resetAllControls(null);
            $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive').data('serialize', $('#' + Clinical_Cognitive.params.PanelID + ' #frmCognitive').serialize());
            dfd.resolve();
        }
        else {
            if (Clinical_Cognitive.params["IsAttatchedWithNote"] && Clinical_Cognitive.params["IsAttatchedWithNote"].toLowerCase() == 'true') {
                utility.DisplayMessages("Record Associated with Notes so it Cannot be deleted.", 3);
                dfd.resolve();
            }
            else {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        utility.myConfirm('23', function () {
                            var selectedValue = diseaseId;
                            if (selectedValue == "" || selectedValue == "undefined") {
                            }
                            else {
                                Clinical_Cognitive.cognitiveStatusDelete(selectedValue, StatusType).done(function (response) {

                                    response = JSON.parse(response);
                                    if (response.status != false) {
                                        if (StatusType == "Status") {
                                            divDetails = " #sectionCognitiveDetail";
                                        }
                                        else if (StatusType == "FunctionalStatus") {
                                            divDetails = " #sectionFunctionalDetail";
                                        }
                                        else if (StatusType == "MentalStatus") {
                                            divDetails = " #sectionMentalDetail";
                                        }

                                        $(obj).remove();
                                        $('#' + Clinical_Cognitive.params.PanelID + divDetails).resetAllControls(null);
                                        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #txtCognitiveStatus").val('');
                                        $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #CognitiveDetails").addClass('disableAll');
                                        dfd.resolve();
                                        utility.DisplayMessages(response.Message, 1);
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                        dfd.resolve();
                                    }
                                });
                            }
                        }, function () { },
                            '23'
                        );
                    }
                    else {
                        utility.DisplayMessages(strMessage, 2);
                        dfd.resolve();
                    }

                });
            }
        }

        dfd.done(function () {
            $('#' + Clinical_Cognitive.params.PanelID + " #frmPlanOfCare #sectionDiseaseDetails").addClass('disableAll');
        });
        return dfd;
    },

    cognitiveStatusDelete: function (goalId, StatusType) {
        var objData = new Object();

        var cognitiveId = $('#' + Clinical_Cognitive.params.PanelID + " #frmCognitive #hfCognitiveId").val();
        objData["CognitiveId"] = cognitiveId;
        if (StatusType == "Status") {
            objData["CognitiveStatusId"] = goalId;
            objData["commandType"] = "DELETE_cognitiveStatus";
        }
        else if (StatusType == "FunctionalStatus") {
            objData["FunctionalStatusId"] = goalId;
            objData["commandType"] = "DELETE_FUNCTIONALSTATUS";
        }

        else if (StatusType == "MentalStatus") {
            objData["MentalStatusId"] = goalId;
            objData["commandType"] = "DELETE_MENTALSTATUS";
        }

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "Cognitive");
    },

    deleteFunctionalStatus: function (obj, ev) {

        var dfd = new $.Deferred();
        ev.stopPropagation();
        var diseaseId = $(obj).attr('id');
        if (diseaseId < 0) {
            $(obj).remove();
            //  $('#' + Clinical_PlanOfCare.params.PanelID + ' #sectionDiseaseDetails').resetAllControls(null);
            dfd.resolve();
        }
        //else {
        //    AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //        if (strMessage == "") {
        //            utility.myConfirm('23', function () {
        //                var selectedValue = diseaseId;
        //                if (selectedValue == "" || selectedValue == "undefined") {
        //                }
        //                else {
        //                    Clinical_PlanOfCare.planOfCareGoalDelete(selectedValue).done(function (response) {

        //                        response = JSON.parse(response);
        //                        if (response.status != false) {

        //                            $(obj).remove();
        //                            $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtInstructions").val('');
        //                            $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #FamilyMemberDetails").addClass('disableAll');
        //                            dfd.resolve();
        //                            utility.DisplayMessages(response.Message, 1);

        //                        }
        //                        else {
        //                            utility.DisplayMessages(response.Message, 3);
        //                        }
        //                    });
        //                }
        //            }, function () { },
        //                '23'
        //            );
        //        }
        //        else
        //            utility.DisplayMessages(strMessage, 2);
        //    });
        //}

        //dfd.done(function () {
        //    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #sectionDiseaseDetails").addClass('disableAll');
        //});
    },

    detachCognitiveFromNotes: function (CognitiveId) {
        var strMessage = "";
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_cognitive');
                var selectedValue = CognitiveId.replace('Cli_Cognitive_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_Cognitive.detachCognitiveFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + CognitiveId).remove();

                            Clinical_ProgressNote.saveComponentSOAPText('Functional And Cognitive');
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () { },
                '1'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
    },

    detachCognitiveFromNotes_DBCall: function (CognitiveId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["CognitiveId"] = CognitiveId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "detach_Cognitive_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ClinicalSummary", "Cognitive");
    },




}