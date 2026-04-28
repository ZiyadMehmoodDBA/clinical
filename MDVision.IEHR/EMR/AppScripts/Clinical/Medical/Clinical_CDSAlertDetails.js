//Author: Ahmad Raza
//Date:  09-03-2016
//This file will handle all actions performed for CDSAlertDetails

Clinical_CDSAlertDetails = {
    params: [],
    bIsFirstLoad: true,
    patientId: null,
    originalStatus: null,
    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: Function to load CDS ALerts
    Load: function (params) {
        BackgroundLoaderShow(true);
        params["TabID"] = 'Clinical_CDSAlertDetails';
        Clinical_CDSAlertDetails.params = params;

        if ($("div#PatientProfile #hfPatientId").val() != "") {
            Clinical_CDSAlertDetails.patientId = $("div#PatientProfile #hfPatientId").val();
        }
        else {
            Clinical_CDSAlertDetails.patientId = Clinical_CDSAlertDetails.params.PatientId;
        }


        if (Clinical_CDSAlertDetails.params.PanelID != 'pnlClinicalCDSAlertDetails') {
            Clinical_CDSAlertDetails.params.PanelID = Clinical_CDSAlerts.params.PanelID + ' #pnlClinicalCDSAlertDetails';
        } else {
            Clinical_CDSAlertDetails.params.PanelID = 'pnlClinicalCDSAlertDetails';
        }

        var self = $('#frmClinicalCDSAlertDetails');
        //Start 02-03-2016 Humaira Yousaf to load dropdowns
        if (Clinical_CDSAlertDetails.bIsFirstLoad == true) {
            Clinical_CDSAlertDetails.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {
                //Start 04-03-2016 Humaira Yousaf to load CDS

                Clinical_CDSAlertDetails.CDSSearch(Clinical_CDSAlertDetails.params.CDSId);
                // if (Clinical_CDSAlertDetails.params.PanelID == "pnlClinicalFaceSheet #pnlClinicalCDS #pnlClinicalCDSAlertDetails") {
                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvLocation #lblLocation").text('Clinical Summary');
                //   $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvLocation #lblLocation").text('FaceSheet');
                // }
                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvAge #lblAge").text($(" #mainForm span#lblPatientData span").text().split(',')[2]);
                //if (typeof $(" #mainForm span#lblPatientData span").text().split(',')[1] != "undefined") {
                //    $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvAge #lblAge").text($(" #mainForm span#lblPatientData span").text().split(',')[1]);
                //}
                //else {
                //    $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvAge").addClass('hidden');
                //}
                //$('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvAge #lblAge").text($(" #mainForm span#lblPatientData span").text().split(',')[1]);
                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvGender #lblGender").text($(" #mainForm span#lblPatientData span").text().split(',')[2]);
                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails").data('serialize', $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails").serialize());
                //End 04-03-2016 Humaira Yousaf to load CDS

            });

        }
        if (globalAppdata['IsExpand'] && globalAppdata['IsExpand'].toLowerCase() == 'true') {
            $("#pnlClinicalCDSAlertDetails section div.toggle-content").css("display", "block");
            $("#pnlClinicalCDSAlertDetails section section.toggle").addClass("active");
        }
        else {
            $("#pnlClinicalCDSAlertDetails section section.toggle").removeClass("active");
            $("#pnlClinicalCDSAlertDetails section div.toggle-content").css("display", "none");
        }

    },

    //Author: Ahmad Raza
    //Date :  10-03-2016
    //Reason: Function to search CDS ALerts
    CDSSearch: function (cdsId, PageNo, rpp) {

        var isEnableInfo = false;
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Therapeutic Reference Resources", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                isEnableInfo = true;
            }
        });


        var strMessage1 = "";
        AppPrivileges.GetFormPrivileges("Medical_CDS Alerts", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage1) {
            if (strMessage1 == "") {
                Clinical_CDSAlertDetails.searchCDS(cdsId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        var self = $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon");

                        var cdsdetail = JSON.parse(response.CDSLoad_JSON);
                        if (cdsdetail != null && cdsdetail.length > 0) {
                            cdsdetail = cdsdetail[0];
                        }
                        if (cdsdetail.Status) {
                            switch (cdsdetail.Status.toLowerCase()) {
                                case "due":
                                    $('#' + Clinical_CDSAlertDetails.params.PanelID + ' #frmClinicalCDSAlertDetails #Due').attr('checked', true);
                                    break;
                                case "done":
                                    $('#' + Clinical_CDSAlertDetails.params.PanelID + ' #frmClinicalCDSAlertDetails #Done').attr('checked', true);
                                    break;
                                case "override":
                                    $('#' + Clinical_CDSAlertDetails.params.PanelID + ' #frmClinicalCDSAlertDetails #Override').attr('checked', true);
                                    break;
                                default:
                                    break;

                            }
                            Clinical_CDSAlertDetails.originalStatus = cdsdetail.Status;
                        }
                        utility.bindMyJSONByName(true, cdsdetail, false, self);
                        var ruleTypes = cdsdetail.RuleTypeDescription;

                        if (ruleTypes.indexOf(',') > -1) {
                            ruleTypes = "Combination of (" + ruleTypes + ")";
                        }
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails #txtAlertType").val(ruleTypes);
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails #txtAlertNote").val(cdsdetail.Comments);

                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails #txtDeveloper").val(cdsdetail.txtDeveloper);
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails #txtFSource").val(cdsdetail.txtFundingSource);
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails #txtRelease").val(cdsdetail.txtRelease);
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails #referenceUrl").text(cdsdetail.txtReferenceUrl);
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails #referenceUrl").attr("href", cdsdetail.txtReferenceUrl);
                        //if (cdsdetail.TriggerLocation == '1,2') {
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvLocation #lblLocation").text('Clinical Summary');
                        /*}
                        else {
                            if (cdsdetail.TriggerLocation == '1') {
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvLocation #lblLocation").text('FaceSheet');
                            }
                            else if (cdsdetail.TriggerLocation == '2') {
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvLocation #lblLocation").text('Notes');
                            }
                        }*/
                        if (cdsdetail.Gender == '0,1,2') {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvGender #lblGender").text('Male,Female,Other');
                        }
                        else if (cdsdetail.Gender == '0,1') {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvGender #lblGender").text('Male,Female');
                        }
                        else if (cdsdetail.Gender == '0,2') {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvGender #lblGender").text('Male,Other');
                        }
                        else {
                            if (cdsdetail.Gender == '0') {
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvGender #lblGender").text('Male');
                            }
                            else if (cdsdetail.Gender == '1') {
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvGender #lblGender").text('Female');
                            }
                            else if (cdsdetail.Gender == '2') {
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvGender #lblGender").text('Other');
                            }
                        }

                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon #lblEthnicityName").text(cdsdetail.Ethnicity);
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon #lblRaceName").text(cdsdetail.Race);

                        var lblAge = $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvAge #lblAge");
                        if (cdsdetail.CDSAgeCondition == 1) {
                            $(lblAge).text(cdsdetail.CDSAgeFrom);
                        }
                        else if (cdsdetail.CDSAgeCondition == 2) {
                            $(lblAge).text('Greater than ' + cdsdetail.CDSAgeFrom);
                        }
                        else if (cdsdetail.CDSAgeCondition == 3) {
                            $(lblAge).text('Greater than or equal to ' + cdsdetail.CDSAgeFrom);
                        }
                        else if (cdsdetail.CDSAgeCondition == 4) {
                            $(lblAge).text('Less than ' + cdsdetail.CDSAgeTo);
                        }
                        else if (cdsdetail.CDSAgeCondition == 5) {
                            $(lblAge).text('Less than or equal to ' + cdsdetail.CDSAgeTo);
                        }
                        else if (cdsdetail.CDSAgeCondition == 6) {
                            $(lblAge).text('Between ' + cdsdetail.CDSAgeFrom + ' and ' + cdsdetail.CDSAgeTo);
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvAge").addClass('hidden');
                        }

                        var self = $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon");

                        var cdsdetail = JSON.parse(response.PatientDemographicJSON);
                        if (cdsdetail != null && cdsdetail.length > 0) {
                            cdsdetail = cdsdetail[0];
                        }
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvLocation #lblLocation").text('Clinical Summary');
                        utility.bindMyJSONByName(true, cdsdetail, false, self);
                        $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvAge #lblAge").text($(" #mainForm span#lblPatientData span").text().split(',')[2]);

                        var vitalDetail = JSON.parse(response.VitalSignJSON);
                        if (vitalDetail != null && vitalDetail.length > 0) {

                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvVitals #VitalLabel").removeClass('hidden');
                            vitalDetail = vitalDetail[0];
                            if (vitalDetail.Weight == "" || vitalDetail.Height == "" || vitalDetail.BMI == "") {
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvVitals #VitalsLabel").addClass('hidden');
                            }
                            else {
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvVitals #VitalsLabel").removeClass('hidden');
                            }
                            var vitals = ' Weight ' + vitalDetail.Weight + ', Height ' + vitalDetail.Height + ', BMI ' + vitalDetail.BMI;
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvVitals #lblWeight").text(vitals);

                        }

                        var cdsVitalDetail = JSON.parse(response.CDSVitalSignJSON);
                        if (cdsVitalDetail != null && cdsVitalDetail.length > 0) {


                            cdsVitalDetail = cdsVitalDetail[0];
                            if (cdsVitalDetail.Weight == "" || cdsVitalDetail.Height == "" || cdsVitalDetail.BMI == "") {
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvVitals #VitalsLabel").addClass('hidden');
                            }
                            else {
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvVitals #VitalsLabel").removeClass('hidden');
                            }
                            // var vitals = cdsVitalDetail.Weight != "" ? ' Weight ' + cdsVitalDetail.Weight : "" + cdsVitalDetail.Height != "" ? ', Height ' + cdsVitalDetail.Height : "" + cdsVitalDetail.BMI != "" ? ', BMI ' + cdsVitalDetail.BMI : "";
                            var weight = cdsVitalDetail.Weight != "" ? ' Weight ' + cdsVitalDetail.Weight : "";
                            var height = cdsVitalDetail.Height != "" ? ', Height ' + cdsVitalDetail.Height : "";
                            var bmi = cdsVitalDetail.BMI != "" ? ', BMI ' + cdsVitalDetail.BMI : "";
                            var vitals = weight + " " + height + " " + bmi;
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvVitals #lblWeight").text(vitals);

                        }

                        var cdsMedicationDetail = JSON.parse(response.CDSMedicationJSON);
                        if (cdsMedicationDetail != null && cdsMedicationDetail.length > 0) {
                            // cdsMedicationDetail = cdsMedicationDetail[0];
                            var comma = "";
                            if (cdsMedicationDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvMedication #MedicationsLabel").removeClass('hidden');

                            $.each(cdsMedicationDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                var $infoButtonrow = "";
                                if (item.NDCID != "" && isEnableInfo != false) {
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(item.NDCID, "Clinical_CDSAlertDetails", 1);
                                }
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvMedication #lblMedication").append(comma + item.MedicationName + ' ' + $infoButtonrow);

                            });


                            //  $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvMedication #lblMedication").text(cdsMedicationDetail.MedicationName);
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvMedication #MedicationsLabel").addClass('hidden');
                        }
                        var medicationDetail = JSON.parse(response.MedicationJSON);
                        if (medicationDetail != null && medicationDetail.length > 0) {

                            var comma = "";
                            if (medicationDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvMedication #MedicationsLabel").removeClass('hidden');
                            $.each(medicationDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                var $infoButtonrow = "";
                                if (item.NDCID != "" && isEnableInfo != false) {
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(item.NDCID, "Clinical_CDSAlertDetails", 1);
                                }
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvMedication #lblMedication").append(comma + item.MedicationName + ' ' + $infoButtonrow);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvMedication #MedicationsLabel").addClass('hidden');
                        }

                        var cdsLabsDetail = JSON.parse(response.CDSLabResultJSON);
                        if (cdsLabsDetail != null && cdsLabsDetail.length > 0) {
                            // cdsMedicationDetail = cdsMedicationDetail[0];

                            var comma = "";
                            if (cdsLabsDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvLabs #LabsLabel").removeClass('hidden');

                            $.each(cdsLabsDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                var $infoButtonrow = "";
                                if (item.LabResultName != "" && isEnableInfo != false) {
                                    var loinc = item.LabResultName.split(' - ')[0];
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(loinc, "Clinical_CDSAlertDetails", 3);
                                }
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvLabs #lblLabs").append(comma + item.LabResultName + ' ' + $infoButtonrow);

                            });


                            //  $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvMedication #lblMedication").text(cdsMedicationDetail.MedicationName);
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvLabs #LabsLabel").addClass('hidden');
                        }
                        var labsDetail = JSON.parse(response.LabResultJSON);
                        if (labsDetail != null && labsDetail.length > 0) {

                            var comma = "";
                            if (labsDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvLabs #LabsLabel").removeClass('hidden');
                            $.each(labsDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                var $infoButtonrow = "";
                                if (item.LabResultName != "" && isEnableInfo != false) {
                                    var loinc = item.LabResultName.split(' - ')[0];
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(loinc, "Clinical_CDSAlertDetails", 3);
                                }
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvLabs #lblLabs").append(comma + item.LabResultName + ' ' + $infoButtonrow);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvLabs #LabsLabel").addClass('hidden');
                        }
                        var problemListDetail = JSON.parse(response.ProblemListJSON);
                        if (problemListDetail != null && problemListDetail.length > 0) {

                            var comma = "";
                            if (problemListDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvProblemList #ProblemsLabel").removeClass('hidden');
                            $.each(problemListDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                var $infoButtonrow = "";
                                if (item.ICD10 != "" && isEnableInfo != false) {
                                    //  var loinc = item.LabResultName.split('-')[0] + '-' + item.LabResultName.split('-')[1];
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(item.ICD10, "Clinical_CDSAlertDetails", 2);
                                }
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvProblemList #lblProblem").append(comma + item.ProblemName + ' ' + $infoButtonrow);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvProblemList #ProblemsLabel").addClass('hidden');
                        }
                        var cdsProblemListDetail = JSON.parse(response.CDSProblemListJSON);
                        if (cdsProblemListDetail != null && cdsProblemListDetail.length > 0) {

                            var comma = "";
                            if (cdsProblemListDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvProblemList #ProblemsLabel").removeClass('hidden');
                            $.each(cdsProblemListDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                var $infoButtonrow = "";
                                if (item.Problem != "" && isEnableInfo != false) {
                                    var ICD10 = item.Problem.split('-')[1].trim();
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(ICD10, "Clinical_CDSAlertDetails", 2);
                                }
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvProblemList #lblProblemList").append(comma + item.Problem + ' ' + $infoButtonrow);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvProblemList #ProblemsLabel").addClass('hidden');
                        }
                        var cdsAllergyDetail = JSON.parse(response.CDSAllergyJSON);
                        if (cdsAllergyDetail != null && cdsAllergyDetail.length > 0) {

                            var comma = "";
                            if (cdsAllergyDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvAllergy #AllergiesLabel").removeClass('hidden');
                            $.each(cdsAllergyDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvAllergy #lblAllergy").append(comma + item.Allergen);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#alertCon div#dvAllergy #AllergiesLabel").addClass('hidden');
                        }
                        var allergyDetail = JSON.parse(response.AllergyJSON);
                        if (allergyDetail != null && allergyDetail.length > 0) {

                            var comma = "";
                            if (allergyDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvAllergy #AllergiesLabel").removeClass('hidden');
                            $.each(allergyDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvAllergy #lblAllergy").append(comma + item.Allergen);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails div#actualCon div#dvAllergy #AllergiesLabel").addClass('hidden');
                        }

                        var cds_OrderSets = JSON.parse(response.CDSOrderSetJSON);
                        var Questionnair_html = JSON.parse(response.CDSLoad_JSON);
                        if (Questionnair_html && Questionnair_html[0]) {
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #cds_Questionnaire").html(Questionnair_html[0].QuestionnaireHTML);
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #cds_Questionnaire")
                                .find('[type=radio],[type=checkbox],[type=number],[type=hidden],[type=text],textarea').each(function () {

                                    if ($(this).attr('type') == 'text') {
                                        $(this).on("input", function () {
                                            Clinical_CDSAlertDetails.setControlVales(this);
                                        });
                                    }
                                    else if ($(this).is('textarea')) {
                                        $(this).on("blur", function () {
                                            $(this).text($(this).val());
                                            $(this).val($(this).val());
                                            $(this).attr('value', $(this).val());
                                            $(this).attr('text', $(this).val());
                                        });
                                    }
                                    else if ($(this).attr('type') == 'checkbox' || $(this).attr('type') == 'radio') {
                                        $(this).on("click", function () {
                                            Clinical_CDSAlertDetails.setControlVales(this);
                                        });
                                        $(this).on("chnage", function () {
                                            Clinical_CDSAlertDetails.setControlVales(this);

                                        });
                                    }

                                });
                        }


                        if (cds_OrderSets && cds_OrderSets.length > 0) {
                            $.each(cds_OrderSets, function (index, item) {
                                if (item.OrderSetId != "" && item.OrderSetName != "") {
                                    html_orderset = '<input type="checkbox" id="chkOset_' + item.OrderSetId + '" orderSetId="' + item.OrderSetId + '"><label for="chkOset_' + item.OrderSetId + '" style="padding:0 0 0 5px;">' + item.OrderSetName + '</label></br>';
                                    $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails #OrderSetlist").append(html_orderset);
                                }
                            });
                        }
                        else
                            $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails #OrderSetlist").html("");
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage1, 2);
        });
    },

    setControlVales: function ($this) {

        if ($($this).attr('type') == 'text')
            $($this).attr('value', $($this).val());
        else if ($($this).attr('type') == 'checkbox' || $($this).attr('type') == 'radio') {
            if ($($this).is(':checked'))
                $($this).attr('checked', 'checked');
            else
                $($this).attr('checked', '');
        }
        else if ($($this).is('textarea')) {
            $($this).text($($this).val());
            $($this).val($($this).val());
            $($this).attr('value', $($this).val());
            $($this).attr('text', $($this).val());
        }


    },

    //Author: Ahmad Raza
    //Date :  10-03-2016
    //Reason: Function to call DB to search CDS ALerts
    searchCDS: function (CDSId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = new Object();
        objData["CDSId"] = CDSId;
        objData["isPopup"] = "Yes";
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        objData["PatientId"] = Clinical_CDSAlertDetails.patientId;
        objData["CDSPatientStatusId"] = Clinical_CDSAlertDetails.params.CDSPatientStatusId;
        objData["NoteId"] = Clinical_CDSAlertDetail.params.NotesId && Clinical_CDSAlertDetail.params.NotesId != undefined ? Clinical_CDSAlertDetail.params.NotesId : "";
        objData["commandType"] = "get_cds_alert_detail_against_patient";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");

    },
    //Author: Ahmad Raza
    //Date :  15-03-2016
    //Reason: Updating Status of CDS Alert
    updateCDSAlertStatus: function () {
        var strMessage = "";

        AppPrivileges.GetFormPrivileges("Medical_CDS Alerts", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var status = $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails input[type='radio']:checked").attr('id');
                if ((Clinical_CDSAlertDetails.originalStatus == "Done" || Clinical_CDSAlertDetails.originalStatus == "Override") && (Clinical_CDSAlertDetails.originalStatus != status && status == "Due")) {
                    utility.DisplayMessages("User cannot change Done or Override to Due status.", 2);
                }
                else if ((Clinical_CDSAlertDetails.originalStatus == "Done") && Clinical_CDSAlertDetails.originalStatus != status) {
                    utility.DisplayMessages("User cannot change Done status.", 2);
                }
                else {
                    Clinical_CDSAlertDetails.updateCDSAlertStatusDBCall().done(function (response) {
                        response = JSON.parse(response);
                        if (response.status != false) {
                            utility.DisplayMessages(response.message, 1);
                            Clinical_CDSAlertDetails.UnLoad('saveExit');
                            Clinical_CDSAlerts.CDSSearch(null, null, null);
                            var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
                            CDSAlertCount -= 1;
                            if (CDSAlertCount > 0) {

                                $(" #mainForm  li#CDSAlert span").text(CDSAlertCount);

                                var ids = $(" #mainForm  li#CDSAlert #hfCDSIDs").val().split(",");
                                ids.pop(response.CDSId);
                                $(" #mainForm  li#CDSAlert #hfCDSIDs").val(ids.toString());

                            }
                            else {
                                $(" #mainForm  li#CDSAlert span").text('');
                                $(" #mainForm  li#CDSAlert #hfCDSIDs").val("")
                            }
                        }
                    });
                }
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },
    //Author: Ahmad Raza
    //Date :  15-03-2016
    //Reason: DB Call to Update Status of CDS Alert
    updateCDSAlertStatusDBCall: function () {
        var objData = new Object();
        objData["CDSStatus"] = $('#' + Clinical_CDSAlertDetails.params.PanelID + " #frmClinicalCDSAlertDetails input[type='radio']:checked").attr('id');
        objData["CDSId"] = Clinical_CDSAlertDetails.params.CDSId;
        objData["CDSPatientStatusId"] = Clinical_CDSAlertDetails.params.CDSPatientStatusId;
        objData["QuestionnaireHTML"] = $('#' + Clinical_CDSAlertDetails.params.PanelID + " #cds_Questionnaire").html();
        objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();
        objData["commandType"] = "update_cds_status_selected_alert";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");

    },


    //Author: Ahmad Raza
    //Date :  10-03-2016
    //Reason: Function to unload CDS ALerts
    UnLoad: function (caller) {
        if (caller == 'saveExit') {
            UnloadActionPan(Clinical_CDSAlertDetails.params["ParentCtrl"], "Clinical_CDSAlertDetails");
        }
        else {
            utility.UnLoadDialog("frmClinicalCDSAlertDetails", function () {
                UnloadActionPan(Clinical_CDSAlertDetails.params["ParentCtrl"], "Clinical_CDSAlertDetails");
            }, function () {
                UnloadActionPan(Clinical_CDSAlertDetails.params["ParentCtrl"], "Clinical_CDSAlertDetails");
            });
        }
    },






}