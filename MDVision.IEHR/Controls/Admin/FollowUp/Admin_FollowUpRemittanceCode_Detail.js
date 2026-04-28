followUpRemittanceCodeDetail = {
    params: [],
    Load: function (params) {
        followUpRemittanceCodeDetail.params = params;
        followUpRemittanceCodeDetail.LoadRemittanceCode();
    },

    LoadRemittanceCode: function () {
        if (followUpRemittanceCodeDetail.params.mode == "Add") {
            $('#followUpRemittanceCodeDetail #txtCode').attr("enabled", "enabled");
            followUpRemittanceCodeDetail.ValidateRemittanceCode();
            //serialize Data after all controls loaded.
            $('#frmfollowUpRemittanceCodeDetail').data('serialize', $('#frmfollowUpRemittanceCodeDetail').serialize());
        }
        else if (followUpRemittanceCodeDetail.params.mode == "Edit") {
            $('#followUpRemittanceCodeDetail #txtCode').attr("disabled", "disabled");
            followUpRemittanceCodeDetail.FillRemittanceCode(followUpRemittanceCodeDetail.params.RemittanceId).done(function (response) {
                if (response.status != false) {
                    var remittanceCode_detail = JSON.parse(response.RemittanceCodeFill_JSON);
                    var self = $("#followUpRemittanceCodeDetail");
                    utility.bindMyJSON(true, remittanceCode_detail, false, self).done(function () {

                        //if (RemittanceCode_detail.chkActive == 'True')
                        //    $("#followUpRemittanceCodeDetail #chkActive").attr("checked", true);
                        //else
                        //    $("#followUpRemittanceCodeDetail #chkActive").attr("checked", false);

                        //serialize Data after all controls loaded.
                        $('#frmfollowUpRemittanceCodeDetail').data('serialize', $('#frmfollowUpRemittanceCodeDetail').serialize());

                    });


                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            followUpRemittanceCodeDetail.ValidateRemittanceCode();
        }
    },

    ValidateRemittanceCode: function () {
        $('#frmfollowUpRemittanceCodeDetail')
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
            followUpRemittanceCodeDetail.RemittanceCodeSave();
        });
    },
    
    RemittanceCodeSave: function () {
        var strMessage = "";
        var self = $("#followUpRemittanceCodeDetail");
        var myJSON = self.getMyJSON();
        if (followUpRemittanceCodeDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Remittance Code", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    followUpRemittanceCodeDetail.SaveRemittanceCode(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_FollowUpRemittanceCode.RemittanceCodeSearch(response.RemittanceId);
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
            });
        }
        else if (followUpRemittanceCodeDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Remittance Code", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    followUpRemittanceCodeDetail.UpdateRemittanceCode(myJSON, followUpRemittanceCodeDetail.params.RemittanceId).done(function (response) {
                        if (response.status != false) {
                            Admin_FollowUpRemittanceCode.RemittanceCodeSearch(followUpRemittanceCodeDetail.params.RemittanceId);
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
            });
        }
    },

    SaveRemittanceCode: function (RemittanceCodeData) {
        var data = "RemittanceCodeData=" + RemittanceCodeData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_REMITTANCECODE", "SAVE_REMITTANCECODE");
    },

    UpdateRemittanceCode: function (RemittanceCodeData, RemittanceId) {
        var data = "RemittanceCodeData=" + RemittanceCodeData + "&RemittanceId=" + RemittanceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_REMITTANCECODE", "UPDATE_REMITTANCECODE");
    },

    FillRemittanceCode: function (RemittanceId) {
        var data = "RemittanceId=" + RemittanceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_REMITTANCECODE", "FILL_REMITTANCECODE");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmfollowUpRemittanceCodeDetail', function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ShowHistory: function () {
        var PanelID = 'followUpRemittanceCodeDetail';
        var ParentCtrl = 'followUpRemittanceCodeDetail';
        var ProfileName = 'Remittance Code';
        var DBTableName = 'RemittanceCode';
        var ColumnKeyId = followUpRemittanceCodeDetail.params.RemittanceId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}