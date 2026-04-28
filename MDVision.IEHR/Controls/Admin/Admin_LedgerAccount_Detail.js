ledgeraccountDetail = {
    params: [],
    Enable: false,
    Load: function (params) {
        ledgeraccountDetail.params = params;
        var self = null;
        if (ledgeraccountDetail.params.PanelID == 'tblledgeraccountDetail')
            self = $('#tblledgeraccountDetail');
        else
            self = $('#' + ledgeraccountDetail.params.PanelID + ' #tblledgeraccountDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {          
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#tblledgeraccountDetail #divLedgerAccount_Entity").css("display", "none");
            //    $("#tblledgeraccountDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }

            $("#ledgeraccountDetail #chkIsActive").attr("checked", true);
            ledgeraccountDetail.LoadLedgerAccount();
        });
               
    },

    LoadLedgerAccount: function () {
        if (ledgeraccountDetail.params.mode == "Add") {
            $('#ledgeraccountDetail #txtShortName').attr("enabled", "enabled");

            ledgeraccountDetail.ValidationLedgerAccount();
            //Serialize data
            $('#frmLedgerAccountDetail').data('serialize', $('#frmLedgerAccountDetail').serialize());
        }
        else if (ledgeraccountDetail.params.mode == "Edit") {

            $('#ledgeraccountDetail #txtShortName').attr("disabled", "disabled");
            $('#ledgeraccountDetail #ddlEntity').attr("disabled", "disabled");
            $('#ledgeraccountDetail #lstType').attr("disabled", "disabled");
            $('#ledgeraccountDetail #lstApplyTo').attr("disabled", "disabled");
            $('#ledgeraccountDetail #lstSystemCategory').attr("disabled", "disabled");
            ledgeraccountDetail.FillLedgerAccount(ledgeraccountDetail.params.LedgerAccountId).done(function (response) {
                if (response.status != false) {
                    var ledgeraccount_detail = JSON.parse(response.LedgerAccountFill_JSON);
                    var ledger_detail = JSON.parse(response.LedgerAccountLoad_JSON);
                    var self = $("#ledgeraccountDetail");

                    if (ledger_detail[0].IsSystem == 'True') {
                        $("#ledgeraccountDetail #frmLedgerAccountDetail :input").attr("disabled", true);
                        $("#ledgeraccountDetail #btnLedgerSave").hide();
                    }
                    else {
                    }

                    utility.bindMyJSON(true, ledgeraccount_detail, false, self).done(function () {

                        
                        if (ledgeraccount_detail.ChkIsActive == 'True') {
                            $("#ledgeraccountDetail #chkIsActive").attr("checked", true);
                        }
                        else {
                            $("#ledgeraccountDetail #chkIsActive").attr("checked", false);
                        }
                        ledgeraccountDetail.ValidationLedgerAccount();

                        //Serialize data
                        $('#frmLedgerAccountDetail').data('serialize', $('#frmLedgerAccountDetail').serialize());

                    });

                    
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    LedgerAccountSave: function () {
        var strMessage = "";
        var self = $("#ledgeraccountDetail");
        var myJSON = self.getMyJSON();
        if (ledgeraccountDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Ledger Account", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ledgeraccountDetail.SaveLedgerAccount(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_LedgerAccount.LedgerAccountSearch(response.LedgerAccountId);
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
        else if (ledgeraccountDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Ledger Account", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    ledgeraccountDetail.UpdateLedgerAccount(myJSON, ledgeraccountDetail.params.LedgerAccountId).done(function (response) {
                        if (response.status != false) {
                            Admin_LedgerAccount.LedgerAccountSearch(ledgeraccountDetail.params.LedgerAccountId);
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

    ValidationLedgerAccount: function () {
        $('#frmLedgerAccountDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  ShortName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Entity: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Type: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ApplyTo: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  }
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           ledgeraccountDetail.LedgerAccountSave();
       });
    },

    SaveLedgerAccount: function (LedgerAccountData) {
        var data = "LedgerAccountData=" + LedgerAccountData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_LEDGER_ACCOUNT_DETAIL", "SAVE_LEDGER_ACCOUNT");
    },

    UpdateLedgerAccount: function (LedgerAccountData, LedgerAccountID) {
        var data = "LedgerAccountData=" + LedgerAccountData + "&LedgerAccountID=" + LedgerAccountID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_LEDGER_ACCOUNT_DETAIL", "UPDATE_LEDGER_ACCOUNT");
    },

    FillLedgerAccount: function (LedgerAccountID) {
        var data = "LedgerAccountID=" + LedgerAccountID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_LEDGER_ACCOUNT_DETAIL", "FILL_LEDGER_ACCOUNT");
    },

    UpdateLedgerAccountActiveInactive: function (LedgerAccountID, IsActive) {
        var data = "LedgerAccountID=" + LedgerAccountID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_LEDGER_ACCOUNT_DETAIL", "UPDATE_LEDGER_ACCOUNT_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmLedgerAccountDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },
    ShowHistory: function () {
        var PanelID = 'ledgeraccountDetail';
        var ParentCtrl = 'ledgeraccountDetail';
        var ProfileName = 'Ledger Account';
        var DBTableName = 'LedgerAccount';
        var ColumnKeyId = ledgeraccountDetail.params.LedgerAccountId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}