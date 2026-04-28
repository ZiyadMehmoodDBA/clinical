/*
    Author: Khaleel Ur Rehman + ZeeshanAK
    Creation Date: 14-01-2016
    OverView:This File Is created for Clinical Medications
*/

Clinical_Medications = {
    params: [],
    startUpScreen: "manage_medications",
    EditableGrid: null,
    myGrid: null,
    medIdsForReconciledView: "",
    lastVisitReconciledIds: "",
    isViewed: false,
    controlToInvoke: null,
    Load: function (params) {
        //Begin 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
        Clinical_Medications.startUpScreen = "manage_medications";
        if (Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet") {
            Clinical_Medications.params.patientID = Clinical_FaceSheet.params.patientID;
        }
        //End 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
        Clinical_Medications.params = params;
        if (Clinical_Medications.params.PanelID != 'pnlClinicalMedications') {
            Clinical_Medications.params.PanelID = Clinical_Medications.params.PanelID + ' #pnlClinicalMedications';
        } else {
            Clinical_Medications.params.PanelID = 'pnlClinicalMedications';
        }
        if (Clinical_Medications.bIsFirstLoad) {
            Clinical_Medications.bIsFirstLoad = false;

        }
        if (Clinical_Medications.params.ParentCtrl == 'clinicalTabProgressNote') {

            $("#" + Clinical_Medications.params.PanelID + " #btnPrint").addClass('hidden');
            $("#" + Clinical_Medications.params.PanelID + " #btnPrintNote").show();
            $("#" + Clinical_Medications.params.PanelID + " #btnAddMedications").show();
            $("#" + Clinical_Medications.params.PanelID + " #btnMedicationReset").show();
            $("#" + Clinical_Medications.params.PanelID + " #activatePrescriptionButtonForNotes").show();
            $("#" + Clinical_Medications.params.PanelID + " #btnPrescriptionsReset").show();
        }
        else {
            $("#" + Clinical_Medications.params.PanelID + " #btnPrintNote").hide();
            $("#" + Clinical_Medications.params.PanelID + " #btnAddMedications").hide();
            $("#" + Clinical_Medications.params.PanelID + " #btnMedicationReset").hide();
            $("#" + Clinical_Medications.params.PanelID + " #activatePrescriptionButtonForNotes").hide();
            $("#" + Clinical_Medications.params.PanelID + " #btnPrescriptionsReset").hide();
            $("#" + Clinical_Medications.params.PanelID + ' #divchkReconciledViewOnNote').hide();
        }
        if (params["MedicationsTab"] == "Prescription") {

        }
        else {
            if (Clinical_Medications.params.ParentCtrl == 'clinicalTabProgressNote') {
                $("#" + Clinical_Medications.params.PanelID + ' #divchkReconciledViewOnNote').show();
            }
        }
        //fix EMR-3639
        Clinical_Prescriptions.prescriptionSearch();
        Clinical_Medications.medicationsSearch();
        //fix EMR-3639
        if ($('#PatientProfile #hfPatientId').val() != "") {
            Clinical_Medications.params.patientID = Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }
        if (Clinical_Medications.params.ParentCtrl != "clinicalTabProgressNote") {
            if (globalAppdata.DefaultTabMedications == 'Prescriptions')
                Clinical_Medications.showPrescriptionTab();
            else
                Clinical_Medications.showMedicationTab();
        } else {
            if (Clinical_Medications.params.MedicationsTab == 'Prescription') {
                Clinical_Medications.showPrescriptionTab();
            } else {
                Clinical_Medications.showMedicationTab();
            }
        }
        /*
          Change Implement BY: Muhammad Azhar Shahzad
          Reason:To Show navigation on Progress Note
          Created Date: Januaray 15, 2016
      */
        //Code for progress note navigation
        if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
            if (Clinical_Medications.params.MedicationsTab == 'Prescription') {
                EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Medications.params.PanelID, 'Medical', 'Prescription', 'Clinical_Medications.unLoadTab(true);', null, true);
            } else {
                EMRUtility.appendPrevNext_NotesComponent_Btns(Clinical_Medications.params.PanelID, 'Medical', 'Medications', 'Clinical_Medications.unLoadTab(true);', null, true);
            }

        }

        if (Clinical_Medications.params.ParentCtrl == "clinicalTabFaceSheet") {
            EMRUtility.MakeFaceSheetPager('#' + Clinical_Medications.params.PanelID + " div#FaceSheetPager", Clinical_Medications.params.FaceSheetComponents, 'medications');
        } else if (Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet") {
            EMRUtility.MakeFaceSheetPager('#pnlClinicalFaceSheet #pnlClinicalMedications' + " div#FaceSheetPager", Clinical_Medications.params.FaceSheetComponents, 'medications');
        }

        utility.callbackAfterAllDOMLoaded(function () {

            var faceSheetpager = $('#FaceSheetPager');
            if (faceSheetpager.length > 0) {
                //show/hide button controls acording to resoltion
                EMRUtility.HideShowFaceSheetPagerBtnControls(faceSheetpager);
                $("#FaceSheetPager").find("div.slick-track").css("width", "1356px");
            }
        });

        //Load MU3 Alerts
        utility.LoadMUAlerts(Clinical_Medications.params.patientID, true);
    },

    medReconciledOnNote: function () {
        var bMedReconciledOnNote = $("#pnlClinicalProgressNote").find("#hfbMedReconciled").val();
        var chkReconciled = $("#" + Clinical_Medications.params.PanelID + ' #divchkReconciledViewOnNote #chkReconciledViewOnNote');
        if (bMedReconciledOnNote == "1") {
            chkReconciled.prop("checked", false);
            chkReconciled.trigger("click");
        }
        else {
            chkReconciled.prop("checked", false);
        }
    },

    ShowHideReconciledViewOnNote: function (value) {
        if (value == 'Med') {
            if (Clinical_Medications.params.ParentCtrl == 'clinicalTabProgressNote') {
                $("#" + Clinical_Medications.params.PanelID + ' #divchkReconciledViewOnNote').show();
            }
            else {
                $("#" + Clinical_Medications.params.PanelID + " #btnPrintNote").hide();
                $("#" + Clinical_Medications.params.PanelID + ' #divchkReconciledViewOnNote').hide();
            }

            Clinical_Medications.showMedicationTab();
            Clinical_Medications.medicationsSearch();

        } else {
            $("#" + Clinical_Medications.params.PanelID + " #btnPrintNote").hide();
            $("#" + Clinical_Medications.params.PanelID + ' #divchkReconciledViewOnNote').hide();
            Clinical_Medications.showPrescriptionTab();
            Clinical_Prescriptions.prescriptionSearch();

        }
    },

    showMedicationTab: function () {
        $("#" + Clinical_Medications.params.PanelID + ' #listMedications').tab("show");
        $("#" + Clinical_Medications.params.PanelID + ' #Medications').addClass("active");
        $("#" + Clinical_Medications.params.PanelID + ' #Prescriptions').removeClass('active');
        Clinical_Medications.params.MedicationsTab = 'Medications';
        $("#" + Clinical_Medications.params.PanelID + ' #btnOpenDrFirst').html('<i class="fa fa-plus-circle"></i> Manage');
        //Begin 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
        Clinical_Medications.startUpScreen = "manage_medications";
        //End 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
        $("#" + Clinical_Medications.params.PanelID + ' #btnPrintMedPres').attr("onClick", "Clinical_Medications.PrintClinicalMedications()");
    },

    showPrescriptionTab: function () {
        $("#" + Clinical_Medications.params.PanelID + ' #divchkReconciledViewOnNote').hide();
        $("#" + Clinical_Medications.params.PanelID + ' #listPrescriptions').tab("show");
        $("#" + Clinical_Medications.params.PanelID + ' #Prescriptions').addClass("active");
        $("#" + Clinical_Medications.params.PanelID + ' #Medications').removeClass('active');
        Clinical_Medications.params.MedicationsTab = 'Prescription';
        $("#" + Clinical_Medications.params.PanelID + ' #btnOpenDrFirst').html('<i class="fa fa-plus-circle"></i> Prescribe');
        //Begin 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
        Clinical_Medications.startUpScreen = "patient";
        //End 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
        $("#" + Clinical_Medications.params.PanelID + ' #btnPrintMedPres').attr("onClick", "Clinical_Prescriptions.PrintClinicalPrescription()");
    },

    showMedicationPrescriptionTab: function (MedicationsTab) {
        if (MedicationsTab == 'Prescription') {
            $("#" + Clinical_Medications.params.PanelID + ' #listPrescriptions').tab("show");
            $("#" + Clinical_Medications.params.PanelID + ' #Prescriptions').addClass("active");
            $("#" + Clinical_Medications.params.PanelID + ' #Medications').removeClass('active');
            Clinical_Medications.params.MedicationsTab = 'Prescription';
            $("#" + Clinical_Medications.params.PanelID + ' #btnOpenDrFirst').html('<i class="fa fa-plus-circle"></i> Prescribe');
            //Begin 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
            Clinical_Medications.startUpScreen = "patient";
            //End 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
            $("#" + Clinical_Medications.params.PanelID + ' #btnPrintMedPres').attr("onClick", "Clinical_Prescriptions.PrintClinicalPrescription()");

        } else {
            $("#" + Clinical_Medications.params.PanelID + ' #listMedications').tab("show");
            $("#" + Clinical_Medications.params.PanelID + ' #Medications').addClass("active");
            $("#" + Clinical_Medications.params.PanelID + ' #Prescriptions').removeClass('active');
            Clinical_Medications.params.MedicationsTab = 'Medications';
            $("#" + Clinical_Medications.params.PanelID + ' #btnOpenDrFirst').html('<i class="fa fa-plus-circle"></i> Manage');
            //Begin 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
            Clinical_Medications.startUpScreen = "manage_medications";
            //End 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
            $("#" + Clinical_Medications.params.PanelID + ' #btnPrintMedPres').attr("onClick", "Clinical_Medications.PrintClinicalMedications()");

        }
    },
    domReadyFuncMedications: function () {
        (function ($) {
            'use strict';
            $(function () {
                $('#' + Clinical_Medications.params.PanelID + ' [data-plugin-ios-switch]').each(function () {
                    var $this = $(this);
                    $this.themePluginIOS7Switch();
                });
            });
        }).apply(this, [jQuery]);

        $("#" + Clinical_Medications.params.PanelID + ' #ulSocialHxTabsItems li').on('click', function (item, ev) {
            if (this.id == 'listMedications') {
                $("#" + Clinical_Medications.params.PanelID + ' #btnOpenDrFirst').html('<i class="fa fa-plus-circle"></i> Manage');
                //Begin 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
                Clinical_Medications.startUpScreen = "manage_medications";
                //End 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
            } else {
                $("#" + Clinical_Medications.params.PanelID + ' #btnOpenDrFirst').html('<i class="fa fa-plus-circle"></i> Prescribe');
                //Begin 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
                Clinical_Medications.startUpScreen = "patient";
                //End 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
            }
        });
    },

    medicationsSearch: function (medicationId, PageNo, rpp) {
        var dfd = $.Deferred();
        /*
         Change Implement BY: Muhammad Azhar Shahzad
         Reason:Adding selection column of checkbox of Medications for Progress Notes
         Created Date: Januaray 15, 2016
     */
        if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
            $("#" + Clinical_Medications.params.PanelID + " #btnPrint").hide();
            $("#" + Clinical_Medications.params.PanelID + " #btnPrint").addClass('hidden');
            $("#" + Clinical_Medications.params.PanelID + " #btnPrintNote").hide();
            $("#" + Clinical_Medications.params.PanelID + " #btnPrintNote").addClass('hidden');
            if ($("#" + Clinical_Medications.params.PanelID + " #dgvMedications thead tr #selectRecordMedications").length == 0) {
                $("#" + Clinical_Medications.params.PanelID + " #dgvMedications thead tr").prepend('<th style="width:50px"><input type="checkbox" onchange="Clinical_Medications.checkUncheckAllMeds(this);" controlname="selectRecordMedications" id="selectRecordMedications" name="chkHeader" class="input-block" coltype="checkbox"/></th>');
            }
        } else {
            $("#" + Clinical_Medications.params.PanelID + " #dgvMedications th#selectRecordMedications").remove();
        }
        if (globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "true")
            if ($("#" + Clinical_Medications.params.PanelID + " #dgvMedications thead tr th#ddlRoute").length == 0)
                $("#" + Clinical_Medications.params.PanelID + " #dgvMedications thead tr th:last").before('<th id="ddlRoute" class="size140 size-min140">Route</th>');

            
        //end change  Januaray 15, 2016
        if ($('#' + Clinical_Medications.params.PanelID + " #pnlMedications_Result").css("display") == "none") {
            $('#' + Clinical_Medications.params.PanelID + " #pnlMedications_Result").show();
        }
        var self = $('#' + Clinical_Medications.params.PanelID);
        var myJSON = self.getMyJSON();
        Clinical_Medications.searchMedications(myJSON, medicationId, PageNo, rpp).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Medications.isViewed = false;
                $.when(Clinical_Medications.medicationsGridLoad(response)).then(function () {
                    dfd.resolve();
                });
                var TableControl = Clinical_Medications.params.PanelID + " #dgvMedications";
                var PagingPanelControlID = Clinical_Medications.params.PanelID + " #divMedications_Paging";
                var ClassControlName = "Clinical_Medications";
                var PagesToDisplay = 5;
                var iTotalDisplayRecords = response.iTotalDisplayRecords;
                setTimeout(CreatePagination(response.MedicationCount, PageNo, rpp, PagingPanelControlID, TableControl, ClassControlName, PagesToDisplay, iTotalDisplayRecords, function (PrimaryID, PageNumber, ResultPerPage) {
                    Clinical_Medications.medicationsSearch(PrimaryID, PageNumber, ResultPerPage);
                }), 10);
                if (response.MedicationReviewSoap_JSON != null) {
                    Clinical_Medications.insertReviewedInfoOnTop(response.MedicationReviewSoap_JSON);
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            //Clinical_Medications.showMedicationPrescriptionTab(Clinical_Medications.params.MedicationsTab);//krnew
        });
        return dfd;
    },
    searchMedications: function (medicationsData, medicationId, PageNumber, RowsPerPage) {
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 15;
        }
        var objData = JSON.parse(medicationsData);
        var IsCurrent = null;
        IsCurrent = $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive');

        if (IsCurrent == null || IsCurrent == "1") {
            IsCurrent = true;
            $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive', 1);
        } else {
            IsCurrent = false;
            $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive', 0);
        }
        if ($('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').is(':checked') || $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').length == 0) {
            IsCurrent = true;
            $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive', 1);
        } else {
            IsCurrent = false;
            $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive', 0);
        }
        objData["MedicationID"] = medicationId;
        objData["PageNumber"] = PageNumber;
        objData["RowsPerPage"] = RowsPerPage;
        if (Clinical_Medications.params.patientID != null) {
            objData["PatientId"] = Clinical_Medications.params.patientID;
        }
        else {
            objData["PatientId"] = Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }
        objData["isCurrent"] = IsCurrent;
        objData["commandType"] = "SEARCH_MEDICATIONS";
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        //Start 26-10-2016 Humaira Yousaf for logging of view action
        objData["isViewed"] = Clinical_Medications.isViewed;
        //End 26-10-2016 Humaira Yousaf for logging of view action
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Medications");
    },

    PrintClinicalMedications: function () {


        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Medical_Medications", "PRINT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                Clinical_Medications.searchAllMedications('{}', 0).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        var IsCurrent = $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive');
                        var title = "";
                        if (IsCurrent == null || IsCurrent == "1") {
                            title = "Current Medications";
                        } else {
                            title = "Past Medications";
                        }
                        var tbl = $('<table/>').append('<caption><b>' + title + '</b></caption><tr><th>Medication</th> <th>Dosage</th><th>Pharmacy</th><th>Refills</th><th>Provider</th><th>Prescribed On</th><th>Started On</th></tr>');
                        if (response.MedicationCount > 0) {


                            var MedicationJSONData = JSON.parse(response.MedicationLoad_JSON);
                            $.each(MedicationJSONData, function (i, item) {



                                var $row = $('<tr/>');
                                IsActive = 1;
                                activeTitle = "Inactive Record";
                                tglclass = "fa fa-toggle-on red";
                                var SelectionCheckBoxColumn = "";
                                var Checked = "";
                                Checked = " checked";

                                SelectionCheckBoxColumn = "";
                                var actions = '';

                                var DoseValue = "";
                                if (item.Action != "" || item.Dose != "" || item.DoseUnit != "" || item.Routeby != "" || item.DoseTiming != "" || item.DoseOther) {
                                    DoseValue = item.Action + " " + item.Dose + " " + item.DoseUnit + " " + item.Routeby + " " + item.DoseTiming + " " + item.DoseOther + " " + (item.PatientNotes != "" ? "(" + item.PatientNotes + ")" : "");
                                }
                                else {
                                    DoseValue = item.PatientNotes;
                                }
                                $row.append(SelectionCheckBoxColumn + '<td>' + item.MedicationName + '</td><td>' + DoseValue + '</td><td>' + item.PharmacyName + '</td><td>' + item.Refill + '</td><td>' + item.Provider + '</td><td>' + utility.RemoveTimeFromDate(null, item.PrescribedOn) + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td>');
                                $(tbl).last().append($row);
                            });
                            Clinical_Medications.PrintReport('<table class="table table-bordered table-striped table-hover mb-none table-condensed">' + $(tbl).html() + '</table>');
                        } else {

                            var IsCurrent = $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive');
                            var message = '';
                            if (IsCurrent == null || IsCurrent == "1") {
                                IsCurrent = true;
                                message = 'No Active Medications Found';
                            } else {
                                message = 'No InActive Medications Found';
                            }
                            var $row = $('<tr/>');
                            $row.append('<td valign="top" colspan="8" align="center" class="dataTables_empty">' + message + '</td>');
                            $(tbl).last().append($row);
                            Clinical_Medications.PrintReport('<table class="table table-bordered table-striped table-hover mb-none table-condensed">' + $(tbl).html() + '</table>');
                        }
                    } else {
                        utility.DisplayMessages(response.Message, 2);
                    }
                });

            }
            else {
                utility.DisplayMessages(strMessage, 2);
            }
        });

    },

    searchAllMedications: function (medicationsData, medicationId) {

        var objData = JSON.parse(medicationsData);
        var IsCurrent = null;
        IsCurrent = $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive');

        if (IsCurrent == null || IsCurrent == "1") {
            IsCurrent = true;
            $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive', 1);
        } else {
            IsCurrent = false;
            $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive', 0);
        }
        if ($('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').is(':checked') || $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').length == 0) {
            IsCurrent = true;
            $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive', 1);
        } else {
            IsCurrent = false;
            $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive', 0);
        }
        objData["MedicationID"] = medicationId;

        if (Clinical_Medications.params.patientID != null) {
            objData["PatientId"] = Clinical_Medications.params.patientID;
        }
        else {
            objData["PatientId"] = Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }
        objData["isCurrent"] = IsCurrent;
        objData["commandType"] = "SEARCH_ALL_MEDICATIONS";
        //Start 26-10-2016 Humaira Yousaf for logging of view action
        objData["isViewed"] = Clinical_Medications.isViewed;
        //End 26-10-2016 Humaira Yousaf for logging of view action
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Medications");
    },
    PrintReport: function (htmlContent) {

        var html = htmlContent;
        var patientId = Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();

        Clinical_Medications.getHeaderFooterInfo(patientId).done(function (response) {
            response = JSON.parse(response);

            if (response.status == true) {

                var HeaderLogo = response.HeaderLogo;
                var FooterText = response.FooterText;
                var PracticeText = response.PracticeText;

                if (PracticeText != null && PracticeText !== "" && FooterText != null && FooterText !== "") {
                    var patientData = "";
                    if (response.PatientText != 'undefined' && response.PatientText != null) {
                        patientData = response.PatientText;
                        patientData = patientData.replace("12:00AM", "");
                        patientData = a = patientData.split('<br/>');

                    }
                    var newPatientText = '';
                    for (var i = 0; i < patientData.length; i++) {
                        if ($.trim(patientData[i]) != '') {
                            newPatientText += '<li>' + patientData[i] + '</li>';
                        }
                    }
                    var providerData = "";
                    if (response.ProviderText != 'undefined' && response.ProviderText != null)
                        providerData = a = response.ProviderText.split('<br/>');
                    var newProviderText = '';
                    for (var i = 0; i < providerData.length; i++) {
                        if ($.trim(providerData[i]) != '') {
                            newProviderText += '<li align="right">' + providerData[i] + '</li>';
                        }
                    }

                    var formHeaderHtml = '<div id="printcall">' +
                               '<div id="PatientInfo">' +
                               '<div class="col-sm-4 col-lg-2 pull-left">' +
                               '<img src="' + HeaderLogo + '" class="img-responsive" height="100px" width="240px"></div>' +
                                    '<div class="col-sm-4 col-lg-2 pull-right">' +
                                    '<ul class="col-sm-4 col-lg-2  list-unstyled">' +
                                    '<li id="PatientPractice" align="right">' + response.PracticeText + '</li>' +
                                     '</div><div class="clearfix"></div>' +
                                      '<div class="col-sm-4 col-lg-2 pull-left">' +
                                    '<ul class="list-unstyled" align="left">' +
                                        newPatientText + '</ul></div>' +
                                     '<div class="col-sm-4 col-lg-2 pull-right">' +
                                     '<ul class="col-sm-4 col-lg-2  list-unstyled">' +
                                    //'<li id="PatientProvider" align="right">' +
                                    newProviderText + '</ul></div>' +
                                '<div class="clearfix"></div>' +

                                '<div class="splitter m-none mt-xs"><div class="spacer3"></div></div><div class="spacer3"></div><div class="spacer3"></div><div class="spacer3"></div><div class="spacer3"></div>' +
                                '</div><div class="spacer5"></div>' +
                                '<section class="">' +
                                '<div class=""  id="grdICDCPTPrint" style="font-size:12px;">' + html + '</div></section></div>' +
                                '<div class="spacer10"></div><hr /><h4 id="templateHeader"></h4><hr />';

                    var docType = '<!doctype html>';
                    var docCnt = formHeaderHtml;

                    var docHead = '<head><script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                 + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                 + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
                 + '</script>'
                 + '</head>';

                    var footer = ' <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">'
                  + '<div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>'
                + '<div id="ClinicalReportsFooterText" class="blueBgPrint" style="float:left; width:100%;padding:2px 5px 0 5px;height:22px;">'
                + '<span id="ClinicalReportsFooter">' + FooterText + '</span>'
                 + '</div> </div>';

                    if (utility.UserBrowser() == "IE") {
                        Clinical_Medications.printPatientMedicationInfo(docType, docHead, docCnt , footer);
                    } else {
                        var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=1000, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                        var newWin = window.open("", "_blank", winAttr);
                        writeDoc = newWin.document;
                        writeDoc.open();
                        writeDoc.write(docType + '<html>' + docHead + '<body style="font-size:30px;">' + docCnt + footer + '</body></html>');
                        //writeDoc.close();

                        var checkForContent = function () {
                            setTimeout(function () {
                                var content = newWin.document.querySelector('body');

                                if (content && content.innerHTML.length) {
                                    newWin.focus();
                                    newWin.print();
                                    newWin.close();
                                } else {
                                    checkForContent();
                                }
                            }, 200);
                        };
                        checkForContent();
                    }
                }
                else {
                    Patient_Demographic.FillDemographic(patientId).done(function (patientInfo) {

                        var PatientProfileInfo = JSON.parse(patientInfo.DemographicFill_JSON);
                        var demographic_detail = JSON.parse(patientInfo.DemographicFill_JSON);

                        var patientAge = "";

                        if (PatientProfileInfo.Age) {
                            patientAge = PatientProfileInfo.Age.split(',');
                            if (parseInt((PatientProfileInfo.Age.split(',')[0]).split(' ')[1]) > 0) {

                                patientAge = PatientProfileInfo.Age.split(',')[0]; //age in years
                            } else if (parseInt((PatientProfileInfo.Age.split(',')[1]).split(' ')[1]) > 0) {
                                patientAge = PatientProfileInfo.Age.split(',')[1]; //age in months
                            } else {
                                patientAge = PatientProfileInfo.Age.split(',')[2]; //age in days

                            }
                        }

                        var form = "#pnlClinicalSuperBillTemplate #frmSuperBillTemplate";
                        var templateName = $(form + ' #ddlSuperBillTemplate option:selected').text();
                        //var DOB = 'DOB: ' + PatientProfileInfo.DOB;
                        var patientInfo = patientAge + " " + PatientProfileInfo.Sex;
                        var patientAddress = PatientProfileInfo.Address1 + ', ' + PatientProfileInfo.City + ', ' + PatientProfileInfo.State + ', ' + PatientProfileInfo.Zip;
                        var PatientEmail = '';
                        var PatientHomePhone = '';
                        var PatientCellPhone = '';

                        if (PatientProfileInfo.Email != null && PatientProfileInfo.Email != "") {
                            PatientEmail = '<li id="PatientEmail">Email: ' + PatientProfileInfo.Email + '</li>';
                        }
                        if (PatientProfileInfo.HomeTel != null && PatientProfileInfo.HomeTel != "") {
                            PatientHomePhone = '<li id="PatientHomePhone">Home Phone: ' + PatientProfileInfo.HomeTel + '</li>';
                        }
                        if (PatientProfileInfo.Cell != null && PatientProfileInfo.Cell != "") {
                            PatientCellPhone = '<li id="PatientCellPhone">Cell Phone: ' + PatientProfileInfo.Cell + '</li>';
                        }

                        if (globalAppdata['DateFormat'])
                            date_format = globalAppdata['DateFormat'];
                        var date = new Date();
                        //date_format = date_format.replace("mm", date.getMonth() + 1);
                        //date_format = date_format.replace("yyyy", date.getFullYear());
                        var day = "";
                        if (date.getDate().length < 2) {
                            day = "0" + date.getDate();
                        }
                        else {
                            day = date.getDate();
                        }
                        //date_format = date_format.replace("dd", day);

                        //var NotesLoad_JSON = JSON.parse(response.NotesLoad_JSON);
                        //      var providerData = JSON.parse(response.NoteHeaderProviderData);
                        //var providerName = (providerData[0].FirstName != "" ? providerData[0].FirstName : "") + (providerData[0].LastName != "" ? " " + providerData[0].LastName : "");
                        //date_format = utility.RemoveTimeFromDate(null, NotesLoad_JSON[0].VisitDate);
                        //PatientProfileInfo.Provider = providerName;
                        setTimeout(function () {

                            var formHeaderHtml = '<div id="printcall">' +
                                '<div id="PatientInfo">' +
                                '<div class="col-sm-4 col-lg-2 pull-left">' +
                                '<img src="content/images/SHS-nav-logo-small-100.png" class="img-responsive"></div>' +
                                     '<div class="col-sm-4 col-lg-2 pull-right">' +
                                     '<ul class="col-sm-4 col-lg-2  list-unstyled">' +
                                     '<li id="DOS" align="right">DOS: ' + date + '</li>' +
                                     '<li id="PatientProvider" align="right">Provider: ' + PatientProfileInfo.Provider + '</li>' +
                                      '</div>    <div class="clearfix"></div> <div class="splitter m-none mt-xs"><div class="spacer3"></div></div>' +
                                       '<div class="col-sm-4 col-lg-2 pull-left">' +
                                     '<ul class="list-unstyled" align="left">' +
                                     '<li id="PatientName" >' + PatientProfileInfo.FullName + '</li>' +
                                     '<li id="PatientAccount" ></li>' + PatientProfileInfo.AccountNo + '</li>' +
                                      '<li id="PatientAge" >' + patientInfo + '</li>' +
                                     '<li id="PatientAddress">' + patientAddress + '</li>' +
                                     PatientHomePhone +
                                     PatientCellPhone +
                                     PatientEmail +
                                     '</ul>' + '</div>' +
                                 '<div class="clearfix"></div>' +

                                 '<div class="splitter m-none mt-xs"><div class="spacer3"></div></div><div class="spacer3"></div><div class="spacer3"></div><div class="spacer3"></div><div class="spacer3"></div>' +
                                 '</div><div class="spacer5"></div>' +
                                 '<section class="">' +
                                 '<div class=""  id="grdICDCPTPrint" style="font-size:12px;">' + html + '</div></section></div>' +
                                 '<div class="spacer10"></div><hr /><h4 id="templateHeader"></h4><hr />';

                            var docType = '<!doctype html>';
                            var docCnt = formHeaderHtml;

                            var docHead = '<head><script src="Scripts/js/jquery-2.1.1.js"></script> <script src="Scripts/js/bootstrap.js"></script><link rel="stylesheet" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" href="Content/Blue/theme.css" />'
                         + '<link rel="stylesheet"  href="Content/Blue/theme-custom.css" /><link rel="stylesheet" href="Content/Blue/default.css" /><link rel="stylesheet" href="Content/Default/print-media.css" /><script src="Scripts/ApplicationCommand/utility.js"></script>'
                         + '<script>var isPrinted = false;function RemoveBottomSpace(){ $(".statmentPrint").removeAttr("style");isPrinted = true;window.print(); } '
                         + '</script>'
                         + '</head>';



                            var footer = ' <div style="position: absolute;left: 20px;right: 20px;font-size: 90%;bottom: 20px;">'
                          + '<div class="blueBgPrint" style=" float:left; width:100%; height:3px;margin-bottom:3px;"></div>'
                        + '<div id="ClinicalReportsFooterText" class="blueBgPrint" style="float:left; width:100%;padding:2px 5px 0 5px;height:22px;">'
                         + 'Generated by:'
                        + '<span id="ClinicalReportsFooter"> MDVISION PM EMR</span><span class="whiteColorPrint" style="float:right;"> Page 1 of 1</span>'
                         + '</div> </div>';
                            if (utility.UserBrowser() == "IE") {
                                Clinical_Medications.printPatientMedicationInfo(docType, docHead, docCnt, footer);
                            } else {
                                var winAttr = "location=yes, statusbar=no, menubar=no, titlebar=no, toolbar=no,dependent=no, width=1000, height=600, resizable=yes, screenX=200, screenY=200, personalbar=no, scrollbars=yes";;
                                var newWin = window.open("", "_blank", winAttr);
                                writeDoc = newWin.document;
                                writeDoc.open();
                                writeDoc.write(docType + '<html>' + docHead + '<body style="font-size:30px;">' + docCnt + footer + '</body></html>');
                                var checkForContent = function () {
                                    setTimeout(function () {
                                        var content = newWin.document.querySelector('body');

                                        if (content && content.innerHTML.length) {
                                            newWin.focus();
                                            newWin.print();
                                            newWin.close();
                                        } else {
                                            checkForContent();
                                        }
                                    }, 200);
                                };
                                checkForContent();
                            }
                        }, 100);


                    });
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },


    printPatientMedicationInfo: function (docType, docHead, docCnt, footer) {
        var frame1 = $('<iframe />');
        frame1[0].name = "ClinicalMedicationPrint";
        frame1.attr("scrolling", "no");
        frame1.css({ "position": "absolute", "top": "-1000000px", "overflow": "hidden", "margin": "0px", "padding": "0px" });
        $("body").append(frame1);
        var frameDoc = frame1[0].contentWindow ? frame1[0].contentWindow : frame1[0].contentDocument.document ? frame1[0].contentDocument.document : frame1[0].contentDocument;
        frameDoc.document.open();
        frameDoc.document.write('<html><head>');
        frameDoc.document.write('</head><body  style="font-size:30px;">');
        frameDoc.document.write('<link rel="stylesheet" media="print" href="Content/Blue/bootstrap.css" /><link rel="stylesheet" media="print" href="Content/Default/bootstrap.css" /> <link rel="stylesheet" media="print" href="Content/Blue/theme.css" /><link rel="stylesheet" media="print" href="Content/Blue/theme-custom.css" /><link rel="stylesheet" media="print" href="Content/Blue/default.css" /><link media="print" rel="stylesheet" href="Content/Default/print-media.css" />');
        frameDoc.document.write(docCnt);
        frameDoc.document.write('</footer>' + footer + '</footer>');
        frameDoc.document.write('</body></html>');
        frameDoc.document.close();
        window.frames["ClinicalMedicationPrint"].document.execCommand('print', false, null);
        frame1.remove();
    },

    getHeaderFooterInfo: function (patientId) {

        var objData = {};
        objData["PatientID"] = patientId;
        objData["commandType"] = "get_report_header_footer";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "Medical", "Medications");
    },
    UpdateRouteIdByMedicationId: function (MedicationId,obj) {
        var RouteId = $(obj).val();
        var objData = {};
        objData["MedicationId"] = MedicationId;
        objData["RouteId"] = RouteId;
        objData["commandType"] = "update_RouteId_ByMedicationId";
        var data = JSON.stringify(objData);
        $.when(MDVisionService.APIService(data, "Medical", "Medications")).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false)
                utility.DisplayMessages(response.Message, 1);
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    UpdateNegationReasonIdByMedicationId: function(MedicationId,obj){
        var NegationReasonId = $(obj).val();
        var objData = {};
        objData["MedicationId"] = MedicationId;
        objData["NegationReason"] = NegationReasonId;
        objData["commandType"] = "update_NegationReasonId_ByMedicationId";
        var data = JSON.stringify(objData);
        $.when(MDVisionService.APIService(data, "Medical", "Medications")).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false)
                utility.DisplayMessages(response.Message, 1);
            else
                utility.DisplayMessages(response.Message, 3);
        });
    },
    medicationsGridLoad: function (response) {
        var dfd = $.Deferred();
        var IsCurrent = $('#' + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #divSwitch #switchActive').attr('isactive');
        //Start By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        //$('#' + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications").dataTable().fnDestroy();
        //$('#' + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications tbody").find("tr").remove();

        if ($.fn.dataTable.isDataTable("#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications")) {
            $("#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications").dataTable().fnClearTable();
            $("#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications").dataTable().fnDestroy();
        }
        $("#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications tbody").find("tr").remove();
        //End By Babur on 2/16/2016 - Below line inorder to remove duplicate grid search
        //--------------------
        var arraTemp = [];
        if (response.MedicationCount > 0) {
            //--------------------

            var MedicationJSONData = JSON.parse(response.MedicationLoad_JSON);
            var MedicationHistoryLoad_JSON = JSON.parse(response.MedicationHistoryLoad_JSON);
            var MedicationAntimicrobialRoute_JSON = JSON.parse(response.MedicationAntimicrobialRoute);
            var MedicationNegationReason_JSON = JSON.parse(response.MedicationNegationReason);
            // get Actions
            $.each(MedicationJSONData, function (i, item) {
                var MedicationId = item.MedicationID;
                var ChildHistory_Medication = $.grep(MedicationHistoryLoad_JSON, function (n, i) {
                    return n.MedicationID == MedicationId;
                });

                var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(item.NDCID, 'Clinical_Medications', 1);
                var isDel = item.IsDeleted;
                //Start By Babur on 1/19/2016 - show deleted allergies
                var strDrugNameWithDeleted = item.MedicationName;

                if (item.IsDeleted == "True") {
                    strDrugNameWithDeleted += " -- (Deleted)";
                }

                strDrugNameWithDeleted = strDrugNameWithDeleted + " " + $infoButtonrow

                //End By Babur on 1/19/2016 - show deleted allergies


                //Start By Babur on 1/28/2016 - show refill medications with empty dosage
                var strDrugDosage = "";

                if (item.Action != "" || item.Dose != "" || item.DoseUnit != "" || item.Routeby != "" || item.DoseTiming != "" || item.DoseOther != "") {
                    strDrugDosage = item.Action + " " + item.Dose + " " + item.DoseUnit + " " + item.Routeby + " " + item.DoseTiming + " " + item.DoseOther;
                    //strDrugDosage += " " + item.PatientNotes;
                }
                else {
                    //strDrugDosage = item.PatientNotes;
                }

                //End By Babur on 1/28/2016 - show refill medications with empty dosage


                var $row = $('<tr/>');
                if (item.IsActive == "True") {
                    IsActive = 0;
                    activeTitle = "Active Record";
                    tglclass = "fa fa-toggle-on green";
                }
                else {
                    IsActive = 1;
                    activeTitle = "Inactive Record";
                    tglclass = "fa fa-toggle-on red";
                }
                //adding checkboxes column and disabling that row, if Prescriptions already binded with notes
                var SelectionCheckBoxColumn = "";
                var Checked = "";
                if (item.IsNoteLinked == "True") {
                    if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.MedicationID + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                        Checked = " ";
                    } else {
                        Checked = " checked";
                    }
                } else {
                    if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.MedicationID + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                        Checked = " checked";
                    } else {
                        Checked = "";
                    }
                }
                var htmlRoute = '<option value="">--Select--</option>';
                $.each(MedicationAntimicrobialRoute_JSON, function (i, result) {
                    if (!result.ParentId)
                        if (result.Id == item.RouteId)
                            htmlRoute += '<option selected class="bold" value="' + result.Id + '">' + result.Name + '</option>';
                        else 
                            htmlRoute += '<option class="bold" value="' + result.Id + '">' + result.Name + '</option>';
                    else
                        if (result.Id == item.RouteId)
                            htmlRoute += '<option selected class="optionChild" value="' + result.Id + '">&nbsp&nbsp&nbsp' + result.Name + '</option>';
                        else
                            htmlRoute += '<option class="optionChild" value="' + result.Id + '">&nbsp&nbsp&nbsp' + result.Name + '</option>';
                });
                var finalRouteHtmlDDL = '<td><select class="form-control" onchange="Clinical_Medications.UpdateRouteIdByMedicationId(' + item.MedicationID + ', this)">' + htmlRoute + '</select></td>';

                if (globalAppdata["isTransPubHealthAgAntimicobialUse"] && globalAppdata["isTransPubHealthAgAntimicobialUse"].toLowerCase() == "false")
                    finalRouteHtmlDDL = "";
                var htmlNegationReason = '<option value="">--Select--</option>';
                $.each(MedicationNegationReason_JSON, function (i, result) {
                    if (result.LookUpId == item.NegationReasonId)
                        htmlNegationReason += '<option selected class="optionChild" value="' + result.LookUpId + '">&nbsp&nbsp&nbsp' + result.DropDown + '</option>';
                    else
                        htmlNegationReason += '<option class="optionChild" value="' + result.LookUpId + '">&nbsp&nbsp&nbsp' + result.DropDown + '</option>';
                });
                var finalNegationReasonHtmlDDL = '<select class="form-control" onchange="Clinical_Medications.UpdateNegationReasonIdByMedicationId(' + item.MedicationID + ', this)">' + htmlNegationReason + '</select>';


                if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
                    SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="Clinical_Medications.enableAddMedication(this);" id="' + item.MedicationID + '" name="SelectCheckBoxMedication" ' + Checked + ' class="input-block"/></td>';
                } else {
                    SelectionCheckBoxColumn = "";
                }
                var actions = '&nbsp;<a class="btn btn-xs " href="javascript:void(O)" title="Activity Log" onclick="Clinical_Medications.rowHistory(' + item.MedicationID + ');"> <i class="fa fa-history blue"></i></a>'
                if (ChildHistory_Medication.length > 0) {
                    //$row.append(selectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td class="actions" id="' + item.AllergyId + '" >' + actions + '</td><td>' + item.Allergen + '</td><td>' + item.Reaction + '</td><td ' + color + ' >' + item.Severity + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + '<br> ' + strModifiedByWithDeleted + '</td><td align="center"> ' + comments + ' </td>');
                    $row.append(SelectionCheckBoxColumn + '<td><a href="#" class="on-editing expand-row" title="Expand/Collapse Record"><i class="fa fa-plus-square"></i></a></td><td style="display:none;">' + item.MedicationID + '</td><td>' + actions + '</td><td>' + strDrugNameWithDeleted + '</td><td>' + strDrugDosage + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td>' + finalRouteHtmlDDL + '<td>' + finalNegationReasonHtmlDDL + '</td>');
                } else {
                    //$row.append(selectionCheckBoxColumn + '<td></td><td class="actions" id="' + item.AllergyId + '" >' + actions + '</td><td>' + item.Allergen + '</td><td>' + item.Reaction + '</td><td>' + utility.RemoveTimeFromDate(null, item.OnSetDate) + '</td><td>' + item.LastModified + '<br> ' + strModifiedByWithDeleted + '</td>');
                    $row.append(SelectionCheckBoxColumn + '<td></td><td style="display:none;">' + item.MedicationID + '</td><td>' + actions + '</td><td>' + strDrugNameWithDeleted + '</td><td>' + strDrugDosage + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td>' + finalRouteHtmlDDL + '<td>' + finalNegationReasonHtmlDDL + '</td>');
                }

                //$row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.MedicationID + '</td><td class="actions" id="' + item.MedicationID + '" >' + actions + '</td><td>' + strDrugNameWithDeleted + '</td><td>' + strDrugDosage + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td>');

                //  $row.append(SelectionCheckBoxColumn + '<td style="display:none;">' + item.MedicationID + '</td><td>' + actions + '</td><td>' + strDrugNameWithDeleted + '</td><td>' + strDrugDosage + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td>');
                $('#' + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications tbody").last().append($row);


                var currentRowchilds = $();

                if (ChildHistory_Medication.length > 0) {
                    $.each(ChildHistory_Medication, function (i, item) {


                        var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(item.NDCID, 'Clinical_Medications', 1);
                        var isDel = item.IsDeleted;
                        //Start By Babur on 1/19/2016 - show deleted allergies

                        var strDrugNameWithDeleted = item.MedicationName;

                        if (item.IsDeleted == "True") {
                            strDrugNameWithDeleted += " -- (Deleted)";
                        }

                        strDrugNameWithDeleted = strDrugNameWithDeleted + " " + $infoButtonrow


                        //End By Babur on 1/19/2016 - show deleted allergies


                        //Start By Babur on 1/28/2016 - show refill medications with empty dosage
                        var strDrugDosage = "";

                        if (item.Action != "" || item.Dose != "" || item.DoseUnit != "" || item.Routeby != "" || item.DoseTiming != "" || item.DoseOther != "") {
                            strDrugDosage = item.Action + " " + item.Dose + " " + item.DoseUnit + " " + item.Routeby + " " + item.DoseTiming + " " + item.DoseOther;
                            //strDrugDosage += " " + item.PatientNotes;
                        }
                        else {
                            //strDrugDosage = item.PatientNotes;
                        }

                        //End By Babur on 1/28/2016 - show refill medications with empty dosage


                        var $row = $('<tr/>');
                        if (item.IsActive == "True") {
                            IsActive = 0;
                            activeTitle = "Active Record";
                            tglclass = "fa fa-toggle-on green";
                        }
                        else {
                            IsActive = 1;
                            activeTitle = "Inactive Record";
                            tglclass = "fa fa-toggle-on red";
                        }
                        //adding checkboxes column and disabling that row, if Prescriptions already binded with notes
                        var SelectionCheckBoxColumn = "";
                        var Checked = "";
                        if (item.IsNoteLinked == "True") {
                            if (Clinical_ProgressNote.DetachedNoteComponentIds.length != 0 && $.inArray(item.MedicationID + "", Clinical_ProgressNote.DetachedNoteComponentIds) > -1) {
                                Checked = " ";
                            } else {
                                Checked = " checked";
                            }
                        } else {
                            if (Clinical_ProgressNote.AttachedNoteComponentIds.length != 0 && $.inArray(item.MedicationID + "", Clinical_ProgressNote.AttachedNoteComponentIds) > -1) {
                                Checked = " checked";
                            } else {
                                Checked = "";
                            }
                        }

                        if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
                            SelectionCheckBoxColumn = '<td><input type="checkbox" onchange="Clinical_Medications.enableAddMedication(this);" id="' + item.MedicationID + '" name="SelectCheckBoxMedication" ' + Checked + ' class="input-block"/></td>';
                        } else {
                            SelectionCheckBoxColumn = "";
                        }

                        var actions = '&nbsp;<a class="btn btn-xs " href="javascript:void(O)" title="Activity Log" onclick="Clinical_Medications.rowHistory(' + item.MedicationID + ');"> <i class="fa fa-history blue"></i></a>'
                        //Start//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed
                        if (Clinical_Medications.params.ActionPanContainer == "actionPanClinicalProgressNote") {
                            var currentHistory = '<tr class="childRow-bg">' + SelectionCheckBoxColumn + '<td></td><td style="display:none;">' + item.MedicationID + '</td><td>' + actions + '</td><td>' + strDrugNameWithDeleted + '</td><td>' + strDrugDosage + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td></tr>';
                        }
                        else {
                            var currentHistory = '<tr class="childRow-bg">' + SelectionCheckBoxColumn + '<td></td><td style="display:none;">' + item.MedicationID + '</td><td>' + actions + '</td><td>' + strDrugNameWithDeleted + '</td><td>' + strDrugDosage + '</td><td>' + utility.RemoveTimeFromDate(null, item.StartDate) + '</td></tr>';

                        }
                        //End//Ahmad Raza//14/12/2015//Row Sequence issue in History Grid, fixed

                        currentRowchilds = currentRowchilds.add(currentHistory);

                    });
                }

                if (currentRowchilds.length > 0) {

                }

                arraTemp.push({ row: $row, childs: currentRowchilds });
            });

            if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
                Clinical_Medications.medReconciledOnNote();
            }

        } else {

            $("#" + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #dgvMedications').DataTable({
                "language": {
                    "emptyTable": "No Medications Found"
                }, "bDestroy": true, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        //Inalize grid
        var panelGrid = "#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result";
        var gridId = "#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications";

        //Start By Babur on 2/16/2016 - Below line comment out inorder to remove duplicate grid search

        if ($.fn.dataTable.isDataTable(gridId) || $(gridId).parent().parent().hasClass("dataTables_wrapper"))
            ;
        else
            Clinical_Medications.myGrid = EMRUtility.MakeEditableGrid(panelGrid, gridId, Clinical_Medications, 0, false, true, false, true, false, null);
        //if ($('#' + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications_wrapper").length > 0){
        //}
        //else {
        //    $('#' + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications").DataTable({ "bDestroy": true, "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "aaSorting": [], "order": [], "aoColumnDefs": [{ "bSortable": false, "aTargets": [0] }] }); // to remove records per page dropdown
        //}
        //Start//28//06//2016//Azhar //Sorting removed from first column of grid |EMR-1361
        if ($('#' + Clinical_Medications.params.PanelID + ' #dgvMedications').length > 0) {
            $('#' + Clinical_Medications.params.PanelID + ' #dgvMedications').dataTable().fnSettings().aoColumns[0].bSortable = false;
            $('#' + Clinical_Medications.params.PanelID + ' #dgvMedications thead tr th:first-child').removeClass('sorting_asc');
            $('#' + Clinical_Medications.params.PanelID + ' #dgvMedications').dataTable().fnSettings().aoColumns[6].bSortable = false; //route
            $('#' + Clinical_Medications.params.PanelID + ' #dgvMedications thead tr th:nth-child(7)').removeClass('sorting');
            //End//28//06//2016//Azhar//Sorting removed from first column of grid
        }
        //rander childs
        $.each(arraTemp, function (i, item) {

            if (Clinical_Medications.myGrid != null) {
                var row = Clinical_Medications.myGrid.datatable.row(item.row);
                if (item.childs.length > 0) {
                    row.child(item.childs);
                }
                else {
                    //row.find("td:nth-child(1)").html("");
                }
            }

        });

        var NoRecordFoundTD = $("#" + Clinical_Medications.params.PanelID + ' #pnlMedications_Result #dgvMedications tr td.dataTables_empty');
        var checked = '';
        var NoRecordFoundMsg = "No Active Medications Found";
        if (IsCurrent == "0") {
            $("#" + Clinical_Medications.params.PanelID + " #btnPrint").hide();
            $("#" + Clinical_Medications.params.PanelID + " #btnPrint").addClass('hidden');
            NoRecordFoundMsg = "No InActive Medications Found";
        } else if (IsCurrent == null) {
            $("#" + Clinical_Medications.params.PanelID + " #btnPrint").show();
            $("#" + Clinical_Medications.params.PanelID + " #btnPrint").removeClass('hidden');
            IsCurrent = "1";
            checked = 'checked="checked"';
        } else {
            $("#" + Clinical_Medications.params.PanelID + " #btnPrint").show();
            $("#" + Clinical_Medications.params.PanelID + " #btnPrint").removeClass('hidden');
            NoRecordFoundMsg = "No Active Medications Found";
            IsCurrent = "1";
            checked = 'checked="checked"';
        }
        NoRecordFoundTD.html(NoRecordFoundMsg);
        var HtmlOfSwitchA = '<span class="pr-xs">Past</span><div class="btnWidgetSwitch switch switch-xs switch-success">' +
                         '<input id="switchActive" isactive="' + IsCurrent + '" type="checkbox" ' + checked + ' name="switch" data-plugin-ios-switch="" style="display: inline;" onchange="Clinical_Medications.activeMedicationSearch(this);">' +
                          '</div><span class="pl-xs">Current</span>';


        $("#" + Clinical_Medications.params.PanelID + ' #pnlMedications_Result .datatables-header div:first').html('<div id="divSwitch">' + HtmlOfSwitchA + '</div>');
        Clinical_Medications.domReadyFuncMedications();
        EMRUtility.fixDataTableDuplication("#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result");
        dfd.resolve();
        return dfd;
    },
    rowDetail: function ($row, ClassName) {
        var currentMedicationId = $($row).attr("id") != null ? $($row).attr("id") : -1;
        if (currentMedicationId > 0) {
            //Clinical_Allergies.AllergiesEdit(currentAllergyId);
            Clinical_Medications.ShowHistory(currentMedicationId);
        }
    },
    rowHistory: function (MedicationId) {
        var currentMedicationId = MedicationId != null ? MedicationId : -1;
        if (currentMedicationId > 0) {
            Clinical_Medications.ShowHistory(currentMedicationId);
        }
    },
    ShowHistory: function (medicationId) {
        //Start 20-10-2016 Edit By Humaira Yousaf Bug# QAC2-491
        var tabId = Clinical_Medications.params.TabID;
        if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote" || Clinical_Medications.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" || Clinical_Medications.params.ParentCtrl == "Clinical_Treatment") {
            tabId = "Clinical_Medications";
        }
        EMRUtility.showCurrentItemHistory(Clinical_Medications.params.PanelID, null, medicationId, "Medication", Clinical_Medications.params.patientID, tabId, null);
        //End 20-10-2016 Edit By Humaira Yousaf Bug# QAC2-491
    },
    rowDraw: function ($row, _self, values) {

        _self.datatable.row($row.get(0)).data(values);
        $actions = $row.find('td.actions');
        if ($actions.get(0)) {
            _self.rowSetActionsDefault($row);
        }
        _self.datatable.draw();
    },
    //End//Ahmad Raza//02/12/2015//This function will draw a new row in grid

    //Start//Ahmad Raza//02/12/2015//This function will be called when user is editing the row of grid and then cancel the updation
    rowCancel: function ($row, obj) {

        var _self = obj,
            $actions,
            i,
            data;

        if ($row.hasClass('adding')) {
            _self.datatable.row($row.get(0)).remove().draw();

        } else {

            data = _self.datatable.row($row.get(0)).data();
            _self.datatable.row($row.get(0)).data(data);

            $actions = $row.find('td.actions');
            if ($actions.get(0)) {
                _self.rowSetActionsDefault($row);
            }

            _self.datatable.draw();
        }
    },
    rowExpand: function ($row, obj) {
        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);

        //Start//14/12/2015//Ahmad Raza//Logic implemented for plus,minus sign on row expand/ row collapse, when form opened in notes
        if (Clinical_Medications.params.ActionPanContainer == "actionPanClinicalProgressNote" && $row.parent().parent().attr("id") == "dgvMedications") {
            if (row.child.isShown()) {
                // This row is already open - close it
                $row.find("td:nth-child(2) .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
                //tr.removeClass('shown');
            }
            else {
                $row.find("td:nth-child(2) .fa-plus-square").attr("class", "fa fa-minus-square");
                // Open this row
                row.child.show();
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        }
        else {
            if (row.child.isShown()) {
                // This row is already open - close it
                $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();
                //tr.removeClass('shown');
            }
            else {
                $row.find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
                // Open this row
                row.child.show();
                if (row.child() != null && row.child().find('[data-toggle="tooltip"]').length > 0) {
                    row.child().find('[data-toggle="tooltip"]').tooltip({ html: true });
                }
            }
        }
        //End//14/12/2015//Ahmad Raza//Logic implemented for plus,minus sign on row expand/ row collapse, when form opened in notes


    },

    ShowHideEditableGridRows: function (isShow) {

        var VitalsGridId = "#" + Clinical_Medications.params.PanelID + " #dgvMedications";
        var dataTable = $(VitalsGridId).DataTable();

        dataTable.row().nodes().each(function (parentRow, index) {

            var row = Clinical_Medications.EditableGrid.datatable.row(parentRow);

            if (isShow == true) {

                row.child.show();
                $(parentRow).find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");

            }
            else {

                $(parentRow).find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                row.child.hide();

            }

        });

    },
    unLoadTab: function (nextOrPre, controlToInvoke) {
        var parentPanelId = null;
        Clinical_Medications.controlToInvoke = controlToInvoke;
        if (Clinical_Medications.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet") {
            if (Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet") {
                parentPanelId = GetTab(Clinical_FaceSheet.params["ParentCtrl"]).PanelID;
                Clinical_FaceSheet.params.ChildPanelID = null;
                UnloadActionPan(Clinical_Medications.params.ParentCtrl, 'Clinical_Medications', null, parentPanelId);
            } else {
                UnloadActionPan(Clinical_Medications.params.ParentCtrl, 'Clinical_Medications');
            }
            Clinical_FaceSheet.loadFaceSheet();
        }
        else if (Clinical_Medications.params && Clinical_Medications.params.ParentCtrl && Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
            if (Clinical_Prescriptions.IsPrescriptionDeleted) {
                $("#clinicalTabProgressNote").trigger("click");
                Clinical_Prescriptions.IsPrescriptionDeleted = false;
            }
            var exist = false;
            $("#" + Clinical_Medications.params.PanelID + " #dgvMedications tbody").find('input[type="checkbox"]').each(function () {
                if (this.checked) {
                    exist = true;
                }
                if (exist) {
                    return false;
                }
            });
            $("#" + Clinical_Medications.params.PanelID + " #dgvPrescriptions tbody").find('input[type="checkbox"]').each(function () {
                if (this.checked) {
                    exist = true;
                }
                if (exist) {
                    return false;
                }
            });
            if (exist) {
                utility.myConfirmNote('1', function () {
                    $.when(Clinical_Medications.addMedicationsToNotes(null, null, true)).then(function () {
                        $.when(Clinical_Prescriptions.addPrescriptionsToNotes()).then(function () { Clinical_Medications.unloadBirthHistory(nextOrPre); });
                        //Clinical_Medications.unloadBirthHistory();
                    });
                }, "", function () {
                    Clinical_Medications.unloadBirthHistory();
                });
            }
            else {
                $.when(Clinical_Medications.AppendReviewedSoapText(true)).then(function () {
                    Clinical_Medications.unloadBirthHistory();
                });
            }
        }
        else if (Clinical_Medications.params && Clinical_Medications.params.ParentCtrl && Clinical_Medications.params.ParentCtrl == "Clinical_Treatment") {
            $.when(Clinical_Treatment.prescriptionSearch()).then(function () {
                Clinical_Medications.unloadBirthHistory();
            });
        }
        else {
                Clinical_Medications.unloadBirthHistory();
        }
    },
    openDrFirst: function () {

        var IsPatientRegisteredOnDrFirst;

        if (Clinical_Medications.params.PatientId != null && Clinical_Medications.params.PatientId != "") {
            Clinical_Medications.params.patientID = Clinical_Medications.params.PatientId;
        }
        else {
            Clinical_Medications.params.patientID = Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }


        $.when(IsUserHaveRcopiaRights = EMRUtility.CheckUserHaveRcopiaRights(globalAppdata["AppUserId"])).then(function () {
            if (IsUserHaveRcopiaRights.response == true) {
                $.when(IsPatientRegisteredOnDrFirst = EMRUtility.CheckPatientIsRegisteredOnDrFirst(Clinical_Medications.params.patientID)).then(function () {
                    if (IsPatientRegisteredOnDrFirst.response) {
                        var params = [];
                        var PanelID = "";
                        if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {

                            PanelID = 'pnlClinicalProgressNote #pnlClinicalMedications';
                            params["PanelID"] = 'pnlClinicalProgressNote #pnlClinicalMedications';
                            params["PrPanelID"] = 'pnlClinicalProgressNote #pnlClinicalMedications';
                            params["NotesId"] = Clinical_Medications.params.NotesId == null ? 0 : Clinical_Medications.params.NotesId;
                        }
                        else if (Clinical_Medications.params.ParentCtrl == "Clinical_Treatment") {

                            PanelID = 'pnlClinicalProgressNote #pnlClinicalTreatment #pnlClinicalMedications';
                            params["PanelID"] = PanelID;
                            params["PrPanelID"] = PanelID;
                            params["NotesId"] = 0;
                        }
                        else if (Clinical_Medications.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet") {

                            PanelID = 'pnlClinicalFaceSheet #pnlClinicalMedications';
                            params["PanelID"] = 'pnlClinicalFaceSheet #pnlClinicalMedications';
                            params["PrPanelID"] = 'pnlClinicalFaceSheet #pnlClinicalMedications';
                        }
                        else {
                            PanelID = 'pnlClinicalMedications';
                            params["PrPanelID"] = 'pnlClinicalMedications';
                        }
                        if ($("#" + PanelID + " #ulSocialHxTabsItems ").find("li.active").attr('id') == "listMedications") {
                            params["ComeFromModuleName"] = "Medication"
                        }
                        else {
                            params["ComeFromModuleName"] = "Prescription"
                        }
                        //Begin 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
                        params["StartupScreen"] = Clinical_Medications.startUpScreen;
                        //End 4/27/2016  Edit By M Ahmad    Imran Bug # EMR-682
                        params["PatientId"] = Clinical_Medications.params.patientID;
                        params["ParentCtrl"] = Clinical_Medications.params.ParentCtrl != "clinicalTabProgressNote" ? Clinical_Medications.params.TabID : "Clinical_Medications";

                        if (Clinical_Medications.params.ParentCtrl == "clinicalTabFaceSheet" || Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" || Clinical_Medications.params.ParentCtrl == "Clinical_Treatment") {
                            params["ParentCtrl"] = "Clinical_Medications";
                        }
                        params["FromAdmin"] = 0;
                        Clinical_Medications.AddDRFirstTab();
                        LoadActionPan("DRFirst", params, PanelID);
                    }
                    else {
                        utility.DisplayMessages("Patient Is Not Registered On DrFirst", 3);
                    }
                });
            }
            else {
                utility.DisplayMessages("You are not authorized to access this feature.", 2);
            }
        });
    },
    AddDRFirstTab: function () {
        var count = 0;
        for (var i = 0; i < TabsArray.length; i++) {
            if (TabsArray[i].TabID == "DRFirst") {
                count++;
            }
        }
        if (count < 1) {
            var Tab = new Object();
            Tab["Selected"] = true;
            Tab["TabID"] = "DRFirst";
            Tab["MasterTabID"] = "mstrTabClinical";
            Tab["PanelID"] = "pnlClinicalDRFirst";
            Tab["Active"] = false;
            Tab["ContainerControlID"] = "DRFirst";
            Tab["Container"] = "";
            Tab["Path"] = "./EMR/HTML/Clinical/Medical/DRFirst.html";
            Tab["isActionPan"] = true;
            Tab["ActionPanContainer"] = "actionPanClinicalDRFirst";
            TabsArray.push(Tab);

        }



    },
    /* Unloads Medications
      Author: Muhammad Azhar Shahzad */
    unloadBirthHistory: function (nextOrPre) {
        if (Clinical_Medications.params["FromAdmin"] == "0") {
            if (Clinical_Medications.params != null && Clinical_Medications.params.ParentCtrl != null) {

                if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote" && nextOrPre == true) {
                    UnloadActionPan(Clinical_Medications.params.ParentCtrl, 'Clinical_Medications');
                    if (Clinical_Medications.controlToInvoke != null) {
                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Medications.controlToInvoke.replace(/\s/g, ''));
                            Clinical_Medications.controlToInvoke = null;
                        }, 600);
                    }
                }
                else {
                    UnloadActionPan(Clinical_Medications.params.ParentCtrl, 'Clinical_Medications');
                    if (Clinical_Medications.controlToInvoke != null) {

                        setTimeout(function () {
                            Clinical_ProgressNote.SelectNotesComponentTab(Clinical_Medications.controlToInvoke.replace(/\s/g, ''));
                            Clinical_Medications.controlToInvoke = null;
                        }, 600);

                    }
                }
            }
            else
                UnloadActionPan(null, 'Clinical_Medications');
            if (Clinical_Medications.params.MedicationsTab == 'Prescription') {
                EMRUtility.scrollToPNcomponent('clinical_prescription');
            } else {
                EMRUtility.scrollToPNcomponent('clinical_medications');
            }
        }
        else {
            $("#mstrDivMedical #clinicalMenu_History_Medications").remove();
            RemoveAdminTab();
            if (Clinical_Medications.params.MedicationsTab == 'Prescription') {
                EMRUtility.scrollToPNcomponent('clinical_prescription');
            } else {
                EMRUtility.scrollToPNcomponent('clinical_medications');
            }
        }
    },
    activeMedicationSearch: function () { },

    /* Enable the Add to Notes button at least one of the Medication is selected.
       Author: ZeeshanAK | Date: January 15,2015 */
    enableAddMedication: function (obj) {
        if ($(obj).is(':checked')) {
            if ($.inArray(obj.id + 'meds', Clinical_ProgressNote.AttachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.push(obj.id + 'meds');
            } if ($.inArray(obj.id + 'meds', Clinical_ProgressNote.DetachedNoteComponentIds) != -1) {
                var index = Clinical_ProgressNote.DetachedNoteComponentIds.indexOf(obj.id + 'meds');
                if (index > -1) {
                    Clinical_ProgressNote.DetachedNoteComponentIds.splice(index, 1);
                }
            }
        } else {

            var index = Clinical_ProgressNote.AttachedNoteComponentIds.indexOf(obj.id + 'meds');
            if (index > -1) {
                Clinical_ProgressNote.AttachedNoteComponentIds.splice(index, 1);
            }
            if ($.inArray(obj.id + 'meds', Clinical_ProgressNote.DetachedNoteComponentIds) == -1) {
                Clinical_ProgressNote.DetachedNoteComponentIds.push(obj.id + 'meds');
            }
        }
    },


    activeMedicationSearch: function () {
        Clinical_Medications.medicationsSearch();
    },

    noActiveMedicationSoapText: function (hideAlertMessage) {
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        if ($(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main0').length == 0) {
            Clinical_Medications.checkMedicationsExists();
            var htmlForNoMedication = '<section id="Cli_Medications_Main0"> <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="Cli_Medication_0"><i class="fa fa-edit"></i></a>' +
    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="Cli_Medications_Main0"  ><i class="fa fa-times"></i></a></div> ' +
    '<div id="Cli_Medication_0"><ul class="list-unstyled"><li id="NoMedications"> No Active Medications</li></ul></div></section>';
            Clinical_Medications.updateMedicationsHtml(htmlForNoMedication, '0', noteHTMLCtrl, null, false, false, null, hideAlertMessage);
        }
    },

    createMedicationsBodyHTMLFromNotes: function (Medications, AttachedMedications, hideAlertMessage, bNotSaveCompt) {

        Clinical_Medications.checkMedicationsExists();

        if (Medications && Medications.length > 0) {

            var fromDrFirst = false;
            Clinical_Medications.createMedicationBodyHTMLWithOutReconcileFromNote(Medications, AttachedMedications, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', -1, hideAlertMessage, bNotSaveCompt);
        } else {

            Clinical_Medications.noActiveMedicationSoapText(hideAlertMessage);
        }
    },

    createMedicationBodyHTMLWithOutReconcileFromNote: function (Medications, AttachedMedications, noteHTMLCtrl, medicationsId, hideAlertMessage, bNotSaveCompt) {

        var dfd = $.Deferred();
        Clinical_Medications.checkMedicationsExists();

        if (Medications) {

            var MedicationsSOAPJSON = Medications;
            var medicationReviewSoapJSON = [];

            var $mainDivMedications = $(document.createElement('div'));

            var $mainDivCurrentMedications = $(document.createElement('div'));
            $mainDivCurrentMedications.attr('id', "Section_CurrentMedications");
            $mainDivCurrentMedications.append('<h6 class="text-bold pl-default" style = "color:#468aea">Current Medications</h6>' + "<div id='AllCurrentMedications'></div>");
            var $mainDivPastMedications = $(document.createElement('div'));
            $mainDivPastMedications.attr('id', "Section_PastMedications");
            $mainDivPastMedications.append('<h6 class="text-bold pl-default" style = "color:#bd0e09">Past Medications</h6>' + "<div id='AllPastMedications'></div>");

            if (MedicationsSOAPJSON == null || MedicationsSOAPJSON.length == 0) {

                if (medicationReviewSoapJSON != null && medicationReviewSoapJSON.length != 0) {

                    $.each(medicationReviewSoapJSON, function (index, item) {

                        if (item.ReviewedOn != null && item.ReviewedOn != '') {

                            var dateFormat = item.ReviewedOn;
                            var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);

                            $ListMedications.append("<li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) + " on:  " + ReviewedOndateFormat + "</li>");
                        }
                    });
                }
                else {
                    return "";
                }
            }
            else {
                if ($(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main0').length != 0) {
                    $(noteHTMLCtrl + ' #Cli_Medications_Main0').parent().remove();
                }
            }
            var $DivNewCurrentMedications = $(document.createElement('div'));
            var $DivNewPastMedications = $(document.createElement('div'));
            var AListId = [];
            var def = [];
            $.each(MedicationsSOAPJSON, function (index, element) {

                var ALid = element.MedicationID;
                AListId.push(ALid);
                var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1, "", 'Clinical_Medications');
                var $SectionBodyMedications = $(document.createElement('section'));

                $SectionBodyMedications.attr('id', "Cli_Medications_Main" + ALid);

                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Medication_" + ALid);

                var $ListMedications = $(document.createElement('ul'));
                $ListMedications.attr('class', 'list-unstyled')

                $SectionBodyMedications.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Medication_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Medications_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');

                var duration = "";

                if (element.Duration == '') {
                    duration;
                }
                else if (element.Duration == '0') {
                    duration;
                }
                else if (element.Duration == '1') {
                    duration = " for " + element.Duration + " Day";
                }
                else {
                    duration = " for " + element.Duration + " Days";
                }
                var strDrugDosage = "";
                if (element.Action != "" || element.Dose != "" || element.DoseUnit != "" || element.Routeby != "" || element.DoseTiming != "" || element.DoseOther != "") {
                    strDrugDosage = element.Action + " " + element.Dose + " " + element.DoseUnit + " " + element.Routeby + " " + element.DoseTiming + " " + element.DoseOther + duration;
                    strDrugDosage += element.PatientNotes != "" ? (" (" + element.PatientNotes + ")") : "";
                }
                else {
                    strDrugDosage = element.PatientNotes;
                }

                $ListMedications.append("<li> <strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" + ", " +
                    strDrugDosage +
                    (element.Quantity == '' ? "" : ", Quantity  " + element.Quantity) + " " +
                     (element.QuantityUnit == "" ? "" : " " + element.QuantityUnit) + "(s)" +
                                (element.Refill == '' ? "" : ", Refill(s)  " + element.Refill) + " " +
                                ((element.Substitution == null || element.Substitution == "") ? "" : "," + ((element.Substitution).toLowerCase() == 'n' ? ' Dispense as written ' : ' Substitution permitted ')) +
                                (element.PrescribedOn == null || element.PrescribedOn == '' ? "" : ", Prescribed On " + element.PrescribedOn) +
                                (element.ProviderName == null || element.ProviderName == '' ? "" : " by " + element.ProviderName) + "</li>"
                );

                $ListMedications.append((element.Comments == "" ? "" : "<li>Comments: " + element.Comments));

                $DetailsDiv.append($ListMedications);
                $SectionBodyMedications.append($DetailsDiv);

                if ($(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                    if (!((element.StopDate == '' || element.StopDate == null) || new Date(element.StopDate) > new Date())) {
                        $DivNewPastMedications.append($SectionBodyMedications);
                    }
                    else {
                        $DivNewCurrentMedications.append($SectionBodyMedications);
                    }
                }
                else {

                    var CommentHTML = "";
                    var CommentsID = $(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid + ' ul li:Last').attr('id');

                    if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                        CommentHTML = $(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                    }

                    $(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).html($SectionBodyMedications.html());
                    $(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid + ' ul').append(CommentHTML);;
                }
            });

            $.when.apply($, def).done(function ($n) {

                if (AListId.join(",") != "") {
                    medicationsId = AListId.join(",");
                }

                if ($DivNewPastMedications.html() != '' || $DivNewCurrentMedications.html() != '') {
                    if ($DivNewCurrentMedications.html() != '') {
                        if ($(noteHTMLCtrl).find('#Section_CurrentMedications').length == 0) {
                            $mainDivMedications.append($mainDivCurrentMedications);
                            $mainDivMedications.find('#AllCurrentMedications').append($DivNewCurrentMedications);
                        }
                        else {
                            $mainDivMedications.append($(noteHTMLCtrl).find('#Section_CurrentMedications')[0].outerHTML);
                            $mainDivMedications.find('#AllCurrentMedications').append($DivNewCurrentMedications);
                        }
                    }
                    else {
                        if ($(noteHTMLCtrl).find('#Section_CurrentMedications').length != 0) {
                            $mainDivMedications.append($(noteHTMLCtrl).find('#Section_CurrentMedications')[0].outerHTML);
                        }
                    }

                    if ($DivNewPastMedications.html() != '') {
                        if ($(noteHTMLCtrl).find('#Section_PastMedications').length == 0) {
                            $mainDivMedications.append($mainDivPastMedications);
                            $mainDivMedications.find('#AllPastMedications').append($DivNewPastMedications);
                        }
                        else {
                            $mainDivMedications.append($(noteHTMLCtrl).find('#Section_PastMedications')[0].outerHTML);
                            $mainDivMedications.find('#AllPastMedications').append($DivNewPastMedications);
                        }
                    }
                    else {
                        if ($(noteHTMLCtrl).find('#Section_PastMedications').length != 0) {
                            $mainDivMedications.append($(noteHTMLCtrl).find('#Section_PastMedications')[0].outerHTML);
                        }
                    }

                    if (medicationReviewSoapJSON != null && medicationReviewSoapJSON.length != 0) {
                        $.each(medicationReviewSoapJSON, function (index, item) {
                            if (item.ReviewedOn != null && item.ReviewedOn != '') {
                                var dateFormat = item.ReviewedOn
                                var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                                $mainDivMedications.append("<section id='Cli_Medications_ReviewByMedication'><li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                           " on:  " + ReviewedOndateFormat + "</li></section>");
                            }
                        });
                    }
                    $.when(Clinical_Medications.setMedicationsHtml($mainDivMedications.html(), medicationsId, AttachedMedications, noteHTMLCtrl, false, false, null, hideAlertMessage)).then(function () {
                        dfd.resolve('ok');
                    });
                }
                else {
                    if (medicationReviewSoapJSON != null && medicationReviewSoapJSON.length != 0) {
                        $.each(medicationReviewSoapJSON, function (index, item) {
                            if (item.ReviewedOn != null && item.ReviewedOn != '') {
                                var dateFormat = item.ReviewedOn
                                var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                                $mainDivMedications.append("<section id='Cli_Medications_ReviewByMedication'><li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                           " on:  " + ReviewedOndateFormat + "</li></section>");
                            }
                        });
                    }

                    $.when(Clinical_Medications.setMedicationsHtml('', medicationsId, AttachedMedications, noteHTMLCtrl, false, false, null, hideAlertMessage)).then(function () {
                        if (!bNotSaveCompt)
                            Clinical_ProgressNote.saveComponentSOAPText('Medications', hideAlertMessage);
                        dfd.resolve('ok');
                    });
                }
            });
        }

        return dfd;
    },

    setMedicationsHtml: function (medicationsHTML, medicationsID, AttachedMedications, noteHTMLCtrl, isReconcileView, bMedReconciled, MedReconciledId, hideAlertMessage) {
        var dfd = $.Deferred();
        $(noteHTMLCtrl + ' clinical_medications').parent().parent().addClass('initialVisitBody');

        if (medicationsHTML != '') {
            var divMedication = $(noteHTMLCtrl + ' clinical_medications a:nth-child(1)')[0].outerHTML + $(noteHTMLCtrl + ' clinical_medications a:nth-child(2)')[0].outerHTML + $(noteHTMLCtrl + ' clinical_medications a:nth-child(3)')[0].outerHTML;
            $(noteHTMLCtrl + ' clinical_medications').html(divMedication);
            $(noteHTMLCtrl).find(' #Section_CurrentMedications').remove();
            $(noteHTMLCtrl).find(' #Section_PastMedications').remove();
            $(noteHTMLCtrl).find(' #Section_MedicationPrescribed').remove();
            $(noteHTMLCtrl).find(' #Section_MedicationReconciled').remove();
            $(noteHTMLCtrl).find(' #Cli_Medications_ReviewByMedication').remove();
            $(medicationsHTML).insertAfter($(noteHTMLCtrl + ' clinical_medications').parent());

            if (bMedReconciled != true) {
                $(noteHTMLCtrl).find(' #Section_MedicationCurrent').remove();
                $(noteHTMLCtrl).find(' #Section_MedicationStop').remove();
                $(noteHTMLCtrl).find(' #Section_MedicationPrescribed').remove();
                $(noteHTMLCtrl).find(' #Section_MedicationReconciled').remove();
            }
        }


        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (medicationsHTML != '' && medicationsID != null && medicationsID != '' && medicationsID != '0' && AttachedMedications.length > 0) {

            $.when(Clinical_ProgressNote.saveComponentSOAPText('Medications', hideAlertMessage, null, bMedReconciled, MedReconciledId)).then(function () {
                // Grid row was removing which was attaching to Note
                if (fromDrFirst != null && fromDrFirst) {
                    Clinical_Medications.medicationsSearch();
                }
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    /* This function will load latest medication by patient ID
       Author: ZeeshanAK | Date: January 19,2015 */
    getLatestMedicationsByPatientId: function (fromDrFirst, hideAlertMessage) {
        Clinical_Medications.getLatestClinicalMedicationsByPatientId_DBCall().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                Clinical_Medications.checkMedicationsExists();
                if (response.MedicationSoapCount > 0) {

                    Clinical_Medications.createMedicationBodyHTMLWithOutReconcile(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', -1, fromDrFirst, hideAlertMessage);
                } else {
                    Clinical_ProgressNote.saveComponentSOAPText('Medications', hideAlertMessage);
                    //Clinical_Medications.noActiveMedicationSoapText(hideAlertMessage);
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }

        });
    },


    /* Reset button to uncheck all selected Medications.
       Author: ZeeshanAK | Date: January 15,2015 */
    resetSelectedMedications: function () {
        $("#" + Clinical_Medications.params.PanelID + " #selectRecordMedications").prop('checked', false);
        $("#" + Clinical_Medications.params.PanelID + " input[name='SelectCheckBoxMedication']:checkbox").prop('checked', false);
    },
    addMedicationsToNotesAndClose: function () {
        $.when(Clinical_Medications.addMedicationsToNotes()).then(function () {
            $("#mainForm  li#CDSAlert").show();
            $.when(ClinicalCDSDetail.showCDSAlert("", Clinical_Medications.params.patientID)).then(function () {
                if (Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote")
                    Clinical_ProgressNote.LoadCDSAlerts();
            });
            if (Clinical_Medications.params && Clinical_Medications.params.ParentCtrl && Clinical_Medications.params.ParentCtrl == "clinicalTabProgressNote") {
                Clinical_Medications.unloadBirthHistory();
            }
        });
    },

    /* This function will add medications to Notes
       Author: ZeeshanAK | Date: January 15,2015 */
    addMedicationsToNotes: function (selectedAttachedMedications, selectedDetachedMedications, hideAlertMessageForPrescription) {
        var dfd = $.Deferred();
        //---------------------------By khaleel Ur Rehman to remove previous soap text--------
        var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
        if (Clinical_ProgressNote.params["TemplateName"] != '')
            CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

        //$(CompnentSelector).find("#clinicalMenu_Medical_Medications").parent().parent().find("section").remove();
        //------------------------------------------------------------------------------------
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        var medicationExists = false;
        if ($("#" + Clinical_Medications.params.PanelID + " #dgvMedications tbody tr").length == 1) {
            if ($("#" + Clinical_Medications.params.PanelID + " #dgvMedications tbody tr td").length > 1) {
                medicationExists = true;
            }
        }
        else if ($("#" + Clinical_Medications.params.PanelID + " #dgvMedications tbody tr").length > 1) {
            medicationExists = true;
        }
        if (!medicationExists) {
            $.when(Clinical_Medications.AppendReviewedSoapText(null)).then(function () {
                Clinical_Medications.noActiveMedicationSoapText();
                var AttachSelectedMedsAndPrsc = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
                var DettachSelectedMedsAndPrsc = Clinical_ProgressNote.DetachedNoteComponentIds.slice();
                //Check for Medications Values
                if (AttachSelectedMedsAndPrsc.join().indexOf("prsc") != -1 || DettachSelectedMedsAndPrsc.join().indexOf("prsc") != -1) {
                    var AttachSelectedprsc = EMRUtility.slicefunc(AttachSelectedMedsAndPrsc, "prsc", 0, -4);
                    var dettachSelectedprsc = EMRUtility.slicefunc(DettachSelectedMedsAndPrsc, "prsc", 0, -4);
                    $.when(Clinical_Prescriptions.addPrescriptionsToNotes(AttachSelectedprsc, dettachSelectedprsc, hideAlertMessageForPrescription)).then(function () {
                        dfd.resolve();
                    });
                }
                else {

                    dfd.resolve();
                }
            });


        }
        else {

            if ($("#" + Clinical_Medications.params.PanelID + " #selectRecordMedications").prop('checked') == true) { // || $("#" + Clinical_Medications.params.PanelID + " #selectRecordMedications").prop('checked') == false

                $("#" + Clinical_Medications.params.PanelID + " #dgvMedications tbody").find('input[type="checkbox"]').each(function () {
                    Clinical_Medications.enableAddMedication(this);
                });
            }
            var AttachedSelectedMedications = [];
            var DettachedSelectedMedications = [];

            if ($("#" + Clinical_Medications.params.PanelID + " #chkReconciledViewOnNote").is(':checked')) {
                AttachedSelectedMedications = Clinical_Medications.medIdsForReconciledView.split(",");
            }

            if ((selectedAttachedMedications != '' && selectedAttachedMedications != null) || (selectedDetachedMedications != '' && selectedDetachedMedications != null)) {
                AttachedSelectedMedications = selectedAttachedMedications;
                DettachedSelectedMedications = selectedDetachedMedications;
            } else if ($("#" + Clinical_Medications.params.PanelID + " #chkReconciledViewOnNote").prop('checked') == false) {
                var AttachSelectedMedsAndPrsc = Clinical_ProgressNote.AttachedNoteComponentIds.slice();
                var DettachSelectedMedsAndPrsc = Clinical_ProgressNote.DetachedNoteComponentIds.slice();
                //Check for Medications Values
                if (AttachSelectedMedsAndPrsc.join().indexOf("prsc") != -1 || DettachSelectedMedsAndPrsc.join().indexOf("prsc") != -1) {
                    var AttachSelectedprsc = EMRUtility.slicefunc(AttachSelectedMedsAndPrsc, "prsc", 0, -4);
                    var dettachSelectedprsc = EMRUtility.slicefunc(DettachSelectedMedsAndPrsc, "prsc", 0, -4);
                    Clinical_Prescriptions.addPrescriptionsToNotes(AttachSelectedprsc, dettachSelectedprsc, hideAlertMessageForPrescription);
                }
                AttachedSelectedMedications = EMRUtility.slicefunc(AttachSelectedMedsAndPrsc, "meds", 0, -4);
                DettachedSelectedMedications = EMRUtility.slicefunc(DettachSelectedMedsAndPrsc, "meds", 0, -4);
            }

            var detachedvalues = DettachedSelectedMedications;
            if (AttachedSelectedMedications != null && AttachedSelectedMedications != '') {
                for (var i = 0; i < Clinical_ProgressNote.AttachedNoteComponentIds.length; i++) {
                    var ALid = Clinical_ProgressNote.AttachedNoteComponentIds[i];
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Medications_Main' + ALid).length != 0) {
                        var index = AttachedSelectedMedications.indexOf(ALid);
                        if (index > -1) {
                            AttachedSelectedMedications.splice(index, 1);
                        }
                    }
                }
            }

            if (DettachedSelectedMedications.join() != null && DettachedSelectedMedications.join() != '') {
                Clinical_Medications.detachMedicationsFromNotes(DettachedSelectedMedications).done(function () {
                    if (AttachedSelectedMedications.join() != null && AttachedSelectedMedications.join() != '') {
                        $.when(Clinical_Medications.attachMedicationsFromNotes(AttachedSelectedMedications)).then(function () {
                            dfd.resolve();
                        });
                    } else {
                        $.when(Clinical_Medications.AppendReviewedSoapText(null)).then(function () {
                            $.when(Clinical_ProgressNote.saveComponentSOAPText('Medications')).then(function () {
                                dfd.resolve();
                            });
                        });
                    }
                });
            }
            else if (AttachedSelectedMedications.join() != null && AttachedSelectedMedications.join() != '') {
                $.when(Clinical_Medications.attachMedicationsFromNotes(AttachedSelectedMedications)).then(function () {
                    dfd.resolve();
                });
            }
            else {
                $.when(Clinical_Medications.AppendReviewedSoapText(null)).then(function () {
                    dfd.resolve();
                });
            }
        }
        return dfd;
    },
    attachMedicationsFromNotes: function (AttachedSelectedMedications) {
        var dfd = $.Deferred();
        Clinical_Medications.getMedicationsInfo(AttachedSelectedMedications.join()).done(function () {
            setTimeout(function () {
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                if (Clinical_Medications.params != null && Clinical_Medications.params.PanelID.indexOf('pnlClinicalMedications') != -1) {
                    Clinical_Medications.medicationsSearch();
                    dfd.resolve();
                }
                else {
                    dfd.resolve();
                }
            }, 5);
        });
        return dfd;
    },
    detachMedicationsFromNotes: function (dettachedMedicationsIds) {
        var dfd = new $.Deferred();
        Clinical_Medications.detachMedicationsFromNotes_DBCall(dettachedMedicationsIds.join()).done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                for (var i = 0; i < dettachedMedicationsIds.length; i++) {
                    var ALid = dettachedMedicationsIds[i];

                    var HeadingNotRemoved = true;
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Medications_Main' + ALid).parent().parent().attr("id") == "AllCurrentMedications") {
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllCurrentMedications').find('section[id*="Cli_Medications_Main"]').length == 1) {
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_CurrentMedications').remove();
                            HeadingNotRemoved = false;
                        }
                    }
                    if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Cli_Medications_Main' + ALid).parent().parent().attr("id") == "AllPastMedications") {
                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllPastMedications').find('section[id*="Cli_Medications_Main"]').length == 1) {
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_PastMedications').remove();
                            HeadingNotRemoved = false;
                        }
                    }
                    if (HeadingNotRemoved) {
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('section[id*="Cli_Medications_Main' + ALid + '"]').remove();
                    }
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
            dfd.resolve('ok');
        });
        return dfd.promise();
    },
    //Author: Abid Ali
    //Date :  02-03-2016
    //Description: To attach Lab Order With Notes
    getMedicationsForReconciledView_DBCall: function (medicationsId) {
        var objData = new Object();
        objData["MedicationIDs"] = medicationsId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_Medications.params.patientID;
        objData["commandType"] = "get_medications_forreconciledview";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Medications");
    },

    /* This function will handle load of Medications for SOAP. It represents service call to API
       Author: ZeeshanAK | Date: January 14,2015 */
    getMedicationsForSOAP_DBCall: function (medicationsId) {
        var objData = new Object();
        objData["MedicationIDs"] = medicationsId;
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["PatientId"] = Clinical_Medications.params.patientID;
        objData["commandType"] = "get_Medications_forsoap";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Medications");
    },

    /* This function will handle detachment of Medications from Notes. It represents service call to API
       Author: ZeeshanAK | Date: January 14,2015 */
    detachMedicationsFromNotes_DBCall: function (medicationsId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["MedicationIDs"] = medicationsId;
        objData["commandType"] = "detach_Medications_from_notes";
        var data = JSON.stringify(objData);
        // serach parameter , class name, command name of class
        return MDVisionService.APIService(data, "MEDICAL", "Medications");
    },

    /* This function will handle the load of Medications by PatientID. It represents service call to API
       Author: ZeeshanAK | Date: January 14,2015 */
    getLatestClinicalMedicationsByPatientId_DBCall: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["UserId"] = globalAppdata["AppUserId"];
        objData["EntityId"] = globalAppdata["SeletedEntityId"];
        objData["commandType"] = "getlatest_medicationsby_patientid";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Medications");
    },

    getReviewedInfoByPatientId_DBCALL: function () {
        var objData = new Object();
        if (Clinical_Notes.params.patientID == "" || Clinical_Notes.params.patientID == "undefined") {
            objData["PatientId"] = 0;
        } else {
            objData["PatientId"] = Clinical_Notes.params.patientID;
        }
        objData["commandType"] = "search_medications";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Medications");
    },

    /* This function will create HTML for the medications to be inserted in Notes
       Author: ZeeshanAK | Date: January 14,2015 */
    createMedicationBodyHTMLWithOutReconcile: function (response, noteHTMLCtrl, medicationsId, fromDrFirst, hideAlertMessage, FromOrderSet) {
        var dfd = $.Deferred();
        Clinical_Medications.checkMedicationsExists();
        if (typeof FromOrderSet != typeof undefined && FromOrderSet != null && FromOrderSet) {
            response.MedicationSoap_JSON = response.MedicationsSoap_JSON;
            response.MedicationSoapCount = response.MedicationsSoapCount;
            response.MedicationReviewSoap_JSON = null;
        }
        if (response.MedicationSoap_JSON != null && response.MedicationSoap_JSON != '') {
            var MedicationsSOAPJSON = JSON.parse(response.MedicationSoap_JSON);
            var medicationReviewSoapJSON = [];
            if (response.MedicationReviewSoap_JSON != null) {
                medicationReviewSoapJSON = JSON.parse(response.MedicationReviewSoap_JSON);
            }

            var $mainDivMedications = $(document.createElement('div'));

            var $mainDivCurrentMedications = $(document.createElement('div'));
            $mainDivCurrentMedications.attr('id', "Section_CurrentMedications");
            $mainDivCurrentMedications.append('<h6 class="text-bold pl-default" style = "color:#468aea">Current Medications</h6>' + "<div id='AllCurrentMedications'></div>");
            var $mainDivPastMedications = $(document.createElement('div'));
            $mainDivPastMedications.attr('id', "Section_PastMedications");
            $mainDivPastMedications.append('<h6 class="text-bold pl-default" style = "color:#bd0e09">Past Medications</h6>' + "<div id='AllPastMedications'></div>");



            if (MedicationsSOAPJSON == null || MedicationsSOAPJSON.length == 0) {
                if (medicationReviewSoapJSON != null && medicationReviewSoapJSON.length != 0) {
                    $.each(medicationReviewSoapJSON, function (index, item) {
                        if (item.ReviewedOn != null && item.ReviewedOn != '') {
                            var dateFormat = item.ReviewedOn
                            var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                            $ListMedications.append("<li >" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                       " on:  " + ReviewedOndateFormat + "</li>");
                        }
                    });
                } else {
                    return "";
                }
            } else {
                if ($(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main0').length != 0) {
                    //$(noteHTMLCtrl + ' #Cli_Medications_Main0').parent().remove();
                    $(noteHTMLCtrl).find("section#Cli_Medications_Main0").remove();
                }
            }

            var $DivNewCurrentMedications = $(document.createElement('div'));
            var $DivNewPastMedications = $(document.createElement('div'));


            var AListId = [];
            var def = [];
            $.each(MedicationsSOAPJSON, function (index, element) {
                var ALid = element.MedicationID;
                var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1, "", 'Clinical_Medications');
                var $SectionBodyMedications = $(document.createElement('section'));
                $SectionBodyMedications.attr('id', "Cli_Medications_Main" + ALid);
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Medication_" + ALid);
                var $ListMedications = $(document.createElement('ul'));


                $ListMedications.attr('class', 'list-unstyled')

                $SectionBodyMedications.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Medication_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Medications_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');
                var duration = "";
                if (element.Duration == '') {
                    duration;
                }
                else if (element.Duration == '0') {
                    duration;
                }
                else if (element.Duration == '1') {
                    duration = " for " + element.Duration + " Day";
                }
                else {
                    duration = " for " + element.Duration + " Days";
                }

                var strDrugDosage = "";
                if (element.Action != "" || element.Dose != "" || element.DoseUnit != "" || element.Routeby != "" || element.DoseTiming != "" || element.DoseOther != "") {
                    strDrugDosage = element.Action + " " + element.Dose + " " + element.DoseUnit + " " + element.Routeby + " " + element.DoseTiming + " " + element.DoseOther + duration;
                    strDrugDosage += element.PatientNotes != "" ? (" (" + element.PatientNotes + ")") : "";
                }
                else {
                    strDrugDosage = element.PatientNotes;
                }

                $ListMedications.append("<li> <strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" + ", " +
                    //(((element.StopDate == '' || element.StopDate == null) || new Date(element.StopDate) > new Date()) ? "" : '<span style = "color:orange;font-weight:bold"> (past) </span>') +
                    strDrugDosage +
                    (element.Quantity == '' ? "" : ", Quantity  " + element.Quantity) + " " +
                     (element.QuantityUnit == "" ? "" : (" " + element.QuantityUnit) + "(s)") +
                                (element.Refill == '' ? "" : ", Refill(s)  " + element.Refill) + " " +
                                ((element.Substitution == null || element.Substitution == "") ? "" : "," + ((element.Substitution).toLowerCase() == 'n' ? ' Dispense as written ' : ' Substitution permitted ')) +
                                (element.PrescribedOn == null || element.PrescribedOn == '' ? "" : ", Prescribed On " + element.PrescribedOn) +
                                (element.ProviderName == null || element.ProviderName == '' ? "" : " by " + element.ProviderName) + "</li>"
            );
                $ListMedications.append((element.Comments == "" ? "" : "<li>Comments: " + element.Comments));

                $DetailsDiv.append($ListMedications);
                $SectionBodyMedications.append($DetailsDiv);
                //if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                if ($(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).length == 0) {
                    AListId.push(ALid);
                    if (!((element.StopDate == '' || element.StopDate == null) || new Date(element.StopDate) > new Date())) {
                        def.push(
                        $.when(Clinical_Medications.RemoveIfExistINAnyOtherMedicationType(noteHTMLCtrl, ALid)).then(function () {
                            $DivNewPastMedications.append($SectionBodyMedications);
                        })
                        );
                    }
                    else {
                        def.push(
                        $.when(Clinical_Medications.RemoveIfExistINAnyOtherMedicationType(noteHTMLCtrl, ALid)).then(function () {
                            $DivNewCurrentMedications.append($SectionBodyMedications);
                        })
                        );
                    }
                    //$mainDivMedications.append($SectionBodyMedications);
                } else {
                    var medicationType = "current";
                    if (!((element.StopDate == '' || element.StopDate == null) || new Date(element.StopDate) > new Date())) {
                        medicationType = "past";
                    }

                    var InNoteWhereIsMedication = "";
                    if ($(noteHTMLCtrl + ' #Section_PastMedications #Cli_Medications_Main' + ALid).length > 0) {
                        InNoteWhereIsMedication = "past";
                    }
                    else if ($(noteHTMLCtrl + ' #Section_CurrentMedications #Cli_Medications_Main' + ALid).length > 0) {
                        InNoteWhereIsMedication = "current";
                    }
                    if (InNoteWhereIsMedication != medicationType) {
                        AListId.push(ALid);
                        if (!((element.StopDate == '' || element.StopDate == null) || new Date(element.StopDate) > new Date())) {
                            def.push(
                            $.when(Clinical_Medications.RemoveIfExistINAnyOtherMedicationType(noteHTMLCtrl, ALid)).then(function () {
                                $DivNewPastMedications.append($SectionBodyMedications);
                            })
                            );
                        }
                        else {
                            def.push(
                            $.when(Clinical_Medications.RemoveIfExistINAnyOtherMedicationType(noteHTMLCtrl, ALid)).then(function () {
                                $DivNewCurrentMedications.append($SectionBodyMedications);
                            })
                            );
                        }
                    }
                    else {
                        var CommentHTML = "";
                        var CommentsID = $(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid + ' ul li:Last').attr('id');
                        if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                            CommentHTML = $(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid + ' ul li:Last').get(0).outerHTML;
                        }
                        $(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid).html($SectionBodyMedications.html());
                        $(noteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_Main' + ALid + ' ul').append(CommentHTML);;
                    }
                }


            });

            $.when.apply($, def).done(function ($n) {

                if (AListId.join(",") != "") {
                    medicationsId = AListId.join(",");
                }

                if ($DivNewPastMedications.html() != '' || $DivNewCurrentMedications.html() != '') {
                    if ($DivNewCurrentMedications.html() != '') {
                        if ($(noteHTMLCtrl).find('#Section_CurrentMedications').length == 0) {
                            $mainDivMedications.append($mainDivCurrentMedications);
                            $mainDivMedications.find('#AllCurrentMedications').append($DivNewCurrentMedications);
                        }
                        else {
                            $mainDivMedications.append($(noteHTMLCtrl).find('#Section_CurrentMedications')[0].outerHTML);
                            $mainDivMedications.find('#AllCurrentMedications').append($DivNewCurrentMedications);
                        }
                    }
                    else {
                        if ($(noteHTMLCtrl).find('#Section_CurrentMedications').length != 0) {
                            $mainDivMedications.append($(noteHTMLCtrl).find('#Section_CurrentMedications')[0].outerHTML);
                        }
                    }

                    if ($DivNewPastMedications.html() != '') {
                        if ($(noteHTMLCtrl).find('#Section_PastMedications').length == 0) {
                            $mainDivMedications.append($mainDivPastMedications);
                            $mainDivMedications.find('#AllPastMedications').append($DivNewPastMedications);
                        }
                        else {
                            $mainDivMedications.append($(noteHTMLCtrl).find('#Section_PastMedications')[0].outerHTML);
                            $mainDivMedications.find('#AllPastMedications').append($DivNewPastMedications);
                        }
                    }
                    else {
                        if ($(noteHTMLCtrl).find('#Section_PastMedications').length != 0) {
                            $mainDivMedications.append($(noteHTMLCtrl).find('#Section_PastMedications')[0].outerHTML);
                        }
                    }

                    if (medicationReviewSoapJSON != null && medicationReviewSoapJSON.length != 0) {
                        $.each(medicationReviewSoapJSON, function (index, item) {
                            if (item.ReviewedOn != null && item.ReviewedOn != '') {
                                var dateFormat = item.ReviewedOn
                                var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                                $mainDivMedications.append("<section id='Cli_Medications_ReviewByMedication'><li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                           " on:  " + ReviewedOndateFormat + "</li></section>");
                            }
                        });
                    }
                    $.when(Clinical_Medications.updateMedicationsHtml($mainDivMedications.html(), medicationsId, noteHTMLCtrl, fromDrFirst, false, false, null, hideAlertMessage)).then(function () {
                        dfd.resolve('ok');
                    });
                }
                else {
                    if (medicationReviewSoapJSON != null && medicationReviewSoapJSON.length != 0) {
                        $.each(medicationReviewSoapJSON, function (index, item) {
                            if (item.ReviewedOn != null && item.ReviewedOn != '') {
                                var dateFormat = item.ReviewedOn
                                var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                                $mainDivMedications.append("<section id='Cli_Medications_ReviewByMedication'><li>" + (item.ReviewedBy == '' ? "" : "Medications reviewed by " + item.ReviewedBy) +
                           " on:  " + ReviewedOndateFormat + "</li></section>");
                            }
                        });
                    }

                    $.when(Clinical_Medications.updateMedicationsHtml('', medicationsId, noteHTMLCtrl, null, false, false, null, hideAlertMessage)).then(function () {
                        Clinical_ProgressNote.saveComponentSOAPText('Medications', hideAlertMessage);
                        dfd.resolve('ok');
                    });
                }
            });
        }
        return dfd;
    },


    AppendReviewedSoapText: function (hideAlertMessage) {
        var dfd = $.Deferred();

        if ($("#" + Clinical_Medications.params.PanelID + " #reviewedByTop").text() != "") {
            Clinical_Medications.checkMedicationsExists();
            var NoteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
            var $mainDivMedications = $(document.createElement('div'));
            $.when(reviewedString = Clinical_Medications.GetReviewedMedicationInfo()).then(function () {
                if (reviewedString.response != "") {
                    if ($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_ReviewByMedication').length == 0) {
                        $mainDivMedications.append("<section id='Cli_Medications_ReviewByMedication'><li>" + reviewedString.response + "</li></section>");
                    }
                    else {
                        $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#Cli_Medications_ReviewByMedication').remove();
                        $mainDivMedications.append("<section id='Cli_Medications_ReviewByMedication'><li>" + reviewedString.response + "</li></section>");
                    }
                }
                var whereAppend = "";
                var mainDivHTML = $mainDivMedications.html();
                var FinalHTML = "";
                if ($('#Section_CurrentMedications').length > 0 && $('#Section_PastMedications').length > 0) {
                    whereAppend = "CurrentPastMedications";
                }
                else if ($('#Section_CurrentMedications').length > 0) {
                    whereAppend = "CurrentMedications";
                }
                else if ($('#Section_PastMedications').length > 0) {
                    whereAppend = "PastMedications";
                }
                if (whereAppend != "") {
                    if (whereAppend == "CurrentPastMedications") {
                        $("#Section_PastMedications").after(mainDivHTML);
                        FinalHTML += $("#Section_CurrentMedications")[0].outerHTML;
                        FinalHTML += $("#Section_PastMedications")[0].outerHTML;
                        FinalHTML += mainDivHTML;
                    }
                    if (whereAppend == "CurrentMedications") {
                        $("#Section_CurrentMedications").after(mainDivHTML);
                        FinalHTML += $("#Section_CurrentMedications")[0].outerHTML;
                        FinalHTML += mainDivHTML;
                    }
                    if (whereAppend == "PastMedications") {
                        $("#Section_PastMedications").after(mainDivHTML);
                        FinalHTML += $("#Section_PastMedications")[0].outerHTML;
                        FinalHTML += mainDivHTML;
                    }
                }
                //End//04/01/2015//Ahmad Raza//Delete icon not working in ProgressNote issue fixed
                if (FinalHTML != '') {
                    $.when(Clinical_Medications.updateMedicationsHtml(FinalHTML, "", NoteHTMLCtrl, hideAlertMessage)).then(function () {
                        $.when(Clinical_ProgressNote.saveComponentSOAPText('Medications', hideAlertMessage)).then(function () {
                            dfd.resolve();
                        });
                    });

                } else {
                    $.when(Clinical_Medications.updateMedicationsHtml('', "", NoteHTMLCtrl, hideAlertMessage)).then(function () {
                        Clinical_ProgressNote.saveComponentSOAPText('Medications', hideAlertMessage);
                        dfd.resolve();
                    });
                }
            });
        }
        else {
            dfd.resolve();
        }

        return dfd;
    },


    RemoveIfExistINAnyOtherMedicationType: function (NoteHTMLCtrl, PLid) {
        var dfd = $.Deferred();
        var MainDivArray = ["AllCurrentMedications", "AllPastMedications"];
        var found = false;
        $.each(MainDivArray, function (index, item) {
            if ($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#' + item + ' #Cli_Medications_Main' + PLid).length != 0) {
                found = true;
                return;
            }
        });
        if (found) {
            var Ids = [];
            Ids.push(PLid);
            $.when(Clinical_Medications.detachMedicationsFromNotes(Ids)).then(function () {
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd.promise();
    },



    //This Function is used to create SOAP html and append it to  Progress note
    //createMedicationBodyHTMLNew
    createMedicationBodyHTML: function (response, NoteHTMLCtrl, ImmunizationId, bMedReconciled) {
        Clinical_Medications.checkMedicationsExists();
        if (response.MedicationSoap_JSON != null && response.MedicationSoap_JSON != '') {
            var MedicationsSOAPJSON = JSON.parse(response.MedicationSoap_JSON);



            var $mainDivVital = $(document.createElement('div'));

            var $mainDivCurrent = $(document.createElement('div'));
            $mainDivCurrent.attr('id', "Section_MedicationCurrent");
            //$mainDivCurrent.css('padding', "8px");
            $mainDivCurrent.append('<h6 style="color: #4B0082;" class="text-bold">Current Medications</h6> ' + "<div id='AllCurrentMedication'></div>");

            var $mainDivStop = $(document.createElement('div'));
            $mainDivStop.attr('id', "Section_MedicationStop");
            //$mainDivStop.css('padding', "8px");
            $mainDivStop.append('<h6 style="color: #4B0082;" class="text-bold">Discontinued Medications</h6>' + "<div id='AllStopMedication'></div>");//kr
            //--------------------------------
            var $mainDivPrescribed = $(document.createElement('div'));
            $mainDivPrescribed.attr('id', "Section_MedicationPrescribed");
            //$mainDivPrescribed.css('padding', "8px");
            $mainDivPrescribed.append('<h6 class="text-bold" style="color: #4B0082;">Prescribed Medications</h6>' + "<div id='AllPrescribedMedication'></div>");

            var $mainDivReconciled = $(document.createElement('div'));
            $mainDivReconciled.attr('id', "Section_MedicationReconciled");
            //$mainDivReconciled.css('padding', "8px");
            $mainDivReconciled.append('<h6 class="text-bold" style="color: #4B0082;">Reconciled Medications</h6>' + "<div id='AllReconciledMedication'></div>");

            var $mainDivRenew = $(document.createElement('div'));
            $mainDivRenew.attr('id', "Section_MedicationRenew");
            //$mainDivRenew.css('padding', "8px");
            $mainDivRenew.append('<h6 class="text-bold" style="color: #4B0082;">Renewed Medications</h6>' + "<div id='AllRenewedMedication'></div>");
            //------------------------------------------

            var $DivNewCurrent = $(document.createElement('div'));
            var $DivNewStop = $(document.createElement('div'));
            var $DivNewPrescribed = $(document.createElement('div'));
            var $DivNewReconciled = $(document.createElement('div'));
            var $DivNewRenew = $(document.createElement('div'));

            if (MedicationsSOAPJSON == null || MedicationsSOAPJSON.length == 0) {
                Clinical_ProgressNote.saveComponentSOAPText('Medications');
                return "";
            }

            var PListId = [];
            var arrMedReconciledIds = [];
            var arrlastVisitReconciledIds = [];
            var arrRenewedMedsId = [];
            if (bMedReconciled == true) {
                arrlastVisitReconciledIds = Clinical_Medications.lastVisitReconciledIds.split(",")
            }
            $.each(MedicationsSOAPJSON, function (index, element) {
                var color = "";
                var $infoButtonrow = Clinical_InfoButtonView.GenerateInfoLink(element.NDCID, 'clinicalTabProgressNote', 1, "", 'Clinical_Medications');
                var duration = "";
                if (element.Duration == '') {
                    duration;
                }
                else if (element.Duration == '0') {
                    duration;
                }
                else if (element.Duration == '1') {
                    duration = " for " + element.Duration + " Day";
                }
                else {
                    duration = " for " + element.Duration + " Days";
                }
                var CurDate = new Date();
                if (element.StopDate != "") {
                    var StopDate = new Date(element.StopDate);
                }


                var PLid = element.MedicationID;
                var isLastVisitReconciledMed = $.inArray(PLid, arrlastVisitReconciledIds) > -1 ? true : false;

                var $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Medications_Main" + PLid);
                $SectionBodyVital.attr('class', "Cli_Medications_Main" + PLid);
                $SectionBodyVital.attr('name', "Section_MedicationCurrent");
                var $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Medication_" + PLid);
                var $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Medication_" + PLid + '"  medType="Current_' + PLid + '" ><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Medications_Main" + PLid + '" medType="Current_' + PLid + '" ><i class="fa fa-times"></i></a></div> ');

                //$SectionBodyMedications.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Medication_" + ALid + '"><i class="fa fa-edit"></i></a>' +
                //'<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Medications_Main" + ALid + '"  ><i class="fa fa-times"></i></a></div> ');

                //kr'Patient underwent <ProcedureCode ProcedureName> based on the following assessment: <Diagnosis> from <From> to <To>. Comments: <Comments>'
                if (element.StopDate == "" || StopDate > CurDate) {//for current
                    $ListVital.append("<li>" +
                                        "<strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" + ", " +
                        //$ListMedications.html("<div id='current'><strong>Current Medications </strong><li> <strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" +
                            element.Action + " " + element.Dose + " " + element.DoseUnit + " " + element.Routeby + " " + element.DoseTiming + " " + element.DoseOther +
                            duration 
                            //(element.StartDate == '' ? "" : "</br>Started on  " + element.StartDate.split(' ')[0])
                                        );



                    $DetailsDiv.append($ListVital);
                    $SectionBodyVital.append($DetailsDiv);

                    if ($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllCurrentMedication #Cli_Medications_Main' + PLid).length == 0) {
                        PListId.push(PLid);
                        if (element.StopDate == "" || StopDate > CurDate) {
                            if (($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().find('[id*="Cli_Medications_Main"]')).length == 1) {
                                $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().parent().remove();
                            }
                            else {
                                $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).remove();
                            }
                            //Clinical_Medications.detachMedicationsFromNotes_DBCall(PLid);
                            $DivNewCurrent.append($SectionBodyVital);
                        }

                    } else {

                        var CommentHTML = "";
                        var CommentsID = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllCurrentMedication #Cli_Medications_Main' + PLid + ' ul li:Last').attr('id');
                        if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                            CommentHTML = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllCurrentMedication #Cli_Medications_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                        }
                        $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllCurrentMedication #Cli_Medications_Main' + PLid).html($SectionBodyVital.html());
                        $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllCurrentMedication #Cli_Medications_Main' + PLid + ' ul').append(CommentHTML);
                    }
                }

                if (isLastVisitReconciledMed == false) {

                    $SectionBodyVital = $(document.createElement('section'));
                    $SectionBodyVital.attr('id', "Cli_Medications_Main" + PLid);
                    $SectionBodyVital.attr('class', "Cli_Medications_Main" + PLid);
                    $SectionBodyVital.attr('name', "Section_MedicationStop");
                    $DetailsDiv = $(document.createElement('div'));
                    $DetailsDiv.attr('id', "Cli_Medication_" + PLid);
                    $ListVital = $(document.createElement('ul'));

                    $ListVital.attr('class', 'list-unstyled')

                    $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Medication_" + PLid + '" medType="Stop_' + PLid + '" ><i class="fa fa-edit"></i></a>' +
                        '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Medications_Main" + PLid + '" medType="Stop_' + PLid + '" ><i class="fa fa-times"></i></a></div> ');
                    if (StopDate <= CurDate) {
                        $ListVital.append("<li>" +
                                            "<strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" + ", " +
                            //$ListMedications.html("<div id='current'><strong>Current Medications </strong><li> <strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" +
                                //(((element.StopDate == '' || element.StopDate == null) || new Date(element.StopDate) > new Date()) ? "" : '<span style = "color:orange;font-weight:bold"> (past) </span>') +
                                element.Action + " " + element.Dose + " " + element.DoseUnit + " " + element.Routeby + " " + element.DoseTiming + " " + element.DoseOther +
                                 duration 
                                //(element.StartDate == '' ? "" : "</br>Started on  " + element.StartDate.split(' ')[0]) +
                                //(element.StopDate == '' ? "" : "</br>Discontinued on  " + element.StopDate.split(' ')[0])
                                            );


                        $DetailsDiv.append($ListVital);
                        $SectionBodyVital.append($DetailsDiv);

                        if ($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllStopMedication #Cli_Medications_Main' + PLid).length == 0) {
                            PListId.push(PLid);
                            if (StopDate <= CurDate) {
                                if (($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().find('[id*="Cli_Medications_Main"]')).length == 1) {
                                    $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().parent().remove();
                                }
                                else {
                                    $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).remove();
                                }
                                //Clinical_Medications.detachMedicationsFromNotes_DBCall(PLid);
                                $DivNewStop.append($SectionBodyVital);
                            }

                        } else {

                            var CommentHTML = "";
                            var CommentsID = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllStopMedication #Cli_Medications_Main' + PLid + ' ul li:Last').attr('id');
                            if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                                CommentHTML = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllStopMedication #Cli_Medications_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                            }
                            $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllStopMedication #Cli_Medications_Main' + PLid).html($SectionBodyVital.html());
                            $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllStopMedication #Cli_Medications_Main' + PLid + ' ul').append(CommentHTML);
                        }

                    }
                }

                if (isLastVisitReconciledMed == false) {
                    $SectionBodyVital = $(document.createElement('section'));
                    $SectionBodyVital.attr('id', "Cli_Medications_Main" + PLid);
                    $SectionBodyVital.attr('class', "Cli_Medications_Main" + PLid);
                    $SectionBodyVital.attr('name', "Section_MedicationRenew");
                    $DetailsDiv = $(document.createElement('div'));
                    $DetailsDiv.attr('id', "Cli_Medication_" + PLid);
                    $ListVital = $(document.createElement('ul'));

                    $ListVital.attr('class', 'list-unstyled')

                    $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Medication_" + PLid + '" medType="Renew_' + PLid + '" ><i class="fa fa-edit"></i></a>' +
                        '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Medications_Main" + PLid + '"  medType="Renew_' + PLid + '" ><i class="fa fa-times"></i></a></div> ');
                    if (element.Renew == "True") {
                        arrRenewedMedsId.push(PLid);
                        $ListVital.append("<li>" +
                                            "<strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" + ", " +
                            //$ListMedications.html("<div id='current'><strong>Current Medications </strong><li> <strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" +
                                //(((element.StopDate == '' || element.StopDate == null) || new Date(element.StopDate) > new Date()) ? "" : '<span style = "color:orange;font-weight:bold"> (past) </span>') +
                                element.Action + " " + element.Dose + " " + element.DoseUnit + " " + element.Routeby + " " + element.DoseTiming + " " + element.DoseOther +
                                 duration +
                                (element.Quantity == '' ? "" : " Quantity " + element.Quantity) +
                                (element.QuantityUnit == "" ? "" : " " + element.QuantityUnit) + "(s)" +
                                (element.Refill == '' ? "" : " Refill(s) " + element.Refill) +
                                //" " + element.Substitution +
                                ((element.Substitution == null || element.Substitution == "") ? "" : ", " + ((element.Substitution).toLowerCase() == 'n' ? ' Dispense as written ' : ' Substitution permitted ')) +
                                (element.PrescribedOn == '' ? "" : " , Prescribed on " + element.PrescribedOn.split(' ')[0]) +
                                (element.ModifiedOn == '' ? "" : " Renewed on " + element.ModifiedOn.split(' ')[0]) +
                                (element.ProviderName == '' ? "" : " by " + element.ProviderName)
                                //(element.ModifiedOn == '' ? "" : "</br>Renewed on:  " + element.ModifiedOn.split(' ')[0])
                                            );


                        $DetailsDiv.append($ListVital);
                        $SectionBodyVital.append($DetailsDiv);

                        if ($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllRenewedMedication #Cli_Medications_Main' + PLid).length == 0) {
                            PListId.push(PLid);
                            if (element.Renew == "True") {
                                if (($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().find('[id*="Cli_Medications_Main"]')).length == 1) {
                                    $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().parent().remove();
                                }
                                else {
                                    $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).remove();
                                }
                                //Clinical_Medications.detachMedicationsFromNotes_DBCall(PLid);
                                $DivNewRenew.append($SectionBodyVital);
                            }

                        } else {

                            var CommentHTML = "";
                            var CommentsID = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllRenewedMedication #Cli_Medications_Main' + PLid + ' ul li:Last').attr('id');
                            if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                                CommentHTML = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllRenewedMedication #Cli_Medications_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                            }
                            $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllRenewedMedication #Cli_Medications_Main' + PLid).html($SectionBodyVital.html());
                            $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllRenewedMedication #Cli_Medications_Main' + PLid + ' ul').append(CommentHTML);
                        }
                    }
                }

                if (isLastVisitReconciledMed == false && $.inArray(PLid, arrRenewedMedsId) < 0) {

                    $SectionBodyVital = $(document.createElement('section'));
                    $SectionBodyVital.attr('id', "Cli_Medications_Main" + PLid);
                    $SectionBodyVital.attr('class', "Cli_Medications_Main" + PLid);
                    $SectionBodyVital.attr('name', "Section_MedicationPrescribed");
                    $DetailsDiv = $(document.createElement('div'));
                    $DetailsDiv.attr('id', "Cli_Medication_" + PLid);
                    $ListVital = $(document.createElement('ul'));

                    $ListVital.attr('class', 'list-unstyled')

                    $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Medication_" + PLid + '" medType="Prescrip_' + PLid + '"><i class="fa fa-edit"></i></a>' +
                        '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Medications_Main" + PLid + '" medType="Prescrip_' + PLid + '" ><i class="fa fa-times"></i></a></div> ');
                    if (element.PrescriptionRcopiaID != null && element.PrescriptionRcopiaID != "") {
                        $ListVital.append("<li>" +
                                            "<strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" + ", " +
                            //$ListMedications.html("<div id='current'><strong>Current Medications </strong><li> <strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" +
                                //(((element.StopDate == '' || element.StopDate == null) || new Date(element.StopDate) > new Date()) ? "" : '<span style = "color:orange;font-weight:bold"> (past) </span>') +
                                element.Action + " " + element.Dose + " " + element.DoseUnit + " " + element.Routeby + " " + element.DoseTiming + " " + element.DoseOther +
                                 duration +
                                 (element.Quantity == '' ? "" : " Quantity " + element.Quantity) +
                                 (element.QuantityUnit == "" ? "" : " " + element.QuantityUnit) + "(s)" +
                                 (element.Refill == '' ? "" : " Refill(s) " + element.Refill + " ") +
                                 //element.Substitution +
                                 ((element.Substitution == null || element.Substitution == "") ? "" : ", " + ((element.Substitution).toLowerCase() == 'n' ? ' Dispense as written ' : ' Substitution permitted ')) +
                                 (element.PrescribedOn == '' ? "" : " , Prescribed on " + element.PrescribedOn.split(' ')[0]) +
                                 (element.ProviderName == '' ? "" : " by " + element.ProviderName)
                                //(element.StartDate == '' ? "" : "</br>Started on:  " + element.StartDate.split(' ')[0])
                                            );

                        $DetailsDiv.append($ListVital);
                        $SectionBodyVital.append($DetailsDiv);

                        if ($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllPrescribedMedication #Cli_Medications_Main' + PLid).length == 0) {
                            PListId.push(PLid);
                            if (element.PrescriptionRcopiaID != null && element.PrescriptionRcopiaID != "") {
                                if (($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().find('[id*="Cli_Medications_Main"]')).length == 1) {
                                    $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().parent().remove();
                                }
                                else {
                                    $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).remove();
                                }
                                //Clinical_Medications.detachMedicationsFromNotes_DBCall(PLid);
                                $DivNewPrescribed.append($SectionBodyVital);
                            }

                        } else {

                            var CommentHTML = "";
                            var CommentsID = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllPrescribedMedication #Cli_Medications_Main' + PLid + ' ul li:Last').attr('id');
                            if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                                CommentHTML = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllPrescribedMedication #Cli_Medications_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                            }
                            $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllPrescribedMedication #Cli_Medications_Main' + PLid).html($SectionBodyVital.html());
                            $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllPrescribedMedication #Cli_Medications_Main' + PLid + ' ul').append(CommentHTML);
                        }
                    }

                }

                $SectionBodyVital = $(document.createElement('section'));
                $SectionBodyVital.attr('id', "Cli_Medications_Main" + PLid);
                $SectionBodyVital.attr('class', "Cli_Medications_Main" + PLid);
                $SectionBodyVital.attr('name', "Section_MedicationReconciled");
                $DetailsDiv = $(document.createElement('div'));
                $DetailsDiv.attr('id', "Cli_Medication_" + PLid);
                $ListVital = $(document.createElement('ul'));

                $ListVital.attr('class', 'list-unstyled')

                $SectionBodyVital.append(' <div class="pull-right hidden"><a href="javascript:void(0);" class=" btn btn-link btn-xs btnPNC_Edit" title="Edit" name="' + "Cli_Medication_" + PLid + '"  medType="Reconciled_' + PLid + '"><i class="fa fa-edit"></i></a>' +
                    '<a href="javascript:void(0);" class=" btn btn-link btn-xs closeBtn btnPNC_Remove" title="Remove" name="' + "Cli_Medications_Main" + PLid + '"   medType="Reconciled_' + PLid + '"><i class="fa fa-times"></i></a></div> ');

                if ((element.StopDate == "" || StopDate > CurDate) || (element.Renew == "1") || (element.PrescriptionRcopiaID != null && element.PrescriptionRcopiaID != "")) {
                    arrMedReconciledIds.push(element.MedicationID);
                    $ListVital.append("<li>" +
                                        "<strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" + ", " +
                                            //$ListMedications.html("<div id='current'><strong>Current Medications </strong><li> <strong>" + element.MedicationName + " " + $infoButtonrow + " </strong>" +
                            //(((element.StopDate == '' || element.StopDate == null) || new Date(element.StopDate) > new Date()) ? "": '<span style = "color:orange;font-weight:bold"> (past) </span>') +
                            element.Action + " " + element.Dose + " " + element.DoseUnit + " " + element.Routeby + " " + element.DoseTiming + " " + element.DoseOther +
                             duration 
                            //(element.StartDate == '' ? "" : "</br>Started on  " + element.StartDate.split(' ')[0])
                                        );


                    $DetailsDiv.append($ListVital);
                    $SectionBodyVital.append($DetailsDiv);

                    if ($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllReconciledMedication #Cli_Medications_Main' + PLid).length == 0) {
                        PListId.push(PLid);
                        if ((element.StopDate == "" || StopDate > CurDate) || (element.Renew == "1") || (element.PrescriptionRcopiaID != null && element.PrescriptionRcopiaID != "")) {
                            if (($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().find('[id*="Cli_Medications_Main"]')).length == 0) {
                                $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().parent().remove();
                            }
                            if (($(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().find('[id*="Cli_Medications_Main"]')).length == 1) {
                                $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).parent().parent().parent().remove();
                            }
                            else {
                                $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('.Cli_Medications_Main' + PLid).remove();
                            }
                            //Clinical_Medications.detachMedicationsFromNotes_DBCall(PLid);
                            $DivNewReconciled.append($SectionBodyVital);
                        }

                    } else {

                        var CommentHTML = "";
                        var CommentsID = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllReconciledMedication #Cli_Medications_Main' + PLid + ' ul li:Last').attr('id');
                        if (CommentsID != null && CommentsID.indexOf("Comments") >= 0) {
                            CommentHTML = $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllReconciledMedication #Cli_Medications_Main' + PLid + ' ul li:Last').get(0).outerHTML;
                        }
                        $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllReconciledMedication #Cli_Medications_Main' + PLid).html($SectionBodyVital.html());
                        $(NoteHTMLCtrl + ' clinical_medications').parent().parent().find('#AllReconciledMedication #Cli_Medications_Main' + PLid + ' ul').append(CommentHTML);
                    }
                }



            });
            //if (PListId.join(",") != "") {
            //    ImmunizationId = PListId.join(",");
            //}
            if ($DivNewCurrent.html() != '' || $DivNewStop.html() != '' || $DivNewPrescribed.html() != '' || $DivNewReconciled.html() != '') {//repeat || operator
                if ($DivNewCurrent.html() != '') {// if else repeat...
                    if ($(NoteHTMLCtrl).find('#Section_MedicationCurrent').length == 0) {
                        $mainDivVital.append($mainDivCurrent);
                        $mainDivVital.find('#AllCurrentMedication').append($DivNewCurrent);
                    }
                    else {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationCurrent')[0].outerHTML);
                        $mainDivVital.find('#AllCurrentMedication').append($DivNewCurrent);
                    }
                }
                else {
                    if ($(NoteHTMLCtrl).find('#Section_MedicationCurrent').length != 0) {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationCurrent')[0].outerHTML);
                    }
                }
                if ($DivNewStop.html() != '') {
                    if ($(NoteHTMLCtrl).find('#Section_MedicationStop').length == 0) {
                        $mainDivVital.append($mainDivStop);
                        $mainDivVital.find('#AllStopMedication').append($DivNewStop);
                    }
                    else {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationStop')[0].outerHTML);
                        $mainDivVital.find('#AllStopMedication').append($DivNewStop);
                    }
                } else {
                    if ($(NoteHTMLCtrl).find('#Section_MedicationStop').length != 0) {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationStop')[0].outerHTML);
                    }
                }
                //------------------------------
                if ($DivNewPrescribed.html() != '') {
                    if ($(NoteHTMLCtrl).find('#Section_MedicationPrescribed').length == 0) {
                        $mainDivVital.append($mainDivPrescribed);
                        $mainDivVital.find('#AllPrescribedMedication').append($DivNewPrescribed);
                    }
                    else {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationPrescribed')[0].outerHTML);
                        $mainDivVital.find('#AllPrescribedMedication').append($DivNewPrescribed);
                    }
                }
                else {
                    if ($(NoteHTMLCtrl).find('#Section_MedicationPrescribed').length != 0) {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationPrescribed')[0].outerHTML);
                    }
                }

                if ($DivNewRenew.html() != '') {
                    if ($(NoteHTMLCtrl).find('#Section_MedicationRenew').length == 0) {
                        $mainDivVital.append($mainDivRenew);
                        $mainDivVital.find('#AllRenewedMedication').append($DivNewRenew);
                    }
                    else {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationRenew')[0].outerHTML);
                        $mainDivVital.find('#AllRenewedMedication').append($DivNewRenew);
                    }
                }
                else {
                    if ($(NoteHTMLCtrl).find('#Section_MedicationRenew').length != 0) {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationRenew')[0].outerHTML);
                    }
                }

                if ($DivNewReconciled.html() != '') {
                    if ($(NoteHTMLCtrl).find('#Section_MedicationReconciled').length == 0) {
                        $mainDivVital.append($mainDivReconciled);
                        $mainDivVital.find('#AllReconciledMedication').append($DivNewReconciled);
                    }
                    else {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationReconciled')[0].outerHTML);
                        $mainDivVital.find('#AllReconciledMedication').append($DivNewReconciled);
                    }
                }
                else {
                    if ($(NoteHTMLCtrl).find('#Section_MedicationReconciled').length != 0) {
                        $mainDivVital.append($(NoteHTMLCtrl).find('#Section_MedicationReconciled')[0].outerHTML);
                    }
                }

                var MedReconciledId = arrMedReconciledIds.join(","); //arr.join(", ")

                //------------------------------
                Clinical_Medications.updateMedicationsHtml($mainDivVital.html(), ImmunizationId, NoteHTMLCtrl, null, true, bMedReconciled, MedReconciledId);
            } else {
                Clinical_Medications.updateMedicationsHtml('', ImmunizationId, NoteHTMLCtrl, null, true, false, "");
                Clinical_ProgressNote.saveComponentSOAPText('Medications');
            }

        }
    },

    /* This Function will check, if medications Soap is already attached in Progress note, if medications is not attached than it will create main divs to attach medications
       Author: ZeeshanAK | Date: January 14,2015 */
    checkMedicationsExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_medications').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #SubjectiveNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="MedicationsComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_medications title="Medications"  id="' + this.id + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'Medications\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Medications">Medications</a> ' +
                       '<a onclick="Clinical_ProgressNote.Edit_ComponentAttached(this,\'Medications\');" class=" btn btn-link btn-link-print btn-xs hidden btnPNC_Edit" title="Edit" name=""><i class="fa fa-edit"></i></a>' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Medications\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_medications> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).find('.btnPNC_Edit').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).find('.btnPNC_Edit').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_medications').parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($('#' +Clinical_ProgressNote.params["PanelID"]+ ' #hfNotesId').val());
    },

    /* This Function will update the HTML for Medications
      Author: ZeeshanAK | Date: January 14,2015 */
    updateMedicationsHtml: function (medicationsHTML, medicationsID, noteHTMLCtrl, fromDrFirst, isReconcileView, bMedReconciled, MedReconciledId, hideAlertMessage) {
        var dfd = $.Deferred();
        $(noteHTMLCtrl + ' clinical_medications').parent().parent().addClass('initialVisitBody');

        if (medicationsHTML != '') {
            var divMedication = $(noteHTMLCtrl + ' clinical_medications a:nth-child(1)')[0].outerHTML + $(noteHTMLCtrl + ' clinical_medications a:nth-child(2)')[0].outerHTML + $(noteHTMLCtrl + ' clinical_medications a:nth-child(3)')[0].outerHTML;
            $(noteHTMLCtrl + ' clinical_medications').html(divMedication);
            $(noteHTMLCtrl).find(' #Section_CurrentMedications').remove();
            $(noteHTMLCtrl).find(' #Section_PastMedications').remove();
            $(noteHTMLCtrl).find(' #Section_MedicationPrescribed').remove();
            $(noteHTMLCtrl).find(' #Section_MedicationReconciled').remove();
            $(noteHTMLCtrl).find(' #Cli_Medications_ReviewByMedication').remove();
            $(medicationsHTML).insertAfter($(noteHTMLCtrl + ' clinical_medications').parent());

            if (bMedReconciled != true) {
                $(noteHTMLCtrl).find(' #Section_MedicationCurrent').remove();
                $(noteHTMLCtrl).find(' #Section_MedicationStop').remove();
                $(noteHTMLCtrl).find(' #Section_MedicationPrescribed').remove();
                $(noteHTMLCtrl).find(' #Section_MedicationReconciled').remove();
            }
        }


        //Binding Hovering and onClick functions to Progress Note HTML
        Clinical_ProgressNote.ProgressnoteHtmlHoverEvents();
        if (medicationsHTML != '' && medicationsID != null && medicationsID != '' && medicationsID != '0') {
            $.when(Clinical_Medications.attachMedicationFromNotes(medicationsID, fromDrFirst, bMedReconciled, MedReconciledId, hideAlertMessage)).then(function () {
                dfd.resolve();
            });
        }
        else {
            dfd.resolve();
        }
        return dfd;
    },

    /* This Function detach Medications From progress note
      Author: ZeeshanAK | Date: January 14,2015 */
    detachMedicationsComponent: function (componentName, isUpdate, ComponentRemove) {

        var Clinical_MedicationIds = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_medications').parent().parent().find('section[id*="Cli_Medications_Main"]').map(function () {
            return this.id.replace("Cli_Medications_Main", "");
        }).get().join(',');
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .MedicationsComponent').attr('NoteComponentId');

        if (ComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_medications').parent().parent().addClass('hidden');
                    $('#' +Clinical_ProgressNote.params["PanelID"]+ ' #InitialOfficeVisit #ProgressnoteHTML .MedicationsComponent div#Section_CurrentMedications, div#Section_PastMedications').remove();
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Medications', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Medications']").remove();
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_medications').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Medications']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_medications').parent().parent().addClass('hidden');
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .MedicationsComponent div#Section_CurrentMedications, div#Section_PastMedications').remove();
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Medications', true))
                }
                else {
                    if (NoteComponentId && NoteComponentId != "NCDummyId")
                        promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                    else {
                        var def = $.Deferred();
                        promise.push(def);
                        def.resolve();
                    }
                }
                $.when.apply($, promise).done(function () {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Medications']").remove();
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_medications').parent().parent().remove();
                    utility.DisplayMessages('Successfully Deleted', 1);
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_medications').parent().parent().find('section[id*="Cli_Medications_Main"]').remove();
        }

        if (Clinical_MedicationIds == "" || Clinical_MedicationIds == "undefined") {
            var promise = [];
            if (Clinical_ProgressNote.params["TemplateName"]) {
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_medications').parent().parent().addClass('hidden');
                $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .MedicationsComponent div#Section_CurrentMedications, div#Section_PastMedications').remove();
                promise.push(Clinical_ProgressNote.saveComponentSOAPText('Medications', true))
            }
            else {
                if (NoteComponentId && NoteComponentId != "NCDummyId")
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                else {
                    var def = $.Deferred();
                    promise.push(def);
                    def.resolve();
                }
            }
            $.when.apply($, promise).done(function () {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Medications']").remove();
                if (Clinical_ProgressNote.params["TemplateName"] == "")
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_medications').parent().parent().remove();
                utility.DisplayMessages('Successfully Deleted', 1);
                Clinical_ProgressNote.ShowHideComponetsHeaders();
                Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
            });
        }
        else {
            Clinical_Medications.detachMedicationsFromNotes_DBCall(Clinical_MedicationIds).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    if (isUpdate) {
                        Clinical_ProgressNote.saveComponentSOAPText('Medications', true);
                    }
                    utility.DisplayMessages(response.Message, 1);
                    setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 5);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    enableDisableGridSelection: function (isDisabled) {
        if (isDisabled == true) {
            $("#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications th:first input").addClass("disableAll");
            $("#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications tr").each(function (index, item) {
                $(item).find("td:first input").addClass("disableAll");
            });
        }
        else {
            $("#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications th:first input").removeClass("disableAll");
            $("#" + Clinical_Medications.params.PanelID + " #pnlMedications_Result #dgvMedications tr").each(function (index, item) {
                $(item).find("td:first input").removeClass("disableAll");
            });
        }
    },

    /* This Function detach get Medications Soap Text and attach that to Progress note
      Author: ZeeshanAK | Date: January 14,2015 */
    getMedicationsForReconciledView: function (obj) {
        if (!(obj != null && $(obj).prop("checked") == true)) {
            Clinical_Medications.enableDisableGridSelection(false);
            //.find("td:first input").removeClass("disableAll");
            return false;
        }
        Clinical_Medications.enableDisableGridSelection(true);
        //if ($("#" + Clinical_Medications.params.PanelID + " #chkReconciledViewOnNote").is(':checked')) {
        Clinical_Medications.getMedicationsForReconciledView_DBCall().done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {
                    var ReconciledMedsIds = response.ReconcileMedsIds;
                    if (response.LastVisitReconciledMedIds == "") {
                        Clinical_Medications.medIdsForReconciledView = response.ReconcileMedsIds;
                    }
                    else {
                        Clinical_Medications.lastVisitReconciledIds = response.LastVisitReconciledMedIds;
                        var arrLastVisitReconciledId = response.LastVisitReconciledMedIds.split(",");
                        var arrAfterLastVisitMedId = response.ReconcileMedsIds.split(",");
                        var arrPatAllMedsIds = arrLastVisitReconciledId.concat(arrAfterLastVisitMedId.filter(function (item) { return arrLastVisitReconciledId.indexOf(item) < 0; }));
                        Clinical_Medications.medIdsForReconciledView = arrPatAllMedsIds.join(",");
                        //$.inArray(23412343, arr3)
                    }

                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            }
        });
    },

    /* This Function detach get Medications Soap Text and attach that to Progress note
      Author: ZeeshanAK | Date: January 14,2015 */
    getMedicationsInfo: function (medicationsID) {
        var dfd = new $.Deferred();
        var bMedReconciled = $("#" + Clinical_Medications.params.PanelID + " #chkReconciledViewOnNote").is(':checked');
        if (bMedReconciled == true) {
            medicationsID = Clinical_Medications.medIdsForReconciledView;
        }
        if (medicationsID == null || medicationsID == '') {
            return false;
        } else if (medicationsID == "0") {
            Clinical_Medications.noActiveMedicationSoapText();
            return false;
        }

        Clinical_Medications.getMedicationsForSOAP_DBCall(medicationsID).done(function (response) {
            if (response != "") {
                response = JSON.parse(response);
                if (response.status != false) {

                    if (response.MedicationSoapCount > 0) {
                        if (bMedReconciled == true) {
                            Clinical_Medications.createMedicationBodyHTML(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', medicationsID, bMedReconciled);
                            dfd.resolve('ok');
                        } else {
                            $.when(Clinical_Medications.createMedicationBodyHTMLWithOutReconcile(response, '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML', medicationsID)).then(function () {
                                dfd.resolve('ok');
                            });
                        }
                    }
                    else {
                        dfd.resolve('ok');
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve('ok');
                }
            }
            else {
                dfd.resolve('ok');
            }

        });
        return dfd.promise();
    },

    /* This Function will attach Medications to Progress Note for current Patient Selected
     Author: ZeeshanAK | Date: January 14,2015 */
    attachMedicationFromNotes: function (medicationID, fromDrFirst, bMedReconciled, MedReconciledId, hideAlertMessage) {
        var dfd = $.Deferred();
        var selectedValue = medicationID;
        if (selectedValue == "" || selectedValue == "undefined") {
            dfd.resolve();
        }
        else {
            Clinical_Medications.attachMedicationsWithNotes_DBCall(selectedValue, bMedReconciled, MedReconciledId).done(function (response) {
                response = JSON.parse(response);
                if (response.status != false) {
                    $.when(Clinical_ProgressNote.saveComponentSOAPText('Medications', hideAlertMessage, null, bMedReconciled, MedReconciledId)).then(function () {
                        // Grid row was removing which was attaching to Note
                        //   $('#' + medicationID).remove();
                        if (fromDrFirst != null && fromDrFirst) {
                            Clinical_Medications.medicationsSearch();
                        }
                        dfd.resolve();
                    });

                }
                else {
                    dfd.resolve();
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        return dfd;
    },

    /* This function will handle attachment of Medications with Notes. It represents service call to API
       Author: ZeeshanAK | Date: January 14,2015 */
    attachMedicationsWithNotes_DBCall: function (medicationId, bMedReconciled, MedReconciledId) {
        var objData = new Object();
        objData["NoteId"] = Clinical_ProgressNote.params.NotesId;
        objData["MedicationIDs"] = medicationId;
        if (bMedReconciled != null && bMedReconciled != "") {
            objData["bMedReconciled"] = bMedReconciled;
            objData["MedReconciledId"] = MedReconciledId;
        }
        else {
            objData["bMedReconciled"] = "0";
            objData["MedReconciledId"] = "";
        }

        objData["commandType"] = "attach_medications_with_notes";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Medications");
    },

    /* This function ask for Detaching Medication from Progress Note for current Patient Selected
       Author: ZeeshanAK | Date: January 14,2015 */
    detachMedicationFromNotes: function (medicationID) {
        utility.myConfirm('1', function () {
            EMRUtility.scrollToPNcomponent('clinical_medications');
            var selectedValue = medicationID.replace('Cli_Medications_Main', '');
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                Clinical_Medications.detachMedicationsFromNotes_DBCall(selectedValue).done(function (response) {
                    response = JSON.parse(response);
                    if (response.status != false) {
                        if ($('.' + medicationID).length > 0) {
                            $('.' + medicationID).each(function (index, elment) {
                                var Type = $(elment).attr('name');
                                $(elment).remove();
                                if ($('#' + Clinical_ProgressNote.params["PanelID"] + " section[name=" + Type + "]").length == 0) {
                                    $('#' + Clinical_ProgressNote.params["PanelID"] + "  div#" + Type).remove();
                                }
                            });
                        } else {
                            $('#' + medicationID).remove();
                        }


                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllCurrentMedications').find('section[id*="Cli_Medications_Main"]').length == 0) {
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_CurrentMedications').remove();
                        }


                        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML #AllPastMedications').find('section[id*="Cli_Medications_Main"]').length == 0) {
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML').find('#Section_PastMedications').remove();
                        }


                        Clinical_ProgressNote.saveComponentSOAPText('Medications');
                        setTimeout(Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val()), 15);
                        //   utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
        }, function () { },
            '1'
        );
    },

    checkUncheckAllMeds: function (obj) {
        if ($(obj).is(':checked')) {
            $("#" + Clinical_Medications.params.PanelID + " input[name='SelectCheckBoxMedication']:checkbox").prop('checked', true);
        } else {
            $("#" + Clinical_Medications.params.PanelID + " input[name='SelectCheckBoxMedication']:checkbox").prop('checked', false);
        }
        $("#" + Clinical_Medications.params.PanelID + " #dgvMedications tbody").find('input[type="checkbox"]').each(function () {
            Clinical_Medications.enableAddMedication(this);
        });

    },

    getRcopiaInformaionForMedicationSoap: function () {
        if (Clinical_Medications.params.length > 0 && Clinical_Medications.params.ParentCtrl == 'clinicalTabProgressNote') {
            Clinical_Medications.getLatestMedicationsByPatientId(true);
        }
    },

    /* This function will insert Reviewed by info on top
       Author: ZeeshanAK | Date: January 26,2015 */
    insertReviewedInfoOnTop: function (MedicationSoap_JSON) {
        var MedicationsSOAPJSON = JSON.parse(MedicationSoap_JSON);
        if (MedicationsSOAPJSON.length > 0) {
            var dateFormat = MedicationsSOAPJSON[0].ReviewedOn;
            var Day = new Date(dateFormat);
            var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);

            $("#" + Clinical_Medications.params.PanelID + " #reviewedByTop").html((MedicationsSOAPJSON[0].ReviewedBy == '' ? "" : "Medications reviewed by " + MedicationsSOAPJSON[0].ReviewedBy) +
                " on " + utility.GetDayNameFromDayNumber(Day.getDay()) + " " + ReviewedOndateFormat);
        }

    },





    GetReviewedMedicationInfo: function () {
        var dfd = $.Deferred();
        Clinical_Medications.GetReviewedMedicationInfo_DB_CALL().done(function (response) {
            response = JSON.parse(response);
            if (response.status != false) {
                if (response.ReviewedTotalCount > 0) {


                    var MedicationReviewSoap_JSON = JSON.parse(response.MedicationReviewSoap_JSON)[0];
                    if (MedicationReviewSoap_JSON.ReviewedOn != null && MedicationReviewSoap_JSON.ReviewedOn != '') {
                        var dateFormat = MedicationReviewSoap_JSON.ReviewedOn;
                        var ReviewedOndateFormat = $.datepicker.formatDate('MM dd, yy ', new Date(dateFormat)) + utility.RemoveDateFromDateTime(null, dateFormat);
                        dfd.response = (MedicationReviewSoap_JSON.ReviewedBy == '' ? "" : "Medications reviewed by " + MedicationReviewSoap_JSON.ReviewedBy) + " on:  " + ReviewedOndateFormat;
                    }
                    else {
                        dfd.response = "";
                    }

                    dfd.resolve();
                }
                else {
                    dfd.response = "";
                    dfd.resolve();
                }
            }
            else {
                dfd.response = "";
                utility.DisplayMessages(strMessage, 3);
                dfd.resolve();
            }
        });
        return dfd;
    },
    GetReviewedMedicationInfo_DB_CALL: function () {
        var objData = new Object();
        if (Clinical_Medications.params.patientID != null && typeof Clinical_Medications.params.patientID != typeof undefined) {
            objData["PatientId"] = Clinical_Medications.params.patientID;
        }
        else {
            objData["PatientId"] = Clinical_Medications.params.ParentCtrl == "Clinical_FaceSheet" ? Clinical_FaceSheet.params.patientID : $('#PatientProfile #hfPatientId').val();
        }
        objData["commandType"] = "loadMedicationsReviewedBy";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "MEDICAL", "Medications");
    },
}