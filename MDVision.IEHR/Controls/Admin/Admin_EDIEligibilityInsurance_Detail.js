EDIEligibilityInsuranceDetail = {
    params: [],
    Load: function (params) {
        EDIEligibilityInsuranceDetail.params = params;
        
        var self = $('#tblEDIEligibilityInsuranceDetail');
        self.loadDropDowns(true).done(function () {

            EDIEligibilityInsuranceDetail.LoadEDIEligibilityInsurance();
        });
    },

    LoadEDIEligibilityInsurance: function () {
        if (EDIEligibilityInsuranceDetail.params.mode == "Add") {

            //serialize data
            $('#frmEDIEligibilityInsuranceDetail').data('serialize', $('#frmEDIEligibilityInsuranceDetail').serialize());
            EDIEligibilityInsuranceDetail.ValidateEDIEligibilityInsurance();
        }
        else if (EDIEligibilityInsuranceDetail.params.mode == "Edit") {
            EDIEligibilityInsuranceDetail.FillEDIEligibilityInsurance(EDIEligibilityInsuranceDetail.params.EDIEligibilityInsuranceId).done(function (response) {
                if (response.status != false) {
                    var EDIEligibilityInsurance_detail = JSON.parse(response.EDIEligibilityInsuranceFill_JSON);
                    var self = $("#EDIEligibilityInsuranceDetail");
                    
                    utility.bindMyJSON(true, EDIEligibilityInsurance_detail, false, self).done(function () {

                        if (EDIEligibilityInsurance_detail.chkTaxID == 'True')
                            $("#EDIEligibilityInsuranceDetail #chkTaxID").attr("checked", true);
                        else
                            $("#EDIEligibilityInsuranceDetail #chkTaxID").attr("checked", false);
                        if (EDIEligibilityInsurance_detail.chkActive == 'True')
                            $("#EDIEligibilityInsuranceDetail #chkActive").attr("checked", true);
                        else
                            $("#EDIEligibilityInsuranceDetail #chkActive").attr("checked", false);
                        EDIEligibilityInsuranceDetail.ValidateEDIEligibilityInsurance();

                        //serialize data
                        $('#frmEDIEligibilityInsuranceDetail').data('serialize', $('#frmEDIEligibilityInsuranceDetail').serialize());

                    });
                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateEDIEligibilityInsurance: function () {
        $('#frmEDIEligibilityInsuranceDetail')
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
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   EligibilityInsurance: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PayorId: {
                       group: '.col-sm-4',
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
            EDIEligibilityInsuranceDetail.EDIEligibilityInsuranceSave();
        });
    },

    EDIEligibilityInsuranceSave: function () {
        var strMessage = "";
        var self = $("#EDIEligibilityInsuranceDetail");
        var myJSON = self.getMyJSON();
        if (EDIEligibilityInsuranceDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("EDI Eligibility Insurance", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDIEligibilityInsuranceDetail.SaveEDIEligibilityInsurance(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceSearch(response.EDIEligibilityInsuranceId);
                            utility.DisplayMessages(response.message, 1);
                            EDIEligibilityInsuranceDetail.UnLoad();
                            CacheManager.BindCodes('GetEDIEligibilityInsurance', true);
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
        else if (EDIEligibilityInsuranceDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("EDI Eligibility Insurance", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDIEligibilityInsuranceDetail.UpdateEDIEligibilityInsurance(myJSON, EDIEligibilityInsuranceDetail.params.EDIEligibilityInsuranceId).done(function (response) {
                        if (response.status != false) {
                            if (EDIEligibilityInsuranceDetail.params["ParentCtrl"] != 'insurancePlanDetail')
                                Admin_EDIEligibilityInsurance.EDIEligibilityInsuranceSearch(EDIEligibilityInsuranceDetail.params.EDIEligibilityInsuranceId);
                            utility.DisplayMessages(response.message, 1);
                            EDIEligibilityInsuranceDetail.UnLoad();
                            CacheManager.BindCodes('GetEDIEligibilityInsurance', true);
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

    SaveEDIEligibilityInsurance: function (EDIEligibilityInsuranceData) {
        var data = "EDIEligibilityInsuranceData=" + EDIEligibilityInsuranceData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_ELIGIBILITY_INSURANCE_DETAIL", "SAVE_EDI_ELIGIBILITY_INSURANCE");
    },

    UpdateEDIEligibilityInsurance: function (EDIEligibilityInsuranceData, EDIEligibilityInsuranceID) {
        var data = "EDIEligibilityInsuranceData=" + EDIEligibilityInsuranceData + "&EDIEligibilityInsuranceID=" + EDIEligibilityInsuranceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_ELIGIBILITY_INSURANCE_DETAIL", "UPDATE_EDI_ELIGIBILITY_INSURANCE");
    },

    FillEDIEligibilityInsurance: function (EDIEligibilityInsuranceID) {
        var data = "EDIEligibilityInsuranceID=" + EDIEligibilityInsuranceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_ELIGIBILITY_INSURANCE_DETAIL", "FILL_EDI_ELIGIBILITY_INSURANCE");
    },

    UpdateEDIEligibilityInsuranceActiveInactive: function (EDIEligibilityInsuranceID, IsActive) {
        var data = "EDIEligibilityInsuranceID=" + EDIEligibilityInsuranceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_ELIGIBILITY_INSURANCE_DETAIL", "UPDATE_EDI_ELIGIBILITY_INSURANCE_ACTIVE_INACTIVE");
    },

    //UnLoad: function () {


    //    utility.UnLoadDialog("frmEDIEligibilityInsuranceDetail", function () {
    //        UnloadActionPan();
    //    }, function () {
    //        UnloadActionPan();
    //    });

    //},
    UnLoad: function (Tab) {
        if (EDIEligibilityInsuranceDetail.params["FromAdmin"] == "0" || EDIEligibilityInsuranceDetail.params["RefCtrl"] == "lnkEligibilityInsuranceEDI") {


            if (EDIEligibilityInsuranceDetail.params != null && EDIEligibilityInsuranceDetail.params.ParentCtrl != null && EDIEligibilityInsuranceDetail.params.PanelID != 'EDIEligibilityInsuranceDetail') {
                UnloadActionPan(EDIEligibilityInsuranceDetail.params.ParentCtrl, 'EDIEligibilityInsuranceDetail', null, EDIEligibilityInsuranceDetail.params.PanelID);
            }

            else if (EDIEligibilityInsuranceDetail.params != null && EDIEligibilityInsuranceDetail.params.ParentCtrl != null) {
                UnloadActionPan(EDIEligibilityInsuranceDetail.params.ParentCtrl, 'EDIEligibilityInsuranceDetail');
            }

            else
                UnloadActionPan(null, 'EDIEligibilityInsuranceDetail');
        }
        else {
            RemoveAdminTab();
        }
    },
    ShowHistory: function () {
        var PanelID = 'EDIEligibilityInsuranceDetail';
        var ParentCtrl = 'EDIEligibilityInsuranceDetail';
        var ProfileName = 'EDI Eligibility Insurance';
        var DBTableName = 'EDIEligibilityInsurance';
        var ColumnKeyId = EDIEligibilityInsuranceDetail.params.EDIEligibilityInsuranceId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}