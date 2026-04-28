Treatment_ProblemSelection = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {

        Treatment_ProblemSelection.params = params;
        Treatment_ProblemSelection.params.DeleteProblem = [];
        if (Treatment_ProblemSelection.params.PanelID != 'pnlTreatment_ProblemSelection') {
            Treatment_ProblemSelection.params.PanelID = Treatment_ProblemSelection.params.PanelID + ' #pnlTreatment_ProblemSelection';
        }
        else {
            Treatment_ProblemSelection.params.PanelID = 'pnlTreatment_ProblemSelection';
        }
        Treatment_ProblemSelection.LoadTreatmentProblems();
    },
    LoadTreatmentProblems: function () {
        var Treatmentexists = false;
        var DeleteTreatmentexists = false;
        $.each(Treatment_ProblemSelection.params.TreatmentProblems, function (i, item) {
            var IsDeleteTreatment = false;
            if (item.PrescriptionIds == "" &&
            item.LabOrderIds == "" &&
            item.DiagnosticImagingIds == "" &&
            item.ProcedureIds == "" &&
            item.ImmunizationIds == "" &&
            item.TherapeuticIds == "" &&
            item.ReferralIds == "") {
                if (item.TreatmentId > 0) {
                    Treatment_ProblemSelection.params.DeleteProblem.push(item);
                    IsDeleteTreatment = true;
                }
            }
            if (!IsDeleteTreatment) {
                Treatmentexists = true;
                var params = "this,'" + item.ProblemDescription + "'";
                var funtion = "onchange = \"Treatment_ProblemSelection.ChangeTreatmentSelection(this,'" + item.ProblemDescription + "')\"";
                var li = '<li id="' + item.ProblemId + '" ><input ' + (item.TreatmentId > 0 ? "checked" : "") + ' type="checkbox" id="' + item.ProblemId + '"  value="" ' + (item.TreatmentId > 0 ? funtion : "") + '><span>' + "   " + item.ProblemDescription + '</span></li>';
                $("#" + Treatment_ProblemSelection.params.PanelID + " #TreatmentProblems").append(li);
            }
            else {
                DeleteTreatmentexists = true;
                var li = '<li id="' + item.ProblemId + '" ><span>' + "   " + item.ProblemDescription + '</span></li>';
                $("#" + Treatment_ProblemSelection.params.PanelID + " #DeleteTreatmentProblems").append(li);
            }
        });
        if (!Treatmentexists) {
            $("#" + Treatment_ProblemSelection.params.PanelID + " #TreatmentProblemDiv").hide();
        }
        if (!DeleteTreatmentexists) {
            $("#" + Treatment_ProblemSelection.params.PanelID + " #DeleteTreatmentProblemDiv").hide();
        }
    },
    ChangeTreatmentSelection: function (obj, ProblemDescription) {
        if ($(obj).prop("checked")) {
            if ($("#" + Treatment_ProblemSelection.params.PanelID + " #DeleteTreatmentProblems li#" + $(obj).attr("id")).length > 0) {
                $("#" + Treatment_ProblemSelection.params.PanelID + " #DeleteTreatmentProblems li#" + $(obj).attr("id")).remove();
                if ($("#" + Treatment_ProblemSelection.params.PanelID + " #DeleteTreatmentProblems li").length == 0) {
                    $("#" + Treatment_ProblemSelection.params.PanelID + " #DeleteTreatmentProblemDiv").hide();
                }
            }
        }
        else {
            if ($("#" + Treatment_ProblemSelection.params.PanelID + " #DeleteTreatmentProblems li#" + $(obj).attr("id")).length == 0) {
                $("#" + Treatment_ProblemSelection.params.PanelID + " #DeleteTreatmentProblemDiv").show();
                var li = '<li id="' + $(obj).attr("id") + '" ><span>' + "   " + ProblemDescription + '</span></li>';
                $("#" + Treatment_ProblemSelection.params.PanelID + " #DeleteTreatmentProblems").append(li);
            }
        }
    },
    LoadPatientAndOrdProblems_DBCALL: function () {
        var objData = {};
        objData["OrderSetId"] = Treatment_ProblemSelection.params.OrderSetId;
        objData["PatientId"] = Treatment_ProblemSelection.params.PatientId;
        objData["commandType"] = "Load_Patient_And_Ord_Problems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    AddToNote: function () {
        var SelectedProblems = [];
        var MayBeDeleteProblems = [];

        var count = 0;
        $.each($("#" + Treatment_ProblemSelection.params.PanelID + " #frmProblemListsComments input"), function (i, item) {
            var ProbId = $(this).parent().attr("id");
            var CurrentProblem = $.grep(Treatment_ProblemSelection.params.TreatmentProblems, function (n, i) {
                return n.ProblemId == ProbId;
            });
            if ($(this).prop("checked")) {
                if (CurrentProblem.length > 0) {
                    SelectedProblems.push(CurrentProblem[0]);
                    count++;
                }
            }
            else {
                if (CurrentProblem.length > 0) {
                    MayBeDeleteProblems.push(CurrentProblem[0]);
                }
            }
        });

        $.each(MayBeDeleteProblems, function (i, item) {
            if (item.TreatmentId > 0) {
                Treatment_ProblemSelection.params.DeleteProblem.push(item);
            }
        });
        ////TreatmentId
        if (SelectedProblems.length > 0 || Treatment_ProblemSelection.params.DeleteProblem.length > 0) {
            Treatment_ProblemSelection.UnLoad();
            Treatment_ProblemSelection.SaveUpdateTreatment(SelectedProblems, Treatment_ProblemSelection.params.DeleteProblem).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    Clinical_Treatment.createTreatmentBodyHTML(response.TreatmentData, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', false);
                    Clinical_Treatment.UnLoadTab(false);
                    EMRUtility.scrollToPNcomponent('clinical_treatment');
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            utility.DisplayMessages("Select atleast one Treatment Plan", 2);
        }
    },
    SaveUpdateTreatment: function (SelectedProblems, DeleteProblems) {
        var objData = {};
        objData.NoteId = Treatment_ProblemSelection.params.NoteId;
        objData.Comment = Treatment_ProblemSelection.params.Comment;
        objData.Treatments = SelectedProblems;
        objData.DeleteTreatments = DeleteProblems;
        objData["commandType"] = "SAVE_TREATMENT";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "ClinicalNotes", "Treatment");
    },
    UnLoad: function () {
        if (Treatment_ProblemSelection.params["FromAdmin"] == "0") {
            if (Treatment_ProblemSelection.params != null && Treatment_ProblemSelection.params.ParentCtrl != null) {
                UnloadActionPan(Treatment_ProblemSelection.params.ParentCtrl, 'Treatment_ProblemSelection');
            }
            else
                UnloadActionPan(null, 'Clinical_Treatment');
        }
        else {
            RemoveAdminTab();
        }
    },
}