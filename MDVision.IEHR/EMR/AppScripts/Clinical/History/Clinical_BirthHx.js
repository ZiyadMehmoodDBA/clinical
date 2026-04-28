Clinical_BirthHx = {
    //Author: Muhammad Azhar Shahzad
    //Date: January 06,2015
    //This file will handle all actions performed for Birth History and it's child handling
    //Once BirthHx will be created then it's child can be created then.
    bIsFirstLoad: true,
    params: [],
    generalJSON: '',
    maternityJSON: '',
    newBornJSON: '',
    overallComments: '',
    unremarkable: false,
    date: '',
    //Author: Muhammad Azhar Shahzad
    //Date: January 06,2015
    //This function will be called once tab is clicked, it expects parameters to be used for BirthHx
    Load: function (params) {
        Clinical_BirthHx.params = params;
        if (Clinical_BirthHx.params.mode == null) {
            Clinical_BirthHx.params.mode = "Add";
        }
        if (Clinical_BirthHx.params.PanelID != 'pnlClinicalBirthHx') {
            Clinical_BirthHx.params.PanelID = Clinical_BirthHx.params.PanelID + ' #pnlClinicalBirthHx';
        } else {
            Clinical_BirthHx.params.PanelID = 'pnlClinicalBirthHx';
        }
        if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabNotes") {
            Clinical_BirthHx.bIsFirstLoad = true;
            $('#divViewHistorySummary').addClass('hidden');
            $(' #pnlClinicalBirthHx').removeClass('row');
        }
        var BirthHxId = "";
        if (Clinical_BirthHx.params.mode == "Add" || Clinical_BirthHx.params.BirthHxId == null || Clinical_BirthHx.params.BirthHxId == "" || Clinical_BirthHx.params.BirthHxId == "-1" || Clinical_BirthHx.params.TabID == "clinicalTabBirthHx") {
            Clinical_BirthHx.params.BirthHxId = "-1";
            Clinical_BirthHx.params.mode = "Add";
        }
        else if (Clinical_BirthHx.params.mode == "Edit") {
            BirthHxId = Clinical_BirthHx.params.BirthHxId;
            //Clinical_BirthHx.BirthHxEdit(BirthHxId);
        }

        Clinical_BirthHx.resetBirthHx(true);


        //Load Dropdown





        var self = $('#' + Clinical_BirthHx.params.PanelID);



        self.loadDropDowns(true).done(function () {
            BackgroundLoaderShow(true);
            Clinical_BirthHx.loadBirthHx('general');
            /*Load all autocompletes for this form (Providers),
        Author: ZeeshanAK  | Date: January 05, 2016  */
            Clinical_BirthHx.loadAllAutocomplete();
        });

        //end Load Dropdown

        if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_BirthHx.params.PanelID, 'History', 'BirthHx', 'Clinical_BirthHx.unLoadTab();');
        }
        Clinical_BirthHx.showSaveBtnsForBirthHx();
        //end change azhar Dec 15, 2015
        Clinical_BirthHx.domReadyFunc();
        //Start || 14 July, 2016 || ZeeshanAK || Fix for EMR-1516
        utility.CreateDatePicker('pnlClinicalBirthHx #frmClinicalBirthHx #PatientDOB',
        function (ev) {
            Clinical_BirthHx.checkDobAndAdmitDates($('#pnlClinicalBirthHx #frmClinicalBirthHx #PatientDOB'));
            //Clinical_BirthHx.ValidatePatientDOB($('#pnlClinicalBirthHx #frmClinicalBirthHx #PatientDOB'));
            //if ($('#pnlClinicalBirthHx #frmClinicalBirthHx').data("bootstrapValidator") != null) {
            //    $('#pnlClinicalBirthHx #frmClinicalBirthHx').bootstrapValidator('revalidateField', 'PatientDOB');
            //}

        }, false);

        $('#pnlClinicalBirthHx #frmClinicalBirthHx #PatientDOB').datepicker('setEndDate', new Date());
        utility.CreateDatePicker('pnlClinicalBirthHx #frmClinicalBirthHx #DateAdmitted',
        function (ev) {
            Clinical_BirthHx.checkDobAndAdmitDates($('#pnlClinicalBirthHx #frmClinicalBirthHx #DateAdmitted'));
            //Clinical_BirthHx.ValidateDateAdmitted($('#pnlClinicalBirthHx #frmClinicalBirthHx #DateAdmitted'));
            //if ($('#pnlClinicalBirthHx #frmClinicalBirthHx').data("bootstrapValidator") != null) {
            //    $('#pnlClinicalBirthHx #frmClinicalBirthHx').bootstrapValidator('revalidateField', 'DateAdmitted');
            //}
        }, true);
        $('#pnlClinicalBirthHx #frmClinicalBirthHx #DateAdmitted').datepicker('setEndDate', new Date());
        //End   || 14 July, 2016 || ZeeshanAK || Fix for EMR-1516



        /*
          Change Implement BY: Muhammad Azhar Shahzad
          Reason:To Show navigation on Progress Note
          Created Date: Dec 15, 2015
      */
        //Code for progress note navigation

        Clinical_BirthHx.toggleReadyFunction();

        if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_BirthHx.params.PanelID + ' #btnBirthHxSave').addClass('hidden');
            $('#' + Clinical_BirthHx.params.PanelID + ' #btnAddVitalsOnNote').addClass('hidden');
            Clinical_BirthHx.generalJSON = '';
            Clinical_BirthHx.maternityJSON = '';
            Clinical_BirthHx.newBornJSON = '';
        }
        else {
            $('#' + Clinical_BirthHx.params.PanelID + ' #btnBirthHxSave').removeClass('hidden');
        }

    },

    /*Load all autocompletes for this form (Providers),
         Author: ZeeshanAK  | Date: January 05, 2016  */
    loadAllAutocomplete: function () {
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#" + Clinical_BirthHx.params.PanelID + " input#txtProvider");
            var hfCtrl = $("#pnlClinicalBirthHx #hfProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
        });
    },
    Preventzerovalue: function (obj) {
        var Ctrlvalue = "";
        if (obj != null) {
            Ctrlvalue = $(obj).val();
        }
        var Value = parseInt(Ctrlvalue);


        Value = Value > 0 ? Value : "";

        $(obj).val(Value);

    },

    toggleReadyFunction: function () {
        $(function () {
            (function ($) {
                'use strict';
                $(function () {
                    $('#' + Clinical_BirthHx.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                        var $this = $(this);

                        $this.themePluginIOS7Switch();
                    });
                });
            }).apply(this, [jQuery]);

        });


    },

    /* Shows the Save button if the BirthHx is shown on top of Progress Notes
        Author: Muhammad Azhar Shahzad */
    showSaveBtnsForBirthHx: function () {
        if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {

            $('#' + Clinical_BirthHx.params.PanelID + ' #btnAddVitalsOnNote').show();
            $('#' + Clinical_BirthHx.params.PanelID + ' #btnBirthHxSave').show();
            $('#' + Clinical_BirthHx.params.PanelID + '  #dtBirthHxDate').prop('disabled', false);
        } else {
            $('#' + Clinical_BirthHx.params.PanelID + ' #btnAddVitalsOnNote').hide();
            $('#' + Clinical_BirthHx.params.PanelID + ' #btnBirthHxSave').show();
            $('#' + Clinical_BirthHx.params.PanelID + '  #dtBirthHxDate').prop('disabled', false);
        }
    },


    /* Initializing date pickers and binding DOM events
       Author: Muhammad Azhar Shahzad */
    domReadyFunc: function () {

        utility.CreateDatePicker(Clinical_BirthHx.params.PanelID + '  #dtBirthHxDate', function () {
        }, true);
        $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionGeneral').on('change', function () {
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsGeneralUpdate').val('true');
        });
        $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionMaternalDelivery').on('change', function () {
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsDeliveryUpdate').val('true');
        });
        $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionNewborn').on('change', function () {
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsNewbornUpdate').val('true');
        });

        $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx [data-plugin-keyboard-numpad]').keyboard({
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

        $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx [data-plugin-keyboard-numpad-nodecimal]').keyboard({
            customLayout: {
                'default': [
                    '7 8 9 {b}',
                    '4 5 6 {clear}',
                    '1 2 3 {t}',
                    '0     {a} {c} '
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



    },

    /* Enable/Disable BirthHx sections i.e. General, Maternal and Newborn
       Author: Muhammad Azhar Shahzad */
    enableDisableBirthHxControls: function (obj, currentStatus) {

        if (obj != null) {
            Clinical_BirthHx.CacheSectionsData(true);
            $($(obj).parent()).find("li").each(function () {
                $(this).removeClass("active");
            });
            if ($(obj).hasClass("active") == false) {
                $(obj).addClass("active");
            }
        }
        if (currentStatus != null && currentStatus != "") {
            if (currentStatus == "General") {
                $('#' + Clinical_BirthHx.params.PanelID + ' div#sectionGeneral').show();
                $('#' + Clinical_BirthHx.params.PanelID + ' div#sectionMaternalDelivery').hide();
                $('#' + Clinical_BirthHx.params.PanelID + ' div#sectionNewborn').hide();
            }
            else if (currentStatus == 'MaternalDelivery') {
                $('#' + Clinical_BirthHx.params.PanelID + ' div#sectionMaternalDelivery').show();
                $('#' + Clinical_BirthHx.params.PanelID + ' div#sectionGeneral').hide();
                $('#' + Clinical_BirthHx.params.PanelID + ' div#sectionNewborn').hide();
            }
            else if (currentStatus == 'Newborn') {
                $('#' + Clinical_BirthHx.params.PanelID + ' div#sectionNewborn').show();
                $('#' + Clinical_BirthHx.params.PanelID + ' div#sectionMaternalDelivery').hide();
                $('#' + Clinical_BirthHx.params.PanelID + ' div#sectionGeneral').hide();
            }
            $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #hfBirthHxDataChangeBit').val("1");
        }
    },
    CacheSectionsData: function (updateUI) {
        var dfd = $.Deferred();
        if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
            var activeSection = $('#' + Clinical_BirthHx.params.PanelID + ' #ulSection > li.active').attr('id');
            if (activeSection == "general") {
                if (Clinical_BirthHx.generalJSON != '') {
                    var updatedJSON = $('#' + Clinical_BirthHx.params.PanelID + " #sectionGeneral").getMyJSONByName();
                    if (Clinical_BirthHx.generalJSON != updatedJSON) {
                        $.when(Clinical_BirthHx.cacheBirthHxJSON('General', updatedJSON)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        if ($('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #chkBirthHxUnremarkable").is(":checked")) {
                            $.when(Clinical_BirthHx.cacheBirthHxJSON('General', updatedJSON, true)).then(function () {
                                dfd.resolve();
                            });
                        }
                        else {
                            if (Clinical_BirthHx.unremarkable && !$('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #chkBirthHxUnremarkable").is(":checked")) {
                                $.when(Clinical_BirthHx.cacheBirthHxJSON('General', updatedJSON, true)).then(function () {
                                    dfd.resolve();
                                });
                            }
                            else {
                                var date = $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #dtBirthHxDate").val();
                                var unremarkable = $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #chkBirthHxUnremarkable").prop('checked');
                                var comments = $('#' + Clinical_BirthHx.params.PanelID + " #txtBirthHxComments").val();

                                var dataChanged = Clinical_BirthHx.date != date || Clinical_BirthHx.unremarkable != unremarkable || Clinical_BirthHx.overallComments != comments;
                                if (dataChanged) {
                                    $.when(Clinical_BirthHx.cacheBirthHxJSON('General', updatedJSON, true)).then(function () {
                                        dfd.resolve();
                                    });
                                }
                                else {
                                    dfd.resolve();
                                }
                            }
                        }
                    }
                }
                else {
                    dfd.resolve();
                }
            }
            else if (activeSection == "maternalDelivery") {
                if (Clinical_BirthHx.maternityJSON != '') {
                    var updatedJSON = $('#' + Clinical_BirthHx.params.PanelID + " #sectionMaternalDelivery").getMyJSONByName();
                    if (Clinical_BirthHx.maternityJSON != updatedJSON) {
                        $.when(Clinical_BirthHx.cacheBirthHxJSON('MaternalDelivery', updatedJSON)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        dfd.resolve();
                    }
                }
                else {
                    dfd.resolve();
                }
            }
            else if (activeSection == "newborn") {
                if (Clinical_BirthHx.newBornJSON != '') {
                    var updatedJSON = $('#' + Clinical_BirthHx.params.PanelID + " #sectionNewborn").getMyJSONByName();
                    if (Clinical_BirthHx.newBornJSON != updatedJSON) {
                        $.when(Clinical_BirthHx.cacheBirthHxJSON('NewBorn', updatedJSON)).then(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        dfd.resolve();
                    }
                }
                else {
                    dfd.resolve();
                }
            }
            else {
                dfd.resolve();
            }

            if (updateUI == true) {
                var ComponentName = 'General';
                Clinical_BirthHx.bindcachedData('General');
                Clinical_BirthHx.bindcachedData('MaternalDelivery');
                Clinical_BirthHx.bindcachedData('Newborn');
                dfd.resolve();
            }
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    //CacheSectionsData: function (updateUI) {
    //    if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
    //        var activeSection = $('#' + Clinical_BirthHx.params.PanelID + ' #ulSection > li.active').attr('id');
    //        if (activeSection == "general") {
    //            if (Clinical_BirthHx.generalJSON != '') {
    //                var updatedJSON = $('#' + Clinical_BirthHx.params.PanelID + " #sectionGeneral").getMyJSONByName();
    //                Clinical_BirthHx.cacheBirthHxJSON('General', updatedJSON);

    //                //if (Clinical_BirthHx.generalJSON != updatedJSON) {
    //                //    Clinical_BirthHx.cacheBirthHxJSON('General', updatedJSON);
    //                //}
    //            }
    //        }
    //        else if (activeSection == "maternalDelivery") {
    //            if (Clinical_BirthHx.maternityJSON != '') {
    //                var updatedJSON = $('#' + Clinical_BirthHx.params.PanelID + " #sectionMaternalDelivery").getMyJSONByName();
    //                if (Clinical_BirthHx.maternityJSON != updatedJSON) {
    //                    Clinical_BirthHx.cacheBirthHxJSON('MaternalDelivery', updatedJSON);
    //                }
    //            }
    //        }
    //        else if (activeSection == "newborn") {
    //            if (Clinical_BirthHx.newBornJSON != '') {
    //                var updatedJSON = $('#' + Clinical_BirthHx.params.PanelID + " #sectionNewborn").getMyJSONByName();
    //                if (Clinical_BirthHx.newBornJSON != updatedJSON) {
    //                    Clinical_BirthHx.cacheBirthHxJSON('NewBorn', updatedJSON);
    //                }
    //            }
    //        }

    //        if (updateUI == true) {
    //            var ComponentName = 'General';
    //            Clinical_BirthHx.bindcachedData('General');
    //            Clinical_BirthHx.bindcachedData('MaternalDelivery');
    //            Clinical_BirthHx.bindcachedData('Newborn');
    //        }

    //    }
    //},

    bindcachedData: function (componentName) {
        var cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON(componentName);
        if (cachedJSON) {
            cachedJson = typeof cachedJSON != "object" ? JSON.parse(cachedJSON) : cachedJSON;
            utility.bindMyJSONByName(true, cachedJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #section" + componentName))
        }
    },
    /* Resets the currently active section under BirthHx
       Author: Muhammad Azhar Shahzad */
    resetBirthHx: function (resetAll) {
        if (resetAll != null && resetAll) {
            EMRUtility.resetControlValue($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionGeneral'));
            $('#' + Clinical_BirthHx.params.PanelID + " #hfProvider").val("");
            if ($('#' + Clinical_BirthHx.params.PanelID + " #lnkProviderEdit").css("display") == "inline") {
                $('#' + Clinical_BirthHx.params.PanelID + " #lnkProviderEdit").css("display", "none");
                $('#' + Clinical_BirthHx.params.PanelID + " #lblProvider").css("display", "inline");
            }
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsGeneralUpdate').val('false');
            EMRUtility.resetControlValue($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionMaternalDelivery'));
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsDeliveryUpdate').val('false');
            EMRUtility.resetControlValue($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionNewborn'));
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsNewbornUpdate').val('false');
            $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #chkBirthHxUnremarkable").prop('checked', false);
            $('#' + Clinical_BirthHx.params.PanelID + ' #BirthHxChildSection').removeClass('disableAll');
        } else {
            var generalChange = $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsGeneralUpdate').val();
            var DeliveryChange = $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsDeliveryUpdate').val();
            var NewbornChange = $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsNewbornUpdate').val();
            if (NewbornChange == 'true' || DeliveryChange == 'true' || generalChange == 'true') {
                /* Added Confirm Dialogue before reseting the fields, This change is implemented by Azhar, on verbal discussion with Zubair(BA)*/
                utility.myConfirm('22', function () {
                    if ($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionGeneral').is(':visible')) {
                        //Start || 14 July, 2016 || ZeeshanAK || Fix for EMR-1516
                        //set default Date Formate
                        if (globalAppdata['DateFormat'])
                            date_format = globalAppdata['DateFormat'];
                        $('#' + Clinical_BirthHx.params.PanelID + " #PatientDOB").datepicker('setDate', $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
                        $('#' + Clinical_BirthHx.params.PanelID + " #DateAdmitted").datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
                        //End   || 14 July, 2016 || ZeeshanAK || Fix for EMR-1516

                        EMRUtility.resetControlValue($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionGeneral'));
                        $('#' + Clinical_BirthHx.params.PanelID + " #hfProvider").val("");
                        if ($('#' + Clinical_BirthHx.params.PanelID + " #lnkProviderEdit").css("display") == "inline") {
                            $('#' + Clinical_BirthHx.params.PanelID + " #lnkProviderEdit").css("display", "none");
                            $('#' + Clinical_BirthHx.params.PanelID + " #lblProvider").css("display", "inline");
                        }
                        $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsGeneralUpdate').val('true');

                    } else if ($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionMaternalDelivery').is(':visible')) {
                        EMRUtility.resetControlValue($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionMaternalDelivery'));
                        $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsDeliveryUpdate').val('true');

                    } else if ($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionNewborn').is(':visible')) {
                        EMRUtility.resetControlValue($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #sectionNewborn'));
                        $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsNewbornUpdate').val('true');
                    }
                    $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').data('serialize', $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').serialize());
                }, function () { });
            }
        }
    },
    /* Saves BirthHx data for all three sections,
       If unload is true it will close the modal popup of BirthHx from Progress Note
       Author: Muhammad Azhar Shahzad */
    saveBirthHx: function (unload, attachToNote) {
        /* Change to fix EMR-251, setting timeout on the save functions
                Changed by: ZeeshanAK | Date: January 28,2015 */
        // allow user to save/update , when user has made any changes in the forum
        //change done: Azhar, Janurary 30,2016
        if ($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #ulSection li.active').text().indexOf('General') > -1) {
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsGeneralUpdate').val('true');
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsDeliveryUpdate').val('false');
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsNewbornUpdate').val('false');
        }
        else if ($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #ulSection li.active').text().indexOf('Maternal') > -1) {
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsGeneralUpdate').val('false');
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsDeliveryUpdate').val('true');
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsNewbornUpdate').val('false');
        }
        else if ($('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #ulSection li.active').text().indexOf('Information') > -1) {
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsGeneralUpdate').val('false');
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsDeliveryUpdate').val('false');
            $("#" + Clinical_BirthHx.params["PanelID"] + ' #frmClinicalBirthHx #hfIsNewbornUpdate').val('true');
        }
        if (EMRUtility.compareFormDataWithSerialized(Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx')) {
            setTimeout(function () {
                var strMessage = "";
                var self = $("#" + Clinical_BirthHx.params["PanelID"]);
                //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1457
                var isValid = true;

                var selectedId = $('#' + Clinical_BirthHx.params.PanelID + ' #ulSection').find('li.active').attr('id');

                if (selectedId == 'general' && $('#' + Clinical_BirthHx.params.PanelID + ' #PatientDOB').val() != '' && $('#' + Clinical_BirthHx.params.PanelID + ' #DateAdmitted').val() != '') {

                    var dob = Date.parse($('#' + Clinical_BirthHx.params.PanelID + ' #PatientDOB').val());
                    var doa = Date.parse($('#' + Clinical_BirthHx.params.PanelID + ' #DateAdmitted').val());

                    isValid = dob <= doa ? true : false;
                }
                if (selectedId == 'general' && isValid == false) {
                    utility.DisplayMessages("Date Admitted must be greater than Patient’s Date of Birth.", 3);
                    return;
                }
                //End 13-07-2016 Edit By Humaira Yousaf Bug#1457
                if (Clinical_BirthHx.params.mode == "Add") {

                    var myJSON = self.getMyJSONByName();

                    /* If mode = Add then send the save DB call
                       Author: Muhammad Azhar Shahzad */
                    Clinical_BirthHx.birthHxSave_DBCall(myJSON, Clinical_BirthHx.params.patientID).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_BirthHx.bindhiddenFields(response);
                            $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #btnHistory").removeClass('hidden');
                            Clinical_BirthHx.params.HxTypeId = response.BirthHxId;
                            $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #divSwitch").removeClass('disableAll');
                            if (attachToNote != false) {
                                Clinical_BirthHx.ChangeCurrentPast(1, null, null, null);
                                Clinical_BirthHx.BindCurrentBirthHxSoapText(response);
                            }
                            Clinical_BirthHx.params["BirthHxId"] = response.BirthHxId;
                            Clinical_BirthHx.params["mode"] = "Edit";
                            //if unremarkable is checked than all fiedls should be reset for childs of Birth history
                            if ($('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #chkBirthHxUnremarkable").is(":checked")) {
                                Clinical_BirthHx.resetBirthHx(true);
                            }
                            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                if (!attachToNote) {
                                    Clinical_BirthHx.getBirthHxInfo(unload, null, true);
                                }
                            }
                            utility.DisplayMessages(response.Message, 1);

                            $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').data('serialize', $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });

                }
                else if (Clinical_BirthHx.params.mode == "Edit") {
                    var myJSON = self.getMyJSONByName();

                    /* If mode = Edit then send the update DB call
                      Author: Muhammad Azhar Shahzad */
                    Clinical_BirthHx.birthHxUpdate_DBCall(myJSON, Clinical_BirthHx.params.BirthHxId).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            Clinical_BirthHx.bindhiddenFields(response);
                            $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #btnHistory").removeClass('hidden');
                            $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #divSwitch").removeClass('disableAll');
                            if (attachToNote != false) {
                                Clinical_BirthHx.ChangeCurrentPast(1, null, null, null);
                                Clinical_BirthHx.BindCurrentBirthHxSoapText(response);
                            }
                            //if unremarkable is checked than all fiedls should be reset for childs of Birth history
                            if ($('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #chkBirthHxUnremarkable").is(":checked")) {
                                Clinical_BirthHx.resetBirthHx(true);
                            }
                            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                if (!attachToNote) {
                                    Clinical_BirthHx.getBirthHxInfo(unload);
                                }
                            } else {
                                utility.DisplayMessages(response.Message, 1);
                            }
                            $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').data('serialize', $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').serialize());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });


                }

            }, 5);
            //End of Zeeshan's Change
        } else {
            utility.DisplayMessages("Please make any changes to save/update Birth History", 3);
        }
    },

    bindhiddenFields: function (response) {
        if (response.GeneralId > 0) {
            $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionGeneral #hfBirthHxGeneralId").val(response.GeneralId);
        }
        if (response.MaternalDeliveryId > 0) {
            $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionMaternalDelivery #hfBirthHxMaternalDeliveryId").val(response.MaternalDeliveryId);
        }
        if (response.NewbornId > 0) {
            $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionNewborn #hfBirthHxNewbornId").val(response.NewbornId);
        }
    },

    /* Call to load BirthHx component
       Author: Muhammad Azhar Shahzad */
    loadBirthHx: function (birthHxType) {
        Clinical_BirthHx.loadBirthHxComponent(birthHxType);
        $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #hfBirthHxType").val(birthHxType);
    },

    /* Load the requested BirthHx component received as argument
       Author: Muhammad Azhar Shahzad */
    loadBirthHxComponent: function (birthHxType) {
        Clinical_BirthHx.fillBirthHx_DBCall(birthHxType).done(function (response) {
            BackgroundLoaderShow(false);
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_BirthHx.BindCurrentBirthHxSoapText(response);
                // if BirthHx is Opening From Progress Note then Birth Hx Date Should Be disabled
                if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                    /* To disable date control if mode is edit */
                    $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #dtBirthHxDate").removeClass("disableAll");
                    /* To disable date control if mode is edit */
                }
                else {
                    $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #dtBirthHxDate").addClass("disableAll");
                }

                var birthhxDetail = JSON.parse(response.BirthHxFill_JSON);
                var birthhxGeneralDetail = JSON.parse(response.GeneralHxFill_JSON);
                var birthhxMaternalDeliveryDetail = JSON.parse(response.MaternalDeliveryHxFill_JSON);
                var birthhxNewBornDetail = JSON.parse(response.NewBornFill_JSON);
                var self = $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx");

                Clinical_BirthHx.overallComments = birthhxDetail.BirthHxComments || "";
                Clinical_BirthHx.unremarkable = birthhxDetail.BirthHxUnremarkable;
                Clinical_BirthHx.date = birthhxDetail.BirthHxDate || $('#' + Clinical_BirthHx.params.PanelID + " #dtBirthHxDate").val();

                if (birthhxDetail.BirthHxId > 0) {
                    Clinical_BirthHx.params.HxTypeId = birthhxDetail.BirthHxId;
                    $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #btnHistory").removeClass('hidden');

                    var detailsJSON = '';
                    if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        var cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON();
                        if (cachedJSON != '') {
                            detailsJSON = cachedJSON;
                        }
                        else {
                            detailsJSON = birthhxDetail;
                        }
                    }
                    else {
                        detailsJSON = birthhxDetail;
                    }

                    utility.bindMyJSONByName(true, detailsJSON, false, self).done(function () {
                        Clinical_BirthHx.params.BirthHxId = detailsJSON.BirthHxId;
                        Clinical_BirthHx.params.mode = "Edit";
                        //if unremarkable is checked than all fiedls should be reset for childs of Birth history
                        Clinical_BirthHx.unRemarkableBirthHx($('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #chkBirthHxUnremarkable"));

                    });
                    //if unremarkable is checked than all fiedls should be reset for childs of Birth history
                    if (birthhxDetail.unRemarkableBirthHx) {
                        Clinical_BirthHx.resetBirthHx(true);
                    } else {
                        if (birthhxGeneralDetail.GeneralId > 0) {
                            var detailsJSON = '';
                            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON('General');
                                if (cachedJSON != '') {
                                    detailsJSON = cachedJSON;
                                }
                                else {
                                    detailsJSON = birthhxGeneralDetail;
                                }
                            }
                            else {
                                detailsJSON = birthhxGeneralDetail;
                            }

                            utility.bindMyJSONByName(true, detailsJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionGeneral")).done(function () {
                                Clinical_BirthHx.enableDisableProviderLink(birthhxGeneralDetail);
                            });
                        }
                        else {
                            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON('General');
                                cachedJSON = cachedJSON || '';
                                utility.bindMyJSONByName(true, cachedJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionGeneral")).done(function () {
                                    Clinical_BirthHx.enableDisableProviderLink(cachedJSON);
                                });
                            }
                        }
                        if (birthhxMaternalDeliveryDetail.MaternalDeliveryId > 0) {
                            var detailsJSON = '';
                            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON('MaternalDelivery');
                                if (cachedJSON != '') {
                                    detailsJSON = cachedJSON;
                                }
                                else {
                                    detailsJSON = birthhxMaternalDeliveryDetail;
                                }
                            }
                            else {
                                detailsJSON = birthhxMaternalDeliveryDetail;
                            }

                            utility.bindMyJSONByName(true, detailsJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionMaternalDelivery"));
                        }
                        else {
                            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON('MaternalDelivery');
                                cachedJSON = cachedJSON || '';
                                utility.bindMyJSONByName(true, cachedJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionMaternalDelivery"));
                            }
                        }
                        if (birthhxNewBornDetail.NewbornId) {

                            var detailsJSON = '';
                            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON('NewBorn');
                                if (cachedJSON != '') {
                                    detailsJSON = cachedJSON;
                                }
                                else {
                                    detailsJSON = birthhxNewBornDetail;
                                }
                            }
                            else {
                                detailsJSON = birthhxNewBornDetail;
                            }
                            utility.bindMyJSONByName(true, detailsJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionNewborn")).done(function () {
                                if (birthhxNewBornDetail.bFetalDistress != null) {
                                    if (birthhxNewBornDetail.bFetalDistressYes) {
                                        $('#' + Clinical_BirthHx.params.PanelID + " #bFetalDistressYes").prop('checked', true);
                                    } else {
                                        $('#' + Clinical_BirthHx.params.PanelID + " #bFetalDistressNo").prop('checked', true);
                                    }
                                }

                            });
                        }
                        else {
                            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                                var cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON('NewBorn');
                                cachedJSON = cachedJSON || '';
                                utility.bindMyJSONByName(true, cachedJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionNewborn")).done(function () {
                                    if (cachedJSON.bFetalDistress != null) {
                                        if (cachedJSON.bFetalDistressYes) {
                                            $('#' + Clinical_BirthHx.params.PanelID + " #bFetalDistressYes").prop('checked', true);
                                        } else {
                                            $('#' + Clinical_BirthHx.params.PanelID + " #bFetalDistressNo").prop('checked', true);
                                        }
                                    }

                                });
                            }
                        }
                    }
                } else {
                    $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #btnHistory").addClass('hidden');
                    var $row = $('<tr/>');
                    $row.append('<td style="display:none;"></td><td>&nbsp;</td><td>No Known Birth History</td><td></td>');
                    $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvBirthHx tbody").html($row);
                    $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #divSwitch").addClass('disableAll');

                    var detailsJSON = '';
                    if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                        var cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON('General');
                        cachedJSON = cachedJSON || "";
                        utility.bindMyJSONByName(true, cachedJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionGeneral"));
                        //cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON('MaternalDelivery');
                        //utility.bindMyJSONByName(true, cachedJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionMaternalDelivery"));
                        //cachedJSON = Clinical_BirthHx.getCacheBirthHxJSON('NewBorn');
                        //utility.bindMyJSONByName(true, cachedJSON, false, $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionNewborn"));
                    }
                }
                $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').data('serialize', $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').serialize());
                Clinical_BirthHx.generalJSON = $('#' + Clinical_BirthHx.params.PanelID + " #sectionGeneral").getMyJSONByName();
                Clinical_BirthHx.maternityJSON = $('#' + Clinical_BirthHx.params.PanelID + " #sectionMaternalDelivery").getMyJSONByName();
                Clinical_BirthHx.newBornJSON = $('#' + Clinical_BirthHx.params.PanelID + " #sectionNewborn").getMyJSONByName();

                if (Clinical_HistorySummary.HistoryCacheList.BirthHx != null) {
                    $('#' + Clinical_BirthHx.params.PanelID + " #txtBirthHxComments").val(Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxComments);
                    $('#' + Clinical_BirthHx.params.PanelID + " #dtBirthHxDate").val(Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxDate);
                    var unremarkableValue = Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxUnremarkable;
                    $('#' + Clinical_BirthHx.params.PanelID + " #chkBirthHxUnremarkable").prop("checked", unremarkableValue);
                    if (unremarkableValue) {
                        Clinical_BirthHx.unRemarkableBirthHx($('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #chkBirthHxUnremarkable"));
                    }
                }
            }
            else {
                $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').data('serialize', $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx').serialize());
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    enableDisableProviderLink: function (birthhxGeneralDetail) {
        if (birthhxGeneralDetail.ResponsiblePhysicianId != "" && birthhxGeneralDetail.ResponsiblePhysicianId != null) {
            $('#' + Clinical_BirthHx.params.PanelID + " #hfProvider").val(birthhxGeneralDetail.ResponsiblePhysicianId);
            $('#' + Clinical_BirthHx.params.PanelID + " #txtProvider").val(birthhxGeneralDetail.ResponsiblePhysicianId_text);
            if ($('#' + Clinical_BirthHx.params.PanelID + " #lnkProviderEdit").css("display") == "none") {
                $('#' + Clinical_BirthHx.params.PanelID + " #lnkProviderEdit").css("display", "inline");
                $('#' + Clinical_BirthHx.params.PanelID + " #lblProvider").css("display", "none");
            }
        } else {
            $('#' + Clinical_BirthHx.params.PanelID + " #hfProvider").val("");
            if ($('#' + Clinical_BirthHx.params.PanelID + " #lnkProviderEdit").css("display") == "inline") {
                $('#' + Clinical_BirthHx.params.PanelID + " #lnkProviderEdit").css("display", "none");
                $('#' + Clinical_BirthHx.params.PanelID + " #lblProvider").css("display", "inline");
            }
        }
    },

    /* This function will handle unremarkable feature for Birth Hx
       Author: Muhammad Azhar Shahzad | Date: January 06,2015 */
    unRemarkableBirthHx: function (obj) {
        var isRemarkable = $(obj).prop("checked");
        if (isRemarkable == true) {
            $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #btnBirthHxSave').hide();
            $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #btnAddVitalsOnNote').hide();
            $('#' + Clinical_BirthHx.params.PanelID + ' #ulBirthHxTabsItems').addClass('disableAll');

            $('#' + Clinical_BirthHx.params.PanelID + ' #UnremarkableButton').show();
            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_BirthHx.params.PanelID + ' #btnAddBirthHxOnNoteOverlAll').hide();
                $('#' + Clinical_BirthHx.params.PanelID + ' #btnOverallBirthHxSave').hide();
            } else {
                $('#' + Clinical_BirthHx.params.PanelID + ' #btnAddBirthHxOnNoteOverlAll').hide();
                $('#' + Clinical_BirthHx.params.PanelID + ' #btnOverallBirthHxSave').show();
            }
            //By khaleel Ur Rehman to disable child section...Date:07 january 2016
            $('#' + Clinical_BirthHx.params.PanelID + ' #BirthHxChildSection').addClass('disableAll');

            if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_HistorySummary.HistoryCacheList.BirthHx != null && Clinical_HistorySummary.HistoryCacheList.BirthHx.length > 0 && Clinical_HistorySummary.HistoryCacheList.BirthHx.BirthHxUnremarkable == true) {
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxGeneral = {};
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxMaternalDelivery = {};
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxNewborn = {};
            }
        }
        else {
            $('#' + Clinical_BirthHx.params.PanelID + ' #UnremarkableButton').hide();
            $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #btnBirthHxSave').show();
            //By khaleel Ur Rehman to enable child section...Date:07 january 2016
            $('#' + Clinical_BirthHx.params.PanelID + ' #BirthHxChildSection').removeClass('disableAll');

        }
        Clinical_BirthHx.showSaveBtnsForBirthHx();
    },

    /* Implementing checkboxes functionality as radio boxes i.e. only one can be selected at a time
       Author: Muhammad Azhar Shahzad */
    checkUncheckFetalDistress: function (currentChkBox) {
        if ($(currentChkBox).is(':checked')) {
            $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #bFetalDistressNo,#" + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #bFetalDistressYes").prop('checked', false);
            $(currentChkBox).prop('checked', true);
        } else {
            $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #bFetalDistressNo,#" + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx #bFetalDistressYes").prop('checked', false);
        }
    },

    /* This function won't allow NumberOfLivingFetuses to be greater from NumberOfFetuses
       Author: Muhammad Azhar Shahzad */
    compareFetuses: function (obj) {
        var numberOfFetuses = $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #NumberOfFetuses').val();
        var numberOfLivingFetuses = $('#' + Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx #NumberOfLivingFetuses').val();
        if (numberOfFetuses != '' && numberOfLivingFetuses != '') {
            if (Number(numberOfFetuses) < Number(numberOfLivingFetuses)) {
                //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1513
                utility.DisplayMessages("Number of Living Fetuses cannot be greater than Number of Fetuses.", 3);
                //End 13-07-2016 Edit By Humaira Yousaf Bug#1513
                $(obj).focus();
                /* Change to fix EMR-251
                Changed by: ZeeshanAK | Date: January 28,2015 */
                $(obj).val("");
                //End of Zeeshan's Change
            }
        }
    },

    /* Opens Provider search popup
       Author: Muhammad Azhar Shahzad */
    openProvider: function () {
        var params = [];
        params["IsOptional"] = true;
        params["RefForm"] = "frmClinicalBirthHx";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_BirthHx";
        LoadActionPan('Admin_Provider', params);
    },

    /* Opens selected provider's detail
       Author: Muhammad Azhar Shahzad */
    openProviderDetail: function () {
        var params = [];
        params["ProviderId"] = $('#pnlClinicalBirthHx #hfProvider').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtProvider";
        params["ParentCtrl"] = "Clinical_BirthHx";
        params["IsOptional"] = true;
        LoadActionPan('providerDetail', params);
    },

    /* Code for progress note navigation. If user opens Birth History from Progress Note.
       Clicking close “X” button, a prompt message will be displayed
            “Are you want to save the changes?
            The date will be modified with current date.”
            i.	Clicking yes from the prompt will update the date as well as add the Birth history component on the progress notes.
            ii.	Clicking No will close the Birth history popup and will not add Birth history component on Progress notes.
       Author: Muhammad Azhar Shahzad */
    unLoadTab: function () {
        Clinical_BirthHx.bIsFirstLoad = true;
        var socialHxType = $('#' + Clinical_BirthHx.params.PanelID + " #hfBirthHxType").val();
        if (Clinical_BirthHx.params.ParentCtrl == "clinicalTabProgressNote") {
            if (EMRUtility.compareFormDataWithSerialized(Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx')) {


                utility.myConfirmNote('1', function () {
                    // var socialHxType = $('#' + Clinical_SocialHx.params.PanelID + " #frmClinicalSocialHx #hfSocialHxType").val();

                    Clinical_BirthHx.bNextPrev = true;
                    Clinical_BirthHx.addBirthHxToNotes();

                    Clinical_BirthHx.unloadBirthHistory();
                    if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalBirthHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabBirthHx', 'BirthHx');
                    }
                }, function () {

                    Clinical_BirthHx.bNextPrev = true;
                    Clinical_BirthHx.saveBirthHx(socialHxType, true);

                    Clinical_BirthHx.unloadBirthHistory();
                    if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalBirthHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabBirthHx', 'BirthHx');
                    }
                }, function () {
                    Clinical_BirthHx.unloadBirthHistory();
                    if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalBirthHx") {
                        UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                        Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabBirthHx', 'BirthHx');
                    }
                });

            } else {
                Clinical_BirthHx.unloadBirthHistory();
                if (Clinical_MedicalHx.params.PanelID == "pnlClinicalHistorySummary #pnlClinicalBirthHx") {
                    UnloadActionPan(Clinical_HistorySummary.params.ParentCtrl, 'Clinical_HistorySummary');
                    Clinical_HistorySummary.RemoveTabFromTabArray('clinicalTabBirthHx', 'BirthHx');

                }
            }
        } else {
            Clinical_BirthHx.unloadBirthHistory();
        }

    },

    /* Unloads BirthHx
       Author: Muhammad Azhar Shahzad */
    unloadBirthHistory: function () {
        if (Clinical_BirthHx.params["FromAdmin"] == "0") {
            if (Clinical_BirthHx.params != null && Clinical_BirthHx.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_BirthHx.params.ParentCtrl, 'Clinical_BirthHx');
            }
            else
                UnloadActionPan(null, 'Clinical_BirthHx');
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_BirthHx").remove();
            RemoveAdminTab();
        }
        EMRUtility.scrollToPNcomponent('clinical_birthhx');
    },

    /* This function will handle load of BirthHx and it's childs as specified by BirthHxType
       It represents service call to API
       Author: Muhammad Azhar Shahzad | Date: January 06,2015 */
    fillBirthHx_DBCall: function (birthHxType, BirthHxId) {
        var objData = new Object();
        objData["PatientId"] = Clinical_BirthHx.params.patientID;
        objData["BirthHxType"] = birthHxType != null ? birthHxType : "general";
        objData["birthHxSection"] = birthHxType != null ? birthHxType : "general";
        objData["BirthHxId"] = BirthHxId != null ? BirthHxId : 0;
        objData["commandType"] = "FILL_BirthHx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "birthHistory");
    },

    /* This function will handle save of BirthHx based on birthData and patientID
       It represents service call to API
       Author: Muhammad Azhar Shahzad | Date: January 06,2015 */
    birthHxSave_DBCall: function (birthData, patientID) {
        var objData = JSON.parse(birthData);
        objData["PatientId"] = patientID;
        objData.ResponsiblePhysicianId_text = objData.ResponsiblePhysicianId;
        objData.ResponsiblePhysicianId = objData.ProviderId;
        if (objData.ResponsiblePhysicianId == "") {
            objData.ProviderId = 0;
            objData.ProviderId = 0;
        }
        objData.BirthHxId = (objData.BirthHxId == "" ? 0 : objData.BirthHxId);
        objData.PatientId = (objData.PatientId == "" ? 0 : objData.PatientId);
        objData.NotesId = (objData.NotesId == "" ? 0 : objData.NotesId);
        objData.GeneralId = (objData.GeneralId == "" ? 0 : objData.GeneralId);
        objData.NewbornId = (objData.NewbornId == "" ? 0 : objData.NewbornId);
        objData.PatientBloodTypeId = (objData.PatientBloodTypeId == "" ? 0 : objData.PatientBloodTypeId);
        objData.ProblemsAtBirthId = (objData.ProblemsAtBirthId == "" ? 0 : objData.ProblemsAtBirthId);
        objData.MaternalDeliveryId = (objData.MaternalDeliveryId == "" ? 0 : objData.MaternalDeliveryId);
        objData.DeliveryMethodId = (objData.DeliveryMethodId == "" ? 0 : objData.DeliveryMethodId);
        objData.DeliveryPresentationId = (objData.DeliveryPresentationId == "" ? 0 : objData.DeliveryPresentationId);
        objData.MaternalHistoryId = (objData.MaternalHistoryId == "" ? 0 : objData.MaternalHistoryId);

        objData["commandType"] = "save_birthhx";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "birthHistory");
    },

    /* This function will handle update of BirthHx based on birthData and birthHxId
       It represents service call to API
       Author: Muhammad Azhar Shahzad | Date: January 06,2016 */
    birthHxUpdate_DBCall: function (birthData, birthHxId) {
        var objData = JSON.parse(birthData);
        objData["BirthHxId"] = birthHxId;
        objData["PatientId"] = Clinical_BirthHx.params.patientID;
        objData.ResponsiblePhysicianId_text = objData.ResponsiblePhysicianId;
        objData.ResponsiblePhysicianId = objData.ProviderId;
        if (objData.ResponsiblePhysicianId == "") {
            objData.ProviderId = 0;
            objData.ProviderId = 0;
        }
        objData.BirthHxId = (objData.BirthHxId == "" ? 0 : objData.BirthHxId);
        objData.PatientId = (objData.PatientId == "" ? 0 : objData.PatientId);
        objData.NotesId = (objData.NotesId == "" ? 0 : objData.NotesId);
        objData.GeneralId = (objData.GeneralId == "" ? 0 : objData.GeneralId);
        objData.NewbornId = (objData.NewbornId == "" ? 0 : objData.NewbornId);
        objData.PatientBloodTypeId = (objData.PatientBloodTypeId == "" ? 0 : objData.PatientBloodTypeId);
        objData.ProblemsAtBirthId = (objData.ProblemsAtBirthId == "" ? 0 : objData.ProblemsAtBirthId);
        objData.MaternalDeliveryId = (objData.MaternalDeliveryId == "" ? 0 : objData.MaternalDeliveryId);
        objData.DeliveryMethodId = (objData.DeliveryMethodId == "" ? 0 : objData.DeliveryMethodId);
        objData.DeliveryPresentationId = (objData.DeliveryPresentationId == "" ? 0 : objData.DeliveryPresentationId);
        objData.MaternalHistoryId = (objData.MaternalHistoryId == "" ? 0 : objData.MaternalHistoryId);

        objData["commandType"] = "UPDATE_BIRTHHX";
        var data = JSON.stringify(objData);
        // BirthHx parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "birthHistory");
    },

    /* -----------------Progress Note-------------
     Added on January 06,2015 by Muhammad Azhar Shahzad
     Reason: These functions are used for Progress Note Soap Attachment, creation and detachment */

    /* Call Back function to add component to Progress Note
       Author: Muhammad Azhar Shahzad */
    addBirthHxToNotes: function () {
        Clinical_BirthHx.saveBirthHx();
    },

    /* This function will get Birth History Soap Text and attach that to Progress note
      Author: Muhammad Azhar Shahzad */
    getBirthHxInfo: function (unloadBirthhx, BirthHxId, hideAlertMessage) {
        if (unloadBirthhx == null) {
            unloadBirthhx = false;
        }
        Clinical_BirthHx.fillBirthHx_DBCall(null, BirthHxId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_BirthHx.createBirthHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', unloadBirthhx, hideAlertMessage);
                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }
            }
        });
    },


    /* This Function will check, if Birth History SOAP is already attached in Progress note, if Birth History is not attached than it will create main divs to attach
      Author: Muhammad Azhar Shahzad */
    checkBirthHxExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_birthhx').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="BirthHxComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_birthhx title="Birth Hx"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'BirthHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="BirthHx">Birth Hx</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'BirthHx\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'BirthHx\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_birthhx> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_birthhx').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());

    },

    /* This Function is used to create SOAP html and append it to  Progress note
      Author: Muhammad Azhar Shahzad */
    createBirthHxBodyHTMLFromNotes: function (BirthHistory, noteHTMLCtrl, unloadBirthhx, hideAlertMessage) {
        Clinical_BirthHx.checkBirthHxExists();
        if (BirthHistory && BirthHistory.BirthHxId && BirthHistory.BirthHxId > 0) {
            var BirthHxFill_Obj = BirthHistory;
            var $mainDivBirthHx = $(document.createElement('div'));

            var birthHxId = BirthHxFill_Obj.BirthHxId;
            if (birthHxId > 0) {
                var $SectionBodyBirthHx = $(document.createElement('section'));
                $SectionBodyBirthHx.attr('id', "Cli_BirthHx_Main" + birthHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_BirthHx_" + birthHxId);
                var $ListBirthHx = $(document.createElement('ul'));

                //$ListBirthHx.attr('class', 'list-unstyled line-height-fix')
                $ListBirthHx.attr('class', 'list-unstyled');//kr

                $SectionBodyBirthHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_BirthHx_" + birthHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_BirthHx_Main" + birthHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                $ListBirthHx.append("<li>" + BirthHxFill_Obj.SoapText + "</li>");
                $DetailsDiv.append($ListBirthHx);
                $SectionBodyBirthHx.append($DetailsDiv);
                if ($(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId).length == 0) {
                    $mainDivBirthHx.append($SectionBodyBirthHx);

                    var birthHxHtml = $mainDivBirthHx.html();
                    $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().addClass('initialVisitBody');
                    if (birthHxHtml != '') {
                        $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().append(birthHxHtml);
                    }
                    return birthHxId;
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId).html($SectionBodyBirthHx.html());
                    $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId + ' ul').append(CommentHTML);


                }

            }
        }
    },


    /* This Function is used to create SOAP html and append it to  Progress note
      Author: Muhammad Azhar Shahzad */
    createBirthHxBodyHTML: function (response, noteHTMLCtrl, unloadBirthhx, hideAlertMessage) {
        Clinical_BirthHx.checkBirthHxExists();
        if (response.BirthHxFill_JSON != null && response.BirthHxFill_JSON != '') {
            var BirthHxFill_Obj = JSON.parse(response.BirthHxFill_JSON);
            var $mainDivBirthHx = $(document.createElement('div'));

            var birthHxId = BirthHxFill_Obj.BirthHxId;
            if (birthHxId > 0) {
                var $SectionBodyBirthHx = $(document.createElement('section'));
                $SectionBodyBirthHx.attr('id', "Cli_BirthHx_Main" + birthHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_BirthHx_" + birthHxId);
                var $ListBirthHx = $(document.createElement('ul'));

                //$ListBirthHx.attr('class', 'list-unstyled line-height-fix')
                $ListBirthHx.attr('class', 'list-unstyled');//kr

                $SectionBodyBirthHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_BirthHx_" + birthHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_BirthHx_Main" + birthHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                //$SectionBodyBirthHx.append(' <div><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_BirthHx_" + birthHxId + '"><i class="fa fa-edit"></i></a>' +
                //'<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_BirthHx_Main" + birthHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                $ListBirthHx.append("<li>" + BirthHxFill_Obj.BirthHxSoapText + "</li>");
                $DetailsDiv.append($ListBirthHx);
                $SectionBodyBirthHx.append($DetailsDiv);
                if ($(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId).length == 0) {
                    $mainDivBirthHx.append($SectionBodyBirthHx);
                    Clinical_BirthHx.updateBirthHxHtml($mainDivBirthHx.html(), birthHxId, noteHTMLCtrl, hideAlertMessage);
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId).html($SectionBodyBirthHx.html());
                    $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId + ' ul').append(CommentHTML);
                    Clinical_ProgressNote.saveComponentSOAPText("BirthHx", hideAlertMessage);
                    Clinical_BirthHx.updateBirthHxHtml("", birthHxId, noteHTMLCtrl, hideAlertMessage);

                }

                if (unloadBirthhx == true) {
                    Clinical_BirthHx.unloadBirthHistory();
                }
            }
        }
    },

    createBirthHxBodyHTMLFromNote: function (response, noteHTMLCtrl, unloadBirthhx, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_BirthHx.checkBirthHxExists();
        if (response) {
            var BirthHxFill_Obj = response;
            var $mainDivBirthHx = $(document.createElement('div'));

            var birthHxId = BirthHxFill_Obj.BirthHxId;

            if (birthHxId > 0) {
                var $SectionBodyBirthHx = $(document.createElement('section'));
                $SectionBodyBirthHx.attr('id', "Cli_BirthHx_Main" + birthHxId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_BirthHx_" + birthHxId);
                var $ListBirthHx = $(document.createElement('ul'));

                //$ListBirthHx.attr('class', 'list-unstyled line-height-fix')
                $ListBirthHx.attr('class', 'list-unstyled');//kr

                $SectionBodyBirthHx.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_BirthHx_" + birthHxId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_BirthHx_Main" + birthHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                //$SectionBodyBirthHx.append(' <div><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_BirthHx_" + birthHxId + '"><i class="fa fa-edit"></i></a>' +
                //'<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_BirthHx_Main" + birthHxId + '"  ><i class="fa fa-times"></i></a></div> ');

                $ListBirthHx.append("<li>" + BirthHxFill_Obj.BirthHxSoapText + "</li>");
                $DetailsDiv.append($ListBirthHx);
                $SectionBodyBirthHx.append($DetailsDiv);
                if ($(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId).length == 0) {
                    $mainDivBirthHx.append($SectionBodyBirthHx);
                    var birthHxHtml = $mainDivBirthHx.html();
                    if (birthHxHtml != '') {
                        $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().addClass('initialVisitBody');
                        $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().append(birthHxHtml);
                    }
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId).html($SectionBodyBirthHx.html());
                    $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().find('#Cli_BirthHx_Main' + birthHxId + ' ul').append(CommentHTML);
                }
            }
        }
        dfd.resolve();
        return dfd;
    },

    /* This Function is called by Progress Notes (Fill BirthHx Func, CopyAllNotesCategories)
      Author: Muhammad Azhar Shahzad */
    updateBirthHxHtml: function (birthHxHtml, birthHxId, noteHTMLCtrl, hideAlertMessage) {
        $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().addClass('initialVisitBody');
        if (birthHxHtml != '') {
            $(noteHTMLCtrl + ' clinical_birthhx').parent().parent().append(birthHxHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (birthHxHtml != '') {
            Clinical_BirthHx.attachBirthHxFromNotes(birthHxId, hideAlertMessage);
        }

    },

    /* This Function detach Birth History From progress note
     Author: Muhammad Azhar Shahzad */
    detach_ComponentsBirthHx: function (componentName, isUpdate, birthHxComponentRemove) {

        var Clinical_BirthHxIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_birthhx').parent().parent().find('section[id*="Cli_BirthHx_Main"]').map(function () {
            return this.id.replace("Cli_BirthHx_Main", "");
        }).get().join(',');

        if (birthHxComponentRemove) {

            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_birthhx').parent().parent().attr('NoteComponentId');
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Birth Hx']").remove();
            if (Clinical_ProgressNote.params["TemplateName"])
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_birthhx').parent().parent().addClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_birthhx').parent().parent().remove();
            var hxComponents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .HxComponent').length;

            if (NoteComponentId && NoteComponentId != "NCDummyId" && hxComponents == 0) {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_birthhx').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('BirthHx', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_birthhx').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            }

        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_birthhx').parent().parent().find('section[id*="Cli_BirthHx_Main"]').remove();
        }

        if (Clinical_BirthHxIds == "" || Clinical_BirthHxIds == "undefined") {
            Clinical_ProgressNote.saveComponentSOAPText("BirthHx", true);
            //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            utility.DisplayMessages('Successfully Deleted', 1);
        }
        else {
            Clinical_BirthHx.detachBirthHxFromNotes_DBCall(Clinical_BirthHxIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (isUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText("BirthHx", true);
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


    /* This Functions ask for Detaching Birth Hx from Progress Note for current Patient Selected
    Author: Muhammad Azhar Shahzad */
    detachBirthHxFromNotes: function (birthHxId) {
        utility.myConfirm('1', function () {
            EMRUtility.scrollToPNcomponent('clinical_birthhx');
            var selectedValue = birthHxId.replace('Cli_BirthHx_Main', '');
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_BirthHx.detachBirthHxFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $('#' + birthHxId).remove();
                        Clinical_ProgressNote.Add_NoText();
                        Clinical_ProgressNote.saveComponentSOAPText("BirthHx", true);
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
    },

    /* This Functions attached Birth Hx to Progress Note for current Patient Selected
    Author: Muhammad Azhar Shahzad */
    attachBirthHxFromNotes: function (birthHxId, hideAlertMessage) {
        var selectedValue = birthHxId;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            Clinical_BirthHx.attachBirthHxFromNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    //If Attached BirthHx Made new inseration to BirthHx Table than good ids should be attached to HTML
                    Clinical_ProgressNote.saveComponentSOAPText("BirthHx", hideAlertMessage);
                    $('#' + birthHxId).remove();
                    // utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },


    /* If BirthHx Component which is dropeed in Progress note has no BirthHx attached, than it will call for Latest BirthHx for this patient
    Author: Muhammad Azhar Shahzad */
    getLatestBirthHxByPatientId: function (hideAlertMessage, droppedComponent) {

        Clinical_BirthHx.getLatestClinical_BirthHxByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_BirthHx.createBirthHxBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, hideAlertMessage);
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
        objData["ComponentType"] = "BirthHistory";
        objData["commandType"] = "getautopopulatesetting";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");

    },

    /* DB Call to detach BirthHx from the Notes
    Author: Muhammad Azhar Shahzad */
    detachBirthHxFromNotes_DBCall: function (birthHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["BirthHxId"] = birthHxId;
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
        objData["commandType"] = "detach_birthhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "birthHistory");
    },

    /* DB Call to attach BirthHx from the Notes
    Author: Muhammad Azhar Shahzad */
    attachBirthHxFromNotes_DBCall: function (birthHxId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["BirthHxId"] = birthHxId;
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
        objData["commandType"] = "attach_birthhx_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "birthHistory");
    },

    /* Retrieves latest BirthHx data against the PatientId
    Author: Muhammad Azhar Shahzad */
    getLatestClinical_BirthHxByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "getlatest_birthhxby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "HISTORY", "birthHistory");
    },

    ChangeCurrentPast: function (obj, PrimaryID, PageNumber, ResultPerPage) {

        if ($(obj).attr('status') == '1' || obj == 1) {
            $(obj).attr('status', 0);
            $('#' + Clinical_BirthHx.params.PanelID + " #pnlCurrent ").addClass("hidden");
            $('#' + Clinical_BirthHx.params.PanelID + " #pnlPast ").removeClass("hidden");
            Clinical_BirthHx.fillhxLog(PrimaryID, PageNumber, ResultPerPage).done(function (response) {
                if (response != "") {
                    var json = JSON.parse(response);
                    Clinical_BirthHx.gridLoad(response);
                    var TableControl = Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvPastBirthHx";
                    var PagingPanelControlID = Clinical_BirthHx.params.PanelID + " #dgvPastBirthHx_Paging";
                    var ClassControlName = "Clinical_BirthHx";
                    var PagesToDisplay = 5;
                    var iTotalDisplayRecords = json.iTotalDisplayRecords;
                    setTimeout(
                        CreatePagination(json.HxLogSoapCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                            Clinical_BirthHx.ChangeCurrentPast(1, PrimaryID, PageNumber, ResultPerPage);
                        }), 10);
                }
            });


        } else {
            $(obj).attr('status', 1);

            $('#' + Clinical_BirthHx.params.PanelID + " #pnlPast").addClass("hidden");
            $('#' + Clinical_BirthHx.params.PanelID + " #pnlCurrent  ").removeClass("hidden");
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
        objData["HxId"] = Clinical_BirthHx.params.HxTypeId;
        objData["HxType"] = "BirthHx";
        objData["Status"] = "All";
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = ResultPerPage;
        objData["commandType"] = "get_hx_log";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "HISTORY", "HistorySummary");
    },

    gridLoad: function (response) {
        var isactive = $('#' + Clinical_BirthHx.params.PanelID + ' #pnlBirthHx_Result #divSwitch #switchActive').attr('isactive');

        //Start 24-05-2016 Muhammad Arshad Remove Duplicate search issue on Datatable
        if ($.fn.dataTable.isDataTable("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvPastBirthHx")) {
            $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvPastBirthHx").dataTable().fnClearTable();
            $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvPastBirthHx").dataTable().fnDestroy();
            $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvPastBirthHx tbody").find("tr").remove();
        }
        var logCount = JSON.parse(response);
        if (logCount.HxLogSoapCount > 0) {
            var LoadJSONData = JSON.parse(logCount.HxLogSoap_JSON); //Parsing array to JSON
            var counter = null;
            for (var i = 0; i < LoadJSONData.length; i++) {
                // $.each(LoadJSONData, function (i, item) {
                var $row = $('<tr/>');
                // $row.attr("onclick", "Clinical_BirthHx.CDSEdit('" + item.CDSId + "',event);");
                //$row.attr("id", "gvCDS_row" + item.CDSId);
                var text = LoadJSONData[i].SoapText;

                counter = i;
                $row.append('<td style="display:none;">' + counter + '</td><td>' + LoadJSONData[i].Action + '</td><td id=sptxt>' + $('<a/>').html($('<a/>').html(text).text()).text() + '</td><td>' + LoadJSONData[i].ModifiedOn + " " + LoadJSONData[i].ModifiedBy + '</td>');
                $row.find('#sptxt').html($('<a/>').html(text).text());
                $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvPastBirthHx tbody").last().append($row);
                //  });
            }
        }
        else {
            $("#" + Clinical_BirthHx.params.PanelID + ' #pnlBirthHx_Result #dgvPastBirthHx').DataTable({
                "destroy": true,
                "language": {
                    "emptyTable": "No Known Birth History"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bInfo": false, "bPaginate": false, "bSortable": false, "aTargets": [0] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_BirthHx.params.PanelID + ' #pnlBirthHx_Result #dgvPastBirthHx'))
            ;
        else {
            $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvPastBirthHx").DataTable({ "destroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [[0, "asc"]], "aoColumnDefs": [{ "bInfo": false, "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        }

        $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvPastBirthHx_filter").remove();
    },

    //--------------end progress Note-----------

    /// Author: ZeeshanAK
    /// Purpose:  Compare DOB and Admission date | Fix for EMR-1516
    /// Date : 14 July, 2016
    checkDobAndAdmitDates: function (obj) {
        var patientDOB = new Date($("#" + Clinical_BirthHx.params["PanelID"] + " #PatientDOB").val());
        var admitDate = new Date($("#" + Clinical_BirthHx.params["PanelID"] + " #DateAdmitted").val());
        if (patientDOB.length != 0 && admitDate.length != 0) {
            if (admitDate < patientDOB) {
                utility.DisplayMessages("Admission Date cannot be more recent than Date Of Birth", 3);
                $(obj).val('');
            }
        }
    },


    BindCurrentBirthHxSoapText: function (resopnse) {
        var $row = $('<tr/>');
        $row.append('<td style="display:none;">' + resopnse.BirthHxId + '</td><td>' + resopnse.IsCreatedOrModified + '</td><td>' + resopnse.SoapText + '</td><td>' + resopnse.LastUpdated + '</td>');
        $("#" + Clinical_BirthHx.params.PanelID + " #pnlBirthHx_Result #dgvBirthHx tbody").html($row);
        if ($('#' + Clinical_BirthHx.params.PanelID + ' #pnlBirthHx_Result #divSwitch #switchVisit').attr('status') == '1') {
            $('#' + Clinical_BirthHx.params.PanelID + ' #pnlCurrent').removeClass('hidden');
            $('#' + Clinical_BirthHx.params.PanelID + ' #pnlPast').addClass('hidden');
        }
    },

    viewHistory: function (cntrl) {
        var birthHxId = Clinical_BirthHx.params.BirthHxId;
        if (birthHxId != null && birthHxId > 0) {
            var primaryKeyId = -1;
            var TableName = "";
            if ($('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionGeneral").is(':visible')) {
                primaryKeyId = $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionGeneral #hfBirthHxGeneralId").val();
                TableName = 'BirthHx_General';
            } else if ($('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionMaternalDelivery").is(':visible')) {
                primaryKeyId = $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionMaternalDelivery #hfBirthHxMaternalDeliveryId").val();
                TableName = 'BirthHx_MaternalDelivery';
            } else if ($('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionNewborn").is(':visible')) {
                primaryKeyId = $('#' + Clinical_BirthHx.params.PanelID + " #frmClinicalBirthHx  #sectionNewborn #hfBirthHxNewbornId").val();
                TableName = 'BirthHx_NewBorn';
            } else {
                primaryKeyId = birthHxId;
                TableName = 'BirthHx';
            }
            if (primaryKeyId != null && primaryKeyId > 0) {
                EMRUtility.showCurrentItemHistory(Clinical_BirthHx.params.PanelID, null, primaryKeyId, TableName, null, ((Clinical_BirthHx.params.ParentCtrl != "clinicalTabProgressNote") && (Clinical_BirthHx.params.ParentCtrl != "clinicalTabFaceSheet")) ? Clinical_BirthHx.params.TabID : "Clinical_BirthHx", null);
            }
        } else {
            $(cntrl).addClass('hidden');
        }
    },

    cacheBirthHxJSON: function (type, jsonData, saveParentOnly) {
        //if (EMRUtility.compareFormDataWithSerialized(Clinical_BirthHx.params.PanelID + ' #frmClinicalBirthHx') ) {
        var dfd = $.Deferred();

        var IsGeneralUpdate = false;
        var IsDeliveryUpdate = false;
        var IsNewbornUpdate = false;
        jsonData = JSON.parse(jsonData);
        jsonData.ProviderId = $('#' + Clinical_BirthHx.params.PanelID + " #hfProvider").val();
        jsonData.BirthHxId = $('#' + Clinical_BirthHx.params.PanelID + " #hfBirthHxId").val();
        jsonData.PatientId = $('#' + Clinical_BirthHx.params.PanelID + " #hfPatientId").val();
        jsonData.NotesId = Clinical_BirthHx.params.NotesId;
        jsonData.GeneralId = $('#' + Clinical_BirthHx.params.PanelID + " #hfBirthHxGeneralId").val();
        jsonData.NewbornId = $('#' + Clinical_BirthHx.params.PanelID + " #hfBirthHxNewbornId").val();
        jsonData.PatientBloodTypeId = $('#' + Clinical_BirthHx.params.PanelID + " #PatientBloodTypeId").val();
        jsonData.ProblemsAtBirthId = $('#' + Clinical_BirthHx.params.PanelID + " #ProblemsAtBirthId").val();
        jsonData.MaternalDeliveryId = $('#' + Clinical_BirthHx.params.PanelID + " #hfBirthHxMaternalDeliveryId").val();
        jsonData.DeliveryMethodId = $('#' + Clinical_BirthHx.params.PanelID + " #DeliveryMethodId").val();
        jsonData.DeliveryPresentationId = $('#' + Clinical_BirthHx.params.PanelID + " #DeliveryPresentationId").val();
        jsonData.MaternalHistoryId = $('#' + Clinical_BirthHx.params.PanelID + " #MaternalHistoryId").val();

        jsonData.ResponsiblePhysicianId_text = jsonData.ResponsiblePhysicianId;
        jsonData.ResponsiblePhysicianId = jsonData.ProviderId;
        if (jsonData.ResponsiblePhysicianId == "") {
            jsonData.ProviderId = 0;
            jsonData.ProviderId = 0;
        }
        jsonData.BirthHxId = (jsonData.BirthHxId == "" ? 0 : jsonData.BirthHxId);
        jsonData.PatientId = (jsonData.PatientId == "" ? 0 : jsonData.PatientId);
        jsonData.NotesId = (jsonData.NotesId == "" ? 0 : jsonData.NotesId);
        jsonData.GeneralId = (jsonData.GeneralId == "" ? 0 : jsonData.GeneralId);
        jsonData.NewbornId = (jsonData.NewbornId == "" ? 0 : jsonData.NewbornId);
        jsonData.PatientBloodTypeId = (jsonData.PatientBloodTypeId == "" ? 0 : jsonData.PatientBloodTypeId);
        jsonData.ProblemsAtBirthId = (jsonData.ProblemsAtBirthId == "" ? 0 : jsonData.ProblemsAtBirthId);
        jsonData.MaternalDeliveryId = (jsonData.MaternalDeliveryId == "" ? 0 : jsonData.MaternalDeliveryId);
        jsonData.DeliveryMethodId = (jsonData.DeliveryMethodId == "" ? 0 : jsonData.DeliveryMethodId);
        jsonData.DeliveryPresentationId = (jsonData.DeliveryPresentationId == "" ? 0 : jsonData.DeliveryPresentationId);
        jsonData.MaternalHistoryId = (jsonData.MaternalHistoryId == "" ? 0 : jsonData.MaternalHistoryId);

        var GeneralObj = null;
        var MaternalDeliveryObj = null;
        var NewBornObj = null;
        if (type == "General" && saveParentOnly != true) {
            IsGeneralUpdate = true;
            GeneralObj = {};
            GeneralObj.GeneralId = jsonData.GeneralId;
            GeneralObj.BirthHxId = jsonData.BirthHxId;
            GeneralObj.HospitalName = jsonData.HospitalName;
            GeneralObj.PatientDOB = jsonData.PatientDOB;
            GeneralObj.LengthStayatHospital = jsonData.LengthStayatHospital;
            GeneralObj.DateAdmitted = jsonData.DateAdmitted;
            GeneralObj.ObstetricianName = jsonData.ObstetricianName;
            GeneralObj.PediatricianName = jsonData.PediatricianName;
            GeneralObj.ResponsiblePhysicianId = jsonData.ResponsiblePhysicianId;
            GeneralObj.GeneralComments = jsonData.GeneralComments;
            GeneralObj.ResponsiblePhysicianId_text = jsonData.ResponsiblePhysicianId_text;
        }

        else if (type == "MaternalDelivery") {
            IsDeliveryUpdate = true;
            MaternalDeliveryObj = {};
            MaternalDeliveryObj.MaternalDeliveryId = jsonData.MaternalDeliveryId;
            MaternalDeliveryObj.BirthHxId = jsonData.BirthHxId;
            MaternalDeliveryObj.Gestation = jsonData.Gestation;
            MaternalDeliveryObj.NumberOfFetuses = jsonData.NumberOfFetuses;
            MaternalDeliveryObj.NumberOfLivingFetuses = jsonData.NumberOfLivingFetuses;
            MaternalDeliveryObj.LaborLength = jsonData.LaborLength;
            MaternalDeliveryObj.DeliveryMethodId = jsonData.DeliveryMethodId;
            MaternalDeliveryObj.DeliveryPresentationId = jsonData.DeliveryPresentationId;
            MaternalDeliveryObj.MaternalHistoryId = jsonData.MaternalHistoryId;
            MaternalDeliveryObj.MaternalHistoryId_text = jsonData.MaternalHistoryId_text;
            MaternalDeliveryObj.DeliveryPresentationId_text = jsonData.DeliveryPresentationId_text;
            MaternalDeliveryObj.DeliveryMethodId_text = jsonData.DeliveryMethodId_text;
            MaternalDeliveryObj.MaternalDeliveryComments = jsonData.MaternalDeliveryComments
        }
        else if (type == "NewBorn") {
            IsNewbornUpdate = true;
            NewBornObj = {};
            NewBornObj.NewbornId = jsonData.NewbornId;
            NewBornObj.BirthHxId = jsonData.BirthHxId;
            NewBornObj.HeadCircumference = jsonData.HeadCircumference;
            NewBornObj.ChestCircumference = jsonData.ChestCircumference;
            NewBornObj.WeightAtBirth = jsonData.WeightAtBirth;
            NewBornObj.LengthAtBirth = jsonData.LengthAtBirth;
            NewBornObj.ApgarAtBirth = jsonData.ApgarAtBirth;
            NewBornObj.ApgarAt5Minutes = jsonData.ApgarAt5Minutes;
            NewBornObj.WeightReleased = jsonData.WeightReleased;
            NewBornObj.PatientBloodTypeId = jsonData.PatientBloodTypeId;
            NewBornObj.ProblemsAtBirthId = jsonData.ProblemsAtBirthId;
            NewBornObj.bFetalDistress = jsonData.bFetalDistress;
            NewBornObj.bFetalDistressYes = jsonData.bFetalDistressYes;
            NewBornObj.bFetalDistressNo = jsonData.bFetalDistressNo
            NewBornObj.NewbornComments = jsonData.NewbornComments
            NewBornObj.PatientBloodTypeId_text = jsonData.PatientBloodTypeId_text;
            NewBornObj.ProblemsAtBirthId_text = jsonData.ProblemsAtBirthId_text
        }

        if (Clinical_HistorySummary.HistoryCacheList.BirthHx == null) {
            Clinical_HistorySummary.HistoryCacheList.BirthHx = [];
            var patientId;

            if (Clinical_BirthHx.params.patientID == null) {
                patientId = $('#PatientProfile #hfPatientId').val();
            } else {
                patientId = Clinical_BirthHx.params.patientID;
            }

            var BirthHxData = {
                BirthHxId: $('#' + Clinical_BirthHx.params.PanelID + " #hfBirthHxId").val(),
                PatientId: Clinical_BirthHx.params.patientID,
                BirthHxDate: $('#' + Clinical_BirthHx.params.PanelID + " #dtBirthHxDate").val(),
                BirthHxUnremarkable: $('#' + Clinical_BirthHx.params.PanelID + " #chkBirthHxUnremarkable").prop("checked"),
                BirthHxComments: $('#' + Clinical_BirthHx.params.PanelID + " #txtBirthHxComments").val(),
                IsGeneralUpdate: IsGeneralUpdate,
                IsDeliveryUpdate: IsDeliveryUpdate,
                IsNewbornUpdate: IsNewbornUpdate,
                BirthHxType: type,
                NotesId: Clinical_BirthHx.params.NotesId,
                BirthHxGeneral: GeneralObj,
                BirthHxMaternalDelivery: MaternalDeliveryObj,
                BirthHxNewborn: NewBornObj,
            }
            Clinical_HistorySummary.HistoryCacheList.BirthHx.push(BirthHxData);
        }
        else {
            if (Clinical_HistorySummary.HistoryCacheList.BirthHx.length > 0) {
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxId = $('#' + Clinical_BirthHx.params.PanelID + " #hfBirthHxId").val();
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].PatientId = Clinical_BirthHx.params.patientID;
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxDate = $('#' + Clinical_BirthHx.params.PanelID + " #dtBirthHxDate").val();
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxUnremarkable = $('#' + Clinical_BirthHx.params.PanelID + " #chkBirthHxUnremarkable").prop("checked");
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxComments = $('#' + Clinical_BirthHx.params.PanelID + " #txtBirthHxComments").val();
                Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxType = type;
                if (type == "General") {
                    Clinical_HistorySummary.HistoryCacheList.BirthHx[0].IsGeneralUpdate = IsGeneralUpdate;
                    Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxGeneral = GeneralObj;
                }
                else if (type == "MaternalDelivery") {
                    Clinical_HistorySummary.HistoryCacheList.BirthHx[0].IsDeliveryUpdate = IsDeliveryUpdate;
                    Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxMaternalDelivery = MaternalDeliveryObj;
                }
                else if (type == "NewBorn") {
                    Clinical_HistorySummary.HistoryCacheList.BirthHx[0].IsNewbornUpdate = IsNewbornUpdate;
                    Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxNewborn = NewBornObj;
                }
            }
        }

        dfd.resolve();
        return dfd;
        //}
        //else {
        //    Clinical_HistorySummary.HistoryCacheList.BirthHx = null;
        //}
    },

    getCacheBirthHxJSON: function (type) {
        if (Clinical_HistorySummary.HistoryCacheList.BirthHx) {

            if (type == "General") {
                return Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxGeneral;
            }
            else if (type == "MaternalDelivery") {
                return Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxMaternalDelivery;
            }
            else if (type == "NewBorn" || type == "Newborn") {
                return Clinical_HistorySummary.HistoryCacheList.BirthHx[0].BirthHxNewborn;
            }
            else {
                return Clinical_HistorySummary.HistoryCacheList.BirthHx[0];
            }

        }

        return '';
    },


    //cacheBirthHxJSON: function (type, jsonData) {

    //    var typeIndex = -1;
    //    if (Clinical_HistorySummary.HistoryCacheList.BirthHx == null) {
    //        Clinical_HistorySummary.HistoryCacheList.BirthHx = [];
    //    }

    //    $.grep(Clinical_HistorySummary.HistoryCacheList.BirthHx, function (item, index) {
    //        if (item.Type == type) {
    //            typeIndex = index;
    //            return;
    //        }
    //    });

    //    if (typeIndex != -1) {
    //        Clinical_HistorySummary.HistoryCacheList.BirthHx[typeIndex].JSON = jsonData;
    //    }
    //    else {
    //        var jsonData = {
    //            Type: type,
    //            JSON: jsonData
    //        };

    //        Clinical_HistorySummary.HistoryCacheList.BirthHx.push(jsonData);
    //    }
    //},

    //getCacheBirthHxJSON: function (type) {
    //    if (Clinical_HistorySummary.HistoryCacheList.BirthHx != null) {
    //        var birthData = $.grep(Clinical_HistorySummary.HistoryCacheList.BirthHx, function (item, index) {
    //            if (item.Type == type) {
    //                return item;
    //            }
    //        });

    //        if (birthData.length > 0) {
    //            return birthData[0].JSON;
    //        }
    //    }

    //    return '';
    //},

    deleteCacheDisease: function (type) {
        var typeIndex = -1;
        $.grep(Clinical_HistorySummary.HistoryCacheList.BirthHx, function (item, index) {
            if (item.Type == type) {
                typeIndex = index;
                return;
            }
        });

        if (typeIndex != -1) {
            Clinical_HistorySummary.HistoryCacheList.BirthHx.splice(typeIndex, 1);
        }
    },

}