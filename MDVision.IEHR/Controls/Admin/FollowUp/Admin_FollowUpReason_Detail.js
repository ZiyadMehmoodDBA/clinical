followUpReasonDetail = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {

        followUpReasonDetail.params = params;

        if (followUpReasonDetail.params["PanelID"] != 'followUpReasonDetail')
            followUpReasonDetail.params["PanelID"] = followUpReasonDetail.params["PanelID"] + ' #followUpReasonDetail'

        if (followUpReasonDetail.bIsFirstLoad) {
            followUpReasonDetail.bIsFirstLoad = false;

            var self = $('#' + followUpReasonDetail.params["PanelID"]);

            self.loadDropDowns(true).done(function() {
                
                if (followUpReasonDetail.params.mode == "Add") {
                }
                else if (followUpReasonDetail.params.mode == "Edit") {
                    followUpReasonDetail.LoadReason();
                }

                followUpReasonDetail.ValidateReasonDetail(followUpReasonDetail.params.reasonId);
                //serialize Data after all controls loaded.
                $('#frmfollowUpReasonDetail').data('serialize', $('#frmfollowUpReasonDetail').serialize());
            });

           
        }
    },

    LoadReason: function (reasonId, mode) {

        //AppPrivileges.GetFormPrivileges("Messages", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //if (strMessage == "") {

        if (followUpReasonDetail.params.mode == "Add") { }
        else if (followUpReasonDetail.params.mode == "Edit") {

            followUpReasonDetail.FillReason(followUpReasonDetail.params.reasonId).done(function (response) {
                if (response.status != false) {

                    var Reason_detail = JSON.parse(response.ReasonLoad_JSON);
                    var self = $('#' + followUpReasonDetail.params["PanelID"]);
                    utility.bindMyJSON(true, JSON.parse(response.ReasonLoad_JSON), false, self);

                    $('#ShortName').attr('disabled', true);

                    if (Reason_detail.Active == "True") {
                        $("#" + followUpReasonDetail.params["PanelID"] + " #Active").prop("checked", true);
                    } else {
                        $("#" + followUpReasonDetail.params["PanelID"] + " #Active").prop("checked", false);
                    }


                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

                //serialize Data after all controls loaded.
                $('#frmfollowUpReasonDetail').data('serialize', $('#frmfollowUpReasonDetail').serialize());
            });
        }
        // }
        //else {
        //    utility.DisplayMessages(strMessage, 2);
        //}
        //});

    },

    FillReason: function (reasonId) {
        var data = "reasonId=" + reasonId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPREASON", "FILL_REASON");
    },

    ValidateReasonDetail: function (reasonId) {
        $('#followUpReasonDetail')
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
                  Description: {
                      group: '.col-sm-5',
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
           followUpReasonDetail.ReasonSave(reasonId);
       });
    },

    ReasonSave: function (reasonId) {
        var strMessage = "";
        var self = $('#' + followUpReasonDetail.params["PanelID"]);
        var myJSON = self.getMyJSON();
        if (followUpReasonDetail.params.mode == "Add") {
            if (strMessage == "") {
                followUpReasonDetail.SaveReason(myJSON).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Admin_FollowUpReason.ReasonSearch();
                        if (followUpReasonDetail.params != null && followUpReasonDetail.params.ParentCtrl != null) {
                        }
                        else {
                        }
                        followUpReasonDetail.params.reasonId = response.ReasonId;
                        $('#frmfollowUpReasonDetail').data('serialize', $('#frmfollowUpReasonDetail').serialize());
                        followUpReasonDetail.UnLoad();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        Admin_FollowUpReason.ReasonSearch();
                    }
                });
            }
        }
        else if (followUpReasonDetail.params.mode == "Edit") {
            if (strMessage == "") {
                followUpReasonDetail.ReasonEdit(myJSON, reasonId, 2).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages(response.Message, 1);
                        Admin_FollowUpReason.ReasonSearch();
                        $('#frmfollowUpReasonDetail').data('serialize', $('#frmfollowUpReasonDetail').serialize());
                        followUpReasonDetail.UnLoad();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        Admin_FollowUpReason.ReasonSearch();
                    }
                });
            }
        }
    },

    ReasonEdit: function (reasonData, reasonId) {
        var data = "reasonData=" + reasonData + "&reasonId=" + reasonId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPREASON", "UPDATE_REASON");
    },

    SaveReason: function (reasonData) {
        var data = "reasonData=" + reasonData;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPREASON", "SAVE_REASON");
    },

    UnLoad: function () {


        utility.UnLoadDialog('frmfollowUpReasonDetail', function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });


    },
    
    ShowHistory: function () {
        var PanelID = 'followUpReasonDetail';
        var ParentCtrl = 'followUpReasonDetail';
        var ProfileName = 'FollowUp Reason';
        var DBTableName = 'FollowupReasons';
        var ColumnKeyId = followUpReasonDetail.params.reasonId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}