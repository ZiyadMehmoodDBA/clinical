Clinical_VitalsComments = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_VitalsComments.params = params;
        //serialize Data.
        $('#frmVitalsComments').data('serialize', $('#frmVitalsComments').serialize());
        Clinical_VitalsComments.ValidateVitals();
    },

    ValidateVitals: function () {
        $('#' + Clinical_VitalsComments.params.PanelID + ' #frmVitalsComments').bootstrapValidator('destroy');
        $('#' + Clinical_VitalsComments.params.PanelID + ' #frmVitalsComments')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Comments: {
                       group: '.col-sm-12',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Clinical_VitalsComments.SaveComments();
        });
    },
    SaveComments: function () {
        Clinical_VitalsComments.CommentsSave().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var patientId = Clinical_Vitals.params.patientID == null ? $('#PatientProfile #hfPatientId').val() : Clinical_Vitals.params.patientID;
                $.when(ClinicalCDSDetail.showCDSAlert("", patientId)).then(function () {
                    if (Clinical_Vitals.params.ParentCtrl == "clinicalTabProgressNote")
                        Clinical_ProgressNote.LoadCDSAlerts();
                });
                $('#frmVitalsComments').data('serialize', $('#frmVitalsComments').serialize());
                Clinical_Vitals.ResetFormData();
                Clinical_Vitals.VitalsSearch();
                Clinical_VitalsComments.UnLoad();
                utility.DisplayMessages("Deleted Successfully", 1);

            }
            else {
                $('#frmVitalsComments').data('serialize', $('#frmVitalsComments').serialize());
                Clinical_Vitals.VitalsSearch();
                Clinical_VitalsComments.UnLoad();
                utility.DisplayMessages(response.message, 3);
            }
        });
    },

    CommentsSave: function () {

        var vitalSignId = Clinical_VitalsComments.params.VitalSignsId;
        var patientId = Clinical_VitalsComments.params.PatientId;
        var comments = $('#Clinical_VitalsComments #txtComments').val();

        var objData = new Object();
        objData["VitalSignsId"] = vitalSignId;
        objData["PatientId"] = patientId;
        objData["DeleteComments"] = comments;
        objData["commandType"] = "UPDATE_VITALS_ACTIVEINACTIVE";

        var data = JSON.stringify(objData);

        //var data = "VitalSignsData=" + VitalSignsData;
        // serach parameter , class name, command name of class 
        return MDVisionService.APIService(data, "MEDICAL", "Vitals");

    },

    UnLoad: function () {

        utility.UnLoadDialog('frmVitalsComments', function () {
            UnloadActionPan(Clinical_VitalsComments.params["ParentCtrl"], "Clinical_VitalsComments", null, Clinical_VitalsComments.params.PanelID);
        }, function () {
            UnloadActionPan(Clinical_VitalsComments.params["ParentCtrl"], "Clinical_VitalsComments", null, Clinical_VitalsComments.params.PanelID);
        });

    },
}