Clinical_Template_Detail = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        Clinical_Template_Detail.params = params;
        if (Clinical_Template_Detail.params["PanelID"] != "pnlClinicalTemplateDetail") {
            Clinical_Template_Detail.params["PanelID"] = Clinical_Template_Detail.params["PanelID"] + ' #pnlClinicalTemplateDetail';
        }
        if (Clinical_Template_Detail.bIsFirstLoad) {
            Clinical_Template_Detail.bIsFirstLoad = false;
            CacheManager.BindDropDownsByID('#pnlClinicalTemplateDetail #LetterId', 'GetLetters', true, 22);

            var self = $('#pnlClinicalTemplateDetail');
            self.loadDropDowns(true).done(function () {
                AppPrivileges.GetFormPrivileges("Template", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_Template_Detail.LoadTemplate();
                    }
                });
            });
            $("#" + Clinical_Template_Detail.params["PanelID"] + " #SearchSectiontxt").keyup(function () {
                if ($("#" + Clinical_Template_Detail.params["PanelID"] + " #SearchSectiontxt").val().length > 3 || $("#" + Clinical_Template_Detail.params["PanelID"] + " #SearchSectiontxt").val().length < 1) {
                    Clinical_Template_Detail.SectionSearch();
                }
            });
        }
    },

    LoadTemplate: function () {
        pageNo = null;
        rpp = null;
        if (Clinical_Template_Detail.params.mode == "Add") {
            $("#" + Clinical_Template_Detail.params["PanelID"] + " #pnlSectioninfo").addClass('disableAll');

            Clinical_Template_Detail.ValidationClinical_Template();
            //serialize data
            $('#frmClinicalTemplateDetail').data('serialize', $('#frmClinicalTemplateDetail').serialize());
        }
        else if (Clinical_Template_Detail.params.mode == "Edit") {
            Clinical_Template_Detail.FillClinical_Template(Clinical_Template_Detail.params.ClinicalTemplateId, pageNo, rpp, true).done(function (response) {
                if (response.status != false) {
                    var templateLoadJson = JSON.parse(response.TemplateLoad_JSON);
                    var self = $("#" + Clinical_Template_Detail.params["PanelID"]);
                    utility.bindMyJSON(true, templateLoadJson[0], false, self).done(function () {
                        if (templateLoadJson[0].IsActive == "True") {
                            $("#" + Clinical_Template_Detail.params["PanelID"] + " #Active").prop("checked", true);
                        } else {
                            $("#" + Clinical_Template_Detail.params["PanelID"] + " #Active").prop("checked", false);
                        }

                        $('#ShortName,#TemplateTypeId,#SpecialityId').attr('disabled', true);
                        $("#" + Clinical_Template_Detail.params["PanelID"] + " #pnlSectioninfo").removeClass('disableAll');
                        Clinical_Template_Detail.ValidationClinical_Template();
                        if (response.status != false) {
                            Clinical_Template_Detail.SectionListLoad(response, pageNo, rpp);
                        }

                        if (templateLoadJson[0].HTMLTemplate != '' && templateLoadJson[0].HTMLTemplate != null) {
                            var htmlClinicalTemplate = templateLoadJson[0].HTMLTemplate;
                            //HTMLClinical_Template = HTMLClinical_Template.replace('&lt;', '<');

                            htmlClinicalTemplate = htmlClinicalTemplate.replace(/&quot;/g, '"');
                            htmlClinicalTemplate = htmlClinicalTemplate.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                            $('#' + Clinical_Template_Detail.params["PanelID"] + ' #HTMLTempalteClinical_Template').html(htmlClinicalTemplate);
                        } else {
                            $('#HTMLTempalteClinical_Template').html('');


                        }
                        if (response.SectionCount > 0) {
                            var clinicalTemplateFillJson = JSON.parse(response.TemplateSectionLoad_JSON);
                            var tempSectionListInfo = [];
                            $.each(clinicalTemplateFillJson, function (i, item) {
                                var divId = item.TemplateSectionId + '_' + item.SectionID;

                                var templateSectionListedHtml = '<li class="bg-primary" id="' + item.TemplateSectionId + '"><a hrefjavascript:void(0) onclick="return false;"><span class="ui-icon ui-icon-arrow-4-diag"></span>' +
                                                   '<div class="col-xs-11 p-none pull-left"><p class="white">' + item.Description + '</p></div></a>' +
                                                   '<div class="col-xs-1 pl-none pr-xs pull-right"><a href="#"></a><a class="btn btn-close btn-xs" onclick="Clinical_Template_Detail.DeleteSectionFromClinical_Template(' + item.SectionID + ',' + item.TemplateSectionId + ',\'' + divId + '\');">' +
                                                   '<i class="fa fa-times"></i></a> </div></li>';
                                $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDiv').append(templateSectionListedHtml);
                                if (item.IsUpdated == "0") {
                                    var tempSectionObjec = {};
                                    tempSectionObjec.SectionID = item.SectionID;
                                    tempSectionObjec.TemplateSectionId = item.TemplateSectionId;
                                    tempSectionObjec.SectionTypeId = item.SectionTypeId;
                                    tempSectionListInfo.push(tempSectionObjec);
                                }

                            });
                            if (tempSectionListInfo.length > 0) {
                                Clinical_Template_Detail.UpdateSectionInTemplate(tempSectionListInfo);
                            }
                            $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDivList').html('');
                            //for (var i = 0; i < response.Clinical_TemplateQuestionCount; i++) {
                            //    $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDivList').append('<li>' + (i + 1) + '</li>');
                            //}
                            $(document).ready(function () {
                                $("#SelectedSectionDiv li").click(function () {
                                    $("#SelectedSectionDiv li").addClass("bg-primary");
                                    $(this).removeClass("bg-primary");
                                    $(this).addClass("bg-success");
                                });
                            });
                        }

                        //serialize data
                        $('#frmClinicalTemplateDetail').data('serialize', $('#frmClinicalTemplateDetail').serialize());

                    });


                }
                else {
                    //   utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    UpdateSectionInTemplate: function (tempSectionListInfo, templateSectionId) {
        $.each(tempSectionListInfo, function (i, item) {

            var divId = item.SectionID + '_' + item.TemplateSectionId + '_TemplateSection';
            var sectionId = item.SectionID;
            var myJson = '{TextSearched :"' + $('#SearchSectiontxt').val() + '"}';
            Clinical_Section.SearchSection(myJson, sectionId, 1, 15).done(function (response) {
                if (response.status != false) {
                    var obj = jQuery.parseJSON(response.SectionLoad_JSON);
                    $('#' + divId).html('');
                    var htmlSectionQg = obj[0].HTMLTemplate;
                    htmlSectionQg = htmlSectionQg.replace(/&quot;/g, '"');
                    htmlSectionQg = htmlSectionQg.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                    $('#' + divId).replaceWith('<div class="toggle-content NoRadiusT" id="' + item.SectionID + '_' + item.TemplateSectionId + '_TemplateSection' + '">' + htmlSectionQg + '</div>');
                }
            });
        });
    },

    SectionSearch: function (sectionID, PageNo, rpp) {
        sectionID = "";
        var myJson = '{TextSearched :"' + $('#SearchSectiontxt').val() + '"}';
        Clinical_Template_Detail.SearchSection(myJson, sectionID, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                Clinical_Template_Detail.SectionListLoad(response, PageNo, rpp);
            }
        });
        // }
    },

    SearchSection: function (sectionData, sectionId, pageNo, rpp) {
        if (pageNo == null) {
            pageNo = 1;
        }
        if (rpp == null) {
            rpp = 6;
        }
        var data = "SectionData=" + sectionData + "&sectionID=" + sectionId + "&PageNo=" + pageNo + "&rpp=" + rpp;

        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "SEARCH_SECTION");
    },

    SectionListLoad: function (response, pageNo, rpp) {
        $("#lstSectionGroup").empty();

        if (response.SectionCount > 0) {
            var sectionLoadJsonData = JSON.parse(response.SectionLoad_JSON);
            $.each(sectionLoadJsonData, function (i, item) {

                var bgcolor = "";
                if (item.IsActive != "True") {
                    bgcolor = "bg-danger";
                }

                var lstSectionGroup = '<div style="z-index:2000" class="bg-primary sectiondrag  mb-xs ' + bgcolor + '" id="' + item.SectionId + '_' + item.SectionTypeId + '" ><a href="#"><span class="ui-icon ui-icon-arrow-4-diag pull-left"></span><span class="white">' + item.Description + '<input type="hidden" class="' + item.SectionId + '_' + item.SectionTypeId + '" value="' + item.HTMLTemplate + '"/></span></a></div>';
                $("#lstSectionGroup").last().append(lstSectionGroup);

            });

            $("#" + Clinical_Template_Detail.params["PanelID"] + " #divSectionlistPaging").css("display", "inline");
            //Showing 1 to 15 of 15 entries
            var recordsPerPage = rpp != null ? rpp : 6;
            var currentPage = pageNo != null ? pageNo : 1;
            var totalDisplayRecords = response.iTotalDisplayRecords_Sections;
            if (totalDisplayRecords == undefined) {
                totalDisplayRecords = response.iTotalDisplayRecords;
            }
            var pagesToShow = Math.ceil(totalDisplayRecords / recordsPerPage);
            var totalPagesToDisplay = pagesToShow > 0 ? pagesToShow : 1;
            if (pageNo == null) {
                Clinical_Template_Detail.GetCustomPaging("divSectionlistPaging", totalDisplayRecords, 2, "Clinical_Template_Detail", currentPage, recordsPerPage);
            }

            var toRecords = (parseInt(currentPage) * parseInt(recordsPerPage)) < totalDisplayRecords ? (parseInt(currentPage) * parseInt(recordsPerPage)) : totalDisplayRecords;
            var totalSections = totalDisplayRecords;

            Clinical_Template_Detail.ShowHideListGrid(currentPage, recordsPerPage, totalSections);


            $(".sectiondrag").draggable({
                revert: true,
                appendTo: 'body',
                helper: 'clone',
                stack: '.SectionDropInTemplate',
                start: function () {
                    $(this).css('z-index', '2000');
                },
                stop: function () {
                    $(this).css('z-index', '2000');
                }
            });


            $(".SectionDropInTemplate").droppable({
                accept: ".sectiondrag",
                activeClass: 'droppable-active',
                hoverClass: 'droppable-hover',
                drop: function (ev, ui) {
                    //getting Information Of Question Dropped
                    var sectionText = ui.draggable.find('.white').html();
                    var quesSectionType = ui.draggable.attr('id').split('_');
                    var sectionId = quesSectionType[0];
                    var sectionTypeId = quesSectionType[1];

                    var myJson = '{TextSearched :"' + $('#SearchSectiontxt').val() + '"}';
                    var refId = $(this);
                    var sectionHtml = $('.sectiondrag .' + ui.draggable.attr('id')).val();


                    Clinical_Template_Detail.InsertSectioninTemplate(sectionId, 1, 1).done(function (response) {
                        if (response.status != false) {
                            //var obj = jQuery.parseJSON(response.SectionLoad_JSON);

                            // Clinical_Template_Detail.getSectionsData(SectionId, SectionTypeId, 1, 1);
                            var divId = response.TemplateSectionId + '_' + sectionId + '_' + sectionTypeId;
                            $("#SelectedSectionDiv li").addClass("bg-primary");
                            var sectionListedHtml = '<li class="bg-success" id="' + response.TemplateSectionId + '"><a href="#" onclick="Clinical_Template_Detail.getSectionsData(' + sectionId + ',' + sectionTypeId + ',1,1' + ');"><span class="ui-icon ui-icon-arrow-4-diag"></span>' +
                                                   '<div class="col-xs-11 p-none pull-left"><p class="white">' + sectionText + '</p></div></a>' +
                                                   '<div class="col-xs-1 pl-none pr-xs pull-right"><a href="#"></a><a class="btn btn-close btn-xs" onclick="Clinical_Template_Detail.DeleteSectionFromClinical_Template(' + sectionId + ',' + response.TemplateSectionId + ',\'' + divId + '\');">' +
                                                   '<i class="fa fa-times"></i></a> </div></li>';
                            $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDiv').append(sectionListedHtml);
                            $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDivList').html('');

                            var droppedQuestionHeadingId = ui.draggable.attr('id');
                            var droppedQuestionHeadingText = ui.draggable.children().children()[1].innerText;

                            //var clinicalTemplateFillJson = JSON.parse(response.TemplateSectionLoad_JSON);

                            var droppedHeadingCollapsable = '<section class="toggle mb-sm active"  style="margin-bottom: 0px !important;" onclick="Clinical_Template_Detail.OnSectionHeadingDropAtTemplate(' + sectionId + ',' + response.TemplateSectionId + ');" id="' + sectionId + '_' + response.TemplateSectionId + '"><label class="">' + droppedQuestionHeadingText + '</label></section>';
                            // ui.draggable.detach().appendTo(Ref);//.css('position','initial'));
                            $(refId).append(droppedHeadingCollapsable);


                            //$(RefID).append(SectionHTML);
                            $(refId).append('<div class="toggle-content NoRadiusT" id="' + sectionId + '_' + response.TemplateSectionId + '_TemplateSection' + '">' + sectionHtml + '</div>');


                            var self = $("#" + Clinical_Template_Detail.params["PanelID"]);
                            //  var myJSON = self.getMyJSON();
                            Clinical_Template_Detail.UpdateClinicalTemplate(null, Clinical_Template_Detail.params.ClinicalTemplateId);
                            //for (var i = 0; i < $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDiv').find('li').length; i++) {
                            //    $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDivList').append('<li>' + (i + 1) + '</li>');
                            //}
                        }
                    });

                }

            });

        } else {
            Clinical_Template_Detail.ShowHideListGrid(0, 0, 0);
        }
    },

    OnSectionHeadingDropAtTemplate: function (id, TempSectionID) {
        var sectionId = id;
        id = id + '_' + TempSectionID + '_TemplateSection';
        $('#HTMLTempalteClinical_Template').find('#' + id + '.toggle-content').toggle();

        $('#HTMLTemplate').find('#' + id).toggle();
    },

    GetCustomPaging: function (pagingDivId, totalRecords, pagesToDisplay, className, pageNo, rpp) {
        GetPagingSmallSize(pagingDivId, totalRecords, pagesToDisplay, className, pageNo, rpp);
    },

    InsertSectioninTemplate: function (sectionId, pageNo, rpp) {
        if (pageNo == null) {
            pageNo = 1;
        }
        if (rpp == null) {
            rpp = 6;
        }

        var data = "SectionId=" + sectionId + "&ClinicalTemplateId=" + Clinical_Template_Detail.params.ClinicalTemplateId;

        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "INSERT_SECTION_IN_TEMPLATE");
    },

    DeleteSectionFromClinical_Template: function (sectionID, templateSectionId, divId) {
        utility.myConfirm('1', function () {
            var selectedValue = templateSectionId;
            var sectinId = divId.split('_')[1];
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Template_Detail.DeleteSectionFromTemplate(sectinId, selectedValue).done(function (response) {
                    if (response.status != false) {
                        $('#' + sectinId + '_' + templateSectionId).remove();
                        $('#' + sectinId + '_' + templateSectionId + '_TemplateSection').remove();

                        //$('#' + Clinical_Template_Detail.params.clinicaltemplateId + '_TemplateSection #' + divID).remove();
                        $('#SelectedSectionDiv').find('li#' + selectedValue).remove();

                        $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDivList').html('');
                        //for (var i = 0; i < $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDiv').find('li').length; i++) {
                        //    $('#' + Clinical_Template_Detail.params["PanelID"] + ' #SelectedSectionDivList').append('<li>' + (i + 1) + '</li>');
                        //}
                        var self = $("#" + Clinical_Template_Detail.params["PanelID"]);
                        //if (self == )
                        var myJson = self.getMyJSON();
                        Clinical_Template_Detail.UpdateClinicalTemplate(null, Clinical_Template_Detail.params.ClinicalTemplateId);
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

    DeleteSectionFromTemplate: function (sectinId, templateSectionId) {
        var data = "SectinID=" + sectinId + "&TemplateSectionId=" + templateSectionId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "DELETE_SECTIONFRMTEMPLATE");
    },

    ShowHideListGrid: function (currentPage, recordsPerPage, totalSections) {
        $("#lstSectionGroup").find('li').hide();
        for (var i = (recordsPerPage * currentPage) - 5; i < (recordsPerPage * currentPage) + recordsPerPage - 5; i++) {
            $("#lstSectionGroup").find('li:nth-child(' + i + ')').show();
        }
        var toRecords = (parseInt(currentPage) * parseInt(recordsPerPage)) < totalSections ? (parseInt(currentPage) * parseInt(recordsPerPage)) : totalSections;
        if (totalSections == null) {
            totalSections = $("#lstSectionGroup").find('li').length;
        }
        var showingText = "Showing " + currentPage > 0 ? ((parseInt(currentPage) - 1) * parseInt(recordsPerPage) + 1) : currentPage + " to " + toRecords + " of " + totalSections + " Record(s)";
        $("#divSectionlistPaging #divShowingEntries").text(showingText);
        // Change Background Color to Black for selected page
        $("#divSectionlistPaging li").each(function () {
            if ($(this).text() == currentPage) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });

    },

    SelectedPageClick: function (PageNo, objPage) {
        Clinical_Template_Detail.ShowHideListGrid(PageNo, 6);
        Clinical_Template_Detail.SectionSearch(null, PageNo, 6);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#divSectionlistPaging li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";
        if (currentPageNo != "" && currentPageNo > 0) {
            //  questionGroupDetail.ShowHideListGrid(currentPageNo, 6, null);
            Clinical_Template_Detail.SectionSearch(null, currentPageNo, 6);
        }
        var LastPageNo = $("#divSectionlistPaging li:nth-child(3)").text();
        if (currentPageNo < Number(LastPageNo)) {
            Clinical_Template_Detail.PreviousClickSmallSizePaging(TotalPages, DisplayPages, pagingDivId, "Clinical_Template_Detail.");
            setTimeout(function () {
                $("#divSectionlistPaging li:nth-child(4)").attr("class", "active");
            }, 200
            );
        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#divSectionlistPaging li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        var LastPageNo = $("#divSectionlistPaging li:nth-child(4)").text();
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            //questionGroupDetail.ShowHideListGrid(currentPageNo, 6, null);
            Clinical_Template_Detail.SectionSearch(null, currentPageNo, 6);
        }
        if (currentPageNo > Number(LastPageNo)) {
            Clinical_Template_Detail.NextClickSmallSizePaging(TotalPages, DisplayPages, pagingDivId, "Clinical_Template_Detail.");
            setTimeout(function () { $("#divSectionlistPaging li:nth-child(3)").attr("class", "active"); }, 200
            );
        }
    },

    FillClinical_Template: function (clinicalTemplateId, pageNo, rpp, sectionDetils) {

        if (pageNo == null) {
            pageNo = 1;
        }
        if (rpp == null) {
            rpp = 6;
        }

        var sectionsDetailsJson = "";
        if (sectionDetils) {
            var pageNoSection = 1; var rppSection = 6;
            sectionsDetailsJson = '{SearchSectiontxt :"' + $('#SearchSectiontxt').val() + '",PageNoSection:"' + pageNoSection + '",rppSection:"' + rppSection + '"}';
        }
        var data = "ClinicalTemplateId=" + clinicalTemplateId + "&PageNo=" + pageNo + "&rpp=" + rpp + "&SectionsDetailsJson=" + sectionsDetailsJson;

        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "FILL_TEMPLATE");
    },

    ValidationClinical_Template: function () {
        $('#frmClinicalTemplateDetail')
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
                  TemplateTypeId: {
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
           Clinical_Template_Detail.Clinical_Template_Save();
       });
    },

    Clinical_Template_Save: function () {
        var strMessage = "";
        var self = $("#" + Clinical_Template_Detail.params["PanelID"]);
        var myJson = self.getMyJSON();
        if (Clinical_Template_Detail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Template", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (Clinical_Template_Detail.params.ClinicalTemplateId == "-1") {
                        Clinical_Template_Detail.Clinical_TemplateSave(myJson).done(function (response) {
                            if (response.status != false) {
                                $("#" + Clinical_Template_Detail.params["PanelID"] + " #pnlSectioninfo").removeClass('disableAll');
                                Clinical_Template_Detail.params.ClinicalTemplateId = response.ClinicalTemplateId;


                                utility.DisplayMessages(response.message, 1);
                                $("#" + Clinical_Template_Detail.params["PanelID"] + " #pnlSectioninfo").removeClass('disableAll');
                                Clinical_Template_Detail.ValidationClinical_Template();
                                //Clinical_Template_Detail.SectionListLoad(myJSON,1,6);
                                Clinical_Template_Detail.SectionSearch();
                                Clinical_Template_Detail.FillClinical_Template(Clinical_Template_Detail.params.ClinicalTemplateId, pageNo, rpp, true).done(function (response) {
                                    if (response.status != false) {
                                        Clinical_Template_Detail.SectionListLoad(response, pageNo, rpp);

                                    }

                                });


                                //  $('#Canvas,#ShortName,#SpecialityID').attr('disabled', true);
                                //section Search    Clinical_Template_Detail.QuestionsSearch(null, null, 6);

                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (Clinical_Template_Detail.params.ClinicalTemplateId != "-1" && Clinical_Template_Detail.params.ClinicalTemplateId != "" && Clinical_Template_Detail.params.ClinicalTemplateId != "0") {
                        Clinical_Template_Detail.UpdateClinicalTemplate(myJson, Clinical_Template_Detail.params.ClinicalTemplateId).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);

                                UnloadActionPan();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }

                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (Clinical_Template_Detail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Template", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Clinical_Template_Detail.UpdateClinicalTemplate(myJson, Clinical_Template_Detail.params.ClinicalTemplateId, 2).done(function (response) {
                        if (response.status != false) {
                            //load Templates Details    Clinical_Question_Group.QuestionGroupSearch(0, 1, 15);
                            utility.DisplayMessages(response.message, 1);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    Clinical_TemplateSave: function (clinicalTemplateData, clinicalTemplateId) {

        var data = "ClinicalTemplateData=" + clinicalTemplateData + "&ClinicalTemplateId=" + clinicalTemplateId;//+ "&HTMLTempalteQuestionGroup=" + (QuestionGroupData != null ? null : $('#' + Clinical_Template_Detail.params["PanelID"] + ' #HTMLTempalteQuestionGroup').html());
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "SAVE_TEMPLATE_GROUP");
    },

    UpdateClinicalTemplate: function (clinicalTemplateData, clinicalTemplateId) {

        var data = "ClinicalTemplateData=" + clinicalTemplateData + "&ClinicalTemplateId=" + clinicalTemplateId + "&HTMLTempalteTemplate=" + $('#' + Clinical_Template_Detail.params["PanelID"] + ' #HTMLTempalteClinical_Template').html();
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "CLINICAL_TEMPLATE", "UPDATE_TEMPLATE_GROUP");
    },

    Get_Sections_Of_Template: function (SectionId, QuesGroupSectionId, fieldsJSON) {
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

    UnLoadTab: function () {
        var self = $("#" + Clinical_Template_Detail.params["PanelID"]);
        var myJson = self.getMyJSON();
        Clinical_Template_Detail.UpdateClinicalTemplate(myJson, Clinical_Template_Detail.params.ClinicalTemplateId, 2).done(function (response) {


            utility.UnLoadDialog("frmClinicalTemplateDetail", function () {
                UnloadActionPan();
                Clinical_Template.TemplateSearch();
            }, function () {
                UnloadActionPan();
                Clinical_Template.TemplateSearch();
            });

        });
    },

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

    NextClickSmallSizePaging: function (TotalPages, DisplayPages, pagingDivId, ClassName) {
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
            setTimeout(function () {
                $('#pagerParent ul li:eq(2)').trigger("click");
            }, 100);
        }
    },

    PreviousClickSmallSizePaging: function (TotalPages, DisplayPages, pagingDivId, ClassName) {
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
        }

    }
}

