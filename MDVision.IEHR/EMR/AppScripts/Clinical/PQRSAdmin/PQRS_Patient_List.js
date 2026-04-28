//Author: Ahmad Raza
//File:   PQRS_Patient_List
//Date:   16-08-2016
PQRS_Patient_List = {
    params: [],
    bIsFirstLoad: true,
    arrReasonTextBox: [],
    arrMeasureQuestionnaireAnswers: [],
    arrMeasureQuestions: [],
    Load: function (params) {
        PQRS_Patient_List.params = params;

        if (PQRS_Patient_List.params.PanelID != 'pnlPatientList') {
            PQRS_Patient_List.params.PanelID = PQRS_Patient_List.params.PanelID + ' #pnlPatientList';
        } else {
            PQRS_Patient_List.params.PanelID = 'pnlPatientList';
        }
        if (PQRS_Patient_List.params.PatientIds != null && PQRS_Patient_List.params.PatientIds != "") {
            PQRS_Patient_List.PQRS_GetPatientsFromIds(PQRS_Patient_List.params.PatientIds);
            PQRS_Patient_List.GetVBPMeasureQuestionnaireAnswers(PQRS_Patient_List.params.measureId);
        }
        else {
            PQRS_Patient_List.PQRS_GetPatientsFromVisits(PQRS_Patient_List.params.VisitIds)
        }

    },
    saveMeasureReasoning: function () {
        var MeasureName = $('#' + PQRS_Patient_List.params.PanelID + ' #headingReasons').text().indexOf('Preventive Care and Screening: Body Mass Index (BMI) Screening and Follow-Up Plan') > -1 ? "421a" : "";
        MeasureName = (MeasureName != "") ? MeasureName : PQRS_Patient_List.params.measureId;
        var arrReasonig = $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody tr');
        var objData = null;
        var arrObjReasoning = [];
        var arrMeasuresToSave = [];
        //$.each(arrNonCompliantPatients, function (i, item) {
        //    arrPatient.push(item.Patientid);
        //});

        //var PatientIds = $.unique(arrPatient).join(",");
        if (MeasureName != "421a" && MeasureName != "0419" && MeasureName != "0043" && MeasureName != "0068" && MeasureName != "PHQ2" && MeasureName != "PHQ9") {

            $.each(arrReasonig, function (i, item) {
                var CurrentReasoning = $(item).find("input[type='radio']:checked,input[type='text'][value!='']");//$(item).find("input");
                $.each(CurrentReasoning, function (i, CurrentReasoning) {
                    //if (CurrentReasoning.length > 0) {
                    //    if ($(CurrentReasoning[0]).attr("Type") == "radio") {
                    //        CurrentReasoning = $(item).find("input:checked");
                    //    }
                    //    else if ($(CurrentReasoning[0]).attr("Type") == "text") {
                    //        CurrentReasoning = $(item).find("input[value!='']");
                    //    }
                    //}
                    CurrentReasoning = $(CurrentReasoning);
                    if (CurrentReasoning.length > 0 && ((CurrentReasoning.attr("type") == "radio" && CurrentReasoning.prop("checked") == true) || (CurrentReasoning.attr("type") == "text" && CurrentReasoning.val() != ''))) {
                        var CurrentMeasureId = CurrentReasoning.attr("MeasureId");
                        var CurrentNoteId = CurrentReasoning.attr("NoteId");
                        if (arrObjReasoning[CurrentNoteId] != null && arrObjReasoning[CurrentNoteId] != "") {
                            objData = arrObjReasoning[CurrentNoteId];
                        }
                        else {
                            objData = new Object();
                        }
                        arrMeasuresToSave.push(CurrentReasoning.attr("NoteId"));
                        objData.PatientId = CurrentReasoning.attr("PatientId");
                        objData.MeasureId = CurrentReasoning.attr("MeasureId");
                        objData.NoteId = CurrentReasoning.attr("NoteId");
                        if (CurrentReasoning.attr("name").indexOf("txtSystolic") > -1) {
                            if (CurrentReasoning.val() != "") {
                                objData.Systolic = CurrentReasoning.val();
                                objData.SystolicLOINC = CurrentReasoning.attr("LOINC");
                            }
                        }
                        else if (CurrentReasoning.attr("name").indexOf("txtDiastolic") > -1) {
                            if (CurrentReasoning.val() != "") {
                                objData.Diastolic = CurrentReasoning.val();
                                objData.DiastolicLOINC = CurrentReasoning.attr("LOINC");
                            }
                        }
                        else if (CurrentReasoning.attr("name").indexOf("txtBMI") > -1) {
                            if (CurrentReasoning.val() != "") {
                                objData.BMI = CurrentReasoning.val();
                                objData.BMILOINC = CurrentReasoning.attr("LOINC");
                            }
                        }
                        else {
                            if (CurrentReasoning.attr("SNOMEDCT") != null && CurrentReasoning.attr("SNOMEDCT") != "") {
                                if (objData.SNOMED != null && objData.SNOMED != "") {
                                    if (CurrentReasoning.attr("SNOMEDCT") != "") {
                                        var allSNOMED = objData.SNOMED + "," + CurrentReasoning.attr("SNOMEDCT");
                                        allSNOMED = $.unique(allSNOMED.split(",")).join(",");
                                        objData.SNOMED = allSNOMED;
                                    }
                                }
                                else {
                                    objData.SNOMED = CurrentReasoning.attr("SNOMEDCT");//"CodeValue");
                                }

                            }
                            if (CurrentReasoning.attr("CPT") != null && CurrentReasoning.attr("CPT") != "") {
                                if (objData.CPT != null && objData.CPT != "" && CurrentReasoning.attr("CPT") != "") {
                                    var allCPT = objData.CPT + "," + CurrentReasoning.attr("CPT");
                                    allCPT = $.unique(allCPT.split(",")).join(",");
                                    objData.CPT = allCPT;
                                }
                                else {
                                    objData.CPT = CurrentReasoning.attr("CPT");//"CodeValue");
                                }

                            }
                            if (CurrentReasoning.attr("CVX") != null && CurrentReasoning.attr("CVX") != "") {
                                if (objData.CVX != null && objData.CVX != "" && CurrentReasoning.attr("CVX")) {
                                    var allCVX = objData.CVX + "," + CurrentReasoning.attr("CVX");
                                    allCVX = $.unique(allCVX.split(",")).join(",");
                                    objData.CVX = allCVX;//"CodeValue");
                                }
                                else {
                                    objData.CVX = CurrentReasoning.attr("CVX");//"CodeValue");
                                }

                            }
                            if (CurrentReasoning.attr("HCPCS") != null && CurrentReasoning.attr("HCPCS") != "") {
                                if (objData.HCPCS != null && objData.HCPCS != "" && CurrentReasoning.attr("HCPCS")) {
                                    var allHCPCS = objData.HCPCS + "," + CurrentReasoning.attr("HCPCS");
                                    allHCPCS = $.unique(allHCPCS.split(",")).join(",");
                                    objData.HCPCS = allHCPCS;
                                }
                                else {
                                    objData.HCPCS = CurrentReasoning.attr("HCPCS");//"CodeValue");
                                }

                            }
                            if (CurrentReasoning.attr("rxnorm") != null && CurrentReasoning.attr("rxnorm") != "") {
                                if (objData.RXNORM != null && objData.RXNORM != "" && CurrentReasoning.attr("rxnorm")) {
                                    var allRXNORM = objData.RXNORM + "," + CurrentReasoning.attr("rxnorm");
                                    allRXNORM = $.unique(allRXNORM.split(",")).join(",");
                                    objData.RXNORM = allRXNORM;
                                }
                                else {
                                    objData.RXNORM = CurrentReasoning.attr("rxnorm");//"CodeValue");
                                }

                            }
                            if (CurrentReasoning.attr("LOINC") != null && CurrentReasoning.attr("LOINC") != "") {
                                if (objData.LOINC != null && objData.LOINC != "") {
                                    var allLOINC = objData.LOINC + "," + CurrentReasoning.attr("LOINC");
                                    allLOINC = $.unique(allLOINC.split(",")).join(",");
                                    objData.LOINC = allLOINC;
                                }
                                else {
                                    objData.LOINC = CurrentReasoning.attr("LOINC");//"CodeValue");
                                }

                            }
                            if (CurrentReasoning.attr("ICD9CM") != null && CurrentReasoning.attr("ICD9CM") != "") {
                                if (objData.ICD9CM != null && objData.ICD9CM != "") {
                                    var allICD9CM = objData.ICD9CM + "," + CurrentReasoning.attr("ICD9CM");
                                    allICD9CM = $.unique(allICD9CM.split(",")).join(",");
                                    objData.ICD9CM = allICD9CM;
                                }
                                else {
                                    objData.ICD9CM = CurrentReasoning.attr("ICD9CM");//"CodeValue");
                                }

                            }
                            if (CurrentReasoning.attr("ICD10CM") != null && CurrentReasoning.attr("ICD10CM") != "") {
                                if (objData.ICD10CM != null && objData.ICD10CM != "") {
                                    var allICD10CM = objData.ICD10CM + "," + CurrentReasoning.attr("ICD10CM");
                                    allICD10CM = $.unique(allICD10CM.split(",")).join(",");
                                    objData.ICD10CM = allICD10CM;
                                }
                                else {
                                    objData.ICD10CM = CurrentReasoning.attr("ICD10CM");//"CodeValue");
                                }

                            }

                            if (CurrentReasoning.attr("ActionResult") != null && CurrentReasoning.attr("ActionResult") != "") {
                                if (objData.ActionResult != null && objData.ActionResult != "") {
                                    var allActionResult = objData.ActionResult + "," + CurrentReasoning.attr("ActionResult");
                                    allActionResult = $.unique(allActionResult.split(",")).join(",");
                                    objData.ActionResult = allActionResult;
                                }
                                else {
                                    objData.ActionResult = CurrentReasoning.attr("ActionResult");//"CodeValue");
                                }

                            }
                        }
                        objData.ReportFromDate = PQRS_Patient_List.params.ReportFromDate;
                        objData.ReportToDate = PQRS_Patient_List.params.ReportToDate;
                        if (objData.MeasureId == "0018") {
                            objData.commandType = "updateSystolicDiastolic_cqm0018";
                        }

                        if (PQRS_Patient_List.params["FromParentCtrl"] == "ProgressNote" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabNotes" || PQRS_Patient_List.params.FromParentCtrl == "mstrTabDashBoard" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabPhoneEncounter" || PQRS_Patient_List.params.FromParentCtrl == "Clinical_NotesView") {
                            objData.bSignNote = "1";
                        }
                        else {
                            objData.bSignNote = "0";
                        }
                        arrObjReasoning[CurrentNoteId] = objData;
                    }
                });


            });
        }
        else if (PQRS_Patient_List.params.measureId == "PHQ2" || PQRS_Patient_List.params.measureId == "PHQ9") {
            PQRS_Patient_List.saveMeasureReasoningForPHQ(arrObjReasoning, arrMeasuresToSave);
        }
        else {
            PQRS_Patient_List.saveMeasureReasoningFor421(arrObjReasoning, arrMeasuresToSave);
        }
        var self = $('#' + PQRS_Patient_List.params.PanelID + ' form #frmPatientList #dgvMeasureReasons');
        var MeasureIds = $.unique(arrMeasuresToSave).join(",").split(",");
        //for (var i = 0; i < arrObjReasoning.length; i++) {
        //    arrObjReasoning[i].commandType = "save_cqm_reasoning";
        //    PQRS_Patient_List.saveCQMReasoning_DbCall(arrObjReasoning[i]);
        //}
        $.each(MeasureIds, function (i, item) {
            var objMeasure = arrObjReasoning[item];
            var isValidToSave = false;
            //if (PQRS_Patient_List.params.measureId == "PHQ9") {
            //    var arrReasonToSave = $('#frmPatientList #dgvMeasureReasons').find("input[type='radio'][id*='MainRad']:checked");
            //    if (arrReasonToSave.length > 1) {
            //        isValidToSave = true;
            //    }
            //}
            //else if (objMeasure != null) {
            //    isValidToSave = true;
            //}
            if (objMeasure != null) {
                isValidToSave = true;
            }
            if (isValidToSave == true) {
                objMeasure.commandType = "save_cqm_reasoning";
                var currentAnswerScores = 0;
                var selectedPatientId = objMeasure.PatientId;
                var selectedNoteId = objMeasure.NoteId;
                var selectedQuestionAnswersId = objMeasure.QuestionAnswersId;
                objMeasure.ProviderId = PQRS_Patient_List.params.ProviderId;

                if (PQRS_Patient_List.params.measureId == "PHQ2" || PQRS_Patient_List.params.measureId == "PHQ9") {
                    objMeasure.commandType = "save_vbp_reasoning";
                    var arrCurrentAnswers = objMeasure.QuestionAnswersId.split(",");

                    var currentQuestionAnswers = $.grep(PQRS_Patient_List.arrMeasureQuestionnaireAnswers, function (a) {
                        return $.inArray(a.QuestionAnswersId, arrCurrentAnswers) > -1// == PQRS_ICDCPTCodes.params.ReasonDescription;
                    });
                    $.each(currentQuestionAnswers, function (i, item) {
                        var score = item.Answer.split(" -")[0].trim();
                        if ($.isNumeric(score) == true) {
                            currentAnswerScores += parseInt(score);
                        }
                    });
                    objMeasure.Score = currentAnswerScores;
                }
                PQRS_Patient_List.saveCQMReasoning_DbCall(objMeasure).done(function (response) {
                    response = JSON.parse(response);


                    if (response.status != false) {
                        if (PQRS_Patient_List.params.FromParentCtrl == "ReportsSSRSDashboard" && (PQRS_Patient_List.params.measureId == "PHQ2" || PQRS_Patient_List.params.measureId == "PHQ9")) {
                            $.when(PQRS_Patient_List.loadPHQ9AfterPHQ2(PQRS_Patient_List.params.measureId)).then(function () {
                                var myvar = "2";
                                //if (PQRS_Patient_List.params.measureId == "PHQ2" && currentAnswerScores > 2) {

                                //    var mystr = "1";
                                //    //$("#pnlReportsSSRSDashboard #VBPIndividualReport #dgvIndividualReport tr:nth-child(2) td:nth-child(2)").parent().text()
                                //    $("#pnlReportsSSRSDashboard #VBPIndividualReport #dgvIndividualReport tr:has(td:contains('PHQ9'))").find("td:last a").trigger("click");
                                //}
                            });
                            setTimeout(function () {
                                if (PQRS_Patient_List.params.measureId == "PHQ2" && currentAnswerScores > 2) {
                                    ReportsSSRSDashboard.params.PHQ2SelectedPatientId = selectedPatientId;
                                    ReportsSSRSDashboard.params.PHQ2SelectedNoteId = selectedNoteId;
                                    ReportsSSRSDashboard.params.PHQ2SelectedQuestionAnswersId = selectedQuestionAnswersId;
                                    var arrcurrentMeasureReasoning = JSON.parse(ReportsSSRSDashboard.arrReasoning["PHQ9"]);
                                    var currentPatientReasoning = $.grep(arrcurrentMeasureReasoning, function (a) {
                                        return a.Patientid == selectedPatientId && a.NoteId == selectedNoteId;
                                    });
                                    if (currentPatientReasoning.length > 0) {
                                        //$("#pnlReportsSSRSDashboard #VBPIndividualReport #dgvIndividualReport tr:nth-child(2) td:nth-child(2)").parent().text()
                                        $("#pnlReportsSSRSDashboard #VBPIndividualReport #dgvIndividualReport tr:has(td:contains('PHQ9'))").find("td:last a").trigger("click");
                                    }
                                }
                                else {
                                    ReportsSSRSDashboard.params.PHQ2SelectedPatientId = null;
                                    ReportsSSRSDashboard.params.PHQ2SelectedNoteId = null;
                                    ReportsSSRSDashboard.params.PHQ2SelectedQuestionAnswersId = null;
                                }
                            }, 1000);
                        }
                        else if (PQRS_Patient_List.params.FromParentCtrl == "ReportsSSRSDashboard") {
                            PQRS_Patient_List.params.ParentCtrl = "mstrTabReports";
                            $("#CQMIndividualReport #CQMIndividualReport #btngenerateCQMindividualreport").trigger("click");
                            PQRS_Patient_List.UnLoad();
                        }
                        else if (PQRS_Patient_List.params.FromParentCtrl == "ProgressNote") {
                            PQRS_Patient_List.params.ParentCtrl = "clinicalTabProgressNote";
                            //$("#pnlClinicalProgressNote #actionPanClinicalProgressNote").modal('hide');
                            $.when(Clinical_ProgressNote.signNote(PQRS_Patient_List.params.BillingInformation, PQRS_Patient_List.params.Obj, PQRS_Patient_List.params.customSigMsg, PQRS_Patient_List.params.isComponentSelect, PQRS_Patient_List.params.NotesId, PQRS_Patient_List.params.IsFromProgressNote)).then(function () {

                                PQRS_Patient_List.UnLoad();

                            });
                            if (PQRS_Patient_List.params.BillingInformation != null) {

                            }
                        }
                        else if (PQRS_Patient_List.params.FromParentCtrl == "clinicalTabNotes") {
                            PQRS_Patient_List.params.ParentCtrl = "clinicalTabNotes";
                            $.when(Clinical_Notes.SignNotesAfterCQM(PQRS_Patient_List.params.NotesId, null, PQRS_Patient_List.params.NoteStatus, PQRS_Patient_List.params.isComponentSelect, PQRS_Patient_List.params.VisitDate)).then(function () {
                                PQRS_Patient_List.UnLoad();
                            });
                        }
                        else if (PQRS_Patient_List.params.FromParentCtrl == "mstrTabDashBoard") {
                            PQRS_Patient_List.params.ParentCtrl = "mstrTabDashBoard";
                            $.when(DashBoard.NotesStatusUpdateAfterCQM(PQRS_Patient_List.params.NotesId, PQRS_Patient_List.params.PatientId, PQRS_Patient_List.params.ProviderId, PQRS_Patient_List.params.isComponentSelect, PQRS_Patient_List.params.VisitDate, PQRS_Patient_List.params.BillingInfoId, PQRS_Patient_List.params.AppointmentDate, PQRS_Patient_List.params.VisitId, PQRS_Patient_List.params.NoteDate, PQRS_Patient_List.params.PatientTypeId)).then(function () {
                                PQRS_Patient_List.UnLoad();
                            });
                        }
                        else if (PQRS_Patient_List.params.FromParentCtrl == "clinicalTabPhoneEncounter") {
                            PQRS_Patient_List.params.ParentCtrl = "clinicalTabPhoneEncounter";
                            $.when(Clinical_PhoneEncounter.SignNotesAfterCQM(PQRS_Patient_List.params.NotesId, null, PQRS_Patient_List.params.NoteStatus, PQRS_Patient_List.params.isComponentSelect, PQRS_Patient_List.params.VisitDate)).then(function () {
                                PQRS_Patient_List.UnLoad();
                            });
                        }
                        else if (PQRS_Patient_List.params.FromParentCtrl == "Clinical_NotesView") {
                            PQRS_Patient_List.params.ParentCtrl = "Clinical_NotesView";
                            $.when(Clinical_NotesView.SignNotesAfterCQM(PQRS_Patient_List.params.NotesId)).then(function () {
                                PQRS_Patient_List.UnLoad();
                            });
                        }

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                utility.DisplayMessages("Please select reason value(s) to Save.", 3);
            }

        });

    },
    loadPHQ9AfterPHQ2: function (ReportType) {
        var objDfd = $.Deferred();
        if (ReportType == "PHQ2" || ReportType == "PHQ9") {
            PQRS_Patient_List.params.ParentCtrl = "mstrTabReports";
            $("#VBPIndividualReport #VBPIndividualReport #btngenerateVBPindividualreport").trigger("click");
            PQRS_Patient_List.UnLoad();
            objDfd.resolve("ok");
            return objDfd;
            //setTimeout(function () {
            //    if (ReportType == "PHQ2" && currentAnswerScores > 2) {

            //        var mystr = "1";
            //        //$("#pnlReportsSSRSDashboard #VBPIndividualReport #dgvIndividualReport tr:nth-child(2) td:nth-child(2)").parent().text()
            //        $("#pnlReportsSSRSDashboard #VBPIndividualReport #dgvIndividualReport tr:has(td:contains('PHQ9'))").find("td:last a").trigger("click");
            //    }
            //}, 500);
        }
    },
    saveMeasureReasoningFor421: function (arrObjReasoning, arrMeasuresToSave) {
        var objData = null;
        var arrReasonToSave = $('#frmPatientList #dgvMeasureReasons').find("input[type='radio'][id*='MainRad']:checked");
        if (arrReasonToSave.length > 0) {
            $.each(arrReasonToSave, function (k, reason) {
                var parentTd = $(reason).parent();
                var arrCurrentReasoning = parentTd.find("a[id*='anchorAssociateCode'][selectedValue!='']");
                $.each(arrCurrentReasoning, function (i, CurrentReasoning) {
                    CurrentReasoning = $(CurrentReasoning);
                    if (CurrentReasoning.attr("selectedValue") != null && CurrentReasoning.attr("selectedValue") != "") {
                        var CurrentMeasureId = CurrentReasoning.attr("MeasureId");
                        var CurrentNoteId = CurrentReasoning.attr("NoteId");
                        if (arrObjReasoning[CurrentNoteId] != null && arrObjReasoning[CurrentNoteId] != "") {
                            objData = arrObjReasoning[CurrentNoteId];
                        }
                        else {
                            objData = new Object();
                        }

                        arrMeasuresToSave.push(CurrentReasoning.attr("NoteId"));
                        objData.PatientId = CurrentReasoning.attr("PatientId");
                        objData.MeasureId = CurrentReasoning.attr("MeasureId");
                        objData.NoteId = CurrentReasoning.attr("NoteId");
                        objData.ReportFromDate = PQRS_Patient_List.params.ReportFromDate;
                        objData.ReportToDate = PQRS_Patient_List.params.ReportToDate;
                        if ($(reason).attr("ActionResult") != null && $(reason).attr("ActionResult") != "") {
                            if (objData.ActionResult != null && objData.ActionResult != "") {
                                var allActionResult = objData.ActionResult + "," + $(reason).attr("ActionResult");
                                allActionResult = $.unique(allActionResult.split(",")).join(",");
                                objData.ActionResult = allActionResult;
                            }
                            else {
                                objData.ActionResult = $(reason).attr("ActionResult");//"CodeValue");
                            }
                        }
                        var arrSelectedValues = CurrentReasoning.attr("selectedValue").split(",");
                        $.each(arrSelectedValues, function (j, selectedValue) {
                            var arrselectedValue = selectedValue.split(":");
                            if (arrselectedValue.length > 2) {
                                if (objData[arrselectedValue[1]] != null && objData[arrselectedValue[1]] != "") {
                                    var allValues = objData[arrselectedValue[1]] + "," + CurrentReasoning.attr(arrselectedValue[1]);
                                    allValues = $.unique(allValues.split(",")).join(",");
                                    if (arrselectedValue[1] == "SNOMEDCT") {
                                        objData["SNOMED"] = allValues;
                                    }
                                    else {
                                        objData[arrselectedValue[1]] = allValues;
                                    }
                                }
                                else {
                                    if (arrselectedValue[1] == "SNOMEDCT") {
                                        objData["SNOMED"] = CurrentReasoning.attr(arrselectedValue[1]);
                                    }
                                    else {
                                        objData[arrselectedValue[1]] = CurrentReasoning.attr(arrselectedValue[1]);
                                    }
                                }

                                if (arrselectedValue[1] == "txtBMI") {
                                    objData.BMI = CurrentReasoning.attr("bmi");
                                    objData.BMILOINC = CurrentReasoning.attr("bmiloinc");
                                }
                            }
                        });
                        if (PQRS_Patient_List.params["FromParentCtrl"] == "ProgressNote" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabNotes" || PQRS_Patient_List.params.FromParentCtrl == "mstrTabDashBoard" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabPhoneEncounter" || PQRS_Patient_List.params.FromParentCtrl == "Clinical_NotesView") {
                            objData.bSignNote = "1";
                        }
                        else {
                            objData.bSignNote = "0";
                        }

                        arrObjReasoning[CurrentNoteId] = objData;
                    }

                });
            });
        }
        //else {
        //    utility.DisplayMessages("Please select reason value(s) to Save.", 3);
        //}
    },
    saveMeasureReasoningForPHQ: function (arrObjReasoning, arrMeasuresToSave) {
        PQRS_Patient_List.savePHQReasoningUpdated(arrObjReasoning, arrMeasuresToSave);
        return;
        var objData = null;
        var arrReasonToSave = $('#frmPatientList #dgvMeasureReasons').find("input[type='radio'][id*='MainRad']:checked");
        if (arrReasonToSave.length > 0) {
            $.each(arrReasonToSave, function (k, reason) {
                var parentTd = $(reason).parent();
                var currentMainRadio = $(reason);
                var arrCurrentReasoning = parentTd.find("a[id*='anchorAssociateCode'][selectedValue!='']");
                $.each(arrCurrentReasoning, function (i, CurrentReasoning) {
                    CurrentReasoning = $(CurrentReasoning);
                    if (CurrentReasoning.attr("selectedValue") != null && CurrentReasoning.attr("selectedValue") != "") {
                        var CurrentMeasureId = CurrentReasoning.attr("MeasureId");
                        var CurrentNoteId = CurrentReasoning.attr("NoteId");
                        if (arrObjReasoning[CurrentNoteId] != null && arrObjReasoning[CurrentNoteId] != "") {
                            objData = arrObjReasoning[CurrentNoteId];
                        }
                        else {
                            objData = new Object();
                        }
                        objData.CPT = currentMainRadio.attr("CPT");
                        arrMeasuresToSave.push(CurrentReasoning.attr("NoteId"));
                        objData.PatientId = CurrentReasoning.attr("PatientId");
                        objData.MeasureId = CurrentReasoning.attr("MeasureId");
                        objData.NoteId = CurrentReasoning.attr("NoteId");
                        objData.ReportFromDate = PQRS_Patient_List.params.ReportFromDate;
                        objData.ReportToDate = PQRS_Patient_List.params.ReportToDate;
                        if ($(reason).attr("ActionResult") != null && $(reason).attr("ActionResult") != "") {
                            if (objData.ActionResult != null && objData.ActionResult != "") {
                                var allActionResult = objData.ActionResult + "," + $(reason).attr("ActionResult");
                                allActionResult = $.unique(allActionResult.split(",")).join(",");
                                objData.ActionResult = allActionResult;
                            }
                            else {
                                objData.ActionResult = $(reason).attr("ActionResult");//"CodeValue");
                            }
                        }
                        if (CurrentReasoning.attr("MeasureQuestionnaireId") != null && CurrentReasoning.attr("MeasureQuestionnaireId") != "") {
                            if (objData.MeasureQuestionnaireId != null && objData.MeasureQuestionnaireId != "") {
                                var allMeasureQuestionnaireId = objData.MeasureQuestionnaireId + "," + CurrentReasoning.attr("MeasureQuestionnaireId");
                                allMeasureQuestionnaireId = $.unique(allMeasureQuestionnaireId.split(",")).join(",");
                                objData.MeasureQuestionnaireId = allMeasureQuestionnaireId;
                            }
                            else {
                                objData.MeasureQuestionnaireId = CurrentReasoning.attr("MeasureQuestionnaireId");//"CodeValue");
                            }
                        }
                        var arrSelectedValues = CurrentReasoning.attr("selectedValue").split(",");
                        $.each(arrSelectedValues, function (j, selectedValue) {
                            var arrselectedValue = selectedValue.split(":");
                            if (arrselectedValue.length > 2) {
                                if (objData[arrselectedValue[1]] != null && objData[arrselectedValue[1]] != "") {
                                    var allValues = objData[arrselectedValue[1]] + "," + CurrentReasoning.attr(arrselectedValue[1]);
                                    allValues = $.unique(allValues.split(",")).join(",");
                                    if (arrselectedValue[1] == "SNOMEDCT") {
                                        objData["SNOMED"] = allValues;
                                    }
                                    else {
                                        objData[arrselectedValue[1]] = allValues;
                                    }
                                }
                                else {
                                    objData[arrselectedValue[1]] = CurrentReasoning.attr(arrselectedValue[1]);
                                }

                                if (arrselectedValue[1] == "txtBMI") {
                                    objData.BMI = CurrentReasoning.attr("bmi");
                                    objData.BMILOINC = CurrentReasoning.attr("bmiloinc");
                                }
                            }
                        });
                        if (PQRS_Patient_List.params["FromParentCtrl"] == "ProgressNote" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabNotes" || PQRS_Patient_List.params.FromParentCtrl == "mstrTabDashBoard" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabPhoneEncounter" || PQRS_Patient_List.params.FromParentCtrl == "Clinical_NotesView") {
                            objData.bSignNote = "1";
                        }
                        else {
                            objData.bSignNote = "0";
                        }

                        arrObjReasoning[CurrentNoteId] = objData;
                    }

                });
            });
        }
        //else {
        //    utility.DisplayMessages("Please select reason value(s) to Save.", 3);
        //}
    },
    savePHQReasoningUpdated: function (arrObjReasoning, arrMeasuresToSave) {
        var objData = null;
        var arrReasonToSave = $("#frmPatientList #dgvMeasureReasons tbody tr");//.find("select option[value!='']:selected");//$("#frmPatientList #dgvMeasureReasons tbody tr option[value!='']:selected");//.find("input[type='radio'][id*='MainRad']:checked");
        var lstquest = $("#frmPatientList #dgvMeasureReasons tbody tr").length - 1;
        if (PQRS_Patient_List.params.measureId == "PHQ9") {
            arrReasonToSave = $("#frmPatientList #dgvMeasureReasons tbody tr").not(":eq(" + lstquest + ")");
            lstquest = $("#frmPatientList #dgvMeasureReasons tbody tr:eq(" + lstquest + ")").find("select option[value!='']:selected").length;
        }
        else {
            lstquest = 1;
        }
        var arrRowsToSave = $.grep(arrReasonToSave, function (rows, idx) {
            return $(rows).find("select option[value!='']:selected").length > 0;
        });
        if (arrRowsToSave.length > 0 && lstquest > 0) {
            var currentAnswerScores = 0;
            var CurrentNoteId = null;
            arrReasonToSave = $("#frmPatientList #dgvMeasureReasons tbody tr");
            var arrRowsToSave = $.grep(arrReasonToSave, function (rows, idx) {
                return $(rows).find("select option[value!='']:selected").length > 0;
            });
            $.each(arrRowsToSave, function (i, reason) {
                var CurrentReasoning = $(reason);
                var CurrentMeasureId = CurrentReasoning.attr("MeasureId");
                CurrentNoteId = CurrentReasoning.attr("NoteId");
                if (arrObjReasoning[CurrentNoteId] != null && arrObjReasoning[CurrentNoteId] != "") {
                    objData = arrObjReasoning[CurrentNoteId];
                }
                else {
                    objData = new Object();
                }
                objData.CPT = "96127";//currentMainRadio.attr("CPT");
                arrMeasuresToSave.push(CurrentReasoning.attr("NoteId"));
                objData.PatientId = CurrentReasoning.attr("PatientId");
                objData.MeasureId = CurrentReasoning.attr("MeasureId");
                objData.NoteId = CurrentReasoning.attr("NoteId");
                objData.ReportFromDate = PQRS_Patient_List.params.ReportFromDate;
                objData.ReportToDate = PQRS_Patient_List.params.ReportToDate;
                var currentReason = $(CurrentReasoning.find("select option[value!='']:selected"));
                var score = currentReason.text().split(" -")[0].trim();
                if ($.isNumeric(score) == true) {
                    currentAnswerScores += parseInt(score);
                }
                if (currentReason.attr("MeasureQuestionnaireId") != null && currentReason.attr("MeasureQuestionnaireId") != "") {
                    if (objData.MeasureQuestionnaireId != null && objData.MeasureQuestionnaireId != "") {
                        var allMeasureQuestionnaireId = objData.MeasureQuestionnaireId + "," + currentReason.attr("MeasureQuestionnaireId");
                        allMeasureQuestionnaireId = $.unique(allMeasureQuestionnaireId.split(",")).join(",");
                        objData.MeasureQuestionnaireId = allMeasureQuestionnaireId;
                    }
                    else {
                        objData.MeasureQuestionnaireId = currentReason.attr("MeasureQuestionnaireId");//"CodeValue");
                    }
                }
                if (currentReason.attr("QuestionAnswersId") != null && currentReason.attr("QuestionAnswersId") != "") {
                    if (objData.QuestionAnswersId != null && objData.QuestionAnswersId != "") {
                        var allQuestionAnswersId = objData.QuestionAnswersId + "," + currentReason.attr("QuestionAnswersId");
                        allQuestionAnswersId = $.unique(allQuestionAnswersId.split(",")).join(",");
                        objData.QuestionAnswersId = allQuestionAnswersId;
                    }
                    else {
                        objData.QuestionAnswersId = currentReason.attr("QuestionAnswersId");//"CodeValue");
                    }
                }
                arrObjReasoning[CurrentNoteId] = objData;
            });
            //objData.Score = currentAnswerScores;

        }
    },
    saveCQMReasoning_DbCall: function (objData) {
        //var objData = {};
        ////if (NotesData != null) {
        ////    objData = JSON.parse(NotesData);
        ////}
        //objData["commandType"] = "save_cqm_reasoning";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    //Author: Muhammad Arshad
    //Date: 12 April 2017
    //Description: Call to Load VBP Measure Questionnaire Answers
    GetVBPMeasureQuestionnaireAnswers: function (MeasureId) {
        PQRS_Patient_List.loadVBPMeasureQuestionnaire_DbCall(MeasureId).done(function (response) {
            response = JSON.parse(response);
            PQRS_Patient_List.arrMeasureQuestionnaireAnswers = JSON.parse(response.VBPMeasureQuestionnaireAnswersLoad_JSON);
            var currentMeasureQuestionnaires = [];
            $.each(PQRS_Patient_List.arrMeasureQuestionnaireAnswers, function (i, item) {
                currentMeasureQuestionnaires.push(item.Question);
            });
            PQRS_Patient_List.arrMeasureQuestions = $.unique(currentMeasureQuestionnaires);
            var QuestIds = currentMeasureQuestionnaires.join(",");
        });
    },
    loadVBPMeasureQuestionnaire_DbCall: function (MeasureId) {
        var objData = {};
        objData["MeasureId"] = MeasureId;
        objData["commandType"] = "load_vbp_measurequestionnaireAnswers";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    //Author: Muhammad Arshad
    //Function Name: openPrintPreview Screen
    //Date: 23-01-2017
    //Description: Call to Load Patient Data based in Given PatientIds
    PQRS_GetPatientsFromIds: function (PatientIds) {
        PQRS_Patient_List.getPatientsData_DbCall(PatientIds).done(function (response) {
            response = JSON.parse(response);
            if (response.status && response.AllPatientsCount > 0) {
                if (PQRS_Patient_List.params.OnlyShowPatients == "1") {
                    $('#' + PQRS_Patient_List.params.PanelID + ' #btnPrint').removeClass('disableAll');
                }
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvPatientList tbody').empty();
                if (PQRS_Patient_List.params.measureId == "PHQ2" || PQRS_Patient_List.params.measureId == "PHQ9") {
                    PQRS_Patient_List.bindVBPRows(response);
                }
                else {
                    PQRS_Patient_List.bindCQMRows(response);
                }

                if (PQRS_Patient_List.params["FromParentCtrl"] == "ProgressNote" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabNotes" || PQRS_Patient_List.params.FromParentCtrl == "mstrTabDashBoard" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabPhoneEncounter" || PQRS_Patient_List.params.FromParentCtrl == "Clinical_NotesView") {
                    PQRS_Patient_List.CQM_GetMeasureReasons(PatientIds);
                }

            }
        });
    },
    //Author: Muhammad Arshad
    //Date: 25 April 2017
    //Description: Binds CQM Rows Based on Given
    bindCQMRows: function (response) {
        $.each(JSON.parse(response.AllPatientsLoad_JSON), function (i, item) {
            var $row = $('<tr/>');
            $row.attr('id', "dgvPatientListRow" + item.PatientId);
            $row.attr('PatientId', item.PatientId);
            //$row.attr('VisitId', item.VisitId);
            $row.attr('onclick', "PQRS_Patient_List.CQM_GetMeasureReasons('" + item.PatientId + "');utility.SelectGridRow($('#dgvPatientListRow" + item.PatientId + "'));");
            $row.append('<td>' + item.AccountNumber + '</td><td style="color:red">' + item.PatientName + '</td><td style="color:red">' + utility.RemoveTimeFromDate(null, item.DOB) + '</td>');
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvPatientList tbody').append($row);
            if (PQRS_Patient_List.params.measureId == "PHQ9" && i == (response.AllPatientsCount - 1)) {
                //Show Measure Reasoning
                setTimeout(function myfunction() {
                    if (ReportsSSRSDashboard.params.PHQ2SelectedPatientId != null && ReportsSSRSDashboard.params.PHQ2SelectedPatientId != "") {
                        //$("#dgvPatientList tbody tr#dgvPatientListRow" + ReportsSSRSDashboard.params.PHQ2SelectedPatientId).trigger("click");
                        var objSelectedPatientRow = $('#dgvPatientListRow' + ReportsSSRSDashboard.params.PHQ2SelectedPatientId);
                        objSelectedPatientRow.trigger("click");
                        utility.SelectGridRow(objSelectedPatientRow);
                        //PQRS_Patient_List.VBP_GetMeasureReasonsforPHQ2(ReportsSSRSDashboard.params.PHQ2SelectedPatientId);
                    }
                }, 500);
            }
        });
    },
    //Author: Muhammad Arshad
    //Date: 22 May 2017
    //Description: Shows Current Note Reasoning Based on Given Note if it's Draft
    showCurrentNoteReasoning: function (NotesId, PatientId) {
        PQRS_Patient_List.loadNoteInfo_DBCALL(NotesId).done(function (response) {
            response = JSON.parse(response);
            //var notesDetail = JSON.parse(response.NoteInfo_JSON);
            if (response.status != false && response.NoteInfo_JSON.length > 0 && response.NoteInfo_JSON[0].NoteStatus.toLowerCase() != 'signed') {
                PQRS_Patient_List.CQM_GetMeasureReasons(PatientId, NotesId);
                utility.SelectGridRow($('#dgvPatientListRow' + PatientId + "" + NotesId));
            }
            else if (response.NoteInfo_JSON.length > 0 && response.NoteInfo_JSON[0].NoteStatus.toLowerCase() == 'signed') {
                $('#dgvPatientListRow' + PatientId + "" + NotesId).addClass("disableAll");
            }
        });
    },
    //Author: Muhammad Arshad
    //Date: 25 April 2017
    //Description: Binds VBP Rows Based on Given
    bindVBPRows: function (response) {
        $('#' + PQRS_Patient_List.params.PanelID + ' #dgvPatientList th#thDOB').text("D.O.S");
        var selectedPatientsData = JSON.parse(response.AllPatientsLoad_JSON);
        $.each(selectedPatientsData, function (m, currentPatientData) {
            var selectedPatientReasoning = $.grep(PQRS_Patient_List.params.arrcurrentMeasureReasoning, function (currentReasoning, idx) {
                return currentReasoning.Patientid == currentPatientData.PatientId;
            });
            $.each(selectedPatientReasoning, function (i, item) {
                var $row = $('<tr/>');
                //var currentPatientData = $.grep(selectedPatientsData, function (a) {
                //    return a.PatientId == item.Patientid;
                //});
                $row.attr('id', "dgvPatientListRow" + currentPatientData.PatientId + item.NoteId);
                $row.attr('PatientId', currentPatientData.PatientId);
                //$row.attr('VisitId', item.VisitId);
                $row.attr('onclick', "PQRS_Patient_List.showCurrentNoteReasoning('" + item.NoteId + "','" + currentPatientData.PatientId + "');");
                //$row.attr('onclick', "PQRS_Patient_List.CQM_GetMeasureReasons('" + currentPatientData.PatientId + "','" + item.NoteId + "');utility.SelectGridRow($('#dgvPatientListRow" + currentPatientData.PatientId + item.NoteId + "'));");
                $row.append('<td>' + currentPatientData.AccountNumber + '</td><td style="color:red">' + currentPatientData.PatientName + '</td><td style="color:red">' + utility.RemoveTimeFromDate(null, item.NoteDate) + '</td>');
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvPatientList tbody').append($row);
                if (PQRS_Patient_List.params.measureId == "PHQ9" && i == (selectedPatientReasoning.length - 1)) {
                    //Show Measure Reasoning
                    setTimeout(function myfunction() {
                        if (ReportsSSRSDashboard.params.PHQ2SelectedPatientId != null && ReportsSSRSDashboard.params.PHQ2SelectedPatientId != "") {
                            //$("#dgvPatientList tbody tr#dgvPatientListRow" + ReportsSSRSDashboard.params.PHQ2SelectedPatientId).trigger("click");
                            var objSelectedPatientRow = $('#' + PQRS_Patient_List.params.PanelID + " #dgvPatientList tr[id='dgvPatientListRow" + ReportsSSRSDashboard.params.PHQ2SelectedPatientId + "" + ReportsSSRSDashboard.params.PHQ2SelectedNoteId + "']");//$('#dgvPatientListRow' + ReportsSSRSDashboard.params.PHQ2SelectedPatientId);
                            if (objSelectedPatientRow.length > 0) {
                                objSelectedPatientRow = $(objSelectedPatientRow[0]);
                                objSelectedPatientRow.trigger("click");
                                //utility.SelectGridRow(objSelectedPatientRow);
                                //PQRS_Patient_List.VBP_GetMeasureReasonsforPHQ2(ReportsSSRSDashboard.params.PHQ2SelectedPatientId);
                            }

                        }
                    }, 500);
                }
            });
        });

    },
    //Author: Muhammad Arshad
    //Function Name: openAssociateCode
    //Date: 24-01-2017
    //Description: Opening Associate Codes screen
    openAssociateCodes: function (FromCtrlId, CodeType, CodeValues, FromCtrlName, GroupOperator, CodeDescription, MeasureId, ReasonDescription, DescValue, Patientid, NoteId, selectedValue) {
        var attrData = null;
        if (selectedValue != null) {
            attrData = selectedValue;
        }
        var params = [];
        params["VBPQuestionAnswers"] = PQRS_Patient_List.arrMeasureQuestionnaireAnswers;
        params["mode"] = "Add";
        params["FromCtrl"] = FromCtrlId;
        params["FromCtrlName"] = FromCtrlName;
        params["CodeType"] = CodeType;
        params["CodeValues"] = CodeValues;
        params["GroupOperator"] = GroupOperator;
        params["CodeDescription"] = CodeDescription;
        params["ReasonDescription"] = ReasonDescription;
        params["MeasureId"] = MeasureId;
        params["DescValue"] = DescValue;
        params["Patientid"] = Patientid;
        params["NoteId"] = NoteId;
        params["selectedValue"] = null;
        if (selectedValue != null) {
            params["selectedValue"] = selectedValue;
        }
        params["ParentCtrl"] = "PQRS_Patient_List";
        var currentCtrl = $('#' + PQRS_Patient_List.params.PanelID + " input[type='radio'][id='" + FromCtrlId + "'][name='" + FromCtrlName + "']");
        var arrAttrNameValue = [];
        if (currentCtrl.length > 0) {
            var arrAttr = currentCtrl[0].attributes;
            $.each(arrAttr, function (i, item) {
                if (item.name.indexOf("div") > -1) {
                    arrAttrNameValue[item.name] = item.value.trim();
                }
            });
        }
        params["arrAttrNameValue"] = arrAttrNameValue;
        //params["VisitIds"] = VisitIds;
        //params["PatientId"] = ReportsSSRSDashboard.params.PatientId;
        params["FromAdmin"] = 0;
        //./ params["ParentCtrl"] = "ReportsSSRSDashboard";
        LoadActionPan('PQRS_ICDCPTCodes', params);


    },
    //Author: Muhammad Arshad
    //Function Name: openPrintPreview Screen
    //Date: 25-01-2017
    //Description: Opening Print Screen
    openPrintView: function (FromCtrlId, CodeType, CodeValues) {

        var params = [];
        params["mode"] = "Add";
        params["FromCtrl"] = FromCtrlId;
        params["CodeType"] = CodeType;
        params["CodeValues"] = CodeValues;
        params["ProviderId"] = PQRS_Patient_List.params.ProviderId;
        params["ParentCtrl"] = "PQRS_Patient_List";
        params["OnlyShowPatients"] = PQRS_Patient_List.params.OnlyShowPatients;
        //params["VisitIds"] = VisitIds;
        params["PatientId"] = PQRS_Patient_List.params.PatientId;
        params["NoteId"] = PQRS_Patient_List.params.NoteId;//dgvPatientList
        if (PQRS_Patient_List.params.OnlyShowPatients == "1") {
            params["FromControlId"] = '#' + PQRS_Patient_List.params.PanelID + ' #divMainPatientList';
        }
        else {
            params["FromControlId"] = '#' + PQRS_Patient_List.params.PanelID + ' #detailTable';
        }

        params["FromAdmin"] = 0;
        //./ params["ParentCtrl"] = "ReportsSSRSDashboard";
        LoadActionPan('PQRS_MissingDataView', params);


    },
    //Author: Muhammad Arshad
    //Date: 14/03/2017
    //Description: Load Patient Recent Note
    getPatientRecentNote: function (PatientId, MeasureReasoning) {
        var recentNoteId = "";
        var arrnotes = [];
        $.each(MeasureReasoning, function (index, item) {
            arrnotes.push(item.NoteId);
        });
        arrnotes = $.unique(arrnotes);
        var strNotes = arrnotes.join(",");
        if (arrnotes != null && arrnotes.length > 0) {
            PQRS_Patient_List.getPatientRecentNote_DbCall(PatientId, strNotes).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    recentNoteId = response.NoteId;
                }
                return recentNoteId;
            });
        }

    },
    //Author: Muhammad Arshad
    //Function Name: openPrintPreview Screen
    //Date: 24-01-2017
    //Description: Load Patient NonCompliant Reasons
    CQM_GetMeasureReasons: function (PatientId, NoteId) {
        // var response = JSON.parse(PQRS_Patient_List.params.arrcurrentMeasureReasoning);
        //UnloadActionPan("clinicalTabProgressNote", "BillingInformation");
        if (PQRS_Patient_List.params.BillingInformation != null && PQRS_Patient_List.params.BillingInformation.params.ParentCtrl == "clinicalTabProgressNote") {
            $("#actionPanClinicalProgressNote div#pnlBillingInformation").remove();
        }
        else if (PQRS_Patient_List.params.BillingInformation != null && PQRS_Patient_List.params.BillingInformation.params.ParentCtrl == "Clinical_NotesView") {
            $("#actionPanNotesView div#pnlBillingInformation").remove();
        }
        //$("#actionPanNotesView div#pnlBillingInformation").remove();"
        if (PQRS_Patient_List.params.FromParentCtrl == "ProgressNote" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabNotes" || PQRS_Patient_List.params.FromParentCtrl == "mstrTabDashBoard" || PQRS_Patient_List.params.FromParentCtrl == "clinicalTabPhoneEncounter" || PQRS_Patient_List.params.FromParentCtrl == "Clinical_NotesView") {
            var firstReasoning = PQRS_Patient_List.params.arrcurrentMeasureReasoning[0];//JSON.parse(PQRS_Patient_List.params.arrcurrentMeasureReasoning[0]);
            PQRS_Patient_List.params["measureId"] = firstReasoning.MeasureId;
            PQRS_Patient_List.params["MeasureFullName"] = firstReasoning.VersionNum + " " + firstReasoning.MeasureDescription;
        }
        if (PQRS_Patient_List.params["measureId"] == "0421a" || PQRS_Patient_List.params["measureId"] == "0419" || PQRS_Patient_List.params["measureId"] == "0043" || PQRS_Patient_List.params["measureId"] == "0068") {
            PQRS_Patient_List.CQM_GetMeasureReasonsfor421(PatientId);
            return;
        }
        if (PQRS_Patient_List.params["ReportType"] == "VBP") {
            var mystr = "1";
            var mystr2 = "2";
            PQRS_Patient_List.VBP_GetMeasureReasonsforPHQ2(PatientId, NoteId);
            return;
        }

        var currentPatientReasoning = $.grep(PQRS_Patient_List.params.arrcurrentMeasureReasoning, function (a) {
            return a.Patientid == PatientId;
        });
        var b = [];

        $.each(currentPatientReasoning, function (index, event) {
            var events = $.grep(b, function (e) {
                return event.FlowId === e.FlowId &&
                    event.NoteId === e.NoteId;
            });
            if (events.length === 0) {
                b.push(event);
            }
        });
        currentPatientReasoning = b.sort(function (a, b) { return a.FlowId > b.FlowId });
        var arrnotes = [];
        $.each(currentPatientReasoning, function (index, item) {
            arrnotes.push(item.NoteId);
        });
        arrnotes = $.unique(arrnotes);

        var ProviderId = PQRS_Patient_List.params.ProviderId;
        PQRS_Patient_List.params.PatientId = PatientId;
        if (currentPatientReasoning.length > 0) {
            $('#' + PQRS_Patient_List.params.PanelID + ' #btnSave').removeClass('disableAll');
            $('#' + PQRS_Patient_List.params.PanelID + ' #btnPrint').removeClass('disableAll');
            $('#' + PQRS_Patient_List.params.PanelID + ' #detailTable').removeClass('hidden');
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody').empty();
            var radBtnNo = 1;
            $.each(currentPatientReasoning, function (i, item) {
                var $row = $('<tr/>');
                var radBtnName = "rad" + item.MeasureId;
                if ($('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody tr input[name="rad' + item.MeasureId + '"]').length > 0) {
                    radBtnName = radBtnName + i;
                }
                var arrCodeType = item.CodeType.split(",");
                var arrCodes = item.CodeValue.split("|");
                var radBtns = "";
                PQRS_Patient_List.params.NoteId = item.NoteId;
                var arrReason = item.DescValue.trim().replace('"', '').split(",");
                if (item.DescValue.trim() == "") {
                    arrReason[0] = item.MeasureDescription.trim();
                }
                if (arrReason.length > 0) {
                    $.each(arrReason, function (k, ReasonVal) {
                        ReasonVal = ReasonVal.replace(/&quot;/g, '');

                        if (item.CodeType == "LOINC" && item.CodeValue == '8480-6') {
                            radBtnName = "txtSystolic";
                            radBtns += '<input type="text" MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" LOINC="8480-6" id="' + radBtnName + radBtnNo + '" max="255" name="' + radBtnName + '" data-plugin-keyboard-numpad maxlength="3" class="form-control" onblur="utility.ValidateDecimal(event, 0); Clinical_Vitals.ValidateBP(this, null);"><br/>';
                        }
                        else if (item.CodeType == "LOINC" && item.CodeValue == '8462-4') {
                            radBtnName = "txtDiastolic";
                            radBtns += '<input type="text" MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" LOINC="8462-4" id="' + radBtnName + radBtnNo + '" max="255" name="' + radBtnName + '" data-plugin-keyboard-numpad maxlength="3" class="form-control" onblur="utility.ValidateDecimal(event, 0); Clinical_Vitals.ValidateBP(null, this);"><br/>';
                        }
                        else if (item.CodeType == "LOINC" && item.CodeValue == '39156-5') {
                            var arrBMIValue = item.DescValue.trim().split("(");//(result >= 18.5 kg/m2)(result < 25 kg/m2)
                            var strBMIValue = ""
                            var minBMIValue = 0;
                            var maxBMIValue = 0;
                            var BMIValueLength = arrBMIValue.length;
                            $.each(arrBMIValue, function (i, item) {
                                strBMIValue = item.replace("kg/m2", "");
                                strBMIValue = utility.getNumericPart(strBMIValue);
                                if (item != "") {
                                    if (BMIValueLength > 2) {
                                        if (item.indexOf("&gt;") > -1) {
                                            minBMIValue = strBMIValue.replace(")", "").trim();
                                        }
                                        else if (item.indexOf("&lt;") > -1) {
                                            maxBMIValue = strBMIValue.replace(")", "").trim();
                                        }
                                    }
                                    else {
                                        if (item.indexOf("&gt;") > -1) {
                                            minBMIValue = strBMIValue.replace(")", "").trim();
                                            maxBMIValue = 0;
                                        }
                                        else if (item.indexOf("&lt;") > -1) {
                                            maxBMIValue = strBMIValue.replace(")", "").trim();
                                            minBMIValue = 0;
                                        }
                                    }
                                }
                            });

                            radBtnName = "txtBMI";
                            radBtns += '<input type="text" title="' + item.DescValue + '" MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" LOINC="39156-5" id="' + radBtnName + radBtnNo + '" max="255" name="' + radBtnName + '" data-plugin-keyboard-numpad maxlength="3" class="form-control" onblur="utility.ValidateValue(event, 0,\'' + minBMIValue + '\',\'' + maxBMIValue + '\');"><br/>';
                        }
                        else {
                            var onclick = "";
                            if (PQRS_Patient_List.params.OnlyShowPatients != "1") {
                                onclick = "PQRS_Patient_List.openAssociateCodes('" + radBtnNo + "','" + item.CodeType.trim() + "','" + item.CodeValue.trim() + "','" + radBtnName + "','" + item.GroupOperator.trim() + "','" + item.CodeDescription.trim() + "','" + item.MeasureId + "','" + ReasonVal.trim() + "');"
                            }

                            var expandCollapseIcon = ' <a id="anchorAssociateCode"' + radBtnNo + ' MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" href="#" onclick="' + onclick + '" class="tab_space" title="Associate Code(s)"><i class="fa fa-plus-square"></i></a>';
                            var ExceptionalSNOMED = "";
                            if (item.MeasureId == "0421a") {
                                if (item.MeasureDescription != null && item.MeasureDescription.toLowerCase().indexOf("overweigh") > -1) {
                                    ExceptionalSNOMED = "238131007";
                                }
                                else if (item.MeasureDescription != null && item.MeasureDescription.toLowerCase().indexOf("underweigh") > -1) {
                                    //Intervention, Order: Below Normal Follow up(reason: Underweight)
                                    ExceptionalSNOMED = "248342006";
                                }
                            }

                            radBtns += '<input class="mb-sm disableAll" type="radio" ActionResult="' + ExceptionalSNOMED + '" title="' + ReasonVal + '" MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" name="' + radBtnName + '" id="' + radBtnNo + '" CodeType="' + item.CodeType.trim() + '"' + '" CodeValue="' + item.CodeValue.trim() + '" />' + ReasonVal.trim() + expandCollapseIcon + '<br/>';
                        }
                        radBtnNo += 1;
                    });
                    var MeasureData = "";
                    if (item.CodeType == "LOINC" && item.CodeValue == '39156-5') {
                        MeasureData = '<td>' + item.VersionNum.trim() + ' ' + item.MeasureDescription.trim() + " " + item.DescValue.trim() + '<br/></td>';
                    }
                    else {
                        MeasureData = '<td>' + item.VersionNum.trim() + ' ' + item.MeasureDescription.trim() + '<br/></td>';
                    }

                    $row.append(MeasureData + '<td>' + radBtns + '</td>');
                    $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody').append($row);
                }
            });


        }
    },
    CQM_GetMeasureReasonsfor421: function (PatientId) {
        // var response = JSON.parse(PQRS_Patient_List.params.arrcurrentMeasureReasoning);
        var currentPatientReasoning = $.grep(PQRS_Patient_List.params.arrcurrentMeasureReasoning, function (a) {
            return a.Patientid == PatientId;
        });

        var b = [];

        $.each(currentPatientReasoning, function (index, event) {
            var events = $.grep(b, function (e) {
                return event.FlowId === e.FlowId &&
                    event.NoteId === e.NoteId;
            });
            if (events.length === 0) {
                b.push(event);
            }
        });
        currentPatientReasoning = b.sort(function (a, b) { return a.FlowId > b.FlowId });
        var arrnotes = [];
        $.each(currentPatientReasoning, function (index, item) {
            arrnotes.push(item.NoteId);
        });
        arrnotes = $.unique(arrnotes);
        var ProviderId = PQRS_Patient_List.params.ProviderId;
        PQRS_Patient_List.params.PatientId = PatientId;
        if (currentPatientReasoning.length > 0) {
            $('#' + PQRS_Patient_List.params.PanelID + ' #btnSave').removeClass('disableAll');
            $('#' + PQRS_Patient_List.params.PanelID + ' #btnPrint').removeClass('disableAll');
            $('#' + PQRS_Patient_List.params.PanelID + ' #detailTable').removeClass('hidden');
            $('#' + PQRS_Patient_List.params.PanelID + ' #detailTable #headingReasons').text(PQRS_Patient_List.params.MeasureFullName);
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody').empty();
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMeasure').removeClass("col-sm-6");
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMissingValue').removeClass("col-sm-6");
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMeasure').addClass("col-sm-11");
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMissingValue').addClass("col-sm-1");
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMeasure').text("Select one of the following missing values:");
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMissingValue').addClass("hidden");
            var radBtnNo = 1;
            //var recentNoteId = PQRS_Patient_List.getPatientRecentNote(PatientId, currentPatientReasoning);

            if (arrnotes != null && arrnotes.length > 0) {
                PQRS_Patient_List.getPatientRecentNote_DbCall(PatientId, arrnotes.join(",")).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        arrnotes = [];
                        arrnotes.push(response.NoteId);
                    }
                    $.each(arrnotes, function (j, CurrentNote) {

                        var currentNoteReasoning = $.grep(PQRS_Patient_List.params.arrcurrentMeasureReasoning, function (a) {
                            return a.NoteId == CurrentNote;
                        });
                        if (PQRS_Patient_List.params["measureId"] == "0419" && currentNoteReasoning.length > 0) {
                            var currentReasoning = currentNoteReasoning[0];
                            currentNoteReasoning = [];
                            currentNoteReasoning.push(currentReasoning);
                        }

                        var arrConditions = [];
                        $.each(currentNoteReasoning, function (i, item) {
                            arrConditions.push(item.ConditionGroupNumber);
                        });
                        arrConditions = $.unique(arrConditions);
                        $.each(arrConditions, function (k, condition) {
                            var $row = $('<tr/>');

                            var MeasureData = "";
                            var radBtns = "";
                            var isRadBtnAdded = 0;
                            var CurrentRowMainradBtns = "";
                            $.each(currentNoteReasoning, function (i, item) {
                                if (item.ConditionGroupNumber != condition) {
                                    return;
                                }

                                var radBtnName = "rad" + item.ConditionGroupNumber + item.GroupOperator;
                                PQRS_Patient_List.params.NoteId = item.NoteId;
                                var ExceptionalSNOMED = "";
                                if (item.MeasureId == "0421a") {
                                    if (item.MeasureDescription != null && item.MeasureDescription.toLowerCase().indexOf("overweigh") > -1) {
                                        ExceptionalSNOMED = "238131007";
                                    }
                                    else if (item.MeasureDescription != null && item.MeasureDescription.toLowerCase().indexOf("underweigh") > -1) {
                                        //Intervention, Order: Below Normal Follow up(reason: Underweight)
                                        ExceptionalSNOMED = "248342006";
                                    }
                                }
                                // var CurrentRowMainradBtns = '<input class="mb-sm disableAll" type="radio" ActionResult="' + ExceptionalSNOMED + '" MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" name="' + radBtnName + '" id=MainRad"' + radBtnNo + '" /><span>All of the following:</span>';
                                if (MeasureData.indexOf(radBtnName) < 0 && radBtns == "") {
                                    var radBtnText = "";
                                    if (item.GroupOperator == "OR" || PQRS_Patient_List.params["measureId"] == "0043") {
                                        radBtnText = "ONE of the following";
                                    }
                                    else {
                                        radBtnText = "All of the following";
                                    }
                                    radBtns = '<span>' + radBtnText + '</span>';
                                    isRadBtnAdded = 1;
                                }
                                else {
                                    radBtns = "";
                                }
                                if (1 == 0) {//(PQRS_Patient_List.params["measureId"] == "0043") {
                                    MeasureData = PQRS_Patient_List.get0043Reason(radBtnNo, radBtns, MeasureData, item, radBtnName);
                                }
                                else {
                                    var onclick = "";
                                    if (PQRS_Patient_List.params.OnlyShowPatients != "1") {
                                        onclick = "PQRS_Patient_List.openAssociateCodes('" + radBtnNo + "','" + item.CodeType.trim() + "','" + item.CodeValue.trim() + "','" + radBtnName + "','" + item.GroupOperator.trim() + "','" + item.CodeDescription.trim() + "','" + item.MeasureId + "','" + item.MeasureDescription.trim() + "','" + item.DescValue.trim() + "','" + item.Patientid + "','" + item.NoteId + "',$(this).attr('selectedValue'));"
                                    }

                                    var expandCollapseIcon = ' <a id="anchorAssociateCode' + radBtnNo + '" MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" href="#" onclick="' + onclick + '" class="tab_space" title="Associate Code(s)"><i class="fa fa-plus-square"></i></a>';

                                    if (radBtns != "" && MeasureData.substr(-5).indexOf("<br") > -1) {
                                        MeasureData += "<br/>";
                                    }
                                    if (item.GroupOperator != "OR") {
                                        if (item.MeasureId == "0421a" && (item.FlowId == "35" || item.FlowId == "39")) {
                                            MeasureData = MeasureData.replace("<span>ONE of the following</span>", "");
                                        }
                                        MeasureData = item.MeasureDescription.trim() + ' ' + expandCollapseIcon + MeasureData + (MeasureData.substr(-5).indexOf("<br") < 0 ? "<br/>" : "");
                                    }
                                    else {
                                        if (radBtns != "" && MeasureData.substr(-5).indexOf("<br") > -1) {
                                            MeasureData += "<br/>";
                                        }
                                        MeasureData += radBtns + '<br/>' + item.MeasureDescription.trim() + ' ' + expandCollapseIcon;
                                    }
                                }

                                if (CurrentRowMainradBtns == "") {
                                    CurrentRowMainradBtns = '<input class="mb-sm disableAll" type="radio" ActionResult="' + ExceptionalSNOMED + '" MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" name="' + radBtnName + '" id="MainRad' + radBtnNo + '" />' + radBtns + ';'//<span>All of the following:</span>';
                                }
                                radBtnNo += 1;
                                MeasureData = MeasureData.replace('<br/><br/>', '<br/>');
                            });

                            $row.append('<td>' + CurrentRowMainradBtns + "<br/>" + MeasureData + '</td>' + '<td class="hidden">' + radBtns + '</td>');
                            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody').append($row);
                            radBtns = "";
                            MeasureData = "";
                        });
                    });
                });
            }
        }
    },
    VBP_GetMeasureReasonsforPHQ2: function (PatientId, NoteId) {
        // var response = JSON.parse(PQRS_Patient_List.params.arrcurrentMeasureReasoning);
        PQRS_Patient_List.params.ResoningPatientId = PatientId;
        var currentPatientReasoning = $.grep(PQRS_Patient_List.params.arrcurrentMeasureReasoning, function (a) {
            return a.Patientid == PatientId;
        });

        var b = [];

        $.each(currentPatientReasoning, function (index, event) {
            var events = $.grep(b, function (e) {
                return event.FlowId === e.FlowId &&
                    event.NoteId === e.NoteId;
            });
            if (events.length === 0) {
                b.push(event);
            }
        });
        currentPatientReasoning = b.sort(function (a, b) { return a.FlowId > b.FlowId });
        var arrnotes = [];
        if (NoteId != null && NoteId != "") {
            arrnotes.push(NoteId);
        }
        else {
            $.each(currentPatientReasoning, function (index, item) {
                arrnotes.push(item.NoteId);
            });
        }

        arrnotes = $.unique(arrnotes);
        var ProviderId = PQRS_Patient_List.params.ProviderId;
        PQRS_Patient_List.params.PatientId = PatientId;
        if (currentPatientReasoning.length > 0) {
            $('#' + PQRS_Patient_List.params.PanelID + ' #btnSave').removeClass('disableAll');
            $('#' + PQRS_Patient_List.params.PanelID + ' #btnPrint').removeClass('disableAll');
            $('#' + PQRS_Patient_List.params.PanelID + ' #detailTable').removeClass('hidden');
            $('#' + PQRS_Patient_List.params.PanelID + ' #detailTable #headingReasons').text(PQRS_Patient_List.params.MeasureFullName);
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody').empty();
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMeasure').removeClass("col-sm-6");
            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMissingValue').removeClass("col-sm-6");

            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMeasure').text("Select " + (PQRS_Patient_List.params.measureId == "PHQ2" ? "one" : "all") + " of the following missing values:");
            if (PQRS_Patient_List.params.measureId == "PHQ2" || PQRS_Patient_List.params.measureId == "PHQ9") {
                //headerMeasureEntry
                $('#' + PQRS_Patient_List.params.PanelID + ' #headerMeasureEntry').addClass("hidden");
                $('#' + PQRS_Patient_List.params.PanelID + ' #divSpacer1').addClass("hidden");
                $('#' + PQRS_Patient_List.params.PanelID + ' #divClearFix1').addClass("hidden");
                $('#' + PQRS_Patient_List.params.PanelID + ' #divSpacer2').addClass("hidden");
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMissingValue').removeClass("hidden");
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMeasure').addClass("col-sm-9");
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMissingValue').addClass("col-sm-3");
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMeasure').text("Patient Health Questionnaire - " + PQRS_Patient_List.params.measureId.replace("PHQ", ""));
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMissingValue').text("Reasoning");
            }
            else {
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMissingValue').addClass("hidden");
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMeasure').addClass("col-sm-11");
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons th#thMissingValue').addClass("col-sm-1");
            }

            var radBtnNo = 1;
            //var recentNoteId = PQRS_Patient_List.getPatientRecentNote(PatientId, currentPatientReasoning);

            if (arrnotes != null && arrnotes.length > 0) {
                PQRS_Patient_List.getPatientRecentNote_DbCall(PatientId, arrnotes.join(",")).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        arrnotes = [];
                        arrnotes.push(response.NoteId);
                    }
                    $.each(arrnotes, function (j, CurrentNote) {
                        var currentReasoning = $.grep(currentPatientReasoning, function (a) {
                            return a.NoteId == CurrentNote;
                        });

                        var currentNoteReasoning = $.grep(PQRS_Patient_List.arrMeasureQuestionnaireAnswers, function (a) {
                            return a.NoteId == CurrentNote;
                        });
                        var arrConditions = [];
                        if (PQRS_Patient_List.params.measureId == "PHQ2") {
                            arrConditions.push(1);
                        }
                        else if (PQRS_Patient_List.params.measureId == "PHQ9") {
                            arrConditions.push(1);
                            arrConditions.push(2);
                        }
                        //arrConditions = $.unique(PQRS_Patient_List.arrMeasureQuestions);
                        var CurrentRowMainradBtns = "";
                        $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody tr').remove();
                        if (PQRS_Patient_List.params.measureId == "PHQ2" || PQRS_Patient_List.params.measureId == "PHQ9") {
                            var arrConditions = [];

                            var arrUniqueConditions = $.unique(PQRS_Patient_List.arrMeasureQuestions);
                            for (var i = 0; i <= arrUniqueConditions.length; i++) {
                                arrConditions.push(i + 1);
                            }
                            PQRS_Patient_List.bindPHQ2Rows(arrConditions, arrUniqueConditions, CurrentNote);
                        }
                        else {
                            $.each(arrConditions, function (k, condition) {
                                var $row = $('<tr/>');

                                var MeasureData = "";
                                var radBtns = "";
                                var isRadBtnAdded = 0;

                                var radBtnName = "rad" + condition;
                                PQRS_Patient_List.params.NoteId = CurrentNote;
                                var ExceptionalSNOMED = "";
                                var arrMeasureReason = [];
                                // var CurrentRowMainradBtns = '<input class="mb-sm disableAll" type="radio" ActionResult="' + ExceptionalSNOMED + '" MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" name="' + radBtnName + '" id=MainRad"' + radBtnNo + '" /><span>All of the following:</span>';
                                if (MeasureData.indexOf(radBtnName) < 0 && radBtns == "") {
                                    var radBtnText = "";
                                    if (PQRS_Patient_List.params["measureId"] == "PHQ9") {
                                        if (condition == 1) {
                                            radBtnText = "Over the last 2 weeks, how often have you been bothered by any of the following problems:";
                                        }
                                        else if (PQRS_Patient_List.params["measureId"] == "PHQ9" && condition == 2) {
                                            radBtnText = PQRS_Patient_List.arrMeasureQuestions[PQRS_Patient_List.arrMeasureQuestions.length - 1];
                                        }

                                    }
                                    else if (PQRS_Patient_List.params["measureId"] == "PHQ2") {
                                        radBtnText = "All of the following";
                                    }
                                    radBtns = '<span>' + radBtnText + '</span>';
                                    isRadBtnAdded = 1;
                                }
                                else {
                                    radBtns = "";
                                }
                                var onclick = "";
                                var currentCondition = condition;
                                var arrUniqueConditions = $.unique(PQRS_Patient_List.arrMeasureQuestions);
                                if (PQRS_Patient_List.params["measureId"] == "PHQ9" && condition == 2) {

                                    arrUniqueConditions = [];
                                    arrUniqueConditions.push(PQRS_Patient_List.arrMeasureQuestions[PQRS_Patient_List.arrMeasureQuestions.length - 1]);
                                }

                                $.each(arrUniqueConditions, function (i, question) {
                                    if (PQRS_Patient_List.params["measureId"] == "PHQ9" && currentCondition == 1 && i > 8) {
                                        return;
                                    }
                                    var currentQuestionAnswers = $.grep(PQRS_Patient_List.arrMeasureQuestionnaireAnswers, function (a) {
                                        return a.Question == question;
                                    });
                                    var arrcurrentQuestionAnswers = $.unique(currentQuestionAnswers).join(",");

                                    if (PQRS_Patient_List.params.OnlyShowPatients != "1") {
                                        onclick = "PQRS_Patient_List.openAssociateCodes('" + radBtnNo + "','Question','" + arrcurrentQuestionAnswers.trim() + "','" + radBtnName + "','" + "" + "','" + "" + "','" + PQRS_Patient_List.params["measureId"] + "','" + question.trim() + "','" + arrcurrentQuestionAnswers.trim() + "','" + PQRS_Patient_List.params.PatientId + "','" + CurrentNote + "',$(this).attr('selectedValue'));"
                                    }

                                    var expandCollapseIcon = ' <a id="anchorAssociateCode' + radBtnNo + '" MeasureId="' + PQRS_Patient_List.params["measureId"] + '" PatientId="' + PQRS_Patient_List.params.PatientId + '" NoteId="' + CurrentNote + '" href="#" onclick="' + onclick + '" class="tab_space" title="Associate Code(s)"><i class="fa fa-plus-square"></i></a>';

                                    if (radBtns != "" && MeasureData.substr(-5).indexOf("<br") > -1) {
                                        MeasureData += "<br/>";
                                    }
                                    if (PQRS_Patient_List.params["measureId"] == "PHQ9" && currentCondition == 2) {
                                        MeasureData += radBtns + ' ' + expandCollapseIcon;
                                    }
                                    else {
                                        MeasureData += radBtns + '<br/>' + question + ' ' + expandCollapseIcon;
                                    }

                                    radBtnNo += 1;
                                    MeasureData = MeasureData.replace("<span>" + radBtnText + "</span>", "");
                                    MeasureData = MeasureData.replace('<br/><br/>', '<br/>');
                                    var VBPCPTCode = '';
                                    if (currentReasoning.length > 0 && currentReasoning[0].CodeType == "CPT") {
                                        VBPCPTCode = 'CPT="' + currentReasoning[0].CodeValue + '"';
                                    }
                                    if ((PQRS_Patient_List.params["measureId"] == "PHQ9" && currentCondition == 2) || CurrentRowMainradBtns == "") {
                                        CurrentRowMainradBtns = '<input class="mb-sm disableAll" type="radio" ' + VBPCPTCode + ' ActionResult="' + ExceptionalSNOMED + '" MeasureId="' + PQRS_Patient_List.params["measureId"] + '" PatientId="' + PQRS_Patient_List.params.PatientId + '" NoteId="' + CurrentNote + '" name="' + radBtnName + '" id="MainRad' + radBtnNo + '" />' + radBtns + (PQRS_Patient_List.params["measureId"] == "PHQ9" ? '' : ':')//<span>All of the following:</span>';
                                    }
                                    radBtnNo += 1;
                                    MeasureData = MeasureData.replace('<br/><br/>', '<br/>');
                                });
                                MeasureData = MeasureData.replace('<br/><br/>', '<br/>');
                                $row.append('<td>' + CurrentRowMainradBtns + ((CurrentRowMainradBtns.indexOf("How difficult have these problems made it for you to do your work, take care of things at home, or get along with other people") > -1 || MeasureData.indexOf("<br/>") > -1) ? "" : "<br/>") + MeasureData + '</td>' + '<td class="hidden">' + radBtns + '</td>');
                                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody').append($row);
                                radBtns = "";
                                MeasureData = "";
                            });
                        }
                    });
                });
            }
            //});
        }
    },
    bindPHQ2Rows: function (arrNoOfRows, Questions, NoteId) {
        $.each(arrNoOfRows, function (i, Row) {
            var $row = $('<tr/>');
            $row.attr("MeasureId", PQRS_Patient_List.params["measureId"]);
            $row.attr("PatientId", PQRS_Patient_List.params.PatientId);
            $row.attr("NoteId", NoteId);
            $row.attr("ProviderId", PQRS_Patient_List.params.ProviderId);
            var currentRowQuestion = "";
            var currentQuestionAnswers = [];
            var Ctrlddlanswer = null; //$('<select id="ddlAnswer' + Row + '" name="Answer" class="form-control"></select>');
            if (Row == 1) {
                currentRowQuestion = "<span><b>Over the last 2 weeks, how often have you been bothered by any of the following problems?<span class='required'>*</span></b></span>";
            }
            else {
                Ctrlddlanswer = $('<select id="ddlAnswer' + Row + '" name="Answer" class="form-control"></select>');
                var arrPHQ2QuestionAnswers = [];
                if (ReportsSSRSDashboard.params.PHQ2SelectedQuestionAnswersId != null && ReportsSSRSDashboard.params.PHQ2SelectedQuestionAnswersId != "") {
                    arrPHQ2QuestionAnswers = ReportsSSRSDashboard.params.PHQ2SelectedQuestionAnswersId.split(",");
                }

                currentRowQuestion = Questions[Row - 2];
                currentQuestionAnswers = $.grep(PQRS_Patient_List.arrMeasureQuestionnaireAnswers, function (a) {
                    return a.Question == currentRowQuestion;
                });
                Ctrlddlanswer.empty();
                Ctrlddlanswer.append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(currentQuestionAnswers, function (j, item) {
                    Ctrlddlanswer.append(
                        $('<option/>', {
                            value: item.QuestionAnswersId,
                            html: item.Answer,
                            selected: ($.inArray(item.QuestionAnswersId, arrPHQ2QuestionAnswers) !== -1 ? true : false),
                            QuestionAnswersId: item.QuestionAnswersId,
                            MeasureQuestionnaireId: item.MeasureQuestionnaireId
                        })
                    );
                });
            }
            if (Row == 1) {
                $row.append('<td>' + currentRowQuestion + '</td>' + '<td class=""></td>');
            }
            else {
                //for last question it must be bold EMR-3815
                if(i == 10)
                    $row.append('<td><b>' + currentRowQuestion + '<span class="required">*</span></b></td>' + '<td class="">' + '<select id="ddlAnswer' + Row + '" name="Answer" class="form-control">' + Ctrlddlanswer.html() + '</select>' + '</td>');
                else
                    $row.append('<td>' + currentRowQuestion + '</td>' + '<td class="">' + '<select id="ddlAnswer' + Row + '" name="Answer" class="form-control">' + Ctrlddlanswer.html() + '</select>' + '</td>');
            }

            $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody').append($row);
        });
    },
    get0043Reason: function (radBtnNo, radBtns, MeasureData, Reason, radBtnName) {
        var onclick = "";
        var arrReason = Reason.MeasureDescription.trim().split('&quot;,');
        var arrCodeType = Reason.CodeType.trim().split(',');
        var arrCodeValue = Reason.CodeValue.trim().split('|');
        $.each(arrReason, function (i, CurrentCondition) {
            var item = [];
            item.MeasureDescription = CurrentCondition;
            item.MeasureId = Reason.MeasureId.trim();
            item.GroupOperator = "";
            item.Patientid = Reason.Patientid.trim();
            item.CodeDescription = "";
            item.DescValue = "";
            item.NoteId = Reason.NoteId.trim();
            if (i == 0) {
                item.CodeType = arrCodeType[0];
                item.CodeValue = arrCodeValue[0];
            }
            else if (i == 1) {
                item.CodeType = arrCodeType[1] + arrCodeType[2];
                item.CodeValue = arrCodeValue[1] + arrCodeValue[2];
            }
            else if (i == 2) {
                item.CodeType = arrCodeType[3];
                item.CodeValue = arrCodeValue[3];
            }
            if (PQRS_Patient_List.params.OnlyShowPatients != "1") {
                onclick = "PQRS_Patient_List.openAssociateCodes('" + radBtnNo + "','" + item.CodeType.trim() + "','" + item.CodeValue.trim() + "','" + radBtnName + "','" + item.GroupOperator.trim() + "','" + item.CodeDescription.trim() + "','" + item.MeasureId + "','" + item.MeasureDescription.trim() + "','" + item.DescValue.trim() + "','" + item.Patientid + "','" + item.NoteId + "',$(this).attr('selectedValue'));"
            }

            var expandCollapseIcon = ' <a id="anchorAssociateCode' + radBtnNo + '" MeasureId="' + item.MeasureId + '" PatientId="' + item.Patientid + '" NoteId="' + item.NoteId + '" href="#" onclick="' + onclick + '" class="tab_space" title="Associate Code(s)"><i class="fa fa-plus-square"></i></a>';

            if (radBtns != "") {
                MeasureData += "<br/>";
            }
            if (item.GroupOperator != "OR") {
                MeasureData = item.MeasureDescription.trim() + ' ' + expandCollapseIcon + MeasureData + "<br/>";
            }
            else {
                if (radBtns != "") {
                    MeasureData += "<br/>";
                }
                MeasureData += radBtns + '<br/>' + item.MeasureDescription.trim() + ' ' + expandCollapseIcon;
            }
            radBtnNo += 1;
        });

        return MeasureData;
    },
    //Author: Muhammad Arshad
    //Function Name: openPrintPreview Screen
    //Date: 24-01-2017
    //Description: DB Call to Load Patient Data
    getPatientsData_DbCall: function (PatientIds) {
        var objData = {};
        //if (NotesData != null) {
        //    objData = JSON.parse(NotesData);
        //}
        objData["PatientIds"] = PatientIds;
        objData["commandType"] = "load_patients_cqm";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    //Author: Muhammad Arshad
    //Function Name: openPrintPreview Screen
    //Date: 14/03/2017
    //Description: DB Call to Load Patient RecentNote
    getPatientRecentNote_DbCall: function (PatientId, NotesIds) {
        var objData = {};
        objData["PatientId"] = PatientId;
        objData["NoteId"] = NotesIds;
        objData["commandType"] = "get_patient_recent_note";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    PQRS_GetPatientsFromVisits: function (VisitsID) {
        PQRS_Patient_List.getPatientsFromVisits_DbCall(VisitsID).done(function (response) {
            response = JSON.parse(response);
            if (response.status && response.patientCount > 0) {
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvPatientList tbody').empty();
                $.each(JSON.parse(response.patientList_JSON), function (i, item) {
                    var $row = $('<tr/>');
                    $row.attr('PatientId', item.PatientId);
                    $row.attr('VisitId', item.VisitId);
                    $row.attr('onclick', 'PQRS_Patient_List.PQRS_GetMeasureReasons();');
                    $row.append('<td>' + item.AccountNumber + '</td><td>' + item.PatientName + '</td><td>' + item.DOB + '</td>');
                    $('#' + PQRS_Patient_List.params.PanelID + ' #dgvPatientList tbody').append($row);
                });
            }
        });
    },
    getPatientsFromVisits_DbCall: function (VisitsID) {
        var objData = {};
        objData["VisitsID"] = VisitsID;
        objData["commandType"] = "PQRS_Get_Patients_From_Visits";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSReports");
    },
    PQRS_GetMeasureReasons: function () {
        PQRS_Patient_List.getMeasureReasons_DbCall(PQRS_Patient_List.params.measureId).done(function (response) {
            response = JSON.parse(response);
            if (response.status && response.MeasureReasonsCount > 0) {
                $('#' + PQRS_Patient_List.params.PanelID + ' #detailTable').removeClass('hidden');
                $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody').empty();
                $.each(JSON.parse(response.MeasureReasonsList_JSON), function (i, item) {
                    var $row = $('<tr/>');
                    $row.append('<td><input type="radio" name="MeasureReason" id="' + item.ReasonId + '"/></td><td>' + item.ReasonCode + '</td><td>' + item.Reason + '</td>');
                    $('#' + PQRS_Patient_List.params.PanelID + ' #dgvMeasureReasons tbody').append($row);
                });
            }
        });
    },
    getMeasureReasons_DbCall: function (measureId) {
        var objData = {};
        objData["MeasureId"] = measureId;
        objData["commandType"] = "PQRS_GetMeasureReasons";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "PQRS", "PQRSReports");
    },
    loadNoteInfo_DBCALL: function (NotesId) {
        var objData = {
        };
        objData["NotesId"] = NotesId;
        objData["commandType"] = "load_clinical_note_info";

        var data = JSON.stringify(objData);
        // sNotesch parameter , class name, command name of class
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },
    UnLoad: function (caller) {
        var selectedNoteReasoning = $.grep(PQRS_Patient_List.params.arrcurrentMeasureReasoning, function (reasoning, indx) {
            return reasoning.NoteId == ReportsSSRSDashboard.params.PHQ2SelectedNoteId && reasoning.MeasureId == "PHQ9";
        });
        if (PQRS_Patient_List.params.measureId == "PHQ9"
            && caller != null && caller == "save"
            && ReportsSSRSDashboard.params.PHQ2SelectedPatientId != null && ReportsSSRSDashboard.params.PHQ2SelectedPatientId != ""
            && selectedNoteReasoning.length > 0) {
            utility.DisplayMessages("Please provide information regarding PHQ-9.", 3);
            return;
        }
        if ((PQRS_Patient_List.params.measureId == "PHQ2" || PQRS_Patient_List.params.measureId == "PHQ9") && parseInt(PQRS_Patient_List.params.ResoningPatientId) > 0) {
            if ($("#PatientProfile #hfPatientId").val() == PQRS_Patient_List.params.ResoningPatientId) {
                setPatientBanner(PQRS_Patient_List.params.ResoningPatientId);
            }
        }
        if (PQRS_Patient_List.params != null && PQRS_Patient_List.params.ParentCtrl != null) {
            UnloadActionPan(PQRS_Patient_List.params.ParentCtrl, 'PQRS_Patient_List');
        }
        else
            UnloadActionPan(null, 'PQRS_Patient_List');
    }
}