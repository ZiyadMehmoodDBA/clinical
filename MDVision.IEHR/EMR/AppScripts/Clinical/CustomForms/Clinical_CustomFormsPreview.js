Clinical_CustomFormsPreview = {

    params: [],
    ProviderCPTs: [],

    Load: function (params) {
        Clinical_CustomFormsPreview.params = params;
        if (Clinical_CustomFormsPreview.params.PanelID != 'pnlClinicalCustomFormsPreview') {
            Clinical_CustomFormsPreview.params.PanelID = Clinical_CustomFormsPreview.params.PanelID + ' #pnlClinicalCustomFormsPreview';
        } else {
            Clinical_CustomFormsPreview.params.PanelID = 'pnlClinicalCustomFormsPreview';
        }
        if (Clinical_CustomFormsPreview.params.CustomFormId) {
            Clinical_CustomFormsPreview.customFormPreview(Clinical_CustomFormsPreview.params.CustomFormId);
        } else if (Clinical_CustomFormsPreview.params.PatientCustomFormId) {
            Clinical_CustomFormsPreview.patientCustomFormPreview(Clinical_CustomFormsPreview.params.PatientCustomFormId);
        }

    },
    domeReady: function () {
        $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormTinyMCEditorPreview input").on('change', function () {
            if ($(this).attr("type") == "radio") {
                if ($(this).attr("name")) {
                    if ($(this).prop("checked")) {
                        $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormTinyMCEditorPreview input:radio[name='" + $(this).attr("name") + "']").removeAttr("checked");
                        $(this).attr("checked", "checked");
                        $(this).prop("checked", "checked");
                    }
                    else {
                        $(this).removeAttr("checked");
                    }
                }
                else {
                    if ($(this).prop("checked")) {
                        $(this).attr("checked", "checked");
                    }
                    else {
                        $(this).removeAttr("checked");
                    }
                }

            }
            if ($(this).attr("type") == "checkbox") {
                if ($(this).prop("checked")) {
                    $(this).attr("checked", "checked");
                }
                else {
                    $(this).removeAttr("checked");
                }
            }
            if ($(this).attr("type") == "text") {
                $(this).attr("value", $(this).val());
            }
        });
        $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormTinyMCEditorPreview textarea").on('change', function () {
            $(this).html($(this).val());
        });

        $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormTinyMCEditorPreview select").on('change', function () {
            let text = $(this).find("option:selected").text();
            $(this).find("option").removeAttr("selected");
            $(this).find("option").filter(function () {
                return $(this).text() == text;
            }).prop("selected", true).attr('selected', true);

        });

        if (Clinical_CustomFormsPreview.params.ParentCtrl == "clinicalTabProgressNote") {
            $.each(($("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormTinyMCEditorPreview input")), function () {
                if ($(this).attr("id")) {
                    if ($(this).attr("type") == "radio" || $(this).attr("type") == "checkbox") {
                        if ($("#ProgressnoteHTML #" + $(this).attr("id")).length > 0) {
                            if ($("#ProgressnoteHTML #" + $(this).attr("id")).prop("checked")) {
                                $(this).attr("checked", "checked");
                                $(this).prop("checked", true);
                            }
                            else {
                                $(this).removeAttr("checked");
                                $(this).prop("checked", false);
                            }
                        }
                    }
                    if ($(this).attr("type") == "text") {
                        if ($("#ProgressnoteHTML #" + $(this).attr("id")).length > 0) {
                            $(this).val($("#ProgressnoteHTML #" + $(this).attr("id")).val());
                            $(this).attr("value", $(this).val());
                        }
                    }
                }
            });
            $.each(($("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormTinyMCEditorPreview textarea")), function () {
                $(this).html($("#ProgressnoteHTML #" + $(this).attr("id")).val());
            });
            $.each(($("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormTinyMCEditorPreview select")), function () {
                if ($("#ProgressnoteHTML #" + $(this).attr("id")).length > 0) {
                    $(this).find("option").removeAttr("selected");
                    let id=$(this).attr("id");
                    $(this).find("option").filter(function (i,item) {
                        return $(item).text() == $("#ProgressnoteHTML #" + id).find("option:selected").text();
                    }).prop("selected", true).attr('selected', true);
                }
            });
        }
    },
    customFormPreview: function (formId, event) {
        Clinical_CustomFormsDetails.customFormFill_DBCall(formId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var actionPan = $('#' + Clinical_CustomFormsPreview.params.PanelID + ' #frmCustomFormPreview');
                $(actionPan).find("#CustomFormHeading").text(response.listCustomForm[0].FormHeading == null || response.listCustomForm[0].FormHeading == "" ? "Select Heading" : response.listCustomForm[0].FormHeading);
                if (response.listCustomForm[0].CustomFormHTML != "") {
                    var html = response.listCustomForm[0].CustomFormHTML;

                    var refinedText = "";
                    html = $.parseHTML(html);
                    $.each(html, function (i, item) {
                        var a = $(item)[0];
                        $(a).find('label').text();
                        var s = $(a).find('label')[0];
                        if ($(item).text().indexOf('_') > -1) {
                            refinedText = "";
                            var startindex = $(item).text().indexOf("_");
                            var lastindex = $(item).text().lastIndexOf("_");
                            var len = $(item).text().length;
                            var initstring = $(item).text().substring(0, startindex);
                            var endstring = $(item).text().substring(lastindex + 1, len);
                            refinedText += initstring + $('#PatientProfile #hfDischargeDate').val() + endstring;

                            //  $(item)[0].textContent = refinedText;
                            $(s).text(refinedText);
                        }
                    });
                    $($("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails")).empty();
                    $(html).find('input[id*=txtt_]').css({ 'border': '1px solid #CCC' });
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").html($(html));
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find('.questionAction').remove();
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #toolQuestionGroupHTML").find('.questionAction').remove();
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").attr('canvasCol', response.listCustomForm[0].CanvasCols);
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormTitle").text(response.listCustomForm[0].FormName);
                }
                $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find("div.ui-resizable-handle.ui-resizable-e").remove();
                var patientId = "";
                if (Clinical_CustomFormsPreview.params.ParentCtrl == "clinicalTabProgressNote")
                    patientId = Clinical_ProgressNote.params.patientID;
                if (Clinical_CustomFormsPreview.params.ParentCtrl == 'Patient_CustomForm')
                    patientId = Select_CustomForm.params.PatientId;
                if (Clinical_CustomFormsPreview.params.ParentCtrl == "clinicalTabProgressNote" || Clinical_CustomFormsPreview.params.ParentCtrl == 'Patient_CustomForm') {
                    var $PatientTagsList = $('#' + Clinical_CustomFormsPreview.params.PanelID + ' #frmCustomFormPreview .TagInserted[value^="{{ Patient"]');
                    if ($PatientTagsList.length > 0) {
                        var dupes = {
                        };
                        var distinctList = [];
                        $.each($PatientTagsList, function (i, el) {
                            if (!dupes[el.id]) {
                                dupes[el.id] = true;
                                distinctList.push(el);
                            }
                        });
                        Clinical_CustomFormsPreview.FillDemographic(patientId).done(function (patientInfo) {
                            if (patientInfo.status != false) {
                                var demographic_detail = JSON.parse(patientInfo.DemographicFill_JSON);
                                Clinical_CustomFormsPreview.ReplacePatientInfo(demographic_detail, distinctList);
                                Clinical_CustomFormsPreview.DoInitilizingofControls();
                            }
                        });
                    }
                    else
                        Clinical_CustomFormsPreview.DoInitilizingofControls();
                }
                else
                    Clinical_CustomFormsPreview.DoInitilizingofControls();
                $('#frmPreview').data('serialize', $('#frmPreview').serialize());

                

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    DoInitilizingofControls: function () {
        //For initilizing toggle for question groups.
        $($("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find($('.toggleButton'))).click(function () {
            var section = $(this).closest('section');
            if (section) {
                if (section.hasClass('active')) {
                    section.removeClass('active');
                    $(section).find('.toggle-content').css('display', 'none');
                }
                else {
                    section.addClass('active');
                    $(section).find('.toggle-content').css('display', 'block');
                }
            }
        });
        $("#" + Clinical_CustomFormsPreview.params.PanelID + " .toggleEditableHeader").find('#lnkQuestionGroupTitle').remove();
        Clinical_CustomFormsPreview.setWidthofTable();
        Clinical_CustomFormsPreview.initilizeDatePickers();
        Clinical_CustomFormsPreview.initilizeTimePickers();
        $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails .toggleCheck").each(function () {
            var $this = $(this);
            var $patent = $($this.closest('li').find('.toolcontroldiv'));
            $this.themePluginIOS7Switch();
            //   setTimeout(function () {
            if ($patent && $patent.attr('defaultselection')) {
                if ($patent.attr('defaultselection') == "0")
                    $($patent.find('.ios-switch')).removeClass('on').removeClass('off').addClass('on');
                else
                    $($patent.find('.ios-switch')).removeClass('on').removeClass('off').addClass('off');
            }
            //  }, 300)
        });
        $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails #customFormIosSwitchPreview").remove();
        if ($("[id^='customFormMultipleSelectCombo_").find('select').length > 0) {
            Clinical_CustomFormsPreview.initilizeMultiSelectCustomFormPreview();
        }
        if (Clinical_CustomFormsPreview.params.IsAddToNote == true) {
            $('#btnCustomFormAddToNote').show();
            Clinical_CustomFormsPreview.fillCustomFormOnNotes();
        }
        else {
            $('#btnCustomFormAddToNote').hide();
            if (Clinical_CustomFormsPreview.params.ParentCtrl == "Patient_CustomForm") {
                $('#actionButtonsPatient').show();
            }
        }
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
            container: 'body'
        });
        Clinical_CustomFormsPreview.domeReady();
    },
    getTinyMCEditorView: function (question) {
        var str = '';
        if (question.find('#customFormTinyMCEditorPreview')[0]) {
            question.find('#customFormTinyMCEditorPreview p').removeClass("TagInserted");
            str += question.find('#customFormTinyMCEditorPreview')[0].outerHTML;
        }
        return str;
    },
    FillDemographic: function (PatientID) {
        var objData = new Object();
        objData["PatientID"] = PatientID;
        objData["CommandType"] = "fill_patient_details_for_custom_forms";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },
    ReplacePatientInfo: function (demographic_detail, distinctList) {
        var htmlTextToBeReplaced = $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails")[0];
        if (htmlTextToBeReplaced) {
            htmlTextToBeReplaced = htmlTextToBeReplaced.outerHTML;
            $.each(distinctList, function (index, element) {
                switch (element.value.toString().trim()) {
                    case '{{ Patient Account Number }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Account Number }}"/g, "'" + demographic_detail.AccountNo + "'");
                        break;
                    case '{{ Patient First Name }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient First Name }}"/g, "'" + demographic_detail.FirstName + "'");
                        break;
                    case '{{ Patient Last Name }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Last Name }}"/g, "'" + demographic_detail.LastName + "'");
                        break;
                    case '{{ Patient Full Name }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Full Name }}"/g, "'" + demographic_detail.FullName + "'");
                        break;
                    case '{{ Patient MI }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient MI }}"/g, "'" + demographic_detail.MiddleInitial + "'");
                        break;
                    case '{{ Patient Prefix }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Prefix }}"/g, "'" + demographic_detail.Prefix + "'");
                        break;
                    case '{{ Patient Suffix }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Suffix }}"/g, "'" + demographic_detail.Suffix + "'");
                        break;
                    case '{{ Patient Gender }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Gender }}"/g, "'" + demographic_detail.Sex + "'");
                        break;
                    case '{{ Patient Date of Birth }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Date of Birth }}"/g, "'" + demographic_detail.DOB + "'");
                        break;
                    case '{{ Patient SSN }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient SSN }}"/g, "'" + demographic_detail.SSN + "'");
                        break;
                    case '{{ Patient MR Number }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient MR Number }}"/g, "'" + demographic_detail.MRN + "'");
                        break;
                    case '{{ Patient Marital Status }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Marital Status }}"/g, "'" + demographic_detail.MaritalStatus + "'");
                        break;
                    case '{{ Patient Email Address }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Email Address }}"/g, "'" + demographic_detail.Email + "'");
                        break;
                    case '{{ Patient Address 1 }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Address 1 }}"/g, "'" + demographic_detail.Address1 + "'");
                        break;
                    case '{{ Patient Address 2 }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Address 2 }}"/g, "'" + demographic_detail.Address2 + "'");
                        break;
                    case '{{ Patient City }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient City }}"/g, "'" + demographic_detail.City + "'");
                        break;
                    case '{{ Patient State }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient State }}"/g, "'" + demographic_detail.State + "'");
                        break;
                    case '{{ Patient ZIP Code }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient ZIP Code }}"/g, "'" + demographic_detail.Zip + "'");
                        break;
                    case '{{ Patient ZIP Code Ext }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient ZIP Code Ext }}"/g, "'" + demographic_detail.ZipExt + "'");
                        break;
                    case '{{ Patient Home Phone }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Home Phone }}"/g, "'" + demographic_detail.HomeTel + "'");
                        break;
                    case '{{ Patient Work Phone }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Work Phone }}"/g, "'" + demographic_detail.WorkTel + "'");
                        break;
                    case '{{ Patient Work Phone Ext }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Work Phone Ext }}"/g, "'" + demographic_detail.PatientWorkPhoneExt + "'");
                        break;
                    case '{{ Patient Cell Number }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Cell Number }}"/g, "'" + demographic_detail.Cell + "'");
                        break;
                    case '{{ Patient Fax Number }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Fax Number }}"/g, "'" + demographic_detail.Fax + "'");
                        break;
                    case '{{ Patient Ethnicity }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Ethnicity }}"/g, "'" + demographic_detail.Ethnicity + "'");
                        break;
                    case '{{ Patient Race }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Race }}"/g, "'" + demographic_detail.Race + "'");
                        break;
                    case '{{ Patient Preferred Language }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Preferred Language }}"/g, "'" + demographic_detail.PrefLanguage + "'");
                        break;
                    case '{{ Patient Emergency Contact Name }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Emergency Contact Name }}"/g, "'" + demographic_detail.PatientEmergencyContactName + "'");
                        break;
                    case '{{ Patient Emergency Contact Address }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Emergency Contact Address }}"/g, "'" + demographic_detail.PatientEmergencyContactAddress + "'");
                        break;
                    case '{{ Patient Emergency Relationship }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Emergency Relationship }}"/g, "'" + demographic_detail.EmergencyContactRelationship + "'");
                        break;
                    case '{{ Patient Emergency Contact Phone }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Emergency Contact Phone }}"/g, "'" + demographic_detail.PatientEmergencyContactPhone + "'");
                        break;
                    case '{{ Patient Emergency Contact Cell }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Emergency Contact Cell }}"/g, "'" + demographic_detail.PatientEmergencyContactCell + "'");
                        break;
                    case '{{ Patient Language }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Language }}"/g, "'" + demographic_detail.PrefLanguage + "'");
                        break;
                    case '{{ Patient Age }}':
                        htmlTextToBeReplaced = htmlTextToBeReplaced.replace(/"{{ Patient Age }}"/g, "'" + Clinical_CustomFormsPreview.formateAge(demographic_detail.Age) + "'");
                        break;

                    default:
                        break;
                }
            });
            $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").html($(htmlTextToBeReplaced));
            $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find('#customFormTinyMCEditorPreview input[readonly][type=text][class=TagInserted]').each(function () {
                $(this).replaceWith($("<span />").text(this.value));
            });
        }
    },
    formateAge: function (Age) {
        var patientAge = "";
        if (Age) {
            patientAge = Age.split(',');

            /////
            if (parseInt((Age.split(',')[0]).split(' ')[1]) > 0) {

                patientAge = Age.split(',')[0]; //age in years
            } else if (parseInt((Age.split(',')[1]).split(' ')[1]) > 0) {
                patientAge = Age.split(',')[1]; //age in months
            } else {
                patientAge = Age.split(',')[2]; //age in days

            }
        }
        return patientAge.trim();
    },
    patientCustomFormPreview: function (patientCustomFormId, event) {
        Patient_CustomForm.customFormFill_DBCall(patientCustomFormId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var actionPan = $('#' + Clinical_CustomFormsPreview.params.PanelID + ' #frmCustomFormPreview');
                $(actionPan).find("#CustomFormHeading").text(response.listCustomForm[0].FormHeading == null || response.listCustomForm[0].FormHeading == "" ? "Select Heading" : response.listCustomForm[0].FormHeading);
                if (response.listCustomForm[0].CustomFormHTML != "") {
                    var html = response.listCustomForm[0].CustomFormHTML;
                    $($("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails")).empty();
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").html($(html));
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find('.questionAction').remove();
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #toolQuestionGroupHTML").find('.questionAction').remove();
                    //For initilizing toggle for question groups.
                    $($("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find($('.toggleButton'))).click(function () {
                        var section = $(this).closest('section');
                        if (section) {
                            if (section.hasClass('active')) {
                                section.removeClass('active');
                                $(section).find('.toggle-content').css('display', 'none');
                            }
                            else {
                                section.addClass('active');
                                $(section).find('.toggle-content').css('display', 'block');
                            }
                        }
                    });
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " .toggleEditableHeader").find('#lnkQuestionGroupTitle').remove()
                    Clinical_CustomFormsPreview.setWidthofTable();
                    Clinical_CustomFormsPreview.initilizeDatePickers();
                    Clinical_CustomFormsPreview.initilizeTimePickers();
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails .toggleCheck").each(function () {
                        var $this = $(this);
                        var $patent = $($this.closest('li').find('.toolcontroldiv'));
                        $this.themePluginIOS7Switch();
                        //   setTimeout(function () {
                        if ($patent && $patent.attr('defaultselection')) {
                            if ($patent.attr('defaultselection') == "0")
                                $($patent.find('.ios-switch')).removeClass('on').removeClass('off').addClass('on');
                            else
                                $($patent.find('.ios-switch')).removeClass('on').removeClass('off').addClass('off');
                        }
                        //  }, 300)
                    });
                    $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails #customFormIosSwitchPreview").remove();
                }
                // });
                if ($("[id^='customFormMultipleSelectCombo_").find('select').length > 0) {
                    Clinical_CustomFormsPreview.initilizeMultiSelectCustomFormPreview();
                }
                if (Clinical_CustomFormsPreview.params.IsAddToNote == true) {
                    $('#btnCustomFormAddToNote').show();
                    Clinical_CustomFormsPreview.fillCustomFormOnNotes();
                }
                else {
                    $('#btnCustomFormAddToNote').hide();
                    $('#actionButtonsPatient').show();
                }
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({
                    container: 'body'
                });
                $('.btnWidgetSwitch').each(function () {
                    if ($(this).find('.ios-switch').length > 1) {
                        $(this).find('.ios-switch:first').remove();
                    }
                });
                $('#pnlClinicalCustomFormsPreview .btnWidgetSwitch').each(function () {
                    if (!($(this).find('input').attr('checked'))) {
                        $(this).find('.ios-switch').removeClass('on').addClass('off');
                    }
                })
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    setWidthofTable: function () {
        $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find("#tblContext").each(function () {
            var table = $(this);
            if (table) {
                cols = $(table.find('tr:first-child')).find('td').length;
                if (cols) {
                    var totalCols = (parseInt(cols) + 1);
                    var widthofTd = Math.floor(100 / totalCols);
                    $(".tblContextTd").css({
                        'width': widthofTd + '%'
                    });
                }
            }
        });
    },
    initilizeDatePickers: function () {
        $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find(".dateField").each(function () {
            var dateFormat = $(this).attr('dateformat');
            var date_format = 'dd/mm/yyyy';
            if (dateFormat)
                date_format = dateFormat.toLowerCase();
            $(this).datepicker({
                format: date_format,
                todayBtn: 'linked',
            }).on('changeDate', function (e) {
                $(this).datepicker('hide');
            });
            if (this.value != '' && this.value != null && typeof this.value != "undefined") {
                this.value = utility.RemoveTimeFromDate(null, this.value);
                $(this).datepicker("setDate", this.value);
            }
        });
    },
    initilizeTimePickers: function () {
        $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormDetails").find(".timeField").each(function () {
            var timeFormat = $(this).attr('timeformat');
            var showMerdn = false;
            if (timeFormat == "24")
                showMerdn = false;
            else if (timeFormat == "12")
                showMerdn = true;
            $(this).timepicker({
                showMeridian: showMerdn,
                //appendWidgetTo: $(this).closest('.controlContainerDiv'),
            }).on('show.timepicker', function (e) {
                $(".bootstrap-timepicker-widget").css("top", "100%");
            });
            if (this.value != '' && this.value != null && typeof this.value != "undefined") {
                $(this).timepicker("setTime", this.value);
            }
        });
    },
    initilizeMultiSelectCustomFormPreview: function () {
        $("[id^='customFormMultipleSelectCombo_").find('select').each(function (i, e) {
            $(e).multiselect('destroy');
            $(e).attr('multiple', 'multiple')
            $(e).multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                onDropdownShow: function (event) {
                    $(e).parent().find('.multiselect-container > li > a ').css('white-space', 'normal');
                },
            });
            var defaultValues = $(e).parent().parent().parent().attr('defaultselection');
            $(e).val('');

            $(e).multiselect('select', defaultValues.split(','));
            $(e).multiselect("refresh");
        });
        $("[id^='customFormMultipleSelectCombo_").find('select').next().next().remove()
    },

    UnLoadTab: function () {

        if (Clinical_CustomFormsPreview.params.ParentCtrl == "clinicalTabProgressNote") {

            if (Clinical_CustomFormsPreview.params["IsPreview"] == true) {
                UnloadActionPan(null, 'Clinical_CustomFormsPreview');
                return;
            }
            if (EMRUtility.compareFormDataWithSerialized(Clinical_CustomFormsPreview.params.PanelID + ' #frmPreview')) {
                utility.myConfirmNote('CustomFormsPreview', function () {
                    $('#pnlClinicalCustomFormsPreview #btnCustomFormAddToNote').trigger('click');

                }, null, function () {
                    if (Clinical_CustomFormsPreview.params != null && Clinical_CustomFormsPreview.params.ParentCtrl != null) {
                        UnloadActionPan(Clinical_CustomFormsPreview.params.ParentCtrl, 'Clinical_CustomFormsPreview', null, "pnlClinicalProgressNote #pnlClinicalCustomForms");
                    }
                    else
                        UnloadActionPan(null, 'Clinical_CustomFormsPreview');
                },
               '1'
               );
            }
            else {
                if (Clinical_CustomFormsPreview.params != null && Clinical_CustomFormsPreview.params.ParentCtrl != null) {


                    utility.myConfirmNote('CustomFormsPreview', function () {
                        $('#pnlClinicalCustomFormsPreview #btnCustomFormAddToNote').trigger('click');

                    }, null, function () {
                        if (Clinical_CustomFormsPreview.params != null && Clinical_CustomFormsPreview.params.ParentCtrl != null) {
                            var $problemTools = $('#' + Clinical_CustomFormsPreview.params.PanelID + ' #frmPreview').find("div[id^='toolProblems_']");
                            $problemTools.each(function () {
                                var $ProblemsListLi = $(this).find('#customFormProblemsList li');
                                if ($ProblemsListLi.length > 0) {
                                    $ProblemsListLi.each(function () {
                                        if ($(this).attr('isnew')) {
                                            $(this).addClass('hidden');
                                        }
                                    })
                                }
                            });

                            var $procedureTools = $('#' + Clinical_CustomFormsPreview.params.PanelID + ' #frmPreview').find("div[id^='toolProcedures_']");
                            $procedureTools.each(function () {
                                var $ProceduresListLi = $(this).find('#customFormProceduresList li');
                                if ($ProceduresListLi.length > 0) {
                                    $ProceduresListLi.each(function () {
                                        if ($(this).attr('isnew')) {
                                            $(this).addClass('hidden');
                                        }
                                    })
                                }
                            });

                            Clinical_CustomFormsPreview.removeDeletedCPTAndICD();
                            UnloadActionPan(Clinical_CustomFormsPreview.params.ParentCtrl, 'Clinical_CustomFormsPreview', null, "pnlClinicalProgressNote #pnlClinicalCustomForms");
                        }
                        else
                            UnloadActionPan(null, 'Clinical_CustomFormsPreview');
                    },
              '1'
              );

                    //  UnloadActionPan(Clinical_CustomFormsPreview.params.ParentCtrl, 'Clinical_CustomFormsPreview', null, "pnlClinicalProgressNote #pnlClinicalCustomForms");
                }
                else
                    UnloadActionPan(null, 'Clinical_CustomFormsPreview');
            }

        }
        else {
            UnloadActionPan(Clinical_CustomFormsPreview.params.ParentCtrl, 'Clinical_CustomFormsPreview');
        }
    },

    addCustomFormToNote: function (event) {
        if (event != null && event != 'fromUnLoad') {
            event.preventDefault();
        }
        Clinical_CustomFormsPreview.removeDeletedCPTAndICD();
        var isValidProc = Clinical_CustomFormsPreview.validateProcedures();
        var isValidProbs = Clinical_CustomFormsPreview.validateProblems();
        var isValidMS = Clinical_CustomFormsPreview.validateMultiSelectValues();
        var isValidYesNo = Clinical_CustomFormsPreview.validateYesNo();
        var isValidTable = Clinical_CustomFormsPreview.validateTable();
        var isValidSS = Clinical_CustomFormsPreview.validateSingleSelect();
        if (isValidMS && isValidYesNo && isValidTable && isValidSS && isValidProc && isValidProbs) {
            Clinical_CustomFormsPreview.addCustomFormToDocs(Clinical_CustomFormsPreview.params.CustomFormName).done(function (customFormNameForDoc) {
                Clinical_CustomFormsPreview.getProviderCPTs_DBCall().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var ProviderCPTs_JSON = JSON.parse(response.ProviderCPTsList_JSON);
                        Clinical_CustomFormsPreview.ProviderCPTs = ProviderCPTs_JSON;
                        Clinical_CustomFormsPreview.UnloadTabAfterAddToNote(event, customFormNameForDoc);
                    }
                    else {
                        Clinical_CustomFormsPreview.UnloadTabAfterAddToNote(event, customFormNameForDoc);
                    }
                });
            });
        }
    },
    UnloadTabAfterAddToNote: function (event, customFormNameForDoc) {
        Clinical_CustomFormsPreview.getCustomFormInfo(Clinical_CustomFormsPreview.params.CustomFormId, true, customFormNameForDoc);
        if (event == 'fromUnLoad') {
            UnloadActionPan(Clinical_CustomFormsPreview.params.ParentCtrl, 'Clinical_CustomFormsPreview', null, "pnlClinicalProgressNote #pnlClinicalCustomForms");
        } else {
            if (Clinical_CustomForms.params["ParentCtrl"] == "clinicalTabProgressNote") {
                if (Clinical_CustomFormsPreview.params != null && Clinical_CustomFormsPreview.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_CustomFormsPreview.params.ParentCtrl, 'Clinical_CustomFormsPreview', null, "pnlClinicalProgressNote #pnlClinicalCustomForms");
                }
                else {
                    UnloadActionPan(null, 'Clinical_CustomFormsPreview');
                }
            } else {
                UnloadActionPan(Clinical_CustomFormsPreview.params.ParentCtrl, 'Clinical_CustomFormsPreview');
            }
        }
    },
    addCustomFormToDocs: function (CustomFormName) {

        var def = $.Deferred();

        var patientId = $('#PatientProfile #hfPatientId').val();

        Patient_CustomForm.getHeaderFooterInfo(patientId).done(function (response) {
            response = JSON.parse(response);

            if (response.status == true) {
                var html = $('#actionPanClinicalProgressNote #containerCustomFormPreview').html();
                var HeaderLogo = response.HeaderLogo;
                var FooterText = response.FooterText;

                if (HeaderLogo != null && HeaderLogo !== "" && FooterText != null && FooterText !== "") {
                    var patientData = "";
                    if (response.PatientText != 'undefined' && response.PatientText != null)
                        patientData = a = response.PatientText.split('<br/>');
                    var newPatientText = '';
                    for (var i = 0; i < patientData.length; i++) {
                        if ($.trim(patientData[i]) != '') {
                            newPatientText += '<li>' + patientData[i] + '</li>';
                        }
                    }
                    var providerData = "";
                    if (response.ProviderText != 'undefined' && response.ProviderText != null)
                        providerData = a = response.ProviderText.split('<br/>');
                    var newProviderText = '';
                    for (var i = 0; i < providerData.length; i++) {
                        if ($.trim(providerData[i]) != '') {
                            newProviderText += '<li align="right">' + providerData[i] + '</li>';
                        }
                    }


                    var formHeaderHtml = '<div id="printcall">' +
                     '<div id="PatientInfo" class="col-xs-12 p-none">' +
                     '<div class="col-sm-4 col-lg-2 pull-left">' +
                      '<img src="' + HeaderLogo + '" class="img-responsive" height="100px" width="240px"></div>' +
                          '<ul class="list-unstyled pull-right line-height-fix">' +
                          '<li id="PatientPractice" class="text-right">' + response.PracticeText + '</li></ul>' +
                           '<div class="clearfix"></div>' +
                             '<div class="splitter m-none mt-xs">' +
                             '<div class="spacer3"></div></div>' +
                               '<div class="spacer3"></div>' +
                          '<ul class="list-unstyled pull-left line-height-fix" >' +
                              newPatientText + '</ul>' +
                           '<ul class="list-unstyled pull-right line-height-fix">' +
                          //'<li id="PatientProvider" align="right">' +
                          newProviderText + '</ul>' +
                      '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                               '<div class="spacer3"></div>' +
                      '<section class="">' +
                      '<div class=""  id="grdICDCPTPrint" style="font-size:12px;">' + html + '</div></section></div>' +
                      '<div class="spacer10"></div><h4 id="templateHeader"></h4>';

                    var docType = '<!doctype html>';
                    var docCnt = formHeaderHtml;

                    var docHead = '<head><script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                 + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                 + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
                 + '</script>'
                 + '</head>';

                    var footer = ' <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">'
                  + '<div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>'
                + '<div id="ClinicalReportsFooterText" class="blueBgPrint" style="float:left; width:100%;padding:2px 5px 0 5px;height:22px;">'
                + '<span id="ClinicalReportsFooter">' + FooterText + '</span>'
                 + '</div> </div>';

                    var html = docCnt;
                    var ProgressNoteSign = $("<div id='customformsign' ></div>").append(html);
                    $("#Customeformprintparentdiv").append(ProgressNoteSign);
                    $("#customformsign #txtTextField").each(function (i, e) {
                        var scrollHeight = e.scrollHeight;
                        if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                            $(e).height(scrollHeight + 50);
                        } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                            $(e).height(scrollHeight + 40);
                        } else {
                            $(e).height(scrollHeight + 20);
                        }
                        $(e).parents('.toolcontroldiv').addClass('heightReset');
                    });
                    $("#customformsign #txtFreeText").each(function (i, e) {
                        var scrollHeight = e.scrollHeight;
                        if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                            $(e).height(scrollHeight + 130);
                        } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                            $(e).height(scrollHeight + 100);
                        } else {
                            $(e).height(scrollHeight + 50);
                        }
                        $(e).parents('.toolcontroldiv').addClass('heightReset');
                    });
                    $('#customformsign span.required').remove();
                    //var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=1000, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                    //var newWin = window.open("", "_blank", winAttr);
                    //writeDoc = newWin.document;
                    //writeDoc.open();
                    // writeDoc.write(docType + '<html>' + docHead + '<body style="font-size:30px;">' + docCnt + footer + '</body></html>');
                    ////writeDoc.close();

                    //////   Our changes End
                    //setTimeout(function () {
                    //    newWin.focus();
                    //    newWin.print();
                    //    newWin.close();
                    //}, 200);

                    kendo.drawing.drawDOM("#Customeformprintparentdiv #customformsign", {
                        landscape: false,
                        scale: 0.6,
                        paperSize: "A4",
                        // margin: "2cm 3cm ",
                        margin: {
                            left: "10mm",
                            //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                            top: "3mm",
                            //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                            right: "10mm",
                            bottom: "15mm"
                        },
                        template: $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html()
                    }).then(function (group) {
                        kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                            Clinical_NotesView.pdf = dataURL;
                            var params = [];
                            params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                            params["PreviewPdf"] = true;
                            var data = new FormData();
                            data.append('notes', params["PrintPDFDataURL"]);
                            data.append("PatientID", $('#PatientProfile #hfPatientId').val());

                            data.append("FileName", "Signed Note");
                            data.append("fileType", "application/pdf");
                            data.append("FolderName", "Custom Form");
                            Clinical_CustomFormsPreview.params.CustomFormNameForDoc ? data.append("CustomFormNameForDoc", Clinical_CustomFormsPreview.params.CustomFormNameForDoc) : data.append("CustomFormNameForDoc", "");
                            $("#Customeformprintparentdiv").html('');
                            var myObject = new Object();
                            myObject.ddlFolder = "12";
                            myObject.CustomFormName = CustomFormName;
                            myObject.Folder = "Custom Form";
                            myObject.DOS = Clinical_ProgressNote.params.VisitDateForFollowUp;

                            var myJSON = JSON.stringify(myObject);
                            data.append("PatientDocumentData", myJSON);
                            if (Clinical_CustomFormsPreview.params.CustomFormNameForDoc) {
                                Clinical_CustomFormsPreview.updateDocument(data).done(function (response) {
                                    if (response.status != false) {
                                        //return response.CustomFormNameForDoc;
                                        def.resolve(response.CustomFormNameForDoc);
                                    }
                                });
                            } else {
                                Document_Import.SaveImport(data).done(function (response) {
                                    if (response.status != false) {
                                        //return response.CustomFormNameForDoc;
                                        def.resolve(response.CustomFormNameForDoc);
                                    }
                                });
                            }

                        });

                    });
                    return def.promise();
                }
                else {
                    Patient_Demographic.FillDemographic(patientId).done(function (patientInfo) {

                        var PatientProfileInfo = JSON.parse(patientInfo.DemographicFill_JSON);
                        var demographic_detail = JSON.parse(patientInfo.DemographicFill_JSON);

                        var patientAge = "";

                        if (PatientProfileInfo.Age) {
                            patientAge = PatientProfileInfo.Age.split(',');
                            if (parseInt((PatientProfileInfo.Age.split(',')[0]).split(' ')[1]) > 0) {

                                patientAge = PatientProfileInfo.Age.split(',')[0]; //age in years
                            } else if (parseInt((PatientProfileInfo.Age.split(',')[1]).split(' ')[1]) > 0) {
                                patientAge = PatientProfileInfo.Age.split(',')[1]; //age in months
                            } else {
                                patientAge = PatientProfileInfo.Age.split(',')[2]; //age in days

                            }
                        }

                        //var form = "#pnlClinicalSuperBillTemplate #frmSuperBillTemplate";
                        //var templateName = $(form + ' #ddlSuperBillTemplate option:selected').text();
                        //var DOB = 'DOB: ' + PatientProfileInfo.DOB;
                        var patientInfo = patientAge + " " + PatientProfileInfo.Sex;
                        var patientAddress = PatientProfileInfo.Address1 + ', ' + PatientProfileInfo.City + ', ' + PatientProfileInfo.State + ', ' + PatientProfileInfo.Zip;
                        var PatientEmail = '';
                        var PatientHomePhone = '';
                        var PatientCellPhone = '';

                        if (PatientProfileInfo.Email != null && PatientProfileInfo.Email != "") {
                            PatientEmail = '<li id="PatientEmail">Email: ' + PatientProfileInfo.Email + '</li>';
                        }
                        if (PatientProfileInfo.HomeTel != null && PatientProfileInfo.HomeTel != "") {
                            PatientHomePhone = '<li id="PatientHomePhone">Home Phone: ' + PatientProfileInfo.HomeTel + '</li>';
                        }
                        if (PatientProfileInfo.Cell != null && PatientProfileInfo.Cell != "") {
                            PatientCellPhone = '<li id="PatientCellPhone">Cell Phone: ' + PatientProfileInfo.Cell + '</li>';
                        }

                        if (globalAppdata['DateFormat'])
                            date_format = globalAppdata['DateFormat'];
                        var date = new Date();
                        //date_format = date_format.replace("mm", date.getMonth() + 1);
                        //date_format = date_format.replace("yyyy", date.getFullYear());
                        var day = "";
                        if (date.getDate().length < 2) {
                            day = "0" + date.getDate();
                        }
                        else {
                            day = date.getDate();
                        }
                        //date_format = date_format.replace("dd", day);

                        //var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);
                        //      var providerData = JSON.parse(response.NoteHeaderProviderData);
                        //var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "");
                        //date_format = utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate);
                        //PatientProfileInfo.Provider = providerName;
                        setTimeout(function () {
                            var html = $('#actionPanClinicalProgressNote #containerCustomFormPreview').html();
                            var formHeaderHtml = '<div id="printcall">' +
                                    '<div id="PatientInfo">' +
                                    '<div class="col-sm-4 col-lg-2 pull-left">' +
                                    '<img src="content/images/SHS-nav-logo-small-100.png" class="img-responsive"></div>' +
                                         '<ul class="list-unstyled pull-right line-height-fix">' +
                                         '<li id="DOS" class="text-right">DOS: ' + date + '</li>' +
                                         '<li id="PatientProvider" class="text-right"">Provider: ' + PatientProfileInfo.Provider + '</li></ul>' +
                                          '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                           '<div class="spacer3"></div>' +
                                         '<ul class="list-unstyled pull-left line-height-fix">' +
                                         '<li id="PatientName" >' + PatientProfileInfo.FullName + '</li>' +
                                         '<li id="PatientAccount" ></li>' + PatientProfileInfo.AccountNo + '</li>' +
                                          '<li id="PatientAge" >' + patientInfo + '</li>' +
                                         '<li id="PatientAddress">' + patientAddress + '</li>' +
                                         PatientHomePhone +
                                         PatientCellPhone +
                                         PatientEmail +
                                         '</ul>' +

                                     '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                               '<div class="spacer3"></div>' +
                                     '<div class=""  id="grdICDCPTPrint" style="font-size:12px;">' + html + '</div></section></div>' +
                                     '<div class="spacer10"></div><h4 id="templateHeader"></h4>';

                            var docType = '<!doctype html>';
                            var docCnt = formHeaderHtml;

                            //   var docHead = '<head><script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                            //+ '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                            //+ '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
                            //+ '</script>'
                            //+ '</head>';


                            var footer = ' <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">'
                          + '<div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>'
                        + '<div id="ClinicalReportsFooterText" class="blueBgPrint" style="float:left; width:100%;padding:2px 5px 0 5px;height:22px;">'
                         + 'Generated by:'
                        + '<span id="ClinicalReportsFooter"> MDVISION PM EMR</span><span class="whiteColorPrint" style="float:right;"> Page 1 of 1</span>'
                         + '</div> </div>';
                            var html = docCnt; //+ '</body></html>';
                            var ProgressNoteSign = $("<div id='customformsign' ></div>").append(html);
                            $("#Customeformprintparentdiv").append(ProgressNoteSign);
                            $("#customformsign #txtTextField").each(function (i, e) {
                                var scrollHeight = e.scrollHeight;
                                if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                                    $(e).height(scrollHeight + 50);
                                } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                                    $(e).height(scrollHeight + 40);
                                } else {
                                    $(e).height(scrollHeight + 20);
                                }
                                $(e).parents('.toolcontroldiv').addClass('heightReset');
                            });
                            $("#customformsign #txtFreeText").each(function (i, e) {
                                var scrollHeight = e.scrollHeight;
                                if ($(e).parents('li').hasClass('col-xs-12 col-sm-4')) {
                                    $(e).height(scrollHeight + 130);
                                } else if ($(e).parents('li').hasClass('col-xs-12 col-sm-6')) {
                                    $(e).height(scrollHeight + 100);
                                } else {
                                    $(e).height(scrollHeight + 50);
                                }
                                $(e).parents('.toolcontroldiv').addClass('heightReset');
                            });
                            $('#customformsign span.required').remove();
                            // $('#sendHtml').append(html);
                            kendo.drawing.drawDOM("#Customeformprintparentdiv #customformsign", {
                                landscape: false,
                                scale: 0.6,
                                paperSize: "A4",
                                // margin: "2cm 3cm ",
                                margin: {
                                    left: "10mm",
                                    //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                    top: "3mm",
                                    //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                                    right: "10mm",
                                    bottom: "15mm"
                                },
                                template: $("#pnlClinicalCustomFormsPreview #page-templateLegacy").html()
                            }).then(function (group) {
                                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                                    Clinical_NotesView.pdf = dataURL;
                                    var params = [];
                                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                                    params["PreviewPdf"] = true;
                                    var data = new FormData();
                                    data.append('notes', params["PrintPDFDataURL"]);
                                    data.append("PatientID", $('#PatientProfile #hfPatientId').val());

                                    data.append("FileName", "Signed Note");
                                    data.append("fileType", "application/pdf");
                                    data.append("FolderName", "Custom Form");
                                    Clinical_CustomFormsPreview.params.CustomFormNameForDoc ? data.append("CustomFormNameForDoc", Clinical_CustomFormsPreview.params.CustomFormNameForDoc) : data.append("CustomFormNameForDoc", "");
                                    $("#Customeformprintparentdiv").html('');
                                    var myObject = new Object();
                                    myObject.ddlFolder = "12";
                                    myObject.CustomFormName = CustomFormName;
                                    myObject.Folder = "Custom Form";
                                    myObject.DOS = Clinical_ProgressNote.params.VisitDateForFollowUp;

                                    var myJSON = JSON.stringify(myObject);
                                    data.append("PatientDocumentData", myJSON);
                                    if (Clinical_CustomFormsPreview.params.CustomFormNameForDoc) {
                                        Clinical_CustomFormsPreview.updateDocument(data).done(function (response) {
                                            if (response.status != false) {
                                                //return response.CustomFormNameForDoc;
                                                def.resolve(response.CustomFormNameForDoc);
                                            }
                                        });
                                    } else {
                                        Document_Import.SaveImport(data).done(function (response) {
                                            if (response.status != false) {
                                                //return response.CustomFormNameForDoc;
                                                def.resolve(response.CustomFormNameForDoc);
                                            }
                                        });
                                    }

                                });

                            });
                            return def.promise();
                            //var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=1000, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                            //var newWin = window.open("", "_blank", winAttr);
                            //writeDoc = newWin.document;
                            //writeDoc.open();
                            //writeDoc.write(docType + '<html>' + docHead + '<body style="font-size:30px;">' + docCnt + footer + '</body></html>');
                            ////writeDoc.close();

                            //////   Our changes End
                            //setTimeout(function () {
                            //    newWin.focus();
                            //    newWin.print();
                            //    newWin.close();
                            //}, 200);
                        }, 100);


                    });
                    return def.promise();
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return def.promise();
    },
    updateDocument: function (data, PatientDocumentData) {
        //var data = "data=" + data + "&PatientDocumentData=" + PatientDocumentData;// + "&PatientID=" + PatientID + "&DocumentID=" + DocumentID;
        // serach parameter , class name, command name of class
        return MDVisionService.fileService(data, "PATIENT_DOCUMENT", "UPDATE_PATIENT_DOCUMENT_FROM_NOTES");
    },

    validateProcedures: function () {
        var isValid = false;
        var self = $("#" + Clinical_CustomFormsPreview.params.PanelID);
        var customFormProcedures = self.find("div[questiontype='Procedures']");
        if (customFormProcedures.length > 0) {
            customFormProcedures.each(function () {
                var isRequired = $(this).attr('ismandatory');
                if (isRequired == "true") {

                    if ($(this).find('#customFormProceduresList li').length > 0) {
                        isValid = true;
                        $(this).find('input').css('border-color', '');
                    }
                    else {
                        $(this).find('input').css('border-color', 'rgb(204, 39, 36)');
                        utility.DisplayMessages("Please enter Procedure(s).", 2)
                        isValid = false;
                        return false;
                    }
                }
                else {
                    isValid = true;
                }
            });
        }
        else {
            isValid = true;
        }
        return isValid;
    },
    validateProblems: function () {
        var isValid = false;
        var self = $("#" + Clinical_CustomFormsPreview.params.PanelID);
        var customFormProblems = self.find("div[questiontype='Problems']");
        if (customFormProblems.length > 0) {
            customFormProblems.each(function () {
                var isRequired = $(this).attr('ismandatory');
                if (isRequired == "true") {

                    if ($(this).find('#customFormProblemsList li').length > 0) {
                        isValid = true;
                        $(this).find('input').css('border-color', '');
                    }
                    else {
                        $(this).find('input').css('border-color', 'rgb(204, 39, 36)');
                        utility.DisplayMessages("Please enter Problem(s).", 2)
                        isValid = false;
                        return false;
                    }
                }
                else {
                    isValid = true;
                }
            });
        }
        else {
            isValid = true;
        }
        return isValid;
    },
    validateMultiSelectValues: function () {
        var isValid = false;
        var self = $("#" + Clinical_CustomFormsPreview.params.PanelID);
        var customFormMultipleSelectCombo = self.find("div[questiontype='MultipleSelectCombo']");
        if (customFormMultipleSelectCombo.length > 0) {
            customFormMultipleSelectCombo.each(function () {
                var isRequired = $(this).attr('ismandatory');
                if (isRequired == "true") {
                    var selectOptions = $(this).find('#toolSingleSelectDropdown_ option:Selected').map(function () {
                        return this.value;
                    }).get().join(',');
                    if (selectOptions != "") {
                        isValid = true;
                        $($(this).find('button')[0]).css('border-color', '');
                    }
                    else {
                        $($(this).find('button')[0]).css('border-color', 'rgb(204, 39, 36)');
                        utility.DisplayMessages("Please select any value.", 2)
                        isValid = false;
                        return false;
                    }
                }
                else {
                    isValid = true;
                }
            });
        }
        else {
            isValid = true;
        }
        return isValid;
    },
    validateYesNo: function () {
        var isValid = false;
        var self = $("#" + Clinical_CustomFormsPreview.params.PanelID);
        var customFormYesNo = self.find("div[questiontype='YesNo']");
        if (customFormYesNo.length > 0) {
            customFormYesNo.each(function (i, e) {
                var isRequired = $(this).attr('ismandatory');
                if (isRequired == "true") {
                    var IsChecked = $(this).find("input[type='checkbox']").is(":checked");
                    if (IsChecked == true) {
                        isValid = true;
                        $(self).find("#customFormYesNoPreview").removeAttr("style");
                    }
                    else {
                        isValid = false;
                        $(e).css({
                            border: '1px solid #cc2724', display: 'inline-block', width: '100%'
                        });
                        utility.DisplayMessages("Please select any option.", 2)
                        isValid = false;
                        return false;
                    }
                }
                else {
                    isValid = true;
                }
            });
        }
        else {
            isValid = true;
        }
        return isValid;
    },
    validateTable: function () {
        var isValid = false;
        var self = $("#" + Clinical_CustomFormsPreview.params.PanelID);
        var customFormTable = self.find("div[questiontype='Table']");
        if (customFormTable.length > 0) {
            customFormTable.each(function () {
                var isRequired = $(this).attr('ismandatory');
                if (isRequired == "true") {
                    $(this).find("#tblContext").find('br').remove();
                    var emptyCells = $(this).find("#tblContext td:empty").length;
                    var totalCells = $(this).find("#tblContext td").length;
                    if (totalCells > emptyCells) {
                        isValid = true;
                        $(this).find("#tblContext").css('border-color', '');
                    }
                    else {
                        $(this).find("#tblContext").css('border-color', 'rgb(204, 39, 36)');
                        utility.DisplayMessages("Please fill the table.", 2)
                        isValid = false;
                        return false;
                    }
                }
                else {
                    isValid = true;
                }
            });
        }
        else {
            isValid = true;
        }
        return isValid;
    },
    validateSingleSelect: function () {
        var isValid = false;
        var self = $("#" + Clinical_CustomFormsPreview.params.PanelID);
        var customFormSingleSelectCombo = self.find("div[questiontype='SingleSelectDropdown']");
        if (customFormSingleSelectCombo.length > 0) {
            customFormSingleSelectCombo.each(function () {
                var isRequired = $(this).attr('ismandatory');
                if (isRequired == "true") {
                    var selectOptions = $(this).find('select option:Selected').map(function () {
                        return this.value;
                    }).get().join(',');
                    if (selectOptions != "0" && selectOptions != "") {
                        isValid = true;
                        $(this).find('select').css('border-color', '');
                    }
                    else {
                        $(this).find('select').css('border-color', 'rgb(204, 39, 36)');
                        utility.DisplayMessages("Please select any value.", 2);
                        isValid = false;
                        return false;
                    }
                }
                else {
                    isValid = true;
                }
            });
        }
        else {
            isValid = true;
        }
        return isValid;
    },
    getCustomFormInfo: function (customFormId, hideAlertMessage, customFormNameForDoc) {
        if (customFormId == null || customFormId == '') {
            return false;
        }
        var customFormSentenceView = Clinical_CustomFormsPreview.getCustomFormSentenceView($('#actionPanClinicalProgressNote #customFormDetails'));
        var refinedText = "";
        customFormSentenceView = $.parseHTML(customFormSentenceView);
        if (customFormSentenceView) {
            //$.each(customFormSentenceView, function (i, item) {

            //    if ($(item).text().indexOf('_') > -1) {
            //        refinedText = "";
            //        var startindex = $(item).text().indexOf("_");
            //        var lastindex = $(item).text().lastIndexOf("_");
            //        var len = $(item).text().length;
            //        var initstring = $(item).text().substring(0, startindex);
            //        var endstring = $(item).text().substring(lastindex + 1, len);
            //        refinedText += initstring + $('#PatientProfile #hfDischargeDate').val() + endstring;
            //        $(item).text(refinedText);
            //    }
            //});

            var CPListId = [];
            $("#" + Clinical_CustomFormsPreview.params.PanelID + " #customFormProblemsList > li").each(function (ind, item) {
                if ($(item).attr('id')) {
                    CPListId.push($(item).attr('id'));
                }
            });
            var ProblemListsId = "";
            if (CPListId.join(",") != "") {
                ProblemListsId = CPListId.join(",");
                Clinical_CustomFormsPreview.attachProbleWithNote(ProblemListsId);
            }

            Clinical_CustomFormsPreview.createCustomFormBodyHTML(customFormSentenceView, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', customFormId, hideAlertMessage, customFormNameForDoc);
        }

    },

    createCustomFormBodyHTML: function (response, NoteHTMLCtrl, customFormId, hideAlertMessage, customFormNameForDoc) {
        // Start PRD-120
        $.each(response, function (i, k) {
            if ($(this).hasClass("ellipses")) {
                $(this).removeClass("ellipses")
            }
        });
        // END PRD-120
        var customFormUniqueId = Clinical_CustomForms.getCustomFormForNotes(Clinical_CustomFormsPreview.params.CustomFormName, Clinical_CustomFormsPreview.params.CustomFormId, Clinical_CustomFormsPreview.params.CustomFormUniqueId, customFormNameForDoc);
        if (!customFormUniqueId) {
            customFormUniqueId = Clinical_CustomFormsPreview.params.CustomFormUniqueId;
        }
        var $mainDivCustomForm = $(document.createElement('div'));
        var $SectionBodyCustomForm = $(document.createElement('section'));
        $SectionBodyCustomForm.attr('id', "Cli_CustomForm_Main" + customFormUniqueId);
        var $DetailsDiv = $(document.createElement('div'));
        $DetailsDiv.attr('id', "Cli_CustomForm_" + customFormUniqueId);
        var $ListCustomForm = $(document.createElement('ul'));
        $ListCustomForm.attr('class', 'list-unstyled')

        $ListCustomForm.append((response == "" ? "" : response));
        $DetailsDiv.append($ListCustomForm);
        $SectionBodyCustomForm.append($DetailsDiv);
        $mainDivCustomForm.append($SectionBodyCustomForm);

        if (!customFormUniqueId) {
            customFormUniqueId = Clinical_CustomFormsPreview.params.CustomFormUniqueId;
        }
        if ($mainDivCustomForm.html() != '') {
            Clinical_CustomFormsPreview.updateCustomFormHtml($mainDivCustomForm.html(), customFormId, NoteHTMLCtrl, hideAlertMessage, customFormUniqueId);
        } else {
            Clinical_CustomFormsPreview.updateCustomFormHtml('', customFormId, NoteHTMLCtrl, hideAlertMessage, customFormUniqueId);
            //Clinical_ProgressNote.updateProgressNoteHTML(null, null, hideAlertMessage);
            var customForm = $(NoteHTMLCtrl + ' clinical_customform[uniqueid=' + customFormUniqueId + ']');
            Clinical_ProgressNote.saveComponentSOAPText('Custom Forms', hideAlertMessage, null, null, null, null, customForm);
        }
    },
    updateCustomFormHtml: function (CustomFormHtml, customFormId, NoteHTMLCtrl, hideAlertMessage, customFormUniqueId) {
        if (customFormUniqueId) {
            var customForm = $(NoteHTMLCtrl + ' clinical_customform[uniqueid=' + customFormUniqueId + ']');
            if (customForm.attr('uniqueid') == customFormUniqueId) {
                customForm.parent().parent().addClass('initialVisitBody');
                if (CustomFormHtml != '') {
                    if (customForm.parent().parent().find('section').length > 0) {
                        customForm.parent().parent().find('section').remove();
                    }
                    customForm.parent().parent().append(CustomFormHtml);
                }
            }
        }
        if (CustomFormHtml != '' && customFormId != null && customFormId != '') {
            Clinical_CustomFormsPreview.attachCustomFormWithNotes(customFormId, hideAlertMessage, customForm);
        }
        
            customForm.parent().parent().find('section').addClass("disableAll");
       
    },
    attachCustomFormWithNotes: function (customFormId, hideAlertMessage, customForm) {
        Clinical_CustomFormsPreview.attachCustomFormWithNotes_DBCall(customFormId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ProgressNote.saveComponentSOAPText('Custom Forms', hideAlertMessage, null, null, null, null, customForm);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    attachCustomFormWithNotes_DBCall: function (customFormId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["CustomFormId"] = customFormId;
        objData["commandType"] = "attach_custom_form_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");

    },

    attachCustomFormWithPatient: function (customFormId, hideAlertMessage) {
        Clinical_CustomFormsPreview.attachCustomFormWithNotes_DBCall(customFormId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ProgressNote.saveComponentSOAPText('Custom Forms', hideAlertMessage);
                //Clinical_ProgressNote.updateProgressNoteHTML(null, null, hideAlertMessage);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    attachCustomFormWithPatient_DBCall: function (customFormId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["CustomFormId"] = customFormId;
        objData["commandType"] = "attach_custom_form_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");

    },

    detachCustomForm: function (ComponentId, IsUpdate, CustomFormComponentRemove, CustomFormUniqueId) {
        if (CustomFormComponentRemove) {


            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .CustomFormsComponent').attr('NoteComponentId');

            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                $.when(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId)).then(function () {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Custom Forms']").remove();
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_customform').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Custom Forms']").remove();
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_customform').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
            }


            var objDeffered = $.Deferred();
            var totalContents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_customform');
            if (totalContents && totalContents.length > 0) {
                totalContents.each(function (i, e) {
                    var CustomFormUniqueId = $(e).attr('uniqueid');
                    var UniqueId = $(e).attr('id');
                    if (CustomFormUniqueId) {
                        Clinical_CustomFormsPreview.detachCustomFormFromNotes_DBCall(UniqueId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                var section = '#Cli_CustomForm_Main' + UniqueId;
                                //$('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML' + section).closest('li.initialVisitBody').remove();
                                $(e).closest('li.initialVisitBody').remove();
                                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button").each(function (i, btn) {
                                    if ($(btn).attr('id') == UniqueId) {
                                        $(btn).remove()
                                    }
                                    objDeffered.resolve();
                                });
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });

                    }
                });
            }
            else {
                objDeffered.resolve();
            }
            objDeffered.resolve().then(function () {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_customforms').closest('li.initialVisitBody').remove();
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button.btn-customForm").remove();

                var customForm = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_customform[uniqueid=' + CustomFormUniqueId + ']');
                Clinical_ProgressNote.saveComponentSOAPText('Custom Forms', true, null, null, null, null, customForm);
                //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            });
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_customform').parent().parent().find('section[id*="Cli_CustomForm_Main"]').remove();
        }
    },
    detachCustomFormComponent: function (ComponentId, IsUpdate, CustomFormComponentRemove, CustomFormUniqueId, customFormNameForDoc, IsLength) {
        var problemIds = [];
        var $customForm = $("#Cli_CustomForm_" + CustomFormUniqueId);
        var procedureIds = [];
        $customForm.find($("[questionid^='toolProcedures_'] span")).each(function (i, el) {
            var prcocedureId = $(el).attr('id');
            procedureIds.push(prcocedureId);
        });
        if (procedureIds.length > 0) {
            Clinical_CustomFormsPreview.DeleteProcedure(procedureIds.join()).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) { }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

        var customForm = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_customform[uniqueid=' + CustomFormUniqueId + ']');
        Clinical_CustomFormsPreview.detachCustomFormFromNotes_DBCall(ComponentId, customFormNameForDoc).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (IsUpdate) {
                    Clinical_ProgressNote.saveComponentSOAPText("Custom Forms", true, null, null, null, null, customForm);
                    //Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                }
                utility.DisplayMessages(response.Message, 1);
                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        if (CustomFormComponentRemove) {

            //var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_customform').parent().parent().attr('NoteComponentId');
            var NoteComponentId = $(customForm).closest('li').attr('NoteComponentId');

            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button").each(function (i, e) {
                if ($(e).attr('uniqueid') == CustomFormUniqueId) {
                    $(e).remove()
                }
            });
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_customform').each(function (i, elem) {
                if ($(elem).attr('uniqueid') == CustomFormUniqueId) {
                    if (Clinical_ProgressNote.params["TemplateName"])
                        $(elem).parent().parent().addClass('hidden');
                    else
                        $(elem).parent().parent().remove();
                }
            });

            var CFComponents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' .CustomFormsComponent').length;
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_customform[uniqueid=' + CustomFormUniqueId + ']').parent().parent().addClass('hidden');
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Patient Education', true))
            }
            else {
                if (NoteComponentId && NoteComponentId != "NCDummyId")
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
            }
            $.when.apply($, promise).done(function () {
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_customform[uniqueid=' + CustomFormUniqueId + ']').parent().parent().remove();
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                utility.DisplayMessages('Successfully Deleted', 1);
            });
        }
    },

    detachCustomFormFromNotes_DBCall: function (customFormId, customFormNameForDoc) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["CustomFormId"] = customFormId;
        objData["CustomFormDocName"] = customFormNameForDoc;
        objData["commandType"] = "detach_custom_form_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    getCustomFormSentenceView: function (customFormMarkup) {
        var customFormSentenceView = '';
        $(customFormMarkup).find('li').each(function (i, elem) {
            var question = $(elem).find('div');
            var questionType = question.attr('questiontype');

            switch (questionType) {
                case "TextField":
                    customFormSentenceView += "<li style='white-space:pre-line' questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getTextFieldSentenceView(question);
                    break;
                case "CheckBox":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getCheckBoxSentenceView(question);
                    break;
                case "YesNo":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getYesNoSentenceView(question);
                    break;
                case "Toggle":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getToggleSentenceView(question);
                    break;
                case "SingleSelectDropdown":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getSSDSentenceView(question);
                    break;
                case "MultipleSelectCombo":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getMSDSentenceView(question);
                    break;
                case "FractionField":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getFractionSentenceView(question)
                    break;
                case "FreeText":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getFreeTextSentenceView(question);
                    break;
                case "Image":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getImageSentenceView(question);
                    break;
                case "DateField":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getDateFieldSentenceView(question);
                    break;
                case "TimeField":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getTimeFieldSentenceView(question);
                    break;
                case "Table":
                    customFormSentenceView += "<li class='Of-a' questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getTableSentenceView(question);
                    break;
                    //Begin Edited By Fahad Malik on 29-11-2016 to fix bug#: EMR-2006
                case "Header":
                    var lbl = $(question).find('label');
                    var getstyle = $(lbl).attr('style');
                    customFormSentenceView += "<li class='" + $(lbl).attr('class') + "' style='display: inline;" + getstyle + "' questionid='" + question.attr('id') + "'> " + lbl.text();
                    break;
                    //End Edited By Fahad Malik on 29-11-2016 to fix bug#: EMR-2006
                case "Problems":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getProblemsSentenceView(question);
                    break;
                case "Procedures":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getProceduresSentenceView(question);
                    break;
                case "TinyMCEditor":
                    customFormSentenceView += "<li questionid='" + question.attr('id') + "'> " + Clinical_CustomFormsPreview.getTinyMCEditorView(question);
                    break;

                default:
            }
        });
        return customFormSentenceView;
    },

    getTextFieldSentenceView: function (question) {
        var str = '';
        var inputValue;
        if ($(question).attr('issingleline') == "true") {
            inputValue = $(question).find('input').val();
            inputValue ? str += question.attr('questiontitle') + ": " + inputValue : str;
        } else {
            if ($(question).find('textarea') && $(question).find('textarea').length > 0) {
                inputValue = $(question).find('textarea').val();
                //inputValue = inputValue.split(/\n/);
                inputValue ? str += question.attr('questiontitle') + ": " + inputValue : str;
            }
            /* $.each(inputValue, function (i, e) {
                 if (i > 0) {
                     str += "<li> " + e + "</li>";
                 }
             });*/
        }
        return str;
    },

    getCheckBoxSentenceView: function (question) {
        var str = '';
        var checkboxChecked = '<i class="fa fa-check-square-o" aria-hidden="true"></i>';
        var checkboxUnChecked = '<i class="fa fa-square-o" aria-hidden="true"></i>';
        var selectedCheckboxes = question.find('#customFormCheckBoxPreview div input').map(function () {
            return this.checked
        });
        selectedCheckboxes = $.makeArray(selectedCheckboxes);
        var isAllFalse = selectedCheckboxes.every(Clinical_CustomFormsPreview.isAllFalse);
        if (!isAllFalse) {
            if (question.find('#lblCheckBoxTitle'))
                str += question.find('#lblCheckBoxTitle').text() != '' ? question.find('#lblCheckBoxTitle').text() + ': ' : '';
            $.each(question.attr('questionlabel').split(','), function (i, elem) {
                str += selectedCheckboxes[i] ? checkboxChecked + '&nbsp;' + elem + '&nbsp;' : checkboxUnChecked + '&nbsp;' + elem + '&nbsp;&nbsp;&nbsp;';
            });
        }
        return str;

    },

    getYesNoSentenceView: function (question) {
        var str = '';
        var isNegative = '';
        var selectedCheckboxes = question.find('#customFormYesNoPreview div input').map(function () {
            return this.checked
        });
        selectedCheckboxes = $.makeArray(selectedCheckboxes);
        var isAllFalse = selectedCheckboxes.every(Clinical_CustomFormsPreview.isAllFalse);
        if (!isAllFalse) {
            $.each(selectedCheckboxes, function (i, e) {
                e && i == 0 ? isNegative = 'Yes' : '';
                e && i == 1 ? isNegative = 'No' : '';
            });
            str += question.attr('questionlabel') + ": " + isNegative;
        }
        return str;
    },

    getToggleSentenceView: function (question) {
        var str = '';
        var isNegative = '';
        var isChecked = question.find('.ios-switch').hasClass('on');
        isChecked ? isNegative = 'Positive' : isNegative = 'Negative';
        str += question.attr('questionlabel') + ": " + isNegative;
        return str;
    },

    getSSDSentenceView: function (question) {
        var str = '';
        var selectedValue = parseInt(question.find('select').val()) - 1;
        if (selectedValue >= 0) {
            str += question.attr('questionlabel') + ": " + JSON.parse(question.attr('dropdownvalues'))[selectedValue];
        }
        return str;
    },

    getMSDSentenceView: function (question) {
        var str = '';
        var selectedText = '';
        var selectedValues = question.find('select option:selected').map(function () {
            return this.text
        });
        if (selectedValues.length > 0) {
            var selectedText = $.makeArray(selectedValues).join();
            selectedText = selectedText.replace(/,/g, ", ");
            str += question.attr('questionlabel') + ": " + selectedText.replace(/,([^,]*)$/, ' and $1');
        }
        return str;
    },

    getFractionSentenceView: function (question) {
        var str = '';
        var txtFractionField1 = question.find('#txtFractionField1').val();
        var txtFractionField2 = question.find('#txtFractionField2').val();
        if (txtFractionField1 && txtFractionField2) {
            if (question.find('#lblFractionTitle').text().indexOf('*') > -1) {
                str += question.find('#lblFractionTitle').text().replace('*', '') + ": " + txtFractionField1 + "/" + txtFractionField2;
            } else {
                str += question.find('#lblFractionTitle').text() + ": " + txtFractionField1 + "/" + txtFractionField2;
            }

        }
        return str;
    },

    getFreeTextSentenceView: function (question) {
        var str = '';
        var textareaText = question.find('textarea').val();
        if (textareaText) {
            if (question.attr('textcase') == "Upper") {
                textareaText = textareaText.toUpperCase();
            } else if (question.attr('textcase') == "Lower") {
                textareaText = textareaText.toLowerCase();
            }
            if (question.find('#lblFreeText').text().indexOf('*') > -1) {
                str += question.find('#lblFreeText').text().replace('*', '') + ": " + textareaText;
            } else {
                str += question.find('#lblFreeText').text() + ": " + textareaText;
            }

        }
        return str;
    },

    getImageSentenceView: function (question) {
        var str = '';
        if ($(question).find('img').length > 0) {
            str += question.find('#customFormImage').html();
        }
        else {
            str = '';
        }
        return str;
    },

    getDateFieldSentenceView: function (question) {
        var str = '';
        question.find('input').val() ? str += question.find('input').val() : str;
        return str;
    },

    getTimeFieldSentenceView: function (question) {
        var str = '';
        str += question.find('input').val();
        return str;
    },

    getTableSentenceView: function (question) {
        var str = '';
        if (!(Clinical_CustomFormsPreview.isTableEmpty(question))) {
            str += question.find('#tblContext')[0].outerHTML;
        }
        return str;
    },
    getProblemsSentenceView: function (question) {
        var str = '';
        var $problemLi = question.find('#customFormProblemsList li');

        if ($problemLi.length > 0) {
            str = question.attr('questiontitle') + ': ';
            if (question.attr('issingleline')) {
                $problemLi.each(function (i, e) {
                    i > 0 ? str += '<br>' : '';
                    str += "<span id=\"" + $(e).attr('id') + "\" icd9Code=\"" + $(e).attr('icd9Code') + "\" icd9Desc=\"" + $(e).attr('icd9Desc') + "\" icd10Code=\"" + $(e).attr('icd10Code') + "\" icd10Desc=\"" + $(e).attr('icd10Desc') + "\" snomedCode=\"" + $(e).attr('snomedCode') + "\" snomedDesc=\"" + $(e).attr('snomedDesc') + "\"><strong>" + $(e).text() + ' <a onclick="Clinical_InfoButtonView.getInfofromMediPlus(\'' + +$(e).attr('icd10code') + '\',\'clinicalTabProgressNote\',\'2\',\'\',\'Clinical_ProblemLists\')" style="cursor:pointer"><b>(Info)</b></a> </strong> Modified on ' + moment().format('MM/DD/YYYY') + '.</span>';
                });
            } else {
                $problemLi.each(function (i, e) {
                    str += " <span id=\"" + $(e).attr('id') + "\" icd9Code=\"" + $(e).attr('icd9Code') + "\" icd9Desc=\"" + $(e).attr('icd9Desc') + "\" icd10Code=\"" + $(e).attr('icd10Code') + "\" icd10Desc=\"" + $(e).attr('icd10Desc') + "\" snomedCode=\"" + $(e).attr('snomedCode') + "\" snomedDesc=\"" + $(e).attr('snomedDesc') + "\"><strong>" + $(e).text() + ' <a onclick="Clinical_InfoButtonView.getInfofromMediPlus(\'' + $(e).attr('icd10code') + '\',\'clinicalTabProgressNote\',\'2\',\'\',\'Clinical_ProblemLists\')" style="cursor:pointer"><b>(Info)</b></a> </strong> Modified on ' + moment().format('MM/DD/YYYY') + '.</span>';
                });
            }
        }
        return str;
    },
    getProceduresSentenceView: function (question) {
        var str = '';
        var $procLi = question.find('#customFormProceduresList li');

        if ($procLi.length > 0) {
            str = question.attr('questiontitle') + ': ';
            if (question.attr('issingleline')) {
                $procLi.each(function (i, e) {
                    i > 0 ? str += '<br>' : '';
                    $.each(Clinical_CustomFormsPreview.ProviderCPTs, function (index, item) {
                        if (item.ProcedureId == $(e).attr('id') && item.CPTCode == $(e).attr('cptCode') && item.CPTCodeDescription == $(e).attr('cptDescription')) {
                            if (item.ShowCPTCode == 1) {
                                str += " <span id=\"" + $(e).attr('id') + "\" cptcode=\"" + $(e).attr('cptCode') + "\" cptDescription=\"" + $(e).attr('cptDescription') + "\">" + $(e).attr('cptcode') + ' - ' + $(e).attr('cptdescription') + '.</span>';
                            }
                            else {
                                str += " <span id=\"" + $(e).attr('id') + "\" cptcode=\"" + $(e).attr('cptCode') + "\" cptDescription=\"" + $(e).attr('cptDescription') + "\">" + $(e).attr('cptdescription') + '.</span>';
                            }
                        }
                    });
                });
            }
            else {
                $procLi.each(function (i, e) {
                    $.each(Clinical_CustomFormsPreview.ProviderCPTs, function (index, item) {
                        if (item.ProcedureId == $(e).attr('id') && item.CPTCode == $(e).attr('cptCode') && item.CPTCodeDescription == $(e).attr('cptDescription')) {
                            if (item.ShowCPTCode == 1) {
                                str += " <span id=\"" + $(e).attr('id') + "\" cptcode=\"" + $(e).attr('cptCode') + "\" cptDescription=\"" + $(e).attr('cptDescription') + "\">" + $(e).attr('cptcode') + ' - ' + $(e).attr('cptdescription') + '.</span>';
                            }
                            else {
                                str += " <span id=\"" + $(e).attr('id') + "\" cptcode=\"" + $(e).attr('cptCode') + "\" cptDescription=\"" + $(e).attr('cptDescription') + "\">" + $(e).attr('cptdescription') + '.</span>';
                            }
                        }
                    });
                });
            }
        }
        return str;
    },

    getProviderCPTs_DBCall: function () {
        var objData = new Object();
        objData["CustomFormId"] = Clinical_CustomFormsPreview.params.CustomFormId;
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["commandType"] = "get_provider_CPTs";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");
    },

    isAllFalse: function (element, index, array) {
        return element == false;
    },

    fillCustomFormOnNotes: function () {
        var customForm = $('#actionPanClinicalProgressNote #customFormDetails');
        var customFormData = $('#pnlClinicalProgressNote #ProgressnoteHTML' + ' #Cli_CustomForm_' + Clinical_CustomFormsPreview.params.CustomFormUniqueId + '').find('li')
        customFormData.each(function (i, elem) {
            var questionId = $(elem).attr('questionid');
            var question = customForm.find('#' + questionId)
            var questionType = $(question).attr('questiontype');
            if (question.length > 0) {
                switch (questionType) {
                    case "TextField":

                        Clinical_CustomFormsPreview.fillTextFieldFromNotes(question, elem);
                        break;
                    case "CheckBox":
                        Clinical_CustomFormsPreview.fillCheckBoxFromNotes(question, elem);
                        break;
                    case "YesNo":
                        Clinical_CustomFormsPreview.fillYesNoFromNotes(question, elem);
                        break;
                    case "Toggle":
                        Clinical_CustomFormsPreview.fillToggleFromNotes(question, elem);
                        break;
                    case "SingleSelectDropdown":
                        Clinical_CustomFormsPreview.fillSSDFromNotes(question, elem);
                        break;
                    case "MultipleSelectCombo":
                        Clinical_CustomFormsPreview.fillMSDFromNotes(question, elem);
                        break;
                    case "FractionField":
                        Clinical_CustomFormsPreview.fillFractionFromNotes(question, elem);
                        break;
                    case "FreeText":
                        Clinical_CustomFormsPreview.fillFreeTextFromNotes(question, elem);
                        break;
                        //case "Image":
                        //    Clinical_CustomFormsPreview.fillFreeTextFromNotes(question);
                        //    break;
                    case "DateField":
                        Clinical_CustomFormsPreview.fillDateFieldFromNotes(question, elem);
                        break;
                    case "TimeField":
                        Clinical_CustomFormsPreview.fillTimeFieldFromNotes(question, elem);
                        break;
                    case "Table":
                        Clinical_CustomFormsPreview.fillTableFromNotes(question, elem);
                        break;
                    case "Problems":
                        Clinical_CustomFormsPreview.fillProblemsFromNotes(question, elem);
                        break;
                    case "Procedures":
                        Clinical_CustomFormsPreview.fillProceduresFromNotes(question, elem);
                        break;
                    default:
                }
            }

        });

    },

    fillTextFieldFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            if ($(question).find('input').length > 0) {
                $(question).find('input').val($(elem).text().substring($(elem).text().indexOf(':')).replace(': ', '').trim());
            } else {
                var fieldText = $(elem).text().substring($(elem).text().indexOf(':')).replace(': ', '').trim();
                //$(elem).siblings().each(function (i, e) {
                //    if (!$(e).text().indexOf(':') > 0)
                //        fieldText += "\n" + $(e).text().trim();
                //});
                $(question).find('textarea').val(fieldText.trim());
            }
        }
    },
    fillCheckBoxFromNotes: function (question, elem) {
        var selectedCheckboxes = $(elem).find('i').map(function () {
            return $(this).hasClass('fa fa-check-square-o');
        });
        question.find('#customFormCheckBoxPreview div input').each(function (i, e) {
            $(e).prop('checked', false);
            $(e).removeAttr('checked');
        });
        question.find('#customFormCheckBoxPreview div input').each(function (i, e) {
            if (selectedCheckboxes[i]) {
                $(e).prop('checked', true);
                $(e).attr('checked', 'checked');
            }
        });
    },
    fillYesNoFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            if ($(elem).text().split(':')[1].trim() == "Yes") {
                //EMR - 6529 BY mahmad
                question.find('input[id*=chkYes]').prop('checked', true).attr('checked', 'checked');
                question.find('input[id*=chkNo]').prop('checked', false).attr('checked', '');
                //EMR - 6529 BY mahmad
            } else if ($(elem).text().split(':')[1].trim() == "No") {
                //EMR - 6529 BY mahmad
                question.find('input[id*=chkNo]').prop('checked', true).attr('checked', 'checked');
                question.find('input[id*=chkYes]').prop('checked', false).attr('checked', '');
                //EMR - 6529 BY mahmad
            }
        }
    },
    fillToggleFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            if ($(elem).text().split(':')[1].trim() == "Positive") {
                question.find('.ios-switch').addClass('on');
            } else if ($(elem).text().split(':')[1].trim() == "Negative") {
                question.find('.ios-switch').removeClass('on');
            }
        }
    },
    fillSSDFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            question.find('select option').each(function (i, e) {
                if ($(e).text() == $(elem).text().split(':')[1].trim()) {
                    question.find('select').val(i);
                }
            });
        }
    },
    fillMSDFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            var sentenceValues = $(elem).text().split(':')[1].replace(' and ', ',').trim().split(',');
            var ddlVal = JSON.parse(question.attr('dropdownvalues'));
            var selectedValues = [];
            $.each(sentenceValues, function (i, e) {
                $.each(ddlVal, function (ii, ee) {
                    if (e.trim() == ee)
                        selectedValues.push(ii)
                });
            });

            selectedValues = selectedValues.join();
            question.find("[id^='customFormMultipleSelectCombo'] select").val('');
            EMRUtility.selectOptionsByCommaSeprateValue(question.find("[id^='customFormMultipleSelectCombo'] select"), selectedValues);
            question.find("[id^='customFormMultipleSelectCombo'] select").multiselect("refresh");
        }
    },
    fillFractionFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            var fraction = $(elem).text().substring($(elem).text().indexOf(':')).replace(': ', '').split('/');;
            var frac1 = fraction[0];
            var frac2 = fraction[1];
            question.find('#txtFractionField1').val(frac1);
            question.find('#txtFractionField2').val(frac2);
        }
    },
    fillFreeTextFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            question.find('textarea').val($(elem).text().substring($(elem).text().indexOf(':')).replace(': ', '').trim());
        }
    },
    fillDateFieldFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            question.find('input').val($(elem).text().trim());
        }
    },
    fillTimeFieldFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            question.find('input').val($(elem).text().trim());
        }
    },
    fillTableFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            question.find('#tblContext')[0].outerHTML = $(elem).find('table')[0].outerHTML;
        }
    },
    fillProblemsFromNotes: function (question, elem) {
        var problemsDiv = $('#pnlClinicalCustomFormsPreview #frmCustomFormPreview .problemsListDiv').clone().removeClass('problemsListDiv');

        $(elem).find('span').each(function (index, item) {
            var currId = $(this).attr('id');
            var icd9Code = $(this).attr('icd9Code');
            var icd9Description = $(this).attr('icd9Desc');
            var icd10Code = $(this).attr('icd10Code');
            var icd10Description = $(this).attr('icd10Desc');
            var snomedCode = $(this).attr('snomedCode');
            var snomedDescription = $(this).attr('snomedDesc');

            var li = "<li  id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' icd9Code=\"" + icd9Code + "\" icd9Desc=\"" + icd9Description + "\" icd10Code=\"" + icd10Code + "\" icd10Desc=\"" + icd10Description + "\" snomedCode=\"" + snomedCode + "\" snomedDesc=\"" + snomedDescription + "\"><a href='#'>" + icd10Code + " - " + icd10Description + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProblem($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

            var problemsExist = $(problemsDiv).find("ul#customFormProblemsList > li").length;

            if (problemsExist == 0) {

                $(problemsDiv).find("ul#customFormProblemsList").append(li);
                $(problemsDiv).css('display', 'block');
                $(question).append($(problemsDiv));
                $(question).css('height', 'auto');
            }
            else {
                $(problemsDiv).find("ul#customFormProblemsList").append(li);
            }

        });
    },

    fillProceduresFromNotes: function (question, elem) {

        var procedureDiv = $('#pnlClinicalCustomFormsPreview #frmCustomFormPreview .proceduresListDiv').clone().removeClass('proceduresListDiv');

        $(elem).find('span').each(function (index, item) {
            var id = $(this).attr('id');
            var cptcode = $(this).attr('cptcode');
            var cptDescription = $(this).attr('cptDescription');
            var li = "<li  id=" + id + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' cptcode=\"" + cptcode + "\" cptDescription=\"" + cptDescription + "\"><a href='#'>" + cptcode + " - " + cptDescription + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProcedure($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

            var procedureExist = $(procedureDiv).find("ul#customFormProceduresList > li").length;

            if (procedureExist == 0) {
                $(procedureDiv).find("ul#customFormProceduresList").append(li);
                $(procedureDiv).css('display', 'block');
                $(question).append($(procedureDiv));
                $(question).css('height', 'auto');
            }
            else {
                $(question).find("ul#customFormProceduresList").append(li);
            }
        });
    },

    isTableEmpty: function (question) {
        var isEmpty = question.find('Table td').map(function (e) {
            return $(this).text().length > 0;
        });

        isEmpty = $.makeArray(isEmpty);
        var isAllFalse = isEmpty.every(Clinical_CustomFormsPreview.isAllFalse);
        return isAllFalse;
    },

    BindICD9AutoComplete: function (element) {

        var selectedProblemTool = $(element).parents("div[id*='toolProblems_']");
        if (selectedProblemTool) {
            var parentId = $(selectedProblemTool).attr('id');
            $(element).attr("data-popupunload", "false");

            var descriptionCrtl = $(element);
            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_CustomFormsPreview", null, false, null, parentId);
        }
    },

    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },

    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },

    deleteCustomFormProblem: function (obj, ev) {
        var dfd = new $.Deferred();
        ev.stopPropagation();
        var problemsList = $(obj).parent();
        utility.myConfirm('23', function () {
            var problemId = $(obj).attr('id');
            $(obj).addClass('hidden');
            utility.DisplayMessages("Successfully Deleted", 1);
            dfd.resolve();

            var problemsCount = $(problemsList).find('li').length;
            var removedProblemsCount = $(problemsList).find('li.hidden').length;

            if (problemsCount == removedProblemsCount) {
                var selectedProblemTool = $(problemsList).parents("div[id*='toolProblems_']");
                $(selectedProblemTool).children().last().css('display', 'none');
            }
        }, function () {
        },
            '23'
    );
    },

    openICDCode: function (Ctrl) {
        var selectedProblemTool = $(Ctrl).parents("div[id*='toolProblems_']");
        var parentId = $(selectedProblemTool).attr('id');
        var txtProblem = $(selectedProblemTool).find('#txtProblemsCustomForm');
        var parentCtrl = 'Clinical_CustomFormsPreview';
        if ($(Ctrl).parents('#tblBatch_FaxSend').length > 0) {
            parentCtrl = "Batch_FaxSend";
        }
        else {
            parentCtrl = 'Clinical_CustomFormsPreview';
        }

        var controlToLoad = "Admin_IMOICD";
        txtProblem.attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = parentCtrl;
        params["RefCtrl"] = txtProblem;
        params["ContainerProblemDivId"] = parentId;

        LoadActionPan(controlToLoad, params);
    },

    bindAutoComplete: function (element) {

        var selectedProcedureTool = $(element).parents("div[id*='toolProcedures_']");
        if (selectedProcedureTool) {
            var parentId = $(selectedProcedureTool).attr('id');
            $(element).attr("data-popupunload", "false");

            utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', $(element), null, true, -1, "CPT", true, "Clinical_CustomFormsPreview", element, true, null, parentId);
        }
    },

    BindProcedureListItem: function (cptCode, cptDescription, ContainerCrtl) {

        var selectedProcedureTool = $(ContainerCrtl).parents("div[id*='toolProcedures_']");

        var currId = -1;
        $(selectedProcedureTool).find("ul#customFormProceduresList li[id*='-']").each(function (i, item) {
            currId = $(this).attr("id");
        });

        currId = parseInt(currId) + (-1);
        var li = "<li isnew='true' id=" + currId + " onclick='' onmouseover='Clinical_CustomFormsPreview.showIcon(this);' onmouseout='Clinical_CustomFormsPreview.hideIcon(this);' cptcode=\"" + cptCode + "\" cptDescription=\"" + cptDescription + "\"><a href='#'>" + cptCode + " - " + cptDescription + "<span class='removeIconListHover' onclick='Clinical_CustomFormsPreview.deleteCustomFormProcedure($($(this).parent()).parent(), event);'><i class='fa fa-close'></i></span></a></li>"

        var IsAlreadyExist = false;

        $(selectedProcedureTool).find("ul#customFormProceduresList li").each(function (i, item) {
            var code = $(item).attr('cptcode');
            var desc = $(item).attr('cptDescription');
            if (cptCode == code && $.trim(cptDescription.toLowerCase()) == $.trim(desc.toLowerCase())) {
                IsAlreadyExist = true;
                return;
            }
        });

        if (!IsAlreadyExist) {

            var procedureExist = $(selectedProcedureTool).find("ul#customFormProceduresList > li").length;

            if (procedureExist == 0) {
                var procedureDiv = $('#pnlClinicalCustomFormsPreview #frmCustomFormPreview .proceduresListDiv').clone().removeClass('proceduresListDiv');
                $(procedureDiv).find("ul#customFormProceduresList").append(li);
                $(procedureDiv).css('display', 'block');
                $(selectedProcedureTool).append($(procedureDiv));
                $(selectedProcedureTool).css('height', 'auto');
            }
            else {
                $(selectedProcedureTool).find("ul#customFormProceduresList").append(li);
                $(selectedProcedureTool).children().last().css('display', 'block');
            }

            if (Clinical_CustomFormsPreview.params.ParentCtrl != "Favorite_CustomForms") {
                Clinical_CustomFormsPreview.saveProcedure(li, selectedProcedureTool);
            }

            $(ContainerCrtl).val('');
            if ($(ContainerCrtl).attr('style')) {
                $(ContainerCrtl).removeAttr('style');
            }
        }
        else {
            utility.DisplayMessages('Procedure already added', 2);
            $(ContainerCrtl).val('');
        }
    },

    deleteCustomFormProcedure: function (obj, ev) {

        var dfd = new $.Deferred();
        ev.stopPropagation();
        var proceduresList = $(obj).parent();
        utility.myConfirm('23', function () {
            var procedureId = $(obj).attr('id');
            $(obj).addClass('hidden');
            utility.DisplayMessages("Successfully Deleted", 1);
            dfd.resolve();

            var proceduresCount = $(proceduresList).find('li').length;
            var removedProceduresCount = $(proceduresList).find('li.hidden').length;

            if (proceduresCount == removedProceduresCount) {
                var selectedProcedureTool = $(proceduresList).parents("div[id*='toolProcedures_']");
                $(selectedProcedureTool).children().last().css('display', 'none');
            }
        }, function () {
        },
            '23'
    );
    },

    openCPTCode: function (Ctrl) {

        var selectedProcedureTool = $(Ctrl).parents("div[id*='toolProcedures_']");
        var parentId = $(selectedProcedureTool).attr('id');
        var params = [];

        if ($(Ctrl).parents('#tblBatch_FaxSend').length > 0) {
            params["FromAdmin"] = "0";
            params["ParentCtrl"] = "Batch_FaxSend";
            params["ParentCtrlPanelID"] = Batch_FaxSend.params.PanelID;
            params["ContainerProcedureDivId"] = parentId;
            LoadActionPan('Admin_IMOCPT', params, Batch_FaxSend.params.PanelID);
        }
        else {
            params["FromAdmin"] = "0";
            params["ParentCtrl"] = "Clinical_CustomFormsPreview";
            params["ParentCtrlPanelID"] = Clinical_CustomFormsPreview.params.PanelID;
            params["ContainerProcedureDivId"] = parentId;
            LoadActionPan('Admin_IMOCPT', params, Clinical_CustomFormsPreview.params.PanelID);
        }
    },

    removeDeletedCPTAndICD: function () {

        var problemTools = $('#' + Clinical_CustomFormsPreview.params.PanelID + ' #frmPreview').find("div[id*='toolProblems_']");
        var problemIds = [];

        $(problemTools).each(function (index, item) {
            var problemsList = $(item).find('ul#customFormProblemsList');
            var removedCount = $(problemsList).find('li.hidden').length;
            if (removedCount > 0) {
                $(problemsList).find('li.hidden').each(function (i, el) {
                    var problemId = $(el).attr('id');
                    problemIds.push(problemId);
                    $(el).remove();
                });
            }
        });

        if (problemIds.length > 0) {
            Clinical_CustomFormsPreview.DeleteProblemList(problemIds.join()).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }


        var procedureTools = $('#' + Clinical_CustomFormsPreview.params.PanelID + ' #frmPreview').find("div[id*='toolProcedures_']");
        var procedureIds = [];

        $(procedureTools).each(function (index, item) {
            var procedureTools = $(item).find('ul#customFormProceduresList');
            var removedCount = $(procedureTools).find('li.hidden').length;
            if (removedCount > 0) {
                $(procedureTools).find('li.hidden').each(function (i, el) {
                    var prcocedureId = $(el).attr('id');
                    procedureIds.push(prcocedureId);
                    $(el).remove();
                });
            }
        });

        if (procedureIds.length > 0) {
            Clinical_CustomFormsPreview.DeleteProcedure(procedureIds.join()).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

    },


    DeleteProblemList: function (ProblemListIds) {

        var objData = new Object();
        objData["ProblemListIds"] = ProblemListIds;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "delete_detach_problems";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");

    },

    DeleteProcedure: function (ProcedureIds) {

        var objData = new Object();
        objData["ProcedureIds"] = ProcedureIds;
        objData["commandType"] = "delete_detach_procedures";
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CustomForm", "CustomForm");

    },

    saveProblem: function (problem, txtBoxCtrlParentId) {
        if (Clinical_CustomFormsPreview.params.ParentCtrl == "Patient_CustomForm") {
            return;
        }
        var dfd = $.Deferred();
        var problemid = $(problem).attr('id');
        var selectedProblemTool = $("#pnlClinicalCustomFormsPreview #frmCustomFormPreview").find('#' + txtBoxCtrlParentId);
        Clinical_CustomFormsPreview.SaveProblemList(problem).done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {

                $(selectedProblemTool).find("ul#customFormProblemsList > li#" + problemid).attr('id', response.ProblemListId);
                //Clinical_CustomFormsPreview.attachProbleWithNote(response.ProblemListId);
                dfd.resolve();
            }
            else {
                if (response.Message.toLowerCase() == "problem already exists.") {

                    utility.DisplayMessages("Selected ICD/Problem also exists under Patient Problems", 3);
                    if (!(Clinical_CustomFormsPreview.CheckProblemAllReadyInCustomForm(selectedProblemTool, problemid))) {
                        $(selectedProblemTool).find("ul#customFormProblemsList > li#" + problemid).attr('id', response.ProblemListId);
                    }
                    else {
                        $(selectedProblemTool).find("ul#customFormProblemsList > li#" + problemid).remove();
                    }
                    dfd.resolve();
                }
                else {


                    $(selectedProblemTool).find("ul#customFormProblemsList > li#" + problemid).remove();
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve();
                }
            }
        });

        return dfd;
    },

    CheckProblemAllReadyInCustomForm: function (selectedProblemTool, id) {
        var found = false;
        var icd10code = $(selectedProblemTool).find("ul#customFormProblemsList > li#" + id).attr('icd10code');
        $.each($(selectedProblemTool).find("ul#customFormProblemsList > li"), function (i, item) {
            if ($(item).attr("id") != id && $(item).attr('icd10code') == icd10code && !$(item).hasClass("hidden")) {
                found = true;
            }
        })
        return found;
    },
    SaveProblemList: function (problem) {

        var objData = new Object();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();

        objData["ProblemListId"] = $(problem).attr("id");
        objData["ICD9"] = $(problem).attr("icd9code");
        objData["ICD10"] = $(problem).attr("icd10code");
        objData["ICD9_Description"] = $(problem).attr("icd9desc");
        objData["ICD10_Description"] = $(problem).attr("icd10desc");
        objData["SNOMEDID"] = $(problem).attr("snomedcode");
        objData["SNOMED_DESCRIPTION"] = $(problem).attr("snomeddesc");
        objData["Description"] = $(problem).attr("icd10code") + " - " + $(problem).attr("icd10desc");
        objData["ProblemName"] = $(problem).attr("icd10desc");
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["CustomFormId"] = Clinical_CustomFormsPreview.params.CustomFormId;
        objData["CheckProblemExists"] = "1";
        objData["commandType"] = "SAVE_PROBLEMLIST";

        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },


    attachProbleWithNote: function (ProblemsListId) {
        Clinical_CustomFormsPreview.attachProblemsListWithNotes_DBCall(ProblemsListId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ProblemListSoapCount > 0) {
                    Clinical_ProblemLists.createProblemListBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', null, true);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    attachProblemsListWithNotes_DBCall: function (ProblemsListId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProblemListId"] = ProblemsListId;
        objData["commandType"] = "attach_problem_with_notes_load_soap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "ProblemList");
    },

    saveProcedure: function (procedure, procedureTool) {
        if (Clinical_CustomFormsPreview.params.ParentCtrl == "Patient_CustomForm") {
            return;
        }
        var objDeffered = $.Deferred();
        var procedureId = $(procedure).attr('id');

        Clinical_CustomFormsPreview.saveProcedure_DbCall(procedure).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var ProceduresLoadJSONData = JSON.parse(response.ProcedureLoad_JSON);
                var updateProcedureId = ProceduresLoadJSONData[0].ProcedureId;
                if (updateProcedureId == "") {
                    utility.DisplayMessages("Procedure already exists.", 3);
                    $(procedureTool).find("ul#customFormProceduresList > li#" + procedureId).remove();
                }
                else {
                    $(procedureTool).find("ul#customFormProceduresList > li#" + procedureId).attr('id', updateProcedureId);
                    Clinical_CustomFormsPreview.attachProcedureWithNote(updateProcedureId);
                }
                objDeffered.resolve();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

        return objDeffered;
    },

    saveProcedure_DbCall: function (procedure) {
        var objData = {};
        var objDetail = {};
        var procedureData = [];
        objDetail["CPTCode"] = $(procedure).attr('cptcode');
        objDetail["CPT_DESCRIPTION"] = $(procedure).attr('cptdescription');
        objDetail["ProcedureId"] = $(procedure).attr('id');
        objDetail["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objDetail["NotesId"] = Clinical_ProgressNote.params.NotesId;
        procedureData.push(objDetail);
        objData["procedureDetailModel"] = procedureData;
        objData["CustomFormId"] = Clinical_CustomFormsPreview.params.CustomFormId;
        objData["commandType"] = "save_procedures";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },

    attachProcedureWithNote: function (ProcedureId) {
        Clinical_CustomFormsPreview.attachProcedureWithNotes_DBCall(ProcedureId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    attachProcedureWithNotes_DBCall: function (ProcedureId) {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["ProcedureId"] = ProcedureId;
        objData["commandType"] = "attach_procedures_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");
    },

}