//Author : Farooq Ahmad
//Date : 21/05/2016

Batch_ImportCCDA = {
    bIsFirstLoad: true,
    params: [],
    PatientId: 0,
    XMLContent: "",
    XMLParse_AllergiesLoad_JSON: [],
    XMLParse_ProblemLoad_JSON: [],
    XMLParse_MedicationLoad_JSON: [],
    ccdaValidationResult: '',
    PrivacySegmentedDocumentPaths: [],
    url: '',
    ParsedHTMLContentBody: null,
    PreferenceArrayUnsaved: [],
    WaitForHTMLLoadInIframeCounter: 0,
    HTMLContentLoadedInInframe: false,
    Load: function (params) {
        Batch_ImportCCDA.params = params;

        Batch_ImportCCDA.resetValues();
        Batch_ImportCCDA.resetPrivacySegmentedDocumentValues();

        if (Batch_ImportCCDA.bIsFirstLoad) {
            Batch_ImportCCDA.bIsFirstLoad = false;
        }
        var self = $('#pnlBatchImportCCDA');
        self.loadDropDowns(true).done(function () {

        });
        $(".toggledCcdaSections").remove();
        //if (Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "") {
        //    $("#" + Batch_ImportCCDA.params["PanelID"] + " #totalFiles").text('1 file(s) selected');
        //    Batch_ImportCCDA.SelectImportFileOfInbox();
        //}

        var select = $('#pnlBatchImportCCDA #ddlImportTypeCCDA');
        var Options = { 'CCDA': 'CCDA', 'CCR': 'CCR', 'CCD': 'CCD', 'CCD V3': 'CCD V3', 'Referral Note': 'Referral Note', 'Care Plan': 'Care Plan', 'C32': 'C 32' };
        $('option', select).remove();
        $.each(Options, function (text, key) {
            var option = new Option(key, text);
            if (key == 'CCD V3' || key == 'Referral Note' || key == 'Care Plan' || key == 'CCD V3' || key == 'C 32') {
                if (globalAppdata["isConsolidatedCDACreationPreformance"] && globalAppdata["isConsolidatedCDACreationPreformance"].toLowerCase() == "true")
                    select.append($(option));
            }
            else
                select.append($(option));
                
        });

        if (globalAppdata["isDataSegmentationPrivacy"] && globalAppdata["isDataSegmentationPrivacy"].toLowerCase() == "false")
            $("#pnlBatchImportCCDA #liImportPrivacySegmentedDocument").addClass("hidden");
        else
            $("#pnlBatchImportCCDA #liImportPrivacySegmentedDocument").removeClass("hidden");
        if (globalAppdata["isConsolidatedCDACreationPreformance"] && globalAppdata["isConsolidatedCDACreationPreformance"].toLowerCase() == "false") {
            $("#pnlBatchImportCCDA #btnSavePreference").addClass("hidden");
            $("#pnlBatchImportCCDA #divCCDAValidate").addClass("hidden");
            $("#pnlBatchImportCCDA #divReferenceFileName").addClass("hidden");
            $("#pnlBatchImportCCDA #divValidationObjective").addClass("hidden");
        }
        else {
            $("#pnlBatchImportCCDA #btnSavePreference").removeClass("hidden");
            $("#pnlBatchImportCCDA #divCCDAValidate").removeClass("hidden");
            $("#pnlBatchImportCCDA #divReferenceFileName").removeClass("hidden");
            $("#pnlBatchImportCCDA #divValidationObjective").removeClass("hidden");
        }
    },

    resetValues: function () {
        $("#" + Batch_ImportCCDA.params["PanelID"] + " #ulCheckBoxes").addClass("disableAll");
        $("#pnlBatchImportCCDA #btnCreatePatient").prop("disabled", true);
        $("#pnlBatchImportCCDA #btnCreatePatient").addClass("hide");
        Batch_ImportCCDA.CheckComponents(false);
        document.getElementById("Upload_Import_file").value = ""
        $("#" + Batch_ImportCCDA.params["PanelID"] + " #totalFiles").text('0 file(s) selected');
        $("#pnlBatchImportCCDA #tblPatient tbody").html('<tr><td colspan="4">No Available Record</td></tr>');
        document.getElementById("IframXML").srcdoc = '';
        Batch_ImportCCDA.XMLContent = "";
        Batch_ImportCCDA.XMLParse_AllergiesLoad_JSON = [];
        Batch_ImportCCDA.XMLParse_ProblemLoad_JSON = [];
        Batch_ImportCCDA.XMLParse_MedicationLoad_JSON = [];
        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", true);
        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", true);
        $("#pnlBatchImportCCDA #btnImportCCDA").removeClass("hide");
        $("#pnlBatchImportCCDA .liCCDAImport").addClass("hide");
        $("#pnlBatchImportCCDA #Medication").prop("checked", true);
        $("#pnlBatchImportCCDA #Allergies").prop("checked", true);
        $("#pnlBatchImportCCDA #Problems").prop("checked", true);
        Batch_ImportCCDA.ccdaValidationResult = "";
        $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerErrorList").addClass("hide");
        $("#pnlBatchImportCCDA #ddlImportFileTypeCCDA").val("0");
        $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").html("<option value='0'>Select</option>");
        Batch_ImportCCDA.PatientId = 0;
        if (Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "") {
            $("#" + Batch_ImportCCDA.params["PanelID"] + " #totalFiles").text('1 file(s) selected');
            Batch_ImportCCDA.SelectImportFileOfInbox();
        }
    },

    resetPrivacySegmentedDocumentValues: function () {
        $("#" + Batch_ImportCCDA.params.PanelID + " #btnImportPrivacySegmentedDocument").prop("disabled", true);
        document.getElementById("Import_Privacy_file").value = ""
        $("#" + Batch_ImportCCDA.params.PanelID + " #totalPrivacyFiles").text('0 file(s) selected');
        document.getElementById("IframeXMLPrivacySegmentedDocument").srcdoc = '';
    },

    SetDropdownListForValidation: function (ThisCTRL) {
        var value = $(ThisCTRL).val();
        if (value && value != "0" && value == "170.315_b4_CCDS_Amb") {
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").html("");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='Readme.txt'>Select</option>");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='170.315_b4_ccds_create_amb_sample1_v13.pdf'>170.315_b4_ccds_create_amb_sample1_v13.pdf</option>");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='170.315_b4_ccds_create_amb_sample1_v14.pdf'>170.315_b4_ccds_create_amb_sample1_v14.pdf</option>");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='170.315_b4_ccds_create_amb_sample2_v13.pdf'>170.315_b4_ccds_create_amb_sample2_v13.pdf</option>");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='170.315_b4_ccds_create_amb_sample2_v14.pdf'>170.315_b4_ccds_create_amb_sample2_v14.pdf</option>");
        } else if (value && value != "0" && value == "170.315_b1_ToC_Amb") {
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").html("");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='Readme.txt'>Select</option>");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='170.315_b1_toc_amb_sample1_v13.pdf'>170.315_b1_toc_amb_sample1_v13.pdf</option>");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='170.315_b1_toc_amb_sample1_v14.pdf'>170.315_b1_toc_amb_sample1_v14.pdf</option>");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='170.315_b1_toc_amb_sample2_v13.pdf'>170.315_b1_toc_amb_sample2_v13.pdf</option>");
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").append("<option value='170.315_b1_toc_amb_sample2_v14.pdf'>170.315_b1_toc_amb_sample2_v14.pdf</option>");
        } else {
            $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").html("<option value='0'>Select</option>");
        }
    },

    CheckComponents: function (mode) {
        if (mode) {
            var Medication = $("#pnlBatchImportCCDA #Medication").prop('checked');
            var Allergies = $("#pnlBatchImportCCDA #Allergies").prop('checked');
            var Problems = $("#pnlBatchImportCCDA #Problems").prop('checked');

            if (!Medication && !Allergies && !Problems)
                return false;
            else
                return true;
        }
        else {
            $("#pnlBatchImportCCDA #Medication").attr('checked', false);
            $("#pnlBatchImportCCDA #Allergies").attr('checked', false);
            $("#pnlBatchImportCCDA #Problems").attr('checked', false);
            return true;
        }
    },

    CheckComponentsImport: function (mode) {
        if (mode) {
            var Medication = $("#pnlBatchImportCCDA #Medication").prop('checked');
            var Allergies = $("#pnlBatchImportCCDA #Allergies").prop('checked');
            var Problems = $("#pnlBatchImportCCDA #Problems").prop('checked');
            var SmokingStatus = $("#pnlBatchImportCCDA #SmokingStatus").prop('checked');
            var LaboratoryTest = $("#pnlBatchImportCCDA #LaboratoryTest").prop('checked');
            var LaboratoryResult = $("#pnlBatchImportCCDA #LaboratoryResult").prop('checked');
            var VitalSigns = $("#pnlBatchImportCCDA #VitalSigns").prop('checked');
            var Procedures = $("#pnlBatchImportCCDA #Procedures").prop('checked');
            var Immunizations = $("#pnlBatchImportCCDA #Immunizations").prop('checked');
            var UDI = $("#pnlBatchImportCCDA #UDI").prop('checked');
            var Goals = $("#pnlBatchImportCCDA #Goals").prop('checked');
            var HealthConcerns = $("#pnlBatchImportCCDA #HealthConcerns").prop('checked');
            var PlanOfTreatment = $("#pnlBatchImportCCDA #PlanOfTreatment").prop('checked');

            if (!Medication && !Allergies && !Problems && !SmokingStatus && !LaboratoryTest && !LaboratoryResult && !VitalSigns && !Procedures && !Immunizations
                    && !UDI && !Goals && !HealthConcerns && !PlanOfTreatment)
                return false;
            else
                return true;
        }
        else {
            $("#pnlBatchImportCCDA #Medication").attr('checked', false);
            $("#pnlBatchImportCCDA #Allergies").attr('checked', false);
            $("#pnlBatchImportCCDA #Problems").attr('checked', false);
            return true;
        }
    },

    ValidateCCDA2: function () {
        var selectedTypeCCDA = $("#pnlBatchImportCCDA #ddlImportTypeCCDA").val();
        var formdata = new Object();

        // Convert it to a blob to upload
        //var blob = Batch_ImportCCDA.b64toBlob(utility.encodeURIData(Patient_MessageCompose.xmlByteData), "application/xml;base64");
        formdata.XMLContent = Patient_MessageCompose.xmlByteData;
        formdata.FileType = selectedTypeCCDA;
        formdata.DocfileType = "xml";
        formdata.DocfileName = "test";
        var errorsCount = 0;
        MDVisionService.APIServiceComplex(formdata, "ClinicalSummary", "ValidateCCDA").done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                Batch_ImportCCDA.ccdaValidationResult = JSON.parse(response.Message);
                Batch_ImportCCDA.url = response.url;
                document.getElementById("IframXML").src = response.dataHtml;
                $("#IframXML").removeAttr('srcdoc');
                var newArray = [];
                var ccdaValidationResults = Batch_ImportCCDA.ccdaValidationResult.ccdaValidationResults;
                if (ccdaValidationResults.length > 0) {
                    newArray = ccdaValidationResults.filter(function (el) {
                        return el.type == "C-CDA MDHT Conformance Error"
                            || el.type == "ONC 2015 S&CC Vocabulary Validation Conformance Error"
                            || el.type == "ONC 2015 S&CC Reference C-CDA Validation Error"
                    });
                    errorsCount = newArray.length;
                }
                if (errorsCount > 0) {
                    utility.DisplayMessages("The selected file is not valid. File contains " + errorsCount + " error(s).", 3);
                    $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainer").removeClass("hide");
                }
                else {
                    utility.DisplayMessages("The selected file is valid", 3);
                    $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainer").addClass("hide");
                    $("#pnlBatchImportCCDA #IframXML").removeClass("hide");
                    if (Batch_ImportCCDA.PatientId && Batch_ImportCCDA.PatientId > 0) {
                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", false);
                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", true);
                    } else {
                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", true);
                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", false);
                    }
                }
                Batch_ImportCCDA.DisplayReadableData();
            }
            else {
                var message = response.Message;
                if (message != "" && (message == "An error occurred while sending the request." || message == "an error occurred while sending the request.")) {
                    $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainer").addClass("hide");
                    $("#pnlBatchImportCCDA #IframXML").removeClass("hide");
                    if (Batch_ImportCCDA.PatientId && Batch_ImportCCDA.PatientId > 0) {
                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", false);
                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", true);
                    } else {
                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", true);
                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", false);
                    }
                }
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },

    ValidateCCDA: function () {

        $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerErrorList").addClass("hide");
        var x = document.getElementById("Upload_Import_file");
        var DocfileType = $("#pnlBatchImportCCDA #ddlImportFileTypeCCDA").val();
        var DocfileName = $("#pnlBatchImportCCDA #ddlImportFileNameTypeCCDA").val();
        var txt = "";
        if ('files' in x || (Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "")) {
            if (x.files.length == 0 && (Patient_MessageCompose == null || Patient_MessageCompose.xmlByteData == null || Patient_MessageCompose.xmlByteData == "")) {
                utility.DisplayMessages("Please select file.", 3);
            } else {
                if (DocfileType != "0") {
                    if (DocfileName != "0") {
                        var selectedTypeCCDA = $("#pnlBatchImportCCDA #ddlImportTypeCCDA").val();
                        var formdata = new Object();
                        if (Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "") {
                            formdata.XMLContent = Patient_MessageCompose.xmlByteData;
                            formdata.IsFile = false;
                        } else if (Batch_ImportCCDA.XMLContent && Batch_ImportCCDA.XMLContent != "") {
                            formdata.XMLContent = Batch_ImportCCDA.XMLContent;
                            formdata.IsFile = true;
                        }
                        formdata.FileType = selectedTypeCCDA;
                        formdata.DocfileType = DocfileType;
                        formdata.DocfileName = DocfileName;
                        var errorsCount = 0;
                        MDVisionService.APIServiceComplex(formdata, "ClinicalSummary", "ValidateCCDA").done(function (response) {
                            response = JSON.parse(response);
                            if (response.status) {
                                Batch_ImportCCDA.ccdaValidationResult = JSON.parse(response.Message);
                                if (response.url && response.url != "") {
                                    Batch_ImportCCDA.url = response.url;
                                    document.getElementById("IframXML").src = response.dataHtml;
                                    $("#IframXML").removeAttr('srcdoc');
                                }
                                var newArray = [];
                                var ccdaValidationResults = Batch_ImportCCDA.ccdaValidationResult.ccdaValidationResults;
                                if (ccdaValidationResults.length > 0) {
                                    newArray = ccdaValidationResults.filter(function (el) {
                                        return el.type == "C-CDA MDHT Conformance Error"
                                            || el.type == "ONC 2015 S&CC Vocabulary Validation Conformance Error"
                                            || el.type == "ONC 2015 S&CC Reference C-CDA Validation Error"
                                    });
                                    errorsCount = newArray.length;
                                }
                                if (errorsCount > 0) {
                                    utility.DisplayMessages("The selected file is not valid. File contains " + errorsCount + " error(s).", 3);
                                    $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainer").removeClass("hide");
                                }
                                else {
                                    utility.DisplayMessages("The selected file is valid", 3);
                                    $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainer").addClass("hide");
                                    $("#pnlBatchImportCCDA #IframXML").removeClass("hide");
                                    if (Batch_ImportCCDA.PatientId && Batch_ImportCCDA.PatientId > 0) {
                                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", false);
                                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", true);
                                    } else {
                                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", true);
                                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", false);
                                    }
                                }
                                Batch_ImportCCDA.DisplayReadableData();
                            }
                            else {
                                var message = response.Message;
                                if (message != "" && (message == "An error occurred while sending the request." || message == "an error occurred while sending the request.")) {
                                    $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainer").addClass("hide");
                                    $("#pnlBatchImportCCDA #IframXML").removeClass("hide");
                                    if (Batch_ImportCCDA.PatientId && Batch_ImportCCDA.PatientId > 0) {
                                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", false);
                                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", true);
                                    } else {
                                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", true);
                                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", false);
                                    }
                                }
                                utility.DisplayMessages(response.Message, 3);
                            }

                        });
                    } else {
                        utility.DisplayMessages("Please select reference filename", 3);
                    }
                } else {
                    utility.DisplayMessages("Please select validation objective", 3);
                }
            }
        }
        else {
            utility.DisplayMessages("Please select file.", 3);
        }
    },

    DisplayReadableData: function () {
        $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerErrorList").addClass("hide");
        $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainer").addClass("hide");
        $("#pnlBatchImportCCDA #IframXML").removeClass("hide");
    },

    DisplayErrorMessages: function () {
        $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerDocType").html("");
        $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerErrorList").addClass("hide");
        if (Batch_ImportCCDA.ccdaValidationResult != "" && Batch_ImportCCDA.ccdaValidationResult.ccdaValidationResults.length > 0) {
            $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainer").removeClass("hide");
            $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerDocType").removeClass("hide");
            $("#pnlBatchImportCCDA #IframXML").addClass("hide");
            var resultMetaData = Batch_ImportCCDA.ccdaValidationResult.resultsMetaData.resultMetaData;
            if (resultMetaData && resultMetaData.length > 0) {
                var metadata = "";
                var errorsCount = 0;
                var newArray = [];
                var ccdaValidationResults = Batch_ImportCCDA.ccdaValidationResult.ccdaValidationResults;
                resultMetaData.forEach(function (element) {
                    errorsCount = 0;
                    if (ccdaValidationResults.length > 0) {
                        errorsCount = ccdaValidationResults.filter(function (el) {
                            return el.type == element.type
                        }).length;
                        //errorsCount = newArray.length;
                    }
                    metadata += '<div class="list-group-item col-lg-12 col-md-12 col-xs-12">';
                    if (element.type == "C-CDA MDHT Conformance Error" || element.type == "ONC 2015 S&CC Vocabulary Validation Conformance Error" || element.type == "ONC 2015 S&CC Reference C-CDA Validation Error") {
                        metadata += '<span class="badge btn-danger">' + errorsCount + '</span>';
                    }
                    else if (element.type == "C-CDA MDHT Conformance Warning" || element.type == "ONC 2015 S&CC Vocabulary Validation Conformance Warning" || element.type == "ONC 2015 S&CC Reference C-CDA Validation Warning") {
                        metadata += '<span class="badge btn-warning">' + errorsCount + '</span>';
                    }
                    else {
                        metadata += '<span class="badge btn-info">' + errorsCount + '</span>';
                    }

                    metadata += '<a class="hoverpointer" data-msgtype="' + element.type + '" onclick="Batch_ImportCCDA.DisplayErrorMessagesDetail(this);">' + element.type + '</a>';
                    metadata += '</div>';
                });
                $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerDocType").append(metadata);
                $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainer").removeClass("hidden");

            }
        } else {
            $("#Batch_ImportCCDA #IframXML").removeClass("hide");
        }

    },

    DisplayErrorMessagesDetail: function (ThisCTRL) {
        if (Batch_ImportCCDA.ccdaValidationResult != "" && Batch_ImportCCDA.ccdaValidationResult.ccdaValidationResults.length > 0) {
            $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerErrorList").removeClass("hide");
            var messageType = $(ThisCTRL).attr("data-msgtype");
            $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerErrorList").html("");
            var ccdaValidationResults = Batch_ImportCCDA.ccdaValidationResult.ccdaValidationResults;
            var newArray = [];
            newArray = ccdaValidationResults.filter(function (el) {
                return el.type == messageType
            })
            if (newArray.length > 0) {
                newArray.forEach(function (element) {
                    var metadataDetail = "";

                    metadataDetail = '<div class="list-group-item col-lg-12 col-md-12 col-xs-12" >';
                    if (element.type == "C-CDA MDHT Conformance Error" || element.type == "ONC 2015 S&CC Vocabulary Validation Conformance Error" || element.type == "ONC 2015 S&CC Reference C-CDA Validation Error") {
                        metadataDetail += '<strong class="text-danger">Error </strong>';
                    }
                    else if (element.type == "C-CDA MDHT Conformance Warning" || element.type == "ONC 2015 S&CC Vocabulary Validation Conformance Warning" || element.type == "ONC 2015 S&CC Reference C-CDA Validation Warning") {
                        metadataDetail += '<strong class="text-warning">Warning </strong>';
                    }
                    else {
                        metadataDetail += '<strong class="text-info">Info </strong>';
                    }

                    metadataDetail += '<p style="display: inline; word-break: break-all; word-wrap: break-word;">' + element.description + '</p>';
                    if (element.xPath != null) {
                        metadataDetail += '<strong style="color: black;" class="ng-scope"><p style="display: inline; word-break: break-all; word-wrap: break-word;" class="ng-binding"> ' + element.xPath + '</p></strong></br>';
                    }
                    metadataDetail += '<div class="ng-scope"><u>Line number:</u><strong style="color: black;" class="ng-binding"> ' + element.documentLineNumber + '</strong></div>';
                    metadataDetail += '</div>';

                    $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerErrorList").append(metadataDetail);
                });


            }
        } else {
            $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerErrorList").addClass("hide");
            $("#pnlBatchImportCCDA #BatchImportCCDAXMLContainerErrorList").html("");
        }
    },

    NextImport: function () {
        var x = document.getElementById("Upload_Import_file");
        var txt = "";
        var ImportProviderId = $("#pnlBatchImportCCDA #ddlprovider").val();
        if ('files' in x || (Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "")) {
            if (x.files.length == 0 && (Patient_MessageCompose == null || Patient_MessageCompose.xmlByteData == null || Patient_MessageCompose.xmlByteData == "")) {
                utility.DisplayMessages("Please select file.", 3);
            } else {
                if (parseInt(Batch_ImportCCDA.PatientId) > 0) {
                    if ($("#" + Batch_ImportCCDA.params["PanelID"] + " #tblPatient #rbtnSelect").is(':checked')) {
                        if (Batch_ImportCCDA.CheckComponents(true)) {
                            var params = [];
                            if (Batch_ImportCCDA.params["ParentCtrl"] == "Patient_Search") {
                                params["ProviderId"] = "-1";
                                params["FromAdmin"] = "0";
                                params["RefCtrl"] = "txtSearchProvider";
                                params["RefCtrlHidden"] = "hfSearchProvider";
                                params["RefCtrlLabel"] = "lblSearchProvider";
                                params["RefCtrlLink"] = "lnkSearchProviderEdit";
                                params["ParentCtrl"] = "Batch_ImportCCDA";
                                params["ParentCtrlPanelID"] = "pnlBatchImportCCDA";
                                params["PatientId"] = parseInt(Batch_ImportCCDA.PatientId);
                                params["OpenFromPopup"] = "1";
                                Batch_ImportCCDA.params["SelectedPatientId"] = Batch_ImportCCDA.PatientId;//"87560";
                                Batch_ImportCCDA.params["ImportProviderId"] = ImportProviderId;//"87560";
                                Batch_ImportCCDA.params["Medication"] = $("#pnlBatchImportCCDA #Medication").prop('checked');
                                Batch_ImportCCDA.params["Allergies"] = $("#pnlBatchImportCCDA #Allergies").prop('checked');
                                Batch_ImportCCDA.params["Problems"] = $("#pnlBatchImportCCDA #Problems").prop('checked');
                                params["XMLContent"] = Batch_ImportCCDA.XMLContent;
                                LoadActionPan("Batch_PatientImportCCDA", params);
                            }
                            else {
                              
                                params["ParentCtrl"] = "batchTabImportCCDA";
                                params["PatientId"] = parseInt(Batch_ImportCCDA.PatientId);
                                params["PanelID"] = "pnlBatchPatientImportCCDA";
                                params["ActionPanContainer"] = "actionPanBatchPatientImportCCDA";
                                params["TabID"] = Batch_ImportCCDA.params.TabID;
                                params["FromAdmin"] = 1;
                                Batch_ImportCCDA.params["SelectedPatientId"] = Batch_ImportCCDA.PatientId;//"87560";
                                Batch_ImportCCDA.params["ImportProviderId"] = ImportProviderId;
                                Batch_ImportCCDA.params["Medication"] = $("#pnlBatchImportCCDA #Medication").prop('checked');
                                Batch_ImportCCDA.params["Allergies"] = $("#pnlBatchImportCCDA #Allergies").prop('checked');
                                Batch_ImportCCDA.params["Problems"] = $("#pnlBatchImportCCDA #Problems").prop('checked');
                                if ((Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "") || Batch_ImportCCDA.url != "") {
                                    params["XMLContent"] = Batch_ImportCCDA.url;
                                    params["IsFile"] = false;
                                } else if (Batch_ImportCCDA.XMLContent && Batch_ImportCCDA.XMLContent != "") {
                                    params["XMLContent"] = Batch_ImportCCDA.XMLContent;
                                    params["IsFile"] = true;
                                }
                                LoadBatchTab("PatientImportCCDA", false);
                            }
                            var istoggle = false;
                            if (Batch_ImportCCDA.PatientId + "" == localStorage.getItem("SelectedPatientId")) {
                                istoggle = true;
                            }
                            // Record MU3 Alert
                            MU_Alerts.UpdateMUAlertProfile("Reconciliation", 0, Batch_ImportCCDA.PatientId, true, ImportProviderId, [], istoggle);

                        }
                        else {
                            utility.DisplayMessages("Please select Component.", 3);
                        }
                    }
                    else {
                        utility.DisplayMessages("Please select patient.", 3);
                    }
                }
                else {
                    utility.DisplayMessages("Please save patient.", 3);
                }
            }
        }
        else {
            utility.DisplayMessages("Please select file.", 3);
        }
    },

    ImportCCDA2: function () {
        document.getElementById("IframXML").srcdoc = '';
        if (Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "") {
            var params = [];

            params["ParentCtrl"] = "batchTabImportCCDA";
            params["PatientId"] = parseInt(Batch_ImportCCDA.PatientId);
            params["PanelID"] = "pnlBatchPatientImportCCDA";
            params["ActionPanContainer"] = "actionPanBatchPatientImportCCDA";
            params["TabID"] = Batch_ImportCCDA.params.TabID;
            params["FromAdmin"] = 1;
            Batch_ImportCCDA.params["SelectedPatientId"] = Batch_ImportCCDA.PatientId;
            Batch_ImportCCDA.params["Medication"] = $("#pnlBatchImportCCDA #Medication").prop('checked');
            Batch_ImportCCDA.params["Allergies"] = $("#pnlBatchImportCCDA #Allergies").prop('checked');
            Batch_ImportCCDA.params["Problems"] = $("#pnlBatchImportCCDA #Problems").prop('checked');
            params["XMLContent"] = Batch_ImportCCDA.url;
            var selectedTypeCCDA = $("#pnlBatchImportCCDA #ddlImportTypeCCDA").val();
            var data = "XMLContent=" + Batch_ImportCCDA.url + "&FileType=" + selectedTypeCCDA;
            MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "XML_TO_Import").done(function (response) {
                if (response.status) {
                    utility.DisplayMessages(response.Message, 3);
                    Batch_ImportCCDA.resetValues();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            utility.DisplayMessages("Please select file.", 3);
        }
    },

    ImportCCDA: function () {

        var x = document.getElementById("Upload_Import_file");
        var txt = "";
        if ('files' in x || (Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "")) {
            if (x.files.length == 0 && (Patient_MessageCompose == null || Patient_MessageCompose.xmlByteData == null || Patient_MessageCompose.xmlByteData == "")) {
                utility.DisplayMessages("Please select file.", 3);
            } else {
                //if (parseInt(Batch_ImportCCDA.PatientId) > 0) {
                //if ($("#" + Batch_ImportCCDA.params["PanelID"] + " #tblPatient #rbtnSelect").is(':checked')) {
                //if (Batch_ImportCCDA.CheckComponentsImport(true)) {
                var params = [];
                if (Batch_ImportCCDA.params["ParentCtrl"] == "Patient_Search") {
                    params["ProviderId"] = "-1";
                    params["FromAdmin"] = "0";
                    params["RefCtrl"] = "txtSearchProvider";
                    params["RefCtrlHidden"] = "hfSearchProvider";
                    params["RefCtrlLabel"] = "lblSearchProvider";
                    params["RefCtrlLink"] = "lnkSearchProviderEdit";
                    params["ParentCtrl"] = "Batch_ImportCCDA";
                    params["ParentCtrlPanelID"] = "pnlBatchImportCCDA";
                    params["PatientId"] = parseInt(Batch_ImportCCDA.PatientId);
                    params["OpenFromPopup"] = "1"; Batch_ImportCCDA.params["SelectedPatientId"] = Batch_ImportCCDA.PatientId;//"87560";//"87560";
                    Batch_ImportCCDA.params["Medication"] = $("#pnlBatchImportCCDA #Medication").prop('checked');
                    Batch_ImportCCDA.params["Allergies"] = $("#pnlBatchImportCCDA #Allergies").prop('checked');
                    Batch_ImportCCDA.params["Problems"] = $("#pnlBatchImportCCDA #Problems").prop('checked');
                    params["XMLContent"] = Batch_ImportCCDA.XMLContent;

                }
                else {
                    params["ParentCtrl"] = "batchTabImportCCDA";
                    params["PatientId"] = parseInt(Batch_ImportCCDA.PatientId);
                    params["PanelID"] = "pnlBatchPatientImportCCDA";
                    params["ActionPanContainer"] = "actionPanBatchPatientImportCCDA";
                    params["TabID"] = Batch_ImportCCDA.params.TabID;
                    params["FromAdmin"] = 1;
                    Batch_ImportCCDA.params["SelectedPatientId"] = Batch_ImportCCDA.PatientId;//"87560";
                    Batch_ImportCCDA.params["Medication"] = $("#pnlBatchImportCCDA #Medication").prop('checked');
                    Batch_ImportCCDA.params["Allergies"] = $("#pnlBatchImportCCDA #Allergies").prop('checked');
                    Batch_ImportCCDA.params["Problems"] = $("#pnlBatchImportCCDA #Problems").prop('checked');
                    params["XMLContent"] = Batch_ImportCCDA.XMLContent;
                    if ((Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "") || Batch_ImportCCDA.url != "") {
                        params["XMLContent"] = Batch_ImportCCDA.url;
                        params["IsFile"] = false;
                    } else if (Batch_ImportCCDA.XMLContent && Batch_ImportCCDA.XMLContent != "") {
                        params["XMLContent"] = Batch_ImportCCDA.XMLContent;
                        params["IsFile"] = true;
                    }
                    var selectedTypeCCDA = $("#pnlBatchImportCCDA #ddlImportTypeCCDA").val();
                    params["ProviderId"] = $("#pnlBatchImportCCDA #ddlprovider").val();
                    params["FacilityId"] = $("#pnlBatchImportCCDA #ddlfacility").val();
                    data = "XMLContent=" + params["XMLContent"] + "&FileType=" + selectedTypeCCDA + "&IsFile=" + params["IsFile"] + "&ProviderId=" + params["ProviderId"] + "&FacilityId=" + params["FacilityId"] + "&FileName=" + Batch_ImportCCDA.FileName;
                    MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "XML_TO_Import").done(function (response) {
                        if (response.status) {
                            utility.DisplayMessages(response.Message, 3);
                            if (Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "") {
                                Patient_MessageCompose.xmlByteData == "";
                            }
                            Batch_ImportCCDA.url = "";
                            Batch_ImportCCDA.resetValues();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                            //Batch_ImportCCDA.resetValues();
                        }
                    });
                }
                //}
                //else {
                //    utility.DisplayMessages("Please select Component.", 3);
                //}
                //}
                //else {
                //    utility.DisplayMessages("Please select patient.", 3);
                //}
                //}
                //else {
                //    utility.DisplayMessages("Please save patient.", 3);
                //}
            }
        }
        else {
            utility.DisplayMessages("Please select file.", 3);
        }
    },

    SelectImportFileOfInbox: function () {
        document.getElementById("IframXML").srcdoc = '';
        var selectedTypeCCDA = $("#pnlBatchImportCCDA #ddlImportTypeCCDA").val();
        if (Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "" && selectedTypeCCDA != "" && selectedTypeCCDA != "0") {
            var formdata = new Object();
            formdata.XMLContent = Batch_ImportCCDA.EncodeSpecialCharXML(Patient_MessageCompose.xmlByteData);
            formdata.IsFile = false;

            formdata.FileType = selectedTypeCCDA;
            formdata.DocfileType = "xml";
            formdata.DocfileName = "test";
            MDVisionService.APIServiceComplex(formdata, "ClinicalSummary", "ReconcileCCDABase64").done(function (response) {

                response = JSON.parse(response);
                if (response.status) {
                    var myJson = JSON.parse(response.PatientInfo);
                    Batch_ImportCCDA.XMLParse_AllergiesLoad_JSON = JSON.parse(response.AllergiesInfo);
                    Batch_ImportCCDA.XMLParse_ProblemLoad_JSON = JSON.parse(response.ProblemsInfo);
                    Batch_ImportCCDA.XMLParse_MedicationLoad_JSON = JSON.parse(response.MedicationsInfo);
                    document.getElementById("IframXML").src = response.dataHtml;
                    Batch_ImportCCDA.DisplayReadableData();
                    if (response.url && response.url != "") {
                        Batch_ImportCCDA.url = response.url;
                    }
                    $("#IframXML").removeAttr('srcdoc');


                    Batch_ImportCCDA.CCDASectionCheckBoxList();

                    if (myJson && myJson.length > 0) {
                        myJson = myJson[0];
                        myJson["MaritalStatus"] = myJson["MaritialStatus"];
                        myJson["Zip"] = myJson["ZIPCode"];
                        myJson["Ethnicity"] = myJson["EthnicityId"];
                        myJson["Race"] = myJson["RaceId"];
                        myJson["PrefLanguage"] = myJson["PrefLanguageId"];
                        myJson["Sex"] = myJson["Gender"];
                        myJson["strEthnicityIds"] = myJson["strEthnicityIds"];
                        myJson["strRaceIds"] = myJson["strRaceIds"];
                        myJson["strRaceIds"] = myJson["strRaceIds"];
                        myJson["ConfidentialityCode"] = myJson["ConfidentialityCode"];
                        myJson["MI"] = myJson["MI"];
                        myJson["MaritialStatus"] = myJson["MaritialStatus"];
                        myJson["HomePhoneNo"] = myJson["HomePhoneNo"];
                        myJson["WorkPhoneNo"] = myJson["WorkPhoneNo"];
                    }

                    Batch_ImportCCDA.PatientInfo = myJson;
                    var data = "PatientInfo=" + JSON.stringify(myJson);

                    return MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "Check_Patient").done(function (response) {
                        if (response.status) {
                            if (response.PatientId > 0) {
                                Batch_ImportCCDA.PatientId = response.PatientId;
                                // Different Patient Is selected in Banner which cause discrepancy EMR-7224
                                //try {
                                //    $(" #mainForm  li#CDSAlert").show();
                                //    $(" #mainForm #hfTriggerLocation").val('CCDA');
                                //    ClinicalCDSDetail.showCDSAlert('', Batch_ImportCCDA.PatientId);
                                //}
                                //catch (ex) {
                                //    console.log(ex);
                                //}

                                $("#pnlBatchImportCCDA #btnCreatePatient").prop("disabled", true);

                                var item = myJson;
                                var $row = $('<tr/>');
                                try {
                                    var date = new Date(item.DOB);
                                    item.DOB = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                                } catch (ex) {
                                    console.log(ex);
                                }
                                $row.attr("PatientId", response.PatientId);
                                $row.append('<td><div class="pb-xs"><div class="radio-custom"><input type="radio" id="rbtnSelect" name="aa"><label class="control-label"></label></div></div></td><td>' + item.LastName + ', ' + item.FirstName + '</td><td>' + item.DOB + '</td><td>' + item.Gender + '</td>');
                                $("#pnlBatchImportCCDA #tblPatient tbody").html($row);
                                $("#pnlBatchImportCCDA #btnNextReconcilation").removeClass("hide");
                                $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", false);
                                $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", true);
                                $("#pnlBatchImportCCDA #btnCreatePatient").prop("disabled", true);
                                $("#pnlBatchImportCCDA .liCCDAImport").addClass("hide");
                                $("#pnlBatchImportCCDA #btnCreatePatient").addClass("hide");
                                $("#pnlBatchImportCCDA #Medication").prop("checked", true);
                                $("#pnlBatchImportCCDA #Medication").prop("disabled", false);
                                $("#pnlBatchImportCCDA #Allergies").prop("checked", true);
                                $("#pnlBatchImportCCDA #Allergies").prop("disabled", false);
                                $("#pnlBatchImportCCDA #Problems").prop("checked", true);
                                $("#pnlBatchImportCCDA #Problems").prop("disabled", false);
                            }
                            else {
                                Batch_ImportCCDA.PatientId = 0;
                                $("#pnlBatchImportCCDA #btnCreatePatient").addClass("hide");
                                $("#pnlBatchImportCCDA #btnCreatePatient").prop("disabled", true);
                                $("#pnlBatchImportCCDA #tblPatient tbody").html('<tr><td colspan="4">No Available Record</td></tr>');
                                $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", true);
                                $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", true);
                                $("#pnlBatchImportCCDA .liCCDAImport").addClass("hide");
                                $("#pnlBatchImportCCDA #Medication").prop("checked", true);
                                $("#pnlBatchImportCCDA #Medication").prop("disabled", true);
                                $("#pnlBatchImportCCDA #Allergies").prop("checked", true);
                                $("#pnlBatchImportCCDA #Allergies").prop("disabled", true);
                                $("#pnlBatchImportCCDA #Problems").prop("checked", true);
                                $("#pnlBatchImportCCDA #Problems").prop("disabled", true);
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                    Batch_ImportCCDA.DisplayReadableData();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ImportFileSelect: function (file) {
        Batch_ImportCCDA.XMLParse_AllergiesLoad_JSON = [];
        Batch_ImportCCDA.XMLParse_ProblemLoad_JSON = [];
        Batch_ImportCCDA.XMLParse_MedicationLoad_JSON = [];
        $("#Batch_ImportCCDA #IframXML").addClass("hide");
        if (file.files.length > 0) {
            var f = file.files[0];
            var fileExt = f.name.split('.').pop();

            if (fileExt.toLowerCase() != 'xml') {
                Batch_ImportCCDA.XMLContent = "";
                utility.DisplayMessages("Please select xml file.", 3);
                Batch_ImportCCDA.resetValues();
                return;
            }

            if (f) {

                var r = new FileReader();
                r.onload = function (e) {
                    Batch_ImportCCDA.FileName = f.name;
                    var contents = e.target.result;
                    contents = contents.replace(/&amp;/g, 'and');
                    var selectedTypeCCDA = $("#pnlBatchImportCCDA #ddlImportTypeCCDA").val();
                    var xmlcontent = Batch_ImportCCDA.EncodeSpecialCharXML(contents);
                    data = "XMLContent=" + xmlcontent + "&FileType=" + selectedTypeCCDA + "&IsFile=true";
                    Batch_ImportCCDA.XMLContent = xmlcontent;

                    //contents
                    MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "XML_TO_HTML").done(function (response) {

                        if (response.status) {
                            if (Patient_MessageCompose) {
                                Patient_MessageCompose.xmlByteData = "";
                                Batch_ImportCCDA.url = "";
                            }
                            document.getElementById("IframXML").src = response.data;

                            Batch_ImportCCDA.CCDASectionCheckBoxList();


                            $("#IframXML").removeAttr('srcdoc');
                            //$($('#IframXML').contents()[0].body).find('h3 a').each(function () {
                            //    $(this).attr("id",$(this).attr("name"));
                            //});

                            if ($("#" + Batch_ImportCCDA.params["PanelID"] + " #ulCheckBoxes").hasClass("disableAll")) {
                                $("#" + Batch_ImportCCDA.params["PanelID"] + " #ulCheckBoxes").removeClass("disableAll");
                            }
                            $("#" + Batch_ImportCCDA.params["PanelID"] + " #totalFiles").text('1 file(s) selected');

                            var myJson = JSON.parse(response.PatientInfo);
                            Batch_ImportCCDA.XMLParse_AllergiesLoad_JSON = JSON.parse(response.AllergiesInfo);
                            Batch_ImportCCDA.XMLParse_ProblemLoad_JSON = JSON.parse(response.ProblemsInfo);
                            Batch_ImportCCDA.XMLParse_MedicationLoad_JSON = JSON.parse(response.MedicationsInfo);

                            if (myJson && myJson.length > 0) {
                                myJson = myJson[0];
                                myJson["MaritalStatus"] = myJson["MaritialStatus"];
                                myJson["Zip"] = myJson["ZIPCode"];
                                myJson["Ethnicity"] = myJson["EthnicityId"];
                                myJson["Race"] = myJson["RaceId"];
                                myJson["PrefLanguage"] = myJson["PrefLanguageId"];
                                myJson["Sex"] = myJson["Gender"];
                                myJson["strEthnicityIds"] = myJson["strEthnicityIds"];
                                myJson["strRaceIds"] = myJson["strRaceIds"];
                                myJson["strRaceIds"] = myJson["strRaceIds"];
                                myJson["ConfidentialityCode"] = myJson["ConfidentialityCode"];
                                myJson["MI"] = myJson["MI"];
                                myJson["MaritialStatus"] = myJson["MaritialStatus"];
                                myJson["HomePhoneNo"] = myJson["HomePhoneNo"];
                                myJson["WorkPhoneNo"] = myJson["WorkPhoneNo"];
                            }

                            Batch_ImportCCDA.PatientInfo = myJson;
                            var data = "PatientInfo=" + JSON.stringify(myJson);

                            return MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "Check_Patient").done(function (response) {
                                if (response.status) {
                                    if (response.PatientId > 0) {
                                        Batch_ImportCCDA.PatientId = response.PatientId;
                                        // Different Patient Is selected in Banner which cause discrepancy EMR-7224
                                        //try {
                                        //    $(" #mainForm  li#CDSAlert").show();
                                        //    $(" #mainForm #hfTriggerLocation").val('CCDA');
                                        //    ClinicalCDSDetail.showCDSAlert('', Batch_ImportCCDA.PatientId);
                                        //}
                                        //catch (ex) {
                                        //    console.log(ex);
                                        //}

                                        $("#pnlBatchImportCCDA #btnCreatePatient").prop("disabled", true);

                                        var item = myJson;
                                        var $row = $('<tr/>');
                                        try {
                                            var date = new Date(item.DOB);
                                            item.DOB = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                                        } catch (ex) {
                                            console.log(ex);
                                        }
                                        $row.attr("PatientId", response.PatientId);
                                        $row.append('<td><div class="pb-xs"><div class="radio-custom"><input type="radio" id="rbtnSelect" name="aa"><label class="control-label"></label></div></div></td><td>' + item.LastName + ', ' + item.FirstName + '</td><td>' + item.DOB + '</td><td>' + item.Gender + '</td>');
                                        $("#pnlBatchImportCCDA #tblPatient tbody").html($row);
                                        $("#pnlBatchImportCCDA #btnNextReconcilation").removeClass("hide");
                                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", false);
                                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", true);
                                        $("#pnlBatchImportCCDA #btnCreatePatient").prop("disabled", true);
                                        //$("#pnlBatchImportCCDA #btnImportCCDA").addClass("hide");
                                        $("#pnlBatchImportCCDA .liCCDAImport").addClass("hide");
                                        $("#pnlBatchImportCCDA #btnCreatePatient").addClass("hide");
                                        $("#pnlBatchImportCCDA #Medication").prop("checked", true);
                                        $("#pnlBatchImportCCDA #Medication").prop("disabled", false);
                                        $("#pnlBatchImportCCDA #Allergies").prop("checked", true);
                                        $("#pnlBatchImportCCDA #Allergies").prop("disabled", false);
                                        $("#pnlBatchImportCCDA #Problems").prop("checked", true);
                                        $("#pnlBatchImportCCDA #Problems").prop("disabled", false);
                                    }
                                    else {
                                        Batch_ImportCCDA.PatientId = 0;
                                        $("#pnlBatchImportCCDA #btnCreatePatient").addClass("hide");
                                        $("#pnlBatchImportCCDA #btnCreatePatient").prop("disabled", true);
                                        $("#pnlBatchImportCCDA #tblPatient tbody").html('<tr><td colspan="4">No Available Record</td></tr>');
                                        //$("#pnlBatchImportCCDA #btnNextReconcilation").addClass("hide");
                                        $("#pnlBatchImportCCDA #btnNextReconcilation").prop("disabled", true);
                                        $("#pnlBatchImportCCDA #btnImportCCDA").prop("disabled", true);
                                        ////$("#pnlBatchImportCCDA #btnImportCCDA").removeClass("hide");
                                        $("#pnlBatchImportCCDA .liCCDAImport").addClass("hide");
                                        $("#pnlBatchImportCCDA #Medication").prop("checked", true);
                                        $("#pnlBatchImportCCDA #Medication").prop("disabled", true);
                                        $("#pnlBatchImportCCDA #Allergies").prop("checked", true);
                                        $("#pnlBatchImportCCDA #Allergies").prop("disabled", true);
                                        $("#pnlBatchImportCCDA #Problems").prop("checked", true);
                                        $("#pnlBatchImportCCDA #Problems").prop("disabled", true);
                                    }
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                        else {
                            if (response.Message == "the selected file is not valid") {
                                utility.DisplayMessages("The selected file is not valid", 3);
                            } else if (response.Message == "privacy protected") {
                                utility.kendoAlert("Alert", "This file cannot be imported as it is privacy protected.", "OK");
                            } else if ((response.Message.toLowerCase().indexOf("unknown document") >= 0)) {
                                if (response.Message.toLowerCase().indexOf("valid CCD file") >= 0)
                                    utility.DisplayMessages("Unknown document type. The selected file is not a valid CCD file.", 3);
                                else if (response.Message.toLowerCase().indexOf("valid ccd file") >= 0)
                                    utility.DisplayMessages("Unknown document type. The selected file is not a valid CCD file.", 3);
                                else if (response.Message.toLowerCase().indexOf("valid ccd v3 file") >= 0)
                                    utility.DisplayMessages("Unknown document type. The selected file is not a valid CCD V3 file.", 3);
                                else if (response.Message.toLowerCase().indexOf("valid referral note") >= 0)
                                    utility.DisplayMessages("Unknown document type. The selected file is not a valid referral note file.", 3);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                            Batch_ImportCCDA.resetValues();
                        }
                    });
                }
                r.readAsText(f);
            } else {
                Batch_ImportCCDA.XMLContent = "";
                utility.DisplayMessages("Failed to load file.", 3);
                Batch_ImportCCDA.resetValues();
            }
        }
    },

    CreatePatient: function () {
        Batch_ImportCCDA.SaveDemographic(Batch_ImportCCDA.PatientInfo, true);
    },

    SaveDemographic: function (myJSON) {
        var objData = myJSON;
        var data = "PatientInfo=" + JSON.stringify(objData);
        MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "CREATE_Patient").done(function (response) {
            response = JSON.parse(response);
            if (response.status) {
                utility.DisplayMessages(response.message, 1);
                Batch_ImportCCDA.PatientId = response.PatientId;
                var objData = new Object();
                objData["PatientId"] = Batch_ImportCCDA.PatientId;
                objData["FileName"] = Batch_ImportCCDA.FileName;
                var data = JSON.stringify(objData);
                var data = "PatientIdAndFileName=" + JSON.stringify(objData);
                MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "Create_DBAuditNewPatient").done(function (response) {
                    if (response.status) {
                        $("#pnlBatchImportCCDA #btnCreatePatient").prop("disabled", true);
                        var item = Batch_ImportCCDA.PatientInfo;
                        var $row = $('<tr/>');
                        $row.attr("PatientId", Batch_ImportCCDA.PatientId);
                        try {
                            var date = new Date(item.DOB);
                            item.DOB = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                        } catch (ex) {
                            console.log(ex);
                        }
                        $row.append('<td>' + item.LastName + ' ' + item.FirstName + '</td><td>' + item.DOB + '</td><td>' + item.Sex + '</td><td><div class="pb-xs"><div class="radio-custom"><input type="radio" id="rbtnSelect" name="aa"><label class="control-label"></label></div></div></td>');
                        $("#pnlBatchImportCCDA #tblPatient tbody").html($row);
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    SearchPatient: function (PatientId) {

        var objData = new Object();
        objData["PatientID"] = PatientId;
        objData["PageNo"] = 1;
        objData["CommandType"] = "Search";
        objData["rpp"] = 15;
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Patient", "Patient");
    },

    ImportPrivacyFileSelect: function (file) {
        if (file.files.length > 0) {
            Batch_ImportCCDA.PrivacySegmentedDocument = file.files[0];
            var fileExt = Batch_ImportCCDA.PrivacySegmentedDocument.name.split('.').pop();
            $("#" + Batch_ImportCCDA.params["PanelID"] + " #totalPrivacyFiles").text('1 file(s) selected');
            $("#" + Batch_ImportCCDA.params.PanelID + " #btnImportPrivacySegmentedDocument").prop("disabled", false);
            if (fileExt != 'xml') {
                utility.DisplayMessages("Please select xml file.", 3);
                Batch_ImportCCDA.resetPrivacySegmentedDocumentValues();
                return;
            }
        }
    },

    ImportPrivacySegmentedDocument: function () {
        if (Batch_ImportCCDA.PrivacySegmentedDocument) {
            var r = new FileReader();
            r.onload = function (e) {
                var contents = e.target.result;
                contents = contents.replace(/&amp;/g, 'and');
                data = "XMLContent=" + contents + "&FileName=" + Batch_ImportCCDA.PrivacySegmentedDocument.name;

                //contents
                MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "IMPORT_PRIVACY_FILE").done(function (response) {
                    if (response.status) {
                        utility.DisplayMessages(response.message, 1);
                        Batch_ImportCCDA.PrivacySegmentedDocumentSearch();
                    }
                    else {
                        utility.kendoAlert("Alert", response.message, "OK");
                    }
                    Batch_ImportCCDA.resetPrivacySegmentedDocumentValues();
                });
            }
            r.readAsText(Batch_ImportCCDA.PrivacySegmentedDocument);
        } else {
            utility.DisplayMessages("Failed to load file.", 3);
            Batch_ImportCCDA.resetPrivacySegmentedDocumentValues();
        }
    },

    PrivacySegmentedDocumentSearch: function (Id, PageNumber, ResultPerPage) {
        Batch_ImportCCDA.PrivacySegmentedDocument_DBCall(Id, PageNumber, ResultPerPage).done(function (response) {
            if (response.status) {
                if ($("#" + Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument").css("display") == "none") {
                    $("#" + Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument").show();
                }
                Batch_ImportCCDA.PrivacySegmentedDocumentGridLoad(response);
                var TableControl = Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument";

                var PagingPanelControlID = Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument_Paging";
                var ClassControlName = "Batch_ImportCCDA";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(
                    CreatePagination(response.PrivacySegmentedDocumentCount, PageNumber, ResultPerPage, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (Id, PageNumber, ResultPerPage) {
                        Batch_ImportCCDA.PrivacySegmentedDocumentSearch(Id, PageNumber, ResultPerPage);
                    }), 10);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    PrivacySegmentedDocumentGridLoad: function (response) {
        $("#" + Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument").dataTable().fnDestroy();
        $("#" + Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument tbody").find("tr").remove();
        if (response.PrivacySegmentedDocumentCount > 0) {
            var PatientInformationSubmissionJSONData = JSON.parse(response.PrivacySegmentedDocumentLoad_JSON);
            PrivacySegmentedDocumentPaths = [];
            $.each(PatientInformationSubmissionJSONData, function (i, item) {
                var ConfidentialityCode = item.ConfidentialityCode == 'R' ? 'Restricted' : 'Not-Restricted';
                var $row = $('<tr/>');
                $row.attr("id", "gvPatientInformationSubmission_row" + item.Id);
                $row.attr("Id", item.Id);
                $row.append('<td style="display:none;">' + item.Id + '</td><td>' + item.LastName + ', ' + item.FirstName + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOB) + '</td><td>' + ConfidentialityCode + '</td><td><a href="#" class="pb-xs" onclick="Batch_ImportCCDA.PrivacySegmentedDocumentSelect(' + item.Id + ');">Display CCDA</a></td><td>' + item.CreatedBy + ' ' + item.CreatedOn + '</td>');

                $("#" + Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument tbody").last().append($row);
                PrivacySegmentedDocumentPaths.push({
                    Id: item.Id, FilePath: item.FilePath, ConfidentialityCode: item.ConfidentialityCode
                });
            });
        }
        else {

            $("#" + Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument").DataTable({
                "language": {
                    "emptyTable": "No Record Available."
                }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{
                    "bSortable": false, "bPaginate": false, "aTargets": [1]
                }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument"))
            ;
        else
            $("#" + Batch_ImportCCDA.params.PanelID + " #dgvPrivacySegmentedDocument").DataTable({
                "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{
                    "bSortable": false, "bPaginate": false, "aTargets": [1]
                }]
            }); // to remove records per page dropdown


    },

    PrivacySegmentedDocument_DBCall: function (Id, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var data = "Id=" + Id + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "SEARCH_PRIVACY_SEGMENTED_DOCUMENT");
    },

    PrivacySegmentedDocumentSelect: function (key) {
        var Path = '', ConfidentialityCode = '';
        for (var i = 0; i < PrivacySegmentedDocumentPaths.length; i++) {
            if (PrivacySegmentedDocumentPaths[i].Id == key) {
                Path = PrivacySegmentedDocumentPaths[i].FilePath;
                ConfidentialityCode = PrivacySegmentedDocumentPaths[i].ConfidentialityCode;
            }
        }
        var data = "Path=" + Path + "&ConfidentialityCode=" + ConfidentialityCode;
        MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "PRIVACY_SEGMENTED_DOCUMENT_HTML").done(function (response) {
            if (response.status) {
                $("#IframeXMLPrivacySegmentedDocument").removeAttr('srcdoc');
                document.getElementById("IframeXMLPrivacySegmentedDocument").src = response.data;
            }
            else {
                utility.kendoAlert("Alert", response.message, "OK");
                Batch_ImportCCDA.resetPrivacySegmentedDocumentValues();
            }
        });
    },

    UnLoad: function () {
        if (Batch_ImportCCDA.params != null && Batch_ImportCCDA.params.ParentCtrl) {
            UnloadActionPan(Batch_ImportCCDA.params.ParentCtrl);
            // UnloadActionPan(Batch_ImportCCDA.params["ParentCtrl"], "Batch_ImportCCDA", null, Batch_ImportCCDA.params["ParentCtrlPanelID"]);
            Batch_ImportCCDA.params = null;
        }
        else {
            UnloadActionPan();
            var CurrentMasterTab = GetCurrentMasterTab();
            if (CurrentMasterTab.TabID == "mstrTabPatient" && PatientArray.length <= 0) {
                ClosePatientNew();
                $('.modal-backdrop.fade.in').remove();
            }
        }
    },

    UnLoadTab: function () {
        RemoveAdminTab();
    },

    SelectTab: function (Type) {
        Batch_ImportCCDA.Type = Type;
        //$(Patient_Referrals.params.PanelID + " #headingTitle").html("Search " + Type + " Referrals");
        if (Batch_ImportCCDA.Type == "ImportCCDA") {
            //Batch_ImportCCDA.ValidateIncomingTab();
            if (!$("#" + Batch_ImportCCDA.params.PanelID + " #ImportCCDA").hasClass("active")) {
                $("#" + Batch_ImportCCDA.params.PanelID + " #ImportCCDA").addClass("active");
            }
            $("#" + Batch_ImportCCDA.params.PanelID + " #ImportCypress").removeClass("active");
            $("#" + Batch_ImportCCDA.params.PanelID + " #ImportPrivacySegmentedDocument").removeClass("active");
        }
        else if (Batch_ImportCCDA.Type == "ImportPrivacySegmentedDocument") {
            $("#" + Batch_ImportCCDA.params.PanelID + " #ImportCypress").removeClass("active");
            $("#" + Batch_ImportCCDA.params.PanelID + " #ImportCCDA").removeClass("active");
            $("#" + Batch_ImportCCDA.params.PanelID + " #ImportPrivacySegmentedDocument").addClass("active");
            Batch_ImportCCDA.PrivacySegmentedDocumentSearch();
        }
        else {
            //Batch_ImportCCDA.ValidateOutcomingTab();
            $("#" + Batch_ImportCCDA.params.PanelID + " #ImportCCDA").removeClass("active");
            $("#" + Batch_ImportCCDA.params.PanelID + " #ImportPrivacySegmentedDocument").removeClass("active");
            if (!$("#" + Batch_ImportCCDA.params.PanelID + " #ImportCypress").hasClass("active")) {
                $("#" + Batch_ImportCCDA.params.PanelID + " #ImportCypress").addClass("active");
            }
        }
        //Patient_Referrals.ReferralSearch();
        return true;
    },

    //ImportCypressFileSelect: function (file) {
    //    if (file.files.length > 0) {
    //        var f = file.files[0];
    //        var fileExt = f.name.split('.').pop();

    //        if (fileExt != 'xml') {
    //            Batch_ImportCCDA.XMLContent = "";
    //            utility.DisplayMessages("Please select xml file.", 3);
    //            Batch_ImportCCDA.resetValues();
    //            return;
    //        }

    //        if (f) {

    //            var r = new FileReader();
    //            r.onload = function (e) {
    //                Batch_ImportCCDA.FileName = f.name;
    //                var contents = e.target.result;
    //                contents = contents.replace(/&amp;/g, 'and');
    //                //data = "XMLContent=" + contents + "&FileName=" + f.name;
    //                data = "XMLContent=" + contents;
    //                Batch_ImportCCDA.XMLContent = contents;

    //                MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "CYPRESS_XML_TO_HTML").done(function (response) {
    //                    if (response.status) {
    //                        document.getElementById("IframXMLHTML").src = response.data;
    //                        $("#IframXMLHTML").removeAttr('srcdoc');
    //                        $("#" + Batch_ImportCCDA.params["PanelID"] + " #totalFiles").text('1 file(s) selected, ' + f.name);
    //                        utility.DisplayMessages("Successfully Imported", 1);
    //                    }
    //                    else {
    //                        utility.DisplayMessages(response.Message, 3);
    //                        //Batch_ImportCCDA.resetValues();
    //                    }
    //                });
    //            }
    //            r.readAsText(f);
    //        } else {
    //            Batch_ImportCCDA.XMLContent = "";
    //            utility.DisplayMessages("Failed to load file.", 3);
    //            Batch_ImportCCDA.resetValues();
    //        }
    //    }
    //},

    ImportCypressFileSelect: function (file) {
        var contents = new Array();
        if (file.files.length > 0) {
            var f = file.files;
            $(f).each(function (i, item) {

                var fileExt = item.name.split('.').pop();

                if (fileExt.toLowerCase() != 'xml') {
                    Batch_ImportCCDA.XMLContent = "";
                    utility.DisplayMessages("Please select xml file.", 3);
                    Batch_ImportCCDA.resetValues();
                    return;
                }

                if (item) {

                    var r = new FileReader();
                    r.onload = function (e) {
                        Batch_ImportCCDA.FileName = item.name;
                        contents.push(e.target.result);
                        if (file.files.length == contents.length) {

                            var objData = new Object();
                            objData["listXML"] = contents;

                            MDVisionService.defaultService(objData, "Batch_ClinicalImportCCDA", "CYPRESS_XML_TO_HTML").done(function (response) {
                                if (response.status) {
                                    document.getElementById("IframXMLHTML").src = response.data;
                                    $("#IframXMLHTML").removeAttr('srcdoc');
                                    $("#" + Batch_ImportCCDA.params["PanelID"] + " #totalFiles").text('1 file(s) selected, ' + item.name);
                                    utility.DisplayMessages("Successfully Imported", 1);
                                }
                                else {

                                    if (response.Message.indexOf("input string was not in a correct format.couldn't store <> in") > -1) {

                                        response.Message = "Please select a default Provider/facility before importing data.";
                                    }
                                    utility.DisplayMessages(response.Message, 3);
                                    //Batch_ImportCCDA.resetValues();
                                }
                            });
                        }
                    }
                    r.readAsText(item);
                }
            });


        } else {
            Batch_ImportCCDA.XMLContent = "";
            utility.DisplayMessages("Failed to load file.", 3);
            Batch_ImportCCDA.resetValues();
        }

        // }
    },

    ImportCypressData: function () {

        utility.DisplayMessages("Baby, Easy", 3);
    },
    b64toBlob: function (b64Data, contentType, sliceSize) {
        contentType = contentType || '';
        sliceSize = sliceSize || 512;

        var byteCharacters = atob(b64Data);
        var byteArrays = [];

        for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            var slice = byteCharacters.slice(offset, offset + sliceSize);

            var byteNumbers = new Array(slice.length);
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }

            var byteArray = new Uint8Array(byteNumbers);

            byteArrays.push(byteArray);
        }

        var blob = new Blob(byteArrays, {
            type: contentType
        });
        return blob;
    },
    CCDASectionCheckBoxList: function () {

        Batch_ImportCCDA.ParsedHTMLContentBody = null;
        Batch_ImportCCDA.PreferenceArrayUnsaved = [];
        $(".toggledCcdaSections").remove();
        Batch_ImportCCDA.WaitForHTMLLoadInIframeCounter = 1;
        Batch_ImportCCDA.HTMLContentLoadedInInframe = false;

        if (!globalAppdata["ccdaPreference"]) {
            globalAppdata["ccdaPreference"] = JSON.parse(localStorage.getItem("ccdaPreference"));
        }

        setTimeout(function () { Batch_ImportCCDA.WaitForHTMLLoadInIframe() }, 300);
    },
    WaitForHTMLLoadInIframe: function () {

        console.log(Batch_ImportCCDA.WaitForHTMLLoadInIframeCounter);
        console.log("outer: " + Batch_ImportCCDA.HTMLContentLoadedInInframe);

        if (Batch_ImportCCDA.HTMLContentLoadedInInframe && $("#IframXML").contents().find('body').find('h3').find('a') && $("#IframXML").contents().find('body').find('h3').find('a').length > 0) {

            console.log("inner: " + Batch_ImportCCDA.HTMLContentLoadedInInframe);

            var liHtml = '<div class="checkbox-custom toggledCcdaSections">';

            liHtml += '<input type="checkbox" name="allcheck" id="selectAllSections" onclick="Batch_ImportCCDA.toggleAllCCDASection(this)">';
            liHtml += '<label class="control-label" for="allcheck">All</label>';
            liHtml += '</div>';

            $("ul#ccdaImportSectionList").append(liHtml);

            if ($("#ddlImportTypeCCDA").val() != "C32" && $("#ddlImportTypeCCDA").val() != "CCR") {
                if ($("#IframXML").contents().find('body').find('table:nth-child(2) tbody tr:nth-child(5) td') && $("#IframXML").contents().find('body').find('table:nth-child(2) tbody tr:nth-child(5) td')[1].innerText.length <= 0) {
                    $("#IframXML").contents().find('body').find('table:nth-child(2) tbody tr:nth-child(5) td')[1].innerText = "Unknown"
                }
                if ($("#IframXML").contents().find('body').find('table:nth-child(2) tbody tr:nth-child(6) td') && $("#IframXML").contents().find('body').find('table:nth-child(2) tbody tr:nth-child(6) td')[1].innerText.length <= 0) {
                    $("#IframXML").contents().find('body').find('table:nth-child(2) tbody tr:nth-child(6) td')[1].innerText = "Unknown"
                }
            }


            Batch_ImportCCDA.ParsedHTMLContentBody = $("#IframXML").contents().find('body').clone();

            $.each((Batch_ImportCCDA.ParsedHTMLContentBody).find('h3').find('a'), function (index, item) {
                Batch_ImportCCDA.HideCCDASection(item.name, "");

                var liHtml = '<div class="checkbox-custom toggledCcdaSections">';
                liHtml += '<input type="checkbox" data-section-name="' + item.innerText.toLowerCase() + '" value="' + item.name + '" id="' + item.name + '" name="' + item.name + '" onclick="Batch_ImportCCDA.toggleCCDASection(this)">';
                liHtml += '<label class="control-label" for="' + item.name + '">' + item.innerText + '</label>';
                liHtml += '</div>';

                $("ul#ccdaImportSectionList").append(liHtml);
            });

            if (globalAppdata["ccdaPreference"]) {
                $.each(globalAppdata["ccdaPreference"], function (i, pname) {
                    pname = pname.toLowerCase();
                    var input = $('input[data-section-name="' + pname + '"]');
                    Batch_ImportCCDA.PreferenceArrayUnsaved.push(pname);
                    if (input.length == 1) {
                        input.prop("checked", true);
                        Batch_ImportCCDA.ShowCCDASection(input.val(), pname);
                    }
                });
            }
        } else {
            if (Batch_ImportCCDA.WaitForHTMLLoadInIframeCounter > 30) {
                return;
            }
            if ($("#IframXML").contents().find('body').find('h3').find('a') && $("#IframXML").contents().find('body').find('h3').find('a').length > 0) {
                Batch_ImportCCDA.HTMLContentLoadedInInframe = true;
            }
            console.log("bottom: " + Batch_ImportCCDA.HTMLContentLoadedInInframe);
            Batch_ImportCCDA.WaitForHTMLLoadInIframeCounter++;
            setTimeout(function () { Batch_ImportCCDA.WaitForHTMLLoadInIframe() }, 300);
        }
    },
    toggleAllCCDASection: function (e) {
        $.each($("ul#ccdaImportSectionList").find('input[type=checkbox]:not("#selectAllSections")'), function (index, item) {

            var isAll = $("#selectAllSections")[0].checked;
            if (item.checked != isAll) {
                item.checked = isAll;
                if (isAll) {
                    Batch_ImportCCDA.ShowCCDASection(item.value, $(item).data('section-name'));
                } else {
                    Batch_ImportCCDA.HideCCDASection(item.value, $(item).data('section-name'));
                }
            }
        });
    },

    toggleCCDASection: function (event) {
        var name = $(event).data('section-name');
        if (event.checked) {

            Batch_ImportCCDA.ShowCCDASection(event.value, name);

        } else {
            Batch_ImportCCDA.HideCCDASection(event.value, name);
        }

    },

    ShowCCDASection: function (value, name) {
        var index = Batch_ImportCCDA.PreferenceArrayUnsaved.indexOf(name);
        if (index == -1) {
            Batch_ImportCCDA.PreferenceArrayUnsaved.push(name);
        }

        var toc = $("#IframXML").contents().find('a[name="toc"]').parent().next();
        var body = $("#IframXML").contents().find('body');

        var li = (Batch_ImportCCDA.ParsedHTMLContentBody).find('a[href="#' + value + '"]').parent().clone();
        toc.append(li);

        var nextAllTags = (Batch_ImportCCDA.ParsedHTMLContentBody).find('a[name="' + value + '"]').parent().nextAll().clone();

        var h3 = (Batch_ImportCCDA.ParsedHTMLContentBody).find('a[name="' + value + '"]').parent().clone();
        body.append(h3);

        for (var i = 0; i < nextAllTags.length; i++) {

            if ($(nextAllTags[i]).is('div')) {
                body.append(nextAllTags[i]);
            } else {
                break;
            }
        }

    },

    HideCCDASection: function (value, name) {

        var index = Batch_ImportCCDA.PreferenceArrayUnsaved.indexOf(name);
        if (index > -1) {
            Batch_ImportCCDA.PreferenceArrayUnsaved.splice(index, 1);
        }

        $("#IframXML").contents().find('a[href="#' + value + '"]').parent().remove(); // toc
        var h3 = $("#IframXML").contents().find('a[name="' + value + '"]').parent();

        var element = h3.next();
        var nextElement = null;
        do {
            nextElement = element.next();
            element.remove();

        } while (nextElement.lenght > 0 && nextElement.is('div'))

        h3.remove(); // h3
    },

    SavePreference: function (e) {

        globalAppdata["ccdaPreference"] = Batch_ImportCCDA.PreferenceArrayUnsaved;

        localStorage.setItem("ccdaPreference", JSON.stringify(globalAppdata["ccdaPreference"]));

        utility.DisplayMessages("Preference save successfully.", 1);
    },
    EncodeSpecialCharXML: function (content) {
        return utility.decodeHtml(content.replace(/&lt;/g, " 0x3c ").replace(/&gt;/g, " 0x3e ")); //.replace(/&#160;/g, " ").replace(/&amp;/g, " & ").replace(/&lt;/g, " < ").replace(/&gt;/g, " > ")
    }

};

