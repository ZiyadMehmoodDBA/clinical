folderTypeDetail = {
    params: [],
    Load: function (params) {
        folderTypeDetail.params = params;
        
        var self = null;
        if (folderTypeDetail.params.PanelID == "folderTypeDetail")
            self = $('#folderTypeDetail');
        else
            self = $('#' + folderTypeDetail.params.PanelID + ' #folderTypeDetail');

        if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
            self.find("#ddlEntity").attr('disabled', 'disabled');
        }
        self.loadDropDowns(true).done(function () {
            if (globalAppdata.AppUserName.toLowerCase() != "mdvision") {
                self.find("#ddlEntity").val(globalAppdata["SeletedEntityId"]);
            }
            folderTypeDetail.LoadFolderType();
        });
        
    },

    LoadFolderType: function () {
        if (folderTypeDetail.params.mode == "Add") {

            //serialize data
            $('#frmfolderTypeDetail').data('serialize', $('#frmfolderTypeDetail').serialize());
            folderTypeDetail.ValidateFolderType();
        }
        else if (folderTypeDetail.params.mode == "Edit") {
            folderTypeDetail.FillFolderType(folderTypeDetail.params.FolderTypeId).done(function (response) {
                if (response.status != false) {
                    var foldertype_detail = JSON.parse(response.FolderTypeDetail_JSON);
                    var self = $("#folderTypeDetail");
                    utility.bindMyJSON(true, foldertype_detail, false, self).done(function () {

                        if (foldertype_detail.chkActive == 'True')
                            $("#folderTypeDetail #chkActive").attr("checked", true);
                        else
                            $("#folderTypeDetail #chkActive").attr("checked", false);

                        //serialize data
                        $('#frmfolderTypeDetail').data('serialize', $('#frmfolderTypeDetail').serialize());
                        folderTypeDetail.ValidateFolderType();
                    });
                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateFolderType: function () {
        $('#frmfolderTypeDetail')
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
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    entity: {
                        group: '.col-sm-3',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                }
            })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            folderTypeDetail.FolderTypeSave();
        });
    },

    FolderTypeSave: function () {
        var strMessage = "";
        var self = $("#folderTypeDetail");
        var myJSON = self.getMyJSON();
        if (folderTypeDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("FolderType", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    folderTypeDetail.SaveFolderType(myJSON).done(function (response) {
                        if (response.status != false) {
                            folderTypeDetail.params["FolderTypeId"] = response.FolderTypeId;
                            utility.DisplayMessages(response.message, 1);
                            Admin_FolderType.FolderTypeSearch(response.FolderTypeId);
                            UnloadActionPan(folderTypeDetail.params["ParentCtrl"], "folderTypeDetail");
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
        else if (folderTypeDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("FolderType", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    folderTypeDetail.UpdateFolderType(myJSON, folderTypeDetail.params.FolderTypeId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                           
                            
                            Admin_FolderType.FolderTypeSearch(folderTypeDetail.params.FolderTypeId);
                                UnloadActionPan(folderTypeDetail.params["ParentCtrl"], "folderTypeDetail");
                            

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
        CacheManager.BindCodes('GetDocumentType', true);
    },

    SaveFolderType: function (FolderTypeData, DocTypeId) {
        var data = "FolderTypeData=" + FolderTypeData + "&DocTypeId=" + DocTypeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER_TYPE_DETAIL", "SAVE_FOLDER_TYPE");
    },

    UpdateFolderType: function (FolderTypeData, DocTypeId) {
        var data = "FolderTypeData=" + FolderTypeData + "&DocTypeId=" + DocTypeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER_TYPE_DETAIL", "UPDATE_FOLDER_TYPE");
    },

    FillFolderType: function (DocTypeId) {
        var data = "DocTypeId=" + DocTypeId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER_TYPE_DETAIL", "FILL_FOLDER_TYPE");
    },

    UpdateFolderTypeActiveInactive: function (DocTypeId, IsActive) {
        var data = "DocTypeId=" + DocTypeId + "&IsActive=" + IsActive;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER_TYPE_DETAIL", "UPDATE_FOLDER_TYPE_ACTIVE_INACTIVE");
    },

    //FillCityState: function (zipcode, cityname, statename) {
    //    var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE");
    //},

    UnLoad: function () {

        utility.UnLoadDialog("frmfolderTypeDetail", function () {
            UnloadActionPan(folderTypeDetail.params["ParentCtrl"], "folderTypeDetail");
        }, function () {
            UnloadActionPan(folderTypeDetail.params["ParentCtrl"], "folderTypeDetail");
        });

    },
    
    ShowHistory: function () {
        var PanelID = 'folderTypeDetail';
        var ParentCtrl = 'folderTypeDetail';
        var ProfileName = 'FolderType';
        var DBTableName = 'DocumentType';
        var ColumnKeyId = folderTypeDetail.params.FolderTypeId;
        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
}