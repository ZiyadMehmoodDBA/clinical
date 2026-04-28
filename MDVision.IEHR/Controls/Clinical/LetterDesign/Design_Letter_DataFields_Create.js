designLetterDataFieldsCreate = {
    params: [],
    bIsFirstLoad: true,

    BillId: -1,

    Load: function (params) {

        designLetterDataFieldsCreate.params = params;

        if (designLetterDataFieldsCreate.params["PanelID"] != 'designLetterDataFieldsCreate')
            designLetterDataFieldsCreate.params["PanelID"] = designLetterDataFieldsCreate.params["PanelID"] + 'designLetterDataFieldsCreate'

        if (designLetterDataFieldsCreate.bIsFirstLoad) {
            designLetterDataFieldsCreate.bIsFirstLoad = false;

            var self = $('#' + designLetterDataFieldsCreate.params["PanelID"]);

            self.loadDropDowns(true).done(function () {

                if (globalAppdata['IsAdmin'] != "True") {
                    $("#designLetterDataFieldsCreate #divEntity").css("display", "none");
                    $("#designLetterDataFieldsCreate #lstEntityId").val(globalAppdata["SeletedEntityId"]);
                }

                $('#designLetterDataFieldsCreate #ddlCategory option:selected').attr("selected", null);
                $('#designLetterDataFieldsCreate #ddlCategory option[value="' + designLetterDataFieldsCreate.params["CategoryID"] + '"]').attr("selected", "selected");

            });

            designLetterDataFieldsCreate.ValidateFieldsDetail(designLetterDataFieldsCreate.params.FieldsId);
        }
    },

    ValidateFieldsDetail: function (fieldsID) {
        $('#frmdesignLetterDataFieldsCreate')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  txtDataFieldName: {
                      group: '.col-xs-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  ddlCategory: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  txtDataFieldDescription: {
                      group: '.col-xs-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  lstEntityId: {
                      group: '.col-xs-4',
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
           designLetterDataFieldsCreate.saveFields(fieldsID);
       });
    },

    saveFields: function (fieldsID) {
        var strMessage = "";
        var self = $('#designLetterDataFieldsCreate');
        var myJSON = self.getMyJSON();
        //if (questionDetail.params.mode == null) {
        //AppPrivileges.GetFormPrivileges("Messages", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            if (designLetterDataFieldsCreate.params.mode == "Add") {
                designLetterDataFieldsCreate.FieldsSave(myJSON).done(function (response) {
                    if (response.status != false) {
                        //questionDetail.params.patientID = response.patientID;
                        utility.DisplayMessages(response.message, 1);
                        //if (questionDetail.params.ParentCtrl == "User_Message") {
                        //    Patient_Message.SearchPatientMessage(questionDetail.params.MessageId, 1, questionDetail.params.AssignedToId);
                        //}
                        //else
                        //    Patient_Message.SearchPatientMessage(questionDetail.params.MessageId);
                        //Patient_MessageAdd.UnLoad();
                        if (designLetterDataFieldsCreate.params != null && designLetterDataFieldsCreate.params.ParentCtrl != null) {
                            UnloadActionPan(designLetterDataFieldsCreate.params.ParentCtrl, 'designLetterDataFieldsCreate');
                        }
                        else
                            UnloadActionPan(null, 'designLetterDataFieldsCreate');
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
        else
            utility.DisplayMessages(strMessage, 2);
        //});
        //}
    },

    FieldsSave: function (fieldsData) {
        var data = "fieldsData=" + fieldsData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "DESIGN_LETTER", "SAVE_FIELDS");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmdesignLetterDataFieldsCreate', function () {
            if (designLetterDataFieldsCreate.params.ParentCtrl == "letterDetail") {
                UnloadActionPan('letterDetail');
            } else {
                UnloadActionPan();
            }
        }, function () {
            UnloadActionPan();
        });
    },

}