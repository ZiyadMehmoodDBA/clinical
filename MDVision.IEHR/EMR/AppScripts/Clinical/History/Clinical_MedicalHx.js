Clinical_MedicalHx = {
    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This file will handle all actions performed for Medical History and it's child handling
    //Once MedicalHx will be created then it's child can be created then.
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    currentSelectedDisease: [],
    bNextPrev: false,
    controlToInvoke: null,
    FavListName: 'ClinicalMedicalHx',
    medicalHxJSON: '',
    medicalDate: '',
    unremarkable: false,
    overallComments: '',
    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will be called once tab is clicked, it expects parameters to be used for MedicalHx
    Load: function (params) {
        Clinical_MedicalHx.params = params;

        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (Clinical_MedicalHx.params.mode == null) {
            Clinical_MedicalHx.params.mode = "Add";
        }
        if (Clinical_MedicalHx.params.PanelID != 'pnlClinicalMedicalHx') {
            Clinical_MedicalHx.params.PanelID = Clinical_MedicalHx.params.PanelID + ' #pnlClinicalMedicalHx';
        } else {
            Clinical_MedicalHx.params.PanelID = 'pnlClinicalMedicalHx';
            Clinical_MedicalHx.params.CurrentNotesProviderId = "";
        }
        if (typeof Clinical_MedicalHx.params.ParentCtrl != typeof undefined && Clinical_MedicalHx.params.ParentCtrl != null && Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
            if (Clinical_MedicalHx.params.PanelID.indexOf("pnlClinicalProgressNote") < 0) {
                Clinical_MedicalHx.params.PanelID = "pnlClinicalProgressNote " + "#" + Clinical_MedicalHx.params.PanelID;
            }
        }
        Clinical_MedicalHx.ResetFormData();
        //if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabNotes"  ) {
        //    Clinical_MedicalHx.bIsFirstLoad = true;
        //    $('#divViewHistorySummary').addClass('hidden');
        //    $(' #pnlClinicalMedicalHx').removeClass('row');
        //}


        if (Clinical_MedicalHx.bIsFirstLoad) {
            EMRUtility.setFavoriteSectionStyle(Clinical_MedicalHx.params.PanelID);
            Clinical_MedicalHx.favoriteListSearch();
            var MedicalHxId = "";
            if (Clinical_MedicalHx.params.mode == "Add" || Clinical_MedicalHx.params.MedicalHxId == null || Clinical_MedicalHx.params.MedicalHxId == "" || Clinical_MedicalHx.params.MedicalHxId == "-1") {
                MedicalHxId = "-1";
            }
            else if (Clinical_MedicalHx.params.mode == "Edit") {
                MedicalHxId = Clinical_MedicalHx.params.MedicalHxId;
                //Clinical_MedicalHx.MedicalHxEdit(MedicalHxId);
            }

            //Load Dropdown

            var self = $('#' + Clinical_MedicalHx.params.PanelID);

            self.loadDropDowns(true).done(function () {
                //Start 11/01/2016 Muhammad Arshad Loading MedicalHx Tab and unserializ form
                $.when(Clinical_MedicalHx.loadMedicalHxTabnUnserializeForm()).then(function () {
                    Clinical_MedicalHx.loadfavoriteListContent($("#" + Clinical_MedicalHx.params.PanelID + " #ddlFavoriteListMedicalHx"));

                    if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        $('#' + Clinical_MedicalHx.params.PanelID + ' #btnTobaccoSave').addClass('hidden');
                        $('#' + Clinical_MedicalHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');

                        var details = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").clone();
                        $(details).resetAllControls(null);
                        Clinical_MedicalHx.medicalHxJSON = $(details).getMyJSONByName();
                    }
                    else {
                        $('#' + Clinical_MedicalHx.params.PanelID + ' #btnTobaccoSave').removeClass('hidden');
                    }

                });
                //End 11/01/2016 Muhammad Arshad Loading MedicalHx Tab and unserializ form
            });
            Clinical_MedicalHx.validateMedicalHx();
            //end Load Dropdown
            utility.CreateDatePicker(Clinical_MedicalHx.params.PanelID + '  #dtMedicalHxDate', function () {
            }, true);
            //Start 2/02/2016 Abid Ali , for bug# 242
            EMRUtility.ValidateFromToDate('frmClinicalMedicalHx', 'dtMedicalDiseaseFromDate', 'dtMedicalDiseaseToDate', true, function () { }, function () { }, "To Date should be greater than From Date");
            //End 2/02/2016 Abid Ali , for bug# 242
            //utility.CreateDatePicker(Clinical_MedicalHx.params.PanelID + '  section#sectionDiseaseDetails #dtpSexualLMP', function () {
            //}, false);

            if ($('#' + Clinical_MedicalHx.params.PanelID + ' #PatientProfile #hfPatientId').val() != "") {
                $('#' + Clinical_MedicalHx.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            }
            //22/12/2015//AhmadRaza//Form Serialization
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('serialize', $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').serialize());


            //Code for progress note navigation
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_MedicalHx.params.PanelID + ' #pnlClinicalMedicalHx').removeClass('row');
                EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_MedicalHx.params.PanelID, 'History', 'MedicalHx', 'Clinical_MedicalHx.unLoadTab(true);', null, true);
                $('#' + Clinical_MedicalHx.params.PanelID + ' #btnAddVitalsOnNote').show();
                if ($('#' + Clinical_MedicalHx.params.PanelID + " div#MedicalHxDisease ul#ulMedicalDisease li.active").length > 0) {
                    $('#' + Clinical_MedicalHx.params.PanelID + ' #btnAddVitalsOnNote').prop('disabled', false);
                }
                else {
                    $('#' + Clinical_MedicalHx.params.PanelID + ' #btnAddVitalsOnNote').prop('disabled', true);
                }
                //  $('#' + Clinical_MedicalHx.params.PanelID + '  #dtMedicalHxDate').prop('disabled', true);
            } else {
                $('#' + Clinical_MedicalHx.params.PanelID + ' #btnAddVitalsOnNote').hide();
                $('#' + Clinical_MedicalHx.params.PanelID + '  #dtMedicalHxDate').prop('disabled', false);
            }


            //Begin 18-01-2016 syed zia , for number pad
            Clinical_MedicalHx.domReadyFunction();
            //End 18-01-2016 syed zia , for number pad

            // Clinical_MedicalHx.bIsFirstLoad = false;
        }
        if (EMRUtility.getFreeTextStatus("Clinical_MedicalHx")) {
            var panel = "#pnlClinicalMedicalHx";
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalMedicalHx";
            }

            $(panel + " #DivICDAutoComplete").hide();
            if ($(panel + " #DivFreeText").hasClass("hidden")) {
                $(panel + " #DivFreeText").removeClass("hidden");
            }
            $(panel + " #rdSearchFreeText").prop('checked', 'checked');
        }
        else {
            var panel = "#pnlClinicalMedicalHx";
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalMedicalHx";
            }

            $(panel + " #DivICDAutoComplete").show();
            if (!$(panel + " #DivFreeText").hasClass("hidden")) {
                $(panel + " #DivFreeText").addClass("hidden");
            }
            $(panel + " #rdSearchICD").prop('checked', 'checked');
        }

        //if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
        //    $('#' + Clinical_MedicalHx.params.PanelID + ' #btnTobaccoSave').addClass('hidden');
        //    $('#' + Clinical_MedicalHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');

        //    var details = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").clone();
        //    $(details).resetAllControls(null);
        //    Clinical_MedicalHx.medicalHxJSON = $(details).getMyJSONByName();
        //}
        //else {
        //    $('#' + Clinical_MedicalHx.params.PanelID + ' #btnTobaccoSave').removeClass('hidden');
        //}
        if (EMRUtility.getFavListStatus(Clinical_MedicalHx.FavListName)) {
            $('#' + Clinical_MedicalHx.params.PanelID + " #favSectionDiv").addClass("toggledHor");
            $('#' + Clinical_MedicalHx.params.PanelID + " #FormDiv").addClass("toggleHorContainer");
        }
        else {
            $('#' + Clinical_MedicalHx.params.PanelID + " #favSectionDiv").removeClass("toggledHor");
            $('#' + Clinical_MedicalHx.params.PanelID + " #FormDiv").removeClass("toggleHorContainer");
        }
    },

    favoriteListSearch: function () {
        var ProviderId = null;
        if (Clinical_MedicalHx.params.CurrentNotesProviderId != "undefined" && Clinical_MedicalHx.params.CurrentNotesProviderId && Clinical_MedicalHx.params.CurrentNotesProviderId !="")
            ProviderId = Clinical_MedicalHx.params.CurrentNotesProviderId;

        Favorite_ProcedureOrder.searchFavoriteList_DBCall("MedicalHistory", null, 1, 5000, ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + Clinical_MedicalHx.params.PanelID + ' #ddlFavoriteListMedicalHx');
                var favouriteProcedures = JSON.parse(response.FavoriteListJSON)
                $ddl.empty();
                $ddl.append($('<option/>', {
                    id: -1,
                    value: -1,
                    html: "- Select -"
                }));
                $.each(favouriteProcedures, function (i, item) {
                    if (item.Name != "") {
                        $ddl.append(
                          $('<option/>', {
                              id: item.FavoriteListId,
                              value: item.FavoriteListId,
                              html: item.Name,
                          })
                        );
                    }

                });
                if (favouriteProcedures.length > 0) {
                    EMRUtility.getFavListValue(Clinical_MedicalHx.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + Clinical_MedicalHx.params.PanelID + " #ddlFavoriteListMedicalHx option[value='" + response1.favListVal + "']").length > 0) {
                                    $ddl.val(response1.favListVal);
                                    $ddl.trigger("onchange");
                                }
                                else {
                                    if (favouriteProcedures.length == 1) {
                                        $ddl.val(favouriteProcedures[0].FavoriteListId);
                                        $ddl.trigger("onchange");
                                    }
                                    else if (favouriteProcedures.length > 1) {
                                        $ddl.trigger("onchange");
                                    }
                                }
                            }
                            else {
                                if (favouriteProcedures.length == 1) {
                                    $ddl.val(favouriteProcedures[0].FavoriteListId);
                                    $ddl.trigger("onchange");
                                }
                                else if (favouriteProcedures.length > 1) {
                                    $ddl.trigger("onchange");
                                }
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                //   $ddl.trigger("onchange");

            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
        });

    },

    showMedicalHxHistory: function (medicalHxId) {

        var parentCtrlId = Clinical_MedicalHx.params.TabID;
        var grantParentCtrlId = null;
        if (Clinical_MedicalHx.params.TabID == "clinicalTabProgressNote") {

            parentCtrlId = "Clinical_HistorySummary";
            grantParentCtrlId = "pnlClinicalProgressNote";
        }
        EMRUtility.showCurrentItemHistory(Clinical_MedicalHx.params.PanelID, null, null, "MedicalHx,MedicalHx_Disease", null, parentCtrlId, medicalHxId, grantParentCtrlId);
    },

    //Function Name: disableEnterKey
    //Author: Ahmad Raza
    //Date: 17-05-2016
    //Description: To prevent submition of form on Enter Press
    disableEnterKey: function (e) {

        if (e.which == 13) // Enter key = keycode 13
        {
            e.preventDefault();
        }
    },

    //Start Farooq Ahmad 01/29/2016
    enableDurationValidation: function () {

        $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('bootstrapValidator').enableFieldValidators('MedicalDiseaseDurationPeriod', false);
        $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('bootstrapValidator').enableFieldValidators('MedicalDiseaseDurationLength', false);
        var stayLength = $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #txtMedicalDiseaseDurationLength').val();
        var ddlVal = $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #ddlMedicalDiseaseDurationPeriod').val();
        if (stayLength != null && stayLength != "") {
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('bootstrapValidator').enableFieldValidators('MedicalDiseaseDurationPeriod', true);
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #lblDuration').html('Duration<span class="required">*</span>');
        }
        else {
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('bootstrapValidator').enableFieldValidators('MedicalDiseaseDurationPeriod', false);
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #lblDuration').html('Duration');
        }
        if (ddlVal != null && ddlVal != "") {
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('bootstrapValidator').enableFieldValidators('MedicalDiseaseDurationLength', true);
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #lblDuration').html('Duration<span class="required">*</span>');

        } else {
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('bootstrapValidator').enableFieldValidators('MedicalDiseaseDurationLength', false);
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #lblDuration').html('Duration');
        }

    },
    //End Farooq Ahmad 01/29/2016
    //Start//18/01/2016//Ahmad Raza//Implimented validation on Procedure field
    validateMedicalHx: function () {
        $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   MedicalDiseaseDurationPeriod: {
                       group: '.col-xs-8',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   MedicalDiseaseDurationLength: {
                       group: '.col-xs-4',
                       enabled: false,
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
            if (e.type == "success") {
                Clinical_MedicalHx.MedicalHxSave("Disease");
            }

            e.type = "";


        });
        Clinical_MedicalHx.enableDurationValidation();
    },
    //End//18/01/2016//Ahmad Raza//Implimented validation on Procedure field

    //Begin 18-01-2016 syed zia , for number pad
    domReadyFunction: function () {
        $(function () {

            (function ($) {
                'use strict';
                $(function () {
                    $('#' + Clinical_MedicalHx.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);

            $('#' + Clinical_MedicalHx.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });


            $('.toggleHorSmallLeft section').unbind('click').bind("click", function (e) {
                $(this).parent().toggleClass("toggled");
                Clinical_MedicalHx.toggleHorSmallLeftIcon($(this));

            });
            //EMR-70 Bug number Resolution
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx [data-plugin-keyboard-numpad]').keyboard({
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

    },

    //End 19-01-2016 syed zia , for number pad


    //Author: Muhammad Arshad
    //Date: 07-01-2016
    //This function will handle Resetting/Clearing of MedicalHx Details

    ResetFormData: function () {
        //Start//15/01/2016//Ahmad Raza//Implimented business rule for confirm dialog on Reset
        $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').resetAllControls(null);
        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        $('#' + Clinical_MedicalHx.params.PanelID + ' #sectionDiseaseDetails').find('input[type=text], textarea').val('');
        $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #ulMedicalDisease').html("");
        $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #MedicalHxDisease').removeClass('disableAll');
        //End//15/01/2016//Ahmad Raza//Implimented business rule for confirm dialog on Reset
    },

    //Author: Muhammad Arshad
    //Date: 12-09-2015
    //This function will handle fill of MedicalHx Statuses like SmokingStatus,AlcoholStatus,DrugAbuseStatus,SexualHxStatus
    loadMedicalHxDiseases: function (Crtl, result, StatusType) {
        var currentLiClass = "";
        var currentLiClick = "";
        var ParentDiv = "";
        if (StatusType != null && StatusType.toLowerCase() == "disease") {
            Crtl = '#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx div#MedicalHxDisease #ulMedicalDisease";
            ParentDiv = "MedicalHxDisease";
        }
        if ($(Crtl).length > 0)
            l = $(Crtl);

        l.empty();


        var isFirstLi = true;
        //$.each(result, function (j, item) {
        //    if (item.Value != "") {
        //        if (isFirstLi == true) {
        //            currentLiClass = 'class="active"';
        //            isFirstLi = false;
        //        }
        //        else {
        //            currentLiClass = "";
        //        }
        //        var onClick = currentLiClick == "" ? "" : currentLiClick + "(this,'" + String(item.Name) + "');";
        //        //item.Value = item.Value == "" ? 0 : item.Value;
        //        l.append('<li id="' + item.Value + '" ' + currentLiClass + ' onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '"><a href="#' + ParentDiv + '">' + item.Name + ' </a></li>');
        //    }

        //});
        $.each(result, function (j, item) {
            //item.Value = item.Value == "" ? 0 : item.Value;
            var li = "";
            if (item.FreeTextICD != null && typeof item.FreeTextICD != "undefiend" && item.FreeTextICD != "") {
                li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Desc=\"" + $.trim(item.FreeTextICD) + "\" FreeText=\"" + $.trim(item.FreeTextICD) + "\"><a href='#'>" + $.trim(item.FreeTextICD) + "<span class='removeIconListHover' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            }
            else {
                li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Code=\"" + $.trim(item.ICD9Code) + "\" icd9Desc=\"" + $.trim(item.ICD9CodeDescription) + "\" icd10Code=\"" + $.trim(item.ICD10Code) + "\" icd10Desc=\"" + $.trim(item.ICD10CodeDescription) + "\" snomedCode=\"" + $.trim(item.SNOMEDID) + "\" snomedDesc=\"" + $.trim(item.SNOMEDDescription) + "\"><a href='#'>" + $.trim(item.ICD10Code) + " - " + $.trim(item.ICD9CodeDescription) + "<span class='removeIconListHover' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            }
            l.append(li);
        });
        ///$('#' + Clinical_MedicalHx.params.PanelID + ' #ulMedicalDisease li:first').trigger('click');
        //
        if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.MedicalHx != null && Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList != null) {
            $(Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList).each(function () {
                if ($(this)[0].DiseaseLi != null || $(this)[0].DiseaseLi != undefined) {
                    l.append($(this)[0].DiseaseLi);
                }
            });
        }

        var previouslySelectedDisease = Clinical_MedicalHx.currentSelectedDisease["DiseaseId"];
        $(" div#MedicalHxDisease ul#ulMedicalDisease li#" + previouslySelectedDisease).trigger('click');
    },

    //Start//11/01/2016//Ahmad Raza//Logic implimented to open CPT Codes window
    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_MedicalHx";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Clinical_MedicalHx.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_MedicalHx.params.PanelID);
    },
    //End//11/01/2016//Ahmad Raza//Logic implimented to open CPT Codes window

    //Start//11/01/2016//Ahmad Raza//Logic implimented to bind CPT code to Procedure field of MedicalHx
    bindAutoComplete: function (element) {
        var hiddenCrtl = $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode');
        if ($(hiddenCrtl).val() == "") {
            $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode').val("");
            $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val("");
            $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val("");
            $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDCode').val("");
            $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDDescription').val("");
        }
        else {
            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_MedicalHx", null, true);
        }
    },
    //End//11/01/2016//Ahmad Raza//Logic implimented to bind CPT code to Procedure field of MedicalHx

    //Start//11/01/2016//Ahmad Raza//Logic implimented to get latest Medical history of patient
    getLatestMedicalHxByPatientId: function (hideAlertMessage) {

        Clinical_MedicalHx.getLatestClinical_MedicalHxByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_MedicalHx.createMedicalHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }

        });

    },

    getAutoPopulateSetting_DBCall: function () {

        var objData = new Object();
        objData["UserId"] = globalAppdata["AppUserId"];
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["ComponentType"] = "MedicalHistory";
        objData["commandType"] = "getautopopulatesetting";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");

    },


    getLatestClinical_MedicalHxByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_medicalhxby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "medicalHistory");
    },
    //End//11/01/2016//Ahmad Raza//Logic implimented to get latest Medical history of patient

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle fill of MedicalHx and it's childs as specified by MedicalHxType


    loadMedicalHxComponent: function (MedicalHxType, diseaseId, isDiseaseFill, isNewLoad, isRefreshHistoryGrid) {
        var dfd = $.Deferred();
        BackgroundLoaderShow(true);
        Clinical_MedicalHx.fillMedicalHx(MedicalHxType, diseaseId).done(function (response) {
            //if (isNewLoad == true) {
            //    if ($('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfSelectedDisease").val() != "" && $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfSelectedDisease").val() != "undefined") {
            //        var list = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ulMedicalDisease li#" + $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfSelectedDisease").val());

            //        $(list).addClass('active');
            //        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #sectionDiseaseDetails").removeClass("disableAll");
            //        list.trigger('click');

            //        return;

            //    }
            //}
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    var DiseaseLoadDetails = JSON.parse(response.MedicalHxDiseaseLoad_JSON);

                    if (isDiseaseFill != true) {
                        Clinical_MedicalHx.loadMedicalHxDiseases('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx div#MedicalHxDisease #ulMedicalDisease", DiseaseLoadDetails, "disease");
                    }

                    //Begin 12-28-2015 Muhammad Arshad Bug# EMR-161 Medical History Clinical Module -> Date
                    if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        /* Start 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                        //$('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #dtMedicalHxDate").addClass("disableAll");
                        /* End 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                    }
                    //End 12-28-2015 Muhammad Arshad Bug# EMR-161 Medical History Clinical Module -> Date

                    var Medicalhx_detail = JSON.parse(response.MedicalHxFill_JSON);
                    if (Medicalhx_detail.MedicalHxId > 0) {
                        Clinical_MedicalHx.params.HxTypeId = Medicalhx_detail.MedicalHxId;
                        var MainMedicalHx = JSON.parse(response.MedicalHxFill_JSON);

                        //if (MainMedicalHx != {}) {
                        //    var $row = $('<tr/>');
                        //    $row.append('<td style="display:none;">' + MainMedicalHx.MedicalHxId + '</td><td>' + MainMedicalHx.IsCreatedOrModified + '</td><td>' + MainMedicalHx.MedicalHxSoapText + '</td><td>' + MainMedicalHx.LastUpdated + '</td>');
                        //    $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvMedicalHx tbody").html($row);

                        //}
                        if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            $('#' + Clinical_MedicalHx.params.PanelID + " #dtMedicalHxDate").removeClass('disableAll');
                        }
                        else {
                            $('#' + Clinical_MedicalHx.params.PanelID + " #dtMedicalHxDate").addClass('disableAll');
                        }

                        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #aHistory").removeClass('hidden');
                        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #aHistory").attr('onclick', 'Clinical_MedicalHx.showMedicalHxHistory(' + Medicalhx_detail.MedicalHxId + ')');

                        if (isRefreshHistoryGrid == null) {
                            Clinical_MedicalHx.BindCurrentMedicationSoapText(response, DiseaseLoadDetails.length > 0);
                        }

                    }
                    else {
                        var $row = $('<tr/>');
                        $row.append('<td style="display:none;"></td><td>&nbsp;</td><td>No Known Medical History</td><td></td>');
                        $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvMedicalHx tbody").html($row);
                        $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #divSwitch").addClass('disableAll');
                    }
                    var Disease_detail = JSON.parse(response.DiseaseFill_JSON);
                    var self = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx");
                    if (isDiseaseFill != true) {
                        utility.bindMyJSONByName(true, Medicalhx_detail, false, self).done(function () {

                            //Start//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225
                            var upperDate = self.find('input[name*="MedicalHxDate"]');

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
                                //Start//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225 Vitals in Clinical Module -> Add Vitals
                            }

                            //End//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225

                            Clinical_MedicalHx.medicalDate = $('#' + Clinical_MedicalHx.params.PanelID + " #dtMedicalHxDate").val();

                            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.MedicalHx != null) {
                                $('#' + Clinical_MedicalHx.params.PanelID + " #dtMedicalHxDate").val(Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxDate);
                            }

                            if (Clinical_HistorySummary.HistoryCacheList.MedicalHx != null && Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxComments != '') {
                                $('#' + Clinical_MedicalHx.params.PanelID + " #txtMedicalHxComments").val(Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxComments);
                            }
                            else {
                                $('#' + Clinical_MedicalHx.params.PanelID + " #txtMedicalHxComments").val(Medicalhx_detail.MedicalHxComments);
                                Clinical_MedicalHx.overallComments = $('#' + Clinical_MedicalHx.params.PanelID + " #txtMedicalHxComments").val();
                            }

                            Clinical_MedicalHx.unremarkable = Medicalhx_detail.MedicalHxUnremarkable == "False" || Medicalhx_detail.MedicalHxUnremarkable == null ? false : true;;

                            if (Clinical_HistorySummary.HistoryCacheList.MedicalHx != null) {
                                $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #chkMedicalHxUnremarkable").prop("checked", Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxUnremarkable);
                            }

                            Clinical_MedicalHx.params.mode = "Edit";
                        });
                    }
                    Clinical_MedicalHx.params.mode = "Edit";
                    if ($('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #chkMedicalHxUnremarkable").prop("checked") == false) {
                        Medicalhx_detail.MedicalHxUnremarkable = 'false';
                    }
                    if (Medicalhx_detail.MedicalHxUnremarkable != null && typeof Medicalhx_detail.MedicalHxUnremarkable !== 'undefined') {
                        if (Medicalhx_detail.MedicalHxUnremarkable.toLowerCase() != "true") {
                            var self = null;
                            var MedicalHx_Child_Detail = null;
                            if (MedicalHxType.toLowerCase() == "disease") {
                                if (isDiseaseFill == true) {
                                    if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                        var cachedJSON = Clinical_MedicalHx.getCacheMedicalHxJSON(diseaseId);
                                        if (cachedJSON != '') {
                                            Clinical_MedicalHx.bindCurrentTabJSON(MedicalHxType.toLowerCase(), cachedJSON, "div#MedicalHxDisease", "#ulMedicalDisease");
                                        }
                                        else {
                                            Clinical_MedicalHx.bindCurrentTabJSON(MedicalHxType.toLowerCase(), response.DiseaseFill_JSON, "div#MedicalHxDisease", "#ulMedicalDisease");
                                        }
                                    }
                                    else {
                                        Clinical_MedicalHx.bindCurrentTabJSON(MedicalHxType.toLowerCase(), response.DiseaseFill_JSON, "div#MedicalHxDisease", "#ulMedicalDisease");
                                    }
                                }
                                    //Start/25-1-2016/Abid Ali/ Disable section detail on form load
                                else {
                                    $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #sectionDiseaseDetails").addClass('disableAll');
                                    if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.MedicalHx != null) {
                                        $.each(Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList, function (i, item) {
                                            Clinical_MedicalHx.bindCurrentTabJSON(MedicalHxType.toLowerCase(), item.JSON, "#frmClinicalMedicalHx", "#ulMedicalDisease")
                                        });
                                    }
                                }
                                //End/25-1-2016/Abid Ali/ Disable section detail on form load
                            }
                            else if (MedicalHxType.toLowerCase() == "miscellaneous") {

                            }
                            else {
                                self = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx");
                            }

                        }
                        else {
                            if (isDiseaseFill != true) {
                                var chkUnremarkable = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #chkMedicalHxUnremarkable")
                                Clinical_MedicalHx.unRemarkableMedicalHx(chkUnremarkable, "1");
                            }
                        }
                    }
                    //Start//16/12/2015//Ahmad Raza//Serializing the form
                    $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('serialize', $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').serialize());
                    //End//16/12/2015//Ahmad Raza//Serializing the form
                    //Start//26/01/2016//Abid Ali//enable to date when from date is filled
                    Clinical_MedicalHx.enableToDate();
                    //End//26/01/2016//Abid Ali//enable to date when from date is filled

                    //   Clinical_MedicalHx.medicalHxJSON = $('#' + Clinical_MedicalHx.params.PanelID + " #MedicalHxDisease").getMyJSON();

                    //if (diseaseSelected == true) {
                    //    Clinical_MedicalHx.cacheMedicalHxJSON();
                    //}
                    if (isDiseaseFill == true && Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        Clinical_MedicalHx.medicalHxJSON = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").getMyJSONByName();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                dfd.resolve();
            }
            else {

                /* Start 10/12/2015 Muhammad Irfan If MedicalHx not saved then trigger the first list item of status */

                if (MedicalHxType.toLowerCase() == "tobacco") {
                    $('#' + Clinical_MedicalHx.params.PanelID + ' #ulSmokingStatus li:first').trigger('click');
                }

                /* End 10/12/2015 Muhammad Irfan If MedicalHx not saved then trigger the first list item of status */

                $('#' + Clinical_MedicalHx.params.PanelID + " div#DrugAbuse #ddlDrugType,div#SexualHx #ddlSexualSTD").multiselect({

                });

                //$('#' + Clinical_MedicalHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect({

                //});
                dfd.resolve();
            }

            BackgroundLoaderShow(false);
        });
        return dfd;
    },

    cacheMedicalHxJSON: function (diseaseId, diseaseData, diseaseLi) {
        var dfd = $.Deferred();
        var diseaseIndex = -1;

        var isFavListOpened = $('#' + Clinical_MedicalHx.params.PanelID + " #favSectionDiv").hasClass("toggledHor");
        var FavListVal = $('#' + Clinical_MedicalHx.params.PanelID + ' #ddlFavoriteListMedicalHx').val();



        if (Clinical_HistorySummary.HistoryCacheList.MedicalHx == null) {

            var patientId;
            if (Clinical_MedicalHx.params.patientID == null) {
                patientId = $('#PatientProfile #hfPatientId').val();
            } else {
                patientId = Clinical_MedicalHx.params.patientID;
            }
            var noteId = Clinical_ProgressNote.params.NotesId;

            var medicalHxData = {
                MedicalHxId: $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxId").val(),
                MedicalHxType: 'Disease',
                MedicalHxDate: $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #dtMedicalHxDate").val(),
                MedicalHxUnremarkable: $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #chkMedicalHxUnremarkable").prop("checked"),
                MedicalHxComments: $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #txtMedicalHxComments").val(),
                PatientId: patientId,
                NotesId: noteId,
                MedicalDiseaseList: [],
                isFavListOpened: isFavListOpened,
                FavListVal: FavListVal,
                FavListName: Clinical_MedicalHx.FavListName
            }

            Clinical_HistorySummary.HistoryCacheList.MedicalHx = medicalHxData;
        }
        else {
            Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxDate = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #dtMedicalHxDate").val();
            Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxUnremarkable = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #chkMedicalHxUnremarkable").prop("checked");
            Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxComments = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #txtMedicalHxComments").val();
            Clinical_HistorySummary.HistoryCacheList.MedicalHx.isFavListOpened = isFavListOpened;
            Clinical_HistorySummary.HistoryCacheList.MedicalHx.FavListVal = FavListVal;
            Clinical_HistorySummary.HistoryCacheList.MedicalHx.FavListName = Clinical_MedicalHx.FavListName;
            Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalHxId = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxId").val();
        }

        if (diseaseId != null) {

            $.grep(Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList, function (item, index) {
                if (item.DiseaseId == diseaseId) {
                    diseaseIndex = index;
                    return;
                }
            });

            var diseaseObj = JSON.parse(diseaseData);

            if (diseaseIndex != -1) {
                var selectedDisease = $('#' + Clinical_MedicalHx.params.PanelID + " div#MedicalHxDisease ul#ulMedicalDisease li#" + diseaseId);
                $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfSelectedDisease").val(selectedDisease.attr("id"));

                var FreeTextAttr = selectedDisease.attr("FreeText");
                var FreeText = '';
                if (typeof FreeTextAttr !== typeof undefined) {
                    FreeText = selectedDisease.text();
                }

                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalHxId = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxId").val(),
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].DiseaseId = diseaseId,
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].Disease_text = selectedDisease.text(),
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].FreeTextICD = FreeText,
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].ICD9Code = selectedDisease.attr("icd9code"),
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].ICD9CodeDescription = FreeText == '' ? selectedDisease.attr("icd9desc") : FreeText,
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].ICD10Code = selectedDisease.attr("icd10code"),
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].ICD10CodeDescription = selectedDisease.attr("icd10desc"),
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].SNOMEDID = selectedDisease.attr("snomedcode"),
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].SNOMEDDescription = selectedDisease.attr("snomeddesc"),
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].CPTCode = $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode').val();
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].CPTCodeId = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val();
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].CPTDescription = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val();
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].CPTSNOMEDCodeId = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDCode').val();
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].CPTSNOMEDDescription = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDDescription').val();
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseAgggravatedBy = diseaseObj.MedicalDiseaseAgggravatedBy;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseAgggravatedBy_RefValue = diseaseObj.MedicalDiseaseAgggravatedBy_RefValue;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseAgggravatedBy_text = diseaseObj.MedicalDiseaseAgggravatedBy_text;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseComments = diseaseObj.MedicalDiseaseComments;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseDurationLength = diseaseObj.MedicalDiseaseDurationLength;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseDurationPeriod = diseaseObj.MedicalDiseaseDurationPeriod;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseDurationPeriod_RefValue = diseaseObj.MedicalDiseaseDurationPeriod_RefValue;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseDurationPeriod_text = diseaseObj.MedicalDiseaseDurationPeriod_text;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseFromDate = diseaseObj.MedicalDiseaseFromDate;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseLocation = diseaseObj.MedicalDiseaseLocation;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseOnset = diseaseObj.MedicalDiseaseOnset;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseasePattern = diseaseObj.MedicalDiseasePattern;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseasePattern_RefValue = diseaseObj.MedicalDiseasePattern_RefValue;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseasePattern_text = diseaseObj.MedicalDiseasePattern_text;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseSeverity = diseaseObj.MedicalDiseaseSeverity;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseSeverity_RefValue = diseaseObj.MedicalDiseaseSeverity_RefValue;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseSeverity_text = diseaseObj.MedicalDiseaseSeverity_text;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseStatus = diseaseObj.MedicalDiseaseStatus;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseStatus_RefValue = diseaseObj.MedicalDiseaseStatus_RefValue;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseStatus_text = diseaseObj.MedicalDiseaseStatus_text;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseTestResult = diseaseObj.MedicalDiseaseTestResult;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseTestResult_RefValue = diseaseObj.MedicalDiseaseTestResult_RefValue;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseTestResult_text = diseaseObj.MedicalDiseaseTestResult_text;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseToDate = diseaseObj.MedicalDiseaseToDate;
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseDurationPeriodText = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseDurationPeriod option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseDurationPeriod option:selected").text());
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseTestResultText = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseTestResult option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseTestResult option:selected").text());
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseStatusText = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseStatus option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseStatus option:selected").text());
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseSeverityText = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseSeverity option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseSeverity option:selected").text());
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseasePatternText = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseasePattern option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseasePattern option:selected").text());
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].MedicalDiseaseAgggravatedByText = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseAgggravatedBy option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseAgggravatedBy option:selected").text());
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList[diseaseIndex].JSON = diseaseData;
            }
            else {

                var selectedDisease = $('#' + Clinical_MedicalHx.params.PanelID + " div#MedicalHxDisease ul#ulMedicalDisease li#" + diseaseId);
                $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfSelectedDisease").val(selectedDisease.attr("id"));

                var FreeTextAttr = selectedDisease.attr("FreeText");
                var FreeText = '';
                if (typeof FreeTextAttr !== typeof undefined) {
                    FreeText = selectedDisease.text();
                }

                var medDiseaseData = {
                    DiseaseId: diseaseId,
                    DiseaseLi: diseaseLi,
                    Disease_text: selectedDisease.text(),
                    FreeTextICD: FreeText,
                    ICD9Code: selectedDisease.attr("icd9code"),
                    ICD9CodeDescription: FreeText == '' ? selectedDisease.attr("icd9desc") : FreeText,
                    ICD10Code: selectedDisease.attr("icd10code"),
                    ICD10CodeDescription: selectedDisease.attr("icd10desc"),
                    SNOMEDID: selectedDisease.attr("snomedcode"),
                    SNOMEDDescription: selectedDisease.attr("snomeddesc"),
                    MedicalHxId: $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxId").val(),
                    CPTCode: $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode').val(),
                    CPTCodeId: $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val(),
                    CPTDescription: $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val(),
                    CPTSNOMEDCodeId: $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDCode').val(),
                    CPTSNOMEDDescription: $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDDescription').val(),
                    MedicalDiseaseAgggravatedBy: diseaseObj.MedicalDiseaseAgggravatedBy,
                    MedicalDiseaseAgggravatedBy_RefValue: diseaseObj.MedicalDiseaseAgggravatedBy_RefValue,
                    MedicalDiseaseAgggravatedBy_text: diseaseObj.MedicalDiseaseAgggravatedBy_text,
                    MedicalDiseaseComments: diseaseObj.MedicalDiseaseComments,
                    MedicalDiseaseDurationLength: diseaseObj.MedicalDiseaseDurationLength,
                    MedicalDiseaseDurationPeriod: diseaseObj.MedicalDiseaseDurationPeriod,
                    MedicalDiseaseDurationPeriod_RefValue: diseaseObj.MedicalDiseaseDurationPeriod_RefValue,
                    MedicalDiseaseDurationPeriod_text: diseaseObj.MedicalDiseaseDurationPeriod_text,
                    MedicalDiseaseFromDate: diseaseObj.MedicalDiseaseFromDate,
                    MedicalDiseaseLocation: diseaseObj.MedicalDiseaseLocation,
                    MedicalDiseaseOnset: diseaseObj.MedicalDiseaseOnset,
                    MedicalDiseasePattern: diseaseObj.MedicalDiseasePattern,
                    MedicalDiseasePattern_RefValue: diseaseObj.MedicalDiseasePattern_RefValue,
                    MedicalDiseasePattern_text: diseaseObj.MedicalDiseasePattern_text,
                    MedicalDiseaseSeverity: diseaseObj.MedicalDiseaseSeverity,
                    MedicalDiseaseSeverity_RefValue: diseaseObj.MedicalDiseaseSeverity_RefValue,
                    MedicalDiseaseSeverity_text: diseaseObj.MedicalDiseaseSeverity_text,
                    MedicalDiseaseStatus: diseaseObj.MedicalDiseaseStatus,
                    MedicalDiseaseStatus_RefValue: diseaseObj.MedicalDiseaseStatus_RefValue,
                    MedicalDiseaseStatus_text: diseaseObj.MedicalDiseaseStatus_text,
                    MedicalDiseaseTestResult: diseaseObj.MedicalDiseaseTestResult,
                    MedicalDiseaseTestResult_RefValue: diseaseObj.MedicalDiseaseTestResult_RefValue,
                    MedicalDiseaseTestResult_text: diseaseObj.MedicalDiseaseTestResult_text,
                    MedicalDiseaseToDate: diseaseObj.MedicalDiseaseToDate,
                    MedicalDiseaseDurationPeriodText: ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseDurationPeriod option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseDurationPeriod option:selected").text()),
                    MedicalDiseaseTestResultText: ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseTestResult option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseTestResult option:selected").text()),
                    MedicalDiseaseStatusText: ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseStatus option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseStatus option:selected").text()),
                    MedicalDiseaseSeverityText: ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseSeverity option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseSeverity option:selected").text()),
                    MedicalDiseasePatternText: ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseasePattern option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseasePattern option:selected").text()),
                    MedicalDiseaseAgggravatedByText: ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseAgggravatedBy option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseAgggravatedBy option:selected").text()),
                    JSON: diseaseData
                };

                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList.push(medDiseaseData);
            }
        }

        dfd.resolve();
        return dfd;
    },

    getCacheMedicalHxJSON: function (diseaseId) {
        if (Clinical_HistorySummary.HistoryCacheList.MedicalHx != null && Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList.length > 0) {
            var medicalDisease = $.grep(Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList, function (item, index) {
                if (item.DiseaseId == diseaseId) {
                    return item;
                }
            });

            if (medicalDisease.length > 0) {
                return medicalDisease[0].JSON;
            }
        }

        return '';
    },
    ChangeCurrentPast: function (obj, PrimaryID, PageNumber, ResultPerPage) {

        if ($(obj).attr('status') == '1' || obj == 1) {
            $(obj).attr('status', 0);
            $('#' + Clinical_MedicalHx.params.PanelID + " #pnlCurrent ").addClass("hidden");
            $('#' + Clinical_MedicalHx.params.PanelID + " #pnlPast ").removeClass("hidden");
            Clinical_MedicalHx.fillhxLog(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                if (response != "") {
                    var json = JSON.parse(response);
                    Clinical_MedicalHx.gridLoad(response);
                    var TableControl = Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvPastMedicalHx";
                    var PagingPanelControlID = Clinical_MedicalHx.params.PanelID + " #dgvPastMedicalHx_Paging";
                    var ClassControlName = "Clinical_MedicalHx";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = json.iTotalDisplayRecords;
                    setTimeout(
                        CreatePagination(json.HxLogSoapCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_MedicalHx.ChangeCurrentPast(1, PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                }
            });


        } else {
            $(obj).attr('status', 1);

            $('#' + Clinical_MedicalHx.params.PanelID + " #pnlPast").addClass("hidden");
            $('#' + Clinical_MedicalHx.params.PanelID + " #pnlCurrent  ").removeClass("hidden");
        }

    },
    fillhxLog: function (PrimaryID, PageNumber, ResultPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (ResultPerPage == null) {
            ResultPerPage = 15;
        }
        var objData = {};
        objData["HxId"] = Clinical_MedicalHx.params.HxTypeId;
        objData["HxType"] = "MedicalHx";
        objData["Status"] = "All";
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = ResultPerPage;
        objData["commandType"] = "get_hx_log";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");
    },
    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle fill of MedicalHx and it's childs as specified by MedicalHxType

    loadMedicalHx: function (MedicalHxType, isLoadNew) {
        var dfd = $.Deferred();
        if (isLoadNew == true) {
            $.when(Clinical_MedicalHx.loadMedicalHxComponent(MedicalHxType, undefined, undefined, true)).then(function () {
                dfd.resolve();
            });
            $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val(MedicalHxType);
        } else if (EMRUtility.compareFormDataWithSerialized(Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx')) {

            utility.myConfirm('12', function () {
                var MedicalHxTypeCurrent = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val();
                Clinical_MedicalHx.MedicalHxSave(MedicalHxTypeCurrent, false);

                $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val(MedicalHxType);
                Clinical_MedicalHx.loadMedicalHxComponent(MedicalHxType);
                BackgroundLoaderShow(true);
                dfd.resolve();
            }, function () {


                Clinical_MedicalHx.loadMedicalHxComponent(MedicalHxType);
                $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val(MedicalHxType);
                dfd.resolve();
            });

        } else {
            Clinical_MedicalHx.loadMedicalHxComponent(MedicalHxType);
            $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val(MedicalHxType);
            dfd.resolve();
        }
        //End//22/12/2015//Ahmad Raza//Implemented prompt problem if user changed but didn't saved data and want to go to another tab
        return dfd;
    },

    loadMedicalHxTabnUnserializeForm: function () {
        var dfd = $.Deferred();
        $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx  #sectionDiseaseDetails').data('serialize', null);
        $.when(Clinical_MedicalHx.loadMedicalHx('disease', true)).then(function () {
            dfd.resolve();
        });
        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val('disease');
        return dfd;
    },


    //Author: Muhammad Arshad
    //Date: 12-07-2015
    //This function will handle fill of MedicalHx's childs Tabs as specified by TabType

    bindCurrentTabJSON: function (TabType, currentTabJSON, CurrentTabContainerId, ulTabStatusId) {
        var alcoholhx_detail = JSON.parse(currentTabJSON);
        self = $('#' + Clinical_MedicalHx.params.PanelID + " " + CurrentTabContainerId);
        //$.each(alcoholhx_detail, function (i, item) {

        utility.bindMyJSONByName(true, alcoholhx_detail, false, self).done(function () {
            if (TabType != null && TabType.toLowerCase() == "tobacco") {
                //if (item.TobaccoRecentlyQuit.toLowerCase() == "true") {
                //    // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                //    $('#' + Clinical_MedicalHx.params.PanelID + " section#sectionTobacco #chkTobaccoRecentlyQuit").prop("checked", false);
                //    $('#' + Clinical_MedicalHx.params.PanelID + " section#sectionTobacco #chkTobaccoRecentlyQuit").trigger("click");

                //}
            }
            //Start//14/01/2016//Ahmad Raza//setting values in hidden field when existing disease is selected
            $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfCPTCode").val(alcoholhx_detail.CPTCodeId);
            $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfCPTDescription").val(alcoholhx_detail.CPTDescription);
            //End//14/01/2016//Ahmad Raza//setting values in hidden field when existing disease is selected

            $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDCode').val(alcoholhx_detail.CPTSNOMEDCodeId);
            $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDDescription').val(alcoholhx_detail.CPTSNOMEDDescription);

            BackgroundLoaderShow(false);
        });
        // });
    },

    //Author: Abid Ali
    //Date: 26-01-2016
    //This function will handle pop search on procedure field
    OpenSearchPopup: function (SearchType, Ctrl, HiddenCtrl) {
        var controlToLoad = "";
        if (SearchType == "ICD") {

            controlToLoad = "Admin_IMOICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (SearchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx  #txtDisease").attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        //params["ParentCtrl"] = Clinical_MedicalHx.params["TabID"];
        if (Clinical_MedicalHx.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_MedicalHx';
        }

        else {
            params["ParentCtrl"] = Clinical_MedicalHx.params["TabID"];
        }





        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Clinical_MedicalHx.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle unremarkable feature for Medical Hx
    unRemarkableMedicalHx: function (obj, isFromLoad) {
        var isRemarkable = $(obj).prop("checked");
        if (isRemarkable == true) {
            $('#' + Clinical_MedicalHx.params.PanelID + ' #sectionDiseaseDetails').resetAllControls(null);
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #btnMedicalHxSave').hide();
            }
            else {
                $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #btnMedicalHxSave').show();
            }
            $('#' + Clinical_MedicalHx.params.PanelID + " div#MedicalHxDisease ul#ulMedicalDisease").empty();
            $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #MedicalHxDisease").addClass('disableAll');

            Clinical_MedicalHx.enableDisbaleUlItems('ulFavoriteListMedicalHxContent', false);

            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.MedicalHx != null) {
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList = [];
            }
        }
        else {
            $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #MedicalHxDisease").removeClass('disableAll');
            //Start/25-1-2016/Abid Ali/ Disable section detail on form load
            if (Clinical_MedicalHx.checkActiveDisease() != true)
                $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #sectionDiseaseDetails").addClass('disableAll');
            //End/25-1-2016/Abid Ali/ Disable section detail on form load
            $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #btnMedicalHxSave').hide();
            Clinical_MedicalHx.enableDisbaleUlItems('ulFavoriteListMedicalHxContent', true);
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will clear value of given control as specified by obj

    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Medical History Clinical Module -> Fields should be blank when select other status
                var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                if (groupRadBtn.length > 1) {
                    $.each(groupRadBtn, function (i, item) {
                        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                            $(item).trigger("click");
                        }
                    });
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Medical History Clinical Module -> Fields should be blank when select other status
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
    //Date: 12-28-2015
    //This function will check if details has any value for selected status
    //Begin 12-28-2015 Muhammad Arshad Bug# EMR-159 Medical History Clinical Module -> Save
    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";
        if (TabType != null && TabType != "") {
            if (TabType.toLowerCase() == "disease") {
                sectionDetails = "sectionDiseaseDetails";
            }
        }
        if (sectionDetails != "") {
            var self = $('#' + Clinical_MedicalHx.params.PanelID + ' section#' + sectionDetails).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if ($(this).prop("disabled") != true) {
                    var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                    if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                        DetailExists = true;
                    }
                    if ($(this).attr('type') == 'checkbox' && this.checked == true) {
                        DetailExists = true;
                    }
                    if ($(this).attr('type') == 'radio' && $(this).attr('id').toLowerCase().indexOf("yes") > -1 && this.checked == true) {
                        DetailExists = true;
                    }
                    if (currentElementTagName.toLowerCase() == 'select' && $(this).val() != null && $(this).val() != "") {
                        DetailExists = true;
                    }
                    //if (currentElementTagName.toLowerCase() == 'ul') {
                    //    $(this).find('li.active').removeClass('active');
                    //}
                }
            });
        }

        return DetailExists;

    },

    //End 12-28-2015 Muhammad Arshad Bug# EMR-159 Medical History Clinical Module -> Save


    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle Add/Edit of MedicalHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous), it expects MedicalHxType to be Add/Edit
    MedicalHxSave: function (MedicalHxType, UnloadMedicalhx, attachToNote) {

        //Start//17-02-2016//Ahmad Raza//Fixed Bug#339
        if (!$('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #chkMedicalHxUnremarkable").is(':checked')) {
            //  $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx").bootstrapValidator('revalidateField', 'CPTCode');
        }
        // if ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #txtCPTCode").val() != "" || $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #chkMedicalHxUnremarkable").is(':checked')) {
        //End//17-02-2016//Ahmad Raza//Fixed Bug#339
        var MedicalHxId = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxId").val() != "" ? $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxId").val() : "-1";
        if (parseInt(MedicalHxId) > 0) {
            Clinical_MedicalHx.params.mode = "Edit";
        }
        else {
            Clinical_MedicalHx.params.mode = "Add";
        }

        //Start//11/02/2016//Abid Ali// fixed bug#315
        //Start//26/12/2016//Zain ul abdin// IMP-112
        //var overallComments = "";
        //overallComments = $('#' + Clinical_MedicalHx.params.PanelID + " #txtMedicalHxComments").val();
        //overallComments = typeof overallComments == "undefined" ? "" : overallComments
        //if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote" && overallComments != "") {

        //    var DetailExists = true;
        //}
        //else {
        //    DetailExists = Clinical_MedicalHx.isDetailExists(MedicalHxType.toLowerCase());
        //}
        //if ($('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #chkMedicalHxUnremarkable").is(':checked')) {
        //    DetailExists = true;
        //}
        //if (overallComments != "") {
        //    DetailExists = true;
        //}
        //End//26/12/2016//Zain ul abdin// IMP-112
        //End//11/02/2016//Abid Ali// fixed bug#315

        var DetailExists = true;
        if (DetailExists == true) {
            var strMessage = "";
            var self = null;
            if (MedicalHxType.toLowerCase() == "disease") {
                self = $('#' + Clinical_MedicalHx.params.PanelID + " section#sectionDiseaseDetails");
            }
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);
            if (MedicalHxType.toLowerCase() == "disease") {

                var selectedDisease = $('#' + Clinical_MedicalHx.params.PanelID + " div#MedicalHxDisease ul#ulMedicalDisease li.active");
                $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfSelectedDisease").val(selectedDisease.attr("id"));
                objData["DiseaseId"] = selectedDisease.attr("id");
                objData["Disease_text"] = selectedDisease.text().indexOf('-') > -1 ? selectedDisease.text().substring(selectedDisease.text().indexOf('-') + 2, selectedDisease.text().lenght) : selectedDisease.text();

                //-------------------
                var FreeTextAttr = selectedDisease.attr("FreeText");
                if (typeof FreeTextAttr !== typeof undefined) {
                    objData["FreeTextICD"] = selectedDisease.text().indexOf('-') > -1 ? selectedDisease.text().substring(selectedDisease.text().indexOf('-') + 2, selectedDisease.text().lenght) : selectedDisease.text();
                }
                else {
                    objData["ICD9Code"] = selectedDisease.attr("icd9code");
                    objData["ICD9CodeDescription"] = selectedDisease.attr("icd9desc");
                    objData["ICD10Code"] = selectedDisease.attr("icd10code");
                    objData["ICD10CodeDescription"] = selectedDisease.attr("icd10desc");
                    objData["SNOMEDID"] = selectedDisease.attr("snomedcode");
                    objData["SNOMEDDescription"] = selectedDisease.attr("snomeddesc");
                }


                //-------------------
            }

            objData["MedicalHxId"] = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxId").val();
            objData["MedicalHxType"] = MedicalHxType != null ? MedicalHxType : "";
            objData["MedicalHxDate"] = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #dtMedicalHxDate").val();
            objData["MedicalHxUnremarkable"] = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #chkMedicalHxUnremarkable").prop("checked");
            objData["MedicalHxComments"] = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #txtMedicalHxComments").val();
            myJSON = JSON.stringify(objData);
            //return false;
            if (Clinical_MedicalHx.params.mode == "Add") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Medical Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var result;
                        result = Clinical_MedicalHx.validateProcedure();
                        if (result != false) {
                            Clinical_MedicalHx.saveMedicalHx(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_MedicalHx.SaveFavToggelStatus($('#' + Clinical_MedicalHx.params.PanelID + ' #ddlFavoriteListMedicalHx').val());
                                    Clinical_MedicalHx.params.HxTypeId = response.MedicalHxId;
                                    $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #divSwitch").removeClass('disableAll');
                                    Clinical_MedicalHx.ChangeCurrentPast(1, null, null, null);
                                    if (attachToNote != false) {
                                        Clinical_MedicalHx.BindCurrentMedicationSoapText(response, true);
                                    }
                                    if (response.diseaseId != "") {
                                        var diseaseResponse = JSON.parse(response.diseaseId);

                                        if (diseaseResponse.diseaseId > 0) {
                                            $('#' + Clinical_MedicalHx.params.PanelID + " div#MedicalHxDisease ul#ulMedicalDisease li.active").attr('id', diseaseResponse.diseaseId)
                                            Clinical_MedicalHx.currentSelectedDisease["DiseaseId"] = diseaseResponse.diseaseId;
                                        }
                                    }
                                    //Clinical_MedicalHx.loadMedicalHxComponent(MedicalHxType);
                                    $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxId").val(response.MedicalHxId);
                                    Clinical_MedicalHx.params.mode = "Edit";
                                    if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote" && UnloadMedicalhx == true && attachToNote != false) {
                                        Clinical_MedicalHx.getMedicalHxInfo(MedicalHxType, UnloadMedicalhx, null, true);
                                    }
                                    utility.DisplayMessages(response.message, 1);
                                    //Start//17/12/2015//Ahmad Raza//Serializing the form
                                    $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('serialize', $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').serialize());
                                    //End//17/12/2015//Ahmad Raza//Serializing the form
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
                //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
            }
            else if (Clinical_MedicalHx.params.mode == "Edit") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Medical Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var result;
                        result = Clinical_MedicalHx.validateProcedure();
                        if (result != false) {
                            Clinical_MedicalHx.updateMedicalHx(myJSON, Clinical_MedicalHx.params.MedicalHxId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_MedicalHx.SaveFavToggelStatus($('#' + Clinical_MedicalHx.params.PanelID + ' #ddlFavoriteListMedicalHx').val());
                                    $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #divSwitch").removeClass('disableAll');
                                    Clinical_MedicalHx.ChangeCurrentPast(1, null, null, null);
                                    if (attachToNote != false) {
                                        Clinical_MedicalHx.BindCurrentMedicationSoapText(response, true);
                                    }
                                    if (response.diseaseId != "") {
                                        var diseaseResponse = JSON.parse(response.diseaseId);
                                        if (diseaseResponse.diseaseId > 0) {
                                            $('#' + Clinical_MedicalHx.params.PanelID + " div#MedicalHxDisease ul#ulMedicalDisease li.active").attr('id', diseaseResponse.diseaseId)
                                            Clinical_MedicalHx.currentSelectedDisease["DiseaseId"] = diseaseResponse.diseaseId;
                                        }
                                    }
                                    //Clinical_MedicalHx.loadMedicalHxComponent(MedicalHxType);
                                    if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote" && UnloadMedicalhx == true && attachToNote != false) {
                                        Clinical_MedicalHx.getMedicalHxInfo(MedicalHxType, UnloadMedicalhx, null, true);
                                    }
                                    //Clinical_MedicalHx.AppointmentStatusSearch(Clinical_MedicalHx.params.MedicalHxignsId);
                                    utility.DisplayMessages(response.message, 1);
                                    //Start//17/12/2015//Ahmad Raza//Serializing the form
                                    $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').data('serialize', $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx').serialize());
                                    //End//17/12/2015//Ahmad Raza//Serializing the form
                                }
                                else {
                                    utility.DisplayMessages(response.message, 3);
                                }
                            });
                        }
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
                //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
            }
        }
        else {
            //Begin 12-28-2015 Muhammad Arshad Bug# EMR-159 Medical History Clinical Module -> Save
            utility.DisplayMessages("Please enter any value", 3);
            //End 12-28-2015 Muhammad Arshad Bug# EMR-159 Medical History Clinical Module -> Save
        }
        //  }
    },


    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + Clinical_MedicalHx.params.PanelID + " #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Clinical_MedicalHx.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Clinical_MedicalHx.FavListName, FavListVal);
        });
    },

    BindCurrentMedicationSoapText: function (resopnse, isDiseaseExists) {

        var json = null;
        var MedicalHxSoapText = null;
        if (resopnse.MedicalHxId > 0) {
            json = resopnse;
            MedicalHxSoapText = resopnse.SoapText;
        } else {
            json = JSON.parse(resopnse.MedicalHxFill_JSON);
            MedicalHxSoapText = json.MedicalHxSoapText;
        }

        var finalSoapText = '<p>';
        if (MedicalHxSoapText != "") {
            var soapText = document.createElement('div');
            $(soapText).html(MedicalHxSoapText);

            $(soapText).find('div').each(function (index, item) {
                var soap = $(this).html();
                var nextDisease = $($(soapText).find('div')[index + 1]).html();

                if (soap != "") {
                    if (nextDisease != null) {
                        if (soap.indexOf('The patient underwent') == -1 && soap.indexOf('The patient also underwent') == -1 && soap.indexOf('Status') == -1 && soap.indexOf('Test Result') == -1 && soap.indexOf('Onset') == -1 && soap.indexOf('Duration') == -1 && soap.indexOf('Severity') == -1
                             && soap.indexOf('Pattern') == -1 && soap.indexOf('Aggravated by') == -1 && soap.indexOf('Location') == -1 &&
                            nextDisease.indexOf('The patient underwent') == -1 && nextDisease.indexOf('The patient also underwent') == -1 && nextDisease.indexOf('Status') == -1 && nextDisease.indexOf('Test Result') == -1 && nextDisease.indexOf('Onset') == -1 && nextDisease.indexOf('Duration') == -1 && nextDisease.indexOf('Severity') == -1
                             && nextDisease.indexOf('Pattern') == -1 && nextDisease.indexOf('Aggravated by') == -1 && nextDisease.indexOf('Location') == -1) {

                            finalSoapText += soap[soap.length - 1] == ":" || soap[soap.length - 1] == "." ? soap.replace(soap[soap.length - 1], ', ') : soap + ", ";
                        }
                        else {
                            finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                        }
                    }
                    else {
                        finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                    }
                }
            });
            $(soapText).find('div').remove();

            if (finalSoapText == '<p>') {
                finalSoapText += $(soapText).html() + "</p>"
            }
            else {
                finalSoapText += '<br>' + $(soapText).html() + "</p>"
            }
        }
        var $row = $('<tr/>');
        if (isDiseaseExists) {
            $row.append('<td style="display:none;">' + json.MedicalHxId + '</td><td>' + json.IsCreatedOrModified + '</td><td>' + finalSoapText + '</td><td>' + json.LastUpdated + '</td>');
        }
        else {
            $row.append('<td>&nbsp;</td><td>No Known Medical History</td><td></td>');
        }

        $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvMedicalHx tbody").html($row);
        if ($('#' + Clinical_MedicalHx.params.PanelID + ' #pnlMedicalHx_Result #divSwitch #switchVisit').attr('status') == '1') {
            $('#' + Clinical_MedicalHx.params.PanelID + ' #pnlCurrent').removeClass('hidden');
            $('#' + Clinical_MedicalHx.params.PanelID + ' #pnlPast').addClass('hidden');

        }



    },

    //Start//12/01/2016//Ahmad Raza//Logic to compare txtCPT values with IMO
    validateProcedure: function () {
        var cptcode = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val();
        var cptdescp = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val();
        cptcode = (cptcode != null && cptcode != "") ? cptcode + " - " : "";
        if ($('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode').val() == "") {
            $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val('');
            $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val('');
            return true;
        }
        else if (cptcode + cptdescp != $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode').val().trim()) {
            utility.DisplayMessages("Procedure not Valid", 2);
            $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode').val('');
            return false;
        }
        else
            return true;

        //var cptDescription = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val();
        //var cptCode = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val();
        //var $txtCptCode = $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode');
        //if ($txtCptCode.val() == "") {
        //    return true;
        //}
        ////Start//2/02/2016//Abid Ali//added Logic to compare txtCPT values with IMO
        //if (cptCode == "") {
        //    if (cptDescription != $txtCptCode.val()) {
        //        utility.DisplayMessages("Procedure not Valid", 2);
        //        $txtCptCode.val('');
        //        return false;
        //    }
        //}
        //    //End//2/02/2016//Abid Ali//added Logic to compare txtCPT values with IMO
        //else if (cptCode + " - " + cptDescription != $txtCptCode.val()) {
        //    utility.DisplayMessages("Procedure not Valid", 2);
        //    $txtCptCode.val('');
        //    return false;
        //}
        //else
        //    return true;

        var cptDescription = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val();
        var cptCode = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val();
        var $txtCptCode = $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode');
        if ($txtCptCode.val() == "") {
            return true;
        }
        //Start//2/02/2016//Abid Ali//added Logic to compare txtCPT values with IMO
        if (cptCode == "") {
            if (cptDescription != $txtCptCode.val()) {
                utility.DisplayMessages("Procedure not Valid", 2);
                $txtCptCode.val('');
                return false;
            }
        }
            //End//2/02/2016//Abid Ali//added Logic to compare txtCPT values with IMO
        else if (cptCode + " - " + cptDescription != $txtCptCode.val()) {
            utility.DisplayMessages("Procedure not Valid", 2);
            $txtCptCode.val('');
            return false;
        }
        else
            return true;

    },
    //End//12/01/2016//Ahmad Raza//Logic to compare txtCPT values with IMO

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle load of MedicalHx and it's childs as specified by MedicalHxType
    //It represents service call to API
    fillMedicalHx: function (MedicalHxType, DiseaseId, MedicalHxId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["MedicalHxType"] = MedicalHxType != null ? MedicalHxType : "disease";
        objData["MedicalHxId"] = MedicalHxId;
        objData["commandType"] = "FILL_MedicalHx";
        objData["DiseaseId"] = DiseaseId != null ? DiseaseId : 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "MedicalHx");
    },

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle Add of MedicalHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous)
    //It represents service call to API
    saveMedicalHx: function (MedicalHxData) {
        var objData = JSON.parse(MedicalHxData);
        if (Clinical_MedicalHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_MedicalHx.params.patientID;
        }
        //Start//14/01/2016//Ahmad Raza//sending text values of drop downs in model object
        objData["MedicalDiseaseDurationPeriodText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseDurationPeriod option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseDurationPeriod option:selected").text());
        objData["MedicalDiseaseTestResultText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseTestResult option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseTestResult option:selected").text());
        objData["MedicalDiseaseStatusText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseStatus option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseStatus option:selected").text());
        objData["MedicalDiseaseSeverityText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseSeverity option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseSeverity option:selected").text());
        objData["MedicalDiseasePatternText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseasePattern option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseasePattern option:selected").text());
        objData["MedicalDiseaseAgggravatedByText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseAgggravatedBy option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseAgggravatedBy option:selected").text());

        objData["CPTCode"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode').val();
        objData["CPTCodeId"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val();
        objData["CPTDescription"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val();
        objData["CPTSNOMEDCodeId"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDCode').val();
        objData["CPTSNOMEDDescription"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDDescription').val();
        objData["FromClinicalSide"] = "1";
        //End//14/01/2016//Ahmad Raza//sending text values of drop downs in model object
        objData["commandType"] = "SAVE_MedicalHx";
        var data = JSON.stringify(objData);

        //var data = "MedicalHxignsData=" + MedicalHxignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "MedicalHx");
    },

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle Edit of MedicalHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous)
    //It represents service call to API
    updateMedicalHx: function (MedicalHxData, MedicalHxId) {

        var objData = JSON.parse(MedicalHxData);
        if (Clinical_MedicalHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_MedicalHx.params.patientID;
        }
        //Start//14/01/2016//Ahmad Raza//sending text values of drop downs in model object
        objData["MedicalDiseaseDurationPeriodText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseDurationPeriod option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseDurationPeriod option:selected").text());
        objData["MedicalDiseaseTestResultText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseTestResult option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseTestResult option:selected").text());
        objData["MedicalDiseaseStatusText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseStatus option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseStatus option:selected").text());
        objData["MedicalDiseaseSeverityText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseSeverity option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseSeverity option:selected").text());
        objData["MedicalDiseasePatternText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseasePattern option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseasePattern option:selected").text());
        objData["MedicalDiseaseAgggravatedByText"] = ($("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseAgggravatedBy option:selected").text() == "- Select -" ? "" : $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ddlMedicalDiseaseAgggravatedBy option:selected").text());
        objData["CPTCode"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode').val();
        objData["CPTCodeId"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val();
        objData["CPTDescription"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val();
        objData["CPTSNOMEDCodeId"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDCode').val();
        objData["CPTSNOMEDDescription"] = $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDDescription').val();
        //End//14/01/2016//Ahmad Raza//sending text values of drop downs in model object
        objData["commandType"] = "UPDATE_MedicalHx";
        objData["FromClinicalSide"] = "1";

        var data = JSON.stringify(objData);

        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "MedicalHx");

    },

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle Unload of MedicalHx Tab

    unLoadTab: function (NextOrPre, controlToInvoke) {

        Clinical_MedicalHx.bIsFirstLoad = true;
        //Start//15-02-2016//Ahmad Raza//fixed Bug#332
        var MedicalHxType = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val();
        var detailExists = Clinical_MedicalHx.isDetailExists(MedicalHxType.toLowerCase());

        if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
            Clinical_MedicalHx.controlToInvoke = controlToInvoke;
            var diseaseAdded = $('#' + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease > li").filter(function () {
                return $(this).attr("id") < 0;
            });

            if (EMRUtility.compareFormDataWithSerialized(Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx') || diseaseAdded.length > 0) {
                utility.myConfirmNote('1', function () {
                    //var MedicalHxType = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val();
                    if (MedicalHxType == "" || MedicalHxType == "undefined") {
                        Clinical_MedicalHx.UnloadMedicalHistory(NextOrPre);
                        if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalMedicalHx") {
                            UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                            Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabMedicalHx', 'MedicalHx');
                        }
                    }
                    else {
                        Clinical_MedicalHx.bNextPrev = true;
                        Clinical_MedicalHx.addMedicalHxToNotes(MedicalHxType, true);
                    }
                }, function () {
                    //var MedicalHxType = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val();
                    if (MedicalHxType == "" || MedicalHxType == "undefined") {
                        Clinical_MedicalHx.UnloadMedicalHistory(NextOrPre);
                        if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalMedicalHx") {
                            UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                            Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabMedicalHx', 'MedicalHx');
                        }
                    }
                    else {
                        Clinical_MedicalHx.bNextPrev = true;
                        Clinical_MedicalHx.MedicalHxSave(MedicalHxType, true, false);
                        Clinical_MedicalHx.UnloadMedicalHistory(NextOrPre);
                    }
                }, function () {
                    Clinical_MedicalHx.UnloadMedicalHistory(NextOrPre);
                    if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalMedicalHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabMedicalHx', 'MedicalHx');
                    }
                },
               '1'
               );
            }
            else {
                Clinical_MedicalHx.UnloadMedicalHistory();
                if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalMedicalHx") {
                    UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                    Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabMedicalHx', 'MedicalHx');
                }
            }
            //End//15-02-2016//Ahmad Raza//fixed Bug#332
        } else {
            Clinical_MedicalHx.UnloadMedicalHistory();
        }

    },

    //Author: Muhammad Arshad
    //Date: 01-04-2016
    //This function will handle Unload of MedicalHistory
    UnloadMedicalHistory: function (NextOrPre) {
        Clinical_MedicalHx.bIsFirstLoad = true;
        if (Clinical_MedicalHx.params["FromAdmin"] == "0") {
            if (Clinical_MedicalHx.params != null && Clinical_MedicalHx.params.ParentCtrl != null) {
                if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote" && NextOrPre == true) {

                    UnloadActionPan(Clinical_MedicalHx.params.ParentCtrl, 'Clinical_MedicalHx');
                    if (Clinical_MedicalHx.controlToInvoke != null) {
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_MedicalHx.controlToInvoke);
                            Clinical_MedicalHx.controlToInvoke = null;
                        }, 400);
                    }
                }
                else {
                    UnloadActionPan(Clinical_MedicalHx.params.ParentCtrl, 'Clinical_MedicalHx');
                    if (Clinical_MedicalHx.controlToInvoke != null)
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_MedicalHx.controlToInvoke);
                            Clinical_MedicalHx.controlToInvoke = null;
                        }, 400);
                }
            }
            else
                UnloadActionPan(null, 'Clinical_MedicalHx');
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_MedicalHx").remove();
            RemoveAdminTab();
        }
        EMRUtility.scrollToPNcomponent('clinical_medicalhx');
    },

    /*
  Author: Abid Ali
  Date: 26/01/2016
  This function will enable to date when fromDate has saome data
  */
    enableToDate: function () {
        var fromDate = $('#' + Clinical_MedicalHx.params.PanelID + " #dtMedicalDiseaseFromDate").val();
        if (fromDate != "" && typeof fromDate != "undefined") {
            $('#' + Clinical_MedicalHx.params.PanelID + " #dtMedicalDiseaseToDate").prop("disabled", false);
        }
            //Start 2/02/2016 Abid Ali , for bug# 242
        else {
            $('#' + Clinical_MedicalHx.params.PanelID + " #dtMedicalDiseaseToDate").prop("disabled", true);
        }
        //Start 2/02/2016 Abid Ali , for bug# 242

    },

    //-----------------Progress Note-------------

    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Call Back function to add component to Progress Note
    addMedicalHxToNotes: function () {
        var MedicalHxId = Clinical_MedicalHx.params.MedicalHxId;
        var MedicalHxType = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxType").val();
        Clinical_MedicalHx.MedicalHxSave(MedicalHxType, true);
    },

    //this function will get Medical History Soap Text and attach that to Progress note
    getMedicalHxInfo: function (MedicalHxType, UnloadMedicalhx, MedicalHxId, hideAlertMessage) {
        Clinical_MedicalHx.fillMedicalHx(MedicalHxType, null, MedicalHxId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_MedicalHx.createMedicalHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadMedicalhx, hideAlertMessage);

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //This Function will check, if Medical History Soap is already attached in Progress note, if Medical History is not attached than it will create main divs to attach allergy
    checkMedicalHxExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Medicalhx').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="MedicalHxComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_Medicalhx title="Medical Hx"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'MedicalHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="MedicalHx">Medical Hx</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'MedicalHx\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'MedicalHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_Medicalhx> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Medicalhx').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    createMedicalHxBodyHTMLFromNotes: function (MedicalHistory, NoteHTMLCtrl, UnloadMedicalhx, hideAlertMessage) {

        Clinical_MedicalHx.checkMedicalHxExists();

        if (MedicalHistory && MedicalHistory.MedicalHxId && MedicalHistory.MedicalHxId > 0) {

            var MedicalHxFill_Obj = MedicalHistory;
            var $mainDivMedicalHx = $(document.createElement('div'));

            var MedicalHxId = MedicalHxFill_Obj.MedicalHxId;

            if (MedicalHxId > 0) {

                var $SectionBodyMedicalHx = $(document.createElement('section'));
                $SectionBodyMedicalHx.attr('id', "Cli_MedicalHx_Main" + MedicalHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_MedicalHx_" + MedicalHxId);
                var $ListMedicalHx = $(document.createElement('ul'));

                $ListMedicalHx.attr('class', 'list-unstyled')

                $SectionBodyMedicalHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_MedicalHx_" + MedicalHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_MedicalHx_Main" + MedicalHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                var finalSoapText = '<p>';
                if (MedicalHxFill_Obj.SoapText != "") {
                    var soapText = document.createElement('div');
                    $(soapText).html(MedicalHxFill_Obj.SoapText);

                    $(soapText).find('div').each(function (index, item) {
                        var soap = $(this).html();
                        var nextDisease = $($(soapText).find('div')[index + 1]).html();

                        if (soap != "") {
                            if (nextDisease != null) {
                                if (soap.indexOf('The patient underwent') == -1 && soap.indexOf('The patient also underwent') == -1 && soap.indexOf('Status') == -1 && soap.indexOf('Test Result') == -1 && soap.indexOf('Onset') == -1 && soap.indexOf('Duration') == -1 && soap.indexOf('Severity') == -1
                                     && soap.indexOf('Pattern') == -1 && soap.indexOf('Aggravated by') == -1 && soap.indexOf('Location') == -1 &&
                                    nextDisease.indexOf('The patient underwent') == -1 && nextDisease.indexOf('The patient also underwent') == -1 && nextDisease.indexOf('Status') == -1 && nextDisease.indexOf('Test Result') == -1 && nextDisease.indexOf('Onset') == -1 && nextDisease.indexOf('Duration') == -1 && nextDisease.indexOf('Severity') == -1
                                     && nextDisease.indexOf('Pattern') == -1 && nextDisease.indexOf('Aggravated by') == -1 && nextDisease.indexOf('Location') == -1) {

                                    finalSoapText += soap[soap.length - 1] == ":" || soap[soap.length - 1] == "." ? soap.replace(soap[soap.length - 1], ', ') : soap + ", ";
                                }
                                else {
                                    finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                                }
                            }
                            else {
                                finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                            }
                        }
                    });

                    $(soapText).find('div').remove();
                    var text = $(soapText).html();
                    if (text.indexOf('Unremarkable') != -1) {
                        finalSoapText += $(soapText).html() + "</p>"
                    }
                    else {
                        finalSoapText += '<br>' + $(soapText).html() + "</p>"
                    }

                    $ListMedicalHx.append("<li>" + finalSoapText + "</li>");

                }
                else {
                    $ListMedicalHx.append("<li>" + MedicalHxFill_Obj.MedicalHxSoapText + "</li>");
                }

                $DetailsDiv.append($ListMedicalHx);
                $SectionBodyMedicalHx.append($DetailsDiv);

                if ($(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId).length == 0) {
                    $mainDivMedicalHx.append($SectionBodyMedicalHx);

                    var MedicalHxHtml = $mainDivMedicalHx.html();
                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().addClass('initialVisitBody');

                    if (MedicalHxHtml != '') {
                        $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().append(MedicalHxHtml);
                    }

                    //Binding Hovering and onClick functions to Progress Note HTML
                    Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
                    return MedicalHxId;

                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul li:Last').attr('id');

                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul li:Last').get(0).outerHTML;
                    }

                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId).html($SectionBodyMedicalHx.html());
                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul').append(CommentHTML);
                }
            }
        }
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createMedicalHxBodyHTML: function (response, NoteHTMLCtrl, UnloadMedicalhx, hideAlertMessage) {
        Clinical_MedicalHx.checkMedicalHxExists();
        if (response.MedicalHxFill_JSON != null && response.MedicalHxFill_JSON != '') {
            var MedicalHxFill_Obj = JSON.parse(response.MedicalHxFill_JSON);
            var $mainDivMedicalHx = $(document.createElement('div'));

            var MedicalHxId = MedicalHxFill_Obj.MedicalHxId;
            if (MedicalHxId > 0) {
                var $SectionBodyMedicalHx = $(document.createElement('section'));
                $SectionBodyMedicalHx.attr('id', "Cli_MedicalHx_Main" + MedicalHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_MedicalHx_" + MedicalHxId);
                var $ListMedicalHx = $(document.createElement('ul'));

                $ListMedicalHx.attr('class', 'list-unstyled')

                $SectionBodyMedicalHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_MedicalHx_" + MedicalHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_MedicalHx_Main" + MedicalHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                var finalSoapText = '<p>';
                if (MedicalHxFill_Obj.MedicalHxSoapText != "") {
                    var soapText = document.createElement('div');
                    $(soapText).html(MedicalHxFill_Obj.MedicalHxSoapText);

                    $(soapText).find('div').each(function (index, item) {
                        var soap = $(this).html();
                        var nextDisease = $($(soapText).find('div')[index + 1]).html();

                        if (soap != "") {
                            if (nextDisease != null) {
                                if (soap.indexOf('The patient underwent') == -1 && soap.indexOf('The patient also underwent') == -1 && soap.indexOf('Status') == -1 && soap.indexOf('Test Result') == -1 && soap.indexOf('Onset') == -1 && soap.indexOf('Duration') == -1 && soap.indexOf('Severity') == -1
                                     && soap.indexOf('Pattern') == -1 && soap.indexOf('Aggravated by') == -1 && soap.indexOf('Location') == -1 &&
                                    nextDisease.indexOf('The patient underwent') == -1 && nextDisease.indexOf('The patient also underwent') == -1 && nextDisease.indexOf('Status') == -1 && nextDisease.indexOf('Test Result') == -1 && nextDisease.indexOf('Onset') == -1 && nextDisease.indexOf('Duration') == -1 && nextDisease.indexOf('Severity') == -1
                                     && nextDisease.indexOf('Pattern') == -1 && nextDisease.indexOf('Aggravated by') == -1 && nextDisease.indexOf('Location') == -1) {

                                    finalSoapText += soap[soap.length - 1] == ":" || soap[soap.length - 1] == "." ? soap.replace(soap[soap.length - 1], ', ') : soap + ", ";
                                }
                                else {
                                    finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                                }
                            }
                            else {
                                finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                            }
                        }
                    });
                    $(soapText).find('div').remove();
                    var text = $(soapText).html();
                    if (text.indexOf('Unremarkable') != -1) {
                        finalSoapText += $(soapText).html() + "</p>"
                    }
                    else {
                        finalSoapText += '<br>' + $(soapText).html() + "</p>"
                    }

                    $ListMedicalHx.append("<li>" + finalSoapText + "</li>");

                }
                else {
                    $ListMedicalHx.append("<li>" + MedicalHxFill_Obj.MedicalHxSoapText + "</li>");
                }
                $DetailsDiv.append($ListMedicalHx);
                $SectionBodyMedicalHx.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId).length == 0) {
                    $mainDivMedicalHx.append($SectionBodyMedicalHx);
                    Clinical_MedicalHx.updateMedicalHxHtml($mainDivMedicalHx.html(), MedicalHxId, NoteHTMLCtrl, hideAlertMessage);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId).html($SectionBodyMedicalHx.html());
                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText("MedicalHx", hideAlertMessage);
                    Clinical_MedicalHx.updateMedicalHxHtml("", MedicalHxId, NoteHTMLCtrl, hideAlertMessage);

                }

                if (UnloadMedicalhx == true) {
                    Clinical_MedicalHx.UnloadMedicalHistory(Clinical_MedicalHx.bNextPrev);
                }
            }
        }
    },

    createMedicalHxBodyHTMLFromNote: function (response, NoteHTMLCtrl, UnloadMedicalhx, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_MedicalHx.checkMedicalHxExists();
        if (response) {
            var MedicalHxFill_Obj = response;
            var $mainDivMedicalHx = $(document.createElement('div'));
            var MedicalHxId = MedicalHxFill_Obj.MedicalHxId;

            if (MedicalHxId > 0) {
                var $SectionBodyMedicalHx = $(document.createElement('section'));
                $SectionBodyMedicalHx.attr('id', "Cli_MedicalHx_Main" + MedicalHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_MedicalHx_" + MedicalHxId);
                var $ListMedicalHx = $(document.createElement('ul'));

                $ListMedicalHx.attr('class', 'list-unstyled')

                $SectionBodyMedicalHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_MedicalHx_" + MedicalHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_MedicalHx_Main" + MedicalHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                var finalSoapText = '<p>';
                if (MedicalHxFill_Obj.MedicalHxSoapText != "") {
                    var soapText = document.createElement('div');
                    $(soapText).html(MedicalHxFill_Obj.MedicalHxSoapText);

                    $(soapText).find('div').each(function (index, item) {
                        var soap = $(this).html();
                        var nextDisease = $($(soapText).find('div')[index + 1]).html();

                        if (soap != "") {
                            if (nextDisease != null) {
                                if (soap.indexOf('The patient underwent') == -1 && soap.indexOf('The patient also underwent') == -1 && soap.indexOf('Status') == -1 && soap.indexOf('Test Result') == -1 && soap.indexOf('Onset') == -1 && soap.indexOf('Duration') == -1 && soap.indexOf('Severity') == -1
                                     && soap.indexOf('Pattern') == -1 && soap.indexOf('Aggravated by') == -1 && soap.indexOf('Location') == -1 &&
                                    nextDisease.indexOf('The patient underwent') == -1 && nextDisease.indexOf('The patient also underwent') == -1 && nextDisease.indexOf('Status') == -1 && nextDisease.indexOf('Test Result') == -1 && nextDisease.indexOf('Onset') == -1 && nextDisease.indexOf('Duration') == -1 && nextDisease.indexOf('Severity') == -1
                                     && nextDisease.indexOf('Pattern') == -1 && nextDisease.indexOf('Aggravated by') == -1 && nextDisease.indexOf('Location') == -1) {

                                    finalSoapText += soap[soap.length - 1] == ":" || soap[soap.length - 1] == "." ? soap.replace(soap[soap.length - 1], ', ') : soap + ", ";
                                }
                                else {
                                    finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                                }
                            }
                            else {
                                finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                            }
                        }
                    });
                    $(soapText).find('div').remove();
                    if (finalSoapText == '<p>') {
                        finalSoapText += $(soapText).html() + "</p>"
                    }
                    else {
                        finalSoapText += '<br>' + $(soapText).html() + "</p>"
                    }

                    $ListMedicalHx.append("<li>" + finalSoapText + "</li>");

                }
                else {
                    $ListMedicalHx.append("<li>" + MedicalHxFill_Obj.MedicalHxSoapText + "</li>");
                }
                $DetailsDiv.append($ListMedicalHx);
                $SectionBodyMedicalHx.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId).length == 0) {
                    $mainDivMedicalHx.append($SectionBodyMedicalHx);
                    var medicalHxHtml = $mainDivMedicalHx.html();
                    if ($mainDivMedicalHx != '') {
                        $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().addClass('initialVisitBody');
                        $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().append(medicalHxHtml);
                    }

                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId).html($SectionBodyMedicalHx.html());
                    $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().find('#Cli_MedicalHx_Main' + MedicalHxId + ' ul').append(CommentHTML);
                }
            }
        }
        dfd.resolve();
        return dfd;
    },

    // This Function is called by Progress Notes (Fill MedicalHx Func, CopyAllNotesCategories)
    updateMedicalHxHtml: function (MedicalHxHtml, MedicalHxId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().addClass('initialVisitBody');
        if (MedicalHxHtml != '') {
            $(NoteHTMLCtrl + ' clinical_Medicalhx').parent().parent().append(MedicalHxHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (MedicalHxHtml != '') {
            Clinical_MedicalHx.AttachMedicalHxFromNotes(MedicalHxId, hideAlertMessage);
        }

    },

    //This Function detach Medical History From progress note
    detach_ComponentsMedicalHx: function (ComponentName, IsUpdate, MedicalHxComponentRemove) {
        var Clinical_MedicalHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Medicalhx').parent().parent().find('section[id*="Cli_MedicalHx_Main"]').map(function () {
            return this.id.replace("Cli_MedicalHx_Main", "");
        }).get().join(',');

        if (MedicalHxComponentRemove) {

            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_medicalhx').parent().parent().attr('NoteComponentId');
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Medical Hx']").remove()
            if (Clinical_ProgressNote.params["TemplateName"])
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Medicalhx').parent().parent().addClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Medicalhx').parent().parent().remove();
            var hxComponents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .HxComponent').length;

            if (NoteComponentId && NoteComponentId != "NCDummyId" && hxComponents == 0) {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Medicalhx').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('MedicalHx', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Medicalhx').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            }
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Medicalhx').parent().parent().find('section[id*="Cli_MedicalHx_Main"]').remove();
        }

        if (Clinical_MedicalHxIds == "" || Clinical_MedicalHxIds == "undefined") {
            //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            Clinical_ProgressNote.saveComponentSOAPText("MedicalHx", true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            Clinical_MedicalHx.DetachMedicalHxFromNotes_DBCall(Clinical_MedicalHxIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                        Clinical_ProgressNote.saveComponentSOAPText("MedicalHx", true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //This Functions ask for Detaching Medical Hx from Progress Note for current Patient Selected
    detachMedicalHxFromNotes: function (MedicalHxId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_medicalhx');
                var selectedValue = MedicalHxId.replace('Cli_MedicalHx_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_MedicalHx.DetachMedicalHxFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + MedicalHxId).remove();
                            //  Clinical_ProgressNote.updateProgressNoteHTML();
                            Clinical_ProgressNote.Add_NoText();
                            Clinical_ProgressNote.saveComponentSOAPText("MedicalHx", true);

                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () { },
                '1'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //This Functions attached Medical Hx to Progress Note for current Patient Selected
    AttachMedicalHxFromNotes: function (MedicalHxId, hideAlertMessage) {
        Clinical_MedicalHx.AttachMedicalHxFromNotes_DBCall(MedicalHxId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //If Attached MedicalHx Made new inseration to MedicalHx Table than good ids should be attached to HTML
                Clinical_ProgressNote.saveComponentSOAPText("MedicalHx", hideAlertMessage);
                $('#' + MedicalHxId).remove();

                // utility.DisplayMessages(response.Message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    //If MedicalHx Component which is dropeed in Progress note has no MedicalHx attached, than it will call for Latest MedicalHx for this patient
    //getLatestMedicalHxByPatientId: function (hideAlertMessage) {
    //    var strMessage = '';
    //    //   AppPrivileges.GetFormPrivileges("Notes_Notes", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
    //    if (strMessage == "") {
    //        Clinical_MedicalHx.getLatestClinical_MedicalHxByPatientId_DBCall().done(function (response) {
    //            response = JSON.parse(response);
    //            if (response.status != false) {
    //                Clinical_MedicalHx.createMedicalHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', hideAlertMessage);
    //            }
    //            else {
    //                utility.DisplayMessages(strMessage, 3);
    //            }

    //        });
    //    }
    //    else {
    //        utility.DisplayMessages(strMessage, 3);
    //    }
    //},

    //-----Server calls of Notes----------
    DetachMedicalHxFromNotes_DBCall: function (MedicalHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["MedicalHxId"] = MedicalHxId;
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
        objData["commandType"] = "detach_Medicalhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "MedicalHx");
    },

    AttachMedicalHxFromNotes_DBCall: function (MedicalHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["MedicalHxId"] = MedicalHxId;
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
        objData["commandType"] = "attach_Medicalhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "MedicalHx");
    },

    getLatestClinical_MedicalHxByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_Medicalhxby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "MedicalHx");
    },

    //--------------end progress Note-----------


    /* Author: Muhammad Irfan
       Date: 11/01/2016
       Overview: Following function is created to search ICD-9, ICD-10, Diagnosis in MedicalHx
    */

    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_MedicalHx", null, false);
    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function will show cross button on hover li
    */
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function will hide cross button on hover li
    */
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function delete the selected li from disease
    */
    deleteMedicalHxDisease: function (obj, ev) {
        //Start/25-1-2016/Abid Ali/ init deffered object
        var dfd = new $.Deferred();
        //End/25-1-2016/Abid Ali/ init deffered object
        ev.stopPropagation();
        var diseaseId = $(obj).attr('id');
        //if (diseaseId < 0) {
        //    $(obj).remove();
        //    //Start//07-03-2016//Ahmad Raza// fixed EMR Bug #434
        //    $('#' + Clinical_MedicalHx.params.PanelID + ' #sectionDiseaseDetails').resetAllControls(null);
        //    //End//07-03-2016//Ahmad Raza// fixed EMR Bug #434

        //    //Start/25-1-2016/Abid Ali/ resolve deffered object
        //    dfd.resolve();
        //    //End/25-1-2016/Abid Ali/ resolve deffered object
        //}
        // else {
        AppPrivileges.GetFormPrivileges("History_Medical Hx", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('23', function () {
                    var selectedValue = $(obj).attr('id');
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    var diseaseId = $(obj).attr('id');
                    if (diseaseId < 0) {
                        //Start 01-07-2016 Humaira Yousaf
                        Clinical_MedicalHx.enableFavoriteList($(obj));
                        //Start 01-07-2016 Humaira Yousaf
                        $(obj).remove();
                        //Start//07-03-2016//Ahmad Raza// fixed EMR Bug #434
                        $('#' + Clinical_MedicalHx.params.PanelID + ' #sectionDiseaseDetails').resetAllControls(null);
                        if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            Clinical_MedicalHx.deleteCacheDisease(diseaseId);
                        }
                        utility.DisplayMessages("Successfully Deleted", 1);
                        //End//07-03-2016//Ahmad Raza// fixed EMR Bug #434

                        //Start/25-1-2016/Abid Ali/ resolve deffered object
                        dfd.resolve();
                        //End/25-1-2016/Abid Ali/ resolve deffered object
                    }
                    else {
                        Clinical_MedicalHx.medicalHxDiseaseDelete(diseaseId).done(function (response) {

                            response = JSON.parse(response);
                            if (response.status != false) {
                                //Start 01-07-2016 Humaira Yousaf
                                Clinical_MedicalHx.enableFavoriteList($(obj));
                                //Start 01-07-2016 Humaira Yousaf
                                $('#' + Clinical_MedicalHx.params.PanelID + ' #sectionDiseaseDetails').resetAllControls(null);
                                $(obj).remove();
                                $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #sectionDiseaseDetails").addClass('disableAll');
                                //Start/25-1-2016/Abid Ali/ resolve deffered object
                                dfd.resolve();
                                //End/25-1-2016/Abid Ali/ resolve deffered object
                                if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    Clinical_MedicalHx.deleteCacheDisease(diseaseId);
                                }
                                utility.DisplayMessages(response.Message, 1);

                                Clinical_MedicalHx.loadMedicalHx('disease', true);
                                $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx").bootstrapValidator('resetForm', true);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    '23'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //}

        dfd.done(function () {
            //Start/25-1-2016/Abid Ali/ Disable section detail when ul is empty
            //var $diseases = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #MedicalHxDisease ul#ulMedicalDisease");
            //if (!$diseases.has('li').length)
            $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #sectionDiseaseDetails").addClass('disableAll');
            //End/25-1-2016/Abid Ali/ Disable section detail when ul is empty
        });


    },

    deleteCacheDisease: function (diseaseId) {
        var diseaseIndex = -1;
        if (Clinical_HistorySummary.HistoryCacheList.MedicalHx) {
            $.grep(Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList, function (item, index) {
                if (item.DiseaseId == diseaseId) {
                    diseaseIndex = index;
                    return;
                }
            });

            if (diseaseIndex != -1) {
                Clinical_HistorySummary.HistoryCacheList.MedicalHx.MedicalDiseaseList.splice(diseaseIndex, 1);
            }
        }
    },

    /*
    Author: Abid Ali
    Date: 25/01/2016
    Overview: return true if Disease has active class
    */
    checkActiveDisease: function () {
        var $disease = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #MedicalHxDisease ul#ulMedicalDisease li");
        var isActive = false;
        $disease.each(function () {
            if ($(this).hasClass('active')) {
                isActive = true;
            }
        });
        return isActive;
    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function call api service of delete
    */
    medicalHxDiseaseDelete: function (diseaseId) {
        var objData = new Object();
        objData["DiseaseId"] = diseaseId;
        //Start/26-1-2016/Abid Ali/ pass medicalHx Id
        var medicalHxId = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #hfMedicalHxId").val();
        objData["MedicalHxId"] = medicalHxId;
        //Start/26-1-2016/Abid Ali/ pass medicalHx Id
        objData["commandType"] = "DELETE_MEDICALHXDISEASE";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "MedicalHx");
    },
    /*
    Author: MuhammadIrfan
    Date: 12/01/2016
    Overview: This function perform fill action on click disease li
    */
    fillMedicalHxDisease: function (obj, ev) {

        ev.stopPropagation();

        if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_MedicalHx.medicalHxJSON != '') {
            if (Clinical_MedicalHx.medicalHxJSON != $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").getMyJSONByName()) {
                var diseaseId = $('#' + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease > li.active").attr('id');
                var diseaseData = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").getMyJSONByName();
                Clinical_MedicalHx.cacheMedicalHxJSON(diseaseId, diseaseData);
            }
        }

        Clinical_MedicalHx.currentSelectedDisease["DiseaseId"] = $(obj).attr('id');
        $('#' + Clinical_MedicalHx.params.PanelID + ' #btnAddVitalsOnNote').prop('disabled', false);
        $("#" + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx").bootstrapValidator('resetForm', true);
        var diseaseId = $(obj).attr('id');
        Clinical_MedicalHx.resetValues();
        $('#' + Clinical_MedicalHx.params.PanelID + ' #frmClinicalMedicalHx #btnMedicalHxSave').hide();
        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #sectionDiseaseDetails").removeClass('disableAll');

        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #MedicalHxDisease ul#ulMedicalDisease li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == diseaseId) {
                if ($(this).hasClass("active") == false) {
                    $(this).addClass("active");
                    $(this).find('div').css('display', '');
                }
            }
            else {
                $(this).removeClass("active");
                $(this).find('div').css('display', 'none');
            }
        });

        Clinical_MedicalHx.loadMedicalHxComponent('disease', diseaseId, true, null, false);



        //alert('called');

    },
    /* Author: MuhammadIrfan
      Date: 12/01/2016
     Overview: Resets medicalhx disease details section contrrols*/
    resetValues: function () {

        $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #sectionDiseaseDetails").find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
            $(this).val('');
        });

    },

    toggleHorSmallLeftIcon: function (e) {
        if (e === undefined) {
            var icon = $('.toggleHorSmallLeft');
            icon.each(function (i) {
                var $this = $(this).children("section").children();
                if ($(this).hasClass("toggled")) {
                    $this.append('<i class="fa fa-chevron-down"></i>');
                }
                else {
                    $this.append('<i class="fa fa-chevron-up"></i>');
                }
            });
        }
        else if (e != undefined) {
            var icon = $(e.children().children());
            if (icon.hasClass("fa-chevron-up")) {
                icon.toggleClass("fa-chevron-down fa-chevron-up")
            }
            else {
                icon.toggleClass("fa-chevron-up fa-chevron-down")
            }
        }
    },

    selectAllFavoriteListContent: function () {

        $('#' + Clinical_MedicalHx.params.PanelID + ' #ulFavoriteListMedicalHxContent li').each(function (i, item) {
            $(item).trigger("click");
        });
    },
    AutoSearchFavMedicalHx: function () {
        utility.Keyupdelay(function () {
            Clinical_MedicalHx.loadfavoriteListContent();
        });
    },
    loadfavoriteListContent: function (obj) {
        if (typeof obj == typeof undefined || obj == null) {
            obj = $('#' + Clinical_MedicalHx.params.PanelID + ' #ddlFavoriteListMedicalHx');
        }
        var SearchData = $('#' + Clinical_MedicalHx.params.PanelID + ' #FavSearchBox').val();
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            //Start 01-07-2016 Humaira Yousaf to disable Select All link
            if (selectedOption.attr("id") != "-1") {
                Clinical_MedicalHx.favoriteList_CPTSearch(selectedOption.attr("id"), SearchData);
            }
            else {
                $('#' + Clinical_MedicalHx.params.PanelID + ' #ulFavoriteListMedicalHxContent').empty();
                $('#' + Clinical_MedicalHx.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
            }
            //End 01-07-2016 Humaira Yousaf to disable Select All link
        }
    },

    favoriteList_CPTSearch: function (FavoriteListId, SearchData) {
         var $UL = $('#' +Clinical_MedicalHx.params.PanelID + ' #ulFavoriteListMedicalHxContent');
        Clinical_MedicalHx.searchFavoriteList_ICD_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

               
                $UL.empty();

                if (response.FavoriteListICDCount > 0) {
                    // $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ulFavCompliantDisease li").remove();
                    var FavoriteListJSON = JSON.parse(response.FavoriteListICDJSON);

                    if (FavoriteListJSON.length > 0) {
                        $('#' + Clinical_MedicalHx.params.PanelID + ' #favSelectAllLink').removeClass('disableAll');
                    }
                    else {
                        $('#' + Clinical_MedicalHx.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
                    }

                    var li = "";
                    $.each(FavoriteListJSON, function (i, item) {
                        if (typeof item.ICD9Code == 'undefined' || item.ICD9Code == null) { item.ICD9Code = ''; }
                        if (typeof item.ICD10Code == 'undefined' || item.ICD10Code == null) { item.ICD10Code = ''; }
                        if (typeof item.SNOMEDID == 'undefined' || item.SNOMEDID == null) { item.SNOMEDID = ''; }
                        if (typeof item.ICD10CodeDescription == 'undefined' || item.ICD10CodeDescription == null) { item.ICD10CodeDescription = ''; }
                        var diagnosis = item.ICD9Code + " - " + item.ICD10Code + " - " + item.SNOMEDID + " - " + item.ICD10CodeDescription;
                        var ICD9Code = "" + item.ICD9Code + "";
                        var ICD10Code = "" + item.ICD10Code + "";
                        var ICD9CodeDescription = "" + item.ICD9CodeDescription + "";
                        var ICD10CodeDescription = "" + item.ICD10CodeDescription + "";
                        var SNOMEDID = "" + item.SNOMEDID + "";
                        var SNOMEDDescription = "" + item.SNOMEDDescription + "";

                        var onclick = 'Clinical_MedicalHx.BindMedicalHxUl(\'' + item.ICD9Code + '\',\'' + item.ICD9CodeDescription + '\',\'' + item.ICD10Code + '\',\'' + item.ICD10CodeDescription + '\',\'' + item.SNOMEDID + '\',\'' + item.SNOMEDDescription + '\',this)';

                        var LiId = item.ICD9CodeDescription + '-' + item.SNOMEDID;

                        var isFound = Clinical_MedicalHx.isFavoriteHistoryFound(LiId, item.ICD9CodeDescription);
                        if (isFound == true) {
                            $UL.append('<li class="disableAll" onclick="' + onclick + '" id="' + LiId + '">' + item.ICD9CodeDescription + '</li>');
                        }
                        else {
                            $UL.append('<li onclick="' + onclick + '" id="' + LiId + '">' + item.ICD9CodeDescription + '</li>');
                        }

                        //$UL.append('<li onclick="' + onclick + '">' + item.ICD9CodeDescription + ' - ' + item.SNOMEDID + '</li>');


                    });

                }
            }
            else {
                $UL.empty();
            }
        });
    },


    searchFavoriteList_ICD_DBCall: function (FavoriteListICDId, FavoriteListId, PageNumber, RowsPerPage, SearchData) {
        if (PageNumber == null) {
            PageNumber = -1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = -1;
        }
        var objData = {};
        objData["IsActive"] = $('#' + Favorite_Problems.params.PanelID + ' #divSwitch #switchActive').is(':checked');
        objData["FavoriteListId"] = FavoriteListId == null ? 0 : FavoriteListId;
        objData["FavoriteListICDId"] = FavoriteListICDId == null ? 0 : FavoriteListICDId;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = 0;
        }
        else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["SearchData"] = SearchData;
        objData["commandType"] = "load_favoritelist_icd";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    BindMedicalHxUl: function (icd9Code, icd9Description, icd10Code, icd10Description, snomedCode, snomedDescription, sender) {

        var currId = -1;
        $("#pnlClinicalMedicalHx #frmClinicalMedicalHx div#MedicalHxDisease #ulMedicalDisease li[id*='-']").each(function (i, item) {

            currId = $(this).attr("id");

        });

        currId = parseInt(currId) + (-1);

        var li = "<li  id=" + currId + " onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd9Description + "<span class='removeIconListHover' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";


        var IsAlreadyExist = false;
        $('#pnlClinicalMedicalHx #ulMedicalDisease li').each(function () {
            if ($(this).attr('icd9Code') == icd9Code && $(this).attr('icd9Desc') == icd9Description &&
                $(this).attr('icd10Code') == icd10Code && $(this).attr('icd10Desc') == icd10Description &&
                $(this).attr('snomedCode') == snomedCode && $(this).attr('snomedDesc') == snomedDescription) {
                IsAlreadyExist = true;
            }
        });

        if (!IsAlreadyExist) {
            $('#pnlClinicalMedicalHx #ulMedicalDisease').append(li);
            $(li).trigger('click');
            $('#pnlClinicalMedicalHx #txtDisease').val('');
            if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                var diseaseId = $('#' + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease > li.active").attr('id');
                var disease = $(li).get(0).outerHTML;
                var diseaseDetails = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").clone();
                $(diseaseDetails).resetAllControls(null);
                var diseaseData = $(diseaseDetails).getMyJSONByName();
                Clinical_MedicalHx.cacheMedicalHxJSON(diseaseId, diseaseData, disease);
            }
            $(sender).addClass('disableAll');
        }
        else {
            utility.DisplayMessages('Diagnosis already added', 2);

            $('#pnlClinicalMedicalHx #txtDisease').val('');
        }

    },

    AddNewLabRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription) {

        var medicalHxDiseaseUl = $('#' + Clinical_MedicalHx.params.PanelID + " #frmClinicalMedicalHx #ulMedicalDisease");

        // var rowId = CurrentRow.attr("id");

        var li = "<li  id=" + currId + " onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Code=\"" + cptCode + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + procDesc + " - " + cptCode + "<span class='removeIconListHover' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";

        //var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        //var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + procDesc + '"  />';

        $(medicalHxDiseaseUl).append(li);
        //add cptCode and description to the test row

        // return CurrentRow;
    },

    //Function Name: isFavoriteHistoryFound
    //Author Name: Humaira Yousaf
    //Created Date: 01-07-2016
    //Description: Checks if Favorite History is found
    isFavoriteHistoryFound: function (favICDCode, favCPTDesc) {

        var isFound = false;
        $("#" + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease li").each(function (index, item) {
            if ($(item).attr('icd9desc') != null) {
                var currentRowCPTCode = $(item).text() != null ? $(item).attr('icd9desc') + '-' + $(item).attr('snomedcode') : "";
                if (currentRowCPTCode == favICDCode) {
                    isFound = true;
                }
            }
            //else {
            //    var currentRowCPTCode = $(item).find("input[id*='ProcedureProcedure']").val() != null ? $(item).find("input[id*='ProcedureProcedure']").val() : "";
            //    if (currentRowCPTCode == favCPTCode) {
            //        isFound = true;
            //    }
            //}
        });

        return isFound;
    },

    //Author: Ahmad Raza
    //Date :  12-07-2016
    //Function Name: validateSpecialCharacters
    //Description: This function will validate the special characters
    validateSpecialCharacters: function (event) {
        var valid = (event.which >= 48 && event.which <= 57) || (event.which >= 65 && event.which <= 90) || (event.which >= 97 && event.which <= 122);
        if (!valid) {
            event.preventDefault();
        }

    },

    gridLoad: function (response) {
        var isactive = $('#' + Clinical_MedicalHx.params.PanelID + ' #pnlMedicalHx_Result #divSwitch #switchActive').attr('isactive');

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvPastMedicalHx")) {
            $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvPastMedicalHx").dataTable().fnClearTable();
            $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvPastMedicalHx").dataTable().fnDestroy();
            $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvPastMedicalHx tbody").find("tr").remove();
        }
        var logCount = JSON.parse(response);
        if (logCount.HxLogSoapCount > 0) {
            var LoadJSONData = JSON.parse(logCount.HxLogSoap_JSON); //Parsing array to JSON
            var counter = null;
            for (var i = 0; i < LoadJSONData.length; i++) {

                //  $.each(LoadJSONData, function (i, item) {

                var $row = $('<tr/>');

                // $row.attr("onclick", "Clinical_MedicalHx.CDSEdit('" + item.CDSId + "',event);");
                //$row.attr("id", "gvCDS_row" + item.CDSId);

                var text = LoadJSONData[i].SoapText;

                counter = i;
                $row.append('<td style="display:none;">' + counter + '</td><td>' + LoadJSONData[i].Action + '</td><td id="sptxt">' + $('<a/>').html($('<a/>').html(text).text()).text() + '</td><td>' + LoadJSONData[i].ModifiedOn + " " + LoadJSONData[i].ModifiedBy + '</td>');
                $row.find('#sptxt').html($('<a/>').html(text).text());
                $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvPastMedicalHx tbody").last().append($row);

                //   });
            }
        }
        else {
            $("#" + Clinical_MedicalHx.params.PanelID + ' #pnlMedicalHx_Result #dgvPastMedicalHx').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Known Medical History"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bInfo": false, "bPaginate": false, "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_MedicalHx.params.PanelID + ' #pnlMedicalHx_Result #dgvPastMedicalHx'))
            ;
        else {
            $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvPastMedicalHx").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[0, "asc"]], "aoColumnDefs": [{ "bInfo": false, "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        }

        $("#" + Clinical_MedicalHx.params.PanelID + " #pnlMedicalHx_Result #dgvPastMedicalHx_filter").remove();
    },

    //Function Name: enableFavoriteList
    //Author Name: Humaira Yousaf
    //Created Date: 01-07-2016
    //Description: Enables Favorite List

    enableFavoriteList: function (deleteRow) {

        $('#' + Clinical_MedicalHx.params.PanelID + ' #ulFavoriteListMedicalHxContent li').each(function (index, item) {
            if ($(deleteRow).attr('icd9desc') != null) {
                var deleteRowCPTCode = $(deleteRow).text() != null ? $(deleteRow).attr('icd9desc') + '-' + $(deleteRow).attr('snomedcode') : "";
                if (deleteRowCPTCode == $(item).attr("id")) {
                    $(item).removeClass('disableAll');
                }
            }
            else {
                $(item).removeClass('disableAll');

                //var deleteRowCPTCode = $(deleteRow).find("input[id*='ProcedureProcedure']").val() != null ? $(deleteRow).find("input[id*='ProcedureProcedure']").val() : "";
                //if (deleteRowCPTCode == $(item).attr("id")) {
                //    $(item).removeClass('disableAll');
                //}
            }
        });
    },

    enableDisbaleUlItems: function (ulId, isEnable) {

        $('#' + Clinical_MedicalHx.params.PanelID + ' #' + ulId + ' li').each(function () {

            if (isEnable) {
                $(this).removeClass('disableAll');
            }
            else {
                $(this).addClass('disableAll');
            }
        });
    },

    SearchTypeChange: function () {
        var IsFreeText = false;
        var RadioButtonVal = false;
        var panel = "";
        if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #pnlClinicalMedicalHx";
        }
        else {
            panel = "#pnlClinicalMedicalHx";
        }
        $('input[name=SearchType]:checked', panel).val() == 1 ? IsFreeText = true : IsFreeText = false;
        if (IsFreeText) {
            $(panel + " #DivICDAutoComplete").hide();
            if ($(panel + " #DivFreeText").hasClass("hidden")) {
                $(panel + " #DivFreeText").removeClass("hidden");
            }
        }
        else {
            $(panel + " #DivICDAutoComplete").show();
            if (!$(panel + " #DivFreeText").hasClass("hidden")) {
                $(panel + " #DivFreeText").addClass("hidden");
            }
        }
        Clinical_MedicalHx.SaveFreeTextStatus();

    },
    BindDisease: function (obj) {
        var FreeTextValue = $(obj).val().trim().toLowerCase();
        if (FreeTextValue != "") {
            var currId = -1;
            $("#pnlClinicalMedicalHx #frmClinicalMedicalHx #MedicalHxDisease ul#ulMedicalDisease li[id*='-']").each(function (i, item) {
                currId = $(this).attr("id");
            });

            currId = parseInt(currId) + (-1);

            var li = "<li  id=" + currId + " onclick='Clinical_MedicalHx.fillMedicalHxDisease(this, event);' onmouseover='Clinical_MedicalHx.showIcon(this);' onmouseout='Clinical_MedicalHx.hideIcon(this);' icd9Desc=\"" + $(obj).val() + "\" FreeText=\"" + FreeTextValue + "\"><a href='#'>" + FreeTextValue + "<span class='removeIconListHover' onclick='Clinical_MedicalHx.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            var IsAlreadyExist = false;
            $('#pnlClinicalMedicalHx #ulMedicalDisease li').each(function () {
                if ($(this).attr('icd9Desc').toLowerCase() == FreeTextValue) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#pnlClinicalMedicalHx #ulMedicalDisease').append(li);
                $(li).trigger('click');
                $('.modal-backdrop').removeClass('in');
                $('.modal-backdrop').addClass('out');
                $('.modal-backdrop').hide();
                $(obj).val('');

                if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {

                    $('#' + Clinical_MedicalHx.params.PanelID + ' #txtCPTCode').val("");
                    $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTCode').val("");
                    $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTDescription').val("");
                    $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDCode').val("");
                    $('#' + Clinical_MedicalHx.params.PanelID + ' #hfCPTSNOMEDDescription').val("");

                    var diseaseId = $('#' + Clinical_MedicalHx.params.PanelID + " #ulMedicalDisease > li.active").attr('id');
                    var disease = $(li).get(0).outerHTML;
                    var diseaseDetails = $('#' + Clinical_MedicalHx.params.PanelID + " #sectionDiseaseDetails").clone();
                    $(diseaseDetails).resetAllControls(null);
                    var diseaseData = $(diseaseDetails).getMyJSONByName();
                    Clinical_MedicalHx.cacheMedicalHxJSON(diseaseId, diseaseData, disease);
                }
            }
            else {
                utility.DisplayMessages('Disease already added', 2);
                $(obj).val('');
            }
        }
        else {
            $(obj).val('');
        }

    },

    // Start 4/1/2016 Muhammad Ahmad Imran
    //Purpose Save/update favList Status
    SaveFreeTextStatus: function () {
        if (Clinical_MedicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #pnlClinicalMedicalHx";
        }
        else {
            panel = "#pnlClinicalMedicalHx";
        }
        var IsFreeText = false;
        $('input[name=SearchType]:checked', panel).val() == 1 ? IsFreeText = true : IsFreeText = false;
        EMRUtility.insertUpdateFreeTextStatus("Clinical_MedicalHx", IsFreeText);
    },
}