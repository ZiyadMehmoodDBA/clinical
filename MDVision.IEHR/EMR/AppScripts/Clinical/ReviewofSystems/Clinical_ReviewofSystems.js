Clinical_ReviewofSystems = {
    //Author: Muhammad Azhar Shahzad
    //Date: January 26, 2016
    //This file will handle all actions performed for Review Of Systems and it's child handling
    //Once ReviewofSystems will be created then it's child can be created then.
    bIsFirstLoad: true,
    params: [],
    CharacteristicsDetailsInfo: [],
    CharcSystemInfo: [],
    CharcDetailDivJSON: '',
    controlToInvoke: null,
    bNextPrev: false,

    //Author: Muhammad Azhar Shahzad
    //Date: January 26, 2016
    //This function will be called once tab is clicked, it expects parameters to be used for ReviewofSystems
    Load: function (params) {
        Clinical_ReviewofSystems.params = params;

        Clinical_ReviewofSystems.params.mode = Clinical_ReviewofSystems.params.mode || "Add";

        if (Clinical_ReviewofSystems.params.PanelID != 'pnlClinicalReviewofSystems') {
            Clinical_ReviewofSystems.params.PanelID = Clinical_ReviewofSystems.params.PanelID + ' #pnlClinicalReviewofSystems';
        }

        //Code for progress note navigation
        if (Clinical_ReviewofSystems.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_ReviewofSystems.params.PanelID, '', 'ReviewofSystems', 'Clinical_ReviewofSystems.unLoadTab(true);', null, true);
        }

        Clinical_ReviewofSystems.params.ROSDataTemplateId = $('#hfROSDataTemptId').val();
        $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfPatientId").val($("div#PatientProfile #hfPatientId").val());

        Clinical_ReviewofSystems.domReadyFunction();

        Clinical_ReviewofSystems.loadROSSystemInfo();
    },

    clearInfoForResetReviewofSystems: function () {
        var ROSInfoId = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfROSSystemInfoID').val();
        if (ROSInfoId != "" && Number(ROSInfoId) > 0 && Clinical_Notes.params.NotesId > 0) {
            Clinical_ReviewofSystems.createReviewofSystemsBodyHTML('', '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, null);
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_ReviewofSystems').closest('li').remove();
            Clinical_ProgressNote.saveComponentSOAPText('Review of Systems');
            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
        }
        var formControlId = '#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems';
        EMRUtility.resetControlValue($(formControlId + ' #divExamCharacteristics'));
        EMRUtility.resetControlValue($(formControlId + ' #divReviewofSystemsSystems'));
        EMRUtility.resetControlValue($(formControlId + ' #divReviewofSystemsSystemSection'));

        $(formControlId + ' #chkReviewofSystemssNormal').prop('checked', false);
        $(formControlId + ' #divReviewofSystemsSystems ul li').find('span:first-child').removeClass("green");
        $(formControlId).find('[id=bookIconNormal],[id=bookIconNormalGlobal]').addClass('disabled');
        $(formControlId + ' #divReviewofSystemsSystemSection').empty();
        $($(formControlId + ' #divReviewofSystemsSystems').parent()).find("li").each(function () {
            $(this).removeClass("active");
            $(this).removeClass("green");
        });
        $(formControlId + ' #hfIsFormHasChange').val('');

        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #txtROSComments').val('');
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' div.toggle').prop('disabled', 'disabled');

        Clinical_ReviewofSystems.clearSearchBox();
        Clinical_ReviewofSystems.unmarkAllSystemsAsNormalCache();
        Clinical_ReviewofSystems.clearCachingROS();

        if ($('#' + Clinical_ReviewofSystems.params.PanelID + ' #divNormalGlobal').next().find('textarea').length > 0) {
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divNormalGlobal').next().remove();
        }

        utility.CreateDatePicker(Clinical_ReviewofSystems.params.PanelID + '  #dtReviewofSystemsDate', function () { }, true);
        Clinical_ReviewofSystems.handleDetailToggleDiv();
    },

    //By Khaleel Ur Rehman for Reset functionality. Date : 04 Feb 2016.
    resetReviewofSystems: function (resetAll) {
        var ROSInfoId = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfROSSystemInfoID').val();
        if (ROSInfoId != "" && Number(ROSInfoId) > 0 && Clinical_Notes.params.NotesId > 0) {
            resetAll = true;
        } else {
            resetAll = false;
        }
        if (resetAll != null && resetAll || $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val() == 'true') {
            utility.myConfirm('26', function () {

                if (ROSInfoId != "" && Number(ROSInfoId) > 0 && Clinical_Notes.params.NotesId > 0) {
                    Clinical_ReviewofSystems.disAssociateSystemsAgainstNoteId().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_ReviewofSystems.clearInfoForResetReviewofSystems();
                            Clinical_ReviewofSystems.loadROSSystemInfo();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    Clinical_ReviewofSystems.clearInfoForResetReviewofSystems();
                }
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').data('serialize', $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').serialize());
            }, function () { });
        }
    },

    //Author: Muhammad Azhar Shahzad
    //Date: January 26, 2016
    //This function will handle Initialization of KeyPad control
    domReadyFunction: function () {
        utility.CreateDatePicker(Clinical_ReviewofSystems.params.PanelID + '  #dtReviewofSystemsDate', function () { }, true);

        $('#' + Clinical_ReviewofSystems.params.PanelID).loadDropDowns(true);

        Clinical_ReviewofSystems.clearCachingROS();

        // Change for hiding Add to note button on template selection screen
        $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #actionButtons').hide();

        $('#' + Clinical_ReviewofSystems.params.PanelID + " .toggleVertical div.toggle").click(function (e) {
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
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' [data-plugin-toggle]').each(function () {
            var $this = $(this),
                opts = {
                };

            var pluginOptions = $this.data('plugin-options');
            if (pluginOptions)
                opts = pluginOptions;

            $this.themePluginToggle(opts);
        });
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').on('change', function () {
            $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('true');
        });
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #ReviewofSystems #Duration').keyboard({
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
                    $('#' + Clinical_ReviewofSystems.params.PanelID + ' #ReviewofSystems #Duration').focus();

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
         .addTyping()
         .keydown(function (e) {
             var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
             if (keyCode == 9) {
                 $('#' + Clinical_ReviewofSystems.params.PanelID + ' #ReviewofSystems #Duration').next().hide();
                 $('#' + Clinical_ReviewofSystems.params.PanelID + ' #ReviewofSystems #divExamCharacteristics #ROSCharacteristicsDetailDurationId').focus();
                 $('#' + Clinical_ReviewofSystems.params.PanelID + ' #ReviewofSystems #Duration').removeClass('ui-keyboard-input-current');
             }
         });
    },


    // Author: Muhammad Azhar Shahzad
    // ROS save. Checks the state if it is Add or Edit and proceeds accordingly
    ReviewofSystemsSave: function (UnloadReviewofSystems, IsSaveAs) {
        var ROSDateExists = false;
        var rosDate = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #dtReviewofSystemsDate").val();
        if (rosDate != null && rosDate != '') {
            ROSDateExists = true;
        }
        if (ROSDateExists) {
            if ($("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val() != '') {
                Clinical_ReviewofSystems.updatingCommentsOfNormal();
                // set time out to allow blur fuction triggers first
                setTimeout(function () {
                    // caching current selected System informations and characteristics Details Information
                    Clinical_ReviewofSystems.CacheCharacteristicDetailInfo(true);
                    var CurrentSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li.active");

                    if (CurrentSystem.find('[name=isSystemNormal][type=hidden]').val() == "True") {
                        Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, true, false);
                    } else {
                        Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, false, false);
                    }
                    //end caching current selected System informations and characteristics Details Information

                    $.when(Clinical_ReviewofSystems.existingUncheckAll()).then(function () {

                        Clinical_ReviewofSystems.saveReviewofSystems_DbCall(IsSaveAs).done(function (response) {

                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (IsSaveAs == null || IsSaveAs == false) {
                                    Clinical_ReviewofSystems.params.mode = "Edit";

                                    //binding SOAP text with NOte
                                    if (Clinical_ReviewofSystems.params.ParentCtrl == "clinicalTabProgressNote") {
                                        Clinical_ReviewofSystems.getReviewofSystemsInfo(response.soapTextROS, UnloadReviewofSystems);
                                    } else {
                                        utility.DisplayMessages(response.Message, 1);
                                    }

                                    Clinical_ReviewofSystems.hideDetailAfterSaveUpdate();

                                    Clinical_ReviewofSystems.clearCachingROS();

                                    if (UnloadReviewofSystems != null && UnloadReviewofSystems == false) {
                                        Clinical_ReviewofSystems.loadROSSystemInfo(true);
                                    }
                                } else {
                                    Clinical_ReviewofSystems.loadDataTemplateSystems(0);
                                }
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    });
                }, 5);
            } else {
                utility.DisplayMessages("Please make any changes to save/update ROS Systems Information.", 3);
            }
        }
        else {
            utility.DisplayMessages("Please enter Review of System Date value", 3);
        }
    },
    loadROSAgainstDataTemplateFromProviderNotes: function (ROSDataTemplateId, TemplateID, NotesID) {
        var dfd = $.Deferred();
        Clinical_ReviewofSystems.clearCachingROS();
        Clinical_ReviewofSystems.params.ROSTemplateId = -1;
        Clinical_ReviewofSystems.params.ROSDataTemplateId = ROSDataTemplateId;
        Clinical_ReviewofSystems.params.isShowTemplate = false;

        Clinical_ReviewofSystems.getROSSystemsFromProvidersNote_DBCall(ROSDataTemplateId, TemplateID, NotesID).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.response = response;
                dfd.resolve();
            }
            else {
                dfd.response = "";
                dfd.resolve();
            }
        });
        return dfd;
    },

    // Author: Khaleel Ur Rehman.
    // Purpose : To hide and disable Detail of a Characteristic After Save and Update.
    // Date : 17-Feb-2016.
    hideDetailAfterSaveUpdate: function () {
        Clinical_ReviewofSystems.handleDetailToggleDiv();
        if ($('#' + Clinical_ReviewofSystems.params.PanelID + " #ActiveSystemUL > li").hasClass('active')) {
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #ActiveSystemUL > li").removeClass('active');
        }
    },

    // Checks if previously checked box is unchecked or not
    existingUncheckAll: function () {
        var objDeffered = $.Deferred();
        $('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul [type=hidden][name=isCharacteristicsExists]:not([value=''])").each(function (index, element) {
            var SystemName = $(element).closest('li').text();
            var sID = $($(element).closest('li')[0]).attr('id').split('_')[2];
            if (Clinical_ReviewofSystems.CharcSystemInfo[SystemName] != null) {
                var tempJson = JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[SystemName]);
                var result = false;
                if (tempJson != null) {
                    var isnormal = tempJson['ReviewofSystemsSectionNormal' + "_" + sID];
                    if (isnormal && (isnormal == true || isnormal == "True")) {
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
                    if (result == false) {
                        var sysPatID = $(element).closest("li").find("#systemPatientID").val();
                        if ((typeof ($(element).closest("li").find("#isCharacteristicsExists").val()) != 'undefined' && $(element).closest("li").find("#isCharacteristicsExists").val()) != '') {
                            Clinical_ReviewofSystems.rOSSystemPatientReset_DBCall(sysPatID).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    $(element).closest("li").removeClass('green');
                                    $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfROSSystemInfoID').val('-1');
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

    //************************************************
    //start save as functions
    SaveAsDataTemp: function () {
        Clinical_ReviewofSystems.ReviewofSystemsSave(false, true);
    },

    showRosTemplateSaveAs: function () {
        var ActionPanID = '#' + Clinical_ReviewofSystems.params.PanelID + " #actionPanClinicalReviewofSystems";
        $(ActionPanID).prepend($("#ReviewofSystems_DataTemp_SaveAs").html());
        $(ActionPanID).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false

        }).on('hidden.bs.modal', function () {
            $('body').addClass('modal-open');
        })
        .on('shown.bs.modal', function () {
            Clinical_ReviewofSystems.validateROSTemplateSaveAs(ActionPanID);
        });
    },

    validateROSTemplateSaveAs: function (ActionPanID) {
        $(ActionPanID + ' #frmROSDataTemplateSaveAs').bootstrapValidator('destroy');
        $(ActionPanID + ' #frmROSDataTemplateSaveAs').bootstrapValidator({
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
                }
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            Clinical_ReviewofSystems.saveAsRosDataTemplate();
        });
    },
    saveAsRosDataTemplate: function () {
        //Start || 12 July, 2016 || ZeeshanAK || Fix for Data modified message popping up unnecessary.
        if (Clinical_ReviewofSystems.params.ROSDataTemplateId <= 0 && $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSTemplateId").val() <= 0) {
            utility.DisplayMessages('You cannot perform this action in new add mode.', 3);
        }
            //End   || 12 July, 2016 || ZeeshanAK || Fix for Data modified message popping up unnecessary.
        else {
            $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('true');
            Clinical_ReviewofSystems.SaveAsDataTemp();
            Clinical_ReviewofSystems.unloadReviewOfSystemsDataTemplateDetailSaveAs();
        }
    },
    unloadReviewOfSystemsDataTemplateDetailSaveAs: function () {
        var ActionPanId = '#' + Clinical_ReviewofSystems.params.PanelID + " #actionPanClinicalReviewofSystems";
        $(ActionPanId).modal('hide');
        setTimeout(function () {
            $(ActionPanId).find('div').first().remove();
        }, 300);
    },
    //end save as function
    //***************************************

    // DB call to load all Systems
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
        , charcNegCharacteristics = []
        , UncheckedCharacteristics = [];
        var SystemIDs = $.map($('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
            return n.split('_')[2];
        });
        var SystemNames = $.map($('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
            var sName = n.split('_')[1];
            var sID = n.split('_')[2];
            if (Clinical_ReviewofSystems.CharcSystemInfo[sName] != null) {
                objData[sName] = Clinical_ReviewofSystems.CharcSystemInfo[sName];
                var tempJson = JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[sName]);
                var isnormal = tempJson['ReviewofSystemsSectionNormal' + "_" + sID];
                if (isnormal != null && (isnormal == true || isnormal == "True")) {
                    isNormal.push(sID);
                    //Start || 11 April, 2016 || ZeeshanAK || Fix for Bug EMR-637
                    if (tempJson.hdnDescriptionNormal != null) {
                        isNormalDescription.push(decodeURIComponent(escape(tempJson['hdnDescriptionNormal'])));
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
                        } else if (index == false) {
                            var splitStringCharc = item.split('_');
                            if (splitStringCharc.length > 3 && splitStringCharc[3] != null && UncheckedCharacteristics.indexOf(splitStringCharc[3]) == -1) {
                                UncheckedCharacteristics.push(splitStringCharc[3]);
                            }
                        }
                    });
                }
                var isselectAllPositive = tempJson['selectAllPositive' + "_" + sID];
                var isselectAllNegative = tempJson['selectAllNegative' + "_" + sID];

                if (isselectAllPositive != null && (isselectAllPositive == true || isselectAllPositive == "True")) {
                    isAllPositive.push(sID);
                } else if (isselectAllNegative && (isselectAllNegative == true || isselectAllNegative == "True")) {
                    isAllNegative.push(sID);
                }
            }
            return sName;
        });
        if ((charcNegCharacteristics != null && charcNegCharacteristics.length > 0)
            || (charcPosCharacteristics != null && charcPosCharacteristics.length > 0)) {
            var tempArray = []
            $.each(UncheckedCharacteristics, function (index, item) {
                if (!((charcNegCharacteristics != null && charcNegCharacteristics.indexOf(item) != -1) || (charcPosCharacteristics != null && charcPosCharacteristics.indexOf(item) != -1))) {
                    tempArray.push(item);
                }
            });
            UncheckedCharacteristics = tempArray;
        }

        if (!$('#' + Clinical_ReviewofSystems.params.PanelID + ' #chkReviewofSystemssNormal').is(':checked')) {
            var DetailInfoObj = [];
            $(Clinical_ReviewofSystems.CharacteristicsDetailsInfo).each(function (index, item) {
                var objDetail = JSON.parse(item.DetailInfo);
                objDetail["ROSSystemID"] = item.SystemId;
                objDetail["ROSCharacteristicsId"] = item.CharcId;
                objDetail["ROSCharacteristicsDetailsId"] = objDetail.hfROSCharacteristicsDetailsId;
                objDetail["NotesId"] = Clinical_ReviewofSystems.params.NotesId;
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
        objData["UncheckedCharacteristics"] = UncheckedCharacteristics;
        objData["rosPatientCharcObjList"] = DetailInfoObj;
        objData["ROSSystemIds"] = SystemIDs;
        objData["ROSSystemNames"] = SystemNames;
        objData["NormalSystems"] = isNormal;
        objData["isNormalDescription"] = isNormalDescription;
        objData["ReviewofSystemsDate"] = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #dtReviewofSystemsDate').val();
        objData["ROSisNormal"] = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #chkReviewofSystemssNormal').is(':checked');
        objData["ROSNormalDescription"] = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hdnDescriptionNormalGlobal').val();
        objData["ROSComments"] = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #txtROSComments').val();
        objData["PatientId"] = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfPatientId').val();
        objData["NotesId"] = Clinical_ReviewofSystems.params.NotesId;
        objData["ROSSystemPatSortingOrder"] = $('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul [name=systemPatientID]").map(function (i, n) { if (n.value != '-1') { return n.value; } }).get().join();//$('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray");
        objData["systemSortingOrder"] = $('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul li").map(function (i, n) {
            return n.id.split('_')[2]
        }).get().join();
        objData["SystemsWithDetails"] = $('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul li.green").map(function (i, n) {
            if ($(n).find('[name=isSystemNormal]').val() != 'True') {
                return $(n).find('[name=systemPatientID]').val()
            }
        }).get();
        objData["ROSSystemInfoID"] = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfROSSystemInfoID').val();
        objData["ROSSystemPatientID"] = 1;
        objData["ROSTemplateId"] = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSTemplateId").val();
        if (isSaveAS != null) {
            objData["isSaveAS"] = isSaveAS == null ? false : isSaveAS;
            objData["DataTemplateName"] = isSaveAS == null ? '' : $('#txtTemplateNameSaveAs').val();
            objData["SystemsWrapperString"] = JSON.stringify(objData);
        }
        objData["ROSDataTemplateId"] = Clinical_ReviewofSystems.getROSDataTemplateID();;

        objData["commandType"] = "SAVE_ReviewofSystems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewofSystem", "ReviewofSystems");
    },

    //----------------- Caching functions
    // Caches all Characteristics info for a selected system
    //Start || 26 April, 2016 || ZeeshanAK || Change made for fixing EMR-639
    CacheCharacteristicInfo: function (CurrentSystem, cacheAll, IsNormal, removeCache, SystemName, SystemID) {
        if (CurrentSystem == null || removeCache == true && SystemName != null) {
            Clinical_ReviewofSystems.CharcSystemInfo[SystemName] = [];
            var tempData = {
            };
            tempData['ReviewofSystemsSectionNormal' + "_" + SystemID] = "";
            tempData['SystemNormalDescription' + "_" + SystemID] = "";
            Clinical_ReviewofSystems.CharcSystemInfo[SystemName] = JSON.stringify(tempData);
        } else {
            var charcSystemDiv = '#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystemSection';
            if ($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #SystemSections').find('input[type=checkbox]:checked').length > 0) {// || IsNormal == true) {
                if (cacheAll != null && cacheAll == true) {
                    $.map($('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
                        var sName = n.split('_')[1];
                        var sID = n.split('_')[2];
                        if (removeCache != null && removeCache == true) {
                            if (Clinical_ReviewofSystems.CharcSystemInfo[sName] != null) {
                                var objData = JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[sName]);
                                Object.keys(objData).forEach(function (key, index) {
                                    // key: the name of the object key
                                    // index: the ordinal position of the key within the object
                                    objData[key] = '';
                                });
                                Clinical_ReviewofSystems.CharcSystemInfo[sName] = JSON.stringify(objData);
                            }
                        } else {
                            var obj = {
                            };
                            obj['ReviewofSystemsSectionNormal' + "_" + sID] = IsNormal;
                            if (Clinical_ReviewofSystems.CharcSystemInfo[sName] != null) {
                                var tempData = JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[sName]);
                                tempData['ReviewofSystemsSectionNormal' + "_" + sID] = IsNormal;
                                Clinical_ReviewofSystems.CharcSystemInfo[sName] = JSON.stringify(tempData);
                            } else {
                                Clinical_ReviewofSystems.CharcSystemInfo[sName] = JSON.stringify(obj);
                            }
                        }
                    });

                } else {
                    Clinical_ReviewofSystems.addgreenClassToSystem(charcSystemDiv, CurrentSystem);
                }

            } else {
                SystemNameData = unescape($(charcSystemDiv).getMyJSON());
                Clinical_ReviewofSystems.CharcSystemInfo[CurrentSystem.attr('title')] = SystemNameData;
                $(CurrentSystem).find('span:first-child').removeClass('green');
                if ($(CurrentSystem).hasClass('green')) {
                    $(CurrentSystem).removeClass('green');
                }
                $(charcSystemDiv).data(CurrentSystem.attr('title'), []);
            }
        }
    },

    addgreenClassToSystem: function (charcSystemDiv, CurrentSystem) {
        /* this check will check the caching state of system characteristics, if user has made any changes in characteristics
                Change Implemented By: Azhar Shahzad
                Date: januray 02, 2016 02:56pm */
        if ($(charcSystemDiv).data(CurrentSystem.attr('title')) != null && $(charcSystemDiv).data(CurrentSystem.attr('title')).length > 0) {
            if ($(charcSystemDiv).data(CurrentSystem.attr('title')) != $(charcSystemDiv).getMyJSON() || Clinical_ReviewofSystems.CharcSystemInfo[CurrentSystem] != $(charcSystemDiv).getMyJSON()) {
                SystemNameData = unescape($(charcSystemDiv).getMyJSON());
                Clinical_ReviewofSystems.CharcSystemInfo[CurrentSystem.attr('title')] = SystemNameData;
                $(CurrentSystem).find('span:first-child').addClass('green');
            }
        } else {
            SystemNameData = unescape($(charcSystemDiv).getMyJSON());
            Clinical_ReviewofSystems.CharcSystemInfo[CurrentSystem.attr('title')] = SystemNameData;
            $(CurrentSystem).find('span:first-child').addClass('green');
            if ($(CurrentSystem).hasClass('green')) {
                $(CurrentSystem).removeClass('green');
            }
            $(charcSystemDiv).data(CurrentSystem.attr('title'), []);
        }
        //end change januray 02, 2016 02:56pm
    },


    // Binds Characteristics details
    bindCacheCharacteristicDetailInfo: function (cntrl) {
        var charcId = cntrl.id;
        var detail = $.grep(Clinical_ReviewofSystems.CharacteristicsDetailsInfo, function (item, index) {
            return item.Id == charcId;
        });
        if ((detail != null && detail.length != 0)) {
            detailJson = JSON.parse(detail[0].DetailInfo);
            Clinical_ReviewofSystems.populateDetailAgainstCharacteristic(detailJson);
        }

    },

    // Caches all Characteristics details info for a selected system
    CacheCharacteristicDetailInfo: function (resetCache) {
        var activeSystemControl = '#' + Clinical_ReviewofSystems.params.PanelID + " #ActiveSystemUL > li.active";
        var ExamCharcControl = '#' + Clinical_ReviewofSystems.params.PanelID + ' #divExamCharacteristics';
        if ($(activeSystemControl).length > 0) {
            var charcId = $(activeSystemControl).attr('id');
            var charcDetailDiv = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divExamCharacteristics');
            var DetailData = unescape($(charcDetailDiv).getMyJSON());

            var detailDataParsed = JSON.parse(DetailData);
            if (detailDataParsed.Duration == '' || detailDataParsed.ROSCharacteristicsDetailDurationId_text == "- Select -") {
                detailDataParsed.Duration = '';
                detailDataParsed.ROSCharacteristicsDetailDurationId_text = '';
            }
            DetailData = JSON.stringify(detailDataParsed);

            if (resetCache != null && resetCache == true) {
                $(ExamCharcControl).resetAllControls();
            }
            Clinical_ReviewofSystems.CharcDetailDivJSON = unescape($(ExamCharcControl).clone().getMyJSON());

            Clinical_ReviewofSystems.CharcDetailDivJSON = unescape($(ExamCharcControl).clone().getMyJSON());

            var indexCh = -1;

            if (DetailData != Clinical_ReviewofSystems.CharcDetailDivJSON) {

                $.grep(Clinical_ReviewofSystems.CharacteristicsDetailsInfo, function (item, index) {
                    if (item.Id == charcId) {
                        indexCh = index;
                        return;
                    }
                });

                if (indexCh != -1) {
                    Clinical_ReviewofSystems.CharacteristicsDetailsInfo[indexCh].DetailInfo = DetailData;
                }
                else {
                    var Ids = charcId.split('_');

                    var CharsDetailInfo = {
                        Id: charcId.split(' ').join(''),
                        SystemId: Ids.length > 0 ? Ids[1] : null,
                        CharcId: Ids.length > 0 ? Ids[2] : null,
                        DetailInfo: DetailData
                    };
                    Clinical_ReviewofSystems.CharacteristicsDetailsInfo.push(CharsDetailInfo);
                }
            }
        }
    },

    // Resets the Systems cache
    resetSystemCacheInfo: function (systemName, SytemId, Cntrl) {
        var charcSystemDiv = '#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystemSection';
        var tempData = {
        };
        tempData['ReviewofSystemsSectionNormal' + "_" + SytemId] = "False";
        tempData['SystemNormalDescription' + "_" + SytemId] = "";
        Clinical_ReviewofSystems.CharcSystemInfo[systemName] = JSON.stringify(tempData);
        var CurrentSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #sectionReviewofSystems li.active");
        if (CurrentSystem != null && CurrentSystem.length > 0) {
            if ($(charcSystemDiv).data(CurrentSystem.attr('title')) != null && $(charcSystemDiv).data(CurrentSystem.attr('title')).length > 0) {
                if ($(charcSystemDiv).data(CurrentSystem.attr('title')) != $(charcSystemDiv).getMyJSON()) {
                    SystemNameData = unescape($(charcSystemDiv).getMyJSON());
                    Clinical_ReviewofSystems.CharcSystemInfo[CurrentSystem.attr('title')] = SystemNameData;
                    $(CurrentSystem).find('span:first-child').removeClass('green');
                }
            } else {
                SystemNameData = unescape($(charcSystemDiv).getMyJSON());
                Clinical_ReviewofSystems.CharcSystemInfo[CurrentSystem.attr('title')] = SystemNameData;
                $(CurrentSystem).find('span:first-child').removeClass('green');
                if ($(CurrentSystem).hasClass('green')) {
                    $(CurrentSystem).removeClass('green');
                }

            }
        }
    },

    // Resets the Characteristics details info cache
    resetCharcCacheDetailInfo: function (systemname, Cntrl, SystemId) {
        var detailObj = $.grep(Clinical_ReviewofSystems.CharacteristicsDetailsInfo, function (item, index) {
            return (item.Id == charcId);
        });

        Clinical_ReviewofSystems.CharacteristicsDetailsInfo.splice(detailObj, 1);
    },

    //--------------- end chaching functions

    loadTemplateSystems: function (ROSTemplateId) {
        Clinical_ReviewofSystems.getROSTemplateSystems_DBCall(ROSTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ROSTemptCount > 0) {
                    var SystemsJSON = JSON.parse(response.ROSTempt_JSON);

                    $.each(SystemsJSON, function (index, item) {
                        var $row = $('<tr/>');
                        $row.attr("onclick", "utility.SelectGridRow($('#gvTemplates_row" + item.ROSTemplateId + "'));Clinical_ReviewofSystems.loadROSAgainstTemplate(" + item.ROSTemplateId + ");");
                        $row.attr("id", "gvTemplates_row" + item.ROSTemplateId);
                        $row.append('<td style="display:none;">' + item.ROSTemplateId + '</td><td><a class="btn  btn-xs" href="#"  title="Select Template"><i class="fa fa-check black"></i></a>&nbsp;</td><td>' + item.TemplateName + '</td>');
                        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #dgvClinicalNotesTemplates tbody').last().append($row);
                    });

                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    loadDataTemplateSystems: function (ROSTemplateId) {
        Clinical_ReviewofSystems.searchROSDataTemplate_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ROSDataTemplateCount > 0) {

                    var ROSDataTemplateLoad_JSON = JSON.parse(response.ROSDataTemplateLoad_JSON);
                    //Start || 12 July, 2016 || ZeeshanAK || Change for refreshing Data Templates after Save As
                    $('#' + Clinical_ReviewofSystems.params.PanelID + ' #dgvClinicalNotesDataTemplates tbody').empty();
                    //End   || 12 July, 2016 || ZeeshanAK || Change for refreshing Data Templates after Save As

                    $.each(ROSDataTemplateLoad_JSON, function (index, item) {

                        var $row = $('<tr/>');
                        $row.attr("onclick", "utility.SelectGridRow($('#gvDataTemplates_row" + item.ROSDataTemplateId + "'));Clinical_ReviewofSystems.loadROSAgainstDataTemplate(" + item.ROSDataTemplateId + ");");
                        $row.attr("id", "gvDataTemplates_row" + item.ROSDataTemplateId);
                        $row.append('<td style="display:none;">' + item.ROSDataTemplateId + '</td><td><a class="btn  btn-xs" href="javascript:void(0)"  title="Select Data Template"><i class="fa fa-check black"></i></a>&nbsp;</td><td>' + item.DataTemplateName + '</td>');
                        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #dgvClinicalNotesDataTemplates tbody').last().append($row);
                    });

                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },
    /* Hide the Add button and show ROS Templates tab
    Author: ZeeshanAK | Date: March 11, 2016 */
    showROSTemplatesTab: function (TabID) {

        if (TabID == 'LiROSTemplates' && $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #viewTemplates').is(':visible') == false) {
            if ($("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val() == 'true') {
                var CurrentSystem = $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #divReviewofSystemsSystems').find("li.active");
                if (CurrentSystem.attr('title') != null && CurrentSystem.attr('title') != '') {
                    if (CurrentSystem.find('[name=isSystemNormal][type=hidden]').val() == "True") {
                        Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, true, false);
                    } else {
                        Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, false, false);
                    }
                }
                Clinical_ReviewofSystems.CacheCharacteristicDetailInfo(true);

                $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('');

                if ($('#' + Clinical_ReviewofSystems.params.PanelID + ' #dgvClinicalNotesTemplates tbody tr').length <= 0) {
                    Clinical_ReviewofSystems.loadTemplateSystems(null);
                }
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS').removeClass("active");
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS').addClass('hidden')
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiROSTemplates a').tab('show');
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiROSTemplates').addClass("active");
                $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #divReviewofSystemsSystemSection').html('');

            } else {
                if ($('#' + Clinical_ReviewofSystems.params.PanelID + ' #dgvClinicalNotesTemplates tbody tr').length <= 0) {
                    Clinical_ReviewofSystems.loadTemplateSystems(null);
                }
                if ($('#' + Clinical_ReviewofSystems.params.PanelID + ' #dgvClinicalNotesDataTemplates tbody tr').length <= 0) {
                    Clinical_ReviewofSystems.loadDataTemplateSystems(null);
                }
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS').removeClass("active");
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS').addClass('hidden')
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiROSTemplates a').tab('show');
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiROSTemplates').addClass("active");
                $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #divReviewofSystemsSystemSection').html('');
            }
            $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #actionButtons').hide();
        } else if (TabID == 'LiAddROS' && $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #addROSSystem').is(':visible') == false) {
            // $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS').trigger("click");
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS a').tab('show');
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS').removeClass("hidden");
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiROSTemplates').removeClass("active");
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS').addClass("active");
            Clinical_ReviewofSystems.toggleVerticalWidth();
            $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #divReviewofSystemsSystemSection').html('');
            $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #actionButtons').show();
        }
    },

    loadROSAgainstTemplate: function (ROSTemplateId) {
        var PrevROSTemplateId = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSTemplateId").val();
        var PrevROSDataTemplateId = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSDataTemplateId").val();
        var message = "Would you like to change & overwrite the existing ROS Template ?";
        if (PrevROSDataTemplateId > 0 || (PrevROSTemplateId > 0 && ROSTemplateId != PrevROSTemplateId)) {
            utility.myConfirm(message, function () {
                Clinical_ReviewofSystems.clearCachingROS();
                Clinical_ReviewofSystems.params.ROSDataTemplateId = -1;
                var objTabROSTemplates = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiROSTemplates');
                var objTabAddROS = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS');
                objTabROSTemplates.removeClass("active");
                objTabAddROS.removeClass("hidden");

                objTabAddROS.find("a").trigger("click");

                Clinical_ReviewofSystems.loadSystems(ROSTemplateId, '-1');

            }, "", "Confirm Changes");
        } else {
            Clinical_ReviewofSystems.clearCachingROS();
            Clinical_ReviewofSystems.params.ROSDataTemplateId = -1;
            var objTabROSTemplates = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiROSTemplates');
            var objTabAddROS = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS');
            objTabROSTemplates.removeClass("active");
            objTabAddROS.removeClass("hidden");

            objTabAddROS.find("a").trigger("click");

            Clinical_ReviewofSystems.loadSystems(ROSTemplateId, '-1');
        }
    },
    loadROSAgainstDataTemplate: function (ROSDataTemplateId) {
        var PrevROSTemplateId = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSTemplateId").val();
        var PrevROSDataTemplateId = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSDataTemplateId").val();
        var message = "Would you like to change & overwrite the existing ROS Template ?";
        if (PrevROSTemplateId > 0 && ROSDataTemplateId != PrevROSDataTemplateId) {
            utility.myConfirm(message, function () {
                Clinical_ReviewofSystems.clearCachingROS();
                Clinical_ReviewofSystems.params.ROSTemplateId = -1;
                var objTabROSTemplates = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiROSTemplates');
                var objTabAddROS = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS');
                objTabROSTemplates.removeClass("active");
                objTabAddROS.removeClass("hidden");


                Clinical_ReviewofSystems.params.ROSDataTemplateId = ROSDataTemplateId;
                Clinical_ReviewofSystems.params.isShowTemplate = false;
                Clinical_ReviewofSystems.loadROSSystemInfo();
                $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('true');

            }, "", "Confirm Changes");
        }
        else {
            Clinical_ReviewofSystems.clearCachingROS();
            Clinical_ReviewofSystems.params.ROSTemplateId = -1;
            var objTabROSTemplates = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiROSTemplates');
            var objTabAddROS = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #LiAddROS');
            objTabROSTemplates.removeClass("active");
            objTabAddROS.removeClass("hidden");


            Clinical_ReviewofSystems.params.ROSDataTemplateId = ROSDataTemplateId;
            Clinical_ReviewofSystems.params.isShowTemplate = false;
            Clinical_ReviewofSystems.loadROSSystemInfo();
            $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('true');
        }

    },
    /* This Function will load all the Systems and add it as two column sortable list
     Author: ZeeshanAK | Date: January 28, 2016 */
    //Start || 12 July, 2016 || ZeeshanAK || Fix for Data modified message popping up unnecessary.
    loadSystems: function (ROSTemplateId, ROSDataTemplateId, fromAddToNote) {
        $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSTemplateId").val(ROSTemplateId);
        if (ROSDataTemplateId == null || (!isNaN(ROSDataTemplateId) && Number(ROSDataTemplateId) < 1)) {
            ROSDataTemplateId = '-1';
        } else {
            if (!fromAddToNote && fromAddToNote != undefined) {
                $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('true');
            } else {
                setTimeout(function () {
                    //End   || 24 August, 2016 || ZeeshanAK || Fix for EMR - 871
                    if (Clinical_ReviewofSystems.params.ROSDataTemplateId == null || Clinical_ReviewofSystems.params.ROSDataTemplateId < 1) {
                        $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('');
                    }
                    //End   || 24 August, 2016 || ZeeshanAK || Fix for EMR - 871
                }, 150);
            }
        }
        //End   || 12 July, 2016 || ZeeshanAK || Fix for Data modified message popping up unnecessary.

        $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSDataTemplateId").val(ROSDataTemplateId);
        Clinical_ReviewofSystems.getROSSystems_DBCall(ROSTemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                var $ListVital = $(document.createElement('ul'));
                $ListVital.addClass('listPrimaryPad listTwoColumn');
                var SystemsJSON = JSON.parse(response.Systems_JSON);

                $.each(SystemsJSON, function (index, element) {
                    var ROSSystemPatientID = -1;
                    if (element.ROSSystemPatientID != null && element.ROSSystemPatientID != '') {
                        ROSSystemPatientID = element.ROSSystemPatientID;
                    }
                    if ((element.isCharacteristicsExists != null && element.isCharacteristicsExists != '') || (element.IsNormal != null && element.IsNormal != '' && element.IsNormal == "True")) {
                        $ListVital.append("<li  id='ROSSystem_" + element.Name + "_" + element.ROSSystemId + "' title='" + element.Name + "' class='green' onclick='Clinical_ReviewofSystems.GetCharacteristics(this," + element.ROSSystemId + ",\"" + element.Name + "\"," + ROSSystemPatientID + ");'><input type='hidden' id='isSystemNormal' name='isSystemNormal' value=\"" + element.IsNormal + "\" /><input type='hidden' id='systemNormalDescription' name='systemNormalDescription' value=\"" + element.SystemNormalDescription + "\" /><input type='hidden' id='systemPatientID' name='systemPatientID' value=\"" + ROSSystemPatientID + "\" /><input type='hidden' id='isCharacteristicsExists' name='isCharacteristicsExists' value=\"" + element.isCharacteristicsExists + "\" /><a  class='' ><span class='size85per ellipses pull-left btnEffect'>" + element.Name + "</span><span class='removeIconListHover' onclick='Clinical_ReviewofSystems.clearSectionInfo(event, this," + element.ROSSystemId + ",\"" + element.Name + "\");'><i class='fa fa-close'></i></span></a></li>");
                    } else {
                        $ListVital.append("<li  id='ROSSystem_" + element.Name + "_" + element.ROSSystemId + "' title='" + element.Name + "' class='' onclick='Clinical_ReviewofSystems.GetCharacteristics(this," + element.ROSSystemId + ",\"" + element.Name + "\"," + ROSSystemPatientID + ");'><input type='hidden' id='isSystemNormal' name='isSystemNormal' value=\"" + element.IsNormal + "\" /><input type='hidden' id='systemNormalDescription' name='systemNormalDescription' value=\"" + element.SystemNormalDescription + "\" /><input type='hidden' id='systemPatientID' name='systemPatientID' value=\"" + ROSSystemPatientID + "\" /><input type='hidden' id='isCharacteristicsExists' name='isCharacteristicsExists' value=\"" + element.isCharacteristicsExists + "\" /><a  class='' ><span class='size85per ellipses pull-left btnEffect'>" + element.Name + "</span><span class='removeIconListHover' onclick='Clinical_ReviewofSystems.clearSectionInfo(event, this," + element.ROSSystemId + ",\"" + element.Name + "\");'><i class='fa fa-close'></i></span></a></li>");
                    }

                    var isSystemNormal = element.IsNormal;
                    var systemNormalDescription = element.SystemNormalDescription;

                    var tempData = {
                    };
                    tempData['ReviewofSystemsSectionNormal' + "_" + element.ROSSystemId] = isSystemNormal;
                    tempData['SystemNormalDescription' + "_" + element.ROSSystemId] = systemNormalDescription;
                    Clinical_ReviewofSystems.CharcSystemInfo[element.Name] = JSON.stringify(tempData);
                });
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSTemplateId").val(ROSTemplateId);
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').html($ListVital[0].outerHTML);

                Clinical_ReviewofSystems.sortingInitializationSystem();
                var isSystemNormal = Clinical_ReviewofSystems.isAllNormalSystems();
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #chkReviewofSystemssNormal').prop('checked', isSystemNormal);

                if (isSystemNormal) {
                    $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divReviewofSystemsSystems ul li').find('span:first-child').addClass("green");
                    Clinical_ReviewofSystems.markAllSystemsAsNormalCache();
                }


            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    /* This Function will make the Divs sortable
    Author: ZeeshanAK | Date: January 28, 2016 */
    sortingInitializationSystem: function () {
        $('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul").sortable({
            connectWith: '#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul",
            //  revert: true,
            placeholder: "ui-state-highlight",
            //helper: 'clone'
            stop: function (event, ui) {
                setTimeout(function () {
                    var sortedIdsInOrder = $('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray");
                    $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('true');
                }
                , 5);
            }
        });
    },

    // Normalizes the cache for all of the systems
    markAllSystemsAsNormalCache: function () {
        var systemListCntrl = '#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul";
        $.map($(systemListCntrl).sortable("toArray"), function (n, i) { // just use arr
            var sName = n.split('_')[1];
            var sID = n.split('_')[2];

            Clinical_ReviewofSystems.markNormalSection(sName, sID, true);
        });
        $(systemListCntrl + " li [name=isSystemNormal][type=hidden]").val("True");
        Clinical_ReviewofSystems.normalizeAllSectionsArray();
    },

    // Denormalizes the cache for all of the systems
    unmarkAllSystemsAsNormalCache: function () {
        var systemListCntrl = '#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul";
        $.map($(systemListCntrl).sortable("toArray"), function (n, i) { // just use arr
            var sName = n.split('_')[1];
            var sID = n.split('_')[2];
            // var $currentSystemli = $($('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul li")[i]);
            // if ($currentSystemli.find('[name=isCharacteristicsExists][type=hidden]').val() == '') {
            Clinical_ReviewofSystems.unmarkNormalSection(sName, sID);
            // }
        });
        $(systemListCntrl + " li [name=isSystemNormal][type=hidden]").val("False");
        $(systemListCntrl + " li [name=systemNormalDescription][type=hidden]").val('');
        Clinical_ReviewofSystems.unNormalizeAllSectionsArray();
    },

    // Checks if all systems are normal or not
    isAllNormalSystems: function () {
        var systemListCntrl = '#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul";
        if ($(systemListCntrl + " li [name=isSystemNormal][type=hidden][value=False]").length > 0) {
            return false;
        } else {
            if ($(systemListCntrl + " li [name=isSystemNormal][type=hidden][value='']").length > 0) {
                return false;
            }
            return true;
        }
    },

    updatingCommentsOfNormal: function () {
        var systemNormal = '#frmClinicalReviewofSystems #divNormal';
        var ROSGlobalNormal = '#frmClinicalReviewofSystems #divNormalGlobal';
        var $globaltxtarea = $(ROSGlobalNormal).parent().find("textarea");
        var $sysnormaltxtarea = $(systemNormal).find("textarea");
        if ($(ROSGlobalNormal + ' input:checkbox').is(':checked') && $globaltxtarea.length > 0) {
            var Comments = $globaltxtarea.val().replace(/\n/g, '<br/>');
            $(ROSGlobalNormal).find("input:hidden").val(Comments);
            // $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li.active").find('[name=systemNormalDescription]').val(Comments);
            Clinical_ReviewofSystems.changeColorOfGlobalBookIcon();
            $globaltxtarea.closest("div.rightInnerAddon").remove();
        }

        if ($(systemNormal + ' [name=ReviewofSystemsSectionNormal').is(':checked') && $sysnormaltxtarea.length > 0) {

            var NormalComments = $sysnormaltxtarea.val().replace(/\n/g, '<br/>');
            $(systemNormal).find("input:hidden").val(NormalComments);
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #bookIconNormal").removeClass('disabled');
            Clinical_ReviewofSystems.changeColorOfNormalBookIcon();

            var systemname = $(systemNormal).attr('systemname');
            var $activeSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li[title='" + systemname + "']");
            $activeSystem = $activeSystem || $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #sectionReviewofSystems li.active");
            $activeSystem.find("#isSystemNormal").val('True');
            $activeSystem.find("#systemNormalDescription").val(NormalComments);
            $sysnormaltxtarea.closest("div.rightInnerAddon").remove();

        }
    },
    /* This Function will load all the Characteristics for a specific System
    Author: ZeeshanAK | Date: January 28, 2016 */
    GetCharacteristics: function (obj, SystemID, SystemName, ROSSystemPatientID) {

        Clinical_ReviewofSystems.updatingCommentsOfNormal();

        var charcSystemDiv = '#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystemSection';
        var SystemIsNormal = $(obj).find('[name=isSystemNormal][type=hidden]').val();
        var SystemNormalDescription = $(obj).find('[name=systemNormalDescription][type=hidden]').val();

        var $CharcDetailDiv = $("#ReviewofSystems").find('.toggle');
        if (!($CharcDetailDiv.prev().hasClass('hidden'))) {
            $CharcDetailDiv.prev().addClass('hidden');
            $CharcDetailDiv.removeClass('active');
        }
        if (!($CharcDetailDiv.hasClass('disableAll'))) {
            Clinical_ReviewofSystems.CacheCharacteristicDetailInfo(true);
        }
        var CurrentSystem = $($(obj).parent()).find("li.active");
        if (CurrentSystem.attr('title') != null && CurrentSystem.attr('title') != '') {
            if (CurrentSystem.find('[name=isSystemNormal][type=hidden]').val() == "True") {
                Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, true, false);
            } else {
                Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, false, false);
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

        Clinical_ReviewofSystems.getROSSystemsCharacteristics_DBCall(SystemID, ROSSystemPatientID).done(function (response) {

            CurrentSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #sectionReviewofSystems li.active");

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
                }
                if (GlobalChecked != null && GlobalChecked != "") {
                    isSectionNormalBookEnabled = "enabled blue";
                } else {
                    isSectionNormalBookEnabled = "disabled gray";
                }

                var divSearchBox = "<div class='col-md-3'><div class='input-group mb-default'><div class='input-group-addon'><i class='fa fa-search'></i></div><input type='text' id='txtSearchCharacteristics' onkeyup='Clinical_ReviewofSystems.getFilteredCharacteristics(this);' class='form-control' placeholder='Search Characteristics...' /><div class='input-group-btn'><a onClick='Clinical_ReviewofSystems.clearSearchBox(this);'class='btn btn-primary btn-xs pl-xs pr-xs'><i class='fa fa-times-circle-o'></i></a></div></div></div>";
                var divNormalCheckBox = "<div class='col-md-9' id='divNormal' systemname='" + CurrentSystem + "'><a class='btn btn-xs pull-left " + isSectionNormalBookEnabled + "' id='bookIconNormal' href='javascript:void(0);'  onclick='Clinical_ReviewofSystems.showTextArea(this);'> <i class='fa fa-book'></i></a><input type='checkbox'class='mr-xs ml-xs pull-left' id='ReviewofSystemsSectionNormal_" + SystemID + "' onclick='Clinical_ReviewofSystems.markCurrentSectionAsNormal(this, \"" + SystemName + "\", \"" + SystemID + "\");' class='checkbox-custom' name='ReviewofSystemsSectionNormal' " + GlobalChecked + "/><label class='control-label'>Normal</label><input type='hidden' id='hdnDescriptionNormal' name='hdnDescriptionNormal' /></div>";
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
                        BookClass = '';
                    }
                    else if (element.IsPositive == "False") {
                        IsPositive = "";
                        isSelectAllPositiveChecked = "";
                    } else {
                        IsNegative = "";
                        isSelectAllNegativeChecked = "";
                    }
                    var hdnROSSystemPatientCharacteristicsID = "<input type='hidden' id=" + "SystemPatientCharacteristicsID_" + element.ROSSystemPatientCharacteristicsID + " name= 'SystemPatientCharacteristicsID' value='" + element.ROSSystemPatientCharacteristicsID + "'><input type='hidden' name='isCharcDetailExists' id='isCharcDetailExists' value='" + element.isCharcDetailExists + "'>";
                    HTMLSystemCharacteristics += "<li id='" + SystemName + "_" + SystemID + "_" + element.ROSSystemCharacteristicsId + "' name='" + SystemName + "_" + SystemID + "' systemname='" + SystemName + "' SystemID='" + SystemID + "' class='col-xs-4'  onClick='Clinical_ReviewofSystems.loadSysPatCharcDetail(this" + ((element.isCharcDetailExists != null && element.isCharcDetailExists != '') ? "," + element.isCharcDetailExists : "") + ");'>" +
                        "<div><a class='btn  mt-xxs btn-xs pull-left disabled gray'  id='bookIcon' href='#' onclick='Clinical_ReviewofSystems.showTextArea(this);'> <i class='fa fa-book'></i></a>" +
                       "<div class='mt-xxs pull-left'> " +
                       "<div class='checkbox-icon checkbox-check mr-xs pull-left'>" +
                       "<input class='' type='checkbox' " + IsPositive + " onchange='Clinical_ReviewofSystems.checkUncheckPositiveNegative(this, \"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='Charc_Pos_" + SystemID + "_" + element.ROSSystemCharacteristicsId + "'  name='chkPositive'>" +
                       "</div><div class='checkbox-icon checkbox-cross pull-left'>" +
                       "<input type='checkbox'  class='' " + IsNegative + " onchange='Clinical_ReviewofSystems.checkUncheckPositiveNegative(this,\"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='Charc_Neg_" + SystemID + "_" + element.ROSSystemCharacteristicsId + "'  name='chkNegative'>" +
                       "</div></div><label class='pull-left mt-xxs pl-xxs control-label'>" + element.CharacteristicsName + hdnDescription + hdnROSSystemPatientCharacteristicsID + "</label><div class='clearfix'></div></div></li>";

                });

                if (GlobalChecked != "") {
                    isSelectAllPositiveChecked = '';
                    isSelectAllNegativeChecked = '';
                }

                HTMLSelectAllCheckBox += "<li id='selectAllBookMark' class='col-xs-4'><div>" +
                  "<div class='mt-xxs ml-lg pull-left'>" +
                  "<div class='checkbox-icon checkbox-check mr-xs pull-left'>" +
                  "<input class='pull-left' type='checkbox' " + isSelectAllPositiveChecked + " onchange='Clinical_ReviewofSystems.selectAllCheckBoxes(this, \"Positive" + "\", \"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='selectAllPositive_" + SystemID + "'  name='selectAllPositive'>" +
                  "</div><div class='checkbox-icon checkbox-cross pull-left'>" +
                  "<input type='checkbox' " + isSelectAllNegativeChecked + " class='pull-left' onchange='Clinical_ReviewofSystems.selectAllCheckBoxes(this, \"Negative" + "\", \"ROSSystem_" + SystemName + "_" + SystemID + "\");' id='selectAllNegative_" + SystemID + "' name='selectAllNegative'>" +
                  "</div>" +
                  "</div><label class='pull-left mt-xxs pl-xxs control-label bold'>Select All</label><div class='clearfix'></div></div></div></li>";

                $ListVital.append(HTMLSelectAllCheckBox);
                $ListVital.append(HTMLSystemCharacteristics);

                $(charcSystemDiv).html($ListVital[0].outerHTML).prepend(DivRow[0].outerHTML);

                /* this check will check the caching state of system characteristics, if user has made any changes in characteristics, it will bind the user state
                   Change Implemented By: Azhar Shahzad
                   Date: januray 02, 2016 02:56pm */
                if (Clinical_ReviewofSystems.CharcSystemInfo != null && Clinical_ReviewofSystems.CharcSystemInfo[SystemName] != null) {
                    utility.bindMyJSON(true, JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[SystemName]), false, $(charcSystemDiv)).done(function () {
                        if ($('[id*=ReviewofSystemsSectionNormal]').is(':checked')) {
                            $('[id*=Charc_Pos_],[id*=Charc_Neg_]').prop('checked', false);

                        }
                    });

                    if (SystemNormalDescription != '' || (JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[SystemName]).hdnDescriptionNormal != null && JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[SystemName]).hdnDescriptionNormal != '')) {
                        if (SystemNormalDescription != '') {
                            $('#hdnDescriptionNormal').val(SystemNormalDescription);
                            Clinical_ReviewofSystems.changeColorOfNormalBookIcon();
                            $('#frmClinicalReviewofSystems #divNormal #bookIconNormal').removeClass('disabled');
                            Clinical_ReviewofSystems.markNormalSection(SystemName, SystemID);
                        } else if (JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[SystemName]).hdnDescriptionNormal != null) {
                            $('#hdnDescriptionNormal').val(JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[SystemName]).hdnDescriptionNormal);
                            Clinical_ReviewofSystems.changeColorOfNormalBookIcon();
                            $('#frmClinicalReviewofSystems #divNormal #bookIconNormal').removeClass('disabled');
                            Clinical_ReviewofSystems.markNormalSection(SystemName, SystemID);
                        }

                    } else {
                        $('[id*=Charc_Pos_],[id*=Charc_Neg_]').each(function (index, item) {
                            if ($(item).is(':checked')) {
                                $(item).prop('checked', true);
                                Clinical_ReviewofSystems.bookIconClassToggle($(item).parent().parent().siblings().siblings('[id=bookIcon]'), false);

                                Clinical_ReviewofSystems.changeColorForliCharacteristics(item);
                            } else {
                                if ($(item).parent().parent().find("[type=checkbox]:checked").length > 0) {
                                    Clinical_ReviewofSystems.bookIconClassToggle($(item).parent().parent().siblings().siblings('[id=bookIcon]'), false);
                                    Clinical_ReviewofSystems.changeColorForliCharacteristics(item);
                                } else {
                                    Clinical_ReviewofSystems.bookIconClassToggle($(item).parent().parent().siblings().siblings('[id=bookIcon]'), true);
                                    Clinical_ReviewofSystems.changeColorForliCharacteristics(item);
                                }
                            }
                        });
                    }

                    Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, false, false);
                    $(charcSystemDiv).data(SystemName, Clinical_ReviewofSystems.CharcSystemInfo[SystemName]);

                } else {

                    $(charcSystemDiv).data(SystemName, $(charcSystemDiv).getMyJSON());

                    //If Normal (Global) is checked, make the selected Section Normal too
                    if ($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #chkReviewofSystemssNormal').is(':checked') || (SystemNormalDescription != '')) {

                        $('#hdnDescriptionNormal').val(SystemNormalDescription);
                        Clinical_ReviewofSystems.changeColorOfNormalBookIcon();
                        $('#frmClinicalReviewofSystems #divNormal #bookIconNormal').removeClass('disabled');
                        Clinical_ReviewofSystems.markNormalSection(SystemName, SystemID);
                    }
                        // else bind data to characteristics
                    else {

                        $.each(SystemCharacteristicsJSON, function (index, item) {
                            if (item.IsPositive == 'True') {
                                $('#Charc_Pos_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').prop('checked', true);
                                Clinical_ReviewofSystems.bookIconClassToggle($('#Charc_Pos_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').parent().siblings().siblings('[id=bookIcon]'), false);
                                $('#Charc_Desc_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').val(item.Description);
                            } else if (item.IsPositive == 'False') {
                                $('#Charc_Neg_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').prop('checked', true)
                                Clinical_ReviewofSystems.bookIconClassToggle($('#Charc_Pos_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').parent().siblings().siblings('[id=bookIcon]'), false);
                                $('#Charc_Desc_' + item.ROSSystemId + "_" + item.ROSSystemCharacteristicsId + '').val(item.Description);
                            }
                        });

                    }

                    Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, false, false);
                    $(charcSystemDiv).data(SystemName, Clinical_ReviewofSystems.CharcSystemInfo[SystemName]);
                }
                //end change januray 02, 2016 02:56pm

                if ($(charcSystemDiv).find('input:checkbox[id*=ReviewofSystemsSectionNormal]').is(':checked')) {

                    $("#ReviewofSystems").find('.toggle').addClass('disableAll');
                    $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL").addClass('disableAll');
                    if (Clinical_ReviewofSystems.isAllNormalSystems()) {
                        Clinical_ReviewofSystems.markAllSystemsAsNormalCache();
                    }

                } else if ($(charcSystemDiv).find('input:checkbox:not([id*=ReviewofSystemsSectionNormal])').is(':checked') == true) {

                    $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL").removeClass('disableAll');

                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            Clinical_ReviewofSystems.handleColorForSelectedSystem(obj);

        });

        Clinical_ReviewofSystems.handleDetailToggleDiv();
    },

    // Author : Khaleel Ur Rehman.
    // Purpose : Handle color for selected system.
    // Date : 26-Feb-2016
    handleColorForSelectedSystem: function (obj) {
        if (($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #ActiveSystemUL li').find('input[type=checkbox]:checked').length > 0) || ($(obj).closest("li").find("#isCharacteristicsExists").val()) || $(obj).closest("li").find("#isSystemNormal").val() == "True") {
            $(obj).closest("li").find('span:first-child').addClass("green");
        } else {
            //Start || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
            $(obj).closest("li").removeClass("green").find('span:first-child').removeClass("green");
            //End   || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-471
        }
    },
    // Author : Khaleel Ur Rehman.
    // Purpose : handle Detail Toggle Div.
    // Date : 18-Feb-2016
    handleDetailToggleDiv: function () {
        $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').prev().addClass('hidden');
        countWidth();
        $("#ReviewofSystems").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
        $("#ReviewofSystems").find('.toggle').removeClass('active');
        $("#ReviewofSystems").find('.toggle').addClass('disableAll');
    },


    //Start By Khaleel Ur Rehman To Filter Characteristics on 28 January 2016.
    getFilteredCharacteristics: function (cntrl) {
        var rex = new RegExp($(cntrl).val(), 'i');
        $('.searchable li').hide();
        $('.searchable li').filter(function () {
            return rex.test($(this).text());
        }).show();
    },

    //By Khaleel Ur Rehman to Show confirmation box to save data if there are any comments in the text area on focus out
    textAreaFocusOut: function (e, cntl) {
        if ($(cntl).attr('flag') != '1') {
            //Start || 25 April, 2016 || ZeeshanAK || Change made for fixing EMR-633
            if ($(cntl).length > 0) {
                //End   || 25 April, 2016 || ZeeshanAK || Change made for fixing EMR-633
                var control = $(cntl);
                Clinical_ReviewofSystems.setValueInHiddenDescription(e, control, 1);
            }

        } else {
            $(cntl).attr('flag', '0');
        }
    },

    //By Khaleel Ur Rehman To Show text area when Book Mark Icon is clicked... Date: 28 January 2016.
    showTextArea: function (cntrl) {

        var txtArea = "";
        txtArea = '<div class="rightInnerAddon">' +
            '<textarea id="" maxlength="5000" spellcheck="true" class="form-control pr-xlg height-max105 size100per textAreaScroll"   onkeydown=\"Clinical_ReviewofSystems.setValueInHiddenDescription(event,this,0);\" onblur=\"Clinical_ReviewofSystems.textAreaFocusOut(event,this);\"></textarea><div class="clearfix"></div>'
            + '</div>';
        if (cntrl.id == 'bookIconNormal') {
            $(cntrl).closest('div').find('textarea').remove();
            $(cntrl).closest('div').append(txtArea);
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #divNormal").find("textarea").html($('#divNormal').find("input:hidden").val().replace(/<br\s*[\/]?>/gi, "\n"));
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #bookIconNormal").addClass('disabled');//kr
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #divNormal").find("textarea").focus();
        } else if (cntrl.id == 'bookIconNormalGlobal') {
            $(cntrl).closest('div:not(.checkbox-custom)').find('textarea').remove();
            $(cntrl).closest('div:not(.checkbox-custom)').append(txtArea);
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #divNormalGlobal").parent().find("textarea").html($('#divNormalGlobal').find("input:hidden").val().replace(/<br\s*[\/]?>/gi, "\n"));
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #divNormalGlobal").parent().find("textarea").focus();
        } else {
            if ($(cntrl).closest('ul').find('.rightInnerAddon').length == 0) {
                $(cntrl).closest('ul').find('textarea').remove();
                $(cntrl).closest('li').append(txtArea);
                $(cntrl).closest('li').find("textarea").html($(cntrl).closest('li').find("[type=hidden][id^=Charc_Desc_]").val().replace(/<br\s*[\/]?>/gi, "\n"));
                //$(cntrl).closest('li').find("textarea").focus();
            }
            $('.textAreaScroll').slimScroll({
                position: 'right',
                height: '100%',
            }).on('input', function () {
                Clinical_ReviewofSystems.increaseRowsTextarea(this);
            }).on('focus', function () {
                Clinical_ReviewofSystems.increaseRowsTextarea(this);
            });

            $(cntrl).closest('li').find("textarea").focus();
        }
    },

    // Increases the row of text area if user types more.
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

    //Change color of Global book icon with normal checkbox.
    changeColorOfGlobalBookIcon: function () {
        var $BookIConControl = $('#frmClinicalReviewofSystems #divNormalGlobal #bookIconNormalGlobal');
        if ($('#frmClinicalReviewofSystems #divNormalGlobal #hdnDescriptionNormalGlobal').val()) {
            if ($BookIConControl.hasClass("gray")) {
                $BookIConControl.removeClass("gray");
            }
            if ($BookIConControl.hasClass("gray")) {
                $BookIConControl.removeClass("gray");
            }
            $BookIConControl.removeClass("blue");
            $BookIConControl.addClass("green");
            $BookIConControl.find('i').removeClass("blue");
            $BookIConControl.find('i').addClass("green");
            $BookIConControl.tooltip({
                placement: 'right',
                container: 'body',
                title: function () {
                    return $(this).parent().find('input:hidden').val().replace(/<br\s*[\/]?>/gi, "\n");
                }
            });
        }
        else {
            $BookIConControl.removeClass("green");
            $BookIConControl.find('i').removeClass("green");
            if ($('#frmClinicalReviewofSystems #divNormalGlobal input:checkbox').is(':checked')) {
                $BookIConControl.removeClass("gray");
                $BookIConControl.find('i').removeClass("gray");
                $BookIConControl.addClass("blue");
            }
        }
    },

    //Change color of section book icon with normal checkbox.
    changeColorOfNormalBookIcon: function () {
        var $BookIconControl = $('#frmClinicalReviewofSystems #divNormal #bookIconNormal');
        if ($('#frmClinicalReviewofSystems #divNormal').find("input:hidden").val()) {
            $BookIconControl.find('i').removeClass("blue");
            $BookIconControl.find('i').addClass("green");
            $BookIconControl.removeClass("blue");
            $BookIconControl.addClass("green");
            $BookIconControl.removeClass('disabled');
            $BookIconControl.tooltip({
                placement: 'left',
                container: 'body',
                title: function () {
                    return $(this).parent().find('input:hidden').val().replace(/<br\s*[\/]?>/gi, "\n");
                }
            });
        }
        else {
            $BookIconControl.removeClass("green");
            $BookIconControl.find('i').removeClass("green")
        }
    },

    //Change All color of Characteristics
    changeAllColorForliCharacteristics: function (cntrl) {

        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystemSection li').each(function (index, element) {
            if (typeof $(element).find("input:hidden").val() != 'undefined' && $(element).find("input:hidden").val() != '') {
                $(element).find("a").find("i").removeClass("blue");
                $(element).find("a").find("i").addClass("green");
                $(element).find("a").find("i").tooltip({
                    placement: 'left',
                    container: 'body',
                    title: function () {
                        return $(this).closest('li').find("[type=hidden][name=isCharcDetailExists]").val().replace(/<br\s*[\/]?>/gi, "\n");
                    }
                });
            }
            else {
                $(element).find("a").find("i").removeClass("green");
            }
        });

    },

    //Change color of Characteristics
    changeColorForliCharacteristics: function (cntrl) {
        var $controlNearLi = $(cntrl).closest('li');
        if ($controlNearLi.find("[type=hidden][id^=Charc_Desc_]").val()) {
            $controlNearLi.find("a").find("i").removeClass("blue");
            $controlNearLi.find("a").removeClass("disabled");
            $controlNearLi.find("a").find("i").addClass("green");
            $controlNearLi.find("a").find("i").tooltip({
                placement: 'left',
                container: 'body',
                title: function () {
                    return $(this).closest('li').find("[type=hidden][id^=Charc_Desc_]").val().replace(/<br\s*[\/]?>/gi, "\n");
                }
            });
        }
        else {
            $controlNearLi.find("a").find("i").removeClass("green");
        }
    },

    //By Khaleel Ur Rehman to set val in hidden field from text area when Press Enter in Text Area.
    setValueInHiddenDescription: function (e, cntrl, btnClicked) {

        var keyCode = e.keyCode ? e.keyCode : e.which ? e.which : e.charCode;
        if (btnClicked == 1) {

            $(cntrl).parent().parent().find("textarea").attr('flag', '1')

            if ($(cntrl).closest('#divNormal').length > 0) {

                var NormalComments = $('#frmClinicalReviewofSystems #divNormal').find("textarea").val().replace(/\n/g, '<br/>');
                $('#frmClinicalReviewofSystems #divNormal').find("input:hidden").val(NormalComments);
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #bookIconNormal").removeClass('disabled');//kr
                Clinical_ReviewofSystems.changeColorOfNormalBookIcon();
                var systemname = $('#frmClinicalReviewofSystems #divNormal').attr('systemname');
                var activeSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li[title='" + systemname + "']");
                activeSystem = activeSystem || $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #sectionReviewofSystems li.active");
                activeSystem.find("#isSystemNormal").val('True');
                activeSystem.find("#systemNormalDescription").val(NormalComments);

            }
            else if ($(cntrl).parent().parent().parent().find('#divNormalGlobal').length > 0) {

                var Comments = $('#frmClinicalReviewofSystems #divNormalGlobal').parent().find("textarea").val().replace(/\n/g, '<br/>');
                $('#frmClinicalReviewofSystems #divNormalGlobal').find("input:hidden").val(Comments);
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li.active").find('[name=systemNormalDescription]').val(Comments);
                Clinical_ReviewofSystems.changeColorOfGlobalBookIcon();
                var CurrentSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li.active");
                Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, true, false);

            }
            else {

                var $CharcLi = $(cntrl).closest('li');
                var comments = $CharcLi.find("textarea").val().replace(/\n/g, '<br/>');
                $CharcLi.find("[type=hidden][id^=Charc_Desc_]").val(comments);
                Clinical_ReviewofSystems.changeColorForliCharacteristics(cntrl);
                var systemname = $CharcLi.attr('systemname');
                var activeSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li[title='" + systemname + "']");
                var CurrentSystem = activeSystem || $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li.active");
                Clinical_ReviewofSystems.CacheCharacteristicInfo(CurrentSystem, false, true, false);

            }

            $(cntrl).closest("div.rightInnerAddon").remove();
        }
    },

    //End by Khaleel Ur Rehman.

    /* Implementing checkboxes functionality as radio boxes i.e. only one can be selected at a time.
        And enable the book icon only if one of the checkbox is checked.
        Also, uncheck the Normal checkbox for this characteristics
       Author: ZeeshanAK | Date: January 28, 2016 */
    checkUncheckPositiveNegative: function (checkedBox, systemNameWithID) {

        if (!$("#ReviewofSystems").find('.toggle').prev().hasClass('hidden')) {
            $("#ReviewofSystems").find('.toggle').prev().addClass('hidden');
            countWidth();
            $("#ReviewofSystems").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
            $("#ReviewofSystems").find('.toggle').removeClass('active');
        }

        if (!$("#ReviewofSystems").find('.toggle').hasClass('disableAll')) {
            Clinical_ReviewofSystems.CacheCharacteristicDetailInfo(false);
        }

        if ($("#ActiveSystemUL > li").hasClass('active'))
            $("#ActiveSystemUL > li").removeClass('active');

        Clinical_ReviewofSystems.validateDurationDetails();

        $(checkedBox).parents('li').addClass('active');

        if ($(checkedBox).is(':checked')) {

            Clinical_ReviewofSystems.bookIconClassToggle($(checkedBox).closest('li').find('[id=bookIcon]'), false);
            if ($(checkedBox).parent().siblings('.checkbox-icon').find('[type=checkbox]').is(':checked')) {
                $(checkedBox).parent().siblings('.checkbox-icon').find('[type=checkbox]').prop('checked', false);
            }
            $('[id^=selectAllNeg] ,[id^=selectAllPos').prop('checked', false);

            if (!$("#ReviewofSystems").find('.toggle').hasClass('disableAll')) {
                Clinical_ReviewofSystems.LoadCharacteristicDetailInfo(checkedBox);
            }

        } else {
            Clinical_ReviewofSystems.bookIconClassToggle($(checkedBox).parent().parent().siblings().siblings('[id=bookIcon]'), true);
            $('[id^=selectAllNeg] ,[id^=selectAllPos').prop('checked', false);
            var charcId = $(checkedBox).parents('li').attr('id');

            var message = "Unselecting a characteristic will remove its detail. Are you sure you want to unselect?";
            var isCharcDetailExists = $(checkedBox).closest('li').find("[type=hidden][name=isCharcDetailExists]").val();

            var systemPatientCharacteristicsID = $(checkedBox).closest('li').find("[type=hidden][name=SystemPatientCharacteristicsID]").attr('id');
            systemPatientCharacteristicsID = systemPatientCharacteristicsID != null ? systemPatientCharacteristicsID.split('_')[1] : systemPatientCharacteristicsID;

            if (systemPatientCharacteristicsID != -1 && systemPatientCharacteristicsID != '' && systemPatientCharacteristicsID != null) {

                utility.myConfirm(message, function () {
                    Clinical_ReviewofSystems.deleteSystemPatientCharcDetail_DBCall(systemPatientCharacteristicsID).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_ReviewofSystems.uncheckCharacteristic($(checkedBox));
                        }
                    });

                }, function () {
                    $(checkedBox).prop('checked', true);
                    Clinical_ReviewofSystems.enableDisableCharcDetails();
                });

            } else {

                Clinical_ReviewofSystems.uncheckCharacteristic($(checkedBox));

            }

            Clinical_ReviewofSystems.hideTextAreaWhenBothChkBoxesUnchecked(checkedBox);

        }

        $('#' + Clinical_ReviewofSystems.params.PanelID + ' [name=ReviewofSystemsSectionNormal]').prop('checked', false);

        Clinical_ReviewofSystems.enableDisableCharcDetails();

        var charcSystemDiv = '#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystemSection';
        var CurrentSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #sectionReviewofSystems li.active");

        if ($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #ActiveSystemUL li').find('input[type=checkbox]:checked').length > 0) {
            Clinical_ReviewofSystems.addgreenClassToSystem(charcSystemDiv, CurrentSystem);
        } else {
            SystemNameData = unescape($(charcSystemDiv).getMyJSON());
            Clinical_ReviewofSystems.CharcSystemInfo[CurrentSystem.attr('title')] = SystemNameData;
            $(CurrentSystem).find('span:first-child').removeClass('green');
            if ($(CurrentSystem).hasClass('green')) {
                $(CurrentSystem).removeClass('green');
            }
            $(charcSystemDiv).data(CurrentSystem.attr('title'), []);
        }

    },

    // By Khaleel Ur Rehman
    // Purpose: To Restrict user to enter description for a characteristic when both positive and negative checkbxes are unchecked.
    // Date: 26 Feb 2016
    hideTextAreaWhenBothChkBoxesUnchecked: function (chkBox) {
        if ($(chkBox).closest("li").find('input[type=checkbox]:checked').length == 0) {
            $(chkBox).closest("li").find("div.rightInnerAddon").remove();
        }
    },

    // When a Characteristics is clicked its detail info is loaded from cache or sends DB call
    LoadCharacteristicDetailInfo: function (checkedBox) {

        var charcId = $(checkedBox).closest('li').attr('id');
        var charcDetailDiv = $('#divExamCharacteristics');

        if (Clinical_ReviewofSystems.CharacteristicsDetailsInfo != null && Clinical_ReviewofSystems.CharacteristicsDetailsInfo.length > 0) {

            var detailObj = $.grep(Clinical_ReviewofSystems.CharacteristicsDetailsInfo, function (item, index) {
                return (item.Id == charcId);
            });

            if (detailObj.length > 0) {
                utility.bindMyJSON(true, JSON.parse(detailObj[0].DetailInfo), false, $(charcDetailDiv));
                $(charcDetailDiv).data($(checkedBox).closest('li'), detailObj[0].DetailInfo);
                Clinical_ReviewofSystems.validateDurationDetails();
            }
            else {
                $(charcDetailDiv).resetAllControls();
            }

        } else {
            $(charcDetailDiv).resetAllControls();

        }
    },

    // Marks section as normal
    markNormalSection: function (systemName, SystemID, fromMarkAll) {
        if (Clinical_ReviewofSystems.CharcSystemInfo[systemName] != null && Clinical_ReviewofSystems.CharcSystemInfo[systemName] != undefined) {
            var objData = JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[systemName]);
            Object.keys(objData).forEach(function (key, index) {
                // key: the name of the object key
                // index: the ordinal position of the key within the object
                if (key.indexOf("Charc_Neg_") > -1 || key.indexOf('Charc_Pos_') > -1 || key.indexOf('Charc_Desc') > -1) {
                    objData[key] = '';
                }
            });
            Clinical_ReviewofSystems.CharcSystemInfo[systemName] = JSON.stringify(objData);
        }
        if (Clinical_ReviewofSystems.CharacteristicsDetailsInfo != null) {
            $.each(Clinical_ReviewofSystems.CharacteristicsDetailsInfo, function (index, element) {

                if (element) {
                    if (element.SystemId == SystemID) {
                        //Clinical_ReviewofSystems.CharacteristicsDetailsInfo[index] = null;
                        //Start || 14 March, 2016 || ZeeshanAK || Changes made for fixing EMR-440
                        var detailObj = $.grep(Clinical_ReviewofSystems.CharacteristicsDetailsInfo, function (item, index) {
                            return (item.SystemId == SystemID);
                        });
                        Clinical_ReviewofSystems.CharacteristicsDetailsInfo.splice(detailObj, detailObj.length);

                    }
                } else {
                    return false;
                }
            });

        }


        var $CurrentSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li.active");
        $CurrentSystem.find("[name=isSystemNormal][type=hidden]").val("True");
        Clinical_ReviewofSystems.CacheCharacteristicInfo($CurrentSystem, false, true, false);


        $("#ReviewofSystems").find('.toggle').addClass('disableAll');
        $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL > li").removeClass('active');
        $('#txtSearchCharacteristics').parent().addClass('disableAll');

        if (fromMarkAll == null) {
            var isSystemNormal = Clinical_ReviewofSystems.isAllNormalSystems();

            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #chkReviewofSystemssNormal').prop('checked', isSystemNormal);
            Clinical_ReviewofSystems.bookIconClassToggle($('#' + Clinical_ReviewofSystems.params.PanelID + " #bookIconNormalGlobal"), !isSystemNormal);
            Clinical_ReviewofSystems.markCurrentActiveSectionAsNormal(isSystemNormal, null, systemName);

            Clinical_ReviewofSystems.changeColorOfNormalBookIcon();
        }
    },

    // unMarks section as normal
    unmarkNormalSection: function (systemName, SystemID) {
        var SystemCharc = '#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems";

        $(SystemCharc + " #ActiveSystemUL").removeClass('disableAll');
        $(SystemCharc + " #ActiveSystemUL li").find('input[type=checkbox]').prop("disabled", false);

        Clinical_ReviewofSystems.bookIconClassToggle($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').find('[id=bookIconNormal],[id =bookIconNormalGlobal]'), true);

        $(SystemCharc + ' #chkReviewofSystemssNormal').prop('checked', false);
        $(SystemCharc + ' #txtSearchCharacteristics').parent().removeClass('disableAll');
        $(SystemCharc).find('[id=divNormal],[id =bookIconNormalGlobal]').val('');
        $(SystemCharc + ' #divNormal').find("div.rightInnerAddon").remove();
        $(SystemCharc + ' #divNormalGlobal').find("input:hidden").val('');
        $(SystemCharc).find("li.active").find("[name=isSystemNormal][type=hidden]").val("False");
        $(SystemCharc).find("li.active").find("[name=systemNormalDescription][type=hidden]").val('');
        $(SystemCharc + ' #divNormal').find("[name=hdnDescriptionNormal][type=hidden]").val('');
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystems').find("li.active").removeClass('green').find('span:first-child').removeClass("green");

        Clinical_ReviewofSystems.changeColorOfGlobalBookIcon();
        Clinical_ReviewofSystems.changeColorOfNormalBookIcon();
        Clinical_ReviewofSystems.CacheCharacteristicInfo(null, false, false, true, systemName, SystemID);
    },

    /* Mark current section as normal i.e. uncheck all checkboxes and disable them
      Author: ZeeshanAK | Date: January 29, 2016 */
    markCurrentSectionAsNormal: function (obj, systemName, SystemID) {
        var message = 'This will mark the entire ' + systemName + ' as Normal and reset all values in all sections of ' + systemName + '. Would you like to proceed?';
        var isToMarkNormal = $(obj).prop("checked");
        if (isToMarkNormal == true) {
            utility.myConfirm(message, function () {
                countWidth();
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').removeClass('active');
                Clinical_ReviewofSystems.markNormalSection(systemName, SystemID, null);
                Clinical_ReviewofSystems.bookIconClassToggle($('#' + Clinical_ReviewofSystems.params.PanelID + " #bookIconNormal"), false);
            }, function () {
                $(obj).prop("checked", false);
                Clinical_ReviewofSystems.bookIconClassToggle($('#' + Clinical_ReviewofSystems.params.PanelID + " #bookIconNormal"), true);
            }, "Confirm Mark Normal");
        }
        else {
            Clinical_ReviewofSystems.unmarkNormalSection(systemName, SystemID);
        }
    },

    bookIconClassToggle: function (Control, IsDisabled) {
        var isCharC = $(Control).closest('li') || $(Control).closest('#divNormalGlobal');
        var charcComments = isCharC.find('[id^="Charc_Desc"]').val();
        if (charcComments == null || charcComments == '') {
            if (!IsDisabled) {
                $(Control).removeClass('disabled');
                $(Control).removeClass('gray');
                $(Control).addClass('blue');
                if ($(Control).find('i') != null && $(Control).find('i').length > 0) {
                    $(Control).find('i').removeClass('disabled');
                    $(Control).find('i').removeClass('gray');
                    $(Control).find('i').addClass('blue');
                }
            } else {
                Clinical_ReviewofSystems.disableBookIcon(Control);
            }
        } else if (IsDisabled) {
            Clinical_ReviewofSystems.disableBookIcon(Control);
        }
    },
    disableBookIcon: function (Control) {
        $(Control).removeClass('green');
        $(Control).removeClass('blue');
        $(Control).addClass('gray');
        $(Control).addClass('disabled');
        if ($(Control).find('i') != null && $(Control).find('i').length > 0) {
            $(Control).find('i').removeClass('green');
            $(Control).find('i').removeClass('blue');
            $(Control).find('i').addClass('gray');
            $(Control).find('i').addClass('disabled');
        }
    },
    /* Normalize currently active section
      Author: ZeeshanAK | Date: February 02, 2016 */
    markCurrentActiveSectionAsNormal: function (sectionNormal, globalNormal, systemName) {
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #SystemSections').find('input[type=checkbox]').closest('li').find("div.rightInnerAddon").remove();
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #ActiveSystemUL li').find('input[type=checkbox]').prop({
            'checked': false, 'disabled': true
        });
        $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL").addClass('disableAll')
        Clinical_ReviewofSystems.bookIconClassToggle($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #ActiveSystemUL li').find('[id=bookIcon]'), true);
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').find('[id=bookIconNormal]').removeClass('disabled');

        if ((globalNormal != null && globalNormal == true) || Clinical_ReviewofSystems.isAllNormalSystems()) {
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' [name^=ReviewofSystemsSectionNormal]').prop('checked', true);
            Clinical_ReviewofSystems.markAllSystemsAsNormalCache();
        } else {
            Clinical_ReviewofSystems.CacheCharacteristicInfo(null, false);
        }

    },

    /* Select all Positive or Negative checkboxes if it's selected from Select All
      Author: ZeeshanAK | Date: February 01, 2016 */
    selectAllCheckBoxes: function (obj, isPositive, systemNameWithID) {
        var $CharacteristicsList = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL li");
        if ($('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL > li").hasClass('active'))
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL > li").removeClass('active');

        $(obj).parents('li').addClass('active');

        if ($(obj).prop("checked")) {
            if (isPositive == 'Positive') {
                $CharacteristicsList.find('input[id*=Pos]').prop('checked', true);
                $CharacteristicsList.find('input[id*=Neg]').prop('checked', false);
                Clinical_ReviewofSystems.bookIconClassToggle($CharacteristicsList.find('[id=bookIcon]'), false);

            } else {
                $CharacteristicsList.find('input[id*=Pos]').prop('checked', false);
                $CharacteristicsList.find('input[id*=Neg]').prop('checked', true);
                Clinical_ReviewofSystems.bookIconClassToggle($CharacteristicsList.find('[id=bookIcon]'), false);
            }
        } else {
            $CharacteristicsList.find('input[id*=Pos]').prop('checked', false);
            $CharacteristicsList.find('input[id*=Neg]').prop('checked', false);
            Clinical_ReviewofSystems.bookIconClassToggle($CharacteristicsList.find('[id=bookIcon]'), true);
        }

        var charcSystemDiv = '#' + Clinical_ReviewofSystems.params.PanelID + ' #divReviewofSystemsSystemSection';
        var $CurrentSystem = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #sectionReviewofSystems li.active");
        if ($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #ActiveSystemUL li').find('input[type=checkbox]:checked').length > 0) {

            Clinical_ReviewofSystems.addgreenClassToSystem(charcSystemDiv, $CurrentSystem);
        } else {
            SystemNameData = unescape($(charcSystemDiv).getMyJSON());
            Clinical_ReviewofSystems.CharcSystemInfo[$CurrentSystem.attr('title')] = SystemNameData;
            $CurrentSystem.find('span:first-child').removeClass('green');
            if ($CurrentSystem.hasClass('green')) {
                $CurrentSystem.removeClass('green');
            }
            $(charcSystemDiv).data($CurrentSystem.attr('title'), []);
        }
    },

    // Clears the searchbox
    //Author: ZeeshanAK | Date: February 01, 2016 */
    clearSearchBox: function () {
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #txtSearchCharacteristics').val('');
        Clinical_ReviewofSystems.getFilteredCharacteristics('');
    },

    /* Normalize all sections of ROS
      Author: ZeeshanAK | Date: February 02, 2016 */
    markSectionsAsNormal: function (obj) {
        var isToMarkNormal = $(obj).prop("checked");
        if (isToMarkNormal == true) {
            utility.myConfirm('24', function () {
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divReviewofSystemsSystems ul li').find('span:first-child').addClass("green");
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' [name=ReviewofSystemsSectionNormal]').prop('checked', true);
                Clinical_ReviewofSystems.bookIconClassToggle($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').find('[id=bookIconNormalGlobal]'), false);
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divReviewofSystemsSystemSection #ActiveSystemUL').find('i').removeClass('green');//kr
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divReviewofSystemsSystemSection #ActiveSystemUL').find('input:hidden').val('');//kr
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL > li").removeClass('active');
                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #txtSearchCharacteristics').parent().addClass('disableAll');

                //If there's any active Section, Normalize it
                if ($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #ActiveSystemUL li:nth-child(1)').attr("id")) {
                    Clinical_ReviewofSystems.markCurrentActiveSectionAsNormal(false, true);
                    $('#' + Clinical_ReviewofSystems.params.PanelID + ' #ReviewofSystems').find('.toggle').addClass('disableAll');
                } else {

                }
                Clinical_ReviewofSystems.markAllSystemsAsNormalCache();
                Clinical_ReviewofSystems.CharacteristicsDetailsInfo = [];

            }, function () {
                $(obj).prop("checked", false);
            });
        }
        else {
            $(obj).prop("checked", false);

            Clinical_ReviewofSystems.CacheCharacteristicInfo(null, true, false, true);
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL").removeClass('disableAll');
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divReviewofSystemsSystems ul li').removeClass("green");
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divReviewofSystemsSystems ul li span').removeClass("green");
            $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL li").find('input[type=checkbox]').prop("disabled", false);
            Clinical_ReviewofSystems.bookIconClassToggle($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').find('[id=bookIconNormal],[id=bookIconNormalGlobal]').find('[id=bookIcon]'), true);
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' [name=ReviewofSystemsSectionNormal]').prop('checked', false);
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #txtSearchCharacteristics').parent().removeClass('disableAll');
            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divNormalGlobal').find("input:hidden").val('');
            $(obj).parent().next("div.rightInnerAddon").remove();
            Clinical_ReviewofSystems.changeColorOfGlobalBookIcon();
            Clinical_ReviewofSystems.unmarkAllSystemsAsNormalCache();
        }

    },

    /* Load ROS System info for the patient
      Author: ZeeshanAK | Date: February 03, 2016 */
    loadROSSystemInfo: function (fromAddToNote) {
        Clinical_ReviewofSystems.getROSPatientInfo_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var ROSPatientInfoJSON = response.ROSPatientInfo_JSON;
                if (ROSPatientInfoJSON.ROSTemplateId != null && ROSPatientInfoJSON.ROSTemplateId != 0 && !Clinical_ReviewofSystems.params.isShowTemplate) {

                    //Start || 12 July, 2016 || ZeeshanAK || Fix for Data modified message popping up unnecessary.
                    Clinical_ReviewofSystems.loadSystems(ROSPatientInfoJSON.ROSTemplateId, ROSPatientInfoJSON.ROSDataTemplateId, fromAddToNote);
                    //End   || 12 July, 2016 || ZeeshanAK || Fix for Data modified message popping up unnecessary.

                    Clinical_ReviewofSystems.showROSTemplatesTab('LiAddROS');

                    if (ROSPatientInfoJSON != null) {

                        var self = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems");
                        utility.bindMyJSONByName(true, ROSPatientInfoJSON, false, self).done(function () {

                            Clinical_ReviewofSystems.params.ROSSystemInfoID = ROSPatientInfoJSON.ROSSystemInfoID;
                            Clinical_ReviewofSystems.params.mode = "Edit";

                            var $dtReviewofSystemsDate = $('#' + Clinical_ReviewofSystems.params.PanelID + '  #dtReviewofSystemsDate');
                            if ($dtReviewofSystemsDate.val() == '') {
                                $dtReviewofSystemsDate.datepicker("setDate", new Date());
                            } else {
                                $dtReviewofSystemsDate.datepicker("setDate", new Date($dtReviewofSystemsDate.val()));
                            }

                            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfROSSystemInfoID').val(ROSPatientInfoJSON.ROSSystemInfoID);


                            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #chkReviewofSystemssNormal').prop('checked', ROSPatientInfoJSON.ROSisNormal);
                            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hdnDescriptionNormalGlobal').val(ROSPatientInfoJSON.ROSNormalDescription);

                            if (ROSPatientInfoJSON.ROSisNormal == true) {
                                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').find('[id=bookIconNormalGlobal]').removeClass('disabled');
                            } else {
                                $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').find('[id=bookIconNormalGlobal]').addClass('disabled');
                            }

                            Clinical_ReviewofSystems.changeColorOfGlobalBookIcon();

                            $('#' + Clinical_ReviewofSystems.params.PanelID + ' #txtROSComments').val(ROSPatientInfoJSON.ROSComments);

                        });

                        if (ROSPatientInfoJSON.ROSDataTemplateId && Number(ROSPatientInfoJSON.ROSDataTemplateId) < 1) {
                            $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('');
                        }
                    }

                    $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #btnSaveAsDataTemp').show();

                } else {

                    $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #btnSaveAsDataTemp').hide();
                    Clinical_ReviewofSystems.loadTemplateSystems(ROSPatientInfoJSON.ROSTemplateId);
                    Clinical_ReviewofSystems.loadDataTemplateSystems(ROSPatientInfoJSON.ROSTemplateId);
                }

            }

        });
    },

    //Author: Khaleel Ur Rehman.
    //Purpose : To load system Patient characteristic detail.
    // Date : 16-Feb-2016
    loadSysPatCharcDetail: function (cntrl, isCharcDetailExists) {

        if (!$('#' + Clinical_ReviewofSystems.params.PanelID + ' #ReviewofSystems').find('.toggle').hasClass('disableAll')) {
            Clinical_ReviewofSystems.CacheCharacteristicDetailInfo(true);
        }

        Clinical_ReviewofSystems.addActiveClass(cntrl);
        EMRUtility.resetControlValue($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divExamCharacteristics'));

        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #headingSysCharDetails').html($(cntrl).text() + ' Detail');
        var charcDetailId = $(cntrl).find("label").find("input[type='hidden']:eq(1)").attr("id").split('_')[1];
        var detail = $.grep(Clinical_ReviewofSystems.CharacteristicsDetailsInfo, function (item, index) {
            return item.Id == cntrl.id;
        });
        if (isCharcDetailExists != null && isCharcDetailExists != '' && (detail == null || detail.length == 0)) {
            Clinical_ReviewofSystems.getSystemPatientCharcDetail_DBCall(charcDetailId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var detail = JSON.parse(response.SystemCharacteristicsDetails_JSON);
                    Clinical_ReviewofSystems.populateDetailAgainstCharacteristic(detail[0]);
                    Clinical_ReviewofSystems.CacheCharacteristicDetailInfo(false);
                }
            });
        } else {
            if ((detail != null && detail.length != 0)) {
                Clinical_ReviewofSystems.bindCacheCharacteristicDetailInfo(cntrl);
                Clinical_ReviewofSystems.validateDurationDetails();

            }
        }
    },

    //Author: Khaleel Ur Rehman.
    //Purpose : To load system Patient characteristic detail.
    // Date : 16-Feb-2016
    populateDetailAgainstCharacteristic: function (detail) {
        var formId = '#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems';
        $(formId + ' #PreviousHistory').val(detail.PreviousHistory);
        $(formId + ' #ROSCharacteristicsDetailStatusId').val(detail.ROSCharacteristicsDetailStatusId);
        $(formId + ' #Onset').val(detail.Onset);
        $(formId + ' #Duration').val(detail.Duration);
        $(formId + ' #ROSCharacteristicsDetailDurationId').val(detail.ROSCharacteristicsDetailDurationId);
        $(formId + ' #ROSCharacteristicsDetailPatternId').val(detail.ROSCharacteristicsDetailPatternId);
        $(formId + ' #ROSCharacteristicsDetailSeverityId').val(detail.ROSCharacteristicsDetailSeverityId);
        $(formId + ' #ROSCharacteristicsDetailCourseId').val(detail.ROSCharacteristicsDetailCourseId);
        $(formId + ' #ROSCharacteristicsDetailRadiationId').val(detail.ROSCharacteristicsDetailRadiationId);
        $(formId + ' #ROSCharacteristicsDetailFrequencyId').val(detail.ROSCharacteristicsDetailFrequencyId);
        $(formId + ' #ROSCharacteristicsDetailContextId').val(detail.ROSCharacteristicsDetailContextId);
        $(formId + ' #ROSCharacteristicsDetailCharacterCSZId').val(detail.ROSCharacteristicsDetailCharacterCSZId);
        $(formId + ' #ROSCharacteristicsDetailAggravedById').val(detail.ROSCharacteristicsDetailAggravedById);
        $(formId + ' #ROSCharacteristicsDetailRelievedById').val(detail.ROSCharacteristicsDetailRelievedById);
        $(formId + ' #Location').val(detail.Location);
        $(formId + ' #PrecipitatedBY').val(detail.PrecipitatedBY);
        $(formId + ' #AssociatedWith').val(detail.AssociatedWith);
        $(formId + ' #hfROSCharacteristicsDetailsId').val(detail.ROSCharacteristicsDetailsId);
        Clinical_ReviewofSystems.validateDurationDetails();
    },

    /* Add active class if a characteristics li is clicked
       Author: ZeeshanAK | Date: February 09, 2016 */
    addActiveClass: function (selectedLi) {
        $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #ActiveSystemUL > li").removeClass('active');
        $(selectedLi).addClass('active');
        if ($(selectedLi).find('input:checked').length == 0) {
            if (!$('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').prev().hasClass('hidden')) {
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').prev().addClass('hidden');
                countWidth();
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').prev().addClass('hidden').parent().parent().scrollLeft(0);
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').removeClass('active');
            }
        }
        Clinical_ReviewofSystems.enableDisableCharcDetails(selectedLi);
    },

    // Enabling and Disabling of characteristics details
    enableDisableCharcDetails: function (selectedLi) {
        if (selectedLi != null) {
            var objList = $.grep($(selectedLi).find('input:checkbox'), function (element) {
                if ($(element).is(':checked')) {
                    return element;
                }
            });
            if (objList.length > 0) {
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').removeClass('disableAll');
            } else {
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').addClass('disableAll');
            }
        } else {
            if ($('#' + Clinical_ReviewofSystems.params.PanelID + " #divReviewofSystemsSystemSection").find('input:checkbox:not([id*=ReviewofSystemsSectionNormal])').is(':checked') == true) {
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').removeClass('disableAll');
            }
            else if ($('#' + Clinical_ReviewofSystems.params.PanelID + " #divReviewofSystemsSystemSection").find('input:checkbox[id*=ReviewofSystemsSectionNormal]')) {
                $('#' + Clinical_ReviewofSystems.params.PanelID + " #ReviewofSystems").find('.toggle').addClass('disableAll');
            }
        }

    },

    clearInfoForSystemReset: function (obj, SystemID, SystemName) {
        var systemCharc = '#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divReviewofSystemsSystemSection';

        $(systemCharc).find('input[type=checkbox]').prop('checked', false);

        $(systemCharc + ' [id^=Charc_Pos_').parent().parent().removeClass('red');
        $(systemCharc + ' input[id^=Charc_Desc_]').val('');
        $(systemCharc + ' input[id^=selectAllPositive_]').prop('checked', false).parent().parent().removeClass('red');
        $(systemCharc + ' ul li a i').removeClass("green");
        $(systemCharc + " ul li").removeClass('active');
        $(systemCharc + ' ul li').find("input").removeAttr('disabled');
        $(systemCharc + ' ul').removeClass("disableAll");

        var detailObj = $.grep(Clinical_ReviewofSystems.CharacteristicsDetailsInfo, function (item, index) {
            return (item.SystemId == SystemID);
        });
        Clinical_ReviewofSystems.CharacteristicsDetailsInfo.splice(detailObj, 1);

        Clinical_ReviewofSystems.CharcSystemInfo[SystemName] = null;

        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #hdnDescriptionNormal').val('');
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #bookIconNormal').removeClass("green");
        //    $('#' + Clinical_ReviewofSystems.params.PanelID + ' input[id^=ReviewofSystemsSectionNormal]').prop('checked', false);
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' input[id^=chkReviewofSystemssNormal]').prop('checked', false).siblings('a').addClass("disabled");
        $("#" + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val('true');
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #bookIconNormal').addClass("disabled");
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #txtSearchCharacteristics').parent().removeClass('disableAll');

        $(obj).closest("li").removeClass('green');
        $(obj).parent().find('span:first-child').removeClass('green');
        $(obj).closest("li").find("#isSystemNormal").val('');
        $(obj).closest("li").find("#systemNormalDescription").val('');

        Clinical_ReviewofSystems.bookIconClassToggle($(systemCharc + ' input[id^=Charc_Pos_' + SystemID + ']').closest('li').find('[id=bookIcon]'), true);
        Clinical_ReviewofSystems.handleDetailToggleDiv();
    },

    /* Clear system info when red X is clicked on a system. Also, remove the cached info
       Author: ZeeshanAK | Date: February 10, 2016 */
    clearSectionInfo: function (event, obj, SystemID, SystemName) {
        event.stopPropagation();
        var $CharcDIVli = $(obj).closest("li");
        if (($("#divReviewofSystemsSystemSection").find('input[type=checkbox]:checked').length > 0 || $CharcDIVli.hasClass('green'))) {

            var message = 'This will reset all values in ' + SystemName + ' System and clear all information. This action is irreversible and cannot be undone. Would you like to proceed?';
            var sysPatID = $CharcDIVli.find("#systemPatientID").val();
            var sysPatNormal = $CharcDIVli.find("#isSystemNormal").val();
            var CharcID = $CharcDIVli.find("#isCharacteristicsExists").val();

            utility.myConfirm(message, function () {

                if ((CharcID) || (sysPatNormal || sysPatNormal == 'True')) {

                    Clinical_ReviewofSystems.rOSSystemPatientReset_DBCall(sysPatID).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_ReviewofSystems.clearInfoForSystemReset(obj, SystemID, SystemName);
                        }
                    });
                } else {
                    Clinical_ReviewofSystems.clearInfoForSystemReset(obj, SystemID, SystemName);
                }
            }, function () {
            }, 'Reset Confirmation');

        }
    },

    /* To normalize all sections in CharcSystemInfo
           Author: ZeeshanAK | Date: February 16, 2016 */
    normalizeAllSectionsArray: function () {
        $.map($('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
            var sName = n.split('_')[1];
            var objData = JSON.parse(Clinical_ReviewofSystems.CharcSystemInfo[sName]);
            Object.keys(objData).forEach(function (key, index) {
                if (key.indexOf("Charc_Neg_") > -1 || key.indexOf('Charc_Pos_') > -1 || key.indexOf('Charc_Desc') > -1) {
                    objData[key] = '';
                } else if (key.indexOf("ReviewofSystemsSectionNormal") > -1) {
                    objData[key] = 'True';
                }

            });
            Clinical_ReviewofSystems.CharcSystemInfo[sName] = JSON.stringify(objData);
        });
    },

    /* To normalize all sections in CharcSystemInfo
       Author: ZeeshanAK | Date: February 19, 2016 */
    uncheckCharacteristic: function (checkedBox) {
        var $charcLI = $(checkedBox).parents('li');
        var detailObj = $.grep(Clinical_ReviewofSystems.CharacteristicsDetailsInfo, function (item, index) {
            return (item.Id == $charcLI.attr('id'));
        });
        Clinical_ReviewofSystems.CharacteristicsDetailsInfo.splice(detailObj, 1);

        Clinical_ReviewofSystems.bookIconClassToggle($charcLI.find('[id=bookIcon]'), true);
        $charcLI.find("[type=hidden]").val('');

        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #divExamCharacteristics').resetAllControls();
        Clinical_ReviewofSystems.enableDisableCharcDetails();
        Clinical_ReviewofSystems.handleDetailToggleDiv();
    },

    /* To make sure duration has a qualifier selected
        Author: ZeeshanAK | Date: February 25, 2016 */
    validateDurationDetails: function (obj) {
        var $duration = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divExamCharacteristics #Duration');
        var $durationQualifier = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divExamCharacteristics #ROSCharacteristicsDetailDurationId');
        if ($duration.val() != null && $duration.val() != '') {
            $durationQualifier.prop("disabled", false);
        } else {
            $durationQualifier.prop("disabled", true).find("option:first").attr("selected", true);
        }
    },

    unNormalizeAllSectionsArray: function () {
        $.map($('#' + Clinical_ReviewofSystems.params.PanelID + " div#divReviewofSystemsSystems ul").sortable("toArray"), function (n, i) { // just use arr
            var sName = n.split('_')[1];
            var sID = n.split('_')[2];
            var tempData = {
            };
            tempData['ReviewofSystemsSectionNormal' + "_" + sID] = 'False';
            tempData['SystemNormalDescription' + "_" + sID] = '';
            Clinical_ReviewofSystems.CharcSystemInfo[sName] = JSON.stringify(tempData);
        });
    },

    //Author: Muhammad Azhar Shahzad
    //Date: January 26, 2016
    //This function will handle Unload of ReviewofSystems Tab
    unLoadTab: function (nextOrPre, controlToInvoke) {
        //Code for progress note navigation
        // Date: 12-11-2015
        // Change Author: Muhammad Azhar Shahzad
        // If User OPen Review Of Systems From Progress Note
        /*Clicking close “X” button, a prompt message will be displayed
            “Are you want to save the changes?
            The date will be modified with current date.”
            i.	Clicking yes from the prompt will update the date as well as add the Review Of Systems component on the progress notes.
            ii.	Clicking No will close the Review Of Systems popup and will not add Review Of Systems component on Progress notes.
        */
        Clinical_ReviewofSystems.controlToInvoke = controlToInvoke;

        if (Clinical_ReviewofSystems.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_ReviewofSystems.params["PanelID"] + ' #frmClinicalReviewofSystems #hfIsFormHasChange').val() == 'true') {
            utility.myConfirm('Data has been modified. Do you want to save the changes ?', function () {

                Clinical_ReviewofSystems.bNextPrev = true;
                Clinical_ReviewofSystems.ReviewofSystemsSave(true);

            }, function () {
                Clinical_ReviewofSystems.UnloadReviewofSystems(nextOrPre);
            },
           'Confirmation'
           );
        } else {
            if (controlToInvoke != null) {
                Clinical_ReviewofSystems.UnloadReviewofSystems(nextOrPre);
            }
            else {
                Clinical_ReviewofSystems.UnloadReviewofSystems();
            }
        }
    },


    //        }, function () {
    //            Clinical_ReviewofSystems.UnloadReviewofSystems();
    //        },
    //       'Confirmation'
    //       );
    //    } else {
    //        Clinical_ReviewofSystems.UnloadReviewofSystems();
    //    }
    //},
    clearCachingROS: function () {
        $('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems #divNormalGlobal').find("input:hidden").val('');
        Clinical_ReviewofSystems.bookIconClassToggle($('#' + Clinical_ReviewofSystems.params.PanelID + ' #frmClinicalReviewofSystems').find('[id=bookIconNormalGlobal]'), true);
        Clinical_ReviewofSystems.CharacteristicsDetailsInfo = [];
        Clinical_ReviewofSystems.CharcDetailDivJSON = '';
        Clinical_ReviewofSystems.CharcSystemInfo = [];
    },

    UnloadReviewofSystems: function (nextOrPre) {

        Clinical_ReviewofSystems.clearCachingROS();

        if (Clinical_ReviewofSystems.params["FromAdmin"] == "0") {

            if (Clinical_ReviewofSystems.params != null && Clinical_ReviewofSystems.params.ParentCtrl != null) {

                if (Clinical_ReviewofSystems.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {

                    UnloadActionPan(Clinical_ReviewofSystems.params.ParentCtrl, 'Clinical_ReviewofSystems');

                    if (Clinical_ReviewofSystems.controlToInvoke != null) {

                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_ReviewofSystems.controlToInvoke.replace(/\s/g, ''));
                            Clinical_ReviewofSystems.controlToInvoke = null;
                        }, 600);

                    }

                }
                else {
                    UnloadActionPan(Clinical_ReviewofSystems.params.ParentCtrl, 'Clinical_ReviewofSystems');
                }
            }
            else
                UnloadActionPan(null, 'Clinical_ReviewofSystems');
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_ReviewofSystems").remove();
            RemoveAdminTab();
        }
        EMRUtility.scrollToPNcomponent('clinical_reviewofsystems');
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
            Clinical_ReviewofSystems.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
        }
        else {
            $('.toggleVertical').each(function (e) {
                currentPanel = $(this);
                panelChildrens = currentPanel.children().children("section.panel");
                applyWidth = currentPanel.children();
                Clinical_ReviewofSystems.toggleVerticalWidthCtrl(currentPanel, panelChildrens, applyWidth);
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


    //-----------------Progress Note-------------
    // added on January 26, 2016 by Muhammad Azhar Shahzad
    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Call Back function to add component to Progress Note
    addReviewofSystemsToNotes: function () {
        Clinical_ReviewofSystems.ReviewofSystemsSave(true);
    },

    //this function will get Review Of Systems Soap Text and attach that to Progress note
    getReviewofSystemsInfo: function (soapTextROS, UnloadReviewofSystems, reviewOfSystemId) {
        var ROSSystemInfoID = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfROSSystemInfoID').val();
        Clinical_ReviewofSystems.createReviewofSystemsBodyHTML(soapTextROS, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadReviewofSystems, ROSSystemInfoID);
    },

    //This Function will check, if Review Of Systems Soap is already attached in Progress note, if Review Of Systems is not attached than it will create main divs to attach allergy
    checkReviewofSystemsExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';
            $(CompnentSelector).append(' <li class="ReviewofSystemsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_ReviewofSystems title="Review of Systems"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'ReviewofSystemsRevmap\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Review of Systems">Review of Systems</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'ReviewofSystems\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'ReviewofSystems\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_ReviewofSystems> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },


    createReviewofSystemsBodyHTMLFromnotes: function (soapTextROS, NoteHTMLCtrl, UnloadReviewofSystems, ROSSystemInfoID) {
        Clinical_ReviewofSystems.checkReviewofSystemsExists();
        if (soapTextROS == null) {
            soapTextROS = '';
            return;
        }

        var $mainDivReviewofSystems = $(document.createElement('div'));
        var $SectionBodyReviewofSystems = $(document.createElement('section'));
        var $DetailsDiv = $(document.createElement('div'));
        var $ListReviewofSystems = $(document.createElement('ul'));

        $DetailsDiv.attr('id', "Cli_ReviewofSystems_" + ROSSystemInfoID);
        $SectionBodyReviewofSystems.attr('id', "Cli_ReviewofSystems_Main" + ROSSystemInfoID);
        $ListReviewofSystems.attr('class', 'list-unstyled')

        $SectionBodyReviewofSystems.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ReviewofSystems_" + ROSSystemInfoID + '"><i class="fa fa-edit"></i></a>' +
            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ReviewofSystems_Main" + ROSSystemInfoID + '"  ><i class="fa fa-times"></i></a></div> ');

        $ListReviewofSystems.append("<li>" + soapTextROS + "</li>");
        $DetailsDiv.append($ListReviewofSystems);
        $SectionBodyReviewofSystems.append($DetailsDiv);
        if ($(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID).length == 0) {
            $mainDivReviewofSystems.html($SectionBodyReviewofSystems);

            var ReviewofSystemsHtml = $mainDivReviewofSystems.html();
            if (ReviewofSystemsHtml != '') {
                $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().addClass('initialVisitBody');
                if ($(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('section').length > 0) {
                    $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('section').remove();
                    $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().append(ReviewofSystemsHtml);

                } else {
                    $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().append(ReviewofSystemsHtml);
                }
            }

        } else {

            var CommentHTML = "";
            var CommentsID = $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID + ' ul li:Last').attr('id');
            if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                CommentHTML = $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID + ' ul li:Last').get(0).outerHTML;
            }
            $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID).html($SectionBodyReviewofSystems.html());
            $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID + ' ul').append(CommentHTML);

        }

        Clinical_ProgressNote.hoverFunction();
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createReviewofSystemsBodyHTML: function (soapTextROS, NoteHTMLCtrl, UnloadReviewofSystems, ROSSystemInfoID) {

        Clinical_ReviewofSystems.checkReviewofSystemsExists();

        var $RosCntrlOnNotes = $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID);
        var $mainDivReviewofSystems = $(document.createElement('div'));
        var $SectionBodyReviewofSystems = $(document.createElement('section'));
        var $ListReviewofSystems = $(document.createElement('ul'));
        var $DetailsDiv = $(document.createElement('div'));

        $SectionBodyReviewofSystems.attr('id', "Cli_ReviewofSystems_Main" + ROSSystemInfoID);
        $DetailsDiv.attr('id', "Cli_ReviewofSystems_" + ROSSystemInfoID);
        $ListReviewofSystems.attr('class', 'list-unstyled')

        $SectionBodyReviewofSystems.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_ReviewofSystems_" + ROSSystemInfoID + '"><i class="fa fa-edit"></i></a>' +
            '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_ReviewofSystems_Main" + ROSSystemInfoID + '"  ><i class="fa fa-times"></i></a></div> ');

        if (soapTextROS == null) {
            soapTextROS = '';
            Clinical_ProgressNote.saveComponentSOAPText('Review of Systems');
            return;
        }

        $ListReviewofSystems.append("<li>" + soapTextROS + "</li>");
        $DetailsDiv.append($ListReviewofSystems);
        $SectionBodyReviewofSystems.append($DetailsDiv);


        if ($RosCntrlOnNotes.length == 0) {

            $mainDivReviewofSystems.html($SectionBodyReviewofSystems);
            Clinical_ReviewofSystems.updateReviewofSystemsHtml($mainDivReviewofSystems.html(), ROSSystemInfoID, NoteHTMLCtrl);

        } else {

            var CommentHTML = "";
            var $CommentsID = $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent().find('#Cli_ReviewofSystems_Main' + ROSSystemInfoID + ' ul li:Last');
            if ($CommentsID.attr('id') != null && $CommentsID.attr('id').indexOf("Comments") >= 0) {
                CommentHTML = $CommentsID.get(0).outerHTML;
            }
            $RosCntrlOnNotes.html($SectionBodyReviewofSystems.html());
            $RosCntrlOnNotes.find('ul').append(CommentHTML);

            Clinical_ProgressNote.saveComponentSOAPText('Review of Systems');
            Clinical_ReviewofSystems.updateReviewofSystemsHtml("", ROSSystemInfoID, NoteHTMLCtrl);
            Clinical_ProgressNote.hoverFunction();
        }

        if (UnloadReviewofSystems == true) {
            Clinical_ReviewofSystems.UnloadReviewofSystems(Clinical_ReviewofSystems.bNextPrev);
        }
    },

    // This Function is called by Progress Notes (Fill ReviewofSystems Func, CopyAllNotesCategories)
    updateReviewofSystemsHtml: function (ReviewofSystemsHtml, ROSSystemInfoID, NoteHTMLCtrl) {
        var $RosControlOnNotes = $(NoteHTMLCtrl + ' clinical_ReviewofSystems').parent().parent();
        $RosControlOnNotes.addClass('initialVisitBody');
        if (ReviewofSystemsHtml != '') {
            if ($RosControlOnNotes.find('section').length > 0) {
                $RosControlOnNotes.find('section').remove();
                $RosControlOnNotes.append(ReviewofSystemsHtml);

            } else {
                $RosControlOnNotes.append(ReviewofSystemsHtml);
            }

        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (ReviewofSystemsHtml != '') {
            Clinical_ProgressNote.saveComponentSOAPText('Review of Systems');
            Clinical_ProgressNote.hoverFunction();
        }

    },

    //This Function detach Review Of Systems From progress note
    detach_ComponentsReviewofSystems: function (ComponentName, IsUpdate, ReviewofSystemsComponentRemove) {

        utility.myConfirm('28', function () {
            Clinical_ReviewofSystems.disAssociateSystemsAgainstNoteId().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .ReviewofSystemsComponent').attr('NoteComponentId');
                    if (ReviewofSystemsComponentRemove) {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Review of Systems']").remove();
                        if (Clinical_ProgressNote.params["TemplateName"])
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').parent().parent().addClass('hidden');
                        else
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').parent().parent().remove();
                    } else {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_ReviewofSystems').parent().parent().find('section[id*="Cli_ReviewofSystems_Main"]').remove();
                    }
                    if (IsUpdate) {
                        if (NoteComponentId && NoteComponentId != "NCDummyId") {
                            var promise = [];
                            if (Clinical_ProgressNote.params["TemplateName"]) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_ReviewofSystems').parent().parent().addClass('hidden');
                                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Review of Systems', true))
                            }
                            else
                                promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                            $.when.apply($, promise).done(function () {
                                if (Clinical_ProgressNote.params["TemplateName"] == "")
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_ReviewofSystems').parent().parent().remove();
                                Clinical_ProgressNote.ShowHideComponetsHeaders();
                                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                            });
                        }
                        Clinical_ProgressNote.hoverFunction();
                    }
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }, function () {
        }, '1');
    },

    //This Functions ask for Detaching Review of Systems from Progress Note for current Patient Selected
    detachReviewofSystemsFromNotes: function (ROSSystemInfoID) {


        utility.myConfirm('28', function () {
            EMRUtility.scrollToPNcomponent('clinical_reviewofsystems');
            Clinical_ReviewofSystems.disAssociateSystemsAgainstNoteId().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + ROSSystemInfoID).remove();

                    Clinical_ProgressNote.saveComponentSOAPText('Review of Systems');
                    Clinical_ProgressNote.hoverFunction();
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }, function () {
        }, '1');

    },

    //This Functions attached Review of Systems to Progress Note for current Patient Selected
    AttachReviewofSystemsFromNotes: function (ROSSystemInfoID) {

        if (ROSSystemInfoID) {
            Clinical_ProgressNote.saveComponentSOAPText('Review of Systems');
            Clinical_ProgressNote.hoverFunction();
            $('#' + ROSSystemInfoID).remove();
        }
    },

    //If ReviewofSystems Component which is dropeed in Progress note has no ReviewofSystems attached, than it will call for Latest ReviewofSystems for this patient
    getLatestReviewofSystemsByPatientId: function () {

        Clinical_ReviewofSystems.getLatestClinical_ReviewofSystemsByPatientId_DBCall().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                var ROSPatientInfoJSON = response.ROSPatientInfo_JSON;
                var ROSSystemInfoID = ROSPatientInfoJSON.ROSSystemInfoID;
                Clinical_ReviewofSystems.createReviewofSystemsBodyHTML(ROSPatientInfoJSON.SoapText, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, ROSSystemInfoID);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    toggleAttribute: function (control, IsPositive, ROSSystemPatientCharacteristicsID, event) {
        if (event != null) {
            event.preventDefault();
            event.stopPropagation();
        }

        Clinical_ProgressNote.IsNoteComponentAvaliable(false, "ReviewofSystems").done(function (res) {
            if (res == true) {

                Clinical_ReviewofSystems.toggleAttribute_DbCall(IsPositive, ROSSystemPatientCharacteristicsID).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (IsPositive) {
                            $(control).text(' ( - ) ');
                            $(control).attr('onclick', 'Clinical_ReviewofSystems.toggleAttribute(this,false,' + ROSSystemPatientCharacteristicsID + ',event);')
                        } else {
                            $(control).html(' ( + ) ');
                            $(control).attr('onclick', 'Clinical_ReviewofSystems.toggleAttribute(this,true,' + ROSSystemPatientCharacteristicsID + ',event);')
                        }
                        $(control).toggleClass('red');
                        $(control).toggleClass('green');
                        Clinical_ProgressNote.saveComponentSOAPText('Review of Systems');
                        Clinical_ProgressNote.hoverFunction();
                    }
                });


            }
        });


    },
    //-----Server calls of Notes----------

    //Author : Khaleel Ur Rehman.
    //Purpose : disAssociate Systems Against Patient.
    //Date : 15-Feb-2016.
    disAssociateSystemsAgainstNoteId: function () {
        var objData = {};
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["ROSSystemInfoID"] = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfROSSystemInfoID').val();
        objData["commandType"] = "disAssociate_Systems_AgainstNoteId";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewofSystem", "ReviewofSystems");
    },


    getROSTemplateSystems_DBCall: function (ROSTemplateId) {
        var objData = new Object();
        objData["ROSTemplateId"] = (ROSTemplateId == null) ? 0 : ROSTemplateId;
        objData["ROSSystemInfoID"] = Clinical_Notes.params.ROSSystemInfoID;
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["commandType"] = "load_ros_template_systems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },

    /* DB call to load all Systems
    Author: ZeeshanAK | Date: January 28, 2016 */
    getROSSystems_DBCall: function (ROSTemplateId) {

        var objData = new Object();
        objData["ROSTemplateId"] = (ROSTemplateId == null || ROSTemplateId == '') ? 0 : ROSTemplateId;
        objData["ROSDataTemplateId"] = Clinical_ReviewofSystems.getROSDataTemplateID();
        objData["ROSSystemInfoID"] = Clinical_Notes.params.ROSSystemInfoID;
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["commandType"] = "load_ros_systems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },
    getROSSystemsFromProvidersNote_DBCall: function (ROSDataTemplateId, TemplateID, NotesID) {
        var objData = new Object();
        objData["ROSTemplateId"] = (TemplateID == null || TemplateID == '') ? 0 : TemplateID;
        objData["ROSDataTemplateId"] = ROSDataTemplateId;
        objData["NotesId"] = NotesID;
        objData["commandType"] = "load_and_save_ros_systems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },

    /* DB call to load all Characteristics for a specific System
    Author: ZeeshanAK | Date: January 28, 2016 */
    getROSSystemsCharacteristics_DBCall: function (SystemId, ROSSystemPatientID) {
        var objData = new Object();
        objData["ROSSystemPatientID"] = ROSSystemPatientID;
        objData["ROSDataTemplateId"] = Clinical_ReviewofSystems.getROSDataTemplateID();
        objData["ROSSystemId"] = SystemId;
        objData["commandType"] = "load_ros_systems_characteristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },

    /* DB call to load ROS System PatientInfo
 Author: ZeeshanAK | Date: January 28, 2016 */
    getROSPatientInfo_DBCall: function () {

        var objData = new Object();
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["ROSSystemInfoID"] = $('#' + Clinical_ReviewofSystems.params.PanelID + ' #hfROSSystemInfoID').val();
        objData["ROSDataTemplateId"] = Clinical_ReviewofSystems.getROSDataTemplateID();
        objData["commandType"] = "load_ros_systems_patientinfo";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },

    getROSDataTemplateID: function () {
        var ROSDataTemplateId = $('#' + Clinical_ReviewofSystems.params.PanelID + " #frmClinicalReviewofSystems #hfROSDataTemplateId").val();
        var parmROSDataTemplateId = Clinical_ReviewofSystems.params.ROSDataTemplateId;
        if (ROSDataTemplateId != parmROSDataTemplateId && parmROSDataTemplateId != null && parmROSDataTemplateId != '') {
            ROSDataTemplateId = parmROSDataTemplateId;
        }
        return (ROSDataTemplateId == null || ROSDataTemplateId == '') ? -1 : ROSDataTemplateId;
    },
    //DB call to load Charcteristics Detail
    getSystemPatientCharcDetail_DBCall: function (charcDetailId) {
        var objData = new Object();
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["ROSSystemPatientCharacteristicsID"] = charcDetailId;
        var ROSDataTemplateId = Clinical_ReviewofSystems.params.ROSDataTemplateId;
        objData["ROSDataTemplateId"] = Clinical_ReviewofSystems.getROSDataTemplateID();
        objData["commandType"] = "load_Characteristic_Detail";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },

    // DB Call to Reset ROS System Patient
    rOSSystemPatientReset_DBCall: function (sysPatId) {
        var objData = new Object();
        objData["ROSSystemPatientID"] = sysPatId;
        objData["commandType"] = "ros_system_patient_reset";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },

    /* DB call to Delete Characteristics Details Info
        Author: ZeeshanAK | Date: February 19, 2016 */
    deleteSystemPatientCharcDetail_DBCall: function (charcDetailId) {
        var objData = new Object();
        objData["RemoveSystemCharcDetails"] = true
        objData["ROSSystemPatientCharacteristicsID"] = charcDetailId;
        objData["commandType"] = "delete_characteristics_details";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewOfSystems");
    },

    // DB call to detach  ROS from notes
    DetachReviewofSystemsFromNotes_DBCall: function (ROSSystemInfoID) {
        var objData = {
        };
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ROSSystemInfoID"] = ROSSystemInfoID;
        objData["commandType"] = "detach_ReviewofSystems_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewofSystems");
    },

    // DB call to attach  ROS from notes
    AttachReviewofSystemsFromNotes_DBCall: function (ROSSystemInfoID) {
        var objData = {
        };
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ROSSystemInfoID"] = ROSSystemInfoID;

        objData["commandType"] = "attach_ReviewofSystems_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewofSystems");
    },

    // DB call to get latest ROS by Patient ID
    getLatestClinical_ReviewofSystemsByPatientId_DBCall: function () {
        var objData = new Object();
        objData["NotesId"] = Clinical_Notes.params.NotesId;
        objData["ROSSystemInfoID"] = -1;
        objData["ROSDataTemplateId"] = Clinical_ReviewofSystems.getROSDataTemplateID();
        objData["commandType"] = "getlatest_ReviewofSystemsby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewofSystems");
    },

    toggleAttribute_DbCall: function (IsPositive, ROSSystemPatientCharacteristicsID) {
        var objData = new Object();
        objData["IsPositive"] = IsPositive;
        objData["ROSSystemPatientCharacteristicsID"] = ROSSystemPatientCharacteristicsID;
        objData["commandType"] = "toggle_ROSSystemPatientCharacteristics";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystem", "ReviewofSystems");
    },


    searchROSDataTemplate_DBCall: function () {

        var objData = new Object();
        objData["ProviderId"] = Clinical_ProgressNote.params.CurrentNotesProviderId
        objData["commandType"] = "Load_ROS_DataTemplate_ForProvider";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ReviewOfSystemDataTemplate", "ReviewOfSystemDataTemplate");

    },
}