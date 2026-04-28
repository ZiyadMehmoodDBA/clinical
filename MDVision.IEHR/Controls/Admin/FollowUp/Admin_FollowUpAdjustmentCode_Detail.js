followUpAdjustmentCodeDetail = {
    params: [],
    Load: function (params) {
        followUpAdjustmentCodeDetail.params = params;
        followUpAdjustmentCodeDetail.LoadAdjustmentCode();
    },

    LoadAdjustmentCode: function () {
        if (followUpAdjustmentCodeDetail.params.mode == "Add") {
            $('#followUpAdjustmentCodeDetail #txtCode').attr("enabled", "enabled");
            followUpAdjustmentCodeDetail.ValidateAdjustmentCode();
            //serialize Data after all controls loaded.
            $('#frmfollowUpAdjustmentCodeDetail').data('serialize', $('#frmfollowUpAdjustmentCodeDetail').serialize());
        }
        else if (followUpAdjustmentCodeDetail.params.mode == "Edit") {
            $('#followUpAdjustmentCodeDetail #txtCode').attr("disabled", "disabled");
            followUpAdjustmentCodeDetail.FillAdjustmentCode(followUpAdjustmentCodeDetail.params.AdjustmentId).done(function (response) {
                if (response.status != false) {
                    var AdjustmentCode_detail = JSON.parse(response.AdjustmentCodeFill_JSON);
                    var self = $("#followUpAdjustmentCodeDetail");
                    utility.bindMyJSON(true, AdjustmentCode_detail, false, self).done(function () {

                        if (AdjustmentCode_detail.CreatedBy == "MDVISION")
                            $('#followUpAdjustmentCodeDetail #chkActive').attr("disabled", "disabled");

                        //if (AdjustmentCode_detail.chkActive == 'True')
                        //    $("#followUpAdjustmentCodeDetail #chkActive").attr("checked", true);
                        //else
                        //    $("#followUpAdjustmentCodeDetail #chkActive").attr("checked", false);

                        //serialize Data after all controls loaded.
                        $('#frmfollowUpAdjustmentCodeDetail').data('serialize', $('#frmfollowUpAdjustmentCodeDetail').serialize());

                    });


                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            followUpAdjustmentCodeDetail.ValidateAdjustmentCode();
        }
    },

    ValidateAdjustmentCode: function () {
        $('#frmfollowUpAdjustmentCodeDetail')
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
            followUpAdjustmentCodeDetail.AdjustmentCodeSave();
        });
    },
    
    AdjustmentCodeSave: function () {
        var strMessage = "";
        var self = $("#followUpAdjustmentCodeDetail");
        var myJSON = self.getMyJSON();
        if (followUpAdjustmentCodeDetail.params.mode == "Add") {
            //AppPrivileges.GetFormPrivileges("Appointment Status", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    followUpAdjustmentCodeDetail.SaveAdjustmentCode(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_FollowUpAdjustmentCode.AdjustmentCodeSearch(response.AdjustmentId);
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
        else if (followUpAdjustmentCodeDetail.params.mode == "Edit") {
           // AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    followUpAdjustmentCodeDetail.UpdateAdjustmentCode(myJSON, followUpAdjustmentCodeDetail.params.AdjustmentId).done(function (response) {
                        if (response.status != false) {
                            Admin_FollowUpAdjustmentCode.AdjustmentCodeSearch(followUpAdjustmentCodeDetail.params.AdjustmentId);
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

    SaveAdjustmentCode: function (AdjustmentCodeData) {
        var data = "AdjustmentCodeData=" + AdjustmentCodeData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ADJUSTMENTCODE", "SAVE_ADJUSTMENTCODE");
    },

    UpdateAdjustmentCode: function (AdjustmentCodeData, AdjustmentId) {
        var data = "AdjustmentCodeData=" + AdjustmentCodeData + "&AdjustmentId=" + AdjustmentId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ADJUSTMENTCODE", "UPDATE_ADJUSTMENTCODE");
    },

    FillAdjustmentCode: function (AdjustmentId) {
        var data = "AdjustmentId=" + AdjustmentId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ADJUSTMENTCODE", "FILL_ADJUSTMENTCODE");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmfollowUpAdjustmentCodeDetail', function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ShowHistory: function () {
        var PanelID = 'followUpAdjustmentCodeDetail';
        var ParentCtrl = 'followUpAdjustmentCodeDetail';
        var ProfileName = 'FollowUp Adjustment Code';
        var DBTableName = 'AdjustmentReasonCode';
        var ColumnKeyId = followUpAdjustmentCodeDetail.params.AdjustmentId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}