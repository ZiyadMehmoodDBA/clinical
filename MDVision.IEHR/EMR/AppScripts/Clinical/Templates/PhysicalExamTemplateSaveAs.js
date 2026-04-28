PhysicalExamTemplateSaveAs = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {
        PhysicalExamTemplateSaveAs.params = params;

        PhysicalExamTemplateSaveAs.params.PanelID = 'pnlClinicalPhysicalExamTemplateSaveAs';
        PhysicalExamTemplateSaveAs.validatePhysicalExamTemplateSaveAs();
    },


    validatePhysicalExamTemplateSaveAs: function () {
        $('#' + PhysicalExamTemplateSaveAs.params.PanelID + " #frmClinicalPhysicalExamTemplateSaveAs")
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   TemplateNameSaveAs: {
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
                    case 'btnTemplateSaveAs':
                        var newTemplateName = $('#' + PhysicalExamTemplateSaveAs.params.PanelID + " #txtTemplateNameSaveAs").val();
                        //PhysicalExamTemplateDetail.params["mode"] = "ADD";

                        PhysicalExamTemplateDetail.savePhysicalExamTemplate(null, newTemplateName).done(function (response) {

                            PhysicalExamTemplateSaveAs.UnLoad();
                            response = JSON.parse(response);
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);                              
                               
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                      
                        break;

                }

            }
            e.type = "";
        });

    },

    UnLoad: function () {
        if (PhysicalExamTemplateSaveAs.params.ParentCtrl == 'clinicalTabProcedures') {
            UnloadActionPan(PhysicalExamTemplateSaveAs.params["ParentCtrl"], "PhysicalExamTemplateSaveAs");
        } else {
            UnloadActionPan(PhysicalExamTemplateSaveAs.params.ParentCtrl, 'PhysicalExamTemplateSaveAs');
        }
    },
}