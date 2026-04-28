CCMEnrollmentInfo = {
    bIsFirstLoad: true,
    ConsentFileStream: {},
    FilesContainer: { Files: undefined, Name: "Uploaded_Document" },
    IsVerbal: false,

    params: [],
    Load: function (params) {
        CCMEnrollmentInfo.params = params;

        $('#chkVerbal').change(function () {
            if ($(this).is(":checked")) {
                //var returnVal = confirm("Are you sure?");
                //$(this).attr("checked", returnVal);
                CCMEnrollmentInfo.IsVerbal = true;
                $("#consentFileName").text('No File Selected');
                CCMEnrollmentInfo.ConsentFileStream = "";
                $("#ConsentAnchor").css('pointer-events', 'none');
            }
            else {
                CCMEnrollmentInfo.IsVerbal = false;
                if (CCMEnrollmentInfo.ConsentFileStream != "")
                    $("#ConsentAnchor").css('pointer-events', 'auto');
                else
                    $("#ConsentAnchor").css('pointer-events', 'none');
            }
        });

        if (CCMEnrollmentInfo.params != null && CCMEnrollmentInfo.params.PanelID != "pnlCCMEnrollmentInfo") {
            CCMEnrollmentInfo.params["PanelID"] = CCMEnrollmentInfo.params["PanelID"] + ' #pnlCCMEnrollmentInfo';
        }
        else {
            CCMEnrollmentInfo.params["PanelID"] = "pnlCCMEnrollmentInfo";
        }

        if (CCMEnrollmentInfo.bIsFirstLoad) {
            CCMEnrollmentInfo.bIsFirstLoad = false;
            var self = $("#" + CCMEnrollmentInfo.params.PanelID);
            var data = "IsActive=&ID=" + CCMEnrollmentInfo.params.PatientId;

            self.loadDropDownsWithTitle(true, data).done(function () {
                self.loadDropDowns(true).done(function () {
                    $("#pnlCCMEnrollmentInfo #modaldialog").removeClass('modal-dialog-lg');
                    CCMEnrollmentInfo.LoadAllControls();
                    CCMEnrollmentInfo.LoadCCMEnrollmentInfo();
                });
            });
        }
    },

    LoadCCMEnrollmentInfo: function () {
        PageNo = null;
        rpp = null;
        if (CCMEnrollmentInfo.params.mode.toLowerCase() == "add") {
            $("#consentFileName").text('No File Selected');
            CCMEnrollmentInfo.ConsentFileStream = "";
            $("#ConsentAnchor").css('pointer-events', 'none');
            CCMEnrollmentInfo.ValidateCCMEnrollementInfo();

            //Serialize data
            $('#frmCCMEnrollmentInfo').data('serialize', $('#frmCCMEnrollmentInfo').serialize());
        }
        else if (CCMEnrollmentInfo.params.mode.toLowerCase() == "edit") {
            CCMEnrollmentInfo.FillCCMEnrollmentInfo(CCMEnrollmentInfo.params.EnrollmentInfoId, PageNo, rpp).done(function (response) {
                if (response.status != false) {
                    var CCMEnrollmentInfo_detail = response.CCMEnrollmentInfoFill_JSON;
                    var self = $("#" + CCMEnrollmentInfo.params["PanelID"]);

                    if (CCMEnrollmentInfo_detail.StartingFrom != "") {
                        CCMEnrollmentInfo_detail.StartingFrom = utility.RemoveTimeFromDate(null, CCMEnrollmentInfo_detail.StartingFrom);
                    }
                    if (CCMEnrollmentInfo_detail.EndingOn != "") {
                        CCMEnrollmentInfo_detail.EndingOn = utility.RemoveTimeFromDate(null, CCMEnrollmentInfo_detail.EndingOn);
                    }
                    if (CCMEnrollmentInfo_detail.ConsentDate != "") {
                        CCMEnrollmentInfo_detail.ConsentDate = utility.RemoveTimeFromDate(null, CCMEnrollmentInfo_detail.ConsentDate);
                    }
                    utility.bindMyJSONByName(true, CCMEnrollmentInfo_detail, false, self).done(function () {
                        if (CCMEnrollmentInfo_detail.ConsentPath != "") {
                            self.find("#uploadFilePH").val("Consent.pdf");
                            self.find("#consentFileName").text(CCMEnrollmentInfo_detail.ConsentFileName);
                            CCMEnrollmentInfo.ConsentFileStream = CCMEnrollmentInfo_detail.ConsentPath;
                        }
                        else {
                            $("#consentFileName").text('No File Selected');
                            CCMEnrollmentInfo.ConsentFileStream = "";
                            $("#ConsentAnchor").css('pointer-events', 'none');
                        }

                        CCMEnrollmentInfo_detail.ISVerbal == "True" ? $("#chkVerbal").attr('checked', true) : $("#chkVerbal").attr('checked', false)
                        CCMEnrollmentInfo.ValidateCCMEnrollementInfo();

                        //Serialize data
                        $('#frmCCMEnrollmentInfo').data('serialize', $('#frmCCMEnrollmentInfo').serialize());

                    });

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    LoadAllControls: function () {
        utility.ValidateStartToEndDate(CCMEnrollmentInfo.params.PanelID + ' #frmCCMEnrollmentInfo', 'dtpStartingFrom', 'dtpEndingOn', true, function () {
            //from change
            CCMEnrollmentInfo.setEndDate();

        }, function () {


            //to date change
        }


        );

        utility.CreateDatePicker(CCMEnrollmentInfo.params.PanelID + ' #frmCCMEnrollmentInfo #dtpConsentDate', function (ev) {
            //    $('#frmCCMEnrollmentInfo #dtpConsentDate').datepicker('setDate', new Date());
        });
        $('#frmCCMEnrollmentInfo #dtpConsentDate').datepicker('setDate', new Date());
        $("#ddlProgram").val(1);
    },

    setEndDate: function () {

        var self = $("#" + CCMEnrollmentInfo.params.PanelID + " #frmCCMEnrollmentInfo");

        var startingDate = self.find('#dtpStartingFrom').datepicker("getDate");
        if (startingDate == null || startingDate == "")
        { return; }

        var duration = Number(self.find("#txtDuration").val());
        var durationUnit = self.find("#ddlDurationUnit").val();
        var initialDate = new Date(startingDate);
        var endingDate = "";
        if (durationUnit == "days") {


            endingDate = new Date(Number(initialDate.getFullYear()), initialDate.getMonth(), initialDate.getDate() + duration);
        }
        else if (durationUnit == "weeks") {
            endingDate = new Date(Number(initialDate.getFullYear()), initialDate.getMonth(), initialDate.getDate() + duration * 7);

        }
        else if (durationUnit == "months") {
            endingDate = new Date(Number(initialDate.getFullYear()), initialDate.getMonth() + duration, initialDate.getDate());
        }
        else if (durationUnit == "years") {
            endingDate = new Date(Number(initialDate.getFullYear()) + duration, initialDate.getMonth(), initialDate.getDate());
        }

        self.find('#dtpEndingOn').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), endingDate));
    },

    ValidateCCMEnrollementInfo: function () {
        $('#frmCCMEnrollmentInfo')
            .bootstrapValidator({
                live: 'disabled',
                message: 'This value is not valid',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {

                    Program: {
                        group: '.col-sm-4',
                        validators: {
                            notEmpty: {
                                message: ''
                            }
                        }
                    },
                    CareTeam: {
                        group: '.col-sm-4',
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
            CCMEnrollmentInfo.saveCCMEnrollmentInfo();
        });
    },

    saveCCMEnrollmentInfo: function () {

        var self = $("#" + CCMEnrollmentInfo.params["PanelID"]);

        var strMessage = "";
        var duration = Number(self.find("#txtDuration").val());
        var durationUnit = $("#ddlDurationUnit").val();
        if (durationUnit == "days") {
            if (Number(new Date().getFullYear()) % 4 == 0) {
                if (duration < 366) {
                    utility.DisplayMessages('Duration must be equal to or greater than 1 Year', 3);
                    return false;
                }
            }
            else {
                if (duration < 365) {
                    utility.DisplayMessages('Duration must be equal to or greater than 1 Year', 3);
                    return false;
                }
            }
        }

        if (durationUnit == "weeks") {
            if (duration < 52) {
                utility.DisplayMessages('Duration must be equal to or greater than 1 Year', 3);
                return false;
            }
        }
        if (durationUnit == "months") {
            if (duration < 12) {
                utility.DisplayMessages('Duration must be equal to or greater than 1 Year', 3);
                return false;
            }
        }
        if (durationUnit == "years") {
            if (duration < 1) {
                utility.DisplayMessages('Duration must be equal to or greater than 1 Year', 3);
                return false;
            }
        }

        var myJSON = self.getMyJSONByName();
        if (CCMEnrollmentInfo.params.mode.toLowerCase() == "add") {
            //fixme add priviliges
            //  AppPrivileges.GetFormPrivileges("Chronic Care Management", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                CCMEnrollmentInfo.CCMEnrollmentInfoSave(myJSON).done(function (response) {
                    if (response != undefined && response != 'undefined' && response != null) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);

                            if (CCMEnrollmentInfo.params["ParentCtrl"] == "mstrTabDashBoard") {
                                DashBoard.DashBoardCCMEnrollmentInfoSearch();
                            }

                            setPatientBanner(CCMEnrollmentInfo.params.PatientId, "1", '');

                            if (CCMEnrollmentInfo.params != null && CCMEnrollmentInfo.params.ParentCtrl != null) {
                                UnloadActionPan(CCMEnrollmentInfo.params.ParentCtrl, 'CCMEnrollmentInfo');
                            }
                            else
                                UnloadActionPan(null, 'CCMEnrollmentInfo');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                    else {
                        utility.DisplayMessages('Some error occured while saving record', 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            // });
        }
        else if (CCMEnrollmentInfo.params.mode.toLowerCase() == "edit") {
            //   AppPrivileges.GetFormPrivileges("Advance Payment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                CCMEnrollmentInfo.CCMEnrollmentInfoUpdate(myJSON).done(function (response) {

                    if (response != undefined && response != 'undefined' && response != null) {
                        if (response.status != false) {

                            utility.DisplayMessages(response.Message, 1);
                            UnloadActionPan(CCMEnrollmentInfo.params["ParentCtrl"]);
                            if (CCMEnrollmentInfo.params["ParentCtrl"] == "mstrTabDashBoard") {
                                DashBoard.DashBoardCCMEnrollmentInfoSearch();
                            }
                            setPatientBanner(CCMEnrollmentInfo.params.PatientId, "1", '');
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    }
                    else {
                        utility.DisplayMessages('Some error occured while saving record', 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            //  });
        }
    },

    FillCCMEnrollmentInfo: function (EnrollmentInfoId) {
        var objData = new Object();

        objData["EnrollmentInfoId"] = EnrollmentInfoId;
        //objData["PatientId"] = CCMEnrollmentInfo.params.PatientId;

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMEnrollmentInfo", "FillCCMEnrollmentInfo");
    },

    CCMEnrollmentInfoSave: function (CCMEnrollmentData) {
        var objData = JSON.parse(CCMEnrollmentData);

        if (CCMEnrollmentInfo.ConsentFileStream != null && CCMEnrollmentInfo.ConsentFileStream != "") {
            if (typeof (CCMEnrollmentInfo.ConsentFileStream) == "object") {
                objData["ConsentFileStream"] = "";
            }
            else
            {
                objData["ConsentFileStream"] = CCMEnrollmentInfo.ConsentFileStream;
                objData["ConsentFileName"] = $("#consentFileName").text();
            }
                
        }

        objData["PatientId"] = CCMEnrollmentInfo.params.PatientId;
        objData["StatusId"] = "2";
        if (CCMEnrollmentInfo.IsVerbal)
            var verb = true;
        else
            var verb = false;
        objData["ISVerbal"] = verb;

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMEnrollmentInfo", "SaveCCMEnrollmentInfo");
    },

    CCMEnrollmentInfoUpdate: function (CCMEnrollmentData) {
        var objData = JSON.parse(CCMEnrollmentData);

        objData["EnrollmentInfoId"] = CCMEnrollmentInfo.params.EnrollmentInfoId;
        objData["PatientId"] = CCMEnrollmentInfo.params.PatientId;
        //objData["ConsentFileStream"] = CCMEnrollmentInfo.ConsentFileStream;

        if (CCMEnrollmentInfo.ConsentFileStream != null && CCMEnrollmentInfo.ConsentFileStream != "") {
            if (typeof (CCMEnrollmentInfo.ConsentFileStream) == "object") {
                objData["ConsentFileStream"] = "";
            }
            else {
                objData["ConsentFileStream"] = CCMEnrollmentInfo.ConsentFileStream;
                objData["ConsentFileName"] = $("#consentFileName").text();
            }
        }
        else {
            objData["ConsentFileStream"] = "";
        }
        objData["StatusId"] = "2";
        //objData["ISVerbal"] = CCMEnrollmentInfo.IsVerbal;

        if (CCMEnrollmentInfo.IsVerbal)
            var verb = true;
        else
            var verb = false;
        objData["ISVerbal"] = verb;

        var data = JSON.stringify(objData);
        return MDVisionService.CCMAPIService(data, "CCMEnrollmentInfo", "UpdateCCMEnrollmentInfo");
    },

    BufferFile: function (obj) {
        var toReturn = true;

        if (obj.files && obj.files.length != 0) {
            CCMEnrollmentInfo.ValidateUploadedFiles();
            CCMEnrollmentInfo.FilesContainer.Files = obj.files;
            //$('#frmCCMEnrollmentInfo').bootstrapValidator('revalidateField', 'ConsentFileName');


            var reader = new FileReader();
            reader.readAsBinaryString(obj.files[0]);

            reader.onload = function (readerEvt) {
                var binaryString = readerEvt.target.result;
                CCMEnrollmentInfo.ConsentFileStream = btoa(binaryString);
                //  return toReturn;
            };


        }
        else {
            delete CCMEnrollmentInfo.FilesContainer.Files;
            CCMEnrollmentInfo.TruncateFileControl();
            //$('#frmCCMEnrollmentInfo').bootstrapValidator('revalidateField', 'ConsentFileName');
            toReturn = false;
            // return toReturn;
        }
        //return toReturn;

    },

    ValidateUploadedFiles: function () {
        var fileName = "";
        var files = $('#Upload_Import_consent').get(0).files;
        for (var i = 0 ; i < files.length; i++) {
            var fileType = files[i].type;
            if (fileType != "application/pdf") {
                utility.DisplayMessages("Please attach PDF file", 4);
                CCMEnrollmentInfo.TruncateFileControl();
                return false;
            }
            //if (CCMEnrollmentInfo.ValidateFileSize(files) > Number(globalAppdata['FileSize'])) {
            //    utility.DisplayMessages("Maximum " + Number(globalAppdata['FileSize']) + "MB  is allowed", 4);
            //    CCMEnrollmentInfo.TruncateFileControl();
            //    return false;
            //}
            if (files[i].name.length > 256) {
                utility.DisplayMessages("File Name should have maximun 256 Characters", 4);
                CCMEnrollmentInfo.TruncateFileControl();
                return false;
            }
            fileName = fileName + files[i].name + ',';
        }
        fileName = fileName.slice(0, fileName.length - 1);
        document.getElementById("uploadFilePH").value = fileName;
        $("#consentFileName").text(fileName);
        $("#chkVerbal").attr("checked", false);
        CCMEnrollmentInfo.IsVerbal = false;

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
        $("#" + CCMEnrollmentInfo.params.PanelID + " #uploadFilePH").val('');
        $('#' + CCMEnrollmentInfo.params.PanelID + ' #totalFiles').text("0 file(s) selected");
        $('#' + CCMEnrollmentInfo.params.PanelID + ' #Upload_Import_file').val('');
        CCMEnrollmentInfo.ConsentFileStream = "";
    },


    OpenCCMAgreement: function (PatientId) {

        var params = [];
        params["ParentCtrl"] = 'CCMEnrollmentInfo';
        params["PatientId"] = PatientId;
        params["PatientName"] = CCMEnrollmentInfo.params.PatientName;
        params["ProviderName"] = CCMEnrollmentInfo.params.ProviderName;
        params["FromAdmin"] = "0";

        if (CCMEnrollmentInfo.params.PatientName == null || CCMEnrollmentInfo.params.PatientName == "") {

            var Enroll = CCMEnrollmentInfo.params.EnrollmentInfoId;
            if (Enroll == undefined || Enroll == 'undefined' || Enroll == "" || Enroll == null)
                Enroll = "0";

            var objData = new Object();
            objData["EnrollmentInfoId"] = Enroll;
            objData["PatientId"] = CCMEnrollmentInfo.params.PatientId;
            objData["ProviderId"] = "0";

            CCM_Patient_Hub.PatientHubStaticLoad(objData).done(function (response) {
                if (response.status != false) {
                    if (response.PHCount > 0) {
                        var response = JSON.parse(response.PHList_JSON);
                        CCMEnrollmentInfo.params.PatientName = response[0].PatientName;
                        CCMEnrollmentInfo.params.ProviderName = response[0].ProviderName;
                        params["PatientName"] = CCMEnrollmentInfo.params.PatientName;
                        params["ProviderName"] = CCMEnrollmentInfo.params.ProviderName;
                        LoadActionPan('CCMAgreement', params);
                    }
                }
            });

        }
        else {
            LoadActionPan('CCMAgreement', params);
        }

    },


    OpenConsentView: function () {

        var params = [];
        params["ParentCtrl"] = 'CCMEnrollmentInfo';
        params["FromAdmin"] = "0";
        params["ConsentFileStream"] = CCMEnrollmentInfo.ConsentFileStream;

        LoadActionPan('CCMConsent', params);
    },

    IsVerbal: function (obj) {
        if ($(obj).is(":checked")) {
            CCMEnrollmentInfo.IsVerbal = true;
            $("#consentFileName").text('No File Selected');
            $("#ConsentAnchor").css('pointer-events', 'none');
        }
        else {
            CCMEnrollmentInfo.IsVerbal = false;
            $("#ConsentAnchor").css('pointer-events', 'auto');
        }
    },


    setConsentFile: function (base64) {

        CCMEnrollmentInfo.ConsentFileStream = base64;
        $("#consentFileName").text('Consent.pdf');
        $("#ConsentAnchor").css('pointer-events', 'auto');
        $("#chkVerbal").attr("checked", false);
        CCMEnrollmentInfo.IsVerbal = false;
        //$("#" + CCMEnrollmentInfo.params.PanelID + " #uploadFilePH").val('Consent.pdf');
        //$('#frmCCMEnrollmentInfo').bootstrapValidator('revalidateField', 'ConsentFileName');


    },

    UnLoad: function () {

        if (CCMEnrollmentInfo.params != null && CCMEnrollmentInfo.params.ParentCtrl != null && CCMEnrollmentInfo.params.PanelID != 'pnlCCMEnrollmentInfo') {
            UnloadActionPan(CCMEnrollmentInfo.params.ParentCtrl, 'pnlCCMEnrollmentInfo', null, CCMEnrollmentInfo.params.PanelID);
        }

        else if (CCMEnrollmentInfo.params != null && CCMEnrollmentInfo.params.ParentCtrl != null) {
            UnloadActionPan(CCMEnrollmentInfo.params.ParentCtrl, 'CCMEnrollmentInfo');
        }

    },
}