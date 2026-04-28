resourcesDetail = {
    params: [],
    Load: function (params) {
        resourcesDetail.params = params;
        
        var self = $('#tblresourcesDetail');
        self.loadDropDowns(true).done(function () {

            resourcesDetail.LoadResources();
           
        });
        resourcesDetail.LoadAutoComplete();
    },
    LoadAutoComplete :function(){
        CacheManager.BindCodes('GetProvider', false).done(function (result) {
            var Ctrl = $("#frmResourcesDetail #txtResourceProvider");
            var hfCtrl = $("#" + resourcesDetail.params.PanelID + " #hfResourceProvider");
            var onSelect = function (e) { $("#" + resourcesDetail.params.PanelID + " #txtResourceProvider").attr("ProviderId", e.id); };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, onSelect);
        });
    },
    LoadResources: function () {
        if (resourcesDetail.params.mode == "Add") {
            $('#resourcesDetail #txtShortName').attr("enabled", "enabled");
            $('#resourcesDetail #frmResourcesDetail #lnkResourceProviderEdit').css("display", "none");
            $('#resourcesDetail #frmResourcesDetail #lblResourceProvider').css("display", "inline");
            //Serialize data.
            $('#frmResourcesDetail').data('serialize', $('#frmResourcesDetail').serialize());
            resourcesDetail.ValidateResources();
        }
        else if (resourcesDetail.params.mode == "Edit") {
            $('#resourcesDetail #txtShortName').attr("disabled", "disabled");
            resourcesDetail.FillResources(resourcesDetail.params.ResourceId).done(function (response) {
                if (response.status != false) {
                    var resources_detail = JSON.parse(response.ResourcesFill_JSON);
                    if (resources_detail.hfResourceProvider > 0 && resources_detail.txtResourceProvider != "")
                    {
                        $('#resourcesDetail #frmResourcesDetail #lblResourceProvider').css("display", "none");
                        $('#resourcesDetail #frmResourcesDetail #lnkResourceProviderEdit').css("display", "inline");
                    }
                    var self = $("#resourcesDetail");
                    utility.bindMyJSON(true, resources_detail, false, self).done(function () {
                    
                        if (resources_detail.chkProviderRequired == 'True')
                            $("#resourcesDetail #chkProviderRequired").attr("checked", true);
                        else
                            $("#resourcesDetail #chkProviderRequired").attr("checked", false);

                        resourcesDetail.ValidateResources();
                        //Serialize data.
                        $('#frmResourcesDetail').data('serialize', $('#frmResourcesDetail').serialize());

                    });
                    
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateResources: function () {
        $('#frmResourcesDetail')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   shortName: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Facility: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           }).on('success.form.bv', function (e) {
               e.preventDefault();
               resourcesDetail.ResourcesSave();
           });
    },

    ResourcesSave: function () {
        var strMessage = "";
        var self = $("#resourcesDetail");
        var myJSON = self.getMyJSON();
        if (resourcesDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Resources", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    resourcesDetail.SaveResources(myJSON).done(function (response) {
                        if (response.status != false) {
                            Admin_Resources.ResourcesSearch(response.ResourceId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetResources', true);
                            UnloadActionPan(resourcesDetail.params["ParentCtrl"]);
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
        else if (resourcesDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Resources", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    resourcesDetail.UpdateResources(myJSON, resourcesDetail.params.ResourceId, 1).done(function (response) {
                        if (response.status != false) {
                            //Admin_Resources.ResourcesSearch(resourcesDetail.params.ResourceId);
                            utility.DisplayMessages(response.message, 1);
                            CacheManager.BindCodes('GetResources', true);
                            if (resourcesDetail.params.RefCtrl != null) {
                                UnloadActionPan(resourcesDetail.params["ParentCtrl"], "resourcesDetail");
                            }
                            else {
                                Admin_Resources.ResourcesSearch(resourcesDetail.params.ResourceId);
                                UnloadActionPan(resourcesDetail.params["ParentCtrl"], "resourcesDetail");
                            }
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

    SaveResources: function (ResourcesData) {
        var data = "ResourcesData=" + ResourcesData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCES_DETAIL", "SAVE_RESOURCES");
    },

    UpdateResources: function (ResourcesData, resourceID, IsActive) {
        var data = "ResourcesData=" + ResourcesData + "&ResourceID=" + resourceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCES_DETAIL", "UPDATE_RESOURCES");
    },

    FillResources: function (ResourceID) {
        var data = "ResourceID=" + ResourceID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCES_DETAIL", "FILL_RESOURCES");
    },

    UpdateResourceActiveInactive: function (ResourceID, IsActive) {
        var data = "ResourceID=" + ResourceID + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_RESOURCES_DETAIL", "UPDATE_RESOURCE_ACTIVE_INACTIVE");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmResourcesDetail", function () {
            UnloadActionPan(resourcesDetail.params["ParentCtrl"], "resourcesDetail");
        }, function () {
            UnloadActionPan(resourcesDetail.params["ParentCtrl"], "resourcesDetail");
        });


    },
    OpenProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];

        params["IsOptional"] = true;
        params["RefForm"] = 'frmResourcesDetail';
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'resourcesDetail';

        LoadActionPan('Admin_Provider', params);
    },
    OpenProviderDetail: function (HiddenCtrl, TxtBoxCtrl) {

        var params = [];
        params["ProviderId"] = $('#' + resourcesDetail.params["PanelID"] + ' #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = TxtBoxCtrl;
        params["ParentCtrl"] = 'resourcesDetail';
        LoadActionPan('providerDetail', params);
    },

    ShowHistory: function () {
        var PanelID = 'resourcesDetail';
        var ParentCtrl = 'resourcesDetail';
        var ProfileName = 'Resources';
        var DBTableName = 'Resources';
        var ColumnKeyId = resourcesDetail.params.ResourceId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);

    },
}