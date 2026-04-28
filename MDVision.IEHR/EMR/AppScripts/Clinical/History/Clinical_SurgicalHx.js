//Author: Muhammad Arshad
//Date: 14-01-2016
//This file will handle all actions performed for Surgical History and it's child handling
//Once SurgicalHx will be created then it's child can be created then.
Clinical_SurgicalHx = {
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    cloneSurgicalForm: null,
    bNextPrev: false,
    controlToInvoke: null,
    ArrayDisease: [],
    currentSelectedDisease: [],
    lastSelectedType: null,
    FavListName: 'ClinicalSurgicalHx',
    surgicalHxJSON: '',
    surgicalDate: '',
    unremarkable: false,
    overallComments: '',
    //surgicalHxJSON: '',
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will be called once tab is clicked, it expects parameters to be used for SurgicalHx
    Load: function (params) {
        Clinical_SurgicalHx.params = params;

        $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (Clinical_SurgicalHx.params.mode == null) {
            Clinical_SurgicalHx.params.mode = "Add";
        }
        if (Clinical_SurgicalHx.params.PanelID != 'pnlClinicalSurgicalHx') {
            Clinical_SurgicalHx.params.PanelID = Clinical_SurgicalHx.params.PanelID + ' #pnlClinicalSurgicalHx';
        } else {
            Clinical_SurgicalHx.params.PanelID = 'pnlClinicalSurgicalHx';
            Clinical_SurgicalHx.params.CurrentNotesProviderId = "";
        }
        Clinical_SurgicalHx.ResetFormData();
        if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabNotes") {
            Clinical_SurgicalHx.bIsFirstLoad = true;
            $('#divViewHistorySummary').addClass('hidden');
            $(' #pnlClinicalSurgicalHx').removeClass('row');
        }
        var SurgicalHxId = "";
        if (Clinical_SurgicalHx.params.mode == "Add" || Clinical_SurgicalHx.params.SurgicalHxId == null || Clinical_SurgicalHx.params.SurgicalHxId == "" || Clinical_SurgicalHx.params.SurgicalHxId == "-1") {
            SurgicalHxId = "-1";
        }
        else if (Clinical_SurgicalHx.params.mode == "Edit") {
            SurgicalHxId = Clinical_SurgicalHx.params.SurgicalHxId;
            //Clinical_SurgicalHx.SurgicalHxEdit(SurgicalHxId);
        }

        if (Clinical_SurgicalHx.bIsFirstLoad == true) {
            EMRUtility.setFavoriteSectionStyle(Clinical_SurgicalHx.params.PanelID);
            Clinical_SurgicalHx.favoriteListSearch();
            //Load Dropdown
            //if (Clinical_SurgicalHx.bIsFirstLoad) { // start of if condition
            //    Clinical_SurgicalHx.bIsFirstLoad = false;
            var self = $('#' + Clinical_SurgicalHx.params.PanelID);

            self.loadDropDowns(true).done(function () {

                Clinical_SurgicalHx.loadSurgicalHx("disease", true);
                Clinical_SurgicalHx.validateSurgicalHx();

                if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {

                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnSurgicalSave').addClass('hidden');
                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');
                    var details = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").clone();
                    $(details).resetAllControls(null);
                    Clinical_SurgicalHx.surgicalHxJSON = $(details).getMyJSONByName();
                }
                else {
                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnSurgicalSave').removeClass('hidden');
                }


            });
            Clinical_SurgicalHx.loadAllAutocomplete();
            //end Load Dropdown
            utility.CreateDatePicker(Clinical_SurgicalHx.params.PanelID + '  #dtpSurgicalHxDate', function () {
            }, true);

            utility.CreateDatePicker(Clinical_SurgicalHx.params.PanelID + '  section#sectionSurgicalDetails div#SurgicalDetails #dtpSurgicalSurgeryDate', function () {
                // calculateAgeAtSurgery method will be called in CompareSurgeryDatewithDateofBirth() method.
                // change made by faizan ameen.


                // Clinical_SurgicalHx.calculateAgeAtSurgery();
                Clinical_SurgicalHx.CompareSurgeryDatewithDateofBirth();
            }, false);

            if ($('#' + Clinical_SurgicalHx.params.PanelID + ' #PatientProfile #hfPatientId').val() != "") {
                $('#' + Clinical_SurgicalHx.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            }
            //22/12/2015//AhmadRaza//Form Serialization
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').data('serialize', $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').serialize());


            //Code for progress note navigation
            if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                //taking clone and remove main surgicalhx form
                // Clinical_SurgicalHx.takeDeepCloneAndRemoveSection('#ctrlPanClinical > #pnlClinicalSurgicalHx');
                //taking clone of the main surgicalhx form


                EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_SurgicalHx.params.PanelID, 'History', 'SurgicalHx', 'Clinical_SurgicalHx.unLoadTab(true);', null, true);
                $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnAddVitalsOnNote').show();


                if ($('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #ulSurgicalDisease li.active").length > 0) {
                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnAddVitalsOnNote').prop('disabled', false);
                }
                else {
                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnAddVitalsOnNote').prop('disabled', true);
                }
                //   $('#' + Clinical_SurgicalHx.params.PanelID + '  #dtpSurgicalHxDate').prop('disabled', true);
            } else {
                //taking clone and remove noteprogress surgicalhx form
                // Clinical_SurgicalHx.takeDeepCloneAndRemoveSection('#ctrlPanClinical > #pnlClinicalProgressNote');
                //taking clone and remove noteprogress surgicalhx form

                $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnAddVitalsOnNote').hide();
                $('#' + Clinical_SurgicalHx.params.PanelID + '  #dtpSurgicalHxDate').prop('disabled', false);
            }
            //} // end of if condition

            Clinical_SurgicalHx.domReadyFunction();
            Clinical_SurgicalHx.bIsFirstLoad = false;
            if (Clinical_SurgicalHx.lastSelectedType == "FreeText") {
                $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #rdofreetext").prop("checked", true);
            } else {
                $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #rdoprocedure").prop("checked", true);
            }

        }


        if (EMRUtility.getFreeTextStatus("Clinical_SurgicalHx")) {
            var panel = "#pnlClinicalSurgicalHx";
            if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalSurgicalHx";
            }

            $(panel + " #DivICDAutoComplete").addClass("hidden");
            if ($(panel + " #DivFreeText").hasClass("hidden")) {
                $(panel + " #DivFreeText").removeClass("hidden");
            }
            $(panel + " #rdofreetext").prop("checked", true);
        }
        else {
            var panel = "#pnlClinicalSurgicalHx";
            if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                panel = "#pnlClinicalProgressNote #pnlClinicalSurgicalHx";
            }

            $(panel + " #DivICDAutoComplete").removeClass("hidden");
            if (!$(panel + " #DivFreeText").hasClass("hidden")) {
                $(panel + " #DivFreeText").addClass("hidden");
            }
            $(panel + " #rdoprocedure").prop("checked", true);
        }

        //if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {

        //    $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnSurgicalSave').addClass('hidden');
        //    $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');
        //    var details = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").clone();
        //    $(details).resetAllControls(null);
        //    Clinical_SurgicalHx.surgicalHxJSON = $(details).getMyJSONByName();
        //}
        //else {
        //    $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnSurgicalSave').removeClass('hidden');
        //}

        if (EMRUtility.getFavListStatus(Clinical_SurgicalHx.FavListName)) {
            $('#' + Clinical_SurgicalHx.params.PanelID + " #favSectionDiv").addClass("toggledHor");
            $('#' + Clinical_SurgicalHx.params.PanelID + " #FormDiv").addClass("toggleHorContainer");
        }
        else {
            $('#' + Clinical_SurgicalHx.params.PanelID + " #favSectionDiv").removeClass("toggledHor");
            $('#' + Clinical_SurgicalHx.params.PanelID + " #FormDiv").removeClass("toggleHorContainer");
        }
    },
    ResetFormData: function () {
        var details = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx");
        $(details).resetAllControls(null);
        var diseaseDetail = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails");
        $(diseaseDetail).resetAllControls(null);
        Clinical_SurgicalHx.bIsFirstLoad = true;
        $('#' + Clinical_SurgicalHx.params.PanelID + ' #Surgical').removeClass('disableAll');
    },
    changeProcedureField: function () {

        if ($('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #rdofreetext").prop("checked") == true) {
            Clinical_SurgicalHx.lastSelectedType = "FreeText";
            //$('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtDisease").addClass('hidden');
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #DivICDAutoComplete").addClass('hidden');
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #btnsearchcpt").addClass('hidden');
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtFreeText").removeClass('hidden');
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #DivFreeText").removeClass('hidden');
        } else {
            Clinical_SurgicalHx.lastSelectedType = "CPT";
            //   $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtDisease").removeClass('hidden');
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #btnsearchcpt").removeClass('hidden');
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtFreeText").addClass('hidden');
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #DivFreeText").addClass('hidden');
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #DivICDAutoComplete").removeClass('hidden');
        }

        Clinical_SurgicalHx.SaveFreeTextStatus();

    },
    SaveFreeTextStatus: function () {
        if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "#pnlClinicalProgressNote #pnlClinicalSurgicalHx";
        }
        else {
            panel = "#pnlClinicalSurgicalHx";
        }
        var IsFreeText = false;
        $('input[name=rdoprocedure]:checked', panel).val() == 1 ? IsFreeText = true : IsFreeText = false;
        EMRUtility.insertUpdateFreeTextStatus("Clinical_SurgicalHx", IsFreeText);
    },
    createFreeTextLi: function () {

        var diseaseText = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtFreeText").val();
        if (diseaseText) {
            diseaseText = diseaseText.trim();
        }
        if (diseaseText != null && diseaseText != '') {
            var currId = -1;
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #Surgical ul#ulSurgicalDisease li[id*='-']").each(function (i, item) {

                currId = $(this).attr("id");

            });

            currId = parseInt(currId) + (-1);
            var li = "<li  id=" + currId + " onclick='Clinical_SurgicalHx.fillSurgicalHxDisease(this, event);' onmouseover='Clinical_SurgicalHx.showIcon(this);' onmouseout='Clinical_SurgicalHx.hideIcon(this);' ><a href='#'>" + diseaseText + "<span class='removeIconListHover' onclick='Clinical_SurgicalHx.deleteSurgicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"
            var IsAlreadyExist = false;
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #ulSurgicalDisease li").each(function () {
                if ($(this).text().toLowerCase() == diseaseText.toLowerCase()) {
                    IsAlreadyExist = true;
                }
            });

            if (!IsAlreadyExist) {
                $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #ulSurgicalDisease").append(li);
                $(li).trigger('click');
                $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtFreeText").val('');
                if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    var diseaseId = $('#' + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease > li.active").attr('id');
                    var disease = $(li).get(0).outerHTML;
                    var diseaseDetail = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").clone();
                    $(diseaseDetail).resetAllControls(null);
                    var diseaseData = $(diseaseDetail).getMyJSONByName();
                    Clinical_SurgicalHx.cacheSurgicalHxJSON(diseaseId, diseaseData, disease);
                }
            }
            else {
                utility.DisplayMessages('Procedure already added', 2);
            }
        }
        else {
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtFreeText").val("");
        }

    },
    //Author: Muhammad Arshad
    //Date: 14-01-2016
    //This function will handle Initialization of KeyPad control

    showSurgicalHxHistory: function (surgicalHxId) {

        var parentCtrlId = Clinical_SurgicalHx.params.TabID;
        var grantParentCtrlId = null;
        if (Clinical_SurgicalHx.params.TabID == "clinicalTabProgressNote") {

            parentCtrlId = "Clinical_HistorySummary";
            grantParentCtrlId = "pnlClinicalProgressNote";
        }

        EMRUtility.showCurrentItemHistory(Clinical_SurgicalHx.params.PanelID, null, null, "SurgicalHx,SurgicalHx_Disease", null, parentCtrlId, surgicalHxId, grantParentCtrlId);
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

    domReadyFunction: function () {

        $(function () {


            (function ($) {
                'use strict';
                $(function () {
                    $('#' + Clinical_SurgicalHx.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);


            Clinical_SurgicalHx.calculateAgeAtSurgery();
            Clinical_SurgicalHx.CompareSurgeryDatewithDateofBirth();
            $('#' + Clinical_SurgicalHx.params.PanelID + ' [data-plugin-toggle]').each(function () {
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
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx [data-plugin-keyboard-numpad]').keyboard({
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

    //Author: Abid Ali
    //Date: 29-01-2016
    //For Form submission and bootstrap validation
    validateSurgicalHx: function () {
        $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (e.type == "success") {
                Clinical_SurgicalHx.surgicalHxSave('disease');
            }
            e.type = "";
        });
    },
    //Author: Ahmad Raza
    //Date: 03-02-2016
    //calculating age at surgery
    calculateAgeAtSurgery: function () {
        var $surgeryDateContext = $('#' + Clinical_SurgicalHx.params.PanelID + '  section#sectionSurgicalDetails div#SurgicalDetails #dtpSurgicalSurgeryDate');
        var surgeryDate = $surgeryDateContext.val();
        var Totalyears = "";
        if (surgeryDate != "") {
            var second = 1000;
            var minute = second * 60;
            var hour = minute * 60;
            var day = hour * 24;
            var week = day * 7;
            var birthday = new Date($('#PatientProfile #hfPatientDOB').val());
            surgeryDate = new Date(surgeryDate)
            var timediff = surgeryDate - birthday;
            var years = surgeryDate.getFullYear() - birthday.getFullYear();
            var months = (surgeryDate.getFullYear() * 12 + surgeryDate.getMonth()) - (birthday.getFullYear() * 12 + birthday.getMonth());
            var days = Math.floor(timediff / day);
            var hours = Math.floor(timediff / hour);
            var minutes = Math.floor(timediff / minute);
            var seconds = Math.floor(timediff / second);
            var weeks = Math.floor(timediff / week);
            var age = ~~((Date.now() - birthday) / (31557600000));

            diff = new Date(
            surgeryDate.getFullYear() - birthday.getFullYear(),
            surgeryDate.getMonth() - birthday.getMonth(),
            surgeryDate.getDate() - birthday.getDate()
            );
            var ageInYears = diff.getYear();
            var ageInMonths = diff.getMonth();
            var ageInDays = diff.getDate();
            if (timediff == "0") {
                Totalyears += 1 + " day";
            }
            else {
                if (ageInYears > 0) {
                    Totalyears = ageInYears + " years ";
                }
                if (ageInMonths > 0) {
                    Totalyears += ageInMonths + " months ";
                }
                // uncomment below age in years
                // Faizan ameen Improvement EMR-1690
                // Dated: 25-oct-2016.

                if (ageInDays > 0) {
                    Totalyears += ageInDays + " days";
                }
            }
            // Totalyears = (years - 1) + " years";

        }
        if (surgeryDate != "")
            $('#' + Clinical_SurgicalHx.params.PanelID + '  section#sectionSurgicalDetails div#SurgicalDetails #txtAgeAtSurgery').val(Totalyears)
        $surgeryDateContext.blur(function () {
            if ($(this).val() == "")
                $('#' + Clinical_SurgicalHx.params.PanelID + '  section#sectionSurgicalDetails div#SurgicalDetails #txtAgeAtSurgery').val("");
        });
    },
    // compare surgerdate method will call calculateAgeAtSurgery() method if surgery date will be greater
    // Faizan ameen.
    // dated 20-oct-2016
    CompareSurgeryDatewithDateofBirth: function () {
        var $surgeryDateContext = $('#' + Clinical_SurgicalHx.params.PanelID + '  section#sectionSurgicalDetails div#SurgicalDetails #dtpSurgicalSurgeryDate');
        var surgeryDate = $($surgeryDateContext).datepicker('getDate'); //new Date($surgeryDateContext.val()).getDate();
        var birthday = $('#PatientProfile #hfPatientDOB').datepicker('getDate'); //new Date($('#PatientProfile #hfPatientDOB').val()).getDate();
        if ((birthday - surgeryDate) > 0) {
            // $surgeryDateContext.blur(function () {
            $('#' + Clinical_SurgicalHx.params.PanelID + '  section#sectionSurgicalDetails div#SurgicalDetails #txtAgeAtSurgery').val("");
            $('#' + Clinical_SurgicalHx.params.PanelID + '  section#sectionSurgicalDetails div#SurgicalDetails #dtpSurgicalSurgeryDate').val("");
            utility.DisplayMessages("Surgery Date must be greater than or equal to date of birth", 2);
            //  });

        }
        else {
            Clinical_SurgicalHx.calculateAgeAtSurgery();
        }
    },


    //Author: Muhammad Arshad
    //Date: 19-01-2016
    //This function will handle autocomplete for Ordering/Performing Provider
    loadAllAutocomplete: function () {
        CacheManager.BindCodesWithEntityId('GetProviderEntityBased', false, globalAppdata["SeletedEntityId"]).done(function (result) {
            var Ctrl = $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalPerformingProvider");
            var hfCtrl = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalPerformingProviderId");
            var onSelect = function (e) { Ctrl.attr("ProviderId", e.id); };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", EntityProviders, null, hfCtrl, onSelect);
        });

        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOrderingProvider");
            var hfCtrl = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalOrderingProviderId");
            var onSelect = function (e) { Ctrl.attr("ProviderId", e.id); };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });
    },

    //Author: Muhammad Arshad
    //Date: 19-01-2016
    //This function will handle provider search screen
    openProvider: function (isPerformingOrder) {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmClinicalSurgicalHx";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "clinicalTabSurgicalHx";
        if (typeof Clinical_HistorySummary != typeof undefined && Clinical_HistorySummary != null && Clinical_HistorySummary.params.ParentCtrl == "clinicalTabProgressNote") {
            params["ParentCtrl"] = "Clinical_HistorySummary";
        }
        else {
            params["ParentCtrl"] = "clinicalTabSurgicalHx";
        }
        var RefCtrl = "";
        var RefCtrlHidden = "";
        var RefCtrlLabel = "";
        var RefCtrlLink = "";
        var RefTitle = "";
        if (isPerformingOrder == true) {
            RefCtrl = "txtSurgicalPerformingProvider";
            RefCtrlHidden = "hfSurgicalPerformingProviderId";
            RefCtrlLabel = "lblSurgicalPerformingProviderProvider";
            RefCtrlLink = "lnkSurgicalPerformingProviderEdit";
            RefTitle = "Performing Provider";
            params["OnlyEntity"] = true;

        }
        else {
            RefCtrl = "txtSurgicalOrderingProvider";
            RefCtrlHidden = "hfSurgicalOrderingProviderId";
            RefCtrlLabel = "lblSurgicalOrderingProviderProvider";
            RefCtrlLink = "lnkSurgicalOrderingProviderEdit";
            RefTitle = "Ordering Provider";
            params["OnlyEntity"] = false;
        }

        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["Title"] = RefTitle;
        LoadActionPan('Admin_Provider', params);
    },

    //Author: Muhammad Arshad
    //Date: 19-01-2016
    //This function will handle provider edit screen
    openProviderDetail: function (isPerformingOrder) {
        //Admin_Provider.ProviderEdit($('#pnlDemographic #hfProvider').val(),'patTabDemographic');
        var params = [];

        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        var RefCtrl = "";
        var RefCtrlHidden = "";
        var RefCtrlLabel = "";
        var RefCtrlLink = "";
        var RefTitle = "";
        if (isPerformingOrder == true) {
            params["ProviderId"] = $('#' + Clinical_SurgicalHx.params.PanelID + ' #hfSurgicalPerformingProviderId').val();
            RefCtrl = "txtSurgicalPerformingProvider";
            RefCtrlHidden = "hfSurgicalPerformingProviderId";
            RefCtrlLabel = "lblSurgicalPerformingProviderProvider";
            RefCtrlLink = "lnkSurgicalPerformingProviderEdit";
            RefTitle = "Performing Provider";
        }
        else {
            params["ProviderId"] = $('#' + Clinical_SurgicalHx.params.PanelID + ' #hfSurgicalOrderingProviderId').val();
            RefCtrl = "txtSurgicalOrderingProvider";
            RefCtrlHidden = "hfSurgicalOrderingProviderId";
            RefCtrlLabel = "lblSurgicalOrderingProviderProvider";
            RefCtrlLink = "lnkSurgicalOrderingProviderEdit";
            RefTitle = "Ordering Provider";
        }

        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["Title"] = RefTitle;
        params["ParentCtrl"] = 'clinicalTabSurgicalHx';
        LoadActionPan('providerDetail', params);
    },

    ChangeCurrentPast: function (obj, PrimaryID, PageNumber, ResultPerPage) {

        if ($(obj).attr('status') == '1' || obj == 1) {
            $(obj).attr('status', 0);
            $('#' + Clinical_SurgicalHx.params.PanelID + " #pnlCurrent ").addClass("hidden");
            $('#' + Clinical_SurgicalHx.params.PanelID + " #pnlPast ").removeClass("hidden");

            Clinical_SurgicalHx.fillhxLog(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                if (response != "") {
                    var json = JSON.parse(response);
                    Clinical_SurgicalHx.gridLoad(response);
                    var TableControl = Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvPastSurgicalHx";
                    var PagingPanelControlID = Clinical_SurgicalHx.params.PanelID + " #dgvPastSurgicalHx_Paging";
                    var ClassControlName = "Clinical_SurgicalHx";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = json.iTotalDisplayRecords;
                    setTimeout(
                        CreatePagination(json.HxLogSoapCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_SurgicalHx.ChangeCurrentPast(1, PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                }
            });


        } else {
            $(obj).attr('status', 1);

            $('#' + Clinical_SurgicalHx.params.PanelID + " #pnlPast").addClass("hidden");
            $('#' + Clinical_SurgicalHx.params.PanelID + " #pnlCurrent  ").removeClass("hidden");
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
        objData["HxId"] = Clinical_SurgicalHx.params.HxTypeId;
        objData["HxType"] = "SurgicalHx";
        objData["Status"] = "All";
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = ResultPerPage;
        objData["commandType"] = "get_hx_log";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");
    },
    //Author: Abid Ali
    //Date: 27-01-2016
    //This function will handle fill of SurgicalHx and it's childs as specified by SurgicalHxType
    loadSurgicalHxComponent: function (surgicalHxType, diseaseId, isDiseaseFill, isRefreshHistoryGrid) {

        Clinical_SurgicalHx.fillSurgicalHx(surgicalHxType, diseaseId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    var DiseaseLoadDetails = JSON.parse(response.surgicalHxDiseaseLoad_JSON);
                    var Surgicalhx_detail = JSON.parse(response.SurgicalHxFill_JSON);


                    if (Surgicalhx_detail.SurgicalHxId > 0) {

                        Clinical_SurgicalHx.params.HxTypeId = Surgicalhx_detail.SurgicalHxId;

                        if (Surgicalhx_detail != "" && Surgicalhx_detail != null) {


                            if (isRefreshHistoryGrid == null) {

                                var $row = $('<tr/>');
                                if (DiseaseLoadDetails.length > 0) {
                                    $row.append('<td style="display:none;">' + Surgicalhx_detail.SurgicalHxId + '</td><td>' + Surgicalhx_detail.IsCreatedOrModified + '</td><td>' + Surgicalhx_detail.SurgicalHxSoapText + '</td><td>' + Surgicalhx_detail.LastUpdated + '</td>');
                                }
                                else {
                                    $row.append('<td>&nbsp;</td><td>No Known Surgical History</td><td></td>');
                                    //$row.append('<td valign="top" colspan="4" class="dataTables_empty">No Known Surgical History</td>');
                                }
                                $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvSurgicalHx tbody").html($row);
                            }
                        }


                        if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            $('#' + Clinical_SurgicalHx.params.PanelID + " #dtpSurgicalHxDate").removeClass('disableAll');
                        }
                        else {
                            $('#' + Clinical_SurgicalHx.params.PanelID + " #dtpSurgicalHxDate").addClass('disableAll');
                        }

                        $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #aHistory").removeClass('hidden');
                        $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #aHistory").attr('onclick', 'Clinical_SurgicalHx.showSurgicalHxHistory(' + Surgicalhx_detail.SurgicalHxId + ')');
                    }
                    else {
                        var $row = $('<tr/>');
                        $row.append('<td style="display:none;"></td><td>&nbsp;</td><td>No Known Surgical History</td><td></td>');
                        $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvSurgicalHx tbody").html($row);
                        $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #divSwitch").addClass('disableAll');

                    }
                    var self = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx");

                    if (isDiseaseFill != true) {
                        utility.bindMyJSONByName(true, Surgicalhx_detail, false, self).done(function () {
                            //Start//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225
                            var upperDate = self.find('input[name*="SurgicalHxDate"]');

                            //Start//02/02/2016//Abid Ali// For bug# EMR-497
                            $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val(Surgicalhx_detail.SurgicalHxId);
                            //End//02/02/2016//Abid Ali// For bug# EMR-497

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
                            Clinical_SurgicalHx.surgicalDate = $('#' + Clinical_SurgicalHx.params.PanelID + " #dtpSurgicalHxDate").val();

                            if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
                                $('#' + Clinical_SurgicalHx.params.PanelID + " #dtpSurgicalHxDate").val(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxDate);
                            }

                            if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null && Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxComments != '') {
                                $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOverallComments").val(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxComments);
                            }
                            else {
                                $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOverallComments").val(Surgicalhx_detail.SurgicalHxComments);
                                Clinical_SurgicalHx.overallComments = $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOverallComments").val();
                            }

                            Clinical_SurgicalHx.unremarkable = Surgicalhx_detail.SurgicalHxUnremarkable == "False" || Surgicalhx_detail.SurgicalHxUnremarkable == null ? false : true;;

                            if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
                                $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop("checked", Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxUnremarkable);
                            }
                        });
                        Clinical_SurgicalHx.resetValues();
                        Clinical_SurgicalHx.loadSurgicalHxDiseases('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx div#Surgical #ulSurgicalDisease", DiseaseLoadDetails, "disease");

                        if ($('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #ulSurgicalDisease li").length > 0) {
                            //var selectedDisease = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSelectedDisease").val();
                            //if (selectedDisease != "" && selectedDisease != "undefined" && selectedDisease > 0) {
                            //    var list = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #ulSurgicalDisease li#" + selectedDisease);
                            //    $(list).addClass('active');
                            //    $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #sectionSurgicalDetails").removeClass("disableAll");
                            //    list.trigger('click');
                            //    return;
                            //}
                        }
                        $('#' + Clinical_SurgicalHx.params.PanelID + ' #sectionSurgicalDetails').addClass('disableAll');
                        if (Surgicalhx_detail.length == 0) {
                            if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
                                $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxId);
                                $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtpSurgicalHxDate").val(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxDate);
                                $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop("checked", Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxUnremarkable);
                                $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOverallComments").val(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxComments);
                                var upperDate = self.find('input[name*="SurgicalHxDate"]');
                                if (upperDate.val() == "") {
                                    upperDate.datepicker('setDate', new Date());
                                } else {
                                    var date_format = 'mm/dd/yyyy';
                                    if (globalAppdata['DateFormat']) {
                                        date_format = globalAppdata['DateFormat'];
                                    }
                                    upperDate.datepicker({ date_format: date_format.replace('yy', '') }).val(upperDate.val());
                                    upperDate.datepicker("setDate", upperDate.val());
                                }
                            }
                        }
                    }
                    Clinical_SurgicalHx.params.mode = "Edit";

                    if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {

                        // $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtpSurgicalHxDate").addClass("disableAll");
                    }
                    if ($('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop("checked") == false) {
                        Surgicalhx_detail.SurgicalHxUnremarkable = 'false';
                    }
                    if (Surgicalhx_detail.SurgicalHxUnremarkable != null && typeof Surgicalhx_detail.SurgicalHxUnremarkable !== "undefined") {
                        if (Surgicalhx_detail.SurgicalHxUnremarkable.toLowerCase() != "true") {

                            if (surgicalHxType.toLowerCase() == "disease") {
                                if (isDiseaseFill == true) {

                                    if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                        var cachedJSON = Clinical_SurgicalHx.getCacheSurgicalHxJSON(diseaseId);
                                        if (cachedJSON != '') {
                                            Clinical_SurgicalHx.bindCurrentTabJSON(surgicalHxType.toLowerCase(), cachedJSON, "#frmClinicalSurgicalHx", "#ulSurgicalDisease");
                                        }
                                        else {
                                            Clinical_SurgicalHx.bindCurrentTabJSON(surgicalHxType.toLowerCase(), response.surgicalHxDiseaseFill_JSON, "#frmClinicalSurgicalHx", "#ulSurgicalDisease");
                                        }
                                    }
                                    else {
                                        Clinical_SurgicalHx.bindCurrentTabJSON(surgicalHxType.toLowerCase(), response.surgicalHxDiseaseFill_JSON, "#frmClinicalSurgicalHx", "#ulSurgicalDisease");
                                    }

                                }
                                else {
                                    if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
                                        $.each(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList, function (i, item) {
                                            Clinical_SurgicalHx.bindCurrentTabJSON(surgicalHxType.toLowerCase(), item.JSON, "#frmClinicalSurgicalHx", "#ulSurgicalDisease")
                                        });
                                    }



                                }
                            }

                        }
                        else {
                            if (isDiseaseFill != true) {
                                var chkUnremarkable = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable")
                                Clinical_SurgicalHx.unRemarkableSurgicalHx(chkUnremarkable, "1");

                            }
                        }
                    }
                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').data('serialize', $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').serialize());
                    //if (diseaseId > 0) {
                    //    $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').data('serialize', $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').serialize());
                    //}
                    if (isDiseaseFill == true && Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        Clinical_SurgicalHx.surgicalHxJSON = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").getMyJSONByName();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {

            }

        });

    },
    cacheSurgicalHxJSON: function (diseaseId, diseaseData, diseaseLi) {
        // if (EMRUtility.compareFormDataWithSerialized(Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx') || diseaseId != null) {


        var dfd = $.Deferred();
        var diseaseIndex = -1;
        var patientId = Clinical_SurgicalHx.params.patientID || $('#PatientProfile #hfPatientId').val();

        var FavListVal = $('#' + Clinical_SurgicalHx.params.PanelID + ' #ddlFavoriteListSurgicalHx').val()
        var isFavListOpened = $('#' + Clinical_SurgicalHx.params.PanelID + " #favSectionDiv").hasClass("toggledHor");


        if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx == null) {
            //Clinical_HistorySummary.HistoryCacheList.SurgicalHx = {};

            var surgicalHxData = {
                SurgicalHxId: $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val(),
                SurgicalHxType: 'Disease',
                SurgicalHxDate: $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtpSurgicalHxDate").val(),
                SurgicalHxUnremarkable: $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop("checked"),
                SurgicalHxComments: $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOverallComments").val(),
                PatientId: patientId,
                SurgicalDiseaseList: [],
                FavListVal: FavListVal,
                isFavListOpened: isFavListOpened,
                FavListName: Clinical_SurgicalHx.FavListName,
            }
            Clinical_HistorySummary.HistoryCacheList.SurgicalHx = surgicalHxData;
        }
        else {
            Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxDate = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtpSurgicalHxDate").val();
            Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxUnremarkable = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop("checked");
            Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalHxComments = $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOverallComments").val();
            Clinical_HistorySummary.HistoryCacheList.SurgicalHx.FavListVal = FavListVal;
            Clinical_HistorySummary.HistoryCacheList.SurgicalHx.isFavListOpened = isFavListOpened;
            Clinical_HistorySummary.HistoryCacheList.SurgicalHx.FavListName = Clinical_SurgicalHx.FavListName;
        }

        //var diseaseIndex = -1;
        //var patientId = Clinical_SurgicalHx.params.patientID || $('#PatientProfile #hfPatientId').val();

        //var surgicalHxData = {
        //    SurgicalHxId: $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val(),
        //    SurgicalHxType: 'Disease',
        //    SurgicalHxDate: $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtpSurgicalHxDate").val(),
        //    SurgicalHxUnremarkable: $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop("checked"),
        //    SurgicalHxComments: $('#' + Clinical_SurgicalHx.params.PanelID + " #txtSurgicalOverallComments").val(),
        //    PatientId: patientId,
        //    SurgicalDiseaseList: []
        //}

        //if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx == null) {
        //    Clinical_HistorySummary.HistoryCacheList.SurgicalHx = {};
        //}

        //Clinical_HistorySummary.HistoryCacheList.SurgicalHx = surgicalHxData;

        if (diseaseId != null) {

            $.grep(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList, function (item, index) {
                if (item.DiseaseId == diseaseId) {
                    diseaseIndex = index;
                    return;
                }
            });

            var diseaseObj = JSON.parse(diseaseData);

            if (diseaseIndex != -1) {

                var selectedDisease = $('#' + Clinical_SurgicalHx.params.PanelID + " ul#ulSurgicalDisease li#" + diseaseId);
                var FreeText = selectedDisease.attr("FreeText") || selectedDisease.text();

                $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #hfSelectedDisease").val(selectedDisease.attr("id"));


                var diseaseCacheObj = Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList[diseaseIndex];


                diseaseCacheObj.SurgicalHxId = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val();
                diseaseCacheObj.SurgicalHxDate = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtpSurgicalHxDate").val();
                diseaseCacheObj.DiseaseId = diseaseId;
                diseaseCacheObj.SurgicalHxUnremarkable = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop("checked");
                diseaseCacheObj.SurgicalHxType = 'Disease';
                diseaseCacheObj.Disease = $('#' + Clinical_SurgicalHx.params.PanelID + " #txtDisease").val();
                diseaseCacheObj.patientId = Clinical_SurgicalHx.params.patientID || $('#PatientProfile #hfPatientId').val();
                diseaseCacheObj.Disease_text = selectedDisease.text();
                //diseaseCacheObj.FreeTextICD = FreeText;


                //if ($('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #rdofreetext").prop("checked") == false) {

                if (selectedDisease.attr('cptdesc') && selectedDisease.attr('cptdesc') != '') {
                    diseaseCacheObj.CPTCodeId = selectedDisease.attr('cptCode');
                    diseaseCacheObj.CPTDescription = selectedDisease.attr('cptdesc');
                    diseaseCacheObj.CPTCode = selectedDisease.attr('cptCode');
                    diseaseCacheObj.CPTSNOMEDCodeId = selectedDisease.attr('snomedcode');
                    diseaseCacheObj.CPTSNOMEDDescription = selectedDisease.attr('snomeddesc');
                }
                else {
                    diseaseCacheObj.CPTCodeId = "";
                    diseaseCacheObj.CPTDescription = "";
                    diseaseCacheObj.CPTCode = "";
                    diseaseCacheObj.CPTSNOMEDCodeId = "";
                    diseaseCacheObj.CPTSNOMEDDescription = "";
                    diseaseCacheObj.FreeTextProcedure = selectedDisease.text();
                }
                diseaseCacheObj.SurgicalStatus = diseaseObj.SurgicalStatus;
                diseaseCacheObj.SurgicalStatusText = diseaseObj.SurgicalStatus_text;
                diseaseCacheObj.SurgicalLocation = diseaseObj.SurgicalLocation;
                diseaseCacheObj.SurgicalSurgeryDate = diseaseObj.SurgicalSurgeryDate;
                diseaseCacheObj.AgeAtSurgery = diseaseObj.AgeAtSurgery;
                diseaseCacheObj.SurgicalReason = diseaseObj.SurgicalReason;
                diseaseCacheObj.SurgicalOrderingProvider = diseaseObj.SurgicalOrderingProvider;
                diseaseCacheObj.SurgicalPerformingProvider = diseaseObj.SurgicalPerformingProvider;
                diseaseCacheObj.SurgicalDiseaseComments = diseaseObj.SurgicalComments;

                diseaseCacheObj.PerformingProviderId = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalPerformingProvider").val() == "" ? "" : $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalPerformingProvider").attr('providerid');
                diseaseCacheObj.OrderingProviderId = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalOrderingProvider").val() == "" ? "" : $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalOrderingProvider").attr('providerid');

                diseaseCacheObj.JSON = diseaseData;

                Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList[diseaseIndex] = diseaseCacheObj;
            }
            else {

                var selectedDisease = $('#' + Clinical_SurgicalHx.params.PanelID + " ul#ulSurgicalDisease li#" + diseaseId);
                var FreeText = selectedDisease.attr("FreeText") || selectedDisease.text();
                //var FreeTextAttr = selectedDisease.attr("FreeText") || selectedDisease.text();
                var IsFreeText = selectedDisease.attr('cptdesc') && selectedDisease.attr('cptdesc') != '' ? false : true;
                //$('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #rdofreetext").prop("checked");

                $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #hfSelectedDisease").val(selectedDisease.attr("id"));

                var SurDiseaseData = {
                    SurgicalHxId: $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val(),
                    SurgicalHxDate: $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtpSurgicalHxDate").val(),
                    DiseaseId: diseaseId,
                    SurgicalHxUnremarkable: $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop("checked"),
                    SurgicalHxType: 'Disease',
                    Disease: $('#' + Clinical_SurgicalHx.params.PanelID + " #txtDisease").val(),
                    patientId: Clinical_SurgicalHx.params.patientID == null ? $('#PatientProfile #hfPatientId').val() : Clinical_SurgicalHx.params.patientID,
                    Disease_text: selectedDisease.text(),
                    //FreeTextICD: FreeText,
                    CPTCodeId: IsFreeText == false ? selectedDisease.attr('cptCode') : "",
                    CPTDescription: IsFreeText == false ? selectedDisease.attr('cptdesc') : "",
                    CPTCode: IsFreeText == false ? selectedDisease.attr('cptCode') : "",
                    CPTSNOMEDCodeId: IsFreeText == false ? selectedDisease.attr('snomedcode') : "",
                    CPTSNOMEDDescription: IsFreeText == false ? selectedDisease.attr('snomeddesc') : "",
                    FreeTextProcedure: IsFreeText == false ? "" : selectedDisease.text(),
                    SurgicalStatus: diseaseObj.SurgicalStatus,
                    SurgicalStatusText: diseaseObj.SurgicalStatus_text,
                    SurgicalLocation: diseaseObj.SurgicalLocation,
                    SurgicalSurgeryDate: diseaseObj.SurgicalSurgeryDate,
                    AgeAtSurgery: diseaseObj.AgeAtSurgery,
                    SurgicalReason: diseaseObj.SurgicalReason,
                    SurgicalOrderingProvider: diseaseObj.SurgicalOrderingProvider,
                    SurgicalPerformingProvider: diseaseObj.SurgicalPerformingProvider,
                    SurgicalDiseaseComments: diseaseObj.SurgicalComments,
                    PerformingProviderId: $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalPerformingProvider").val() == "" ? "" : $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalPerformingProvider").attr('providerid'),
                    OrderingProviderId: $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalOrderingProvider").val() == "" ? "" : $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalOrderingProvider").attr('providerid'),
                    JSON: diseaseData,
                    DiseaseLi: diseaseLi,
                };

                Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList.push(SurDiseaseData);
            }

            dfd.resolve();
            return dfd;
        }

        //}
        //else {
        //    if (diseaseId != null && Clinical_HistorySummary.HistoryCacheList.SurgicalHx && Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList.length > 0) {

        //        Clinical_SurgicalHx.deleteCacheDisease(diseaseId)
        //    }


        //}
    },


    getCacheSurgicalHxJSON: function (diseaseId) {
        var surgicalHx = Clinical_HistorySummary.HistoryCacheList.SurgicalHx;

        if (surgicalHx && surgicalHx.SurgicalDiseaseList.length > 0) {
            var surgicalDisease = $.grep(surgicalHx.SurgicalDiseaseList, function (item, index) {
                if (item.DiseaseId == diseaseId) {
                    return item;
                }
            });

            if (surgicalDisease.length > 0) {
                return surgicalDisease[0].JSON;
            }
        }

        return '';
    },


    deleteCacheDisease: function (diseaseId) {
        var diseaseIndex = -1;
        if (Clinical_HistorySummary.HistoryCacheList.SurgicalHx) {
            $.grep(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList, function (item, index) {
                if (item.DiseaseId == diseaseId) {
                    diseaseIndex = index;
                    return;
                }
            });

            if (diseaseIndex != -1) {
                Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList.splice(diseaseIndex, 1);
            }
        }
    },
    //Author: Abid Ali
    //Date: 27-01-2016
    //This function will handle load of SurgicalHx disease
    loadSurgicalHxDiseases: function (crtl, result, statusType) {
        var currentLiClass = "";
        var currentLiClick = "";
        var parentDiv = "";
        if (statusType != null && statusType.toLowerCase() == "disease") {
            crtl = '#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx div#Surgical #ulSurgicalDisease";
            parentDiv = "Surgical";
        }
        if ($(crtl).length > 0)
            l = $(crtl);

        l.empty();
        var isFirstLi = true;
        $.each(result, function (j, item) {
            var li = "";

            if (item.FreeTextCPT != "") {
                li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_SurgicalHx.fillSurgicalHxDisease(this, event);' ><a href='#'>" + item.FreeTextCPT + "<span class='removeIconListHover' onclick='Clinical_SurgicalHx.deleteSurgicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></li>";
            } else {

                li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_SurgicalHx.fillSurgicalHxDisease(this, event);' cptCode=\"" + item.CPTCode + "\" cptDesc=\"" + item.CPTCodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\"><a href='#'>" + item.CPTCodeDescription + "<span class='removeIconListHover' onclick='Clinical_SurgicalHx.deleteSurgicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></li>";
            }
            l.append(li);
        });
        if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
            $(Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList).each(function () {
                if ($(this)[0].DiseaseLi != null || $(this)[0].DiseaseLi != undefined) {
                    l.append($(this)[0].DiseaseLi);
                }
            });
        }
        var previouslySelectedDisease = Clinical_SurgicalHx.currentSelectedDisease["DiseaseId"];
        $("#" + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx div#Surgical #ulSurgicalDisease li#" + previouslySelectedDisease).trigger('click');

    },

    //Author: Abid Ali
    //Date: 27-01-2016
    //This function will handle fill of SurgicalHx disease
    fillSurgicalHxDisease: function (obj, ev) {

        ev.stopPropagation();
        if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SurgicalHx.surgicalHxJSON != '') {
            if (Clinical_SurgicalHx.surgicalHxJSON != $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").getMyJSONByName()) {
                var diseaseId = $('#' + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease > li.active").attr('id');
                var diseaseData = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").getMyJSONByName();
                Clinical_SurgicalHx.cacheSurgicalHxJSON(diseaseId, diseaseData);
            }
        }
        Clinical_SurgicalHx.currentSelectedDisease["DiseaseId"] = $(obj).attr('id');

        $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnAddVitalsOnNote').prop('disabled', false);
        $("#" + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx").bootstrapValidator('resetForm', true);
        var diseaseId = $(obj).attr('id');

        Clinical_SurgicalHx.resetValues();
        $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnSurgicalHxSave').hide();
        $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #sectionSurgicalDetails").removeClass('disableAll');

        $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #Surgical ul#ulSurgicalDisease li").each(function (i, item) {
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

        Clinical_SurgicalHx.loadSurgicalHxComponent('disease', diseaseId, true, false);

    },

    /*
    Author: Muhammad Arshad
    Date: 28/01/2016
    Overview: This function delete the selected li from disease
    */
    deleteSurgicalHxDisease: function (obj, ev) {
        var dfd = new $.Deferred();
        ev.stopPropagation();
        var diseaseId = $(obj).attr('id');
        if (diseaseId < 0) {
            utility.myConfirm('23', function () {
                Clinical_SurgicalHx.enableFavoriteList($(obj));

                $(obj).remove();
                if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnAddVitalsOnNote').attr("disabled", true);
                    Clinical_SurgicalHx.deleteCacheDisease(diseaseId);
                }
                //Start//07-03-2016//Ahmad Raza// fixed EMR Bug #435
                $('#' + Clinical_SurgicalHx.params.PanelID + ' #sectionSurgicalDetails').resetAllControls(null);

                //End//07-03-2016//Ahmad Raza// fixed EMR Bug #435

                // added by faizan ameeen QAC2-458
                $("#" + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx").bootstrapValidator('resetForm', true);

                // End of  added by faizan ameeen QAC2-458
                dfd.resolve();

            }, function () { },
                        '23'
                    );

        } else {
            AppPrivileges.GetFormPrivileges("History_Surgical Hx", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('23', function () {
                        var selectedValue = diseaseId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Clinical_SurgicalHx.surgicalHxDiseaseDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_SurgicalHx.enableFavoriteList($(obj));
                                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #sectionSurgicalDetails').resetAllControls(null);
                                    if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                        $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnAddVitalsOnNote').attr("disabled", true);
                                        Clinical_SurgicalHx.deleteCacheDisease(diseaseId);
                                    }
                                    $(obj).remove();
                                    // added by faizan ameeen QAC2-458
                                    $("#" + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx").bootstrapValidator('resetForm', true);

                                    // End of  added by faizan ameeen QAC2-458
                                    dfd.resolve();
                                    utility.DisplayMessages(response.Message, 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }

                                Clinical_SurgicalHx.loadSurgicalHx("disease", true);
                            });
                        }
                    }, function () { },
                        '23'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

        dfd.done(function () {
            //var $diseases = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #Surgical ul#ulSurgicalDisease");
            // if (!$diseases.has('li').length)
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #sectionSurgicalDetails").addClass('disableAll');
            $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSelectedDisease").val("");
        });
    },



    ///Author: Farooq Ahmad
    //Date: 27-01-2016
    //Logic to popup search from on procedure field of Surgicalhx disease
    openSearchPopup: function (searchType, ctrl, hiddenCtrl) {
        var controlToLoad = "";
        if (searchType == "ICD") {
            controlToLoad = "Admin_IMOICD";
        }
        else if (searchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (searchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtDisease").attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        if (Clinical_SurgicalHx.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_SurgicalHx';
        }

        else {
            params["ParentCtrl"] = Clinical_SurgicalHx.params["TabID"];
        }

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        if (hiddenCtrl != null) {
            params["RefHiddenCtrl"] = hiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Clinical_SurgicalHx.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params, Clinical_SurgicalHx.params.PanelID);
        }

    },

    ///Author: Farooq Ahmad
    //Date: 27-01-2016
    //Logic to show delete icon
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },
    ///Author: Farooq Ahmad
    //Date: 27-01-2016
    //Logic to hide delete icon
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },

    ///Author: syed zia
    //Date: 22-01-2016
    //Logic implimented to bind CPT code to Procedure field of SurgicalHx
    bindAutoComplete: function (element) {

        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_SurgicalHx", null, true);

    },

    ///Author: syed zia
    //Date: 22-01-2016
    //Logic implimented to bind CPT code to Procedure field of SurgicalHx
    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_SurgicalHx";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Clinical_SurgicalHx.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_SurgicalHx.params.PanelID);
    },

    ///Author: syed zia
    //Date: 22-01-2016
    //Logic implimented to bind ICD code of SurgicalHx
    bindICD9AutoComplete: function (element) {
        var descriptionCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_SurgicalHx", null, false);
    },

    ///Author: syed zia
    //Date: 22-01-2016
    //Logic to reset control values
    resetValues: function () {

        $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #sectionSurgicalDetails").find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
            $(this).val('');
        });

    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle fill of SurgicalHx and it's childs as specified by SurgicalHxType
    loadSurgicalHx: function (surgicalHxType, isLoadNew) {

        surgicalHxType = typeof surgicalHxType != "undefined" ? surgicalHxType : "";
        var dataExists = Clinical_SurgicalHx.isDetailExists(surgicalHxType.toLowerCase());

        if (isLoadNew == true) {
            Clinical_SurgicalHx.loadSurgicalHxComponent(surgicalHxType);
            $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxType").val(surgicalHxType);
        }
        else {
            if (dataExists != false) {
                if (EMRUtility.compareFormDataWithSerialized(Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx')) {

                    utility.myConfirm('12', function () {
                        var surgicalHxTypeCurrent = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxType").val();
                        Clinical_SurgicalHx.surgicalHxSave(surgicalHxTypeCurrent, false);

                        $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxType").val(surgicalHxType);
                        Clinical_SurgicalHx.loadSurgicalHxComponent(surgicalHxType);
                        BackgroundLoaderShow(true);

                    }, function () {
                        Clinical_SurgicalHx.loadSurgicalHxComponent(surgicalHxType);
                    });

                }
                else {
                    Clinical_SurgicalHx.loadSurgicalHxComponent(surgicalHxType);
                    $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxType").val(surgicalHxType);
                }
            }
            else {
                $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxType").val(surgicalHxType);
                Clinical_SurgicalHx.loadSurgicalHxComponent(surgicalHxType);
            }
        }

    },

    //Author: Abid Ali
    //Date: 21-01-2016
    //This function will handle bindJasonWithForm feature for Surgical Hx
    bindCurrentTabJSON: function (tabType, currentTabJSON, currentTabContainerId, ulTabStatusId) {
        var surgicalhx_detail = JSON.parse(currentTabJSON);
        self = $('#' + Clinical_SurgicalHx.params.PanelID + " " + currentTabContainerId);

        utility.bindMyJSONByName(true, surgicalhx_detail, false, self).done(function () {

            //Start//11-02-2016//Ahmad Raza//fixed EMR Bug#316
            if (surgicalhx_detail != null && surgicalhx_detail.SurgicalSurgeryDate != null && surgicalhx_detail.SurgicalSurgeryDate != "") {
                var surgeryDate = self.find('input[name*="SurgicalSurgeryDate"]');
                if (surgeryDate.val() == "") {
                    surgeryDate.datepicker('setDate', new Date());
                } else {
                    var dateformat = 'mm/dd/yyyy';
                    if (globalAppdata['DateFormat'])
                        dateformat = globalAppdata['DateFormat'];
                    surgeryDate.datepicker({ dateformat: dateformat.replace('yy', '') }).val(surgeryDate.val());
                    surgeryDate.datepicker("setDate", surgeryDate.val());
                }
            }
            //End//11-02-2016//Ahmad Raza//fixed EMR Bug#316

            //Start//14/01/2016//Ahmad Raza//setting values in hidden field when existing disease is selected
            $('#' + Clinical_SurgicalHx.params.PanelID + " #hfCPTCode").val(surgicalhx_detail.CPTCodeId);
            $('#' + Clinical_SurgicalHx.params.PanelID + " #hfCPTDescription").val(surgicalhx_detail.CPTDescription);

            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalPerformingProvider").attr('providerid', surgicalhx_detail.SurgicalPerformingProviderId);
            $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalOrderingProvider").attr('providerid', surgicalhx_detail.SurgicalOrderingProviderId);

            //End//14/01/2016//Ahmad Raza//setting values in hidden field when existing disease is selected
            BackgroundLoaderShow(false);
        });


        // });
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle unremarkable feature for Surgical Hx
    unRemarkableSurgicalHx: function (obj, isFromLoad) {
        var isRemarkable = $(obj).prop("checked");
        if (isRemarkable == true) {

            $('#' + Clinical_SurgicalHx.params.PanelID + ' #sectionSurgicalDetails').resetAllControls(null);
            if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnSurgicalHxSave').hide();
            }
            else {
                $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnSurgicalHxSave').show();
            }
            $('#' + Clinical_SurgicalHx.params.PanelID + " div#Surgical ul#ulSurgicalDisease").empty();
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #Surgical').addClass('disableAll');
            Clinical_SurgicalHx.enableDisbaleUlItems('ulFavoriteListSurgicalHxContent', false);
            if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SurgicalHx != null) {
                Clinical_HistorySummary.HistoryCacheList.SurgicalHx.SurgicalDiseaseList = [];
            }
        }
        else {
            /* Start 27/01/2016 Abid Ali for bug*/
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #btnSurgicalHxSave').hide();
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #Surgical').removeClass('disableAll');
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #sectionSurgicalDetails').addClass('disableAll');
            /* End 27/01/2016 Abid Ali for bug*/

            Clinical_SurgicalHx.enableDisbaleUlItems('ulFavoriteListSurgicalHxContent', true);
        }
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will clear value of given control as specified by obj
    resetControlValue: function (obj) {
        var currentElementTagName = obj.tagName != null ? obj.tagName : obj.prop("tagName");
        if ($(obj).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea')
            $(obj).val('');
        if ($(obj).attr('type') == 'checkbox' || $(obj).attr('type') == 'radio') {

            if ($(obj).attr('type') == 'radio') {
                obj.checked = false;
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Surgical History Clinical Module -> Fields should be blank when select other status
                var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                if (groupRadBtn.length > 1) {
                    $.each(groupRadBtn, function (i, item) {
                        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                            $(item).trigger("click");
                        }
                    });
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Surgical History Clinical Module -> Fields should be blank when select other status
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
    //Date: 12-03-2015
    //This function will check whether value exists in any control of the form or not
    isDetailExists: function (TabType) {
        var detailExists = false;
        var sectionDetails = "";
        if (TabType != null && TabType != "") {
            if (TabType.toLowerCase() == "disease") {
                sectionDetails = "sectionSurgicalDetails";
            }
        }
        if (sectionDetails != "") {
            var self = $('#' + Clinical_SurgicalHx.params.PanelID + ' section#' + sectionDetails).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if ($(this).prop("disabled") != true) {
                    var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                    if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                        detailExists = true;
                    }
                    if ($(this).attr('type') == 'checkbox' && this.checked == true) {
                        detailExists = true;
                    }
                    if ($(this).attr('type') == 'radio' && $(this).attr('id').toLowerCase().indexOf("yes") > -1 && this.checked == true) {
                        detailExists = true;
                    }
                    if (currentElementTagName.toLowerCase() == 'select' && $(this).val() != null && $(this).val() != "") {
                        detailExists = true;
                    }

                }
            });
        }

        return detailExists;

    },


    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Add/Edit of SurgicalHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous), it expects SurgicalHxType to be Add/Edit
    surgicalHxSave: function (surgicalHxType, unloadSurgicalhx, attachToNote) {

        //Start//17-02-2016//Ahmad Raza//Fixed Bug#340
        if (!$('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").is(':checked')) {
            // $("#" + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx").bootstrapValidator('revalidateField', 'CPTCode');
        }
        // if ($("#" + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalCPTCode").val() != "" || $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").is(':checked')) {
        //End//17-02-2016//Ahmad Raza//Fixed Bug#340
        var surgicalHxId = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val() != "" ? $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val() : "-1";
        if (parseInt(surgicalHxId) > 0) {
            Clinical_SurgicalHx.params.mode = "Edit";
        }
        else {
            Clinical_SurgicalHx.params.mode = "Add";
        }

        //Start//11/02/2016//Abid Ali// fixed bug#315
        //Start//26/12/2016//Zain ul abdin// IMP-112
        //var overallComments = "";
        //overallComments = $('#' + Clinical_SurgicalHx.params.PanelID + " #SurgicalOverallComments").val();
        //overallComments = typeof overallComments == "undefined" ? "" : overallComments;
        //if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote" && overallComments != "") {

        //    var detailExists = true;
        //}
        //else {
        //    detailExists = Clinical_SurgicalHx.isDetailExists(surgicalHxType.toLowerCase());
        //}
        //if ($('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").is(':checked')) {
        //    detailExists = true;
        //}
        //if (overallComments != "") {
        //    detailExists = true;
        //}
        //End//26/12/2016//Zain ul abdin// IMP-112
        //End//11/02/2016//Abid Ali// fixed bug#315

        var detailExists = true;
        if (detailExists == true) {

            var strMessage = "";
            var self = null;
            if (surgicalHxType.toLowerCase() == "disease") {
                self = $('#' + Clinical_SurgicalHx.params.PanelID + " section#sectionSurgicalDetails");
            }
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);
            if (surgicalHxType.toLowerCase() == "disease") {
                var selectedDisease = $('#' + Clinical_SurgicalHx.params.PanelID + " div#Surgical ul#ulSurgicalDisease li.active");
                objData["DiseaseId"] = selectedDisease.attr("id");
                $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSelectedDisease").val(objData["DiseaseId"]);
                objData["Disease_text"] = selectedDisease.text();

                if ($('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #rdofreetext").prop("checked") == false) {
                    //objData["ICD9Code"] = selectedDisease.attr("icd9code");
                    //objData["ICD9CodeDescription"] = selectedDisease.attr("icd9desc");
                    //objData["ICD10Code"] = selectedDisease.attr("icd10code");
                    //objData["ICD10CodeDescription"] = selectedDisease.attr("icd10desc");
                    //objData["SNOMEDID"] = selectedDisease.attr("snomedcode");
                    //objData["SNOMEDDescription"] = selectedDisease.attr("snomeddesc");
                    objData["CPTCodeId"] = selectedDisease.attr('cptCode');
                    objData["CPTCode"] = objData["CPTCodeId"];
                    objData["CPTDescription"] = selectedDisease.attr('cptdesc');

                    objData["CPTSNOMEDCodeId"] = selectedDisease.attr('snomedcode');
                    objData["CPTSNOMEDDescription"] = selectedDisease.attr('snomeddesc');
                    //-------------------
                } else {
                    objData["FreeTextProcedure"] = selectedDisease.text();

                    objData["CPTCodeId"] = "";
                    objData["CPTCode"] = "";
                    objData["CPTDescription"] = "";

                    objData["CPTSNOMEDCodeId"] = "";
                    objData["CPTSNOMEDDescription"] = "";
                }
                objData["SurgicalStatus"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #ddlSurgicalStatus").val();
                objData["SurgicalStatusText"] = ($("#" + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #ddlSurgicalStatus option:selected").text() == "- Select -" ? "" : $("#" + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #ddlSurgicalStatus option:selected").text());
                objData["SurgicalLocation"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalLocation").val();
                objData["SurgicalSurgeryDate"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtpSurgicalSurgeryDate").val();
                objData["AgeAtSurgery"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtAgeAtSurgery").val();
                objData["SurgicalReason"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalReason").val();
                objData["SurgicalOrderingProvider"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalOrderingProvider").val();
                objData["SurgicalPerformingProvider"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalPerformingProvider").val();
                objData["SurgicalDiseaseComments"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalComments").val();

                if ($('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalPerformingProvider").val() == "") {
                    objData["PerformingProviderId"] = "";
                } else {
                    objData["PerformingProviderId"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalPerformingProvider").attr('providerid');
                }
                if ($('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalOrderingProvider").val() == "") {
                    objData["OrderingProviderId"] = "";
                } else {
                    objData["OrderingProviderId"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #txtSurgicalOrderingProvider").attr('providerid');
                }

                //objData["PerformingProviderId"] = $('#' + Clinical_SurgicalHx.params.PanelID + ' #hfSurgicalPerformingProviderId').val();
                //objData["OrderingProviderId"] = $('#' + Clinical_SurgicalHx.params.PanelID + ' #hfSurgicalOrderingProviderId').val();
            }

            objData["SurgicalHxId"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val();
            objData["SurgicalHxType"] = surgicalHxType != null ? surgicalHxType : "";
            objData["SurgicalHxDate"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #dtpSurgicalHxDate").val();
            objData["SurgicalHxUnremarkable"] = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalSurgicalHx #chkSurgicalHxUnremarkable").prop("checked");
            objData["SurgicalHxComments"] = $('#' + Clinical_SurgicalHx.params.PanelID + ' #txtSurgicalOverallComments').val();
            myJSON = JSON.stringify(objData);
            //return false;
            if (Clinical_SurgicalHx.params.mode == "Add") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Surgical Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var result;
                        result = true;//Clinical_SurgicalHx.validateProcedure();
                        if (result != false) {
                            Clinical_SurgicalHx.saveSurgicalHx(myJSON).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {

                                    Clinical_SurgicalHx.SaveFavToggelStatus($('#' + Clinical_SurgicalHx.params.PanelID + ' #ddlFavoriteListSurgicalHx').val());
                                    Clinical_SurgicalHx.params.HxTypeId = response.SurgicalHxId;
                                    $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #divSwitch").removeClass('disableAll');
                                    if (!attachToNote) {
                                        Clinical_SurgicalHx.ChangeCurrentPast(1, null, null, null);
                                        Clinical_SurgicalHx.BindCurrentSurgicalSoapText(response);
                                    }
                                    if (response.diseaseId != "") {
                                        var diseaseResponse = JSON.parse(response.diseaseId);

                                        if (diseaseResponse.diseaseId > 0) {
                                            $('#' + Clinical_SurgicalHx.params.PanelID + " div#Surgical ul#ulSurgicalDisease li.active").attr('id', diseaseResponse.diseaseId)
                                            $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSelectedDisease").val(diseaseResponse.diseaseId)
                                            Clinical_SurgicalHx.currentSelectedDisease["DiseaseId"] = diseaseResponse.diseaseId;
                                        }
                                    }

                                    $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxId").val(response.SurgicalHxId);
                                    Clinical_SurgicalHx.params.mode = "Edit";

                                    if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote" && unloadSurgicalhx == true) {
                                        if (!attachToNote) {
                                            Clinical_SurgicalHx.getSurgicalHxInfo(surgicalHxType, unloadSurgicalhx, null, true);
                                        }

                                    }
                                    utility.DisplayMessages(response.message, 1);
                                    //Serializing the form
                                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').data('serialize', $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').serialize());

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
            else if (Clinical_SurgicalHx.params.mode == "Edit") {
                //21-01-2016 syed zia ,Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Surgical Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        var result;
                        result = true;//Clinical_SurgicalHx.validateProcedure();
                        if (result != false) {
                            Clinical_SurgicalHx.updateSurgicalHx(myJSON, Clinical_SurgicalHx.params.SurgicalHxId).done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    Clinical_SurgicalHx.SaveFavToggelStatus($('#' + Clinical_SurgicalHx.params.PanelID + ' #ddlFavoriteListSurgicalHx').val());

                                    Clinical_SurgicalHx.params.HxTypeId = response.surgicalHxId;
                                    $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #divSwitch").removeClass('disableAll');
                                    if (!attachToNote) {
                                        Clinical_SurgicalHx.ChangeCurrentPast(1, null, null, null);
                                        Clinical_SurgicalHx.BindCurrentSurgicalSoapText(response);
                                    }
                                    if (response.diseaseId != "") {
                                        var diseaseResponse = JSON.parse(response.diseaseId);
                                        if (diseaseResponse.diseaseId > 0) {
                                            $('#' + Clinical_SurgicalHx.params.PanelID + " div#Surgical ul#ulSurgicalDisease li.active").attr('id', diseaseResponse.diseaseId);
                                            $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSelectedDisease").val(diseaseResponse.diseaseId);
                                            Clinical_SurgicalHx.currentSelectedDisease["DiseaseId"] = diseaseResponse.diseaseId;
                                        }
                                    }
                                    //Clinical_SurgicalHx.loadSurgicalHxComponent(surgicalHxType);
                                    if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote" && unloadSurgicalhx == true) {
                                        if (!attachToNote) {
                                            Clinical_SurgicalHx.getSurgicalHxInfo(surgicalHxType, unloadSurgicalhx);
                                        }
                                    } else {
                                        //Clinical_SurgicalHx.AppointmentStatusSearch(Clinical_SurgicalHx.params.SurgicalHxignsId);
                                        utility.DisplayMessages(response.message, 1);
                                    }

                                    //Start//17/12/2015//Ahmad Raza//Serializing the form
                                    $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').data('serialize', $('#' + Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx').serialize());
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
            //Begin 12-28-2015 Muhammad Arshad Bug# EMR-159 Surgical History Clinical Module -> Save
            utility.DisplayMessages("Please enter any value", 3);
            //End 12-28-2015 Muhammad Arshad Bug# EMR-159 Surgical History Clinical Module -> Save
        }
        //}
    },

    SaveFavToggelStatus: function (FavListVal) {
        var isFavListOpened = $('#' + Clinical_SurgicalHx.params.PanelID + " #favSectionDiv").hasClass("toggledHor");
        $.when(EMRUtility.insertUpdateFavListStatus(Clinical_SurgicalHx.FavListName, isFavListOpened)).then(function () {
            EMRUtility.insertUpdateFavListVal(Clinical_SurgicalHx.FavListName, FavListVal);
        });
    },
    BindCurrentSurgicalSoapText: function (resopnse) {
        var $row = $('<tr/>');

        if (resopnse.SoapText != "") {
            $row.append('<td style="display:none;">' + resopnse.surgicalHxId + '</td><td>' + resopnse.IsCreatedOrModified + '</td><td>' + resopnse.SoapText + '</td><td>' + resopnse.LastUpdated + '</td>');
        }
        else {
            $row.append('<td>&nbsp;</td><td>No Known Surgical History</td><td></td>');
        }
        $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvSurgicalHx tbody").html($row);

        if ($('#' + Clinical_SurgicalHx.params.PanelID + ' #pnlSurgicalHx_Result #divSwitch #switchVisit').attr('status') == '1') {
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #pnlCurrent').removeClass('hidden');
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #pnlPast').addClass('hidden');
        }
    },
    //Author: Abid Ali
    //Date: 02-02-2016
    //Logic to compare txtCPT values with IMO
    validateProcedure: function () {
        if ($('#' + Clinical_SurgicalHx.params.PanelID + ' #txtSurgicalCPTCode').val() == "") {
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #hfCPTCode').val('');
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #hfCPTDescription').val('');
            return true;
        }
        else if ($('#' + Clinical_SurgicalHx.params.PanelID + ' #hfCPTCode').val() + " - " + $('#' + Clinical_SurgicalHx.params.PanelID + ' #hfCPTDescription').val() != $('#' + Clinical_SurgicalHx.params.PanelID + ' #txtSurgicalCPTCode').val()) {
            utility.DisplayMessages("Procedure not Valid", 2);
            $('#' + Clinical_SurgicalHx.params.PanelID + ' #txtSurgicalCPTCode').val('');
            return false;
        }
        else
            return true;
        //var cptDescription = $('#' + Clinical_SurgicalHx.params.PanelID + ' #hfCPTDescription').val();
        //var cptCode = $('#' + Clinical_SurgicalHx.params.PanelID + ' #hfCPTCode').val();
        //var $txtCptCode = $('#' + Clinical_SurgicalHx.params.PanelID + ' #txtSurgicalCPTCode');
        //if ($txtCptCode.val() == "") {
        //    return true;
        //}
        //if (cptCode == "") {
        //    if (cptDescription != $txtCptCode.val()) {
        //        utility.DisplayMessages("Procedure not Valid", 2);
        //        $txtCptCode.val('');
        //        return false;
        //    }
        //}
        //else if (cptCode + " - " + cptDescription != $txtCptCode.val()) {
        //    utility.DisplayMessages("Procedure not Valid", 2);
        //    $txtCptCode.val('');
        //    return false;
        //}
        //else
        //    return true;


    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle load of SurgicalHx and it's childs as specified by SurgicalHxType
    //It represents service call to API
    fillSurgicalHx: function (surgicalHxType, diseaseId, surgicalHxId) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["SurgicalHxType"] = surgicalHxType != null ? surgicalHxType : "Surgical";
        objData["SurgicalHxId"] = surgicalHxId != null ? surgicalHxId : 0;
        objData["commandType"] = "FILL_SurgicalHx";
        objData["DiseaseId"] = diseaseId != null ? diseaseId : 0;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SurgicalHx");
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Add of SurgicalHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous)
    //It represents service call to API
    saveSurgicalHx: function (surgicalHxData) {
        var objData = JSON.parse(surgicalHxData);
        if (Clinical_SurgicalHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_SurgicalHx.params.patientID;
        }
        objData["commandType"] = "SAVE_SurgicalHx";

        var data = JSON.stringify(objData);

        //var data = "SurgicalHxignsData=" + SurgicalHxignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "SurgicalHx");
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Edit of SurgicalHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous)
    //It represents service call to API
    updateSurgicalHx: function (surgicalHxData, surgicalHxId) {

        var objData = JSON.parse(surgicalHxData);
        if (Clinical_SurgicalHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_SurgicalHx.params.patientID;
        }
        objData["commandType"] = "UPDATE_SurgicalHx";


        var data = JSON.stringify(objData);

        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "SurgicalHx");

    },
    //Author: Muhammad Arshad
    //Date: 28-01-2016
    //Overview: This function call api service of delete
    surgicalHxDiseaseDelete: function (diseaseId) {
        var objData = new Object();
        objData["DiseaseId"] = diseaseId;
        //Start/26-1-2016/Abid Ali/ pass SurgicalHx Id
        var surgicalHxId = $('#' + Clinical_SurgicalHx.params.PanelID + "  #hfSurgicalHxId").val();
        objData["SurgicalHxId"] = surgicalHxId;
        //Start/26-1-2016/Abid Ali/ pass SurgicalHx Id
        objData["PatientId"] = Clinical_SurgicalHx.params.patientID || $('#PatientProfile #hfPatientId').val();

        objData["commandType"] = "DELETE_SURGICALHXDISEASE";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SurgicalHx");
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Unload of SurgicalHx Tab
    unLoadTab: function (nextOrPre, controlToInvoke) {
        Clinical_SurgicalHx.bIsFirstLoad = true;
        //Start//11-02-2016//Ahmad Raza//fixed EMR Bug#301
        var detailExists = Clinical_SurgicalHx.isDetailExists("disease");
        if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
            Clinical_SurgicalHx.controlToInvoke = controlToInvoke;

            var diseaseAdded = $('#' + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease > li").filter(function () {
                return $(this).attr("id") < 0;
            });

            if (EMRUtility.compareFormDataWithSerialized(Clinical_SurgicalHx.params.PanelID + ' #frmClinicalSurgicalHx') || diseaseAdded.length > 0) {
                utility.myConfirmNote('1', function () {
                    var surgicalHxType = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxType").val();
                    if (surgicalHxType == "" || surgicalHxType == "undefined") {
                        Clinical_SurgicalHx.unloadSurgicalHistory(nextOrPre);
                        if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSurgicalHx") {
                            UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                            Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSurgicalHx', 'SurgicalHx');
                        }
                    }
                    else {
                        Clinical_SurgicalHx.bNextPrev = true;
                        Clinical_SurgicalHx.surgicalHxSave(surgicalHxType, true);
                    }
                }, function () {
                    var surgicalHxType = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxType").val();
                    if (surgicalHxType == "" || surgicalHxType == "undefined") {
                        Clinical_SurgicalHx.unloadSurgicalHistory(nextOrPre);
                        if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSurgicalHx") {
                            UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                            Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSurgicalHx', 'SurgicalHx');
                        }
                    }
                    else {
                        Clinical_SurgicalHx.bNextPrev = true;
                        Clinical_SurgicalHx.surgicalHxSave(surgicalHxType, true, true);
                    }
                    Clinical_SurgicalHx.unloadSurgicalHistory();
                    if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSurgicalHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSurgicalHx', 'SurgicalHx');
                    }
                }, function () {
                    Clinical_SurgicalHx.unloadSurgicalHistory(nextOrPre);
                    if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSurgicalHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSurgicalHx', 'SurgicalHx');
                    }
                });
            }
            else {
                Clinical_SurgicalHx.unloadSurgicalHistory();
                if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSurgicalHx") {
                    UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                    Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSurgicalHx', 'SurgicalHx');
                }
            }
            //End//11-02-2016//Ahmad Raza//fixed EMR Bug#301

        } else {
            Clinical_SurgicalHx.unloadSurgicalHistory();
        }
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Unload of SurgicalHx form
    unloadSurgicalHistory: function (nextOrPre) {

        if (Clinical_SurgicalHx.params["FromAdmin"] == "0") {
            if (Clinical_SurgicalHx.params != null && Clinical_SurgicalHx.params.ParentCtrl != null) {
                if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                    UnloadActionPan(Clinical_SurgicalHx.params.ParentCtrl, 'Clinical_SurgicalHx');
                    if (Clinical_SurgicalHx.controlToInvoke != null) {
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_SurgicalHx.controlToInvoke);
                            Clinical_SurgicalHx.controlToInvoke = null;
                        }, 400);
                    }
                }
                else {
                    UnloadActionPan(Clinical_SurgicalHx.params.ParentCtrl, 'Clinical_SurgicalHx');
                    if (Clinical_SurgicalHx.controlToInvoke != null)
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_SurgicalHx.controlToInvoke);
                            Clinical_SurgicalHx.controlToInvoke = null;
                        }, 400);
                }
            }
            else
                UnloadActionPan(null, 'Clinical_SurgicalHx');
        }
        else {
            $("#mstrDivSurgical #clinicalMenu_History_SurgicalHx").remove();
            RemoveAdminTab();
        }
        EMRUtility.scrollToPNcomponent('clinical_surgicalhx');
    },

    //Author: Abid Ali
    //Date: 10-02-2016
    //Abid AliThis function will take clone of any jquery context
    takeDeepCloneAndRemoveSection: function (sectionId) {
        Clinical_SurgicalHx.cloneSurgicalForm = $(sectionId).clone(true);
        $(sectionId).remove();
    },


    //-----------------Progress Note-------------
    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Author: Ahmad Raza
    //Date: 05-02-2016
    //Call Back function to add component to Progress Note
    addSurgicalHxToNotes: function () {
        var surgicalHxId = Clinical_SurgicalHx.params.SurgicalHxId;
        var surgicalHxType = $('#' + Clinical_SurgicalHx.params.PanelID + " #hfSurgicalHxType").val();
        Clinical_SurgicalHx.surgicalHxSave(surgicalHxType, true);

    },

    //Author: Ahmad Raza
    //Date: 05-02-2016
    //this function will get Surgical History Soap Text and attach that to Progress note
    getSurgicalHxInfo: function (surgicalHxType, unloadSurgicalhx, surgicalHxId, hideAlertMessage) {
        Clinical_SurgicalHx.fillSurgicalHx(surgicalHxType, null, surgicalHxId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_SurgicalHx.createSurgicalHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', unloadSurgicalhx, hideAlertMessage);

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //Author: Ahmad Raza
    //Date: 05-02-2016
    //This Function will check, if Surgical History Soap is already attached in Progress note, if Surgical History is not attached than it will create main divs to attach allergy
    checkSurgicalHxExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Surgicalhx').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';


            $(CompnentSelector).append(' <li class="SurgicalHxComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_Surgicalhx title="Surgical Hx"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'SurgicalHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="SurgicalHx">Surgical Hx</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'SurgicalHx\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'SurgicalHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_Surgicalhx> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Surgicalhx').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    createSurgicalHxBodyHTMLFromNote: function (response, noteHTMLCtrl, unloadSurgicalhx, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_SurgicalHx.checkSurgicalHxExists();
        if (response) {
            var surgicalHxFill_Obj = response;
            var $mainDivSurgicalHx = $(document.createElement('div'));

            var surgicalHxId = surgicalHxFill_Obj.SurgicalHxId;
            if (surgicalHxId > 0) {
                var $sectionBodySurgicalHx = $(document.createElement('section'));
                $sectionBodySurgicalHx.attr('id', "Cli_SurgicalHx_Main" + surgicalHxId);
                var $detailsDiv = $(document.createElement('div'));
                $detailsDiv.attr('id', "Cli_SurgicalHx_" + surgicalHxId);
                var $listSurgicalHx = $(document.createElement('ul'));

                $listSurgicalHx.attr('class', 'list-unstyled')

                $sectionBodySurgicalHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_SurgicalHx_" + surgicalHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_SurgicalHx_Main" + surgicalHxId + '"  ><i class="fa fa-times"></i></a></div> ');


                $listSurgicalHx.append("<li>" + surgicalHxFill_Obj.SurgicalHxSoapText + "</li>");
                $detailsDiv.append($listSurgicalHx);
                $sectionBodySurgicalHx.append($detailsDiv);
                if ($(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId).length == 0) {
                    $mainDivSurgicalHx.append($sectionBodySurgicalHx);
                    var surgicalHxHtml = $mainDivSurgicalHx.html();
                    if (surgicalHxHtml != '') {
                        $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().addClass('initialVisitBody');
                        $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().append(surgicalHxHtml);
                    }
                } else {
                    var commentHTML = "";
                    var commentsID = $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId + ' ul li:Last').attr('id');
                    if (commentsID != null && commentsID.indexOf("Comments") >= 0) {
                        commentHTML = $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId).html($sectionBodySurgicalHx.html());
                    $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId + ' ul').append(commentHTML);
                }

            }
        }
        dfd.resolve();
        return dfd;
    },

    createSurgicalHxBodyHTMLFromNotes: function (SurgicalHistory, noteHTMLCtrl, unloadSurgicalhx, hideAlertMessage) {
        Clinical_SurgicalHx.checkSurgicalHxExists();
        if (SurgicalHistory && SurgicalHistory.SurgicalHxId && SurgicalHistory.SurgicalHxId > 0) {
            var surgicalHxFill_Obj = SurgicalHistory;
            var $mainDivSurgicalHx = $(document.createElement('div'));

            var surgicalHxId = surgicalHxFill_Obj.SurgicalHxId;
            if (surgicalHxId > 0) {
                var $sectionBodySurgicalHx = $(document.createElement('section'));
                $sectionBodySurgicalHx.attr('id', "Cli_SurgicalHx_Main" + surgicalHxId);
                var $detailsDiv = $(document.createElement('div'));
                $detailsDiv.attr('id', "Cli_SurgicalHx_" + surgicalHxId);
                var $listSurgicalHx = $(document.createElement('ul'));

                $listSurgicalHx.attr('class', 'list-unstyled')

                $sectionBodySurgicalHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_SurgicalHx_" + surgicalHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_SurgicalHx_Main" + surgicalHxId + '"  ><i class="fa fa-times"></i></a></div> ');


                $listSurgicalHx.append("<li>" + surgicalHxFill_Obj.SoapText + "</li>");
                $detailsDiv.append($listSurgicalHx);
                $sectionBodySurgicalHx.append($detailsDiv);
                if ($(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId).length == 0) {
                    $mainDivSurgicalHx.append($sectionBodySurgicalHx);
                    //Clinical_SurgicalHx.updateSurgicalHxHtml($mainDivSurgicalHx.html(), surgicalHxId, noteHTMLCtrl, hideAlertMessage);
                    var surgicalHxHtml = $mainDivSurgicalHx.html();
                    if (surgicalHxHtml != '') {
                        $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().addClass('initialVisitBody');

                        $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().append(surgicalHxHtml);
                    }
                    return surgicalHxId;
                } else {

                    var commentHTML = "";
                    var commentsID = $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId + ' ul li:Last').attr('id');
                    if (commentsID != null && commentsID.indexOf("Comments") >= 0) {
                        commentHTML = $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId).html($sectionBodySurgicalHx.html());
                    $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId + ' ul').append(commentHTML);

                }

            }
        }
    },

    //Author: Ahmad Raza
    //Date: 05-02-2016
    //This Function is used to create SOAP html and append it to  Progress note
    createSurgicalHxBodyHTML: function (response, noteHTMLCtrl, unloadSurgicalhx, hideAlertMessage) {
        Clinical_SurgicalHx.checkSurgicalHxExists();
        if (response.SurgicalHxFill_JSON != null && response.SurgicalHxFill_JSON != '') {
            var surgicalHxFill_Obj = JSON.parse(response.SurgicalHxFill_JSON);
            var $mainDivSurgicalHx = $(document.createElement('div'));

            var surgicalHxId = surgicalHxFill_Obj.SurgicalHxId;
            if (surgicalHxId > 0) {
                var $sectionBodySurgicalHx = $(document.createElement('section'));
                $sectionBodySurgicalHx.attr('id', "Cli_SurgicalHx_Main" + surgicalHxId);
                var $detailsDiv = $(document.createElement('div'));
                $detailsDiv.attr('id', "Cli_SurgicalHx_" + surgicalHxId);
                var $listSurgicalHx = $(document.createElement('ul'));

                $listSurgicalHx.attr('class', 'list-unstyled')

                $sectionBodySurgicalHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_SurgicalHx_" + surgicalHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_SurgicalHx_Main" + surgicalHxId + '"  ><i class="fa fa-times"></i></a></div> ');


                $listSurgicalHx.append("<li>" + surgicalHxFill_Obj.SurgicalHxSoapText + "</li>");
                $detailsDiv.append($listSurgicalHx);
                $sectionBodySurgicalHx.append($detailsDiv);
                if ($(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId).length == 0) {
                    $mainDivSurgicalHx.append($sectionBodySurgicalHx);
                    Clinical_SurgicalHx.updateSurgicalHxHtml($mainDivSurgicalHx.html(), surgicalHxId, noteHTMLCtrl, hideAlertMessage);
                } else {

                    var commentHTML = "";
                    var commentsID = $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId + ' ul li:Last').attr('id');
                    if (commentsID != null && commentsID.indexOf("Comments") >= 0) {
                        commentHTML = $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId).html($sectionBodySurgicalHx.html());
                    $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().find('#Cli_SurgicalHx_Main' + surgicalHxId + ' ul').append(commentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText("SurgicalHx", hideAlertMessage);
                    Clinical_SurgicalHx.updateSurgicalHxHtml("", surgicalHxId, noteHTMLCtrl, hideAlertMessage);

                }

                if (unloadSurgicalhx == true) {
                    Clinical_SurgicalHx.unloadSurgicalHistory(Clinical_SurgicalHx.bNextPrev);
                }
            }
        }
    },

    //Author: Ahmad Raza
    //Date: 05-02-2016
    //This Function is called by Progress Notes (Fill SurgicalHx Func, CopyAllNotesCategories)
    updateSurgicalHxHtml: function (surgicalHxHtml, surgicalHxId, noteHTMLCtrl, hideAlertMessage) {
        $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().addClass('initialVisitBody');
        if (surgicalHxHtml != '') {
            $(noteHTMLCtrl + ' clinical_Surgicalhx').parent().parent().append(surgicalHxHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (surgicalHxHtml != '') {
            Clinical_SurgicalHx.attachSurgicalHxFromNotes(surgicalHxId, hideAlertMessage);
        }

    },

    //Author: Ahmad Raza
    //Date: 05-02-2016
    //This Function detach Surgical History From progress note
    detach_ComponentsSurgicalHx: function (componentName, isUpdate, surgicalHxComponentRemove) {
        var clinicalSurgicalHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Surgicalhx').parent().parent().find('section[id*="Cli_SurgicalHx_Main"]').map(function () {
            return this.id.replace("Cli_SurgicalHx_Main", "");
        }).get().join(',');

        if (surgicalHxComponentRemove) {

            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Surgicalhx').parent().parent().attr('NoteComponentId');
            //Start//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Surgical Hx']").remove();
            //End//31/12/2015//Ahmad Raza// changes made in context of fixing bug#181
            if (Clinical_ProgressNote.params["TemplateName"])
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Surgicalhx').parent().parent().addClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Surgicalhx').parent().parent().remove();

            var hxComponents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .HxComponent').length;

            if (NoteComponentId && NoteComponentId != "NCDummyId" && hxComponents == 0) {
                //$.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                //    Clinical_ProgressNote.ShowHideComponetsHeaders();
                //    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                //});
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Surgicalhx').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('SurgicalHx', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_Surgicalhx').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            }
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Surgicalhx').parent().parent().find('section[id*="Cli_SurgicalHx_Main"]').remove();
        }

        if (clinicalSurgicalHxIds == "" || clinicalSurgicalHxIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText("SurgicalHx", true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            Clinical_SurgicalHx.detachSurgicalHxFromNotes_DBCall(clinicalSurgicalHxIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (isUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText("SurgicalHx", true);
                        //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
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

    //Author: Ahmad Raza
    //Date: 05-02-2016
    //This Functions ask for Detaching Surgical Hx from Progress Note for current Patient Selected
    detachSurgicalHxFromNotes: function (surgicalHxId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_surgicalhx');
                var selectedValue = surgicalHxId.replace('Cli_SurgicalHx_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_SurgicalHx.detachSurgicalHxFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + surgicalHxId).remove();
                            Clinical_ProgressNote.Add_NoText();
                            Clinical_ProgressNote.saveComponentSOAPText("SurgicalHx", true);
                            //Clinical_ProgressNote.updateProgressNoteHTML();
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

    //Author: Ahmad Raza
    //Date: 05-02-2016
    //This Functions attached Surgical Hx to Progress Note for current Patient Selected
    attachSurgicalHxFromNotes: function (surgicalHxId, hideAlertMessage) {
        if (surgicalHxId == "" || surgicalHxId == "undefined") {
        }
        else {
            Clinical_SurgicalHx.attachSurgicalHxFromNotes_DBCall(surgicalHxId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    //If Attached SurgicalHx Made new inseration to SurgicalHx Table than good ids should be attached to HTML
                    Clinical_ProgressNote.saveComponentSOAPText("SurgicalHx", hideAlertMessage);
                    $('#' + surgicalHxId).remove();
                    // utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //Author: Ahmad Raza
    //Date: 05-02-2016
    //If SurgicalHx Component which is dropeed in Progress note has no SurgicalHx attached, than it will call for Latest SurgicalHx for this patient
    getLatestSurgicalHxByPatientId: function (hideAlertMessage, droppedComponent) {

        Clinical_SurgicalHx.getLatestClinical_SurgicalHxByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_SurgicalHx.createSurgicalHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
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
        objData["ComponentType"] = "SurgicalHistory";
        objData["commandType"] = "getautopopulatesetting";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");

    },


    //-----Server calls of Notes----------
    //Author: Ahmad Raza
    //Date: 08-02-2016
    //DB Call to detach surgical hx from notes
    detachSurgicalHxFromNotes_DBCall: function (surgicalHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["SurgicalHxId"] = surgicalHxId;
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
        objData["commandType"] = "detach_Surgicalhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "SurgicalHx");
    },

    //Author: Ahmad Raza
    //Date: 08-02-2016
    //DB Call to attach surgical hx with notes
    attachSurgicalHxFromNotes_DBCall: function (surgicalHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["SurgicalHxId"] = surgicalHxId;
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
        objData["commandType"] = "attach_Surgicalhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "SurgicalHx");
    },

    //Author: Ahmad Raza
    //Date: 08-02-2016
    //DB Call to get latest soap text for surgical hx
    getLatestClinical_SurgicalHxByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_Surgicalhxby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SurgicalHx");
    },

    //--------------end progress Note-----------

    favoriteListSearch: function () {
        var ProviderId = null;
        if (Clinical_SurgicalHx.params.CurrentNotesProviderId != "undefined" && Clinical_SurgicalHx.params.CurrentNotesProviderId && Clinical_SurgicalHx.params.CurrentNotesProviderId !="")
            ProviderId = Clinical_SurgicalHx.params.CurrentNotesProviderId;
        Favorite_ProcedureOrder.searchFavoriteList_DBCall("SurgicalHistory", null, 1, 5000, ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var $ddl = $('#' + Clinical_SurgicalHx.params.PanelID + ' #ddlFavoriteListSurgicalHx');
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
                    EMRUtility.getFavListValue(Clinical_SurgicalHx.FavListName).done(function (response1) {
                        response1 = JSON.parse(response1);
                        if (response1.status != false) {
                            if (response1.favListVal != "" && response1.favListVal != "-1") {
                                if ($("#" + Clinical_SurgicalHx.params.PanelID + " #ddlFavoriteListSurgicalHx option[value='" + response1.favListVal + "']").length > 0) {
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

            }
            //else {
            //    utility.DisplayMessages(response.Message, 3);
            //}
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

        $('#' + Clinical_SurgicalHx.params.PanelID + ' #ulFavoriteListSurgicalHxContent li').each(function (i, item) {
            $(item).trigger("click");
        });
    },
    AutoSearchFavSurgicalHx: function () {
        utility.Keyupdelay(function () {
            Clinical_SurgicalHx.loadfavoriteListContent();
        });
    },
    loadfavoriteListContent: function (obj) {
        if (typeof obj == typeof undefined || obj == null) {
            obj = $('#' + Clinical_SurgicalHx.params.PanelID + ' #ddlFavoriteListSurgicalHx');
        }
        var SearchData = $('#' + Clinical_SurgicalHx.params.PanelID + ' #FavSearchBox').val();
        if (obj != null) {
            var selectedOption = $(obj).find("option:selected");
            //Start 01-07-2016 Humaira Yousaf to disable Select All link
            if (selectedOption.attr("id") != "-1") {
                Clinical_SurgicalHx.favoriteList_CPTSearch(selectedOption.attr("id"), SearchData);
            }
            else {
                $('#' + Clinical_SurgicalHx.params.PanelID + ' #ulFavoriteListSurgicalHxContent').empty();
                $('#' + Clinical_SurgicalHx.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
            }
            //End 01-07-2016 Humaira Yousaf to disable Select All link

        }
    },

    favoriteList_CPTSearch: function (FavoriteListId, SearchData) {
        var $UL = $('#' + Clinical_SurgicalHx.params.PanelID + ' #ulFavoriteListSurgicalHxContent');
        Clinical_SurgicalHx.searchFavoriteList_ICD_DBCall(null, FavoriteListId, 1, 5000, SearchData).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {


                $UL.empty();

                if (response.FavoriteListCPTCount > 0) {
                    // $('#' + Clinical_ProblemLists.params.PanelID + " #frmClinicalProblemLists #ulFavCompliantDisease li").remove();
                    var FavoriteListJSON = JSON.parse(response.FavoriteListCPTJSON);
                    if (FavoriteListJSON.length > 0) {
                        $('#' + Clinical_SurgicalHx.params.PanelID + ' #favSelectAllLink').removeClass('disableAll');
                    }
                    else {
                        $('#' + Clinical_SurgicalHx.params.PanelID + ' #favSelectAllLink').addClass('disableAll');
                    }
                    var li = "";
                    $.each(FavoriteListJSON, function (i, item) {
                        if (typeof item.CPTCode == 'undefined' || item.CPTCode == null) { item.CPTCode = ''; }
                        if (typeof item.SNOMEDID == 'undefined' || item.SNOMEDID == null) { item.SNOMEDID = ''; }
                        if (typeof item.CPTCodeDescription == 'undefined' || item.CPTCodeDescription == null) { item.CPTCodeDescription = ''; }
                        var diagnosis = item.CPTCode + " - " + item.SNOMEDID + " - " + item.CPTCodeDescription;
                        var CPTCode = "" + item.CPTCode + "";
                        var CPTCodeDescription = "" + item.CPTCodeDescription + "";
                        var SNOMEDID = "" + item.SNOMEDID + "";
                        var SNOMEDDescription = "" + item.SNOMED_DESCRIPTION + "";

                        var onclick = 'Clinical_SurgicalHx.BindSurgicalHxUl(\'' + item.CPTCode + '\',\'' + item.CPTCodeDescription + '\',\'' + item.SNOMEDID + '\',\'' + item.SNOMED_DESCRIPTION + '\',this)';

                        var LiId = item.CPTCodeDescription;

                        var isFound = Clinical_SurgicalHx.isFavoriteHistoryFound(LiId, item.CPTCodeDescription);
                        if (isFound == true) {
                            $UL.append('<li class="disableAll" onclick="' + onclick + '" id="' + LiId + '">' + item.CPTCodeDescription + '</li>');
                        }
                        else {
                            $UL.append('<li onclick="' + onclick + '" id="' + LiId + '">' + item.CPTCodeDescription + '</li>');
                        }

                        // $UL.append('<li onclick="' + onclick + '">' + item.ICD9CodeDescription + ' - ' + item.SNOMEDID + '</li>');


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
        objData["FavoriteListCPTId"] = FavoriteListICDId == null ? 0 : FavoriteListICDId;
        if (globalAppdata['AppUserName'] == DefaultUser) {
            objData["EntityId"] = 0;
        }
        else {
            objData["EntityId"] = globalAppdata["SeletedEntityId"];
        }
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["SearchData"] = SearchData;
        objData["commandType"] = "load_favoritelist_cpt";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FavoriteList", "FavoriteList");
    },

    BindSurgicalHxUl: function (cptCode, cptDescription, snomedCode, snomedDescription, sender) {

        var currId = -1;
        $("#pnlClinicalSurgicalHx #frmClinicalSurgicalHx div#Surgical #ulSurgicalDisease li[id*='-']").each(function (i, item) {

            currId = $(this).attr("id");

        });

        currId = parseInt(currId) + (-1);

        var li = "<li  id=" + currId + " onclick='Clinical_SurgicalHx.fillSurgicalHxDisease(this, event);' onmouseover='Clinical_SurgicalHx.showIcon(this);' onmouseout='Clinical_SurgicalHx.hideIcon(this);' cptCode=\"" + cptCode + "\" cptDesc=\"" + cptDescription + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + cptDescription + "<span class='removeIconListHover' onclick='Clinical_SurgicalHx.deleteSurgicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";


        var IsAlreadyExist = false;
        $('#pnlClinicalSurgicalHx #ulSurgicalDisease li').each(function () {
            if ($(this).attr('cptdesc') == cptDescription &&

                $(this).attr('snomedcode') == snomedCode && $(this).attr('snomeddesc') == snomedDescription) {
                IsAlreadyExist = true;
            }
        });

        if (!IsAlreadyExist) {
            $('#pnlClinicalSurgicalHx #ulSurgicalDisease').append(li);
            $(li).trigger('click');
            $('#pnlClinicalSurgicalHx #txtDisease').val('');
            $(sender).addClass('disableAll');
            if (Clinical_SurgicalHx.params.ParentCtrl == "clinicalTabProgressNote") {
                var diseaseId = $('#' + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease > li.active").attr('id');
                var disease = $(li).get(0).outerHTML;
                var diseaseDetail = $('#' + Clinical_SurgicalHx.params.PanelID + " #sectionSurgicalDetails").clone();
                $(diseaseDetail).resetAllControls(null);
                var diseaseData = $(diseaseDetail).getMyJSONByName();
                Clinical_SurgicalHx.cacheSurgicalHxJSON(diseaseId, diseaseData, disease);
            }
        }
        else {
            utility.DisplayMessages('Procedure already added', 2);

            $('#pnlClinicalSurgicalHx #txtDisease').val('');
        }

    },
    AddNewLabRow: function (RowId, mode, CurrRef, cptCode, procDesc, cptDescription) {

        var surgicalHxDiseaseUl = $('#' + Clinical_SurgicalHx.params.PanelID + " #frmClinicalMedicalHx #ulMedicalDisease");

        // var rowId = CurrentRow.attr("id");

        var li = "<li  id=" + currId + " onclick='Clinical_SurgicalHx.fillSurgicalHxDisease(this, event);' onmouseover='Clinical_SurgicalHx.showIcon(this);' onmouseout='Clinical_SurgicalHx.hideIcon(this);' icd9Code=\"" + cptCode + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + procDesc + "<span class='removeIconListHover' onclick='Clinical_SurgicalHx.deleteSurgicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>";

        //var cptCodeHtml = '<input type="hidden" id="CPTCode' + rowId + '"  name="CPTCode" value="' + cptCode + '" />';
        //var cptDescHtml = '<input type="hidden" id="CPTDescription' + rowId + '" name="CPTDescription"  value="' + procDesc + '"  />';

        $(surgicalHxDiseaseUl).append(li);
        //add cptCode and description to the test row

        // return CurrentRow;
    },

    //Function Name: isFavoriteHistoryFound
    //Author Name: Humaira Yousaf
    //Created Date: 01-07-2016
    //Description: Checks if Favorite History is found
    isFavoriteHistoryFound: function (favICDCode, favCPTDesc) {

        var isFound = false;
        $("#" + Clinical_SurgicalHx.params.PanelID + " #ulSurgicalDisease li").each(function (index, item) {
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

    gridLoad: function (response) {
        var isactive = $('#' + Clinical_SurgicalHx.params.PanelID + ' #pnlSurgicalHx_Result #divSwitch #switchActive').attr('isactive');

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvPastSurgicalHx")) {
            $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvPastSurgicalHx").dataTable().fnClearTable();
            $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvPastSurgicalHx").dataTable().fnDestroy();
            $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvPastSurgicalHx tbody").find("tr").remove();
        }
        var logCount = JSON.parse(response);
        if (logCount.HxLogSoapCount > 0) {
            var LoadJSONData = JSON.parse(logCount.HxLogSoap_JSON); //Parsing array to JSON
            var counter = null;
            for (var i = 0; i < LoadJSONData.length; i++) {
                //   $.each(LoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "Clinical_SurgicalHx.CDSEdit('" + item.CDSId + "',event);");
                //$row.attr("id", "gvCDS_row" + item.CDSId);
                var text = LoadJSONData[i].SoapText;

                counter = i;
                $row.append('<td style="display:none;">' + counter + '</td><td>' + LoadJSONData[i].Action + '</td><td id="sptxt">' + $('<a/>').html($('<a/>').html(text).text()).text() + '</td><td>' + LoadJSONData[i].ModifiedOn + " " + LoadJSONData[i].ModifiedBy + '</td>');
                $row.find('#sptxt').html($('<a/>').html(text).text());
                $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvPastSurgicalHx tbody").last().append($row);
                //  });
            }
        }
        else {
            $("#" + Clinical_SurgicalHx.params.PanelID + ' #pnlSurgicalHx_Result #dgvPastSurgicalHx').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Known Surgical History"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bInfo": false, "bPaginate": false, "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_SurgicalHx.params.PanelID + ' #pnlSurgicalHx_Result #dgvPastSurgicalHx'))
            ;
        else {
            $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvPastSurgicalHx").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[0, "asc"]], "aoColumnDefs": [{ "bInfo": false, "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        }

        $("#" + Clinical_SurgicalHx.params.PanelID + " #pnlSurgicalHx_Result #dgvPastSurgicalHx_filter").remove();
    },

    //Function Name: enableFavoriteList
    //Author Name: Humaira Yousaf
    //Created Date: 01-07-2016
    //Description: Enables Favorite List
    enableFavoriteList: function (deleteRow) {

        $('#' + Clinical_SurgicalHx.params.PanelID + ' #ulFavoriteListSurgicalHxContent li').each(function (index, item) {
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

        $('#' + Clinical_SurgicalHx.params.PanelID + ' #' + ulId + ' li').each(function () {

            if (isEnable) {
                $(this).removeClass('disableAll');
            }
            else {
                $(this).addClass('disableAll');
            }
        });
    },



}