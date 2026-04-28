Patient_MessageReply = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Patient_MessageReply.params = params;

        if (Patient_MessageReply.bIsFirstLoad) {
            Patient_MessageReply.bIsFirstLoad = false;
            var self = $('#pnlPatientMessageReply');
            self.loadDropDowns(true).done(function () {
                // Set AssignedTo/Status from Message
                $("#pnlPatientMessageReply #ddlAssignedto").val(Patient_MessageReply.params.AssignedToId);
                $("#pnlPatientMessageReply #ddlStatus").val(Patient_MessageReply.params.StatusId);
                Patient_MessageAdd.ValidatePatientMessage('#pnlPatientMessageReply #frmPatientMessageReply', 'Reply');

                //serialize data
                $('#frmPatientMessageReply').data('serialize', $('#frmPatientMessageReply').serialize());
            });           
        }
    },

    SaveMessageReply: function () {
        var self = $('#pnlPatientMessageReply');
        var myJSON = self.getMyJSON();
        Patient_MessageReply.MessageReplySave(myJSON).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.message, 1);
                if (Patient_MessageReply.params.ParentCtrl == "Patient_MessageEdit") {
                    Patient_MessageEdit.FillMessage(Patient_MessageReply.params.MessageId);
                }
                else if (Patient_MessageReply.params.ParentCtrl == "patTabMessages") {
                    Patient_Message.SearchPatientMessage(Patient_MessageReply.params.MessageId);
                }
                //Patient_MessageReply.UnLoad();
                updateNotificationsCounts();
                UnloadActionPan(Patient_MessageReply.params.ParentCtrl, 'Patient_MessageReply');

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    MessageReplySave: function (MessageReplyData) {
        var data = "MessageReplyData=" + MessageReplyData + "&MessageID=" + Patient_MessageReply.params.MessageId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_MESSAGE", "SAVE_MESSAGE_REPLY");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmPatientMessageReply', function () {
            if (Patient_MessageReply.params != null && Patient_MessageReply.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageReply.params.ParentCtrl, 'Patient_MessageReply');
            }
            else
                UnloadActionPan(null, 'Patient_MessageReply');
        }, function () {
            if (Patient_MessageReply.params != null && Patient_MessageReply.params.ParentCtrl != null) {
                UnloadActionPan(Patient_MessageReply.params.ParentCtrl, 'Patient_MessageReply');
            }
            else
                UnloadActionPan(null, 'Patient_MessageReply');
        });
    },
}