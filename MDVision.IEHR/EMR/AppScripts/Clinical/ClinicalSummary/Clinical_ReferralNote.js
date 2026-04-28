/*
 Author: Sameer Ahmed
Date: 31/03/2016
Overview: This file is created to show clinical summary
*/

Clinical_ReferralNote = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_ReferralNote.params = params;

        if (Clinical_ReferralNote.params.PanelID != 'Clinical_ReferralNote') {
            Clinical_ReferralNote.params.PanelID = Clinical_ReferralNote.params.PanelID + ' #Clinical_ReferralNote';
        } else {
            Clinical_ReferralNote.params.PanelID = 'Clinical_ReferralNote';
        }
        if (Clinical_ReferralNote.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_ReferralNote.params.PanelID + " div#FaceSheetPager", Clinical_ReferralNote.params.FaceSheetComponents, 'appointments');
        }
        Clinical_ReferralNote.setListComponents();
        if (Clinical_ReferralNote.bIsFirstLoad) {
            Clinical_ReferralNote.bIsFirstLoad = false;
            Clinical_ReferralNote.LoadNotes();
            Clinical_ReferralNote.FillReferralProvider();
            $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit').on("change", function () {
                Clinical_ReferralNote.loadNotesData($(this).val());
                Clinical_ReferralNote.setListComponents();
            });
            $('#' + Clinical_ReferralNote.params.PanelID + ' #chkEncryption').on("change", function () {
                if (!$(this).prop("checked")) {
                    $('#' + Clinical_ReferralNote.params.PanelID + ' #txtPassword').val('');
                }
            })
        }

        Clinical_ReferralNote.ShowSectionPreference();
        if (globalAppdata["isTransitionCareDirectProject"] && globalAppdata["isTransitionCareDirectProject"].toLowerCase() == "false")
            $('#' + Clinical_ReferralNote.params.PanelID + ' #btnTransmitCCDA').addClass("hidden");
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
                var ddlReferringProvider = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlReferringProvider');
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

    // Author: Sameer Ahmed
    //Date: 25/04/2016
    CommandType: {
        ViewHtml: 1,
        Download: 2,
        GenerateHash: 3,
        TransmitCCDA: 4
    },

    // Author: Sameer Ahmed
    //Date: 04/04/2016
    //Overview: This function is to display Clinical Summary
    displayReferralNoteHTML: function (summaryType) {

        if ($('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_ReferralNote";
            allRecordsparams["NoteId"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["ReferralProviderId"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlReferringProvider option:selected').val();
            allRecordsparams["ReferralReason"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #txtReferralReason').val();
            Clinical_ReferralNote.generateCCDA(Clinical_ReferralNote.CommandType.ViewHtml, allRecordsparams);
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },


    TransmitCCDA: function (toEmail, IncludeXML, IncludeHTML, DocType) {
        if ($('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            Clinical_ReferralNote.params["toEmail"] = allRecordsparams["toEmail"] = toEmail;
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_ReferralNote";
            allRecordsparams["NoteId"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["ReferralProviderId"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlReferringProvider option:selected').val();
            allRecordsparams["ReferralReason"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #txtReferralReason').val();
            allRecordsparams["IncludeXML"] = IncludeXML;
            allRecordsparams["IncludeHTML"] = IncludeHTML;
            allRecordsparams["DOS"] = $("#frmClinicalReferralNote #ddlClinicalVisit :selected").text();
            allRecordsparams["DocType"] = DocType;
            Clinical_ReferralNote.generateCCDA(Clinical_ReferralNote.CommandType.TransmitCCDA, allRecordsparams); 
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },

    //Author:Farooq Ahmad
    //Date: 07/04/2016
    //Overview: This function is created to Load Clinical Summary HTML
    downloadReferralNotePDFData: function () {
        if ($('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            var objData = new Object();

            objData["NoteId"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = objData["PatientId"] = Clinical_ReferralNote.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, objData["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    objData["commandType"] = "PDF";
                    objData["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    objData["Components"] = Clinical_ReferralNote.getSelectedComponentJSONArray();

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


    // Author: Sameer Ahmed
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

    // Author: Sameer Ahmed
    //Date: 15-04-2016
    //Overview: This function will return the selected compenent JSON Object Array
    getSelectedComponentJSONArray: function () {
        var Components = [], compId = -1;
        $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements input[type='checkbox']:checked").each(function (index, item) {
            if ($(item).attr('value') != null) {
                var CompIds = $(item).attr('value').split(",");
                for (var comId in CompIds) {
                    componentId = Clinical_ReferralNote.TryParseInt(CompIds[comId], 0);
                    if (componentId == 0) {
                        componentId = compId--;
                    }
                    var obj = {
                        componentId: componentId,
                        componentName: $(item).attr('class'),
                    };
                    Components.push(obj);


                }
                componentId = Clinical_ReferralNote.TryParseInt($(item).attr('value'), 0);
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


    // Author: Sameer Ahmed
    //Date : 13/04/2016
    //Overview: This function will create the XML and Download 
    downloadReferralNoteXMLData: function (xmlType) {
        if ($('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            if (!Clinical_ReferralNote.checkEncryption())
                return;
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_ReferralNote";
            allRecordsparams["NoteId"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["Password"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #txtPassword').val();
            var param = Clinical_ReferralNote.CommandType.Download;
            if (xmlType == 'TransmitCCDA') {
                param = Clinical_ReferralNote.CommandType.TransmitCCDA;
            }
            Clinical_ReferralNote.generateCCDA(param, allRecordsparams);

        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },

    // Author: Sameer Ahmed
    //Date: 01/04/2016
    //Overview: This function is to select Data Elements
    selectAllDataElements: function (obj) {
        if (obj != null) {
            var isChecked = false;
            if ($(obj).prop("checked") == true) {
                isChecked = true;
                Clinical_ReferralNote.paramsComponents = [];
                $("#" + Clinical_ReferralNote.params.PanelID + " #divDataElements input[type='checkbox']").each(function (i, item) {
                    var currentId = $(item).attr("id");
                    var objectChecked = new Object();
                    objectChecked.componentName = $(item).attr('name');
                    objectChecked.componentId = (Clinical_ReferralNote.paramsComponents.length + 1) * (-1);
                    Clinical_ReferralNote.paramsComponents.push(objectChecked);
                })

            }
            else {
                Clinical_ReferralNote.setListComponents();
            }
            var chkDataElements = $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements input[type='checkbox']").each(function (i, item) {
                var currentId = $(item).attr("id");
                if (currentId != null && currentId != "chkDemographicDataElement" && currentId != "chkProviderDataElement") {
                    $(item).prop("checked", isChecked);
                }
            });
        }
    },

    // Author: Sameer Ahmed
    //Date: 01/04/2016
    //Overview: This function is to select Data Element
    selectDataElement: function (obj) {
        if (obj != null) {
            var chkSelectAllDataElements = $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote input[type='checkbox'][id='chkSelectAllDataElements']");
            var chkDataElements = $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements input[type='checkbox']");
            var chkCheckedDataElements = $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements input[type='checkbox']:checked");

            if ($(obj).is(':checked')) {
                var objectChecked = new Object();
                objectChecked.componentName = $(obj).attr('name');
                objectChecked.componentId = -(Clinical_ReferralNote.paramsComponents.length + 1);
                Clinical_ReferralNote.paramsComponents.push(objectChecked);

            } else {
                Clinical_ReferralNote.paramsComponents = $.grep(Clinical_ReferralNote.paramsComponents, function (e) {
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
        Clinical_ReferralNote.paramsComponents = [];
        var objectChecked = new Object();
        objectChecked.componentName = "DemographicDataElement";
        objectChecked.componentId = -1;
        Clinical_ReferralNote.paramsComponents.push(objectChecked);
        var objectChecked = new Object();
        objectChecked.componentName = "ProviderDataElement";
        objectChecked.componentId = -2;
        Clinical_ReferralNote.paramsComponents.push(objectChecked);
    },
    // Author: Sameer Ahmed
    //Date: 31/03/2016
    //Overview: This function is created to LoadNotes
    LoadNotes: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Notes_Notes", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_ReferralNote.NotesLoad().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var NotesArray = JSON.parse(response.NotesLoad_JSON);
                        var ddlClinicalVisit = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit');
                        $(ddlClinicalVisit).find('option').remove();
                        $(ddlClinicalVisit).append($('<option>', {
                            value: '',
                            text: '--Select--'
                        }));
                        for (var Note in NotesArray) {
                            var date = new Date(NotesArray[Note].VisitDate);
                            var displayValue = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                            displayValue = displayValue + ' ' + NotesArray[Note].VisitTime;

                            if (NotesArray[Note].NoteStatus.toLowerCase() == "signed") {
                                $(ddlClinicalVisit).append($('<option>', {
                                    value: NotesArray[Note].NotesId,
                                    text: displayValue
                                }));
                            }
                        }

                        if (NotesArray.length == 1) {
                            $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit').val(NotesArray[0].NotesId.toString());
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



    //OverView  : If the Encryption is Selected Than Password Is Mendatory
    checkEncryption: function () {
        var isEncrypted = $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #chkEncryption").prop("checked");
        if (isEncrypted) {
            if ($("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #txtPassword").val() == "") {
                utility.DisplayMessages("please enter password to Encrypt.", 3);
                $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #txtPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else if ($("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #txtPassword").val() != $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #txtConfirmPassword").val()) {
                utility.DisplayMessages("password did not match confirm password.", 3);
                $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #txtConfirmPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else {
                $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #txtPassword").closest('div').removeClass("has-feedback has-error");
                $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #txtConfirmPassword").closest('div').removeClass("has-feedback has-error");
            }

        }
        return true;
    },

    // Author: Sameer Ahmed
    //Date: 04/04/2016
    //Overview: This function will generate the hash value
    generateCCDA: function (isFrom, ParentParams) {
        var param = new Object();
        if ($('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {

            param["NoteId"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = param["PatientId"] = Clinical_ReferralNote.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, param["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);

                    param["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    param["commandType"] = "XMLReferralNote";
                    if (Clinical_ReferralNote.paramsComponents && Clinical_ReferralNote.paramsComponents.length == 0) {
                        param["Components"] = Clinical_ReferralNote.getSelectedComponentJSONArray();
                    } else {
                        param["Components"] = Clinical_ReferralNote.paramsComponents;// Clinical_ClinicalSummary.getSelectedComponentJSONArray();
                    }

                    param["referralProvider"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #ddlReferringProvider option:selected').val();
                    param["raferralReason"] = $('#' + Clinical_ReferralNote.params.PanelID + ' #txtReferralReason').val();


                    data = JSON.stringify(param);
                    MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
                        var responseDetail = response = JSON.parse(response);
                        if (response.status != false) {
                            response.data = JSON.parse(response.data);
                            $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                            if (isFrom == Clinical_ReferralNote.CommandType.ViewHtml) {
                                param["XMLData"] = response.data.xmlData; //base64 String
                                param["ParentCtrl"] = "Clinical_ReferralNote";
                                var componentActionPan = "Clinical_ClinicalSummaryHTML";
                                LoadActionPan(componentActionPan, param);

                                MU_Alerts.UpdateMUAlertProfile("TransitionOfCare", ParentParams["NoteId"], ParentParams["PatientId"]);
                            }
                            else if (isFrom == Clinical_ReferralNote.CommandType.Download) {

                                if (!Clinical_ReferralNote.checkEncryption())
                                    return;
                                param["XMLData"] = response.data.xmlData;
                                param["Encryption"] = $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #chkEncryption").prop("checked");
                                param["IncludeHashCode"] = $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #chkIncludeHashCode").prop("checked");
                                param["Password"] = $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #txtPassword").val();
                                param["commandType"] = "DOWNLOAD";
                                param["Template"] = "ReferralNote";
                                param["SummaryType"] = "2"; // 2 for ReferralNote
                                data = JSON.stringify(param);
                                MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {
                                        var zip = new JSZip();
                                        if ($("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #chkIncludeHashCode").prop("checked"))
                                            zip.file("HashCode.txt", response.HashCode);
                                        var xml = zip.folder("XML");

                                        xml.file("XMLData.xml", response.XMLByte, { base64: true });

                                        var html = zip.folder("HTML");
                                        html.file("htmlData.html", response.HTMLByte, { base64: true });
                                        zip.generateAsync({ type: "blob" })
                                        .then(function (content) {
                                            saveAs(content, "CCDA.zip");
                                        });

                                        MU_Alerts.UpdateMUAlertProfile("TransitionOfCare", ParentParams["NoteId"], ParentParams["PatientId"]);
                                    }
                                    else {
                                        utility.DisplayMessages(response.Message, 3);
                                    }
                                });
                            }
                            else if (isFrom == Clinical_ReferralNote.CommandType.TransmitCCDA) {
                                if (!Clinical_ReferralNote.checkEncryption())
                                    return;
                                param["IncludeXML"] = ParentParams["IncludeXML"];
                                param["IncludeHTML"] = ParentParams["IncludeHTML"];
                                param["XMLData"] = response.data.xmlData;
                                param["Encryption"] = $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #chkEncryption").prop("checked");
                                param["IncludeHashCode"] = $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #chkIncludeHashCode").prop("checked");
                                param["Password"] = $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #txtPassword").val();
                                param["commandType"] = "SendEmail";
                                param["toEmail"] = Clinical_ReferralNote.params["toEmail"];
                                param["msgType"] = $('#frmTransmitReferralNote #selectmsgType').val();
                                param["MessageDetail"] = $('#frmTransmitReferralNote #txtMessageDtl').val();

                                param["DOS"] = ParentParams["DOS"];
                                param["DocType"] = ParentParams["DocType"];
                                param["PatientAccountNo"] = $('#hfAccountNo').val();

                                data = JSON.stringify(param);
                                MDVisionService.APIService(data, "CLINICALSUMMARY", "SendEmail").done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {

                                        utility.DisplayMessages(response.data, 1);

                                        if ($('#frmTransmitReferralNote #selectmsgType').val() == "direct")
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
        if ($('#' + Clinical_ReferralNote.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var param = [];
            param["ParentCtrl"] = "Clinical_ReferralNote";
            param["DocType"] = "Referral Note";
            var componentActionPan = "Clinical_TransmitReferralNote";
            LoadActionPan(componentActionPan, param);
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_ReferralNote.params.PanelID + " #frmClinicalReferralNote #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }


    },

    //S4: function () {
    //    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    //},

    // Author: Sameer Ahmed
    //Date: 31/03/2016
    //Overview: This function is created to LoadNotes
    NotesLoad: function () {
        var objData = new Object();
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 2000;
        if (Clinical_ReferralNote.params.PatientId != null) {
            objData["PatientId"] = Clinical_ReferralNote.params.PatientId;
        }
        objData["commandType"] = "SEARCH_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    /*
    Author  Sameer Ahmed
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
                var chkCheckedDataElements = $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
                $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements input[type='checkbox']").each(function () {
                    if ($(this).attr("disabled") != "disabled")
                        $(this).removeAttr("checked")
                });

                Clinical_ReferralNote.ShowSectionPreference();

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


                            $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements ." + className).attr("checked", "checked");
                            $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements ." + className).attr('value', componentId);
                        } catch (ex) {
                            console.log(ex);
                        }

                    }



                });
                $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements .VisitReason").attr("checked", "checked");
                $("#" + Clinical_ReferralNote.params.PanelID + " #ulDataElements .VisitReason").attr('value', NotesId);
            }
            else {
                //  utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    /*
   Author:Sameer Ahmed
    Date: 11/9/2017
    Overview: This function is created to search appointment
    */
    appointmentSearch: function (PageNo, rpp) {

        Clinical_ReferralNote.searchAppointments(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_ReferralNote.appointmentGridLoad(response);

                // Start 26/11/2015 Muhammad Irfan for Bug # EMR-25 

                var TableControl = "Clinical_ReferralNote #dgvClinical_ReferralNote";
                var PagingPanelControlID = "Clinical_ReferralNote #divFaceSheetAppointmentPaging";
                var ClassControlName = "Clinical_ReferralNote";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AppointmentsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    Clinical_ReferralNote.appointmentSearch(PageNumber, ResultPerPage);
                }), 10);

                // End 26/11/2015 Muhammad Irfan for Bug # EMR-25 

            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });
    },

    /*
   Author:Sameer Ahmed
    Date: 11/9/2017
    Overview: This function is created to load grid of appointments
    */
    appointmentGridLoad: function (response) {
        $("#" + Clinical_ReferralNote.params.PanelID + " #dgvClinical_ReferralNote").dataTable().fnDestroy();
        $("#" + Clinical_ReferralNote.params.PanelID + " #dgvClinical_ReferralNote tbody").find("tr").remove();
        if (response.AppointmentsCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.AppointmentsLoad_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td>' + utility.RemoveTimeFromDate(null, item.ScheduleDate) + '</td><td>' + item.SchReason + '</td><td>' + item.ProviderName + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#" + Clinical_ReferralNote.params.PanelID + " #dgvClinical_ReferralNote tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_ReferralNote.params.PanelID + " #divFaceSheetAppointmentPaging").css("display", "none");
            $("#" + Clinical_ReferralNote.params.PanelID + " #dgvClinical_ReferralNote").DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_ReferralNote.params.PanelID + " #dgvClinical_ReferralNote"))
            ;
        else
            $("#" + Clinical_ReferralNote.params.PanelID + " #dgvClinical_ReferralNote").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $('.table-responsive').css('min-height', '220px');
    },
    /*
   Author:Sameer Ahmed
    Date: 11/9/2017
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
   Author:Sameer Ahmed
    Date: 11/9/2017
    Overview: This function is created to Unload screen
    */
    UnLoad: function () {
        var objDeffered = $.Deferred();
        Clinical_ReferralNote.setListComponents();
        UnloadActionPan(Clinical_ReferralNote.params.ParentCtrl, 'Clinical_ReferralNote');
        objDeffered.resolve();
        return objDeffered;
    },
    SavePreferenceDisplayCCDA: function () {
        var sections = [];

        $.each($("#ulDataElements input[type='checkbox']:checked").not("[disabled]"), function (index, el) {
            sections.push(el.id);
        });
        localStorage.setItem("ccdaPreferenceReferralNote", JSON.stringify(sections));

        utility.DisplayMessages("Preference save successfully.", 1);
    },
    ShowSectionPreference: function () {

        var sections = JSON.parse(localStorage.getItem("ccdaPreferenceReferralNote")) || [];

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