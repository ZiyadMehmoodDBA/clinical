Admin_CCMTemplatePreview = {

    params: [],


    Load: function (params) {
        Admin_CCMTemplatePreview.params = params;
        if (Admin_CCMTemplatePreview.params.PanelID != 'pnlAdminCCMTemplatePreview') {
            Admin_CCMTemplatePreview.params.PanelID = Admin_CCMTemplatePreview.params.PanelID + ' #pnlAdminCCMTemplatePreview';
        } else {
            Admin_CCMTemplatePreview.params.PanelID = 'pnlAdminCCMTemplatePreview';
        }
        $('#TemplateTitle').text(Admin_CCMTemplatePreview.params.TemplateName);

        Admin_CCMTemplatePreview.TemplatePreview(Admin_CCMTemplateDetails.params.TemplateId);
    },


    getControlMarkup: function (control) {
        var questionType = $(control).attr('questiontype');
        var ControlMarkup;
        switch (questionType) {
            case "TextField":
                if ($(control).attr('issingleline') == "true") {
                    ControlMarkup = $(control).find('input:not(:checkbox)');
                } else {
                    ControlMarkup = $(control).find('textarea:not(:checkbox)');
                }
                break;
            case "CheckBox":
                ControlMarkup = $(control).find('#TemplateCheckBoxPreview');
                ControlMarkup = ControlMarkup.removeClass('pull-right col-sm-5');
                break;
            case "YesNo":
                ControlMarkup = $(control).find('#TemplateYesNoPreview');
                break;
            case "SingleSelectDropdown":
                ControlMarkup = $(control).find('#TemplateSingleSelectDropdownList');
                break;
            case "MultipleSelectCombo":
                //if ($(control).find(".questionTitle").length>0) {
                //    if (Admin_CCMTemplatePreview.params.ParentCtrl == "Admin_CCMTemplateDetails") {
                //        $(control).find(".questionTitle").parent().remove();
                //    }
                //}
                ControlMarkup = $(control);
                break;
            case "FractionField":
                ControlMarkup = $(control).find('#TemplateFraction').removeClass('pull-right');
                break;
            case "FreeText":
                ControlMarkup = $(control).find('textarea');
                break;
            case "Image":
                ControlMarkup = $(control).find("#TemplateImage");
                break;
            case "DateField":
                ControlMarkup = $(control).find('input:not(:checkbox)').parent();
                break;
            case "TimeField":
                ControlMarkup = $(control).find('input:not(:checkbox)').parent();
                break;

            default:
        }

        return '<td style="width:50%">' + ControlMarkup[0].outerHTML + '</td>'
    },


    expandCollapseRow: function (obj, qstnId) {
        var icon = $(obj).find('i');
        if (icon.attr('class') == "fa fa-plus") {
            icon.removeClass("fa fa-plus").addClass("fa fa-minus");
            $('#TemplateDetails table').find('tr').filter("[parentqstn='" + qstnId + "']").removeClass('hidden');
        } else {
            icon.removeClass("fa fa-minus").addClass("fa fa-plus");
            $('#TemplateDetails table').find('tr').filter("[parentqstn='" + qstnId + "']").addClass('hidden');
        }
    },

    TemplatePreview: function (templateId, event) {
        Admin_CCMTemplateDetails.TemplateFill_DBCall(templateId).done(function (response) {
            var previewUl = $('#TemplateDetails');
            response = JSON.parse(response);
            if (response.status != false) {
                var sections = response.Sections_JSON;
                var questions = $.grep(response.Questions_JSON, function (el, i) {
                    return el.ParentQuestId == null
                });
                var subQuestions = $.grep(response.Questions_JSON, function (el, i) {
                    return el.ParentQuestId != null
                });
                var table = $('<table/>').addClass('table table-bordered table-condensed table-hover');

                $.each(sections, function (i, e) {
                    var rowHeader = $('<tr><th colspan="2" class="bg-gray">' + e.ShortName + '</th></tr>');
                    table.append(rowHeader);
                    var mendaoryMarkup = "";
                    $.each(questions, function (i, qstn) {
                        mendaoryMarkup = "";
                        if (qstn.SectionId == e.SectionId) {
                            if (qstn.QuestionHTML) {
                                if ($(qstn.QuestionHTML).attr('ismandatory') && $(qstn.QuestionHTML).attr('ismandatory') == "true") {
                                    mendaoryMarkup = '<span class="required">*</span>';
                                }
                            }
                            var row = $('<tr questionid ="' + qstn.QuestionId + '" />');
                            row.append('<td  style="width:50%">' + mendaoryMarkup + qstn.Description.replace('*', '') + '</td>').attr('questionid', qstn.QuestionId);
                            row.append(qstn.QuestionHTML ? Admin_CCMTemplatePreview.getControlMarkup(qstn.QuestionHTML) : "");
                            row.find('#actionDiv').remove();
                            table.append(row);
                        }
                        $.each(subQuestions, function (iii, subQ) {
                            mendaoryMarkup = "";
                            if (qstn.QuestionId == subQ.ParentQuestId && subQ.SectionId == e.SectionId) {
                                if (subQ.QuestionHTML) {
                                    if ($(subQ.QuestionHTML).attr('ismandatory') && $(subQ.QuestionHTML).attr('ismandatory') == "true") {
                                        mendaoryMarkup = '<span class="required">*</span>';
                                    }
                                }
                                var plusIcon = '<a href="#/" onclick="Admin_CCMTemplatePreview.expandCollapseRow(this,' + subQ.ParentQuestId + ');" class="btn btn-link btn-xs"><i class="fa fa-plus"></i></a>';
                                if (table.find('tr').filter("[questionid='" + subQ.ParentQuestId + "']").find('i').attr('class') != "fa fa-plus") {
                                    table.find('tr').filter("[questionid='" + subQ.ParentQuestId + "']").find('td:eq(0)').prepend(plusIcon);
                                }
                                var row = $('<tr/>').addClass('hidden');
                                row.append('<td  style="width:50%"> <div class="pl-xlg">' + mendaoryMarkup + subQ.Description.replace('*', '') + '</div></td>').attr('parentQstn', subQ.ParentQuestId).addClass('subquestion');
                                row.append(subQ.QuestionHTML ? Admin_CCMTemplatePreview.getControlMarkup(subQ.QuestionHTML) : "");
                                row.find('#actionDiv').remove();
                                table.append(row);
                            }
                        });
                    });
                    previewUl.append(table);

                });
                Admin_CCMTemplatePreview.initilizeDatePickers();
                Admin_CCMTemplatePreview.initilizeTimePickers();
                Admin_CCMTemplatePreview.initilizeMultiSelectTemplatePreview();
                if ($(previewUl).find(".questionTitle").length > 0) {
                    if (Admin_CCMTemplatePreview.params.ParentCtrl == "Admin_CCMTemplateDetails") {
                        $(previewUl).find(".questionTitle").parent().remove();
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    setWidthofTable: function () {
        $("#" + Admin_CCMTemplatePreview.params.PanelID + " #TemplateDetails").find("#tblContext").each(function () {
            var table = $(this);
            if (table) {
                cols = $(table.find('tr:first-child')).find('td').length;
                if (cols) {
                    var totalCols = (parseInt(cols) + 1);
                    var widthofTd = Math.floor(100 / totalCols);
                    $(".tblContextTd").css({ 'width': widthofTd + '%' });
                }
            }
        });
    },
    initilizeDatePickers: function () {
        //$("#" + Admin_CCMTemplatePreview.params.PanelID + " #TemplateDetails").find(".dateField").each(function () {
        $(".dateField").each(function () {
            var dateFormat = $(this).attr('dateformat');
            var date_format = 'dd/mm/yyyy';
            if (dateFormat)
                date_format = dateFormat.toLowerCase();
            $(this).datepicker({
                format: date_format,
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
        $(".timeField").each(function () {
            var timeFormat = $(this).attr('timeformat');
            var showMerdn = false;
            if (timeFormat == "24")
                showMerdn = false;
            else if (timeFormat == "12")
                showMerdn = true;
            $(this).timepicker({
                showMeridian: showMerdn,
            }).on('changeTime.timepicker', function (e) {
                //  $(this).timepicker('hideWidget');
            });
            if (this.value != '' && this.value != null && typeof this.value != "undefined") {
                $(this).timepicker("setTime", this.value);
            }
        });
    },
    initilizeMultiSelectTemplatePreview: function () {
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
            var defaultValues = $(e).parents("[id^='toolMultipleSelectCombo_']").attr('defaultselection');
            $(e).val('');

            $(e).multiselect('select', defaultValues.split(','));
            $(e).multiselect("refresh");
        });
        $("[id^='customFormMultipleSelectCombo_").find('select').next().next().remove();
        if (Admin_CCMTemplatePreview.params.ParentCtrl == "Admin_CCMTemplateDetails") {
           // $('#TemplateMultipleSelectComboLabel').parent().remove();
            //$('#actionDiv').remove();
            var dropDown = $('#TemplateDetails').find("[id^='customFormMultipleSelectCombo_']");
            //dropDown.parent().unwrap();
            dropDown.parent().removeClass('col-sm-5 pull-right');
            dropDown.parent().parent().removeClass('panel-body toolcontroldiv col-xs-12');

        }
    },

    UnLoadTab: function () {
        if (Admin_CCMTemplatePreview.params["ParentCtrl"] == "clinicalTabProgressNote") {
            if (Admin_CCMTemplatePreview.params != null && Admin_CCMTemplatePreview.params.ParentCtrl != null) {
                UnloadActionPan(Admin_CCMTemplatePreview.params.ParentCtrl, 'Admin_CCMTemplatePreview', null, "pnlClinicalProgressNote #pnlClinicalTemplates");
            }
            else
                UnloadActionPan(null, 'Admin_CCMTemplatePreview');
        }
        else {
            UnloadActionPan(Admin_CCMTemplatePreview.params.ParentCtrl, 'Admin_CCMTemplatePreview');
        }
        Admin_CCMTemplatePreview.params = [];
    },

    addTemplateToNote: function (event) {
        event.preventDefault();
        //var allGood = Admin_CCMTemplatePreview.getTemplateInfo(Admin_CCMTemplatePreview.params.TemplateId, true);

        //if (allGood) {
        var isValidMS = Admin_CCMTemplatePreview.validateMultiSelectValues();
        var isValidYesNo = Admin_CCMTemplatePreview.validateYesNo();
        var isValidTable = Admin_CCMTemplatePreview.validateTable();
        var isValidSS = Admin_CCMTemplatePreview.validateSingleSelect();
        if (isValidMS && isValidYesNo && isValidTable && isValidSS) {
            Admin_CCMTemplatePreview.getTemplateInfo(Admin_CCMTemplatePreview.params.TemplateId, true);
            Admin_CCMTemplatePreview.UnLoadTab();
            Clinical_Templates.UnLoadTab();
        }
        //} else {
        //    utility.DisplayMessages("Please fill mandatory fields.", 2)
        //}

    },
    validateMultiSelectValues: function () {
        var isValid = false;
        var self = $("#" + Admin_CCMTemplatePreview.params.PanelID);
        var TemplateMultipleSelectCombo = self.find("div[questiontype='MultipleSelectCombo']");
        if (TemplateMultipleSelectCombo.length > 0) {
            TemplateMultipleSelectCombo.each(function () {
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
        var self = $("#" + Admin_CCMTemplatePreview.params.PanelID);
        var TemplateYesNo = self.find("div[questiontype='YesNo']");
        if (TemplateYesNo.length > 0) {
            TemplateYesNo.each(function () {
                var isRequired = $(this).attr('ismandatory');
                if (isRequired == "true") {
                    var IsChecked = $(this).find("input[type='checkbox']").is(":checked");
                    if (IsChecked == true) {
                        isValid = true;
                        $(self).find("#TemplateYesNoPreview").removeAttr("style");
                    }
                    else {
                        isValid = false;
                        $(self).find("#TemplateYesNoPreview").css({ border: '1px solid #cc2724', display: 'inline-block', width: '100%' });
                        utility.DisplayMessages("Please select any option.", 2)
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
        var self = $("#" + Admin_CCMTemplatePreview.params.PanelID);
        var TemplateTable = self.find("div[questiontype='Table']");
        if (TemplateTable.length > 0) {
            TemplateTable.each(function () {
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
        var self = $("#" + Admin_CCMTemplatePreview.params.PanelID);
        var TemplateSingleSelectCombo = self.find("div[questiontype='SingleSelectDropdown']");
        if (TemplateSingleSelectCombo.length > 0) {
            TemplateSingleSelectCombo.each(function () {
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
                        utility.DisplayMessages("Please select any value.", 2)
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
    getTemplateInfo: function (TemplateId, hideAlertMessage) {
        if (TemplateId == null || TemplateId == '') {
            return false;
        }
        // var dfd = new $.Deferred();
        var TemplateSentenceView = Admin_CCMTemplatePreview.getTemplateSentenceView($('#actionPanClinicalProgressNote #TemplateDetails'));
        //if (TemplateSentenceView != '') {
        Admin_CCMTemplatePreview.createTemplateBodyHTML(TemplateSentenceView, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', TemplateId, hideAlertMessage);
        //    return true;
        //} else {
        //    return false;
        //}
        //.resolve('ok');
        //return dfd.promise();
    },

    createTemplateBodyHTML: function (response, NoteHTMLCtrl, TemplateId, hideAlertMessage) {
        var TemplateUniqueId = Clinical_Templates.getTemplateForNotes(Admin_CCMTemplatePreview.params.TemplateName, Admin_CCMTemplatePreview.params.TemplateId, Admin_CCMTemplatePreview.params.TemplateUniqueId);
        if (!TemplateUniqueId) {
            TemplateUniqueId = Admin_CCMTemplatePreview.params.TemplateUniqueId;
        }
        var $mainDivTemplate = $(document.createElement('div'));
        var $SectionBodyTemplate = $(document.createElement('section'));
        $SectionBodyTemplate.attr('id', "Cli_Template_Main" + TemplateUniqueId);
        var $DetailsDiv = $(document.createElement('div'));
        $DetailsDiv.attr('id', "Cli_Template_" + TemplateUniqueId);
        var $ListTemplate = $(document.createElement('ul'));
        $ListTemplate.attr('class', 'list-unstyled')
        $SectionBodyTemplate.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Template_" + TemplateId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Template_Main" + TemplateUniqueId + '"  ><i class="fa fa-times"></i></a></div> ');

        $ListTemplate.append((response == "" ? "" : response));
        $DetailsDiv.append($ListTemplate);
        $SectionBodyTemplate.append($DetailsDiv);
        $mainDivTemplate.append($SectionBodyTemplate);

        if (!TemplateUniqueId) {
            TemplateUniqueId = Admin_CCMTemplatePreview.params.TemplateUniqueId;
        }
        if ($mainDivTemplate.html() != '') {
            Admin_CCMTemplatePreview.updateTemplateHtml($mainDivTemplate.html(), TemplateId, NoteHTMLCtrl, hideAlertMessage, TemplateUniqueId);
        } else {
            Admin_CCMTemplatePreview.updateTemplateHtml('', TemplateId, NoteHTMLCtrl, hideAlertMessage, TemplateUniqueId);
            Clinical_ProgressNote.updateProgressNoteHTML(null, null, hideAlertMessage);
        }
    },
    updateTemplateHtml: function (TemplateHtml, TemplateId, NoteHTMLCtrl, hideAlertMessage, TemplateUniqueId) {
        if (TemplateUniqueId) {
            var Template = $(NoteHTMLCtrl + ' clinical_Template[uniqueid=' + TemplateUniqueId + ']');
            if (Template.attr('uniqueid') == TemplateUniqueId) {
                Template.parent().parent().addClass('initialVisitBody');
                if (TemplateHtml != '') {
                    if (Template.parent().parent().find('section').length > 0) {
                        Template.parent().parent().find('section').remove();
                    }
                    Template.parent().parent().append(TemplateHtml);
                }
            }
        }

        //Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (TemplateHtml != '' && TemplateId != null && TemplateId != '') {
            Admin_CCMTemplatePreview.attachTemplateWithNotes(TemplateId, hideAlertMessage);
        }
    },
    attachTemplateWithNotes: function (TemplateId, hideAlertMessage) {
        Admin_CCMTemplatePreview.attachTemplateWithNotes_DBCall(TemplateId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ProgressNote.updateProgressNoteHTML(null, null, hideAlertMessage);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    attachTemplateWithNotes_DBCall: function (TemplateId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["TemplateId"] = TemplateId;
        objData["commandType"] = "attach_custom_form_with_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "Template", "Template");

    },
    detachTemplateComponent: function (ComponentId, IsUpdate, TemplateComponentRemove, TemplateUniqueId) {
        Admin_CCMTemplatePreview.detachTemplateFromNotes_DBCall(ComponentId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (IsUpdate) {
                    Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
                }
                utility.DisplayMessages(response.Message, 1);
                setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        if (TemplateComponentRemove) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button").each(function (i, e) {
                if ($(e).attr('uniqueid') == TemplateUniqueId) {
                    $(e).remove()
                }
            });
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Template').each(function (i, elem) {
                if ($(elem).attr('uniqueid') == TemplateUniqueId) {
                    $(elem).parent().parent().remove();
                }
            });
        }
    },
    detachTemplate: function (ComponentId, IsUpdate, TemplateComponentRemove, TemplateUniqueId) {
        if (TemplateComponentRemove) {
            var objDeffered = $.Deferred();
            var totalContents = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_Template');
            if (totalContents && totalContents.length > 0) {
                totalContents.each(function (i, e) {
                    var TemplateUniqueId = $(e).attr('uniqueid');
                    var UniqueId = $(e).attr('id');
                    if (TemplateUniqueId) {
                        Admin_CCMTemplatePreview.detachTemplateFromNotes_DBCall(UniqueId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                var section = '#Cli_Template_Main' + UniqueId;
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
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_Templates').closest('li.initialVisitBody').remove();
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button.btn-Template").remove();
                Clinical_ProgressNote.updateProgressNoteHTML(null, null, true);
            });
        }
    },
    detachTemplateFromNotes_DBCall: function (TemplateId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["TemplateId"] = TemplateId;
        objData["commandType"] = "detach_custom_form_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "Template", "Template");
    },

    getTemplateSentenceView: function (TemplateMarkup) {
        var TemplateSentenceView = '';
        $(TemplateMarkup).find('li').each(function (i, elem) {
            var question = $(elem).find('div');
            var questionType = question.attr('questiontype');

            switch (questionType) {
                case "TextField":
                    //if (question.find('span').hasClass('required')) {
                    //    if ($(question).attr('issingleline') == "true") {
                    //        if ($(question).find('input').val().length == 0) {
                    //            $(question).find('input').wrap("<div class='has-feedback has-error'></div>");
                    //            TemplateSentenceView = '';
                    //        } else {
                    //            TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getTextFieldSentenceView(question);
                    //        }
                    //    } else {
                    //        if ($(question).find('textarea').val().length == 0) {
                    //            $(question).find('textarea').wrap("<div class='has-feedback has-error'></div>");
                    //            TemplateSentenceView = '';
                    //        } else {
                    //            TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getTextFieldSentenceView(question);
                    //        }
                    //    }
                    //} else {
                    TemplateSentenceView += "<li style='white-space:pre-line' questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getTextFieldSentenceView(question);
                    //}
                    break;
                case "CheckBox":
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getCheckBoxSentenceView(question);
                    break;
                case "YesNo":
                    //if (question.find('span').hasClass('required')) {
                    //    if (question.find('input:checked').length == 0) {
                    //        question.find('TemplateYesNoPreview').addClass('has-error');
                    //        TemplateSentenceView = '';
                    //    }
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getYesNoSentenceView(question);
                    //}
                    break;
                case "Toggle":
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getToggleSentenceView(question);
                    break;
                case "SingleSelectDropdown":
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getSSDSentenceView(question);
                    break;
                case "MultipleSelectCombo":
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getMSDSentenceView(question);
                    break;
                case "FractionField":
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getFractionSentenceView(question)
                    break;
                case "FreeText":
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getFreeTextSentenceView(question);
                    break;
                case "Image":
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getImageSentenceView(question);
                    break;
                case "DateField":
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getDateFieldSentenceView(question);
                    break;
                case "TimeField":
                    TemplateSentenceView += "<li questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getTimeFieldSentenceView(question);
                    break;
                case "Table":
                    TemplateSentenceView += "<li class='Of-a' questionid='" + question.attr('id') + "'> " + Admin_CCMTemplatePreview.getTableSentenceView(question);
                    break;
                    //Begin Edited By Fahad Malik on 29-11-2016 to fix bug#: EMR-2006
                case "Header":
                    var lbl = $(question).find('label');
                    var getstyle = $(lbl).attr('style');
                    TemplateSentenceView += "<li class='" + $(lbl).attr('class') + "' style='display: inline;" + getstyle + "' questionid='" + question.attr('id') + "'> " + lbl.text();
                    break;
                    //End Edited By Fahad Malik on 29-11-2016 to fix bug#: EMR-2006
                default:
            }
        });
        return TemplateSentenceView;
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
        var selectedCheckboxes = question.find('#TemplateCheckBoxPreview div input').map(function () {
            return this.checked
        });
        selectedCheckboxes = $.makeArray(selectedCheckboxes);
        var isAllFalse = selectedCheckboxes.every(Admin_CCMTemplatePreview.isAllFalse);
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
        var selectedCheckboxes = question.find('#TemplateYesNoPreview div input').map(function () {
            return this.checked
        });
        selectedCheckboxes = $.makeArray(selectedCheckboxes);
        var isAllFalse = selectedCheckboxes.every(Admin_CCMTemplatePreview.isAllFalse);
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
        str += question.find('#TemplateImage').html();
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
        if (!(Admin_CCMTemplatePreview.isTableEmpty(question))) {
            str += question.find('#tblContext')[0].outerHTML;
        }
        return str;
    },

    isAllFalse: function (element, index, array) {
        return element == false;
    },

    fillTemplateOnNotes: function () {
        var Template = $('#actionPanClinicalProgressNote #TemplateDetails');
        var TemplateData = $('#pnlClinicalProgressNote #ProgressnoteHTML' + ' #Cli_Template_' + Admin_CCMTemplatePreview.params.TemplateUniqueId + '').find('li')
        TemplateData.each(function (i, elem) {
            var questionId = $(elem).attr('questionid');
            var question = Template.find('#' + questionId)
            var questionType = $(question).attr('questiontype');
            if (question.length > 0) {
                switch (questionType) {
                    case "TextField":

                        Admin_CCMTemplatePreview.fillTextFieldFromNotes(question, elem);
                        break;
                    case "CheckBox":
                        Admin_CCMTemplatePreview.fillCheckBoxFromNotes(question, elem);
                        break;
                    case "YesNo":
                        Admin_CCMTemplatePreview.fillYesNoFromNotes(question, elem);
                        break;
                    case "Toggle":
                        Admin_CCMTemplatePreview.fillToggleFromNotes(question, elem);
                        break;
                    case "SingleSelectDropdown":
                        Admin_CCMTemplatePreview.fillSSDFromNotes(question, elem);
                        break;
                    case "MultipleSelectCombo":
                        Admin_CCMTemplatePreview.fillMSDFromNotes(question, elem);
                        break;
                    case "FractionField":
                        Admin_CCMTemplatePreview.fillFractionFromNotes(question, elem);
                        break;
                    case "FreeText":
                        Admin_CCMTemplatePreview.fillFreeTextFromNotes(question, elem);
                        break;
                        //case "Image":
                        //    Admin_CCMTemplatePreview.fillFreeTextFromNotes(question);
                        //    break;
                    case "DateField":
                        Admin_CCMTemplatePreview.fillDateFieldFromNotes(question, elem);
                        break;
                    case "TimeField":
                        Admin_CCMTemplatePreview.fillTimeFieldFromNotes(question, elem);
                        break;
                    case "Table":
                        Admin_CCMTemplatePreview.fillTableFromNotes(question, elem);
                        break;
                    default:
                }
            }

        });

    },

    fillTextFieldFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            if ($(question).find('input').length > 0) {
                $(question).find('input').val($(elem).text().split(':')[1].trim());
            } else {
                var fieldText = $(elem).text().split(':')[1].trim();
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
        question.find('#TemplateCheckBoxPreview div input').each(function (i, e) {
            $(e).prop('checked', false);
            $(e).removeAttr('checked');
        });
        question.find('#TemplateCheckBoxPreview div input').each(function (i, e) {
            if (selectedCheckboxes[i]) {
                $(e).prop('checked', true);
                $(e).attr('checked', 'checked');
            }
        });
    },
    fillYesNoFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            if ($(elem).text().split(':')[1].trim() == "Yes") {
                question.find('#chkYes').prop('checked', true).attr('checked', 'checked');
                question.find('#chkNo').prop('checked', false).attr('checked', '');
            } else if ($(elem).text().split(':')[1].trim() == "No") {
                question.find('#chkNo').prop('checked', true).attr('checked', 'checked');
                question.find('#chkYes').prop('checked', false).attr('checked', '');
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
            question.find('#TemplateMultipleSelectCombo select').val('');
            EMRUtility.selectOptionsByCommaSeprateValue(question.find('#TemplateMultipleSelectCombo select'), selectedValues);
            question.find('#TemplateMultipleSelectCombo select').multiselect("refresh");
        }
    },
    fillFractionFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            var fraction = $(elem).text().split(':')[1].trim().split('/');
            var frac1 = fraction[0];
            var frac2 = fraction[1];
            question.find('#txtFractionField1').val(frac1);
            question.find('#txtFractionField2').val(frac2);
        }
    },
    fillFreeTextFromNotes: function (question, elem) {
        if ($(elem).text().trim().length > 0) {
            question.find('textarea').val($(elem).text().split(':')[1].trim());
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
    isTableEmpty: function (question) {
        var isEmpty = question.find('Table td').map(function (e) {
            return $(this).text().length > 0;
        });

        isEmpty = $.makeArray(isEmpty);
        var isAllFalse = isEmpty.every(Admin_CCMTemplatePreview.isAllFalse);
        return isAllFalse;
    },
}