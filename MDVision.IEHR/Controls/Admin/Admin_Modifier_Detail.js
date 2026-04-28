modifierDetail = {
    params: [],
    Load: function (params) {
        modifierDetail.params = params;
        modifierDetail.LoadModifier();
    },

    LoadModifier: function () {
        if (modifierDetail.params.mode == "Add") {
            $('#frmModifierDetail').data('serialize', $('#frmModifierDetail').serialize());
            modifierDetail.ValidateModifier();
        }
        else if (modifierDetail.params.mode == "Edit") {
            $('#modifierDetail #txtModifierCode').attr("disabled", "disabled");
            modifierDetail.FillModifier(modifierDetail.params.ModifierId).done(function (response) {
                if (response.status != false) {
                    var modifier_detail = JSON.parse(response.ModifierFill_JSON);
                    var self = $("#modifierDetail");
                   
                    utility.bindMyJSON(true, modifier_detail, false, self);
                    if (modifier_detail.chkActive == 'True')
                        $("#modifierDetail #chkActive").attr("checked", true);
                    else
                        $("#modifierDetail #chkActive").attr("checked", false);
                    modifierDetail.ValidateModifier();
                    $('#frmModifierDetail').data('serialize', $('#frmModifierDetail').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateModifier: function () {
        $('#frmModifierDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   ModifierCode: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Description: {
                       group: '.col-sm-8',
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
            modifierDetail.ModifierSave();
        });
    },

    ModifierSave: function () {
        var strMessage = "";
        var self = $("#modifierDetail");
        var myJSON = self.getMyJSON();
        if (modifierDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Modifier", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    modifierDetail.SaveModifier(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_Modifier.ModifierSearch(response.ModifierId);
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
        else if (modifierDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Modifier", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    modifierDetail.UpdateModifier(myJSON, modifierDetail.params.ModifierId).done(function (response) {
                        if (response.status != false) {
                            Admin_Modifier.ModifierSearch(modifierDetail.params.ModifierId);
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

    SaveModifier: function (ModifierData) {
        var data = "ModifierData=" + ModifierData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_MODIFIER_DETAIL", "SAVE_MODIFIER");
    },

    UpdateModifier: function (ModifierData, ModifierID) {
        var data = "ModifierData=" + ModifierData + "&ModifierID=" + ModifierID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_MODIFIER_DETAIL", "UPDATE_MODIFIER");
    },

    FillModifier: function (ModifierID) {
        var data = "ModifierID=" + ModifierID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_MODIFIER_DETAIL", "FILL_MODIFIER");
    },

    UpdateModifierActiveInactive: function (ModifierID, IsActive) {
        var data = "ModifierID=" + ModifierID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_MODIFIER_DETAIL", "UPDATE_MODIFIER_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmModifierDetail", function () {
            UnloadActionPan(modifierDetail.params["ParentCtrl"], "modifierDetail");
        }, function () {
            UnloadActionPan(modifierDetail.params["ParentCtrl"], "modifierDetail");
        });

    },

    ShowHistory: function () {
        var PanelID = 'modifierDetail';
        var ParentCtrl = 'modifierDetail';
        var ProfileName = 'Modifier';
        var DBTableName = 'Modifier';
        var ColumnKeyId = modifierDetail.params.ModifierId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}