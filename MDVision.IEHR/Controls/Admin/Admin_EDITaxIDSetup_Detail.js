editaxidsetupDetail = {
    params: [],
    Enable: false,
    Load: function (params) {
        editaxidsetupDetail.params = params;
        
        var self = null;
        if (editaxidsetupDetail.params.PanelID == "editaxidsetupDetail")
            self = $('#editaxidsetupDetail');
        else
            self = $('#' + editaxidsetupDetail.params.PanelID + ' #editaxidsetupDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            $("#editaxidsetupDetail #chkIsActive").attr("checked", true);
            editaxidsetupDetail.LoadEDITaxIDSetup();
        });
        
    },


    LoadEDITaxIDSetup: function () {
        if (editaxidsetupDetail.params.mode == "Add") {

            //serialize data
            $('#frmEDITaxIDSetupDetail').data('serialize', $('#frmEDITaxIDSetupDetail').serialize());
            editaxidsetupDetail.ValidationEDITaxIDSetupId();
        }
        else if (editaxidsetupDetail.params.mode == "Edit") {
            editaxidsetupDetail.FillEDITaxIDSetup(editaxidsetupDetail.params.EDITaxIDSetupId).done(function (response) {
                if (response.status != false) {
                    var editaxidsetup_detail = JSON.parse(response.EDITaxIDSetupFill_JSON);
                    var self = $("#editaxidsetupDetail");
                    utility.bindMyJSON(true, editaxidsetup_detail, false, self).done(function () {

                        if (editaxidsetup_detail.chkActive == 'True') {
                            $("#editaxidsetupDetail #chkActive").attr("checked", true);
                        }
                        else {
                            $("#editaxidsetupDetail #chkActive").attr("checked", false);
                        }
                        editaxidsetupDetail.ValidationEDITaxIDSetupId();

                        //serialize data
                        $('#frmEDITaxIDSetupDetail').data('serialize', $('#frmEDITaxIDSetupDetail').serialize());

                    });
                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    EDITaxIDSetupSave: function () {
        var strMessage = "";
        var self = $("#editaxidsetupDetail");
        var myJSON = self.getMyJSON();
        if (editaxidsetupDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("EDI Tax ID Setup", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    editaxidsetupDetail.SaveEDITaxIDSetup(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_EDITaxIDSetup.EDITaxIDSetupSearch(response.EDITaxIDSetupId);
                            utility.DisplayMessages(response.message, 1);
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
        else if (editaxidsetupDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("EDI Tax ID Setup", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    editaxidsetupDetail.UpdateEDITaxIDSetup(myJSON, editaxidsetupDetail.params.EDITaxIDSetupId).done(function (response) {
                        if (response.status != false) {
                            Admin_EDITaxIDSetup.EDITaxIDSetupSearch(editaxidsetupDetail.params.EDITaxIDSetupId);

                            utility.DisplayMessages(response.message, 1);
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

    ValidationEDITaxIDSetupId: function () {
        $('#frmEDITaxIDSetupDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  TaxID: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ClearingHouse: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Entity: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Submitter: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Receiver: {
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           editaxidsetupDetail.EDITaxIDSetupSave();
       });
    },

    SaveEDITaxIDSetup: function (EDITaxIDSetupData) {
        var data = "EDITaxIDSetupData=" + EDITaxIDSetupData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_TAX_ID_SETUP_DETAIL", "SAVE_EDI_TAX_ID_SETUP");
    },

    UpdateEDITaxIDSetup: function (EDITaxIDSetupData, EDITaxIDSetupID) {
        var data = "EDITaxIDSetupData=" + EDITaxIDSetupData + "&EDITaxIDSetupID=" + EDITaxIDSetupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_TAX_ID_SETUP_DETAIL", "UPDATE_EDI_TAX_ID_SETUP");
    },

    FillEDITaxIDSetup: function (EDITaxIDSetupID) {
        var data = "EDITaxIDSetupID=" + EDITaxIDSetupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_TAX_ID_SETUP_DETAIL", "FILL_EDI_TAX_ID_SETUP");
    },

    UpdateEDITaxIDSetupActiveInactive: function (EDITaxIDSetupID, IsActive) {
        var data = "EDITaxIDSetupID=" + EDITaxIDSetupID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_EDI_TAX_ID_SETUP_DETAIL", "UPDATE_EDI_TAX_ID_SETUP_ACTIVE_INACTIVE");
    },

    LoadEntityBasedData: function (entityID) {

        if (entityID != "") {
            CacheManager.BindDropDownsByEntityID('#' + editaxidsetupDetail.params["PanelID"] + ' #ddlClearingHouse', 'GetClearingHouse', false, entityID);
            CacheManager.BindDropDownsByEntityID('#' + editaxidsetupDetail.params["PanelID"] + ' #ddlSubmittterSetup', 'GetSubmitterSetup', false, entityID);
            CacheManager.BindDropDownsByEntityID('#' + editaxidsetupDetail.params["PanelID"] + ' #ddlReceiverSetup', 'GetEDIReceiverSetup', false, entityID);
        }
        else {
            CacheManager.BindDropDownsByEntityID('#' + editaxidsetupDetail.params["PanelID"] + ' #ddlClearingHouse', 'GetClearingHouse', true, entityID);
            CacheManager.BindDropDownsByEntityID('#' + editaxidsetupDetail.params["PanelID"] + ' #ddlSubmittterSetup', 'GetSubmitterSetup', true, entityID);
            CacheManager.BindDropDownsByEntityID('#' + editaxidsetupDetail.params["PanelID"] + ' #ddlReceiverSetup', 'GetEDIReceiverSetup', true, entityID);

        }

    },

    UnLoad: function () {


        utility.UnLoadDialog("frmEDITaxIDSetupDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },
    ShowHistory: function () {
        var PanelID = 'editaxidsetupDetail';
        var ParentCtrl = 'editaxidsetupDetail';
        var ProfileName = 'EDI Tax ID Setup';
        var DBTableName = 'EDITaxIDSetup';
        var ColumnKeyId = editaxidsetupDetail.params.EDITaxIDSetupId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

}