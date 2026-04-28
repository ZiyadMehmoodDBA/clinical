Admin_OccupationStatusDetail = {

    params: [],
    Enable: false,
    Load: function (params) {
        Admin_OccupationStatusDetail.params = params;

        if (Admin_OccupationStatusDetail.params["PanelID"] != "pnlOccupationStatusDetail") {
            Admin_OccupationStatusDetail.params["PanelID"] = "pnlOccupationStatusDetail";
        }
        var self = $('#' + Admin_OccupationStatusDetail.params["PanelID"]);
        self.find("#hfStatusId").val(Admin_OccupationStatusDetail.params.StatusId);
        debugger;
        Admin_OccupationStatusDetail.LoadOccupationStatus();

    },

    LoadOccupationStatus: function () {
        if (Admin_OccupationStatusDetail.params.mode == "Add") {
            $('#pnlOccupationStatusDetail #frmOccupationStatusDetail').data('serialize', $('#pnlOccupationStatusDetail #frmOccupationStatusDetail').serialize());
            Admin_OccupationStatusDetail.ValidationOccupationStatusCode();
        }
        else if (Admin_OccupationStatusDetail.params.mode == "Edit") {
            Admin_OccupationStatusDetail.FillOccupationStatus(Admin_OccupationStatusDetail.params.StatusId).done(function (response) {
                if (response.status != false) {
                    var OccupationStatus_detail = JSON.parse(response.OccupationStatus_JSON);
                    var self = $("#pnlOccupationStatusDetail");
                    utility.bindMyJSON(true, OccupationStatus_detail, false, self);
                    console.log(OccupationStatus_detail.IsOccupation);
                    if (OccupationStatus_detail.IsOccupation == 'True') {
                        $("#pnlOccupationStatusDetail #chkOccupation").prop("checked", true).trigger("click");
                    }
                    else {
                        $("#pnlOccupationStatusDetail #chkIndustry").prop("checked", false).trigger("click");
                    }
                    $('#frmOccupationStatusDetail').data('serialize', $('#frmOccupationStatusDetail').serialize());
                    Admin_OccupationStatusDetail.ValidationOccupationStatusCode();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    FillOccupationStatus: function (StatusID) {
        var data = "StatusID=" + StatusID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_OCCUPATION_STATUS_DETAIL", "FILL_OCCUPATION_STATUS");
    },

    ValidationOccupationStatusCode: function () {
        $('#pnlOccupationStatusDetail #frmOccupationStatusDetail')
          .bootstrapValidator({
              live: 'disabled',
              message: 'This value is not valid',
              feedbackIcons: {
                  valid: 'glyphicon glyphicon-ok',
                  invalid: 'glyphicon glyphicon-remove',
                  validating: 'glyphicon glyphicon-refresh'
              },
              fields: {
                  Description: {
                      group: '.col-sm-10',
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
           Admin_OccupationStatusDetail.OccupationStatusDetailSave();
       });
    },

    OccupationStatusDetailSave: function () {
        $('#frmOccupationStatusDetail').data('serialize', $('#frmOccupationStatusDetail').serialize());
        var strMessage = "";
        var self = $("#pnlOccupationStatusDetail");
        var myJSON = self.getMyJSON();
        myJSON = JSON.parse(myJSON);
        myJSON['chkIsActive'] = $("#frmOccupationStatusDetail input[name='chkIsActive']:checked").val();
        myJSON = JSON.stringify(myJSON);
        if (Admin_OccupationStatusDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Occupation Status", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    if (Admin_OccupationStatusDetail.params.StatusId == "-1" || Admin_OccupationStatusDetail.params.StatusId == "" || Admin_OccupationStatusDetail.params.StatusId == "0") {
                        Admin_OccupationStatusDetail.SaveOccupationStatus(myJSON).done(function (response) {
                            if (response.status != false) {
                                Admin_OccupationStatus.OccupationStatusSearch(response.StatusId);
                                utility.DisplayMessages(response.message, 1);
                                Admin_OccupationStatusDetail.UnLoad();
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                    else if (Admin_OccupationStatusDetail.params.StatusId != "-1" && Admin_OccupationStatusDetail.params.StatusId != "" && Admin_OccupationStatusDetail.params.StatusId != "0") {
                        Admin_OccupationStatusDetail.SaveOccupationStatus(myJSON).done(function (response) {
                            if (response.status != false) {
                                Admin_OccupationStatus.OccupationStatusSearch(response.StatusId);
                                Admin_OccupationStatusDetail.UnLoad();
                                utility.DisplayMessages(response.message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (Admin_OccupationStatusDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Occupation Status", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Admin_OccupationStatusDetail.SaveOccupationStatus(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_OccupationStatus.OccupationStatusSearch(response.StatusId);
                            Admin_OccupationStatusDetail.UnLoad();
                            utility.DisplayMessages(response.message, 1);
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

    SaveOccupationStatus: function (OccupationStatusData) {
        var data = "OccupationStatusData=" + OccupationStatusData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_OCCUPATION_STATUS_DETAIL", "SAVE_OCCUPATIONSTATUS_CODE");
    },

    UnLoad: function () {
        if ($('#frmOccupationStatusDetail').serialize() != $('#frmOccupationStatusDetail').data('serialize')) {
            utility.myConfirm('2', function () {
                UnloadActionPan(Admin_OccupationStatusDetail.params["ParentCtrl"], "Admin_OccupationStatusDetail");
            }, function () { },
                    '2'
                );
        }
        else {
            UnloadActionPan(Admin_OccupationStatusDetail.params["ParentCtrl"], "Admin_OccupationStatusDetail");
        }
    },




}