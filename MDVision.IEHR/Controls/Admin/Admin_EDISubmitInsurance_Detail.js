EDISubmitInsuranceDetail = {
    params: [],
    Load: function (params) {
        EDISubmitInsuranceDetail.params = params;
       
        var self = $('#tblEDISubmitInsuranceDetail');
        self.loadDropDowns(true).done(function () {

            EDISubmitInsuranceDetail.LoadEDISubmitInsurance();
        });
    },

    LoadEDISubmitInsurance: function () {
        if (EDISubmitInsuranceDetail.params.mode == "Add") {

            //serialize data
            $('#frmEDISubmitInsuranceDetail').data('serialize', $('#frmEDISubmitInsuranceDetail').serialize());
            EDISubmitInsuranceDetail.ValidateEDISubmitInsurance();
        }
        else if (EDISubmitInsuranceDetail.params.mode == "Edit") {
            EDISubmitInsuranceDetail.FillEDISubmitInsurance(EDISubmitInsuranceDetail.params.EDISubmitInsuranceId).done(function (response) {
                if (response.status != false) {
                    var EDISubmitInsurance_detail = JSON.parse(response.EDISubmitInsuranceFill_JSON);
                    var self = $("#EDISubmitInsuranceDetail");
                    
                    utility.bindMyJSON(true, EDISubmitInsurance_detail, false, self).done(function () {

                        if (EDISubmitInsurance_detail.chkAdmissionDateRequired == 'True')
                            $("#EDISubmitInsuranceDetail #chkAdmissionDateRequired").attr("checked", true);
                        else
                            $("#EDISubmitInsuranceDetail #chkAdmissionDateRequired").attr("checked", false);
                        if (EDISubmitInsurance_detail.chkAnesthesiaByMinutes == 'True')
                            $("#EDISubmitInsuranceDetail #chkAnesthesiaByMinutes").attr("checked", true);
                        else
                            $("#EDISubmitInsuranceDetail #chkAnesthesiaByMinutes").attr("checked", false);
                        if (EDISubmitInsurance_detail.chkActive == 'True')
                            $("#EDISubmitInsuranceDetail #chkActive").attr("checked", true);
                        else
                            $("#EDISubmitInsuranceDetail #chkActive").attr("checked", false);
                        EDISubmitInsuranceDetail.ValidateEDISubmitInsurance();


                        //serialize data
                        $('#frmEDISubmitInsuranceDetail').data('serialize', $('#frmEDISubmitInsuranceDetail').serialize());

                    });
                  
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateEDISubmitInsurance: function () {
        $('#frmEDISubmitInsuranceDetail')
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
                   SubmitInsName: {
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
            EDISubmitInsuranceDetail.EDISubmitInsuranceSave();
        });
    },

    EDISubmitInsuranceSave: function () {
        var strMessage = "";
        var self = $("#EDISubmitInsuranceDetail");
        var myJSON = self.getMyJSON();
        if (EDISubmitInsuranceDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("EDI Submit Insurance", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDISubmitInsuranceDetail.SaveEDISubmitInsurance(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_EDISubmitInsurance.EDISubmitInsuranceSearch(response.EDISubmitInsuranceId);
                            utility.DisplayMessages(response.message, 1);
                            EDISubmitInsuranceDetail.UnLoad();
                            CacheManager.BindCodes('GetEDISubmitInsurance', true);
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
        else if (EDISubmitInsuranceDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("EDI Submit Insurance", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    EDISubmitInsuranceDetail.UpdateEDISubmitInsurance(myJSON, EDISubmitInsuranceDetail.params.EDISubmitInsuranceId).done(function (response) {
                        if (response.status != false) {
                            if (EDISubmitInsuranceDetail.params["ParentCtrl"] != 'insurancePlanDetail')
                                Admin_EDISubmitInsurance.EDISubmitInsuranceSearch(EDISubmitInsuranceDetail.params.EDISubmitInsuranceId);

                            utility.DisplayMessages(response.message, 1);
                            EDISubmitInsuranceDetail.UnLoad();
                            CacheManager.BindCodes('GetEDISubmitInsurance', true);
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

    SaveEDISubmitInsurance: function (EDISubmitInsuranceData) {
        var data = "EDISubmitInsuranceData=" + EDISubmitInsuranceData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SUBMIT_INSURANCE_DETAIL", "SAVE_EDI_SUBMIT_INSURANCE");
    },

    UpdateEDISubmitInsurance: function (EDISubmitInsuranceData, EDISubmitInsuranceID) {
        var data = "EDISubmitInsuranceData=" + EDISubmitInsuranceData + "&EDISubmitInsuranceID=" + EDISubmitInsuranceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SUBMIT_INSURANCE_DETAIL", "UPDATE_EDI_SUBMIT_INSURANCE");
    },

    FillEDISubmitInsurance: function (EDISubmitInsuranceID) {
        var data = "EDISubmitInsuranceID=" + EDISubmitInsuranceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SUBMIT_INSURANCE_DETAIL", "FILL_EDI_SUBMIT_INSURANCE");
    },

    UpdateEDISubmitInsuranceActiveInactive: function (EDISubmitInsuranceID, IsActive) {
        var data = "EDISubmitInsuranceID=" + EDISubmitInsuranceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_SUBMIT_INSURANCE_DETAIL", "UPDATE_EDI_SUBMIT_INSURANCE_ACTIVE_INACTIVE");
    },

    //UnLoad: function () {

    //    utility.UnLoadDialog("frmEDISubmitInsuranceDetail", function () {
    //        UnloadActionPan();
    //    }, function () {
    //        UnloadActionPan();
    //    });

    //},
    UnLoad: function () {
        if (EDISubmitInsuranceDetail.params["FromAdmin"] == "0" || EDISubmitInsuranceDetail.params["RefCtrl"] == "lnkInsuranceEDI") {


            if (EDISubmitInsuranceDetail.params != null && EDISubmitInsuranceDetail.params.ParentCtrl != null && EDISubmitInsuranceDetail.params.PanelID != 'EDISubmitInsuranceDetail') {
                UnloadActionPan(EDISubmitInsuranceDetail.params.ParentCtrl, 'EDISubmitInsuranceDetail', null, EDISubmitInsuranceDetail.params.PanelID);
            }

            else if (EDISubmitInsuranceDetail.params != null && EDISubmitInsuranceDetail.params.ParentCtrl != null) {
                UnloadActionPan(EDISubmitInsuranceDetail.params.ParentCtrl, 'EDISubmitInsuranceDetail');
            }

            else
                UnloadActionPan(null, 'EDISubmitInsuranceDetail');
        }
        else {
            RemoveAdminTab();
        }
    },
    ShowHistory: function () {
        var PanelID = 'EDISubmitInsuranceDetail';
        var ParentCtrl = 'EDISubmitInsuranceDetail';
        var ProfileName = 'EDI Submit Insurance';
        var DBTableName = 'EDISubmitInsurance';
        var ColumnKeyId = EDISubmitInsuranceDetail.params.EDISubmitInsuranceId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

}