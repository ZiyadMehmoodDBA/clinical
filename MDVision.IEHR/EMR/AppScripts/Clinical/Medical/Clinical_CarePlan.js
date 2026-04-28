
Clinical_CarePlan = {
    bIsFirstLoad: true,
    params: [],
    ItemsForNoteList: [],
    DetachItemsNoteList: [],

    CurrentTab: '',
    Load: function (params) {
        Clinical_CarePlan.params = params;

        if (Clinical_CarePlan.params.PanelID != 'pnlClinicalCarePlan') {
            Clinical_CarePlan.params.PanelID = Clinical_CarePlan.params.PanelID + ' #pnlClinicalCarePlan';
        } else {
            Clinical_CarePlan.params.PanelID = 'pnlClinicalCarePlan';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_CarePlan.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }

        var self = $('#' + Clinical_CarePlan.params.PanelID);
        self.loadDropDowns(true).done(function () {

        });
        if (Clinical_CarePlan.bIsFirstLoad == true) {
            Clinical_CarePlan.bIsFirstLoad = false;
            //Serialization
            $('#' + Clinical_CarePlan.params.PanelID + ' #frmClinicalCarePlan').data('serialize', $('#' + Clinical_CarePlan.params.PanelID + ' #frmClinicalCarePlan').serialize());
        }
        Clinical_CarePlan.CreateDatePickers();
        Clinical_CarePlan.domReadyFunction();
        $('#' + Clinical_CarePlan.params.PanelID + ' .tabs-custom-body #Goals').addClass('active');
        Clinical_CarePlan.ResetControls("Goals", null, true);
        Clinical_CarePlan.LoadCarePlan("Goals");

        if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_CarePlan.params.PanelID + ' #btnAddCarePlanToNote').removeClass('hidden');
        }
        else {
            $('#' + Clinical_CarePlan.params.PanelID + ' #btnAddCarePlanToNote').addClass('hidden');
        }
    },

    domReadyFunction: function () {
        $(function () {
            $('#' + Clinical_CarePlan.params.PanelID + ' [data-plugin-toggle]').each(function () {
                var $this = $(this),
                    opts = {};


                var pluginOptions = $this.data('plugin-options');
                if (pluginOptions)
                    opts = pluginOptions;

                $this.themePluginToggle(opts);
            });

            $('#' + Clinical_CarePlan.params.PanelID + ' #frmClinicalCarePlan [data-plugin-keyboard-numpad]').keyboard({
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
                            Clinical_CarePlan.ValidateHeightFeet(e, keyboard.$preview);
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
    CreateDatePickers: function () {

        utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + '  #dtCarePlanDate', function () {
        }, true);
        utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + '  #dtConcenDate', function () {
        }, true);
        utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + '  #dtObservationDate', function () {
        }, true);
        utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + '  #dtRiskDate', function () {
        }, true);
        utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + '  #dtInterventionDate', function () {
        }, true);
    },
    changeGoalsField: function (fromEdit) {

        if ($('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #GoalsList #rdoProcedures").prop("checked") == true) {
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #GoalsList #DivICDAutoComplete").addClass('hidden');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #GoalsList #DivCPTAutoComplete").removeClass('hidden');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #GoalsList #CarePlanGoals #ulCarePlanGoals").empty();
            if (fromEdit) {
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #DivCPTAutoComplete").addClass('disableAll');
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #txtProcedure").attr('readonly', true);
            }
            else {
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #DivCPTAutoComplete").removeClass('disableAll');
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #txtProcedure").removeAttr('readonly');
            }
        } else {
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #GoalsList #DivICDAutoComplete").removeClass('hidden');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #GoalsList #DivCPTAutoComplete").addClass('hidden');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #GoalsList #CarePlanGoals #ulCarePlanGoals").empty();
            if (fromEdit) {
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #DivICDAutoComplete").addClass('disableAll');
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #txtDisease").attr('readonly', true);
            }
            else {
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #DivICDAutoComplete").removeClass('disableAll');
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #txtDisease").removeAttr('readonly');
            }
        }
    },

    bindICD9AutoComplete: function (element, type) {
        var descriptionCrtl = $(element);
        var panel = "";
        if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
            panel = "pnlClinicalProgressNote #pnlClinicalCarePlan";
        }
        else {
            panel = "pnlClinicalCarePlan";
        }
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_CarePlan", null, false, panel, type);

    },
    disableEnterKey: function (e) {

        if (e.which == 13) // Enter key = keycode 13
        {
            e.preventDefault();
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

    openSearchPopup: function (searchType, ctrl, type) {
        var controlToLoad = "";
        if (searchType == "ICD") {
            controlToLoad = "Admin_IMOICD";

        }
        else if (searchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (searchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }

        $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #" + ctrl).attr('data-popupunload', 'true');
        var params = [];
        params["FromAdmin"] = "0";

        if (Clinical_CarePlan.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_CarePlan';
        }
        else
            params["ParentCtrl"] = Clinical_CarePlan.params["TabID"];

        if (ctrl != null) {
            params["RefCtrl"] = ctrl;
        }
        params["ContainerProblemDivId"] = type;
        if (controlToLoad != "") {
            if (Clinical_CarePlan.params.TabID == 'clinicalTabProgressNote' && searchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }
    },

    bindAutoComplete: function (element) {

        var hiddenCrtl = $(element);
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_CarePlan", null, true);

    },
    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_CarePlan";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtCPTCode";
        params["ParentCtrlPanelID"] = Clinical_CarePlan.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_CarePlan.params.PanelID);
    },

    CarePlanSave: function (type) {
        var careplanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() != "" ? $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() : "-1";
        if (parseInt(careplanId) > 0) {
            Clinical_CarePlan.params.mode = "Edit";
        }
        else {
            Clinical_CarePlan.params.mode = "Add";
        }

        var isValid = Clinical_CarePlan.ValidateInputData(type);
        if (isValid == false) {
            utility.DisplayMessages("Please enter any value.", 1);
            return false;
        }

        var myJSON = Clinical_CarePlan.getJSONData(type, careplanId);

        if (Clinical_CarePlan.params.mode == "Add") {
            Clinical_CarePlan.saveCarePlan(myJSON, type).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val(response.CarePlanId);
                    Clinical_CarePlan.params.mode = "Edit";
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_CarePlan.ResetControls(type);
                    Clinical_CarePlan.LoadGrid(response, type);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        if (Clinical_CarePlan.params.mode == "Edit") {
            Clinical_CarePlan.saveCarePlan(myJSON, type).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val(response.CarePlanId);
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_CarePlan.ResetControls(type);
                    Clinical_CarePlan.LoadGrid(response, type);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    saveCarePlan: function (carePlanData, type) {

        var objData = JSON.parse(carePlanData);
        if (Clinical_CarePlan.params.PatientId == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_CarePlan.params.PatientId;
        }
        if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        }
        objData["CarePlanType"] = type;
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        objData["commandType"] = "save_careplan";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    LoadGrid: function (response, type) {
        if (type == "Goals") {
            Clinical_CarePlan.GoalsGridLoad(response);
        }
        else if (type == "Concern") {
            Clinical_CarePlan.ConcernGridLoad(response);
        }
        else if (type == "Intervention") {
            Clinical_CarePlan.InterventionGridLoad(response);
        }
        else if (type == "Outcome") {
            Clinical_CarePlan.OutcomesGridLoad(response);
        }
    },

    ValidateInputData: function (type) {
        if (type == 'Concern') {
            var selectedConcern = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Concerns #sectionConcerns #txtConcern");
            var selectedObservation = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Concerns #sectionConcerns #txtObservation");
            var selectedRisk = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Concerns #sectionConcerns #txtRisk");

            if (selectedConcern.val() == "" && selectedObservation.val() == "" && selectedRisk.val() == "") {
                return false;
            }
        }

        return true;
    },
    getJSONData: function (type, careplanId) {

        var self = null;
        var myJSON = "{}";

        if (type == "Goals") {

            self = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #sectionGoals");
            myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);

            var selectedGoal = $('#' + Clinical_CarePlan.params.PanelID + " div#CarePlanGoals ul#ulCarePlanGoals li.active");
            if (selectedGoal.length > 0) {
                objData["GoalId"] = selectedGoal.attr("id");

                var problemSelected = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #rdoProblems").prop("checked");
                if (problemSelected == true) {
                    objData["ICD9Code"] = selectedGoal.attr("icd9code");
                    objData["ICD9CodeDescription"] = selectedGoal.attr("icd9desc");
                    objData["ICD10Code"] = selectedGoal.attr("icd10code");
                    objData["ICD10CodeDescription"] = selectedGoal.attr("icd10desc");
                    objData["SNOMEDID"] = selectedGoal.attr("snomedcode");
                    objData["SNOMEDDescription"] = selectedGoal.attr("snomeddesc");
                } else {
                    objData["CPTCodeId"] = selectedGoal.attr('cptCode');
                    objData["CPTCode"] = objData["CPTCodeId"];
                    objData["CPTDescription"] = selectedGoal.attr('cptdesc');
                    objData["CPTSNOMEDCodeId"] = selectedGoal.attr('snomedcode');
                    objData["CPTSNOMEDDescription"] = selectedGoal.attr('snomeddesc');
                }

            }
        }
        else if (type == "Concern") {

            self = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Concerns #sectionConcerns");
            myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);
            objData["HealthConcernsId"] = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfConcernsId").val();

            var selectedConcern = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Concerns #sectionConcerns #txtConcern");
            if (selectedConcern.length > 0 && selectedConcern.val() != "") {
                objData["Concerns_ICD9Code"] = selectedConcern.attr("icd9code");
                objData["Concerns_ICD9CodeDescription"] = selectedConcern.attr("icd9description");
                objData["Concerns_ICD10Code"] = selectedConcern.attr("icd10code");
                objData["Concerns_ICD10CodeDescription"] = selectedConcern.attr("icd10description");
                objData["Concerns_SNOMEDID"] = selectedConcern.attr("snomedcode");
                objData["Concerns_SNOMEDDescription"] = selectedConcern.attr("snomeddescription");
            }

            var selectedObservation = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Concerns #sectionConcerns #txtObservation");
            if (selectedObservation.length > 0 && selectedObservation.val() != "") {
                objData["Observation_ICD9Code"] = selectedObservation.attr("icd9code");
                objData["Observation_ICD9CodeDescription"] = selectedObservation.attr("icd9description");
                objData["Observation_ICD10Code"] = selectedObservation.attr("icd10code");
                objData["Observation_ICD10CodeDescription"] = selectedObservation.attr("icd10description");
                objData["Observation_SNOMEDID"] = selectedObservation.attr("snomedcode");
                objData["Observation_SNOMEDDescription"] = selectedObservation.attr("snomeddescription");
            }

            var selectedRisk = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Concerns #sectionConcerns #txtRisk");
            if (selectedRisk.length > 0 && selectedRisk.val() != "") {
                objData["Risk_ICD9Code"] = selectedRisk.attr("icd9code");
                objData["Risk_ICD9CodeDescription"] = selectedRisk.attr("icd9description");
                objData["Risk_ICD10Code"] = selectedRisk.attr("icd10code");
                objData["Risk_ICD10CodeDescription"] = selectedRisk.attr("icd10description");
                objData["Risk_SNOMEDID"] = selectedRisk.attr("snomedcode");
                objData["Risk_SNOMEDDescription"] = selectedRisk.attr("snomeddescription");
            }
        }
        else if (type == "Intervention") {

            self = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Interventions #sectionInterventions");
            myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);

            objData["InterventionId"] = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfInterventionId").val();
            var selectedIntervention = $('#' + Clinical_CarePlan.params.PanelID + " div#CarePlanInterventions ul#ulCarePlanInterventions li.active");
            if (selectedIntervention.length > 0) {
                objData["InterventionId"] = selectedIntervention.attr("id");
                objData["ICD9Code"] = selectedIntervention.attr("icd9code");
                objData["ICD9CodeDescription"] = selectedIntervention.attr("icd9desc");
                objData["ICD10Code"] = selectedIntervention.attr("icd10code");
                objData["ICD10CodeDescription"] = selectedIntervention.attr("icd10desc");
                objData["SNOMEDID"] = selectedIntervention.attr("snomedcode");
                objData["SNOMEDDescription"] = selectedIntervention.attr("snomeddesc");

                var goalIds = $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionGoals option:Selected').map(function () {
                    return this.value;
                }).get().join(',');

                if (goalIds.length > 0) {
                    objData["GoalIds"] = goalIds;
                }

                var medicationIds = $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionsMedications option:Selected').map(function () {
                    return this.value;
                }).get().join(',');

                if (medicationIds.length > 0) {
                    objData["MedicationIds"] = medicationIds;
                }
            }
        }
        else if (type == "Outcome") {

            self = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Outcomes #sectionOutcomes");
            myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);

            objData["OutcomesId"] = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfOutcomesId").val();
            var selectedOutcome = $('#' + Clinical_CarePlan.params.PanelID + " div#CarePlanOutcomes ul#ulCarePlanOutcomes li.active");
            if (selectedOutcome.length > 0) {
                objData["InterventionId"] = selectedOutcome.attr("id");
                objData["ICD9Code"] = selectedOutcome.attr("icd9code");
                objData["ICD9CodeDescription"] = selectedOutcome.attr("icd9desc");
                objData["ICD10Code"] = selectedOutcome.attr("icd10code");
                objData["ICD10CodeDescription"] = selectedOutcome.attr("icd10desc");
                objData["SNOMEDID"] = selectedOutcome.attr("snomedcode");
                objData["SNOMEDDescription"] = selectedOutcome.attr("snomeddesc");

                var goalIds = $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeGoals option:Selected').map(function () {
                    return this.value;
                }).get().join(',');

                if (goalIds.length > 0) {
                    objData["GoalIds"] = goalIds;
                }

                var interventionIds = $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeInterventions option:Selected').map(function () {
                    return this.value;
                }).get().join(',');

                if (interventionIds.length > 0) {
                    objData["InterventionIds"] = interventionIds;
                }
            }
        }
        else if (type == "CareTeam") {
            var objData = {};
            objData["ProviderId"] = $('#' + Clinical_CarePlan.params.PanelID + ' #ddlProvider option:selected').attr('value');
            objData["CareTeamId"] = Clinical_CarePlan.params.CareTeamId;

        }
        objData["CarePlanId"] = careplanId;
        objData["CarePlanType"] = type != null ? type : "";
        objData["Comments"] = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #txtCarePlanComments").val();
        myJSON = JSON.stringify(objData);

        return myJSON;
    },

    GoalsGridLoad: function (response) {

        $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan").dataTable().fnDestroy();
        $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan tbody").find("tr").remove();

        if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan thead tr #selectRecordGoals").length == 0) {
                $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan thead tr th:first").after('<th style="width:50px"><input type="checkbox" onchange="Clinical_CarePlan.checkUncheckSelectAll(this);" controlname="selectRecordGoals" id="selectRecordGoals" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan" + " th#selectRecordGoals").remove();
        }

        if (response.GoalsCount > 0) {
            var goalsData = JSON.parse(response.Goals_JSON); //Parsing array to JSON

            $.each(goalsData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "dgvCarePlan" + item.GoalId);
                $row.attr("GoalId", item.GoalId);
                $row.data("rowsdata", JSON.stringify(item));

                var goalValue = '';
                if (item.GoalValue) {
                    goalValue = item.GoalValue + '%';
                }
                else {
                    goalValue = 'N/A';
                }

                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "1") {
                    Checked = " checked";

                } else {
                    Checked = "";
                }

                if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_CarePlan.CacheSelectedItem(this,event,\'Goals\');" id="' + item.GoalId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }

                var IsDefault = false;
                if (item.ICDId != null) {
                    $row.append('<td style="display:none;">' + item.GoalId + '</td>' + SelectionCheckBoxColumn + '<td><a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.GoalDelete(\'' + item.GoalId + '\',false,event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.GoalEdit(\'' + item.GoalId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_CarePlan.showHistory(\'' + item.CarePlanId + '\',\'' + item.GoalId + '\',\'CarePlan_Goals\', event );" title="Record History"> <i class="fa fa-history blue"></i></a></td><td>' + item.ICD10CodeDescription + '</td><td>' + goalValue + '</td><td>' + item.GoalStatusValue + '</td><td>' + (item.GoalDate != null ? utility.RemoveTimeFromDate(null, item.GoalDate) : "") + '</td>');
                }
                else {
                    $row.append('<td style="display:none;">' + item.GoalId + '</td>' + SelectionCheckBoxColumn + '<td><a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.GoalDelete(\'' + item.GoalId + '\',false,event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.GoalEdit(\'' + item.GoalId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_CarePlan.showHistory(\'' + item.CarePlanId + '\',\'' + item.GoalId + '\',\'CarePlan_Goals\', event );" title="Record History"> <i class="fa fa-history blue"></i></a></td><td>' + (item.CPTDescription != null && item.CPTDescription != "" ? item.CPTDescription : "") + '</td><td>' + goalValue + '</td><td>' + item.GoalStatusValue + '</td><td>' + (item.GoalDate != null ? utility.RemoveTimeFromDate(null, item.GoalDate) : "") + '</td>');
                }
                $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan tbody").last().append($row);
            });

            Clinical_CarePlan.CheckUncheckSelectAllOnLoad('dgvCarePlan');
        }
        else {
            $('#' + Clinical_CarePlan.params.PanelID + ' #pnlCarePlan_Result #dgvCarePlan').DataTable({
                "language": {
                    "emptyTable": "No Goal Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "bFilter": false, "bInfo": false, "bPaginate": false,
            });

        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_CarePlan.params.PanelID + ' #pnlCarePlan_Result #dgvCarePlan'))
            ;
        else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
        }

    },

    ConcernGridLoad: function (response) {

        $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern").dataTable().fnDestroy();
        $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern tbody").find("tr").remove();


        if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern thead tr #selectRecordConcern").length == 0) {
                $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern thead tr th:first").after('<th style="width:50px"><input type="checkbox" onchange="Clinical_CarePlan.checkUncheckSelectAll(this);" controlname="selectRecordGoals" id="selectRecordConcern" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        }
        else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern" + " th#selectRecordConcern").remove();
        }

        if (response.ConcernCount > 0) {
            var concernData = JSON.parse(response.Concern_JSON);
            $.each(concernData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "dgvHealthConcern" + item.HealthConcernsId);
                $row.attr("HealthConcernId", item.HealthConcernsId);
                $row.data("rowsdata", JSON.stringify(item));

                var statusVal = Clinical_CarePlan.GetConcernStatusValue(item);
                var concernType = Clinical_CarePlan.GetConcernTypeValue(item);
                var date = Clinical_CarePlan.GetConcernDate(item);

                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "1") {
                    Checked = " checked";

                } else {
                    Checked = "";
                }

                if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_CarePlan.CacheSelectedItem(this,event,\'Concern\');" id="' + item.HealthConcernsId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }

                $row.append('<td style="display:none;">' + item.HealthConcernsId + '</td>' + SelectionCheckBoxColumn + '<td><a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.ConcernDelete(\'' + item.HealthConcernsId + '\',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.ConcernEdit(\'' + item.HealthConcernsId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_CarePlan.showHistory(\'' + item.CarePlanId + '\',\'' + item.HealthConcernsId + '\',\'CarePlan_HealthConcerns\', event );" title="Record History"> <i class="fa fa-history blue"></i></a></td><td>' + concernType + '</td><td>' + statusVal + '</td><td>' + date + '</td>');

                $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern tbody").last().append($row);
            });

            Clinical_CarePlan.CheckUncheckSelectAllOnLoad('dgvHealthConcern');
        }
        else {
            $('#' + Clinical_CarePlan.params.PanelID + ' #pnlCarePlan_Result #dgvHealthConcern').DataTable({
                "language": {
                    "emptyTable": "No Concern Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "bFilter": false, "bInfo": false, "bPaginate": false,
            });

        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_CarePlan.params.PanelID + ' #pnlCarePlan_Result #dgvHealthConcern'))
            ;
        else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
        }
    },

    fillGoalDetail: function (obj) {

        var goalId = $(obj).attr('id');
        $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan div#CarePlanGoals ul#ulCarePlanGoals li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == goalId) {
                if ($(this).hasClass("active") == false) {
                    $(this).addClass("active");
                    $(this).find('div').css('display', '');
                }
            }
            else {
                $(this).removeClass("active");
                $(this).find('div').css('display', 'none');
            }
        });
    },

    GetConcernStatusValue: function (concern) {

        var statusVal = '';
        if (concern.ConcernsStatusValue != '' && concern.RiskStatusValue != '') {
            statusVal = concern.ConcernsStatusValue + '/' + concern.RiskStatusValue;
        }
        else if (concern.ConcernsStatusValue != '') {
            statusVal = concern.ConcernsStatusValue;
        }
        else if (concern.RiskStatusValue != '') {
            statusVal = concern.RiskStatusValue;
        }
        else {
            statusVal = '';
        }
        return statusVal;
    },

    GetConcernTypeValue: function (concern) {

        var concernType = "";
        if (concern.Concerns_ICD9Code != null || concern.Concerns_ICD10Code != null) {
            concernType = "Concern";
        }
        if (concern.Observation_ICD9Code != null || concern.Observation_ICD10Code != null) {
            if (concernType != '') {
                concernType += "/Observation";
            }
            else {
                concernType += "Observation";
            }
        }
        if (concern.Risk_ICD9Code != null || concern.Risk_ICD10Code != null) {
            if (concernType != '') {
                concernType += "/Risk";
            }
            else {
                concernType += "Risk";
            }
        }

        return concernType;
    },

    ResetControls: function (type, caller, fromLoad) {

        if (type == "Goals") {
            if (caller != 'Clearbtn') {
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #CarePlanGoals #ulCarePlanGoals").empty();
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #rdoProblems").prop("checked", true);
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #sectionGoals").addClass('disableAll');
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfGoalId").val('-1');
                Clinical_CarePlan.changeGoalsField();
            }
            var details = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Goals #sectionGoals");
            $(details).resetAllControls(null);

            utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + '  #dtCarePlanDate', function () {
            }, true);

            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan").removeClass('hidden');
        }
        if (type == "Concern") {
            var details = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Concerns #sectionConcerns");
            $(details).resetAllControls(null);
            utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + ' #frmClinicalCarePlan #Concerns #dtConcenDate', function () {
            }, true);
            utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + ' #frmClinicalCarePlan #Concerns #dtObservationDate', function () {
            }, true);
            utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + ' #frmClinicalCarePlan #Concerns #dtRiskDate', function () {
            }, true);
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern").removeClass('hidden');

            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes").addClass('hidden');

            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfConcernsId").val('-1');
            $('#' + Clinical_CarePlan.params.PanelID + ' #concernInfoDiv').addClass('disableAll');
            $('#' + Clinical_CarePlan.params.PanelID + ' #observationInfoDiv').addClass('disableAll');
            $('#' + Clinical_CarePlan.params.PanelID + ' #riskInfoDiv').addClass('disableAll');
        }
        if (type == "Intervention") {
            var details = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Interventions #sectionInterventions");
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionGoals').multiselect('clearSelection');
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionsMedications').multiselect('clearSelection');

            if (caller != 'Clearbtn') {
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #CarePlanInterventions #ulCarePlanInterventions").empty();
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfInterventionId").val('-1');
                $(details).addClass('disableAll');
            }
            else {
                $('#' + Clinical_CarePlan.params.PanelID + " ddlInterventionGoals option").removeAttr("selected");
                $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionGoals').multiselect("refresh");
                $('#' + Clinical_CarePlan.params.PanelID + " ddlInterventionsMedications option").removeAttr("selected");
                $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionsMedications').multiselect("refresh");
            }

            $(details).resetAllControls(null);
            utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + ' #frmClinicalCarePlan #Interventions #dtInterventionDate', function () {
            }, true);

            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions").removeClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes").addClass('hidden');
        }

        if (type == "Outcome") {
            var details = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #Outcomes #sectionOutcomes");
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeGoals').multiselect('clearSelection');
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeInterventions').multiselect('clearSelection');

            if (caller != 'Clearbtn') {
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #CarePlanOutcomes #ulCarePlanOutcomes").empty();
                $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfOutcomesId").val('-1');
                $(details).addClass('disableAll');
            }
            else {
                $('#' + Clinical_CarePlan.params.PanelID + " ddlOutcomeGoals option").removeAttr("selected");
                $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeGoals').multiselect("refresh");
                $('#' + Clinical_CarePlan.params.PanelID + " ddlOutcomeInterventions option").removeAttr("selected");
                $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeInterventions').multiselect("refresh");
            }
            $(details).resetAllControls(null);
            utility.CreateDatePicker(Clinical_CarePlan.params.PanelID + ' #frmClinicalCarePlan #Outcomes #dtOutcomeDate', function () {
            }, true);

            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes").removeClass('hidden');

            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions").addClass('hidden');
        }
        else if (type == "CareTeam") {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvCarePlan").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvHealthConcern").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions").addClass('hidden');
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes").addClass('hidden');
        }

        Clinical_CarePlan.CurrentTab = type;


        if (fromLoad == true) {
            Clinical_CarePlan.ItemsForNoteList = [];
            Clinical_CarePlan.DetachItemsNoteList = [];
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val('-1');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfGoalId").val('-1');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfConcernsId").val('-1');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfInterventionId").val('-1');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfOutcomesId").val('-1');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #txtCarePlanComments").val('');
            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCareTeamId").val('-1');
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionGoals').multiselect("destroy");
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionsMedications').multiselect("destroy");
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeGoals').multiselect("destroy");
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeInterventions').multiselect('destroy');
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionGoals').empty();
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionsMedications').empty();
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeInterventions').empty();
            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeGoals').empty();
            $('#' + Clinical_CarePlan.params.PanelID + " #dgvCareTeam tbody").empty();
            $('#' + Clinical_CarePlan.params.PanelID + " #dgvCareTeam thead").empty();
        }
    },

    LoadCarePlan: function (type) {
        var careplanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() != "" ? $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() : "-1";
        Clinical_CarePlan.SetTabActiveClass(type);
        Clinical_CarePlan.LoadCarePlan_DbCall(type).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.CarePlanCount > 0) {
                    var carePlanInfo = JSON.parse(response.CarePlan_JSON);
                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val(carePlanInfo[0].CarePlanId);
                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #txtCarePlanComments").val(carePlanInfo[0].Comments);
                }
                Clinical_CarePlan.ResetControls(type);
                if (type == "Goals") {
                    Clinical_CarePlan.GoalsGridLoad(response);
                }
                else if (type == "Concern") {
                    Clinical_CarePlan.ConcernGridLoad(response);
                }
                else if (type == "Intervention") {
                    var objDeffered = $.Deferred();
                    var self = $('#' + Clinical_CarePlan.params.PanelID);
                    var patientId = $("#PatientProfile #hfPatientId").val();
                    var carePlanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() != "" ? $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() : "-1";
                    var data = "ID=" + carePlanId;
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionGoals').multiselect("destroy");
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionGoals').empty();
                    self.find('#Interventions #GoalsDiv').loadDropDowns(true, data).done(function () {
                        $('#' + Clinical_CarePlan.params.PanelID + ' #sectionInterventions #ddlInterventionGoals').multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                    });

                    var data = "ID=" + patientId;
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionsMedications').multiselect("destroy");
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionsMedications').empty();
                    self.find('#Interventions #MedicationsDiv').loadDropDowns(true, data).done(function () {
                        $('#' + Clinical_CarePlan.params.PanelID + ' #sectionInterventions #ddlInterventionsMedications').multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                    });
                    objDeffered.resolve();

                    Clinical_CarePlan.InterventionGridLoad(response);
                }
                else if (type == "Outcome") {
                    var objDeffered = $.Deferred();
                    var self = $('#' + Clinical_CarePlan.params.PanelID);
                    var carePlanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() != "" ? $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() : "-1";
                    var data = "ID=" + carePlanId;
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeGoals').multiselect("destroy");
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeGoals').empty();
                    self.find('#Outcomes #OutcomeGoalsDiv').loadDropDowns(true, data).done(function () {
                        $('#' + Clinical_CarePlan.params.PanelID + ' #sectionOutcomes #ddlOutcomeGoals').multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                    });
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeInterventions').multiselect('destroy');
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeInterventions').empty();
                    self.find('#Outcomes #OutcomeInteventionsDiv').loadDropDowns(true, data).done(function () {
                        $('#' + Clinical_CarePlan.params.PanelID + ' #sectionOutcomes #ddlOutcomeInterventions').multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            enableCaseInsensitiveFiltering: true,
                            maxHeight: 247
                        });
                    });
                    objDeffered.resolve();

                    Clinical_CarePlan.OutcomesGridLoad(response);
                }
                else if (type == "CareTeam") {
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ddlProvider').change(function () {
                        var providerId = $('#' + Clinical_CarePlan.params.PanelID + ' #ddlProvider option:selected').attr('value');
                        Clinical_CarePlan.CareTeamSelect(providerId);
                    });

                    if (response.CareTeamCount > 0) {
                        var careTeam = JSON.parse(response.CareTeam_JSON);
                        if (careTeam != null && careTeam.length > 0) {
                            $('#' + Clinical_CarePlan.params.PanelID + ' #ddlProvider').val(careTeam[0].ProviderId);
                            Clinical_CarePlan.CareTeamMemberSelect(careTeam[0].CareTeamId, careTeam[0].Name, careTeam[0].ProviderName, careTeam[0].CareManagerName, careTeam[0].CareCoordinatorName, careTeam[0].CareGiverName, careTeam[0].Specialty, careTeam[0].ProviderPhone, careTeam[0].CareManagerPhone, careTeam[0].CareCoordinatorPhone, careTeam[0].CareGiverPhone, true);
                        }
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadCarePlan_DbCall: function (type) {

        var objData = {};
        if (Clinical_CarePlan.params.PatientId == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_CarePlan.params.PatientId;
        }
        var careplanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() != "" ? $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() : "-1";
        objData["CarePlanId"] = careplanId;
        objData["CarePlanType"] = type;
        if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
            objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        }
        else {
            objData["ProviderId"] = 0;
        }

        objData["commandType"] = "fill_careplan";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    GoalEdit: function (goalId, ev) {

        var carePlanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() != "" ? $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() : "-1";

        Clinical_CarePlan.LoadCarePlanGoal_DbCall(carePlanId, goalId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.GoalsCount > 0) {
                    var carePlanGoal = JSON.parse(response.Goals_JSON);
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanGoals').empty();
                    Clinical_CarePlan.ResetControls("Goals");
                    var CPTDescription = "";
                    var ICD10CodeDescription = "";
                    var li = "";
                    if (carePlanGoal[0].CPTDescription != null) {
                        CPTDescription = carePlanGoal[0].CPTDescription;
                    }
                    if (carePlanGoal[0].ICD10CodeDescription != null) {
                        ICD10CodeDescription = carePlanGoal[0].ICD10CodeDescription;
                    }
                    if (carePlanGoal[0].ICDId != null) {
                        li = "<li class='active' style=' min-height: 25px;' id=" + carePlanGoal[0].GoalId + " onclick='Clinical_CarePlan.fillGoalDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + carePlanGoal[0].ICD9Code + "\" icd9Desc=\"" + carePlanGoal[0].ICD9CodeDescription + "\" icd10Code=\"" + carePlanGoal[0].ICD10Code + "\" icd10Desc=\"" + carePlanGoal[0].ICD10CodeDescription + "\" snomedCode=\"" + carePlanGoal[0].SNOMEDID + "\" snomedDesc=\"" + carePlanGoal[0].SNOMEDDescription + "\"><a href='#'>" + ICD10CodeDescription + "<span class='removeIconListHover' onclick='Clinical_CarePlan.GoalDelete(\"" + carePlanGoal[0].GoalId + "\", true, event);'><i class='fa fa-close'></i></span></a></li>";
                        $('#' + Clinical_CarePlan.params.PanelID + ' #rdoProblems').prop('checked', true);
                    }
                    else {
                        li = "<li class='active' 'active' style=' min-height: 25px;'  id=" + carePlanGoal[0].GoalId + " onclick='Clinical_CarePlan.fillGoalDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' cptCode=\"" + carePlanGoal[0].CPTCode + "\" cptDesc=\"" + carePlanGoal[0].CPTDescription + "\" snomedCode=\"" + carePlanGoal[0].CPTSNOMEDID + "\" snomedDesc=\"" + carePlanGoal[0].CPTSNOMEDDescription + "\"><a href='#'>" + CPTDescription + "<span class='removeIconListHover' onclick='Clinical_CarePlan.GoalDelete(\"" + carePlanGoal[0].GoalId + "\", true,event);'><i class='fa fa-close'></i></span></a></li>";
                        $('#' + Clinical_CarePlan.params.PanelID + ' #rdoProcedures').prop('checked', true);
                    }
                    Clinical_CarePlan.changeGoalsField(true);
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanGoals').append(li);
                    $('#' + Clinical_CarePlan.params.PanelID + ' #sectionGoals').removeClass('disableAll');

                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val(carePlanGoal[0].CarePlanId);
                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfGoalId").val(carePlanGoal[0].GoalId);
                    var self = $('#' + Clinical_CarePlan.params.PanelID);
                    utility.bindMyJSONByName(true, carePlanGoal[0], false, self).done(function () {
                    });
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadCarePlanGoal_DbCall: function (carePlanId, goalId) {

        var objData = {};
        objData["CarePlanId"] = carePlanId;
        objData["GoalId"] = goalId;

        if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
            objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        }
        else {
            objData["ProviderId"] = 0;
        }
        objData["commandType"] = "fill_careplangoal";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    GoalDelete: function (goalId, isFromList, ev) {
        ev.stopPropagation();
        utility.myConfirm('54', function () {
            if (goalId <= 0) {
                $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanGoals').find('li#' + goalId).remove();
                Clinical_CarePlan.ResetControls("Goals");
            }
            else {
                Clinical_CarePlan.GoalDelete_DbCall(goalId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        if (isFromList) {
                            $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanGoals').find('li#' + goalId).remove();
                            Clinical_CarePlan.ResetControls("Goals");
                        }
                        Clinical_CarePlan.LoadCarePlan("Goals");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }

        }, function () { },
                    '23'
        );
    },
    GoalDelete_DbCall: function (goalId) {

        var objData = {};
        objData["GoalId"] = goalId;
        objData["CarePlanId"] = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val();
        objData["commandType"] = "delete_careplangoal";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    ConcernEdit: function (ConcernId, ev) {

        var carePlanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() != "" ? $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() : "-1";

        Clinical_CarePlan.LoadCarePlanHealthConcern_DbCall(carePlanId, ConcernId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ConcernCount > 0) {
                    var concern = JSON.parse(response.Concern_JSON);
                    Clinical_CarePlan.ResetControls("Concern");

                    if (concern[0].Concerns_ICD9Code != "" || concern[0].Concerns_ICD10Code) {
                        var concernCtrl = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #txtConcern");
                        $(concernCtrl).val(concern[0].Concerns_ICD10CodeDescription);

                        $(concernCtrl).attr('icd9Code', concern[0].Concerns_ICD9Code);
                        $(concernCtrl).attr('icd9Description', concern[0].Concerns_ICD9CodeDescription);
                        $(concernCtrl).attr('icd10Code', concern[0].Concerns_ICD10Code);
                        $(concernCtrl).attr('icd10Description', concern[0].Concerns_ICD10CodeDescription);
                        $(concernCtrl).attr('snomedCode', concern[0].Concerns_SNOMEDID);
                        $(concernCtrl).attr('snomedDescription', concern[0].Concerns_SNOMEDDescription);

                        $('#' + Clinical_CarePlan.params.PanelID + ' #concernInfoDiv').removeClass('disableAll');
                    }

                    if (concern[0].Observation_ICD9Code != "" || concern[0].Observation_ICD10Code) {
                        var observationCtrl = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #txtObservation");
                        $(observationCtrl).val(concern[0].Observation_ICD10CodeDescription);

                        $(observationCtrl).attr('icd9Code', concern[0].Observation_ICD9Code);
                        $(observationCtrl).attr('icd9Description', concern[0].Observation_ICD9CodeDescription);
                        $(observationCtrl).attr('icd10Code', concern[0].Observation_ICD10Code);
                        $(observationCtrl).attr('icd10Description', concern[0].Observation_ICD10CodeDescription);
                        $(observationCtrl).attr('snomedCode', concern[0].Observation_SNOMEDID);
                        $(observationCtrl).attr('snomedDescription', concern[0].Observation_SNOMEDDescription);

                        $('#' + Clinical_CarePlan.params.PanelID + ' #observationInfoDiv').removeClass('disableAll');
                    }

                    if (concern[0].Risk_ICD9Code != "" || concern[0].Risk_ICD10Code) {
                        var riskCtrl = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #txtRisk");
                        $(riskCtrl).val(concern[0].Risk_ICD10CodeDescription);

                        $(riskCtrl).attr('icd9Code', concern[0].Risk_ICD9Code);
                        $(riskCtrl).attr('icd9Description', concern[0].Risk_ICD9CodeDescription);
                        $(riskCtrl).attr('icd10Code', concern[0].Risk_ICD10Code);
                        $(riskCtrl).attr('icd10Description', concern[0].Risk_ICD10CodeDescription);
                        $(riskCtrl).attr('snomedCode', concern[0].Risk_SNOMEDID);
                        $(riskCtrl).attr('snomedDescription', concern[0].Risk_SNOMEDDescription);

                        $('#' + Clinical_CarePlan.params.PanelID + ' #riskInfoDiv').removeClass('disableAll');
                    }

                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val(concern[0].CarePlanId);
                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfConcernsId").val(concern[0].HealthConcernsId);
                    var self = $('#' + Clinical_CarePlan.params.PanelID);
                    utility.bindMyJSONByName(true, concern[0], false, self).done(function () {
                        //utility.RemoveTimeFromDate(null, item.ConcernsDate)
                    });
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadCarePlanHealthConcern_DbCall: function (carePlanId, concernId) {

        var objData = {};
        objData["CarePlanId"] = carePlanId;
        objData["HealthConcernsId"] = concernId;

        objData["commandType"] = "fill_careplanConcern";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    ConcernDelete: function (concernId, ev) {
        ev.stopPropagation();
        utility.myConfirm('55', function () {
            Clinical_CarePlan.ConcernDelete_DbCall(concernId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Clinical_CarePlan.LoadCarePlan("Concern");
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }, function () { },
                    '23'
        );
    },

    ConcernDelete_DbCall: function (concernId) {

        var objData = {};
        objData["HealthConcernsId"] = concernId;
        objData["CarePlanId"] = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val();
        objData["commandType"] = "delete_careplanconcern";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    EnableDisableControls: function (obj, type) {

        if (type == 'Concern') {
            if ($(obj).val() == "") {
                $('#' + Clinical_CarePlan.params.PanelID + " #concernInfoDiv").addClass('disableAll');
            }
            else {
                $('#' + Clinical_CarePlan.params.PanelID + " #concernInfoDiv").removeClass('disableAll');
            }
        }
        else if (type == 'Observation') {
            if ($(obj).val() == "") {
                $('#' + Clinical_CarePlan.params.PanelID + " #observationInfoDiv").addClass('disableAll');
            }
            else {
                $('#' + Clinical_CarePlan.params.PanelID + " #observationInfoDiv").removeClass('disableAll');
            }

        }
        else if (type == 'Risk') {
            if ($(obj).val() == "") {
                $('#' + Clinical_CarePlan.params.PanelID + " #riskInfoDiv").addClass('disableAll');
            }
            else {
                $('#' + Clinical_CarePlan.params.PanelID + " #riskInfoDiv").removeClass('disableAll');
            }
        }
    },

    fillInterventionDetail: function (obj) {
        var interventionId = $(obj).attr('id');
        $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan div#CarePlanInterventions ul#ulCarePlanInterventions li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == interventionId) {
                if ($(this).hasClass("active") == false) {
                    $(this).addClass("active");
                    $(this).find('div').css('display', '');
                }
            }
            else {
                $(this).removeClass("active");
                $(this).find('div').css('display', 'none');
            }
        });
    },

    IntializeMultiSelectDropDown: function () {
        $('#' + Clinical_CarePlan.params.PanelID + ' #sectionInterventions #ddlInterventionGoals').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247
        });

        $('#' + Clinical_CarePlan.params.PanelID + ' #sectionInterventions #ddlInterventionsMedications').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 247
        });
    },
    ShowCarePlanGoals: function () {
        $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanTabsItems a[href="#Goals"]').trigger('click');
        Clinical_CarePlan.LoadCarePlan('Goals');
    },
    ShowMedications: function () {

        Clinical_CarePlan.unLoadTab();
        var med = $("#sortableMedical").find('li#clinicalMenu_Medical_Medications a[href="#mstrTabMedical"]');
        if (med != null) {
            med.trigger('click');
        }
    },

    InterventionGridLoad: function (response) {

        $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions").dataTable().fnDestroy();
        $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions tbody").find("tr").remove();

        if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions thead tr #selectRecordIntervention").length == 0) {
                $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions thead tr th:first").after('<th style="width:50px"><input type="checkbox" onchange="Clinical_CarePlan.checkUncheckSelectAll(this);" controlname="selectRecordGoals" id="selectRecordIntervention" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        }
        else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions" + " th#selectRecordIntervention").remove();
        }

        if (response.InterventionCount > 0) {
            var interventions = JSON.parse(response.Intervention_JSON);

            $.each(interventions, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "dgvInterventions" + item.InterventionId);
                $row.attr("InterventionId", item.InterventionId);
                $row.data("rowsdata", JSON.stringify(item));

                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "1") {
                    Checked = " checked";

                } else {
                    Checked = "";

                }

                if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_CarePlan.CacheSelectedItem(this,event,\'Intervention\');" id="' + item.InterventionId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                var CodeDescription = '';
                if (item.ICD10CodeDescription) {
                    CodeDescription = item.ICD10CodeDescription;
                }
                else if (item.SNOMEDDescription) {
                    CodeDescription = item.SNOMEDDescription;
                }
                else if (item.LOINCDescription) {
                    CodeDescription = item.LOINCDescription;
                }

                $row.append('<td style="display:none;">' + item.InterventionId + '</td>' + SelectionCheckBoxColumn + '<td><a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.InterventionDelete(\'' + item.InterventionId + '\',false,event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.InterventionEdit(\'' + item.InterventionId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_CarePlan.showHistory(\'' + item.CarePlanId + '\',\'' + item.InterventionId + '\',\'CarePlan_Interventions\', event );" title="Record History"> <i class="fa fa-history blue"></i></a></td><td>' + CodeDescription + '</td><td>' + item.InterventionStatusValue + '</td><td>' + (item.InterventionDate != null ? utility.RemoveTimeFromDate(null, item.InterventionDate) : "") + '</td>');
                $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions tbody").last().append($row);
            });
            Clinical_CarePlan.CheckUncheckSelectAllOnLoad('dgvInterventions');
        }
        else {
            $('#' + Clinical_CarePlan.params.PanelID + ' #pnlCarePlan_Result #dgvInterventions').DataTable({
                "language": {
                    "emptyTable": "No Intervention Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "bFilter": false, "bInfo": false, "bPaginate": false,
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_CarePlan.params.PanelID + ' #pnlCarePlan_Result #dgvInterventions'))
            ;
        else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvInterventions").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
        }
    },

    InterventionEdit: function (interventionId, ev) {

        var carePlanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() != "" ? $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() : "-1";

        Clinical_CarePlan.LoadCarePlanIntervention_DbCall(carePlanId, interventionId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.InterventionCount > 0) {
                    var intervention = JSON.parse(response.Intervention_JSON);
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanInterventions').empty();
                    Clinical_CarePlan.ResetControls("Intervention");
                    var li = "";
                    var CodeDescription = '';
                    if (intervention[0].ICD10CodeDescription) {
                        CodeDescription = intervention[0].ICD10CodeDescription;
                    }
                    else if (intervention[0].SNOMEDDescription) {
                        CodeDescription = intervention[0].SNOMEDDescription;
                    }
                    else if (intervention[0].LOINCDescription) {
                        CodeDescription = intervention[0].LOINCDescription;
                    }
                    li = "<li class='active' id=" + intervention[0].InterventionId + " onclick='Clinical_CarePlan.fillInterventionDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + intervention[0].ICD9Code + "\" icd9Desc=\"" + intervention[0].ICD9CodeDescription + "\" icd10Code=\"" + intervention[0].ICD10Code + "\" icd10Desc=\"" + intervention[0].ICD10CodeDescription + "\" snomedCode=\"" + intervention[0].SNOMEDID + "\" snomedDesc=\"" + intervention[0].SNOMEDDescription + "\"><a href='#'>" + CodeDescription + "<span class='removeIconListHover' onclick='Clinical_CarePlan.InterventionDelete(\"" + intervention[0].InterventionId + "\", true, event);'><i class='fa fa-close'></i></span></a></li>";

                    $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanInterventions').append(li);
                    $('#' + Clinical_CarePlan.params.PanelID + ' #sectionInterventions').removeClass('disableAll');

                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val(intervention[0].CarePlanId);
                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfInterventionId").val(intervention[0].InterventionId);
                    var self = $('#' + Clinical_CarePlan.params.PanelID);
                    utility.bindMyJSONByName(true, intervention[0], false, self).done(function () {
                        $('#' + Clinical_CarePlan.params.PanelID + " ddlInterventionGoals option").removeAttr("selected");
                        $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionGoals').multiselect('clearSelection');
                        $('#' + Clinical_CarePlan.params.PanelID + " ddlInterventionsMedications option").removeAttr("selected");
                        $('#' + Clinical_CarePlan.params.PanelID + ' #ddlInterventionsMedications').multiselect('clearSelection');

                        if (intervention[0].GoalIds != null) {
                            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #ddlInterventionGoals").val(intervention[0].GoalIds.split(','));
                            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #ddlInterventionGoals").multiselect("refresh");
                        }

                        if (intervention[0].MedicationIds != null) {
                            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #ddlInterventionsMedications").val(intervention[0].MedicationIds.split(','));
                            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #ddlInterventionsMedications").multiselect("refresh");
                        }

                    });
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadCarePlanIntervention_DbCall: function (carePlanId, interventionId) {

        var objData = {};
        objData["CarePlanId"] = carePlanId;
        objData["InterventionId"] = interventionId;

        objData["commandType"] = "fill_careplanintervention";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    InterventionDelete: function (interventionId, isFromList, ev) {
        ev.stopPropagation();
        utility.myConfirm('56', function () {
            if (interventionId <= 0) {
                $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanInterventions').find('li#' + interventionId).remove();
                Clinical_CarePlan.ResetControls("Intervention");
            }
            else {
                Clinical_CarePlan.InterventionDelete_DbCall(interventionId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        if (isFromList) {
                            $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanInterventions').find('li#' + interventionId).remove();
                            Clinical_CarePlan.ResetControls("Intervention");
                        }
                        Clinical_CarePlan.LoadCarePlan("Intervention");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }

        }, function () { },
                    '23'
        );
    },
    InterventionDelete_DbCall: function (interventionId) {

        var objData = {};
        objData["InterventionId"] = interventionId;
        objData["CarePlanId"] = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val();
        objData["commandType"] = "delete_careplanintervention";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    fillOutcomeDetail: function (obj) {
        var outcomeId = $(obj).attr('id');
        $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan div#CarePlanOutcomes ul#ulCarePlanOutcomes li").each(function (i, item) {
            if ($(this).attr("id") != null && $(this).attr("id") == outcomeId) {
                if ($(this).hasClass("active") == false) {
                    $(this).addClass("active");
                    $(this).find('div').css('display', '');
                }
            }
            else {
                $(this).removeClass("active");
                $(this).find('div').css('display', 'none');
            }
        });
    },

    ShowInterventions: function () {
        $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanTabsItems a[href="#Interventions"]').trigger('click');
        Clinical_CarePlan.LoadCarePlan('Intervention');
    },

    OutcomesGridLoad: function (response) {

        $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes").dataTable().fnDestroy();
        $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes tbody").find("tr").remove();

        if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
            if ($("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes thead tr #selectRecordOutcome").length == 0) {
                $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes thead tr th:first").after('<th style="width:50px"><input type="checkbox" onchange="Clinical_CarePlan.checkUncheckSelectAll(this);" controlname="selectRecordGoals" id="selectRecordOutcome" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        }
        else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes" + " th#selectRecordOutcome").remove();
        }

        if (response.OutcomesCount > 0) {
            var outcomes = JSON.parse(response.Outcomes_JSON);

            $.each(outcomes, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("id", "dgvOutcomes" + item.OutcomesId);
                $row.attr("OutcomesId", item.OutcomesId);
                $row.data("rowsdata", JSON.stringify(item));

                var outcome = item.OutcomeValue != null ? item.OutcomeValue : "";

                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "1") {
                    Checked = " checked";
                } else {
                    Checked = "";
                }

                if (Clinical_CarePlan.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onclick="Clinical_CarePlan.CacheSelectedItem(this,event,\'Outcome\');" id="' + item.OutcomesId + '" name="SelectCheckBoxOrder" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }

                $row.append('<td style="display:none;">' + item.OutcomesId + '</td>' + SelectionCheckBoxColumn + '<td><a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.OutcomeDelete(\'' + item.OutcomesId + '\',false,event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="Clinical_CarePlan.OutcomeEdit(\'' + item.OutcomesId + '\',event);" title="Edit Record"><i class="fa fa-edit black"></i></a>&nbsp;<a class="btn  btn-xs" href="#" onclick="Clinical_CarePlan.showHistory(\'' + item.CarePlanId + '\',\'' + item.OutcomesId + '\',\'CarePlan_Outcomes\', event );" title="Record History"> <i class="fa fa-history blue"></i></a></td><td>' + (item.ICD10CodeDescription != null ? item.ICD10CodeDescription : item.LOINCDescription) + '</td><td>' + outcome + '</td><td>' + (item.OutcomeDate != null ? utility.RemoveTimeFromDate(null, item.OutcomeDate) : "") + '</td>');
                $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes tbody").last().append($row);
            });

            Clinical_CarePlan.CheckUncheckSelectAllOnLoad('dgvOutcomes');
        }
        else {
            $('#' + Clinical_CarePlan.params.PanelID + ' #pnlCarePlan_Result #dgvOutcomes').DataTable({
                "language": {
                    "emptyTable": "No Outcome Found."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }], "bDestroy": true, "bFilter": false, "bInfo": false, "bPaginate": false,
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Clinical_CarePlan.params.PanelID + ' #pnlCarePlan_Result #dgvOutcomes'))
            ;
        else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #dgvOutcomes").DataTable({ "searching": false, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]] });
        }
    },

    OutcomeEdit: function (outcomeId, ev) {

        var carePlanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() != "" ? $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val() : "-1";

        Clinical_CarePlan.LoadCarePlanOutcome_DbCall(carePlanId, outcomeId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.OutcomesCount > 0) {
                    var outcome = JSON.parse(response.Outcomes_JSON);
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanOutcomes').empty();
                    Clinical_CarePlan.ResetControls("Outcome");
                    var li = "";
                    if (outcome[0].ICDId != null) {
                        li = "<li class='active' id=" + outcome[0].OutcomesId + " onclick='Clinical_CarePlan.fillOutcomeDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + outcome[0].ICD9Code + "\" icd9Desc=\"" + outcome[0].ICD9CodeDescription + "\" icd10Code=\"" + outcome[0].ICD10Code + "\" icd10Desc=\"" + outcome[0].ICD10CodeDescription + "\" snomedCode=\"" + outcome[0].SNOMEDID + "\" snomedDesc=\"" + outcome[0].SNOMEDDescription + "\"><a href='#'>" + outcome[0].ICD10CodeDescription + "<span class='removeIconListHover' onclick='Clinical_CarePlan.OutcomeDelete(\"" + outcome[0].OutcomesId + "\", true, event);'><i class='fa fa-close'></i></span></a></li>";
                    }
                    else if (outcome[0].LOINCCode != null) {
                        li = "<li class='active' id=" + outcome[0].OutcomesId + " onclick='Clinical_CarePlan.fillOutcomeDetail(this, event);' onmouseover='Clinical_CarePlan.showIcon(this);' onmouseout='Clinical_CarePlan.hideIcon(this);' icd9Code=\"" + outcome[0].ICD9Code + "\" icd9Desc=\"" + outcome[0].ICD9CodeDescription + "\" icd10Code=\"" + outcome[0].ICD10Code + "\" LOINCDescription=\"" + outcome[0].LOINCDescription + "\" snomedCode=\"" + outcome[0].SNOMEDID + "\" snomedDesc=\"" + outcome[0].SNOMEDDescription + "\"><a href='#'>" + outcome[0].LOINCDescription + "<span class='removeIconListHover' onclick='Clinical_CarePlan.OutcomeDelete(\"" + outcome[0].OutcomesId + "\", true, event);'><i class='fa fa-close'></i></span></a></li>";
                    }
                    $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanOutcomes').append(li);
                    $('#' + Clinical_CarePlan.params.PanelID + ' #sectionOutcomes').removeClass('disableAll');

                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val(outcome[0].CarePlanId);
                    $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfOutcomesId").val(outcome[0].OutcomesId);

                    var self = $('#' + Clinical_CarePlan.params.PanelID);
                    utility.bindMyJSONByName(true, outcome[0], false, self).done(function () {
                        $('#' + Clinical_CarePlan.params.PanelID + " ddlOutcomeGoals option").removeAttr("selected");
                        $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeGoals').multiselect('clearSelection');
                        $('#' + Clinical_CarePlan.params.PanelID + " ddlOutcomeInterventions option").removeAttr("selected");
                        $('#' + Clinical_CarePlan.params.PanelID + ' #ddlOutcomeInterventions').multiselect('clearSelection');

                        if (outcome[0].GoalIds != null) {
                            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #ddlOutcomeGoals").val(outcome[0].GoalIds.split(','));
                            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #ddlOutcomeGoals").multiselect("refresh");
                        }

                        if (outcome[0].InterventionIds != null) {
                            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #ddlOutcomeInterventions").val(outcome[0].InterventionIds.split(','));
                            $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #ddlOutcomeInterventions").multiselect("refresh");
                        }
                    });
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    LoadCarePlanOutcome_DbCall: function (carePlanId, outcomeId) {

        var objData = {};
        objData["CarePlanId"] = carePlanId;
        objData["OutcomesId"] = outcomeId;

        objData["commandType"] = "fill_careplanoutcomes";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    OutcomeDelete: function (outcomeId, isFromList, ev) {
        ev.stopPropagation();
        utility.myConfirm('57', function () {
            if (outcomeId <= 0) {
                $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanOutcomes').find('li#' + outcomeId).remove();
                Clinical_CarePlan.ResetControls("Outcome");
            }
            else {
                Clinical_CarePlan.OutcomeDelete_DbCall(outcomeId).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        if (isFromList) {
                            $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanOutcomes').find('li#' + outcomeId).remove();
                            Clinical_CarePlan.ResetControls("Outcome");
                        }
                        Clinical_CarePlan.LoadCarePlan("Outcome");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }

        }, function () { },
                    '23'
        );
    },
    OutcomeDelete_DbCall: function (outcomeId) {

        var objData = {};
        objData["OutcomesId"] = outcomeId;
        objData["CarePlanId"] = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val();
        objData["commandType"] = "delete_careplanoutcomes";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    CareTeamSelect: function (providerId) {
        var params = [];
        params["ParentCtrl"] = 'Clinical_CarePlan';
        params["FromAdmin"] = "0";
        params["PatientId"] = $('#PatientProfile #hfPatientId').val();
        params["ProviderId"] = providerId;
        LoadActionPan('CCMCareTeam', params);
    },
    CareTeamMemberSelect: function (CareTeamId, Name, ProviderName, CareManagerName, CareCoordinatorName, CareGiverName, Specialty, ProviderPhone, CareManagerPhone, CareCoordinatorPhone, CareGiverPhone, isFromLoad) {

        Clinical_CarePlan.params.CareTeamId = CareTeamId;
        $('#' + Clinical_CarePlan.params.PanelID + " #dgvCareTeam tr").remove();

        var careTeamtbl = $('#' + Clinical_CarePlan.params.PanelID + " #dgvCareTeam tbody");
        var tableHead = $('#' + Clinical_CarePlan.params.PanelID + " #dgvCareTeam thead");
        tableHead.append('<tr><th>Name</th><th>Role</th><th>Speciality</th><th>Contact Number</th></tr>');
        careTeamtbl.append('<tr id=' + CareTeamId + '><td>  ' + CareManagerName + '</td><td>Care Manager</td><td></td><td>' + CareManagerPhone + '</td></tr>');
        careTeamtbl.append('<tr id=' + CareTeamId + '><td>  ' + CareCoordinatorName + '</td><td>Care Coordinator</td><td></td><td>' + CareCoordinatorPhone + '</td></tr>');
        careTeamtbl.append('<tr id=' + CareTeamId + '><td>  ' + CareGiverName + '</td><td>Care Giver</td><td></td><td>' + CareGiverPhone + '</td></tr>');
        careTeamtbl.append('<tr id=' + CareTeamId + '><td>  ' + ProviderName + '</td><td>Provider</td><td>' + Specialty + '</td><td>' + ProviderPhone + '</td></tr>');
        if (!isFromLoad) {
            CCMCareTeam.UnLoad();
        }
    },

    showHistory: function (carePlanId, columnKeyId, tableName, ev) {
        ev.stopPropagation();
        var ParentCtrl = 'clinicalTabCarePlan';
        var ParentCtrlPanelID = null;

        if (Clinical_CarePlan.params.TabID == "clinicalTabProgressNote") {

            ParentCtrl = "Clinical_CarePlan";
            ParentCtrlPanelID = "pnlClinicalProgressNote";
        }

        EMRUtility.showCurrentItemHistory(Clinical_CarePlan.params.PanelID, null, columnKeyId, tableName, null, ParentCtrl, carePlanId, ParentCtrlPanelID);
    },

    unLoadTab: function () {
        if (Clinical_CarePlan.params != null && Clinical_CarePlan.params.ParentCtrl != null) {
            if (Clinical_CarePlan.params.ParentCtrl == 'clinicalTabCarePlan')
                UnloadActionPan(Clinical_CarePlan.params["ParentCtrl"], "Clinical_CarePlan");
            else {
                UnloadActionPan(Clinical_CarePlan.params.ParentCtrl, 'Clinical_CarePlan', null, Clinical_CarePlan.params.PanelID);
            }
        }
        else {
            if (Clinical_CarePlan.params.FromAdmin == "0") {
                UnloadActionPan(null, 'Clinical_CarePlan');
            }
            else {
                RemoveAdminTab();
            }
        }

        Clinical_CarePlan.CurrentTab = '';
        Clinical_CarePlan.ItemsForNoteList = [];
    },

    CacheSelectedItem: function (obj, event, type) {
        if (event != null) {
            event.stopPropagation();
        }
        //totalRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder tr").length;
        //totalRows -= 1;
        //selectedRows = $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder tbody tr input:checked").length;
        //if (totalRows == selectedRows) {
        //    $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder #selectRecordOrders").prop("checked", true);
        //}
        //else {
        //    $("#" + Clinical_LabOrder.params.PanelID + " #dgvLabOrder #selectRecordOrders").prop("checked", false);
        //}

        var indexItem = -1;

        $.grep(Clinical_CarePlan.ItemsForNoteList, function (item, index) {
            if (item.CarePlanType == Clinical_CarePlan.CurrentTab && item.CarePlanValue == obj.id) {
                indexItem = index;
                return;
            }
        });

        if ($(obj).is(':checked')) {
            if (indexItem == -1) {
                var selectedItem = {
                    CarePlanValue: obj.id,
                    CarePlanType: Clinical_CarePlan.CurrentTab
                };

                Clinical_CarePlan.ItemsForNoteList.push(selectedItem);
            }
        }
        else {
            if (indexItem > -1) {
                Clinical_CarePlan.ItemsForNoteList.splice(indexItem, 1);
            }
        }
    },

    addCarePlanToNote: function (fromAddToNote) {
        var objDeffered = $.Deferred();
        if (Clinical_CarePlan.ItemsForNoteList.length > 0) {

            Clinical_CarePlan.addCarePlanToNoteDB_Call().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (fromAddToNote == true) {
                        Clinical_CarePlan.createCarePlanBodyHTML('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', response);
                    }
                    Clinical_CarePlan.CurrentTab = '';
                    Clinical_CarePlan.ItemsForNoteList = [];
                }
                else {
                    //utility.DisplayMessages(response.Message, 3);
                }
                objDeffered.resolve();
                Clinical_CarePlan.unLoadTab();
            });
        }
        else {
            objDeffered.resolve();
            Clinical_CarePlan.unLoadTab();
        }
        return objDeffered;
    },

    addCarePlanToNoteDB_Call: function () {
        var obj = {};
        obj["ItemsForNoteList"] = Clinical_CarePlan.ItemsForNoteList;
        obj["NotesId"] = Clinical_ProgressNote.params.NotesId;
        obj["PatientId"] = $('#PatientProfile #hfPatientId').val();
        obj["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        obj["commandType"] = "addcareplantonote";

        var data = JSON.stringify(obj);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    createCarePlanBodyHTML: function (NoteHTMLCtrl, response) {

        var carePlanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val();

        Clinical_CarePlan.checkCarePlanExists();

        if (Clinical_CarePlan.ItemsForNoteList.length > 0) {

            var goalsList = $.grep(Clinical_CarePlan.ItemsForNoteList, function (item, index) {
                return item.CarePlanType == 'Goals';
            });

            var concernList = $.grep(Clinical_CarePlan.ItemsForNoteList, function (item, index) {
                return item.CarePlanType == 'Concern';
            });

            var interventionList = $.grep(Clinical_CarePlan.ItemsForNoteList, function (item, index) {
                return item.CarePlanType == 'Intervention';
            });

            var outcomesList = $.grep(Clinical_CarePlan.ItemsForNoteList, function (item, index) {
                return item.CarePlanType == 'Outcome';
            });

            var $SectionBodyCarePlan = $(document.createElement('section'));
            $SectionBodyCarePlan.attr('id', "Cli_CarePlan_Main" + carePlanId);

            $.each(goalsList, function (i, item) {
                var goalId = item.CarePlanValue; i
                var $mainDiv = $(document.createElement('section'));
                $mainDiv.attr('id', "Cli_CarePlan_Main_Goals_" + goalId);

                var goalData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvCarePlan tr#dgvCarePlan" + goalId).data('rowsdata');
                var goalObj = JSON.parse(goalData);
                if (goalObj) {

                    $mainDiv.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_CarePlan_Goals_" + goalObj.GoalId + '"  ><i class="fa fa-times"></i></a></div> ');

                    var $List = $(document.createElement('ul'));
                    $List.attr('class', 'list-unstyled');
                    var soap = '';
                    if (goalObj.ICD10Code != null) {
                        soap = '<li><span>Goals: <b>' + goalObj.ICD10Code + '-' + goalObj.ICD10CodeDescription + '</b>';
                    }
                    else {
                        if (goalObj.ShowCPTCode == 1) {
                            var CPTCode = goalObj.CPTCode == null ? "" : goalObj.CPTCode;
                            var CPTDescription = goalObj.CPTDescription == null ? "" : goalObj.CPTDescription
                            soap = '<li><span>Goals: <b>' + CPTCode + (CPTCode != "" ? '-' : "") + CPTDescription + '</b>';
                        }
                        else {
                            var CPTDescription = goalObj.CPTDescription == null ? "" : goalObj.CPTDescription
                            soap = '<li><span>Goals: <b>' + CPTDescription + '</b>';
                        }

                    }

                    soap += goalObj.GoalComments != "" ? ' Comments: ' + goalObj.GoalComments : "";
                    soap += goalObj.GoalDate != null ? ' ' + goalObj.GoalDate : "";
                    soap += goalObj.GoalStatusValue != "" ? ' ' + goalObj.GoalStatusValue : "";
                    soap += goalObj.PatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.PatientPriority) : "";
                    soap += goalObj.ProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.ProviderPriority) : "";
                    soap += '</span></li>';

                    $($List).append(soap);
                    $mainDiv.append($List);
                }
                $SectionBodyCarePlan.append($mainDiv);
            });

            $.each(concernList, function (i, item) {
                var concernId = item.CarePlanValue;
                var $mainDiv = $(document.createElement('section'));
                $mainDiv.attr('id', "Cli_CarePlan_Main_Concern_" + concernId);

                var concernData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvHealthConcern tr#dgvHealthConcern" + concernId).data('rowsdata');
                var concernObj = JSON.parse(concernData);
                if (concernObj) {
                    $mainDiv.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_CarePlan_Concern_" + concernObj.HealthConcernsId + '"  ><i class="fa fa-times"></i></a></div> ');

                    var $List = $(document.createElement('ul'));
                    $List.attr('class', 'list-unstyled');
                    var soap = '<li><span>Health Concerns: ';
                    if (concernObj.Concerns_ICD10Code) {
                        soap += '<br>Concern: <b>' + concernObj.Concerns_ICD10Code + '-' + concernObj.Concerns_ICD10CodeDescription + '</b>';
                        soap += concernObj.ConcernsComments != "" && concernObj.ConcernsComments != null ? ' Comments:' + concernObj.ConcernsComments : "";
                        soap += concernObj.ConcernsDate != "" && concernObj.ConcernsDate != null ? ' ' + concernObj.ConcernsDate : "";
                        soap += concernObj.ConcernsStatusValue != "" ? ' ' + concernObj.ConcernsStatusValue : "";
                    }
                    if (concernObj.Observation_ICD10Code) {
                        soap += '<br>Observation: <b>' + concernObj.Observation_ICD10Code + '-' + concernObj.Observation_ICD10CodeDescription + '</b>';
                        soap += concernObj.ObservationComments != "" && concernObj.ObservationComments != null ? ' Comments:' + concernObj.ObservationComments : "";
                        soap += concernObj.ObservationDate != "" && concernObj.ObservationDate != null ? ' ' + concernObj.ObservationDate : "";
                        soap += concernObj.ObservationPatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(concernObj.ObservationPatientPriority) : "";
                        soap += concernObj.ObservationProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(concernObj.ObservationProviderPriority) : "";
                    }
                    if (concernObj.Risk_ICD10Code) {
                        soap += '<br>Risk: <b>' + concernObj.Risk_ICD10Code + '-' + concernObj.Risk_ICD10CodeDescription + '</b>';
                        soap += concernObj.RiskComments != "" && concernObj.RiskComments != null ? ' Comments:' + concernObj.RiskComments : "";
                        soap += concernObj.RiskDate != "" && concernObj.RiskDate != null ? ' ' + concernObj.RiskDate : "";
                        soap += concernObj.RiskStatusValue != "" ? ' ' + concernObj.RiskStatusValue : "";
                        soap += concernObj.RiskPatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(concernObj.RiskPatientPriority) : "";
                        soap += concernObj.RiskProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(concernObj.RiskProviderPriority) : "";
                    }
                    soap += '</span></li>';

                    $($List).append(soap);

                    $mainDiv.append($List);
                }
                $SectionBodyCarePlan.append($mainDiv);

            });

            $.each(interventionList, function (i, item) {
                var inteventionId = item.CarePlanValue;
                var $mainDiv = $(document.createElement('section'));
                $mainDiv.attr('id', "Cli_CarePlan_Main_Intervention_" + inteventionId);

                var interventionData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvInterventions tr#dgvInterventions" + inteventionId).data('rowsdata');
                var interventionObj = JSON.parse(interventionData);
                if (interventionObj) {
                    $mainDiv.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_CarePlan_Intervention_" + interventionObj.InterventionId + '"  ><i class="fa fa-times"></i></a></div> ');

                    var $List = $(document.createElement('ul'));
                    $List.attr('class', 'list-unstyled');
                    var soap = '<li><span>Interventions: ';
                    soap += '<b>' + interventionObj.ICD10Code + '-' + interventionObj.ICD10CodeDescription + '</b>';
                    soap += interventionObj.InterventionComments != "" ? ' Comments: ' + interventionObj.InterventionComments : "";
                    soap += interventionObj.InterventionDate != "" ? ' ' + interventionObj.InterventionDate : "";
                    soap += interventionObj.InterventionStatusValue != "" ? ' ' + interventionObj.InterventionStatusValue : "";
                    soap += '</span></li>';

                    $($List).append(soap);
                    $mainDiv.append($List);

                    if (interventionObj.GoalIds) {
                        var goals = interventionObj.GoalIds.split(',');

                        $.each(goals, function (i, item) {

                            var goalData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvCarePlan tr#dgvCarePlan" + item).data('rowsdata');
                            var goalObj = JSON.parse(goalData);
                            if (goalObj) {
                                var $ListGoal = $(document.createElement('ul'));
                                $ListGoal.attr('class', 'list-unstyled');
                                var soap = '';
                                if (goalObj.ICD10Code != null) {
                                    soap = '<li><span style="color:green !important;margin-right: -5px;">Goals:</span><span><b> ' + goalObj.ICD10Code + '-' + goalObj.ICD10CodeDescription + '</b>';
                                }
                                else {
                                    if (goalObj.ShowCPTCode == 1) {
                                        soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTCode + '-' + goalObj.CPTDescription + '</b>';
                                    }
                                    else {
                                        soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTDescription + '</b>';
                                    }
                                }

                                soap += goalObj.GoalComments != "" ? ' Comments: ' + goalObj.GoalComments : "";
                                soap += goalObj.GoalDate != null ? ' ' + goalObj.GoalDate : "";
                                soap += goalObj.GoalStatusValue != "" ? ' ' + goalObj.GoalStatusValue : "";
                                soap += goalObj.PatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.PatientPriority) : "";
                                soap += goalObj.ProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.ProviderPriority) : "";
                                soap += '</span></li>';

                                $($ListGoal).append(soap);
                                $mainDiv.append($ListGoal);
                            }
                        });
                    }

                    if (interventionObj.MedicationIds) {
                        Clinical_CarePlan.CreateMedicationSoapText(interventionObj, response, $mainDiv);
                    }
                }
                $SectionBodyCarePlan.append($mainDiv);

            });

            $.each(outcomesList, function (i, item) {
                var outcomesId = item.CarePlanValue;
                var $mainDiv = $(document.createElement('section'));
                $mainDiv.attr('id', "Cli_CarePlan_Main_Outcome_" + outcomesId);

                var outcomesData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvOutcomes tr#dgvOutcomes" + outcomesId).data('rowsdata');
                var outcomeObj = JSON.parse(outcomesData);
                if (outcomeObj) {
                    $mainDiv.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_CarePlan_Outcome_" + outcomeObj.OutcomesId + '"  ><i class="fa fa-times"></i></a></div> ');

                    var $List = $(document.createElement('ul'));
                    $List.attr('class', 'list-unstyled');

                    var soap = '<li><span>Health Status Evaluations/Outcomes: ';
                    soap += '<b>' + outcomeObj.ICD10Code + '-' + outcomeObj.ICD10CodeDescription + '</b>';
                    soap += outcomeObj.OutcomeComments != "" ? ' Comments: ' + outcomeObj.OutcomeComments : "";
                    soap += outcomeObj.OutcomeValue ? ' ' + outcomeObj.OutcomeValue : "";
                    soap += outcomeObj.OutcomeDate != "" ? ' ' + outcomeObj.OutcomeDate : "";
                    soap += '</span></li>';

                    $($List).append(soap);
                    $mainDiv.append($List);

                    if (outcomeObj.GoalIds) {
                        var goals = outcomeObj.GoalIds.split(',');

                        $.each(goals, function (i, item) {

                            var goalData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvCarePlan tr#dgvCarePlan" + item).data('rowsdata');
                            var goalObj = JSON.parse(goalData);
                            if (goalObj) {
                                var $ListGoal = $(document.createElement('ul'));
                                $ListGoal.attr('class', 'list-unstyled');
                                var soap = '';
                                if (goalObj.ICD10Code != null) {
                                    soap = '<li><span style="color:green !important;margin-right: -5px;">Goals:</span><span><b> ' + goalObj.ICD10Code + '-' + goalObj.ICD10CodeDescription + '</b>';
                                }
                                else {
                                    if (goalObj.ShowCPTCode == 1) {
                                        soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTCode + '-' + goalObj.CPTDescription + '</b>';
                                    }
                                    else {
                                        soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTDescription + '</b>';
                                    }

                                }

                                soap += goalObj.GoalComments != "" ? ' Comments: ' + goalObj.GoalComments : "";
                                soap += goalObj.GoalDate != null ? ' ' + goalObj.GoalDate : "";
                                soap += goalObj.GoalStatusValue != "" ? ' ' + goalObj.GoalStatusValue : "";
                                soap += goalObj.PatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.PatientPriority) : "";
                                soap += goalObj.ProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.ProviderPriority) : "";
                                soap += '</span></li>';

                                $($ListGoal).append(soap);
                                $mainDiv.append($ListGoal);
                            }
                        });
                    }

                    if (outcomeObj.InterventionIds) {
                        var interventions = outcomeObj.InterventionIds.split(',');

                        $.each(interventions, function (i, item) {

                            var interventionData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvInterventions tr#dgvInterventions" + item).data('rowsdata');
                            var interventionObj = JSON.parse(interventionData);
                            if (interventionObj) {

                                var $List = $(document.createElement('ul'));
                                $List.attr('class', 'list-unstyled');
                                var soap = '<li><span>Interventions: ';
                                soap += '<b>' + interventionObj.ICD10Code + '-' + interventionObj.ICD10CodeDescription + '</b>';
                                soap += interventionObj.InterventionComments != "" ? ' Comments: ' + interventionObj.InterventionComments : "";
                                soap += interventionObj.InterventionDate != "" ? ' ' + interventionObj.InterventionDate : "";
                                soap += interventionObj.InterventionStatusValue != "" ? ' ' + interventionObj.InterventionStatusValue : "";
                                soap += '</span></li>';

                                $($List).append(soap);
                                $mainDiv.append($List);

                                if (interventionObj.GoalIds) {
                                    var goals = interventionObj.GoalIds.split(',');

                                    $.each(goals, function (i, item) {

                                        var goalData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvCarePlan tr#dgvCarePlan" + item).data('rowsdata');
                                        var goalObj = JSON.parse(goalData);
                                        if (goalObj) {
                                            var $ListGoal = $(document.createElement('ul'));
                                            $ListGoal.attr('class', 'list-unstyled');
                                            var soap = '';
                                            if (goalObj.ICD10Code != null) {
                                                soap = '<li><span style="color:green !important;margin-right: -5px;">Goals:</span><span><b> ' + goalObj.ICD10Code + '-' + goalObj.ICD10CodeDescription + '</b>';
                                            }
                                            else {
                                                if (goalObj.ShowCPTCode == 1) {
                                                    soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTCode + '-' + goalObj.CPTDescription + '</b>';
                                                }
                                                else {
                                                    soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTDescription + '</b>';
                                                }
                                            }

                                            soap += goalObj.GoalComments != "" ? ' Comments: ' + goalObj.GoalComments : "";
                                            soap += goalObj.GoalDate != null ? ' ' + goalObj.GoalDate : "";
                                            soap += goalObj.GoalStatusValue != "" ? ' ' + goalObj.GoalStatusValue : "";
                                            soap += goalObj.PatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.PatientPriority) : "";
                                            soap += goalObj.ProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.ProviderPriority) : "";
                                            soap += '</span></li>';

                                            $($ListGoal).append(soap);
                                            $mainDiv.append($ListGoal);
                                        }
                                    });
                                }
                                if (interventionObj.MedicationIds) {
                                    Clinical_CarePlan.CreateMedicationSoapText(interventionObj, response, $mainDiv);
                                }
                            }
                        });
                    }
                }
                $SectionBodyCarePlan.append($mainDiv);

            });

            var $div = $(NoteHTMLCtrl + ' Clinical_CarePlan').parent().parent().find('#careplanCommentsDiv');
            if ($div.length > 0) {
                $div.remove();
            }
            var $commentsDiv = $(document.createElement('div'));
            $commentsDiv.attr('id', 'careplanCommentsDiv');
            var comments = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #txtCarePlanComments").val();
            $commentsDiv.append(comments);
            $SectionBodyCarePlan.append($commentsDiv);

            $(NoteHTMLCtrl + ' Clinical_CarePlan').parent().parent().addClass('initialVisitBody  ml-none');
            $(NoteHTMLCtrl + ' Clinical_CarePlan').parent().parent().append($SectionBodyCarePlan);

            Clinical_ProgressNote.saveComponentSOAPText('Care Plan');

        }
        Clinical_ProgressNote.hoverFunction();
    },

    checkCarePlanExists: function (cpId) {
        var carePlanId = '';
        if (cpId) {
            carePlanId = cpId;
        }
        else {
            carePlanId = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val();
        }

        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' Clinical_CarePlan').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="CarePlanComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<Clinical_CarePlan title="Care Plan"  id="' + carePlanId + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'CarePlan\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Care Plan">Care Plan</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'CarePlan\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'CarePlan\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</Clinical_CarePlan> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_careplan').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    GetPriority: function (val) {
        var text = '';
        if (val == 1) {
            text = "High Priority";
        }
        else if (val == 2) {
            text = "Medium Priority";
        }
        else if (val == 3) {
            text = "Low Priority";
        }
        else {
            text = '';
        }
        return text;
    },

    detachCarePlanFromNotes: function (detachedvalues) {
        var dfd = new $.Deferred();
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("Medical_Care Plan", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var selectedValue;
                if (detachedvalues.indexOf("Cli_CarePlan_") > -1) {
                    var selectedItem = detachedvalues.split('_');
                    if (selectedItem.length > 0) {
                        var carePlanType = selectedItem[2];
                        var carePlanValue = selectedItem[3];

                        utility.myConfirm('1', function () {
                            EMRUtility.scrollToPNcomponent('clinical_careplan');
                            Clinical_CarePlan.DetachItemsNoteList = [];
                            if (carePlanValue == "" || carePlanValue == "undefined") {
                                dfd.resolve('ok');
                            }
                            else {
                                var selectedItem = {
                                    CarePlanValue: carePlanValue,
                                    CarePlanType: carePlanType
                                };

                                Clinical_CarePlan.DetachItemsNoteList.push(selectedItem);

                                Clinical_CarePlan.detachCarePlanFromNotes_DBCall().done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {

                                        for (var i = 0; i < Clinical_CarePlan.DetachItemsNoteList.length; i++) {
                                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_CarePlan_Main_' + Clinical_CarePlan.DetachItemsNoteList[i].CarePlanType + '_' + Clinical_CarePlan.DetachItemsNoteList[i].CarePlanValue).remove();
                                        }
                                        Clinical_ProgressNote.saveComponentSOAPText('Care Plan');
                                        utility.DisplayMessages(response.Message, 1);
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                    dfd.resolve('ok');
                                });
                            }
                        }, function () { },
                                     '1'
                                 );
                    }

                }
                else {
                    dfd.resolve('ok');
                }
            }
            else {
                dfd.resolve('ok');
                utility.DisplayMessages(strMessage, 2);
            }
        });
        return dfd.promise();
    },


    detachCarePlanFromNotes_DBCall: function () {
        var objData = {};
        objData["NotesId"] = Clinical_ProgressNote.params.NotesId;
        objData["DetachItemsNoteList"] = Clinical_CarePlan.DetachItemsNoteList;
        objData["commandType"] = "detach_careplan_from_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");
    },

    detach_ComponentsCarePlan: function (ComponentName, IsUpdate, ComponentRemove) {

        var carePlanIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_careplan').parent().parent().find('section[id*="Cli_CarePlan_Main_"]').map(function () {
            return this.id.replace("Cli_CarePlan_Main_", "");
        }).get().join(',');

        Clinical_CarePlan.DetachItemsNoteList = [];

        if (ComponentRemove) {
            var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_careplan').parent().parent().attr('NoteComponentId');
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_careplan').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Care Plan', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_careplan').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_careplan').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Care Plan', true))
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
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_careplan').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }

        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_careplan').parent().parent().find('section[id*="Cli_CarePlan_Main_"]').remove();
        }

        if (carePlanIds == "" || carePlanIds == "undefined") {
            Clinical_ProgressNote.Detach_ComponentsOthers(ComponentName, true);
        }
        else {
            var carePlanIdsList = carePlanIds.split(',');
            $(carePlanIdsList).each(function (index, item) {
                var carePlanItem = item.split('_');
                if (carePlanItem.length > 0) {
                    var selectedItem = {
                        CarePlanValue: carePlanItem[1],
                        CarePlanType: carePlanItem[0]
                    };

                    Clinical_CarePlan.DetachItemsNoteList.push(selectedItem);
                }

            });

            Clinical_CarePlan.detachCarePlanFromNotes_DBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Care Plan', true);
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

    getcarePlanByPatientId: function (hideAlertMessage) {
        var strMessage = '';
        if (strMessage == "") {
            Clinical_CarePlan.getcarePlanByPatientId_DBCall().done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_CarePlan.createCarePlanBodyHTMLForNote(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');

                }
                else {
                    utility.DisplayMessages(strMessage, 3);
                }

            });
        }
        else {
            utility.DisplayMessages(strMessage, 3);
        }
    },

    getcarePlanByPatientId_DBCall: function () {
        var objData = new Object();
        objData["CarePlanId"] = $('#' + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfCarePlanId").val();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["commandType"] = "get_careplan_by_patientid";
        objData["ProviderId"] = $("#pnlClinicalProgressNote #hfProviderId").val();
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CarePlan", "CarePlan");

    },

    createCarePlanBodyHTMLForNote: function (response, NoteHTMLCtrl) {

        if (response != null) {
            Clinical_CarePlan.ItemsForNoteList = [];
            var carePlanId = response.CarePlanId;

            Clinical_CarePlan.checkCarePlanExists(carePlanId);
            var $SectionBodyCarePlan = $(document.createElement('section'));
            $SectionBodyCarePlan.attr('id', "Cli_CarePlan_Main" + carePlanId);

            if (response.GoalsCount > 0) {
                var GoalsList = JSON.parse(response.GoalsList);

                $.each(GoalsList, function (i, item) {
                    var goalId = item.GoalId;
                    var $mainDiv = $(document.createElement('section'));
                    $mainDiv.attr('id', "Cli_CarePlan_Main_Goals_" + goalId);

                    //var goalData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvCarePlan tr#dgvCarePlan" + goalId).data('rowsdata');
                    var goalObj = item;
                    if (goalObj) {

                        $mainDiv.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_CarePlan_Goals_" + item.GoalId + '"  ><i class="fa fa-times"></i></a></div> ');

                        var $List = $(document.createElement('ul'));
                        $List.attr('class', 'list-unstyled');
                        var soap = '';
                        if (goalObj.ICD10Code != null) {
                            soap = '<li><span>Goals: <b>' + goalObj.ICD10Code + '-' + goalObj.ICD10CodeDescription + '</b>';
                        }
                        else {
                            if (goalObj.ShowCPTCode == 1) {
                                soap = '<li><span>Goals: <b>' + goalObj.CPTCode + '-' + goalObj.CPTDescription + '</b>';
                            }
                            else {
                                soap = '<li><span>Goals: <b>' + goalObj.CPTDescription + '</b>';
                            }
                        }

                        soap += goalObj.GoalComments != "" ? ' Comments: ' + goalObj.GoalComments : "";
                        soap += goalObj.GoalDate != null ? ' ' + goalObj.GoalDate : "";
                        soap += goalObj.GoalStatusValue != "" ? ' ' + goalObj.GoalStatusValue : "";
                        soap += goalObj.PatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.PatientPriority) : "";
                        soap += goalObj.ProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.ProviderPriority) : "";
                        soap += '</span></li>';

                        $($List).append(soap);
                        $mainDiv.append($List);
                    }
                    $SectionBodyCarePlan.append($mainDiv);

                    var goaltoAttach = {
                        CarePlanType: "Goals",
                        CarePlanValue: goalObj.GoalId
                    };

                    Clinical_CarePlan.ItemsForNoteList.push(goaltoAttach);
                });
            }

            if (response.ConcernCount > 0) {
                var ConcernsList = JSON.parse(response.ConcernsList);

                $.each(ConcernsList, function (i, item) {
                    var concernId = item.HealthConcernsId;
                    var $mainDiv = $(document.createElement('section'));
                    $mainDiv.attr('id', "Cli_CarePlan_Main_Concern_" + concernId);

                    //var concernData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvHealthConcern tr#dgvHealthConcern" + concernId).data('rowsdata');
                    var concernObj = item;
                    if (concernObj) {
                        $mainDiv.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_CarePlan_Concern_" + concernObj.HealthConcernsId + '"  ><i class="fa fa-times"></i></a></div> ');

                        var $List = $(document.createElement('ul'));
                        $List.attr('class', 'list-unstyled');
                        var soap = '<li><span>Health Concerns: ';
                        if (concernObj.Concerns_ICD10Code) {
                            soap += '<br> Concern: <b>' + concernObj.Concerns_ICD10Code + '-' + concernObj.Concerns_ICD10CodeDescription + '</b>';
                            soap += concernObj.ConcernsComments != "" ? ' Comments:' + concernObj.ConcernsComments : "";
                            soap += concernObj.ConcernsDate != "" ? ' ' + concernObj.ConcernsDate : "";
                            soap += concernObj.ConcernsStatusValue != "" ? ' ' + concernObj.ConcernsStatusValue : "";
                        }
                        if (concernObj.Observation_ICD10Code) {
                            soap += '<br> Observation: <b>' + concernObj.Observation_ICD10Code + '-' + concernObj.Observation_ICD10CodeDescription + '</b>';
                            soap += concernObj.ObservationComments != "" ? ' Comments:' + concernObj.ObservationComments : "";
                            soap += concernObj.ObservationDate != "" ? ' ' + concernObj.ObservationDate : "";
                            soap += concernObj.ObservationPatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(concernObj.ObservationPatientPriority) : "";
                            soap += concernObj.ObservationProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(concernObj.ObservationProviderPriority) : "";
                        }
                        if (concernObj.Risk_ICD10Code) {
                            soap += '<br> Risk: <b>' + concernObj.Risk_ICD10Code + '-' + concernObj.Risk_ICD10CodeDescription + '</b>';
                            soap += concernObj.RiskComments != "" ? ' Comments:' + concernObj.RiskComments : "";
                            soap += concernObj.RiskDate != "" ? ' ' + concernObj.RiskDate : "";
                            soap += concernObj.RiskStatusValue != "" ? ' ' + concernObj.RiskStatusValue : "";
                            soap += concernObj.RiskPatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(concernObj.RiskPatientPriority) : "";
                            soap += concernObj.RiskProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(concernObj.RiskProviderPriority) : "";
                        }
                        soap += '</span></li>';

                        $($List).append(soap);

                        $mainDiv.append($List);
                    }
                    $SectionBodyCarePlan.append($mainDiv);

                    var concerntoAttach = {
                        CarePlanType: "Concern",
                        CarePlanValue: concernObj.HealthConcernsId
                    };

                    Clinical_CarePlan.ItemsForNoteList.push(concerntoAttach);

                });
            }
            if (response.InterventionsCount > 0) {
                var InterventionsList = JSON.parse(response.InterventionsList);
                $.each(InterventionsList, function (i, item) {
                    var inteventionId = item.InterventionId;
                    var $mainDiv = $(document.createElement('section'));
                    $mainDiv.attr('id', "Cli_CarePlan_Main_Intervention_" + inteventionId);

                    //var interventionData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvInterventions tr#dgvInterventions" + inteventionId).data('rowsdata');
                    var interventionObj = item;
                    if (interventionObj) {
                        $mainDiv.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_CarePlan_Intervention_" + interventionObj.InterventionId + '"  ><i class="fa fa-times"></i></a></div> ');

                        var $List = $(document.createElement('ul'));
                        $List.attr('class', 'list-unstyled');
                        var soap = '<li><span>Interventions: ';
                        soap += '<b>' + interventionObj.ICD10Code + '-' + interventionObj.ICD10CodeDescription + '</b>';
                        soap += interventionObj.InterventionComments != "" ? ' Comments: ' + interventionObj.InterventionComments : "";
                        soap += interventionObj.InterventionDate != "" ? ' ' + interventionObj.InterventionDate : "";
                        soap += interventionObj.InterventionStatusValue != "" ? ' ' + interventionObj.InterventionStatusValue : "";
                        soap += '</span></li>';

                        $($List).append(soap);
                        $mainDiv.append($List);

                        if (interventionObj.GoalIds) {
                            var goals = interventionObj.GoalIds.split(',');
                            var carePlanGoals = JSON.parse(response.GoalsList);

                            $.each(goals, function (i, item) {

                                var goalObj = $.grep(carePlanGoals, function (goal, index) {
                                    return goal.GoalId == item;
                                });

                                // var goalData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvCarePlan tr#dgvCarePlan" + item).data('rowsdata');
                                //var goalObj = JSON.parse(goalData);
                                if (goalObj.length > 0) {
                                    goalObj = goalObj[0];
                                    var $ListGoal = $(document.createElement('ul'));
                                    $ListGoal.attr('class', 'list-unstyled');
                                    var soap = '';
                                    if (goalObj.ICD10Code != null) {
                                        soap = '<li><span style="color:green !important;margin-right: -5px;">Goals:</span><span><b> ' + goalObj.ICD10Code + '-' + goalObj.ICD10CodeDescription + '</b>';
                                    }
                                    else {
                                        if (goalObj.ShowCPTCode == 1) {
                                            soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTCode + '-' + goalObj.CPTDescription + '</b>';
                                        }
                                        else {
                                            soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTDescription + '</b>';
                                        }
                                    }

                                    soap += goalObj.GoalComments != "" ? ' Comments: ' + goalObj.GoalComments : "";
                                    soap += goalObj.GoalDate != null ? ' ' + goalObj.GoalDate : "";
                                    soap += goalObj.GoalStatusValue != "" ? ' ' + goalObj.GoalStatusValue : "";
                                    soap += goalObj.PatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.PatientPriority) : "";
                                    soap += goalObj.ProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.ProviderPriority) : "";
                                    soap += '</span></li>';

                                    $($ListGoal).append(soap);
                                    $mainDiv.append($ListGoal);
                                }
                            });
                        }
                        if (interventionObj.MedicationIds) {
                            Clinical_CarePlan.CreateMedicationSoapText(interventionObj, response, $mainDiv);
                        }
                    }
                    $SectionBodyCarePlan.append($mainDiv);

                    var interventionToAttach = {
                        CarePlanType: "Intervention",
                        CarePlanValue: interventionObj.InterventionId
                    };

                    Clinical_CarePlan.ItemsForNoteList.push(interventionToAttach);
                });
            }

            if (response.OutcomesCount) {
                var OutcomesList = JSON.parse(response.OutcomesList);
                $.each(OutcomesList, function (i, item) {
                    var outcomesId = item.OutcomesId;
                    var $mainDiv = $(document.createElement('section'));
                    $mainDiv.attr('id', "Cli_CarePlan_Main_Outcome_" + outcomesId);

                    // var outcomesData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvOutcomes tr#dgvOutcomes" + outcomesId).data('rowsdata');
                    var outcomeObj = item;
                    if (outcomeObj) {
                        $mainDiv.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_CarePlan_Outcome_" + outcomeObj.OutcomesId + '"  ><i class="fa fa-times"></i></a></div> ');

                        var $List = $(document.createElement('ul'));
                        $List.attr('class', 'list-unstyled');

                        var soap = '<li><span>Health Status Evaluations/Outcomes: ';
                        soap += '<b>' + outcomeObj.ICD10Code + '-' + outcomeObj.ICD10CodeDescription + '</b>';
                        soap += outcomeObj.OutcomeComments != "" ? ' Comments: ' + outcomeObj.OutcomeComments : "";
                        soap += outcomeObj.OutcomeValue ? ' ' + outcomeObj.OutcomeValue : "";
                        soap += outcomeObj.OutcomeDate != "" ? ' ' + outcomeObj.OutcomeDate : "";
                        soap += '</span></li>';

                        $($List).append(soap);
                        $mainDiv.append($List);

                        if (outcomeObj.GoalIds) {
                            var goals = outcomeObj.GoalIds.split(',');
                            var carePlanGoals = JSON.parse(response.GoalsList);

                            $.each(goals, function (i, item) {

                                var goalObj = $.grep(carePlanGoals, function (goal, index) {
                                    return goal.GoalId == item;
                                });
                                //var goalData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvCarePlan tr#dgvCarePlan" + item).data('rowsdata');
                                //var goalObj = JSON.parse(goalData);
                                if (goalObj.length > 0) {
                                    goalObj = goalObj[0];
                                    var $ListGoal = $(document.createElement('ul'));
                                    $ListGoal.attr('class', 'list-unstyled');
                                    var soap = '';
                                    if (goalObj.ICD10Code != null) {
                                        soap = '<li><span style="color:green !important;margin-right: -5px;">Goals:</span><span><b> ' + goalObj.ICD10Code + '-' + goalObj.ICD10CodeDescription + '</b>';
                                    }
                                    else {
                                        if (goalObj.ShowCPTCode == 1) {
                                            soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTCode + '-' + goalObj.CPTDescription + '</b>';
                                        }
                                        else {
                                            soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTDescription + '</b>';
                                        }
                                    }

                                    soap += goalObj.GoalComments != "" ? ' Comments: ' + goalObj.GoalComments : "";
                                    soap += goalObj.GoalDate != null ? ' ' + goalObj.GoalDate : "";
                                    soap += goalObj.GoalStatusValue != "" ? ' ' + goalObj.GoalStatusValue : "";
                                    soap += goalObj.PatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.PatientPriority) : "";
                                    soap += goalObj.ProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.ProviderPriority) : "";
                                    soap += '</span></li>';

                                    $($ListGoal).append(soap);
                                    $mainDiv.append($ListGoal);
                                }
                            });
                        }

                        if (outcomeObj.InterventionIds) {
                            var interventions = outcomeObj.InterventionIds.split(',');

                            var carePlanInterventions = JSON.parse(response.InterventionsList);

                            $.each(interventions, function (i, item) {

                                var interventionObj = $.grep(carePlanInterventions, function (intervention, index) {
                                    return intervention.InterventionId == item;
                                });

                                //var interventionData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvInterventions tr#dgvInterventions" + item).data('rowsdata');
                                //var interventionObj = JSON.parse(interventionData);
                                if (interventionObj.length > 0) {
                                    interventionObj = interventionObj[0];
                                    var $List = $(document.createElement('ul'));
                                    $List.attr('class', 'list-unstyled');
                                    var soap = '<li><span>Interventions: ';
                                    soap += '<b>' + interventionObj.ICD10Code + '-' + interventionObj.ICD10CodeDescription + '</b>';
                                    soap += interventionObj.InterventionComments != "" ? ' Comments: ' + interventionObj.InterventionComments : "";
                                    soap += interventionObj.InterventionDate != "" ? ' ' + interventionObj.InterventionDate : "";
                                    soap += interventionObj.InterventionStatusValue != "" ? ' ' + interventionObj.InterventionStatusValue : "";
                                    soap += '</span></li>';

                                    $($List).append(soap);
                                    $mainDiv.append($List);

                                    if (interventionObj.GoalIds) {
                                        var goals = interventionObj.GoalIds.split(',');
                                        var carePlanGoals = JSON.parse(response.GoalsList);

                                        $.each(goals, function (i, item) {

                                            var goalObj = $.grep(carePlanGoals, function (goal, index) {
                                                return goal.GoalId == item;
                                            });

                                            //var goalData = $("#" + Clinical_CarePlan.params.PanelID + " #dgvCarePlan tr#dgvCarePlan" + item).data('rowsdata');
                                            //var goalObj = JSON.parse(goalData);
                                            if (goalObj.length > 0) {
                                                goalObj = goalObj[0];
                                                var $ListGoal = $(document.createElement('ul'));
                                                $ListGoal.attr('class', 'list-unstyled');
                                                var soap = '';
                                                if (goalObj.ICD10Code != null) {
                                                    soap = '<li><span style="color:green !important;margin-right: -5px;">Goals:</span><span><b> ' + goalObj.ICD10Code + '-' + goalObj.ICD10CodeDescription + '</b>';
                                                }
                                                else {
                                                    if (goalObj.ShowCPTCode == 1) {
                                                        soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTCode + '-' + goalObj.CPTDescription + '</b>';
                                                    }
                                                    else {
                                                        soap = '<li><span style="color:green !important;margin-right: -5px;">Goals: </span><span><b> ' + goalObj.CPTDescription + '</b>';
                                                    }
                                                }

                                                soap += goalObj.GoalComments != "" ? ' Comments: ' + goalObj.GoalComments : "";
                                                soap += goalObj.GoalDate != null ? ' ' + goalObj.GoalDate : "";
                                                soap += goalObj.GoalStatusValue != "" ? ' ' + goalObj.GoalStatusValue : "";
                                                soap += goalObj.PatientPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.PatientPriority) : "";
                                                soap += goalObj.ProviderPriority != null ? ' ' + Clinical_CarePlan.GetPriority(goalObj.ProviderPriority) : "";
                                                soap += '</span></li>';

                                                $($ListGoal).append(soap);
                                                $mainDiv.append($ListGoal);
                                            }
                                        });
                                    }
                                    if (interventionObj.MedicationIds) {
                                        Clinical_CarePlan.CreateMedicationSoapText(interventionObj, response, $mainDiv);
                                    }
                                }
                            });
                        }
                    }
                    $SectionBodyCarePlan.append($mainDiv);

                    var outcomeToAttach = {
                        CarePlanType: "Outcome",
                        CarePlanValue: outcomeObj.OutcomesId
                    };

                    Clinical_CarePlan.ItemsForNoteList.push(outcomeToAttach);
                });
            }

            var $commentsDiv = $(document.createElement('div'));
            var comments = response.Comments ? response.Comments : "";
            $commentsDiv.append(comments);
            $SectionBodyCarePlan.append($commentsDiv);

            $(NoteHTMLCtrl + ' Clinical_CarePlan').parent().parent().addClass('initialVisitBody  ml-none');
            $(NoteHTMLCtrl + ' Clinical_CarePlan').parent().parent().append($SectionBodyCarePlan);

            Clinical_CarePlan.addCarePlanToNote().done(function (response) {
                Clinical_ProgressNote.saveComponentSOAPText('Care Plan');
            });

        }
        Clinical_ProgressNote.hoverFunction();
    },
    GetConcernDate: function (concern) {

        var date = '';
        if (concern.Concerns_ICD9Code != null || concern.Concerns_ICD10Code != null) {
            date += concern.ConcernsDate != null ? utility.RemoveTimeFromDate(null, concern.ConcernsDate) : "";
        }
        if (concern.Observation_ICD9Code != null || concern.Observation_ICD10Code != null) {
            if (date != '') {
                date += ",";
            }
            date += concern.ObservationDate != null ? utility.RemoveTimeFromDate(null, concern.ObservationDate) : "";
        }
        if (concern.Risk_ICD9Code != null || concern.Risk_ICD10Code != null) {
            if (date != '') {
                date += ",";
            }
            date += concern.RiskDate != null ? utility.RemoveTimeFromDate(null, concern.RiskDate) : "";
        }

        return date;
    },
    checkUncheckSelectAll: function (obj) {
        var gridName = '';

        switch (Clinical_CarePlan.CurrentTab) {
            case "Goals":
                gridName = "dgvCarePlan";
                break;
            case "Concern":
                gridName = "dgvHealthConcern";
                break;
            case "Intervention":
                gridName = "dgvInterventions";
                break;
            case "Outcome":
                gridName = "dgvOutcomes";
                break;
            default:
                gridName = '';
                break;
        }

        if ($(obj).is(':checked')) {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #" + gridName + ' tbody').find('input[type="checkbox"]').prop('checked', true);
        }
        else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #" + gridName + ' tbody').find('input[type="checkbox"]').prop('checked', false);
        }
        $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #" + gridName + ' tbody').find('input[type="checkbox"]').each(function () {
            Clinical_CarePlan.CacheSelectedItem(this);
        });
    },

    CheckUncheckSelectAllOnLoad: function (gridName) {

        var noOfRows = $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #" + gridName + " tbody tr").length;
        var noOfCheckedRows = $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #" + gridName + " tbody").find('input[type="checkbox"]:checked').length;
        if (noOfRows == noOfCheckedRows) {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #" + gridName + " th").find('input[type="checkbox"]').prop('checked', true);
        }
        else {
            $("#" + Clinical_CarePlan.params.PanelID + " #pnlCarePlan_Result #" + gridName + " th").find('input[type="checkbox"]').prop('checked', false);
        }
    },
    SetTabActiveClass: function (type) {

        $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanTabsItems > li').removeClass('active');
        $('#' + Clinical_CarePlan.params.PanelID + ' .tabs-custom-body .tab-pane').removeClass('active');

        switch (type) {

            case "Goals":
                $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanTabsItems > li#listGoals').addClass('active');
                $('#' + Clinical_CarePlan.params.PanelID + ' .tabs-custom-body #Goals').addClass('active');
                break;
            case "Concern":
                $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanTabsItems > li#listConcerns').addClass('active');
                $('#' + Clinical_CarePlan.params.PanelID + ' .tabs-custom-body #Concerns').addClass('active');
                break;
            case "Intervention":
                $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanTabsItems > li#listInterventions').addClass('active');
                $('#' + Clinical_CarePlan.params.PanelID + ' .tabs-custom-body #Interventions').addClass('active');
                break;
            case "Outcome":
                $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanTabsItems > li#listOutcomes').addClass('active');
                $('#' + Clinical_CarePlan.params.PanelID + ' .tabs-custom-body #Outcomes').addClass('active');
                break;
            case "CareTeam":
                $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanTabsItems > li#listCareTeam').addClass('active');
                $('#' + Clinical_CarePlan.params.PanelID + ' .tabs-custom-body #CareTeam').addClass('active');
                break;
            default:
                $('#' + Clinical_CarePlan.params.PanelID + ' #ulCarePlanTabsItems > li#listGoals').addClass('active');
                $('#' + Clinical_CarePlan.params.PanelID + ' .tabs-custom-body #Goals').addClass('active');
        }
    },

    CreateMedicationSoapText: function (interventionObj, response, mainDiv) {
        var interventionMedications = interventionObj.MedicationIds.split(',');
        var medicationsList = JSON.parse(response.Medications_JSON);

        $.each(interventionMedications, function (i, item) {
            var medciationObj = $.grep(medicationsList, function (med, index) {
                return med.MedicationID == item;
            });

            if (medciationObj.length > 0) {
                medciationObj = medciationObj[0];
                var $ListMed = $(document.createElement('ul'));
                $ListMed.attr('class', 'list-unstyled');
                var soap = '<li><span style="color:green !important;margin-right: -5px;">Medications:</span><span><b> ' + medciationObj.MedicationName + '</b></span></li>';
                $($ListMed).append(soap);
                $(mainDiv).append($ListMed);
            }
        });
    },

    downloadXML: function() {
        var param = new Object();
        var Components = [];
        Components.push({
            componentId: -1,
            componentName: "DemographicDataElement",
        });

        Components.push({
            componentId: -2,
            componentName: "ProviderDataElement",
        });

        Components.push({
            componentId: -3,
            componentName: "HealthConcerns",
        });

        Components.push({
            componentId: -4,
            componentName: "PlanOfCare",
        });
        Components.push({
            componentId: -5,
            componentName: "Interventions"
        });
        Components.push({
            componentId: -6,
            componentName: "CarePlanOutcomes"
        });

        param["FromAdmin"] = "0";
        param["UserId"] = globalAppdata['AppUserId'];
        param["PatientId"] = $('#PatientProfile #hfPatientId').val();
        param["ParentCtrl"] = "Clinical_CarePlan";
        param["ProviderId"] = $("#PatientProfile #hfPatientProviderId").val()
        param["Template"] = "CarePlan";
        param["NoteId"] = 0;
        param["commandType"] = "xmlCarePlan";
        param["Components"] = Components;
        param["IsConfidential"] = false;
        data = JSON.stringify(param);
        MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
            var responseDetail = response = JSON.parse(response);
            if (response.status != false) {
                response.data = JSON.parse(response.data);
                $("#" + Clinical_CarePlan.params.PanelID + " #frmClinicalCarePlan #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                param["XMLData"] = response.data.xmlData;
                param["Encryption"] = false;
                param["IncludeHashCode"] = false;
                param["Password"] = "";
                param["commandType"] = "DOWNLOAD";
                param["SummaryType"] = "1"; // 1 for clinical Summary
                data = JSON.stringify(param);
                MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var zip = new JSZip();
                        var xml = zip.folder("XML");
                        xml.file("XMLData.xml", response.XMLByte, { base64: true });
                        var html = zip.folder("HTML");
                        html.file("htmlData.html", response.HTMLByte, { base64: true });
                        zip.generateAsync({ type: "blob" })
                        .then(function (content) {
                            saveAs(content, "CCDA.zip");
                        });
                        utility.DisplayMessages("Care Plan Downloaded Successfully.", 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    }
};
