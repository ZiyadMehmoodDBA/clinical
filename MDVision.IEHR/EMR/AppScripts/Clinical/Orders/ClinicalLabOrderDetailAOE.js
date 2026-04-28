ClinicalLabOrderDetailAOE = {

    params: [],
    bIsFirstLoad: true,
    EditableGrid: null,
    LabOrderProblems: [],
    FavListName: "LabOrderDetail",
    checkedProblems: [],
    JarsId: "",
    Specimens: [],
    SpecimenOptionId: "",
    MultiSelectQuestions: [],
    UnAnsweredSelections: [],
    Load: function (params) {
        BackgroundLoaderShow(true);
        ClinicalLabOrderDetail.AOEsExists = true;
        //ClinicalLabOrderDetailAOE.params["TabID"] = 'ClinicalLabOrderDetailAOE';
        ClinicalLabOrderDetailAOE.params = params;

        ClinicalLabOrderDetailAOE.Specimens = [];
        ClinicalLabOrderDetailAOE.InvalidMultiSelects = [];

        if (ClinicalLabOrderDetailAOE.params.PanelID != 'pnlClinicalLabOrderDetailAOE') {
            ClinicalLabOrderDetailAOE.params.PanelID = ClinicalLabOrderDetailAOE.params.PanelID + ' #pnlClinicalLabOrderDetailAOE';
        } else {
            ClinicalLabOrderDetailAOE.params.PanelID = 'pnlClinicalLabOrderDetailAOE';
        }
        ClinicalLabOrderDetailAOE.ValidateLabOrderAOE();
        //$('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divQuestions").html($('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divTemplateQuestion").clone());
        //$('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divQuestions").html($('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divTemplateQuestion").clone());
        //$('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divQuestions").html($('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divTemplateQuestion").clone());
        //$('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divQuestions").html($('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divTemplateQuestion").clone());


        if (ClinicalLabOrderDetailAOE.params.CPTCode != null && ClinicalLabOrderDetail.CPTCodeQA.length < 1) {

            ClinicalLabOrderDetailAOE.loadLabOrderDetailQuestionAndAnswer(ClinicalLabOrderDetailAOE.params.CPTCode, ClinicalLabOrderDetailAOE.params.LabOrderTestId);
        }
        else if (ClinicalLabOrderDetail.CPTCodeQA.length > 0) {
            var LabOrderAOE_Fill_JSON = $.grep(ClinicalLabOrderDetail.CPTCodeQA, function (question, index) {
                return question.Question != "" && question.CPTCode == ClinicalLabOrderDetailAOE.params.CPTCode;
            });

            var LabOrderAOE_Answers_Fill_JSON = $.grep(ClinicalLabOrderDetail.CPTCodeQA, function (question, index) {
                return question.Answer != "" && question.CPTCode == ClinicalLabOrderDetailAOE.params.CPTCode;
            });

            if (LabOrderAOE_Fill_JSON.length < 1) {
                ClinicalLabOrderDetailAOE.loadLabOrderDetailQuestionAndAnswer(ClinicalLabOrderDetailAOE.params.CPTCode, ClinicalLabOrderDetailAOE.params.LabOrderTestId);
            }
            else {
                ClinicalLabOrderDetailAOE.buildQuestions(LabOrderAOE_Fill_JSON, LabOrderAOE_Answers_Fill_JSON);
            }

        }

        if (ClinicalLabOrderDetailAOE.SpecimenOptionId != "") {
          //  $('#' + ClinicalLabOrderDetailAOE.SpecimenOptionId + ' option:selected').prop('selected', false);
        }

    },

    //Author: Muhammad Arshad
    //Date :  11-08-2016
    //This function will handle validation of LabOrderAOE Questions
    ValidateLabOrderAOE: function () {
        $("#" + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE")
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   //// Start 27/11/2015 Muhammad Irfan Bug # 91,92

                   //ProblemName: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
                   //Provider: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //}
                   //// End 27/11/2015 Muhammad Irfan Bug # 91,92

                   ////Color: {
                   ////    group: '.col-sm-4',
                   ////    validators: {
                   ////        notEmpty: {
                   ////            message: ''
                   ////        }
                   ////    }
                   ////}
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            ClinicalLabOrderDetailAOE.saveCPTQA();
        });
    },


    loadLabOrderDetailQuestionAndAnswer: function (CPTCode, LabOrderTestId) {
        ClinicalLabOrderDetail.getLabOrderQuestionAnswerDbCall(CPTCode, LabOrderTestId).done(function (response) {

            if (response != "") {
                response = JSON.parse(response);
                if (response.status == true) {
                    response.LabOrderAOE_Fill_JSON = JSON.parse(response.LabOrderAOE_Fill_JSON);
                    response.LabOrderAOE_Answers_Fill_JSON = JSON.parse(utility.decodeHtml(response.LabOrderAOE_Answers_Fill_JSON));
                    ClinicalLabOrderDetailAOE.buildQuestions(response.LabOrderAOE_Fill_JSON, response.LabOrderAOE_Answers_Fill_JSON);

                }
                //if (response.TextAnswer == 'NO') {

                //}
                //else {

                //}
            }

        });
    },

    checkDefaultAnswers: function (selectedAnswers, Question) {
        selectedAnswers = [];
        if (ClinicalLabOrderDetailAOE.params.CPTDescription == "Urine Cytology")
        {
            if (Question == "Services") {
                var obj = {};
                obj["Answer"] = "Global";
                obj["Answers"] = "Consults,Global,Professional Only,Technical Only";
                obj["CPTCode"] = "UrineCytology";
                obj["Question"] = "Services";
                obj["Required"] = "yes";
                selectedAnswers.push(obj);
            }
            if (Question == "Specimen") {
                var obj = {};
                obj["Answer"] = "Urine";
                obj["Answers"] = "Urine";
                obj["CPTCode"] = "UrineCytology";
                obj["Question"] = "Specimen";
                obj["Required"] = "yes";
                obj["MultiAnswerList"] = "YES";
                selectedAnswers.push(obj);
            }
            if (Question == "CollectionMethod") {
                var obj = {};
                obj["Answer"] = "Voided";
                obj["Answers"] = "Aspiration,Bladder Wash,Catheterized,Cystoscopy,Cytology,Ileal Conduit,Left Renal Wash,Left Ureteral Wash,Post Cystoscopy Void,Random,Renal Wash,Right Renal Wash,Right Ureteral Wash,Urethral Wash,Voided";
                obj["CPTCode"] = "UrineCytology";
                obj["Question"] = "CollectionMethod";
                obj["Required"] = "yes";
                selectedAnswers.push(obj);
            }
            if (Question == "Jars") {
                var obj = {};
                obj["Answer"] = "1";
                obj["Answers"] = "1";
                obj["CPTCode"] = "UrineCytology";
                obj["Question"] = "Jars";
                obj["Required"] = "yes";
                selectedAnswers.push(obj);
            }
        }

        if (ClinicalLabOrderDetailAOE.params.CPTDescription == "UroVysion FISH") {
            if (Question == "Services") {
                var obj = {};
                obj["Answer"] = "Global";
                obj["Answers"] = "Consults,Global,Professional Only,Technical Only";
                obj["CPTCode"] = "UVFISH";
                obj["Question"] = "Services";
                obj["Required"] = "yes";
                selectedAnswers.push(obj);
            }
            if (Question == "Specimen") {
                var obj = {};
                obj["Answer"] = "Urine";
                obj["Answers"] = "Urine";
                obj["CPTCode"] = "UVFISH";
                obj["Question"] = "Specimen";
                obj["Required"] = "yes";
                obj["MultiAnswerList"] = "YES";
                selectedAnswers.push(obj);
            }
            if (Question == "CollectionMethod") {
                var obj = {};
                obj["Answer"] = "Voided";
                obj["Answers"] = "Aspiration,Bladder Wash,Catheterized,Cystoscopy,Cytology,Ileal Conduit,Left Renal Wash,Left Ureteral Wash,Post Cystoscopy Void,Random,Renal Wash,Right Renal Wash,Right Ureteral Wash,Ureteral Wash,Urethral Wash,Voided";
                obj["CPTCode"] = "UVFISH";
                obj["Question"] = "CollectionMethod";
                obj["Required"] = "yes";
                selectedAnswers.push(obj);
            }
            if (Question == "Jars") {
                var obj = {};
                obj["Answer"] = "1";
                obj["Answers"] = "1";
                obj["CPTCode"] = "UVFISH";
                obj["Question"] = "Jars";
                obj["Required"] = "yes";
                selectedAnswers.push(obj);
            }
        }

        if (ClinicalLabOrderDetailAOE.params.CPTDescription == "Vasectomy") {
            if (Question == "Services") {
                var obj = {};
                obj["Answer"] = "Global";
                obj["Answers"] = "Consults,Global,Professional Only,Technical Only";
                obj["CPTCode"] = "VasDefHist";
                obj["Question"] = "Services";
                obj["Required"] = "yes";
                selectedAnswers.push(obj);
            }
            if (Question == "Specimen") {
                var obj = {};
                obj["Answer"] = "Right Vas Deferens,  Left Vas Deferens";
                obj["Answers"] = "Right Vas Deferens,Left Vas Deferens,One segment of vas deferens,Bilateral Vas Deferans,Vas Deferens";
                obj["CPTCode"] = "VasDefHist";
                obj["Question"] = "Specimen";
                obj["Required"] = "yes";
                obj["MultiAnswerList"] = "YES";
                selectedAnswers.push(obj);
            }
            if (Question == "CollectionMethod") {
                var obj = {};
                obj["Answer"] = "Scalpel Excision";
                obj["Answers"] = "No Scalpel Vasectomy,Scalpel Excision,Vasectomy";
                obj["CPTCode"] = "VasDefHist";
                obj["Question"] = "CollectionMethod";
                obj["Required"] = "yes";
                selectedAnswers.push(obj);
            }
            if (Question == "Jars") {
                var obj = {};
                obj["Answer"] = "2";
                obj["Answers"] = "2";
                obj["CPTCode"] = "VasDefHist";
                obj["Question"] = "Jars";
                obj["Required"] = "yes";
                selectedAnswers.push(obj);
            }
        }
        return selectedAnswers;
    },
    buildQuestions: function (LabOrderAOE_Fill_JSON, LabOrderAOE_Answers_Fill_JSON) {
        var objDeffered = $.Deferred();
        ClinicalLabOrderDetailAOE.MultiSelectQuestions = [];
        var divQuestions = $('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divQuestions");
        divQuestions.children().remove();
        divQuestions.append('<strong>' + ClinicalLabOrderDetailAOE.params.CPTDescription + '</strong><div class="spacer10"></div>')
        //response.LabOrderAOE_Fill_JSON = JSON.parse(response.LabOrderAOE_Fill_JSON);
        //response.LabOrderAOE_Answers_Fill_JSON = JSON.parse(response.LabOrderAOE_Answers_Fill_JSON);
        $.each(LabOrderAOE_Fill_JSON, function (i, item) {
            item.Question = item.Question.replace(/&quot;/g, '"');
            var newHTML = $('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divTemplateQuestion").clone();
            newHTML.removeClass("hidden");
            newHTML.attr("id", "divQuestion" + (i + 1));
            newHTML.find("#lblQuestionTemplateId").text(item.Question);
            if (item.Required.toLowerCase() == "yes") {
                var spnRequired = $('<span id="spnRequired" class="required">*</span>');
                newHTML.find("#lblQuestionTemplateId").append(spnRequired)
            }
            newHTML.find("#lblQuestionTemplateId").attr("id", "lblQuestion" + (i + 1));
            var selectedAnswer = $.grep(LabOrderAOE_Answers_Fill_JSON, function (answer, index) {
                return answer.Question == item.Question;
            });
            //If no values were selected (Select Default Answers for VitalAxis TEMPORARY SOLUTION FOR ROSEN
            if (selectedAnswer.length == 0) { 
                selectedAnswer = ClinicalLabOrderDetailAOE.checkDefaultAnswers(selectedAnswer, item.Question);
            }
            item.Answers = utility.decodeHtml(item.Answers);
            if ((item.Answers != null && (item.Answers.indexOf(',') > -1 || (item.AnswerList == "YES")) && (item.Question != "Jars"))) {
                newHTML.find("#txtAnswerTemplateId").remove();
                var newddlQuestion = newHTML.find("#ddlAnswerTemplateId").attr("id", "ddlAnswer" + (i + 1));
                newddlQuestion.attr("name", "ddlAnswer" + (i + 1));
                if (item.Question != "Specimen" && item.MultiAnswerList != "YES")
                {
                    newddlQuestion.append('<option value="">-Select-</option>');
                }
                var arrAnswers = item.Answers.split(",");
                $.each(arrAnswers, function (j, answer) {
                    newddlQuestion.append('<option value="' + answer.trim() + '">' + answer + '</option>');
                });

                if (selectedAnswer.length > 0) {
                    newddlQuestion.val(selectedAnswer[0].Answer.trim());
                }
                // VitalAxis Conditions
                if (item.MultiAnswerList == 'YES') {
                    newddlQuestion.attr('MultiAnswerList', item.MultiAnswerList);
                    newddlQuestion.attr("multiple", "multiple");
                    if (selectedAnswer.length > 0) {
                        var dataarray = selectedAnswer[0].Answer.trim().split(",");
                        for (var Nawa = 0; Nawa < dataarray.length; Nawa++) {
                            dataarray[Nawa] = dataarray[Nawa].trim();
                        }
                        newddlQuestion.val(dataarray);
                       // newddlQuestion.multiselect("refresh");
                        if (item.Question == "Specimen") {
                            ClinicalLabOrderDetailAOE.Specimens = dataarray;
                        }

                    }
                    if (item.Question == "Specimen") {
                     
                        newddlQuestion.multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247,
                            nonSelectedText: 'All',
                            selectAll: false,
                            onChange: function (option, checked, select) {
                                ClinicalLabOrderDetailAOE.checkSpecimens(option, checked, newddlQuestion);
                            },


                        });
                        ClinicalLabOrderDetailAOE.SpecimenOptionId = 'ddlAnswer' + (i + 1);
                    }
                    else {
                        newddlQuestion.multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247,
                            nonSelectedText: 'All',
                            selectAll: false,

                        });
                    }
                    ClinicalLabOrderDetailAOE.MultiSelectQuestions.push(newddlQuestion);
                    if (selectedAnswer.length <= 0)
                    {
                        setTimeout(function () {
                            $('#ddlAnswer' + (i + 1)).val(new Array());
                            $('#ddlAnswer' + (i + 1)).multiselect('refresh');
                        }, 400);
                    }
                }
            }
            else {
                newHTML.find("#ddlAnswerTemplateId").remove();
                var txtAnswer = newHTML.find("#txtAnswerTemplateId").attr("id", "txtAnswer" + (i + 1));
                if (item.Question == "Jars") { // VitalAxis 
                    txtAnswer.attr('disabled', 'true');
                    txtAnswer.val('0');
                    ClinicalLabOrderDetailAOE.JarsId = txtAnswer.attr("name", "txtAnswer" + (i + 1));
                }
                if (selectedAnswer.length > 0) {
                    txtAnswer.val(selectedAnswer[0].Answer);
                }
                
                txtAnswer.attr("name", "txtAnswer" + (i + 1));

            }



            divQuestions.append(newHTML);
            divQuestions.append($('<div class="spacer10"></div>'));

            if ((i + 1) == LabOrderAOE_Fill_JSON.length) {
                var DivQuestion = $('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div[id*='divQuestion']").not("div#divQuestions");
                $.each(DivQuestion, function (i, item) {
                    var currentQuest = $(item);
                    if (currentQuest.find("span[id*='spnRequired']").length > 0) {
                        var QuestCtrls = currentQuest.find("input[type='text'],select");
                        $.each(QuestCtrls, function (j, ctrl) {
                            //$('#surveyForm').formValidation('addField', $option);
                            var ctrlName = $(ctrl).attr("name");
                            $("#" + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE").bootstrapValidator('addField', ctrlName, {
                                group: '.' + $(ctrl).closest("div").attr("class").split(" ")[0],
                                validators: {
                                    notEmpty: {
                                        message: ''
                                    }
                                }
                            });
                        });
                    }
                });
            }
        });
        //    .promise().done(function () {
        //    utility.DisplayMessages("each called successfully", 3);
        //});
    },
    checkSpecimens: function(option, check, select) { // only for VitalAxis
        if (check == true && option != undefined) {
            if (option.html().trim() != "-Select-") {
                ClinicalLabOrderDetailAOE.Specimens.push(option.html().trim());
                ClinicalLabOrderDetailAOE.JarsId.val(ClinicalLabOrderDetailAOE.Specimens.length);
            }
        }
        else if (check == true && option == undefined) { // select all 
            var Id = select.attr('id');
            ClinicalLabOrderDetailAOE.Specimens = [];
            $('#' + Id + ' option').each(function () {
                if ($(this).val().trim() != "-Select-") {
                    ClinicalLabOrderDetailAOE.Specimens.push($(this).val());
                }
            });
            ClinicalLabOrderDetailAOE.JarsId.val(ClinicalLabOrderDetailAOE.Specimens.length);
        }
        else if (check == false && option == undefined) {
            ClinicalLabOrderDetailAOE.Specimens = [];
            ClinicalLabOrderDetailAOE.JarsId.val(ClinicalLabOrderDetailAOE.Specimens.length);
        }
        else {
            ClinicalLabOrderDetailAOE.Specimens = jQuery.grep(ClinicalLabOrderDetailAOE.Specimens, function (value) {
                return value != option.html().trim();
            });
            ClinicalLabOrderDetailAOE.JarsId.val(ClinicalLabOrderDetailAOE.Specimens.length);
        }
    },
    saveCPTQA: function () {
        ClinicalLabOrderDetailAOE.InvalidMultiSelects = [];
        var self = $('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div#divQuestions")
        self.find("div[id*='divQuestion']").each(function (index, item) {
            var LabOrderQuestionAnswerModel = {};
            LabOrderQuestionAnswerModel['CPTCode'] = ClinicalLabOrderDetailAOE.params.CPTCode;
            var $this = $(this);
            var $lblQuestion = $this.find("label[id*='lblQuestion']");
            var $selectControl = $this.find("select[id*='ddlAnswer']");
            var $inputControl = $this.find("input[id*='txtAnswer']");
            var isRequiredControl = $this.find("span[id*='spnRequired']").length > 0 ? "yes" : "now";
            if ($lblQuestion.length > 0) {
                LabOrderQuestionAnswerModel['Question'] = $lblQuestion.clone().children().remove().end().text();
                LabOrderQuestionAnswerModel['Required'] = isRequiredControl;
            }

            if ($selectControl.length > 0) {
                if ($selectControl.attr('MultiAnswerList') !=  undefined && $selectControl.attr('MultiAnswerList') == "YES" ) // VitalAxis conditions
                {
                    var multiselectValues = $selectControl.parent().children().find('button').attr('title');
                    if (multiselectValues.indexOf("-Select-, ") > 0) {
                        if (LabOrderQuestionAnswerModel['Question'] == 'Specimen') {
                            $('#' + ClinicalLabOrderDetailAOE.JarsId).val = (($('#' + ClinicalLabOrderDetailAOE.JarsId).val() == "0") ? "0" : $('#' + ClinicalLabOrderDetailAOE.JarsId).val() - 1);
                        }
                        multiselectValues = multiselectValues.substring(multiselectValues.indexOf("-Select-, "), multiselectValues.length);
                    }
                    if (multiselectValues.toLowerCase() == "all" || multiselectValues.toLowerCase() == "none selected") {
                        ClinicalLabOrderDetailAOE.InvalidMultiSelects.push(LabOrderQuestionAnswerModel['Question']);
                        utility.DisplayMessages("Please select at least one value in " + LabOrderQuestionAnswerModel['Question'],2);
                        return;
                    }
                    LabOrderQuestionAnswerModel['Answer'] = multiselectValues;
                    LabOrderQuestionAnswerModel['MultiAnswerList'] = "YES";
                }
                else {
                    LabOrderQuestionAnswerModel['Answer'] = $selectControl.find('option:selected').val();
                }
                var tags = [];
                $selectControl.find('option').each(function () {
                    if ($(this).val() != "") {
                        tags.push($(this).val());
                    }

                });
                LabOrderQuestionAnswerModel['Answers'] = tags.join(',');

            }
            if ($inputControl.length > 0) {
                LabOrderQuestionAnswerModel['Answer'] = $inputControl.val();
                LabOrderQuestionAnswerModel['Answers'] = $inputControl.val();
            }
                
            ClinicalLabOrderDetail.pusCPTCodeQA(LabOrderQuestionAnswerModel);

        });
        if (ClinicalLabOrderDetailAOE.InvalidMultiSelects.length <= 0) {
            ClinicalLabOrderDetailAOE.UnLoad('Saver');
        }
        
    },

    isQuestionAnswersValid: function () {
        var isValid = true;
        var DivQuestion = $('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE div[id*='divQuestion']").not("div#divQuestions");
        $.each(DivQuestion, function (i, item) {
            var currentQuest = $(item);
            if (currentQuest.find("span[id*='spnRequired']").length > 0) {
                var QuestCtrls = currentQuest.find("input[type='text'],select");
                $.each(QuestCtrls, function (j, ctrl) {
                    if (!($(ctrl).hasClass('multiselect-search')) && ($(ctrl).attr('multiple') != 'multiple')) {
                        var answer = $(ctrl).val();
                        if (!(answer != null && answer != "")) {
                            isValid = false;
                        }
                    }
                });
            }
        });
        return isValid;
    },

    UnLoad: function (caller) {
        var isAnswered = ClinicalLabOrderDetailAOE.isQuestionAnswersValid();
        var test = ClinicalLabOrderDetailAOE.params.CPTCode + " - " + ClinicalLabOrderDetailAOE.params.CPTDescription;
        var code = ClinicalLabOrderDetailAOE.params.CPTCode;
        ClinicalLabOrderDetailAOE.UnAnsweredSelections = [];
        if (caller != 'Saver' && caller != null) {
            $('#divQuestions select').each(function () {
                $('#' + this.id + ' option:selected').prop("selected", false);
            });
        }
        var isExists = false;
        if (caller != 'Saver' && ClinicalLabOrderDetail.CPTCodeQA.length == 0 && ClinicalLabOrderDetail.params.mode != "Edit") {
            if (isAnswered == false) {
                $.each(ClinicalLabOrderDetail.ArrayValidation, function (i, item) {
                    if (item.Test == test) {
                        isExists = true;
                    }
                });
                if (isExists == false) {
                    ClinicalLabOrderDetail.ArrayValidation.push({
                        Test: test,
                        IsValid: isAnswered,
                        Code: code
                    });
                }
            }
            else if (isAnswered == true && ClinicalLabOrderDetail.ArrayValidation.length > 0) {
                $.each(ClinicalLabOrderDetail.ArrayValidation, function (i, item) {
                    if (item.Test == test) {
                        ClinicalLabOrderDetail.ArrayValidation.splice(i, 1);
                    }
                });

            }
        }
        if (isAnswered == true && ClinicalLabOrderDetail.ArrayValidation.length > 0) {
            $.each(ClinicalLabOrderDetail.ArrayValidation, function (i, item) {
                if (item.Test == test) {
                    ClinicalLabOrderDetail.ArrayValidation.splice(i, 1);
                }
            });

        }

        ClinicalLabOrderDetailAOE.checkedProblems = [];
        ClinicalLabOrderDetail.colorCPTCodeAOEIcon();
        var form = '#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE";
        var saveButtonisHidden = $('#' + ClinicalLabOrderDetailAOE.params.PanelID + " #frmClinicalLabOrderDetailAOE #btnSaveOrder").hasClass("hidden");

        if (caller == 'saveExit' || saveButtonisHidden == true) {
            if (ClinicalLabOrderDetailAOE.params["ParentCtrl"] == "Clinical_LabOrder") {
                UnloadActionPan(ClinicalLabOrderDetailAOE.params["ParentCtrl"], "ClinicalLabOrderDetailAOE", null, ClinicalLabOrderDetailAOE.params["ParentCtrlPanelID"]);
            }
            else {
                UnloadActionPan(ClinicalLabOrderDetailAOE.params["ParentCtrl"], "ClinicalLabOrderDetailAOE");

            }
        }
        else {

            //   utility.UnLoadDialog(form, function () {
            if (ClinicalLabOrderDetailAOE.params["ParentCtrl"] == "Clinical_LabOrder") {
                UnloadActionPan(ClinicalLabOrderDetailAOE.params["ParentCtrl"], "ClinicalLabOrderDetailAOE", null, ClinicalLabOrderDetailAOE.params["ParentCtrlPanelID"]);
            }
            else {
                $.each(ClinicalLabOrderDetailAOE.MultiSelectQuestions, function (i, item) {
                    if ($('#' + item.attr('id') + ' option:selected').length <= 0) {
                        if (ClinicalLabOrderDetail.CPTCodeQA.length <= 0) {
                            ClinicalLabOrderDetailAOE.UnAnsweredSelections.push(item);
                        }
                        
                    }
                });
                    UnloadActionPan(ClinicalLabOrderDetailAOE.params["ParentCtrl"], "ClinicalLabOrderDetailAOE");
                
            }
                    
                
            
            //}, function () {
            //    if (ClinicalLabOrderDetailAOE.params["ParentCtrl"] == "Clinical_LabOrder") {
            //        UnloadActionPan(ClinicalLabOrderDetailAOE.params["ParentCtrl"], "ClinicalLabOrderDetailAOE", null, ClinicalLabOrderDetailAOE.params["ParentCtrlPanelID"]);
            //    }
            //    else {
            //        UnloadActionPan(ClinicalLabOrderDetailAOE.params["ParentCtrl"], "ClinicalLabOrderDetailAOE");

            //    }
            //});
        }
    },


}