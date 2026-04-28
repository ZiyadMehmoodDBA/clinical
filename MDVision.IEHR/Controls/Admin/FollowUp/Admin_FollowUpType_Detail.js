followUpTypeDetail = {
    params: [],
    bIsFirstLoad: true,

    Load: function (params) {

        followUpTypeDetail.params = params;

        if (followUpTypeDetail.params["PanelID"] != 'followUpTypeDetail')
            followUpTypeDetail.params["PanelID"] = followUpTypeDetail.params["PanelID"] + ' #followUpTypeDetail'

        if (followUpTypeDetail.bIsFirstLoad) {
            followUpTypeDetail.bIsFirstLoad = false;

            var self = $('#' + followUpTypeDetail.params["PanelID"]);

            self.loadDropDowns(true).done(function() {
                
                if (followUpTypeDetail.params.mode == "Add") {
                }
                else if (followUpTypeDetail.params.mode == "Edit") {
                    followUpTypeDetail.LoadType();
                }

                followUpTypeDetail.ValidateTypeDetail(followUpTypeDetail.params.TypeId);
                //serialize Data after all controls loaded.
                $('#frmfollowUpTypeDetail').data('serialize', $('#frmfollowUpTypeDetail').serialize());
            });

            
        }
    },

    LoadType: function (TypeId, mode) {

        //AppPrivileges.GetFormPrivileges("Messages", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //if (strMessage == "") {

        if (followUpTypeDetail.params.mode == "Add") { }
        else if (followUpTypeDetail.params.mode == "Edit") {

            followUpTypeDetail.FillType(followUpTypeDetail.params.TypeId).done(function (response) {
                if (response.status != false) {

                    var Type_detail = JSON.parse(response.TypeLoad_JSON);
                    var self = $('#' + followUpTypeDetail.params["PanelID"]);
                    utility.bindMyJSON(true, JSON.parse(response.TypeLoad_JSON), false, self);

                    $('#ShortName').attr('disabled', true);

                    if (Type_detail.Active == "True") {
                        $("#" + followUpTypeDetail.params["PanelID"] + " #Active").prop("checked", true);
                    } else {
                        $("#" + followUpTypeDetail.params["PanelID"] + " #Active").prop("checked", false);
                    }

                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

                //serialize Data after all controls loaded.
                $('#frmfollowUpTypeDetail').data('serialize', $('#frmfollowUpTypeDetail').serialize());
            });
        }
        // }
        //else {
        //    utility.DisplayMessages(strMessage, 2);
        //}
        //});

    },

    FillType: function (TypeId) {
        var data = "TypeId=" + TypeId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPTYPE", "FILL_TYPE");
    },

    ValidateTypeDetail: function (TypeId) {
        $('#followUpTypeDetail')
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
                      group: '.col-sm-6',
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
           followUpTypeDetail.TypeSave(TypeId);
       });
    },

    TypeSave: function (TypeId) {
        var strMessage = "";
        var self = $('#' + followUpTypeDetail.params["PanelID"]);
        var myJSON = self.getMyJSON();
        if (followUpTypeDetail.params.mode == "Add") {
            if (strMessage == "") {
                followUpTypeDetail.SaveType(myJSON).done(function (response) {
                    if (response.status != false) {
                        Admin_FollowUpType.TypeSearch();
                        utility.DisplayMessages(response.Message, 1);
                        if (followUpTypeDetail.params != null && followUpTypeDetail.params.ParentCtrl != null) {
                        }
                        else {
                        }
                        followUpTypeDetail.params.TypeId = response.TypeId;
                        $('#frmfollowUpTypeDetail').data('serialize', $('#frmfollowUpTypeDetail').serialize());
                        followUpTypeDetail.UnLoad();
                    }
                    else {
                        Admin_FollowUpType.TypeSearch();
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
        else if (followUpTypeDetail.params.mode == "Edit") {
            if (strMessage == "") {
                followUpTypeDetail.TypeEdit(myJSON, TypeId, 2).done(function (response) {
                    if (response.status != false) {
                        //followUpTypeDetail.UnLoad();
                        Admin_FollowUpType.TypeSearch();
                        $('#frmfollowUpTypeDetail').data('serialize', $('#frmfollowUpTypeDetail').serialize());
                        followUpTypeDetail.UnLoad();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        Admin_FollowUpType.TypeSearch();
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
    },

    TypeEdit: function (TypeData, TypeId) {
        var data = "TypeData=" + TypeData + "&TypeId=" + TypeId;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPTYPE", "UPDATE_TYPE");
    },

    SaveType: function (TypeData) {
        var data = "TypeData=" + TypeData;
        return MDVisionService.defaultService(data, "ADMIN_FOLLOWUPTYPE", "SAVE_TYPE");
    },


    UnLoad: function () {


        utility.UnLoadDialog('frmfollowUpTypeDetail', function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });


    },
        
    ShowHistory: function () {
        var PanelID = 'followUpTypeDetail';
        var ParentCtrl = 'followUpTypeDetail';
        var ProfileName = 'FollowUp Type';
        var DBTableName = 'ARType';
        var ColumnKeyId = followUpTypeDetail.params.TypeId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

}