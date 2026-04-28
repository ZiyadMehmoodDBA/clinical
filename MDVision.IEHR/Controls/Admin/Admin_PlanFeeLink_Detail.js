planfeelinkDetail = {
    params: [],
    Enable: false,
    Load: function (params) {
        planfeelinkDetail.params = params;
        
        var self = null;
        if (planfeelinkDetail.params.PanelID == "planfeelinkDetail")
            self = $('#planfeelinkDetail');
        else
            self = $('#' + planfeelinkDetail.params.PanelID + ' #planfeelinkDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {

            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#planfeelinkDetail #divPlanFeeLink_Entity").css("display", "none");
            //    $("#planfeelinkDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }

            $("#planfeelinkDetail #chkIsActive").attr("checked", true);
            planfeelinkDetail.LoadPlanFeeLink();
            
        });
               
    },

    LoadPlanFeeLink: function () {
        if (planfeelinkDetail.params.mode == "Add") {


            planfeelinkDetail.ValidationPlanFeeLink();
            //serialize data
            $('#frmPlanFeeLinkDetail').data('serialize', $('#frmPlanFeeLinkDetail').serialize());
            
        }
        else if (planfeelinkDetail.params.mode == "Edit") {
            planfeelinkDetail.FillPlanFeeLink(planfeelinkDetail.params.PlanFeeLinkId).done(function (response) {
                if (response.status != false) {
                    var planfeelink_detail = JSON.parse(response.PlanFeeLinkFill_JSON);
                    var self = $("#planfeelinkDetail");
                    
                    utility.bindMyJSON(true, planfeelink_detail, false, self).done(function () {

                        if (planfeelink_detail.ChkIsActive == 'True') {
                            $("#planfeelinkDetail #chkIsActive").attr("checked", true);
                        }
                        else {
                            $("#planfeelinkDetail #chkIsActive").attr("checked", false);
                        }

                        planfeelinkDetail.ValidationPlanFeeLink();
                        //serialize data
                        $('#frmPlanFeeLinkDetail').data('serialize', $('#frmPlanFeeLinkDetail').serialize());

                    });
                    
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    PlanFeeLinkSave: function () {
        var strMessage = "";
        var self = $("#planfeelinkDetail");
        var myJSON = self.getMyJSON();
        if (planfeelinkDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Plan Fee Link", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    planfeelinkDetail.SavePlanFeeLink(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_PlanFeeLink.PlanFeeLinkSearch(response.PlanFeeLinkId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetPlanFeeLink', true);
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
        else if (planfeelinkDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Plan Fee Link", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    planfeelinkDetail.UpdatePlanFeeLink(myJSON, planfeelinkDetail.params.PlanFeeLinkId).done(function (response) {
                        if (response.status != false) {
                            Admin_PlanFeeLink.PlanFeeLinkSearch(planfeelinkDetail.params.PlanFeeLinkId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetPlanFeeLink', true);
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

    ValidationPlanFeeLink: function () {
        $('#frmPlanFeeLinkDetail')
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
                      group: '.col-xs-5',
                      validators: {
                          notEmpty: {
                              message: ''
                          },
                      }
                  },
                  entity: {
                      group: '.col-xs-4',
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
           planfeelinkDetail.PlanFeeLinkSave();
       });
    },

    SavePlanFeeLink: function (PlanFeeLinkData) {
        var data = "PlanFeeLinkData=" + PlanFeeLinkData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_FEE_LINK_DETAIL", "SAVE_PLAN_FEE_LINK");
    },

    UpdatePlanFeeLink: function (PlanFeeLinkData, PlanFeeLinkID) {
        var data = "PlanFeeLinkData=" + PlanFeeLinkData + "&PlanFeeLinkID=" + PlanFeeLinkID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_FEE_LINK_DETAIL", "UPDATE_PLAN_FEE_LINK");
    },

    FillPlanFeeLink: function (PlanFeeLinkID) {
        var data = "PlanFeeLinkID=" + PlanFeeLinkID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_FEE_LINK_DETAIL", "FILL_PLAN_FEE_LINK");
    },

    UpdatePlanFeeLinkActiveInactive: function (PlanFeeLinkID, IsActive) {
        var data = "PlanFeeLinkID=" + PlanFeeLinkID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_FEE_LINK_DETAIL", "UPDATE_PLAN_FEE_LINK_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmPlanFeeLinkDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },
    ShowHistory: function () {
        var PanelID = 'planfeelinkDetail';
        var ParentCtrl = 'planfeelinkDetail';
        var ProfileName = 'Plan Fee Link';
        var DBTableName = 'PlanFeeLink';
        var ColumnKeyId = planfeelinkDetail.params.PlanFeeLinkId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}