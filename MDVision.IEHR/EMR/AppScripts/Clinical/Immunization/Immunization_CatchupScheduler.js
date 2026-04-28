Immunization_CatchupScheduler = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Immunization_CatchupScheduler.params = params;
        if (Immunization_CatchupScheduler.params != null && Immunization_CatchupScheduler.params.PanelID != "pnlImmunization_CatchupScheduler") {
            Immunization_CatchupScheduler.params["PanelID"] = Immunization_CatchupScheduler.params["PanelID"] + ' #pnlImmunization_CatchupScheduler';
        }
        else {

            Immunization_CatchupScheduler.params["PanelID"] = "pnlImmunization_CatchupScheduler"
        }
        Immunization_CatchupScheduler.AddHeaderFooter();
        //if (Immunization_CatchupScheduler.bIsFirstLoad) {
        //    Immunization_CatchupScheduler.bIsFirstLoad = false;
        //    var self = $('#' + Immunization_CatchupScheduler.params["PanelID"]);
        //    self.loadDropDowns(true).done(function () {
        //        Immunization_CatchupScheduler.ImmunizationPreview(Immunization_CatchupScheduler.params.PatientId, Immunization_CatchupScheduler.params.UserId, Immunization_CatchupScheduler.params.ImmunizationIds);
        //    });
        //}
    },
    AddHeaderFooter: function () {
        Clinical_ReportHeader.ReportHeaderPrint_DbCall(-1, Immunization_CatchupScheduler.params.patientID, 'Immunization').done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {

                $('#' + Immunization_CatchupScheduler.params.PanelID + " #CatchupScheduleHeader").html(response.Header);
                $('#' + Immunization_CatchupScheduler.params.PanelID + " #CatchupScheduleFooter").html(response.Footer);

            }
        });


    },
    UnLoadTab: function () {

        if (Immunization_CatchupScheduler.params != null && Immunization_CatchupScheduler.params.ParentCtrl) {
            UnloadActionPan(Immunization_CatchupScheduler.params["ParentCtrl"], "Immunization_CatchupScheduler");
        }
        else {
            UnloadActionPan();
        }

    },

}