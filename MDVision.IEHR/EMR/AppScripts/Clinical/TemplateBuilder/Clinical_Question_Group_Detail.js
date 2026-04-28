questionGroupDetail = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        questionGroupDetail.params = params;
        if (questionGroupDetail.params["PanelID"] != "questionGroupDetail") {
            questionGroupDetail.params["PanelID"] = questionGroupDetail.params["PanelID"] + ' #questionGroupDetail';
        }

        if (questionGroupDetail.bIsFirstLoad) {
            questionGroupDetail.bIsFirstLoad = false;
            //  $('#QuestionsDetailsDiv').attr('disabled', true);
            $("div#QuestionsDetailsDiv *").attr('disabled', true);
            var self = $('#' + questionGroupDetail.params["PanelID"]);

            self.loadDropDowns(true).done(function () {

                questionGroupDetail.LoadQuestionGroup();
            });
            $("#" + questionGroupDetail.params["PanelID"] + " #txtRowspPageHF").val(6);
            $("#" + questionGroupDetail.params["PanelID"] + " #txtPageNoHF").val(1);
            $("#" + questionGroupDetail.params["PanelID"] + " #SearchQuestiontxt").keyup(function () {
                if ($("#" + questionGroupDetail.params["PanelID"] + " #SearchQuestiontxt").val().length > 3 || $("#" + questionGroupDetail.params["PanelID"] + " #SearchQuestiontxt").val().length < 1) {
                    questionGroupDetail.QuestionsSearch();
                }

            });

        }
        questionGroupDetail.HideAllControls();
    },

    LoadQuestionGroup: function () {
        PageNo = null;
        rpp = null;
        // $("#" + questionGroupDetail.params["PanelID"] + " #pnlQuestionsinfo").removeClass('disableAll');
        if (questionGroupDetail.params.mode == "Add") {
            //   $('#' + questionGroupDetail.params["PanelID"] + ' #txtShortName').attr("enabled", "enabled");

            $("#" + questionGroupDetail.params["PanelID"] + " #pnlQuestionsinfo").addClass('disableAll');

            questionGroupDetail.ValidationQuestionGroup();

            //Serialize data
            $('#' + questionGroupDetail.params["PanelID"] + ' #frmQuestionGroupDetail').data('serialize', $('#' + questionGroupDetail.params["PanelID"] + ' #frmQuestionGroupDetail').serialize());
        }
        else if (questionGroupDetail.params.mode == "Edit") {
            //  $('#' + questionGroupDetail.params["PanelID"] + ' #txtShortName').attr("disabled", "disabled");

            questionGroupDetail.FillQuestionGroup(questionGroupDetail.params.QuestionGroupId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    var QuestionGroup_detail = JSON.parse(response.QuestionGroupFill_JSON);
                    var self = $("#" + questionGroupDetail.params["PanelID"]);
                    utility.bindMyJSON(true, QuestionGroup_detail[0], false, self).done(function () {
                        if (QuestionGroup_detail[0].IsActive == "True") {
                            $("#" + questionGroupDetail.params["PanelID"] + " #Active").prop("checked", true);
                        } else {
                            $("#" + questionGroupDetail.params["PanelID"] + " #Active").prop("checked", false);
                        }

                        if (QuestionGroup_detail.ddlEntity == null || QuestionGroup_detail.ddlEntity == "") {
                            $('#' + questionGroupDetail.params["PanelID"] + ' #txtQuestionGroup').attr("disabled", "disabled");
                        }

                        $("#" + questionGroupDetail.params["PanelID"] + " #pnlQuestionsinfo").removeClass('disableAll');
                        questionGroupDetail.ValidationQuestionGroup();

                      
                        $('#Canvas,#ShortName,#SpecialityID').attr('disabled', true);
                        questionGroupDetail.QuestionsSearch(null, null, 6);

                        $(function () {

                            $('#SearchQuestiontxt').keypress(function (e) {
                                var keycode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
                                if (keycode == 13) {
                                    questionGroupDetail.QuestionsSearch();
                                }
                            });
                        });
                        if (QuestionGroup_detail[0].HTMLTemplate != '' && QuestionGroup_detail[0].HTMLTemplate != null) {
                            HTMLQuestionGroup = QuestionGroup_detail[0].HTMLTemplate;
                            //HTMLQuestionGroup = HTMLQuestionGroup.replace('&lt;', '<');

                            HTMLQuestionGroup = HTMLQuestionGroup.replace(/&quot;/g, '"');
                            HTMLQuestionGroup = HTMLQuestionGroup.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                            //var aHTMLQuestionGroup = HTMLQuestionGroup.find('src =').split();
                            // Parsed response from Server Side
                            $('#' + questionGroupDetail.params["PanelID"] + ' #HTMLTempalteQuestionGroup').html(HTMLQuestionGroup);
                        } else {
                            $('#HTMLTempalteQuestionGroup').html('');
                            if (QuestionGroup_detail[0].Canvas > 1) {
                                for (var i = 0; i < QuestionGroup_detail[0].Canvas; i++) {
                                    $('#HTMLTempalteQuestionGroup').append('<div class="col-sm-' + 12 / QuestionGroup_detail[0].Canvas + ' questiondrop question-parrent ui-droppable"></div>');
                                }
                            } else {
                                $('#HTMLTempalteQuestionGroup').append('<div class="col-sm-12 questiondrop question-parrent ui-droppable"></div>');
                            }

                        }

                        questionGroupDetail.GetQuestions_QuestionGroup(questionGroupDetail.params.QuestionGroupId, null, null).done(function (response) {
                            if (response.QuestionGroupQuestionCount > 0) {
                                var QuestionGroupFill_JSON = JSON.parse(response.QuestionGroupFill_JSON);
                                var FirstInsuranceId = "";
                                var UpdatedQuestoins = "";
                                var QuesQuestionListInfo = [];

                                $.each(QuestionGroupFill_JSON, function (i, item) {
                                    //Set ToolTip for Comments.
                                    $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

                                    var divID = item.QuesGroupQuestionId + '_' + item.QuestionID + '_' + item.QuestionTypeId;
                                    //QuestionHeading QuestionType  var divID = response.QuesGroupQuestionId + '_' + QuestionId + '_' + questionTypeId;
                                    var QuestionListedHTML = '<li class="bg-primary" id="' + item.QuesGroupQuestionId + '"><a href="#" onclick="questionGroupDetail.getQuestionsData(' + item.QuestionID + ',' + item.QuestionTypeId + ',1,1' + ');"><span class="ui-icon ui-icon-arrow-4-diag"></span>' +
                                                       '<div class="col-xs-11 p-none pull-left"><p class="white size90per ellipses"  data-toggle="tooltip" data-placement="top" title="' + item.QuestionDescription + '">' + item.QuestionDescription + '</p></div></a>' +
                                                       '<div class="col-xs-1 pl-none pr-xs pull-right"><a href="#"></a><a class="btn btn-close btn-xs" onclick="questionGroupDetail.DeleteQuestionFromQuestionGroup(' + item.QuesGroupQuestionId + ',\'' + divID + '\');">' +
                                                       '<i class="fa fa-times"></i></a> </div></li>';
                                    $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDiv').append(QuestionListedHTML);
                                    if (item.IsUpdated == "0") {
                                        var QuesQuestionObjec = {};
                                        QuesQuestionObjec.QuestionID = item.QuestionID;
                                        QuesQuestionObjec.QuesGroupQuestionId = item.QuesGroupQuestionId;
                                        QuesQuestionObjec.QuestionTypeId = item.QuestionTypeId;
                                        QuesQuestionListInfo.push(QuesQuestionObjec);
                                    }

                                });
                                if (QuesQuestionListInfo != null && QuesQuestionListInfo.length > 0) {
                                    questionGroupDetail.UpdateQuestionsInQuestionGroup(QuesQuestionListInfo);
                                }
                                $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');
                                //for (var i = 0; i < response.QuestionGroupQuestionCount; i++) {
                                //    $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDivList').append('<li>' + (i + 1) + '</li>');
                                //}
                                $(document).ready(function () {
                                    $("#SelectedQuestionDiv li").click(function () {
                                        $("#SelectedQuestionDiv li").addClass("bg-primary");
                                        $(this).removeClass("bg-primary");
                                        $(this).addClass("bg-success");
                                    });
                                });
                            }
                        });
                        
                        setTimeout(function () {
                            //Serialize data
                            $('#' + questionGroupDetail.params["PanelID"] + ' #frmQuestionGroupDetail').data('serialize', $('#' + questionGroupDetail.params["PanelID"] + ' #frmQuestionGroupDetail').serialize());
                        }, 1000);
                       
                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    UpdateQuestionsInQuestionGroup: function (QuesQuestionListInfo) {
        //   utility.myConfirm('Question was updated after last modified of Question Group, Do you want to get Updated Question?', function () {
        $.each(QuesQuestionListInfo, function (i, item) {

            var divID = item.QuesGroupQuestionId + '_' + item.QuestionID + '_' + item.QuestionTypeId;
            //getting Information Of Question Dropped                                      
            var QuestionId = item.QuestionID;
            var myJSON = '{TextSearched :"' + $('#SearchQuestiontxt').val() + '"}';
            var RefID = $(this);
            questionGroupDetail.SearchQuestion(myJSON, QuestionId, 1, 15).done(function (response) {
                // questionGroupDetail.InsertQuestioninQuestionGroup(myJSON, QuestionId, 1, 1).done(function (response) {
                if (response.status != false) {
                    var obj = jQuery.parseJSON(response.QuestionLoad_JSON);
                    $('#' + item.QuesGroupQuestionId).removeClass("bg-primary");
                    $('#' + item.QuesGroupQuestionId).addClass("bg-warning");
                    $('#' + divID).html('');
                    $('#' + divID).replaceWith(questionGroupDetail.DroppedQuestionInsertionInCanvas(obj, response, item.QuesGroupQuestionId));
                    var self = $("#" + questionGroupDetail.params["PanelID"]);
                    //  var myJSON = self.getMyJSON();
                    questionGroupDetail.UpdateQuestionGroup(null, questionGroupDetail.params.QuestionGroupId);
                    questionGroupDetail.UpdateQuestionGroupQUESTION(item.QuesGroupQuestionId, QuestionId);

                }
            });
        });
        // }, function () { }, 'Question Updated');
    },

    UpdateQuestionGroupQUESTION: function (QuesGroupQuestionId, QuestionId) {

        var data = "QuesGroupQuestionId=" + QuesGroupQuestionId + "&QuestionId=" + QuestionId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Clinical_Question_Group", "UPDATE_QUESTION_IN_QUESTIONGROUP");
    },

    ValidationQuestionGroup: function () {
        $('#frmQuestionGroupDetail')
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
           questionGroupDetail.QuestionGroupSave();
       });
    },

    QuestionGroupSave: function () {
        var strMessage = "";
        var self = $("#" + questionGroupDetail.params["PanelID"]);
        var myJSON = self.getMyJSON();
        if (questionGroupDetail.params.mode == "Add") {
            //  AppPrivileges.GetFormPrivileges("CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (questionGroupDetail.params.QuestionGroupId == "-1") {
                    questionGroupDetail.SaveQuestionGroup(myJSON).done(function (response) {
                        if (response.status != false) {
                            $("#" + questionGroupDetail.params["PanelID"] + " #pnlQuestionsinfo").removeClass('disableAll');
                            questionGroupDetail.params.QuestionGroupId = response.QuestionGroupId;
                            Clinical_Question_Group.QuestionGroupSearch();
                            utility.DisplayMessages(response.message, 1);
                            $("#" + questionGroupDetail.params["PanelID"] + " #pnlQuestionsinfo").removeClass('disableAll');
                            questionGroupDetail.ValidationQuestionGroup();



                            $('#Canvas,#ShortName,#SpecialityID').attr('disabled', true);
                            questionGroupDetail.QuestionsSearch(null, null, 6);
                            var NumberOfCanvasColumns = Number($('#Canvas').val());
                            if (NumberOfCanvasColumns > 1) {
                                for (var i = 0; i < NumberOfCanvasColumns; i++) {
                                    $('#HTMLTempalteQuestionGroup').append('<div class="col-sm-' + 12 / NumberOfCanvasColumns + ' questiondrop question-parrent ui-droppable"></div>');
                                }
                            } else {
                                $('#HTMLTempalteQuestionGroup').append('<div class="col-sm-12 questiondrop question-parrent ui-droppable"></div>');
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else if (questionGroupDetail.params.QuestionGroupId != "-1" && questionGroupDetail.params.QuestionGroupId != "" && questionGroupDetail.params.QuestionGroupId != "0") {
                    questionGroupDetail.UpdateQuestionGroup(myJSON, questionGroupDetail.params.QuestionGroupId).done(function (response) {
                        if (response.status != false) {
                            questionGroupDetail.QuestionsSearch(questionGroupDetail.params.QuestionGroupId);
                            utility.DisplayMessages(response.message, 1);

                            UnloadActionPan();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }
            //    else
            //        utility.DisplayMessages(strMessage, 2);
            //});
        }
        else if (questionGroupDetail.params.mode == "Edit") {
            // AppPrivileges.GetFormPrivileges("CPT", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                questionGroupDetail.UpdateQuestionGroup(myJSON, questionGroupDetail.params.QuestionGroupId, $('#' + questionGroupDetail.params["PanelID"] + ' #HTMLTempalteQuestionGroup').html()).done(function (response) {
                    if (response.status != false) {
                        Clinical_Question_Group.QuestionGroupSearch(0, 1, 15);
                        utility.DisplayMessages(response.message, 1);

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
    },

    UpdateQuestionGroup: function (QuestionGroupData, QuestionGroupId, UpdatedHTMLforQuestionGroup) {

        var data = "UpdatedHTMLforQuestionGroup=" + UpdatedHTMLforQuestionGroup + "&QuestionGroupData=" + QuestionGroupData + "&QuestionGroupId=" + QuestionGroupId + "&HTMLTempalteQuestionGroup=" + (QuestionGroupData != null ? null : $('#' + questionGroupDetail.params["PanelID"] + ' #HTMLTempalteQuestionGroup').html());
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Clinical_Question_Group", "UPDATE_QUESTION_GROUP");
    },

    DeleteQuestionFromQuestionGroup: function (QuesGroupQuestionId, divID) {
        utility.myConfirm('1', function () {
            var selectedValue = QuesGroupQuestionId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                questionGroupDetail.DeleteQuestionGroupQuestion(null, selectedValue).done(function (response) {
                    if (response.status != false) {
                        $('#' + divID).remove();
                        $('#' + QuesGroupQuestionId).remove();
                        //alert(questionGroupDetail.params.QuestionGroupId);

                        $('#' + questionGroupDetail.params.QuestionGroupId + '_SectionQuestionGroup #' + divID).remove();
                        $('#SelectedQuestionDiv').find('li#' + selectedValue).remove();

                        $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDiv').find('li#' + selectedValue).remove();
                        $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');
                        //for (var i = 0; i < $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDiv').find('li').length; i++) {
                        //    $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDivList').append('<li>' + (i + 1) + '</li>');
                        //}
                        var self = $("#" + questionGroupDetail.params["PanelID"]);
                        //if (self == )
                        var myJSON = self.getMyJSON();
                        questionGroupDetail.UpdateQuestionGroup(null, questionGroupDetail.params.QuestionGroupId, $('#' + questionGroupDetail.params.QuestionGroupId + '_SectionQuestionGroup').html());
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
        var data = "QuestionID=" + QuestionID + "&QUESTIONGROUPID=" + QuestionGroupId;//$('#' + questionGroupDetail.params["PanelID"] + ' #QuestionGroupID').val();
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Clinical_Question_Group", "DELETE_QUESTIONFROM_QUESTION_GROUP");
    },

    SaveQuestionGroup: function (QuestionGroupData) {
        var data = "QuestionGroupData=" + QuestionGroupData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Clinical_Question_Group", "SAVE_QUESTION_GROUP");
    },

    GetQuestions_QuestionGroup: function (QuestionGroupId, QuesGroupQuestionId, fieldsJSON) {
        var fieldsJSON = "";
        if (QuestionGroupId == null) {
            QuestionGroupId = 0;
        }
        if (QuesGroupQuestionId == null) {
            QuesGroupQuestionId = 0;
        }

        var data = "fieldsJSON=" + fieldsJSON + "&QuestionGroupId=" + QuestionGroupId + "&QuesGroupQuestionId=" + QuesGroupQuestionId;

        return MDVisionService.defaultService(data, "Clinical_Question_Group", "GET_QUESTIONS_OF_QUESTIONGROUP");
    },//GET_QUESTIONS_OF_QUESTIONGROUP

    FillQuestionGroup: function (QuestionGroupId, PageNo, rpp) {
        var QuestionGroupData = "";
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 6;
        }
        if (QuestionGroupData == null) {
            QuestionGroupData = "";
        }
        var data = "QuestionGroupData=" + QuestionGroupData + "&QuestionGroupId=" + QuestionGroupId + "&PageNo=" + PageNo + "&rpp=" + rpp;

        return MDVisionService.defaultService(data, "Clinical_Question_Group", "SEARCH_QUESTION_GROUP");
    },

    UnLoad: function () {

        utility.UnLoadDialog('#' + questionGroupDetail.params["PanelID"] + ' #frmQuestionGroupDetail', function () {
            UnloadActionPan(questionGroupDetail.params["ParentCtrl"], "questionGroupDetail");
        }, function () {
            UnloadActionPan(questionGroupDetail.params["ParentCtrl"], "questionGroupDetail");
        });
    },

    QuestionsSearch: function (questionID, PageNo, rpp) {
        var QuestionId = "";
        var myJSON = '{TextSearched :"' + $('#SearchQuestiontxt').val() + '"}';
        questionGroupDetail.SearchQuestion(myJSON, QuestionId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                questionGroupDetail.QuestionListLoad(response, PageNo, rpp);

            }
        });
        // }
    },

    SearchQuestion: function (QuestionData, questionID, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 6;
        }
        var data = "QuestionData=" + QuestionData + "&questionID=" + questionID + "&PageNo=" + PageNo + "&rpp=" + rpp;

        return MDVisionService.defaultService(data, "Clinical_Question_Group", "SEARCH_QUESTION");
    },

    InsertQuestioninQuestionGroup: function (QuestionData, questionID, PageNo, rpp) {
        if (PageNo == null) {
            PageNo = 1;
        }
        if (rpp == null) {
            rpp = 6;
        }

        var data = "questionID=" + questionID + "&QUESTIONGROUPID=" + questionGroupDetail.params.QuestionGroupId;

        return MDVisionService.defaultService(data, "Clinical_Question_Group", "INSERT_QUESTION_IN_QUESTIONGROUP");
    },

    QuestionListLoad: function (response, PageNo, rpp) {
        $("#lstQuestionGroup").empty();
        
        if (response.QuestionCount > 0) {
            var QuestionLoadJSONData = JSON.parse(response.QuestionLoad_JSON);
            var FirstInsuranceId = "";
            $.each(QuestionLoadJSONData, function (i, item) {

                var bgcolor = "";
                if (item.IsActive != "True") {
                    bgcolor = "bg-danger";
                }

                var lstQuestionGroup = '<div style="z-index:2000" class="bg-primary questiondrag  mb-tiny ' + bgcolor + '" id="' + item.QuestionId + '_' + item.QuestionTypeId + '" ><a href="#"><div class="col-xs-1 size-max10 p-none"><span class="ui-icon ui-icon-arrow-4-diag mt-tiny pull-left"></span></div><div class="col-xs-8 col-lg-7 pr-none"><p class="white size100per ellipses m-none" data-toggle="tooltip" data-placement="top" title="' + item.Description + '">' + item.Description + '</p></div><div class="col-xs-3 col-lg-4 pull-right p-none"><p class="white m-none size95per ellipses text-right" data-toggle="tooltip" data-placement="top" title="' + item.Description1 + '">' + item.Description1 + '</p></div><div class="clearfix"></div></a></div>';
                $("#lstQuestionGroup").last().append(lstQuestionGroup);

            });

            $("#" + questionGroupDetail.params["PanelID"] + " #divQuestionlistPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var RecordsPerPage = rpp != null ? rpp : 6;
            var CurrentPage = PageNo != null ? PageNo : 1;
            var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
            var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
            if (PageNo == null) {
                questionGroupDetail.GetCustomPaging("divQuestionlistPaging", response.iTotalDisplayRecords, 2, "questionGroupDetail", CurrentPage, RecordsPerPage);
            }

            var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
            TotalQuestions = response.iTotalDisplayRecords;
            $('#pnlClinicalQuestionGroup #txtTotalQuestions').val(TotalQuestions);
            questionGroupDetail.ShowHideListGrid(CurrentPage, RecordsPerPage, TotalQuestions);


            $(".questiondrag").draggable({
                revert: true,
                appendTo: 'body',
                helper: 'clone',
                stack: '.questiondrop',
                start: function () {
                    // $(this).css('z-index', '9999');
                    $(this).css('z-index', '2000');
                    //  $(this).css('position', 'absolute');
                },
                stop: function () {
                    //  $(ui.helper).clone(true);
                    // $(this).css('z-index', '0');
                    $(this).css('z-index', '2000');
                }
            });


            $(".questiondrop").droppable({
                accept: ".questiondrag",
                activeClass: 'droppable-active',
                hoverClass: 'droppable-hover',
                drop: function (ev, ui) {
                    //getting Information Of Question Dropped
                    var QuestionText = ui.draggable.find('.white').html();
                    var Ques_QuestionType = ui.draggable.attr('id').split('_');
                    var QuestionId = Ques_QuestionType[0];
                    var questionTypeId = Ques_QuestionType[1];

                    var myJSON = '{TextSearched :"' + $('#SearchQuestiontxt').val() + '"}';
                    var RefID = $(this);
                    //sectionDetail.FillQuestion(QuestionId, 1, 1).done(function (response) {
                    //    if (response.status != false) {
                    //        var Question_detail = JSON.parse(response.QuestionLoad_JSON);
                    //        var imgStream = Question_detail.imdIDPreview;
                    questionGroupDetail.InsertQuestioninQuestionGroup(myJSON, QuestionId, 1, 1).done(function (response) {
                        if (response.status != false) {
                            var obj = jQuery.parseJSON(response.QuestionLoad_JSON);
                            $(RefID).append(questionGroupDetail.DroppedQuestionInsertionInCanvas(obj, response, response.QuesGroupQuestionId));
                            questionGroupDetail.getQuestionsData(QuestionId, questionTypeId, 1, 1);
                            var divID = response.QuesGroupQuestionId + '_' + QuestionId + '_' + questionTypeId;
                            $("#SelectedQuestionDiv li").addClass("bg-primary");
                            var QuestionListedHTML = '<li class="bg-success" id="' + response.QuesGroupQuestionId + '"><a href="#" onclick="questionGroupDetail.getQuestionsData(' + QuestionId + ',' + questionTypeId + ',1,1' + ');"><span class="ui-icon ui-icon-arrow-4-diag"></span>' +
                                                   '<div class="col-xs-11 p-none pull-left"><p class="white size90per ellipses" data-toggle="tooltip" data-placement="top" title="' + QuestionText + '">' + QuestionText + '</p></div></a>' +
                                                   '<div class="col-xs-1 pl-none pr-xs pull-right"><a href="#"></a><a class="btn btn-close btn-xs" onclick="questionGroupDetail.DeleteQuestionFromQuestionGroup(' + response.QuesGroupQuestionId + ',\'' + divID + '\');">' +
                                                   '<i class="fa fa-times"></i></a> </div></li>';
                            $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDiv').append(QuestionListedHTML);
                            $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDivList').html('');



                            var self = $("#" + questionGroupDetail.params["PanelID"]);
                            //  var myJSON = self.getMyJSON();
                            questionGroupDetail.UpdateQuestionGroup(null, questionGroupDetail.params.QuestionGroupId);
                            //for (var i = 0; i < $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDiv').find('li').length; i++) {
                            //    $('#' + questionGroupDetail.params["PanelID"] + ' #SelectedQuestionDivList').append('<li>' + (i + 1) + '</li>');
                            //}
                        }
                    });
                    //    }
                    //});
                }

            });

        } else {
            questionGroupDetail.ShowHideListGrid(0, 0, 0)
        }
    },

    DroppedQuestionInsertionInCanvas: function (obj, response, QuesGroupQuestionId) {
        QuesGroupQuestionId = typeof response.QuesGroupQuestionId == "undefined" ? QuesGroupQuestionId : response.QuesGroupQuestionId;
        var QuestionText = obj[0].Description;
        var QuestionId = obj[0].QuestionId;
        var questionTypeId = obj[0].QuestionTypeId;
        var $ctrl = "";
        if (questionTypeId == 1 || questionTypeId == 10) {
            $ctrl = $('<input/>').attr({ type: 'text', name: 'text', id: 'TextField_' + QuestionId + '_' + QuesGroupQuestionId }).addClass("form-control");
        }
        else if (questionTypeId == 2) {
            $ctrl = '<div class="col-xs-6 radio-custom radio-small"><input type="radio" id="RadioTrue_ ' + QuestionId + '_' + QuesGroupQuestionId + '" name="r2_' + QuesGroupQuestionId + '"checked="checked"><label id="LabelRadioTrue_ ' + QuestionId + '_' + QuesGroupQuestionId + '" class="control-label" for="">' + obj[0].BoolTrueDisplay + '</label></div><div class="col-xs-6 radio-custom radio-small"><input type="radio" name="r2_' + QuesGroupQuestionId + '" id="RadioFalse_' + QuestionId + '_' + QuesGroupQuestionId + '"><label id="LabelRadioFalse_' + QuestionId + '_' + QuesGroupQuestionId + '" class="control-label" for="">' + obj[0].BoolFalseDisplay + '</label></div>';
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
            var $ctrl = $('<select />').attr({ "id": 'Select_' + QuestionId + '_' + QuesGroupQuestionId, "type": 'select' }).addClass("form-control");
            for (var val in data) {
                $('<option />', { value: val, text: data[val] }).appendTo($ctrl);
            }
        }
        else if (questionTypeId == 5) {
            $ctrl = '<div class="col-xs-5 p-none size-min50"><input type="text" id="FractionField1_' + QuestionId + '_' + QuesGroupQuestionId + '" class="form-control text-center"></div><div class="col-xs-2 text-center"><h4 class="m-none">/</h4></div><div class="col-xs-5 p-none size-min50"> <input type="text" id="FractionField2_' + QuestionId + '_' + QuesGroupQuestionId + '" class="form-control text-center"></div>';
        }
        else if (questionTypeId == 6) {
            $ctrl = '<div class="input-group"><span class="input-group-addon"><i class="fa fa-calendar"></i></span><input id="dpQuestion_' + QuestionId + '_' + QuesGroupQuestionId + '" class="form-control" type="text" data-plugin-datepicker=""></div>';
        }
        else if (questionTypeId == 7) {
            $ctrl = '<div class="input-group"><span class="input-group-addon"> <i class="fa fa-clock-o"></i> </span><input id="tpQuestion_' + QuestionId + '_' + QuesGroupQuestionId + '" type="text" data-plugin-timepicker="" data-plugin-options="" class="form-control"></div>';
        }
        else if (questionTypeId == 9) {

            //sectionDetail.FillQuestion(QuestionId, 1, 1).done(function (response) {
            //    if (response.status != false) {
            //        var Question_detail = JSON.parse(response.QuestionLoad_JSON);
            //        $ctrl = '<div class="col-xs-6"> <img id="imgField_' + QuestionId + '" type="img" class="img-responsive img-center img-thumbnail NoRadius p-xs" src="' + Question_detail.imdIDPreview + '" alt="Image" /> </div>';
            //    }
            //});
            $ctrl = '<img id="imgField_' + QuestionId + '_' + QuesGroupQuestionId + '" type="img" class="img-responsive img-center img-thumbnail NoRadius p-xs" src="" alt="Image" />';
            //$ctrl = '<div class="col-xs-6"> <img id="imgField_' + QuestionId + '_' + QuesGroupQuestionId + '" class="img-responsive img-center img-thumbnail NoRadius p-xs" src="" alt="Image" /> </div>';
        }



        var divID = QuesGroupQuestionId + '_' + QuestionId + '_' + questionTypeId;
        //HTML Creation of Question

        var QuestionHTMLDroppedStartDiv = '<div class="col-xs-12 question-child questiondrag" id="' + divID + '">';
        var QuestionHTMLHeading = '<button aria-hidden="true" class="fa fa-times noBorder pull-right closeQuestion" type="button" id="' + QuestionId + '" onclick="questionGroupDetail.DeleteQuestionFromQuestionGroup(' + QuesGroupQuestionId + ',\'' + divID + '\');"></button><h5 class="QuestionHeading">' + obj[0].Description + '</h5>';
        var QuestionTypeHTML = '<div class=" col-xs-5 p-none size-min90 QuestionType"><label class="control-label">' + obj[0].Description1 + '</label></div>';
        var QuestionDetailHTML = '<div class="col-xs-7 col-sm-12 col-lg-7 p-none size-min90">' + ((typeof $ctrl[0].outerHTML === "undefined") ? $ctrl : $ctrl[0].outerHTML) + '</div>';
        var QuestionHTMLDroppedendtDiv = '</div>';
        //End Html Creation of QUestion

        $(document).ready(function () {
           
            $("#SelectedQuestionDiv li").click(function () {
                $("#SelectedQuestionDiv li").addClass("bg-primary");
                $(this).removeClass("bg-primary");
                $(this).addClass("bg-success");
            });
        });


        return QuestionHTMLDroppedStartDiv + QuestionHTMLHeading + QuestionTypeHTML + QuestionDetailHTML + QuestionHTMLDroppedendtDiv;
    },

    HideAllControls: function () {
        $('div[id^="divAnswer"]').hide();
    },

    ShowHideListGrid: function (CurrentPage, RecordsPerPage, TotalQuestions) {
        $("#lstQuestionGroup").find('li').hide();
        for (var i = (RecordsPerPage * CurrentPage) - 5; i < (RecordsPerPage * CurrentPage) + RecordsPerPage - 5; i++) {
            $("#lstQuestionGroup").find('li:nth-child(' + i + ')').show();
        }
        var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < TotalQuestions ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : TotalQuestions;
        if (TotalQuestions == null) {
            TotalQuestions = $("#lstQuestionGroup").find('li').length;
        }
        var showingText = "Showing " + CurrentPage > 0 ? ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) : CurrentPage + " to " + toRecords + " of " + TotalQuestions + " Record(s)";
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

    getQuestionsData: function (questionId, questionTypeId, rpp, currPage) {
        sectionDetail.FillQuestion(questionId, rpp, currPage).done(function (response) {
            if (response.status != false) {
                var Question_detail = JSON.parse(response.QuestionLoad_JSON);
                if (questionTypeId == 1) {

                    questionDetail.HideAllControls();
                    $('#divAnswerTextFieldAnswer').show();
                    if (Question_detail.chkAutoComplete == "True") {
                        $("#" + questionGroupDetail.params["PanelID"] + " #chkAutoComplete").prop("checked", true);
                    } else {
                        $("#" + questionGroupDetail.params["PanelID"] + " #chkAutoComplete").prop("checked", false);
                    }
                    if (Question_detail.radSingleLine == "True") {
                        $("#" + questionGroupDetail.params["PanelID"] + " #radSingleLine").attr('checked', 'checked');
                    } else {
                        $("#" + questionGroupDetail.params["PanelID"] + " #radMultiLine").attr('checked', 'checked');
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

    SelectedPageClick: function (PageNo, objPage) {
        questionGroupDetail.ShowHideListGrid(PageNo, 6);
        questionGroupDetail.QuestionsSearch(null, PageNo, 6);
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
            //  questionGroupDetail.ShowHideListGrid(currentPageNo, 6, null);
            questionGroupDetail.QuestionsSearch(null, currentPageNo, 6);
        }
        var LastPageNo = $("#divQuestionlistPaging li:nth-child(3)").text();
        if (currentPageNo < Number(LastPageNo)) {
            questionGroupDetail.PreviousClickSmallSizePaging(TotalPages, DisplayPages, pagingDivId, "questionGroupDetail.",true);
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
            //questionGroupDetail.ShowHideListGrid(currentPageNo, 6, null);
            questionGroupDetail.QuestionsSearch(null, currentPageNo, 6);
        }
        if (currentPageNo > Number(LastPageNo)) {
            questionGroupDetail.NextClickSmallSizePaging(TotalPages, DisplayPages, pagingDivId, "questionGroupDetail.",true);
            setTimeout(function () { $("#divQuestionlistPaging li:nth-child(3)").attr("class", "active"); }, 200
            );
        }
    },

    GetCustomPaging: function (pagingDivId, TotalRecords, PagesToDisplay, ClassName, PageNo, rpp) {
        questionGroupDetail.GetPagingSmallSize(pagingDivId, TotalRecords, PagesToDisplay, ClassName, PageNo, rpp);
    },

    /////////////////--------------------//////////////////

    GetPagingSmallSize: function (pagingDivId, TotalRecords, DisplayPages, ClassName, PageNo, rpp) {
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
        var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" class="p-xs pt-none pb-none" href="#null" title="Previous Pages" onclick="' + ClassName + 'PreviousClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-left"></i></a></li>';
        var nextChunck = '<li id="nextLink"><a id="lnkNextPages"  class="p-xs pt-none pb-none" href="#null" title="Next Pages" onclick="' + ClassName + 'NextClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-right"></i></a></li>';
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
    },

    NextClickSmallSizePaging: function (TotalPages, DisplayPages, pagingDivId, ClassName,FullPageBack) {
        //pagingDivId=$(pagesLinkDiv).attr("id");
        var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" class="p-xs pt-none pb-none" href="#null" title="Previous Pages" onclick="' + ClassName + 'PreviousClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-left"></i></a></li>';
        var nextChunck = '<li id="nextLink"><a id="lnkNextPages"  class="p-xs pt-none pb-none" href="#null" title="Next Pages" onclick="' + ClassName + 'NextClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-right"></i></a></li>';
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
            if (!FullPageBack) {
                setTimeout(function () {
                    $('#pagerParent ul li:eq(2) a').trigger("click");
                }, 100);
            }
            
        }
    },

    PreviousClickSmallSizePaging: function (TotalPages, DisplayPages, pagingDivId, ClassName, FullPageBack) {
        var prevChunck = '<li id="preLink" ><a id="lnkPrevPages" class="p-xs pt-none pb-none" href="#null" title="Previous Pages" onclick="' + ClassName + 'PreviousClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-left"></i></a></li>';
        var nextChunck = '<li id="nextLink"><a id="lnkNextPages"  class="p-xs pt-none pb-none" href="#null" title="Next Pages" onclick="' + ClassName + 'NextClickSmallSizePaging(' + TotalPages + ',' + DisplayPages + ',\'' + pagingDivId + '\',\'' + ClassName + '\');"><i class="fa fa-angle-double-right"></i></a></li>';
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
            if (!FullPageBack) {
                setTimeout(function () {
                    $('#pagerParent ul li:eq(2) a').trigger("click");
                }, 100);
            }
            
        }

    }

    

    ///////////////////------------------/////////////////
}


