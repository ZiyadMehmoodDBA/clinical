Clinical_HealthCareSurvey = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Clinical_HealthCareSurvey.params = params;

        if (Clinical_HealthCareSurvey.params.PanelID != 'Clinical_HealthCareSurvey') {
            Clinical_HealthCareSurvey.params.PanelID = Clinical_HealthCareSurvey.params.PanelID + ' #Clinical_HealthCareSurvey';
        } else {
            Clinical_HealthCareSurvey.params.PanelID = 'Clinical_HealthCareSurvey';
        }
        if (Clinical_HealthCareSurvey.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_HealthCareSurvey.params.PanelID + " div#FaceSheetPager", Clinical_HealthCareSurvey.params.FaceSheetComponents, 'appointments');
        }

        if (Clinical_HealthCareSurvey.bIsFirstLoad) {
            Clinical_HealthCareSurvey.bIsFirstLoad = false;
            Clinical_HealthCareSurvey.LoadNotes();
            Clinical_HealthCareSurvey.FillReferralProvider();
            $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit').on("change", function () {
                Clinical_HealthCareSurvey.loadNotesData($(this).val());
            });
            $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #chkEncryption').on("change", function () {
                if (!$(this).prop("checked")) {
                    $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #txtPassword').val('');
                }
            })
        }
    },


    FillReferralProvider: function () {
        var ReferringProviderData = '{"txtLastName":"","txtFirstName":"","txtNPI":"","chkActive":"1","chkActive_text":"Active","ddlEntity":"' + globalAppdata.SeletedEntityId + '"}';
        var data = "ReferringProviderData=" + ReferringProviderData + "&ReferringProviderID=undefined&PageNumber=1&RowsPerPage=2000";
        MDVisionService.defaultService(data, "ADMIN_REFERRING_PROVIDER", "SEARCH_REFERRING_PROVIDER").done(function (response) {
            if (response.status != "false") {
                var output = response.data;

                var output = JSON.parse(response.ReferringProviderLoad_JSON)
                var ddlReferringProvider = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlReferringProvider');
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

    CommandType: {
        ViewHtml: 1,
        Download: 2,
        GenerateHash: 3,
        TransmitCCDA: 4
    },

    displayHealthCareSurveyHTML: function (summaryType) {

        if ($('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_HealthCareSurvey";
            allRecordsparams["NoteId"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["ReferralProviderId"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlReferringProvider option:selected').val();
            allRecordsparams["ReferralReason"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #txtReferralReason').val();
            Clinical_HealthCareSurvey.generateCCDA(Clinical_HealthCareSurvey.CommandType.ViewHtml, allRecordsparams);
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },


    TransmitCCDA: function (toEmail, IncludeXML, IncludeHTML) {
        if ($('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            Clinical_HealthCareSurvey.params["toEmail"] = allRecordsparams["toEmail"] = toEmail;
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_HealthCareSurvey";
            allRecordsparams["NoteId"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["ReferralProviderId"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlReferringProvider option:selected').val();
            allRecordsparams["ReferralReason"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #txtReferralReason').val();
            allRecordsparams["IncludeXML"] = IncludeXML;
            allRecordsparams["IncludeHTML"] = IncludeHTML;
            Clinical_HealthCareSurvey.generateCCDA(Clinical_HealthCareSurvey.CommandType.TransmitCCDA, allRecordsparams);
        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },

    downloadHealthCareSurveyPDFData: function () {
        if ($('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            var objData = new Object();

            objData["NoteId"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = objData["PatientId"] = Clinical_HealthCareSurvey.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, objData["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                    objData["commandType"] = "PDF";
                    objData["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    objData["Components"] = Clinical_HealthCareSurvey.getSelectedComponentJSONArray();

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
        $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements input[type='checkbox']:checked").each(function (index, item) {
            if ($(item).attr('value') != null) {
                var CompIds = $(item).attr('value').split(",");
                for (var comId in CompIds) {
                    componentId = Clinical_HealthCareSurvey.TryParseInt(CompIds[comId], 0);
                    if (componentId == 0) {
                        componentId = compId--;
                    }
                    var obj = {
                        componentId: componentId,
                        componentName: $(item).attr('class'),
                    };
                    Components.push(obj);


                }
                componentId = Clinical_HealthCareSurvey.TryParseInt($(item).attr('value'), 0);
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


    downloadHealthCareSurveyXMLData: function (xmlType) {
        if ($('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {
            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #ddlClinicalVisit").closest('div').removeClass("has-feedback has-error");
            if (!Clinical_HealthCareSurvey.checkEncryption())
                return;
            var allRecordsparams = [];
            var chkCheckedDataElements = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            allRecordsparams["chkCheckedDataElements"] = chkCheckedDataElements;
            allRecordsparams["FromAdmin"] = "0";
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = "Clinical_HealthCareSurvey";
            allRecordsparams["NoteId"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            allRecordsparams["Password"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #txtPassword').val();
            var param = Clinical_HealthCareSurvey.CommandType.Download;
            if (xmlType == 'TransmitCCDA') {
                param = Clinical_HealthCareSurvey.CommandType.TransmitCCDA;
            }
            Clinical_HealthCareSurvey.generateCCDA(param, allRecordsparams);

        }
        else {
            utility.DisplayMessages("please select the clinical visit.", 3);
            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #ddlClinicalVisit").closest('div').addClass("has-feedback has-error");
        }
    },
    selectAllDataElements: function (obj) {
        if (obj != null) {
            var isChecked = false;
            if ($(obj).prop("checked") == true) {
                isChecked = true;
            }
            var chkDataElements = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements input[type='checkbox']").each(function (i, item) {
                var currentId = $(item).attr("id");
                if (currentId != null && currentId != "chkDemographicDataElement" && currentId != "chkProviderDataElement") {
                    $(item).prop("checked", isChecked);
                }
            });
        }
    },

    selectDataElement: function (obj) {
        if (obj != null) {
            var chkSelectAllDataElements = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey input[type='checkbox'][id='chkSelectAllDataElements']");
            var chkDataElements = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements input[type='checkbox']");
            var chkCheckedDataElements = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
            if (chkCheckedDataElements.length == chkDataElements.length) {
                chkSelectAllDataElements.prop("checked", true);
            }
            else {
                chkSelectAllDataElements.prop("checked", false);
            }
        }
    },

    LoadNotes: function () {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Notes_Notes", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_HealthCareSurvey.NotesLoad().done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var NotesArray = JSON.parse(response.NotesLoad_JSON);
                        var ddlClinicalVisit = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit');
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
                            $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit').val(NotesArray[0].NotesId.toString());
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


    checkEncryption: function () {
        var isEncrypted = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #chkEncryption").prop("checked");
        if (isEncrypted) {
            if ($("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #txtPassword").val() == "") {
                utility.DisplayMessages("please enter password to Encrypt.", 3);
                $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #txtPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else if ($("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #txtPassword").val() != $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #txtConfirmPassword").val()) {
                utility.DisplayMessages("password did not match confirm password.", 3);
                $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #txtConfirmPassword").closest('div').addClass("has-feedback has-error");
                return false;
            }
            else {
                $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #txtPassword").closest('div').removeClass("has-feedback has-error");
                $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #txtConfirmPassword").closest('div').removeClass("has-feedback has-error");
            }

        }
        return true;
    },

    generateCCDA: function (isFrom, ParentParams) {
        var Components = [];
        Components.push({
            componentId: -1,
            componentName: "DemographicDataElement",
        });

        Components.push({
            componentId: -2,
            componentName: "ProviderDataElement",
        });

        Components.push({
            componentId: -3,
            componentName: "SocialHistory",
        });

        Components.push({
            componentId: -4,
            componentName: "PayersSection",
        });
        Components.push({
            componentId: -5,
            componentName: "VisitReason"
        });
        Components.push({
            componentId: -6,
            componentName: "Vitals"
        });
        Components.push({
            componentId: -7,
            componentName: "OutPatientEnct"
        });
        Components.push({
            componentId: -8,
            componentName: "Medications"
        });
        Components.push({
            componentId: -9,
            componentName: "Immunization"
        });
        Components.push({
            componentId: -10,
            componentName: "ServiceProcedures"
        });
        Components.push({
            componentId: -11,
            componentName: "ProblemsHS"
        });
        Components.push({
            componentId: -12,
            componentName: "LabResults"
        });
        var param = new Object();
        if ($('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val() != '') {

            param["NoteId"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlClinicalVisit option:selected').val();
            Clinical_ProgressNote.params.patientID = param["PatientId"] = Clinical_HealthCareSurvey.params.PatientId;

            Clinical_ProgressNote.FillNotes(null, param["NoteId"]).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);

                    param["ProviderId"] = Clinical_Notes_detail.ProviderId;
                    param["commandType"] = "XMLHealthCareSurvey";
                    param["Components"] = Components;
                    param["referralProvider"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #ddlReferringProvider option:selected').val();
                    param["raferralReason"] = $('#' + Clinical_HealthCareSurvey.params.PanelID + ' #txtReferralReason').val();


                    data = JSON.stringify(param);
                    MDVisionService.APIService(data, "CLINICALSUMMARY", "ClinicalSummary").done(function (response) {
                        var responseDetail = response = JSON.parse(response);
                        if (response.status != false) {
                            response.data = JSON.parse(response.data);
                            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #hfXMLData").val(response.data.xmlData); //Base64 string in hidden field
                            if (isFrom == Clinical_HealthCareSurvey.CommandType.ViewHtml) {
                                param["XMLData"] = response.data.xmlData; //base64 String
                                param["ParentCtrl"] = "Clinical_HealthCareSurvey";
                                var componentActionPan = "Clinical_ClinicalSummaryHTML";
                                LoadActionPan(componentActionPan, param);
                            }
                            else if (isFrom == Clinical_HealthCareSurvey.CommandType.Download) {

                                if (!Clinical_HealthCareSurvey.checkEncryption())
                                    return;
                                param["XMLData"] = response.data.xmlData;
                                param["Encryption"] = false;
                                param["IncludeHashCode"] = false;
                                param["Password"] = "";
                                param["commandType"] = "DOWNLOAD";
                                param["Template"] = "HealthCareSurvey";
                                param["SummaryType"] = "2"; // 2 for HealthCareSurvey
                                data = JSON.stringify(param);
                                MDVisionService.APIService(data, "CLINICALSUMMARY", "DownloadFile").done(function (response) {
                                    response = JSON.parse(response);
                                    if (response.status != false) {
                                        var zip = new JSZip();
                                        if ($("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #chkIncludeHashCode").prop("checked"))
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
                            else if (isFrom == Clinical_HealthCareSurvey.CommandType.TransmitCCDA) {
                                if (!Clinical_HealthCareSurvey.checkEncryption())
                                    return;
                                param["IncludeXML"] = ParentParams["IncludeXML"];
                                param["IncludeHTML"] = ParentParams["IncludeHTML"];
                                param["XMLData"] = response.data.xmlData;
                                param["Encryption"] = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #chkEncryption").prop("checked");
                                param["IncludeHashCode"] = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #chkIncludeHashCode").prop("checked");
                                param["Password"] = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #frmClinicalHealthCareSurvey #txtPassword").val();
                                param["commandType"] = "SendEmail";
                                param["toEmail"] = Clinical_HealthCareSurvey.params["toEmail"];
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


    NotesLoad: function () {
        var objData = new Object();
        objData["PageNumber"] = 1;
        objData["RowsPerPage"] = 2000;
        if (Clinical_HealthCareSurvey.params.PatientId != null) {
            objData["PatientId"] = Clinical_HealthCareSurvey.params.PatientId;
        }
        objData["commandType"] = "SEARCH_CLINICAL_NOTES";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CLINICALNOTES", "ClinicalNotes");
    },

    loadNotesData: function (NotesId) {
        Clinical_ProgressNote.params.patientID = Clinical_ClinicalSummary.params.PatientId;
        Clinical_ProgressNote.FillNotes(null, NotesId).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                var Clinical_Notes_detail = JSON.parse(response.NotesFill_JSON);
                var myNoteText = $(Clinical_Notes_detail.NoteText);
                var chkCheckedDataElements = $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements input[type='checkbox']:checked");
                $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements input[type='checkbox']").each(function () {
                    if ($(this).attr("disabled") != "disabled")
                        $(this).removeAttr("checked")
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


                            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements ." + className).attr("checked", "checked");
                            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements ." + className).attr('value', componentId);
                        } catch (ex) {
                            console.log(ex);
                        }

                    }



                });
                $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements .VisitReason").attr("checked", "checked");
                $("#" + Clinical_HealthCareSurvey.params.PanelID + " #ulDataElements .VisitReason").attr('value', NotesId);
            }
            else {
                //  utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    appointmentSearch: function (PageNo, rpp) {

        Clinical_HealthCareSurvey.searchAppointments(PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_HealthCareSurvey.appointmentGridLoad(response);


                var TableControl = "Clinical_HealthCareSurvey #dgvClinical_HealthCareSurvey";
                var PagingPanelControlID = "Clinical_HealthCareSurvey #divFaceSheetAppointmentPaging";
                var ClassControlName = "Clinical_HealthCareSurvey";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.AppointmentsCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (myJSON, PageNumber, ResultPerPage) {
                    Clinical_HealthCareSurvey.appointmentSearch(PageNumber, ResultPerPage);
                }), 10);


            }
            else {
                utility.DisplayMessages(response.Message, 3);

            }
        });
    },
    appointmentGridLoad: function (response) {
        $("#" + Clinical_HealthCareSurvey.params.PanelID + " #dgvClinical_HealthCareSurvey").dataTable().fnDestroy();
        $("#" + Clinical_HealthCareSurvey.params.PanelID + " #dgvClinical_HealthCareSurvey tbody").find("tr").remove();
        if (response.AppointmentsCount > 0) {
            var AppointmentSearchJSONData = JSON.parse(response.AppointmentsLoad_JSON);
            $.each(AppointmentSearchJSONData, function (i, item) {
                var $row = $('<tr/>');

                $row.append('<td>' + utility.RemoveTimeFromDate(null, item.ScheduleDate) + '</td><td>' + item.SchReason + '</td><td>' + item.ProviderName + '</td><td>' + item.Time + '</td><td>' + item.Duration + '</td><td>' + item.AppointmentStatus + '</td>');

                $("#" + Clinical_HealthCareSurvey.params.PanelID + " #dgvClinical_HealthCareSurvey tbody").last().append($row);
            });
        }
        else {
            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #divFaceSheetAppointmentPaging").css("display", "none");
            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #dgvClinical_HealthCareSurvey").DataTable({
                "language": {
                    "emptyTable": "No Schedule Found"
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }

        if ($.fn.dataTable.isDataTable("#" + Clinical_HealthCareSurvey.params.PanelID + " #dgvClinical_HealthCareSurvey"))
            ;
        else
            $("#" + Clinical_HealthCareSurvey.params.PanelID + " #dgvClinical_HealthCareSurvey").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
        // $('.table-responsive').css('min-height', '220px');
    },
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
    UnLoad: function () {
        var objDeffered = $.Deferred();
        UnloadActionPan(Clinical_HealthCareSurvey.params.ParentCtrl, 'Clinical_HealthCareSurvey');
        objDeffered.resolve();
        return objDeffered;
    },
}