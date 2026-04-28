CCM_Vitals = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        CCM_Vitals.params = params;

        VitalSignsId = CCM_Vitals.params.VitalSignsId;

        var self = $('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals");
        self.loadDropDowns(true).done(function () {
            if (VitalSignsId != null && VitalSignsId != "" && VitalSignsId > 0) {
                CCM_Vitals.VitalsEdit(VitalSignsId);

            } else {
                CCM_Vitals.ResetFormData();
                CCM_Vitals.params.VitalSignsId = -1;
                CCM_Vitals.params.mode = "Add";
            }
        });
        CCM_Vitals.ValidateVitals();
        $('#' + CCM_Vitals.params.PanelID + " #hfPatientId").val(CCM_Vitals.params.PatientId);
        CCM_Vitals.domReadyFunction();
    },
    domReadyFunction: function () {
        utility.CreateDatePicker(CCM_Vitals.params.PanelID + '  #dpVitalsDate', function () {
        }, true);
        $('#' + CCM_Vitals.params.PanelID + ' #tpVitalsTime').timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });
        $('#' + CCM_HRAssessmentDetail.params.PanelID + ' [data-plugin-toggle]').each(function () {
            var $this = $(this),
                opts = {};

            var pluginOptions = $this.data('plugin-options');
            if (pluginOptions)
                opts = pluginOptions;

            $this.themePluginToggle(opts);
        });
        $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #frmClinicalVitals [data-plugin-keyboard-numpad]').keyboard({
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
                        CCM_Vitals.ValidateHeightFeet(e, keyboard.$preview);
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

    FillAllDropDowns: function (VitalSignsId) {
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlSmokingStatus', 'GetAllVitalSigns', true, 'Smoking Status');
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlSeverityofPain', 'GetAllVitalSigns', true, 'Severity of Pain');
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlBloodType', 'GetAllVitalSigns', true, 'Blood Type');
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlSmokingStatus', 'GetAllVitalSigns', true, 'Smoking Status');
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlPosition' + VitalSignsId, 'GetAllVitalSigns', true, 'Position');
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlCuffLocation' + VitalSignsId, 'GetAllVitalSigns', true, 'Cuff Location');
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlCuffSize' + VitalSignsId, 'GetAllVitalSigns', true, 'Cuff Size');
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlRythm' + VitalSignsId, 'GetAllVitalSigns', true, 'Rhythm');
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlPattern' + VitalSignsId, 'GetAllVitalSigns', true, 'Pattern');
        CacheManager.BindDropDownsByEntityID('#' + CCM_Vitals.params.PanelID + ' #ddlMethod' + VitalSignsId, 'GetAllVitalSigns', true, 'Method');
    },

    InitializeAllTimeWidget: function (VitalSignsId, RowType, event) {
        $('#' + CCM_Vitals.params.PanelID + ' #tpBloodPressureTime' + VitalSignsId).timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });
        if (parseInt(VitalSignsId) <= 0) {
            $('#' + CCM_Vitals.params.PanelID + ' #tpBloodPressureTime' + VitalSignsId).timepicker('setTime', new Date());
            $('#' + CCM_Vitals.params.PanelID + ' #tpPulseTime' + VitalSignsId).timepicker('setTime', new Date());
            $('#' + CCM_Vitals.params.PanelID + ' #tpTempratureTime' + VitalSignsId).timepicker('setTime', new Date());
            $('#' + CCM_Vitals.params.PanelID + ' #tpRespirationTime' + VitalSignsId).timepicker('setTime', new Date());
        }

        $('#' + CCM_Vitals.params.PanelID + ' #tpPulseTime' + VitalSignsId).timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });

        $('#' + CCM_Vitals.params.PanelID + ' #tpTempratureTime' + VitalSignsId).timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });

        $('#' + CCM_Vitals.params.PanelID + ' #tpRespirationTime' + VitalSignsId).timepicker().on('changeTime.timepicker', function (e) {
            disableFocus: false
        });

        if (RowType == "BloodPressure") {
            CCM_Vitals.BindNumPad("#BloodPressure" + VitalSignsId);
        }
        else if (RowType == "Pulse") {
            CCM_Vitals.BindNumPad("#Pulse" + VitalSignsId);
        }
        else if (RowType == "Temprature") {
            CCM_Vitals.BindNumPad("#Temprature" + VitalSignsId);
        }
        else if (RowType == "Respiration") {
            CCM_Vitals.BindNumPad("#Respiration" + VitalSignsId);
        }
    },

    BindNumPad: function (DivId) {
        $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals ' + DivId).find('[data-plugin-keyboard-numpad]').keyboard({
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

    ResetFormData: function () {
        $("#" + CCM_Vitals.params.PanelID + " #divTemplateRespiration").nextAll().remove();
        $("#" + CCM_Vitals.params.PanelID + " #divTemplateTemprature").nextAll().remove();
        $("#" + CCM_Vitals.params.PanelID + " #divTemplatePulse").nextAll().remove();
        $("#" + CCM_Vitals.params.PanelID + " #divTemplateBloodPressure").nextAll().remove();
        //Add default rows

       // $('#' + CCM_Vitals.params.PanelID + ' #btnsave').text('Add');

        $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals').resetAllControls(null);
        ($('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals')).find('input[type=text], textarea').val('');
        $('#' + CCM_Vitals.params.PanelID + ' #dpVitalsDate').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
        $('#' + CCM_Vitals.params.PanelID + ' #tpVitalsTime').timepicker('setTime', new Date());
        CCM_Vitals.AddNewRow('BloodPressure', CCM_Vitals.params.PanelID + ' #divTemplateBloodPressure', "-1");
        CCM_Vitals.AddNewRow('Pulse', CCM_Vitals.params.PanelID + ' #divTemplatePulse', "-1");
        CCM_Vitals.AddNewRow('Temprature', CCM_Vitals.params.PanelID + ' #divTemplateTemprature', "-1");
        CCM_Vitals.AddNewRow('Respiration', CCM_Vitals.params.PanelID + ' #divTemplateRespiration', "-1");

        CCM_Vitals.params["VitalSignsId"] = "";
        CCM_Vitals.params["mode"] = "Add";
    },

    AddNewRow: function (RowType, TemplateRowId, RowId, CurrentItemJSON, event) {
        var scrollTopPx = $(window).scrollTop();
        if (TemplateRowId.indexOf('pnl') < 0) {
            TemplateRowId = CCM_Vitals.params.PanelID + " #" + TemplateRowId;
        }

        var regexpr = new RegExp(RowType, "g");
        if (TemplateRowId != null && TemplateRowId != "") {
            var TemplateRow = $("#" + TemplateRowId);
            var newRow = TemplateRow.clone().css("display", "");
            if (RowId != null && parseInt(RowId) < 0) {
                var LastChildRowId = $('#' + CCM_Vitals.params.PanelID + " section#" + RowType + " div[id*='" + RowType + "']:last").attr("id");
                if (LastChildRowId.indexOf("divTemplate") > -1) {
                    RowId = RowType + "-1";
                }
                else {
                    LastChildRowId = $('#' + CCM_Vitals.params.PanelID + " section#" + RowType + " div[id*='" + RowType + "']:eq(1)").attr("id");

                    var intRowId = LastChildRowId.replace(regexpr, "");
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
            ////Insert Child row on top of existing row
            newRow.insertAfter($("#" + TemplateRowId));
            if (CurrentItemJSON != null && CurrentItemJSON != "") {
                utility.bindMyJSONByName(true, CurrentItemJSON, false, newRow).done(function () {
                });
            }
            CCM_Vitals.InitializeAllTimeWidget(intCurrentRowId, RowType);
        }

        $(window).scrollTop(scrollTopPx);
    },

    ValidateVitals: function () {
        $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals').bootstrapValidator('destroy');
        $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   VitalSignDate11: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   VitalsTime11: {
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
            CCM_Vitals.VitalsSave();
        });
    },

    isDetailExists: function (section) {
        var DetailExists = false;
        var sectionDetails = "";
        for (var i = 0; i < section.length; i++) {

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
                var self = $('#' + CCM_Vitals.params.PanelID + ' section#' + sectionDetails).find('[type=text]:not([data-plugin-timepicker]),textarea,select').each(function () {

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

    VitalsSave: function () {
        var isNonEmpty = false;

         if (CCM_Vitals.bIsFirstLoad) { CCM_Vitals.params.mode = "Add"; CCM_Vitals.bIsFirstLoad = false; } else CCM_Vitals.params.mode = "Edit";

        $($('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals section#BloodPressure div#divTemplateBloodPressure").nextAll().toArray().reverse()).each(function (i, item) {
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
        $($('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals section#Pulse div#divTemplatePulse").nextAll().toArray().reverse()).each(function (i, item) {
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
        $($('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals section#Temprature div#divTemplateTemprature").nextAll().toArray().reverse()).each(function (i, item) {
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
        $($('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals section#Respiration div#divTemplateRespiration").nextAll().toArray().reverse()).each(function (i, item) {
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

        var selfBloodPressure = $('#' + CCM_Vitals.params.PanelID + " #BloodPressure");
        var myBloodPressureJSON = selfBloodPressure.getMyJSONByName();
        var isBPRowsValid = true;
        $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals section#BloodPressure input').each(function () {
            if ($(this).css("border-bottom-color").indexOf("rgb(255") > -1) {
                isBPRowsValid = false;
                return false;
            }
        });

        var BloodPressureIds = $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals input[id*="BPId"]').map(function () {
            return this.id.replace("BPId", "");
        }).get().join(',');

        $('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals #BloodPressureIds").val(BloodPressureIds);

        var PulseIds = $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals input[id*="PulsId"]').map(function () {
            return this.id.replace("PulsId", "");
        }).get().join(',');

        $('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals #PulseIds").val(PulseIds);

        var TempratureIds = $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals input[id*="TempId"]').map(function () {
            return this.id.replace("TempId", "");
        }).get().join(',');

        $('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals #TempratureIds").val(TempratureIds);

        var RespirationIds = $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals input[id*="RespId"]').map(function () {
            return this.id.replace("RespId", "");
        }).get().join(',');

        $('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals #RespirationIds").val(RespirationIds);

        //BEGIN 23-12-2015 Muhammad Arshad Bug# EMR-98 Vitals Workflow in Clinical Module -> Add Vitals-> Add Button
        var isVitalsValid = false;

        isVitalsValid = CCM_Vitals.isDetailExists($('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals section'));

        if ($('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals #dpVitalsDate").val() == "" || $('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals #tpVitalsTime").val() == "") {
            isVitalsValid = false;
        }
        if (isVitalsValid == true && isBPRowsValid == true) {
            var strMessage = "";
            var self = $('#' + CCM_Vitals.params.PanelID + " #frmClinicalVitals");
            var myJSON = self.getMyJSONByName();
            if (CCM_Vitals.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("Medical_Vitals", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {

                        CCM_Vitals.SaveVitals(myJSON, "", false).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                CCM_Vitals.params.VitalSignsId = response.VitalsId;
                                if (CCM_HRAssessmentDetail != null) {
                                    CCM_HRAssessmentDetail.params.VitalSignsId = response.VitalsId;
                                }                                
                                CCM_Vitals.params.mode = "Edit"
                                CCM_Vitals.VitalsEdit(CCM_Vitals.params.VitalSignsId);
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
                //End//21/12/2015//Ahmad Raza//Logic implemented to check privileges
            }
            else if (CCM_Vitals.params.mode == "Edit") {
                //Start//21/12/2015//Ahmad Raza//Logic implemented to check privileges
                AppPrivileges.GetFormPrivileges("Medical_Vitals", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        CCM_Vitals.updateVitalRecord(myJSON);

                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
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
        }

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
                if (Ctrlvalue.indexOf('.') != -1) {
                    var arr = Ctrlvalue.split('.');

                    if (arr.length > 0) {
                        var inches = arr[1];
                        var feet = arr[0];
                        var secondinch = inches.substring(0, 2);
                        var heightval = parseFloat(feet * 12) + parseFloat(inches);
                        objData["Height"] = heightval;
                    }
                }
                else {
                    var heightval = parseFloat(objData["Height"]) * 12;;
                    objData["Height"] = heightval;
                }
            }
        }
        objData["RiskAssessmentId"] = CCM_Vitals.params.HRAssessmentId;
        var data = JSON.stringify(objData);

        //console.log(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");
    },

    updateVitalRecord: function (myJSON) {
        var vitalDetail = JSON.parse(myJSON);
        CCM_Vitals.UpdateVitals(myJSON, CCM_Vitals.params.VitalSignsId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
                CCM_Vitals.VitalsEdit(CCM_Vitals.params.VitalSignsId);
            }
            else {
                utility.DisplayMessages(response.message, 3);
            }
        });
    },
    //************* Update and Fill **************\\
    VitalsEdit: function (VitalSignsId) {
        //Conflict resolve of Functions Name By Azhar shahzad on 11 dec 2015
        CCM_Vitals.FillVitals_DBCall(VitalSignsId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                // $("#" + CCM_Vitals.params.PanelID + " #btnCopyVitals").css("display", "");
                var vitals_detail = JSON.parse(response.VitalsFill_JSON);
                var vitals_BloodPressure_detail = JSON.parse(response.VitalsBlooPressureFill_JSON);
                var vitals_Pulse_detail = JSON.parse(response.VitalsPulseFill_JSON);
                var vitals_Temperature_detail = JSON.parse(response.VitalsTemperatureFill_JSON);
                var vitals_Respiration_detail = JSON.parse(response.VitalsRespirationFill_JSON);
                // Remove Existing Blood Pressure Rows
                $("#" + CCM_Vitals.params.PanelID + " #divTemplateBloodPressure").nextAll().remove();
                $.each(vitals_BloodPressure_detail, function (i, item) {
                    if (parseInt(item.CurrentBloodPressureId) > 0) {

                        CCM_Vitals.AddNewRow('BloodPressure', CCM_Vitals.params.PanelID + ' #divTemplateBloodPressure', item.CurrentBloodPressureId, item);
                        //var CurrentBPRow = $("#BloodPressure")
                    }
                });

                if (vitals_BloodPressure_detail.length < 1) {
                    CCM_Vitals.AddNewRow('BloodPressure', CCM_Vitals.params.PanelID + ' #divTemplateBloodPressure', "-1");

                }

                // Remove Existing Pulse Rows
                $("#" + CCM_Vitals.params.PanelID + " #divTemplatePulse").nextAll().remove();
                $.each(vitals_Pulse_detail, function (i, item) {
                    if (parseInt(item.CurrentPulseId) > 0) {
                        CCM_Vitals.AddNewRow('Pulse', CCM_Vitals.params.PanelID + ' #divTemplatePulse', item.CurrentPulseId, item);
                    }
                });
                if (vitals_Pulse_detail.length < 1) {
                    CCM_Vitals.AddNewRow('Pulse', CCM_Vitals.params.PanelID + ' #divTemplatePulse', "-1");

                }
                // Remove Existing Temprature Rows
                $("#" + CCM_Vitals.params.PanelID + " #divTemplateTemprature").nextAll().remove();
                $.each(vitals_Temperature_detail, function (i, item) {
                    if (parseInt(item.CurrentTemperatureId) > 0) {
                        CCM_Vitals.AddNewRow('Temprature', CCM_Vitals.params.PanelID + ' #divTemplateTemprature', item.CurrentTemperatureId, item);
                    }
                });
                if (vitals_Temperature_detail.length < 1) {
                    CCM_Vitals.AddNewRow('Temprature', CCM_Vitals.params.PanelID + ' #divTemplateTemprature', "-1");

                }
                // Remove Existing Respiration Rows
                $("#" + CCM_Vitals.params.PanelID + " #divTemplateRespiration").nextAll().remove();
                $.each(vitals_Respiration_detail, function (i, item) {
                    if (parseInt(item.CurrentRespirationId) > 0) {
                        CCM_Vitals.AddNewRow('Respiration', CCM_Vitals.params.PanelID + ' #divTemplateRespiration', item.CurrentRespirationId, item);
                    }
                });
                if (vitals_Respiration_detail.length < 1) {
                    CCM_Vitals.AddNewRow('Respiration', CCM_Vitals.params.PanelID + ' #divTemplateRespiration', "-1");
                }

                var self = $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals');
                utility.bindMyJSONByName(true, vitals_detail, false, self).done(function () {
                    //Begin 23-12-2015 Muhammad Arshad Bug#EMR-113 Vitals in Progress Notes-> Update button
                    if (vitals_detail.Height == "10") {
                        $('#' + CCM_Vitals.params.PanelID + ' #txtHeight').val(Number(vitals_detail.Height).toFixed(2));
                    }
                    if (vitals_detail.BloodType == "0") {
                        $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals #ddlBloodType').val($('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals #ddlBloodType option:first').val());
                    }
                    if (vitals_detail.SeverityofPain == "0") {
                        $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals #ddlSeverityofPain').val($('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals #ddlSeverityofPain option:first').val());
                    }
                    if (vitals_detail.SmokingStatus == "0") {
                        $('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals #ddlSmokingStatus').val($('#' + CCM_Vitals.params.PanelID + ' #frmClinicalVitals #ddlSmokingStatus option:first').val());
                    }
                   // $('#' + CCM_Vitals.params.PanelID + ' #btnsave').text('Save');
                    //End 23-12-2015 Muhammad Arshad Bug#EMR-113 Vitals in Progress Notes-> Update button
                    //Begin 15-07-2016 Edit By Humaira Yousaf Bug#1582
                    if (vitals_detail.Height == "0" || vitals_detail.Height == "" || vitals_detail.Height == null) {
                        $('#' + CCM_Vitals.params.PanelID + ' #txtHeightFeet').val("");
                        $('#' + CCM_Vitals.params.PanelID + ' #txtHeightInches').val("");
                    }
                    else {
                        var totalInches = parseFloat(vitals_detail.Height);
                        var feet = Math.floor(totalInches / 12);
                        $('#' + CCM_Vitals.params.PanelID + ' #txtHeightFeet').val(feet);
                        var inches = totalInches % 12;
                        $('#' + CCM_Vitals.params.PanelID + ' #txtHeightInches').val(Number(inches).toFixed(2));
                    }
                    if (vitals_detail.Weight == "0") {
                        $('#' + CCM_Vitals.params.PanelID + ' #txtWeight').val("");
                    }
                    if (vitals_detail.BMI == "0") {
                        $('#' + CCM_Vitals.params.PanelID + ' #txtBMI').val("");
                    }
                    if (vitals_detail.BSA == "0") {
                        $('#' + CCM_Vitals.params.PanelID + ' #txtBSA').val("");
                    }
                    if (vitals_detail.HeadCir == "0") {
                        $('#' + CCM_Vitals.params.PanelID + ' #txtHeadCir').val("");
                    }
                    //End 15-07-2016 Edit By Humaira Yousaf Bug#1582
                    CCM_Vitals.params["VitalSignsId"] = VitalSignsId;
                    CCM_Vitals.params["mode"] = "Edit";
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

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
            if (Ctrlvalue.indexOf('.') != -1) {
                var arr = Ctrlvalue.split('.');

                if (arr.length > 0) {
                    var inches = arr[1];
                    var feet = arr[0];
                    var secondinch = inches.substring(0, 2);
                    var heightval = parseFloat(feet * 12) + parseFloat(inches);
                    objData["Height"] = heightval;
                }
            }
            else {
                var heightval = parseFloat(objData["Height"]) * 12;
                objData["Height"] = heightval;
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

        objData["PatientId"] = CCM_Vitals.params.patientID;
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },

    //************* End Update and Fill **************\\

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


    },
    //End 11-30-2015 Muhammad Arshad Bug # EMR-101 Vitals Workflow in Clinical Module -> Add Vitals-> Diastolic
    calculateBMI2: function (objWeight, objHeightInFeet, TargetCtrl, objheightInches) {
        if ($('#txtHeightInches').val() != "" && $('#txtHeightFeet').val() == "") {
            $('#txtHeightFeet').attr("disabled", "disabled");
            $('#txtHeightFeet').removeClass('ui-widget-content');
        }
        else {
            $('#txtHeightFeet').removeAttr("disabled");
        }
        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + CCM_Vitals.params.PanelID + " #frmClinicalVitals #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + CCM_Vitals.params.PanelID + " #frmClinicalVitals #txtHeightFeet").val();
            if (HeightInFeet == '') {
                HeightInFeet = 0;
            }
        }
        var HeightInches = "";
        if (objheightInches != null) {
            HeightInches = $(objheightInches).val();
        }
        else {
            HeightInches = $("#" + CCM_Vitals.params.PanelID + " #frmClinicalVitals #txtHeightInches").val();
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
            CtrlName = $("#" + CCM_Vitals.params.PanelID + " #txtBMI");
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
        if (WeightInLbs != "" && HeightInches != "" && BMI != "Infinity")
            CtrlName.val(BMI);
        else
            CtrlName.val('');
    },
    calculateBMI: function (objWeight, objHeightInFeet, TargetCtrl) {

        var WeightInLbs = "";
        if (objWeight != null) {
            WeightInLbs = $(objWeight).val();
        }
        else {
            WeightInLbs = $("#" + CCM_Vitals.params.PanelID + " #frmClinicalVitals #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + CCM_Vitals.params.PanelID + " #frmClinicalVitals #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + CCM_Vitals.params.PanelID + " #txtBMI");
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

        var heightInInches = CCM_Vitals.convertHeightInches(heightInFeet);

        var result = (weightInLbs / (heightInInches * heightInInches)) * 703;
        var BMI = result.toFixed(2);
        //var weightInKG = CCM_Vitals.convertWeight(weightInLbs);
        ////console.log('wei' + weightInKG);
        //var heightInMetres = CCM_Vitals.convertHeight(heightInFeet);
        ////console.log('hei' + heightInMetres);
        //var result = weightInKG / (heightInMetres * heightInMetres);
        ////console.log(BMI);
        //var BMI = result.toFixed(2);
        if (WeightInLbs != "" && HeightInFeet != "" && BMI != "Infinity")
            CtrlName.val(BMI);
        else
            CtrlName.val('');
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
    ValidateHeightInches: function (event, obj) {

        var Ctrlvalue = "";
        if (obj != null) {
            Ctrlvalue = $(obj).val();
        }

        if ($('#txtHeightFeet').val() != "") {
            if (Ctrlvalue.indexOf('.') != -1) {
                var arr = Ctrlvalue.split('.');
                if (arr.length > 0) {
                    var leftVal = arr[0];
                    var rightVal = arr[1];
                    var secondinch = rightVal.substring(0, 2);
                    if (leftVal <= 11 || rightVal <= 99) {
                        var heightval = leftVal <= 11 ? arr[0] + '.' + secondinch : arr[0] + '.' + secondinch;
                        $(obj).val(heightval);
                    }
                }
            }
            else {
                var heightval = Ctrlvalue <= 11 ? Ctrlvalue : 11;
                $(obj).val(heightval);
            }
        }
        else {
            if (Ctrlvalue.indexOf('.') != -1) {
                var arr = Ctrlvalue.split('.');
                if (arr.length > 0) {
                    var leftVal = arr[0];
                    var rightVal = arr[1];
                    var secondinch = rightVal.substring(0, 2);
                    var heightval = leftVal <= 149 ? arr[0] + '.' + secondinch : arr[0] + '.' + secondinch;
                    $(obj).val(heightval);
                }
            }
            else {
                var heightval = Ctrlvalue <= 149 ? Ctrlvalue : Ctrlvalue.substring(0, Ctrlvalue.length - 1);
                $(obj).val(heightval);
            }
        }
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
            WeightInLbs = $("#" + CCM_Vitals.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + CCM_Vitals.params.PanelID + " #txtHeightFeet").val();
            if (HeightInFeet == '') {
                HeightInFeet = 0;
            }
        }
        var HeightInches = "";
        if (objheightInches != null) {
            HeightInches = $(objheightInches).val();
        }
        else {
            HeightInches = $("#" + CCM_Vitals.params.PanelID + " #txtHeightInches").val();
            if (HeightInches == '' || HeightInches == '.') {
                HeightInches = 0;
            }
        }
        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + CCM_Vitals.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);

        var weightInKG = CCM_Vitals.convertWeight(weightInLbs);

        HeightInches = parseFloat(parseInt(HeightInFeet) * 12 + parseFloat(HeightInches));
        var heightIn_cm = CCM_Vitals.convertInchHeightTo_cm(HeightInches);

        var result = 0.007184 * Math.pow(heightIn_cm, 0.725) * Math.pow(weightInKG, 0.425);

        var BSA = result.toFixed(2)
        if (WeightInLbs != "" && HeightInches != "")
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
            WeightInLbs = $("#" + CCM_Vitals.params.PanelID + " #txtWeight").val();
        }

        var HeightInFeet = "";
        if (objHeightInFeet != null) {
            HeightInFeet = $(objHeightInFeet).val();
        }
        else {
            HeightInFeet = $("#" + CCM_Vitals.params.PanelID + " #txtHeight").val();
        }

        var CtrlName = "";
        if (TargetCtrl != null) {
            CtrlName = $(TargetCtrl);
        }
        else {
            CtrlName = $("#" + CCM_Vitals.params.PanelID + " #txtBSA");
        }

        if (WeightInLbs == "" || WeightInLbs == ".")
            var weightInLbs = 0;
        else
            var weightInLbs = parseFloat(WeightInLbs);
        if (HeightInFeet == "" || HeightInFeet == ".")
            heightInFeet = 0;
        else
            var heightInFeet = parseFloat(HeightInFeet);

        var weightInKG = CCM_Vitals.convertWeight(weightInLbs);
        //console.log('wei' + weightInKG);
        var heightIn_cm = CCM_Vitals.convertHeightTo_cm(heightInFeet);
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
}