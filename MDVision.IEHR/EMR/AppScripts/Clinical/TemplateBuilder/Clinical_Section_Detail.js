sectionDetail = {

    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        sectionDetail.params = params;


        if (sectionDetail.params["PanelID"] != 'sectionDetail')
            sectionDetail.params["PanelID"] = sectionDetail.params["PanelID"] + ' #sectionDetail';
        if (sectionDetail.bIsFirstLoad) {
            sectionDetail.bIsFirstLoad = false;

            var self = $('#' + sectionDetail.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                sectionDetail.SectionLoad();
            });

            $("#" + Patient_Search.params["PanelID"] + " #txtRowspPageHF").val(6);
            $("#" + Patient_Search.params["PanelID"] + " #txtPageNoHF").val(1);



        }
    },

    SectionLoad: function () {
        $('input[type=radio][name=Q_QG_Radio]').change(function () {
            if (this.value == 'Question') {
                sectionDetail.QuestionSearch(null, null, 6);
                $(function () {
                    $('#Short_Name').keypress(function (e) {
                        var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                        if (keycode == 13) {
                            sectionDetail.QuestionSearch(null, null, 6);
                        }
                    });
                });
            }
            else if (this.value == 'QuestionGroup') {
                sectionDetail.QuestionGroupSearch();
                $(function () {
                    $('#Short_Name').keypress(function (e) {
                        var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                        if (keycode == 13) {
                            sectionDetail.QuestionGroupSearch();
                        }
                    });
                });
            }
        });

        $("#" + sectionDetail.params["PanelID"] + " #Short_Name").keyup(function () {
            if ($("#" + sectionDetail.params["PanelID"] + " #Short_Name").val().length > 3 || $("#" + sectionDetail.params["PanelID"] + " #Short_Name").val().length < 1) {
                if ($('#Search_Question').is(':checked')) {
                    sectionDetail.QuestionSearch();
                } else {
                    sectionDetail.QuestionGroupSearch();
                }
            }

        });

        sectionDetail.QuestionSearch(null, null, 6);
        sectionDetail.HideAllControls();

        if (sectionDetail.params.mode == "Add") {
            $("#" + sectionDetail.params["PanelID"] + " #pnlQuestionsinfo").addClass('disableAll');
            $('#frmsectionDetail').data('serialize', $('#frmsectionDetail').serialize());
            sectionDetail.ValidateSectionDetail(sectionDetail.params.SectionId);
        }
        else {
            sectionDetail.SectionLoadEditMode();
        }
    },

    SectionLoadEditMode: function () {
        PageNo = null;
        rpp = null;
        sectionDetail.FillSection(sectionDetail.params.SectionId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                var Section_detail = JSON.parse(response.SectionLoad_JSON);
                var self = $('#sectionDetail');
                utility.bindMyJSON(true, JSON.parse(response.SectionLoad_JSON), false, self).done(function () {

                    if (Section_detail.chkSectionDetailActive == "True") {
                        $("#" + sectionDetail.params["PanelID"] + " #chkSectionDetailActive").prop("checked", true);
                    } else {
                        $("#" + sectionDetail.params["PanelID"] + " #chkSectionDetailActive").prop("checked", false);
                    }

                    $('#txtSectionDetailTitle,#txtSectionDetailShortName,#ddlSectionDetailSpecialty').attr('disabled', true);
                    $("#" + sectionDetail.params["PanelID"] + " #pnlQuestionsinfo").removeClass('disableAll');

                   
                    //sectionDetail.QuestionSearch(null, null, 6);

                    if (Section_detail.canvasColumn != '' && Section_detail.canvasColumn != null) {
                        HTMLSection = Section_detail.canvasColumn;

                        HTMLSection = HTMLSection.replace(/&quot;/g, '"');
                        HTMLSection = HTMLSection.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                        $('#' + sectionDetail.params["PanelID"] + ' #canvasColumn').html(HTMLSection);

                    }
                    // Get QuestionGroups of Particular Section
                    sectionDetail.GetQuestionGroup_Sections(sectionDetail.params.SectionId, null, null).done(function (response) {
                        if (response.SectionQuestionGroupCount > 0) {
                            var SectionFill_JSON = JSON.parse(response.SectionFill_JSON);
                            var UpdatedQuestoins = "";
                            var SectionQuesstionGroupQuestionListInfo = [];

                            $.each(SectionFill_JSON, function (i, item) {

                                var divID = item.SectionID + '_' + item.QuestionGroupId;

                                var QuestionGroupHeadingsList = '<section id="' + item.QuesGroupSectionId + '" class="panel-title pt-none pb-none"><label>' + item.ShortName + '</label><div><a class="btn btn-close btn-xs" style="display:none" onclick="sectionDetail.DeleteQuestionGroupFromSection(' + item.QuesGroupSectionId + ',\'' + divID + '\'' + ',' + item.QuestionGroupId + ');">' +
                                                                '<i class="fa fa-times"></i></a></div><ul class="sortable ui-sortable" id="' + item.QuestionGroupId + '_' + item.QuesGroupSectionId + '"></ul></section>';

                                $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDiv').append(QuestionGroupHeadingsList);
                                $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');

                                var QuesGroupSectionId = item.QuesGroupSectionId;


                                // Get Questions of Particular QuestionGroup
                                questionGroupDetail.GetQuestions_QuestionGroup(item.QuestionGroupId, null, null).done(function (response) {
                                    if (response.QuestionGroupQuestionCount > 0) {
                                        var QuestionGroupFill_JSON = JSON.parse(response.QuestionGroupFill_JSON);
                                        var UpdatedQuestoins = "";
                                        var QuesQuestionListInfo = [];
                                        $.each(QuestionGroupFill_JSON, function (i, items) {
                                            var divID = items.QuesGroupQuestionId + '_' + items.QuestionID + '_' + items.QuestionTypeId;

                                            var QuestionListedHTML = '<li class="bg-primary" id="' + items.QuesGroupQuestionId + '"><a href="#" onclick="questionGroupDetail.getQuestionsData(' + items.QuestionID + ',' + items.QuestionTypeId + ',1,1' + ');"><span class="ui-icon ui-icon-arrow-4-diag"></span>' +
                                                                     '<div class="col-xs-11 p-none pull-left"><p class="white">' + items.QuestionDescription + '</p></div></a>' +
                                                                     '<div class="col-xs-1 pl-none pr-xs pull-right"><a href="#"></a><a class="btn btn-close btn-xs" onclick="sectionDetail.DeleteQuestionFromQuestionGroup(' + item.QuestionGroupId + ',' + QuesGroupSectionId + ',' + items.QuesGroupQuestionId + ',\'' + divID + '\'' + ');">' +
                                                                     '<i class="fa fa-times"></i></a> </div></li>';

                                            $('#' + item.QuesGroupSectionId).append(QuestionListedHTML);
                                            $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');
                                            if (items.IsUpdated == "0") {
                                                var QuesQuestionObjec = {};
                                                QuesQuestionObjec.QuestionID = items.QuestionID;
                                                QuesQuestionObjec.QuesGroupQuestionId = items.QuesGroupQuestionId;
                                                QuesQuestionObjec.QuestionTypeId = items.QuestionTypeId;
                                                QuesQuestionListInfo.push(QuesQuestionObjec);
                                            }
                                            if (QuesQuestionListInfo != null && QuesQuestionListInfo.length > 0) {
                                                questionGroupDetail.UpdateQuestionsInQuestionGroup(QuesQuestionListInfo);
                                            }
                                            //$('#' + item.QuesGroupSectionId + ' #SelectedQuestionDivList').html('');
                                            //for (var i = 0; i < response.QuestionGroupQuestionCount; i++) {
                                            //    $('#' + item.QuesGroupSectionId + ' #SelectedQuestionDivList').append('<li>' + (i + 1) + '</li>');
                                            //}

                                        });
                                        $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');
                                        $(document).ready(function () {
                                            $("#SelectedQuestionDiv li").click(function () {
                                                $("#SelectedQuestionDiv li").addClass("bg-primary");
                                                $(this).removeClass("bg-primary");
                                                $(this).addClass("bg-success");
                                            });
                                        });
                                    }
                                });
                                if (item.IsUpdated == "0") {
                                    var QuesGroupSectionObj = {};
                                    QuesGroupSectionObj.QuestionGroupId = item.QuestionGroupId;
                                    QuesGroupSectionObj.QuesGroupSectionId = item.QuesGroupSectionId;
                                    SectionQuesstionGroupQuestionListInfo.push(QuesGroupSectionObj);

                                } if (SectionQuesstionGroupQuestionListInfo != null && SectionQuesstionGroupQuestionListInfo.length > 0) {
                                    sectionDetail.UpdateQuestionGroupInSection(SectionQuesstionGroupQuestionListInfo, item.QuesGroupSectionId);
                                }
                            });
                        }
                        $(function () {

                            $('#Short_Name').keypress(function (e) {
                                var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                                if (keycode == 13) {
                                    sectionDetail.QuestionSearch();
                                }
                            });
                        });
                    });
                    sectionDetail.QuestionSearch(null, null, 6);

                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
        sectionDetail.ValidateSectionDetail(sectionDetail.params.SectionId);

    },

    UpdateQuestionGroupInSection: function (QuesQuestionListInfo, QuesGroupSectionId) {
        //   utility.myConfirm('Question was updated after last modified of Question Group, Do you want to get Updated Question?', function () {
        $.each(QuesQuestionListInfo, function (i, item) {

            var divID = item.QuestionGroupId + '_' + QuesGroupSectionId + '_SectionQuestionGroup';
            var QuestionGroupId = item.QuestionGroupId;
            var myJSON = '{TextSearched :"' + $('#SearchQuestiontxt').val() + '"}';
            var RefID = $(this);
            sectionDetail.SearchQuestionGroup(myJSON, QuestionGroupId, 1, 15).done(function (response) {
                if (response.status != false) {
                    var obj = jQuery.parseJSON(response.QuestionGroupFill_JSON);
                    $('#' + divID).html('');
                    var HTMLSectionQG = obj[0].HTMLTemplate;
                    HTMLSectionQG = HTMLSectionQG.replace(/&quot;/g, '"');
                    HTMLSectionQG = HTMLSectionQG.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                    $('#' + divID).replaceWith('<div class="toggle-content panel-body NoRadiusT" id="' + item.QuestionGroupId + '_' + QuesGroupSectionId + '_SectionQuestionGroup' + '">' + HTMLSectionQG + '</div>');

                    var QuestionGroupHTML = $('#' + item.QuestionGroupId + '_' + QuesGroupSectionId + '_SectionQuestionGroup').html();
                    sectionDetail.UpdateQuestionGroupSectionFromSection(QuesGroupSectionId, item.QuestionGroupId);

                    var self = $("#" + sectionDetail.params["PanelID"]);
                    var myJSON = self.getMyJSON();
                    sectionDetail.SectionEdit(myJSON, sectionDetail.params.SectionId);

                    sectionDetail.QuestionDragToHeading();
                }
            });
        });
    },

    UpdateQuestionGroupSectionFromSection: function (QuesGroupSectionId, QuestionGroupId) {
        var data = "QuestionGroupId=" + QuestionGroupId + "&QuesGroupSectionId=" + QuesGroupSectionId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "UPDATE_QUESTION_GROUP_SECTION_FROM_SECTION");
    },

    ValidateSectionDetail: function (SectionId) {
        $('#frmsectionDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  txtSectionDetailShortName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }

                      }
                  },
                  txtSectionDetailTitle: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }

                      }
                  },
                  ddlSectionDetailType: {
                      group: '.col-sm-3',
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
           sectionDetail.SectionSave(SectionId);
       });
    },

    SectionSave: function (SectionID) {
        var strMessage = "";
        var self = $("#" + sectionDetail.params["PanelID"]);
        var myJSON = self.getMyJSON();
        if (sectionDetail.params.mode == "Add") {
            if (strMessage == "") {
                sectionDetail.SaveSection(myJSON).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                        if (sectionDetail.params != null && sectionDetail.params.ParentCtrl != null) {

                            //UnloadActionPan(sectionDetail.params.ParentCtrl, 'sectionDetail');
                            // Clinical_Section.SectionSearch();
                        }
                        else {
                            //UnloadActionPan(sectionDetail.params.ParentCtrl, 'sectionDetail');
                            //   Clinical_Section.SectionSearch();
                        }
                        $("#" + sectionDetail.params["PanelID"] + " #pnlQuestionsinfo").removeClass('disableAll');
                        sectionDetail.params.SectionId = response.MessageId;
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            //    else
            //        utility.DisplayMessages(strMessage, 2);
            //});
        }
        else if (sectionDetail.params.mode == "Edit") {
            if (strMessage == "") {
                sectionDetail.SectionEdit(myJSON, SectionID, 2).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
    },

    SaveSection: function (SectionData) {
        var data = "SectionData=" + SectionData;
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "SAVE_SECTION");
    },

    SectionEdit: function (SectionData, SectionID) {
        var data = "SectionData=" + SectionData + "&SectionID=" + SectionID + "&HTMLTempalteSection=" + $('#' + sectionDetail.params["PanelID"] + ' #canvasColumn').html();
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "UPDATE_SECTION");
    },

    GetQuestionGroup_Sections: function (SectionId, QuesGroupSectionId, fieldsJSON) {
        var fieldsJSON = "";
        if (SectionId == null) {
            SectionId = 0;
        }
        if (QuesGroupSectionId == null) {
            QuesGroupSectionId = 0;
        }

        var data = "fieldsJSON=" + fieldsJSON + "&SectionId=" + SectionId + "&QuesGroupSectionId=" + QuesGroupSectionId;

        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "GETQUESTIONGROUP_SECTIONS");
    },

    HideAllControls: function () {
        $('div[id^="divAnswer"]').hide();
    },

    QuestionSearch: function myfunction(questionID, PageNo, rpp) {
        var QuestionId = "";
        var myJSON = '{TextSearched :"' + $('#Short_Name').val() + '"}';
        sectionDetail.SearchQuestion(myJSON, QuestionId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                sectionDetail.QuestionsListLoad(response, PageNo, rpp);

            }
        });
    },

    SearchQuestion: function (QuestionData, questionID, PageNo, rpp) {

        if (PageNo == null) PageNo = 1;
        if (rpp == null) rpp = 6;

        var data = "QuestionData=" + QuestionData + "&questionID=" + questionID + "&PageNo=" + PageNo + "&rpp=" + rpp;
        return MDVisionService.defaultService(data, "Clinical_Question_Group", "SEARCH_QUESTION");
    },

    QuestionGroupSearch: function myfunction(questionGroupID, PageNo, rpp) {
        var QuestionGroupID = "";
        var myJSON = '{TextSearched :"' + $('#Short_Name').val() + '"}';
        sectionDetail.SearchQuestionGroup(myJSON, QuestionGroupID, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                sectionDetail.QuestionsGroupListLoad(response, PageNo, rpp);
            }
        });
    },

    SearchQuestionGroup: function (QuestionGroupData, QuestionGroupID, PageNo, rpp) {

        if (PageNo == null) PageNo = 1;
        if (rpp == null) rpp = 6;

        var data = "QuestionGroupData=" + QuestionGroupData + "&QuestionGroupID=" + QuestionGroupID + "&PageNo=" + PageNo + "&rpp=" + rpp;
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "SEARCH_QUESTION_GROUP");
    },

    QuestionsListLoad: function (response, PageNo, rpp) {
        $("#lstQuestion").empty();
        //$("#lstQuestionSortOrder").empty();
        if (response.QuestionCount > 0) {
            var QuestionLoadJSONData = JSON.parse(response.QuestionLoad_JSON);
            var FirstInsuranceId = "";

            $.each(QuestionLoadJSONData, function (i, item) {

                var bgcolor = "";
                if (item.IsActive != "True") {
                    bgcolor = "bg-danger";
                }
                var lstQuestion = '<div class="bg-primary sectionquestiondrag ' + bgcolor + '" id="' + item.QuestionId + '_' + item.QuestionTypeId + '" style="margin-bottom:2px"><a href="#"><span class="ui-icon ui-icon-arrow-4-diag" style="float:left;"></span><span class="white">' + item.Description + '</span><span class="pull-right white">' + item.Description1 + '</span></a></div>';
                $("#lstQuestion").last().append(lstQuestion);
            });

            $("#" + sectionDetail.params["PanelID"] + " #divQuestionlistPaging").css("display", "inline");
            var RecordsPerPage = rpp != null ? rpp : 6;
            var CurrentPage = PageNo != null ? PageNo : 1;
            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                sectionDetail.GetCustomPaging("divQuestionlistPaging", response.iTotalDisplayRecords, 2, "sectionDetail", CurrentPage, RecordsPerPage);
            }

            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            TotalQuestions = response.iTotalDisplayRecords;
            $('#sectionDetail #txtTotalQuestions').val(TotalQuestions);
            sectionDetail.ShowHideListGrid(CurrentPage, RecordsPerPage, TotalQuestions)

            sectionDetail.QuestionDragToHeading();
        }
    },

    QuestionDragToHeading: function () {
        $(".sectionquestiondrag").draggable({
            revert: true,
            appendTo: 'body',
            stack: '.questiondrop ',

            start: function () {
                $('#tabs').css('z-index', '9999');
            },
            stop: function () {
                $('#tabs').css('z-index', '0');
            }
        });

        $(".questiondrop ").droppable({
            accept: ".sectionquestiondrag",
            activeClass: 'droppable-active',
            hoverClass: 'droppable-hover',
            // tolerance: 'pointer',
            drop: function (ev, ui) {

                var divID = ui.draggable.attr('id');
                var Ques_QuestionType = ui.draggable.attr('id').split('_');
                var QuestionId = Ques_QuestionType[0];
                var questionTypeId = Ques_QuestionType[1];
                QuestionText = ui.draggable.find('.white').html();

                var myJSON = '{TextSearched :"' + $('#Short_Name').val() + '"}';
                var Ref = $(this);
                var $ctrl;
                sectionDetail.SearchQuestion(myJSON, QuestionId, 1, 1).done(function (response) {
                    if (response.status != false) {
                        var obj = jQuery.parseJSON(response.QuestionLoad_JSON);
                        if (questionTypeId == 1 || questionTypeId == 10) {
                            $ctrl = $('<input/>').attr({ type: 'text', name: 'text_' + QuestionId, id: 'TextField_' + QuestionId }).addClass("form-control");
                        }
                        else if (questionTypeId == 2) {
                            $ctrl = '<div class="radio-custom col-xs-2"><input type="radio" id=RadioTrue_' + QuestionId + ' name=r2_' + QuestionId + ' checked="checked"><label id="lblRadioFieldPreview1" class="control-label" for="">' + obj[0].BoolTrueDisplay + '</label></div><div class="radio-custom col-xs-2"><input type="radio" name=r2_' + QuestionId + ' id=RadioFalse_' + QuestionId + '><label id="lblRadioFieldPreview2" class="control-label" for="">' + obj[0].BoolFalseDisplay + '</label></div>';
                        }
                        else if (questionTypeId == 3) {

                            var data = ['- SELECT -'];
                            var drpValues = obj[0].DisplayText;
                            var drp = drpValues.split(',');
                            var texts = [];
                            for (var i = 0; i < drp.length; i++) {
                                if (/\S/.test(drp[i])) {
                                    texts.push($.trim(drp[i]));
                                    data.push($.trim(drp[i]));
                                }
                            }
                            var $ctrl = $('<select />').attr({ "id": 'Select_' + QuestionId, "type": 'select' }).addClass("form-control");
                            for (var val in data) {
                                $('<option />', { value: val, text: data[val] }).appendTo($ctrl);
                            }
                        }
                        else if (questionTypeId == 5) {
                            $ctrl = '<div class="col-xs-2 pr-xs"><input type="text" id="FractionField1_' + QuestionId + '" class="form-control"></div><div class="size10 pull-left text-center"><h4 class="m-none">/</h4></div><div class="col-xs-2 pl-xs"> <input type="text" id="FractionField2_' + QuestionId + '" class="form-control"></div>';
                        }
                        else if (questionTypeId == 6) {
                            $ctrl = '<div class="input-group"><span class="input-group-addon"><i class="fa fa-calendar"></i></span><input id="dpQuestion_' + QuestionId + '" class="form-control" type="text" data-plugin-datepicker=""></div>';
                        }
                        else if (questionTypeId == 7) {
                            $ctrl = '<div class="input-group"><span class="input-group-addon"> <i class="fa fa-clock-o"></i> </span><input id="tpQuestion_' + QuestionId + '" type="text" data-plugin-timepicker="" data-plugin-options="" class="form-control"></div>';
                        }
                        else if (questionTypeId == 9) {

                            sectionDetail.FillQuestion(QuestionId, 1, 1).done(function (response) {
                                if (response.status != false) {
                                    var Question_detail = JSON.parse(response.QuestionLoad_JSON);
                                    $ctrl = '<div class="col-xs-6"> <img id="imgField_' + QuestionId + '" class="img-responsive img-center img-thumbnail NoRadius p-xs" src="' + Question_detail.imdIDPreview + '" alt="Image" /> </div>';
                                }
                            });
                        }

                    }
                    var currentcanvas = $('#dynamicQuestionDrop').html();
                    //if (currentcanvas.indexOf(divID) !== -1) {
                    //}
                    //else {
                    //}

                    var QuestionGroupIdDroppedQuestion = Ref.parent().attr('id').split('_')[0];

                    //Insert Question into QuestionGroup from Section
                    var QuesGroupQuestionId, QuesGroupSectionId;
                    sectionDetail.GetQuestionGroup_Sections(sectionDetail.params.SectionId, null, null).done(function (response) {
                        if (response.SectionQuestionGroupCount > 0) {
                            var SectionFill_JSON = JSON.parse(response.SectionFill_JSON);
                            $.each(SectionFill_JSON, function (i, item) {
                                QuesGroupSectionId = item.QuesGroupSectionId;
                            });

                            sectionDetail.InsertQuestioninQuestionGroupFromSection(QuestionGroupIdDroppedQuestion, QuestionId).done(function (response) {
                                if (response.status != false) {
                                    QuesGroupQuestionId = response.QuesGroupQuestionId;

                                    var QuestionHTMLDroppedStartDiv = '<div class="col-xs-12 question-child questiondrag" id="' + QuesGroupQuestionId + '_' + divID + '">';
                                    divID = QuesGroupQuestionId + '_' + divID;
                                    var QuestionHTMLHeading = '<button aria-hidden="true" class="fa fa-times noBorder pull-right" type="button" id="' + QuestionId + '" onclick="sectionDetail.DeleteQuestionFromQuestionGroup(' + QuestionGroupIdDroppedQuestion + ',' + QuesGroupSectionId + ',' + QuesGroupQuestionId + ',\'' + divID + '\'' + ');"></button><h5 class="QuestionHeading">' + obj[0].Description + '</h5>';
                                    var QuestionTypeHTML = '<div class="size40per pull-left QuestionType"><label class="control-label">' + obj[0].Description1 + '</label></div>';
                                    var QuestionDetailHTML = '<div class="size60per pull-left">' + ((typeof $ctrl[0].outerHTML === "undefined") ? $ctrl : $ctrl[0].outerHTML) + '</div>';
                                    var QuestionHTMLDroppedendtDiv = '</div>';

                                    $(Ref).append(QuestionHTMLDroppedStartDiv + QuestionHTMLHeading + QuestionTypeHTML + QuestionDetailHTML + QuestionHTMLDroppedendtDiv);

                                    //Update ClinicalQuestionGroups HTML, as changes are made in ClinicalQuestionGroup from ClinicalSection
                                    var QuestionGroupHTML = $('#' + QuestionGroupIdDroppedQuestion + '_' + QuesGroupSectionId + '_SectionQuestionGroup').html(); //('#' + id + '.toggle-content')
                                    sectionDetail.UpdateQuestionGroupFromSection(QuestionGroupHTML, QuestionGroupIdDroppedQuestion);

                                    var self = $("#" + sectionDetail.params["PanelID"]);
                                    var myJSON = self.getMyJSON();
                                    sectionDetail.SectionEdit(myJSON, sectionDetail.params.SectionId);

                                    var questionGroupId = Ref.parent().attr('id').split('_')[0];//$('#' + QuestionId).parent().parent().attr('id').split('_')[0];

                                    var QuestionListedHTML = '<li class="bg-primary" id="' + QuesGroupQuestionId + '"><a href="#" onclick="sectionDetail.getQuestionsData(' + QuestionId + ',' + questionTypeId + ',1,1' + ');"><span class="ui-icon ui-icon-arrow-4-diag"></span>' +
                                                           '<div class="col-xs-11 p-none pull-left"><p class="white">' + QuestionText + '</p></div></a>' +
                                                           '<div class="col-xs-1 pl-none pr-xs pull-right"><a href="#"></a><a class="btn btn-close btn-xs" onclick="sectionDetail.DeleteQuestionFromQuestionGroup(' + questionGroupId + ',' + QuesGroupSectionId + ',' + QuesGroupQuestionId + ',\'' + divID + '\'' + ');">' +
                                                           '<i class="fa fa-times"></i></a> </div></li>';

                                    $('#' + sectionDetail.params["PanelID"] + ' #AddedQuestions ul#' + questionGroupId + '_' + QuesGroupSectionId).last().append(QuestionListedHTML);
                                    $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');
                                }
                            });
                        }
                    });
                    setTimeout(function () { $('#' + QuestionId).css("position", ""); }, 300);
                    $(document).ready(function () {
                        //$("#dpQuestion_" + QuestionId).datepicker({
                        //}).on('changeDate', function (e) {
                        //    $(this).datepicker('hide');
                        //});
                        //Azhar change this for max length property and re use, if caused any issue ping azhar
                        utility.CreateDatePicker("#dpQuestion_" + QuestionId, function () {
                            //  on-change callback method
                        }, true);
                        $("#tpQuestion_" + QuestionId).timepicker({
                            defaultTime: '12:00 PM'
                        });
                    });

                    $('#frmsectionDetail').data('serialize', $('#frmsectionDetail').serialize());
                });

            }

        });
    },

    DeleteQuestionFromQuestionGroup: function (QgID, QuesGroupSectionId, QuesGroupQuestionId, divID) {
        var QuesGroupSectionID = QuesGroupSectionId;
        utility.myConfirm('1', function () {
            var selectedValue = QuesGroupQuestionId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                var QuestionID = divID.split('_')[0];
                sectionDetail.DeleteQuestionGroupQuestion(QuestionID, selectedValue).done(function (response) {
                    if (response.status != false) {
                        $('#' + QgID + '_SectionQuestionGroup #' + divID).remove();
                        $('#HTMLTempalteQuestionGroup #' + divID).remove();
                        $('#' + divID).remove();
                        $('#AddedQuestions #' + QuesGroupQuestionId).remove();

                        $('#' + sectionDetail.params["PanelID"] + ' #AddedQuestions').find('section#' + selectedValue).remove();
                        $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');
                        //var self = $("#" + sectionDetail.params["PanelID"]);
                        //var myJSON = self.getMyJSON();
                        //var QuestionGroupHTML = $('#' + QgID + '_SectionQuestionGroup').html();
                        //sectionDetail.UpdateQuestionGroupFromSection(QuestionGroupHTML, QgID);

                        var QuestionGroupHTML = $('#' + QgID + '_' + QuesGroupSectionID + '_SectionQuestionGroup').html();
                        sectionDetail.UpdateQuestionGroupFromSection(QuestionGroupHTML, QgID, $('#HTMLTempalteQuestionGroup').html());

                        var self = $("#" + sectionDetail.params["PanelID"]);
                        var myJSON = self.getMyJSON();
                        sectionDetail.SectionEdit(myJSON, sectionDetail.params.SectionId);

                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
              '1'
          );
    },

    DeleteQuestionGroupQuestion: function (QuestionID, QuestionGroupId) {
        var data = "QuestionID=" + QuestionID + "&QUESTIONGROUPID=" + QuestionGroupId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Clinical_Question_Group", "DELETE_QUESTIONFROM_QUESTION_GROUP");
    },

    UpdateQuestionGroupFromSection: function (QuestionGroupHTML, QuestionGroupId, UpdatedQuestionGroupHTML) {

        var data = "HTMLSectionQuestionGroup=" + QuestionGroupHTML + "&QuestionGroupId=" + QuestionGroupId + "&UpdatedQuestionGroupHTML=" + UpdatedQuestionGroupHTML;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "UPDATE_QUESTION_GROUP_FROM_SECTION");
    },

    InsertQuestioninQuestionGroupFromSection: function (questionGroupId, questionID) {

        var data = "questionID=" + questionID + "&questionGroupId=" + questionGroupId;
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "INSERT_QUESTION_IN_QUESTIONGROUP_FROM_SECTION");
    },

    DropHeading: function () {

        var params = [];
        params["mode"] = "Add";
        params["ParentCtrl"] = "sectionDetail";
        LoadActionPan('sectionHeadingDetail', params);

    },

    QuestionsGroupListLoad: function (response, PageNo, rpp) {
        $("#lstQuestion").empty();
        //$("#lstQuestionSortOrder").empty();
        if (response.QuestionGroupCount > 0) {
            var QuestionGroupFill_JSON = JSON.parse(response.QuestionGroupFill_JSON);
            var FirstInsuranceId = "";
            var RecordsPerPage = rpp != null ? rpp : 6;
            var CurrentPage = PageNo != null ? PageNo : 1;
            $.each(QuestionGroupFill_JSON, function (i, item) {

                var bgcolor = "";
                if (item.IsActive != "True") {
                    bgcolor = "bg-danger";
                }

                //<section class="toggle mb-sm active"><label class="">XYZ</label></section>

                var lstQuestion = '<div class="bg-primary questiondrag ' + bgcolor + '" id="' + item.QuestionGroupID + '" ><a href="#"><span class="ui-icon ui-icon-arrow-4-diag" style="float:left;"></span><span class="white">' + item.ShortName + '</span></a></div>';
                //var lstQuestion = '<div class="bg-primary questiondrag ' + bgcolor + '" id="' + item.QuestionGroupID + '" ><a href="#"><span class="ui-icon ui-icon-arrow-4-diag" style="float:left;"></span><span class="white">' + item.ShortName + '</span></a></div>';
                //var lstQuestion = '<section class="toggle mb-sm active bg-primary questiondrag ' + bgcolor + '" id="' + item.QuestionGroupID + '"><a href="#"><span class="ui-icon ui-icon-arrow-4-diag" style="float:left;"><span class="white">' + item.ShortName + '</span</a></section>';
                $("#lstQuestion").last().append(lstQuestion);
            });

            $("#" + sectionDetail.params["PanelID"] + " #divQuestionlistPaging").css("display", "inline");
            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                sectionDetail.GetCustomPaging("divQuestionlistPaging", response.iTotalDisplayRecords, 2, "sectionDetail", CurrentPage, RecordsPerPage);
            }

            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            TotalQuestions = response.iTotalDisplayRecords;
            $('#sectionDetail #txtTotalQuestions').val(TotalQuestions);
            sectionDetail.ShowHideListGrid(CurrentPage, RecordsPerPage, TotalQuestions);

            sectionDetail.QuestionGroupCreated();

        }
    },

    QuestionGroupCreated: function () {

        $(".questiondrag").draggable({
            revert: true,
            appendTo: 'body',
            stack: '.Sectiondrop',

            start: function () {
                $('#tabs').css('z-index', '9999');
            },
            stop: function () {
                $('#tabs').css('z-index', '0');
            }
        });

        $(".Sectiondrop").droppable({
            accept: ".questiondrag",
            activeClass: 'droppable-active',
            hoverClass: 'droppable-hover',
            // tolerance: 'pointer',
            drop: function (ev, ui) {

                var questionGroupId = ui.draggable.attr('id');
                //var Ques_QuestionType = ui.draggable.attr('id').split('_');
                //var QuestionId = Ques_QuestionType[0];
                //var questionTypeId = Ques_QuestionType[1];

                QuestionText = ui.draggable.find('.white').html();

                var myJSON = '{TextSearched :"' + $('#Short_Name').val() + '"}';
                var Ref = $(this);
                var HTMLQuestionGroup;
                sectionDetail.SearchQuestionGroup(myJSON, questionGroupId, 1, 1).done(function (response) {
                    sectionDetail.InsertQuestionGroup_Section(myJSON, questionGroupId, 1, 1).done(function (response) {
                        if (response.status != false) {
                            var obj = jQuery.parseJSON(response.QuestionGroupLoad_JSON);

                            if (obj[0].HTMLTemplate != '' && obj[0].HTMLTemplate != null) {
                                HTMLQuestionGroup = obj[0].HTMLTemplate;
                                //HTMLQuestionGroup = HTMLQuestionGroup.replace('&lt;', '<');

                                HTMLQuestionGroup = HTMLQuestionGroup.replace(/&quot;/g, '"');
                                HTMLQuestionGroup = HTMLQuestionGroup.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                                // $('#' + sectionDetail.params["PanelID"] + ' #HTMLTempalteQuestionGroup').html(HTMLQuestionGroup);
                            }

                        }
                        var divID = questionGroupId + '_' + response.QuesGroupSectionId;
                        var _QuesGroupSectionId = response.QuesGroupSectionId;
                        var QuestionListedHTML = '<li class="bg-primary" id="' + response.QuesGroupSectionId + '"><a href="#" onclick="sectionDetail.getQuestionsData(' + questionGroupId + ',' + questionGroupId + ',1,1' + ');"><span class="ui-icon ui-icon-arrow-4-diag"></span>' +
                                               '<div class="col-xs-11 p-none pull-left"><p class="white">' + QuestionText + '</p></div></a>' +
                                               '<div class="col-xs-1 pl-none pr-xs pull-right"><a href="#"></a><a class="btn btn-close btn-xs" onclick="sectionDetail.DeleteQuestionGroupFromSection(' + response.QuesGroupSectionId + ',\'' + divID + '\'' + ',' + questionGroupId + ');">' +
                                               '<i class="fa fa-times"></i></a> </div></li>';

                        var QuestionGroupHeadings = '<section id="' + questionGroupId + '" class="panel-title pt-none pb-none"><label>' + QuestionText + '</label><div><a class="btn btn-close btn-xs" style="display:none" onclick="sectionDetail.DeleteQuestionGroupFromSection(' + response.QuesGroupSectionId + ',\'' + divID + '\'' + ',' + questionGroupId + ');">' +
                                               '<i class="fa fa-times"></i><div></a><ul class="sortable ui-sortable" id="' + questionGroupId + '_' + response.QuesGroupSectionId + '"></ul></section>';

                        $('#' + sectionDetail.params["PanelID"] + ' #AddedQuestions').append(QuestionGroupHeadings);
                        $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');

                        //$('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDiv').append(QuestionListedHTML);
                        //$('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');

                        questionGroupDetail.GetQuestions_QuestionGroup(questionGroupId, null, null).done(function (response) {
                            if (response.QuestionGroupQuestionCount > 0) {
                                var QuestionGroupFill_JSON = JSON.parse(response.QuestionGroupFill_JSON);
                                $.each(QuestionGroupFill_JSON, function (i, item) {
                                    var divID = item.QuesGroupQuestionId + '_' + item.QuestionID + '_' + item.QuestionTypeId;

                                    var QuestionListedHTML = '<li class="bg-primary" id="' + item.QuesGroupQuestionId + '"><a href="#" onclick="questionGroupDetail.getQuestionsData(' + item.QuestionID + ',' + item.QuestionTypeId + ',1,1' + ');"><span class="ui-icon ui-icon-arrow-4-diag"></span>' +
                                                             '<div class="col-xs-11 p-none pull-left"><p class="white">' + item.QuestionDescription + '</p></div></a>' +
                                                             '<div class="col-xs-1 pl-none pr-xs pull-right"><a href="#"></a><a class="btn btn-close btn-xs" onclick="sectionDetail.DeleteQuestionFromQuestionGroup(' + item.QuestionGroupID + ',' + _QuesGroupSectionId + ',' + item.QuesGroupQuestionId + ',\'' + divID + '\'' + ');">' +
                                                             '<i class="fa fa-times"></i></a> </div></li>';

                                    $('#' + item.QuestionGroupID + '_' + _QuesGroupSectionId).append(QuestionListedHTML);
                                    $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');

                                });
                                $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');
                                $(document).ready(function () {
                                    $("#SelectedQuestionDiv li").click(function () {
                                        $("#SelectedQuestionDiv li").addClass("bg-primary");
                                        $(this).removeClass("bg-primary");
                                        $(this).addClass("bg-success");
                                    });
                                });
                            }
                        });

                        //$('#' + questionGroupId).append(HTMLQuestionGroup);
                        var DroppedQuestionHeadingId = ui.draggable.attr('id');
                        var DroppedQuestionHeadingText = ui.draggable.children().children()[1].innerText;

                        var DroppedHeadingCollapsable = '<section class="toggle mb-sm active" style="margin-bottom: 0px !important;" onclick="sectionDetail.OnSectionHeadingDrop(' + DroppedQuestionHeadingId + ',' + response.QuesGroupSectionId + ');" id="' + DroppedQuestionHeadingId + '_' + response.QuesGroupSectionId + '"><label class="">' + DroppedQuestionHeadingText + '</label></section>';
                        // ui.draggable.detach().appendTo(Ref);//.css('position','initial'));
                        Ref.append(DroppedHeadingCollapsable);
                        if (HTMLQuestionGroup == undefined) {
                            //HTMLQuestionGroup = "";
                            var questionGroupDetails = JSON.parse(response.QuestionGroupLoad_JSON);
                            var CanvasColumns = "";
                            if (questionGroupDetails[0].Canvas > 1) {
                                for (var i = 0; i < questionGroupDetails[0].Canvas; i++) {
                                    CanvasColumns = CanvasColumns + '<div class="col-sm-' + 12 / questionGroupDetails[0].Canvas + ' questiondrop" style=" left: 0px; top: 0px;min-height:200px;"></div>';
                                }
                            } else {
                                CanvasColumns = CanvasColumns + '<div class="questiondrop" style=" left: 0px; top: 0px;min-height:200px;"></div>';
                            }
                            HTMLQuestionGroup = CanvasColumns;
                        }
                        Ref.append('<div class="toggle-content panel-body NoRadiusT" id="' + DroppedQuestionHeadingId + '_' + response.QuesGroupSectionId + '_SectionQuestionGroup' + '">' + HTMLQuestionGroup + '</div>');

                        //$("#canvasColumnOne").append('<div id="droppable2-inner" class="ui-widget-header"><p>Inner droppable (greedy)</p></div>');

                        setTimeout(function () { $('#' + questionGroupId).css("position", ""); }, 300);
                        $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');
                        $(document).ready(function () {
                            //$("#dpQuestion_" + questionGroupId).datepicker({
                            //}).on('changeDate', function (e) {
                            //    $(this).datepicker('hide');
                            //});
                            utility.CreateDatePicker("#dpQuestion_" + questionGroupId, function () {
                                //  on-change callback method
                            }, true);
                            $("#tpQuestion_" + questionGroupId).timepicker({
                                defaultTime: '12:00 PM'
                            });
                        });
                        var self = $("#" + sectionDetail.params["PanelID"]);
                        var myJSON = self.getMyJSON();
                        sectionDetail.SectionEdit(myJSON, sectionDetail.params.SectionId);
                        //for (var i = 0; i < $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDiv').find('li').length; i++) {
                        //    $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').append('<li>' + (i + 1) + '</li>');
                        //}
                        // $("#canvasColumn").height($("#canvasColumn").height() + 200);
                    });
                });
            }
        });
    },

    OnSectionHeadingDrop: function (id, QG_SectionID) {
        var QGID = id;
        var QGSectionID = QG_SectionID;
        id = id + '_' + QG_SectionID + '_SectionQuestionGroup';
        $('#canvasColumn').find('#' + id + '.toggle-content').toggle();
        //$('#dynamicQuestionDrop').find('#' + id + '.toggle-content').toggle();
        $('#HTMLTempalteClinical_Template').find('#' + id).toggle();

        $('#HTMLTemplate').find('#' + id).toggle();
    },

    InsertQuestionGroup_Section: function (QuestionGroupData, questionGroupID, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 6;
        }
        var data = "questionGroupID=" + questionGroupID + "&SectionId=" + sectionDetail.params.SectionId;

        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "INSERT_QUESTIONGROUP_SECTION");
    },

    DeleteQuestionGroupFromSection: function (Section_QuestionGroupId, divID, questionGroupId) {
        utility.myConfirm('1', function () {
            var selectedValue = Section_QuestionGroupId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                sectionDetail.DeleteSectionQuestionGroup(selectedValue).done(function (response) {
                    if (response.status != false) {
                        $('#' + divID).children().remove();
                        $('#pnlQuestionsinfo #' + questionGroupId).children().remove();
                        $('#AddedQuestions #' + questionGroupId).remove();

                        $('#' + sectionDetail.params["PanelID"] + ' #AddedQuestions').find('section#' + selectedValue).remove();
                        $('#' + sectionDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');

                        utility.DisplayMessages(response.Message, 1);
                        var self = $("#" + sectionDetail.params["PanelID"]);
                        var myJSON = self.getMyJSON();
                        sectionDetail.SectionEdit(myJSON, sectionDetail.params.SectionId);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
              '1'
          );
    },

    DeleteSectionQuestionGroup: function (Section_QuestionGroupId, SectionID) {
        var data = "Section_QuestionGroupId=" + Section_QuestionGroupId + "&SectionID=" + sectionDetail.params['SectionId'];
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "DELETE_QUESTION_GROUP_FROM_SECTION");
    },

    GetCustomPaging: function (pagingDivId, TotalRecords, PagesToDisplay, ClassName, PageNo, rpp, q_qg) {
        if (!q_qg)
            GetPagingSmallSize(pagingDivId, TotalRecords, PagesToDisplay, ClassName, PageNo, rpp);
        GetPagingSmallSize(pagingDivId, TotalRecords, PagesToDisplay, ClassName, PageNo, rpp);

    },

    ShowHideListGrid: function (CurrentPage, RecordsPerPage, TotalQuestions, q_qg) {
        // $("#lstQuestion").find('li').hide();
        for (var i = (RecordsPerPage * CurrentPage) - 5; i < (RecordsPerPage * CurrentPage) + RecordsPerPage - 5; i++) {
            $("#lstQuestion").find('li:nth-child(' + i + ')').show();
        }
        var toRecords = (RecordsPerPage * CurrentPage) + RecordsPerPage - 6;
        if (TotalQuestions == null) {
            TotalQuestions = $("#lstQuestion").find('li').length;
        }

        var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + TotalQuestions + " Record(s)";
        $("#divQuestionlistPaging #divShowingEntries").text(showingText);
        // Change Background Color to Black for selected page
        $("#divQuestionlistPaging li").each(function () {
            if ($(this).text() == CurrentPage) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
    },

    SelectedPageClick: function (PageNo, objPage) {
        sectionDetail.ShowHideListGrid(PageNo, 6);

        // sectionDetail.QuestionGroupSearch(null, PageNo, 15);
        var IsQuestion = $('#Search_Question').is(':checked');
        if (IsQuestion) {
            sectionDetail.QuestionSearch(null, PageNo, 6);
        }
        else {
            sectionDetail.QuestionGroupSearch(null, PageNo, 6);
        }

    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#divQuestionlistPaging li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            //sectionDetail.ShowHideListGrid(currentPageNo, 6, null);
            var IsQuestion = $('#Search_Question').is(':checked');
            if (IsQuestion) {
                sectionDetail.QuestionSearch(null, currentPageNo, 6);
            }
            else {
                sectionDetail.QuestionGroupSearch(null, currentPageNo, 6);
            }
            //sectionDetail.QuestionSearch(null, currentPageNo, 6);
        }
        var LastPageNo = $("#divQuestionlistPaging li:nth-child(3)").text();
        if (currentPageNo < LastPageNo) {
            PreviousClickSmallSizePaging(TotalPages, DisplayPages, pagingDivId, "sectionDetail.");
            setTimeout(function () {
                $("#divQuestionlistPaging li:nth-child(4)").attr("class", "active");
            }, 200
            );
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#divQuestionlistPaging li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        var LastPageNo = $("#divQuestionlistPaging li:nth-child(4)").text();
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            //sectionDetail.QuestionSearch(null, currentPageNo, 6);
            var IsQuestion = $('#Search_Question').is(':checked');
            if (IsQuestion) {
                sectionDetail.QuestionSearch(null, currentPageNo, 6);
            }
            else {
                sectionDetail.QuestionGroupSearch(null, currentPageNo, 6);
            }
        }
        if (currentPageNo > LastPageNo) {
            NextClickSmallSizePaging(TotalPages, DisplayPages, pagingDivId, "sectionDetail.");
            setTimeout(function () { $("#divQuestionlistPaging li:nth-child(3)").attr("class", "active"); }, 200
            );
        }
    },

    getQuestionsData: function (questionId, questionTypeId, rpp, currPage) {
        //alert(questionId + '' + qGroupId);

        sectionDetail.FillQuestion(questionId, rpp, currPage).done(function (response) {
            if (response.status != false) {
                var Section_detail = JSON.parse(response.QuestionLoad_JSON);
                if (questionTypeId == 1) {
                    questionDetail.HideAllControls();
                    $('#divAnswerTextFieldAnswer').show();
                    if (Section_detail.chkAutoComplete == "True") {
                        $("#" + sectionDetail.params["PanelID"] + " #chkAutoComplete").prop("checked", true);
                    } else {
                        $("#" + sectionDetail.params["PanelID"] + " #chkAutoComplete").prop("checked", false);
                    }
                }
                else if (questionTypeId == 2) {
                    questionDetail.HideAllControls();
                    $('#divAnswerRadioFieldAnswer').show();
                }
                else if (questionTypeId == 3) {
                    questionDetail.HideAllControls();
                    $('#divAnswerDropdownFieldAnswer').show();
                }
                else if (questionTypeId == 5) {
                    questionDetail.HideAllControls();
                    $('#divAnswerFractionFieldAnswer').show();
                }
                else if (questionTypeId == 6) {
                    questionDetail.HideAllControls();
                    $('#divAnswerDateFieldAnswer').show();
                }
                else if (questionTypeId == 7) {
                    questionDetail.HideAllControls();
                    $('#divAnswerTimeFieldAnswer').show();
                }
                else if (questionTypeId == 9) {
                    questionDetail.HideAllControls();
                    $('#divAnswerImageFieldAnswer').show();
                }
                else if (questionTypeId == 10) {
                    questionDetail.HideAllControls();
                    $('#divAnswerNumberFieldAnswer').show();
                }

                //var Tab = params.TabID;
                if (params.TabID == 'clinicalTabSection') {
                    var self = $('#sectionDetail');
                    utility.bindMyJSON(true, JSON.parse(response.QuestionLoad_JSON), false, self);
                } else {
                    var self = $('#questionGroupDetail');
                    utility.bindMyJSON(true, JSON.parse(response.QuestionLoad_JSON), false, self);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    getQuestionsData1: function myfunction(questionId, questionTypeId, rpp, currPage) {

        sectionDetail.FillQuestion(questionId, 1, 1).done(function (response) {
            if (response.status != false) {
                // Get the JSON of Selected Question
                var obj = jQuery.parseJSON(response.QuestionLoad_JSON);

                var newlabel = document.createElement("Label");
                newlabel.setAttribute("for", obj.ShortName);
                newlabel.innerHTML = obj.questionName;
                var $lbl1 = $('<label/>').attr({ name: 'lbl1' }).addClass("control-label");

                if (questionTypeId == 1 || questionTypeId == 10) {

                    var theFirstData = obj.TextLength;
                    var $ctrl = $('<input/>').attr({ type: 'text', name: 'text' }).addClass("form-control");
                    $('#testidkkk').append(newlabel);
                    $('#testidkkk').append($ctrl);

                }
                else if (questionTypeId == 2) {

                    var label1 = document.createElement("Label");
                    label1.setAttribute("for", obj.ShortName);
                    label1.innerHTML = obj.questionRadioLabel1;
                    var $ctrl1 = $('<input/>').attr({ type: 'radio', name: 'rad' }).addClass("radio-custom");
                    var label2 = document.createElement("Label");
                    label2.setAttribute("for", obj.ShortName);
                    label2.innerHTML = obj.questionRadioLabel2;
                    var $ctrl2 = $('<input/>').attr({ type: 'radio', name: 'rad' }).addClass("radio-custom");
                    $('#testidkkk').append(label1);
                    $('#testidkkk').append($ctrl1);
                    $('#testidkkk').append(label2);
                    $('#testidkkk').append($ctrl2);

                }
                else if (questionTypeId == 3) {

                    var data = ['- SELECT -'];
                    var drpValues = obj.txtareaElementsDropDown;
                    var drp = drpValues.split(',');
                    var texts = [];
                    for (var i = 0; i < drp.length; i++) {
                        if (/\S/.test(drp[i])) {
                            texts.push($.trim(drp[i]));
                            data.push($.trim(drp[i]));
                        }
                    }
                    var s = $('<select />').addClass("form-control");
                    for (var val in data) {
                        $('<option />', { value: val, text: data[val] }).appendTo(s);
                    }
                    $('#testidkkk').append(newlabel);
                    $('#testidkkk').append(s);

                }
                    //col-xs-2 pl-none pr-md

                else if (questionTypeId == 5) {

                    $('#testidkkk').append(newlabel);
                    var $ctrl1 = $('<input/>').attr({ type: 'text', name: 'text1', style: "width: 100px;" }).addClass("form-control");
                    $('#testidkkk').append($ctrl1);
                    $('#testidkkk').append('/');
                    var $ctrl2 = $('<input/>').attr({ type: 'text', name: 'text2', style: "width: 100px;" }).addClass("form-control");
                    $('#testidkkk').append($ctrl2);
                }

                else if (questionTypeId == 7) {
                    $('#testidkkk').append(newlabel);
                    var tPicker = '<div class="input-group"><span class="input-group-addon"> <i class="fa fa-clock-o"></i> </span><input id="tpQuestion_' + questionId + '" type="text" data-plugin-timepicker="" data-plugin-options="" class="form-control"></div>';
                    var $ctrl1 = $('<input/>').attr({ type: 'text', id: 'tpQuestion', style: "width: 100px;" }).addClass("form-control");
                    $('#testidkkk').append(tPicker);
                    //$('#testidkkk').append($ctrl1);
                    $(document).ready(function () {
                        $("#tpQuestion_" + questionId).timepicker({
                            defaultTime: '12:00 PM'
                            //defaultTime: false
                        });
                    });
                }
                else if (questionTypeId == 6) {
                    $('#testidkkk').append(newlabel);
                    var dtpicker = '<div class="input-group"><span class="input-group-addon"><i class="fa fa-calendar"></i></span><input id="dpQuestion_' + questionId + '" class="form-control" type="text" data-plugin-datepicker=""></div>';
                    $('#testidkkk').append(dtpicker);
                    $(document).ready(function () {
                        //$("#dpQuestion_" + questionId).datepicker({
                        //}).on('changeDate', function (e) {
                        //    $(this).datepicker('hide');
                        //    $('#frmquestionDetail').bootstrapValidator('revalidateField', 'DOS');
                        //});
                        utility.CreateDatePicker("#dpQuestion_" + questionId, function () {
                            //  on-change callback method
                        }, false);
                    });
                }
                else if (questionTypeId == 9) {
                    $('#testidkkk').append(newlabel);
                    var img = '<div class="col-xs-6"> <img id="imdIDPreview" class="img-responsive img-center img-thumbnail NoRadius p-xs" src="" alt="Image" /> </div>';
                    $('#testidkkk').append(img);
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });


    },

    FillQuestion: function (questionId, rpp, currPage) {
        var data = "QuestionId=" + questionId + "&PageNo=" + currPage + "&rpp=" + rpp;
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "FILL_QUESTION");
    },

    FillSection: function (sectionId, PageNo, rpp) {
        var QuestionGroupData = "";
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 6;
        }
        var data = "sectionId=" + sectionId + "&PageNo=" + PageNo + "&rpp=" + rpp;
        return MDVisionService.defaultService(data, "CLINICAL_SECTION", "FILL_SECTION");
    },

    GetSelectedRadio: function myfunction(e) {
        //alert('aa');
    },

    UnLoad: function () {
        sectionDetail.GetQuestionGroup_Sections(sectionDetail.params.SectionId, null, null).done(function (response) {
            if (response.SectionQuestionGroupCount > 0) {
                var SectionFill_JSON = JSON.parse(response.SectionFill_JSON);
                $.each(SectionFill_JSON, function (i, item) {
                    var HtmlId = item.QuestionGroupId + '_' + item.QuesGroupSectionId + '_SectionQuestionGroup';
                    sectionDetail.UpdateQuestionGroupFromSection($('#' + HtmlId).html(), item.QuestionGroupId, $('#' + HtmlId).html());
                });

                UnloadActionPan();
                Clinical_Section.SectionSearch();

                //utility.UnLoadDialog('frmsectionDetail', function () {
                //    UnloadActionPan();
                //    Clinical_Section.SectionSearch();
                //}, function () {
                //    UnloadActionPan();
                //    Clinical_Section.SectionSearch();
                //});
            }
            else {
                UnloadActionPan();
                Clinical_Section.SectionSearch();
            }
        });
    },
}

function GetPagingSmallSize(pagingDivId, TotalRecords, DisplayPages, ClassName, PageNo, rpp) {
    var RecordsPerPage = rpp != null ? rpp : 15;
    var CurrentPage = PageNo != null ? PageNo : 1;
    var TotalPages = Math.ceil(TotalRecords / RecordsPerPage);
    var totalPagesToDisplay = TotalPages > 0 ? TotalPages : 1;
    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < TotalRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : TotalRecords;
    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + TotalRecords + " Record(s)";

    if (ClassName != null) {
        ClassName = ClassName + ".";
    }
    else
        ClassName = "";
    $("#" + pagingDivId).attr("class", "col-sm-12");
    if ($("#" + pagingDivId + " #divShowingEntries"))
        $("#" + pagingDivId + " #divShowingEntries").remove();
    if ($("#" + pagingDivId + " #lnkPrevPages"))
        $("#" + pagingDivId + " #lnkPrevPages").remove();
    if ($("#" + pagingDivId + " #lnkNextPages"))
        $("#" + pagingDivId + " #lnkNextPages").remove();
    if ($("#" + pagingDivId + " #preLink"))
        $("#" + pagingDivId + " #preLink").remove();
    if ($("#" + pagingDivId + " #pageslink"))
        $("#" + pagingDivId + " #pageslink").remove();
    if ($("#" + pagingDivId + " #nextLink"))
        $("#" + pagingDivId + " #nextLink").remove();
    if ($("#" + pagingDivId + " #pagerParent"))
        $("#" + pagingDivId + " #pagerParent").remove();
    var ShowingEntriesDiv = '<div class="col-sm-6 pl-none" id="divShowingEntries">' + showingText + '</div>';
    var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" class="p-xs pt-none pb-none" href="#null" title="Previous Pages" onclick="PreviousClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-left"></i></a></li>';
    var nextChunck = '<li id="nextLink"><a id="lnkNextPages"  class="p-xs pt-none pb-none" href="#null" title="Next Pages" onclick="NextClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-right"></i></a></li>';
    var prevLink = '<li id="preLink" ><a id="lnkPrev" class="p-xs pt-none pb-none" href="#null" title="Previous Page" onclick="' + ClassName + 'PreviousPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-left"></i></a></li>';
    var nextLink = '<li id="nextLink"><a id="lnkNext" class="p-xs pt-none pb-none" href="#null" title="Next Page" onclick="' + ClassName + 'NextPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-right"></i></a></li>';
    var pagesLinkDiv = '<ul class="pagination pull-right m-none" >';
    pagesLinkDiv += prevChunck;
    pagesLinkDiv += prevLink;
    var noOfDisplayPages = TotalPages < DisplayPages ? TotalPages : DisplayPages;
    for (i = 1 ; i <= noOfDisplayPages ; i++) {
        pagesLinkDiv += '<li><a href="#" class="p-xs pt-none pb-none" onclick="' + ClassName + 'SelectedPageClick(' + i + ',this,' + TotalRecords + ',' + rpp + ');">' + i + '</a></li>';
    }
    pagesLinkDiv += nextLink;
    pagesLinkDiv += nextChunck;
    pagesLinkDiv += '</ul>';
    var pagerParent = '<div id="pagerParent" class="col-sm-6">' + pagesLinkDiv + '</div>';
    $("#" + pagingDivId).append(ShowingEntriesDiv, pagerParent);
}

function NextClickSmallSizePaging(TotalPages, DisplayPages, pagingDivId, ClassName) {
    //pagingDivId=$(pagesLinkDiv).attr("id");
    var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" class="p-xs pt-none pb-none" href="#null" title="Previous Pages" onclick="PreviousClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-left"></i></a></li>';
    var nextChunck = '<li id="nextLink"><a id="lnkNextPages"  class="p-xs pt-none pb-none" href="#null" title="Next Pages" onclick="NextClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-right"></i></a></li>';
    var prevLink = '<li id="preLink" ><a id="lnkPrev" class="p-xs pt-none pb-none" href="#null" title="Previous Page" onclick="' + ClassName + 'PreviousPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-left"></i></a></li>';
    var nextLink = '<li id="nextLink"><a id="lnkNext" class="p-xs pt-none pb-none" href="#null" title="Next Page" onclick="' + ClassName + 'NextPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-right"></i></a></li>';
    var pagesLinkDiv = '<ul class="pagination pull-right m-none" >';
    pagesLinkDiv += prevChunck;
    pagesLinkDiv += prevLink;
    start = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
    var TotalLi = $('#' + pagingDivId + ' #pagerParent ul li').length;
    last = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(' + (TotalLi - 3) + ')').text());
    if (last < TotalPages) {
        last = last + 1;
        //$("#pageslink").empty();
        var pagesLinks = "";
        var currentPageNo = 1;
        for (i = last ; i <= TotalPages && (currentPageNo <= DisplayPages) ; i++) {
            pagesLinkDiv += '<li><a href="#"  class="p-xs pt-none pb-none" onclick="' + ClassName + 'SelectedPageClick(' + i + ',this);">' + i + '</a></li>';
            currentPageNo += 1;
        }

        pagesLinkDiv += nextLink;
        pagesLinkDiv += nextChunck;
        pagesLinkDiv += '</ul>';
        //var pagerParent = '<div id="pagerParent" class="col-sm-6">' + pagesLinkDiv + '</div>';
        $("#pagerParent").html(pagesLinkDiv);
        setTimeout(function () {
            $('#pagerParent ul li:eq(2)').trigger("click");
        }, 100);
    }
}

function PreviousClickSmallSizePaging(TotalPages, DisplayPages, pagingDivId, ClassName) {
    var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" class="p-xs pt-none pb-none" href="#null" title="Previous Pages" onclick="PreviousClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-left"></i></a></li>';
    var nextChunck = '<li id="nextLink"><a id="lnkNextPages"  class="p-xs pt-none pb-none" href="#null" title="Next Pages" onclick="NextClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-right"></i></a></li>';
    var prevLink = '<li id="preLink" ><a id="lnkPrev" class="p-xs pt-none pb-none" href="#null" title="Previous Page" onclick="' + ClassName + 'PreviousPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-left"></i></a></li>';
    var nextLink = '<li id="nextLink"><a id="lnkNext" class="p-xs pt-none pb-none" href="#null" title="Next Page" onclick="' + ClassName + 'NextPageClick(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\');"><i class="fa fa-angle-right"></i></a></li>';
    var pagesLinkDiv = '<ul class="pagination pull-right m-none" >';
    pagesLinkDiv += prevChunck;
    pagesLinkDiv += prevLink;
    start = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(2)').text());
    var TotalLi = $('#' + pagingDivId + ' #pagerParent ul li').length;
    last = parseInt($('#' + pagingDivId + ' #pagerParent ul li:eq(' + (TotalLi - 3) + ')').text());
    DisplayPages = parseInt(DisplayPages);
    last = last + 1;
    if (start > 1) {
        for (i = start - DisplayPages ; i < start  ; i++) {
            if (i > 0) {
                pagesLinkDiv += '<li><a href="#"  class="p-xs pt-none pb-none" onclick="' + ClassName + 'SelectedPageClick(' + i + ',this);">' + i + '</a></li>';
            }
        }
        pagesLinkDiv += nextLink;
        pagesLinkDiv += nextChunck;
        pagesLinkDiv += '</ul>';
        $("#pagerParent").html(pagesLinkDiv);
    }

}
