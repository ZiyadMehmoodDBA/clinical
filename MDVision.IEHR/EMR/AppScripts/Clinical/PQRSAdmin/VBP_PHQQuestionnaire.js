//Author: Muhammad Arshad   
//File:   VBP_PHQQuestionnaire
//Date:   April 24, 2017
VBP_PHQQuestionnaire = {
    params: [],
    bIsFirstLoad: true,
    arrMeasureQuestionnaireAnswers: [],
    arrMeasureQuestions: [],
    Load: function (params) {
        VBP_PHQQuestionnaire.params = params;
        if (VBP_PHQQuestionnaire.params.PanelID != 'pnlPHQQuestionnaire') {
            VBP_PHQQuestionnaire.params.PanelID = VBP_PHQQuestionnaire.params.PanelID + ' #pnlPHQQuestionnaire';
        }
        else {
            VBP_PHQQuestionnaire.params.PanelID = 'pnlPHQQuestionnaire';
        }

        $.when(VBP_PHQQuestionnaire.GetVBPMeasureQuestionnaireAnswers("PHQ9")).then(function () {
            var selectedOptions = $("#" + VBP_PHQQuestionnaire.params["PanelID"] + " select:lt(2)");
            if (VBP_PHQQuestionnaire.params.PHQ2Answers4PHQ9_Params && VBP_PHQQuestionnaire.params.PHQ2Answers4PHQ9_Params.length > 0 && selectedOptions.length > 0) {
                $.each(selectedOptions, function (i, option) {
                    var CurrentReasoning = $(option);
                    $('#' + VBP_PHQQuestionnaire.params["PanelID"] + ' select option[value=' + VBP_PHQQuestionnaire.params.PHQ2Answers4PHQ9_Params[i] + ']').attr("selected", true);
                });
            }
        });
    },
    //Author: Muhammad Arshad
    //Date: 24 April 2017
    //Description: Call to Load VBP Measure Questionnaire Answers
    GetVBPMeasureQuestionnaireAnswers: function (MeasureId) {
        var deffered = $.Deferred();
        PQRS_Patient_List.loadVBPMeasureQuestionnaire_DbCall(MeasureId).done(function (response) {
            response = JSON.parse(response);
            VBP_PHQQuestionnaire.arrMeasureQuestionnaireAnswers = JSON.parse(response.VBPMeasureQuestionnaireAnswersLoad_JSON);
            var currentMeasureQuestionnaires = [];
            $.each(VBP_PHQQuestionnaire.arrMeasureQuestionnaireAnswers, function (i, item) {
                currentMeasureQuestionnaires.push(item.Question);
            });
            VBP_PHQQuestionnaire.arrMeasureQuestions = $.unique(currentMeasureQuestionnaires);
            var QuestIds = currentMeasureQuestionnaires.join(",");
            var objFirstGroupQuestion = $("#pnlPHQQuestionnaire #divQuestionGroup1").empty().append('<div class="col-sm-12 noWordBreak"><label class="control-label"><strong>Over the last 2 weeks, how often have you been bothered by any of the following problems?</strong><span class="required">*</span></label></div>');
            var objSecondGroupQuestion = $("#pnlPHQQuestionnaire #divQuestionGroup2").empty();
            var def = [];
            $.each(VBP_PHQQuestionnaire.arrMeasureQuestions, function (i, Question) {
                var currentQuestionAnswers = $.grep(VBP_PHQQuestionnaire.arrMeasureQuestionnaireAnswers, function (item, indx) {
                    return item.Question == Question;
                });
                if (i < 9) {
                    var lblQuestion = '<div class="col-sm-8"><label class="control-label">' + Question + '</label></div>';
                    var ddlAnswer = '<div class="col-sm-4"><select id="ddlAnswerId' + (i + 1) + '" name="AnswerId" class="form-control" onchange=""></select></div><div class="spacer10"></div><div class="clearfix"></div>';
                    objFirstGroupQuestion.append(lblQuestion, ddlAnswer);
                    var objddlAnswer = objFirstGroupQuestion.find("select#ddlAnswerId" + (i + 1));
                    VBP_PHQQuestionnaire.LoadVBPAnswer("PHQ9", objddlAnswer, currentQuestionAnswers);
                }
                else {
                    var lblQuestion = '<div class="col-sm-12"><label class="control-label"><strong>' + Question + '?</strong></label></div>';
                    var radAnswers = VBP_PHQQuestionnaire.LoadVBPAnswerAsRadioBtn("PHQ9", null, currentQuestionAnswers);
                    objFirstGroupQuestion.append(lblQuestion, radAnswers);
                    if (i == 9) {
                        def.push(VBP_PHQQuestionnaire.GetNoteVBPScore(VBP_PHQQuestionnaire.params["NotesId"]));
                    }
                    
                }
            });
            $.when.apply($, def).done(function ($n) {
                deffered.resolve();
            });
        });
        return deffered;
    },
    LoadVBPAnswer: function (measureId, objddl, arrAnswers) {
        // Loads Answers as DropDown
        objddl.empty();
        objddl.append($('<option/>', {
            value: "",
            html: "- SELECT -"
        }));
        $.each(arrAnswers, function (i, item) {
            if (item.VBPScoreID && item.IsActive == 'True') {
                objddl.append(
                 $('<option/>', {
                     value: item.QuestionAnswersId,
                     html: item.Answer,
                     QuestionAnswersId: item.QuestionAnswersId,
                     MeasureQuestionnaireId: item.MeasureQuestionnaireId,
                     Selected:true
                 })
             );
            }
            else{
            objddl.append(
                $('<option/>', {
                    value: item.QuestionAnswersId,
                    html: item.Answer,
                    QuestionAnswersId: item.QuestionAnswersId,
                    MeasureQuestionnaireId: item.MeasureQuestionnaireId
                    
                })
            );
            }  
        });
        //PQRS_ICDCPTCodes.ShowVBPAnswerSelection();
    },
    LoadVBPAnswerAsRadioBtn: function (measureId, objddl, arrAnswers) {
        var rdbtn = "";
        $.each(arrAnswers, function (i, item) {
            if (item.VBPScoreID && item.IsActive=='True') {
                rdbtn += '<div class="col-sm-3 noWordBreak text-center"><label class="size100per">' + item.Answer + '</label><div class=""><input class="mb-sm" QuestionAnswersId="' + item.QuestionAnswersId + '" MeasureQuestionnaireId="' + item.MeasureQuestionnaireId + '" checked=checked type="radio" name="radAnswer" id="radAnswer' + (i + 1) + '" />' + '</div></div>';
            }
            else {
                rdbtn += '<div class="col-sm-3 noWordBreak text-center"><label class="size100per">' + item.Answer + '</label><div class=""><input class="mb-sm" QuestionAnswersId="' + item.QuestionAnswersId + '" MeasureQuestionnaireId="' + item.MeasureQuestionnaireId + '" type="radio" name="radAnswer" id="radAnswer' + (i + 1) + '" />' + '</div></div>';
            }
        });
        return rdbtn;
    },
    saveCurrentScore: function () {
        //var self = $("#" + VBP_PHQQuestionnaire.params["PanelID"]);
        var isValidToSave = false;
        var selectedOptions = $("#" + VBP_PHQQuestionnaire.params["PanelID"] + " option[value!='']:selected");
        var selectedRadio = $("#" + VBP_PHQQuestionnaire.params["PanelID"] + " input[type='radio']:checked");
        if (selectedOptions.length > 0 && selectedRadio.length > 0) {
            isValidToSave = true;
            var firstGroupQuestLength = selectedOptions.length;
            var secGroupQuestLength = selectedRadio.length;
            var TotalScore = 0;
            var objData = new Object();
            objData.PatientId = VBP_PHQQuestionnaire.params["PatientId"];
            objData.MeasureId = "PHQ9";//VBP_PHQQuestionnaire.params["MeasureId"];
            objData.NoteId = VBP_PHQQuestionnaire.params["NotesId"];
            objData.ProviderId = VBP_PHQQuestionnaire.params["ProviderId"];
            objData.NoteDate = VBP_PHQQuestionnaire.params["NoteDate"];
            $.each(selectedOptions, function (i, option) {
                //calculate score of answers
                var score = option.text.split(" -")[0].trim();
                if ($.isNumeric(score) == true) {
                    TotalScore += parseInt(score);
                }
                var CurrentReasoning = $(option);
                if (CurrentReasoning.attr("MeasureQuestionnaireId") != null && CurrentReasoning.attr("MeasureQuestionnaireId") != "") {
                    if (objData.MeasureQuestionnaireId != null && objData.MeasureQuestionnaireId != "") {
                        var allMeasureQuestionnaireId = objData.MeasureQuestionnaireId + "," + CurrentReasoning.attr("MeasureQuestionnaireId");
                        allMeasureQuestionnaireId = $.unique(allMeasureQuestionnaireId.split(",")).join(",");
                        objData.MeasureQuestionnaireId = allMeasureQuestionnaireId;
                    }
                    else {
                        objData.MeasureQuestionnaireId = CurrentReasoning.attr("MeasureQuestionnaireId");
                    }
                }

                if (CurrentReasoning.attr("QuestionAnswersId") != null && CurrentReasoning.attr("QuestionAnswersId") != "") {
                    if (objData.QuestionAnswersId != null && objData.QuestionAnswersId != "") {
                        var allQuestionAnswersId = objData.QuestionAnswersId + "," + CurrentReasoning.attr("QuestionAnswersId");
                        allQuestionAnswersId = $.unique(allQuestionAnswersId.split(",")).join(",");
                        objData.QuestionAnswersId = allQuestionAnswersId;
                    }
                    else {
                        objData.QuestionAnswersId = CurrentReasoning.attr("QuestionAnswersId");
                    }
                }

                if (i == (firstGroupQuestLength - 1)) {
                    objData.Score = TotalScore;
                }
            });

            $.each(selectedRadio, function (i, option) {
                var CurrentReasoning = $(option);
                if (CurrentReasoning.attr("MeasureQuestionnaireId") != null && CurrentReasoning.attr("MeasureQuestionnaireId") != "") {
                    if (objData.MeasureQuestionnaireId != null && objData.MeasureQuestionnaireId != "") {
                        var allMeasureQuestionnaireId = objData.MeasureQuestionnaireId + "," + CurrentReasoning.attr("MeasureQuestionnaireId");
                        allMeasureQuestionnaireId = $.unique(allMeasureQuestionnaireId.split(",")).join(",");
                        objData.MeasureQuestionnaireId = allMeasureQuestionnaireId;
                    }
                    else {
                        objData.MeasureQuestionnaireId = CurrentReasoning.attr("MeasureQuestionnaireId");
                    }
                }

                if (CurrentReasoning.attr("QuestionAnswersId") != null && CurrentReasoning.attr("QuestionAnswersId") != "") {
                    if (objData.QuestionAnswersId != null && objData.QuestionAnswersId != "") {
                        var allQuestionAnswersId = objData.QuestionAnswersId + "," + CurrentReasoning.attr("QuestionAnswersId");
                        allQuestionAnswersId = $.unique(allQuestionAnswersId.split(",")).join(",");
                        objData.QuestionAnswersId = allQuestionAnswersId;
                    }
                    else {
                        objData.QuestionAnswersId = CurrentReasoning.attr("QuestionAnswersId");
                    }
                }
            });
            objData.commandType = "save_vbp_reasoning";
            PQRS_Patient_List.saveCQMReasoning_DbCall(objData).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    SelectTab("clinicalTabProgressNote", "false");
                    ClinicalMenuClick(event, null, null, null, 'clinicalTabProgressNote', 'button');
                    VBP_PHQQuestionnaire.UnLoad('Save');
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        else {
            utility.DisplayMessages("Please provide information regarding PHQ-9.", 3);
        }
    },
    //Author: Muhammad Arshad
    //Date: 12 April 2017
    //Description: Call to Load VBP Measure Questionnaire Answers
    GetNoteVBPScore: function (NotesId) {
        var deffered = $.Deferred();
        requests = [];
        VBP_PHQQuestionnaire.loadVBPScore_DbCall(NotesId,'PHQ9').done(function (response) {
            response = JSON.parse(response);
            var arrLoadJSON = JSON.parse(response.VBPScoreLoad_JSON);
            if (arrLoadJSON != null && arrLoadJSON.length > 0) {
                VBP_PHQQuestionnaire.params["isValidNoteScore"] = true;
            }
            else {
                VBP_PHQQuestionnaire.params["isValidNoteScore"] = false;
            }
            $.each(arrLoadJSON, function (i, selection) {
                var selectedOptions = $("#" + VBP_PHQQuestionnaire.params["PanelID"] + " select option[MeasureQuestionnaireId='" + selection.MeasureQuestionnaireId + "'][QuestionAnswersId='" + selection.QuestionAnswersId + "']");
                $.each(selectedOptions, function (j, option) {
                    $(option).prop("selected", true);
                });
                var arrselectedRadio = $("#" + VBP_PHQQuestionnaire.params.PanelID + " input[type='radio'][MeasureQuestionnaireId='" + selection.MeasureQuestionnaireId + "'][QuestionAnswersId='" + selection.QuestionAnswersId + "']");
                $.each(arrselectedRadio, function (k, radio) {
                    $(radio).prop("checked", true);
                });
                $('#frmPHQQuestionnaire').data('serialize', $('#frmPHQQuestionnaire').serialize());
            });
            deffered.resolve();
        });
        return deffered.promise();
    },
    loadVBPScore_DbCall: function (NotesId,MeasureNumber) {
        var objData = {};
        objData["NotesId"] = NotesId;
        objData["MeasureNumber"] = MeasureNumber;
        objData["commandType"] = "load_vbp_score";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    UnLoad: function (caller) {
        var isValidNoteScore = false;
        if (VBP_PHQQuestionnaire.params.ParentCtrl == "clinicalTabProgressNote" || VBP_PHQQuestionnaire.params.ParentCtrl == "Clinical_Procedures") {
            setPatientBanner(VBP_PHQQuestionnaire.params["PatientId"]);
        }
        
        if ((VBP_PHQQuestionnaire.params.ParentCtrl == "clinicalTabProgressNote" || VBP_PHQQuestionnaire.params.ParentCtrl == "Clinical_Procedures")
            && caller.toLowerCase() != "save") {
            if ($('#frmPHQQuestionnaire').serialize() != $('#frmPHQQuestionnaire').data('serialize')) {
                utility.myConfirm('45', function () {
                    return;
                }, function () {
                    VBP_PHQ2Questionnaire.PopulateScoreCancelOrClose(VBP_PHQQuestionnaire.params["NotesId"],true);
                    VBP_PHQQuestionnaire.UnloadActionPan();
                }
                );
            }
            else {
                VBP_PHQ2Questionnaire.PopulateScoreCancelOrClose(VBP_PHQQuestionnaire.params["NotesId"],true);
                VBP_PHQQuestionnaire.UnloadActionPan();
            }
        }
        else {
            VBP_PHQ2Questionnaire.PopulateScoreCancelOrClose(VBP_PHQQuestionnaire.params["NotesId"],true);
            VBP_PHQQuestionnaire.UnloadActionPan();
        }
    },
    UnloadActionPan: function () {
        if (VBP_PHQQuestionnaire.params != null && VBP_PHQQuestionnaire.params.ParentCtrl != null) {
            UnloadActionPan(VBP_PHQQuestionnaire.params.ParentCtrl, 'VBP_PHQQuestionnaire');
        }
        else
            UnloadActionPan(null, 'VBP_PHQQuestionnaire');
    },
}