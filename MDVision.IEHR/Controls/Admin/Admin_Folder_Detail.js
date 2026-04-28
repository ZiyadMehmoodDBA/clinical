folderDetail = {
    params: [],
    Load: function (params) {
        folderDetail.params = params;
        var self = $('#folderDetail');
        self.loadDropDowns(true).done(function () {

            folderDetail.LoadFolder();
        });
        
    },

    LoadFolder: function () {
        if (folderDetail.params.mode == "Add") {

            //serialize data
            $('#frmfolderDetail').data('serialize', $('#frmfolderDetail').serialize());
            folderDetail.ValidateFolder();
        }
        else if (folderDetail.params.mode == "Edit") {
            folderDetail.FillFolder(folderDetail.params.FolderId).done(function (response) {
                if (response.status != false) {
                    var document_detail = JSON.parse(response.DocumentFill_JSON);
                    var self = $("#folderDetail");
                    utility.bindMyJSON(true, document_detail, false, self).done(function () {
                                                        

                        if (document_detail.chkActive == 'True')
                            $("#folderDetail #chkActive").attr("checked", true);
                        else
                            $("#folderDetail #chkActive").attr("checked", false);

                        if (Admin_Folder.SystemGeneratedFolderList(document_detail.txtName)) {
                            $('#folderDetail #txtName').attr("disabled", "disabled");
                        }
                        //serialize data
                        $('#frmfolderDetail').data('serialize', $('#frmfolderDetail').serialize());
                        folderDetail.ValidateFolder();

                    });
                    
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateFolder: function () {
        $('#frmfolderDetail')
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
                    Type: {
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
            folderDetail.FolderSave();
        });
    },

    FolderSave: function () {
        var strMessage = "";
        var self = $("#folderDetail");
        var myJSON = self.getMyJSON();
        if (folderDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Folder", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    folderDetail.SaveFolder(myJSON).done(function (response) {
                        if (response.status != false) {
                            folderDetail.params["FolderID"] = response.DocumentId;
                            utility.DisplayMessages(response.message, 1);
                            //CacheManager.BindCodes('GetDocument', true);
                            if (folderDetail.params.PanelID != null && (folderDetail.params.PanelID == "pnlPatientDocument" || folderDetail.params.PanelID == "pnlBatchDocuments")) {
                                CacheManager.BindListCodes('#' + Patient_Document.params["PanelID"] + ' #lstDocument', 'GetDocument', true);
                                Patient_Document.LoadFolders();
                            }
                            else
                                Admin_Folder.FolderSearch(response.DocumentId);
                            
                            if (folderDetail.params.ParentCtrl != null) {
                                UnloadActionPan(folderDetail.params["ParentCtrl"], "folderDetail");
                            }
                            else {
                                UnloadActionPan(null, "folderDetail");
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
        else if (folderDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Folder", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    folderDetail.UpdateFolder(myJSON, folderDetail.params.FolderId).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            //CacheManager.BindCodes('GetDocument', true);
                            Admin_Folder.FolderSearch(folderDetail.params.FolderId);
                            UnloadActionPan(folderDetail.params["ParentCtrl"], "folderDetail");
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

    SaveFolder: function (FolderData) {
        var data = "FolderData=" + FolderData;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER", "SAVE_FOLDER");
    },

    UpdateFolder: function (FolderData, FolderID) {
        var data = "FolderData=" + FolderData + "&FolderID=" + FolderID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER", "UPDATE_FOLDER");
    },

    FillFolder: function (FolderID) {
        var data = "FolderID=" + FolderID;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_FOLDER", "FILL_FOLDER");
    },

    

    //FillCityState: function (zipcode, cityname, statename) {
    //    var data = "zipcode=" + zipcode + "&cityname" + cityname + "&statename" + statename;
    //    // serach parameter , class name, command name of class 
    //    return MDVisionService.defaultService(data, "CITY_STATE_CONFIG", "CITYSTATE");
    //},

    UnLoad: function () {

        utility.UnLoadDialog("frmfolderDetail", function () {
            UnloadActionPan(folderDetail.params["ParentCtrl"], "folderDetail");
        }, function () {
            UnloadActionPan(folderDetail.params["ParentCtrl"], "folderDetail");
        });
    },
    ShowHistory: function () {
        var PanelID = 'folderDetail';
        var ParentCtrl = 'folderDetail';
        var ProfileName = 'Folder';
        var DBTableName = 'Documents';
        var ColumnKeyId = folderDetail.params.FolderId;
        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },

}