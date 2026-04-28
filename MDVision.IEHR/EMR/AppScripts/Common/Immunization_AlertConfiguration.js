Immunization_AlertConfiguration = {
    bIsFirstLoad: true,
    params: [],
    ActiveTabDBString: "",
    Load: function (params) {
        Immunization_AlertConfiguration.params = params;
        if (Immunization_AlertConfiguration.params.PanelID != 'pnlImmunization_AlertConfiguration') {
            Immunization_AlertConfiguration.params.PanelID = Immunization_AlertConfiguration.params.PanelID + ' #pnlImmunization_AlertConfiguration';
        } else {
            Immunization_AlertConfiguration.params.PanelID = 'pnlImmunization_AlertConfiguration';
        }

        Immunization_AlertConfiguration.ActiveInActiveTab('Birth_2_years');
    },
    ActiveInActiveTab: function (TabId) {
        if (TabId != "") {
            $('#' + Immunization_AlertConfiguration.params.PanelID).find('[id*="Alertlist"]').removeClass('active');
            $('#' + Immunization_AlertConfiguration.params.PanelID).find('[id*="tabAlert_"]').removeClass('active');
            $('#' + Immunization_AlertConfiguration.params.PanelID + ' #Alertlist' + TabId).addClass('active');
            $('#' + Immunization_AlertConfiguration.params.PanelID + ' #tabAlert_' + TabId).addClass('active');
            Immunization_AlertConfiguration.SetActiveTabDbString(TabId);
        }
    },
    SetActiveTabDbString: function (TabId) {
        if (TabId != "") {
            if (TabId == "Birth_2_years") {
                Immunization_AlertConfiguration.ActiveTabDBString = "Birth - 2 Years";
            }
            else if (TabId == "2_18_years") {
                Immunization_AlertConfiguration.ActiveTabDBString = "2 - 18 Years";
            }
            else if (TabId == "Adult") {
                Immunization_AlertConfiguration.ActiveTabDBString = "Adult";
            }
            else if (TabId == "Recurring") {
                Immunization_AlertConfiguration.ActiveTabDBString = "Recurring";
            }
        }
    },
    unLoadTab: function () {
        if (Immunization_AlertConfiguration.params["FromAdmin"] == "0") {
            if (Immunization_AlertConfiguration.params != null && Immunization_AlertConfiguration.params.ParentCtrl != null) {
                UnloadActionPan(Immunization_AlertConfiguration.params.ParentCtrl, 'Immunization_AlertConfiguration');
            } else
                UnloadActionPan(null, 'Immunization_AlertConfiguration');
        } else {
            RemoveAdminTab();
        }
    },
    OpenAlertsPage: function () {
        //Immunization_AlertConfiguration.GetImmunizationAlertCount_DB_CALL($('#PatientProfile #hfPatientId').val()).done(function (response) {
        //    response = JSON.parse(response);
        //    if (response.status != false) {

        BackgroundLoaderShow(true);
        var params = [];
        params["FromAdmin"] = 0;
        LoadActionPan("Immunization_AlertPreview", params);

        //    }
        //});
    },
    SetImmunizationAlertCount: function (PatientId, VaccineInsert) {
        var dfd = $.Deferred();
        $.when(Immunization_AlertConfiguration.InsertOrUpdatePatientImmunizationAlert(PatientId, VaccineInsert)).done(function (response) {
            if (response.status != false) {
                if (response.ImmunizationAlertCount != '0') {
                    $("#mainForm  li#ImmunizationAlert Span").html(response.ImmunizationAlertCount);
                    dfd.resolve();
                }
                else {
                    $("#mainForm  li#ImmunizationAlert Span").html("");
                    dfd.resolve();
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    OnlySetImmunizationAlertCount: function (PatientId) {
        var dfd = $.Deferred();
        Immunization_AlertConfiguration.GetImmunizationAlertCount_DB_CALL(PatientId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ImmunizationAlertCount != '0') {
                    $(" #mainForm  li#ImmunizationAlert Span").html(response.ImmunizationAlertCount);
                    dfd.resolve();
                }
                else {
                    $(" #mainForm  li#ImmunizationAlert Span").html("");
                    dfd.resolve();
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    InsertOrUpdatePatientImmunizationAlert: function (PatientId, IsVaccineInsert) {
        var dfd = $.Deferred();
        Immunization_AlertConfiguration.InsertOrUpdatePatientImmunizationAlert_DB_CALL(PatientId, IsVaccineInsert).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                dfd.resolve(response);
            }
            else {

                utility.DisplayMessages(response.Message, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    InsertOrUpdatePatientImmunizationAlert_DB_CALL: function (PatientId, IsVaccineInsert) {
        var objData = {};
        objData["PatientId"] = PatientId;
        objData["IsVaccineInsert"] = IsVaccineInsert;
        objData["commandType"] = "InsertOrUpdatePatientImmunizationAlert";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Immunization");
    },
    GetImmunizationAlertCount_DB_CALL: function (PatientId) {
        var objData = {};
        objData["PatientId"] = PatientId;
        objData["commandType"] = "Get_ImmunizationAlertCount";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Immunization");
    },

};