PhysicalExamDataTemplateSaveAs = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        PhysicalExamDataTemplateSaveAs.params = params;


        PhysicalExamDataTemplateSaveAs.params.PanelID = 'pnlClinicalPhysicalExamDataTemplateSaveAs';


        PhysicalExamDataTemplateSaveAs.validatePhysicalExamDataTemplateSaveAs();
    },
    validatePhysicalExamDataTemplateSaveAs: function () {
        $('#' + PhysicalExamDataTemplateSaveAs.params.PanelID + " #frmClinicalPhysicalExamDataTemplateSaveAs")
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   DataTemplateNameSaveAs: {
                       group: '.col-sm-12',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (e.type == "success") {

                var $form = $(e.target);
                var $button = $form.data('bootstrapValidator').getSubmitButton();
                switch ($button.attr('id')) {
                    case 'btnDataTemplateSaveAs':
                        var newTemplateName = $('#' + PhysicalExamDataTemplateSaveAs.params.PanelID + " #txtDataTemplateNameSaveAs").val();
                        PhysicalExamDataTemplateDetail.addUpdatePhysExamDataTemplate(true, newTemplateName);
                        PhysicalExamDataTemplateSaveAs.UnLoad();
                        break;

                }

            }
            e.type = "";
        });

    },
    // Fill comments function

    ProceduresSearch: function (ProcedureID, PatientId) {

        PhysicalExamDataTemplateSaveAs.SearchProcedure(ProcedureID, PatientId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var problemJSON = JSON.parse(response.ProcedureLoad_JSON);

                var comments = problemJSON[0].Comments;

                $('#pnlClinicalProcedures #txtComments').val(comments);

                var comments = $('#pnlClinicalProcedures #txtComments').val();
                $('#pnlClinicalProcedures #hfGridComments').val(comments);

                $('#frmProceduresComments').data('serialize', $('#frmProceduresComments').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    SearchProcedure: function (ProcedureID, PatientId) {

        var objData = new Object();
        objData["ProcedureID"] = ProcedureID;
        objData["PatientId"] = PatientId;
        objData["commandType"] = "search_procedures";
        objData["NotesId"] = PhysicalExamDataTemplateSaveAs.params.NotesId;
        objData["IsActive"] = "1";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Procedure");

    },
    // End Fill comments function


    SaveComments: function () {

        var comments = $('#pnlClinicalProcedures #txtComments').val();

        $('#pnlClinicalProcedures #hfGridComments').val(comments);

        $('#frmProceduresComments').data('serialize', $('#frmProceduresComments').serialize());

        PhysicalExamDataTemplateSaveAs.UnLoad();
    },

    //SaveComments: function () {
    //    PhysicalExamDataTemplateSaveAs.CommentsSave().done(function (response) {
    //        response = JSON.parse(response);
    //        if (response.status != false) {
    //            $('#frmProceduresComments').data('serialize', $('#frmProceduresComments').serialize());
    //            PhysicalExamDataTemplateSaveAs.UnLoad();
    //            utility.DisplayMessages(response.message, 1);

    //        }
    //        else {
    //            $('#frmProceduresComments').data('serialize', $('#frmProceduresComments').serialize());
    //            PhysicalExamDataTemplateSaveAs.UnLoad();
    //            utility.DisplayMessages(response.message, 3);
    //        }
    //    });
    //},

    CommentsSave: function () {

        var vitalSignId = PhysicalExamDataTemplateSaveAs.params.ProcedureID;
        var patientId = PhysicalExamDataTemplateSaveAs.params.PatientId;
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
        //    UnloadActionPan(PhysicalExamDataTemplateSaveAs.params["ParentCtrl"], "actionPanProceduresComments");
        //}, function () {
        //    UnloadActionPan(PhysicalExamDataTemplateSaveAs.params["ParentCtrl"], "actionPanProceduresComments");
        //});

        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */
        if (PhysicalExamDataTemplateSaveAs.params.ParentCtrl == 'clinicalTabProcedures') {
            UnloadActionPan(PhysicalExamDataTemplateSaveAs.params["ParentCtrl"], "PhysicalExamDataTemplateSaveAs");
        } else {
            UnloadActionPan(PhysicalExamDataTemplateSaveAs.params.ParentCtrl, 'PhysicalExamDataTemplateSaveAs');
        }
        /* Start 22/12/2015 Muhammad Irfan for bug # EMR-144 */
    },
}