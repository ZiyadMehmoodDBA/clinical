Clinical_GlobalQuestionGroup = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_GlobalQuestionGroup.params = params;
        if (Clinical_GlobalQuestionGroup.params["PanelID"] != 'pnlClinicalGlobalQuestionGroup')
            Clinical_GlobalQuestionGroup.params["PanelID"] = Clinical_GlobalQuestionGroup.params["PanelID"] + ' #pnlClinicalGlobalQuestionGroup';
        if (Clinical_GlobalQuestionGroup.bIsFirstLoad) {
            Clinical_GlobalQuestionGroup.bIsFirstLoad = false;
            var self = $('#' + Clinical_GlobalQuestionGroup.params.PanelID);
            self.loadDropDowns(true).done(function () {
                Clinical_GlobalQuestionGroup.ValidateGlobalQuestionGroup();
                if (Clinical_GlobalQuestionGroup.params["mode"] == "Edit" && Clinical_GlobalQuestionGroup.params.QuestionGroupId > 0) {
                    Clinical_GlobalQuestionGroup.globalQuestionGroupFill(Clinical_GlobalQuestionGroup.params.QuestionGroupId);
                    $('#frmGlobalQuestionGroup').data('serialize', $('#frmGlobalQuestionGroup').serialize());
                }
                else {
                    Clinical_GlobalQuestionGroup.GlobalQuestionGroupLoad();
                }
            });
            $(".globalQuestionGroupHeading").focusout(function () {
                if ($(this).is(":visible") && $(this).val() != "") {
                    $(this).hide().siblings("#lblGlobalQuestionGroupHeading").show().text($(this).val());
                    $('#txtFormHeading').val($(this).val());
                    $('#lnkEditFormName').show();
                }
            });
        }
    },
    clearGlobalQuestionGroup: function () {

    },
    globalQuestionGroupFill: function (QuestionGroupId) {
        Clinical_GlobalQuestionGroup.globalQuestionGroupFill_DBCall(QuestionGroupId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_GlobalQuestionGroup.globalQuestionGroupFillData(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    globalQuestionGroupFill_DBCall: function (QuestionGroupId) {
        var objData = {};
        objData["QuestionGroupId"] = QuestionGroupId;
        objData["commandType"] = "Fill_Global_Question_Group";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "GlobalQuestionGroup");
    },
    globalQuestionGroupFillData: function (response) {
        var globalQuestionGroupData = response.listGlobalQuestion;
        $('#' + Clinical_GlobalQuestionGroup.params.PanelID + ' input#txtGroupName').val(globalQuestionGroupData[0].QuestionGroupName);

        $('#' + Clinical_GlobalQuestionGroup.params.PanelID + ' input#txtGroupTitile').val(globalQuestionGroupData[0].QuestionGroupTitle);
        $('#' + Clinical_GlobalQuestionGroup.params.PanelID + ' input#chkActive').prop('checked', globalQuestionGroupData[0].IsActive == 'True' ? true : false);
        $('#' + Clinical_GlobalQuestionGroup.params.PanelID + ' input#chkSaveGlobally').prop('checked', globalQuestionGroupData[0].SaveGlobally == 'True' ? true : false);
        if ($('#' + Clinical_GlobalQuestionGroup.params.PanelID + ' input#chkActive').hasClass('disableAll'))
            $('#' + Clinical_GlobalQuestionGroup.params.PanelID + ' input#chkActive').removeClass('disableAll');
        $('#' + Clinical_GlobalQuestionGroup.params.PanelID + ' #ddlCanvasCol').val(globalQuestionGroupData[0].CanvasCols);
        $('#frmGlobalQuestionGroup').data('serialize', $('#frmGlobalQuestionGroup').serialize());
    },
    GlobalQuestionGroupLoad: function () {
        if (Clinical_GlobalQuestionGroup.params.mode == "Add") {
            $('#frmClinical_GlobalQuestionGroup').data('serialize', $('#frmClinical_GlobalQuestionGroup').serialize());
        }
        else {
            Clinical_GlobalQuestionGroup.SectionLoadEditMode();
        }
    },
    UnLoadTab: function () {
        utility.UnLoadDialog("frmGlobalQuestionGroup", function () {
            UnloadActionPan(Clinical_GlobalQuestionGroup.params["ParentCtrl"], "Clinical_GlobalQuestionGroup");
        }, function () {
            UnloadActionPan(Clinical_GlobalQuestionGroup.params["ParentCtrl"], "Clinical_GlobalQuestionGroup");
        });
    },
    GlobalQuestionGroupSave: function () {
        var strMessage = "";
        var self = $("#" + Clinical_GlobalQuestionGroup.params.PanelID + " #frmGlobalQuestionGroup");
        var myJSON = self.getMyJSONByName();
        if (Clinical_GlobalQuestionGroup.params.mode == "Add" && Clinical_GlobalQuestionGroup.params.UniqueQuestionId) {
            Clinical_GlobalQuestionGroup.SaveGlobalQuestionGroup(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (Clinical_GlobalQuestionGroup.params.UniqueQuestionId) {
                        var customFormQuestionGroup = $('#' + Clinical_GlobalQuestionGroup.params.UniqueQuestionId);
                        if ($(customFormQuestionGroup).find("#lblQuestionGroupTitle")) {
                            if (self.find("#txtGroupTitile").val() != "") {
                                $(customFormQuestionGroup).find("#lblQuestionGroupTitle").empty();
                                $(customFormQuestionGroup).find("#lblQuestionGroupTitle").append(self.find("#txtGroupTitile").val());
                            }
                        }
                        $(customFormQuestionGroup).attr('QuestionGroupId', response.QuestionGroupId);
                        $(customFormQuestionGroup).attr('QuestionGroupName', self.find('#txtGroupName').val());
                        $(customFormQuestionGroup).attr('SaveGlobally', self.find("#chkSaveGlobally").is(':checked'));
                        $(customFormQuestionGroup).attr('IsActive', self.find("#chkIsActive").is(':checked'));
                        $(customFormQuestionGroup).attr('QuestionGroupTitle', self.find('#txtGroupTitile').val());
                        $(customFormQuestionGroup).attr('CanvasCol', self.find('#ddlCanvasCol option:selected').val());
                        $(customFormQuestionGroup).find('#toolQuestionGroupHTML').attr('CanvasCol', self.find('#ddlCanvasCol option:selected').val());
                    }
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_GlobalQuestionGroup.params.QuestionGroupId = response.QuestionGroupId;
                    if (Clinical_GlobalQuestionGroup.params && Clinical_GlobalQuestionGroup.params.ParentCtrl == "Clinical_CustomFormsDetails")
                        Clinical_CustomFormsDetails.InitializeDragableGroup();
                    // UnloadActionPan('Clinical_CustomFormsDetails', 'Clinical_GlobalQuestionGroup');
                    if (Clinical_GlobalQuestionGroup.params != null && Clinical_GlobalQuestionGroup.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_GlobalQuestionGroup.params.ParentCtrl, 'Clinical_GlobalQuestionGroup');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_GlobalQuestionGroup');
                    setTimeout(function () {
                        Clinical_GlobalQuestionGroup.fillGlobalQuestionGroups();
                        Clinical_CustomFormsDetails.updateCustomeFormHTML();
                        Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
                    }, 300)
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else if (Clinical_GlobalQuestionGroup.params.mode == "Edit") {
            var selectedSchTypeId = $("#" + Clinical_GlobalQuestionGroup.params.PanelID + " #frmGlobalQuestionGroup #ddlScheduleType option:selected").val();
            Clinical_GlobalQuestionGroup.UpdateGlobalQuestionGroup(myJSON, Clinical_GlobalQuestionGroup.params.UniqueQuestionId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var customFormQuestionGroup = $('#' + Clinical_CustomFormsDetails.params.PanelID + ' #' + Clinical_GlobalQuestionGroup.params.UniqueQuestionId);
                    //$(customFormQuestionGroup).find("#lblQuestionGroupTitle").text(self.find('#txtGroupTitile').val());
                    if ($(customFormQuestionGroup).find("#lblQuestionGroupTitle")) {
                        if (self.find("#txtGroupTitile").val() != "") {
                            $(customFormQuestionGroup).find("#lblQuestionGroupTitle").empty();
                            $(customFormQuestionGroup).find("#lblQuestionGroupTitle").append(self.find("#txtGroupTitile").val());
                        }
                    }
                    $(customFormQuestionGroup).attr('QuestionGroupName', self.find('#txtGroupName').val());
                    $(customFormQuestionGroup).attr('SaveGlobally', self.find("#chkSaveGlobally").is(':checked'));
                    $(customFormQuestionGroup).attr('IsActive', self.find("#chkIsActive").is(':checked'));
                    $(customFormQuestionGroup).attr('QuestionGroupTitle', self.find('#txtGroupTitile').val());
                    $(customFormQuestionGroup).attr('CanvasCol', self.find('#ddlCanvasCol option:selected').val());
                    $(customFormQuestionGroup).find('#toolQuestionGroupHTML').attr('CanvasCol', self.find('#ddlCanvasCol option:selected').val());
                    utility.DisplayMessages(response.Message, 1);
                    if (Clinical_GlobalQuestionGroup.params != null && Clinical_GlobalQuestionGroup.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_GlobalQuestionGroup.params.ParentCtrl, 'Clinical_GlobalQuestionGroup');
                    }
                    else
                        UnloadActionPan(null, 'Clinical_GlobalQuestionGroup');
                    setTimeout(function () {
                        Clinical_GlobalQuestionGroup.fillGlobalQuestionGroups();
                        Clinical_CustomFormsDetails.updateCustomeFormHTML();
                        Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
                    }, 300)
                }
                else {
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
    },
    ValidateGlobalQuestionGroup: function () {
        $('#' + Clinical_GlobalQuestionGroup.params.PanelID + ' #frmGlobalQuestionGroup')
        .bootstrapValidator('destroy');
        $('#' + Clinical_GlobalQuestionGroup.params.PanelID + ' #frmGlobalQuestionGroup')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   QuestionGroupName: {
                       group: '.col-lg-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Clinical_GlobalQuestionGroup.GlobalQuestionGroupSave();
        });
    },
    SaveGlobalQuestionGroup: function (jsonData) {
        var objData = JSON.parse(jsonData);
        objData["commandType"] = "SAVE_GLOBAL_QUESTION_GROUP";
        var customFormQuestionGroup = $('#' + Clinical_GlobalQuestionGroup.params.UniqueQuestionId);
        objData["QuestionGroupHTML"] = $(customFormQuestionGroup).parent().html().trim();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "GlobalQuestionGroup");
    },
    UpdateGlobalQuestionGroup: function (jsonData, UniqueQuestionId) {
        var objData = JSON.parse(jsonData);
        var customFormQuestionGroup = $('#' + UniqueQuestionId);
        objData["QuestionGroupId"] = Clinical_GlobalQuestionGroup.params.QuestionGroupId;
        objData["QuestionGroupHTML"] = $(customFormQuestionGroup).parent().html().trim();
        //objData["CanvasCols"] = $(customFormQuestionGroup).attr('CanvasCols');
        objData["commandType"] = "UPDATE_GLOBAL_QUESTION_GROUP";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "GlobalQuestionGroup");
    },
    globalQuestionsSelect_DBCall: function (PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 1000;
        }

        var objData = {};
        objData["commandType"] = "load_global_question";
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "GlobalQuestion");
    },

    globalQuestionGroupSelect_DBCall: function (PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 1000;
        }

        var objData = {};
        objData["commandType"] = "load_global_question_group_savedglobally";
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "GlobalQuestionGroup");
    },

    showQuestions: function (obj) {
        $('#divQuestionProperties button').removeClass('active');
        var button = $(obj);
        if (button.text() == "Question Groups") {
            $('#ulQuestionGroups').show();
            $('#ulQuestions').hide();
            $('#divQuestionProperties #btnQuestionGroups').addClass('active');
        } else {
            $('#ulQuestionGroups').hide();
            $('#ulQuestions').show();
            $('#divQuestionProperties #btnQuestion').addClass('active');
        }

    },

    deleteQuestionGroup: function (questionGroupId, event) {
        if (event != null)
            event.stopPropagation;
        utility.myConfirm('1', function () {
            if (questionGroupId > 0) {
                Clinical_GlobalQuestionGroup.deleteQuestionGroup_DBCall(questionGroupId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if ($('#customFormDetails').find("[id^='toolQuestionGroup_']").length > 0)
                        {
                            $('#customFormDetails').find("[id^='toolQuestionGroup_']").each(function (i,e) {
                                if( parseInt(questionGroupId) == parseInt($(e).attr('questiongroupid')));
                                $(e).attr('questiongroupid', '-1');
                            });
                        }
                        Clinical_GlobalQuestionGroup.fillGlobalQuestionGroups();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        });
    },

    deleteQuestion: function (questionId, event) {
        if (event != null)
            event.stopPropagation;
        utility.myConfirm('1', function () {
            if (questionId > 0) {
                Clinical_GlobalQuestionGroup.deleteQuestion_DBCall(questionId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_GlobalQuestionGroup.fillGlobalQuestions();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            }
        });
    },

    deleteQuestionGroup_DBCall: function (questionGroupId) {
        var objData = {};
        objData["QuestionGroupId"] = questionGroupId
        objData["commandType"] = "delete_global_question_group";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "GlobalQuestionGroup");
    },

    deleteQuestion_DBCall: function (questionId) {
        var objData = {};
        objData["QuestionId"] = questionId
        objData["commandType"] = "delete_global_question";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "GlobalQuestion");
    },

    questionGroupFillData: function (response) {
        var questionGroupData = response.listQuestionGroup;
        var ulQuestionGroup = $('#ulQuestionGroups');
        ulQuestionGroup.empty();
        if (response.questionGroupCount > 0) {
            if (!(typeof (questionsData) == "string")) {
                $.each(questionGroupData, function (index, elem) {
                    var liHtml = "<li class='ui-state-default pl-xs' onclick='Clinical_GlobalQuestionGroup.openQuestionGroupDetail(this,event)'><div class='col-xs-12 p-none ellipses size-max90per' title=" + elem.QuestionGroupName + "> <i class='fa fa-arrows-alt black mr-xs'></i>" + elem.QuestionGroupName + "</div> <a class='removeIconList' href='javascript:void(0);' onclick='Clinical_GlobalQuestionGroup.deleteQuestionGroup(" + elem.QuestionGroupId + ",event);' title='Delete Record'><i class='fa fa-close red'></i></a> <input type='hidden' value='" + elem.QuestionGroupHTML.replace('questionid="-1"', 'questionid = "' + elem.QuestionGroupId + '"') + "''><div class='clearfix'></div></li>";
                    ulQuestionGroup.append(liHtml);
                });
            }
        }
        $('#frmCustomFormsDetails #ulQuestionGroups li').draggable({
            revert: "invalid",
            appendTo: "#pnlClinicalCustomFormsDetails #customFormDetails",
            containment: '.dragableCF',
            connectToSortable: '.dragableCF',
            stack: "#pnlClinicalCustomFormsDetails #customFormDetails",
            connectWith: '.dragableCF',
            helper: function (ev, ui) {
                var ctrlHtml = $(this).find('input').val();
                var lstToAppend = '';
                lstToAppend = '<li class="draggable col-xs-12 mb-sm" style="height:160px;">' + ctrlHtml + '</li>';
                return lstToAppend;
            },
            stop: function (ev, ui) {
                $(ui.helper).removeAttr('style');
                if ($(ui.helper).find('.toolcontroldiv'))
                    $(ui.helper).find('.toolcontroldiv').attr('questiongroupid', '-1');
                setTimeout(function () {
                    Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
                }, 300);
            },
        });
    },

    questionsFillData: function (response) {
        var questionsData = response.listQuestions;
        var ulQuestion = $('#ulQuestions');
        ulQuestion.empty();
        if (!(typeof (questionsData) == "string")) {
            $.each(questionsData, function (index, elem) {
                var qstnHtml = $(elem.QuestionHTML);
                var questiontype = qstnHtml.attr('questiontype')
                questiontype = questiontype.split(/(?=[A-Z])/).join(' ');
                var liHtml = "<li class='ui-state-default pl-xs' onclick='Clinical_GlobalQuestionGroup.openQuestionDetail(this,event)'> <div class='col-xs-7 p-none ellipses size-max60per pr-default' title=" + elem.QuestionName + "><i class='fa fa-arrows-alt black mr-tiny'></i> " + elem.QuestionName + " </div> <div class='col-xs-5 p-none ellipses size-max40per pull-right pr-default' title=" + questiontype + "> " + questiontype + "</div> <a class='removeIconList' href='javascript:void(0);' onclick='Clinical_GlobalQuestionGroup.deleteQuestion(" + elem.QuestionId + ",event);' title='Delete Record'><i class='fa fa-close red'></i></a> <input type='hidden' value='" + elem.QuestionHTML.replace('questionid="-1"', 'questionid = "' + elem.QuestionId + '"') + "'><div class='clearfix'></div> </li>";
                ulQuestion.append(liHtml);
            });
        }

        var canvasCol = $("#" + Clinical_CustomFormsDetails.params.PanelID + " #frmCustomFormsDetails #ddlCanvasCol option:selected").val();

        $('#frmCustomFormsDetails #ulQuestions li').draggable({
            revert: "invalid",
            appendTo: "#pnlClinicalCustomFormsDetails #customFormDetails",
            containment: '.dragableCF',
            connectToSortable: '.dragableCF',
            stack: "#pnlClinicalCustomFormsDetails #customFormDetails",
            connectWith: '.dragableCF',
            helper: function (ev, ui) {
                var bootStrpCss = 'col-xs-12';
                switch (canvasCol) {
                    case "1":
                        bootStrpCss = 'col-xs-12'
                        break;
                    case "2":
                        bootStrpCss = 'col-xs-12 col-sm-6'
                        break;
                    case "3":
                        bootStrpCss = 'col-xs-12  col-sm-4'
                        break;
                    default:
                        bootStrpCss = 'col-xs-12'
                        break;

                }

                var ctrlHtml = $(this).find('input').val();
                var questiontype = $(ctrlHtml).attr('questiontype');
                var lstToAppend = '';
                if (questiontype == "Table") {
                    lstToAppend = '<li class="draggable col-xs-12 mb-sm">' + ctrlHtml + '</li>';
                }
                else if (questiontype == "QuestionGroup") {
                    lstToAppend = '<li class="draggable col-xs-12 mb-sm">' + ctrlHtml + '</li>';
                    Clinical_CustomFormsDetails.InitializeDragableGroup($(ctrlHtml));
                }
                else if (questiontype == "Header") {
                    lstToAppend = '<li class="draggable col-xs-12 mb-sm">' + ctrlHtml + '</li>';
                }
                else {
                    lstToAppend = '<li class="draggable ' + bootStrpCss + ' mb-sm">' + ctrlHtml + '</li>';
                }
                return lstToAppend;

            },
            stop: function (ev, ui) {
                $(ui.helper).removeAttr('style');
                setTimeout(function () {
                    Clinical_CustomFormsDetails.customFormHTMLHoverEvent();
                }, 300);
            },

        });

    },

    fillGlobalQuestions: function () {
        Clinical_GlobalQuestionGroup.globalQuestionsSelect_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_GlobalQuestionGroup.questionsFillData(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    fillGlobalQuestionGroups: function () {
        Clinical_GlobalQuestionGroup.globalQuestionGroupSelect_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_GlobalQuestionGroup.questionGroupFillData(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    filterQuestions: function (obj) {
        if (obj != null) {
            var strSearch = $(obj).val();

            if ($('#btnQuestion').hasClass('active')) {
                $('#ulQuestions li').each(function () {
                    var currentLiText = $(this).text().trim();
                    var showCurrentLi = currentLiText.toLowerCase().indexOf(strSearch.toLowerCase()) > -1 ? true : false;
                    $(this).toggle(showCurrentLi);
                });
            }
            else if ($('#btnQuestionGroups').hasClass('active')) {
                $('#ulQuestionGroups li').each(function () {
                    var currentLiText = $(this).text().trim();
                    var showCurrentLi = currentLiText.toLowerCase().indexOf(strSearch.toLowerCase()) > -1 ? true : false;
                    $(this).toggle(showCurrentLi);
                });

            }

        }
    },


    openQuestionDetail: function (obj, event) {
        if (event != null) {
            if ($(event.target).hasClass('fa-close')) {
                return;
            }
            else
                event.stopPropagation;
        }
        var selectedLi = $(obj);

        Clinical_CustomFormsDetails.EditQuestion(null, null, selectedLi);
    },
    openQuestionGroupDetail: function (obj, event) {
        if (event != null) {
            if ($(event.target).hasClass('fa-close')) {
                return;
            }
            else
                event.stopPropagation;
        }
        var selectedLi = $(obj);

        //  Clinical_CustomFormsDetails.EditQuestion(null, null, selectedLi);
    },
}