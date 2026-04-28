CCM_HRAssessmentDetail = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        CCM_HRAssessmentDetail.params = params;

        if (CCM_HRAssessmentDetail.params.PanelID != 'pnlCCM_HRAssessmentDetail') {
            CCM_HRAssessmentDetail.params.PanelID = CCM_HRAssessmentDetail.params.PanelID + ' #pnlCCM_HRAssessmentDetail';
        } else {
            CCM_HRAssessmentDetail.params.PanelID = 'pnlCCM_HRAssessmentDetail';
        }

        if (CCM_HRAssessmentDetail.params.HRAssessmentId != null && CCM_HRAssessmentDetail.params.HRAssessmentId > 0) {
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #headerId').text('Update Assessment');
            CCM_HRAssessmentDetail.fillHRAssessment();
        } else {
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #headerId').text('Add New Assessment');
            var self = $('#' + CCM_HRAssessmentDetail.params.PanelID);
            self.loadDropDowns(true).done(function () {
                CCM_HRAssessmentDetail.ShowHideHRAssessmentTemplate(false);
            });
        }
        CCM_HRAssessmentDetail.switchTab('LiAddHRA');        
    },

    fillHRAssessment: function (hRAssessmentId) {
        CCM_HRAssessmentDetail.fillHRAssessment_DbCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var cpLIst = JSON.parse(response.CPList_JSON);
                if (cpLIst != null && cpLIst.length > 0) {
                    $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #ddlTemplateHRAssessment').val(cpLIst[0].HRAssessmentTemplateId);
                    $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #hfHRAssessmentTemplateId').val(cpLIst[0].HRAssessmentTemplateId);
                    $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divHRAssessmentHTML').html(cpLIst[0].HRAssessmentHTML);
                    CCM_HRAssessmentDetail.params.VitalSignsId = cpLIst[0].VitalsId;
                    CCM_HRAssessmentDetail.params.HRAssessmentId = cpLIst[0].HRAssessmentId;
                    CCM_HRAssessmentDetail.params.mode = "Edit";
                    var vitalParams = [];
                    vitalParams["VitalSignsId"] = cpLIst[0].VitalsId;
                    vitalParams["HRAssessmentId"] = cpLIst[0].HRAssessmentId;
                    vitalParams["PanelID"] = CCM_HRAssessmentDetail.params.PanelID;
                    vitalParams["PatientId"] = CCM_HRAssessmentDetail.params.PatientId;
                    vitalParams["EnrollmentInfoId"] = CCM_HRAssessmentDetail.params.EnrollmentInfoId;
                    CCM_Vitals.Load(vitalParams);
                    CCM_HRAssessmentDetail.ShowHideHRAssessmentTemplate(true);
                    CCM_HRAssessmentDetail.initilizeDatePickers();
                    CCM_HRAssessmentDetail.initilizeTimePickers();
                    CCM_HRAssessmentDetail.initilizeMultiSelectTemplatePreview();
                } else {
                    CCM_HRAssessmentDetail.params.mode = "Add";
                    CCM_HRAssessmentDetail.ShowHideHRAssessmentTemplate(false);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    loadHRAssessmentTemplate: function () {
        var selectedTempt = $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #ddlTemplateHRAssessment').val();
        if (selectedTempt != null && selectedTempt != '' && selectedTempt != 0) {
            CCM_HRAssessmentDetail.ShowHideHRAssessmentTemplate(true);
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #hfHRAssessmentTemplateId').val(selectedTempt);
            CCM_HRAssessmentDetail.TemplatePreview(selectedTempt);
        } else {
            CCM_HRAssessmentDetail.ShowHideHRAssessmentTemplate(false);
            utility.DisplayMessages("Please select Template.", 3);
        }
    },

    TemplatePreview: function (templateId, event) {
        CCM_HRAssessmentDetail.TemplateFill_DBCall(templateId).done(function (response) {
            var previewUl = $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divHRAssessmentHTML');
            response = JSON.parse(response);
            if (response.status != false) {
                var sections = response.Sections_JSON;
                var questions = $.grep(response.Questions_JSON, function (el, i) {
                    return el.ParentQuestId == null
                });
                var subQuestions = $.grep(response.Questions_JSON, function (el, i) {
                    return el.ParentQuestId != null
                });
                var table = $('<table/>').addClass('table table-bordered table-striped table-condensed table-hover');

                $.each(sections, function (i, e) {
                    var rowHeader = $('<tr/>');
                    rowHeader.append('<th>' + e.ShortName + '<th/>');
                    table.append(rowHeader);

                    $.each(questions, function (i, qstn) {
                        if (qstn.SectionId == e.SectionId) {
                            var row = $('<tr questionid ="' + qstn.QuestionId + '" />');
                            row.append('<td>' + qstn.Description + '</td>').attr('questionid', qstn.QuestionId);
                            row.append(qstn.QuestionHTML ? CCM_HRAssessmentDetail.getControlMarkup(qstn.QuestionHTML) : "");
                            row.find('#actionDiv').remove();
                            table.append(row);
                        }
                        $.each(subQuestions, function (iii, subQ) {
                            if (qstn.QuestionId == subQ.ParentQuestId && subQ.SectionId == e.SectionId) {
                                var plusIcon = '<a href="#/" onclick="CCM_HRAssessmentDetail.expandCollapseRow(this,' + subQ.ParentQuestId + ');" class="btn btn-link btn-xs"><i class="fa fa-plus"></i></a>';
                                if (table.find('tr').filter("[questionid='" + subQ.ParentQuestId + "']").find('i').attr('class') != "fa fa-plus") {
                                    table.find('tr').filter("[questionid='" + subQ.ParentQuestId + "']").find('td:eq(0)').prepend(plusIcon);
                                }
                                var row = $('<tr/>').addClass('hidden');
                                row.append('<td> <div class="pl-xlg">' + subQ.Description + '</div></td>').attr('parentQstn', subQ.ParentQuestId).addClass('subquestion');
                                row.append(subQ.QuestionHTML ? CCM_HRAssessmentDetail.getControlMarkup(subQ.QuestionHTML) : "");
                                row.find('#actionDiv').remove();
                                table.append(row);
                            }
                        });
                    });
                    previewUl.append(table);

                });
                CCM_HRAssessmentDetail.initilizeDatePickers();
                CCM_HRAssessmentDetail.initilizeTimePickers();
                CCM_HRAssessmentDetail.initilizeMultiSelectTemplatePreview();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    getControlMarkup: function (control) {
        var questionType = $(control).attr('questiontype');
        var ControlMarkup;
        switch (questionType) {
            case "TextField":
                ControlMarkup = $(control).find('input:not(:checkbox)');
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
                ControlMarkup = $(control);
                break;
            case "FractionField":
                ControlMarkup = $(control).find('#TemplateFraction');
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

        return '<td>' + ControlMarkup[0].outerHTML + '</td>'
    },
    expandCollapseRow: function (obj, qstnId) {
        var icon = $(obj).find('i');
        if (icon.attr('class') == "fa fa-plus") {
            icon.removeClass("fa fa-plus").addClass("fa fa-minus");
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divHRAssessmentHTML table').find('tr').filter("[parentqstn='" + qstnId + "']").removeClass('hidden');
        } else {
            icon.removeClass("fa fa-minus").addClass("fa fa-plus");
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divHRAssessmentHTML table').find('tr').filter("[parentqstn='" + qstnId + "']").addClass('hidden');
        }
    },
    initilizeDatePickers: function () {
        //$("#" + CCM_HRAssessmentDetail.params.PanelID + " #divHRAssessmentHTML").find(".dateField").each(function () {
        $('#' + CCM_HRAssessmentDetail.params.PanelID + " .dateField").each(function () {
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
        $('#' + CCM_HRAssessmentDetail.params.PanelID + " .timeField").each(function () {
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
        $('#' + CCM_HRAssessmentDetail.params.PanelID + " [id^='customFormMultipleSelectCombo_").find('select').each(function (i, e) {
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
        });
        $('#' + CCM_HRAssessmentDetail.params.PanelID + " [id^='customFormMultipleSelectCombo_").find('select').next().next().remove();
        if (CCM_HRAssessmentDetail.params.ParentCtrl == "CCM_HRAssessmentDetail") {
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #TemplateMultipleSelectComboLabel').parent().remove();
            //$('#actionDiv').remove();
            var dropDown = $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divHRAssessmentHTML').find("[id^='customFormMultipleSelectCombo_']");
            //dropDown.parent().unwrap();
            dropDown.parent().removeClass('col-sm-5 pull-right');
            dropDown.parent().parent().removeClass('panel-body toolcontroldiv col-xs-12');

        }
    },

    ShowHideHRAssessmentTemplate: function (isHide) {
        if (isHide) {
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divSelectTemplate').hide();
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divHRAssessmentDetail').show();
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #ccm_HRAmodal').removeClass('modal-dialog-md');
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #ccm_HRAmodal').addClass('modal-dialog-full');
        } else {
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divSelectTemplate').show();
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divHRAssessmentDetail').hide();            
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #ccm_HRAmodal').removeClass('modal-dialog-full');
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #ccm_HRAmodal').addClass('modal-dialog-md');
        }
    },

    saveHRAssessment: function () {

        CCM_HRAssessmentDetail.saveHRAssessment_DbCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                CCM_HRAssessmentDetail.params.HRAssessmentId = response.HRAssessmentId;
                utility.DisplayMessages(response.Message, 1);
                CCM_HRAssessment.searchHRAssessment();
                var vitalParams = [];
                vitalParams["VitalSignsId"] = CCM_HRAssessmentDetail.params.VitalSignsId;
                vitalParams["HRAssessmentId"] = CCM_HRAssessmentDetail.params.HRAssessmentId;
                vitalParams["PanelID"] = CCM_HRAssessmentDetail.params.PanelID;
                vitalParams["PatientId"] = CCM_HRAssessmentDetail.params.PatientId;
                vitalParams["EnrollmentInfoId"] = CCM_HRAssessmentDetail.params.EnrollmentInfoId;
                 CCM_HRAssessmentDetail.params.mode = "Edit";
                CCM_Vitals.Load(vitalParams);
                CCM_HRAssessmentDetail.switchTab('LiAddVitals');
                CCM_HRAssessmentDetail.fillHRAssessment();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    getHtml: function (div) {
        div.find("input, select, textarea").each(function () {
            var $this = $(this);
            if ($this.is("[type='radio']") || $this.is("[type='checkbox']")) {
                if ($this.prop("checked")) {
                    $this.attr("checked", "checked");
                }
            } else {
                if ($this.is("select")) {
                    $this.find(":selected").attr("selected", "selected");
                } else {
                    $this.attr("value", $this.val());
                }
            }
        });
        return div.html();
    },
    //-------------------Print & Export--------------------//
    exportToPdf: function () {
        CCM_HRAssessmentDetail.printHRAssessment(false);
    },

    printHRAssessment: function (isPrint) {
        CCM_HRAssessmentDetail.FillDemographic_DbCall(CCM_HRAssessmentDetail.params.PatientId).done(function (response) {
            if (response.status != false) {
                var PatientProfileInfo = JSON.parse(response.DemographicFill_JSON);
                var response = { Header: "" };
                var contents = $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divHRAssessmentHTML').html();
                CCM_HRAssessmentDetail.PrintWithoutCustomHeader(response, PatientProfileInfo, contents, isPrint);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    PrintWithoutCustomHeader: function (response, PatientProfileInfo, contents, isPrint) {
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


        $('#' + CCM_HRAssessmentDetail.params["PanelID"] + ' #PatientInfo #ProviderList').html(

                                   '<li id="PatientProvider">Provider: ' + PatientProfileInfo.Provider + '</li>'
                                    );
        $('#' + CCM_HRAssessmentDetail.params["PanelID"] + ' #PatientInfo #PatientList').html(
                               '<li id="PatientName">' + PatientProfileInfo.FullName + '</li>' +
                               '<li id="PatientAccount"></li>' + PatientProfileInfo.AccountNo + '</li>' +
                                '<li id="PatientAge">' + patientInfo + '</li>' +
                               '<li id="PatientAddress">' + patientAddress + '</li>' +
                               PatientHomePhone +
                               PatientCellPhone +
                               PatientEmail
                                );
        $('#' + CCM_HRAssessmentDetail.params["PanelID"] + ' #PatientInfo #PracticeList').html(
                           '<li>Practice: ' + PatientProfileInfo.Practice + '</li>'

                           );

        CCM_HRAssessmentDetail.getPrintHRAssessmentPDF(response, contents, true, isPrint);
    },

    setPDFcss: function () {
        $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #ulContent td span").css('border', 'none');
        $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #ulContent td :input").css('border', 'none');
        $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #ulContent td :input").css('width', '338px');
    },

    getPrintHRAssessmentPDF: function (response, contents, isWithOutHeaders, isPrint) {

        if ($('#' + CCM_HRAssessmentDetail.params["PanelID"] + ' #printcall:visible').length == 0) {
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + ' #printcall').css('display', '');
        }
        $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #ulContent").html(contents);
        CCM_HRAssessmentDetail.setPDFcss();
        var imgLength = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall img").length;
        var images = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall img");

        var Logo = '';
        if (response.Header != null) {
            var logoHTML = $(response.Header);
            Logo = logoHTML.find('img').prop('src');
        } else if (response.ReportHeaderInfo != null) {
            var logoHTML = $(response.ReportHeaderInfo);
            Logo = logoHTML.find('img').prop('src');
        }
        var HeaderLogo = '';
        if (Logo != null && Logo != '') {
            $.each(images, function (i, item) {
                if (item.src == Logo) {
                    HeaderLogo = item.src;
                }

            });
        } else {
            if (images != null && images.length > 0) {
                HeaderLogo = images[0].src;
            }
        }


        var FooterText = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall footer").text().split('Generated by: ').join('');

        if (HeaderLogo !== null && HeaderLogo !== "") {
            var CustomLogo = '<img id="ClinicalReportsHeaderLogo" src="' + HeaderLogo + '   " class="img-responsive" style="width: 100px;">';
            var insideHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', HeaderLogo);
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html(PageTemp);
        }
        else {
            var defaultLogo = 'content/images/SHS-nav-logo-small-100.png';
            var insideHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsHeaderLogo').prop('src', defaultLogo);
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html(PageTemp);
        }

        // ----- footer 
        if (FooterText !== null && FooterText !== "") {
            var insideHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsFooter').text(FooterText);
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html(PageTemp);

        }
        else {
            var footerText = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall  footer").text().split('Generated by: ').join('');
            var insideHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            if (footerText == null || footerText == '') {
                footerText = 'MDVISION PM EMR'
            }
            $(PageTemp).find('#ClinicalReportsFooter').text(footerText);
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html(PageTemp);

        }
        //PracticeDiv start
        if ($('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PracticeList").length > 0) {
            var practiceHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PracticeList").length == 1 ? $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PracticeList")[0].outerHTML : $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PracticeList")[1].outerHTML;
            var insideHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#PracticeDiv').html(practiceHTML.split('#').join('\\#'));
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html(PageTemp);
        }
        //PracticeDiv end
        //PatientList start
        if ($('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PatientList").length > 0) {
            var patientHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PatientList").length == 1 ? $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PatientList")[0].outerHTML : $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PatientList")[1].outerHTML;
            var insideHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#PatientDiv').html(patientHTML.split('#').join('\\#'));
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html(PageTemp);
        }
        //PatientList end
        //ProviderDiv start
        if ($('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #ProviderList").length > 0) {
            var providerHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #ProviderList").length == 1 ? $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #ProviderList")[0].outerHTML : $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #ProviderList")[1].outerHTML;
            var insideHTML = $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html();;
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ProviderDiv').html(providerHTML.split('#').join('\\#'));
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html(PageTemp);
        }
        var topMargin = 0
        if (isWithOutHeaders) {
            topMargin = 45 + 3;
        } else {
            var height = 0;
            var ProvHeight = $(PageTemp).find('#ProviderDiv ul li:not(:empty)').length * 6;
            var pracHeight = $(PageTemp).find('#PracticeDiv ul li:not(:empty)').length * 6;
            var patHeight = $(PageTemp).find('#PatientDiv ul li:not(:empty)').length * 6;
            if (patHeight > ProvHeight) {
                height += pracHeight + (patHeight / 2) + $(PageTemp).find('#PatientDiv ul li:not(:empty)').length - 2;
            } else {
                height += pracHeight + (ProvHeight / 2) + $(PageTemp).find('#ProviderDiv ul li:not(:empty)').length - 2;
            }
            topMargin = height <= 18 ? 18 : height + 3;
            if (height <= 18) {
                topMargin = height <= 18 ? 18 + 15 : height + 3;
            }
            topMargin += 10;
        }
        $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PatientInfo").hide();
        if ($('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template .blueBorderPrint").length >= 1) {
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall .form-group").hide();
            $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall footer").hide();
        }
        // --------------------------------------------- start Download functionality--------------------------------------------------------------
        kendo.drawing.drawDOM('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            // margin: "2cm 3cm ",
            margin: {
                left: "10mm",
                top: topMargin + "mm",
                right: "10mm",
                bottom: "30mm"
            },
            template: $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #page-template").html()
        }).then(function (group) {
            if (isPrint) {
                kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                    var params = [];
                    params["PrintPDFDataURL"] = dataURL.split('data:application/pdf;base64,').join('');
                    params["PreviewPdf"] = true;
                    utility.PDFViewer(params["PrintPDFDataURL"], true, '#pnlCCM_HRAssessmentDetail #PreviewReportPrint', false, true);
                    $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PatientInfo").hide();
                    $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall footer").hide();
                    $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall").hide();
                });
            } else {
                kendo.drawing.pdf.saveAs(group, "HRAssessment.pdf");
                $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall #PatientInfo").hide();
                $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall footer").hide();
                $('#' + CCM_HRAssessmentDetail.params["PanelID"] + " #printcall").hide();
            }
        });

        // ------------------------------------- End Download functionality--------------------------------------
    },

    FillDemographic_DbCall: function (PatientID) {

        var objData = new Object();
        objData["PatientID"] = PatientID;
        objData["CommandType"] = "fill_patient_demographic";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "PatientDemographic");
    },
    //-------------------Print & Export--------------------//

    //---------------Vitals------//

    switchTab: function (TabID) {

        if (TabID == 'LiAddHRA' && $("#" + CCM_HRAssessmentDetail.params["PanelID"] + ' #frmCCM_HRAssessmentDetail #addHRA').is(':visible') == false) {
           
            if (CCM_HRAssessmentDetail.params.HRAssessmentId != null && CCM_HRAssessmentDetail.params.HRAssessmentId > 0) {
                $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #LiAddVitals').removeClass("disableAll");
            } else {
                $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #LiAddVitals').removeClass("active");
                $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #LiAddVitals').addClass('disableAll');
            }

            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #LiAddHRA a').tab('show');
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #LiAddHRA').addClass("active");
        } else if (TabID == 'LiAddVitals' && $("#" + CCM_HRAssessmentDetail.params["PanelID"] + ' #frmCCM_HRAssessmentDetail #addVitals').is(':visible') == false) {
            $("#" + CCM_HRAssessmentDetail.params["PanelID"] + ' #frmCCM_HRAssessmentDetail #addVitals').load();
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #LiAddVitals a').tab('show');
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #LiAddVitals').removeClass("disableAll");
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #LiAddHRA').removeClass("active");
            $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #LiAddVitals').addClass("active");
        }
    },

    
    //------------vitals-------//

    fillHRAssessment_DbCall: function () {
        var objData = {};
        objData["HRAssessmentId"] = CCM_HRAssessmentDetail.params.HRAssessmentId;
        objData["commandType"] = "FILL_CCM_HR_ASSESSMENT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Patient_Hub", "HRAssessment");
    },

    saveHRAssessment_DbCall: function () {

        var objData = {};
        objData["HRAssessmentTemplateId"] = $('#' + CCM_HRAssessmentDetail.params.PanelID + ' #hfHRAssessmentTemplateId').val();
        objData["HRAssessmentHTML"] = CCM_HRAssessmentDetail.getHtml($('#' + CCM_HRAssessmentDetail.params.PanelID + ' #divHRAssessmentHTML'));
        objData["EnrollmentInfoId"] = CCM_HRAssessmentDetail.params.EnrollmentInfoId;
        if (CCM_HRAssessmentDetail.params.mode == "Edit") {
            objData["commandType"] = "UPDATE_CCM_HR_ASSESSMENT";
            objData["HRAssessmentId"] = CCM_HRAssessmentDetail.params.HRAssessmentId;
        } else {
            objData["commandType"] = "SAVE_CCM_HR_ASSESSMENT";
            objData["HRAssessmentId"] = -1;
        }
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Patient_Hub", "HRAssessment");
    },

    TemplateFill_DBCall: function (templateId) {
        var objData = new Object();
        objData["commandType"] = "FILL_CCM_TEMPLATE";
        objData["TemplateId"] = templateId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CCMTemplate", "CCMTemplate");
    },

    Unload: function () {
        CCM_Vitals.bIsFirstLoad = true;
        if (CCM_HRAssessmentDetail.params["FromAdmin"] == "0") {
            if (CCM_HRAssessmentDetail.params != null && CCM_HRAssessmentDetail.params.ParentCtrl != null) {
                UnloadActionPan(CCM_HRAssessmentDetail.params.ParentCtrl, 'CCM_HRAssessmentDetail');
            }
            else
                UnloadActionPan(null, 'CCM_HRAssessmentDetail');
        }
        else {
            RemoveAdminTab();
        }
    },
}