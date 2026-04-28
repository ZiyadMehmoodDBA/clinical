Patient_Information_Delete = {
    params: [],

    Load: function (params) {
        Patient_Information_Delete.params = params;
        if (Patient_Information_Delete.params.PanelID != "pnlPatientInformationDelete")
            Patient_Information_Delete.params.PanelID = Patient_Information_Delete.params.PanelID + " #pnlPatientInformationDelete";
        else
            Patient_Information_Delete.params.PanelID = "pnlPatientInformationDelete";

        Patient_Information_Delete.validateInformationSubmission();

    },

    validateInformationSubmission: function () {
        var $self = $('#' + Patient_Information_Delete.params.PanelID + " #frmPatientInformationSubmissionDelete");
        $self.bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                Reason: {
                    group: '.col-sm-12',
                    validators: {
                        notEmpty: {
                            message: ''
                        }
                    }
                },
            }

        }).on('success.form.bv', function (e) {
            e.preventDefault();
            Patient_Information_Delete.DeleteInformationSubmission();
        });

    },

    DeleteInformationSubmission: function () {
        var self = $("#" + Patient_Information_Delete.params.PanelID + " #frmPatientInformationSubmissionDelete");
        var myJSON = self.getMyJSONByName();
        Patient_Information_Delete.DeleteInformationSubmission_DBCall(myJSON).done(function (response) {
            if (response.status != false) {
                Patient_Information_Submission.InformationSearch();
                utility.DisplayMessages(response.Message, 1);
                Patient_Information_Delete.UnLoad();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    DeleteInformationSubmission_DBCall: function (Data) {
        var data = "Data=" + Data + "&Id=" + Patient_Information_Delete.params.Id;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DELETE_PATIENT_INFORMATION_SUBMISSION");
    },

    UnLoad: function () {
        if (Patient_Information_Delete.params && Patient_Information_Delete.params.ParentCtrl) {
            UnloadActionPan(Patient_Information_Delete.params.ParentCtrl, 'Patient_Information_Delete');
        }
        else
            UnloadActionPan(null, 'Patient_Information_Delete');
    },

}