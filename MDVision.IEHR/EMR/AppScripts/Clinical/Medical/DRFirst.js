DRFirst = {
    params: [],
    Load: function (params) {
        BackgroundLoaderShow(true);
        DRFirst.params = params;
        if (DRFirst.params.ParentCtrl == "demographicDetail") {
            $("#pnlClinicalDRFirst #headingTitle").html = "Patient";
            $("#pnlClinicalDRFirst #headingTitle1").html = "Patient";

        }
        else if (DRFirst.params.StartupScreen == "message" || DRFirst.params.StartupScreen == "report") {
            $("#pnlClinicalDRFirst #headingTitle").html("Refills Management and Prescription Signing");
            $("#pnlClinicalDRFirst #headingTitle1").html("Refills Management and Prescription Signing");
        }
        else if (DRFirst.params.ComeFromModuleName == "Allergies") {
            $("#pnlClinicalDRFirst #headingTitle").html("Allergy Management");
            $("#pnlClinicalDRFirst #headingTitle1").html("Allergy Management");
        }
        else if (DRFirst.params.ComeFromModuleName == "Medication") {
            $("#pnlClinicalDRFirst #headingTitle").html("Medication Management");
            $("#pnlClinicalDRFirst #headingTitle1").html("Medication Management");
        }
        else if (DRFirst.params.ComeFromModuleName == "Prescription") {
            $("#pnlClinicalDRFirst #headingTitle").html("Prescriptions Management");
            $("#pnlClinicalDRFirst #headingTitle1").html("Prescriptions Management");
        }

        DRFirst.params.mode = "Add";

        if (DRFirst.params.PanelID != 'pnlClinicalDRFirst') {
            DRFirst.params.PanelID = DRFirst.params.PanelID + ' #pnlClinicalDRFirst';
        } else {
            DRFirst.params.PanelID = 'pnlClinicalDRFirst';
        }

        if ($('#PatientProfile #hfPatientId').val() != "") {
            $("#pnlClinicalDRFirst #hfPatientId").val($('#PatientProfile #hfPatientId').val());
        }
        DRFirst.GetDrFirstUrl().done(function (response) {

            response = JSON.parse(response);
            if (response.status != false) {
                $("#pnlClinicalDRFirst #Dr").attr("src", response.DrFirstUrl);
                setTimeout(function () {
                    BackgroundLoaderShow(true);
                }, 5);
                var arm = document.getElementById("Dr");
                arm.addEventListener('load', function () {
                    //console.log('loadeddr')
                    BackgroundLoaderShow(false);
                    sessClearInterval();// remove last Activity tracker
                });
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }


        });
        //DRFirst.setHttpContext();

    },

    UnLoadTab: function () {
        if (DRFirst.params["FromAdmin"] == "0") {
            if (DRFirst.params != null && DRFirst.params.ParentCtrl != null) {
                UnloadActionPan(DRFirst.params.ParentCtrl, 'DRFirst', null, DRFirst.params.PrPanelID);
            }
            else
                UnloadActionPan(null, 'DRFirst');
        }
        else {
            RemoveAdminTab();
        }
        if (DRFirst.params.ParentCtrl == "demographicDetail") {//Not Use In Future
            return;
        }
        if (DRFirst.params.StartupScreen == "message" || DRFirst.params.StartupScreen == "report") {
            DRFirst.DownLoadAllMedicationsAndPrescriptionsForLIMPMode().done(function (response) {
                BackgroundLoaderShow(true);
                response = JSON.parse(response);
                if (response.status != false) {
                    //Start 26-10-2016 Humaira Yousaf for logging of view action
                    Clinical_Medications.isViewed = true;
                    //End 26-10-2016 Humaira Yousaf for logging of view action
                    if ($('#PatientProfile #hfPatientId').val() != "") {
                        DRFirst.SearchMedicationOrPrescription(true);
                    }


                    //Start 07-11-2016 Humaira Yousaf for db audit
                    Clinical_Prescriptions.isViewed = true;
                    //End 07-11-2016 Humaira Yousaf for db audit
                    if ($('#PatientProfile #hfPatientId').val() != "") {
                        DRFirst.SearchMedicationOrPrescription(false);
                    }
                    BackgroundLoaderShow(false);
                }
                else {

                    BackgroundLoaderShow(false);
                    if (response.Message != null) {
                        if (response.Message != "") {
                            utility.DisplayMessages(response.Message, 3);
                        }
                        else {
                            utility.DisplayMessages("Problem occured in DrFirst downloading In LIMP Mode", 3);
                        }
                    }
                }

            });
            Patient_Message.RefreshCount();
        }
        else {
            BackgroundLoaderShow(true);


            DRFirst.DownLoadAllClinicals().done(function (response) {
                BackgroundLoaderShow(true);
                response = JSON.parse(response);
                if (response.status != false) {
                    var AllergiesSearch = true;
                    $("#mainForm  li#CDSAlert").show();
                    if ($('#PatientProfile #hfPatientId').val() == "" || $('#PatientProfile #hfPatientId').val() == null) {
                        $('#PatientProfile #hfPatientId').val(DRFirst.params.PatientId)
                    }
                    $.when(ClinicalCDSDetail.showCDSAlert("", $('#PatientProfile #hfPatientId').val())).then(function () {
                        if (DRFirst.params.ParentCtrl != "clinicalTabAllergies" && DRFirst.params.ParentCtrl != "clinicalTabMedications")
                            Clinical_ProgressNote.LoadCDSAlerts();
                    });
                    if (DRFirst.params.ParentCtrl != "clinicalTabAllergies" && DRFirst.params.ParentCtrl != "clinicalTabMedications") {//work when drfisrt interaction from notes
                        if ((response.AllergyDownloadSuccessfully != false && response.IsAllergyDownload != false) || (typeof response.AllergyReviewID != typeof undefined && response.AllergyReviewID > 0)) {
                            //SavedAllergyIds;
                            if (DRFirst.params.ComeFromModuleName == "Allergies") {
                                //$.when(Clinical_Allergies.getRcopiaInformaionForAllergySoap(response.SavedAllergyIds)).then(function () {
                                //Start 26-10-2016 Humaira Yousaf to add view action log
                                Clinical_Allergies.isViewed = true;
                                //End 26-10-2016 Humaira Yousaf to add view action log
                                $.when(Clinical_Allergies.allergiesSearch()).then(function () {
                                    if (response.SavedAllergyIds != "" && typeof response.SavedAllergyIds != "undefined") {
                                        $.each(response.SavedAllergyIds.split(','), function (i, item) {
                                            if ($.inArray(item, Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                                                Clinical_ProgressNote.AttachedNoteComponentIds.push(item);
                                            } if ($.inArray(item, Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                                                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(item);
                                                if (index > -1) {
                                                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                                                }
                                            }
                                            $("#pnlClinicalProgressNote #dgvAllergies input#" + item).prop("checked", true);
                                        });
                                    }
                                    else {
                                        if (typeof response.AllergyReviewID != typeof undefined && response.AllergyReviewID > 0) {
                                            var respo = {};
                                            respo.AllergySoapCount = 0;
                                            respo.AllergyReviewCount = response.AllergyReviewID;
                                            Clinical_Allergies.createAllergyBodyHTML(respo, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', "", false);
                                        }
                                    }
                                });
                            }
                        }
                        if ((response.MedicationDownloadSuccessfully != false && response.IsMedicationDownload != false) || (typeof response.MedicationReviewID != typeof undefined && response.MedicationReviewID > 0)) {
                            if (DRFirst.params.ComeFromModuleName == "Medication" || DRFirst.params.ComeFromModuleName == "Prescription") {
                                //$.when(Clinical_Medications.getRcopiaInformaionForMedicationSoap(response.SavedMedicationIds)).then(function () {
                                //Start 26-10-2016 Humaira Yousaf for logging of view action
                                Clinical_Medications.isViewed = true;
                                //End 26-10-2016 Humaira Yousaf for logging of view action
                                if (response.IsPrescriptionDeleted) {
                                    Clinical_Prescriptions.IsPrescriptionDeleted = true;
                                }
                                $.when(DRFirst.SearchMedicationOrPrescription(true)).then(function () {
                                    
                                    if (response.SavedMedicationIds != "" && typeof response.SavedMedicationIds != "undefined") {
                                        $.each(response.SavedMedicationIds.split(','), function (i, item) {
                                            Clinical_ProgressNote.AttachedNoteComponentIds.push(item + "meds");
                                            $("#pnlClinicalProgressNote #dgvMedications input#" + item).prop("checked", true);
                                        });
                                    }
                                    if (response.PrescriptionDownloadSuccessfully != false && response.IsPrescriptionDownload != false) {

                                        //$.when(Clinical_Prescriptions.getRcopiaInformaionForPrescriptionsSoap(response.SavedPrescriptionIds)).then(function () {

                                        //Start 07-11-2016 Humaira Yousaf for db audit
                                        Clinical_Prescriptions.isViewed = true;
                                        //End 07-11-2016 Humaira Yousaf for db audit
                                        $.when(DRFirst.SearchMedicationOrPrescription(false)).then(function () {
                                            if (response.SavedPrescriptionIds != "" && typeof response.SavedPrescriptionIds != "undefined") {
                                                $.each(response.SavedPrescriptionIds.split(','), function (i, item) {
                                                    Clinical_ProgressNote.AttachedNoteComponentIds.push(item + "prsc");
                                                    $("#pnlClinicalProgressNote #dgvPrescriptions input#" + item).prop("checked", true);
                                                });
                                            }
                                        });

                                        //});

                                    }
                                    else {

                                    }
                                });
                                //});
                            }
                        }
                        else {
                            if (response.PrescriptionDownloadSuccessfully != false && response.IsPrescriptionDownload != false) {
                                if (DRFirst.params.ComeFromModuleName == "Medication" || DRFirst.params.ComeFromModuleName == "Prescription") {

                                    if (response.IsPrescriptionDeleted) {
                                        Clinical_Prescriptions.IsPrescriptionDeleted = true;
                                    }
                                    //$.when(Clinical_Prescriptions.getRcopiaInformaionForPrescriptionsSoap(response.SavedPrescriptionIds)).then(function () {

                                    //Start 07-11-2016 Humaira Yousaf for db audit
                                    Clinical_Prescriptions.isViewed = true;
                                    //End 07-11-2016 Humaira Yousaf for db audit
                                    $.when(DRFirst.SearchMedicationOrPrescription(false)).then(function () {
                                        if (response.SavedPrescriptionIds != "" && typeof response.SavedPrescriptionIds != "undefined") {
                                            $.each(response.SavedPrescriptionIds.split(','), function (i, item) {
                                                Clinical_ProgressNote.AttachedNoteComponentIds.push(item + "prsc");
                                                $("#pnlClinicalProgressNote #dgvPrescriptions input#" + item).prop("checked", true);
                                            });
                                        }
                                    });

                                    //});
                                }
                            }
                        }


                    } else {
                        if (DRFirst.params.ComeFromModuleName == "Allergies") {
                            //Start 26-10-2016 Humaira Yousaf to add view action log
                            Clinical_Allergies.isViewed = true;
                            //End 26-10-2016 Humaira Yousaf to add view action log
                            Clinical_Allergies.allergiesSearch();
                        }
                        if (DRFirst.params.ComeFromModuleName == "Medication") {
                            //Start 26-10-2016 Humaira Yousaf for logging of view action
                            Clinical_Medications.isViewed = true;
                            //End 26-10-2016 Humaira Yousaf for logging of view action
                            DRFirst.SearchMedicationOrPrescription(true);
                        }
                        if (DRFirst.params.ComeFromModuleName == "Prescription") {
                            //Start 07-11-2016 Humaira Yousaf for db audit
                            Clinical_Prescriptions.isViewed = true;
                            //End 07-11-2016 Humaira Yousaf for db audit
                            DRFirst.SearchMedicationOrPrescription(false);
                        }
                        BackgroundLoaderShow(false);
                    }
                    //Start Farooq Ahmad rebind the grid of component on close 24/5/2016
                    try {
                        if (DRFirst.params.ParentCtrl == "batchTabImportCCDA") {
                            if (Batch_PatientImportCCDA != null) {
                                Batch_PatientImportCCDA.LoadPatientComponents(Batch_ImportCCDA.params.SelectedPatientId, true);
                                Batch_PatientImportCCDA.ParseXMLAndBind();
                                return;
                            }

                        }
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                    //End Farooq Ahmad rebind the grid of component on close 24/5/2016
                }
                else {

                    BackgroundLoaderShow(false);
                    if (response.Message != null) {
                        if (response.Message != "") {
                            utility.DisplayMessages(response.Message, 3);
                        }
                        else {
                            utility.DisplayMessages("Problem occured in DrFirst downloading", 3);
                        }
                    }
                }

            });
        }

        sess_Reset();
        initSession();// attach last Activity tracker
    },

    SearchMedicationOrPrescription: function (IsMedication) {

        if ($('#PatientProfile #hfPatientId').val() != "") {
            utility.LoadMUAlerts($('#PatientProfile #hfPatientId').val(), true);
        }

        if (IsMedication)
           return Clinical_Medications.medicationsSearch();
        else
           return Clinical_Prescriptions.prescriptionSearch();

        
    },

    DownLoadAllClinicals: function () {
        var objData = new Object();
        objData["PatientId"] = DRFirst.params["PatientId"];
        objData["commandType"] = "downloadallclinicals";
        objData["NotesId"] = DRFirst.params.NotesId == null ? 0 : DRFirst.params.NotesId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Rcopia");
    },
    DownLoadAllMedicationsAndPrescriptionsForLIMPMode: function () {
        var objData = new Object();
        objData["commandType"] = "DownloadClinicalsForLIMPMode";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Rcopia");
    },
    GetDrFirstUrl: function () {
        var objData = new Object();
        if (DRFirst.params["StartupScreen"] == "" || DRFirst.params["StartupScreen"] == "") {
            objData["PatientId"] = null;
        }
        else {
            objData["PatientId"] = DRFirst.params["PatientId"];
        }

        objData["StartupScreen"] = DRFirst.params["StartupScreen"];
        objData["commandType"] = "get_ssourl";
        var data = JSON.stringify(objData);

        return MDVisionService.APIService(data, "MEDICAL", "Rcopia");
    },
}