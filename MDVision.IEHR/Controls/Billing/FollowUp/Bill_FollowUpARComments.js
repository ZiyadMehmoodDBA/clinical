Bill_FollowUpARComments = {
    params: [],
    Load: function (params) {
        Bill_FollowUpARComments.params = params;
        Bill_FollowUpARComments.ValidateFollowUpARComments();
        if (Bill_FollowUpARComments.params.VisitId) {
            $('#pnlFollowUpARComments #frmFollowUpARComments #hfVisitId').val(Bill_FollowUpARComments.params.VisitId);
        }
        if (Bill_FollowUpARComments.params.IsFromClaim) {
            $('#pnlFollowUpARComments #frmFollowUpARComments #hfIsFromClaim').val(Bill_FollowUpARComments.params.IsFromClaim);
        }
        if (Bill_FollowUpARComments.params.FollowUpCommentId) {
            Bill_FollowUpARComments.SearchFollowUpComment(Bill_FollowUpARComments.params.FollowUpCommentId);
        }
    },
    SearchFollowUpComment:function(FollowUpCommentId){
        Bill_FollowUpARComments.FollowUpCommentsSearch(FollowUpCommentId,0).done(function (response) {
            if (response.status != false) {
                var obj = response.FollowUpCommentInfo;
                $(response.FollowUpCommentInfo).each(function (i, item) {
                    $('#pnlFollowUpARComments #frmFollowUpARComments #hfVisitId').val(item.VisitId);
                    $('#pnlFollowUpARComments #frmFollowUpARComments #hfIsFromClaim').val(item.IsFromClaim);
                    $('#pnlFollowUpARComments #frmFollowUpARComments #hfFollowUpCommentId').val(item.Id);
                    $('#pnlFollowUpARComments #frmFollowUpARComments #txtFollowUpArComments').val(item.followUpComments);
                });
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },
    DeleteFollowUpComments: function (FollowUpCommentId) {
        utility.myConfirm('Do you want to delete  this record?', function () {
            Bill_FollowUpARComments.FollowUpCommentsDelete(FollowUpCommentId).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 3);
                if (Bill_FollowUpARComments.params.ParentCtrl == "EncounterChargeCapture") {
                    EncounterChargeCapture.FollowUpCommentLoad(Bill_FollowUpARComments.params.VisitId);
                }
                else if (Bill_FollowUpARComments.params.ParentCtrl == "Bill_FollowUpInsuranceAR_Detail") {
                    Bill_FollowUpInsuranceAR_Detail.FollowUpCommentLoad(Bill_FollowUpARComments.params.VisitId);
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        }, function () { }, ' Confirm Delete');
    },
    ValidateFollowUpARComments: function () {
        $('#pnlFollowUpARComments #frmFollowUpARComments').bootstrapValidator({
            live: 'disabled',
            message: 'This value is not valid',
            feedbackIcons: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                txtFollowUpArComments: {
                    group: '.col-sm-12',
                    validators: {
                        notEmpty: {
                            message: ''
                        },
                    }
                },
            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            Bill_FollowUpARComments.Save();
        });
    },

    Save:function()
    {
       
        var self = $('#pnlFollowUpARComments');
        var myJSON = self.getMyJSONByName();
        if (Bill_FollowUpARComments.params.mode == "Add") {
            Bill_FollowUpARComments.SaveFollowUpComments(myJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Bill_FollowUpARComments.UnLoad();
                    if (Bill_FollowUpARComments.params.ParentCtrl == "EncounterChargeCapture") {
                        EncounterChargeCapture.FollowUpCommentLoad(Bill_FollowUpARComments.params.VisitId);
                    }
                    else if (Bill_FollowUpARComments.params.ParentCtrl == "Bill_FollowUpInsuranceAR_Detail") {
                        Bill_FollowUpInsuranceAR_Detail.FollowUpCommentLoad(Bill_FollowUpARComments.params.VisitId);
                    }
                }
            });
        }
        else {
            Bill_FollowUpARComments.UpdateFollowUpComments(myJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    Bill_FollowUpARComments.UnLoad();
                    if (Bill_FollowUpARComments.params.ParentCtrl == "EncounterChargeCapture") {
                        EncounterChargeCapture.FollowUpCommentLoad(Bill_FollowUpARComments.params.VisitId);
                    }
                    else if (Bill_FollowUpARComments.params.ParentCtrl == "Bill_FollowUpInsuranceAR_Detail") {
                        Bill_FollowUpInsuranceAR_Detail.FollowUpCommentLoad(Bill_FollowUpARComments.params.VisitId);
                    }
                }
            });
        }

    },
    SaveFollowUpComments: function (myJSONData) {
        
        var data = "Comment_Data=" + myJSONData;
        return MDVisionService.defaultService(data, "BILLING_FOLLOWUP_COMMENTS", "SAVE_FOLLOWUP_AR_COMMENTS");
    },
    UpdateFollowUpComments: function (myJSONData) {

        var data = "Comment_Data=" + myJSONData;
        return MDVisionService.defaultService(data, "BILLING_FOLLOWUP_COMMENTS", "UPDATE_FOLLOWUP_AR_COMMENTS");
    },
    FollowUpCommentsDelete: function (FollowUpCommentId) {

        var data = "FollowUpCommentId=" + FollowUpCommentId;
        return MDVisionService.defaultService(data, "BILLING_FOLLOWUP_COMMENTS", "DELETE_FOLLOWUP_AR_COMMENTS");
    },
    FollowUpCommentsSearch: function (FollowUpCommentId,VisitId) {

        var data = "FollowUpCommentId=" + FollowUpCommentId + "&VisitId=" + VisitId;
        return MDVisionService.defaultService(data, "BILLING_FOLLOWUP_COMMENTS", "GET_FOLLOWUP_AR_COMMENTS");
    },
    
    UnLoad: function () {
        if (Bill_FollowUpARComments.params.ParentCtrl != "") {
            UnloadActionPan(Bill_FollowUpARComments.params.ParentCtrl);
        }
        else {
            UnloadActionPan();
        }
    },
}