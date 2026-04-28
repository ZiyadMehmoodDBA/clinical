Batch_PatientImportCCDA = {
    bIsFirstLoad: true,
    params: [],
    RemoveTabHeader: false,
    AllergiesLoad_JSON: [],
    ProblemLoad_JSON: [],
    MedicationLoad_JSON: [],
    XMLContent: '',

    Load: function (params) {

        $("#pnlBatchPatientImportCCDA #tblAllergies tbody,#pnlBatchPatientImportCCDA #tblProblemLoad tbody,#pnlBatchPatientImportCCDA #tblMadication tbody").html('');
        $("#pnlBatchPatientImportCCDA #tblParseAllergies tbody,#pnlBatchPatientImportCCDA #tblParseProblemLoad tbody,#pnlBatchPatientImportCCDA #tblParseMadication tbody").html('');
        $("#pnlBatchPatientImportCCDA #tblMergeAllergies tbody,#pnlBatchPatientImportCCDA #tblMergeProblemLoad tbody,#pnlBatchPatientImportCCDA #tblMergeMadication tbody").html('');

        Batch_PatientImportCCDA.params = params;

        if (Batch_ImportCCDA.params["Medication"]) {
            $("#pnlBatchPatientImportCCDA .Madication").show();
        } else {
            $("#pnlBatchPatientImportCCDA .Madication").hide();
        }
        if (Batch_ImportCCDA.params["Allergies"]) {
            $("#pnlBatchPatientImportCCDA .Allergies").show();
        } else {
            $("#pnlBatchPatientImportCCDA .Allergies").hide();
        }
        if (Batch_ImportCCDA.params["Problems"]) {
            $("#pnlBatchPatientImportCCDA .ProblemLoad").show();
        } else {
            $("#pnlBatchPatientImportCCDA .ProblemLoad").hide();
        }
        if (!Batch_ImportCCDA.params["Medication"]) {
            Batch_ImportCCDA.XMLParse_MedicationLoad_JSON = [];
        }
        if (!Batch_ImportCCDA.params["Allergies"]) {
            Batch_ImportCCDA.XMLParse_AllergiesLoad_JSON = [];
        }
        if (!Batch_ImportCCDA.params["Problems"]) {
            Batch_ImportCCDA.XMLParse_ProblemLoad_JSON = [];
        }
        if ((Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "") || Batch_ImportCCDA.url != "") {
            Batch_PatientImportCCDA.XMLContent = Batch_ImportCCDA.url;
        } else if (Batch_ImportCCDA.XMLContent && Batch_ImportCCDA.XMLContent != "") {
            Batch_PatientImportCCDA.XMLContent = Batch_ImportCCDA.XMLContent
        }
        //Batch_PatientImportCCDA.XMLContent = Batch_ImportCCDA.XMLContent;//Batch_PatientImportCCDA.params["XMLContent"];
        //Batch_PatientImportCCDA.ParseXMLAndBind();

        if (Batch_PatientImportCCDA.params["ParentCtrl"] == "Patient_Search") {
            $('#Patient_Search #actionPanPatientSearch #Batch_ImportCCDA').hide();
            $('#Patient_Search #actionPanPatientSearch #Batch_PatientImportCCDA').show();
        }


        if (Batch_PatientImportCCDA.bIsFirstLoad) {
            Batch_PatientImportCCDA.bIsFirstLoad = false;
        }

        Batch_PatientImportCCDA.RemoveTabHeader = false;
        Batch_PatientImportCCDA.RemoveTabHeaderRepeat();



        if (Batch_ImportCCDA != null && Batch_ImportCCDA.params != null) {
            if (Batch_ImportCCDA.params.SelectedPatientId != null && Batch_ImportCCDA.params.SelectedPatientId > 0) {
                Batch_PatientImportCCDA.LoadPatientComponents(Batch_ImportCCDA.params.SelectedPatientId).done(function () {
                    Batch_PatientImportCCDA.LoadPatientParsingComponents(Batch_ImportCCDA.XMLParse_AllergiesLoad_JSON, Batch_ImportCCDA.XMLParse_ProblemLoad_JSON, Batch_ImportCCDA.XMLParse_MedicationLoad_JSON);
                });
            }
            else {
                $("#pnlBatchPatientImportCCDA #tblAllergies tBatch_ImportCCDA.XMLParse_MedicationLoad_JSONbody").html('<tr><td colspan="4" class="center" >No Allergy List Found.</td></tr>');
                $("#pnlBatchPatientImportCCDA #tblProblemLoad tbody").html('<tr><td colspan="4" class="center" >No Problem List Found..</td></tr>');
                $("#pnlBatchPatientImportCCDA #tblMadication tbody").html('<tr><td colspan="4" class="center" >No Medication List Found.</td></tr>');
            }
        }
    },

    CancelReconcile:function(){
        SelectTab("batchTabImportCCDA", "false");
    },

    ParseXMLAndBind: function () {
        if (Batch_PatientImportCCDA.XMLParse_AllergiesLoad_JSON != null && Batch_PatientImportCCDA.XMLParse_ProblemLoad_JSON != null && Batch_PatientImportCCDA.XMLParse_MedicationLoad_JSON != null) {
            Batch_PatientImportCCDA.LoadPatientParsingComponents(Batch_PatientImportCCDA.XMLParse_AllergiesLoad_JSON, Batch_PatientImportCCDA.XMLParse_ProblemLoad_JSON, Batch_PatientImportCCDA.XMLParse_MedicationLoad_JSON);
            return;
        }

        if (Batch_ImportCCDA.XMLContent) {
            Batch_PatientImportCCDA.params["XMLContent"] = Batch_ImportCCDA.XMLContent;
            var InculdedComponents = new Object();
            var Medication = false;
            var Allergies = false;
            var Problems = false;
            if (Batch_ImportCCDA.params["Medication"]) {
                Medication = true;
            }
            if (Batch_ImportCCDA.params["Allergies"]) {
                Allergies = true;
            }
            if (Batch_ImportCCDA.params["Problems"]) {
                Problems = true;
            }
            InculdedComponents = {
                'Medication': Medication,
                'Allergies': Allergies,
                'Problems': Problems
            };

            data = "XMLContent=" + Batch_PatientImportCCDA.params["XMLContent"] + "&PatientId=" + Batch_PatientImportCCDA.params["PatientId"] + "&IncludedComponents=" + JSON.stringify(InculdedComponents);

            Batch_ImportCCDA.XMLContent = Batch_PatientImportCCDA.params["XMLContent"];

            MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "PARSE_XML_COMPONENT").done(function (response) {
                if (response.status) {

                    if (response.error != '') {
                        // utility.DisplayMessages(response.error, 3);
                    }
                    Batch_PatientImportCCDA.XMLParse_AllergiesLoad_JSON = AllergiesLoad_JSON = JSON.parse(response.Allergy);
                    Batch_PatientImportCCDA.XMLParse_ProblemLoad_JSON = ProblemLoad_JSON = JSON.parse(response.Problems);
                    Batch_PatientImportCCDA.XMLParse_MedicationLoad_JSON = MedicationLoad_JSON = JSON.parse(response.Medication);
                    Batch_PatientImportCCDA.LoadPatientParsingComponents(AllergiesLoad_JSON, ProblemLoad_JSON, MedicationLoad_JSON);

                }
                else {
                    utility.DisplayMessages(response.Message, 3);

                    $("#pnlBatchPatientImportCCDA #tblParseAllergies tbody").html('<tr><td colspan="4" class="center" >No Allergy List Found.</td></tr>');
                    $("#pnlBatchPatientImportCCDA #tblParseProblemLoad tbody").html('<tr><td colspan="4" class="center" >No Problem List Found.</td></tr>');
                    $("#pnlBatchPatientImportCCDA #tblParseMadication tbody").html('<tr><td colspan="4" class="center" >No Madication List Found.</td></tr>');
                }
            });

        }
    },

    NextImport: function () {

    },

    formatAMPM: function (date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return strTime;
    },

    LoadPatientParsingComponents: function (lstAllergies, lstProblem, lstMedication) {

        if (lstAllergies != null && lstAllergies.length > 0) {
            $("#pnlBatchPatientImportCCDA #tblParseAllergies tbody").html('');
            $.each(lstAllergies, function (i, item) {
                var resultAllergy = false;
                if (Batch_PatientImportCCDA.AllergiesLoad_JSON && Batch_PatientImportCCDA.AllergiesLoad_JSON.length > 0) {
                    resultAllergy = Batch_PatientImportCCDA.containsAllery(item, Batch_PatientImportCCDA.AllergiesLoad_JSON);
                }
                var $row = $('<tr/>');
                if (resultAllergy) {
                    $row.attr("RowAdded", 0);
                } else {
                    $row.attr("RowAdded", 1);
                }
                $row.attr("JsonData", JSON.stringify(item));

                var date = new Date(Date.parse(Date(item.OnSetDate)));
                item.OnSetDate = (date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                $row.append('<td style="display:none;">' + item.AllergyId + '</td><td>' + item.Allergen + '</td><td>' + item.OnSetDate + '</td><td>' + item.Status + '</td>');
                if (resultAllergy) {
                    $row.append('<td> -- </td>')
                } else {
                    if (item.Status == "active") {
                        $row.append('<td>' + $("#ddlImportListActiveAllergy").html() + '</td>')
                    } else {
                        $row.append('<td>' + $("#ddlImportListInActiveAllergy").html() + '</td>')
                    }
                }
                $("#pnlBatchPatientImportCCDA #tblParseAllergies tbody").last().append($row);
            });
        }
        else {
            var emptyTableMsg = "No Allergy List Found.";
            var $row = $('<tr/>');
            $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
            $("#pnlBatchPatientImportCCDA #tblParseAllergies tbody").last().append($row);
        }

        if (lstProblem != null && lstProblem.length > 0) {
            $("#pnlBatchPatientImportCCDA #tblParseProblemLoad tbody").html('');
            $.each(lstProblem, function (i, item) {
                var resultProblem = false;
                if (Batch_PatientImportCCDA.ProblemLoad_JSON && Batch_PatientImportCCDA.ProblemLoad_JSON.length > 0) {
                    resultProblem = Batch_PatientImportCCDA.containsProblem(item, Batch_PatientImportCCDA.ProblemLoad_JSON);
                }
                var $row = $('<tr/>');
                if (resultProblem) {
                    $row.attr("RowAdded", 0);
                } else {
                    $row.attr("RowAdded", 1);
                }
                $row.attr("JsonData", JSON.stringify(item));

                var date = new Date(Date.parse(Date(item.StartDate)));
                item.StartDate = (date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                var StatusProb = "";
                if (item.EndDate == null || item.EndDate == "") {
                    StatusProb = "active";
                } else { StatusProb = "Inactive"; }
                $row.append('<td style="display:none;">' + item.ProblemListId + '</td><td>' + item.ICD10 + ' - ' + item.ICD10_Description + '</td><td>' + item.StartDate + '</td><td>' + StatusProb + '</td>');
                if (resultProblem) {
                    $row.append('<td class="text-center"> -- </td>')
                } else {
                    if (item.EndDate == null || item.EndDate == "") {
                        $row.append('<td>' + $("#ddlImportListActiveAllergy").html() + '</td>')
                    } else {
                        $row.append('<td>' + $("#ddlImportListInActiveAllergy").html() + '</td>')
                    }
                }
                $("#pnlBatchPatientImportCCDA #tblParseProblemLoad tbody").last().append($row);
            });
        }
        else {
            var emptyTableMsg = "No Problem List Found.";
            var $row = $('<tr/>');
            $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
            $("#pnlBatchPatientImportCCDA #tblParseProblemLoad tbody").last().append($row);
        }

        if (lstMedication.length > 0) {
            $("#pnlBatchPatientImportCCDA #tblParseMadication tbody").html('');
            $.each(lstMedication, function (i, item) {
                var resultMedication = false;
                if (Batch_PatientImportCCDA.MedicationLoad_JSON && Batch_PatientImportCCDA.MedicationLoad_JSON.length > 0) {
                    resultMedication = Batch_PatientImportCCDA.containsMedicationParser(item, Batch_PatientImportCCDA.MedicationLoad_JSON);
                }
                var $row = $('<tr/>');
                if (resultMedication) {
                    $row.attr("RowAdded", 0);
                } else {
                    $row.attr("RowAdded", 1);
                }
                $row.attr("JsonData", JSON.stringify(item));

                var date = new Date(Date.parse(Date(item.StartDate)));
                item.StartDate = (date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                var statusMed = "";
                if (item.StopDate == null || item.StopDate == "") {
                    statusMed = "active";
                } else {
                    statusMed = "completed";
                }
                $row.append('<td style="display:none;">' + item.MedicationID + '</td><td>' + item.DrugDescription + '</td><td>' + item.StartDate + '</td><td>' + statusMed + '</td>');

                if (resultMedication) {
                    $row.append('<td>--</td>')
                } else if (!resultMedication && (item.StopDate == null || item.StopDate == "")) {
                    $row.append('<td>' + $("#ddlImportListActiveMedication").html() + '</td>')
                } else {
                    $row.append('<td>' + $("#ddlImportListInActiveMedication").html() + '</td>')
                }
                $("#pnlBatchPatientImportCCDA #tblParseMadication tbody").last().append($row);
            });
        }
        else {
            var emptyTableMsg = "No Medication List Found.";
            var $row = $('<tr/>');
            $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');


            $("#pnlBatchPatientImportCCDA #tblParseMadication tbody").last().append($row);
        }

    },

    AddInCurrentPatient: function (cell, parseTableName, mergeTableName) {

        $(cell).html('<i class="fa fa-close red"></i>');
        $(cell).attr('onclick', 'Batch_PatientImportCCDA.RemoveFromCurrentPatient(this,\'' + parseTableName + '\',\'' + mergeTableName + '\')');
        var row = $(cell).parent().parent();
        isAdded = $(row).attr("RowAdded");
        $(row).attr("RowAdded", "1");
        if ($("#pnlBatchPatientImportCCDA #" + mergeTableName + " tbody tr").length == 1) {
            if ($("#pnlBatchPatientImportCCDA #" + mergeTableName + " tbody tr td").attr('colspan') == 4) {
                $("#pnlBatchPatientImportCCDA #" + mergeTableName + " tbody").html("");
            }
        }

        $("#pnlBatchPatientImportCCDA #" + mergeTableName + " tbody").last().append($(row));
        if ($("#pnlBatchPatientImportCCDA #" + parseTableName + " tbody tr").length == 0) {
            var emptyTableMsg = "No more record.";
            var $row = $('<tr/>');
            $row.append('<td colspan="4" class="center" >' + emptyTableMsg + '</td>');
            $("#pnlBatchPatientImportCCDA #" + parseTableName + " tbody").last().append($row);
        }


    },

    DeleteFromPatientData: function (type, primaryKey, Description) {
        utility.myConfirm('Do you want to delete ' + type + '?',
            function () {

                if (type == 'Medication') {
                    var params = [];
                    params["StartupScreen"] = 'manage_medications';
                    params["PatientId"] = Batch_ImportCCDA.params.SelectedPatientId;
                    params["ParentCtrl"] = Batch_ImportCCDA.params.ParentCtrl != "clinicalTabProgressNote" ? Batch_ImportCCDA.params.TabID : "Batch_ImportCCDA";
                    params["FromAdmin"] = 0;
                    LoadActionPan("DRFirst", params);
                }

                if (type == 'Allergy') {
                    var params = [];
                    params["StartupScreen"] = 'manage_allergies';
                    params["PatientId"] = Batch_ImportCCDA.params.SelectedPatientId;
                    params["ParentCtrl"] = Batch_ImportCCDA.params.ParentCtrl != "clinicalTabProgressNote" ? Batch_ImportCCDA.params.TabID : "Batch_ImportCCDA";
                    params["FromAdmin"] = 0;
                    LoadActionPan("DRFirst", params);
                }

                if (type == "ProblemList") {
                    var objData = new Object();
                    objData["ProblemListId"] = primaryKey;
                    objData["commandType"] = "DELETE_PROBLEMLIST";
                    objData["PatientId"] = Batch_ImportCCDA.params.SelectedPatientId;
                    objData["Description"] = Description;
                    var data = JSON.stringify(objData);
                    return MDVisionService.APIService(data, "MEDICAL", "ProblemList").done(function (response) {
                        response = JSON.parse(response);
                        Batch_PatientImportCCDA.ParseXMLAndBind();
                        if (Batch_ImportCCDA != null && Batch_ImportCCDA.params != null) {
                            if (Batch_ImportCCDA.params.SelectedPatientId != null && Batch_ImportCCDA.params.SelectedPatientId > 0) {
                                Batch_PatientImportCCDA.LoadPatientComponents(Batch_ImportCCDA.params.SelectedPatientId, true);
                            }
                            else {
                                $("#pnlBatchPatientImportCCDA #tblAllergies tbody").html('<tr><td colspan="4" class="center" >No Allergy List Found.</td></tr>');
                                $("#pnlBatchPatientImportCCDA #tblProblemLoad tbody").html('<tr><td colspan="4" class="center" >No Problem List Found..</td></tr>');
                                $("#pnlBatchPatientImportCCDA #tblMadication tbody").html('<tr><td colspan="4" class="center" >No Medication List Found.</td></tr>');
                            }
                        }
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                        } else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }

            },
            function () {

            },
             'Confirm Delete');
    },

    ReconcileData: function () {

        var insertedAllergies = [];
        var insertedProblemLoad = [];
        var insertedMadication = [];
        var medicationCounter = -1;
        var problemCounter = -1;
        var allergyCounter = -1;

        $("#pnlBatchPatientImportCCDA #tblParseAllergies tbody").find('tr').each(function () {
            if ($(this).attr("RowAdded") == "1") {
                var selectedValueAllergn = $(this).find("select").val();
                if (selectedValueAllergn && selectedValueAllergn > 0) {
                    var JsonData = JSON.parse($(this).attr("JsonData"));
                    var obj = new Object();
                    obj["AllergyId"] = allergyCounter;
                    obj["Allergen"] = JsonData["Allergen"];
                    obj["CreatedBy"] = JsonData["CreatedBy"];
                    obj["ModifiedBy"] = JsonData["ModifiedBy"];
                    obj["OnSetDate"] = JsonData["OnSetDate"];
                    obj["Reaction"] = JsonData["Reaction"];
                    obj["RxnormID"] = JsonData["RxnormID"];
                    obj["RxnormIDType"] = JsonData["RxnormIDType"];
                    obj["Severity"] = JsonData["Severity"];
                    obj["Status"] = JsonData["Status"];
                    obj["Type"] = JsonData["Type"];
                    obj["TypeSNOMEDCode"] = JsonData["TypeSNOMEDCode"];
                    if (selectedValueAllergn == 2) {
                        obj["IsActive"] = false;
                    } else if (selectedValueAllergn == 1) {
                        obj["IsActive"] = true;
                    }
                    insertedAllergies.push(obj);
                    allergyCounter = allergyCounter - 1;
                }
            }
        });

        $("#pnlBatchPatientImportCCDA #tblAllergies tbody").find('tr').each(function () {
            var selectedValueAllergy = $(this).find("select").val();
            if (selectedValueAllergy > 0) {
                var obj = new Object();
                obj["AllergyId"] = $(this).attr("AllergyId");
                if (selectedValueAllergy == 2) {
                    obj["IsActive"] = false;
                } else if (selectedValueAllergy == 1) {
                    obj["IsActive"] = true;
                }
                insertedAllergies.push(obj);
            }
        });

        $("#pnlBatchPatientImportCCDA #tblParseProblemLoad tbody").find('tr').each(function () {
            if ($(this).attr("RowAdded") == "1") {
                var selectedValueProb = $(this).find("select").val();

                if (selectedValueProb && selectedValueProb > 0) {
                    var JsonData = JSON.parse($(this).attr("JsonData"));
                    var obj = new Object();
                    obj["ProblemListId"] = problemCounter;
                    obj["ChronicityLevel"] = JsonData["ChronicityLevel"];
                    obj["Code"] = JsonData["Code"];
                    obj["CodeType"] = JsonData["CodeType"];
                    //obj["Comments"] = JsonData["Comments"];
                    obj["CreatedBy"] = JsonData["CreatedBy"];
                    obj["CreatedOn"] = JsonData["CreatedOn"];
                    obj["ModifiedBy"] = JsonData["ModifiedBy"];
                    obj["ModifiedOn"] = JsonData["ModifiedOn"];
                    obj["Description"] = JsonData["Description"];
                    obj["EndDate"] = JsonData["EndDate"];
                    obj["EntityId"] = JsonData["EntityId"];
                    obj["FacilityId"] = JsonData["FacilityId"];
                    obj["ICD9"] = JsonData["ICD9"];
                    obj["ICD9_Description"] = JsonData["ICD9_Description"];
                    obj["ICD10"] = JsonData["ICD10"];
                    obj["ICD10_Description"] = JsonData["ICD10_Description"];
                    obj["IsActive"] = JsonData["IsActive"];
                    obj["NegationIndex"] = JsonData["NegationIndex"];
                    obj["NegationReason"] = JsonData["NegationReason"];
                    obj["ProblemName"] = JsonData["ProblemName"];
                    obj["ProviderId"] = JsonData["ProviderId"];
                    obj["SNOMEDID"] = JsonData["SNOMEDID"];
                    obj["SNOMED_DESCRIPTION"] = JsonData["SNOMED_DESCRIPTION"];
                    obj["Severity"] = JsonData["Severity"];
                    obj["StartDate"] = JsonData["StartDate"];
                    obj["Status"] = JsonData["Status"];
                    obj["UserId"] = JsonData["UserId"];
                    if (selectedValueProb == 2) {
                        obj["IsActive"] = false;
                    } else if (selectedValueProb == 1) {
                        obj["IsActive"] = true;
                    }
                    if (obj["NegationIndex"] == null || obj["NegationIndex"] == "") {
                        obj["NegationIndex"] = false;
                    }
                    insertedProblemLoad.push(obj);
                    problemCounter = problemCounter - 1;
                }
            }
        });

        $("#pnlBatchPatientImportCCDA #tblProblemLoad tbody").find('tr').each(function () {
            var selectedValueProb = $(this).find("select").val();
            if (selectedValueProb > 0) {
                var obj = new Object();
                obj["ProblemListId"] = $(this).attr("ProblemListId");
                obj["EndDate"] = $(this).attr("EndDate");
                if (selectedValueProb == 2) {
                    obj["IsActive"] = false;
                    if (obj["EndDate"] == null || obj["EndDate"] == "") {
                        obj["EndDate"] = new Date();
                    }
                } else if (selectedValueProb == 1) {
                    obj["IsActive"] = true;
                }
                obj["NegationIndex"] = false;
                insertedProblemLoad.push(obj);
            }
        });

        $("#pnlBatchPatientImportCCDA #tblParseMadication tbody").find('tr').each(function () {
            if ($(this).attr("RowAdded") == "1") {
                var selectedValueMedication = $(this).find("select").val();

                if (selectedValueMedication && selectedValueMedication > 0) {
                    var JsonData = JSON.parse($(this).attr("JsonData"));
                    var obj = new Object();
                    obj["MedicationID"] = medicationCounter;
                    obj["RxNormId"] = JsonData["RxNormCode"];
                    obj["DoseValue"] = JsonData["DoseValue"];
                    obj["DoseUnit"] = JsonData["DoseUnit"];
                    obj["DrugDescription"] = JsonData["DrugDescription"];
                    obj["PatientNotes"] = JsonData["PatientNotes"];
                    obj["RouteBy"] = JsonData["RouteBy"];
                    obj["RouteCode"] = JsonData["RouteCode"];
                    obj["StartDate"] = JsonData["StartDate"];
                    obj["Status"] = JsonData["Status"];
                    obj["StopDate"] = JsonData["StopDate"];
                    obj["Quantity"] = JsonData["Quantity"];
                    obj["QuantityUnit"] = JsonData["QuantityUnit"];
                    obj["Substitution"] = JsonData["Substitution"];
                    obj["RepeatNumber"] = JsonData["RepeatNumber"];
                    obj["NegationReason"] = JsonData["NegationReason"];
                    obj["NegationIndex"] = JsonData["NegationIndex"];
                    obj["CreatedBy"] = JsonData["CreatedBy"];
                    obj["CreatedOn"] = JsonData["CreatedOn"];
                    obj["ModifiedBy"] = JsonData["ModifiedBy"];
                    obj["ModifiedOn"] = JsonData["ModifiedOn"];
                    obj["Refill"] = JsonData["Refill"];
                    obj["UserId"] = JsonData["UserId"];
                    obj["IsActive"] = true;
                    if (selectedValueMedication == 1) {                        
                        obj["IsActive"] = false;
                        if (obj["StopDate"] == null || obj["StopDate"] == "") {
                            obj["StopDate"] = new Date();
                        }
                    } else {
                        if (obj["StopDate"] != null && obj["StopDate"] != "") {
                            obj["StopDate"] = "";
                        }
                        obj["IsActive"] = true;
                    }
                    if (obj["NegationIndex"] == null || obj["NegationIndex"] == "") {
                        obj["NegationIndex"] = false;
                    }
                    insertedMadication.push(obj);
                    medicationCounter = medicationCounter - 1;
                }
            }
        });

        $("#pnlBatchPatientImportCCDA #tblMadication tbody").find('tr').each(function () {
            var selectedValueMedication = $(this).find("select").val();
            if (selectedValueMedication && selectedValueMedication > 0) {
                var obj = new Object();
                obj["MedicationID"] = $(this).attr("MedicationID");
                obj["StopDate"] = $(this).attr("StopDate");
                if (selectedValueMedication == 1) {
                    obj["IsActive"] = false;
                    if (obj["StopDate"] == null || obj["StopDate"] == "") {
                        obj["StopDate"] = new Date();
                    }
                } else if (selectedValueMedication == 2) {
                    obj["IsActive"] = true;
                }
                obj["NegationIndex"] = false;
                insertedMadication.push(obj);
            }
        });

        if(insertedMadication.length > 0 || insertedProblemLoad.length > 0 || insertedAllergies.length > 0){
            var objData = new Object();
            objData["PatientId"] = Batch_ImportCCDA.params.SelectedPatientId;
            objData["lstMedication"] = JSON.stringify(insertedMadication);
            objData["lstProblems"] = JSON.stringify(insertedProblemLoad);
            objData["lstAllergies"] = JSON.stringify(insertedAllergies);
            objData["ProviderId"] = $("#pnlBatchImportCCDA #ddlprovider").val();
            objData["FacilityId"] = $("#pnlBatchImportCCDA #ddlfacility").val();
            objData["FileName"] = Batch_ImportCCDA.FileName;
         
            var data = "PatientData=" + JSON.stringify(objData);
            return MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "INSERT_COMPONENTS").done(function (response) {
                if (response.status) {
                    utility.DisplayMessages("Data Reconciled Successfully", 1);
                    if (Patient_MessageCompose && Patient_MessageCompose.xmlByteData && Patient_MessageCompose.xmlByteData != "") {
                        Patient_MessageCompose.xmlByteData = "";
                    }
                    Batch_ImportCCDA.url = "";
                    //Start//24-05-2016//Ahmad Raza//showing CDS ALert icon on CCDA Import
                    //$(" #mainForm  li#CDSAlert").show();
                    //$(" #mainForm #hfTriggerLocation").val('CCDA');
                    //End//24-05-2016//Ahmad Raza//showing CDS ALert icon on CCDA Import
                    var istoggle = false;
                    if (Batch_ImportCCDA.params.SelectedPatientId + "" == localStorage.getItem("SelectedPatientId"))
                    {
                        istoggle = true;
                        ClinicalCDSDetail.showCDSAlert('', Batch_ImportCCDA.params.SelectedPatientId);
                    }

                    // To remove MU3 Alert
                    MU_Alerts.UpdateMUAlertProfile("Reconciliation,InCorporateSummaryOfCare", 0, Batch_ImportCCDA.params.SelectedPatientId, false, Batch_ImportCCDA.params.ImportProviderId, [], istoggle);
                    Batch_PatientImportCCDA.CancelReconcile();
                }
            });
        } else {
            utility.DisplayMessages("There is no data to reconcile", 3);
        }
    },

    RemoveFromCurrentPatient: function (cell, parseTableName, mergeTableName) {

        $(cell).html('<i class="fa fa-compress black"></i>');
        $(cell).attr('onclick', 'Batch_PatientImportCCDA.AddInCurrentPatient(this,\'' + parseTableName + '\',\'' + mergeTableName + '\')');
        var row = $(cell).parent().parent();
        isAdded = $(row).attr("RowAdded");
        $(row).attr("RowAdded", "1")
        if ($("#pnlBatchPatientImportCCDA #" + parseTableName + " tbody tr:first  td[colspan=4]").length > 0) {
            $("#pnlBatchPatientImportCCDA #" + parseTableName + " tbody").html('');
        }

        $("#pnlBatchPatientImportCCDA #" + parseTableName + " tbody").last().append($(row));
    },

    ResetAllergiesData: function () {
        $("#pnlBatchPatientImportCCDA #tblAllergies tbody").html('<tr><td colspan="4" class="center" >No Allergy List Found.</td></tr>');
        //$("#pnlBatchPatientImportCCDA #tblMergeAllergies tbody").html('<tr><td colspan="4" class="center" >No Allergy List Found.</td></tr>');
    },

    ResetProblemsData: function () {

        $("#pnlBatchPatientImportCCDA #tblProblemLoad tbody").html('<tr><td colspan="4" class="center" >No Problem List Found.</td></tr>');
        //$("#pnlBatchPatientImportCCDA #tblMergeProblemLoad tbody").html('<tr><td colspan="4" class="center" >No Problem List Found.</td></tr>');
    },

    ResetMadicationData: function () {
        $("#pnlBatchPatientImportCCDA #tblMadication tbody").html('<tr><td colspan="4" class="center" >No Medication List Found.</td></tr>');
        //$("#pnlBatchPatientImportCCDA #tblMergeMadication tbody").html('<tr><td colspan="4" class="center" >No Medication List Found.</td></tr>');
    },

    LoadPatientComponents: function (PatientId, isRebind) {

        if (isRebind) {
            Batch_PatientImportCCDA.AddedMadication = $("#pnlBatchPatientImportCCDA #tblMergeMadication tbody tr[rowadded='1']");
            Batch_PatientImportCCDA.AddedProblemLoad = $("#pnlBatchPatientImportCCDA #tblMergeProblemLoad tbody tr[rowadded='1']");
            Batch_PatientImportCCDA.AddedAllergies = $("#pnlBatchPatientImportCCDA #tblMergeAllergies tbody tr[rowadded='1']");

        }

        var data = "PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "Batch_ClinicalImportCCDA", "Load_Patient_Component").done(function (response) {
            if (response.status) {

                //Batch_PatientImportCCDA.ResetAllergiesData();
                //Batch_PatientImportCCDA.ResetProblemsData();
                //Batch_PatientImportCCDA.ResetMadicationData();

                AllergiesLoad_JSON = JSON.parse(response.AllergiesLoad_JSON);
                ProblemLoad_JSON = JSON.parse(response.ProblemLoad_JSON);
                MedicationLoad_JSON = JSON.parse(response.MedicationLoad_JSON);
                Batch_PatientImportCCDA.AllergiesLoad_JSON = AllergiesLoad_JSON;
                Batch_PatientImportCCDA.ProblemLoad_JSON = ProblemLoad_JSON;
                Batch_PatientImportCCDA.MedicationLoad_JSON = MedicationLoad_JSON;

                if (AllergiesLoad_JSON.length > 0) {
                    $("#pnlBatchPatientImportCCDA #tblAllergies tbody").html('');
                    $.each(AllergiesLoad_JSON, function (i, item) {
                        var resultAllergy = false;
                        if (Batch_ImportCCDA.XMLParse_AllergiesLoad_JSON && Batch_ImportCCDA.XMLParse_AllergiesLoad_JSON.length > 0) {
                            resultAllergy = Batch_PatientImportCCDA.containsAllery(item, Batch_ImportCCDA.XMLParse_AllergiesLoad_JSON);
                        }
                        var $row = $('<tr/>');
                        $row.attr("AllergyId", item.AllergyId);

                        var date = new Date(Date.parse(Date(item.OnSetDate)));
                        item.OnSetDate = (date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                        $row.append('<td style="display:none;">' + item.AllergyId + '</td><td>' + item.Allergen + '</td><td>' + item.OnSetDate + '</td><td>' + ((item.IsActive == "1" || item.IsActive.toLowerCase() == "true") ? "Active" : "Inactive") + '</td>');
                        if (resultAllergy) {
                            if (item.IsActive == "1" || item.IsActive.toLowerCase() == "true") {
                                $row.append('<td>' + $("#ddlAlreadyExistsActiveAllergy").html() + '</td>')
                            } else {
                                $row.append('<td>' + $("#ddlAlreadyExistsInActiveAllergy").html() + '</td>')
                            }
                        } else {
                            if (item.IsActive == "1" || item.IsActive.toLowerCase() == "true") {
                                $row.append('<td>' + $("#ddlAlreadyExistsActiveAllergy").html() + '</td>')
                            } else {
                                $row.append('<td>' + $("#ddlAlreadyExistsInActiveAllergy").html() + '</td>')
                            }
                        }
                        $("#pnlBatchPatientImportCCDA #tblAllergies tbody").last().append($row);


                        //var $row2 = $('<tr/>');
                        //$row2.attr("AllergyId", item.AllergyId);

                        //var date = new Date(Date.parse(Date(item.OnSetDate)));
                        //item.OnSetDate = (date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                        //$row2.append('<td style="display:none;">' + item.AllergyId + '</td><td><a class="btn btn-xs"  onclick="Batch_PatientImportCCDA.DeleteFromPatientData(\'Allergy\',\'' + item.AllergyId + '\');" title="Delete Record"><i class="fa fa-close red"></i></a></td><td>' + item.Allergen + '</td><td>' + item.OnSetDate + '</td><td>' + ((item.IsActive == "1" || item.IsActive.toLowerCase() == "true") ? "Active" : "Inactive") + '</td>');
                        //$("#pnlBatchPatientImportCCDA #tblMergeAllergies tbody").last().append($row2);

                    });
                }

                if (ProblemLoad_JSON.length > 0) {
                    $("#pnlBatchPatientImportCCDA #tblProblemLoad tbody").html('');
                    $.each(ProblemLoad_JSON, function (i, item) {
                        var resultProblem = false;
                        if (Batch_ImportCCDA.XMLParse_ProblemLoad_JSON && Batch_ImportCCDA.XMLParse_ProblemLoad_JSON.length > 0) {
                            resultProblem = Batch_PatientImportCCDA.containsProblem(item, Batch_ImportCCDA.XMLParse_ProblemLoad_JSON);
                        }
                        var $row = $('<tr/>');
                        $row.attr("ProblemListId", item.ProblemListId);
                        $row.attr("IsActive", item.IsActive);
                        $row.attr("EndDate", item.EndDate);

                        var date = new Date(Date.parse(Date(item.StartDate)));
                        item.StartDate = (date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();

                        var Description = item.ICD10_Description + ((item.SNOMED_DESCRIPTION.length > 0) ? ' - ( ' + item.SNOMED_DESCRIPTION + ' )' : " ");

                        $row.append('<td style="display:none;">' + item.ProblemListId + '</td><td>' + Description + '</td><td>' + item.StartDate + '</td><td>' + ((item.IsActive == "1" || item.IsActive.toLowerCase() == "true") ? "Active" : "Inactive") + '</td>');
                        if (resultProblem) {
                            if (item.IsActive == "1" || item.IsActive.toLowerCase() == "true") {
                                $row.append('<td>' + $("#ddlAlreadyExistsActiveAllergy").html() + '</td>')
                            } else {
                                $row.append('<td>' + $("#ddlAlreadyExistsInActiveAllergy").html() + '</td>')
                            }
                        } else {
                            if (item.IsActive == "1" || item.IsActive.toLowerCase() == "true") {
                                $row.append('<td>' + $("#ddlAlreadyExistsActiveAllergy").html() + '</td>')
                            } else {
                                $row.append('<td>' + $("#ddlAlreadyExistsInActiveAllergy").html() + '</td>')
                            }
                        }
                        $("#pnlBatchPatientImportCCDA #tblProblemLoad tbody").last().append($row);


                        //var $row2 = $('<tr/>');
                        //$row2.attr("ProblemListId", item.ProblemListId);

                        //var date = new Date(Date.parse(Date(item.StartDate)));
                        //item.StartDate = (date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                        //$row2.append('<td style="display:none;">' + item.ProblemListId + '</td><td><a class="btn btn-xs" onclick="Batch_PatientImportCCDA.DeleteFromPatientData(\'ProblemList\',\'' + item.ProblemListId + '\',\'' + item.Description + '\');" title="Delete Record"><i class="fa fa-close red"></i></a></td><td>' + Description + '</td><td>' + item.StartDate + '</td><td>' + ((item.IsActive == "1" || item.IsActive.toLowerCase() == "true") ? "Active" : "Inactive") + '</td>');
                        //$("#pnlBatchPatientImportCCDA #tblMergeProblemLoad tbody").last().append($row2);

                    });
                }

                if (MedicationLoad_JSON.length > 0) {
                    $("#pnlBatchPatientImportCCDA #tblMadication tbody").html('');
                    //$("#pnlBatchPatientImportCCDA #tblMadication tbody,#pnlBatchPatientImportCCDA #tblMergeMadication tbody").html('');
                    $.each(MedicationLoad_JSON, function (i, item) {
                        var resultMedication = false;
                        if (Batch_ImportCCDA.XMLParse_MedicationLoad_JSON && Batch_ImportCCDA.XMLParse_MedicationLoad_JSON.length > 0) {
                            //resultMedication = Batch_ImportCCDA.XMLParse_MedicationLoad_JSON.filter(function (entry) { return entry.RxNormCode = item.RxnormID; });
                            resultMedication = Batch_PatientImportCCDA.containsMedication(item, Batch_ImportCCDA.XMLParse_MedicationLoad_JSON);
                        }
                        var $row = $('<tr/>');
                        $row.attr("MedicationID", item.MedicationID);
                        $row.attr("StopDate", item.StopDate);
                        var date = new Date(Date.parse(Date(item.StartDate)));
                        item.StartDate = (date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                        var statusMedPat = "";
                        if (item.StopDate == null || item.StopDate == "") {
                            statusMedPat = "active";
                        } else {
                            statusMedPat = "completed";
                        }
                        $row.append('<td style="display:none;">' + item.MedicationID + '</td><td>' + item.MedicationName + '</td><td>' + item.StartDate + '</td><td>' + statusMedPat + '</td>');
                        if (resultMedication && item.StopDate && item.StopDate != null && item.StopDate != "") {
                            $row.append('<td>' + $("#ddlAlreadyExistsInActiveMedication").html() + '</td>')
                        } else if (resultMedication && (item.StopDate == null || item.StopDate == "")) {
                            $row.append('<td>' + $("#ddlAlreadyExistsActiveMedication").html() + '</td>')
                        } else {
                            if (item.StopDate == null || item.StopDate == "") {
                                $row.append('<td>' + $("#ddlAlreadyNotExistsActiveMedication").html() + '</td>')
                            } else if (item.StopDate != null && item.StopDate != "") {
                                $row.append('<td>' + $("#ddlAlreadyNotExistsInActiveMedication").html() + '</td>')
                            } else {
                                $row.append('<td>' + $("#ddlAlreadyExistsActiveMedication").html() + '</td>')
                            }
                        }
                        $("#pnlBatchPatientImportCCDA #tblMadication tbody").last().append($row);


                        //var $row2 = $('<tr/>');
                        //$row2.attr("MedicationID", item.MedicationID);

                        //var date = new Date(Date.parse(Date(item.StartDate)));
                        //item.StartDate = (date.getMonth() + 1) + '-' + date.getDate() + '-' + date.getFullYear();
                        //$row2.append('<td style="display:none;">' + item.MedicationID + '</td><td><a class="btn btn-xs" onclick="Batch_PatientImportCCDA.DeleteFromPatientData(\'Medication\',\'' + item.MedicationID + '\');" title="Delete Record"><i class="fa fa-close red"></i></a></td><td>' + item.MedicationName + '</td><td>' + item.StartDate + '</td><td>' + ((item.PrescriptionRcopiaID != "" && parseInt(item.PrescriptionRcopiaID) > 0) ? "Prescribed" : "UnPrescribed") + '</td>');
                        //$("#pnlBatchPatientImportCCDA #tblMergeMadication tbody").last().append($row2);
                    });
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            var output = response;
        });



    },

    containsProblem: function (obj, list) {
        var i;
        for (i = 0; i < list.length; i++) {
            if (list[i].ICD10 == obj.ICD10) {
                return true;
            }
        }
        return false;
    },

    containsAllery: function (obj, list) {
        var i;
        for (i = 0; i < list.length; i++) {
            if (list[i].RxnormID == obj.RxnormID) {
                return true;
            }
        }
        return false;
    },

    containsMedication: function (obj, list) {
        var i;
        for (i = 0; i < list.length; i++) {
            if (list[i].RxNormCode == obj.RxnormID) {
                return true;
            }
        }
        return false;
    },

    containsMedicationParser: function (obj, list) {
        var i;
        for (i = 0; i < list.length; i++) {
            if (list[i].RxnormID == obj.RxNormCode) {
                //console.log(list[i].RxnormID + "-" + obj.RxNormCode);
                return true;
            }
        }
        return false;
    },

    RemoveTabHeaderRepeat: function () {

        if (!Batch_PatientImportCCDA.RemoveTabHeader && $("#mstrDivBatch #PatientImportCCDA").length == 0) {
            setTimeout(function () { Batch_PatientImportCCDA.RemoveTabHeaderRepeat(); }, 1);
        }
        else {
            $("#mstrDivBatch #PatientImportCCDA").parent().remove();
            Batch_PatientImportCCDA.RemoveTabHeader = true;
        }

        $("#mstrDivBatch #PatientImportCCDA").parent().remove();

    },

    Cancel: function () {

        if (Batch_PatientImportCCDA.params["ParentCtrl"] == "Patient_Search") {

            $('#Patient_Search #actionPanPatientSearch #Batch_PatientImportCCDA').remove();
            $('#Patient_Search #actionPanPatientSearch #Batch_ImportCCDA').show();
            Batch_PatientImportCCDA.UnLoad();
        }
        else {

            $('#' + Batch_PatientImportCCDA.params.PanelID).parent().parent().find('#pnlBatchImportCCDA').show();
            $('#' + Batch_PatientImportCCDA.params.PanelID).parent().remove();
            Batch_PatientImportCCDA.UnLoad();
        }
    },

    ImportFileSelect: function (file) {
        if (file.files.length > 0) {
            var f = file.files[0];
            if (f) {
                var r = new FileReader();
                r.onload = function (e) {
                    var contents = e.target.result;

                    document.getElementById("IframXML").srcdoc = contents.replace(/</g, '&lt;').replace(/>/g, '&gt;<br/>');
                    //alert("Got the file.n"
                    //      + "name: " + f.name + "n"
                    //      + "type: " + f.type + "n"
                    //      + "size: " + f.size + " bytesn"
                    //      + "starts with: " + contents.substr(1, contents.indexOf("n"))
                    //);
                }
                r.readAsText(f);
            } else {
                alert("Failed to load file");
            }
        }
    },

    BatchClinicalQualityMeasureSearch: function (cqmid, pageNo, rpp) {

        if ($("#" + Batch_PatientImportCCDA.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result").css("display") === "none")
            $("#" + Batch_PatientImportCCDA.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result").show();

        var self = $("#" + Batch_PatientImportCCDA.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Search");
        var myJson = self.getMyJSON();

        if ($("#ddlprovider :selected").val() == "" && $("#dtpAppointmentDateFrom").val() == "" && $("#dtpAppointmentDateTo").val() == "") {
            utility.DisplayMessages("Sepcify a Criteria to Filter", 3);
        } else {
            Batch_PatientImportCCDA.SearchBatchClinicalQualityMeasure(myJson, cqmid, pageNo, rpp).done(function (response) {

                if (response.status != false) {
                    Batch_PatientImportCCDA.BatchClinicalQualityMeasureGridLoad(response);

                    var tableControl = Batch_PatientImportCCDA.params["PanelID"] + " #dgvBatchClinicalQualityMeasure";
                    var pagingPanelControlId = Batch_PatientImportCCDA.params["PanelID"] + " #divBatchClinicalQualityMeasurePaging";
                    var classControlName = "Batch_PatientImportCCDA";
                    var pagesToDisplay = 5;
                    var iTotalDisplayRecords = response.iTotalDisplayRecords;
                    setTimeout
                    (
                        CreatePagination(response.BatchClinicalQualityMeasureCount, pageNo, rpp, pagingPanelControlId, tableControl, classControlName, pagesToDisplay, iTotalDisplayRecords,
                            function (primaryId, pageNumber, resultPerPage) {
                                Batch_PatientImportCCDA.BatchClinicalQualityMeasureSearch(primaryId, pageNumber, resultPerPage);
                            }
                        ), 10);
                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    BatchClinicalQualityMeasureExport: function () {

        if ($("#" + Batch_PatientImportCCDA.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result").css("display") === "none")
            $("#" + Batch_PatientImportCCDA.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result").show();

        var self = $("#" + Batch_PatientImportCCDA.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Search");
        var myJson = self.getMyJSON();

        if ($("#dgvBatchClinicalQualityMeasure tr").length > 1) {
            Batch_PatientImportCCDA.BatchClinicalQualityMeasureExport_(myJson).done(function (response) {
                if (response.status != false) {
                    download("data:application/octet-stream;base64," + response.DownloadFile, response.FileName + ".xml", "application/octet-stream");

                } else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        } else {
            utility.DisplayMessages("No Record Found to Export.", 3);
        }
    },

    ClinicalQualityMeasureDetail: function (mode, cqmid, providerId, measure, event) {
        if (event != null)
            event.stopPropagation();


        var dateFrom = $("#dtpAppointmentDateFrom").val();
        var dateTo = $("#dtpAppointmentDateTo").val();
        var params = [];
        params["mode"] = mode;
        params["CQMID"] = cqmid;
        params["providerId"] = providerId;
        params["Measure"] = measure;
        params["ParentCtrl"] = "batchTabClinicalQualityMeasure";
        params["dateFrom"] = dateFrom;
        params["dateTo"] = dateTo;
        LoadActionPan("Batch_PatientImportCCDADetail", params);
    },

    BatchClinicalQualityMeasureGridLoad: function (response) {
        $("#" + Batch_PatientImportCCDA.params["PanelID"] + " #dgvBatchClinicalQualityMeasure").dataTable().fnDestroy();
        $("#" + Batch_PatientImportCCDA.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result #dgvBatchClinicalQualityMeasure tbody").find("tr").remove();

        if (response.BatchClinicalQualityMeasureCount > 0) {
            var batchClinicalQualityMeasureLoadJson = JSON.parse(response.BatchClinicalQualityMeasureLoad_JSON);
            $.each(batchClinicalQualityMeasureLoadJson, function (i, item) {
                var $row = $("<tr/>");

                $row.attr("id", "gvBatchClinicalQualityMeasure_row" + item.CQMID);
                $row.attr("onclick", "Batch_PatientImportCCDA.ClinicalQualityMeasureDetail('Edit','" + item.CQMID + "','" + response.ProviderId + "','" + item.Measure + "',event);utility.SelectGridRow($(this));");

                $row.attr("CQMID", item.CQMID);

                $row.append("<td style=\"display:none;\">" + item.CQMID + "</td><td>" + item.CQMID + "</td><td>" + item.Measure + "</td><td>" + item.InitialPopulation + "</td><td>" + item.Denominator + "</td><td>" + item.Numerator + "</td><td>" + item.DenominatorExclusion + "</td><td>" + item.DenominatorException + "</td><td>" + item.PerfromanceRate1 + "</td>");
                $("#" + Batch_PatientImportCCDA.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result #dgvBatchClinicalQualityMeasure tbody").last().append($row);
            });
            if ($("#dtpAppointmentDateFrom").val() == "") {
                $("#dtpAppointmentDateFrom").datepicker("setDate", response.DateFrom);
            }
            if ($("#dtpAppointmentDateTo").val() == "") {
                $("#dtpAppointmentDateTo").datepicker("setDate", response.DateTo);
            }
        }
        else {
            $("#" + Batch_PatientImportCCDA.params["PanelID"] + " #dgvBatchClinicalQualityMeasure").DataTable({
                "language": {
                    "emptyTable": "No Measure Found"
                },
                "autoWidth": false,
                "bLengthChange": false,
                "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable("#" + Batch_PatientImportCCDA.params["PanelID"] + " #dgvBatchClinicalQualityMeasure"));
        else
            $("#" + Batch_PatientImportCCDA.params["PanelID"] + " #pnlBatchClinicalQualityMeasure_Result #dgvBatchClinicalQualityMeasure").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });
    },

    SearchBatchClinicalQualityMeasure: function (batchClinicalQualityMeasureData, cqmid, pageNumber, rowsPerPage) {
        if (pageNumber == null)
            pageNumber = 1;
        if (rowsPerPage == null)
            rowsPerPage = 15;

        var providerId = $("#ddlprovider :selected").val();

        var data = "BatchClinicalQualityMeasureData=" + batchClinicalQualityMeasureData + "&CQMID=" + cqmid + "&PageNumber=" + pageNumber + "&RowsPerPage=" + rowsPerPage + "&providerId=" + providerId;
        return MDVisionService.defaultService(data, "Batch_PatientImportCCDA", "SEARCH_CQM_MEASURES");
    },

    BatchClinicalQualityMeasureExport_: function (batchClinicalQualityMeasureData, cqmid) {
        var providerId = $("#ddlprovider :selected").val();
        var appointmentDateFrom = $("#dtpAppointmentDateFrom").val();
        var appointmentDateTo = $("#dtpAppointmentDateTo").val();

        var data = "BatchClinicalQualityMeasureData=" + batchClinicalQualityMeasureData + "&CQMID=" + cqmid + "&providerId=" + providerId + "&AppointmentDateFrom=" + appointmentDateFrom + "&AppointmentDateTo=" + appointmentDateTo;
        return MDVisionService.defaultService(data, "Batch_PatientImportCCDA", "EXPORT_CQM_MEASURES");
    },

    UnLoad: function () {

        if (Batch_PatientImportCCDA.params != null && Batch_PatientImportCCDA.params.ParentCtrl) {
            UnloadActionPan(Batch_PatientImportCCDA.params.ParentCtrl);
            Batch_PatientImportCCDA.params = null;
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
    }
}