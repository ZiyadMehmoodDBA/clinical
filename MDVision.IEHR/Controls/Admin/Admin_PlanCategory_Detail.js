planCategoryDetail = {
    params: [],
    Load: function (params) {
        planCategoryDetail.params = params;
        var self = null;
        if (planCategoryDetail.params.PanelID == 'tblplanCategoryDetail')
            self = $('#tblplanCategoryDetail');
        else
            self = $('#' + planCategoryDetail.params.PanelID + ' #tblplanCategoryDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {           
            //if (globalAppdata['IsAdmin'] != "True") {
            //    $("#tblplanCategoryDetail #divPlanCategory_Entity").css("display", "none");
            //    $("#tblplanCategoryDetail #ddlEntity").val(globalAppdata["SeletedEntityId"]);
            //}
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            planCategoryDetail.LoadPlanCategory();
        });
               
    },

    LoadPlanCategory: function () {
        if (planCategoryDetail.params.mode == "Add") {
            $('#planCategoryDetail #txtShortName').attr("enabled", "enabled");

            //Serialize data.
            $('#frmPlanCategoryDetail').data('serialize', $('#frmPlanCategoryDetail').serialize());
            planCategoryDetail.ValidatePlanCategory();
        }
        else if (planCategoryDetail.params.mode == "Edit") {
            $('#planCategoryDetail #txtShortName').attr("disabled", "disabled");
            planCategoryDetail.FillPlanCategory(planCategoryDetail.params.PlanCategoryId).done(function (response) {
                if (response.status != false) {
                    var planCategory_detail = JSON.parse(response.PlanCategoryFill_JSON);
                    var self = $("#planCategoryDetail");
                    utility.bindMyJSON(true, planCategory_detail, false, self).done(function () {


                        if (planCategory_detail.chkActive == 'True')
                            $("#planCategoryDetail #chkActive").attr("checked", true);
                        else
                            $("#planCategoryDetail #chkActive").attr("checked", false);

                        planCategoryDetail.ValidatePlanCategory();
                        //Serialize data.
                        $('#frmPlanCategoryDetail').data('serialize', $('#frmPlanCategoryDetail').serialize());

                    });

                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidatePlanCategory: function () {
        $('#frmPlanCategoryDetail')
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
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Entity: {
                       group: '.col-md-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            planCategoryDetail.PlanCategorySave();
        });
    },

    PlanCategorySave: function () {
        var strMessage = "";
        var self = $("#planCategoryDetail");
        var myJSON = self.getMyJSON();
        if (planCategoryDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Plan Category", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    planCategoryDetail.SavePlanCategory(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_PlanCategory.PlanCategorySearch(response.PlanCategoryId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetPlanCategory', true);
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
        else if (planCategoryDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Plan Category", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    planCategoryDetail.UpdatePlanCategory(myJSON, planCategoryDetail.params.PlanCategoryId).done(function (response) {
                        if (response.status != false) {
                            Admin_PlanCategory.PlanCategorySearch(planCategoryDetail.params.PlanCategoryId);
                            utility.DisplayMessages(response.message, 1);
                            UnloadActionPan();
                            CacheManager.BindCodes('GetPlanCategory', true);
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

    SavePlanCategory: function (PlanCategoryData) {
        var data = "PlanCategoryData=" + PlanCategoryData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_CATEGORY_DETAIL", "SAVE_PLAN_CATEGORY");
    },

    UpdatePlanCategory: function (PlanCategoryData, PlanCategoryID) {
        var data = "PlanCategoryData=" + PlanCategoryData + "&PlanCategoryID=" + PlanCategoryID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_CATEGORY_DETAIL", "UPDATE_PLAN_CATEGORY");
    },

    FillPlanCategory: function (PlanCategoryID) {
        var data = "PlanCategoryID=" + PlanCategoryID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_CATEGORY_DETAIL", "FILL_PLAN_CATEGORY");
    },

    UpdatePlanCategoryActiveInactive: function (PlanCategoryID, IsActive) {
        var data = "PlanCategoryID=" + PlanCategoryID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLAN_CATEGORY_DETAIL", "UPDATE_PLAN_CATEGORY_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmPlanCategoryDetail", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },

    ShowHistory: function () {
        var PanelID = 'planCategoryDetail';
        var ParentCtrl = 'planCategoryDetail';
        var ProfileName = 'Plan Category';
        var DBTableName = 'PlanCategory';
        var ColumnKeyId = planCategoryDetail.params.PlanCategoryId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}