/*
Author: Muhammad Arshad
Date: 31/03/2016
Overview: This file is created to show clinical summary
*/

Clinical_ClinicalSummary = {
    bIsFirstLoad: true,
    params: [],
    paramsComponents: [],
    Load: function (params) {
        Clinical_ClinicalSummary.params = params;

        if (Clinical_ClinicalSummary.params.PanelID != 'Clinical_ClinicalSummary') {
            Clinical_ClinicalSummary.params.PanelID = Clinical_ClinicalSummary.params.PanelID + ' #Clinical_ClinicalSummary';
        } else {
            Clinical_ClinicalSummary.params.PanelID = 'Clinical_ClinicalSummary';
        }
        if (Clinical_ClinicalSummary.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_ClinicalSummary.params.PanelID + " div#FaceSheetPager", Clinical_ClinicalSummary.params.FaceSheetComponents, 'appointments');
        }
        Clinical_ClinicalSummary.setListComponents();
        if (Clinical_ClinicalSummary.bIsFirstLoad) {
            Clinical_ClinicalSummary.bIsFirstLoad = false;
            Clinical_ClinicalSummary.LoadNotes();
            $('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit').on("change", function () {
                if ($(this).val() != "")
                    Clinical_ClinicalSummary.loadNotesData($(this).val());
                else {
                    $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']").each(function () {
                        if ($(this).attr("disabled") != "disabled")
                            $(this).prop("checked", false);
                    });
                }
                Clinical_ClinicalSummary.setListComponents();
                var objectChecked = new Object();
                objectChecked.componentName = "VisitReasonDataElement";
                objectChecked.componentId = -3;
                Clinical_ClinicalSummary.paramsComponents.push(objectChecked);
            });
            $('#' + Clinical_ClinicalSummary.params.PanelID + ' #chkEncryption').on("change", function () {
                if (!$(this).prop("checked")) {
                    $('#' + Clinical_ClinicalSummary.params.PanelID + ' #txtPassword').val('');
                }
            })
        }
    },


    //Author: Farooq Ahmad
    //Date: 25/04/2016
    CommandType: {
        ViewHtml: 1,
        Download: 2,
        GenerateHash: 3
    },

    //Author: Muhammad Arshad
    //Date: 04/04/2016
    //Overview: This function is to display Clinical Summary
    displayClinicalSummaryHTML: function (summaryType) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FaceSheet_Export CCDA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
                    $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
                    var allRecordsparams = [];
                    var chkCheckedDataElements = $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']:checked");
                    allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
                    allRecordsparams["FromAdmin"] = "0";
                    allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                    allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                    allRecordsparams["ParentCtrl"] = "Clinical_ClinicalSummary";
                    allRecordsparams["NoteId"] = $('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();

                    Clinical_ClinicalSummary.generateCCDA(Clinical_ClinicalSummary.CommandType.ViewHtml, allRecordsparams);
                }
                else {
                    utility.DisplayMessages("please select the clinical visit.", 3);
                    $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });


    },

    displayCancerReportSummaryHTML: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FaceSheet_Export CCDA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if ($('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
                //$("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
                var allRecordsparams = [];
                var chkCheckedDataElements = [];// $("#" + Clinical_ClinicalSummary.params.PanelID + " ul#ulDataElements input[type='checkbox']:checked");

                chkCheckedDataElements.push({
                    componentId: -1,
                    componentName: "CancerReport",
                });
                chkCheckedDataElements.push({
                    componentId: -2,
                    componentName: "MedicationsDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -2,
                    componentName: "MedicationsAdministeredDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -3,
                    componentName: "ProblemsDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -4,
                    componentName: "VitalSignsDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -5,
                    componentName: "SmokingStatusDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -6,
                    componentName: "ProceduresDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -7,
                    componentName: "AssessmentDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -8,
                    componentName: "PlanOfTreatmentDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -9,
                    componentName: "PayersSectionDataElement",
                });

                allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
                allRecordsparams["FromAdmin"] = "0";
                allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                //allRecordsparams["ParentCtrl"] = "Clinical_Notes";
                allRecordsparams["NoteId"] = Clinical_Notes.params.NotesId;// $('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();

                Clinical_ClinicalSummary.generateCancerCCDA(Clinical_ClinicalSummary.CommandType.ViewHtml, allRecordsparams);
                //}
                //else {
                //    utility.DisplayMessages("please select the clinical visit.", 3);
                //    $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
                //}
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },


    downloadCancerSummaryXMLData: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FaceSheet_Export CCDA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                //if ($('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
                var allRecordsparams = [];
                var chkCheckedDataElements = [];// $("#" + Clinical_ClinicalSummary.params.PanelID + " ul#ulDataElements input[type='checkbox']:checked");
                chkCheckedDataElements.push({
                    componentId: -1,
                    componentName: "CancerReportDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -2,
                    componentName: "FamilyHistorySectionDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -3,
                    componentName: "AssessmentDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -4,
                    componentName: "MedicationsAdministeredDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -5,
                    componentName: "MedicationsDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -6,
                    componentName: "PayersSectionDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -7,
                    componentName: "PlanOfTreatmentDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -8,
                    componentName: "ProblemsDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -9,
                    componentName: "ProceduresDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -10,
                    componentName: "LabResultsDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -11,
                    componentName: "SocialHistoryDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -12,
                    componentName: "VitalSignsDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -13,
                    componentName: "EncountersDataElement",
                });
                chkCheckedDataElements.push({
                    componentId: -14,
                    componentName: "OccupationHx",
                });

                allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
                allRecordsparams["FromAdmin"] = "0";
                allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                //allRecordsparams["ParentCtrl"] = "Clinical_Notes";
                allRecordsparams["NoteId"] = Clinical_Notes.params.NotesId;
                allRecordsparams["Template"] = "CancerReport";


                //$("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
                //if (!Clinical_ClinicalSummary.checkEncryption())
                //    return;
                //var allRecordsparams = [];
                //var chkCheckedDataElements = $("#" + Clinical_ClinicalSummary.params.PanelID + " ul#ulDataElements input[type='checkbox']:checked");
                //allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
                //allRecordsparams["FromAdmin"] = "0";
                //allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                //allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                //allRecordsparams["ParentCtrl"] = "Clinical_ClinicalSummary";
                //allRecordsparams["NoteId"] = $('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();
                //allRecordsparams["Password"] = $('#' + Clinical_ClinicalSummary.params.PanelID + ' #txtPassword').val();
                //allRecordsparams["Template"] = "ClinicalSummary";
                Clinical_ClinicalSummary.generateCancerCCDA(Clinical_ClinicalSummary.CommandType.Download, allRecordsparams);

                //}
                //else {
                //    utility.DisplayMessages("please select the clinical visit.", 3);
                //    $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
                //}

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },


    //Author:Farooq Ahmad
    //Date: 07/04/2016
    //Overview: This function is created to Load Clinical Summary HTML
    downloadClinicalSummaryPDFData: function () {
        if ($('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            var objData = new Object();

            objData["NoteId"] = $('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = objData["PatientId"] = Clinical_ClinicalSummary.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, objData["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    objData["commandType"] = "PDF";
                    objData["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    objData["Components"] = Clinical_ClinicalSummary.getSelectedComponentJSONArray();

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
        $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']:checked").each(function (index, item) {
            if ($(item).attr('value') != null) {
                var CompIds = $(item).attr('value').split(",");
                for (var comId in CompIds) {
                    componentId = Clinical_ClinicalSummary.TryParseInt(CompIds[comId], 0);
                    if (componentId == 0) {
                        componentId = compId--;
                    }
                    var obj = {
                        componentId: componentId,
                        componentName: $(item).attr('class'),
                    };
                    Components.push(obj);
                }
                componentId = Clinical_ClinicalSummary.TryParseInt($(item).attr('value'), 0);
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


    //Author: Farooq Ahmad
    //Date : 13/04/2016
    //Overview: This function will create the XML and Download 
    downloadClinicalSummaryXMLData: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FaceSheet_Export CCDA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
                    $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
                    if (!Clinical_ClinicalSummary.checkEncryption())
                        return;
                    var allRecordsparams = [];
                    var chkCheckedDataElements = $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']:checked");
                    allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
                    allRecordsparams["FromAdmin"] = "0";
                    allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                    allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                    allRecordsparams["ParentCtrl"] = "Clinical_ClinicalSummary";
                    allRecordsparams["NoteId"] = $('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();
                    allRecordsparams["Password"] = $('#' + Clinical_ClinicalSummary.params.PanelID + ' #txtPassword').val();
                    allRecordsparams["Template"] = "ClinicalSummary";
                    Clinical_ClinicalSummary.generateCCDA(Clinical_ClinicalSummary.CommandType.Download, allRecordsparams);

                }
                else {
                    utility.DisplayMessages("please select the clinical visit.", 3);
                    $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
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
                Clinical_ClinicalSummary.paramsComponents = [];
                $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']").each(function (i, item) {
                    var currentId = $(item).attr("id");
                    var objectChecked = new Object();
                    objectChecked.componentName = $(item).attr('name');
                    objectChecked.componentId = -(Clinical_ClinicalSummary.paramsComponents.length+1);
                    Clinical_ClinicalSummary.paramsComponents.push(objectChecked);
                })
            } else {
                Clinical_ClinicalSummary.setListComponents();
            }
            var chkDataElements = $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']").each(function (i, item) {
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
            var chkSelectAllDataElements = $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary input[type='checkbox'][id='chkSelectAllDataElements']");
            var chkDataElements = $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']");
            var chkCheckedDataElements = $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']:checked");

            if ($(obj).is(':checked')) {
                var objectChecked = new Object();
                objectChecked.componentName = $(obj).attr('name');
                objectChecked.componentId = (Clinical_ClinicalSummary.paramsComponents.length+1)*(-1);
                Clinical_ClinicalSummary.paramsComponents.push(objectChecked);

            } else {
                Clinical_ClinicalSummary.paramsComponents = $.grep(Clinical_ClinicalSummary.paramsComponents, function (e) {
                    return e.componentName != $(obj).attr('name');
                });
            }

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
                Clinical_ClinicalSummary.NotesLoad().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var NotesArray = JSON.parse(response.NotesLoad_JSON);
                        var ddlClinicalVisit = $('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit');
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
                            $('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit').val(NotesArray[0].NotesId.toString());
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
        var isEncrypted = $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #chkEncryption").prop("checked");
        if (isEncrypted) {
            if ($("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtPassword").val() == "") {
                utility.DisplayMessages("please enter password to Encrypt.", 3);
                $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else if ($("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtPassword").val() != $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtConfirmPassword").val()) {
                utility.DisplayMessages("password did not match confirm password.", 3);
                $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtConfirmPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else {
                $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtPassword").closest('div').removeClass("has-feedback has-error");
                $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtConfirmPassword").closest('div').removeClass("has-feedback has-error");
            }

        }
        return true;
    },

    //Author: Farooq Ahmad
    //Date: 04/04/2016
    //Overview: This function will generate the hash value
    generateCCDA: function (isFrom, ParentParams) {
        var param = new Object();
        if ($('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {

            param["NoteId"] = $('#' + Clinical_ClinicalSummary.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = param["PatientId"] = Clinical_ClinicalSummary.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, param["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {


                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    param["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    param["commandType"] = "XML";
                    if (Clinical_ClinicalSummary.paramsComponents && Clinical_ClinicalSummary.paramsComponents.length == 0) {
                        param["Components"] = Clinical_ClinicalSummary.getSelectedComponentJSONArray();
                    } else {
                        param["Components"] = Clinical_ClinicalSummary.paramsComponents;// Clinical_ClinicalSummary.getSelectedComponentJSONArray();
                    }
                    param["IsConfidential"] = $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #chkConfidential").prop("checked");
                    data = JSON.stringify(param);
                    MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
                        var responseDetail = response = JSON.parse(response);
                        if (response.status != false) {
                            response.data = JSON.parse(response.data);
                            $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                            if (isFrom == Clinical_ClinicalSummary.CommandType.ViewHtml) {
                                param["XMLData"] = response.data.xmlData; //base64 String
                                param["ParentCtrl"] = "Clinical_ClinicalSummary";
                                var componentActionPan = "Clinical_ClinicalSummaryHTML";
                                LoadActionPan(componentActionPan, param);
                            }
                            else if (isFrom == Clinical_ClinicalSummary.CommandType.Download) {

                                if (!Clinical_ClinicalSummary.checkEncryption())
                                    return;
                                param["XMLData"] = response.data.xmlData;
                                param["Encryption"] = $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #chkEncryption").prop("checked");
                                param["IncludeHashCode"] = $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #chkIncludeHashCode").prop("checked");
                                param["Password"] = $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtPassword").val();
                                param["commandType"] = "DOWNLOAD";
                                param["SummaryType"] = "1"; // 1 for clinical Summary
                                data = JSON.stringify(param);
                                MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {
                                        var zip = new JSZip();
                                        if ($("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #chkIncludeHashCode").prop("checked"))
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

    generateCancerCCDA: function (isFrom, ParentParams) {
        Clinical_ProgressNote.FillNotes(null, ParentParams["NoteId"]).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var param = new Object();
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                param["ProviderId"] = Clinical_Notes_detail.ProviderId;
                param["NoteId"] = ParentParams.NoteId;
                param["PatientId"] = ParentParams.PatientId;
                param["commandType"] = "xmlCancerReport";
                param["Components"] = ParentParams.chkCheckedDataElements;
                param["IsConfidential"] = "False";
                data = JSON.stringify(param);
                MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
                    var responseDetail = response = JSON.parse(response);
                    if (response.status != false) {
                        response.data = JSON.parse(response.data);
                        //$("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                        if (isFrom == Clinical_ClinicalSummary.CommandType.ViewHtml) {
                            param["XMLData"] = response.data.xmlData; //base64 String
                            //param["ParentCtrl"] = "clinicalTabProgressNote";
                            var componentActionPan = "Clinical_ClinicalSummaryHTML";
                            LoadActionPan(componentActionPan, param);
                        }
                        else if (isFrom == Clinical_ClinicalSummary.CommandType.Download) {

                            //if (!Clinical_ClinicalSummary.checkEncryption())
                            //    return;
                            param["XMLData"] = response.data.xmlData;
                            param["Encryption"] = "False";//$("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #chkEncryption").prop("checked");
                            param["IncludeHashCode"] = "False"; $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #chkIncludeHashCode").prop("checked");
                            param["Password"] = "";// $("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #txtPassword").val();
                            param["commandType"] = "DOWNLOAD";
                            param["SummaryType"] = "1"; // 1 for clinical Summary
                            data = JSON.stringify(param);
                            MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                                response = JSON.parse(response);
                                if (response.status != false) {
                                    var zip = new JSZip();
                                    if ($("#" + Clinical_ClinicalSummary.params.PanelID + " #frmClinicalSummary #chkIncludeHashCode").prop("checked"))
                                        zip.file("HashCode.txt", response.HashCode);
                                    var xml = zip.folder("XML");

                                    xml.file("XMLData.xml", response.XMLByte, { base64: true });

                                    var html = zip.folder("HTML");
                                    html.file("htmlData.html", response.HTMLByte, { base64: true });
                                    zip.generateAsync({ type: "blob" })
                                    .then(function (content) {
                                        saveAs(content, "CancerReport.zip");
                                    });
                                    utility.DisplayMessages("Cancer Report Downloaded Successfully.", 1);
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        return dfd.promise();
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
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
        if (Clinical_ClinicalSummary.params.PatientId != null) {
            objData["PatientId"] = Clinical_ClinicalSummary.params.PatientId;
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
                var chkCheckedDataElements = $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']:checked");
                $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements input[type='checkbox']").each(function () {
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


                            $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements ." + className).prop("checked", true);
                            if (className.toLowerCase() != 'radiologyresults') {
                                $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements ." + className).attr('value', componentId);
                            }

                        } catch (ex) {
                            console.log(ex);
                        }

                    }



                });
                $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements .VisitReason").prop("checked", true);
                $("#" + Clinical_ClinicalSummary.params.PanelID + " #divDataElements .VisitReason").attr('value', NotesId);
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

        Clinical_ClinicalSummary.searchAppointments(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ClinicalSummary.appointmentGridLoad(response);

                // Start 26/11/2015 Muhammad Irfan for Bug # EMR-25 

                var TableControl = "Clinical_ClinicalSummary #dgvClinical_ClinicalSummary";
                var PagingPanelControlID = "Clinical_ClinicalSummary #divFaceSheetAppointmentPaging";
                var ClassControlName = "Clinical_ClinicalSummary";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AppointmentsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    Clinical_ClinicalSummary.appointmentSearch(PageNumber, ResultPerPage);
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
        $("#" + Clinical_ClinicalSummary.params.PanelID + " #dgvClinical_ClinicalSummary").dataTable().fnDestroy();
        $("#" + Clinical_ClinicalSummary.params.PanelID + " #dgvClinical_ClinicalSummary tbody").find("tr").remove();
        if (response.AppointmentsCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.AppointmentsLoad_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td>' + utility.RemoveTimeFromDate(null, item.ScheduleDate) + '</td><td>' + item.SchReason + '</td><td>' + item.ProviderName + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#" + Clinical_ClinicalSummary.params.PanelID + " #dgvClinical_ClinicalSummary tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_ClinicalSummary.params.PanelID + " #divFaceSheetAppointmentPaging").css("display", "none");
            $("#" + Clinical_ClinicalSummary.params.PanelID + " #dgvClinical_ClinicalSummary").DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_ClinicalSummary.params.PanelID + " #dgvClinical_ClinicalSummary"))
            ;
        else
            $("#" + Clinical_ClinicalSummary.params.PanelID + " #dgvClinical_ClinicalSummary").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
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
        Clinical_ClinicalSummary.setListComponents();
        UnloadActionPan(Clinical_ClinicalSummary.params.ParentCtrl, 'Clinical_ClinicalSummary');
        objDeffered.resolve();
        return objDeffered;
    },

    setListComponents: function () {
        Clinical_ClinicalSummary.paramsComponents = [];
        var objectChecked = new Object();
        objectChecked.componentName = "DemographicDataElement";
        objectChecked.componentId = -1;
        Clinical_ClinicalSummary.paramsComponents.push(objectChecked);
        var objectChecked = new Object();
        objectChecked.componentName = "ProviderDataElement";
        objectChecked.componentId = -2;
        Clinical_ClinicalSummary.paramsComponents.push(objectChecked);
    }
}