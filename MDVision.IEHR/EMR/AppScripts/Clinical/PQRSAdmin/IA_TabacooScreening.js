IA_TabacooScreening = {
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This file will handle all actions performed for Social History and it's child handling
    //Once SocialHx will be created then it's child can be created then.
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    bNextPrev: false,
    controlToInvoke: null,
    SelectedTab: {},
    date: '',
    unremarkable: false,
    overallComments: '',
    //Farooq Ahmad 19/01/2016
    retainedComponentMisHx: '',
    /* Start 11/01/2016 syed Zia for selected status */
    SeletedChildStatus: "",
    isRemarkableFormload: true,

    /* End 13/01/2016 Syed Zia for selected status */

    /* End 14/01/2016 Syed Zia,flag for trigger the click of first exited history tab */
    isFromTabTrigger: true,

    /* End 14/01/2016 syed Zia,flag for trigger the click of first exited history tab */

    /* Start 20/01/2016 Abid Ali for first time MiscellaneousTab load bit */
    isMiscellaneousTabTrigger: false,
    /* End 20/01/2016 Abid Ali for first time MiscellaneousTab load bit */
    isSaved: false,
    CacheTabJSON: '',
    PreviousTab: '',
    socialHxJSON: '',
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will be called once tab is clicked, it expects parameters to be used for SocialHx
    //Start//18/01/2016//Ahmad Raza//Changed radio buttons to check boxes on misc form
    readyFunction: function () {
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #CaffeineIntakDetails input:checkbox').on('click', function () {
            // in the handler, 'this' refers to the box clicked on
            var $box = $(this);
            console.log("testing check box");
            if ($box.is(":checked")) {
                // the name of the box is retrieved using the .attr() method
                // as it is assumed and expected to be immutable
                var group = "input:checkbox[name='" + $box.attr("name") + "']";
                // the checked state of the group/box on the other hand will change
                // and the current value is retrieved using .prop() method
                $(group).prop("checked", false);
                $box.prop("checked", true);
            } else {
                $box.prop("checked", false);
            }
        });
    },
    //End//18/01/2016//Ahmad Raza//Changed radio buttons to check boxes on misc form
    Load: function (params) {
        IA_TabacooScreening.params = params;

        //SelectedTab["PatientId"] = $("div#PatientProfile #hfPatientId").val();

        var bAlcoholExist = false;
        var bDrugExist = false;
        var bSexualExist = false;
        var bTobaccoExist = false;
        var bMiscHxExist = false;
        var bIsTriggerManually = false;

        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (IA_TabacooScreening.params.mode == null) {
            IA_TabacooScreening.params.mode = "Add";
        }
        if (IA_TabacooScreening.params.PanelID != 'pnlTabacooScreening') {
            IA_TabacooScreening.params.PanelID = IA_TabacooScreening.params.PanelID + ' #pnlTabacooScreening';
        } else {
            IA_TabacooScreening.params.PanelID = 'pnlTabacooScreening';
        }

        IA_TabacooScreening.ResetFormData();
        var SocialHxId = "";
        if (IA_TabacooScreening.params.mode == "Add" || IA_TabacooScreening.params.SocialHxId == null || IA_TabacooScreening.params.SocialHxId == "" || IA_TabacooScreening.params.SocialHxId == "-1") {
            SocialHxId = "-1";
        }
        else if (IA_TabacooScreening.params.mode == "Edit") {
            SocialHxId = IA_TabacooScreening.params.SocialHxId;
            //IA_TabacooScreening.SocialHxEdit(SocialHxId);
        }
        //if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
        //    IA_TabacooScreening.bIsFirstLoad = true;
        //    $('#divViewHistorySummary').addClass('hidden');
        //    $(' #pnlTabacooScreening').removeClass('row');
        //}
        /* Start 09/12/2015 Muhammadn Irfan to disabale  dtpSexualLMP if the sex of patient is Male */
        if ($('#PatientProfile #hfPatientSex').val() != "" && $('#PatientProfile #hfPatientSex').val() == "Male") {
            $('#' + IA_TabacooScreening.params.PanelID + ' #dtpSexualLMP').addClass('disableAll');
        }
        /* End 09/12/2015 Muhammadn Irfan to disabale  dtpSexualLMP if the sex of patient is Male */


        if (IA_TabacooScreening.bIsFirstLoad == true) {
            IA_TabacooScreening.bIsFirstLoad = false;
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus").sortable({
                out: function (event, ui) {
                    //utility.myConfirm('21', function () {
                    var sortedIdsInOrder = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus").sortable("toArray");

                    var miscCompnentSorted = []
                    $.each(sortedIdsInOrder, function (index, element) {
                        var ComponentName = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus li#" + element).text().trim();
                        miscCompnentSorted.push(ComponentName);
                    });

                    IA_TabacooScreening.updateComponentOrderSorting(miscCompnentSorted.join(','));
                    //}, function () {

                    //    $(ui.sender).sortable('cancel');
                    //},
                    //    '5'
                    //);
                },

                start: function (event, ui) {
                    //$(ui.item).attr("StartIndex", ui.item.index());
                    $(ui.item).attr("StartIndex", $(ui.item).attr("id"));
                    console.log('start');
                },
                stop: function (event, ui) {

                }
            })
            //.on('sortreceive', function (event, ui) {
            //    alert(this.id); // Where the item is dropped
            //    alert(ui.sender[0].id); // Where it came from
            //    alert(ui.item[0].id); // Which item
            //});
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus").disableSelection();

            //Load Dropdown
            //if (IA_TabacooScreening.bIsFirstLoad) { // start of if condition
            //    IA_TabacooScreening.bIsFirstLoad = false;

            var DetailDivName = $("#" + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active a").attr("href");
            var self = $('#' + IA_TabacooScreening.params.PanelID);

            //if (DetailDivName != null && DetailDivName != "") {
            //    self = $('#' + IA_TabacooScreening.params.PanelID + " " + DetailDivName);
            //}

            self.loadDropDowns(true).done(function () {



                //IA_TabacooScreening.FillAllDropDowns(SocialHxignId);

                $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType option").removeAttr("selected");
                $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD option").removeAttr("selected");

                /* Start 09/12/2015 Muhammad Irfan Load Sexual Complaints dropdown on Sex base */
                if ($('#PatientProfile #hfPatientSex').val() != "" && $('#PatientProfile #hfPatientSex').val() == "Male") {
                    CacheManager.BindDropDownsByEntityID('#' + IA_TabacooScreening.params.PanelID + ' #ddlSexualComplaints', 'GetSexualHxComplaints', true, 'Male');
                } else if ($('#PatientProfile #hfPatientSex').val() != "" && $('#PatientProfile #hfPatientSex').val() == "Female") {
                    CacheManager.BindDropDownsByEntityID('#' + IA_TabacooScreening.params.PanelID + ' #ddlSexualComplaints', 'GetSexualHxComplaints', true, 'Female');
                }
                /* End 09/12/2015 Muhammad Irfan Load Sexual Complaints dropdown on Sex base */

                /* Start 09/12/2015 Muhammad Irfan This will trigger tobacco which will load tobacco panel */
                // $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').trigger('click');
                /* End 09/12/2015 Muhammad Irfan This will trigger tobacco which will load tobacco panel */

                //Start//16/12/2015//Ahmad Raza//Loading Tabacco Tab and unserializ form
                IA_TabacooScreening.loadTabaccoTabnUnserializeForm();
                IA_TabacooScreening.isFromTabTrigger = true;

                //End//16/12/2015//Ahmad Raza//Loading Tabacco Tab and unserializ form
                //  IA_TabacooScreening.triggerSocialHistoryTab();
                //IA_TabacooScreening.loadSocialHx("tobacco");

                $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());

            });
            IA_TabacooScreening.validateSocialHx();
            IA_TabacooScreening.readyFunction();
            //end Load Dropdown
            utility.CreateDatePicker(IA_TabacooScreening.params.PanelID + '  #dtSocialHxDate', function () {
            }, true);

            utility.CreateDatePicker(IA_TabacooScreening.params.PanelID + '  section#sectionSexualHx #dtpSexualLMP', function () {
            }, false);

            if ($('#' + IA_TabacooScreening.params.PanelID + ' #PatientProfile #hfPatientId').val() != "") {
                $('#' + IA_TabacooScreening.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            }
            //22/12/2015//AhmadRaza//Form Serialization
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());

            /*
               Change Implement BY: Muhammad Azhar Shahzad
               Reason:To Show navigation on Progress Note
               Created Date: Dec 15, 2015
           */
            //Code for progress note navigation
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {

                //Start//01/18/2016//Abid Ali//Reset Main SocialHx html form data
                IA_TabacooScreening.removeMainSocialHxTabPageHtml();
                //End//01/18/2016//Abid Ali//Remove Main SocialHx form data

                //Start 20-01-2016 Muhammad Arshad Change HTML when socialHx page is opened from note flow
                $('#' + IA_TabacooScreening.params.PanelID + ' div#pnlSection_Search').removeClass("panel-body NoRadiusT");//
                $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSocialHx').removeClass("panel panel-featured");
                $('#' + IA_TabacooScreening.params.PanelID + ' div.modal-footer').removeClass("hidden");
                //END 20-01-2016 Muhammad Arshad Change HTML when socialHx page is opened from note flow


                EMRUtility.appendPrevNext_NotesComponent_Btns(IA_TabacooScreening.params.PanelID, 'History', 'SocialHx', 'IA_TabacooScreening.unLoadTab(true);', null, true);
                $('#' + IA_TabacooScreening.params.PanelID + ' #btnAddVitalsOnNote').show();
                // $('#' + IA_TabacooScreening.params.PanelID + '  #dtSocialHxDate').prop('disabled', true);

            } else {

                $('#' + IA_TabacooScreening.params.PanelID + ' #btnAddVitalsOnNote').hide();
                $('#' + IA_TabacooScreening.params.PanelID + '  #dtSocialHxDate').prop('disabled', false);
            }
            //end change azhar Dec 15, 2015
            IA_TabacooScreening.domReadyFunction();


        } else {
            IA_TabacooScreening.bIsFirstLoad = false;
        }
        IA_TabacooScreening.readyPluggin();

        if (LastSocialHx != null && LastSocialHx.PatientId == $('#PatientProfile #hfPatientId').val()) {
            setTimeout(function () {
                $('#' + IA_TabacooScreening.params.PanelID + ' #' + LastSocialHx.SocialHxType + " a").trigger('click');
            }, 100)
        }

        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnTobaccoSave').addClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnAlcoholSave').addClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnDrugSave').addClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnSexualSave').addClass('hidden');
            //$('#' + IA_TabacooScreening.params.PanelID + ' #btnOccupationSave').addClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnSleepSave').addClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnExercisesSave').addClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnHousingSave').addClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnCaffeineSave').addClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');
            IA_TabacooScreening.socialHxJSON = '';
        }
        else {
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnTobaccoSave').removeClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnAlcoholSave').removeClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnDrugSave').removeClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnSexualSave').removeClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnOccupationSave').removeClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnSleepSave').removeClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnExercisesSave').removeClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnHousingSave').removeClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #btnCaffeineSave').removeClass('hidden');
        }

        utility.CreateDatePicker('pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxFromDate',
        function (ev) {
        }, false);
        utility.CreateDatePicker('pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxToDate',
        function (ev) {
        }, false);

        utility.ValidateFromToDate('pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx', 'dtTravelHxFromDate', 'dtTravelHxToDate', true);

        utility.CreateDatePicker('pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #dtOccupationHxStartDate',
                function (ev) {
                }, false);
        utility.CreateDatePicker('pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate',
        function (ev) {
        }, false);
        utility.ValidateFromToDate('pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails', 'dtOccupationHxStartDate', 'dtOccupationHxEndDate', true);

    },
    ResetFormData: function () {

        var details = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx");
        $(details).resetAllControls(null);

        var detailsSection = $('#' + IA_TabacooScreening.params.PanelID + " #TabsSection");
        $(detailsSection).resetAllControls(null);
        IA_TabacooScreening.bIsFirstLoad = true;
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #TobaccoDataChangeBit").val(0);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfAlcoholId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfAlcoholDataChangeBit").val(0);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfDrugAbuseId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfDrugAbuseId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfDrugAbuseDataChangeBit").val(0);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSexualHxId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfMiscHxId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfOccupationHxId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfTravelDetailHxId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSleepHxId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfExercisesHxId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfHousingHxId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfCaffeineIntakeHxId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(-1);
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val('tobacco');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SocialHxSoapText").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx ul:not(#ulSocialHxTabsItems) li").removeClass('active');
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ulSocialHxTabsItems').removeClass('disableAll');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx ul#ulSocialHxTabsItems li").removeClass('successLight');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #TabsSection #Tobacco").removeClass('disableAll');
    },
    validateSocialHx: function () {
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   TobaccoCessationPeriod: {
                       group: '.col-xs-8',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   TobaccoCessationLength: {
                       group: '.col-xs-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   AlcoholCessationLength: {
                       group: '.col-xs-4',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   AlcoholCessationPeriod: {
                       group: '.col-xs-8',
                       enabled: true,
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
                IA_TabacooScreening.socialHxSave();
            }

            e.type = "";


        });
        IA_TabacooScreening.enableAlcoholDurationValidation();
        IA_TabacooScreening.enableDurationValidation();

    },

    //Begin 14-01-2016 syed zia , for number pad
    domReadyFunction: function () {

        /* Start 20/01/2016/ Abid Ali/Sets MiscellaneousTab bit to false on close patient event */
        $('#btnClosePatient').on('click', function () {
            IA_TabacooScreening.isMiscellaneousTabTrigger = false;
            // IA_TabacooScreening.isDataExist = false;
        });
        /* End 20/01/2016/ Abid Ali/Sets MiscellaneousTab bit to false on close patient event */

        $(function () {
            $('#' + IA_TabacooScreening.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            //EMR-70 Bug number Resolution
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx [data-plugin-keyboard-numpad]').keyboard({
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

    //End 14-01-2016 syed zia , for number pad


    //Begin 15-01-2016 syed zia, for sleep hours validation
    validateSleepHours: function (obj) {
        var Ctrlvalue = "";
        if (obj != null) {
            Ctrlvalue = $(obj).val();
        }
        var sleepval = Ctrlvalue;
        //Begin 26-01-2016 Abid Ali, for sleep hours validation
        var activeChilStatusText = IA_TabacooScreening.getActiveChildMenuStatus();
        if (activeChilStatusText != "") {
            //Start || 15 July, 2016 || ZeeshanAK || Fix for EMR-1610
            if (activeChilStatusText.indexOf("Less than 4 hours a night") > -1) {
                sleepval = parseInt(Ctrlvalue) < 4 ? Ctrlvalue : 3;
            }
            else if (activeChilStatusText.indexOf("More than 7 hours a night") > -1) {
                sleepval = parseInt(Ctrlvalue) > 7 ? Ctrlvalue : 8;
            }
            else {
                sleepval = parseInt(Ctrlvalue) < 25 ? Ctrlvalue : "";
            }
            //End   || 15 July, 2016 || ZeeshanAK || Fix for EMR-1610

        }
        //End 26-01-2016 Abid Ali, for sleep hours validation

        sleepval = sleepval < 24 ? sleepval : 24;
        sleepval = sleepval > 0 ? sleepval : "";
        $(obj).val(sleepval);
        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());

    },

    EnableDurationPregnantSexual: function () {
        if ($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx #RadSexualNoPregnant").is(":checked")) {
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx #txtSexualHxPregnancyDuration").removeAttr('disabled');
        }
        else if ($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx #RadSexualYesPregnant").is(":checked"))
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx #txtSexualHxPregnancyDuration").removeAttr('disabled');
    },

    enableAlcoholDurationValidation: function () {

        var stayLength = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #txtAlcoholCessationLength').val();
        var ddlVal = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ddlAlcoholCessationPeriod').val();
        if (stayLength != null && stayLength != '') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('AlcoholCessationPeriod', true);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblAlcoholDuration').html('Duration<span class="required">*</span>');
        }
        else {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('AlcoholCessationPeriod', false);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblAlcoholDuration').html('Duration');
        }
        if (ddlVal != null && ddlVal != '') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('AlcoholCessationLength', true);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblAlcoholDuration').html('Duration<span class="required">*</span>');

        } else {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('AlcoholCessationLength', false);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblAlcoholDuration').html('Duration');
        }

    },


    //Author: Abid Ali
    //Date: 26-01-2016
    //Returns text of active child menu status
    getActiveChildMenuStatus: function () {
        var activeChilStatusText = "";
        $('#' + IA_TabacooScreening.params.PanelID + " #ulMiscChildStatus > li").each(function () {
            var $this = $(this);
            if ($this.hasClass('active')) {
                activeChilStatusText = $this.find('a').text();
            }
        });
        return activeChilStatusText;
    },

    //End 15-01-2016 syed zia, for sleep hours validation

    //Author: Muhammad Arshad
    //Date: 12-09-2015
    //This function will handle fill of SocialHx Statuses like SmokingStatus,AlcoholStatus,DrugAbuseStatus,SexualHxStatus

    loadSocialHxStatuses: function (Crtl, methodName, reload, StatusType) {
        var currentLiClass = "";
        var currentLiClick = "";
        var ParentDiv = "";
        if (StatusType != null && StatusType.toLowerCase() == "tobacco") {
            Crtl = '#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus";
            currentLiClick = "IA_TabacooScreening.enableDisableTobaccoControls";
            ParentDiv = "Tobacco";
            methodName = "GetTobaccoSmokingStatus";
        }
        else if (StatusType != null && StatusType.toLowerCase() == "alcohol") {
            Crtl = '#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Alcohol #ulAlcoholStatus";
            currentLiClick = "IA_TabacooScreening.enableDisableAlcoholControls";
            ParentDiv = "Alcohol";
            methodName = "GetAlcoholStatus";
        }
        else if (StatusType != null && StatusType.toLowerCase() == "drug") {
            Crtl = '#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#DrugAbuse #ulDrugStatus";
            currentLiClick = "IA_TabacooScreening.enableDisableDrugAbuseControls";
            ParentDiv = "DrugAbuse";
            methodName = "GetDrugAbuseStatus";
        }
        else if (StatusType != null && StatusType.toLowerCase() == "sexual") {
            Crtl = '#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#SexualHx #ulSexualStatus";
            currentLiClick = "IA_TabacooScreening.enableDisableSexualHxControls";
            ParentDiv = "SexualHx";
            methodName = "GetSexualHxStatus";
        }
        return MDVisionService.lookups(methodName, reload).done(function (result) {
            result = JSON.parse(result[methodName]);
            if ($(Crtl).length > 0)
                l = $(Crtl);

            l.empty();

            var isFirstLi = true;
            $.each(result, function (j, item) {

                if (item.Value != "") {
                    if (isFirstLi == true) {
                        currentLiClass = '';
                        isFirstLi = false;
                    }
                    else {
                        currentLiClass = "";
                    }
                    var onClick = currentLiClick + "(this,'" + String(item.Name) + "','" + String(item.Value) + "');";
                    //item.Value = item.Value == "" ? 0 : item.Value;

                    if (item.Name != "Chews tobacco" && item.Name != "Does not chew tobacco") {
                        if (StatusType.toLowerCase() == "tobacco") {
                            l.append('<li id="' + item.Value + '" ' + currentLiClass + ' onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '"><a href="javascript:void(0)" title="' + item.RefValue + '">' + item.Name + ' </a></li>');
                        }
                        else {
                            l.append('<li id="' + item.Value + '" ' + currentLiClass + ' onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.RefValue + '"><a href="javascript:void(0)">' + item.Name + ' </a></li>');
                        }
                    }
                }

            });

        });
    },

    TravelHxDelete: function (id, event) {
        if (event != null)
            event.stopPropagation();
        utility.myConfirm('1', function () {
            AppPrivileges.GetFormPrivileges("History_Social Hx", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    IA_TabacooScreening.DeleteSocialHxMiscHxTravelHx_DBCall(id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            IA_TabacooScreening.SocialHxMiscHxTravelHxSearch();
                            utility.DisplayMessages(response.Message, 1);
                            IA_TabacooScreening.fillSocialHx('miscellaneous_travel').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            IA_TabacooScreening.BindCurrentSocialHxSoapText(response, true);
                                        }
                                    }
                                }
                            });
                        }
                        else
                            utility.DisplayMessages(response.Message, 2);
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }, function () { },
            '1'
        );
    },
    OccupationHxDelete: function (id, event) {
        if (event != null)
            event.stopPropagation();
        utility.myConfirm('1', function () {
            AppPrivileges.GetFormPrivileges("History_Social Hx", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    IA_TabacooScreening.DeleteSocialHxMiscHxOccupationHx_DBCall(id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            IA_TabacooScreening.fillSocialHx('miscellaneous_occupation').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            IA_TabacooScreening.BindCurrentSocialHxSoapText(response, true);
                                        }
                                    }
                                }
                            });
                            IA_TabacooScreening.SocialHxMiscHxOccupationHxSearch();
                            utility.DisplayMessages(response.Message, 1);
                        }
                        else
                            utility.DisplayMessages(response.Message, 2);
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }, function () { },
            '1'
        );
    },

    AddNewSocialHxOccupationDetail: function () {
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #hfOccupationHxId").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxStartDate").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #txtOccupationHxDetails").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #txtOccupationComments").val('');
    },
    SaveSocialHxOccupationDetail: function () {
        if ($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").length == 0) {
            utility.DisplayMessages("Please select status first ", 2);
            return false;
        }
        if ($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").length > 1) {
            utility.DisplayMessages("Please select only one status ", 2);
            return false;
        }
        var mode = "Add";
        var SocialHxId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() != "" ? $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() : "-1";
        self = $('#' + IA_TabacooScreening.params.PanelID + " div#OccupationDetails");
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (parseInt(objData["OccupationHxId"]) > 0)
            mode = "Edit";

        if ($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").is(":checked"))
            objData["RadOccupation"] = true;
        else if ($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPresentExperience").is(":checked"))
            objData["RadOccupation"] = false;

        objData["SocialHxId"] = SocialHxId;
        objData["MiscChildStatus"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id");
        objData["MiscChildStatusText"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text();

        objData["SocialHxDate"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
        objData["SocialHxUnremarkable"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
        objData["SocialComments"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();

        if (mode == "Add") {
            AppPrivileges.GetFormPrivileges("History_Social Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    objData["commandType"] = "SAVE_sochxmischxOccupationhx";
                    IA_TabacooScreening.SaveSocialHxMiscHxOccupationHx_DbCall(objData).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            IA_TabacooScreening.AddNewSocialHxOccupationDetail();
                            IA_TabacooScreening.SocialHxMiscHxOccupationHxSearch();
                            IA_TabacooScreening.fillSocialHx('miscellaneous_occupation').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            IA_TabacooScreening.BindCurrentSocialHxSoapText(response, true);
                                        }
                                    }
                                }
                            });
                            utility.DisplayMessages(response.Message, 1);
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
        else if (mode == "Edit") {
            AppPrivileges.GetFormPrivileges("History_Social Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    objData["commandType"] = "Update_sochxmischxOccupationhx";
                    IA_TabacooScreening.SaveSocialHxMiscHxOccupationHx_DbCall(objData).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            IA_TabacooScreening.AddNewSocialHxOccupationDetail();
                            IA_TabacooScreening.SocialHxMiscHxOccupationHxSearch();
                            IA_TabacooScreening.fillSocialHx('miscellaneous_occupation').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            IA_TabacooScreening.BindCurrentSocialHxSoapText(response, true);
                                        }
                                    }
                                }
                            });
                            utility.DisplayMessages(response.Message, 1);
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
    SaveSocialHxMiscHxOccupationHx_DbCall: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHxMiscHxOccupationHx");
    },
    TravelHxEdit: function (id, event) {
        if (event != null)
            event.stopPropagation();

        AppPrivileges.GetFormPrivileges("History_Social Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                IA_TabacooScreening.SearchSocialHxMiscHxTravelHx_DBCall(null, null, id).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx");
                        var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                        if (response.MiscHxTravelHxCount > 0) {
                            response.SocialHxMiscHxTravelHx_JSON = JSON.parse(response.SocialHxMiscHxTravelHx_JSON);
                            var detail = response.SocialHxMiscHxTravelHx_JSON[0];
                            //$('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + detail.StatusId).addClass("active");
                            IA_TabacooScreening.markStatusActive('ulMiscChildStatus', detail.StatusId);
                            $.when(IA_TabacooScreening.enableDisableTravelControls($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text().trim(), detail.StatusId, true)).then(function () {
                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #hfTravelDetailHxId").val(detail.TravelHxId);
                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxFromDate").val(utility.RemoveTimeFromDate(null, detail.FromDate));
                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxToDate").val(utility.RemoveTimeFromDate(null, detail.Todate));
                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #txtTravelHxLocation").val(detail.Location);
                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #txtTravelHxComments").val(detail.Comments);
                                if (detail.StatusId == $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id") && $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text().trim() == "Does Not Travel")
                                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").removeClass('disableAll');
                            });
                        }
                    }
                    else
                        utility.DisplayMessages(response.Message, 2);
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    OccupationHxEdit: function (id, event) {
        if (event != null)
            event.stopPropagation();
        AppPrivileges.GetFormPrivileges("History_Social Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                IA_TabacooScreening.SearchSocialHxMiscHxOccupationHx_DBCall(null, null, id).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails");
                        if (response.MiscHxOccupationHxCount > 0) {
                            response.SocialHxMiscHxOccupationHx_JSON = JSON.parse(response.SocialHxMiscHxOccupationHx_JSON);
                            var detail = response.SocialHxMiscHxOccupationHx_JSON[0];
                            detail.IsPast = detail.IsPast.toLowerCase() == 'true';
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #hfOccupationHxId").val(detail.OccupationHxId);
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxStartDate").val((utility.RemoveTimeFromDate(null, detail.StartDate)));
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").val((utility.RemoveTimeFromDate(null, detail.EndDate)));
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #txtOccupationHxDetails").val(detail.OccupationDetail);
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #txtOccupationComments").val(detail.Comments);
                            (detail.IsPast ? $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").prop("checked", true) : $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPresentExperience").prop("checked", true));
                            IA_TabacooScreening.EnableDisableToDateOccupation(true);
                            //$('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + detail.StatusId).addClass("active");
                            IA_TabacooScreening.markStatusActive('ulMiscChildStatus', detail.StatusId);
                        }
                    }
                    else
                        utility.DisplayMessages(response.Message, 2);
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    SaveSocialHxTravelDetail: function () {
        var mode = "Add";
        var SocialHxId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() != "" ? $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() : "-1";
        self = $('#' + IA_TabacooScreening.params.PanelID + " div#divTravelDetailsHx");
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (parseInt(objData["TravelHxId"]) > 0)
            mode = "Edit";
        objData["SocialHxId"] = SocialHxId;
        objData["MiscChildStatus"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id");
        objData["MiscChildStatusText"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text();

        objData["SocialHxDate"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
        objData["SocialHxUnremarkable"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
        objData["SocialComments"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();

        if (mode == "Add") {
            AppPrivileges.GetFormPrivileges("History_Social Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    objData["commandType"] = "SAVE_sochxmischxtravelhx";
                    IA_TabacooScreening.SaveSocialHxTraveHx_DbCall(objData).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(response.sociahxid);
                            IA_TabacooScreening.AddNewSocialHxTravelDetail();
                            IA_TabacooScreening.SocialHxMiscHxTravelHxSearch();
                            IA_TabacooScreening.fillSocialHx('miscellaneous_travel').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            IA_TabacooScreening.BindCurrentSocialHxSoapText(response, true);
                                        }
                                    }
                                }
                            });
                            utility.DisplayMessages(response.Message, 1);
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
        else if (mode == "Edit") {
            AppPrivileges.GetFormPrivileges("History_Social Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    objData["commandType"] = "Update_sochxmischxtravelhx";
                    IA_TabacooScreening.SaveSocialHxTraveHx_DbCall(objData).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(response.sociahxid);
                            IA_TabacooScreening.AddNewSocialHxTravelDetail();
                            IA_TabacooScreening.SocialHxMiscHxTravelHxSearch();
                            IA_TabacooScreening.fillSocialHx('miscellaneous_travel').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            IA_TabacooScreening.BindCurrentSocialHxSoapText(response, true);
                                        }
                                    }
                                }
                            });
                            utility.DisplayMessages(response.Message, 1);
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
    SaveSocialHxTraveHx_DbCall: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHxMiscHxTravelHx");
    },
    SocialHxMiscHxOccupationHxSearch: function (PageNo, rpp) {
        IA_TabacooScreening.SearchSocialHxMiscHxOccupationHx_DBCall(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                IA_TabacooScreening.SocialHxMiscHxOccupationHxGridLoad(response);
                var TableControl = "pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx";
                var PagingPanelControlID = "pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #divSocialHxMiscHxOccupationHxPaging";
                var ClassControlName = "IA_TabacooScreening";
                var PagesToDisplay = 3;
                var iTotalDisplayRecords = response.iTotalSocialHxMiscHxOccupationHxDisplayRecords;
                setTimeout(CreatePagination(response.iTotalSocialHxMiscHxOccupationHxDisplayRecords, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PageNumber, ResultPerPage) {
                    IA_TabacooScreening.SocialHxMiscHxOccupationHxSearch(PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SocialHxMiscHxOccupationHxGridLoad: function (response) {
        var isTrueSet;
        $("#pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx").dataTable().fnDestroy();
        $("#pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx tbody").find("tr").remove();
        if (response.MiscHxOccupationHxCount > 0) {
            var SocialHxMiscHxOccupationHxJSONData = JSON.parse(response.SocialHxMiscHxOccupationHx_JSON);
            $.each(SocialHxMiscHxOccupationHxJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvSHxMiscHxTravelHx_row" + i);
                isTrueSet = (item.IsPast.toLowerCase() == 'true');
                $row.append('<td style="display:none;">' + item.OccupationHxId + '</td><td><a class="btn btn-xs" href="javascript:void(0);" onclick="IA_TabacooScreening.OccupationHxEdit(' + item.OccupationHxId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn btn-xs" href="javascript:void(0);" onclick="IA_TabacooScreening.OccupationHxDelete(' + item.OccupationHxId + ', event);"   title="Delete Record"><i class="fa fa-close red"></i></a></td><td>' + (isTrueSet ? "Past" : "Present") + '</td><td>' + (item.OccupationDetail ? item.OccupationDetail : "N/A") + '</td><td>' + ((item.StartDate) ? (utility.RemoveTimeFromDate(null, item.StartDate)) : "N/A") + '</td><td>' + ((item.EndDate) ? (utility.RemoveTimeFromDate(null, item.EndDate)) : "N/A") + '</td>');
                IA_TabacooScreening.markStatusActive('ulMiscChildStatus', item.StatusId, true);
                $("#pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx tbody").last().append($row);
            });
        }
        else {
            $("#pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #divSocialHxMiscHxOccupationHxPaging").css("display", "none");
            $('#pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx').DataTable({
                "language": {
                    "emptyTable": "No Occupation History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx'))
            ;
        else
            $("#pnlTabacooScreening #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
    },
    SocialHxMiscHxTravelHxSearch: function (PageNo, rpp) {
        IA_TabacooScreening.SearchSocialHxMiscHxTravelHx_DBCall(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                IA_TabacooScreening.SocialHxMiscHxTravelHxGridLoad(response);
                var TableControl = "pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx";
                var PagingPanelControlID = "pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #divSocialHxMiscHxTravelHxPaging";
                var ClassControlName = "IA_TabacooScreening";
                var PagesToDisplay = 3;
                var iTotalDisplayRecords = response.iTotalSocialHxMiscHxTravelHxDisplayRecords;
                setTimeout(CreatePagination(response.iTotalSocialHxMiscHxTravelHxDisplayRecords, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PageNumber, ResultPerPage) {
                    IA_TabacooScreening.SocialHxMiscHxTravelHxSearch(PageNumber, ResultPerPage);
                }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    EnableDisableToDateOccupation: function (edit) {
        if ($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPresentExperience").is(":checked")) {
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").val('');
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #divOccupationEndDate").css('display', 'none');
        }
        else if ($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").is(":checked")) {
            if (!edit)
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").val('');
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").removeAttr('disabled');
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #divOccupationEndDate").css('display', '');
        }
    },
    SearchSocialHxMiscHxOccupationHx_DBCall: function (PageNo, rpp, OccupationHxId) {
        if (!PageNo)
            PageNo = 1;
        if (!rpp)
            rpp = 15;
        var objData = {};
        if (!OccupationHxId)
            OccupationHxId = 0;
        objData["OccupationHxId"] = OccupationHxId;
        objData["PageNumber"] = PageNo;
        objData["RowsPerPage"] = rpp;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "Fill_SocHxMiscHxOccupationHx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHxMiscHxOccupationHx");
    },
    SearchSocialHxMiscHxTravelHx_DBCall: function (PageNo, rpp, TravelHxId) {
        if (!PageNo)
            PageNo = 1;
        if (!rpp)
            rpp = 15;
        var objData = {};
        if (!TravelHxId)
            TravelHxId = 0;
        objData["TravelHxId"] = TravelHxId;
        objData["PageNumber"] = PageNo;
        objData["RowsPerPage"] = rpp;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "Fill_SocHxMiscHxTravelHx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHxMiscHxTravelHx");
    },
    DeleteSocialHxMiscHxTravelHx_DBCall: function (TravelHxId) {
        var objData = {};
        objData["TravelHxId"] = TravelHxId;
        objData["SocialHxId"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val();
        objData["commandType"] = "delete_sochxmischxtravelhx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHxMiscHxTravelHx");
    },
    DeleteSocialHxMiscHxOccupationHx_DBCall: function (OccupationId) {
        var objData = {};
        objData["OccupationHxId"] = OccupationId;
        objData["SocialHxId"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val();
        objData["commandType"] = "delete_sochxmischxoccupationhx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHxMiscHxOccupationHx");
    },
    SocialHxMiscHxTravelHxGridLoad: function (response) {
        $("#pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx").dataTable().fnDestroy();
        $("#pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx tbody").find("tr").remove();
        if (response.MiscHxTravelHxCount > 0) {
            var SocialHxMiscHxTravelHxJSONData = JSON.parse(response.SocialHxMiscHxTravelHx_JSON);
            $.each(SocialHxMiscHxTravelHxJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvSHxMiscHxTravelHx_row" + i);
                $row.attr("statusid", item.StatusId);
                $row.append('<td style="display:none;">' + item.TravelHxId + '</td><td><a class="btn btn-xs" href="javascript:void(0);" onclick="IA_TabacooScreening.TravelHxEdit(' + item.TravelHxId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn btn-xs" href="javascript:void(0);" onclick="IA_TabacooScreening.TravelHxDelete(' + item.TravelHxId + ', event);"   title="Delete Record"><i class="fa fa-close red"></i></a></td><td>' + (item.Location ? item.Location : "N/A") + '</td><td>' + (item.Comments ? item.Comments : "N/A") + '</td><td>' + ((item.FromDate) ? (utility.RemoveTimeFromDate(null, item.FromDate)) : "N/A") + '</td><td>' + ((item.Todate) ? (utility.RemoveTimeFromDate(null, item.Todate)) : "N/A") + '</td>');
                IA_TabacooScreening.markStatusActive('ulMiscChildStatus', item.StatusId, true);
                if (item.statusname == "Does Not Travel" && $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text().trim() == "Does Not Travel")
                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").addClass('disableAll');
                $("#pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx tbody").last().append($row);
            });
        }
        else {
            $("#pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #divSocialHxMiscHxTravelHxPaging").css("display", "none");
            $('#pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx').DataTable({
                "language": {
                    "emptyTable": "No Travel History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx'))
            ;
        else
            $("#pnlTabacooScreening #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
    },
    AddNewSocialHxTravelDetail: function () {
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #hfTravelDetailHxId").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxFromDate").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxToDate").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #txtTravelHxLocation").val('');
        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #txtTravelHxComments").val('');
    },
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle fill of SocialHx and it's childs as specified by SocialHxType


    loadSocialHxComponent: function (SocialHxType) {

        //Start 08-01-2016 Muhammad Arshad LoadMiscHx Tab
        if (SocialHxType != null && SocialHxType.toLowerCase().indexOf("miscellaneous") > -1) {

            IA_TabacooScreening.fillSocialHx(SocialHxType).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        IA_TabacooScreening.BindCurrentSocialHxSoapText(response, false);
                        if (response.socialHxLoad_JSON) {
                            var tabsData = JSON.parse(response.socialHxLoad_JSON);
                            IA_TabacooScreening.checkTabColor(tabsData);
                        }
                        //Begin 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                            /* Start 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                            //$('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").addClass("disableAll");
                            /* End 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                        }
                        //End 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                        if (typeof (response.SocialHxFill_JSON) != "undefined") {
                            var socialhx_detail = JSON.parse(response.SocialHxFill_JSON);
                            // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                            if (IA_TabacooScreening.isRemarkableFormload == false) {
                                socialhx_detail.SocialHxUnremarkable = "false";
                            }
                            // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                            var self = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx");
                            utility.bindMyJSONByName(true, socialhx_detail, false, self).done(function () {

                                IA_TabacooScreening.params.mode = "Edit";
                            });

                            IA_TabacooScreening.showParentData(socialhx_detail);
                            if (socialhx_detail.SocialHxUnremarkable.toLowerCase() != "true") {
                                if (SocialHxType.toLowerCase() == "miscellaneous_components") {

                                    var arrComponents = JSON.parse(response.socialHxMiscHxComponentLoad_JSON);
                                    var addedMiscComponents = response.IsCreatedOrModified;
                                    //if (IA_TabacooScreening.retainedComponentMisHx == "") {
                                    //    if (JSON.parse(response.SleepHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (arrComponents[obj].ComponentName.toLowerCase() == "sleep")
                                    //                    IA_TabacooScreening.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //    else if (JSON.parse(response.CaffeineIntakeHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (arrComponents[obj].ComponentName.toLowerCase() == "caffeine intake")
                                    //                    IA_TabacooScreening.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //    else if (JSON.parse(response.HousingHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (arrComponents[obj].ComponentName.toLowerCase() == "housing")
                                    //                    IA_TabacooScreening.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //    else if (JSON.parse(response.ExercisesHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (obj.ComponentName.toLowerCase() == "exercises")
                                    //                    IA_TabacooScreening.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //    else if (JSON.parse(response.OccupationHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (obj.ComponentName.toLowerCase() == "occupation")
                                    //                    IA_TabacooScreening.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //}
                                    IA_TabacooScreening.loadMiscHxComponents('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus", arrComponents, IA_TabacooScreening.retainedComponentMisHx, "MiscHxMainStatus", "", addedMiscComponents);
                                }
                                else {
                                    var self = null;
                                    var SocialHx_Child_Detail = null;
                                    if (SocialHxType.toLowerCase().indexOf("miscellaneous") > -1) {
                                        var DetailsSection = "";
                                        var currentJSON = "";

                                        if (SocialHxType.toLowerCase() == "miscellaneous_occupation") {
                                            DetailsSection = "div#OccupationDetails";
                                            currentJSON = response.OccupationHxFill_JSON;

                                            if (currentJSON != null) {
                                                var miscHx = JSON.parse(currentJSON);
                                                if (miscHx.length > 0) {
                                                    var miscHxId = miscHx[0].MiscHxId;
                                                    var tableName = "SocialHx_MiscHx_OccupationHx";
                                                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #occupationHistory").removeClass('hidden');
                                                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #occupationHistory").attr('onclick', 'IA_TabacooScreening.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');
                                                }
                                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails").getMyJSONByName();
                                            }
                                            IA_TabacooScreening.SocialHxMiscHxOccupationHxSearch();

                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_sleep") {
                                            DetailsSection = "div#SleepDetails";
                                            currentJSON = response.SleepHxFill_JSON;

                                            if (currentJSON != null) {
                                                var miscHx = JSON.parse(currentJSON);
                                                var miscHxId = miscHx[0].MiscHxId;
                                                var tableName = "SocialHx_MiscHx_SleepHx";
                                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #sleepHistory").removeClass('hidden');
                                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #sleepHistory").attr('onclick', 'IA_TabacooScreening.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');

                                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SleepDetails").getMyJSONByName();
                                            }
                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_exercises") {
                                            DetailsSection = "div#ExercisesDetails";
                                            currentJSON = response.ExercisesHxFill_JSON;

                                            if (currentJSON != null) {
                                                var miscHx = JSON.parse(currentJSON);
                                                var miscHxId = miscHx[0].MiscHxId;
                                                var tableName = "SocialHx_MiscHx_ExercisesHx";
                                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #exercisesHistory").removeClass('hidden');
                                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #exercisesHistory").attr('onclick', 'IA_TabacooScreening.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');

                                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ExercisesDetails").getMyJSONByName();
                                            }
                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_housing") {
                                            DetailsSection = "div#HousingDetails";
                                            currentJSON = response.HousingHxFill_JSON;

                                            if (currentJSON != null) {
                                                var miscHx = JSON.parse(currentJSON);
                                                var miscHxId = miscHx[0].MiscHxId;
                                                var tableName = "SocialHx_MiscHx_HousingHx";
                                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #housingHistory").removeClass('hidden');
                                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #housingHistory").attr('onclick', 'IA_TabacooScreening.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');

                                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #HousingDetails").getMyJSONByName();
                                            }

                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_caffeineintake") {
                                            DetailsSection = "div#CaffeineIntakDetails";
                                            currentJSON = response.CaffeineIntakeHxFill_JSON;
                                            if (currentJSON != null) {
                                                var miscHx = JSON.parse(currentJSON);
                                                var miscHxId = miscHx[0].MiscHxId;
                                                var tableName = "SocialHx_MiscHx_CaffeineIntakHx";
                                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #caffeineHistory").removeClass('hidden');
                                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #caffeineHistory").attr('onclick', 'IA_TabacooScreening.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');

                                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #CaffeineIntakDetails").getMyJSONByName();
                                            }
                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_travel") {
                                            DetailsSection = "div#divTravelDetailsHx";
                                            currentJSON = response.TravelHxFill_JSON;
                                            if (currentJSON != null) {
                                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx").getMyJSONByName();
                                            }
                                            IA_TabacooScreening.SocialHxMiscHxTravelHxSearch();
                                        }
                                        else {

                                        }

                                        var detailsJSON = '';
                                        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                            var statusId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscChildStatus > li.active").attr('id');
                                            if (statusId == null && miscHx != null) {
                                                statusId = miscHx[0].MiscChildStatus;
                                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType.toLowerCase());
                                                if (cachedJSON != '') {
                                                    detailsJSON = cachedJSON;
                                                    IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), currentJSON, DetailsSection, "#ulMiscChildStatus");
                                                    //.done(function () {
                                                    //    var childLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscChildStatus > li.active");
                                                    //    if (childLi.length == 0) {
                                                    //        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscChildStatus > li#" + statusId).addClass('active');
                                                    //    }
                                                    //});
                                                }
                                                else {
                                                    IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), currentJSON, DetailsSection, "#ulMiscChildStatus");
                                                }
                                            }
                                            else {
                                                IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), currentJSON, DetailsSection, "#ulMiscChildStatus");
                                            }
                                        }
                                        else {
                                            IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), currentJSON, DetailsSection, "#ulMiscChildStatus");
                                        }

                                        //Start 08-01-2015 Muhammad Arshad Serializing the Current Details form
                                        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  ' + DetailsSection).data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').serialize());
                                        //Start 08-01-2015 Muhammad Arshad Serializing the Current Details form

                                    }
                                    else {
                                        self = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx");
                                    }
                                    //if (firstLoad == true) {
                                    //    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMisStatus");
                                    //}
                                }

                            }
                            else {
                                var chkUnremarkable = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable")
                                IA_TabacooScreening.unRemarkableSocialHx(chkUnremarkable, "1");
                            }
                        }
                        else {
                            if (SocialHxType.toLowerCase() == "miscellaneous_occupation") {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == "miscellaneous_sleep") {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SleepDetails").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == "miscellaneous_travel") {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx").getMyJSONByName();
                            }
                            if (SocialHxType.toLowerCase() == "miscellaneous_exercises") {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ExercisesDetails").getMyJSONByName();
                            }
                            if (SocialHxType.toLowerCase() == "miscellaneous_housing") {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #HousingDetails").getMyJSONByName();
                            }
                            if (SocialHxType.toLowerCase() == "miscellaneous_caffeineintake") {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #CaffeineIntakDetails").getMyJSONByName();
                            }
                            if (SocialHxType.toLowerCase() == "miscellaneous_components") {
                                var arrComponents = JSON.parse(response.socialHxMiscHxComponentLoad_JSON);
                                IA_TabacooScreening.loadMiscHxComponents('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus", arrComponents, "", "MiscHxMainStatus", "");
                            }
                            if (SocialHxType == 'miscellaneous_occupation') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails");
                                var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails").getMyJSONByName();

                                }
                            }
                            else if (SocialHxType == 'miscellaneous_sleep') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails");
                                var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails").getMyJSONByName();
                                }
                            }
                            else if (SocialHxType == 'miscellaneous_exercises') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails");
                                var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails").getMyJSONByName();
                                }
                            }
                            else if (SocialHxType == 'miscellaneous_housing') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails");
                                var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails").getMyJSONByName();
                                }
                            }
                            else if (SocialHxType == 'miscellaneous_caffeineintake') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails");
                                var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            if (cachedJSON.RadCaffieneharmful == true) {
                                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", false);
                                            }
                                            else if (cachedJSON.RadCaffieneharmful == false) {
                                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", false);
                                            }
                                            else {
                                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
                                            }
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails").getMyJSONByName();
                                }
                            }
                            else if (SocialHxType == 'miscellaneous_travel') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx");
                                var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {

                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx").getMyJSONByName();
                                }
                            }
                        }

                        //Start//16/12/2015//Ahmad Raza//Serializing the form
                        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());
                        //End//16/12/2015//Ahmad Raza//Serializing the form
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                }
                else {

                }

            });
        }
        else {

            //   IA_TabacooScreening.isMiscellaneousTabTrigger = false;

            IA_TabacooScreening.loadSocialHxStatuses('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus", "GetTobaccoSmokingStatus", true, SocialHxType).done(function () {
                IA_TabacooScreening.fillSocialHx(SocialHxType).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (typeof (response.SocialHxFill_JSON) != "undefined") {
                            if (response.status != false) {
                                // Start 17/12/2015 Muhammad Irfan Checks if data of sub tabs of socialHx exists
                                IA_TabacooScreening.BindCurrentSocialHxSoapText(response, false);
                                var tabsData = JSON.parse(response.socialHxLoad_JSON);
                                if (tabsData[0].SocialHxId > 0) {
                                    IA_TabacooScreening.params.HxTypeId = tabsData[0].SocialHxId;
                                    if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").removeClass("disableAll");
                                    } else {
                                        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").addClass("disableAll");
                                    }

                                }
                                IA_TabacooScreening.checkTabColor(tabsData);

                                // End 17/12/2015 Muhammad Irfan Checks if data of sub tabs of socialHx exists

                                // Start 12/01/2016 Syed Zia,trigger first existed history tab click event

                                // End 12/01/2016 Syed Zia,Syed Zia,trigger first existed history tab click event


                                //Begin 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && IA_TabacooScreening.isFromTabTrigger) {
                                    IA_TabacooScreening.isFromTabTrigger = false;
                                    IA_TabacooScreening.triggerSocialHistoryTab();
                                    /* Start 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                                    // $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").addClass("disableAll");
                                    /* End 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                                }
                                //End 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date

                                var socialhx_detail = JSON.parse(response.SocialHxFill_JSON);

                                // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                                if (IA_TabacooScreening.isRemarkableFormload == false) {
                                    socialhx_detail.SocialHxUnremarkable = "false";
                                }
                                // end 13/01/2016 syed zia, for remarkable checked/ unchecked

                                var self = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx");
                                utility.bindMyJSONByName(true, socialhx_detail, false, self).done(function () {
                                    //Start//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225
                                    var upperDate = self.find('input[name*="SocialHxDate"]');

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
                                    }

                                    //End//02/02/2016//Ahmad Raza// changed the implementation way of setDate on datepicker for Bug # EMR-225

                                    IA_TabacooScreening.showParentData(socialhx_detail);

                                    IA_TabacooScreening.params.mode = "Edit";
                                });
                                if (socialhx_detail.SocialHxUnremarkable.toLowerCase() != "true") {
                                    var self = null;
                                    var SocialHx_Child_Detail = null;

                                    var socialHx = JSON.parse(response.SocialHxFill_JSON);
                                    var socialHxId = socialHx.SocialHxId;

                                    if (SocialHxType.toLowerCase() == "tobacco") {
                                        if (response.TobaccoHxFill_JSON != null) {
                                            //var socialHx = JSON.parse(response.SocialHxFill_JSON);
                                            //var socialHxId = socialHx.SocialHxId;
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Tobacco #aHistory").removeClass('hidden');
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Tobacco #aHistory").attr('onclick', 'IA_TabacooScreening.showSocialHxTobaccoHistory(' + socialHxId + ')');

                                        }
                                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
                                        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Tobacco', -1);
                                            if (cachedJSON != '') {
                                                utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus li#" + cachedJSON.StatusId).addClass("active");
                                                });
                                            }
                                            else {
                                                IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.TobaccoHxFill_JSON, "div#Tobacco", "#ulSmokingStatus");
                                            }
                                        }
                                        else {
                                            IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.TobaccoHxFill_JSON, "div#Tobacco", "#ulSmokingStatus");

                                        }
                                        //Start//16/12/2015//Ahmad Raza//Serializing the Tobacco form
                                        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').serialize());
                                        //End//16/12/2015//Ahmad Raza//Serializing the Tobacco form
                                        //IA_TabacooScreening.CacheTabJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSON();
                                        IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                                        IA_TabacooScreening.PreviousTab = 'tobacco';
                                        IA_TabacooScreening.enableDisableCounsellingTopic('tobacco');

                                    }
                                    else if (SocialHxType.toLowerCase() == "alcohol") {
                                        if (response.AlcoholHxFill_JSON != null) {
                                            var tableName = 'SocialHx_Alcohol';
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Alcohol #alcoholHistory").removeClass('hidden');
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #Alcohol #alcoholHistory").attr('onclick', 'IA_TabacooScreening.showSocialHxItemHistory(' + socialHxId + ', \'' + tableName + '\')');

                                        }
                                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol");
                                        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Alcohol', -1);
                                            if (cachedJSON != '') {
                                                utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Alcohol #ulAlcoholStatus li#" + cachedJSON.StatusId).addClass("active");
                                                });
                                            }
                                            else {
                                                IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.AlcoholHxFill_JSON, "div#Alcohol", "#ulAlcoholStatus");
                                            }
                                        }
                                        else {
                                            IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.AlcoholHxFill_JSON, "div#Alcohol", "#ulAlcoholStatus");
                                        }
                                        //Start//16/12/2015//Ahmad Raza//Serializing the Alcohol form
                                        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Alcohol').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #Alcohol').serialize());
                                        //End//16/12/2015//Ahmad Raza//Serializing the Alcohol form
                                        //IA_TabacooScreening.CacheTabJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSON();
                                        IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                                        IA_TabacooScreening.PreviousTab = 'alcohol';
                                        IA_TabacooScreening.enableDisableCounsellingTopic('alcohol');
                                    }
                                    else if (SocialHxType.toLowerCase() == "drug") {
                                        if (response.DrugAbuseFill_JSON != null) {
                                            var tableName = 'SocialHx_DrugAbuse';
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #DrugAbuse #drugHistory").removeClass('hidden');
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #DrugAbuse #drugHistory").attr('onclick', 'IA_TabacooScreening.showSocialHxItemHistory(' + socialHxId + ', \'' + tableName + '\')');

                                        }
                                        //$("#ddlDrugType option:selected")
                                        $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType option").removeAttr("selected");
                                        $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("clearSelection");
                                        $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("refresh");

                                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse");
                                        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('DrugAbuse', -1);
                                            if (cachedJSON != '') {
                                                utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                                    if (cachedJSON != '') {
                                                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val(cachedJSON.DrugType.split(','));
                                                    }
                                                    $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                                    $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect({
                                                        enableFiltering: true,
                                                        enableCaseInsensitiveFiltering: true,
                                                    });
                                                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#DrugAbuse #ulDrugStatus li#" + cachedJSON.StatusId).addClass("active");
                                                });
                                            }
                                            else {
                                                IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.DrugAbuseFill_JSON, "div#DrugAbuse", "#ulDrugStatus");
                                            }
                                        }
                                        else {
                                            IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.DrugAbuseFill_JSON, "div#DrugAbuse", "#ulDrugStatus");

                                        }
                                        $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect({
                                            //Start//Ahmad Raza// SelectAll and Search feature added in Type Multiselect
                                            includeSelectAllOption: true,
                                            enableFiltering: true,
                                            enableCaseInsensitiveFiltering: true,
                                            //End//Ahmad Raza// SelectAll and Search feature added in Type Multiselect

                                            // Start 16-12-2015 Muhammad Arshad showing details in Drug multiselect tooltip
                                            buttonTitle: function (options, select) {
                                                var buttonTitle = "";
                                                $.each(options, function (i, item) {
                                                    if (buttonTitle != "") {
                                                        buttonTitle += "," + $(item).attr("refvalue");
                                                    }
                                                    else {
                                                        buttonTitle += $(item).attr("refvalue");
                                                    }

                                                });

                                                return buttonTitle;
                                            }
                                            // End 16-12-2015 Muhammad Arshad showing details in Drug multiselect tooltip
                                        });
                                        //$('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").parent().find('[data-toggle="dropdown"]').tooltip({
                                        //    placement: 'right',
                                        //    container: 'body',
                                        //    title: function () {
                                        //        return $(this).attr('title');
                                        //    }
                                        //});



                                        /* Options List ToolTip text
                                        Author: Muhammad Azhar Shahzad
                                        Date: 17 Dec 2015*/
                                        $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").parent().find('.multiselect-container li').not('.filter, .group').tooltip({
                                            placement: 'right',
                                            container: 'body',
                                            title: function () {
                                                // getting Value of hover options
                                                var value = $(this).find('input').val();
                                                return $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType Option").map(function (index, element) {
                                                    if (element.value == value) {
                                                        return $(element).attr("refvalue");
                                                    }
                                                }).get(0);
                                            }
                                        });




                                        //End Azhar Changed
                                        //Start//17/12/2015//Ahmad Raza//Serializing the DrugAbuse form
                                        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #DrugAbuse').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #DrugAbuse').serialize());
                                        //End//17/12/2015//Ahmad Raza//Serializing the DrugAbuse form

                                        //IA_TabacooScreening.CacheTabJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSON();
                                        IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                                        IA_TabacooScreening.PreviousTab = 'drug';
                                    }
                                    else if (SocialHxType.toLowerCase() == "sexual") {
                                        if (response.DrugAbuseFill_JSON != null) {
                                            var tableName = 'SocialHx_SexualHx';
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #SexualHx #sexualHistory").removeClass('hidden');
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx  #SexualHx #sexualHistory").attr('onclick', 'IA_TabacooScreening.showSocialHxItemHistory(' + socialHxId + ', \'' + tableName + '\')');

                                        }

                                        //self = $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx");
                                        $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD option").removeAttr("selected");
                                        $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("clearSelection");
                                        $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("refresh");

                                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx");
                                        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Sexual', -1);
                                            if (cachedJSON != '') {
                                                utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                                    if (cachedJSON.RadSexualAbusedSexually == true) {
                                                        detailSection.find("#RadSexualYesAbusedSexually").prop("checked", true);
                                                    }
                                                    else {
                                                        if (detailSection.find("#RadSexualNoAbusedSexually").attr("disabled") != "disabled") {
                                                            detailSection.find("#RadSexualNoAbusedSexually").prop("checked", true);
                                                        }
                                                    }
                                                    if (cachedJSON.RadSexualPainWithIntercourse == true) {
                                                        detailSection.find("#RadSexualYesPainWithIntercourse").prop("checked", true);
                                                    }
                                                    else {
                                                        if (detailSection.find("#RadSexualNoPainWithIntercourse").attr("disabled") != "disabled") {
                                                            detailSection.find("#RadSexualNoPainWithIntercourse").prop("checked", true);
                                                        }
                                                    }
                                                    if (cachedJSON.RadSexualPregnant == true)
                                                        detailSection.find("#RadSexualYesPregnant").prop("checked", true);
                                                    else
                                                        detailSection.find("#RadSexualNoPregnant").prop("checked", true);

                                                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus li#" + cachedJSON.StatusId).addClass("active");
                                                });
                                            }
                                            else {
                                                IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.SexualHxFill_JSON, "div#SexualHx", "#ulSexualStatus");
                                            }
                                        }
                                        else {
                                            IA_TabacooScreening.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.SexualHxFill_JSON, "div#SexualHx", "#ulSexualStatus");
                                        }


                                        $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect({
                                            //Start//Ahmad Raza// SelectAll and Search feature added in Type Multiselect
                                            includeSelectAllOption: true,
                                            enableFiltering: true,
                                            enableCaseInsensitiveFiltering: true,
                                            //End//Ahmad Raza// SelectAll and Search feature added in Type Multiselect
                                            //Start//17/12/2015//Ahmad Raza//Multiselect implimented in SocialHx's Sexual tab
                                            listTitle: function (options, select) {
                                                var listTitle = "";
                                                $.each(options, function (i, item) {
                                                    if (listTitle != "") {
                                                        listTitle += "," + $(item).attr("refvalue");
                                                    }
                                                    else {
                                                        listTitle += $(item).attr("refvalue");
                                                    }

                                                });
                                                //Start//17/12/2015//Ahmad Raza//Serializing the Sexual form
                                                //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #SexualHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #SexualHx').serialize());
                                                //End//17/12/2015//Ahmad Raza//Serializing the Sexual form
                                                return listTitle;
                                            }
                                            //End//17/12/2015//Ahmad Raza//Multiselect implimented in SocialHx's Sexual tab
                                        });
                                        //Start//16/12/2015//Ahmad Raza//Serializing the Sexual form
                                        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #SexualHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #SexualHx').serialize());
                                        //End//16/12/2015//Ahmad Raza//Serializing the Sexual form

                                        //IA_TabacooScreening.CacheTabJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSON();
                                        IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                                        IA_TabacooScreening.PreviousTab = 'sexual';
                                        if ($('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualExposedToSTD option:selected').val() == "") {
                                            var objSTD = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD');
                                            objSTD.attr("disabled", "disabled");
                                            objSTD.find("option:selected").removeAttr("selected");
                                            objSTD.multiselect("disable");
                                            objSTD.multiselect("clearSelection");
                                        }
                                    }
                                    else if (SocialHxType.toLowerCase() == "miscellaneous") {

                                    }
                                    else {
                                        self = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx");
                                    }
                                }
                                else {
                                    var chkUnremarkable = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable")
                                    IA_TabacooScreening.unRemarkableSocialHx(chkUnremarkable, "1");
                                }
                                //Start//16/12/2015//Ahmad Raza//Serializing the form
                                $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());
                                //End//16/12/2015//Ahmad Raza//Serializing the form
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        }
                        else {


                            var $row = $('<tr/>');
                            $row.append('<td style="display:none;"></td><td>&nbsp;</td><td>No Known Social History</td><td></td>');
                            $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").html($row);
                            $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #divSwitch").addClass('disableAll');

                            IA_TabacooScreening.showParentData(null);
                            //if (SocialHxType.toLowerCase() == "tobacco") {
                            //    $('#' + IA_TabacooScreening.params.PanelID + ' #ulSmokingStatus li:first').trigger('click');
                            //} else if (SocialHxType.toLowerCase() == "alcohol") {
                            //    $('#' + IA_TabacooScreening.params.PanelID + ' #ulAlcoholStatus li:first').trigger('click');
                            //} else if (SocialHxType.toLowerCase() == "drug") {
                            //    $('#' + IA_TabacooScreening.params.PanelID + ' #ulDrugStatus li:first').trigger('click');
                            //} else if (SocialHxType.toLowerCase() == "sexual") {
                            //    $('#' + IA_TabacooScreening.params.PanelID + ' #ulSexualStatus li:first').trigger('click');
                            //}
                            $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType,div#SexualHx #ddlSexualSTD").multiselect({

                            });

                            if (SocialHxType.toLowerCase() == 'tobacco') {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == 'alcohol') {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == 'drug') {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == 'sexual') {
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                            }

                            if (SocialHxType.toLowerCase() == 'tobacco') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Tobacco', -1);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                                }

                            }
                            else if (SocialHxType == 'alcohol') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol");
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Alcohol', -1);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Alcohol #ulAlcoholStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                }
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                            }
                            else if (SocialHxType == 'drug') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse");
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('DrugAbuse', -1);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            if (cachedJSON != '') {
                                                $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val(cachedJSON.DrugType.split(','));
                                            }
                                            $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                            $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect({
                                                enableFiltering: true,
                                                enableCaseInsensitiveFiltering: true,
                                            });
                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#DrugAbuse #ulDrugStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                                }
                            }

                            else if (SocialHxType == 'sexual') {
                                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx");
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Sexual', -1);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            if (cachedJSON.RadSexualAbusedSexually == true) {
                                                detailSection.find("#RadSexualYesAbusedSexually").prop("checked", true);
                                            }

                                            else if (cachedJSON.RadSexualAbusedSexually == false) {
                                                if (detailSection.find("#RadSexualNoAbusedSexually").attr("disabled") != "disabled") {
                                                    detailSection.find("#RadSexualNoAbusedSexually").prop("checked", true);
                                                }
                                            }
                                            else if (cachedJSON.RadSexualAbusedSexually == "-1") {
                                                detailSection.find("#RadSexualYesAbusedSexually").prop("checked", false);
                                                detailSection.find("#RadSexualNoAbusedSexually").prop("checked", false);
                                            }

                                            if (cachedJSON.RadSexualPainWithIntercourse == true) {
                                                detailSection.find("#RadSexualYesPainWithIntercourse").prop("checked", true);
                                            }
                                            else if (cachedJSON.RadSexualPainWithIntercourse == false) {
                                                if (detailSection.find("#RadSexualNoPainWithIntercourse").attr("disabled") != "disabled") {
                                                    detailSection.find("#RadSexualNoPainWithIntercourse").prop("checked", true);
                                                }
                                            }
                                            else if (cachedJSON.RadSexualPainWithIntercourse == "-1") {
                                                detailSection.find("#RadSexualYesPainWithIntercourse").prop("checked", false);
                                                detailSection.find("#RadSexualNoPainWithIntercourse").prop("checked", false);
                                            }
                                            if (cachedJSON.RadSexualPregnant == true)
                                                detailSection.find("#RadSexualYesPregnant").prop("checked", true);

                                            else if (cachedJSON.RadSexualPregnant == false)
                                                detailSection.find("#RadSexualNoPregnant").prop("checked", true);
                                            else if (cachedJSON.RadSexualPregnant == "-1") {
                                                detailSection.find("#RadSexualYesPregnant").prop("checked", false);
                                                detailSection.find("#RadSexualNoPregnant").prop("checked", false);
                                            }

                                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                                }
                            }

                        }
                    }
                    else {

                        /* Start 10/12/2015 Muhammad Irfan If SocialHx not saved then trigger the first list item of status */

                        //if (SocialHxType.toLowerCase() == "tobacco") {
                        //    $('#' + IA_TabacooScreening.params.PanelID + ' #ulSmokingStatus li:first').trigger('click');
                        //} else if (SocialHxType.toLowerCase() == "alcohol") {
                        //    $('#' + IA_TabacooScreening.params.PanelID + ' #ulAlcoholStatus li:first').trigger('click');
                        //} else if (SocialHxType.toLowerCase() == "drug") {
                        //    $('#' + IA_TabacooScreening.params.PanelID + ' #ulDrugStatus li:first').trigger('click');
                        //} else if (SocialHxType.toLowerCase() == "sexual") {
                        //    $('#' + IA_TabacooScreening.params.PanelID + ' #ulSexualStatus li:first').trigger('click');
                        //}

                        /* End 10/12/2015 Muhammad Irfan If SocialHx not saved then trigger the first list item of status */

                        $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType,div#SexualHx #ddlSexualSTD").multiselect({

                        });

                        //$('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect({

                        //});
                    }

                });
            });
        }
        //End 08-01-2016 Muhammad Arshad LoadMiscHx Tab

    },

    checkTabColor: function (tabsData) {
        if (tabsData[0].bAlcoholExist == 'True') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').addClass('successLight');
            IA_TabacooScreening.bAlcoholExist = true;

        }
        else {
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel.length > 0) {
                    if (!$('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').hasClass('successLight')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').addClass('successLight');
                        IA_TabacooScreening.bAlcoholExist = true;
                    }
                }
            }
            else {
                $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').removeClass('successLight');
                IA_TabacooScreening.bAlcoholExist = false;
            }
            //$('#' + IA_TabacooScreening.params.PanelID + ' #ulAlcoholStatus li:first').trigger('click');
        }
        if (tabsData[0].bDrugExist == 'True') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').addClass('successLight');
            IA_TabacooScreening.bDrugExist = true;
        }
        else {
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel.length > 0) {
                    if (!$('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').hasClass('successLight')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                        IA_TabacooScreening.bDrugExist = true;
                    }
                }
            }
            else {
                $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').removeClass('successLight');
                IA_TabacooScreening.bDrugExist = false;
            }
        }
        if (tabsData[0].bSexualExist == 'True') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').addClass('successLight');
            IA_TabacooScreening.bSexualExist = true;
        }
        else {
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel.length > 0) {
                    if (!$('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').hasClass('successLight')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').addClass('successLight');
                        IA_TabacooScreening.bSexualExist = true;
                    }
                }
            }
            else {
                $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').removeClass('successLight');
                IA_TabacooScreening.bSexualExist = false;
            }

            //$('#' + IA_TabacooScreening.params.PanelID + ' #ulSexualStatus li:first').trigger('click');
        }
        if (tabsData[0].bTobaccoExist == 'True') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').addClass('successLight');
            IA_TabacooScreening.bTobaccoExist = true;
        }
        else {
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel.length > 0) {
                    if (!$('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').hasClass('successLight')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').addClass('successLight');
                        IA_TabacooScreening.bTobaccoExist = true;
                    }
                }
            }
            else {
                $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').removeClass('successLight');
                IA_TabacooScreening.bTobaccoExist = false;
            }
        }

        if (tabsData[0].bMiscHxExist == 'True') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').addClass('successLight');
            IA_TabacooScreening.bMiscHxExist = true;
        }
        else {
            $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').removeClass('successLight');
            IA_TabacooScreening.bMiscHxExist = false;
            IA_TabacooScreening.changeTabColor("miscellaneous_components");
        }
    },

    showParentData: function (socialhx_detail) {
        IA_TabacooScreening.date = $('#' + IA_TabacooScreening.params.PanelID + " #dtSocialHxDate").val();

        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
            $('#' + IA_TabacooScreening.params.PanelID + " #dtSocialHxDate").val(Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxDate);
        }

        if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null && Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialComments != '') {
            $('#' + IA_TabacooScreening.params.PanelID + " #txtSocialComments").val(Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialComments);
        }
        else {
            if (socialhx_detail != null) {
                $('#' + IA_TabacooScreening.params.PanelID + " #txtSocialComments").val(socialhx_detail.SocialComments);
            }
            IA_TabacooScreening.overallComments = $('#' + IA_TabacooScreening.params.PanelID + " #txtSocialComments").val();
        }
        if (socialhx_detail != null) {
            IA_TabacooScreening.unremarkable = socialhx_detail.SocialHxUnremarkable == "False" || socialhx_detail.SocialHxUnremarkable == null ? false : true;;
        }
        else {
            IA_TabacooScreening.unremarkable = false;
        }

        if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked", Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable);

            if (Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable) {
                var chkUnremarkable = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable")
                IA_TabacooScreening.unRemarkableSocialHx(chkUnremarkable, "1");
            }
        }
    },
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle fill of SocialHx and it's childs as specified by SocialHxType

    loadSocialHx: function (SocialHxType, isLoadNew) {

        var currentTab = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
        currentTab = (typeof currentTab == "undefined") ? "" : currentTab;
        var DataExists = IA_TabacooScreening.isDetailExists(currentTab.toLowerCase());

        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && IA_TabacooScreening.socialHxJSON != '') {
            IA_TabacooScreening.cachePrevTabData(currentTab);
        }

        setTimeout(function () {
            LastSocialHx = new Object();
            LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
            LastSocialHx["SocialHxType"] = $('#' + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
        }, 100);



        if (IA_TabacooScreening.bIsTriggerManually) {
            isLoadNew = IA_TabacooScreening.bIsTriggerManually;
            IA_TabacooScreening.bIsTriggerManually = false;

        }

        if (isLoadNew == true) {
            IA_TabacooScreening.loadSocialHxComponent(SocialHxType);
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
        }
        else {
            if (DataExists != false) {
                if (EMRUtility.compareFormDataWithSerialized(IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx')) {
                    //Start 12-07-2016 Humaira Yousaf
                    if (IA_TabacooScreening.isSaved == true) {
                        IA_TabacooScreening.isSaved = false;
                        var socialHxTypeCurrentTab = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                        IA_TabacooScreening.loadSocialHxComponent(socialHxTypeCurrentTab);
                        IA_TabacooScreening.loadSocialHxComponent(SocialHxType);
                        if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                        }
                    }
                    else if (IA_TabacooScreening.CacheTabJSON != '') {
                        var isChanged = false;
                        if (IA_TabacooScreening.PreviousTab == "tobacco") {
                            if (IA_TabacooScreening.CacheTabJSON != $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSON()) {
                                isChanged = true;
                            }
                        }
                        else if (IA_TabacooScreening.PreviousTab == "alcohol") {
                            if (IA_TabacooScreening.CacheTabJSON != $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSON()) {
                                isChanged = true;
                            }
                        }
                        else if (IA_TabacooScreening.PreviousTab == "drug") {
                            if (IA_TabacooScreening.CacheTabJSON != $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSON()) {
                                isChanged = true;
                            }
                        }
                        else if (IA_TabacooScreening.PreviousTab == "sexual") {
                            if (IA_TabacooScreening.CacheTabJSON != $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSON()) {
                                isChanged = true;
                            }
                        }
                        else {
                            isChanged = false;
                        }
                        if (isChanged == true) {
                            utility.myConfirm('12', function () {
                                var socialHxTypeCurrent = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                                IA_TabacooScreening.socialHxSave(socialHxTypeCurrent, false);

                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                                IA_TabacooScreening.loadSocialHxComponent(SocialHxType);
                                BackgroundLoaderShow(true);

                            }, function () {

                                var socialHxTypeCurrentTab = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                                IA_TabacooScreening.loadSocialHxComponent(socialHxTypeCurrentTab);
                                IA_TabacooScreening.loadSocialHxComponent(SocialHxType);
                                if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                                }
                            });
                        }
                        else {
                            var socialHxTypeCurrentTab = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                            IA_TabacooScreening.loadSocialHxComponent(socialHxTypeCurrentTab);
                            IA_TabacooScreening.loadSocialHxComponent(SocialHxType);
                            if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                            }
                        }

                    }
                    else {
                        var socialHxTypeCurrentTab = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                        IA_TabacooScreening.loadSocialHxComponent(socialHxTypeCurrentTab);
                        IA_TabacooScreening.loadSocialHxComponent(SocialHxType);
                        if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                        }
                    }
                    //End 12-07-2016 Humaira Yousaf
                }
                else {
                    //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Alcohol').data('serialize', null);
                    //IA_TabacooScreening.loadSocialHx(SocialHxType); IA_TabacooScreening.selectTabColor(SocialHxType);
                    IA_TabacooScreening.loadSocialHxComponent(SocialHxType);

                    if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                    }

                }
            }
            else {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                IA_TabacooScreening.loadSocialHxComponent(SocialHxType);
            }
        }



        //End//22/12/2015//Ahmad Raza//Implemented prompt problem if user changed but didn't saved data and want to go to another tab

    },

    cachePrevTabData: function (socialType) {
        var dfd = $.Deferred();
        socialType = socialType.toLowerCase();

        if (socialType == 'miscellaneous') {
            socialType += "_" + $('#' + IA_TabacooScreening.params.PanelID + " #ulMiscStatus > li.active > a").text().replace(" ", "").trim();
            socialType = socialType.toLowerCase();
        }

        if (socialType == 'tobacco') {
            $.when(IA_TabacooScreening.cacheTobaccoTabData(socialType)).then(function () {
                dfd.resolve();
            });
        }
        else if (socialType == 'alcohol') {
            $.when(IA_TabacooScreening.cacheAlcoholTabData(socialType)).then(function () {
                dfd.resolve();
            });
        }
        else if (socialType == 'drug' || socialType == 'drug abuse') {
            $.when(IA_TabacooScreening.cacheDrugTabData(socialType)).then(function () {
                dfd.resolve();
            });
        }
        else if (socialType == 'sexual' || socialType == 'sexual hx') {
            $.when(IA_TabacooScreening.cacheSexualTabData(socialType)).then(function () {
                dfd.resolve();
            });
        }
        else if (socialType == "miscellaneous_occupation" || socialType == "miscellaneous_travel" || socialType == "miscellaneous_sleep" || socialType == "miscellaneous_exercises" || socialType == "miscellaneous_housing" || socialType == "miscellaneous_caffeineintake") {
            $.when(IA_TabacooScreening.markStatusActive("ulMiscStatus", $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id'))).then(function () {
                dfd.resolve();
            });
        }

        return dfd;
    },
    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle active state of current li item on the basis of li's Id
    markStatusActive: function (ulId, liId, TravelOrOcupation) {
        var dfd = $.Deferred();
        if (ulId != null && ulId != "") {
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && IA_TabacooScreening.socialHxJSON != '') {
                var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                var socialTypeMiscText = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active a").text().replace(" ", "").trim();
                var statusId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscChildStatus > li.active").attr('id');
                var updatedJSON = '';
                var detailsSection = '';
                if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Occupation') {
                    updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Sleep') {
                    updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Exercises') {
                    updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Housing') {
                    updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Caffeine Intake') {
                    updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Travel') {
                    updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx").getMyJSONByName();
                }
                if (updatedJSON != '' && IA_TabacooScreening.socialHxJSON != updatedJSON) {
                    if (socialTypeMiscId != null && statusId != null) {
                        $.when(IA_TabacooScreening.cacheSocialHxJSON(socialTypeMiscId, statusId, updatedJSON, "miscellaneous_" + socialTypeMiscText)).then(function () {
                            dfd.resolve();
                        });
                        IA_TabacooScreening.SetIsLast(socialTypeMiscId, statusId, "miscellaneous_" + socialTypeMiscText);
                    }
                    else {
                        dfd.resolve();
                    }
                }
                else {
                    var json = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails").getMyJSONByName();
                    //var detailsSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails").clone();
                    //$(detailsSection).resetAllControls(null);
                    var emptyJson = $(detailsSection).getMyJSONByName();
                    if (updatedJSON != '' && statusId != null && statusId > 0 && json != '') {
                        $.when(IA_TabacooScreening.cacheSocialHxJSON(socialTypeMiscId, statusId, updatedJSON, "miscellaneous_" + socialTypeMiscText)).then(function () {
                            dfd.resolve();
                        });
                        IA_TabacooScreening.SetIsLast(socialTypeMiscId, statusId, "miscellaneous_" + socialTypeMiscText);
                    }
                    else {
                        dfd.resolve();
                    }
                }
            }
            else {
                dfd.resolve();
            }
            if (TravelOrOcupation) {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous ul#" + ulId + " li").each(function (i, item) {
                    if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                        if ($(this).hasClass("active") == false) {
                            $(this).addClass("active");
                        }
                    }
                });
            }
            else {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous ul#" + ulId + " li").each(function (i, item) {
                    if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                        if ($(this).hasClass("active") == false) {
                            $(this).addClass("active");
                        }
                    }
                    else {
                        $(this).removeClass("active");
                    }
                });
            }


            //to clear fields on tab select
            $('#' + IA_TabacooScreening.params.PanelID + ' #txtHousingPresent').val('');
            $('#' + IA_TabacooScreening.params.PanelID + ' #txtHousingPast').val('');
            $('#' + IA_TabacooScreening.params.PanelID + ' #txtHousingComments').val('');
            $('#' + IA_TabacooScreening.params.PanelID + ' #txtSleepHours').val('');
        }
        else {
            dfd.resolve();
        }
        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());
        return dfd;
    },

    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle load of SocialHx Miscellanous and it's childs Status as specified by MiscTab
    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
    loadMiscHxTab: function (MiscHxType, liId, firstLoad) {
        //Start 28-10-2016 Humaira Yousaf for misc tab
        IA_TabacooScreening.markStatusActive('ulMiscStatus', liId);
        //End 28-10-2016 Humaira Yousaf for misc tab
        if (MiscHxType != null && MiscHxType != "") {
            var DetailsDivId = "";
            var StatusType = "";
            var StatusCtrl = '#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus";


            if (MiscHxType.toLowerCase() == "occupation") {
                BackgroundLoaderShow(true);
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Occupation Status");
                DetailsDivId = "OccupationDetails";
                StatusType = "occupation";
                MDVisionService.lookups("getOccupationStatus", true).done(function (result) {
                    result = JSON.parse(result['getOccupationStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    IA_TabacooScreening.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    IA_TabacooScreening.loadSocialHx('miscellaneous_occupation', true);
                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").prop('checked', true);
                    IA_TabacooScreening.EnableDisableToDateOccupation();
                    BackgroundLoaderShow(false);
                });

            }
            else if (MiscHxType.toLowerCase() == "sleep") {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Sleeping Status");
                DetailsDivId = "SleepDetails";
                BackgroundLoaderShow(true);
                StatusType = "sleep";
                MDVisionService.lookups("getSleepHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getSleepHxStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    IA_TabacooScreening.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    IA_TabacooScreening.loadSocialHx('miscellaneous_sleep', true);
                    BackgroundLoaderShow(false);
                });

            }
            else if (MiscHxType.toLowerCase() == "exercises") {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Exercises Status");
                DetailsDivId = "ExercisesDetails";
                StatusType = "exercises";
                BackgroundLoaderShow(true);
                MDVisionService.lookups("getExercisesHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getExercisesHxStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    IA_TabacooScreening.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    IA_TabacooScreening.loadSocialHx('miscellaneous_exercises', true);
                    BackgroundLoaderShow(false);
                });

            }
            else if (MiscHxType.toLowerCase() == "housing") {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Housing Status");
                DetailsDivId = "HousingDetails";
                StatusType = "housing";
                BackgroundLoaderShow(true);
                MDVisionService.lookups("getHousingHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getHousingHxStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    IA_TabacooScreening.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    IA_TabacooScreening.loadSocialHx('miscellaneous_housing', true);
                    BackgroundLoaderShow(false);
                });



            }
            else if (MiscHxType.toLowerCase() == "caffeine intake") {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Caffeine Status");
                DetailsDivId = "CaffeineIntakDetails";
                StatusType = "caffeine intake";

                //Start 11/01/2016 Syed Zia Code to  Save/Update MiscHx Caffeine Intake Hx
                BackgroundLoaderShow(true);
                MDVisionService.lookups("getCaffeineIntakeHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getCaffeineIntakeHxStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    IA_TabacooScreening.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    IA_TabacooScreening.loadSocialHx('miscellaneous_caffeineintake', true);

                    BackgroundLoaderShow(false);
                });

                //Start 11/01/2016 Syed Zia Code to  Save/Update MiscHx Caffeine Intake Hx
            }
            else if (MiscHxType.toLowerCase() == "travel") {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Status");
                DetailsDivId = "divTravelDetailsHx";
                StatusType = "Travel";
                BackgroundLoaderShow(true);
                MDVisionService.lookups("getTravelHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getTravelHxStatus']);
                    IA_TabacooScreening.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    IA_TabacooScreening.loadSocialHx('miscellaneous_travel', true);

                    BackgroundLoaderShow(false);
                });
            }
            else if (MiscHxType.toLowerCase() == "miscellaneous_components") {
                BackgroundLoaderShow(true);
                IA_TabacooScreening.loadSocialHx('miscellaneous_components', true);
                BackgroundLoaderShow(false);
            }



            //Show respective details area
            var mainStatuUlId = "ulMiscStatus";
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div[id*='Details']").each(function (i, item) {
                if ($(this).attr("id") != null && $(this).attr("id").toLowerCase() == DetailsDivId.toLowerCase()) {

                    //Start//1-19-2016//Abid Ali/ Disable elements on first load of miscellnous
                    if (firstLoad == true) {
                        //$(this).find('input, textarea, button').each(function (index, item) {
                        //    $(item).attr('disabled', 'disabled');
                        //});
                        $(this).css("display", "none");
                        //Hide child status li controls
                        $(StatusCtrl).css("display", "none");
                        $('#headingChildMiscStatus').text("Miscellaneous Status");
                        // unselect all main status
                        IA_TabacooScreening.markStatusActive(mainStatuUlId, null);
                    }
                    else {
                        //$(this).find('input, textarea, button').each(function (index, item) {
                        //    $(item).removeAttr('disabled');
                        //});
                        $(this).css("display", "");
                        $(StatusCtrl).css("display", "");
                    }
                    //End//1-19-2016//Abid Ali/ Disable elements on first load of miscellnous
                }
                else {
                    $(this).css("display", "none");
                }
            });
        }
    },

    //Author: Muhammad Arshad
    //Date: 01-13-2016
    //This function will handle fill of MiscHx Components
    loadMiscHxComponents: function (Crtl, arrComponents, currentLiClick, ParentDiv, StatusType, addedMiscComponents) {
        var l = $(Crtl);

        l.empty();

        var currentLiClass;
        var isFirstLi = true;

        //Start/01-20-2016/Abid Ali// for saving active class li id and component name
        var activeLiId;
        var activeLiComponentName;
        //End/01-20-2016/Abid Ali// for saving active class li id and component name

        $.each(arrComponents, function (j, item) {
            if (item.ComponentId != "") {
                if (isFirstLi == true) {
                    currentLiClass = '';

                    //Start/01-20-2016/Abid Ali// for assign active class li id and component name
                    activeLiId = item.ComponentId;
                    activeLiComponentName = item.ComponentName.toLowerCase();
                    //End/01-20-2016/Abid Ali// for assign active class li id and component name

                    isFirstLi = false;
                }
                else {
                    currentLiClass = "";
                }

                var onClick = "";
                //Start 28-10-2016 Humaira Yousaf for misc tab
                onClick = " IA_TabacooScreening.loadMiscHxTab('" + item.ComponentName.toLowerCase() + "', '" + item.ComponentId + "')";
                //End 28-10-2016 Humaira Yousaf for misc tab

                if (l.find('.active').length > 0) {
                    if (!(item.ComponentName.toLowerCase() == "travel" && globalAppdata["isTransPubHealthAgCaseReporting"] && globalAppdata["isTransPubHealthAgCaseReporting"].toLowerCase() == "false"))
                        l.append('<li id="' + item.ComponentId + '" ' + currentLiClass + ' onclick="' + onClick + '" value=' + item.ComponentId + ' refValue="' + item.ComponentId + '"><a href="javascript:void(0)">' + item.ComponentName + ' </a></li>');
                }
                else {
                    if (addedMiscComponents && addedMiscComponents.indexOf(item.ComponentName) != -1) {
                        if (!(item.ComponentName.toLowerCase() == "travel" && globalAppdata["isTransPubHealthAgCaseReporting"] && globalAppdata["isTransPubHealthAgCaseReporting"].toLowerCase() == "false"))
                            l.append('<li class="active" id="' + item.ComponentId + '" ' + currentLiClass + ' onclick="' + onClick + '" value=' + item.ComponentId + ' refValue="' + item.ComponentId + '"><a href="javascript:void(0)">' + item.ComponentName + ' </a></li>');
                    }
                    else {
                        if (!(item.ComponentName.toLowerCase() == "travel" && globalAppdata["isTransPubHealthAgCaseReporting"] && globalAppdata["isTransPubHealthAgCaseReporting"].toLowerCase() == "false"))
                            l.append('<li id="' + item.ComponentId + '" ' + currentLiClass + ' onclick="' + onClick + '" value=' + item.ComponentId + ' refValue="' + item.ComponentId + '"><a href="javascript:void(0)">' + item.ComponentName + ' </a></li>');
                    }
                }

                //  l.append('<li id="' + item.ComponentId + '" ' + currentLiClass + ' onclick="' + onClick + '" value=' + item.ComponentId + ' refValue="' + item.ComponentId + '"><a href="javascript:void(0)">' + item.ComponentName + ' </a></li>');
            }
        });

        //Start/ 19/01/2016 Abid Ali,Remove Active Class from Misc and MiscChilds if tab is first time triggered.
        if (IA_TabacooScreening.isMiscellaneousTabTrigger == false) {
            //IA_TabacooScreening.loadMiscHxTab(activeLiComponentName, activeLiId, true);
            //set isMiscellaneousTabTrigger = true;
            //$(Crtl).find("li:first-child").trigger("click");
            IA_TabacooScreening.isMiscellaneousTabTrigger = true;
            l.find('li.active').trigger('click');
        }
            //End/ 19/01/2016 Abid Ali,Remove Active Class from Misc and MiscChilds if tab is first time triggered.
        else {
            if (currentLiClick != "") {
                $(Crtl).find("#" + currentLiClick).trigger("click");
            }
            else {
                if ($(Crtl).find("li.active").length > 0) {
                    $(Crtl).find("li.active").trigger("click");
                }
                else {
                    if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                        var found = false;
                        for (var a = 0; a < 5; a++) {
                            if (a == 0) {
                                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel.length > 0) {
                                    $.each(($(Crtl).find("li")), function (i, item) {
                                        if ($(item).find("a").text().toLowerCase().indexOf("occupation") > -1) {
                                            if (!$(item).hasClass("active")) {
                                                $(item).addClass("active");
                                            }
                                            IA_TabacooScreening.ClearStatusLiAndDetailSection();
                                            $(item).trigger("click");
                                        }
                                    });
                                    break;
                                }

                            }
                            else if (a == 1) {
                                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel.length > 0) {
                                    $.each(($(Crtl).find("li")), function (i, item) {
                                        if ($(item).find("a").text().toLowerCase().indexOf("sleep") > -1) {
                                            if (!$(item).hasClass("active")) {
                                                $(item).addClass("active");
                                            }
                                            IA_TabacooScreening.ClearStatusLiAndDetailSection();
                                            $(item).trigger("click");
                                        }
                                    });
                                    break;
                                }

                            }
                            else if (a == 2) {
                                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel.length > 0) {
                                    $.each(($(Crtl).find("li")), function (i, item) {
                                        if ($(item).find("a").text().toLowerCase().indexOf("exercises") > -1) {
                                            if (!$(item).hasClass("active")) {
                                                $(item).addClass("active");
                                            }
                                            IA_TabacooScreening.ClearStatusLiAndDetailSection();
                                            $(item).trigger("click");
                                        }
                                    });
                                    break;
                                }
                            }
                            else if (a == 3) {
                                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel.length > 0) {
                                    $.each(($(Crtl).find("li")), function (i, item) {
                                        if ($(item).find("a").text().toLowerCase().indexOf("housing") > -1) {
                                            if (!$(item).hasClass("active")) {
                                                $(item).addClass("active");
                                            }
                                            IA_TabacooScreening.ClearStatusLiAndDetailSection();
                                            $(item).trigger("click");
                                        }
                                    });
                                    break;
                                }
                            }
                            else if (a == 4) {
                                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel.length > 0) {
                                    $.each(($(Crtl).find("li")), function (i, item) {
                                        if ($(item).find("a").text().toLowerCase().indexOf("caffeine") > -1) {
                                            if (!$(item).hasClass("active")) {
                                                $(item).addClass("active");
                                            }
                                            IA_TabacooScreening.ClearStatusLiAndDetailSection();
                                            $(item).trigger("click");
                                        }
                                    });
                                    break;
                                }
                            }
                            else if (a == 5) {
                                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel.length > 0) {
                                    $.each(($(Crtl).find("li")), function (i, item) {
                                        if ($(item).find("a").text().toLowerCase().indexOf("travel") > -1) {
                                            if (!$(item).hasClass("active")) {
                                                $(item).addClass("active");
                                            }
                                            IA_TabacooScreening.ClearStatusLiAndDetailSection();
                                            $(item).trigger("click");
                                        }
                                    });
                                    break;
                                }
                            }
                        }

                    }
                    else {
                        IA_TabacooScreening.ClearStatusLiAndDetailSection();
                    }

                    //$(Crtl).find("li:first-child").trigger("click");
                }
            }

        }
        setTimeout(function () {
            LastSocialHx = new Object();
            LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
            LastSocialHx["SocialHxType"] = $('#' + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
        });



    },
    ClearStatusLiAndDetailSection: function () {
        $('#' + IA_TabacooScreening.params.PanelID + " #ulMiscChildStatus").html("");
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#OccupationDetails').css("display", "none");
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#ExercisesDetails').css("display", "none");
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#HousingDetails').css("display", "none");
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#CaffeineIntakDetails').css("display", "none");
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#SleepDetails').css("display", "none");
    },

    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle fill of child status on basis of StatusType
    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
    loadMiscChildStatus: function (Crtl, arrStatus, currentLiClick, ParentDiv, StatusType, firstLoad) {
        var l = $(Crtl);

        l.empty();

        var currentLiClass;
        var isFirstLi = true;
        //Start/ 19/01/2016 Abid Ali check for first load  of Misctab
        if (firstLoad == true) {
            // Do Not load chile status components
        }
            //End/ 19/01/2016 Abid Ali check for first load  of Misctab
        else {
            $.each(arrStatus, function (j, item) {
                if (item.Value != "") {
                    if (isFirstLi == true) {
                        currentLiClass = '';
                        isFirstLi = false;
                    }
                    else {
                        currentLiClass = "";
                    }
                    var onClick = ""; //currentLiClick != null && currentLiClick != "" ? currentLiClick + "(this,'" + String(item.Description) + "');" : "";
                    onClick = "IA_TabacooScreening.markStatusActive('ulMiscChildStatus', '" + item.Value + "');";

                    //Start 12/01/2016 Syed Zia,for child tab selection click
                    if (StatusType != null && StatusType.toLowerCase() == "occupation") {
                        onClick += "IA_TabacooScreening.enableDisableOccupationControls('" + item.Name + "','" + item.Value + "');";
                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "exercises") {
                        onClick += "IA_TabacooScreening.enableDisableExercisesControls('" + item.Name + "','" + item.Value + "');";
                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "caffeine intake") {

                        onClick += "IA_TabacooScreening.enableDisableCaffeineIntakeControls('" + item.Name + "','" + item.Value + "');";
                        // onClick += "IA_TabacooScreening.loadSocialHxComponent('miscellaneous_caffeineintake');";

                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "sleep") {
                        onClick += "IA_TabacooScreening.enableDisableSleepControls('" + item.Name + "','" + item.Value + "');";
                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "housing") {
                        onClick += "IA_TabacooScreening.enableDisableHousingControls('" + item.Name + "','" + item.Value + "');";
                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "travel") {
                        onClick += "IA_TabacooScreening.enableDisableTravelControls('" + item.Name + "','" + item.Value + "');";
                    }
                    //End 12/01/2016 Syed Zia,for child tab selection click

                    //item.Value = item.Value == "" ? 0 : item.Value;
                    l.append('<li id="' + item.Value + '" ' + currentLiClass + ' onclick="' + onClick + '" value=' + item.Value + ' refValue="' + item.Value + '"><a href="javascript:void(0)">' + item.Name + ' </a></li>');
                }

            });
        }
    },
    enableDisableTravelControls: function (currentStatus, currentStatusValue, editMode) {
        var def = $.Deferred();
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                    IA_TabacooScreening.resetControlValue(this);
                }
            });
            $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {
                if (!editMode)
                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfTravelDetailHxId").val(-1);

                if (currentStatus == "Does Not Travel") {
                    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxFromDate').val();
                    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxFromDate').attr('disabled', true);
                    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxToDate').val();
                    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxToDate').attr('disabled', true);
                    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #txtTravelHxLocation').val();
                    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #txtTravelHxLocation').attr('disabled', true);
                }
                else {
                    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxFromDate').removeAttr('disabled');
                    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxToDate').removeAttr('disabled');
                    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #txtTravelHxLocation').removeAttr('disabled');
                }
                $.each($('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dgvSocialHxMiscHxTravelHx tbody tr'), function (i, itm) {
                    if ($(itm).attr('statusid'))
                        if ($(itm).attr('statusid') == $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id") && $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").text().trim() == "Does Not Travel")
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").addClass('disableAll');
                        else
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").removeClass('disableAll');
                    else {
                        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").removeClass('disableAll');
                    }
                });
                def.resolve();
            });
        }
        else
            def.resolve();
        return def
    },
    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle enabling/disabling of Exercises controls on Miscellanous Tab

    //start 12-01-2016 Syed Zia, for Occupation controls enable/ disable
    enableDisableOccupationControls: function (currentStatus, currentStatusValue) {
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#OccupationDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                    // Faizan ameen
                    // Date: 21-Oct-2016
                    // uncoment the below line
                    // BUG QAC2-502
                    IA_TabacooScreening.resetControlValue(this);
                }
            });
            $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {
            });
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").prop('checked', true);
            IA_TabacooScreening.EnableDisableToDateOccupation();
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfOccupationHxId").val(-1);
        }
    },
    //end 12-01-2016 Syed Zia, for Occupation controls
    enableDisableExercisesControls: function (currentStatus, currentStatusValue) {
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#ExercisesDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (currentStatus.toLowerCase() == "does not exercise") {
                    if ($(this).attr("id") != "txtExercisesComments") {
                        $(this).attr("disabled", "disabled");
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            IA_TabacooScreening.resetControlValue(this);
                        }
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                }
                else {
                    $(this).removeAttr("disabled");
                }
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                    IA_TabacooScreening.resetControlValue(this);
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
            });
        }

        //loadSocialHxComponent
        $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {
            if (!isPopulated) {
                var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails");
                detailSection.resetAllControls(null);
            }

        })
    },

    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle enabling/disabling of Exercises controls on Miscellanous Tab
    enableDisableCaffeineIntakeControls: function (currentStatus, currentStatusValue) {
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#CaffeineIntakDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (currentStatus.toLowerCase() == "does not use") {
                    if ($(this).attr("id") != "txtCaffeineComments") {
                        $(this).attr("disabled", "disabled");
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            IA_TabacooScreening.resetControlValue(this);
                        }
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                }
                else {
                    $(this).removeAttr("disabled");
                }
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                    IA_TabacooScreening.resetControlValue(this);
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
            });

            $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails");
                    detailSection.resetAllControls(null);
                }

            });
        }
    },
    //Start 12/01/2016 Syed Zia,for sleep enable/ disable
    enableDisableSleepControls: function (currentStatus, currentStatusValue) {
        if (currentStatus != null && currentStatus != "") {
            IA_TabacooScreening.validateSleepHours($('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#SleepDetails #txtSleepHours'));
            var self = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#SleepDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                    //Start || 15 July, 2016 || ZeeshanAK || Fix for EMR-1610
                    //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #txtSleepHours').val('');
                    //End   || 15 July, 2016 || ZeeshanAK || Fix for EMR-1610
                    //  IA_TabacooScreening.resetControlValue(this);
                    IA_TabacooScreening.resetControlValue(this);
                }
            });

            $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {

            });
        }

        //if (currentStatus != null && currentStatus != "") {
        //    var self = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#OccupationDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
        //        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
        //            // Faizan ameen
        //            // Date: 21-Oct-2016
        //            // uncoment the below line
        //            // BUG QAC2-502
        //            IA_TabacooScreening.resetControlValue(this);
        //        }
        //    });


        //    $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {
        //    });
        //}
    },

    enableDisableHousingControls: function (currentStatus, currentStatusValue) {
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#HousingDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                    //  IA_TabacooScreening.resetControlValue(this);
                }
            });
            $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {
            });
        }
    },

    //End 12/01/2016 Syed Zia,for child tab selection click


    //Start//16/12/2015//Ahmad Raza//Method to compare form data with serialized data
    socialHistoryStateCheck: function (SocialHxType) {

        if (SocialHxType.toLowerCase() == "tobacco") {
            return EMRUtility.compareFormDataWithSerialized(IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #Tobacco');
        }
        else if (SocialHxType.toLowerCase() == "alcohol") {
            return EMRUtility.compareFormDataWithSerialized(IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #Alcohol');
        }
        else if (SocialHxType.toLowerCase() == "drug") {

            return EMRUtility.compareFormDataWithSerialized(IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #DrugAbuse');
            $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect({

            });
        }
        else if (SocialHxType.toLowerCase() == "sexual") {

            return EMRUtility.compareFormDataWithSerialized(IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #SexualHx');
            // $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect({

            // });
        }
        else if (SocialHxType.toLowerCase() == "miscellaneous") {
            return EMRUtility.compareFormDataWithSerialized(IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #Miscellaneous');

        } else {
            return EMRUtility.compareFormDataWithSerialized(IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx');

        }

    },
    //End//16/12/2015//Ahmad Raza//Method to compare form data with serialized data

    loadTabaccoTabnUnserializeForm: function () {
        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx div').removeClass('active');
        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ulSocialHxTabsItems>li').removeClass('active');

        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').data('serialize', null);
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Alcohol').data('serialize', null);
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #DrugAbuse').data('serialize', null);
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #SexualHx').data('serialize', null);
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous').data('serialize', null);

        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').addClass('active');
        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ulSocialHxTabsItems#listTobacco').addClass('active');
        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Tobacco').addClass('active');
        IA_TabacooScreening.loadSocialHx('tobacco', true);
        IA_TabacooScreening.selectTabColor('tobacco');
        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ulSocialHxTabsItems#listTobacco').addClass('active');
        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Tobacco').addClass('active');
        // $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco a').trigger('click');
    },

    triggerSocialHistoryTab: function () {

        try {
            if (LastSocialHx != null && LastSocialHx.PatientId == $('#PatientProfile #hfPatientId').val()) {
                $('#' + IA_TabacooScreening.params.PanelID + ' #' + LastSocialHx.SocialHxType + " a").trigger('click');

            }
            else {
                IA_TabacooScreening.bIsTriggerManually = true;
                if (IA_TabacooScreening.bTobaccoExist == true) {
                    $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco a').trigger('click');
                }
                else if (IA_TabacooScreening.bAlcoholExist == true) {
                    $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol a').trigger('click');
                }
                else if (IA_TabacooScreening.bDrugExist == true) {
                    $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse a').trigger('click');
                }
                else if (IA_TabacooScreening.bSexualExist == true) {
                    $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx a').trigger('click');
                }
                else if (IA_TabacooScreening.bMiscHxExist == true) {
                    $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx a').trigger('click');
                }
                else {
                    $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco a').trigger('click');
                }
            }
        } catch (ex) {
            console.log(ex);
        }
    },
    //Author: Muhammad Arshad
    //Date: 12-07-2015
    //This function will handle fill of SocialHx's childs Tabs as specified by TabType

    bindCurrentTabJSON: function (TabType, currentTabJSON, CurrentTabContainerId, ulTabStatusId) {
        var alcoholhx_detail = JSON.parse(currentTabJSON);
        self = $('#' + IA_TabacooScreening.params.PanelID + " " + CurrentTabContainerId);
        if (alcoholhx_detail.length > 0) {
            $.each(alcoholhx_detail, function (i, item) {

                //Start 21-12-2015 Muhammad Arshad handling status related controls enabling disabling on edit mode
                var statusId = "";
                var SocialHxType = '';
                var miscHxType = '';
                if (TabType != null && TabType.toLowerCase() == "tobacco") {
                    statusId = item.SmokingStatus;
                    IA_TabacooScreening.SeletedChildStatus = statusId;
                    SocialHxType = "Tobacco";
                }
                else if (TabType != null && TabType.toLowerCase() == "alcohol") {
                    statusId = item.AlcoholStatus;
                    IA_TabacooScreening.SeletedChildStatus = statusId;
                    SocialHxType = "Alcohol";
                }
                else if (TabType != null && TabType.toLowerCase() == "drug") {
                    statusId = item.DrugStatus;
                    IA_TabacooScreening.SeletedChildStatus = statusId;
                    SocialHxType = "DrugAbuse";
                }
                else if (TabType != null && TabType.toLowerCase() == "sexual") {
                    statusId = item.SexualStatus;
                    IA_TabacooScreening.SeletedChildStatus = statusId;
                    SocialHxType = "Sexual";
                }
                else if (TabType != null && TabType.toLowerCase().indexOf("miscellaneous") > -1) {
                    statusId = item.MiscChildStatus;
                    IA_TabacooScreening.SeletedChildStatus = statusId;
                    miscHxType = TabType;
                    SocialHxType = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                }

                //End 21-12-2015 Muhammad Arshad handling status related controls enabling disabling on edit mode
                var detailsJSON = '';
                var foundDataFromJson = false;
                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                    var cachedJSON = '';
                    if (miscHxType) {
                        cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(SocialHxType, statusId, miscHxType);
                    }
                    else {
                        cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(SocialHxType, statusId);
                    }

                    if (cachedJSON != '') {
                        detailsJSON = cachedJSON;
                        foundDataFromJson = true;
                    }
                    else {
                        detailsJSON = item;
                    }
                }
                else {
                    detailsJSON = item;
                }
                utility.bindMyJSONByName(true, detailsJSON, false, self).done(function () {

                    if (foundDataFromJson) {
                        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li").removeClass("active");
                        $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li#" + detailsJSON.StatusId).addClass("active");
                        if (TabType != null && TabType.toLowerCase() == "sexual") {
                            IA_TabacooScreening.setSexualControlls(detailsJSON.StatusId);
                        }
                    }
                    else {
                        if (statusId != "") {
                            //Start 28-10-2016 Modified By Humaira Yousaf to stop triggering of click event
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li").removeClass("active");
                            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li#" + statusId).addClass("active");
                            //End 28-10-2016 Modified By Humaira Yousaf to stop triggering of click event
                            if (TabType != null && TabType.toLowerCase() == "sexual") {
                                IA_TabacooScreening.setSexualControlls(statusId);
                            }
                        }
                    }
                    if (TabType != null && TabType.toLowerCase() == "drug") {
                        //$('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType option").removeAttr("selected");
                        //$('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("clearSelection");
                        //$('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("refresh");
                        $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").val(item.DrugType.split(','));
                        $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("refresh");
                        // Start 16-12-2015 Muhammad Arshad showing details in Drug multiselect tooltip
                        $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect({
                            //Start//Ahmad Raza// SelectAll and Search feature added in Type Multiselect
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            //End//Ahmad Raza// SelectAll and Search feature added in Type Multiselect
                            //buttonText: function (options, select) {
                            //    var numberOfOptions = $(this).children('option').length;
                            //},
                            buttonTitle: function (options, select) {
                                var buttonTitle = "";
                                $.each(options, function (i, item) {
                                    if (buttonTitle != "") {
                                        buttonTitle += "," + $(item).attr("refvalue");
                                    }
                                    else {
                                        buttonTitle += $(item).attr("refvalue");
                                    }

                                });
                                return buttonTitle;
                            }
                        });
                        // End 16-12-2015 Muhammad Arshad showing details in Drug multiselect tooltip
                    }
                    if (TabType != null && TabType.toLowerCase() == "tobacco") {
                        if (item.TobaccoRecentlyQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionTobacco #chkTobaccoRecentlyQuit").prop("checked", false);
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionTobacco #chkTobaccoRecentlyQuit").trigger("click");

                        }
                        if (item.TobaccoWouldQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionTobacco #chkTobaccoWouldQuit").prop("checked", false);
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionTobacco #chkTobaccoWouldQuit").trigger("click");

                        }
                        var activeStatus = $.trim($('#' + IA_TabacooScreening.params.PanelID + ' #ulSmokingStatus').find('li.active > a').text());
                        if (activeStatus == 'Never smoker' || activeStatus == 'Unknown if ever smoked' || activeStatus == 'Does not chew tobacco' || activeStatus == 'Non Smoker') {
                            var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                                if ($(this).attr("id") != "txtTobaccoComments") {
                                    $(this).attr("disabled", "disabled");
                                }
                                else {
                                    $(this).removeAttr("disabled");
                                }
                            });
                        }

                    }
                    if (TabType != null && TabType.toLowerCase() == "alcohol") {
                        if (item.AlcoholRecentlyQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionAlcohol #chkAlcoholRecentlyQuit").prop("checked", false);
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionAlcohol #chkAlcoholRecentlyQuit").trigger("click");

                        }
                        if (item.AlcoholNotReadyToQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionAlcohol #chkAlcoholNotReadyToQuit").prop("checked", false);
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionAlcohol #chkAlcoholNotReadyToQuit").trigger("click");

                        }
                        if (item.AlcoholWouldQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionAlcohol #chkAlcoholWouldQuit").prop("checked", false);
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionAlcohol #chkAlcoholWouldQuit").trigger("click");

                        }

                    }
                    if (TabType != null && TabType.toLowerCase() == "drug") {
                        if (item.DrugRecentlyQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #chkDrugRecentlyQuit").prop("checked", false);
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #chkDrugRecentlyQuit").trigger("click");

                        }
                        if (item.DrugWouldQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #chkDrugWouldQuit").prop("checked", false);
                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #chkDrugWouldQuit").trigger("click");

                        }
                    }
                    if (TabType != null && TabType.toLowerCase() == "sexual") {
                        //$('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD option").removeAttr("selected");
                        //$('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("clearSelection");
                        //$('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("refresh");
                        $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").val(item.SexualSTD.split(','));
                        $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("refresh");
                        //Start//17/12/2015//Ahmad Raza//Multiselect implimented in SocialHx's Sexual tab
                        $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            listTitle: function (options, select) {
                                var listTitle = "";
                                $.each(options, function (i, item) {
                                    if (listTitle != "") {
                                        listTitle += "," + $(item).attr("refvalue");
                                    }
                                    else {
                                        listTitle += $(item).attr("refvalue");
                                    }

                                });
                                return listTitle;
                            }
                        });
                        //End//17/12/2015//Ahmad Raza//Multiselect implimented in SocialHx's Sexual tab

                        //Start//14/01/2016//Ahmad Raza//Values not binding in Social hx's Sexual tab issue, fixed
                        if (item.RadSexualYesAbusedSexually == "No") {
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #RadSexualNoAbusedSexually").prop('checked', true);
                        }
                        else {
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #RadSexualYesAbusedSexually").prop('checked', true);
                        }

                        if (item.RadSexualYesPainWithIntercourse == "No") {
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #RadSexualNoPainWithIntercourse").prop('checked', true);
                        }
                        else {
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #RadSexualYesPainWithIntercourse").prop('checked', true);
                        }
                        if (item.RadSexualYesPregnant == "No") {
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #RadSexualNoPregnant").prop('checked', true);
                        }
                        else {
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #RadSexualYesPregnant").prop('checked', true);
                        }
                        if (item.SexualExposedToSTD.toLowerCase() == "yes") {
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("enable");

                        }
                        else {
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("disable");
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("clearSelection");
                            $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("refresh");
                        }
                        //End//14/01/2016//Ahmad Raza//Values not binding in Social hx's Sexual tab issue, fixed
                        //$('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect({

                        //});
                    }
                    if (TabType != null && TabType.toLowerCase().indexOf("miscellaneous") > -1) {
                        if (TabType.toLowerCase() == "miscellaneous_occupation") {

                            var myvar = "";
                        }

                        /* Start 11/01/2015 Syed Zia, for checkbox checked/unchecked */
                        if (TabType.toLowerCase() == "miscellaneous_caffeineintake") {
                            if (item.CaffeineHarmful.toLowerCase() == "true") {
                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", false);
                            }
                            else if (item.CaffeineHarmful.toLowerCase() == "false") {
                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", false);
                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                            }
                            else {
                                $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
                            }
                        }

                        /* End 11/01/2015 Syed Zia, for checkbox checked/unchecked */
                    }
                    var ActualStatusId = "";
                    if (foundDataFromJson) {
                        ActualStatusId = detailsJSON.StatusId;
                    }
                    else {
                        if (TabType != null && TabType.toLowerCase() == "tobacco") {
                            ActualStatusId = item.SmokingStatus;
                        }
                        else if (TabType != null && TabType.toLowerCase() == "alcohol") {
                            ActualStatusId = item.AlcoholStatus;
                        }
                        else if (TabType != null && TabType.toLowerCase() == "drug") {
                            ActualStatusId = item.DrugStatus;
                        }
                        else if (TabType != null && TabType.toLowerCase() == "sexual") {
                            ActualStatusId = item.SexualStatus;
                        }
                    }
                    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li").each(function () {
                        if (TabType != null && TabType.toLowerCase() == "tobacco") {
                            if ($(this).attr("id") == ActualStatusId) {
                                if ($(this).hasClass("active") == false) {
                                    $(this).addClass("active");
                                    /* Start 09/12/2015 Muhammad Irfan, to trigger the saved status on retrieving */
                                    //Start 28-10-2016 Modified By Humaira Yousaf to stop triggering of click event
                                    // $(this).trigger("click");
                                    //End 28-10-2016 Modified By Humaira Yousaf to stop triggering of click event
                                    /* End 09/12/2015 Muhammad Irfan, to trigger the saved status on retrieving */
                                }
                                //$(this).trigger("click");
                            }
                            else {
                                $(this).removeClass("active");
                            }
                        }
                        else if (TabType != null && TabType.toLowerCase() == "alcohol") {
                            if ($(this).attr("id") == ActualStatusId) {
                                if ($(this).hasClass("active") == false) {
                                    $(this).addClass("active");
                                    /* Start 09/12/2015 Muhammad Irfan, to trigger the saved status on retrieving */
                                    $(this).trigger("click");
                                    /* End 09/12/2015 Muhammad Irfan, to trigger the saved status on retrieving */
                                }
                                //$(this).trigger("click");
                            }
                            else {
                                $(this).removeClass("active");
                            }
                        }
                        else if (TabType != null && TabType.toLowerCase() == "drug") {
                            if ($(this).attr("id") == ActualStatusId) {
                                if ($(this).hasClass("active") == false) {
                                    $(this).addClass("active");
                                    /* Start 09/12/2015 Muhammad Irfan, to trigger the saved status on retrieving */
                                    $(this).trigger("click");
                                    /* End 09/12/2015 Muhammad Irfan, to trigger the saved status on retrieving */
                                }
                                //$(this).trigger("click");
                            }
                            else {
                                $(this).removeClass("active");
                            }
                        }
                        else if (TabType != null && TabType.toLowerCase() == "sexual") {
                            if ($(this).attr("id") == ActualStatusId) {
                                if ($(this).hasClass("active") == false) {
                                    $(this).addClass("active");
                                    /* Start 09/12/2015 Muhammad Irfan, to trigger the saved status on retrieving */
                                    $(this).trigger("click");
                                    /* End 09/12/2015 Muhammad Irfan, to trigger the saved status on retrieving */
                                }
                                //$(this).trigger("click");
                            }
                            else {
                                $(this).removeClass("active");
                            }
                        }

                    });
                    BackgroundLoaderShow(false);
                });
            });
        }
            //else if (alcoholhx_detail.length == null && TabType.toLowerCase().indexOf("miscellaneous") > -1) {
            //    utility.bindMyJSONByName(true, JSON.parse(currentTabJSON), false, self);
            //}
        else {

            if (TabType == 'alcohol') {
                $("#" + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Alcohol').resetAllControls(null);
            }
            else if (TabType == 'tobacco') {
                $("#" + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #Tobacco').resetAllControls(null);
            }

            else if (TabType == 'drug') {
                $("#" + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #DrugAbuse').resetAllControls(null);
            }
            else if (TabType == 'sexual') {
                $("#" + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #SexualHx').resetAllControls(null);
            }
            //* Start 01/14/2016 Abid Ali for bug
            // $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li").first().trigger("click");
            //* Start 01/14/2016 Abid Ali for bug
        }
    },
    setSexualControlls: function (statusId) {
        var currentStatus = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus li#" + statusId).text().trim();
        if (currentStatus != null && currentStatus != "") {
            if (currentStatus == "Practices safe sex") {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "ddlSexualExposedToSTD" || $(this).attr("id") == "ddlSexualSTD" || $(this).attr("id") == "RadSexualYesAbusedSexually" || $(this).attr("id") == "RadSexualNoAbusedSexually") {
                        $(this).attr("disabled", "disabled");
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }

                    if ($(this).attr("id") == "ddlSexualExposedToSTD") {
                        IA_TabacooScreening.enableDisableSTDControls(this);
                    }
                });
            }
            else if (currentStatus == 'Sexually Active' || currentStatus == 'Not Sexually Active') {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    $(this).removeAttr("disabled");
                    if ($(this).attr("id") == "ddlSexualExposedToSTD") {
                        IA_TabacooScreening.enableDisableSTDControls(this);
                    }
                });
            }
        }
    },
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle unremarkable feature for Social Hx
    unRemarkableSocialHx: function (obj, isFromLoad) {
        var isRemarkable = $(obj).prop("checked");

        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
            if (isRemarkable == true) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel = [];
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel = [];
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel = [];
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel = [];
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel = [];
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel = [];
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel = [];
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel = [];
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel = [];
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel = [];
                }
                else {
                    var patientId;
                    if (IA_TabacooScreening.params.patientID == null) {
                        patientId = $('#PatientProfile #hfPatientId').val();
                    } else {
                        patientId = IA_TabacooScreening.params.patientID;
                    }
                    var SocialHxData = {
                        SocialHxId: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(),
                        PatientId: patientId,
                        SocialHxDate: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val(),
                        SocialHxUnremarkable: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked"),
                        SocialComments: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val(),
                        NotesId: Clinical_ProgressNote.params.NotesId,
                        lstTobaccoModel: [],
                        lstAlcoholModel: [],
                        lstDrugAbuseModel: [],
                        lstSexualHxModel: [],
                        lstOccupationHxModel: [],
                        lstTravelHxModel: [],
                        lstSleepHxModel: [],
                        lstExercisesHxModel: [],
                        lstHousingHxModel: [],
                        lstCaffeineIntakHxModel: [],
                    }
                    Clinical_HistorySummary.HistoryCacheList.SocialHx = SocialHxData;
                }
            }
            else {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
                }
                else {
                    var patientId;
                    if (IA_TabacooScreening.params.patientID == null) {
                        patientId = $('#PatientProfile #hfPatientId').val();
                    } else {
                        patientId = IA_TabacooScreening.params.patientID;
                    }
                    var SocialHxData = {
                        SocialHxId: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(),
                        PatientId: patientId,
                        SocialHxDate: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val(),
                        SocialHxUnremarkable: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked"),
                        SocialComments: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val(),
                        NotesId: Clinical_ProgressNote.params.NotesId,
                        lstTobaccoModel: [],
                        lstAlcoholModel: [],
                        lstDrugAbuseModel: [],
                        lstSexualHxModel: [],
                        lstOccupationHxModel: [],
                        lstSleepHxModel: [],
                        lstTravelHxModel: [],
                        lstExercisesHxModel: [],
                        lstHousingHxModel: [],
                        lstCaffeineIntakHxModel: [],
                    }
                    Clinical_HistorySummary.HistoryCacheList.SocialHx = SocialHxData;
                }
            }
        }
        if (isRemarkable == true) {
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #btnSocialHxSave').hide();
            }
            else {
                $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #btnSocialHxSave').show();
            }

            /* Start 23/12/2015 Muhammad Irfan for bug # EMR-153 */
            $('#' + IA_TabacooScreening.params.PanelID + ' #ulSocialHxTabsItems').addClass('disableAll');
            /* End 23/12/2015 Muhammad Irfan for bug # EMR-153 */

            /* Start 07/01/2016 Muhammad Irfan for bug # EMR-153 */
            $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco,#listAlcohol,#listDrugAbuse,#listSexualHx').removeClass('successLight');
            /* End 07/01/2016 Muhammad Irfan for bug # EMR-153 */

        }
        else {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #btnSocialHxSave').hide();
        }
        var self = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').find('div#Tobacco,div#Alcohol,div#DrugAbuse,div#SexualHx,div#Miscellaneous').each(function () {
            if (isRemarkable == true) {
                if ($(this).hasClass("disableAll") == false) {
                    $(this).addClass("disableAll");

                }
                $(this).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
                    $(this).attr("disabled", "disabled");
                    IA_TabacooScreening.resetControlValue(this);
                });
                //IA_TabacooScreening.resetControlValue(this);
            }
            else {
                $(this).removeClass("disableAll");
                /* Start 23/12/2015 Muhammad Irfan for bug # EMR-153 */
                $('#' + IA_TabacooScreening.params.PanelID + ' #ulSocialHxTabsItems').removeClass('disableAll');
                /* End 23/12/2015 Muhammad Irfan for bug # EMR-153 */

                $(this).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
                    $(this).removeAttr("disabled");
                    IA_TabacooScreening.resetControlValue(this);
                });
                /* Start 07/01/2016 Muhammad Irfan for bug # EMR-153 */
                //if (IA_TabacooScreening.bAlcoholExist == true)
                //    $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').addClass('successLight');
                //if (IA_TabacooScreening.bTobaccoExist == true)
                //    $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').addClass('successLight');
                //if (IA_TabacooScreening.bDrugExist == true)
                //    $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                //if (IA_TabacooScreening.bSexualExist == true)
                //    $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').addClass('successLight');



                //$('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco,#listAlcohol,#listDrugAbuse,#listSexualHx').removeClass('active');
                /* End 07/01/2016 Muhammad Irfan for bug # EMR-153 */
            }
        });
        if (!isRemarkable) {
            var activeTabId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx ul#ulSocialHxTabsItems li.active").attr("id")
            IA_TabacooScreening.isRemarkableFormload = false;
            $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #" + activeTabId + " a").trigger("click");
        }
        else {
            IA_TabacooScreening.isRemarkableFormload = true;
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
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                var groupRadBtn = $("input[name='" + $(obj).attr('name') + "']");
                if (groupRadBtn.length > 1) {
                    $.each(groupRadBtn, function (i, item) {
                        if ($(item).attr("id").toLowerCase().indexOf("no") > -1) {
                            $(item).trigger("click");
                        }
                    });
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
            }
            else {
                obj.checked = false;
            }
        }

        if (currentElementTagName.toLowerCase() == 'select') {
            $(obj).find('option:selected').removeAttr('selected');
            //$(this).attr('selectedIndex', '-1')
            if ($(obj).attr("ddlmultilist")) {
            }
            else {
                $(obj).find('option:eq(0)').attr('selected', 'selected');
            }

        }
        if (currentElementTagName.toLowerCase() == 'ul') {
            $(obj).find('li.active').removeClass('active');
        }

        //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());

    },

    //Author: Muhammad Arshad
    //Date: 12-28-2015
    //This function will check if details has any value for selected status
    //Begin 12-28-2015 Muhammad Arshad Bug# EMR-159 Social History Clinical Module -> Save
    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";
        if (TabType != null && TabType != "") {
            if (TabType.toLowerCase() == "tobacco") {
                sectionDetails = "section#sectionTobacco";
            }
            else if (TabType.toLowerCase() == "alcohol") {
                sectionDetails = "section#sectionAlcohol";
            }
            else if (TabType.toLowerCase() == "drug") {
                sectionDetails = "section#sectionDrugAbuse";
            }
            else if (TabType.toLowerCase() == "sexual") {
                sectionDetails = "section#sectionSexualHx";
            }
            else if (TabType.toLowerCase() == "miscellaneous_occupation") {
                sectionDetails = "div#OccupationDetails";
            }
            else if (TabType.toLowerCase() == "miscellaneous_sleep") {
                sectionDetails = "div#SleepDetails";
            }
            else if (TabType.toLowerCase() == "miscellaneous_exercises") {
                sectionDetails = "div#ExercisesDetails";
            }
            else if (TabType.toLowerCase() == "miscellaneous_housing") {
                sectionDetails = "div#HousingDetails";
            }
            else if (TabType.toLowerCase() == "miscellaneous_caffeineintake") {
                sectionDetails = "div#CaffeineIntakDetails";
            }
            else if (TabType.toLowerCase() == "miscellaneous_travel") {
                sectionDetails = "div#divTravelDetailsHx";
            }
        }
        if (sectionDetails != "") {
            var self = $('#' + IA_TabacooScreening.params.PanelID + ' ' + sectionDetails).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                //Start 10-05-2016 Edit by Humaira Yousaf Bug# EMR-987
                var activeStatus = $.trim($('#' + IA_TabacooScreening.params.PanelID + ' #ulSmokingStatus').find('li.active > a').text());
                if ($(this).prop("disabled") != true && DetailExists == false) {
                    var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                    if ((activeStatus == "Never smoker" || activeStatus == "Unknown if ever smoked" || activeStatus == "Non Smoker") && this.id == "txtTobaccoComments") {
                        DetailExists = true;
                    }
                    //End 10-05-2016 Edit by Humaira Yousaf Bug# EMR-987

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
                    //if (currentElementTagName.toLowerCase() == 'ul') {
                    //    $(this).find('li.active').removeClass('active');
                    //}
                }
            });
        }

        return DetailExists;

    },

    //End 12-28-2015 Muhammad Arshad Bug# EMR-159 Social History Clinical Module -> Save

    //Author: Muhammad Arshad
    //Date: 12-09-2015
    //This function will Enable Disable controls for Exposed to STD, if it is Yes then next control will be enabled otherwise disabled

    enableDisableSTDControls: function (obj) {
        var objSTD = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD');
        if (obj != null) {

            if ($(obj).find("option:selected").val().toLowerCase() == "yes") {
                objSTD.removeAttr("disabled");
                objSTD.find("option:selected").removeAttr("selected");
                objSTD.multiselect("enable");
            }
            else if ($(obj).find("option:selected").val().toLowerCase() == "no") {
                objSTD.attr("disabled", "disabled");
                objSTD.find("option:selected").removeAttr("selected");
                objSTD.multiselect("disable");
                objSTD.multiselect("clearSelection");
                IA_TabacooScreening.resetControlValue(objSTD);
            }
            else {
                objSTD.attr("disabled", "disabled");
                objSTD.find("option:selected").removeAttr("selected");
                objSTD.multiselect("disable");
                objSTD.multiselect("clearSelection");
                IA_TabacooScreening.resetControlValue(objSTD);
                $(obj).val("");

                $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').val("");
                $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').find("option:selected").removeAttr("selected");
                $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').multiselect("disable");
                $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').multiselect("clearSelection");
                $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').multiselect("refresh");
            }
        }
        else {
            objSTD.removeAttr("disabled");
        }
    },

    //Author: Muhammad Arshad
    //Date: 12-09-2015
    //This function will Enable Disable controls for Recently Quit, if it is checked then Lenght of Cessation control will be disabled otherwise enabled

    enableDisableCessationControls: function (obj, TabType) {
        var CessationControls = "";
        if (TabType != null && TabType.toLowerCase() == "tobacco") {
            CessationControls = "section#sectionTobacco #txtTobaccoCessationLength,#ddlTobaccoCessationPeriod";
        }
        else if (TabType != null && TabType.toLowerCase() == "alcohol") {
            CessationControls = "section#sectionAlcohol #txtAlcoholCessationLength,#ddlAlcoholCessationPeriod";
        }
        else if (TabType != null && TabType.toLowerCase() == "drug") {
            CessationControls = "section#sectionDrugAbuse #txtDrugCessationLength,#ddlDrugCessationPeriod";
        }
        var objCessation = $('#' + IA_TabacooScreening.params.PanelID).find(CessationControls).each(function () {
            if (obj != null) {

                if ($(obj).prop("checked") == true) {
                    $(this).attr("disabled", "disabled");
                }
                else {
                    $(this).removeAttr("disabled");
                }
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                IA_TabacooScreening.resetControlValue(this);
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
            }
            else {
                $(this).removeAttr("disabled");
            }
        });
    },

    enableCessationValidation: function () {



        // var stayLength = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #txtDrugCessationLength').val();
        var ddlVal = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ddlDrugCessationPeriod').val();
        //if (stayLength != null && stayLength != '') {
        //    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('DrugCessationPeriod', true);
        //    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation<span class="required">*</span>');
        //}
        //else {
        //    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('DrugCessationPeriod', false);
        //    $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation');
        //}
        if (ddlVal != null && ddlVal != '') {
            // $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('DrugCessationLength', true);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation<span class="required">*</span>');

        } else {
            // $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('DrugCessationLength', false);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation');
        }

    },
    //Author: Abid Ali
    //Date: 02-02-2016
    //This function will uncheck all the checkboxes (checkBoxToBeUncheck) passed to the function
    unCheck: function (obj, checkBoxToBeUncheck, TabType) {

        //Please don't remove these comments as it may be use
        // for future use

        var checkboxArray = checkBoxToBeUncheck.split(',');
        var recentlydQuit = "recentlyquit";
        /*
        //Start  01-03-2016 Farooq Ahmad this vaiable will store the selected smoking status
        var smokingStatusText = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus li.active").text().toLowerCase().trim();
        //End  01-03-2016 Farooq Ahmad  this vaiable will store the selected smoking status
        */

        /* if (smokingStatusText == 'current every day smoker' ||smokingStatusText == 'light tobacco smoker') {
             return false;
         }*/

        if ($(obj).attr('name') == "TobaccoRecentlyQuit") {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationPeriod', false);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration');

        }

        if ($(obj).attr('name') == "DrugRecentlyQuit") {
            if ($(obj).is(":checked")) {
                $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation');
            }


        }
        $.each(checkboxArray, function (idex, item) {

            if ($(obj).is(":checked")) {
                if (item.toLowerCase().indexOf(recentlydQuit) != -1) {
                    if ($('#' + item).is(":checked")) {
                        $('#' + item).trigger('click');
                    }
                }
                else {
                    $('#' + item).prop("checked", false);
                }
            }
        });
        //If recently quit checkbox is checked/uncheked then enableDisable cessationControls
        if (!($(obj).attr('id').indexOf('WouldQuit') > -1) && !($(obj).attr('id').indexOf('NotReadyToQuit') > -1)) {
            IA_TabacooScreening.enableDisableCessationControls(obj, TabType);
        }

    },
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will Enable Disable controls for Tobacco on basis of selected Status as specified by currentStatus

    enableDisableTobaccoControls: function (obj, currentStatus, currentStatusValue) {

        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && IA_TabacooScreening.socialHxJSON != '') {
            IA_TabacooScreening.cacheTobaccoTabData();
        }

        if (obj != null) {
            $($(obj).parent()).find("li").each(function () {
                $(this).removeClass("active");
            });
            if ($(obj).hasClass("active") == false) {
                $(obj).addClass("active");
            }
        }
        // added by faizan ameen bug no : QAC2-348
        // remove mandatory check from duration field on menu changed.
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationPeriod', false);
        $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration');

        if (currentStatus != null && currentStatus != "") {

            var statusId = $(obj).attr('id');
            //var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Tobacco', statusId);

            //if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && cachedJSON != '') {
            //    detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
            //    utility.bindMyJSONByName(true, cachedJSON, false, detailSection);
            //}
            //else
            {
                if (currentStatus == "Current every day smoker" || currentStatus == 'Current some day smoker' || currentStatus == 'Heavy tobacco smoker' || currentStatus == 'Light tobacco smoker') {
                    var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {

                        if ($(this).attr("id") == "txtTobaccoCessationLength" || $(this).attr("id") == "ddlTobaccoCessationPeriod" || $(this).attr("id") == "chkTobaccoRecentlyQuit") {
                            $(this).attr("disabled", "disabled");
                            if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                                IA_TabacooScreening.resetControlValue(this);
                            }
                        }
                        else {
                            $(this).removeAttr("disabled");
                        }

                        if (currentStatus == "Current every day smoker") {
                            IA_TabacooScreening.resetControlValue(this);
                        }
                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                            IA_TabacooScreening.resetControlValue(this);
                            //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                        }

                        //End 14-01-2016 syed zia ,for same status click refresh problem
                    });
                }
                else if (currentStatus == 'Former smoker') {
                    var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                        if ($(this).attr("id") == "ddlTobaccoCounsellingPeriod" || $(this).attr("id") == "ddlTobaccoCounsellingTopic" || $(this).attr("id") == "chkTobaccoWouldQuit") {
                            $(this).attr("disabled", "disabled");
                            if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                                IA_TabacooScreening.resetControlValue(this);
                            }
                        }
                        else {
                            $(this).removeAttr("disabled");
                        }

                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            // Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                            IA_TabacooScreening.resetControlValue(this);
                            //  End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                        }
                        //End 14-01-2016 syed zia ,for same status click refresh problem

                    });
                }
                else if (currentStatus == 'Never smoker' || currentStatus == 'Unknown if ever smoked' || currentStatus == 'Does not chew tobacco' || currentStatus == 'Non Smoker') {
                    var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                        if ($(this).attr("id") != "txtTobaccoComments") {
                            $(this).attr("disabled", "disabled");
                            if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                                IA_TabacooScreening.resetControlValue(this);
                            }
                        }
                        else {
                            $(this).removeAttr("disabled");
                        }

                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                            IA_TabacooScreening.resetControlValue(this);
                            //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                        }
                        //End 14-01-2016 syed zia ,for same status click refresh problem
                    });
                }
                else if (currentStatus == 'Smoker, current status unknown') {
                    var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                        $(this).removeAttr("disabled");
                        //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                        IA_TabacooScreening.resetControlValue(this);
                        //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            // Start 16-12-2015 Muhammad Arshad we dont need to reset values while this status is selected
                            IA_TabacooScreening.resetControlValue(this);
                            // End 16-12-2015 Muhammad Arshad we dont need to reset values while this status is selected
                        }
                        //End 14-01-2016 syed zia ,for same status click refresh problem
                    });
                }
                else if (currentStatus == 'Chews tobacco') {
                    var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                        if ($(this).attr("id") == "txtTobaccoCessationLength" || $(this).attr("id") == "ddlTobaccoType" || $(this).attr("id") == "ddlTobaccoCessationPeriod" || $(this).attr("id") == 'chkTobaccoRecentlyQuit') {
                            $(this).attr("disabled", "disabled");
                            if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                                IA_TabacooScreening.resetControlValue(this);
                            }
                        }
                        else {
                            $(this).removeAttr("disabled");
                        }

                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            //    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                            IA_TabacooScreening.resetControlValue(this);
                            //    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                        }
                        //End 14-01-2016 syed zia ,for same status click refresh problem

                    });
                }
            }
            $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionTobacco #hfTobaccoDataChangeBit').val("1");
            $.when(IA_TabacooScreening.fillDetails()).then(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
                    detailSection.resetAllControls(null);
                }

                //if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && IA_TabacooScreening.socialHxJSON != '') {
                //    var updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                //    if (IA_TabacooScreening.socialHxJSON != updatedJSON) {
                //        IA_TabacooScreening.cacheSocialHxJSON('Tobacco', statusIdToCache, updatedJSON);
                //    }
                //}


                //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());
            });

        }

        IA_TabacooScreening.enableDisableCounsellingTopic('tobacco');
    },

    cacheTobaccoTabData: function (socialType) {
        var dfd = $.Deferred();
        var updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
        var statusId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulSmokingStatus > li.active").attr('id');

        if (IA_TabacooScreening.socialHxJSON != updatedJSON) {
            if (statusId != null) {
                $.when(IA_TabacooScreening.cacheSocialHxJSON('Tobacco', statusId, updatedJSON)).then(function () {
                    dfd.resolve();
                });
                IA_TabacooScreening.SetIsLast('Tobacco', statusId);
            }
            else {
                dfd.resolve();
            }
        }
        else {
            if (socialType == 'tobacco') {
                var json = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionTobacco").getMyJSONByName();
                var detailsSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionTobacco").clone();
                $(detailsSection).resetAllControls(null);
                var emptyJson = $(detailsSection).getMyJSONByName();

                if (statusId != null && statusId > 0 && json == emptyJson) {
                    $.when(IA_TabacooScreening.cacheSocialHxJSON('Tobacco', statusId, updatedJSON)).then(function () {
                        dfd.resolve();
                    });
                    IA_TabacooScreening.SetIsLast('Tobacco', statusId);
                }
                else {

                    var date = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
                    var unremarkable = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
                    var comments = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();

                    var dataChanged = IA_TabacooScreening.date != date || IA_TabacooScreening.unremarkable != unremarkable || IA_TabacooScreening.overallComments != comments;
                    if (dataChanged) {
                        $.when(IA_TabacooScreening.cacheSocialHxJSON('', statusId, updatedJSON, null, true)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        dfd.resolve();
                    }
                }
            }
            else {
                dfd.resolve();
            }
        }

        return dfd;
    },
    //Abid Ali
    fillDetails: function () {
        var dfd = new $.Deferred();
        var SocialHxType = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
        var isPopulated = false;
        IA_TabacooScreening.fillSocialHx(SocialHxType).then(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    ////Begin 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                    //if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                    //    /* Start 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                    //    $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").addClass("disableAll");
                    //    /* End 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                    //}
                    //End 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                        var socialhx_detail = JSON.parse(response.SocialHxFill_JSON);
                        // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                        if (IA_TabacooScreening.isRemarkableFormload == false) {
                            socialhx_detail.SocialHxUnremarkable = "false";
                        }
                        // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                        var self = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx");
                        utility.bindMyJSONByName(true, socialhx_detail, false, self).done(function () {

                            IA_TabacooScreening.params.mode = "Edit";
                        });
                    }
                    if (SocialHxType == 'tobacco') {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
                        var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus li.active");
                        var statusId = $activeLi.attr('id');
                        if (typeof response.TobaccoHxFill_JSON != "undefined") {
                            var tobaccoDetails = JSON.parse(response.TobaccoHxFill_JSON)[0];
                            if (tobaccoDetails != null && statusId == tobaccoDetails.SmokingStatus) {
                                isPopulated = true;
                                var detailsJSON = '';
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Tobacco', statusId);
                                    if (cachedJSON != '') {
                                        detailsJSON = cachedJSON;
                                    }
                                    else {
                                        detailsJSON = tobaccoDetails;
                                    }
                                }
                                else {
                                    detailsJSON = tobaccoDetails;
                                }

                                utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {
                                    return dfd.resolve(isPopulated);
                                });
                            }
                            else {
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Tobacco', statusId);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            return dfd.resolve(isPopulated);
                                        });
                                    }
                                }
                            }
                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();

                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Tobacco', statusId);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {

                                    });
                                }
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'alcohol') {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol");
                        var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Alcohol #ulAlcoholStatus li.active");
                        var statusId = $activeLi.attr('id');
                        if (typeof response.AlcoholHxFill_JSON != "undefined") {
                            var alcoholDetails = JSON.parse(response.AlcoholHxFill_JSON)[0];

                            if (alcoholDetails != null && statusId == alcoholDetails.AlcoholStatus) {

                                var detailsJSON = '';
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Alcohol', statusId);
                                    if (cachedJSON != '') {
                                        detailsJSON = cachedJSON;
                                    }
                                    else {
                                        detailsJSON = alcoholDetails;
                                    }
                                }
                                else {
                                    detailsJSON = alcoholDetails;
                                }

                                utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {


                                });
                            }
                            else {
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Alcohol', statusId);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Alcohol', statusId);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }

                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                            }
                        }
                    }

                    else if (SocialHxType == 'drug') {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse");
                        var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#DrugAbuse #ulDrugStatus li.active");
                        var statusId = $activeLi.attr('id');
                        if (typeof response.DrugAbuseFill_JSON != "undefined") {
                            var drugDetails = JSON.parse(response.DrugAbuseFill_JSON)[0];

                            if (drugDetails != null && statusId == drugDetails.DrugStatus) {
                                var detailsJSON = '';
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('DrugAbuse', statusId);
                                    if (cachedJSON != '') {
                                        detailsJSON = cachedJSON;
                                    }
                                    else {
                                        detailsJSON = drugDetails;
                                    }
                                }
                                else {
                                    detailsJSON = drugDetails;
                                }

                                utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {
                                    $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val(drugDetails.DrugType.split(','));
                                    $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                    $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect({
                                        enableFiltering: true,
                                        enableCaseInsensitiveFiltering: true,
                                    });
                                });
                            }
                            else {
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('DrugAbuse', statusId);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            if (drugDetails != null) {
                                                $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val(drugDetails.DrugType.split(','));
                                            }
                                            $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                            $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect({
                                                enableFiltering: true,
                                                enableCaseInsensitiveFiltering: true,
                                            });
                                        });
                                    }
                                }
                            }
                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('DrugAbuse', statusId);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {

                                        if (cachedJSON != '') {
                                            $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val(cachedJSON.DrugType.split(','));
                                        }
                                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect({
                                            enableFiltering: true,
                                            enableCaseInsensitiveFiltering: true,
                                        });

                                    });
                                }
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'sexual') {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx");
                        var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus li.active");
                        var statusId = $activeLi.attr('id');
                        if (typeof response.SexualHxFill_JSON != "undefined") {
                            var sexualDetails = JSON.parse(response.SexualHxFill_JSON)[0];

                            if (sexualDetails != null && statusId == sexualDetails.SexualStatus) {

                                var detailsJSON = '';
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Sexual', statusId);
                                    if (cachedJSON != '') {
                                        detailsJSON = cachedJSON;
                                    }
                                    else {
                                        detailsJSON = sexualDetails;
                                    }
                                }
                                else {
                                    detailsJSON = sexualDetails;
                                }

                                utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {

                                    if (sexualDetails.RadSexualYesAbusedSexually.toLowerCase() == "yes") {
                                        detailSection.find("#RadSexualYesAbusedSexually").prop("checked", true);
                                    }
                                    else {
                                        detailSection.find("#RadSexualNoAbusedSexually").prop("checked", true);
                                    }
                                    if (sexualDetails.RadSexualYesPainWithIntercourse.toLowerCase() == "yes") {

                                        detailSection.find("#RadSexualYesPainWithIntercourse").prop("checked", true);
                                    }
                                    else {
                                        detailSection.find("#RadSexualNoPainWithIntercourse").prop("checked", true);
                                    }
                                    if (sexualDetails.RadSexualYesPregnant.toLowerCase() == "yes") {

                                        detailSection.find("#RadSexualYesPregnant").prop("checked", true);
                                    }
                                    else {
                                        detailSection.find("#RadSexualNoPregnant").prop("checked", true);
                                    }

                                });
                            }
                            else {
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Sexual', statusId);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {

                                            if (sexualDetails.RadSexualYesAbusedSexually.toLowerCase() == "yes") {
                                                detailSection.find("#RadSexualYesAbusedSexually").prop("checked", true);
                                            }
                                            else {
                                                detailSection.find("#RadSexualNoAbusedSexually").prop("checked", true);
                                            }
                                            if (sexualDetails.RadSexualYesPainWithIntercourse.toLowerCase() == "yes") {

                                                detailSection.find("#RadSexualYesPainWithIntercourse").prop("checked", true);
                                            }
                                            else {
                                                detailSection.find("#RadSexualNoPainWithIntercourse").prop("checked", true);
                                            }
                                            if (sexualDetails.RadSexualYesPregnant.toLowerCase() == "yes") {

                                                detailSection.find("#RadSexualYesPregnant").prop("checked", true);
                                            }
                                            else {
                                                detailSection.find("#RadSexualNoPregnant").prop("checked", true);
                                            }

                                        });
                                    }
                                }
                            }
                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON('Sexual', statusId);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        if (cachedJSON.RadSexualAbusedSexually == true) {
                                            detailSection.find("#RadSexualYesAbusedSexually").prop("checked", true);
                                        }
                                        else {
                                            if (detailSection.find("#RadSexualNoAbusedSexually").attr("disabled") != "disabled") {
                                                detailSection.find("#RadSexualNoAbusedSexually").prop("checked", true);
                                            }

                                        }
                                        if (cachedJSON.RadSexualPainWithIntercourse == true) {

                                            detailSection.find("#RadSexualYesPainWithIntercourse").prop("checked", true);
                                        }
                                        else {
                                            if (detailSection.find("#RadSexualNoPainWithIntercourse").attr("disabled") != "disabled") {
                                                detailSection.find("#RadSexualNoPainWithIntercourse").prop("checked", true);
                                            }
                                        }
                                    });
                                }

                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_occupation') {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails");
                        var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var statusId = $activeLi.attr('id');
                        var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                        if (typeof response.OccupationHxFill_JSON != "undefined") {
                            var occupationDetails = JSON.parse(response.OccupationHxFill_JSON)[0];
                            if (occupationDetails != null && statusId == occupationDetails.MiscChildStatus) {

                                var detailsJSON = '';
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        detailsJSON = cachedJSON;
                                    }
                                    else {
                                        detailsJSON = occupationDetails;
                                    }
                                }
                                else {
                                    detailsJSON = occupationDetails;
                                }

                                utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {
                                });
                            }
                            else {
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails").getMyJSONByName();
                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails").getMyJSONByName();

                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_sleep') {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails");
                        var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var statusId = $activeLi.attr('id');
                        var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');

                        if (typeof response.SleepHxFill_JSON != "undefined") {
                            var sleepDetails = JSON.parse(response.SleepHxFill_JSON)[0];

                            if (sleepDetails != null && statusId == sleepDetails.MiscChildStatus) {

                                var detailsJSON = '';
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        detailsJSON = cachedJSON;
                                    }
                                    else {
                                        detailsJSON = occupationDetails;
                                    }
                                }
                                else {
                                    detailsJSON = occupationDetails;
                                }

                                utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {
                                });
                            }
                            else {
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails").getMyJSONByName();
                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_exercises') {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails");
                        var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var statusId = $activeLi.attr('id');
                        var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');

                        if (typeof response.ExercisesHxFill_JSON != "undefined") {
                            var exerciseDetails = JSON.parse(response.ExercisesHxFill_JSON)[0];
                            if (exerciseDetails != null && statusId == exerciseDetails.MiscChildStatus) {
                                var detailsJSON = '';
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        detailsJSON = cachedJSON;
                                    }
                                    else {
                                        detailsJSON = exerciseDetails;
                                    }
                                }
                                else {
                                    detailsJSON = exerciseDetails;
                                }
                                utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {

                                });
                            }
                            else {
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails").getMyJSONByName();
                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails").getMyJSONByName();

                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_housing') {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails");
                        var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var statusId = $activeLi.attr('id');
                        var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');

                        if (typeof response.HousingHxFill_JSON != "undefined") {
                            var housingDetails = JSON.parse(response.HousingHxFill_JSON)[0];
                            if (housingDetails != null && statusId == housingDetails.MiscChildStatus) {
                                var detailsJSON = '';
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        detailsJSON = cachedJSON;
                                    }
                                    else {
                                        detailsJSON = exerciseDetails;
                                    }
                                }
                                else {
                                    detailsJSON = exerciseDetails;
                                }
                                utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {

                                });
                            }
                            else {
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails").getMyJSONByName();
                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_caffeineintake') {
                        var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails");
                        var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var socialTypeMiscId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                        var statusId = $activeLi.attr('id');

                        if (typeof response.CaffeineIntakeHxFill_JSON != "undefined") {
                            var caffeineDetails = JSON.parse(response.CaffeineIntakeHxFill_JSON)[0];

                            if (caffeineDetails != null && statusId == caffeineDetails.MiscChildStatus) {

                                var detailsJSON = '';
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        detailsJSON = cachedJSON;
                                    }
                                    else {
                                        detailsJSON = caffeineDetails;
                                    }
                                }
                                else {
                                    detailsJSON = caffeineDetails;
                                }

                                utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {
                                });

                                if (caffeineDetails.CaffeineHarmful.toLowerCase() == "true") {
                                    $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                }
                                else if (caffeineDetails.CaffeineHarmful.toLowerCase() == "false") {
                                    $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                                }
                                else {
                                    $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
                                }
                            }
                            else {
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != []) {
                                        utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {
                                        });

                                        if (caffeineDetails.CaffeineHarmful.toLowerCase() == "true") {
                                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                        }
                                        else if (caffeineDetails.CaffeineHarmful.toLowerCase() == "false") {
                                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                                        }
                                        else {
                                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
                                        }
                                    }
                                }
                            }
                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails").getMyJSONByName();
                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = IA_TabacooScreening.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        if (cachedJSON.RadCaffieneharmful == true) {
                                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", false);
                                        }
                                        else if (cachedJSON.RadCaffieneharmful == false) {
                                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", false);
                                        }
                                        else {
                                            $('#' + IA_TabacooScreening.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
                                        }
                                    });
                                }
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_travel') {
                        if (typeof response.TravelHxFill_JSON != "undefined" && response.TravelHxFill_JSON != "[]") {
                            var TravelDetails = JSON.parse(response.TravelHxFill_JSON)[0];

                            IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx").getMyJSONByName();
                        }
                        else {
                            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote")
                                IA_TabacooScreening.socialHxJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx").getMyJSONByName();
                        }
                    }

                    /* else if (SocialHxType == 'miscellaneous_occupation') {

                         if (typeof response.SexualHxFill_JSON != "undefined") {
                             var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails");
                             var sexualDetails = JSON.parse(response.SexualHxFill_JSON)[0];
                             var $activeLi = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSexualStatus li.active");
                             var statusId = $activeLi.attr('id');
                             if (statusId == sexualDetails.DrugStatus) {

                                 utility.bindMyJSONByName(true, sexualDetails, false, detailSection).done(function () {

                                 });
                             }
                         }
                     }*/
                }
            }

            //$('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());


            return dfd.resolve(isPopulated);
        });

        //return dfd.resolve(isPopulated);
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will Enable Disable controls for Alcohol on basis of selected Status as specified by currentStatus

    enableDisableAlcoholControls: function (obj, currentStatus, currentStatusValue) {

        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && IA_TabacooScreening.socialHxJSON != '') {
            IA_TabacooScreening.cacheAlcoholTabData();
        }
        if (obj != null) {
            $($(obj).parent()).find("li").each(function () {
                $(this).removeClass("active");
            });
            if ($(obj).hasClass("active") == false) {
                $(obj).addClass("active");
            }
        }
        if (currentStatus != null && currentStatus != "") {
            if (currentStatus == "Drinks daily" || currentStatus == 'Frequently drinks') {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "txtAlcoholCessationLength" || $(this).attr("id") == "ddlAlcoholCessationPeriod" || $(this).attr("id") == "chkAlcoholRecentlyQuit") {
                        $(this).attr("disabled", "disabled");
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            IA_TabacooScreening.resetControlValue(this);
                        }
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                        IA_TabacooScreening.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            else if (currentStatus == 'Has history of Alcoholism') {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "ddlAlcoholCounsellingPeriod" || $(this).attr("id") == "ddlAlcoholCounsellingTopic" || $(this).attr("id") == "chkAlcoholWouldQuit") {
                        $(this).attr("disabled", "disabled");
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            IA_TabacooScreening.resetControlValue(this);
                        }
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                        IA_TabacooScreening.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            else if (currentStatus == 'Does not drink' || currentStatus == 'Drinking status unknown' || currentStatus == 'Denies Usage') {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") != "txtAlcoholComments") {
                        $(this).attr("disabled", "disabled");
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            IA_TabacooScreening.resetControlValue(this);
                        }
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                        IA_TabacooScreening.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            else if (currentStatus == 'Occasional drinker' || currentStatus == 'Social drinker') {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    $(this).removeAttr("disabled");
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                        IA_TabacooScreening.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    // Start 16-12-2015 Muhammad Arshad we dont need to reset values while this status is selected
                    //IA_TabacooScreening.resetControlValue(this);
                    // End 16-12-2015 Muhammad Arshad we dont need to reset values while this status is selected
                });
            }
            //else if (currentStatus == 'Chews Tobacco') {
            //    var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionAlcohol [type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
            //        if ($(this).attr("id") == "txtTobaccoCessationLength" || $(this).attr("id") == "ddlTobaccoType") {
            //            $(this).attr("disabled", "disabled");
            //        }
            //        else {
            //            $(this).removeAttr("disabled");
            //        }
            //    });
            //}
            $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionAlcohol #hfAlcoholDataChangeBit').val("1");
            $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol");
                    detailSection.resetAllControls(null);
                }

            })
        }

        IA_TabacooScreening.enableDisableCounsellingTopic('alcohol');

    },

    cacheAlcoholTabData: function (socialType) {
        var dfd = $.Deferred();
        var updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
        var statusId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulAlcoholStatus > li.active").attr('id');

        if (IA_TabacooScreening.socialHxJSON != updatedJSON) {
            if (statusId != null) {
                $.when(IA_TabacooScreening.cacheSocialHxJSON('Alcohol', statusId, updatedJSON)).then(function () {
                    dfd.resolve();
                });
                IA_TabacooScreening.SetIsLast('Alcohol', statusId);
            }
            else {
                dfd.resolve();
            }
        }
        else {
            if (socialType == 'alcohol') {
                var json = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol").getMyJSONByName();
                var detailsSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol").clone();
                $(detailsSection).resetAllControls(null);
                var emptyJson = $(detailsSection).getMyJSONByName();

                if (statusId != null && statusId > 0 && json == emptyJson) {
                    $.when(IA_TabacooScreening.cacheSocialHxJSON('Alcohol', statusId, updatedJSON)).then(function () {
                        dfd.resolve();
                    });
                    IA_TabacooScreening.SetIsLast('Alcohol', statusId);
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                dfd.resolve();
            }
        }
        return dfd;
    },
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will Enable Disable controls for DrugAbuse on basis of selected Status as specified by currentStatus

    enableDisableDrugAbuseControls: function (obj, currentStatus, currentStatusValue) {

        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && IA_TabacooScreening.socialHxJSON != '') {
            IA_TabacooScreening.cacheDrugTabData();
        }
        if (obj != null) {
            $($(obj).parent()).find("li").each(function () {
                $(this).removeClass("active");
            });
            if ($(obj).hasClass("active") == false) {
                $(obj).addClass("active");
            }
        }
        if (currentStatus != null && currentStatus != "") {
            if (currentStatus == "Current every day drug user" || currentStatus == 'Current someday drug user') {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionDrugAbuse').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "txtDrugCessationLength" || $(this).attr("id") == "ddlDrugCessationPeriod" || $(this).attr("id") == "chkDrugRecentlyQuit") {
                        $(this).attr("disabled", "disabled");
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            IA_TabacooScreening.resetControlValue(this);

                        }

                    }
                    else if ($(this).attr("id") == "ddlDrugType") {
                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val("");
                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                    }
                    else {
                        $(this).removeAttr("disabled");

                        $('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("enable");
                    }
                    //if ($(this).attr("id") == "ddlDrugType") {
                    //    IA_TabacooScreening.enableDisableSTDControls(this);
                    //}

                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                        IA_TabacooScreening.resetControlValue(this);
                        //$('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem
                    //$('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                });
            }
            else if (currentStatus == 'Former drug user') {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionDrugAbuse').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "chkDrugWouldQuit") {
                        $(this).attr("disabled", "disabled");
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            IA_TabacooScreening.resetControlValue(this);
                        }

                    }
                    else if ($(this).attr("id") == "ddlDrugType") {
                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val("");
                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                    }
                    else {
                        $(this).removeAttr("disabled");

                        $('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("enable");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                        IA_TabacooScreening.resetControlValue(this);
                        //$('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem
                    //$('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            else if (currentStatus == 'Never used drugs' || currentStatus == 'Unknown if ever used drugs' || currentStatus == 'Denies Usage') {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionDrugAbuse').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") != "txtDrugComments") {
                        $(this).attr("disabled", "disabled");
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            IA_TabacooScreening.resetControlValue(this);
                            // $('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");

                        }
                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val("");
                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                        $('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("disable");
                        //$('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    }
                    else if ($(this).attr("id") == "ddlDrugType") {
                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val("");
                        $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                    }
                    else {

                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                        IA_TabacooScreening.resetControlValue(this);
                        //$('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    //$('#' + IA_TabacooScreening.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionDrugAbuse #hfDrugAbuseDataChangeBit').val("1");
            $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse");
                    detailSection.resetAllControls(null);
                    $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").val("");
                    $('#' + IA_TabacooScreening.params.PanelID + " #ddlDrugType").multiselect("refresh");
                }
            })
        }
    },
    cacheDrugTabData: function (socialType) {
        var dfd = $.Deferred();
        var updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
        var statusId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulDrugStatus > li.active").attr('id');

        if (IA_TabacooScreening.socialHxJSON != updatedJSON) {
            if (statusId != null) {
                $.when(IA_TabacooScreening.cacheSocialHxJSON('DrugAbuse', statusId, updatedJSON)).then(function () {
                    dfd.resolve();
                });
                IA_TabacooScreening.SetIsLast('DrugAbuse', statusId);
            }
            else {
                dfd.resolve();
            }
        }
        else {
            if (socialType == 'drug' || socialType == 'drug abuse') {
                var json = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse").getMyJSONByName();
                var detailsSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse").clone();
                $(detailsSection).resetAllControls(null);
                $(detailsSection).find("#ddlDrugType").val("");
                $(detailsSection).find("#ddlDrugType").multiselect("refresh");
                var emptyJson = $(detailsSection).getMyJSONByName();

                if (statusId != null && statusId > 0 && json == emptyJson) {
                    $.when(IA_TabacooScreening.cacheSocialHxJSON('DrugAbuse', statusId, updatedJSON)).then(function () {
                        dfd.resolve();
                    });
                    IA_TabacooScreening.SetIsLast('DrugAbuse', statusId);
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                dfd.resolve();
            }
        }
        return dfd;
    },
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will Enable Disable controls for DrugAbuse on basis of selected Status as specified by currentStatus

    enableDisableSexualHxControls: function (obj, currentStatus, currentStatusValue) {

        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && IA_TabacooScreening.socialHxJSON != '') {
            IA_TabacooScreening.cacheSexualTabData();
        }
        if (obj != null) {
            $($(obj).parent()).find("li").each(function () {
                $(this).removeClass("active");
            });
            if ($(obj).hasClass("active") == false) {
                $(obj).addClass("active");
            }
        }
        if (currentStatus != null && currentStatus != "") {
            if (currentStatus == "Practices safe sex") {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "ddlSexualExposedToSTD" || $(this).attr("id") == "ddlSexualSTD" || $(this).attr("id") == "RadSexualYesAbusedSexually" || $(this).attr("id") == "RadSexualNoAbusedSexually") {
                        $(this).attr("disabled", "disabled");
                        if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                            IA_TabacooScreening.resetControlValue(this);
                        }


                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                        IA_TabacooScreening.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    if ($(this).attr("id") == "ddlSexualExposedToSTD") {
                        IA_TabacooScreening.enableDisableSTDControls(this);
                    }
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                });
            }
            else if (currentStatus == 'Sexually Active' || currentStatus == 'Not Sexually Active') {
                var self = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    $(this).removeAttr("disabled");
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (IA_TabacooScreening.SeletedChildStatus != currentStatusValue) {
                        IA_TabacooScreening.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    if ($(this).attr("id") == "ddlSexualExposedToSTD") {
                        IA_TabacooScreening.enableDisableSTDControls(this);
                    }
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }

            $.when(IA_TabacooScreening.fillDetails()).done(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx");
                    detailSection.resetAllControls(null);
                    $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').val('');
                    $('#' + IA_TabacooScreening.params.PanelID + " section#sectionSexualHx #ddlSexualSTD").multiselect("refresh");
                }
            })
        }
    },

    cacheSexualTabData: function (socialType) {
        var dfd = $.Deferred();
        var updatedJSON = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
        var statusId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus > li.active").attr('id');

        if (IA_TabacooScreening.socialHxJSON != updatedJSON) {
            if (statusId != null) {
                $.when(IA_TabacooScreening.cacheSocialHxJSON('Sexual', statusId, updatedJSON)).then(function () {
                    dfd.resolve();
                });
                IA_TabacooScreening.SetIsLast('Sexual', statusId);
            }
            else {
                dfd.resolve();
            }
        }
        else {
            if (socialType == 'sexual' || socialType == 'sexual hx') {
                var json = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx").getMyJSONByName();
                var detailsSection = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx").clone();
                $(detailsSection).resetAllControls(null);
                $(detailsSection).find("#ddlSexualSTD").val("");
                $(detailsSection).find("#ddlSexualSTD").multiselect("refresh");
                var emptyJson = $(detailsSection).getMyJSONByName();

                if (statusId != null && statusId > 0 && json == emptyJson) {
                    $.when(IA_TabacooScreening.cacheSocialHxJSON('Sexual', statusId, updatedJSON)).then(function () {
                        dfd.resolve();
                    });
                    IA_TabacooScreening.SetIsLast('Sexual', statusId);
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                dfd.resolve();
            }
        }
        return dfd;
    },

    MU3SocialHx: function () {

    },

    VarifyMUAlert: function () {
        var PatientId = IA_TabacooScreening.params.patientID;
        var obj_ = utility.MU3Demographics("#" + Patient_Demographic.params.PanelID + " #frmDemographic", PatientId);
        if (obj_ != null) {
            if (Patient_Demographic.params.mode == "Add") {
                Patient_Demographic.SaveMUAlert(obj_).done(function (result) {

                    if (result.status != false) {
                        utility.toggelMU3Alerts(true);
                    }
                    else {
                        console.log(result.Message);
                    }
                });
            }
            else {
                Patient_Demographic.UpdateMUAlert(obj_).done(function (result) {
                    if (result.status != false) {
                        var data = JSON.parse(result.MUAlerts_JSON);
                        var IsAnyOtherAlert = data.filter(item=>item.PatientId == PatientId);
                        if (IsAnyOtherAlert.length > 0 && result.MissingDataAlertCount > 0) {
                            utility.toggelMU3Alerts(true, result.MissingDataAlertCount);
                        }
                        else {
                            utility.toggelMU3Alerts(false, result.MissingDataAlertCount);
                        }
                    }
                    else {
                        console.log(result.Message);
                    }
                });
            }
        }
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Add/Edit of SocialHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous), it expects SocialHxType to be Add/Edit
    socialHxSave: function (SocialHxType, UnloadSocialhx, attachToNote) {

        var SocialHxId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() != "" ? $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() : "-1";
        if (parseInt(SocialHxId) > 0) {
            IA_TabacooScreening.params.mode = "Edit";
        }
        else {
            IA_TabacooScreening.params.mode = "Add";
        }
        //Start 12-07-2016 Edit By Humaira Yousaf for Bug# EMR-1422
        //Start//11/02/2016//Abid Ali// fixed bug#315
        //var overallComments = "";
        //overallComments = $('#' + IA_TabacooScreening.params.PanelID + " #txtSocialComments").val();
        //overallComments = typeof overallComments == "undefined" ? "" : overallComments
        //if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && overallComments != "") {

        //    var DetailExists = true;
        //}
        //else {
        //    DetailExists = IA_TabacooScreening.isDetailExists(SocialHxType.toLowerCase());
        //}
        //if ($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").is(':checked')) {
        //    DetailExists = true;
        //}
        //if (overallComments != "") {
        //    DetailExists = true;
        //}
        //End//11/02/2016//Abid Ali// fixed bug#315

        var DetailExists = true;
        //End 12-07-2016 Edit By Humaira Yousaf for Bug# EMR-1422
        if (DetailExists == true) {
            var strMessage = "";
            var self = null;
            if (SocialHxType.toLowerCase() == "tobacco") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#Tobacco");
            }
            else if (SocialHxType.toLowerCase() == "alcohol") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#Alcohol");
            }
            else if (SocialHxType.toLowerCase() == "drug") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse");
            }
            else if (SocialHxType.toLowerCase() == "sexual") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx");
            }
            else if (SocialHxType.toLowerCase().indexOf("miscellaneous") > -1) {
                IA_TabacooScreening.retainedComponentMisHx = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #MiscHxMainStatus #ulMiscStatus li.active").attr("id");
                if (SocialHxType.toLowerCase() == "miscellaneous_occupation") {
                    self = $('#' + IA_TabacooScreening.params.PanelID + " div#OccupationDetails");
                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_sleep") {
                    self = $('#' + IA_TabacooScreening.params.PanelID + " div#SleepDetails");
                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_exercises") {
                    self = $('#' + IA_TabacooScreening.params.PanelID + " div#ExercisesDetails");

                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_housing") {
                    self = $('#' + IA_TabacooScreening.params.PanelID + " div#HousingDetails");
                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_caffeineintake") {
                    self = $('#' + IA_TabacooScreening.params.PanelID + " div#CaffeineIntakDetails");
                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_travel") {
                    self = $('#' + IA_TabacooScreening.params.PanelID + " div#divTravelDetailsHx");
                }
            }
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);
            if (SocialHxType.toLowerCase() == "drug") {
                var DrugStatusIds = self.find('#ddlDrugType option:selected').map(function () {
                    return this.value;
                }).get().join(',');
                /*
                   Change Implement BY: Muhammad Azhar Shahzad
                   Reason: To get Drug Status text for Soap Text Cteation
                   Created Date: Dec 15, 2015
               */
                var DrugStatusText = self.find('#ddlDrugType option:selected').map(function () {
                    return this.text;
                }).get().join(',');
                objData["DrugType"] = DrugStatusIds;
                objData["DrugType_text"] = DrugStatusText;
            }
            // Start 07/01/2016 Muhammad Arshad	MiscHx Related code
            if (SocialHxType.toLowerCase().indexOf("miscellaneous") > -1) {
                objData["MiscChildStatus"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id");
                objData["MiscChildStatusText"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text();
                if (SocialHxType.toLowerCase() == "miscellaneous_exercises") {
                    objData["ExercisesTypeText"] = $('#' + IA_TabacooScreening.params.PanelID + " div#ExercisesDetails").find('#ddlExercisesType option:selected').text();
                    objData["ExercisesDietText"] = $('#' + IA_TabacooScreening.params.PanelID + " div#ExercisesDetails").find('#ddlExercisesDiet option:selected').text();
                }

            }
            // End 07/01/2016 Muhammad Arshad	MiscHx Related code
            if (SocialHxType.toLowerCase() == "sexual") {
                var STDIds = "";
                var STDtext = "";
                if (self.find('#ddlSexualExposedToSTD option:selected').val() != "" && self.find('#ddlSexualExposedToSTD option:selected').val() != "No") {


                    STDIds = self.find('#ddlSexualSTD option:selected').map(function () {
                        return this.value;
                    }).get().join(',');
                    /*
                       Change Implement BY: Muhammad Azhar Shahzad
                       Reason: To get Drug Status text for Soap Text Cteation
                       Created Date: Dec 15, 2015
                   */
                    STDtext = self.find('#ddlSexualSTD option:selected').map(function () {
                        return this.text;
                    }).get().join(',');
                }
                objData["SexualSTD"] = STDIds;
                objData["SexualSTD_text"] = STDtext;
                if (self.find('#RadSexualYesAbusedSexually').is(':checked') == true) {
                    objData["RadSexualAbusedSexually"] = true;
                }
                else if (self.find('#RadSexualNoAbusedSexually').is(':checked') == true) {
                    objData["RadSexualAbusedSexually"] = false;
                } else {
                    objData["RadSexualAbusedSexually"] = "";
                }

                if (self.find('#RadSexualYesPainWithIntercourse').is(':checked') == true) {
                    objData["RadSexualPainWithIntercourse"] = true;
                }
                else if (self.find('#RadSexualNoPainWithIntercourse').is(':checked') == true) {
                    objData["RadSexualPainWithIntercourse"] = false;
                } else {
                    objData["RadSexualPainWithIntercourse"] = "";
                }
                if (self.find('#RadSexualYesPregnant').is(':checked') == true) {
                    objData["RadSexualPregnant"] = true;
                }
                else if (self.find('#RadSexualNoPregnant').is(':checked') == true) {
                    objData["RadSexualPregnant"] = false;
                } else {
                    objData["RadSexualPregnant"] = "";
                }
                objData["SexualHxPregnancyDuration"] = self.find('#txtSexualHxPregnancyDuration').val();
            }
            objData["SocialHxId"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val();
            objData["SocialHxType"] = SocialHxType != null ? SocialHxType : "";
            objData["SocialHxDate"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
            objData["SocialHxUnremarkable"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
            objData["SocialComments"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();
            myJSON = JSON.stringify(objData);
            //Begin 15-07-2016 Edit By Humaira Yousaf Bug#1582
            if (SocialHxType.toLowerCase() == "tobacco" || SocialHxType.toLowerCase() == "alcohol" || SocialHxType.toLowerCase() == "drug") {
                var msg = IA_TabacooScreening.isCessationValid(SocialHxType.toLowerCase());
                if (msg != "") {
                    IA_TabacooScreening.isSaved = false;
                    utility.DisplayMessages(msg, 3);
                    return;
                }
            }
            //End 15-07-2016 Edit By Humaira Yousaf Bug#1582
            //return false;
            if (IA_TabacooScreening.params.mode == "Add") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Social Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        IA_TabacooScreening.saveSocialHx(myJSON).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                IA_TabacooScreening.params.HxTypeId = response.SocialHxId;

                                if (response.MUAlertsCount && response.MUAlertsCount > 0)
                                    utility.toggelMU3Alerts(true, response.MUAlertsCount);
                                else
                                    utility.toggelMU3Alerts(false, 0);

                                $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #divSwitch").removeClass('disableAll');
                                if (!attachToNote) {
                                    IA_TabacooScreening.ChangeCurrentPast(1, null, null, null);
                                    IA_TabacooScreening.BindCurrentSocialHxSoapText(response, false);
                                }
                                LastSocialHx = new Object();
                                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                                LastSocialHx["SocialHxType"] = $('#' + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');

                                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(response.SocialHxId);
                                IA_TabacooScreening.params.mode = "Edit";
                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && UnloadSocialhx == true) {
                                    if (!attachToNote) {
                                        IA_TabacooScreening.getSocialHxInfo(SocialHxType, UnloadSocialhx);
                                    }
                                } else {
                                    utility.DisplayMessages(response.message, 1);
                                }
                                //Start//17/12/2015//Ahmad Raza//Serializing the form
                                $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());
                                //End//17/12/2015//Ahmad Raza//Serializing the form
                                IA_TabacooScreening.BindCurrentSocialHxSoapText(response, true);
                                IA_TabacooScreening.isSaved = true;
                            }
                            else {
                                IA_TabacooScreening.isSaved = false;
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
                //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
            }
            else if (IA_TabacooScreening.params.mode == "Edit") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Social Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        IA_TabacooScreening.updateSocialHx(myJSON, IA_TabacooScreening.params.SocialHxId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {

                                if (response.MUAlertsCount && response.MUAlertsCount > 0)
                                    utility.toggelMU3Alerts(true, response.MUAlertsCount);
                                else
                                    utility.toggelMU3Alerts(false, 0);

                                $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #divSwitch").removeClass('disableAll');
                                if (!attachToNote) {
                                    IA_TabacooScreening.ChangeCurrentPast(1, null, null, null);
                                    IA_TabacooScreening.BindCurrentSocialHxSoapText(response, false);
                                }
                                LastSocialHx = new Object();
                                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                                LastSocialHx["SocialHxType"] = $('#' + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');

                                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && UnloadSocialhx == true) {
                                    if (!attachToNote) {
                                        IA_TabacooScreening.getSocialHxInfo(SocialHxType, UnloadSocialhx);
                                    }
                                } else {
                                    //IA_TabacooScreening.AppointmentStatusSearch(IA_TabacooScreening.params.SocialHxignsId);
                                    utility.DisplayMessages(response.message, 1);
                                }

                                //Start//17/12/2015//Ahmad Raza//Serializing the form
                                $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').serialize());
                                //End//17/12/2015//Ahmad Raza//Serializing the form

                                IA_TabacooScreening.isSaved = true;
                                IA_TabacooScreening.BindCurrentSocialHxSoapText(response, true);
                            }
                            else {
                                IA_TabacooScreening.isSaved = false;
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
                //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
            }
        }
        else {
            //Begin 12-28-2015 Muhammad Arshad Bug# EMR-159 Social History Clinical Module -> Save
            utility.DisplayMessages("Please enter any value", 3);
            //End 12-28-2015 Muhammad Arshad Bug# EMR-159 Social History Clinical Module -> Save
        }

    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle load of SocialHx and it's childs as specified by SocialHxType
    //It represents service call to API
    fillSocialHx: function (SocialHxType) {
        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["SocialHxType"] = SocialHxType != null ? SocialHxType : "tobacco";
        objData["commandType"] = "FILL_SocialHx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHx");
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Add of SocialHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous)
    //It represents service call to API
    saveSocialHx: function (SocialHxData) {
        var objData = JSON.parse(SocialHxData);
        if (IA_TabacooScreening.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = IA_TabacooScreening.params.patientID;
        }
        objData["commandType"] = "SAVE_SocialHx";
        //Start//18/01/2016//Ahmad Raza//checkbox's value handled
        if ($('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #RadYesCaffeineharmful').is(':checked')) {
            objData["RadCaffieneharmful"] = "True";
        }
        else if ($('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #RadNoCaffeineharmful').is(':checked')) {
            objData["RadCaffieneharmful"] = "False";
        }
        else {
            objData["RadCaffieneharmful"] = "";
        }
        //End//18/01/2016//Ahmad Raza//checkbox's value handled
        var data = JSON.stringify(objData);

        //var data = "SocialHxignsData=" + SocialHxignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "SocialHx");
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Edit of SocialHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous)
    //It represents service call to API
    updateSocialHx: function (SocialHxData, SocialHxId) {

        var objData = JSON.parse(SocialHxData);
        if (IA_TabacooScreening.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = IA_TabacooScreening.params.patientID;
        }

        objData["commandType"] = "UPDATE_SOCIALHx";
        //Start//18/01/2016//Ahmad Raza//checkbox's value handled
        if ($('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #RadYesCaffeineharmful').is(':checked')) {
            objData["RadCaffieneharmful"] = "True";
        }
        else if ($('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #RadNoCaffeineharmful').is(':checked')) {
            objData["RadCaffieneharmful"] = "False";
        }
        else {
            objData["RadCaffieneharmful"] = "";
        }
        //End//18/01/2016//Ahmad Raza//checkbox's value handled
        var data = JSON.stringify(objData);

        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "SocialHx");

    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Unload of SocialHx Tab

    unLoadTab: function (NextOrPre, controlToInvoke, bFromNote) {

        if (IA_TabacooScreening.params.ParentPanelID) {
            UnloadActionPan(IA_TabacooScreening.params.ParentCtrl, 'IA_TabacooScreening', null, IA_TabacooScreening.params.ParentPanelID);
        }
        else
            UnloadActionPan(IA_TabacooScreening.params.ParentCtrl);






    },

    UnloadSocialHistory: function (NextOrPre) {

        var socialHxMainHtml;
        if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote") {

            IA_TabacooScreening.params.aTagTabPages = $('#pnlClinicalProgressNote #pnlTabacooScreening').find('section#removeDiv > ul').clone();
            IA_TabacooScreening.params.divTagTabPages = $('#pnlClinicalProgressNote #pnlTabacooScreening').find('section#removeDiv > div.tab-content').clone();
            socialHxMainHtml = $.trim($('#ctrlPanClinical > #pnlTabacooScreening').find('section#removeDiv').html());
        }

        if (IA_TabacooScreening.params["FromAdmin"] == "0") {
            if (IA_TabacooScreening.params != null && IA_TabacooScreening.params.ParentCtrl != null) {
                if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && NextOrPre == true)
                    UnloadActionPan(IA_TabacooScreening.params.ParentCtrl, 'Clinical_SurgicalHx');
                if (IA_TabacooScreening.controlToInvoke != null) {
                    setTimeout(function () {
                        Clinical_ProgressNote.SelectNotesComponentTab(IA_TabacooScreening.controlToInvoke);
                        IA_TabacooScreening.controlToInvoke = null;
                    }, 400);
                }
                else {
                    UnloadActionPan(IA_TabacooScreening.params.ParentCtrl, 'Clinical_SurgicalHx');
                    if (IA_TabacooScreening.controlToInvoke != null)
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(IA_TabacooScreening.controlToInvoke);
                            IA_TabacooScreening.controlToInvoke = null;
                        }, 400);
                }


                if (socialHxMainHtml == null || typeof socialHxMainHtml == 'undefined' || socialHxMainHtml == "") {

                    IA_TabacooScreening.resetMainSocialHxTabPageHtml(IA_TabacooScreening.params.aTagTabPages, IA_TabacooScreening.params.divTagTabPages);
                }
            }
            else {
                UnloadActionPan(null, 'IA_TabacooScreening');

                if (socialHxMainHtml == null || typeof socialHxMainHtml == 'undefined' || socialHxMainHtml == "") {

                    IA_TabacooScreening.resetMainSocialHxTabPageHtml(IA_TabacooScreening.params.aTagTabPages, IA_TabacooScreening.params.divTagTabPages);
                }
            }

        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_SocialHx").remove();
            RemoveAdminTab();

            if (socialHxMainHtml == null || typeof socialHxMainHtml == 'undefined' || socialHxMainHtml == "") {

                IA_TabacooScreening.resetMainSocialHxTabPageHtml(IA_TabacooScreening.params.aTagTabPages, IA_TabacooScreening.params.divTagTabPages);
            }
        }
        EMRUtility.scrollToPNcomponent('IA_TabacooScreening');
    },

    /*
    Author: Muhammad Arshad
    Date: 12/16/2015
    This function will handle the color of sub tabs for each tab in SocialHx if some data is presenet in tab
    */

    changeTabColor: function (tabName) {
        if (tabName != null && tabName.toLowerCase() == "tobacco") {
            $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox') {
                    if ($(this).is(':checked')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != "") {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').removeClass('successLight');
                    }
                }
            });

            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel.length > 0) {
                    if (!$('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').hasClass('successLight')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listTobacco').addClass('successLight');
                    }
                }
            }
        }
        else if (tabName != null && tabName.toLowerCase() == "alcohol") {
            $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox') {
                    if ($(this).is(':checked')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != "") {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').removeClass('successLight');
                    }
                }
            });
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel.length > 0) {
                    if (!$('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').hasClass('successLight')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listAlcohol').addClass('successLight');
                    }
                }
            }
        }
        else if (tabName != null && tabName.toLowerCase() == "drug") {
            $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionDrugAbuse').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox') {
                    if ($(this).is(':checked')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != "" && $(this).val() != null) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').removeClass('successLight');
                    }
                }
            });
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel.length > 0) {
                    if (!$('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').hasClass('successLight')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                    }
                }
            }
        }
        else if (tabName != null && tabName.toLowerCase() == "sexual") {
            $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox' || $(this).attr("type") == "radio") {
                    if ($(this).is(':checked') && $(this).attr("id") != "RadSexualNoPainWithIntercourse" && $(this).attr("id") != "RadSexualNoAbusedSexually" && $(this).attr("id") != "RadSexualNoPregnant") {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != null && $(this).val() != "") {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').removeClass('successLight');
                    }
                }
            });
            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel.length > 0) {
                    if (!$('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').hasClass('successLight')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listSexualHx').addClass('successLight');
                    }
                }
            }
        }
        else if (tabName != null && tabName.toLowerCase() == "miscellaneous_components") {
            $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionMiscDetails').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox') {
                    if ($(this).is(':checked')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').removeClass('successLight');
                    }
                }
                else if ($(this).attr('type') == 'radio') {
                    if ($(this).is(':checked') && $(this).attr("id") != "RadOccupationPresenttExperience") {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != "") {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').removeClass('successLight');
                    }
                }
            });

            if (IA_TabacooScreening.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel.length > 0) {
                    if (!$('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').hasClass('successLight')) {
                        $('#' + IA_TabacooScreening.params.PanelID + ' #listMiscHx').addClass('successLight');
                    }
                }
            }
        }
    },

    /*
    Author: Muhammad Irfan
    Date: 07/12/2015
    This function will handle the color of sub tabs in SocialHx if some data is presenet in tab
    then on navigation of tab change the color of tab to green
    */
    selectTabColor: function (ctrlTab) {
        if (ctrlTab == "tobacco") {
            IA_TabacooScreening.changeTabColor("alcohol");
            IA_TabacooScreening.changeTabColor("drug");
            IA_TabacooScreening.changeTabColor("sexual");
            IA_TabacooScreening.changeTabColor("miscellaneous_components");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
        else if (ctrlTab == "alcohol") {
            IA_TabacooScreening.changeTabColor("tobacco");
            IA_TabacooScreening.changeTabColor("drug");
            IA_TabacooScreening.changeTabColor("sexual");
            IA_TabacooScreening.changeTabColor("miscellaneous_components");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
        else if (ctrlTab == "drug") {

            IA_TabacooScreening.changeTabColor("tobacco");
            IA_TabacooScreening.changeTabColor("alcohol");
            IA_TabacooScreening.changeTabColor("sexual");
            IA_TabacooScreening.changeTabColor("miscellaneous_components");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
        else if (ctrlTab == "sexual") {
            IA_TabacooScreening.changeTabColor("tobacco");
            IA_TabacooScreening.changeTabColor("alcohol");
            IA_TabacooScreening.changeTabColor("drug");
            IA_TabacooScreening.changeTabColor("miscellaneous_components");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
        else if (ctrlTab == "miscellaneous_components") {
            IA_TabacooScreening.changeTabColor("tobacco");
            IA_TabacooScreening.changeTabColor("alcohol");
            IA_TabacooScreening.changeTabColor("drug");
            IA_TabacooScreening.changeTabColor("sexual");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + IA_TabacooScreening.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
    },


    //-----------------Progress Note-------------
    // added on Dec 14,2015 by Muhammad Azhar Shahzad
    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Call Back function to add component to Progress Note
    addSocialHxToNotes: function () {
        var SocialHxId = IA_TabacooScreening.params.SocialHxId;
        var socialHxType = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
        IA_TabacooScreening.socialHxSave(socialHxType, true);
    },

    //this function will get Social History Soap Text and attach that to Progress note
    getSocialHxInfo: function (socialHxType, UnloadSocialhx) {
        IA_TabacooScreening.fillSocialHx(socialHxType).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    IA_TabacooScreening.createSocialHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadSocialhx);

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //This Function will check, if Social History Soap is already attached in Progress note, if Social History is not attached than it will create main divs to attach allergy
    checkSocialHxExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML IA_TabacooScreening').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="SocialHxComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<IA_TabacooScreening title="Social Hx"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'SocialHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="SocialHx">Social Hx</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'SocialHx\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'SocialHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</IA_TabacooScreening> </header></li>');


            Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
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
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createSocialHxBodyHTML: function (response, NoteHTMLCtrl, UnloadSocialhx, hideAlertMessage) {
        IA_TabacooScreening.checkSocialHxExists();
        if (response.SocialHxFill_JSON != null && response.SocialHxFill_JSON != '') {
            var SocialHxFill_Obj = JSON.parse(response.SocialHxFill_JSON);
            var $mainDivSocialHx = $(document.createElement('div'));

            var socialHxId = SocialHxFill_Obj.SocialHxId;
            var $SectionBodySocialHx = $(document.createElement('section'));
            $SectionBodySocialHx.attr('id', "Cli_SocialHx_Main" + socialHxId);
            var $DetailsDiv = $(document.createElement('div'));
            $DetailsDiv.attr('id', "Cli_SocialHx_" + socialHxId);
            var $ListSocialHx = $(document.createElement('ul'));

            $ListSocialHx.attr('class', 'list-unstyled')

            $SectionBodySocialHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_SocialHx_" + socialHxId + '"><i class="fa fa-edit"></i></a>' +
                '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_SocialHx_Main" + socialHxId + '"  ><i class="fa fa-times"></i></a></div> ');


            $ListSocialHx.append("<li>" + SocialHxFill_Obj.SocialHxSoapText + "</li>");
            $DetailsDiv.append($ListSocialHx);
            $SectionBodySocialHx.append($DetailsDiv);
            if ($(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).length == 0) {
                $mainDivSocialHx.append($SectionBodySocialHx);
                IA_TabacooScreening.updateSocialHxHtml($mainDivSocialHx.html(), socialHxId, NoteHTMLCtrl, hideAlertMessage);
            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).html($SectionBodySocialHx.html());
                $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul').append(CommentHTML);
                Clinical_ProgressNote.saveComponentSOAPText("SocialHx", hideAlertMessage);
                IA_TabacooScreening.updateSocialHxHtml("", socialHxId, NoteHTMLCtrl, hideAlertMessage);

            }

            if (UnloadSocialhx == true) {
                IA_TabacooScreening.UnloadSocialHistory(IA_TabacooScreening.bNextPrev);
            }
        }
    },

    createSocialHxBodyHTMLFromNotes: function (SocialHistory, NoteHTMLCtrl, UnloadSocialhx, hideAlertMessage) {

        IA_TabacooScreening.checkSocialHxExists();

        if (SocialHistory && SocialHistory.SocialHxId && SocialHistory.SocialHxId > 0) {
            var SocialHxFill_Obj = SocialHistory;
            var $mainDivSocialHx = $(document.createElement('div'));

            var socialHxId = SocialHxFill_Obj.SocialHxId;
            var $SectionBodySocialHx = $(document.createElement('section'));
            $SectionBodySocialHx.attr('id', "Cli_SocialHx_Main" + socialHxId);
            var $DetailsDiv = $(document.createElement('div'));
            $DetailsDiv.attr('id', "Cli_SocialHx_" + socialHxId);
            var $ListSocialHx = $(document.createElement('ul'));

            $ListSocialHx.attr('class', 'list-unstyled')

            $SectionBodySocialHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_SocialHx_" + socialHxId + '"><i class="fa fa-edit"></i></a>' +
                '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_SocialHx_Main" + socialHxId + '"  ><i class="fa fa-times"></i></a></div> ');

            $ListSocialHx.append("<li>" + SocialHxFill_Obj.SoapText + "</li>");
            $DetailsDiv.append($ListSocialHx);
            $SectionBodySocialHx.append($DetailsDiv);

            if ($(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).length == 0) {
                $mainDivSocialHx.append($SectionBodySocialHx);
                var SocialHxHtml = $mainDivSocialHx.html();

                $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().addClass('initialVisitBody');
                if (SocialHxHtml != '') {
                    $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().append(SocialHxHtml);
                }

                //Binding Hovering and onClick functions to Progress Note HTML
                Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
                return socialHxId;

            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).html($SectionBodySocialHx.html());
                $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul').append(CommentHTML);
            }
        }
    },

    createSocialHxBodyHTMLFromNote: function (response, NoteHTMLCtrl, UnloadSocialhx, hideAlertMessage) {
        var dfd = $.Deferred();
        IA_TabacooScreening.checkSocialHxExists();

        if (response) {
            var SocialHxFill_Obj = response;
            var $mainDivSocialHx = $(document.createElement('div'));
            var socialHxId = SocialHxFill_Obj.SocialHxId;

            var $SectionBodySocialHx = $(document.createElement('section'));
            $SectionBodySocialHx.attr('id', "Cli_SocialHx_Main" + socialHxId);
            var $DetailsDiv = $(document.createElement('div'));
            $DetailsDiv.attr('id', "Cli_SocialHx_" + socialHxId);
            var $ListSocialHx = $(document.createElement('ul'));

            $ListSocialHx.attr('class', 'list-unstyled')

            $SectionBodySocialHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_SocialHx_" + socialHxId + '"><i class="fa fa-edit"></i></a>' +
                '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_SocialHx_Main" + socialHxId + '"  ><i class="fa fa-times"></i></a></div> ');


            $ListSocialHx.append("<li>" + SocialHxFill_Obj.SocialHxSoapText + "</li>");
            $DetailsDiv.append($ListSocialHx);
            $SectionBodySocialHx.append($DetailsDiv);
            if ($(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).length == 0) {
                $mainDivSocialHx.append($SectionBodySocialHx);
                var SocialHxHtml = $mainDivSocialHx.html();
                if (SocialHxHtml != '') {
                    $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().addClass('initialVisitBody');
                    $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().append(SocialHxHtml);
                }
            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).html($SectionBodySocialHx.html());
                $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul').append(CommentHTML);
            }
        }

        dfd.resolve();
        return dfd;
    },

    createAlcoholHTML: function () {
        var AlcoholHTML = "<li id='socialAlcohol_" + item.AlcoholId + "' title='Alcohol'><strong>Alcohol: </strong>";
        AlcoholHTML += IA_TabacooScreening.IsNullReturnSoapValue(item.Status) + IA_TabacooScreening.IsNullReturnSoapValue(item.Type) + IA_TabacooScreening.IsNullReturnSoapValue(item.Frequency) + (string.IsNullOrEmpty(item.UsagePeriod) ? "" : " for " + item.UsagePeriod);
        AlcoholHTML += ", Patient has quit " + item.CessationLength + " ago" + (item.bWouldQuit ? " , Patient would quit" : "") + (item.bRecentlyQuit ? " , Patient recently quit" : "") + (item.bNotReadyToQuit ? " , Patient not ready to quit" : "");
        AlcoholHTML += ", Counselling " + item.CounsellingPeriod + " for " + item.CounsellingTopic + ", " + item.Comments + "</li>";
    },

    createDrugAbuseHTML: function () {
        var DrugAbuseHTML = "<li id='socialDrugAbuse_" + item.DrugAbuseId + "' title='Drug Abuse'><strong>Drug Abuse: </strong>";
        DrugAbuseHTML += IA_TabacooScreening.IsNullReturnSoapValue(item.Status) + IA_TabacooScreening.IsNullReturnSoapValue(item.DrugAbuseId.ToString()) + IA_TabacooScreening.IsNullReturnSoapValue(item.FrequencyDaily) + " for " + item.UsagePeriod;
        DrugAbuseHTML += ", Patient has quit " + item.CessationLength + " ago" + (item.bWouldQuit ? " , Patient would quit" : "") + (item.bRecentlyQuit ? " , Patient recently quit" : "") + ", " + item.Comments + "</li>";
    },

    createTabaccoHTML: function () {
        var TabaccoHTML = "<li id='socialTobacco_" + item.TobaccoId + "' title='Tobacco' name=''><strong>Tobacco: </strong>";
        TabaccoHTML += IA_TabacooScreening.IsNullReturnSoapValue(item.Status) + IA_TabacooScreening.IsNullReturnSoapValue(item.Type) + IA_TabacooScreening.IsNullReturnSoapValue(item.Frequency);
        TabaccoHTML += (string.IsNullOrEmpty(item.UsagePeriod) ? "" : " for " + item.UsagePeriod) + (string.IsNullOrEmpty(item.CessationLength.ToString()) ? "" : ", Patient has quit " + item.CessationLength + " ago");
        TabaccoHTML += (item.bRecentlyQuit ? " , Patient recently quit" : "") + (item.bWouldQuit ? " , Patient would quit" : "") + ", Counselling " + item.CounsellingPeriod + " for " + item.CounsellingTopic + ", " + item.Comments + "</li>";
    },
    IsNullReturnSoapValue: function (SoapValue) {
        return (SoapValue == "") ? "" : SoapValue + ",";
    },
    createSexualHxHTML: function () {
        if ($('#hfSocialHxSoapText').val() == "") {

        }
        var SexualHxHTML = "<li id='socialSexualHx_" + item.SexualHxId + "' title='Sexual Hx'><strong>Sexual Hx: </strong>";
        SexualHxHTML += IA_TabacooScreening.IsNullReturnSoapValue(item.Status) + (string.IsNullOrEmpty(item.Preference) ? "" : ", Prefers " + item.Preference) + ", Using Protection: " + (item.bUSingProtection ? "Yes" : "No");
        SexualHxHTML += ", Method: " + item.ProtectionMethod + ", How often: +item.howOften+, Patient has complaints of " + item.Complaint + ", Exposed to STD:" + (item.bExposedToSTD ? "Yes" : "No");
        SexualHxHTML += ", STD: + item.DrugName+, Experiences pain with intercourse: " + (item.bPainWithIntercourse ? "Yes" : "No") + ", Abused sexually: " + (item.bSexuallyAbused ? "Yes" : "No") + ", Last Menstrual Period is " + item.LMP + ", " + item.Comments + "<Overall Comments></li>";
    },

    // This Function is called by Progress Notes (Fill SocialHx Func, CopyAllNotesCategories)
    updateSocialHxHtml: function (SocialHxHtml, socialHxId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().addClass('initialVisitBody');
        if (SocialHxHtml != '') {
            $(NoteHTMLCtrl + ' IA_TabacooScreening').parent().parent().append(SocialHxHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (SocialHxHtml != '') {
            IA_TabacooScreening.AttachSocialHxFromNotes(socialHxId, hideAlertMessage);
        }

    },

    enableDurationValidation: function () {



        var stayLength = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #txtTobaccoCessationLength').val();
        var ddlVal = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ddlTobaccoCessationPeriod').val();
        if (stayLength != null && stayLength != '') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationPeriod', true);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration<span class="required">*</span>');
        }
        else {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationPeriod', false);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration');
        }
        if (ddlVal != null && ddlVal != '') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationLength', true);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration<span class="required">*</span>');

        } else {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationLength', false);
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration');
        }

    },

    //This Function detach Social History From progress note
    detach_ComponentsSocialHx: function (ComponentName, IsUpdate, SocialHxComponentRemove) {
        var IA_TabacooScreeningIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML IA_TabacooScreening').parent().parent().find('section[id*="Cli_SocialHx_Main"]').map(function () {
            return this.id.replace("Cli_SocialHx_Main", "");
        }).get().join(',');

        if (SocialHxComponentRemove) {

            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML IA_TabacooScreening').parent().parent().attr('NoteComponentId');
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Social Hx']").remove();
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML IA_TabacooScreening').parent().parent().remove();
            var hxComponents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .HxComponent').length;

            if (NoteComponentId && NoteComponentId != "NCDummyId" && hxComponents == 0) {
                $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            }
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML IA_TabacooScreening').parent().parent().find('section[id*="Cli_SocialHx_Main"]').remove();
        }

        if (IA_TabacooScreeningIds == "" || IA_TabacooScreeningIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText("SocialHx", true);
            //Clinical_ProgressNote.Detach_ComponentsOthers(ComponentName, true);
        }
        else {
            IA_TabacooScreening.DetachSocialHxFromNotes_DBCall(IA_TabacooScreeningIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText("SocialHx", true);
                        //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                    Clinical_ProgressNote.Add_NoText("SocialHx");
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    //This Functions ask for Detaching Social Hx from Progress Note for current Patient Selected
    detachSocialHxFromNotes: function (SocialHxId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = SocialHxId.replace('Cli_SocialHx_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    IA_TabacooScreening.DetachSocialHxFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + SocialHxId).remove();
                            Clinical_ProgressNote.Add_NoText();
                            Clinical_ProgressNote.saveComponentSOAPText("SocialHx", true);
                            // Clinical_ProgressNote.updateProgressNoteHTML();
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
            }, function () {
            },
                '1'
            );
        }
        else
            utility.DisplayMessages(strMessage, 2);
        // });
    },

    //This Functions attached Social Hx to Progress Note for current Patient Selected
    AttachSocialHxFromNotes: function (SocialHxId, hideAlertMessage) {
        IA_TabacooScreening.AttachSocialHxFromNotes_DBCall(SocialHxId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                //If Attached SocialHx Made new inseration to SocialHx Table than good ids should be attached to HTML
                Clinical_ProgressNote.saveComponentSOAPText("SocialHx", hideAlertMessage);
                $('#' + SocialHxId).remove();

                // utility.DisplayMessages(response.Message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    //If SocialHx Component which is dropeed in Progress note has no SocialHx attached, than it will call for Latest SocialHx for this patient
    getLatestSocialHxByPatientId: function (hideAlertMessage, droppedComponent) {

        IA_TabacooScreening.getLatestIA_TabacooScreeningByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                IA_TabacooScreening.createSocialHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
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
        objData["ComponentType"] = "SocialHistory";
        objData["commandType"] = "getautopopulatesetting";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");

    },
    //-----Server calls of Notes----------
    DetachSocialHxFromNotes_DBCall: function (SocialHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["SocialHxId"] = SocialHxId;
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
        objData["commandType"] = "detach_socialhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "SocialHx");
    },

    AttachSocialHxFromNotes_DBCall: function (SocialHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["SocialHxId"] = SocialHxId;
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
        objData["commandType"] = "attach_socialhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "SocialHx");
    },

    getLatestIA_TabacooScreeningByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_socialhxby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHx");
    },
    // end Azhar Change on Dec 15,2015
    //--------------end progress Note-----------


    // Date: 28/12/2015
    // Author: Muhammad Irfan
    // Overview: Enable disable sexualHx controls on change of Using protection
    enableDisableUsingProtection: function (obj) {
        var objProtectionMethod = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualProtectionMethod');
        var objProtectionPeriod = $('#' + IA_TabacooScreening.params.PanelID + ' section#sectionSexualHx #ddlSexualProtectionPeriod');
        if (obj != null) {

            if ($(obj).find("option:selected").val().toLowerCase() == "yes") {
                objProtectionMethod.removeAttr("disabled");
                objProtectionPeriod.removeAttr("disabled");
            }
            else {
                objProtectionMethod.attr("disabled", "disabled");
                objProtectionPeriod.attr("disabled", "disabled");
                objProtectionMethod.val('');
                objProtectionPeriod.val('');
                //IA_TabacooScreening.resetControlValue(objProtectionMethod);
                //IA_TabacooScreening.resetControlValue(objProtectionPeriod);
            }
        }
        else {
            objSTD.removeAttr("disabled");
        }
    },

    // Date: 14/01/2016
    // Author: Muhammad Irfan
    // Overview: This function will sort the miscHx sorting order
    updateComponentOrderSorting: function (miscCompnentSorted) {
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges(" Face Sheet", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        IA_TabacooScreening.updateMiscOrderSorting_Dbcall(miscCompnentSorted).done(function (response) {
        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    // Date: 14/01/2016
    // Author: Muhammad Irfan
    // Overview: This function will call api service
    updateMiscOrderSorting_Dbcall: function (MiscComponentSorted) {

        var objData = new Object();
        objData["MiscComponentSortedOrder"] = MiscComponentSorted;
        objData["UserId"] = globalAppdata['AppUserId'];
        objData["commandType"] = "UPDATE_COMPONENTORDERSORTING";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHx");

    },

    // Date: 01/18/2016
    // Author: Abid Ali
    // Overview: This function will remove tabpage html from main socialHx form
    removeMainSocialHxTabPageHtml: function () {
        //$('#ctrlPanClinical > #pnlTabacooScreening').find('section#removeDiv > ul').remove();
        //$('#ctrlPanClinical > #pnlTabacooScreening').find('section#removeDiv > div.tab-content').remove();
        $('#ctrlPanClinical > #pnlTabacooScreening').remove();
        RemoveCurrentTab("clinicalTabSocialHx");
    },

    showSocialHxTobaccoHistory: function (tobaccoId) {
        //Begin 03-07-2016 Edit By Humaira Yousaf Bug# 1358
        var ParentCtrl = 'clinicalTabSocialHx';
        var ParentCtrlPanelID = null;
        if (IA_TabacooScreening.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'IA_TabacooScreening';
            ParentCtrlPanelID = IA_TabacooScreening.params.PanelID;
        }
        else if (IA_TabacooScreening.params["ParentCtrl"] == "clinicalTabFaceSheet") {
            ParentCtrl = 'IA_TabacooScreening';
            ParentCtrlPanelID = IA_TabacooScreening.params.PanelID;
        }
        EMRUtility.showCurrentItemHistory(IA_TabacooScreening.params.PanelID, null, null, "SocialHx_Tobacco", null, 'IA_TabacooScreening', tobaccoId, ParentCtrlPanelID);
        //End 03-07-2016 Edit By Humaira Yousaf Bug# 1358

    },

    showSocialHxItemHistory: function (socialHxId, tableName) {
        //Start 03-07-2016 Edit By Humaira Yousaf Bug# 1358

        var ParentCtrl = 'clinicalTabSocialHx';
        var ParentCtrlPanelID = null;
        //if (IA_TabacooScreening.params["ParentCtrl"] == "clinicalTabProgressNote") {
        //    ParentCtrl = 'IA_TabacooScreening';
        //    ParentCtrlPanelID = IA_TabacooScreening.params.PanelID;
        //}


        if (IA_TabacooScreening.params.TabID == "clinicalTabProgressNote") {

            ParentCtrl = "Clinical_HistorySummary";
            ParentCtrlPanelID = "pnlClinicalProgressNote";
        }
        else if (IA_TabacooScreening.params["ParentCtrl"] == "clinicalTabFaceSheet") {
            ParentCtrl = 'IA_TabacooScreening';
            ParentCtrlPanelID = IA_TabacooScreening.params.PanelID;
        }
        EMRUtility.showCurrentItemHistory(IA_TabacooScreening.params.PanelID, null, null, tableName, null, 'IA_TabacooScreening', socialHxId, ParentCtrlPanelID);
        //End 03-07-2016 Edit By Humaira Yousaf Bug# 1358

    },

    // Date: 01/18/2016
    // Author: Abid Ali
    // Overview: This function will reset tabpage html of main socialHx form
    resetMainSocialHxTabPageHtml: function (ul, tabContent) {
        //Append tabpages to main MainSocialHx
        $('#ctrlPanClinical > #pnlTabacooScreening').find('section#removeDiv').append(ul).append(tabContent);
        //reset multi-select dropdown list
        $('#ctrlPanClinical > #pnlTabacooScreening').find("#ddlSexualSTD").next().remove();
        $('#ctrlPanClinical > #pnlTabacooScreening').find("#ddlSexualSTD").multiselect('rebuild');
        $('#ctrlPanClinical > #pnlTabacooScreening').find("#ddlDrugType").next().remove();
        $('#ctrlPanClinical > #pnlTabacooScreening').find("#ddlDrugType").multiselect('rebuild');
    },
    // Date: 12-07-2016
    // Author: Humaira Yousaf
    // Overview: Enable Disable Counselling Topic dropdown
    enableDisableCounsellingTopic: function (sender) {

        if (sender == "tobacco") {
            var selectedVal = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingPeriod").val();
            if (selectedVal == "" || selectedVal == "4") {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingTopic").addClass("disableAll");
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingTopic").val($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingTopic option:first").val());
            }
            else {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingTopic").removeClass("disableAll");
            }
        }
        else if (sender == "alcohol") {
            var selectedVal = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingPeriod").val();
            if (selectedVal == "" || selectedVal == "4") {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingTopic").addClass("disableAll");
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingTopic").val($('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingTopic option:first").val());
            }
            else {
                $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingTopic").removeClass("disableAll");
            }
        }
    },
    // Date: 15-07-2016
    // Author: Humaira Yousaf
    // Overview: Validates Cessation length and period
    isCessationValid: function (sender) {
        var Message = "";
        var cessation;
        var cessationLength;
        var cessationPeriod;
        var ddlVal;

        if (sender == 'tobacco') {
            cessation = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx input[id*=txtTobaccoCessationLength]');
            cessationPeriod = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ddlTobaccoCessationPeriod');
        }

        if (sender == 'alcohol') {
            cessation = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx input[id*=txtAlcoholCessationLength]');
            cessationPeriod = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ddlAlcoholCessationPeriod');
        }

        if (sender == 'drug') {
            cessation = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx input[id*=txtDrugCessationLength]');
            cessationPeriod = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ddlDrugCessationPeriod');
        }

        cessationLength = $(cessation).val();
        ddlVal = $(cessationPeriod).val();

        if (cessationLength != null && cessationLength != '') {
            if (ddlVal == null || ddlVal == '') {
                $(cessationPeriod).focus();
                Message = "Please select Cessation Period.";
            }
        }

        if (ddlVal != null && ddlVal != '') {
            if (cessationLength == null || cessationLength == '') {
                $(cessation).focus();
                Message = "Please enter Cessation Length.";
            }
        }

        return Message;
    },

    gridLoad: function (response) {
        var isactive = $('#' + IA_TabacooScreening.params.PanelID + ' #pnlSocialHx_Result #divSwitch #switchActive').attr('isactive');

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx")) {
            $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx").dataTable().fnClearTable();
            $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx").dataTable().fnDestroy();
            $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx tbody").find("tr").remove();
        }
        var logCount = JSON.parse(response);
        if (logCount.HxLogSoapCount > 0) {
            var counter = null;
            var LoadJSONData = JSON.parse(logCount.HxLogSoap_JSON); //Parsing array to JSON
            for (var i = 0; i < LoadJSONData.length; i++) {
                // $.each(LoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                var text = LoadJSONData[i].SoapText;

                counter = i;

                var outerDiv = $('<div/>').html(LoadJSONData[i].SoapText);
                $(outerDiv).html($(outerDiv).text());
                $(outerDiv).find('strong').wrap("<a href='#'></a>");
                text = $(outerDiv).html()

                $(outerDiv).find('strong').each(function () {
                    if ($(this).text().toLowerCase().indexOf("tobacco") > -1) {

                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listTobacco').trigger('onclick');$('#ulSocialHxTabsItems #listTobacco a').trigger('onclick');IA_TabacooScreening.SelectTab('Tobacco');");
                    }
                    else if ($(this).text().toLowerCase().indexOf("alcohol") > -1) {
                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listAlcohol').trigger('onclick');$('#ulSocialHxTabsItems #listAlcohol a').trigger('onclick');IA_TabacooScreening.SelectTab('Alcohol');");
                    }
                    else if ($(this).text().toLowerCase().indexOf("drug abuse") > -1) {
                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listDrugAbuse').trigger('onclick');$('#ulSocialHxTabsItems #listDrugAbuse a').trigger('onclick');IA_TabacooScreening.SelectTab('DrugAbuse');");
                    }
                    else if ($(this).text().toLowerCase().indexOf("sexual") > -1) {
                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listSexualHx').trigger('onclick');$('#ulSocialHxTabsItems #listSexualHx a').trigger('onclick');IA_TabacooScreening.SelectTab('SexualHx');");
                    }
                    else if ($(this).text().toLowerCase().indexOf("misc") > -1) {
                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listMiscHx').trigger('onclick');$('#ulSocialHxTabsItems #listMiscHx a').trigger('onclick');IA_TabacooScreening.SelectTab('Miscellaneous');");
                    }
                });
                text = $(outerDiv).html();
                $row.append('<td style="display:none;">' + counter + '</td><td>' + LoadJSONData[i].Action + '</td><td id="sptxt">' + text + '</td><td>' + LoadJSONData[i].ModifiedOn + " " + LoadJSONData[i].ModifiedBy + '</td>');
                $row.find('#sptxt').html(text);
                $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx tbody").last().append($row);
                //   });
            }
        }
        else {
            $("#" + IA_TabacooScreening.params.PanelID + ' #pnlSocialHx_Result #dgvPastSocialHx').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Known Social History"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bInfo": false, "bPaginate": false, "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + IA_TabacooScreening.params.PanelID + ' #pnlSocialHx_Result #dgvPastSocialHx'))
            ;
        else {
            $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[0, "asc"]], "aoColumnDefs": [{ "bInfo": false, "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        }

        $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx_filter").remove();
    },
    readyPluggin: function () {

        (function ($) {
            'use strict';
            $(function () {
                $('[data-plugin-ios-switch]').each(function () {
                    var $this = $(this);

                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);
    },

    ChangeCurrentPast: function (obj, PrimaryID, PageNumber, ResultPerPage) {

        if ($(obj).attr('status') == '1' || obj == 1) {
            $(obj).attr('status', 0);
            $('#' + IA_TabacooScreening.params.PanelID + " #pnlCurrent ").addClass("hidden");
            $('#' + IA_TabacooScreening.params.PanelID + " #pnlPast ").removeClass("hidden");
            IA_TabacooScreening.fillhxLog(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                if (response != "") {
                    var json = JSON.parse(response);
                    IA_TabacooScreening.gridLoad(response);

                    var TableControl = IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx";
                    var PagingPanelControlID = IA_TabacooScreening.params.PanelID + " #dgvPastSocialHx_Paging";
                    var ClassControlName = "IA_TabacooScreening";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = json.iTotalDisplayRecords;
                    setTimeout(
                        CreatePagination(json.HxLogSoapCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            IA_TabacooScreening.ChangeCurrentPast(1, PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                }
            });


        } else {
            $(obj).attr('status', 1);

            $('#' + IA_TabacooScreening.params.PanelID + " #pnlPast").addClass("hidden");
            $('#' + IA_TabacooScreening.params.PanelID + " #pnlCurrent  ").removeClass("hidden");
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
        objData["HxId"] = IA_TabacooScreening.params.HxTypeId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = ResultPerPage;
        objData["HxType"] = "SocialHx";
        objData["Status"] = "All";
        objData["commandType"] = "get_hx_log";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");
    },


    BindCurrentSocialHxSoapText: function (resopnse, IsVarifyMUAlert) {

        var $row = $('<tr/>');
        var outerDiv = $('<div/>').html(resopnse.SoapText);
        $(outerDiv).find('strong').wrap("<a href='#'></a>");
        resopnse.SoapText = $(outerDiv).html()

        $(outerDiv).find('strong').each(function () {
            if ($(this).text().toLowerCase().indexOf("tobacco") > -1) {

                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listTobacco').trigger('onclick');$('#ulSocialHxTabsItems #listTobacco a').trigger('onclick');IA_TabacooScreening.SelectTab('Tobacco');");
            }
            else if ($(this).text().toLowerCase().indexOf("alcohol") > -1) {
                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listAlcohol').trigger('onclick');$('#ulSocialHxTabsItems #listAlcohol a').trigger('onclick');IA_TabacooScreening.SelectTab('Alcohol');");
            }
            else if ($(this).text().toLowerCase().indexOf("drug abuse") > -1) {
                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listDrugAbuse').trigger('onclick');$('#ulSocialHxTabsItems #listDrugAbuse a').trigger('onclick');IA_TabacooScreening.SelectTab('DrugAbuse');");
            }
            else if ($(this).text().toLowerCase().indexOf("sexual") > -1) {
                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listSexualHx').trigger('onclick');$('#ulSocialHxTabsItems #listSexualHx a').trigger('onclick');IA_TabacooScreening.SelectTab('SexualHx');");
            }
            else if ($(this).text().toLowerCase().indexOf("misc") > -1) {
                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listMiscHx').trigger('onclick');$('#ulSocialHxTabsItems #listMiscHx a').trigger('onclick');IA_TabacooScreening.SelectTab('Miscellaneous');");
            }
        });

        resopnse.SoapText = $(outerDiv).html();
        if (typeof resopnse.IsCreatedOrModified != typeof undefined && typeof resopnse.LastUpdated != typeof undefined) {
            $row.append('<td style="display:none;">' + resopnse.SocialHxId + '</td><td>' + resopnse.IsCreatedOrModified + '</td><td>' + resopnse.SoapText + '</td><td>' + resopnse.LastUpdated + '</td>');
            $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").html($row);
        }
        else {
            var $row1 = $('<tr/>');
            $row1.append('<td style="display:none;"></td><td>&nbsp;</td><td>No Known Social History</td><td></td>');
            $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").html($row1);
        }


        if ($('#' + IA_TabacooScreening.params.PanelID + ' #pnlSocialHx_Result #divSwitch #switchVisit').attr('status') == '1') {
            $('#' + IA_TabacooScreening.params.PanelID + ' #pnlCurrent').removeClass('hidden');
            $('#' + IA_TabacooScreening.params.PanelID + ' #pnlPast').addClass('hidden');
        }

        //if (IsVarifyMUAlert && $("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody")) {
        //    IA_TabacooScreening.VarifyMUAlert();
        //}

    },

    VarifyMUAlert: function () {

        var m1_obj = {
            ProfileName: "SocialHx",
            Fields: "",
            PatientId: IA_TabacooScreening.params.patientID,
            IsShowAlert: false,
            Type: "MU3"
        };

        var Fileds = "";
        if ($("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").find("[id^='socialMiscHxTravel']").length <= 0) {
            Fileds += "Travel";
        }
        var isFrom = false;
        var isTo = false;
        if ($($("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").find("[id^='socialMiscHxOccupation']")).length > 0) {
            $.each($($("#" + IA_TabacooScreening.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").find("[id^='socialMiscHxOccupation']")), function () {

                var vals = $(this).html().split("from")[1].split("to");
                if (vals.length > 0) {
                    if (vals[0].trim().length > 0) {
                        isFrom = true;
                    }

                    if (vals.length > 1) {
                        var to_ = vals[1].split("<br>");
                        if (to_[0].trim().length > 0) {
                            isTo = true;
                        }
                    }
                }
            });

        }

        if (isFrom == false)
            Fileds += ",From Date";
        if (isTo == false)
            Fileds += ",To Date";

        if (Fileds != "") {
            m1_obj.Fields = Fileds.indexOf(",") == 0 ? Fileds.slice(1, Fileds.length) : Fileds;
            m1_obj.IsShowAlert = true;
        }
        else {
            m1_obj.IsShowAlert = false;
        }

        var array_ = [];
        array_.push(m1_obj);

        Patient_Demographic.UpdateMUAlert(array_).done(function (result) {

            if (result.status != false) {
                var data = JSON.parse(result.MUAlerts_JSON);
                var IsAnyOtherAlert = data.filter(item=>item.PatientId + "" == IA_TabacooScreening.params.patientID);
                if (IsAnyOtherAlert.length > 0 && result.MissingDataAlertCount > 0) {
                    utility.toggelMU3Alerts(true, result.MissingDataAlertCount);
                }
                else {
                    utility.toggelMU3Alerts(false, result.MissingDataAlertCount);
                }

            }
            else {
                console.log(result.Message);
            }
        });
    },

    SelectTab: function (tabName) {
        $('#' + IA_TabacooScreening.params.PanelID + ' .tabs-custom-body .tab-pane').removeClass('active');
        $('#' + IA_TabacooScreening.params.PanelID + ' .tabs-custom-body #' + tabName).addClass('active');
        $('#' + IA_TabacooScreening.params.PanelID + ' #ulSocialHxTabsItems li').removeClass('active');
        if (tabName == 'Tobacco')
            $('#' + IA_TabacooScreening.params.PanelID + ' #ulSocialHxTabsItems #listTobacco').addClass('active');
        else if (tabName == 'Alcohol')
            $('#' + IA_TabacooScreening.params.PanelID + ' #ulSocialHxTabsItems #listAlcohol').addClass('active');
        else if (tabName == 'DrugAbuse')
            $('#' + IA_TabacooScreening.params.PanelID + ' #ulSocialHxTabsItems #listDrugAbuse').addClass('active');
        else if (tabName == 'SexualHx')
            $('#' + IA_TabacooScreening.params.PanelID + ' #ulSocialHxTabsItems #listSexualHx').addClass('active');
        else if (tabName == 'Miscellaneous')
            $('#' + IA_TabacooScreening.params.PanelID + ' #ulSocialHxTabsItems #listMiscHx').addClass('active');
    },

    // Date: 15-07-2016
    // Author: Humaira Yousaf
    // Overview: Enable Disable MenstrualPeriod
    enableDisableMenstrualPeriod: function () {
        var selectedPreference = $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx #ddlSexualPreferences').val();

        if (selectedPreference == "2") {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx input[id*=dtpSexualLMP]').removeClass('disableAll');
        }
        else {
            $('#' + IA_TabacooScreening.params.PanelID + ' #frmClinicalSocialHx input[id*=dtpSexualLMP]').addClass('disableAll');
        }
    },
    cacheSocialHxJSON: function (socialHxType, statusId, stausData, socialTypeMiscText, saveParentOnly) {
        var dfd = $.Deferred();
        if (typeof socialTypeMiscText == typeof undefined || socialTypeMiscText == null) {
            socialTypeMiscText = "Nothing";
        }
        stausData = JSON.parse(stausData);
        var self = null;
        if (socialHxType.toLowerCase() == "tobacco") {
            self = $('#' + IA_TabacooScreening.params.PanelID + " div#Tobacco");
        }
        else if (socialHxType.toLowerCase() == "alcohol") {
            self = $('#' + IA_TabacooScreening.params.PanelID + " div#Alcohol");
        }
        else if (socialHxType.toLowerCase() == "drugabuse") {
            self = $('#' + IA_TabacooScreening.params.PanelID + " div#DrugAbuse");
        }
        else if (socialHxType.toLowerCase() == "sexual") {
            self = $('#' + IA_TabacooScreening.params.PanelID + " div#SexualHx");
        }
        else if (socialTypeMiscText.toLowerCase().indexOf("miscellaneous") > -1) {
            IA_TabacooScreening.retainedComponentMisHx = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #MiscHxMainStatus #ulMiscStatus li.active").attr("id");
            if (socialTypeMiscText.toLowerCase() == "miscellaneous_occupation") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#OccupationDetails");
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_sleep") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#SleepDetails");
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_exercises") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#ExercisesDetails");

            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_housing") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#HousingDetails");
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_caffeineintake") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#CaffeineIntakDetails");
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_travel") {
                self = $('#' + IA_TabacooScreening.params.PanelID + " div#divTravelDetailsHx");
            }
        }



        if (socialHxType.toLowerCase() == "drugabuse") {
            var DrugStatusIds = self.find('#ddlDrugType option:selected').map(function () {
                return this.value;
            }).get().join(',');
            /*
               Change Implement BY: Muhammad Azhar Shahzad
               Reason: To get Drug Status text for Soap Text Cteation
               Created Date: Dec 15, 2015
           */
            var DrugStatusText = self.find('#ddlDrugType option:selected').map(function () {
                return this.text;
            }).get().join(',');
            stausData["DrugType"] = DrugStatusIds;
            stausData["DrugType_text"] = DrugStatusText;
        }
        // Start 07/01/2016 Muhammad Arshad	MiscHx Related code
        if (socialTypeMiscText.toLowerCase().indexOf("miscellaneous") > -1) {
            stausData["MiscChildStatus"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id");
            stausData["MiscChildStatusText"] = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text();
            if (socialTypeMiscText.toLowerCase() == "miscellaneous_exercises") {
                stausData["ExercisesTypeText"] = $('#' + IA_TabacooScreening.params.PanelID + " div#ExercisesDetails").find('#ddlExercisesType option:selected').text();
                stausData["ExercisesDietText"] = $('#' + IA_TabacooScreening.params.PanelID + " div#ExercisesDetails").find('#ddlExercisesDiet option:selected').text();
            }
        }
        // End 07/01/2016 Muhammad Arshad	MiscHx Related code
        if (socialHxType.toLowerCase() == "sexual") {
            var STDIds = "";
            var STDtext = "";
            if (self.find('#ddlSexualExposedToSTD option:selected').val() != "" && self.find('#ddlSexualExposedToSTD option:selected').val() != "No") {


                STDIds = self.find('#ddlSexualSTD option:selected').map(function () {
                    return this.value;
                }).get().join(',');

                STDtext = self.find('#ddlSexualSTD option:selected').map(function () {
                    return this.text;
                }).get().join(',');
            }
            stausData["SexualSTD"] = STDIds;
            stausData["SexualSTD_text"] = STDtext;
            if (self.find('#RadSexualYesAbusedSexually').is(':checked') == true) {
                stausData["RadSexualAbusedSexually"] = true;
            }
            else if (self.find('#RadSexualNoAbusedSexually').is(':checked') == true) {
                stausData["RadSexualAbusedSexually"] = false;
            } else {
                stausData["RadSexualAbusedSexually"] = "-1";
            }

            if (self.find('#RadSexualYesPainWithIntercourse').is(':checked') == true) {
                stausData["RadSexualPainWithIntercourse"] = true;
            }
            else if (self.find('#RadSexualNoPainWithIntercourse').is(':checked') == true) {
                stausData["RadSexualPainWithIntercourse"] = false;
            } else {
                stausData["RadSexualPainWithIntercourse"] = "-1";
            }

            if (self.find('#RadSexualYesPregnant').is(':checked') == true) {
                stausData["RadSexualPregnant"] = true;
            }
            else if (self.find('#RadSexualNoPregnant').is(':checked') == true) {
                stausData["RadSexualPregnant"] = false;
            } else {
                stausData["RadSexualPregnant"] = "-1";
            }
            stausData["SexualHxPregnancyDuration"] = self.find('#txtSexualHxPregnancyDuration').val();
        }

        stausData.SocialHxType = socialHxType;
        stausData.StatusId = statusId;
        stausData.IsLast = true;



        var statusIndex = -1;
        if (Clinical_HistorySummary.HistoryCacheList.SocialHx == null) {
            Clinical_HistorySummary.HistoryCacheList.SocialHx = {};
            var patientId;

            var lstTobaccoModel = [];
            var lstAlcoholModel = [];
            var lstDrugAbuseModel = [];
            var lstSexualHxModel = [];


            var lstOccupationHxModel = [];
            var lstTravelHxModel = [];
            var lstSleepHxModel = [];
            var lstExercisesHxModel = [];
            var lstHousingHxModel = [];
            var lstCaffeineIntakHxModel = [];

            if (IA_TabacooScreening.params.patientID == null) {
                patientId = $('#PatientProfile #hfPatientId').val();
            } else {
                patientId = IA_TabacooScreening.params.patientID;
            }



            if (saveParentOnly != true) {
                if (socialHxType.toLowerCase() == "tobacco") {
                    lstTobaccoModel.push(stausData);
                }
                else if (socialHxType.toLowerCase() == "alcohol") {
                    lstAlcoholModel.push(stausData);
                }
                else if (socialHxType.toLowerCase() == "drugabuse") {
                    lstDrugAbuseModel.push(stausData);
                }
                else if (socialHxType.toLowerCase() == "sexual") {
                    lstSexualHxModel.push(stausData);
                }
                else if (socialTypeMiscText.toLowerCase() == "miscellaneous_occupation") {
                    lstOccupationHxModel.push(stausData);
                }
                else if (socialTypeMiscText.toLowerCase() == "miscellaneous_sleep") {
                    lstSleepHxModel.push(stausData);
                }
                else if (socialTypeMiscText.toLowerCase() == "miscellaneous_exercises") {
                    lstExercisesHxModel.push(stausData);
                }
                else if (socialTypeMiscText.toLowerCase() == "miscellaneous_housing") {
                    lstHousingHxModel.push(stausData);
                }
                else if (socialTypeMiscText.toLowerCase() == "miscellaneous_caffeineintake") {
                    lstCaffeineIntakHxModel.push(stausData);
                }
                else if (socialTypeMiscText.toLowerCase() == "miscellaneous_travel") {
                    lstTravelHxModel.push(stausData);
                }
            }

            var SocialHxData = {
                SocialHxId: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(),
                PatientId: patientId,
                SocialHxDate: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val(),
                SocialHxUnremarkable: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked"),
                SocialComments: $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val(),
                SocialHxType: socialHxType != null ? socialHxType : "",
                NotesId: Clinical_ProgressNote.params.NotesId,
                lstTobaccoModel: lstTobaccoModel,
                lstAlcoholModel: lstAlcoholModel,
                lstDrugAbuseModel: lstDrugAbuseModel,
                lstSexualHxModel: lstSexualHxModel,
                lstOccupationHxModel: lstOccupationHxModel,
                lstTravelHxModel: lstTravelHxModel,
                lstSleepHxModel: lstSleepHxModel,
                lstExercisesHxModel: lstExercisesHxModel,
                lstHousingHxModel: lstHousingHxModel,
                lstCaffeineIntakHxModel: lstCaffeineIntakHxModel,
            }
            Clinical_HistorySummary.HistoryCacheList.SocialHx = SocialHxData;
        }
        else {


            var patientId;

            if (IA_TabacooScreening.params.patientID == null) {
                patientId = $('#PatientProfile #hfPatientId').val();
            } else {
                patientId = IA_TabacooScreening.params.patientID;
            }

            if (socialHxType.toLowerCase() == "tobacco") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel.push(stausData);

            }
            else if (socialHxType.toLowerCase() == "alcohol") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel.push(stausData);

            }
            else if (socialHxType.toLowerCase() == "drugabuse") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel.push(stausData);
            }
            else if (socialHxType.toLowerCase() == "sexual") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel.push(stausData);

            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_occupation") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel.push(stausData);

            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_travel") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel.push(stausData);
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_sleep") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel.push(stausData);

            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_exercises") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel.push(stausData);

            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_housing") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel.push(stausData);
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_caffeineintake") {
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel = [];
                Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel.push(stausData);
            }
            Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxId = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val();
            Clinical_HistorySummary.HistoryCacheList.SocialHx.PatientId = patientId;
            Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxDate = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
            Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
            Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialComments = $('#' + IA_TabacooScreening.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();
            Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxType = socialHxType != null ? socialHxType : "";
            Clinical_HistorySummary.HistoryCacheList.SocialHx.NotesId = Clinical_ProgressNote.params.NotesId;
        }
        dfd.resolve();
        return dfd;
    },
    getCacheSocialHxJSON: function (socialHxType, statusId, socialTypeMiscText) {
        if (typeof socialTypeMiscText == typeof undefined || socialTypeMiscText == null) {
            socialTypeMiscText = "Nothing";
        }
        if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
            if (socialHxType.toLowerCase() == "tobacco") {
                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }

                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }
            else if (socialHxType.toLowerCase() == "alcohol") {

                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }
                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }
            else if (socialHxType.toLowerCase() == "drugabuse") {
                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }
                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }
            else if (socialHxType.toLowerCase() == "sexual") {

                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }
                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_occupation") {

                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }
                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_sleep") {

                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }
                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_exercises") {

                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }
                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_housing") {

                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }
                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_caffeineintake") {

                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }
                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_travel") {

                var socialHistory = $.grep(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel, function (item, index) {
                    if (statusId != -1) {
                        if (item.SocialHxType == socialHxType && item.StatusId == statusId) {
                            return item;
                        }
                    }
                    else {
                        if (item.SocialHxType == socialHxType && item.IsLast) {
                            return item;
                        }
                    }
                });

                if (socialHistory.length > 0) {
                    return socialHistory[0];
                }
            }

        }

        return '';
    },
    SetIsLast: function (socialHxType, statusId, socialTypeMiscText) {
        if (typeof socialTypeMiscText == typeof undefined || socialTypeMiscText == null) {
            socialTypeMiscText = "Nothing";
        }
        if (socialHxType.toLowerCase() == "tobacco") {
            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }
        else if (socialHxType.toLowerCase() == "alcohol") {

            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }
        else if (socialHxType.toLowerCase() == "drugabuse") {

            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }
        else if (socialHxType.toLowerCase() == "sexual") {

            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }
        else if (socialTypeMiscText.toLowerCase() == "miscellaneous_occupation") {

            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }
        else if (socialTypeMiscText.toLowerCase() == "miscellaneous_sleep") {

            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }
        else if (socialTypeMiscText.toLowerCase() == "miscellaneous_travel") {
            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }
        else if (socialTypeMiscText.toLowerCase() == "miscellaneous_exercises") {

            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }
        else if (socialTypeMiscText.toLowerCase() == "miscellaneous_housing") {


            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }
        else if (socialTypeMiscText.toLowerCase() == "miscellaneous_caffeineintake") {

            $(Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel).each(function (index, item) {
                if (item.SocialHxType == socialHxType && item.StatusId != statusId) {
                    item.IsLast = false;
                }
            });
        }



    },
}