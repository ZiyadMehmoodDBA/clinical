EDIClaimStatusInsuranceDetail = {
    params: [],
    Load: function (params) {
        EDIClaimStatusInsuranceDetail.params = params;
        
        var self = $('#tblEDIClaimStatusInsuranceDetail');
        self.loadDropDowns(true).done(function () {

            EDIClaimStatusInsuranceDetail.LoadEDIClaimStatusInsurance();

        });
    },

    LoadEDIClaimStatusInsurance: function () {
        if (EDIClaimStatusInsuranceDetail.params.mode == "Add") {

            //serialize data
            $('#frmEDIClaimStatusInsuranceDetail').data('serialize', $('#frmEDIClaimStatusInsuranceDetail').serialize());
            EDIClaimStatusInsuranceDetail.ValidateEDIClaimStatusInsurance();
        }
        else if (EDIClaimStatusInsuranceDetail.params.mode == "Edit") {
            EDIClaimStatusInsuranceDetail.FillEDIClaimStatusInsurance(EDIClaimStatusInsuranceDetail.params.EDIClaimStatusInsuranceId).done(function (response) {
                if (response.status != false) {
                    var EDIClaimStatusInsurance_detail = JSON.parse(response.EDIClaimStatusInsuranceFill_JSON);
                    var self = $("#EDIClaimStatusInsuranceDetail");
                   
                    utility.bindMyJSON(true, EDIClaimStatusInsurance_detail, false, self).done(function () {

                        if (EDIClaimStatusInsurance_detail.chkTaxID == 'True')
                            $("#EDIClaimStatusInsuranceDetail #chkTaxID").attr("checked", true);
                        else
                            $("#EDIClaimStatusInsuranceDetail #chkTaxID").attr("checked", false);
                        if (EDIClaimStatusInsurance_detail.chkActive == 'True')
                            $("#EDIClaimStatusInsuranceDetail #chkActive").attr("checked", true);
                        else
                            $("#EDIClaimStatusInsuranceDetail #chkActive").attr("checked", false);
                        EDIClaimStatusInsuranceDetail.ValidateEDIClaimStatusInsurance();

                        //serialize data
                        $('#frmEDIClaimStatusInsuranceDetail').data('serialize', $('#frmEDIClaimStatusInsuranceDetail').serialize());

                    });
                    
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateEDIClaimStatusInsurance: function () {
        $('#frmEDIClaimStatusInsuranceDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ClearingHouse: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ClaimStatusInsurance: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PayorId: {
                       group: '.col-sm-3',
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
            EDIClaimStatusInsuranceDetail.EDIClaimStatusInsuranceSave();
        });
    },

    EDIClaimStatusInsuranceSave: function () {
        var strMessage = "";
        var self = $("#EDIClaimStatusInsuranceDetail");
        var myJSON = self.getMyJSON();
        if (EDIClaimStatusInsuranceDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("EDI Claim Status Insurance", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDIClaimStatusInsuranceDetail.SaveEDIClaimStatusInsurance(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceSearch(response.EDIClaimStatusInsuranceId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetEDIClaimStatusInsurance', true);
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
        else if (EDIClaimStatusInsuranceDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("EDI Claim Status Insurance", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDIClaimStatusInsuranceDetail.UpdateEDIClaimStatusInsurance(myJSON, EDIClaimStatusInsuranceDetail.params.EDIClaimStatusInsuranceId).done(function (response) {
                        if (response.status != false) {
                            Admin_EDIClaimStatusInsurance.EDIClaimStatusInsuranceSearch(EDIClaimStatusInsuranceDetail.params.EDIClaimStatusInsuranceId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetEDIClaimStatusInsurance', true);
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

    SaveEDIClaimStatusInsurance: function (EDIClaimStatusInsuranceData) {
        var data = "EDIClaimStatusInsuranceData=" + EDIClaimStatusInsuranceData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_CLAIM_STATUS_INSURANCE_DETAIL", "SAVE_EDI_CLAIM_STATUS_INSURANCE");
    },

    UpdateEDIClaimStatusInsurance: function (EDIClaimStatusInsuranceData, EDIClaimStatusInsuranceID) {
        var data = "EDIClaimStatusInsuranceData=" + EDIClaimStatusInsuranceData + "&EDIClaimStatusInsuranceID=" + EDIClaimStatusInsuranceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_CLAIM_STATUS_INSURANCE_DETAIL", "UPDATE_EDI_CLAIM_STATUS_INSURANCE");
    },

    FillEDIClaimStatusInsurance: function (EDIClaimStatusInsuranceID) {
        var data = "EDIClaimStatusInsuranceID=" + EDIClaimStatusInsuranceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_CLAIM_STATUS_INSURANCE_DETAIL", "FILL_EDI_CLAIM_STATUS_INSURANCE");
    },

    UpdateEDIClaimStatusInsuranceActiveInactive: function (EDIClaimStatusInsuranceID, IsActive) {
        var data = "EDIClaimStatusInsuranceID=" + EDIClaimStatusInsuranceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_CLAIM_STATUS_INSURANCE_DETAIL", "UPDATE_EDI_CLAIM_STATUS_INSURANCE_ACTIVE_INACTIVE");
    },

    ShowHistory: function () {
        var PanelID = 'EDIClaimStatusInsuranceDetail';
        var ParentCtrl = 'EDIClaimStatusInsuranceDetail';
        var ProfileName = 'EDI Claim Status Insurance';
        var DBTableName = 'EDIClaimStatusInsurance';
        var ColumnKeyId = EDIClaimStatusInsuranceDetail.params.EDIClaimStatusInsuranceId

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
    UnLoad: function () {

        utility.UnLoadDialog("frmEDIClaimStatusInsuranceDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },

}