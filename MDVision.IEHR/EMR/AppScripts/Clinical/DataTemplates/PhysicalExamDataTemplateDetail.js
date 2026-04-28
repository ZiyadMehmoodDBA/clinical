PhysicalExamDataTemplateDetail = {

    //Author: Abid Ali
    //Date: 13-06-2016
    //This file will handle all actions performed for Physical Exam Data Template Detail
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    FamilyMembers: [],
    ExamDetails: {},
    SelectedSystem: '',
    array: [],
    myArr: [],
    parentCtrlGlobel: null,
    mainSelected: null,
    SectionNormalInfo: [],
    selectedcharacteristicsIds: [],
    characteristicsWithData: [],
    selectedsubcharacteristicsIds: [],
    subcharacteristicsWithData: [],
    selectedData: null,
    isNormalTriggred: false,
    isBothUnCheck: false,
    specialityCheckedIds: [],
    providerSelectedIds: [],
    providerCheckedIds: [],
    normalSystemIdsGlobel: [],
    selectedPhyExamTempData: [],
    SpecialtyIds: '',
    ProviderIds: '',
    isSystemNormal: false,
    DBDataJSON: [],
    DataJSON: [],
    // CharDetails: 'CharDetails',
    // SubCharDetails:'SubCharDetails',
    footerButtons: null,

    Load: function (params) {

        PhysicalExamDataTemplateDetail.params = params;
        PhysicalExamDataTemplateDetail.footerButtons = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #footerButtons');
        if (PhysicalExamDataTemplateDetail.params.PanelID != 'pnlClinicalPhysicalExamDataTemplateDetail') {
            PhysicalExamDataTemplateDetail.params.PanelID = PhysicalExamDataTemplateDetail.params.PanelID + ' #pnlClinicalPhysicalExamDataTemplateDetail';
        } else {
            PhysicalExamDataTemplateDetail.params.PanelID = 'pnlClinicalPhysicalExamDataTemplateDetail';
        }

        var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID);

        self.loadDropDowns(true).done(function () {

        });
        PhysicalExamDataTemplateDetail.domReadyFunction();

        if (PhysicalExamDataTemplateDetail.params.mode == "Edit") {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #btnSaveAs").removeClass('hidden');
            PhysicalExamDataTemplateDetail.loadTemplateBasedPhysicalExam(PhysicalExamDataTemplateDetail.params.TemplateId, 'Edit', PhysicalExamDataTemplateDetail.params.DataTemplateId);
            PhysicalExamDataTemplateDetail.footerButtons.show();
        }
        else {
            PhysicalExamDataTemplateDetail.loadPhysicalExamTemplates();
            PhysicalExamDataTemplateDetail.footerButtons.hide();
        }
        //Validate PhysicalExamDataTemplate form
        PhysicalExamDataTemplateDetail.validatePhysExamDataTemplate();
    },

    //Author: Abid Ali
    //Date: 15-June-2016
    //Client side validation before submiting form.
    validatePhysExamDataTemplate: function (ActionPanID) {
        var self = '#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail';
        $(self).bootstrapValidator('destroy');
        $(self)
          .bootstrapValidator({
              live: 'enabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  DataTemplateName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           if (e.type == "success") {

               var $form = $(e.target);
               var $button = $form.data('bootstrapValidator').getSubmitButton();
               switch ($button.attr('id')) {
                   case 'btnSave':
                       PhysicalExamDataTemplateDetail.addUpdatePhysExamDataTemplate();
                       break;
                   case 'btnSaveAs':
                       PhysicalExamDataTemplateDetail.dataTemplateSaveAsPopup();
                       break;
               }
           }

       });
    },

    dataTemplateSaveAsPopup: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Clinical_Data Template_Physical Exam", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                //Start/Abid Ali/Bug# EMR-1856

                var $self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail");

                var $formSubmitBtns = $self.find("#footerButtons");
                var $btnSave = $formSubmitBtns.find("#btnSave");
                var $btnSaveAs = $formSubmitBtns.find("#btnSaveAs");

                $btnSave.prop("disabled", false);
                $btnSaveAs.prop("disabled", false);

                //End/Abid Ali/Bug# EMR-1856

                var params = [];
                params["FromAdmin"] = '0';
                params["TabID"] = "PhysicalExamDataTemplateSaveAs";
                params["ParentCtrl"] = 'PhysicalExamDataTemplateDetail';
                LoadActionPan('PhysicalExamDataTemplateSaveAs', params, PhysicalExamDataTemplateDetail.params.PanelID);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },


    //Author: Abid Ali
    //Date: 13-06-2016
    //Intializes Components
    domReadyFunction: function () {
        $(function () {
            setTimeout(function () { countWidth(); }, 405);
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " .toggleVertical div.toggle").click(function (e) {
                if ($(this).children().hasClass("active")) {
                    $(this).prev().removeClass("hidden");

                    countWidth(e);
                    $(this).parent().parent().scrollLeft(1000);
                }
                else {
                    $(this).prev().addClass("hidden");
                    countWidth(e);
                    $(this).parent().parent().scrollLeft(0);
                }
            });
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail').on('change', function () {
                $("#" + PhysicalExamDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val('true');
            });


        });


    },


    //Author: Abid Ali
    //Date: 15-June-2016
    //Save Update Physical Exam Data template
    addUpdatePhysExamDataTemplate: function (isSaveAs, templateName) {

        var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail");
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        myJSON = JSON.parse(myJSON);

        var templateId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfTemplateId').val();
        myJSON["TemplateId"] = templateId == "" ? -1 : templateId;
        var dataTemplateId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfDataTemplateId').val();
        myJSON["DataTemplateId"] = dataTemplateId == "" ? -1 : dataTemplateId;

        //Save as data template
        if (isSaveAs) {

            //Save Template Name
            if (templateName != null) {
                myJSON["DataTemplateName"] = templateName;
                myJSON["DataTemplateId"] = -1;
            }
        }
        myJSON = JSON.stringify(myJSON);
        if (PhysicalExamDataTemplateDetail.params.mode == "Add") {

            //AppPrivileges.GetFormPrivileges("Clinical_Data Template_Physical Exam", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            PhysicalExamDataTemplateDetail.insertUpdatePhysExamDataTemplate_DBCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    PhysicalExamDataTemplate.loadPhysExamDataTemplate();
                    utility.DisplayMessages(response.message, 1);


                    UnloadActionPan(PhysicalExamDataTemplateDetail.params["ParentCtrl"], "PhysicalExamDataTemplateDetail");
                    PhysicalExamDataTemplateDetail.DataJSON = [];
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            //    }
            //    else
            //        utility.DisplayMessages(strMessage, 2);
            //});
        }
        else if (PhysicalExamDataTemplateDetail.params.mode == "Edit") {

            //AppPrivileges.GetFormPrivileges("Clinical_Data Template_Physical Exam", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            //    if (strMessage == "") {
            PhysicalExamDataTemplateDetail.insertUpdatePhysExamDataTemplate_DBCall(myJSON).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    PhysicalExamDataTemplate.loadPhysExamDataTemplate();
                    utility.DisplayMessages(response.message, 1);

                    if (isSaveAs == null) {
                        UnloadActionPan(PhysicalExamDataTemplateDetail.params["ParentCtrl"], "PhysicalExamDataTemplateDetail");
                    }
                    PhysicalExamDataTemplateDetail.DataJSON = [];
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            //    }
            //    else
            //        utility.DisplayMessages(strMessage, 2);
            //});
        }
    },

    ////Author: Abid Ali
    ////Date: 13-06-2016
    ////This function will handle Unload of Physical Exam Data Tepmlate Tab
    //unLoadTab: function () {

    //    UnloadActionPan(PhysicalExamDataTemplateDetail.params["ParentCtrl"], "PhysicalExamDataTemplateDetail");

    //},


    //Author: Abid Ali
    //Date: 13-06-2016
    //This function will load physical Exam templates
    showPhysicalExamDataTemplatesTab: function (obj) {
        var $liAddAnchorTag = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #AddEditPhysicalExamDataTemplate a');

        if (PhysicalExamDataTemplateDetail.params.mode == "Add") {
            $liAddAnchorTag.hide();
        }
        PhysicalExamDataTemplateDetail.footerButtons.hide();
    },

    showButtons: function () {
        PhysicalExamDataTemplateDetail.footerButtons.show();
    },

    //Author: Abid Ali
    //Date: 13-06-2016
    //Description: This function will load the physical Exam template
    loadPhysicalExamTemplates: function () {
        PhysicalExamDataTemplateDetail.PhysicalExamTemplateLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //PhysicalExamDataTemplateDetail.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadPhysicalExam);
                var arrTemplates = JSON.parse(response.PhysExamTemplateFill_JSON);
                var ctrlTemplates = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #dgvPhysicalExamDataTemplates');//$('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #ulPhysExamTemplates');
                PhysicalExamDataTemplateDetail.bindPhysExamTemplate(ctrlTemplates, arrTemplates);
                // var objtabPhysicalExamTemplates = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #LiPhysicalExamDataTemplate');

            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
        });
    },

    //Author: Abid Ali
    //Date: 13-06-2016
    //This function will bind physical Exam Templates
    bindPhysExamTemplate: function (Crtl, arrComponents, providerId, specialtyId) {

        $(Crtl).find("tbody").find("tr").remove();
        var currentLiClass = "";
        var isFirstLi = true;
        var activeLiId;
        var activeLiComponentName;
        var ParentDiv = "view";

        $.each(arrComponents, function (i, item) {
            //Start 9 March 2016 Muhammad Arshad Filtering Templates
            // if (item.IsDefault.toLowerCase() == "true") {
            var $row = $('<tr/>');

            $row.attr("id", "gvPhysExamTemplate_row" + item.physExamTemplateId);
            $row.attr("physExamTemplateId", item.physExamTemplateId);
            $row.attr("isdefault", item.IsDefault);
            $row.attr("providerids", item.ProviderIds);
            $row.attr("specialtyids", item.SpecialtyIds);
            $row.attr("IsActive", item.IsActive);
            if (item.IsActive == "True") {
                isactive = 0;
                activeRecord = "Active Record";
                tglclass = "fa fa-toggle-on green";
            }
            else {
                isactive = 1;
                activeRecord = "Inactive Record";
                tglclass = "fa fa-toggle-on red";
            }

            $row.attr("onclick", "utility.SelectGridRow($('#gvPhysExamTemplate_row" + item.physExamTemplateId + "'))");
            var actionTick = "PhysicalExamDataTemplateDetail.loadTemplateBasedPhysicalExam('" + item.physExamTemplateId + "','Add');";
            var strAction = '<a class="btn btn-xs" href="#" onclick="' + actionTick + '" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;';
            $row.append('<td style="display:none;">' + item.physExamTemplateId + '</td><td>' + strAction + '</td><td>' + item.physExamTemplateName + '</td>');
            $(Crtl).find("tbody").last().append($row);
            //  }
            //End 9 March 2016 Muhammad Arshad Filtering Templates

        });
        // PhysicalExamDataTemplateDetail.toggleVerticalWidth();
    },


    //Author: Abid Ali
    //Date: 14-06-2016
    //This function will load physical Exam based on selected TemplateId
    loadTemplateBasedPhysicalExam: function (templateId, caller, dataTemplateId) {

        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfTemplateId').val(templateId);
        if (dataTemplateId != null)
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfDataTemplateId').val(dataTemplateId);


        var objtabPhysicalExamTemplates = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #LiPhysicalExamDataTemplate');
        objtabPhysicalExamTemplates.removeClass("active");
        var objtabAddPhysicalExam = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #AddEditPhysicalExamDataTemplate');
        objtabAddPhysicalExam.removeClass("hidden");

        objtabAddPhysicalExam.find("a").show();
        objtabAddPhysicalExam.find("a").trigger("click");
        objtabAddPhysicalExam.find("a").text(caller);

        var title = " PE Data Template";
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #headingTitle').text(caller + title);

        // $.when(PhysicalExamDataTemplateDetail.loadPhysicalExam("", true)).done(function (selectedData) {
        var selectedData = null;
        PhysicalExamDataTemplateDetail.loadPhysicalExamSystem(selectedData);
        //  });

        //$.when(PhysicalExamDataTemplateDetail.loadChildData(null, "mainpesystem")).then(function () {
        //});

        //}
    },

    // ------------------- New Physical Exam Json ---------------

    //Author: Muhammad Arshad
    //Date:09/03/2016
    //This function will filter physical Exam Templates based on current ProviderNote's ProviderId/SpecialtyId

    isTemplateRelevant: function (currNotesProviderId, currentSpecialtyId, templateProviderIds, templateSpecialtyIds) {
        var isRelevant = false;
        if (templateProviderIds != null && templateProviderIds != "") {
            if (templateProviderIds.split(',').indexOf(currNotesProviderId) > -1) {
                isRelevant = true;
            }
            else {
                isRelevant = false;
            }
        }
        else if (templateSpecialtyIds != null && templateSpecialtyIds != "") {
            if (templateSpecialtyIds.split(',').indexOf(currentSpecialtyId) > -1) {
                isRelevant = true;
            }
            else {
                isRelevant = false;
            }
        }
        else if ((templateProviderIds == null || templateProviderIds == "") && (templateSpecialtyIds == null || templateSpecialtyIds == ""))
            isRelevant = true;
        return isRelevant;
    },

    //Author: Muhammad Arshad
    //Date:04/03/2016
    //This function will bind physical Exam Templates
    bindPhysExamTemplate: function (Crtl, arrComponents, providerId, specialtyId) {

        $(Crtl).find("tbody").find("tr").remove();
        var currentLiClass = "";
        var isFirstLi = true;
        var activeLiId;
        var activeLiComponentName;
        var ParentDiv = "view";

        $.each(arrComponents, function (i, item) {
            //Start 9 March 2016 Muhammad Arshad Filtering Templates
            //if (item.IsDefault.toLowerCase() == "true" || PhysicalExamDataTemplateDetail.isTemplateRelevant(providerId, specialtyId, item.ProviderIds, item.SpecialtyIds) == true) {
            var $row = $('<tr/>');

            $row.attr("id", "gvPhysExamTemplate_row" + item.physExamTemplateId);
            $row.attr("physExamTemplateId", item.physExamTemplateId);
            $row.attr("isdefault", item.IsDefault);
            $row.attr("providerids", item.ProviderIds);
            $row.attr("specialtyids", item.SpecialtyIds);
            $row.attr("IsActive", item.IsActive);
            if (item.IsActive == "True") {
                isactive = 0;
                activeRecord = "Active Record";
                tglclass = "fa fa-toggle-on green";
            }
            else {
                isactive = 1;
                activeRecord = "Inactive Record";
                tglclass = "fa fa-toggle-on red";
            }

            $row.attr("onclick", "utility.SelectGridRow($('#gvPhysExamTemplate_row" + item.physExamTemplateId + "'))");
            var actionTick = "PhysicalExamDataTemplateDetail.loadTemplateBasedPhysicalExam('" + item.physExamTemplateId + "','Add');";
            var strAction = '<a class="btn btn-xs" href="#" onclick="' + actionTick + '" title="Select Record"><i class="fa fa-check black"></i></a>&nbsp;';
            $row.append('<td style="display:none;">' + item.physExamTemplateId + '</td><td>' + strAction + '</td><td>' + item.physExamTemplateName + '</td>');
            $(Crtl).find("tbody").last().append($row);
            //}
            //End 9 March 2016 Muhammad Arshad Filtering Templates

        });
        PhysicalExamDataTemplateDetail.toggleVerticalWidth();
    },

    //Author: Muhammad Arshad
    //Date:04/03/2016
    //This function will load physical Exam Templates
    loadPhysExamTemplates: function () {
        var providerId = PhysicalExamDataTemplateDetail.params["CurrentNotesProviderId"];
        var specialtyId = "";
        Admin_Provider.SearchProvider(null, providerId, 1, 15).done(function (response) {
            if (response.status == true) {
                var myJSON = JSON.parse(response.ProviderLoad_JSON);
                specialtyId = myJSON[0].SpecialtyId;
                providerId = myJSON[0].ProviderId;
                PhysicalExamDataTemplateDetail.loadPhysicalExamTemplate(providerId, specialtyId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //PhysicalExamDataTemplateDetail.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadPhysicalExam);
                        var arrTemplates = JSON.parse(response.PhysExamTemplateFill_JSON);
                        var ctrlTemplates = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #dgvPhysicalExamDataTemplates');//$('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #ulPhysExamTemplates');
                        PhysicalExamDataTemplateDetail.bindPhysExamTemplate(ctrlTemplates, arrTemplates, providerId, specialtyId);
                        var objtabPhysicalExamTemplates = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #LiPhysicalExamDataTemplate');
                        objtabPhysicalExamTemplates.find("a").trigger("click");
                    }
                    else {
                        utility.DisplayMessages(strMessage, 3);
                    }
                });
            }
        });
    },

    //Author: Muhammad Arshad
    //Date:07/03/2016
    //This function will show physical Exam Templates Tab
    showPhysExamTemplatesTab: function () {
        PhysicalExamDataTemplateDetail.isFirstLoad = true;
        var objtabPhysicalExamTemplates = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #LiPhysicalExamDataTemplate');
        objtabPhysicalExamTemplates.trigger("click");
        var objtabAddPhysExam = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #LiAddPhysicalExam');
        objtabAddPhysExam.addClass("hidden");
    },

    //Author: Muhammad Arshad
    //Date: 23/06/2016
    //This function will load PhysicalExam Data Template
    loadPhysicalExamSystem: function (selectedData) {


        PhysicalExamDataTemplateDetail.fillPhysicalExamDataTemplate().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);

                if (response.status != false) {

                    var setDataInTempDataJson = function () {

                        dbSelectedData = JSON.parse(response.dataResponse);
                        PhysicalExamDataTemplateDetail.DBDataJSON = [];
                        if (dbSelectedData != null) {
                            $.each(dbSelectedData, function (index, item) {
                                PhysicalExamDataTemplateDetail.DBDataJSON[index] = item;
                            });
                        }
                    }
                    //set in data json
                    $.when(setDataInTempDataJson()).then(PhysicalExamDataTemplateDetail.pushInDataJson());

                    var DataTemplateDetails = JSON.parse(response.PhysExamDataTemplateFill_JSON);
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #txtComments').val(DataTemplateDetails[0].Comments);
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #txtDataTemplateName').val(DataTemplateDetails[0].DataTemplateName);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {

            }
            //Load sys/sec/char/subChar
            PhysicalExamDataTemplateDetail.loadPhysicalExamStatuses(0, "mainpesystem", 0);
        });

    },

    pushInDataJson: function () {
        for (var index in PhysicalExamDataTemplateDetail.DBDataJSON) {
            if (PhysicalExamDataTemplateDetail.DBDataJSON[index].PhysicalExamSystemId != null && parseInt(PhysicalExamDataTemplateDetail.DBDataJSON[index].PhysicalExamSystemId) > 0) {
                PhysicalExamDataTemplateDetail.DataJSON.push(PhysicalExamDataTemplateDetail.DBDataJSON[index]);
            }
        }

    },
    //Function Name: setIsNormalSystems
    //Author Name: Humaira Yousaf
    //Created Date: 11-02-2016
    //Description: Sets normal system green on load
    //Params: var normalSystemIds
    setIsNormalSystems: function (normalSystemIds) {
        PhysicalExamDataTemplateDetail.SectionNormalInfo = [];
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li').each(function (index, item) {

            for (var i = 0; i < normalSystemIds.length; i++) {
                if ($(item).attr('id') == normalSystemIds[i]) {
                    $(item).addClass("green");
                    PhysicalExamDataTemplateDetail.SectionNormalInfo.push(parseInt(normalSystemIds[i]));
                    break;
                }
            }
        });
    },


    //Author: Farooq Ahmad
    //Date:04/02/2016
    //This function will bind user systems to form
    bindPhysicalExamUserSystem: function (Crtl, arrComponents, currentLiClick, ParentDiv, StatusType, selectedData) {

        if (typeof selectedData == "undefined" || selectedData == null)
            selectedData = PhysicalExamDataTemplateDetail.selectedData;

        var l = $(Crtl);

        l.empty();

        var systemsDetail = JSON.parse(selectedData);


        var currentLiClass;
        var isFirstLi = true;


        var activeLiId;
        var activeLiComponentName;

        $.each(arrComponents, function (j, item) {
            if (item.SystemOrder != "") {
                if (isFirstLi == true) {
                    currentLiClass = 'class="active"';

                    activeLiId = item.SystemOrder;
                    activeLiComponentName = item.ShortName.toLowerCase();

                    isFirstLi = false;
                }
                else {
                    currentLiClass = "";
                }

                var onClick = "";

                onClick = "PhysicalExamDataTemplateDetail.showHideChildControls('ulPhysicalExamSystems', '" + item.SystemId + "', event);"

                l.append('<li id="' + item.SystemId + '" ' + currentLiClass + ' onmouseover="PhysicalExamDataTemplateDetail.showDeleteIcon(this)" onmouseout="PhysicalExamDataTemplateDetail.hideDeleteIcon(this)"  onclick="' + onClick + '" value=' + item.SystemOrder + ' refValue="' + item.SystemOrder + '"><a href="#' + ParentDiv + '">' + item.ShortName + ' <span style="dispaly:none;" class="removeIconListHover" onclick="PhysicalExamDataTemplateDetail.physicalExamDelete(' + item.SystemId + ')"><i class="fa fa-times"></i></span></a></li>');
            }
        });
        PhysicalExamDataTemplateDetail.toggleVerticalWidth();

        //4/2/2016 Farooq Ahmad//For Sortable
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems').sortable({
            update: function (evt) {
                var obj = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems').sortable('toArray');
                var OrderOfExams = '';
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li').each(function () {

                    OrderOfExams = $(this).attr("value") + ',' + OrderOfExams;
                });
                if (OrderOfExams.length > 0) {
                    OrderOfExams = OrderOfExams.substring(0, OrderOfExams.length - 1);
                    PhysicalExamDataTemplateDetail.updateSectionOrderSorting(OrderOfExams);
                }
            }
        });

        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems').disableSelection();
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li').each(function () {

            $thisLi = $(this);
            $thisLi.removeClass("active");
            //here to change selected item
            if ($thisLi.attr("id") == $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first').attr("parentid")) {
                $thisLi.addClass("active");
            }
            ////start/Abid Ali/ make systems green and save sections in system list
            //var id = $thisLi.attr("id");

            //if (systemsDetail != null && typeof systemsDetail != 'undefined' && systemsDetail.length > 0) {
            //    $.each(systemsDetail, function (i, systemComponent) {
            //        if (systemComponent.SystemId == id) {

            //            if (!systemComponent.IsNormal && systemComponent.Sections.length <= 0)
            //                $thisLi.removeClass("green");
            //            else
            //                $thisLi.addClass("green").data('SystemSectionIds_' + id, systemComponent);
            //        }
            //    });

            //}
            //if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #chkPhysicalExamsNormal').is(":checked"))
            //    $thisLi.addClass("green");
            ////End/Abid Ali/ make systems green and save sections in system list
        });

    },

    //Author: Farooq Ahmad
    //Date:04/02/2016
    //This function will show Delete Icon of li
    showDeleteIcon: function (li) {

        if ($(li).hasClass('green'))
            $(li).find(".removeIconListHover").show();
    },

    //Author: Farooq Ahmad
    //Date:04/02/2016
    //This function will hide Delete Icon of li
    hideDeleteIcon: function (li) {
        $(li).find(".removeIconListHover").hide();
    },

    // Date: 04/02/2016
    // Author: Farooq Ahmad
    //This function will handle sorting of user systems
    updateSectionOrderSorting: function (SectionSorted) {
        var strMessage = "";

        PhysicalExamDataTemplateDetail.updateSectionOrderSorting_Dbcall(SectionSorted).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var arrComponents = JSON.parse(response.PhysicalExamSystem_JSON);
                    PhysicalExamDataTemplateDetail.bindPhysicalExamUserSystem($('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems'), arrComponents);

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });

    },


    // Date: 04/02/2016
    // Author: Farooq Ahmad
    //This function will handle db call to sort user systems
    updateSectionOrderSorting_Dbcall: function (SectionSorted) {
        var objData = new Object();
        objData["SystemCustomSorted"] = SectionSorted;
        objData["UserId"] = globalAppdata['AppUserId'];
        objData["commandType"] = "UPDATE_SECTIONORDERSORTING";
        objData["TemplateId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExamUserSystem");
    },

    //Author: Muhammad Arshad
    //Date: 14-01-2016
    //This function will handle Initialization of KeyPad control
    domReadyFunction: function () {
        $(function () {


            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            //EMR-70 Bug number Resolution
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail [data-plugin-keyboard-numpad]').keyboard({
                customLayout: {
                    'default': [
                        '7 8 9 {b}',
                        '4 5 6 {clear}',
                        '1 2 3 {t}',
                        '0   .  {a} {c} '
                    ]
                },
                change: function (e, keyboard, el) {
                    if (keyboard.$preview.attr('maxlength') != null && !keyboard.$preview.keyboard().getkeyboard().options.maxLength) {
                        keyboard.$preview.keyboard().getkeyboard().options.maxLength = keyboard.$preview.attr('maxlength');
                    }
                    if (keyboard.$preview.attr('oninput') != null) {
                        keyboard.$preview.trigger('oninput');
                    }
                    // Fix # EMR-96
                    if (keyboard.$preview.attr('name') == 'Height') {
                        if (keyboard.$preview.attr('onkeyup') != null) {
                            keyboard.$preview.trigger('onkeyup');
                            EMRUtility.ValidateHeight(e, keyboard.$preview);
                        }
                    } else if (keyboard.$preview.attr('onkeyup') != null) {
                        keyboard.$preview.trigger('onkeyup');
                    }

                },
                layout: 'custom',
                reposition: true,
                appendLocally: this,
                restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
                preventPaste: true,  // prevent ctrl-v and right click
                usePreview: false,
                autoAccept: true,
                tabNavigation: true
            })
                  .addTyping();

        });

        $(".toggleVertical div.toggle").click(function (e) {
            if ($(this).children().hasClass("active")) {
                $(this).prev().removeClass("hidden");

                PhysicalExamDataTemplateDetail.toggleVerticalWidth(e);
                $(this).parent().parent().scrollLeft(1000);
            }
            else {
                $(this).prev().addClass("hidden");
                PhysicalExamDataTemplateDetail.toggleVerticalWidth(e);
                $(this).parent().parent().scrollLeft(0);
            }
        });

        $('[data-plugin-toggle]').each(function () {
            var $this = $(this),
                opts = {};

            var pluginOptions = $this.data('plugin-options');
            if (pluginOptions)
                opts = pluginOptions;

            $this.themePluginToggle(opts);
        });


        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails').find('select,input,textarea').each(function () {

            $(this).change(function () {
                PhysicalExamDataTemplateDetail.UpdateJSONDetailSection();
                //alert('update JSON For detail');
            });


            //.on('change', function () {
            //                alert('update JSON For detail');
            //            });

        });

    },

    //Author: Muhammad Arshad
    //Date: 18-01-2016
    //This function will handle filtering of PhysicalExam Characteristics/Sub Characteristics
    filterOptions: function (obj, ulId) {
        if (obj != null && ulId != null) {
            var strSearch = $(obj).val();
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #" + ulId + " li").each(function () {
                var currentLiText = $(this).text();
                var showCurrentLi = currentLiText.toLowerCase().indexOf(strSearch.toLowerCase()) > -1 ? true : false;
                $(this).toggle(showCurrentLi);
            });

        }
    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will check if any characteristic/subcharacteristic is selected
    isCharacteristicChecked: function (objLi) {
        var DetailExists = false;
        objulSystem.find("li").each(function (i, item) {
            $(objLi).find("input[type='checkbox']").each(function (i, chkitem) {
                if ($(chkitem).prop("checked") == true && DetailExists == false) {
                    DetailExists = true;
                }
            });
        });

        return DetailExists
    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will return Characteristic Type
    getCharacteristicType: function (liObject) {
        var isSubcharacteristic = $(liObject).parent().attr("id").indexOf("SubCharacteristics") > -1 ? true : false;
        var characteristicType = isSubcharacteristic == true ? "Sub-Characteristic" : "Characteristic";
        return characteristicType;
    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will handle show/hide of Detail Textbox
    openNormalExamDetail: function (objButton, detailParentId) {
        if (objButton != null) {
            var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail")
            var isMainNormalChecked = self.find("input[id='chkPhysicalExamsNormal']").prop("checked");
            if (isMainNormalChecked == true) {
                var NormalExamsDetailDiv = self.find("div#divNormalExamsDetail");
                if (NormalExamsDetailDiv.hasClass("hidden") == true) {
                    NormalExamsDetailDiv.removeClass("hidden")
                }
                else {

                    //Abid Ali/ for making the book icon blue of normal checkbox when textbox is empty

                    /* var normalExamDiv = self.find("div#divNormalExams");
                     var normalExamDetailText = NormalExamsDetailDiv.find('#txtNormalExamsDetail').val();
                     if (normalExamDetailText == '') {

                         normalExamDiv.find('#btnNormalExamDetails').find('i').removeClass('green').addClass('blue');
                     }
                     else {
                         normalExamDiv.find('#btnNormalExamDetails').find('i').removeClass('blue').addClass('green');
                     } */

                    NormalExamsDetailDiv.addClass("hidden")
                }
            }
            else {
                utility.DisplayMessages("Please check Normal Exam to view details", 3);
            }
            //.removeClass("hidden");
        }
    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will handle show/hide of Detail Textbox
    openCharacteristicDetail: function (objButton, detailParentId) {
        if (objButton != null && detailParentId != null) {
            var liObject = $(objButton).parent().parent();
            var characteristicType = PhysicalExamDataTemplateDetail.getCharacteristicType(liObject);
            var pkid = $(liObject).prop("id");
            var type = $(liObject).closest("ul").prop("id");
            var isParentChecked = PhysicalExamDataTemplateDetail.isSystemChecked(null, liObject);
            if (isParentChecked == true) {
                var SystemDetailDiv = $(objButton).parent().find("div#divDetail" + detailParentId);
                if (SystemDetailDiv.hasClass("hidden") == true) {
                    SystemDetailDiv.removeClass("hidden");
                    var comments = PhysicalExamDataTemplateDetail.FillCommentsFromJSON(pkid, type)
                    $(SystemDetailDiv).find('textarea').val(comments);
                    $(SystemDetailDiv).find('textarea').text(comments);
                    $(SystemDetailDiv).find('textarea').focus();
                }
                else {
                    SystemDetailDiv.addClass("hidden")
                    var comments = $(SystemDetailDiv).find('textarea').val();
                    PhysicalExamDataTemplateDetail.updateTheCommentsOfCharAndSubChar(comments, pkid, type);
                    $(SystemDetailDiv).find('textarea').focusout();
                }
            }
            else {
                utility.DisplayMessages("Please mark this " + characteristicType + " as +ve/-ve to view detail", 3);
            }
            //.removeClass("hidden");
        }
    },


    FillCommentsFromJSON: function (pkId, type) {

        if (type.toLowerCase() == "ulexamsubcharacteristics") {

            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                    for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                        for (var subcharIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                            if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId == pkId) {
                                return PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].Comments;
                            }
                        }
                    }
                }
            }
        }
        else if (type.toLowerCase() == "ulexamcharacteristics") {

            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                    for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == pkId) {
                            return PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].Comments;
                        }
                    }
                }
            }
        }
        return "";
    },
    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will handle save/update of Detail checkbox
    saveCharacteristicDetail: function (event, txt) {

        $(txt).closest("li").find('button[id*="btnOpenDetail"]').trigger('click');

    },

    //Author: Muhammad Arshad
    //Date: 22-01-2016
    //This function will Save Normal system's inner detail
    saveExamSystemNormalDetail: function (obj, ctrlDetail, parentLiId) {

        //Start Farooq Ahmad 0/03/2016 if obj is not a button then find button and store its reference into obj
        try {
            if (typeof obj.id == 'undefined' || obj.id == null) {
                obj = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail ").find("a[id*=btnSaveNormalSystemDetail]")
                if (obj.length > 0)
                    obj = obj[0];
            }
        } catch (ex) {
            console.log(ex);
        }
        //End Farooq Ahmad 0/03/2016 if obj is not a button then find button and store its reference into obj


        var comments = obj.parentElement.parentElement.firstElementChild.firstElementChild.value;
        var physicalExamId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();

        if (comments != "") {
            var myJSON = "{}";
            var objData = JSON.parse(myJSON);
            objData["PhysicalExamSystemId"] = parentLiId;
            objData["SystemId"] = parentLiId;
            objData["PatientPhysicalExamId"] = physicalExamId;
            objData["isFromNormalComments"] = true;
            objData["NormalComments"] = comments;
            objData["NotesId"] = Clinical_Notes.params.NotesId;

            var myJSON = JSON.stringify(objData);

            PhysicalExamDataTemplateDetail.saveDetailforNormalSystem(myJSON, physicalExamId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    // //     $(currentLiObject).addClass('green').find('.rightInnerAddon').addClass('hidden');
                    //     $(currentLiObject).find("button[id*='btnOpenDetail']").addClass('green');
                    //Begin 28-03-2016 Edit By Humaira Yousaf Bug# EMR-452
                    //  $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first").find('a#btnNormalSectionDetails > i').removeClass('blue').addClass('green');
                    //End 28-03-2016 Edit By Humaira Yousaf Bug# EMR-452
                    utility.DisplayMessages(response.message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            utility.DisplayMessages("Please enter detail.", 3);
        }


    },
    //End//18-02-2016//Ahmad Raza//Saving Normal system's inner detail

    //Author: Farooq Ahmad
    //Date: 13-06-2016
    //This function will handle show/hide of PhysicalExam child controls

    showHideChildControls: function (parentCtrl, liId, event) {

        try {
            if (event.target.parentNode.className == "removeIconListHover") {
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #ulPhysicalExamSystems  li#' + liId).removeClass('green');
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #SystemSections').addClass('hidden');
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #SectionCharacteristics').addClass('hidden');
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #CharacteristicsSubCharacteristics').addClass('hidden');
                PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
                return;
            }
        }
        catch (ex) {
            console.log(ex);
        }

        PhysicalExamDataTemplateDetail.parentCtrlGlobel = parentCtrl;

        //$('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #sectionPhysicalExamDetails").addClass('hidden');

        if (parentCtrl == "ulPhysicalExamSystems") {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #divTogglePhysicalExamDetails").addClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SystemSections").addClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SectionCharacteristics").addClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        if (parentCtrl == "ulPhysicalExamSystems" && liId != $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li.active").attr("id")) {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #divTogglePhysicalExamDetails").addClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SectionCharacteristics").addClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics").addClass('hidden');
        }
        else if ((parentCtrl == "ulPhysicalExamSystemSection" && liId != $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li.active").attr("id"))) {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #divTogglePhysicalExamDetails").addClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails").addClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics").addClass('hidden');

        }

        if (parentCtrl != null && parentCtrl != "") {

            var childPartialId = "";
            var isSystemSectionCtrl = "";
            var isCharacteristicsCtrl = "";
            var isSubCharacteristicsCtrl = "";

            if (parentCtrl.toLowerCase() == "ulphysicalexamsystems") {
                isSystemSectionCtrl = "1";
                childPartialId = "System";
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section[id^='SectionCharacteristics']").addClass("hidden");

            }
            else if (parentCtrl.toLowerCase() == "ulphysicalexamsystemsection") {
                isSystemSectionCtrl = "1";
                childPartialId = "Section";
            }
            else if (parentCtrl.toLowerCase() == "ulexamcharacteristics") {
                isCharacteristicsCtrl = "1";
                childPartialId = "Characteristics";
            }
            else if (parentCtrl.toLowerCase() == "ulexamsubcharacteristics") {
                isSubCharacteristicsCtrl = "1";
                childPartialId = "SubCharacteristics";
            }

            if (liId != null && liId != "") {
                $('#' + PhysicalExamDataTemplateDetail.parentCtrlGlobel).find("li").each(function (i, item) {
                    if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                        $(this).closest("ul").find("li").removeClass('active');
                        $(this).addClass('active');
                        var objCurrent = item;



                        $.when(PhysicalExamDataTemplateDetail.loadChildData(liId, childPartialId)).then(function () {

                            PhysicalExamDataTemplateDetail.toggleDetailsDiv();
                            var objectListItem = null;
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam").css('width', 'auto');
                            if (PhysicalExamDataTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystems") {
                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SystemSections").removeClass('hidden');
                                //PhysicalExamDataTemplateDetail.manageJsonData(liId, 'system');
                            }
                            else if (PhysicalExamDataTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulphysicalexamsystemsection") {
                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SectionCharacteristics").removeClass('hidden');
                            }
                            else if (PhysicalExamDataTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                objectListItem = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #ulExamCharacteristics  li#' + liId);
                                var isbothUnCheck = ($(objectListItem).find('input[type=checkbox]:checked').length == 0);

                                var haveChild = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li").length > 0;

                                if (!isbothUnCheck && haveChild)
                                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics").removeClass('hidden');
                                else
                                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics").addClass('hidden');

                                try {
                                    var detail = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");

                                    PhysicalExamDataTemplateDetail.manageJsonData(liId, 'characteristics', detail);


                                } catch (ex) {
                                    console.log(ex);
                                }

                            }
                            else if (PhysicalExamDataTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulexamsubcharacteristics") {
                                try {
                                    objectListItem = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #ulExamSubCharacteristics  li#' + liId);
                                    var detail = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");

                                    PhysicalExamDataTemplateDetail.manageJsonData(liId, 'subcharacteristics', detail);
                                } catch (ex) {
                                    console.log(ex);
                                }

                                isSubCharacteristicsCtrl = "1";
                                childPartialId = "SubCharacteristics";
                            }
                            if (PhysicalExamDataTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics" || PhysicalExamDataTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulexamsubcharacteristics") {

                                var hideDetailSection = true;
                                if ($(objectListItem).find('input[type=checkbox]:checked').length == 0) {
                                    // PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
                                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #sectionPhysicalExamDetails").addClass("hidden");
                                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #divTogglePhysicalExamDetails").addClass("hidden");
                                } else {
                                    PhysicalExamDataTemplateDetail.toggleDetailsDiv();
                                    PhysicalExamDataTemplateDetail.showHideToggleDetails(true);
                                    if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #divTogglePhysicalExamDetails .active").length > 0)
                                        hideDetailSection = false;
                                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #detailHeading ").text($(objectListItem).text() + " Detail");
                                    if (PhysicalExamDataTemplateDetail.parentCtrlGlobel.toLowerCase() == "ulexamcharacteristics") {
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails").find("#hfCharacteristicId").val(liId);
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails").find("#hfSubCharacteristicId").val('-1');
                                    }
                                    else {
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails").find("#hfCharacteristicId").val('-1');
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails").find("#hfSubCharacteristicId").val(liId);
                                    }

                                }
                                if (!hideDetailSection) {
                                    PhysicalExamDataTemplateDetail.toggleVerticalWidth();
                                    $(".toggleVertical div.toggle").parent().parent().scrollLeft(1000);
                                }

                                PhysicalExamDataTemplateDetail.BindDetailsOfCharAndSubChar(liId, PhysicalExamDataTemplateDetail.parentCtrlGlobel.toLowerCase());
                            }


                            PhysicalExamDataTemplateDetail.addGreenClasses();
                        });
                    }
                });
            }
        }
    },
    //Author:  Farooq Ahmad
    //Date:    14/06/2016
    //This Function will add green class if the current Data is in JSON
    addGreenClasses: function () {

        if (PhysicalExamDataTemplateDetail.DataJSON == null) {
            PhysicalExamDataTemplateDetail.DataJSON = [];
            return;
        }
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').find("#ulPhysicalExamSystems li,#ulPhysicalExamSystemSection li,#ulExamCharacteristics li,#ulExamSubCharacteristics li").removeClass('green');

        for (var index in PhysicalExamDataTemplateDetail.DataJSON) {

            var SystemId = PhysicalExamDataTemplateDetail.DataJSON[index].SystemId;
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').find("#ulPhysicalExamSystems li#" + SystemId).addClass('green');

            //Start//Added by Abid For normal section
            var selectedSystemId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').find("#ulPhysicalExamSystems li.active").attr('id');
            if (PhysicalExamDataTemplateDetail.DataJSON[index].IsNormal && selectedSystemId == SystemId) {
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').find("#ulPhysicalExamSystemSection li:first").find('input[type="checkbox"]').trigger('click')
            }
            //End//Added by Abid For normal section
            if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections.length > 0) {

                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {

                    var SectionId = PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].SectionId;
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').find("#ulPhysicalExamSystemSection li#" + SectionId).addClass('green');


                    if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics.length > 0) {

                        for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {

                            var CharacteristicId = PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId;

                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').find("#ulExamCharacteristics li#" + CharacteristicId).addClass('green');

                            if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].IsPositive.toString().toLowerCase() == "true") {
                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + CharacteristicId).find("input[type='checkbox'][id*='+ve']").prop("checked", true);
                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + CharacteristicId).find("input[type='checkbox'][id*='-ve']").prop("checked", false);
                            }
                            else {
                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + CharacteristicId).find("input[type='checkbox'][id*='+ve']").prop("checked", false);
                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + CharacteristicId).find("input[type='checkbox'][id*='-ve']").prop("checked", true);
                            }

                            if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].Comments && PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].Comments.length > 0) {
                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + CharacteristicId).find("button[id*='btnOpenDetail']").addClass("green");
                            }

                            if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics.length > 0) {

                                for (var subcharIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                                    var SubCharacteristicId = PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId;
                                    if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].IsPositive.toString().toLowerCase() == "true") {
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li#' + SubCharacteristicId).find("input[type='checkbox'][id*='+ve']").prop("checked", true);
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li#' + SubCharacteristicId).find("input[type='checkbox'][id*='-ve']").prop("checked", false);
                                    }
                                    else {
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li#' + SubCharacteristicId).find("input[type='checkbox'][id*='+ve']").prop("checked", false);
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li#' + SubCharacteristicId).find("input[type='checkbox'][id*='-ve']").prop("checked", true);
                                    }

                                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').find("#ulExamSubCharacteristics li#" + SubCharacteristicId).addClass('green');
                                    if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].Comments && PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].Comments.length > 0) {
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li#' + SubCharacteristicId).find("button[id*='btnOpenDetail']").addClass("green");
                                    }

                                }
                            }

                        }
                    }
                }
            }
        }
    },

    //Author: Farooq Ahmad
    //Date: 09/06/2016
    //This Function will manage the JSON Array
    manageJsonData: function (pkId, type, detail, ISNORMAL, noramalDetail, isPositive) {
        if (PhysicalExamDataTemplateDetail.DataJSON == null) {
            PhysicalExamDataTemplateDetail.DataJSON = [];
            PhysicalExamDataTemplateDetail.getFromAlreadyAdded();
        }
        var selectedJSON = null;
        //if (detail != null && $(detail).css("visibility") != "hidden") {
        //    var myJSON = detail != null ? detail.getMyJSONByName() : "{}";
        //    selectedJSON = JSON.parse(myJSON);
        //}
        if (type.toLowerCase() == "system") {
            PhysicalExamDataTemplateDetail.updateJSONArraySystem(pkId, ISNORMAL);
        }
        else if (type.toLowerCase() == "characteristics") {
            isCharacteristicsCtrl = "1";
            PhysicalExamDataTemplateDetail.updateJSONArrayChar(pkId, selectedJSON);
        }
        else if (type.toLowerCase() == "subcharacteristics") {
            isSubCharacteristicsCtrl = "1";

            PhysicalExamDataTemplateDetail.updateJSONArrayForSubChar(pkId, selectedJSON);

        }
        console.log(PhysicalExamDataTemplateDetail.DataJSON);
    },

    //Author :Farooq Ahmad
    //Date : 10-06-2016
    updateJSONArrayChar: function (pkId, selectedJSON) {
        var IsSystemExist = false, IsSectionExist = false, IsCharacteristicsExist = false;
        var sectionId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + pkId).attr('parentid');
        var systemId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li#' + sectionId).attr('parentid');
        if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + pkId).find('input:checked').length > 0) {
            var IsPositive = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + pkId).find('input:checked').attr('id').indexOf('-ve') > -1 ? false : true;
            var IsNegative = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + pkId).find("input[type='checkbox'][id*='-ve']").prop("checked");
            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                if (PhysicalExamDataTemplateDetail.DataJSON[index].SystemId == systemId) {
                    IsSystemExist = true;
                    for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].SectionId == sectionId) {
                            IsSectionExist = true;

                            for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                                if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == pkId) {
                                    IsCharacteristicsExist = true;
                                    if (selectedJSON != null)
                                        PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicDetailModel = selectedJSON;
                                    PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].IsPositive = IsPositive;
                                    PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].IsNegative = IsNegative
                                }
                            }
                            if (!IsCharacteristicsExist) {
                                PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics.push({ SectionCharacteristicId: pkId, IsPositive: IsPositive, IsNegative: IsNegative, SubCharacteristics: [], SectionCharacteristicDetailModel: selectedJSON });
                            }
                        }
                    }
                    if (!IsSectionExist) {
                        PhysicalExamDataTemplateDetail.DataJSON[index].Sections.push({ SectionId: sectionId, Characteristics: [{ SectionCharacteristicId: pkId, IsPositive: IsPositive, IsNegative: IsNegative, SubCharacteristics: [], SectionCharacteristicDetailModel: selectedJSON }] });
                    }
                }
            }
            if (!IsSystemExist) {
                var obj = {
                    SystemId: systemId,
                    Sections: [{ SectionId: sectionId, Characteristics: [{ SectionCharacteristicId: pkId, IsPositive: IsPositive, IsNegative: IsNegative, SubCharacteristics: [], SectionCharacteristicDetailModel: selectedJSON }] }]
                }
                PhysicalExamDataTemplateDetail.DataJSON.push(obj);
            }
        }
        else {
            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                if (PhysicalExamDataTemplateDetail.DataJSON[index].SystemId == systemId) {

                    for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].SectionId == sectionId) {

                            for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                                if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == pkId) {
                                    PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics.splice(charIndex, 1);

                                    if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics.length == 0) {
                                        PhysicalExamDataTemplateDetail.DataJSON[index].Sections.splice(secIndex, 1);
                                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections.length == 0) {
                                            PhysicalExamDataTemplateDetail.DataJSON.splice(index, 1);
                                        }
                                    }

                                    break;
                                }
                            }

                        }
                    }

                }
            }
        }
    },

    //Author :Farooq Ahmad
    //Date : 10-06-2016
    updateJSONArraySection: function (pkId) {
        var IsSystemExist = false, IsSectionExist = false;
        var sectionId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li#' + pkId).attr('id');
        var systemId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li#' + pkId).attr('parentid');


        for (var index in PhysicalExamDataTemplateDetail.DataJSON) {

            if (PhysicalExamDataTemplateDetail.DataJSON[index].SystemId == systemId) {

                IsSystemExist = true;

                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {

                    if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].SectionId == sectionId) {

                        IsSectionExist = true;
                    }
                }
                if (!IsSectionExist) {
                    PhysicalExamDataTemplateDetail.DataJSON[index].Sections.push({ SectionId: sectionId, Characteristics: [] });
                }
            }
        }

        if (!IsSystemExist) {
            var obj = {
                SystemId: systemId,
                Sections: [{ SectionId: sectionId, Characteristics: [] }]
            }
            PhysicalExamDataTemplateDetail.DataJSON.push(obj);
        }
        PhysicalExamDataTemplateDetail.addGreenClasses();
    },

    //Author : Farooq Ahmad
    //Date : 10-06-2016
    updateJSONArrayForSubChar: function (pkId, selectedJSON) {
        var IsSystemExist = false, IsSectionExist = false, IsCharacteristicsExist = false, IsSubCharacteristicsExist = false;

        var characteristicId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li#' + pkId).attr('parentid');
        var sectionId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + characteristicId).attr('parentid');
        var systemId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li#' + sectionId).attr('parentid');
        var IsNegative = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + pkId).find("input[type='checkbox'][id*='-ve']").prop("checked");
        if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li#' + pkId).find('input:checked').length > 0) {
            var IsPositive = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li#' + pkId).find('input:checked').attr('id').indexOf('-ve') > -1 ? false : true;
            var IsNegative = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li#' + pkId).find("input[type='checkbox'][id*='-ve']").prop("checked");
            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                if (PhysicalExamDataTemplateDetail.DataJSON[index].SystemId == systemId) {

                    for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].SectionId == sectionId) {

                            for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                                if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == characteristicId) {

                                    for (var subcharIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId == pkId) {
                                            IsSubCharacteristicsExist = true;
                                            if (selectedJSON != null)
                                                PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicDetailModel = selectedJSON;
                                            PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].IsPositive = IsPositive;
                                            PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].IsNegative = IsNegative;
                                        }
                                    }

                                    if (!IsSubCharacteristicsExist) {

                                        obj = {
                                            SubCharacteristicId: pkId,
                                            SubCharacteristicDetailModel: selectedJSON,
                                            IsPositive: IsPositive,
                                            IsNegative: IsNegative
                                        }
                                        PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics.push(obj);

                                    }

                                }
                            }

                        }
                    }

                }
            }

        }
        else {
            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                if (PhysicalExamDataTemplateDetail.DataJSON[index].SystemId == systemId) {

                    for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].SectionId == sectionId) {

                            for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                                if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == characteristicId) {

                                    for (var subcharIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId == pkId) {
                                            PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics.splice(subcharIndex, 1);
                                            if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics.length == 0) {
                                                PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics.splice(charIndex, 1);
                                                if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics.length == 0) {
                                                    PhysicalExamDataTemplateDetail.DataJSON[index].Sections.splice(secIndex, 1);
                                                    if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections.length == 0) {
                                                        PhysicalExamDataTemplateDetail.DataJSON.splice(index, 1);
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                    }

                                }
                            }

                        }
                    }

                }
            }
        }
    },

    //Author: Farooq Ahmad
    //Date: 13/06/2016
    //This Function will manage the JSON Array
    updateJSONArraySystem: function (systemId, ISNORMAL) {
        var isNormal = ISNORMAL == "True" ? true : false;
        var Comments = '';
        if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first').attr('parentid') == systemId) {
            isNormal = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first').find('#chkNormalSection').prop("checked");
            Comments = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first').find('#txtNormalSectionDetail').text();
        }
        var obj = {
            Id: 0,
            SystemId: systemId,
            IsNormal: isNormal,
            Comments: Comments,
            Sections: []
        };
        var isExist = false;
        for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
            if (PhysicalExamDataTemplateDetail.DataJSON[index].SystemId == systemId) {
                isExist = true;
                PhysicalExamDataTemplateDetail.DataJSON[index].IsNormal = isNormal;
                PhysicalExamDataTemplateDetail.DataJSON[index].Comments = Comments;
                if (isNormal) {
                    PhysicalExamDataTemplateDetail.DataJSON[index].Sections = [];
                }
            }
        }

        if (!isExist) {

            PhysicalExamDataTemplateDetail.DataJSON.push(obj);
        }
        else {
            if (!isNormal) {
                PhysicalExamDataTemplateDetail.physicalExamDataTemplateDelete(systemId, false);
            }
        }
    },

    getSystemFromJson: function (systemId) {
        var systemObj = null;
        for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
            if (PhysicalExamDataTemplateDetail.DataJSON[index].SystemId == systemId) {
                systemObj = PhysicalExamDataTemplateDetail.DataJSON[index];
                return systemObj;
            }
        }
    },
    //Author: Farooq Ahmad
    //Date: 09/06/2016
    //This Function will manage the JSON Array
    getFromAlreadyAdded: function () {
        var loadedData = JSON.parse(PhysicalExamDataTemplateDetail.selectedData);

        PhysicalExamDataTemplateDetail.DataJSON = [];

        for (var index in loadedData) {
            obj = {
                Id: loadedData[index].Id,
                SystemId: loadedData[index].SystemId,
                Sections: []
            };
            for (var secIndex in loadedData[index].Sections) {
                objSec = {
                    Id: loadedData[index].Sections[secIndex].Id,
                    SectionId: loadedData[index].Sections[secIndex].SectionId,
                    Characteristics: []
                };
                for (var charIndex in loadedData[index].Sections[secIndex].Characteristics) {

                    objchar = {
                        Id: loadedData[index].Sections[secIndex].Characteristics[charIndex].Id,
                        SectionCharacteristicId: loadedData[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId,
                        SectionCharacteristicDetailModel: loadedData[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicDetailModel,
                        IsPositive: loadedData[index].Sections[secIndex].Characteristics[charIndex].IsPositive,
                        SubCharacteristics: []
                    }
                    for (var subCharIndex in loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {

                        objsubchar = {
                            Id: loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex].Id,
                            SubCharacteristicId: loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristicId,
                            SubCharacteristicDetailModel: loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex].SubCharacteristicDetailModel,
                            IsSubCharacteristicPositive: loadedData[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex].IsSubCharacteristicPositive
                        }
                        objchar.SubCharacteristics.push(objsubchar);
                    }
                    objSec.Characteristics.push(objchar);
                }
                obj.Sections.push(objSec);
            }
            PhysicalExamDataTemplateDetail.DataJSON.push(obj);
        }
    },

    //Author : Farooq Ahmad
    //Date : 15/06/2016
    //This Function will manage the detail section of characterisctics and subcharaterisctics
    UpdateJSONDetailSection: function () {
        var detailJSON = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails').getMyJSONByName();
        var detail = JSON.parse(detailJSON);
        detail.SubCharacteristicId = detail.SubCharacteristicId == "" ? -1 : detail.SubCharacteristicId
        if (parseInt(detail.CharacteristicId) > 0 && parseInt(detail.SubCharacteristicId) < 0) {
            PhysicalExamDataTemplateDetail.updateJSONArrayChar(detail.CharacteristicId, detail);
        }
        else if (parseInt(detail.SubCharacteristicId) > 0) {
            PhysicalExamDataTemplateDetail.updateJSONArrayForSubChar(detail.SubCharacteristicId, detail);
        }

    },


    //Author: Abid Ali
    //Date: 25-01-2016
    //This function check for null and undefined of an object
    isNullOrUndefined: function (object) {
        return object == null && typeof object == 'undefined' ? true : false;
    },

    selectAllCharacteristics: function (objcheckAll, IsPostive) {
        if (objcheckAll != null) {

            var pstivePartialId = "+ve";
            var ngtivePartialId = "-ve";

            var isChecked = $(objcheckAll).prop("checked");


            var parentUlCtrl = $(objcheckAll).closest("ul");

            if (IsPostive == true) {
                if (isChecked == true) {
                    $(parentUlCtrl).find(" input[id*='" + ngtivePartialId + "']").each(function () {
                        $(this).prop("checked", !isChecked);
                    });
                }
                $(parentUlCtrl).find(" input[id*='" + pstivePartialId + "']").each(function () {
                    $(this).prop("checked", isChecked);
                });
            }
            else if (IsPostive != true) {
                if (isChecked == true) {
                    $(parentUlCtrl).find(" input[id*='" + pstivePartialId + "']").each(function () {
                        $(this).prop("checked", !isChecked);
                    });
                }
                $(parentUlCtrl).find(" input[id*='" + ngtivePartialId + "']").each(function () {
                    $(this).prop("checked", isChecked);
                });

            }
            PhysicalExamDataTemplateDetail.parentCtrlGlobel = $(parentUlCtrl).attr('id');
            $(parentUlCtrl).find('li').each(function () {
                if ($(this).attr("id") != 'undefined') {
                    if (PhysicalExamDataTemplateDetail.parentCtrlGlobel == "ulExamCharacteristics")
                        PhysicalExamDataTemplateDetail.updateJSONArrayChar($(this).attr("id"));
                    else if (PhysicalExamDataTemplateDetail.parentCtrlGlobel == "ulExamSubCharacteristics")
                        PhysicalExamDataTemplateDetail.updateJSONArrayForSubChar($(this).attr("id"));
                }
            });

            PhysicalExamDataTemplateDetail.addGreenClasses();
        }
    },

    //Author: Ahmad Raza
    //Date: 25-01-2016
    //This function get number part of an object
    getNumberPart: function (obj) {
        var innernumericPart = 0;
        $.each(obj, function (i, item) {
            if (i.indexOf("SystemId") > -1) {
                innernumericPart = i.replace(/[^\d]+/, '');
                return innernumericPart;
            }
        });
        return innernumericPart;
    },

    //Author: Abid Ali
    //Date: 25-01-2016
    //This function get value of the inputed key of an object
    getValueFromObject: function (obj, key) {
        var value = null;
        $.each(obj, function (i, item) {
            if (i.indexOf(key) > -1) {
                value = item;
            }
        });
        return value;
    },

    //Author: Abid Ali
    //Date: 25-01-2016
    //This function check systems present in PhysicalExamDataTemplateDetail.myArr
    checkSystemInPhysicalExamMyArr: function (systemId) {

        var exists = false;
        $.each(PhysicalExamDataTemplateDetail.myArr, function (index, item) {
            if (systemId == PhysicalExamDataTemplateDetail.getValueFromObject(item, 'SystemId')) {
                exists = true;
                return exists;
            }
        });
        return exists;
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will check if any characteristic/subcharacteristic has data in details section
    toggleDetailsDiv: function (liId, isReplace) {
        var objDeffered = $.Deferred();
        var sectionDetails = "";
        var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,[type=hidden]').each(function () {
            PhysicalExamDataTemplateDetail.resetControlValue($(this));
        });
        objDeffered.resolve();
        return objDeffered;
    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will handle toggling of +ve/-ve checkboxes
    toggleCheckBoxes: function (chkObject) {

        if ($(chkObject).prop('checked')) {
            $(chkObject).parent().find("input[type=checkbox]").prop('checked', false);
            $(chkObject).prop('checked', true);
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails").removeClass('hidden');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #detailHeading ").text($(chkObject).closest('li').text() + " Detail");

            if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                PhysicalExamDataTemplateDetail.toggleDetailsDiv();
                var CharacteristicId = $(chkObject).closest('li').attr("id");

            }
            else {
                PhysicalExamDataTemplateDetail.toggleDetailsDiv();
                var SubCharacteristicId = $(chkObject).closest('li').attr("id");

            }

        }
        else {
            var isbothUnCheck = true;
            $(chkObject).parent().find("input[type=checkbox]").each(function () {
                if ($(this).prop('checked')) {
                    isbothUnCheck = false;
                }
            });
            if (isbothUnCheck) {
                setTimeout(function (chkObject) {
                    $(chkObject).closest('li').removeClass("active");
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics");

                    if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail  #CharacteristicsSubCharacteristics").addClass('hidden');;
                    }
                    PhysicalExamDataTemplateDetail.toggleDetailsDiv();
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails").addClass('hidden');

                }, 100, chkObject);
            }
            else {
                PhysicalExamDataTemplateDetail.toggleDetailsDiv();
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails").removeClass('hidden');
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #detailHeading ").text($(chkObject).closest('li').text() + " Detail");

                if ($(chkObject).closest('ul').attr("id").toLowerCase() == "ulexamcharacteristics") {
                    PhysicalExamDataTemplateDetail.toggleDetailsDiv();
                    var CharacteristicId = $(chkObject).closest('li').attr("id");

                }
                else {
                    PhysicalExamDataTemplateDetail.toggleDetailsDiv();
                    var SubCharacteristicId = $(chkObject).closest('li').attr("id");

                }
            }
        }

    },

    //Author: Abid Ali
    //Date: 14-03-2016
    //This function will return selected dataobject by passing partType and partId
    getComponentFromSelectedData: function (partType, partId) {
        var component = null;
        var isComponentFound = false;
        var selectedData = JSON.parse(PhysicalExamDataTemplateDetail.selectedData);

        for (var systemIndex = 0; systemIndex < selectedData.length; systemIndex++) {

            if (selectedData[systemIndex].SystemId == partId && partType.toLowerCase() == "system") {
                component = selectedData[systemIndex];
                isComponentFound = true;

            }
            if (partType.toLowerCase() == "section") {
                for (var sectionIndex = 0; sectionIndex < selectedData[systemIndex].Sections.length; sectionIndex++) {

                    if (selectedData[systemIndex].Sections[sectionIndex].SectionId == partId) {
                        component = selectedData[systemIndex].Sections[sectionIndex];
                        isComponentFound = true;
                        break;
                    }
                    if (partType.toLowerCase() == "char") {

                        for (var charIndex = 0; charIndex < selectedData[systemIndex].Sections[sectionIndex].Characteristics.length; charIndex++) {

                            if (selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex].SectionCharacteristicId == partId) {
                                component = selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex];
                                isComponentFound = true;
                                break;
                            }
                            if (partType.toLowerCase() == "subchar") {
                                for (var subCharIndex = 0; subCharIndex < selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex].SubCharacteristics.length; subCharIndex++) {

                                    if (selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex].CharSubCharacteristicId == partId) {
                                        component = selectedData[systemIndex].Sections[sectionIndex].Characteristics[charIndex].SubCharacteristics[subCharIndex];
                                        isComponentFound = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (isComponentFound) {
                break;
            }
        }
        return component;
    },

    //Author: Farooq Ahmad
    //Date:25-02-2016
    //This function will delete from JSon Object
    deleteFromJsonObject: function (type, selectedLiId) {
        var nameOfList = type;

        if (PhysicalExamDataTemplateDetail.selectedData != null && PhysicalExamDataTemplateDetail.selectedData != "") {
            var objSelectedDate = JSON.parse(PhysicalExamDataTemplateDetail.selectedData);


            isMatch = false;

            for (var SysIndex = 0; SysIndex < objSelectedDate.length; SysIndex++) {

                if (nameOfList.toLowerCase() == "ulphysicalexamsystems" && selectedLiId == objSelectedDate[SysIndex].SystemId) {
                    objSelectedDate.splice(SysIndex, 1);
                    break;
                }
                for (var secIndex = 0; secIndex < objSelectedDate[SysIndex].Sections.length; secIndex++) {

                    if (nameOfList.toLowerCase() == "ulPhysicalExamSystemSection" && selectedLiId == objSelectedDate[secIndex].SectionId) {
                        objSelectedDate[SysIndex].Sections.splice(secIndex, 1);
                        break;
                    }
                    for (var CharIndex = 0; CharIndex < objSelectedDate[SysIndex].Sections[secIndex].Characteristics.length; CharIndex++) {
                        if (nameOfList.toLowerCase() == "ulexamcharacteristics" && selectedLiId == objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SectionCharacteristicId.toString()) {
                            //delete objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex];
                            objSelectedDate[SysIndex].Sections[secIndex].Characteristics.splice(CharIndex, 1);
                            isMatch = true;
                            break;
                        }
                        else if (nameOfList.toLowerCase() == "ulexamsubcharacteristics") {
                            for (var SubCharIndex = 0; SubCharIndex < objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SubCharacteristics.length; SubCharIndex++) {
                                if (selectedLiId == objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SubCharacteristics[SubCharIndex].CharSubCharacteristicId.toString()) {
                                    // delete objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SubCharacteristics[SubCharIndex];
                                    objSelectedDate[SysIndex].Sections[secIndex].Characteristics[CharIndex].SubCharacteristics.splice(SubCharIndex, 1);
                                    isMatch = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (isMatch) break;
                }
                if (isMatch) break;
            }
            PhysicalExamDataTemplateDetail.selectedData = JSON.stringify(objSelectedDate);

        }
        if (PhysicalExamDataTemplateDetail.myArr != null) {

            var RemoveIndexArray = [];
            for (var count in PhysicalExamDataTemplateDetail.myArr) {
                var num = PhysicalExamDataTemplateDetail.getNumberPart(PhysicalExamDataTemplateDetail.myArr[count]);
                if (num == selectedLiId && nameOfList.toLowerCase() == "ulexamcharacteristics") {
                    count = parseInt(count);
                    RemoveIndexArray.push(count);
                    //PhysicalExamDataTemplateDetail.myArr.splice(count, 1);
                }
                else if (PhysicalExamDataTemplateDetail.myArr[count]["SystemId" + num] == selectedLiId && nameOfList.toLowerCase() == "ulphysicalexamsystems") {
                    count = parseInt(count);
                    RemoveIndexArray.push(count);
                    //PhysicalExamDataTemplateDetail.myArr.splice(count, 1);
                }
            }
            try {
                for (var removeIndex = RemoveIndexArray.length - 1; removeIndex > -1; removeIndex--) {
                    PhysicalExamDataTemplateDetail.myArr.splice(removeIndex, 1);
                }
            } catch (ex) {
                console.log(ex);
            }

        }

        if (PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId] != null) {
            if (nameOfList.toLowerCase() == "ulexamsubcharacteristics") {

                delete PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId];
                PhysicalExamDataTemplateDetail.toggleDetailsDiv('', true);
                PhysicalExamDataTemplateDetail.isBothUnCheck = true;
            }
            else if (nameOfList.toLowerCase() == "ulexamcharacteristics") {
                var DeletingIndexes = [];
                for (var count in PhysicalExamDataTemplateDetail.ExamDetails) {
                    var jsonExamDetail = JSON.parse(PhysicalExamDataTemplateDetail.ExamDetails[count]);

                    var num = PhysicalExamDataTemplateDetail.getNumberPart(jsonExamDetail);
                    if (jsonExamDetail["CharacteristicId" + num] == selectedLiId) {
                        DeletingIndexes.push(count);

                    }
                }

                for (var count in DeletingIndexes) {
                    delete PhysicalExamDataTemplateDetail.ExamDetails[DeletingIndexes[count]];
                }
            }
            else if (nameOfList.toLowerCase() == "ulphysicalexamsystems") {
                var DeletingIndexes = [];
                for (var count in PhysicalExamDataTemplateDetail.ExamDetails) {
                    var jsonExamDetail = JSON.parse(PhysicalExamDataTemplateDetail.ExamDetails[count]);

                    var num = PhysicalExamDataTemplateDetail.getNumberPart(jsonExamDetail);
                    if (jsonExamDetail["SystemId" + num] == selectedLiId) {
                        DeletingIndexes.push(count);

                    }
                }

                for (var count in DeletingIndexes) {
                    delete PhysicalExamDataTemplateDetail.ExamDetails[DeletingIndexes[count]];
                }
            }
        }

        if (nameOfList.toLowerCase() == "ulphysicalexamsystems") {
            var index = 0;
            var RemoveDetailExamIndexes = [];
            for (var count in PhysicalExamDataTemplateDetail.ExamDetails) {
                var jsonExamDetail = JSON.parse(PhysicalExamDataTemplateDetail.ExamDetails[count]);
                var num = PhysicalExamDataTemplateDetail.getNumberPart(jsonExamDetail);
                if (jsonExamDetail["SystemId" + num] == selectedLiId && nameOfList.toLowerCase() == "ulphysicalexamsystems") {
                    RemoveDetailExamIndexes.push(num);
                    //delete PhysicalExamDataTemplateDetail.ExamDetails[num];
                    //break;
                }
                index++;
            }
            for (var index = RemoveDetailExamIndexes.length - 1; index > -1; index--) {
                delete PhysicalExamDataTemplateDetail.ExamDetails[RemoveDetailExamIndexes[index]];
            }
            PhysicalExamDataTemplateDetail.toggleDetailsDiv('', true);
            PhysicalExamDataTemplateDetail.isBothUnCheck = true;
            // $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #ulPhysicalExamSystemSection #chkNormalSection").removeAttr("checked");
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #ulphysicalexamsystems li#" + selectedLiId).trigger('click');
            PhysicalExamDataTemplateDetail.setHiddenFieldValues('ulphysicalexamsystems', selectedLiId);

        }
    },

    //Author: Farooq Ahmad
    //Date:22-02-2016
    //This function will set the values in hidden field
    setHiddenFieldValues: function (currentUlId, currentId, parentObj) {
        var systemId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li.active').attr("id");
        var sectionId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li.active').attr("id");
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails input[id*="hfSystemId"]').val(systemId);
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails input[id*="hfSectionId"]').val(sectionId);
        var characteristicId = "";
        var isCharacteristicPostive = false;
        var isSubCharacteristicPostive = false;
        var subCharacteristicId = "";
        if (currentUlId.toLowerCase() == "ulexamcharacteristics") {
            if (currentId != null && currentId.indexOf("+ve") > -1) {
                isCharacteristicPostive = true;
            }
            characteristicId = $(parentObj).parent().attr("id");
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails input[id*="hfCharacteristicId"]').val(characteristicId);
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails input[id*="hfIsCharacteristicPositive"]').val(isCharacteristicPostive);


        }
        else if (currentUlId.toLowerCase() == "ulexamsubcharacteristics") {
            if (currentId != null && currentId.indexOf("+ve") > -1) {
                isSubCharacteristicPostive = true;
            }
            characteristicId = $(parentObj).parent().attr("parentid");
            subCharacteristicId = $(parentObj).parent().attr("id");

            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails input[id*="hfCharacteristicId"]').val(characteristicId);

            var chkOfCharacteristics = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li#' + characteristicId + '  input[type=checkbox]:checked').attr("id");
            if (chkOfCharacteristics != null && chkOfCharacteristics.indexOf("+ve") > -1) {
                isCharacteristicPostive = true;
            }
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails input[id*="hfSubCharacteristicId"]').val(subCharacteristicId);
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails input[id*="hfIsCharacteristicPositive"]').val(isCharacteristicPostive);
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails input[id*="hfIsSubCharacteristicPositive"]').val(isSubCharacteristicPostive);
        }
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will handle show/Hide of Details section
    showHideToggleDetails: function (isToShow) {
        var toggleDetailsDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails");
        if (isToShow == false) {
            if (toggleDetailsDiv.hasClass("hidden") == false) {
                toggleDetailsDiv.addClass("hidden");
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails").addClass('hidden');
            }
        }
        else {

            toggleDetailsDiv.removeClass("hidden");
        }

    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will handle show/Hide of section/Characteristics/Sub-Characteristics normal exam
    showHideSectionCharacteristics: function (isFromSystems) {
        if (isFromSystems == true) {
            var sectionDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SystemSections");

            //Start 09-02-2016 Humaira Yousaf to reset selected checkboxes
            var characteristicDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SectionCharacteristics");
            var subCharacteristicDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics");

            $(characteristicDiv).find("#ulExamCharacteristics > li").find('input:checkbox').prop('checked', false);
            $(subCharacteristicDiv).find("#ulExamSubCharacteristics> li").find('input:checkbox').prop('checked', false);
            //End 09-02-2016 Humaira Yousaf to reset selected checkboxes

            // sectionDiv.find("ul#ulPhysicalExamSystemSection li input[id='chkNormalSection']").trigger("click");
            if (sectionDiv.hasClass("hidden") == false) {
                sectionDiv.find("ul#ulPhysicalExamSystemSection li").not(":first").each(function (i, item) {
                    $(item).removeClass("active");
                    $(item).addClass("disableAll");
                });
                sectionDiv.addClass("hidden");
            }
        }
        var characteristicDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SectionCharacteristics");
        var subCharacteristicDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics");
        if (characteristicDiv.hasClass("hidden") == false) {
            characteristicDiv.addClass("hidden");
        }

        if (subCharacteristicDiv.hasClass("hidden") == false) {
            subCharacteristicDiv.addClass("hidden");
        }

        PhysicalExamDataTemplateDetail.showHideToggleDetails(true);

    },


    //Author Farooq Ahmad
    //Date : 15-06-2016
    AllSystemNormarl: function (chk) {
        if ($(chk).prop("checked")) {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li").each(function () {
                $(this).addClass("green");
                $(this).addClass("disableAll");
            });
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail").find("#SystemSections,#SectionCharacteristics,#CharacteristicsSubCharacteristics").addClass("hidden");
            PhysicalExamDataTemplateDetail.DataJSON = [];
        }
        else {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li").each(function () {
                $(this).removeClass("green");
                $(this).removeClass("disableAll");
            });
        }
    },




    //Author: Muhammad Arshad
    //Date: 25-01-2016
    //This function will handle mark systems as Normal and hide Section/Characteristics
    markSectionAsNormal: function (obj, isForSection, isFormLoad, physExamNormalComments) {
        var parentid = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first").attr("parentid");
        if ($(obj).prop("checked")) {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:not(:first)").addClass("disableAll");
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li#" + parentid).addClass("green");
        }
        else {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:not(:first)").removeClass("disableAll");
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li#" + parentid).removeClass("green");
        }
        PhysicalExamDataTemplateDetail.updateJSONArraySystem(parentid);
        ////Start 04-02-2016 Humaira Yousaf to get active system name
        //var systemName = $("#ulPhysicalExamSystems > li.active").find('a').text();
        ////End 04-02-2016 Humaira Yousaf to get active system name
        //var isToMarkNormal = $(obj).prop("checked");
        //var NormalExamsDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #divNormalExams");

        //if (isForSection != true) {
        //    if (isToMarkNormal == true) {
        //        //Start/Feb 9, 2016/Abid Ali to handle form onload event
        //        if (isFormLoad == true) {
        //            PhysicalExamDataTemplateDetail.SectionNormalInfo = [];
        //            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li").each(function (i, item) {
        //                $(item).removeClass("active");
        //                PhysicalExamDataTemplateDetail.SectionNormalInfo.push($(item).attr('id'));
        //                if ($(item).hasClass("green") == false) {
        //                    $(item).addClass("green");
        //                }
        //            });
        //            PhysicalExamDataTemplateDetail.showHideSectionCharacteristics(true);
        //            PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
        //            //Start 04-02-2016 Humaira Yousaf to remove hidden class
        //            if (NormalExamsDiv.hasClass("hidden") == true) {
        //                NormalExamsDiv.removeClass("hidden");
        //            }
        //            //End 04-02-2016 Humaira Yousaf to remove hidden class

        //            if (physExamNormalComments != null && physExamNormalComments != "") {
        //                //Start Farooq Ahmad 29/03/2016 EMR-469 Physical Exam in provider Notes -> Bluebook comment box should be closed after save the data
        //                //PhysicalExamDataTemplateDetail.openNormalExamDetail(this);
        //                //End Farooq Ahmad 29/03/2016 EMR-469 Physical Exam in provider Notes -> Bluebook comment box should be closed after save the data
        //                NormalExamsDiv.find('i').addClass('green');
        //            }
        //            else {
        //                NormalExamsDiv.find('i').removeClass('green');
        //            }
        //        }
        //            //End/Feb 9, 2016/Abid Ali to handle form onload event
        //        else {
        //            utility.myConfirm('24', function () {
        //                var selectedSystemCharIds = [];
        //                var selectedSystemIds = [];
        //                var systemLiIds = [];
        //                PhysicalExamDataTemplateDetail.SectionNormalInfo = [];
        //                var sectionLiIds = [];
        //                var systemsLength = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems").length;
        //                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li").each(function (i, item) {
        //                    $(item).removeClass("active");
        //                    PhysicalExamDataTemplateDetail.SectionNormalInfo.push($(item).attr('id'));
        //                    var physicalExamId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();
        //                    //Start 09-02-2016 Humaira Yousaf to handle CSS changes
        //                    if ($(item).hasClass("green") == false) {
        //                        $(item).addClass("green");
        //                    }
        //                    //End 09-02-2016 Humaira Yousaf to handle CSS changes

        //                    //Start 14-03-2016 Abid Ali to handle Delete systems and child data on makring Normal
        //                    var selectedSystemDataObject = $(item).data('SystemSectionIds_' + $(item).attr('id'));
        //                    if (!PhysicalExamDataTemplateDetail.isNullOrUndefined(selectedSystemDataObject)) {

        //                        // Loop through all data sections and delete data from Json and Dom
        //                        $.each(selectedSystemDataObject.Sections, function (sectionIndex, section) {

        //                            PhysicalExamDataTemplateDetail.deleteSectionDetail('System', selectedSystemDataObject.SystemId, physicalExamId, section.SectionId, 'markAsNormal');
        //                            PhysicalExamDataTemplateDetail.deleteFromJsonObject('ulPhysicalExamSystemSection', section.SectionId);

        //                            for (var char = 0; char < selectedSystemDataObject.Sections[sectionIndex].Characteristics.length; char++) {
        //                                PhysicalExamDataTemplateDetail.deleteFromJsonObject('ulExamCharacteristics', selectedSystemDataObject.Sections[sectionIndex].Characteristics[char].SectionCharacteristicId);

        //                                for (var subChar = 0; subChar < selectedSystemDataObject.Sections[sectionIndex].Characteristics[char].SubCharacteristics.length; subChar++) {
        //                                    PhysicalExamDataTemplateDetail.deleteFromJsonObject('ulExamSubCharacteristics', selectedSystemDataObject.Sections[sectionIndex].Characteristics[char].SubCharacteristics[subChar].CharSubCharacteristicId);
        //                                }
        //                            }

        //                        });
        //                        // Delete systems from Dom (system Li's objects)
        //                        if (selectedSystemDataObject.Sections.length > 0) {
        //                            var systemsArray = [];
        //                            systemsArray.push(selectedSystemDataObject.SystemId);
        //                            PhysicalExamDataTemplateDetail.deleteDomObjectFromList('#ulPhysicalExamSystems', systemsArray);
        //                        }
        //                    }
        //                });
        //                //End/14-03-2016 Abid Ali/ to handle Delete systems and child data on makring Normal

        //                PhysicalExamDataTemplateDetail.showHideSectionCharacteristics(true);
        //                PhysicalExamDataTemplateDetail.showHideToggleDetails(false);

        //                //Start 04-02-2016 Humaira Yousaf to remove hidden class
        //                if (NormalExamsDiv.hasClass("hidden") == true) {
        //                    NormalExamsDiv.removeClass("hidden");
        //                }
        //                //End 04-02-2016 Humaira Yousaf to remove hidden class
        //            }, function () {
        //                $(obj).prop("checked", false);
        //                if (NormalExamsDiv.hasClass("hidden") == false) {
        //                    NormalExamsDiv.addClass("hidden")
        //                }
        //                //Start 16-03-2016 Farooq Ahmad to add the popup name
        //            }, 'Confirm Reset');
        //            //End 16-03-2016 Farooq Ahmad to add the popup name
        //        }
        //    }
        //    else {
        //        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li").each(function (i, item) {
        //            $(item).removeClass("active");
        //            //Start 09-02-2016 Humaira Yousaf to handle CSS changes
        //            $(item).removeClass("green");
        //            //End 09-02-2016 Humaira Yousaf to handle CSS changes
        //        });
        //        //Start 04-02-2016 Humaira Yousaf to add hidden class
        //        if (NormalExamsDiv.hasClass("hidden") == false) {
        //            NormalExamsDiv.addClass("hidden")
        //        }
        //        //End 04-02-2016 Humaira Yousaf to add hidden class

        //        PhysicalExamDataTemplateDetail.SectionNormalInfo = [];

        //        //Start/29-3-2016/Abid Ali/ For Bug# EMR-444
        //        var $section = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection");
        //        $section.addClass('disabledAll').find('li').first().find('input').prop('checked', false);
        //        $section.find('li').first().find('#btnNormalSectionDetails').find('i').addClass('blue').removeClass('green');
        //        $section.find('li').first().find('#txtNormalSectionDetail').val('');
        //        //End/29-3-2016/Abid Ali/ For Bug# EMR-444

        //        //Start//29-03-2016//Abid Ali//For Bug# EMR-448
        //        PhysicalExamDataTemplateDetail.unCheckMainNormalCheckbox();
        //        //End//29-03-2016//Abid Ali//For Bug# EMR-448
        //    }
        //}
        //else if (isForSection == true) {

        //    var charUlId = "ulExamCharacteristics";
        //    var subCharUlId = "ulExamSubCharacteristics";


        //    //Start 14-04-2016// Abid Ali //Delete Char/SubCahr Data from Json
        //    if (PhysicalExamDataTemplateDetail.charIds.length > 0) {

        //        PhysicalExamDataTemplateDetail.deleteListItemsFromJson(charUlId, PhysicalExamDataTemplateDetail.charIds);

        //        //Delete SubChar Data from Json
        //        if (PhysicalExamDataTemplateDetail.subCharIds.length > 0) {

        //            PhysicalExamDataTemplateDetail.deleteListItemsFromJson(subCharUlId, PhysicalExamDataTemplateDetail.subCharIds);
        //        }
        //    }
        //    //End 14-04-2016// Abid Ali //Delete Char/SubCahr Data from Json

        //    if (isToMarkNormal == true) {

        //        var characteristicDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SectionCharacteristics");
        //        var subCharacteristicDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics");
        //        //Start 23-02-2016 Humaira Yousaf to delete systems data
        //        var systemId = $("#ulPhysicalExamSystems > li.active").attr('id');

        //        if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems li#" + systemId).data("SystemSectionIds_" + systemId) != null &&
        //             $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems li#" + systemId).data("SystemSectionIds_" + systemId).Sections.length > 0) {
        //            //End 23-02-2016 Humaira Yousaf to delete systems data
        //            var msg = 'This will mark the entire ' + systemName + ' as Normal and reset all values in all sections of ' + systemName + '. Would you like to proceed?';

        //            utility.myConfirm(msg, function () {

        //                //Start 23-02-2016 Humaira Yousaf to delete systems data
        //                var physicalExamId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();
        //                var systemSections = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems li#" + systemId).data("SystemSectionIds_" + systemId);
        //                var sectionLiIds = [];
        //                var systemLiIds = [];
        //                systemLiIds.push(systemId);
        //                var deffered = $.Deferred();

        //                //Start 14-03-2016 Abid Ali to handle Delete systems and child data on makring Normal
        //                //Delete All sections and nested data
        //                $.each(systemSections.Sections, function (sectionIndex, section) {
        //                    PhysicalExamDataTemplateDetail.deleteSectionDetail('System', systemId, physicalExamId, section.SectionId, 'markAsNormal');
        //                    PhysicalExamDataTemplateDetail.deleteFromJsonObject('ulPhysicalExamSystemSection', section.SectionId);
        //                    sectionLiIds.push(section.SectionId);
        //                    for (var char = 0; char < systemSections.Sections[sectionIndex].Characteristics.length; char++) {
        //                        PhysicalExamDataTemplateDetail.deleteFromJsonObject('ulExamCharacteristics', systemSections.Sections[sectionIndex].Characteristics[char].SectionCharacteristicId);

        //                        for (var subChar = 0; subChar < systemSections.Sections[sectionIndex].Characteristics[char].SubCharacteristics.length; subChar++) {
        //                            PhysicalExamDataTemplateDetail.deleteFromJsonObject('ulExamSubCharacteristics', systemSections.Sections[sectionIndex].Characteristics[char].SubCharacteristics[subChar].CharSubCharacteristicId);
        //                        }
        //                    }

        //                    if (sectionIndex == systemSections.Sections.length - 1)
        //                        return deffered.resolve();
        //                });

        //                deffered.done(function () {

        //                    PhysicalExamDataTemplateDetail.deleteFromJsonObject('ulPhysicalExamSystems', systemId);
        //                    PhysicalExamDataTemplateDetail.deleteDomObjectFromList('#ulPhysicalExamSystems', systemLiIds);

        //                });
        //                //End 14-03-2016 Abid Ali to handle Delete systems and child data on makring Normal

        //                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li").not(":first").each(function (i, item) {
        //                    $(item).removeClass("active").removeClass('green');

        //                    if ($(item).hasClass("disableAll") == false) {
        //                        $(item).addClass("disableAll");
        //                    }

        //                    PhysicalExamDataTemplateDetail.showHideSectionCharacteristics(false);
        //                    PhysicalExamDataTemplateDetail.showHideToggleDetails(false);

        //                    $(subCharacteristicDiv).find('#ulExamSubCharacteristics > li').removeClass('active')
        //                    $(subCharacteristicDiv).find('#ulExamSubCharacteristics > li').find('input:checkbox').prop('checked', false);

        //                    var SystemsUL = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems');
        //                    $(PhysicalExamDataTemplateDetail.SelectedSystem).addClass('green');
        //                    //if ($(SystemsUL).find('li').length == $(SystemsUL).find('li.green').length) {
        //                    //    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail input[id='chkPhysicalExamsNormal']").prop("checked", true);
        //                    //}

        //                });

        //            }, function () {
        //                $(obj).prop("checked", false);
        //                //Start 16-03-2016 Farooq Ahmad to add the popup name
        //            }, 'Confirm Reset');
        //            //End 16-03-2016 Farooq Ahmad to add the popup name
        //        }
        //        else {

        //            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li").not(":first").each(function (i, item) {
        //                $(item).removeClass("active").removeClass('green');
        //                if ($(item).hasClass("disableAll") == false) {
        //                    $(item).addClass("disableAll");
        //                }

        //                PhysicalExamDataTemplateDetail.showHideSectionCharacteristics(false);
        //                PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
        //            });
        //            var SystemsUL = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems');
        //            $(PhysicalExamDataTemplateDetail.SelectedSystem).addClass('green');
        //            if ($(SystemsUL).find('li').length == $(SystemsUL).find('li.green').length) {
        //                // $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail input[id='chkPhysicalExamsNormal']").prop("checked", true);
        //                // NormalExamsDiv.removeClass("hidden")
        //            }

        //        }
        //        //Start 09-02-2016 Humaira Yousaf to chace normal systems
        //        PhysicalExamDataTemplateDetail.cacheNormalSystem();
        //        //End 09-02-2016 Humaira Yousaf to chace normal systems
        //        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first").find('#btnNormalSectionDetails').removeClass('hidden');
        //    }
        //    else {

        //        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li").not(":first").each(function (i, item) {
        //            $(item).removeClass("disableAll").removeClass('green');

        //        });
        //        $(PhysicalExamDataTemplateDetail.SelectedSystem).removeClass("green");

        //        var SystemsUL = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems');
        //        if ($(SystemsUL).find('li').length > $(SystemsUL).find('li.green').length) {

        //            $("#chkPhysicalExamsNormal").prop('checked', false);
        //            var NormalExamsDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #divNormalExams");
        //            if (NormalExamsDiv.hasClass("hidden") == false) {
        //                NormalExamsDiv.addClass("hidden");
        //            }
        //        }
        //        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first").find('#btnNormalSectionDetails').addClass('hidden');
        //        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first").find('#textAreaNormal').addClass('hidden');
        //        //Start 09-02-2016 Humaira Yousaf to remove normal systems
        //        PhysicalExamDataTemplateDetail.removeNormalSystem();
        //        //End 09-02-2016 Humaira Yousaf to remove normal systems
        //    }
        //}

        PhysicalExamDataTemplateDetail.toggleVerticalWidth();
    },

    //Remove From Json Object
    deleteListItemsFromJson: function (ul, liIds) {

        $.each(liIds, function (index, id) {

            PhysicalExamDataTemplateDetail.deleteFromJsonObject(ul, id);

        });
    },

    //Author: Abid Ali
    //Date: 26-02-2016
    //This function will delete Data objects from DOM using list (ids) passed and resets the item color to green
    deleteDomObjectFromList: function (listType, ListIds) {
        if (listType != null && typeof listType != 'undefined') {
            var LiIds = [];
            if (jQuery.type(ListIds) === "string") {
                LiIds.push(ListIds);
            }
            else {
                LiIds = ListIds;
            }

            var divPhysicaExam = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail ";
            if (listType.toLowerCase() == '#ulPhysicalExamSystems'.toLowerCase()) {
                // remove data from selected ids
                $.each(LiIds, function (index, item) {
                    $(divPhysicaExam + listType + " li#" + item).removeData("SystemSectionIds_" + item);//.removeClass('green active');
                });
                // remove all data from section
                //$(divPhysicaExam + "#ulPhysicalExamSystemSection li").each(function () {

                //})
            }
            else if (listType.toLowerCase() == '#ulPhysicalExamSystemSection'.toLowerCase()) {

                $.each(LiIds, function (index, item) {
                    $(divPhysicaExam + listType + " li#" + item).removeData("SystemCharacteristicsIds_" + item);
                    $(divPhysicaExam + listType + " li#" + item).removeData('SystemCharacteristicPk_' + item);
                });
            }
            else if (listType.toLowerCase() == '#ulExamCharacteristics'.toLowerCase()) {

                $.each(LiIds, function (index, item) {
                    $(divPhysicaExam + listType + " li#" + item).removeData("SystemSubCharacteristicsIds_" + item)
                    $(divPhysicaExam + listType + " li#" + item).removeData('SystemCharacteristicDetails_' + item)//.removeClass('green active');
                });
            }
            else if (listType.toLowerCase() == '#ulExamSubCharacteristics'.toLowerCase()) {

                $.each(LiIds, function (index, item) {
                    $(divPhysicaExam + listType + " li#" + item).removeData("SystemSubCharacteristicDetails_" + item).removeData('SystemSubCharacteristicPk_' + item)//.removeClass('green active');
                });
            }
        }
    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will handle load of child of either Systems/Section/Characteristics
    loadChildData: function (parentId, parentType) {
        var objDeffered = $.Deferred();
        PhysicalExamDataTemplateDetail.loadPhysicalExamStatuses(parentId, parentType).done(function () {

            objDeffered.resolve();

        });
        return objDeffered;
    },

    //Author: Farooq Ahmad
    //Date: 07-03-2016
    //This function will handle normal comments of the sections
    saveNormalSectionDetail: function (event, val) {
        if (event.which == 13) {
            event.preventDefault();
            var onClickFunction = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail   #btnSaveNormalSystemDetail" + val).attr("onclick")

            eval(onClickFunction);

            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection #textAreaNormal").addClass("hidden");
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:first").addClass("green");
        }

    },

    //Author: Muhammad Arshad
    //Date: 12-09-2015
    //This function will handle fill of PhysicalExam Statuses like SmokingStatus,AlcoholStatus,DrugAbuseStatus,SexualHxStatus
    loadPhysicalExamStatuses: function (parentId, parentType, templateId) {
        var templateId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfTemplateId').val();
        var dataTemplateId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfDataTemplateId').val();
        var currentLiClass = "";
        var currentLiClick = "";
        var currentCtrlId = "";
        var ParentDiv = "";
        var data = "";

        var selectedData = PhysicalExamDataTemplateDetail.getObjectOfClickedElement(parentType, parentId);

        if (parentType != null && parentType.toLowerCase() == "mainpesystem") {
            Crtl = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems";
            currentLiClick = "PhysicalExamDataTemplateDetail.showHideChildControls";
            ParentDiv = "divPhysicalExamSystems";
            methodName = "GetPhysicalExamDataTemplateSystem";
            currentCtrlId = "ulPhysicalExamSystems";
            data = "ID=" + templateId + "&ID2=" + dataTemplateId;
            // PhysicalExamDataTemplateDetail.DataJSON = [];

        }
        else if (parentType != null && parentType.toLowerCase() == "system") {
            Crtl = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection";
            currentLiClick = "PhysicalExamDataTemplateDetail.showHideChildControls";
            ParentDiv = "divPhysicalExamSystemSection";
            methodName = "GetPhysicalExamDataTemplateSectionBySystemId";
            currentCtrlId = "ulPhysicalExamSystemSection";
            data = "ID=" + parentId + "&ID2=" + templateId + "&ID3=" + dataTemplateId;
            //   PhysicalExamDataTemplateDetail.isNormalTriggred = selectedData != null ? selectedData.IsNormal : false;
        }
        else if (parentType != null && parentType.toLowerCase() == "section") {
            Crtl = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics";
            currentLiClick = "PhysicalExamDataTemplateDetail.showHideChildControls";
            ParentDiv = "divExamCharacteristics";
            methodName = "GetPhysicalExamDataTemplateCharBySectionId";
            currentCtrlId = "ulExamCharacteristics";
            data = "ID=" + parentId + "&ID2=" + templateId + "&ID3=" + dataTemplateId;
        }
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {
            Crtl = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulExamSubCharacteristics";
            currentLiClick = "PhysicalExamDataTemplateDetail.showHideChildControls";
            ParentDiv = "divExamSubCharacteristics";
            //Start 11-02-2016 Humaira Yousaf to get subCharacteristics
            methodName = "GetPhysicalExamDataTemplateSubCharByCharId";
            currentCtrlId = "ulExamSubCharacteristics";
            data = "ID=" + parentId + "&ID2=" + templateId + "&ID3=" + dataTemplateId;
            //End 11-02-2016 Humaira Yousaf to get subCharacteristics
        }
        else if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {
            //  Crtl = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #CharacteristicsDetails";
            currentLiClick = "PhysicalExamDataTemplateDetail.showHideChildControls";
            ParentDiv = "amSubCharacteristics";
            methodName = "GetSocialHxCounsellingPeriod";
            currentCtrlId = "";
        }
        else {
            data = "ID=" + parentId;
        }

        return MDVisionService.lookups(methodName, true, data).done(function (result) {
            result = JSON.parse(result[methodName]);
            if ($(Crtl).length > 0)
                l = $(Crtl);
            if (parentType != null && parentType.toLowerCase() == "subcharacteristics") {
                return;
            }

            l.empty();

            var item = {};

            var isFirstLi = true;
            var onClick = "";//currentLiClick + "('" + currentCtrlId + "','" + String(item.Value) + "');";
            //item.Value = item.Value == "" ? 0 : item.Value;
            if (parentType.toLowerCase() == "section" || parentType.toLowerCase() == "characteristics" || parentType.toLowerCase() == "subcharacteristics") {
                //Start 11-02-2016 Humaira Yousaf to show select all checkboxes only if there is data
                if (result.length > 0) {
                    //End 11-02-2016 Humaira Yousaf to show select all checkboxes only if there is data
                    var liInnerText = "";
                    liInnerText = '<div><input type="checkbox" id="chkSelectAll+ve" name="SelectAll+ve" class="ml-xlg pull-left" onclick="PhysicalExamDataTemplateDetail.selectAllCharacteristics(this,true);"><input type="checkbox" id="chkSelectAll-ve" name="SelectAll-ve" class="ml-sm pull-left" onclick="PhysicalExamDataTemplateDetail.selectAllCharacteristics(this,false);"><label class="control-label pull-left pl-xs">Select All</label><div class="clearfix"></div></div>';
                    l.append('<li id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '">' + liInnerText + ' </li>');
                }
            }
            else if (parentType.toLowerCase() == "system") {
                var normalDetail = '<a id="btnNormalSectionDetails" onclick="PhysicalExamDataTemplateDetail.openNormalSectionDetail(this);" class="btnEffect pull-left ml-md hidden"><i class="fa fa-book blue"></i></a><div id="textAreaNormal" class="rightInnerAddon pb-xs hidden"><textarea rows="1" spellcheck="true" id="txtNormalSectionDetail" class="form-control pr-xlg height-max105 size100per textAreaScroll" onkeypress="PhysicalExamDataTemplateDetail.saveNormalSectionDetail(event,' + item.Value + ')"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveNormalSystemDetail' + item.Value + '" onclick="PhysicalExamDataTemplateDetail.saveExamSystemNormalDetail(this,\'textNormalSectionDetailNormal' + '\',\'' + parentId + '\');" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div></div>';

                liInnerText = '<div><input type="checkbox" id="chkNormalSection" name="NormalSection" class="pull-left" onclick="PhysicalExamDataTemplateDetail.markSectionAsNormal(this,true);"><label class="control-label pull-left pl-xs">Normal</label>' + normalDetail + '<div class="clearfix"></div></div>';
                //Start 09-02-2016 Humaira Yousaf for section normal details
                l.append('<li id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '">' + liInnerText + ' </li>');
                //End 09-02-2016 Humaira Yousaf for section normal details
            }

            $.each(result, function (j, item) {
                if (item.Value != "") {



                    if (isFirstLi == true) {
                        //currentLiClass = 'class="active"';
                        isFirstLi = false;
                    }
                    else {
                        currentLiClass = "";
                    }
                    var physicalExamId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();
                    var onClick = currentLiClick + "('" + currentCtrlId + "','" + String(item.Value) + "');";

                    var greenclass = item.RefValue == "1" ? "green" : "";

                    var deleteClick = "";

                    //Parent Type and its childs
                    var liInnerText = '<a href="#' + ParentDiv + '">' + item.Name + '</a>';

                    if (parentType.toLowerCase() == "section" || parentType.toLowerCase() == "characteristics") {

                        var isSubCharacteristic = false;

                        if (parentType.toLowerCase() == "characteristics") {
                            isSubCharacteristic = true;

                            //onClick += " PhysicalExamDataTemplateDetail.BindDetailsOfCharAndSubChar('" + item.Value + "','ulexamsubcharacteristics');";
                        }
                        else {
                            // onClick += " PhysicalExamDataTemplateDetail.BindDetailsOfCharAndSubChar('" + item.Value + "','ulexamcharacteristics');";
                        }


                        //Start 12-02-2016 Humaira Yousaf to open characteristic detail
                        if (item.RefName != "") {
                            liInnerText = '<div><button type="button" id="btnOpenDetail' + item.Value + '"  data-toggle="tooltip" data-placement="top"  onclick="PhysicalExamDataTemplateDetail.openCharacteristicDetail(this,\'' + item.Value + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-book"></i></button><input type="checkbox" id="chk' + item.Value + '+ve" name="' + item.Value + '" class="ml-sm pull-left  char" onclick="PhysicalExamDataTemplateDetail.toggleCheckBoxes(this);"><input type="checkbox" id="chk' + item.Value + '-ve" name="' + item.Value + '" class="ml-sm pull-left char" onclick="PhysicalExamDataTemplateDetail.toggleCheckBoxes(this);"><label class="control-label pull-left ml-xs size65per ellipses" data-toggle="tooltip"  title="" data-original-title="' + item.Name + '">' + item.Name + '</label><span id="btnShowSubCharacteristics' + item.Value + '" onclick="" class="pull-right" disabled="disabled"><i class="fa fa-caret-right blue"></i></span><div class="clearfix"></div><div id="divDetail' + item.Value + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" spellcheck="true" maxlength="250" id="txtCharacteristicDetail' + item.Value + '" name="CharacteristicDetail' + item.Value + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox" onblur="PhysicalExamDataTemplateDetail.validateMaxLength(this)" onkeypress="PhysicalExamDataTemplateDetail.saveDetailComments(event,this)">></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.Value + '" onclick="PhysicalExamDataTemplateDetail.saveCharacteristicDetail(event,this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div></div>';
                        }
                        else {
                            liInnerText = '<div><button type="button" id="btnOpenDetail' + item.Value + '"  data-toggle="tooltip" data-placement="top"  onclick="PhysicalExamDataTemplateDetail.openCharacteristicDetail(this,\'' + item.Value + '\');" class="btn btn-link btn-xs pull-left"><i class="fa fa-book"></i></button><input type="checkbox" id="chk' + item.Value + '+ve" name="' + item.Value + '" class="ml-sm pull-left  char" onclick="PhysicalExamDataTemplateDetail.toggleCheckBoxes(this);"><input type="checkbox" id="chk' + item.Value + '-ve" name="' + item.Value + '" class="ml-sm pull-left char" onclick="PhysicalExamDataTemplateDetail.toggleCheckBoxes(this);"><label class="control-label pull-left ml-xs size65per ellipses" data-toggle="tooltip"  title="" data-original-title="' + item.Name + '">' + item.Name + '</label><div class="clearfix"></div><div id="divDetail' + item.Value + '" class="rightInnerAddon pb-xs hidden"><textarea rows="1" maxlength="250" spellcheck="true" id="txtCharacteristicDetail' + item.Value + '" name="CharacteristicDetail' + item.Value + '" type="text" class="form-control pr-xlg height-max105 size100per textAreaScroll commentbox " onblur="PhysicalExamDataTemplateDetail.validateMaxLength(this)" onkeypress="PhysicalExamDataTemplateDetail.saveDetailComments(event,this)"></textarea><div class="clearfix"></div><div class="rightInnerAddonBtn"><a id="btnSaveDetail' + item.Value + '" onclick="PhysicalExamDataTemplateDetail.saveCharacteristicDetail(event,this);" class="btn btn-link btn-xs"><i class="fa fa-save"></i></a></div></div><div class="clearfix"></div></div>';
                        }
                        //End 12-02-2016 Humaira Yousaf  to open characteristic detail
                    }
                    var mouseHoverEvent = "";
                    if (parentType != null && parentType.toLowerCase() == "mainpesystem") {

                        mouseHoverEvent = 'onmouseover="PhysicalExamDataTemplateDetail.showDeleteIcon(this)" onmouseout="PhysicalExamDataTemplateDetail.hideDeleteIcon(this)"';
                        liInnerText = '<a href="#' + ParentDiv + '">' + item.Name + '<span style="dispaly:none;" class="removeIconListHover" onclick="PhysicalExamDataTemplateDetail.physicalExamDataTemplateDelete(' + item.Value + ')"><i class="fa fa-times"></i></span></a>';
                        PhysicalExamDataTemplateDetail.addGreenClasses();
                    }

                    l.append('<li class="' + greenclass + '"' + mouseHoverEvent + '" id="' + item.Value + '" ' + currentLiClass + ' parentid="' + parentId + '" onclick="' + onClick + '" value="' + item.Value + '" refValue="' + item.RefValue + '" subCharacteristicExist="' + item.RefName + ' ">' + liInnerText + '</li>');

                    //setTimeout(function () {

                    //    //Load systems
                    //    if (parentType != null && parentType.toLowerCase() == "mainpesystem") {

                    //        //if (item.RefValue == "1") {
                    //        //    PhysicalExamDataTemplateDetail.manageJsonData(item.Value, 'system', "", item.RefName);

                    //        //}

                    //    }
                    //        //Load sections
                    //    else if (parentType.toLowerCase() == "system") {

                    //        ////Push Json object in client side JSON data
                    //        //var currentSystem = PhysicalExamDataTemplateDetail.getSystemFromJson(parentId);
                    //        //if (currentSystem != null && currentSystem.IsNormal == true) {
                    //        //    var $sectionUl = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #ulPhysicalExamSystemSection");
                    //        //    $sectionUl.find('li').first().find('input[type="checkbox"]').trigger('click');
                    //        //}
                    //        //if (item.RefValue == "1") {
                    //        //    PhysicalExamDataTemplateDetail.updateJSONArraySection(item.Value);
                    //        //}
                    //    }
                    //        //Load chars
                    //    else if (parentType.toLowerCase() == "section") {

                    //        //if (item.RefValue == "1") {

                    //        //    if (item.IsActive.indexOf(",") > -1) {
                    //        //        var posNegArray = item.IsActive.split(',');
                    //        //        var IsPositive = posNegArray[0];
                    //        //        var IsNegative = posNegArray[1];
                    //        //        if (IsPositive == "True") {
                    //        //            l.find("input[type='checkbox'][id*='" + item.Value + "+ve']").prop("checked", true);

                    //        //        }
                    //        //        else if (IsNegative == "True") {
                    //        //            l.find("input[type='checkbox'][id*='" + item.Value + "-ve']").prop("checked", true);
                    //        //        }
                    //        //        //For Detail section
                    //        //        if (item.ExValue != "") {

                    //        //            var details = JSON.parse(item.ExValue)[0];
                    //        //            //l.find("li#" + item.Value).data(PhysicalExamDataTemplateDetail.CharDetails, details);
                    //        //            // var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");
                    //        //            PhysicalExamDataTemplateDetail.updateJSONArrayChar(item.Value, JSON.stringify(details));
                    //        //            // utility.bindMyJSONByName(true, details, false, detailSection);
                    //        //        }
                    //        //        else {
                    //        //            PhysicalExamDataTemplateDetail.updateJSONArrayChar(item.Value, null);
                    //        //        }
                    //        //        PhysicalExamDataTemplateDetail.updateTheCommentsOfCharAndSubChar(item.RefName, item.Value, "ulexamcharacteristics");

                    //        //        //PhysicalExamDataTemplateDetail.addGreenClasses();
                    //        //    }
                    //        //}
                    //        //PhysicalExamDataTemplateDetail.addGreenClasses();
                    //    }
                    //        //Load SubChars
                    //    else if (parentType.toLowerCase() == "characteristics") {

                    //        //if (item.RefValue == "1") {

                    //        //    if (item.IsActive.indexOf(",") > -1) {
                    //        //        var posNegArray = item.IsActive.split(',');
                    //        //        var IsPositive = posNegArray[0];
                    //        //        var IsNegative = posNegArray[1];
                    //        //        if (IsPositive == "True") {
                    //        //            l.find("input[type='checkbox'][id*='" + item.Value + "+ve']").prop("checked", true);

                    //        //        }
                    //        //        else if (IsNegative == "True") {
                    //        //            l.find("input[type='checkbox'][id*='" + item.Value + "-ve']").prop("checked", true);

                    //        //        }
                    //        //        // PhysicalExamDataTemplateDetail.manageJsonData(item.Value, 'subcharacteristics', "");
                    //        //        PhysicalExamDataTemplateDetail.updateTheCommentsOfCharAndSubChar(item.RefName, item.Value, "ulexamsubcharacteristics");
                    //        //        PhysicalExamDataTemplateDetail.addGreenClasses();
                    //        //    }
                    //        //    //For Detail section
                    //        //    if (item.ExValue != "") {
                    //        //        var details = JSON.parse(item.ExValue)[0];
                    //        //        //l.find("li#" + item.Value).data(PhysicalExamDataTemplateDetail.SubCharDetails, details);
                    //        //        // var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");

                    //        //        PhysicalExamDataTemplateDetail.updateJSONArrayForSubChar(item.Value, JSON.stringify(details));
                    //        //        //   utility.bindMyJSONByName(true, details, false, detailSection);
                    //        //    }
                    //        //}
                    //        // PhysicalExamDataTemplateDetail.addGreenClasses();
                    //    }
                    //}, 100);
                }
            });

            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

            $('.textAreaScroll').slimScroll({
                position: 'right',
                height: '100%',
            });
        });
    },

    //Fill form by type
    loadDetails: function (pkId, list) {

        var ulType = list == "Char" ? "ulExamCharacteristics" : "ulExamSubCharacteristics";
        var $ctrl = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #" + ulType);
        var details = null;
        if (list == "Char") {
            details = $ctrl.find("li#" + pkId).data(PhysicalExamDataTemplateDetail.CharDetails);
        }
        else {
            details = $ctrl.find("li#" + pkId).data(PhysicalExamDataTemplateDetail.SubCharDetails);
        }
        if (details) {
            var $detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");
            utility.bindMyJSONByName(true, details, false, $detailSection);
        }
    },

    FillCommentsFromJSON: function (pkId, type) {

        if (type.toLowerCase() == "ulexamsubcharacteristics") {

            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                    for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                        for (var subcharIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                            if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId == pkId) {
                                return PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].Comments;
                            }
                        }
                    }
                }
            }
        }
        else if (type.toLowerCase() == "ulexamcharacteristics") {

            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                    for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == pkId) {
                            return PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].Comments;
                        }
                    }
                }
            }
        }
        return "";
    },

    //Author : Farooq Ahmad
    //Date : 15/06/2016
    //This Function Will Bind the detail of Char And Subchar
    BindDetailsOfCharAndSubChar: function (pkId, type) {
        if (type == "ulexamcharacteristics") {
            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                    for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                        if (pkId == PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId) {
                            var JSONDetail = PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicDetailModel;
                            if (JSONDetail != null && JSONDetail.CharacteristicId != null) {
                                var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails");
                                utility.bindMyJSONByName(true, JSONDetail, false, detailSection);
                            }
                        }
                    }
                }
            }
        }
        else if (type == "ulexamsubcharacteristics") {
            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {
                    for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {
                        for (var subcharIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                            if (pkId == PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId) {
                                var JSONDetail = PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicDetailModel;
                                if (JSONDetail != null && JSONDetail.SubCharacteristicId != null) {
                                    var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails");
                                    utility.bindMyJSONByName(true, JSONDetail, false, detailSection);
                                }
                            }
                        }

                    }
                }
            }
        }
    },
    //Author : Ahmad Raza
    //Date :   09/03/2016
    //Reason:  validating maxlength on blur
    validateMaxLength: function (obj) {
        var maxLength = 250;
        if (obj.value.length > maxLength) {

            obj.value = obj.value.substring(0, 250);
        }
        if (obj.value.length > 0) {
            $(obj).closest("li").find('button[id*="btnOpenDetail"]').addClass("green");
        }
        else {
            $(obj).closest("li").find('button[id*="btnOpenDetail"]').removeClass("green");
        }
        $(obj).closest("li").find('button[id*="divDetail"]').addClass("hidden");

    },

    //Author :Farooq Ahmad
    //Date : 04/03/2016
    //This will save the comments of the characteristiscs and sub characteristices
    saveDetailComments: function (event, txt) {
        if (event.which == 13) {
            event.preventDefault();

            $(txt).closest("li").find('button[id*="btnOpenDetail"]').trigger('click');

            //var onClickFunction = $(txt).parent().parent().find('.btn-link').attr("onclick");
            //eval(onClickFunction);
        } else {
            var comments = $(txt).val();
            var pkid = $(txt).closest('li').attr('id');
            var type = $(txt).closest('ul').attr('id');
            PhysicalExamDataTemplateDetail.updateTheCommentsOfCharAndSubChar(comments, pkid, type);
        }
    },


    resetPhysicalExamDataTemplate: function () {

        utility.myConfirm('This will reset all values in all Systems and clear all information. This action is irreversible and cannot be undone. Would you like to proceed? ', function () {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {

                PhysicalExamDataTemplateDetail.resetControlValue(this);
            });
            //utility.CreateDatePicker(PhysicalExamDataTemplateDetail.params.PanelID + '  #dtPhysicalExamDate', function () {
            //}, true);
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID).find('ul li').removeClass('green');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID).find('ul li').removeClass('active');


            $('#' + PhysicalExamDataTemplateDetail.params.PanelID).find('section#SystemSections,section#SectionCharacteristics,section#SectionCharacteristics,section#CharacteristicsSubCharacteristics,section#sectionPhysicalExamDetails').addClass('hidden');

            PhysicalExamDataTemplateDetail.myArr = [];
            PhysicalExamDataTemplateDetail.ExamDetails = {};
            PhysicalExamDataTemplateDetail.SectionNormalInfo = [];

            PhysicalExamDataTemplateDetail.selectedcharacteristicsIds = [];
            PhysicalExamDataTemplateDetail.characteristicsWithData = [];
            PhysicalExamDataTemplateDetail.selectedsubcharacteristicsIds = [];
            PhysicalExamDataTemplateDetail.subcharacteristicsWithData = [];
            PhysicalExamDataTemplateDetail.SectionNormalInfo = [];
            PhysicalExamDataTemplateDetail.DataJSON = [];
            PhysicalExamDataTemplateDetail.DBDataJSON = [];
            PhysicalExamDataTemplateDetail.addGreenClasses();

        }, function () {

        }, "Confirm Reset");



    },

    //Author: Farooq Ahmad
    ///Date : 14/06/2016
    //This function will update The Json With Comments
    updateTheCommentsOfCharAndSubChar: function (comments, pkid, type) {
        if (type.toLowerCase() == "ulexamsubcharacteristics") {

            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {

                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {

                    for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {

                        for (var subcharIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics) {
                            if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].SubCharacteristicId == pkid) {

                                PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SubCharacteristics[subcharIndex].Comments = comments
                            }
                        }
                    }
                }
            }
        }
        else if (type.toLowerCase() == "ulexamcharacteristics") {

            for (var index in PhysicalExamDataTemplateDetail.DataJSON) {

                for (var secIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections) {

                    for (var charIndex in PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics) {

                        if (PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].SectionCharacteristicId == pkid) {

                            PhysicalExamDataTemplateDetail.DataJSON[index].Sections[secIndex].Characteristics[charIndex].Comments = comments
                        }
                    }
                }
            }
        }
    },

    //Author :  Abid Ali
    //Date : 19-2-2016
    //Description : Load data from database and mark the selected li's as green
    markActiveLis: function (result, selectedData, parentType, l) {

        //var systemsLength = $()
        $.each(result, function (j, item) {
            if (item.Value != "") {

                if (parentType != null && parentType.toLowerCase() == "system") {

                    if (selectedData != null) {
                        var firstLi = l.find('li').first();
                        if (selectedData.IsNormal) {

                            //if (!($('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail input[id='chkPhysicalExamsNormal']").prop("checked") == true))
                            // firstLi.find('input[type="checkbox"]').prop('checked', true);

                            if (selectedData.Comments != "") {
                                //green the book icon and pleace comments in textarea field
                                //   firstLi.find('a#btnNormalSectionDetails > i').removeClass('blue').addClass('green');
                                firstLi.find('textarea').text(selectedData.Comments);
                            }
                            //mark disable each section
                            l.find('li#' + item.Value).addClass('disableAll');
                        }
                        else {

                            if (typeof selectedData.Sections != 'undefined' && selectedData.Sections.length > 0) {

                                var sections = selectedData.Sections;
                                $.each(sections, function (i, SectionComponent) {
                                    if (SectionComponent.SectionId == item.Value) {
                                        var currentSectionLi = l.find('li#' + item.Value);
                                        currentSectionLi.data('SystemCharacteristicsIds_' + item.Value, SectionComponent);
                                        currentSectionLi.data('SystemCharacteristicPk_' + item.Value, SectionComponent.Id);
                                        if (currentSectionLi.hasClass("green") == false) {
                                            currentSectionLi.addClass("green");
                                        }
                                        if (currentSectionLi.hasClass("active") == false) {
                                            currentSectionLi.addClass("active");
                                        }
                                        return;
                                    }
                                });
                            }
                            else {
                                firstLi.find('input[type="checkbox"]').prop('checked', false);
                            }
                        }
                    }
                }
                    // color and save data in characteristics lis
                else if (parentType != null && parentType.toLowerCase() == "section") {
                    var isCharactristicsExists = false;
                    if (selectedData != null) {
                        var characteristics = selectedData.Characteristics;
                        $.each(characteristics, function (i, charactristicsComponent) {
                            if (charactristicsComponent.SectionCharacteristicId == item.Value) {

                                isCharactristicsExists = true;
                                // PhysicalExamDataTemplateDetail.showHideToggleDetails(true);
                                l.find('li#' + item.Value).data('SystemSubCharacteristicsIds_' + item.Value, charactristicsComponent).addClass("green");
                                if (charactristicsComponent.IsDetailExists)
                                    l.find('li#' + item.Value).data('SystemCharacteristicDetails_' + item.Value, charactristicsComponent.SectionCharacteristicDetailModel);

                                if (charactristicsComponent.IsPositive) {
                                    var firstLi = l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '+ve"]');

                                    if (l.find('li#' + item.Value).attr('subcharacteristicexist').trim() != "") {

                                        firstLi.trigger('click');

                                    } else {
                                        firstLi.prop('checked', true);
                                    }

                                }
                                else {

                                    if (l.find('li#' + item.Value).attr('subcharacteristicexist').trim() != "") {
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '-ve"]').trigger('click');
                                    } else {
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '-ve"]').prop('checked', true);
                                    }
                                }
                                if (charactristicsComponent.Comments != "") {
                                    //  l.find('li#' + item.Value).find('button').first().addClass('green');
                                    l.find('li#' + item.Value).find('button').attr("data-original-title", charactristicsComponent.Comments);
                                    l.find('li#' + item.Value).find('button').attr("title", charactristicsComponent.Comments);
                                    l.find('li#' + item.Value).find('textarea').text(charactristicsComponent.Comments);
                                }
                                else {
                                    l.find('li#' + item.Value).find('button').first().removeClass('green');
                                    l.find('li#' + item.Value).find('textarea').text('');
                                }
                                // Registers on click with every element in li
                                // if (charactristicsComponent.IsDetailExists) {

                                l.find('li#' + item.Value).on('click', function () {
                                    var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");
                                    var dataDetail = {};
                                    liId = $(this).attr('id');
                                    var textForHeading = $(this).find('label').text() + ' Details';
                                    var currentObjectData = $(this).data('SystemCharacteristicDetails_' + item.Value);
                                    if (!PhysicalExamDataTemplateDetail.isNullOrUndefined(currentObjectData)) {
                                        $.each(currentObjectData, function (key, value) {
                                            dataDetail[key + liId] = value;
                                        });

                                        PhysicalExamDataTemplateDetail.showHideToggleDetails(true);
                                        $.when(PhysicalExamDataTemplateDetail.toggleDetailsDiv(liId)).then(function () {
                                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #detailHeading").text(textForHeading);
                                            utility.bindMyJSONByName(true, dataDetail, false, detailSection);
                                        });
                                    }
                                    else {
                                        var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");
                                        var selectedText = $(this).find('label').text() + ' Details';
                                        liId = $(this).attr('id');
                                        var currentObjectData = PhysicalExamDataTemplateDetail.ExamDetails[liId];
                                        currentObjectData = (typeof currentObjectData != 'undefined' && currentObjectData != null) ? currentObjectData : "{}";
                                        if ($(this).find("input:checkbox:checked").length > 0) {
                                            PhysicalExamDataTemplateDetail.showHideToggleDetails(true);
                                            $.when(PhysicalExamDataTemplateDetail.toggleDetailsDiv(liId)).then(function () {
                                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                                utility.bindMyJSONByName(true, JSON.parse(currentObjectData), false, detailSection);
                                            });
                                        }
                                        else {
                                            PhysicalExamDataTemplateDetail.deleteFromJsonObject("ulExamCharacteristics", item.Value);
                                            PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
                                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails").addClass('hidden');
                                        }
                                    }

                                });

                                //   }
                                return;
                            }
                        });
                        PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
                    }
                    if (!isCharactristicsExists) {

                        l.find('li#' + item.Value).on('click', function () {
                            var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");
                            var selectedText = $(this).find('label').text() + ' Details';
                            liId = $(this).attr('id');
                            var currentObjectData = PhysicalExamDataTemplateDetail.ExamDetails[liId];
                            currentObjectData = (typeof currentObjectData != 'undefined' && currentObjectData != null) ? currentObjectData : "{}";
                            if ($(this).find("input:checkbox:checked").length > 0) {
                                PhysicalExamDataTemplateDetail.showHideToggleDetails(true);
                                $.when(PhysicalExamDataTemplateDetail.toggleDetailsDiv(liId)).then(function () {
                                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                    utility.bindMyJSONByName(true, JSON.parse(currentObjectData), false, detailSection);
                                });
                            }
                            else {
                                PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails").addClass('hidden');
                            }

                        });

                    }
                }
                else if (parentType != null && parentType.toLowerCase() == "characteristics") {
                    var isSubCharactristicsExists = false;
                    if (selectedData != null) {
                        var subCharacteristics = selectedData.SubCharacteristics;
                        $.each(subCharacteristics, function (i, subCharcetristicsComponent) {
                            if (subCharcetristicsComponent.CharSubCharacteristicId == item.Value) {

                                isSubCharactristicsExists = true;

                                if (subCharcetristicsComponent.IsDetailExists)
                                    l.find('li#' + item.Value).data('SystemSubCharacteristicDetails_' + item.Value, subCharcetristicsComponent.SectionCharacteristicSubCharacteristicDetailModel);
                                l.find('li#' + item.Value).data('SystemSubCharacteristicPk_' + item.Value, subCharcetristicsComponent.Id);

                                l.find('li#' + item.Value).addClass("green");
                                if (subCharcetristicsComponent.IsPositive) {

                                    if (l.find('li#' + item.Value).attr('subcharacteristicexist').trim() != "") {

                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '+ve"]').trigger('click');
                                    }
                                    else {
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '+ve"]').prop('checked', true);
                                    }
                                }
                                else {
                                    if (l.find('li#' + item.Value).attr('subcharacteristicexist').trim() != "")
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '-ve"]').trigger('click');
                                    else
                                        l.find('li#' + item.Value).find('input[id*="chk' + item.Value + '-ve"]').prop('checked', true);
                                }
                                if (subCharcetristicsComponent.Comments != "") {
                                    // l.find('li#' + item.Value).find('button').first().addClass('green');
                                    l.find('li#' + item.Value).find('textarea').text(subCharcetristicsComponent.Comments)
                                    l.find('li#' + item.Value).find('button').attr("data-original-title", subCharcetristicsComponent.Comments);
                                    l.find('li#' + item.Value).find('button').attr("title", subCharcetristicsComponent.Comments);
                                }
                                else {
                                    l.find('li#' + item.Value).find('button').first().removeClass('green');
                                }

                                //  if (subCharcetristicsComponent.IsDetailExists) {

                                l.find('li#' + item.Value).on('click', function () {
                                    var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #sectionPhysicalExamDetails");
                                    var dataDetail = {};
                                    liId = $(this).attr("id");
                                    var selectedText = $(this).find('label').text() + ' Details';
                                    var currentObjectData = $(this).data('SystemSubCharacteristicDetails_' + item.Value);
                                    if (!PhysicalExamDataTemplateDetail.isNullOrUndefined(currentObjectData)) {
                                        $.each(currentObjectData, function (key, value) {
                                            dataDetail[key + liId] = value;
                                        });

                                        if ($(this).find("input:checkbox:checked").length > 0) {
                                            PhysicalExamDataTemplateDetail.showHideToggleDetails(true);
                                            $.when(PhysicalExamDataTemplateDetail.toggleDetailsDiv(liId)).then(function () {
                                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                                utility.bindMyJSONByName(true, dataDetail, false, detailSection);
                                            });
                                        }
                                        else {
                                            PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
                                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails").addClass('hidden');
                                        }
                                    }
                                    else {
                                        var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");
                                        var selectedText = $(this).find('label').text() + ' Details';
                                        liId = $(this).attr('id');
                                        var currentObjectData = PhysicalExamDataTemplateDetail.ExamDetails[liId];
                                        currentObjectData = (typeof currentObjectData != 'undefined' && currentObjectData != null) ? currentObjectData : "{}";
                                        if ($(this).find("input:checkbox:checked").length > 0) {
                                            PhysicalExamDataTemplateDetail.showHideToggleDetails(true);
                                            if (!PhysicalExamDataTemplateDetail.isBothUnCheck) {
                                                PhysicalExamDataTemplateDetail.isBothUnCheck = false;
                                                $.when(PhysicalExamDataTemplateDetail.toggleDetailsDiv(liId)).then(function () {
                                                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                                    utility.bindMyJSONByName(true, JSON.parse(currentObjectData), false, detailSection);
                                                });
                                            }
                                        }
                                        else {
                                            PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
                                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails").addClass('hidden');
                                        }
                                    }
                                });
                                // }
                                return;
                            }
                        });
                    }
                    if (!isSubCharactristicsExists) {

                        l.find('li#' + item.Value).on('click', function () {
                            var detailSection = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExamDetails");
                            var selectedText = $(this).find('label').text() + ' Details';
                            liId = $(this).attr('id');
                            var currentObjectData = PhysicalExamDataTemplateDetail.ExamDetails[liId];
                            currentObjectData = (typeof currentObjectData != 'undefined' && currentObjectData != null) ? currentObjectData : "{}";
                            if ($(this).find("input:checkbox:checked").length > 0) {
                                PhysicalExamDataTemplateDetail.showHideToggleDetails(true);
                                if (!PhysicalExamDataTemplateDetail.isBothUnCheck) {
                                    PhysicalExamDataTemplateDetail.isBothUnCheck = false;
                                    $.when(PhysicalExamDataTemplateDetail.toggleDetailsDiv(liId)).then(function () {
                                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #sectionPhysicalExamDetails header.panel-heading h2").text(selectedText);
                                        utility.bindMyJSONByName(true, JSON.parse(currentObjectData), false, detailSection);
                                    });
                                }
                            }
                            else {
                                PhysicalExamDataTemplateDetail.showHideToggleDetails(false);
                                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail div#divTogglePhysicalExamDetails").addClass('hidden');
                            }

                        });

                    }
                }
            }

        });
        return;
    },

    //Author: Farooq Ahmad
    //Date: 25-02-2016
    //This function will Reset All Data In Physical Exam Form
    resetPhysicalExam: function () {

        utility.myConfirm('This will reset all values in all Systems and clear all information. This action is irreversible and cannot be undone. Would you like to proceed? ', function () {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {

                PhysicalExamDataTemplateDetail.resetControlValue(this);
            });
            utility.CreateDatePicker(PhysicalExamDataTemplateDetail.params.PanelID + '  #dtPhysicalExamDate', function () {
            }, true);
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID).find('ul li').removeClass('green');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID).find('ul li').removeClass('active');
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID).find('section#SystemSections,section#SectionCharacteristics,section#SectionCharacteristics,section#CharacteristicsSubCharacteristics,section#sectionPhysicalExamDetails').addClass('hidden');

            PhysicalExamDataTemplateDetail.myArr = [];
            PhysicalExamDataTemplateDetail.ExamDetails = {};
            PhysicalExamDataTemplateDetail.SectionNormalInfo = [];

            PhysicalExamDataTemplateDetail.selectedcharacteristicsIds = [];
            PhysicalExamDataTemplateDetail.characteristicsWithData = [];
            PhysicalExamDataTemplateDetail.selectedsubcharacteristicsIds = [];
            PhysicalExamDataTemplateDetail.subcharacteristicsWithData = [];
            PhysicalExamDataTemplateDetail.SectionNormalInfo = [];


        }, function () {

        }, "Confirm Reset");



    },

    //Author: Abid Ali
    //Date: 19-02-2016
    //This function will get  data of the inpute li Id and listType
    getObjectOfClickedElement: function (parentType, parentId) {
        var objData = null;
        //retrieve data of sections from system li's
        if (parentType != null && parentType.toLowerCase() == "system") {
            var ctrl = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems";
            objData = $(ctrl).find('li#' + parentId).data("SystemSectionIds_" + parentId);
        }
            //retrieve data of characteristics from section li's
        else if (parentType != null && parentType.toLowerCase() == "section") {
            var ctrl = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection";
            objData = $(ctrl).find('li#' + parentId).data("SystemCharacteristicsIds_" + parentId);
        }
            //retrieve data of subCharacteristics from characteristics li's
        else if (parentType != null && parentType.toLowerCase() == "characteristics") {
            var ctrl = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulExamCharacteristics";
            objData = $(ctrl).find('li#' + parentId).data("SystemSubCharacteristicsIds_" + parentId);
        }
        return objData;
    },

    //Author: Farooq Ahmad
    //Date: 25-02-2016
    //This function will set icon visibilty to block
    showIcon: function (obj) {

        $(obj).find('span').css('display', '');

    },

    //Author: Farooq Ahmad
    //Date: 25-02-2016
    //This function will set icon visibilty to none
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('span').css('display', 'none');
        }

    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle fill of PhysicalExam and it's childs as specified by PhysicalExamType
    loadPhysicalExamComponent: function (PhysicalExamType) {
        //Start 11-02-2016 Humaira Yousaf
        var objDeffered = $.Deferred();
        //End 11-02-2016 Humaira Yousaf
        PhysicalExamDataTemplateDetail.fillPhysicalExam(PhysicalExamType).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);

                var chkPhysicalExamNormal = '#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #chkPhysicalExamsNormal";
                var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail');

                if (response.physicalExamIsActive != false) {

                    var PhysicalExam_detail = JSON.parse(response.PatientPhysicalExamFill_JSON);
                    utility.bindMyJSONByName(true, PhysicalExam_detail, false, self).done(function () {
                        PhysicalExamDataTemplateDetail.params.mode = "Edit";
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #txtComments").val(PhysicalExam_detail.Comments);
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val(PhysicalExam_detail.physicalExamId);

                        var upperDate = self.find('input[name*="PatientPhysicalExamDate"]');

                        if (upperDate.val() == "") {
                            upperDate.datepicker('setDate', new Date());
                        } else {

                            //$(this).datepicker('setDate', $.datepicker.parseDate('yy-mm-dd', $(this).val()));
                            var date_format = 'mm/dd/yyyy';
                            //set default Date Formate
                            if (globalAppdata['DateFormat'])
                                date_format = globalAppdata['DateFormat'];
                            //  $(this).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), $(this).val()));
                            upperDate.datepicker({ date_format: date_format.replace('yy', '') }).val(upperDate.val());
                            upperDate.datepicker("setDate", upperDate.val());

                            PhysicalExamDataTemplateDetail.markSectionAsNormal(chkPhysicalExamNormal, false, true, PhysicalExam_detail.NormalExamsDetail);
                        }

                        //Start 11-02-2016 Humaira Yousaf
                        objDeffered.resolve(response.patientPhysicalExamSystemsFill_JSON);
                        //End 11-02-2016 Humaira Yousaf
                    });

                }
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').data('serialize', $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').serialize());

            }
            else {
                // utility.DisplayMessages(response.Message, 3);
                objDeffered.resolve(null);
            }
        });
        return objDeffered.promise();
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle fill of PhysicalExam and it's childs as specified by PhysicalExamType
    loadPhysicalExam: function (PhysicalExamType, isLoadNew) {

        if (isLoadNew == true) {
            //Start 11-02-2016 Humaira Yousaf for deffered object
            var objDeffered = $.Deferred();
            PhysicalExamDataTemplateDetail.loadPhysicalExamComponent(PhysicalExamType).done(function (response) {
                objDeffered.resolve(response);
            });
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + "  #hfPhysicalExamType").val(PhysicalExamType);
            return objDeffered.promise();
            //End 11-02-2016 Humaira Yousaf for deffered object
        }

    },

    //Author: Muhammad Arshad
    //Date: 19-01-2016
    //This function will handle fill of FamilyMember Details as specified by Given JSON
    fillCurrentMember: function (MemberId) {
        var currentJSON = PhysicalExamDataTemplateDetail.FamilyMembers[MemberId]
        if (currentJSON != null && currentJSON != "") {
            //PhysicalExamDataTemplateDetail.FamilyMembers
            //currentJSON = PhysicalExamDataTemplateDetail.FamilyMembers[Id];
            var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail');
            var currentJSON_detail = JSON.parse(currentJSON);
            utility.bindMyJSONByName(true, currentJSON_detail, false, self).done(function () {
                //Clinical_SocialHx.params.mode = "Edit";
            });
        }
    },


    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle active state of current li item on the basis of li's Id
    markStatusActive: function (ulId, liId) {
        if (ulId != null && ulId != "") {
            // Mark current status as Acitve
            //preActiveStatus = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #Miscellaneous ul#" + ulId + " li.active").text().trim();

            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #PhysicalExam ul#" + ulId + " li").each(function (i, item) {
                if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                    if ($(this).hasClass("active") == false) {
                        $(this).addClass("active");
                        $(this).children().removeClass("green");
                        var currentId = $(this).attr("id");
                        ////var MemberDetailDiv = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #FamilyDisease div#FamilyMemberDetails');
                        ////MemberDetailDiv.removeClass("disableAll")
                        //PhysicalExamDataTemplateDetail.resetFamilyMemberControls();
                        //PhysicalExamDataTemplateDetail.fillCurrentMember(currentId);
                    }
                }
                else {
                    if ($(this).hasClass("active") == true) {
                        $(this).removeClass("active");
                    }
                }
            });
        }

    },

    //Author: Muhammad
    //Date: 01-05-2016
    //This function will handle enabling/disabling of Exercises controls on Miscellanous Tab
    enableDisableExercisesControls: function (currentStatus) {
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #Miscellaneous section#sectionMiscDetails div#ExercisesDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (currentStatus.toLowerCase() == "does not exercise") {
                    if ($(this).attr("id") != "txtExercisesComments") {
                        $(this).attr("disabled", "disabled");
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                }
                else {
                    $(this).removeAttr("disabled");
                }
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
                PhysicalExamDataTemplateDetail.resetControlValue(this);
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
            });
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle enabling/disabling of Exercises controls on Miscellanous Tab
    enableDisableList: function (listId, isDisable) {
        if (listId != null && listId != "") {
            var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail ' + listId + " li").not(":first").each(function () {

                if (isDisable) {
                    if (!$(this).hasClass('disableAll'))
                        $(this).addClass('disableAll');
                }
                else
                    $(this).removeClass('disableAll');
            });

        }
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle unremarkable feature for Family Hx
    unRemarkablePhysicalExam: function (obj, isFromLoad) {
        var isRemarkable = $(obj).prop("checked");
        if (isRemarkable == true) {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #btnPhysicalExamSave').show();

            /* Start 23/12/2015 Muhammad Irfan for bug # EMR-153 */
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #ulPhysicalExamTabsItems').addClass('disableAll');
            /* End 23/12/2015 Muhammad Irfan for bug # EMR-153 */

            /* Start 07/01/2016 Muhammad Irfan for bug # EMR-153 */
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #listTobacco,#listAlcohol,#listDrugAbuse,#listSexualHx').removeClass('successLight');
            /* End 07/01/2016 Muhammad Irfan for bug # EMR-153 */

        }
        else {
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #btnPhysicalExamSave').hide();
        }
        var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').find('div#Tobacco,div#Alcohol,div#DrugAbuse,div#SexualHx,div#Miscellaneous').each(function () {
            if (isRemarkable == true) {
                if ($(this).hasClass("disableAll") == false) {
                    $(this).addClass("disableAll");

                }
                $(this).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
                    $(this).attr("disabled", "disabled");
                    PhysicalExamDataTemplateDetail.resetControlValue(this);
                });
                //PhysicalExamDataTemplateDetail.resetControlValue(this);
            }
            else {
                $(this).removeClass("disableAll");
                /* Start 23/12/2015 Muhammad Irfan for bug # EMR-153 */
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #ulPhysicalExamTabsItems').removeClass('disableAll');
                /* End 23/12/2015 Muhammad Irfan for bug # EMR-153 */

                $(this).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
                    $(this).removeAttr("disabled");
                    PhysicalExamDataTemplateDetail.resetControlValue(this);
                });
                /* Start 07/01/2016 Muhammad Irfan for bug # EMR-153 */
                //if (PhysicalExamDataTemplateDetail.bAlcoholExist == true)
                //    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #listAlcohol').addClass('successLight');
                //if (PhysicalExamDataTemplateDetail.bTobaccoExist == true)
                //    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #listTobacco').addClass('successLight');
                //if (PhysicalExamDataTemplateDetail.bDrugExist == true)
                //    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                //if (PhysicalExamDataTemplateDetail.bSexualExist == true)
                //    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #listSexualHx').addClass('successLight');



                //$('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #listTobacco,#listAlcohol,#listDrugAbuse,#listSexualHx').removeClass('active');
                /* End 07/01/2016 Muhammad Irfan for bug # EMR-153 */
            }
        });
        if (!isRemarkable)
            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #listTobacco a").trigger("click");


    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will clear value of given control as specified by obj
    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea' || $(obj).attr('type') == 'hidden')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
                var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                if (groupRadBtn.length > 1) {
                    $.each(groupRadBtn, function (i, item) {
                        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                            $(item).trigger("click");
                        }
                    });
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Family History Clinical Module -> Fields should be blank when select other status
            }
            else {
                obj.checked = false;
            }
        }

        if (currentElementTagName.toLowerCase() == 'select') {
            $(obj).find('option:selected').removeAttr('selected');
            //$(this).attr('selectedIndex', '-1');
            $(obj).find('option:eq(0)').attr('selected', 'selected');
        }
        if (currentElementTagName.toLowerCase() == 'ul') {
            $(obj).find('li.active').removeClass('active');
        }
    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will check if any characteristic/subcharacteristic is selected
    isSystemChecked: function (objulSystem, liObject) {
        var DetailExists = false;
        if (liObject != null && liObject != "") {
            $(liObject).find("input[type='checkbox']").each(function (i, chkitem) {
                if ($(chkitem).prop("checked") == true && DetailExists == false) {
                    DetailExists = true;
                }
            });
        }
        else {
            objulSystem.find("li").each(function (i, item) {
                $(item).find("input[type='checkbox']").each(function (i, chkitem) {
                    if ($(chkitem).prop("checked") == true && DetailExists == false) {
                        DetailExists = true;
                    }
                });
            });
        }


        return DetailExists
    },

    //Author: Muhammad Arshad
    //Date: 02-02-2016
    //This function will check if any characteristic/subcharacteristic has data in details section
    isDetailsHaveData: function () {
        var DetailExists = false;
        var sectionDetails = "";
        var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails').find('[type=hidden],[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
            if ($(this).prop("disabled") != true && DetailExists == false) {
                var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'checkbox' && this.checked == true) {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'radio' && $(this).attr('id').toLowerCase().indexOf("no") > -1 && this.checked == true) {
                    DetailExists = false;
                }
                if (currentElementTagName.toLowerCase() == 'select' && $(this).val() != null && $(this).val() != "") {
                    DetailExists = true;
                }
                if ($(this).attr('type') == 'hidden' && $(this).val() != "" && $(this).val() != "-1") {
                    DetailExists = true;
                }
                //if (currentElementTagName.toLowerCase() == 'ul') {
                //    $(this).find('li.active').removeClass('active');
                //}
            }
        });

        return DetailExists;
    },

    //Author: Muhammad Arshad
    //Date: 21-01-2016
    //This function will check if any characteristic/subcharacteristic has any value selected
    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";


        var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #PhysicalExam");
        var objCharacteristic = self.find("div#divExamCharacteristics");
        var objSubCharacteristic = self.find("section#CharacteristicsSubCharacteristics");
        if (objSubCharacteristic.hasClass("hidden") == false) {
            DetailExists = PhysicalExamDataTemplateDetail.isSystemChecked(objSubCharacteristic.find("ul#ulExamSubCharacteristics"));
            if (DetailExists == false) {
                DetailExists = PhysicalExamDataTemplateDetail.isSystemChecked(objCharacteristic.find("ul#ulExamCharacteristics"));
            }
        }
        else if (objCharacteristic.hasClass("hidden") == false) {
            DetailExists = PhysicalExamDataTemplateDetail.isSystemChecked(objCharacteristic.find("ul#ulExamCharacteristics"));
        }
        return DetailExists;

    },

    //Author: Muhammad Arshad
    //Date: 03-02-2016
    //This function will return comma Separated Ids of either selected Characteristics/SubCharacteristics from Given JSON on basis of characteristics Type as parameter
    getCommaSeparatedIds: function (arrSelectedJSON, IsSubCharacteristic) {
        var selectedIds = "";
        var isFirstSelected = false;
        //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
        if (IsSubCharacteristic == true) {
            PhysicalExamDataTemplateDetail.subcharacteristicsWithData.length = 0;
            PhysicalExamDataTemplateDetail.selectedsubcharacteristicsIds.length = 0;
        }
        else if (IsSubCharacteristic == false) {
            PhysicalExamDataTemplateDetail.characteristicsWithData.length = 0;
            PhysicalExamDataTemplateDetail.selectedcharacteristicsIds.length = 0;
        }
        //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
        if (arrSelectedJSON != null)
            $.each(arrSelectedJSON, function (i, item) {
                var numericPart = 0;
                for (key in item) {
                    var myval = key;
                    if (key.indexOf("SubCharacteristicId") > -1) {
                        numericPart = key.replace(/[^\d]+/, '');
                    }
                }

                //var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                //if (currentSubCharacteristicId != null && currentSubCharacteristicId != "" && parseInt(currentSubCharacteristicId) > 0) {
                //    //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                //    PhysicalExamDataTemplateDetail.subcharacteristicsWithData.push(item);
                //    //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                //}
                //else if (currentCharacteristicId != null && currentCharacteristicId != "" && parseInt(currentCharacteristicId) > 0) {
                //    //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                //    PhysicalExamDataTemplateDetail.characteristicsWithData.push(item);
                //    //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                //}

                if (IsSubCharacteristic == true) {
                    var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                    var currentCharacteristicId = item["CharacteristicId" + numericPart];
                    if (currentSubCharacteristicId != null && currentSubCharacteristicId != "" && parseInt(currentSubCharacteristicId) > 0) {
                        if (isFirstSelected == false) {
                            selectedIds = currentSubCharacteristicId;
                            isFirstSelected = true;
                        }
                        else if (isFirstSelected == true) {
                            selectedIds += "," + currentSubCharacteristicId;
                        }
                        //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                        var num = PhysicalExamDataTemplateDetail.getNumberPart(item);

                        var Index = PhysicalExamDataTemplateDetail.selectedsubcharacteristicsIds.indexOf(num);
                        if (Index == -1 && item["CharacteristicId" + num] != num) {
                            PhysicalExamDataTemplateDetail.subcharacteristicsWithData.push(item);
                            PhysicalExamDataTemplateDetail.selectedsubcharacteristicsIds.push(num);
                        }


                        //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                    }
                }
                else {
                    var currentSubCharacteristicId = item["CharacteristicId" + numericPart];
                    if (item["CharacteristicId" + numericPart] != "") {
                        var currentCharacteristicId = item["CharacteristicId" + numericPart];
                        if (currentCharacteristicId != null && currentCharacteristicId != "" && parseInt(currentCharacteristicId) > 0) {
                            if (isFirstSelected == false) {
                                selectedIds = currentCharacteristicId;
                                isFirstSelected = true;
                            }
                            else if (isFirstSelected == true) {
                                selectedIds += "," + currentCharacteristicId;
                            }
                            var currentSubCharacteristicId = item["SubCharacteristicId" + numericPart];
                            if (currentSubCharacteristicId == "" || currentSubCharacteristicId == "-1") {
                                //Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                                PhysicalExamDataTemplateDetail.characteristicsWithData.push(item);
                                PhysicalExamDataTemplateDetail.selectedcharacteristicsIds.push(currentCharacteristicId);
                                //End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
                            }

                        }
                    }
                }
            });

        return selectedIds;
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Add/Edit of PhysicalExam and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous), it expects PhysicalExamType to be Add/Edit
    PhysicalExamSave: function (PhysicalExamType, UnloadPhysicalExam) {

        var PhysicalExamId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val() != "" ? $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val() : "-1";

        if (parseInt(PhysicalExamId) > 0) {
            PhysicalExamDataTemplateDetail.params.mode = "Edit";
        }
        else {
            PhysicalExamDataTemplateDetail.params.mode = "Add";
        }
        var obj = new Object();
        obj.bNormal = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #chkPhysicalExamsNormal").is(':checked');
        obj.TemplateId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        obj.PatientId = $('#PatientProfile #hfPatientId').val();
        obj.NotesId = PhysicalExamDataTemplateDetail.params.NotesId;
        obj.Comments = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + "#txtComments").val();
        obj.Systems = PhysicalExamDataTemplateDetail.DataJSON;


        if (PhysicalExamDataTemplateDetail.params.mode == "Add") {

            AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    PhysicalExamDataTemplateDetail.savePhysicalExam(obj).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val(response.PhysicalExamId);
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val(response.NoteId);
                            PhysicalExamDataTemplateDetail.params.mode = "Edit";
                            if (PhysicalExamDataTemplateDetail.params.ParentCtrl == "clinicalTabProgressNote" && UnloadPhysicalExam == true) {
                                PhysicalExamDataTemplateDetail.getPhysicalExamInfo(PhysicalExamType, UnloadPhysicalExam);
                            }
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').data('serialize', $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').serialize());
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
        else if (PhysicalExamDataTemplateDetail.params.mode == "Edit") {

            AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    PhysicalExamDataTemplateDetail.updatePhysicalExam(myJSON, PhysicalExamId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            if (PhysicalExamDataTemplateDetail.params.ParentCtrl == "clinicalTabProgressNote" && UnloadPhysicalExam == true) {
                                PhysicalExamDataTemplateDetail.getPhysicalExamInfo(PhysicalExamType, UnloadPhysicalExam);
                            }
                            utility.DisplayMessages(response.message, 1);
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val(response.PhysicalExamId);
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val(response.NotesId);
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').data('serialize', $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').serialize());

                        }
                        else {
                            utility.DisplayMessages(response.message, 3);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });

        }

        //var isFirstSelected = false;
        ////Farooq Ahmad
        ////update the global exam detail array before posting to the server
        //var finalStr = PhysicalExamDataTemplateDetail.UpdateExamDetailArray();

        //var arrSelectedJSON = [];
        //$.each(PhysicalExamDataTemplateDetail.ExamDetails, function (i, item) {
        //    var mystr = item;
        //    arrSelectedJSON.push(JSON.parse(item));
        //    finalStr = finalStr.concat(item);
        //    finalStr = finalStr.replace("}{", ",");
        //});

        ////Start 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic
        //var selectedCharacteristicIds = PhysicalExamDataTemplateDetail.getCommaSeparatedIds(arrSelectedJSON, false);
        //selectedCharacteristicIds = PhysicalExamDataTemplateDetail.selectedcharacteristicsIds.join(",");

        //var selectedSubCharacteristicIds = PhysicalExamDataTemplateDetail.getCommaSeparatedIds(arrSelectedJSON, true);
        //selectedSubCharacteristicIds = PhysicalExamDataTemplateDetail.selectedsubcharacteristicsIds.join(",");

        //PhysicalExamDataTemplateDetail.SelectedDataArrayToExamDetailArray(selectedCharacteristicIds, selectedSubCharacteristicIds);

        //var finalStrCharData = PhysicalExamDataTemplateDetail.CharWithData();

        //var finalStrSubCharData = PhysicalExamDataTemplateDetail.SubCharWithData();

        ////End 11-02-2016 Muhammad Arshad Handling Detail for Characteristic/subcharacteristic

        //var errorMessage = PhysicalExamDataTemplateDetail.CheckForSysSecChar();

        //var DetailExists = false;

        //var mainErrorMessage = "Please enter any value";
        //if (errorMessage != "") {
        //    DetailExists = false;
        //    mainErrorMessage = errorMessage;
        //}
        //else {
        //    // if (PhysicalExamType != undefined && PhysicalExamType != "") {
        //    DetailExists = PhysicalExamDataTemplateDetail.isDetailExists(); //PhysicalExam type parameter removed by Ahmad Raza
        //    // }
        //}
        //if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + "#txtComments").val() != '') {
        //    DetailExists = true;
        //}
        //if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #chkPhysicalExamsNormal").is(':checked')) {
        //    DetailExists = true;
        //}
        //var PhysicalExamDateExists = false;
        //if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #dtPhysicalExamDate").val() != null && $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #dtPhysicalExamDate").val() != '') {
        //    PhysicalExamDateExists = true;
        //}

        ////Start Farooq Ahmad 26/02/2016
        //var DurationValid = PhysicalExamDataTemplateDetail.durationIsValid();
        //if (DurationValid != "") {
        //    mainErrorMessage = DurationValid;
        //    DetailExists = false;
        //}
        ////End Farooq Ahmad 26/02/2016

        ////Start 10-02-2016 Humaira Yousaf to check SectionNormalInfo
        //if ((PhysicalExamDataTemplateDetail.SectionNormalInfo.length > 0 || DetailExists == true) && PhysicalExamDateExists) {
        //    //End 10-02-2016 Humaira Yousaf to check SectionNormalInfo
        //    var strMessage = "";
        //    var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail");
        //    var myJSON = self != null ? self.getMyJSONByName() : "{}";
        //    var objData = JSON.parse(myJSON);
        //    objData["characteristicIds"] = selectedCharacteristicIds;
        //    objData["subcharacteristicIds"] = selectedSubCharacteristicIds;
        //    objData["characteristicdata"] = finalStrCharData;
        //    objData["subcharacteristicdata"] = finalStrSubCharData;
        //    objData["Comments"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + "  #txtComments").val();
        //    //Start 09-02-2016 Humaira Yousaf to send ids of normal systems
        //    objData["NormalSystemIds"] = PhysicalExamDataTemplateDetail.SectionNormalInfo;

        //    //Start/ 29-03-2016 / Abid Ali/ To check systemId
        //    if (objData["SystemId"] != null && objData["SystemId"] == '')
        //        objData["SystemId"] = -1;
        //    //End/ 29-03-2016 / Abid Ali/ To check systemId

        //    myJSON = JSON.stringify(objData);
        //    //return false;
        //    if (PhysicalExamDataTemplateDetail.params.mode == "Add") {

        //        AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //            if (strMessage == "") {
        //                PhysicalExamDataTemplateDetail.savePhysicalExam(myJSON).done(function (response) {
        //                    response = JSON.parse(response);
        //                    if (response.status != false) {
        //                        utility.DisplayMessages(response.message, 1);
        //                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val(response.PhysicalExamId);
        //                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val(response.NoteId);
        //                        PhysicalExamDataTemplateDetail.params.mode = "Edit";
        //                        if (PhysicalExamDataTemplateDetail.params.ParentCtrl == "clinicalTabProgressNote" && UnloadPhysicalExam == true) {
        //                            PhysicalExamDataTemplateDetail.getPhysicalExamInfo(PhysicalExamType, UnloadPhysicalExam);
        //                        }
        //                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').data('serialize', $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').serialize());
        //                    }
        //                    else {
        //                        utility.DisplayMessages(response.Message, 3);
        //                    }
        //                });
        //            }
        //            else
        //                utility.DisplayMessages(strMessage, 2);
        //        });
        //    }
        //    else if (PhysicalExamDataTemplateDetail.params.mode == "Edit") {

        //        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //            if (strMessage == "") {
        //                PhysicalExamDataTemplateDetail.updatePhysicalExam(myJSON, PhysicalExamId).done(function (response) {
        //                    response = JSON.parse(response);
        //                    if (response.status != false) {
        //                        if (PhysicalExamDataTemplateDetail.params.ParentCtrl == "clinicalTabProgressNote" && UnloadPhysicalExam == true) {
        //                            PhysicalExamDataTemplateDetail.getPhysicalExamInfo(PhysicalExamType, UnloadPhysicalExam);
        //                        }
        //                        utility.DisplayMessages(response.message, 1);
        //                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val(response.PhysicalExamId);
        //                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val(response.NotesId);
        //                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').data('serialize', $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail').serialize());

        //                    }
        //                    else {
        //                        utility.DisplayMessages(response.message, 3);
        //                    }
        //                });
        //            }
        //            else
        //                utility.DisplayMessages(strMessage, 2);
        //        });

        //    }
        //}
        //else {
        //    if (PhysicalExamDateExists == false) {
        //        utility.DisplayMessages("Please enter Date.", 3);
        //    }
        //    else {
        //        utility.DisplayMessages(mainErrorMessage, 3);
        //    }
        //}

    },

    //Author : Farooq Ahmad
    //Date : 13-06-2016
    //This Function will return the Characteristics with data
    CharWithData: function () {
        var finalStrCharData = "";
        $.each(PhysicalExamDataTemplateDetail.characteristicsWithData, function (i, item) {
            var subCharNum = PhysicalExamDataTemplateDetail.getNumberPart(item);
            if (item['SubCharacteristicId' + subCharNum] == '' || item['SubCharacteristicId' + subCharNum] == '-1')
                item['CharacteristicId' + subCharNum] = subCharNum;
            finalStrCharData = finalStrCharData.concat(JSON.stringify(item)); //JSON.stringify(objData);
            finalStrCharData = finalStrCharData.replace("}{", ",");
        });
        return finalStrCharData;
    },

    //Author :  Farooq Ahmad
    //Date : 13-06-2016
    //This function will return the SubCharacteristics with data
    SubCharWithData: function () {
        var finalStrSubCharData = "";
        $.each(PhysicalExamDataTemplateDetail.subcharacteristicsWithData, function (i, item) {

            var subCharNum = PhysicalExamDataTemplateDetail.getNumberPart(item);
            item['SubCharacteristicId' + subCharNum] = subCharNum;

            finalStrSubCharData = finalStrSubCharData.concat(JSON.stringify(item));
            finalStrSubCharData = finalStrSubCharData.replace("}{", ",");
        });
        return finalStrSubCharData;
    },

    //Author : Farooq Ahmad
    //Date : 13-06-2016
    //This function will check the System , Section and Characterisctics are selected or not
    CheckForSysSecChar: function () {
        var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #sectionPhysicalExam");
        var errorMessage = "";
        var selectedSystemId = self.find("#ulPhysicalExamSystems li.active").attr("id");
        var selectedSectionId = self.find("#ulPhysicalExamSystemSection li.active").attr("id");
        var selectedCharacteristicId = self.find("#ulExamCharacteristics li.active").attr("id");
        var selectedSubCharacteristicId = 1;
        if (self.find("#CharacteristicsSubCharacteristics").hasClass("hidden") == false) {
            selectedSubCharacteristicId = self.find("#ulExamSubCharacteristics li.active").attr("id");
        }
        if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #divTogglePhysicalExamDetails-1').hasClass('hidden') == true) {
            if (!(selectedSystemId != null && parseInt(selectedSystemId) > 0)) {
                errorMessage = "Please select any system.";
            }

            //Start 10-02-2016 Humaira Yousaf to check SectionNormalInfo
            if (PhysicalExamDataTemplateDetail.SectionNormalInfo == null || PhysicalExamDataTemplateDetail.SectionNormalInfo.length == 0) {
                //End 10-02-2016 Humaira Yousaf to check SectionNormalInfo
                if (!(selectedSectionId != null && parseInt(selectedSectionId) > 0)) {
                    errorMessage = "Please select any section.";
                }
                else if (!(selectedCharacteristicId != null && parseInt(selectedCharacteristicId) > 0)) {
                    errorMessage = "Please select any characteristic.";
                }
            }
        }
        return errorMessage;
    },

    //Author : Farooq Ahmad
    //Date: 13-06-2016
    //This Function will store the Data in Global ExamDetail Array from the selectedData
    SelectedDataArrayToExamDetailArray: function (selectedCharacteristicIds, selectedSubCharacteristicIds) {
        var selectedDataObj = JSON.parse(PhysicalExamDataTemplateDetail.selectedData);
        for (var sys in selectedDataObj) {
            for (var sec in selectedDataObj[sys].Sections) {
                for (var char in selectedDataObj[sys].Sections[sec].Characteristics) {
                    var isExit = false;
                    for (var AlredyAddedCharIds in PhysicalExamDataTemplateDetail.selectedcharacteristicsIds) {
                        if (selectedDataObj[sys].Sections[sec].Characteristics[char].SectionCharacteristicId == PhysicalExamDataTemplateDetail.selectedcharacteristicsIds[AlredyAddedCharIds]) {
                            isExit = true;
                            break;
                        }
                    }
                    if (!isExit) {
                        var numberPart = selectedDataObj[sys].Sections[sec].Characteristics[char].SectionCharacteristicId;
                        var Obj = {};
                        Obj["SystemId" + numberPart] = selectedDataObj[sys].SystemId.toString();
                        Obj["SectionId" + numberPart] = selectedDataObj[sys].Sections[sec].SectionId.toString();
                        Obj["CharacteristicId" + numberPart] = selectedDataObj[sys].Sections[sec].Characteristics[char].SectionCharacteristicId.toString();
                        Obj["SubCharacteristicId" + numberPart] = "";
                        Obj["CharacteristicsDetails" + numberPart] = "";
                        Obj["IsCharacteristicPositive" + numberPart] = selectedDataObj[sys].Sections[sec].Characteristics[char].IsPositive.toString();
                        Obj["IsSubCharacteristicPositive" + numberPart] = "";
                        var detail = selectedDataObj[sys].Sections[sec].Characteristics[char].SectionCharacteristicDetailModel;
                        Obj["PhysicalExamPreviousHistory" + numberPart] = detail.PhysicalExamPreviousHistory.toString();
                        Obj["PhysicalExamOnset" + numberPart] = detail.PhysicalExamOnset.toString();
                        Obj["PhysicalExamDurationLength" + numberPart] = detail.PhysicalExamDurationLength.toString();
                        Obj["PhysicalExamLocation" + numberPart] = detail.PhysicalExamLocation.toString();
                        Obj["PhysicalExamPercipitatedby" + numberPart] = detail.PhysicalExamPercipitatedby.toString();
                        Obj["PhysicalExamAssociatedwith" + numberPart] = detail.PhysicalExamAssociatedwith.toString();
                        Obj["PhysicalExamStatus" + numberPart] = detail.PhysicalExamStatus.toString();
                        Obj["PhysicalExamStatus" + numberPart + "_text"] = detail.PhysicalExamStatus.toString();
                        Obj["PhysicalExamDurationPeriod" + numberPart] = detail.PhysicalExamDurationPeriod.toString();
                        Obj["PhysicalExamDurationPeriod" + numberPart + "_text"] = detail.PhysicalExamOnset.toString();
                        Obj["PhysicalExamPattern" + numberPart] = detail.PhysicalExamPattern.toString();
                        Obj["PhysicalExamPattern" + numberPart + "_text"] = detail.PhysicalExamPattern.toString();
                        Obj["PhysicalExamSeverity" + numberPart] = detail.PhysicalExamSeverity.toString();
                        Obj["PhysicalExamSeverity" + numberPart + "_text"] = detail.PhysicalExamSeverity.toString();
                        Obj["PhysicalExamCourse" + numberPart] = detail.PhysicalExamCourse.toString();
                        Obj["PhysicalExamCourse" + numberPart + "_text"] = detail.PhysicalExamCourse.toString();
                        Obj["PhysicalExamRadiation" + numberPart] = detail.PhysicalExamRadiation.toString();;
                        Obj["PhysicalExamRadiation" + numberPart + "_text"] = detail.PhysicalExamRadiation.toString();
                        Obj["PhysicalExamFrequency" + numberPart] = detail.PhysicalExamFrequency.toString();
                        Obj["PhysicalExamFrequency" + numberPart + "_text"] = detail.PhysicalExamFrequency.toString();
                        Obj["PhysicalExamContext" + numberPart] = detail.PhysicalExamContext.toString();
                        Obj["PhysicalExamContext" + numberPart + "_text"] = detail.PhysicalExamContext.toString();
                        Obj["PhysicalExamCharacter" + numberPart] = detail.PhysicalExamCharacter.toString();
                        Obj["PhysicalExamCharacter" + numberPart + "_text"] = detail.PhysicalExamCharacter.toString();
                        Obj["PhysicalExamAgggravatedby" + numberPart] = detail.PhysicalExamAgggravatedby.toString();
                        Obj["PhysicalExamAgggravatedby" + numberPart + "_text"] = detail.PhysicalExamAgggravatedby.toString();
                        Obj["PhysicalExamRelievedby" + numberPart] = detail.PhysicalExamRelievedby.toString();
                        Obj["PhysicalExamRelievedby" + numberPart + "_text"] = detail.PhysicalExamRelievedby.toString();

                        PhysicalExamDataTemplateDetail.ExamDetails[numberPart] = JSON.stringify(Obj);
                        selectedCharacteristicIds = selectedCharacteristicIds + (selectedCharacteristicIds == '' ? '' : ',') + Obj["CharacteristicId" + numberPart];
                        PhysicalExamDataTemplateDetail.characteristicsWithData.push(Obj);
                    }

                    for (var subchar in selectedDataObj[sys].Sections[sec].Characteristics[char].SubCharacteristics) {
                        var isExit = false;

                        for (var AlredyAddedSubCharIds in PhysicalExamDataTemplateDetail.selectedsubcharacteristicsIds) {
                            if (selectedDataObj[sys].Sections[sec].Characteristics[char].SubCharacteristics[subchar].Id == PhysicalExamDataTemplateDetail.selectedsubcharacteristicsIds[AlredyAddedSubCharIds]) {
                                isExit = true;
                                break;
                            }
                        }
                        if (!isExit) {
                            var numberPart = selectedDataObj[sys].Sections[sec].Characteristics[char].SubCharacteristics[subchar].CharSubCharacteristicId.toString();
                            var Obj = {};
                            Obj["SystemId" + numberPart] = selectedDataObj[sys].SystemId.toString();
                            Obj["SectionId" + numberPart] = selectedDataObj[sys].Sections[sec].SectionId.toString();
                            Obj["CharacteristicId" + numberPart] = selectedDataObj[sys].Sections[sec].Characteristics[char].SectionCharacteristicId.toString();
                            Obj["SubCharacteristicId" + numberPart] = selectedDataObj[sys].Sections[sec].Characteristics[char].SubCharacteristics[subchar].CharSubCharacteristicId.toString();
                            Obj["CharacteristicsDetails" + numberPart] = "";
                            Obj["IsCharacteristicPositive" + numberPart] = selectedDataObj[sys].Sections[sec].Characteristics[char].IsPositive.toString();
                            var detail = selectedDataObj[sys].Sections[sec].Characteristics[char].SubCharacteristics[subchar].SectionCharacteristicSubCharacteristicDetailModel;
                            Obj["IsSubCharacteristicPositive" + numberPart] = selectedDataObj[sys].Sections[sec].Characteristics[char].SubCharacteristics[subchar].IsPositive.toString();
                            Obj["PhysicalExamPreviousHistory" + numberPart] = detail.PhysicalExamPreviousHistory.toString();
                            Obj["PhysicalExamOnset" + numberPart] = detail.PhysicalExamOnset.toString();
                            Obj["PhysicalExamDurationLength" + numberPart] = detail.PhysicalExamDurationLength.toString();
                            Obj["PhysicalExamLocation" + numberPart] = detail.PhysicalExamLocation.toString();
                            Obj["PhysicalExamPercipitatedby" + numberPart] = detail.PhysicalExamPercipitatedby.toString();
                            Obj["PhysicalExamAssociatedwith" + numberPart] = detail.PhysicalExamAssociatedwith.toString();
                            Obj["PhysicalExamStatus" + numberPart] = detail.PhysicalExamStatus.toString();
                            Obj["PhysicalExamStatus" + numberPart + "_text"] = detail.PhysicalExamStatus.toString();
                            Obj["PhysicalExamDurationPeriod" + numberPart] = detail.PhysicalExamDurationPeriod.toString();
                            Obj["PhysicalExamDurationPeriod" + numberPart + "_text"] = detail.PhysicalExamOnset.toString();
                            Obj["PhysicalExamPattern" + numberPart] = detail.PhysicalExamPattern.toString();
                            Obj["PhysicalExamPattern" + numberPart + "_text"] = detail.PhysicalExamPattern.toString();
                            Obj["PhysicalExamSeverity" + numberPart] = detail.PhysicalExamSeverity.toString();
                            Obj["PhysicalExamSeverity" + numberPart + "_text"] = detail.PhysicalExamSeverity.toString();
                            Obj["PhysicalExamCourse" + numberPart] = detail.PhysicalExamCourse.toString();
                            Obj["PhysicalExamCourse" + numberPart + "_text"] = detail.PhysicalExamCourse.toString();
                            Obj["PhysicalExamRadiation" + numberPart] = detail.PhysicalExamRadiation.toString();;
                            Obj["PhysicalExamRadiation" + numberPart + "_text"] = detail.PhysicalExamRadiation.toString();
                            Obj["PhysicalExamFrequency" + numberPart] = detail.PhysicalExamFrequency.toString();
                            Obj["PhysicalExamFrequency" + numberPart + "_text"] = detail.PhysicalExamFrequency.toString();
                            Obj["PhysicalExamContext" + numberPart] = detail.PhysicalExamContext.toString();
                            Obj["PhysicalExamContext" + numberPart + "_text"] = detail.PhysicalExamContext.toString();
                            Obj["PhysicalExamCharacter" + numberPart] = detail.PhysicalExamCharacter.toString();
                            Obj["PhysicalExamCharacter" + numberPart + "_text"] = detail.PhysicalExamCharacter.toString();
                            Obj["PhysicalExamAgggravatedby" + numberPart] = detail.PhysicalExamAgggravatedby.toString();
                            Obj["PhysicalExamAgggravatedby" + numberPart + "_text"] = detail.PhysicalExamAgggravatedby.toString();
                            Obj["PhysicalExamRelievedby" + numberPart] = detail.PhysicalExamRelievedby.toString();
                            Obj["PhysicalExamRelievedby" + numberPart + "_text"] = detail.PhysicalExamRelievedby.toString();

                            PhysicalExamDataTemplateDetail.ExamDetails[numberPart] = JSON.stringify(Obj);
                            PhysicalExamDataTemplateDetail.subcharacteristicsWithData.push(Obj);

                            selectedSubCharacteristicIds = selectedSubCharacteristicIds + (selectedSubCharacteristicIds == '' ? '' : ',') + Obj["SubCharacteristicId" + numberPart];
                        }
                    }
                }
            }
        }
    },

    //Author : Farooq Ahmad
    //Date : 13-06-2016
    //This function will update the detail of the Characteristics And Subcharacteristics
    UpdateExamDetailArray: function () {

        var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails')
        var finalStr = self != null ? self.getMyJSONByName() : "";
        var selectedJSON = JSON.parse(finalStr);
        var selectedLiId = PhysicalExamDataTemplateDetail.getNumberPart(selectedJSON)
        if (selectedLiId != null && selectedLiId != "") {
            if (PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId] != null) {
                PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId] = PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId].replace(PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId], finalStr);
            }
            else {
                PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId] = finalStr;
            }
        }
        return finalStr;

    },

    //Author : Farooq Ahmad
    //Date : 13-06-2016
    //This function will update the detail if SystemId and SectionId are match with existing other wise add
    AddUpdateExamDetailArrayOnSystemIdAndSectionId: function (paramId) {

        var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails');
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var selectedJSON = JSON.parse(myJSON);
        var selectedLiId = PhysicalExamDataTemplateDetail.getNumberPart(selectedJSON);
        if (paramId != null) {
            selectedLiId = paramId;
        }
        if (selectedLiId != null && selectedLiId != "") {
            if (PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId] != null) {
                if (JSON.parse(PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId])["SectionId" + selectedLiId] == selectedJSON["SectionId" + selectedLiId] &&
                JSON.parse(PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId])["SystemId" + selectedLiId] == selectedJSON["SystemId" + selectedLiId])
                    PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId] = PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId].replace(PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId], myJSON);
            }
            else {
                PhysicalExamDataTemplateDetail.ExamDetails[selectedLiId] = myJSON;
            }
        }

    },

    //Author: Farooq Ahmad
    //Date: 26/02/2016
    //This will validate the detail section
    durationIsValid: function () {
        var Message = "";

        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails input[id*=txtPhysicalExamDurationLength]').each(function () {
            var num = $(this).attr('id').replace(/[^\d]+/, '');
            var stayLength = $(this).val();
            var ddlVal = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails #ddlPhysicalExamDurationPeriod' + num).val();
            if (stayLength != null && stayLength != '') {
                if (ddlVal == null || ddlVal == '') {
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #frmPhysicalExamDataTemplateDetail #CharacteristicsDetails #ddlPhysicalExamDurationPeriod' + num).focus();
                    Message = "Please select Physical Exam Duration Period";
                }
            }

            if (ddlVal != null && ddlVal != '') {
                if (stayLength == null || stayLength == '') {
                    $(this).focus();
                    Message = "Please enter Physical Exam Duration Period";
                }
            }
        });

        return Message;


    },

    //Author: Muhammad Arshad
    //Date: 04-03-2016
    //This function will load PhysicalExam templates
    loadPhysicalExamTemplate: function (providerId, specialtyId) {
        var objData = new Object();
        objData["commandType"] = "load_physcialexam_default_template";
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["ProviderIds"] = providerId;
        objData["SpecialtyIds"] = specialtyId;
        objData["TemplateId"] = 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle load of PhysicalExam and it's childs as specified by PhysicalExamType
    //It represents service call to API
    fillPhysicalExam: function (PhysicalExamType, physicalExamId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["PhysicalExamType"] = PhysicalExamType != null ? PhysicalExamType : "tobacco";
        objData["PatientPhysicalExamId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();
        objData["NotesId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val() == "" ? PhysicalExamDataTemplateDetail.params.NotesId : $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val();
        objData["commandType"] = "FILL_PatientPhysicalExam";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    fillPhysicalExamForSoap: function (physicalExamId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["PatientPhysicalExamId"] = physicalExamId != null ? physicalExamId : $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();
        // objData["NotesId"] = Clinical_Notes.params.NotesId != null ? Clinical_Notes.params.NotesId : ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val() == "" ? PhysicalExamDataTemplateDetail.params.NotesId : $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val());
        objData["commandType"] = "FILL_PatientPhysicalExam";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Add of PhysicalExam
    //It represents service call to API
    savePhysicalExam: function (PhysicalExamData) {
        var objData = JSON.parse(PhysicalExamData);
        if (PhysicalExamDataTemplateDetail.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = PhysicalExamDataTemplateDetail.params.patientID;
        }
        objData["TemplateId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        objData["NotesId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val();

        objData["commandType"] = "SAVE_PatientPhysicalExam";

        var data = JSON.stringify(objData);

        ////var data = "PhysicalExamignsData=" + PhysicalExamignsData;
        //// serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //Author : Abid Ali
    //Date: 08-02-2015
    //This function will handle Edit of PhysicalExam
    //It represents service call to API
    updatePhysicalExam: function (PhysicalExamData, PhysicalExamId) {

        var objData = JSON.parse(PhysicalExamData);
        if (PhysicalExamDataTemplateDetail.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = PhysicalExamDataTemplateDetail.params.patientID;
        }
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        objData["TemplateId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        objData["NotesId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val();
        objData["commandType"] = "UPDATE_patientphysicalexam";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");

    },

    //Author : Muhammad Arshad
    //Date: 15-02-2016
    //This function will handle Insert/Update of Detail for Characteristic/SubCharacteristic
    //It represents service call to API
    saveDetailforCharacteristic: function (PhysicalExamData, PhysicalExamId) {

        var objData = JSON.parse(PhysicalExamData);
        if (PhysicalExamDataTemplateDetail.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = PhysicalExamDataTemplateDetail.params.patientID;
        }
        objData["PatientPhysicalExamId"] = PhysicalExamId;


        if (PhysicalExamId > 0)
            objData["commandType"] = "UPDATE_DetailforCharacteristic";
        else
            objData["commandType"] = "SAVE_DetailforCharacteristic";



        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");

    },

    //Author : Muhammad Arshad
    //Date: 15-02-2016
    //This function will handle Insert/Update of Detail for SubCharacteristic
    //It represents service call to API
    saveDetailforSubCharacteristic: function (PhysicalExamData, PhysicalExamId) {

        var objData = JSON.parse(PhysicalExamData);
        if (PhysicalExamDataTemplateDetail.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = PhysicalExamDataTemplateDetail.params.patientID;
        }
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        if (PhysicalExamId > 0)
            objData["commandType"] = "UPDATE_DetailforCharacteristic";
        else
            objData["commandType"] = "SAVE_DetailforCharacteristic";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");

    },

    //Author : Ahmad Raza
    //Date: 18-02-2016
    //This function will handle saving of Normal system's inner detail
    //It represents service call to API
    saveDetailforNormalSystem: function (PhysicalExamData, PhysicalExamId) {

        var objData = JSON.parse(PhysicalExamData);
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        //Start Farooq Ahmad 01/03/2016 Set the NotesId in Post Data
        objData["NotesId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val() == "" ? PhysicalExamDataTemplateDetail.params.NotesId : $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val();
        //End Farooq Ahmad 01/03/2016 Set the NotesId in Post Data
        if (PhysicalExamId > 0)
            objData["commandType"] = "UPDATE_SYSTEM_NORMAL_DETAIL";
        else
            objData["commandType"] = "SAVE_SYSTEM_NORMAL_DETAIL";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");

    },

    //Author : Ahmad Raza
    //Date: 18-02-2016
    //This function will handle Delete system detail
    deleteSystemDetail: function (parentType, parentId, physicalExamId, itemId) {


    },

    //Author : Ahmad Raza
    //Date: 18-02-2016
    //This function will call DB to delete Characteristics detail with prompt
    deleteCharacteristicDetail: function (parentType, sectionId, physicalExamId, charId) {

        //   var characteristicName = $("#ulExamCharacteristics > li#" + charId).find('label').text();
        //   utility.myConfirm('This will reset all values in ' + characteristicName + ' Characteristic and clear all information. This action is irreversible and cannot be undone. Would you like to proceed? ', function () {

        PhysicalExamDataTemplateDetail.deleteCharacteristicDetailDBCall(parentType, sectionId, physicalExamId, charId).done(function (response) {
            //   response = JSON.parse(response);
            if (response == "Successfully Deleted") {
                //       utility.DisplayMessages(response, 1);
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics > li#" + charId).find('input').attr('checked', false);
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics > li#" + charId).removeClass('green');
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics").find('li').attr('parentid', charId).find('input').attr('checked', false);
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics").find('li').attr('parentid', charId).removeClass('green');
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics > li#" + charId).removeData();
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection > li#" + sectionId).removeData();
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics  li").removeClass('green').removeClass('active');
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics ").addClass('hidden');

                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics > li#" + charId).find('#txtCharacteristicDetail' + charId).val('');
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics > li#" + charId).find('#btnOpenDetail' + charId).removeClass('green');
                $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics > li#" + charId).find('#btnOpenDetail' + charId).parent().find("div#divDetail" + charId).addClass('hidden');
                //End 26-02-2016 Edit By Humaira Yousaf Bug# 394
                // PhysicalExamDataTemplateDetail.deleteDomObjectFromList('#ulPhysicalExamSystemSection', sectionId);
                //PhysicalExamDataTemplateDetail.deleteDomObjectFromList('#ulExamCharacteristics', charId);
                //   PhysicalExamDataTemplateDetail.deleteFromJsonObject('ulExamCharacteristics', charId);
            }
            else {
                utility.DisplayMessages(response, 3);
            }
        });


        //  }, function () {

        //  }, "Confirm Delete");
    },

    //Author : Ahmad Raza
    //Date: 18-02-2016
    //This function will call DB to delete Characteristics detail
    deleteCharacteristicDetailDBCall: function (parentType, sectionId, physicalExamId, charId) {

        if (!PhysicalExamDataTemplateDetail.isNullOrUndefined(sectionId)) {
            var objData = new Object();
            objData["patientId"] = $('#PatientProfile #hfPatientId').val();
            objData["patientPhysicaExamlId"] = physicalExamId;
            objData["sectionId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection li#" + sectionId).data("SystemCharacteristicsIds_" + sectionId).Id;
            objData["characteristicId"] = charId;
            objData["commandType"] = "delete_Characteristics";
            var data = JSON.stringify(objData);
            return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
        }

    },

    //Author : Abid Ali
    //Date: 15-03-2016
    //This function will delete SubCharacteristics detail from json and Dom
    deleteSubCharacteristicDetail: function (parentType, charId, physicalExamId, subCharPKId, subCharLiId) {

        //Delete corresponding char Li object
        var subCharData = PhysicalExamDataTemplateDetail.getObjectOfClickedElement('#ulExamSubCharacteristics', subCharLiId);

        if (!PhysicalExamDataTemplateDetail.isNullOrUndefined(subCharData)) {
            PhysicalExamDataTemplateDetail.deleteFromJsonObject('ulExamSubCharacteristics', subCharLiId)
            PhysicalExamDataTemplateDetail.deleteDomObjectFromList('#ulExamSubCharacteristics', subCharLiId);
        }

    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Unload of PhysicalExam Tab
    unLoadTab: function (nextOrPre, controlToInvoke) {
        //Code for progress note navigation
        // Date: 12-11-2015
        // Change Author: Muhammad Azhar Shahzad
        // If User OPen Family History From Progress Note
        /*Clicking close “X” button, a prompt message will be displayed
            “Are you want to save the changes?
            The date will be modified with current date.”
            i.	Clicking yes from the prompt will update the date as well as add the Family history component on the progress notes.
            ii.	Clicking No will close the Family history popup and will not add Family history component on Progress notes.
        */
        PhysicalExamDataTemplateDetail.DataJSON = [];
        PhysicalExamDataTemplateDetail.DataJSON = [];
        var detailExists = PhysicalExamDataTemplateDetail.isDetailExists();
        if (PhysicalExamDataTemplateDetail.params.ParentCtrl == "clinicalTabProgressNote") {
            PhysicalExamDataTemplateDetail.controlToInvoke = controlToInvoke;
            if (detailExists == true || $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #txtComments").val() != "") {
                utility.myConfirm('19', function () {
                    var PhysicalExamType = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + "  #hfPhysicalExamType").val();
                    if (typeof PhysicalExamType == "undefined" || PhysicalExamType == "") {
                        PhysicalExamDataTemplateDetail.UnloadPhysicalExam(nextOrPre);
                    }
                    else {

                        PhysicalExamDataTemplateDetail.bNextPrev = true;

                        PhysicalExamDataTemplateDetail.PhysicalExamSave(PhysicalExamType, true);
                    }
                }, function () {
                    PhysicalExamDataTemplateDetail.UnloadPhysicalExam(nextOrPre);
                },
               '1'
               );
            }
            else {
                PhysicalExamDataTemplateDetail.UnloadPhysicalExam(nextOrPre);
            }
        } else {

            PhysicalExamDataTemplateDetail.UnloadPhysicalExam();
        }
    },

    UnloadPhysicalExam: function (nextOrPre) {

        PhysicalExamDataTemplateDetail.selectedData = null;
        //Start Farooq Ahmad 18/02/2016 Empty Cache Array on Unload
        PhysicalExamDataTemplateDetail.myArr = [];
        PhysicalExamDataTemplateDetail.ExamDetails = {};
        PhysicalExamDataTemplateDetail.SectionNormalInfo = [];

        PhysicalExamDataTemplateDetail.selectedcharacteristicsIds = [];
        PhysicalExamDataTemplateDetail.characteristicsWithData = [];
        PhysicalExamDataTemplateDetail.selectedsubcharacteristicsIds = [];
        PhysicalExamDataTemplateDetail.subcharacteristicsWithData = [];
        //End Farooq Ahmad 18/02/2016 Empty Cach Array On Unload

        if (PhysicalExamDataTemplateDetail.params["FromAdmin"] == "0") {
            if (PhysicalExamDataTemplateDetail.params != null && PhysicalExamDataTemplateDetail.params.ParentCtrl != null) {

                if (PhysicalExamDataTemplateDetail.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                    UnloadActionPan(PhysicalExamDataTemplateDetail.params["ParentCtrl"], "PhysicalExamDataTemplateDetail");
                    if (PhysicalExamDataTemplateDetail.controlToInvoke != null) {
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(PhysicalExamDataTemplateDetail.controlToInvoke.replace(/\s/g, ''));
                            PhysicalExamDataTemplateDetail.controlToInvoke = null;
                        }, 400);
                    }
                }
                else {
                    UnloadActionPan(PhysicalExamDataTemplateDetail.params["ParentCtrl"], "PhysicalExamDataTemplateDetail");
                    if (PhysicalExamDataTemplateDetail.controlToInvoke != null)
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(PhysicalExamDataTemplateDetail.controlToInvoke);
                            PhysicalExamDataTemplateDetail.controlToInvoke = null;
                        }, 400);
                }

            }
            else
                UnloadActionPan(null, 'PhysicalExamDataTemplateDetail');
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_PhysicalExam").remove();
            RemoveAdminTab();
        }

        PhysicalExamDataTemplateDetail.SectionNormalInfo = [];
        EMRUtility.scrollToPNcomponent('PhysicalExamDataTemplateDetail');
    },

    //-----------------Progress Note-------------
    // added on Dec 14,2015 by Muhammad Azhar Shahzad
    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Call Back function to add component to Progress Note
    addPhysicalExamToNotes: function () {
        var PhysicalExamId = PhysicalExamDataTemplateDetail.params.PhysicalExamId;
        var PhysicalExamType = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamType").val();

        PhysicalExamDataTemplateDetail.PhysicalExamSave(PhysicalExamType, true);
    },

    //this function will get Family History Soap Text and attach that to Progress note
    getPhysicalExamInfo: function (PhysicalExamType, UnloadPhysicalExam, physicalExamId) {
        PhysicalExamDataTemplateDetail.fillPhysicalExamForSoap(physicalExamId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    PhysicalExamDataTemplateDetail.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadPhysicalExam);

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //Function name: getPhysicalExamSoapInfo
    //Author: Ahmad Raza
    //Date: 11-04-2016
    //Description: This function will handle load of PhysicalExam soap text
    getPhysicalExamSoapInfo: function (physicalExamId) {
        if (physicalExamId == null || physicalExamId == '') {
            return false;
        }
        PhysicalExamDataTemplateDetail.get_PhysicalExam_ForSOAP_DBCall(physicalExamId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                PhysicalExamDataTemplateDetail.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', physicalExamId);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    //Function name: get_PhysicalExam_ForSOAP_DBCall
    //Author: Ahmad Raza
    //Date: 11-04-2016
    //Description: This function will handle DB CAll for load of PhysicalExam soap text
    get_PhysicalExam_ForSOAP_DBCall: function (physicalExamId) {
        var objData = new Object();
        objData["PatientPhysicalExamId"] = physicalExamId;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "get_physicalexam_for_soap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },
    //Function name:loadTemplatePhysicalExam
    //Author: Ahmad Raza
    //Date: 11-04-2016
    //This function will handle load of PhysicalExam tamplate
    loadTemplatePhysicalExam: function () {

        PhysicalExamDataTemplateDetail.fillPhysicalExam("tobacco").done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                var ExamDetail = JSON.parse(response.PatientPhysicalExamFill_JSON);

                if (response.status != false) {
                    PhysicalExamDataTemplateDetail.isFirstLoad = true;
                    PhysicalExamDataTemplateDetail.loadTemplateBasedPhysicalExam(ExamDetail.TemplateId, "Edit");


                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
            PhysicalExamDataTemplateDetail.showPhysExamTemplatesTab();
        });

    },

    //This Function will check, if Family History Soap is already attached in Progress note, if Family History is not attached than it will create main divs to attach allergy
    checkPhysicalExamExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML PhysicalExamDataTemplateDetail').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="PhysicalExamComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<PhysicalExamDataTemplateDetail title="Physical Exam"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'PhysicalExam\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="PhysicalExam">Physical Exam</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'PhysicalExam\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</PhysicalExamDataTemplateDetail> </header></li>');
            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createPhysicalExamBodyHTML: function (response, NoteHTMLCtrl, UnloadPhysicalExam) {
        PhysicalExamDataTemplateDetail.checkPhysicalExamExists();
        if (response.PatientPhysicalExamFill_JSON != null & response.PatientPhysicalExamFill_JSON != '') {
            var PhysicalExamFill_Obj = JSON.parse(response.PatientPhysicalExamFill_JSON);
            var $mainDivPhysicalExam = $(document.createElement('div'));

            var PhysicalExamId = PhysicalExamFill_Obj.physicalExamId;
            if (PhysicalExamId > 0) {
                var $SectionBodyPhysicalExam = $(document.createElement('section'));
                $SectionBodyPhysicalExam.attr('id', "Cli_PhysicalExam_Main" + PhysicalExamId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_PhysicalExam_" + PhysicalExamId);
                var $ListPhysicalExam = $(document.createElement('ul'));

                $ListPhysicalExam.attr('class', 'list-unstyled')

                $SectionBodyPhysicalExam.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_PhysicalExam_" + PhysicalExamId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_PhysicalExam_Main" + PhysicalExamId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListPhysicalExam.append("<li>" + PhysicalExamFill_Obj.physicalExamSoapText + "</li>");
                $DetailsDiv.append($ListPhysicalExam);
                $SectionBodyPhysicalExam.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' PhysicalExamDataTemplateDetail').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId).length == 0) {
                    $mainDivPhysicalExam.append($SectionBodyPhysicalExam);
                    PhysicalExamDataTemplateDetail.updatePhysicalExamHtml($mainDivPhysicalExam.html(), PhysicalExamId, NoteHTMLCtrl);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' PhysicalExamDataTemplateDetail').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' PhysicalExamDataTemplateDetail').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' PhysicalExamDataTemplateDetail').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId).html($SectionBodyPhysicalExam.html());
                    $(NoteHTMLCtrl + ' PhysicalExamDataTemplateDetail').parent().parent().find('#Cli_PhysicalExam_Main' + PhysicalExamId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.updateProgressNoteHTML();
                    PhysicalExamDataTemplateDetail.updatePhysicalExamHtml("", PhysicalExamId, NoteHTMLCtrl);

                }

                if (UnloadPhysicalExam == true) {
                    PhysicalExamDataTemplateDetail.UnloadPhysicalExam(PhysicalExamDataTemplateDetail.bNextPrev);
                }
            }
        }
    },

    IsNullReturnSoapValue: function (SoapValue) {
        return (SoapValue == "") ? "" : SoapValue + ",";
    },

    // This Function is called by Progress Notes (Fill PhysicalExam Func, CopyAllNotesCategories)
    updatePhysicalExamHtml: function (PhysicalExamHtml, PhysicalExamId, NoteHTMLCtrl) {
        $(NoteHTMLCtrl + ' PhysicalExamDataTemplateDetail').parent().parent().addClass('initialVisitBody');
        if (PhysicalExamHtml != '') {
            $(NoteHTMLCtrl + ' PhysicalExamDataTemplateDetail').parent().parent().append(PhysicalExamHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (PhysicalExamHtml != '') {
            PhysicalExamDataTemplateDetail.AttachPhysicalExamFromNotes(PhysicalExamId);
        }

    },

    //This Function detach Family History From progress note
    detach_ComponentsPhysicalExam: function (ComponentName, IsUpdate, PhysicalExamComponentRemove) {
        var PhysicalExamDataTemplateDetailIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML PhysicalExamDataTemplateDetail').parent().parent().find('section[id*="Cli_PhysicalExam_Main"]').map(function () {
            return this.id.replace("Cli_PhysicalExam_Main", "");
        }).get().join(',');
        if (PhysicalExamDataTemplateDetailIds == "" || PhysicalExamDataTemplateDetailIds == "undefined") {
        }
        else {


            PhysicalExamDataTemplateDetail.DetachPhysicalExamFromNotes_DBCall(PhysicalExamDataTemplateDetailIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {

                        Clinical_ProgressNote.updateProgressNoteHTML();
                    }
                    //    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (PhysicalExamComponentRemove) {
            //Start//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Physical Exam']").remove();
            //End//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML PhysicalExamDataTemplateDetail').parent().parent().remove();
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML PhysicalExamDataTemplateDetail').parent().parent().find('section[id*="Cli_PhysicalExam_Main"]').remove();
        }
    },

    //This Functions ask for Detaching Family Hx from Progress Note for current Patient Selected
    detachPhysicalExamFromNotes: function (PhysicalExamId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('29', function () {
                var selectedValue = PhysicalExamId.replace('Cli_PhysicalExam_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    PhysicalExamDataTemplateDetail.DetachPhysicalExamFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + PhysicalExamId).remove();

                            Clinical_ProgressNote.updateProgressNoteHTML();
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () {
            },
                '29'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //This Functions attached Family Hx to Progress Note for current Patient Selected
    AttachPhysicalExamFromNotes: function (PhysicalExamId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            var selectedValue = PhysicalExamId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                PhysicalExamDataTemplateDetail.AttachPhysicalExamFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //If Attached PhysicalExam Made new inseration to PhysicalExam Table than good ids should be attached to HTML
                        Clinical_ProgressNote.updateProgressNoteHTML();
                        $('#' + PhysicalExamId).remove();
                        // utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }


        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //If PhysicalExam Component which is dropeed in Progress note has no PhysicalExam attached, than it will call for Latest PhysicalExam for this patient
    getLatestPhysicalExamByPatientId: function () {
        var strMessage = '';
        if (strMessage == "") {
            PhysicalExamDataTemplateDetail.getLatestPhysicalExamDataTemplateDetailByPatientId_DBCall().done(function (response) {
                //Start//02-03-2016//Ahmad Raza//Exception handled
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        PhysicalExamDataTemplateDetail.createPhysicalExamBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
                    }
                    else {
                        utility.DisplayMessages(strMessage, 3);
                    }
                    //End//02-03-2016//Ahmad Raza//Exception handled
                }

            });
        }
        else {
            utility.DisplayMessages(strMessage, 3);
        }
    },

    //-----Server calls of Notes----------
    DetachPhysicalExamFromNotes_DBCall: function (PhysicalExamId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PhysicalExamId"] = PhysicalExamId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "detach_PhysicalExam_from_notes";
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    AttachPhysicalExamFromNotes_DBCall: function (PhysicalExamId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["PhysicalExamId"] = PhysicalExamId;
        if (Clinical_ProgressNote.params.NotesVisitId == "" || Clinical_ProgressNote.params.NotesVisitId == "undefined") {
            objData["VisitId"] = 0;
        } else {
            if (Clinical_ProgressNote.params.NotesVisitId < 1) {
                objData["VisitId"] = $('#pnlClinicalProgressNote #hfVisitId').val();
            } else {
                objData["VisitId"] = Clinical_ProgressNote.params.NotesVisitId;
            }

        }
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["TemplateId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfPhysicalExamTemplateId').val();
        objData["commandType"] = "attach_PhysicalExam_with_notes";
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    getLatestPhysicalExamDataTemplateDetailByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["commandType"] = "getlatest_PhysicalExamby_patientid";
        objData["PatientPhysicalExamId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },
    // end Azhar Change on Dec 15,2015
    //--------------end progress Note-----------

    //Author: Muhammad Arshad
    //Date: 26-01-2016
    //This function will handle setting/calculating width of PhysicalExam

    toggleVerticalWidth: function (obj) {


        var panelChildrens = null;
        var currentPanel = null;
        var applyWidth = null;
        if (obj != null) {
            currentPanel = $(obj.delegateTarget).parent();
            panelChildrens = currentPanel.children("section.panel");
            applyWidth = currentPanel;
            PhysicalExamDataTemplateDetail.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
        }
        else {
            $('.toggleVertical').each(function (e) {
                currentPanel = $(this);
                panelChildrens = currentPanel.children().children("section.panel");
                applyWidth = currentPanel.children();
                PhysicalExamDataTemplateDetail.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
            });


        }
    },

    //Author: Muhammad Arshad
    //Date: 26-01-2016
    //This function will handle calculating width of PhysicalExam

    toggleVerticalWidthCtrl: function (currentPanel, panelChildrens, applyWidth) {
        var totalWidth = 0;
        var hidden = 0;
        //find total width of panels
        panelChildrens.each(function (e) {
            totalWidth += $(this).outerWidth(true);
        });
        //find total width of hidden panel
        currentPanel.find("section.hidden").each(function (e) {
            hidden += $(this).outerWidth(true);
        });
        //apply width to div
        applyWidth.width(((totalWidth - hidden) + 60) + "px");

    },

    //Function Name: openNormalSectionDetail
    //Author Name: Humaira Yousaf
    //Created Date: 09-02-2016
    //Description: Opens detail textbox of sections
    //Params: var objButton
    //Params: var detailParentId
    openNormalSectionDetail: function (objButton, detailParentId) {
        if (objButton != null) {
            var self = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail")
            var isMainNormalChecked = self.find("input[id='chkNormalSection']").prop("checked");
            if (isMainNormalChecked == true) {
                var NormalExamsDetailDiv = self.find("div#textAreaNormal");
                if (NormalExamsDetailDiv.hasClass("hidden") == true) {
                    NormalExamsDetailDiv.removeClass("hidden")
                    self.find('#txtNormalSectionDetail').focus();
                }
                else {
                    NormalExamsDetailDiv.addClass("hidden")
                    self.find('#txtNormalSectionDetail').focusout();
                }
            }
            else {
                utility.DisplayMessages("Please check Normal Checkbox to view details", 3);
            }
        }
    },
    //Function Name: cacheNormalSystem
    //Author Name: Humaira Yousaf
    //Created Date: 09-02-2016
    //Description: Temporarily saves systems ids which are set as normal
    cacheNormalSystem: function () {
        var systemId = $(PhysicalExamDataTemplateDetail.SelectedSystem).attr('id');
        var indexNormal = -1;
        if (PhysicalExamDataTemplateDetail.SectionNormalInfo != null && PhysicalExamDataTemplateDetail.SectionNormalInfo.length > 0) {
            $.grep(PhysicalExamDataTemplateDetail.SectionNormalInfo, function (item, index) {
                if (item == parseInt(systemId)) {
                    indexNormal = index;
                    return;
                }
            });
            if (indexNormal == -1) {
                PhysicalExamDataTemplateDetail.SectionNormalInfo.push(systemId);
            }
        }
        else {
            PhysicalExamDataTemplateDetail.SectionNormalInfo.push(systemId);
        }
    },

    //Function Name: cacheNormalSystem
    //Author Name: Humaira Yousaf
    //Created Date: 09-02-2016
    //Description: Removes system from temporarily saved normal systems
    removeNormalSystem: function (sysId) {
        var systemId = $(PhysicalExamDataTemplateDetail.SelectedSystem).attr('id');

        //Start//29-03-2016//Abid Ali//removing sysIds from SectionNormalInfo array
        if (!PhysicalExamDataTemplateDetail.isNullOrUndefined(sysId)) {
            systemId = sysId;
        }
        //End//29-03-2016//Abid Ali//removing systemIds from SectionNormalInfo array

        var indexToDelete = -1;
        if (PhysicalExamDataTemplateDetail.SectionNormalInfo != null && PhysicalExamDataTemplateDetail.SectionNormalInfo.length > 0) {
            $.grep(PhysicalExamDataTemplateDetail.SectionNormalInfo, function (item, index) {
                if (item == parseInt(systemId)) {
                    indexToDelete = index;
                    return;
                }
            });
            PhysicalExamDataTemplateDetail.SectionNormalInfo.splice(indexToDelete, 1);
        }
    },

    //Function Name: physicalExamDelete
    //Author Name: Humaira Yousaf
    //Created Date: 18-02-2016
    //Description: Deletes physical exam
    //Params: var systemId
    physicalExamDataTemplateDelete: function (systemId, isDialogue) {

        var systemName = $("#ulPhysicalExamSystems li#" + systemId).text();

        //utility.myConfirm('This will reset all values in ' + systemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed? ', function () {
        try {
            PhysicalExamDataTemplateDetail.DeleteSystemFromJSON(systemId, isDialogue);
        } catch (ex) {
            console.log(ex);
        }
        try {
            // PhysicalExamDataTemplateDetail.unCheckMainNormalCheckbox();
        } catch (ex) {
            console.log(ex);
        }
        try {
            PhysicalExamDataTemplateDetail.addGreenClasses();
        } catch (ex) {
            console.log(ex);
        }


        //}, function () {

        //},
        //'Confirm Delete'
        //);
    },


    //Author : Farooq Ahmad
    //Date : 15/06/2016
    //This Function will Delete the System From the JSON
    DeleteSystemFromJSON: function (systemId, isDialogue) {
        for (var index in PhysicalExamDataTemplateDetail.DataJSON) {
            if (PhysicalExamDataTemplateDetail.DataJSON[index].SystemId == systemId) {
                if (PhysicalExamDataTemplateDetail.DataJSON[index].PhysicalExamSystemId != null && parseInt(PhysicalExamDataTemplateDetail.DataJSON[index].PhysicalExamSystemId) > 0) {
                    var systemName = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + "#ulPhysicalExamSystems li#" + systemId).find('a').text();

                    isDialogue = isDialogue == null ? true : false;
                    if (isDialogue) {
                        utility.myConfirm('This will reset all values in ' + systemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed? ', function () {
                            PhysicalExamDataTemplateDetail.DataJSON.splice(index, 1);
                            PhysicalExamDataTemplateDetail.addGreenClasses();
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:not(:first)").removeClass("disableAll");
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li#" + systemId).removeClass("green");
                            $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #btnNormalSectionDetails").addClass("hidden");

                        }, function () { }, 'Confirm Delete');
                    }
                    else {
                        PhysicalExamDataTemplateDetail.DataJSON.splice(index, 1);
                        PhysicalExamDataTemplateDetail.addGreenClasses();
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:not(:first)").removeClass("disableAll");
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li#" + systemId).removeClass("green");
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #btnNormalSectionDetails").addClass("hidden");
                    }


                } else {
                    PhysicalExamDataTemplateDetail.DataJSON.splice(index, 1);
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystemSection li:not(:first)").removeClass("disableAll");
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulPhysicalExamSystems li#" + systemId).removeClass("green");
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #btnNormalSectionDetails").addClass("hidden");
                }
                break;
            }
        }
    },

    //Author: Abid Ali
    //Date: 29-03-2016
    //This function will uncheck main Normal checkbox and will clear normal details.
    unCheckMainNormalCheckbox: function () {

        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #chkPhysicalExamsNormal").prop('checked', false);
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #divNormalExams").find('button').find('i').removeClass('green').addClass('blue');
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #divNormalExams").find('#txtNormalExamsDetail').val('');
        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #divNormalExams").find('#divNormalExamsDetail').addClass('hidden');

    },

    deletePhysicalExam: function (systemId) {


        var objData = new Object();
        objData["patientId"] = $('#PatientProfile #hfPatientId').val();
        objData["patientPhysicaExamlId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();
        objData["systemId"] = systemId;
        objData["commandType"] = "delete_patientphysicalexam";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //Function Name: deleteSectionDetail
    //Author Name: Humaira Yousaf
    //Created Date: 19-02-2016
    //Description: Deletes physical exam system section
    //Params: var parentType
    //Params: var parentId
    //Params: var physicalExamId
    //Params: var itemId
    //Params: var caller
    deleteSectionDetail: function (parentType, parentId, physicalExamId, itemId, caller) {

        // Start Humaira Yousaf 23-02-2016 to delete normal system data
        if (caller == 'markAsNormal') {
            PhysicalExamDataTemplateDetail.deletePhysicalExamSystemSection(parentId, physicalExamId, itemId).done(function (response) {
                if (response == "Successfully Deleted") {
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li").removeClass('green').removeClass('active');
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li").find('input:checkbox:checked').prop('checked', false);
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection li#" + itemId).removeClass('green').removeClass('active');

                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li").find('input:checkbox:checked').prop('checked', false);
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics  li").removeClass('green').removeClass('active');
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics ").addClass('hidden');
                    $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SectionCharacteristics").addClass('hidden');

                    utility.DisplayMessages(response, 1);
                }
                else {
                    utility.DisplayMessages(response, 3);
                }

            });
        }
        else {

            var sectionName = $("#ulPhysicalExamSystemSection > li.active").find('a').text();
            utility.myConfirm('This will reset all values in ' + sectionName + ' section and clear all information. This action is irreversible and cannot be undone. Would you like to proceed? ', function () {
                PhysicalExamDataTemplateDetail.deletePhysicalExamSystemSection(parentId, physicalExamId, itemId).done(function (response) {
                    if (response == "Successfully Deleted") {
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li").removeClass('green').removeClass('active');
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamCharacteristics li").find('input:checkbox:checked').prop('checked', false);
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystemSection li#" + itemId).removeClass('green').removeClass('active');

                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics li").find('input:checkbox:checked').prop('checked', false);
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #ulExamSubCharacteristics  li").removeClass('green').removeClass('active');
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#CharacteristicsSubCharacteristics ").addClass('hidden');
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#SectionCharacteristics").addClass('hidden');

                        utility.DisplayMessages(response, 1);
                    }
                    else {
                        utility.DisplayMessages(response, 3);
                    }

                });
            }, function () {

            }, "Confirm Delete");
        }
        // End Humaira Yousaf 23-02-2016 to delete normal system data
        // return dfd.promise();
    },

    //Function Name: deletePhysicalExamSystemSection
    //Author Name: Humaira Yousaf
    //Created Date: 19-02-2016
    //Description: Deletes physical exam system section
    //Params: var systemId
    //Params: var physicalExamId
    //Params: var sectionId
    deletePhysicalExamSystemSection: function (systemId, physicalExamId, sectionId) {

        var objData = new Object();
        objData["systemId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail section#sectionPhysicalExam div#PhysicalExam #ulPhysicalExamSystems li#" + systemId).data("SystemSectionIds_" + systemId).Id;
        objData["patientPhysicaExamlId"] = physicalExamId;
        objData["sectionId"] = sectionId;
        objData["commandType"] = "delete_patientphysicalexamsystemsection";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //Begin 26-02-2016 Added By Humaira Yousaf Bug# 377

    //Function Name: saveExamNormalDetail
    //Author Name: Humaira Yousaf
    //Created Date: 26-02-2016
    //Description: Saves normal exam detail
    //Params: var obj
    saveExamNormalDetail: function (obj) {
        var PhysicalExamDateExists = false;
        if ($('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #dtPhysicalExamDate").val() != null && $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #frmPhysicalExamDataTemplateDetail #dtPhysicalExamDate").val() != '') {
            PhysicalExamDateExists = true;
        }
        if (PhysicalExamDateExists) {
            var currentParent = $(obj).parent();
            var currentLiObject = $(obj).parent().parent().parent().parent();

            var PhysicalExamId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val() != "" ? $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val() : "-1";
            var comments = $("#txtNormalExamsDetail").val();
            if (comments != "") {
                var myJSON = "{}";
                var objData = JSON.parse(myJSON);
                objData["Comments"] = comments;
                var myJSON = JSON.stringify(objData);

                PhysicalExamDataTemplateDetail.saveDetailforNormalExam(myJSON, PhysicalExamId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        //Start Farooq Ahmad 01/03/2016 Set the Physical Exam Id to Hidden Control
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val(response.PhysicalExamId);
                        //End Farooq Ahmad 01/03/2016 Set the Physical Exam Id to Hidden Control
                        //Begin 28-03-2016 Edit By Humaira Yousaf Bug# EMR-452
                        $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #btnNormalExamDetails").find('i').removeClass('blue')//.addClass('green');
                        //End 28-03-2016 Edit By Humaira Yousaf Bug# EMR-452
                        utility.DisplayMessages(response.message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });


            }
            else {
                utility.DisplayMessages("Please enter detail", 3);
            }
        }
        else {
            utility.DisplayMessages("Please enter Date.", 3);
        }
    },
    //Function Name: saveDetailforNormalExam
    //Author Name: Humaira Yousaf
    //Created Date: 26-02-2016
    //Description: Saves normal exam detail
    //Params: var PhysicalExamData
    //Params: var PhysicalExamId
    saveDetailforNormalExam: function (PhysicalExamData, PhysicalExamId) {

        var objData = JSON.parse(PhysicalExamData);
        if (PhysicalExamDataTemplateDetail.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = PhysicalExamDataTemplateDetail.params.patientID;
        }
        //Start Farooq Ahmad 01/03/2016 Set the Note Id Value
        objData["NotesId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val() == "" ? PhysicalExamDataTemplateDetail.params.NotesId : $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val();
        //End Farooq Ahmad 01/03/2016 Set the Note Id Value
        objData["PatientPhysicalExamId"] = PhysicalExamId;
        if (PhysicalExamId > 0)
            objData["commandType"] = "update_detailforexam";
        else
            objData["commandType"] = "save_detailforexam";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },
    //End 26-02-2016 Added By Humaira Yousaf Bug# 377


    //End//======== For Detail book Icon

    //Start//------------- Db Calls ---------------

    //Author: Abid Ali
    //Date: 13-06-2016
    //Description: This function will load the physical Exam templates
    PhysicalExamTemplateLoad: function () {
        var objData = new Object();
        objData["commandType"] = "load_physcialexam_default_template";
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["TemplateId"] = 0;
        objData["IsActive"] = 1;//PhysicalExamDataTemplate.Switch;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamTemplate", "PhysicalExamTemplate");
    },

    //Author: Abid Ali
    //Date: 14-06-2016
    //This function will handle load of PhysicalExam and it's childs as specified by PhysicalExamType
    //It represents service call to API
    fillPhysicalExam: function (PhysicalExamType, physicalExamId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["PhysicalExamType"] = PhysicalExamType != null ? PhysicalExamType : "tobacco";
        objData["PatientPhysicalExamId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfPhysicalExamId").val();
        objData["NotesId"] = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val() == "" ? PhysicalExamDataTemplateDetail.params.NotesId : $('#' + PhysicalExamDataTemplateDetail.params.PanelID + " #hfNotesId").val();
        objData["commandType"] = "FILL_PatientPhysicalExam";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExam", "PhysicalExam");
    },

    //Author: Abid Ali
    //Date: 14-06-2016
    //This function will call Db to load physical Exam Data Template
    fillPhysicalExamDataTemplate: function () {
        var objData = new Object();
        objData["commandType"] = "Fill_PhysExam_DataTemplate";
        var templateId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfTemplateId').val();
        objData["TemplateId"] = templateId == "" ? -1 : templateId;
        var dataTemplateId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfDataTemplateId').val();
        objData["DataTemplateId"] = dataTemplateId == "" ? -1 : dataTemplateId;
        objData["IsActive"] = PhysicalExamDataTemplate.Switch;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamDataTemplate", "PhysicalExamDataTemplate");
    },
    //Author: Abid Ali
    //Date: 15-June-2016
    //This function will call will save physExamDataTemplate
    insertUpdatePhysExamDataTemplate_DBCall: function (jsonData) {
        var objData = JSON.parse(jsonData);
        objData["commandType"] = "add_update_PhysExam_DataTemplate";
        objData["Systems"] = PhysicalExamDataTemplateDetail.DataJSON;

        // var templateId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfTemplateId').val();
        // objData["TemplateId"] = templateId == "" ? -1 : templateId;
        // var dataTemplateId = $('#' + PhysicalExamDataTemplateDetail.params.PanelID + ' #hfDataTemplateId').val();
        // objData["DataTemplateId"] = dataTemplateId == "" ? -1 : dataTemplateId;

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PhysicalExamDataTemplate", "PhysicalExamDataTemplate");
    },
    //End//------------- Db Calls ---------------
}
