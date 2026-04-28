Admin_CCMQuestionDetails = {
    params: [],
    bIsFirstLoad: true,
    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    BillId: -1,
    Load: function (params) {
        Admin_CCMQuestionDetails.params = params;
        if (Admin_CCMQuestionDetails.params["PanelID"] != 'pnlAdminCCMQuestionDetails')
            Admin_CCMQuestionDetails.params["PanelID"] = Admin_CCMQuestionDetails.params["PanelID"] + ' #pnlAdminCCMQuestionDetails'
        if (Admin_CCMQuestionDetails.bIsFirstLoad) {
            Admin_CCMQuestionDetails.bIsFirstLoad = false;
            var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                Admin_CCMQuestionDetails.ValidateQuestionDetails(Admin_CCMQuestionDetails.params.QuestionID);
                if (Admin_CCMQuestionDetails.params.mode == "Edit") {
                    Admin_CCMQuestionDetails.HideAllControls();
                    if (Admin_CCMQuestionDetails.params.QuestionType == 'TextField') {
                        $('#divAnswerTextFieldAnswer').show();
                        $('#divPreviewTextFieldPreview').show();
                        Admin_CCMQuestionDetails.PopulateTextFieldForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == 'YesNo') {
                        $('#divAnswerYesNoAnswer').show();
                        $('#divPreviewYesNoFieldPreview').show();
                        Admin_CCMQuestionDetails.PopulateYesNoForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == 'Toggle') {
                        $('#divAnswerToggleAnswer').show();
                        $('#divPreviewToggleFieldPreview').show();
                        Admin_CCMQuestionDetails.PopulateToggleForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == 'SingleSelectDropdown') {
                        $('#divAnswerDropdownFieldAnswer').show();
                        $('#divPreviewDropdownFieldPreview').show();
                        Admin_CCMQuestionDetails.PopulateSingleSelectDropdownForm(Admin_CCMQuestionDetails.params.QuestionID);
                    } else if (Admin_CCMQuestionDetails.params.QuestionType == 'MultipleSelectCombo') {
                        $('#divAnswerMultiSelectDropdownFieldAnswer').show();
                        $('#divPreviewMultiSelectDropdownFieldPreview').show();
                        Admin_CCMQuestionDetails.PopulateMultiSelectComboForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == 'CheckBox') {
                        $('#divAnswerCheckBoxAnswer').show();
                        $('#divPreviewCheckBoxFieldPreview').show();
                        var formValidation = $("#" + Admin_CCMQuestionDetails.params["PanelID"] + ' #frmAdminCCMQuestionDetails').data("bootstrapValidator");
                        if (formValidation) {
                            formValidation.enableFieldValidators('Title', false);
                        }
                        $('#rfvTitle').hide();
                        Admin_CCMQuestionDetails.PopulateCheckBoxForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == 3) {
                        $('#divAnswerDropdownFieldAnswer').show();
                        $('#divPreviewDropdownFieldPreview').show();
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == 'FractionField') {
                        $('#divAnswerFractionFieldAnswer').show();
                        $('#divPreviewFractionFieldPreview').show();
                        $('#divQuestionDetailTitle').show();
                        $('#divQuestionDetailLabel').hide();
                        $('#divFractionFieldQuestion').show();
                        $('#divIsNumeric').hide();
                        Admin_CCMQuestionDetails.populateFractionFieldForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == 'DateField') {
                        $('#divAnswerDateFieldAnswer').show();
                        $('#divPreviewDateFieldPreview').show();
                        $('#divIsNumeric').hide();
                        utility.CreateDatePicker(Admin_CCMQuestionDetails.params.PanelID + ' #dtpDateFieldPreview', function () { });
                        $('#' + Admin_CCMQuestionDetails.params.PanelID + ' #dtpDefaultDate').datepicker({
                        }).on('changeDate', function (e) {
                            $(this).datepicker('hide');
                            $("#dtpDateFieldPreview").val($("#dtpDefaultDate").val());
                        });
                        Admin_CCMQuestionDetails.populateDateFieldForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == 'Header') {
                        $('#divAnswerHeader').show();
                        $('#divHeaderPreview').show();
                        $('#divQuestionDetailTitle').hide();
                        $('#divQuestionDetailLabel').hide();
                        $('#divIsNumeric').hide();
                        $("#divIsMandatory").hide();
                        Admin_CCMQuestionDetails.populateHeaderForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == "TimeField") {
                        $('#divAnswerTimeFieldAnswer').show();
                        $('#divPreviewTimeFieldPreview').show();
                        $('#divQuestionDetailTitle').show();
                        $('#divQuestionDetailLabel').hide();
                        $('#divIsNumeric').hide();
                        $('#tpDefaultTime').timepicker({
                            showMeridian: true,
                        }).on('changeTime.timepicker', function (e) {
                            //$(this).timepicker('hide');
                            $("#tpQuestion").val($("#tpDefaultTime").val());
                        });
                        Admin_CCMQuestionDetails.populateTimeFieldForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == "FreeText") {
                        $('#divAnswerFreeFieldAnswer').show();
                        $('#divPreviewFreeFieldPreview').show();
                        $('#divQuestionDetailTitle').show();
                        $('#divQuestionDetailLabel').hide();
                        $('#divIsNumeric').hide();
                        Admin_CCMQuestionDetails.populateFreeTextForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == "Table") {
                        $('#divAnswerTable').show();
                        $('#divPreviewTable').show();
                        $('#divQuestionDetailTitle').show();
                        $('#divQuestionDetailLabel').hide();
                        $('#divIsNumeric').hide();
                        Admin_CCMQuestionDetails.drawTable(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == "Image") {
                        $('#divAnswerImageFieldAnswer').show();
                        $('#divPreviewImageFieldPreview').show();
                        Admin_CCMQuestionDetails.PopulateImageForm(Admin_CCMQuestionDetails.params.QuestionID);
                    }
                    else if (Admin_CCMQuestionDetails.params.QuestionType == 10) {
                        $('#divAnswerNumberFieldAnswer').show();
                        $('#divPreviewNumberFieldPreview').show();
                    }
                    //Admin_CCMQuestionDetails.LoadQuestion();
                    Admin_CCMQuestionDetails.showHideMandatory();
                }

            });
        }
    },

    LoadQuestion: function (QuestionId, mode) {
        if (Admin_CCMQuestionDetails.params.mode == "Edit") {
            Admin_CCMQuestionDetails.FillQuestion(Admin_CCMQuestionDetails.params.QuestionId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Question_detail = JSON.parse(response.QuestionLoad_JSON);
                    var self = $('#pnlAdminCCMQuestionDetails');
                    utility.bindMyJSONByName(true, JSON.parse(response.QuestionLoad_JSON), false, self).done(function () {
                        $('#ShortName,#questionType').attr('disabled', true);
                        Admin_CCMQuestionDetails.getText();
                        $('#frmAdminCCMQuestionDetails').data('serialize', $('#frmAdminCCMQuestionDetails').serialize());
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    HideAllControls: function () {
        $('div[id^="divAnswer"]').hide();
        $('div[id^="divPreview"]').hide();
    },

    HideAllPreviewDivs: function () {
        $('div[id^="divPreview"]').hide();
    },

    FillQuestion: function (questionId) {
        var objData = new Object();
        objData["questionID"] = questionId;
        objData["commandType"] = "fill_question";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TEMPLATEBUILDER", "ClinicalQuestion");
    },

    ValidateQuestionDetails: function (QuestionID) {
        $('#frmAdminCCMQuestionDetails')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  QuestionName: {
                      group: '.col-xs-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  questionType: {
                      group: '.col-xs-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Title: {
                      group: '.col-xs-12',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Label: {
                      group: '.col-xs-12',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  uploadFile: {
                      group: '.col-sm-8',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           Admin_CCMQuestionDetails.saveQuestion(QuestionID);
       });
    },
    saveQuestion: function (QuestionID) {
        var self = $('#pnlAdminCCMQuestionDetails');
        var myJSON = self.getMyJSONByName();
        var filess = Admin_CCMQuestionDetails.FilesContainer.Files;
        if (Admin_CCMQuestionDetails.params.mode == "Edit") {
            var isQuestionExists = Admin_CCMQuestionDetails.QuestionExists();
            if (isQuestionExists == false) {
                var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
                var question = detailsForm.find("div[id='" + QuestionID + "']");
                var quesLi = $(question).closest('li');
                if (Admin_CCMQuestionDetails.params.QuestionType == 'TextField') {
                    Admin_CCMQuestionDetails.UpdateTextFieldInCustonFormDetails(Admin_CCMQuestionDetails.params.QuestionID);
                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);
                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'YesNo') {
                    Admin_CCMQuestionDetails.UpdateYesNoInTemplateDetails(Admin_CCMQuestionDetails.params.QuestionID);
                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);
                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'Toggle') {
                    Admin_CCMQuestionDetails.UpdateToggleInTemplateDetails(Admin_CCMQuestionDetails.params.QuestionID);
                    CCMTemplateDetails.updateTemplateHTML(quesLi);
                } else if (Admin_CCMQuestionDetails.params.QuestionType == 'SingleSelectDropdown') {
                    Admin_CCMQuestionDetails.UpdateSingleSelectDropdownInTemplateDetails(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);

                } else if (Admin_CCMQuestionDetails.params.QuestionType == 'MultipleSelectCombo') {
                    Admin_CCMQuestionDetails.UpdateMultiSelectComboInTemplateDetails(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);

                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'Image') {
                    Admin_CCMQuestionDetails.UpdateImageInTemplateDetails(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);

                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'FractionField') {
                    Admin_CCMQuestionDetails.updateFractionFieldForm(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);
                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'DateField') {
                    Admin_CCMQuestionDetails.updateDateFieldForm(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);
                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'Header') {
                    Admin_CCMQuestionDetails.updateHeaderForm(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);
                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'TimeField') {
                    Admin_CCMQuestionDetails.updateTimeFieldForm(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);
                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'CheckBox') {
                    Admin_CCMQuestionDetails.UpdateCheckBoxInTemplateDetails(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);
                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'FreeText') {
                    Admin_CCMQuestionDetails.updateFreeTextForm(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);
                }
                else if (Admin_CCMQuestionDetails.params.QuestionType == 'Table') {
                    Admin_CCMQuestionDetails.updateTable(Admin_CCMQuestionDetails.params.QuestionID);

                    Admin_CCMTemplateDetails.updateQuestionHTML(quesLi);
                }
                if (Admin_CCMQuestionDetails.params != null && Admin_CCMQuestionDetails.params.ParentCtrl != null) {
                    UnloadActionPan(Admin_CCMQuestionDetails.params.ParentCtrl, 'Admin_CCMQuestionDetails');
                }
                else
                    UnloadActionPan(null, 'Admin_CCMQuestionDetails');
            }
            else {
                utility.DisplayMessages("Question with the same name already exists.", 3);
            }
            //});
            //}
        }
    },

    QuestionSave: function (QuestionData, file) {

        var objData = JSON.parse(QuestionData);
        objData["commandType"] = "SAVE_QUESTION";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TEMPLATEBUILDER", "ClinicalQuestion");
    },

    QuestionEdit: function (QuestionData, QuestionID, file) {
        var objData = JSON.parse(QuestionData);
        objData["file"] = file;
        objData["questionID"] = QuestionID;
        objData["commandType"] = "UPDATE_QUESTION";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "TEMPLATEBUILDER", "ClinicalQuestion");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmAdminCCMQuestionDetails', function () {
            if (Admin_CCMQuestionDetails.params != null && Admin_CCMQuestionDetails.params.ParentCtrl != null) {
                UnloadActionPan(Admin_CCMQuestionDetails.params.ParentCtrl, 'Admin_CCMQuestionDetails');
            }
            else
                UnloadActionPan(null, 'Admin_CCMQuestionDetails');
        }, function () {
        });
    },

    QuestionTypeChanged: function (e, type) {

        var questionType = $('#questionType :selected').text();
        var questionTypeTrimed = $('#questionType :selected').text().replace(/ /g, '');

        $("#txtTitle").removeAttr("disabled");

        if (questionType === 'Radio Button') {

            Admin_CCMQuestionDetails.HideAllControls();
            $('#divAnswerRadioFieldAnswer').show();
            $('#divPreviewRadioFieldPreview').show();
        }
        else if (questionType === 'Text Field') {

            Admin_CCMQuestionDetails.HideAllControls();
            $('#divAnswerTextFieldAnswer').show();
            $('#divPreviewTextFieldPreview').show();
        }
        else if (questionType === 'Drop Down') {
            Admin_CCMQuestionDetails.HideAllControls();
            $('#divAnswerDropdownFieldAnswer').show();
            $('#divPreviewDropdownFieldPreview').show();
        }

        else if (questionType === 'Multi Select Drop Down') {
            Admin_CCMQuestionDetails.HideAllControls();
            $('#divAnswerMultiSelectDropdownFieldAnswer').show();
            $('#divPreviewMultiSelectDropdownFieldPreview').show();
        }
        else if (questionType === 'Fraction Field') {
            Admin_CCMQuestionDetails.HideAllControls();
            $('#divAnswerFractionFieldAnswer').show();
            $('#divPreviewFractionFieldPreview').show();
        }
        else if (questionType === 'Date') {
            Admin_CCMQuestionDetails.HideAllControls();
            $('#divAnswerDateFieldAnswer').show();
            $('#divPreviewDateFieldPreview').show();
        }
        else if (questionType === 'Time') {
            Admin_CCMQuestionDetails.HideAllControls();
            $('#divAnswerTimeFieldAnswer').show();
            $('#divPreviewTimeFieldPreview').show();
        }
        else if (questionType === 'Image Field') {
            Admin_CCMQuestionDetails.HideAllControls();
            $('#divAnswerImageFieldAnswer').show();
            $('#divPreviewImageFieldPreview').show();
        }
        else if (questionType === 'Number Field') {
            Admin_CCMQuestionDetails.HideAllControls();
            $('#divAnswerNumberFieldAnswer').show();
            $('#divPreviewNumberFieldPreview').show();
        }
    },

    getText: function () {
        var questionType = $('#questionType :selected').text();
        if (questionType === 'Radio Button') {

            Admin_CCMQuestionDetails.HideAllPreviewDivs();
            $('#divPreviewRadioFieldPreview').show();

        } else if (questionType === 'Text Field') {

            Admin_CCMQuestionDetails.HideAllPreviewDivs();
            $('#divPreviewTextFieldPreview').show();

        } else if (questionType === 'Drop Down') {

            Admin_CCMQuestionDetails.HideAllPreviewDivs();
            $('#divPreviewDropdownFieldPreview').show();

        } else if (questionType === 'Multi Select Drop Down') {

            Admin_CCMQuestionDetails.HideAllPreviewDivs();
            $('#divPreviewMultiSelectDropdownFieldPreview').show();

        } else if (questionType === 'Fraction Field') {

            Admin_CCMQuestionDetails.HideAllPreviewDivs();
            $('#divPreviewFractionFieldPreview').show();

        } else if (questionType === 'Date') {

            Admin_CCMQuestionDetails.HideAllPreviewDivs();
            $('#divPreviewDateFieldPreview').show();

        } else if (questionType === 'Time') {

            Admin_CCMQuestionDetails.HideAllPreviewDivs();
            $('#divPreviewTimeFieldPreview').show();

        } else if (questionType === 'Image Field') {

            Admin_CCMQuestionDetails.HideAllPreviewDivs();
            $('#divPreviewImageFieldPreview').show();

        } else if (questionType === 'Number Field') {

            Admin_CCMQuestionDetails.HideAllPreviewDivs();
            $('#divPreviewNumberFieldPreview').show();

        }

        var reqText = $('#lblTextFieldPreview').find('.required');
        $('#lblTextFieldPreview').text($('#txtTitle').val());
        //if (reqText.length > 0)
        //    $('#lblTextFieldPreview').append(reqText);

        $('#lblRadioFieldPreview').text($('#txtTitle').val());
        $('#lblDropdownFieldPreview').text($('#txtTitle').val());
        $('#lblMultiSelectDropdownFieldPreview').text($('#txtTitle').val());
        var reqFrac = $('#lblFractionFieldPreview').find('.required');
        $('#lblFractionFieldPreview').text($('#txtTitle').val());
        //if (reqFrac.length > 0)
        //    $('#lblFractionFieldPreview').append(reqFrac);

        var reqDate = $('#lblDateFieldPreview').find('.required');
        $('#lblDateFieldPreview').text($('#txtTitle').val());
        //if (reqDate.length > 0)
        //    $('#lblDateFieldPreview').append(reqDate);
        var reqTime = $('#lblTimeFieldPreview').find('.required');
        $('#lblTimeFieldPreview').text($('#txtTitle').val());
        //if (reqTime.length > 0)
        //    $('#lblTimeFieldPreview').append(reqTime);
        $('#lblImageFieldPreview').text($('#txtTitle').val());
        var reqNo = $('#lblNumberFieldPreview').find('.required');
        $('#lblNumberFieldPreview').text($('#txtTitle').val());
        //if (reqNo.length > 0)
        //    $('#lblNumberFieldPreview').append(reqNo);
        var reqFraction = $('#lblFractionTitlePreview').find('.required');
        $('#lblFractionTitlePreview').text($('#txtTitle').val());
        //if (reqFraction.length > 0)
        //    $('#lblFractionTitlePreview').append(reqFraction);
        var reqFree = $('#lblFreeFieldPreview').find('.required');
        $('#lblFreeFieldPreview').text($('#txtTitle').val());
        //if (reqFree.length > 0)
        //    $('#lblFreeFieldPreview').append(reqFree);
        //  var reqTable = $('#lblTableTitlePreview').find('.required');
        $('#lblTableTitlePreview').text($('#txtTitle').val());
        //if (reqFree.length > 0)
        //    $('#lblTableTitlePreview').append(reqFree);
        $('#lblCheckBoxTitlePreview').text($('#txtTitle').val());
        Admin_CCMQuestionDetails.showHideMandatory();
    },

    getRadioLabel1Text: function () {

        $('#lblRadioFieldPreview1').text($('#questionRadioLabel1').val());
    },

    getRadioLabel2Text: function () {

        $('#lblRadioFieldPreview2').text($('#questionRadioLabel2').val());
    },

    getFractionDefaultValue1: function () {

        $('#txtFractionFieldPreview1').val($('#txtFarctionFieldDefaultValue1').val());
    },

    getFractionDefaultValue2: function () {

        $('#txtFractionFieldPreview2').val($('#txtFarctionFieldDefaultValue2').val());
    },

    ddlElementsFromTextArea: function () {
        var data = ['- SELECT -'];
        var lines = $('#txtareaElementsDropDown').val();
        lines = lines.split(/\n/);
        var arraytoCommaSeperatedString = lines.join(',');
        var commaSeperatedStringtoArray = arraytoCommaSeperatedString.split(','); //alert(commaSeperatedStringtoArray);
        //var texts = [];
        for (var i = 0; i < lines.length; i++) {
            if (/\S/.test(lines[i])) {
                //texts.push($.trim(lines[i]));
                data.push($.trim(lines[i]));
            }
        }
        //alert(JSON.stringify(texts));
        var ddlElements = $('#ddlElements');
        for (var val in data) {
            $('<option />', { value: val, text: data[val] }).appendTo(ddlElements);
        }
    },

    ddlMultiSelectElementsFromTextArea: function () {
        var data = ['- SELECT -'];
        var lines = $('#txtareaElementsDropDownMultiSelect').val();
        lines = lines.split(/\n/);
        var arraytoCommaSeperatedString = lines.join(',');
        var commaSeperatedStringtoArray = arraytoCommaSeperatedString.split(','); //alert(commaSeperatedStringtoArray);
        for (var i = 0; i < lines.length; i++) {
            if (/\S/.test(lines[i])) {
                data.push($.trim(lines[i]));
            }
        }
        var ddlElements = $('#ddlMultiSelectElements');
        for (var val in data) {
            $('<option />', { value: val, text: data[val] }).appendTo(ddlElements);
        }
    },

    GetLabelText: function myfunction(e, idtxtarea, iddrpdwn) {
        var box;
        if (e.innerHTML == "") {
            var selectedValue = $('#' + iddrpdwn + ' :selected').text();
            var val = ' {{' + selectedValue + '}} ';
            box = $("#" + idtxtarea);
            box.val(box.val() + val);
        }
        else {
            var txt = ' {{' + e.innerHTML + '}} ';
            box = $("#" + idtxtarea);
            box.val(box.val() + txt);
        }
    },


    ValidateUploadedFiles: function () {
        var fileName = "";
        var files = $('#Upload_Import_file').get(0).files;
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "image/jpeg" && fileType != "image/png" && fileType != "image/jpg" && fileType != "image/gif" && fileType != "image/bmp") {
                utility.DisplayMessages("File Type is Invalid", 4);
                return false;
            }
            if (Admin_CCMQuestionDetails.ValidateFileSize(files) > Number(globalAppdata['FileSize'])) {
                utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
                return false;
            }
            fileName = fileName + files[i].name + ',';
        }
        fileName = fileName.slice(0, fileName.length - 1);
        document.getElementById("uploadFilePH").value = fileName;
        $('#totalFiles').text(files.length + " file(s) selected");
        $("#frmAdminCCMQuestionDetails").bootstrapValidator('revalidateField', 'uploadFile');
        return true;
    },

    ValidateFileSize: function (files) {
        var size = 0;
        $.each(files, function (index, file) {
            size = size + Number((file.size / (1024 * 1024)).toFixed(2));
        });
        return size;
    },
    PopulateTextFieldForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        if (question) {
            var name = $(question).attr("name");
            if (name) {
                self.find("#txtQuestionName").val(name);
            }
            var isnumber = $(question).attr("isnumber");
            if (isnumber) {
                if (isnumber == "true") {
                    Admin_CCMQuestionDetails.populateNumberValue(question, self);
                }
                else {
                    Admin_CCMQuestionDetails.populateTextValue(question, self);
                }
            }
            //var isnewline = $(question).attr("isnewline");
            //if (isnewline) {
            //    if (isnewline == "true")
            //        self.find("#chkNewLine").attr('checked', 'checked');
            //}
            var ismandatory = $(question).attr("ismandatory");
            if (ismandatory) {
                if (ismandatory == "true")
                    self.find("#chkMandatory").attr('checked', 'checked');
            }
        }
    },
    populateNumberValue: function (question, self) {
        self.find("#chkNumber").attr('checked', 'checked');
        $('#divAnswerNumberFieldAnswer').show();
        $('#divPreviewNumberFieldPreview').show();
        $('#divAnswerTextFieldAnswer').hide();
        $('#divPreviewTextFieldPreview').hide();
        var maxlength = $(question).attr("max");
        if (maxlength) {
            self.find("#txtNumberMaxLength").val(maxlength);
        }
        var minLength = $(question).attr("min");
        if (minLength) {
            self.find("#txtNumberMinLength").val(minLength);
        }
        var defaultvalue = $(question).attr("defaultvalue");
        if (defaultvalue) {
            self.find("#txtNumberQuestionDefaultValue").val(defaultvalue);
            self.find("#txtNumericDefaultPreview").val(defaultvalue);
        }
        var precision = $(question).attr("precision");
        if (precision) {
            self.find("#questionNumberPrecisiontxt").val(precision);
        }
        var questiontitle = $(question).attr("questiontitle");
        if (questiontitle) {
            self.find("#txtTitle").val(questiontitle.replace('*', ''));
            self.find("#lblNumberFieldPreview").text(questiontitle.replace('*', ''));
        }
        Admin_CCMQuestionDetails.setNumericDefaultValue();
    },

    // Start for handeling Image Form - ZeeshanAK on 10 October, 2016
    PopulateImageForm: function (QuestionID) {
        $('#chkNumber').parent().hide();
        $('#chkMandatory').parent().hide();

        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        if (question) {
            var name = $(question).attr("name");
            if (name != "") {
                self.find("#txtQuestionName").val(name);
            } else {
                $("#txtQuestionName").attr('placeholder', 'Image Field');
            }
            var questionlabel = $(question).attr("questionlabel");
            if (questionlabel) {
                self.find("#txtTitle").val(questionlabel);
            }
            if (question.find('#TemplateImage img').length > 0) {

                $('#TemplateImagePreview').parent().html(question.find('#TemplateImage').html());
                $('#totalFiles').text('1 file(s) selected');
                $('#uploadFilePH').val($(question).attr("filename"));

            } else {
            }
        }
        $("#lblImageFieldPreview").text($("#txtTitle").val());
    },
    UpdateImageInTemplateDetails: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }
        if (question) {
            var name = self.find("#txtQuestionName").val();
            if (name) {
                $(question).attr("name", name);
            }
            var questionlabel = self.find("#txtTitle").val();
            if (questionlabel) {
                $(question).attr("questionlabel", questionlabel);
            }
            question.find('#TemplateImageLabel').text(questionlabel);
            question.find('#TemplateImage').empty().html($('#TemplateImagePreview').parent().html());

            var fileName = self.find('#uploadFilePH').val();
            if (fileName != null) {
                $(question).attr("filename", fileName);
            }
            // if (title) {
            //$(question).find("label").attr('data-toggle', 'tooltip');
            $(question).find("label").attr('data-original-title', questionlabel);
        }
    },
    BufferFile: function (obj) {
        var toReturn = true;

        if (obj.files) {
            if (Admin_CCMQuestionDetails.ValidateUploadedFiles()) {
                Admin_CCMQuestionDetails.FilesContainer.Files = obj.files;
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#TemplateImagePreview').attr('src', e.target.result);
                    $('#TemplateImagePreview').show();
                }
                reader.readAsDataURL(obj.files[0]);
            }
        }
        return toReturn;
    },

    // End for handeling Image Form - ZeeshanAK on 10 October, 2016






    // Start for handeling Multiple Select Dropdown Form - ZeeshanAK on 07 October, 2016
    PopulateMultiSelectComboForm: function (QuestionID) {
        $('#chkNumber').parent().addClass('hidden');
        $('#divQuestionDetailTitle').addClass('hidden');
        $('#divQuestionDetailLabel').removeClass('hidden');
        Admin_CCMQuestionDetails.ddlMultiSelectFromTextArea();

        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        if (question) {
            var name = $(question).attr("name");
            if (name != "") {
                self.find("#txtQuestionName").val(name);
            } else {
                $("#txtQuestionName").attr('placeholder', 'Multiple Dropdown Field');
            }
            var ismandatory = $(question).attr("ismandatory");
            if (ismandatory) {
                if (ismandatory == "true")
                    self.find("#chkMandatory").prop('checked', true)
            }
            var questionlabel = $(question).attr("questionlabel");
            if (questionlabel) {
                self.find("#txtLabel").val(questionlabel);
            }
            $("#lblMultiSelectDropdownFieldPreview").text(questionlabel);

            var dropdownvalues = $(question).attr("dropdownvalues");
            if (dropdownvalues && dropdownvalues.length > 0) {
                dropdownvalues = JSON.parse(dropdownvalues);
                var s = "";
                $.each(dropdownvalues, function (i, e) {
                    i == 0 ? s += e : s += "\n " + e
                });
                $('#txtareaElementsDropDownMultiSelect').val(s);
                Admin_CCMQuestionDetails.ddlMultiSelectFromTextArea();
            }

            var defaultvalue = $(question).attr("defaultselection");
            if (defaultvalue) {
                $("#ddlMultiSelectComboDefaultValues").val('');
                EMRUtility.selectOptionsByCommaSeprateValue($("#ddlMultiSelectComboDefaultValues"), defaultvalue);
                $('#ddlMultiSelectComboDefaultValues').multiselect("refresh");
                Admin_CCMQuestionDetails.changeDefaultValueMultiSelectCombo();
            }

        }

    },
    UpdateMultiSelectComboInTemplateDetails: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }

        if (question) {
            var name = self.find("#txtQuestionName").val();
            if (name) {
                $(question).attr("name", name);
            }
            var ismandatory = self.find("#chkMandatory").is(':checked');
            if (ismandatory != null) {
                $(question).attr("ismandatory", ismandatory);
            }
            var questionlabel = self.find("#txtLabel").val();
            if (questionlabel) {
                $(question).attr("questionlabel", questionlabel);

                question.find('#TemplateMultipleSelectComboLabel').empty().html($('#lblMultiSelectDropdownFieldPreview').html());
            }
            var defaultselection = self.find("#ddlMultiSelectComboDefaultValues").val();
            if (defaultselection != null) {
                defaultselection = defaultselection.join();
                $(question).attr("defaultselection", defaultselection);
            }
            else {
                $(question).attr("defaultselection", "");
            }

            var data = [];
            var lines = $('#txtareaElementsDropDownMultiSelect').val();
            lines = lines.split(/\n/);

            for (var i = 0; i < lines.length; i++) {
                if (/\S/.test(lines[i])) {
                    data.push($.trim(lines[i]));
                }
            }

            var dropdownvalues = JSON.stringify(data);
            if (dropdownvalues) {
                $(question).attr("dropdownvalues", dropdownvalues);
            }

            //$('#ddlMultiSelectComboPreview option').each(function (i, e) {
            //    $.each(defaultselection.split(','), function (ii, ee) {
            //        if ($(e).val() == ee) {
            //            $(e).attr('selected', 'selected');
            //        }
            //    });
            //});
            question.find('#toolSingleSelectDropdown_').empty().append($('#ddlMultiSelectComboPreview').html());
            if (!$(question).find("label").attr('data-toggle'))
                $(question).find("label").attr('data-toggle', 'tooltip');
            $(question).find("label").attr('data-original-title', questionlabel);
            if ($("[id^='customFormMultipleSelectCombo_").find('select').length > 0) {
                Admin_CCMTemplatePreview.initilizeMultiSelectTemplatePreview();
            }
        }
    },
    updateMultiSelectComboPreview: function () {
        $("#lblMultiSelectDropdownFieldPreview").text($("#txtLabel").val());
        Admin_CCMQuestionDetails.showHideMandatory();
    },
    changeDefaultValueMultiSelectCombo: function (obj) {
        var defaultVal = $("#ddlMultiSelectComboDefaultValues").val();
        if (defaultVal != null) {
            var defaultVals = $("#ddlMultiSelectComboDefaultValues").val().join();
            var defaultText = $("#ddlMultiSelectComboDefaultValues option:selected").map(function () {
                return this.text
            });

            if (typeof (defaultText) != "undefined" && defaultText.length > 0) {
                var s = "";
                $.each(defaultText, function (i, e) {
                    i == 0 ? s += e : s += "\n " + e
                });
                $('#defaultValuesCombo').val('').val(s);
            }
            $("#ddlMultiSelectComboPreview").val('');
            EMRUtility.selectOptionsByCommaSeprateValue($("#ddlMultiSelectComboPreview"), defaultVals);
            $('#ddlMultiSelectComboPreview').multiselect("refresh");
        }
        else {
            $('#defaultValuesCombo').val('');
            $("#ddlMultiSelectComboPreview").val('');
            $('#ddlMultiSelectComboPreview').multiselect("refresh");
        }

    },
    ddlMultiSelectFromTextArea: function () {
        var ddlMultiSelectComboPreview = $('#ddlMultiSelectComboPreview');
        var ddlMultiSelectComboDefault = $('#ddlMultiSelectComboDefaultValues');
        ddlMultiSelectComboPreview.empty();
        ddlMultiSelectComboDefault.empty();

        var labels = [];
        var ddlElement = $('#ddlMultiSelectDefaultValues');
        var lines = $('#txtareaElementsDropDownMultiSelect').val();
        lines = lines.split(/\n/);

        for (var i = 0; i < lines.length; i++) {
            if (/\S/.test(lines[i])) {
                labels.push($.trim(lines[i]));
            }
        }

        $.each(labels, function (i, item) {
            if (item != '' && item != null) {
                ddlMultiSelectComboDefault.append($('<option/>', { value: i, html: item, }));
                ddlMultiSelectComboPreview.append($('<option/>', { value: i, html: item, }));
            }
        });

        ddlMultiSelectComboDefault.multiselect('destroy');
        ddlMultiSelectComboPreview.multiselect('destroy');

        ddlMultiSelectComboDefault.multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            onDropdownShow: function (event) {
                ddlMultiSelectComboDefault.parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
            },
        });
        ddlMultiSelectComboPreview.multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            onDropdownShow: function (event) {
                ddlMultiSelectComboPreview.parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
            },
        });
    },
    // End for handeling Multiple Select Dropdown Form - ZeeshanAK on 07 October, 2016


    // Start for handeling Single Select Dropdown Form - ZeeshanAK on 05 October, 2016
    PopulateSingleSelectDropdownForm: function (QuestionID) {
        $('#chkNumber').parent().addClass('hidden');
        $('#divQuestionDetailTitle').addClass('hidden');
        $('#divQuestionDetailLabel').removeClass('hidden');

        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        if (question) {
            var name = $(question).attr("name");
            if (name != "") {
                self.find("#txtQuestionName").val(name);
            } else {
                $("#txtQuestionName").attr('placeholder', 'Single Dropdown Field');
            }
            var ismandatory = $(question).attr("ismandatory");
            if (ismandatory) {
                if (ismandatory == "true")
                    self.find("#chkMandatory").prop('checked', true)
            }
            var questionlabel = $(question).attr("questionlabel");
            if (questionlabel) {
                self.find("#txtLabel").val(questionlabel);
            }
            $("#lblDropdownFieldPreview").text(questionlabel);

            var dropdownvalues = $(question).attr("dropdownvalues");
            if (dropdownvalues.length > 0) {
                dropdownvalues = JSON.parse(dropdownvalues);
                var s = "";
                $.each(dropdownvalues, function (i, e) {
                    i == 0 ? s += e : s += "\n " + e
                });
                $('#txtareaElementsDropDown').val(s);
                Admin_CCMQuestionDetails.ddlElementsFromTextArea();
            }

            var defaultvalue = $(question).attr("defaultselection");
            if (defaultvalue) {
                self.find("#ddlSingleSelectDefaultValues").val(defaultvalue);
            }
            Admin_CCMQuestionDetails.changeDefaultValue();
        }

    },
    UpdateSingleSelectDropdownInTemplateDetails: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }

        if (question) {
            var name = self.find("#txtQuestionName").val();
            if (name) {
                $(question).attr("name", name);
            }

            var ismandatory = self.find("#chkMandatory").is(':checked');
            if (ismandatory != null) {
                $(question).attr("ismandatory", ismandatory);
            }
            var questionlabel = self.find("#txtLabel").val();
            if (questionlabel) {
                $(question).attr("questionlabel", questionlabel);

                question.find('#TemplateSingleSelectDropdownLabel').empty().html($('#lblDropdownFieldPreview').html());
            }
            var defaultselection = self.find("#ddlSingleSelectDefaultValues option:selected").val();
            if (defaultselection) {
                $(question).attr("defaultselection", defaultselection);
            }
            else {
                $(question).attr("defaultselection", "");
            }
            var data = [];
            var lines = $('#txtareaElementsDropDown').val();
            lines = lines.split(/\n/);

            for (var i = 0; i < lines.length; i++) {
                if (/\S/.test(lines[i])) {
                    data.push($.trim(lines[i]));
                }
            }

            var dropdownvalues = JSON.stringify(data);
            if (dropdownvalues) {
                $(question).attr("dropdownvalues", dropdownvalues);
            }

            question.find('#TemplateSingleSelectDropdownList').empty().html($('#ddlSingleSelectPreview').parent().html());
            if (!$(question).find("label").attr('data-toggle'))
                $(question).find("label").attr('data-toggle', 'tooltip');
            $(question).find("label").attr('data-original-title', questionlabel);
        }
    },
    updateSingleSelectDropdownPreview: function () {
        $("#lblDropdownFieldPreview").text($("#txtLabel").val());
        Admin_CCMQuestionDetails.showHideMandatory();
    },
    ddlElementsFromTextArea: function () {
        var ddlSingleSelectPreview = $('#ddlSingleSelectPreview');
        var defaultValues = $('#ddlSingleSelectDefaultValues');
        ddlSingleSelectPreview.empty();
        defaultValues.empty();

        var data = [];
        var lines = $('#txtareaElementsDropDown').val();
        lines = lines.split(/\n/);

        for (var i = 0; i < lines.length; i++) {
            if (/\S/.test(lines[i])) {
                data.push($.trim(lines[i]));
            }
        }

        $('<option />', { value: 0, text: ' - Select - ' }).appendTo(defaultValues);
        $('<option />', { value: 0, text: ' - Select - ' }).appendTo(ddlSingleSelectPreview);
        $.each(data, function (i, e) {
            $('<option />', { value: i + 1, text: e }).appendTo(ddlSingleSelectPreview);
            $('<option />', { value: i + 1, text: e }).appendTo(defaultValues);
        });



    },
    changeDefaultValue: function () {
        $("#divPreviewDropdownFieldPreview #ddlSingleSelectPreview option").removeAttr('selected');
        $('#divPreviewDropdownFieldPreview #ddlSingleSelectPreview').val($('#ddlSingleSelectDefaultValues option:selected').val());
        $("#divPreviewDropdownFieldPreview #ddlSingleSelectPreview option:selected").attr('selected', 'selected');
    },
    // End for handeling Single Select Dropdown Form - ZeeshanAK on 05 October, 2016


    // Start for handeling YesNo Form - ZeeshanAK on 29 September, 2016
    PopulateYesNoForm: function (QuestionID) {
        $('#chkNumber').parent().addClass('hidden');
        $('#divQuestionDetailTitle').addClass('hidden');
        $('#divQuestionDetailLabel').removeClass('hidden');

        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        if (question) {
            var name = $(question).attr("name");
            if (name != "") {
                self.find("#txtQuestionName").val(name);
            } else {
                $("#txtQuestionName").attr('placeholder', 'Check Box');
            }
            //var isnewline = $(question).attr("isnewline");
            //if (isnewline) {
            //    if (isnewline == "true")
            //        self.find("#chkNewLine").prop('checked', true)
            //}
            var ismandatory = $(question).attr("ismandatory");
            if (ismandatory) {
                if (ismandatory == "true")
                    self.find("#chkMandatory").prop('checked', true)
            }
            var questionlabel = $(question).attr("questionlabel");
            if (questionlabel) {
                self.find("#txtLabel").val(questionlabel);
            }
            var defaultvalue = $(question).attr("defaultselection");
            if (defaultvalue) {
                self.find("#chkDefaultSelectionYesNo").val(defaultvalue);
            }
        }
        $("#lblYesNoFieldPreview").text($("#txtLabel").val());
        Admin_CCMQuestionDetails.changeYesNoSelection();

    },
    UpdateYesNoInTemplateDetails: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }

        if (question) {
            var name = self.find("#txtQuestionName").val();
            if (name) {
                $(question).attr("name", name);
            }

            //var isnewline = self.find("#chkNewLine").is(':checked');
            //if (isnewline != null) {
            //    $(question).attr("isnewline", isnewline);
            //}
            var ismandatory = self.find("#chkMandatory").is(':checked');
            if (ismandatory != null) {
                $(question).attr("ismandatory", ismandatory);
            }

            var questionlabel = self.find("#txtLabel").val();
            if (questionlabel) {
                $(question).attr("questionlabel", questionlabel);
            }
            var defaultselection = self.find("#chkDefaultSelectionYesNo option:selected").val()
            if (defaultselection) {
                $(question).attr("defaultselection", defaultselection);
            }
            else {
                $(question).attr("defaultselection", "");
            }
            $(question).find('#TemplateYesNoPreview').empty().html($('#divYesNoPreview').html());
            $(question).find("#TemplateYesNoLabel").attr('data-original-title', self.find("#txtLabel").val());
            $(question).find("#TemplateYesNoLabel").text(self.find("#txtLabel").val());
            $(question).find("#lblYesNoFieldPreview").remove();
            var mendaoryMarkup = '<span class="required">*</span>';
            var lblTextField = $(question).find('#TemplateYesNoLabel');
            if (ismandatory == true) {
                lblTextField.prepend(mendaoryMarkup);
            }
        }
    },

    YesOrNo: function (obj) {
        $(obj).closest("#TemplateYesNoPreview").find('#chkYes').prop('checked', false);
        $(obj).closest("#TemplateYesNoPreview").find('#chkNo').prop('checked', false);
        if (obj.id == "chkYes") {
            $(obj).closest("#TemplateYesNoPreview").find('#chkYes').prop('checked', true);
        } else if (obj.id == "chkNo") {
            $(obj).closest("#TemplateYesNoPreview").find('#chkNo').prop('checked', true);
        }
    },
    updateYesNoPreview: function () {
        $("#lblYesNoFieldPreview").text($("#txtLabel").val());
        Admin_CCMQuestionDetails.showHideMandatory();
    },
    changeYesNoSelection: function (obj) {
        var selectedText = $('#chkDefaultSelectionYesNo option:selected').text();

        $('#divYesNoPreview input:checked').prop('checked', false);
        $('#divYesNoPreview input').removeAttr('checked');


        if (selectedText == "Yes") {
            $('#chkYes').prop('checked', true);
            $('#chkYes').attr('checked', 'checked');
        } else if (selectedText == "No") {
            $('#chkNo').prop('checked', true);
            $('#chkNo').attr('checked', 'checked');
        }
    },
    // End for handeling YesNo Form - ZeeshanAK on 29 September, 2016


    // Start for handeling Toggle Form - ZeeshanAK on 04 October, 2016
    PopulateToggleForm: function (QuestionID) {
        $('#chkNumber').parent().addClass('hidden');
        $('#divQuestionDetailTitle').addClass('hidden');
        $('#divQuestionDetailLabel').removeClass('hidden');
        $('#chkMandatory').parent().hide();

        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        if (question) {
            var name = $(question).attr("name");
            if (name != "") {
                self.find("#txtQuestionName").val(name);
            } else {
                $("#txtQuestionName").attr('placeholder', 'Toggle Button');
            }
            //var isnewline = $(question).attr("isnewline");
            //if (isnewline) {
            //    if (isnewline == "true")
            //        self.find("#chkNewLine").prop('checked', true)
            //}
            var questionlabel = $(question).attr("questionlabel");
            if (questionlabel) {
                self.find("#txtLabel").val(questionlabel);
            }
            var defaultvalue = $(question).attr("defaultselection");
            if (defaultvalue) {
                self.find("#chkDefaultSelectionToggle").val(defaultvalue);
            }
        }
        $("#lblToggleFieldPreview").text($("#txtLabel").val());
        EMRUtility.SwicthWidgetInializatoin();
        Admin_CCMQuestionDetails.changeToggleSelection();


    },
    UpdateToggleInTemplateDetails: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }

        if (question) {
            var name = self.find("#txtQuestionName").val();
            if (name) {
                $(question).attr("name", name);
            }

            var questionlabel = self.find("#txtLabel").val();
            if (questionlabel) {
                $(question).attr("questionlabel", questionlabel);
            }
            var defaultselection = self.find("#chkDefaultSelectionToggle option:selected").val()
            if (defaultselection) {
                $(question).attr("defaultselection", defaultselection);
                if (defaultselection == "1") {
                    $(question.find('#TemplateIosSwitchPreview')).removeClass('on')
                } else {
                    $(question.find('#TemplateIosSwitchPreview')).addClass('on')
                }
            }
            else {
                $(question).attr("defaultselection", "");
            }
            $(question.find('#TemplateToggleLabel')).text(questionlabel);


        }
    },
    updateTogglePreview: function () {
        $("#lblToggleFieldPreview").text($("#txtLabel").val());
    },
    changeToggleSelection: function (obj) {
        var selectedText = $('#chkDefaultSelectionToggle option:selected').text();

        if (selectedText == "OFF") {
            $('#divSwitchToggle .ios-switch').removeClass('on');
        } else {
            $('#divSwitchToggle .ios-switch').addClass('on');
        }
    },
    changeToggleSwitch: function (obj) {
        var isChecked = $(obj).is(':checked');
        if (isChecked == true) {
            $('#chkDefaultSelectionToggle').val(0);
        } else {
            $('#chkDefaultSelectionToggle').val(1);
        }
    },
    // End for handeling Toggle Form - ZeeshanAK on 04 October, 2016

    // Start for handeling CheckBox Form - ZeeshanAK on 29 September, 2016
    PopulateCheckBoxForm: function (QuestionID) {
        $('#chkNumber').parent().addClass('hidden');
        $('#divQuestionDetailCheckBox').removeClass('hidden');
        $('#chkMandatory').parent().hide();

        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        self.find("#txtTitle").val(question.find('#lblCheckBoxTitle').text().replace('*', ''));
        self.find("#lblCheckBoxTitlePreview").text(question.find('#lblCheckBoxTitle').text().replace('*', ''));
        if (question) {
            var name = $(question).attr("name");
            if (name != "") {
                self.find("#txtQuestionName").val(name);
            } else {
                $("#txtQuestionName").attr('placeholder', 'Check Box');
            }
            var selectionmode = $(question).attr("selectionmode");
            if (selectionmode) {
                self.find("#chkSelectionMode").val(selectionmode)
            }
            var questionlabel = $(question).attr("questionlabel");
            if (questionlabel) {
                $('#divCheckBoxLabels').empty();
                $.each(questionlabel.split(','), function (i, e) {
                    var labelHtml = "<div class='col-xs-3'>"
                                    + "<label class='control-label'>Label " + (i + 1) + "</label>"
                                    + "<input class='form-control' id='txtLabel_" + (i + 1) + "' name='Label' value ='" + e + "' type='text' onblur='Admin_CCMQuestionDetails.updatePreview(this);'>"
                                 + "</div>";
                    $('#divCheckBoxLabels').append(labelHtml);
                });
                Admin_CCMQuestionDetails.updateCheckBoxPreview();
            }
            var defaultselection = $(question).attr("defaultselection");
            if (defaultselection) {
                if (selectionmode == "1") {
                    EMRUtility.selectOptionsByCommaSeprateValue($('#chkDefaultSelectionChkBoxMultiple'), defaultselection);
                    $('#chkDefaultSelectionChkBoxMultiple').multiselect("refresh");
                } else {
                    self.find("#chkDefaultSelectionChkBox").val(defaultselection)
                }
            }
            var checkboxHtml = question.find('#TemplateCheckBoxPreview').html();
            if (checkboxHtml.indexOf('div') >= 0) {
                $('#divCheckBoxPreview').empty().html(checkboxHtml);
            }
            Admin_CCMQuestionDetails.changeDefaultSelection(this);
        }
        //Admin_CCMQuestionDetails.updateCheckBoxPreview();
    },
    UpdateCheckBoxInTemplateDetails: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }

        if (question) {
            var name = self.find("#txtQuestionName").val();
            if (name) {
                $(question).attr("name", name);
            }

            var labels = [];
            $('#divCheckBoxLabels div input').each(function (i, e) {
                labels.push($(e).val());
            });
            var questionlabel = labels.join();
            if (questionlabel) {
                $(question).attr("questionlabel", questionlabel);
            }
            var selectionmode = self.find("#chkSelectionMode").val();
            if (selectionmode) {
                $(question).attr("selectionmode", selectionmode);
            }

            if (selectionmode == "1") {
                var defaultselection = $('#chkDefaultSelectionChkBoxMultiple').val();
                if (defaultselection != null) {
                    defaultselection = defaultselection.join();
                    $(question).attr("defaultselection", defaultselection);
                }
            } else {
                var defaultselection = self.find("#chkDefaultSelectionChkBox").val();
                if (defaultselection) {
                    $(question).attr("defaultselection", defaultselection);
                }
            }
            question.find('#lblCheckBoxTitle').text(self.find("#txtTitle").val());
            var checkboxHtml = $('#divCheckBoxPreview').html();
            question.find('#TemplateCheckBoxPreview').empty().html(checkboxHtml);
            $(question).find("#lblCheckBoxTitle").attr('data-toggle', 'tooltip');
            $(question).find("#lblCheckBoxTitle").attr('data-original-title', self.find("#txtTitle").val());

        }
    },
    updateCheckBoxPreview: function () {
        var multiSelection;
        $('#chkSelectionMode option:selected').text() == "Single Selection" ? multiSelection = false : multiSelection = true;

        var labels = [];
        $('#divCheckBoxLabels div input').each(function (i, e) {
            labels.push($(e).val());
        });
        var ddlElementSingle = $('#chkDefaultSelectionChkBox');
        var ddlElementMultiple = $('#chkDefaultSelectionChkBoxMultiple');
        ddlElementSingle.empty().hide();
        ddlElementMultiple.empty().hide();
        ddlElementMultiple.next().remove()
        $('#divCheckBoxPreview').empty();

        if (multiSelection == false) {
            ddlElementSingle.show();
            $('<option />', { value: 0, text: '-Select-' }).appendTo(ddlElementSingle);
            $.each(labels, function (i, e) {
                $('<option />', { value: i + 1, text: e }).appendTo(ddlElementSingle);

                var checkBoxHtml = "<div class='checkbox-custom checkbox-default pull-left mr-lg'>"
                            + "<input type='checkbox' multiselect='" + multiSelection + "' onchange='Admin_CCMQuestionDetails.checkUncheckBox(this)' id='chkLabel_" + i + "' name='" + e + "'>"
                        + "<label id='txtLabel_" + i + "' class='control-label'>" + e + "</label>"
                   + " </div>";
                $('#divCheckBoxPreview').append(checkBoxHtml);
            });
        } else {
            Admin_CCMQuestionDetails.fillMultiSelectDropdown(labels, ddlElementMultiple, multiSelection);
        }

    },
    AddNewCheckBoxLabel: function (obj) {
        var labelNumber = ($('#divCheckBoxLabels div').length) + 1;
        var labelHtml = "<div class='col-xs-3'>"
                            + "<label class='control-label'>Label " + labelNumber + "</label>"
                            + "<input class='form-control' id='txtLabel_" + labelNumber + "' name='Label' value ='Label " + labelNumber + "' type='text' onblur='Admin_CCMQuestionDetails.updatePreview(this);'>"
                         + "</div>";
        $('#divCheckBoxLabels').append(labelHtml);
        Admin_CCMQuestionDetails.updateCheckBoxPreview();
    },
    checkUncheckBox: function (obj) {
        if ($(obj).is(':checked')) {
            if ($(obj).attr('multiselect') == "true") {

                var selectedValues = $('#divCheckBoxPreview input:checked').map(function () {
                    return this.id;
                }).get().join()
                $('#chkDefaultSelectionChkBoxMultiple').val('');
                EMRUtility.selectOptionsByCommaSeprateValue($('#chkDefaultSelectionChkBoxMultiple'), selectedValues);
                $('#chkDefaultSelectionChkBoxMultiple').multiselect("refresh");

            } else {
                $(obj).parent().parent().find('input').each(function (i, e) {
                    $(e).prop('checked', false);
                    $(e).removeAttr('checked');
                });
                $('#chkDefaultSelectionChkBox option').each(function (index, element) {
                    if ($(element).text() == $(obj).attr('name')) {
                        $('#chkDefaultSelectionChkBox').val(index)
                    }
                });
            }
            $(obj).prop('checked', true);
            $(obj).attr('checked', 'checked');
        } else {
            $(obj).prop('checked', false);
            $(obj).removeAttr('checked');
            if ($(obj).attr('multiselect') == "true") {

                var selectedValues = $('#divCheckBoxPreview input:checked').map(function () {
                    return this.id;
                }).get().join()
                $('#chkDefaultSelectionChkBoxMultiple').val('');
                EMRUtility.selectOptionsByCommaSeprateValue($('#chkDefaultSelectionChkBoxMultiple'), selectedValues);
                $('#chkDefaultSelectionChkBoxMultiple').multiselect("refresh");

            } else {
                $(obj).parent().parent().find('input').each(function (i, e) {
                    $(e).prop('checked', false);
                    $(e).removeAttr('checked');
                });
                $('#chkDefaultSelectionChkBox').val(0);
            }
        }
    },
    changeSelectionMode: function () {
        Admin_CCMQuestionDetails.updateCheckBoxPreview();
    },
    changeDefaultSelection: function (obj) {
        var multiSelection;
        $('#chkSelectionMode option:selected').text() == "Single Selection" ? multiSelection = false : multiSelection = true;
        //multiSelection == true ? selectedText = $('#chkDefaultSelectionChkBoxMultiple option:selected').text() : selectedText = $('#chkDefaultSelectionChkBox option:selected').text();

        $('#divCheckBoxPreview input').prop('checked', false).removeAttr('checked');
        if (multiSelection == true) {
            var selectedVals = $('#chkDefaultSelectionChkBoxMultiple option:selected').map(function () {
                return this.text
            });

            $('#divCheckBoxPreview input').each(function (i, e) {
                $.each(selectedVals, function (ii, ee) {
                    if (ee == $(e).attr('name')) {
                        $(e).prop('checked', true);
                        $(e).attr('checked', 'checked');
                    }
                });
            });

        } else {
            var selectedText = $('#chkDefaultSelectionChkBox option:selected').text()
            $('#divCheckBoxPreview input').each(function (i, e) {
                if ($(e).next().text() == selectedText) {
                    $(e).attr('defaultselect', true);
                    $(e).prop('checked', true);
                    $(e).attr('checked', 'checked');
                }
                //else {
                //    $(e).attr('defaultselect', false);
                //    $(e).prop('checked', false);
                //    $(e).removeAttr('checked');
                //}
            });
        }
    },
    fillMultiSelectDropdown: function (labels, ddlElement, multiSelection) {
        if (labels.length > 0) {
            ddlElement.show();
            $.each(labels, function (i, item) {
                if (item != '' && item != null) {
                    ddlElement.append(
                        $('<option/>', {
                            value: i,
                            html: item,
                        })
                    );
                    var checkBoxHtml = "<div class='checkbox-custom checkbox-default pull-left mr-lg'>"
                                                + "<input type='checkbox' multiselect='" + multiSelection + "' onchange='Admin_CCMQuestionDetails.checkUncheckBox(this)' id='" + i + "' name='" + item + "'>"
                                            + "<label id='txtLabel_" + i + "' class='control-label'>" + item + "</label>"
                                       + " </div>";
                    $('#divCheckBoxPreview').append(checkBoxHtml);
                }
            });


            ddlElement.multiselect('destroy');

            ddlElement.multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    ddlElement.parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },

            });
        }
    },
    // End for handeling CheckBox Form - ZeeshanAK on 29 September, 2016

    populateTextValue: function (question, self) {
        $('#divAnswerTextFieldAnswer').show();
        $('#divPreviewTextFieldPreview').show();
        $('#divAnswerNumberFieldAnswer').hide();
        $('#divPreviewNumberFieldPreview').hide();
        var maxlength = $(question).attr("maxlength");
        if (maxlength) {
            self.find("#txtTextLength").val(maxlength);
        }
        var textcase = $(question).attr("textcase");
        if (textcase) {
            self.find('#ddlTextCase option').filter(function () {
                return $.trim($(this).text().toLowerCase()) == textcase.toLowerCase();
            }).attr('selected', true);
        }

        var issingleline = $(question).attr("issingleline");
        if (issingleline) {
            if (issingleline == "true") {
                self.find("#radSingleLine").attr('checked', 'checked');
                Admin_CCMQuestionDetails.setTextFildAsMultiline(false);
            }
            else {
                Admin_CCMQuestionDetails.setTextFildAsMultiline(true);
                self.find("#radMultiLine").attr('checked', 'checked');
            }
        }
        if (textcase == "Lower") {
            self.find("#txtDefaultValue").addClass('text-lowercase');
            self.find("#txtTextFieldPreview").addClass('text-lowercase');
        }
        if (textcase == "Upper") {
            self.find("#txtDefaultValue").addClass('text-uppercase');
            self.find("#txtTextFieldPreview").addClass('text-uppercase');
        }
        if (textcase == "Sentence") {
            self.find("#txtDefaultValue").addClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
            self.find("#txtTextFieldPreview").removeClass('text-uppercase').removeClass('text-lowercase').addClass('text-capitalize');
        }
        var questiontitle = $(question).attr("questiontitle");
        if (questiontitle) {
            self.find("#txtTitle").val(questiontitle.replace('*', ''));
            self.find("#lblTextFieldPreview").text(questiontitle.replace('*', ''));
        }
        var defaultvalue = $(question).find("#txtTextField").val();
        if (defaultvalue) {
            self.find("#txtDefaultValue").val(defaultvalue);
            self.find("#txtTextFieldPreview").val(defaultvalue);
        }
    },
    UpdateTextFieldInCustonFormDetails: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }

        if (question) {
            var name = self.find("#txtQuestionName").val();
            if (name) {
                $(question).attr("name", name);
            }
            //var isnewline = self.find("#chkNewLine").is(':checked');
            //if (isnewline != null) {
            //    $(question).attr("isnewline", isnewline);
            //}
            var ismandatory = self.find("#chkMandatory").is(':checked');
            if (ismandatory != null) {
                $(question).attr("ismandatory", ismandatory);
            }
            var questiontitle = self.find("#txtTitle").val();
            if (questiontitle) {
                $(question).find('label').text(questiontitle);
                $(question).attr("questiontitle", questiontitle);
                $(question).find('label').attr('data-original-title', questiontitle);
                // $(question).find('label').attr('title', questiontitle);
            }
            var isnumber = self.find("#chkNumber").is(':checked');
            if (isnumber) {
                $(question).attr("isnumber", isnumber);

                Admin_CCMQuestionDetails.updateNumericValue(question, self);

            } else {
                Admin_CCMQuestionDetails.updateTextValue(question, self);
            }
            var isMandatory = $('#chkMandatory').is(':checked');
            var mendaoryMarkup = '<span class="required">*</span>';
            var lblTextField = $(question).find('#lblTextField');
            if (isMandatory == true) {
                lblTextField.prepend(mendaoryMarkup);
                $(question).find('#txtTextField').attr('required', 'required');
            }
            else {
                $(question).find('#txtTextField').removeAttr('required');
                lblTextField.find('required');
            }

        }
    },
    updateTextValue: function (question, self) {
        $(question).find("#txtTextField").attr("type", "text");
        var issingleline = self.find("#radSingleLine").is(':checked');
        var ismulti = self.find("#radMultiLine").is(':checked');
        if (issingleline) {
            if (issingleline == true) {
                $(question).attr("issingleline", "true");
            }
        }
        else {
            if (ismulti == true) {
                $(question).attr("issingleline", "false");
            }
        }
        var maxlength = self.find("#txtTextLength").val();
        //if (maxlength) {
        $(question).attr("maxlength", maxlength);
        $(question).find("#txtTextField").attr("maxlength", maxlength);;
        //}
        var textcase = self.find("#ddlTextCase option:selected").text();
        //if (textcase) {
        if (textcase == "Lower") {
            $(question).find("#txtTextField").addClass('text-lowercase');
            $(question).attr("textcase", "Lower");
        }
        if (textcase == "Upper") {
            $(question).find("#txtTextField").addClass('text-uppercase');
            $(question).attr("textcase", "Upper");
        }
        if (textcase == "Sentence") {
            $(question).find("#txtTextField").addClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
            $(question).attr("textcase", "Sentence");
        }
        // }
        var defaultvalue = self.find("#txtDefaultValue").val();
        //   if (defaultvalue) {
        $(question).find("#txtTextField").val(defaultvalue);
        $(question).find("#txtTextField").attr('value', defaultvalue)
        $(question).attr("defaultvalue", defaultvalue);
        // }
        $(question).attr("type", "text");
        $(question).find("#txtTextField").removeAttr("onkeypress");
        var textId = $(question).find("#txtTextField").attr('id');
        var sclases = $("#" + textId).attr("class");
        var maxLength = $("#" + textId).attr("maxlength");
        if (ismulti == true) {
            var $txtarea = $("<textarea />");
            $txtarea.attr("id", textId);
            $txtarea.attr("rows", 3);
            $txtarea.attr("cols", 60);
            $txtarea.val(defaultvalue);
            $txtarea.attr("value", defaultvalue);
            $txtarea.attr("class", sclases);
            $txtarea.attr("maxlength", maxLength);
            $txtarea.attr("spellcheck", true);
            $($txtarea).text(defaultvalue);
            $(question).find("#txtTextField").replaceWith($txtarea);
        }
        else if (ismulti == false) {
            var $input = $("<input />");
            $input.attr("id", textId);
            $input.attr("class", "form-control");
            $input.val(defaultvalue);
            $input.attr("value", defaultvalue);
            $input.attr("class", sclases);
            $input.attr("maxlength", maxLength);
            $(question).find("#txtTextField").replaceWith($input);

        }
    },
    updateNumericValue: function (question, self) {
        var textId = $(question).find("#txtTextField").attr('id');
        var sclases = $("#" + textId).attr("class");
        var $input = $("<input />");
        $input.attr("id", textId);
        $input.attr("class", "form-control");
        $input.val(defaultvalue);
        $input.attr("value", defaultvalue);
        $input.attr("class", sclases);
        $(question).find("#txtTextField").replaceWith($input);
        var minlength = self.find("#txtNumberMinLength").val();
        if (minlength) {
            $(question).attr("min", minlength);
            $(question).find("#txtTextField").attr("min", minlength);;
        }
        var maxlength = self.find("#txtNumberMaxLength").val();
        //if (maxlength) {
        $(question).attr("max", maxlength);
        $(question).find("#txtTextField").attr("max", maxlength);;
        //}
        var precision = self.find("#questionNumberPrecisiontxt").val();
        //if (precision) {
        $(question).attr("precision", precision);
        $(question).find("#txtTextField").attr("precision", precision);;
        //}
        var defaultvalue = self.find("#txtNumberQuestionDefaultValue").val();
        //if (defaultvalue) {
        $(question).find("#txtTextField").val(defaultvalue);
        $(question).find("#txtTextField").attr('value', defaultvalue)
        $(question).attr("defaultvalue", defaultvalue);
        //}
        $(question).find("#txtTextField").removeClass('text-uppercase').removeClass('text-lowercase').removeClass('text-capitalize');
        $(question).attr("type", "number");
        $(question).find("#txtTextField").attr("onkeypress", "utility.ValidateDecimalCustomForm(event, 2," + minlength + "," + maxlength + ");");
        var minlength = $("#txtNumberMinLength").val() == "" ? 0 : parseInt($("#txtNumberMinLength").val());
        var maxlength = $("#txtNumberMaxLength").val() == "" ? 9999999 : parseInt($("#txtNumberMaxLength").val());
        var Precision = $("#questionNumberPrecisiontxt").val() == "" ? 0 : parseInt($("#questionNumberPrecisiontxt").val());
        $(question).find("#txtTextField").attr("onkeypress", "utility.ValidateDecimalCustomForm(event, " + Precision + "," + minlength + "," + maxlength + ");");
    },
    setTextFieldDefaultValue: function (obj) {
        $('#txtTextFieldPreview').val($(obj).val());
    },
    setTextFieldPreview: function (obj) {
        var length = $("#txtTextLength").val();
        var textcase = $("#ddlTextCase option:selected").val();
        $('#txtDefaultValue').removeClass('text-uppercase').removeClass('text-lowercase').removeClass('text-capitalize');
        $('#txtTextFieldPreview').removeClass('text-uppercase').removeClass('text-lowercase').removeClass('text-capitalize');
        if (textcase) {
            if (textcase == "Lower") {
                $('#txtDefaultValue').addClass('text-lowercase');
                $("#txtTextFieldPreview").addClass('text-lowercase');
            }
            if (textcase == "Upper") {
                $('#txtDefaultValue').addClass('text-uppercase');
                $("#txtTextFieldPreview").addClass('text-uppercase');
            }
            if (textcase == "Sentence") {
                $('#txtDefaultValue').addClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
                $("#txtTextFieldPreview").addClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
            }
        }
        var defaultVal = $("#txtDefaultValue").val();
        $("#txtDefaultValue").attr("maxlength", length);
        $("#txtTextFieldPreview").attr("maxlength", length);
        $('#txtTextFieldPreview').val(defaultVal);
    },
    setNumericDefaultValue: function (obj) {
        obj = $("#txtNumberQuestionDefaultValue");
        var minlength = $("#txtNumberMinLength").val() == "" ? 0 : parseInt($("#txtNumberMinLength").val());
        var maxlength = $("#txtNumberMaxLength").val() == "" ? 9999999 : parseInt($("#txtNumberMaxLength").val());
        var Precision = $("#questionNumberPrecisiontxt").val() == "" ? 0 : parseInt($("#questionNumberPrecisiontxt").val());

        $('#txtNumericDefaultPreview').val($(obj).val());
        $("#txtNumericDefaultPreview").attr("onkeypress", "utility.ValidateDecimalCustomForm(event, " + Precision + "," + minlength + "," + maxlength + ");");
        $(obj).attr("onkeypress", "utility.ValidateDecimalCustomForm(event, " + Precision + "," + minlength + "," + maxlength + ");");
    },
    showHideNumericAnswer: function (obj) {
        if ($(obj).is(':checked')) {
            $('#divAnswerNumberFieldAnswer').show();
            $('#divPreviewNumberFieldPreview').show();
            $('#divAnswerTextFieldAnswer').hide();
            $('#divPreviewTextFieldPreview').hide();
        }
        else {
            $('#divAnswerTextFieldAnswer').show();
            $('#divPreviewTextFieldPreview').show();
            $('#divAnswerNumberFieldAnswer').hide();
            $('#divPreviewNumberFieldPreview').hide();
        }
    },
    updatePreview: function () {
        var questionType = Admin_CCMQuestionDetails.params.QuestionType
        if (questionType === 'YesNo') {
            Admin_CCMQuestionDetails.updateYesNoPreview();
        } else if (questionType === 'CheckBox') {
            Admin_CCMQuestionDetails.updateCheckBoxPreview();
        } else if (questionType === 'Toggle') {
            Admin_CCMQuestionDetails.updateTogglePreview();
        } else if (questionType === 'SingleSelectDropdown') {
            Admin_CCMQuestionDetails.updateSingleSelectDropdownPreview();
        } else if (questionType === 'MultipleSelectCombo') {
            Admin_CCMQuestionDetails.updateMultiSelectComboPreview();
        }
    },
    populateFractionFieldForm: function (QuestionID, fromGroup) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        var name = $(question).attr("name");
        if (name) {
            self.find("#txtQuestionName").val(name);
        }
        var ismandatory = $(question).attr("ismandatory");
        if (ismandatory) {
            if (ismandatory == "true")
                self.find("#chkMandatory").attr('checked', 'checked');
        }
        var fractionTitle = $(question).find("#lblFractionTitle").text();
        if (fractionTitle) {
            self.find("#txtTitle").val(fractionTitle.replace('*', ''));
            $("#lblFractionTitlePreview").text(fractionTitle.replace('*', ''));
        }
        var fractionFieldlbl1 = $(question).find("#lblFractionField1").text();
        if (fractionFieldlbl1) {
            $("#lblFractionFieldPreview1").text(fractionFieldlbl1.replace('*', ''));
            self.find("#txtFarctionFieldLabelValue1").val(fractionFieldlbl1.replace('*', ''));
        }
        var fractionFieldlbl2 = $(question).find("#lblFractionField2").text();
        if (fractionFieldlbl2) {
            $("#lblFractionFieldPreview2").text(fractionFieldlbl2.replace('*', ''));
            self.find("#txtFarctionFieldLabelValue2").val(fractionFieldlbl2.replace('*', ''));
        }

        var fractionField1 = $(question).find("#txtFractionField1").val();
        if (fractionField1) {
            $("#txtFractionFieldPreview1").val(fractionField1);
            $("#txtFarctionFieldDefaultValue1").val(fractionField1);
        }
        var fractionField2 = $(question).find("#txtFractionField2").val();
        if (fractionField2) {
            $("#txtFractionFieldPreview2").val(fractionField2);
            $("#txtFarctionFieldDefaultValue2").val(fractionField2);
        }
    },
    getFractionLabel1ValueText: function () {

        $('#lblFractionFieldPreview1').text($('#txtFarctionFieldLabelValue1').val());
    },
    getFractionLabel2ValueText: function () {

        $('#lblFractionFieldPreview2').text($('#txtFarctionFieldLabelValue2').val());
    },
    updateFractionFieldForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }

        var name = self.find("#txtQuestionName").val();
        // if (name) {
        $(question).attr("name", name);
        //}
        //var isnewline = self.find("#chkNewLine").is(':checked');
        //// if (isnewline) {
        //$(question).attr("isnewline", isnewline);
        //  }

        var fractionTitle = self.find("#txtTitle").val();
        $(question).find("#lblFractionTitle").text(fractionTitle.replace('*', ''));
        $(question).find("#lblFractionTitle").attr('data-original-title', fractionTitle.replace('*', ''));
        // $(question).find("#lblFractionTitle").attr('title', fractionTitle.replace('*', ''));
        var ismandatory = self.find("#chkMandatory").is(':checked');
        //if (ismandatory) {
        $(question).attr("ismandatory", ismandatory);
        // }
        var fractionFieldlbl1 = self.find("#txtFarctionFieldLabelValue1").val();
        //  if (fractionFieldlbl1) {
        $(question).find("#lblFractionField1").text(fractionFieldlbl1);
        // }
        var fractionFieldlbl2 = self.find("#txtFarctionFieldLabelValue2").val();
        //if (fractionFieldlbl2) {
        $(question).find("#lblFractionField2").text(fractionFieldlbl2);
        // }
        var fractionField1 = self.find("#txtFarctionFieldDefaultValue1").val();
        // if (fractionField1) {
        $(question).find("#txtFractionField1").val(fractionField1);
        $(question).find("#txtFractionField1").attr('value', fractionField1);
        // }
        var fractionField2 = self.find("#txtFarctionFieldDefaultValue2").val();
        // if (fractionField2) {
        $(question).find("#txtFractionField2").val(fractionField2);
        $(question).find("#txtFractionField2").attr('value', fractionField2);
        // }
        var isMandatory = $('#chkMandatory').is(':checked');
        var mendaoryMarkup = '<span class="required">*</span>';
        $(question).find('#lblFractionField1');
        if (isMandatory == true) {
            // $($(question).find('#lblFractionField1')).prepend(mendaoryMarkup);
            //$($(question).find('#lblFractionField2')).prepend(mendaoryMarkup);
            $(question).find("#lblFractionTitle").prepend(mendaoryMarkup);
            $('#txtFractionField1').attr('required', 'required');
            $('#txtFractionField2').attr('required', 'required');
        }
        else {
            // $($(question).find('#lblFractionField1')).find('required');
            //$($(question).find('#lblFractionField2')).find('required');
            $(question).find("#lblFractionTitle");
            $('#txtFractionField1').removeAttr('required');
            $('#txtFractionField2').removeAttr('required');
        }
    },
    populateDateFieldForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        var name = $(question).attr("name");
        if (name) {
            self.find("#txtQuestionName").val(name);
        }
        //var isnewline = $(question).attr("isnewline");
        //if (isnewline) {
        //    if (isnewline == "true")
        //        self.find("#chkNewLine").attr('checked', 'checked');
        //}
        var ismandatory = $(question).attr("ismandatory");
        if (ismandatory) {
            if (ismandatory == "true")
                self.find("#chkMandatory").attr('checked', 'checked');
        }
        var dateFieldTitle = $(question).find("#lblDateFieldTitle").text();
        if (dateFieldTitle) {
            $("#lblDateFieldPreview").text(dateFieldTitle.replace('*', ''));
            self.find("#txtTitle").val(dateFieldTitle.replace('*', ''));
        }
        var defaultDateFieldvalue = $(question).find("#dtpDateField").val();
        if (defaultDateFieldvalue) {
            $("#dtpDefaultDate").val(defaultDateFieldvalue);
            $("#dtpDateFieldPreview").val(defaultDateFieldvalue);
        }
        var formate = $(question).find("#dtpDateField").attr("dateformat");
        if (formate) {
            self.find('#ddlDateFormat option').filter(function () {
                return $.trim($(this).text().toLowerCase()) == formate.toLowerCase();
            }).attr('selected', true);
        }
    },
    updateDateFieldForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }
        var name = self.find("#txtQuestionName").val();
        //if (name) {
        $(question).attr("name", name);
        //}
        //var isnewline = self.find("#chkNewLine").is(':checked');
        //// if (isnewline) {
        //$(question).attr("isnewline", isnewline);
        // }
        var ismandatory = self.find("#chkMandatory").is(':checked');
        // if (ismandatory) {
        $(question).attr("ismandatory", ismandatory);
        //}
        var title = self.find("#txtTitle").val();
        // if (title) {
        $(question).find("#lblDateFieldTitle").text(title);
        $(question).find("#lblDateFieldTitle").attr('data-original-title', title);
        // $(question).find("#lblDateFieldTitle").attr('title', title);
        // }
        var formate = self.find("#ddlDateFormat option:selected").text();
        //if (formate) {
        $(question).find("#dtpDateField").attr("dateformat", formate);
        $(question).attr("dateformat", formate);
        // }
        var defaultDate = $("#dtpDefaultDate").val();
        $(question).find("#dtpDateField").attr('value', defaultDate);
        // if (defaultDate) {
        $(question).find("#dtpDateField").val(defaultDate);
        $(question).attr("defaultdate", defaultDate);
        //}
        var isMandatory = $('#chkMandatory').is(':checked');
        var mendaoryMarkup = '<span class="required">*</span>';
        $(question).find('#lblFractionField1');
        if (isMandatory == true) {
            $($(question).find('#lblDateFieldTitle')).prepend(mendaoryMarkup);
            $('#dtpDateField').attr('required', 'required');
        }
        else {
            $($(question).find('#lblDateFieldTitle')).find('required');
            $('#dtpDateField').removeAttr('required');
        }
        Admin_CCMTemplatePreview.initilizeDatePickers();
    },
    updateDateFieldPreview: function () {
        $("#dtpDateFieldPreview").val($("#dtpDefaultDate").val());
    },
    populateHeaderForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        var name = $(question).attr("name");
        if (name) {
            self.find("#txtQuestionName").val(name);
        }
        //var isnewline = $(question).attr("isnewline");
        //if (isnewline) {
        //    if (isnewline == "true")
        //        self.find("#chkNewLine").attr('checked', 'checked');
        //}
        var ismandatory = $(question).attr("ismandatory");
        if (ismandatory) {
            if (ismandatory == "true")
                self.find("#chkMandatory").attr('checked', 'checked');
        }
        var headerText = $(question).find("#lblHeader").text();
        if (headerText) {
            $("#lblHeaderPreview").text(headerText);
            self.find("#txtHeaderText").val(headerText);
        }
        var textcase = $(question).attr("textcase");
        if (textcase) {
            self.find('#ddlHeadeFontCase option').filter(function () {
                return $.trim($(this).text().toLowerCase()) == textcase.toLowerCase();
            }).attr('selected', true);
        }
        var fontSize = $(question).attr("fontSize");
        if (fontSize) {
            $("#txtHeaderFontSize").val(fontSize);
        }
        Admin_CCMQuestionDetails.setHeaderPreview();
    },
    setHeaderPreview: function () {
        $("#lblHeaderPreview").text($("#txtHeaderText").val());
        $("#lblHeaderPreview").removeClass('text-uppercase').removeClass('text-lowercase').removeClass('text-capitalize');
        var fontSize = $("#txtHeaderFontSize").val();
        var textcase = $("#ddlHeadeFontCase option:selected").text();
        if (fontSize && fontSize != "") {
            $("#lblHeaderPreview").css("font-size", fontSize + "px");
        }
        if (textcase == "Lower")
            $("#lblHeaderPreview").addClass('text-lowercase');
        if (textcase == "Upper")
            $("#lblHeaderPreview").addClass('text-uppercase');
        if (textcase == "Sentence")
            $("#lblHeaderPreview").addClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
    },
    updateHeaderForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }
        var name = self.find("#txtQuestionName").val();
        //if (name) {
        $(question).attr("name", name);
        //}
        //var isnewline = self.find("#chkNewLine").is(':checked');
        ////  if (isnewline) {
        //$(question).attr("isnewline", isnewline);
        // }
        var ismandatory = self.find("#chkMandatory").is(':checked');
        // if (ismandatory) {
        $(question).attr("ismandatory", ismandatory);
        //  }
        var title = self.find("#txtHeaderText").val();
        // if (title) {
        $(question).find("#lblHeader").text(title);
        // }
        $(question).find("#lblHeader").removeClass('text-uppercase').removeClass('text-lowercase').removeClass('text-capitalize');
        var fontSize = $("#txtHeaderFontSize").val();
        var textcase = $("#ddlHeadeFontCase option:selected").text();
        //  if (fontSize && fontSize != "") {
        $(question).find("#lblHeader").css("font-size", fontSize + "px");
        $(question).attr("fontSize", fontSize);
        // }
        $(question).attr("textcase", textcase);
        if (textcase == "Lower") {
            $(question).find("#lblHeader").addClass('text-lowercase');
        }
        if (textcase == "Upper") {
            $(question).find("#lblHeader").addClass('text-uppercase');
        }
        if (textcase == "Sentence") {
            $(question).find("#lblHeader").addClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
        }
    },
    populateTimeFieldForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        var name = $(question).attr("name");
        if (name) {
            self.find("#txtQuestionName").val(name);
        }
        //var isnewline = $(question).attr("isnewline");
        //if (isnewline) {
        //    if (isnewline == "true")
        //        self.find("#chkNewLine").attr('checked', 'checked');
        //}
        var ismandatory = $(question).attr("ismandatory");
        if (ismandatory) {
            if (ismandatory == "true")
                self.find("#chkMandatory").attr('checked', 'checked');
        }
        var timeformat = $(question).find("#dtpTimeField").attr("timeformat");
        if (timeformat) {
            if (timeformat == 12) {
                self.find("rad12Hours");
                Admin_CCMQuestionDetails.changeTimeFormat(true);
            }
            else if (timeformat == 24) {
                self.find("rad24Hours");
                Admin_CCMQuestionDetails.changeTimeFormat(false);
            }
            //self.find('#ddlDateFormat option').filter(function () {
            //    return $.trim($(this).text().toLowerCase()) == timeformat.toLowerCase();
            //}).attr('selected', true);
        }
        var title = $(question).find("#lblTimeFieldTitle");
        var timeFieldTitle = $(question).find("#lblTimeFieldTitle").text();
        if (timeFieldTitle) {
            timeFieldTitle
            $("#lblTimeFieldPreview").text(timeFieldTitle.replace('*', ''));
            self.find("#txtTitle").val(timeFieldTitle.replace('*', ''));
        }
        var defaultTimeFieldvalue = $(question).find("#dtpTimeField").val();
        if (defaultTimeFieldvalue) {
            $("#tpDefaultTime").val(defaultTimeFieldvalue);
            $("#tpQuestion").val(defaultTimeFieldvalue);
        }
    },
    updateTimeFieldForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }
        var name = self.find("#txtQuestionName").val();
        //   if (name) {
        $(question).attr("name", name);
        //  }
        //var isnewline = self.find("#chkNewLine").is(':checked');
        //// if (isnewline) {
        //$(question).attr("isnewline", isnewline);
        //}
        var ismandatory = self.find("#chkMandatory").is(':checked');
        //if (ismandatory) {
        $(question).attr("ismandatory", ismandatory);
        // }
        var title = self.find("#txtTitle").val();
        //  if (title) {
        $(question).find("#lblTimeFieldTitle").text(title);
        $(question).find("#lblTimeFieldTitle").attr('data-original-title', title);
        //$(question).find("#lblTimeFieldTitle").attr('title', title);
        // }

        var formate = "24";
        if (self.find("#rad12Hours").is(':checked'))
            formate = "12";
        if (formate) {
            $(question).find("#dtpTimeField").attr("timeformat", formate);
            $(question).attr("Timeformat", formate);
        }
        var defaultTime = $("#tpDefaultTime").val();
        //if (defaultTime) {
        $(question).find("#dtpTimeField").val(defaultTime);
        $(question).find("#dtpTimeField").attr('value', defaultTime);
        $(question).find("#dtpTimeField").attr("timeformat", formate);
        $(question).attr("defaultTime", defaultTime);
        // }
        var isMandatory = $('#chkMandatory').is(':checked');
        var mendaoryMarkup = '<span class="required">*</span>';
        $(question).find('#lblFractionField1');
        if (isMandatory == true) {
            $($(question).find('#lblTimeFieldTitle')).prepend(mendaoryMarkup);
            $('#dtpTimeField').attr('required', 'required');
        }
        else {
            $($(question).find('#lblTimeFieldTitle')).find('required');
            $('#dtpTimeField').removeAttr('required');
        }
        Admin_CCMTemplatePreview.initilizeTimePickers();
    },
    changeTimeFormat: function (showMeridian) {
        $('#tpDefaultTime').timepicker('remove');
        $('#tpQuestion').timepicker('remove');
        $('#tpQuestion').timepicker({
            showMeridian: showMeridian,
            //defaultTime: false
        });
        $('#tpDefaultTime').timepicker({
            showMeridian: showMeridian,
        }).on('changeTime.timepicker', function (e) {
            //$(this).timepicker('hide');
            $("#tpQuestion").val($("#tpDefaultTime").val());
        });
    },
    updateTimeFieldPreview: function () {
        $('#tpQuestion').val()
        $("#tpQuestion").val($("#tpDefaultTime").val());
    },
    updateDateFormat: function () {
        var date_format = $("#ddlDateFormat option:selected").val();
        if (date_format) {
            date_format = date_format.toLowerCase();
            $("#dtpDateFieldPreview").val('');
            $("#dtpDateFieldPreview").datepicker('remove')
            $("#dtpDateFieldPreview").datepicker({ format: date_format });
            $("#dtpDefaultDate").val('');
            $("#dtpDefaultDate").datepicker('remove')
            $("#dtpDefaultDate").datepicker({ format: date_format });
            $('#' + Admin_CCMQuestionDetails.params.PanelID + ' #dtpDefaultDate').datepicker({
                format: date_format
            }).on('changeDate.timepicker', function (e) {
                $(this).datepicker('hide');
                $("#dtpDateFieldPreview").val($("#dtpDefaultDate").val());
            });
        }
    },
    showHideMandatory: function () {
        var isMandatory = $('#chkMandatory').is(':checked');
        var isnueric = $('#chkNumber').is(':checked');
        var mendaoryMarkup = '<span class="required">*</span>';
        switch (Admin_CCMQuestionDetails.params.QuestionType) {
            case "TextField":
                $('#lblNumberFieldPreview').find('.required').remove();
                $('#lblTextFieldPreview').find('.required').remove();
                if (isMandatory == true) {
                    if (isnueric == true) {
                        $('#lblNumberFieldPreview').prepend(mendaoryMarkup);
                    }
                    else {
                        $('#lblTextFieldPreview').prepend(mendaoryMarkup);
                        $('#txtTextFieldPreview').attr('required', 'required');
                    }

                }
                else {
                    if (isnueric == true) {
                        $('#lblNumberFieldPreview').find('.required').remove();

                    }
                    else {
                        $('#lblTextFieldPreview').find('.required').remove();
                        $('#txtTextFieldPreview').attr('required', 'required');
                    }
                }
                break;
            case "TextField":
                $('#lblTextFieldPreview').find('.required').remove();
                if (isMandatory == true) {
                    $('#lblTextFieldPreview').prepend(mendaoryMarkup);
                }
                else {
                    $('#lblTextFieldPreview').find('.required').remove();
                }
                break;
            case "YesNo":
                $('#lblYesNoFieldPreview').find('.required').remove();
                if (isMandatory == true) {
                    $('#lblYesNoFieldPreview').prepend(mendaoryMarkup);
                    //$('#chkYes').attr('required', 'required');
                    //$('#chkNo').attr('required', 'required');
                }
                else {
                    $('#lblYesNoFieldPreview').find('.required').remove();
                    //$('#chkYes').removeAttr('required');
                    //$('#chkNo').removeAttr('required');
                }
                break;
            case "FractionField":
                lblFractionTitlePreview
                $('#lblFractionTitlePreview').find('.required').remove();
                if (isMandatory == true) {
                    $('#lblFractionTitlePreview').prepend(mendaoryMarkup);
                }
                else {
                    $('#lblFractionTitlePreview').find('.required').remove();
                }
                //$('#lblFractionFieldPreview1').find('.required').remove();
                //$('#lblFractionFieldPreview2').find('.required').remove();
                //if (isMandatory == true) {
                //    $('#lblFractionFieldPreview1').prepend(mendaoryMarkup);
                //    $('#lblFractionFieldPreview2').prepend(mendaoryMarkup);
                //}
                //else {
                //    $('#lblFractionFieldPreview1').find('.required').remove();
                //    $('#lblFractionFieldPreview2').find('.required').remove();
                //}
                break;
            case "DateField":
                $('#lblDateFieldPreview').find('.required').remove();
                if (isMandatory == true) {
                    $('#lblDateFieldPreview').prepend(mendaoryMarkup);
                }
                else {
                    $('#lblDateFieldPreview').find('.required').remove();
                }
                break;
            case "TimeField":
                $('#lblTimeFieldPreview').find('.required').remove();
                if (isMandatory == true) {
                    $('#lblTimeFieldPreview').prepend(mendaoryMarkup);
                }
                else {
                    $('#lblTimeFieldPreview').find('.required').remove();
                }
                break;
            case "FreeText":
                $('#lblFreeFieldPreview').find('.required').remove();
                if (isMandatory == true) {
                    $('#lblFreeFieldPreview').prepend(mendaoryMarkup);
                }
                else {
                    $('#lblFreeFieldPreview').find('.required').remove();
                }
                break;
            case "SingleSelectDropdown":
                $('#lblDropdownFieldPreview').find('.required').remove();
                if (isMandatory == true) {
                    $('#lblDropdownFieldPreview').prepend(mendaoryMarkup);
                    $('#ddlSingleSelectPreview').attr('required', 'required');
                    $('#ddlSingleSelectPreview option:eq(0)').val('');
                }
                else {
                    $('#lblDropdownFieldPreview').find('.required').remove();
                    $('#ddlSingleSelectPreview').removeAttr('required');
                    $('#ddlSingleSelectPreview option:eq(0)').val('0');
                }
                break;
            case "MultipleSelectCombo":
                $('#lblMultiSelectDropdownFieldPreview').find('.required').remove();
                if (isMandatory == true) {
                    $('#lblMultiSelectDropdownFieldPreview').prepend(mendaoryMarkup);
                }
                else {
                    $('#lblMultiSelectDropdownFieldPreview').find('.required').remove();
                }
                break;
            case "Table":
                $('#lblTableTitlePreview').find('.required').remove();
                if (isMandatory == true) {
                    $('#lblTableTitlePreview').prepend(mendaoryMarkup);
                }
                else {
                    $('#lblTableTitlePreview').find('.required').remove();
                }
                break;
            default:
                break;
        }
    },
    setFreeFieldDefaultValue: function () {
        var obj = $("#txtFreeTextDefaultValue");
        var length = $("#txtFreeTextLength").val();
        var textcase = $("#ddlFreeTextCase option:selected").text();
        $('#txtFreeFieldPreview').removeClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
        $('#txtFreeTextDefaultValue').removeClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
        if (textcase) {
            if (textcase == "Lower") {
                $('#txtFreeFieldPreview').addClass('text-lowercase');
                $("#txtFreeTextDefaultValue").addClass('text-lowercase');
            }
            if (textcase == "Upper") {
                $('#txtFreeFieldPreview').addClass('text-uppercase');
                $("#txtFreeTextDefaultValue").addClass('text-uppercase');
            }
            if (textcase == "Sentence") {
                $('#txtFreeFieldPreview').addClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
                $('#txtFreeTextDefaultValue').addClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
            }
        }
        var defaultVal = $("#txtFreeTextDefaultValue").val();
        $('#txtFreeFieldPreview').val($(obj).val());
        $('#txtFreeFieldPreview').attr("maxlength", length);
        $('#txtFreeTextDefaultValue').attr("maxlength", length);

    },
    populateFreeTextForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        var name = $(question).attr("name");
        if (name) {
            self.find("#txtQuestionName").val(name);
        }
        var title = $(question).find("#lblFreeText").text();
        self.find("#txtTitle").val(title.replace('*', ''));
        self.find("#lblFreeFieldPreview").text(title.replace('*', ''));
        //var isnewline = $(question).attr("isnewline");
        //if (isnewline) {
        //    if (isnewline == "true")
        //        self.find("#chkNewLine").attr('checked', 'checked');
        //}
        var ismandatory = $(question).attr("ismandatory");
        if (ismandatory) {
            if (ismandatory == "true")
                self.find("#chkMandatory").attr('checked', 'checked');
        }
        var freeText = $(question).find("#txtFreeText").text();
        if (freeText) {
            self.find("#txtFreeFieldPreview").val(freeText);
            self.find("#txtFreeTextDefaultValue").val(freeText);
        }
        var textcase = $(question).attr("textcase");
        if (textcase) {
            self.find('#ddlFreeTextCase option').filter(function () {
                return $.trim($(this).text().toLowerCase()) == textcase.toLowerCase();
            }).attr('selected', true);
        }
        var maxlength = $(question).attr("maxlength");
        if (maxlength) {
            $("#txtFreeTextLength").val(maxlength);
        }
        Admin_CCMQuestionDetails.setFreeFieldDefaultValue();
    },
    updateFreeTextForm: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }
        var name = self.find("#txtQuestionName").val();
        // if (name) {
        $(question).attr("name", name);
        // }
        //var isnewline = self.find("#chkNewLine").is(':checked');
        //// if (isnewline) {
        //$(question).attr("isnewline", isnewline);
        // }
        var title = self.find("#txtTitle").val();
        //  if (title) {
        $(question).find("#lblFreeText").text(title);
        $(question).find("#lblFreeText").attr('data-original-title', title);
        //$(question).find("#lblFreeText").attr('title', title);
        var ismandatory = self.find("#chkMandatory").is(':checked');
        //   if (ismandatory) {
        $(question).attr("ismandatory", ismandatory);
        //  }
        var defaultVal = self.find("#txtFreeTextDefaultValue").val();
        //  if (defaultVal) {
        $(question).find("#txtFreeText").text(defaultVal);
        $(question).find("#txtFreeText").val(defaultVal);
        $(question).find("#txtFreeText").attr('value', defaultVal);
        //   }
        $(question).find("#txtFreeText").removeClass('text-uppercase').removeClass('text-lowercase').removeClass('text-capitalize');
        var length = $("#txtFreeTextLength").val();
        var textcase = $("#ddlFreeTextCase option:selected").text();
        //  if (length && length != "") {
        $(question).find("#txtFreeText").css("maxlength", length);
        $(question).attr("maxlength", length);
        // }
        $(question).attr("textcase", textcase);
        if (textcase == "Lower") {
            $(question).find("#txtFreeText").addClass('text-lowercase');
        }
        if (textcase == "Upper") {
            $(question).find("#txtFreeText").addClass('text-uppercase');
        }
        if (textcase == "Sentence") {
            $(question).find("#txtFreeText").addClass('text-capitalize').removeClass('text-uppercase').removeClass('text-lowercase');
        }
        var mendaoryMarkup = '<span class="required">*</span>';
        if (ismandatory == true) {
            $($(question).find('#lblFreeText')).prepend(mendaoryMarkup);
            $('#txtFreeText').attr('required', 'required');
        }
        else {
            $($(question).find('#lblFreeText')).find('required');
            $('#txtFreeText').removeAttr('required');
        }
    },
    QuestionExists: function () {
        var QuestionID = Admin_CCMQuestionDetails.params.QuestionID
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var questionExists = false;
        detailsForm.find('.toolcontroldiv').each(function () {
            var QuestionName = $("#txtQuestionName").val();
            var name = $(this).attr("name");
            var id = $(this).attr("id");
            if (name && QuestionName != "") {
                if ((id && QuestionID) && (id != QuestionID) && (name.toLowerCase() == QuestionName.toLowerCase())) {
                    questionExists = true;
                }
            }
        });
        return questionExists;
    },
    drawTable: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            $('#ulQuestions li input').each(function (i, elem) {
                if ($($(elem).val()).attr('id') == QuestionID) {
                    question = $($(elem).val());
                    self.find('#divSelectedQuestion').html($(elem).val());
                }
            });
        }
        var name = $(question).attr("name");
        if (name) {
            self.find("#txtQuestionName").val(name);
        }
        //var isnewline = $(question).attr("isnewline");
        //if (isnewline) {
        //    if (isnewline == "true")
        //        self.find("#chkNewLine").attr('checked', 'checked');
        //}
        var ismandatory = $(question).attr("ismandatory");
        if (ismandatory) {
            if (ismandatory == "true")
                self.find("#chkMandatory").attr('checked', 'checked');
        }
        var title = $(question).find("#lblTabelTitle").text();
        self.find("#txtTitle").val(title.replace('*', ''));
        self.find("#lblTableTitlePreview").text(title.replace('*', ''));
        var tblPreview = $("#tblPreview");
        if (Admin_CCMQuestionDetails.params["TableRows"] && Admin_CCMQuestionDetails.params["TableCols"]) {
            var rows = Admin_CCMQuestionDetails.params["TableRows"];
            var cols = Admin_CCMQuestionDetails.params["TableCols"];
            for (r = 0; r < rows; r++) {
                var $row = $('<tr/>');
                {
                    for (c = 0; c < cols; c++) {
                        $row.append('<td contenteditable="true" style=""></td>');
                    }
                }
                tblPreview.append($row);
            }
            // tblPreview.css({ "border": "1px dashed #BBB; ", "width": "100%" });
        }
        var table = $(question).find('#tblContext');
        var width = $(table).attr("width");
        self.find("#txtTableWidth").val(width);
        var height = $(table).attr("height");
        self.find("#txtTableHeight").val(height);
        var cellSpace = $(table).attr("cellspacing");
        self.find("#txtTabelCellSpace").val(cellSpace);
        var cellPadding = $(table).attr("cellpadding");
        self.find("#txtTableCellPadding").val(cellPadding);
        var border = $(table).attr("border");
        self.find("#txtTableBorder").val(border);
        var align = $(table).attr("width");
        var align = $(table).attr("align");
        if (align) {
            self.find('#ddlAlignment option').filter(function () {
                return $.trim($(this).text().toLowerCase()) == align.toLowerCase();
            }).attr('selected', true);
        }
        var showCaption = $(question).attr("showCaption");
        if (showCaption) {
            if (showCaption == "true")
                self.find("#chkCaption").attr('checked', 'checked');
        }
        //$(tblPreview).attr("cellspacing", cellSpace);
        //$(tblPreview).attr("cellpadding", cellPadding);
        //$(tblPreview).attr("width", width);
        //$(tblPreview).attr("height", height);
        //$(tblPreview).attr("cellpadding", cellPadding);
        //$(tblPreview).attr("border", border);
        //$(tblPreview).attr("align", align);
        //$(table).css({ 'border-spacing': cellSpace + 'px', 'border-collapse': 'separate' });
        //$(table).find('td').css({ 'padding': cellPadding + 'px' });
        Admin_CCMQuestionDetails.showHideMandatory();
        Admin_CCMQuestionDetails.setTableDefaultPreview();
    },
    updateTable: function (QuestionID) {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var detailsForm = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var question = detailsForm.find("div[id='" + QuestionID + "']");
        if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
            question = self.find('#divSelectedQuestion').find('div.toolcontroldiv');
        }
        var name = self.find("#txtQuestionName").val();
        $(question).attr("name", name);
        //var isnewline = self.find("#chkNewLine").is(':checked');
        //$(question).attr("isnewline", isnewline);
        var ismandatory = self.find("#chkMandatory").is(':checked');
        $(question).attr("ismandatory", ismandatory);
        var title = self.find("#txtTitle").val();
        $(question).find("#lblTabelTitle").text(title);
        // $(question).find("#lblTabelTitle").attr('title', title);
        $(question).find("#lblTabelTitle").attr('data-original-title', title);
        var table = $(question).find('#tblContext');
        var width = self.find("#txtTableWidth").val();
        var height = self.find("#txtTableHeight").val();
        var cellSpace = self.find("#txtTabelCellSpace").val();
        var cellPadding = self.find("#txtTableCellPadding").val();
        var border = self.find("#txtTableBorder").val();
        var height = self.find("#txtTableHeight").val();
        var align = $("#ddlAlignment option:selected").val();
        //$(table).css({ "width": width + 'px', "height": height + 'px', "width": width + 'px', "height": height + 'px', });
        $(table).attr("cellspacing", cellSpace);
        $(table).attr("cellpadding", cellPadding);
        $(table).attr("width", width);
        $(table).attr("height", height);
        $(table).attr("cellpadding", cellPadding);
        $(table).attr("border", border);
        $(table).attr("align", align);
        var showCaption = self.find("#chkCaption").is(':checked');
        $(question).attr("showCaption", showCaption);
        if (showCaption == true) {
            if ($(question).find("#lblTabelTitle").hasClass("hidden"))
                $(question).find("#lblTabelTitle").removeClass("hidden");
        }
        else {
            if (!$(question).find("#lblTabelTitle").hasClass("hidden"))
                $(question).find("#lblTabelTitle").addClass("hidden");
        }
        $(table).removeAttr('style');
        $(table).find('tr').find('td').each(function () {
            //$(this).attr("cellspacing", cellSpace);
            //$(this).attr("cellpadding", cellPadding);
            //$(this).attr("width", width);
            //$(this).attr("height", height);
            //$(this).attr("cellpadding", cellPadding);
            //$(this).attr("border", border);
            //$(this).attr("align", align);
            $(this).removeAttr('style');
        });
        $(table).css({ 'border-spacing': cellSpace + 'px', 'border-collapse': 'separate' });
        $(table).find('td').css({ 'padding': cellPadding + 'px' });
        var mendaoryMarkup = '<span class="required">*</span>';
        if (ismandatory == true) {
            $($(question).find('#lblTabelTitle')).prepend(mendaoryMarkup);
        }
        else {
            $($(question).find('#lblTabelTitle')).find('required').remove();
        }
    },
    setTableDefaultPreview: function () {
        var self = $('#' + Admin_CCMQuestionDetails.params["PanelID"] + " #frmAdminCCMQuestionDetails");
        var table = $(self).find('#tblPreview');
        var width = self.find("#txtTableWidth").val();
        var height = self.find("#txtTableHeight").val();
        var cellSpace = self.find("#txtTabelCellSpace").val();
        var cellPadding = self.find("#txtTableCellPadding").val();
        var border = self.find("#txtTableBorder").val();
        var height = self.find("#txtTableHeight").val();
        var align = $("#ddlAlignment option:selected").val();
        // $(table).css({ "width": width + 'px', "height": height + 'px', "width": width + 'px', "height": height + 'px', });
        $(table).attr("cellspacing", cellSpace);
        $(table).attr("cellpadding", cellPadding);
        $(table).attr("width", width);
        $(table).attr("height", height);
        $(table).attr("cellpadding", cellPadding);
        $(table).attr("border", border);
        $(table).attr("align", align);
        var showCaption = self.find("#chkCaption").is(':checked');
        if (showCaption == true) {
            if ($(self).find("#lblTableTitlePreview").hasClass("hidden"))
                $(self).find("#lblTableTitlePreview").removeClass("hidden");
        }
        else {
            if (!$(self).find("#lblTableTitlePreview").hasClass("hidden"))
                $(self).find("#lblTableTitlePreview").addClass("hidden");
        }
        $(table).css({ 'border-spacing': cellSpace + 'px', 'border-collapse': 'separate' });
        $(table).find('td').css({ 'padding': cellPadding + 'px' });
    },
    setTextFildAsMultiline: function (isMultiLine) {
        var textValue = $("#txtDefaultValue").val();
        var sclases = $("#txtDefaultValue").attr("class");
        var maxLength = $("#txtDefaultValue").attr("maxlength");
        if (isMultiLine == true) {
            var $txtarea = $("<textarea />");
            $txtarea.attr("id", "txtDefaultValue");
            $txtarea.attr("rows", 3);
            $txtarea.attr("cols", 60);
            $txtarea.val(this.value);
            $txtarea.attr("class", sclases);
            $txtarea.attr("maxlength", maxLength);
            $txtarea.attr("spellcheck", true);
            $txtarea.attr("onblur", "Admin_CCMQuestionDetails.setTextFieldPreview(this);");
            $("#txtDefaultValue").replaceWith($txtarea);
            var $txtareaPre = $("<textarea />");
            $txtareaPre.attr("id", "txtTextFieldPreview");
            $txtareaPre.attr("rows", 3);
            $txtareaPre.attr("cols", 60);
            $txtareaPre.attr("spellcheck", true);
            $txtareaPre.val(this.value);
            $("#txtTextFieldPreview").replaceWith($txtareaPre);
            $txtareaPre.attr("class", sclases);

            $txtareaPre.attr("maxlength", maxLength);
            $($txtareaPre).val(textValue);
            $($txtarea).val(textValue);
        }
        else if (isMultiLine == false) {
            var $input = $("<input />");
            $input.attr("id", "txtDefaultValue");
            $input.attr("class", "form-control");
            //$input.val(this.value);
            $input.attr("class", sclases);
            $input.attr("maxlength", maxLength);
            $input.attr("onblur", "Admin_CCMQuestionDetails.setTextFieldPreview(this);");
            $("#txtDefaultValue").replaceWith($input);
            var $inputPre = $("<input />");
            $inputPre.attr("id", "txtTextFieldPreview");
            $inputPre.attr("class", "form-control");
            $inputPre.attr("class", sclases);
            $inputPre.attr("maxlength", maxLength);
            //$input.val(this.value);
            $("#txtTextFieldPreview").replaceWith($inputPre);
            $($inputPre).val(textValue);
            $($input).val(textValue);
        }
    },
    updateGlobalQuestion: function (containerWdgt) {
        var questionHtml = '';
        if (containerWdgt) {
            questionHtml = $(containerWdgt).html().trim();
        }
        //if (Admin_CCMQuestionDetails.params["FromQuestionGroup"]) {
        //    questionHtml = $(containerWdgt.find('input').val());
        //}
        var objData = new Object();
        objData["QuestionName"] = $(questionHtml).attr("name");
        objData["QuestionId"] = $(questionHtml).attr("QuestionId");
        objData["QuestionHTML"] = questionHtml;
        objData["IsActive"] = "true";
        objData["commandType"] = "update_global_question";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Template", "GlobalQuestion");
    },

    globalQuestionUpdate: function () {
        var self = $('#pnlAdminCCMQuestionDetails');
        var question = self.find('#divSelectedQuestion');//.find('div.toolControldiv');
        Admin_CCMQuestionDetails.updateGlobalQuestion(question).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                self.find('#divSelectedQuestion').empty();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
}

function readImageURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imdIDPreview')
                .attr('src', e.target.result)
                .width(200)
                .height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}
//var width = $("#someElem").inlineStyle("width");
(function ($) {
    $.fn.inlineStyle = function (prop) {
        var styles = this.attr("style"),
            value;
        styles && styles.split(";").forEach(function (e) {
            var style = e.split(":");
            if ($.trim(style[0]) === prop) {
                value = style[1];
            }
        });
        return value;
    };
}(jQuery));