VBP_MissingDataAlert = {
    params: [],
    arrMeasureQuestions: [],
    arrMeasureQuestionnaireAnswers: [],
    BillingInformation_DBResponse: new Object(),
    Load: function (params) {
        VBP_MissingDataAlert.params = params;

        if (VBP_MissingDataAlert.params.PanelID != 'pnlVBP_MissingDataAlert') {
            VBP_MissingDataAlert.params.PanelID = VBP_MissingDataAlert.params.PanelID + ' #pnlVBP_MissingDataAlert';
        } else {
            VBP_MissingDataAlert.params.PanelID = 'pnlVBP_MissingDataAlert';
        }
        if (VBP_MissingDataAlert.params.TabID && VBP_MissingDataAlert.params.TabID == "clinicalTabDepression" && VBP_MissingDataAlert.params.patientID) {
            VBP_MissingDataAlert.params.PatientIds = VBP_MissingDataAlert.params.patientID;
            $("#" + VBP_MissingDataAlert.params.PanelID + " #DepressiondetailTable #btnDepressionCancel").css("display", "none");
        }

        if (VBP_MissingDataAlert.params.PHQ2Missing || VBP_MissingDataAlert.params.PHQ9Missing || VBP_MissingDataAlert.params.Depression) {
            if (VBP_MissingDataAlert.params.PHQ2Missing) {
                VBP_MissingDataAlert.GetPatientDetailsMissingDataAlert(VBP_MissingDataAlert.params.PatientIds).done(function () {
                    VBP_MissingDataAlert.GetVBPMeasureQuestionnaireAnswersPHQ2("PHQ2");
                });
            }
            else if (VBP_MissingDataAlert.params.PHQ9Missing) {
                VBP_MissingDataAlert.GetPatientDetailsMissingDataAlert(VBP_MissingDataAlert.params.PatientIds).done(function () {
                    VBP_MissingDataAlert.GetVBPMeasureQuestionnaireAnswersPHQ9("PHQ9");
                });
            }
            else if (VBP_MissingDataAlert.params.Depression) {
                VBP_MissingDataAlert.GetPatientDetailsMissingDataAlert(VBP_MissingDataAlert.params.PatientIds).done(function () {
                    $("#" + VBP_MissingDataAlert.params.PanelID + " #headingTitle").text('Depression Screening Activity');
                    $("#" + VBP_MissingDataAlert.params.PanelID + " #ulTabs li a").text('IA');
                    VBP_MissingDataAlert.GetVBPMeasureQuestionnaireAnswersPHQ9("IA_BMH_4");
                   
                });
            }
        }
        else {
            VBP_MissingDataAlert.GetPatientDetailsMissingDataAlert(VBP_MissingDataAlert.params.PatientIds).done(function () {
                $.when(measure = VBP_MissingDataAlert.GetVBPMeasureQuestionnaireAnswersPHQ2orPHQ9()).then(function () {
                    if (measure.response != "") {
                        var PHQMeasures = measure.response.split(',');
                        if (PHQMeasures.length > 0) {
                            $.each(PHQMeasures, function (key) {
                                if (PHQMeasures[key] == "PHQ2") {
                                    VBP_MissingDataAlert.params.ProviderAssignedPHQ2 = true;
                                }
                                else if (PHQMeasures[key] == "PHQ9") {
                                    VBP_MissingDataAlert.params.ProviderAssignedPHQ9 = true;
                                }
                            });
                            $.each(PHQMeasures, function (key) {
                                if (PHQMeasures[key] == "PHQ2") {
                                    VBP_MissingDataAlert.GetVBPMeasureQuestionnaireAnswersPHQ2("PHQ2");
                                    return false;
                                }
                                else if (PHQMeasures[key] == "PHQ9") {
                                    VBP_MissingDataAlert.GetVBPMeasureQuestionnaireAnswersPHQ9("PHQ9");
                                    return false;
                                }
                            });
                        }
                    }
                });
            });
        }
        
    },
    GetPatientDetailsMissingDataAlert: function (PatientIds, NotesId) {
        var _deff = $.Deferred();
        VBP_MissingDataAlert.getPatientsData_DbCall(PatientIds).done(function (response) {
            response = JSON.parse(response);
            if (response.status && response.AllPatientsCount > 0) {
                $('#' + VBP_MissingDataAlert.params.PanelID + ' #dgvPatientList tbody').empty();
                VBP_MissingDataAlert.bindVBPRows(response);
            }
            _deff.resolve();
        });
        return _deff;
    },
    saveCurrentScorePHQ2: function () {
        var isValidToSave = false;
        var PHQ2Answers4PHQ9_Params = [];
        var selectedOptions = $("#" + VBP_MissingDataAlert.params["PanelID"] + " #PHQ2detailTable option[value!='']:selected");
        if (selectedOptions.length > 0) {
            isValidToSave = true;
            var firstGroupQuestLength = selectedOptions.length;
            var TotalScore = 0;
            var objData = new Object();
            objData.PatientId = VBP_MissingDataAlert.params["PatientIds"];
            objData.MeasureId = "PHQ2";
            objData.CPT = "96127";
            objData.NoteId = VBP_MissingDataAlert.params["NoteId"];
            objData.ProviderId = VBP_MissingDataAlert.params["ProviderId"];
            objData.NoteDate = VBP_MissingDataAlert.params["NoteDate"];
            var currentMeasureQuestionnaires = [];
            $.each(selectedOptions, function (i, option) {
                //calculate score of answers

                var score = option.text.split(" -")[0].trim();
                if ($.isNumeric(score) == true) {
                    TotalScore += parseInt(score);
                }
                var CurrentReasoning = $(option);
                if (CurrentReasoning.attr("MeasureQuestionnaireId") != null && CurrentReasoning.attr("MeasureQuestionnaireId") != "") {
                    if (objData.MeasureQuestionnaireId != null && objData.MeasureQuestionnaireId != "") {
                        var allMeasureQuestionnaireId = objData.MeasureQuestionnaireId + "," + CurrentReasoning.attr("MeasureQuestionnaireId");
                        allMeasureQuestionnaireId = $.unique(allMeasureQuestionnaireId.split(",")).join(",");
                        objData.MeasureQuestionnaireId = allMeasureQuestionnaireId;
                    }
                    else {
                        objData.MeasureQuestionnaireId = CurrentReasoning.attr("MeasureQuestionnaireId");
                    }
                }

                if (CurrentReasoning.attr("QuestionAnswersId") != null && CurrentReasoning.attr("QuestionAnswersId") != "") {
                    PHQ2Answers4PHQ9_Params.push(CurrentReasoning.attr("QuestionAnswersId"));
                    if (objData.QuestionAnswersId != null && objData.QuestionAnswersId != "") {
                        var allQuestionAnswersId = objData.QuestionAnswersId + "," + CurrentReasoning.attr("QuestionAnswersId");
                        allQuestionAnswersId = $.unique(allQuestionAnswersId.split(",")).join(",");
                        objData.QuestionAnswersId = allQuestionAnswersId;
                    }
                    else {
                        objData.QuestionAnswersId = CurrentReasoning.attr("QuestionAnswersId");
                    }
                }

                if (i == (firstGroupQuestLength - 1)) {
                    objData.Score = TotalScore;
                }
            });
            objData.commandType = "save_vbp_reasoning";
            PQRS_Patient_List.saveCQMReasoning_DbCall(objData).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (TotalScore > 2 && (VBP_MissingDataAlert.params.ProviderAssignedPHQ9 || VBP_MissingDataAlert.params.PHQ9Missing)) {
                        $.when(VBP_MissingDataAlert.GetVBPMeasureQuestionnaireAnswersPHQ9("PHQ9")).then(function () {
                            VBP_MissingDataAlert.ReLoad_Procedures();
                        });
                    }
                    else {
                        VBP_MissingDataAlert.CQMWithReasoningLoadAndContinueSignNotes();
                        VBP_MissingDataAlert.UnLoad('save');
                        utility.DisplayMessages(response.message, 2);
                    }
                }
                else {
                    VBP_MissingDataAlert.CQMWithReasoningLoadAndContinueSignNotes();
                    VBP_MissingDataAlert.UnLoad('save');
                    utility.DisplayMessages(response.message, 3);
                }
            });
        }
        else {
            //VBP_PHQ2Questionnaire.PopulateScoreCancelOrClose(VBP_PHQ2Questionnaire.params["NotesId"]);
            utility.DisplayMessages("Please provide information regarding PHQ-2.", 3);
        }
    },
    InsertMissingCptInBillingInfoCpt: function () {
        var deffered = $.Deferred();
        VBP_MissingDataAlert.InsertMissingCptonValidateVBP_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                VBP_MissingDataAlert.BillingInformation_DBResponse = response;
                response.ProcedureListFill_JSON = JSON.parse(response.ProcedureListFill_JSON);
                if (!response.ProcedureListFill_JSON.length > 0) {
                    $.when(Clinical_ProgressNote.CQMWithReasoningLoad(VBP_MissingDataAlert.params.BillingInformation, VBP_MissingDataAlert.params.Obj, VBP_MissingDataAlert.params.customSigMsg, VBP_MissingDataAlert.params.isComponentSelect)).then(function () {
                    });
                    return false;
                }
                var counter = -1;
                $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').each(function (i, item) {
                    i = i + 1;
                    if ($(item).find("#txtCPT" + $(item).attr("id")).val() == "") {
                        counter = $(item).attr("id");
                        counter*=-1;
                    }
                });
                var item = response.ProcedureListFill_JSON[0];
                if ($('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr input[id*="txtCPT"]').length > 0) {
                    VBP_MissingDataAlert.BindCptRowithValue(item, counter);
                    deffered.resolve();
                }
                else
                {
                    $.when(BillingInformation.AddNewChargeRow(null, 'Add')).then(function () {
                        $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').each(function (i, item) {
                            i = i + 1;
                            if ($(item).find("#txtCPT" + $(item).attr("id")).val() == "") {
                                counter = i;
                            }
                        });
                        VBP_MissingDataAlert.BindCptRowithValue(item, counter)
                        deffered.resolve();
                    });
                }
            }
            else
            deffered.resolve();
        });
        return deffered;
    },

    BindCptRowithValue: function (item,counter) {
        var Object = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation');
        $(Object).find("#hfCPTCode-" + counter).val(item.CPTCode);
        $(Object).find("#hfFee-" + counter).val(item.Fee);
        $(Object).find("#hfCPTDescription-" + counter).val(item.CPT_DESCRIPTION);
        $(Object).find("#hfCPTSNOMEDCodeId-" + counter).val(item.SNOMEDID);
        $(Object).find("#hfCPTSNOMEDDescription-" + counter).val(item.SNOMED_DESCRIPTION);
        $(Object).find("#txtModifier1-" + counter).val(item.Modifier);
        $(Object).find("#txtCPT-" + counter).val(item.CPTCode + ' - ' + item.CPT_DESCRIPTION);
        utility.SetAutoCompleteSourceforValidate($(Object).find("#txtCPT-" + counter), item.CPTCode, item.CPT_DESCRIPTION);
        $(Object).find("#-" + counter).attr("IsLabBasedCPT", item.IsLabBasedCPT);
        $(Object).find("#-" + counter).attr("Type", item.BillType);
        $(Object).find("#-" + counter).attr("CheckBoxValue", item.BillingInfoTimeId);
        $(Object).find("#-" + counter).attr("CustomFormId", item.CustomFormId);
        $(Object).find("#hfUnitsId-" + counter).val(item.Unit);
        $(Object).find("#txtUnits-" + counter).val(item.Unit);
        $(Object).find("#hfStartDate-" + counter).val(item.StartDate);
        $(Object).find("#hfEndDate-" + counter).val(item.EndDate);
        if (item.Unit != null && item.Unit != '') {
            if (isNaN(parseInt(item.Unit)))
                item.Unit = '1';
            else {
                unit = parseInt(item.Unit);
                if (unit < 1 || unit > 99)
                    item.Unit = '1';
            }
            try {
                BillingInformation.loadCPTPointers(Object, item, counter, item);
            } catch (ex) {
                console.log(ex);
            }
        }
        else
            BillingInformation.loadCPTPointers(Object, item, counter, item);

        var currentCPT = {
        };
        currentCPT.CPTCode = item.CPTCode;
        currentCPT.CPTDescription = item.CPT_DESCRIPTION.replace(/"/g, "'").replace(/&#39;/g, "");
        currentCPT.Modifier1 = item.Modifier;
        currentCPT.Modifier2 = "";
        currentCPT.Modifier3 = "";
        currentCPT.Modifier4 = "";

        var icd_cods = item.ICDCodes.split(',');
        var pinter1 = icd_cods.length > 0 && icd_cods[0] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[0], VBP_MissingDataAlert.params.Obj.ICDs) : '1';
        var pinter2 = icd_cods.length > 1 && icd_cods[1] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[1], VBP_MissingDataAlert.params.Obj.ICDs) : '';
        var pinter3 = icd_cods.length > 2 && icd_cods[2] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[2], VBP_MissingDataAlert.params.Obj.ICDs) : '';
        var pinter4 = icd_cods.length > 3 && icd_cods[3] != '' ? Clinical_ProgressNote.Getpointers(icd_cods[3], VBP_MissingDataAlert.params.Obj.ICDs) : '';
        currentCPT.DxPointer1 = pinter1;
        currentCPT.DxPointer2 = pinter2;
        currentCPT.DxPointer3 = pinter3;
        currentCPT.DxPointer4 = pinter4;
        currentCPT.UnitsId = item.Unit;
        currentCPT.Unit = item.Unit;
        currentCPT.DOSFrom = item.StartDate;
        currentCPT.DOSTo = item.EndDate;
        currentCPT.CPTSNOMEDCodeId = item.SNOMEDID;
        currentCPT.CPTSNOMEDDescription = item.SNOMED_DESCRIPTION;
        currentCPT.txtFEE = item.Fee;
        currentCPT.Fee = item.Fee;
        if (i == 0) {
            currentCPT.Copay = item.Copay;
        } else {
            currentCPT.Copay = "";
        }
        if (item.ExpectedFee && item.ExpectedFee != "0") {
            currentCPT.hfExpectedFee = item.ExpectedFee;
            currentCPT.Expectedfee = item.ExpectedFee;
        }
        else {
            currentCPT.hfExpectedFee = "0.00";
            currentCPT.Expectedfee = "0.00";
        }
        currentCPT.PatCharges = item.PatCharges;
        currentCPT.Inscharges = item.Inscharges;
        var self = $('#' + BillingInformation.params.PanelID);
        var POSCode = self.find('#txtPOS').val() == "" ? 0 : self.find('#txtPOS').val();
        currentCPT.POS = VBP_MissingDataAlert.params.POS ? VBP_MissingDataAlert.params.POS : POSCode;
        VBP_MissingDataAlert.params.Obj.CPTs.push(currentCPT);
        VBP_MissingDataAlert.putBillingInformationCPTid(VBP_MissingDataAlert.BillingInformation_DBResponse);
    },
    putBillingInformationCPTid: function (response) {
        var parsedObject = null;
        var BillingInfoObject = null;
        var BillingInfoCptObject = null;
        var rowId = 0;
        var Object = $('#' + BillingInformation.params.PanelID + ' #tblBillingInformation');
        try {
            if (response.BillingInfoFillJson) {
                parsedObject = JSON.parse(response.BillingInfoFillJson);
                if (parsedObject) {
                    BillingInfoObject = JSON.parse(parsedObject.BillingInfoFill_JSON);
                    if (BillingInfoObject) {
                        BillingInfoCptObject = JSON.parse(parsedObject.BillingInfoCPTFill_JSON);
                        if (BillingInfoCptObject) {
                            $(BillingInfoCptObject).each(function (index, item) {
                                $('#' + BillingInformation.params.PanelID + ' #dgvBillVisitCharge tbody tr').each(function (i, tm) {
                                    item.DOSTo = new Date(item.DOSTo);
                                    item.DOSFrom = new Date(item.DOSFrom);
                                    var dayTo = item.DOSTo.getDate();
                                    var monthTo = item.DOSTo.getMonth() + 1;
                                    var yearTo = item.DOSTo.getFullYear();
                                    if (dayTo < 10)
                                        dayTo = "0" + dayTo;
                                    if (monthTo < 10)
                                        monthTo = "0" + monthTo;
                                    var dateTo = monthTo + "/" + dayTo + "/" + yearTo;
                                    var dayFrom = item.DOSFrom.getDate();
                                    var monthFrom = item.DOSFrom.getMonth() + 1;
                                    var yearFrom = item.DOSFrom.getFullYear();
                                    if (dayFrom < 10)
                                        dayFrom = "0" + dayFrom;
                                    if (monthFrom < 10)
                                        monthFrom = "0" + monthFrom;
                                    var dateFrom = monthFrom + "/" + dayFrom + "/" + yearFrom;
                                    item.DOSTo = dateTo;
                                    item.DOSFrom = dateFrom;
                                    if (BillingInformation.params.ParentCtrl == "billTabOutOfOfficeVisits") {
                                        if (item.CPTCode && $(this).find("input:hidden[id*='hfCPTCode']").val() == item.CPTCode && item.CPTCodeDescription && $(this).find("input:hidden[id*='hfCPTDescription']").val() == item.CPTCodeDescription && $(this).find("input[id*='dtpDOSFrom']").val() == item.DOSFrom && $(this).find("input[id*='dtpDOSTo']").val() == item.DOSTo) {
                                            rowId = $(this).attr("id");
                                            $(Object).find("#hfBillingInfoCPTId" + rowId).val(item.BillingInfoCPTId ? item.BillingInfoCPTId : "");
                                            return false;
                                        }
                                    }
                                    else {
                                        if (item.CPTCode && $(this).find("input:hidden[id*='hfCPTCode']").val() == item.CPTCode && item.CPTCodeDescription && $(this).find("input:hidden[id*='hfCPTDescription']").val() == item.CPTCodeDescription) {
                                            rowId = $(this).attr("id");
                                            $(Object).find("#hfBillingInfoCPTId" + rowId).val(item.BillingInfoCPTId ? item.BillingInfoCPTId : "");
                                            return false;
                                        }
                                    }

                                });
                            });
                        }
                    }
                }
            }
        } catch (e) {
            console.log(e);
        }
    },

    InsertMissingCptonValidateVBP_DBCall: function () {
        var objData = new Object();
        objData["BillingInfoId"] = BillingInformation.params.BillingInfoId;
        objData["NotesId"] = BillingInformation.params.NotesId;
        objData["commandType"] = "INSERT_MISSING_CPT_IN_BILLING_INFO_CPT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");
    },
    CQMWithReasoningLoadAndContinueSignNotes: function () {
        if (VBP_MissingDataAlert.params.ParentCtrl == 'mstrTabDashBoard') {
            DashBoard.CQMWithReasoningLoad(VBP_MissingDataAlert.params.NoteId, VBP_MissingDataAlert.params.PatientId, VBP_MissingDataAlert.params.ProviderId, VBP_MissingDataAlert.params.isComponentSelect, VBP_MissingDataAlert.params.VisitDate, VBP_MissingDataAlert.params.BillingInfoId, VBP_MissingDataAlert.params.AppointmentDate, VBP_MissingDataAlert.params.VisitId, VBP_MissingDataAlert.params.NoteDate, VBP_MissingDataAlert.params.PatientTypeId, VBP_MissingDataAlert.params.FacilityId, VBP_MissingDataAlert.params.POS, VBP_MissingDataAlert.params.RefProviderID, VBP_MissingDataAlert.params.IsPhoneEncounter);
        }
        else if (VBP_MissingDataAlert.params.ParentCtrl == 'Clinical_NotesSearch') {
            Clinical_NotesSearch.CQMWithReasoningLoad(VBP_MissingDataAlert.params.NoteId, VBP_MissingDataAlert.params.event, VBP_MissingDataAlert.params.NoteStatus, VBP_MissingDataAlert.params.isComponentSelect, VBP_MissingDataAlert.params.VisitDate, VBP_MissingDataAlert.params.PatientId, VBP_MissingDataAlert.params.ProviderId, VBP_MissingDataAlert.params.BillingInfoId, VBP_MissingDataAlert.params.AppointmentDate, VBP_MissingDataAlert.params.VisitId, VBP_MissingDataAlert.params.NoteDate, VBP_MissingDataAlert.params.PatientTypeId, VBP_MissingDataAlert.params.FacilityId, VBP_MissingDataAlert.params.POS, VBP_MissingDataAlert.params.RefProviderID)
        }
        else if (VBP_MissingDataAlert.params.ParentCtrl == 'clinicalTabNotes') {
            Clinical_Notes.CQMWithReasoningLoad(VBP_MissingDataAlert.params.NoteId, VBP_MissingDataAlert.params.event, VBP_MissingDataAlert.params.NoteStatus, VBP_MissingDataAlert.params.isComponentSelect, VBP_MissingDataAlert.params.VisitDate, VBP_MissingDataAlert.params.PatientId, VBP_MissingDataAlert.params.ProviderId, VBP_MissingDataAlert.params.BillingInfoId, VBP_MissingDataAlert.params.AppointmentDate, VBP_MissingDataAlert.params.VisitId, VBP_MissingDataAlert.params.NoteDate, VBP_MissingDataAlert.params.PatientTypeId, VBP_MissingDataAlert.params.FacilityId, VBP_MissingDataAlert.params.POS, VBP_MissingDataAlert.params.RefProviderID)
        }
        else if (VBP_MissingDataAlert.params.ParentCtrl == 'clinicalTabProgressNote') {
            $.when(Clinical_ProgressNote.CQMWithReasoningLoad(VBP_MissingDataAlert.params.BillingInformation, VBP_MissingDataAlert.params.Obj, VBP_MissingDataAlert.params.customSigMsg, VBP_MissingDataAlert.params.isComponentSelect, VBP_MissingDataAlert.params.NoteId, VBP_MissingDataAlert.params.IsFromProgressNote));
            VBP_MissingDataAlert.ReLoad_Procedures();

        }
        else if (VBP_MissingDataAlert.params.ParentCtrl == 'BillingInformation') {
            $.when(VBP_MissingDataAlert.InsertMissingCptInBillingInfoCpt()).then(function () {
                $.when(Clinical_ProgressNote.CQMWithReasoningLoad(VBP_MissingDataAlert.params.BillingInformation, VBP_MissingDataAlert.params.Obj, VBP_MissingDataAlert.params.customSigMsg, VBP_MissingDataAlert.params.isComponentSelect)).then(function () {
                });
            });
            VBP_MissingDataAlert.ReLoad_Procedures();
        }
        else if (VBP_MissingDataAlert.params.ParentCtrl == 'Clinical_NotesView') {
            Clinical_NotesView.CQMWithReasoningLoad();
        }
    },
    ReLoad_Procedures: function () {
        Clinical_ProgressNote.getNoteComponentHTML(VBP_MissingDataAlert.params.NoteId, 'Procedures').done(function (res) {
            Clinical_ProgressNote.updateNoteComponentHTML(res, 'Procedures');
            Clinical_Procedures.CalculateVBPSocreAndAppend(Clinical_ProgressNote.params.NotesId, true).done(function (response) {
                response = JSON.parse(response);
                var NoteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
                if (response.status != false) {
                    $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('section').each(function () {
                        if ($(this).find("ul li span strong").length > 0) {
                            $(this).find("ul li span strong").closest('li').remove();
                        }
                        if ($(this).find("ul li a").length > 0 && $(this).find("ul li a") && $(this).find("ul li a").attr("onclick").indexOf('VBP_PHQQuestionnaire') > -1) {
                            $(this).find("ul li a").closest('li').remove();
                            $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + response.ProceudereID + ' ul').append(response.PHQSoapText);
                        }
                        else {
                            var SecId = $(this).attr("id"); var responseID = 'Cli_Procedures_Main' + response.ProceudereID;
                            if (SecId == responseID) {
                                $(NoteHTMLCtrl + ' clinical_procedures').parent().parent().find('#Cli_Procedures_Main' + response.ProceudereID + ' ul').append(response.PHQSoapText);
                            }
                        }
                    });
                    Clinical_ProgressNote.saveComponentSOAPText('Procedures', true);
                }
            });
        });
    },

    saveCurrentScorePHQ9: function () {
        var isValidToSave = false;
        var selectedOptions = $("#" + VBP_MissingDataAlert.params["PanelID"] + " #PHQ9detailTable option[value!='']:selected");
        var selectedRadio = $("#" + VBP_MissingDataAlert.params["PanelID"] + " #PHQ9detailTable input[type='radio']:checked");
        if (selectedOptions.length > 0 && selectedRadio.length > 0) {
            isValidToSave = true;
            var firstGroupQuestLength = selectedOptions.length;
            var secGroupQuestLength = selectedRadio.length;
            var TotalScore = 0;
            var objData = new Object();
            objData.PatientId = VBP_MissingDataAlert.params["PatientIds"];
            objData.MeasureId = "PHQ9";
            objData.CPT = "96127";
            objData.NoteId = VBP_MissingDataAlert.params["NoteId"];
            objData.ProviderId = VBP_MissingDataAlert.params["ProviderId"];
            objData.NoteDate = VBP_MissingDataAlert.params["NoteDate"];
            $.each(selectedOptions, function (i, option) {
                //calculate score of answers
                var score = option.text.split(" -")[0].trim();
                if ($.isNumeric(score) == true) {
                    TotalScore += parseInt(score);
                }
                var CurrentReasoning = $(option);
                if (CurrentReasoning.attr("MeasureQuestionnaireId") != null && CurrentReasoning.attr("MeasureQuestionnaireId") != "") {
                    if (objData.MeasureQuestionnaireId != null && objData.MeasureQuestionnaireId != "") {
                        var allMeasureQuestionnaireId = objData.MeasureQuestionnaireId + "," + CurrentReasoning.attr("MeasureQuestionnaireId");
                        allMeasureQuestionnaireId = $.unique(allMeasureQuestionnaireId.split(",")).join(",");
                        objData.MeasureQuestionnaireId = allMeasureQuestionnaireId;
                    }
                    else {
                        objData.MeasureQuestionnaireId = CurrentReasoning.attr("MeasureQuestionnaireId");
                    }
                }

                if (CurrentReasoning.attr("QuestionAnswersId") != null && CurrentReasoning.attr("QuestionAnswersId") != "") {
                    if (objData.QuestionAnswersId != null && objData.QuestionAnswersId != "") {
                        var allQuestionAnswersId = objData.QuestionAnswersId + "," + CurrentReasoning.attr("QuestionAnswersId");
                        allQuestionAnswersId = $.unique(allQuestionAnswersId.split(",")).join(",");
                        objData.QuestionAnswersId = allQuestionAnswersId;
                    }
                    else {
                        objData.QuestionAnswersId = CurrentReasoning.attr("QuestionAnswersId");
                    }
                }

                if (i == (firstGroupQuestLength - 1)) {
                    objData.Score = TotalScore;
                }
            });

            $.each(selectedRadio, function (i, option) {
                var CurrentReasoning = $(option);
                if (CurrentReasoning.attr("MeasureQuestionnaireId") != null && CurrentReasoning.attr("MeasureQuestionnaireId") != "") {
                    if (objData.MeasureQuestionnaireId != null && objData.MeasureQuestionnaireId != "") {
                        var allMeasureQuestionnaireId = objData.MeasureQuestionnaireId + "," + CurrentReasoning.attr("MeasureQuestionnaireId");
                        allMeasureQuestionnaireId = $.unique(allMeasureQuestionnaireId.split(",")).join(",");
                        objData.MeasureQuestionnaireId = allMeasureQuestionnaireId;
                    }
                    else {
                        objData.MeasureQuestionnaireId = CurrentReasoning.attr("MeasureQuestionnaireId");
                    }
                }

                if (CurrentReasoning.attr("QuestionAnswersId") != null && CurrentReasoning.attr("QuestionAnswersId") != "") {
                    if (objData.QuestionAnswersId != null && objData.QuestionAnswersId != "") {
                        var allQuestionAnswersId = objData.QuestionAnswersId + "," + CurrentReasoning.attr("QuestionAnswersId");
                        allQuestionAnswersId = $.unique(allQuestionAnswersId.split(",")).join(",");
                        objData.QuestionAnswersId = allQuestionAnswersId;
                    }
                    else {
                        objData.QuestionAnswersId = CurrentReasoning.attr("QuestionAnswersId");
                    }
                }
            });
            objData.commandType = "save_vbp_reasoning";
            PQRS_Patient_List.saveCQMReasoning_DbCall(objData).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    VBP_MissingDataAlert.CQMWithReasoningLoadAndContinueSignNotes();
                    VBP_MissingDataAlert.UnLoad('save');
                }
                else {
                    VBP_MissingDataAlert.CQMWithReasoningLoadAndContinueSignNotes();
                    VBP_MissingDataAlert.UnLoad('save');
                    utility.DisplayMessages(response.message, 3);
                }
            });

        }
        else {
            utility.DisplayMessages("Please provide information regarding PHQ-9.", 3);
        }
    },
    bindVBPRows: function (response) {
        var selectedPatientsData = JSON.parse(response.AllPatientsLoad_JSON);
        $.each(selectedPatientsData, function (i, item) {
            var $row = $('<tr/>');
            $row.attr('id', "dgvPatientListRow_" + item.PatientId + "," + 41819);
            $row.attr('PatientId', item.PatientId);
            //VBP_MissingDataAlert.params.NoteId
            //$row.attr('onclick', "VBP_MissingDataAlert.showCurrentNoteReasoning('" + 41819 + "','" + item.PatientId + "');");
            $row.append('<td style="color:red">' + item.AccountNumber + '</td><td style="color:red">' + item.LastName + '</td><td style="color:red">' + item.FirstName + '</td><td style="color:red">' + utility.RemoveTimeFromDate(null, item.DOB) + '</td><td style="color:red">' + item.Age + '</td><td style="color:red">' + item.Sex + '</td>');
            $('#' + VBP_MissingDataAlert.params.PanelID + ' #dgvPatientList tbody').append($row);
        });
    },
    GetVBPMeasureQuestionnaireAnswersPHQ2orPHQ9: function () {
        var _deff = $.Deferred();

        var PHQMeasures = "";
        //VBP_MissingDataAlert.params.ProviderId // 219
        VBP_MissingDataAlert.LoadMeasuresByProviderid_DbCall(VBP_MissingDataAlert.params.ProviderId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                PHQMeasures = response.Measures;
            }
            _deff.response = PHQMeasures;
            _deff.resolve();
        });
        return _deff;
    },
    GetVBPMeasureQuestionnaireAnswersPHQ2: function (MeasureId) {
        var _deff = $.Deferred();

        PQRS_Patient_List.loadVBPMeasureQuestionnaire_DbCall(MeasureId).done(function (response) {
            response = JSON.parse(response);
            VBP_MissingDataAlert.arrMeasureQuestionnaireAnswers = JSON.parse(response.VBPMeasureQuestionnaireAnswersLoad_JSON);
            var currentMeasureQuestionnaires = [];
            $.each(VBP_MissingDataAlert.arrMeasureQuestionnaireAnswers, function (i, item) {
                currentMeasureQuestionnaires.push(item.Question);
            });
            VBP_MissingDataAlert.arrMeasureQuestions = $.unique(currentMeasureQuestionnaires);
            var QuestIds = currentMeasureQuestionnaires.join(",");
            var objFirstGroupQuestion = $("#pnlVBP_MissingDataAlert #PHQ2detailTable #divQuestionGroup1").empty();
            $("#pnlVBP_MissingDataAlert #PHQ2detailTable #divQuestionGroup1").append('<tr><td colspan="2"><b>Over the last 2 weeks, how often have you been bothered by any of the following problems?<span class="required">*</span></b></td></tr>)');

            $.each(VBP_MissingDataAlert.arrMeasureQuestions, function (i, Question) {
                var currentQuestionAnswers = $.grep(VBP_MissingDataAlert.arrMeasureQuestionnaireAnswers, function (item, indx) {
                    return item.Question == Question;
                });
                var lblQuestion = '<td><label class="control-label">' + Question + '</label></td>';
                var ddlAnswer = '<td><select id="ddlAnswerId' + (i + 1) + '" name="AnswerId" class="form-control" onchange=""></select></td>';
                var finlstr = "<tr>" + lblQuestion + ddlAnswer + "</tr>";
                objFirstGroupQuestion.append(finlstr);
                var objddlAnswer = objFirstGroupQuestion.find("select#ddlAnswerId" + (i + 1));
                VBP_PHQQuestionnaire.LoadVBPAnswer(MeasureId, objddlAnswer, currentQuestionAnswers);
                $("#pnlVBP_MissingDataAlert #PHQ9detailTable").addClass('hidden');
            });
            _deff.resolve();
        });
        return _deff;
    },
    LoadMeasuresByProviderid_DbCall: function (ProviderID) {
        var objData = {};
        objData["ProviderId"] = ProviderID;
        objData["commandType"] = "load_provider_vbp_measures";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    GetVBPMeasureQuestionnaireAnswersPHQ9: function (MeasureId) {
        var deffered = $.Deferred();
        VBP_MissingDataAlert.loadVBPMeasureQuestionnaire_DbCall(MeasureId).done(function (response) {
            response = JSON.parse(response);
            VBP_MissingDataAlert.arrMeasureQuestionnaireAnswers = JSON.parse(response.VBPMeasureQuestionnaireAnswersLoad_JSON);
            var currentMeasureQuestionnaires = [];
            $.each(VBP_MissingDataAlert.arrMeasureQuestionnaireAnswers, function (i, item) {
                currentMeasureQuestionnaires.push(item.Question);
            });
            VBP_MissingDataAlert.arrMeasureQuestions = $.unique(currentMeasureQuestionnaires);
            var QuestIds = currentMeasureQuestionnaires.join(",");
            $("#pnlVBP_MissingDataAlert #PHQ9detailTable").removeClass('hidden');
            $("#pnlVBP_MissingDataAlert #PHQ2detailTable #divPHQ2btns").addClass('hidden');
            $("#pnlVBP_MissingDataAlert #DepressiondetailTable").addClass('hidden');
            if (VBP_MissingDataAlert.params.PHQ9Missing)
                $("#pnlVBP_MissingDataAlert #PHQ2detailTable").addClass('hidden');
            var objFirstGroupQuestion = $("#pnlVBP_MissingDataAlert #PHQ9detailTable #divQuestionGroup1").empty();
            if (VBP_MissingDataAlert.params.Depression) {
                $("#pnlVBP_MissingDataAlert #DepressiondetailTable").removeClass('hidden');
                $("#pnlVBP_MissingDataAlert #PHQ9detailTable").addClass('hidden');
                $("#pnlVBP_MissingDataAlert #PHQ2detailTable").addClass('hidden');
                objFirstGroupQuestion = $("#pnlVBP_MissingDataAlert #DepressiondetailTable #divQuestionGroup1").empty();
                $("#pnlVBP_MissingDataAlert #DepressiondetailTable #divQuestionGroup1").append('<tr><td colspan="2"><b>Over the last 2 weeks, how often have you been bothered by any of the following problems?<span class="required">*</span></b></td></tr>)');
            }
            $("#pnlVBP_MissingDataAlert #PHQ9detailTable #divQuestionGroup1").append('<tr><td colspan="2"><b>Over the last 2 weeks, how often have you been bothered by any of the following problems?<span class="required">*</span></b></td></tr>)');
            var objSecondGroupQuestion = $("#pnlVBP_MissingDataAlert #PHQ9detailTable #divQuestionGroup2").empty();
            var def = [];
            $.each(VBP_MissingDataAlert.arrMeasureQuestions, function (i, Question) {
                var currentQuestionAnswers = $.grep(VBP_MissingDataAlert.arrMeasureQuestionnaireAnswers, function (item, indx) {
                    return item.Question == Question;
                });
                $.each(currentQuestionAnswers, function (i,obj) {
                    if (!$("#" + VBP_MissingDataAlert.params["PanelID"] + " #DepressiondetailTable #Comments").val() && obj.Comments) {
                        $("#" + VBP_MissingDataAlert.params["PanelID"] + " #DepressiondetailTable #Comments").val(obj.Comments);
                 }
                });
                if (i < 9) {

                    var lblQuestion = '<td><label class="control-label">' + Question + '</label></td>';
                    var ddlAnswer = '<td><select id="ddlAnswerId' + (i + 1) + '" name="AnswerId" class="form-control" onchange=""></select></td>';
                    var finlstr = "<tr>" + lblQuestion + ddlAnswer + "</tr>";
                    objFirstGroupQuestion.append(finlstr);
                    var objddlAnswer = objFirstGroupQuestion.find("select#ddlAnswerId" + (i + 1));
                    if (VBP_MissingDataAlert.params.Depression){
                        VBP_PHQQuestionnaire.LoadVBPAnswer("IA_BMH_4", objddlAnswer, currentQuestionAnswers);
                    }else{
                        VBP_PHQQuestionnaire.LoadVBPAnswer("PHQ9", objddlAnswer, currentQuestionAnswers);}
                }
                else {
                    var lblQuestion = '<td colspan="2"><label class="control-label col-xs-12"><strong>' + Question + '?<span class="required">*</span></strong></label>';
                    if (VBP_MissingDataAlert.params.Depression) {
                        var radAnswers = VBP_PHQQuestionnaire.LoadVBPAnswerAsRadioBtn("IA_BMH_4", null, currentQuestionAnswers);
                    } else {
                        var radAnswers = VBP_PHQQuestionnaire.LoadVBPAnswerAsRadioBtn("PHQ9", null, currentQuestionAnswers);
                    }
                    
                    var finlstr = "<tr>" + lblQuestion + radAnswers + "</td></tr>";
                    objFirstGroupQuestion.append(finlstr);
                }
            });
            $.when.apply($, def).done(function ($n) {
                deffered.resolve();
            });
        });
        return deffered;
    },
    loadVBPMeasureQuestionnaire_DbCall: function (MeasureId) {
        var objData = {};
        objData["MeasureId"] = MeasureId;
        objData["NoteId"] = VBP_MissingDataAlert.params.NoteId;
        objData["PatientId"] = VBP_MissingDataAlert.params.PatientIds;
        objData["commandType"] = "load_vbp_measurequestionnaireAnswers";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    getPatientsData_DbCall: function (PatientIds) {
        var objData = {};
        objData["PatientIds"] = PatientIds;
        objData["commandType"] = "load_patients_cqm";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    saveCurrentScoreDepression: function () {
        var isValidToSave = false;
        var selectedOptions = $("#" + VBP_MissingDataAlert.params["PanelID"] + " #DepressiondetailTable").find("option[value!='']:selected");
        var selectedRadio = $("#" + VBP_MissingDataAlert.params["PanelID"] + " #DepressiondetailTable").find("input[type='radio']:checked");
        if (selectedOptions.length > 0 || selectedRadio.length > 0) {
            isValidToSave = true;
            var firstGroupQuestLength = selectedOptions.length;
            var secGroupQuestLength = selectedRadio.length;
            var TotalScore = 0;
            var objData = new Object();
            objData.PatientId = VBP_MissingDataAlert.params["PatientIds"];
            objData.MeasureId = "IA_BMH_4";
            objData.NoteId = VBP_MissingDataAlert.params["NoteId"];
            objData.ProviderId = VBP_MissingDataAlert.params["ProviderId"];
            //objData.NoteDate = VBP_MissingDataAlert.params["NoteDate"];
            $.each(selectedOptions, function (i, option) {
                //calculate score of answers
                var score = option.text.split(" -")[0].trim();
                if ($.isNumeric(score) == true) {
                    TotalScore += parseInt(score);
                }
                var CurrentReasoning = $(option);
                if (CurrentReasoning.attr("MeasureQuestionnaireId") != null && CurrentReasoning.attr("MeasureQuestionnaireId") != "") {
                    if (objData.MeasureQuestionnaireId != null && objData.MeasureQuestionnaireId != "") {
                        var allMeasureQuestionnaireId = objData.MeasureQuestionnaireId + "," + CurrentReasoning.attr("MeasureQuestionnaireId");
                        allMeasureQuestionnaireId = $.unique(allMeasureQuestionnaireId.split(",")).join(",");
                        objData.MeasureQuestionnaireId = allMeasureQuestionnaireId;
                    }
                    else {
                        objData.MeasureQuestionnaireId = CurrentReasoning.attr("MeasureQuestionnaireId");
                    }
                }

                if (CurrentReasoning.attr("QuestionAnswersId") != null && CurrentReasoning.attr("QuestionAnswersId") != "") {
                    if (objData.QuestionAnswersId != null && objData.QuestionAnswersId != "") {
                        var allQuestionAnswersId = objData.QuestionAnswersId + "," + CurrentReasoning.attr("QuestionAnswersId");
                        allQuestionAnswersId = $.unique(allQuestionAnswersId.split(",")).join(",");
                        objData.QuestionAnswersId = allQuestionAnswersId;
                    }
                    else {
                        objData.QuestionAnswersId = CurrentReasoning.attr("QuestionAnswersId");
                    }
                }

                if (i == (firstGroupQuestLength - 1)) {
                    objData.Score = TotalScore;
                }
            });

            $.each(selectedRadio, function (i, option) {
                var CurrentReasoning = $(option);
                if (CurrentReasoning.attr("MeasureQuestionnaireId") != null && CurrentReasoning.attr("MeasureQuestionnaireId") != "") {
                    if (objData.MeasureQuestionnaireId != null && objData.MeasureQuestionnaireId != "") {
                        var allMeasureQuestionnaireId = objData.MeasureQuestionnaireId + "," + CurrentReasoning.attr("MeasureQuestionnaireId");
                        allMeasureQuestionnaireId = $.unique(allMeasureQuestionnaireId.split(",")).join(",");
                        objData.MeasureQuestionnaireId = allMeasureQuestionnaireId;
                    }
                    else {
                        objData.MeasureQuestionnaireId = CurrentReasoning.attr("MeasureQuestionnaireId");
                    }
                }

                if (CurrentReasoning.attr("QuestionAnswersId") != null && CurrentReasoning.attr("QuestionAnswersId") != "") {
                    if (objData.QuestionAnswersId != null && objData.QuestionAnswersId != "") {
                        var allQuestionAnswersId = objData.QuestionAnswersId + "," + CurrentReasoning.attr("QuestionAnswersId");
                        allQuestionAnswersId = $.unique(allQuestionAnswersId.split(",")).join(",");
                        objData.QuestionAnswersId = allQuestionAnswersId;
                    }
                    else {
                        objData.QuestionAnswersId = CurrentReasoning.attr("QuestionAnswersId");
                    }
                }
            });
            objData.commandType = "save_vbp_depression";
            objData.MeasureType = "Depression screening";
            objData.Comments = $("#" + VBP_MissingDataAlert.params["PanelID"] + " #DepressiondetailTable #Comments").val();
                
            PQRS_Patient_List.saveCQMReasoning_DbCall(objData).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (VBP_MissingDataAlert.params.TabID && VBP_MissingDataAlert.params.TabID != "clinicalTabDepression"){
                        VBP_MissingDataAlert.UnLoad('save');
                    }
                    else if (VBP_MissingDataAlert.params.TabID && VBP_MissingDataAlert.params.TabID == "clinicalTabDepression") {
                        utility.DisplayMessages(response.message, 1);
                        if (response.MUAlertsCount && response.MUAlertsCount > 0)
                            utility.toggelMU3Alerts(true, response.MUAlertsCount);
                        else
                            utility.toggelMU3Alerts(false, 0);
                    }
                }
                else {
                    if (VBP_MissingDataAlert.params.TabID && VBP_MissingDataAlert.params.TabID != "clinicalTabDepression") {
                        VBP_MissingDataAlert.UnLoad('save');
                        utility.DisplayMessages(response.message, 3);
                    }
                }
            });

        }
        else {
            utility.DisplayMessages("Please provide information regarding Depression.", 3);
        }
    },
    UnLoad: function (caller) {
        if (VBP_MissingDataAlert.params != null && VBP_MissingDataAlert.params.ParentCtrl != null) {
            if (VBP_MissingDataAlert.params.ParentCtrl == 'BillingInformation') {
                if (caller && caller == 'save') {
                    UnloadActionPan(VBP_MissingDataAlert.params.ParentCtrl, 'VBP_MissingDataAlert');
                }                
                else {
                    UnloadActionPan(VBP_MissingDataAlert.params.ParentCtrl, 'VBP_MissingDataAlert');
                    UnloadActionPan(BillingInformation.params["ParentCtrl"], "BillingInformation");
                }
            }
            else if (VBP_MissingDataAlert.params.ParentCtrl == 'Clinical_ProblemLists') {
                UnloadActionPan(VBP_MissingDataAlert.params.ParentCtrl, 'VBP_MissingDataAlert', null, Clinical_ProblemLists.params.PanelID);
            }
        else
            UnloadActionPan(VBP_MissingDataAlert.params.ParentCtrl, 'VBP_MissingDataAlert');
        }
        else
            UnloadActionPan(null, 'VBP_MissingDataAlert');
        if (VBP_MissingDataAlert.params.ParentCtrl == 'BillingInformation' || VBP_MissingDataAlert.params.ParentCtrl == 'clinicalTabProgressNote')
            Clinical_ProgressNote.params.isVBPExists = 0;
        setPatientBanner(VBP_MissingDataAlert.params.PatientIds);
    },
}