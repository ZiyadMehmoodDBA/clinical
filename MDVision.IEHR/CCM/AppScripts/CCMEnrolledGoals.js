
CCMEnrolledGoals = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        CCMEnrolledGoals.params = params;

        if (CCMEnrolledGoals.params.PanelID != 'pnlCCMEnrolledGoals')
            CCMEnrolledGoals.params.PanelID = CCMEnrolledGoals.params.PanelID + ' #pnlCCMEnrolledGoals';
        else 
            CCMEnrolledGoals.params.PanelID = 'pnlCCMEnrolledGoals';

        var self = $('#' + CCMEnrolledGoals.params.PanelID);
        //if (Admin_CCMICDGroups_Detail.bIsFirstLoad)
        //{
            self.loadDropDowns(true).done(function () {
                var objData = new Object();
                objData["EnrollmentInfoId"] = CCMEnrolledGoals.params.EnrollmentInfoId;

                CCMEnrolledGoals.PatientHubGoalsLoad(objData).done(function (response) {
                    if (response.status != false) {
                        if (response.PHCount > 0) {
                            var goalLoadDetails = JSON.parse(response.PHList_JSON);
                            CCMEnrolledGoals.BindPatientHubGoal('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #ulCCMEnrolledGoalsGoal", goalLoadDetails);
                        }
                    }
                });
            });
        //}

        // Instructions
        $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtInstructions").on('change', function (event) {
            var value = $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtInstructions").val();
            $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #ulCCMEnrolledGoalsGoal li.active").attr('instructions', value);
        });
    },

    PatientHubGoalsLoad: function (objData) {
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "LoadCCMPatientHUBGoals");
    },

    activeInActive: function () {
        $('#' + CCMEnrolledGoals.params.PanelID + ' #frmCCMEnrolledGoals #ulCCMEnrolledGoalsGoal li').on('click', function () {
            $(this).parent().find('li.active').removeClass('active');
            $(this).addClass('active');
            $('#' + CCMEnrolledGoals.params.PanelID + ' #frmCCMEnrolledGoals #FamilyMemberDetails').removeClass('disableAll');
        });

    },

    openCPTCode: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "CCMEnrolledGoals";
        params["RefHiddenCtrl"] = "hfCPTCode";
        params["RefCtrl"] = "txtGoal";
        params["ParentCtrlPanelID"] = CCMEnrolledGoals.params.PanelID;
        LoadActionPan('Admin_IMOCPT', params, CCMEnrolledGoals.params.PanelID);
    },

    loadPlanOfCare: function (planOfCareId) {
        CCMEnrolledGoals.fillPlanOfCare(null, planOfCareId).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    var goalLoadDetails = JSON.parse(response.GoalLoad_JSON);
                    var a = goalLoadDetails[0];
                    //MK
                    //if (typeof a != "undefined" && a.PlanOfCareId > 0) {
                    //$('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #aHistory").removeClass('hidden');
                    //$('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #aHistory").attr('onclick', 'CCMEnrolledGoals.showHistory(' + a.PlanOfCareId + ')');
                    //}

                    CCMEnrolledGoals.loadPlanOfCareGoal('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #ulCCMEnrolledGoalsGoal", goalLoadDetails);
                    if (CCMEnrolledGoals.params.ParentCtrl == "clinicalTabProgressNote") {
                        $('#' + CCMEnrolledGoals.params.PanelID + " #frmClinicalFamilyHx #dtFamilyHxDate").addClass("disableAll");
                    }
                    var planOfCareDetail = JSON.parse(response.PlanOfCareFill_JSON);
                    var self = $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals");
                    //MK
                    //$('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtClinicalInstructions").val(planOfCareDetail.ClinicalInstructions);
                    //$('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtFutureScheduleAppointments").val(planOfCareDetail.FutureAppointments);
                    //$('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtAid").val(planOfCareDetail.PatientDecisionAid);
                    utility.bindMyJSONByName(true, planOfCareDetail, false, self).done(function () {
                        CCMEnrolledGoals.params.mode = "Edit";
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

    //fillPlanOfCare: function (planOfCareType, planOfCareId, goalId) {
    //    var objData = new Object();
    //    objData["PlanOfCareId"] = planOfCareId != null ? planOfCareId : 0;
    //    objData["GoalId"] = goalId != null ? goalId : 0;
    //    objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
    //    objData["NoteId"] = CCMEnrolledGoals.params.NotesId;
    //    objData["commandType"] = "FILL_PlanOfCare";
    //    var data = JSON.stringify(objData);
    //    return MDVisionService.APIService(data, "ClinicalSummary", "PlanOfCare");
    //},

    BindPatientHubGoal: function (crtl, result)
    {
        var objData = new Object();
        objData["EnrollmentInfoId"] = CCMEnrolledGoals.params.EnrollmentInfoId;

        if ($(crtl).length > 0)
            l = $(crtl);

        l.empty();

        var isFirstLi = true;

        $.each(result, function (j, item) {

            //onclick = 'CCMEnrolledGoals.fillPlanOfCareGoal(this, event);'
            var li = "<li  id=\"" + item.EnrolledGoalsICDId + "\" onclick = 'CCMEnrolledGoals.fillInstructions(\"" + item.Instruction + "\", \"" + item.EnrolledGoalsICDId
                + "\");' instructions=\"" + item.Instruction + "\" onmouseover='CCMEnrolledGoals.showIcon(this, \"" + item.Instruction + "\", \"" + item.EnrolledGoalsICDId
                + "\");' onmouseout='CCMEnrolledGoals.hideIcon(this);' cptCode=\"" + item.CPTCode + "\" cptDescription=\"" + item.CPTDescription + "\" snomedCode=\""
                + item.SNOMEDCode + "\" snomedDesc=\"" + item.SNOMEDDescription + "\"><a href='#'>" + item.CPTDescription + "<span  class='removeIconListHover' onclick='CCMEnrolledGoals.deletePatientHubGoals(\""
                + item.EnrolledGoalsICDId + "\", \"" + item.EnrolledGoalsId + "\").done(function (response) { utility.DisplayMessages(response.Message, 1);  CCMEnrolledGoals.removeGoal(\""
                + item.EnrolledGoalsICDId + "\"); });'><i class='fa fa-times'></i></span></a></li>"
            l.append(li);
        });
    },

    removeGoal: function (EnrolledGoalsICDId)
    {
        var objData = new Object();
        objData["EnrollmentInfoId"] = CCMEnrolledGoals.params.EnrollmentInfoId;
        $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #ulCCMEnrolledGoalsGoal #" + EnrolledGoalsICDId).remove();
        CCM_Patient_Hub.PatientHubGoals(objData);
    },

    fillInstructions: function (Instruction, Id)
    {
        CCMEnrolledGoals.activeInActive();
        $("#ulCCMEnrolledGoalsGoal #" + Id).addClass('active');
        $("#FamilyMemberDetails").removeClass('disableAll');
        //$("#txtInstructions").text(Instruction);
        $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtInstructions").text(Instruction);
        $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtInstructions").val(Instruction);
    },

    showIcon: function (obj, instructions, Id) {
        CCMEnrolledGoals.activeInActive();
        //$("#ulCCMEnrolledGoalsGoal #" + Id).addClass('active');
        $(obj).find('div').css('display', '');
        //$(obj).addClass("active");
        // $("#FamilyMemberDetails").removeClass('disableAll');
        //$('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtInstructions").text(instructions);
        //$('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtInstructions").val(instructions);
    },

    hideIcon: function (obj) {
        if ($(obj).hasClass("active") == false) {
            $(obj).find('div').css('display', 'none');
            //$("#FamilyMemberDetails").addClass('disableAll');
            //$("#txtInstructions").text('');
        }
    },

    deletePatientHubGoals: function (EnrolledGoalsICDId, EnrolledGoalsId)
    {
        var objData = new Object();
        objData["EnrolledGoalsId"] = EnrolledGoalsId;
        objData["EnrolledGoalsICDId"] = EnrolledGoalsICDId;

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "DeletePatientHubEnrolledGoals");
    },

    loadPlanOfCareGoal: function (crtl, result, statusType) {
        var currentLiClass = "";
        var currentLiClick = "";

        if ($(crtl).length > 0)
            l = $(crtl);
        l.empty();
        var isFirstLi = true;
        $.each(result, function (j, item) {

            var li = "<li  id=\"" + item.GoalId + "\" onclick='CCMEnrolledGoals.fillPlanOfCareGoal(this, event);' onmouseover='CCMEnrolledGoals.showIcon(this);' onmouseout='CCMEnrolledGoals.hideIcon(this);' cptCode=\"" + item.ICD9Code + "\" cptDescription=\"" + item.ICD9CodeDescription + "\" icd10Code=\"" + item.ICD10Code + "\" icd10Desc=\"" + item.ICD10CodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\"><a href='#'>" + item.ICD9CodeDescription + "<span  class='removeIconListHover' onclick='CCMEnrolledGoals.deletePlanOfCareGoal($($(this).parent()).parent(), event);'><i class='fa fa-times'></i></span></a></li>"
            //var li = "<li  id=\"" + item.DiseaseId + "\" onclick='CCMEnrolledGoals.fillMedicalHxDisease(this, event);' onmouseover='CCMEnrolledGoals.showIcon(this);' onmouseout='CCMEnrolledGoals.hideIcon(this);' icd9Code=\"" + item.ICD9Code + "\" icd9Desc=\"" + item.ICD9CodeDescription + "\" icd10Code=\"" + item.ICD10Code + "\" icd10Desc=\"" + item.ICD10CodeDescription + "\" snomedCode=\"" + item.SNOMEDID + "\" snomedDesc=\"" + item.SNOMEDDescription + "\"><a href='#'>" + item.ICD9CodeDescription + "<div id='deleteIcon' style='display:none' class='pull-right' onclick='CCMEnrolledGoals.deleteMedicalHxDisease($($(this).parent()).parent(), event);'><i class='fa fa-close red'></i></div></a></li>"
            l.append(li);
        });
    },

    resetValues: function () {

        $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #FamilyMemberDetails").find('[type=text],[type=password],[type=checkbox],textarea,[type=radio],select').each(function () {
            $(this).val('');
        });

    },

    fillPlanOfCareGoal: function (obj, ev) {

        ev.stopPropagation();
        $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #FamilyMemberDetails").removeClass("disableAll");
        var goalId = $(obj).attr('id');
        CCMEnrolledGoals.resetValues();
        $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #hfGoalId").val(goalId);

        $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals ul#ulCCMEnrolledGoalsGoal li").each(function (i, item) {
            var hfTextInstruction = "hfTextInstruction";
            $(this).append('<input type="hidden" id=' + hfTextInstruction + $(this).attr('id') + ' name="myfieldname" value=' + $("#txtInstructions").val() + ' />');
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
            CCMEnrolledGoals.loadPlanOfCareComponent('goal', goalId, true);
        }
        var instructions = obj.getAttribute("instructions");
        $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtInstructions").val(instructions);

    },
   
    loadPlanOfCareComponent: function (planOfCareType, goalId, isGoalFill, isNewLoad) {
        BackgroundLoaderShow(true);
        CCMEnrolledGoals.fillPlanOfCare(planOfCareType, null, goalId).done(function (response) {
            if (isNewLoad == true) {
                if ($('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #hfSelectedGoal").val() != "" && $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #hfSelectedGoal").val() != "undefined") {
                    var list = $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #ulCCMEnrolledGoalsGoal li#" + $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #hfSelectedGoal").val());
                    $(list).addClass('active');
                    $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #FamilyMemberDetails").removeClass("disableAll");
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
                    //    $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #aHistory").removeClass('hidden');
                    //    $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #aHistory").attr('onclick', 'CCMEnrolledGoals.showHistory(' + planOfCaredetail.PlanOfCareId + ')');
                    //}
                    $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtInstructions").val(goaldetail.Instructions);
                    CCMEnrolledGoals.params.mode = "Edit";
                    $('#' + CCMEnrolledGoals.params.PanelID + ' #frmClinicalMedicalHx').data('serialize', $('#' + CCMEnrolledGoals.params.PanelID + ' #frmClinicalMedicalHx').serialize());

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
            else {

            }
            BackgroundLoaderShow(false);
        });
    },

    
    deletePlanOfCareGoal: function (obj, ev) {

        var dfd = new $.Deferred();
        ev.stopPropagation();
        var diseaseId = $(obj).attr('id');
        if (diseaseId < 0) {
            $(obj).remove();
            $('#' + CCMEnrolledGoals.params.PanelID + ' #sectionDiseaseDetails').resetAllControls(null);
            dfd.resolve();
        } else {
            AppPrivileges.GetFormPrivileges("Notes_Notes", "DELETE", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    utility.myConfirm('23', function () {
                        var selectedValue = diseaseId;
                        if (selectedValue == "" || selectedValue == "undefined") {
                        }
                        else {
                            CCMEnrolledGoals.planOfCareGoalDelete(selectedValue).done(function (response) {

                                response = JSON.parse(response);
                                if (response.status != false) {

                                    $(obj).remove();
                                    $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #txtInstructions").val('');
                                    $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #FamilyMemberDetails").addClass('disableAll');
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
            $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #sectionDiseaseDetails").addClass('disableAll');
        });
    },

    planOfCareGoalDelete: function (goalId) {
        var objData = new Object();
        objData["GoalId"] = goalId;
        var planOfCareId = $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #hfPlanOfCareId").val();
        objData["PlanOfCareId"] = planOfCareId;
        objData["commandType"] = "DELETE_PLANOFCAREGOAL";

        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalSummary", "PlanOfCare");
    },

    isDetailExists: function (TabType) {
        var DetailExists = false;
        var sectionDetails = "";
        if (TabType != null && TabType != "") {
            if (TabType.toLowerCase() == "goal") {
                sectionDetails = "sectionPlanOfCareDetail";
            }
        }
        if (sectionDetails != "") {
            var self = $('#' + CCMEnrolledGoals.params.PanelID + ' section#' + sectionDetails).find('[type=text],[type=password], textarea,[type=checkbox], [type=radio],select').each(function () {
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

    planOfCareSave: function (PlanOfCareType)
    {
        var strMessage = "";
        if (PlanOfCareType.toLowerCase() == "goal") {

            var Goalslength = $('#' + CCMEnrolledGoals.params.PanelID + " ul#ulCCMEnrolledGoalsGoal li").length;
            var objs = [];
            for (var i = 0; i < Goalslength; i++) {

                var selectedGoal = $('#' + CCMEnrolledGoals.params.PanelID + " ul#ulCCMEnrolledGoalsGoal li.active");
                $('#' + CCMEnrolledGoals.params.PanelID + " #frmCCMEnrolledGoals #hfselectedGoal").val(selectedGoal.attr("id"));

                var objData = new Object();
                objData["cptcode"] = $($('#' + CCMEnrolledGoals.params.PanelID + " ul#ulCCMEnrolledGoalsGoal li")[i]).attr('cptcode');
                objData["cptdescription"] = $($('#' + CCMEnrolledGoals.params.PanelID + " ul#ulCCMEnrolledGoalsGoal li")[i]).attr('cptdescription');
                objData["SNOMEDID"] = $($('#' + CCMEnrolledGoals.params.PanelID + " ul#ulCCMEnrolledGoalsGoal li")[i]).attr('snomedcode');
                objData["SNOMEDDescription"] = $($('#' + CCMEnrolledGoals.params.PanelID + " ul#ulCCMEnrolledGoalsGoal li")[i]).attr('snomeddescription');
                objData["Instruction"] = $($('#' + CCMEnrolledGoals.params.PanelID + " ul#ulCCMEnrolledGoalsGoal li")[i]).attr('instructions');
                objData["EnrollmentInfoId"] = CCMEnrolledGoals.params.EnrollmentInfoId;
                objData["PatientId"] = CCMEnrolledGoals.params.patientID;

                if (objData["SNOMEDDescription"] == undefined || objData["SNOMEDDescription"] == 'undefined' || objData["SNOMEDDescription"] == null) {
                    objData["SNOMEDDescription"] = "";
                }

                if (objData["SNOMEDID"] == undefined || objData["SNOMEDID"] == 'undefined' || objData["SNOMEDID"] == null) {
                    objData["SNOMEDID"] = "";
                }

                if (objData["Instruction"] == undefined || objData["Instruction"] == 'undefined' || objData["Instruction"] == null) {
                    objData["Instruction"] = "";
                }

                objs.push(objData);
            }
            if (objs.length > 0) {
                if (CCMEnrolledGoals.params.mode == "Add") {
                    if (strMessage == "") {
                        CCMEnrolledGoals.savePlanOfCare(objs).done(function (response) {
                            if (response.status != false) {

                                utility.DisplayMessages(response.Message, 1);
                                var objData = new Object();
                                objData["EnrollmentInfoId"] = CCMEnrolledGoals.params.EnrollmentInfoId;
                                CCM_Patient_Hub.PatientHubGoals(objData);
                                CCMEnrolledGoals.UnLoad();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                }
                else if (CCMEnrolledGoals.params.mode == "Edit") {
                    //AppPrivileges.GetFormPrivileges("Notes_Notes", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                    if (strMessage == "") {
                        CCMEnrolledGoals.savePlanOfCare(objData).done(function (response) {

                            if (response.status != false) {
                                utility.DisplayMessages("Successfully Updated", 1);
                                CCMEnrolledGoals.params.mode = "Edit";

                                var objData = new Object();
                                objData["EnrollmentInfoId"] = CCMEnrolledGoals.params.EnrollmentInfoId;
                                CCM_Patient_Hub.PatientHubGoals(objData);

                                CCMEnrolledGoals.UnLoad();
                            }
                            else {
                                utility.DisplayMessages(response.message, 3);
                            }
                        });
                    }
                    else
                        utility.DisplayMessages(strMessage, 2);
                    //});
                }
            }
            else {
                utility.DisplayMessages("No Goals found", 3);
            }
            //}
        }
    },

    savePlanOfCare: function (objData)
    {
        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "Patient_Hub", "SaveCCMEnrolledGoals_CCMEnrolledGoalsCPT");
    },

    bindAutoComplete: function (element)
    {
        var hiddenCrtl = $('#' + CCMEnrolledGoals.params.PanelID + ' #txtGoal');
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_CPTCODE', hiddenCrtl, null, true, -1, "CPT", true, "CCMEnrolledGoals", null, true);
    },

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
        //params["ParentCtrl"] = CCMEnrolledGoals.params["TabID"];
        if (CCMEnrolledGoals.params.TabID == 'clinicalTabProgressNote') {
            params['FromProgressNote'] = 'pnlClinicalProgressNote';
            params["ParentCtrl"] = 'CCMEnrolledGoals';
        }
        else {
            params["ParentCtrl"] = CCMEnrolledGoals.params["TabID"];
        }

        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            if (CCMEnrolledGoals.params.TabID == 'clinicalTabProgressNote' && SearchType == "ICD")
                LoadActionPan(controlToLoad, params, 'pnlClinicalProgressNote');
            else
                LoadActionPan(controlToLoad, params);
        }
    },

    UnLoad: function () {
        var objDeffered = $.Deferred();
        UnloadActionPan(CCMEnrolledGoals.params.ParentCtrl, 'CCMEnrolledGoals');
        objDeffered.resolve();
        return objDeffered;
    },

    InstructionHandle: function () {
        //$("#ulCCMEnrolledGoalsGoal li.active").attr('id');
        var aaa = "hfTextInstruction" + $("#ulCCMEnrolledGoalsGoal li.active").attr('id');

    }

}