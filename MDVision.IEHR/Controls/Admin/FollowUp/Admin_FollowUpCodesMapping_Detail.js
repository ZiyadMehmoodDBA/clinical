followUpCodesMappingDetail = {
    params: [],
    Load: function (params) {
        followUpCodesMappingDetail.params = params;
        

        var self = $('#followUpCodesMappingDetail');
        self.loadDropDowns(true).done(function () {
            followUpCodesMappingDetail.LoadCodesMapping();
        });
    },

    LoadCodesMapping: function () {
        if (followUpCodesMappingDetail.params.mode == "Add") {
            followUpCodesMappingDetail.ValidateCodesMapping();
            //serialize Data after all controls loaded.
            $('#frmfollowUpCodesMappingDetail').data('serialize', $('#frmfollowUpCodesMappingDetail').serialize());
        }
        else if (followUpCodesMappingDetail.params.mode == "Edit") {
            followUpCodesMappingDetail.FillCodesMapping(followUpCodesMappingDetail.params.CodesMappingId).done(function (response) {
                if (response.status != false) {
                    var codesMapping_detail = JSON.parse(response.CodesMappingFill_JSON);
                    var self = $("#followUpCodesMappingDetail");
                    utility.bindMyJSON(true, codesMapping_detail, false, self).done(function () {

                        $('#frmfollowUpCodesMappingDetail').data('serialize', $('#frmfollowUpCodesMappingDetail').serialize());

                    });


                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            followUpCodesMappingDetail.ValidateCodesMapping();
        }
    },

    ValidateCodesMapping: function () {
        $('#frmfollowUpCodesMappingDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ClaimStatusCode: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ClaimStatusCategoryCode: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Action: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Reason: {
                       group: '.col-sm-4',
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
            followUpCodesMappingDetail.CodesMappingSave();
        });
    },

    CodesMappingSave: function () {
        var strMessage = "";
        var self = $("#followUpCodesMappingDetail");
        var myJSON = self.getMyJSON();
        if (followUpCodesMappingDetail.params.mode == "Add") {
            //AppPrivileges.GetFormPrivileges("Appointment Status", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                followUpCodesMappingDetail.SaveCodesMapping(myJSON).done(function (response) {
                    if (response.status != false) {
                        Admin_FollowUpCodesMapping.CodesMappingSearch(response.CodesMappingId);
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
        else if (followUpCodesMappingDetail.params.mode == "Edit") {
            // AppPrivileges.GetFormPrivileges("Appointment Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                followUpCodesMappingDetail.UpdateCodesMapping(myJSON, followUpCodesMappingDetail.params.CodesMappingId).done(function (response) {
                    if (response.status != false) {
                        Admin_FollowUpCodesMapping.CodesMappingSearch(followUpCodesMappingDetail.params.CodesMappingId);
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

    SaveCodesMapping: function (CodesMappingData) {
        var data = "CodesMappingData=" + CodesMappingData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CODESMAPPING", "SAVE_CODESMAPPING");
    },

    UpdateCodesMapping: function (CodesMappingData, CodesMappingId) {
        var data = "CodesMappingData=" + CodesMappingData + "&CodesMappingId=" + CodesMappingId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CODESMAPPING", "UPDATE_CODESMAPPING");
    },

    FillCodesMapping: function (CodesMappingId) {
        var data = "CodesMappingId=" + CodesMappingId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_CODESMAPPING", "FILL_CODESMAPPING");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmfollowUpCodesMappingDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

        
    },

    ShowHistory: function () {
        var PanelID = 'followUpCodesMappingDetail';
        var ParentCtrl = 'followUpCodesMappingDetail';
        var ProfileName = 'FollowUp Codes Mapping';
        var DBTableName = 'CodesMapping';
        var ColumnKeyId = followUpCodesMappingDetail.params.CodesMappingId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}