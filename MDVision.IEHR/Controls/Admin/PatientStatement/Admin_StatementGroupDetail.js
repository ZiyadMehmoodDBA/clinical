StatementGroupDetail = {
    bIsFirstLoad: true,
    params: [],
    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    Load: function (params) {

        StatementGroupDetail.params = params;

        if (StatementGroupDetail.bIsFirstLoad) {
            StatementGroupDetail.bIsFirstLoad = false;


            var self = $('#StatementGroupDetail #messagesContainer');
            self.loadDropDowns(true).done(function () {
                //CacheManager.BindDropDownsByID('#StatementGroupDetail #ddlLetters', 'GetLetters', true, -1).done(function () {
                StatementGroupDetail.LoadStatementGroup();
                //});

            });


        }
    },


    ValidateStatementGroup: function () {
        $('#frmStatementGroup')
                 .bootstrapValidator({
                     live: 'disabled',
                     message: 'This value is not valid',
                     feedbackIcons: {
                         valid: 'glyphicon glyphicon-ok',
                         invalid: 'glyphicon glyphicon-remove',
                         validating: 'glyphicon glyphicon-refresh'
                     },
                     fields: {
                         name: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         //message1: {
                         //    group: '.col-sm-4',
                         //    validators: {
                         //        notEmpty: {
                         //            message: ''
                         //        }
                         //    }
                         //},
                         //message2: {
                         //    group: '.col-sm-4',
                         //    validators: {
                         //        notEmpty: {
                         //            message: ''
                         //        }
                         //    }
                         //},
                         //message3: {
                         //    group: '.col-sm-4',
                         //    validators: {
                         //        notEmpty: {
                         //            message: ''
                         //        }
                         //    }
                         //},
                         //message4: {
                         //    group: '.col-sm-4',
                         //    validators: {
                         //        notEmpty: {
                         //            message: ''
                         //        }
                         //    }
                         //},
                         //message5: {
                         //    group: '.col-sm-4',
                         //    validators: {
                         //        notEmpty: {
                         //            message: ''
                         //        }
                         //    }
                         //},
                         outstandingDays: {
                             group: '.col-sm-4',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         numberOfStatements: {
                             group: '.col-sm-4',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         letters: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         FromName: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         FromAddress: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         FromCity: {
                             group: '.col-sm-2',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         FromState: {
                             group: '.col-sm-2',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         FromZip: {
                             group: '.col-sm-1',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                       
                         RemitToName: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         RemitToAddress: {
                             group: '.col-sm-3',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         RemitToCity: {
                             group: '.col-sm-2',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         RemitToState: {
                             group: '.col-sm-2',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },

                         RemitToZip: {
                             group: '.col-sm-1',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                        
                         OfficeHoursFrom: {
                             group: '.col-sm-2',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },
                         OfficeHoursTo: {
                             group: '.col-sm-2',
                             validators: {
                                 notEmpty: {
                                     message: ''
                                 }
                             }
                         },

                         TelePhone: {
                             group: '.col-sm-2',
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
                 StatementGroupDetail.SaveStatementGroup();
             });
    },

    //OpenUploadImage: function () {
    //    var params = [];
    //    params = StatementGroupDetail.params;
    //    params["FromAdmin"] = "0";
    //    params["ParentCtrl"] = "StatementGroupDetail";
    //    LoadActionPan('uploadImage', params);
    //},
    //setImageSource: function (sourceString) {
    //    $('#frmStatementGroup  #imgStatementGroup').attr('src', sourceString);
    //   // $('#imgPatientProfile').attr('src', sourceString);
    //},


    BufferFile: function (obj) {
        var toReturn = true;

        if (obj.files && obj.files.length != 0) {
            StatementGroupDetail.ValidateUploadedFiles();
            StatementGroupDetail.FilesContainer.Files = obj.files;
            $('#frmStatementGroup').bootstrapValidator('revalidateField', 'fileupload');
            //Document_Import.FilesContainer.Name = obj.files[0].name;
            //var FileData = JSON.stringify(obj.files[0].rawFile);
            //var FileData = obj.files[0].name;
        }
        else {
            delete StatementGroupDetail.FilesContainer.Files;
            StatementGroupDetail.TruncateFileControl();
            $('#frmStatementGroup').bootstrapValidator('revalidateField', 'fileupload');
            toReturn = false;
        }
        return toReturn;

    },
    ValidateUploadedFiles: function () {
        var fileName = "";
        var files = $('#Upload_Import_file').get(0).files;
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "application/pdf" && fileType != "image/jpeg" && fileType != "image/png" && fileType != "image/jpg" && fileType != "image/gif" && fileType != "image/bmp") {
                utility.DisplayMessages("File Type is Invalid", 4);
                StatementGroupDetail.TruncateFileControl();
                return false;
            }
            if (StatementGroupDetail.ValidateFileSize(files) > Number(globalAppdata['FileSize'])) {
                utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
                StatementGroupDetail.TruncateFileControl();
                return false;
            }
            if (files[i].name.length > 256) {
                utility.DisplayMessages("File Name should have maximun 256 Characters", 4);
                StatementGroupDetail.TruncateFileControl();
                return false;
            }
            fileName = fileName + files[i].name + ',';
        }
        fileName = fileName.slice(0, fileName.length - 1);
        document.getElementById("uploadFilePH").value = fileName;
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#frmStatementGroup #imgGroupImage').attr('src', e.target.result);
        }

        reader.readAsDataURL(files[0]);
        $('#totalFiles').text(files.length + " file(s) selected");
        return true;
    },

    ValidateFileSize: function (files) {
        var size = 0;
        $.each(files, function (index, file) {
            size = size + Number((file.size / (1024 * 1024)).toFixed(2));
        });
        return size;

    },
    TruncateFileControl: function () {
        $("#" + StatementGroupDetail.params.PanelID + " #uploadFilePH").val('');
        $('#' + StatementGroupDetail.params.PanelID + ' #totalFiles').text("0 file(s) selected");
        $('#' + StatementGroupDetail.params.PanelID + ' #Upload_Import_file').val('');
    },
    LoadStatementGroup: function () {


        if (StatementGroupDetail.params.mode == "Add") {

            StatementGroupDetail.ValidateStatementGroup();
            //serialize Data.
            $('#frmStatementGroup').data('serialize', $('#frmStatementGroup').serialize());

        }
        else if (StatementGroupDetail.params.mode == "Edit") {

            StatementGroupDetail.FillStatementGroup(StatementGroupDetail.params.StatementGroupId).done(function (response) {

                if (response.status != false) {

                    var self = $("#StatementGroupDetail");
                    var StatementGroup_JSON = JSON.parse(response.StatementGroupFill_JSON);
                    if (StatementGroup_JSON.uploadFilePH != "" && StatementGroup_JSON.uploadFilePH != null) {
                        $("#totalFiles").text("1 file(s) selected");
                    }

                    utility.bindMyJSON(true, StatementGroup_JSON, false, self).done(function () {


                        //StatementGroupDetail.SupperBillLoad(response);

                        StatementGroupDetail.ValidateStatementGroup();
                        //serialize Data.
                        $('#frmStatementGroup').data('serialize', $('#frmStatementGroup').serialize());

                    });

                }
                else {
                    UnloadActionPan();
                    utility.DisplayMessages(response.Message, 3);
                }

            });

        }
    },



    FillStatementGroup: function (StatementGroupID) {

        var data = "StatementGroupID=" + StatementGroupID;
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_GROUP", "FILL_STATEMENT_GROUP");

    },

    SaveStatementGroup: function () {

        var self = $("#StatementGroupDetail");
        var myJSON = self.getMyJSON();

        if (StatementGroupDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Statement Group", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    StatementGroupDetail.SaveMessage(myJSON).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);

                            //serialize Data.
                            $('#frmStatementGroup').data('serialize', $('#frmStatementGroup').serialize());
                            StatementGroupDetail.UnLoad();
                            Admin_StatementGroup.SearchStatementGroup();
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
        else if (StatementGroupDetail.params.mode == "Edit") {

            AppPrivileges.GetFormPrivileges("Statement Group", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    StatementGroupDetail.UpdateStatementGroup(myJSON).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);

                            //serialize Data.
                            $('#frmStatementGroup').data('serialize', $('#frmStatementGroup').serialize());
                            StatementGroupDetail.UnLoad();
                            Admin_StatementGroup.SearchStatementGroup();
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

    SaveMessage: function (StatementGroupData) {

        var data = "StatementGroupData=" + StatementGroupData;
        // save parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_GROUP", "SAVE_STATEMENT_GROUP");
    },

    UpdateStatementGroup: function (StatementGroupData) {

        var data = "StatementGroupData=" + StatementGroupData;
        // update parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_STATEMENT_GROUP", "UPDATE_STATEMENT_GROUP");
    },

    UnLoad: function () {

        utility.UnLoadDialog("frmStatementGroup", function () {
            UnloadActionPan();
        }, function () {
            UnloadActionPan();
        });
    },

    ShowHistory: function () {
        var PanelID = 'StatementGroupDetail';
        var ParentCtrl = 'StatementGroupDetail';
        var ProfileName = 'Statement Group';
        var DBTableName = 'PatientStatementGroup';
        var ColumnKeyId = StatementGroupDetail.params.StatementGroupId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
};

