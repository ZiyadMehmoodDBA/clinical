

Admin_ERAAdjustmentCodesDetail = {
    params: [],
    bIsFirstLoad: true,
    Enable: false,
    Load: function (params) {
        Admin_ERAAdjustmentCodesDetail.params = params;
        if (Admin_ERAAdjustmentCodesDetail.bIsFirstLoad) {
            if (Admin_ERAAdjustmentCodesDetail.params["PanelID"] != 'Admin_ERAAdjustmentCodesDetail') {
                Admin_ERAAdjustmentCodesDetail.params["PanelID"] = Admin_ERAAdjustmentCodesDetail.params["PanelID"] + " #Admin_ERAAdjustmentCodesDetail";
            }
            var self = $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"]);
            self.loadDropDowns(true).done(function () {
                Admin_ERAAdjustmentCodesDetail.LoadERAAdjustmentCodes();            
            });
            $(document).ready(function () {
                $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"]+' #ERAActionId').change(function () {
                    Admin_ERAAdjustmentCodesDetail.FillLedgerAccount(this, null, null)
                });
                
            });
        }

    },

    LoadERAAdjustmentCodes: function () {
        if (Admin_ERAAdjustmentCodesDetail.params.mode == "Add") {
           
            //Serialize data
            $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + ' #frmAdmin_ERAAdjustmentCodesDetail').data('serialize', $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + ' #frmAdmin_ERAAdjustmentCodesDetail').serialize());
            Admin_ERAAdjustmentCodesDetail.ValidationERAAdjustmentCodes();
        }
        else if (Admin_ERAAdjustmentCodesDetail.params.mode == "Edit") {

         
            Admin_ERAAdjustmentCodesDetail.FillERAAdjustmentCodes(Admin_ERAAdjustmentCodesDetail.params.ERAAdjCodeId).done(function (response) {
                if (response.status != false) {
                    var ERAAdjustmentCodes_detail = JSON.parse(response.ERAAdjustmentCodeFill_JSON);
                    var self = $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"]);
                    utility.bindMyJSON(true, ERAAdjustmentCodes_detail, false, self).done(function () {

                        if (ERAAdjustmentCodes_detail.IsActive == 'True') {
                            $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"]+" #IsActive").attr("checked", true);
                        }
                        else {
                            $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + " #IsActive").attr("checked", false);
                        }
                        $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + " #ERAActionId option").each(function () {
                            if ($(this).val().split('_')[0] == ERAAdjustmentCodes_detail.ERAActionId) {
                                $(this).prop("selected", true);
                                Admin_ERAAdjustmentCodesDetail.FillLedgerAccount(this, ERAAdjustmentCodes_detail.LedgerAccountId, $(this).val().split('_')[1]);
                            }
                        })
                        
                        Admin_ERAAdjustmentCodesDetail.ValidationERAAdjustmentCodes();

                        Admin_ERAAdjustmentCodesDetail.DisableControl('Clearinghouse', self.find('#PracticeId'));
                        Admin_ERAAdjustmentCodesDetail.DisableControl('Clearinghouse', self.find('#ClearingHouseId'));
                        //Serialize data
                        $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + ' #frmAdmin_ERAAdjustmentCodesDetail').data('serialize', $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + ' #frmAdmin_ERAAdjustmentCodesDetail').serialize());

                    });


                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ERAAdjustmentCodesSave: function () {
        var strMessage = "";
        var self = $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"]);
        var myJSON = self.getMyJSON();
        if (Admin_ERAAdjustmentCodesDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Ledger Account", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_ERAAdjustmentCodesDetail.SaveERAAdjustmentCodes(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_ERAAdjustmentCodes.ERAAdjustmentCodesSearch('0');
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
        else if (Admin_ERAAdjustmentCodesDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Ledger Account", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_ERAAdjustmentCodesDetail.UpdateERAAdjustmentCodes(myJSON, Admin_ERAAdjustmentCodesDetail.params.ERAAdjCodeId).done(function (response) {
                        if (response.status != false) {
                            Admin_ERAAdjustmentCodes.ERAAdjustmentCodesSearch('0');
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

    ValidationERAAdjustmentCodes: function () {
     
        $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + ' #frmAdmin_ERAAdjustmentCodesDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  ClaimAdjReasonCodeID: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ClaimAdjuGroupCodeID: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ERAActionId: {
                      group: '.col-sm-3',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  LedgerAccountId: {
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
           Admin_ERAAdjustmentCodesDetail.ERAAdjustmentCodesSave();
       });
    },

    SaveERAAdjustmentCodes: function (ERAAdjustmentCodesData) {
        var data = "ERAAdjustmentCodesData=" + ERAAdjustmentCodesData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ERAADJUSTMENTCODES_DETAIL", "SAVE_ERAADJUSTMENT_CODES");
    },

    UpdateERAAdjustmentCodes: function (ERAAdjustmentCodesData, ERAAdjustmentCodesID) {
        var data = "ERAAdjustmentCodesData=" + ERAAdjustmentCodesData + "&ERAAdjustmentCodesID=" + ERAAdjustmentCodesID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ERAADJUSTMENTCODES_DETAIL", "UPDATE_ERAADJUSTMENT_CODES");
    },

    FillERAAdjustmentCodes: function (ERAAdjustmentCodesID) {
        var data = "ERAAdjustmentCodesID=" + ERAAdjustmentCodesID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ERAADJUSTMENTCODES_DETAIL", "FILL_ERAADJUSTMENT_CODES");
    },

    UpdateERAAdjustmentCodesActiveInactive: function (ERAAdjustmentCodesID, IsActive) {
        var data = "ERAAdjustmentCodesID=" + ERAAdjustmentCodesID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_ERAADJUSTMENTCODES_DETAIL", "UPDATE_ERAADJUSTMENT_CODES_ACTIVE_INACTIVE");
    },

    FillLedgerAccount: function (ActionTypeID, LedgerAccountId, ActionTypeValue) {
        if (ActionTypeValue==null) {
            ActionTypeValue = $('#' + ActionTypeID.id).val().split('_')[1];
        }
        
        if (ActionTypeValue != "") {
            CacheManager.BindDropDownsByTwoIDs('#' + Admin_ERAAdjustmentCodesDetail.params.PanelID + ' #LedgerAccountId', 'GetLedgerAccount', true, Number(ActionTypeValue), -1).done(function () {
                if (LedgerAccountId!=null) {
                    $('#' + Admin_ERAAdjustmentCodesDetail.params.PanelID + ' #LedgerAccountId').val(LedgerAccountId);
                }
            });
            $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + " #LedgerAccountId").removeAttr("disabled");
            $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + " #lblLedgerAccount_req").show();
        } else {
           

            $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + " #LedgerAccountId").val("");
            $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + " #lblLedgerAccount_req").hide();
            $('#' + Admin_ERAAdjustmentCodesDetail.params["PanelID"] + " #LedgerAccountId").attr("disabled", "disabled");
        }
        
    },

    UnLoad: function () {

        utility.UnLoadDialog(Admin_ERAAdjustmentCodesDetail.params["PanelID"] + " #frmAdmin_ERAAdjustmentCodesDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    DisableControl: function (ControlName,Ctrl) {
        if (ControlName == "Clearinghouse")
        {
            if ($(Ctrl).val() == "")
            {
                $('#' + Admin_ERAAdjustmentCodesDetail.params.PanelID + ' #PracticeId').removeAttr('disabled');
            }
            else {
                $('#' + Admin_ERAAdjustmentCodesDetail.params.PanelID + ' #PracticeId').attr('disabled','disabled');
            }
        }
        else if (ControlName == "Practice")
        {
            if ($(Ctrl).val() == "") {
                $('#' + Admin_ERAAdjustmentCodesDetail.params.PanelID + ' #ClearingHouseId').removeAttr('disabled');
            }
            else {
                $('#' + Admin_ERAAdjustmentCodesDetail.params.PanelID + ' #ClearingHouseId').attr('disabled', 'disabled');
            }
        }
    },

    ShowHistory: function () {
        var PanelID = 'Admin_ERAAdjustmentCodesDetail';
        var ParentCtrl = 'Admin_ERAAdjustmentCodesDetail';
        var ProfileName = 'ERA Adjustment Codes';
        var DBTableName = 'ERAAdjustmentCode';
        var ColumnKeyId = Admin_ERAAdjustmentCodesDetail.params.ERAAdjCodeId

    utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
},
}