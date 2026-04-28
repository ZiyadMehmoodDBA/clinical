feegroupDetail = {
    params: [],
    Enable: false,
    Load: function (params) {
        feegroupDetail.params = params;

        var self = null;
        if (feegroupDetail.params.PanelID == "feegroupDetail")
            self = $('#feegroupDetail');
        else
            self = $('#' + feegroupDetail.params.PanelID + ' #feegroupDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {            
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#feegroupDetail #divFeeGroup_Entity").css("display", "none");
            //    $("#feegroupDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }

            $("#feegroupDetail #chkIsActive").attr("checked", true);
            feegroupDetail.LoadFeeGroup();
           
        });
           
    },


    LoadFeeGroup: function () {
        if (feegroupDetail.params.mode == "Add") {

            //serialize data
            $('#frmFeeGroupDetail').data('serialize', $('#frmFeeGroupDetail').serialize());
            feegroupDetail.ValidationFeeGroup();
        }
        else if (feegroupDetail.params.mode == "Edit") {
            feegroupDetail.FillFeeGroup(feegroupDetail.params.FeeGroupId).done(function (response) {
                if (response.status != false) {
                    var feegroup_detail = JSON.parse(response.FeeGroupFill_JSON);
                    var self = $("#feegroupDetail");

                    utility.bindMyJSON(true, feegroup_detail, false, self).done(function () {

                        if (feegroup_detail.ChkIsActive == 'True') {
                            $("#feegroupDetail #chkIsActive").attr("checked", true);
                        }
                        else {
                            $("#feegroupDetail #chkIsActive").attr("checked", false);
                        }
                        feegroupDetail.LoadNextFeeGroup(feegroup_detail.ddlEntity, feegroup_detail.ddlNextFeeGroup);
                        feegroupDetail.ValidationFeeGroup();

                        //serialize data
                        $('#frmFeeGroupDetail').data('serialize', $('#frmFeeGroupDetail').serialize());

                    });

                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    LoadNextFeeGroup: function (entityID, ddlNextFeeGroup) {
        var EntityID = "";
        feegroupDetail.FillNextFeeGroup(entityID).done(function (response) {
            if (response.status != false) {
                var feegroup_detail = JSON.parse(response.NextFeeGroupLoad_JSON);
                $("#tblfeegroupDetail #ddlNextFeeGroup").empty();
                $("#tblfeegroupDetail #ddlNextFeeGroup").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(feegroup_detail, function (i, item) {
                    $("#tblfeegroupDetail #ddlNextFeeGroup").append(
                        $('<option/>', {
                            value: item.FeeGroupId,
                            html: item.EntityName + " - " + item.ShortName
                        })
                    );
                });
            }
            $('#tblfeegroupDetail #ddlNextFeeGroup').val(ddlNextFeeGroup);
        });
    },

    FeeGroupSave: function () {
        var strMessage = "";
        var self = $("#feegroupDetail");
        var myJSON = self.getMyJSON();
        if (feegroupDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Fee Group", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    feegroupDetail.SaveFeeGroup(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_FeeGroup.FeeGroupSearch(response.FeeGroupId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetFeeGroup', true);
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
        else if (feegroupDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Fee Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    feegroupDetail.UpdateFeeGroup(myJSON, feegroupDetail.params.FeeGroupId).done(function (response) {
                        if (response.status != false) {
                            Admin_FeeGroup.FeeGroupSearch(feegroupDetail.params.FeeGroupId);

                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetFeeGroup', true);
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

    ValidationFeeGroup: function () {
        $('#frmFeeGroupDetail')
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
                      group: '.col-md-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Entity: {
                      group: '.col-md-4',
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
           feegroupDetail.FeeGroupSave();
       });
    },

    SaveFeeGroup: function (FeeGroupData) {
        var data = "FeeGroupData=" + FeeGroupData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FEE_GROUP_DETAIL", "SAVE_FEE_GROUP");
    },

    UpdateFeeGroup: function (FeeGroupData, FeeGroupID) {
        var data = "FeeGroupData=" + FeeGroupData + "&FeeGroupID=" + FeeGroupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FEE_GROUP_DETAIL", "UPDATE_FEE_GROUP");
    },

    FillFeeGroup: function (FeeGroupID) {
        var data = "FeeGroupID=" + FeeGroupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FEE_GROUP_DETAIL", "FILL_FEE_GROUP");
    },

    FillNextFeeGroup: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FEE_GROUP_DETAIL", "FILL_NEXT_FEE_GROUP");
    },

    UpdateFeeGroupActiveInactive: function (FeeGroupID, IsActive) {
        var data = "FeeGroupID=" + FeeGroupID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FEE_GROUP_DETAIL", "UPDATE_FEE_GROUP_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmFeeGroupDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });

    },

    ShowHistory: function () {
        var PanelID = 'feegroupDetail';
        var ParentCtrl = 'feegroupDetail';
        var ProfileName = 'Fee Group';
        var DBTableName = 'FeeGroup';
        var ColumnKeyId = feegroupDetail.params.FeeGroupId

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}