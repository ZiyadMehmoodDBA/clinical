procedurecategoryDetail = {
    params: [],
    Enable: false,
    Load: function (params) {

        procedurecategoryDetail.params = params;
        var self = $('#tblprocedurecategoryDetail');
        $("#procedurecategoryDetail #chkIsActive").attr("checked", true);
        procedurecategoryDetail.LoadProcedureCategory();
    },

    LoadProcedureCategory: function () {
        if (procedurecategoryDetail.params.mode == "Add") {

            $('#frmProcedureCategoryDetail').data('serialize', $('#frmProcedureCategoryDetail').serialize());
            procedurecategoryDetail.ValidationProcedureCategory();
        }
        else if (procedurecategoryDetail.params.mode == "Edit") {
            $('#procedurecategoryDetail #txtName').attr("disabled", "disabled");
            procedurecategoryDetail.FillProcedureCategory(procedurecategoryDetail.params.ProcedureCategoryId).done(function (response) {
                if (response.status != false) {
                    var procedurecategory_detail = JSON.parse(response.ProcedureCategoryFill_JSON);
                    var self = $("#procedurecategoryDetail");
                    
                    utility.bindMyJSON(true, procedurecategory_detail, false, self).done(function () {

                        if (procedurecategory_detail.ChkIsActive == 'True') {
                            $("#procedurecategoryDetail #chkIsActive").attr("checked", true);
                        }
                        else {
                            $("#procedurecategoryDetail #chkIsActive").attr("checked", false);
                        }

                        procedurecategoryDetail.ValidationProcedureCategory();
                        $('#frmProcedureCategoryDetail').data('serialize', $('#frmProcedureCategoryDetail').serialize());

                    });

                    
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ProcedureCategorySave: function () {
        var strMessage = "";
        var self = $("#procedurecategoryDetail");
        var myJSON = self.getMyJSON();
        if (procedurecategoryDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Procedure Category", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    procedurecategoryDetail.SaveProcedureCategory(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_ProcedureCategory.ProcedureCategorySearch(response.ProcedureCategoryId);
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
        else if (procedurecategoryDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Procedure Category", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    procedurecategoryDetail.UpdateProcedureCategory(myJSON, procedurecategoryDetail.params.ProcedureCategoryId).done(function (response) {
                        if (response.status != false) {
                            Admin_ProcedureCategory.ProcedureCategorySearch(procedurecategoryDetail.params.ProcedureCategoryId);
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

    ValidationProcedureCategory: function () {
        $('#frmProcedureCategoryDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  Name: {
                      group: '.col-xs-6',
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
           procedurecategoryDetail.ProcedureCategorySave();
       });
    },

    SaveProcedureCategory: function (ProcedureCategoryData) {
        var data = "ProcedureCategoryData=" + ProcedureCategoryData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_CATEGORY_DETAIL", "SAVE_PROCEDURE_CATEGORY");
    },

    UpdateProcedureCategory: function (ProcedureCategoryData, ProcedureCategoryID) {
        var data = "ProcedureCategoryData=" + ProcedureCategoryData + "&ProcedureCategoryID=" + ProcedureCategoryID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_CATEGORY_DETAIL", "UPDATE_PROCEDURE_CATEGORY");
    },

    FillProcedureCategory: function (ProcedureCategoryID) {
        var data = "ProcedureCategoryID=" + ProcedureCategoryID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_CATEGORY_DETAIL", "FILL_PROCEDURE_CATEGORY");
    },

    UpdateProcedureCategoryActiveInactive: function (ProcedureCategoryID, IsActive) {
        var data = "ProcedureCategoryID=" + ProcedureCategoryID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROCEDURE_CATEGORY_DETAIL", "UPDATE_PROCEDURE_CATEGORY_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmProcedureCategoryDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });


    },

    ShowHistory: function () {
        var PanelID = 'procedurecategoryDetail';
        var ParentCtrl = 'procedurecategoryDetail';
        var ProfileName = 'Procedure Category';
        var DBTableName = 'ProcedureCategory';
        var ColumnKeyId = procedurecategoryDetail.params.ProcedureCategoryId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}