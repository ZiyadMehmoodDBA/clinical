Immunization_VaccineTypeSelection = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        if (params != null) {
            Immunization_VaccineTypeSelection.params = params;
        }
    },
    UnLoad: function () {
        UnloadActionPan(Immunization_VaccineTypeSelection.params["ParentCtrl"], "Immunization_VaccineTypeSelection");
    },

    OpenAddVaccine: function (Type) {
        var params = [];
        var Page = "Immunization_AddVaccine";
        params["ParentCtrl"] = "Immunization_ImmunizationAddImmInj";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        params["FromAdmin"] = Immunization_VaccineTypeSelection.params["FromAdmin"];
        if (Type == "immunization") {
            Page = "Immunization_AddVaccine";
        }
        else if (Type == "therapeutic") {
            Page = "Immunization_TherapeuticDetail";
        }
        
        setTimeout(function (params) {
            LoadActionPan(Page, params);
        }, 900, params);
        Immunization_VaccineTypeSelection.UnLoad();
    },
}