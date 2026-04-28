CaseAdjusterDetail={
    params: [],
    Load: function (params) {
        CaseAdjusterDetail.params = params;
        var self = null;
        if (CaseAdjusterDetail.params.PanelID != 'CaseAdjusterDetail')
            self = $('#' + CaseAdjusterDetail.params.PanelID + ' #CaseAdjusterDetail')
        else
            self = $('#CaseAdjusterDetail');
        self.loadDropDowns(true).done(function () {
           
            $('#frm_CaseAdjusterDetail').data('serialize', $('#CaseAdjusterDetail').serialize());
        });
        utility.CreateDatePicker(CaseAdjusterDetail.params.PanelID + ' #frm_CaseAdjusterDetail #dtpDOB', function (ev) { },false);
        CaseAdjusterDetail.LoadCaseAdjuster();
       CaseAdjusterDetail.ValidateCaseAdjuster();
    },
    LoadCaseAdjuster: function () {
        if (CaseAdjusterDetail.params.mode == "Add") {
            $('#' + CaseAdjusterDetail.params.PanelID + ' #CaseAdjusterDetail #RadCaseAdjusterPhone').prop("checked", true);
            $('#' + CaseAdjusterDetail.params.PanelID + ' #CaseAdjusterDetail #chkActive').prop("checked", true);
           
        }
        else if (CaseAdjusterDetail.params.mode == "Edit") {
           
            CaseAdjusterDetail.FillCaseAdjuster(CaseAdjusterDetail.params.CaseAdjusterId).done(function (response) {
                if (response.status != false) {
                    var CaseAdjuster_detail = JSON.parse(response.CaseFill_JSON);
                    var self = $("#CaseAdjusterDetail");
                    utility.bindMyJSON(true, CaseAdjuster_detail, false, self);
                    $('#frm_CaseAdjusterDetail').data('serialize', $('#frm_CaseAdjusterDetail').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },
    FillCaseAdjuster: function (CaseAdjusterId) {
        var data = "CaseAdjusterId=" + CaseAdjusterId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE_ADJUSTER", "FILL_CASE");
    },
    ValidateCaseAdjuster: function () {
        $('#CaseAdjusterDetail #frm_CaseAdjusterDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  FirstName: {
                      group: '.col-md-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  LastName: {
                      group: '.col-md-6',
                      validators: {
                          notEmpty: {
                              message: ''
                          }
                      }
                  },
                  Email: {
                      group:'.col-md-6',
                      validators: {
                          emailAddress: {
                              message: ''
                          }
                      }
                  },
              }
          })
       .on('success.form.bv', function (e) {
           e.preventDefault();
           CaseAdjusterDetail.CaseAdjusterSave();
       });
    },
    CaseAdjusterSave:function(){
        var strMessage = "";
        var self = $("#CaseAdjusterDetail");
        var myJSON = self.getMyJSON();
        if (CaseAdjusterDetail.params.mode == "Add") {
            CaseAdjusterDetail.SaveCaseAdjuster(myJSON).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    CaseAdjusterDetail.UnloadPan();
                    Patient_CaseAdjuster.CaseAdjusterSearch(response.CaseAdjusterId);
                }
                else {
                    if (response.Message.indexOf('duplicate key') > -1) {
                        utility.DisplayMessages("Case Adjuster already exists", 3);
                    } else
                        utility.DisplayMessages(response.Message, 3);
                }
            });
            
        }
        else if (CaseAdjusterDetail.params.mode == "Edit") {
            CaseAdjusterDetail.UpdateCaseAdjuster(myJSON, CaseAdjusterDetail.params.CaseAdjusterId).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.message, 1);
                    CaseAdjusterDetail.UnloadPan();
                    Patient_CaseAdjuster.CaseAdjusterSearch(CaseAdjusterDetail.params.CaseAdjusterId);
                }
                else {
                    if (response.Message.indexOf('duplicate key') > -1) {
                        utility.DisplayMessages("Case Adjuster already exists", 3);
                    } else
                        utility.DisplayMessages(response.Message, 3);
                }
            }); 
        }
    },
    SaveCaseAdjuster: function (CaseAdjusterData) {
        var data = "CaseAdjusterData=" + CaseAdjusterData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE_ADJUSTER", "SAVE_CASE_ADJUSTER");
    },
    UpdateCaseAdjuster: function (CaseAdjusterData, CaseAdjusterId) {
        var data = "CaseData=" + CaseAdjusterData + "&CaseAdjusterId=" + CaseAdjusterId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "PATIENT_CASE_ADJUSTER", "UPDATE_CASE");
    },
    UnloadPan: function () {
        UnloadActionPan(CaseAdjusterDetail.params.ParentCtrl, 'CaseAdjusterDetail');
    }
}