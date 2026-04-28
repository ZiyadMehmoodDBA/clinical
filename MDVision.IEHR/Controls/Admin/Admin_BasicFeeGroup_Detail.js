basicfeegroupDetail = {
    params: [],
    Enable: false,
    Load: function (params) {
        basicfeegroupDetail.params = params;
        
        //initialization of date-picker.
        utility.CreateDatePicker('tblbasicfeegroupDetail #txtEndDate', function () {
            //on-change callback method 
        });

        var self = null;
        if (basicfeegroupDetail.params.PanelID == "basicfeegroupDetail")
            self = $('#basicfeegroupDetail');
        else
            self = $('#' + basicfeegroupDetail.params.PanelID + ' #basicfeegroupDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {            
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#basicfeegroupDetail #divBasicFeeGroup_Entity").css("display", "none");
            //    $("#basicfeegroupDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
                
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            $("#basicfeegroupDetail #chkIsActive").attr("checked", true);
            basicfeegroupDetail.LoadBasicFeeGroup();
        });

        
           
    },

    LoadBasicFeeGroup: function () {
        if (basicfeegroupDetail.params.mode == "Add") {

            //serialize data
            $('#frmBasicFeeGroupDetail').data('serialize', $('#frmBasicFeeGroupDetail').serialize());
            basicfeegroupDetail.ValidationBasicFeeGroup();
        }
        else if (basicfeegroupDetail.params.mode == "Edit") {
            basicfeegroupDetail.FillBasicFeeGroup(basicfeegroupDetail.params.BasicFeeGroupId).done(function (response) {
                if (response.status != false) {
                    var basicfeegroup_detail = JSON.parse(response.BasicFeeGroupFill_JSON);
                    var self = $("#basicfeegroupDetail");
                    utility.bindMyJSON(true, basicfeegroup_detail, false, self).done(function () {

                        if (basicfeegroup_detail.ChkIsActive == 'True')
                            $("#basicfeegroupDetail #chkIsActive").attr("checked", true);
                        else
                            $("#basicfeegroupDetail #chkIsActive").attr("checked", false);
                        basicfeegroupDetail.LoadNextBasicFeeGroup(basicfeegroup_detail.ddlEntity, basicfeegroup_detail.ddlNextBasicFeeGroup);
                        basicfeegroupDetail.ValidationBasicFeeGroup();

                        //serialize data
                        $('#frmBasicFeeGroupDetail').data('serialize', $('#frmBasicFeeGroupDetail').serialize());

                    });

                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    BasicFeeGroupSave: function () {
        var strMessage = "";
        var self = $("#basicfeegroupDetail");
        var myJSON = self.getMyJSON();
        if (basicfeegroupDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Basic Fee Group", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    basicfeegroupDetail.SaveBasicFeeGroup(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_BasicFeeGroup.BasicFeeGroupSearch(response.BasicFeeGroupId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetBasicFeeGroup', true);
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
        else if (basicfeegroupDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Basic Fee Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    basicfeegroupDetail.UpdateBasicFeeGroup(myJSON, basicfeegroupDetail.params.BasicFeeGroupId).done(function (response) {
                        if (response.status != false) {
                            Admin_BasicFeeGroup.BasicFeeGroupSearch(basicfeegroupDetail.params.BasicFeeGroupId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetBasicFeeGroup', true);
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

    ValidationBasicFeeGroup: function () {
        $('#frmBasicFeeGroupDetail')
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
                      group: '.col-sm-4',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  Entity: {
                      group: '.col-sm-4',
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
           basicfeegroupDetail.BasicFeeGroupSave();
       });
    },

    LoadNextBasicFeeGroup: function (entityID, ddlNextBasicFeeGroup) {
        var EntityID = "";
        basicfeegroupDetail.FillNextBasicFeeGroup(entityID).done(function (response) {
            if (response.status != false) {
                var basicfeegroup_detail = JSON.parse(response.NextBasicFeeGroupLoad_JSON);
                $("#basicfeegroupDetail #ddlNextBasicFeeGroup").empty();
                $("#basicfeegroupDetail #ddlNextBasicFeeGroup").append($('<option/>', {
                    value: "",
                    html: "- SELECT -"
                }));
                $.each(basicfeegroup_detail, function (i, item) {
                    $("#basicfeegroupDetail #ddlNextBasicFeeGroup").append(
                        $('<option/>', {
                            value: item.BasicFeeGroupId,
                            html: item.EntityName +" - "+ item.ShortName
                        })
                    );
                });
            }
            $('#basicfeegroupDetail #ddlNextBasicFeeGroup').val(ddlNextBasicFeeGroup);
        });
    },

    SaveBasicFeeGroup: function (BasicFeeGroupData) {
        var data = "BasicFeeGroupData=" + BasicFeeGroupData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_GROUP_DETAIL", "SAVE_BASIC_FEE_GROUP");
    },

    UpdateBasicFeeGroup: function (BasicFeeGroupData, BasicFeeGroupID) {
        var data = "BasicFeeGroupData=" + BasicFeeGroupData + "&BasicFeeGroupID=" + BasicFeeGroupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_GROUP_DETAIL", "UPDATE_BASIC_FEE_GROUP");
    },

    FillBasicFeeGroup: function (BasicFeeGroupID) {
        var data = "BasicFeeGroupID=" + BasicFeeGroupID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_GROUP_DETAIL", "FILL_BASIC_FEE_GROUP");
    },

    FillNextBasicFeeGroup: function (EntityID) {
        var data = "EntityID=" + EntityID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_GROUP_DETAIL", "FILL_NEXT_BASIC_FEE_GROUP");
    },

    UpdateBasicFeeGroupActiveInactive: function (BasicFeeGroupID, IsActive) {
        var data = "BasicFeeGroupID=" + BasicFeeGroupID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_BASIC_FEE_GROUP_DETAIL", "UPDATE_BASIC_FEE_GROUP_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmBasicFeeGroupDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },

    ShowHistory: function () {
        var PanelID = 'basicfeegroupDetail';
        var ParentCtrl = 'basicfeegroupDetail';
        var ProfileName = 'Basic Fee Group';
        var DBTableName = 'BasicFeeGroup';
        var ColumnKeyId = basicfeegroupDetail.params.BasicFeeGroupId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}