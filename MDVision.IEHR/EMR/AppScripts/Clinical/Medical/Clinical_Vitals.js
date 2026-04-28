Clinical_Vitals = {
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    Load: function (params) {

        Clinical_Vitals.params = params;

        $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        if (Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet") {
            Clinical_Vitals.params.patientID = Clinical_FaceSheet.params.patientID;
            $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        //Clinical_Vitals.EditableGrid = utility.MakeEditableGrid(PanelVitalsGrid, VitalsGridId, Clinical_Vitals, "0", false, false, false, false);
        if (Clinical_Vitals.params.mode == null) {
            Clinical_Vitals.params.mode = "Add";
        }
        if (Clinical_Vitals.params.PanelID != 'pnlClinicalVitals') {
            Clinical_Vitals.params.PanelID = Clinical_Vitals.params.PanelID + ' #pnlClinicalVitals';
        } else {
            Clinical_Vitals.params.PanelID = 'pnlClinicalVitals';
        }
        var VitalSignId = "";
        Clinical_Vitals.ResetFormData();
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        if (Clinical_Vitals.params.mode == "Add" || Clinical_Vitals.params.VitalSignId == null || Clinical_Vitals.params.VitalSignId == "" || Clinical_Vitals.params.VitalSignId == "-1") {
            VitalSignId = "-1";
            if (Clinical_Vitals.bIsFirstLoad == true) {
                $('#' + Clinical_Vitals.params.PanelID + ' #BloodPressure').loadDropDowns(true).done(function () {

                    $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                });
                //Clinical_Vitals.ResetFormData();
                //Add default rows
                //Clinical_Vitals.AddNewRow('BloodPressure', Clinical_Vitals.params.PanelID + ' #divTemplateBloodPressure', VitalSignId);
                //Clinical_Vitals.AddNewRow('Pulse', Clinical_Vitals.params.PanelID + ' #divTemplatePulse', VitalSignId);
                //Clinical_Vitals.AddNewRow('Temprature', Clinical_Vitals.params.PanelID + ' #divTemplateTemprature', VitalSignId);
                //Clinical_Vitals.AddNewRow('Respiration', Clinical_Vitals.params.PanelID + ' #divTemplateRespiration', VitalSignId);

                //Clinical_Vitals.AddNewRow('Oxygen', 'divTemplateOxygen', VitalSignId);
                Clinical_Vitals.bIsFirstLoad = false;
                //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1398
                $('#' + Clinical_Vitals.params.PanelID + ' #btnAddNew').addClass('hidden');
                //End 13-07-2016 Edit By Humaira Yousaf Bug#1398
            }

        }
        else if (Clinical_Vitals.params.mode == "Edit") {
            VitalSignId = Clinical_Vitals.params.VitalSignId;
            Clinical_Vitals.VitalsEdit(VitalSignId);
        }


        Clinical_Vitals.ValidateVitals();



        //Load Dropdown

        var self = $('#' + Clinical_Vitals.params.PanelID);

        $('#' + Clinical_Vitals.params.PanelID + ' #BloodPressure').loadDropDowns(true).done(function () { });
        self.loadDropDowns(true).done(function () {
            Clinical_Vitals.VitalsSearch();
            //Clinical_Vitals.FillAllDropDowns(VitalSignId);
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
            if (Clinical_Vitals.params.ParentCtrl == "clinicalTabFaceSheet") {
                $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals").data('serialize', $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals").serialize());
            }
            /* End 15/12/2015 Muhammad Irfan to serialize form data for facesheet  */

        });

        //end Load Dropdown
        utility.CreateDatePicker(Clinical_Vitals.params.PanelID + '  #dpVitalsDate', function () {
            $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals").bootstrapValidator('revalidateField', 'VitalSignDate');
        }, true);
        $('#' + Clinical_Vitals.params.PanelID + ' #tpVitalsTime').timepicker().on('changeTime.timepicker', function (e) {
            showTimeZone: false;
            disableFocus: false;
            $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals").bootstrapValidator('revalidateField', 'VitalsTime');
        });

        if ($('#' + Clinical_Vitals.params.PanelID + ' #PatientProfile #hfPatientId').val() != "") {
            $('#' + Clinical_Vitals.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        if (Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet") {
            $('#' + Clinical_Vitals.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }

        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
            EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Vitals.params.PanelID, 'Medical', 'Vitals', 'Clinical_Vitals.unLoadTab();', null, true);
        }
        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_Vitals.params.PanelID + " div#FaceSheetPager", Clinical_Vitals.params.FaceSheetComponents, 'vitals');
        } else if (Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlClinicalVitals' + " div#FaceSheetPager", Clinical_Vitals.params.FaceSheetComponents, 'vitals');
        }
        Clinical_Vitals.domReadyFunction();
        utility.callbackAfterAllDOMLoaded(function () {
            $("#frmClinicalVitals").data('serialize', $("#frmClinicalVitals").serialize());
            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1356px");
            }
        });
    },

    domReadyFunction: function () {
        $(function () {
            $('#' + Clinical_Vitals.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};

                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });
            //EMR-70 Bug number Resolution
            $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals [data-plugin-keyboard-numpad]').keyboard({
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
                            Clinical_Vitals.ValidateHeightFeet(e, keyboard.$preview);
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
            $('#' + Clinical_Vitals.params.PanelID + ' #txtComment').on("click", function (e) {
                if (!$('#' + Clinical_Vitals.params.PanelID + ' #MacroDescDetails').is(":hidden")) {
                    $('#' + Clinical_Vitals.params.PanelID + ' #MacroDescDetails').hide();
                }
            });
            $('#' + Clinical_Vitals.params.PanelID + ' #txtComment').on("keyup", function (e) {

                if (e.keyCode == 190 || e.keyCode == 110) // dot key is pressed
                {
                    e.preventDefault();
                    if ($('#' + Clinical_Vitals.params.PanelID + ' #txtComment').find("#marker").length > 0) {
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtComment').find("#marker").remove();
                    }
                    EMRUtility.pasteHtmlAtCaret('<span id=marker></span>');
                    if (EMRUtility.callAutopopulationOrNot(Clinical_Vitals.params.PanelID, "txtComment")) {
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtComment').focus();
                        EMRUtility.AutoKeyWordPopulateForDiv(Clinical_Vitals.params.PanelID, "txtComment", "Vitals", 0);
                    }
                    else {
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtComment').find("#marker").remove();
                    }
                }
            });
        });

    },
    //Dropdown Functions

    FillAllDropDowns: function (VitalSignId) {

        //var dfd = new $.Deferred();
        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlSmokingStatus', 'GetAllVitalSigns', true, 'Smoking Status');
        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlSeverityofPain', 'GetAllVitalSigns', true, 'Severity of Pain');
        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlBloodType', 'GetAllVitalSigns', true, 'Blood Type');
        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlSmokingStatus', 'GetAllVitalSigns', true, 'Smoking Status');


        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlPosition' + VitalSignId, 'GetAllVitalSigns', true, 'Position');
        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlCuffLocation' + VitalSignId, 'GetAllVitalSigns', true, 'Cuff Location');
        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlCuffSize' + VitalSignId, 'GetAllVitalSigns', true, 'Cuff Size');
        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlRythm' + VitalSignId, 'GetAllVitalSigns', true, 'Rhythm');
        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlPattern' + VitalSignId, 'GetAllVitalSigns', true, 'Pattern');
        CacheManager.BindDropDownsByEntityID('#pnlClinicalVitals #ddlMethod' + VitalSignId, 'GetAllVitalSigns', true, 'Method');
        //    .done(function () {
        //    dfd.resolve(true);
        //});

        //return dfd.promise();
    },

    InitializeAllTimeWidget: function (VitalSignId, RowType, event) {
        //Start 11-07-2016 Humaira Yousaf
        $('#pnlClinicalVitals #tpBloodPressureTime' + VitalSignId).timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });
        if (parseInt(VitalSignId) <= 0) {
            $('#pnlClinicalVitals #tpBloodPressureTime' + VitalSignId).timepicker('setTime', new Date($('#userCurrentTime').html()));
            $('#pnlClinicalVitals #tpPulseTime' + VitalSignId).timepicker('setTime', new Date($('#userCurrentTime').html()));
            $('#pnlClinicalVitals #tpTempratureTime' + VitalSignId).timepicker('setTime', new Date($('#userCurrentTime').html()));
            $('#pnlClinicalVitals #tpRespirationTime' + VitalSignId).timepicker('setTime', new Date($('#userCurrentTime').html()));
        }

        $('#pnlClinicalVitals #tpPulseTime' + VitalSignId).timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });

        $('#pnlClinicalVitals #tpTempratureTime' + VitalSignId).timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });

        $('#pnlClinicalVitals #tpRespirationTime' + VitalSignId).timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });
        //End 11-07-2016 Humaira Yousaf
        if (RowType == "BloodPressure") {
            Clinical_Vitals.BindNumPad("#BloodPressure" + VitalSignId);
        }
        else if (RowType == "Pulse") {
            Clinical_Vitals.BindNumPad("#Pulse" + VitalSignId);
        }
        else if (RowType == "Temprature") {
            Clinical_Vitals.BindNumPad("#Temprature" + VitalSignId);
        }
        else if (RowType == "Respiration") {
            Clinical_Vitals.BindNumPad("#Respiration" + VitalSignId);
        }
    },

    //End Dropdown Functions

    CopyVitals: function (VitalSignsId) {
        Clinical_Vitals.params.mode = "Add";
        $('#pnlClinicalVitals #btnsave').text('Add');
        $('#pnlClinicalVitals #dpVitalsDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
        $('#pnlClinicalVitals #tpVitalsTime').timepicker('setTime', new Date($('#userCurrentTime').html()));
        //$("#txtSystolic10").attr("id").replace($("#txtSystolic10").attr("id").match(/(\d+)/)[1],"-1")
        //Clear Blood Pressure HTML ids
        $("#pnlClinicalVitals #frmClinicalVitals #BloodPressureIds").val("");
        var BloodPressureInitialDivId = -1;
        $('#pnlClinicalVitals #frmClinicalVitals div[id*="BloodPressure"]').each(function () {
            if ($(this).attr("id").indexOf("Template") < 0) {
                $(this).attr("id", "divBloodPressure" + BloodPressureInitialDivId);
                $(this).find("input,select").each(function () {
                    if ($(this).attr("id") != null && $(this).attr("id") != "" && $(this).attr("id").indexOf("Template") < 0) {
                        var NewId = $(this).attr("id").replace($(this).attr("id").match(/(\d+)/)[1], BloodPressureInitialDivId);
                        // Id is already in negative then no need to change it
                        if (NewId.indexOf("--") < 0) {
                            $(this).attr("id", NewId);
                        }
                    }
                    if ($(this).attr("name") != null && $(this).attr("name") != "" && $(this).attr("name").indexOf("Template") < 0) {
                        var NewName = $(this).attr("name").replace($(this).attr("name").match(/(\d+)/)[1], BloodPressureInitialDivId);
                        // NAme is already in negative then no need to change it
                        if (NewName.indexOf("--") < 0) {
                            $(this).attr("name", NewName);
                        }
                    }
                });
                BloodPressureInitialDivId = BloodPressureInitialDivId - 1;
            }
        });

        var BloodPressureInitialId = -1;
        $('#pnlClinicalVitals #frmClinicalVitals input[id*="BPId"]').each(function () {
            if ($(this).attr("id").indexOf("Template") < 0) {
                $(this).attr("id", "BPId" + BloodPressureInitialId);
                $(this).attr("name", "BPId" + BloodPressureInitialId);
                $(this).val(BloodPressureInitialId);
                BloodPressureInitialId = BloodPressureInitialId - 1;
            }
        });

        //Clear Pulse HTML ids
        $("#pnlClinicalVitals #frmClinicalVitals #PulseIds").val("");
        var PulseInitialDivId = -1;
        $('#pnlClinicalVitals #frmClinicalVitals div[id*="Pulse"]').each(function () {
            if ($(this).attr("id").indexOf("Template") < 0) {
                $(this).attr("id", "divPulse" + PulseInitialDivId);
                $(this).find("input,select").each(function () {
                    if ($(this).attr("id") != null && $(this).attr("id") != "" && $(this).attr("id").indexOf("Template") < 0) {
                        var NewId = $(this).attr("id").replace($(this).attr("id").match(/(\d+)/)[1], PulseInitialDivId);
                        // Id is already in negative then no need to change it
                        if (NewId.indexOf("--") < 0) {
                            $(this).attr("id", NewId);
                        }

                    }
                    if ($(this).attr("name") != null && $(this).attr("name") != "" && $(this).attr("name").indexOf("Template") < 0) {
                        var NewName = $(this).attr("name").replace($(this).attr("name").match(/(\d+)/)[1], PulseInitialDivId);
                        // NAme is already in negative then no need to change it
                        if (NewName.indexOf("--") < 0) {
                            $(this).attr("name", NewName);
                        }

                    }
                });
                PulseInitialDivId = PulseInitialDivId - 1;
            }
        });

        var PulseInitialId = -1;
        $('#pnlClinicalVitals #frmClinicalVitals input[id*="PulsId"]').each(function () {
            if ($(this).attr("id").indexOf("Template") < 0) {
                $(this).attr("id", "PulsId" + PulseInitialId);
                $(this).attr("name", "PulsId" + PulseInitialId);
                $(this).val(PulseInitialId);
                PulseInitialId = PulseInitialId - 1;
            }
        });

        //Clear Temprature HTML ids
        $("#pnlClinicalVitals #frmClinicalVitals #TempratureIds").val("");
        var TempratureInitialDivId = -1;
        $('#pnlClinicalVitals #frmClinicalVitals div[id*="Temprature"]').each(function () {
            if ($(this).attr("id").indexOf("Template") < 0) {
                $(this).attr("id", "divTemprature" + TempratureInitialDivId);
                $(this).find("input,select").each(function () {

                    if ($(this).attr("id") != null && $(this).attr("id") != "" && $(this).attr("id").indexOf("Template") < 0) {
                        var NewId = $(this).attr("id").replace($(this).attr("id").match(/(\d+)/)[1], TempratureInitialDivId);
                        // Id is already in negative then no need to change it
                        if (NewId.indexOf("--") < 0) {
                            $(this).attr("id", NewId);
                        }
                    }
                    if ($(this).attr("name") != null && $(this).attr("name") != "" && $(this).attr("name").indexOf("Template") < 0) {
                        var NewName = $(this).attr("name").replace($(this).attr("name").match(/(\d+)/)[1], TempratureInitialDivId);
                        // NAme is already in negative then no need to change it
                        if (NewName.indexOf("--") < 0) {
                            $(this).attr("name", NewName);
                        }
                    }
                });
                TempratureInitialDivId = TempratureInitialDivId - 1;
            }
        });

        var TempratureInitialId = -1;
        $('#pnlClinicalVitals #frmClinicalVitals input[id*="TempId"]').each(function () {
            if ($(this).attr("id").indexOf("Template") < 0) {
                $(this).attr("id", "TempId" + TempratureInitialId);
                $(this).attr("name", "TempId" + TempratureInitialId);
                $(this).val(TempratureInitialId);
                TempratureInitialId = TempratureInitialId - 1;
            }
        });

        //Clear Respiration HTML ids
        $("#pnlClinicalVitals #frmClinicalVitals #RespirationIds").val("");
        var RespirationInitialDivId = -1;
        $('#pnlClinicalVitals #frmClinicalVitals div[id*="Respiration"]').each(function () {
            if ($(this).attr("id").indexOf("Template") < 0) {
                $(this).attr("id", "divRespiration" + RespirationInitialDivId);
                $(this).find("input,select").each(function () {
                    if ($(this).attr("id") != null && $(this).attr("id") != "" && $(this).attr("id").indexOf("Template") < 0) {
                        var NewId = $(this).attr("id").replace($(this).attr("id").match(/(\d+)/)[1], RespirationInitialDivId);
                        // Id is already in negative then no need to change it
                        if (NewId.indexOf("--") < 0) {
                            $(this).attr("id", NewId);
                        }
                    }
                    if ($(this).attr("name") != null && $(this).attr("name") != "" && $(this).attr("name").indexOf("Template") < 0) {
                        var NewName = $(this).attr("name").replace($(this).attr("name").match(/(\d+)/)[1], RespirationInitialDivId);
                        // NAme is already in negative then no need to change it
                        if (NewName.indexOf("--") < 0) {
                            $(this).attr("name", NewName);
                        }
                    }
                });
                RespirationInitialDivId = RespirationInitialDivId - 1;
            }
        });

        var RespirationInitialId = -1;
        $('#pnlClinicalVitals #frmClinicalVitals input[id*="RespId"]').each(function () {
            if ($(this).attr("id").indexOf("Template") < 0) {
                $(this).attr("id", "RespId" + RespirationInitialId);
                $(this).attr("name", "RespId" + RespirationInitialId);
                $(this).val(RespirationInitialId);
                RespirationInitialId = RespirationInitialId - 1;
            }
        });

    },

    AddNewRow: function (RowType, TemplateRowId, RowId, CurrentItemJSON, event) {

        //Begin 22-09-2016 Added By Abid Ali Bug# QAC1-121
        var scrollTopPx = $(window).scrollTop();
        //End 22-09-2016 Added By Abid Ali Bug# QAC1-121

        //Begin 22-09-2016 Added By Abid Ali Bug# QAC1-87
        if (TemplateRowId.indexOf('pnl') < 0) {
            TemplateRowId = Clinical_Vitals.params.PanelID + " #" + TemplateRowId;
        }
        //End 22-09-2016 Added By Abid Ali Bug# QAC1-87

        var regexpr = new RegExp(RowType, "g");
        if (TemplateRowId != null && TemplateRowId != "") {
            var TemplateRow = $("#" + TemplateRowId);
            var newRow = TemplateRow.clone().css("display", "");
            if (RowId != null && parseInt(RowId) < 0) {
                var LastChildRowId = $('#' + Clinical_Vitals.params.PanelID + " section#" + RowType + " div[id*='" + RowType + "']:last").attr("id");
                if (LastChildRowId.indexOf("divTemplate") > -1) {
                    RowId = RowType + "-1";
                }
                else {

                    //Begin 21-09-2016 Added By Abid Ali Bug#QAC1-55
                    LastChildRowId = $('#' + Clinical_Vitals.params.PanelID + " section#" + RowType + " div[id*='" + RowType + "']:eq(1)").attr("id");
                    //End 21-09-2016 Added By Abid Ali Bug#QAC1-55

                    var intRowId = LastChildRowId.replace(regexpr, "");//RowId=RowId.replace(/BloodPressure/g,"")
                    if (intRowId < 0) {
                        RowId = RowType + (intRowId - 1);
                    }
                    else {
                        RowId = RowType + "-1";
                    }
                }
            }
            else if (RowId != null && parseInt(RowId) > 0) {
                RowId = RowType + RowId;
            }
            newRow.attr("id", RowId);
            var intCurrentRowId = RowId.replace(regexpr, "");

            newRow.find("input,select").each(function () {
                if ($(this).attr("id") != null && $(this).attr("id") != "" && $(this).attr("id").indexOf("Template") > -1) {
                    var currentId = $(this).attr("id").replace("Template", intCurrentRowId);
                    $(this).attr("id", currentId);
                }
                if ($(this).attr("name") != null && $(this).attr("name") != "" && $(this).attr("name").indexOf("Template") > -1) {
                    var currentId = $(this).attr("name").replace("Template", intCurrentRowId);
                    $(this).attr("name", currentId);
                }
            });
            if (RowType == "BloodPressure") {
                newRow.find("input[id*='BPId']").val(intCurrentRowId);
            }
            else if (RowType == "Pulse") {
                newRow.find("input[id*='PulsId']").val(intCurrentRowId);
            }
            else if (RowType == "Temprature") {
                newRow.find("input[id*='TempId']").val(intCurrentRowId);
            }
            else if (RowType == "Respiration") {
                newRow.find("input[id*='RespId']").val(intCurrentRowId);
            }
            //newRow.insertAfter($("section#" + RowType + " div[id*='" + RowType + "']:last"))
            ////Insert Child row on top of existing row
            newRow.insertAfter($("#" + TemplateRowId));
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            if (CurrentItemJSON != null && CurrentItemJSON != "") {
                utility.bindMyJSONByName(true, CurrentItemJSON, false, newRow).done(function () {


                    //$('#pnlClinicalVitals #btnsave').text('Update');
                    //Clinical_Vitals.params["VitalSignsId"] = VitalSignsId;
                    //Clinical_Vitals.params["mode"] = "Edit";
                });


            }
            // To be held for sometime later
            //Clinical_Vitals.FillAllDropDowns(intCurrentRowId);
            Clinical_Vitals.InitializeAllTimeWidget(intCurrentRowId, RowType);
        }

        //Begin 22-09-2016 Added By Abid Ali Bug# QAC1-121
        $(window).scrollTop(scrollTopPx);
        //End 22-09-2016 Added By Abid Ali Bug# QAC1-121

    },

    BindNumPad: function (DivId) {
        // bug # EMR-108 Clicking in the blanks fields should display the number pad.

        $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals ' + DivId).find('[data-plugin-keyboard-numpad]').keyboard({
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
            appendLocally: this,
            restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
            preventPaste: true,  // prevent ctrl-v and right click
            usePreview: false,
            autoAccept: true,
            tabNavigation: true
        })
                .addTyping();
    },

    ValidateVitals: function () {
        $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals').bootstrapValidator('destroy');
        $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   VitalSignDate: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   VitalSignDate11: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   VitalsTime: {
                       group: '.col-sm-4',
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
            Clinical_Vitals.VitalsSave();
        });
    },
    //Start//07/01/2016//Ahmad Raza//Resolved Bug# EMR-98
    isDetailExists: function (section) {
        var DetailExists = false;
        var sectionDetails = "";
        for (var i = 1; i < section.length; i++) {

            if (section[i].id == "BloodPressure") {
                sectionDetails = "BloodPressure";
            }
            else if (section[i].id == "Pulse") {
                sectionDetails = "Pulse";
            }
            else if (section[i].id == "Temprature") {
                sectionDetails = "Temprature";
            }
            else if (section[i].id == "Respiration") {

                sectionDetails = "Respiration";
            }
            else if (section[i].id == "General") {

                sectionDetails = "General";
            }
            else if (section[i].id == "Oxygen") {
                sectionDetails = "Oxygen";
            }
            else if (section[i].id == "Structure") {
                sectionDetails = "Structure";
            }

            if (sectionDetails != "") {
                var sys = "";
                var dys = "";
                var self = $('#' + Clinical_Vitals.params.PanelID + ' section#' + sectionDetails).find('[type=text]:not([data-plugin-timepicker]),textarea,select').each(function () {

                    if ($(this).prop("disabled") != true) {

                        var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                        if (sectionDetails == "BloodPressure") {
                            if ($(this).attr("id").indexOf("txtSystolic") > -1) {
                                sys = $(this).val();
                            }
                            if ($(this).attr("id").indexOf("txtDiastolic") > -1) {
                                dys = $(this).val();
                            }
                        }
                        if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {

                            DetailExists = true;
                        }

                        if (currentElementTagName.toLowerCase() == 'select' && $(this).val() != null && $(this).val() != "") {
                            DetailExists = true;
                        }

                    }
                });

                if ((sys == "" && dys != "") || (sys != "" && dys == "")) {
                    DetailExists = false;
                }

            }

        }

        return DetailExists;

    },
    //Start//07/01/2016//Ahmad Raza//Resolved Bug# EMR-98

    VitalsSave: function () {
        var dfd = $.Deferred();
        //Begin 12-2-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
        var isNonEmpty = false;
        $($('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals section#BloodPressure div#divTemplateBloodPressure").nextAll().toArray().reverse()).each(function (i, item) {
            var BPFields = $(this).find("input[id*='Systolic'],input[id*='Diastolic']");
            if ($(BPFields[0]).val() != "" && $(BPFields[1]).val() != "") {
                if (isNonEmpty == false) {
                    isNonEmpty = true;
                }
            }
            else {
                if (isNonEmpty == false) {
                    isNonEmpty = true;
                }
                else {
                    $(this).remove();
                }
            }
        });

        var isNonEmpty = false;
        $($('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals section#Pulse div#divTemplatePulse").nextAll().toArray().reverse()).each(function (i, item) {
            var BPFields = $(this).find("input[id*='Result']");
            if ($(BPFields[0]).val() != "") {
                if (isNonEmpty == false) {
                    isNonEmpty = true;
                }
            }
            else {
                if (isNonEmpty == false) {
                    isNonEmpty = true;
                }
                else {
                    $(this).remove();
                }
            }
        });

        var isNonEmpty = false;
        $($('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals section#Temprature div#divTemplateTemprature").nextAll().toArray().reverse()).each(function (i, item) {
            var BPFields = $(this).find("input[id*='Result']");
            if ($(BPFields[0]).val() != "") {
                if (isNonEmpty == false) {
                    isNonEmpty = true;
                }
            }
            else {
                if (isNonEmpty == false) {
                    isNonEmpty = true;
                }
                else {
                    $(this).remove();
                }
            }
        });

        var isNonEmpty = false;
        $($('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals section#Respiration div#divTemplateRespiration").nextAll().toArray().reverse()).each(function (i, item) {
            var BPFields = $(this).find("input[id*='Result']");
            if ($(BPFields[0]).val() != "") {
                if (isNonEmpty == false) {
                    isNonEmpty = true;
                }
            }
            else {
                if (isNonEmpty == false) {
                    isNonEmpty = true;
                }
                else {
                    $(this).remove();
                }
            }
        });
        //End 12-2-2015 Muhammad Arshad Bug # EMR-100 Vitals Workflow in Clinical Module -> Add Vitals-> Blank rows
        var selfBloodPressure = $('#' + Clinical_Vitals.params.PanelID + " #BloodPressure");
        var myBloodPressureJSON = selfBloodPressure.getMyJSONByName();
        var isBPRowsValid = true;
        $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals section#BloodPressure input').each(function () {
            if ($(this).css("border-bottom-color").indexOf("rgb(255") > -1) {
                isBPRowsValid = false;
                return false;
            }
        });

        var BloodPressureIds = $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals input[id*="BPId"]').map(function () {
            return this.id.replace("BPId", "");
        }).get().join(',');

        $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #BloodPressureIds").val(BloodPressureIds);

        var PulseIds = $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals input[id*="PulsId"]').map(function () {
            return this.id.replace("PulsId", "");
        }).get().join(',');

        $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #PulseIds").val(PulseIds);

        var TempratureIds = $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals input[id*="TempId"]').map(function () {
            return this.id.replace("TempId", "");
        }).get().join(',');

        $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #TempratureIds").val(TempratureIds);

        var RespirationIds = $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals input[id*="RespId"]').map(function () {
            return this.id.replace("RespId", "");
        }).get().join(',');

        $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #RespirationIds").val(RespirationIds);

        //BEGIN 23-12-2015 Muhammad Arshad Bug# EMR-98 Vitals Workflow in Clinical Module -> Add Vitals-> Add Button
        var isVitalsValid = false;

        //if ((($('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #dpVitalsDate").val() != "") && ($('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #tpVitalsTime").val() != "")) && ($('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtSystolic-1").val() != "" || $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtDiastolic-1").val() != "" || $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtPulseResult-1").val() != "" || $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtTemperatureResult-1").val() != "" || $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #ddlSeverityofPain").val() != "" || $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtComments").val() != "")) {
        //   isVitalsValid = true;

        // }
        //END 23-12-2015 Muhammad Arshad Bug# EMR-98 Vitals Workflow in Clinical Module -> Add Vitals-> Add Button

        //Start//07/01/2016//Ahmad Raza//Bug# 98 resolved
        isVitalsValid = Clinical_Vitals.isDetailExists($('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals section'));

        if ($('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #dpVitalsDate").val() == "" || $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #tpVitalsTime").val() == "") {
            isVitalsValid = false;
        }
        //End//07/01/2016//Ahmad Raza//Bug# 98 resolved
        if (isVitalsValid == true && isBPRowsValid == true) {
            if (Clinical_Vitals.params.mode == "Add" && Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
                $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #hfIsFromNote").val(1);
            }
            else if (Clinical_Vitals.params.mode == "Add") {
                $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #hfIsFromNote").val(0);
            }
            var strMessage = "";
            if (Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet") {

                $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #hfPatientId").val(Clinical_FaceSheet.params.patientID);
            }
            var self = $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals");
            if (self.find("#txtComment").text()) {
                self.find("#txtComments").val(self.find("#txtComment").text()); // add comments in  existing field
            }
            else {
                self.find("#txtComments").val("");
            }
            var myJSON = self.getMyJSONByName();
            //return false;
            if (Clinical_Vitals.params.mode == "Add") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("Medical_Vitals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        Clinical_Vitals.SaveVitals(myJSON, "", false).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                $("#mainForm  li#CDSAlert").show();
                                var patientId = Clinical_Vitals.params.patientID == null ? $('#PatientProfile #hfPatientId').val() : Clinical_Vitals.params.patientID;
                                $.when(ClinicalCDSDetail.showCDSAlert("", patientId)).then(function () {
                                    if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                                        Clinical_ProgressNote.LoadCDSAlerts();
                                });
                                //if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
                                //    var cDate = response.VitalsSignDate;
                                //    var nDate = cDate.split('T')[0];
                                //    var p1 = nDate.split('-')[1];
                                //    var p2 = nDate.split('-')[2];
                                //    var p3 = nDate.split('-')[0];
                                //    var fDate = p1 + "/" + p2 + "/" + p3;
                                //    Clinical_Vitals.FillVitals(response.VitalsId, null, null, null, true, fDate);
                                //    //Start 29-06-2016 Humaira Yousaf to close popup
                                //    UnloadActionPan(Clinical_Vitals.params["ParentCtrl"], "Clinical_Vitals");
                                //    //End 29-06-2016 Humaira Yousaf to close popup

                                //} else {

                                //Clinical_Vitals.VitalsSearch(response.VitalsId);
                                //Clinical_Vitals.AppointmentStatusSearch(response.VitalSignId);

                                Clinical_Vitals.VitalsSearch();
                                utility.callbackAfterAllDOMLoaded(function () {
                                    var newlyAddedRow = $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals tbody tr#' + response.VitalsId + ' td')[0];
                                    $(newlyAddedRow).find('input').trigger('click');
                                });

                                utility.DisplayMessages(response.message, 1);
                                //$("#divTemplateRespiration").nextAll().remove();
                                //$("#divTemplateTemprature").nextAll().remove();
                                //$("#divTemplatePulse").nextAll().remove();
                                //$("#divTemplateBloodPressure").nextAll().remove();
                                ////Add default rows
                                //Clinical_Vitals.AddNewRow('BloodPressure', 'divTemplateBloodPressure', "-1");
                                //Clinical_Vitals.AddNewRow('Pulse', 'divTemplatePulse', "-1");
                                //Clinical_Vitals.AddNewRow('Temprature', 'divTemplateTemprature', "-1");
                                //Clinical_Vitals.AddNewRow('Respiration', 'divTemplateRespiration', "-1");
                                Clinical_Vitals.ResetFormData();

                                //Clinical_Vitals.params.mode = "Add";
                                //$('#pnlClinicalVitals #btnsave').text('Add');
                                //$('#pnlClinicalVitals #dpVitalsDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
                                //$('#pnlClinicalVitals #tpVitalsTime').timepicker('setTime', new Date());
                                //}
                                //Clinical_Vitals.VitalsEdit(response.VitalsId);
                                dfd.resolve();
                            }
                            else {
                                dfd.resolve();
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else {
                        dfd.resolve();
                        utility.DisplayMessages(strMessage, 2);
                    }
                });
                //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
            }
            else if (Clinical_Vitals.params.mode == "Edit") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("Medical_Vitals", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_Vitals.params.VitalSignId == Clinical_Vitals.params.NoteVitalSignId && (Clinical_Vitals.params.VitalSignId > 0 && Clinical_Vitals.params.NoteVitalSignId > 0)) {
                            //Start//17-02-2016//Ahmad Raza//Fixed Bug#344
                            //utility.myConfirm('16', function () {

                            // $('#' + Clinical_Vitals.params.PanelID + ' #tpVitalsTime').timepicker('setTime', new Date().toLocaleTimeString());
                            //  $('#' + Clinical_Vitals.params.PanelID + ' #dpVitalsDate').datepicker("setDate", new Date().toLocaleDateString());

                            var self = $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals");
                            var myJSON = self.getMyJSONByName();
                            $.when(Clinical_Vitals.updateVitalRecord(myJSON)).then(function () {
                                dfd.resolve();
                            });
                            //Start 29-06-2016 Humaira Yousaf to close popup
                            //utility.callbackAfterAllDOMLoaded(function () { UnloadActionPan(Clinical_Vitals.params["ParentCtrl"], "Clinical_Vitals"); });

                            //End 29-06-2016 Humaira Yousaf to close popup
                            //}, function () {
                            //    //Clinical_Vitals.UnLoadTab();    //fixed bug EMR-1305
                            //    dfd.resolve();

                            //}, '1'
                            //);
                            //End//17-02-2016//Ahmad Raza//Fixed Bug#344
                        } else
                            if ($('#' + Clinical_Vitals.params.PanelID + ' #hfNoteId').val() != '' && parseInt($('#' + Clinical_Vitals.params.PanelID + ' #hfNoteId').val()) > 0 && Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
                                utility.myConfirm('16', function () {
                                    var self = $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals");
                                    var myJSON = self.getMyJSONByName();
                                    $.when(Clinical_Vitals.updateVitalRecord(myJSON)).then(function () {
                                        dfd.resolve();
                                    });
                                    Clinical_Vitals.ResetFormData();
                                }, function () {
                                    //exception handling
                                    if (Clinical_Vitals.params.VitalSignsId != null) {
                                        $.when(Clinical_Vitals.VitalsEdit(Clinical_Vitals.params.VitalSignsId)).then(function () {
                                            dfd.resolve();
                                        });
                                    } else {
                                        $.when(Clinical_Vitals.VitalsEdit(Clinical_Vitals.params.VitalSignId)).then(function () {
                                            dfd.resolve();
                                        });
                                    }
                                    //  Clinical_Vitals.UnLoadTab();
                                }, '1'
                                    );
                            } else {
                                var self = $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals");
                                var myJSON = self.getMyJSONByName();
                                $.when(Clinical_Vitals.updateVitalRecord(myJSON)).then(function () {
                                    dfd.resolve();
                                });
                            }

                    }
                    else {
                        dfd.resolve();
                        utility.DisplayMessages(strMessage, 2);
                    }
                });
                //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
            }
        }
        else {
            //BEGIN 23-12-2015 Muhammad Arshad Bug# EMR-98 Vitals Workflow in Clinical Module -> Add Vitals-> Add Button
            if (isVitalsValid == false) {
                utility.DisplayMessages("Please enter any value", 3);
            }
            //END 23-12-2015 Muhammad Arshad Bug# EMR-98 Vitals Workflow in Clinical Module -> Add Vitals-> Add Button
            dfd.resolve();
        }
        return dfd;
    },

    updateVitalRecord: function (myJSON) {
        var dfd = $.Deferred();
        var vitalDetail = JSON.parse(myJSON);
        if (myJSON != "{}") {
            Clinical_Vitals.UpdateVitals(myJSON, Clinical_Vitals.params.VitalSignsId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    dfd.resolve();
                    //Clinical_Vitals.AppointmentStatusSearch(Clinical_Vitals.params.VitalSignsId);
                    utility.DisplayMessages(response.message, 1);
                    $("#mainForm  li#CDSAlert").show();
                    var patientId = Clinical_Vitals.params.patientID == null ? $('#PatientProfile #hfPatientId').val() : Clinical_Vitals.params.patientID;
                    $.when(ClinicalCDSDetail.showCDSAlert("", patientId)).then(function () {
                        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                            Clinical_ProgressNote.LoadCDSAlerts();
                    });
                    if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote" && Clinical_Vitals.params.VitalSignId == Clinical_Vitals.params.NoteVitalSignId) {
                        //Start//28-07-2016//Ahmad Raza//logic to prevent previous vital's attachment with note
                        //if (Date.parse(vitalDetail.VitalSignDate) < Date.parse(new Date().toLocaleDateString())) {

                        //    utility.DisplayMessages("Previous Vital record can not be attached with provider notes.", 3);

                        //    return;
                        //} //End//28-07-2016//Ahmad Raza//logic to prevent previous vital's attachment with note
                        //else {
                        //    if ($('#' + Clinical_Vitals.params.PanelID + ' #hfNoteId').val() != '' && parseInt($('#' + Clinical_Vitals.params.PanelID + ' #hfNoteId').val()) > 0) {
                        //        // utility.myConfirm('16', function () {
                        //        //Clinical_Notes.UpdateSoapText_VitalInNotes($('#' + Clinical_Vitals.params.PanelID + ' #hfNoteId').val(), Clinical_Vitals.params.VitalSignsId);
                        //        Clinical_Vitals.FillVitals(Clinical_Vitals.params.VitalSignsId, null, null, null, false);
                        //        Clinical_Vitals.VitalsSearch();
                        //        // Clinical_ProgressNote.FillVitals(Clinical_Vitals.params.VitalSignsId, null, null, null, false);
                        //        //  }, function () {
                        //        //      Clinical_Vitals.UnLoadTab();
                        //        // }, '1'
                        //        // );
                        //    } else {
                        //        Clinical_Vitals.FillVitals(Clinical_Vitals.params.VitalSignsId, null, null, null, false);
                        //        Clinical_Vitals.VitalsSearch();
                        //    }
                        //}
                        Clinical_Vitals.VitalsSearch();
                        Clinical_Vitals.LoadDefaultsVital();
                    } else if ($('#' + Clinical_Vitals.params.PanelID + ' #hfNoteId').val() != '' && parseInt($('#' + Clinical_Vitals.params.PanelID + ' #hfNoteId').val()) > 0 && Clinical_Vitals.params.TabID == "clinicalTabProgressNote") {
                        // utility.myConfirm('16', function () {
                        Clinical_Notes.UpdateSoapText_VitalInNotes($('#' + Clinical_Vitals.params.PanelID + ' #hfNoteId').val(), Clinical_Vitals.params.VitalSignsId);
                        Clinical_Vitals.LoadDefaultsVital();
                        utility.DisplayMessages(response.message, 1);
                    } else {
                        Clinical_Vitals.LoadDefaultsVital();
                    }
                    $('#' + Clinical_Vitals.params.PanelID + ' #lblVitalsHeading').text('Add Vitals');
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
        else
            dfd.resolve();
        return dfd;
    },

    LoadDefaultsVital: function () {
        Clinical_Vitals.VitalsSearch();
        //Clinical_Vitals.VitalsEdit(Clinical_Vitals.params.VitalSignsId);
        Clinical_Vitals.ResetFormData();

    },

    SaveVitals: function (VitalSignsData, CommandType, isGridOrForm) {

        var objData = JSON.parse(VitalSignsData);
        if (CommandType != null && CommandType != "") {
            objData["commandType"] = CommandType;
        }
        else {
            objData["commandType"] = "SAVE_VITALS";
        }
        var feet = 0;
        var inches = 0;
        if (objData["HeightFeet"]) {
            if (objData["HeightInches"]) {
                if (isNaN(objData["HeightInches"]) || objData["HeightInches"] == "") {
                    inches = 0;
                }
                else {
                    inches = parseFloat(objData["HeightInches"]);
                }
            }
            feet = parseInt(objData["HeightFeet"]);
            if (feet != 0 && feet != "") {
                inches = (parseFloat(parseInt(feet) * 12) + inches);
                objData["Height"] = inches;
            }
            else if (feet == 0) {
                objData["Height"] = inches;
            }

        }
        else {
            if (objData["HeightInches"]) {
                inches = parseFloat(objData["HeightInches"]);
                objData["Height"] = inches;
            }

        }

        if (isGridOrForm) {
            var feetandInches = objData["Height"];

            if (objData["Height"]) {
                var Ctrlvalue = "";
                if (objData["Height"] != null) {
                    Ctrlvalue = objData["Height"];
                }
                //if (Ctrlvalue.indexOf('.') != -1) {
                //    var arr = Ctrlvalue.split('.');

                //    if (arr.length > 0) {
                //        var inches = arr[1];
                //        var feet = arr[0];
                //        var secondinch = inches.substring(0, 2);
                //        var heightval = parseFloat(feet * 12) + parseFloat(inches);
                //        objData["Height"] = heightval;
                //    }
                //}
                //else {
                //    var heightval = parseFloat(objData["Height"]) * 12;;
                //    objData["Height"] = heightval;
                //}
            }
        }
        // objData["PatientId"] = Clinical_Vitals.params.patientID;
        if (CommandType != "UPDATE_VITALS_FROM_GRID" && CommandType != "SAVE_VITALS_FROM_GRID") {
            objData["Comments"] = $("#" + Clinical_Vitals.params.PanelID + " #txtComment").html();
        }
        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");
    },

    // ************** Vitals Search Functions ***************** \\

    VitalsSearch: function (VitalSignsId, PageNo, rpp) {
        BackgroundLoaderShow(true);
        // Adding selection column of checkbox of Vitals for Progress Notes on 10 Dec 2015 by Azhar
        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_Vitals.params.PanelID + " #dgvVitals thead tr #SelectRecord").length == 0) {
                $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals thead tr").prepend(' <th controlname="selectRecordVitals" class="size20 p-none" id="SelectRecord" coltype="checkbox"></th>');
            }
        } else {
            $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals th#SelectRecord").remove();
        }
        var PanelVitalGrid = "#" + Clinical_Vitals.params.PanelID + " #pnlVitals_Result";
        var VitalGridId = "#" + Clinical_Vitals.params.PanelID + " #dgvVitals";
        $(VitalGridId + " tbody").find("tr").remove();

        //Begin 12-23-2015 Muhammad Arshad Bug # EMR-104 Vitals Workflow in Clinical Module -> Past Vitals History -> Date Column
        $(VitalGridId + ' th#thVitalSignDate').attr("colspan", "1");
        $(VitalGridId + ' th#thVitalSignTime').css("display", "");//.remove();

        $(VitalGridId + ' th#thHeightFeet').attr("colspan", "1");
        $(VitalGridId + ' th#thHeightInches').css("display", "");

        //End 12-23-2015 Muhammad Arshad Bug # EMR-104 Vitals Workflow in Clinical Module -> Past Vitals History -> Date Column

        //Begin 11-30-2015 Muhammad Arshad Bug # EMR-105 Vitals Workflow in Clinical Module -> Past Vitals History -> Systolic/Diastolic Columns
        $(VitalGridId + ' th#thSystolic').attr("colspan", "1").text("Systolic mmHg");
        $(VitalGridId + ' th#thDiastolic').css("display", "");//.remove();


        //End 11-30-2015 Muhammad Arshad Bug # EMR-105 Vitals Workflow in Clinical Module -> Past Vitals History -> Systolic/Diastolic Columns

        if ($.fn.dataTable.isDataTable(VitalGridId)) {
            $(VitalGridId).dataTable().fnClearTable();
            $(VitalGridId).dataTable().fnDestroy();
        }
        //$(ChargeGridId).dataTable().fnDestroy();
        $(PanelVitalGrid).find('.datatables-header').remove();
        $(PanelVitalGrid).find('.datatables-footer').remove();



        Clinical_Vitals.EditableGrid = EMRUtility.MakeEditableGrid(PanelVitalGrid, VitalGridId, Clinical_Vitals, 0, false, true, false, false, false, null);
        var tableCount = $(PanelVitalGrid).find('div.table-responsive');
        for (var i = 0 ; i < tableCount.length - 1 ; i++) {
            $(tableCount[i]).removeClass('Of-a');

        }
        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Appointment Status", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        if ($("#" + Clinical_Vitals.params.PanelID + " #pnlVitals_Result").css("display") == "none") {
            $("#" + Clinical_Vitals.params.PanelID + " #pnlVitals_Result").show();
        }
        $(VitalGridId + ' th#thHeightFeet').attr("colspan", "2").html("Height<br> ft & in");
        $(VitalGridId + ' th#thHeightInches').css("display", "none");
        //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
        AppPrivileges.GetFormPrivileges("Medical_Vitals", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_Vitals.SearchVitals(VitalSignsId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var data = JSON.parse(response.ParentVitalsLoad_JSON);
                        if (data.length > 0 && data[0].Height) {
                            var totalInches = parseFloat(data[0].Height);
                            var feet = Math.floor(totalInches / 12);
                            $('#' + Clinical_Vitals.params.PanelID + ' #txtHeightFeet').val(feet);
                            var inches = totalInches % 12;
                            $('#' + Clinical_Vitals.params.PanelID + ' #txtHeightInches').val(Number(inches).toFixed(2));
                        }
                        if ($(EMREditableGrid.options.table) != null && $(EMREditableGrid.options.table).length > 0) {
                            Clinical_Vitals.VitalsGridLoad(response);
                            //Start//09/12/2015//Ahmad Raza//server side pagination Logic

                            var TableControl = Clinical_Vitals.params.PanelID + " #dgvVitals";
                            var PagingPanelControlID = Clinical_Vitals.params.PanelID + " #divVitalsPaging";
                            var ClassControlName = "Clinical_Vitals";
                            var PagesToDisplay = 5;
                            var iTotalDisplayRecords = response.iTotalDisplayRecords;
                            setTimeout(
                                CreatePagination(response.ParentVitalsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, RowsPerPage) {
                                    Clinical_Vitals.VitalsSearch(PrimaryID, PageNumber, RowsPerPage);
                                })
                            , 10);
                            setTimeout(function () {
                                if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote" && $("#" + Clinical_Vitals.params.PanelID + "  #divVitalsPaging #btnAddVitalsOnNote").length == 0) {
                                    $('<button class="btn btn-success btn-sm pull-right mr-default" type="button" onclick="Clinical_Vitals.addVitalsToNotes();" disabled id="btnAddVitalsOnNote">Add on Note</button>').insertAfter("#" + Clinical_Vitals.params.PanelID + "  #divVitalsPaging .pagination")
                                }

                            }, 11);
                        }
                        //End//09/12/2015//Ahmad Raza//server side pagination Logic
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
    },

    buildRowChild: function (CurrentRow, ParentRowId, ChildRowId, item, arrChildItems, Status) {
        var incrementCount = 0;
        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
            incrementCount = 1;
        }
        var row = Clinical_Vitals.EditableGrid.datatable.row(CurrentRow);
        if (arrChildItems != null && arrChildItems.length > 0) {
            var CurrentRowchilds = $();
            $.each(arrChildItems, function (i, item) {
                var currentChildRow = $("#" + CurrentRow.attr("id")).clone();
                currentChildRow.attr("id", "Child" + item.VitalChildId);
                currentChildRow.attr("parentvitalid", ParentRowId);
                currentChildRow.addClass("childRow-bg");
                $(currentChildRow).find("td:nth-child(" + (1 + incrementCount) + ")").html("");
                $(currentChildRow).find("td:nth-child(" + (2 + incrementCount) + ")").html("");
                $(currentChildRow).find("td:nth-child(" + (3 + incrementCount) + ")").html("");
                $(currentChildRow).find("td:nth-child(" + (4 + incrementCount) + ")").html("");
                if (item.BPModifiedOn != null && item.BPModifiedOn != "") {
                    var ChildBPTitle = item.BPModifiedOn + " by " + item.BPModifiedBy;
                    $(currentChildRow).find("td:nth-child(" + (5 + incrementCount) + ")").attr("title", ChildBPTitle);
                }
                if (item.BPModifiedOn != null && item.BPModifiedOn != "") {
                    var ChildBPTitle = item.BPModifiedOn + " by " + item.BPModifiedBy;
                    $(currentChildRow).find("td:nth-child(" + (6 + incrementCount) + ")").attr("title", ChildBPTitle);
                }
                if (item.BPNegationReasonId != null && item.BPNegationReasonId != "") {
                    $(currentChildRow).find("td:nth-child(" + (7 + incrementCount) + ")").attr("BPNegationReasonId", item.BPNegationReasonId);
                }
                if (item.PulseModifiedOn != null && item.PulseModifiedOn != "") {
                    var ChildPulseTitle = item.PulseModifiedOn + " by " + item.PulseModifiedBy;
                    $(currentChildRow).find("td:nth-child(" + (8 + incrementCount) + ")").attr("title", ChildPulseTitle);
                }
                if (item.TempModifiedOn != null && item.TempModifiedOn != "") {
                    var ChildTempTitle = item.TempModifiedOn + " by " + item.TempModifiedBy;
                    $(currentChildRow).find("td:nth-child(" + (9 + incrementCount) + ")").attr("title", ChildTempTitle);
                }
                if (item.RespModifiedOn != null && item.RespModifiedOn != "") {
                    var ChildRespTitle = item.RespModifiedOn + " by " + item.RespModifiedBy;
                    $(currentChildRow).find("td:nth-child(" + (10 + incrementCount) + ")").attr("title", ChildRespTitle);
                }
                $(currentChildRow).find('select[name*="BPNegationReason"]').attr("ddlist", "GetVaccineRefusalReason");
                $(currentChildRow).find('select[name*="NegationReason"]').attr("ddlist", "GetVaccineRefusalReason");

                var currentChild = Clinical_Vitals.FillCurrentRow(currentChildRow, item, row, true);
                CurrentRowchilds = CurrentRowchilds.add(currentChildRow);

                if (Status == "Signed") {
                    $(currentChildRow).addClass('disableAll');
                }
            });
            row.child(CurrentRowchilds).show();
            setTimeout(function () {
                row.child.hide();
            }, 100);

        }
        else {
            $(CurrentRow).find("td:nth-child(" + (1 + incrementCount) + ")").html("");
        }
        //var row = Clinical_Vitals.EditableGrid.datatable.row(CurrentRow);
        ////row.child(Clinical_Vitals.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        //row.child($("tr#" + CurrentRow.attr("id")).html()).show();
        //row.child.show();
        //row.child().attr("id", "Child" + ChildRowId);
        //row.child().attr("parentvitalid", ParentRowId);
        //$("#" + row.child().attr("id")).html($("tr#" + CurrentRow.attr("id")).html());
        //$("#" + row.child().attr("id")).addClass("childRow-bg");


        //$("#" + row.child().attr("id") + " td:nth-child(1)").html("");
        //$("#" + row.child().attr("id") + " td:nth-child(2)").html("");
        //$("#" + row.child().attr("id") + " td:nth-child(3)").html("");
        //Clinical_Vitals.FillCurrentRow(row.child(), item, row, true);
        //////CurrentRow.next().attr("id", "Child" + CurrentRow.attr("id"));
        //////CurrentRow.next().loadDropDowns(true);
        //////CurrentRow.loadDropDowns(true);
        ////$(CurrentRow).find('input:text[id*="txtTotalFEE"]').attr('disabled', true);
        //////row.child().loadDropDowns(true);

        ////// Fix related to http://192.168.0.16:8080/browse/PMS-2027
        ////row.child().loadDropDowns(true).done(function () {
        ////    //utility.bindMyJSON(true, item, false, $(newChildRow));
        ////});



        return row.child();
    },


    removeControlFromChildRow: function (existingctrls) {
        if (existingctrls != null) {
            if (existingctrls.length > 1) {
                var currentCtrlParent = $(existingctrls[existingctrls.length - 1]).parent()//self.find('input[name="BPId' + item.BPId + '"]').parent();
                if (currentCtrlParent != null) {
                    currentCtrlParent.html("");//$($("#pnlVitals_Result").find('input[name="BPId' + item.BPId + '"]')[1]).parent()
                }
            }
        }
    },

    FillCurrentRow: function (self, item, row, isChildRow, RowIndex) {
        // Start added new check box for selection of vitals for progress note by Azhar Shahzad on 10 Dec 2015
        var CurrentCheckbox = self.find('input[name*="selectRecordVitals"]');

        // Adding selection column of checkbox of Vitals for Progress Notes on 10 Dec 2015 by Azhar
        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
            CurrentCheckbox.off().on('change', function () {
                Clinical_Vitals.enableAddVitals(this);
            });

            var Checked = "";
            var isDisabled = "";
            var NotesId = item.NotesId;
            var noteVitalDate = item.VitalSignDate;

            if (RowIndex != null && RowIndex > 0) {
                isDisabled = " disabled";
            }
            if (NotesId != "") {
                if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.VitalSignId + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                    Checked = "";
                } else {
                    Checked = " checked";
                    isDisabled = "";
                }
                if (NotesId != Clinical_ProgressNote.params.NotesId) {
                    isDisabled = " disabled";
                    Checked = "";
                } else {
                    Checked = " checked";
                    isDisabled = "";
                }
            }
            else {
                if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.VitalSignId + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                    Checked = " checked";
                    isDisabled = " ";
                } else {
                    Checked = "";
                }
                //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1405
                if (Date.parse(noteVitalDate) < Date.parse(Clinical_Vitals.params.VitalSignDate)) {
                    Checked = "";
                    isDisabled = " disabled";
                } else {
                    Checked = "";
                }
                //End 13-07-2016 Edit By Humaira Yousaf Bug#1405

            }
            //Begin 22-09-2016 Edit By Abid Ali QAC1-67

            //if (isDisabled != "") {
            //    CurrentCheckbox.prop('disabled', isDisabled);

            //}

            //End 22-09-2016 Edit By Abid Ali QAC1-67
            if (item.IsNoteLinked == "True" || item.IsNoteLinked == "1") {
                Checked = "checked";
                if ($.inArray(item.VitalSignId, Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                    Clinical_ProgressNote.AttachedNoteComponentIds.push(item.VitalSignId);
                }
            }
            if (Checked != "") {
                CurrentCheckbox.prop('checked', Checked);
            }
        }
        // end added new check box for selection of vitals for progress note by Azhar Shahzad on 10 Dec 2015
        var currentBPId = item.BPId != null ? item.BPId : "";
        self.find('input[name*="BPId"]').val(currentBPId);
        self.find("input[name='BloodPressureIds']").val(currentBPId);
        //if (currentBPId == "") {
        //    self.find('input[name*="BPId"]').parent() != null ? self.find('input[name*="BPId"]').parent().html("") : "";
        //    self.find('input[name*="Diastolic"]').remove();
        //}
        //else {
        //    self.find('input[name*="BPId"]').val(currentBPId);
        //    self.find("input[name='BloodPressureIds']").val(currentBPId);
        //}

        var currentPulseId = item.PulseId != null ? item.PulseId : "";
        self.find('input[name*="PulsId"]').val(currentPulseId);
        self.find("input[name='PulseIds']").val(currentPulseId);
        //if (currentPulseId == "") {
        //    self.find('input[name*="PulsId"]').parent() != null ? self.find('input[name*="PulsId"]').parent().html("") : "";
        //}
        //else {
        //    self.find('input[name*="PulsId"]').val(currentPulseId);
        //    self.find("input[name='PulseIds']").val(currentPulseId);
        //}

        var currentTemperatureId = item.TemperatureId != null ? item.TemperatureId : "";

        self.find('input[name*="TempId"]').val(currentTemperatureId);
        self.find("input[name='TempratureIds']").val(currentTemperatureId);
        //if (currentTemperatureId == "") {
        //    self.find('input[name*="TempId"]').parent() != null ? self.find('input[name*="TempId"]').parent().html("") : "";
        //}
        //else {
        //    self.find('input[name*="TempId"]').val(currentTemperatureId);
        //    self.find("input[name='TempratureIds']").val(currentTemperatureId);
        //}

        var currentRespirationId = item.RespirationId != null ? item.RespirationId : "";
        self.find('input[name*="RespId"]').val(currentRespirationId);
        self.find("input[name='RespirationIds']").val(currentRespirationId);
        //if (currentRespirationId == "") {
        //    self.find('input[name*="RespId"]').parent() != null ? self.find('input[name*="RespId"]').parent().html("") : "";
        //}
        //else {
        //    self.find('input[name*="RespId"]').val(currentRespirationId);
        //    self.find("input[name='RespirationIds']").val(currentRespirationId);
        //}



        if (isChildRow == true) {

            //Weight,Height,BSA,BMI,SPO2,Comments
            self.find('input[name*="selectRecordVitals"],input[name*="Weight"],input[name*="Height"],input[name*="BSA"],input[name*="BMI"],input[name*="SPO2"],input[name*="Comments"],select[id*="ddlVitalNegationReason"]').remove();
        }
        utility.bindMyJSONByName(true, item, false, self).done(function () {
            if (item.Height && item.Height != "") {
                var height = parseFloat(item.Height);
                var feet = parseInt(height / 12);
                var inches = height % 12;
                //var with2Decimals = feet.toString().match(/^-?\d+(?:\.\d{0,2})?/)[0];
                var strInches = inches.toString();
                if (strInches.indexOf('.') > -1) {
                    inches = inches.toFixed(2);
                }

                self.find('input[name*="HeightFeet"]').val(feet);
                self.find('input[name*="HeightInches"]').val(inches);
            }

            var color = Clinical_Vitals.GetBMIColor(item.BMI);
            self.find('input[name*="BMI"]').css("color", color);
            if ($("#" + Clinical_Vitals.params.PanelID + " #txtBMI").val() != "" && color == "red") {
                $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #bmiwarningicon").removeClass('hidden');
            } else {
                $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #bmiwarningicon").addClass('hidden');
            }

            //Begin 11-30-2015 Muhammad Arshad Bug # EMR-121 Vitals in Progress Notes-> Clicking on New Add vitals Hyperlink
            self.find('input[name*="VitalSignDate"]').each(function () {
                if ($(this).val() == "") {
                    $(this).datepicker('setDate', new Date());
                } else {
                    //Start 29-01-2016 Muhammad Arshad Azhar Shahzad changed the implementation way of setDate on datepicker for Bug # EMR-243 Vitals in Clinical Module -> Add Vitals
                    //$(this).datepicker('setDate', $.datepicker.parseDate('yy-mm-dd', $(this).val()));
                    var date_format = 'mm/dd/yyyy';
                    //set default Date Formate
                    if (globalAppdata['DateFormat'])
                        date_format = globalAppdata['DateFormat'];
                    //  $(this).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), $(this).val()));
                    $(this).datepicker({ date_format: date_format.replace('yy', '') }).val($(this).val());
                    $(this).datepicker("setDate", $(this).val());
                    //End 29-01-2016 Muhammad Arshad Azhar Shahzad changed the implementation way of setDate on datepicker for Bug # EMR-243 Vitals in Clinical Module -> Add Vitals
                }
            });
            //Begin 11-30-2015 Muhammad Arshad Bug # EMR-121 Vitals in Progress Notes-> Clicking on New Add vitals Hyperlink

            self.find('input[name*="VitalSignTime"]').each(function () {
                $(this).timepicker().on('changeTime.timepicker', function (e) {
                    disableFocus: false
                });
                if ($(this).val() == "") {
                    $(this).timepicker('setTime', new Date($('#userCurrentTime').html()));
                } else {
                    $(this).timepicker('setTime', $(this).val());
                }

            });
            self.find('input[name*="VitalSignTime"]').attr("name", "VitalsTime");

            self.find('input[name*="BPId"]').attr("name", "BPId" + item.BPId);
            //var existingBPCtrls = $("#pnlVitals_Result").find('input[name="BPId' + item.BPId + '"]');
            //if (existingBPCtrls != null && existingBPCtrls.length > 1) {
            //    Clinical_Vitals.removeControlFromChildRow(existingBPCtrls);
            //}

            self.find('input[name*="PulsId"]').attr("name", "PulsId" + item.PulseId);
            //var existingPulseCtrls = $("#pnlVitals_Result").find('input[name="PulsId' + item.PulseId + '"]');
            //Clinical_Vitals.removeControlFromChildRow(existingPulseCtrls);

            self.find('input[name*="TempId"]').attr("name", "TempId" + item.TemperatureId);
            //var existingTemperatureCtrls = $("#pnlVitals_Result").find('input[name="TempId' + item.TemperatureId + '"]');
            //Clinical_Vitals.removeControlFromChildRow(existingTemperatureCtrls);

            self.find('input[name*="RespId"]').attr("name", "RespId" + item.RespirationId);
            //var existingRespirationCtrls = $("#pnlVitals_Result").find('input[name="RespId' + item.RespirationId + '"]');
            //Clinical_Vitals.removeControlFromChildRow(existingRespirationCtrls);

            self.find('input[name*="Systolic"]').attr("name", "Systolic" + item.BPId);

            self.find('input[name*="Diastolic"]').attr("name", "Diastolic" + item.BPId);
            self.find('select[name*="BPNegationReason"]').attr("name", "BPNegationReason" + item.BPId);

            self.find('input[name*="PulseResult"]').attr("name", "PulseResult" + item.PulseId);
            self.find('input[name*="TemperatureResult"]').attr("name", "TemperatureResult" + item.TemperatureId);
            self.find('input[name*="RespirationResult"]').attr("name", "RespirationResult" + item.RespirationId);
            //setTimeout(function () {
            //    if (row != null) {
            //        row.child.hide();
            //    }
            //    //if (existingBPCtrls != null && existingBPCtrls.length > 1) {
            //    //    self.find('input[name="Diastolic' + item.BPId + '"]').remove();
            //    //}
            //}, 100);

            $(self).loadDropDowns(true).done(function () {
                if (item.BPNegationReasonId) {
                    self.find('select[name*="BPNegationReason"]').val(item.BPNegationReasonId).attr("selected", true);
                }
                //  self.find('select[name="NegationReason"]').val(item.NegationReasonId);
                //self.find('select[name*="BPNegationReason"]').attr("name", "BPNegationReason" + item.NegationReasonId);
            });

            return self;
            BackgroundLoaderShow(false);
        });
    },

    AddNewVitalRow: function (RowId, mode, CurrRef, VitalLatestBPId, VitalLatestPulseId, VitalLatestTempId, VitalLatestRespId, VitalNotesId, isFromAddNewButton, NegationReasonId, BPNegationReasonId) {
        //var myRows= EncounterChargeCapture.EditableGrid;
        var MasterVisitId = $("#" + Clinical_Vitals.params.PanelID + " #hfMasterVisitId").val();
        // Hide PrimaryFee Column if Visit is Primary
        var ChargeGridId = "#" + Clinical_Vitals.params.PanelID + " #dgvVitals";
        if (MasterVisitId == "") {

            $(ChargeGridId + " th#ColumnPrimaryFee").css("display", "none");
        }
        else {

            $(ChargeGridId + " th#ColumnPrimaryFee").css("display", "");
        }


        var CurrentRow = null;
        if (RowId && RowId > 0) {
            //if (EncounterChargeCapture.params.ParentCtrl != null) {
            //    CurrentRow = EncounterChargeCapture.EditableGrid.rowAdd(RowId, "");
            //}
            //else {
            CurrentRow = Clinical_Vitals.EditableGrid.rowAdd(RowId, Clinical_Vitals.params.VitalSignsId, VitalLatestBPId, VitalLatestPulseId, VitalLatestTempId, VitalLatestRespId, VitalNotesId, NegationReasonId, BPNegationReasonId);
            //}

        }
        else {
            var TemplateRow = $("#" + Clinical_Vitals.params.PanelID + " #pnlVitals_Result #dgvVitals tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Child") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            //if (EncounterChargeCapture.params.ParentCtrl != null) {
            //    CurrentRow = EncounterChargeCapture.EditableGrid.rowAdd(TemplateRowId - 1, "");
            //}
            //else {
            CurrentRow = Clinical_Vitals.EditableGrid.rowAdd(TemplateRowId - 1, Clinical_Vitals.params.VitalSignsId, VitalLatestBPId, VitalLatestPulseId, VitalLatestTempId, VitalLatestRespId, null, NegationReasonId, BPNegationReasonId);
            // }
            //End 11-30-2015 Muhammad Arshad Bug # EMR-121 Vitals in Progress Notes-> Clicking on New Add vitals Hyperlink
            if (isFromAddNewButton != null && isFromAddNewButton == "1") {
                // $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals").dataTable().fnPageChange('last');
                $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals tbody tr").removeClass("active");
                CurrentRow.addClass('active');
                CurrentRow.find('td:first').html('');
            }
        }
        CurrentRowSelected = $("#" + Clinical_Vitals.params.PanelID + " tr#" + $(CurrentRow).attr("id"));
        if (CurrentRowSelected.length == 0) {
            // $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals").dataTable().fnPageChange('last');
            CurrentRow = $("#" + Clinical_Vitals.params.PanelID + " tr#" + $(CurrentRow).attr("id"));
        } else {
            CurrentRow = CurrentRowSelected;
        }
        //$(CurrentRow).find('input[name*="BPSystolic"]').attr("name", "BPSystolic" + VitalLatestBPId);

        //utility.CreateDatePicker(Clinical_Vitals.params.PanelID + ' #dgvVitals #dtpDOSFrom' + $(CurrentRow).attr("id"), function () {
        //    //on-change callback method



        //}, false);
        //// Start Copy Previous Row Data to New Row
        //if ($(CurrentRow).attr("id") != null && parseInt($(CurrentRow).attr("id")) < 0) {
        //    var PreviousRow = $("#" + $(CurrentRow).prev().attr("id"));
        //    $(CurrentRow).find('input[id*="txtICD1"]').val(PreviousRow.find('input[id*="txtICD1"]').val());
        //    $(CurrentRow).find('input[id*="hfICD1"]').val(PreviousRow.find('input[id*="hfICD1"]').val());
        //    $(CurrentRow).find('input[id*="hfICDDescription1"]').val(PreviousRow.find('input[id*="hfICDDescription1"]').val());
        //    $(CurrentRow).find('input[id*="hfICD101"]').val(PreviousRow.find('input[id*="hfICD101"]').val());
        //    $(CurrentRow).find('input[id*="hfICD10Description1"]').val(PreviousRow.find('input[id*="hfICD10Description1"]').val());
        //    $(CurrentRow).find('input[id*="hfSNOMED1"]').val(PreviousRow.find('input[id*="hfSNOMED1"]').val());
        //    $(CurrentRow).find('input[id*="hfSNOMEDDescription1"]').val(PreviousRow.find('input[id*="hfSNOMEDDescription1"]').val());

        //    $(CurrentRow).find('input[id*="txtICD2"]').val(PreviousRow.find('input[id*="txtICD2"]').val());
        //    $(CurrentRow).find('input[id*="hfICD2"]').val(PreviousRow.find('input[id*="hfICD2"]').val());
        //    $(CurrentRow).find('input[id*="hfICDDescription2"]').val(PreviousRow.find('input[id*="hfICDDescription2"]').val());
        //    $(CurrentRow).find('input[id*="hfICD102"]').val(PreviousRow.find('input[id*="hfICD102"]').val());
        //    $(CurrentRow).find('input[id*="hfICD10Description2"]').val(PreviousRow.find('input[id*="hfICD10Description2"]').val());
        //    $(CurrentRow).find('input[id*="hfSNOMED2"]').val(PreviousRow.find('input[id*="hfSNOMED2"]').val());
        //    $(CurrentRow).find('input[id*="hfSNOMEDDescription2"]').val(PreviousRow.find('input[id*="hfSNOMEDDescription2"]').val());

        //    $(CurrentRow).find('input[id*="txtICD3"]').val(PreviousRow.find('input[id*="txtICD3"]').val());
        //    $(CurrentRow).find('input[id*="hfICD3"]').val(PreviousRow.find('input[id*="hfICD3"]').val());
        //    $(CurrentRow).find('input[id*="hfICDDescription3"]').val(PreviousRow.find('input[id*="hfICDDescription3"]').val());
        //    $(CurrentRow).find('input[id*="hfICD103"]').val(PreviousRow.find('input[id*="hfICD103"]').val());
        //    $(CurrentRow).find('input[id*="hfICD10Description3"]').val(PreviousRow.find('input[id*="hfICD10Description3"]').val());
        //    $(CurrentRow).find('input[id*="hfSNOMED3"]').val(PreviousRow.find('input[id*="hfSNOMED3"]').val());
        //    $(CurrentRow).find('input[id*="hfSNOMEDDescription3"]').val(PreviousRow.find('input[id*="hfSNOMEDDescription3"]').val());

        //    $(CurrentRow).find('input[id*="txtICD4"]').val(PreviousRow.find('input[id*="txtICD4"]').val());
        //    $(CurrentRow).find('input[id*="hfICD4"]').val(PreviousRow.find('input[id*="hfICD4"]').val());
        //    $(CurrentRow).find('input[id*="hfICDDescription4"]').val(PreviousRow.find('input[id*="hfICDDescription4"]').val());
        //    $(CurrentRow).find('input[id*="hfICD104"]').val(PreviousRow.find('input[id*="hfICD104"]').val());
        //    $(CurrentRow).find('input[id*="hfICD10Description4"]').val(PreviousRow.find('input[id*="hfICD10Description4"]').val());
        //    $(CurrentRow).find('input[id*="hfSNOMED4"]').val(PreviousRow.find('input[id*="hfSNOMED4"]').val());
        //    $(CurrentRow).find('input[id*="hfSNOMEDDescription4"]').val(PreviousRow.find('input[id*="hfSNOMEDDescription4"]').val());
        //}

        //// End Copy Previous Row Data to New Row

        //var row = Clinical_Vitals.EditableGrid.datatable.row(CurrentRow);
        //Clinical_Vitals.buildRowChild(CurrentRow, CurrentRow.attr("id"));

        // We don't need Up/Down for newly added Row as Id is in Minus
        //EncounterChargeCapture.enableUpAndDown($(CurrentRow));
        //Show Delete option while row is in Edit Mode

        Clinical_Vitals.enableRemoveRow($(CurrentRow));



        //Bind Events for controls as specified in Vitals HTML file on such controls
        var objSystolic = $(CurrentRow).find('input[name*="Systolic"]');
        var objDiastolic = $(CurrentRow).find('input[name*="Diastolic"]');

        objSystolic.on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
            utility.ValidateDecimal(event, 0);
            Clinical_Vitals.ValidateBP(objSystolic, objDiastolic);
        });

        objSystolic.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");


        objSystolic.on("keypress input", function (event) {

            utility.ValidateLenght(this, 3)
        });
        //A hack to register input event by triggering keypress event
        objSystolic.trigger("keypress");


        objDiastolic.on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
            utility.ValidateDecimal(event, 0);
            Clinical_Vitals.ValidateBP(objSystolic, objDiastolic);
        });
        objDiastolic.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");

        objDiastolic.on("keypress input", function (event) {
            utility.ValidateLenght(this, 3)
        });
        objDiastolic.trigger("keypress");

        var objPulseResult = $(CurrentRow).find('input[name*="PulseResult"]').on("keydown", function (event) {
            utility.ValidateDecimal(event, 0);
        });
        objPulseResult.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");

        var objTemperatureResult = $(CurrentRow).find('input[name*="TemperatureResult"]').on("keydown", function (event) {
            utility.ValidateDecimal(event, 2, 0, 9999999, "");
        });

        objPulseResult.on("keypress input", function (event) {

            utility.ValidateLenght(this, 3)
        }).trigger("keypress");

        objPulseResult.on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
        });

        objTemperatureResult.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");

        objTemperatureResult.on("keypress input", function (event) {

            utility.ValidateLenght(this, 5);

        }).trigger("keypress");

        objTemperatureResult.on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
        });

        var objRespirationResult = $(CurrentRow).find('input[name*="RespirationResult"]').on("keydown", function (event) {
            utility.ValidateDecimal(event, 0);
        });
        objRespirationResult.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");

        objRespirationResult.on("keypress input", function (event) {

            utility.ValidateLenght(this, 3);

        }).trigger("keypress");

        objRespirationResult.on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
        });

        var objSPO2 = $(CurrentRow).find('input[name*="SPO2"]').on("keydown", function (event) {
            utility.ValidateDecimal(event, 0);
        });

        objSPO2.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");

        var objHeight = $(CurrentRow).find('input[name*="Height"]');

        var objft = $(CurrentRow).find('input[name*="HeightFeet"]');
        var objin = $(CurrentRow).find('input[name*="HeightInches"]');

        //objHeight.on("keyup", function (event) {

        //    //  utility.ValidateLenght(this, 4);

        //    EMRUtility.ValidateHeight(event, this);

        //});


        //objHeight.on("blur", function (event) {
        //    Clinical_Vitals.EnableDisableSaveButton(RowId);
        //});

        objft.on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
        });
        objin.on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
        });

        //objHeight.on("keydown", function (event) {
        //    utility.ValidateDecimal(event, 2);
        //});

        //.on("keydown", function (event) {
        //    utility.ValidateDecimal(event, 0);
        //});
        var objWeight = $(CurrentRow).find('input[name*="Weight"]');

        objWeight.on("keypress input", function (event) {

            utility.ValidateLenght(this, 4);

        }).trigger("keypress");


        var BMIObj = $(CurrentRow).find('[name *= "BMI"]');
        var BSAObj = $(CurrentRow).find('[name *= "BSA"]');

        // objHeight.attr("onkeyup", "EMRUtility.ValidateHeight(event, this);");
        objWeight.attr("onblur", "utility.ValidateDecimal(event, 2);EMRUtility.ValidateWeight(event, this);");

        objWeight.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        objWeight.on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
        });
        //objHeight.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        objft.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
        objin.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");

        //objft.on("keyup", function (event) {

        //    Clinical_Vitals.ValidateHeightFeet(event, this);
        //});
        //objin.on("keyup", function (event) {

        //    Clinical_Vitals.ValidateHeightInches(event, this);
        //});

        $(CurrentRow).find('input[name*="SPO2"]').attr("oninput", "EMRUtility.setPercentage(event,this);");

        objSPO2.on("keypress input", function (event) {

            utility.ValidateLenght(this, 3);

        }).trigger("keypress");

        objSPO2.on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
            EMRUtility.setPercentage(event, this);
        });


        objWeight.on("input", function (event) {

            utility.ValidateLenght(this, 4);
            utility.ValidateDecimal(event, 2);
            Clinical_Vitals.calculateBMI2($(this), $(objft), $(BMIObj), $(objin));
            Clinical_Vitals.calculateBSA2($(this), $(objft), $(BSAObj), $(objin));
        });

        //objHeight.on("input", function (event) {

        //    Clinical_Vitals.calculateBMI($(objWeight), $(this), $(BMIObj));
        //    Clinical_Vitals.calculateBSA($(objWeight), $(this), $(BSAObj));
        //});

        objft.on("keypress input", function (event) {
            Clinical_Vitals.ValidateHeightFeet(event, this);
            Clinical_Vitals.calculateBMI2($(this), $(objft), $(BMIObj), $(objin));
            Clinical_Vitals.calculateBSA2($(this), $(objft), $(BSAObj), $(objin));
        });
        var feetid = $(objft).attr('id');
        objin.on("keypress input", function (event) {
            Clinical_Vitals.ValidateHeightInches(event, this, feetid);
            Clinical_Vitals.calculateBMI2($(this), $(objft), $(BMIObj), $(objin));
            Clinical_Vitals.calculateBSA2($(this), $(objft), $(BSAObj), $(objin));
        });


        $(CurrentRow).find('input[name*="Comments"]').on("blur", function (event) {
            Clinical_Vitals.EnableDisableSaveButton(RowId);
        });

        $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals tbody tr#-1').find('.expand-row').remove();
        //Begin 11-30-2015 Muhammad Arshad Bug # EMR-121 Vitals in Progress Notes-> Clicking on New Add vitals Hyperlink
        $(CurrentRow).find('input[name*="VitalSignDate"]').each(function () {
            if ($(this).val() == "") {
                $(this).datepicker('setDate', new Date());
            } else {
                //Start 29-01-2016 Muhammad Arshad Azhar Shahzad changed the implementation way of setDate on datepicker for Bug # EMR-243 Vitals in Clinical Module -> Add Vitals
                //$(this).datepicker('setDate', $(this).val());
                var date_format = 'mm/dd/yyyy';
                //set default Date Formate
                if (globalAppdata['DateFormat'])
                    date_format = globalAppdata['DateFormat'];
                //  $(this).datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), $(this).val()));
                $(this).datepicker({ date_format: date_format.replace('yy', '') }).val($(this).val());
                $(this).datepicker("setDate", $(this).val());
                //End 29-01-2016 Muhammad Arshad Azhar Shahzad changed the implementation way of setDate on datepicker for Bug # EMR-243 Vitals in Clinical Module -> Add Vitals
            }
        });

        // bug # EMR-108 Clicking in the blanks fields should display the number pad.
        $(CurrentRow).find('[data-plugin-keyboard-numpad]').keyboard({
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
                var BMIObj = keyboard.$preview.parent().parent().find('[name = "BMI"]');
                var BSAObj = keyboard.$preview.parent().parent().find('[name = "BSA"]');
                if (keyboard.$preview.attr('name') == 'Weight') {
                    //var objHeight = keyboard.$preview.parent().parent().find('[name = "Height"]');
                    //Clinical_Vitals.calculateBMI(keyboard.$preview, $(objHeight), $(BMIObj));
                    //Clinical_Vitals.calculateBSA(keyboard.$preview, $(objHeight), $(BSAObj));
                    var wght = keyboard.$preview.parent().parent().find('[name = "Weight"]');
                    var objfoot = keyboard.$preview.parent().parent().find('[name = "HeightFeet"]');
                    var objinc = keyboard.$preview.parent().parent().find('[name = "HeightInches"]');
                    Clinical_Vitals.calculateBMI2($(wght), $(objfoot), $(BMIObj), $(objinc));
                    Clinical_Vitals.calculateBSA2($(wght), $(objfoot), $(BSAObj), $(objinc));
                } else if (keyboard.$preview.attr('name') == 'HeightFeet' || keyboard.$preview.attr('name') == 'HeightInches') {
                    //  keyboard.$preview.trigger('onkeyup');
                    //   EMRUtility.ValidateHeight(e, keyboard.$preview);
                    var objWeight = keyboard.$preview.parent().parent().find('[name = "Weight"]');
                    var objfoot = keyboard.$preview.parent().parent().find('[name = "HeightFeet"]');
                    var objinc = keyboard.$preview.parent().parent().find('[name = "HeightInches"]');
                    if (keyboard.$preview.attr('name') == 'HeightFeet') {
                        Clinical_Vitals.ValidateHeightFeet(event, $(objfoot));
                    } else if (keyboard.$preview.attr('name') == 'HeightInches') {
                        var feetid = $(objfoot).attr('id');
                        Clinical_Vitals.ValidateHeightInches(event, $(objinc), feetid);
                    }

                    Clinical_Vitals.calculateBMI2($(objWeight), $(objfoot), $(BMIObj), $(objinc));
                    Clinical_Vitals.calculateBSA2($(objWeight), $(objfoot), $(BSAObj), $(objinc));
                }
                else if (keyboard.$preview.attr('oninput') != null) {
                    keyboard.$preview.trigger('oninput');
                } else if (keyboard.$preview.attr('onkeyup') != null) {
                    keyboard.$preview.trigger('onkeyup');
                } else if (keyboard.$preview.attr('onkeydown') != null) {
                    keyboard.$preview.trigger('onkeydown');
                }
            },
            layout: 'custom',
            appendLocally: this,
            restrictInput: true, // Prevent keys not in the displayed keyboard from being typed in
            preventPaste: true,  // prevent ctrl-v and right click
            usePreview: false,
            autoAccept: true,
            tabNavigation: true
        })
                .addTyping()
            .on('focus', function (ev) {
                var offset = $(this).position();
                $(this).parent().find('.ui-keyboard').attr('style', 'top: ' + (offset.top + 22) + 'px !important;');
            });
        //end bug fix
        var self = $("#" + Clinical_Vitals.params.PanelID + " tr#" + $(CurrentRow).attr("id"));
        var CurrentCheckbox = self.find('input[name*="selectRecordVitals"]');
        // Adding selection column of checkbox of Vitals for Progress Notes on 10 Dec 2015 by Azhar
        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
            CurrentCheckbox.off().on('change', function () {
                Clinical_Vitals.enableAddVitals(this);
            });
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        return CurrentRow;
    },

    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
        //    .each(function () {
        //    $(this).removeclass('hidden')
        //});
    },

    VitalsGridLoad: function (response) {

        //$("#pnlClinicalVitals #dgvVitals").dataTable().fnDestroy();
        //$("#pnlVitals_Result #dgvVitals tbody").find("tr").remove();
        if (response.ParentVitalsCount > 0) {
            Clinical_Vitals.EditableGrid.datatable.clear().draw();
            ////new
            //var actions = "";
            //$("#pnlClinicalVitals #dgvVitals tr th").each(function () {
            //    if ($(this).attr("coltype") && $(this).attr("coltype").toLowerCase() == "action") {
            //        var arrActionType = [];
            //        if ($(this).attr("ActionType") != null) {
            //            arrActionType = $(this).attr("ActionType").split(',');
            //            actions = EditableGrid.GetActions(arrActionType);
            //        }
            //    }
            //});
            var VitalsLoadJSONData = JSON.parse(response.ParentVitalsLoad_JSON);
            var ChildVitalsLoadJSONData = JSON.parse(response.ChildVitalsLoad_JSON);

            $.each(VitalsLoadJSONData, function (i, item) {
                ////var charge_detail = JSON.parse(item);
                var VitalSignId = item.VitalSignId;
                //Start//27-05-2016//Ahmad Raza//logic to convert multiline comments from html
                var regex = /<br\s*[\/]?>/gi;
                item.Comments = $('<div/>').html(item.Comments).text();
                item.Comments = item.Comments.replace(regex, "\n");
                var comm = '<div>' + item.Comments + '</div>';
                var obj = $(comm);

                item.Comments = $(obj).text();


                //End//27-05-2016//Ahmad Raza//logic to convert multiline comments from html
                var arrChildVitals = [];
                $.each(ChildVitalsLoadJSONData, function (i, item) {
                    if (item.VitalSignId == VitalSignId) {
                        arrChildVitals.push(item);
                    }
                });
                var ParentBPTitle = item.BPModifiedOn + " by " + item.BPModifiedBy;
                var ParentPulseTitle = item.PulseModifiedOn + " by " + item.PulseModifiedBy;
                var ParentTempTitle = item.TempModifiedOn + " by " + item.TempModifiedBy;
                var ParentRespTitle = item.RespModifiedOn + " by " + item.RespModifiedBy;
                //VitalLatestBPId, VitalLatestPulseId, VitalLatestTempId, VitalLatestRespId

                var CurrentRow = Clinical_Vitals.AddNewVitalRow(VitalSignId, "Edit", null, item.BPId, item.PulseId, item.TemperatureId, item.RespirationId, item.NotesId, null, item.NegationReasonId, item.BPNegationReasonId);
                Clinical_Vitals.buildRowChild(CurrentRow, CurrentRow.attr("id"), null, null, arrChildVitals, item.NoteStatus);
                //$.each(arrChildVitals, function (i, item) {
                //    var currentChildRow = Clinical_Vitals.buildRowChild(CurrentRow, CurrentRow.attr("id"), item.VitalChildId, item, arrChildVitals);
                //});

                // Change Done By Muhammad Azhar Shahzad on 10 Dec 2015

                //adding checkboxes column and disabling that row, if problem list already binded with notes
                var SelectionCheckBoxColumn = "";
                var Checked = "";

                var incrementCount = 0;
                if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
                    incrementCount = 1;
                }
                $(CurrentRow).find("td:nth-child(" + (5 + incrementCount) + ")").attr("title", ParentBPTitle);
                $(CurrentRow).find("td:nth-child(" + (6 + incrementCount) + ")").attr("title", ParentBPTitle);
                $(CurrentRow).find("td:nth-child(" + (7 + incrementCount) + ")").attr("title", ParentPulseTitle);
                $(CurrentRow).find("td:nth-child(" + (8 + incrementCount) + ")").attr("title", ParentTempTitle);
                $(CurrentRow).find("td:nth-child(" + (9 + incrementCount) + ")").attr("title", ParentRespTitle);
                //Start 25-05-2016 Edit By Humaira Yousaf Bug# EMR-759
                $(CurrentRow).find("td:nth-child(" + (15 + incrementCount) + ")").attr("title", item.Comments);
                //End 25-05-2016 Edit By Humaira Yousaf Bug# EMR-759
                //$(CurrentRow).find("td:nth-child(6)").html("");

                //$(CurrentRow).loadDropDowns(true).done(function () {
                //    $(CurrentRow).find('select[name="NegationReason"]').val(item.NegationReasonId);
                //});


                if (item.NoteStatus == "Signed") {
                    $(CurrentRow).find("td:nth-child(n+2)").addClass('disableAll');
                    $(CurrentRow).attr('status', 'disable');
                }

                //Start 29-06-2016 Humaira Yousaf to enable/disable Delete button
                if (item.NotesId > 0) {
                    $($(CurrentRow).find("td:nth-child(2)").find('a')[1]).addClass('disableAll'); // disable Delete button if vital is associated with note
                }
                else {
                    $($(CurrentRow).find("td:nth-child(2)").find('a')[1]).removeClass('disableAll')
                }
                //End 29-06-2016 Humaira Yousaf to enable/disable Delete button

                var self = $("#" + Clinical_Vitals.params.PanelID + " tr#" + VitalSignId);
                Clinical_Vitals.FillCurrentRow(self, item, CurrentRow, false, i);

                var row = Clinical_Vitals.EditableGrid.datatable.row(CurrentRow);

                /********************************/
                var newChildRow = row.child();
                var objChildSystolic = $(CurrentRow).find('input[name*="Systolic"]');
                var objChildDiastolic = $(CurrentRow).find('input[name*="Diastolic"]');
                var objChildBMI = $(CurrentRow).find('input[name*="BMI"]');
                var BMIVal = objChildBMI[0];
                var BMIVal = $(BMIVal).val();
                if (parseFloat(BMIVal) >= 25.00 || parseFloat(BMIVal) <= 18.5) {
                    $(objChildBMI).parent().find('#bmiwarningicon').removeClass('hidden');
                }

                var systolicVal = objChildSystolic[0];
                var systolicVal = $(systolicVal).val();
                var diastolicVal = objChildDiastolic[0];
                var diastolicVal = $(diastolicVal).val();

                if (systolicVal != "" && parseFloat(systolicVal) > 120) {

                    $(objChildSystolic).parent().find('#systolicwarningicon').removeClass('hidden');
                } else if (systolicVal != "" && parseFloat(systolicVal) <= 120) {

                    $(objChildSystolic).parent().find('#systolicwarningicon').addClass('hidden');
                }
                if (diastolicVal != "" && parseFloat(diastolicVal) > 80) {

                    $(objChildDiastolic).parent().find('#diastolicwarningicon').removeClass('hidden');
                } else if (diastolicVal != "" && parseFloat(diastolicVal) <= 80) {

                    $(objChildDiastolic).parent().find('#diastolicwarningicon').addClass('hidden');
                }

                objChildSystolic.trigger('blur');
                objChildDiastolic.trigger('blur');
                objChildSystolic.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");
                objChildDiastolic.attr("data-plugin-keyboard-numpad", "data-plugin-keyboard-numpad");

                if (newChildRow != null) {
                    utility.bindMyJSON(true, item, false, $(newChildRow));
                    //row.child().loadDropDowns(true).done(function () {
                    //    utility.bindMyJSON(true, item, false, $(newChildRow));
                    //    //serialize data
                    //    // $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());

                    //});
                }
                /********************************/
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            });
            $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals").dataTable().fnPageChange('first');

            //Begin 12-23-2015 Muhammad Arshad Bug # EMR-104 Vitals Workflow in Clinical Module -> Past Vitals History -> Date Column
            $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals th#thVitalSignDate').attr("colspan", "2");
            $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals th#thVitalSignTime').css("display", "none");//.remove();
            //End 12-23-2015 Muhammad Arshad Bug # EMR-104 Vitals Workflow in Clinical Module -> Past Vitals History -> Date Column

            //Begin 11-30-2015 Muhammad Arshad Bug # EMR-105 Vitals Workflow in Clinical Module -> Past Vitals History -> Systolic/Diastolic Columns
            $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals th#thSystolic').attr("colspan", "2").html("Blood Pressure <br> mmHg");
            $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals th#thDiastolic').css("display", "none");//.remove();

            $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals th#thHeightFeet').attr("colspan", "2").html("Height<br> ft & in");
            $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals th#thHeightInches').css("display", "none");
            //End 11-30-2015 Muhammad Arshad Bug # EMR-105 Vitals Workflow in Clinical Module -> Past Vitals History -> Systolic/Diastolic Columns
        }
        else {
            $("#" + Clinical_Vitals.params.PanelID + " #pnlVitals_Result #divVitalsPaging").css("display", "none");
            $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals').DataTable({
                "language": {
                    "emptyTable": "No Vitals Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "bSort": false
            });
        }



        //if ($.fn.dataTable.isDataTable('#dgvVitals'))
        //    ;
        //else
        //    $("#pnlVitals_Result #dgvVitals").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": true, "aTargets": [0] }] }); // to remove records per page dropdown

        //$("#pnlClinicalVitals #pnlVitals_Result #dgvVitals th")[0].click();

        //var PanelGrid = "#pnlClinicalVitals #pnlVitals_Result";
        //var GridId = "#pnlClinicalVitals #dgvVitals";
        //utility.MakeEditableGrid(PanelGrid, GridId, Clinical_Vitals);

        ////$("#pnlVitals_Result #dgvVitals").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": true, "aTargets": [0] }] }); // to remove records per page dropdown


        $("#pnlClinicalVitals #pnlVitals_Result").attr("style", "");
    },

    SearchVitals: function (VitalSignsId, PageNumber, RowsPerPage) {

        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }

        var objData = new Object();
        objData["VitalSignsId"] = VitalSignsId;
        objData["PageNo"] = PageNumber;
        objData["rpp"] = RowsPerPage;
        objData["PatientId"] = Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["NotesId"] = Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote" ? Clinical_ProgressNote.params.NotesId : null;
        objData["commandType"] = "SEARCH_VITALS";

        var data = JSON.stringify(objData);
        //var data = "VitalSignsId=" + VitalSignsId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },

    // Pagination Functions //

    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlVitals_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Clinical_Vitals.VitalsSearch(null, PageNo, 15);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlVitals_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        if (currentPageNo != "" && currentPageNo > 0) {
            Clinical_Vitals.VitalsSearch(null, currentPageNo, 15);

        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {

        var currentPageNo = "";
        $("#pnlVitals_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });

        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        if (currentPageNo != "" && currentPageNo <= TotalPages) {
            Clinical_Vitals.VitalsSearch(null, currentPageNo, 15);
        }
    },

    checkUncheckAll: function (obj) {
        $('#' + Clinical_Vitals.params["PanelID"] + ' #dgvVitals input[id*="chkCli_Vitals"]').prop("checked", $(obj).prop("checked"));
    },


    //************ Delete Functions ***************\\

    VitalsDelete: function (VitalsId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }

        //AppPrivileges.GetFormPrivileges("Appointment Status", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        utility.myConfirm('1', function () {
            var selectedValue = VitalsId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Vitals.DeleteVitals(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var table1 = $('#dgvVitals').DataTable({ "bSort": false });
                        table1.row('.active').remove().draw(false);
                        utility.DisplayMessages(response.Message, 1);
                        Clinical_Vitals.VitalsSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '1'
            );
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});

    },

    DeleteVitals: function (VitalSignsId) {

        var objData = new Object();
        objData["VitalSignsId"] = VitalSignsId;
        objData["commandType"] = "DELETE_VITALS";
        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },
    //************* End Delete Functions ***********\\

    //************* Update and Fill **************\\
    VitalsEdit: function (VitalSignsId) {
        var dfd = $.Deferred();
        //Conflict resolve of Functions Name By Azhar shahzad on 11 dec 2015
        Clinical_Vitals.FillVitals_DBCall(VitalSignsId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                // $("#" + Clinical_Vitals.params.PanelID + " #btnCopyVitals").css("display", "");
                var vitals_detail = JSON.parse(response.VitalsFill_JSON);
                var vitals_BloodPressure_detail = JSON.parse(response.VitalsBlooPressureFill_JSON);
                var vitals_Pulse_detail = JSON.parse(response.VitalsPulseFill_JSON);
                var vitals_Temperature_detail = JSON.parse(response.VitalsTemperatureFill_JSON);
                var vitals_Respiration_detail = JSON.parse(response.VitalsRespirationFill_JSON);
                // Remove Existing Blood Pressure Rows
                $("#" + Clinical_Vitals.params.PanelID + " #divTemplateBloodPressure").nextAll().remove();
                $.each(vitals_BloodPressure_detail, function (i, item) {
                    if (parseInt(item.CurrentBloodPressureId) > 0) {

                        Clinical_Vitals.AddNewRow('BloodPressure', Clinical_Vitals.params.PanelID + ' #divTemplateBloodPressure', item.CurrentBloodPressureId, item);
                        //var CurrentBPRow = $("#BloodPressure")
                    }
                });

                $.each(vitals_BloodPressure_detail, function (i, item) {
                    var objDiastolic = $('*[id*=Diastolic]')[i + 1];
                    var objSystolic = $('*[id*=Systolic]')[i + 1];
                    Clinical_Vitals.ValidateBP(objSystolic, objDiastolic);
                });

                if (vitals_BloodPressure_detail.length < 1) {
                    Clinical_Vitals.AddNewRow('BloodPressure', Clinical_Vitals.params.PanelID + ' #divTemplateBloodPressure', "-1");

                }

                // Remove Existing Pulse Rows
                $("#" + Clinical_Vitals.params.PanelID + " #divTemplatePulse").nextAll().remove();
                $.each(vitals_Pulse_detail, function (i, item) {
                    if (parseInt(item.CurrentPulseId) > 0) {
                        Clinical_Vitals.AddNewRow('Pulse', Clinical_Vitals.params.PanelID + ' #divTemplatePulse', item.CurrentPulseId, item);
                    }
                });
                if (vitals_Pulse_detail.length < 1) {
                    Clinical_Vitals.AddNewRow('Pulse', Clinical_Vitals.params.PanelID + ' #divTemplatePulse', "-1");

                }
                // Remove Existing Temprature Rows
                $("#" + Clinical_Vitals.params.PanelID + " #divTemplateTemprature").nextAll().remove();
                $.each(vitals_Temperature_detail, function (i, item) {
                    if (parseInt(item.CurrentTemperatureId) > 0) {
                        Clinical_Vitals.AddNewRow('Temprature', Clinical_Vitals.params.PanelID + ' #divTemplateTemprature', item.CurrentTemperatureId, item);
                    }
                });
                if (vitals_Temperature_detail.length < 1) {
                    Clinical_Vitals.AddNewRow('Temprature', Clinical_Vitals.params.PanelID + ' #divTemplateTemprature', "-1");

                }
                // Remove Existing Respiration Rows
                $("#" + Clinical_Vitals.params.PanelID + " #divTemplateRespiration").nextAll().remove();
                $.each(vitals_Respiration_detail, function (i, item) {
                    if (parseInt(item.CurrentRespirationId) > 0) {
                        Clinical_Vitals.AddNewRow('Respiration', Clinical_Vitals.params.PanelID + ' #divTemplateRespiration', item.CurrentRespirationId, item);
                    }
                });
                if (vitals_Respiration_detail.length < 1) {
                    Clinical_Vitals.AddNewRow('Respiration', Clinical_Vitals.params.PanelID + ' #divTemplateRespiration', "-1");
                }

                var self = $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals');
                utility.bindMyJSONByName(true, vitals_detail, false, self).done(function () {
                    //Begin 23-12-2015 Muhammad Arshad Bug#EMR-113 Vitals in Progress Notes-> Update button
                    if (vitals_detail.Height == "10") {
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtHeight').val(Number(vitals_detail.Height).toFixed(2));
                    }
                    if (vitals_detail.BloodType == "0") {
                        $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals #ddlBloodType').val($('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals #ddlBloodType option:first').val());
                    }
                    if (vitals_detail.SeverityofPain == "0") {
                        $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals #ddlSeverityofPain').val($('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals #ddlSeverityofPain option:first').val());
                    }
                    if (vitals_detail.SmokingStatus == "0") {
                        $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals #ddlSmokingStatus').val($('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals #ddlSmokingStatus option:first').val());
                    }
                    $('#' + Clinical_Vitals.params.PanelID + ' #btnsave').text('Save');
                    //End 23-12-2015 Muhammad Arshad Bug#EMR-113 Vitals in Progress Notes-> Update button
                    //Begin 15-07-2016 Edit By Humaira Yousaf Bug#1582
                    if (vitals_detail.Height == "0" || vitals_detail.Height == "" || vitals_detail.Height == null) {
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtHeightFeet').val("");
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtHeightInches').val("");
                    }
                    else {
                        var totalInches = parseFloat(vitals_detail.Height);
                        var feet = Math.floor(totalInches / 12);
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtHeightFeet').val(feet);
                        var inches = totalInches % 12;
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtHeightInches').val(Number(inches).toFixed(2));
                    }
                    if (vitals_detail.Weight == "0") {
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtWeight').val("");
                    }
                    if (vitals_detail.BMI == "0") {
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtBMI').val("");
                    }
                    if (vitals_detail.BSA == "0") {
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtBSA').val("");
                    }
                    if (vitals_detail.HeadCir == "0") {
                        $('#' + Clinical_Vitals.params.PanelID + ' #txtHeadCir').val("");
                    }
                    if (parseFloat(vitals_detail.BMI) >= 25.00 || parseFloat(vitals_detail.BMI) <= 18.5) {
                        $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #bmiwarningicon").removeClass('hidden');
                        var CtrlName = $("#" + Clinical_Vitals.params.PanelID + " #txtBMI");
                        var color = Clinical_Vitals.GetBMIColor(vitals_detail.BMI);
                        CtrlName.css("color", color);
                    } else {
                        $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #bmiwarningicon").addClass('hidden');
                        var CtrlName = $("#" + Clinical_Vitals.params.PanelID + " #txtBMI");
                        var color = Clinical_Vitals.GetBMIColor(vitals_detail.BMI);
                        CtrlName.css("color", color);
                    }
                    let comm = '<div>' + vitals_detail.Comments + '</div>';
                    let obj = $(comm);
                    $('#' + Clinical_Vitals.params.PanelID + ' #txtComment').html(obj);
                    //End 15-07-2016 Edit By Humaira Yousaf Bug#1582
                    Clinical_Vitals.params["VitalSignsId"] = VitalSignsId;
                    Clinical_Vitals.params["mode"] = "Edit";
                });
                $('#' + Clinical_Vitals.params.PanelID + ' #lblVitalsHeading').text('Edit Vitals');
                //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1398
                $('#' + Clinical_Vitals.params.PanelID + ' #btnAddNew').removeClass('hidden');
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
                //End 13-07-2016 Edit By Humaira Yousaf Bug#1398
                dfd.resolve();
            }
            else {
                dfd.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd;
    },

    FillVitals_DBCall: function (VitalSignsId) {
        var objData = new Object();
        objData["VitalSignsId"] = VitalSignsId;
        objData["commandType"] = "FILL_VITALS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");
    },

    UpdateVitals: function (VitalSignsData, VitalSignsId, CommandType) {

        var objData = JSON.parse(VitalSignsData);
        objData["VitalSignsId"] = VitalSignsId;
        var inches = 0;
        if (CommandType != null && CommandType != "") {
            objData["commandType"] = CommandType;
        }
        else {
            objData["commandType"] = "UPDATE_VITALS";
        }
        if (objData["Height"]) {
            var Ctrlvalue = "";
            if (objData["Height"] != null) {
                Ctrlvalue = objData["Height"];
            }
        }
        if (objData["HeightFeet"]) {
            if (objData["HeightInches"]) {
                if (isNaN(objData["HeightInches"]) || objData["HeightInches"] == "") {
                    inches = 0;
                }
                else {
                    inches = parseFloat(objData["HeightInches"]);
                }

            }
            feet = objData["HeightFeet"];
            if (feet != 0 && feet != "") {
                inches = (parseFloat(parseInt(feet) * 12) + inches);
            }
            objData["Height"] = inches;
        }
        else {
            if (objData["HeightInches"]) {
                inches = parseFloat(objData["HeightInches"]);
                objData["Height"] = inches;
            }
        }
        objData["PatientId"] = Clinical_Vitals.params.patientID;
        if (CommandType != "UPDATE_VITALS_FROM_GRID") {
            objData["Comments"] = $("#" + Clinical_Vitals.params.PanelID + " #txtComment").html();
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },

    //************* End Update and Fill **************\\

    ResetFormData: function () {
        $("#" + Clinical_Vitals.params.PanelID + " #divTemplateRespiration").nextAll().remove();
        $("#" + Clinical_Vitals.params.PanelID + " #divTemplateTemprature").nextAll().remove();
        $("#" + Clinical_Vitals.params.PanelID + " #divTemplatePulse").nextAll().remove();
        $("#" + Clinical_Vitals.params.PanelID + " #divTemplateBloodPressure").nextAll().remove();
        //Add default rows

        $('#' + Clinical_Vitals.params.PanelID + ' #lblVitalsHeading').text('Add Vitals');
        $('#' + Clinical_Vitals.params.PanelID + ' #btnsave').text('Add');
        //    $("#" + Clinical_Vitals.params.PanelID + " #btnCopyVitals").css("display", "none");

        $('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals').resetAllControls(null);
        // $('#pnlClinicalVitals #frmClinicalVitals')[0].reset();
        $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #hfPatientId").val($("div#PatientProfile #hfPatientId").val());

        if (Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet") {
            $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }

        ($('#' + Clinical_Vitals.params.PanelID + ' #frmClinicalVitals')).find('input[type=text], textarea').val('');
        $('#' + Clinical_Vitals.params.PanelID + ' #dpVitalsDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
        $('#' + Clinical_Vitals.params.PanelID + ' #tpVitalsTime').timepicker('setTime', new Date($('#userCurrentTime').html()));
        Clinical_Vitals.AddNewRow('BloodPressure', Clinical_Vitals.params.PanelID + ' #divTemplateBloodPressure', "-1");
        Clinical_Vitals.AddNewRow('Pulse', Clinical_Vitals.params.PanelID + ' #divTemplatePulse', "-1");
        Clinical_Vitals.AddNewRow('Temprature', Clinical_Vitals.params.PanelID + ' #divTemplateTemprature', "-1");
        Clinical_Vitals.AddNewRow('Respiration', Clinical_Vitals.params.PanelID + ' #divTemplateRespiration', "-1");

        Clinical_Vitals.params["VitalSignsId"] = "";
        Clinical_Vitals.params["mode"] = "Add";
        //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1398
        $('#' + Clinical_Vitals.params.PanelID + ' #btnAddNew').addClass('hidden');
        $('#' + Clinical_Vitals.params.PanelID + ' #txtComment').html('');
        //End 13-07-2016 Edit By Humaira Yousaf Bug#1398
    },

    unLoadTab: function (nextOrPre, controlToInvoke) {
        var parentPanelId = null;
        Clinical_Vitals.controlToInvoke = controlToInvoke;
        var objDeffered = $.Deferred();
        /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet") {

            if (Clinical_Vitals.params["FromAdmin"] == "0") {
                if (Clinical_Vitals.params != null && Clinical_Vitals.params.ParentCtrl != null) {

                    if (Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet") {
                        parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                        Clinical_FaceSheet.params.ChildPanelID = null;
                        UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals', null, parentPanelId);
                    } else {
                        UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals');
                    }

                }
                else
                    UnloadActionPan(null, 'Clinical_Vitals');
            }
            else {
                //$("#mstrDivMedical #clinicalMenu_Medical_Vitals").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('clinical_vitals');


            Clinical_FaceSheet.loadFaceSheet();
        }
            /* Start 15/12/2015 Muhammad Irfan to serialize form data for facesheet */
        else if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
            //if (!$("#" + Clinical_Vitals.params.PanelID + " #btnAddVitalsOnNote").prop('disabled')) {
            if ($('#frmClinicalVitals').serialize() != $('#frmClinicalVitals').data('serialize') || ($("#" + Clinical_Vitals.params.PanelID).find('input[name*="selectRecordVitals"]').is(':checked') && Clinical_ProgressNote.AttachedNoteComponentIds.length > 0)) {
                utility.myConfirmNote('1', function () {

                    try {
                        Clinical_Vitals.EditableGrid.datatable.clear().draw();

                        if ($.fn.dataTable.isDataTable("#" + Clinical_Vitals.params.PanelID + " #dgvVitals")) {
                            $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals").dataTable().fnClearTable();
                            $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals").dataTable().fnDestroy();
                            $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals tbody").find("tr").remove();
                        }
                    }
                    catch (ex) {
                        console.log(ex);
                    }

                    if (nextOrPre == true) {
                        Clinical_Vitals.addVitalsToNotes();
                        UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals');
                        if (Clinical_Vitals.controlToInvoke != null) {

                            setTimeout(function () {
                                Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Vitals.controlToInvoke.replace(/\s/g, ''));
                                Clinical_Vitals.controlToInvoke = null;
                            }, 600);

                        }
                    } else {
                        if (Clinical_ProgressNote.AttachedNoteComponentIds != "" || Clinical_ProgressNote.DetachedNoteComponentIds != "") {
                            Clinical_Vitals.addVitalsToNotes();
                            UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals');
                        } else {
                            $.when(Clinical_Vitals.VitalsSave()).then(function () {
                                Clinical_Vitals.addVitalsToNotes();
                                utility.callbackAfterAllDOMLoaded(function () {
                                    Clinical_ProgressNote.AttachedNoteComponentIds = [];
                                    Clinical_ProgressNote.DetachedNoteComponentIds = [];
                                    UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals');
                                });

                            });
                        }
                    }
                },
                "",
                function () {
                    UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals');
                    if (nextOrPre == true) {
                        if (Clinical_Vitals.controlToInvoke != null) {

                            setTimeout(function () {
                                Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Vitals.controlToInvoke.replace(/\s/g, ''));
                                Clinical_Vitals.controlToInvoke = null;
                            }, 600);

                        }
                    }
                });
            }
            else {
                try {
                    Clinical_Vitals.EditableGrid.datatable.clear().draw();

                    if ($.fn.dataTable.isDataTable("#" + Clinical_Vitals.params.PanelID + " #dgvVitals")) {
                        $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals").dataTable().fnClearTable();
                        $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals").dataTable().fnDestroy();
                        $("#" + Clinical_Vitals.params.PanelID + " #dgvVitals tbody").find("tr").remove();
                    }
                    UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals');

                    if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                        if (Clinical_Vitals.controlToInvoke != null) {
                            setTimeout(function () {
                                Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Vitals.controlToInvoke.replace(/\s/g, ''));
                                Clinical_Vitals.controlToInvoke = null;
                            }, 600);
                        }
                    }

                }
                catch (ex) {
                    console.log(ex);
                }
            }

            //    }
            //else {
            //        UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals');
            //        if (nextOrPre == true) {
            //            if (Clinical_Vitals.controlToInvoke != null) {

            //                setTimeout(function () {
            //                    Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Vitals.controlToInvoke.replace(/\s/g, ''));
            //                    Clinical_Vitals.controlToInvoke = null;
            //                }, 400);

            //            }
            //        }
            //    }

            EMRUtility.scrollToPNcomponent('clinical_vitals');
        }
        else {
            if (Clinical_Vitals.params["FromAdmin"] == "0") {
                if (Clinical_Vitals.params != null && Clinical_Vitals.params.ParentCtrl != null) {
                    if (!$("#" + Clinical_Vitals.params.PanelID + " #btnAddVitalsOnNote").prop('disabled')) {
                        Clinical_Vitals.addVitalsToNotes();
                    }
                    UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals');
                }
                else
                    UnloadActionPan(null, 'Clinical_Vitals');
            }
            else {
                //$("#mstrDivMedical #clinicalMenu_Medical_Vitals").remove();
                RemoveAdminTab();
            }
            objDeffered.resolve();
            EMRUtility.scrollToPNcomponent('clinical_vitals');
        }


        return objDeffered;
    },



    //-------------------Editable Grid Methods Starts IrFan---

    // Begin 15-4-2016 Abid Ali , Shows Audit History of selected Record
    rowHistory: function ($row, obj) {

        var _self = obj;
        var vitalId = $row.attr('id');
        var TabId = Clinical_Vitals.params.TabID;
        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote" || Clinical_Vitals.params.ParentCtrl == 'clinicalTabFaceSheet') {
            TabId = 'Clinical_Vitals';
        }
        EMRUtility.showCurrentItemHistory(Clinical_Vitals.params.PanelID, null, null, "VitalSigns,VitalSignsBloodPressure,VitalSignsPulse,VitalSignsRespiration,VitalSignsTemperature", null, TabId, vitalId);
    },
    // End 15-4-2016 Abid Ali , Shows Audit History of selected Record

    rowSave: function ($row, obj) {

        //if (obj.rowValidate($row)) {

        var _self = obj,
        $actions,
        values = [];

        if ($row.hasClass('adding')) {
            $row.removeClass('adding');
        }
        // Begin 12-2-2015 Muhammad Arshad bug # EMR-101 Vitals Workflow in Clinical Module -> Add Vitals-> Diastolic
        var isBPRowsValid = true;
        $row.find("input[id*='BP']").each(function () {
            if ($(this).css("border-bottom-color").indexOf("rgb(255") > -1) {
                isBPRowsValid = false;
                return false;
            }
        });
        if (isBPRowsValid == false) {
            return true;
        }
        // End 12-2-2015 Muhammad Arshad bug # EMR-101 Vitals Workflow in Clinical Module -> Add Vitals-> Diastolic
        values = $row.find('td').map(function () {

            var $this = $(this);

            if ($this.hasClass('expand')) {
                return '<a href="#" class="hidden on-editing expand-row" title="Expand/Collapse Record" ><i class="fa fa-plus-square"></i></a>';
            }
            else if ($this.hasClass('actions')) {

                return _self.datatable.cell(this).data();
            }
            else if ($this.hasClass('ddl')) {
                return $.trim($this.find('select').val());

            } else {
                $obj_ = $this.find('input');

                if ($obj_.attr('type') == "checkbox") {
                    if ($obj_.prop('checked'))
                        return $.trim("True");
                    else
                        return $.trim("False");
                }
                else
                    return $.trim($obj_.val());
            }
        });

        var date = 2;
        var time = 3;
        var sys = 4;
        var dys = 5;
        var pls = 6;
        var tmp = 7;
        var rsp = 8;
        var wet = 9;
        var htft = 10;
        var htin = 11;
        var spo = 14;
        var cmm = 15;
        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
            date = 3;
            time = 4;
            sys = 5;
            dys = 6;
            pls = 7;
            tmp = 8;
            rsp = 9;
            wet = 10;
            htft = 11;
            htin = 12;
            spo = 15;
            cmm = 16;
        }
        if ((values[date] != "" && values[time] != "") && (values[sys] != "" || values[dys] != "" || values[pls] != "" || values[tmp] != "" || values[rsp] != "" || values[wet] != "" || values[htft] != "" || values[htin] != "" || values[spo] != "" || values[cmm] != "")) {
            if ((values[sys] != "" && values[dys] == "") || (values[dys] != "" && values[sys] == "")) {
                utility.DisplayMessages("Please enter value", 3);
            }
            else {
                var id = $row.attr("id");
                var ParentBloodPressureIds = $($row).find('input[name*="BPId"]').map(function () {
                    return this.value.replace("BPId", "");
                }).get().join(',');

                var ParentPulsIds = $($row).find('input[name*="PulsId"]').map(function () {
                    return this.value.replace("PulsId", "");
                }).get().join(',');

                var ParentTempIds = $($row).find('input[name*="TempId"]').map(function () {
                    return this.value.replace("TempId", "");
                }).get().join(',');

                var ParentRespIds = $($row).find('input[name*="RespId"]').map(function () {
                    return this.value.replace("RespId", "");
                }).get().join(',');

                var ParentBPNegationReason = $($row).find('select[name*="BPNegationReason"]').map(function () {
                    return this.value.replace("BPNegationReason", "");
                }).get().join(',');

                var ParentVitalNegationReason = $($row).find('select[name="NegationReason"]').val();

                var NotesId = $row.attr("vitalnotesid");
                var myJSON = $row.getMyJSONByName();
                //var myJSON = CurrentRow.getMyJSON();
                var JSONToSave = myJSON;
                var childRow = Clinical_Vitals.EditableGrid.datatable.row($row).child();
                var ChildRowBPIds = "";//PulsId
                var ChildRowPulsIds = "";
                var ChildRowTempIds = "";
                var ChildRowRespIds = "";
                var ChildBPNegationReasonId = "";
                var ChildNegationReasonId = "";

                if (childRow != null) {
                    $.each(childRow, function (i, item) {
                        var CurrentChildBPIds = $(item).find('input[name*="BPId"]').map(function () {
                            return this.value.replace("BPId", "");
                        }).get().join(',');
                        if (CurrentChildBPIds != null && CurrentChildBPIds != "") {
                            ChildRowBPIds = ChildRowBPIds + "," + CurrentChildBPIds;
                        }

                        var CurrentChildPulsId = $(item).find('input[name*="PulsId"]').map(function () {
                            return this.value.replace("PulsId", "");
                        }).get().join(',');
                        if (CurrentChildPulsId != null && CurrentChildPulsId != "") {
                            ChildRowPulsIds = ChildRowPulsIds + "," + CurrentChildPulsId;
                        }

                        var CurrentChildTempId = $(item).find('input[name*="TempId"]').map(function () {
                            return this.value.replace("TempId", "");
                        }).get().join(',');
                        if (CurrentChildTempId != null && CurrentChildTempId != "") {
                            ChildRowTempIds = ChildRowTempIds + "," + CurrentChildTempId;
                        }

                        var CurrentChildRespId = $(item).find('input[name*="RespId"]').map(function () {
                            return this.value.replace("RespId", "");
                        }).get().join(',');
                        if (CurrentChildRespId != null && CurrentChildRespId != "") {
                            ChildRowRespIds = ChildRowRespIds + "," + CurrentChildRespId;
                        }

                        var CurrentChildBPNegationReasonId = $(item).find('select[name*="BPNegationReason"]').map(function () {
                            return this.value.replace("BPNegationReason", "");
                        }).get().join(',');
                        if (CurrentChildBPNegationReasonId != null && CurrentChildBPNegationReasonId != "") {
                            ChildBPNegationReasonId = ChildBPNegationReasonId + "," + CurrentChildBPNegationReasonId;
                        }

                        //var CurrentChildNegationReasonId = $(item).find('select[name*="NegationReason"]').map(function () {
                        //    return this.value.replace("NegationReason", "");
                        //}).get().join(',');
                        //if (CurrentChildNegationReasonId != null && CurrentChildNegationReasonId != "") {
                        //    ChildNegationReasonId = ChildNegationReasonId + "," + CurrentChildNegationReasonId;
                        //}
                        //var childRowsJSON = item.getMyJSONByName();
                        //JSONToSave = utility.MergeJSON(myJSON, childRowsJSON);
                    });

                    var childRowsJSON = childRow.getMyJSONByName();
                    JSONToSave = utility.MergeJSON(myJSON, childRowsJSON);
                }
                var parsedJSON = JSON.parse(JSONToSave)
                if (ParentBloodPressureIds != null && ParentBloodPressureIds != "") {
                    ParentBloodPressureIds = ParentBloodPressureIds + ChildRowBPIds;
                    parsedJSON.BloodPressureIds = ParentBloodPressureIds;
                }
                if (ParentPulsIds != null && ParentPulsIds != "") {
                    ParentPulsIds = ParentPulsIds + ChildRowPulsIds;
                    parsedJSON.PulseIds = ParentPulsIds;
                }
                if (ParentTempIds != null && ParentTempIds != "") {
                    ParentTempIds = ParentTempIds + ChildRowTempIds;
                    parsedJSON.TempratureIds = ParentTempIds;
                }
                if (ParentRespIds != null && ParentRespIds != "") {
                    ParentRespIds = ParentRespIds + ChildRowRespIds;
                    parsedJSON.RespirationIds = ParentRespIds;
                }

                if (ParentBPNegationReason != null && ParentBPNegationReason != "") {
                    ParentBPNegationReason = ParentBPNegationReason + ChildBPNegationReasonId;
                    parsedJSON.BPNegarionReasonIds = ParentBPNegationReason;
                }

                if (ParentVitalNegationReason != null && ParentVitalNegationReason != "") {
                    // ParentVitalNegationReason = ParentVitalNegationReason + ChildNegationReasonId;
                    parsedJSON.NegationReason = ParentVitalNegationReason;
                }
                if (Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet") {
                    $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #hfPatientId").val(Clinical_FaceSheet.params.patientID);
                }
                parsedJSON.PatientId = $('#' + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #hfPatientId").val();

                //Begin 21-09-2016 Added By Abid Ali Bug#QAC1-68
                if (parsedJSON.VitalSignTime != null && parsedJSON.VitalSignTime != "") {
                    parsedJSON.VitalsTime = parsedJSON.VitalSignTime;
                }
                if (values[date]) {
                    parsedJSON.VitalSignDate = values[date];
                }
                //Begin 21-09-2016 Added By Abid Ali Bug#QAC1-68

                if (NotesId) {
                    parsedJSON.NotesId = NotesId;
                }

                JSONToSave = JSON.stringify(parsedJSON);
                //return false;
                if (id && id > 0) {

                    //Edit Record
                    //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                    var strMessage = "";
                    AppPrivileges.GetFormPrivileges("Medical_Vitals", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        if (strMessage == "") {
                            // Confirm Dialog if Updating record is attached with some note
                            if (NotesId && NotesId > 0 && ((Clinical_Vitals.params.TabID && Clinical_Vitals.params.TabID == "clinicalTabProgressNote") || (Clinical_Vitals.params.ParentCtrl && Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")) && NotesId == Clinical_ProgressNote.params.NotesId) {
                                utility.myConfirm('16', function () {
                                    var vitalDetail = JSON.parse(JSONToSave);
                                    Clinical_Vitals.UpdateVitals(JSONToSave, id, "UPDATE_VITALS_FROM_GRID").done(function (response) {
                                        response = JSON.parse(response);
                                        if (response.status != false) {
                                            $("#mainForm  li#CDSAlert").show();
                                            var patientId = Clinical_Vitals.params.patientID == null ? $('#PatientProfile #hfPatientId').val() : Clinical_Vitals.params.patientID;
                                            $.when(ClinicalCDSDetail.showCDSAlert("", patientId)).then(function () {
                                                if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                                                    Clinical_ProgressNote.LoadCDSAlerts();
                                            });
                                            //Start//28-07-2016//Ahmad Raza//logic to prevent previous vital's attachment with note
                                            if (Date.parse(vitalDetail.VitalSignDate) < Date.parse(new Date().toLocaleDateString())) {
                                                utility.DisplayMessages(response.message, 1);
                                                utility.DisplayMessages("Previous Vital record can not be attached with provider notes.", 3);

                                                return;
                                            } //End//28-07-2016//Ahmad Raza//logic to prevent previous vital's attachment with note
                                            else {
                                                if (NotesId != null && NotesId != '') {
                                                    Clinical_Notes.UpdateSoapText_VitalInNotes(NotesId, id);
                                                    if (NotesId == $('#' + Clinical_Vitals.params.PanelID + ' #hfNoteId').val()) {
                                                        //Clinical_Vitals.FillVitals(id, null, null, null, false);
                                                        Clinical_Vitals.GetVitalInfo(id);
                                                        Clinical_Vitals.VitalsEdit(id);
                                                    }
                                                }

                                                utility.DisplayMessages(response.message, 1);
                                                //Clinical_Vitals.rowDraw($row, _self, values);
                                                Clinical_Vitals.VitalsSearch();
                                            }
                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                        }
                                    });
                                },
                                                function () {

                                                    Clinical_Vitals.VitalsSearch();
                                                }, '1'
                                                );
                            } else {
                                Clinical_Vitals.UpdateVitals(JSONToSave, id, "UPDATE_VITALS_FROM_GRID").done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {
                                        utility.DisplayMessages(response.message, 1);
                                        $("#mainForm  li#CDSAlert").show();
                                        var patientId = Clinical_Vitals.params.patientID == null ? $('#PatientProfile #hfPatientId').val() : Clinical_Vitals.params.patientID;
                                        $.when(ClinicalCDSDetail.showCDSAlert("", patientId)).then(function () {
                                            if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                                                Clinical_ProgressNote.LoadCDSAlerts();
                                        });
                                        //Clinical_Vitals.rowDraw($row, _self, values);
                                        Clinical_Vitals.VitalsSearch();
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            //    }
                            //});
                        }
                        else
                            utility.DisplayMessages(strMessage, 2);
                    });
                    //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                }
                else {
                    //Add Record

                    if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
                        //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('div[id^="Cli_Vitals_"]').length > 0) {


                        //utility.myConfirm('14', function () {
                        //    //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1405
                        //    var vitalDetail = JSON.parse(JSONToSave);
                        //    if (Date.parse(vitalDetail.VitalSignDate) < Date.parse(Clinical_Vitals.params.VitalSignDate)) {
                        //        utility.DisplayMessages("Previous Vital record can not be attached with provider notes.", 3);
                        //        Clinical_Vitals.SaveVitals(JSONToSave, "SAVE_VITALS_FROM_GRID", true).done(function (response) {
                        //            response = JSON.parse(response);
                        //            if (response.status != false) {
                        //                $("#mainForm  li#CDSAlert").show();
                        //                $.when(ClinicalCDSDetail.showCDSAlert("", Clinical_Vitals.params.patientID)).then(function () {
                        //                    if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                        //                        Clinical_ProgressNote.LoadCDSAlerts();
                        //                    EMRUtility.scrollToPNcomponent('clinical_vitals');
                        //                });
                        //                //$row.attr("id", response.BFSPlanId);
                        //                //$row.attr("onclick", "utility.SelectGridRow($(this))");
                        //                utility.DisplayMessages(response.message, 1);
                        //                //Clinical_Vitals.rowDraw($row, _self, values);
                        //                Clinical_Vitals.VitalsSearch();
                        //                Clinical_Vitals.LoadDefaultsVital();

                        //            }
                        //            else {
                        //                utility.DisplayMessages(response.Message, 3);
                        //            }
                        //        });

                        //        return;
                        //    }
                        //    //End 13-07-2016 Edit By Humaira Yousaf Bug#1405
                        //    //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                        //    AppPrivileges.GetFormPrivileges("Medical_Vitals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                        //        if (strMessage == "") {
                        //            Clinical_Vitals.SaveVitals(JSONToSave, "SAVE_VITALS_FROM_GRID", true).done(function (response) {
                        //                response = JSON.parse(response);
                        //                if (response.status != false) {
                        //                    $("#mainForm  li#CDSAlert").show();
                        //                    $.when(ClinicalCDSDetail.showCDSAlert("", Clinical_Vitals.params.patientID)).then(function () {
                        //                        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                        //                            Clinical_ProgressNote.LoadCDSAlerts();
                        //                        EMRUtility.scrollToPNcomponent('clinical_vitals');
                        //                    });
                        //                    if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {

                        //                        Clinical_Vitals.FillVitals(response.VitalsId, null, null, null, false);
                        //                        EMRUtility.scrollToPNcomponent('clinical_vitals');
                        //                    } else {
                        //                        //$row.attr("id", response.BFSPlanId);
                        //                        //$row.attr("onclick", "utility.SelectGridRow($(this))");
                        //                        utility.DisplayMessages(response.message, 1);
                        //                        //Clinical_Vitals.rowDraw($row, _self, values);
                        //                        Clinical_Vitals.VitalsSearch();
                        //                    }
                        //                }
                        //                else {
                        //                    utility.DisplayMessages(response.Message, 3);
                        //                }
                        //            });
                        //        }
                        //        else
                        //            utility.DisplayMessages(strMessage, 2);
                        //    });
                        //    //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                        //}, function () {
                        //Clinical_Vitals.SaveVitals(JSONToSave, "SAVE_VITALS_FROM_GRID", true).done(function (response) {
                        //    response = JSON.parse(response);
                        //    $("#mainForm  li#CDSAlert").show();
                        //    $.when(ClinicalCDSDetail.showCDSAlert("", Clinical_Vitals.params.patientID)).then(function () {
                        //        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                        //            Clinical_ProgressNote.LoadCDSAlerts();
                        //        EMRUtility.scrollToPNcomponent('clinical_vitals');
                        //    });
                        //    if (response.status != false) {

                        //        //$row.attr("id", response.BFSPlanId);
                        //        //$row.attr("onclick", "utility.SelectGridRow($(this))");
                        //        utility.DisplayMessages(response.message, 1);
                        //        //Clinical_Vitals.rowDraw($row, _self, values);
                        //        Clinical_Vitals.VitalsSearch();
                        //        Clinical_Vitals.LoadDefaultsVital();

                        //    }
                        //    else {
                        //        utility.DisplayMessages(response.Message, 3);
                        //    }
                        //});
                        //},
                        //     '1'
                        // );
                        //}
                        //else {
                        AppPrivileges.GetFormPrivileges("Medical_Vitals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                Clinical_Vitals.SaveVitals(JSONToSave, "SAVE_VITALS_FROM_GRID", true).done(function (response) {
                                    response = JSON.parse(response);
                                    $("#mainForm  li#CDSAlert").show();
                                    var patientId = Clinical_Vitals.params.patientID == null ? $('#PatientProfile #hfPatientId').val() : Clinical_Vitals.params.patientID;
                                    $.when(ClinicalCDSDetail.showCDSAlert("", patientId)).then(function () {
                                        if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                                            Clinical_ProgressNote.LoadCDSAlerts();
                                        EMRUtility.scrollToPNcomponent('clinical_vitals');
                                    });
                                    if (response.status != false) {
                                        utility.DisplayMessages(response.message, 1);
                                        Clinical_Vitals.VitalsSearch();
                                        Clinical_Vitals.LoadDefaultsVital();
                                        utility.callbackAfterAllDOMLoaded(function () {
                                            var newlyAddedRow = $("#" + Clinical_Vitals.params.PanelID + ' #dgvVitals tbody tr#' + response.VitalsId + ' td')[0];
                                            $(newlyAddedRow).find('input').trigger('click');
                                        });
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            else
                                utility.DisplayMessages(strMessage, 2);
                        });
                        //  }
                    } else {
                        //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                        AppPrivileges.GetFormPrivileges("Medical_Vitals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                            if (strMessage == "") {
                                Clinical_Vitals.SaveVitals(JSONToSave, "SAVE_VITALS_FROM_GRID", true).done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {
                                        $("#mainForm  li#CDSAlert").show();
                                        var patientId = Clinical_Vitals.params.patientID == null ? $('#PatientProfile #hfPatientId').val() : Clinical_Vitals.params.patientID;
                                        $.when(ClinicalCDSDetail.showCDSAlert("", patientId)).then(function () {
                                            if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                                                EMRUtility.scrollToPNcomponent('clinical_vitals');
                                            Clinical_ProgressNote.LoadCDSAlerts();
                                        });
                                        //$row.attr("id", response.BFSPlanId);
                                        //$row.attr("onclick", "utility.SelectGridRow($(this))");
                                        utility.DisplayMessages(response.message, 1);
                                        //Clinical_Vitals.rowDraw($row, _self, values);
                                        Clinical_Vitals.VitalsSearch();
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            else
                                utility.DisplayMessages(strMessage, 2);
                        });
                        //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                    }
                }
                //}
                //    });


                //}
            }
        }
        else {
            utility.DisplayMessages("Please enter any value", 3);
        }
    },

    rowDetail: function ($row, ClassName) {
        var currentVitalSignId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentVitalSignId > 0) {
            Clinical_Vitals.VitalsEdit(currentVitalSignId);
        }
    },

    rowAdd: function () {
        //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
        AppPrivileges.GetFormPrivileges("Medical_Vitals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                EditableGrid.rowAdd();
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
        //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
    },

    rowRemove: function ($row, obj) {


        var id = $row.attr("id");
        //Begin 03-07-2016 Edit By Humaira Yousaf Bug# 1360
        if (parseInt(id) < 0) {
            Clinical_Vitals.EditableGrid.datatable.row($row.get(0)).remove().draw();
        }
            //End 03-07-2016 Edit By Humaira Yousaf Bug# 1360
        else {
            //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
            var strMessage = "";
            AppPrivileges.GetFormPrivileges("Medical_Vitals", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('1', function () {
                        var selectedValue = id;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            var params = [];
                            params["VitalSignsId"] = selectedValue;
                            //if (Clinical_Vitals.params.PanelID.indexOf("Progress") > -1) {
                            //    params["ParentCtrl"] = "clinicalTabProgressNote";
                            //}
                            //else {
                            //    params["ParentCtrl"] = "clinicalTabVitals";
                            //}
                            params["ParentCtrl"] = "Clinical_Vitals";
                            //params["ParentCtrl"] = Clinical_Vitals.params.PanelID;
                            params["PatientId"] = Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
                            LoadActionPan('Clinical_VitalsComments', params, Clinical_Vitals.params.PanelID);

                            //Clinical_Vitals.DeleteVitals(selectedValue).done(function (response) {
                            //    response = JSON.parse(response);
                            //    if (response.status != false) {

                            //        if ($row.hasClass('adding')) {
                            //        }
                            //        var _self = obj;
                            //        _self.datatable.row($row.get(0)).remove().draw();

                            //        utility.DisplayMessages(response.Message, 1);
                            //    }
                            //    else {
                            //        utility.DisplayMessages(response.Message, 3);
                            //    }
                            //});


                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);


            }, function () {
            },
            //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                '1'
                );
            //    }
            //    else
            //        utility.DisplayMessages(strMessage, 2);
            //});
        }

    },

    rowCancel: function ($row, obj) {


        var _self = obj,
$actions,
i,
data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },

    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },

    rowExpand: function ($row, obj) {
        var _self = obj,
$actions,
values = [];
        var row = _self.datatable.row($row);
        if (row.child.isShown()) {
            // This row is already open - close it
            $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child.hide();
            //tr.removeClass('shown');
        }
        else {
            $row.find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child.show();
        }

    },

    ShowHideEditableGridRows: function (isShow) {

        var VitalsGridId = "#pnlClinicalVitals #dgvVitals";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = Clinical_Vitals.EditableGrid.datatable.row(parentRow);

            if (isShow == true) {

                row.child.show();
                $(parentRow).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");

            }
            else {

                $(parentRow).find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();

            }

        });

    },
    //-------------------Editable Grid Methods Ends---
    // Editable row API call functions

    UpdateVitalsRow: function (VitalSignsData, VitalSignsId) {
        var objData = JSON.parse(VitalSignsData);
        objData["commandType"] = "UPDATE_VITALS";
        objData["VitalSignsId"] = VitalSignsId;
        objData["PatientId"] = Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");
    },
    SaveVitalsRow: function (VitalSignsData) {
        var objData = JSON.parse(VitalSignsData);
        objData["PatientId"] = Clinical_Vitals.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "SAVE_VITALS";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");
    },

    // End Editable row API call functions

    //-----End server calls------

    //----- Calculate BSA/BMI/Weight/Temperature Functions by IrFan
    convertHeight: function (feet) {
        return feet * 12 * 0.0254;
    },
    convertWeight: function (pounds) {
        return pounds / 2.20462262185;
    },

    //Begin 11-30-2015 Muhammad Arshad Bug # EMR-101 Vitals Workflow in Clinical Module -> Add Vitals-> Diastolic
    ValidateBP: function (objSystolic, objDiastolic) {
        var systolicVal = 0;
        var diastolicVal = 0;
        if (objSystolic != null) {
            systolicVal = $(objSystolic).val();
        }
        else if (objDiastolic != null) {
            objSystolic = $($(objDiastolic).parent().parent().prevAll()[0]).find("input[id*='txtSystolic']");
            systolicVal = $(objSystolic).val();

        }
        if (objDiastolic != null) {
            diastolicVal = $(objDiastolic).val();
        }
        else if (objSystolic != null) {
            objDiastolic = $($(objSystolic).parent().parent().nextAll()[0]).find("input[id*='txtDiastolic']");
            diastolicVal = $(objDiastolic).val();
        }
        // Start 01/01/2016 Muhammad Irfan for bug # EMR-191 systolic/ diastolic check
        if ((diastolicVal != "" && systolicVal != "") && (parseInt(diastolicVal) >= parseInt(systolicVal))) {
            $(objDiastolic).css("border", "1px solid red");
            // Faizan Ameen QAC2-528
            // Dated: 21-oct-2016
            // Uncomented the below value , clear the diastolic textbox.

            $(objDiastolic).val("");
            utility.DisplayMessages("Diastolic should be less than Systolic", 3);
            $(objDiastolic).parent().find('i').addClass('hidden');
            return;

        }
        else {
            //Begin 11-30-2015 Muhammad Arshad Bug # EMR-102 Vitals Workflow in Clinical Module -> Add Vitals-> Systolic/Diastolic
            //Validation for Systolic/Diastolic value if one value is there
            if (systolicVal != "") {
                $(objSystolic).css("border", "1px solid #ccc");
                if (diastolicVal == "") {
                    $(objDiastolic).css("border", "1px solid red");
                }
                else {
                    $(objDiastolic).css("border", "1px solid #ccc");
                }

            }
            else if (diastolicVal != "") {
                $(objDiastolic).css("border", "1px solid #ccc");
                if (systolicVal == "") {
                    $(objSystolic).css("border", "1px solid red");
                }
                else {
                    $(objSystolic).css("border", "1px solid #ccc");
                }
            }
            else {
                $(objDiastolic).css("border", "1px solid #ccc");
                $(objSystolic).css("border", "1px solid #ccc");
              
            }
            //End 11-30-2015 Muhammad Arshad Bug # EMR-102 Vitals Workflow in Clinical Module -> Add Vitals-> Systolic/Diastolic
        }

        if (systolicVal != "" && parseFloat(systolicVal) > 120) {
            $(objSystolic).css("color", "red");
            $(objSystolic).parent().parent().find('#systolicwarningicon').removeClass('hidden');
        } else if (systolicVal != "" && parseFloat(systolicVal) <= 120) {
            $(objSystolic).css("color", "black");
            $(objSystolic).parent().parent().find('#systolicwarningicon').addClass('hidden');
        }
        if (diastolicVal != "" && parseFloat(diastolicVal) > 80) {
            $(objDiastolic).css("color", "red");
            $(objDiastolic).parent().parent().find('#diastolicwarningicon').removeClass('hidden');
        } else if (diastolicVal != "" && parseFloat(diastolicVal) <= 80) {
            $(objDiastolic).css("color", "black");
            $(objDiastolic).parent().parent().find('#diastolicwarningicon').addClass('hidden');
        }
        if (systolicVal == "") {
            $(objSystolic).parent().parent().find('#systolicwarningicon').addClass('hidden');
            
        }
        if (diastolicVal == "") {
            $(objDiastolic).parent().parent().find('#diastolicwarningicon').addClass('hidden');
        }

    },
    //End 11-30-2015 Muhammad Arshad Bug # EMR-101 Vitals Workflow in Clinical Module -> Add Vitals-> Diastolic
    calculateBMI2: function (objWeight, objHeightInFeet, TargetCtrl, objheightInches) {
        if (objWeight == null && objHeightInFeet == null && objheightInches == null) {
            if ($('#txtHeightInches').val() != "" && $('#txtHeightFeet').val() == "") {
                $('#txtHeightFeet').attr("disabled", "disabled");
                $('#txtHeightFeet').removeClass('ui-widget-content');
            }
            else {
                $('#txtHeightFeet').removeAttr("disabled");
            }
        }
        else {
            if (objheightInches != null && objheightInches.val() != "" && objHeightInFeet.val() == "") {
                objHeightInFeet.attr("disabled", "disabled");
                objHeightInFeet.removeClass('ui-widget-content');
            }
            else {
                objHeightInFeet.removeAttr("disabled");
            }

        }

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
            HeightInFeet == "" ? HeightInFeet = 0 : HeightInFeet;
        }
        else {
            HeightInFeet = $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtHeightFeet").val();
            if (HeightInFeet == '') {
                HeightInFeet = 0;
            }
        }
        var HeightInches = "";
        if (objheightInches != null) {
            HeightInches = $(objheightInches).val();
            HeightInches == "" ? HeightInches = 0 : HeightInches;
        }
        else {
            HeightInches = $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtHeightInches").val();
            if (HeightInches == '' || HeightInches == '.') {
                HeightInches = 0;
            }
        }
        HeightInches = parseFloat(parseInt(HeightInFeet) * 12 + parseFloat(HeightInches));
        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + Clinical_Vitals.params.PanelID + " #txtBMI");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInches == "" || HeightInches == ".")
            HeightInches = 0;
        else {
            var toFix = 0;
            HeightInches = HeightInches.toString();
            if (HeightInches.toString().indexOf(".") >= 0) {
                if (HeightInches.split('.')[1] != null) {

                    if (HeightInches.split('.')[1].length == 2) {
                        toFix = 2;
                    }
                    else if (HeightInches.split('.')[1].length == 1) {
                        toFix = 1;
                    }
                }
            }
        }
        var result = (weightInLbs / (HeightInches * HeightInches)) * 703;
        var BMI = result.toFixed(2);
        if (WeightInLbs != "" && HeightInches != "" && BMI != "Infinity" && BMI != "NaN") {
            CtrlName.val(BMI);
            var color = Clinical_Vitals.GetBMIColor(BMI);
            CtrlName.css("color", color);
            //if ($("#" + Clinical_Vitals.params.PanelID + " #txtBMI").val() != "" && color == "red") {
             if (color == "red") {
                $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #bmiwarningicon").removeClass('hidden');
                CtrlName.parent().find('i').removeClass('hidden');
            } else {
                $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #bmiwarningicon").addClass('hidden');
            }

        }
        else {
            CtrlName.val('');
            $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #bmiwarningicon").addClass('hidden');
            CtrlName.parent().find('i').addClass('hidden');
        }

    },
    GetBMIColor: function (BMI) {
        if (parseFloat(BMI) >= 25.00 || parseFloat(BMI) <= 18.5) {
            return "red";
        }
        else {
            return "black";
        }
    },
    GetBPColor: function (ctrl, val) {
        if (true) {
            if (parseFloat(val) >= 120) {
                $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #bmiwarningicon").removeClass('hidden');
                return "red";
            }
            else {
                return "black";
            }
        }

    },
    calculateBMI: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + Clinical_Vitals.params.PanelID + " #frmClinicalVitals #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + Clinical_Vitals.params.PanelID + " #txtBMI");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            HeightInFeet = 0;
        else
            var HeightInFeet = HeightInFeet.toString()
        if (HeightInFeet.split('.')[1] != null) {
            var toFix = 0;
            if (HeightInFeet.split('.')[1].length == 2) {
                toFix = 2;
            }
            else if (HeightInFeet.split('.')[1].length == 1) {
                toFix = 1;
            }
        }

        var heightInFeet = parseFloat(HeightInFeet).toFixed(toFix);

        var heightInInches = Clinical_Vitals.convertHeightInches(heightInFeet);

        var result = (weightInLbs / (heightInInches * heightInInches)) * 703;
        var BMI = result.toFixed(2);
        //var weightInKG = Clinical_Vitals.convertWeight(weightInLbs);
        ////console.log('wei' + weightInKG);
        //var heightInMetres = Clinical_Vitals.convertHeight(heightInFeet);
        ////console.log('hei' + heightInMetres);
        //var result = weightInKG / (heightInMetres * heightInMetres);
        ////console.log(BMI);
        //var BMI = result.toFixed(2);
        if (WeightInLbs != "" && HeightInFeet != "" && BMI != "Infinity")
            CtrlName.val(BMI);
        else {
            CtrlName.val('');
        }
    },
    // Start 30/11/2015 Muhammad Irfan Bug # EMR-95
    convertHeightInches: function (feet) {
        var newFeet = feet.toString();
        var a = newFeet.split(".");
        var fee = parseInt(a[0]);
        var inch = parseInt(a[1]);
        if (isNaN(inch))
            return (fee * 12);
        else
            return (fee * 12) + inch;
    },
    ValidateHeightFeet: function (event, obj) {

        var Ctrlvalue = "";
        if (obj != null) {
            Ctrlvalue = $(obj).val();
        }
        if (Ctrlvalue.indexOf('.') != -1) {

            var arr = Ctrlvalue.split('.');

            if (arr.length > 0) {
                var inches = arr[0];
                var discard = arr[1];
                if (inches <= 9) {
                    var heightval = inches <= 9 ? inches : 9;
                    $(obj).val(heightval);
                }
                else {
                    $(obj).val(heightval);
                }
            }
        }
        else {

            var heightval = Ctrlvalue <= 9 ? Ctrlvalue : Ctrlvalue.charAt(0);
            $(obj).val(heightval);
        }
    },
    ValidateHeightInches: function (event, obj, objfoot) {

        var Ctrlvalue = "";
        if (obj != null) {
            Ctrlvalue = $(obj).val();
        }

        if (Ctrlvalue != "") {
            if (Ctrlvalue.indexOf('.') != -1) {
                var arr = Ctrlvalue.split('.');
                if (arr.length > 0) {
                    var leftVal = arr[0];
                    var rightVal = arr[1];
                    var secondinch = rightVal.substring(0, 2);
                    //if (leftVal <= 11 || rightVal <= 99) {
                    var heightval = leftVal <= 11 ? arr[0] + '.' + secondinch : arr[0] + '.' + secondinch;
                    $(obj).val(heightval);
                    //}
                }
            }
            else {
                if (objfoot && objfoot != '' && $("#" + objfoot).val() == "") {
                    var heightval = Ctrlvalue <= 149 ? Ctrlvalue : Ctrlvalue.substring(0, Ctrlvalue.length - 1);
                    $(obj).val(heightval);
                }
                else {
                    var heightval = Ctrlvalue <= 11 ? Ctrlvalue : Ctrlvalue.substring(0, Ctrlvalue.length - 1);
                    $(obj).val(heightval);
                }

            }
        }
        //else {
        //    if (Ctrlvalue.indexOf('.') != -1) {
        //        var arr = Ctrlvalue.split('.');
        //        if (arr.length > 0) {
        //            var leftVal = arr[0];
        //            var rightVal = arr[1];
        //            var secondinch = rightVal.substring(0, 2);
        //            var heightval = leftVal <= 149 ? arr[0] + '.' + secondinch : arr[0] + '.' + secondinch;
        //            $(obj).val(heightval);
        //        }
        //    }
        //    else {
        //        var heightval = Ctrlvalue <= 149 ? Ctrlvalue : Ctrlvalue.substring(0, Ctrlvalue.length - 1);
        //        $(obj).val(heightval);
        //    }
        //}
    },

    // End 30/11/2015 Muhammad Irfan Bug # EMR-95
    convertHeightTo_cm: function (feet) {
        return feet * 12 * 2.54;
    },
    calculateBSA2: function (objWeight, objHeightInFeet, TargetCtrl, objheightInches) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + Clinical_Vitals.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
            HeightInFeet == "" ? HeightInFeet = 0 : HeightInFeet;
        }
        else {
            HeightInFeet = $("#" + Clinical_Vitals.params.PanelID + " #txtHeightFeet").val();
            if (HeightInFeet == '') {
                HeightInFeet = 0;
            }
        }
        var HeightInches = "";
        if (objheightInches != null) {
            HeightInches = $(objheightInches).val();
            HeightInches == "" ? HeightInches = 0 : HeightInches;
        }
        else {
            HeightInches = $("#" + Clinical_Vitals.params.PanelID + " #txtHeightInches").val();
            if (HeightInches == '' || HeightInches == '.') {
                HeightInches = 0;
            }
        }
        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + Clinical_Vitals.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);

        var weightInKG = Clinical_Vitals.convertWeight(weightInLbs);

        HeightInches = parseFloat(parseInt(HeightInFeet) * 12 + parseFloat(HeightInches));
        var heightIn_cm = Clinical_Vitals.convertInchHeightTo_cm(HeightInches);

        var result = 0.007184 * Math.pow(heightIn_cm, 0.725) * Math.pow(weightInKG, 0.425);

        var BSA = result.toFixed(2)
        if (WeightInLbs != "" && HeightInches != "" && BSA != "NaN")
            CtrlName.val(BSA);
        else
            CtrlName.val('');
    },

    convertInchHeightTo_cm: function (inches) {
        return inches * 2.54;
    },
    calculateBSA: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + Clinical_Vitals.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + Clinical_Vitals.params.PanelID + " #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + Clinical_Vitals.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var weightInKG = Clinical_Vitals.convertWeight(weightInLbs);
        //console.log('wei' + weightInKG);
        var heightIn_cm = Clinical_Vitals.convertHeightTo_cm(heightInFeet);
        //console.log('hei' + heightIn_cm);
        var result = 0.007184 * Math.pow(heightIn_cm, 0.725) * Math.pow(weightInKG, 0.425);
        //console.log(BSA);
        var BSA = result.toFixed(2)
        if (WeightInLbs != "" && HeightInFeet != "")
            CtrlName.val(BSA);
        else
            CtrlName.val('');
    },
    kilosToLbs: function (kilo) {

        var nearExact = kilo / 0.45359237;
        var lbs = nearExact;
        return lbs;
        //var oz = (nearExact - lbs) * 16;
        //return {
        //    pounds: lbs,
        //    ounces: oz
        //};
    },
    LbsToKilo: function (lbs) {

        var nearExact = lbs / 2.2;
        var kg = nearExact;
        return kg;

    },
    convertToC: function (Faren) {
        var fTempVal = parseFloat(Faren);
        var cTempVal = (fTempVal - 32) * (5 / 9);
        return cTempVal;
    },
    convertToF: function (Cel) {
        var cTempVal = parseFloat(Cel);
        var fTempVal = (cTempVal * (9 / 5)) + 32;
        return fTempVal;
    },


    //----- End Calculate BSA/BMI/Weight/Temperature Functions

    // Added BY Muhammad Azhar Shahzad
    // Date: 11 Dec 2015
    // Functions Also taken from Progress Note, So Vitals Function for progress note, should be in Vitals File
    //-------------------------Progress Note Functions start--------------------
    //This Function enable/disable add to note button
    enableAddVitals: function (obj) {

        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id, Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                //Clinical_ProgressNote.AttachedNoteComponentIds = [];
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id);
            } if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id);
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
            //$("#" + Clinical_Vitals.params.PanelID).find('input[name*="selectRecordVitals"]:not(#' + obj.id + ')').prop('checked', false);
        } else {
            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id);
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id, Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id);
            }
        }

        if (Clinical_ProgressNote.AttachedNoteComponentIds.length > 0 || Clinical_ProgressNote.DetachedNoteComponentIds.length > 0) {
            $("#" + Clinical_Vitals.params.PanelID + "  #divVitalsPaging #btnAddVitalsOnNote").prop('disabled', false);
        } else {
            $("#" + Clinical_Vitals.params.PanelID + "  #divVitalsPaging #btnAddVitalsOnNote").prop('disabled', true);
        }
    },
    //Call Back function to add component to Progress Note
    addVitalsToNotes: function () {
        var SelectedVitalAttached = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
        var SelectedVitalDetached = Clinical_ProgressNote.DetachedNoteComponentIds.slice();


        //if (SelectedVitalAttached != null && SelectedVitalAttached != '') {
        //    for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
        //        var Vitalid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
        //        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Vitals_Main' + Vitalid).length != 0) {
        //            var index = SelectedVitalAttached.indexOf(Vitalid);
        //            if (index > -1) {
        //                SelectedVitalAttached.splice(index, 1);
        //            }
        //        }
        //    }
        //}
        if (SelectedVitalDetached.join() != null && SelectedVitalDetached.join() != '') {
            Clinical_Vitals.detachVitalsFromNotes(SelectedVitalDetached).done(function () {
                if (SelectedVitalAttached.join() != null && SelectedVitalAttached.join() != '') {
                    var isNewVital = false;
                    //if ($('#dgvVitals tbody tr#' + SelectedVitalAttached.join()).find('td.disableAll').length > 1) {
                    if ($('#dgvVitals tbody tr#' + SelectedVitalAttached.join()).attr('status') == 'disable') {
                        Clinical_Vitals.AttachVitalSignFromNotes(SelectedVitalAttached.join()).done(function (response) {

                            setTimeout(function () {
                                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                                if (Clinical_Vitals.params != null && Clinical_Vitals.params.PanelID.indexOf('pnlClinicalVitals') != -1 && $('#' + Clinical_Vitals.params.PanelID).is(':visible')) {
                                    Clinical_Vitals.VitalsSearch();
                                }
                            }, 5);

                        });
                    } else {
                        Clinical_Vitals.attachVitalsFromNotes(SelectedVitalAttached, isNewVital);
                    }
                } else {
                    Clinical_ProgressNote.saveComponentSOAPText('Vitals');
                }
            });
        } else if (SelectedVitalAttached.join() != null && SelectedVitalAttached.join() != '') {
            var isNewVital = false;
            Clinical_Vitals.attachVitalsFromNotes(SelectedVitalAttached, isNewVital);
        } else {
            Clinical_Vitals.VitalsSearch();
        }

        //attachment check end
        if (Clinical_Vitals.params && Clinical_Vitals.params.ParentCtrl && Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote") {
            UnloadActionPan(Clinical_Vitals.params.ParentCtrl, 'Clinical_Vitals');
        }
    },
    attachVitalsFromNotes: function (AttachedSelectedVitals, isNewVital) {

        Clinical_Vitals.GetVitalInfo(AttachedSelectedVitals.join(), null, isNewVital).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_Vitals.params != null && Clinical_Vitals.params.PanelID.indexOf('pnlClinicalVitals') != -1 && AttachedSelectedVitals.length == 0 && $('#' + Clinical_Vitals.params.PanelID).is(':visible')) {
                    Clinical_Vitals.VitalsSearch();
                }
            }, 5);
        });
    },
    detachVitalsFromNotes: function (SelectedVitalDetached) {
        var dfd = new $.Deferred();
        Clinical_Vitals.DetachVitalSignFromNotes_DBCall(SelectedVitalDetached.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < SelectedVitalDetached.length; i++) {
                    var Vitalid = SelectedVitalDetached[i];
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Vitals_Main' + Vitalid).remove();
                }
                //   utility.DisplayMessages(response.Message, 1);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    //taken from progress note
    //this function will get Vitals Soap Text and attach that to Progress note
    GetVitalInfo: function (VitalSignsId, hideAlertMessage) {
        var dfd = new $.Deferred();
        if (VitalSignsId == null || VitalSignsId == '') {
            return false;
        }

        Clinical_Vitals.Get_Vitalsigns_ForSOAP(VitalSignsId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.VitalSignSoapCount > 0) {
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').length > 0
                            && $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().find('[id^=Cli_Vitals_Main]').length > 0
                            && $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().find('#Cli_Vitals_Main' + VitalSignsId).length == 0) {
                            Clinical_Vitals.detach_ComponentsVitals('Vitals', false, false).done(function () {
                                Clinical_Vitals.CreateVitalBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
                                dfd.resolve('ok');
                            });
                        } else {
                            Clinical_Vitals.CreateVitalBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', hideAlertMessage);
                            dfd.resolve('ok');
                        }
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve('ok');
                }
            }

        });
        return dfd.promise();
    },

    //This Function will check, if Vital Soap is already attached in Progress note, if Vital is not attached than it will create main divs to attach allergy
    CheckVitalExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_vitals').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ObjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="initialVisitBody VitalsComponent"  NoteComponentId="NCDummyId"> <header>' +
                      '<clinical_vitals title="Vitals"  id="' + this.id + '" class="NotesComponent">' +
                      '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Vitals\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Vitals">Vitals</a> ' +
                       '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Vitals\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                      '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Vitals\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                 '</clinical_vitals> </header></li>');
            Clinical_ProgressNote.ShowHideComponetsHeaders();
            $('.initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('.initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_vitals').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    FormateVitalSoapText: function (response, element, index) {
        if (!index) {
            index = 0;
        }
        //var VitalSignSoap_JSON = JSON.parse(response.VitalSignSoap_JSON);
        var VitalSignsBloodPressureSoap_JSON = JSON.parse(response.VitalSignsBloodPressureSoap_JSON)[index];
        var VitalSignsPulseSoap_JSON = JSON.parse(response.VitalSignsPulseSoap_JSON)[index];
        var VitalSignsTempratureSoap_JSON = JSON.parse(response.VitalSignsTempratureSoap_JSON)[index];
        var VitalSignsRespirationSoap_JSON = JSON.parse(response.VitalSignsRespirationSoap_JSON)[index];

        var bloodPressureSoap = "";
        var pulseSoap = "";
        var tempratureSoap = "";
        var respirationSoap = "";
        var vitalSoapText = "";
        if (VitalSignsBloodPressureSoap_JSON) {
            bloodPressureSoap += (VitalSignsBloodPressureSoap_JSON.Systolic != "" && VitalSignsBloodPressureSoap_JSON.Diastolic != "") ? "BP: " + VitalSignsBloodPressureSoap_JSON.Systolic + '/' + VitalSignsBloodPressureSoap_JSON.Diastolic + " mmHg, " : "";
            if (VitalSignsBloodPressureSoap_JSON.Systolic != "" && VitalSignsBloodPressureSoap_JSON.Diastolic != "" && VitalSignsBloodPressureSoap_JSON.Time != null && VitalSignsBloodPressureSoap_JSON.Time != "" && VitalSignsBloodPressureSoap_JSON.length > 1) {
                bloodPressureSoap = bloodPressureSoap.substring(0, bloodPressureSoap.length - 2);
            }
        }
        if (VitalSignsPulseSoap_JSON) {
            pulseSoap += VitalSignsPulseSoap_JSON.PulseResult != "" ? "Pulse: " + VitalSignsPulseSoap_JSON.PulseResult + " bpm, " : "";
            pulseSoap += VitalSignsPulseSoap_JSON.PulseRhythm != "" ? "Rhythm: " + VitalSignsPulseSoap_JSON.PulseRhythm + ", " : "";
            if ((VitalSignsPulseSoap_JSON.PulseResult != "" || VitalSignsPulseSoap_JSON.PulseRhythm != "") && VitalSignsPulseSoap_JSON.Time != null && VitalSignsPulseSoap_JSON.Time != "" && VitalSignsPulseSoap_JSON.length > 1) {
                pulseSoap = pulseSoap.substring(0, pulseSoap.length - 2);
            }
        }
        if (VitalSignsTempratureSoap_JSON) {
            tempratureSoap += VitalSignsTempratureSoap_JSON.TemperatureResult != "" ? "Temperature: " + VitalSignsTempratureSoap_JSON.TemperatureResult + " F, " : "";
        }
        if (VitalSignsRespirationSoap_JSON) {

            respirationSoap += VitalSignsRespirationSoap_JSON.RespirationResult != "" ? "Respiration Rate: " + VitalSignsRespirationSoap_JSON.RespirationResult + " rpm, " : "";
            respirationSoap += VitalSignsRespirationSoap_JSON.RespirationRateRhythm != "" ? "Pattern: " + VitalSignsRespirationSoap_JSON.RespirationRateRhythm + ", " : "";
            if ((VitalSignsRespirationSoap_JSON.RespirationResult != "" || VitalSignsRespirationSoap_JSON.RespirationRateRhythm != "") && VitalSignsRespirationSoap_JSON.Time != null && VitalSignsRespirationSoap_JSON.Time != "" && VitalSignsRespirationSoap_JSON.length > 1) {
                respirationSoap = respirationSoap.substring(0, respirationSoap.length - 2);
            }
        }
        vitalSoapText += bloodPressureSoap;
        vitalSoapText += pulseSoap;
        vitalSoapText += tempratureSoap;
        vitalSoapText += respirationSoap;
        vitalSoapText += (element.Weight == null) ? "" : (element.Weight == "" || element.Weight == "0" ? "" : "Weight: " + element.Weight + " lbs, ");
        vitalSoapText += (element.Height == null) ? "" : (element.Height == "" ? "" : "Height: " + element.Height + " in, ");
        vitalSoapText += (element.BSA == null) ? "" : (element.BSA == "" || element.BSA == "0" ? "" : "BSA: " + element.BSA + " m2, ");

        var color = Clinical_Vitals.GetBMIColor(element.BMI);
        var bmiSoap = '<span style=color:' + color + '>' + element.BMI + ' kg/m2, </span>';

        vitalSoapText += (element.BMI == null) ? "" : (element.BMI == "" || element.BMI == "0" ? "" : 'BMI: ' + bmiSoap);
        vitalSoapText += (element.HeadCr == null) ? "" : (element.HeadCr == "" || element.HeadCr == "0" ? "" : "Head Circumference: " + element.HeadCr + " cm, ");
        vitalSoapText += (element.BloodGroup == null) ? "" : (element.BloodGroup == "" ? "" : "Blood Group: " + element.BloodGroup + ", ");

        vitalSoapText += (element.SPO2 == null) ? "" : (element.SPO2 == "" ? "" : "SPO2:  " + element.SPO2 + "%, ");
        vitalSoapText += (element.InhaledO2Concentration) ? "Inhaled O2 Concentration:  " + element.InhaledO2Concentration + "%, " : "";
        vitalSoapText += (element.OxygenSource == null) ? "" : (element.OxygenSource == "" ? "" : "Oxygen Source: " + element.OxygenSource + ", ");
        vitalSoapText += (element.PeakFlow == null) ? "" : (element.PeakFlow == "" ? "" : "Peak Flow: " + element.PeakFlow + " L/min, ");

        return vitalSoapText;
    },

    //This Function is used to create SOAP html and append it to  Progress note
    CreateVitalBodyHTML: function (response, NoteHTMLCtrl, hideAlertMessage, isUpdateOnly, bNoteSaveCompnt) {
        Clinical_Vitals.CheckVitalExists();
        if (response.VitalSignSoapCount > 0 && response.VitalSignSoap_JSON != null && response.VitalSignSoap_JSON != '') {
            var VitalSignSoap_JSON = JSON.parse(response.VitalSignSoap_JSON);
            $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('section[id^="Cli_Vitals_Main"]').remove();

            var $mainDivVital = $(document.createElement('div'));

            if (VitalSignSoap_JSON == null || VitalSignSoap_JSON.length == 0) {
                return "";
            }
            var VitalId = VitalSignSoap_JSON[0].VitalSignId;
            $.each(VitalSignSoap_JSON, function (index, element) {

                var Vid = element.VitalSignId;
                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Vitals_Main" + Vid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Vitals_" + Vid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Vitals_" + Vid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Vitals_Main" + Vid + '"  ><i class="fa fa-times"></i></a></div> ');

                var vitalSoapText = Clinical_Vitals.FormateVitalSoapText(response, element, index);
                if (element.Comments == null || element.Comments == "") {
                    vitalSoapText += " on " + element.VitalSignDate + " at " + element.VitalSignTime;
                } else {
                    //element.Comments += ", on " + element.VitalSignDate + " at " + element.VitalSignTime;
                }

                if (element.Comments != null) {
                    var tmp = utility.decodeHtml(element.Comments);
                    //var spn = document.createElement('span');
                    //spn.innerHTML = tmp;
                    var cntnt = utility.getContent(tmp);
                    element.Comments = cntnt == "" ? "" : cntnt;
                }
               
                $ListVital.append('<li>' + vitalSoapText + '</li>');
                $ListVital.append((element.Comments == null) ? "" : (element.Comments == "" ? "" : "<li id='vitalcomments'>Comments: " + $('<div/>').html(element.Comments).text() + "</li>"));

                var formatedcomments = $ListVital.find('#vitalcomments div').last();
                var unformatedcomments = $ListVital.find('#vitalcomments');
                if (formatedcomments && formatedcomments.length > 0) {
                    formatedcomments.append(", on " + element.VitalSignDate + " at " + element.VitalSignTime);

                } else {
                    unformatedcomments.append(", on " + element.VitalSignDate + " at " + element.VitalSignTime);
                }

                $DetailsDiv.append($ListVital);
                $SectionBodyVital.append($DetailsDiv);

                $mainDivVital.append($SectionBodyVital);

                // VitalId = VitalId.split(',').length > 1 ? VitalId.split(',' + Vid).join('') : ''
                VitalId = VitalId.split(',' + Vid).join('');
                var CommentHTML = "";
                var CommentsID = $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid + ' ul li:Last').attr('id');
                if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                    CommentHTML = $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid + ' ul li:Last').get(0).outerHTML;
                }
                $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid).html($SectionBodyVital.html());
                $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().find('#Cli_Vitals_Main' + Vid + ' ul').append(CommentHTML);;

            });

            if ($mainDivVital.html() != '' && (isUpdateOnly == null || !isUpdateOnly)) {

                Clinical_Vitals.UpdateVitalHtml($mainDivVital.html(), VitalSignSoap_JSON, NoteHTMLCtrl, hideAlertMessage, null, bNoteSaveCompnt);
                //Clinical_Vitals.UpdateVitalHtml($mainDivVital.html(), VitalId, NoteHTMLCtrl, hideAlertMessage, null, bNoteSaveCompnt);
            } else {
                if (isUpdateOnly) {
                    $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().append($mainDivVital.html());
                }
                Clinical_Vitals.UpdateVitalHtml("", VitalSignSoap_JSON, NoteHTMLCtrl, hideAlertMessage, isUpdateOnly, bNoteSaveCompnt);
                //Clinical_Vitals.UpdateVitalHtml("", VitalId, NoteHTMLCtrl, hideAlertMessage, isUpdateOnly, bNoteSaveCompnt);
                if (!bNoteSaveCompnt)
                    Clinical_ProgressNote.saveComponentSOAPText('Vitals', hideAlertMessage);
            }
        } else {
            if (!bNoteSaveCompnt)
                Clinical_ProgressNote.saveComponentSOAPText('Vitals', hideAlertMessage);
        }
    },

    //From notes Azhar
    CreateVitalBodyHTMLFromNotes: function (Vital, Attached_vitals, NoteHTMLCtrl, hideAlertMessage, bNoteSaveCompnt) {
        Clinical_Vitals.CheckVitalExists();
        if (Vital) {
            var element = Vital;
            var VitalId = element.VitalSignId;
            if (!VitalId || VitalId == 0) {
                return;
            }
            Clinical_Vitals.Get_Vitalsigns_ForSOAP(Attached_vitals[0].VitalSignId).done(function (response) {
                response = JSON.parse(response);
                if (response.VitalSignSoapCount > 0) {
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').length > 0
                        && $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().find('[id^=Cli_Vitals_Main]').length > 0
                        && $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().find('#Cli_Vitals_Main' + VitalId).length == 0) {
                        Clinical_Vitals.detach_ComponentsVitals('Vitals', false, false).done(function () {
                            Clinical_Vitals.CreateVitalBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
                        });
                    } else {
                        Clinical_Vitals.CreateVitalBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', hideAlertMessage);
                    }
                }
            });
        }
    },


    // This Function is called by Progress Notes (Fill Vitals Func, CopyAllNotesCategories)
    UpdateVitalHtml: function (VitalsHtml, VitalSignSoap_JSON, NoteHTMLCtrl, hideAlertMessage, isUpdateOnly, bNotSaveCompt) {
        $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().addClass('initialVisitBody');
        if (VitalsHtml != '') {
            $(NoteHTMLCtrl + ' clinical_Vitals').parent().parent().append(VitalsHtml);
        }

        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (VitalsHtml != '') {
            $.each(VitalSignSoap_JSON, function (i, item) {

                Clinical_Vitals.AttachVitalSignFromNotes(item.VitalSignId, hideAlertMessage, bNotSaveCompt);

            });

        }

    },

    //This Functions ask for Detaching Vital sign from Progress Note for current Patient Selected
    DetachVitalSignFromNotes: function (VitalSignId) {
        var strMessage = "";
        // AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                var selectedValue = VitalSignId.replace('Cli_Vitals_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_Vitals.DetachVitalSignFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + VitalSignId).remove();

                            Clinical_ProgressNote.saveComponentSOAPText('Vitals', true);
                            setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                            //if (Clinical_Vitals.params != null && Clinical_Vitals.params.PanelID.indexOf('pnlClinicalVitals') != -1) {
                            //    Clinical_Vitals.VitalsSearch();
                            //}
                            utility.DisplayMessages(response.Message, 1);
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

    //This Functions attached Vital sign to Progress Note for current Patient Selected
    AttachVitalSignFromNotes: function (VitalSignId, hideAlertMessage, bNoteSaveCompnt) {
        var dfd = new $.Deferred();

        var selectedValue = VitalSignId;
        if (selectedValue == "" || selectedValue == "undefined") {
            dfd.resolve('ok');
        }
        else {
            Clinical_Vitals.AttachVitalSignFromNotes_DBCall(selectedValue).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var attachedVitals = JSON.parse(response.VitalsLoad_JSON);
                    //console.log(JSON.parse(response.VitalsLoad_JSON));
                    if (Number(VitalSignId) != Number(attachedVitals[0].VitalSignId)) {
                        response.VitalSignSoapCount = 1;
                        response.VitalSignSoap_JSON = response.VitalsLoad_JSON;
                        Clinical_Vitals.detach_ComponentsVitals('Vitals', false, false, true);
                        Clinical_Vitals.Get_Vitalsigns_ForSOAP(attachedVitals[0].VitalSignId).done(function (responseVitals) {
                            if (responseVitals != "") {
                                responseVitals = JSON.parse(responseVitals);
                                if (responseVitals.status != false) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Vitals_Main' + VitalSignId).remove();
                                    Clinical_Vitals.CreateVitalBodyHTML(responseVitals, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', hideAlertMessage, true);

                                    dfd.resolve(attachedVitals.VitalSignId);
                                }
                                else {
                                    utility.DisplayMessages(responseVitals.Message, 3);
                                }
                            }
                        });

                        return false;
                    }
                    if (!bNoteSaveCompnt)
                        Clinical_ProgressNote.saveComponentSOAPText('Vitals', hideAlertMessage);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
                dfd.resolve('ok');
            });
        }
        return dfd.promise();
    },

    //If Attached Vitals Made new inseration to Vitals Table than good ids should be attached to HTML
    // This Function is called by Functions (AttachVitalSignFromNotes)
    FillVitals: function (VitalId, data, PatientId, VisitId, IsNew, vitalSignDate) {
        //if (IsNew == true && $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().find('section[id*="Cli_Vitals_Main"]').length > 0) {
        //    utility.myConfirm('14', function () {
        //        //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1405
        //        if (Date.parse(vitalSignDate) < Date.parse(Clinical_Vitals.params.VitalSignDate)) {
        //            utility.DisplayMessages("Previous Vital record can not be attached with provider notes.", 3);
        //            return;
        //        }
        //        //End 13-07-2016 Edit By Humaira Yousaf Bug#1405
        //        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfVitalsId').val(VitalId);
        //        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfVitalsData').val(data);
        //        Clinical_Vitals.GetVitalInfo(VitalId);
        //        EMRUtility.scrollToPNcomponent('clinical_vitals');
        //    }, function () { },
        //           '1'
        //       );
        //} else {
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfVitalsId').val(VitalId);
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #hfVitalsData').val(data);

        //  var VitalsHtml = Clinical_ProgressNote.CreateHTML(data, VitalId);
        Clinical_Vitals.GetVitalInfo(VitalId);
        EMRUtility.scrollToPNcomponent('clinical_vitals');
        // }
        //  Clinical_Vitals.UnLoadTab();    //fixed bug EMR-1305
    },

    //If Vitals Component which is dropeed in Progress note has no Vital attached, than it will call for Latest Vital for this patient
    GetLatestVitalByPatientId: function (hideAlertMessage) {
        Clinical_Vitals.GetLatestVitalByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                // if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').length > 0 && $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().find('#Cli_Vitals_Main' + VitalSignsId).length == 0) {
                if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().find('section').length > 0) {
                    Clinical_Vitals.detach_ComponentsVitals('Vitals', true, true).done(function (response) {
                        Clinical_Vitals.CreateVitalBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', hideAlertMessage);
                    });
                } else {
                    Clinical_Vitals.CreateVitalBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', hideAlertMessage);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },


    //Detaching Previously Attached Vitals
    detach_ComponentsVitals: function (ComponentName, IsUpdate, VitalComponentRemove, hideAlertMessage) {

        var dfd = new $.Deferred();
        var Clinical_VitalIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_vitals').parent().parent().find('section[id*="Cli_Vitals_Main"]').map(function () {
            return this.id.replace("Cli_Vitals_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .VitalsComponent').attr('NoteComponentId');

        if (VitalComponentRemove) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Vitals']").remove();
            if (Clinical_ProgressNote.params["TemplateName"])
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_vitals').parent().parent().addClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_vitals').parent().parent().remove();
        }
        else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_vitals').parent().parent().find('section[id*="Cli_Vitals_Main"]').remove();
        }

        if (Clinical_VitalIds == "" || Clinical_VitalIds == "undefined") {

            if (hideAlertMessage == null) {
                //Start 31-05-2016 Edit By Humaira Yousaf Bug# EMR-1224
                if (NoteComponentId && NoteComponentId != "NCDummyId") {
                    var promise = [];
                    if (Clinical_ProgressNote.params["TemplateName"]) {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().addClass('hidden');
                        promise.push(Clinical_ProgressNote.saveComponentSOAPText('Vitals', true))
                    }
                    else
                        promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                    $.when.apply($, promise).done(function () {
                        if (Clinical_ProgressNote.params["TemplateName"] == "")
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().remove();
                        Clinical_ProgressNote.ShowHideComponetsHeaders();
                        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    });
                }
                utility.DisplayMessages('Successfully Deleted', 1);
                //End 31-05-2016 Edit By Humaira Yousaf Bug# EMR-1224
            }

            dfd.resolve('ok');
        }
        else {
            Clinical_Vitals.DetachVitalSignFromNotes_DBCall(Clinical_VitalIds).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {


                        if (NoteComponentId && NoteComponentId != "NCDummyId") {
                            var promise = [];
                            if (Clinical_ProgressNote.params["TemplateName"]) {
                                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().addClass('hidden');
                                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Vitals', true))
                            }
                            else
                                promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                            $.when.apply($, promise).done(function () {
                                if (Clinical_ProgressNote.params["TemplateName"] == "")
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_vitals').parent().parent().remove();
                                Clinical_ProgressNote.ShowHideComponetsHeaders();
                                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                            });
                        }
                        utility.DisplayMessages(response.Message, 1);
                    }
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

                dfd.resolve('ok');
            });
        }

        return dfd.promise();
    },

    //This Function will make vital copy
    CopyVital: function (elementVital) {
        if (elementVital != null & elementVital != '') {
            Clinical_Vitals.CopyVital_Dbcall(elementVital, Clinical_ProgressNote.params.NotesId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    Clinical_Vitals.GetVitalInfo(response.VitalsId);
                }
            });
        }
    },

    //-----Server calls of Notes----------
    DetachVitalSignFromNotes_DBCall: function (VitalSignId) {
        var objData = {
        };
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["VitalSignId"] = VitalSignId;
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
        objData["commandType"] = "detach_vitalsign_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    CopyVital_Dbcall: function (VitalSignsId, NotesId) {

        var objData = new Object();
        objData["VitalSignsId"] = VitalSignsId;
        objData["NotesId"] = NotesId;
        objData["commandType"] = "COPY_VITALSIGNS";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },

    Get_Vitalsigns_ForSOAP: function (VitalSignsId) {

        var objData = new Object();
        objData["VitalSignsId"] = VitalSignsId;
        objData["commandType"] = "GET_VITALSIGNS_FORSOAP";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },

    AttachVitalSignFromNotes_DBCall: function (VitalSignId) {
        var objData = {
        };
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["VitalSignId"] = VitalSignId;
        //Start 27-09-2017 Edit By Humaira Yousaf Bug#EMR-4833
        if (Clinical_ProgressNote.params.VisitId) {
            objData["VisitId"] = Clinical_ProgressNote.params.VisitId;
        } else {
            objData["VisitId"] = 0;
        }
        //End 27-09-2017 Edit By Humaira Yousaf Bug#EMR-4833
        if (Clinical_ProgressNote.params.patientID == "" || Clinical_ProgressNote.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        }
        objData["commandType"] = "attach_vitalsign_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    AttachVitalSignFromNotes_DBCall_ForCopyPrevousNote: function (VitalSignId, NotesId) {
        var objData = {
        };
        objData["NotesId"] = NotesId;
        objData["VitalSignId"] = VitalSignId;
        //Start 27-09-2017 Edit By Humaira Yousaf Bug#EMR-4833
        if (Clinical_ProgressNote.params.VisitId) {
            objData["VisitId"] = Clinical_ProgressNote.params.VisitId;
        } else {
            objData["VisitId"] = 0;
        }
        //End 27-09-2017 Edit By Humaira Yousaf Bug#EMR-4833
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "attach_vitalsign_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    //this function get Latest Vital Attach to Patient based on Patient Id From  Server
    GetLatestVitalByPatientId_DBCall: function () {
        var objData = {
        };
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["UserId"] = globalAppdata["AppUserId"];
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["commandType"] = "getlatest_vitalby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Vitals");
    },
    //end taken from progress note

    //Begin 13-07-2016 Edit By Humaira Yousaf Bug#1398
    addNewVital: function () {
        Clinical_Vitals.ResetFormData();
    },

    //End 13-07-2016 Edit By Humaira Yousaf Bug#1405

    EnableDisableSaveButton: function (rowId) {
        var empty = 0;

        $("#" + Clinical_Vitals.params.PanelID + " #" + rowId + " input[type='text']").each(function (index, item) {
            if (index > 1 && index != 9 && index != 10) {
                if (this.value == "") {
                    empty++;
                }
            }
        });
        if (empty == 9) {
            $($("#" + Clinical_Vitals.params.PanelID + " #" + rowId + " a")[0]).addClass('disableAll');
        }
        else {
            $($("#" + Clinical_Vitals.params.PanelID + " #" + rowId + " a")[0]).removeClass('disableAll');
        }
    },
    //this functions generates HTML for Vitals, whicha are attched.
    // This function is called by Progress note (GetLatestVitalByPatientId Func)
    //CreateHTMLVitals: function (response) {
    //    var $mainDivVital = $(document.createElement('div'));
    //    if (response.VitalsCount > 0) {
    //        var VitalsLoadJSONData = JSON.parse(response.VitalsLoad_JSON).VitalSignId;
    //        $.each(VitalsLoadJSONData, function (i, item) {
    //            var $SectionBodyVital = $(document.createElement('section'));
    //            $SectionBodyVital.attr('id', "Cli_Vitals_Main" + item.VitalSignId);

    //            var $DetailsDiv = $(document.createElement('div'));
    //            $DetailsDiv.attr('id', "Cli_Vitals_" + item.VitalSignId);

    //            var $ListVital = $(document.createElement('ul'));
    //            $ListVital.attr('class', 'list-unstyled');
    //            $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Vitals_" + item.VitalSignId + '"><i class="fa fa-edit"></i></a>' +
    //                '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Vitals_Main" + item.VitalSignId + '"  ><i class="fa fa-times"></i></a></div> ');


    //            $ListVital.append("<li>Weight:" + item.Weight + " </li><li>Height:" + item.Height + " </li>");
    //            $DetailsDiv.append($ListVital);
    //            $SectionBodyVital.append($DetailsDiv);
    //            if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_Vitals #Cli_Vitals_' + item.VitalSignId).length == 0) {


    //                $mainDivVital.append($SectionBodyVital);
    //            } else {
    //                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_Vitals #Cli_Vitals_' + item.VitalSignId).html($SectionBodyVital.html());
    //            }
    //        });
    //    }
    //    return $mainDivVital.html();
    //},
    //ChangeHTML: function (response) {
    //    if (response.VitalsCount > 0) {
    //        var attachedVitals = JSON.parse(response.VitalsLoad_JSON);
    //        $.each(attachedVitals, function (i, item) {
    //            if (item.CopyParentId != null && item.CopyParentId != item.VitalSignId) {
    //                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("[name='Cli_Vitals_Main" + item.CopyParentId + "']").attr('name', "Cli_Vitals_Main" + item.VitalSignId);
    //                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("[name='Cli_Vitals_" + item.CopyParentId + "']").attr('name', "Cli_Vitals_" + item.VitalSignId);
    //                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("[title='Cli_Vitals_Main" + item.CopyParentId + "']").attr('title', "Cli_Vitals_Main" + item.VitalSignId);
    //                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("[title='Cli_Vitals_" + item.CopyParentId + "']").attr('title', "Cli_Vitals_" + item.VitalSignId);
    //                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("#Cli_Vitals_Main" + item.CopyParentId).attr('id', "Cli_Vitals_Main" + item.VitalSignId);
    //                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find("#Cli_Vitals_" + item.CopyParentId).attr('id', "Cli_Vitals_" + item.VitalSignId);

    //            }
    //        });
    //    }

    //},

    //this function is removed due to removal of import funcationality of vitals from Grid
    ////This Function Imports Vitals to Progress Note of clinical
    //ImportToProgressNote: function (event) {
    //    if (event != null) {
    //        event.stopPropagation();
    //    }

    //    var Clinical_VitalIds = $('#' + Clinical_Vitals.params["PanelID"] + ' input[id*="chkCli_Vitals"]:checked').map(function () {
    //        return this.id.replace("chkCli_Vitals", "");
    //    }).get().join(',');
    //    if (Clinical_VitalIds == "") {
    //        utility.DisplayMessages("Please select any Vital to continue.", 4);
    //        return false;
    //    } else if (Clinical_VitalIds.split(',').length > 1) {
    //        utility.DisplayMessages("Please select one Vital to continue.", 4);
    //        return false;
    //    }
    //    var data = getTableDataVitals($('#' + Clinical_Vitals.params["PanelID"] + ' #dgvVitals'));

    //    Clinical_Vitals.FillVitals(Clinical_VitalIds, data);
    //},

    //FillVitals: function (VitalId, data, PatientId, VisitId) {

    //    $('#' + Clinical_Vitals.params["PanelID"] + ' #hfVitalsId').val(VitalId);
    //    $('#' + Clinical_Vitals.params["PanelID"] + ' #hfVitalsData').val(data);

    //    var VitalsHtml = Clinical_Vitals.CreateHTML(data);

    //    $('#' + Clinical_Vitals.params["PanelID"] + ' #ProgressnoteHTML clinical_Vitals').append(VitalsHtml);
    //    UnloadActionPan(Clinical_Vitals.params["ParentCtrl"]);
    //},
    //-------------------------Progress Note Functions End--------------------- 
    AutoLoadKeyWords: function (e) {
        console.log(e.key);
    }
}

function getTableDataVitals(table) {
    var data = [];
    table.find('tr').each(function (rowIndex, r) {
        var cols = [];
        if (rowIndex == 0) {
            $(this).find('th,td').each(function (colIndex, c) {
                cols.push(c.textContent);
            });
            data.push(cols);
        } else if ($(this).find('th,td input[type=checkbox]').is(':checked')) {
            $(this).find('th,td').each(function (colIndex, c) {
                cols.push(c.textContent);
            });
            data.push(cols);
        }

    });
    return data;
}