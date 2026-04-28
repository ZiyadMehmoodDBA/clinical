/*
Author: Muhammad Arshad
Date: 31/03/2016
Overview: This file is created to show Plan Of Care
*/

Clinical_PlanOfCare = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_PlanOfCare.params = params;

        if (Clinical_PlanOfCare.params.PanelID != 'pnlPlanOfCare') {
            Clinical_PlanOfCare.params.PanelID = Clinical_PlanOfCare.params.PanelID + ' #pnlPlanOfCare';
        } else {
            Clinical_PlanOfCare.params.PanelID = 'pnlPlanOfCare';
        }
        if (Clinical_PlanOfCare.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_PlanOfCare.params.PanelID + " div#FaceSheetPager", Clinical_PlanOfCare.params.FaceSheetComponents, 'appointments');
        }
        var planOfCareId = "";
        if (Clinical_PlanOfCare.params.mode == "Add" || Clinical_PlanOfCare.params.PlanOfCareId == null || Clinical_PlanOfCare.params.PlanOfCareId == "" || Clinical_PlanOfCare.params.PlanOfCareId == "-1") {
            planOfCareId = "-1";
        }
        else if (Clinical_PlanOfCare.params.mode == "Edit") {
            planOfCareId = Clinical_PlanOfCare.params.PlanOfCareId;
            //Clinical_PlanOfCare.FamilyHxEdit(FamilyHxId);
        }

        var self = $('#' + Clinical_PlanOfCare.params.PanelID);

        self.loadDropDowns(true).done(function () {

            Clinical_PlanOfCare.loadPlanOfCare(planOfCareId);
        });

        //  Clinical_PlanOfCare.domReadyFunction();
        //Clinical_PlanOfCare.appointmentSearch();
    },

    // Method Name: activeInActive
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle active/inactive of Goal li
    activeInActive: function () {
        $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare #ulPlanOfCareGoal li').on('click', function () {
            $(this).parent().find('li.active').removeClass('active');
            $(this).addClass('active');
            $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare #FamilyMemberDetails').removeClass('disableAll');
        });

    },

    openCPTCode: function () {
        var params = [];
        //params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Clinical_PlanOfCare";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtGoal";
        params["ParentCtrlPanelID"] = Clinical_PlanOfCare.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, Clinical_PlanOfCare.params.PanelID);
    },


    // Method Name: loadPlanOfCare
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle load of PlanOfCare
    loadPlanOfCare: function (planOfCareId) {
        Clinical_PlanOfCare.fillPlanOfCare(null, planOfCareId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    var goalLoadDetails = JSON.parse(response.GoalLoad_JSON);
                    var a = goalLoadDetails[0];
                    if (typeof a != "undefined" && a.PlanOfCareId > 0) {
                        $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #aHistory").removeClass('hidden');
                        $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #aHistory").attr('onclick', 'Clinical_PlanOfCare.showHistory(' + a.PlanOfCareId + ')');
                    }

                    Clinical_PlanOfCare.loadPlanOfCareGoal('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #ulPlanOfCareGoal", goalLoadDetails);
                    if (Clinical_PlanOfCare.params.ParentCtrl == "clinicalTabProgressNote") {
                        $('#' + Clinical_PlanOfCare.params.PanelID + " #frmClinicalFamilyHx #dtFamilyHxDate").addClass("disableAll");
                    }
                    var planOfCareDetail = JSON.parse(response.PlanOfCareFill_JSON);
                    var self = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare");
                    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtClinicalInstructions").val(planOfCareDetail.ClinicalInstructions);
                    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtFutureScheduleAppointments").val(planOfCareDetail.FutureAppointments);
                    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtAid").val(planOfCareDetail.PatientDecisionAid);
                    utility.bindMyJSONByName(true, planOfCareDetail, false, self).done(function () {
                        Clinical_PlanOfCare.params.mode = "Edit";
                        $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').data('serialize', $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').serialize());
                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {

            }

        });

    },

    // Method Name: fillPlanOfCare
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle fill of PlanOfCare
    fillPlanOfCare: function (planOfCareType, planOfCareId, goalId) {
        var objData = new Object();
        objData["PlanOfCareId"] = planOfCareId != null ? planOfCareId : 0;
        objData["GoalId"] = goalId != null ? goalId : 0;
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["NoteId"] = Clinical_PlanOfCare.params.NotesId;
        objData["commandType"] = "FILL_PlanOfCare";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "PlanOfCare");
    },

    // Method Name: loadPlanOfCareGoal
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle load of PlanOfCare goal
    loadPlanOfCareGoal: function (crtl, result, statusType) {
        var currentLiClass = "";
        var currentLiClick = "";

        if ($(crtl).length > 0)
            l = $(crtl);
        l.empty();
        var isFirstLi = true;
        $.each(result, function (j, item) {

            var li = "<li  id=\"" + item.GoalId + "\" onclick='Clinical_PlanOfCare.fillPlanOfCareGoal(this, event);' onmouseover='Clinical_PlanOfCare.showIcon(this);' onmouseout='Clinical_PlanOfCare.hideIcon(this);' cptCode=\"" + item.ICD9Code + "\" cptDescription=\"" + item.ICD9CodeDescription + "\" icd10Code=\"" + item.ICD10Code + "\" icd10Desc=\"" + item.ICD10CodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\"><a href='#'>" + item.ICD9CodeDescription + "<span  class='removeIconListHover' onclick='Clinical_PlanOfCare.deletePlanOfCareGoal($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></li>"
            //var li = "<li  id=\"" + item.DiseaseId + "\" onclick='Clinical_PlanOfCare.fillMedicalHxDisease(this, event);' onmouseover='Clinical_PlanOfCare.showIcon(this);' onmouseout='Clinical_PlanOfCare.hideIcon(this);' icd9Code=\"" + item.ICD9Code + "\" icd9Desc=\"" + item.ICD9CodeDescription + "\" icd10Code=\"" + item.ICD10Code + "\" icd10Desc=\"" + item.ICD10CodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\"><a href='#'>" + item.ICD9CodeDescription + "<div id='deleteIcon' style='display:none' class='pull-right' onclick='Clinical_PlanOfCare.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close red'></i></div></a></li>"
            l.append(li);
        });
    },
    // Method Name: resetValues
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle reset control values of PlanOfCare
    resetValues: function () {

        $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #FamilyMemberDetails").find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
            $(this).val('');
        });

    },

    // Method Name: fillPlanOfCareGoal
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle fill of PlanOfCare goal
    fillPlanOfCareGoal: function (obj, ev) {

        ev.stopPropagation();
        $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #FamilyMemberDetails").removeClass("disableAll");
        var goalId = $(obj).attr('id');
        Clinical_PlanOfCare.resetValues();
        $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfGoalId").val(goalId);

        $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare ul#ulPlanOfCareGoal li").each(function (i, item) {
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
        if (goalId > 0) {
            Clinical_PlanOfCare.loadPlanOfCareComponent('goal', goalId, true);
        }
    },
    // Method Name: loadPlanOfCareComponent
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle fill of PlanOfCare goal data
    loadPlanOfCareComponent: function (planOfCareType, goalId, isGoalFill, isNewLoad) {
        BackgroundLoaderShow(true);
        Clinical_PlanOfCare.fillPlanOfCare(planOfCareType, null, goalId).done(function (response) {
            if (isNewLoad == true) {
                if ($('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfSelectedGoal").val() != "" && $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfSelectedGoal").val() != "undefined") {
                    var list = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #ulPlanOfCareGoal li#" + $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfSelectedGoal").val());
                    $(list).addClass('active');
                    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #FamilyMemberDetails").removeClass("disableAll");
                    list.trigger('click');
                    return;
                }
            }
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var goalLoadDetails = JSON.parse(response.GoalLoad_JSON);
                    var planOfCaredetail = JSON.parse(response.PlanOfCareFill_JSON);
                    var goaldetail = JSON.parse(response.GoalFill_JSON);
                    //if (planOfCaredetail.PlanOfCareId > 0) {
                    //    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #aHistory").removeClass('hidden');
                    //    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #aHistory").attr('onclick', 'Clinical_PlanOfCare.showHistory(' + planOfCaredetail.PlanOfCareId + ')');
                    //}
                    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtInstructions").val(goaldetail.Instructions);
                    Clinical_PlanOfCare.params.mode = "Edit";
                    $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').data('serialize', $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').serialize());

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {
                $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').data('serialize', $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').serialize());
            }
            BackgroundLoaderShow(false);
        });
    },

    // Method Name: showIcon
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle show delete icon on PlanofCare Goal li's focus
    showIcon: function (obj) {

        $(obj).find('div').css('display', '');

    },

    // Method Name: showIcon
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle show delete icon on PlanofCare Goal li's focus out
    hideIcon: function (obj) {

        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
        }

    },

    // Method Name: deletePlanOfCareGoal
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle delete of PlanOfCareGoal
    deletePlanOfCareGoal: function (obj, ev) {

        var dfd = new $.Deferred();
        ev.stopPropagation();
        var diseaseId = $(obj).attr('id');
        if (diseaseId < 0) {
            $(obj).remove();
            $('#' + Clinical_PlanOfCare.params.PanelID + ' #sectionDiseaseDetails').resetAllControls(null);
            $('#' +Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').data('serialize', $('#' +Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').serialize());
            dfd.resolve();
        } else {
            AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('23', function () {
                        var selectedValue = diseaseId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            Clinical_PlanOfCare.planOfCareGoalDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {

                                    $(obj).remove();
                                    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtInstructions").val('');
                                    $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #FamilyMemberDetails").addClass('disableAll');
                                    dfd.resolve();
                                    utility.DisplayMessages(response.Message, 1);

                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }, function () { },
                        '23'
                    );
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }

        dfd.done(function () {
            $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #sectionDiseaseDetails").addClass('disableAll');
        });
    },

    // Method Name: planOfCareGoalDelete
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will handle DBCall for delete of PlanOfCareGoal
    planOfCareGoalDelete: function (goalId) {
        var objData = new Object();
        objData["GoalId"] = goalId;
        var planOfCareId = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfPlanOfCareId").val();
        objData["PlanOfCareId"] = planOfCareId;
        objData["commandType"] = "DELETE_PLANOFCAREGOAL";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "PlanOfCare");
    },

    // Method Name: isDetailExists
    // Author: Ahmad Raza
    // Date: 05/04/2016
    // Description: This function will check whether detail exists in plan of care Detail section or not
    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";
        if (TabType != null && TabType != "") {
            if (TabType.toLowerCase() == "goal") {
                sectionDetails = "sectionPlanOfCareDetail";
            }
        }
        if (sectionDetails != "") {
            var self = $('#' + Clinical_PlanOfCare.params.PanelID + ' section#' + sectionDetails).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
                if ($(this).prop("disabled") != true) {
                    var currentElementTagName = this.tagName != null ? this.tagName : $(this).prop("tagName");
                    if (($(this).attr('type') == 'text' || currentElementTagName.toLowerCase() == 'textarea') && $(this).val() != "") {
                        DetailExists = true;
                    }

                }
            });
        }

        return DetailExists;
    },

    // Method Name: planOfCareSave
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle planOfCare Save
    planOfCareSave: function (PlanOfCareType, UnloadMedicalhx,UnloadOrNot) {
        var dfd = $.Deferred();
        var planOfCareId = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfPlanOfCareId").val() != "" ? $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfPlanOfCareId").val() : "-1";
        if (parseInt(planOfCareId) > 0) {
            Clinical_PlanOfCare.params.mode = "Edit";
        }
        else {
            Clinical_PlanOfCare.params.mode = "Add";
        }

        var DetailExists = false;
        var clinicalInstructions = "";
        var futureAppointments = "";
        var patientAids = "";
        clinicalInstructions = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtClinicalInstructions").val();
        futureAppointments = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtFutureScheduleAppointments").val();
        patientAids = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtAid").val();
        clinicalInstructions = typeof clinicalInstructions == "undefined" ? "" : clinicalInstructions
        if (Clinical_PlanOfCare.params.ParentCtrl == "clinicalTabProgressNote" && (clinicalInstructions != "" || futureAppointments != "" || patientAids != "")) {

            DetailExists = true;
        }
        else {
            DetailExists = Clinical_PlanOfCare.isDetailExists(PlanOfCareType.toLowerCase());
        }
        if ($('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #chkMedicalHxUnremarkable").is(':checked')) {
            DetailExists = true;
        }
        if (clinicalInstructions != "" || futureAppointments != "" || patientAids != "") {
            DetailExists = true;
        }
        // if (DetailExists == true) {
        if ($('#' + Clinical_PlanOfCare.params.PanelID + " ul#ulPlanOfCareGoal li").length > 0) {
            var strMessage = "";
            var self = null;
            if (PlanOfCareType.toLowerCase() == "goal") {
                self = $('#' + Clinical_PlanOfCare.params.PanelID + " section#sectionPlanOfCareDetail");
            }
            var myJSON = self != null ? self.getMyJSONByName() : "{}";
            var objData = JSON.parse(myJSON);
            if (PlanOfCareType.toLowerCase() == "goal") {

                var selectedGoal = $('#' + Clinical_PlanOfCare.params.PanelID + " ul#ulPlanOfCareGoal li.active");
                $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfselectedGoal").val(selectedGoal.attr("id"));
                objData["GoalId"] = selectedGoal.attr("id");
                objData["GoalText"] = selectedGoal.text();
                objData["ICD9Code"] = selectedGoal.attr("cptcode");
                objData["ICD9CodeDescription"] = selectedGoal.attr("cptdescription");
                //            objData["ICD10Code"] = selectedGoal.attr("icd10code");
                //           objData["ICD10CodeDescription"] = selectedGoal.attr("icd10desc");
                objData["SNOMEDID"] = selectedGoal.attr("snomedCode");
                objData["SNOMEDDescription"] = selectedGoal.attr("snomedDescription");
            }
            objData["PlanOfCareId"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfPlanOfCareId").val();
            objData["PlanOfCareType"] = PlanOfCareType != null ? PlanOfCareType : "";
            objData["ClinicalInstructions"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtClinicalInstructions").val();
            objData["FutureScheduleAppointments"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtFutureScheduleAppointments").val();
            objData["PatientDecisionAid"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtAid").val();
            myJSON = JSON.stringify(objData);
            if (Clinical_PlanOfCare.params.mode == "Add") {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_PlanOfCare.savePlanOfCare(myJSON).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.goalId != "") {
                                    var goalResponse = JSON.parse(response.goalId);

                                    if (goalResponse.goalId > 0) {
                                        $('#' + Clinical_PlanOfCare.params.PanelID + " ul#ulPlanOfCareGoal li.active").attr('id', goalResponse.goalId)
                                    }
                                }
                                $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfPlanOfCareId").val(response.PlanOfCareId);
                                Clinical_PlanOfCare.params.mode = "Edit";
                                if (Clinical_PlanOfCare.params.ParentCtrl == "clinicalTabProgressNote") {

                                    $.when(Clinical_PlanOfCare.getPlanOfCareInfo(PlanOfCareType, UnloadOrNot, null, true)).then(function () {
                                        dfd.resolve();
                                    });
                                }
                                else {
                                    dfd.resolve();
                                }
                                utility.DisplayMessages(response.message, 1);
                                $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').data('serialize', $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').serialize());
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                                dfd.resolve();
                            }
                        });
                    }
                    else {
                        utility.DisplayMessages(strMessage, 2);
                        dfd.resolve();
                    }

                });
            }
            else if (Clinical_PlanOfCare.params.mode == "Edit") {
                AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        Clinical_PlanOfCare.updatePlanOfCare(myJSON, Clinical_PlanOfCare.params.PlanOfCareId).done(function (response) {
                            response = JSON.parse(response);
                            if (response.status != false) {
                                if (response.goalId != "") {
                                    var goalResponse = JSON.parse(response.goalId);
                                    if (goalResponse > 0) {
                                        $('#' + Clinical_PlanOfCare.params.PanelID + " ul#ulPlanOfCareGoal li.active").attr('id', goalResponse)
                                    }
                                }
                                if (Clinical_PlanOfCare.params.ParentCtrl == "clinicalTabProgressNote") {
                                    $.when(Clinical_PlanOfCare.getPlanOfCareInfo(PlanOfCareType, UnloadOrNot)).then(function () {
                                        dfd.resolve();
                                    });
                                } else {
                                    utility.DisplayMessages(response.message, 1);
                                    dfd.resolve();
                                }
                                $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').data('serialize', $('#' + Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare').serialize());
                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                                dfd.resolve();
                            }
                        });
                    }
                    else {
                        utility.DisplayMessages(strMessage, 2);
                        dfd.resolve();
                    }

                });
            }
        }
        else {
            utility.DisplayMessages("Please enter any value", 3);
            dfd.resolve();
        }
        return dfd;
    },

    // Method Name: updatePlanOfCare
    // Author: Ahmad Raza
    // Date: 04/04/2016
    // Description: This function will handle DBCall for planOfCare update
    updatePlanOfCare: function (planOfCareData, planOfCareId) {

        var objData = JSON.parse(planOfCareData);
        if (Clinical_PlanOfCare.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_PlanOfCare.params.patientID;
        }
        objData["Instructions"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtInstructions").val();
        objData["FutureInstruction"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtFutureScheduleAppointments").val();
        objData["ClinicalInstruction"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtClinicalInstructions").val();
        objData["PatientDecisionAid"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtAid").val();
        objData["NoteId"] = Clinical_Notes.params.NotesId;
        objData["CPTCode"] = $('#' + Clinical_PlanOfCare.params.PanelID + ' #txtCPTCode').val();
        objData["CPTCodeId"] = $('#' + Clinical_PlanOfCare.params.PanelID + ' #hfCPTCode').val();
        objData["CPTDescription"] = $('#' + Clinical_PlanOfCare.params.PanelID + ' #hfCPTDescription').val();
        objData["commandType"] = "UPDATE_PLANOFCARE";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "PlanOfCare");
    },

    // Method Name: updatePlanOfCare
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle DBCall for planOfCare save
    savePlanOfCare: function (planOfCareData) {
        var objData = JSON.parse(planOfCareData);
        if (Clinical_PlanOfCare.params.patientID == null) {
            objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        } else {
            objData["PatientId"] = Clinical_PlanOfCare.params.patientID;
        }
        objData["Instructions"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtInstructions").val();
        objData["FutureInstruction"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtFutureScheduleAppointments").val();
        objData["ClinicalInstruction"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtClinicalInstructions").val();
        objData["PatientDecisionAid"] = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #txtAid").val();
        objData["NoteId"] = Clinical_Notes.params.NotesId;
        objData["CPTCode"] = $('#' + Clinical_PlanOfCare.params.PanelID + ' #txtCPTCode').val();
        objData["CPTCodeId"] = $('#' + Clinical_PlanOfCare.params.PanelID + ' #hfCPTCode').val();
        objData["CPTDescription"] = $('#' + Clinical_PlanOfCare.params.PanelID + ' #hfCPTDescription').val();
        objData["commandType"] = "SAVE_PlanOfCare";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "PlanOfCare");
    },

    // Method Name: bindICD9AutoComplete
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will handle autocomplete of plan of care goal
    bindAutoComplete: function (element) {

        var hiddenCrtl = $('#' + Clinical_PlanOfCare.params.PanelID + ' #txtGoal');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "Clinical_PlanOfCare", null, true);


        //var descriptionCrtl = $(element);
        //utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', descriptionCrtl, null, true, -1, "ICD", true, "Clinical_PlanOfCare", null, false);
    },

    // Method Name: OpenSearchPopup
    // Author: Ahmad Raza
    // Date: 01/04/2016
    // Description: This function will open ICD popup to select
    OpenSearchPopup: function (SearchType, Ctrl, HiddenCtrl) {
        var controlToLoad = "";
        if (SearchType == "ICD") {

            controlToLoad = "Admin_IMOICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (SearchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        var params = [];
        params["FromAdmin"] = "0";
        //params["ParentCtrl"] = Clinical_PlanOfCare.params["TabID"];
        if (Clinical_PlanOfCare.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'Clinical_PlanOfCare';
        }
        else {
            params["ParentCtrl"] = Clinical_PlanOfCare.params["TabID"];
        }

        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (Clinical_PlanOfCare.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }

    },

    // Method Name: appointmentSearch
    // Author: Muhammad Arshad
    // Date: 01/04/2016
    // Description: This function will handle appointmentSearch
    appointmentSearch: function (PageNo, rpp) {
        Clinical_PlanOfCare.searchAppointments(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_PlanOfCare.appointmentGridLoad(response);
                var TableControl = "Clinical_PlanOfCare #dgvClinical_PlanOfCare";
                var PagingPanelControlID = "Clinical_PlanOfCare #divFaceSheetAppointmentPaging";
                var ClassControlName = "Clinical_PlanOfCare";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AppointmentsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    Clinical_PlanOfCare.appointmentSearch(PageNumber, ResultPerPage);
                }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });
    },

    // Method Name: appointmentGridLoad
    // Author: Muhammad Arshad
    // Date: 01/04/2016
    // Description: This function will handle appointment grid load
    appointmentGridLoad: function (response) {
        $("#" + Clinical_PlanOfCare.params.PanelID + " #dgvClinical_PlanOfCare").dataTable().fnDestroy();
        $("#" + Clinical_PlanOfCare.params.PanelID + " #dgvClinical_PlanOfCare tbody").find("tr").remove();
        if (response.AppointmentsCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.AppointmentsLoad_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td>' + utility.RemoveTimeFromDate(null, item.ScheduleDate) + '</td><td>' + item.SchReason + '</td><td>' + item.ProviderName + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#" + Clinical_PlanOfCare.params.PanelID + " #dgvClinical_PlanOfCare tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_PlanOfCare.params.PanelID + " #divFaceSheetAppointmentPaging").css("display", "none");
            $("#" + Clinical_PlanOfCare.params.PanelID + " #dgvClinical_PlanOfCare").DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_PlanOfCare.params.PanelID + " #dgvClinical_PlanOfCare"))
            ;
        else
            $("#" + Clinical_PlanOfCare.params.PanelID + " #dgvClinical_PlanOfCare").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $('.table-responsive').css('min-height', '220px');
    },

    // Method Name: searchAppointments
    // Author: Muhammad Arshad
    // Date: 01/04/2016
    // Description: This function will handle DBCall for appointment search
    searchAppointments: function (PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();
        objData["commandType"] = "Search_FaceSheet_Appointments";
        return MDVisionService.APIService(objData, "FaceSheet", "FaceSheet");

    },


    //-----------------Progress Note-------------
    // Reason: These functions are used for Progress Note Soap Attachment, creation and detachment

    //Method Name: addPlanOfCareToNotes
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: Call Back function to add component to Progress Note
    addPlanOfCareToNotes: function () {
        var planOfCareId = Clinical_PlanOfCare.params.PlanOfCareId;
        var planOfCareType = $('#' + Clinical_PlanOfCare.params.PanelID + " #frmPlanOfCare #hfPlanOfCareType").val();
        Clinical_PlanOfCare.planOfCareSave(planOfCareType, true);
    },

    //Method Name: getPlanOfCareInfo
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: this function will get Plan Of Care Soap Text and attach that to Progress note
    getPlanOfCareInfo: function (planOfCareType, unloadPlanOfCare, planOfCareId, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_PlanOfCare.fillPlanOfCare(planOfCareType, planOfCareId, null).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    $.when(Clinical_PlanOfCare.createPlanOfCareBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', unloadPlanOfCare, hideAlertMessage)).then(function () {
                        dfd.resolve();
                    });
                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(strMessage, 3);
                }
            } else {
                dfd.resolve();
            }

        });
        return dfd;
    },
    //Method Name: checkPlanOfCareExists
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This Function will check, if Medical History Soap is already attached in Progress note, if Medical History is not attached than it will create main divs to attach allergy
    checkPlanOfCareExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Planofcare').length == 0) {

            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="PlanOfCareComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_Planofcare title="Plan Of Care"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'PlanOfCare\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="PlanOfCare">Plan Of Care</a> ' +
                        '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'PlanOfCare\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'PlanOfCare\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_Planofcare> </header></li>');
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
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Planofcare').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    //Method Name: createPlanOfCareBodyHTML
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This Function is used to create SOAP html and append it to  Progress note
    createPlanOfCareBodyHTML: function (response, NoteHTMLCtrl, unloadPlanOfCare, hideAlertMessage) {
        var dfd = $.Deferred();
        Clinical_PlanOfCare.checkPlanOfCareExists();
        if (response.PlanOfCareFill_JSON != null && response.PlanOfCareFill_JSON != '') {
            var PlanOfCareFill_Obj = JSON.parse(response.PlanOfCareFill_JSON);
            var $mainDivPlanOfCare = $(document.createElement('div'));

            var PlanOfCareId = PlanOfCareFill_Obj.PlanOfCareId;
            if (PlanOfCareId > 0) {
                var $SectionBodyPlanOfCare = $(document.createElement('section'));
                $SectionBodyPlanOfCare.attr('id', "Cli_PlanOfCare_Main" + PlanOfCareId);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_PlanOfCare_" + PlanOfCareId);
                var $ListPlanOfCare = $(document.createElement('ul'));

                $ListPlanOfCare.attr('class', 'list-unstyled')

                $SectionBodyPlanOfCare.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_PlanOfCare_" + PlanOfCareId + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_PlanOfCare_Main" + PlanOfCareId + '"  ><i class="fa fa-times"></i></a></div> ');


                $ListPlanOfCare.append("<li>" + PlanOfCareFill_Obj.PlanOfCareSoapText + "</li>");
                $DetailsDiv.append($ListPlanOfCare);
                $SectionBodyPlanOfCare.append($DetailsDiv);
                if ($(NoteHTMLCtrl + ' clinical_PlanOfCare').parent().parent().find('#Cli_PlanOfCare_Main' + PlanOfCareId).length == 0) {
                    $mainDivPlanOfCare.append($SectionBodyPlanOfCare);
                    $.when(Clinical_PlanOfCare.updatePlanOfCareHtml($mainDivPlanOfCare.html(), PlanOfCareId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                        dfd.resolve();
                    });
                } else {

                    var CommentHTML = "";
                    var CommentsID = $(NoteHTMLCtrl + ' clinical_PlanOfCare').parent().parent().find('#Cli_PlanOfCare_Main' + PlanOfCareId + ' ul li:Last').attr('id');
                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(NoteHTMLCtrl + ' clinical_PlanOfCare').parent().parent().find('#Cli_PlanOfCare_Main' + PlanOfCareId + ' ul li:Last').get(0).outerHTML;
                    }
                    $(NoteHTMLCtrl + ' clinical_PlanOfCare').parent().parent().find('#Cli_PlanOfCare_Main' + PlanOfCareId).html($SectionBodyPlanOfCare.html());
                    $(NoteHTMLCtrl + ' clinical_PlanOfCare').parent().parent().find('#Cli_PlanOfCare_Main' + PlanOfCareId + ' ul').append(CommentHTML);
                    $.when(Clinical_PlanOfCare.updatePlanOfCareHtml("", PlanOfCareId, NoteHTMLCtrl, hideAlertMessage)).then(function () {
                        dfd.resolve();
                    });

                }

                if (unloadPlanOfCare == true) {
                    Clinical_PlanOfCare.UnLoad(Clinical_PlanOfCare.bNextPrev);
                }
            } else {
                $.when(Clinical_ProgressNote.saveComponentSOAPText('Plan Of Care', hideAlertMessage)).then(function () {
                    dfd.resolve();
                });
            }
        } else {
            $.when(Clinical_ProgressNote.saveComponentSOAPText('Plan Of Care', hideAlertMessage)).then(function () {
                dfd.resolve();
            });
        }
        return dfd;
    },

    //Method Name: updatePlanOfCareHtml
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This Function is called by Progress Notes (Fill PlanOfCare Func, CopyAllNotesCategories)
    updatePlanOfCareHtml: function (PlanOfCareHtml, PlanOfCareId, NoteHTMLCtrl, hideAlertMessage) {
        var dfd = $.Deferred();
        $(NoteHTMLCtrl + ' clinical_Planofcare').parent().parent().addClass('initialVisitBody');
        if (PlanOfCareHtml != '') {
            $(NoteHTMLCtrl + ' clinical_Planofcare').parent().parent().append(PlanOfCareHtml);
        }
        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (PlanOfCareHtml != '') {
            $.when(Clinical_PlanOfCare.attachPlanOfCareFromNotes(PlanOfCareId, hideAlertMessage)).then(function () {
                dfd.resolve();
            });
        }
        return dfd;
    },

    //Method Name: detach_ComponentsPlanOfCare
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This Function detach Medical History From progress note
    detach_ComponentsPlanOfCare: function (ComponentName, IsUpdate, PlanOfCareComponentRemove) {

        var Clinical_PlanOfCareIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_PlanOfCare').parent().parent().find('section[id*="Cli_PlanOfCare_Main"]').map(function () {
            return this.id.replace("Cli_PlanOfCare_Main", "");
        }).get().join(',');

        if (PlanOfCareComponentRemove) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Plan Of Care']").remove();
            if (Clinical_ProgressNote.params["TemplateName"])
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Planofcare').parent().parent().addClass('hidden');
            else
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Planofcare').parent().parent().remove();
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_Planofcare').parent().parent().find('section[id*="Cli_PlanOfCare_Main"]').remove();
        }

        if (Clinical_PlanOfCareIds == "" || Clinical_PlanOfCareIds == "undefined") {
            Clinical_ProgressNote.Detach_ComponentsOthers(ComponentName, true);
        }
        else {
            Clinical_PlanOfCare.DetachPlanOfCareFromNotes_DBCall(Clinical_PlanOfCareIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (IsUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Plan Of Care', true);
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

    //Method Name: detachPlanOfCareFromNotes
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This Functions ask for Detaching Medical Hx from Progress Note for current Patient Selected
    detachPlanOfCareFromNotes: function (PlanOfCareId) {
        var strMessage = "";
        if (strMessage == "") {
            utility.myConfirm('1', function () {
                EMRUtility.scrollToPNcomponent('clinical_planofcare');
                var selectedValue = PlanOfCareId.replace('Cli_PlanOfCare_Main', '');
                if (selectedValue == "" || selectedValue == "undefined") {
                }
                else {
                    Clinical_PlanOfCare.DetachPlanOfCareFromNotes_DBCall(selectedValue).done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            $('#' + PlanOfCareId).remove();

                            Clinical_ProgressNote.saveComponentSOAPText('Plan Of Care');
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
        }
        else
            utility.DisplayMessages(strMessage, 2);
    },

    //Method Name: AttachPlanOfCareFromNotes
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //This Functions attached Medical Hx to Progress Note for current Patient Selected
    attachPlanOfCareFromNotes: function (PlanOfCareId, hideAlertMessage) {
        var dfd = $.Deferred();
        var strMessage = "";
        if (strMessage == "") {
            var selectedValue = PlanOfCareId;
            if (selectedValue == "" || selectedValue == "undefined") {
                Clinical_ProgressNote.saveComponentSOAPText('Plan Of Care', true);
                utility.DisplayMessages('Successfully Deleted', 1);
                dfd.resolve();
            }
            else {
                Clinical_PlanOfCare.AttachPlanOfCareFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        $.when(Clinical_ProgressNote.saveComponentSOAPText('Plan Of Care', hideAlertMessage)).then(function () {
                            $('#' + PlanOfCareId).remove();
                            dfd.resolve();
                        });

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        dfd.resolve();
                    }
                });
            }
        }
        else {
            utility.DisplayMessages(strMessage, 2);
            dfd.resolve();
        }
        return dfd;

    },

    //Method Name: getLatestPlanOfCareByPatientId
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description:If PlanOfCare Component which is droped in Progress note has no PlanOfCare attached, than it will call for Latest PlanOfCare for this patient
    createPlanOfCareBodyHTML: function () {
        Clinical_PlanOfCare.getLatestClinical_PlanOfCareByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_PlanOfCare.createPlanOfCareBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML');
            }
            else {
                utility.DisplayMessages(strMessage, 3);
            }

        });
    },

    //Method Name: DetachPlanOfCareFromNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This function will handle DBCall to detach Plan of Care from Notes
    DetachPlanOfCareFromNotes_DBCall: function (PlanOfCareId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PlanOfCareId"] = PlanOfCareId;
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
        objData["commandType"] = "detach_PlanOfCare_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ClinicalSummary", "PlanOfCare");
    },

    //Method Name: AttachPlanOfCareFromNotes_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This function will handle DBCall to attach Plan of Care from Notes
    AttachPlanOfCareFromNotes_DBCall: function (PlanOfCareId) {
        var objData = {};
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PlanOfCareId"] = PlanOfCareId;
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
        objData["commandType"] = "attach_PlanOfCare_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "ClinicalSummary", "PlanOfCare");
    },

    //Method Name: getLatestClinical_PlanOfCareByPatientId_DBCall
    //Author Name: Ahmad Raza
    //Created Date: 31-03-2016
    //Description: This function will handle DBCall to get latest Plan of Care by patient Id
    getLatestClinical_PlanOfCareByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["commandType"] = "getlatest_PlanOfCareby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "PlanOfCare");
    },


    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to Unload screen
    */
    UnLoad: function () {
        var objDeffered = $.Deferred();

        if (EMRUtility.compareFormDataWithSerialized(Clinical_PlanOfCare.params.PanelID + ' #frmPlanOfCare')) {
            utility.myConfirmNote('1', function () {
                Clinical_PlanOfCare.planOfCareSave('goal',null,false);
                    UnloadActionPan(Clinical_PlanOfCare.params.ParentCtrl, 'Clinical_PlanOfCare');
                    objDeffered.resolve();
                    return objDeffered;

            }, "", function () {
                UnloadActionPan(Clinical_PlanOfCare.params.ParentCtrl, 'Clinical_PlanOfCare');
                objDeffered.resolve();
                return objDeffered;
            }
           );
        } else {
            UnloadActionPan(Clinical_PlanOfCare.params.ParentCtrl, 'Clinical_PlanOfCare');
            objDeffered.resolve();
            return objDeffered;
        }


    },

    //Method Name: showHistory
    //Author Name: Ahmad Raza
    //Created Date: 20-04-2016
    //Description: This function will show PlanOfCare History
    showHistory: function (planOfCareId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var TabId = Clinical_PlanOfCare.params.TabID;
        if (Clinical_PlanOfCare.params.ParentCtrl == "clinicalTabProgressNote") {
            TabId = 'Clinical_PlanOfCare';
        }
        EMRUtility.showCurrentItemHistory(Clinical_PlanOfCare.params.PanelID, null, null, "PlanofCare,PlanOfCareGoal", null, TabId, planOfCareId);
    },

}