Clinical_ProceduresComments = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_ProceduresComments.params = params;
        //serialize Data.
        //$('#frmProceduresComments').data('serialize', $('#frmProceduresComments').serialize());

        var procedureID = Clinical_ProceduresComments.params.ProcedureID;
        var patientID = Clinical_ProceduresComments.params.PatientId;

        Clinical_ProceduresComments.ProceduresSearch(procedureID, patientID);

    },

    // Fill comments function

    ProceduresSearch: function (ProcedureID, PatientId) {

        $('#pnlClinicalProcedures #txtComments').val(Clinical_ProceduresComments.params.Comments);

        var comments = $('#pnlClinicalProcedures #txtComments').val();
        $('#pnlClinicalProcedures #hfGridComments').val(comments);
        
        $('#frmProceduresComments').data('serialize', $('#frmProceduresComments').serialize());


        //Clinical_ProceduresComments.SearchProcedure(ProcedureID, PatientId).done(function (response) {
        //    response = JSON.parse(response);
        //    if (response.status != false) {
        //        var problemJSON = JSON.parse(response.ProcedureLoad_JSON);

        //        var comments = problemJSON[0].Comments;

        //        $('#pnlClinicalProcedures #txtComments').val(comments);

        //        var comments = $('#pnlClinicalProcedures #txtComments').val();
        //        $('#pnlClinicalProcedures #hfGridComments').val(comments);

        //        $('#frmProceduresComments').data('serialize', $('#frmProceduresComments').serialize());
        //    }
        //    else {
        //        utility.DisplayMessages(response.Message, 3);
        //    }
        //});
    },

    SearchProcedure: function (ProcedureID, PatientId) {

        var objData = new Object();
        objData["ProcedureID"] = ProcedureID;
        objData["PatientId"] = PatientId;
        objData["commandType"] = "search_procedures";
        objData["NotesId"] = Clinical_ProceduresComments.params.NotesId;
        objData["IsActive"] = "1";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },
    // End Fill comments function


    SaveComments: function () {

        var comments = $('#pnlClinicalProcedures #txtComments').val();

        $('#pnlClinicalProcedures #hfGridComments').val(comments);

        $('#frmProceduresComments').data('serialize', $('#frmProceduresComments').serialize());
        if (Clinical_ProceduresComments.params.ParentCtrl == "OrderSet_Procedures") {
            $("#" + OrderSet_Procedures.params.PanelID + " #dgvProcedures tbody tr[id='" + Clinical_ProceduresComments.params.ProcedureID + "']").find("#hfComments").val(comments)
        } else if (Clinical_ProceduresComments.params.ParentCtrl == "Clinical_OrderSetDetails") {
            $("#" + Clinical_OrderSetDetails.params.PanelID + " #dgvProceduresOS tbody tr[id='" + Clinical_ProceduresComments.params.ProcedureID + "']").find("#hfComments").val(comments)
        }
        else if (Clinical_ProceduresComments.params.ParentCtrl == "Clinical_Treatment") {
            $("#" + Clinical_Treatment.params.PanelID + " #dgvProceduresT tbody tr[id='" + Clinical_ProceduresComments.params.ProcedureID + "']").find("#hfComments").val(comments)
        }
        Clinical_ProceduresComments.UnLoad();

        Clinical_Procedures.setCommentsField(Clinical_ProceduresComments.params.ProcedureID, comments);
    },

   
    CommentsSave: function () {

        var vitalSignId = Clinical_ProceduresComments.params.ProcedureID;
        var patientId = Clinical_ProceduresComments.params.PatientId;
        var comments = $('#pnlClinicalProcedures #txtComments').val();

        var objData = new Object();
        objData["ProcedureID"] = vitalSignId;
        objData["PatientId"] = patientId;
        objData["Comments"] = comments;
        objData["commandType"] = "update_procedurecomments";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },

    UnLoad: function () {

        //utility.UnLoadDialog('frmProceduresComments', function () {
        //    UnloadActionPan(Clinical_ProceduresComments.params["ParentCtrl"], "actionPanProceduresComments");
        //}, function () {
        //    UnloadActionPan(Clinical_ProceduresComments.params["ParentCtrl"], "actionPanProceduresComments");
        //});

        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */
        if (Clinical_ProceduresComments.params.ParentCtrl == 'clinicalTabProcedures') {
            UnloadActionPan(Clinical_ProceduresComments.params["ParentCtrl"], "Clinical_ProceduresComments");
        } else {
            UnloadActionPan(Clinical_ProceduresComments.params.ParentCtrl, 'Clinical_ProceduresComments', null, Clinical_ProceduresComments.params.PanelID);
        }
        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */
    },
}