Admin_CCMTemplateDetails = {
    bIsFirstLoad: true,
    params: [],
    AttachCatIds: '',
    isDragged: false,
    selectedSection: null,
    selectedQuestion: null,
    isBelongToQgroup: false,
    sectionId: -1,
    questionId: -1,
    isTemplateCreated: false,
    Load: function (params) {
        Admin_CCMTemplateDetails.params = params;
        if (Admin_CCMTemplateDetails.params["PanelID"] != 'pnlAdminCCMTemplateDetails')
            Admin_CCMTemplateDetails.params["PanelID"] = Admin_CCMTemplateDetails.params["PanelID"] + ' #pnlAdminCCMTemplateDetails';
        if (Admin_CCMTemplateDetails.bIsFirstLoad) {
            Admin_CCMTemplateDetails.bIsFirstLoad = false;
            var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
            if (Admin_CCMTemplateDetails.params.TemplateType) {
                self.find("#modalTitle").text(Admin_CCMTemplateDetails.params.TemplateType);
                if (Admin_CCMTemplateDetails.params.TemplateType == "Health Risk Assessment")
                    self.find('#divICDGroup').addClass('hidden');
            }
            selectedEntity = globalAppdata["SeletedEntityId"];

            self.loadDropDowns(true).done(function () {
                Admin_CCMTemplateDetails.ValidateTemplateDetails();
                Admin_CCMTemplateDetails.IntializeMultiSelectDropDownICDGroup();
                Admin_CCMTemplateDetails.fillICDGroupDropdown();
                if (Admin_CCMTemplateDetails.params["mode"] == "Edit" && Admin_CCMTemplateDetails.params.TemplateId > 0) {
                    Admin_CCMTemplateDetails.TemplateFill(Admin_CCMTemplateDetails.params.TemplateId);
                }
                Admin_CCMTemplateDetails.initializeDragable();
                Admin_CCMTemplateDetails.InitializeSortable();
                $('#frmAdminCCMTemplateDetails').data('serialize', $('#frmAdminCCMTemplateDetails').serialize());
            });
        }
    },
    fillICDGroupDropdown: function () {
        Admin_CCMTemplateDetails.fillICDGroupDropdown_Dbcall().done(function (response) {
            response = JSON.parse(response);
            if (response.icdGroupCount > 0) {
                $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup').empty();
                $.each(JSON.parse(response.icdGroupList_JSON), function (i, item) {
                    if (item.ShortName != '' && item.ShortName != null) {
                        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup').append(
                            $('<option/>', {
                                value: item.ICDGroupId,
                                html: item.ShortName,
                            })
                        );
                    }
                });

                $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup').multiselect('destroy');

                $('#' + Admin_CCMTemplateDetails.params["PanelID"] + ' #ddlICDGroup').multiselect({
                    includeSelectAllOption: true,
                    enableFiltering: true,
                    enableCaseInsensitiveFiltering: true,
                    onDropdownShow: function (event) {
                        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup').parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                    },

                });
                $('#frmAdminCCMTemplateDetails').data('serialize', $('#frmAdminCCMTemplateDetails').serialize());
            }
        });
    },
    fillICDGroupDropdown_Dbcall: function () {
        var objData = new Object();
        objData["commandType"] = "LOOKUP_ICDGROUP";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
    },
    TemplateFill: function (templateId) {
        Admin_CCMTemplateDetails.TemplateFill_DBCall(templateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Admin_CCMTemplateDetails.TemplateFillData(response);
                $('#frmAdminCCMTemplateDetails').data('serialize', $('#frmAdminCCMTemplateDetails').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        setTimeout(function () {
            Admin_CCMTemplateDetails.initializeDragable();
            Admin_CCMTemplateDetails.InitializeSortable();
            $('#frmAdminCCMTemplateDetails').data('serialize', $('#frmAdminCCMTemplateDetails').serialize());
        }, 500);

    },
    TemplateFill_DBCall: function (templateId) {
        var objData = new Object();
        objData["commandType"] = "FILL_CCM_TEMPLATE";
        objData["TemplateId"] = templateId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
    },
    TemplateFillData: function (response) {
        var self = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");
        var TemplateData = response.Template_JSON;
        var questions = $.grep(response.Questions_JSON, function (el, i) {
            return el.ParentQuestId == null
        });
        var subQuestions = $.grep(response.Questions_JSON, function (el, i) {
            return el.ParentQuestId != null
        });
        var selectedSec = Admin_CCMTemplateDetails.SectionFillData(response);
        var selectedQust = Admin_CCMTemplateDetails.QuestionsFillData(questions, selectedSec);
        Admin_CCMTemplateDetails.SubQuestionsFillData(subQuestions, selectedQust, self);
        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' input#txtTemplateTitle').val(TemplateData[0].ShortName);
        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' input#txtDescription').val(TemplateData[0].Description);
        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' input#chkActive').prop('checked', TemplateData[0].IsActive == true ? true : false);
        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup').multiselect('clearSelection', false);
        if ($('#' + Admin_CCMTemplateDetails.params.PanelID + ' input#chkActive').hasClass('disableAll'))
            $('#' + Admin_CCMTemplateDetails.params.PanelID + ' input#chkActive').removeClass('disableAll');
        self.find('#pnlTemplateArea').removeClass('hidden');
        Admin_CCMTemplateDetails.setHeightOfUl(self.find('#lstQuestion'));
        Admin_CCMTemplateDetails.setHeightOfUl(self.find('#lstSubQuestion'));
        Admin_CCMTemplateDetails.params.TemplateId = TemplateData[0].TemplateId;
        setTimeout(function () {
            if (TemplateData[0].ICDGroupIds && TemplateData[0].ICDGroupIds != "") {
                EMRUtility.selectOptionsByCommaSeprateValue($('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup'), TemplateData[0].ICDGroupIds);
                $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup').multiselect("refresh");
            }
            $('#frmAdminCCMTemplateDetails').data('serialize', $('#frmAdminCCMTemplateDetails').serialize());
        }, 200);
    },
    SectionFillData: function (response) {
        var selectedSec = null;
        var Sections_JSON = response.Sections_JSON;
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        var lstSection = self.find('#lstSection');
        $(lstSection).empty();
        if (Sections_JSON && Sections_JSON.length > 0) {
            $.each(Sections_JSON, function (i, item) {
                if (item.SectionId != '' && item.SectionId != null) {
                    if (lstSection) {
                        var uniquId = utility.makeRendomKey();
                        $(lstSection).find("input[type='checkbox']").prop('checked', false);
                        $(lstSection).find("li").removeClass('active');
                        var templateId = Admin_CCMTemplateDetails.params.TemplateId == null ? "" : Admin_CCMTemplateDetails.params.TemplateId;
                        var sectionName = item.ShortName == null ? "" : item.ShortName;
                        var actionDiv = '<div class="col-xs-1 p-none">'
                            + '<div class="form-control pt-none pb-none">'
                            + '<a class="btn btn-xs pull-left" href="#" onclick="Admin_CCMTemplateDetails.RemoveSection(this,event);" title="Delete Record">'
                              + '<i class="fa fa-close red"></i></a>'
                              + '<input type="checkbox" class=" pull-left" onclick="Admin_CCMTemplateDetails.SetSelectedSection(this,event);" name="checkbox" id="secChkb_' + templateId + '_' + uniquId + '" checked="checked"/>'
                              + '<a class="btn btn-xs  pull-left" href="#" onclick="Admin_CCMTemplateDetails.enableSection(this,event,true);" title="Edit Question"><i class="fa fa-pencil black"></i></a>'
                              + '</div>'
                              + '</div>';
                        ctrlHtml = '<div  class="p-none sectionDiv col-xs-12" name="SectionId" sectionId="' + item.SectionId + '" id="section_' + templateId + '_' + uniquId + '">'
                            + actionDiv
                            + '<div class="col-xs-11 pr-none">'
                            + '<input class="form-control sectionField disableAll"  maxLength="55"  name="ShortName" id="sectionField_' + templateId + '_' + uniquId + '"type="text" value="' + sectionName + '" onblur="Admin_CCMTemplateDetails.disableSection(this,event,false);" >'
                            + '</div>'
                            + '</div>';
                        lstSection.prepend('<li class="col-xs-12 mb-xs active">' + ctrlHtml + '</li>');
                        self.find('#txtQuestionHeader').removeClass('hidden');
                        self.find('#txtQuestionHeader').val(sectionName);
                        selectedSec = item.SectionId;
                    }
                }
            });
            self.find('#btnPreview').prop('disabled', false);
        }
        return selectedSec;
    },
    QuestionsFillData: function (Questions_JSON, selectedSec) {
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        var lstQuestion = self.find('#lstQuestion');
        $(lstQuestion).empty();
        var lstSubQuestion = self.find('#lstSubQuestion');
        $(lstSubQuestion).empty();

        var selectedQuest = null;
        if (Questions_JSON && Questions_JSON.length > 0) {
            $.each(Questions_JSON, function (i, item) {
                if (item.QuestionId != '' && item.QuestionId != null && item.ParentQuestId == null && item.SectionId == selectedSec) {
                    var ctrlHtml = item.QuestionHTML;
                    $(lstQuestion).find("input[id='actionChkBox']").prop('checked', false);
                    $(lstQuestion).find("li").removeClass('active');
                    lstQuestion.append('<li class="col-xs-12 mb-xs active selectedQuest">' + ctrlHtml + '</li>');
                    var thisQuestion = lstQuestion.find('.selectedQuest').removeClass('selectedQuest');
                    $(thisQuestion).find('.toolcontroldiv').attr('QuestionId', item.QuestionId);
                    $(thisQuestion).find('.toolcontroldiv').attr('SectionId', item.SectionId);
                    $(thisQuestion).find('#actionChkBox').prop('checked', true);
                    self.find('#txtSubQuestionHeader').removeClass('hidden');
                    self.find('#txtSubQuestionHeader').val(item.Description.replace('*', ''));
                    selectedQuest = item.QuestionId;
                }
                else {
                    if (item.QuestionId != '' && item.QuestionId != null && item.ParentQuestId != null && item.ParentQuestId == selectedQuest) {
                        var ctrlHtml = item.QuestionHTML;
                        lstSubQuestion.append('<li class="col-xs-12 mb-xs active selectedQuest">' + ctrlHtml + '</li>');
                        var thisQuestion = lstSubQuestion.find('.selectedQuest').removeClass('selectedQuest');
                        $(thisQuestion).find('.toolcontroldiv').attr('QuestionId', item.QuestionId);
                        $(thisQuestion).find('.toolcontroldiv').attr('parentquestid', item.Prentquestid);
                    }
                }
            });
        }
        return selectedQuest;
    },
    SubQuestionsFillData: function (SubQuestions_JSON, selectedQuest, self) {
        var lstSubQuestion = self.find('#lstSubQuestion');
        $(lstSubQuestion).empty();
        if (SubQuestions_JSON && SubQuestions_JSON.length > 0) {
            $.each(SubQuestions_JSON, function (i, item) {
                if (item.QuestionId != '' && item.QuestionId != null && !item.Prentquestid && item.ParentQuestId == selectedQuest) {
                    var ctrlHtml = item.QuestionHTML;
                    $(lstSubQuestion).append('<li class="col-xs-12 mb-xs selectedQuest">' + ctrlHtml + '</li>');
                    var thisQuestion = $(lstSubQuestion).find('.selectedQuest').removeClass('selectedQuest');
                    $(thisQuestion).find('.toolcontroldiv').attr('QuestionId', item.QuestionId);
                    $(thisQuestion).find('.toolcontroldiv').attr('parentquestid', item.Prentquestid);
                }
            });
        }
    },
    IntializeMultiSelectDropDownICDGroup: function () {
        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247,
            nonSelectedText: 'All',
            selectAll: false,
            onChange: function (option, checked, select) {
                Admin_CCMTemplateDetails.checkICDGroupIds(option, checked, select);
            },
        });
        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup, #' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlProvider').multiselect('selectAll', false);
    },
    checkICDGroupIds: function (option, checked, select) {
        //specialty context
        var ICDGroupContext = '#' + Admin_CCMTemplateDetails.params.PanelID + ' #divICDGroup';
        var isAllICDGroupSelected = $(ICDGroupContext).find('.dropdown-menu').find('li.multiselect-all').hasClass('active');
        var ICDGroupItems = $(ICDGroupContext).find('.dropdown-menu').find('li').length;
        var checkedICDGroupItems = $(ICDGroupContext).find('.dropdown-menu').find('li.active').length;

        if (checkedICDGroupItems <= 0) {
            Admin_CCMTemplateDetails.icdGroupCheckedIds = [];
            Admin_CCMTemplateDetails.AttachCatIds = '';
        }
        else {
            if (!isAllICDGroupSelected && !(ICDGroupItems == checkedICDGroupItems)) {
                var spacialityId = $(option).attr("value");
                if (checked && spacialityId != "") {
                    Admin_CCMTemplateDetails.icdGroupCheckedIds = Admin_CCMTemplateDetails.removeFromArray(Admin_CCMTemplateDetails.icdGroupCheckedIds, spacialityId);
                    Admin_CCMTemplateDetails.icdGroupCheckedIds.push(spacialityId);
                }
                else {

                    Admin_CCMTemplateDetails.icdGroupCheckedIds = Admin_CCMTemplateDetails.removeFromArray(Admin_CCMTemplateDetails.icdGroupCheckedIds, spacialityId);
                }
            }
            else {
                Admin_CCMTemplateDetails.icdGroupCheckedIds = [];
                $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #ddlICDGroup option').each(function () {
                    var ICDGroupId = $(this).attr("value");
                    Admin_CCMTemplateDetails.icdGroupCheckedIds.push(ICDGroupId);
                });
            }
        }
    },
    UnLoadTab: function () {
        var type = "Care Plan Template"
        if (Admin_CCMTemplateDetails.params.TemplateType && Admin_CCMTemplateDetails.params.TemplateType == "Health Risk Assessment") {
            type = "Health Risk Assessment";
        }
        utility.UnLoadDialog("frmAdminCCMTemplateDetails", function () {
            UnloadActionPan(Admin_CCMTemplateDetails.params["ParentCtrl"], "Admin_CCMTemplateDetails");
            if (type == "Health Risk Assessment") {
                Admin_CCMTemplates.searchHealthRiskAssessment();
            }
            else {
                Admin_CCMTemplates.searchCarePlan();
            }
            Admin_CCMTemplates.searchCarePlan();
        }, function () {
            UnloadActionPan(Admin_CCMTemplateDetails.params["ParentCtrl"], "Admin_CCMTemplateDetails");
            if (type == "Health Risk Assessment") {
                Admin_CCMTemplates.searchHealthRiskAssessment();
            }
            else {
                Admin_CCMTemplates.searchCarePlan();
            }
        });
    },
    removeFromArray: function (array, removeItem) {
        var resultantArray = jQuery.grep(array, function (item) {
            return item != removeItem;
        });
        return resultantArray;
    },
    TemplateDetailsSave: function () {
        var strMessage = "";
        var self = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails");

        var SectionJSON = '{}';
        var TotalSections = 0;
        var TotalQuestions = 0;
        var TotalSubQuestions = 0;
        var isValid = Admin_CCMTemplateDetails.ValidateICDGroup(self);
        /*  self.find('#lstSection li').each(function () {
              TotalSections = TotalSections + 1;
              var liJSON = $(this).getMyJSONByName();
              var jsonArray = JSON.parse(liJSON);
              jsonArray["SectionId" + TotalSections] = $($(this).find('.sectionDiv')).attr("SectionId");
              jsonArray["ShortName" + TotalSections] = $($(this).find('.sectionField')).val();
              liJSON = JSON.stringify(jsonArray);
              SectionJSON = utility.MergeJSON(SectionJSON, liJSON);
          });
          self.find('#lstQuestion li').each(function () {
              TotalQuestions = TotalQuestions + 1;
              var jsonArray = {};
              //toolcontroldiv
              var title = $(this).find('.questionTitle').text();
              jsonArray['QuestionDescription' + TotalQuestions] = title;
              jsonArray['QuestionHTML' + TotalQuestions] = $(this).html();
              jsonArray["QuestSectionId" + TotalQuestions] = $($(this).find('.toolcontroldiv')).attr("SectionId");
              jsonArray["QuestIdForRef" + TotalQuestions] = $($(this).find('.toolcontroldiv')).attr("QuestionId");
              var liJSON = JSON.stringify(jsonArray);
              SectionJSON = utility.MergeJSON(SectionJSON, liJSON);
          });
          self.find('#lstSubQuestion li').each(function () {
              TotalSubQuestions = TotalSubQuestions + 1;
              var jsonArray = {};
              //toolcontroldiv
              var title = $(this).find('.questionTitle').text();
              jsonArray['SubQuestionDescription' + TotalSubQuestions] = title;
              jsonArray['SubQuestionHTML' + TotalSubQuestions] = $(this).html();
              jsonArray["SubQuestIdForRef" + TotalSubQuestions] = $($(this).find('.toolcontroldiv')).attr("ParentQuestId");
              var liJSON = JSON.stringify(jsonArray);

              SectionJSON = utility.MergeJSON(SectionJSON, liJSON);
          });*/
        if (isValid) {
            var myJSON = self.find('#templateDetails').getMyJSONByName();
            myJSON = utility.MergeJSON(myJSON, SectionJSON);
            if (Admin_CCMTemplateDetails.params.mode == "Add") {
                Admin_CCMTemplateDetails.SaveTemplateDetails(myJSON, TotalSections, TotalQuestions, TotalSubQuestions).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var canvasCol = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails #ddlCanvasCol option:selected").val();
                        utility.DisplayMessages(response.Message, 1);
                        self.find('#pnlTemplateArea').removeClass('hidden');
                        Admin_CCMTemplateDetails.initializeDragable();
                        Admin_CCMTemplateDetails.InitializeSortable();
                        Admin_CCMTemplateDetails.params.TemplateId = response.TemplateId;
                        Admin_CCMTemplateDetails.params.mode = "Edit";
                        $('#frmAdminCCMTemplateDetails').data('serialize', $('#frmAdminCCMTemplateDetails').serialize());
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else if (Admin_CCMTemplateDetails.params.mode == "Edit") {
                var selectedSchTypeId = $("#" + Admin_CCMTemplateDetails.params.PanelID + " #frmAdminCCMTemplateDetails #ddlScheduleType option:selected").val();
                Admin_CCMTemplateDetails.UpdateTemplateDetails(myJSON).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        self.find('#pnlTemplateArea').removeClass('hidden');
                        utility.DisplayMessages(response.Message, 1);
                        Admin_CCMTemplateDetails.initializeDragable();
                        Admin_CCMTemplateDetails.InitializeSortable();
                        Admin_CCMTemplateDetails.params.mode = "Edit";
                        $('#frmAdminCCMTemplateDetails').data('serialize', $('#frmAdminCCMTemplateDetails').serialize());
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }

        }
        else {
            utility.DisplayMessages("Please select ICD Group(s).", 3);
        }
    },
    ValidateICDGroup: function (self) {
        var isValid = true;
        if (Admin_CCMTemplateDetails.params.TemplateType && Admin_CCMTemplateDetails.params.TemplateType == "Care Plan Template") {
            $('#' + Admin_CCMTemplateDetails.params.PanelID + " #ddlICDGroup").val() ? $('#' + Admin_CCMTemplateDetails.params.PanelID + " #ddlICDGroup").val().join() : ''
            var icds = self.find('#ddlICDGroup option:Selected').map(function () {
                return this.value;
            }).get().join(',');
            if (icds == "")
                isValid = false;
        }
        return isValid;
    },
    ValidateTemplateDetails: function () {
        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #frmAdminCCMTemplateDetails')
        .bootstrapValidator('destroy');
        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #frmAdminCCMTemplateDetails')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   TemplateTitle: {
                       group: '.col-sm-4',
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
            Admin_CCMTemplateDetails.TemplateDetailsSave();
        });
    },
    SaveTemplateDetails: function (jsonData, TotalSections, TotalQuestions, TotalSubQuestions) {
        var objData = JSON.parse(jsonData);
        objData["TemplateId"] = 0;
        objData["TotalSections"] = TotalSections;
        objData['TotalQuestions'] = TotalQuestions;
        objData['TotalSubQuestions'] = TotalSubQuestions;
        objData['TempLookupId'] = 1;
        if (Admin_CCMTemplateDetails.params.TemplateType && Admin_CCMTemplateDetails.params.TemplateType != "Care Plan Template")
            objData['TempLookupId'] = 2;
        objData["icdsGroupIds"] = $('#' + Admin_CCMTemplateDetails.params.PanelID + " #ddlICDGroup").val() ? $('#' + Admin_CCMTemplateDetails.params.PanelID + " #ddlICDGroup").val().join() : '';
        objData["commandType"] = "SAVE_CCM_TEMPLATE";
        //var data = "commandType='SAVE_CCM_TEMPLATE'&CCMTemplateData=" + jsonData + "&TotalSections=" + TotalSections + "&TotalQuestions=" + TotalQuestions + "&TotalSubQuestions=" + TotalSubQuestions + "&TemplateId=" + TemplateId + "&ICDGroupIds=" + icdsGroupIds;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
    },
    UpdateTemplateDetails: function (jsonData) {
        var objData = JSON.parse(jsonData);
        objData["TemplateId"] = 0;
        objData["TotalSections"] = 0;
        objData['TotalQuestions'] = 0;
        objData['TotalSubQuestions'] = 0;
        objData['TempLookupId'] = Admin_CCMTemplateDetails.params.TemplateId;
        if (Admin_CCMTemplateDetails.params.TemplateType && Admin_CCMTemplateDetails.params.TemplateType != "Care Plan Template")
            objData['TempLookupId'] = 2;
        objData["icdsGroupIds"] = $('#' + Admin_CCMTemplateDetails.params.PanelID + " #ddlICDGroup").val() ? $('#' + Admin_CCMTemplateDetails.params.PanelID + " #ddlICDGroup").val().join() : '';
        objData["commandType"] = "UPDATE_CCM_TEMPLATE";
        objData["TemplateId"] = Admin_CCMTemplateDetails.params.TemplateId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
    },
    /*Initilize Quiestion to be drageable*/
    initializeDragable: function () {
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #frmAdminCCMTemplateDetails #toolbarContainer li').draggable({
            revert: "invalid",
            zIndex: 100,
            stack: ".dragableQest",
            cursor: "move",
            connectToSortable: '.dragableQest',
            helper: function (ev, ui) {
                var toolId = $(this).attr('id');
                var questionId = -1;
                questionId = Admin_CCMTemplateDetails.questionId;
                Admin_CCMTemplateDetails.questionId = Admin_CCMTemplateDetails.questionId - 1;
                var ctrlHtml = Admin_CCMTemplateDetails.getDefaultHTMLOfControl(toolId, questionId);
                var lstToAppend = '<li class="draggable mb-sm">' + ctrlHtml + '</li>';
                return lstToAppend;
            },
            stop: function (ev, ui) {
                $(ui.helper).removeAttr('style');
                $(ui.helper).addClass('col-xs-12');
                var questionId = $($(ui.helper).find('Div.toolcontroldiv')).attr('QuestionId');
                var parent = $(ui.helper).parent();
                var self = $('#' + Admin_CCMTemplateDetails.params.PanelID + ' #frmAdminCCMTemplateDetails');
                if (parent && $(parent).attr('id') == "lstQuestion") {
                    var lstQuestion = self.find('#lstQuestion');
                    var selectedSection = Admin_CCMTemplateDetails.getSelectedSection();
                    self.find('#lstSubQuestion').empty();
                    if (selectedSection) {
                        setTimeout(function () {
                            if ($(parent).find('Div[QuestionId="' + questionId + '"]').length > 0) {
                                if (lstQuestion) {
                                    $(lstQuestion).find("input[type='checkbox']").prop('checked', false);
                                }
                                $(ui.helper).find("#actionChkBox").prop('checked', true);
                                $(ui.helper).find("#actionChkBox").attr('checked', 'checked');
                                self.find('#txtQuestionHeader').removeClass('hidden');
                                self.find('#txtQuestionHeader').val($(selectedSection).find('.sectionField').val());
                                $($(ui.helper).find('Div.toolcontroldiv')).attr('SectionId', $(selectedSection.find('.sectionDiv')).attr('SectionId'));
                                Admin_CCMTemplateDetails.updateQuestionHTML($(ui.helper));
                                Admin_CCMTemplateDetails.setHeightOfUl(parent);
                                self.find('#txtSubQuestionHeader').removeClass('hidden');
                                self.find('#txtSubQuestionHeader').val($($(ui.helper).find('.questionTitle')).text().replace('*', ''));
                            }
                        }, 300)
                    }
                    else {
                        utility.DisplayMessages('Please Add/Select Section first', 2);
                        $(ui.helper).remove();
                        if (!self.find('#txtQuestionHeader').hasClass('hidden'))
                            self.find('#txtQuestionHeader').addClass('hidden');
                    }
                }
                else if (parent && $(parent).attr('id') == "lstSubQuestion") {
                    $(ui.helper).find("input[type='checkbox']").remove();
                    var selectedQuestion = Admin_CCMTemplateDetails.getSelectedQuestion();
                    if (selectedQuestion) {
                        setTimeout(function () {
                            if ($(parent).find('Div[QuestionId="' + questionId + '"]').length > 0) {
                                var lblTitle = $(selectedQuestion).find('.questionTitle');
                                self.find('#txtSubQuestionHeader').removeClass('hidden');
                                if (lblTitle)
                                    self.find('#txtSubQuestionHeader').val($(lblTitle).text().replace('*', ''));
                                else
                                    self.find('#txtSubQuestionHeader').val('');
                                $($(ui.helper).find('Div.toolcontroldiv')).attr('ParentQuestId', $(selectedQuestion).attr('QuestionId'));
                                Admin_CCMTemplateDetails.updateQuestionHTML($(ui.helper));
                                Admin_CCMTemplateDetails.setHeightOfUl(parent);
                            }
                        }, 300)
                    }
                    else {
                        utility.DisplayMessages('Please Add/Select Question first', 2);
                        $(ui.helper).remove();
                        if (!self.find('#txtSubQuestionHeader').hasClass('hidden'))
                            self.find('#txtSubQuestionHeader').addClass('hidden');
                    }
                }
            },
        });
    },
    /*To Genrate HTML of draged Question*/
    getDefaultHTMLOfControl: function (ctrl, questionId) {
        var ctrlHtml = '';
        var uniquId = utility.makeRendomKey();
        var templateId = Admin_CCMTemplateDetails.params.TemplateId == null ? "" : Admin_CCMTemplateDetails.params.TemplateId;
        var actionDiv = '<div id="actionDiv" class="col-xs-1"><a class="btn btn-xs pull-left" href="#" onclick="Admin_CCMTemplateDetails.RemoveQuestion(this,event);" title="Delete Record">'
              + '<i class="fa fa-close red"></i></a>&nbsp;'
              + '<input type="checkbox" id="actionChkBox" class="pull-left" onclick="Admin_CCMTemplateDetails.SetSelectedQuestion(this);" name="checkbox" checked="checked"/>&nbsp;'
              + '<a class="btn btn-xs pull-left" href="#" onclick="Admin_CCMTemplateDetails.EditQuestion(this,event);" title="Edit Question"><i class="fa fa-pencil black"></i></a>'
              + '</div>';
        switch (ctrl) {
            case "toolTextField":
                ctrlHtml = '<div  class="panel-body toolcontroldiv col-xs-12" questionid="' + questionId + '" QuestionType="TextField"  isnumber="false" isnewline="false" ismandatory="false" questiontitle="Select Label" issingleline="true" maxlength="" textcase="" defaultvalue="Text Field" id="toolTextField_' + templateId + '_' + uniquId + '">'
                 + actionDiv
                 + '<div class="controlContainerDiv col-xs-11">'
                 + '<div class="col-xs-5 pull-left">'
                 + '<label class="control-label size-max100per resetLineHeight questionTitle"for="toolTextField" id="lblTextField" data-toggle="tooltip" title="Select Label">Select Label</label>'
                   + '</div>'
                   + '<div class="col-xs-5 pull-right">'
                 + '<input class="form-control " name="TextField" id="txtTextField" type="text" value="Text Field">'
                   + '</div>'
                 + '</div>'
                 + '</div>';
                break;
            case "toolCheckBox":
                ctrlHtml = '<div  class="panel-body toolcontroldiv pad-a-labelsize col-xs-12" questionid="' + questionId + '" questiontype="CheckBox"  isnewline="false" questionlabel="Label 1" selectionmode="0" defaultselection="" id="toolCheckBox_' + templateId + uniquId + '">'
                  + actionDiv
                  + '<div class="controlContainerDiv p-none col-xs-11">'
                  + '<div class="col-sm-5 pull-left">'
                  + '<label id="lblCheckBoxTitle" class="control-label size-max100per resetLineHeight questionTitle"for="toolCheckBox" data-toggle="tooltip" title="Select Label">Select Label</label>'
                  + '</div>'
                  + '<div id="TemplateCheckBoxPreview"class="checkbox-custom checkbox-default pull-right col-sm-5">'
                  + '<input type="checkbox" id="toolCheckBox_">'
                  + '<label class="control-label size-max100per resetLineHeight" for="Active">Label Name</label>'
                  + '</div>'
                    + '</div>'
                  + '</div>';
                break;
            case "toolYesNo":
                ctrlHtml = '<div  class="panel-body toolcontroldiv pad-a-labelsize col-xs-12" questionid="' + questionId + '" questiontype="YesNo"  isnewline="false" ismandatory="false" questionlabel="Select Label" defaultselection="0" id="toolYesNo_' + templateId + uniquId + '">'
                 + actionDiv
                 + '<div class="controlContainerDiv p-none col-xs-11">'
                  + '<div class="col-sm-5 pull-left">'
                 + '<label id="TemplateYesNoLabel" class="control-label size-max100per resetLineHeight questionTitle" for="toolYesNo" data-toggle="tooltip" title="Select Label">Select Label</label>'
                 + '</div>'
                 + '<div id="TemplateYesNoPreview"  class="col-sm-5 p-none">'
                 + '<div class="checkbox-custom checkbox-default pull-left">'
                 + '<input type="checkbox" id="chkYes" onchange="GlobalQuestionDetail.YesOrNo(this);" name="Yes">'
                 + '<label class="control-label size-max100per resetLineHeight" for="Yes">Yes</label>'
                 + '</div>'
                 + '<div class="checkbox-custom checkbox-default pull-right">'
                 + '<input type="checkbox" name="No" onchange="GlobalQuestionDetail.YesOrNo(this);" id="chkNo">'
                 + '<label class="control-label size-max100per resetLineHeight" for="No">No</label>'
                 + '</div>'
                 + '</div>'
                   + '</div>'
                 + '</div>';
                break;
            case "toolSingleSelectDropdown":
                ctrlHtml = '<div  class="panel-body toolcontroldiv col-xs-12" questionid="' + questionId + '" questiontype="SingleSelectDropdown"  isnewline="false" ismandatory="false" questionlabel="Select Label" defaultselection="0" dropdownvalues="" id="toolSingleSelectDropdown_' + templateId + uniquId + '">'
                 + actionDiv
                 + '<div class="controlContainerDiv  col-xs-11">'
                  + '<div class="col-sm-5 pull-left">'
                 + '<label id="TemplateSingleSelectDropdownLabel" class="control-label size-max100per resetLineHeight questionTitle" for="TextField" data-toggle="tooltip" title="Select Label">Select Label</label>'
                 + '</div>'
                  + '<div class="col-sm-5 pull-right">'
                 + '<div id="TemplateSingleSelectDropdownList"> <select class="form-control" name="CanvasCol" id="toolSingleSelectDropdown_"><option val="0">Single Select Dropdown</option></select></div>'
                 + '</div>'
                   + '</div>'
                 + '</div>';
                break;
            case "toolMultipleSelectCombo":
                ctrlHtml = '<div  class="panel-body toolcontroldiv col-xs-12" questionid="' + questionId + '" questiontype="MultipleSelectCombo"  isnewline="false" ismandatory="false" questionlabel="Select Label" defaultselection="" dropdownvalues=""  id="toolMultipleSelectCombo_' + templateId + uniquId + '">'
                  + actionDiv
                  + '<div class="controlContainerDiv col-xs-11">'
                  + '<div class="col-sm-5 pull-left">'
                  + '<label id="TemplateMultipleSelectComboLabel" class="control-label size-max100per resetLineHeight questionTitle" for="TextField" data-toggle="tooltip" title="Select Label">Select Label</label>'
                  + '</div>'
                   + '<div class="col-sm-5 pull-right">'
                 + '<div id="customFormMultipleSelectCombo_' + uniquId + '"> <select class="form-control" id="toolSingleSelectDropdown_"><option val="0">Multiple Select Combo</option></select></div>'
                 + '</div>'
                    + '</div>'
                  + '</div>';
                break;
            case "toolImage":
                ctrlHtml = '<div  class="panel-body toolcontroldiv Of-a col-xs-12" questionid="' + questionId + '"  questiontype="Image" questionlabel="Select Label" filename="" id="toolImage_' + templateId + '_' + uniquId + '">'
                 + actionDiv
                 + '<div class="controlContainerDiv col-xs-11">'
                  + '<div class="col-sm-5 pull-left">'
                 + '<label id="TemplateImageLabel" class="control-label size-max100per resetLineHeight questionTitle" for="TextField" data-toggle="tooltip" title="Select Label">Select Label</label>'
                   + '</div>'
                  + '<div class="col-sm-5 pull-right">'
                 + '<div id="TemplateImage"> <input class="form-control" name="TextField" id="toolImage_" type="" value="Browse Image"> </div>'
                   + '</div>'
                     + '</div>'
                 + '</div>';
                break;
            case "toolFractionField":
                ctrlHtml = '<div  class="panel-body toolcontroldiv pl-xs pb-xs col-xs-12"  questionid="' + questionId + '" questionType="FractionField" isnewline="false" ismandatory="false" id="toolFractionField_' + templateId + uniquId + '">'
                + actionDiv
                + '<div class="controlContainerDiv col-xs-11">'
                         + '<div class="col-xs-5 pull-Left" style="margin-bottom: -8px;">'
                         + '<label class="control-label size-max100per resetLineHeight questionTitle" for="TextField" id="lblFractionTitle" data-toggle="tooltip" title="Title">Title</label>'
                         + '</div>'
                           + '<div class="col-sm-5 p-none pull-right" id="TemplateFraction">'
                         + '<div class="col-xs-4">'
                             + '<label class="control-label" for="txtFractionField1" id="lblFractionField1">Label 1</label>'
                             + '<input class="form-control" name="FractionField1" id="txtFractionField1" type="text">'
                         + '</div>'
                     + '<div class="col-xs-2 text-center pad-a-labelsize">/</div>'
                          + '<div class="col-xs-4">'
                             + '<label class="control-label" for="txtFractionField2" id="lblFractionField2">Label 2</label>'
                             + '<input class="form-control" name="FractionField2" id="txtFractionField2" type="text">'
                         + '</div>'
                         + '</div>'
                  + '</div>'
                 + '</div>';
                break;
            case "toolFreeText":
                ctrlHtml = '<div  class="panel-body toolcontroldiv col-xs-12" questionid="' + questionId + '" questionType="FreeText" textcase="" maxlength="" id="toolFreeText_' + templateId + '_' + uniquId + '" >'
                + actionDiv
                 + '<div class="controlContainerDiv p-none  col-xs-11">'
                   + '<div class="col-sm-5 pull-left">'
                 + '<label class="control-label size-max100per resetLineHeight questionTitle" for="FreeText" id="lblFreeText" data-toggle="tooltip" title="Text Field">Text Field</label>'
                 + '</div>'
                   + '<div class="col-sm-5 pull-right">'
                 + '<textarea id="txtFreeText" spellcheck="true" onchange="Admin_CCMTemplateDetails.updateTextFieldVal(this);" style="width: 100%;-webkit-box-sizing: border-box; -moz-box-sizing: border-box;box-sizing: border-box; ">Add Free Text Here</textarea>'
                 + '</div>'
                 + '</div>'
                 + '</div>';
                break;
            case "toolDateField":
                ctrlHtml = '<div  class="panel-body toolcontroldiv col-xs-12" QuestionType="DateField" questionid="' + questionId + '" isnewline="false" defaultdate="" ismandatory="false" dateformat="" id="toolDateField_' + templateId + '_' + uniquId + '" >'
                 + actionDiv
                 + '<div class="controlContainerDiv col-xs-11">'
                   + '<div class="col-sm-5 pull-left">'
                 + '<label class="control-label size-max100per resetLineHeight questionTitle" for="TextField" id="lblDateFieldTitle" data-toggle="tooltip" title="Text Field">Text Field</label>'
                 + '</div>'
                 + '<div class="col-sm-5 pull-right">'
                 + ' <div class="input-group pull-left">'
                 + ' <span class="input-group-addon"> <i class="fa fa-calendar"></i> </span>'
                 + '<input id="dtpDateField" name="dtpDateField" class="form-control dateField" type="text" data-plugin-datepicker="" maxlength="10" dateformat="">'
                 + '</div>'
                 + '</div>'
                 + '</div>';
                break;
            case "toolTimeField":
                ctrlHtml = '<div  class="panel-body toolcontroldiv col-xs-12" QuestionType="TimeField" questionid="' + questionId + '" isnewline="false" defaulttime=""  ismandatory="false" timeformat="24" id="toolTimeField_' + templateId + '_' + uniquId + '" >'
                 + actionDiv
                 + '<div class="controlContainerDiv col-xs-11">'
                   + '<div class="col-sm-5 pull-left">'
                 + '<label class="control-label size-max100per resetLineHeight questionTitle" for="TextField" id="lblTimeFieldTitle" data-toggle="tooltip" title="Text Field">Select Label</label>'
                  + '</div>'
                  + '<div class="col-sm-5 pull-right">'
                 + ' <div class="input-group pull-left">'
                 + ' <span class="input-group-addon"> <i class="fa fa-clock-o"></i> </span>'
                 + '<input id="dtpTimeField" name="dtpDateField" class="form-control timeField" type="text" data-plugin-timepicker maxlength="10" timeformat="">'
                 + '</div>'
                  + '</div>'
                 + '</div>';
                break;
            default:
                break;

        }
        return ctrlHtml;
    },
    updateTextFieldVal: function (obj) {
        if ($(obj).is("textarea")) {
            $(obj).attr('value', $(obj).val());
            $(obj).text($(obj).val());
        } 
    },
    updateTemplateHTML: function () {
        /*var strMessage = "";
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if (Admin_CCMTemplateDetails.params.mode == "Edit") {
            var self = $("#" + Admin_CCMTemplateDetails.params["PanelID"] + " #frmAdminCCMTemplateDetails");
            var myJSON = self.getMyJSONByName();
            Admin_CCMTemplateDetails.UpdateTemplateDetails(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        */
    },
    InitializeSortable: function () {
        $(".sortableQuest").sortable({
            connectWith: ".sortableQuest",
            placeholder: "dragArea col-xs-12",
            start: function (e, ui) {
                //ui.placeholder.width(ui.item.width());
                ui.placeholder.height(40);
            },
            stop: function (e, ui) {
                Admin_CCMTemplateDetails.updateQuestionOrderNo(false)
            },
        }).disableSelection();;
        $(".sortableSubQuest").sortable({
            connectWith: ".sortableSubQuest",
            placeholder: "dragArea col-xs-12",
            start: function (e, ui) {
                //ui.placeholder.width(ui.item.width());
                ui.placeholder.height(40);
            },
            stop: function (e, ui) {
                Admin_CCMTemplateDetails.updateQuestionOrderNo(true)
            },
        }).disableSelection();;

    },
    //This functions removed Component from Progress Note HTML
    RemoveComponentTab: function (ComponentName, ComponentId, NotesId) {
        utility.myConfirm('1', function () {
        }, function () { },
            '1'
        );

    },
    EditQuestion: function (obj, event, selectedLi) {
        if (event != null) {
            //Prevent click event in case of toogle header/dropdown menu/context menu clicks
            if ($(event.target).attr('id') == "lblQuestionGroupTitle" || $(event.target).hasClass('toggleEditableHeader') || $(event.target).hasClass('dropdown-toggle') || $(event.target).hasClass('tdcontextTable')) {
                return;
            }
            else {
                event.stopPropagation();
            }
        }
        var toolcontroldiv;
        var questionType;
        var questionId;
        var QuestionGroupId;
        if (selectedLi) {

            toolcontroldiv;
            questionType = $(selectedLi.find('input').val()).attr('questiontype');
            questionId = $(selectedLi.find('input').val()).attr('id');
            QuestionGroupId = $(selectedLi.find('input').val()).attr('QuestionGroupId');
            Admin_CCMTemplateDetails.QuestionEdit(questionType, questionId, QuestionGroupId, true);
        }
        else {
            toolcontroldiv = $(obj).closest('div.toolcontroldiv');
            questionType = $(toolcontroldiv).attr('questionType');
            questionId = $(toolcontroldiv).attr('id');
            QuestionGroupId = $(toolcontroldiv).attr('QuestionGroupId');
            Admin_CCMTemplateDetails.QuestionEdit(questionType, questionId, QuestionGroupId, false);
        }
    },
    QuestionEdit: function (questionType, questionId, QuestionGroupId, fromQuestionGroup) {
        if (questionType == "TextField" || questionType == "YesNo" || questionType == "FractionField" || questionType == "DateField" || questionType == "TimeField" || questionType == "CheckBox" || questionType == "FreeText" || questionType == "SingleSelectDropdown" || questionType == "MultipleSelectCombo" || questionType == "Image") {
            var params = [];
            params["QuestionType"] = questionType;
            params["QuestionID"] = questionId;
            params["mode"] = 'Edit';
            //params["FromAdmin"] = Admin_CCMTemplateDetails.params["FromAdmin"];
            params["FromQuestionGroup"] = fromQuestionGroup;
            //if (Admin_CCMTemplateDetails.params["FromAdmin"] == "0") {
            params["ParentCtrl"] = 'Admin_CCMTemplateDetails';
            //}
            LoadActionPan('Admin_CCMQuestionDetails', params);
        }
    },
    SetSelectedQuestion: function (obj, event, selectedLi) {
        if (event != null) {
            //Prevent click event in case of toogle header/dropdown menu/context menu clicks
            if ($(event.target).attr('id') == "lblQuestionGroupTitle" || $(event.target).hasClass('toggleEditableHeader') || $(event.target).hasClass('dropdown-toggle') || $(event.target).hasClass('tdcontextTable')) {
                return;
            }
            else {
                event.stopPropagation();
            }
        }
        var lstQuest;
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        self.find('#txtSubQuestionHeader').val("");
        self.find('#lstSubQuestion').empty();
        lstQuest = $(obj).closest('ul.list-unstyled');
        var toolcontroldiv = $(obj).closest('Div.toolcontroldiv');
        var isChecked = $(obj).is(':checked');
        $(lstQuest).find("input[type='checkbox']").prop('checked', false);
        if (isChecked) {
            $(obj).prop('checked', true);
            $(obj).attr('checked', 'checked');
            Admin_CCMTemplateDetails.FillSubQuestions($(obj).closest('li'));
        }
        else {
            $(obj).prop('checked', false);
            $(obj).removeAttr('checked');
        }
        Admin_CCMTemplateDetails.setSubQuestioHeader();
    },
    addSection: function () {
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        self.find('#lstQuestion').empty();
        self.find('#txtQuestionHeader').val("");
        self.find('#lstSubQuestion').empty();
        self.find('#txtSubQuestionHeader').val("");
        if (!self.find('#txtSubQuestionHeader').hasClass('hidden'))
            self.find('#txtSubQuestionHeader').addClass('hidden');
        var lstSection = self.find('#lstSection');
        if (lstSection) {
            var uniquId = utility.makeRendomKey();
            $(lstSection).find("input[type='checkbox']").prop('checked', false);
            $(lstSection).find("li").removeClass('active');
            var templateId = Admin_CCMTemplateDetails.params.TemplateId == null ? "" : Admin_CCMTemplateDetails.params.TemplateId;
            var actionDiv = '<div class="col-xs-1 p-none">'
                + '<div class="form-control pt-none pb-none">'
                + '<a class="btn btn-xs pull-left" href="#" onclick="Admin_CCMTemplateDetails.RemoveSection(this,event);" title="Delete Record">'
                  + '<i class="fa fa-close red"></i></a>'
                  + '<input type="checkbox" class=" pull-left" onclick="Admin_CCMTemplateDetails.SetSelectedSection(this,event);" name="checkbox" id="secChkb_' + templateId + '_' + uniquId + '" checked="checked"/>'
                  + '<a class="btn btn-xs  pull-left" href="#" onclick="Admin_CCMTemplateDetails.enableSection(this,event,true);" title="Edit Question"><i class="fa fa-pencil black"></i></a>'
                  + '</div>'
                  + '</div>';
            ctrlHtml = '<div  class="p-none sectionDiv col-xs-12" name="SectionId" sectionId="' + Admin_CCMTemplateDetails.sectionId + '" id="section_' + templateId + '_' + uniquId + '">'
                + actionDiv
                + '<div class="col-xs-11 pr-none">'
                + '<input class="form-control sectionField" maxLength="55" name="ShortName" id="sectionField_' + templateId + '_' + uniquId + '"type="text" onblur="Admin_CCMTemplateDetails.disableSection(this,event,false);" >'
                + '</div>'
                + '</div>';
            lstSection.prepend('<li class="col-xs-12 mb-xs active">' + ctrlHtml + '</li>');
            var section = 'section_' + templateId + '_' + uniquId;
            Admin_CCMTemplateDetails.sectionId = Admin_CCMTemplateDetails.sectionId - 1;
            Admin_CCMTemplateDetails.updateSectionHTML($("#section_" + templateId + "_" + uniquId).closest('li'));
        }
    },
    disableSection: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if ($(obj).val() != "") {
            if (!$(obj).hasClass('disableAll'))
                $(obj).addClass('disableAll');
            Admin_CCMTemplateDetails.updateSectionHTML(obj);
            $(obj).css("border", "");
        }
        else {
            utility.DisplayMessages("Section is required.", 2);
            $(obj).css("border", "1px solid red");
            $(obj).focus();
        }
    },
    enableSection: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var sectionField = $(obj).closest('Div.sectionDiv').find('.sectionField');
        if (sectionField) {
            if ($(sectionField).hasClass('disableAll')) {
                $(sectionField).removeClass('disableAll')
            }
            sectionField.focus();
        }
    },
    SetSelectedSection: function (obj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        var lstSection = self.find('#lstSection');
        var isChecked = $(obj).is(':checked');
        $(lstSection).find("input[type='checkbox']").prop('checked', false);
        $(lstSection).find("li").removeClass('active');
        self.find('#lstQuestion').empty();
        self.find('#lstSubQuestion').empty();
        self.find('#txtSubQuestionHeader').val("");
        if (isChecked) {
            $(obj).prop('checked', true);
            $(obj).attr('checked', 'checked');
            var objLi = $(obj).closest('li');
            if (!$(objLi).hasClass('active'))
                $(objLi).addClass('active');
            Admin_CCMTemplateDetails.FillSectionQustions($(objLi));
        }
        else {
            $(obj).prop('checked', false);
            $(obj).removeAttr('checked');
        }
    },
    updateQuestionHeader: function () { },
    updateSubQuestionHeader: function () { },
    getSelectedSection: function () {
        var selectedSection = null;
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        var lstSection = self.find('#lstSection');
        $(lstSection).find("input[type='checkbox']").filter(function (index) {
            if ($(this).prop('checked')) {
                selectedSection = $(this).closest('li');
            }
        });
        return selectedSection;
    },
    getSelectedQuestion: function () {
        var selectedQuestion = null;
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        var lstQuestion = self.find('#lstQuestion');

        var Visits = [];
        $(lstQuestion).find("input[type='checkbox']").filter(function (index) {
            if ($(this).prop('checked')) {
                selectedQuestion = $(this).closest('Div.toolcontroldiv');
            }
        });
        return selectedQuestion;
    },
    SubQuestionSelect_DBCall: function (QuestionId) {
        if (QuestionId && parseInt(QuestionId) > 0) {
            var objData = new Object();
            objData["commandType"] = "SELECT_CCM_SUB_QUESTS";
            objData["QuestionId"] = QuestionId;
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
        }
    },
    SectionQuestionSelect_DBCall: function (SectionId) {
        if (SectionId && parseInt(SectionId) > 0) {
            var objData = new Object();
            objData["commandType"] = "SELECT_CCM_SECT_QUESTS";
            objData["SectionId"] = SectionId;
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
        }
    },
    RemoveSection: function (obj, event) {
        if (event != null)
            event.stopPropagation();
        utility.myConfirm('1', function () {
            if (obj) {
                var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
                var containerWdgt = $(obj).closest('li');
                var sectionId = -1;
                var sectionHtm = $(containerWdgt).html().trim();
                if ($(sectionHtm).attr("SectionId"))
                    sectionId = parseInt($(sectionHtm).attr("SectionId"));
                if (sectionId && sectionId > 0) {
                    Admin_CCMTemplateDetails.deleteSection_DBCall(sectionId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            $(containerWdgt).remove();
                            $.each(self.find("Div[sectionid='" + sectionId + "']"), function () {
                                self.find("Div[ParentQuestId='" + $(this).attr("QuestionId") + "']").remove();
                                $(this).remove();
                            });
                            Admin_CCMTemplateDetails.setQuestioHeader();
                            Admin_CCMTemplateDetails.setSubQuestioHeader();
                            Admin_CCMTemplateDetails.enablDisablePreview();
                            Admin_CCMTemplateDetails.setHeightOfUl(self.find('#lstQuestion'));
                            Admin_CCMTemplateDetails.setHeightOfUl(self.find('#lstSubQuestion'));
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    $(containerWdgt).remove();
                    self.find("Div[sectionid='" + sectionId + "']").remove();
                    Admin_CCMTemplateDetails.enablDisablePreview();
                    Admin_CCMTemplateDetails.setHeightOfUl(self.find('#lstQuestion'));
                    Admin_CCMTemplateDetails.setHeightOfUl(self.find('#lstSubQuestion'));
                }
            }
        },
        function () { },
        '1'
        );
    },
    deleteSection_DBCall: function (SectionId) {
        if (SectionId && parseInt(SectionId) > 0) {
            var objData = new Object();
            objData["commandType"] = "DELETE_CCM_SECTION";
            objData["SectionId"] = SectionId;
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
        }
    },
    previewTemplate: function () {
        var isValidPreview = true;
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        $(self.find("#lstSection li")).each(function () {
            var txt = $(this).find('.sectionField')
            if ($(txt).val() != "") {
                if (!$(txt).hasClass('disableAll'))
                    $(txt).addClass('disableAll');
                $(txt).css("border", "");
            }
            else {
                $(txt).css("border", "1px solid red");
                $(txt).focus();
                isValidPreview = false;
                return false;
            }
        });
        if (isValidPreview) {
            var params = [];
            params["TemplateName"] = $('#txtTemplateTitle').val();
            params["ParentCtrl"] = 'Admin_CCMTemplateDetails';
            LoadActionPan('Admin_CCMTemplatePreview', params);
        }
        else {
            utility.DisplayMessages("Section is required.", 2);
        }
    },
    updateSectionHTML: function (obj) {
        var strMessage = "";
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        // var selectedSection = Admin_CCMTemplateDetails.getSelectedSection();
        var jsonArray = {};
        if ($(obj).closest('li'))
            obj = $(obj).closest('li');
        jsonArray["SectionId"] = $($(obj).find('.sectionDiv')).attr("SectionId");
        jsonArray["ShortName"] = $($(obj).find('.sectionField')).val();
        jsonArray["TemplateId"] = Admin_CCMTemplateDetails.params.TemplateId;
        var myJSON = JSON.stringify(jsonArray);
        Admin_CCMTemplateDetails.SaveSectionDB_Call(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                $($(obj).find('.sectionDiv')).attr("SectionId", response.SectionId);
                if ($(obj).find('.sectionField') && $(obj).find('.sectionField').val() == "")
                    $($(obj).find('.sectionField')).focus();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        Admin_CCMTemplateDetails.setQuestioHeader();
        Admin_CCMTemplateDetails.enablDisablePreview();
    },
    setQuestioHeader: function () {
        var selectedSection = Admin_CCMTemplateDetails.getSelectedSection();
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        if (selectedSection) {
            var sectionName = $($(selectedSection).find('.sectionField')).val();
            self.find('#txtQuestionHeader').removeClass('hidden');
            self.find('#txtQuestionHeader').val(sectionName);
        }
        else {
            if (!self.find('#txtQuestionHeader').hasClass('hidden'))
                self.find('#txtQuestionHeader').addClass('hidden');
            self.find('#txtQuestionHeader').val("");
        }
    },
    setSubQuestioHeader: function () {
        var selectedQuestion = Admin_CCMTemplateDetails.getSelectedQuestion();
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        if (selectedQuestion) {
            var questionName = $($(selectedQuestion).find('.questionTitle')).text().replace('*', '');
            self.find('#txtSubQuestionHeader').removeClass('hidden');
            self.find('#txtSubQuestionHeader').val(questionName);
        }
        else {
            if (!self.find('#txtSubQuestionHeader').hasClass('hidden'))
                self.find('#txtSubQuestionHeader').addClass('hidden');
            self.find('#txtSubQuestionHeader').val("");
        }
    },
    SaveSectionDB_Call: function (jsonData) {
        var objData = JSON.parse(jsonData);
        objData["commandType"] = "SAVE_CCM_SECTION";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
    },
    updateQuestionHTML: function (obj) {
        var strMessage = "";
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        var selectedSection = Admin_CCMTemplateDetails.getSelectedSection();
        var jsonArray = {};
        //toolcontroldiv
        var title = $(obj).find('.questionTitle').text();
        jsonArray['QuestionDescription'] = title;
        jsonArray['QuestionId'] = $($(obj).find('.toolcontroldiv')).attr("QuestionId");
        jsonArray['QuestionHTML'] = $(obj).html();
        //jsonArray['OrderNo'] = $(obj).index();
        var isSubQuestion = false;
        //  if (isSubQuestion) {
        var parentQuestId = 0;
        if ($($(obj).find('.toolcontroldiv')).attr("ParentQuestId")) {
            jsonArray["ParentQuestId"] = $($(obj).find('.toolcontroldiv')).attr("ParentQuestId");
            isSubQuestion = true;
            parentQuestId = $($(obj).find('.toolcontroldiv')).attr("ParentQuestId");
            jsonArray["SectionId"] = "";
        }
        else {
            jsonArray["ParentQuestId"] = "";
            jsonArray["SectionId"] = $($(selectedSection).find('.sectionDiv')).attr("SectionId");
        }
        var myJSON = JSON.stringify(jsonArray);
        Admin_CCMTemplateDetails.SaveQuestionDB_Call(myJSON).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                $($(obj).find('.toolcontroldiv')).attr("QuestionId", response.QuestionId);
                if (!isSubQuestion) {
                    var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
                    if (self.find('Div[ParentQuestId="' + parentQuestId + '"]'))
                        self.find('Div[ParentQuestId="' + parentQuestId + '"]').prop('ParentQuestId', response.QuestionId);
                    Admin_CCMTemplateDetails.updateQuestionOrderNo(false);
                }
                else
                    Admin_CCMTemplateDetails.updateQuestionOrderNo(true);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        Admin_CCMTemplateDetails.setSubQuestioHeader();
    },
    SaveQuestionDB_Call: function (jsonData) {
        var objData = JSON.parse(jsonData);
        objData["commandType"] = "SAVE_CCM_QUESTION";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
    },
    FillSectionQustions: function (objLi) {
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        self.find('#txtQuestionHeader').val($(objLi).find('.sectionField').val());
        if (!self.find('#txtSubQuestionHeader').hasClass('hidden'))
            self.find('#txtSubQuestionHeader').addClass('hidden');
        if (objLi) {
            var SectionId = $(objLi).find('.sectionDiv').attr('SectionId');
            Admin_CCMTemplateDetails.SectionQuestionSelect_DBCall(SectionId).done(function (response) {
                response = JSON.parse(response);
                var Questions_JSON = response.Questions_JSON;
                Admin_CCMTemplateDetails.QuestionsFillData(Questions_JSON, SectionId);
            });
        }
    },
    FillSubQuestions: function (objLi) {
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        self.find('#lstSubQuestion').empty();
        self.find('#txtSubQuestionHeader').val($(objLi).find('.questionTitle').text().replace('*', ''));
        if (objLi) {
            QuestionId = $(objLi).find('.toolcontroldiv').attr('QuestionId');
            Admin_CCMTemplateDetails.SubQuestionSelect_DBCall(QuestionId).done(function (response) {
                response = JSON.parse(response);
                var SubQuestions_JSON = response.SubQuestions_JSON
                Admin_CCMTemplateDetails.SubQuestionsFillData(SubQuestions_JSON, QuestionId, self);
            });
        }
    },
    RemoveQuestion: function (obj, event) {
        if (event != null)
            event.stopPropagation();
        utility.myConfirm('1', function () {
            if (obj) {
                var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
                var containerWdgt = $(obj).closest('li');
                var questionId = -1;
                var questionHtm = $(containerWdgt).html().trim();
                if ($(questionHtm).attr("questionid"))
                    questionId = parseInt($(questionHtm).attr("questionid"));
                if (questionId && questionId > 0) {
                    Admin_CCMTemplateDetails.deleteQuestion_DBCall(questionId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            $(containerWdgt).remove();
                            self.find("Div[ParentQuestId='" + questionId + "']").remove();
                            Admin_CCMTemplateDetails.setQuestioHeader();
                            Admin_CCMTemplateDetails.setSubQuestioHeader();
                            Admin_CCMTemplateDetails.setHeightOfUl($(containerWdgt).closest('ul'));
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    $(containerWdgt).remove();
                    self.find("Div[ParentQuestId='" + questionId + "']").remove();
                    Admin_CCMTemplateDetails.setHeightOfUl($(containerWdgt).closest('ul'));
                }
            }
        },
        function () { },
        '1'
            );
    },
    deleteQuestion_DBCall: function (QuestionId) {
        if (QuestionId && parseInt(QuestionId) > 0) {
            var objData = new Object();
            objData["commandType"] = "DELETE_CCM_QUESTION";
            objData["QuestionId"] = QuestionId;
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
        }
    },
    enablDisablePreview: function () {
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        if (self.find('#lstSection li').length > 0) {
            self.find('#btnPreview').prop('disabled', false);
        }
        else {
            self.find('#btnPreview').prop('disabled', true);
        }
    },
    setHeightOfUl: function (objUl) {
        if (objUl) {
            var totalLi = $(objUl).find('li').length;
            if (totalLi > 0) {
                $(objUl).height(totalLi * 60);
            }
            else
                $(objUl).height(150);
        }
    },
    updateQuestionOrderNo: function (IsSubQues) {
        var self = $('#' + Admin_CCMTemplateDetails.params.PanelID);
        var jsonArray = {};

        var quests = "";
        if (IsSubQues) {
            self.find('#lstSubQuestion li').each(function (index) {
                if (quests != "")
                    quests = quests + ',';
                if ($(this).find('.toolcontroldiv').attr('QuestionId') && $(this).find('.toolcontroldiv').attr('QuestionId') > 0) {
                    quests = quests + $(this).find('.toolcontroldiv').attr('QuestionId');
                }
            });
        }
        else {
            self.find('#lstQuestion li').each(function (index) {
                if (quests != "")
                    quests = quests + ',';
                if ($(this).find('.toolcontroldiv').attr('QuestionId') && $(this).find('.toolcontroldiv').attr('QuestionId') > 0) {
                    quests = quests + $(this).find('.toolcontroldiv').attr('QuestionId');
                }
            });
        }
        if (quests != "") {
            jsonArray['QuestionIds'] = quests;
            var myJSON = JSON.stringify(jsonArray);
            Admin_CCMTemplateDetails.updateQuestionOrderDB_Call(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    updateQuestionOrderDB_Call: function (jsonData) {
        var objData = JSON.parse(jsonData);
        objData["commandType"] = "UPDATE_CCM_QUESTION_ORDER";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
    },
}