
Clinical_CaseReports = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_CaseReports.params = params;

        if (Clinical_CaseReports.params.PanelID != 'CaseReports') {
            Clinical_CaseReports.params.PanelID = Clinical_CaseReports.params.PanelID + ' #CaseReports';
        } else {
            Clinical_CaseReports.params.PanelID = 'CaseReports';
        }
        if (Clinical_CaseReports.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_CaseReports.params.PanelID + " div#FaceSheetPager", Clinical_CaseReports.params.FaceSheetComponents, 'appointments');
        }

        if (Clinical_CaseReports.bIsFirstLoad) {
            Clinical_CaseReports.bIsFirstLoad = false;
            Clinical_CaseReports.LoadNotes();
            $('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit').on("change", function () {
                if ($(this).val() != "")
                    Clinical_CaseReports.loadNotesData($(this).val());
                else {
                    $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements input[type='checkbox']").each(function () {
                        if ($(this).attr("disabled") != "disabled")
                            $(this).prop("checked", false);
                    });
                }
            });
            $('#' + Clinical_CaseReports.params.PanelID + ' #chkEncryption').on("change", function () {
                if (!$(this).prop("checked")) {
                    $('#' + Clinical_CaseReports.params.PanelID + ' #txtPassword').val('');
                }
            })
        }
    },

    CommandType: {
        ViewHtml: 1,
        Download: 2,
        GenerateHash: 3
    },

  
    displayCaseReportsHTML: function (summaryType) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FaceSheet_Export CCDA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
                    $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
                    var allRecordsparams = [];
                    var chkCheckedDataElements = $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
                    allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
                    allRecordsparams["FromAdmin"] = "0";
                    allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                    allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                    allRecordsparams["ParentCtrl"] = "CaseReports";
                    allRecordsparams["NoteId"] = $('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val();
                //   Clinical_CaseReports.downloadCaseReportHTML(Clinical_CaseReports.CommandType.ViewHtml, allRecordsparams);
                    Clinical_CaseReports.generateCCDA(Clinical_CaseReports.CommandType.ViewHtml, allRecordsparams);
                }
                else {
                    utility.DisplayMessages("please select the clinical visit.", 3);
                    $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });


    },


    //Author:Farooq Ahmad
    //Date: 07/04/2016
    //Overview: This function is created to Load Clinical Summary HTML
    downloadCaseReportsPDFData: function () {
        if ($('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            var objData = new Object();

            objData["NoteId"] = $('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = objData["PatientId"] = Clinical_CaseReports.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, objData["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    objData["commandType"] = "PDF";
                    objData["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    objData["Components"] = Clinical_CaseReports.getSelectedComponentJSONArray();

                    var data = JSON.stringify(objData);
                    return MDVisionService.APIService(data, "CaseReports", "CaseReports").done(function (response) {
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

    getSelectedComponentJSONArray: function () {
        var Components = [], compId = -1;
        $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements input[type='checkbox']:checked").each(function (index, item) {
            if ($(item).attr('value') != null) {
                var CompIds = $(item).attr('value').split(",");
                for (var comId in CompIds) {
                    componentId = Clinical_CaseReports.TryParseInt(CompIds[comId], 0);
                    if (componentId == 0) {
                        componentId = compId--;
                    }
                    var obj = {
                        componentId: componentId,
                        componentName: $(item).attr('class'),
                    };
                    Components.push(obj);
                }
                componentId = Clinical_CaseReports.TryParseInt($(item).attr('value'), 0);
            }
            else {
                componentId = compId--;
                var obj = {
                    componentId: componentId,
                    componentName: $(item).attr('class'),
                };
                Components.push(obj);
            }
        });
        return Components;
    },


    downloadCaseReportsXMLData: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FaceSheet_Export CCDA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
                    $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
                    if (!Clinical_CaseReports.checkEncryption())
                        return;
                    var allRecordsparams = [];
                    var chkCheckedDataElements = $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
                    allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
                    allRecordsparams["FromAdmin"] = "0";
                    allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                    allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                    allRecordsparams["ParentCtrl"] = "CaseReports";
                    allRecordsparams["NoteId"] = $('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val();
                    allRecordsparams["Password"] = $('#' + Clinical_CaseReports.params.PanelID + ' #txtPassword').val();
                    allRecordsparams["Template"] = "CaseReports";
                   // Clinical_CaseReports.generateCCDA(Clinical_CaseReports.CommandType.Download, allRecordsparams);
                    Clinical_CaseReports.downloadCaseReportHTML(Clinical_CaseReports.CommandType.Download, allRecordsparams);


                  
            

                }
                else {
                    utility.DisplayMessages("please select the clinical visit.", 3);
                    $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
                }

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
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
            var chkDataElements = $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements input[type='checkbox']").each(function (i, item) {
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
            var chkSelectAllDataElements = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports input[type='checkbox'][id='chkSelectAllDataElements']");
            var chkDataElements = $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements input[type='checkbox']");
            var chkCheckedDataElements = $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
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
                Clinical_CaseReports.NotesLoad().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if (response.ClinicalNotesCount > 0) {
                            var NotesArray = JSON.parse(response.NotesLoad_JSON);
                            var ddlClinicalVisit = $('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit');
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
                                $('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit').val(NotesArray[0].NotesId.toString());
                            }
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
        var isEncrypted = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #chkEncryption").prop("checked");
        if (isEncrypted) {
            if ($("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtPassword").val() == "") {
                utility.DisplayMessages("please enter password to Encrypt.", 3);
                $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else if ($("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtPassword").val() != $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtConfirmPassword").val()) {
                utility.DisplayMessages("password did not match confirm password.", 3);
                $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtConfirmPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else {
                $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtPassword").closest('div').removeClass("has-feedback has-error");
                $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtConfirmPassword").closest('div').removeClass("has-feedback has-error");
            }

        }
        return true;
    },

    //Author: Farooq Ahmad
    //Date: 04/04/2016
    //Overview: This function will generate the hash value
    generateCCDA: function (isFrom, ParentParams) {
        var param = new Object();
        if ($('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {

            param["NoteId"] = $('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = param["PatientId"] = Clinical_CaseReports.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, param["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {


                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    param["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    param["commandType"] = "XML";
                    param["Components"] = Clinical_CaseReports.getSelectedComponentJSONArray();
                    param["IsConfidential"] = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #chkConfidential").prop("checked");
                    data = JSON.stringify(param);
                    MDVisionService.APIService(data, "CaseReports", "CaseReports").done(function (response) {
                        var responseDetail = response = JSON.parse(response);
                        if (response.status != false) {
                            response.data = JSON.parse(response.data);
                            if (response.data.status != false) {
                                $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                                if (isFrom == Clinical_CaseReports.CommandType.ViewHtml) {
                                    param["XMLData"] = response.data.xmlData; //base64 String
                                    param["PatientData"] = JSON.parse(response.data.PatientData_JSON);
                                    param["AuthoringData"] = JSON.parse(response.data.AuthoringData_JSON);
                                    param["PlanOfTreatmentData"] = JSON.parse(response.data.PlanOfTreatmentData_JSON);
                                    param["EncounterData"] = JSON.parse(response.data.EncounterData_JSON);
                                    param["HistoryOfCurrentIllnessData"] = JSON.parse(response.data.HistoryOfCurrentIllnessData_JSON);
                                    param["MedicationData"] = JSON.parse(response.data.MedicationData_JSON);
                                    param["ProblemsData"] = JSON.parse(response.data.ProblemsData_JSON);
                                    param["ReasonForVisitData"] = JSON.parse(response.data.ReasonForVisitData_JSON);
                                    param["ResultsData"] = JSON.parse(response.data.ResultsData_JSON);
                                    param["SocialHistoryData"] = JSON.parse(response.data.SocialHistoryData_JSON);
                                    param["ComponentData"] = JSON.parse(response.data.ComponentsData_JSON);
                                    param["NotesData"] = JSON.parse(response.data.NotesData_JSON);
                                    param["DateTime"] = response.data.GenerateDateTime;
                                    param["PracticeData"] = response.data.PracticeData;
                                    param["ParentCtrl"] = "Clinical_CaseReports";
                                    var componentActionPan = "Clinical_CaseReportsHTML";
                                    LoadActionPan(componentActionPan, param);
                                }
                                else if (isFrom == Clinical_CaseReports.CommandType.Download) {

                                    if (!Clinical_CaseReports.checkEncryption())
                                        return;
                                    param["XMLData"] = response.data.xmlData;
                                    param["Encryption"] = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #chkEncryption").prop("checked");
                                    param["IncludeHashCode"] = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #chkIncludeHashCode").prop("checked");
                                    param["Password"] = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtPassword").val();
                                    param["commandType"] = "DOWNLOAD";
                                    param["SummaryType"] = "1"; // 1 for clinical Summary
                                    data = JSON.stringify(param);
                                    MDVisionService.APIService(data, "CaseReports", "DownloadFile").done(function (response) {
                                        response = JSON.parse(response);
                                        if (response.status != false) {
                                            var zip = new JSZip();
                                            if ($("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #chkIncludeHashCode").prop("checked"))
                                                zip.file("HashCode.txt", response.HashCode);
                                            var xml = zip.folder("XML");

                                            xml.file("XMLData.xml", response.XMLByte, { base64: true });

                                            var html = zip.folder("HTML");
                                            html.file("htmlData.html", response.HTMLByte, { base64: true });
                                            zip.generateAsync({ type: "blob" })
                                            .then(function (content) {
                                                saveAs(content, "CCDA.zip");
                                            });
                                            utility.DisplayMessages("CCDA Downloaded Successfully.", 1);
                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                        }
                                    });
                                }
                            }
                            else {
                                utility.DisplayMessages(response.data.Message, 3);
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

    downloadCaseReportHTML: function (isFrom, ParentParams) {
        var param = new Object();
        if ($('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {

            param["NoteId"] = $('#' + Clinical_CaseReports.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = param["PatientId"] = Clinical_CaseReports.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, param["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {


                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    param["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    param["commandType"] = "XML";
                    param["Components"] = Clinical_CaseReports.getSelectedComponentJSONArray();
                    param["IsConfidential"] = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #chkConfidential").prop("checked");
                    data = JSON.stringify(param);
                    MDVisionService.APIService(data, "CaseReports", "CaseReports").done(function (response) {
                        var responseDetail = response = JSON.parse(response);
                        if (response.status != false) {
                            response.data = JSON.parse(response.data);
                            if (response.data.status != false) {
                                $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                                if (isFrom == Clinical_CaseReports.CommandType.ViewHtml) {
                                    param["XMLData"] = response.data.xmlData; //base64 String
                                    param["PatientData"] = JSON.parse(response.data.PatientData_JSON);
                                    param["AuthoringData"] = JSON.parse(response.data.AuthoringData_JSON);
                                    param["PlanOfTreatmentData"] = JSON.parse(response.data.PlanOfTreatmentData_JSON);
                                    param["EncounterData"] = JSON.parse(response.data.EncounterData_JSON);
                                    param["HistoryOfCurrentIllnessData"] = JSON.parse(response.data.HistoryOfCurrentIllnessData_JSON);
                                    param["MedicationData"] = JSON.parse(response.data.MedicationData_JSON);
                                    param["ProblemsData"] = JSON.parse(response.data.ProblemsData_JSON);
                                    param["ReasonForVisitData"] = JSON.parse(response.data.ReasonForVisitData_JSON);
                                    param["ResultsData"] = JSON.parse(response.data.ResultsData_JSON);
                                    param["SocialHistoryData"] = JSON.parse(response.data.SocialHistoryData_JSON);
                                    param["ComponentData"] = JSON.parse(response.data.ComponentsData_JSON);
                                    param["NotesData"] = JSON.parse(response.data.NotesData_JSON);
                                    param["DateTime"] = JSON.parse(response.data.GenerateDateTime);
                                    param["PracticeData"] = response.data.PracticeData;
                                    param["ParentCtrl"] = "Clinical_CaseReports";
                                    var componentActionPan = "Clinical_CaseReportsHTML";
                                    //LoadActionPan(componentActionPan, param);
                                    Clinical_CaseReportsHTML.params = param;
                                    Clinical_CaseReportsHTML.BindCaseReportHTML('CaseReports');
                                 
                                    
                                }
                                else if (isFrom == Clinical_CaseReports.CommandType.Download) {
                                    params["XMLData"] = response.data.xmlData; //base64 String
                                    params["PatientData"] = JSON.parse(response.data.PatientData_JSON);
                                    params["AuthoringData"] = JSON.parse(response.data.AuthoringData_JSON);
                                    params["PlanOfTreatmentData"] = JSON.parse(response.data.PlanOfTreatmentData_JSON);
                                    params["EncounterData"] = JSON.parse(response.data.EncounterData_JSON);
                                    params["HistoryOfCurrentIllnessData"] = JSON.parse(response.data.HistoryOfCurrentIllnessData_JSON);
                                    params["MedicationData"] = JSON.parse(response.data.MedicationData_JSON);
                                    params["ProblemsData"] = JSON.parse(response.data.ProblemsData_JSON);
                                    params["ReasonForVisitData"] = JSON.parse(response.data.ReasonForVisitData_JSON);
                                    params["ResultsData"] = JSON.parse(response.data.ResultsData_JSON);
                                    params["SocialHistoryData"] = JSON.parse(response.data.SocialHistoryData_JSON);
                                    params["ComponentData"] = JSON.parse(response.data.ComponentsData_JSON);
                                    params["NotesData"] = JSON.parse(response.data.NotesData_JSON);
                                    params["DateTime"] = response.data.GenerateDateTime;
                                    params["PracticeData"] = response.data.PracticeData;
                                    params["ParentCtrl"] = "Clinical_CaseReports";
                                    var componentActionPan = "Clinical_CaseReportsHTML";
                                    //LoadActionPan(componentActionPan, param);
                                    Clinical_CaseReportsHTML.params = params;
                                    Clinical_CaseReportsHTML.BindCaseReportHTML('CaseReports');

                                    if (!Clinical_CaseReports.checkEncryption())
                                        return;
                                    param["XMLData"] = response.data.xmlData;
                                    param["Encryption"] = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #chkEncryption").prop("checked");
                                    param["IncludeHashCode"] = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #chkIncludeHashCode").prop("checked");
                                    param["Password"] = $("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #txtPassword").val();
                                    param["commandType"] = "DOWNLOAD";
                                    param["SummaryType"] = "1"; // 1 for clinical Summary
                                    data = JSON.stringify(param);
                                    MDVisionService.APIService(data, "CaseReports", "DownloadFile").done(function (response) {
                                        response = JSON.parse(response);
                                        if (response.status != false) {
                                            var zip = new JSZip();
                                            if ($("#" + Clinical_CaseReports.params.PanelID + " #frmCaseReports #chkIncludeHashCode").prop("checked"))
                                                zip.file("HashCode.txt", response.HashCode);
                                            var xml = zip.folder("XML");

                                            xml.file("CaseReportXML.xml", param["XMLData"], { base64: true });

                                            var html = zip.folder("HTML");
                                            htmlB64 = $("#CaseReports #hbase64HTML").val();
                                            html.file("CaseReportHTML.html", htmlB64, { base64: true });
                                            zip.generateAsync({ type: "blob" })
                                            .then(function (content) {
                                                saveAs(content, "CaseReport.zip");
                                            });
                                            utility.DisplayMessages("Case Report Downloaded Successfully.", 1);
                                        }
                                        else {
                                            utility.DisplayMessages(response.Message, 3);
                                        }
                                    });
                                }
                            }
                            else {
                                utility.DisplayMessages(response.data.Message, 3);
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
        if (Clinical_CaseReports.params.PatientId != null) {
            objData["PatientId"] = Clinical_CaseReports.params.PatientId;
        }
        objData["commandType"] = "search_clinical_notes_casereporting";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    /*
    Author: Muhammad Arshad
    Date: 31/03/2016
    Overview: This function is load Note Data
    */
    loadNotesData: function (NotesId) {
        Clinical_ProgressNote.params.patientID = Clinical_CaseReports.params.PatientId;
        Clinical_ProgressNote.FillNotes(null, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                var myNoteText = $(Clinical_Notes_detail.NoteText);
                var chkCheckedDataElements = $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
                $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements input[type='checkbox']").each(function () {
                    if ($(this).attr("disabled") != "disabled")
                        $(this).prop("checked", false);
                });
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


                            $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements ." + className).prop("checked", true);
                            if (className.toLowerCase() != 'radiologyresults') {
                                $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements ." + className).attr('value', componentId);
                            }

                        } catch (ex) {
                            console.log(ex);
                        }

                    }



                });
                $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements .VisitReason").prop("checked", true);
                $("#" + Clinical_CaseReports.params.PanelID + " #ulDataElements .VisitReason").attr('value', NotesId);
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

        Clinical_CaseReports.searchAppointments(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_CaseReports.appointmentGridLoad(response);

                // Start 26/11/2015 Muhammad Irfan for Bug # EMR-25 

                var TableControl = "CaseReports #dgvCaseReports";
                var PagingPanelControlID = "CaseReports #divFaceSheetAppointmentPaging";
                var ClassControlName = "CaseReports";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AppointmentsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    Clinical_CaseReports.appointmentSearch(PageNumber, ResultPerPage);
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
        $("#" + Clinical_CaseReports.params.PanelID + " #dgvCaseReports").dataTable().fnDestroy();
        $("#" + Clinical_CaseReports.params.PanelID + " #dgvCaseReports tbody").find("tr").remove();
        if (response.AppointmentsCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.AppointmentsLoad_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td>' + utility.RemoveTimeFromDate(null, item.ScheduleDate) + '</td><td>' + item.SchReason + '</td><td>' + item.ProviderName + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#" + Clinical_CaseReports.params.PanelID + " #dgvCaseReports tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_CaseReports.params.PanelID + " #divFaceSheetAppointmentPaging").css("display", "none");
            $("#" + Clinical_CaseReports.params.PanelID + " #dgvCaseReports").DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_CaseReports.params.PanelID + " #dgvCaseReports"))
            ;
        else
            $("#" + Clinical_CaseReports.params.PanelID + " #dgvCaseReports").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
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
        UnloadActionPan(Clinical_CaseReports.params.ParentCtrl, 'CaseReports');
        objDeffered.resolve();
        return objDeffered;
    },
}