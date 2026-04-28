//Author: Ahmad Raza
//Date:  09-03-2016
//This file will handle all actions performed for CDSAlertDetail

Clinical_CDSAlertDetail = {
    params: [],
    bIsFirstLoad: true,
    patientId: null,

    //Author: Ahmad Raza
    //Date :  09-03-2016
    //Reason: Function to load CDS ALerts
    Load: function (params) {
        BackgroundLoaderShow(true);
        params["TabID"] = 'Clinical_CDSAlertDetail';
        Clinical_CDSAlertDetail.params = params;
        // check for cds alert dialoag without banner patient
        if (Clinical_CDSAlert.params && Clinical_CDSAlert.params.TriggerLocation && Clinical_CDSAlert.params.TriggerLocation == "FromBulkSign") {
            Clinical_CDSAlertDetail.patientId = Clinical_CDSAlertDetail.params.PatientId;
        } else {
            if ($("div#PatientProfile #hfPatientId").val() != "") {
                Clinical_CDSAlertDetail.patientId = $("div#PatientProfile #hfPatientId").val();
            }
            else {
                Clinical_CDSAlertDetail.patientId = Clinical_CDSAlertDetail.params.PatientId;
            }
        }


        if (Clinical_CDSAlertDetail.params.PanelID != 'pnlClinicalCDSAlertDetail') {
            Clinical_CDSAlertDetail.params.PanelID = Clinical_CDSAlertDetail.params.PanelID + ' #pnlClinicalCDSAlertDetail';
        } else {
            Clinical_CDSAlertDetail.params.PanelID = 'pnlClinicalCDSAlertDetail';
        }

        var self = $('#frmClinicalCDSAlertDetail');
        //Start 02-03-2016 Humaira Yousaf to load dropdowns
        if (Clinical_CDSAlertDetail.bIsFirstLoad == true) {
            Clinical_CDSAlertDetail.bIsFirstLoad = false;
            self.loadDropDowns(true).done(function () {
                //Start 04-03-2016 Humaira Yousaf to load CDS

                Clinical_CDSAlertDetail.CDSSearch(Clinical_CDSAlertDetail.params.CDSId);
                //if (Clinical_CDSAlertDetail.params.PanelID == "pnlClinicalFaceSheet #pnlClinicalCDS #pnlClinicalCDSAlertDetail") {
                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvLocation #lblLocation").text('Clinical Summary');
                //   $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvLocation #lblLocation").text('FaceSheet');
                //}
                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvAge #lblAge").text($(" #mainForm span#lblPatientData span").text().split(',')[2]);
                //if (typeof $(" #mainForm span#lblPatientData span").text().split(',')[1] != "undefined") {
                //    $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvAge #lblAge").text($(" #mainForm span#lblPatientData span").text().split(',')[1]);
                //}
                //else {
                //    $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvAge").addClass('hidden');
                //}
                //$('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvAge #lblAge").text($(" #mainForm span#lblPatientData span").text().split(',')[1]);
                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvGender #lblGender").text($(" #mainForm span#lblPatientData span").text().split(',')[2]);
                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail").data('serialize', $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail").serialize());
                //End 04-03-2016 Humaira Yousaf to load CDS

            });

        }
        if (Clinical_CDSAlertDetail.params.ParentCtrl == "clinicalTabProgressNote") {
            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #divShowOnNote").removeClass("hidden");
        } else {
            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #divShowOnNote").addClass("hidden");
        }
        if (globalAppdata['IsExpand'] && globalAppdata['IsExpand'].toLowerCase() == 'true') 
        {
            $("#pnlClinicalCDSAlertDetail section div.toggle-content").css("display", "block");
            $("#pnlClinicalCDSAlertDetail section section.toggle").addClass("active");
        }
        else {
            $("#pnlClinicalCDSAlertDetail section section.toggle").removeClass("active");
            $("#pnlClinicalCDSAlertDetail section div.toggle-content").css("display", "none");
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
        AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage1) {
            if (strMessage1 == "") {
                Clinical_CDSAlertDetail.searchCDS(cdsId, PageNo, rpp).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {

                        var self = $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon");

                        var cdsdetail = JSON.parse(response.CDSLoad_JSON);
                        if (cdsdetail != null && cdsdetail.length > 0) {
                            cdsdetail = cdsdetail[0];
                            Clinical_CDSAlertDetail.params["Title"] = cdsdetail.Title;
                        }
                        utility.bindMyJSONByName(true, cdsdetail, false, self);
                        if (cdsdetail.Status == "Due") {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #Due").prop('checked', true);
                        }
                        else if (cdsdetail.Status == "Done") {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #Done").prop('checked', true);
                        }
                        else if (cdsdetail.Status == "Override") {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #Override").prop('checked', true);

                        }

                        var ruleTypes = cdsdetail.RuleTypeDescription;

                        if (ruleTypes.indexOf(',') > -1) {
                            ruleTypes = "Combination of (" + ruleTypes + ",Demographics)";
                        }
                        if (!ruleTypes)
                            ruleTypes = 'Demographics';
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #txtAlertType").val(ruleTypes);
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #txtAlertNote").val(cdsdetail.Comments);

                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #txtDeveloper").val(cdsdetail.txtDeveloper);
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #txtFSource").val(cdsdetail.txtFundingSource);
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #txtRelease").val(cdsdetail.txtRelease);
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #referenceUrl").text(cdsdetail.txtReferenceUrl);
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #referenceUrl").attr("href", cdsdetail.txtReferenceUrl);
                        //if (cdsdetail.TriggerLocation == '1,2') { //It is changed according to EMR-2817
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvLocation #lblLocation").text('Clinical Summary');
                        /*}
                        else {
                            if (cdsdetail.TriggerLocation == '1') {
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvLocation #lblLocation").text('FaceSheet');
                            }
                            else if (cdsdetail.TriggerLocation == '2') {
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvLocation #lblLocation").text('Notes');
                            }
                        }*/
                        if (cdsdetail.Gender == '0,1,2') {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvGender #lblGender").text('Male,Female,Unknown');
                        }
                        else if (cdsdetail.Gender == '0,1') {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvGender #lblGender").text('Male,Female');
                        }
                        else if (cdsdetail.Gender == '0,2') {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvGender #lblGender").text('Male,Unknown');
                        }
                        else {
                            if (cdsdetail.Gender == '0') {
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvGender #lblGender").text('Male');
                            }
                            else if (cdsdetail.Gender == '1') {
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvGender #lblGender").text('Female');
                            }
                            else if (cdsdetail.Gender == '2') {
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvGender #lblGender").text('Unknown');
                            }
                        }

                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon #lblEthnicityName").text(cdsdetail.Ethnicity);
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon #lblRaceName").text(cdsdetail.Race);

                        if (!cdsdetail.Race) {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon [name='lblRace']").css('display', "none");
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon #lblRaceName").css('display', "none");
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon [name='lblRace']").css('display', "");
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon #lblRaceName").css('display', "");
                        }
                        if (!cdsdetail.Ethnicity)
                        {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon  [name='lblEthnicity']").css('display', "none");
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon  #lblEthnicityName").css('display', "none");
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon  [name='lblEthnicity']").css('display', "");
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon  #lblEthnicityName").css('display', "");
                        }

                        var lblAge = $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvAge #lblAge");
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
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvAge").addClass('hidden');
                        }

                        var self = $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon");

                        var cdsdetail = JSON.parse(response.PatientDemographicJSON);
                        if (cdsdetail != null && cdsdetail.length > 0) {
                            cdsdetail = cdsdetail[0];
                        }
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvLocation #lblLocation").text('Clinical Summary');
                        utility.bindMyJSONByName(true, cdsdetail, false, self);
                        $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvAge #lblAge").text($(" #mainForm span#lblPatientData span").text().split(',')[2]);

                        var vitalDetail = JSON.parse(response.VitalSignJSON);
                        if (vitalDetail != null && vitalDetail.length > 0) {

                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvVitals #VitalLabel").removeClass('hidden');
                            var vitals_ = "";
                            $.each(vitalDetail, function (index, vitalDetail) {

                                if (vitalDetail.Weight != "") {
                                    vitals_ += ' Weight ' + vitalDetail.Weight;
                                }
                                if (vitalDetail.Height != "") {
                                    if (vitals_ != "")
                                        vitals_ += ', Height ' + vitalDetail.Height;
                                    else
                                        vitals_ += ' Height ' + vitalDetail.Height;
                                }
                                if (vitalDetail.BMI != "") {
                                    if (vitals_ != "")
                                        vitals_ += ', BMI ' + vitalDetail.BMI;
                                    else
                                        vitals_ += ' BMI ' + vitalDetail.BMI;
                                }
                                if (vitalDetail.Systolic != "") {
                                    if (vitals_ != "")
                                        vitals_ += ', Systolic ' + vitalDetail.Systolic;
                                    else
                                        vitals_ += ' Systolic ' + vitalDetail.Systolic;
                                }
                                if (vitalDetail.Diastolic != "") {
                                    if (vitals_ != "")
                                        vitals_ += ', Diastolic ' + vitalDetail.Diastolic;
                                    else
                                        vitals_ += ' Diastolic ' + vitalDetail.Diastolic;
                                }
                                if (vitalDetail.PulseResult != "") {
                                    if (vitals_ != "")
                                        vitals_ += ', Pulse ' + vitalDetail.PulseResult;
                                    else
                                        vitals_ += ' Pulse ' + vitalDetail.PulseResult;
                                }
                                if (vitalDetail.TemperatureResult != "") {
                                    if (vitals_ != "")
                                        vitals_ += ', Temperature ' + vitalDetail.TemperatureResult;
                                    else
                                        vitals_ += ' Temperature ' + vitalDetail.TemperatureResult;
                                }
                                if (vitalDetail.RespirationResult != "") {
                                    if (vitals_ != "")
                                        vitals_ += ', Respiration ' + vitalDetail.RespirationResult;
                                    else
                                        vitals_ += ' Respiration ' + vitalDetail.RespirationResult;
                                }
                                if (vitalDetail.SPO2 != "") {
                                    if (vitals_ != "")
                                        vitals_ += ', Oxygen Saturation ' + vitalDetail.SPO2;
                                    else
                                        vitals_ += ' Oxygen Saturation ' + vitalDetail.SPO2;
                                }
                            });

                            if (vitals_ != "")
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #lblActualVitals").text(vitals_);

                            if (vitals_ == "")
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #VitalsLabel").addClass('hidden');
                            //else
                            //$('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #VitalsLabel").removeClass('hidden');

                        }

                        var cdsVitalDetail = JSON.parse(response.CDSVitalSignJSON);
                        if (cdsVitalDetail != null && cdsVitalDetail.length > 0) {

                            var vitals = "";
                            $.each(cdsVitalDetail, function (i, cdsVital_item) {

                                // var vitals = cdsVitalDetail.Weight != "" ? ' Weight ' + cdsVitalDetail.Weight : "" + cdsVitalDetail.Height != "" ? ', Height ' + cdsVitalDetail.Height : "" + cdsVitalDetail.BMI != "" ? ', BMI ' + cdsVitalDetail.BMI : "";
                                //var weight = cdsVitalDetail.Weight != "" ? ' Weight ' + cdsVitalDetail.Weight : "";
                                //var height = cdsVitalDetail.Height != "" ? ', Height ' + cdsVitalDetail.Height : "";
                                //var bmi = cdsVitalDetail.BMI != "" ? ', BMI ' + cdsVitalDetail.BMI : "";


                                var value_ = "";
                                if (cdsVital_item.VitalLogicalOperator == "6")
                                    value_ = cdsVital_item.VitalValueFrom + " " + cdsVital_item.VitalValueTo
                                else
                                    value_ = cdsVital_item.VitalValue;

                                //some vitals contains Result which is undesired
                                vitals += (i > 0 ? ", " : "") + (String(cdsVital_item.VitalType).replace("Result", "")) + Clinical_CDSAlertDetail.getLogicalOperatorById(cdsVital_item.VitalLogicalOperator) + value_ + " ";// + " " + cdsVital_item.VitalUnit;
                            });

                            if (vitals == "") {
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvVitals #VitalsLabel").addClass('hidden');
                            }
                            else {
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvVitals #VitalsLabel").removeClass('hidden');
                            }


                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvVitals #lblAlertVitals").text(vitals);

                        }

                        var cdsMedicationDetail = JSON.parse(response.CDSMedicationJSON);
                        if (cdsMedicationDetail != null && cdsMedicationDetail.length > 0) {
                            // cdsMedicationDetail = cdsMedicationDetail[0];
                            var comma = "";
                            if (cdsMedicationDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvMedication #MedicationsLabel").removeClass('hidden');

                            $.each(cdsMedicationDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                var $infoButtonrow = "";
                                if (item.NDCID != "" && isEnableInfo != false) {
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(item.NDCID, "Clinical_CDSAlertDetail", 1, null, null, Clinical_CDSAlertDetail.params.PatientId);
                                }
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvMedication #lblMedication").append(comma + item.MedicationName + ' ' + $infoButtonrow);

                            });


                            //  $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvMedication #lblMedication").text(cdsMedicationDetail.MedicationName);
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvMedication #MedicationsLabel").addClass('hidden');
                        }
                        var medicationDetail = JSON.parse(response.MedicationJSON);
                        if (medicationDetail != null && medicationDetail.length > 0) {

                            var comma = "";
                            if (medicationDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvMedication #MedicationsLabel").removeClass('hidden');
                            $.each(medicationDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                var $infoButtonrow = "";
                                if (item.NDCID != "" && isEnableInfo != false) {
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(item.NDCID, "Clinical_CDSAlertDetail", 1, null, null, Clinical_CDSAlertDetail.params.PatientId);
                                }
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvMedication #lblMedication").append(comma + item.MedicationName + ' ' + $infoButtonrow);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvMedication #MedicationsLabel").addClass('hidden');
                        }

                        var cdsLabsDetail = JSON.parse(response.CDSLabResultJSON);
                        if (cdsLabsDetail != null && cdsLabsDetail.length > 0) {
                            // cdsMedicationDetail = cdsMedicationDetail[0];

                            var comma = "";
                            if (cdsLabsDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvLabs #LabsLabel").removeClass('hidden');

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
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(loinc, "Clinical_CDSAlertDetail", 3, null, null, Clinical_CDSAlertDetail.params.PatientId);
                                }
                                var labText = '';

                                if (item.LabResultLogicalOperator == "6") {
                                    labText = item.LabResultName + " " + item.LabResultValueFrom + " TO " + item.LabResultValueFrom;
                                }
                                else {

                                    labText = item.LabResultName + Clinical_CDSAlertDetail.getLogicalOperatorById(item.LabResultLogicalOperator) + item.LabResultValue;
                                }
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvLabs #lblLabs").append(comma + labText + ' ' + $infoButtonrow);

                            });


                            //  $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvMedication #lblMedication").text(cdsMedicationDetail.MedicationName);
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvLabs #LabsLabel").addClass('hidden');
                        }
                        var labsDetail = JSON.parse(response.LabResultJSON);
                        if (labsDetail != null && labsDetail.length > 0) {

                            var comma = "";
                            if (labsDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvLabs #LabsLabel").removeClass('hidden');
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
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(loinc, "Clinical_CDSAlertDetail", 3, null, null, Clinical_CDSAlertDetail.params.PatientId);
                                }

                                var labResultText = item.LabResultName + " = " + item.Result;
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvLabs #lblLabs").append(comma + labResultText + ' ' + $infoButtonrow);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvLabs #LabsLabel").addClass('hidden');
                        }
                        var problemListDetail = JSON.parse(response.ProblemListJSON);
                        if (problemListDetail != null && problemListDetail.length > 0) {

                            var comma = "";
                            if (problemListDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvProblemList #ProblemsLabel").removeClass('hidden');
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
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(item.ICD10, "Clinical_CDSAlertDetail", 2, null, null, Clinical_CDSAlertDetail.params.PatientId);
                                }
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvProblemList #lblProblem").append(comma + item.ProblemName + ' ' + $infoButtonrow);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvProblemList #ProblemsLabel").addClass('hidden');
                        }
                        var cdsProblemListDetail = JSON.parse(response.CDSProblemListJSON);
                        if (cdsProblemListDetail != null && cdsProblemListDetail.length > 0) {

                            var comma = "";
                            if (cdsProblemListDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvProblemList #ProblemsLabel").removeClass('hidden');
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
                                    $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(ICD10, "Clinical_CDSAlertDetail", 2, null, null, Clinical_CDSAlertDetail.params.PatientId);
                                }
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvProblemList #lblProblemList").append(comma + item.Problem + ' ' + $infoButtonrow);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvProblemList #ProblemsLabel").addClass('hidden');
                        }
                        var cdsAllergyDetail = JSON.parse(response.CDSAllergyJSON);
                        if (cdsAllergyDetail != null && cdsAllergyDetail.length > 0) {

                            var comma = "";
                            if (cdsAllergyDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvAllergy #AllergiesLabel").removeClass('hidden');
                            $.each(cdsAllergyDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvAllergy #lblAllergy").append(comma + item.Allergen);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvAllergy #AllergiesLabel").addClass('hidden');
                        }
                        var allergyDetail = JSON.parse(response.AllergyJSON);
                        if (allergyDetail != null && allergyDetail.length > 0) {

                            var comma = "";
                            if (allergyDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvAllergy #AllergiesLabel").removeClass('hidden');
                            $.each(allergyDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }
                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvAllergy #lblAllergy").append(comma + item.Allergen);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvAllergy #AllergiesLabel").addClass('hidden');
                        }

                        var cdsInsuranceDetail = JSON.parse(response.CDSInsuranceJSON);
                        if (cdsInsuranceDetail != null && cdsInsuranceDetail.length > 0) {
                            var comma = "";
                            if (cdsInsuranceDetail.length > 1) {
                                comma = " \n ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvInsurance #InsuranceLabel").removeClass('hidden');

                            $.each(cdsInsuranceDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }

                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvInsurance #lblInsurance").append(comma + item.InsuranceName);

                            });

                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#alertCon div#dvInsurance #InsuranceLabel").addClass('hidden');
                        }
                        var InsuranceDetail = JSON.parse(response.InsuranceJSON);
                        if (InsuranceDetail != null && InsuranceDetail.length > 0) {

                            var comma = "";
                            if (InsuranceDetail.length > 1) {
                                comma = " , ";
                            }
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvInsurance #InsuranceLabel").removeClass('hidden');
                            $.each(InsuranceDetail, function (index, item) {
                                if (index > 0) {
                                    comma = " , ";
                                }
                                else {
                                    comma = "";
                                }

                                $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvInsurance #lblInsurance").append(comma + item.InsurancePlanName);

                            });
                        }
                        else {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail div#actualCon div#dvInsurance #InsuranceLabel").addClass('hidden');
                        }

                        var cds_OrderSets = JSON.parse(response.CDSOrderSetJSON);
                        var cds_NoteOrderSets = JSON.parse(response.CDSNoteOrderSetJSON);
                        var Questionnair_html = JSON.parse(response.CDSLoad_JSON);
                        if (Questionnair_html && Questionnair_html[0]) {
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #cds_Questionnaire").html(Questionnair_html[0].QuestionnaireHTML);
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #cds_Questionnaire")
                                .find('[type=radio],[type=checkbox],[type=number],[type=hidden],[type=text],textarea').each(function () {

                                    if ($(this).attr('type') == 'text') {
                                        $(this).on("input", function () {
                                            Clinical_CDSAlertDetail.setControlVales(this);
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
                                            Clinical_CDSAlertDetail.setControlVales(this);
                                        });
                                        $(this).on("chnage", function () {
                                            Clinical_CDSAlertDetail.setControlVales(this);

                                        });
                                    }

                                });
                        }


                        if (cds_OrderSets && cds_OrderSets.length > 0) {
                            $.each(cds_OrderSets, function (index, item) {

                                var checked_ = "";
                                var disable_ = "";
                                var isalready_ = false;

                                var obj_deleted = $.grep(cds_NoteOrderSets, function (item_) { return (item_.OrderSetId == item.OrderSetId && item_.CDSId == item.CDSId && item_.IsLinktoNote.toLowerCase() == 'true' && item_.NoteId == Clinical_CDSAlertDetail.params.NotesId) });
                                if (obj_deleted.length > 0)
                                { checked_ = "checked='checked'"; disable_ = "disabled='disabled'"; isalready_ = true; }
                                if (item.OrderSetId != "") {
                                    //var orderset_detail = '<label style="padding:0 0 0 5px; cursor: pointer;" onclick="Clinical_ProgressNote.OrderSetDetail(\'' + cdsId + '\',\'' + item.OrderSetId + '\',event,\'' + item.OrderSetName + '\',\'' + Clinical_CDSAlertDetail.params.TabID + '\');">' + item.OrderSetName + '</label>';
                                    if (Clinical_CDSAlertDetail.params["ParentCtrl"] == "clinicalTabProgressNote")
                                        html_orderset = '<div class="checkbox-custom"><input type="checkbox" onclick="Clinical_CDSAlertDetail.SetAlertStatus(this);" isalready="' + isalready_ + '" ' + disable_ + '' + checked_ + ' id="chkOset_' + item.OrderSetId + '" orderSetId="' + item.OrderSetId + '"><label onclick="Clinical_ProgressNote.OrderSetDetail(\'' + cdsId + '\',\'' + item.OrderSetId + '\',event,\'' + item.OrderSetName + '\',\'' + Clinical_CDSAlertDetail.params.TabID + '\');">  ' + item.OrderSetName + '</label> </div>';
                                    else
                                        html_orderset = '<label for="chkOset_' + item.OrderSetId + '" style="padding:0 0 0 5px;"><input type="checkbox" onclick="Clinical_CDSAlertDetail.SetAlertStatus(this);" isalready="' + isalready_ + '" ' + disable_ + '' + checked_ + ' id="chkOset_' + item.OrderSetId + '" orderSetId="' + item.OrderSetId + '">  ' + item.OrderSetName + '</label></br>';
                                    $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #OrderSetlist").append(html_orderset);
                                }
                            });
                        }
                        else
                            $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail #OrderSetlist").html("");
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

    SetAlertStatus: function (obj) {

        if ($(obj).is(':checked') == true)
            $("#pnlClinicalCDSAlertDetail #frmClinicalCDSAlertDetail #Done").click();
    },

    SetOrderSetCheckBox: function (obj) {
        if ($("#pnlClinicalCDSAlertDetail #frmClinicalCDSAlertDetail #Done").is(':checked') != true)
            $("#pnlClinicalCDSAlertDetail #frmClinicalCDSAlertDetail #OrderSetlist input[isalready*='false']").prop('checked', false).attr('checked', '');
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
        objData["CDSPatientStatusId"] = Clinical_CDSAlertDetail.params.CDSPatientStatusId;
        objData["PatientId"] = Clinical_CDSAlertDetail.patientId;
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
        AppPrivileges.GetFormPrivileges("ClinicalDecisionSupport_Clinical Decision Support", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Clinical_CDSAlertDetail.updateCDSAlertStatusDBCall().done(function (response) {

                    response = JSON.parse(response);
                    if (response.status != false) {
                        // check for cds alert dialoag without banner patient
                        if (Clinical_CDSAlert.params && Clinical_CDSAlert.params.TriggerLocation && Clinical_CDSAlert.params.TriggerLocation == "FromBulkSign") {
                            utility.DisplayMessages(response.message, 1);
                            Clinical_CDSAlertDetail.UnLoad('saveExit');
                            Clinical_CDSAlert.CDSSearch(null, null, null, Clinical_CDSAlert.params.CDSIds, 'Yes');
                        }
                        else {
                            // Add To Note CDS Order Sets in case of Notes.
                            Clinical_CDSAlertDetail.AddCDSToNotes();

                            utility.DisplayMessages(response.message, 1);
                            Clinical_CDSAlertDetail.UnLoad('saveExit');

                            if (Clinical_CDSAlertDetail.params["ParentCtrl"] != "Clinical_ProgressNote")
                                Clinical_CDSAlert.CDSSearch(null, null, null, $(" #mainForm  li#CDSAlert input").val(), 'Yes');

                            if ($('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail input[type='radio']:checked").attr('id') != 'Due') {
                                var CDSAlertCount = parseInt($(" #mainForm  li#CDSAlert span").text());
                                CDSAlertCount -= 1;
                                if (CDSAlertCount > 0) {

                                    $(" #mainForm  li#CDSAlert span").text(CDSAlertCount);

                                    var ids = $(" #mainForm  li#CDSAlert #hfCDSIDs").val().split(",");
                                    ids = $.grep(ids, function (value) {
                                        return value != response.CDSId;
                                    });
                                    $(" #mainForm  li#CDSAlert #hfCDSIDs").val(ids.toString());
                                    utility.VerifyMUAlert("CDS Alert", "", Clinical_CDSAlert.params.PatientId, true, "IA");
                                }
                                else {
                                    $(" #mainForm  li#CDSAlert span").text('');
                                    $(" #mainForm  li#CDSAlert #hfCDSIDs").val("");
                                    utility.VerifyMUAlert("CDS Alert", "", Clinical_CDSAlert.params.PatientId, false, "IA");
                                }
                                if (Clinical_CDSAlertDetail.params["ParentCtrl"] == "clinicalTabProgressNote" ||
                                    $("#pnlClinicalProgressNote").css('display') == 'block') {
                                    if (CDSAlertCount > 0)
                                        Clinical_ProgressNote.LoadCDSAlerts();
                                    else
                                        $("#pnlClinicalProgressNote #CDSAlert_toggle").css('display', 'none');
                                }
                            }
                        }
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },


    AddCDSToNotes: function () {

        var isAddtoNote = false;
        if ($("#pnlClinicalProgressNote").css('display') == 'block' && (Clinical_ProgressNote.params.NotesId && Clinical_ProgressNote.params.NotesId > 0))
            isAddtoNote = true;
        else if (Clinical_CDSAlertDetail.params.NotesId && Clinical_CDSAlertDetail.params.NotesId > 0)
            isAddtoNote = true;

        if (isAddtoNote) {

            var checked_OrderSets = [];
            $("#" + Clinical_CDSAlertDetail.params["PanelID"] + " #OrderSetlist").find("input:checked").each(function (i, item) {
                if ($(item).attr('isalready').toLowerCase() == 'false')
                    checked_OrderSets.push($(item).attr('ordersetid'));
            });

            if (checked_OrderSets.length > 0) {
                var ordersets_ = checked_OrderSets.join(",");
                Clinical_ProgressNote.LinkCDSOrderSetWithNote(ordersets_, Clinical_CDSAlertDetail.params.CDSId);
            }

        }

    },
    //Author: Ahmad Raza
    //Date :  15-03-2016
    //Reason: DB Call to Update Status of CDS Alert
    updateCDSAlertStatusDBCall: function () {
        var objData = new Object();
        objData["CDSStatus"] = $('#' + Clinical_CDSAlertDetail.params.PanelID + " #frmClinicalCDSAlertDetail input[type='radio']:checked").attr('id');
        objData["CDSId"] = Clinical_CDSAlertDetail.params.CDSId;
        objData["CDSPatientStatusId"] = Clinical_CDSAlertDetail.params.CDSPatientStatusId;
        objData["QuestionnaireHTML"] = $('#' + Clinical_CDSAlertDetail.params.PanelID + " #cds_Questionnaire").html();
        if (Clinical_CDSAlert.params && Clinical_CDSAlert.params.TriggerLocation && Clinical_CDSAlert.params.TriggerLocation == "FromBulkSign") {
            objData["PatientId"] = Clinical_CDSAlertDetail.params.PatientId;
        } else {
            objData["PatientId"] = $("div#PatientProfile #hfPatientId").val();
        }
        objData["commandType"] = "UPDATE_CDS_STATUS";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "CDS", "CDS");

    },


    //Author: Ahmad Raza
    //Date :  10-03-2016
    //Reason: Function to unload CDS ALerts
    UnLoad: function (caller) {
        if (caller == 'saveExit') {

            if (Clinical_CDSAlertDetail.params["ParentCtrl"] == "clinicalTabProgressNote") {

                Clinical_CDSAlertDetail.showOnNote();
            }
            UnloadActionPan(Clinical_CDSAlertDetail.params["ParentCtrl"], "Clinical_CDSAlertDetail");
        }
        else {
            utility.UnLoadDialog("frmClinicalCDSAlertDetail", function () {
                UnloadActionPan(Clinical_CDSAlertDetail.params["ParentCtrl"], "Clinical_CDSAlertDetail");
            }, function () {
                UnloadActionPan(Clinical_CDSAlertDetail.params["ParentCtrl"], "Clinical_CDSAlertDetail");
            });
        }
    },

    getLogicalOperatorById: function (id) {
        var logicalOperator = "";
        switch (id) {
            case "1":
                logicalOperator = "=";
                break;
            case "2":
                logicalOperator = ">";
                break;
            case "3":
                logicalOperator = ">=";
                break;
            case "4":
                logicalOperator = "<";
                break;
            case "5":
                logicalOperator = "<=";
                break;
            case "6":
                logicalOperator = "between";
                break;
            default:
                break;
        }
        return " " + logicalOperator + " ";
    },
    addSelecteAttributeToDropdown: function (obj) {


        $(obj).find("option").each(function (i, item) {

            if ($(this).val() == $(obj).val()) {

                $(this).attr("selected", true);

            }
            else {
                $(this).removeAttr("selected");
            }

        });

    },


    returnheader: function (CDSTitle, id) {

        return '<header>' +
       '<CDSQuestionnaire title="' + CDSTitle + '"  id="' + id + '" class="CDS_NotesComponent">' +
       '<span class="btn-xs" style="cursor: default;color: #0088cc;" title="' + CDSTitle + '">' + CDSTitle + '</span> ' +
       '<a onclick="Clinical_ProgressNote.removeCDSQuestionnaireFromNote(this);" class="btn btn-link btn-xs closeBtn hidden" title="Remove"><i class="fa fa-times"></i></a>' +
       '</CDSQuestionnaire> </header>';
    },



    showOnNote: function () {

        var obj = $("#" + Clinical_CDSAlertDetail.params.PanelID + " #divShowOnNote #chkShowOnNote");

        if ($(obj).prop('checked')) {
            //HEADER start
            CDSTitle = Clinical_CDSAlertDetail.params.Title ? Clinical_CDSAlertDetail.params.Title : "CDS Questionnaire";
            var mainDiv = $(document.createElement('div'));

            var ul = $(document.createElement('ul'));
            var li = $(document.createElement('li'));
            ul.attr('class', 'list-unstyled')
            ul.attr('name', 'CDSQuestionnaire');
            li.attr('class', 'CDSComponent');
            li.attr('NoteComponentId', 'NCDummyId');
            li.append(Clinical_CDSAlertDetail.returnheader(CDSTitle));

            //HEADER end

            //BODY start
            var sectionBody = $(document.createElement('section'));
            // sectionBody.attr('id', "Cli_Goals_Main");
            var bodyDiv = $(document.createElement('div'));
            var bodyUL = $(document.createElement('ul'));

            var Question = "";
            var Answer = "";

            $("#" + Clinical_CDSAlertDetail.params.PanelID + " #cds_Questionnaire div.col-xs-12").each(function (index, item) {

                var bodyLI = $(document.createElement('li'));

                if ($(this).hasClass("dropdown")) {
                    Question = $(this).find("label").text();
                    Answer = $(this).find("select").val();
                }
                else if ($(this).hasClass("yesNo")) {
                    if ($($(this).find("input")[0]).prop('checked') || $($(this).find("input")[1]).prop('checked')) {
                        Question = $($(this).find("label")[0]).text();
                        Answer = $($(this).find("input")[0]).prop('checked') ? " Yes " : " No ";
                    }
                }

                else if ($(this).hasClass("yesNoNote")) {
                    if ($($(this).find("input")[0]).prop('checked') || $($(this).find("input")[1]).prop('checked')) {
                        Question = $($(this).find("label")[0]).text();
                        Answer = $($(this).find("input[type='checkbox']")[0]).prop('checked') ? " Yes " : " No ";
                        Answer += $(this).find("input[type=text]").val() != "" ? ("{" + $(this).find("input[type=text]").val() + "}") : "";
                    }
                }

                else if ($(this).hasClass("textbox")) {
                    Question = $(this).find("label").text();
                    Answer = $(this).find("input").val();
                }
                else if ($(this).hasClass("textarea")) {
                    Question = $(this).find("label").text();
                    Answer = $(this).find("textarea").val();
                }

                if (Answer.trim() != "") {
                    bodyLI.append("<b>" + Question + "</b><br/>");
                    bodyLI.append(Answer);

                    bodyUL.append(bodyLI);
                }
                Question = "";
                Answer = "";
            });
            bodyUL.addClass("list-unstyled");
            sectionBody.append(bodyUL);
            li.append(sectionBody);
            //BODY end

            ul.append(li);
            // mainDiv.append(ul);

            // console.log(mainDiv.html());
            Clinical_ProgressNote.appendHTMLToNote(ul.html());

        }
    },
}