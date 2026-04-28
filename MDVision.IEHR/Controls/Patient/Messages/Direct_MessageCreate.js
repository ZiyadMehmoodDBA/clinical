Direct_MessageCreate = {
    bIsFirstLoad: true,
    params: [],
    TemplateContent: "",
    createdby: "",
    UniqueNumber: "",
    UserMessageId: "",
    PatientLetterId: "",
    LetterTemplateName: "",
    LetterStatus: "",
    AttachPatientId: "",
    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    Load: function (params) {
        Direct_MessageCreate.params = params;
        Direct_MessageCreate.FilesContainer = { Files: [], Name: "Uploaded_Document" };
        if (Direct_MessageCreate.params["PanelID"] != 'pnlDirectMessageCreate') {
            Direct_MessageCreate.params["PanelID"] = Direct_MessageCreate.params["PanelID"] + ' #pnlDirectMessageCreate';
        }
        $('#frmDirectMessageCreate').data('serialize', $('#frmDirectMessageCreate').serialize());

        //$("#" + Direct_MessageCreate.params["PanelID"] + " #password").prop("disabled", true);
        $("#" + Direct_MessageCreate.params["PanelID"] + " #btnSend").prop("disabled", true);

    },

    enabledisablesend: function (obj) {
        var toEmail = $(obj).val();
        if (toEmail) {
            var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            var email = regex.test(toEmail);
            if (email) {
                $('#pnlDirectMessageCreate #btnSend').prop("disabled", false);
            }
        }
    },
    SaveCreateMessage: function () {

        var dfd = new $.Deferred();
        var files = [];
        var FilePath = [];
        var FileType = [];
        var count = 0;

        if (Direct_MessageCreate.FilesContainer.Files != undefined) {
            if (Direct_MessageCreate.FilesContainer.Files.length > 0) {
                $.each(Direct_MessageCreate.FilesContainer.Files, function (key, value) {
                    var oFReader = new FileReader();
                    oFReader.readAsDataURL(Direct_MessageCreate.FilesContainer.Files[key]);
                    oFReader.onload = function (oFREvent) {
                        var file_ = oFREvent.target.result.split('base64,');
                        files.push(file_[1]);
                        FilePath.push(Direct_MessageCreate.FilesContainer.Files[key].name);
                        FileType.push(Direct_MessageCreate.FilesContainer.Files[key].type);
                        count++;
                        if (Direct_MessageCreate.FilesContainer.Files.length == count)
                            dfd.resolve('ok');
                    };

                });
            }
            else if (Direct_MessageCreate.FilesContainer.Files.length == 0) {
                dfd.resolve('ok');
            }
        }

        dfd.then(function () {

            var strMessage = "";
            var self = $('#' + Direct_MessageCreate.params["PanelID"]);
            var myJSON = self.getMyJSONByName();

            if (Direct_MessageCreate.params.mode = "Add") {
                var filestype = JSON.stringify(FileType);
                var filespath = JSON.stringify(FilePath);
                Direct_MessageCreate.SaveCreateMessage_DBCall(myJSON, files, filestype, filespath).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages("Successfully sent!", 1);
                        UnloadActionPan(Direct_MessageCreate.params.ParentCtrl, 'pnlDirectMessageCreate');
                        DashBoard.DashBoardOutgoingDirectMessagesSearch();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        });
    },

    SaveCreateMessage_DBCall: function (messagesData, filedata, filetype, filepath) {
        var objData = JSON.parse(messagesData);
        objData["CommandType"] = "save_direct_message";
        objData["MessageType"] = Direct_MessageCreate.params.MessageType;
        objData["Files"] = filedata;
        objData["FileType"] = filetype;
        objData["FilePath"] = filepath;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Messages", "Messages");

    },

    UnLoad: function () {

        utility.UnLoadDialog('frmPatientMessageCreate', function () {
            if (Direct_MessageCreate.params != null && Direct_MessageCreate.params.ParentCtrl != null) {
                UnloadActionPan(Direct_MessageCreate.params.ParentCtrl, 'pnlDirectMessageCreate');
            }
            else
                UnloadActionPan(null, 'pnlDirectMessageCreate');
        }, function () {
            if (Direct_MessageCreate.params != null && Direct_MessageCreate.params.ParentCtrl != null) {
                UnloadActionPan(Direct_MessageCreate.params.ParentCtrl, 'pnlDirectMessageCreate');
            }
            else
                UnloadActionPan(null, 'pnlDirectMessageCreate');
        });

        if ((Direct_MessageCreate.params.ParentCtrl == "mstrTabDashBoard" && Direct_MessageCreate.params.Caller != "Referrals") || Direct_MessageCreate.params.ParentCtrl == "patTabUserMessages" || Direct_MessageCreate.params.ParentCtrl == "Patient_UserMessagesQuickLink") {
            if (Direct_MessageCreate.params.MessageType == "Patient") {
                if (Direct_MessageCreate.params.FromPatModule == "1") {
                    Patient_UserMessages.SearchPatientMessage();
                } else {
                    if (Direct_MessageCreate.params.FromQuicklink == "1") {
                        Patient_UserMessagesQuickLink.SearchPracticeMessage();
                    }
                    DashBoard.DashBoardPatientMessagesSearch();
                }

            } else {
                if (Direct_MessageCreate.params.FromPatModule == "1") {
                    Patient_UserMessages.SearchPracticeMessage();
                } else {
                    if (Direct_MessageCreate.params.FromQuicklink == "1") {
                        Patient_UserMessagesQuickLink.SearchPracticeMessage();
                    }
                    DashBoard.DashBoardMessagesSearch();
                }
            }
        }
    },

    BufferFile: function (obj) {
        event.stopPropagation();
        var toReturn = true,
            nameHtml;

        if (!(Direct_MessageCreate.FilesContainer.Files.length == 5) && !(Direct_MessageCreate.FilesContainer.Files.length + obj.files.length > 5) && obj.files && obj.files.length != 0 && obj.files.length <= 5) {
            if (Direct_MessageCreate.ValidateUploadedFiles()) {
                for (var i = 0; i < obj.files.length; i++) {
                    Direct_MessageCreate.FilesContainer.Files.push(obj.files[i]);
                    nameHtml = "'" + obj.files[i].name + "'";
                    var fileType = obj.files[i].type;

                    $("#divfilesicons").append('<div class="col-sm-4 mt-xs pl-none"><span class="btn btn-success btn-xs btn-and-anchor size100per"><i class="fa fa-file pull-left mt-tiny"></i>' + "<span class='size-max90per ellipses pull-left'> " + obj.files[i].name + '</span><a id="' + i + '"href="#"  onclick="Direct_MessageCreate.DeleteFile(this);"><i class="fa fa-times"></i> </a></span></div>');
                }
            }
        }
        else {
            utility.DisplayMessages("Cannot attach more than 5 files.", 3)
            toReturn = false;
        }
        return toReturn;

    },

    DeleteFile: function (obj) {
        var selectedFile = $(obj).parent().text();
        var filteredArray = Direct_MessageCreate.FilesContainer.Files.filter(function (a) { return a.name != $(obj).parent().text().trim() });
        Direct_MessageCreate.FilesContainer.Files = filteredArray;
        $(obj).parent().parent().remove();
        var relatedXml = "#pnlDirectMessageCreate #xml_" + selectedFile.replace(/[\W_]/g, "");
        $(relatedXml).remove();
        $('input[type="file"]').val(null);
        //if (Direct_MessageCreate.FilesContainer.Files.length == 0) {
        //    $("#" + Direct_MessageCreate.params["PanelID"] + " #password").prop("disabled", true);
        //}
    },
    GenerateUUID: function () {
        var d = new Date().getTime();
        if (window.performance && typeof window.performance.now === "function") {
            d += performance.now(); //use high-precision timer if available
        }
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    },
    ValidateUploadedFiles: function () {
        var fileName = "";
        var size = 0;
        var files = $('#Upload_Import_file').get(0).files;
        $.each(Direct_MessageCreate.FilesContainer.Files, function (index, file) {
            size = size + Number((Direct_MessageCreate.FilesContainer.Files[index].size / 1024 * 1024).toFixed(2));

        });
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "application/xml" && fileType != "text/xml" && fileType != "application/pdf" && fileType != "application/x-zip-compressed" && fileType != "text/html" && fileType != "text/plain") {
                utility.DisplayMessages("File Type is Invalid", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            if (Document_Import.ValidateFileSize(files) > Number(globalAppdata['FileSize']) || size > (1048576 * Number(globalAppdata['FileSize']))) {
                utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
                Document_Import.TruncateFileControl();
                return false;
            }

            if (Direct_MessageCreate.isFileAlreadyAttached(files[i].name)) {
                utility.DisplayMessages("File already attached with same name!", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            if (files[i].name.length > 256) {
                utility.DisplayMessages("File Name should have maximun 256 Characters", 4);
                Document_Import.TruncateFileControl();
                return false;
            }
            fileName = fileName + files[i].name + ',';
        }
        fileName = fileName.slice(0, fileName.length - 1);
        document.getElementById("uploadFilePH").value = fileName;
        $('#totalFiles').text(files.length + " file(s) selected");
        //$("#" + Direct_MessageCreate.params["PanelID"] + " #password").prop("disabled", false);
        return true;
    },

    isFileAlreadyAttached: function (fileName) {
        for (var i = 0; i < Direct_MessageCreate.FilesContainer.Files.length; i++) {
            if (Direct_MessageCreate.FilesContainer.Files[i].name == fileName)
                return true;
        }
        return false;
    },

    TruncateFileControl: function () {
        $("#" + Direct_MessageCreate.params.PanelID + " #uploadFilePH").val('');
        $('#' + Direct_MessageCreate.params.PanelID + ' #totalFiles').text("0 file(s) selected");
        $('#' + Direct_MessageCreate.params.PanelID + ' #Upload_Import_file').val('');
    },

}