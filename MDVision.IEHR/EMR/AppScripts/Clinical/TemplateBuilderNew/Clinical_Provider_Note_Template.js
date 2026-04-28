// Created By:  Muhammad Ahmad Imran
// Created Date: 3/15/2016
Clinical_Provider_Note_Template = {
    bIsFirstLoad: true,
    params: [],
    Switch: 1,
    Load: function (params) {
        Clinical_Provider_Note_Template.params = params;

        if (Clinical_Provider_Note_Template.params.PanelID != 'pnlClinicalProviderNoteTemplate') {
            Clinical_Provider_Note_Template.params.PanelID = Clinical_Provider_Note_Template.params.PanelID + ' #pnlClinicalProviderNoteTemplate';
        } else {
            Clinical_Provider_Note_Template.params.PanelID = 'pnlClinicalProviderNoteTemplate';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + Clinical_Provider_Note_Template.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        var self = $('#' + Clinical_Provider_Note_Template.params.PanelID);
        self.loadDropDowns(true).done(function () {
            Clinical_Provider_Note_Template.IntializeMultiSelectDropDown();
        });
    },
    //Start//03/15/2016//M Ahmad Imran//Implimented "IntializeMultiSelectDropDown" which intialize all multi select dropdowns
    IntializeMultiSelectDropDown: function () {


        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlSpecialty').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 116

        });
        $('#' + Clinical_Provider_Note_Template.params.PanelID + ' #ddlProvider').multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            enableCaseInsensitiveFiltering: true,
            maxHeight: 116

        });

    },
    //End M Ahmad Imran 03/15/2016
    //Start//03/15/2016//M Ahmad Imran//Implimented "AddProviderNoteTemplate" which open provider note template page in popup 
    AddProviderNoteTemplate: function () {
        var params = [];
        params["ParentCtrl"] = "Clinical_Provider_Note_Template";
        params["FromAdmin"] = 0;
        params["mode"] = "Add";
        LoadActionPan("Clinical_Add_Provider_Note_Template", params);
    },
    //End M Ahmad Imran 03/15/2016
    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (Clinical_Provider_Note_Template.params["FromAdmin"] == "0") {
            if (Clinical_Provider_Note_Template.params != null && Clinical_Provider_Note_Template.params.ParentCtrl != null) {
                UnloadActionPan(Clinical_Provider_Note_Template.params.ParentCtrl, 'Clinical_Provider_Note_Template');
            }
            else
                UnloadActionPan(null, 'Clinical_Provider_Note_Template');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },
}