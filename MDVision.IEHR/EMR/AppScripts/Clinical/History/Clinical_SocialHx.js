Clinical_SocialHx = {
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
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #CaffeineIntakDetails input:checkbox').on('click', function () {
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
        Clinical_SocialHx.params = params;

        //SelectedTab["PatientId"] = $("div#PatientProfile #hfPatientId").val();

        var bAlcoholExist = false;
        var bDrugExist = false;
        var bSexualExist = false;
        var bTobaccoExist = false;
        var bMiscHxExist = false;
        var bIsTriggerManually = false;

        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (Clinical_SocialHx.params.mode == null) {
            Clinical_SocialHx.params.mode = "Add";
        }
        if (Clinical_SocialHx.params.PanelID != 'pnlClinicalSocialHx') {
            Clinical_SocialHx.params.PanelID = Clinical_SocialHx.params.PanelID + ' #pnlClinicalSocialHx';
        } else {
            Clinical_SocialHx.params.PanelID = 'pnlClinicalSocialHx';
        }
        Clinical_SocialHx.ResetFormData();
        var SocialHxId = "";
        if (Clinical_SocialHx.params.mode == "Add" || Clinical_SocialHx.params.SocialHxId == null || Clinical_SocialHx.params.SocialHxId == "" || Clinical_SocialHx.params.SocialHxId == "-1") {
            SocialHxId = "-1";
        }
        else if (Clinical_SocialHx.params.mode == "Edit") {
            SocialHxId = Clinical_SocialHx.params.SocialHxId;
            //Clinical_SocialHx.SocialHxEdit(SocialHxId);
        }
        //if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
        //    Clinical_SocialHx.bIsFirstLoad = true;
        //    $('#divViewHistorySummary').addClass('hidden');
        //    $(' #pnlClinicalSocialHx').removeClass('row');
        //}
        /* Start 09/12/2015 Muhammadn Irfan to disabale  dtpSexualLMP if the sex of patient is Male */
        if ($('#PatientProfile #hfPatientSex').val() != "" && $('#PatientProfile #hfPatientSex').val() == "Male") {
            $('#' + Clinical_SocialHx.params.PanelID + ' #dtpSexualLMP').addClass('disableAll');
        }
        /* End 09/12/2015 Muhammadn Irfan to disabale  dtpSexualLMP if the sex of patient is Male */


        if (Clinical_SocialHx.bIsFirstLoad == true) {
            Clinical_SocialHx.bIsFirstLoad = false;
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus").sortable({
                out: function (event, ui) {
                    //utility.myConfirm('21', function () {
                    var sortedIdsInOrder = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus").sortable("toArray");

                    var miscCompnentSorted = []
                    $.each(sortedIdsInOrder, function (index, element) {
                        var ComponentName = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus li#" + element).text().trim();
                        miscCompnentSorted.push(ComponentName);
                    });

                    Clinical_SocialHx.updateComponentOrderSorting(miscCompnentSorted.join(','));
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
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus").disableSelection();

            //Load Dropdown
            //if (Clinical_SocialHx.bIsFirstLoad) { // start of if condition
            //    Clinical_SocialHx.bIsFirstLoad = false;

            var DetailDivName = $("#" + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active a").attr("href");
            var self = $('#' + Clinical_SocialHx.params.PanelID);

            //if (DetailDivName != null && DetailDivName != "") {
            //    self = $('#' + Clinical_SocialHx.params.PanelID + " " + DetailDivName);
            //}

            self.loadDropDowns(true).done(function () {



                //Clinical_SocialHx.FillAllDropDowns(SocialHxignId);

                $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType option").removeAttr("selected");
                $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD option").removeAttr("selected");

                /* Start 09/12/2015 Muhammad Irfan Load Sexual Complaints dropdown on Sex base */
                if ($('#PatientProfile #hfPatientSex').val() != "" && $('#PatientProfile #hfPatientSex').val() == "Male") {
                    CacheManager.BindDropDownsByEntityID('#' + Clinical_SocialHx.params.PanelID + ' #ddlSexualComplaints', 'GetSexualHxComplaints', true, 'Male');
                } else if ($('#PatientProfile #hfPatientSex').val() != "" && $('#PatientProfile #hfPatientSex').val() == "Female") {
                    CacheManager.BindDropDownsByEntityID('#' + Clinical_SocialHx.params.PanelID + ' #ddlSexualComplaints', 'GetSexualHxComplaints', true, 'Female');
                }
                /* End 09/12/2015 Muhammad Irfan Load Sexual Complaints dropdown on Sex base */

                /* Start 09/12/2015 Muhammad Irfan This will trigger tobacco which will load tobacco panel */
                // $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').trigger('click');
                /* End 09/12/2015 Muhammad Irfan This will trigger tobacco which will load tobacco panel */

                //Start//16/12/2015//Ahmad Raza//Loading Tabacco Tab and unserializ form
                Clinical_SocialHx.loadTabaccoTabnUnserializeForm();
                Clinical_SocialHx.isFromTabTrigger = true;

                //End//16/12/2015//Ahmad Raza//Loading Tabacco Tab and unserializ form
                //  Clinical_SocialHx.triggerSocialHistoryTab();
                //Clinical_SocialHx.loadSocialHx("tobacco");

                $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());

            });
            Clinical_SocialHx.validateSocialHx();
            Clinical_SocialHx.readyFunction();
            //end Load Dropdown
            utility.CreateDatePicker(Clinical_SocialHx.params.PanelID + '  #dtSocialHxDate', function () {
            }, true);

            utility.CreateDatePicker(Clinical_SocialHx.params.PanelID + '  section#sectionSexualHx #dtpSexualLMP', function () {
            }, false);

            if ($('#' + Clinical_SocialHx.params.PanelID + ' #PatientProfile #hfPatientId').val() != "") {
                $('#' + Clinical_SocialHx.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            }
            //22/12/2015//AhmadRaza//Form Serialization
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());

            /*
               Change Implement BY: Muhammad Azhar Shahzad
               Reason:To Show navigation on Progress Note
               Created Date: Dec 15, 2015
           */
            //Code for progress note navigation
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {

                //Start//01/18/2016//Abid Ali//Reset Main SocialHx html form data
                Clinical_SocialHx.removeMainSocialHxTabPageHtml();
                //End//01/18/2016//Abid Ali//Remove Main SocialHx form data

                //Start 20-01-2016 Muhammad Arshad Change HTML when socialHx page is opened from note flow
                $('#' + Clinical_SocialHx.params.PanelID + ' div#pnlSection_Search').removeClass("panel-body NoRadiusT");//
                $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSocialHx').removeClass("panel panel-featured");
                $('#' + Clinical_SocialHx.params.PanelID + ' div.modal-footer').removeClass("hidden");
                //END 20-01-2016 Muhammad Arshad Change HTML when socialHx page is opened from note flow


                EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_SocialHx.params.PanelID, 'History', 'SocialHx', 'Clinical_SocialHx.unLoadTab(true);', null, true);
                $('#' + Clinical_SocialHx.params.PanelID + ' #btnAddVitalsOnNote').show();
                // $('#' + Clinical_SocialHx.params.PanelID + '  #dtSocialHxDate').prop('disabled', true);

            } else {

                $('#' + Clinical_SocialHx.params.PanelID + ' #btnAddVitalsOnNote').hide();
                $('#' + Clinical_SocialHx.params.PanelID + '  #dtSocialHxDate').prop('disabled', false);
            }
            //end change azhar Dec 15, 2015
            Clinical_SocialHx.domReadyFunction();


        } else {
            Clinical_SocialHx.bIsFirstLoad = false;
        }
        Clinical_SocialHx.readyPluggin();

        if (LastSocialHx != null && LastSocialHx.PatientId == $('#PatientProfile #hfPatientId').val()) {
            setTimeout(function () {
                $('#' + Clinical_SocialHx.params.PanelID + ' #' + LastSocialHx.SocialHxType + " a").trigger('click');
            }, 100)
        }

        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnTobaccoSave').addClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnAlcoholSave').addClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnDrugSave').addClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnSexualSave').addClass('hidden');
            //$('#' + Clinical_SocialHx.params.PanelID + ' #btnOccupationSave').addClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnSleepSave').addClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnExercisesSave').addClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnHousingSave').addClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnCaffeineSave').addClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');
            Clinical_SocialHx.socialHxJSON = '';
        }
        else {
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnTobaccoSave').removeClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnAlcoholSave').removeClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnDrugSave').removeClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnSexualSave').removeClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnOccupationSave').removeClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnSleepSave').removeClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnExercisesSave').removeClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnHousingSave').removeClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #btnCaffeineSave').removeClass('hidden');
        }

        utility.CreateDatePicker('pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxFromDate',
        function (ev) {
        }, false);
        utility.CreateDatePicker('pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxToDate',
        function (ev) {
        }, false);

        utility.ValidateFromToDate('pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx', 'dtTravelHxFromDate', 'dtTravelHxToDate', true);

        utility.CreateDatePicker('pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #dtOccupationHxStartDate',
                function (ev) {
                }, false);
        utility.CreateDatePicker('pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate',
        function (ev) {
        }, false);
        utility.ValidateFromToDate('pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails', 'dtOccupationHxStartDate', 'dtOccupationHxEndDate', true);

    },
    ResetFormData: function () {
       
        var details = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx");
        $(details).resetAllControls(null);

        var detailsSection = $('#' + Clinical_SocialHx.params.PanelID + " #TabsSection");
        $(detailsSection).resetAllControls(null);
        Clinical_SocialHx.bIsFirstLoad = true;
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #TobaccoDataChangeBit").val(0);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfAlcoholId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfAlcoholDataChangeBit").val(0);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfDrugAbuseId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfDrugAbuseId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfDrugAbuseDataChangeBit").val(0);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSexualHxId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfMiscHxId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfOccupationHxId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfTravelDetailHxId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSleepHxId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfExercisesHxId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfHousingHxId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfCaffeineIntakeHxId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(-1);
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val('tobacco');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SocialHxSoapText").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx ul:not(#ulSocialHxTabsItems) li").removeClass('active');
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ulSocialHxTabsItems').removeClass('disableAll');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx ul#ulSocialHxTabsItems li").removeClass('successLight');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #TabsSection #Tobacco").removeClass('disableAll');
    },
    validateSocialHx: function () {
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx')
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
                Clinical_SocialHx.socialHxSave();
            }

            e.type = "";


        });
        Clinical_SocialHx.enableAlcoholDurationValidation();
        Clinical_SocialHx.enableDurationValidation();

    },

    //Begin 14-01-2016 syed zia , for number pad
    domReadyFunction: function () {

        /* Start 20/01/2016/ Abid Ali/Sets MiscellaneousTab bit to false on close patient event */
        $('#btnClosePatient').on('click', function () {
            Clinical_SocialHx.isMiscellaneousTabTrigger = false;
            // Clinical_SocialHx.isDataExist = false;
        });
        /* End 20/01/2016/ Abid Ali/Sets MiscellaneousTab bit to false on close patient event */

        $(function () {
            $('#' + Clinical_SocialHx.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            //EMR-70 Bug number Resolution
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx [data-plugin-keyboard-numpad]').keyboard({
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
        var activeChilStatusText = Clinical_SocialHx.getActiveChildMenuStatus();
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
        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());

    },

    EnableDurationPregnantSexual: function () {
        if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx #RadSexualNoPregnant").is(":checked")) {
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx #txtSexualHxPregnancyDuration").removeAttr('disabled');
        }
        else if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx #RadSexualYesPregnant").is(":checked"))
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx #txtSexualHxPregnancyDuration").removeAttr('disabled');
    },

    enableAlcoholDurationValidation: function () {

        var stayLength = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #txtAlcoholCessationLength').val();
        var ddlVal = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ddlAlcoholCessationPeriod').val();
        if (stayLength != null && stayLength != '') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('AlcoholCessationPeriod', true);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblAlcoholDuration').html('Duration<span class="required">*</span>');
        }
        else {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('AlcoholCessationPeriod', false);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblAlcoholDuration').html('Duration');
        }
        if (ddlVal != null && ddlVal != '') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('AlcoholCessationLength', true);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblAlcoholDuration').html('Duration<span class="required">*</span>');

        } else {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('AlcoholCessationLength', false);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblAlcoholDuration').html('Duration');
        }

    },


    //Author: Abid Ali
    //Date: 26-01-2016
    //Returns text of active child menu status
    getActiveChildMenuStatus: function () {
        var activeChilStatusText = "";
        $('#' + Clinical_SocialHx.params.PanelID + " #ulMiscChildStatus > li").each(function () {
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
            Crtl = '#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus";
            currentLiClick = "Clinical_SocialHx.enableDisableTobaccoControls";
            ParentDiv = "Tobacco";
            methodName = "GetTobaccoSmokingStatus";
        }
        else if (StatusType != null && StatusType.toLowerCase() == "alcohol") {
            Crtl = '#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Alcohol #ulAlcoholStatus";
            currentLiClick = "Clinical_SocialHx.enableDisableAlcoholControls";
            ParentDiv = "Alcohol";
            methodName = "GetAlcoholStatus";
        }
        else if (StatusType != null && StatusType.toLowerCase() == "drug") {
            Crtl = '#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#DrugAbuse #ulDrugStatus";
            currentLiClick = "Clinical_SocialHx.enableDisableDrugAbuseControls";
            ParentDiv = "DrugAbuse";
            methodName = "GetDrugAbuseStatus";
        }
        else if (StatusType != null && StatusType.toLowerCase() == "sexual") {
            Crtl = '#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#SexualHx #ulSexualStatus";
            currentLiClick = "Clinical_SocialHx.enableDisableSexualHxControls";
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
                    Clinical_SocialHx.DeleteSocialHxMiscHxTravelHx_DBCall(id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_SocialHx.SocialHxMiscHxTravelHxSearch();
                            utility.DisplayMessages(response.Message, 1);
                            Clinical_SocialHx.fillSocialHx('miscellaneous_travel').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            Clinical_SocialHx.BindCurrentSocialHxSoapText(response,true);
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
                    Clinical_SocialHx.DeleteSocialHxMiscHxOccupationHx_DBCall(id).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_SocialHx.fillSocialHx('miscellaneous_occupation').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            Clinical_SocialHx.BindCurrentSocialHxSoapText(response,true);
                                        }
                                    }
                                }
                            });
                            Clinical_SocialHx.SocialHxMiscHxOccupationHxSearch();
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
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #hfOccupationHxId").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxStartDate").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #txtOccupationHxDetails").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #txtOccupationComments").val('');
    },
    SaveSocialHxOccupationDetail: function () {
        if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").length == 0) {
            utility.DisplayMessages("Please select status first ", 2);
            return false;
        }
        if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").length > 1) {
            utility.DisplayMessages("Please select only one status ", 2);
            return false;
        }
        var mode = "Add";
        var SocialHxId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() != "" ? $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() : "-1";
        self = $('#' + Clinical_SocialHx.params.PanelID + " div#OccupationDetails");
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (parseInt(objData["OccupationHxId"]) > 0)
            mode = "Edit";

        if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").is(":checked"))
            objData["RadOccupation"] = true;
        else if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPresentExperience").is(":checked"))
            objData["RadOccupation"] = false;

        objData["SocialHxId"] = SocialHxId;
        objData["MiscChildStatus"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id");
        objData["MiscChildStatusText"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text();

        objData["SocialHxDate"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
        objData["SocialHxUnremarkable"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
        objData["SocialComments"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();

        if (mode == "Add") {
            AppPrivileges.GetFormPrivileges("History_Social Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    objData["commandType"] = "SAVE_sochxmischxOccupationhx";
                    Clinical_SocialHx.SaveSocialHxMiscHxOccupationHx_DbCall(objData).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_SocialHx.AddNewSocialHxOccupationDetail();
                            Clinical_SocialHx.SocialHxMiscHxOccupationHxSearch();
                            Clinical_SocialHx.fillSocialHx('miscellaneous_occupation').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            Clinical_SocialHx.BindCurrentSocialHxSoapText(response,true);
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
                    Clinical_SocialHx.SaveSocialHxMiscHxOccupationHx_DbCall(objData).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_SocialHx.AddNewSocialHxOccupationDetail();
                            Clinical_SocialHx.SocialHxMiscHxOccupationHxSearch();
                            Clinical_SocialHx.fillSocialHx('miscellaneous_occupation').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            Clinical_SocialHx.BindCurrentSocialHxSoapText(response,true);
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
                Clinical_SocialHx.SearchSocialHxMiscHxTravelHx_DBCall(null, null, id).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx");
                        var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                        if (response.MiscHxTravelHxCount > 0) {
                            response.SocialHxMiscHxTravelHx_JSON = JSON.parse(response.SocialHxMiscHxTravelHx_JSON);
                            var detail = response.SocialHxMiscHxTravelHx_JSON[0];
                            //$('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + detail.StatusId).addClass("active");
                            Clinical_SocialHx.markStatusActive('ulMiscChildStatus', detail.StatusId);
                            $.when(Clinical_SocialHx.enableDisableTravelControls($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text().trim(), detail.StatusId, true)).then(function () {
                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #hfTravelDetailHxId").val(detail.TravelHxId);
                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxFromDate").val(utility.RemoveTimeFromDate(null, detail.FromDate));
                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxToDate").val(utility.RemoveTimeFromDate(null, detail.Todate));
                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #txtTravelHxLocation").val(detail.Location);
                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #txtTravelHxComments").val(detail.Comments);
                                if (detail.StatusId == $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id") && $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text().trim() == "Does Not Travel")
                                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").removeClass('disableAll');
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
                Clinical_SocialHx.SearchSocialHxMiscHxOccupationHx_DBCall(null, null, id).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails");
                        if (response.MiscHxOccupationHxCount > 0) {
                            response.SocialHxMiscHxOccupationHx_JSON = JSON.parse(response.SocialHxMiscHxOccupationHx_JSON);
                            var detail = response.SocialHxMiscHxOccupationHx_JSON[0];
                            detail.IsPast = detail.IsPast.toLowerCase() == 'true';
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #hfOccupationHxId").val(detail.OccupationHxId);
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxStartDate").val((utility.RemoveTimeFromDate(null, detail.StartDate)));
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").val((utility.RemoveTimeFromDate(null, detail.EndDate)));
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #txtOccupationHxDetails").val(detail.OccupationDetail);
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #txtOccupationComments").val(detail.Comments);
                            (detail.IsPast ? $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").prop("checked", true) : $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPresentExperience").prop("checked", true));
                            Clinical_SocialHx.EnableDisableToDateOccupation(true);
                            //$('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + detail.StatusId).addClass("active");
                            Clinical_SocialHx.markStatusActive('ulMiscChildStatus', detail.StatusId);
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
        var SocialHxId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() != "" ? $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() : "-1";
        self = $('#' + Clinical_SocialHx.params.PanelID + " div#divTravelDetailsHx");
        var myJSON = self != null ? self.getMyJSONByName() : "{}";
        var objData = JSON.parse(myJSON);
        if (parseInt(objData["TravelHxId"]) > 0)
            mode = "Edit";
        objData["SocialHxId"] = SocialHxId;
        objData["MiscChildStatus"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id");
        objData["MiscChildStatusText"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text();

        objData["SocialHxDate"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
        objData["SocialHxUnremarkable"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
        objData["SocialComments"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();

        if (mode == "Add") {
            AppPrivileges.GetFormPrivileges("History_Social Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    objData["commandType"] = "SAVE_sochxmischxtravelhx";
                    Clinical_SocialHx.SaveSocialHxTraveHx_DbCall(objData).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(response.sociahxid);
                            Clinical_SocialHx.AddNewSocialHxTravelDetail();
                            Clinical_SocialHx.SocialHxMiscHxTravelHxSearch();
                            Clinical_SocialHx.fillSocialHx('miscellaneous_travel').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            Clinical_SocialHx.BindCurrentSocialHxSoapText(response,true);
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
                    Clinical_SocialHx.SaveSocialHxTraveHx_DbCall(objData).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(response.sociahxid);
                            Clinical_SocialHx.AddNewSocialHxTravelDetail();
                            Clinical_SocialHx.SocialHxMiscHxTravelHxSearch();
                            Clinical_SocialHx.fillSocialHx('miscellaneous_travel').done(function (response) {
                                if (response != "") {
                                    response = JSON.parse(response);
                                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                                        if (response.status != false) {
                                            Clinical_SocialHx.BindCurrentSocialHxSoapText(response,true);
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
        Clinical_SocialHx.SearchSocialHxMiscHxOccupationHx_DBCall(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_SocialHx.SocialHxMiscHxOccupationHxGridLoad(response);
                var TableControl = "pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx";
                var PagingPanelControlID = "pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #divSocialHxMiscHxOccupationHxPaging";
                var ClassControlName = "Clinical_SocialHx";
                var PagesToDisplay = 3;
                var iTotalDisplayRecords = response.iTotalSocialHxMiscHxOccupationHxDisplayRecords;
                setTimeout(CreatePagination(response.iTotalSocialHxMiscHxOccupationHxDisplayRecords, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PageNumber, ResultPerPage) {
                    Clinical_SocialHx.SocialHxMiscHxOccupationHxSearch(PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    SocialHxMiscHxOccupationHxGridLoad: function (response) {
        var isTrueSet;
        $("#pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx").dataTable().fnDestroy();
        $("#pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx tbody").find("tr").remove();
        if (response.MiscHxOccupationHxCount > 0) {
            var SocialHxMiscHxOccupationHxJSONData = JSON.parse(response.SocialHxMiscHxOccupationHx_JSON);
            $.each(SocialHxMiscHxOccupationHxJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvSHxMiscHxTravelHx_row" + i);
                isTrueSet = (item.IsPast.toLowerCase() == 'true');
                $row.append('<td style="display:none;">' + item.OccupationHxId + '</td><td><a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_SocialHx.OccupationHxEdit(' + item.OccupationHxId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_SocialHx.OccupationHxDelete(' + item.OccupationHxId + ', event);"   title="Delete Record"><i class="fa fa-close red"></i></a></td><td>' + (isTrueSet ? "Past" : "Present") + '</td><td>' + (item.OccupationDetail ? item.OccupationDetail : "N/A") + '</td><td>' + ((item.StartDate) ? (utility.RemoveTimeFromDate(null, item.StartDate)) : "N/A") + '</td><td>' + ((item.EndDate) ? (utility.RemoveTimeFromDate(null, item.EndDate)) : "N/A") + '</td>');
                Clinical_SocialHx.markStatusActive('ulMiscChildStatus', item.StatusId, true);
                $("#pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx tbody").last().append($row);
            });
        }
        else {
            $("#pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #divSocialHxMiscHxOccupationHxPaging").css("display", "none");
            $('#pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx').DataTable({
                "language": {
                    "emptyTable": "No Occupation History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx'))
            ;
        else
            $("#pnlClinicalSocialHx #frmClinicalSocialHx #OccupationDetails #dgvSocialHxMiscHxOccupationHx").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
    },
    SocialHxMiscHxTravelHxSearch: function (PageNo, rpp) {
        Clinical_SocialHx.SearchSocialHxMiscHxTravelHx_DBCall(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_SocialHx.SocialHxMiscHxTravelHxGridLoad(response);
                var TableControl = "pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx";
                var PagingPanelControlID = "pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #divSocialHxMiscHxTravelHxPaging";
                var ClassControlName = "Clinical_SocialHx";
                var PagesToDisplay = 3;
                var iTotalDisplayRecords = response.iTotalSocialHxMiscHxTravelHxDisplayRecords;
                setTimeout(CreatePagination(response.iTotalSocialHxMiscHxTravelHxDisplayRecords, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PageNumber, ResultPerPage) {
                    Clinical_SocialHx.SocialHxMiscHxTravelHxSearch(PageNumber, ResultPerPage);
                }), 10);

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    EnableDisableToDateOccupation: function (edit) {
        if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPresentExperience").is(":checked")) {
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").val('');
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #divOccupationEndDate").css('display', 'none');
        }
        else if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").is(":checked")) {
            if (!edit)
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").val('');
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #dtOccupationHxEndDate").removeAttr('disabled');
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #divOccupationEndDate").css('display', '');
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
        objData["SocialHxId"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val();
        objData["commandType"] = "delete_sochxmischxtravelhx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHxMiscHxTravelHx");
    },
    DeleteSocialHxMiscHxOccupationHx_DBCall: function (OccupationId) {
        var objData = {};
        objData["OccupationHxId"] = OccupationId;
        objData["SocialHxId"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val();
        objData["commandType"] = "delete_sochxmischxoccupationhx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "SocialHxMiscHxOccupationHx");
    },
    SocialHxMiscHxTravelHxGridLoad: function (response) {
        $("#pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx").dataTable().fnDestroy();
        $("#pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx tbody").find("tr").remove();
        if (response.MiscHxTravelHxCount > 0) {
            var SocialHxMiscHxTravelHxJSONData = JSON.parse(response.SocialHxMiscHxTravelHx_JSON);
            $.each(SocialHxMiscHxTravelHxJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "gvSHxMiscHxTravelHx_row" + i);
                $row.attr("statusid", item.StatusId);
                $row.append('<td style="display:none;">' + item.TravelHxId + '</td><td><a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_SocialHx.TravelHxEdit(' + item.TravelHxId + ',event);"   title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn btn-xs" href="javascript:void(0);" onclick="Clinical_SocialHx.TravelHxDelete(' + item.TravelHxId + ', event);"   title="Delete Record"><i class="fa fa-close red"></i></a></td><td>' + (item.Location ? item.Location : "N/A") + '</td><td>' + (item.Comments ? item.Comments : "N/A") + '</td><td>' + ((item.FromDate) ? (utility.RemoveTimeFromDate(null, item.FromDate)) : "N/A") + '</td><td>' + ((item.Todate) ? (utility.RemoveTimeFromDate(null, item.Todate)) : "N/A") + '</td>');
                Clinical_SocialHx.markStatusActive('ulMiscChildStatus', item.StatusId, true);
                if (item.statusname == "Does Not Travel" && $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text().trim() == "Does Not Travel")
                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").addClass('disableAll');
                $("#pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx tbody").last().append($row);
            });
        }
        else {
            $("#pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #divSocialHxMiscHxTravelHxPaging").css("display", "none");
            $('#pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx').DataTable({
                "language": {
                    "emptyTable": "No Travel History Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx'))
            ;
        else
            $("#pnlClinicalSocialHx #frmClinicalSocialHx #divTravelDetailsHx #dgvSocialHxMiscHxTravelHx").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
    },
    AddNewSocialHxTravelDetail: function () {
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #hfTravelDetailHxId").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxFromDate").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #dtTravelHxToDate").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #txtTravelHxLocation").val('');
        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx #txtTravelHxComments").val('');
    },
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle fill of SocialHx and it's childs as specified by SocialHxType


    loadSocialHxComponent: function (SocialHxType) {

        //Start 08-01-2016 Muhammad Arshad LoadMiscHx Tab
        if (SocialHxType != null && SocialHxType.toLowerCase().indexOf("miscellaneous") > -1) {

            Clinical_SocialHx.fillSocialHx(SocialHxType).done(function (response) {
                if (response != "") {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        Clinical_SocialHx.BindCurrentSocialHxSoapText(response,false);
                        if (response.socialHxLoad_JSON) {
                            var tabsData = JSON.parse(response.socialHxLoad_JSON);
                            Clinical_SocialHx.checkTabColor(tabsData);
                        }
                        //Begin 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                            /* Start 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                            //$('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").addClass("disableAll");
                            /* End 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                        }
                        //End 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                        if (typeof (response.SocialHxFill_JSON) != "undefined") {
                            var socialhx_detail = JSON.parse(response.SocialHxFill_JSON);
                            // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                            if (Clinical_SocialHx.isRemarkableFormload == false) {
                                socialhx_detail.SocialHxUnremarkable = "false";
                            }
                            // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                            var self = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx");
                            utility.bindMyJSONByName(true, socialhx_detail, false, self).done(function () {

                                Clinical_SocialHx.params.mode = "Edit";
                            });

                            Clinical_SocialHx.showParentData(socialhx_detail);
                            if (socialhx_detail.SocialHxUnremarkable.toLowerCase() != "true") {
                                if (SocialHxType.toLowerCase() == "miscellaneous_components") {

                                    var arrComponents = JSON.parse(response.socialHxMiscHxComponentLoad_JSON);
                                    var addedMiscComponents = response.IsCreatedOrModified;
                                    //if (Clinical_SocialHx.retainedComponentMisHx == "") {
                                    //    if (JSON.parse(response.SleepHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (arrComponents[obj].ComponentName.toLowerCase() == "sleep")
                                    //                    Clinical_SocialHx.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //    else if (JSON.parse(response.CaffeineIntakeHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (arrComponents[obj].ComponentName.toLowerCase() == "caffeine intake")
                                    //                    Clinical_SocialHx.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //    else if (JSON.parse(response.HousingHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (arrComponents[obj].ComponentName.toLowerCase() == "housing")
                                    //                    Clinical_SocialHx.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //    else if (JSON.parse(response.ExercisesHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (obj.ComponentName.toLowerCase() == "exercises")
                                    //                    Clinical_SocialHx.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //    else if (JSON.parse(response.OccupationHxFill_JSON).length > 0) {
                                    //        if (arrComponents != null)
                                    //            for (var obj in arrComponents) {
                                    //                if (obj.ComponentName.toLowerCase() == "occupation")
                                    //                    Clinical_SocialHx.retainedComponentMisHx = arrComponents[obj].ComponentId;
                                    //            }
                                    //    }
                                    //}
                                    Clinical_SocialHx.loadMiscHxComponents('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus", arrComponents, Clinical_SocialHx.retainedComponentMisHx, "MiscHxMainStatus", "", addedMiscComponents);
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
                                                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #occupationHistory").removeClass('hidden');
                                                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #occupationHistory").attr('onclick', 'Clinical_SocialHx.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');
                                                }
                                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails").getMyJSONByName();
                                            }
                                            Clinical_SocialHx.SocialHxMiscHxOccupationHxSearch();

                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_sleep") {
                                            DetailsSection = "div#SleepDetails";
                                            currentJSON = response.SleepHxFill_JSON;

                                            if (currentJSON != null) {
                                                var miscHx = JSON.parse(currentJSON);
                                                var miscHxId = miscHx[0].MiscHxId;
                                                var tableName = "SocialHx_MiscHx_SleepHx";
                                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #sleepHistory").removeClass('hidden');
                                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #sleepHistory").attr('onclick', 'Clinical_SocialHx.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');

                                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SleepDetails").getMyJSONByName();
                                            }
                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_exercises") {
                                            DetailsSection = "div#ExercisesDetails";
                                            currentJSON = response.ExercisesHxFill_JSON;

                                            if (currentJSON != null) {
                                                var miscHx = JSON.parse(currentJSON);
                                                var miscHxId = miscHx[0].MiscHxId;
                                                var tableName = "SocialHx_MiscHx_ExercisesHx";
                                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #exercisesHistory").removeClass('hidden');
                                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #exercisesHistory").attr('onclick', 'Clinical_SocialHx.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');

                                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ExercisesDetails").getMyJSONByName();
                                            }
                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_housing") {
                                            DetailsSection = "div#HousingDetails";
                                            currentJSON = response.HousingHxFill_JSON;

                                            if (currentJSON != null) {
                                                var miscHx = JSON.parse(currentJSON);
                                                var miscHxId = miscHx[0].MiscHxId;
                                                var tableName = "SocialHx_MiscHx_HousingHx";
                                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #housingHistory").removeClass('hidden');
                                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #housingHistory").attr('onclick', 'Clinical_SocialHx.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');

                                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #HousingDetails").getMyJSONByName();
                                            }

                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_caffeineintake") {
                                            DetailsSection = "div#CaffeineIntakDetails";
                                            currentJSON = response.CaffeineIntakeHxFill_JSON;
                                            if (currentJSON != null) {
                                                var miscHx = JSON.parse(currentJSON);
                                                var miscHxId = miscHx[0].MiscHxId;
                                                var tableName = "SocialHx_MiscHx_CaffeineIntakHx";
                                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #caffeineHistory").removeClass('hidden');
                                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Miscellaneous #caffeineHistory").attr('onclick', 'Clinical_SocialHx.showSocialHxItemHistory(' + miscHxId + ', \'' + tableName + '\')');

                                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #CaffeineIntakDetails").getMyJSONByName();
                                            }
                                        }
                                        else if (SocialHxType.toLowerCase() == "miscellaneous_travel") {
                                            DetailsSection = "div#divTravelDetailsHx";
                                            currentJSON = response.TravelHxFill_JSON;
                                            if (currentJSON != null) {
                                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx").getMyJSONByName();
                                            }
                                            Clinical_SocialHx.SocialHxMiscHxTravelHxSearch();
                                        }
                                        else {

                                        }

                                        var detailsJSON = '';
                                        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                            var statusId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscChildStatus > li.active").attr('id');
                                            if (statusId == null && miscHx != null) {
                                                statusId = miscHx[0].MiscChildStatus;
                                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType.toLowerCase());
                                                if (cachedJSON != '') {
                                                    detailsJSON = cachedJSON;
                                                    Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), currentJSON, DetailsSection, "#ulMiscChildStatus");
                                                    //.done(function () {
                                                    //    var childLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscChildStatus > li.active");
                                                    //    if (childLi.length == 0) {
                                                    //        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscChildStatus > li#" + statusId).addClass('active');
                                                    //    }
                                                    //});
                                                }
                                                else {
                                                    Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), currentJSON, DetailsSection, "#ulMiscChildStatus");
                                                }
                                            }
                                            else {
                                                Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), currentJSON, DetailsSection, "#ulMiscChildStatus");
                                            }
                                        }
                                        else {
                                            Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), currentJSON, DetailsSection, "#ulMiscChildStatus");
                                        }

                                        //Start 08-01-2015 Muhammad Arshad Serializing the Current Details form
                                        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  ' + DetailsSection).data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').serialize());
                                        //Start 08-01-2015 Muhammad Arshad Serializing the Current Details form

                                    }
                                    else {
                                        self = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx");
                                    }
                                    //if (firstLoad == true) {
                                    //    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMisStatus");
                                    //}
                                }

                            }
                            else {
                                var chkUnremarkable = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable")
                                Clinical_SocialHx.unRemarkableSocialHx(chkUnremarkable, "1");
                            }
                        }
                        else {
                            if (SocialHxType.toLowerCase() == "miscellaneous_occupation") {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == "miscellaneous_sleep") {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SleepDetails").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == "miscellaneous_travel") {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #divTravelDetailsHx").getMyJSONByName();
                            }
                            if (SocialHxType.toLowerCase() == "miscellaneous_exercises") {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ExercisesDetails").getMyJSONByName();
                            }
                            if (SocialHxType.toLowerCase() == "miscellaneous_housing") {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #HousingDetails").getMyJSONByName();
                            }
                            if (SocialHxType.toLowerCase() == "miscellaneous_caffeineintake") {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #CaffeineIntakDetails").getMyJSONByName();
                            }
                            if (SocialHxType.toLowerCase() == "miscellaneous_components") {
                                var arrComponents = JSON.parse(response.socialHxMiscHxComponentLoad_JSON);
                                Clinical_SocialHx.loadMiscHxComponents('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#MiscHxMainStatus #ulMiscStatus", arrComponents, "", "MiscHxMainStatus", "");
                            }
                            if (SocialHxType == 'miscellaneous_occupation') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails");
                                var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails").getMyJSONByName();

                                }
                            }
                            else if (SocialHxType == 'miscellaneous_sleep') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails");
                                var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails").getMyJSONByName();
                                }
                            }
                            else if (SocialHxType == 'miscellaneous_exercises') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails");
                                var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails").getMyJSONByName();
                                }
                            }
                            else if (SocialHxType == 'miscellaneous_housing') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails");
                                var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails").getMyJSONByName();
                                }
                            }
                            else if (SocialHxType == 'miscellaneous_caffeineintake') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails");
                                var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            if (cachedJSON.RadCaffieneharmful == true) {
                                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", false);
                                            }
                                            else if (cachedJSON.RadCaffieneharmful == false) {
                                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", false);
                                            }
                                            else {
                                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
                                            }
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails").getMyJSONByName();
                                }
                            }
                            else if (SocialHxType == 'miscellaneous_travel') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx");
                                var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, -1, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {

                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx").getMyJSONByName();
                                }
                            }
                        }

                        //Start//16/12/2015//Ahmad Raza//Serializing the form
                        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());
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

            //   Clinical_SocialHx.isMiscellaneousTabTrigger = false;

            Clinical_SocialHx.loadSocialHxStatuses('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus", "GetTobaccoSmokingStatus", true, SocialHxType).done(function () {
                Clinical_SocialHx.fillSocialHx(SocialHxType).done(function (response) {
                    if (response != "") {
                        response = JSON.parse(response);
                        if (typeof (response.SocialHxFill_JSON) != "undefined") {
                            if (response.status != false) {
                                // Start 17/12/2015 Muhammad Irfan Checks if data of sub tabs of socialHx exists
                                Clinical_SocialHx.BindCurrentSocialHxSoapText(response,false);
                                var tabsData = JSON.parse(response.socialHxLoad_JSON);
                                if (tabsData[0].SocialHxId > 0) {
                                    Clinical_SocialHx.params.HxTypeId = tabsData[0].SocialHxId;
                                    if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").removeClass("disableAll");
                                    } else {
                                        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").addClass("disableAll");
                                    }

                                }
                                Clinical_SocialHx.checkTabColor(tabsData);

                                // End 17/12/2015 Muhammad Irfan Checks if data of sub tabs of socialHx exists

                                // Start 12/01/2016 Syed Zia,trigger first existed history tab click event

                                // End 12/01/2016 Syed Zia,Syed Zia,trigger first existed history tab click event


                                //Begin 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SocialHx.isFromTabTrigger) {
                                    Clinical_SocialHx.isFromTabTrigger = false;
                                    Clinical_SocialHx.triggerSocialHistoryTab();
                                    /* Start 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                                    // $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").addClass("disableAll");
                                    /* End 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                                }
                                //End 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date

                                var socialhx_detail = JSON.parse(response.SocialHxFill_JSON);

                                // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                                if (Clinical_SocialHx.isRemarkableFormload == false) {
                                    socialhx_detail.SocialHxUnremarkable = "false";
                                }
                                // end 13/01/2016 syed zia, for remarkable checked/ unchecked

                                var self = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx");
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

                                    Clinical_SocialHx.showParentData(socialhx_detail);

                                    Clinical_SocialHx.params.mode = "Edit";
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
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Tobacco #aHistory").removeClass('hidden');
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Tobacco #aHistory").attr('onclick', 'Clinical_SocialHx.showSocialHxTobaccoHistory(' + socialHxId + ')');

                                        }
                                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
                                        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Tobacco', -1);
                                            if (cachedJSON != '') {
                                                utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus li#" + cachedJSON.StatusId).addClass("active");
                                                });
                                            }
                                            else {
                                                Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.TobaccoHxFill_JSON, "div#Tobacco", "#ulSmokingStatus");
                                            }
                                        }
                                        else {
                                            Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.TobaccoHxFill_JSON, "div#Tobacco", "#ulSmokingStatus");

                                        }
                                        //Start//16/12/2015//Ahmad Raza//Serializing the Tobacco form
                                        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').serialize());
                                        //End//16/12/2015//Ahmad Raza//Serializing the Tobacco form
                                        //Clinical_SocialHx.CacheTabJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSON();
                                        Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                                        Clinical_SocialHx.PreviousTab = 'tobacco';
                                        Clinical_SocialHx.enableDisableCounsellingTopic('tobacco');

                                    }
                                    else if (SocialHxType.toLowerCase() == "alcohol") {
                                        if (response.AlcoholHxFill_JSON != null) {
                                            var tableName = 'SocialHx_Alcohol';
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Alcohol #alcoholHistory").removeClass('hidden');
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #Alcohol #alcoholHistory").attr('onclick', 'Clinical_SocialHx.showSocialHxItemHistory(' + socialHxId + ', \'' + tableName + '\')');

                                        }
                                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol");
                                        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Alcohol', -1);
                                            if (cachedJSON != '') {
                                                utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Alcohol #ulAlcoholStatus li#" + cachedJSON.StatusId).addClass("active");
                                                });
                                            }
                                            else {
                                                Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.AlcoholHxFill_JSON, "div#Alcohol", "#ulAlcoholStatus");
                                            }
                                        }
                                        else {
                                            Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.AlcoholHxFill_JSON, "div#Alcohol", "#ulAlcoholStatus");
                                        }
                                        //Start//16/12/2015//Ahmad Raza//Serializing the Alcohol form
                                        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Alcohol').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #Alcohol').serialize());
                                        //End//16/12/2015//Ahmad Raza//Serializing the Alcohol form
                                        //Clinical_SocialHx.CacheTabJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSON();
                                        Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                                        Clinical_SocialHx.PreviousTab = 'alcohol';
                                        Clinical_SocialHx.enableDisableCounsellingTopic('alcohol');
                                    }
                                    else if (SocialHxType.toLowerCase() == "drug") {
                                        if (response.DrugAbuseFill_JSON != null) {
                                            var tableName = 'SocialHx_DrugAbuse';
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #DrugAbuse #drugHistory").removeClass('hidden');
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #DrugAbuse #drugHistory").attr('onclick', 'Clinical_SocialHx.showSocialHxItemHistory(' + socialHxId + ', \'' + tableName + '\')');

                                        }
                                        //$("#ddlDrugType option:selected")
                                        $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType option").removeAttr("selected");
                                        $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("clearSelection");
                                        $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("refresh");

                                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse");
                                        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('DrugAbuse', -1);
                                            if (cachedJSON != '') {
                                                utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                                    if (cachedJSON != '') {
                                                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val(cachedJSON.DrugType.split(','));
                                                    }
                                                    $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                                    $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect({
                                                        enableFiltering: true,
                                                        enableCaseInsensitiveFiltering: true,
                                                    });
                                                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#DrugAbuse #ulDrugStatus li#" + cachedJSON.StatusId).addClass("active");
                                                });
                                            }
                                            else {
                                                Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.DrugAbuseFill_JSON, "div#DrugAbuse", "#ulDrugStatus");
                                            }
                                        }
                                        else {
                                            Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.DrugAbuseFill_JSON, "div#DrugAbuse", "#ulDrugStatus");

                                        }
                                        $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect({
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
                                        //$('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").parent().find('[data-toggle="dropdown"]').tooltip({
                                        //    placement: 'right',
                                        //    container: 'body',
                                        //    title: function () {
                                        //        return $(this).attr('title');
                                        //    }
                                        //});



                                        /* Options List ToolTip text
                                        Author: Muhammad Azhar Shahzad
                                        Date: 17 Dec 2015*/
                                        $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").parent().find('.multiselect-container li').not('.filter, .group').tooltip({
                                            placement: 'right',
                                            container: 'body',
                                            title: function () {
                                                // getting Value of hover options
                                                var value = $(this).find('input').val();
                                                return $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType Option").map(function (index, element) {
                                                    if (element.value == value) {
                                                        return $(element).attr("refvalue");
                                                    }
                                                }).get(0);
                                            }
                                        });




                                        //End Azhar Changed
                                        //Start//17/12/2015//Ahmad Raza//Serializing the DrugAbuse form
                                        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #DrugAbuse').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #DrugAbuse').serialize());
                                        //End//17/12/2015//Ahmad Raza//Serializing the DrugAbuse form

                                        //Clinical_SocialHx.CacheTabJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSON();
                                        Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                                        Clinical_SocialHx.PreviousTab = 'drug';
                                    }
                                    else if (SocialHxType.toLowerCase() == "sexual") {
                                        if (response.DrugAbuseFill_JSON != null) {
                                            var tableName = 'SocialHx_SexualHx';
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #SexualHx #sexualHistory").removeClass('hidden');
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx  #SexualHx #sexualHistory").attr('onclick', 'Clinical_SocialHx.showSocialHxItemHistory(' + socialHxId + ', \'' + tableName + '\')');

                                        }

                                        //self = $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx");
                                        $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD option").removeAttr("selected");
                                        $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("clearSelection");
                                        $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("refresh");

                                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx");
                                        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                            var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Sexual', -1);
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

                                                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus li#" + cachedJSON.StatusId).addClass("active");
                                                });
                                            }
                                            else {
                                                Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.SexualHxFill_JSON, "div#SexualHx", "#ulSexualStatus");
                                            }
                                        }
                                        else {
                                            Clinical_SocialHx.bindCurrentTabJSON(SocialHxType.toLowerCase(), response.SexualHxFill_JSON, "div#SexualHx", "#ulSexualStatus");
                                        }


                                        $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect({
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
                                                //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #SexualHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #SexualHx').serialize());
                                                //End//17/12/2015//Ahmad Raza//Serializing the Sexual form
                                                return listTitle;
                                            }
                                            //End//17/12/2015//Ahmad Raza//Multiselect implimented in SocialHx's Sexual tab
                                        });
                                        //Start//16/12/2015//Ahmad Raza//Serializing the Sexual form
                                        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #SexualHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #SexualHx').serialize());
                                        //End//16/12/2015//Ahmad Raza//Serializing the Sexual form

                                        //Clinical_SocialHx.CacheTabJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSON();
                                        Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                                        Clinical_SocialHx.PreviousTab = 'sexual';
                                        if ($('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualExposedToSTD option:selected').val() == "") {
                                            var objSTD = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD');
                                            objSTD.attr("disabled", "disabled");
                                            objSTD.find("option:selected").removeAttr("selected");
                                            objSTD.multiselect("disable");
                                            objSTD.multiselect("clearSelection");
                                        }
                                    }
                                    else if (SocialHxType.toLowerCase() == "miscellaneous") {

                                    }
                                    else {
                                        self = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx");
                                    }
                                }
                                else {
                                    var chkUnremarkable = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable")
                                    Clinical_SocialHx.unRemarkableSocialHx(chkUnremarkable, "1");
                                }
                                //Start//16/12/2015//Ahmad Raza//Serializing the form
                                $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());
                                //End//16/12/2015//Ahmad Raza//Serializing the form
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        }
                        else {


                            var $row = $('<tr/>');
                            $row.append('<td style="display:none;"></td><td>&nbsp;</td><td>No Known Social History</td><td></td>');
                            $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").html($row);
                            $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #divSwitch").addClass('disableAll');

                            Clinical_SocialHx.showParentData(null);
                            //if (SocialHxType.toLowerCase() == "tobacco") {
                            //    $('#' + Clinical_SocialHx.params.PanelID + ' #ulSmokingStatus li:first').trigger('click');
                            //} else if (SocialHxType.toLowerCase() == "alcohol") {
                            //    $('#' + Clinical_SocialHx.params.PanelID + ' #ulAlcoholStatus li:first').trigger('click');
                            //} else if (SocialHxType.toLowerCase() == "drug") {
                            //    $('#' + Clinical_SocialHx.params.PanelID + ' #ulDrugStatus li:first').trigger('click');
                            //} else if (SocialHxType.toLowerCase() == "sexual") {
                            //    $('#' + Clinical_SocialHx.params.PanelID + ' #ulSexualStatus li:first').trigger('click');
                            //}
                            $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType,div#SexualHx #ddlSexualSTD").multiselect({

                            });

                            if (SocialHxType.toLowerCase() == 'tobacco') {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == 'alcohol') {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == 'drug') {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                            }
                            else if (SocialHxType.toLowerCase() == 'sexual') {
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                            }

                            if (SocialHxType.toLowerCase() == 'tobacco') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Tobacco', -1);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                                }

                            }
                            else if (SocialHxType == 'alcohol') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol");
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Alcohol', -1);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Alcohol #ulAlcoholStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                }
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                            }
                            else if (SocialHxType == 'drug') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse");
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('DrugAbuse', -1);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            if (cachedJSON != '') {
                                                $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val(cachedJSON.DrugType.split(','));
                                            }
                                            $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                            $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect({
                                                enableFiltering: true,
                                                enableCaseInsensitiveFiltering: true,
                                            });
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#DrugAbuse #ulDrugStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                                }
                            }

                            else if (SocialHxType == 'sexual') {
                                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx");
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Sexual', -1);
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
                                            
                                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus li#" + cachedJSON.StatusId).addClass("active");
                                        });
                                    }
                                    Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                                }
                            }

                        }
                    }
                    else {

                        /* Start 10/12/2015 Muhammad Irfan If SocialHx not saved then trigger the first list item of status */

                        //if (SocialHxType.toLowerCase() == "tobacco") {
                        //    $('#' + Clinical_SocialHx.params.PanelID + ' #ulSmokingStatus li:first').trigger('click');
                        //} else if (SocialHxType.toLowerCase() == "alcohol") {
                        //    $('#' + Clinical_SocialHx.params.PanelID + ' #ulAlcoholStatus li:first').trigger('click');
                        //} else if (SocialHxType.toLowerCase() == "drug") {
                        //    $('#' + Clinical_SocialHx.params.PanelID + ' #ulDrugStatus li:first').trigger('click');
                        //} else if (SocialHxType.toLowerCase() == "sexual") {
                        //    $('#' + Clinical_SocialHx.params.PanelID + ' #ulSexualStatus li:first').trigger('click');
                        //}

                        /* End 10/12/2015 Muhammad Irfan If SocialHx not saved then trigger the first list item of status */

                        $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType,div#SexualHx #ddlSexualSTD").multiselect({

                        });

                        //$('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect({

                        //});
                    }

                });
            });
        }
        //End 08-01-2016 Muhammad Arshad LoadMiscHx Tab

    },

    checkTabColor: function (tabsData) {
        if (tabsData[0].bAlcoholExist == 'True') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').addClass('successLight');
            Clinical_SocialHx.bAlcoholExist = true;

        }
        else {
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel.length > 0) {
                    if (!$('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').hasClass('successLight')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').addClass('successLight');
                        Clinical_SocialHx.bAlcoholExist = true;
                    }
                }
            }
            else {
                $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').removeClass('successLight');
                Clinical_SocialHx.bAlcoholExist = false;
            }
            //$('#' + Clinical_SocialHx.params.PanelID + ' #ulAlcoholStatus li:first').trigger('click');
        }
        if (tabsData[0].bDrugExist == 'True') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').addClass('successLight');
            Clinical_SocialHx.bDrugExist = true;
        }
        else {
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel.length > 0) {
                    if (!$('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').hasClass('successLight')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                        Clinical_SocialHx.bDrugExist = true;
                    }
                }
            }
            else {
                $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').removeClass('successLight');
                Clinical_SocialHx.bDrugExist = false;
            }
        }
        if (tabsData[0].bSexualExist == 'True') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').addClass('successLight');
            Clinical_SocialHx.bSexualExist = true;
        }
        else {
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel.length > 0) {
                    if (!$('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').hasClass('successLight')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').addClass('successLight');
                        Clinical_SocialHx.bSexualExist = true;
                    }
                }
            }
            else {
                $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').removeClass('successLight');
                Clinical_SocialHx.bSexualExist = false;
            }

            //$('#' + Clinical_SocialHx.params.PanelID + ' #ulSexualStatus li:first').trigger('click');
        }
        if (tabsData[0].bTobaccoExist == 'True') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').addClass('successLight');
            Clinical_SocialHx.bTobaccoExist = true;
        }
        else {
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel.length > 0) {
                    if (!$('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').hasClass('successLight')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').addClass('successLight');
                        Clinical_SocialHx.bTobaccoExist = true;
                    }
                }
            }
            else {
                $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').removeClass('successLight');
                Clinical_SocialHx.bTobaccoExist = false;
            }
        }

        if (tabsData[0].bMiscHxExist == 'True') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').addClass('successLight');
            Clinical_SocialHx.bMiscHxExist = true;
        }
        else {
            $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').removeClass('successLight');
            Clinical_SocialHx.bMiscHxExist = false;
            Clinical_SocialHx.changeTabColor("miscellaneous_components");
        }
    },

    showParentData: function (socialhx_detail) {
        Clinical_SocialHx.date = $('#' + Clinical_SocialHx.params.PanelID + " #dtSocialHxDate").val();

        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
            $('#' + Clinical_SocialHx.params.PanelID + " #dtSocialHxDate").val(Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxDate);
        }

        if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null && Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialComments != '') {
            $('#' + Clinical_SocialHx.params.PanelID + " #txtSocialComments").val(Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialComments);
        }
        else {
            if (socialhx_detail != null) {
                $('#' + Clinical_SocialHx.params.PanelID + " #txtSocialComments").val(socialhx_detail.SocialComments);
            }
            Clinical_SocialHx.overallComments = $('#' + Clinical_SocialHx.params.PanelID + " #txtSocialComments").val();
        }
        if (socialhx_detail != null) {
            Clinical_SocialHx.unremarkable = socialhx_detail.SocialHxUnremarkable == "False" || socialhx_detail.SocialHxUnremarkable == null ? false : true;;
        }
        else {
            Clinical_SocialHx.unremarkable = false;
        }

        if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked", Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable);

            if (Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable) {
                var chkUnremarkable = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable")
                Clinical_SocialHx.unRemarkableSocialHx(chkUnremarkable, "1");
            }
        }
    },
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle fill of SocialHx and it's childs as specified by SocialHxType

    loadSocialHx: function (SocialHxType, isLoadNew) {

        var currentTab = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
        currentTab = (typeof currentTab == "undefined") ? "" : currentTab;
        var DataExists = Clinical_SocialHx.isDetailExists(currentTab.toLowerCase());

        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SocialHx.socialHxJSON != '') {
            Clinical_SocialHx.cachePrevTabData(currentTab);
        }

        setTimeout(function () {
            LastSocialHx = new Object();
            LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
            LastSocialHx["SocialHxType"] = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
        }, 100);



        if (Clinical_SocialHx.bIsTriggerManually) {
            isLoadNew = Clinical_SocialHx.bIsTriggerManually;
            Clinical_SocialHx.bIsTriggerManually = false;

        }

        if (isLoadNew == true) {
            Clinical_SocialHx.loadSocialHxComponent(SocialHxType);
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
        }
        else {
            if (DataExists != false) {
                if (EMRUtility.compareFormDataWithSerialized(Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx')) {
                    //Start 12-07-2016 Humaira Yousaf
                    if (Clinical_SocialHx.isSaved == true) {
                        Clinical_SocialHx.isSaved = false;
                        var socialHxTypeCurrentTab = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                        Clinical_SocialHx.loadSocialHxComponent(socialHxTypeCurrentTab);
                        Clinical_SocialHx.loadSocialHxComponent(SocialHxType);
                        if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                        }
                    }
                    else if (Clinical_SocialHx.CacheTabJSON != '') {
                        var isChanged = false;
                        if (Clinical_SocialHx.PreviousTab == "tobacco") {
                            if (Clinical_SocialHx.CacheTabJSON != $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSON()) {
                                isChanged = true;
                            }
                        }
                        else if (Clinical_SocialHx.PreviousTab == "alcohol") {
                            if (Clinical_SocialHx.CacheTabJSON != $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSON()) {
                                isChanged = true;
                            }
                        }
                        else if (Clinical_SocialHx.PreviousTab == "drug") {
                            if (Clinical_SocialHx.CacheTabJSON != $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSON()) {
                                isChanged = true;
                            }
                        }
                        else if (Clinical_SocialHx.PreviousTab == "sexual") {
                            if (Clinical_SocialHx.CacheTabJSON != $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSON()) {
                                isChanged = true;
                            }
                        }
                        else {
                            isChanged = false;
                        }
                        if (isChanged == true) {
                            utility.myConfirm('12', function () {
                                var socialHxTypeCurrent = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                                Clinical_SocialHx.socialHxSave(socialHxTypeCurrent, false);

                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                                Clinical_SocialHx.loadSocialHxComponent(SocialHxType);
                                BackgroundLoaderShow(true);

                            }, function () {

                                var socialHxTypeCurrentTab = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                                Clinical_SocialHx.loadSocialHxComponent(socialHxTypeCurrentTab);
                                Clinical_SocialHx.loadSocialHxComponent(SocialHxType);
                                if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                                }
                            });
                        }
                        else {
                            var socialHxTypeCurrentTab = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                            Clinical_SocialHx.loadSocialHxComponent(socialHxTypeCurrentTab);
                            Clinical_SocialHx.loadSocialHxComponent(SocialHxType);
                            if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                            }
                        }

                    }
                    else {
                        var socialHxTypeCurrentTab = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                        Clinical_SocialHx.loadSocialHxComponent(socialHxTypeCurrentTab);
                        Clinical_SocialHx.loadSocialHxComponent(SocialHxType);
                        if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                        }
                    }
                    //End 12-07-2016 Humaira Yousaf
                }
                else {
                    //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Alcohol').data('serialize', null);
                    //Clinical_SocialHx.loadSocialHx(SocialHxType); Clinical_SocialHx.selectTabColor(SocialHxType);
                    Clinical_SocialHx.loadSocialHxComponent(SocialHxType);

                    if (SocialHxType.toLowerCase() != "miscellaneous_components") {
                        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                    }

                }
            }
            else {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val(SocialHxType);
                Clinical_SocialHx.loadSocialHxComponent(SocialHxType);
            }
        }



        //End//22/12/2015//Ahmad Raza//Implemented prompt problem if user changed but didn't saved data and want to go to another tab

    },

    cachePrevTabData: function (socialType) {
        var dfd = $.Deferred();
        socialType = socialType.toLowerCase();

        if (socialType == 'miscellaneous') {
            socialType += "_" + $('#' + Clinical_SocialHx.params.PanelID + " #ulMiscStatus > li.active > a").text().replace(" ", "").trim();
            socialType = socialType.toLowerCase();
        }

        if (socialType == 'tobacco') {
            $.when(Clinical_SocialHx.cacheTobaccoTabData(socialType)).then(function () {
                dfd.resolve();
            });
        }
        else if (socialType == 'alcohol') {
            $.when(Clinical_SocialHx.cacheAlcoholTabData(socialType)).then(function () {
                dfd.resolve();
            });
        }
        else if (socialType == 'drug' || socialType == 'drug abuse') {
            $.when(Clinical_SocialHx.cacheDrugTabData(socialType)).then(function () {
                dfd.resolve();
            });
        }
        else if (socialType == 'sexual' || socialType == 'sexual hx') {
            $.when(Clinical_SocialHx.cacheSexualTabData(socialType)).then(function () {
                dfd.resolve();
            });
        }
        else if (socialType == "miscellaneous_occupation" || socialType == "miscellaneous_travel" || socialType == "miscellaneous_sleep" || socialType == "miscellaneous_exercises" || socialType == "miscellaneous_housing" || socialType == "miscellaneous_caffeineintake") {
            $.when(Clinical_SocialHx.markStatusActive("ulMiscStatus", $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id'))).then(function () {
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
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SocialHx.socialHxJSON != '') {
                var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                var socialTypeMiscText = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active a").text().replace(" ", "").trim();
                var statusId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscChildStatus > li.active").attr('id');
                var updatedJSON = '';
                var detailsSection = '';
                if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Occupation') {
                    updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Sleep') {
                    updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Exercises') {
                    updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Housing') {
                    updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Caffeine Intake') {
                    updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails").getMyJSONByName();
                }
                else if ($.trim($("#ulMiscStatus > li.active > a").text()) == 'Travel') {
                    updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx").getMyJSONByName();
                }
                if (updatedJSON != '' && Clinical_SocialHx.socialHxJSON != updatedJSON) {
                    if (socialTypeMiscId != null && statusId != null) {
                        $.when(Clinical_SocialHx.cacheSocialHxJSON(socialTypeMiscId, statusId, updatedJSON, "miscellaneous_" + socialTypeMiscText)).then(function () {
                            dfd.resolve();
                        });
                        Clinical_SocialHx.SetIsLast(socialTypeMiscId, statusId, "miscellaneous_" + socialTypeMiscText);
                    }
                    else {
                        dfd.resolve();
                    }
                }
                else {
                    var json = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails").getMyJSONByName();
                    //var detailsSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails").clone();
                    //$(detailsSection).resetAllControls(null);
                    var emptyJson = $(detailsSection).getMyJSONByName();
                    if (updatedJSON != '' && statusId != null && statusId > 0 && json != '') {
                        $.when(Clinical_SocialHx.cacheSocialHxJSON(socialTypeMiscId, statusId, updatedJSON, "miscellaneous_" + socialTypeMiscText)).then(function () {
                            dfd.resolve();
                        });
                        Clinical_SocialHx.SetIsLast(socialTypeMiscId, statusId, "miscellaneous_" + socialTypeMiscText);
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
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous ul#" + ulId + " li").each(function (i, item) {
                    if ($(this).attr("id") != null && $(this).attr("id") == liId) {
                        if ($(this).hasClass("active") == false) {
                            $(this).addClass("active");
                        }
                    }
                });
            }
            else {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous ul#" + ulId + " li").each(function (i, item) {
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
            $('#' + Clinical_SocialHx.params.PanelID + ' #txtHousingPresent').val('');
            $('#' + Clinical_SocialHx.params.PanelID + ' #txtHousingPast').val('');
            $('#' + Clinical_SocialHx.params.PanelID + ' #txtHousingComments').val('');
            $('#' + Clinical_SocialHx.params.PanelID + ' #txtSleepHours').val('');
        }
        else {
            dfd.resolve();
        }
        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());
        return dfd;
    },

    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle load of SocialHx Miscellanous and it's childs Status as specified by MiscTab
    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
    loadMiscHxTab: function (MiscHxType, liId, firstLoad) {
        //Start 28-10-2016 Humaira Yousaf for misc tab
        Clinical_SocialHx.markStatusActive('ulMiscStatus', liId);
        //End 28-10-2016 Humaira Yousaf for misc tab
        if (MiscHxType != null && MiscHxType != "") {
            var DetailsDivId = "";
            var StatusType = "";
            var StatusCtrl = '#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus";


            if (MiscHxType.toLowerCase() == "occupation") {
                BackgroundLoaderShow(true);
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Occupation Status");
                DetailsDivId = "OccupationDetails";
                StatusType = "occupation";
                MDVisionService.lookups("getOccupationStatus", true).done(function (result) {
                    result = JSON.parse(result['getOccupationStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    Clinical_SocialHx.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    Clinical_SocialHx.loadSocialHx('miscellaneous_occupation', true);
                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").prop('checked', true);
                    Clinical_SocialHx.EnableDisableToDateOccupation();
                    BackgroundLoaderShow(false);
                });

            }
            else if (MiscHxType.toLowerCase() == "sleep") {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Sleeping Status");
                DetailsDivId = "SleepDetails";
                BackgroundLoaderShow(true);
                StatusType = "sleep";
                MDVisionService.lookups("getSleepHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getSleepHxStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    Clinical_SocialHx.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    Clinical_SocialHx.loadSocialHx('miscellaneous_sleep', true);
                    BackgroundLoaderShow(false);
                });

            }
            else if (MiscHxType.toLowerCase() == "exercises") {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Exercises Status");
                DetailsDivId = "ExercisesDetails";
                StatusType = "exercises";
                BackgroundLoaderShow(true);
                MDVisionService.lookups("getExercisesHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getExercisesHxStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    Clinical_SocialHx.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    Clinical_SocialHx.loadSocialHx('miscellaneous_exercises', true);
                    BackgroundLoaderShow(false);
                });

            }
            else if (MiscHxType.toLowerCase() == "housing") {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Housing Status");
                DetailsDivId = "HousingDetails";
                StatusType = "housing";
                BackgroundLoaderShow(true);
                MDVisionService.lookups("getHousingHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getHousingHxStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    Clinical_SocialHx.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    Clinical_SocialHx.loadSocialHx('miscellaneous_housing', true);
                    BackgroundLoaderShow(false);
                });



            }
            else if (MiscHxType.toLowerCase() == "caffeine intake") {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Caffeine Status");
                DetailsDivId = "CaffeineIntakDetails";
                StatusType = "caffeine intake";

                //Start 11/01/2016 Syed Zia Code to  Save/Update MiscHx Caffeine Intake Hx
                BackgroundLoaderShow(true);
                MDVisionService.lookups("getCaffeineIntakeHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getCaffeineIntakeHxStatus']);
                    //Abid Ali/01/20/2016/ "firstLoad" param checks whether Miscellenous tab page is first time triggered.
                    Clinical_SocialHx.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    Clinical_SocialHx.loadSocialHx('miscellaneous_caffeineintake', true);

                    BackgroundLoaderShow(false);
                });

                //Start 11/01/2016 Syed Zia Code to  Save/Update MiscHx Caffeine Intake Hx
            }
            else if (MiscHxType.toLowerCase() == "travel") {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #headingChildMiscStatus").text("Status");
                DetailsDivId = "divTravelDetailsHx";
                StatusType = "Travel";
                BackgroundLoaderShow(true);
                MDVisionService.lookups("getTravelHxStatus", true).done(function (result) {
                    result = JSON.parse(result['getTravelHxStatus']);
                    Clinical_SocialHx.loadMiscChildStatus(StatusCtrl, result, null, DetailsDivId, StatusType, firstLoad);
                    Clinical_SocialHx.loadSocialHx('miscellaneous_travel', true);

                    BackgroundLoaderShow(false);
                });
            }
            else if (MiscHxType.toLowerCase() == "miscellaneous_components") {
                BackgroundLoaderShow(true);
                Clinical_SocialHx.loadSocialHx('miscellaneous_components', true);
                BackgroundLoaderShow(false);
            }



            //Show respective details area
            var mainStatuUlId = "ulMiscStatus";
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div[id*='Details']").each(function (i, item) {
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
                        Clinical_SocialHx.markStatusActive(mainStatuUlId, null);
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
                onClick = " Clinical_SocialHx.loadMiscHxTab('" + item.ComponentName.toLowerCase() + "', '" + item.ComponentId + "')";
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
        if (Clinical_SocialHx.isMiscellaneousTabTrigger == false) {
            //Clinical_SocialHx.loadMiscHxTab(activeLiComponentName, activeLiId, true);
            //set isMiscellaneousTabTrigger = true;
            //$(Crtl).find("li:first-child").trigger("click");
            Clinical_SocialHx.isMiscellaneousTabTrigger = true;
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
                    if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                        var found = false;
                        for (var a = 0; a < 5; a++) {
                            if (a == 0) {
                                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel.length > 0) {
                                    $.each(($(Crtl).find("li")), function (i, item) {
                                        if ($(item).find("a").text().toLowerCase().indexOf("occupation") > -1) {
                                            if (!$(item).hasClass("active")) {
                                                $(item).addClass("active");
                                            }
                                            Clinical_SocialHx.ClearStatusLiAndDetailSection();
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
                                            Clinical_SocialHx.ClearStatusLiAndDetailSection();
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
                                            Clinical_SocialHx.ClearStatusLiAndDetailSection();
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
                                            Clinical_SocialHx.ClearStatusLiAndDetailSection();
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
                                            Clinical_SocialHx.ClearStatusLiAndDetailSection();
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
                                            Clinical_SocialHx.ClearStatusLiAndDetailSection();
                                            $(item).trigger("click");
                                        }
                                    });
                                    break;
                                }
                            }
                        }

                    }
                    else {
                        Clinical_SocialHx.ClearStatusLiAndDetailSection();
                    }

                    //$(Crtl).find("li:first-child").trigger("click");
                }
            }

        }
        setTimeout(function () {
            LastSocialHx = new Object();
            LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
            LastSocialHx["SocialHxType"] = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
        });



    },
    ClearStatusLiAndDetailSection: function () {
        $('#' + Clinical_SocialHx.params.PanelID + " #ulMiscChildStatus").html("");
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#OccupationDetails').css("display", "none");
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#ExercisesDetails').css("display", "none");
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#HousingDetails').css("display", "none");
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#CaffeineIntakDetails').css("display", "none");
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#SleepDetails').css("display", "none");
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
                    onClick = "Clinical_SocialHx.markStatusActive('ulMiscChildStatus', '" + item.Value + "');";

                    //Start 12/01/2016 Syed Zia,for child tab selection click
                    if (StatusType != null && StatusType.toLowerCase() == "occupation") {
                        onClick += "Clinical_SocialHx.enableDisableOccupationControls('" + item.Name + "','" + item.Value + "');";
                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "exercises") {
                        onClick += "Clinical_SocialHx.enableDisableExercisesControls('" + item.Name + "','" + item.Value + "');";
                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "caffeine intake") {

                        onClick += "Clinical_SocialHx.enableDisableCaffeineIntakeControls('" + item.Name + "','" + item.Value + "');";
                        // onClick += "Clinical_SocialHx.loadSocialHxComponent('miscellaneous_caffeineintake');";

                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "sleep") {
                        onClick += "Clinical_SocialHx.enableDisableSleepControls('" + item.Name + "','" + item.Value + "');";
                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "housing") {
                        onClick += "Clinical_SocialHx.enableDisableHousingControls('" + item.Name + "','" + item.Value + "');";
                    }
                    else if (StatusType != null && StatusType.toLowerCase() == "travel") {
                        onClick += "Clinical_SocialHx.enableDisableTravelControls('" + item.Name + "','" + item.Value + "');";
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
            var self = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                    Clinical_SocialHx.resetControlValue(this);
                }
            });
            $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {
                if (!editMode)
                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfTravelDetailHxId").val(-1);

                if (currentStatus == "Does Not Travel") {
                    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxFromDate').val();
                    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxFromDate').attr('disabled', true);
                    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxToDate').val();
                    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxToDate').attr('disabled', true);
                    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #txtTravelHxLocation').val();
                    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #txtTravelHxLocation').attr('disabled', true);
                }
                else {
                    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxFromDate').removeAttr('disabled');
                    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dtTravelHxToDate').removeAttr('disabled');
                    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #txtTravelHxLocation').removeAttr('disabled');
                }
                $.each($('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#divTravelDetailsHx #dgvSocialHxMiscHxTravelHx tbody tr'), function (i, itm) {
                    if ($(itm).attr('statusid'))
                        if ($(itm).attr('statusid') == $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id") && $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").text().trim() == "Does Not Travel")
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").addClass('disableAll');
                        else
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").removeClass('disableAll');
                    else {
                        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #secTravelDetail").removeClass('disableAll');
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
            var self = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#OccupationDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                    // Faizan ameen
                    // Date: 21-Oct-2016
                    // uncoment the below line
                    // BUG QAC2-502
                    Clinical_SocialHx.resetControlValue(this);
                }
            });
            $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {
            });
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #OccupationDetails #RadOccupationPastExperience").prop('checked', true);
            Clinical_SocialHx.EnableDisableToDateOccupation();
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfOccupationHxId").val(-1);
        }
    },
    //end 12-01-2016 Syed Zia, for Occupation controls
    enableDisableExercisesControls: function (currentStatus, currentStatusValue) {
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#ExercisesDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (currentStatus.toLowerCase() == "does not exercise") {
                    if ($(this).attr("id") != "txtExercisesComments") {
                        $(this).attr("disabled", "disabled");
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            Clinical_SocialHx.resetControlValue(this);
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
                if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                    Clinical_SocialHx.resetControlValue(this);
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
            });
        }

        //loadSocialHxComponent
        $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {
            if (!isPopulated) {
                var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails");
                detailSection.resetAllControls(null);
            }

        })
    },

    //Author: Muhammad Arshad
    //Date: 01-05-2016
    //This function will handle enabling/disabling of Exercises controls on Miscellanous Tab
    enableDisableCaffeineIntakeControls: function (currentStatus, currentStatusValue) {
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#CaffeineIntakDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (currentStatus.toLowerCase() == "does not use") {
                    if ($(this).attr("id") != "txtCaffeineComments") {
                        $(this).attr("disabled", "disabled");
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            Clinical_SocialHx.resetControlValue(this);
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
                if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                    Clinical_SocialHx.resetControlValue(this);
                }
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
            });

            $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails");
                    detailSection.resetAllControls(null);
                }

            });
        }
    },
    //Start 12/01/2016 Syed Zia,for sleep enable/ disable
    enableDisableSleepControls: function (currentStatus, currentStatusValue) {
        if (currentStatus != null && currentStatus != "") {
            Clinical_SocialHx.validateSleepHours($('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#SleepDetails #txtSleepHours'));
            var self = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#SleepDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                    //Start || 15 July, 2016 || ZeeshanAK || Fix for EMR-1610
                    //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #txtSleepHours').val('');
                    //End   || 15 July, 2016 || ZeeshanAK || Fix for EMR-1610
                    //  Clinical_SocialHx.resetControlValue(this);
                    Clinical_SocialHx.resetControlValue(this);
                }
            });

            $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {

            });
        }

        //if (currentStatus != null && currentStatus != "") {
        //    var self = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#OccupationDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
        //        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
        //            // Faizan ameen
        //            // Date: 21-Oct-2016
        //            // uncoment the below line
        //            // BUG QAC2-502
        //            Clinical_SocialHx.resetControlValue(this);
        //        }
        //    });


        //    $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {
        //    });
        //}
    },

    enableDisableHousingControls: function (currentStatus, currentStatusValue) {
        if (currentStatus != null && currentStatus != "") {
            var self = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous section#sectionMiscDetails div#HousingDetails').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                    //  Clinical_SocialHx.resetControlValue(this);
                }
            });
            $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {
            });
        }
    },

    //End 12/01/2016 Syed Zia,for child tab selection click


    //Start//16/12/2015//Ahmad Raza//Method to compare form data with serialized data
    socialHistoryStateCheck: function (SocialHxType) {

        if (SocialHxType.toLowerCase() == "tobacco") {
            return EMRUtility.compareFormDataWithSerialized(Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #Tobacco');
        }
        else if (SocialHxType.toLowerCase() == "alcohol") {
            return EMRUtility.compareFormDataWithSerialized(Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #Alcohol');
        }
        else if (SocialHxType.toLowerCase() == "drug") {

            return EMRUtility.compareFormDataWithSerialized(Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #DrugAbuse');
            $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect({

            });
        }
        else if (SocialHxType.toLowerCase() == "sexual") {

            return EMRUtility.compareFormDataWithSerialized(Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #SexualHx');
            // $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect({

            // });
        }
        else if (SocialHxType.toLowerCase() == "miscellaneous") {
            return EMRUtility.compareFormDataWithSerialized(Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #Miscellaneous');

        } else {
            return EMRUtility.compareFormDataWithSerialized(Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx');

        }

    },
    //End//16/12/2015//Ahmad Raza//Method to compare form data with serialized data

    loadTabaccoTabnUnserializeForm: function () {
        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx div').removeClass('active');
        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ulSocialHxTabsItems>li').removeClass('active');

        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').data('serialize', null);
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Alcohol').data('serialize', null);
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #DrugAbuse').data('serialize', null);
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #SexualHx').data('serialize', null);
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Miscellaneous').data('serialize', null);

        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx  #Tobacco').addClass('active');
        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ulSocialHxTabsItems#listTobacco').addClass('active');
        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Tobacco').addClass('active');
        Clinical_SocialHx.loadSocialHx('tobacco', true);
        Clinical_SocialHx.selectTabColor('tobacco');
        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ulSocialHxTabsItems#listTobacco').addClass('active');
        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Tobacco').addClass('active');
        // $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco a').trigger('click');
    },

    triggerSocialHistoryTab: function () {

        try {
            if (LastSocialHx != null && LastSocialHx.PatientId == $('#PatientProfile #hfPatientId').val()) {
                $('#' + Clinical_SocialHx.params.PanelID + ' #' + LastSocialHx.SocialHxType + " a").trigger('click');

            }
            else {
                Clinical_SocialHx.bIsTriggerManually = true;
                if (Clinical_SocialHx.bTobaccoExist == true) {
                    $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco a').trigger('click');
                }
                else if (Clinical_SocialHx.bAlcoholExist == true) {
                    $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol a').trigger('click');
                }
                else if (Clinical_SocialHx.bDrugExist == true) {
                    $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse a').trigger('click');
                }
                else if (Clinical_SocialHx.bSexualExist == true) {
                    $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx a').trigger('click');
                }
                else if (Clinical_SocialHx.bMiscHxExist == true) {
                    $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx a').trigger('click');
                }
                else {
                    $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco a').trigger('click');
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
        self = $('#' + Clinical_SocialHx.params.PanelID + " " + CurrentTabContainerId);
        if (alcoholhx_detail.length > 0) {
            $.each(alcoholhx_detail, function (i, item) {

                //Start 21-12-2015 Muhammad Arshad handling status related controls enabling disabling on edit mode
                var statusId = "";
                var SocialHxType = '';
                var miscHxType = '';
                if (TabType != null && TabType.toLowerCase() == "tobacco") {
                    statusId = item.SmokingStatus;
                    Clinical_SocialHx.SeletedChildStatus = statusId;
                    SocialHxType = "Tobacco";
                }
                else if (TabType != null && TabType.toLowerCase() == "alcohol") {
                    statusId = item.AlcoholStatus;
                    Clinical_SocialHx.SeletedChildStatus = statusId;
                    SocialHxType = "Alcohol";
                }
                else if (TabType != null && TabType.toLowerCase() == "drug") {
                    statusId = item.DrugStatus;
                    Clinical_SocialHx.SeletedChildStatus = statusId;
                    SocialHxType = "DrugAbuse";
                }
                else if (TabType != null && TabType.toLowerCase() == "sexual") {
                    statusId = item.SexualStatus;
                    Clinical_SocialHx.SeletedChildStatus = statusId;
                    SocialHxType = "Sexual";
                }
                else if (TabType != null && TabType.toLowerCase().indexOf("miscellaneous") > -1) {
                    statusId = item.MiscChildStatus;
                    Clinical_SocialHx.SeletedChildStatus = statusId;
                    miscHxType = TabType;
                    SocialHxType = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                }

                //End 21-12-2015 Muhammad Arshad handling status related controls enabling disabling on edit mode
                var detailsJSON = '';
                var foundDataFromJson = false;
                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    var cachedJSON = '';
                    if (miscHxType) {
                        cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(SocialHxType, statusId, miscHxType);
                    }
                    else {
                        cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(SocialHxType, statusId);
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
                        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li").removeClass("active");
                        $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li#" + detailsJSON.StatusId).addClass("active");
                        if (TabType != null && TabType.toLowerCase() == "sexual") {
                            Clinical_SocialHx.setSexualControlls(detailsJSON.StatusId);
                        }
                    }
                    else {
                        if (statusId != "") {
                            //Start 28-10-2016 Modified By Humaira Yousaf to stop triggering of click event
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li").removeClass("active");
                            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li#" + statusId).addClass("active");
                            //End 28-10-2016 Modified By Humaira Yousaf to stop triggering of click event
                            if (TabType != null && TabType.toLowerCase() == "sexual") {
                                Clinical_SocialHx.setSexualControlls(statusId);
                            }
                        }
                    }
                    if (TabType != null && TabType.toLowerCase() == "drug") {
                        //$('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType option").removeAttr("selected");
                        //$('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("clearSelection");
                        //$('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("refresh");
                        $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").val(item.DrugType.split(','));
                        $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect("refresh");
                        // Start 16-12-2015 Muhammad Arshad showing details in Drug multiselect tooltip
                        $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect({
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
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionTobacco #chkTobaccoRecentlyQuit").prop("checked", false);
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionTobacco #chkTobaccoRecentlyQuit").trigger("click");

                        }
                        if (item.TobaccoWouldQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionTobacco #chkTobaccoWouldQuit").prop("checked", false);
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionTobacco #chkTobaccoWouldQuit").trigger("click");

                        }
                        var activeStatus = $.trim($('#' + Clinical_SocialHx.params.PanelID + ' #ulSmokingStatus').find('li.active > a').text());
                        if (activeStatus == 'Never smoker' || activeStatus == 'Unknown if ever smoked' || activeStatus == 'Does not chew tobacco' || activeStatus == 'Non Smoker') {
                            var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
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
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionAlcohol #chkAlcoholRecentlyQuit").prop("checked", false);
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionAlcohol #chkAlcoholRecentlyQuit").trigger("click");

                        }
                        if (item.AlcoholNotReadyToQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionAlcohol #chkAlcoholNotReadyToQuit").prop("checked", false);
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionAlcohol #chkAlcoholNotReadyToQuit").trigger("click");

                        }
                        if (item.AlcoholWouldQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionAlcohol #chkAlcoholWouldQuit").prop("checked", false);
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionAlcohol #chkAlcoholWouldQuit").trigger("click");

                        }

                    }
                    if (TabType != null && TabType.toLowerCase() == "drug") {
                        if (item.DrugRecentlyQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #chkDrugRecentlyQuit").prop("checked", false);
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #chkDrugRecentlyQuit").trigger("click");

                        }
                        if (item.DrugWouldQuit.toLowerCase() == "true") {
                            // here we initially set its checked=false, so that when it will be clicked then controls should be disabled
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #chkDrugWouldQuit").prop("checked", false);
                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #chkDrugWouldQuit").trigger("click");

                        }
                    }
                    if (TabType != null && TabType.toLowerCase() == "sexual") {
                        //$('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD option").removeAttr("selected");
                        //$('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("clearSelection");
                        //$('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("refresh");
                        $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").val(item.SexualSTD.split(','));
                        $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("refresh");
                        //Start//17/12/2015//Ahmad Raza//Multiselect implimented in SocialHx's Sexual tab
                        $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect({
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
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #RadSexualNoAbusedSexually").prop('checked', true);
                        }
                        else {
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #RadSexualYesAbusedSexually").prop('checked', true);
                        }

                        if (item.RadSexualYesPainWithIntercourse == "No") {
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #RadSexualNoPainWithIntercourse").prop('checked', true);
                        }
                        else {
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #RadSexualYesPainWithIntercourse").prop('checked', true);
                        }
                        if (item.RadSexualYesPregnant == "No") {
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #RadSexualNoPregnant").prop('checked', true);
                        }
                        else {
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #RadSexualYesPregnant").prop('checked', true);
                        }
                        if (item.SexualExposedToSTD.toLowerCase() == "yes") {
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("enable");

                        }
                        else {
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("disable");
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("clearSelection");
                            $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx #ddlSexualSTD").multiselect("refresh");
                        }
                        //End//14/01/2016//Ahmad Raza//Values not binding in Social hx's Sexual tab issue, fixed
                        //$('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse #ddlDrugType").multiselect({

                        //});
                    }
                    if (TabType != null && TabType.toLowerCase().indexOf("miscellaneous") > -1) {
                        if (TabType.toLowerCase() == "miscellaneous_occupation") {

                            var myvar = "";
                        }

                        /* Start 11/01/2015 Syed Zia, for checkbox checked/unchecked */
                        if (TabType.toLowerCase() == "miscellaneous_caffeineintake") {
                            if (item.CaffeineHarmful.toLowerCase() == "true") {
                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", false);
                            }
                            else if (item.CaffeineHarmful.toLowerCase() == "false") {
                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", false);
                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                            }
                            else {
                                $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
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
                    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li").each(function () {
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
                $("#" + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Alcohol').resetAllControls(null);
            }
            else if (TabType == 'tobacco') {
                $("#" + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #Tobacco').resetAllControls(null);
            }

            else if (TabType == 'drug') {
                $("#" + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #DrugAbuse').resetAllControls(null);
            }
            else if (TabType == 'sexual') {
                $("#" + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #SexualHx').resetAllControls(null);
            }
            //* Start 01/14/2016 Abid Ali for bug
            // $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx " + ulTabStatusId + " li").first().trigger("click");
            //* Start 01/14/2016 Abid Ali for bug
        }
    },
    setSexualControlls: function (statusId) {
        var currentStatus = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus li#" + statusId).text().trim();
        if (currentStatus != null && currentStatus != "") {
            if (currentStatus == "Practices safe sex") {
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "ddlSexualExposedToSTD" || $(this).attr("id") == "ddlSexualSTD" || $(this).attr("id") == "RadSexualYesAbusedSexually" || $(this).attr("id") == "RadSexualNoAbusedSexually") {
                        $(this).attr("disabled", "disabled");
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }

                    if ($(this).attr("id") == "ddlSexualExposedToSTD") {
                        Clinical_SocialHx.enableDisableSTDControls(this);
                    }
                });
            }
            else if (currentStatus == 'Sexually Active' || currentStatus == 'Not Sexually Active') {
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    $(this).removeAttr("disabled");
                    if ($(this).attr("id") == "ddlSexualExposedToSTD") {
                        Clinical_SocialHx.enableDisableSTDControls(this);
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

        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
            if (isRemarkable == true) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
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
                    if (Clinical_SocialHx.params.patientID == null) {
                        patientId = $('#PatientProfile #hfPatientId').val();
                    } else {
                        patientId = Clinical_SocialHx.params.patientID;
                    }
                    var SocialHxData = {
                        SocialHxId: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(),
                        PatientId: patientId,
                        SocialHxDate: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val(),
                        SocialHxUnremarkable: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked"),
                        SocialComments: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val(),
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
                    Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
                }
                else {
                    var patientId;
                    if (Clinical_SocialHx.params.patientID == null) {
                        patientId = $('#PatientProfile #hfPatientId').val();
                    } else {
                        patientId = Clinical_SocialHx.params.patientID;
                    }
                    var SocialHxData = {
                        SocialHxId: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(),
                        PatientId: patientId,
                        SocialHxDate: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val(),
                        SocialHxUnremarkable: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked"),
                        SocialComments: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val(),
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
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #btnSocialHxSave').hide();
            }
            else {
                $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #btnSocialHxSave').show();
            }

            /* Start 23/12/2015 Muhammad Irfan for bug # EMR-153 */
            $('#' + Clinical_SocialHx.params.PanelID + ' #ulSocialHxTabsItems').addClass('disableAll');
            /* End 23/12/2015 Muhammad Irfan for bug # EMR-153 */

            /* Start 07/01/2016 Muhammad Irfan for bug # EMR-153 */
            $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco,#listAlcohol,#listDrugAbuse,#listSexualHx').removeClass('successLight');
            /* End 07/01/2016 Muhammad Irfan for bug # EMR-153 */

        }
        else {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #btnSocialHxSave').hide();
        }
        var self = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').find('div#Tobacco,div#Alcohol,div#DrugAbuse,div#SexualHx,div#Miscellaneous').each(function () {
            if (isRemarkable == true) {
                if ($(this).hasClass("disableAll") == false) {
                    $(this).addClass("disableAll");

                }
                $(this).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
                    $(this).attr("disabled", "disabled");
                    Clinical_SocialHx.resetControlValue(this);
                });
                //Clinical_SocialHx.resetControlValue(this);
            }
            else {
                $(this).removeClass("disableAll");
                /* Start 23/12/2015 Muhammad Irfan for bug # EMR-153 */
                $('#' + Clinical_SocialHx.params.PanelID + ' #ulSocialHxTabsItems').removeClass('disableAll');
                /* End 23/12/2015 Muhammad Irfan for bug # EMR-153 */

                $(this).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select,ul').each(function () {
                    $(this).removeAttr("disabled");
                    Clinical_SocialHx.resetControlValue(this);
                });
                /* Start 07/01/2016 Muhammad Irfan for bug # EMR-153 */
                //if (Clinical_SocialHx.bAlcoholExist == true)
                //    $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').addClass('successLight');
                //if (Clinical_SocialHx.bTobaccoExist == true)
                //    $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').addClass('successLight');
                //if (Clinical_SocialHx.bDrugExist == true)
                //    $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                //if (Clinical_SocialHx.bSexualExist == true)
                //    $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').addClass('successLight');



                //$('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco,#listAlcohol,#listDrugAbuse,#listSexualHx').removeClass('active');
                /* End 07/01/2016 Muhammad Irfan for bug # EMR-153 */
            }
        });
        if (!isRemarkable) {
            var activeTabId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx ul#ulSocialHxTabsItems li.active").attr("id")
            Clinical_SocialHx.isRemarkableFormload = false;
            $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #" + activeTabId + " a").trigger("click");
        }
        else {
            Clinical_SocialHx.isRemarkableFormload = true;
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

        //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());

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
            var self = $('#' + Clinical_SocialHx.params.PanelID + ' ' + sectionDetails).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                //Start 10-05-2016 Edit by Humaira Yousaf Bug# EMR-987
                var activeStatus = $.trim($('#' + Clinical_SocialHx.params.PanelID + ' #ulSmokingStatus').find('li.active > a').text());
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
        var objSTD = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD');
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
                Clinical_SocialHx.resetControlValue(objSTD);
            }
            else {
                objSTD.attr("disabled", "disabled");
                objSTD.find("option:selected").removeAttr("selected");
                objSTD.multiselect("disable");
                objSTD.multiselect("clearSelection");
                Clinical_SocialHx.resetControlValue(objSTD);
                $(obj).val("");

                $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').val("");
                $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').find("option:selected").removeAttr("selected");
                $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').multiselect("disable");
                $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').multiselect("clearSelection");
                $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').multiselect("refresh");
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
        var objCessation = $('#' + Clinical_SocialHx.params.PanelID).find(CessationControls).each(function () {
            if (obj != null) {

                if ($(obj).prop("checked") == true) {
                    $(this).attr("disabled", "disabled");
                }
                else {
                    $(this).removeAttr("disabled");
                }
                //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                Clinical_SocialHx.resetControlValue(this);
                //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
            }
            else {
                $(this).removeAttr("disabled");
            }
        });
    },

    enableCessationValidation: function () {



        // var stayLength = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #txtDrugCessationLength').val();
        var ddlVal = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ddlDrugCessationPeriod').val();
        //if (stayLength != null && stayLength != '') {
        //    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('DrugCessationPeriod', true);
        //    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation<span class="required">*</span>');
        //}
        //else {
        //    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('DrugCessationPeriod', false);
        //    $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation');
        //}
        if (ddlVal != null && ddlVal != '') {
            // $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('DrugCessationLength', true);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation<span class="required">*</span>');

        } else {
            // $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('DrugCessationLength', false);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation');
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
        var smokingStatusText = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus li.active").text().toLowerCase().trim();
        //End  01-03-2016 Farooq Ahmad  this vaiable will store the selected smoking status
        */

        /* if (smokingStatusText == 'current every day smoker' ||smokingStatusText == 'light tobacco smoker') {
             return false;
         }*/

        if ($(obj).attr('name') == "TobaccoRecentlyQuit") {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationPeriod', false);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration');

        }

        if ($(obj).attr('name') == "DrugRecentlyQuit") {
            if ($(obj).is(":checked")) {
                $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDrugCessationLength').html('Length of Cessation');
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
            Clinical_SocialHx.enableDisableCessationControls(obj, TabType);
        }

    },
    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will Enable Disable controls for Tobacco on basis of selected Status as specified by currentStatus

    enableDisableTobaccoControls: function (obj, currentStatus, currentStatusValue) {

        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SocialHx.socialHxJSON != '') {
            Clinical_SocialHx.cacheTobaccoTabData();
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
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationPeriod', false);
        $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration');

        if (currentStatus != null && currentStatus != "") {

            var statusId = $(obj).attr('id');
            //var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Tobacco', statusId);

            //if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && cachedJSON != '') {
            //    detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
            //    utility.bindMyJSONByName(true, cachedJSON, false, detailSection);
            //}
            //else
            {
                if (currentStatus == "Current every day smoker" || currentStatus == 'Current some day smoker' || currentStatus == 'Heavy tobacco smoker' || currentStatus == 'Light tobacco smoker') {
                    var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {

                        if ($(this).attr("id") == "txtTobaccoCessationLength" || $(this).attr("id") == "ddlTobaccoCessationPeriod" || $(this).attr("id") == "chkTobaccoRecentlyQuit") {
                            $(this).attr("disabled", "disabled");
                            if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                                Clinical_SocialHx.resetControlValue(this);
                            }
                        }
                        else {
                            $(this).removeAttr("disabled");
                        }

                        if (currentStatus == "Current every day smoker") {
                            Clinical_SocialHx.resetControlValue(this);
                        }
                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                            Clinical_SocialHx.resetControlValue(this);
                            //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                        }

                        //End 14-01-2016 syed zia ,for same status click refresh problem
                    });
                }
                else if (currentStatus == 'Former smoker') {
                    var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                        if ($(this).attr("id") == "ddlTobaccoCounsellingPeriod" || $(this).attr("id") == "ddlTobaccoCounsellingTopic" || $(this).attr("id") == "chkTobaccoWouldQuit") {
                            $(this).attr("disabled", "disabled");
                            if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                                Clinical_SocialHx.resetControlValue(this);
                            }
                        }
                        else {
                            $(this).removeAttr("disabled");
                        }

                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            // Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                            Clinical_SocialHx.resetControlValue(this);
                            //  End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                        }
                        //End 14-01-2016 syed zia ,for same status click refresh problem

                    });
                }
                else if (currentStatus == 'Never smoker' || currentStatus == 'Unknown if ever smoked' || currentStatus == 'Does not chew tobacco' || currentStatus == 'Non Smoker') {
                    var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                        if ($(this).attr("id") != "txtTobaccoComments") {
                            $(this).attr("disabled", "disabled");
                            if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                                Clinical_SocialHx.resetControlValue(this);
                            }
                        }
                        else {
                            $(this).removeAttr("disabled");
                        }

                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                            Clinical_SocialHx.resetControlValue(this);
                            //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                        }
                        //End 14-01-2016 syed zia ,for same status click refresh problem
                    });
                }
                else if (currentStatus == 'Smoker, current status unknown') {
                    var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                        $(this).removeAttr("disabled");
                        //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                        Clinical_SocialHx.resetControlValue(this);
                        //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            // Start 16-12-2015 Muhammad Arshad we dont need to reset values while this status is selected
                            Clinical_SocialHx.resetControlValue(this);
                            // End 16-12-2015 Muhammad Arshad we dont need to reset values while this status is selected
                        }
                        //End 14-01-2016 syed zia ,for same status click refresh problem
                    });
                }
                else if (currentStatus == 'Chews tobacco') {
                    var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                        if ($(this).attr("id") == "txtTobaccoCessationLength" || $(this).attr("id") == "ddlTobaccoType" || $(this).attr("id") == "ddlTobaccoCessationPeriod" || $(this).attr("id") == 'chkTobaccoRecentlyQuit') {
                            $(this).attr("disabled", "disabled");
                            if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                                Clinical_SocialHx.resetControlValue(this);
                            }
                        }
                        else {
                            $(this).removeAttr("disabled");
                        }

                        //Begin 14-01-2016 syed zia ,for same status click refresh problem
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            //    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                            Clinical_SocialHx.resetControlValue(this);
                            //    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                        }
                        //End 14-01-2016 syed zia ,for same status click refresh problem

                    });
                }
            }
            $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionTobacco #hfTobaccoDataChangeBit').val("1");
            $.when(Clinical_SocialHx.fillDetails()).then(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
                    detailSection.resetAllControls(null);
                }

                //if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SocialHx.socialHxJSON != '') {
                //    var updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                //    if (Clinical_SocialHx.socialHxJSON != updatedJSON) {
                //        Clinical_SocialHx.cacheSocialHxJSON('Tobacco', statusIdToCache, updatedJSON);
                //    }
                //}


                //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());
            });

        }

        Clinical_SocialHx.enableDisableCounsellingTopic('tobacco');
    },

    cacheTobaccoTabData: function (socialType) {
        var dfd = $.Deferred();
        var updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
        var statusId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulSmokingStatus > li.active").attr('id');

        if (Clinical_SocialHx.socialHxJSON != updatedJSON) {
            if (statusId != null) {
                $.when(Clinical_SocialHx.cacheSocialHxJSON('Tobacco', statusId, updatedJSON)).then(function () {
                    dfd.resolve();
                });
                Clinical_SocialHx.SetIsLast('Tobacco', statusId);
            }
            else {
                dfd.resolve();
            }
        }
        else {
            if (socialType == 'tobacco') {
                var json = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionTobacco").getMyJSONByName();
                var detailsSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionTobacco").clone();
                $(detailsSection).resetAllControls(null);
                var emptyJson = $(detailsSection).getMyJSONByName();

                if (statusId != null && statusId > 0 && json == emptyJson) {
                    $.when(Clinical_SocialHx.cacheSocialHxJSON('Tobacco', statusId, updatedJSON)).then(function () {
                        dfd.resolve();
                    });
                    Clinical_SocialHx.SetIsLast('Tobacco', statusId);
                }
                else {

                    var date = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
                    var unremarkable = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
                    var comments = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();

                    var dataChanged = Clinical_SocialHx.date != date || Clinical_SocialHx.unremarkable != unremarkable || Clinical_SocialHx.overallComments != comments;
                    if (dataChanged) {
                        $.when(Clinical_SocialHx.cacheSocialHxJSON('', statusId, updatedJSON, null, true)).then(function () {
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
        var SocialHxType = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
        var isPopulated = false;
        Clinical_SocialHx.fillSocialHx(SocialHxType).then(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    ////Begin 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                    //if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    //    /* Start 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                    //    $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").addClass("disableAll");
                    //    /* End 08/12/2015 Muhammad Irfan To disable date control if mode is edit */
                    //}
                    //End 12-28-2015 Muhammad Arshad Bug# EMR-161 Social History Clinical Module -> Date
                    if (typeof (response.SocialHxFill_JSON) != "undefined") {
                        var socialhx_detail = JSON.parse(response.SocialHxFill_JSON);
                        // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                        if (Clinical_SocialHx.isRemarkableFormload == false) {
                            socialhx_detail.SocialHxUnremarkable = "false";
                        }
                        // Start 13/01/2016 syed zia, for remarkable checked/ unchecked
                        var self = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx");
                        utility.bindMyJSONByName(true, socialhx_detail, false, self).done(function () {

                            Clinical_SocialHx.params.mode = "Edit";
                        });
                    }
                    if (SocialHxType == 'tobacco') {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionTobacco");
                        var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSmokingStatus li.active");
                        var statusId = $activeLi.attr('id');
                        if (typeof response.TobaccoHxFill_JSON != "undefined") {
                            var tobaccoDetails = JSON.parse(response.TobaccoHxFill_JSON)[0];
                            if (tobaccoDetails != null && statusId == tobaccoDetails.SmokingStatus) {
                                isPopulated = true;
                                var detailsJSON = '';
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Tobacco', statusId);
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
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Tobacco', statusId);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            return dfd.resolve(isPopulated);
                                        });
                                    }
                                }
                            }
                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();

                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Tobacco', statusId);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {

                                    });
                                }
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Tobacco").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'alcohol') {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol");
                        var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Alcohol #ulAlcoholStatus li.active");
                        var statusId = $activeLi.attr('id');
                        if (typeof response.AlcoholHxFill_JSON != "undefined") {
                            var alcoholDetails = JSON.parse(response.AlcoholHxFill_JSON)[0];

                            if (alcoholDetails != null && statusId == alcoholDetails.AlcoholStatus) {

                                var detailsJSON = '';
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Alcohol', statusId);
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
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Alcohol', statusId);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Alcohol', statusId);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }

                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
                            }
                        }
                    }

                    else if (SocialHxType == 'drug') {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse");
                        var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#DrugAbuse #ulDrugStatus li.active");
                        var statusId = $activeLi.attr('id');
                        if (typeof response.DrugAbuseFill_JSON != "undefined") {
                            var drugDetails = JSON.parse(response.DrugAbuseFill_JSON)[0];

                            if (drugDetails != null && statusId == drugDetails.DrugStatus) {
                                var detailsJSON = '';
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('DrugAbuse', statusId);
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
                                    $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val(drugDetails.DrugType.split(','));
                                    $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                    $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect({
                                        enableFiltering: true,
                                        enableCaseInsensitiveFiltering: true,
                                    });
                                });
                            }
                            else {
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('DrugAbuse', statusId);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                            if (drugDetails != null) {
                                                $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val(drugDetails.DrugType.split(','));
                                            }
                                            $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                            $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect({
                                                enableFiltering: true,
                                                enableCaseInsensitiveFiltering: true,
                                            });
                                        });
                                    }
                                }
                            }
                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('DrugAbuse', statusId);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {

                                        if (cachedJSON != '') {
                                            $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val(cachedJSON.DrugType.split(','));
                                        }
                                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect({
                                            enableFiltering: true,
                                            enableCaseInsensitiveFiltering: true,
                                        });

                                    });
                                }
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'sexual') {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx");
                        var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus li.active");
                        var statusId = $activeLi.attr('id');
                        if (typeof response.SexualHxFill_JSON != "undefined") {
                            var sexualDetails = JSON.parse(response.SexualHxFill_JSON)[0];

                            if (sexualDetails != null && statusId == sexualDetails.SexualStatus) {

                                var detailsJSON = '';
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Sexual', statusId);
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
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Sexual', statusId);
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
                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON('Sexual', statusId);
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

                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_occupation') {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails");
                        var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var statusId = $activeLi.attr('id');
                        var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                        if (typeof response.OccupationHxFill_JSON != "undefined") {
                            var occupationDetails = JSON.parse(response.OccupationHxFill_JSON)[0];
                            if (occupationDetails != null && statusId == occupationDetails.MiscChildStatus) {

                                var detailsJSON = '';
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
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
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails").getMyJSONByName();
                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #OccupationDetails").getMyJSONByName();

                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_sleep') {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails");
                        var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var statusId = $activeLi.attr('id');
                        var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');

                        if (typeof response.SleepHxFill_JSON != "undefined") {
                            var sleepDetails = JSON.parse(response.SleepHxFill_JSON)[0];

                            if (sleepDetails != null && statusId == sleepDetails.MiscChildStatus) {

                                var detailsJSON = '';
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
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
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails").getMyJSONByName();
                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #SleepDetails").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_exercises') {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails");
                        var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var statusId = $activeLi.attr('id');
                        var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');

                        if (typeof response.ExercisesHxFill_JSON != "undefined") {
                            var exerciseDetails = JSON.parse(response.ExercisesHxFill_JSON)[0];
                            if (exerciseDetails != null && statusId == exerciseDetails.MiscChildStatus) {
                                var detailsJSON = '';
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
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
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails").getMyJSONByName();
                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #ExercisesDetails").getMyJSONByName();

                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_housing') {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails");
                        var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var statusId = $activeLi.attr('id');
                        var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');

                        if (typeof response.HousingHxFill_JSON != "undefined") {
                            var housingDetails = JSON.parse(response.HousingHxFill_JSON)[0];
                            if (housingDetails != null && statusId == housingDetails.MiscChildStatus) {
                                var detailsJSON = '';
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
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
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != '') {
                                        utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        });
                                    }
                                }
                            }
                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails").getMyJSONByName();
                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                    });
                                }
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #HousingDetails").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_caffeineintake') {
                        var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails");
                        var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Miscellaneous #ulMiscChildStatus li.active");
                        var socialTypeMiscId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulMiscStatus > li.active").attr('id');
                        var statusId = $activeLi.attr('id');

                        if (typeof response.CaffeineIntakeHxFill_JSON != "undefined") {
                            var caffeineDetails = JSON.parse(response.CaffeineIntakeHxFill_JSON)[0];

                            if (caffeineDetails != null && statusId == caffeineDetails.MiscChildStatus) {

                                var detailsJSON = '';
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
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
                                    $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                }
                                else if (caffeineDetails.CaffeineHarmful.toLowerCase() == "false") {
                                    $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                                }
                                else {
                                    $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
                                }
                            }
                            else {
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                    var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                    if (cachedJSON != []) {
                                        utility.bindMyJSONByName(true, detailsJSON, false, detailSection).done(function () {
                                        });

                                        if (caffeineDetails.CaffeineHarmful.toLowerCase() == "true") {
                                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                        }
                                        else if (caffeineDetails.CaffeineHarmful.toLowerCase() == "false") {
                                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                                        }
                                        else {
                                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
                                        }
                                    }
                                }
                            }
                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails").getMyJSONByName();
                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_SocialHx.getCacheSocialHxJSON(socialTypeMiscId, statusId, SocialHxType);
                                if (cachedJSON != '') {
                                    utility.bindMyJSONByName(true, cachedJSON, false, detailSection).done(function () {
                                        if (cachedJSON.RadCaffieneharmful == true) {
                                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", true);
                                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", false);
                                        }
                                        else if (cachedJSON.RadCaffieneharmful == false) {
                                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful").prop("checked", true);
                                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadYesCaffeineharmful").prop("checked", false);
                                        }
                                        else {
                                            $('#' + Clinical_SocialHx.params.PanelID + " section#sectionMiscDetails #RadNoCaffeineharmful,#RadYesCaffeineharmful").prop("checked", false);
                                        }
                                    });
                                }
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #CaffeineIntakDetails").getMyJSONByName();
                            }
                        }
                    }
                    else if (SocialHxType == 'miscellaneous_travel') {
                        if (typeof response.TravelHxFill_JSON != "undefined" && response.TravelHxFill_JSON != "[]") {
                            var TravelDetails = JSON.parse(response.TravelHxFill_JSON)[0];

                            Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx").getMyJSONByName();
                        }
                        else {
                            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote")
                                Clinical_SocialHx.socialHxJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails #divTravelDetailsHx").getMyJSONByName();
                        }
                    }

                    /* else if (SocialHxType == 'miscellaneous_occupation') {

                         if (typeof response.SexualHxFill_JSON != "undefined") {
                             var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionMiscDetails");
                             var sexualDetails = JSON.parse(response.SexualHxFill_JSON)[0];
                             var $activeLi = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx div#Tobacco #ulSexualStatus li.active");
                             var statusId = $activeLi.attr('id');
                             if (statusId == sexualDetails.DrugStatus) {

                                 utility.bindMyJSONByName(true, sexualDetails, false, detailSection).done(function () {

                                 });
                             }
                         }
                     }*/
                }
            }

            //$('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());


            return dfd.resolve(isPopulated);
        });

        //return dfd.resolve(isPopulated);
    },

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will Enable Disable controls for Alcohol on basis of selected Status as specified by currentStatus

    enableDisableAlcoholControls: function (obj, currentStatus, currentStatusValue) {

        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SocialHx.socialHxJSON != '') {
            Clinical_SocialHx.cacheAlcoholTabData();
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
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "txtAlcoholCessationLength" || $(this).attr("id") == "ddlAlcoholCessationPeriod" || $(this).attr("id") == "chkAlcoholRecentlyQuit") {
                        $(this).attr("disabled", "disabled");
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            Clinical_SocialHx.resetControlValue(this);
                        }
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                        Clinical_SocialHx.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            else if (currentStatus == 'Has history of Alcoholism') {
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "ddlAlcoholCounsellingPeriod" || $(this).attr("id") == "ddlAlcoholCounsellingTopic" || $(this).attr("id") == "chkAlcoholWouldQuit") {
                        $(this).attr("disabled", "disabled");
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            Clinical_SocialHx.resetControlValue(this);
                        }
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                        Clinical_SocialHx.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            else if (currentStatus == 'Does not drink' || currentStatus == 'Drinking status unknown' || currentStatus == 'Denies Usage') {
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") != "txtAlcoholComments") {
                        $(this).attr("disabled", "disabled");
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            Clinical_SocialHx.resetControlValue(this);
                        }
                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                        Clinical_SocialHx.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            else if (currentStatus == 'Occasional drinker' || currentStatus == 'Social drinker') {
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    $(this).removeAttr("disabled");
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                        Clinical_SocialHx.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    // Start 16-12-2015 Muhammad Arshad we dont need to reset values while this status is selected
                    //Clinical_SocialHx.resetControlValue(this);
                    // End 16-12-2015 Muhammad Arshad we dont need to reset values while this status is selected
                });
            }
            //else if (currentStatus == 'Chews Tobacco') {
            //    var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionAlcohol [type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
            //        if ($(this).attr("id") == "txtTobaccoCessationLength" || $(this).attr("id") == "ddlTobaccoType") {
            //            $(this).attr("disabled", "disabled");
            //        }
            //        else {
            //            $(this).removeAttr("disabled");
            //        }
            //    });
            //}
            $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionAlcohol #hfAlcoholDataChangeBit').val("1");
            $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol");
                    detailSection.resetAllControls(null);
                }

            })
        }

        Clinical_SocialHx.enableDisableCounsellingTopic('alcohol');

    },

    cacheAlcoholTabData: function (socialType) {
        var dfd = $.Deferred();
        var updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Alcohol").getMyJSONByName();
        var statusId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulAlcoholStatus > li.active").attr('id');

        if (Clinical_SocialHx.socialHxJSON != updatedJSON) {
            if (statusId != null) {
                $.when(Clinical_SocialHx.cacheSocialHxJSON('Alcohol', statusId, updatedJSON)).then(function () {
                    dfd.resolve();
                });
                Clinical_SocialHx.SetIsLast('Alcohol', statusId);
            }
            else {
                dfd.resolve();
            }
        }
        else {
            if (socialType == 'alcohol') {
                var json = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol").getMyJSONByName();
                var detailsSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionAlcohol").clone();
                $(detailsSection).resetAllControls(null);
                var emptyJson = $(detailsSection).getMyJSONByName();

                if (statusId != null && statusId > 0 && json == emptyJson) {
                    $.when(Clinical_SocialHx.cacheSocialHxJSON('Alcohol', statusId, updatedJSON)).then(function () {
                        dfd.resolve();
                    });
                    Clinical_SocialHx.SetIsLast('Alcohol', statusId);
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

        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SocialHx.socialHxJSON != '') {
            Clinical_SocialHx.cacheDrugTabData();
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
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionDrugAbuse').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "txtDrugCessationLength" || $(this).attr("id") == "ddlDrugCessationPeriod" || $(this).attr("id") == "chkDrugRecentlyQuit") {
                        $(this).attr("disabled", "disabled");
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            Clinical_SocialHx.resetControlValue(this);

                        }

                    }
                    else if ($(this).attr("id") == "ddlDrugType") {
                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val("");
                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                    }
                    else {
                        $(this).removeAttr("disabled");

                        $('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("enable");
                    }
                    //if ($(this).attr("id") == "ddlDrugType") {
                    //    Clinical_SocialHx.enableDisableSTDControls(this);
                    //}

                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                        Clinical_SocialHx.resetControlValue(this);
                        //$('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem
                    //$('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                });
            }
            else if (currentStatus == 'Former drug user') {
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionDrugAbuse').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "chkDrugWouldQuit") {
                        $(this).attr("disabled", "disabled");
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            Clinical_SocialHx.resetControlValue(this);
                        }

                    }
                    else if ($(this).attr("id") == "ddlDrugType") {
                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val("");
                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                    }
                    else {
                        $(this).removeAttr("disabled");

                        $('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("enable");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                        Clinical_SocialHx.resetControlValue(this);
                        //$('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem
                    //$('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            else if (currentStatus == 'Never used drugs' || currentStatus == 'Unknown if ever used drugs' || currentStatus == 'Denies Usage') {
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionDrugAbuse').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") != "txtDrugComments") {
                        $(this).attr("disabled", "disabled");
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            Clinical_SocialHx.resetControlValue(this);
                            // $('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");

                        }
                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val("");
                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                        $('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("disable");
                        //$('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    }
                    else if ($(this).attr("id") == "ddlDrugType") {
                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val("");
                        $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                    }
                    else {

                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                        Clinical_SocialHx.resetControlValue(this);
                        //$('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    //$('#' + Clinical_SocialHx.params.PanelID + " section#sectionDrugAbuse #ddlDrugType").multiselect("clearSelection");
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }
            $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionDrugAbuse #hfDrugAbuseDataChangeBit').val("1");
            $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse");
                    detailSection.resetAllControls(null);
                    $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").val("");
                    $('#' + Clinical_SocialHx.params.PanelID + " #ddlDrugType").multiselect("refresh");
                }
            })
        }
    },
    cacheDrugTabData: function (socialType) {
        var dfd = $.Deferred();
        var updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #DrugAbuse").getMyJSONByName();
        var statusId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulDrugStatus > li.active").attr('id');

        if (Clinical_SocialHx.socialHxJSON != updatedJSON) {
            if (statusId != null) {
                $.when(Clinical_SocialHx.cacheSocialHxJSON('DrugAbuse', statusId, updatedJSON)).then(function () {
                    dfd.resolve();
                });
                Clinical_SocialHx.SetIsLast('DrugAbuse', statusId);
            }
            else {
                dfd.resolve();
            }
        }
        else {
            if (socialType == 'drug' || socialType == 'drug abuse') {
                var json = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse").getMyJSONByName();
                var detailsSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionDrugAbuse").clone();
                $(detailsSection).resetAllControls(null);
                $(detailsSection).find("#ddlDrugType").val("");
                $(detailsSection).find("#ddlDrugType").multiselect("refresh");
                var emptyJson = $(detailsSection).getMyJSONByName();

                if (statusId != null && statusId > 0 && json == emptyJson) {
                    $.when(Clinical_SocialHx.cacheSocialHxJSON('DrugAbuse', statusId, updatedJSON)).then(function () {
                        dfd.resolve();
                    });
                    Clinical_SocialHx.SetIsLast('DrugAbuse', statusId);
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

        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_SocialHx.socialHxJSON != '') {
            Clinical_SocialHx.cacheSexualTabData();
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
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    if ($(this).attr("id") == "ddlSexualExposedToSTD" || $(this).attr("id") == "ddlSexualSTD" || $(this).attr("id") == "RadSexualYesAbusedSexually" || $(this).attr("id") == "RadSexualNoAbusedSexually") {
                        $(this).attr("disabled", "disabled");
                        if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                            Clinical_SocialHx.resetControlValue(this);
                        }


                    }
                    else {
                        $(this).removeAttr("disabled");
                    }
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                        Clinical_SocialHx.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    if ($(this).attr("id") == "ddlSexualExposedToSTD") {
                        Clinical_SocialHx.enableDisableSTDControls(this);
                    }
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status

                });
            }
            else if (currentStatus == 'Sexually Active' || currentStatus == 'Not Sexually Active') {
                var self = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                    $(this).removeAttr("disabled");
                    //Begin 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                    //Begin 14-01-2016 syed zia ,for same status click refresh problem
                    if (Clinical_SocialHx.SeletedChildStatus != currentStatusValue) {
                        Clinical_SocialHx.resetControlValue(this);
                    }
                    //End 14-01-2016 syed zia ,for same status click refresh problem

                    if ($(this).attr("id") == "ddlSexualExposedToSTD") {
                        Clinical_SocialHx.enableDisableSTDControls(this);
                    }
                    //End 28-12-2015 Muhammad Arshad Bug# EMR-157 Social History Clinical Module -> Fields should be blank when select other status
                });
            }

            $.when(Clinical_SocialHx.fillDetails()).done(function (isPopulated) {
                if (!isPopulated) {
                    var detailSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx");
                    detailSection.resetAllControls(null);
                    $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualSTD').val('');
                    $('#' + Clinical_SocialHx.params.PanelID + " section#sectionSexualHx #ddlSexualSTD").multiselect("refresh");
                }
            })
        }
    },

    cacheSexualTabData: function (socialType) {
        var dfd = $.Deferred();
        var updatedJSON = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #SexualHx").getMyJSONByName();
        var statusId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ulSexualStatus > li.active").attr('id');

        if (Clinical_SocialHx.socialHxJSON != updatedJSON) {
            if (statusId != null) {
                $.when(Clinical_SocialHx.cacheSocialHxJSON('Sexual', statusId, updatedJSON)).then(function () {
                    dfd.resolve();
                });
                Clinical_SocialHx.SetIsLast('Sexual', statusId);
            }
            else {
                dfd.resolve();
            }
        }
        else {
            if (socialType == 'sexual' || socialType == 'sexual hx') {
                var json = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx").getMyJSONByName();
                var detailsSection = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #sectionSexualHx").clone();
                $(detailsSection).resetAllControls(null);
                $(detailsSection).find("#ddlSexualSTD").val("");
                $(detailsSection).find("#ddlSexualSTD").multiselect("refresh");
                var emptyJson = $(detailsSection).getMyJSONByName();

                if (statusId != null && statusId > 0 && json == emptyJson) {
                    $.when(Clinical_SocialHx.cacheSocialHxJSON('Sexual', statusId, updatedJSON)).then(function () {
                        dfd.resolve();
                    });
                    Clinical_SocialHx.SetIsLast('Sexual', statusId);
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

    //Author: Muhammad Arshad
    //Date: 12-03-2015
    //This function will handle Add/Edit of SocialHx and it's childs (Tobacco,Alcohol,DrugAbuse,SexualHx,Miscellaneous), it expects SocialHxType to be Add/Edit
    socialHxSave: function (SocialHxType, UnloadSocialhx, attachToNote) {

        var SocialHxId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() != "" ? $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val() : "-1";
        if (parseInt(SocialHxId) > 0) {
            Clinical_SocialHx.params.mode = "Edit";
        }
        else {
            Clinical_SocialHx.params.mode = "Add";
        }
        //Start 12-07-2016 Edit By Humaira Yousaf for Bug# EMR-1422
        //Start//11/02/2016//Abid Ali// fixed bug#315
        //var overallComments = "";
        //overallComments = $('#' + Clinical_SocialHx.params.PanelID + " #txtSocialComments").val();
        //overallComments = typeof overallComments == "undefined" ? "" : overallComments
        //if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && overallComments != "") {

        //    var DetailExists = true;
        //}
        //else {
        //    DetailExists = Clinical_SocialHx.isDetailExists(SocialHxType.toLowerCase());
        //}
        //if ($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").is(':checked')) {
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
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#Tobacco");
            }
            else if (SocialHxType.toLowerCase() == "alcohol") {
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#Alcohol");
            }
            else if (SocialHxType.toLowerCase() == "drug") {
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse");
            }
            else if (SocialHxType.toLowerCase() == "sexual") {
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx");
            }
            else if (SocialHxType.toLowerCase().indexOf("miscellaneous") > -1) {
                Clinical_SocialHx.retainedComponentMisHx = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #MiscHxMainStatus #ulMiscStatus li.active").attr("id");
                if (SocialHxType.toLowerCase() == "miscellaneous_occupation") {
                    self = $('#' + Clinical_SocialHx.params.PanelID + " div#OccupationDetails");
                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_sleep") {
                    self = $('#' + Clinical_SocialHx.params.PanelID + " div#SleepDetails");
                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_exercises") {
                    self = $('#' + Clinical_SocialHx.params.PanelID + " div#ExercisesDetails");

                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_housing") {
                    self = $('#' + Clinical_SocialHx.params.PanelID + " div#HousingDetails");
                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_caffeineintake") {
                    self = $('#' + Clinical_SocialHx.params.PanelID + " div#CaffeineIntakDetails");
                }
                else if (SocialHxType.toLowerCase() == "miscellaneous_travel") {
                    self = $('#' + Clinical_SocialHx.params.PanelID + " div#divTravelDetailsHx");
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
                objData["MiscChildStatus"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id");
                objData["MiscChildStatusText"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text();
                if (SocialHxType.toLowerCase() == "miscellaneous_exercises") {
                    objData["ExercisesTypeText"] = $('#' + Clinical_SocialHx.params.PanelID + " div#ExercisesDetails").find('#ddlExercisesType option:selected').text();
                    objData["ExercisesDietText"] = $('#' + Clinical_SocialHx.params.PanelID + " div#ExercisesDetails").find('#ddlExercisesDiet option:selected').text();
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
            objData["SocialHxId"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val();
            objData["SocialHxType"] = SocialHxType != null ? SocialHxType : "";
            objData["SocialHxDate"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
            objData["SocialHxUnremarkable"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
            objData["SocialComments"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();
            myJSON = JSON.stringify(objData);
            //Begin 15-07-2016 Edit By Humaira Yousaf Bug#1582
            if (SocialHxType.toLowerCase() == "tobacco" || SocialHxType.toLowerCase() == "alcohol" || SocialHxType.toLowerCase() == "drug") {
                var msg = Clinical_SocialHx.isCessationValid(SocialHxType.toLowerCase());
                if (msg != "") {
                    Clinical_SocialHx.isSaved = false;
                    utility.DisplayMessages(msg, 3);
                    return;
                }
            }
            //End 15-07-2016 Edit By Humaira Yousaf Bug#1582
            //return false;
            if (Clinical_SocialHx.params.mode == "Add") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Social Hx", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_SocialHx.saveSocialHx(myJSON).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                Clinical_SocialHx.params.HxTypeId = response.SocialHxId;

                                $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #divSwitch").removeClass('disableAll');
                                if (!attachToNote) {
                                    Clinical_SocialHx.ChangeCurrentPast(1, null, null, null);
                                    Clinical_SocialHx.BindCurrentSocialHxSoapText(response,false);
                                }
                                LastSocialHx = new Object();
                                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                                LastSocialHx["SocialHxType"] = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');

                                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(response.SocialHxId);
                                Clinical_SocialHx.params.mode = "Edit";
                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && UnloadSocialhx == true) {
                                    if (!attachToNote) {
                                        Clinical_SocialHx.getSocialHxInfo(SocialHxType, UnloadSocialhx);
                                    }
                                } else {
                                    utility.DisplayMessages(response.message, 1);
                                }
                                //Start//17/12/2015//Ahmad Raza//Serializing the form
                                $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());
                                //End//17/12/2015//Ahmad Raza//Serializing the form
                                Clinical_SocialHx.BindCurrentSocialHxSoapText(response,true);
                                Clinical_SocialHx.isSaved = true;
                            }
                            else {
                                Clinical_SocialHx.isSaved = false;
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                });
                //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
            }
            else if (Clinical_SocialHx.params.mode == "Edit") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("History_Social Hx", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_SocialHx.updateSocialHx(myJSON, Clinical_SocialHx.params.SocialHxId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #divSwitch").removeClass('disableAll');
                                if (!attachToNote) {
                                    Clinical_SocialHx.ChangeCurrentPast(1, null, null, null);
                                    Clinical_SocialHx.BindCurrentSocialHxSoapText(response,false);
                                }
                                LastSocialHx = new Object();
                                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                                LastSocialHx["SocialHxType"] = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');

                                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && UnloadSocialhx == true) {
                                    if (!attachToNote) {
                                        Clinical_SocialHx.getSocialHxInfo(SocialHxType, UnloadSocialhx);
                                    }
                                } else {
                                    //Clinical_SocialHx.AppointmentStatusSearch(Clinical_SocialHx.params.SocialHxignsId);
                                    utility.DisplayMessages(response.message, 1);
                                }

                                //Start//17/12/2015//Ahmad Raza//Serializing the form
                                $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('serialize', $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').serialize());
                                //End//17/12/2015//Ahmad Raza//Serializing the form

                                Clinical_SocialHx.isSaved = true;
                                Clinical_SocialHx.BindCurrentSocialHxSoapText(response,true);
                            }
                            else {
                                Clinical_SocialHx.isSaved = false;
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
        if (Clinical_SocialHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_SocialHx.params.patientID;
        }
        objData["commandType"] = "SAVE_SocialHx";
        //Start//18/01/2016//Ahmad Raza//checkbox's value handled
        if ($('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #RadYesCaffeineharmful').is(':checked')) {
            objData["RadCaffieneharmful"] = "True";
        }
        else if ($('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #RadNoCaffeineharmful').is(':checked')) {
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
        if (Clinical_SocialHx.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_SocialHx.params.patientID;
        }

        objData["commandType"] = "UPDATE_SOCIALHx";
        //Start//18/01/2016//Ahmad Raza//checkbox's value handled
        if ($('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #RadYesCaffeineharmful').is(':checked')) {
            objData["RadCaffieneharmful"] = "True";
        }
        else if ($('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #RadNoCaffeineharmful').is(':checked')) {
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
        Clinical_SocialHx.bIsFirstLoad = true;
        //Code for progress note navigation
        // Date: 12-11-2015
        // Change Author: Muhammad Azhar Shahzad
        // If User OPen Social History From Progress Note
        /*Clicking close “X” button, a prompt message will be displayed
            “Are you want to save the changes?
            The date will be modified with current date.”
            i.	Clicking yes from the prompt will update the date as well as add the social history component on the progress notes.
            ii.	Clicking No will close the social history popup and will not add social history component on Progress notes.
        */
        //Start//15-02-2016//Ahmad Raza//fixed Bug#331
        var socialHxType = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
        var detailExists = Clinical_SocialHx.isDetailExists(socialHxType.toLowerCase());
        if (bFromNote == true) {

        }
        else {
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {
                Clinical_SocialHx.controlToInvoke = controlToInvoke;
                if (EMRUtility.compareFormDataWithSerialized(Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx')) {
                    utility.myConfirmNote('1', function () {
                        // var socialHxType = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
                        if (socialHxType == "" || socialHxType == "undefined") {
                            Clinical_SocialHx.UnloadSocialHistory(NextOrPre);
                            if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSocialHx") {
                                UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                                Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSocialHx', 'SocialHx');
                            }
                        }
                        else {
                            Clinical_SocialHx.bNextPrev = true;
                            Clinical_SocialHx.socialHxSave(socialHxType, true);
                        }
                    }, function () {
                        var surgicalHxType = $('#' + Clinical_SocialHx.params.PanelID + " #hfSocialHxType").val();
                        if (surgicalHxType == "" || surgicalHxType == "undefined") {
                            Clinical_SocialHx.UnloadSocialHistory(NextOrPre);
                            if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSocialHx") {
                                UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                                Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSocialHx', 'SocialHx');
                            }
                        }
                        else {
                            Clinical_SocialHx.bNextPrev = true;
                            Clinical_SocialHx.socialHxSave(socialHxType, true, true);
                        }
                        Clinical_SocialHx.UnloadSocialHistory();
                        if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSocialHx") {
                            UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                            Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSocialHx', 'SocialHx');
                        }
                    }, function () {
                        Clinical_SocialHx.UnloadSocialHistory(NextOrPre);
                        if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSocialHx") {
                            UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                            Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSocialHx', 'SocialHx');
                        }
                    });
                    //End//15-02-2016//Ahmad Raza//fixed Bug#331
                } else {

                    Clinical_SocialHx.UnloadSocialHistory();
                    if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSocialHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSocialHx', 'SocialHx');
                    }

                }
            }
            else {
                Clinical_SocialHx.UnloadSocialHistory();
                if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalSocialHx") {
                    UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                    Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabSocialHx', 'SocialHx');
                }
            }
        }

    },

    UnloadSocialHistory: function (NextOrPre) {

        var socialHxMainHtml;
        if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote") {

            Clinical_SocialHx.params.aTagTabPages = $('#pnlClinicalProgressNote #pnlClinicalSocialHx').find('section#removeDiv > ul').clone();
            Clinical_SocialHx.params.divTagTabPages = $('#pnlClinicalProgressNote #pnlClinicalSocialHx').find('section#removeDiv > div.tab-content').clone();
            socialHxMainHtml = $.trim($('#ctrlPanClinical > #pnlClinicalSocialHx').find('section#removeDiv').html());
        }

        if (Clinical_SocialHx.params["FromAdmin"] == "0") {
            if (Clinical_SocialHx.params != null && Clinical_SocialHx.params.ParentCtrl != null) {
                if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && NextOrPre == true)
                    UnloadActionPan(Clinical_SocialHx.params.ParentCtrl, 'Clinical_SurgicalHx');
                if (Clinical_SocialHx.controlToInvoke != null) {
                    setTimeout(function () {
                        Clinical_ProgressNote.SelectNotesComponentTab(Clinical_SocialHx.controlToInvoke);
                        Clinical_SocialHx.controlToInvoke = null;
                    }, 400);
                }
                else {
                    UnloadActionPan(Clinical_SocialHx.params.ParentCtrl, 'Clinical_SurgicalHx');
                    if (Clinical_SocialHx.controlToInvoke != null)
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_SocialHx.controlToInvoke);
                            Clinical_SocialHx.controlToInvoke = null;
                        }, 400);
                }


                if (socialHxMainHtml == null || typeof socialHxMainHtml == 'undefined' || socialHxMainHtml == "") {

                    Clinical_SocialHx.resetMainSocialHxTabPageHtml(Clinical_SocialHx.params.aTagTabPages, Clinical_SocialHx.params.divTagTabPages);
                }
            }
            else {
                UnloadActionPan(null, 'Clinical_SocialHx');

                if (socialHxMainHtml == null || typeof socialHxMainHtml == 'undefined' || socialHxMainHtml == "") {

                    Clinical_SocialHx.resetMainSocialHxTabPageHtml(Clinical_SocialHx.params.aTagTabPages, Clinical_SocialHx.params.divTagTabPages);
                }
            }

        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_SocialHx").remove();
            RemoveAdminTab();

            if (socialHxMainHtml == null || typeof socialHxMainHtml == 'undefined' || socialHxMainHtml == "") {

                Clinical_SocialHx.resetMainSocialHxTabPageHtml(Clinical_SocialHx.params.aTagTabPages, Clinical_SocialHx.params.divTagTabPages);
            }
        }
        EMRUtility.scrollToPNcomponent('clinical_socialhx');
    },

    /*
    Author: Muhammad Arshad
    Date: 12/16/2015
    This function will handle the color of sub tabs for each tab in SocialHx if some data is presenet in tab
    */

    changeTabColor: function (tabName) {
        if (tabName != null && tabName.toLowerCase() == "tobacco") {
            $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionTobacco').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox') {
                    if ($(this).is(':checked')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != "") {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').removeClass('successLight');
                    }
                }
            });

            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTobaccoModel.length > 0) {
                    if (!$('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').hasClass('successLight')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listTobacco').addClass('successLight');
                    }
                }
            }
        }
        else if (tabName != null && tabName.toLowerCase() == "alcohol") {
            $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionAlcohol').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox') {
                    if ($(this).is(':checked')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != "") {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').removeClass('successLight');
                    }
                }
            });
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstAlcoholModel.length > 0) {
                    if (!$('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').hasClass('successLight')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listAlcohol').addClass('successLight');
                    }
                }
            }
        }
        else if (tabName != null && tabName.toLowerCase() == "drug") {
            $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionDrugAbuse').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox') {
                    if ($(this).is(':checked')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != "" && $(this).val() != null) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').removeClass('successLight');
                    }
                }
            });
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstDrugAbuseModel.length > 0) {
                    if (!$('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').hasClass('successLight')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listDrugAbuse').addClass('successLight');
                    }
                }
            }
        }
        else if (tabName != null && tabName.toLowerCase() == "sexual") {
            $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox' || $(this).attr("type") == "radio") {
                    if ($(this).is(':checked') && $(this).attr("id") != "RadSexualNoPainWithIntercourse" && $(this).attr("id") != "RadSexualNoAbusedSexually" && $(this).attr("id") != "RadSexualNoPregnant") {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != null && $(this).val() != "") {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').removeClass('successLight');
                    }
                }
            });
            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSexualHxModel.length > 0) {
                    if (!$('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').hasClass('successLight')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listSexualHx').addClass('successLight');
                    }
                }
            }
        }
        else if (tabName != null && tabName.toLowerCase() == "miscellaneous_components") {
            $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionMiscDetails').find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
                if ($(this).attr('type') == 'checkbox') {
                    if ($(this).is(':checked')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').removeClass('successLight');
                    }
                }
                else if ($(this).attr('type') == 'radio') {
                    if ($(this).is(':checked') && $(this).attr("id") != "RadOccupationPresenttExperience") {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').removeClass('successLight');
                    }
                } else {
                    if ($(this).val() != "") {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').addClass('successLight');
                        return false;
                    } else {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').removeClass('successLight');
                    }
                }
            });

            if (Clinical_SocialHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.SocialHx != null) {
                if (Clinical_HistorySummary.HistoryCacheList.SocialHx.lstOccupationHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstSleepHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstExercisesHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstHousingHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstCaffeineIntakHxModel.length > 0 || Clinical_HistorySummary.HistoryCacheList.SocialHx.lstTravelHxModel.length > 0) {
                    if (!$('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').hasClass('successLight')) {
                        $('#' + Clinical_SocialHx.params.PanelID + ' #listMiscHx').addClass('successLight');
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
            Clinical_SocialHx.changeTabColor("alcohol");
            Clinical_SocialHx.changeTabColor("drug");
            Clinical_SocialHx.changeTabColor("sexual");
            Clinical_SocialHx.changeTabColor("miscellaneous_components");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
        else if (ctrlTab == "alcohol") {
            Clinical_SocialHx.changeTabColor("tobacco");
            Clinical_SocialHx.changeTabColor("drug");
            Clinical_SocialHx.changeTabColor("sexual");
            Clinical_SocialHx.changeTabColor("miscellaneous_components");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
        else if (ctrlTab == "drug") {

            Clinical_SocialHx.changeTabColor("tobacco");
            Clinical_SocialHx.changeTabColor("alcohol");
            Clinical_SocialHx.changeTabColor("sexual");
            Clinical_SocialHx.changeTabColor("miscellaneous_components");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
        else if (ctrlTab == "sexual") {
            Clinical_SocialHx.changeTabColor("tobacco");
            Clinical_SocialHx.changeTabColor("alcohol");
            Clinical_SocialHx.changeTabColor("drug");
            Clinical_SocialHx.changeTabColor("miscellaneous_components");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
        else if (ctrlTab == "miscellaneous_components") {
            Clinical_SocialHx.changeTabColor("tobacco");
            Clinical_SocialHx.changeTabColor("alcohol");
            Clinical_SocialHx.changeTabColor("drug");
            Clinical_SocialHx.changeTabColor("sexual");
            setTimeout(function () {
                LastSocialHx = new Object();
                LastSocialHx["PatientId"] = $('#PatientProfile #hfPatientId').val();
                LastSocialHx["SocialHxType"] = $('#' + Clinical_SocialHx.params.PanelID + " #ulSocialHxTabsItems li.active").attr('id');
            });
        }
    },


    //-----------------Progress Note-------------
    // added on Dec 14,2015 by Muhammad Azhar Shahzad
    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Call Back function to add component to Progress Note
    addSocialHxToNotes: function () {
        var SocialHxId = Clinical_SocialHx.params.SocialHxId;
        var socialHxType = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();
        Clinical_SocialHx.socialHxSave(socialHxType, true);
    },

    //this function will get Social History Soap Text and attach that to Progress note
    getSocialHxInfo: function (socialHxType, UnloadSocialhx) {
        Clinical_SocialHx.fillSocialHx(socialHxType).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_SocialHx.createSocialHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', UnloadSocialhx);

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },

    //This Function will check, if Social History Soap is already attached in Progress note, if Social History is not attached than it will create main divs to attach allergy
    checkSocialHxExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_socialhx').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="SocialHxComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_socialhx title="Social Hx"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'SocialHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="SocialHx">Social Hx</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'SocialHx\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'SocialHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_socialhx> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_socialhx').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    //This Function is used to create SOAP html and append it to  Progress note
    createSocialHxBodyHTML: function (response, NoteHTMLCtrl, UnloadSocialhx, hideAlertMessage) {
        Clinical_SocialHx.checkSocialHxExists();
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
            if ($(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).length == 0) {
                $mainDivSocialHx.append($SectionBodySocialHx);
                Clinical_SocialHx.updateSocialHxHtml($mainDivSocialHx.html(), socialHxId, NoteHTMLCtrl, hideAlertMessage);
            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).html($SectionBodySocialHx.html());
                $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul').append(CommentHTML);
                Clinical_ProgressNote.saveComponentSOAPText("SocialHx", hideAlertMessage);
                Clinical_SocialHx.updateSocialHxHtml("", socialHxId, NoteHTMLCtrl, hideAlertMessage);

            }

            if (UnloadSocialhx == true) {
                Clinical_SocialHx.UnloadSocialHistory(Clinical_SocialHx.bNextPrev);
            }
        }
    },

    createSocialHxBodyHTMLFromNotes: function (SocialHistory, NoteHTMLCtrl, UnloadSocialhx, hideAlertMessage) {

        Clinical_SocialHx.checkSocialHxExists();

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

            if ($(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).length == 0) {
                $mainDivSocialHx.append($SectionBodySocialHx);
                var SocialHxHtml = $mainDivSocialHx.html();

                $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().addClass('initialVisitBody');
                if (SocialHxHtml != '') {
                    $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().append(SocialHxHtml);
                }

                //Binding Hovering and onClick functions to Progress Note HTML
                Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
                return socialHxId;

            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).html($SectionBodySocialHx.html());
                $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul').append(CommentHTML);
            }
        }
    },

    createSocialHxBodyHTMLFromNote: function (response, NoteHTMLCtrl, UnloadSocialhx, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_SocialHx.checkSocialHxExists();

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
            if ($(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).length == 0) {
                $mainDivSocialHx.append($SectionBodySocialHx);
                var SocialHxHtml = $mainDivSocialHx.html();
                if (SocialHxHtml != '') {
                    $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().addClass('initialVisitBody');
                    $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().append(SocialHxHtml);
                }
            } else {

                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId).html($SectionBodySocialHx.html());
                $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().find('#Cli_SocialHx_Main' + socialHxId + ' ul').append(CommentHTML);
            }
        }

        dfd.resolve();
        return dfd;
    },

    createAlcoholHTML: function () {
        var AlcoholHTML = "<li id='socialAlcohol_" + item.AlcoholId + "' title='Alcohol'><strong>Alcohol: </strong>";
        AlcoholHTML += Clinical_SocialHx.IsNullReturnSoapValue(item.Status) + Clinical_SocialHx.IsNullReturnSoapValue(item.Type) + Clinical_SocialHx.IsNullReturnSoapValue(item.Frequency) + (string.IsNullOrEmpty(item.UsagePeriod) ? "" : " for " + item.UsagePeriod);
        AlcoholHTML += ", Patient has quit " + item.CessationLength + " ago" + (item.bWouldQuit ? " , Patient would quit" : "") + (item.bRecentlyQuit ? " , Patient recently quit" : "") + (item.bNotReadyToQuit ? " , Patient not ready to quit" : "");
        AlcoholHTML += ", Counselling " + item.CounsellingPeriod + " for " + item.CounsellingTopic + ", " + item.Comments + "</li>";
    },

    createDrugAbuseHTML: function () {
        var DrugAbuseHTML = "<li id='socialDrugAbuse_" + item.DrugAbuseId + "' title='Drug Abuse'><strong>Drug Abuse: </strong>";
        DrugAbuseHTML += Clinical_SocialHx.IsNullReturnSoapValue(item.Status) + Clinical_SocialHx.IsNullReturnSoapValue(item.DrugAbuseId.ToString()) + Clinical_SocialHx.IsNullReturnSoapValue(item.FrequencyDaily) + " for " + item.UsagePeriod;
        DrugAbuseHTML += ", Patient has quit " + item.CessationLength + " ago" + (item.bWouldQuit ? " , Patient would quit" : "") + (item.bRecentlyQuit ? " , Patient recently quit" : "") + ", " + item.Comments + "</li>";
    },

    createTabaccoHTML: function () {
        var TabaccoHTML = "<li id='socialTobacco_" + item.TobaccoId + "' title='Tobacco' name=''><strong>Tobacco: </strong>";
        TabaccoHTML += Clinical_SocialHx.IsNullReturnSoapValue(item.Status) + Clinical_SocialHx.IsNullReturnSoapValue(item.Type) + Clinical_SocialHx.IsNullReturnSoapValue(item.Frequency);
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
        SexualHxHTML += Clinical_SocialHx.IsNullReturnSoapValue(item.Status) + (string.IsNullOrEmpty(item.Preference) ? "" : ", Prefers " + item.Preference) + ", Using Protection: " + (item.bUSingProtection ? "Yes" : "No");
        SexualHxHTML += ", Method: " + item.ProtectionMethod + ", How often: +item.howOften+, Patient has complaints of " + item.Complaint + ", Exposed to STD:" + (item.bExposedToSTD ? "Yes" : "No");
        SexualHxHTML += ", STD: + item.DrugName+, Experiences pain with intercourse: " + (item.bPainWithIntercourse ? "Yes" : "No") + ", Abused sexually: " + (item.bSexuallyAbused ? "Yes" : "No") + ", Last Menstrual Period is " + item.LMP + ", " + item.Comments + "<Overall Comments></li>";
    },

    // This Function is called by Progress Notes (Fill SocialHx Func, CopyAllNotesCategories)
    updateSocialHxHtml: function (SocialHxHtml, socialHxId, NoteHTMLCtrl, hideAlertMessage) {
        $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().addClass('initialVisitBody');
        if (SocialHxHtml != '') {
            $(NoteHTMLCtrl + ' clinical_socialhx').parent().parent().append(SocialHxHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (SocialHxHtml != '') {
            Clinical_SocialHx.AttachSocialHxFromNotes(socialHxId, hideAlertMessage);
        }

    },

    enableDurationValidation: function () {



        var stayLength = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #txtTobaccoCessationLength').val();
        var ddlVal = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ddlTobaccoCessationPeriod').val();
        if (stayLength != null && stayLength != '') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationPeriod', true);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration<span class="required">*</span>');
        }
        else {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationPeriod', false);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration');
        }
        if (ddlVal != null && ddlVal != '') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationLength', true);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration<span class="required">*</span>');

        } else {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx').data('bootstrapValidator').enableFieldValidators('TobaccoCessationLength', false);
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #lblDuration').html('Duration');
        }

    },

    //This Function detach Social History From progress note
    detach_ComponentsSocialHx: function (ComponentName, IsUpdate, SocialHxComponentRemove) {
        var Clinical_SocialHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_socialhx').parent().parent().find('section[id*="Cli_SocialHx_Main"]').map(function () {
            return this.id.replace("Cli_SocialHx_Main", "");
        }).get().join(',');

        if (SocialHxComponentRemove) {

            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_socialhx').parent().parent().attr('NoteComponentId');
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Social Hx']").remove();
            if (Clinical_ProgressNote.params["TemplateName"])
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_socialhx').parent().parent().addClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_socialhx').parent().parent().remove();
            var hxComponents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .HxComponent').length;

            if (NoteComponentId && NoteComponentId != "NCDummyId" && hxComponents == 0) {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_socialhx').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('SocialHx', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_socialhx').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            }
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_socialhx').parent().parent().find('section[id*="Cli_SocialHx_Main"]').remove();
        }

        if (Clinical_SocialHxIds == "" || Clinical_SocialHxIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText("SocialHx", true);
            //Clinical_ProgressNote.Detach_ComponentsOthers(ComponentName, true);
        }
        else {
            Clinical_SocialHx.DetachSocialHxFromNotes_DBCall(Clinical_SocialHxIds).done(function (response) {
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
                EMRUtility.scrollToPNcomponent('clinical_socialhx');
                var selectedValue = SocialHxId.replace('Cli_SocialHx_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_SocialHx.DetachSocialHxFromNotes_DBCall(selectedValue).done(function (response) {
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
        Clinical_SocialHx.AttachSocialHxFromNotes_DBCall(SocialHxId).done(function (response) {
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

        Clinical_SocialHx.getLatestClinical_SocialHxByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_SocialHx.createSocialHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
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

    getLatestClinical_SocialHxByPatientId_DBCall: function () {
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
        var objProtectionMethod = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualProtectionMethod');
        var objProtectionPeriod = $('#' + Clinical_SocialHx.params.PanelID + ' section#sectionSexualHx #ddlSexualProtectionPeriod');
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
                //Clinical_SocialHx.resetControlValue(objProtectionMethod);
                //Clinical_SocialHx.resetControlValue(objProtectionPeriod);
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
        Clinical_SocialHx.updateMiscOrderSorting_Dbcall(miscCompnentSorted).done(function (response) {
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
        //$('#ctrlPanClinical > #pnlClinicalSocialHx').find('section#removeDiv > ul').remove();
        //$('#ctrlPanClinical > #pnlClinicalSocialHx').find('section#removeDiv > div.tab-content').remove();
        $('#ctrlPanClinical > #pnlClinicalSocialHx').remove();
        RemoveCurrentTab("clinicalTabSocialHx");
    },

    showSocialHxTobaccoHistory: function (tobaccoId) {
        //Begin 03-07-2016 Edit By Humaira Yousaf Bug# 1358
        var ParentCtrl = 'clinicalTabSocialHx';
        var ParentCtrlPanelID = null;
        if (Clinical_SocialHx.params["ParentCtrl"] == "clinicalTabProgressNote") {
            ParentCtrl = 'Clinical_SocialHx';
            ParentCtrlPanelID = Clinical_SocialHx.params.PanelID;
        }
        else if (Clinical_SocialHx.params["ParentCtrl"] == "clinicalTabFaceSheet") {
            ParentCtrl = 'Clinical_SocialHx';
            ParentCtrlPanelID = Clinical_SocialHx.params.PanelID;
        }
        EMRUtility.showCurrentItemHistory(Clinical_SocialHx.params.PanelID, null, null, "SocialHx_Tobacco", null, ParentCtrl, tobaccoId, ParentCtrlPanelID);
        //End 03-07-2016 Edit By Humaira Yousaf Bug# 1358

    },

    showSocialHxItemHistory: function (socialHxId, tableName) {
        //Start 03-07-2016 Edit By Humaira Yousaf Bug# 1358

        var ParentCtrl = 'clinicalTabSocialHx';
        var ParentCtrlPanelID = null;
        //if (Clinical_SocialHx.params["ParentCtrl"] == "clinicalTabProgressNote") {
        //    ParentCtrl = 'Clinical_SocialHx';
        //    ParentCtrlPanelID = Clinical_SocialHx.params.PanelID;
        //}


        if (Clinical_SocialHx.params.TabID == "clinicalTabProgressNote") {

            ParentCtrl = "Clinical_HistorySummary";
            ParentCtrlPanelID = "pnlClinicalProgressNote";
        }
        else if (Clinical_SocialHx.params["ParentCtrl"] == "clinicalTabFaceSheet") {
            ParentCtrl = 'Clinical_SocialHx';
            ParentCtrlPanelID = Clinical_SocialHx.params.PanelID;
        }
        EMRUtility.showCurrentItemHistory(Clinical_SocialHx.params.PanelID, null, null, tableName, null, ParentCtrl, socialHxId, ParentCtrlPanelID);
        //End 03-07-2016 Edit By Humaira Yousaf Bug# 1358

    },

    // Date: 01/18/2016
    // Author: Abid Ali
    // Overview: This function will reset tabpage html of main socialHx form
    resetMainSocialHxTabPageHtml: function (ul, tabContent) {
        //Append tabpages to main MainSocialHx
        $('#ctrlPanClinical > #pnlClinicalSocialHx').find('section#removeDiv').append(ul).append(tabContent);
        //reset multi-select dropdown list
        $('#ctrlPanClinical > #pnlClinicalSocialHx').find("#ddlSexualSTD").next().remove();
        $('#ctrlPanClinical > #pnlClinicalSocialHx').find("#ddlSexualSTD").multiselect('rebuild');
        $('#ctrlPanClinical > #pnlClinicalSocialHx').find("#ddlDrugType").next().remove();
        $('#ctrlPanClinical > #pnlClinicalSocialHx').find("#ddlDrugType").multiselect('rebuild');
    },
    // Date: 12-07-2016
    // Author: Humaira Yousaf
    // Overview: Enable Disable Counselling Topic dropdown
    enableDisableCounsellingTopic: function (sender) {

        if (sender == "tobacco") {
            var selectedVal = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingPeriod").val();
            if (selectedVal == "" || selectedVal == "4") {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingTopic").addClass("disableAll");
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingTopic").val($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingTopic option:first").val());
            }
            else {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlTobaccoCounsellingTopic").removeClass("disableAll");
            }
        }
        else if (sender == "alcohol") {
            var selectedVal = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingPeriod").val();
            if (selectedVal == "" || selectedVal == "4") {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingTopic").addClass("disableAll");
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingTopic").val($('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingTopic option:first").val());
            }
            else {
                $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #ddlAlcoholCounsellingTopic").removeClass("disableAll");
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
            cessation = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx input[id*=txtTobaccoCessationLength]');
            cessationPeriod = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ddlTobaccoCessationPeriod');
        }

        if (sender == 'alcohol') {
            cessation = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx input[id*=txtAlcoholCessationLength]');
            cessationPeriod = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ddlAlcoholCessationPeriod');
        }

        if (sender == 'drug') {
            cessation = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx input[id*=txtDrugCessationLength]');
            cessationPeriod = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ddlDrugCessationPeriod');
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
        var isactive = $('#' + Clinical_SocialHx.params.PanelID + ' #pnlSocialHx_Result #divSwitch #switchActive').attr('isactive');

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx")) {
            $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx").dataTable().fnClearTable();
            $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx").dataTable().fnDestroy();
            $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx tbody").find("tr").remove();
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

                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listTobacco').trigger('onclick');$('#ulSocialHxTabsItems #listTobacco a').trigger('onclick');Clinical_SocialHx.SelectTab('Tobacco');");
                    }
                    else if ($(this).text().toLowerCase().indexOf("alcohol") > -1) {
                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listAlcohol').trigger('onclick');$('#ulSocialHxTabsItems #listAlcohol a').trigger('onclick');Clinical_SocialHx.SelectTab('Alcohol');");
                    }
                    else if ($(this).text().toLowerCase().indexOf("drug abuse") > -1) {
                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listDrugAbuse').trigger('onclick');$('#ulSocialHxTabsItems #listDrugAbuse a').trigger('onclick');Clinical_SocialHx.SelectTab('DrugAbuse');");
                    }
                    else if ($(this).text().toLowerCase().indexOf("sexual") > -1) {
                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listSexualHx').trigger('onclick');$('#ulSocialHxTabsItems #listSexualHx a').trigger('onclick');Clinical_SocialHx.SelectTab('SexualHx');");
                    }
                    else if ($(this).text().toLowerCase().indexOf("misc") > -1) {
                        $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listMiscHx').trigger('onclick');$('#ulSocialHxTabsItems #listMiscHx a').trigger('onclick');Clinical_SocialHx.SelectTab('Miscellaneous');");
                    }
                });
                text = $(outerDiv).html();
                $row.append('<td style="display:none;">' + counter + '</td><td>' + LoadJSONData[i].Action + '</td><td id="sptxt">' + text + '</td><td>' + LoadJSONData[i].ModifiedOn + " " + LoadJSONData[i].ModifiedBy + '</td>');
                $row.find('#sptxt').html(text);
                $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx tbody").last().append($row);
                //   });
            }
        }
        else {
            $("#" + Clinical_SocialHx.params.PanelID + ' #pnlSocialHx_Result #dgvPastSocialHx').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Known Social History"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bInfo": false, "bPaginate": false, "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_SocialHx.params.PanelID + ' #pnlSocialHx_Result #dgvPastSocialHx'))
            ;
        else {
            $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[0, "asc"]], "aoColumnDefs": [{ "bInfo": false, "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        }

        $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx_filter").remove();
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
            $('#' + Clinical_SocialHx.params.PanelID + " #pnlCurrent ").addClass("hidden");
            $('#' + Clinical_SocialHx.params.PanelID + " #pnlPast ").removeClass("hidden");
            Clinical_SocialHx.fillhxLog(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                if (response != "") {
                    var json = JSON.parse(response);
                    Clinical_SocialHx.gridLoad(response);

                    var TableControl = Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvPastSocialHx";
                    var PagingPanelControlID = Clinical_SocialHx.params.PanelID + " #dgvPastSocialHx_Paging";
                    var ClassControlName = "Clinical_SocialHx";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = json.iTotalDisplayRecords;
                    setTimeout(
                        CreatePagination(json.HxLogSoapCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_SocialHx.ChangeCurrentPast(1, PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                }
            });


        } else {
            $(obj).attr('status', 1);

            $('#' + Clinical_SocialHx.params.PanelID + " #pnlPast").addClass("hidden");
            $('#' + Clinical_SocialHx.params.PanelID + " #pnlCurrent  ").removeClass("hidden");
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
        objData["HxId"] = Clinical_SocialHx.params.HxTypeId;
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

                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listTobacco').trigger('onclick');$('#ulSocialHxTabsItems #listTobacco a').trigger('onclick');Clinical_SocialHx.SelectTab('Tobacco');");
            }
            else if ($(this).text().toLowerCase().indexOf("alcohol") > -1) {
                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listAlcohol').trigger('onclick');$('#ulSocialHxTabsItems #listAlcohol a').trigger('onclick');Clinical_SocialHx.SelectTab('Alcohol');");
            }
            else if ($(this).text().toLowerCase().indexOf("drug abuse") > -1) {
                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listDrugAbuse').trigger('onclick');$('#ulSocialHxTabsItems #listDrugAbuse a').trigger('onclick');Clinical_SocialHx.SelectTab('DrugAbuse');");
            }
            else if ($(this).text().toLowerCase().indexOf("sexual") > -1) {
                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listSexualHx').trigger('onclick');$('#ulSocialHxTabsItems #listSexualHx a').trigger('onclick');Clinical_SocialHx.SelectTab('SexualHx');");
            }
            else if ($(this).text().toLowerCase().indexOf("misc") > -1) {
                $(this).closest('a').attr('onclick', "$('#ulSocialHxTabsItems #listMiscHx').trigger('onclick');$('#ulSocialHxTabsItems #listMiscHx a').trigger('onclick');Clinical_SocialHx.SelectTab('Miscellaneous');");
            }
        });

        resopnse.SoapText = $(outerDiv).html();
        if (typeof resopnse.IsCreatedOrModified != typeof undefined && typeof resopnse.LastUpdated != typeof undefined) {
            $row.append('<td style="display:none;">' + resopnse.SocialHxId + '</td><td>' + resopnse.IsCreatedOrModified + '</td><td>' + resopnse.SoapText + '</td><td>' + resopnse.LastUpdated + '</td>');
            $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").html($row);
        }
        else {
            var $row1 = $('<tr/>');
            $row1.append('<td style="display:none;"></td><td>&nbsp;</td><td>No Known Social History</td><td></td>');
            $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").html($row1);
        }


        if ($('#' + Clinical_SocialHx.params.PanelID + ' #pnlSocialHx_Result #divSwitch #switchVisit').attr('status') == '1') {
            $('#' + Clinical_SocialHx.params.PanelID + ' #pnlCurrent').removeClass('hidden');
            $('#' + Clinical_SocialHx.params.PanelID + ' #pnlPast').addClass('hidden');
        }

        if (IsVarifyMUAlert && $("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody"))
        {
            Clinical_SocialHx.VarifyMUAlert();
        }

    },

    VarifyMUAlert: function () {

        var m1_obj = {
            ProfileName: "SocialHx",
            Fields: "",
            PatientId: Clinical_SocialHx.params.patientID,
            IsShowAlert: false,
            Type: "MU3"
        };

        var Fileds = "";
        if ($("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").find("[id^='socialMiscHxTravel']").length <= 0) {
            Fileds += "Travel";
        }
        var isFrom = false;
        var isTo = false;
        if ($($("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").find("[id^='socialMiscHxOccupation']")).length > 0) {
            $.each($($("#" + Clinical_SocialHx.params.PanelID + " #pnlSocialHx_Result #dgvSocialHx tbody").find("[id^='socialMiscHxOccupation']")), function () {

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
                var IsAnyOtherAlert = data.filter(item=>item.PatientId + "" == Clinical_SocialHx.params.patientID);
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
        $('#' + Clinical_SocialHx.params.PanelID + ' .tabs-custom-body .tab-pane').removeClass('active');
        $('#' + Clinical_SocialHx.params.PanelID + ' .tabs-custom-body #' + tabName).addClass('active');
        $('#' + Clinical_SocialHx.params.PanelID + ' #ulSocialHxTabsItems li').removeClass('active');
        if (tabName == 'Tobacco')
            $('#' + Clinical_SocialHx.params.PanelID + ' #ulSocialHxTabsItems #listTobacco').addClass('active');
        else if (tabName == 'Alcohol')
            $('#' + Clinical_SocialHx.params.PanelID + ' #ulSocialHxTabsItems #listAlcohol').addClass('active');
        else if (tabName == 'DrugAbuse')
            $('#' + Clinical_SocialHx.params.PanelID + ' #ulSocialHxTabsItems #listDrugAbuse').addClass('active');
        else if (tabName == 'SexualHx')
            $('#' + Clinical_SocialHx.params.PanelID + ' #ulSocialHxTabsItems #listSexualHx').addClass('active');
        else if (tabName == 'Miscellaneous')
            $('#' + Clinical_SocialHx.params.PanelID + ' #ulSocialHxTabsItems #listMiscHx').addClass('active');
    },

    // Date: 15-07-2016
    // Author: Humaira Yousaf
    // Overview: Enable Disable MenstrualPeriod
    enableDisableMenstrualPeriod: function () {
        var selectedPreference = $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx #ddlSexualPreferences').val();

        if (selectedPreference == "2") {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx input[id*=dtpSexualLMP]').removeClass('disableAll');
        }
        else {
            $('#' + Clinical_SocialHx.params.PanelID + ' #frmClinicalSocialHx input[id*=dtpSexualLMP]').addClass('disableAll');
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
            self = $('#' + Clinical_SocialHx.params.PanelID + " div#Tobacco");
        }
        else if (socialHxType.toLowerCase() == "alcohol") {
            self = $('#' + Clinical_SocialHx.params.PanelID + " div#Alcohol");
        }
        else if (socialHxType.toLowerCase() == "drugabuse") {
            self = $('#' + Clinical_SocialHx.params.PanelID + " div#DrugAbuse");
        }
        else if (socialHxType.toLowerCase() == "sexual") {
            self = $('#' + Clinical_SocialHx.params.PanelID + " div#SexualHx");
        }
        else if (socialTypeMiscText.toLowerCase().indexOf("miscellaneous") > -1) {
            Clinical_SocialHx.retainedComponentMisHx = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #MiscHxMainStatus #ulMiscStatus li.active").attr("id");
            if (socialTypeMiscText.toLowerCase() == "miscellaneous_occupation") {
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#OccupationDetails");
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_sleep") {
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#SleepDetails");
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_exercises") {
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#ExercisesDetails");

            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_housing") {
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#HousingDetails");
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_caffeineintake") {
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#CaffeineIntakDetails");
            }
            else if (socialTypeMiscText.toLowerCase() == "miscellaneous_travel") {
                self = $('#' + Clinical_SocialHx.params.PanelID + " div#divTravelDetailsHx");
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
            stausData["MiscChildStatus"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active").attr("id");
            stausData["MiscChildStatusText"] = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #Miscellaneous #ulMiscChildStatus li.active a").text();
            if (socialTypeMiscText.toLowerCase() == "miscellaneous_exercises") {
                stausData["ExercisesTypeText"] = $('#' + Clinical_SocialHx.params.PanelID + " div#ExercisesDetails").find('#ddlExercisesType option:selected').text();
                stausData["ExercisesDietText"] = $('#' + Clinical_SocialHx.params.PanelID + " div#ExercisesDetails").find('#ddlExercisesDiet option:selected').text();
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

            if (Clinical_SocialHx.params.patientID == null) {
                patientId = $('#PatientProfile #hfPatientId').val();
            } else {
                patientId = Clinical_SocialHx.params.patientID;
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
                SocialHxId: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val(),
                PatientId: patientId,
                SocialHxDate: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val(),
                SocialHxUnremarkable: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked"),
                SocialComments: $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val(),
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

            if (Clinical_SocialHx.params.patientID == null) {
                patientId = $('#PatientProfile #hfPatientId').val();
            } else {
                patientId = Clinical_SocialHx.params.patientID;
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
            Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxId = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxId").val();
            Clinical_HistorySummary.HistoryCacheList.SocialHx.PatientId = patientId;
            Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxDate = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #dtSocialHxDate").val();
            Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialHxUnremarkable = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #chkSocialHxUnremarkable").prop("checked");
            Clinical_HistorySummary.HistoryCacheList.SocialHx.SocialComments = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #txtSocialComments").val();
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