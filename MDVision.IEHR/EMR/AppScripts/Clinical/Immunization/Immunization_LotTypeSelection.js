Immunization_LotTypeSelection = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        if (params != null) {
            Immunization_LotTypeSelection.params = params;
        }
    },
    UnLoad: function () {
        UnloadActionPan(Immunization_LotTypeSelection.params["ParentCtrl"], "Immunization_LotTypeSelection");
    },

    OpenAddLot: function (Type) {
        var params = [];
        params["ParentCtrl"] = "Immunization_LotNumber";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        params["PatientId"] = Immunization_LotTypeSelection.params["patientID"];
        params["FromAdmin"] = Immunization_LotTypeSelection.params["FromAdmin"];
        if (Type == "vaccine") {
            //params["VaccineId"] = Immunization_LotTypeSelection.params["VaccineId"];
            //params["VaccineText"] = Immunization_LotTypeSelection.params["VaccineText"];
            params["Type"] = 'vaccine';
        }
        else if (Type == "therapeutic") {
            //params["TherapeuticInjectionId"] = Immunization_LotNumber.params.TherapeuticInjectionId;
            //params["TherapeuticInjectionText"] = Immunization_LotNumber.params.TherapeuticInjectionText;
            params["Type"] = 'TherapeuticInjection';
        }
        // params["ParentCtrl"] = Admin_BillingProvider.params["ParentCtrl"];
        //if (Immunization_LotNumber.params["FromAdmin"] == "0") {
        //    params["ParentCtrl"] = 'Immunization_LotNumber';
        //}
        setTimeout(function (params) {
            LoadActionPan("Immunization_LotNumberDetail", params);
        }, 900, params);
        Immunization_LotTypeSelection.UnLoad();
        
    },
}