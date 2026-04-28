followUpClaimStatusCategoryCodeDetail = {
    params: [],
    Load: function (params) {
        followUpClaimStatusCategoryCodeDetail.params = params;
        followUpClaimStatusCategoryCodeDetail.LoadClaimStatusCategoryCode();
    },

    LoadClaimStatusCategoryCode: function () {
        if (followUpClaimStatusCategoryCodeDetail.params.mode == "Add") {
            $('#followUpClaimStatusCategoryCodeDetail #txtCode').attr("enabled", "enabled");
            followUpClaimStatusCategoryCodeDetail.ValidateClaimStatusCategoryCode();
            //serialize Data after all controls loaded.
            $('#frmfollowUpClaimStatusCategoryCodeDetail').data('serialize', $('#frmfollowUpClaimStatusCategoryCodeDetail').serialize());
        }
        else if (followUpClaimStatusCategoryCodeDetail.params.mode == "Edit") {
            $('#followUpClaimStatusCategoryCodeDetail #txtCode').attr("disabled", "disabled");
            followUpClaimStatusCategoryCodeDetail.FillClaimStatusCategoryCode(followUpClaimStatusCategoryCodeDetail.params.CSCatCodeId).done(function (response) {
                if (response.status != false) {
                    var remittanceCode_detail = JSON.parse(response.ClaimStatusCategoryCodeFill_JSON);
                    var self = $("#followUpClaimStatusCategoryCodeDetail");
                    utility.bindMyJSON(true, remittanceCode_detail, false, self).done(function () {

                        if (remittanceCode_detail.CreatedBy == "MDVISION")
                            $('#followUpClaimStatusCategoryCodeDetail #chkActive').attr("disabled", "disabled");

                        //if (ClaimStatusCategoryCode_detail.chkActive == 'True')
                        //    $("#followUpClaimStatusCategoryCodeDetail #chkActive").attr("checked", true);
                        //else
                        //    $("#followUpClaimStatusCategoryCodeDetail #chkActive").attr("checked", false);

                        //serialize Data after all controls loaded.
                        $('#frmfollowUpClaimStatusCategoryCodeDetail').data('serialize', $('#frmfollowUpClaimStatusCategoryCodeDetail').serialize());

                    });


                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            followUpClaimStatusCategoryCodeDetail.ValidateClaimStatusCategoryCode();
        }
    },

    ValidateClaimStatusCategoryCode: function () {
        $('#frmfollowUpClaimStatusCategoryCodeDetail')
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
            followUpClaimStatusCategoryCodeDetail.ClaimStatusCategoryCodeSave();
        });
    },

    ClaimStatusCategoryCodeSave: function () {
        var strMessage = "";
        var self = $("#followUpClaimStatusCategoryCodeDetail");
        var myJSON = self.getMyJSON();
        if (followUpClaimStatusCategoryCodeDetail.params.mode == "Add") {
            //AppPrivileges.GetFormPrivileges("Appointment Status", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                followUpClaimStatusCategoryCodeDetail.SaveClaimStatusCategoryCode(myJSON).done(function (response) {
                    if (response.status != false) {
                        Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeSearch(response.CSCatCodeId);
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
        else if (followUpClaimStatusCategoryCodeDetail.params.mode == "Edit") {
            // AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                followUpClaimStatusCategoryCodeDetail.UpdateClaimStatusCategoryCode(myJSON, followUpClaimStatusCategoryCodeDetail.params.CSCatCodeId).done(function (response) {
                    if (response.status != false) {
                        Admin_FollowUpClaimStatusCategoryCode.ClaimStatusCategoryCodeSearch(followUpClaimStatusCategoryCodeDetail.params.CSCatCodeId);
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

    SaveClaimStatusCategoryCode: function (ClaimStatusCategoryCodeData) {
        var data = "ClaimStatusCategoryCodeData=" + ClaimStatusCategoryCodeData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCATCODE", "SAVE_CLAIMSTATUSCATCODE");
    },

    UpdateClaimStatusCategoryCode: function (ClaimStatusCategoryCodeData, CSCatCodeId) {
        var data = "ClaimStatusCategoryCodeData=" + ClaimStatusCategoryCodeData + "&CSCatCodeId=" + CSCatCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCATCODE", "UPDATE_CLAIMSTATUSCATCODE");
    },

    FillClaimStatusCategoryCode: function (CSCatCodeId) {
        var data = "CSCatCodeId=" + CSCatCodeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CLAIMSTATUSCATCODE", "FILL_CLAIMSTATUSCATCODE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmfollowUpClaimStatusCategoryCodeDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ShowHistory: function () {
        var PanelID = 'followUpClaimStatusCategoryCodeDetail';
        var ParentCtrl = 'followUpClaimStatusCategoryCodeDetail';
        var ProfileName = 'FollowUp Claim Status Category Code';
        var DBTableName = 'ClaimStatusCategoryCode';
        var ColumnKeyId = followUpClaimStatusCategoryCodeDetail.params.CSCatCodeId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}