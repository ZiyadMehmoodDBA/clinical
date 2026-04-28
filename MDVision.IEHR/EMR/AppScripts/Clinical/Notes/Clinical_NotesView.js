Clinical_NotesView = {
    bIsFirstLoad: true,
    params: [],
    pdf: "",
    bProcedureDiagnosIsMissing: true,
    arrCQMReasoning: [],
    arrVBPReasoning: [],
    patName: "",
    patDOB: "",
    ProviderName: "",
    DOS: "",
    ExcludeImageIds: [],
    PEDocExists: false,
    Load: function (params) {
        Clinical_NotesView.params = params;
        if (Clinical_NotesView.params == null) {
            Clinical_NotesView.params = [];
        }
        if (Clinical_NotesView.params.PanelID != "Clinical_NotesView") {

            Clinical_NotesView.params["PanelID"] = "Clinical_NotesView";
        }
        // adnan maqbool, for EMR-1853
        if (Clinical_NotesView.params != null && Clinical_NotesView.params.FromProgressnote == "1") {
            Clinical_NotesView.params["PanelID"] = "Clinical_NotesView";
        }
        if (Clinical_NotesView.bIsFirstLoad) {
            Clinical_NotesView.bIsFirstLoad = false;
            var self = $('#' + Clinical_NotesView.params["PanelID"]);

            self.loadDropDowns(true).done(function () {


                if (Clinical_NotesView.params.FromProgressnote != "1")
                    $.when(Clinical_NotesView.NotesPreview(Clinical_NotesView.params.NotesId, Clinical_NotesView.params.PatientId, Clinical_NotesView.params.ProviderId)).then(function () {
                        if (Clinical_NotesView.params.RefSearch == "SignedSearch" && Clinical_Notes.params.HasUnSignPermission) {
                            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonUnsign").removeClass('hidden');
                        } else {
                            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonUnsign").addClass('hidden');
                        }
                        if (Clinical_NotesView.params.Grid != null && typeof Clinical_NotesView.params.Grid != typeof undefined) {
                            if (Clinical_NotesView.params.Grid == "ModifiedNote") {
                                Clinical_NotesView.HideShowButtonsForModifiedNotes(true);
                            }
                            else {
                                Clinical_NotesView.HideShowButtonsForModifiedNotes(false);
                            }
                        }
                        else {
                            Clinical_NotesView.HideShowButtonsForModifiedNotes(false);
                        }

                        if (Clinical_NotesView.params["IsFromDocument"]) {
                            Clinical_NotesView.HideShowButtonForPatientDocument();
                        }
                        /* BackgroundLoaderShow(true);
                        Clinical_NotesView.getPrintnotePDF(false).done(function () {
                            BackgroundLoaderShow(false);

                         });*/

                    });
                Clinical_NotesView.LoadNotesAccess();
            });
        }
        Clinical_ProgressNote.ShowHideComponetsHeaders();

        //Clinical_Notes.checkSignRights(Clinical_NotesView.params.PanelID, 'noteButtonSign');
        Clinical_Notes.checkCoSignRights(Clinical_NotesView.params.PanelID, 'noteButtonCoSign');
        $("#" + Clinical_NotesView.params.PanelID + " input[type=checkbox][name$=Access]").click(function (event) {
            Clinical_NotesView.ToggleAccess(event);
        });
    },
    HideShowButtonsForModifiedNotes: function (FromModifiedNotes) {
        if (FromModifiedNotes) {
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonEdit").hide();
            //$("#" + Clinical_NotesView.params.PanelID + " #noteButtonSign").hide();
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonCoSign").hide();
            $("#" + Clinical_NotesView.params.PanelID + " #btnBillingInfo").hide();
            $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").hide();
            $("#" + Clinical_NotesView.params.PanelID + " #SyndromicSurveillance").hide();
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonAmendment").show();
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonAmendment").attr('disabled', false);
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonPrint").show();
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonPrint").attr('disabled', false);
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonFax").show();
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonFax").attr('disabled', false);
            $("#" + Clinical_NotesView.params.PanelID + " #noteChkReviewed").show();
        }
        else {
            $("#" + Clinical_NotesView.params.PanelID + " #noteChkReviewed").hide();
        }
    },
    MarkAsReviewedChange: function (obj) {
        if ($(obj).prop("checked") == true) {
            utility.myConfirm('43', function () {
                DashBoard.ModifiedNoteReviewed_DBCALL(Clinical_NotesView.params.NotesId, 1).done(function (response) {
                    if (response.status != false) {
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }, function () {

                $(obj).prop("checked", false);
            },
                   '1'
               );
        }
        else {
            utility.myConfirm('44', function () {
                DashBoard.ModifiedNoteReviewed_DBCALL(Clinical_NotesView.params.NotesId, 0).done(function (response) {
                    if (response.status != false) {
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }, function () {
                $(obj).prop("checked", true);
            },
                   '1'
               );
        }
    },
    NoteUnSign: function () {
        var visDate = new Date(Clinical_NotesView.params.VisitDateWithoutTime);
        var cutOffDate = new Date('05/09/2017');
        if (visDate < cutOffDate) {
            utility.DisplayMessages("Provider Note created before May 09, 2017 cannot be UnSigned!", 2);

        } else {
            utility.myConfirm('42', function () {
                Clinical_NotesView.unsignNote_DBCall().done(function (response) {
                    if (response != null) {
                        response = JSON.parse(response);
                    }
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        $("#" + Clinical_NotesView.params.PanelID + " #noteButtonUnsign").addClass('disableAll');
                        Clinical_Notes.NotesSignedSearch(0, null, null, "Signed");
                    } else {
                        utility.DisplayMessages(response.Message, 3);
                    }


                });
            }, function () {

            });
        }
    },
    unsignNote_DBCall: function () {
        var objData = {};
        objData["NotesID"] = Clinical_NotesView.params.NotesId;
        objData["commandType"] = "unsign_note";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    NotesPreview: function (NotesId, PatientId, ProviderId, AmendfromProgress, HideComponentPopup, isComponentSelect, ComponentSelection, appendSign) {
        if (PatientId == null) {
            PatientId = Clinical_NotesView.params.PatientId;
        }
        if (ProviderId == null) {
            ProviderId = Clinical_NotesView.params.ProviderId;
        }
        if (Clinical_NotesView.params.HasUnSignPermission == true) {
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonUnsign").removeClass('hidden');
        } else {
            $("#" + Clinical_NotesView.params.PanelID + " #noteButtonUnsign").addClass('hidden');
        }
        // adnan maqbool, for EMR-1853
        var dfd = new $.Deferred();
        var dfds = [];
        var sectDefer = $.Deferred();
        var compDefer = $.Deferred();
        CacheManager.BindCodes('GetNoteComponents', false).done(function () {
            compDefer.resolve('ok');
        });
        CacheManager.BindCodes('GetNoteSections', false).done(function () {
            sectDefer.resolve('ok');
        });
        $.when(compDefer, sectDefer).then(function () {
            Clinical_NotesView.PreviewNotes(NotesId, PatientId, ProviderId).done(function (response) {
                if (response.status != false) {
                    Clinical_NotesView.patName = response.PatientName;
                    Clinical_NotesView.patDOB = response.PatienDOB;
                    Clinical_NotesView.ProviderName = response.ProviderName;
                    Clinical_NotesView.DOS = response.DOS;
                    Clinical_NotesView.patDOB = Clinical_NotesView.patDOB.replace('12:00AM', '');
                    Clinical_NotesView.DOS = Clinical_NotesView.DOS.replace('12:00AM', '');
                    if (Clinical_NotesView.params.Grid != null && typeof Clinical_NotesView.params.Grid != typeof undefined) {
                        if (Clinical_NotesView.params.Grid == "ModifiedNote") {
                            $("#" + Clinical_NotesView.params.PanelID + " #chkReviewed").prop("checked", response.IsReviewed);
                        }
                    }
                    var objHeader = $(response.ReportHeaderInfo);
                    $(objHeader).find('li').each(function () {
                        if ($(this).text().indexOf('DOB') > -1) {
                            $(this).text($(this).text().replace('12:00AM', ''))
                        }
                    });
                    response.ReportHeaderInfo = $(objHeader).html();
                    var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);

                    // Adding Header Footer to Report, If Selected provider of patient has any Report Header | Change Implmeneted by Azhar Shahzad on Aug08/11/016
                    if (response.ReportHeaderInfo == null || response.ReportHeaderInfo == '') {
                        //Start//13/07/2016//Ahmad Raza//Logic to impliment DR Hajjar's FeedBack after Demo on Notes Preview
                        var patientData = JSON.parse(response.NoteHeaderPatientData);
                        var providerData = JSON.parse(response.NoteHeaderProviderData);
                        var practiceData = JSON.parse(response.NoteHeaderPracticeData);
                        if (patientData.length > 0) {
                            var patientAccount = patientData[0].AccountNumber != "" ? "Acc. #: " + patientData[0].AccountNumber : "";
                            var patientCell = patientData[0].CellNo != "" ? "Ph: " + patientData[0].CellNo : "";
                            var patientName = (patientData[0].FirstName != "" ? patientData[0].FirstName : "") + (patientData[0].LastName != "" ? " " + patientData[0].LastName : "")
                            var age = (patientData[0].Age != "" ? patientData[0].Age + " Y," : "") + " " + patientData[0].Gender + ", DOB: " + utility.RemoveTimeFromDate(null, patientData[0].DOB);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PatientName").html(patientName);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PatientAge").html(age);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PatientAccount").html(patientAccount);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PatientPhone").html(patientCell);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PatientAddress").html(patientData[0].Address1);
                        }
                        if (providerData.length > 0) {
                            var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "")
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #ProviderName").html(providerName);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #Speciality").html(providerData[0].SpecialtyName);
                        }
                        if (practiceData.length > 0) {
                            var city = practiceData[0].City;
                            city += practiceData[0].State != "" ? ", " + practiceData[0].State : "";
                            city += practiceData[0].ZIPCode != "" ? ", " + practiceData[0].ZIPCode : "";
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PracticeName").html(practiceData[0].ShortName);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PracticeAddress").html(practiceData[0].Address);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PracticeCity").html(city);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PracticePhone").html(practiceData[0].PhoneNo);
                        }
                        if (NotesLoad_JSON.length > 0) {
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #NoteDateTime").html(utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate) + " " + NotesLoad_JSON[0].VisitTime);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #VisitReason").html(NotesLoad_JSON[0].VisitReason);

                            Clinical_NotesView.params.BillingInfoId = NotesLoad_JSON[0].BillingInfoId;
                            Clinical_NotesView.params.NoteStatus = NotesLoad_JSON[0].NoteStatus;
                        }


                        //End//13/07/2016//Ahmad Raza//Logic to impliment DR Hajjar's FeedBack after Demo on Notes Preview
                    } else {
                        if (NotesLoad_JSON.length > 0) {
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #NoteDateTime").html(utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate) + " " + NotesLoad_JSON[0].VisitTime);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #VisitReason").html(NotesLoad_JSON[0].VisitReason);
                            Clinical_NotesView.params.BillingInfoId = NotesLoad_JSON[0].BillingInfoId;
                            Clinical_NotesView.params.NoteStatus = NotesLoad_JSON[0].NoteStatus;
                        }
                        $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #printcall > div:not(#PatientInfo)").remove()
                        $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #printcall header").remove();
                        $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #printcall").prepend(response.ReportHeaderInfo);
                        $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #PatientInfo").hide();
                    }

                    if (NotesLoad_JSON.length > 0) {
                        Clinical_NotesView.params.NotesId = NotesLoad_JSON[0].NotesId;
                        Clinical_NotesView.params.VisitId = NotesLoad_JSON[0].VisitId;
                        Clinical_NotesView.params.VisitDate = NotesLoad_JSON[0].VisitDate;
                        params["BillingInfoId"] = parseInt($('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val());
                        params["VisitDate"] = Clinical_ProgressNote.params.VisitDateForFollowUp;
                        params["PatientId"] = Clinical_ProgressNote.params.patientID;
                        params["ProviderId"] = Clinical_ProgressNote.params["CurrentNotesProviderId"];
                        params["PatientTypeId"] = Clinical_ProgressNote.params.PatientTypeId;
                        params["AppointmentDate"] = $('#' + Clinical_ProgressNote.params.PanelID + ' #txtLinkedAppointment').val();
                        //Clinical_ProgressNote.params.PatchBillingInfoId = parseInt($('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val());

                        $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #NoteDateTime").html(utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate) + " " + NotesLoad_JSON[0].VisitTime);
                        $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #VisitReason").html(NotesLoad_JSON[0].VisitReason);
                        Clinical_NotesView.params.BillingInfoId = NotesLoad_JSON[0].BillingInfoId;
                        Clinical_NotesView.params.NoteStatus = NotesLoad_JSON[0].NoteStatus;

                    }
                    if (NotesLoad_JSON.length > 0 && response.NoteComponentListFill_JSON) {

                        $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').empty();
                        $.each(NoteSections, function (i, itm) {
                            if (NotesLoad_JSON[0].TemplateName) {
                                if (itm.value == "Progress")
                                    $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').append(itm.SectionMarkup);
                            }
                            else
                                $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').append(itm.SectionMarkup);
                        });
                        var isBrowserIE = providerDetail.GetIEVersion() > 0;
                        if (response.NoteComponentListFill_JSON.length > 0) {
                            var temp = response.NoteComponentListFill_JSON;

                            //PRD-433 To remove order set component from print(default/classic) preview note soaptext, when user signs the progress note
                            if (NotesLoad_JSON[0].NoteStatus == 'Signed')
                                temp = $.grep(response.NoteComponentListFill_JSON, function (item_) { return (item_.ComponentName != 'Order Sets') });

                            $.each(temp, function (i, item) {
                                if (item.SOAPText.indexOf('Info') > -1) {
                                    item.SOAPText = item.SOAPText.split('(Info)').join('');
                                }
                                //if (item.ComponentName != "Order Sets1") {
                                if (item.SectionName && item.SectionName != "PhoneEncounterData")
                                    $('#' + Clinical_NotesView.params["PanelID"] + " #printcall #ulContent #" + item.SectionName + "NoteComponentList").append(isBrowserIE ? item.SOAPText.replace("System.Byte[]", "image/gif") : item.SOAPText);
                                else if (item.SectionName == "PhoneEncounterData")
                                    $('#' + Clinical_NotesView.params["PanelID"] + " #printcall #ulContent ").prepend(isBrowserIE ? item.SOAPText.replace("System.Byte[]", "image/gif") : item.SOAPText);
                                else {
                                    $('#' + Clinical_NotesView.params["PanelID"] + " #printcall #ulContent ").append(isBrowserIE ? item.SOAPText.replace("System.Byte[]", "image/gif") : item.SOAPText);
                                }
                                //}
                            });

                        } else {
                            $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').html(response.NotesHTML);
                        }
                        if ($("#" + Clinical_NotesView.params["PanelID"] + " #printcall #ulContent #PhoneEncounterData").length > 1) {
                            var encounterMarkup = $("#" + Clinical_NotesView.params["PanelID"] + " #printcall #ulContent #PhoneEncounterData")[0].outerHTML;
                            $("#" + Clinical_NotesView.params["PanelID"] + " #printcall #ulContent #PhoneEncounterData").remove();
                            $("#" + Clinical_NotesView.params["PanelID"] + " #printcall #ulContent").prepend(encounterMarkup);
                        }
                        var id = $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent');
                        if (globalAppdata["NoteFontSize"] == "10") {
                            id.removeClass("font12 font14");
                        } else if (globalAppdata["NoteFontSize"] == "12") {
                            id.removeClass("font14");
                            id.addClass("font12");
                        } else if (globalAppdata["NoteFontSize"] == "14") {
                            id.removeClass("font12");
                            id.addClass("font14");
                        }

                        Clinical_NotesView.CheckBoxes = [];
                        $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').find('li.initialVisitBody').each(function () {
                            if (!$(this).hasClass('hidden')) {
                                if ($(this).find('header + section').length == 0) {
                                    if ($(this).closest('li').find('section').length == 0) {
                                        $(this).hide();
                                    }
                                    else {
                                        if ($(this).find('header').text().indexOf("eSuperbill") == -1 && $(this).find('header').text().indexOf("Images") == -1) {
                                            if ($(this).hasClass("CustomFormsComponent"))
                                                Clinical_NotesView.CheckBoxes.push("Custom Forms");
                                            else
                                                Clinical_NotesView.CheckBoxes.push($(this).find('header:first').text());
                                        }
                                    }
                                } else {
                                    if ($(this).find('header').text().indexOf("eSuperbill") == -1 && $(this).find('header').text().indexOf("Images") == -1) {
                                        if ($(this).hasClass("CustomFormsComponent"))
                                            Clinical_NotesView.CheckBoxes.push("Custom Forms");
                                        else {
                                            if ($(this).find('header').attr('id') != "PhysicalExamSoapSystems") {

                                                Clinical_NotesView.CheckBoxes.push($(this).find('header:first').text());
                                            }
                                        }
                                    }
                                }
                            }
                        });
                        Clinical_NotesView.CheckBoxes = $.unique(Clinical_NotesView.CheckBoxes);
                        //$('#' + Clinical_NotesView.params["PanelID"] + ' #ulContent').find('li.initialVisitBody header + section')

                        $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').find('li.sopTextEditable.defaultli').remove();
                        $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent ul').each(function () {
                            if ($(this).find('li').length == 0) {
                                $(this).css('min-height', '0px');
                                $(this).css('padding', '0px');
                            }
                        });

                        if ($('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').find('section[id*=Cli_Problems_Main]').length > 0 && $('#' + Clinical_NotesView.params["PanelID"] + ' #ulContent').find('section[id*=Cli_Procedures_Main]').length > 0) {
                            Clinical_NotesView.bProcedureDiagnosIsMissing = false;
                        } else {
                            Clinical_NotesView.bProcedureDiagnosIsMissing = true;
                        }

                        var responseIsCPTExsistsInEsupperbill;
                        $.when(responseIsCPTExsistsInEsupperbill = Clinical_ProgressNote.IsCPTExsistsInEsupperbill(Clinical_NotesView.params.NotesId)).then(function () {
                            if (responseIsCPTExsistsInEsupperbill.response != "-1") {
                                if (($('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').find('section[id*=Cli_Problems_Main]').length > 0 && $('#' + Clinical_NotesView.params["PanelID"] + ' #ulContent').find('section[id*=Cli_Procedures_Main]').length > 0) || (responseIsCPTExsistsInEsupperbill.response == "1")) {
                                    Clinical_NotesView.bProcedureDiagnosIsMissing = false;
                                } else {
                                    Clinical_NotesView.bProcedureDiagnosIsMissing = true;
                                }
                            }
                        });

                        $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent .placeholder-free-text').removeClass('placeholder-free-text');
                        if ($('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').find('.PatientEducationComponent').length > 0) {
                            var PEdu = $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').find('.PatientEducationComponent').find("section[id^='Cli_PatientEducation_Main']").length;
                            if (PEdu > 0) {
                                Clinical_NotesView.PEDocExists = true;
                            }
                            else {
                                Clinical_NotesView.PEDocExists = false;
                            }
                        }
                        else {
                            Clinical_NotesView.PEDocExists = false;
                        }
                        Clinical_NotesView.ShowHideComponetsHeaders();
                    }
                    var count_ = 0;
                    var total_ = $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').find("#coSignedByProvider li[id='coSignedBy']").length;
                    if (AmendfromProgress == "AmendfromProgress") {
                        //$('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').append("<br>" + $('<div/>').html(HTMLNotes).find("#AmendmentSection li:last").html());
                    }

                    $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').find('ul.initialVisit').each(function () {
                        if ($(this).find('li.initialVisitBody').length == 0) {
                            $(this).prev('h4').hide();
                        }
                    });


                    var dfd1 = $.Deferred();
                    var providerSign = function () {
                        if (total_ > 0) {
                            $('#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent').find("#coSignedByProvider li[id='coSignedBy']").each(function () {
                                var CoSignedProviderId = $(this).attr("cosignedproviderid");
                                var $coSignedBy = $(this);
                                if (CoSignedProviderId) {
                                    providerDetail.FillProvider(CoSignedProviderId, 0).done(function (response) {
                                        if (response.status != false) {
                                            if ($('div.CoSignComponent').length > 0) {
                                                $('div.CoSignComponent')[0].remove();
                                            }

                                            count_++;
                                            var provider_detailCoS = JSON.parse(response.ProviderFill_JSON);
                                            var eSignature_image_Src = provider_detailCoS.imgeSignature;
                                            var Is_eSignatured = provider_detailCoS.chkIs_eSignatured && provider_detailCoS.chkIs_eSignatured != "" ? JSON.parse(provider_detailCoS.chkIs_eSignatured.toLowerCase()) : false;
                                            if (eSignature_image_Src != "" && Is_eSignatured) {
                                                var isBrowserIE = providerDetail.GetIEVersion() > 0;
                                                if (isBrowserIE) {
                                                    eSignature_image_Src = eSignature_image_Src.replace("System.Byte[]", "image/gif");
                                                }

                                                var imgeCoSignatureHtml = '<li><div style="max-height:350px; overflow-y:auto;margin-top:15px;" >' +
                                                                            '<img id="img_eSignatureCoS_ProgressNotes" src="' + eSignature_image_Src + '" ' +
                                                                                 'alt="" style="height: 125px; width: 315px;border:none;" ' +
                                                                                 'class="img-responsive img-center mt-lg img-thumbnail"/>'
                                                '</div></li>';
                                                $(imgeCoSignatureHtml).insertAfter($($coSignedBy));


                                                //if ($('#' + Clinical_NotesView.params["PanelID"] + ' #ulContent').find(".CosignSignature").length > 1) {
                                                //    $('#' + Clinical_NotesView.params["PanelID"] + ' #ulContent').find(".CosignSignature").hide();
                                                // }
                                            }

                                        }
                                        else
                                            count_++;

                                        if (count_ >= total_)
                                            dfd1.resolve();


                                    });


                                }

                            });

                        }
                        else {
                            dfd1.resolve();

                        }
                        return dfd1.promise();
                    }


                    providerSign().done(function () {

                        //$('#' + Clinical_NotesView.params.PanelID + ' #ProgressNoteComponentList img').addClass('img-responsive');

                        var widthInit = 0;
                        $('#' + Clinical_NotesView.params.PanelID + '  ul.pull-right').each(function () {
                            if ($(this).width() > widthInit) {
                                widthInit = $(this).width();
                            }
                        })
                        if (widthInit > 0)
                            $('#' + Clinical_NotesView.params.PanelID + '  ul.pull-right').width(widthInit);
                        if (response.ReportFooterInfo != null && response.ReportFooterInfo != '') {
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #printcall footer").remove();
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #printcall").append(response.ReportFooterInfo);
                        }
                        if (response.NoteStatus == "Signed" && $('#frmNotesView').length > 0) {
                            //$('#' + Clinical_NotesView.params["PanelID"] + ' #noteButtonEdit, #noteButtonSign').attr('disabled', true);
                            $('#' + Clinical_NotesView.params["PanelID"] + ' #noteButtonEdit').attr('disabled', true);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #noteButtonCoSign").attr('disabled', false);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #noteButtonAmendment").attr('disabled', false);
                            //$('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #btnBillingInfo").attr('disabled', false);
                            $('#' + Clinical_NotesView.params["PanelID"] + " section[id*='Cli_BillingInfo']").parent().css("display", "none");
                            $('#' + Clinical_NotesView.params["PanelID"] + " #clinicalMenu_BillingInfo").css("display", "none");
                            if (NotesLoad_JSON[0].IsNonBilable == "True") {
                                $('#' + Clinical_NotesView.params["PanelID"] + ' #lblNoteBillStatus').parent().removeClass("hidden");
                                $('#' + Clinical_NotesView.params["PanelID"] + ' #lblNoteBillStatus').text("Non-Billable Visit.");
                            }
                            else {
                                $('#' + Clinical_NotesView.params["PanelID"] + ' #lblNoteBillStatus').parent().addClass("hidden");
                                $('#' + Clinical_NotesView.params["PanelID"] + ' #lblNoteBillStatus').text("");
                            }
                            if (parseInt(Clinical_NotesView.params.BillingInfoId) > 0) {
                                if (Clinical_NotesView.params.ParentCtrl == "EncounterChargeCapture" || Clinical_NotesView.params.ParentCtrl == "Bill_ChargeSearch") {
                                    $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                    $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").addClass("disableAll");
                                    // $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').addClass('disableAll');
                                }
                                else if (Clinical_NotesView.params.ParentCtrl == "clinicalTabProgressNote") {
                                    if (parseInt(Clinical_NotesView.params.BillingInfoId) > 0 && NotesLoad_JSON[0].IsNonBilable != "True") {
                                        $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                                        $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
                                    }
                                    else {
                                        $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                                    }
                                    $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').removeClass('disableAll');
                                    $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                    if (NotesLoad_JSON[0].IsNonBilable && NotesLoad_JSON[0].IsNonBilable != "True") {
                                        $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').removeClass('disableAll');
                                        $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                    }
                                    else {
                                        $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").addClass("disableAll");
                                    }
                                }
                                else {
                                    $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').removeClass('disableAll');
                                    $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                    if (NotesLoad_JSON[0].IsNonBilable && NotesLoad_JSON[0].IsNonBilable != "True") {
                                        $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').removeClass('disableAll');
                                        $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                    }
                                    else {
                                        $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").addClass("disableAll");
                                    }
                                }
                                $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").attr("onclick", "Clinical_NotesView.LoadVisitDetail('" + Clinical_NotesView.params.VisitId + "','" + Clinical_NotesView.params.PatientId + "',event)");
                            }
                            var SignedProviderId = NotesLoad_JSON[0].ProviderId;
                            //Start || 27 July, 2016 || Talha Tanweer || Story EMR-86

                            var IsWaterMarkApplied = 0;
                            providerDetail.FillProvider(SignedProviderId, IsWaterMarkApplied).done(function (response) {
                                if (response.status != false) {
                                    var provider_detail = JSON.parse(response.ProviderFill_JSON);
                                    Clinical_NotesView.provider_detail = provider_detail;
                                    var eSignature_image_Src = provider_detail.imgeSignature;
                                    if (parseInt(Clinical_NotesView.params.BillingInfoId) > 0) {
                                        if (Clinical_NotesView.params.ParentCtrl == "EncounterChargeCapture" || Clinical_NotesView.params.ParentCtrl == "Bill_ChargeSearch") {
                                            $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                            $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").addClass("disableAll");
                                            // $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').addClass('disableAll');
                                        }
                                        else if (Clinical_NotesView.params.ParentCtrl == "clinicalTabProgressNote") {
                                            if (parseInt(Clinical_NotesView.params.BillingInfoId) > 0 && NotesLoad_JSON[0].IsNonBilable != "True") {
                                                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                                                $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
                                            }
                                            else {
                                                $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                                            }
                                            $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').removeClass('disableAll');
                                            $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                            if (NotesLoad_JSON[0].IsNonBilable && NotesLoad_JSON[0].IsNonBilable != "True") {
                                                $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').removeClass('disableAll');
                                                $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                            }
                                            else {
                                                $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").addClass("disableAll");
                                            }
                                        }
                                        else {
                                            $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').removeClass('disableAll');
                                            $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").removeClass("hidden");
                                            if (NotesLoad_JSON[0].IsNonBilable && NotesLoad_JSON[0].IsNonBilable != "True") {
                                                $('#' + Clinical_NotesView.params.PanelID + ' #btnBillingInfo').removeClass('disableAll');
                                            }
                                            else {
                                                $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").addClass("disableAll");
                                            }
                                        }
                                        $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").attr("onclick", "Clinical_NotesView.LoadVisitDetail('" + Clinical_NotesView.params.VisitId + "','" + Clinical_NotesView.params.PatientId + "',event)");
                                    }
                                    var Is_eSignatured = provider_detail.chkIs_eSignatured && provider_detail.chkIs_eSignatured != "" ? JSON.parse(provider_detail.chkIs_eSignatured.toLowerCase()) : false;
                                    if (eSignature_image_Src != "" && Is_eSignatured) {
                                        var isBrowserIE = providerDetail.GetIEVersion() > 0;
                                        if (isBrowserIE) {
                                            eSignature_image_Src = eSignature_image_Src.replace("System.Byte[]", "image/gif");
                                        }
                                        var imgeSignatureHtml = '<li><div style="max-height:350px; overflow-y:auto;margin-top:15px;" >' +
                                                                    '<img id="img_eSignature_ProgressNotes" src="' + eSignature_image_Src + '" ' +
                                                                         'alt="" style="height: 125px; width: 315px;border:none;" ' +
                                                                         'class="img-responsive img-center mt-lg img-thumbnail"/>'
                                        '</div></li>';
                                        if (!appendSign) {
                                            if (Clinical_NotesView.params.FromProgressnote == "1") {
                                                if ($('#signedBy').length > 0) {
                                                    $(imgeSignatureHtml).insertBefore($('#signedBy'));
                                                } else {
                                                    $(imgeSignatureHtml).insertBefore($(".list-unstyled li:contains('e-Signed by:')"));
                                                }
                                            }
                                            else {
                                                if ($('#signedBy').length > 0) {
                                                    $(imgeSignatureHtml).insertBefore($('#signedBy'));
                                                } else {
                                                    $(imgeSignatureHtml).insertBefore($(".list-unstyled li:contains('e-Signed by:')"));
                                                }
                                            }
                                        }

                                        if ($('#printcall #img_eSignature_ProgressNotes').length > 1) {
                                            $('#printcall #img_eSignature_ProgressNotes').first().hide();
                                        }
                                    }
                                }

                            });
                        }
                        else {
                            //$('#' + Clinical_NotesView.params["PanelID"] + ' #noteButtonEdit, #noteButtonSign').attr('disabled', false);
                            $('#' + Clinical_NotesView.params["PanelID"] + ' #noteButtonEdit').attr('disabled', false);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #noteButtonCoSign").attr('disabled', true);
                            $('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #noteButtonAmendment").attr('disabled', true);
                            //$('#' + Clinical_NotesView.params.PanelID + " #frmNotesView #btnBillingInfo").attr('disabled', true);
                            $('#' + Clinical_NotesView.params["PanelID"] + " section[id*='Cli_BillingInfo']").parent().css("display", "");
                            $('#' + Clinical_NotesView.params["PanelID"] + " #clinicalMenu_BillingInfo").css("display", "");
                            dfd.resolve('ok');
                        }

                        if (NotesLoad_JSON[0].IsNonBilable == "True") {
                            $('#' + Clinical_NotesView.params["PanelID"] + ' #lblNoteBillStatus').parent().removeClass("hidden");
                            $('#' + Clinical_NotesView.params["PanelID"] + ' #lblNoteBillStatus').text("Non-Billable Visit.");
                        }
                        else {
                            $('#' + Clinical_NotesView.params["PanelID"] + ' #lblNoteBillStatus').parent().addClass("hidden");
                            $('#' + Clinical_NotesView.params["PanelID"] + ' #lblNoteBillStatus').text("");
                        }

                        if (!AmendfromProgress) {

                            if (globalAppdata.NotePrevieStyle != "Classic Style" || Clinical_NotesView.params.IsPhoneEncounter) {
                                BackgroundLoaderShow(true);
                                Clinical_NotesView.getPrintnotePDF(false).done(function () {
                                    BackgroundLoaderShow(false);
                                    dfd.resolve('ok');
                                });
                            } else {
                                Clinical_NotesView.LoadLegacyNoteView(NotesId, PatientId, ProviderId).done(function (response) {
                                    response = JSON.parse(response);
                                    //Start 28-09-2017 Edit By Humaira Yousaf Bug# EMR-4844
                                    Clinical_NotesView.pdf = response.Message;
                                    //End 28-09-2017 Edit By Humaira Yousaf Bug# EMR-4844
                                    utility.PDFViewer(response.Message, false, 'Clinical_NotesView #PreviewReportPrint', true);
                                    $("#printcall").hide();
                                    dfd.resolve('ok');
                                });
                            }
                        }
                        else {
                            dfd.resolve('ok');
                        }
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve('ok');
                }
            });
        });


        return dfd.promise();

        //Clinical_NotesView.PreviewNotesClinical(NotesId).done(function (response) {
        //    response = JSON.parse(response);
        //    utility.PDFViewer(response.NotesHTML, false, 'Clinical_NotesView #PreviewClaimFormIF',true);

        //});
        //return objDeffered;
    },

    LoadLegacyNoteView: function (NotesId, PatientId, ProviderId) {
        var noteID = NotesId;
        var patientID = PatientId;
        var providerId = ProviderId;
        var objNotesComponentViewModel = new Object();
        objNotesComponentViewModel.NotesComponents = new Array();
        for (var i = 0, l = Clinical_NotesView.CheckBoxes.length; i < l; i++) {
            var obj = new Object();
            obj.NotesId = noteID;
            obj.PatientId = patientID;
            obj.ProviderId = providerId;
            var value = Clinical_NotesView.CheckBoxes[i]
            obj.Component = value.trim().split(" ").join("");
            objNotesComponentViewModel.NotesComponents.push(obj);
        }
        var obj = new Object();
        obj.NotesId = noteID;
        obj.PatientId = patientID;
        obj.ProviderId = providerId;
        obj.Component = "NotesComponent";
        objNotesComponentViewModel.NotesComponents.push(obj);
        objNotesComponentViewModel.NotesPreviewStyle = globalAppdata.NotePrevieStyle;
        return MDVisionService.APIServiceComplex(objNotesComponentViewModel, "ClinicalNotes", "LoadLegacyNoteAndRenderTemplate");
    },

    LoadVisitDetail: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (Clinical_NotesView.params.ParentCtrl != "EncounterChargeCapture") {


                    var params = [];
                    params["FromAdmin"] = 0;
                    params["ParentCtrl"] = 'Clinical_NotesView';
                    params["Parent"] = Clinical_NotesView.params.ParentCtrl;

                    params["VisitId"] = VisitId;
                    params["patientID"] = PatientId;
                    params["NotesId"] = Clinical_NotesView.params.NotesId;

                    LoadActionPan('EncounterChargeCapture', params, Clinical_NotesView.params.PanelID);
                } else {
                    if (Clinical_NotesView.params != null && Clinical_NotesView.params.ParentCtrl) {
                        UnloadActionPan(Clinical_NotesView.params.ParentCtrl, "Clinical_NotesView");
                    }
                    else {
                        UnloadActionPan(null, "Clinical_NotesView");
                    }
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenNotesComponentSelection: function (NotesId, PatientId, ProviderId, AmendfromProgress, docAttached) {
        var params = [];
        params["NotesId"] = NotesId;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["AmendfromProgress"] = AmendfromProgress;
        params["IsOptional"] = false;
        params["RefForm"] = "frmNotesView";
        params["CheckBoxes"] = Clinical_NotesView.CheckBoxes;
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_NotesView";
        params["DocAttached"] = docAttached;
        params["NoteStatus"] = Clinical_NotesView.params.NoteStatus;
        LoadActionPan('Clinical_NotesComponentSelection', params);
    },

    ShowHideComponetsHeaders: function () {
        var contentList = '#' + Clinical_NotesView.params["PanelID"] + ' #printcall #ulContent'
        var subjComponents = $(contentList + " #SubjectiveNoteComponentList li.editableContentli");
        var objComponents = $(contentList + " #ObjectiveNoteComponentList li.editableContentli");
        var assesComponents = $(contentList + " #AssessmentNoteComponentList li.editableContentli");
        var progressComponents = $(contentList + " #ProgressNoteComponentList li.editableContentli");
        var planComponents = $(contentList + " #PlanNoteComponentList li.editableContentli");
        var subjHeaders = $(contentList + " #SubjectiveNoteComponentList header + section");
        if (subjHeaders.length == 0) {
            subjHeaders = $(contentList + " #SubjectiveNoteComponentList").find('header').next().find('section');
        }
        var objHeaders = $(contentList + " #ObjectiveNoteComponentList header + section");
        if (objHeaders.length == 0) {
            objHeaders = $(contentList + " #ObjectiveNoteComponentList").find('header').next().find('section');
        }
        var assesHeaders = $(contentList + " #AssessmentNoteComponentList header + section");
        if (assesHeaders.length == 0) {
            assesHeaders = $(contentList + " #AssessmentNoteComponentList").find('header').next().find('section');
        }
        var planHeaders = $(contentList + " #PlanNoteComponentList header + section");
        if (planHeaders.length == 0) {
            planHeaders = $(contentList + " #PlanNoteComponentList").find('header').next().find('section');
        }
        //Begin Edited By Fahad Malik on 16-Dec-2016 to fix bug#: EMR-2231
        var ProgressNotehtm = $("#" + Clinical_NotesView.params["PanelID"] + " #ProgressNoteComponentList header #clinicalMenu_BillingInfo");
        ProgressNotehtm.closest('li').addClass("hidden");
        //End Edited By Fahad Malik on 16-Dec-2016 to fix bug#: EMR-2231
        if ((subjComponents && subjComponents.length > 0) || (subjHeaders && subjHeaders.length > 0)) {
            $(contentList + " #SubjectiveNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $(contentList + " #SubjectiveNoteComponentList").prev('h4').remove();
            $(contentList + " #SubjectiveNoteComponentList").remove();
        }
        if ((objComponents && objComponents.length > 0) || (objHeaders && objHeaders.length > 0)) {
            $(contentList + " #ObjectiveNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $(contentList + " #ObjectiveNoteComponentList").prev('h4').remove();
            $(contentList + " #ObjectiveNoteComponentList").remove();
        }
        if ((assesComponents && assesComponents.length > 0) || (assesHeaders && assesHeaders.length > 0)) {
            $(contentList + " #AssessmentNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $(contentList + " #AssessmentNoteComponentList").prev('h4').remove();
            $(contentList + " #AssessmentNoteComponentList").remove();
        }
        if ((planComponents && planComponents.length > 0) || (planHeaders && planHeaders.length > 0)) {
            $(contentList + " #PlanNoteComponentList").prev('h4').removeClass('hidden');
        }
        else {
            $(contentList + " #PlanNoteComponentList").prev('h4').remove();
            $(contentList + " #PlanNoteComponentList").remove();
        }
        if ((progressComponents && progressComponents.length < 1 && Clinical_NotesView.params.NoteStatus == "Signed")) {
            $(contentList + " #ProgressNoteComponentList").find('clinical_billinginfo').closest('li.initialVisitBody').remove();
        }

    },
    BillingInfo: function () {


        if (parseInt(Clinical_NotesView.params.BillingInfoId) > 0) {
            if (Clinical_NotesView.params.NoteStatus && Clinical_NotesView.params.NoteStatus == "Signed" && Clinical_NotesView.params.BillingInfoStatus && Clinical_NotesView.params.BillingInfoStatus == "Draft") {
                utility.DisplayMessages("No eSuperbill Information is associated with the current provider note.", 2);
                return false;
            }

            Clinical_ProgressNote.FillNotes(null, Clinical_NotesView.params.NotesId, Clinical_NotesView.params.PatientId).done(function (response) {

                response = JSON.parse(response);
                if (response.status != false) {

                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    Clinical_NotesView.params.VisitId = Clinical_Notes_detail.VisitId;


                    var params = [];
                    params["ParentCtrl"] = "Clinical_NotesView";
                    params["FromAdmin"] = 0;
                    params["NotesId"] = Clinical_NotesView.params.NotesId;
                    params["VisitId"] = Clinical_NotesView.params.VisitId;
                    params["NoteDate"] = Clinical_Notes_detail.VisitDate;
                    params["BillingInfoId"] = Clinical_NotesView.params.BillingInfoId;
                    params["VisitDate"] = Clinical_Notes_detail.VisitDate;
                    params["PatientId"] = Clinical_NotesView.params.PatientId;
                    params["ProviderId"] = Clinical_NotesView.params.ProviderId;
                    params["PatientTypeId"] = Clinical_NotesView.params.PatientTypeId;
                    params["AppointmentDate"] = $('#' + Clinical_NotesView.params.PanelID + ' #txtLinkedAppointment').val();
                    params["EnbtnViewCharges"] = Clinical_NotesView.params.EnbtnViewCharges;
                    params["NoteStatus"] = Clinical_NotesView.params.NoteStatus;
                    if (Clinical_NotesView.params.ParentCtrl == "EncounterChargeCapture") {
                        params["IsFromEncounter"] = true;
                    } else {
                        params["IsFromEncounter"] = false;
                    }
                    LoadActionPan("BillingInformation", params);

                } else {
                    utility.DisplayMessages(response.Message, 2);
                }

            });

        }
        else {
            utility.DisplayMessages("No eSuperbill Information is associated with the current provider note.", 2);
        }


    },

    PreviewNotes: function (NotesID, PatientId, ProviderId) {
        var data = "NotesID=" + NotesID + "&PatientId=" + PatientId + "&ProviderId=" + ProviderId + "&FormName=Notes" + "&PreviewStyle=" + globalAppdata.NotePrevieStyle;

        return MDVisionService.defaultService(data, "DASHBOARD", "PREVIEW_NOTES");
    },
    PrintReport: function () {
        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //  if (EMRUtility.detectIE() == false) {
                $("#" + Clinical_NotesView.params["PanelID"] + " #PreviewClaimFormIF")[0].contentWindow.focus();
                $("#" + Clinical_NotesView.params["PanelID"] + " #PreviewClaimFormIF")[0].contentWindow.print();
                // $('#' + Clinical_NotesView.params["PanelID"] + ' #PreviewClaimFormIF').contents().find('#print').click();
                //} else {
                //    utility.PDFViewer($('#' + Clinical_NotesView.params["PanelID"] + ' #PreviewClaimFormIF').parent().find('#helperPDF').val(), true);
                //}

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },

    PrintReports: function (NotesId, PatientId, ProviderId, AmendfromProgress, ComponentSelection) {
        AppPrivileges.GetFormPrivileges("Notes_Notes", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (!NotesId) {
                    NotesId = Clinical_NotesView.params.NotesId;
                }
                if (!PatientId) {
                    PatientId = Clinical_NotesView.params.PatientId;
                }
                if (!ProviderId) {
                    ProviderId = Clinical_NotesView.params.ProviderId;
                }

                if (ComponentSelection == 'disable') {
                    Clinical_NotesView.NotesPreview(NotesId, PatientId, ProviderId, AmendfromProgress);
                }
                else {
                    Clinical_ProgressNote.noteAttachmentExists_DBCall().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            var docAttached = response.IsAttachmentExists == "1" ? true : false;
                            
                            if (docAttached == true || Clinical_NotesView.PEDocExists == true) {
                                Clinical_NotesView.OpenNotesComponentSelection(NotesId, PatientId, ProviderId, AmendfromProgress, true);
                            }
                            else {
                                if (Clinical_NotesView.CheckBoxes.length > 0) {
                                    if (globalAppdata["IsSelectNoteComponent"] == "True") {
                                        Clinical_NotesView.OpenNotesComponentSelection(NotesId, PatientId, ProviderId, AmendfromProgress, false);
                                    }
                                    else {
                                        if (globalAppdata.NotePrevieStyle != "Classic Style" || Clinical_NotesView.params.IsPhoneEncounter) {
                                            $('#' + Clinical_NotesView.params["PanelID"] + " #printcallClone").html($('#' + Clinical_NotesView.params["PanelID"] + " #printcall")[0].innerHTML);
                                            Clinical_NotesView.printNote();

                                        } else {
                                            Clinical_NotesView.LoadLegacyNoteView(NotesId, PatientId, ProviderId).done(function (response) {
                                                response = JSON.parse(response);
                                                //Start 28-09-2017 Edit By Humaira Yousaf Bug# EMR-4844
                                                Clinical_NotesView.pdf = response.Message;
                                                //End 28-09-2017 Edit By Humaira Yousaf Bug# EMR-4844
                                                utility.PDFViewer(response.Message, true, null, null, true);
                                                $("#printcall").hide();

                                            });
                                        }
                                    }

                                }
                                else {
                                    if (globalAppdata.NotePrevieStyle != "Classic Style" || Clinical_NotesView.params.IsPhoneEncounter || typeof Clinical_NotesView.params.IsPhoneEncounter == "undefined") {
                                        $('#' + Clinical_NotesView.params["PanelID"] + " #printcallClone").html($('#' + Clinical_NotesView.params["PanelID"] + " #printcall")[0].innerHTML);
                                        Clinical_NotesView.printNote();

                                    } else {
                                        Clinical_NotesView.LoadLegacyNoteView(NotesId, PatientId, ProviderId).done(function (response) {
                                            response = JSON.parse(response);
                                            //Start 28-09-2017 Edit By Humaira Yousaf Bug# EMR-4844
                                            Clinical_NotesView.pdf = response.Message;
                                            //End 28-09-2017 Edit By Humaira Yousaf Bug# EMR-4844
                                            utility.PDFViewer(response.Message, true, null, null, true);
                                            $("#printcall").hide();

                                        });
                                    }
                                }
                            }

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                Clinical_NotesView.PrintNotesAudit();
            } else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    printNote: function (saveDiagnosticResult) {
        $('#' + Clinical_NotesView.params["PanelID"] + " #printcallClone").css('display', '');
        kendo.drawing.drawDOM('#' + Clinical_NotesView.params["PanelID"] + " #printcallClone", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            margin: {
                left: "10mm",
                top: "7mm",
                right: "10mm",
                bottom: "15mm"
            },
            template: kendo.template($('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html())
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                var params = [];
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                params["PrintPDFDataURL"] = PrintPDFDataURL;
                params["PreviewPdf"] = true;
                utility.PDFViewer(params["PrintPDFDataURL"], true, null, null, true);
                $('#' + Clinical_NotesView.params["PanelID"] + " #printcallClone").hide();

                if (saveDiagnosticResult) {
                    Clinical_NotesView.SaveDiagnosticResultInPatDocs(PrintPDFDataURL);
                }
            });

        });
    },

    PrintNotesAudit: function () {

        var objData = {};
        objData["NotesId"] = Clinical_NotesView.params.NotesId;
        objData["commandType"] = "PRINT_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    PreviewNotesClinical: function (NotesId) {

        var objData = {};
        objData["NotesId"] = NotesId;
        //   var data = "NotesID=" + NotesId;
        objData["commandType"] = "PREVIEW_NOTES";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    // In a New Window
    PrintNotes: function () {
        Clinical_NotesView.PreviewNotes(Clinical_NotesView.params.NotesId).done(function (response) {
            if (response.status != false) {
                var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);
                if (NotesLoad_JSON[0].NoteText != '' && NotesLoad_JSON[0].NoteText != null) {
                    HTMLNotes = NotesLoad_JSON[0].NoteText;

                    HTMLNotes = HTMLNotes.replace(/&quot;/g, '"');
                    HTMLNotes = HTMLNotes.replace(/&lt;/g, '<').replace(/&gt;/g, '>');
                }

                var docType = '<!doctype html>';
                var docCnt = "<h4 class='text-center' style='word-wrap: break-word;white-space: pre-wrap;font-style: normal;font-family: Verdana;font-size: 12pt;font-weight: 400;color: #000;direction: ltr;text-align: left;'>Report Heading</h4>" + HTMLNotes;
                var docHead = '<head> <link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" /></head>';
                var winAttr = "location=yes, statusbar=no, directories=no, menubar=no, titlebar=no, toolbar=no, dependent=no, width=720, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                var newWin = window.open("", "_blank", winAttr);
                writeDoc = newWin.document;
                writeDoc.open();
                writeDoc.write(docType + '<html>' + docHead + '<body onload="">' + docCnt + '</body></html>');
                writeDoc.close();
                newWin.focus();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },
    /*Author: M Ahmad Imran, Date: 05/2/2016
    Purpose: for Notes HL7 creation*/
    SelectSyndromicSurveillance: function (SyndromicType) {

        Clinical_NotesView.NotesHL7CreationDBcall(SyndromicType).done(function (response) {
            response = JSON.parse(response);
            if (response.status == true) {
                // utility.DisplayMessages(strMessage, 2);
                utility.DisplayMessages(response.Message, 1);
                var patientId = Clinical_NotesView.params.patientID;
                var uri = '';
                var dt = new Date();
                var strMimeType = "application/octet-stream";
                var dateString = dt.getFullYear() + '/' + dt.getMonth() + '/' + dt.getDate() + '/' + dt.getHours() + '/' + dt.getMinutes() + '/' + dt.getSeconds();
                download(uri + response.HL7Message, +patientId + "[Syndromic]-" + dateString.replace(/\//g, '') + ".txt", strMimeType);

            }
            else {
                utility.DisplayMessages(response.Message, 4);
            }
        });
    },

    getPrintnotePDF: function (isSignNote, IsPhoneEncounter) {
        var def = $.Deferred();

        if (Clinical_NotesView.params.FromProgressnote == "1") {
            Clinical_NotesView.params["PanelID"] = "progressnotesign #Clinical_NotesView"
        }
        // $('#' + Clinical_NotesView.params["PanelID"] + " #contemporaryViewDiv").css('display', 'none');
        //   $('#' + Clinical_NotesView.params["PanelID"] + " #legacyViewDiv").css('display', 'block');
        //$('#' + Clinical_NotesView.params["PanelID"] + " #legacyViewTemplateDiv").css('display', 'block');

        // var HeaderLogo = $('#' + Clinical_NotesView.params["PanelID"] + " #printcall :first img").prop('src');//response.HeaderLogo;
        var FooterText = $('#' + Clinical_NotesView.params["PanelID"] + " #printcall footer").text().split('Generated by: ').join('');
        $('#progressnotesign').removeClass("hidden");
        $('#progressnotesign').show();
        $('#' + Clinical_NotesView.params["PanelID"] + " #printcall").show();
        $('#' + Clinical_NotesView.params["PanelID"] + " #printcall").css("display", "inline");
        // ----- footer
        if (FooterText !== null && FooterText !== "") {
            var insideHTML = $('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html();
            var PageTemp = $(insideHTML);
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(FooterText);
            $(PageTemp).find('#patientname').text(Clinical_NotesView.patName);
            $(PageTemp).find('#providerName').text(Clinical_NotesView.ProviderName);
            $(PageTemp).find('#patientDOB').text(Clinical_NotesView.patDOB);
            $(PageTemp).find('#patientDOS').text(Clinical_NotesView.DOS);

            $('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html(PageTemp);
        }
        else {
            var footerText = $('#' + Clinical_NotesView.params["PanelID"] + " #printcall  footer").text().split('Generated by: ').join('');
            var insideHTML = $('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html();
            var PageTemp = $(insideHTML);
            if (footerText == null || footerText == '') {
                footerText = 'MDVISION PM EMR'
            }
            $(PageTemp).find('#ClinicalReportsFooterLegacy').text(footerText);
            $(PageTemp).find('#patientname').text(Clinical_NotesView.patName);
            $(PageTemp).find('#providerName').text(Clinical_NotesView.ProviderName);
            $(PageTemp).find('#patientDOB').text(Clinical_NotesView.patDOB);
            $(PageTemp).find('#patientDOS').text(Clinical_NotesView.DOS);
            $('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html(PageTemp);

        }
        var insideHTML = $('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html();
        var PageTemp = $(insideHTML);
        $('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html(PageTemp);
        var height = 0;

        var topMargin = height <= 18 ? 18 : height + 3;

        if ($('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy .blueBorderPrint").length >= 1) {
            $('#' + Clinical_NotesView.params["PanelID"] + " #printcall .form-group").remove();
            $('#' + Clinical_NotesView.params["PanelID"] + " #printcall footer").remove();
        }
        // --------------------------------------------- start Download functionality--------------------------------------------------------------
        kendo.drawing.drawDOM('#' + Clinical_NotesView.params["PanelID"] + " #printcall", {
            landscape: false,
            scale: 0.6,
            paperSize: "A4",
            // margin: "2cm 3cm ",
            margin: {
                left: "10mm",
                //Begin Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                top: "7mm",
                //End Edited By Fahad Malik on 23-12-2016 to fix bug#: EMR-2353
                right: "10mm",
                bottom: "15mm"
            },
            // rowtemplate: $('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html()
            template: kendo.template($('#' + Clinical_NotesView.params["PanelID"] + " #page-templateLegacy").html())
        }).then(function (group) {

            kendo.drawing.pdf.toDataURL(group, function (dataURL) {
                // Clinical_NotesView.pdf = dataURL;
                var params = [];
                var PrintPDFDataURL = dataURL.split('data:application/pdf;base64,').join('');
                params["PrintPDFDataURL"] = PrintPDFDataURL;
                Clinical_NotesView.params["PDFData"] = PrintPDFDataURL;
                Clinical_NotesView.pdf = PrintPDFDataURL;
                params["PreviewPdf"] = true;
                if (!isSignNote) {
                    utility.PDFViewer(params["PrintPDFDataURL"], false, 'PreviewReportPrint', true);

                    if (Clinical_NotesView.params["IsToSavePatientDocuments"] == true) {
                        Clinical_NotesView.SaveSignedNoteToPatientDocument().done(function () {
                            $('#progressnotesign').remove();
                            Clinical_NotesView.params["IsToSavePatientDocuments"] = false;
                            def.resolve();
                        });
                    }
                    else
                        def.resolve('ok');

                } else {
                    var data = new FormData();
                    data.append('notes', Clinical_NotesView.params["PDFData"]);
                    data.append("PatientID", Clinical_NotesView.params.PatientId);
                    data.append("Folder", "12");
                    IsPhoneEncounter ? data.append("FolderName", "Phone Encounter") : data.append("FolderName", "Progress Notes")
                    data.append("FileName", "Signed Note");
                    data.append("fileType", "application/pdf");
                    var myObject = new Object();
                    myObject.ddlFolder = "12";
                    var myJSON = JSON.stringify(myObject);
                    data.append("PatientDocumentData", myJSON);
                    Document_Import.SaveImport(data).done(function (response) {
                        $('#progressnotesign').remove();
                        def.resolve("ok");

                    });

                }
                $('#' + Clinical_NotesView.params["PanelID"] + " #printcall").hide();

            });

        });
        // ------------------------------------- End Download functionality--------------------------------------



        return def.promise();


    },

    SaveSignedNoteToPatientDocument: function () {

        var def = $.Deferred();

        if (Clinical_NotesView.params["PDFData"]) {
            var data = new FormData();
            data.append('notes', Clinical_NotesView.params["PDFData"]);
            data.append("PatientID", Clinical_NotesView.params.PatientId);
            data.append("Folder", "12");
            Clinical_NotesView.params.IsPhoneEncounter ? data.append("FolderName", "Phone Encounter") : data.append("FolderName", "Progress Notes")
            data.append("FileName", "Signed Note");
            data.append("fileType", "application/pdf");
            var myObject = new Object();
            myObject.ddlFolder = "12";
            var myJSON = JSON.stringify(myObject);
            data.append("PatientDocumentData", myJSON);
            Document_Import.SaveImport(data).done(function (response) {
                $('#progressnotesign').remove();
                def.resolve("ok");

            });
        }
        else {
            console.log("empty PDF data.");
            def.resolve("ok");

        }

        return def.promise();
    },



    VBPWithReasoningLoad: function () {
        var objDeffered = $.Deferred();
        Clinical_ProgressNote.loadVBPWithReasoning(Clinical_NotesView.params.VisitDate, Clinical_NotesView.params.VisitDate, Clinical_NotesView.params.PatientId, Clinical_NotesView.params.ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_NotesView.params["VBPResponse"] = response;
                if (response.AllMeasuresReasoningDetailCount > 0) {
                    var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                        return a.Patientid == Clinical_NotesView.params.PatientId && a.NoteId == Clinical_NotesView.params.NotesId;
                    });

                    if (arrNonCompliantPatients.length > 0) {
                        Clinical_NotesView.arrVBPReasoning[Clinical_NotesView.params.PatientId] = JSON.stringify(arrNonCompliantPatients);
                        var CQMFoundMsg = "Our System found some <span class='red'>missing data</span> related to this patient."
                                        + " In order to qualify for the <b>2017 Value Based program incentives,</b> you must enter those <span class='red'>missing data values</span>"
                                        + " against the Value Based program that you have planned to report this year. Do you want to enter the data here before signing off this note?";
                        var PHQ2Missing = false;
                        var PHQ9Missing = false;
                        utility.myConfirm(CQMFoundMsg, function () {
                            $.each(arrNonCompliantPatients, function (key) {
                                if (arrNonCompliantPatients[key].MeasureId == "PHQ2") {
                                    PHQ2Missing = true;
                                }
                                else if (arrNonCompliantPatients[key].MeasureId == "PHQ9") {
                                    PHQ9Missing = true;
                                }
                            });
                            $.when(Clinical_NotesView.openMissingAlert_VBP(null, null, null, 'Clinical_NotesView', Clinical_NotesView.params.NotesId, Clinical_NotesView.params.NoteStatus, false, Clinical_NotesView.params.VisitDate, Clinical_NotesView.params.PatientId, Clinical_NotesView.params.ProviderId, Clinical_NotesView.params.BillingInfoId, Clinical_NotesView.params.AppointmentDate, Clinical_NotesView.params.VisitId, Clinical_NotesView.params.NoteDate, Clinical_NotesView.params.PatientTypeId, Clinical_NotesView.params.FacilityId, Clinical_NotesView.params.POS, Clinical_NotesView.params.RefProviderID, PHQ2Missing, PHQ9Missing)).then(function () {
                                Clinical_NotesView.params.isVBPExists = 1;
                                objDeffered.resolve();
                            });
                        }, function () {
                            $.when(Clinical_NotesView.CQMWithReasoningLoad()).then(function () {
                                objDeffered.resolve();
                            });
                        },
                               '<b>2017 Value Based Program Missing Data Alert</b>', "Yes, I do", "No, not this time"
                          );
                    }
                    else {
                        $.when(Clinical_NotesView.CQMWithReasoningLoad()).then(function () {
                            objDeffered.resolve();
                        });
                    }
                }
                else {
                    $.when(Clinical_NotesView.CQMWithReasoningLoad()).then(function () {
                        objDeffered.resolve();
                    });
                }
            }
            else {
                $.when(Clinical_NotesView.CQMWithReasoningLoad()).then(function () {
                    objDeffered.resolve();
                });
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return objDeffered;
    },
    openMissingAlert_VBP: function (BillingInformation, Obj, customSigMsg, prntctrl, NotesId, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID, PHQ2Missing, PHQ9Missing) {
        params["FromAdmin"] = "0";
        params["BillingInformation"] = BillingInformation;
        params["Obj"] = Obj;
        params["customSigMsg"] = customSigMsg;
        params["ParentCtrl"] = prntctrl;
        params["NoteId"] = NotesId;
        params["PatientIds"] = PatientId;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["isComponentSelect"] = isComponentSelect;
        params["VisitDate"] = VisitDate;
        params["BillingInfoId"] = BillingInfoId;
        params["AppointmentDate"] = AppointmentDate;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["PatientTypeId"] = PatientTypeId;
        params["FacilityId"] = FacilityId;
        params["POS"] = POS;
        params["RefProviderID"] = RefProviderID;
        params["PHQ2Missing"] = PHQ2Missing;
        params["PHQ9Missing"] = PHQ9Missing;

        LoadActionPan('VBP_MissingDataAlert', params);
    },
    SignNotes: function () {
        var objDeffered = $.Deferred();
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
                if (!Clinical_NotesView.params.IsPhoneEncounter && CDSAlertCount > 0) {
                    utility.DisplayMessages("Please change the Status of all the CDS Alerts before signing the Note.", 3);
                }
                else {
                    Clinical_NotesView.VBPWithReasoningLoad();
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    CQMWithReasoningLoad: function () {
        var objDeffered = $.Deferred();
        Clinical_ProgressNote.loadCQMWithReasoning(Clinical_NotesView.params.VisitDate, Clinical_NotesView.params.VisitDate, Clinical_NotesView.params.PatientId, Clinical_NotesView.params.ProviderId, Clinical_NotesView.params.NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_NotesView.params["CQMResponse"] = response;
                if (response.AllMeasuresReasoningDetailCount > 0) {
                    var arrNonCompliantPatients = $.grep(JSON.parse(response.AllMeasuresReasoningDetailLoad_JSON), function (a) {
                        return a.Patientid == Clinical_NotesView.params.PatientId;
                    });

                    Clinical_NotesView.arrCQMReasoning[Clinical_NotesView.params.PatientId] = JSON.stringify(arrNonCompliantPatients);
                    var CQMFoundMsg = "Our System found some <span class='red'>missing data</span> related to this patient."
                                    + " In order to qualify for the <b>2016 CQM incentives,</b> you must enter those <span class='red'>missing data values</span>"
                                    + " against the CQM measures that you have planned to report this year. Do you want to enter the data here before signing off this note?";

                    utility.myConfirm(CQMFoundMsg, function () {
                        objDeffered.resolve();
                        Clinical_Notes.openPatientList(Clinical_NotesView.params.PatientId, null, Clinical_NotesView.params.ProviderId, Clinical_NotesView.params.VisitDate, Clinical_NotesView.params.NotesId, null, "Clinical_NotesView");
                    }, function () {
                        $.when(Clinical_NotesView.SignNotesAfterCQM(Clinical_NotesView.params.NotesId, Clinical_NotesView.params.NoteStatus, false, Clinical_NotesView.params.VisitDate, Clinical_NotesView.params.PatientId, Clinical_NotesView.params.ProviderId, Clinical_NotesView.params.BillingInfoId, Clinical_NotesView.params.AppointmentDate, Clinical_NotesView.params.VisitId, Clinical_NotesView.params.NoteDate, Clinical_NotesView.params.PatientTypeId, Clinical_NotesView.params.FacilityId, Clinical_NotesView.params.POS, Clinical_NotesView.params.RefProviderID)).then(function () {
                            objDeffered.resolve();
                        });
                    },
                          '<b>2016 CQM Missing Data Alert</b>', "Yes, I do", "No, not this time"
                      );

                }
                else {
                    $.when(Clinical_NotesView.SignNotesAfterCQM(Clinical_NotesView.params.NotesId, Clinical_NotesView.params.NoteStatus, false, Clinical_NotesView.params.VisitDate, Clinical_NotesView.params.PatientId, Clinical_NotesView.params.ProviderId, Clinical_NotesView.params.BillingInfoId, Clinical_NotesView.params.AppointmentDate, Clinical_NotesView.params.VisitId, Clinical_NotesView.params.NoteDate, Clinical_NotesView.params.PatientTypeId, Clinical_NotesView.params.FacilityId, Clinical_NotesView.params.POS, Clinical_NotesView.params.RefProviderID)).then(function () {
                        objDeffered.resolve();
                    });
                }
            }
            else {
                objDeffered.resolve();
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },
    SignNotesAfterCQM: function (NotesId, NoteStatus, isComponentSelect, VisitDate, PatientId, ProviderId, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        utility.myConfirm('Do you want to Sign this record?', function () {
            var signMessage = "eSuperbill will also be signed. Are you sure you want to sign the provider note?";
            utility.myConfirm(signMessage, function () {
                DashBoard.NotesUpdate(NotesId).done(function (response) {

                    if (response.status != false) {
                        Clinical_Notes.SetModifiedNoteCount();
                        //$("#noteButtonSign").attr("disabled", "disabled");
                        Clinical_NotesSearch.LoadAttachecdICDsAndCPTs(NotesId, PatientId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                response.ProblemListFill_JSON = JSON.parse(response.ProblemListFill_JSON);
                                response.ProcedureListFill_JSON = JSON.parse(response.ProcedureListFill_JSON);
                                response.ProblemListFill_JSON = $.grep(response.ProblemListFill_JSON, function (a) {
                                    return a.IsNoteLinked == "True";
                                });
                                response.ProcedureListFill_JSON = $.grep(response.ProcedureListFill_JSON, function (a) {
                                    return a.IsNoteLinked == "1" || (parseInt(a.VaccineHxId) > 0 || parseInt(a.ImmTherInjectionId) > 0);
                                });

                                var NumberOfProblems = response.ProblemListFill_JSON.length;
                                var numberOfCPTs = response.ProcedureListFill_JSON.length;

                                if (NumberOfProblems > 0 && numberOfCPTs > 0) {
                                    Clinical_NotesView.CreateCharges(null, NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID);
                                } else {

                                    utility.DisplayMessages("Either Procedure or Diagnosis Code is missing. The charge will not be created", 3);
                                }
                            }
                            else {
                                utility.DisplayMessages("Either Procedure or Diagnosis Code is missing. The charge will not be created", 3);
                            }
                        });

                        Clinical_NotesView.params["IsToSavePatientDocuments"] = true;
                        $.when(Clinical_NotesView.NotesPreview(NotesId, null, null, null, null, null, null)).then(function () {
                            var triggerLocation = 'Notes';
                            if (Clinical_NotesView.params.ParentCtrl == 'clinicalTabProgressNote' && !Clinical_NotesView.params.IsPhoneEncounter) {
                                ClinicalCDSDetail.showCDSAlert(triggerLocation, 0);
                            }
                            $(" #mainForm #hfTriggerLocation").val('Notes')
                            var CurrentTabID = GetCurrentSelectedTab();
                            if (CurrentTabID.PanelID == 'pnlDashboard') {
                                DashBoard.DashBoardEncounterSearch();
                            } else if (CurrentTabID.PanelID == 'pnlClinicalNotes') {
                                if (Clinical_NotesView.params.RefSearch == 'DraftSearch') {
                                    Clinical_Notes.NotesDraftSearch(0, null, null, 'Draft');
                                    Clinical_Notes.NotesSignedSearch(0, null, null, 'Signed');
                                } else {
                                    Clinical_Notes.NotesSearch();
                                }

                            } else if (Clinical_NotesView.params.ParentCtrl == 'mstrTabReports') {
                                ReportsSSRSDashboard.BindReport();
                                Clinical_NotesView.UnLoad();
                            }
                            if (Clinical_NotesView.params.ParentCtrl == 'mstrTabDashBoard') {
                                DashBoard.DashBoardEncounterSearch();
                            } else if (Clinical_NotesView.params.ParentCtrl == 'clinicalTabNotes') {
                                if (Clinical_NotesView.params.RefSearch == 'DraftSearch') {
                                    Clinical_Notes.NotesDraftSearch(0, null, null, 'Draft');
                                    Clinical_Notes.NotesSignedSearch(0, null, null, 'Signed');
                                } else {
                                    Clinical_Notes.NotesSearch();
                                }
                            } else if (Clinical_NotesView.params.ParentCtrl == 'Clinical_NotesSearch') {
                                Clinical_NotesSearch.NotesSearch();


                            } else if (Clinical_NotesView.params.ParentCtrl == "clinicalTabProgressNote") {
                                Clinical_ProgressNote.Clinical_NotesFill(NotesId, Clinical_ProgressNote.params.PanelID + ' #frmClinicalProgressNote');
                            }
                            else if (Clinical_NotesView.params.ParentCtrl == 'clinicalTabPhoneEncounter') {
                                Clinical_PhoneEncounter.NotesDraftSearch(0, null, null, 'Draft');
                                Clinical_PhoneEncounter.NotesSignedSearch(0, null, null, 'Signed');
                            }

                        });
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }, function () {

            }, 'Confirm Sign');


        }, function () {
        },
                    'Confirm Sign'
                    );
    },

    CreateCharges: function (Obj, NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        BillingInformation.params = Clinical_NotesView.initializeBillingInfoParams(NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID);
        Clinical_NotesSearch.CreateObjectForBilling(POS);
    },

    initializeBillingInfoParams: function (NotesId, PatientId, ProviderId, VisitDate, BillingInfoId, AppointmentDate, VisitId, NoteDate, PatientTypeId, FacilityId, POS, RefProviderID) {
        var params = [];
        params["ParentCtrl"] = "clinicalTabNotes";
        params["FromAdmin"] = 0;
        params["NotesId"] = NotesId;
        params["VisitId"] = VisitId;
        params["NoteDate"] = NoteDate;
        params["BillingInfoId"] = BillingInfoId;
        params["VisitDate"] = VisitDate;
        params["PatientId"] = PatientId;
        params["ProviderId"] = ProviderId;
        params["PatientTypeId"] = PatientTypeId;
        params["AppointmentDate"] = AppointmentDate;
        params["FacilityId"] = FacilityId;
        BillingInformation.PatientInfoJSON = {
        };
        BillingInformation.PatientInfoJSON.RefProviderID = RefProviderID;
        BillingInformation.PatientInfoJSON.FacilityID = FacilityId;
        return params;
    },

    LoadNotesAccess: function () {
        Clinical_NotesView.params.isPatientAccessModify = false;
        Clinical_NotesView.params.isAPIAccessModify = false;
        Clinical_NotesView.LoadNotesAccessDBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                if (response.Found) {
                    NotesExtraData = JSON.parse(response.NotesExtraData);
                    if (NotesExtraData.VDTPatient) {
                        Clinical_NotesView.params.isPatientAccessModify = true;
                        if (NotesExtraData.VDTPatient == 1) {
                            $("#" + Clinical_NotesView.params.PanelID + " #isPatientAccess").prop('checked', true);
                        }
                        else if (NotesExtraData.VDTPatient == 2) {
                            $("#" + Clinical_NotesView.params.PanelID + " #isPatientAccess").prop('checked', false);
                        }
                    }
                    if (NotesExtraData.VDTAPIPatient) {
                        Clinical_NotesView.params.isAPIAccessModify = true;
                        if (NotesExtraData.VDTAPIPatient == 1) {
                            $("#" + Clinical_NotesView.params.PanelID + " #isAPIAccess").prop('checked', true);
                        }
                        else if (NotesExtraData.VDTAPIPatient == 2) {
                            $("#" + Clinical_NotesView.params.PanelID + " #isAPIAccess").prop('checked', false);
                        }
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ToggleAccess: function (event) {
        var objData = new Object();
        if (event.target.name == "isPatientAccess") {
            objData["Description"] = "VDT (Patient)";
            if (event.target.checked) {
                objData["ValueSettingId"] = 1;
            }
            else {
                objData["ValueSettingId"] = 2;
            }
        }
        else if (event.target.name == "isAPIAccess") {
            objData["Description"] = "VDT API (Patient)";
            if (event.target.checked) {
                objData["ValueSettingId"] = 1;
            }
            else {
                objData["ValueSettingId"] = 2;
            }
        }
        if ((event.target.name == "isPatientAccess" && !Clinical_NotesView.params.isPatientAccessModify) || (event.target.name == "isAPIAccess" && !Clinical_NotesView.params.isAPIAccessModify)) {
            Clinical_NotesView.SaveNotesAccessDBCall(objData).done(function (response) {
                response = JSON.parse(response);
                if (response.status) {
                    utility.DisplayMessages(response.Message, 1);
                    if (event.target.name == "isPatientAccess") {
                        Clinical_NotesView.params.isPatientAccessModify = true;
                    }
                    else if (event.target.name == "isAPIAccess") {
                        Clinical_NotesView.params.isAPIAccessModify = true;
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }

        else if ((event.target.name == "isPatientAccess" && Clinical_NotesView.params.isPatientAccessModify) || (event.target.name == "isAPIAccess" && Clinical_NotesView.params.isAPIAccessModify)) {
            Clinical_NotesView.UpdateNotesAccessDBCall(objData).done(function (response) {
                response = JSON.parse(response);
                if (response.status) {
                    utility.DisplayMessages(response.Message, 1);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    SaveNotesAccessDBCall: function (objData) {
        objData["NoteId"] = Clinical_NotesView.params.NotesId;
        objData["PatientId"] = Clinical_NotesView.params.PatientId;
        objData["commandType"] = "save_NotesAccess";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "NotesExtraInfo");
    },

    UpdateNotesAccessDBCall: function (objData) {
        objData["NoteId"] = Clinical_NotesView.params.NotesId;
        objData["PatientId"] = Clinical_NotesView.params.PatientId;
        objData["commandType"] = "update_NotesAccess";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "NotesExtraInfo");
    },

    LoadNotesAccessDBCall: function () {
        var objData = {};
        objData["NoteId"] = Clinical_NotesView.params.NotesId;
        objData["PatientId"] = Clinical_NotesView.params.PatientId;
        objData["commandType"] = "search_notesaccess";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "NotesExtraInfo");
    },

    CreateObjectForBilling: function () {
        BillingInformation.BillingInformationLoad().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                response.BillingInfoCPTFill_JSON = JSON.parse(response.BillingInfoCPTFill_JSON);
                response.BillingInfoICDFill_JSON = JSON.parse(response.BillingInfoICDFill_JSON);

                response.BillingInfoFill_JSON = JSON.parse(response.BillingInfoFill_JSON);

                // MK
                Clinical_ProgressNote.params.PatchBillingInfoId = parseInt(response.BillingInfoFill_JSON.BillingInfoId);
                $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val(response.BillingInfoFill_JSON.BillingInfoId);

                Obj = {
                };
                var ICDs = []

                for (var index in response.BillingInfoICDFill_JSON) {
                    var item = response.BillingInfoICDFill_JSON[index];
                    var currentICD = {
                    };
                    if (item.ICDType == "10") {
                        currentICD.ICDCode9 = '';
                        currentICD.ICDDescription9 = item.ICDCodeDescription.replace(/"/g, "'");;

                        currentICD.ICDCode10 = item.ICDCode;
                        currentICD.ICDDescription10 = item.ICDCodeDescription.replace(/"/g, "'");;

                        currentICD.SNOMEDCode = item.SNOMEDID;
                        currentICD.SNOMEDDescription = item.SNOMEDDescription;
                    }
                    else {
                        currentICD.ICDCode9 = item.ICDCode;
                        currentICD.ICDDescription9 = item.ICDCodeDescription.replace(/"/g, "'");;

                        currentICD.ICDCode10 = '';
                        currentICD.ICDDescription10 = item.ICDCodeDescription.replace(/"/g, "'");;
                        currentICD.SNOMEDCode = item.SNOMEDID;
                        currentICD.SNOMEDDescription = item.SNOMEDDescription;
                    }
                    ICDs.push(currentICD);
                }


                var CPTs = []

                for (var index in response.BillingInfoCPTFill_JSON) {
                    var item = response.BillingInfoCPTFill_JSON[index];
                    var currentCPT = {
                    };
                    currentCPT.CPTCode = item.CPTCode;
                    currentCPT.CPTDescription = item.CPTDescription.replace(/"/g, "'").replace(/&#39;/g, "");
                    currentCPT.Modifier1 = item.Modifier1;
                    currentCPT.Modifier2 = item.Modifier2;
                    currentCPT.Modifier3 = item.Modifier3;
                    currentCPT.Modifier4 = item.Modifier4;
                    currentCPT.DxPointer1 = item.ICDPointer1;
                    currentCPT.DxPointer2 = item.ICDPointer2;
                    currentCPT.DxPointer3 = item.ICDPointer3;
                    currentCPT.DxPointer4 = item.ICDPointer4;
                    currentCPT.UnitsId = item.Units;

                    currentCPT.DOSFrom = item.DOSFrom;
                    currentCPT.DOSTo = item.DOSTo;

                    CPTs.push(currentCPT);

                }

                Obj.CPTs = CPTs;
                Obj.ICDs = ICDs;
                if (BillingInformation.params && BillingInformation.params.length == 0) {
                    BillingInformation.params = Clinical_NotesView.initializeBillingInfoParams();
                }
                //if (isNaN(BillingInformation.params.BillingInfoId) || BillingInformation.params.BillingInfoId == null || BillingInformation.params.BillingInfoId == -1) {

                BillingInformation.LoadAttachecdICDsAndCPTs().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        response.ProblemListFill_JSON = JSON.parse(response.ProblemListFill_JSON);
                        response.ProcedureListFill_JSON = JSON.parse(response.ProcedureListFill_JSON);

                        response.ProblemListFill_JSON = $.grep(response.ProblemListFill_JSON, function (a) {
                            return a.IsNoteLinked == "True";
                        });
                        response.ProcedureListFill_JSON = $.grep(response.ProcedureListFill_JSON, function (a) {
                            return a.IsNoteLinked == "1" || (parseInt(a.VaccineHxId) > 0 || parseInt(a.ImmTherInjectionId) > 0);
                        });


                        var counter = 0;
                        var objData = {
                        };
                        objData["BillingInfoId"] = '-1'
                        objData["commandType"] = "BILLING_INFORMATION_SAVE";
                        objData["NotesId"] = BillingInformation.params.NotesId;
                        objData["PatientId"] = BillingInformation.params.PatientId;
                        objData["ProviderId"] = BillingInformation.params["ProviderId"];
                        objData["VisitId"] = BillingInformation.params.VisitId;
                        objData["Status"] = 'Draft';
                        objData["VisitDate"] = BillingInformation.params.VisitDate;
                        objData.ICDs = [];
                        objData.CPTs = [];

                        for (var i in response.ProblemListFill_JSON) {
                            item = response.ProblemListFill_JSON[i];
                            var ICD = {
                            };
                            ICD.ICDCode9 = item.ICD9;
                            ICD.ICDCode10 = item.ICD10;
                            ICD.ICDDescription9 = item.ICD9_Description;
                            ICD.ICDDescription10 = item.ICD10_Description;
                            ICD.SNOMEDCode = item.SNOMEDID;
                            ICD.SNOMEDDescription = item.SNOMED_DESCRIPTION
                            objData.ICDs.push(ICD);
                        }


                        for (var i in response.ProcedureListFill_JSON) {
                            item = response.ProcedureListFill_JSON[i];
                            var currentCPT = {
                            };
                            currentCPT.CPTCode = item.CPTCode;
                            currentCPT.CPTDescription = item.CPT_DESCRIPTION.replace(/"/g, "'").replace(/&#39;/g, "").replace(/&amp;/g, '&');
                            currentCPT.Modifier1 = item.Modifier;
                            currentCPT.Modifier2 = null;
                            currentCPT.Modifier3 = null;
                            currentCPT.Modifier4 = null;
                            currentCPT.DxPointer1 = null;
                            currentCPT.DxPointer2 = null;
                            currentCPT.DxPointer3 = null;
                            currentCPT.DxPointer4 = null;
                            currentCPT.UnitsId = item.Unit;
                            currentCPT.DOSFrom = item.StartDate;
                            currentCPT.DOSTo = item.EndDate;
                            currentCPT.CPTSNOMEDCodeId = item.SNOMEDID;
                            currentCPT.CPTSNOMEDDescription = item.SNOMED_DESCRIPTION;
                            currentCPT.txtFEE = item.Fee;
                            objData.CPTs.push(currentCPT);
                        }
                        BillingInformation.BillingObj = objData;
                        BillingInformation.BillingInfoSave(objData).done(function (InnerResponse) {
                            InnerResponse = JSON.parse(InnerResponse);
                            if (InnerResponse.status != false) {
                                if (BillingInformation.params.ParentCtrl == "Clinical_NotesView") {
                                    Clinical_NotesView.params.BillingInfoId = InnerResponse.BillingInfoId;
                                }
                                BillingInformation.params.BillingInfoId = InnerResponse.BillingInfoId;
                                BillingInformation.CreateCharge(BillingInformation.BillingObj, true);
                                if (Clinical_NotesView.params.BillingInfoId > 0) {
                                    $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').removeClass('disabled');
                                    $("#" + Clinical_ProgressNote.params.PanelID + " #btnNoteViewCharges").attr("onclick", "Clinical_ProgressNote.LoadVisitDetail('" + Clinical_ProgressNote.params.VisitId + "','" + Clinical_ProgressNote.params.PatientId + "',event)");
                                }
                                else {
                                    $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteViewCharges').addClass('disabled');
                                }
                            }
                        });
                    }
                });


                //}
                //else
                //    BillingInformation.CreateCharge(Obj, true);

                if (parseInt($('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val()) > 0) {
                    Clinical_ProgressNote.Signed_BillingInfo().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }, function (response) {

                    });
                }




                findInDiv.hide(true);
            }
        });
    },

    EditNotes: function () {
        var CurrentTabID = GetCurrentSelectedTab();

        //Start//29/12/2015//Ahmad Raza//Privileges logic implemented
        AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (Clinical_NotesView.params.ParentCtrl == 'mstrTabReports') {
                    var objDeffered = $.Deferred();
                    params["QuickAddPatient"] = true;
                    Clinical_NotesView.UnLoad();
                    $.when(setPatientBanner(Clinical_NotesView.params.PatientId)).then(function () {
                        setTimeout(function () { SelectTab('mstrTabClinical', 'false'); }, 5);
                        setTimeout(function () { params["mode"] = "Edit"; params["ForProgressNote"] = true; params["NotesId"] = Clinical_NotesView.params.NotesId; SelectTab("clinicalTabNotes", "false"); }, 10);
                    });
                    //Start//15-03-2016//Ahmad Raza//showing CDS Alert icon on patient selection
                    $(" #mainForm  li#CDSAlert").show();
                    if (globalAppdata.IsImmunizationAlert != "False") {
                        //$(" #mainForm  li#ImmunizationAlert").show();
                    }
                    $(" #mainForm #hfTriggerLocation").val('FaceSheet');
                }
                else if (Clinical_NotesView.params.ParentCtrl == 'mstrTabDashBoard') {
                    DashBoard.EditProgressNote(Clinical_NotesView.params.NotesId, Clinical_NotesView.params.PatientId);
                    Clinical_NotesView.UnLoad();
                } else if (Clinical_NotesView.params.ParentCtrl == 'clinicalTabNotes') {
                    Clinical_Notes.NotesEdit(Clinical_NotesView.params.NotesId, 'Edit');
                    Clinical_NotesView.UnLoad();
                } else if (Clinical_NotesView.params.ParentCtrl == 'clinicalTabPhoneEncounter') {
                    Clinical_PhoneEncounter.NotesEdit(Clinical_NotesView.params.NotesId, 'Edit');
                    Clinical_NotesView.UnLoad();
                } else if (Clinical_NotesView.params.ParentCtrl == 'Clinical_NotesSearch') {
                    //Clinical_NotesSearch.EditProgressNote(Clinical_NotesView.params.NotesId, Clinical_NotesView.params.PatientId);

                    var objDeffered = $.Deferred();
                    params["QuickAddPatient"] = true;
                    var NotesId = Clinical_NotesView.params.NotesId;
                    var PatientId = Clinical_NotesView.params.PatientId
                    Clinical_NotesView.UnLoad();


                    UnloadActionPan();
                    setTimeout(function () {
                        $.when(setPatientBanner(PatientId)).then(function () {

                            setTimeout(function () { SelectTab('mstrTabClinical', 'false'); }, 5);
                            setTimeout(function () {
                                params["mode"] = "Edit"; params["ForProgressNote"] = true;
                                params["NotesId"] = NotesId;
                                SelectTab("clinicalTabNotes", "false");
                            }, 10);
                        });
                    }, 200);
                } else {
                    UnloadActionPan();
                }

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });

        //End//29/12/2015//Ahmad Raza//Privileges logic implemented
    },
    CoSignNotesOpen: function () {
        var params = [];
        params["ParentCtrl"] = "Clinical_NotesView";
        params["FromAdmin"] = 0;
        params["PCParentCtrl"] = Clinical_NotesView.params.ParentCtrl;
        //params["TemplateLetterId"] = $("#" + SelectLetter_Template.params.PanelID + " #ddlTemplateLetter").val();
        //params["TemplateLetterText"] = $("#" + SelectLetter_Template.params.PanelID + " #ddlTemplateLetter option:selected").text();
        params["mode"] = "Add";
        params["NotesId"] = Clinical_NotesView.params.NotesId;
        LoadActionPan("Clinical_NotesCoSign", params);
    },
    AmendmentNotesOpen: function () {
        AppPrivileges.GetFormPrivileges("Notes_Amendment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {

            if (strMessage == "") {
                var params = [];
                params["ParentCtrl"] = "Clinical_NotesView";
                params["FromAdmin"] = 0;
                //params["TemplateLetterId"] = $("#" + SelectLetter_Template.params.PanelID + " #ddlTemplateLetter").val();
                //params["TemplateLetterText"] = $("#" + SelectLetter_Template.params.PanelID + " #ddlTemplateLetter option:selected").text();
                params["mode"] = "Add";
                params["NotesId"] = Clinical_NotesView.params.NotesId;
                params["PCParentCtrl"] = Clinical_NotesView.params.ParentCtrl;
                //params["PatientId"] = SelectLetter_Template.params["PatientId"];
                LoadActionPan("Clinical_NotesAmendment", params);
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },
    /*Author: M Ahmad Imran, Date: 05/2/2016
    Purpose: for Notes HL7 creation DB Call*/
    NotesHL7CreationDBcall: function (SyndromicType) {
        var objData = {
        };
        objData["SyndromicType"] = SyndromicType;
        objData["PatientId"] = 0;
        if (Clinical_NotesView.params.NotesId != null) {
            objData["NotesId"] = Clinical_NotesView.params.NotesId
        }
        objData["commandType"] = "CREATE_NOTES_HL7";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    sendAsFax: function () {
        var params = [];
        params["PDFBase64"] = Clinical_NotesView.pdf;
        params["ParentCtrl"] = "Clinical_NotesView";
        params["PatientId"] = Clinical_NotesView.params.PatientId;
        LoadActionPan("Batch_FaxSend", params);
    },
    UnLoad: function () {

        if (Clinical_NotesView.params != null && Clinical_NotesView.params.ParentCtrl && Clinical_NotesView.params.ParentCtrlPanelID) {
            if (Clinical_Notes.params.ParentCntrlLoadid == "Schedular") {
                Clinical_Notes.params.mode = "Add";
                Clinical_Notes.params.ParentCntrlLoadid = '';
            }
            UnloadActionPan(Clinical_NotesView.params.ParentCtrl, "Clinical_NotesView", null, Clinical_NotesView.params.ParentCtrlPanelID);
            // Clinical_NotesView.params = null;
        }
        else if (Clinical_NotesView.params != null && Clinical_NotesView.params.ParentCtrl) {
            if (Clinical_NotesView.params.ParentCtrl == "Clinical_NotesView") {
                UnloadActionPan("EncounterChargeCapture", "Clinical_NotesView");
            }
            UnloadActionPan(Clinical_NotesView.params.ParentCtrl, "Clinical_NotesView");
        }
        else {
            UnloadActionPan(null, "Clinical_NotesView");
        }
        if (Clinical_NotesView.params.Grid != null && typeof Clinical_NotesView.params.Grid != typeof undefined) {
            if (Clinical_NotesView.params.Grid == "ModifiedNote") {
                DashBoard.DashBoardModifiedNotesSearch();
            }
        }
    },
    HideShowButtonForPatientDocument: function () {
        $("#" + Clinical_NotesView.params.PanelID + " #noteButtonEdit").hide();
        $("#" + Clinical_NotesView.params.PanelID + " #noteButtonCoSign").hide();
        $("#" + Clinical_NotesView.params.PanelID + " #btnBillingInfo").hide();
        $("#" + Clinical_NotesView.params.PanelID + " #btnViewCharges").hide();
        $("#" + Clinical_NotesView.params.PanelID + " #SyndromicSurveillance").hide();
        $("#" + Clinical_NotesView.params.PanelID + " #noteButtonAmendment").hide();
        $("#" + Clinical_NotesView.params.PanelID + " #noteButtonAmendment").attr('disabled', false);
        $("#" + Clinical_NotesView.params.PanelID + " #noteButtonPrint").show();
        $("#" + Clinical_NotesView.params.PanelID + " #noteButtonPrint").attr('disabled', false);
        $("#" + Clinical_NotesView.params.PanelID + " #noteButtonFax").show();
        $("#" + Clinical_NotesView.params.PanelID + " #noteButtonFax").attr('disabled', false);
        $("#" + Clinical_NotesView.params.PanelID + " #noteChkReviewed").hide();
        $("#" + Clinical_NotesView.params.PanelID + " #isPatientAccess").hide();
        $("#" + Clinical_NotesView.params.PanelID + " #isAPIAccess").hide();
        $("#" + Clinical_NotesView.params.PanelID + " label[for='isPatientAccess']").hide();
        $("#" + Clinical_NotesView.params.PanelID + " label[for='isAPIAccess']").hide();
    },

    SaveDiagnosticResultInPatDocs: function (PrintPDFDataURL, diagnosticResultId) {
        var dfd = $.Deferred();
        Clinical_NotesView.SaveDiagnosticResultInPatDocs_DbCall(PrintPDFDataURL, diagnosticResultId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {                         
                dfd.resolve();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });

        return dfd;
    },
    SaveDiagnosticResultInPatDocs_DbCall: function (PrintPDFDataURL, diagnosticResultId) {

        var objData = new Object();       
        objData["Base64String"] = PrintPDFDataURL;
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["FileType"] = "application/pdf";
        if (diagnosticResultId) {
            objData["ResultId"] = diagnosticResultId;
        }
        else {
            objData["ResultId"] = Clinical_NotesView.GetFirstDiagnosticImagingResultId();
        }
        objData["FolderName"] = "Diagnostic Imag";
        objData["TransitionId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "savediagnosticresultinpatdocs";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "ClinicalNotes");
    },
    GetFirstDiagnosticImagingResultId: function () {

        var diagnosticResultId = $('#' + Clinical_NotesView.params["PanelID"] + " #printcall clinical_diagnosticimagingresults").parent().parent().find('section[id*="Cli_DiagnosticImagingResultDetail_Main"]').map(function () {
            return this.id.replace("Cli_DiagnosticImagingResultDetail_Main", "");
        }).get(0);

        return diagnosticResultId;
    },
}
