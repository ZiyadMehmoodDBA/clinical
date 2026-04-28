OrderSet_OrdAndPatientProbSelection = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        OrderSet_OrdAndPatientProbSelection.params = params;
        $('#actionPanClinicalProgressNote #pnlOrderSetAndPatientProblem #titleBlockReason').text(OrderSet_OrdAndPatientProbSelection.params.OSName);
        if (OrderSet_OrdAndPatientProbSelection.params.PanelID != 'pnlOrderSetAndPatientProblem') {
            OrderSet_OrdAndPatientProbSelection.params.PanelID = OrderSet_OrdAndPatientProbSelection.params.PanelID + ' #pnlOrderSetAndPatientProblem';
        }
        else {
            OrderSet_OrdAndPatientProbSelection.params.PanelID = 'pnlOrderSetAndPatientProblem';
        }
            OrderSet_OrdAndPatientProbSelection.ShowDivRadiobtnOptions();
            OrderSet_OrdAndPatientProbSelection.HideProblemsDiv();
            OrderSet_OrdAndPatientProbSelection.BindRadioButtonChangeEvent();
            $('#' + OrderSet_OrdAndPatientProbSelection.params.PanelID + ' input[type=radio][id=rdReplace]').prop('checked', true);
            OrderSet_OrdAndPatientProbSelection.SetOkButtonOnClick("replace");
    },
    ReplaceOrderSet: function () {
        Clinical_OrderSetDetails.detach_ComponentsOrderSet("OrderSets", true, true).done(function () {
            Clinical_ProgressNote.AddOrderSet(OrderSet_OrdAndPatientProbSelection.params.OrderSetId,true);
            OrderSet_OrdAndPatientProbSelection.UnLoad();
        });
    },
    BindRadioButtonChangeEvent: function () {
        $('#' + OrderSet_OrdAndPatientProbSelection.params.PanelID + ' input[type=radio][name=rdBtn_OS_Action]').change(function () {
            if (this.value == "replace") {
                OrderSet_OrdAndPatientProbSelection.ShowWarningLabel();
                OrderSet_OrdAndPatientProbSelection.HideProblemsDiv();
            }
            else if (this.value == "merge") {
                OrderSet_OrdAndPatientProbSelection.HideWarningLabel();
                OrderSet_OrdAndPatientProbSelection.ShowProblemsDiv();
                OrderSet_OrdAndPatientProbSelection.LoadPatientAndOrdProblemsWithDBCall();
            }
            OrderSet_OrdAndPatientProbSelection.SetOkButtonOnClick(this.value);
        });
    },
    SetOkButtonOnClick: function (type) {
        $('#' + OrderSet_OrdAndPatientProbSelection.params.PanelID + ' #btnOS_Ok').attr('onClick', (type && type == "replace") ? "OrderSet_OrdAndPatientProbSelection.ReplaceOrderSet()" : "OrderSet_OrdAndPatientProbSelection.OpenOrderSet()");
    },
    HideWarningLabel: function () {
        $('#' + OrderSet_OrdAndPatientProbSelection.params.PanelID + ' #lblWarning').hide();
    },
    ShowWarningLabel: function () {
        $('#' + OrderSet_OrdAndPatientProbSelection.params.PanelID + ' #lblWarning').show();
    },
    HideProblemsDiv: function () {
        $('#' + OrderSet_OrdAndPatientProbSelection.params.PanelID + ' #divProblems').hide();
    },
    ShowProblemsDiv: function () {
        $('#' + OrderSet_OrdAndPatientProbSelection.params.PanelID + ' #divProblems').show();
    },
    HideDivRadiobtnOptions: function () {
        $('#' + OrderSet_OrdAndPatientProbSelection.params.PanelID + ' #divRadiobtnOptions').hide();
    },
    ShowDivRadiobtnOptions: function () {
        $('#' + OrderSet_OrdAndPatientProbSelection.params.PanelID + ' #divRadiobtnOptions').show();
    },
    LoadPatientAndOrdProblemsWithDBCall: function () {
        Clinical_ProgressNote.LoadPatientAndOrdProblems_DBCALL(OrderSet_OrdAndPatientProbSelection.params.OrderSetId, OrderSet_OrdAndPatientProbSelection.params.PatientId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                OrderSet_OrdAndPatientProbSelection.params.listProblem = response.listProblem;
                OrderSet_OrdAndPatientProbSelection.RemoveProblemUL();
                OrderSet_OrdAndPatientProbSelection.LoadPatientAndOrdProblems();
            }
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    RemoveProblemUL: function () {
        $("#" + OrderSet_OrdAndPatientProbSelection.params.PanelID + " #PProblems li").remove();
        $("#" + OrderSet_OrdAndPatientProbSelection.params.PanelID + " #OProblems li").remove();
    },
    LoadPatientAndOrdProblems: function () {
        $.each(OrderSet_OrdAndPatientProbSelection.params.listProblem, function (i, item) {
            var Icd9 = item.ICD9 ? item.ICD9 + ' - ' : '';
            if (item.PatientProblem.toLowerCase() == "true") {
                var li = '<li id="PatientProb_' + item.ProblemId + '" ICD9="' + item.ICD9 + '" ICD10="' + item.ICD10 + '" Icd10Description="' + item.Icd10Description + '"><input type="checkbox" id="chkPatientProb_' + item.ProblemId + '" name="IsChiefComplaint" value=""><span>' + "   " + Icd9 + item.ICD10 + ' - ' + item.Icd10Description + '</span></li>';
                $("#" + OrderSet_OrdAndPatientProbSelection.params.PanelID + " #PProblems").append(li);
            }
            else {
                var li = '<li id="OrderSetProb_' + item.ProblemId + '" ICD9="' + item.ICD9 + '" ICD10="' + item.ICD10 + '" Icd10Description="' + item.Icd10Description + '"><input type="checkbox" id="chkOrderSetProb_' + item.ProblemId + '" name="IsChiefComplaint" value=""><span>' + "   " + Icd9 + item.ICD10 + ' - ' + item.Icd10Description + '</span></li>';
                $("#" + OrderSet_OrdAndPatientProbSelection.params.PanelID + " #OProblems").append(li);
            }
        });
    },
    LoadPatientAndOrdProblems_DBCALL: function () {
        var objData = {};
        objData["OrderSetId"] = OrderSet_OrdAndPatientProbSelection.params.OrderSetId;
        objData["PatientId"] = OrderSet_OrdAndPatientProbSelection.params.PatientId;
        objData["commandType"] = "Load_Patient_And_Ord_Problems";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "OrderSet", "OrderSet");
    },
    OpenOrderSet: function () {
        var SelectedProblems = [];
        var count = -1;
        $.each($("#" + OrderSet_OrdAndPatientProbSelection.params.PanelID + " #frmProblemListsComments input[type=checkbox]"), function (i, item) {
            if ($(this).prop("checked")) {
                var Icd9 = $(this).parent().attr("ICD9")!="null" ? $(this).parent().attr("ICD9") + ' - ' : '';
                var obj = {};
                obj.id = $(this).parent().attr("id");
                obj.ICD9 = $(this).parent().attr("ICD9");
                obj.ICD10 = $(this).parent().attr("ICD10");
                obj.Icd10Description = $(this).parent().attr("Icd10Description");
                obj.SnomedId = "";
                obj.SnomedDescription = "";
                obj.SnomedCode = "";
                obj.OrderSetProblemId = count;
                obj.Problem = Icd9 + $(this).parent().attr("ICD10") + " - " + $(this).parent().attr("Icd10Description");
                SelectedProblems.push(obj);
                count--;
            }
        });
        //if (SelectedProblems.length > 0) {
            var params = [];
            if (OrderSet_OrdAndPatientProbSelection.params.CDSId == null) {
                if (OrderSet_OrdAndPatientProbSelection.params["ParentCtrl"] == "Clinical_OrderSets") {
                    params["ParentCtrl"] = "Clinical_OrderSets";
                }
                else {
                    params["ParentCtrl"] = "clinicalTabProgressNote";
                }
            } else {
                params["ParentCtrl"] = "clinicalTabProgressNote";
            }
            params["OrderSetId"] = OrderSet_OrdAndPatientProbSelection.params.OrderSetId;
            params["PatientId"] = Clinical_ProgressNote.params.patientID;
            params["CDSId"] = OrderSet_OrdAndPatientProbSelection.params.CDSId;
            params["NoteId"] = Clinical_ProgressNote.params.NotesId;
            params["IsNotes"] = true;
            params["mode"] = "View";
            params["FromAdmin"] = "0";
            params["ParentCtrlPanelID"] = Clinical_ProgressNote.params.PanelID;
            params["OSName"] = OrderSet_OrdAndPatientProbSelection.params.OSName;
            params["SelectedProblems"] = SelectedProblems;
            params["DirectFromNotes"] = true;
            params["ShowSelectedProblems"] = SelectedProblems;
            params["OrderSetType"] = OrderSet_OrdAndPatientProbSelection.params.OrderSetType;
            setTimeout(function (params) {
                LoadActionPan('Clinical_OrderSetDetails', params);
            }, 900, params);
            OrderSet_OrdAndPatientProbSelection.UnLoad();
        //}
        //else {
        //    utility.DisplayMessages("Select atleast one problem", 2);
        //}
    },
    UnLoad: function () {
        if (OrderSet_OrdAndPatientProbSelection.params != null && OrderSet_OrdAndPatientProbSelection.params.ParentCtrl) {
            OrderSet_OrdAndPatientProbSelection.params.PanelID = OrderSet_OrdAndPatientProbSelection.params.PanelID.replace(" #pnlOrderSetAndPatientProblem", "");
            UnloadActionPan(OrderSet_OrdAndPatientProbSelection.params.ParentCtrl, 'OrderSet_OrdAndPatientProbSelection', null, OrderSet_OrdAndPatientProbSelection.params.PanelID);
            Clinical_ProgressNote.ResetDefaultOrderSet();
        }
        else {
            UnloadActionPan();
        }
    },
}