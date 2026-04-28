VBP_PHQ2Questionnaire = {
    params: [],
    bIsFirstLoad: true,
    arrMeasureQuestionnaireAnswers: [],
    PHQ2Answers4PHQ9_Params:[],
    arrMeasureQuestions: [],
    Load: function (params) {
        VBP_PHQ2Questionnaire.params = params;
        if (VBP_PHQ2Questionnaire.params.PanelID != 'pnlPHQ2Questionnaire') {
            VBP_PHQ2Questionnaire.params.PanelID = VBP_PHQ2Questionnaire.params.PanelID + ' #pnlPHQ2Questionnaire';
        }
        else {
            VBP_PHQ2Questionnaire.params.PanelID = 'pnlPHQ2Questionnaire';
        }
        VBP_PHQ2Questionnaire.GetVBPMeasureQuestionnaireAnswers("PHQ2");
    },

    GetVBPMeasureQuestionnaireAnswers: function (MeasureId) {
        PQRS_Patient_List.loadVBPMeasureQuestionnaire_DbCall(MeasureId).done(function (response) {
            response = JSON.parse(response);
            VBP_PHQ2Questionnaire.arrMeasureQuestionnaireAnswers = JSON.parse(response.VBPMeasureQuestionnaireAnswersLoad_JSON);
            var currentMeasureQuestionnaires = [];
            $.each(VBP_PHQ2Questionnaire.arrMeasureQuestionnaireAnswers, function (i, item) {
                currentMeasureQuestionnaires.push(item.Question);
            });
            VBP_PHQ2Questionnaire.arrMeasureQuestions = $.unique(currentMeasureQuestionnaires);
            var QuestIds = currentMeasureQuestionnaires.join(",");
            var objFirstGroupQuestion = $("#pnlPHQ2Questionnaire #divQuestionGroup1").empty().append('<div class="col-sm-12 noWordBreak"><label class="control-label"><strong>Over the last 2 weeks, how often have you been bothered by any of the following problems?</strong></label></div>');
            var objSecondGroupQuestion = $("#pnlPHQ2Questionnaire #divQuestionGroup2").empty();
            $.each(VBP_PHQ2Questionnaire.arrMeasureQuestions, function (i, Question) {
                var currentQuestionAnswers = $.grep(VBP_PHQ2Questionnaire.arrMeasureQuestionnaireAnswers, function (item, indx) {
                    return item.Question == Question;
                });
                var lblQuestion = '<div class="col-sm-8"><label class="control-label">' + Question + '</label></div>';
                var ddlAnswer = '<div class="col-sm-4"><select id="ddlAnswerId' + (i + 1) + '" name="AnswerId" class="form-control" onchange=""></select></div><div class="spacer10"></div><div class="clearfix"></div>';
                objFirstGroupQuestion.append(lblQuestion, ddlAnswer);
                var objddlAnswer = objFirstGroupQuestion.find("select#ddlAnswerId" + (i + 1));
                VBP_PHQQuestionnaire.LoadVBPAnswer("PHQ2", objddlAnswer, currentQuestionAnswers);
                VBP_PHQ2Questionnaire.GetNoteVBPScore(VBP_PHQ2Questionnaire.params["NotesId"]);
            });
        });
    },

    GetNoteVBPScore: function (NotesId) {
        VBP_PHQQuestionnaire.loadVBPScore_DbCall(NotesId, 'PHQ2').done(function (response) {
            response = JSON.parse(response);
            var arrLoadJSON = JSON.parse(response.VBPScoreLoad_JSON);
           
            $.each(arrLoadJSON, function (i, selection) {
                var selectedOptions = $("#" + VBP_PHQ2Questionnaire.params["PanelID"] + " select option[MeasureQuestionnaireId='" + selection.MeasureQuestionnaireId + "'][QuestionAnswersId='" + selection.QuestionAnswersId + "']");
                $.each(selectedOptions, function (j, option) {
                    $(option).prop("selected", true);
                });
                $('#frmPHQ2Questionnaire').data('serialize', $('#frmPHQ2Questionnaire').serialize());
            });
        });
    },

    saveCurrentScore: function () {
        var isValidToSave = false;
        var PHQ2Answers4PHQ9_Params = [];
        var selectedOptions = $("#" + VBP_PHQ2Questionnaire.params["PanelID"] + " option[value!='']:selected");
        if (selectedOptions.length > 0) {
            isValidToSave = true;
            var firstGroupQuestLength = selectedOptions.length;
            var TotalScore = 0;
            var objData = new Object();
            objData.PatientId = VBP_PHQ2Questionnaire.params["PatientId"];
            objData.MeasureId = "PHQ2";
            objData.NoteId = VBP_PHQ2Questionnaire.params["NotesId"];
            objData.ProviderId = VBP_PHQ2Questionnaire.params["ProviderId"];
            objData.NoteDate = VBP_PHQ2Questionnaire.params["NoteDate"];
            var currentMeasureQuestionnaires = [];
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
                    PHQ2Answers4PHQ9_Params.push(CurrentReasoning.attr("QuestionAnswersId"));
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

            VBP_PHQ2Questionnaire.params.PHQ2Answers4PHQ9_Params = PHQ2Answers4PHQ9_Params;
            objData.commandType = "save_vbp_reasoning";
            PQRS_Patient_List.saveCQMReasoning_DbCall(objData).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    VBP_PHQ2Questionnaire.UnLoad('close');
                    if (TotalScore > 2) {
                        //this timeout is because let unloadActionPan completely unload modal pop up.
                        setTimeout(function () { VBP_PHQ2Questionnaire.showPHQ9PopUp(VBP_PHQ2Questionnaire.params.ParentCtrl); }, 901);
                    }
                    setPatientBanner(VBP_PHQ2Questionnaire.params["PatientId"]);
                }
                else {
                    VBP_PHQ2Questionnaire.UnLoad('close');
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            VBP_PHQ2Questionnaire.PopulateScoreCancelOrClose(VBP_PHQ2Questionnaire.params["NotesId"],true);
            utility.DisplayMessages("Please provide information regarding PHQ-2.", 3);
        }
    },
    PopulateScoreCancelOrClose: function (NotesID,PHQ9TextNeeded) {
        Clinical_Procedures.CalculateVBPSocreAndAppend(NotesID, PHQ9TextNeeded).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_procedures').parent().parent().find('section').each(function () {
                    if ($(this).find("ul li span strong").length > 0) {
                        $(this).find("ul li span strong").closest('li').remove();
                    }
                    if ($(this).find("ul li a").length > 0 && $(this).find("ul li a") && $(this).find("ul li a").attr("onclick").indexOf('VBP_PHQ2Questionnaire') > -1) {
                        $(this).find("ul li a").closest('li').remove();
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + response.ProceudereID + ' ul').append(response.PHQSoapText);
                        Clinical_ProgressNote.saveComponentSOAPText('Procedures', true);
                    }
                    else {
                        var SecId = $(this).attr("id"); var responseID = 'Cli_Procedures_Main' + response.ProceudereID;
                        if (SecId == responseID) {
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + response.ProceudereID + ' ul').append(response.PHQSoapText);
                            Clinical_ProgressNote.saveComponentSOAPText('Procedures', true);
                        }
                    }
                });
            }
        });
    },
    showPHQ9PopUp: function (prntcrtl) {
        var params = [];
        params["NotesId"] = VBP_PHQ2Questionnaire.params.NotesId;
        params["PatientId"] = VBP_PHQ2Questionnaire.params.PatientId;
        params["FromAdmin"] = "0";
        prntcrtl ? params["ParentCtrl"] = prntcrtl : params["ParentCtrl"] = "clinicalTabProgressNote";
        params["mode"] = "Add";
        params["VisitId"] = VBP_PHQ2Questionnaire.params.VisitId;
        params["NoteDate"] = VBP_PHQ2Questionnaire.params.NoteDate;
        params["VisitDate"] = VBP_PHQ2Questionnaire.params.VisitDateForFollowUp;
        params["ProviderId"] = VBP_PHQ2Questionnaire.params.ProviderId;
        params["PHQ2Answers4PHQ9_Params"] = VBP_PHQ2Questionnaire.params.PHQ2Answers4PHQ9_Params;
        LoadActionPan("VBP_PHQQuestionnaire", params);
    },

    UnLoad: function (caller) {
       VBP_PHQ2Questionnaire.UnloadActionPan();
       VBP_PHQ2Questionnaire.PopulateScoreCancelOrClose(VBP_PHQ2Questionnaire.params.NotesId,true);
    },
    UnloadActionPan: function () {

        if (VBP_PHQ2Questionnaire.params != null && VBP_PHQ2Questionnaire.params.ParentCtrl != null) {
            UnloadActionPan(VBP_PHQ2Questionnaire.params.ParentCtrl, 'VBP_PHQ2Questionnaire');
        }
        else
            UnloadActionPan(null, 'VBP_PHQ2Questionnaire');
    },
}