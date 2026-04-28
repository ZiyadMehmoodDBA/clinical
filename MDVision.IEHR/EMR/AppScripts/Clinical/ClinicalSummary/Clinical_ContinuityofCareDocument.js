/*
Author: Sameer Ahmed
Date: 9/11/207
Overview: This file is created to show clinical Continuity of care Document. 
*/

Clinical_ContinuityofCareDocument = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_ContinuityofCareDocument.params = params;

        if (Clinical_ContinuityofCareDocument.params.PanelID != 'Clinical_ContinuityofCareDocument') {
            Clinical_ContinuityofCareDocument.params.PanelID = Clinical_ContinuityofCareDocument.params.PanelID + ' #Clinical_ContinuityofCareDocument';
        } else {
            Clinical_ContinuityofCareDocument.params.PanelID = 'Clinical_ContinuityofCareDocument';
        }
        if (Clinical_ContinuityofCareDocument.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_ContinuityofCareDocument.params.PanelID + " div#FaceSheetPager", Clinical_ContinuityofCareDocument.params.FaceSheetComponents, 'appointments');
        }
        Clinical_ContinuityofCareDocument.setListComponents();
        if (Clinical_ContinuityofCareDocument.bIsFirstLoad) {
            Clinical_ContinuityofCareDocument.bIsFirstLoad = false;
            Clinical_ContinuityofCareDocument.LoadNotes();
            $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit').on("change", function () {
                if ($(this).val() != "") {
                    Clinical_ContinuityofCareDocument.loadNotesData($(this).val());
                    Clinical_ContinuityofCareDocument.setListComponents();
                }
                else {
                    $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']").each(function () {
                        if ($(this).attr("disabled") != "disabled")
                            $(this).prop("checked", false);
                    });
                    Clinical_ContinuityofCareDocument.ShowSectionPreference();
                }
            });
            $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #chkEncryption').on("change", function () {
                if (!$(this).prop("checked")) {
                    $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #txtPassword').val('');
                }
            })
        }
        if (globalAppdata["isTransitionCareDirectProject"] && globalAppdata["isTransitionCareDirectProject"].toLowerCase() == "false")
            $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #btnTransmitCCDA').addClass("hidden");

        Clinical_ContinuityofCareDocument.ShowSectionPreference();

    },




    CommandType: {
        ViewHtml: 1,
        Download: 2,
        GenerateHash: 3,
        TransmitCCDA: 4
    },

    //Author: Sameer Ahmed
    //Date: 11/10/2017
    //Overview: This function is to display Clinical Summary
    displayContinuitCareDocumentHTML: function (summaryType) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FaceSheet_Export CCDA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
                    $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
                    var allRecordsparams = [];
                    var chkCheckedDataElements = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
                    allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
                    allRecordsparams["FromAdmin"] = "0";
                    allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                    allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                    allRecordsparams["ParentCtrl"] = "Clinical_ContinuityofCareDocument";
                    allRecordsparams["NoteId"] = $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val();

                    Clinical_ContinuityofCareDocument.generateCCDA(Clinical_ContinuityofCareDocument.CommandType.ViewHtml, allRecordsparams);
                   
                }
                else {
                    utility.DisplayMessages("please select the clinical visit.", 3);
                    $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });


    },

    TransmitCCDA: function (toEmail, IncludeXML, IncludeHTML, DocType) {
        if ($('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            Clinical_ContinuityofCareDocument.params["toEmail"] = allRecordsparams["toEmail"] = toEmail;
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_ContinuityofCareDocument";
            allRecordsparams["NoteId"] = $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["ReferralProviderId"] = $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlReferringProvider option:selected').val();
            allRecordsparams["ReferralReason"] = $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #txtReferralReason').val();
            allRecordsparams["IncludeXML"] = IncludeXML;
            allRecordsparams["IncludeHTML"] = IncludeHTML;
            allRecordsparams["DOS"] = $("#frmClinicalContinuityofCareDocument #ddlClinicalVisit :selected").text();
            allRecordsparams["DocType"] = DocType;
            Clinical_ContinuityofCareDocument.generateCCDA(Clinical_ContinuityofCareDocument.CommandType.TransmitCCDA, allRecordsparams);
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },

    //Author: Sameer Ahmed
    //Date: 11/10/2017
    //Overview: This function is created to Load Clinical Summary HTML
    downloadContinuitCareDocumentPDFData: function () {
        if ($('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            var objData = new Object();

            objData["NoteId"] = $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = objData["PatientId"] = Clinical_ContinuityofCareDocument.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, objData["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    objData["commandType"] = "PDF";
                    objData["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    objData["Components"] = Clinical_ContinuityofCareDocument.getSelectedComponentJSONArray();

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


    //Author: Sameer Ahmed
    //Date: 11-04-2017
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

    //Author: Sameer Ahmed
    //Date: 11-04-2017
    //Overview: This function will return the selected compenent JSON Object Array
    getSelectedComponentJSONArray: function () {
        var Components = [], compId = -1;
        $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']:checked").each(function (index, item) {
            if ($(item).attr('value') != null) {
                var CompIds = $(item).attr('value').split(",");
                for (var comId in CompIds) {
                    componentId = Clinical_ContinuityofCareDocument.TryParseInt(CompIds[comId], 0);
                    if (componentId == 0) {
                        componentId = compId--;
                    }
                    var obj = {
                        componentId: componentId,
                        componentName: $(item).attr('class'),
                    };
                    Components.push(obj);
                }
                componentId = Clinical_ContinuityofCareDocument.TryParseInt($(item).attr('value'), 0);
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


    //Author: Sameer Ahmed
    //Date:  11/10/2017
    //Overview: This function will create the XML and Download 
    downloadContinuitCareDocumentXMLData: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FaceSheet_Export CCDA", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
                    $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
                    if (!Clinical_ContinuityofCareDocument.checkEncryption())
                        return;
                    var allRecordsparams = [];
                    var chkCheckedDataElements = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
                    allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
                    allRecordsparams["FromAdmin"] = "0";
                    allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                    allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                    allRecordsparams["ParentCtrl"] = "Clinical_ContinuityofCareDocument";
                    allRecordsparams["NoteId"] = $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val();
                    allRecordsparams["Password"] = $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #txtPassword').val();
                    allRecordsparams["Template"] = "ClinicalSummary";
                    Clinical_ContinuityofCareDocument.generateCCDA(Clinical_ContinuityofCareDocument.CommandType.Download, allRecordsparams);
                }
                else {
                    utility.DisplayMessages("please select the clinical visit.", 3);
                    $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
                }

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });
    },

    //Author: Sameer Ahmed
    //Date: 01/04/2016
    //Overview: This function is to select Data Elements
    selectAllDataElements: function (obj) {
        if (obj != null) {
            var isChecked = false;
            if ($(obj).prop("checked") == true) {
                isChecked = true;
                Clinical_ContinuityofCareDocument.paramsComponents = [];
                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #divDataElements input[type='checkbox']").each(function (i, item) {
                    var currentId = $(item).attr("id");
                    var objectChecked = new Object();
                    objectChecked.componentName = $(item).attr('name');
                    objectChecked.componentId = -(Clinical_ContinuityofCareDocument.paramsComponents.length + 1);
                    Clinical_ContinuityofCareDocument.paramsComponents.push(objectChecked);
                })
            }
            else {
                Clinical_ContinuityofCareDocument.setListComponents();
            }
            var chkDataElements = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']").each(function (i, item) {
                var currentId = $(item).attr("id");
                if (currentId != null && currentId != "chkDemographicDataElement" && currentId != "chkProviderDataElement") {
                    $(item).prop("checked", isChecked);
                }
            });
        }
    },

    //Author: Sameer Ahmed
    //Date: 01/04/2016
    //Overview: This function is to select Data Element
    selectDataElement: function (obj) {
        if (obj != null) {
            var chkSelectAllDataElements = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument input[type='checkbox'][id='chkSelectAllDataElements']");
            var chkDataElements = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']");
            var chkCheckedDataElements = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']:checked");

            if ($(obj).is(':checked')) {
                var objectChecked = new Object();
                objectChecked.componentName = $(obj).attr('name');
                objectChecked.componentId = (Clinical_ContinuityofCareDocument.paramsComponents.length + 1) * (-1);
                Clinical_ContinuityofCareDocument.paramsComponents.push(objectChecked);

            } else {
                Clinical_ContinuityofCareDocument.paramsComponents = $.grep(Clinical_ContinuityofCareDocument.paramsComponents, function (e) {
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
    setListComponents: function () {
        Clinical_ContinuityofCareDocument.paramsComponents = [];
        var objectChecked = new Object();
        objectChecked.componentName = "DemographicDataElement";
        objectChecked.componentId = -1;
        Clinical_ContinuityofCareDocument.paramsComponents.push(objectChecked);
        var objectChecked = new Object();
        objectChecked.componentName = "ProviderDataElement";
        objectChecked.componentId = -2;
        Clinical_ContinuityofCareDocument.paramsComponents.push(objectChecked);
    },
    //Author: Sameer Ahmed
    //Date: 9/11/207
    //Overview: This function is created to LoadNotes
    LoadNotes: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Notes_Notes", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_ContinuityofCareDocument.NotesLoad().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var NotesArray = JSON.parse(response.NotesLoad_JSON);
                        var ddlClinicalVisit = $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit');
                        $(ddlClinicalVisit).find('option').remove();
                        $(ddlClinicalVisit).append($('<option>', {
                            value: '',
                            text: '--Select--'
                        }));
                        for (var Note in NotesArray) {
                            var date = new Date(NotesArray[Note].VisitDate);
                            var displayValue = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                            displayValue = displayValue + ' ' + NotesArray[Note].VisitTime;

                            if (NotesArray[Note].NoteStatus.toLowerCase() == "signed")
                            {
                                $(ddlClinicalVisit).append($('<option>', {
                                    value: NotesArray[Note].NotesId,
                                    text: displayValue
                                }))
                            }
                        }

                        if (NotesArray.length == 1) {
                            $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit').val(NotesArray[0].NotesId.toString());
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


    //Author : Sameer Ahmed
    //Date : 27/04/2016
    //OverView  : If the Encryption is Selected Than Password Is Mendatory
    checkEncryption: function () {
        var isEncrypted = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkEncryption").prop("checked");
        if (isEncrypted) {
            if ($("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtPassword").val() == "") {
                utility.DisplayMessages("please enter password to Encrypt.", 3);
                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else if ($("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtPassword").val() != $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtConfirmPassword").val()) {
                utility.DisplayMessages("password did not match confirm password.", 3);
                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtConfirmPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else {
                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtPassword").closest('div').removeClass("has-feedback has-error");
                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtConfirmPassword").closest('div').removeClass("has-feedback has-error");
            }

        }
        return true;
    },

    //Author: Sameer Ahmed
    //Date: 04/04/2016
    //Overview: This function will generate the hash value
    generateCCDA: function (isFrom, ParentParams) {
        var param = new Object();
        if ($('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {

            param["NoteId"] = $('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = param["PatientId"] = Clinical_ContinuityofCareDocument.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, param["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {


                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    param["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    param["commandType"] = "XMLContinuityofCareDocument";
                    if (Clinical_ContinuityofCareDocument.paramsComponents && Clinical_ContinuityofCareDocument.paramsComponents.length == 0) {
                        param["Components"] = Clinical_ContinuityofCareDocument.getSelectedComponentJSONArray();
                    } else {
                        param["Components"] = Clinical_ContinuityofCareDocument.paramsComponents;// Clinical_ClinicalSummary.getSelectedComponentJSONArray();
                    }

                    param["IsConfidential"] = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkConfidential").prop("checked");
                    data = JSON.stringify(param);
                    MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
                        var responseDetail = response = JSON.parse(response);
                        if (response.status != false) {
                            response.data = JSON.parse(response.data);
                            $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                            if (isFrom == Clinical_ContinuityofCareDocument.CommandType.ViewHtml) {
                                param["XMLData"] = response.data.xmlData; //base64 String
                                param["ParentCtrl"] = "Clinical_ContinuityofCareDocument";
                                var componentActionPan = "Clinical_ClinicalSummaryHTML";
                                LoadActionPan(componentActionPan, param);
                                MU_Alerts.UpdateMUAlertProfile("TransitionOfCare", ParentParams["NoteId"], ParentParams["PatientId"]);
                            }
                            else if (isFrom == Clinical_ContinuityofCareDocument.CommandType.Download) {

                                if (!Clinical_ContinuityofCareDocument.checkEncryption())
                                    return;
                                param["XMLData"] = response.data.xmlData;
                                param["Encryption"] = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkEncryption").prop("checked");
                                param["IncludeHashCode"] = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkIncludeHashCode").prop("checked");
                                param["Password"] = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtPassword").val();
                                param["commandType"] = "DOWNLOAD";
                                param["SummaryType"] = "1"; // 1 for clinical Summary
                                data = JSON.stringify(param);
                                MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {
                                        var zip = new JSZip();
                                        if ($("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkIncludeHashCode").prop("checked"))
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
                                        MU_Alerts.UpdateMUAlertProfile("TransitionOfCare", ParentParams["NoteId"], ParentParams["PatientId"]);
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            else if (isFrom == Clinical_ContinuityofCareDocument.CommandType.TransmitCCDA) {
                                if (!Clinical_ContinuityofCareDocument.checkEncryption())
                                    return;
                                param["IncludeXML"] = ParentParams["IncludeXML"];
                                param["IncludeHTML"] = ParentParams["IncludeHTML"];
                                param["XMLData"] = response.data.xmlData;
                                param["Encryption"] = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkEncryption").prop("checked");
                                param["IncludeHashCode"] = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #chkIncludeHashCode").prop("checked");
                                param["Password"] = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #txtPassword").val();
                                param["commandType"] = "SendEmail";
                                param["toEmail"] = Clinical_ContinuityofCareDocument.params["toEmail"];
                                param["msgType"] = $('#frmTransmitContinuityofCareDocument #selectmsgType').val();
                                param["MessageDetail"] = $('#frmTransmitContinuityofCareDocument #txtMessageDtl').val();

                                param["DOS"] = ParentParams["DOS"];
                                param["DocType"] = ParentParams["DocType"];
                                param["PatientAccountNo"] = $('#hfAccountNo').val();

                                data = JSON.stringify(param);
                                MDVisionService.APIService(data, "CLINICALSUMMARY", "SendEmail").done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {

                                        utility.DisplayMessages(response.data, 1);

                                        if ($('#frmTransmitContinuityofCareDocument #selectmsgType').val() == "direct")
                                            MU_Alerts.UpdateMUAlertProfile("TransitionOfCare,SecureMessaging", ParentParams["NoteId"], ParentParams["PatientId"]);
                                        else
                                            MU_Alerts.UpdateMUAlertProfile("TransitionOfCare", ParentParams["NoteId"], ParentParams["PatientId"]);
                                    }
                                    else {
                                        utility.DisplayMessages(response.data, 3);
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

        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
        }
    },

    Transmit: function () {
        if ($('#' + Clinical_ContinuityofCareDocument.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var param = [];
            param["ParentCtrl"] = "Clinical_ContinuityofCareDocument";
            param["DocType"] = "Continuity of Care";
            var componentActionPan = "Clinical_TransmitContinuityofCareDocument";
            LoadActionPan(componentActionPan, param);
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #frmClinicalContinuityofCareDocument #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }


    },

    S4: function () {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    },

    //Author: Sameer Ahmed
    //Date: 9/11/207
    //Overview: This function is created to LoadNotes
    NotesLoad: function () {
        var objData = new Object();
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 2000;
        if (Clinical_ContinuityofCareDocument.params.PatientId != null) {
            objData["PatientId"] = Clinical_ContinuityofCareDocument.params.PatientId;
        }
        objData["commandType"] = "SEARCH_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    /*
    Author: Sameer Ahmed
    Date: 9/11/207
    Overview: This function is load Note Data
    */
    loadNotesData: function (NotesId) {
        Clinical_ProgressNote.params.patientID = Clinical_ContinuityofCareDocument.params.PatientId;
        Clinical_ProgressNote.FillNotes(null, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                var myNoteText = $(Clinical_Notes_detail.NoteText);
                var chkCheckedDataElements = $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements input[type='checkbox']").each(function () {
                    if ($(this).attr("disabled") != "disabled")
                        $(this).prop("checked", false);
                });

                Clinical_ContinuityofCareDocument.ShowSectionPreference();

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


                            $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements ." + className).prop("checked", true);
                            if (className.toLowerCase() != 'radiologyresults') {
                                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements ." + className).attr('value', componentId);
                            }

                        } catch (ex) {
                            console.log(ex);
                        }

                    }



                });
                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements .VisitReason").prop("checked", true);
                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #ulDataElements .VisitReason").attr('value', NotesId);
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

        Clinical_ContinuityofCareDocument.searchAppointments(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ContinuityofCareDocument.appointmentGridLoad(response);

                // Start 26/11/2015 Muhammad Irfan for Bug # EMR-25 

                var TableControl = "Clinical_ContinuityofCareDocument #dgvClinical_ContinuityofCareDocument";
                var PagingPanelControlID = "Clinical_ContinuityofCareDocument #divFaceSheetAppointmentPaging";
                var ClassControlName = "Clinical_ContinuityofCareDocument";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AppointmentsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    Clinical_ContinuityofCareDocument.appointmentSearch(PageNumber, ResultPerPage);
                }), 10);

                // End 26/11/2015 Muhammad Irfan for Bug # EMR-25 

            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });
    },

    /*
    Author: Sameer Ahmed
    Date: 11/10/2017
    Overview: This function is created to load grid of appointments
    */
    appointmentGridLoad: function (response) {
        $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #dgvClinical_ContinuityofCareDocument").dataTable().fnDestroy();
        $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #dgvClinical_ContinuityofCareDocument tbody").find("tr").remove();
        if (response.AppointmentsCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.AppointmentsLoad_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td>' + utility.RemoveTimeFromDate(null, item.ScheduleDate) + '</td><td>' + item.SchReason + '</td><td>' + item.ProviderName + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #dgvClinical_ContinuityofCareDocument tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #divFaceSheetAppointmentPaging").css("display", "none");
            $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #dgvClinical_ContinuityofCareDocument").DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #dgvClinical_ContinuityofCareDocument"))
            ;
        else
            $("#" + Clinical_ContinuityofCareDocument.params.PanelID + " #dgvClinical_ContinuityofCareDocument").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $('.table-responsive').css('min-height', '220px');
    },
    /*
    Author: Sameer Ahmed
    Date: 11/10/2017
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
   Author: Sameer Ahmed
    Date: 11/10/2017
    Overview: This function is created to Unload screen
    */
    UnLoad: function () {
        var objDeffered = $.Deferred();
        Clinical_ContinuityofCareDocument.setListComponents();
        UnloadActionPan(Clinical_ContinuityofCareDocument.params.ParentCtrl, 'Clinical_ContinuityofCareDocument');
        objDeffered.resolve();
        return objDeffered;
    },

    SavePreferenceDisplayCCDA: function () {
        var sections = [];

        $.each($("#ulDataElements input[type='checkbox']:checked").not("[disabled]"), function (index, el) {
            sections.push(el.id);
        });
        localStorage.setItem("ccdaPreferenceContinuityofCare", JSON.stringify(sections));

        utility.DisplayMessages("Preference save successfully.", 1);
    },
    ShowSectionPreference: function () {
        var sections = JSON.parse(localStorage.getItem("ccdaPreferenceContinuityofCare")) || [];

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