sectionHeadingDetail = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {
        sectionHeadingDetail.params = params;

        if (sectionHeadingDetail.bIsFirstLoad) {
            sectionHeadingDetail.bIsFirstLoad = false;

            var self = null;
            if ($('#' + sectionHeadingDetail.params["PanelID"] == 'sectionHeadingDetail'))
                self = $('#' + sectionHeadingDetail.params["PanelID"]);
            else
                self = $('#' + sectionHeadingDetail.params["PanelID"] + ' #sectionHeadingDetail');
            self.loadDropDowns(true).done(function () {
                sectionHeadingDetail.ValidateHeadingDetail(sectionHeadingDetail.params.QuestionGroupId);
            });
        }
    },

    ValidateHeadingDetail: function () {
        $('#frmsectionHeadingDetail')
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
                  },
                  Specialty: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          }

                      }
                  },
                  BodySystems: {
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
           sectionHeadingDetail.QuestionGroupSave();
       });
    },

    QuestionGroupSave: function () {
        var strMessage = "";
        var self = $("#" + sectionHeadingDetail.params["PanelID"]);
        var myJSON = self.getMyJSON();
        var questionGroupDetails = JSON.parse(myJSON);
        var QuesGroupSectionId;
        sectionDetail.GetQuestionGroup_Sections(sectionDetail.params.SectionId, null, null).done(function (response) {
            if (response.SectionQuestionGroupCount > 0) {
                var SectionFill_JSON = JSON.parse(response.SectionFill_JSON);
                $.each(SectionFill_JSON, function (i, item) {
                    QuesGroupSectionId = item.QuesGroupSectionId;
                });
            }
        });
        if (sectionHeadingDetail.params.mode == "Add") {
            
            //  AppPrivileges.GetFormPrivileges("CPT", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                
                sectionHeadingDetail.SaveQuestionGroup(myJSON).done(function (response) {
                    
                    if (response.status != false) {
                        
                        sectionHeadingDetail.params.QuestionGroupId = response.QuestionGroupId;
                        utility.DisplayMessages(response.message, 1);

                        $('#frmsectionHeadingDetail').data('serialize', $('#frmsectionHeadingDetail').serialize());
                        UnloadActionPan('sectionDetail', 'sectionHeadingDetail');

                        sectionDetail.InsertQuestionGroup_Section(myJSON, sectionHeadingDetail.params.QuestionGroupId, 1, 1).done(function (response) {
                            if (response.status != "false") {
                                QuesGroupSectionId = response.QuesGroupSectionId;
                                var CanvasColumns = "";
                                if (questionGroupDetails.Canvas > 1) {
                                    for (var i = 0; i < questionGroupDetails.Canvas; i++) {
                                        CanvasColumns = CanvasColumns + '<div class="col-sm-' + 12 / questionGroupDetails.Canvas + ' questiondrop" style=" left: 0px; top: 0px;min-height:200px;"></div>';
                                    }
                                } else {
                                    $('#dynamicQuestionDrop').append('<div class="questiondrop" style=" left: 0px; top: 0px;min-height:200px;"></div>');
                                }
                                var SectionDetail = JSON.parse(response.QuestionGroupLoad_JSON);
                                //var divHeader = '<div class="bg-primary  ui-draggable ui-draggable-handle" id="' + sectionHeadingDetail.params.QuestionGroupId + '" style="z-index: 1; left: 0px; top: 0px;"><a href="#"><span class="ui-icon ui-icon-arrow-4-diag" style="float:left;"></span><span class="white">' + response.QuestionGroupDescription + '</span></a></div>' + CanvasColumns + '<div class="clearfix"></div>';
                                var DroppedHeadingCollapsable = '<section class="toggle mb-sm active" onclick="sectionHeadingDetail.OnSectionHeadingDrop(' + sectionHeadingDetail.params.QuestionGroupId + ',' + QuesGroupSectionId + ');" id="' + sectionHeadingDetail.params.QuestionGroupId + '_' + QuesGroupSectionId + '"><label class="white">' + SectionDetail[0].Description + '</label></section><div class="toggle-content panel-body NoRadiusT" id="' + sectionHeadingDetail.params.QuestionGroupId + '_' + QuesGroupSectionId + '_SectionQuestionGroup' + '">' + CanvasColumns + '</div><div class="clearfix"></div>';
                                //var divHeader = '<div class="bg-primary  ui-draggable ui-draggable-handle" id="' + sectionHeadingDetail.params.QuestionGroupId + '" style="z-index: 1; left: 0px; top: 0px;"><a href="#"><span class="ui-icon ui-icon-arrow-4-diag" style="float:left;"></span><span class="white">' + response.QuestionGroupDescription + '</span></a></div><div class="questiondrop" style=" left: 0px; top: 0px;min-height:200px;"></div><div class="clearfix"></div>';
                                $('#dynamicQuestionDrop').append(DroppedHeadingCollapsable);

                                //var myJSON = '{TextSearched :"' + $('#Short_Name').val() + '"}';
                                //sectionDetail.InsertQuestionGroup_Section(myJSON, sectionHeadingDetail.params.QuestionGroupId, 1, 1);

                                sectionDetail.QuestionDragToHeading();
                            }
                        });

                        
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

    OnSectionHeadingDrop: function (id, QG_SectionID) {
        var QGSectionID = QG_SectionID;
        id = id + '_' + QG_SectionID + '_SectionQuestionGroup';
        //$('#canvasColumn').find('#' + id + '.toggle-content').toggle();
        $('#dynamicQuestionDrop').find('#' + id + '.toggle-content').toggle();
        //$('#HTMLTempalteClinical_Template').find('#' + id).toggle()
    },

    SaveQuestionGroup: function (QuestionGroupData) {
        var data = "QuestionGroupData=" + QuestionGroupData;
        return MDVisionService.defaultService(data, "CLINICAL_QUESTION_GROUP", "SAVE_QUESTION_GROUP");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmsectionHeadingDetail', function () {
            UnloadActionPan('sectionDetail', 'sectionHeadingDetail');
        }, function () {
            UnloadActionPan('sectionDetail', 'sectionHeadingDetail');
        });
    },

}