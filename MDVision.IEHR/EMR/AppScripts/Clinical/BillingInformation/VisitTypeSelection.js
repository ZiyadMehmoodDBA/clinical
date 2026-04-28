VisitTypeSelection = {
    //Author: Farooq Ahmad
    //Date: 21-07-2016
    //This file will handle all actions performed for Billing Information.
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    FamilyMembers: [],
    ExamDetails: {},
    SelectedSystem: '',
    array: [],
    myArr: [],
    parentCtrlGlobel: null,
    mainSelected: null,
    SectionNormalInfo: [],
    selectedcharacteristicsIds: [],
    characteristicsWithData: [],
    selectedsubcharacteristicsIds: [],
    subcharacteristicsWithData: [],
    selectedData: null,
    isNormalTriggred: false,
    isBothUnCheck: false,
    specialityCheckedIds: [],
    providerSelectedIds: [],
    providerCheckedIds: [],
    normalSystemIdsGlobel: [],
    selectedPhyExamTempData: [],
    SpecialtyIds: '',
    ProviderIds: '',


    Load: function (params) {

        if (params != null) {
            VisitTypeSelection.params = params;


        }

    },

    BillingInfoSave: function (objData) {
        objData["BillingInfoId"] = '-1'
        objData["commandType"] = "BILLING_INFORMATION_SAVE";
        objData["NotesId"] = VisitTypeSelection.params.NotesId;
        objData["PatientId"] = VisitTypeSelection.params.PatientId;
        objData["ProviderId"] = VisitTypeSelection.params["ProviderId"];
        objData["VisitId"] = VisitTypeSelection.params.VisitId;
        objData["Status"] = 'Draft';
        objData["VisitDate"] = VisitTypeSelection.params.VisitDate;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "BillingInformation", "BillingInformation");

    },

    NewPatientClick: function () {
        var Obj = new Object();
        Obj["ENMTypeId"] = '1';
        VisitTypeSelection.BillingInfoSave(Obj).done(function (response) {
            response = JSON.parse(response);

            IsRemoveNoteComponent = false;

            if (response.status) {
                $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val(response.BillingInfoId);
                var params = [];
                params["ParentCtrl"] = "clinicalTabProgressNote";
                params["FromAdmin"] = 0;
                params["ProviderId"] = VisitTypeSelection.params["ProviderId"];
                params["NotesId"] = VisitTypeSelection.params.NotesId;
                params["NoteDate"] = VisitTypeSelection.params.NoteDate;
                params["VisitDate"] = VisitTypeSelection.params.VisitDate;
                params["PatientId"] = VisitTypeSelection.params.PatientId;
                params["VisitId"] = VisitTypeSelection.params.VisitId;
                params["BillingInfoId"] = response.BillingInfoId;
                params["NoteStatus"] = VisitTypeSelection.params.NoteStatus;
                setTimeout(function (params) {
                    LoadActionPan("BillingInformation", params);
                }, 900, params);
                VisitTypeSelection.UnLoad();
            }
            else
                VisitTypeSelection.UnLoad();

            setTimeout(function () { IsRemoveNoteComponent = true; }, 1000);

        });




    },

    EstdPatientClick: function () {
        var Obj = new Object();
        Obj["ENMTypeId"] = '2';
        VisitTypeSelection.BillingInfoSave(Obj).done(function (response) {
            response = JSON.parse(response);

            IsRemoveNoteComponent = false;

            if (response.status) {
                $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val(response.BillingInfoId);
                var params = [];
                params["ParentCtrl"] = "clinicalTabProgressNote";
                params["FromAdmin"] = 0;
                params["ProviderId"] = VisitTypeSelection.params["ProviderId"];
                params["VisitDate"] = VisitTypeSelection.params.VisitDate;
                params["NotesId"] = VisitTypeSelection.params.NotesId;
                params["NoteDate"] = VisitTypeSelection.params.NoteDate;
                params["PatientId"] = VisitTypeSelection.params.PatientId;
                params["VisitId"] = VisitTypeSelection.params.VisitId;
                params["BillingInfoId"] = response.BillingInfoId;
                params["NoteStatus"] = VisitTypeSelection.params.NoteStatus;
                setTimeout(function (params) {
                    LoadActionPan("BillingInformation", params);
                }, 900, params);
                VisitTypeSelection.UnLoad();
            }
            else
                VisitTypeSelection.UnLoad();

            setTimeout(function () { IsRemoveNoteComponent = true; }, 1000);
        });
    },

    eSuperbillCCM: function (params) {
        if (params != null) {
            VisitTypeSelection.params = params;
            if (VisitTypeSelection.params.PatientId == undefined || VisitTypeSelection.params.PatientId == 'undefined' || VisitTypeSelection.params.PatientId == null || VisitTypeSelection.params.PatientId == "") {
                VisitTypeSelection.params.PatientId = VisitTypeSelection.params.patientID;
            }

        }
        var Obj = new Object();
        //Obj["ENMTypeId"] = '2';
        VisitTypeSelection.BillingInfoSave(Obj).done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                $('#' + Clinical_ProgressNote.params.PanelID + ' #hfBillingInfoId').val(response.BillingInfoId);
                var params = [];
                params["ParentCtrl"] = "clinicalTabProgressNote";
                params["FromAdmin"] = 0;
                params["ProviderId"] = VisitTypeSelection.params["ProviderId"];
                params["VisitDate"] = VisitTypeSelection.params.VisitDate;
                params["NotesId"] = VisitTypeSelection.params.NotesId;
                params["NoteDate"] = VisitTypeSelection.params.NoteDate;
                params["PatientId"] = VisitTypeSelection.params.PatientId;
                params["VisitId"] = VisitTypeSelection.params.VisitId;
                params["BillingInfoId"] = response.BillingInfoId;
                params["NoteStatus"] = VisitTypeSelection.params.NoteStatus
                params["FromCCM"] = VisitTypeSelection.params.FromCCM;
                setTimeout(function (params) {
                    LoadActionPan("BillingInformation", params);
                }, 900, params);
                VisitTypeSelection.UnLoad();
            }
            else
                VisitTypeSelection.UnLoad()
        });
    },

    UnLoad: function () {

        UnloadActionPan(VisitTypeSelection.params["ParentCtrl"], "VisitTypeSelection");

    },






}