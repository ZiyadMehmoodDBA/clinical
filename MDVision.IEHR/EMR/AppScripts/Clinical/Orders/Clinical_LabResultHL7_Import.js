Clinical_LabResultHL7_Import = {
    params: [],
    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    Load: function (params) {
        Clinical_LabResultHL7_Import.params = params;
    },

    BufferFile: function (obj) {
        var toReturn = true;
        if (obj.files && obj.files.length != 0) {
            Clinical_LabResultHL7_Import.ValidateUploadedFiles();
            Clinical_LabResultHL7_Import.FilesContainer.Files = obj.files;

        }
        else {
            delete Clinical_LabResultHL7_Import.FilesContainer.Files;
            Clinical_LabResultHL7_Import.TruncateFileControl();
            toReturn = false;
        }
        return toReturn;

    },

    ImportLabResultHL7Message: function () {
        if (Clinical_LabResultHL7_Import.FilesContainer.Files) {
            if (Clinical_LabResultHL7_Import.ValidateUploadedFiles()) {
                var data = new FormData();
                $.each(Clinical_LabResultHL7_Import.FilesContainer.Files, function (key, value) {
                    data.append(key, value);
                });
                Clinical_LabResultHL7_Import.SaveImport(data).done(function (response) {
                    if (response.status != false) {
                        utility.DisplayMessages("Data imported successfully.", 1);
                        $(" #mainForm  li#CDSAlert").show();
                        $(" #mainForm #hfTriggerLocation").val('LabResult');
                        ClinicalCDSDetail.showCDSAlert('', response.PatientId);
                       // utility.DisplayMessages(response.message, 1);

                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }
        else
            utility.DisplayMessages("Please select at least one File to upload", 2);

    },

    SaveImport: function (data, PatientDocumentData) {
        return MDVisionService.fileService(data, "CLINICAL_LABRESULT", "SAVE_PATIENT_DOCUMENT");
    },

    UnLoad: function () {
        utility.UnLoadDialog('frmLabResultHL7Import', function () {
            if (Clinical_LabResultHL7_Import.params["FromAdmin"] == "0") {
                if (Clinical_LabResultHL7_Import.params != null && Clinical_LabResultHL7_Import.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_LabResultHL7_Import.params.ParentCtrl, 'Clinical_LabResultHL7_Import');
                }
                else
                    UnloadActionPan(null, 'Clinical_LabResultHL7_Import');
            }
            else {
                RemoveAdminTab();
            }

        }, function () {
            if (Clinical_LabResultHL7_Import.params["FromAdmin"] == "0") {
                if (Clinical_LabResultHL7_Import.params != null && Clinical_LabResultHL7_Import.params.ParentCtrl != null) {
                    UnloadActionPan(Clinical_LabResultHL7_Import.params.ParentCtrl, 'Clinical_LabResultHL7_Import');
                }
                else
                    UnloadActionPan(null, 'Clinical_LabResultHL7_Import');
            }
            else {
                RemoveAdminTab();
            }
        });

    },

    ValidateUploadedFiles: function () {
        var fileName = "";
        var files = $('#Upload_HL7LabResult_file').get(0).files;
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "text/plain") {
                utility.DisplayMessages("File Type is Invalid", 4);
                Clinical_LabResultHL7_Import.TruncateFileControl();
                return false;
            }
            if (Clinical_LabResultHL7_Import.ValidateFileSize(files) > Number(globalAppdata['FileSize'])) {
                utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
                Clinical_LabResultHL7_Import.TruncateFileControl();
                return false;
            }
            if (files[i].name.length > 256) {
                utility.DisplayMessages("File Name should have maximun 256 Characters", 4);
                Clinical_LabResultHL7_Import.TruncateFileControl();
                return false;
            }
            fileName = fileName + files[i].name + ',';
        }
        fileName = fileName.slice(0, fileName.length - 1);
        document.getElementById("uploadFilePH").value = fileName;
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
        $("#" + Clinical_LabResultHL7_Import.params.PanelID + " #uploadFilePH").val('');
        $('#' + Clinical_LabResultHL7_Import.params.PanelID + ' #totalFiles').text("0 file(s) selected");
        $('#' + Clinical_LabResultHL7_Import.params.PanelID + ' #Upload_Import_file').val('');
    }

}
