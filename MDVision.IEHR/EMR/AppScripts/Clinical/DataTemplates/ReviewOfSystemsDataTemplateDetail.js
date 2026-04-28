ReviewOfSystemsDataTemplateDetail = {

    //Author: ZeeshanAK
    //Date: 30-03-2016
    //This file will handle all actions performed for Review Of Systems Data Template Detail
    bIsFirstLoad: true,
    params: [],
    CharacteristicsInfo: [],
    CharacteristicsDetailsInfo: [],


    //specialityCheckedIds: [],
    //providerCheckedIds: [],
    //SpecialtyIds: '',
    //ProviderIds: '',
    numOfCharsChecked: 0,
    CharcSystemInfo: [],

    Load: function (params) {
        ReviewOfSystemsDataTemplateDetail.CharacteristicsInfo = [];
        ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo = [];

        ReviewOfSystemsDataTemplateDetail.CharcSystemInfo = [];
        ReviewOfSystemsDataTemplateDetail.params = params;
        if (ReviewOfSystemsDataTemplateDetail.params.PanelID != 'pnlClinicalReviewofSystemsDataTemplateDetail') {
            ReviewOfSystemsDataTemplateDetail.params.PanelID = ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #pnlClinicalReviewofSystemsDataTemplateDetail';
        } else {
            ReviewOfSystemsDataTemplateDetail.params.PanelID = 'pnlClinicalReviewofSystemsDataTemplateDetail';
        }
        if (ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId > 0 && ReviewOfSystemsDataTemplateDetail.params.ROSTemplateId > 0) {
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #btnSaveAs').show();
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #btnReset').hide();
        } else {
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #btnSaveAs').hide();
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #btnReset').hide();
        }
        //if (ReviewOfSystemsDataTemplateDetail.bIsFirstLoad) {
        //    ReviewOfSystemsDataTemplateDetail.bIsFirstLoad = false;
        //    ReviewOfSystemsDataTemplateDetail.validateROSTemplate();
        //var self = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID);
        //if (ReviewOfSystemsDataTemplateDetail.isSuperAdmin()) {
        //    self = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #entityDDL');
        //}
        // self.loadDropDowns(true).done(function () {
        //ReviewOfSystemsDataTemplateDetail.IntializeMultiSelectDropDown();
        // ReviewOfSystemsDataTemplateDetail.domReadyFunction();
        //if (ReviewOfSystemsDataTemplateDetail.isSuperAdmin()) {
        //    ReviewOfSystemsDataTemplateDetail.enableDisableDropDownLists('ddlSpecialty,ddlProvider', true);
        //} else {
        //    ReviewOfSystemsDataTemplateDetail.enableDisableDropDownLists('ddlSpecialty,ddlProvider', false);
        //}

        if (ReviewOfSystemsDataTemplateDetail.params["mode"] == "Edit" && ReviewOfSystemsDataTemplateDetail.params.ROSTemplateId > 0) {
            // if (ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId == null) {
            //  ReviewOfSystemsDataTemplateDetail.loadROSSystemInfo();
            //} else {
            //    ReviewOfSystemsDataTemplateDetail.loadROSDataTempInfo();
            //}
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #btnSaveAs').show();
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' .modal-title').first().html("Edit ROS Template");
        }
        //else {
        //        $.when(ReviewOfSystemsDataTemplateDetail.isEntitySelected(ReviewOfSystemsDataTemplateDetail.isSuperAdmin())).then(function () {
        //            ReviewOfSystemsDataTemplateDetail.loadSystems(false);
        //            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #btnSaveAs').hide();
        //        });
        //    }


        //});
        var self = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {

        });
        ReviewOfSystemsDataTemplateDetail.loadROSSystemInfo();


        //ReviewOfSystemsDataTemplateDetail.loadTemplateSystems(null);
        ReviewOfSystemsDataTemplateDetail.validateROSDataTemplate();
        ReviewOfSystemsDataTemplateDetail.hideFooterButtons(true);
        ReviewOfSystemsDataTemplateDetail.domReadyFunction();

    },

    //Author: Muhammad Azhar Shahzad
    //Date: January 26, 2016
    //This function will handle Initialization of KeyPad control
    domReadyFunction: function () {
        $(function () {
            setTimeout(function () { countWidth(); }, 405);
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " .toggleVertical div.toggle").click(function (e) {
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
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail').on('change', function () {
                $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val('true');
            });


        });

        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divROSDataTemplate #Duration').keyboard({
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
                } else {
                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divROSDataTemplate #Duration').focus();

                }
            },
            layout: 'custom',
            reposition: true,
            appendLocally: this,
            restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
            preventPaste: true,  // prevent ctrl-v and right click
            usePreview: false,
            autoAccept: true,
            tabNavigation: true//,
            //beforeClose: function (e, keyboard, el, accepted) {
            //    var obj = el;
            //    setTimeout(function () {
            //        ReviewOfSystemsDataTemplateDetail.checkDurationDetails($(obj));
            //    }, 10);
            //}
        })
         .addTyping()
         .keydown(function (e) {
             var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
             if (keyCode == 9) {
                 $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divROSDataTemplate #Duration').next().hide();
                 $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divROSDataTemplate #divExamCharacteristics #ROSCharacteristicsDetailDurationId').focus();
                 $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divROSDataTemplate #Duration').removeClass('ui-keyboard-input-current');
             }
         });
    },

    /* Load ROS System info for the patient
      Author: ZeeshanAK | Date: February 03, 2016 */
    loadROSSystemInfo: function () {
        ReviewOfSystemsDataTemplateDetail.getROSPatientInfo_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var ROSPatientInfoJSON = response.ROSPatientInfo_JSON;
                if (ROSPatientInfoJSON.ROSTemplateId != null && !(ROSPatientInfoJSON.ROSTemplateId <= 0) && ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId != null) {
                    ReviewOfSystemsDataTemplateDetail.loadSystems(ROSPatientInfoJSON.ROSTemplateId, ROSPatientInfoJSON.ROSDataTemplateId);
                    ReviewOfSystemsDataTemplateDetail.showROSTemplatesTab('LiAddROS');
                    if (ROSPatientInfoJSON != null) {
                        var self = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail");
                        utility.bindMyJSONByName(true, ROSPatientInfoJSON, false, self).done(function () {
                            ReviewOfSystemsDataTemplateDetail.params.ROSSystemInfoID = ROSPatientInfoJSON.ROSSystemInfoID;
                            ReviewOfSystemsDataTemplateDetail.params.mode = "Edit";

                            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + '  #txtTemplateName').val(ROSPatientInfoJSON.ROSDataTempName);

                            if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + '  #dtReviewofSystemsDate').val() == '') {
                                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + '  #dtReviewofSystemsDate').datepicker("setDate", new Date());
                            } else {
                                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + '  #dtReviewofSystemsDate').datepicker("setDate", new Date($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + '  #dtReviewofSystemsDate').val()));
                            }
                            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #hfROSSystemInfoID').val(ROSPatientInfoJSON.ROSSystemInfoID);

                            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #txtROSComments').val(ROSPatientInfoJSON.ROSComments);
                        });
                        $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val('');
                    }
                } else {
                    ReviewOfSystemsDataTemplateDetail.loadTemplateSystems(null);
                    //ReviewOfSystemsDataTemplateDetail.loadDataTemplateSystems(ROSPatientInfoJSON.ROSTemplateId);
                }


            }

        });
    },

    loadTemplateSystems: function (ROSTemplateId) {
        ReviewOfSystemsDataTemplateDetail.getROSTemplateSystems_DBCall(ROSTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ROSTemptCount > 0) {
                    var SystemsJSON = JSON.parse(response.ROSTempt_JSON);

                    $.each(SystemsJSON, function (index, item) {
                        var $row = $('<tr/>');
                        $row.attr("onclick", "utility.SelectGridRow($('#gvTemplates_row" + item.ROSTemplateId + "'));ReviewOfSystemsDataTemplateDetail.loadROSAgainstTemplate(" + item.ROSTemplateId + ");");
                        $row.attr("id", "gvTemplates_row" + item.ROSTemplateId);
                        $row.append('<td style="display:none;">' + item.ROSTemplateId + '</td><td><a class="btn  btn-xs" href="#" onclick="ReviewOfSystemsDataTemplateDetail.loadROSAgainstTemplate(' + item.ROSTemplateId + ');" title="Select Template"><i class="fa fa-check black"></i></a>&nbsp;</td><td>' + item.TemplateName + '</td>');
                        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #dgvClinicalNotesDataTemplates tbody').last().append($row);
                        //$('#dgvClinicalNotesDataTemplates tbody').last().append($row);
                    });

                }

            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }

        });
    },


    loadROSAgainstTemplate: function (ROSTemplateId) {
        var objTabROSTemplates = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiROSTemplates');
        var objTabAddROS = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS');
        objTabROSTemplates.removeClass("active");
        objTabAddROS.removeClass("hidden");
        objTabAddROS.find("a").trigger("click");
        ReviewOfSystemsDataTemplateDetail.loadSystems(ROSTemplateId);
        ReviewOfSystemsDataTemplateDetail.hideFooterButtons(false);
    },

    // Hide the Add button and show ROS Templates tab
    //  Author: ZeeshanAK | Date: March 31, 2016
    showROSTemplatesTab: function (TabID) {

        if (TabID == 'LiROSTemplates' && $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #viewTemplates').is(':visible') == false) {
            if ($("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val() == 'true') {
                utility.myConfirm('Data has been modified. Are you sure you want to save the changes?', function () {
                    ReviewOfSystemsDataTemplateDetail.ReviewofSystemsSave(false).done(function () {
                        if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #dgvClinicalNotesDataTemplates tbody tr').length <= 0) {
                            ReviewOfSystemsDataTemplateDetail.loadTemplateSystems(null);
                        }
                        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS').removeClass("active");
                        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS').addClass('hidden')
                        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiROSTemplates a').tab('show');
                        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiROSTemplates').addClass("active");
                        $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val('');
                        ReviewOfSystemsDataTemplateDetail.hideFooterButtons(true);
                        $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #divReviewofSystemsSystemSection').html('');
                    });
                }, function () {

                    $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val('');
                    //ReviewOfSystemsDataTemplateDetail.clearInfoForSystemReset();
                    ReviewOfSystemsDataTemplateDetail.resetDOMcontrolsROS();
                    if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #dgvClinicalNotesDataTemplates tbody tr').length <= 0) {
                        ReviewOfSystemsDataTemplateDetail.loadTemplateSystems(null);

                    }
                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS').removeClass("active");
                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS').addClass('hidden')
                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiROSTemplates a').tab('show');
                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiROSTemplates').addClass("active");
                    ReviewOfSystemsDataTemplateDetail.hideFooterButtons(true);
                    $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #divReviewofSystemsSystemSection').html('');
                },
               'Confirmation'
               );
            } else {
                // ReviewOfSystemsDataTemplateDetail.clearInfoForSystemReset();
                ReviewOfSystemsDataTemplateDetail.resetDOMcontrolsROS();
                if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #dgvClinicalNotesDataTemplates tbody tr').length <= 0) {
                    ReviewOfSystemsDataTemplateDetail.loadTemplateSystems(null);
                }
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS').removeClass("active");
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS').addClass('hidden')
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiROSTemplates a').tab('show');
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiROSTemplates').addClass("active");
                ReviewOfSystemsDataTemplateDetail.hideFooterButtons(true);
                $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #divReviewofSystemsSystemSection').html('');
            }
        } else if (TabID == 'LiAddROS' && $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #addROSSystem').is(':visible') == false) {
            // $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS').trigger("click");
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS a').tab('show');
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS').removeClass("hidden");
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiROSTemplates').removeClass("active");
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #LiAddROS').addClass("active");
            ReviewOfSystemsDataTemplateDetail.toggleVerticalWidth();
            ReviewOfSystemsDataTemplateDetail.hideFooterButtons(false);
        }
    },


    //This function will handle setting/calculating width of PhysicalExam
    //Author: ZeeshanAK | Date: March 09, 2016 
    toggleVerticalWidth: function (obj) {

        var panelChildrens = null;
        var currentPanel = null;
        var applyWidth = null;
        if (obj != null) {
            currentPanel = $(obj.delegateTarget).parent();
            panelChildrens = currentPanel.children("section.panel");
            applyWidth = currentPanel;
            ReviewOfSystemsDataTemplateDetail.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
        }
        else {
            $('.toggleVertical').each(function (e) {
                currentPanel = $(this);
                panelChildrens = currentPanel.children().children("section.panel");
                applyWidth = currentPanel.children();
                ReviewOfSystemsDataTemplateDetail.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
            });
        }
    },

    //This function will handle calculating width of PhysicalExam
    //Author: ZeeshanAK | Date: March 09, 2016 
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




    /* This Function will load all the Systems and add it as two column sortable list 
      Author: ZeeshanAK | Date: January 28, 2016 */
    loadSystems: function (ROSTemplateId, ROSDataTemplateId) {
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #hfROSTemplateId").val(ROSTemplateId);
        if (ROSDataTemplateId == null) {
            ROSDataTemplateId = '-1';
        }
        ReviewOfSystemsDataTemplateDetail.getROSSystems_DBCall(ROSTemplateId, ROSDataTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //ReviewOfSystemsDataTemplateDetail.createReviewofSystemsBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
                var $ListVital = $(document.createElement('ul'));
                $ListVital.addClass('listPrimaryPad listTwoColumn');
                var SystemsJSON = JSON.parse(response.Systems_JSON);

                $.each(SystemsJSON, function (index, element) {
                    var ROSSystemPatientID = element.ROSDataSystemId;
                    if (element.ROSSystemPatientID != null && element.ROSSystemPatientID != '') {
                        ROSSystemPatientID = element.ROSSystemPatientID;
                    }
                    if ((element.isCharacteristicsExists != null && element.isCharacteristicsExists != '') || (element.IsNormal != null && element.IsNormal != '' && element.IsNormal == "True")) {
                        $ListVital.append("<li  id='ROSSystem_" + element.Name + "_" + element.ROSSystemId + "' title='" + element.Name + "' class='green' onclick='ReviewOfSystemsDataTemplateDetail.GetCharacteristics(this," + element.ROSSystemId + ",\"" + element.Name + "\"," + ROSSystemPatientID + ");'><input type='hidden' id='isSystemNormal' name='isSystemNormal' value=\"" + element.IsNormal + "\" /><input type='hidden' id='systemNormalDescription' name='systemNormalDescription' value=\"" + element.SystemNormalDescription + "\" /><input type='hidden' id='systemPatientID' name='systemPatientID' value=\"" + ROSSystemPatientID + "\" /><input type='hidden' id='isCharacteristicsExists' name='isCharacteristicsExists' value=\"" + element.isCharacteristicsExists + "\" /><a  class='' ><span class='size85per ellipses pull-left btnEffect'>" + element.Name + "</span><span class='removeIconListHover' onclick='ReviewOfSystemsDataTemplateDetail.clearSectionInfo(event, this," + element.ROSSystemId + ",\"" + element.Name + "\");'><i class='fa fa-close'></i></span></a></li>");
                    } else {
                        $ListVital.append("<li  id='ROSSystem_" + element.Name + "_" + element.ROSSystemId + "' title='" + element.Name + "' class='' onclick='ReviewOfSystemsDataTemplateDetail.GetCharacteristics(this," + element.ROSSystemId + ",\"" + element.Name + "\"," + ROSSystemPatientID + ");'><input type='hidden' id='isSystemNormal' name='isSystemNormal' value=\"" + element.IsNormal + "\" /><input type='hidden' id='systemNormalDescription' name='systemNormalDescription' value=\"" + element.SystemNormalDescription + "\" /><input type='hidden' id='systemPatientID' name='systemPatientID' value=\"" + ROSSystemPatientID + "\" /><input type='hidden' id='isCharacteristicsExists' name='isCharacteristicsExists' value=\"" + element.isCharacteristicsExists + "\" /><a  class='' ><span class='size85per ellipses pull-left btnEffect'>" + element.Name + "</span><span class='removeIconListHover' onclick='ReviewOfSystemsDataTemplateDetail.clearSectionInfo(event, this," + element.ROSSystemId + ",\"" + element.Name + "\");'><i class='fa fa-close'></i></span></a></li>");
                    }

                    var isSystemNormal = element.IsNormal;
                    var systemNormalDescription = element.SystemNormalDescription;

                    var tempData = {};
                    tempData['ReviewofSystemsSectionNormal' + "_" + element.ROSSystemId] = isSystemNormal;
                    tempData['SystemNormalDescription' + "_" + element.ROSSystemId] = systemNormalDescription;
                    ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[element.Name] = JSON.stringify(tempData);
                });
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #hfROSTemplateId").val(ROSTemplateId);
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').html($ListVital[0].outerHTML);
                ReviewOfSystemsDataTemplateDetail.sortingInitializationSystem();
                //if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #chkReviewofSystemssNormal').is(':checked')) {
                //    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divReviewofSystemsSystems ul li').find('span:first-child').addClass("green");
                //    ReviewOfSystemsDataTemplateDetail.markAllSystemsAsNormalCache();
                //}
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    // This Function will make the Divs sortable 
    //  Author: ZeeshanAK | Date: March 31, 2016
    sortingInitializationSystem: function () {
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul").sortable({
            connectWith: '#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul",
            //  revert: true,
            placeholder: "ui-state-highlight",
            //helper: 'clone'
            stop: function (event, ui) {
                setTimeout(function () {
                    var sortedIdsInOrder = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray");
                    $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val('true');
                }
                    , 5);
            }
        });
    },

    // clearInfoForSystemReset
    //  Author: ZeeshanAK | Date: March 31, 2016
    clearInfoForSystemReset: function (obj, SystemID, SystemName) {
        $(obj).closest("li").removeClass('green');

        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' input[id^=Charc_Pos_' + SystemID + '],#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' input[id^=Charc_Neg_' + SystemID + '],' + '#' + ReviewOfSystemsDataTemplateDetail.params.PanelID +
            ' input[id=selectAllNegative_' + SystemID + '],#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + 'input[id=selectAllPositive_' + SystemID + ']').prop('checked', false);
        Clinical_ReviewofSystems.bookIconClassToggle($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' input[id^=Charc_Pos_' + SystemID + ']').closest('ul').find('[id=bookIcon]'), true);

        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' [id^=Charc_Pos_').parent().parent().removeClass('red');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' input[id^=Charc_Desc_]').val('');
        // $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' input[id^=selectAllPositive_]').prop('checked', false).parent().parent().removeClass('red')
        $(obj).parent().find('span:first-child').removeClass('green');
        var detailObj = $.grep(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo, function (item, index) {
            return (item.SystemId == SystemID);
        });
        ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo.splice(detailObj, 1);
        ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName] = null;
        $(obj).closest("li").find("#isSystemNormal").val('');
        $(obj).closest("li").find("#systemNormalDescription").val('');

    },
    resetDOMcontrolsROS: function () {
        ReviewOfSystemsDataTemplateDetail.handleDetailToggleDiv();
        ReviewOfSystemsDataTemplateDetail.checkUncheckPositiveNegative();

        var obj = ReviewOfSystemsDataTemplateDetail.CharcSystemInfo;
        for (var i in obj) {
            if (obj.hasOwnProperty(i)) {
                obj[i] = null;
            }
        }
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divROSDataTemplate .toggle').addClass('disableAll');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divReviewofSystemsSystems').find("#isSystemNormal").val('');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divReviewofSystemsSystems').find("#systemNormalDescription").val('');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divReviewofSystemsSystems').find('span:first-child').removeClass('green');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divReviewofSystemsSystemSection").find('input[type=checkbox]').prop('checked', false);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divReviewofSystemsSystemSection ul li").removeClass('active');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divReviewofSystemsSystemSection ul li a i').removeClass("green");
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divReviewofSystemsSystemSection ul li').find("input").removeAttr('disabled');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #hdnDescriptionNormal').val('');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #isSystemNormal').val('');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #systemNormalDescription').val('');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #bookIconNormal').removeClass("green");
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' input[id^=ReviewofSystemsSectionNormal]').prop('checked', false);
        //Start || 14 March, 2016 || ZeeshanAK || Change made for fixing EMR-455
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' input[id^=chkReviewofSystemssNormal]').prop('checked', false).siblings('a').addClass("disabled");
        //End   || 14 March, 2016 || ZeeshanAK || Change made for fixing EMR-455
        $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val('true');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divReviewofSystemsSystemSection ul').removeClass("disableAll");
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #bookIconNormal').addClass("disabled");
        //Start || 14 March, 2016 || ZeeshanAK || Change made for fixing EMR-405
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #txtSearchCharacteristics').parent().removeClass('disableAll');
        //End   || 14 March, 2016 || ZeeshanAK || Change made for fixing EMR-405
    },

    // Normalizes the cache for all of the systems
    //  Author: ZeeshanAK | Date: March 31, 2016
    //markAllSystemsAsNormalCache: function () {
    //    $.map($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
    //        var sName = n.split('_')[1];
    //        var sID = n.split('_')[2];

    //        ReviewOfSystemsDataTemplateDetail.markNormalSection(sName, sID, true);
    //    });
    //    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul li [name=isSystemNormal][type=hidden]").val("True");
    //    ReviewOfSystemsDataTemplateDetail.normalizeAllSectionsArray();
    //},

    // To normalize all sections in CharcSystemInfo
    //  Author: ZeeshanAK | Date: March 31, 2016
    normalizeAllSectionsArray: function () {
        $.map($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
            var sName = n.split('_')[1];
            var objData = JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName]);
            Object.keys(objData).forEach(function (key, index) {
                if (key.indexOf("Charc_Neg_") > -1 || key.indexOf('Charc_Pos_') > -1 || key.indexOf('Charc_Desc') > -1) {
                    objData[key] = '';
                } else if (key.indexOf("ReviewofSystemsSectionNormal") > -1) {
                    objData[key] = 'True';
                }
                // key: the name of the object key
                // index: the ordinal position of the key within the object
            });
            ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName] = JSON.stringify(objData);
        });
    },

    // This function will handle Unload of ReviewofSystems Tab
    //  Author: ZeeshanAK | Date: March 31, 2016
    unLoadTab: function () {
        if ($("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val() != '') {
            utility.myConfirm('Data has been modified. Are you sure you want to save the changes?', function () {
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail').submit();
            },
            function () {
                UnloadActionPan(ReviewOfSystemsDataTemplateDetail.params["ParentCtrl"], "ReviewOfSystemsDataTemplateDetail");
            },
            'Confirmation'
            );
        } else {
            UnloadActionPan(ReviewOfSystemsDataTemplateDetail.params["ParentCtrl"], "ReviewOfSystemsDataTemplateDetail");
        }
    },

    // This Function will load all the Characteristics for a specific System
    //  Author: ZeeshanAK | Date: March 31, 2016
    GetCharacteristics: function (obj, SystemID, SystemName, ROSSystemPatientID) {
        var charcSystemDiv = '#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystemSection';
        var SystemIsNormal = $(obj).find('[name=isSystemNormal][type=hidden]').val();
        var SystemNormalDescription = $(obj).find('[name=systemNormalDescription][type=hidden]').val();
        if (!$("divROSDataTemplate").find('.toggle').prev().hasClass('hidden')) {
            $("divROSDataTemplate").find('.toggle').prev().addClass('hidden');
            $("divROSDataTemplate").find('.toggle').removeClass('active');
        }
        if (!$("divROSDataTemplate").find('.toggle').hasClass('disableAll')) {
            ReviewOfSystemsDataTemplateDetail.CacheCharacteristicDetailInfo(true);
        }
        var CurrentSystem = $($(obj).parent()).find("li.active");
        if (CurrentSystem.text() != null && CurrentSystem.text() != '') {
            if (CurrentSystem.find('[name=isSystemNormal][type=hidden]').val() == "True") {
                ReviewOfSystemsDataTemplateDetail.CacheCharacteristicInfo(CurrentSystem, false, true, false);
            } else {
                ReviewOfSystemsDataTemplateDetail.CacheCharacteristicInfo(CurrentSystem, false, false, false);
            }
        } else {
            CurrentSystem = $(obj);
        }
        if (obj != null) {
            $($(obj).parent()).find("li").each(function () {
                $(this).removeClass("active");
            });
            if ($(obj).hasClass("active") == false) {
                $(obj).addClass("active");
            }
        }

        ReviewOfSystemsDataTemplateDetail.getROSSystemsCharacteristics_DBCall(SystemID, ROSSystemPatientID).done(function (response) {
            CurrentSystem = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #sectionReviewofSystems li.active");
            response = JSON.parse(response);
            if (response.status != false) {
                var HTMLSystemCharacteristics = "";
                var HTMLSelectAllCheckBox = "";
                var DivRow = $(document.createElement('div'));
                var SystemCharacteristicsJSON = JSON.parse(response.SystemCharacteristics_JSON);
                var GlobalChecked = "";
                var GlobalComments = "";
                var isSectionNormalBookEnabled = "";
                var isSelectAllPositiveChecked = " checked='checked'";
                var isSelectAllNegativeChecked = " checked='checked'";
                if (SystemCharacteristicsJSON != null && SystemCharacteristicsJSON.length > 0) {
                    if (SystemIsNormal == "True") {
                        GlobalChecked = " checked='checked'";
                        NormalDescriptions = SystemNormalDescription;
                    }
                } else {
                    isSelectAllPositiveChecked = '';
                    isSelectAllNegativeChecked = '';
                }
                if (GlobalChecked != null && GlobalChecked != "") {
                    isSectionNormalBookEnabled = "enabled blue";
                } else {
                    isSectionNormalBookEnabled = "disabled gray";
                }
                var divSearchBox = "<div class='col-md-3'><div class='input-group mb-default'><div class='input-group-addon'><i class='fa fa-search'></i></div><input type='text' id='txtSearchCharacteristics' onkeyup='ReviewOfSystemsDataTemplateDetail.getFilteredCharacteristics(this);' class='form-control' placeholder='Search Characteristics...' /><div class='input-group-btn'><a onClick='ReviewOfSystemsDataTemplateDetail.clearSearchBox(this);'class='btn btn-primary btn-xs pl-xs pr-xs'><i class='fa fa-times-circle-o'></i></a></div></div></div>";
                var divNormalCheckBox = "<div class='col-md-9' id='divNormal' systemname='" + SystemName + "'><a class='btn btn-xs pull-left " + isSectionNormalBookEnabled + "' id='bookIconNormal' href='javascript:void(0);'  onclick='ReviewOfSystemsDataTemplateDetail.showTextArea(this);'> <i class='fa fa-book'></i></a><input type='checkbox'class='mr-xs ml-xs pull-left' id='ReviewofSystemsSectionNormal_" + SystemID + "' onclick='ReviewOfSystemsDataTemplateDetail.markCurrentSectionAsNormal(this, \"" + SystemName + "\", \"" + SystemID + "\");' class='checkbox-custom' name='ReviewofSystemsSectionNormal' " + GlobalChecked + "/><label class='control-label'>Normal</label><input type='hidden' id='hdnDescriptionNormal' name='hdnDescriptionNormal' /></div>";
                DivRow.append(divSearchBox + divNormalCheckBox);
                var $ListVital = $(document.createElement('ul')).addClass("searchable");
                $ListVital.addClass('listPrimaryPad');
                $ListVital.attr("id", "ActiveSystemUL");

                $.each(SystemCharacteristicsJSON, function (index, element) {
                    var hdnDescription = "<input type='hidden' id=" + "Charc_Desc_" + SystemID + "_" + element.ROSSystemCharacteristicsId + " value='" + element.Description + "'>";
                    var IsPositive = " checked='checked'";
                    var IsNegative = " checked='checked'";
                    if (element.IsPositive == null || element.IsPositive == "") {
                        IsPositive = "";
                        IsNegative = "";
                        isSelectAllPositiveChecked = "";
                        isSelectAllNegativeChecked = "";
                    }
                    else if (element.IsPositive == "False") {
                        IsPositive = "";
                        isSelectAllPositiveChecked = "";
                    } else {
                        IsNegative = "";
                        isSelectAllNegativeChecked = "";
                    }
                    var hdnROSSystemPatientCharacteristicsID = "<input type='hidden' id=" + "SystemPatientCharacteristicsID_" + element.ROSSystemPatientCharacteristicsID + " name= 'SystemPatientCharacteristicsID' value='" + element.ROSSystemPatientCharacteristicsID + "'><input type='hidden' name='isCharcDetailExists' id='isCharcDetailExists' value='" + element.isCharcDetailExists + "'>";
                    HTMLSystemCharacteristics += "<li id='" + SystemName + "_" + SystemID + "_" + element.ROSSystemCharacteristicsId + "' name='" + SystemName + "_" + SystemID + "' systemname='" + SystemName + "' class='col-xs-4'  onClick='ReviewOfSystemsDataTemplateDetail.loadSysPatCharcDetail(this" + ((element.isCharcDetailExists != null && element.isCharcDetailExists != '') ? "," + element.isCharcDetailExists : "") + ");'>" +
                       "<div><a class='btn mt-xxs btn-xs pull-left disabled gray'  id='bookIcon' href='#' onclick='ReviewOfSystemsDataTemplateDetail.showTextArea(this);'> <i class='fa fa-book'></i></a>" +
                      "<div class='mt-xxs pull-left'> " +
                      "<div class='checkbox-icon checkbox-check mr-xs pull-left'>" +
                      "<input class='' type='checkbox' " + IsPositive + " onchange='ReviewOfSystemsDataTemplateDetail.checkUncheckPositiveNegative(this, \"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='Charc_Pos_" + SystemID + "_" + element.ROSSystemCharacteristicsId + "'  name='chkPositive'>" +
                      "</div><div class='checkbox-icon checkbox-cross pull-left'>" +
                      "<input type='checkbox'  class='' " + IsNegative + " onchange='ReviewOfSystemsDataTemplateDetail.checkUncheckPositiveNegative(this,\"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='Charc_Neg_" + SystemID + "_" + element.ROSSystemCharacteristicsId + "'  name='chkNegative'>" +
                      "</div></div><label class='pull-left mt-xxs pl-xxs control-label'>" + element.CharacteristicsName + hdnDescription + hdnROSSystemPatientCharacteristicsID + "</label><div class='clearfix'></div></div></li>";

                });
                //              HTMLSelectAllCheckBox += "<li id='selectAllBookMark' class='col-xs-4'><div class='clearfix'>" +
                //"<div class='mt-xxs ml-lg pull-left'><input class='mr-xs ml-xs pull-left' type='checkbox' " + isSelectAllPositiveChecked + " onchange='ReviewOfSystemsDataTemplateDetail.selectAllCheckBoxes(this, \"Positive" + "\", \"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='selectAllPositive_" + SystemID + "'  name='selectAllPositive'><input type='checkbox' " + isSelectAllNegativeChecked + " class='ml-xxs pull-left' onchange='ReviewOfSystemsDataTemplateDetail.selectAllCheckBoxes(this, \"Negative" + "\", \"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='selectAllNegative_" + SystemID + "' name='selectAllNegative'></div><label class='pull-left mt-xxs pl-xxs control-label'>Select All</label><div class='clearfix'></div></div></li>";

                HTMLSelectAllCheckBox += "<li id='selectAllBookMark' class='col-xs-4'><div>" +
                  "<div class='mt-xxs ml-lg pull-left'>" +
                  "<div class='checkbox-icon checkbox-check mr-xs pull-left'>" +
                  "<input class='pull-left' type='checkbox' " + isSelectAllPositiveChecked + " onchange='ReviewOfSystemsDataTemplateDetail.selectAllCheckBoxes(this, \"Positive" + "\", \"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='selectAllPositive_" + SystemID + "'  name='selectAllPositive'>" +
                  "</div><div class='checkbox-icon checkbox-cross pull-left'>" +
                  "<input type='checkbox' " + isSelectAllNegativeChecked + " class='pull-left' onchange='ReviewOfSystemsDataTemplateDetail.selectAllCheckBoxes(this, \"Negative" + "\", \"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='selectAllNegative_" + SystemID + "' name='selectAllNegative'>" +
                  "</div>" +
                  "</div><label class='pull-left mt-xxs pl-xxs control-label'>Select All</label><div class='clearfix'></div></div></div></li>";

                $ListVital.append(HTMLSelectAllCheckBox);
                $ListVital.append(HTMLSystemCharacteristics);
                $(charcSystemDiv).html($ListVital[0].outerHTML).prepend(DivRow[0].outerHTML);//.append(divSearchBox);

                /* this check will check the caching state of system characteristics, if user has made any changes in characteristics, it will bind the user state
                   Change Implemented By: Azhar Shahzad
                   Date: januray 02, 2016 02:56pm */
                if (ReviewOfSystemsDataTemplateDetail.CharcSystemInfo != null && ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName] != null) {
                    utility.bindMyJSON(true, JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName]), false, $(charcSystemDiv)).done(function () {
                        if ($('[id*=ReviewofSystemsSectionNormal]').is(':checked')) {
                            $('[id*=Charc_Pos_],[id*=Charc_Neg_]').prop('checked', false);

                        }
                    });

                    if (SystemNormalDescription != '' || (JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName]).hdnDescriptionNormal != null && JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName]).hdnDescriptionNormal != '')) {
                        if (SystemNormalDescription != '') {
                            $('#hdnDescriptionNormal').val(SystemNormalDescription);
                            ReviewOfSystemsDataTemplateDetail.changeColorOfNormalBookIcon();
                            $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').removeClass('disabled');
                            ReviewOfSystemsDataTemplateDetail.markNormalSection(SystemName, SystemID);
                        } else if (JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName]).hdnDescriptionNormal != null) {
                            $('#hdnDescriptionNormal').val(JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName]).hdnDescriptionNormal);
                            ReviewOfSystemsDataTemplateDetail.changeColorOfNormalBookIcon();
                            $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').removeClass('disabled');
                            ReviewOfSystemsDataTemplateDetail.markNormalSection(SystemName, SystemID);
                        }

                    } else {
                        $('[id*=Charc_Pos_],[id*=Charc_Neg_]').each(function (index, item) {
                            if ($(item).is(':checked')) {
                                $(item).prop('checked', true);
                                Clinical_ReviewofSystems.bookIconClassToggle($(item).parent().parent().siblings().siblings('[id=bookIcon]'), false);

                                ReviewOfSystemsDataTemplateDetail.changeColorForliCharacteristics(item);

                            } else {
                                if ($(item).parent().parent().find("[type=checkbox]:checked").length > 0) {
                                    Clinical_ReviewofSystems.bookIconClassToggle($(item).parent().parent().siblings().siblings('[id=bookIcon]'), false);
                                    ReviewOfSystemsDataTemplateDetail.changeColorForliCharacteristics(item);
                                } else {
                                    Clinical_ReviewofSystems.bookIconClassToggle($(item).parent().parent().siblings().siblings('[id=bookIcon]'), true);
                                    ReviewOfSystemsDataTemplateDetail.changeColorForliCharacteristics(item);
                                }
                                //if ($(item).siblings("[type=checkbox]").is(':checked')) {
                                //    $(item).parent().siblings().siblings('[id=bookIcon]').removeClass('disabled');
                                //    ReviewOfSystemsDataTemplateDetail.changeColorForliCharacteristics(item);
                                //} else {
                                //    $(item).parent().siblings().siblings('[id=bookIcon]').addClass('disabled');
                                //    ReviewOfSystemsDataTemplateDetail.changeColorForliCharacteristics(item);
                                //}
                            }
                        });
                    }



                    ReviewOfSystemsDataTemplateDetail.CacheCharacteristicInfo(CurrentSystem, false, true, false);
                    $(charcSystemDiv).data(SystemName, ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName]);
                } else {
                    $(charcSystemDiv).data(SystemName, $(charcSystemDiv).getMyJSON());
                    //If Normal (Global) is checked, make the selected Section Normal too
                    if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #chkReviewofSystemssNormal').is(':checked') || (SystemNormalDescription != '')) {
                        // ReviewOfSystemsDataTemplateDetail.markCurrentActiveSectionAsNormal();
                        $('#hdnDescriptionNormal').val(SystemNormalDescription);
                        ReviewOfSystemsDataTemplateDetail.changeColorOfNormalBookIcon();
                        $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').removeClass('disabled');
                        ReviewOfSystemsDataTemplateDetail.markNormalSection(SystemName, SystemID);

                    }
                        // else bind data to characteristics
                    else {
                        $.each(SystemCharacteristicsJSON, function (index, item) {
                            if (item.IsPositive == 'True') {
                                $('#Charc_Pos_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').prop('checked', true);
                                Clinical_ReviewofSystems.bookIconClassToggle($('#Charc_Pos_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').parent().siblings().siblings('[id=bookIcon]'), false);
                                // $('#Charc_Pos_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').parent().siblings().siblings('[id=bookIcon]').removeClass('disabled');
                                //  $('#Charc_Pos_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').parent().parent().addClass('red');
                                $('#Charc_Desc_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').val(item.Description);
                            } else if (item.IsPositive == 'False') {
                                $('#Charc_Neg_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').prop('checked', true)
                                Clinical_ReviewofSystems.bookIconClassToggle($('#Charc_Pos_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').parent().siblings().siblings('[id=bookIcon]'), false);
                                //   $('#Charc_Pos_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').parent().siblings().siblings('[id=bookIcon]').removeClass('disabled');
                                $('#Charc_Desc_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').val(item.Description);
                            }
                        });
                    }
                    ReviewOfSystemsDataTemplateDetail.CacheCharacteristicInfo(CurrentSystem, false, false, false);
                    $(charcSystemDiv).data(SystemName, ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName]);
                }
                //end change januray 02, 2016 02:56pm

                if ($(charcSystemDiv).find('input:checkbox:not([id*=ReviewofSystemsSectionNormal])').is(':checked') == true) {
                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #ActiveSystemUL").removeClass('disableAll');
                }
                else if ($(charcSystemDiv).find('input:checkbox[id*=ReviewofSystemsSectionNormal]').is(':checked')) {
                    $("divROSDataTemplate").find('.toggle').addClass('disableAll');
                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #ActiveSystemUL").addClass('disableAll');
                    //if (ReviewOfSystemsDataTemplateDetail.isAllNormalSystems()) {
                    //    ReviewOfSystemsDataTemplateDetail.markAllSystemsAsNormalCache();
                    //}
                }
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }
            ReviewOfSystemsDataTemplateDetail.handleColorForSelectedSystem(obj);

        });
        ReviewOfSystemsDataTemplateDetail.handleDetailToggleDiv();
        // }
    },

    // Caches all Characteristics details info for a selected system
    //  Author: ZeeshanAK | Date: March 31, 2016
    CacheCharacteristicDetailInfo: function (resetCache) {
        if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #ActiveSystemUL > li.active").length > 0) {
            var charcId = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #ActiveSystemUL > li.active").attr('id');
            var charcDetailDiv = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divExamCharacteristics');
            var DetailData = unescape($(charcDetailDiv).getMyJSON());//kr escape.

            var detailDataParsed = JSON.parse(DetailData);
            if (detailDataParsed.Duration == '' || detailDataParsed.ROSCharacteristicsDetailDurationId_text == "- Select -") {
                detailDataParsed.Duration = '';
                detailDataParsed.ROSCharacteristicsDetailDurationId_text = '';
            }
            DetailData = JSON.stringify(detailDataParsed);

            if (resetCache != null && resetCache == true) {
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divExamCharacteristics').resetAllControls();
            }
            ReviewOfSystemsDataTemplateDetail.CharcDetailDivJSON = unescape($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divExamCharacteristics').clone().getMyJSON());//kr escape.

            ReviewOfSystemsDataTemplateDetail.CharcDetailDivJSON = unescape($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divExamCharacteristics').clone().getMyJSON());//kr escape.

            var indexCh = -1;

            if (DetailData != ReviewOfSystemsDataTemplateDetail.CharcDetailDivJSON) {

                $.grep(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo, function (item, index) {
                    if (item.Id == charcId) {
                        indexCh = index;
                        return;
                    }
                });

                if (indexCh != -1) {
                    ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo[indexCh].DetailInfo = DetailData;
                }
                else {
                    var Ids = charcId.split('_');

                    var CharsDetailInfo = {
                        Id: charcId.split(' ').join(''),
                        SystemId: Ids.length > 0 ? Ids[1] : null,
                        CharcId: Ids.length > 0 ? Ids[2] : null,
                        DetailInfo: DetailData
                    };
                    ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo.push(CharsDetailInfo);
                }
            }
        }
    },

    //Change color of section book icon with normal checkbox.
    //  Author: ZeeshanAK | Date: March 31, 2016
    changeColorOfNormalBookIcon: function () {
        if (typeof $('#frmROSDataTemplateDetail #divNormal').find("input:hidden").val() != 'undefined' && $('#frmROSDataTemplateDetail #divNormal').find("input:hidden").val() != '') {
            $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').removeClass("blue");
            $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').removeClass("gray");
            $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').addClass("green");
            $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').removeClass('disabled');
            $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').tooltip({
                placement: 'left',
                container: 'body',
                title: function () {
                    return $(this).parent().find('input:hidden').val().replace(/<br\s*[\/]?>/gi, "\n");
                }
            });
        }
        else {
            $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').removeClass("green");
            $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').addClass("gray");
            //  $('#frmROSDataTemplateDetail #divNormal #bookIconNormal').addClass('disabled');
        }
    },

    // Marks section as normal
    //  Author: ZeeshanAK | Date: March 31, 2016
    markNormalSection: function (systemName, SystemID, fromMarkAll) {
        if (ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[systemName] != null && ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[systemName] != undefined) {
            var objData = JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[systemName]);
            Object.keys(objData).forEach(function (key, index) {
                // key: the name of the object key
                // index: the ordinal position of the key within the object
                if (key.indexOf("Charc_Neg_") > -1 || key.indexOf('Charc_Pos_') > -1 || key.indexOf('Charc_Desc') > -1) {
                    objData[key] = '';
                }
            });
            ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[systemName] = JSON.stringify(objData);
        }
        if (ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo != null) {
            //Start || 20-12-2016 || Zain-ul-abdin || Changes made for fixing EMR-2296
            var detailObj;
            $.each(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo, function (index, element) {
                if (element.SystemId == SystemID) {
                    //ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo[index] = null;
                    //Start || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-440
                    detailObj = $.grep(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo, function (item, index) {
                        return (item.SystemId == SystemID);
                    });
                    //End   || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-440
                }
            });
            ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo.splice(detailObj, 1);
            //End   || 20-12-2016 || Zain-ul-abdin || Changes made for fixing EMR-2296
        }
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').find("li.active").find("[name=isSystemNormal][type=hidden]").val("True");

        var CurrentSystem = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').find("li.active");
        ReviewOfSystemsDataTemplateDetail.CacheCharacteristicInfo(CurrentSystem, false, true, false);


        $("divROSDataTemplate").find('.toggle').addClass('disableAll');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #ActiveSystemUL > li").removeClass('active');
        $('#txtSearchCharacteristics').parent().addClass('disableAll');

        if (fromMarkAll == null) {
            if (ReviewOfSystemsDataTemplateDetail.isAllNormalSystems()) {
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #chkReviewofSystemssNormal').prop('checked', true);
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #bookIconNormalGlobal').removeClass('disabled');
                ReviewOfSystemsDataTemplateDetail.markCurrentActiveSectionAsNormal(true, null, systemName);
            } else {
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #chkReviewofSystemssNormal').prop('checked', false);
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #bookIconNormalGlobal').addClass('disabled');
                ReviewOfSystemsDataTemplateDetail.markCurrentActiveSectionAsNormal(false);

            }
            ReviewOfSystemsDataTemplateDetail.changeColorOfNormalBookIcon();
        }
    },
    /* Normalize currently active section
     Author: ZeeshanAK | Date: February 02, 2016 */
    markCurrentActiveSectionAsNormal: function (sectionNormal, globalNormal, systemName) {
        var FormId = '#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail";
        var Charclist = FormId + " #ActiveSystemUL li";
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #SystemSections').find('input[type=checkbox]').parent().parent().siblings("div.rightInnerAddon").remove();
        $(Charclist).find('input[type=checkbox]').prop({
            'checked': false, 'disabled': true
        });
        $(FormId + " #ActiveSystemUL").addClass('disableAll')
        Clinical_ReviewofSystems.bookIconClassToggle($(Charclist).find('[id=bookIcon]'), true);
        // $(Charclist).find('[id=bookIcon]').addClass('disabled');
        $(FormId).find('[id=bookIconNormal]').removeClass('disabled');

        //    $(FormId + ' #divReviewofSystemsSystemSection ul li a i').addClass("blue");
        //        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divReviewofSystemsSystemSection #ActiveSystemUL').find('input:hidden').val('');//kr
        //  $(Charclist).find('input[id*=Pos]').parent().parent().removeClass('red');
        //if ((globalNormal != null && globalNormal == true) || Clinical_ReviewofSystems.isAllNormalSystems()) {
        //    $('#' + Clinical_ReviewofSystems.params.PanelID + ' [name^=ReviewofSystemsSectionNormal]').prop('checked', true);
        //    Clinical_ReviewofSystems.markAllSystemsAsNormalCache();
        //} else {
        //    Clinical_ReviewofSystems.CacheCharacteristicInfo(null, false);
        //}

    },
    /* Mark current section as normal i.e. uncheck all checkboxes and disable them
    Author: ZeeshanAK | Date: January 29, 2016 */
    markCurrentSectionAsNormal: function (obj, systemName, SystemID) {
        var message = 'This will mark the entire ' + systemName + ' as Normal and reset all values in all sections of ' + systemName + '. Would you like to proceed?';
        var isToMarkNormal = $(obj).prop("checked");
        if (isToMarkNormal == true) {
            utility.myConfirm(message, function () {
                countWidth();
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').removeClass('active');
                ReviewOfSystemsDataTemplateDetail.markNormalSection(systemName, SystemID, null);
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #bookIconNormal").removeClass('disabled');//kr
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #bookIconNormal").removeClass('gray');
            }, function () {
                $(obj).prop("checked", false);
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #bookIconNormal").addClass('disabled');//kr
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #bookIconNormal").addClass('gray');
            }, "Confirm Mark Normal");
        }
        else {
            ReviewOfSystemsDataTemplateDetail.unmarkNormalSection(systemName, SystemID);
        }
    },
    //Change color of Characteristics
    //  Author: ZeeshanAK | Date: March 31, 2016
    changeColorForliCharacteristics: function (cntrl) {
        if (typeof $(cntrl).closest('li').find("[type=hidden][id^=Charc_Desc_]").val() != 'undefined' && $(cntrl).closest('li').find("[type=hidden][id^=Charc_Desc_]").val() != '') {
            $(cntrl).closest('li').find("a").find("i").removeClass("blue");
            $(cntrl).closest('li').find("a").removeClass("disabled");
            $(cntrl).closest('li').find("a").find("i").addClass("green");
            $(cntrl).closest('li').find("a").find("i").tooltip({
                placement: 'left',
                container: 'body',
                title: function () {
                    return $(this).closest('li').find("[type=hidden][id^=Charc_Desc_]").val().replace(/<br\s*[\/]?>/gi, "\n");
                }
            });
        }
        else {
            $(cntrl).closest('li').find("a").find("i").removeClass("green");
        }
    },


    // Caches all Characteristics info for a selected system
    //  Author: ZeeshanAK | Date: March 31, 2016
    CacheCharacteristicInfo: function (CurrentSystem, cacheAll, IsNormal, removeCache, SystemName) {
        if (CurrentSystem == null || removeCache == true && SystemName != null) {
            if (ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName] != null) {
                var objData = JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName]);
                Object.keys(objData).forEach(function (key, index) {
                    // key: the name of the object key
                    // index: the ordinal position of the key within the object
                    objData[key] = '';
                });
                ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName] = JSON.stringify(objData);
            }
        } else {
            var charcSystemDiv = '#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystemSection';
            if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #SystemSections').find('input[type=checkbox]:checked').length > 0 || IsNormal == true) {
                if (cacheAll != null && cacheAll == true) {
                    $.map($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
                        var sName = n.split('_')[1];
                        var sID = n.split('_')[2];
                        if (removeCache != null && removeCache == true) {
                            if (ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName] != null) {
                                var objData = JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName]);
                                Object.keys(objData).forEach(function (key, index) {
                                    // key: the name of the object key
                                    // index: the ordinal position of the key within the object
                                    objData[key] = '';
                                });
                                ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName] = JSON.stringify(objData);
                            }
                        } else {
                            var obj = {};
                            obj['ReviewofSystemsSectionNormal' + "_" + sID] = IsNormal;
                            if (ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName] != null) {
                                var tempData = JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName]);
                                tempData['ReviewofSystemsSectionNormal' + "_" + sID] = IsNormal;
                                ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName] = JSON.stringify(tempData);
                            } else {
                                ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName] = JSON.stringify(obj);
                            }
                        }
                    });

                } else {
                    /* this check will check the caching state of system characteristics, if user has made any changes in characteristics
                 Change Implemented By: Azhar Shahzad
                 Date: januray 02, 2016 02:56pm */
                    if ($(charcSystemDiv).data(CurrentSystem.text()) != null && $(charcSystemDiv).data(CurrentSystem.text()).length > 0) {
                        if ($(charcSystemDiv).data(CurrentSystem.text()) != $(charcSystemDiv).getMyJSON()) {
                            SystemNameData = unescape($(charcSystemDiv).getMyJSON());
                            ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[CurrentSystem.text()] = SystemNameData;
                            $(CurrentSystem).find('span:first-child').addClass('green');
                        }
                    } else {
                        SystemNameData = unescape($(charcSystemDiv).getMyJSON());
                        ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[CurrentSystem.text()] = SystemNameData;
                        $(CurrentSystem).find('span:first-child').addClass('green');
                    }
                    //end change januray 02, 2016 02:56pm
                }

            } else {
                SystemNameData = unescape($(charcSystemDiv).getMyJSON());
                ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[CurrentSystem.text()] = SystemNameData;
                $(CurrentSystem).find('span:first-child').removeClass('green');
            }
        }
    },

    // Checks if all systems are normal or not
    //  Author: ZeeshanAK | Date: March 31, 2016
    isAllNormalSystems: function () {
        if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul li [name=isSystemNormal][type=hidden][value=False]").length > 0) {
            return false;
        } else {
            if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul li [name=isSystemNormal][type=hidden][value='']").length > 0) {
                return false;
            }
            return true;
        }
    },

    //Start By Khaleel Ur Rehman To Filter Characteristics on 28 January 2016.
    getFilteredCharacteristics: function (cntrl) {
        var rex = new RegExp($(cntrl).val(), 'i');
        $('.searchable li').hide();
        $('.searchable li').filter(function () {
            return rex.test($(this).text());
        }).show();
    },


    /* Select all Positive or Negative checkboxes if it's selected from Select All
    Author: ZeeshanAK | Date: February 01, 2016 */
    selectAllCheckBoxes: function (obj, isPositive, systemNameWithID) {

        var Charclist = '#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #ActiveSystemUL li";
        if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #ActiveSystemUL > li").hasClass('active'))
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #ActiveSystemUL > li").removeClass('active');

        $(obj).parents('li').addClass('active');

        if ($(obj).prop("checked")) {
            if (isPositive == 'Positive') {
                $(Charclist).find('input[id*=Pos]').prop('checked', true);
                $(Charclist).find('input[id*=Neg]').prop('checked', false);
                Clinical_ReviewofSystems.bookIconClassToggle($(Charclist).find('[id=bookIcon]'), false);
            } else {
                $(Charclist).find('input[id*=Pos]').prop('checked', false);
                $(Charclist).find('input[id*=Neg]').prop('checked', true);
                Clinical_ReviewofSystems.bookIconClassToggle($(Charclist).find('[id=bookIcon]'), false);
            }
        } else {
            $(Charclist).find('input[id*=Pos]').prop('checked', false);
            $(Charclist).find('input[id*=Neg]').prop('checked', false);
            Clinical_ReviewofSystems.bookIconClassToggle($(Charclist).find('[id=bookIcon]'), true);
        }
        ReviewOfSystemsDataTemplateDetail.changeColorSystemOnCharcChange();
    },

    clearSectionInfo: function (event, obj, SystemID, SystemName) {
        event.stopPropagation();
        //if (Clinical_ReviewofSystems.CharcSystemInfo[SystemName] != null && $(obj).parent().find('span:first-child').hasClass('green')) {
        if (($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divReviewofSystemsSystemSection").find('input[type=checkbox]:checked').length > 0 || $(obj).closest("li").find('span').hasClass('green'))) {
            var message = 'This will reset all values in ' + SystemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?';
            var sysPatID = $(obj).closest("li").find("#systemPatientID").val();
            var sysPatNormal = $(obj).closest("li").find("#isSystemNormal").val();
            var CharcID = $(obj).closest("li").find("#isCharacteristicsExists").val();
            utility.myConfirm(message, function () {

                if ((typeof CharcID != 'undefined' && CharcID != '') || (typeof sysPatNormal != 'undefined' && sysPatNormal == 'True' || sysPatNormal == true)) {

                    ReviewOfSystemsDataTemplateDetail.rOSDataSystemReset_DBCall(sysPatID).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            ReviewOfSystemsDataTemplateDetail.clearInfoForSystemReset(obj, SystemID, SystemName);
                        }
                    });
                } else {
                    ReviewOfSystemsDataTemplateDetail.clearInfoForSystemReset(obj, SystemID, SystemName);
                }
            }, function () {
            }, 'Reset Confirmation');

        }
    },

    // unMarks section as normal
    unmarkNormalSection: function (systemName, SystemID) {
        var FormId = '#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail";
        $(FormId + " #ActiveSystemUL").removeClass('disableAll');
        $(FormId + " #ActiveSystemUL li").find('input[type=checkbox]').prop("disabled", false);
        $(FormId).find('[id=bookIconNormal],[id =bookIconNormalGlobal]').addClass('disabled');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #chkReviewofSystemssNormal').prop('checked', false);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #txtSearchCharacteristics').parent().removeClass('disableAll');
        $(FormId).find('[id=divNormal],[id =bookIconNormalGlobal]').val('');
        $(FormId + ' #divNormal').find("div.rightInnerAddon").remove();
        $(FormId + ' #divNormalGlobal').find("input:hidden").val('');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').find("li.active").find("[name=isSystemNormal][type=hidden]").val("False");
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').find("li.active").find("[name=systemNormalDescription][type=hidden]").val('');
        $(FormId + ' #divNormal').find("[name=hdnDescriptionNormal][type=hidden]").val('');
        //Start || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').find("li.active").removeClass('green').find('span:first-child').removeClass("green");
        //End   || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
        
        ReviewOfSystemsDataTemplateDetail.changeColorOfNormalBookIcon();
        ReviewOfSystemsDataTemplateDetail.CacheCharacteristicInfo(null, false, false, true, systemName, SystemID);
    },
    // Clears the searchbox
    //  Author: ZeeshanAK | Date: March 31, 2016
    clearSearchBox: function () {
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #txtSearchCharacteristics').val('');
        ReviewOfSystemsDataTemplateDetail.getFilteredCharacteristics('');
    },
    changeColorSystemOnCharcChange: function () {
        var CurrentSystem = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #sectionReviewofSystems li.active");
        var charcSystemDiv = '#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystemSection';
        var addGreen = false;
        if (($(charcSystemDiv).find('input:checkbox') != null && $(charcSystemDiv).find('input:checkbox').length > 0)) {
            if ($(charcSystemDiv).find('input:checkbox:checked') != null && $(charcSystemDiv).find('input:checkbox:checked').length > 0) {
                addGreen = true;
            } else {
                addGreen = false;
            }
        }

        if (addGreen) {
            $(CurrentSystem).find('span:first-child').addClass("green");
        } else {
            //Start || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
            $(CurrentSystem).removeClass("green").find('span:first-child').removeClass("green");
            //End   || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
        }
        return addGreen;
    },
    // Handle color for selected system.
    //  Author: ZeeshanAK | Date: March 31, 2016
    handleColorForSelectedSystem: function (obj) {
        var charcSystemDiv = '#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystemSection';
        var addGreen = false;
        if (((typeof $(obj).closest("li").find("#isCharacteristicsExists").val() != 'undefined' && $(obj).closest("li").find("#isCharacteristicsExists").val() != '' && $(obj).closest("li").find("#isCharacteristicsExists").val() != null) || $(obj).closest("li").find("#isSystemNormal").val() == "True")
            && ReviewOfSystemsDataTemplateDetail.changeColorSystemOnCharcChange()) {

            addGreen = true;

        } else {
            addGreen = false;
        }
        if ($(charcSystemDiv).find('input:checkbox:checked') != null && $(charcSystemDiv).find('input:checkbox:checked').length > 0) {
            addGreen = true;
        }
        if (ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo != null && ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo.length > 0) {
            $.each(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo, function (i, item) {
                if ((item.Id).indexOf($(obj).prop('title')) > -1 && $(charcSystemDiv).find('input:checkbox:checked').length > 0) {
                    addGreen = true;
                }
            })
        }
        if (addGreen) {
            $(obj).closest("li").find('span:first-child').addClass("green");
        } else {
            //Start || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
            $(obj).closest("li").removeClass("green").find('span:first-child').removeClass("green");
            //End   || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
        }
    },

    // Handle Detail Toggle Div.
    //  Author: ZeeshanAK | Date: March 31, 2016
    handleDetailToggleDiv: function () {
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " divROSDataTemplate").find('.toggle').prev().addClass('hidden');
        countWidth();
        $("divROSDataTemplate").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
        $("divROSDataTemplate").find('.toggle').removeClass('active');
        $("divROSDataTemplate").find('.toggle').addClass('disableAll');
    },

    //Binding Validation Function
    //  Author: ZeeshanAK | Date: March 31, 2016
    validateROSDataTemplate: function () {
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail').bootstrapValidator('destroy');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  TemplateName: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: 'Specify a Name for the Template and try again.'
                          },
                      }
                  },
                  //Entity: {
                  //    group: '.col-sm-4',
                  //    validators: {
                  //        notEmpty: {
                  //            message: ''
                  //        },
                  //    }
                  //},

              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           //if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ROSSystemsUL li').find('[type=checkbox]:checked').length > 0) {
           ReviewOfSystemsDataTemplateDetail.ReviewofSystemsSave(true);
           //  utility.DisplayMessages('Saving stuff. Sit tight bro!', 1);
           //} else {
           //    utility.DisplayMessages('Please select at least one System to save/update Template', 4);
           //}

       });
    },

    //Hide footer buttons on Template screen and showing back on ROS
    //  Author: ZeeshanAK | Date: March 31, 2016
    hideFooterButtons: function (isHide) {
        isHide ? $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #footerButtons').hide() : $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #footerButtons').show();
    },


    // To load system Patient characteristic detail.
    //  Author: ZeeshanAK | Date: April 31, 2016
    loadSysPatCharcDetail: function (cntrl, isCharcDetailExists) {
        // if (ReviewOfSystemsDataTemplateDetail.checkDurationDetails()) {
        if (!$('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divROSDataTemplate').find('.toggle').hasClass('disableAll')) {
            ReviewOfSystemsDataTemplateDetail.CacheCharacteristicDetailInfo(true);
        }
        ReviewOfSystemsDataTemplateDetail.addActiveClass(cntrl);
        EMRUtility.resetControlValue($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divExamCharacteristics'));
        //ReviewOfSystemsDataTemplateDetail.validateDurationDetails();
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #headingDetail').html($(cntrl).text() + ' Detail');
        var charcDetailId = $(cntrl).find("label").find("input[type='hidden']:eq(1)").attr("id").split('_')[1];
        var detail = $.grep(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo, function (item, index) {
            return item.Id == cntrl.id;
        });
        if (isCharcDetailExists != null && isCharcDetailExists != '' && (detail == null || detail.length == 0)) {
            ReviewOfSystemsDataTemplateDetail.getSystemPatientCharcDetail_DBCall(charcDetailId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var detail = JSON.parse(response.SystemCharacteristicsDetails_JSON);
                    ReviewOfSystemsDataTemplateDetail.populateDetailAgainstCharacteristic(detail[0]);
                    ReviewOfSystemsDataTemplateDetail.CacheCharacteristicDetailInfo(false);
                }
            });
        } else {
            if ((detail != null && detail.length != 0)) {
                ReviewOfSystemsDataTemplateDetail.bindCacheCharacteristicDetailInfo(cntrl);
                ReviewOfSystemsDataTemplateDetail.validateDurationDetails();
                //      }
            }
        }
    },

    // Implementing checkboxes functionality as radio boxes i.e. only one can be selected at a time.
    //    And enable the book icon only if one of the checkbox is checked.
    //    Also, uncheck the Normal checkbox for this characteristics
    //  Author: ZeeshanAK | Date: April 01, 2016
    checkUncheckPositiveNegative: function (checkedBox, systemNameWithID) {
        //event.stopPropagation();
        //  if (ReviewOfSystemsDataTemplateDetail.checkDurationDetails()) {
        if (!$("#divROSDataTemplate").find('.toggle').prev().hasClass('hidden')) {
            $("#divROSDataTemplate").find('.toggle').prev().addClass('hidden');
            countWidth();
            $("#divROSDataTemplate").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
            $("#divROSDataTemplate").find('.toggle').removeClass('active');
        }

        if (!$("#divROSDataTemplate").find('.toggle').hasClass('disableAll')) {
            ReviewOfSystemsDataTemplateDetail.CacheCharacteristicDetailInfo(false);
        }

        if ($("#ActiveSystemUL > li").hasClass('active'))
            $("#ActiveSystemUL > li").removeClass('active');

        if (checkedBox != null) {
            ReviewOfSystemsDataTemplateDetail.validateDurationDetails();

            if ($(checkedBox).is(':checked')) {

                Clinical_ReviewofSystems.bookIconClassToggle($(checkedBox).parent().parent().siblings().siblings('[id=bookIcon]'), false);
                if ($(checkedBox).parent().siblings('.checkbox-icon').find('[type=checkbox]').is(':checked')) {
                    $(checkedBox).parent().siblings('.checkbox-icon').find('[type=checkbox]').prop('checked', false);
                }
                $('[id^=selectAllNeg] ,[id^=selectAllPos').prop('checked', false);

                if (!$("#ReviewofSystems").find('.toggle').hasClass('disableAll')) {
                    ReviewOfSystemsDataTemplateDetail.LoadCharacteristicDetailInfo(checkedBox);
                }

            } else {
                Clinical_ReviewofSystems.bookIconClassToggle($(checkedBox).parent().parent().siblings().siblings('[id=bookIcon]'), true);
                $('[id^=selectAllNeg] ,[id^=selectAllPos').prop('checked', false);
                var charcId = $(checkedBox).closest('li').attr('id');

                var message = "Unselecting a characteristic will remove its detail. Are you sure you want to unselect?";
                var isCharcDetailExists = $(checkedBox).closest('li').find("[type=hidden][name=isCharcDetailExists]").val();

                //var systemPatientCharacteristicsID = $(checkedBox).parent().parent().find("[type=hidden][name=SystemPatientCharacteristicsID]").val();
                var systemPatientCharacteristicsID = $(checkedBox).closest('li').find("[type=hidden][name=SystemPatientCharacteristicsID]").attr('id');
                systemPatientCharacteristicsID = systemPatientCharacteristicsID != null ? systemPatientCharacteristicsID.split('_')[1] : systemPatientCharacteristicsID;
                if (systemPatientCharacteristicsID != -1 && systemPatientCharacteristicsID != '' && systemPatientCharacteristicsID != null) {
                    utility.myConfirm(message, function () {
                        ReviewOfSystemsDataTemplateDetail.deleteROSDataSystemCharcDetail_DBCall(systemPatientCharacteristicsID).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                ReviewOfSystemsDataTemplateDetail.uncheckCharacteristic($(checkedBox));
                            }
                        });

                    }, function () {
                        $(checkedBox).prop('checked', true);
                        ReviewOfSystemsDataTemplateDetail.enableDisableCharcDetails();
                    });
                } else {
                    ReviewOfSystemsDataTemplateDetail.uncheckCharacteristic($(checkedBox));
                }
                ReviewOfSystemsDataTemplateDetail.hideTextAreaWhenBothChkBoxesUnchecked(checkedBox);
            }

            ReviewOfSystemsDataTemplateDetail.enableDisableCharcDetails();
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' [name=ReviewofSystemsSectionNormal]').prop('checked', false);
            ReviewOfSystemsDataTemplateDetail.changeColorSystemOnCharcChange();
        }
    },


    // Add active class if a characteristics li is clicked
    //  Author: ZeeshanAK | Date: April 01, 2016
    addActiveClass: function (selectedLi) {
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #ActiveSystemUL > li").removeClass('active');
        $(selectedLi).addClass('active');
        if (!$('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').prev().hasClass('hidden')) {
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').prev().addClass('hidden');
            countWidth();
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').removeClass('active');
        }
        ReviewOfSystemsDataTemplateDetail.enableDisableCharcDetails(selectedLi);
    },

    // Enabling and Disabling of characteristics details
    //  Author: ZeeshanAK | Date: April 01, 2016
    enableDisableCharcDetails: function (selectedLi) {
        if (selectedLi != null) {
            var objList = $.grep($(selectedLi).find('input:checkbox'), function (element) {
                if ($(element).is(':checked')) {
                    return element;
                }
            });
            if (objList.length > 0) {
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').removeClass('disableAll');
            } else {
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').addClass('disableAll');
            }
        } else {
            if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divReviewofSystemsSystemSection").find('input:checkbox:not([id*=ReviewofSystemsSectionNormal])').is(':checked') == true) {
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').removeClass('disableAll');
            }
            else if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divReviewofSystemsSystemSection").find('input:checkbox[id*=ReviewofSystemsSectionNormal]')) {
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divROSDataTemplate").find('.toggle').addClass('disableAll');
            }
        }

    },

    //Purpose : To load system Patient characteristic detail.
    //  Author: ZeeshanAK | Date: April 01, 2016
    populateDetailAgainstCharacteristic: function (detail) {
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #PreviousHistory').val(detail.PreviousHistory);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailStatusId').val(detail.ROSCharacteristicsDetailStatusId);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #Onset').val(detail.Onset);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #Duration').val(detail.Duration);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailDurationId').val(detail.ROSCharacteristicsDetailDurationId);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailPatternId').val(detail.ROSCharacteristicsDetailPatternId);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailSeverityId').val(detail.ROSCharacteristicsDetailSeverityId);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailCourseId').val(detail.ROSCharacteristicsDetailCourseId);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailRadiationId').val(detail.ROSCharacteristicsDetailRadiationId);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailFrequencyId').val(detail.ROSCharacteristicsDetailFrequencyId);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailContextId').val(detail.ROSCharacteristicsDetailContextId);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailCharacterCSZId').val(detail.ROSCharacteristicsDetailCharacterCSZId);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailAggravedById').val(detail.ROSCharacteristicsDetailAggravedById);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #ROSCharacteristicsDetailRelievedById').val(detail.ROSCharacteristicsDetailRelievedById);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #Location').val(detail.Location);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #PrecipitatedBY').val(detail.PrecipitatedBY);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #AssociatedWith').val(detail.AssociatedWith);
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #hfROSCharacteristicsDetailsId').val(detail.ROSCharacteristicsDetailsId);
        ReviewOfSystemsDataTemplateDetail.validateDurationDetails();
    },

    // Binds Characteristics details
    //  Author: ZeeshanAK | Date: April 01, 2016
    bindCacheCharacteristicDetailInfo: function (cntrl) {
        var charcId = cntrl.id;
        var detail = $.grep(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo, function (item, index) {
            return item.Id == charcId;
        });
        if ((detail != null && detail.length != 0)) {
            detailJson = JSON.parse(detail[0].DetailInfo);

            ReviewOfSystemsDataTemplateDetail.populateDetailAgainstCharacteristic(detailJson);
        }

    },

    // To make sure duration has a qualifier selected
    //  Author: ZeeshanAK | Date: April 01, 2016
    validateDurationDetails: function (obj) {
        var duration = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divExamCharacteristics #Duration');
        var durationQualifier = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divExamCharacteristics #ROSCharacteristicsDetailDurationId');
        if (duration.val() != null && duration.val() != '') {
            durationQualifier.prop("disabled", false);
        } else {
            durationQualifier.prop("disabled", true).find("option:first").attr("selected", true);
        }
    },

    // When a Characteristics is clicked its detail info is loaded from cache or sends DB call
    //  Author: ZeeshanAK | Date: April 01, 2016
    LoadCharacteristicDetailInfo: function (checkedBox) {

        var charcId = $(checkedBox).parents('li').attr('id');
        var charcDetailDiv = $('#divExamCharacteristics');

        if (ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo != null && ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo.length > 0) {

            var detailObj = $.grep(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo, function (item, index) {
                return (item.Id == charcId);
            });

            if (detailObj.length > 0) {
                utility.bindMyJSON(true, JSON.parse(detailObj[0].DetailInfo), false, $(charcDetailDiv));
                charcId = charcId.replace('/', '\\/');

                $(charcDetailDiv).data($(charcId), detailObj[0].DetailInfo);
                ReviewOfSystemsDataTemplateDetail.validateDurationDetails();
            }
            else {
                $(charcDetailDiv).resetAllControls();
            }

        } else {
            $(charcDetailDiv).resetAllControls();

        }
    },

    // To normalize all sections in CharcSystemInfo
    //  Author: ZeeshanAK | Date: April 01, 2016
    uncheckCharacteristic: function (checkedBox) {
        var charcId = $(checkedBox).parents('li').attr('id');
        var detailObj = $.grep(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo, function (item, index) {
            return (item.Id == charcId);
        });
        ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo.splice(detailObj, 1);
        Clinical_ReviewofSystems.bookIconClassToggle($(checkedBox).parent().parent().siblings().siblings('[id=bookIcon]'), true);

        $(checkedBox).parent().parent().parent().find("[type=hidden]").val('');
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divExamCharacteristics').resetAllControls();

        ReviewOfSystemsDataTemplateDetail.enableDisableCharcDetails();
        ReviewOfSystemsDataTemplateDetail.handleDetailToggleDiv();
    },

    // Purpose: To Restrict user to enter description for a characteristic when both positive and negative checkbxes are unchecked.
    //  Author: ZeeshanAK | Date: April 01, 2016
    hideTextAreaWhenBothChkBoxesUnchecked: function (chkBox) {
        if ($(chkBox).closest("li").find('input[type=checkbox]:checked').length == 0) {
            $(chkBox).closest("li").find("div.rightInnerAddon").remove();
        }
    },

    //To Show text area when Book Mark Icon is clicke.
    //  Author: ZeeshanAK | Date: April 01, 2016
    showTextArea: function (cntrl) {
        var txtArea = "";
        txtArea = '<div class="rightInnerAddon">' +
            '<textarea id="" spellcheck="true" maxlength="250" class="form-control pr-xlg height-max105 size100per textAreaScroll"   onkeydown=\"ReviewOfSystemsDataTemplateDetail.setValueInHiddenDescription(event,this,0);\" onblur=\"ReviewOfSystemsDataTemplateDetail.textAreaFocusOut(event,this);\"></textarea><div class="clearfix"></div>' +
            '</div>';
        if (cntrl.id == 'bookIconNormal') {
            $(cntrl).closest('div').find('textarea').remove();
            $(cntrl).closest('div').append(txtArea);
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divNormal").find("textarea").html($('#divNormal').find("input:hidden").val().replace(/<br\s*[\/]?>/gi, "\n"));
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #bookIconNormal").addClass('disabled');//kr
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #divNormal").find("textarea").focus();
        }
        else {
            if ($(cntrl).closest('ul').find('.rightInnerAddon').length == 0) {
                $(cntrl).closest('ul').find('textarea').remove();
                $(cntrl).closest('li').append(txtArea);
                $(cntrl).closest('li').find("textarea").html($(cntrl).closest('li').find("[type=hidden][id^=Charc_Desc_]").val().replace(/<br\s*[\/]?>/gi, "\n"));
            }
            $('.textAreaScroll').slimScroll({
                position: 'right',
                height: '100%',
            }).on('input', function () {
                ReviewOfSystemsDataTemplateDetail.increaseRowsTextarea(this);
            }).on('focus', function () {
                ReviewOfSystemsDataTemplateDetail.increaseRowsTextarea(this);
            });
            $(cntrl).closest('li').find("textarea").focus();
        }
    },


    // Increases the row of text area if user types more.
    //  Author: ZeeshanAK | Date: April 01, 2016
    increaseRowsTextarea: function (obj) {
        while (
      obj.rows > 1 &&
      obj.scrollHeight < obj.offsetHeight
  ) {
            obj.rows--;
        }
        var h = 0;
        while (obj.scrollHeight > obj.offsetHeight && h !== obj.offsetHeight) {
            h = obj.offsetHeight;
            obj.rows++;
        }
    },


    //By Khaleel Ur Rehman to set val in hidden field from text area when Press Enter in Text Area.
    setValueInHiddenDescription: function (e, cntrl, btnClicked) {
        var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        if (btnClicked == 1) {
            $(cntrl).parent().parent().find("textarea").attr('flag', '1')
            if ($(cntrl).closest('#divNormal').length > 0) {
                var NormalComments = $('#frmROSDataTemplateDetail #divNormal').find("textarea").val().replace(/\n/g, '<br/>');
                $('#frmROSDataTemplateDetail #divNormal').find("input:hidden").val(NormalComments);
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #bookIconNormal").removeClass('disabled');//kr
                ReviewOfSystemsDataTemplateDetail.changeColorOfNormalBookIcon();
                var systemname = $('#frmROSDataTemplateDetail #divNormal').attr('systemname');
                var activeSystem = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').find("li[title='" + systemname + "']");
                activeSystem = activeSystem || $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #sectionReviewofSystems li.active");
                activeSystem.find("#isSystemNormal").val('True');
                activeSystem.find("#systemNormalDescription").val(NormalComments);
            }
            else {
                var $CharcLi = $(cntrl).closest('li');
                var comments = $CharcLi.find("textarea").val().replace(/\n/g, '<br/>');
                $CharcLi.find("[type=hidden][id^=Charc_Desc_]").val(comments);
                ReviewOfSystemsDataTemplateDetail.changeColorForliCharacteristics(cntrl);
                var systemname = $CharcLi.attr('systemname');
                var activeSystem = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').find("li[title='" + systemname + "']");
                var CurrentSystem = activeSystem || $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').find("li.active");
                ReviewOfSystemsDataTemplateDetail.CacheCharacteristicInfo(CurrentSystem, false, true, false);
            }
            $(cntrl).closest("div.rightInnerAddon").remove();
        }
    },

    //By Khaleel Ur Rehman to Show confirmation box to save data if there are any comments in the text area on focus out
    textAreaFocusOut: function (e, cntl) {

        if ($(cntl).attr('flag') != '1') {
            if ($(".form-group").find("textarea").length > 0) {
                var message = 'There is unsaved data in description, do you want to save it?';
                var control = $(".form-group").find("textarea");
                ReviewOfSystemsDataTemplateDetail.setValueInHiddenDescription(e, control, 1);
            }
        } else {
            $(cntl).attr('flag', '0');
        }

    },


    ReviewofSystemsSave: function (UnloadReviewofSystems, isSaveAS) {
        var objDeffered = $.Deferred();
        if ($("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val() != '' || (isSaveAS != null && isSaveAS == true)) {
            setTimeout(function () {
                // caching current selected System informations and characteristics Details Information
                ReviewOfSystemsDataTemplateDetail.CacheCharacteristicDetailInfo(true);
                var CurrentSystem = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystems').find("li.active");
                if (CurrentSystem.find('[name=isSystemNormal][type=hidden]').val() == "True") {
                    ReviewOfSystemsDataTemplateDetail.CacheCharacteristicInfo(CurrentSystem, false, true, false);
                } else {
                    ReviewOfSystemsDataTemplateDetail.CacheCharacteristicInfo(CurrentSystem, false, false, false);
                }
                //end caching current selected System informations and characteristics Details Information

                if (ReviewOfSystemsDataTemplateDetail.params.mode == "Add" || ReviewOfSystemsDataTemplateDetail.params.mode == "Edit") {
                    $.when(ReviewOfSystemsDataTemplateDetail.existingUncheckAll()).then(function () {
                        ReviewOfSystemsDataTemplateDetail.saveReviewofSystems_DbCall(isSaveAS).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                ReviewOfSystemsDataTemplateDetail.params.mode = "Edit";

                                //reseting the ROS Form after save/ update ROS
                                ReviewOfSystemsDataTemplateDetail.hideDetailAfterSaveUpdate();
                                //if (ReviewofSystemsType != null && ReviewofSystemsType != 'IsNormal') {
                                $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val('');
                                //}

                                ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo = [];
                                ReviewOfSystemsDataTemplateDetail.CharcDetailDivJSON = '';
                                ReviewOfSystemsDataTemplateDetail.CharcSystemInfo = [];

                                //Start || 26 April, 2016 || ZeeshanAK || Change made for fixing EMR-622
                                //$('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #frmROSDataTemplateDetail #divReviewofSystemsSystemSection').empty();
                                //End   || 26 April, 2016 || ZeeshanAK || Change made for fixing EMR-622

                                //if (UnloadReviewofSystems != null && UnloadReviewofSystems == false && ReviewofSystemsType != 'TabNavigation') {
                                //    ReviewOfSystemsDataTemplateDetail.oad_ros_systemsystemInfo();
                                //}
                                if (UnloadReviewofSystems) { ReviewOfSystemsDataTemplateDetail.unLoadTab(); }
                                ReviewOfSystemsDataTemplate.rosDataTemplateSearch();
                                //end reseting the ROS Form after save/ update ROS
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                            objDeffered.resolve();
                        });
                    });
                } else {
                    objDeffered.resolve();
                }
            }, 5);
        } else {
            utility.DisplayMessages("Please make any changes to save/update ROS Systems Information.", 3);
            objDeffered.resolve();
        }
        return objDeffered;
    },

    // Checks if previously checked box is unchecked or not
    existingUncheckAll: function () {
        var objDeffered = $.Deferred();
        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul [type=hidden][name=isCharacteristicsExists]:not([value=''])").each(function (index, element) {
            var SystemName = $(element).closest('li').text();
            var sID = $($(element).closest('li')[0]).attr('id').split('_')[2];
            if (ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName] != null) {
                var tempJson = JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[SystemName]);
                var result = false;
                if (tempJson != null) {

                    if (tempJson['ReviewofSystemsSectionNormal' + "_" + sID] != null && (tempJson['ReviewofSystemsSectionNormal' + "_" + sID] == true || tempJson['ReviewofSystemsSectionNormal' + "_" + sID] == "True")) {
                        result = true;
                    } else {
                        if (typeof tempJson.txtSearchCharacteristics == "undefined") {
                            result = true;
                        }
                        $.map(tempJson, function (index, item) {

                            if (item.indexOf('Charc_Neg_') > -1 && index == true) {
                                result = true;

                            } else if (item.indexOf('Charc_Pos_') > -1 && index == true) {
                                result = true;
                            }
                        });
                    }
                    if (result == false || Object.keys(tempJson).length === 0) {
                        var sysPatID = $(element).closest("li").find("#systemPatientID").val();
                        if ((typeof ($(element).closest("li").find("#isCharacteristicsExists").val()) != 'undefined' && $(element).closest("li").find("#isCharacteristicsExists").val()) != '') {
                            ReviewOfSystemsDataTemplateDetail.rOSDataSystemReset_DBCall(sysPatID).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(element).closest("li").removeClass('green');
                                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #hfROSSystemInfoID').val('-1');
                                }
                            });
                        }
                    }
                }
            }
        });
        objDeffered.resolve();
        return objDeffered;
    },
    // DB Call to Reset ROS System Patient
    rOSDataSystemReset_DBCall: function (sysPatId) {
        var objData = new Object();
        objData["ROSDataSystemID"] = sysPatId;
        objData["ROSDataTempInfoId"] = ReviewOfSystemsDataTemplateDetail.params.ROSDataTempInfoId == null ? -1 : ReviewOfSystemsDataTemplateDetail.params.ROSDataTempInfoId;

        objData["commandType"] = "ros_data_system_reset";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemDataTemplate", "ReviewOfSystemDataTemplate");
    },
    // Author: Khaleel Ur Rehman.
    // Purpose : To hide and disable Detail of a Characteristic After Save and Update.
    // Date : 17-Feb-2016.
    hideDetailAfterSaveUpdate: function () {
        ReviewOfSystemsDataTemplateDetail.handleDetailToggleDiv();
        if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #ActiveSystemUL > li").hasClass('active')) {
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #ActiveSystemUL > li").removeClass('active');
        }
    },

    rosTemplateLoad: function (ROSTemplateId) {

        ReviewOfSystemsDataTemplateDetail.searchROSTemplate_DBCall(ROSTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                ReviewOfSystemsDataTemplateDetail.rosTemplateDataLoad(response);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    rosTemplateDataLoad: function (response) {
        var ROSTemplateLoadJSONData = JSON.parse(response.ROSTemplateLoad_JSON);
        $.each(ROSTemplateLoadJSONData, function (i, item) {
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' input#txtTemplateName').val(item.TemplateName);
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlEntity').val(item.EntityId);
            //$.when(ReviewOfSystemsDataTemplateDetail.isEntitySelected(ReviewOfSystemsDataTemplateDetail.isSuperAdmin())).then(function () {
            //    ReviewOfSystemsDataTemplateDetail.enableDisableDropDownLists('ddlSpecialty,ddlProvider', false);
            //    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('clearSelection', false);
            //    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
            //    if (item.IsSpecialtyAll == "False") {
            //        // Set the value                
            //        EMRUtility.selectOptionsByCommaSeprateValue($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlSpecialty'), item.SpecialtyIds);
            //        // Then refresh
            //        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect("refresh");
            //    } else {
            //        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('selectAll', false);
            //        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlSpecialty').multiselect('updateButtonText');
            //    }
            //    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('clearSelection', false);
            //    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
            //    if (item.IsProviderAll == "False") {
            //        // Set the value
            //        EMRUtility.selectOptionsByCommaSeprateValue($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlProvider'), item.ProviderIds);
            //        // Then refresh
            //        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlProvider').multiselect("refresh");
            //    } else {
            //        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('selectAll', false);
            //        $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlProvider').multiselect('updateButtonText');
            //    }
            //});
            //  ReviewOfSystemsDataTemplateDetail.isEntitySelected($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ddlEntity'));
        });
    },


    ///////////////////// *********** DB CALLS *********** ////////////////////////


    getROSPatientInfo_DBCall: function () {
        var objData = new Object();
        objData["ROSSystemInfoID"] = ReviewOfSystemsDataTemplateDetail.params.ROSDataTempInfoId == null ? -1 : ReviewOfSystemsDataTemplateDetail.params.ROSDataTempInfoId;
        objData["ROSDataTemplateId"] = ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId == null ? -1 : ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId;
        objData["commandType"] = "load_ros_systems_patientinfo";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },



    /*  Author: Muhammad Azhar Shahzad
Purpose: for Grid Load of Ros template
Creation Date: March 02,2016 */
    searchROSTemplate_DBCall: function (ROSTemplateId) {
        var objData = {};
        objData["ROSTemplateId"] = ROSTemplateId == null ? -1 : ROSTemplateId;
        objData["PageNumber"] = 1;
        objData["IsActive"] = null;
        objData["RowsPerPage"] = 1;
        objData["commandType"] = "search_ros_systems_template";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemTemplate", "ReviewOfSystemTemplate");
    },

    getROSTemplateSystems_DBCall: function (ROSTemplateId) {
        var objData = new Object();
        objData["ROSTemplateId"] = (ROSTemplateId == null) ? 0 : ROSTemplateId;
        objData["ROSSystemInfoID"] = Clinical_Notes.params.ROSSystemInfoID;
        objData["commandType"] = "load_ros_template_systems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },


    // DB call to load all Systems
    //  Author: ZeeshanAK | Date: March 31, 2016
    getROSSystems_DBCall: function (ROSTemplateId, ROSDataTemplateId) {
        var objData = new Object();
        objData["ROSTemplateId"] = (ROSTemplateId == null) ? 0 : ROSTemplateId;
        objData["ROSSystemInfoID"] = Clinical_Notes.params.ROSSystemInfoID;
        if (ROSDataTemplateId == null) {
            objData["ROSDataTemplateId"] = Clinical_ReviewofSystems.params.ROSDataTemplateId == null ? -1 : Clinical_ReviewofSystems.params.ROSDataTemplateId;
        } else {
            objData["ROSDataTemplateId"] = ROSDataTemplateId;
        }

        objData["commandType"] = "load_ros_systems";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },


    getROSSystemsCharacteristics_DBCall: function (SystemId, ROSSystemPatientID) {
        var ROSDataTemplateId = ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId
        var objData = new Object();
        objData["ROSSystemPatientID"] = ROSSystemPatientID;
        objData["ROSDataTemplateId"] = (ROSDataTemplateId == null) ? -1 : ROSDataTemplateId;
        objData["ROSSystemId"] = SystemId;
        objData["commandType"] = "load_ros_systems_characteristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },

    //DB call to load Charcteristics Detail
    //  Author: ZeeshanAK | Date: April 01, 2016
    getSystemPatientCharcDetail_DBCall: function (charcDetailId) {
        var ROSDataTemplateId = ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId
        var objData = new Object();
        objData["ROSDataTemplateId"] = (ROSDataTemplateId == null) ? -1 : ROSDataTemplateId;
        objData["ROSSystemPatientCharacteristicsID"] = charcDetailId;
        objData["commandType"] = "load_Characteristic_Detail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },

    // DB call to Delete Characteristics Details Info
    //  Author: ZeeshanAK | Date: April 01, 2016
    deleteROSDataSystemCharcDetail_DBCall: function (charcDetailId) {
        var objData = new Object();
        objData["RemoveSystemCharcDetails"] = true
        objData["ROSDataSystemCharcID"] = charcDetailId;
        objData["commandType"] = "DELETE_ROS_DATA_SYSTEM_CHARC_DETAILS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemDataTemplate", "ReviewOfSystemDataTemplate");
    },

    // this function is used by both Notes and Progress Note Form
    showRosTemplateSaveAs: function (ActionPanID) {

        $(ActionPanID).prepend($("#ReviewOfSystemsDataTemplateDetail_SaveAs").html());
        $(ActionPanID).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false

        }).on('hidden.bs.modal', function () {
            $('body').addClass('modal-open');
        })
.on('shown.bs.modal', function () {
    ReviewOfSystemsDataTemplateDetail.validateROSTemplateSaveAs(ActionPanID);
});


    },

    validateROSTemplateSaveAs: function (ActionPanID) {
        $(ActionPanID + ' #frmROSDataTemplateDetailSaveAs').bootstrapValidator('destroy');
        $(ActionPanID + ' #frmROSDataTemplateDetailSaveAs')
          .bootstrapValidator({
              live: 'enabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  TemplateNameSaveAs: {
                      group: '.col-sm-4',
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
           ReviewOfSystemsDataTemplateDetail.saveAsRosDataTemplate();
       });
    },
    saveAsRosDataTemplate: function () {
        if (ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId <= 0) {
            utility.DisplayMessages('You cannot perform this action in new add mode.', 3);
        } else {
            $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #txtTemplateName').val($('#txtTemplateNameSaveAs').val());
            // if ($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ROSSystemsUL li').find('[type=checkbox]:checked').length > 0) {
            $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSTemplate #hfIsFormHasChange').val('true');
            ReviewOfSystemsDataTemplateDetail.ReviewofSystemsSave(false, true);
            ReviewOfSystemsDataTemplateDetail.unloadReviewOfSystemsDataTemplateDetailSaveAs();
            //} else {
            //    utility.DisplayMessages('Please select at least one System to save/update Template', 4);
            //}
        }
    },
    unloadReviewOfSystemsDataTemplateDetailSaveAs: function () {
        var ActionPanId = '#pnlClinicalReviewofSystemsDataTemplateDetail #actionPanClinicalReviewofSystemsDataTemplateDetail';


        $(ActionPanId).modal('hide');

        setTimeout(function () {
            $(ActionPanId).find('div').first().remove();
        }, 300);


    },

    resetRosDataTemplate: function () {
        if ($("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val() != '') {
            utility.myConfirm('26', function () {
                ReviewOfSystemsDataTemplateDetail.CharacteristicsInfo = [];
                //ReviewOfSystemsDataTemplateDetail.numOfCharsChecked = 0;

                if (ReviewOfSystemsDataTemplateDetail.params["mode"] = "Edit" && ReviewOfSystemsDataTemplateDetail.params.ROSTemplateId > 0) {
                    //  ReviewOfSystemsDataTemplateDetail.rosTemplateLoad(ReviewOfSystemsDataTemplateDetail.params.ROSTemplateId);
                    // ReviewOfSystemsDataTemplateDetail.loadSystems(ReviewOfSystemsDataTemplateDetail.params.ROSTemplateId);
                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #btnSaveAs').show();
                } else {
                    // ReviewOfSystemsDataTemplateDetail.loadSystems(ReviewOfSystemsDataTemplateDetail.params.ROSTemplateId);
                    $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #btnSaveAs').hide();
                }
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #ActiveSystemUL').empty();
                $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #divReviewofSystemsSystemSection div:not(#ActiveSystemUL)').remove();
                $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #hfIsFormHasChange').val('');
                $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #txtTemplateName').val('');
                $("#" + ReviewOfSystemsDataTemplateDetail.params["PanelID"] + ' #frmROSDataTemplateDetail #txtROSComments').val('');
                ReviewOfSystemsDataTemplateDetail.resetDOMcontrolsROS();
                ReviewOfSystemsDataTemplateDetail.loadROSSystemInfo();
            }, function () { });
        }
    },

    saveReviewofSystems_DbCall: function (isSaveAS) {

        var objData = {
        };
        var isNormal = []
        , isNormalDescription = []
        , SystemsWithDetails = []
        , isAllNegative = []
        , isAllPositive = []
        , charcDescNeg = []
        , charcDescPos = []
        , charcPosSystem = []
        , charcPosCharacteristics = []
        , charcPosName = []
        , charcNegName = []
        , charcNegSystem = []
        , charcNegCharacteristics = [];
        var SystemIDs = $.map($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
            return n.split('_')[2];
        });
        var SystemNames = $.map($('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
            var sName = n.split('_')[1];
            var sID = n.split('_')[2];
            if (ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName] != null) {
                objData[sName] = ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName];
                var tempJson = JSON.parse(ReviewOfSystemsDataTemplateDetail.CharcSystemInfo[sName]);
                if (tempJson['ReviewofSystemsSectionNormal' + "_" + sID] != null && (tempJson['ReviewofSystemsSectionNormal' + "_" + sID] == true || tempJson['ReviewofSystemsSectionNormal' + "_" + sID] == "True")) {
                    isNormal.push(sID);

                    //Start || 11 April, 2016 || ZeeshanAK || Fix for Bug EMR-637

                    //if (tempJson.hdnDescriptionNormal != null) {
                    //    isNormalDescription.push(decodeURI(tempJson['hdnDescriptionNormal']));
                    //} else {
                    //    isNormalDescription.push(tempJson.hdnDescriptionNormal);
                    //}
                    if (tempJson.hdnDescriptionNormal != null) {
                        isNormalDescription.push(decodeURI(tempJson['hdnDescriptionNormal']));
                    } else {
                        isNormalDescription.push(tempJson['SystemNormalDescription_' + sID]);
                    }
                    //End   || 11 April, 2016 || ZeeshanAK || Fix for Bug EMR-637

                } else {
                    $.map(tempJson, function (index, item) {
                        if (item.indexOf('Charc_Neg_') > -1 && index == true) {
                            var splitStringCharc = item.split('_');
                            if (splitStringCharc.length > 2 && splitStringCharc[2] != null) {

                                charcNegSystem.push(splitStringCharc[2]);
                            }
                            if (splitStringCharc.length > 3 && splitStringCharc[3] != null) {
                                charcNegCharacteristics.push(splitStringCharc[3]);
                                charcNegName.push($('#' + item).parent().parent().text());
                            }
                            charcDescNeg.push(tempJson['Charc_Desc_' + splitStringCharc[2] + "_" + splitStringCharc[3]]);

                        } else if (item.indexOf('Charc_Pos_') > -1 && index == true) {
                            var splitStringCharc = item.split('_');
                            if (splitStringCharc.length > 2 && splitStringCharc[2] != null) {
                                charcPosSystem.push(splitStringCharc[2]);
                            }
                            if (splitStringCharc.length > 3 && splitStringCharc[3] != null) {
                                charcPosCharacteristics.push(splitStringCharc[3]);
                                charcPosName.push($('#' + item).parent().parent().text());
                            }
                            charcDescPos.push(tempJson['Charc_Desc_' + splitStringCharc[2] + "_" + splitStringCharc[3]]);
                        }
                    });
                }
                if (tempJson['selectAllPositive' + "_" + sID] != null && (tempJson['selectAllPositive' + "_" + sID] == true || tempJson['selectAllPositive' + "_" + sID] == "True")) {
                    isAllPositive.push(sID);
                } else if (tempJson['selectAllNegative' + "_" + sID] != null && (tempJson['selectAllNegative' + "_" + sID] == true || tempJson['selectAllNegative' + "_" + sID] == "True")) {
                    isAllNegative.push(sID);
                }
            }
            return sName;
        });

        if (!$('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #chkReviewofSystemssNormal').is(':checked')) {
            var DetailInfoObj = [];
            $(ReviewOfSystemsDataTemplateDetail.CharacteristicsDetailsInfo).each(function (index, item) {
                var objDetail = JSON.parse(item.DetailInfo);
                objDetail["ROSSystemID"] = item.SystemId;
                objDetail["ROSCharacteristicsId"] = item.CharcId;
                objDetail["ROSCharacteristicsDetailsId"] = objDetail.hfROSCharacteristicsDetailsId;
                DetailInfoObj.push(objDetail);

            });
        }
        objData["AllPositiveSystems"] = isAllPositive;
        objData["AllNegativeSystems"] = isAllNegative;
        objData["charcPosSystem"] = charcPosSystem;
        objData["charcDescNeg"] = charcDescNeg;
        objData["charcDescPos"] = charcDescPos;
        objData["charcPosName"] = charcPosName;
        objData["charcNegName"] = charcNegName;
        objData["charcNegSystem"] = charcNegSystem;
        objData["charcNegCharacteristics"] = charcNegCharacteristics;
        objData["charcPosCharacteristics"] = charcPosCharacteristics;
        objData["rosPatientCharcObjList"] = DetailInfoObj;
        objData["ROSSystemIds"] = SystemIDs;
        objData["ROSSystemNames"] = SystemNames;
        objData["NormalSystems"] = isNormal;
        objData["isNormalDescription"] = isNormalDescription;
        objData["ReviewofSystemsDate"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #dtReviewofSystemsDate').val();
        objData["ROSisNormal"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #chkReviewofSystemssNormal').is(':checked');
        objData["ROSNormalDescription"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #hdnDescriptionNormalGlobal').val();
        objData["ROSComments"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #txtROSComments').val();
        objData["PatientId"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #hfPatientId').val();
        objData["ROSSystemPatSortingOrder"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul [name=systemPatientID]").map(function (i, n) { if (n.value != '-1') { return n.value; } }).get().join();//$('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray");
        objData["systemSortingOrder"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul li").map(function (i, n) { return n.id.split('_')[2] }).get().join();
        objData["SystemsWithDetails"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " div#divReviewofSystemsSystems ul li.green").map(function (i, n) {
            if ($(n).find('[name=isSystemNormal]').val() != 'True') {
                return $(n).find('[name=systemPatientID]').val()
            }
        }).get();
        objData["ROSSystemInfoID"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + ' #hfROSSystemInfoID').val();
        var ROSDataTempInfoId = ReviewOfSystemsDataTemplateDetail.params.ROSDataTempInfoId == null ? -1 : ReviewOfSystemsDataTemplateDetail.params.ROSDataTempInfoId;
        objData["ROSDataTempInfoId"] = ROSDataTempInfoId;
        objData["ROSSystemPatientID"] = -1;
        var ROSDataTemplateId = ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId == null ? -1 : ReviewOfSystemsDataTemplateDetail.params.ROSDataTemplateId;
        objData["ROSDataTemplateId"] = ROSDataTemplateId;
        objData["ROSTemplateId"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #hfROSTemplateId").val();
        objData["DataTemplateName"] = $('#' + ReviewOfSystemsDataTemplateDetail.params.PanelID + " #frmROSDataTemplateDetail #txtTemplateName").val();
        objData["isSaveAS"] = isSaveAS == null ? false : true;
        objData["commandType"] = "save_rostemplate";
        objData["SystemsWrapperString"] = JSON.stringify(objData);
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "ReviewOfSystemDataTemplate", "ReviewOfSystemDataTemplate");
    },
}


// Handle the extra space between Characteristics and its details
function countWidth(obj) {


    var panelChildrens = null;
    var currentPanel = null;
    var applyWidth = null;
    if (obj != null) {
        currentPanel = $(obj.delegateTarget).parent();
        panelChildrens = currentPanel.children("section.panel");
        applyWidth = currentPanel;
        countWidthApply(currentPanel, panelChildrens, applyWidth);
    }
    else {
        $('.toggleVertical').each(function (e) {
            currentPanel = $(this);
            panelChildrens = currentPanel.children().children("section.panel");
            applyWidth = currentPanel.children();
            countWidthApply(currentPanel, panelChildrens, applyWidth);
        });


    }
}

// Apply the calculated width 
function countWidthApply(currentPanel, panelChildrens, applyWidth) {
    var totalWidth = 0;
    var hidden = 0;
    //find total width of panels
    panelChildrens.each(function (e) {
        totalWidth += $(this).outerWidth(true);
    });
    //find total width of hidden panel
    currentPanel.find(":hidden.panel").each(function (e) {
        hidden += $(this).outerWidth(true);
    });
    //apply width to div
    applyWidth.width((totalWidth - hidden + 50) + "px");
}
