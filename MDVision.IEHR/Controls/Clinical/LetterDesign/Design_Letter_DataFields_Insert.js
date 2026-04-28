designLetterDataFieldsInsert = {
    params: [],
    bIsFirstLoad: true,

    BillId: -1,

    Load: function (params) {

        designLetterDataFieldsInsert.params = params;
       
        if (designLetterDataFieldsInsert.params["PanelID"] != 'designLetterDataFieldsInsert')
            designLetterDataFieldsInsert.params["PanelID"] = designLetterDataFieldsInsert.params["PanelID"] + 'designLetterDataFieldsInsert'

        if (designLetterDataFieldsInsert.bIsFirstLoad) {
            designLetterDataFieldsInsert.bIsFirstLoad = false;

            var self = $('#' + designLetterDataFieldsInsert.params["PanelID"]);

            self.loadDropDowns(true).done(function () {
                $('#designLetterDataFieldsInsert #lstCategoryId option:selected').attr("selected", null);
                $('#designLetterDataFieldsInsert #lstCategoryId option[value="' + designLetterDataFieldsInsert.params["CategoryID"] + '"]').attr("selected", "selected");
               
            });
            var letterId = designLetterDataFieldsInsert.params["letterID"]
            designLetterDataFieldsInsert.ValidateLetterFieldsDetail(designLetterDataFieldsCreate.params.LetterFieldsId, letterId);
        }
    },

    ValidateLetterFieldsDetail: function (letterFieldsId, letterId) {
        $('#frmdesignLetterDataFieldsInsert')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  LetterFields: {
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Format: {
                      group: '.col-sm-4',
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
           designLetterDataFieldsInsert.saveLetterFields(letterFieldsId, letterId);
       });
    },

    saveLetterFields: function (letterFieldsId, letterId) {
      
        var strMessage = "";
        var self = $('#designLetterDataFieldsInsert');
        var myJSON = self.getMyJSON();
        //if (questionDetail.params.mode == null) {
        //AppPrivileges.GetFormPrivileges("Messages", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {

            if (designLetterDataFieldsInsert.params.mode == "Add") {
                designLetterDataFieldsInsert.letterFieldsSave(myJSON, letterId).done(function (response) {
                    if (response.status != false) {
                        //alert(response.LtrFieldId);
                        //questionDetail.params.patientID = response.patientID;
                        utility.DisplayMessages(response.message, 1);
                        //if (questionDetail.params.ParentCtrl == "User_Message") {
                        //    Patient_Message.SearchPatientMessage(questionDetail.params.MessageId, 1, questionDetail.params.AssignedToId);
                        //}
                   
                            //alert(arr.push(drpValue[0]));
                            //alert(drpValue[1]);
                      
                            if (typeof tinyMCE != 'undefined') {
                                var FieldNameofSelectedField = $('#designLetterDataFieldsInsert #lstFieldId option:selected').val().split('~')[1];
                                var InsertFieldInput = '<input type="text" class="FieldInserted_PK" readonly id="' + response.MessageId + '" value="{{ ' + FieldNameofSelectedField + ' }}" style="min-width: 10px; margin: 0 5px; margin-right:5px; border: none;padding: 0 0px;width:'+(FieldNameofSelectedField.length+4)*7+'px;"/>';
                                //var InsertFieldInput = '<input type="text" class="FieldInserted_PK" readonly id="' + response.MessageId + '" value="' + FieldNameofSelectedField + '" style="min-width: 10px; margin: 0 10px; margin-right: right:10px; border: none;border-right: black solid 1px;border-left: black solid 1px;padding: 0 5px;"/>';
                                tinymce.execCommand('mceInsertContent', false,  InsertFieldInput );
                                // tinyMCE.activeEditor.setContent(FieldNameofSelectedField);
                                if (typeof sessionStorage["hfInsertedFieldsId"]=='undefined') {
                                    sessionStorage["hfInsertedFieldsId"]=null;
                                }
                                var FieldsId = sessionStorage["hfInsertedFieldsId"];

                                if (FieldsId != "null") { sessionStorage["hfInsertedFieldsId"] = FieldsId + "," + response.MessageId } else { sessionStorage["hfInsertedFieldsId"] = response.MessageId; };

                            }

                        //else
                        //    Patient_Message.SearchPatientMessage(questionDetail.params.MessageId);
                        //Patient_MessageAdd.UnLoad();
                        if (designLetterDataFieldsInsert.params != null && designLetterDataFieldsInsert.params.ParentCtrl != null) {
                            UnloadActionPan(designLetterDataFieldsInsert.params.ParentCtrl, 'designLetterDataFieldsInsert');
                        }
                        else
                            UnloadActionPan(null, 'designLetterDataFieldsInsert');
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

    letterFieldsSave: function (letterFieldsData, letterId) {
        var data = "letterFieldsData=" + letterFieldsData + "&letterId=" + letterId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "DESIGN_LETTER", "SAVE_LETTER_FIELDS");
    },

    UnLoad: function () {

        utility.UnLoadDialog('frmdesignLetterDataFieldsInsert', function () {
            
            if (designLetterDataFieldsInsert.params.ParentCtrl == "letterDetail") {
                if ($('#hfInsertedFieldsId').val() != '') {
                    //delete this ids
                }
                UnloadActionPan('letterDetail');
            } else {
                UnloadActionPan();
            }

        }, function () {
            UnloadActionPan();
        });

    }

}