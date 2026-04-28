followUpClaimStatusCodeDetail = {
    params: [],
    Load: function (params) {
        followUpClaimStatusCodeDetail.params = params;
        followUpClaimStatusCodeDetail.LoadClaimStatusCode();
    },

    LoadClaimStatusCode: function () {
        if (followUpClaimStatusCodeDetail.params.mode == "Add") {
            $('#followUpClaimStatusCodeDetail #txtCode').attr("enabled", "enabled");
            followUpClaimStatusCodeDetail.ValidateClaimStatusCode();
            //serialize Data after all controls loaded.
            $('#frmfollowUpClaimStatusCodeDetail').data('serialize', $('#frmfollowUpClaimStatusCodeDetail').serialize());
        }
        else if (followUpClaimStatusCodeDetail.params.mode == "Edit") {
            $('#followUpClaimStatusCodeDetail #txtCode').attr("disabled", "disabled");
            followUpClaimStatusCodeDetail.FillClaimStatusCode(followUpClaimStatusCodeDetail.params.CSCodeId).done(function (response) {
                if (response.status != false) {
                    var remittanceCode_detail = JSON.parse(response.ClaimStatusCodeFill_JSON);
                    var self = $("#followUpClaimStatusCodeDetail");
                    utility.bindMyJSON(true, remittanceCode_detail, false, self).done(function () {
                        if (remittanceCode_detail.CreatedBy == "MDVISION")
                            $('#followUpClaimStatusCodeDetail #chkActive').attr("disabled", "disabled");

                        //if (ClaimStatusCode_detail.chkActive == 'True')
                        //    $("#followUpClaimStatusCodeDetail #chkActive").attr("checked", true);
                        //else
                        //    $("#followUpClaimStatusCodeDetail #chkActive").attr("checked", false);

                        //serialize Data after all controls loaded.
                        $('#frmfollowUpClaimStatusCodeDetail').data('serialize', $('#frmfollowUpClaimStatusCodeDetail').serialize());

                    });


                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            followUpClaimStatusCodeDetail.ValidateClaimStatusCode();
        }
    },

    ValidateClaimStatusCode: function () {
        $('#frmfollowUpClaimStatusCodeDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Code: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Description: {
                       group: '.col-sm-6',
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
            followUpClaimStatusCodeDetail.ClaimStatusCodeSave();
        });
    },

    ClaimStatusCodeSave: function () {
        var strMessage = "";
        var self = $("#followUpClaimStatusCodeDetail");
        var myJSON = self.getMyJSON();
        if (followUpClaimStatusCodeDetail.params.mode == "Add") {
            //AppPrivileges.GetFormPrivileges("Appointment Status", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                followUpClaimStatusCodeDetail.SaveClaimStatusCode(myJSON).done(function (response) {
                    if (response.status != false) {
                        Admin_FollowUpClaimStatusCode.ClaimStatusCodeSearch(response.CSCodeId);
                        utility.DisplayMessages(response.Message, 1);
                        //CacheManager.BindCodes('GetAppointmentStatus', true);
                        //MDVisionService.lookups("GetAppointmentStatus", true);

                        UnloadActionPan();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            //});
        }
        else if (followUpClaimStatusCodeDetail.params.mode == "Edit") {
            // AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                followUpClaimStatusCodeDetail.UpdateClaimStatusCode(myJSON, followUpClaimStatusCodeDetail.params.CSCodeId).done(function (response) {
                    if (response.status != false) {
                        Admin_FollowUpClaimStatusCode.ClaimStatusCodeSearch(followUpClaimStatusCodeDetail.params.CSCodeId);
                        utility.DisplayMessages(response.Message, 1);
                        //CacheManager.BindCodes('GetAppointmentStatus', true);
                        //MDVisionService.lookups("GetAppointmentStatus", true);

                        UnloadActionPan();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            //});
        }
    },

    SaveClaimStatusCode: function (ClaimStatusCodeData) {
        var data = "ClaimStatusCodeData=" + ClaimStatusCodeData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCODE", "SAVE_CLAIMSTATUSCODE");
    },

    UpdateClaimStatusCode: function (ClaimStatusCodeData, CSCodeId) {
        var data = "ClaimStatusCodeData=" + ClaimStatusCodeData + "&CSCodeId=" + CSCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCODE", "UPDATE_CLAIMSTATUSCODE");
    },

    FillClaimStatusCode: function (CSCodeId) {
        var data = "CSCodeId=" + CSCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCODE", "FILL_CLAIMSTATUSCODE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmfollowUpClaimStatusCodeDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
        
    },

    ShowHistory: function () {
        var PanelID = 'followUpClaimStatusCodeDetail';
        var ParentCtrl = 'followUpClaimStatusCodeDetail';
        var ProfileName = 'FollowUp Claim Status Code';
        var DBTableName = 'ClaimStatusCode';
        var ColumnKeyId = followUpClaimStatusCodeDetail.params.CSCodeId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}