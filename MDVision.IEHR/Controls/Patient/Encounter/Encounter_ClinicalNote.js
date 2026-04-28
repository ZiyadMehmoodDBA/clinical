EncounterClinicalNote = {
    bIsFirstLoad: true,
    previousHTML : "",
    currentHTML : "",
    params: [],
    Load: function (params) {
        EncounterClinicalNote.params = params;
        if (EncounterClinicalNote.bIsFirstLoad) {
            EncounterClinicalNote.bIsFirstLoad = false;
            var self = $('#pnlEncounterClinicalNote');
            //self.loadDropDowns(true).done(function () {
            //});
            Patient_Demographic.FillPatientInfo(EncounterClinicalNote.params);
            //EncounterClinicalNote.LoadAllAutocomplete();
        }
        EncounterClinicalNote.LoadClinicalTemplate();
    },

    LoadClinicalTemplate: function () {

        if (EncounterClinicalNote.params.mode == "Edit") {
            var self = $("#pnlEncounterClinicalNote");
            var myJson = self.getMyJSON();

            EncounterClinicalNote.FillVisitTemplate(myJson, EncounterClinicalNote.params.VisitId, EncounterClinicalNote.params.patientID).done(function (response) {
                if (response.status != false) {
                    var Visit_Template_Detail = JSON.parse(response.VisitTemplateLoad_JSON);
                    var HTMLTemplate = Visit_Template_Detail[0].HTMLTemplate;
                    HTMLTemplate = HTMLTemplate.replace(/&quot;/g, '"');
                    HTMLTemplate = HTMLTemplate.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                    $('#HTMLTemplate').html(HTMLTemplate);

                    $("#HTMLTemplate  input[id^='dpQuestion_']").datepicker({
                    }).on('changeDate', function (e) {
                        $(this).datepicker('hide');
                        $('#frmEncounterClinicalNote').bootstrapValidator('revalidateField', 'DOS');
                    });
                    $('#HTMLTemplate  input[id^="tpQuestion"]').timepicker({
                        defaultTime: '12:00 PM'
                        //defaultTime: false
                    });

                }
                
                $('#HTMLTemplate input').focus(function () {
                    var id = $(this).attr('id');
                    previous = this.value;
                    var QG_Q_QT = id.split('_');
                    var questionGroupQuestionId = QG_Q_QT[2];
                    var questionId = QG_Q_QT[1];
                    var questionType;
                    if (id.indexOf("Fraction") > -1) {
                        questionType = 5;
                    }
                    if (id.indexOf("Number") > -1) {
                        questionType = 10;
                    }
                    if (id.indexOf("tpQ") > -1) {
                        questionType = 7;
                    }
                    if (id.indexOf("Text") > -1) {
                        questionType = 1;
                    }
                    if (id.indexOf("dpQ") > -1) {
                        questionType = 6;
                    }
                    var divId = questionGroupQuestionId + '_' + questionId + '_' + questionType;
                    var divClass = $('#' + divId)[0].className;
                    var divStart = '<div class="' + divClass + '" id="' + divId + '">';
                    var element = $('#' + questionGroupQuestionId + '_' + questionId + '_' + questionType).html();
                    var divEnd = '</div>';
                    currentHTML = divStart + element + divEnd;

                }).change(function () {
                    if (this.type != "radio") {
                        $(this).attr('value', $(this).val());
                        var id = $(this).attr('id');
                        var QG_Q_QT = id.split('_');
                        var questionGroupQuestionId = QG_Q_QT[2];
                        var questionId = QG_Q_QT[1];
                        var questionType;
                        if (id.indexOf("Fraction") > -1) {
                            questionType = 5;
                        }
                        if (id.indexOf("Number") > -1) {
                            questionType = 10;
                        }
                        if (id.indexOf("tpQ") > -1) {
                            questionType = 7;
                        }
                        if (id.indexOf("Text") > -1) {
                            questionType = 1;
                        }
                        if (id.indexOf("dpQ") > -1) {
                            questionType = 6;
                        }
                        if (id.indexOf("imgField") > -1) {
                            questionType = 9;
                        }
                        var divId = questionGroupQuestionId + '_' + questionId + '_' + questionType;
                        var divClass = $('#' + divId)[0].className;
                        var divStart = '<div class="' + divClass + '" id="' + divId + '">';
                        var element = $('#' + questionGroupQuestionId + '_' + questionId + '_' + questionType).html();
                        var divEnd = '</div>';
                        currentHTML = divStart + element + divEnd;

                        EncounterClinicalNote.GetChunkOfHTML(previousHTML, currentHTML);
                    }
                });

                $("#HTMLTemplate select").focus(function () {
                    // Store the current value on focus, before it changes
                    var id = $(this).attr('id');
                    previous = this.value;

                    var QG_Q_QT = id.split('_');
                    var questionGroupQuestionId = QG_Q_QT[2];
                    var questionId = QG_Q_QT[1];
                    var questionType = 3;
                    var divId = questionGroupQuestionId + '_' + questionId + '_' + questionType;
                    var divClass = $('#' + divId)[0].className;
                    var divStart = '<div class="' + divClass + '" id="' + divId + '">';
                    var element = $('#' + questionGroupQuestionId + '_' + questionId + '_' + questionType).html();
                    var divEnd = '</div>';
                    previousHTML = divStart + element + divEnd;

                }).change(function (data) {
                    var id = $(this).attr('id');
                    var SelectedId = $("option:selected", $(this))[0];
                    $('#' + $(this).attr('id') + '> option').each(function () {
                        $(this).attr("selected", false);
                    });
                    $('#' + id + " option[value=" + SelectedId.value + "]").attr('selected', 'selected');
                    var selectedValue = $('#' + id + " option[value=" + SelectedId.value + "]");
                    $('#' + id).val(SelectedId.value);

                    var QG_Q_QT = id.split('_');
                    var questionGroupQuestionId = QG_Q_QT[2];
                    var questionId = QG_Q_QT[1];
                    var questionType = 3;
                    var divId = questionGroupQuestionId + '_' + questionId + '_' + questionType;
                    var divClass = $('#' + divId)[0].className;
                    var divStart = '<div class="' + divClass + '" id="' + divId + '">';
                    var element = $('#' + questionGroupQuestionId + '_' + questionId + '_' + questionType).html();
                    var divEnd = '</div>';
                    currentHTML = divStart + element + divEnd;
                    //alert(previousHTML + currentHTML);

                    EncounterClinicalNote.GetChunkOfHTML(previousHTML, currentHTML);
                });

                //$('#HTMLTemplate select').change(function (data) {
                //    var id = $(this).attr('id');
                //    var SelectedId = $("option:selected", $(this))[0];
                //    $('#' + $(this).attr('id') + '> option').each(function () {
                //        $(this).attr("selected", false);
                //    });
                //    $('#' + id + " option[value=" + SelectedId.value + "]").attr('selected', 'selected');
                //    var selectedValue = $('#' + id + " option[value=" + SelectedId.value + "]");
                //    $('#' + id).val(SelectedId.value);
                //});

                $('input:radio').focus(function () {
                    // Store the current value on focus, before it changes
                    previous = this.checked;
                }).change(function () {
                    var id = $(this).attr('id');
                    $(this).closest('div').parent().find('input:radio').attr('checked', false);
                    $(this).prop('checked', true);
                    $(this).attr('checked', 'checked');
                    alert(previous + id);
                });
            });
            //EncounterClinicalNote.ValidationClinical_Template_DesignView();

        }
    },

    GetChunkOfHTML: function (previousHTML, currentHTML) {

        var self = $("#" + EncounterClinicalNote.params["PanelID"]);
        var myJson = self.getMyJSON();
        var arrAnswer = [];
        $("#HTMLTemplate div[id$='_TemplateSection']").each(function () {
            var SectionId = $(this).attr('id');
            $("#HTMLTemplate #" + SectionId + " div[id$='_SectionQuestionGroup']").each(function () {
                var QuestionGroupId = $(this).attr('id');

                $("#HTMLTemplate #" + SectionId + " div[id=" + QuestionGroupId + "] input, #HTMLTemplate #" + SectionId + " div[id=" + QuestionGroupId + "] select").each(function () {
                    if ($(this).attr("type") == "text") {
                        // alert('SectionID = ' + SectionId.split('_')[0] + ' ---  QuestionGroupID = ' + QuestionGroupId.split('_')[0] + 'QuestionID =' + $(this).attr('id') + '   Value = ' + $(this).val())
                        arrAnswer.push(SectionId.split('_')[0] + '_' + QuestionGroupId.split('_')[0] + '_' + $(this).attr('id') + "_" + $(this).val() + '_' + $(this).val());
                    }
                    else if ($(this).attr("type") == "select") {
                        var i = this.selectedIndex;
                        var selectedText = this.options[i].text;
                        var selectedVal = this.options[i].value;
                        // alert('SectionID = ' + SectionId.split('_')[0] + ' ---  QuestionGroupID = ' + QuestionGroupId.split('_')[0] + 'QuestionID =' + $(this).attr('id') + "  Text is :" + selectedText + ', Value is :' + selectedVal);
                        arrAnswer.push(SectionId.split('_')[0] + '_' + QuestionGroupId.split('_')[0] + '_' + $(this).attr('id') + "_" + selectedText + '_' + selectedVal);
                    }
                    else if ($(this).attr("type") == "radio") {
                        if (!($(this).is(':checked'))) {
                            // alert('SectionID = ' + SectionId.split('_')[0] + ' ---  QuestionGroupID = ' + QuestionGroupId.split('_')[0] + 'QuestionID = ' + $(this).attr('id') + 'Value =  Checked');
                        }
                        else {
                            var radioName = $(this).attr('name');
                            arrAnswer.push(SectionId.split('_')[0] + '_' + QuestionGroupId.split('_')[0] + '_' + $(this).attr('id') + "_" + $('input[name=' + radioName + ']:checked').attr('id') + "_" + $('input[name=' + radioName + ']:checked').next().text());
                            // alert('SectionID = ' + SectionId.split('_')[0] + ' ---  QuestionGroupID = ' + QuestionGroupId.split('_')[0] + 'QuestionID = ' + $(this).attr('id') + 'Value = Not Checked');
                        }
                    }
                });
            });
        });
        var Answer = arrAnswer;
        EncounterClinicalNote.SaveClinicalTemplateDesignView(myJson, EncounterClinicalNote.params.VisitId, EncounterClinicalNote.params.patientID, previousHTML, currentHTML, Answer).done(function (response) {
            //            if (response.status != false) {
        });

    },

    ValidationClinical_Template_DesignView: function () {
        $('#frmEncounterClinicalNote')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           EncounterClinicalNote.Clinical_Template_DesignView_Save();
       });
    },

    Clinical_Template_DesignView_Save: function () {
        //var strMessage = "";
        var self = $("#" + EncounterClinicalNote.params["PanelID"]);
        //var myJson = self.getMyJSON();
        //if (EncounterClinicalNote.params.mode == "Edit") {
        //    if (strMessage == "") {
        //        EncounterClinicalNote.SaveClinicalTemplateDesignView(myJson, EncounterClinicalNote.params.VisitId, EncounterClinicalNote.params.patientID).done(function (response) {
        //            if (response.status != false) {
        //                //load Templates Details    Clinical_Question_Group.QuestionGroupSearch(0, 1, 15);
        //                utility.DisplayMessages(response.message, 1);
        //            }
        //            else {
        //                utility.DisplayMessages(response.Message, 3);
        //            }
        //        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //}

        var arrAnswer = [];
        $("#HTMLTemplate div[id$='_TemplateSection']").each(function () {
            var SectionId = $(this).attr('id');
            $("#HTMLTemplate #" + SectionId + " div[id$='_SectionQuestionGroup']").each(function () {
                var QuestionGroupId = $(this).attr('id');

                $("#HTMLTemplate #" + SectionId + " div[id=" + QuestionGroupId + "] input, #HTMLTemplate #" + SectionId + " div[id=" + QuestionGroupId + "] select").each(function () {
                    if ($(this).attr("type") == "text") {
                       // alert('SectionID = ' + SectionId.split('_')[0] + ' ---  QuestionGroupID = ' + QuestionGroupId.split('_')[0] + 'QuestionID =' + $(this).attr('id') + '   Value = ' + $(this).val())
                        arrAnswer.push(SectionId.split('_')[0] + '_' + QuestionGroupId.split('_')[0] + '_' + $(this).attr('id') + "_" + $(this).val() + '_' + $(this).val());
                    }
                    else if ($(this).attr("type") == "select") {
                        var i = this.selectedIndex;
                        var selectedText = this.options[i].text;
                        var selectedVal = this.options[i].value;
                       // alert('SectionID = ' + SectionId.split('_')[0] + ' ---  QuestionGroupID = ' + QuestionGroupId.split('_')[0] + 'QuestionID =' + $(this).attr('id') + "  Text is :" + selectedText + ', Value is :' + selectedVal);
                        arrAnswer.push(SectionId.split('_')[0] + '_' + QuestionGroupId.split('_')[0] + '_' + $(this).attr('id') + "_" + selectedText + '_' + selectedVal);
                    }
                    else if ($(this).attr("type") == "radio") {
                        if (!($(this).is(':checked'))) {
                           // alert('SectionID = ' + SectionId.split('_')[0] + ' ---  QuestionGroupID = ' + QuestionGroupId.split('_')[0] + 'QuestionID = ' + $(this).attr('id') + 'Value =  Checked');
                        }
                        else {
                            var radioName = $(this).attr('name');
                            arrAnswer.push(SectionId.split('_')[0] + '_' + QuestionGroupId.split('_')[0] + '_' + $(this).attr('id') + "_" + $('input[name=' + radioName + ']:checked').attr('id') + "_" + $('input[name=' + radioName + ']:checked').next().text());
                           // alert('SectionID = ' + SectionId.split('_')[0] + ' ---  QuestionGroupID = ' + QuestionGroupId.split('_')[0] + 'QuestionID = ' + $(this).attr('id') + 'Value = Not Checked');
                        }
                    }
                });
            });
        });
        var Answer = arrAnswer;
        var myJson = self.getMyJSON();
        if (previousHTML == 'undefined') previousHTML = null;
        if (currentHTML == 'undefined') currentHTML = null;
        EncounterClinicalNote.SaveClinicalTemplateDesignView(myJson, EncounterClinicalNote.params.VisitId, EncounterClinicalNote.params.patientID,previousHTML,currentHTML,  Answer).done(function (response) {
            //            if (response.status != false) {
        });
    },

    SaveClinicalTemplateDesignView: function (clinicalTemplateDesignViewData, VisitId, PatientID, previousHTML, currentHTML, Answer) {

        //var data = "clinicalTemplateDesignViewData=" + clinicalTemplateDesignViewData + "&VisitId=" + VisitId + "&PatientID=" + PatientID + "&ProgressNoteHTML=" + $('#HTMLTemplate').html() + "&Answer=" + Answer;
        var data = "clinicalTemplateDesignViewData=" + clinicalTemplateDesignViewData + "&VisitId=" + VisitId + "&PatientID=" + PatientID + "&ProgressNoteHTML=" + $('#HTMLTemplate').html() + "&previousHTML=" + previousHTML + "&currentHTML=" + currentHTML + "&Answer=" + Answer;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Encounter_ClinicalNote", "SAVE_VISIT_TEMPLATE_DESIGN_VIEW");
    },

    FillVisitTemplate: function (visitData, visitId, patientId) {

        var data = "visitId=" + visitId + "&patientId=" + patientId + "&visitData=" + visitData;;
        return MDVisionService.defaultService(data, "Encounter_ClinicalNote", "FILL_VISIT_TEMPLATE");
    },

}