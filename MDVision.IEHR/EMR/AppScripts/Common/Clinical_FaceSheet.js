
Clinical_FaceSheet = {
    //Author: Muhammad Arshad
    //Date: 12-10-2015
    //This file will handle all actions performed for Face Sheet
    bIsFirstLoad: true,
    EditableGrid: null,
    params: [],
    Components: [],
    FaceSheetModels: [],
    //Author: Muhammad Arshad
    //Date: 12-10-2015
    //This function will be called once tab is clicked, it expects parameters to be used for FaceSheet
    Load: function (params) {
        Clinical_FaceSheet.params = params;
        $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet #hfPatientId").val($("div#PatientProfile #hfPatientId").val());
        //Clinical_FaceSheet.EditableGrid = utility.MakeEditableGrid(PanelVitalsGrid, VitalsGridId, Clinical_FaceSheet, "0", false, false, false, false);
        if (Clinical_FaceSheet.params.mode == null) {
            Clinical_FaceSheet.params.mode = "Add";
        }
        if (Clinical_FaceSheet.params.PanelID != 'pnlClinicalFaceSheet') {
            Clinical_FaceSheet.params.PanelID = Clinical_FaceSheet.params.PanelID + ' #pnlClinicalFaceSheet';
        } else {
            Clinical_FaceSheet.params.PanelID = 'pnlClinicalFaceSheet';
        }
        if (Clinical_FaceSheet.params.ParentCtrl == "demographicDetail") {
            $('#' + Clinical_FaceSheet.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
        }
        if (Clinical_FaceSheet.params.PanelID != 'mstrDivClinical #pnlClinicalFaceSheet') {

            var self = $('#' + Clinical_FaceSheet.params.PanelID);

            self.loadDropDowns(true).done(function () {
                Clinical_FaceSheet.loadFaceSheet();
                self.find('#btnExportCCDA').dropdown();
            });

            utility.CreateDatePicker(Clinical_FaceSheet.params.PanelID + '  #dpVitalsDate', function () {
            }, true);
            $('#' + Clinical_FaceSheet.params.PanelID + ' #tpVitalsTime').timepicker().on('changeTime.timepicker', function (e) {
                disableFocus: false
            });

            if ($('#' + Clinical_FaceSheet.params.PanelID + ' #PatientProfile #hfPatientId').val() != "") {
                $('#' + Clinical_FaceSheet.params.PanelID + " #hfPatientId").val($('#PatientProfile #hfPatientId').val());
            }
            if (Clinical_FaceSheet.params.ParentCtrl == "demographicDetail") {
                $('#' + Clinical_FaceSheet.params.PanelID + " #hfPatientId").val(Clinical_FaceSheet.params.patientID);
            }
            $('#' + Clinical_FaceSheet.params.PanelID + " #divExportCCDA").show();
        }
        if (Clinical_FaceSheet.params.ParentCtrl) {
            $('#' + Clinical_FaceSheet.params.PanelID + " .viewall").addClass("disabled");
        }
        else {
            $('#' + Clinical_FaceSheet.params.PanelID + " .viewall").removeClass("disabled");
        }

        if (globalAppdata["isTransPubHealthAgHealthCareSurveys"] && globalAppdata["isTransPubHealthAgHealthCareSurveys"].toLowerCase() == "false")
            $('#' + Clinical_FaceSheet.params.PanelID + " #menuExportCCDA #liHealthCareSurvey").addClass("hidden");
        if (globalAppdata["isTransPubHealthAgCaseReporting"] && globalAppdata["isTransPubHealthAgCaseReporting"].toLowerCase() == "false")
            $('#' + Clinical_FaceSheet.params.PanelID + " #pnlSection_Search #btnExportCaseReport").addClass("hidden");
        else
            $('#' + Clinical_FaceSheet.params.PanelID + " #pnlSection_Search #btnExportCaseReport").removeClass("hidden");
        if (globalAppdata["isImplantableDevices"] && globalAppdata["isImplantableDevices"].toLowerCase() == "false")
            $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet #divImplantable").addClass("hidden");
        else
            $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet #divImplantable").removeClass("hidden");
        
    },

    disableClinicalSummary:function(){
        if (typeof DefaultMenuSelected != "undefined" && DefaultMenuSelected == "MDVisionBilling") {
            $("#pnlSection_Search #frmClinicalFaceSheet .disableClinicalFace_Sheet").addClass("disableAll");
        }
    },
    //Author: Muhammad Arshad
    //Date: 31-03-2016
    //This function will handle opening of Clinical Summary from Export C-CDA menu
    OpenCaseReports: function () {
        var allRecordsparams = [];
        allRecordsparams["FaceSheetComponents"] = Clinical_FaceSheet.Components;
        allRecordsparams["FromAdmin"] = "0";
        allRecordsparams["UserId"] = globalAppdata['AppUserId'];
        allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
        allRecordsparams["ParentCtrl"] = Clinical_FaceSheet.params.TabID;//'mstrTabDashBoard';
        var componentActionPan = "";
        componentActionPan = "Clinical_CaseReports";


        LoadActionPan(componentActionPan, allRecordsparams);
    },
    openSummary: function (summaryType) {
        AppPrivileges.GetFormPrivileges("FaceSheet_Face Sheet", "Export CCDA", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var allRecordsparams = [];
                allRecordsparams["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                allRecordsparams["FromAdmin"] = "0";
                allRecordsparams["UserId"] = globalAppdata['AppUserId'];
                allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
                allRecordsparams["ParentCtrl"] = Clinical_FaceSheet.params.TabID;//'mstrTabDashBoard';
                var componentActionPan = "";
                if (summaryType != null && summaryType.toLowerCase() == "clinicalsummary") {
                    componentActionPan = "Clinical_ClinicalSummary";
                }
                else if (summaryType != null && summaryType.toLowerCase() == "referralsummary") {
                    componentActionPan = "Clinical_ReferralSummary";
                }
                else if (summaryType != null && summaryType.toLowerCase() == "referralnote") {
                    componentActionPan = "Clinical_ReferralNote";
                }
                else if (summaryType != null && summaryType.toLowerCase() == "countinuityofcaredocument") {
                    componentActionPan = "Clinical_ContinuityofCareDocument";
                }
                else if (summaryType && summaryType.toLowerCase() == "healthcaresurvey") {
                    componentActionPan = "Clinical_HealthCareSurvey";
                }

                LoadActionPan(componentActionPan, allRecordsparams);
            }

        });

    },

    //Author: Muhammad Arshad
    //Date: 12-10-2015
    //This function will handle opening of Face Sheet Components popup on click of ViewAll

    OpenViewAllRecords: function (ComponentType) {
        BackgroundLoaderShow(true);
        if (ComponentType != null) {
            var allRecordsparams = [];
            allRecordsparams["FaceSheetComponents"] = Clinical_FaceSheet.Components;
            allRecordsparams["FromAdmin"] = "0";
            allRecordsparams["UserId"] = globalAppdata['AppUserId'];
            allRecordsparams["PatientId"] = $('#PatientProfile #hfPatientId').val();
            allRecordsparams["ParentCtrl"] = Clinical_FaceSheet.params.TabID;//'mstrTabDashBoard';

            var compnentActionPan = "";
            if (ComponentType.toLowerCase() == "allergies") {
                compnentActionPan = "Clinical_Allergies";
            }
            else if (ComponentType.toLowerCase() == "problemlist") {
                compnentActionPan = "Clinical_ProblemLists";
            }
            else if (ComponentType.toLowerCase() == "vitals") {

                compnentActionPan = "Clinical_Vitals";
            }
            else if (ComponentType.toLowerCase() == "notes") {
                compnentActionPan = "Clinical_NotesSearch";
            }
            else if (ComponentType.toLowerCase() == "appointments") {
                compnentActionPan = "clinicalFaceSheetAppointments";
            }
            else if (ComponentType.toLowerCase() == "implantable devices") {
                compnentActionPan = "Clinical_Implantable";
            }
            else if (ComponentType.replace(/\s/g, '').toLowerCase() == "labresults") {
                compnentActionPan = "Clinical_LabOrder";

                allRecordsparams["Type"] = "Result";
            }
            else if (ComponentType.replace(/\s/g, '').toLowerCase() == "laborders") {
                compnentActionPan = "Clinical_LabOrder";

                allRecordsparams["Type"] = "Order";
            }
            else if (ComponentType.replace(/\s/g, '').toLowerCase() == "procedureorders") {
                compnentActionPan = "Clinical_ProcedureOrder";
            }

            else if (ComponentType.replace(/\s/g, '').toLowerCase() == "radiologyorders") {
                compnentActionPan = "Clinical_RadiologyOrder";
            }
            else if (ComponentType.toLowerCase() == "medications") {
                compnentActionPan = "Clinical_Medications";
            }
            else if (ComponentType.toLowerCase() == "referrals") {
                compnentActionPan = "Patient_Referrals";
            }
            else if (ComponentType.toLowerCase() == "immunization") {
                compnentActionPan = "Clinical_Immunization";
            }
            else if (ComponentType.toLowerCase() == "complaints") {
                compnentActionPan = "Clinical_ComplaintsFaceSheet";
            }
            else if (ComponentType.replace(/\s/g, '').toLowerCase() == "patientdocument") {
                compnentActionPan = "Patient_Document";
            }
            var isResolved = false;
            var isAfterUnload = false;

            if (Clinical_FaceSheet.params.ParentCtrl == "demographicDetail") {

                var parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;

                ComponentType = ComponentType.replace(/\s+/g, "");
                if (ComponentType.toLowerCase().indexOf('vital') > -1) {
                    var params = [];
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["FromAdmin"] = "0";
                    params["ParentCtrl"] = "Clinical_FaceSheet";
                    params["mode"] = "Add";
                    params["VitalSignId"] = "-1";
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_Vitals')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_Vitals', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_Vitals', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_Vitals";

                } else if (ComponentType.toLowerCase().indexOf('problem') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["ProblemListId"] = -1;
                    params["FromAdmin"] = "0";
                    params["mode"] = "Add";
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["ParentCtrl"] = "Clinical_FaceSheet";


                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_ProblemLists')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_ProblemLists', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_ProblemLists', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_ProblemLists";
                } else if (ComponentType.toLowerCase().indexOf('medication') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["MedicationsId"] = "-1";
                    params["MedicationsTab"] = "Medications";
                    params["PrescriptionId"] = "-1";
                    params["FromAdmin"] = "0";
                    params["mode"] = "Add";
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_Medications')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_Medications', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_Medications', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_Medications";
                } else if (ComponentType.toLowerCase().indexOf('allergies') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["AllergyId"] = -1;
                    params["FromAdmin"] = "0";
                    params["mode"] = "Add";
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_Allergies')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_Allergies', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_Allergies', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_Allergies";
                } else if (ComponentType.toLowerCase().indexOf('labresult') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["LabResultId"] = -1;
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["Type"] = "Result";
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";


                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_LabOrder')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_LabOrder', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_LabOrder', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_LabOrder";
                } else if (ComponentType.toLowerCase().indexOf('laborder') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["LabOrderId"] = -1;
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["Type"] = "Order";
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_LabOrder')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_LabOrder', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_LabOrder', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_LabOrder";
                } else if (ComponentType.toLowerCase().indexOf('procedureorders') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["ProcedureOrderId"] = -1;
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";


                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_ProcedureOrder')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_ProcedureOrder', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_ProcedureOrder', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_ProcedureOrder";
                } else if (ComponentType.toLowerCase().indexOf('radiologyorders') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["RadiologyOrderId"] = -1;
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["Type"] = "Order";
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_RadiologyOrder')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_RadiologyOrder', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_RadiologyOrder', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_RadiologyOrder";
                } else if (ComponentType.toLowerCase().indexOf('referrals') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["ReferralId"] = "-1";
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Patient_Referrals')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Patient_Referrals', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Patient_Referrals', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Patient_Referrals";
                } else if (ComponentType.toLowerCase().indexOf('immunization') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["ImmunizationId"] = "-1";
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["patientID"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_Immunization')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_Immunization', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_Immunization', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_Immunization";
                } else if (ComponentType.toLowerCase().indexOf('notes') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["UserId"] = globalAppdata["AppUserId"];
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["PatientId"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_NotesSearch')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_NotesSearch', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_NotesSearch', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_NotesSearch";
                } else if (ComponentType.toLowerCase().indexOf('appointment') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["UserId"] = globalAppdata["AppUserId"];
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["PatientId"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'clinicalFaceSheetAppointments')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('clinicalFaceSheetAppointments', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('clinicalFaceSheetAppointments', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "clinicalFaceSheetAppointments";
                } else if (ComponentType.toLowerCase().indexOf('complaints') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["UserId"] = globalAppdata["AppUserId"];
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["PatientId"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_ComplaintsFaceSheet')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_ComplaintsFaceSheet', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_ComplaintsFaceSheet', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_ComplaintsFaceSheet";
                } else if (ComponentType.toLowerCase().indexOf('document') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["UserId"] = globalAppdata["AppUserId"];
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["PatientId"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";


                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Patient_Document')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Patient_Document', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Patient_Document', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Patient_Document";
                } else if (ComponentType.toLowerCase().indexOf('implantable') > -1) {

                    BackgroundLoaderShow(false);
                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        UnloadActionPan('Clinical_FaceSheet', Clinical_FaceSheet.params.ChildPanelID);
                    }

                    var params = [];
                    params["UserId"] = globalAppdata["AppUserId"];
                    params["mode"] = "Add";
                    params["FromAdmin"] = "0";
                    params["PatientId"] = Clinical_FaceSheet.params.patientID;
                    params["FaceSheetComponents"] = Clinical_FaceSheet.Components;
                    params["ParentCtrl"] = "Clinical_FaceSheet";

                    if (Clinical_FaceSheet.params.ChildPanelID != null && Clinical_FaceSheet.params.ChildPanelID != "") {
                        $.when(UnloadActionPan(null, 'Clinical_Implantable')).then(function () {
                            setTimeout(function () {
                                LoadActionPan('Clinical_Implantable', params, parentPanelId);
                            }, 510);
                        });
                    } else {
                        setTimeout(function () {
                            LoadActionPan('Clinical_Implantable', params, parentPanelId);
                        }, 510);
                    }
                    Clinical_FaceSheet.params.ChildPanelID = "Clinical_Implantable";
                }
            } else {

                var FaceSheetActionPan = GetTab("clinicalTabFaceSheet").ActionPanContainer;


                //Start//07/01/2016//Abid Ali//PopUp shown/hiddens events show/hide its own pager controls

                $('#' + GetTab("clinicalTabFaceSheet").ActionPanContainer).modal().on('shown.bs.modal', function () {

                    //Checks button controls inside div.#FaceSheetPager
                    var faceSheetPagerLength = $(this).find('#FaceSheetPager').length;
                    //Jquery div.#FaceSheetPager context
                    var faceSheetpager = $(this).find('#FaceSheetPager');
                    if (faceSheetPagerLength > 0) {
                        //show/hide button controls acording to resoltion
                        EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                    }
                    if (ComponentType.replace(/\s/g, '').toLowerCase() == "radiologyorders" ||
                        ComponentType.replace(/\s/g, '').toLowerCase() == "laborders" ||
                        ComponentType.replace(/\s/g, '').toLowerCase() == "labresults" ||
                        ComponentType.replace(/\s/g, '').toLowerCase() == "procedureorders") {
                        $('#' + GetTab("clinicalTabFaceSheet").ActionPanContainer).find(".panel-actions").remove();
                    }
                }).on('hidden.bs.modal', function () {
                    if ($('body').find('.modal-backdrop').length > 0) {
                        $('body').addClass('modal-open');
                    }
                    else
                        $('body').removeClass('modal-open');

                    // remove top most popedup component
                    Clinical_FaceSheet.EmptyFaceSheetModels();


                });
                // Registers resize event for Facesheet
                EMRUtility.PopUpResize("clinicalTabFaceSheet");
                //End//07/01/2016//Abid Ali//PopUp shown/hiddens events show/hide its own pager controls


                if ($("#" + FaceSheetActionPan).html().trim() != "" && $("#" + FaceSheetActionPan).html().trim().toLowerCase().indexOf("pnl") > 0) {
                    var ChildPanelID = $("#" + FaceSheetActionPan).find("div:first").attr("id");
                    if (ChildPanelID == "pnlClinicalFaceSheet") {
                        ChildPanelID = "pnlPatientDocument";
                    }
                    var ChildActionPan = "";
                    if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("allergies") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        Clinical_Allergies.unLoadTab().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);


                        });
                    }
                    if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("patientdocument") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        Patient_Document.PagerUnload().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);


                        });
                    }
                    if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("referrals") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        Patient_Referrals.UnLoadTab().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);


                        });
                    }
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("problem") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        Clinical_ProblemLists.UnLoadTab().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);


                        });
                    }
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("vitals") > -1) {
                        //BackgroundLoaderShow(false);
                        isAfterUnload = true;
                        Clinical_Vitals.unLoadTab().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("notes") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        Clinical_NotesSearch.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("appointments") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        clinicalFaceSheetAppointments.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                        //Start//14-06-2016//Ahmad Raza// implimentation of showing Lab Results on Facesheet
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("labresult") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        clinicalFaceSheetAppointments.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                        //End//14-06-2016//Ahmad Raza// implimentation of showing Lab Results on Facesheet
                        //Start//30-06-2016//Ahmad Raza// implimentation of showing Lab orders on Facesheet
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("laborder") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        clinicalFaceSheetAppointments.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                        //End//30-06-2016//Ahmad Raza// implimentation of showing Lab orders on Facesheet

                        //Start//14-06-2016//Ahmad Raza// implimentation of showing Medications on Facesheet
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("medications") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        clinicalFaceSheetAppointments.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                        //End//14-06-2016//Ahmad Raza// implimentation of showing Medications on Facesheet

                        //Start//30-06-2016//Ahmad Raza// implimentation of showing immunization on Facesheet
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("immunization") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        clinicalFaceSheetAppointments.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                        //End//30-06-2016//Ahmad Raza// implimentation of showing immunization on Facesheet

                        //Start//30-06-2016//Ahmad Raza// implimentation of showing procedure order on Facesheet
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("procedureorder") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        clinicalFaceSheetAppointments.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                        //End//30-06-2016//Ahmad Raza// implimentation of showing procedure order on Facesheet

                        //Start//30-06-2016//Ahmad Raza// implimentation of showing radiology order on Facesheet
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("radiologyorder") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        clinicalFaceSheetAppointments.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                        //End//30-06-2016//Ahmad Raza// implimentation of showing radiology order on Facesheet

                        //Start//30-06-2016//Ahmad Raza// implimentation of showing radiology order on Facesheet
                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("complaints") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        clinicalFaceSheetAppointments.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }
                        //End//30-06-2016//Ahmad Raza// implimentation of showing radiology order on Facesheet

                    else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("implantable") > -1) {
                        isAfterUnload = true;
                        BackgroundLoaderShow(false);
                        clinicalFaceSheetAppointments.UnLoad().then(function () {
                            isResolved = true;
                            setTimeout(function () {
                                LoadActionPan(compnentActionPan, allRecordsparams);
                            }, 500);
                        });
                    }

                    //Start//30-06-2016//Ahmad Raza// implimentation of showing Complaints on Facesheet
                    //else if (ChildPanelID != null && ChildPanelID.toLowerCase().indexOf("complaints") > -1) {
                    //    BackgroundLoaderShow(false);
                    //    clinicalFaceSheetAppointments.UnLoad().then(function () {
                    //        isResolved = true;
                    //        setTimeout(function () {
                    //            LoadActionPan(compnentActionPan, allRecordsparams);
                    //        }, 500);
                    //    });
                    //}
                    //End//30-06-2016//Ahmad Raza// implimentation of showing Complaints on Facesheet

                    //UnloadActionPan("clinicalTabFaceSheet", ChildActionPan);
                }


                if (isAfterUnload == true) {
                    ////setTimeout(function () {
                    //    LoadActionPan(ActionPan, params);
                    //    BackgroundLoaderShow(false);
                    ////}, 1000);
                }
                else {
                    LoadActionPan(compnentActionPan, allRecordsparams);
                    BackgroundLoaderShow(false);
                }

            }

        }
    },

    updateComponentOrderSorting: function (FaceSheetCompnentSorted) {
        IsBackgroundLoaderShow = false;
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("FaceSheet_Face Sheet", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_FaceSheet.updateFaceSheetOrderSorting_Dbcall(FaceSheetCompnentSorted).done(function (response) {
                    IsBackgroundLoaderShow = true;
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
            Clinical_FaceSheet.loadFaceSheet(false);
        });
    },
    //Author: Muhammad Arshad
    //Date: 12-10-2015
    //This function will handle fill of Face Sheet

    bindFaceSheet: function (response) {

        var faceSheetLoad = JSON.parse(response.FaceSheetLoad_JSON);
        var faceSheetLoadJSON = faceSheetLoad.listFSOrder;
        var allergiesLoadJSON = JSON.parse(response.Allergy_JSON);
        var problemListLoadJSON = JSON.parse(response.ProblemList_JSON);
        var notesLoadJSON = JSON.parse(response.Notes_JSON);
        var vitalSignsSoapLoadJSON = JSON.parse(response.VitalSignsSoap_JSON);
        var patientAppointmentLoadJSON = JSON.parse(response.PatientAppointment_JSON);
        var historyLoadJSON = JSON.parse(response.History_JSON);
        var labResultLoadJSON = JSON.parse(response.LabResults_JSON);
        var medicationsLoadJSON = JSON.parse(response.Medications_JSON);
        var labOrdersLoadJSON = JSON.parse(response.LabOrders_JSON);
        var procedureOrdersLoadJSON = JSON.parse(response.ProcedureOrders_JSON);
        var radiologyOrdersLoadJSON = JSON.parse(response.RadiologyOrders_JSON);
        var patientReferralsLoadJSON = JSON.parse(response.PatientReferrals_JSON);
        var immunizationLoadJSON = JSON.parse(response.Immunization_JSON);
        var complaintsLoadJSON = JSON.parse(response.Complaints_JSON);
        var PatientDocumentLoadJSON = JSON.parse(response.PatientDocument_JSON);
        var implantableDevicesLoadJSON = JSON.parse(response.Implantable_JSON);

        // Start 12-14-2015 Muhammad Arshad Load Componentssponse.Immuni Div on the go on the basis of response
        // first we clear all components while loading facesheet
        $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet div#divUserFaceSheet").empty();

        var divGridTemplate = $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet div#pnlResultTemplate").clone();
        var divDragable = "";
        Clinical_FaceSheet.Components = [];

        var patientGender = $("div#PatientProfile #hfPatientSex").val();
        var gender = {
            Male: "male",
            Female: "female"
        };

        $.each(faceSheetLoadJSON, function (i, item) {

            var currentCompDivId = "";
            if (item.ComponentName.toLowerCase() == "allergies") {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divAllergies";
                $("#listAllergiesData").empty();
                $("#listAllergiesData").append("<li> <div class='col-xs-3'>Allergen</div> <div class='col-xs-6'>Reaction</div><div class='col-xs-3'>Onset Date</div> </li>");
                if (allergiesLoadJSON.length > 0) {
                    $.each(allergiesLoadJSON, function (i, item) {
                        //Begin 28-12-2015 Muhammad Arshad Bug# EMR-170 Patient Face Sheet in Clinical Module -> Allergies UI
                        $("#listAllergiesData").append("<li class='col-xs-3'> <div class='col-xs-3' data-toggle='tooltip' data-placement='left' title='" + item.Allergen + "'> <div class='size100per ellipses'>" + item.Allergen + " </div></div> <div class='col-xs-6' data-toggle='tooltip' data-placement='left' title='" + item.Reaction + "'> <div class='size100per ellipses'>" + item.Reaction + " </div></div><div class='col-xs-3'> " + utility.RemoveTimeFromDate(null, item.Severity) + " </div> </li>");
                        //End 28-12-2015 Muhammad Arshad Bug# EMR-170 Patient Face Sheet in Clinical Module -> Allergies UI
                    });
                }
                else {
                    $("#listAllergiesData").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Allergy Found.</div></li>");
                }
            }
            else if (item.ComponentName.toLowerCase() == "problem list") {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divProblemList";
                $("#listProblemListData").empty();
                $("#listProblemListData").append("<li> <div class='col-xs-4'>Problem</div> <div class='col-xs-4'>Chronicity Level</div><div class='col-xs-4'>Status</div> </li>");
                if (problemListLoadJSON.length > 0) {
                    if (problemListLoadJSON.length == 1 && problemListLoadJSON[0].ProblemName == 'No Known Problems') {
                        $("#listProblemListData").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Known Problems.</div></li>");
                    }
                    else {
                        $.each(problemListLoadJSON, function (i, item) {
                            var status = "";
                            if (item.IsActive == "True") {
                                status = "Active"
                            } else {
                                status = item.InActiveChkBoxValue;
                            }

                            if (item.ProblemName != 'No Known Problems')
                                $("#listProblemListData").append("<li class='col-xs-3'> <div class='col-xs-4'  data-toggle='tooltip' data-placement='left' title='" + item.ProblemName + "' > <div class='size100per ellipses'> " + item.ProblemName + " </div></div> <div class='col-xs-4'> " + item.ChronicityLevel + " </div><div class='col-xs-4'> " + status + " </div> </li>");

                        });
                    }
                }
                else {
                    $("#listProblemListData").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Problem List Found.</div></li>");
                }

            }
            else if (item.ComponentName.toLowerCase() == "lab results") {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divLabResults";
                $("#lstLabResults").empty();
                $("#lstLabResults").append("<li><div class='col-xs-8'>Test</div><div class='col-xs-4'>Received On</div> </li>");
                if (labResultLoadJSON.length > 0) {
                    $.each(labResultLoadJSON, function (i, item) {
                        var CreatedOn = item.CreatedOn;
                        var d = new Date(CreatedOn);
                        var convertedDate = CreatedOn == "" ? "" : $.datepicker.formatDate('mm/dd/yy ', new Date(d));
                        $("#lstLabResults").append("<li class='col-xs-3'> <div class='col-xs-8'  data-toggle='tooltip' data-placement='left' title='" + item.CPTCodeDescription + "' > <div class='size100per ellipses'> " + item.CPTCodeDescription + " </div></div> <div class='col-xs-4'> " + convertedDate + " </div> </li>");

                    });
                }
                else {
                    $("#lstLabResults").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Lab Result Found.</div></li>");
                }

            }

            else if (item.ComponentName.toLowerCase() == "medications") {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divMedications";
                $("#lstMedications").empty();
                $("#lstMedications").append("<li><div class='col-xs-8'>Drug</div><div class='col-xs-4'>Provider</div> </li>");
                if (medicationsLoadJSON.length > 0) {
                    $.each(medicationsLoadJSON, function (i, item) {

                        $("#lstMedications").append("<li class='col-xs-3'> <div class='col-xs-8'  data-toggle='tooltip' data-placement='left' title='" + item.MedicationName + "' > <div class='size100per ellipses'> " + item.MedicationName + " </div></div> <div class='col-xs-4'> " + item.ProviderName + " </div> </li>");

                    });
                }
                else {
                    $("#lstMedications").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Medication Found.</div></li>");
                }

            }
            else if (item.ComponentName.toLowerCase().indexOf("vitals") > -1) {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divVitals";
                $("#listVitalsData").empty();
                $("#listVitalsData").append("<li> <div class='col-xs-3'>Date</div> <div class='col-xs-3'>BP</div><div class='col-xs-3'>Temp.</div><div class='col-xs-3'>BMI</div> </li>");
                if (vitalSignsSoapLoadJSON.length > 0) {                    
                    $.each(vitalSignsSoapLoadJSON, function (i, item) {                       
                        if (item.Systolic > 0 && item.Diastolic > 0) {
                            $("#listVitalsData").append("<li class='col-xs-3'> <div class='col-xs-3'> " + utility.RemoveTimeFromDate(null, item.VitalSignDate) + " </div> <div class='col-xs-3'> " + item.Systolic + '/' + item.Diastolic + " </div><div class='col-xs-3'> " + item.TemperatureResult + " </div><div class='col-xs-3'> " + (item.BMI ? Clinical_FaceSheet.GetBMIHtml(item.BMI) : "") + " </div> </li>");
                        }
                        else {
                            $("#listVitalsData").append("<li class='col-xs-3'> <div class='col-xs-3'> " + utility.RemoveTimeFromDate(null, item.VitalSignDate) + " </div> <div class='col-xs-3'> " + item.Systolic + '' + item.Diastolic + " </div><div class='col-xs-3'> " + item.TemperatureResult + " </div><div class='col-xs-3'> " + (item.BMI ? Clinical_FaceSheet.GetBMIHtml(item.BMI) : "") + " </div> </li>");
                        }
                    });
                }
                else {
                    $("#listVitalsData").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Vitals Found.</div></li>");
                }
            }
            else if (item.ComponentName.toLowerCase().indexOf("note") > -1) {
                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divNotes";
                $("#listNotesData").empty();
                $("#listNotesData").append("<li> <div class='col-xs-4'>Visit Reason</div> <div class='col-xs-4'>VisitDate</div><div class='col-xs-4'>Provider Name</div> </li>");
                if (notesLoadJSON.length > 0) {
                    $.each(notesLoadJSON, function (i, item) {
                        $("#listNotesData").append("<li class='col-xs-3'> <div class='col-xs-4'> " + item.VisitReason + " </div> <div class='col-xs-4'> " + utility.RemoveTimeFromDate(null, item.VisitDate) + " </div><div class='col-xs-4'> " + item.ProviderName + " </div> </li>");
                    });
                }
                else {
                    $("#listNotesData").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Notes Found.</div></li>");
                }
            }
            else if (item.ComponentName.toLowerCase().indexOf("history") > -1) {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divHisotry";
                $("#divHistoryData").empty();
                if (historyLoadJSON.length > 0) {
                    $.each(historyLoadJSON, function (i, item) {
                        var finalSoapText = '';
                        if (item.MedicalHxSoapText != "") {
                            finalSoapText = '<p>';
                            var soapText = document.createElement('div');
                            $(soapText).html(item.MedicalHxSoapText);

                            $(soapText).find('div').each(function (index, item) {
                                var soap = $(this).html();
                                var nextDisease = $($(soapText).find('div')[index + 1]).html();

                                if (soap != "") {
                                    if (nextDisease != null) {
                                        if (soap.indexOf('The patient underwent') == -1 && soap.indexOf('The patient also underwent') == -1 && soap.indexOf('Status') == -1 && soap.indexOf('Test Result') == -1 && soap.indexOf('Onset') == -1 && soap.indexOf('Duration') == -1 && soap.indexOf('Severity') == -1
                                             && soap.indexOf('Pattern') == -1 && soap.indexOf('Aggravated by') == -1 && soap.indexOf('Location') == -1 &&
                                            nextDisease.indexOf('The patient underwent') == -1 && nextDisease.indexOf('The patient also underwent') == -1 && nextDisease.indexOf('Status') == -1 && nextDisease.indexOf('Test Result') == -1 && nextDisease.indexOf('Onset') == -1 && nextDisease.indexOf('Duration') == -1 && nextDisease.indexOf('Severity') == -1
                                             && nextDisease.indexOf('Pattern') == -1 && nextDisease.indexOf('Aggravated by') == -1 && nextDisease.indexOf('Location') == -1) {

                                            finalSoapText += soap[soap.length - 1] == ":" || soap[soap.length - 1] == "." ? soap.replace(soap[soap.length - 1], ', ') : soap + ", ";
                                        }
                                        else {
                                            finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                                        }
                                    }
                                    else {
                                        finalSoapText += soap[soap.length - 1] == ":" ? soap.replace(soap[soap.length - 1], '.') + " " : soap + " ";
                                    }
                                }
                            });
                            $(soapText).find('div').remove();
                            if (finalSoapText == '<p>') {
                                finalSoapText += $(soapText).html() + "</p>"
                            }
                            else {
                                finalSoapText += '<br>' + $(soapText).html() + "</p>"
                            }
                        }
                        $("#divHistoryData").append($.parseHTML(item.SocialHxSoapText + "\n" + item.BirthHxSoapText + "\n" + finalSoapText + "\n" + item.FamilyHxSoapText + "\n" + item.SurgicalHxSoapText + "\n" + item.HospitalizationHxSoapText + "\n</br>" + item.SocPsyandBehaviorHxSoapText));
                    });
                }
                else {
                    $("#divHistoryData").append("<div class='col-xs-12 noWordBreak text-center'>No History Found.</div>");
                }
            }

            else if (item.ComponentName.toLowerCase().indexOf("lab orders") > -1) {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divLabOrders";
                $("#lstLabOrders").empty();
                $("#lstLabOrders").append("<li><div class='col-xs-8'>Test</div><div class='col-xs-4'>Ordered On</div> </li>");
                if (labOrdersLoadJSON.length > 0) {
                    $.each(labOrdersLoadJSON, function (i, item) {
                        var OrderDate = item.OrderDate;
                        var d = new Date(OrderDate);
                        var convertedDate = OrderDate == "" ? "" : $.datepicker.formatDate('mm/dd/yy ', new Date(d));
                        $("#lstLabOrders").append("<li class='col-xs-3'> <div class='col-xs-8'  data-toggle='tooltip' data-placement='left' title='" + item.Test + "' > <div class='size100per ellipses'> " + item.Test + " </div></div> <div class='col-xs-4'> " + convertedDate + " </div> </li>");
                    });
                }
                else {
                    $("#lstLabOrders").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Lab Order Found.</div></li>");
                }
            }

            else if (item.ComponentName.toLowerCase().indexOf("procedure orders") > -1) {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divProcedureOrders";
                $("#lstProcedureOrders").empty();
                $("#lstProcedureOrders").append("<li><div class='col-xs-8'>Procedure</div><div class='col-xs-4'>Ordered On</div> </li>");
                if (procedureOrdersLoadJSON.length > 0) {
                    $.each(procedureOrdersLoadJSON, function (i, item) {
                        var OrderDate = item.OrderDate;
                        var d = new Date(OrderDate);
                        var convertedDate = OrderDate == "" ? "" : $.datepicker.formatDate('mm/dd/yy ', new Date(d));
                        $("#lstProcedureOrders").append("<li class='col-xs-3'> <div class='col-xs-8'  data-toggle='tooltip' data-placement='left' title='" + item.Procedures + "' > <div class='size100per ellipses'> " + item.Procedures + " </div></div> <div class='col-xs-4'> " + convertedDate + " </div> </li>");

                    });
                }
                else {
                    $("#lstProcedureOrders").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Procedure Order Found.</div></li>");
                }
            }

            else if (item.ComponentName.toLowerCase().indexOf("radiology orders") > -1) {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divRadiologyOrders";
                $("#lstRadiologyOrders").empty();
                $("#lstRadiologyOrders").append("<li><div class='col-xs-8'>Procedure</div><div class='col-xs-4'>Ordered On</div> </li>");
                if (radiologyOrdersLoadJSON.length > 0) {
                    $.each(radiologyOrdersLoadJSON, function (i, item) {
                        var OrderDate = item.OrderDate;
                        var d = new Date(OrderDate);
                        var convertedDate = OrderDate == "" ? "" : $.datepicker.formatDate('mm/dd/yy ', new Date(d));
                        $("#lstRadiologyOrders").append("<li class='col-xs-3'> <div class='col-xs-8'  data-toggle='tooltip' data-placement='left' title='" + item.Test + "' > <div class='size100per ellipses'> " + item.Test + " </div></div> <div class='col-xs-4'> " + convertedDate + " </div> </li>");

                    });
                }
                else {
                    $("#lstRadiologyOrders").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Diagnostic Imaging Order Found.</div></li>");
                }
            }

            else if (item.ComponentName.toLowerCase().indexOf("referrals") > -1) {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divRefferals";
                $("#lstReferrals").empty();
                $("#lstReferrals").append("<li><div class='col-xs-8'>Procedure</div><div class='col-xs-4'>Ordered On</div> </li>");
                if (patientReferralsLoadJSON.length > 0) {
                    $.each(patientReferralsLoadJSON, function (i, item) {
                        var CreatedOn = item.CreatedOn;
                        var d = new Date(CreatedOn);
                        var convertedDate = CreatedOn == "" ? "" : $.datepicker.formatDate('mm/dd/yy ', new Date(d));
                        $("#lstReferrals").append("<li class='col-xs-3'> <div class='col-xs-8'  data-toggle='tooltip' data-placement='left' title='" + item.Procedures + "' > <div class='size100per ellipses'> " + item.Procedures + " </div></div> <div class='col-xs-4'> " + convertedDate + " </div> </li>");

                    });
                }
                else {
                    $("#lstReferrals").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Patient Referral Found.</div></li>");
                }
            }

            else if (item.ComponentName.toLowerCase().indexOf("immunization") > -1) {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divImmunization";
                $("#lstImmunizations").empty();
                $("#lstImmunizations").append("<li> <div class='col-xs-4'>Immunization</div> <div class='col-xs-4'>Administered On</div><div class='col-xs-4'>Type</div> </li>");
                if (immunizationLoadJSON.length > 0) {
                    $.each(immunizationLoadJSON, function (i, item) {
                        var AdministrationDate;
                        var d;
                        if (item.AdministrationDate != "") {
                            AdministrationDate = item.AdministrationDate;
                            d = new Date(AdministrationDate);
                        }

                        $("#lstImmunizations").append("<li class='col-xs-3'> <div class='col-xs-4'  data-toggle='tooltip' data-placement='left' title='" + item.VaccineName + "' > <div class='size100per ellipses'> " + item.VaccineName + " </div></div> <div class='col-xs-4'> " + (item.AdministrationDate != "" ? $.datepicker.formatDate('mm/dd/yy ', new Date(d)) : "") + " </div> <div class='col-xs-4'> " + item.Type + " </div></li>");
                    });
                }
                else {
                    $("#lstImmunizations").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Immunization Found.</div></li>");
                }
            }

            else if (item.ComponentName.toLowerCase().indexOf("appointment") > -1) {
                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divAppointments";
                $("#listAppointmentData").empty();
                $("#listAppointmentData").append("<li> <div class='col-xs-4'>Date</div> <div class='col-xs-4'>Reason</div><div class='col-xs-4'>Provider</div> </li>");
                Clinical_FaceSheet.params.patientAppointmentLoadJSON = patientAppointmentLoadJSON;
                
                if (patientAppointmentLoadJSON.length > 0) {
                    $.each(patientAppointmentLoadJSON, function (i, item) {
                        if (item.AppointmentStatus != null && item.AppointmentStatus == "Cancel") {
                            $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet #listAppointmentData").append("<li  id=" + item.AppointmentId + " class='col-xs-3'> <div class='col-xs-4'> " + utility.RemoveTimeFromDate(null, item.ScheduleDate) + ' ' + item.ScheduleTimeFrom + " </div> <div class='col-xs-4'> " + item.CancellationReason + " </div><div class='col-xs-4'> " + item.ProviderName + " </div></li>");
                        }
                        else {
                            $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet #listAppointmentData").append("<li  id=" + item.AppointmentId + " class='col-xs-3'> <div class='col-xs-4'> " + utility.RemoveTimeFromDate(null, item.ScheduleDate) + ' ' + item.ScheduleTimeFrom + " </div> <div class='col-xs-4'> " + item.SchReason + " </div><div class='col-xs-4'> " + item.ProviderName + " </div></li>");
                        }
                        
                    });

                }
                else {
                    $("#listAppointmentData").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Appointment Found.</div></li>");
                }
            }

            else if (item.ComponentName.toLowerCase().indexOf("complaints") > -1) {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divComplaints";
                $("#lstComplaints").empty();
                $("#lstComplaints").append("<li><div class='col-xs-8'>Complaints</div><div class='col-xs-4'>VisitDate</div> </li>");
                if (complaintsLoadJSON.length > 0) {
                    $.each(complaintsLoadJSON, function (i, item) {

                        var visitDate = item.VisitDate;
                        var d = new Date(visitDate);
                        var convertedDate = visitDate == "" ? "" : $.datepicker.formatDate('mm/dd/yy ', new Date(d));
                        if (item.IsChiefComplaint.toLowerCase() == "true") {
                            $("#lstComplaints").append("<li class='col-xs-3'> <div class='col-xs-8'  data-toggle='tooltip' data-placement='left' title='" + item.ComplaintDescription + " (CC)' > <div class='size100per ellipses'> " + item.ComplaintDescription + " (CC) </div></div> <div class='col-xs-4'> " + convertedDate + " </div> </li>");

                        }
                        else {
                            $("#lstComplaints").append("<li class='col-xs-3'> <div class='col-xs-8'  data-toggle='tooltip' data-placement='left' title='" + item.ComplaintDescription + "' > <div class='size100per ellipses'> " + item.ComplaintDescription + " </div></div> <div class='col-xs-4'> " + convertedDate + " </div> </li>");
                        }

                    });
                }
                else {
                    $("#lstComplaints").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Complaints Found</div></li>");
                }
            }
            else if (item.ComponentName.toLowerCase().indexOf("patient document") > -1) {

                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divPatientDocument";
                $("#lstPatientDocument").empty();
                $("#lstPatientDocument").append("<li><div class='col-xs-8'>Name</div><div class='col-xs-4'>Upload Date</div> </li>");
                if (PatientDocumentLoadJSON.length > 0) {
                    $.each(PatientDocumentLoadJSON, function (i, item) {
                        var UploadDate = item.UploadDate;
                        var d = new Date(UploadDate);
                        var convertedDate = UploadDate == "" ? "" : $.datepicker.formatDate('mm/dd/yy ', new Date(d));
                        $("#lstPatientDocument").append("<li class='col-xs-3'> <div class='col-xs-8'  data-toggle='tooltip' data-placement='left' title='" + item.Name + "' > <div class='size100per ellipses'> " + item.Name + " </div></div> <div class='col-xs-4'> " + convertedDate + " </div> </li>");
                    });
                }
                else {
                    $("#lstPatientDocument").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>No Patient Document Found.</div></li>");
                }
            }

            else if (item.ComponentName.toLowerCase() == "implantable devices") {
                Clinical_FaceSheet.Components[i] = item.ComponentName;
                currentCompDivId = "div#divImplantable";
                $("#listImplantableData").empty();
                $("#listImplantableData").append("<li> <div class='col-xs-4'>Device Name</div> <div class='col-xs-3'>Device ID</div><div class='col-xs-2'>Status</div> <div class='col-xs-2'>Expiry</div></li>");
                if (implantableDevicesLoadJSON.length > 0) {
                    $.each(implantableDevicesLoadJSON, function (i, item) {
                        $("#listImplantableData").append("<li class='col-xs-3'> <div class='col-xs-4' data-toggle='tooltip' data-placement='left' title='" + item.GMDNPName + "'> <div class='size100per ellipses'>" + item.GMDNPName + " </div></div> <div class='col-xs-3' data-toggle='tooltip' data-placement='left' title='" + item.DI + "'> <div class='size100per ellipses'>" + item.DI + " </div></div><div class='col-xs-2'> " + item.Status + " </div><div class='col-xs-2'> " + item.Expiration_Date + " </div> </li>");
                    });
                }
                else {
                    $("#listImplantableData").append("<li class='col-xs-12'> <div class='col-xs-12 noWordBreak text-center'>Patient has no implantable device.</div></li>");
                }
            }


            var NewChild = $(currentCompDivId).clone();
            $(NewChild).css("display", "");
            $(NewChild).attr("componentName", item.ComponentName);
            $(NewChild).attr("FaceSheetId", item.FaceSheetId);
            $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet div#divUserFaceSheet").append($(NewChild)[0]);
            $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
        });
        //var imScroll = false;
        //$(".gridStriped").scroll(function () {
        //    imScroll = true;
        //});

        $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet div#divUserFaceSheet").sortable({
            connectWith: '#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet div#divUserFaceSheet",
            placeholder: "ui-state-highlight",
            // cancel: ".gridStriped",
            //handle: ':not(.gridStriped)',
            handle: 'header',//Add handle option For the fixes related to QAC2-518
            start: function (event) {
                var content = $(this);//.children('.FaceSheet_Drop');
                //event.originalEvent;
                // console.log( event.originalEvent);
                // if we're scrolling, don't start and cancel drag
                //if (event.originalEvent.pageX-content.offset().left > content.innerWidth())
                //{
                //    console.log('should-cancel');
                //    $(this).trigger("mouseup");
                //    return false;
                //}
            },
            stop: function (event, ui) {
                //if (imScroll == false) {
                setTimeout(function () {
                    var sortedIdsInOrder = $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet div#divUserFaceSheet").sortable("toArray");

                    var FaceSheetCompnentSorted = []
                    $.each(sortedIdsInOrder, function (index, element) {
                        var ComponentID = $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet div#divUserFaceSheet div#" + element).attr("facesheetid");
                        FaceSheetCompnentSorted.push(ComponentID);
                    });


                    Clinical_FaceSheet.updateComponentOrderSorting(FaceSheetCompnentSorted.join(','));
                }
                 , 5);
                //}
                //imScroll = false;
            }
        });

       Clinical_FaceSheet.SetToolTipOnAppointments();
    },

    GetBMIHtml: function (BMI) {
        var bmiSoap = '';
        if (parseFloat(BMI) >= 25.00) {
            bmiSoap = '<span style=color:red>' + BMI + '</span>';
        }
        else
        {
            bmiSoap = '<span style=color:black>' + BMI + '</span>';
        }

        return bmiSoap;
    },   

    loadFaceSheet: function (checkCDS) {
        var cdsAlert = true;
        checkCDS == false ? cdsAlert = false : cdsAlert = true;
        IsBackgroundLoaderShow = false;
        Clinical_FaceSheet.fillFaceSheetNew("LoadFaceSheetAll").done(function (response) {
            var response = JSON.parse(response);
            if (response.status == true) {
                if (Clinical_FaceSheet.params.ParentCtrl == "demographicDetail") {
                    $('#' + Clinical_FaceSheet.params.PanelID + " #btnClose").removeClass('hidden');
                    $('#' + Clinical_FaceSheet.params.PanelID + " #btnExportCCDA").addClass('hidden');
                    $('#' + Clinical_FaceSheet.params.PanelID + " #btnExportCaseReport").addClass('hidden');
                }

                var FaceSheetCount = JSON.parse(response.FaceSheetLoad_JSON);
                $('#' + Clinical_FaceSheet.params.PanelID + " #btnPrintFaceSheet").show();
                if (FaceSheetCount.FaceSheetCount == 16) {
                    Clinical_FaceSheet.bindFaceSheet(response);
                    $(" #mainForm  li#CDSAlert").show();
                    var triggerLocation = 'FaceSheet';
                    if (cdsAlert == true && Clinical_FaceSheet.params.ParentCtrl != "demographicDetail") {
                        IsBackgroundLoaderShow = false;
                        ClinicalCDSDetail.showCDSAlert(triggerLocation, 0);
                        IsBackgroundLoaderShow = true;
                    }
                }
                else {
                    Clinical_FaceSheet.saveFaceSheet().done(function (response) {
                        var response = JSON.parse(response)
                        if (response.status == true) {
                            Clinical_FaceSheet.loadFaceSheet();
                        }
                        else {
                            utility.DisplayMessages(response.Message, 2);
                            $('#' + Clinical_FaceSheet.params.PanelID + " #btnPrintFaceSheet").hide();
                        }
                    });
                }
                Clinical_FaceSheet.disableClinicalSummary();
            }
            else {
                utility.DisplayMessages(response.Message, 2);
                $('#' + Clinical_FaceSheet.params.PanelID + " #btnPrintFaceSheet").hide();
            }
        });
        IsBackgroundLoaderShow = true;
    },

    SetToolTipOnAppointments: function () {
       
        $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet #listAppointmentData").find("li").each(function (index, item) {
            if ($(item).attr("id")) {

                $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet #listAppointmentData li#" + item.id).kendoTooltip({
                            // filter: ".k-event:not(.k-event-drag-hint) > div .PatientNameTooltip, .k-task",
                            position: "right",
                            width: 425,
                            autoHide: true,
                            animation: false,
                            content: kendo.template($('#tooltipTemplateInClinicalFaceSheet').html())
                        });
                      
                       
                    }

                });
        
        
    },
    //Author: Muhammad Arshad
    //Date: 12-10-2015
    //This function will handle load of Face Sheet
    //It represents service call to API

    fillFaceSheetNew: function (actionMethod) {
        var objData = new Object();
        objData["PatientId"] = Clinical_FaceSheet.params.ParentCtrl == "demographicDetail" ? Clinical_FaceSheet.params.patientID : $("div#PatientProfile #hfPatientId").val();
        //objData["commandType"] = component;
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FaceSheet", actionMethod);
    },


    //Author: Muhammad Arshad
    //Date: 12-15-2015
    //This function will handle save of Face Sheet
    //It represents service call to API
    saveFaceSheet: function () {
        var objData = new Object();

        objData["PatientId"] = $('#' + Clinical_FaceSheet.params.PanelID + " #frmClinicalFaceSheet #hfPatientId").val();

        objData["UserId"] = globalAppdata['AppUserId'];;
        objData["commandType"] = "Save_FaceSheet";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FaceSheet", "FaceSheet");
    },

    updateFaceSheetOrderSorting_Dbcall: function (FaceSheetCompnentSorted) {
        var objData = new Object();
        //objData["FaceSheetId"] = "1";
        //objData["ComponentName"] = "qsd";
        //objData["ComponentOrder"] = "1";
        //objData["RecordCount"] = "4";
        objData["FaceSheetComponentSorted"] = FaceSheetCompnentSorted;
        objData["UserId"] = globalAppdata['AppUserId'];
        objData["commandType"] = "UPDATE_COMPONENTORDERSorting";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FaceSheet", "FaceSheet");
    },
    //Author: Muhammad Arshad
    //Date: 12-10-2015
    //This function will handle Unload of FaceSheet
    UnLoadTab: function () {
        if (Clinical_FaceSheet.params["FromAdmin"] == "0") {
            if (Clinical_FaceSheet.params != null && Clinical_FaceSheet.params.ParentCtrl != null && Clinical_FaceSheet.params.ParentCtrl != "demographicDetail") {
                UnloadActionPan(Clinical_FaceSheet.params.ParentCtrl, 'Clinical_FaceSheet');
            }
            else
                UnloadActionPan(null, 'Clinical_FaceSheet');
        }
        else {
            //$("#mstrDivMedical #clinicalMenu_Medical_Vitals").remove();
            if (Clinical_FaceSheet.params.ParentCtrl != "demographicDetail") {
                RemoveAdminTab();
            } else if (Clinical_FaceSheet.params.ParentCtrl == "demographicDetail") {
                UnloadActionPan(Clinical_FaceSheet.params.ParentCtrl, 'Clinical_FaceSheet');
            }

        }
    },

    /**
    Author: Muhammad Irfan
    Date: 04/01/2016
    Overview: This function will load action pan of padf view for facesheet
    **/
    PrintFaceSheet: function () {
        var parentPanelId = null;
        var params = [];
        params["FromAdmin"] = "0";
        params["UserId"] = globalAppdata['AppUserId'];
        params["PatientId"] = Clinical_FaceSheet.params.ParentCtrl == "demographicDetail" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        params["ParentCtrl"] = Clinical_FaceSheet.params.ParentCtrl == "demographicDetail" ? "Clinical_FaceSheet" : Clinical_FaceSheet.params.TabID;//'mstrTabDashBoard';.


        if (params["ParentCtrl"] == "Clinical_FaceSheet") {
            parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
            LoadActionPan('Clinical_FaceSheetView', params, parentPanelId);
        }
        LoadActionPan('Clinical_FaceSheetView', params);
    },
    //Start//06/01/2016//Abid Ali//Mehtod to empty activePopedUpComponentsPagerdiv array
    EmptyFaceSheetModels: function () {

        // Added By Abid Ali
        var lastElement = Clinical_FaceSheet.FaceSheetModels.length - 1;
        if (lastElement > -1) {
            Clinical_FaceSheet.FaceSheetModels.splice(lastElement, 1);
        }
    },
    LoadToolTipData: function (AppointmentId) {
        
        var TooltipDetail = "";
        var response = Clinical_FaceSheet.ToolTipDataLoad(AppointmentId);//.done(function (response) {
        var Model = JSON.parse(response);
        var ResponseDetail = Model.ResponseModel;
        ResponseDetail = JSON.parse(ResponseDetail);
        var AppointmentDetail = new Object();
        if (ResponseDetail.status != false) {
            TooltipDetail = JSON.parse(ResponseDetail.ToolTipDataFill_JSON);

            var patientAge = "";
            if (TooltipDetail.PatientAge) {

                patientAge = TooltipDetail.PatientAge.split(',');

                if (parseInt((TooltipDetail.PatientAge.split(',')[0]).split(' ')[1]) > 0) {

                    patientAge = TooltipDetail.PatientAge.split(',')[0]; //age in years
                } else if (parseInt((TooltipDetail.PatientAge.split(',')[1]).split(' ')[1]) > 0) {
                    patientAge = TooltipDetail.PatientAge.split(',')[1]; //age in months
                } else {
                    patientAge = TooltipDetail.PatientAge.split(',')[2]; //age in days

                }


            }
            else {
                patientAge = TooltipDetail.Age + ' Year(s)';
            }

            var reminder_delivery_status = TooltipDetail.Status == (undefined) ? "" : TooltipDetail.Status;
            var reminder_delivery_response = TooltipDetail.KeyPress == (undefined) ? "" : TooltipDetail.KeyPress;
            var reminder_delivery_response_message = TooltipDetail.ResponseMessage == (undefined) ? "" : TooltipDetail.ResponseMessage;
            var status_style_color = "";

            if (reminder_delivery_status != "") {
                reminder_delivery_status = reminder_delivery_status.toLocaleLowerCase() == 'new' ? 'Waiting' : reminder_delivery_status;
                reminder_delivery_status = reminder_delivery_status.toLocaleLowerCase() == 'failed' ? 'Not Delivered' : reminder_delivery_status;
                reminder_delivery_status = reminder_delivery_status.toLocaleLowerCase() == 'made' ? 'Successfully Delivered' : reminder_delivery_status;
            }

            if (reminder_delivery_response != "") {
                reminder_delivery_response = reminder_delivery_response.toLocaleLowerCase() == '1' ? 'Confirm' : reminder_delivery_response;
                reminder_delivery_response = reminder_delivery_response.toLocaleLowerCase() == '2' ? 'Cancel' : reminder_delivery_response;
                //reminder_delivery_response = reminder_delivery_response.toLocaleLowerCase() == '0' ? 'Invalid Key Press' : reminder_delivery_response;
                if (reminder_delivery_response.toLocaleLowerCase() == '3' || reminder_delivery_response.toLocaleLowerCase() == '4' || reminder_delivery_response.toLocaleLowerCase() == '5' || reminder_delivery_response.toLocaleLowerCase() == '6' || reminder_delivery_response.toLocaleLowerCase() == '7' || reminder_delivery_response.toLocaleLowerCase() == '8' || reminder_delivery_response.toLocaleLowerCase() == '9') {
                    reminder_delivery_response = 'Invalid Key Press';
                }
                if (reminder_delivery_response.toLocaleLowerCase() == '0') {
                    reminder_delivery_response = 'No Response';
                }
            }

            if (reminder_delivery_response_message != "" && reminder_delivery_response_message.indexOf("answering") >= 0)
                reminder_delivery_response = "Message left on answering machine.";

            if (reminder_delivery_response.toLocaleLowerCase() == "confirm") {
                status_style_color = "green";
            }


            AppointmentDetail["Age"] = patientAge;
            AppointmentDetail["Gender"] = TooltipDetail.Gender;
            AppointmentDetail["AccountNumber"] = TooltipDetail.AccountNumber;
            AppointmentDetail["DOB"] = TooltipDetail.DOB;
            AppointmentDetail["CellNo"] = TooltipDetail.CellNo;
            AppointmentDetail["EmailAddress"] = TooltipDetail.EmailAddress;
            AppointmentDetail["Duration"] = TooltipDetail.Duration;
            AppointmentDetail["PrimaryInsuranceName"] = TooltipDetail.PrimaryInsuranceName;
           
            AppointmentDetail["ReminderDeliveryStatus"] = reminder_delivery_status;
            AppointmentDetail["ReminderDeliveryResponse"] = reminder_delivery_response;
          
            AppointmentDetail["StatusStyleColor"] = status_style_color;
            AppointmentDetail["ScheduleDate"] = TooltipDetail.ScheduleDate;
            AppointmentDetail["ScheduleProvider"] = TooltipDetail.ScheduleProvider;
            AppointmentDetail["ScheduleFacility"] = TooltipDetail.ScheduleFacility;
            AppointmentDetail["CancellationReason"] = TooltipDetail.CancellationReason;

            if (TooltipDetail.RescheduleDate) {
                AppointmentDetail["RescheduleColor"] = "#000000";
                AppointmentDetail["RescheduleDate"] = TooltipDetail.RescheduleDate ? TooltipDetail.RescheduleDate : "";
                AppointmentDetail["RescheduleProvider"] = TooltipDetail.RescheduleProvider ? TooltipDetail.RescheduleProvider : "";
                AppointmentDetail["RescheduleFacility"] = TooltipDetail.RescheduleFacility ? TooltipDetail.RescheduleFacility : "";
                AppointmentDetail["RescheduleText"] = "Rescheduled From:";

            }
            else if (TooltipDetail.ScheduleDate) {
                AppointmentDetail["RescheduleColor"] = "#000000";
                AppointmentDetail["RescheduleDate"] = TooltipDetail.ScheduleDate ? TooltipDetail.ScheduleDate : "";
                AppointmentDetail["RescheduleProvider"] = TooltipDetail.RescheduleProvider ? TooltipDetail.RescheduleProvider : "";
                AppointmentDetail["RescheduleFacility"] = TooltipDetail.RescheduleFacility ? TooltipDetail.RescheduleFacility : "";
                AppointmentDetail["RescheduleText"] = "Rescheduled To:";
            }
            else {
                AppointmentDetail["RescheduleColor"] = "#F5F5F5";
                AppointmentDetail["RescheduleDate"] = "";
                AppointmentDetail["RescheduleProvider"] = "";
                AppointmentDetail["RescheduleFacility"] = "";
                AppointmentDetail["RescheduleText"] = "Rescheduled From:";


            }
            AppointmentDetail["CopayBal"] = TooltipDetail.CopayBal;
            if (TooltipDetail.AppointmentStatus == "Cancel") {
                AppointmentDetail["ReasonComments"] = TooltipDetail.CancellationReason;
            }
            else {
                if (TooltipDetail.ReasonComments)
                    AppointmentDetail["ReasonComments"] = TooltipDetail.ReasonComments.replace(/#@#/g, ',');
                else
                    AppointmentDetail["ReasonComments"] = "";
            }

            if (TooltipDetail.imgPatient)
                AppointmentDetail["imgPatient"] = TooltipDetail.imgPatient;
            else {
                if (TooltipDetail.Gender == "Male")
                    AppointmentDetail["imgPatient"] = "../../Content/images/default_male_profile.gif";
                else
                    AppointmentDetail["imgPatient"] = "../../Content/images/default_female_profile.gif";

            }
            AppointmentDetail["ProviderName"] = TooltipDetail.ProviderName;
            AppointmentDetail["FacilityName"] = TooltipDetail.FacilityName;
            AppointmentDetail["Comments"] = TooltipDetail.Comments;
            AppointmentDetail["CopayClass"] = TooltipDetail.CopayClass;
            AppointmentDetail["AppointmentStatus"] = TooltipDetail.AppointmentStatus;
            AppointmentDetail["VisitTypeName"] = TooltipDetail.VisitTypeName;
            AppointmentDetail["PatientType"] = TooltipDetail.PatientType;
            AppointmentDetail["StatusColor"] = TooltipDetail.StatusColor;
            AppointmentDetail["VisitTypeColor"] = TooltipDetail.VisitTypeColor;
            AppointmentDetail["PatientName"] = TooltipDetail.PatientName;

// Ast-188
            if (TooltipDetail.ModifiedOn == TooltipDetail.CreatedOn) {
                AppointmentDetail["ModifiedOn"] = "";
                AppointmentDetail["ModifiedBy"] = "";
            }
            else {
                AppointmentDetail["ModifiedOn"] = TooltipDetail.ModifiedOn;
                AppointmentDetail["ModifiedBy"] = TooltipDetail.ModifiedBy;
            }
            AppointmentDetail["CreatedBy"] = TooltipDetail.CreatedBy;

            AppointmentDetail["CreatedOn"] = TooltipDetail.CreatedOn;

            return AppointmentDetail;
        }
        else {

            return AppointmentDetail;
        }


    },
    ToolTipDataLoad: function (AppointmentId) {
        var objData = new Object();
        objData["CommandType"] = "fill_tooltip_data";
        objData["AppointmentId"] = AppointmentId;
        var data = JSON.stringify(objData);
        return MDVisionService.APIServiceSyncCall(data, "Scheduler", "PMSScheduler");

    },
    //End//06/01/2016//Abid Ali//Mehtod to empty activePopedUpComponentsPagerdiv array
}

