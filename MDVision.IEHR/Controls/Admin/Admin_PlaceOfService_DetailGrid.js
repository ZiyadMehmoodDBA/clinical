placeofserviceDetailGrid = {
    params: [],
    Load: function (params) {
        placeofserviceDetailGrid.params = params;
        
        var self = $('#placeofserviceDetailGrid');
        self.loadDropDowns(true).done(function() {
            
            placeofserviceDetailGrid.LoadPOSPlan();
        });
    },

    LoadPOSPlan: function () {
        if (placeofserviceDetailGrid.params.mode == "Add") {
            placeofserviceDetailGrid.ValidatePOSPlan();
        }
        else if (placeofserviceDetailGrid.params.mode == "Edit") {
            placeofserviceDetailGrid.FillPOSPlan(placeofserviceDetailGrid.params.POSPlanId).done(function (response) {
                if (response.status != false) {
                    var posPlan_detail = JSON.parse(response.POSPlanFill_JSON);
                    var self = $("#placeofserviceDetailGrid");
                    utility.bindMyJSON(true, posPlan_detail, false, self);
                    placeofserviceDetailGrid.ValidatePOSPlan();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidatePOSPlan: function () {
        $('#frmplaceofserviceDetailGrid')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Plan: {
                       group: '.col-md-12',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   POS: {
                       group: '.col-md-12',
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
            placeofserviceDetailGrid.POSPlanSave();
        });
    },

    POSPlanSave: function () {
        var strMessage = "";
        var self = $("#placeofserviceDetailGrid");
        var myJSON = self.getMyJSON();
        if (placeofserviceDetailGrid.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Place Of Service", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    placeofserviceDetailGrid.SavePOSPlan(myJSON).done(function (response) {
                        if (response.status != false) {
                            placeOfServiceDetail.LoadPlaceOfServicePlanInfo().done(function (response) {
                                if (response.status != false) {
                                    placeOfServiceDetail.POSPlanGridLoad(response);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                            utility.DisplayMessages(response.message, 1);
                            //UnloadActionPan();
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
        else if (placeofserviceDetailGrid.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Place Of Service", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    placeofserviceDetailGrid.UpdatePOSPlan(myJSON, placeofserviceDetailGrid.params.POSPlanId, 1).done(function (response) {
                        if (response.status != false) {
                            placeOfServiceDetail.LoadPlaceOfServicePlanInfo().done(function (response) {
                                if (response.status != false) {
                                    placeOfServiceDetail.POSPlanGridLoad(response);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                            utility.DisplayMessages(response.message, 1);
                            //UnloadActionPan();
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

    SavePOSPlan: function (POSPlanData) {
        var data = "POSPlanData=" + POSPlanData + "&PlaceOfServiceID=" + placeOfServiceDetail.params.PlaceOfServiceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "SAVE_POS_PLAN_INFO");
    },

    UpdatePOSPlan: function (POSPlanData, POSPlanID) {
        var data = "POSPlanData=" + POSPlanData + "&POSPlanID=" + POSPlanID + "&PlaceOfServiceID=" + placeOfServiceDetail.params.PlaceOfServiceId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "UPDATE_POS_PLAN_INFO");
    },

    FillPOSPlan: function (POSPlanID) {
        var data = "POSPlanID=" + POSPlanID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PLACE_OF_SERVICE_DETAIL", "FILL_POS_PLAN_INFO");
    },

    UnLoad: function () {
        UnloadActionPan();
    },
}