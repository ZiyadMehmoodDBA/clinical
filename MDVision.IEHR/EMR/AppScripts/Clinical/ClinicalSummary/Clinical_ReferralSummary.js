/*
Author: Muhammad Arshad
Date: 31/03/2016
Overview: This file is created to show clinical summary
*/

Clinical_ReferralSummary = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_ReferralSummary.params = params;

        if (Clinical_ReferralSummary.params.PanelID != 'Clinical_ReferralSummary') {
            Clinical_ReferralSummary.params.PanelID = Clinical_ReferralSummary.params.PanelID + ' #Clinical_ReferralSummary';
        } else {
            Clinical_ReferralSummary.params.PanelID = 'Clinical_ReferralSummary';
        }
        if (Clinical_ReferralSummary.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_ReferralSummary.params.PanelID + " div#FaceSheetPager", Clinical_ReferralSummary.params.FaceSheetComponents, 'appointments');
        }

        if (Clinical_ReferralSummary.bIsFirstLoad) {
            Clinical_ReferralSummary.bIsFirstLoad = false;
            Clinical_ReferralSummary.LoadNotes();
            Clinical_ReferralSummary.FillReferralProvider();
            $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit').on("change", function () {
                Clinical_ReferralSummary.loadNotesData($(this).val());
            });
            $('#' + Clinical_ReferralSummary.params.PanelID + ' #chkEncryption').on("change", function () {
                if (!$(this).prop("checked")) {
                    $('#' + Clinical_ReferralSummary.params.PanelID + ' #txtPassword').val('');
                }
            })
        }

        Clinical_ReferralSummary.ShowSectionPreference();
       
    },


    //Author : Farooq Ahmad
    //Date: 05/04/2016
    FillReferralProvider: function () {
        var ReferringProviderData = '{"txtLastName":"","txtFirstName":"","txtNPI":"","chkActive":"1","chkActive_text":"Active","ddlEntity":"' + globalAppdata.SeletedEntityId + '"}';
        var data = "ReferringProviderData=" + ReferringProviderData + "&ReferringProviderID=undefined&PageNumber=1&RowsPerPage=2000";
        MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER", "SEARCH_REFERRING_PROVIDER").done(function (response) {
            if (response.status != "false") {
                var output = response.data;

                var output = JSON.parse(response.ReferringProviderLoad_JSON)
                var ddlReferringProvider = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlReferringProvider');
                $(ddlReferringProvider).find('option').remove();
                $(ddlReferringProvider).append($('<option>', {
                    value: '',
                    text: '--Select--'
                }));
                for (var index in output) {
                    var obj = output[index];
                    $(ddlReferringProvider).append($('<option>', {
                        value: obj.ReferringProviderId,
                        text: obj.FirstName + ' ' + obj.LastName
                    }));
                }
            }

        });
    },

    //Author: Farooq Ahmad
    //Date: 25/04/2016
    CommandType: {
        ViewHtml: 1,
        Download: 2,
        GenerateHash: 3,
        TransmitCCDA: 4
    },

    //Author: Farooq Ahmad
    //Date: 04/04/2016
    //Overview: This function is to display Clinical Summary
    displayReferralSummaryHTML: function (summaryType) {

        if ($('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_ReferralSummary";
            allRecordsparams["NoteId"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["ReferralProviderId"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlReferringProvider option:selected').val();
            allRecordsparams["ReferralReason"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #txtReferralReason').val();
            Clinical_ReferralSummary.generateCCDA(Clinical_ReferralSummary.CommandType.ViewHtml, allRecordsparams);
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },


    TransmitCCDA: function (toEmail, IncludeXML, IncludeHTML, DocType) {
        if ($('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            Clinical_ReferralSummary.params["toEmail"] = allRecordsparams["toEmail"] = toEmail;
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_ReferralSummary";
            allRecordsparams["NoteId"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["ReferralProviderId"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlReferringProvider option:selected').val();
            allRecordsparams["ReferralReason"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #txtReferralReason').val();
            allRecordsparams["IncludeXML"] = IncludeXML;
            allRecordsparams["IncludeHTML"] = IncludeHTML;
            allRecordsparams["DOS"] = $("#frmReferralSummary #ddlClinicalVisit :selected").text();
            allRecordsparams["DocType"] = DocType;
            Clinical_ReferralSummary.generateCCDA(Clinical_ReferralSummary.CommandType.TransmitCCDA, allRecordsparams);
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },

    //Author:Farooq Ahmad
    //Date: 07/04/2016
    //Overview: This function is created to Load Clinical Summary HTML
    downloadReferralSummaryPDFData: function () {
        if ($('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            var objData = new Object();

            objData["NoteId"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = objData["PatientId"] = Clinical_ReferralSummary.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, objData["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    objData["commandType"] = "PDF";
                    objData["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    objData["Components"] = Clinical_ReferralSummary.getSelectedComponentJSONArray();

                    var data = JSON.stringify(objData);
                    return MDVisionService.APIService(data, "ClinicalSummary", "ClinicalSummary").done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            function S4() {
                                return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
                            }
                            guid = (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
                            download("data:application/octet-stream;base64," + response.data, guid + ".pdf", "application/octet-stream");
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
        }

    },


    //Author: Farooq Ahmad
    //Date: 15-04-2016
    //Overview: This function will Parse String Into Int or Zero
    TryParseInt: function (str, defaultValue) {
        var retValue = defaultValue;
        if (str !== null) {
            if (str.length > 0) {
                if (!isNaN(str)) {
                    retValue = parseInt(str);
                }
            }
        }
        return retValue;
    },

    //Author: Farooq Ahmad
    //Date: 15-04-2016
    //Overview: This function will return the selected compenent JSON Object Array
    getSelectedComponentJSONArray: function () {
        var Components = [], compId = -1;
        $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements input[type='checkbox']:checked").each(function (index, item) {
            if ($(item).attr('value') != null) {
                var CompIds = $(item).attr('value').split(",");
                for (var comId in CompIds) {
                    componentId = Clinical_ReferralSummary.TryParseInt(CompIds[comId], 0);
                    if (componentId == 0) {
                        componentId = compId--;
                    }
                    var obj = {
                        componentId: componentId,
                        componentName: $(item).attr('class'),
                    };
                    Components.push(obj);


                }
                componentId = Clinical_ReferralSummary.TryParseInt($(item).attr('value'), 0);
            }
            else {
                componentId = compId--;
                var obj = {
                    componentId: componentId,
                    componentName: $(item).attr('class'),
                };
                Components.push(obj);
            }

            if ($(item).attr('class').toLowerCase() == "problemlists") {
                componentId = compId--;
                var obj = {
                    componentId: componentId,
                    componentName: 'encounterdiagnostic',
                };
                Components.push(obj);
            }
        });
        return Components;
    },


    //Author: Farooq Ahmad
    //Date : 13/04/2016
    //Overview: This function will create the XML and Download 
    downloadReferralSummaryXMLData: function (xmlType) {
        if ($('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            if (!Clinical_ReferralSummary.checkEncryption())
                return;
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_ReferralSummary";
            allRecordsparams["NoteId"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["Password"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #txtPassword').val();
            var param = Clinical_ReferralSummary.CommandType.Download;
            if (xmlType == 'TransmitCCDA') {
                param = Clinical_ReferralSummary.CommandType.TransmitCCDA;
            }
            Clinical_ReferralSummary.generateCCDA(param, allRecordsparams);

        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },

    //Author: Muhammad Arshad
    //Date: 01/04/2016
    //Overview: This function is to select Data Elements
    selectAllDataElements: function (obj) {
        if (obj != null) {
            var isChecked = false;
            if ($(obj).prop("checked") == true) {
                isChecked = true;
            }
            var chkDataElements = $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements input[type='checkbox']").each(function (i, item) {
                var currentId = $(item).attr("id");
                if (currentId != null && currentId != "chkDemographicDataElement" && currentId != "chkProviderDataElement") {
                    $(item).prop("checked", isChecked);
                }
            });
        }
    },

    //Author: Muhammad Arshad
    //Date: 01/04/2016
    //Overview: This function is to select Data Element
    selectDataElement: function (obj) {
        if (obj != null) {
            var chkSelectAllDataElements = $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary input[type='checkbox'][id='chkSelectAllDataElements']");
            var chkDataElements = $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements input[type='checkbox']");
            var chkCheckedDataElements = $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            if (chkCheckedDataElements.length == chkDataElements.length) {
                chkSelectAllDataElements.prop("checked", true);
            }
            else {
                chkSelectAllDataElements.prop("checked", false);
            }
        }
    },

    //Author: Farooq Ahmad
    //Date: 31/03/2016
    //Overview: This function is created to LoadNotes
    LoadNotes: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Notes_Notes", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_ReferralSummary.NotesLoad().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var NotesArray = JSON.parse(response.NotesLoad_JSON);
                        var ddlClinicalVisit = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit');
                        $(ddlClinicalVisit).find('option').remove();
                        $(ddlClinicalVisit).append($('<option>', {
                            value: '',
                            text: '--Select--'
                        }));
                        for (var Note in NotesArray) {
                            var date = new Date(NotesArray[Note].VisitDate);
                            var displayValue = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                            displayValue = displayValue + ' ' + NotesArray[Note].VisitTime;

                            $(ddlClinicalVisit).append($('<option>', {
                                value: NotesArray[Note].NotesId,
                                text: displayValue
                            }))
                        }

                        if (NotesArray.length == 1) {
                            $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit').val(NotesArray[0].NotesId.toString());
                        }
                    }
                    else {
                        utility.DisplayMessages(strMessage, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },


    //Author : Farooq Ahmad 
    //Date : 27/04/2016
    //OverView  : If the Encryption is Selected Than Password Is Mendatory
    checkEncryption: function () {
        var isEncrypted = $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #chkEncryption").prop("checked");
        if (isEncrypted) {
            if ($("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #txtPassword").val() == "") {
                utility.DisplayMessages("please enter password to Encrypt.", 3);
                $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #txtPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else if ($("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #txtPassword").val() != $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #txtConfirmPassword").val()) {
                utility.DisplayMessages("password did not match confirm password.", 3);
                $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #txtConfirmPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else {
                $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #txtPassword").closest('div').removeClass("has-feedback has-error");
                $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #txtConfirmPassword").closest('div').removeClass("has-feedback has-error");
            }

        }
        return true;
    },

    //Author: Farooq Ahmad
    //Date: 04/04/2016
    //Overview: This function will generate the hash value
    generateCCDA: function (isFrom, ParentParams) {
        var param = new Object();
        if ($('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {

            param["NoteId"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = param["PatientId"] = Clinical_ReferralSummary.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, param["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);

                    param["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    param["commandType"] = "XMLReferral";
                    param["Components"] = Clinical_ReferralSummary.getSelectedComponentJSONArray();
                    param["referralProvider"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlReferringProvider option:selected').val();
                    param["raferralReason"] = $('#' + Clinical_ReferralSummary.params.PanelID + ' #txtReferralReason').val();


                    data = JSON.stringify(param);
                    MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
                        var responseDetail = response = JSON.parse(response);
                        if (response.status != false) {
                            response.data = JSON.parse(response.data);
                            $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                            if (isFrom == Clinical_ReferralSummary.CommandType.ViewHtml) {
                                param["XMLData"] = response.data.xmlData; //base64 String
                                param["ParentCtrl"] = "Clinical_ReferralSummary";
                                var componentActionPan = "Clinical_ClinicalSummaryHTML";
                                LoadActionPan(componentActionPan, param);
                            }
                            else if (isFrom == Clinical_ReferralSummary.CommandType.Download) {

                                if (!Clinical_ReferralSummary.checkEncryption())
                                    return;
                                param["XMLData"] = response.data.xmlData;
                                param["Encryption"] = $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #chkEncryption").prop("checked");
                                param["IncludeHashCode"] = $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #chkIncludeHashCode").prop("checked");
                                param["Password"] = $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #txtPassword").val();
                                param["commandType"] = "DOWNLOAD";
                                param["Template"] = "ReferralSummary";
                                param["SummaryType"] = "2"; // 2 for ReferralSummary
                                data = JSON.stringify(param);
                                MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {
                                        var zip = new JSZip();
                                        if ($("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #chkIncludeHashCode").prop("checked"))
                                            zip.file("HashCode.txt", response.HashCode);
                                        var xml = zip.folder("XML");

                                        xml.file("XMLData.xml", response.XMLByte, { base64: true });

                                        var html = zip.folder("HTML");
                                        html.file("htmlData.html", response.HTMLByte, { base64: true });
                                        zip.generateAsync({ type: "blob" })
                                        .then(function (content) {
                                            saveAs(content, "CCDA.zip");
                                        });
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            else if (isFrom == Clinical_ReferralSummary.CommandType.TransmitCCDA) {
                                if (!Clinical_ReferralSummary.checkEncryption())
                                    return;
                                param["IncludeXML"] = ParentParams["IncludeXML"];
                                param["IncludeHTML"] = ParentParams["IncludeHTML"];
                                param["XMLData"] = response.data.xmlData;
                                param["Encryption"] = $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #chkEncryption").prop("checked");
                                param["IncludeHashCode"] = $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #chkIncludeHashCode").prop("checked");
                                param["Password"] = $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #txtPassword").val();
                                param["commandType"] = "SendEmail";
                                param["toEmail"] = Clinical_ReferralSummary.params["toEmail"];
                                param["msgType"] = $('#frmTransmitCCDA #selectmsgType').val();
                                param["MessageDetail"] = $('#frmTransmitCCDA #txtMessageDtl').val();
                                param["DOS"] = ParentParams["DOS"]; 
                                param["DocType"] = ParentParams["DocType"];
                                param["PatientAccountNo"] = $('#hfAccountNo').val();
                                
                                data = JSON.stringify(param);
                                MDVisionService.APIService(data, "CLINICALSUMMARY", "SendEmail").done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {

                                        utility.DisplayMessages(response.data, 1);
                                    }
                                    else {
                                        utility.DisplayMessages(response.data, 3);
                                    }
                                });


                            }

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });




                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });

        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
        }
    },


    Transmit: function () {
        if ($('#' + Clinical_ReferralSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var param = [];
            param["ParentCtrl"] = "Clinical_ReferralSummary";
            param["DocType"] = "Referral Summary";
            var componentActionPan = "Clinical_TransmitCCDA";
            LoadActionPan(componentActionPan, param);
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ReferralSummary.params.PanelID + " #frmReferralSummary #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }

    },

    S4: function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    },

    //Author: Farooq Ahmad
    //Date: 31/03/2016
    //Overview: This function is created to LoadNotes
    NotesLoad: function () {
        var objData = new Object();
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 2000;
        if (Clinical_ReferralSummary.params.PatientId != null) {
            objData["PatientId"] = Clinical_ReferralSummary.params.PatientId;
        }
        objData["commandType"] = "SEARCH_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    /*
    Author: Muhammad Arshad
    Date: 31/03/2016
    Overview: This function is load Note Data
    */
    loadNotesData: function (NotesId) {
        Clinical_ProgressNote.params.patientID = Clinical_ClinicalSummary.params.PatientId;
        Clinical_ProgressNote.FillNotes(null, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                var myNoteText = $(Clinical_Notes_detail.NoteText);
                var chkCheckedDataElements = $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
                $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements input[type='checkbox']").each(function () {
                    if ($(this).attr("disabled") != "disabled")
                        $(this).removeAttr("checked")
                });

                Clinical_ReferralSummary.ShowSectionPreference();

                myNoteText.find("li.initialVisitBody").each(function (i, item) {
                    var onClickFunctionName = $(item).find("header a:first").attr("onclick");
                    var compIds = [];
                    var sectionOnClick = $(item).find("section").each(function () {
                        var ID = $(this).attr('id').replace(/[^\d]+/, '');;
                        compIds.push(ID);
                    });
                    var componentId = compIds.join();
                    if (onClickFunctionName.indexOf("'") > -1) {
                        try {
                            onClickFunctionName = onClickFunctionName.substring(onClickFunctionName.indexOf("'") + 1);
                            var className = onClickFunctionName.substring(0, onClickFunctionName.indexOf("'"));


                            $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements ." + className).attr("checked", "checked");
                            $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements ." + className).attr('value', componentId);
                        } catch (ex) {
                            console.log(ex);
                        }

                    }



                });
                $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements .VisitReason").attr("checked", "checked");
                $("#" + Clinical_ReferralSummary.params.PanelID + " #ulDataElements .VisitReason").attr('value', NotesId);
            }
            else {
                //  utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to search appointment
    */
    appointmentSearch: function (PageNo, rpp) {

        Clinical_ReferralSummary.searchAppointments(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ReferralSummary.appointmentGridLoad(response);

                // Start 26/11/2015 Muhammad Irfan for Bug # EMR-25 

                var TableControl = "Clinical_ReferralSummary #dgvClinical_ReferralSummary";
                var PagingPanelControlID = "Clinical_ReferralSummary #divFaceSheetAppointmentPaging";
                var ClassControlName = "Clinical_ReferralSummary";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AppointmentsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    Clinical_ReferralSummary.appointmentSearch(PageNumber, ResultPerPage);
                }), 10);

                // End 26/11/2015 Muhammad Irfan for Bug # EMR-25 

            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });
    },

    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to load grid of appointments
    */
    appointmentGridLoad: function (response) {
        $("#" + Clinical_ReferralSummary.params.PanelID + " #dgvClinical_ReferralSummary").dataTable().fnDestroy();
        $("#" + Clinical_ReferralSummary.params.PanelID + " #dgvClinical_ReferralSummary tbody").find("tr").remove();
        if (response.AppointmentsCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.AppointmentsLoad_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td>' + utility.RemoveTimeFromDate(null, item.ScheduleDate) + '</td><td>' + item.SchReason + '</td><td>' + item.ProviderName + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#" + Clinical_ReferralSummary.params.PanelID + " #dgvClinical_ReferralSummary tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_ReferralSummary.params.PanelID + " #divFaceSheetAppointmentPaging").css("display", "none");
            $("#" + Clinical_ReferralSummary.params.PanelID + " #dgvClinical_ReferralSummary").DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_ReferralSummary.params.PanelID + " #dgvClinical_ReferralSummary"))
            ;
        else
            $("#" + Clinical_ReferralSummary.params.PanelID + " #dgvClinical_ReferralSummary").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $('.table-responsive').css('min-height', '220px');
    },
    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to call handler for data
    */
    searchAppointments: function (PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();
        objData["commandType"] = "Search_FaceSheet_Appointments";
        return MDVisionService.APIService(objData, "FaceSheet", "FaceSheet");

    },

    /*
    Author: Muhammad Irfan
    Date: 21/12/2015
    Overview: This function is created to Unload screen
    */
    UnLoad: function () {
        var objDeffered = $.Deferred();
        UnloadActionPan(Clinical_ReferralSummary.params.ParentCtrl, 'Clinical_ReferralSummary');
        objDeffered.resolve();
        return objDeffered;
    },
    SavePreferenceDisplayCCDA: function () {
        var sections = [];

        $.each($("#ulDataElements input[type='checkbox']:checked").not("[disabled]"), function (index, el) {
            sections.push(el.id);
        });
        localStorage.setItem("ccdaPreferenceReferralSummary", JSON.stringify(sections));

        utility.DisplayMessages("Preference save successfully.", 1);
    },
    ShowSectionPreference: function () {
        var sections = JSON.parse(localStorage.getItem("ccdaPreferenceReferralSummary")) || [];

        $.each(sections, function (index, el) {
            var obj = "#" + el;
            $(obj).prop('checked', true);

            var objectChecked = new Object();
            objectChecked.componentName = $(obj).attr('name');
            objectChecked.componentId = (Clinical_ContinuityofCareDocument.paramsComponents.length + 1) * (-1);
            Clinical_ContinuityofCareDocument.paramsComponents.push(objectChecked);
        });
    }
}