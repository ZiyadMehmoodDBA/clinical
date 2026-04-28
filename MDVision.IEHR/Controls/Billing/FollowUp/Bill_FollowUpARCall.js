Bill_FollowUpARCall = {
    bIsFirstLoad: true,
    params: [],
    CallPrivilege: "",
    //bVisitFirst: true,
    Load: function (params) {
        Bill_FollowUpARCall.params = params;
        if (Bill_FollowUpARCall.params.PanelID != "pnlBillFollowUpARCall")
            Bill_FollowUpARCall.params.PanelID = Bill_FollowUpARCall.params.PanelID + ' #pnlBillFollowUpARCall';
        if (Bill_FollowUpARCall.params["FollowUpARType"] == "insurance") {
            Bill_FollowUpARCall.CallPrivilege = "FollowUp Insurance AR";
        } else if (Bill_FollowUpARCall.params["FollowUpARType"] == "patient") {
            Bill_FollowUpARCall.CallPrivilege = "FollowUp Patient AR";
        }
        if (Bill_FollowUpARCall.bIsFirstLoad) {
            Bill_FollowUpARCall.bIsFirstLoad = false;

            Bill_FollowUpARCall.ValidateFollowUpARCall();
            $('#' + Bill_FollowUpARCall.params.PanelID).loadDropDowns(true).done(function () {
                $('#' + Bill_FollowUpARCall.params.PanelID + ' #frmBillFollowUpARCall').data('serialize', $('#' + Bill_FollowUpARCall.params.PanelID + ' #frmBillFollowUpARCall').serialize());
            });
        }

    },


    ValidateFollowUpARCall: function () {
        $('#frmBillFollowUpARCall')
                 .bootstrapValidator({
                     live: 'disabled',
                     message: 'This value is not valid',
                     feedbackIcons: {
                         valid: 'glyphicon glyphicon-ok',
                         invalid: 'glyphicon glyphicon-remove',
                         validating: 'glyphicon glyphicon-refresh'
                     },
                     fields: {
                         callType: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         action: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },

                         reason: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },

                         status: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },

                         duration: {
                             group: '.col-sm-3',
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
                 Bill_FollowUpARCall.FollowUpARCallSave();
             });
    },


    FollowUpARCallSave: function () {

        var self = $("#" + Bill_FollowUpARCall.params.PanelID);
        var myJSON = self.getMyJSON();

        var strMessage = "";
        AppPrivileges.GetFormPrivileges(Bill_FollowUpARCall.CallPrivilege, "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Bill_FollowUpARCall.SaveFollowUpARCall(myJSON).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        $('#frmBillFollowUpARCall').data('serialize', $('#frmBillFollowUpARCall').serialize());

                        if (Bill_FollowUpARCall.params.FollowUpARType == "insurance") {

                            Bill_FollowUpInsuranceAR_Detail.appendCallComments("Duration " + $("#" + Bill_FollowUpARCall.params.PanelID + " #frmBillFollowUpARCall #txtDuration").val() + " Min " + $("#" + Bill_FollowUpARCall.params.PanelID + " #frmBillFollowUpARCall #txtComments").val());
                        }
                        else if (Bill_FollowUpARCall.params.FollowUpARType == "patient") {

                            Bill_FollowUpPatientAR_Detail.appendCallComments("Duration " + $("#" + Bill_FollowUpARCall.params.PanelID + " #frmBillFollowUpARCall #txtDuration").val() + " Min " + $("#" + Bill_FollowUpARCall.params.PanelID + " #frmBillFollowUpARCall #txtComments").val());
                        }

                        Bill_FollowUpARCall.UnLoad();


                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },


    SaveFollowUpARCall: function (FollowUpARCallData) {

        var data = "FollowUpARCallData=" + FollowUpARCallData + "&FollowUpARID=" + Bill_FollowUpARCall.params.FollowUpARID + "&FollowUpARType=" + Bill_FollowUpARCall.params.FollowUpARType;
        // save parameter , class name, command name of class 

        return MDVisionService.defaultService(data, "BILLING_FOLLOWUP_AR_CALL", "SAVE_FOLLOWUP_AR_CALL");
    },



    UnLoad: function () {

        utility.UnLoadDialog(Bill_FollowUpARCall.params.PanelID + ' #frmBillFollowUpARCall', function () {
            UnloadActionPan(Bill_FollowUpARCall.params["ParentCtrl"], "Bill_FollowUpARCall");
        }, function () {
            UnloadActionPan(Bill_FollowUpARCall.params["ParentCtrl"], "Bill_FollowUpARCall");
        });

    },
}