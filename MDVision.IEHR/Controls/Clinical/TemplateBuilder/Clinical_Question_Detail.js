questionDetail = {
    params: [],
    bIsFirstLoad: true,

    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    BillId: -1,

    Load: function (params) {

        questionDetail.params = params;

        if (questionDetail.params["PanelID"] != 'questionDetail')
            questionDetail.params["PanelID"] = questionDetail.params["PanelID"] + ' #questionDetail'

        if (questionDetail.bIsFirstLoad) {
            questionDetail.bIsFirstLoad = false;

            var self = $('#' + questionDetail.params["PanelID"]);

            self.loadDropDowns(true).done(function () {

                if (questionDetail.params.mode == "Add") {

                    questionDetail.HideAllControls();
                    $("#questionName").attr("disabled", "disabled");

                    //serialize data
                    $('#frmquestionDetail').data('serialize', $('#frmquestionDetail').serialize());
                }
                else if (questionDetail.params.mode == "Edit") {

                    questionDetail.HideAllControls();
                    if (questionDetail.params.QuestionTypeId == 1) {

                        $('#divAnswerTextFieldAnswer').show();
                        $('#divPreviewTextFieldPreview').show();

                    }
                    else if (questionDetail.params.QuestionTypeId == 2) {

                        $('#divAnswerRadioFieldAnswer').show();
                        $('#divPreviewRadioFieldPreview').show();

                    }
                    else if (questionDetail.params.QuestionTypeId == 3) {

                        $('#divAnswerDropdownFieldAnswer').show();
                        $('#divPreviewDropdownFieldPreview').show();

                    }
                    else if (questionDetail.params.QuestionTypeId == 5) {

                        $('#divAnswerFractionFieldAnswer').show();
                        $('#divPreviewFractionFieldPreview').show();

                    }
                    else if (questionDetail.params.QuestionTypeId == 6 || questionDetail.params.QuestionTypeId == 8) {

                        $('#divAnswerDateFieldAnswer').show();
                        $('#divPreviewDateFieldPreview').show();

                    }
                    else if (questionDetail.params.QuestionTypeId == 7) {

                        $('#divAnswerTimeFieldAnswer').show();
                        $('#divPreviewTimeFieldPreview').show();

                    }
                    else if (questionDetail.params.QuestionTypeId == 9) {

                        $('#divAnswerImageFieldAnswer').show();
                        $('#divPreviewImageFieldPreview').show();

                    }
                    else if (questionDetail.params.QuestionTypeId == 10) {

                        $('#divAnswerNumberFieldAnswer').show();
                        $('#divPreviewNumberFieldPreview').show();

                    }
                    questionDetail.LoadQuestion();
                }

                questionDetail.ValidateQuestionDetail(questionDetail.params.QuestionId);

            });

           
        }
    },

    LoadQuestion: function (QuestionId, mode) {

        //AppPrivileges.GetFormPrivileges("Messages", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //if (strMessage == "") {

        if (questionDetail.params.mode == "Add") {

            //serialize data
            $('#frmquestionDetail').data('serialize', $('#frmquestionDetail').serialize());

        }
        else if (questionDetail.params.mode == "Edit") {

            questionDetail.FillQuestion(questionDetail.params.QuestionId).done(function (response) {
                if (response.status != false) {

                    var Question_detail = JSON.parse(response.QuestionLoad_JSON);
                    var self = $('#questionDetail');
                    utility.bindMyJSON(true, JSON.parse(response.QuestionLoad_JSON), false, self).done(function () {

                        $('#ShortName,#questionType').attr('disabled', true);

                        questionDetail.getText();

                        if (Question_detail.chkNewLine == "True") {
                            $("#" + questionDetail.params["PanelID"] + " #chkNewLine").prop("checked", true);
                        } else {
                            $("#" + questionDetail.params["PanelID"] + " #chkNewLine").prop("checked", false);
                        }

                        if (questionDetail.params.QuestionTypeId == 1) {

                            if (Question_detail.chkAutoComplete == "True") {
                                $("#" + questionDetail.params["PanelID"] + " #chkAutoComplete").prop("checked", true);
                            } else {
                                $("#" + questionDetail.params["PanelID"] + " #chkAutoComplete").prop("checked", false);
                            }
                            if (Question_detail.radSingleLine == "True") {
                                $("#" + questionDetail.params["PanelID"] + " #radSingleLine").prop("checked", true);
                            } else {
                                $("#" + questionDetail.params["PanelID"] + " #radMultiLine").prop("checked", true);
                            }

                        }

                        //serialize data
                        $('#frmquestionDetail').data('serialize', $('#frmquestionDetail').serialize());
                    });

                   

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        // }
        //else {
        //    utility.DisplayMessages(strMessage, 2);
        //}
        //});

    },

    HideAllControls: function () {
        $('div[id^="divAnswer"]').hide();
        $('div[id^="divPreview"]').hide();
    },

    HideAllPreviewDivs: function () {
        $('div[id^="divPreview"]').hide();
    },

    FillQuestion: function (questionId) {
        var data = "QuestionId=" + questionId;
        return MDVisionService.defaultService(data, "CLINICAL_QUESTION", "FILL_QUESTION");
    },

    ValidateQuestionDetail: function (QuestionID) {
        $('#frmquestionDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  ShortName: {
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
                  questionName: {
                      group: '.col-xs-12',
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
           questionDetail.saveQuestion(QuestionID);
       });
    },

    saveQuestion: function (QuestionID) {
        var strMessage = "";
        var self = $('#questionDetail');
        var myJSON = self.getMyJSON();

        //if (questionDetail.params.mode == null) {
        //AppPrivileges.GetFormPrivileges("Messages", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var filess = questionDetail.FilesContainer.Files;
            if (questionDetail.params.mode == "Edit") {
                //questionDetail.QuestionEdit(myJSON, QuestionID).done(function (response) {
                //    if (response.status != false) {
                //        utility.DisplayMessages(response.message, 1);
                //        if (questionDetail.params != null && questionDetail.params.ParentCtrl != null) {
                //            UnloadActionPan(questionDetail.params.ParentCtrl, 'questionDetail');
                //        }
                //        else
                //            UnloadActionPan(null, 'questionDetail');
                //    }
                //    else {
                //        utility.DisplayMessages(response.Message, 3);
                //    }
                //});
                if (filess && filess[0]) {
                    var reader = new FileReader();
                    var file = "";
                    reader.onload = function (e) {
                        file = e.target.result;
                        questionDetail.QuestionEdit(myJSON, QuestionID, file).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                if (questionDetail.params != null && questionDetail.params.ParentCtrl != null) {
                                    UnloadActionPan(questionDetail.params.ParentCtrl, 'questionDetail');
                                }
                                else
                                    UnloadActionPan(null, 'questionDetail');
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    };
                    reader.readAsDataURL(filess[0]);
                } else {
                    questionDetail.QuestionEdit(myJSON, QuestionID, file).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            if (questionDetail.params != null && questionDetail.params.ParentCtrl != null) {
                                UnloadActionPan(questionDetail.params.ParentCtrl, 'questionDetail');
                            }
                            else
                                UnloadActionPan(null, 'questionDetail');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
            else if (questionDetail.params.mode == "Add") {
                var filess = questionDetail.FilesContainer.Files;
                if (filess && filess[0]) {
                    var reader = new FileReader();
                    var file = "";
                    reader.onload = function (e) {
                        file = e.target.result;
                        questionDetail.QuestionSave(myJSON, file).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                if (questionDetail.params != null && questionDetail.params.ParentCtrl != null) {
                                    UnloadActionPan(questionDetail.params.ParentCtrl, 'questionDetail');
                                }
                                else
                                    UnloadActionPan(null, 'questionDetail');
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    };
                    reader.readAsDataURL(filess[0]);
                } else {
                    questionDetail.QuestionSave(myJSON, file).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            if (questionDetail.params != null && questionDetail.params.ParentCtrl != null) {
                                UnloadActionPan(questionDetail.params.ParentCtrl, 'questionDetail');
                            }
                            else
                                UnloadActionPan(null, 'questionDetail');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
        }
        else
            utility.DisplayMessages(strMessage, 2);
        Clinical_Question.QuestionSearch();
    },

    QuestionSave: function (QuestionData, file) {
        var data = "QuestionData=" + QuestionData + "&file=" + file;
        return MDVisionService.defaultService(data, "CLINICAL_QUESTION", "SAVE_QUESTION");
    },

    QuestionEdit: function (QuestionData, QuestionID, file) {
        var data = "QuestionData=" + QuestionData + "&QuestionID=" + QuestionID + "&file=" + file;
        return MDVisionService.defaultService(data, "CLINICAL_QUESTION", "UPDATE_QUESTION");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmquestionDetail', function () {
            UnloadActionPan();
            Clinical_Question.QuestionSearch();
        }, function () {
            UnloadActionPan();
            Clinical_Question.QuestionSearch();
        });
    },

    QuestionTypeChanged: function (e, type) {

        var questionType = $('#questionType :selected').text();
        var questionTypeTrimed = $('#questionType :selected').text().replace(/ /g, '');

        $("#questionName").removeAttr("disabled");

        if (questionType === 'Radio Button') {

            questionDetail.HideAllControls();
            $('#divAnswerRadioFieldAnswer').show();
            $('#divPreviewRadioFieldPreview').show();
        }
        else if (questionType === 'Text Field') {

            questionDetail.HideAllControls();
            $('#divAnswerTextFieldAnswer').show();
            $('#divPreviewTextFieldPreview').show();
        }
        else if (questionType === 'Drop Down') {
            questionDetail.HideAllControls();
            $('#divAnswerDropdownFieldAnswer').show();
            $('#divPreviewDropdownFieldPreview').show();
        }

        else if (questionType === 'Multi Select Drop Down') {
            questionDetail.HideAllControls();
            $('#divAnswerMultiSelectDropdownFieldAnswer').show();
            $('#divPreviewMultiSelectDropdownFieldPreview').show();
        }
        else if (questionType === 'Fraction Field') {
            questionDetail.HideAllControls();
            $('#divAnswerFractionFieldAnswer').show();
            $('#divPreviewFractionFieldPreview').show();
        }
        else if (questionType === 'Date') {
            questionDetail.HideAllControls();
            $('#divAnswerDateFieldAnswer').show();
            $('#divPreviewDateFieldPreview').show();
        }
        else if (questionType === 'Time') {
            questionDetail.HideAllControls();
            $('#divAnswerTimeFieldAnswer').show();
            $('#divPreviewTimeFieldPreview').show();
        }
        else if (questionType === 'Image Field') {
            questionDetail.HideAllControls();
            $('#divAnswerImageFieldAnswer').show();
            $('#divPreviewImageFieldPreview').show();
        }
        else if (questionType === 'Number Field') {
            questionDetail.HideAllControls();
            $('#divAnswerNumberFieldAnswer').show();
            $('#divPreviewNumberFieldPreview').show();
        }
    },

    getText: function () {

        var questionType = $('#questionType :selected').text();
        if (questionType === 'Radio Button') {

            questionDetail.HideAllPreviewDivs();
            $('#divPreviewRadioFieldPreview').show();

        } else if (questionType === 'Text Field') {

            questionDetail.HideAllPreviewDivs();
            $('#divPreviewTextFieldPreview').show();

        } else if (questionType === 'Drop Down') {

            questionDetail.HideAllPreviewDivs();
            $('#divPreviewDropdownFieldPreview').show();

        } else if (questionType === 'Multi Select Drop Down') {

            questionDetail.HideAllPreviewDivs();
            $('#divPreviewMultiSelectDropdownFieldPreview').show();

        } else if (questionType === 'Fraction Field') {

            questionDetail.HideAllPreviewDivs();
            $('#divPreviewFractionFieldPreview').show();

        } else if (questionType === 'Date') {

            questionDetail.HideAllPreviewDivs();
            $('#divPreviewDateFieldPreview').show();

        } else if (questionType === 'Time') {

            questionDetail.HideAllPreviewDivs();
            $('#divPreviewTimeFieldPreview').show();

        } else if (questionType === 'Image Field') {

            questionDetail.HideAllPreviewDivs();
            $('#divPreviewImageFieldPreview').show();

        } else if (questionType === 'Number Field') {

            questionDetail.HideAllPreviewDivs();
            $('#divPreviewNumberFieldPreview').show();

        }

        $('#lblTextFieldPreview').text($('#questionName').val());
        $('#lblRadioFieldPreview').text($('#questionName').val());
        $('#lblDropdownFieldPreview').text($('#questionName').val());
        $('#lblMultiSelectDropdownFieldPreview').text($('#questionName').val());
        $('#lblFractionFieldPreview').text($('#questionName').val());
        $('#lblDateFieldPreview').text($('#questionName').val());
        $('#lblTimeFieldPreview').text($('#questionName').val());
        $('#lblImageFieldPreview').text($('#questionName').val());
        $('#lblNumberFieldPreview').text($('#questionName').val());

    },

    getRadioLabel1Text: function () {

        $('#lblRadioFieldPreview1').text($('#questionRadioLabel1').val());
    },

    getRadioLabel2Text: function () {

        $('#lblRadioFieldPreview2').text($('#questionRadioLabel2').val());
    },

    getFractionLabel1Text: function () {

        $('#txtFractionFieldPreview1').val($('#questionFarctionLabel1').val());
    },

    getFractionLabel2Text: function () {

        $('#txtFractionFieldPreview2').val($('#questionFarctionLabel2').val());
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
        //alert(JSON.stringify(texts));
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

    BufferFile: function (obj) {
        var toReturn = true;

        if (obj.files) {
            questionDetail.ValidateUploadedFiles();
            questionDetail.FilesContainer.Files = obj.files;
        }
        return toReturn;
    },

    ValidateUploadedFiles: function () {
        var fileName = "";
        var files = $('#Upload_Import_file').get(0).files;
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "application/pdf" && fileType != "image/jpeg" && fileType != "image/png" && fileType != "image/jpg" && fileType != "image/gif" && fileType != "image/bmp") {
                utility.DisplayMessages("File Type is Invalid", 4);
                document.getElementById("uploadFilePH").value = "";
                $('#totalFiles').text("0 file(s) selected");
                $('#Upload_Import_file').val('');
                return false;
            }
            if (questionDetail.ValidateFileSize(files) > Number(globalAppdata['FileSize'])) {
                utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
                document.getElementById("uploadFilePH").value = "";
                $('#totalFiles').text("0 file(s) selected");
                $('#Upload_Import_file').val('');
                return false;
            }
            fileName = fileName + files[i].name + ',';
        }
        fileName = fileName.slice(0, fileName.length - 1);
        document.getElementById("uploadFilePH").value = fileName;
        $('#totalFiles').text(files.length + " file(s) selected");
        return true;
    },

    ValidateFileSize: function (files) {
        var size = 0;
        $.each(files, function (index, file) {
            size = size + Number((file.size / (1024 * 1024)).toFixed(2));
        });
        return size;
    },
}

function readImageURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            //$('#imdID')
            //    .attr('src', e.target.result)
            //    .width(200)
            //    .height(200);
            $('#imdIDPreview')
                .attr('src', e.target.result)
                .width(200)
                .height(200);
        };

        reader.readAsDataURL(input.files[0]);
    }
}