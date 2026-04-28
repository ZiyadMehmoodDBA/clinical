SelectLetter_Template = {
    Load: function (params) {
        SelectLetter_Template.params = params;

        if (SelectLetter_Template.params.PanelID != 'pnlSelectLetter_Template') {
            SelectLetter_Template.params.PanelID = SelectLetter_Template.params.PanelID + ' #pnlSelectLetter_Template';
        } else {
            SelectLetter_Template.params.PanelID = 'pnlSelectLetter_Template';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#" + SelectLetter_Template.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        var self = $('#' + SelectLetter_Template.params.PanelID);
        //Start 11-10-2017 Edit By Humaira Yousaf IMP-1189
        var data = "IsActive=&ID=-1";
        //End 11-10-2017 Edit By Humaira Yousaf IMP-1189
        self.loadDropDowns(true,data).done(function () {
            SelectLetter_Template.ValidateSelectLetter_Template();
        });
        
    },
    ValidateSelectLetter_Template: function () {
        $('#frmSelectLetter_Template')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {

                  TemplateLetterId: {
                      group: '.col-sm-10',
                      validators: {
                          notEmpty: {
                              message: 'Select a template to create the letter. '
                          },
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           SelectLetter_Template.CreateLetter();
       });
    },
    UnLoadTab: function () {
        var objDeffered = $.Deferred();

        if (SelectLetter_Template.params && SelectLetter_Template.params["FromAdmin"] == "0") {
            if (SelectLetter_Template.params != null && SelectLetter_Template.params.ParentCtrl != null) {
                if (SelectLetter_Template.params["ParentCtrlPanelID"])
                    UnloadActionPan(SelectLetter_Template.params["ParentCtrl"], "SelectLetter_Template", null, SelectLetter_Template.params["ParentCtrlPanelID"]);
                else
                    UnloadActionPan(SelectLetter_Template.params.ParentCtrl, 'SelectLetter_Template');
            }
            else
                UnloadActionPan(null, 'SelectLetter_Template');
        }
        else {

            RemoveAdminTab();
        }
        objDeffered.resolve();
        return objDeffered;
    },

    CreateLetter: function () {

        var params = [];
        params["ParentCtrl"] = "SelectLetter_Template";
        params["FromAdmin"] = 0;
        params["TemplateLetterId"] = $("#" + SelectLetter_Template.params.PanelID + " #ddlTemplateLetter").val();
        params["TemplateLetterText"] = $("#" + SelectLetter_Template.params.PanelID + " #ddlTemplateLetter option:selected").text();
        params["mode"] = "Add";
        params["PatientId"] = SelectLetter_Template.params["PatientId"];
        if (SelectLetter_Template.params.ParentCtrl == "clinicalTabProgressNote") {
            params["GrandParent"] = "clinicalTabProgressNote";
            params["NotesId"] = SelectLetter_Template.params.NotesId;
        }
        LoadActionPan("Create_Letter", params);

    },

}