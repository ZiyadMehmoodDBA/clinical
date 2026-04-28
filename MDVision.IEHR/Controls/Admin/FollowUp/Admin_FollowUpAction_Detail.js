FollowUp_Action_Detail = {
    params: null,
    bIsFirstLoad: true,
    Load: function (params) {
        FollowUp_Action_Detail.params = params;
        if (FollowUp_Action_Detail.params.PanelID != "pnlFollowUpActionDetail") {
            FollowUp_Action_Detail.params.PanelID = FollowUp_Action_Detail.params.PanelID + ' #pnlFollowUpActionDetail';
        }
        var self = $('#' + FollowUp_Action_Detail.params.PanelID).find('#ddlContainer');
        if (FollowUp_Action_Detail.bIsFirstLoad) {
            FollowUp_Action_Detail.bIsFirstLoad = false;
            CacheManager.BindDropDownsByID('#' + FollowUp_Action_Detail.params.PanelID + ' #ddlLetter', 'GetLetters', true);
            self.loadDropDowns(true).done(function () {
                FollowUp_Action_Detail.ValidateActionDetail();
                if (FollowUp_Action_Detail.params.mode == undefined) {
                    FollowUp_Action_Detail.params.mode = "add";
                }
                if (FollowUp_Action_Detail.params.mode.toLowerCase().toLowerCase() == "edit")
                    FollowUp_Action_Detail.ActionFill();
                else {

                    //serialize Data after all controls loaded.
                    $('#frmFollowUpActionDetail').data('serialize', $('#frmFollowUpActionDetail').serialize());
                }

            });

        }
    },

    SetLedgerAccount: function (obj, LedgerAccountId) {
        var Option = $(obj).find("option:selected");
        var self = $('#' + FollowUp_Action_Detail.params.PanelID);
        var ddlLedgerAccount = self.find("#ddlLedgerAccount");
        if (Option.text().toLowerCase() == "discount – patient" || Option.text().toLowerCase() == "write off – plan") {
            ddlLedgerAccount.removeAttr("disabled");
            if (Option.text().toLowerCase() == "discount – patient") {
                CacheManager.BindDropDownsByTwoIDs('#' + FollowUp_Action_Detail.params.PanelID + ' #ddlLedgerAccount', 'GetLedgerAccount', true, 4, 1).done(function () {
                    if (LedgerAccountId != null) {
                        $('#' + FollowUp_Action_Detail.params.PanelID + ' #ddlLedgerAccount option').each(function () {
                            if ($(this).val() == LedgerAccountId) {
                                $(this).attr("selected", "selected");
                            }
                            else {
                                $(this).removeAttr("selected");
                            }
                        });
                    }
                    

                });
            }
            else if (Option.text().toLowerCase() == "write off – plan") {
                CacheManager.BindDropDownsByTwoIDs('#' + FollowUp_Action_Detail.params.PanelID + ' #ddlLedgerAccount', 'GetLedgerAccount', true, 3, 2).done(function () {
                    if (LedgerAccountId != null) {
                        $('#' + FollowUp_Action_Detail.params.PanelID + ' #ddlLedgerAccount option').each(function () {
                            if ($(this).val() == LedgerAccountId) {
                                $(this).attr("selected", "selected");
                            }
                            else {
                                $(this).removeAttr("selected");
                            }
                        });
                    }
                    //$('#' + Bill_PaymentPosting.params.PanelID + ' #divInsurancePayment #ddlInsuranceWriteoff option').each(function () {
                    //    if ($(this).attr("RefValue") != null && $(this).attr("RefValue").toLowerCase() == "true") {
                    //        $(this).attr("selected", "selected");
                    //        $('#' + Bill_PaymentPosting.params.PanelID + ' #divInsurancePayment #ddlInsuranceWriteoff').trigger('change');
                    //    }
                    //    else {
                    //        $(this).removeAttr("selected");
                    //    }
                    //});

                });
            }


        }
        else {
            ddlLedgerAccount.empty();
            ddlLedgerAccount.attr("disabled", "disabled");
        }
        //self.find('#txtPlanPriority').val($(Option).attr("PlanPriority"));
        //var ProviderId = self.find('#hfProvider').val();
        //var ProviderId = ProviderId != "" ? parseInt(ProviderId) : 0;
    },

    ValidateActionDetail: function () {
        $('#frmFollowUpActionDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  shortName: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Description: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  suspendDays: {
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
           FollowUp_Action_Detail.ActionSave();
       });
    },
    ActionSave: function () {
        var strMessage = "";
        var self = $('#pnlFollowUpActionDetail')
        var myJSON = self.getMyJSON();
        if (FollowUp_Action_Detail.params.mode == undefined) {
            FollowUp_Action_Detail.params.mode = "add";
        }
        if (FollowUp_Action_Detail.params.mode.toLowerCase() == "add") {
            FollowUp_Action_Detail.SaveAction(myJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    //Admin_FollowUpReason.ReasonSearch();
                    FollowUp_Action_Detail.params.ActionId = response.ActionId;
                    $('#frmFollowUpActionDetail').data('serialize', $('#frmFollowUpActionDetail').serialize());
                    FollowUp_Action_Detail.Unload();
                    Admin_FollowUpAction.ActionSearch(response.ActionId, 1, 15);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        else if (FollowUp_Action_Detail.params.mode.toLowerCase() == "edit") {
            FollowUp_Action_Detail.EditAction(myJSON, FollowUp_Action_Detail.params.ActionId).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    //Admin_FollowUpReason.ReasonSearch();
                    $('#frmFollowUpActionDetail').data('serialize', $('#frmFollowUpActionDetail').serialize());
                    FollowUp_Action_Detail.Unload();
                    Admin_FollowUpAction.ActionSearch(FollowUp_Action_Detail.params.ActionId, 1, 15);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    EditAction: function (actionData, actionID) {
        var data = "actionData=" + actionData + "&actionID=" + actionID;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ACTION", "UPDATE_ACTION");
    },
    SaveAction: function (actionData) {
        var data = "actionData=" + actionData;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ACTION", "SAVE_ACTION");
    },
    FillAction: function (actionId) {
        var data = "actionId=" + actionId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUP_ACTION", "FILL_ACTION");
    },
    ActionFill: function () {
        FollowUp_Action_Detail.FillAction(FollowUp_Action_Detail.params.ActionId).done(function (response) {
            if (response.status != false) {

                var self = $('#' + FollowUp_Action_Detail.params["PanelID"]);
                utility.bindMyJSON(true, JSON.parse(response.ActionLoad_JSON), false, self).done(function () {
                    var LedgerAccount = JSON.parse(response.ActionLoad_JSON).ddlLedgerAccount;
                    
                    FollowUp_Action_Detail.SetLedgerAccount(self.find("#ddlAutoAction"), LedgerAccount);
                    
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

            //serialize Data after all controls loaded.
            $('#frmFollowUpActionDetail').data('serialize', $('#frmFollowUpActionDetail').serialize());
        });
    },
    Unload: function () {

        utility.UnLoadDialog("frmFollowUpActionDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },

    ShowHistory: function () {
        var PanelID = 'pnlFollowUpActionDetail';
        var ParentCtrl = 'FollowUp_Action_Detail';
        var ProfileName = 'FollowUp Action';
        var DBTableName = 'FollowupAction';
        var ColumnKeyId = FollowUp_Action_Detail.params.ActionId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}

