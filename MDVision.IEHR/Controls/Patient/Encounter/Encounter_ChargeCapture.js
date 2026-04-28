EncounterChargeCapture = {
    bIsFirstLoad: true,
    params: [],
    EditableGrid: null,
    IsPatientInsuranceChange: true,
    IsSetSubmissionMode: false,
    IsSaved: null,
    IsZeroFee: false,
    IsCallChange: true,
    IsSelfPay: "False",
    InsurancePlan: null,
    PrevisosProvider: null,
    PreviouseInsPlan: null,
    SelectedInsurancePlan: null,
    IsSplittedClaim: false,
    IsToHideSplittedClaimBtn: false,
    ddlCurrentSubmitStatus: "",
    ddlSubmitStatusonUnload: "",
    IsFeeGreaterThanHPCSCost: false,
    HPCS: "",
    HPCSCost: "",
    FocusedICD: null,
    ICDArrays: null,
    IsVNC: null,
    isloadFrom: 0,
    isloadTo: 0,
    objInsDeffered: "",
    objsyncInsurance: "",
    isFromloadFirst: false,
    cptArray: [],
    IsERAAttached:false,
    CPTCodeSortedArr: [],    //PRD-636  TahreeMalik
    Load: function (params) {
        EncounterChargeCapture.params = params;
        EncounterChargeCapture.IsPatientInsuranceChange = true;
        EncounterChargeCapture.IsSetSubmissionMode = false;
        EncounterChargeCapture.IsCallChange = true;
        EncounterChargeCapture.IsSelfPay = "False";
        EncounterChargeCapture.PrevisosProvider = null;
        EncounterChargeCapture.PreviouseInsPlan = null;
        EncounterChargeCapture.SelectedInsurancePlan = null;
        EncounterChargeCapture.IsSplittedClaim = false;
        EncounterChargeCapture.IsToHideSplittedClaimBtn = false;
        EncounterChargeCapture.ICDArrays = [];
        EncounterChargeCapture.IsVNC = null;
        EncounterChargeCapture.objInsDeffered = $.Deferred();
        EncounterChargeCapture.objsyncInsurance = $.Deferred();
        EncounterChargeCapture.CPTCodeSortedArr = [];       //PRD-636   TahreeMalik

        EncounterChargeCapture.isFromloadFirst = false;
        EncounterChargeCapture.InsurancePlan = null;

        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find(
                        "#txtDOB,#txtAge,#txtSex"
                        ).prop('disabled', true);
      //  $('#dgvVisitCharge_wrapper input[id*="txtdrugCodeCost"]').prop("disabled", true)
        if (!EncounterChargeCapture.params["VisitId"] && $('#' + EncounterChargeCapture.params.PanelID).attr("VisitId")) {
            EncounterChargeCapture.params["VisitId"] = $('#' + EncounterChargeCapture.params.PanelID).attr("VisitId");//-1;
        }
        EncounterChargeCapture.InitilizeToggle();
        if (EncounterChargeCapture.params["PatientId"]) {
            $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val(EncounterChargeCapture.params["PatientId"]);
        }
        if (EncounterChargeCapture.params.ParentCtrl == "Bill_ChargeSearch" || EncounterChargeCapture.params.ParentCtrl == "clinicalTabProgressNote" || EncounterChargeCapture.params.ParentCtrl == "Bill_PaymentPosting" || EncounterChargeCapture.params.ParentCtrl == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.ParentCtrl == "Patient_Case_Detail" || EncounterChargeCapture.params.TabID == "chargeBatchDetail" || EncounterChargeCapture.params.TabID == "schTabCalendar" || EncounterChargeCapture.params.TabID == "schTabMultipleView") {
            $("#" + EncounterChargeCapture.params.PanelID + " #titleChargeCapture").html("Claim Detail");
            $("#" + EncounterChargeCapture.params.PanelID + " #modalbody").removeClass('panel-body tabs-custome-panel-body');
        }
        var ObjDeferred = new $.Deferred();
        IsEncounterAlreadyLoaded = EncounterChargeCapture.IsEncounterTabExist(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl);
        if (IsEncounterAlreadyLoaded) {
            EncounterChargeCapture.bIsFirstLoad = false;
        }
        else {
            EncounterChargeCapture.bIsFirstLoad = true;
            EncounterChargeCapture.AddEncounterTabArray(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl);
        }
        if (EncounterChargeCapture.bIsFirstLoad) {
            //ICD Editable Grid
            //EncounterChargeCapture.LoadAllAutocomplete();
            //Call on input instead of load.optimize the call
            // RefProviders.length = 0;
            // Facilities.length = 0
            // Providers.length = 0;
            // PatientCase.length = 0;
            //////////////////////////////////////////
            EncounterChargeCapture.cptArray = [];
            var PanelICDGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlICD_Result";
            var ICDGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvChargeCaptureICD";
            //utility.MakeEditableGrid(PanelICDGrid, ICDGridId, EncounterChargeCapture);

            //CPT Editable Grid
            var PanelCPTGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlCPT_Result";
            var CPTGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvChargeCaptureCPT";
            //utility.MakeEditableGrid(PanelCPTGrid, CPTGridId, EncounterChargeCapture);

            //Modifier Editable Grid
            var PanelModifierGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlModifier_Result";
            var ModifierGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvChargeCaptureModifier";

            var self = null;
            if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
            else
                self = $('#' + EncounterChargeCapture.params.PanelID);
            CacheManager.BindDropDownsByID(self.find('#ddlClaimType'), 'GetClaimType', false, null, null, true);
            CacheManager.BindDropDownsByID(self.find('#ddlSubmitStatus'), 'GetSubmitStatus', false, null, null, null).done(function () {
                self.loadDropDowns(true).done(function () {
                    EncounterChargeCapture.ddlCurrentSubmitStatus = self.find('#ddlSubmitStatus option:selected').text().toLowerCase();
                    EncounterChargeCapture.ValidateVisit();
                    // Start Anesthesia Checks @@Irfan@@
                    if (EncounterChargeCapture.params.ClaimType != null && EncounterChargeCapture.params.ClaimType != undefined && EncounterChargeCapture.params.ClaimType != "undefined") {
                        self.find('#ddlClaimType option').filter(function () {
                            return $.trim($(this).text().toLowerCase()) == EncounterChargeCapture.params.ClaimType;
                        }).attr('selected', true).trigger('change');



                        if (EncounterChargeCapture.params.ClaimType == 'professional medical') {

                        } else if (EncounterChargeCapture.params.ClaimType == 'professional anesthesia') {
                            EncounterChargeCapture.setDefaultDataForAnesthesia();

                            if (EncounterChargeCapture.params.SpecialityName.toLowerCase() == 'anesthesiology' || EncounterChargeCapture.params.SpecialityName.toLowerCase() == '') {
                                self.find("#radAnesthesiologist").attr('checked', 'checked');
                                self.find("#radAnesthesiologist").trigger('click');
                            } else if (EncounterChargeCapture.params.SpecialityName.toLowerCase() == 'crna') {
                                self.find("#radCRNA").attr('checked', 'checked');
                                self.find("#radCRNA").trigger('click');
                            } else {
                                self.find("#radAnesthesiologist").prop('checked', false);
                                self.find("#radAnesthesiologist").prop('checked', false);
                            }

                        }
                    } else {
                        self.find('#ddlClaimType option').filter(function () {
                            return $.trim($(this).text().toLowerCase()) == "professional medical";
                        }).attr('selected', true);
                    }
                    // End Anesthesia Checks @@Irfan@@
                    if (globalAppdata['DefaultBillingProviderId'] != "- Select -" && globalAppdata['DefaultBillingProviderId'] != "") {
                        self.find('#ddlBillingProvider').val(globalAppdata.DefaultBillingProviderId).attr("selected", "selected");
                    }
                    // set Case Id and number if it have in params
                    if (EncounterChargeCapture.params["CaseId"] && EncounterChargeCapture.params["CaseNo"]) {
                        self.find('#txtCaseNumber').val(EncounterChargeCapture.params["CaseNo"]);
                        self.find('#hfCaseId').val(EncounterChargeCapture.params["CaseId"]);
                        //Beging Edited by Azeem Raza Tayyab on 24-Feb-2016 to Fix Bug #: PMS-4103
                        if (self.find("#lnkCaseNumberEdit").css("display") == "none") {
                            self.find("#lnkCaseNumberEdit").css("display", "inline");
                            self.find("#lblCaseNumber").css("display", "none");
                        }
                        //End Edited by Azeem Raza Tayyab on 24-Feb-2016 to Fix Bug #: PMS-4103
                    }
                    $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
                    //  self.find('#claimAdditionalInfoDiv section').removeClass('active');
                    //  self.find('#claimAdditionalInfoDiv div:eq(0)').css({ 'display': "none" });

                    if (EncounterChargeCapture.params.ParentCtrl == "Bill_ChargeSearch") {

                        $('body').addClass('modal-open');
                    }

                    if (EncounterChargeCapture.params.BillingProvider) {
                        $('#ddlBillingProvider').val(EncounterChargeCapture.params.BillingProvider);
                    }
                    ObjDeferred.resolve();
                });
            });


            utility.CreateDatePicker(EncounterChargeCapture.params.PanelID + ' #dtpDocumentSentDate,#dtpInjuryDate,#dtpAdmissionDate,#dtpCurrentIllnessDate,#dtpDischargeDate,#dtpLMPDate,#dtpLastSeenDate,#dtpHoldTill', function () { });
            var myDate = new Date();
            myDate.setDate(myDate.getDate() + 1);
            self.find('#dtpHoldTill').datepicker('setStartDate', myDate);
            EncounterChargeCapture.BindProvider();
            EncounterChargeCapture.BindFacility();
            EncounterChargeCapture.BindRefProvider();
            EncounterChargeCapture.BindPatientAccount();
        }
        else {
            ObjDeferred.resolve();
        }
        EncounterChargeCapture.ChangeSubTabStyle(EncounterChargeCapture.params["TabID"]);
        if (EncounterChargeCapture.params["VisitId"] && EncounterChargeCapture.params["VisitId"] != "-1") {
            var self = null;
            if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail" || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.ParentCtrl == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.ParentCtrl == "billTabUnClaimedAppointment" || EncounterChargeCapture.params.ParentCtrl == "Bill_ChargeSearch" || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.ParentCtrl == "schTabCalendar" || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "billTabPaymentPosting" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
                self = $("#" + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
            else
                self = $("#" + EncounterChargeCapture.params.PanelID);
            self.find('#aHistory').removeClass('disableAll');
            self.find('#aPrintClaim').removeClass('disableAll');
            self.find('#btnNotesLog').removeClass('disableAll');
            //start syed zia 03-02-2015, bug #PMS-3788
            if (typeof EncounterChargeCapture.params.AppointmentId != "undefined") {
                self.find('#divLoadCopayment').removeClass('disableAll');
            }
            else {
                $('#' + EncounterChargeCapture.params.PanelID + ' #divLoadCopayment').addClass('disableAll');
            }
            //end syed zia 03-02-2015, bug #PMS-3788
            self.find('#aEOB').removeClass('disableAll');
            EncounterChargeCapture.params["mode"] = "Edit";
            self.find('#frmEncounterChargeCapture #txtPatientName').prop('disabled', true);
            self.find('#frmEncounterChargeCapture #txtDOB').prop('disabled', true);
            self.find('#frmEncounterChargeCapture #txtAge').prop('disabled', true);
            self.find('#frmEncounterChargeCapture #txtSex').prop('disabled', true);
            // $("input[name^='news']";
           // $('#dgvVisitCharge_wrapper input[id*="txtdrugCodeCost"]').prop("disabled", true)

            self.find('#divSearchPatient').addClass('disableAll');
            $.when(ObjDeferred).then(function () {
                // EncounterChargeCapture.LoadAllAutocomplete();
                EncounterChargeCapture.BindPatientPAN();
                EncounterChargeCapture.isFromloadFirst = true;
                EncounterChargeCapture.FillVisitData(EncounterChargeCapture.params["VisitId"], $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val());
                EncounterChargeCapture.enablePatientControls();
            });
        }
        else {

            if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail") {
                var txtBatchNumber = self.find("#txtBatchNumber");
                self.find("#hfBatchId").val(EncounterChargeCapture.params.BatchId);
                txtBatchNumber.val(EncounterChargeCapture.params.BatchNumber);
                txtBatchNumber.attr("disabled", "disabled");
            }

            $('#' + EncounterChargeCapture.params.PanelID + ' #aHistory').addClass('disableAll');
            $('#' + EncounterChargeCapture.params.PanelID + ' #aPrintClaim').addClass('disableAll');
            $('#' + EncounterChargeCapture.params.PanelID + ' #divLoadCopayment').addClass('disableAll');
            $('#' + EncounterChargeCapture.params.PanelID + ' #aEOB').addClass('disableAll');
            $('#' + EncounterChargeCapture.params.PanelID + ' #aClinicalNotes').addClass('disableAll');
            $('#' + EncounterChargeCapture.params.PanelID + ' #btnNotesLog').addClass('disableAll');
            EncounterChargeCapture.params["mode"] = "Add";
            EncounterChargeCapture.enablePatientControls();
            //   EncounterChargeCapture.LoadAllAutocomplete();

            EncounterChargeCapture.LoadDefaultData();
            EncounterChargeCapture.FillReferralNumber();

            //EncounterChargeCapture.ValidateVisit();
            EncounterChargeCapture.ShowChargeDetails(true);
            //Charge Editable Grid
            var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
            var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
            //$(ChargeGridId).dataTable().fnDestroy();
            $(ChargeGridId + " tbody tr").remove();
            EncounterChargeCapture.EditableGrid = utility.MakeEditableGrid(PanelChargeGrid, ChargeGridId, EncounterChargeCapture, "0", false, false, false, false);


            $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());

        }
        if (EncounterChargeCapture.bIsFirstLoad) {
            EncounterChargeCapture.bIsFirstLoad = false;
            utility.ValidateFromToDate('frmEncounterChargeCapture', 'dtpAdmissionDate', 'dtpDischargeDate', true);

            utility.ValidateFromToDate('frmEncounterChargeCapture', 'dtpUnableToWorkFrom', 'dtpUnableToWorkTo', true);
            $('#' + EncounterChargeCapture.params.PanelID + ' #dtpInjuryDate,#dtpSubmittedDate,#dtpLMPDate,#dtpCurrentIllnessDate,#dtpChargeDOSFrom,#dtpChargeDOSTo,#dtpLastSeenDate,#dtpDocumentSentDate').datepicker({
                todayHighlight: true,
            }).on('changeDate', function (e) {
                $(this).datepicker('hide');
                //$('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'DOSFrom');//DOSFromDOSTo
                //$('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'DOSTo');
                if ($(e.currentTarget).attr('id') == 'dtpDOSTo') {
                    //EncounterChargeCapture.setInjuryDateRange();
                }

                if ($(e.currentTarget).attr('id') == 'dtpInjuryDate') {
                    if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                        $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'InjuryDate');
                    }
                }

            });


            //if (EncounterChargeCapture.params.AppointmentDate != null || EncounterChargeCapture.params.AppointmentDate != "")
            //    $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #dtpAppointmentDate').val(EncounterChargeCapture.params.AppointmentDate);


            $('#' + EncounterChargeCapture.params.PanelID + ' #dtpLMPDate,#dtpCurrentIllnessDate,#dtpLastSeenDate,#dtpDocumentSentDate').datepicker('setEndDate', new Date());
            //$('#' + EncounterChargeCapture.params.PanelID + ' #dtpInjuryDate').datepicker('setEndDate', $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSTo').val());
            //utility.ValidateFromToDate(EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture', 'dtpFromDOS', 'dtpToDOS', false);

            /******************/
            EncounterChargeCapture.isloadFrom = 0;
            EncounterChargeCapture.isloadTo = 0;
            utility.ValidateFromToDate(EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture', 'dtpDOSFrom', 'dtpDOSTo', true, function () {

                //from date callback
                EncounterChargeCapture.setInjuryDateRange();

                if (EncounterChargeCapture.isloadFrom > 1)
                    EncounterChargeCapture.setDosFromInCharges();

                EncounterChargeCapture.isloadFrom++;

                if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                    $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'DOSFrom');
                }
                EncounterChargeCapture.validateDosFromTo($('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSFrom'), $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSTo'));
            }, function () {

                //to date callback
                if (EncounterChargeCapture.isloadTo > 1)
                    EncounterChargeCapture.setDosToInCharges();

                EncounterChargeCapture.isloadTo++;

                if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                    $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'DOSTo');
                }
                //EncounterChargeCapture.setInjuryDateRange();
                EncounterChargeCapture.validateAdmissionDate();

            });
            EncounterChargeCapture.FillDOS();
            /***********************/
            if (EncounterChargeCapture.params["DOSFrom"]) {
                $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSFrom').val(EncounterChargeCapture.params["DOSFrom"]);
            }
            if (EncounterChargeCapture.params["DOSTo"]) {
                $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSTo').val(EncounterChargeCapture.params["DOSTo"]);
            }
            EncounterChargeCapture.validateAdmissionDate();
            EncounterChargeCapture.validateDosFromTo($('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSFrom'), $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSTo'));

        }
        var self = null;
        if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail" || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.ParentCtrl == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.ParentCtrl == "billTabUnClaimedAppointment" || EncounterChargeCapture.params.ParentCtrl == "Bill_ChargeSearch" || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.ParentCtrl == "schTabCalendar" || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "billTabPaymentPosting" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            self = $("#" + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $("#" + EncounterChargeCapture.params.PanelID);

        if (EncounterChargeCapture.params.patientID && EncounterChargeCapture.params.patientID != "-1") {
            if (EncounterChargeCapture.params.mode.toLowerCase() == "edit" && EncounterChargeCapture.params.ParentCtrl != null) // For Encounter Visits
                self.find('#divAccount,#divPatientName').removeClass("hidden");

            else
                self.find('#divAccount,#divPatientName').addClass("hidden");

            self.find("#frmEncounterChargeCapture #hfPatientId").val(EncounterChargeCapture.params.patientID);
            EncounterChargeCapture.LoadPatientInsurances(self.find("#frmEncounterChargeCapture #hfPatientId").val());
            //EncounterChargeCapture.SelectPrimaryInsurance();
            //EncounterChargeCapture.FillReferralNumber();
        }
        else {
            self.find('#divAccount,#divPatientName').css("display", "");
            var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
            //formValidation.enableFieldValidators('PatientName', true);
        }
        if (EncounterChargeCapture.params.ParentCtrl != "chargeBatchDetail" && EncounterChargeCapture.params.ParentCtrl != "billTabUnClaimedAppointment" && EncounterChargeCapture.params.ParentCtrl != "Bill_ChargeSearch" && EncounterChargeCapture.params.ParentCtrl != "BillingInformation" && EncounterChargeCapture.params.TabID != 'Patient_Case_Detail' && EncounterChargeCapture.params.ParentCtrl != "schTabCalendar" && EncounterChargeCapture.params.TabID != 'batchTabEncounter' && EncounterChargeCapture.params.ParentCtrl != 'Bill_FollowUpPatientAR_Detail' && EncounterChargeCapture.params.ParentCtrl != 'Bill_FollowUpInsuranceAR_Detail' && EncounterChargeCapture.params.ParentCtrl != 'Patient_Ledger' && EncounterChargeCapture.params.ParentCtrl != 'clinicalTabProgressNote' && EncounterChargeCapture.params.TabID != "schTabMultipleView") {
            EncounterChargeCapture.removeDialogClasses();
            self.find('#aEncounter').hide()
        }    

        if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail") {
            //var txtBatchNumber = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture #txtBatchNumber");
            //$('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture #hfBatchId").val(EncounterChargeCapture.params.BatchId);
            //txtBatchNumber.val(EncounterChargeCapture.params.BatchNumber);
            //txtBatchNumber.attr("disabled", "disabled");
            if (EncounterChargeCapture.params.mode == "Add") {
                //$('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture #dtpAppointmentDate").val($.datepicker.formatDate('mm/dd/yy', new Date()));
                //$('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture #dtpDOSFrom").val($.datepicker.formatDate('mm/dd/yy', new Date()));
                //$('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture #dtpDOSTo").val($.datepicker.formatDate('mm/dd/yy', new Date()));
                //utility.CreateDatePicker(EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture,#dtpDOSFrom,#dtpDOSTo', function () {
                //on-change callback method



                //}, true);

                //if (EncounterChargeCapture.params.AppointmentDate != null || EncounterChargeCapture.params.AppointmentDate != "")
                //    $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #dtpAppointmentDate').val(EncounterChargeCapture.params.AppointmentDate);
                //Begin Edited by Azeem Raza Tayyab on 19-Feb-2016 to Fix Bug#PMS-3999
                if (EncounterChargeCapture.params.pAccountNumber != undefined) {
                    $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #txtPatientName').val(EncounterChargeCapture.params.pAccountNumber.split(" ")[0]);
                    $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #txtPatientFullName').val(EncounterChargeCapture.params.pAccountNumber.split(" - ")[1]);
                }
                if (EncounterChargeCapture.params.pID != undefined) {
                    $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #hfPatientId').val(EncounterChargeCapture.params.pID);
                    EncounterChargeCapture.LoadPatientInsurances(EncounterChargeCapture.params.pID);
                    //EncounterChargeCapture.SelectPrimaryInsurance();
                    //EncounterChargeCapture.FillReferralNumber();
                }
                
                //End Edited by Azeem Raza Tayyab on 19-Feb-2016 to Fix Bug#PMS-3999


            }
            //   $($('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture #claimAdditionalInfoDiv section")[0]).addClass('active');
            //   $($('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture #claimAdditionalInfoDiv div:eq(0)")[0]).css({ 'display': "block" });
        }
        if (EncounterChargeCapture.params.ParentCtrl != null) {
            if ($('body #' + EncounterChargeCapture.params.PanelID).length > 1)
                $('#pnlEncounterChargeCapture div.modal-content div').next().get(0).setAttribute('style', 'margin-top:0px !important');
            else {
                //if      ($(document.getElementById(EncounterChargeCapture.params.PanelID).querySelector('#pnlEncounterChargeCapture div.modal-content div')).length > 1)
                if (EncounterChargeCapture.params.ParentCtrl != "clinicalTabProgressNote") {
                    $(document.getElementById(EncounterChargeCapture.params.PanelID).querySelector('#pnlEncounterChargeCapture div.modal-content div')).next().get(0).setAttribute('style', 'margin-top:0px !important');
                }
            }

        }
        self.find("#chkAllCharges").change(function () {

            var IsChecked = $(this).prop('checked');
            if (IsChecked)
                $(this).attr('title', 'Unselect all');
            else
                $(this).attr('title', 'Select all');

            self.find("#dgvVisitCharge tr").each(function (i, row) {
                if ($(this).attr("id") && $(this).attr("id") != null) {
                    var chkActive = $(this).find("input[id^=chkActive]");
                    $(chkActive).prop('checked', IsChecked);
                }
            });

        });
        EncounterChargeCapture.InitializeAnesthesiaTimePicker();
        $.when(ObjDeferred).then(function (event) {
            EncounterChargeCapture.IsDemographicUpdated(EncounterChargeCapture.params.patientID, EncounterChargeCapture.params.VisitId);
            if (EncounterChargeCapture.params["FromCreateClaim"] && EncounterChargeCapture.params["parmsFromCreateClaims"]) {
                var parmsFromCreateClaims = EncounterChargeCapture.params["parmsFromCreateClaims"];
                EncounterChargeCapture.FillPatientInfoFromSearch(parmsFromCreateClaims["patientId"], parmsFromCreateClaims["patFullName"], parmsFromCreateClaims["RefProviderId"], parmsFromCreateClaims["ReProviderName"], parmsFromCreateClaims["providerId"], parmsFromCreateClaims["providerName"], parmsFromCreateClaims["facilityId"], parmsFromCreateClaims["facilityName"], parmsFromCreateClaims["selfPay"], event, parmsFromCreateClaims["billingProviderId"], parmsFromCreateClaims["resourceProviderId"], parmsFromCreateClaims["resourceProviderName"], parmsFromCreateClaims["DOB"], parmsFromCreateClaims["Gender"]);
                EncounterChargeCapture.LoadPatientCase(parmsFromCreateClaims["patientId"]);
            }
            if (EncounterChargeCapture.params.ParentCtrl == "Bill_ChargeSearch" || EncounterChargeCapture.params.ParentCtrl == "billTabUnClaimedAppointment" || EncounterChargeCapture.params.ParentCtrl == 'Patient_Case_Detail' || EncounterChargeCapture.params.ParentCtrl == 'chargeBatchDetail') {
                if ((EncounterChargeCapture.params.mode == "Add" && EncounterChargeCapture.params.ClaimType != 'professional anesthesia') || EncounterChargeCapture.params.ParentCtrl == 'Patient_Case_Detail') {
                    EncounterChargeCapture.AddNewChargeRow(null, 'Add');
                } else {
                    //self.find('#dgvVisitCharge').hide();
                }
            }
        });

        if (EncounterChargeCapture.params["CaseNo"]) {
            EncounterChargeCapture.FillCaseDetail(EncounterChargeCapture.params["CaseNo"]);
        }
        //if(EncounterChargeCapture.params["AppointmentId"] && EncounterChargeCapture.params["AppointmentId"]>0  
        //    && EncounterChargeCapture.params["IsCopayAlert"]  &&  EncounterChargeCapture.params["IsCopayAlert"].toString().toLowerCase()=="true")
        //{

        //}
        
    },
  
    IsDemographicUpdated: function (PatientId, VisitId)
    {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        EncounterChargeCapture.CheckConflict(PatientId, VisitId).done(function (response) {
            ///
            if (response.result == true) {
                EncounterChargeCapture.ddlCurrentSubmitStatus = self.find('#ddlSubmitStatus option:selected').text().toLowerCase();

                if (response.check == 1 && EncounterChargeCapture.ddlCurrentSubmitStatus != "settled" && $('#txtTotalBalances').val() > 0) {
                    utility.myConfirm('Insurance information is updated for this patient. Do you want to sync this claim with insurance of Patient Demographics?', function () {
                        EncounterChargeCapture.SyncClaimDetailWithDemographic();
                        ///
                    }, function () {
                        return false;
                    }, 'Update Insurance');
                }
            }
        });
    },   

    CheckPervileges: function () {
        var Permissions = [];
        Permissions.push('VIEW');
        var obj_eBill = {
            FormName: "Miscellaneous_eSuperbill",
            Permissions: Permissions
        };

        var data_ = [];
        data_.push(obj_eBill, obj_Sign, obj_coSign);

        var data_s = "PrivilegeDate=" + JSON.stringify(data_);
        AppPrivileges.GetMultipleFormPrivileges(data_s, "FORM_PRIVILEGE", "GET_MULTIPLE_FORM_PRIVILEGE", function (response) {
            if (response.status != false) {
                var Privilege_Data = response.Privilege_JSON;
                $.each(Privilege_Data, function (i, item) {
                    if (item.FormName == "Miscellaneous_eSuperbill") {
                        Clinical_ProgressNote.eSuperbillPermissionsFromNote(item.PermissionsResponseModel[0].IsAccessible);
                    }
                    else if (item.FormName == "Notes_Sign") {
                        if (item.PermissionsResponseModel[0].IsAccessible == true) {
                            $('#' + Clinical_ProgressNote.params.PanelID + ' #btnSign').removeClass('hidden');
                        }
                        else {
                            $('#' + Clinical_ProgressNote.params.PanelID + ' #btnSign').addClass('hidden');
                        }
                    }
                    else if (item.FormName == "Notes_Co-Sign") {
                        if (item.PermissionsResponseModel[0].IsAccessible == true) {
                            $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteCoSign').removeClass('hidden');
                        }
                        else {
                            $('#' + Clinical_ProgressNote.params.PanelID + ' #btnNoteCoSign').addClass('hidden');
                        }
                    }
                });
            }
            else {
                utility.DisplayMessages(response.Message, 2);
            }
        });
    },

    PostUnallocatedCopay: function (response) {

        var objDeffered = $.Deferred();

        if (EncounterChargeCapture.isFromloadFirst) {
            EncounterChargeCapture.isFromloadFirst = false;

            if (EncounterChargeCapture.params["VisitId"] && EncounterChargeCapture.params.mode.toLowerCase() == "edit") {

                if (response.status != false) {
                    var visit_detail = JSON.parse(response.VisitFill_JSON);

                    var SearchedfieldsJSON = new Object();
                    SearchedfieldsJSON["txtFacility"] = visit_detail.txtFacility;
                    SearchedfieldsJSON["hfFacility"] = visit_detail.hfFacility;
                    SearchedfieldsJSON["txtProvider"] = visit_detail.txtProvider;
                    SearchedfieldsJSON["hfProvider"] = visit_detail.hfProvider;
                    SearchedfieldsJSON["txtPatientName"] = visit_detail.txtPatientName;
                    SearchedfieldsJSON["hfPatientId"] = visit_detail.hfPatientId;
                    SearchedfieldsJSON["dpStartDate"] = null;
                    SearchedfieldsJSON["dpToDate"] = null;
                    SearchedfieldsJSON["AppointmentId"] = visit_detail.AppointmentId;

                    var myJSON = JSON.stringify(SearchedfieldsJSON);
                    Scheduling_UnallocatedCopayment_Search.SearchCopayReceipt(myJSON).done(function (response) {
                        if (response.status != false) {
                            if (response.UnAllocatedCopayListRecordCount > 0) {

                                var Amount = 0;
                                var UnAllocatedCopayId = 0;
                                var Date_ = "";
                                var IsCount = false;
                                var UnAllocatedCopayJSONData = response.UnAllocatedCopayInfo_JSON;
                                $.each(UnAllocatedCopayJSONData, function (i, item) {

                                    if (item.Status.toLowerCase() == "unallocated" && item.IsDeleted.toLowerCase() == "false" && IsCount == false) {
                                        UnAllocatedCopayId = item.UnAllocatedCopayId;
                                        Amount = item.CopayAmount ? parseFloat(item.CopayAmount).toFixed(Number(globalAppdata.DecimalPlaces)) : 0.00;
                                        Date_ = utility.RemoveTimeFromDate(null, item.ReceiptDate);
                                        IsCount = true;
                                    }
                                });

                                if (IsCount) {

                                    utility.myConfirm("$" + Amount + " Copay paid for visit " + Date_ + ".</br> Do you want to allocate this amount?",
                                    function () {
                                        EncounterChargeCapture.PostUnallocatedCopay_DBCall(EncounterChargeCapture.params["VisitId"], UnAllocatedCopayId).done(function (result) {

                                            if (result.status != false) {

                                                utility.DisplayMessages("Successfully Posted.", 1);

                                                Unallocated_CopaymentView.UnallocatedCopayPreview(UnAllocatedCopayId, false).done(function () {

                                                    var pdf = Unallocated_CopaymentView.pdf;
                                                    Scheduling_UnallocatedCopayment.SaveCopayReceiptInFolder(
                                                        EncounterChargeCapture.params["VisitId"],
                                                        Date_,
                                                        result.ReceiptNumber,
                                                        SearchedfieldsJSON["hfPatientId"],
                                                        pdf).done(function (res) {
                                                            if (res.status != false) {
                                                                utility.DisplayMessages("Receipt saved successfully.", 1);
                                                                objDeffered.resolve(true);
                                                            }
                                                            else {
                                                                utility.DisplayMessages(res.Message, 3);
                                                                objDeffered.resolve(true);
                                                            }
                                                        });
                                                });
                                            }
                                            else {
                                                utility.DisplayMessages(result.Message, 3);
                                                objDeffered.resolve(true);
                                            }
                                        });
                                    },
                                    function () {
                                        objDeffered.resolve(true);
                                    }, "ALERT");

                                }
                                else {
                                    objDeffered.resolve(true);
                                }

                            }
                        }
                        else {
                            objDeffered.resolve(true);
                        }
                    });

                }
                else {
                    objDeffered.resolve(true);
                }
            }
        }
        else {
            objDeffered.resolve(true);
        }



        return objDeffered;

    },

    PostUnallocatedCopay_DBCall: function (VisitId, UnAllocatedCopayId) {

        var data = "VisitId=" + VisitId + "&UnAllocatedCopayId=" + UnAllocatedCopayId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_UNALLOCATEDCOPAYMENT", "POST_UNALLOCATED_COPAYMENT");

    },

    //ValidateTaxonomyCode24IJ:function()
    //{
    //    if ($('#ddlBox24IJShaded').val() == '2')
    //    {
    //        var ProviderId = self.find('#hfProvider').val();
    //        var ProviderId = ProviderId != "" ? parseInt(ProviderId) : 0;
    //        if (ProviderId > 0) {
    //            Admin_Provider.SearchProvider('', ProviderId).done(function (response) {
    //                if (response.status != false) {
    //                    var result = JSON.parse(response.ProviderLoad_JSON)
    //                    if (!result.TaxonomyCode) {
    //                        utility.myConfirm('Taxonomy code is missing in the provider detail screen are you sure you want to select "Print Taxonomy Code" ?', function () {

    //                        }
    //                            , function () {
    //                                     //NO CALLBACK
    //                                 },
    //                          'Confirm Taxonomy Code'

    //                          //
    //                        );

    //                    }
    //                }

    //            });
    //        }
    //    }

    //},
    //ValidateTaxonomyCode24B: function () {
    //    if ($('#ddlBox24BShaded').val() == '2') {
    //        var ProviderId = self.find('#hfProvider').val();
    //        var ProviderId = ProviderId != "" ? parseInt(ProviderId) : 0;
    //        if (ProviderId > 0) {
    //            Admin_Provider.SearchProvider('', ProviderId).done(function (response) {
    //                if (response.status != false) {

    //                }
    //                else {
    //                    utility.DisplayMessages(response.Message, 3);
    //                }
    //            });
    //        }
    //    }

    //},


    setInjuryDateRange: function () {
        //$('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #dtpInjuryDate').datepicker('setStartDate', $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSFrom').val());
        //$('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #dtpInjuryDate').datepicker('setEndDte', $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSTo').val());

        //$('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #dtpInjuryDate').datepicker('setEndDate', $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSFrom').val());

        var self = null;
        //if (EncounterChargeCapture.params.PanelID == 'pnlEncounterChargeCapture')
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var dosFromDate = self.find('#dtpDOSFrom').datepicker('getDate');
        var injuryDate = self.find('#dtpInjuryDate').datepicker('getDate');

        //injury date must be less than or equal to date of service from otherwise this field will get cleared
        if ((dosFromDate - injuryDate) < 0)
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #dtpInjuryDate').val('');

        self.find('#dtpInjuryDate').datepicker('setEndDate', $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSFrom').val());


    },
    // To make sure of Sub tabs styles
    ChangeSubTabStyle: function (SubTabID) {
        var SubTabDiv = $("#" + EncounterChargeCapture.params["PanelID"]).prevAll()
        SubTabDiv.each(function () {
            if ($(this).attr("id") == "DivEncounterTabs") {
                $(this).find("a").attr("class", "");
                $(this).find("a#" + SubTabID).attr("class", "active");
            }
        });
    },
    // adnan maqbool, pms-3942, 12/02/2016
    ClearFields: function () {
        if ($('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtPatientName').val() == "") {
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #lblAccount').show();
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #lnkAccountEdit').hide();
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #lblPatientName').removeClass("hidden");
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #lnkPatientNameEdit').addClass('hidden');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #hfPatientId').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtPatientFullName').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtReferralNumber').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #hfReferralNumerId').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtProvider').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #hfProvider').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtFacility').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #hfFacility').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtRefProvider').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #hfRefProvider').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtResourceProvider').val('');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #hfResourceProvider').val('');
            //$("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #dgvPatientInsurances").find("input[type='checkbox']").prop('disabled', false);
            $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #lstVisitPlans").find("input[type='checkbox']").prop('disabled', false);
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtVisitCopayment').val('');

            $bootstrapValidator = null;
            if ($('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                $bootstrapValidator = $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture').data('bootstrapValidator');
            }

            if ($bootstrapValidator) {
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Provider');
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Facility');
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'ResourceProvider');
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'PatientName');
            }

            //$('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #lblResourceProvider').removeClass("hidden");
            //$('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #lnkResourceProviderEdit').addClass('hidden');

            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtPatientFullName').trigger('onblur');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtReferralNumber').trigger('onblur');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtRefProvider').trigger('onblur');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtProvider').trigger('onblur');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtFacility').trigger('onblur');
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #txtResourceProvider').trigger('onblur');

        }
        else {
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #lblAccount').hide();
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #lnkAccountEdit').show();
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #aEncounter').removeClass('disableAll');
        }
    },
    // end
    FillVisitData: function (VisitId, PatientId, PatientInsuranceId) {
        var frmChargeCapture = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture');
        //if coming from Other Forms
        if (frmChargeCapture.find('#divVisitPlans').css("display") != "none" && EncounterChargeCapture.params["IsReload"] == null) {
            //frmChargeCapture.find('#divChargeDetails').toggleClass("col-lg-10");
        }
        else {
            frmChargeCapture.find('#divChargeDetails').toggleClass("col-lg-10");
        }
        if (PatientInsuranceId != null) {
            if (EncounterChargeCapture.params.ParentCtrl == "BillingInformation") {
                if ($("#divVisitPlans #lstVisitPlans li").length > 0) {
                    $("#divVisitPlans #lstVisitPlans li:first").addClass("active-plan");
                }
            } else {
                frmChargeCapture.find("#divVisitPlans #lstVisitPlans li").each(function () {
                    if ($(this).attr("id") == PatientInsuranceId)
                        $(this).addClass("active-plan");
                    else
                        $(this).removeClass("active-plan");
                });
            }
        } else {
            if (EncounterChargeCapture.params.ParentCtrl == "BillingInformation") {
                if ($("#divVisitPlans #lstVisitPlans li").length > 0) {
                    $("#divVisitPlans #lstVisitPlans li:first").addClass("active-plan");
                }
            }
        }

        frmChargeCapture.find('#chkBillToPatient').prop('disabled', false);
        EncounterChargeCapture.params.mode = "Edit";
        EncounterChargeCapture.params["VisitId"] = VisitId;
        EncounterChargeCapture.ShowChargeDetails(true);
        //Charge Editable Grid
        var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
        var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
        //$(ChargeGridId).dataTable().fnDestroy();
        $(ChargeGridId + " tbody tr").remove();
        EncounterChargeCapture.EditableGrid = utility.MakeEditableGrid(PanelChargeGrid, ChargeGridId, EncounterChargeCapture, "0", false, false, false, false);
        EncounterChargeCapture.ResetAllICDFields();
        //this dropdown not visible at all and looks like no use on this creen .
        // EncounterChargeCapture.LoadPatientLookups(PatientId);
        //////////////////////////
        EncounterChargeCapture.LoadVisit(PatientId, EncounterChargeCapture.params["VisitId"]);
        EncounterChargeCapture.Load_ClaimErrors(EncounterChargeCapture.params["VisitId"]);

    },
    // Begin Jan 5th, 2015, Author: Abdur Rehman Latif, No ticket Change for production
    FillDOS: function () {
        $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSFrom').val($.datepicker.formatDate('mm/dd/yy', new Date()));
        $('#' + EncounterChargeCapture.params.PanelID + ' #dtpDOSTo').val($.datepicker.formatDate('mm/dd/yy', new Date()));
    },
    // End Jan 5th, 2015, Author: Abdur Rehman Latif, No ticket Change for production
    ShowChargeDetails: function (IsShow) {
        var display = "none";
        if (IsShow) {
            display = "inline";
        }
        $('#' + EncounterChargeCapture.params["PanelID"] + ' div[divtype="ChargeDetail"]').each(function () {
            $(this).css("display", display);
        });
    },

    LoadClinicalNotes: function () {

        var providerId = $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #hfProvider').val();
        var notesId = $("#" + EncounterChargeCapture.params.PanelID + ' #hfNoteId').val();
        var patientId = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val()

        if (notesId != null && notesId != "") {


            var params = [];
            params["FromAdmin"] = "0";
            params["NotesId"] = notesId;
            params["PatientId"] = patientId;
            params["RefSearch"] = "DraftSearch";
            params["ProviderId"] = providerId;
            params["EnbtnViewCharges"] = true;
            params["IsFromEncounter"] = true;
            params["IsPhoneEncounter"] = EncounterChargeCapture.params.IsPhoneEncounter;
            params["ParentCtrl"] = "EncounterChargeCapture";// "BillingInformation" ? "EncounterChargeCapture" : EncounterChargeCapture.params.ParentCtrl; //Clinical_Notes
            LoadActionPan('Clinical_NotesView', params);
        } else {
            utility.DisplayMessages("There is no clinical note attached", 3);
        }
    },

    LoadDefaultData: function () {
        var self = null;
        //if (EncounterChargeCapture.params.PanelID == 'pnlEncounterChargeCapture')
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        //Start PMS-2387
        // Set Patient Banner Info into Patient Account/hiddenField/Patient Name
        utility.GetPatientBannerInfo(self.find("#frmEncounterChargeCapture #hfPatientId"), self.find("#txtPatientName"), self.find("#txtPatientFullName"), self.find("#hfRefProvider"), self.find("#txtRefProvider"), function () {
            //self.find("#txtPatientName").trigger("input");//.focus();
            EncounterChargeCapture.CheckSelfPay();
            EncounterChargeCapture.LoadPatientInsurances(self.find("#frmEncounterChargeCapture #hfPatientId").val());
            EncounterChargeCapture.LoadPatientCase(self.find("#frmEncounterChargeCapture #hfPatientId").val());
            EncounterChargeCapture.enablePatientControls();
            //EncounterChargeCapture.SelectPrimaryInsurance();
            //EncounterChargeCapture.FillReferralNumber();
            self.find('#aEncounter').removeClass('disableAll');
        });
        //End PMS-2387
        if (globalAppdata['DefaultProviderName'] != "" && globalAppdata['DefaultProviderId'] != "") {
            self.find('#txtProvider').val(globalAppdata['DefaultProviderName']);
            self.find('#hfProvider').val(globalAppdata['DefaultProviderId']);
            // start 04/02/2016 Muhammad Irfan for bug # PMS-3333
            if (EncounterChargeCapture.params.mode.toLowerCase() == 'add') {
                //self.find('#txtProvider').trigger('blur')
            }
            // end 04/02/2016 Muhammad Irfan for bug # PMS-3333

            //Begin Added on 10-Feb-2016 Azeem Raza Tayyab for bug # PMS-3856; Set Provider value as Resource Provider.
            self.find('#txtResourceProvider').val(globalAppdata['DefaultProviderName']);
            self.find('#hfResourceProvider').val(globalAppdata['DefaultProviderId']);
            //Begin Added on 10-Feb-2016 Azeem Raza Tayyab for bug # PMS-3856
        }
        else {
            if (Patient_Demographic.params.PatientProvider) {
                self.find('#txtProvider').val(Patient_Demographic.params.PatientProvider);
            }
            if (Patient_Demographic.params.PatientProviderId) {
                self.find('#hfProvider').val(Patient_Demographic.params.PatientProviderId);
            }

            if (Patient_Demographic.params.PatientProvider) {
                self.find('#txtResourceProvider').val(Patient_Demographic.params.PatientProvider);
            }
            if (Patient_Demographic.params.PatientProviderId) {
                self.find('#hfResourceProvider').val(Patient_Demographic.params.PatientProviderId);
            }
            //Begin Added on 10-Feb-2016 Azeem Raza Tayyab for bug # PMS-3856
        }
        if (self.find('#txtProvider').val() != "") {
            self.find('#lnkProviderEdit').css("display", "inline");
            self.find('#lblProvider').css("display", "none");
        }
        if (self.find('#txtResourceProvider').val() != "") {
            self.find('#lnkResourceProviderEdit').css("display", "inline");
            self.find('#lblResourceProvider').css("display", "none");
        }
        if (self.find('#txtRefProvider').val() != "") {
            self.find('#lnkRefProviderEdit').css("display", "inline");
            self.find('#lblRefProvider').css("display", "none");
        }
        if (globalAppdata['DefaultFacilityName'] != "" && globalAppdata['DefaultFacilityId'] != "") {
            self.find('#txtFacility').val(globalAppdata['DefaultFacilityName']);
            self.find('#hfFacility').val(globalAppdata['DefaultFacilityId']);

        }
        else {
            // Begin Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3140 Set the value from patient demographics
            if (Patient_Demographic.params.PatientFacility && Patient_Demographic.params.PatientFacilityId) {
                self.find('#txtFacility').val(Patient_Demographic.params.PatientFacility);
                self.find('#hfFacility').val(Patient_Demographic.params.PatientFacilityId);
            }
            // End Jan 6th, 2015, Author: Abdur Rehman Latif, PMS-3140 Set the value from patient demographics
        }

        if (self.find('#txtFacility').val() != "") {
            self.find('#lnkFacilityEdit').css("display", "inline");
            self.find('#lblFacility').css("display", "none");
        }

        if (globalAppdata['DefaultPracticeId'] != "")
            self.find('#hfPractice').val(globalAppdata['DefaultPracticeId']);


        EncounterChargeCapture.FillChargePOS('hfFacility', 'hfPOS');

    },

    //START Patient Encounter Visit Related Code
    CloseVisitTab: function (obj) {
        var TabID = $(obj).parent().attr("id");

        var tabsArr = [];
        TabsArray.filter(function (obj) {
            if (obj.MasterTabID == TabID) {
                RemoveTab(obj);
            }


        });

        var Tab = GetTab(TabID);
        $(obj).parent().remove();
        RemoveTab(Tab);
        EncounterChargeCapture.EditableGrid.datatable().fnDestroy();

        EncounterChargeCapture.bIsFirstLoad = true;

        SelectCurrentTab("patTabEncounter", true);
        $('#patTabEncounter').trigger("click");
    },

    //LoadAllAutocomplete: function () {


    //    //EncounterChargeCapture.BindPatientAccount();
    //},
    BindPatientPAN: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var PatientId = self.find("#frmEncounterChargeCapture #hfPatientId").val();
        CacheManager.BindPatientData('GetPatientPAN', true, PatientId).done(function (result) {
            var Ctrl = self.find("input#txtPriorAuthNumber");
            var hfCtrl = self.find("#hfAuthorizeId");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", PatientCase, null, hfCtrl);
        });
    },
    BindProvider: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        //load only if provider list is empty
        if (Providers.length == 0) {
            CacheManager.BindCodes('GetProvider', false).done(function (result) {
                var Ctrl = self.find("input#txtProvider");
                var hfCtrl = self.find("#hfProvider");
                var onChange = function (valid) {
                    EncounterChargeCapture.FillPlanCopayment('#frmEncounterChargeCapture #ddlPatientInsurance', false, '', true);
                    EncounterChargeCapture.FillReferralNumber();
                    EncounterChargeCapture.CopayTypeConfirmation();
                }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, null, onChange);

                var Ctrl = self.find("input#txtResourceProvider");
                var hfCtrl = self.find("#hfResourceProvider");
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);

                var Ctrl = self.find("input#txtSupervisingProvider");
                var hfCtrl = self.find("#hfSupervisingProvider");
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);

                var Ctrl = self.find("input#txtOrderingProvider");
                var hfCtrl = self.find("#hfOrderingProvider");                
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
                //**** Anesthesia Code
                var anes_array = $.grep(Providers, function (v) {
                    return v.SpecialityName.toLowerCase() == 'anesthesiology';
                });

                var Ctrl = self.find("input#txtAnesthesiologist");
                var hfCtrl = self.find("#hfAnesthesiologist");
                var onChange = function (valid) {
                    if (self.find('#radAnesthesiologist').is(':checked')) {
                        var selectedProviderName = self.find("input#txtAnesthesiologist").val();
                        var selectedProviderId = self.find("input#hfAnesthesiologist").val();

                        // setting provider
                        self.find("input#txtProvider").val(selectedProviderName);
                        self.find("input#hfProvider").val(selectedProviderId);

                        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Provider');

                        }

                        // setting resource provider
                        self.find("input#txtResourceProvider").val(selectedProviderName);
                        self.find("input#hfResourceProvider").val(selectedProviderId);
                        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'ResourceProvider');

                        }
                    }
                }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", anes_array, null, hfCtrl, null, onChange);

                var crna_array = $.grep(Providers, function (v) {
                    return v.SpecialityName.toLowerCase() == 'crna';
                });

                var Ctrl = self.find("input#txtCRNA");
                var hfCtrl = self.find("#hfCRNA");
                var onChange = function (valid) {
                    if (self.find('#radCRNA').is(':checked')) {
                        var selectedProviderName = self.find("input#txtCRNA").val();
                        var selectedProviderId = self.find("input#hfCRNA").val();

                        // setting provider
                        self.find("input#txtProvider").val(selectedProviderName);
                        self.find("input#hfProvider").val(selectedProviderId);
                        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Provider');

                        }

                        // setting resource provider
                        self.find("input#txtResourceProvider").val(selectedProviderName);
                        self.find("input#hfResourceProvider").val(selectedProviderId);
                        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'ResourceProvider');

                        }
                    }
                }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, null, onChange);
                //**** End Anesthesia Code
            });
        }
        else {

            var sourceProviderArr = self.find("input#txtProvider").data('kendoAutoComplete');
            if (!sourceProviderArr) {
                var Ctrl = self.find("input#txtProvider");
                var hfCtrl = self.find("#hfProvider");
                var onChange = function (valid) {
                    EncounterChargeCapture.FillPlanCopayment('#frmEncounterChargeCapture #ddlPatientInsurance', false, '', true);
                    EncounterChargeCapture.FillReferralNumber();
                    EncounterChargeCapture.CopayTypeConfirmation();
                }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, null, onChange);
            }

            var sourceResProviderArr = self.find("input#txtResourceProvider").data('kendoAutoComplete');
            if (!sourceProviderArr) {
                var Ctrl = self.find("input#txtResourceProvider");
                var hfCtrl = self.find("#hfResourceProvider");
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
            }

            var sourceSupProviderArr = self.find("input#txtSupervisingProvider").data('kendoAutoComplete');
            if (!sourceSupProviderArr) {
                var Ctrl = self.find("input#txtSupervisingProvider");
                var hfCtrl = self.find("#hfSupervisingProvider");
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
            }

            var sourceOrderingProviderArr = self.find("input#txtOrderingProvider").data('kendoAutoComplete');
            if (!sourceOrderingProviderArr) {
                var Ctrl = self.find("input#txtOrderingProvider");
                var hfCtrl = self.find("#hfOrderingProvider");
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl);
            }

            var sourceAnesArr = self.find("input#txtAnesthesiologist").data('kendoAutoComplete');
            if (!sourceAnesArr) {

                //**** Anesthesia Code
                var anes_array = $.grep(Providers, function (v) {
                    return v.SpecialityName.toLowerCase() == 'anesthesiology';
                });
                var Ctrl = self.find("input#txtAnesthesiologist");
                var hfCtrl = self.find("#hfAnesthesiologist");
                var onChange = function (valid) {
                    if (self.find('#radAnesthesiologist').is(':checked')) {
                        var selectedProviderName = self.find("input#txtAnesthesiologist").val();
                        var selectedProviderId = self.find("input#hfAnesthesiologist").val();

                        // setting provider
                        self.find("input#txtProvider").val(selectedProviderName);
                        self.find("input#hfProvider").val(selectedProviderId);

                        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Provider');

                        }

                        // setting resource provider
                        self.find("input#txtResourceProvider").val(selectedProviderName);
                        self.find("input#hfResourceProvider").val(selectedProviderId);
                        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'ResourceProvider');

                        }
                    }
                }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", anes_array, null, hfCtrl, null, onChange);
            }


            var sourceCRNAArr = self.find("input#txtCRNA").data('kendoAutoComplete');
            if (!sourceCRNAArr) {

                var crna_array = $.grep(Providers, function (v) {
                    return v.SpecialityName.toLowerCase() == 'crna';
                });
                var Ctrl = self.find("input#txtCRNA");
                var hfCtrl = self.find("#hfCRNA");
                var onChange = function (valid) {
                    if (self.find('#radCRNA').is(':checked')) {
                        var selectedProviderName = self.find("input#txtCRNA").val();
                        var selectedProviderId = self.find("input#hfCRNA").val();

                        // setting provider
                        self.find("input#txtProvider").val(selectedProviderName);
                        self.find("input#hfProvider").val(selectedProviderId);
                        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Provider');

                        }

                        // setting resource provider
                        self.find("input#txtResourceProvider").val(selectedProviderName);
                        self.find("input#hfResourceProvider").val(selectedProviderId);
                        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'ResourceProvider');

                        }
                    }
                }
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Providers, null, hfCtrl, null, onChange);

            }
        }

    },
    UpdatehfOrderingProvider: function () {
        if (!$('#' +EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #txtOrderingProvider').val()) {
      
                $('#' +EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #hfOrderingProvider').val("");
        }
     },
        

    BindFacility: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        if (Facilities.length == 0) {
            CacheManager.BindCodes('GetFacility', false).done(function (result) {
                var Ctrl = self.find("input#txtFacility");
                var hfCtrl = self.find("#hfFacility");
                var onSelect = function (e) { self.find("#hfPractice").val(e.PracticeId); };
                var onChange = function (valid) { EncounterChargeCapture.FillReferralNumber(); EncounterChargeCapture.FillChargePOS('hfFacility', 'hfPOS'); };
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect, onChange);
            });
        }
        else {
            var sourceArr = self.find("input#txtFacility").data('kendoAutoComplete');
            if (!sourceArr) {
                var Ctrl = self.find("input#txtFacility");
                var hfCtrl = self.find("#hfFacility");
                var onSelect = function (e) { self.find("#hfPractice").val(e.PracticeId); };
                var onChange = function (valid) { EncounterChargeCapture.FillReferralNumber(); EncounterChargeCapture.FillChargePOS('hfFacility', 'hfPOS'); };
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", Facilities, null, hfCtrl, onSelect, onChange);
            }

        }
    },
    BindRefProvider: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);


        if (RefProviders.length == 0) {
            CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
                var Ctrl = self.find("input#txtRefProvider");
                var hfCtrl = self.find("#hfRefProvider");
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", RefProviders, null, hfCtrl);
            });
        }
        else {

            var sourceArr = self.find("input#txtRefProvider").data('kendoAutoComplete');
            if (!sourceArr) {
                var Ctrl = self.find("input#txtRefProvider");
                var hfCtrl = self.find("#hfRefProvider");
                utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", RefProviders, null, hfCtrl);
            }

        }
    },
    LoadPatientLookups: function (PatientId) {
        CacheManager.BindDropDownsByID("#" + EncounterChargeCapture.params["PanelID"] + ' #ddlLastVisits', 'GetPatientVisits', true, PatientId);
    },
    LoadProvider: function () {
        var self = "";
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var ProviderId = self.find('#hfProvider').val();
        Admin_Provider.SearchProvider('', ProviderId).done(function (response) {
            if (response.status != false) {
                if (response.ProviderCount > 0) {
                    var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);
                    self.find("#hfProviderCLIA").val(ProviderLoadJSONData[0].CLIA)
                }
            }
        });

    },
    Loadfacility: function () {
        var self = "";
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }
        var facilityId = self.find("#hfFacility").val();
        Admin_Facility.SearchFacility(null, facilityId).done(function (response) {
            if (response.status != false) {
                var FacilityLoadJSONData = JSON.parse(response.FacilityLoad_JSON)[0];
                $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfFacilityCLIA').val(FacilityLoadJSONData.CLIA);
            }
        });
    },
    LoadPatientInsurances: function (PatientId, isFromSyncBtn) {
        if (EncounterChargeCapture.params.mode.toLowerCase() == 'add') {
            if (PatientId != "") {
                EncounterChargeCapture.SearchVisitInsurance(null, PatientId).done(function (response) {
                    if (response.status != false) {
                        EncounterChargeCapture.LoadVisitInsurance(response, isFromSyncBtn);
                        //  EncounterChargeCapture.ShowHidInsuranceSyncButton();
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            }
        }
        else if (EncounterChargeCapture.params.mode.toLowerCase() == 'edit') {
            EncounterChargeCapture.SearchVisitInsurance(EncounterChargeCapture.params["VisitId"], null).done(function (response) {
                if (response.status != false) {
                    if (isFromSyncBtn && response.PatientInsuranceCount > 0) {
                        var self = null;
                        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
                        else
                            self = $('#' + EncounterChargeCapture.params.PanelID);

                        self.find('#chkBillToPatient').prop('checked', false);

                    }
                    EncounterChargeCapture.LoadVisitInsurance(response, isFromSyncBtn);
                    //  EncounterChargeCapture.ShowHidInsuranceSyncButton();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }

    },
    SearchVisitInsurance: function (visitId, patientId) {
        var data = "VisitId=" + visitId + "&PatientId=" + patientId;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "SEARCH_VISIT_INSURANCE");
    },


    LoadVisitInsurance: function (response, isFromSyncBtn) {
        EncounterChargeCapture.LoadInsuranceGrid(response, isFromSyncBtn);
        if (EncounterChargeCapture.params.ParentCtrl == "BillingInformation") {
            if ($("#divVisitPlans #lstVisitPlans li").length > 0) {
                $("#divVisitPlans #lstVisitPlans li:first").addClass("active-plan");
            }
        }
        EncounterChargeCapture.SelectPrimaryInsurance(isFromSyncBtn);
        EncounterChargeCapture.FillReferralNumber();
        if (EncounterChargeCapture.params["CaseNo"]) {
            EncounterChargeCapture.FillCaseDetail(EncounterChargeCapture.params["CaseNo"]);
        }
    },
    LoadPatientCase: function (PatientId) {
        CacheManager.BindPatientData('GetPatientCase', true, PatientId).done(function (result) {
            var Ctrl = $('#' + EncounterChargeCapture.params["PanelID"] + " input#txtCaseNumber");
            var hfCtrl = $("#" + EncounterChargeCapture.params["PanelID"] + " #hfCaseId");
            var onChange = function (valid) { EncounterChargeCapture.FillCaseDetail(Ctrl.val()) };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", PatientCase, null, hfCtrl, null, onChange);
        });
    },

    InsuranceBalanceAlert: function (Option) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var PatientHasCharges = false;
        var InsuranceID = "";
        if (Option) {
            InsuranceID = $(Option).attr("insuranceid");
        }
        var BillToPatient = self.find('#chkBillToPatient').is(':checked');
        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tr").each(function (i, row) {
            if ($(this).attr("id") && $(this).attr("id") != null) {
                var $row = $(row);
                var txtObj = $row.find('input[id*="txtPATCharges"]');
                if (txtObj != undefined && $(txtObj).val() != "0.00" && $(txtObj).val() != "" && $(txtObj).val() != "0") {
                    PatientHasCharges = true;
                }
            }
        });
        if (PatientHasCharges && InsuranceID != "" && EncounterChargeCapture.IsSelfPay == "False" && BillToPatient == false) {
            utility.myConfirm("Would you like to allocate the balance to the insurance", function () {
                $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tr").each(function (i, row) {
                    if ($(this).attr("id") && $(this).attr("id") != null) {
                        var $row = $(row);
                        if ($row.find('input[id*="txtPATCharges"]').val() != "" && $row.find('input[id*="txtPATCharges"]').val() != "0.00") {
                            PatCharges = Number($row.find('input[id*="txtPATCharges"]').val() == "" ? 0 : $row.find('input[id*="txtPATCharges"]').val());
                            INSCharges = Number($row.find('input[id*="txtINSCharges"]').val() == "" ? 0 : $row.find('input[id*="txtINSCharges"]').val());

                            $row.find('input[id*="txtINSCharges"]').val(parseFloat(PatCharges + INSCharges).toFixed(2));
                            $row.find('input[id*="txtPATCharges"]').val("0.00");
                        }
                    }

                });

            }, function () {
                // no case
                //Begin Added By Azeem Raza Tayyab on 29-Mar-2016 to Fix Bug#:PMS-4682
                if (InsuranceID) {
                    //self.find("#dgvPatientInsurances tbody tr[insuranceid='" + InsuranceID + "']").find("input[type='checkbox']").prop('checked', false);
                    self.find("#lstVisitPlans #" + InsuranceID).find("input[type='checkbox']").prop('checked', false);
                }
                //else {
                //  self.find("#dgvPatientInsurances tbody tr[insuranceid='" + InsuranceID + "']").find("input[type='checkbox']").prop('checked', true);
                // }

                return false;

            }, 'Confirmation Alert');
        }
    },

    FillPlanCopayment: function (obj, Ischeck, Ctrl, isFromProvider) {
        if (Ischeck != undefined)
            EncounterChargeCapture.IsPatientInsuranceChange = Ischeck;

        var Option = EncounterChargeCapture.SelectedInsurancePlan;
        if (EncounterChargeCapture.IsCallChange == true && Ctrl == "InsurancePlan") {
            // EncounterChargeCapture.InsuranceBalanceAlert(Option);
        }
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        if (EncounterChargeCapture.IsSelfPay == "False") {
            if (self.find('#radPCP').is(':checked')) {
                self.find('#txtVisitCopayment').val(utility.convertToFigure($(Option).attr("AmtCopay")));
            } else if (self.find('#radSpecialist').is(':checked')) {
                if (Option && $(Option).attr("SpecialistCopay")) {
                    self.find('#txtVisitCopayment').val(utility.convertToFigure($(Option).attr("SpecialistCopay")));
                }
            }
        }
        else {
            self.find('#txtVisitCopayment').val(Number(0).toFixed(globalAppdata.DecimalPlaces));
        }
        var ProviderId = self.find('#hfProvider').val();
        var ProviderId = ProviderId != "" ? parseInt(ProviderId) : 0;
        if (ProviderId > 0) {
            Admin_Provider.SearchProvider('', ProviderId).done(function (response) {
                if (response.status != false) {
                    if (response.ProviderCount > 0) {

                        var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);



                        if (EncounterChargeCapture.params.mode.toLowerCase() == "edit") {

                        } else if (EncounterChargeCapture.params.mode.toLowerCase() == 'add') {
                            if (ProviderLoadJSONData[0].IsSpecialist == "False") {
                                if (Option && $(Option).attr("AmtCopay") && EncounterChargeCapture.IsSelfPay == "False") {
                                    self.find('#txtVisitCopayment').val(utility.convertToFigure($(Option).attr("AmtCopay")));
                                }
                                self.find('#radPCP').trigger("click");
                            }
                            else {
                                if (Option && $(Option).attr("SpecialistCopay") && EncounterChargeCapture.IsSelfPay == "False") {
                                    self.find('#txtVisitCopayment').val(utility.convertToFigure($(Option).attr("SpecialistCopay")));
                                }
                                self.find('#radSpecialist').trigger("click");
                            }

                        }
                        EncounterChargeCapture.SetChargeVisitCopay();
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            if (Option && $(Option).attr("AmtCopay") && EncounterChargeCapture.IsSelfPay == "False") {
                self.find('#txtVisitCopayment').val(utility.convertToFigure($(Option).attr("AmtCopay")));
                EncounterChargeCapture.SetChargeVisitCopay();
            }
        }
        EncounterChargeCapture.setClaimTypeAlert($(Option).attr("claimtypeid"), $(Option).attr("claimtypename"));
        EncounterChargeCapture.SetDefaultSettingByInsurance(self, isFromProvider);
        if (EncounterChargeCapture.IsSelfPay == "False" && EncounterChargeCapture.params.mode.toLowerCase() == "edit" && EncounterChargeCapture.SelectedInsurancePlan) {
            if (EncounterChargeCapture.PreviouseInsPlan == null && Ctrl == "InsurancePlan" && self.find('#txtVisitCopayment').val() != "0.00" && self.find('#txtVisitCopayment').val() != "0" && self.find('#txtVisitCopayment').val() != "") {
                var CurrentRow = $("#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result #dgvVisitCharge tbody tr:first");
                var txtCopay = $(CurrentRow).find('input[id*="txtCOPAY"]');
                var copayAmount = $(txtCopay).val();
                if (CurrentRow.attr("ChargeStatus") != undefined && CurrentRow.attr("ChargeStatus") != null && CurrentRow.attr("ChargeStatus").toLowerCase() == "regular") {
                    utility.myConfirm('Are you sure you want to charge copay ?', function () {
                        $(txtCopay).val($('#' + EncounterChargeCapture.params.PanelID + " #txtVisitCopayment").val());
                    },
                    function () {
                        $(txtCopay).val(copayAmount);
                        return false;
                    }, 'Confirmation');;
                }
            }
        }

    },

    SetSubmissionMode: function () {
        EncounterChargeCapture.IsSetSubmissionMode = true;
    },

    setClaimTypeAlert: function (claimtypeId, claimtypename) {

        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var val1_ = self.find('#ddlClaimType').val();
        var text1_ = self.find('#ddlClaimType').find("option:selected").text();

        if (EncounterChargeCapture.IsPatientInsuranceChange == true && EncounterChargeCapture.IsSelfPay == "False") {
            if (val1_ != "") {
                if (val1_ != claimtypeId) {

                    //alert user.
                    var message_ = "Do you want to create " + utility.getAorAn(claimtypename) + " <b>" + claimtypename + "</b> claim ?";

                    if (claimtypeId != "")
                        utility.myConfirm(message_, function () {

                            self.find('#ddlClaimType').val(claimtypeId);
                            self.find('#ddlClaimType').trigger("change");

                        }, function () { }, 'Confirm Claim Type');

                }
                else {
                    self.find('#ddlClaimType').val(claimtypeId);
                    self.find('#ddlClaimType').trigger("change");
                }
            }
            else {
                self.find('#ddlClaimType').val(claimtypeId);
                self.find('#ddlClaimType').trigger("change");
            }
        }
        else
            EncounterChargeCapture.IsPatientInsuranceChange = true;


    },

    setVisitCopayment: function (obj) {
        var selectedPlan = EncounterChargeCapture.SelectedInsurancePlan;

        var currentObject = $(obj).attr('id');
        if (EncounterChargeCapture.IsSelfPay == "False") {
            if (currentObject == "radPCP") {
                if (selectedPlan && $(selectedPlan).attr("AmtCopay"))
                    $("#" + EncounterChargeCapture.params["PanelID"] + ' #txtVisitCopayment').val(utility.convertToFigure($(selectedPlan).attr("AmtCopay")));

            }
            else {
                if (selectedPlan && $(selectedPlan).attr("SpecialistCopay"))
                    $("#" + EncounterChargeCapture.params["PanelID"] + ' #txtVisitCopayment').val(utility.convertToFigure($(selectedPlan).attr("SpecialistCopay")));
            }
        }
    },

    FillReferralNumber: function () {

        //EncounterChargeCapture.IsPatientInsuranceChange = false;
        var ParentCtrl = "";
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }

        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == 'BillingInformation' || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            ParentCtrl = 'EncounterChargeCapture';
        else
            ParentCtrl = EncounterChargeCapture.params["TabID"];

        var patientID = self.find("#frmEncounterChargeCapture #hfPatientId").val();
        var selectedInsurancePlan = "";
        selectedInsurancePlan = EncounterChargeCapture.GetSelectedInsurancePlan();

        var objReferralNumber = self.find("#txtReferralNumber");
        objReferralNumber.val("");
        self.find("#lnkReferralNumberEdit").hide();
        self.find("#lblReferralNumber").show();
        var ProviderId = self.find("#hfProvider").val();
        var objRefProvider = self.find("#txtRefProvider");
        var FacilityId = self.find("#hfFacility").val();
        var VisitDate = self.find("#dtpDOSFrom").val();
        /*if (selectedInsurancePlan == "" || selectedInsurancePlan == null) {
            objReferralNumber.attr("disabled", "disabled");
        }
        else {
            objReferralNumber.removeAttr("disabled");
        }*/

        if (selectedInsurancePlan != undefined && selectedInsurancePlan != "" && ProviderId != "" && FacilityId != "" && VisitDate != "") {
            patientReferralSearch.SearchReferral("Incoming", selectedInsurancePlan, ProviderId, FacilityId, VisitDate, "1").done(function (response) {
                if (response.status != false) {
                    var PatientReferral = [];
                    if (response.ReferralCount > 0) {
                        var ReferralJSONData = JSON.parse(response.ReferralLoad_JSON);
                        $.each(ReferralJSONData, function (i, item) {
                            PatientReferral.push({ id: item.PatientReferralId, value: item.ReferralAuthNo });
                        });

                        var Ctrl = self.find("#txtReferralNumber");
                        var hfCtrl = self.find("#hfReferralNumerId");
                        utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", PatientReferral, null, hfCtrl);

                        if (PatientReferral.length == 1) {

                            if ((ReferralJSONData[0]["VisitsUsed"]) >= (ReferralJSONData[0]["VisitsAllowed"])) {

                                if (EncounterChargeCapture.params.mode.toLowerCase() == "add") {
                                    self.find("#txtReferralNumber").val("");
                                    self.find("#hfReferralNumerId").val("");
                                    if (self.find("#txtReferralNumber").val() == "") {
                                        self.find("#lnkReferralNumberEdit").hide();
                                        self.find("#lblReferralNumber").show();
                                    }
                                }

                            } else {
                                if (EncounterChargeCapture.params.mode.toLowerCase() == "add" && self.find("#txtReferralNumber").val() == "") {
                                    self.find("#txtReferralNumber").val(PatientReferral[0].value);
                                    self.find("#hfReferralNumerId").val(PatientReferral[0].id);
                                }
                            }
                            //EncounterChargeCapture.ValidateReferralNumber(self.find("#txtReferralNumber"));
                        }

                    }
                    else {
                        self.find("#lnkReferralNumberEdit").hide();
                        self.find("#lblReferralNumber").show();
                    }
                }
            });

        }

        //Lock Claim Fields
        EncounterChargeCapture.LockClaim();
    },

    OpenReferralSearch: function (selectedInsurancePlan, CtrlReferralNo, CtrlRefProvider, ReferralResponse, ParentCtrl, patientID, ReferralType, ProviderId, FacilityId, ReferralDate, Active, FacilityToId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Referral Management", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                setTimeout(function () {
                    var params = [];
                    params["RefCtrlReferralNo"] = CtrlReferralNo;
                    params["RefCtrlRefProvider"] = CtrlRefProvider;
                    params["patientID"] = patientID;
                    params["ReferralResponse"] = ReferralResponse;
                    params["patientInsuranceID"] = selectedInsurancePlan;
                    params["ParentCtrl"] = ParentCtrl;

                    params["ReferralType"] = ReferralType;
                    params["ProviderId"] = ProviderId;
                    params["FacilityId"] = FacilityId;
                    params["ReferralDate"] = ReferralDate;
                    params["Active"] = Active;
                    params["FacilityToId"] = FacilityToId;

                    LoadActionPan('patientReferralSearch', params);
                }, 510);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.ParentCtrl == 'BillingInformation' || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        LoadActionPan('Patient_Search', params);
    },

    PatientDemographics: function (patientid) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Demographic", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["mode"] = 'Edit';
                params["PatBanner"] = true;
                params["patientID"] = patientid;
                params["IsFill"] = false;
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = "EncounterChargeCapture";
                LoadActionPan('demographicDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    FillPatientInfoFromSearch: function (PatientId, patFullName, RefProviderId, RefProviderName, ProviderId, ProviderName, FacilityId, FaciltyName, SelfPay, event, BillingProvider, ResourceProviderId, ResourceProviderName, PatientDOB, PatientGender) {
        if (event != null) {
            event.stopPropagation();
        }
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        self.find("#frmEncounterChargeCapture #hfPatientId").val(PatientId);
        EncounterChargeCapture.IsSelfPay = SelfPay == "" ? "False" : SelfPay;
        EncounterChargeCapture.BindSelfPayToBillToPatient();
        //EncounterChargeCapture.SelectPrimaryInsurance();
        //EncounterChargeCapture.FillReferralNumber();

        self.find("#txtPatientName").val(patFullName.split(" - ")[0]);
        self.find("#txtPatientFullName").val(patFullName.split(" - ")[1]);
        utility.SetKendoAutoCompleteSourceforValidate(self.find("#txtPatientName"), patFullName.split(" - ")[0], self.find("#frmEncounterChargeCapture #hfPatientId"), PatientId, "AccountNumber");
        EncounterChargeCapture.calculateAge(PatientDOB);
        // Faizan
        var dateofBirth = new Date(PatientDOB);
        if (dateofBirth != undefined) {
            var BirthDay = (dateofBirth.getMonth() + 1) + '/' + dateofBirth.getDate() + '/' + dateofBirth.getFullYear()
            self.find("#txtDOB").val(BirthDay);
        }
        if (PatientGender)
            self.find("#txtSex").val(PatientGender.charAt(0));
        //self.find("#txtPatientName").trigger("input");//.focus();
        if (EncounterChargeCapture.params["FromCreateClaim"] && ProviderId && ProviderName) {
            self.find('#hfProvider').val(ProviderId);
            self.find('#txtProvider').val(ProviderName);
            if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Provider');
            }

            if (EncounterChargeCapture.params.SpecialityName.toLowerCase() == 'anesthesiology') {
                self.find('#hfAnesthesiologist').val(ProviderId);
                self.find('#txtAnesthesiologist').val(ProviderName);

                if (self.find('#txtAnesthesiologist').val() != "") {
                    self.find('#lnkAnesthesiologistEdit').css("display", "inline");
                    self.find('#lblAnesthesiologist').css("display", "none");
                }
                if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                    $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Anesthesiologist');
                }
                self.find('#radAnesthesiologist').trigger('click');

            } else if (EncounterChargeCapture.params.SpecialityName.toLowerCase() == 'crna') {
                self.find('#hfCRNA').val(ProviderId);
                self.find('#txtCRNA').val(ProviderName);

                if (self.find('#txtCRNA').val() != "") {
                    self.find('#lnkCRNAEdit').css("display", "inline");
                    self.find('#lblCRNA').css("display", "none");
                }
                if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                    $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'CRNA');
                }
                self.find('#radCRNA').trigger('click');
            }
        }
        else if ((globalAppdata['DefaultProviderName'] == "" || globalAppdata['DefaultProviderName'] == "- Select -") && globalAppdata['DefaultProviderId'] == "") {
            self.find('#hfProvider').val(ProviderId);
            self.find('#txtProvider').val(ProviderName);
            if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Provider');
            }

        }

        if (self.find('#txtProvider').val() != "") {
            self.find('#lnkProviderEdit').css("display", "inline");
            self.find('#lblProvider').css("display", "none");
        }
        //Begin Added on 10-Feb-2016 Azeem Raza Tayyab for bug # PMS-3856; Set Provider value as Resource Provider.
        if (EncounterChargeCapture.params["FromCreateClaim"]) {
            if (ResourceProviderId && ResourceProviderName) {
                self.find('#hfResourceProvider').val(ResourceProviderId);
                self.find('#txtResourceProvider').val(ResourceProviderName);
                if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                    $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'ResourceProvider');
                }
            }
        }
        else if ((globalAppdata['DefaultProviderName'] == "" || globalAppdata['DefaultProviderName'] == "- Select -") && globalAppdata['DefaultProviderId'] == "") {
            self.find('#hfResourceProvider').val(ProviderId);
            self.find('#txtResourceProvider').val(ProviderName);
            if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'ResourceProvider');
            }
        }

        if (self.find('#txtResourceProvider').val() != "") {
            self.find('#lnkResourceProviderEdit').css("display", "inline");
            self.find('#lblResourceProvider').css("display", "none");
        }
        //End Added on 10-Feb-2016 Azeem Raza Tayyab for bug # PMS-3856; Set Provider value as Resource Provider.
        if (EncounterChargeCapture.params["FromCreateClaim"] && FacilityId && FaciltyName) {
            self.find('#hfFacility').val(FacilityId);
            self.find('#txtFacility').val(FaciltyName);
            if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Facility');
            }
        }
        else if ((globalAppdata['DefaultFacilityName'] == "" || globalAppdata['DefaultFacilityName'] == "- Select -") && globalAppdata['DefaultFacilityId'] == "") {
            self.find('#hfFacility').val(FacilityId);
            self.find('#txtFacility').val(FaciltyName);
            if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Facility');
            }
        }
        if (self.find('#txtFacility').val() != "") {
            self.find('#lnkFacilityEdit').css("display", "inline");
            self.find('#lblFacility').css("display", "none");
        }
        //setting ref provider

        // Begin 08-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3246
        if (RefProviderId && RefProviderName) {
            self.find('#hfRefProvider').val(RefProviderId);
            self.find('#txtRefProvider').val(RefProviderName);
            if (self.find('#txtRefProvider') != "") {
                self.find('#lnkRefProviderEdit').css("display", "inline");
                self.find('#lblRefProvider').css("display", "none");
            }

        }
        else {
            self.find('#hfRefProvider').val("");
            self.find('#txtRefProvider').val("");
            self.find('#lnkRefProviderEdit').css("display", "none");
            self.find('#lblRefProvider').css("display", "inline");
        }

        if (self.find('#txtRefProvider').val() != "") {
            self.find('#lnkRefProviderEdit').css("display", "inline");
            self.find('#lblRefProvider').css("display", "none");
        }

        //setTimeout(function () { self.find("#txtPatientName").focus(); }, 100);
        if (self.find("#frmEncounterChargeCapture #hfPatientId").val() != "") {
            self.find("#lnkAccountEdit").show();
            self.find("#lblAccount").hide();
            self.find("#lnkPatientNameEdit").removeClass("hidden");
            self.find("#frmEncounterChargeCapture #lblPatientName").addClass("hidden");
            self.find('#aEncounter').removeClass('disableAll');
        } else {
            self.find("#lnkAccountEdit").hide();
            self.find("#lblAccount").show();
            self.find("#lnkPatientNameEdit").addClass("hidden");
            self.find("#frmEncounterChargeCapture #lblPatientName").removeClass("hidden");
        }

        // End 08-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3246
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView") {
            UnloadActionPan('EncounterChargeCapture');
            // Start 30/01/2016 Muhammad Irfan for bug # 3742
            if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'PatientName');
            }
            // End 30/01/2016 Muhammad Irfan for bug # 3742
        } else {
            UnloadActionPan(EncounterChargeCapture.params["TabID"]);
            // Start 30/01/2016 Muhammad Irfan for bug # 3742
            if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'PatientName');
            }
            // End 30/01/2016 Muhammad Irfan for bug # 3742
        }
        EncounterChargeCapture.enablePatientControls();
        EncounterChargeCapture.LoadPatientInsurances(PatientId);
        utility.InsertRecentPatient(PatientId);
        if (BillingProvider) {
            if ((globalAppdata['DefaultBillingProviderId'] == "" || globalAppdata['DefaultBillingProviderId'] == "- Select -")) { }
            self.find('#ddlBillingProvider option').filter(function () {
                return $.trim($(this).val()) == BillingProvider
            }).attr('selected', true);
        }
    },

    enablePatientControls: function () {


        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }

        AccountNo = self.find("#txtPatientName").val();
        if (AccountNo != null && AccountNo == "") {

            self.find("#divCaseNumber, #divPatientInsurancePlan").each(function () {
                $(this).addClass('disableAll');
            });

        }
        else if (AccountNo != null && AccountNo != "") {

            self.find("#divCaseNumber, #divPatientInsurancePlan").each(function () {
                $(this).removeClass('disableAll');
            });
        }
        if (self.find("#frmEncounterChargeCapture #hfPatientId").val() != "") {
            self.find("#lnkAccountEdit").show();
            self.find("#lblAccount").hide();
            self.find("#lnkPatientNameEdit").removeClass("hidden");
            self.find("#frmEncounterChargeCapture #lblPatientName").addClass("hidden");
        } else {
            self.find("#lnkAccountEdit").hide();
            self.find("#lblAccount").show();
            self.find("#lnkPatientNameEdit").addClass("hidden");
            self.find("#frmEncounterChargeCapture #lblPatientName").removeClass("hidden");
        }

    },

    BindPatientAccount: function () {
        var valid = false;
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }
        var Ctrl = self.find("#txtPatientName");
        var hfCtrl = self.find("#frmEncounterChargeCapture #hfPatientId");
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.AccountNumber && obj.AccountNumber.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.AccountNumber;
                    return true;
                }
                else { return false; }
            });
            if (haveObject.length > 0) {
                if (hfCtrl)
                    $(hfCtrl).val(id_);
                this.value(value_);
                $(link).show();
                $(link).prev().hide();
                self.find("#lnkPatientNameEdit").show();
                self.find("#frmEncounterChargeCapture #lblPatientName").hide();
                self.find('#aEncounter').removeClass('disableAll');
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
                self.find("#lnkPatientNameEdit").hide();
                self.find("#frmEncounterChargeCapture #lblPatientName").show();
                self.find('#aEncounter').addClass('disableAll');
            }

            EncounterChargeCapture.enablePatientControls();
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            self.find("#txtPatientName").val(dataItem.AccountNumber);
            self.find("#txtPatientFullName").val(dataItem.FullName);
            EncounterChargeCapture.IsSelfPay = dataItem.SelfPay == "" ? "False" : dataItem.SelfPay;
            EncounterChargeCapture.BindSelfPayToBillToPatient();
            $bootstrapValidator = null;
            if (self.find('#frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof self.find('#frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                $bootstrapValidator = self.find('#frmEncounterChargeCapture').data('bootstrapValidator');
            }

            if ($bootstrapValidator) {
                $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'PatientName');
            }

            if (dataItem.RefferingProviderName != "") {
                self.find('#txtRefProvider').val(dataItem.RefferingProviderName);
                self.find('#hfRefProvider').val(dataItem.RefferingProviderId);
                self.find("#lnkRefProviderEdit").css("display", "inline");
                self.find("#lblRefProvider").css("display", "none");

            } else {
                self.find('#txtRefProvider').val("");
                self.find('#hfRefProvider').val("");
                self.find("#lnkRefProviderEdit").css("display", "none");
                self.find("#lblRefProvider").css("display", "inline");
            }
            // end 08-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3246

            // Edited by Azeem Raza Tayyab on 19-Apr-2016 to fix bug#:PMS-4898
            if ((globalAppdata['DefaultProviderName'] == "" || globalAppdata['DefaultProviderName'] == "- Select -") && globalAppdata['DefaultProviderId'] == "") {
                if (dataItem.Providername != "") {
                    self.find('#txtProvider').val(dataItem.Providername);
                    self.find('#hfProvider').val(dataItem.ProviderID);
                    if ($bootstrapValidator) {
                        $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Provider');
                    }
                    if (self.find('#txtProvider').val() != "") {
                        self.find('#lnkProviderEdit').css("display", "inline");
                        self.find('#lblProvider').css("display", "none");
                    }
                    self.find('#txtResourceProvider').val(dataItem.Providername);
                    self.find('#hfResourceProvider').val(dataItem.ProviderID);
                    if ($bootstrapValidator) {
                        $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'ResourceProvider');
                    }
                    if (self.find('#txtResourceProvider').val() != "") {
                        self.find('#lnkResourceProviderEdit').css("display", "inline");
                        self.find('#lblResourceProvider').css("display", "none");
                    }

                } else {
                    self.find('#txtProvider').val("");
                    self.find('#hfProvider').val("");
                    self.find('#txtResourceProvider').val("");
                    self.find('#hfResourceProvider').val("");
                    self.find('#lnkProviderEdit').css("display", "none");
                    self.find('#lblProvider').css("display", "inline");
                    self.find('#lnkResourceProviderEdit').css("display", "none");
                    self.find('#lblResourceProvider').css("display", "inline");
                }
            }
            // Edited by Azeem Raza Tayyab on 19-Apr-2016 to fix bug#:PMS-4898
            if ((globalAppdata['DefaultFacilityName'] == "" || globalAppdata['DefaultFacilityName'] == "- Select -") && globalAppdata['DefaultFacilityId'] == "") {
                if (dataItem.Facilityname != "") {
                    self.find('#txtFacility').val(dataItem.Facilityname);
                    self.find('#hfFacility').val(dataItem.FacilityID);
                    if ($bootstrapValidator) {
                        $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Facility');
                    }
                    if (self.find('#txtFacility').val() != "") {
                        self.find('#lnkFacilityEdit').css("display", "inline");
                        self.find('#lblFacility').css("display", "none");
                    }
                } else {
                    self.find('#txtFacility').val("");
                    self.find('#hfFacility').val("");
                    self.find('#lnkFacilityEdit').css("display", "none");
                    self.find('#lblFacility').css("display", "inline");
                }
            }
            EncounterChargeCapture.LoadPatientCase(dataItem.id);
            self.find('#txtPlanPriority').val("");
            EncounterChargeCapture.LoadPatientInsurances(dataItem.id);
            //EncounterChargeCapture.SelectPrimaryInsurance();
            //EncounterChargeCapture.FillReferralNumber();

            utility.InsertRecentPatient(dataItem.id);
        }

        $(Ctrl).kendoAutoComplete({
            dataTextField: 'value',
            filter: 'contains',
            minLength: 4,
            select: onSelect,
            change: onChange,
            dataSource: {
                serverFiltering: true,
                transport: {
                    read: function (e) {
                        utility.GetPatientArray(Ctrl.val(), 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },

    OpenCase: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        var PanelID = EncounterChargeCapture.params["TabID"];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["CaseId"] = "-1";
        params["patientID"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val();
        params["FromAdmin"] = "0";
        params["RefForm"] = 'frmEncounterChargeCapture';
        params["IsOptional"] = false;
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        LoadActionPan('Patient_Case', params);
    },

    OpenCaseDetail: function (HiddenCtrl) {
        var params = [];
        params["CaseId"] = parseInt($('#' + EncounterChargeCapture.params["PanelID"] + ' #' + HiddenCtrl).val());
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["PatientId"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val();
        params["RefCtrl"] = "txtCaseNumber";
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        LoadActionPan('Patient_Case_Detail', params);
    },

    Claim_Print: function (isSubmit, MarkSubmitted, ViewOnly) {

        var visits = EncounterChargeCapture.params["VisitId"];
        var SelectedInsurancePlan = null;
        var insurancePlan = "";
        if (EncounterChargeCapture.SelectedInsurancePlan) {
            SelectedInsurancePlan = EncounterChargeCapture.SelectedInsurancePlan;
        }
        var Charge_Count = $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvVisitCharge tbody tr:not([id*=-])").length;
        if (visits) {
            if (SelectedInsurancePlan && SelectedInsurancePlan.attr('insuranceid') && SelectedInsurancePlan.attr('insuranceid') != "") {

                if (Charge_Count == 1) {
                    var tr_ = $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvVisitCharge tbody tr:not([id*=-])");
                    if (!$(tr_).attr("id")) {
                        Charge_Count = 0;
                    }
                }

                if (Charge_Count > 0) {

                    var ClearingHouseId = $(SelectedInsurancePlan).attr("ediclearinghouseid");
                    if (!ClearingHouseId)
                        ClearingHouseId = "";

                    var params = [];
                    params["mode"] = "Open";
                    params["Visits"] = visits;
                    params["PatientId"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val();
                    params["IsSubmit"] = isSubmit;
                    params["MarkSubmitted"] = MarkSubmitted;
                    params["IsSearch"] = false;
                    params["ClearningHouseId"] = ClearingHouseId;
                    params["ViewOnly"] = ViewOnly;
                    if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.TabID == "Clinical_NotesView" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView" || EncounterChargeCapture.params.TabID == "billTabOutOfOfficeVisits" || EncounterChargeCapture.params.TabID == "Bill_PaymentByBatch")
                        params["ParentCtrl"] = 'EncounterChargeCapture';
                    else
                        params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
                    params["SubmitStatus"] = EncounterChargeCapture.params["SubmitStatus"];
                    params["IsActive"] = EncounterChargeCapture.CheckChargesActiveStatus();
                    LoadActionPan('Bill_ClaimHcfaForm', params);
                }
                else
                    utility.DisplayMessages("Please enter a charge.", 2);

            }
            else
                utility.DisplayMessages("Please select an insurance plan.", 2);
        }
        else {
            utility.DisplayMessages("Please create a visit.", 2);
        }
    },

    //Start ByIA
    OpenClaimBatchSearch: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtBatchNumber";
        params["RefHiddenIdCtrl"] = "hfBatchId";
        //params["ParentCtrl"] = 'EncounterChargeCapture';
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];

        LoadActionPan('Bill_ChargeBatchSearch', params);

    },

    OpenPaymentPosting: function (VisitId, IsFromCopayment) {
        AppPrivileges.GetFormPrivileges("Payment Posting", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                //AST-283 by:MAHMAD
                if (EncounterChargeCapture.params.TabID == "Bill_PaymentByBatch" || EncounterChargeCapture.params.TabID == "Bill_InsurancePaymentByBatch" || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "Clinical_NotesView" || EncounterChargeCapture.params.TabID == "billTabOutOfOfficeVisits" || EncounterChargeCapture.params.TabID == "schTabMultipleView" || EncounterChargeCapture.params.TabID == "mstrTabDashBoard" || EncounterChargeCapture.params.TabID == "EDIReviewReport")
                    params["ParentCtrl"] = 'EncounterChargeCapture';
                else
                    params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
                //AST-283 by:MAHMAD
                params["IsERAAttached"] = EncounterChargeCapture.IsERAAttached;
                params["VisitId"] = EncounterChargeCapture.params.VisitId;
                params["PaymentRef"] = params["ParentCtrl"];
                params["IsFromCollectCopay"] = IsFromCopayment == undefined ? false : true;
                params["patientID"] = EncounterChargeCapture.params.patientID;
                LoadActionPan('Bill_PaymentPosting', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    //End ByIA

    OpenProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        var PanelID = EncounterChargeCapture.params["TabID"];
        if (RefCtrl == 'frmEncounterChargeCapture #txtProvider' || RefCtrl == 'frmEncounterChargeCapture #txtResourceProvider')
            params["IsOptional"] = false;
        else
            params["IsOptional"] = true;
        params["RefForm"] = 'frmEncounterChargeCapture';
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        LoadActionPan('Admin_Provider', params);
    },

    OpenProviderDetail: function (HiddenCtrl, TxtBoxCtrl) {
        var params = [];
        params["ProviderId"] = $('#' + EncounterChargeCapture.params["PanelID"] + ' #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = TxtBoxCtrl;
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        LoadActionPan('providerDetail', params);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenRefProviderDetail: function () {
        var params = [];
        params["ReferringProviderId"] = $('#' + EncounterChargeCapture.params.PanelID + ' #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        LoadActionPan('referringproviderDetail', params);
    },

    OpenFacility: function () {
        var params = [];
        params["IsOptional"] = false;
        params["RefForm"] = 'frmEncounterChargeCapture';
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        LoadActionPan('Admin_Facility', params);
    },

    OpenFacilityDetail: function (HiddenCtrl) {
        var params = [];
        params["FacilityId"] = $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #' + HiddenCtrl).val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtFacility";
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        LoadActionPan('facilityDetail', params);
    },

    OpenElectronicEOB: function () {

        var params = [];
        params["VisitId"] = EncounterChargeCapture.params.VisitId;
        var ChargeCapIds = "";
        self.find("#frmEncounterChargeCapture #dgvVisitCharge tr").each(function () {
            if ($(this).attr("id") && $(this).attr("id") != null) {
                if (ChargeCapIds != "")
                    ChargeCapIds = ChargeCapIds + ",";
                ChargeCapIds = ChargeCapIds + $(this).attr("id");
            }
        });
        params["patientID"] = EncounterChargeCapture.params.patientID;
        params["ChargeCapId"] = ChargeCapIds == "" ? 0 : ChargeCapIds;
        //AST - 525
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.TabID == "Clinical_NotesView" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView" ||EncounterChargeCapture.params.TabID == "Bill_InsurancePaymentByBatch")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        
        LoadActionPan('Bill_ERA_ElectronicEOB', params);

    },

    FillChargePOS: function (facilityHiddenCtrl, POSHiddenCtrl, currRow, mode) {
        var facilityId = $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #' + facilityHiddenCtrl).val();
        if (facilityId > 0) {
            Admin_Facility.SearchFacility(null, facilityId).done(function (response) {
                if (response.status != false) {

                    if (response.FacilityCount > 0) {
                        var FacilityLoadJSONData = JSON.parse(response.FacilityLoad_JSON)[0];

                        $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfFacilityCLIA').val(FacilityLoadJSONData.CLIA);

                        if (POSHiddenCtrl) {

                            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #' + POSHiddenCtrl).val(FacilityLoadJSONData.POSName);
                            if (mode != "edit")
                                EncounterChargeCapture.fillCodeInTable(FacilityLoadJSONData.POSName, "POS", true, currRow);
                        } else {
                            EncounterChargeCapture.fillCodeInTable(FacilityLoadJSONData.POSName, "POS", true, currRow);
                        }
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    fillChargeChildRow: function (CPTCode, insertedRow, childRow) {
        if (CPTCode != null && CPTCode != "") {

            var objDeffered = $.Deferred();

            var j = { "txtShortName": "", "txtCPTCode": String(CPTCode), "txtDiscription": "", "lstTOSId": "", "lstSpeciality": "", "chkDiscontinued": false, "ddlEntity": "", "ddlActive": "" };
            var myJSON = JSON.stringify(j);
            Admin_CPTCode.SearchCPTCode(myJSON, 0, 1, 15).done(function (response) {
                if (response.status != false) {
                    if (response.CPTCount > 0) {
                        var CPTDetails = JSON.parse(response.CPTLoad_JSON)[0];

                        $(insertedRow).find('input[id*="txtUnits"]').val(CPTDetails.BasicUnits);
                        $(insertedRow).find('input[id*="txtTotalFEE"]').val((parseFloat(CPTDetails.BasicUnits) * parseFloat($(insertedRow).find('input[id*="txtFEE"]').val())).toFixed(2));
                        $(insertedRow).find('input[id*="txtExpectedFEE"]').val((parseFloat(CPTDetails.BasicUnits) * parseFloat($(insertedRow).find('input[id*="hfExpectedFEE"]').val())).toFixed(2));

                        var units = parseFloat(CPTDetails.BasicUnits);
                        var fee = parseFloat(CPTDetails.Fee);

                        //EncounterChargeCapture.setNDCData(response, childRow);

                        $(insertedRow).find('input[id*="txtBaseUnits"]').val(parseFloat(CPTDetails.BasicUnits));
                        $(insertedRow).find('input[id*="txtTOS"]').val(CPTDetails.TypeOfServiceCode);
                        if (CPTDetails.CLIA == "True") {
                            if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfFacilityCLIA').val()) {
                                if (EncounterChargeCapture.SelectedInsurancePlan) {
                                    $(childRow).find('input[id*="txtCLIA"]').val($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfFacilityCLIA').val());
                                }
                            }
                            else {
                                $(childRow).find('input[id*="txtCLIA"]').val(self.find("#hfProviderCLIA").val());
                            }
                        }
                        objDeffered.resolve().then(function () {

                            EncounterChargeCapture.ValidateCharges($(insertedRow).find('input[id*="txtUnit"]').attr('id'), $(insertedRow).find('input[id*="txtTotalFEE"]').attr('id'), $(insertedRow).find('input[id*="txtINSCharges"]').attr('id'), $(insertedRow).find('input[id*="txtPATCharges"]').attr('id'), $(insertedRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');

                            return objDeffered;

                        });


                    }
                    else {
                        //// If CPT not found in DB
                        if ($(insertedRow).find('input[id*="txtFEE"]').val() == "" || $(insertedRow).find('input[id*="txtFEE"]').val() == 0) {
                            $(insertedRow).find('input[id*="txtUnits"]').val(1);
                            $(insertedRow).find('input[id*="txtFEE"]').val(0)
                            $(insertedRow).find('input[id*="txtTotalFEE"]').val((parseFloat(1) * parseFloat(0)).toFixed(2));
                        }
                        $(childRow).find('input[id*="txtNDC"]').val("");
                        $(childRow).find('input[id*="txtNDCUnit"]').val("");
                        $(childRow).find('input[id*="txtNDCUnitPrice"]').val("");
                        $(childRow).find('select[id*="ddlNDCMeasurement"]').val("").attr("selected", "selected");
                        $(insertedRow).find('input[id*="txtBaseUnits"]').val(1);

                        objDeffered.resolve().then(function () {
                            EncounterChargeCapture.ValidateCharges($(insertedRow).find('input[id*="txtUnit"]').attr('id'), $(insertedRow).find('input[id*="txtTotalFEE"]').attr('id'), $(insertedRow).find('input[id*="txtINSCharges"]').attr('id'), $(insertedRow).find('input[id*="txtPATCharges"]').attr('id'), $(insertedRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
                            return objDeffered;

                        });
                    }
                }
                else {
                    objDeffered.resolve();
                    return objDeffered;
                }
            });
            // faizan ameen code started
            Admin_CPTCode.SearchHPCSCode(myJSON, 0, 1, 15).done(function (response) {
                if (response.status != false) {
                    if (response.CPTCount > 0) {
                        var CPTDetails = JSON.parse(response.CPTLoad_JSON)[0];

                        $(childRow).find('input[id*="txtdrugCodeCost"]').val(CPTDetails.HPCSCodeCost);



                    }
                    else {

                        $(childRow).find('input[id*="txtdrugCodeCost"]').val("");
                        //// If CPT not found in DB

                    }


                }
                else {

                    $(childRow).find('input[id*="txtdrugCodeCost"]').val("");
                    //// If CPT not found in DB

                }



            });

            // faizan ameen code ended

        }

    },

    setNDCData: function (response, childSelector) {
        if (response.NDCCount == 1) {
            $(childSelector).find('input[id*="txtNDC"]').val(response.NDCData[0].NDCCode);
            $(childSelector).find('input[id*="txtNDCDescription"]').val(response.NDCData[0].NDCDescription);
            $(childSelector).find('input[id*="txtNDCUnit"]').val(response.NDCData[0].Unit);
            $(childSelector).find('input[id*="txtNDCUnitPrice"]').val(response.NDCData[0].UnitPrice);
            $(childSelector).find('select[id*="ddlNDCMeasurement"]').val(response.NDCData[0].NDCMeasurementId).attr("selected", "selected");
        }
    },
    LoadCopayment: function () {
        var params = [];
        if (EncounterChargeCapture.params.AppointmentId != null && EncounterChargeCapture.params.AppointmentId != undefined && EncounterChargeCapture.params.AppointmentId != "") {
            params["AppointmentId"] = EncounterChargeCapture.params.AppointmentId;
        }
        else {
            // if appointement id is not supplied then don't let the copay screen to be opened !!
            return false;

        }

        params["ProviderId"] = $('#' + EncounterChargeCapture.params["PanelID"] + ' #hfProvider').val();
        params["FacilityId"] = $('#' + EncounterChargeCapture.params["PanelID"] + ' #hfFacility').val();
        params["DayDate"] = $('#pnlScheduleCalendar #daydate').text().trim();

        params["PatientId"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val();

        params["PatientVisitId"] = EncounterChargeCapture.params["VisitId"];

        //params["PatientVisitId"] = patientvisitid;
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView") {
            params["ParentCtrl"] = 'EncounterChargeCapture';
        } else {
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        }

        LoadActionPan('schcopayment', params);
    },

    FillVoidClaimInfo: function (self, visit_detail) {
        var voidinfo = "";
        var closedMarkup = "";

        //Original claim
        if (visit_detail.VoidedClaimNumber && visit_detail.NewClaimNumber) {
            closedMarkup = "<div class='col-sm-1 center'><b style='color:red;' title='Claim is Closed' class='text-center'>CLOSED</b></div>";
            voidinfo = "<div class='col-sm-6'><b>Voided to: </b> <a href='#' onclick='EncounterChargeCapture.LoadClaimDetails(" + visit_detail.VoidedVisitId + "," + visit_detail.hfPatientId + ");' title='View Claim Detail' style='color:red'>" + visit_detail.VoidedClaimNumber + "</a><b> Copied to: </b><a href='#' onclick='EncounterChargeCapture.LoadClaimDetails(" + visit_detail.NewVisitId + "," + visit_detail.hfPatientId + ");' title='View Claim Detail' style='color:blue'>" + visit_detail.NewClaimNumber + "</a>";
            self.find('#aVoidReCreate').addClass('disableAll');
            self.find('#aReSubmit').addClass('disableAll');
            self.find('#aReCalculateFee').addClass('disableAll');
            self.find("#SaveVisit").show();
        } // Voided claim
        else if (visit_detail.IsVNC.toLowerCase() == 'false' && visit_detail.OriginalClaimNumber) {
            closedMarkup = "<div class='col-sm-1 center'><b style='color:red;' title='Claim is Closed' class='text-center'>CLOSED</b></div>";
            voidinfo = "<div class='col-sm-6'><b>Voided from: </b> <a href='#' onclick='EncounterChargeCapture.LoadClaimDetails(" + visit_detail.OriginalVisitId + "," + visit_detail.hfPatientId + ");' title='View Claim Detail' style='color:blue'>" + visit_detail.OriginalClaimNumber + "</a>";
            self.find('#aVoidReCreate').addClass('disableAll');
            self.find('#aReSubmit').addClass('disableAll');
            self.find('#aReCalculateFee').addClass('disableAll');
            self.find('#aPrintClaim').addClass('disableAll');
            self.find('#ddlSubmitStatus').prop('disabled', true);
            self.find("#SaveVisit").hide();
            self.find('#aPaymentPosting').addClass('disableAll');
            EncounterChargeCapture.IsVNC = false;

        }// New claim
        else if (visit_detail.IsVNC.toLowerCase() == 'true' && visit_detail.OriginalClaimNumber) {
            voidinfo = "<b>Copied from: </b> <a href='#' onclick='EncounterChargeCapture.LoadClaimDetails(" + visit_detail.OriginalVisitId + "," + visit_detail.hfPatientId + ");' title='View Claim Detail' style='color:blue'>" + visit_detail.OriginalClaimNumber + "</a>";
            self.find("#SaveVisit").show();
        }
        else if (visit_detail.IsLocked == "True") {
            voidinfo = "<div class='center'><b style='color:red;' title='Claim is Closed' class='text-center'>CLOSED</b></div>";
            self.find("#SaveVisit").show();
        }

        if (visit_detail.IsSplitted && visit_detail.IsSplitted.toLowerCase() == 'true' && visit_detail.SplittedToClaimNumber != '' && visit_detail.SplittedToVisitId != '') {
            voidinfo += "<b> Splitted To: </b> <a href='#' onclick='EncounterChargeCapture.LoadClaimDetails(" + visit_detail.SplittedToVisitId + "," + visit_detail.hfPatientId + ");' title='View Claim Detail' style='color:orange'>" + visit_detail.SplittedToClaimNumber + "</a></div>";
            self.find('#aSplitClaim').addClass('disableAll');
            EncounterChargeCapture.IsToHideSplittedClaimBtn = true;

        }
        else if (visit_detail.IsSplitted && visit_detail.IsSplitted.toLowerCase() == 'false' && visit_detail.SplittedFromClaimNumber != '' && visit_detail.SplittedFromVisitId != '') {
            voidinfo += "<b> Splitted From: </b> <a href='#' onclick='EncounterChargeCapture.LoadClaimDetails(" + visit_detail.SplittedFromVisitId + "," + visit_detail.hfPatientId + ");' title='View Claim Detail' style='color:orange'>" + visit_detail.SplittedFromClaimNumber + "</a></div>";
            self.find('#aSplitClaim').addClass('disableAll');
            EncounterChargeCapture.IsSplittedClaim = true;
            EncounterChargeCapture.IsToHideSplittedClaimBtn = true;
            EncounterChargeCapture.LoadPatientInsurances(self.find("#hfPatientId").val());
            //EncounterChargeCapture.SelectPrimaryInsurance();
            //EncounterChargeCapture.FillReferralNumber();

        }
        else if (voidinfo)
            voidinfo += "</div>";


        if (voidinfo) {
            self.find('#ClaimVoidinfo').css('display', 'block').html(voidinfo + closedMarkup);
        }
        else {
            self.find('#ClaimVoidinfo').css('display', 'none');
        }
    },

    LoadVisit: function (PatientId, PatientVisitId) {
                utility.ClearFormValidation('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture');
                EncounterChargeCapture.FillVisit(PatientId, PatientVisitId).done(function (response) {
                    // ALERT for Unallocated Copay -- IMP-815
                    EncounterChargeCapture.PostUnallocatedCopay(response);
                    var self = null;
                    if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail" || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.ParentCtrl == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.ParentCtrl == "billTabUnClaimedAppointment" || EncounterChargeCapture.params.ParentCtrl == "Bill_ChargeSearch" || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.ParentCtrl == "schTabCalendar" || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "billTabPaymentPosting" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
                        self = $("#" + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
                    else
                        self = $("#" + EncounterChargeCapture.params.PanelID);
                    if (response.status != false) {
                        var visit_detail = JSON.parse(response.VisitFill_JSON);
                        var stcList= response.stcList;
                        $("#" + EncounterChargeCapture.params.PanelID + ' #hfNoteId').val(visit_detail.hfNoteId);
                        // Faizan
                        //  $("#" + EncounterChargeCapture.params.PanelID + ' #hfDOB').val(visit_detail.hfDOB);
                       

                         //sameer disabled document button if no document is attch to visit PRD-134
                        if (visit_detail.chkIsDocAttach && visit_detail.chkIsDocAttach == "True") {
                            $('#' + EncounterChargeCapture.params.PanelID + ' #aClaimDocuments #DocAttached').addClass('fa-paperclip');
                        }
                        else {
                            $('#' + EncounterChargeCapture.params.PanelID + ' #aClaimDocuments #DocAttached').removeClass('fa-paperclip');
                        }
                        $('#' + EncounterChargeCapture.params.PanelID + ' #aClaimDocuments').removeClass("disableAll");
                        // disabled ERA buttion if no ERA is Attach to visit PRD-134. 
                        if (visit_detail.chkIsDocAttach && visit_detail.chkIsERAttach == "False") {
                            EncounterChargeCapture.IsERAAttached = false;
                            $('#' + EncounterChargeCapture.params.PanelID + ' #aEOB').addClass('disableAll');
                        }
                        else {
                            EncounterChargeCapture.IsERAAttached = true;
                            $('#' + EncounterChargeCapture.params.PanelID + ' #aEOB').removeClass('disableAll');
                        }
                        EncounterChargeCapture.calculateAge(visit_detail.hfDOB);

                        if (EncounterChargeCapture.params["Parent"] == "clinicalTabNotes" || EncounterChargeCapture.params["Parent"] == "Clinical_NotesView" || EncounterChargeCapture.params["Parent"] == "clinicalTabPhoneEncounter" || visit_detail.hfNoteId == "") {
                            $('#' + EncounterChargeCapture.params.PanelID + ' #aClinicalNotes').addClass('disableAll');
                        } else {
                            $('#' + EncounterChargeCapture.params.PanelID + ' #aClinicalNotes').removeClass('disableAll');
                        }
                        EncounterChargeCapture.params["IsLocked"] = visit_detail.IsLocked == "True" ? true : false;

                        if (EncounterChargeCapture.params.IsLocked == true)
                            self.find('#aVoidReCreate').removeClass('disableAll');
                        else
                            self.find('#aVoidReCreate').addClass('disableAll');

                        self.find('#chkBillToPatient').prop('checked', false);
                        self.find("#txtReferralNumber").val("");
                        self.find("#hfReferralNumerId").val("");
                        if (EMRUtility.CheckPnlOpen("pnlBillPaymentPosting")) {
                            self.find('#aPaymentPosting').addClass('disableAll');
                        } else {
                            self.find('#aPaymentPosting').removeClass('disableAll');
                        }
                        EncounterChargeCapture.FillVoidClaimInfo(self, visit_detail);
                        self.find("#followUpCommentlnk").removeClass("disableAll");
                            EncounterChargeCapture.FollowUpCommentLoad(EncounterChargeCapture.params.VisitId);
                        self.find("#lnkReferralNumberEdit").hide();
                        self.find("#lblReferralNumber").show();
                        utility.bindMyJSON(true, visit_detail, false, self).done(function () {
                            if (response.claimStatus == "0") {
                                EncounterChargeCapture.loadRejection(stcList);
                                //$('#' + EncounterChargeCapture.params.PanelID + ' #aReSubmit').removeClass('disableAll');
                            }
                            EncounterChargeCapture.InsurancePlan = visit_detail.ddlPatientInsurance;
                            if (visit_detail.ddlPatientInsurance != "" && visit_detail.chkBillToPatient == "False") {
                                EncounterChargeCapture.IsCallChange = false;
                                EncounterChargeCapture.BindSelectedInsurancePlan(visit_detail.ddlPatientInsurance);
                                EncounterChargeCapture.PreviouseInsPlan = visit_detail.ddlPatientInsurance;
                            }
                            else {
                                EncounterChargeCapture.IsCallChange = true;
                                EncounterChargeCapture.PreviouseInsPlan = null;
                                EncounterChargeCapture.InsurancePlan = null;
                                $('#' + EncounterChargeCapture.params.PanelID + ' #aPrintClaim').addClass('disableAll');
                            }
                            if (visit_detail.chkIsCleanclaim == "True") {
                                $('#' + EncounterChargeCapture.params.PanelID + ' #chkIsCleanclaim').prop("checked", true);
                            } else {
                                $('#' + EncounterChargeCapture.params.PanelID + ' #chkIsCleanclaim').prop("checked", false);
                            }
                            EncounterChargeCapture.params["SubmitStatus"] = visit_detail.txtSubmitStatus;
                            EncounterChargeCapture.params["ClaimStaus"] = visit_detail.ClaimStatus;
                            if (visit_detail.txtSubmitStatus && (visit_detail.txtSubmitStatus.toLowerCase() != "submitted" && visit_detail.txtSubmitStatus.toLowerCase() != "patient")) {
                                self.find('#aReCalculateFee').removeClass("disableAll");
                            }
                            else {
                                self.find('#aReCalculateFee').addClass("disableAll");
                            }
                            if (self.find("#txtPriorAuthNumber").val() != "") {
                                self.find("#txtPriorAuthNumber").trigger("blur");
                            }
                            if (self.find("#hfReferralNumerId").val() != "") {
                                self.find("#lnkReferralNumberEdit").show();
                                self.find("#lblReferralNumber").hide();
                            }                           
                            EncounterChargeCapture.FillVisitsDetails(self);
                            EncounterChargeCapture.BindICDNValues(self);
                            EncounterChargeCapture.LoadPatientCase($("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val());
                            EncounterChargeCapture.enablePatientControls();
                            //self.find("#dgvPatientInsurances tbody tr[insuranceid='" + visit_detail.ddlPatientInsurance + "']").find("input[type='checkbox']").prop('disabled', true);
                            if (visit_detail.ddlPatientInsurance)
                            { self.find("#lstVisitPlans #" + visit_detail.ddlPatientInsurance).find("input[type='checkbox']").prop('disabled', true); }
                            var CurrentPlanAmtCopay = self.find("#dgvPatientInsurances tbody tr[insuranceid='" + visit_detail.ddlPatientInsurance + "']").attr("AmtCopay");
                            var CurrentPlanSpecialistCopay = self.find("#dgvPatientInsurances tbody tr[insuranceid='" + visit_detail.ddlPatientInsurance + "']").attr("SpecialistCopay");

                            if (parseFloat(CurrentPlanAmtCopay) == parseFloat(visit_detail.txtVisitCopayment)) {
                                self.find('#radPCP').trigger("click");
                            }
                            else if (parseFloat(CurrentPlanSpecialistCopay) == parseFloat(visit_detail.txtVisitCopayment)) {
                                self.find('#radSpecialist').trigger("click");
                            }
                            if (visit_detail.chkAssgBenefits == "True")
                                self.find("#chkAssgBenefits").prop("checked", true);
                            else
                                self.find("#chkAssgBenefits").prop("checked", false);

                            if (self.find('#ddlClaimType').val() != "") {
                                //self.find('#ddlClaimType').trigger("change");
                                self.find('#ddlClaimType').on("change", EncounterChargeCapture.ValidateCaseNumber(self.find('#ddlClaimType')));
                                self.find('#ddlClaimType').on("change", EncounterChargeCapture.EnableAnesthesiaDiv(true));
                            }
                            /************** SETTING RADIO BUTTONS START ****************/
                            var isEmployed = visit_detail.RadEmploymentYes == "True" ? true : false;
                            var isAutoAccident = visit_detail.RadAutoYes == "True" ? true : false;
                            var isOtherAccident = visit_detail.RadOtherYes == "True" ? true : false;
                            var isCopayPaid = visit_detail.chkPaid == "True" ? true : false;

                            if (isEmployed == true) {
                                $("#" + EncounterChargeCapture.params.PanelID + ' #RadEmploymentYes').trigger("click");
                            }
                            else {
                                $("#" + EncounterChargeCapture.params.PanelID + ' #RadEmploymentNo').trigger("click");
                            }

                            if (isAutoAccident == true) {
                                $("#" + EncounterChargeCapture.params.PanelID + ' #RadAutoYes').trigger("click");
                            }
                            else {
                                $("#" + EncounterChargeCapture.params.PanelID + ' #RadAutoNo').trigger("click");
                            }

                            if (isOtherAccident == true) {
                                $("#" + EncounterChargeCapture.params.PanelID + ' #RadOtherYes').trigger("click");
                            }
                            else {
                                $("#" + EncounterChargeCapture.params.PanelID + ' #RadOtherNo').trigger("click");
                            }
                            /************** SETTING RADIO BUTTONS END ****************/

                            $("#" + EncounterChargeCapture.params.PanelID + ' #chkPaid').prop('checked', isCopayPaid)
                            /*********************************************************/
                            //if dos from is empty the current date will be set in dtpDOSFrom, #dtpDOSTo contols
                            if (visit_detail.dtpDOSFrom == "") {
                                $("#" + EncounterChargeCapture.params.PanelID + ' #dtpDOSFrom, #dtpDOSTo').val($.datepicker.formatDate(globalAppdata['DateFormat'], new Date()));
                                EncounterChargeCapture.setInjuryDateRange();
                            }
                            if (visit_detail.ClaimStatus.toLowerCase() == "submitted" || visit_detail.txtSubmitStatus.toLowerCase() == "clearinghouse rejection") {
                                $("#" + EncounterChargeCapture.params.PanelID + " #hfIsSubmitted").val(1);
                                $("#" + EncounterChargeCapture.params.PanelID + " #aReSubmit").removeClass("disableAll");

                            } else {
                                $("#" + EncounterChargeCapture.params.PanelID + " #hfIsSubmitted").val('');
                                $("#" + EncounterChargeCapture.params.PanelID + " #aReSubmit").addClass("disableAll");
                            }

                            if ($(self.find('#txtResourceProvider')).val() != "") {
                                if (self.find('#txtResourceProvider').val() != "") {
                                    self.find('#lnkResourceProviderEdit').css("display", "inline");
                                    self.find('#lblResourceProvider').css("display", "none");
                                }
                            }
                            if ($(self.find('#txtSupervisingProvider')).val() != "") {
                                self.find("#lnkSupervisingProviderEdit").css("display", "inline");
                                self.find("#lblSupervisingProvider").css("display", "none");
                            }
                            if (self.find('#txtCaseNumber').val() != "") {
                                if (self.find("#lnkCaseNumberEdit").css("display") == "none") {
                                    self.find("#lnkCaseNumberEdit").css("display", "inline");
                                    self.find("#lblCaseNumber").css("display", "none");
                                }
                            }
                            EncounterChargeCapture.CheckSelfPay(true);
                            EncounterChargeCapture.EnableDisableICDs();
                            EncounterChargeCapture.FillReferralNumber();
                            if (visit_detail.txtReferralNumber) {
                                self.find("#txtReferralNumber").val(visit_detail.txtReferralNumber);
                            }
                            if (visit_detail.hfReferralNumerId) {
                                self.find("#hfReferralNumerId").val(visit_detail.hfReferralNumerId);
                            }
                            //PMS-1753
                            if (EncounterChargeCapture.params["PanelID"] == "pnlBillChargeSearch" && !EncounterChargeCapture.params["CaseNo"]) {
                                if (visit_detail.txtCaseNumber != "")
                                { EncounterChargeCapture.FillCaseDetail(visit_detail.txtCaseNumber); }
                              }
                        });

                        // fill comments
                       
                        utility.RemoveTimeFromDate("#" + EncounterChargeCapture.params.PanelID + " #dtpAppointmentDate", $("#" + EncounterChargeCapture.params.PanelID + " #dtpAppointmentDate").val());
                        utility.RemoveTimeFromDate("#" + EncounterChargeCapture.params.PanelID + " #dtpSubmittedDate", $("#" + EncounterChargeCapture.params.PanelID + " #dtpSubmittedDate").val());
                        utility.RemoveTimeFromDate("#" + EncounterChargeCapture.params.PanelID + " #dtpClaimDate", $("#" + EncounterChargeCapture.params.PanelID + " #dtpClaimDate").val());
                        utility.RemoveTimeFromDate("#" + EncounterChargeCapture.params.PanelID + " #dtpHoldTill", $("#" + EncounterChargeCapture.params.PanelID + " #dtpHoldTill").val());
                        if (EncounterChargeCapture.params.AppointmentDate != undefined && EncounterChargeCapture.params.AppointmentDate != null && EncounterChargeCapture.params.AppointmentDate != "")
                            $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #dtpAppointmentDate').val(EncounterChargeCapture.getdatetime(new Date(EncounterChargeCapture.params.AppointmentDate), false));
                        var self = null;
                        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture')
                        else
                            self = $('#' + EncounterChargeCapture.params.PanelID)

                        if (self.find("#hfFacility").val() != "") {
                            self.find("#lnkFacilityEdit").css("display", "inline");
                            self.find("#lblFacility").css("display", "none");
                        }

                        if (self.find("#hfProvider").val() != "") {
                            self.find("#lnkProviderEdit").css("display", "inline");
                            self.find("#lblProvider").css("display", "none");
                        }

                        if (self.find("#hfRefProvider").val() != "") {
                            self.find("#lnkRefProviderEdit").css("display", "inline");
                            self.find("#lblRefProvider").css("display", "none");
                        }
                        if (self.find("#hfPatiefrmEncounterChargeCapture #hfPatientIdntId").val() != "") {
                            self.find("#lnkAccountEdit").show();
                            self.find("#lblAccount").hide();
                            self.find("#lnkPatientNameEdit").removeClass("hidden");
                            self.find("#frmEncounterChargeCapture #lblPatientName").addClass("hidden");
                        }
                        if (visit_detail.AppointmentId != null && visit_detail.AppointmentId != "") {
                            self.find("#lnkAppointmentDateEdit").show();
                            self.find("#lblAppointmentDate").hide();
                            self.find("#lnkAppointmentDateEdit").removeClass("hidden");
                            self.find("#frmEncounterChargeCapture #lblAppointmentDate").addClass("hidden");
                        }
                        if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail") {
                            var txtBatchNumber = self.find("#txtBatchNumber");
                            self.find("#hfBatchId").val(EncounterChargeCapture.params.BatchId);
                            txtBatchNumber.val(EncounterChargeCapture.params.BatchNumber);
                            txtBatchNumber.attr("disabled", "disabled");
                        }
                        if (self.find("#hfCRNA").val() != "") {
                            self.find("#lnkCRNAEdit").css("display", "inline");
                            self.find("#lblCRNA").css("display", "none");
                        }

                        if (self.find("#hfAnesthesiologist").val() != "") {
                            self.find("#lnkAnesthesiologistEdit").css("display", "inline");
                            self.find("#lblAnesthesiologist").css("display", "none");
                        }
                        EncounterChargeCapture.FillChargePOS('hfFacility', 'hfPOS', null, "edit");
                        EncounterChargeCapture.params.mode == "Edit";
                        //Load Visit Charges
                        $.when(EncounterChargeCapture.objInsDeffered).then(function () {
                            EncounterChargeCapture.ChargeLoad(response);
                        });

                        EncounterChargeCapture.ClaimPaymentsGridLoad(response);
                        if (EncounterChargeCapture.IsSaved == true) {
                            if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail" && $("#" + ChargeBatch_Viewer.params.PanelID + " #pnlChargeBatchViewer #formDocument").length > 0) {
                                EncounterChargeCapture.FillChargeBatchViewer();
                            }
                        }
                        // case WCNF detail
                        if (EncounterChargeCapture.params["CaseNo"]) {
                            EncounterChargeCapture.FillCaseDetail(EncounterChargeCapture.params["CaseNo"]);
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                    EncounterChargeCapture.setInjuryDateRange();
                    EncounterChargeCapture.validateAdmissionDate();
                    //if (self.find('#chkBillToPatient').is(":checked"))
                    //{  self.find('#chkBillToPatient').attr('disabled', 'disabled');}
                    self.find("#txtNoteCommentsLoad").attr("disabled", "disabled");
                    $('#' + EncounterChargeCapture.params["PanelID"] + " #divRadAnesthesia").find("i").remove();
                    $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());                   
                     // Set  value                   
                    $("#txtClaimComments").html(visit_detail.txtClaimComments);                    
                    $("#hftxtClaimComments").val( $("#txtClaimComments").text());
                   
                    

                    $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());

                });
    },
    loadRejection: function (data) {
        if (data) {
            $.each(data, function (i, item) {
                var $row = $('<tr/>');
                $row.append(
                  '<td>' + item.ClaimNumber + '</a></td>'
                + '<td>' + utility.convertToFigure(item.ChargeAmount, true) + '</td>' //Claim Charge Amount
                + '<td>' + utility.convertToFigure(item.PaidAmount, true) + '</td>' //Claim Payment Amount
                + '<td></td>'
                + '<td>' + item.ClaimCategoryCode + '</td>'
                + '<td>' + item.ClaimStatusCode + '</td>'
                + '<td>' + item.rejectionReason + '</td>'
                );

                $("#" + EncounterChargeCapture.params.PanelID + " #dgvClaimRejection").find("tbody").last().append($row);
            });
        }
    },
    getdatetime: function (dt, bitTimeNeed) {
        var res = "";
        res += EncounterChargeCapture.formatdigits(dt.getMonth() + 1);
        res += "/";
        res += EncounterChargeCapture.formatdigits(dt.getDate());
        res += "/";
        res += EncounterChargeCapture.formatdigits(dt.getFullYear());
        if (bitTimeNeed) {
            res += " ";
            var hours = EncounterChargeCapture.formatdigits(dt.getHours() > 12 ? dt.getHours() - 12 : dt.getHours());
            if (hours == "00")
                hours = "12"
            res += hours
            res += ":";
            res += EncounterChargeCapture.formatdigits(dt.getMinutes());
            res += " " + dt.getHours() > 11 ? " PM" : " AM";
        }
        return res;
    },
    formatdigits: function (val) {
        val = val.toString();
        return val.length == 1 ? "0" + val : val;
    },
    PatientEditAppointment: function () {        
        if ($("#pnlEncounterChargeCapture #AppointmentId").val() != "" && $("#pnlEncounterChargeCapture #AppointmentId").val() != "-1" && $("#pnlEncounterChargeCapture #AppointmentId").val() != "0") {
            AppPrivileges.GetFormPrivileges("Appointment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    appointmentDetail.FillAppointment($("#pnlEncounterChargeCapture #AppointmentId").val()).done(function (response) {
                        if (response.status != false) {
                            var appointment_detail = JSON.parse(response.AppointmentFill_JSON)
                            var params = [];
                            params["AppointmentId"] = $("#pnlEncounterChargeCapture #AppointmentId").val();
                            params["ProviderId"] = appointment_detail.hfProviderId;
                            params["ProviderName"] = appointment_detail.txtProvider;
                            params["ResourceId"] = appointment_detail.hfResourceId;
                            params["ResourceName"] = appointment_detail.txtResource;
                            params["FacilityId"] = appointment_detail.hfFacilityId;
                            params["FacilityName"] = appointment_detail.txtFacility;
                            params["ScheduleReasonId"] = appointment_detail.hfSchReasonId;
                            params["isNoteCreated"] = appointment_detail.isNoteCreated.toLowerCase() == "true" ? true : false;

                            var checkin = (appointment_detail.AppointmentStatus.toLocaleLowerCase() == "check in" || appointment_detail.AppointmentStatus.toLocaleLowerCase() == "check out") ? 1 : 0;
                            checkin = (appointment_detail.AppointmentStatus.toLocaleLowerCase() == "cancel" || appointment_detail.AppointmentStatus.toLocaleLowerCase() == "no show") ? 2 : checkin;
                            params["checkin"] = checkin;

                            params["Time"] = appointment_detail.txtFromTime;
                            params["ToTime"] = appointment_detail.txtToTime;
                            params["SlotMinutes"] = appointment_detail.Duration;
                            //params["IsSpecialist"] = IsSpecialist;
                            params["ScheduleReason"] = appointment_detail.txtSchReason;
                            params["mode"] = "Edit";
                            params["ScheduleDate"] = appointment_detail.hfAppointmentDate;
                            params["ParentCtrl"] = "EncounterChargeCapture";
                            LoadActionPan('appointmentDetail', params);
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    Load_ClaimErrors: function (VisitId) {

        if (VisitId) {
            EncounterChargeCapture.LoadClaimErrors(VisitId).done(function (response) {
                if (response.status != false) {
                    EncounterChargeCapture.LoadClaimErrorsGrid(response);
                }
            });
        }
        else {
            utility.DisplayMessages("Visit not found.", 2);
        }
    },

    LoadClaimErrorsGrid: function (response) {

        //Bind Data in Table
        $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvClaimError tbody").find("tr").remove();
        if (response.ClaimErrorsCount > 0) {

            var ClaimErrorsData = JSON.parse(response.ClaimErrorsJSON);
            $.each(ClaimErrorsData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("onclick", 'utility.SelectGridRow($(this));');
                $row.attr("ErrorId", item.ErrorId);

                $row.append('<td style="display:none;">' + item.ErrorId + '</td><td data-toggle="tooltip" data-placement="right" title=" Job Number: ' + item.JobNumber + '" >' + item.ErrorCode + '</td><td>' + item.Description + '</td><td>' + item.Action + '</td>');

                $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvClaimError tbody").last().append($row);
            });
        }
        else {
            $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvClaimError tbody").last().append("<tr><td colspan='3' style='text-align: center;' >No Claim Error Found.</td></tr>");
        }

        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

    },

    ScrubClaimValidation: function () {

        // PMS-5520 in case of self pay or bill to Patient do not the scrub claim.

        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture')
        else
            self = $('#' + EncounterChargeCapture.params.PanelID)
        var IstoScrub = true;

        if (self.find('#chkBillToPatient').is(':checked') == true)
            IstoScrub = false;

        if (EncounterChargeCapture.IsSelfPay == "True")
            IstoScrub = false;

        if (self.find('#dtpHoldTill').val() != "") {
            if (!(new Date(self.find('#dtpHoldTill').val()) < new Date()))
                IstoScrub = false;
        }


        return IstoScrub
    },

    Scrub_Claim: function (VisitId, Message) {

        if (EncounterChargeCapture.ScrubClaimValidation()) {
            Bill_ClaimSubmission.ScrubClaims(VisitId).done(function (response) {

                if (response.status != false) {

                    if (response.ErrorsCount > 0) {
                        var ErrorMessage = "";
                        if (response.ErrorsCount == 1)
                            ErrorMessage = '<b>' + response.ErrorsCount + '</b> Error.';
                        else
                            ErrorMessage = '<b>' + response.ErrorsCount + '</b> Error(s).';

                        $("#" + EncounterChargeCapture.params["PanelID"] + ' #ClaimError section').addClass('active');
                        $("#" + EncounterChargeCapture.params["PanelID"] + ' #ClaimError div:eq(0)').css({ 'display': "block" });

                        //$('html,body').animate({
                        //    scrollTop: $("#" + EncounterChargeCapture.params["PanelID"] + ' #ClaimError').offset().top
                        //},'slow');

                        utility.DisplayMessages("Claim saved with " + ErrorMessage, 3);
                    }
                    else {
                        utility.DisplayMessages(Message, 1);
                    }

                    //reload Claim Errors.
                    EncounterChargeCapture.Load_ClaimErrors(VisitId);
                }
                else {
                    utility.DisplayMessages("Claim saved.</br>" + response.Message, 3);
                }
            });
        }
        else {
            utility.DisplayMessages(Message, 1);
        }
    },

    FillVisitCharges: function (obj, ChargeId) {
        var selectedVisitId = null;
        if (ChargeId && ChargeId > 0) {
            selectedVisitId = ChargeId;
        }
        else {
            var Option = $(obj).find("option:selected");
            selectedVisitId = $(Option).val();
        }
        if (selectedVisitId != "") {
            EncounterChargeCapture.FillVisit(EncounterChargeCapture.params.patientID, selectedVisitId).done(function (response) {
                if (response.status != false) {
                    //Load Visit Charges
                    EncounterChargeCapture.ChargeLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    ValidateVisit: function () {
        $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture')
           .bootstrapValidator({
               live: 'disabled',
               excluded: [':disabled'],
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   PatientName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   Provider: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   Facility: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   DOSFrom: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DOSTo: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   InjuryDate: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ResourceProvider: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   State: {
                       group: '.pull-left',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ReportTypeCode: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   TransmissionCode: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ControlNumber: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           stringLength: {
                               min: 2,
                               message: 'Attachment control number must have atlest 2 characters.'
                           }
                       }
                   },
                   DocumentSentDate: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CaseNumber: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ClaimType: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DischargeDate: {
                       group: '.col-sm-3',
                       enabled: false,
                       autoFocus: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   AdmissionDate: {
                       group: '.col-sm-3',
                       enabled: false,
                       autoFocus: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   ICD1: {
                       group: '.col-sm-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   Anesthesiologist: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   AnesthesiologistStartTime: {
                       group: '.col-sm-1',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   AnesthesiologistEndTime: {
                       group: '.col-sm-1',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   CRNA: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   CRNAStartTime: {
                       group: '.col-sm-1',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   CRNAEndTime: {
                       group: '.col-sm-1',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   AnesthesiaType: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   AnesthesiaASA: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   AnesServiceType: {
                       group: '.col-sm-3',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   RadAnesthesia: {
                       group: '.pull-right',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
               }
           }).on('keyup', '#txtControlNumber,#dtpDocumentSentDate', function (e) {
               var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
               if (formValidation) {
                   switch ($(this).attr("name")) {
                       case 'ControlNumber':
                           var ControlNumberVal = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #txtControlNumber").val();
                           if (ControlNumberVal != "") {
                               EncounterChargeCapture.EnableAttachmentValidation(true);
                           }
                           else
                               EncounterChargeCapture.EnableAttachmentValidation(false);
                           break;
                       case 'DocumentSentDate':
                           var DocumentSentDateVal = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dtpDocumentSentDate").val();
                           if (DocumentSentDateVal != "") {
                               EncounterChargeCapture.EnableAttachmentValidation(true);
                           }
                           else
                               EncounterChargeCapture.EnableAttachmentValidation(false);
                           break;
                       default:
                           break;
                   }
               }
           }).on('keyup', 'input[id^="txtPOS"]', function (e) {
               //Commented by Azeem Raza Tayyab on 29-Apr-2016 to Fix Bug#: PMS-1657
               /*var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
               if (formValidation) {
                   if ($(this).val() == "21") {
                       formValidation.enableFieldValidators('AdmissionDate', true);
                   }
                   else {
                       var IsValid = true;
                       $('input[id^="txtPOS"]').each(function () {
                           if ($(this).val() == "21") {
                               IsValid = false;
                           }
                       });
                       if (IsValid) { formValidation.enableFieldValidators('AdmissionDate', false); }
                   }
               }*/

           }).on('keyup', 'input[id^="txtCPT"]', function (e) {
               //Commented by Azeem Raza Tayyab on 29-Apr-2016 to Fix Bug#: PMS-1657
               /*var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
               if (formValidation) {
                   if ($(this).val() == "99238" || $(this).val() == "99239") {
                       $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpDischargeDate').prop('disabled', false);
                       formValidation.enableFieldValidators('DischargeDate', true);
                   }
                   else {
                       var IsValid = true;
                       $('input[id^="txtCPT"]').each(function () {
                           if ($(this).val() == "99238" || $(this).val() == "99239") {
                               IsValid = false;
                           }
                       });
                       if (IsValid) {
                           if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpAdmissionDate').val() == "")
                               $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpDischargeDate').prop('disabled', true);
                           formValidation.enableFieldValidators('DischargeDate', false);
                       }
                   }
               }*/

           })
            .on('changeDate', '#dtpDocumentSentDate', function (e) {
                var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
                if (formValidation) {
                    switch ($(this).attr("name")) {
                        case 'DocumentSentDate':
                            var DocumentSentDateVal = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dtpDocumentSentDate").val();
                            if (DocumentSentDateVal != "") {
                                EncounterChargeCapture.EnableAttachmentValidation(true);
                            }
                            else
                                EncounterChargeCapture.EnableAttachmentValidation(false);
                            break;
                        default:
                            break;
                    }
                }

            })
            .on('change', '#ddlReportTypeCode,#ddlTransmissionCode', function (e) {
                var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
                if (formValidation) {
                    switch ($(this).attr("name")) {
                        case 'ReportTypeCode':
                            var ReportTypeCodeVal = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #ddlReportTypeCode").val();
                            if (ReportTypeCodeVal != "") {
                                EncounterChargeCapture.EnableAttachmentValidation(true);
                            }
                            else
                                EncounterChargeCapture.EnableAttachmentValidation(false);
                            break;
                        case 'TransmissionCode':
                            var TransmissionCodeVal = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #ddlTransmissionCode").val();
                            if (TransmissionCodeVal != "") {
                                EncounterChargeCapture.EnableAttachmentValidation(true);
                            }
                            else
                                EncounterChargeCapture.EnableAttachmentValidation(false);
                            break;
                        default:
                            break;

                    }
                }
            }).on('change', '#ddlSubmitStatus', function (e) {
                var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
                if (formValidation) {
                    var submitStatus = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #ddlSubmitStatus option:selected').text();
                    if (submitStatus && submitStatus.toLowerCase() == "pending") {
                        formValidation.enableFieldValidators('ICD1', false);
                        self.find("#spnICD1Required").css("display", "none");
                    }
                    else {
                        formValidation.enableFieldValidators('ICD1', true);
                        self.find("#spnICD1Required").css("display", "inline");
                    }
                    // PRD-50
                    if (EncounterChargeCapture.params["ClaimStaus"] == 'Submitted') {
                        if (submitStatus == 'On Hold' || submitStatus == 'Pending' || submitStatus == 'Ready to Submit Electronic'
                           || submitStatus == 'Ready to Submit Paper' || submitStatus == 'Horizon Review' ||
                            submitStatus == 'Query Pending' || submitStatus == 'Pending Credentialing')
                        {
                            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #ddlSubmitStatus option:contains(Submitted)').prop('selected', true)
                            utility.DisplayMessages('Cannot change Claim status to '+submitStatus+ ' the claim is already submitted', + 4);
                            return;
                        }
                    }
                }
            })
            .on('click', 'input#RadAutoYes,input#RadAutoNo,input#RadOtherYes,input#RadOtherNo', function (e) {
                var formValidation = $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture').data("bootstrapValidator");
                if (formValidation) {
                    var validateInjuryDate = false;
                    formValidation.enableFieldValidators('InjuryDate', EncounterChargeCapture.isRequired('injuryDate'));
                    formValidation.enableFieldValidators('State', EncounterChargeCapture.isRequired('state'));
                }
            })
            .on('success.form.bv', function (e) {
                e.preventDefault();
                EncounterChargeCapture.VisitSave();
            });
    },

    ValidateCaseNumber: function (obj) {

        $bootstrapValidator = null;
        var $bootstrapValidator = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
        if ($bootstrapValidator) {
            if ($(obj).find("option:selected").text().toLowerCase() == "institutional") {
                $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #spcase").css("display", "inline");
                $bootstrapValidator.enableFieldValidators('CaseNumber', true);
            }
            else {
                $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #spcase").css("display", "none");
                $bootstrapValidator.enableFieldValidators('CaseNumber', false);
            }
        }

        //******** Start Anesthesia Checks on change Claim type dropdown
        if ($(obj).find("option:selected").text().toLowerCase() == "professional anesthesia") {
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #divToggleAnesthesiaDetail').show();
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #divRadAnesthesia').show();
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfIsAnesthesiaBilling').val(true);
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #ddlTablist').children("option[value^='divToggleAnesthesiaDetail']").show();
            if ($bootstrapValidator) {
                $bootstrapValidator.enableFieldValidators('AnesthesiaType', true);
                $bootstrapValidator.enableFieldValidators('AnesthesiaASA', true);
                $bootstrapValidator.enableFieldValidators('AnesServiceType', true);
            }
        }
        else {
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #divToggleAnesthesiaDetail').hide();
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #divRadAnesthesia').hide();
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfIsAnesthesiaBilling').val(false);
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #ddlTablist').children("option[value^='divToggleAnesthesiaDetail']").hide();
            if ($bootstrapValidator) {
                $bootstrapValidator.enableFieldValidators('AnesthesiaType', false);
                $bootstrapValidator.enableFieldValidators('AnesthesiaASA', false);
                $bootstrapValidator.enableFieldValidators('AnesServiceType', false);
            }
        }
        //******** End Anesthesia Checks on change Claim type dropdown
    },

    ValidateSubmissionMode: function (self) {
        var IsValid = true;
        var CurrentVisitPlan = EncounterChargeCapture.SelectedInsurancePlan;
        CurrentVisitPlan = EncounterChargeCapture.SelectedInsurancePlan;
        var selectedSubmitStatus = self.find('#ddlSubmitStatus option:selected').text();
        if (selectedSubmitStatus && selectedSubmitStatus.toLowerCase().indexOf('electronic') > -1) {
            var CurrentVisitPlan = EncounterChargeCapture.SelectedInsurancePlan;
            if (CurrentVisitPlan && $(CurrentVisitPlan).attr('insuranceid') && $(CurrentVisitPlan).attr('insuranceid') != "") {
                if ($(CurrentVisitPlan).attr("electronicsubmit") == "False") {
                    utility.DisplayMessages("Please set EDI submit Insurance for this Insurance Plan.", 2);
                    self.find('#ddlSubmitStatus option').filter(function () {
                        return $.trim($(this).text().toLowerCase()) == "pending"
                    }).prop('selected', true);
                    IsValid = false;
                }
            }
            else {
                if (EncounterChargeCapture.IsSelfPay == "False") {
                    utility.DisplayMessages("Please select an insurance plan.", 2);
                    self.find('#ddlSubmitStatus option').filter(function () {
                        return $.trim($(this).text().toLowerCase()) == "pending"
                    }).prop('selected', true);
                    IsValid = false;
                }
            }
        }
        return IsValid;
    },

    ValidateValues: function () {
        if (!utility.ValidateAutoComplete($('#' + EncounterChargeCapture.params["PanelID"] + " #txtProvider"), EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfProvider', false)) {
            return false;
        }
        if (!utility.ValidateAutoComplete($('#' + EncounterChargeCapture.params["PanelID"] + " #txtFacility"), EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfFacility', false)) {
            return false;
        }
        if (!utility.ValidateAutoComplete($('#' + EncounterChargeCapture.params["PanelID"] + " #txtResourceProvider"), EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfResourceProvider', false)) {
            return false;
        }
        else
            return true;
    },

    EnableAttachmentValidation: function (IsEnable) {
        var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
        var IsAttached = false;
        $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #ddlReportTypeCode,#ddlTransmissionCode,#txtControlNumber,#dtpDocumentSentDate').each(function () {
            if ($(this).val() != "") {
                IsAttached = true;
                return false;
            }
        });

        if (IsEnable == true && IsAttached == true) {
            formValidation.enableFieldValidators('ReportTypeCode', true);
            formValidation.enableFieldValidators('TransmissionCode', true);
            formValidation.enableFieldValidators('ControlNumber', true);
            formValidation.enableFieldValidators('DocumentSentDate', true);
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #spnReportTypeCode,#spnTransmissionCode,#spnControlNumber,#spnDocumentSentDate').each(function () {
                $(this).css("display", "");
            });
        }
        else if (IsEnable == false && IsAttached == false) {
            formValidation.enableFieldValidators('ReportTypeCode', false);
            formValidation.enableFieldValidators('TransmissionCode', false);
            formValidation.enableFieldValidators('ControlNumber', false);
            formValidation.enableFieldValidators('DocumentSentDate', false);
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #spnReportTypeCode,#spnTransmissionCode,#spnControlNumber,#spnDocumentSentDate').each(function () {
                $(this).css("display", "none");
            });
        }
    },

    IsVisitPlanActive: function (self) {
        var VisitPlan = $("#" + EncounterChargeCapture.params["PanelID"] + ' #hfInsurancePlan').val();
        var CurrentVisitPlan = EncounterChargeCapture.SelectedInsurancePlan;
        // var selectedSubmitStatus = self.find('#ddlSubmitStatus option:selected').text();
        //var IsValid = true;
        //if (selectedSubmitStatus && selectedSubmitStatus.toLowerCase().indexOf('electronic') > -1 && CurrentVisitPlan && $(CurrentVisitPlan).attr(electronicsubmit) == "True") {
        //    IsValid == true;
        //}
        //if (selectedSubmitStatus && selectedSubmitStatus.toLowerCase().indexOf('paper') > -1 && CurrentVisitPlan && $(CurrentVisitPlan).attr(electronicsubmit) == "False") {
        //    IsValid == true;
        //}
        if (!CurrentVisitPlan || $(CurrentVisitPlan).attr("insuranceid") == "" || VisitPlan == $(CurrentVisitPlan).attr("insuranceid") || $(CurrentVisitPlan).attr("isactive") == "True") {
            return true;
        }
        else {
            return false;
        }
    },

    Claim_VoidReCreate: function () {
        utility.myConfirm("Are you sure you want to void and recreate this claim?", function () {
            EncounterChargeCapture.VoidAndReCreateClaim().done(function (response) {
                if (response.status != false) {
                    EncounterChargeCapture.IsSaved = true;
                    EncounterChargeCapture.FillVisitData(response.visitId, $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val());
                    utility.DisplayMessages(response.Message, 1);
                    EncounterChargeCapture.params['VisitId'] = response.visitId;
                    $('#' + EncounterChargeCapture.params.PanelID + ' #aPrintClaim').removeClass('disableAll');
                    utility.ClearFormValidation('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture');
                    Patient_Demographic.UpdateBalancesInBanner();
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });

        }, function () { }, "Void and Recreate Confirmation");

    },

    Split_Claim: function () {
        utility.myConfirm("Are you sure you want to split this claim?", function () {
            EncounterChargeCapture.SplitClaim().done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    EncounterChargeCapture.FillVisitData(response.VisitId, $("#" + EncounterChargeCapture.params["PanelID"] + " #hfPatientId").val());
                    EncounterChargeCapture.params['VisitId'] = response.VisitId;
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }, function () { }, "Split Claim Confirmation");
    },

    VisitSave: function () {
        //Begin edited by Azeem Raza Tayyab on 29-Apr-2016 to Fix Bug#: PMS-1657
        var IsCPTValid = EncounterChargeCapture.ValidateCPT();
        var IsPOSValid = EncounterChargeCapture.ValidatePOS();
        var self = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture");
        var ICDIsValidated = true;
        var submitStatus = self.find('#ddlSubmitStatus option:selected').text();
        var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
        if (submitStatus && submitStatus.toLowerCase() == "pending") {
            formValidation.enableFieldValidators('ICD1', false);
            self.find("#spnICD1Required").css("display", "none");
        }
        else {
            formValidation.enableFieldValidators('ICD1', true);
            self.find("#spnICD1Required").css("display", "inline");
            if (self.find("#txtICD1").val() == "") {
                ICDIsValidated = false;
            }
        }
        if (IsCPTValid && IsPOSValid && ICDIsValidated) {
            EncounterChargeCapture.ClaimSave();
        }
        else {
            if (!IsCPTValid) {
                utility.myConfirm("Discharge date is required for this CPT, would you like to save without Discharge date?", function () {
                    if (IsPOSValid) {
                        EncounterChargeCapture.ClaimSave();
                    }
                    else {
                        utility.myConfirm("Admission date is required for POS 21, would you like to save without Admission date?", function () {
                        }, function () {
                            var Addsection = self.find('#claimAdditionalInfoDiv section:first');
                            Addsection.addClass('active')
                            Addsection.find('.toggle-content:first').css('display', 'block');
                            self.find("#dtpAdmissionDate").focus();
                        }, "Admission date is required for POS 21");
                    }
                }, function () {
                    var Addsection = self.find('#claimAdditionalInfoDiv section:first');
                    Addsection.addClass('active')
                    Addsection.find('.toggle-content:first').css('display', 'block');
                    self.find("#dtpDischargeDate").focus();
                }, "Discharge date is required for this CPT");

            }
            if (!IsPOSValid) {
                utility.myConfirm("Admission date is required for POS 21, would you like to save without Admission date?", function () {
                    /*Begin Added By Azeem Raza Tayyab on 28-Apr-2016 to fix bug#: PMS-1657*/
                    if (IsCPTValid) {
                        EncounterChargeCapture.ClaimSave();
                    }
                    else {
                        if (!IsCPTValid) {
                            utility.myConfirm("Discharge date is required for this CPT, would you like to save without Discharge date?", function () {
                                EncounterChargeCapture.ClaimSave();
                            }, function () {
                                var Addsection = self.find('#claimAdditionalInfoDiv section:first');
                                Addsection.addClass('active')
                                Addsection.find('.toggle-content:first').css('display', 'block');
                                self.find("#dtpDischargeDate").focus();
                            }, "Discharge date is required for this CPT");

                        }
                    }
                }, function () {
                    var Addsection = self.find('#claimAdditionalInfoDiv section:first');
                    Addsection.addClass('active')
                    Addsection.find('.toggle-content:first').css('display', 'block');
                    self.find("#dtpAdmissionDate").focus();
                }, "Admission date is required for POS 21");
            }

        }
        //End edited by Azeem Raza Tayyab on 29-Apr-2016 to Fix Bug#: PMS-1657


    },
    checkContractualAdjustment: function () {
        let contractualAdjustmentList = [];
        $.each(EncounterChargeCapture.cptArray, function (i,item) {
            if ($("#txtCPT" + item.id).val() != item.code) {
                var obj = {}

                obj.oldCpt = item.code;
                obj.oldConAdj = item.fee - item.expectedFee;
                obj.oldConAdjExists = true;
                if (item.expectedFee == 0) {
                    obj.oldConAdjExists = false;
                }
                obj.newCpt = $("#txtCPT" + item.id).val()
                obj.newConAdj = $("#txtFEE" + item.id).val() - $("#txtExpectedFee" + item.id).val();
                obj.newConAdjExists = true;
                if ($("#txtExpectedFee" + item.id).val() == 0) {
                    obj.newConAdjExists = false;
                }
                contractualAdjustmentList.push(obj)
            }
        })
        return contractualAdjustmentList
    },

    //Created by Azeem Raza Tayyab on 29-Apr-2016 to Fix Bug#: PMS-1657
    ClaimSave: function () {
        var self = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture");
        var BillToPatient = self.find('#chkBillToPatient').is(':checked');
        var IsinsurancePlanChecked = false;
        self.find("#lstVisitPlans").find("input[type='checkbox']").each(function (index) {
            if ($(this).prop("checked") == true) {
                IsinsurancePlanChecked = true;
            }
        });

        if (BillToPatient == false && IsinsurancePlanChecked == false) {
            utility.DisplayMessages("Please select responsible party insurance or Patient.", 3);
            return;
        }

        var strMessage = "";
        for (a = 1; a <= 12; a++) {
            if ($("#hfICD" + a).val().length) {
                $("#hfICD" + a).val($("#hfICD" + a).val().split(" ")[0]);
            }
        }

        EncounterChargeCapture.ChargesRowsSortingOnDOS();

        var arrRowId = [];
        var isChargesValid = true;
        var ChildRowsJSON = "{}";
        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tr").each(function () {
            if ($(this).attr("id") && $(this).attr("id") != null) {
                EncounterChargeCapture.OverrideChargeValidation($(this));

                var IsRowValid = EncounterChargeCapture.EditableGrid.rowValidate($(this));
                //Begin: Author: Abdur Rehman, PMS-1390 Implementation, Date: Jan 18th, 2017
                if (IsRowValid == false && $(this).find('td.expand').find('a').find('i').hasClass('fa-plus-square')) {
                    $(this).find('td.expand').find('a').trigger('click');
                }
                //End: Author: Abdur Rehman, PMS-1390 Implementation, Date: Jan 18th, 2017
                var IsModifiersValid = EncounterChargeCapture.validateModifiers($(this));
                var IsValidICDPointer = EncounterChargeCapture.ValidateICDPinter($(this));
                if (IsValidICDPointer && IsModifiersValid && IsRowValid) {
                    if (EncounterChargeCapture.EditableGrid.datatable.row($(this)).child() != null) {
                        var childRow = EncounterChargeCapture.EditableGrid.datatable.row($(this)).child();
                        var childJSON = childRow.getMyJSON();
                        ChildRowsJSON = utility.MergeJSON(ChildRowsJSON, childJSON);
                    }
                    isChargesValid = true;
                }
                else {

                    isChargesValid = false;
                    return false;
                }
                if (EncounterChargeCapture.validateChargeAmounts($(this))) {
                    isChargesValid = true;
                }
                else {

                    isChargesValid = false;
                    return false;
                }
                // imp-621 Faizan Ameen.
                EncounterChargeCapture.validateDrugCodeCostFee($(this));

                /*****/
                var ChargeStatus = $(this).attr("ChargeStatus");
                if (ChargeStatus && ChargeStatus.toLowerCase() == "submitted") {
                    var ChargeStatusId = String("txtStatus" + $(this).attr("id"));
                    var statusArray = {};

                    statusArray[ChargeStatusId] = "Submitted";

                    var statusJSON = JSON.stringify(statusArray);

                    ChildRowsJSON = utility.MergeJSON(ChildRowsJSON, statusJSON);
                }

                else if (ChargeStatus && ChargeStatus.toLowerCase() == "resubmit") {
                    var ChargeStatusId = String("txtStatus" + $(this).attr("id"));
                    var statusArray = {};

                    statusArray[ChargeStatusId] = "ReSubmit";

                    var statusJSON = JSON.stringify(statusArray);

                    ChildRowsJSON = utility.MergeJSON(ChildRowsJSON, statusJSON);
                }
                /************/

                //to make sure that ids of child rows are not being pushed as we only need parent rows
                if ($(this).attr("id").indexOf('Child') < 0 && $(this).attr("id").indexOf('Anesthesia') < 0)
                    arrRowId.push($(this).attr("id"));
            }
        });
        var strRowIds = arrRowId.join(",");
        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfChargeRowId").val(strRowIds);
  
         // prd-273 set flag for Comments update
        if (EncounterChargeCapture.params.mode == "Edit") {
             if ($("#" +EncounterChargeCapture.params["PanelID"]+ " #frmEncounterChargeCapture #hftxtClaimComments").val()
         != $("#" +EncounterChargeCapture.params["PanelID"]+ " #frmEncounterChargeCapture #txtClaimComments ").text()
        )
         {
           $($("#" +EncounterChargeCapture.params["PanelID"]+ " #frmEncounterChargeCapture #hfIsCommnetsChanged").val("true"))
         }
          else{
                 $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfIsCommnetsChanged").val("false")
           }
        }        
        else{
          $("#" +EncounterChargeCapture.params["PanelID"]+ " #frmEncounterChargeCapture #hfIsCommnetsChanged").val("true")
        }
        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hftxtClaimComments").val($("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #txtClaimComments ").text());
        var self = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture');
        //PRD-594
        if (!$('#' + EncounterChargeCapture.params.PanelID + ' #txtRefProvider').val())
        $('#' + EncounterChargeCapture.params.PanelID + ' #hfRefProvider').val("");
        var myJSON = self.getMyJSON();
                     
        myJSON = utility.MergeJSON(myJSON, ChildRowsJSON);
        var isValidPlan = true;
        var priorityConfirmMsg = "";
        var $option = EncounterChargeCapture.SelectedInsurancePlan;
        var IsActive;
        var insurancePriority = "";
        if ($option) {
            insurancePriority = $($option).attr('planpriority');
            IsActive = $($option).attr('isactive');
        }
        if ($option) {
            isValidPlan = EncounterChargeCapture.IsVisitPlanActive(self);
        }
        var IsValidSubmitionMode = EncounterChargeCapture.ValidateSubmissionMode(self);
        if (insurancePriority != "") {
            var formMode = EncounterChargeCapture.params.mode == "Add" ? "create" : "update";
            if (insurancePriority == "2") {
                priorityConfirmMsg = "Do you want to " + formMode + " claim with Secondary insurance";
            }
            else if (insurancePriority == "3") {
                priorityConfirmMsg = "Do you want to " + formMode + " claim with Tertiary insurance";
            }
        }
        if (self.find('select[id=ddlSubmitStatus]').find('option:selected').text() == "Submitted" && self.find('input[id=dtpSubmittedDate]').val() == "") {
            utility.myConfirm("Claim(s) not Submitted/Resubmitted yet. Are you sure you want to change claim status to \“Submitted\”?", function () {
                if (isValidPlan == true && IsValidSubmitionMode == true && (isChargesValid == true || $('#' + EncounterChargeCapture.params.PanelID + ' #txtNoteComments').val())) {
                    if (priorityConfirmMsg != "") {
                        utility.myConfirm(priorityConfirmMsg, function () {
                            if (EncounterChargeCapture.params.mode == "Add") {
                                EncounterChargeCapture.SaveClaim(strMessage, myJSON);
                            }
                            else if (EncounterChargeCapture.params.mode == "Edit") {
                                EncounterChargeCapture.UpdateClaim(strMessage, myJSON);
                            }
                        }, function () {
                            return false;
                        }, 'Confirmation');
                    }
                    else {
                        if (EncounterChargeCapture.params.mode == "Add") {
                            EncounterChargeCapture.SaveClaim(strMessage, myJSON);
                        }
                        else if (EncounterChargeCapture.params.mode == "Edit") {
                            EncounterChargeCapture.UpdateClaim(strMessage, myJSON);
                        }
                    }
                }
                else if (isValidPlan == false) {
                    utility.DisplayMessages("Insurance Plan is InActive", 2);
                }
            }, function () {
                return false;
            }, 'Confirmation');
        }
        else {
            if (isValidPlan == true && IsValidSubmitionMode == true && (isChargesValid == true || $('#' + EncounterChargeCapture.params.PanelID + ' #txtNoteComments').val())) {
                if (priorityConfirmMsg != "") {
                    utility.myConfirm(priorityConfirmMsg, function () {
                        if (EncounterChargeCapture.params.mode == "Add") {
                            EncounterChargeCapture.SaveClaim(strMessage, myJSON);
                        }
                        else if (EncounterChargeCapture.params.mode == "Edit") {
                            EncounterChargeCapture.UpdateClaim(strMessage, myJSON);
                        }
                    }, function () {
                        return false;
                    }, 'Confirmation');
                }
                else {
                    if (EncounterChargeCapture.params.mode == "Add") {
                        EncounterChargeCapture.SaveClaim(strMessage, myJSON);
                    }
                    else if (EncounterChargeCapture.params.mode == "Edit") {
                        EncounterChargeCapture.UpdateClaim(strMessage, myJSON);
                    }
                }
            }
            else if (isValidPlan == false) {
                utility.DisplayMessages("Insurance Plan is InActive", 2);
            }
        }
    },
    //End Patient Encounter Visit Related Code
    //START Charge Related Code

    validateAmount: function (Fee, InsCharges, PatCharges, txtInsCharges, txtPatCharges, txtCopayCharges, currentCtrl, IsInusranceAttached, IsAssignedBenefits) {
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var BillToPatient = self.find('#chkBillToPatient').is(':checked');
        // var IsInsuranceChecked = self.find("#dgvPatientInsurances tbody").find("input[type='checkbox']").prop("checked");
        InsCharges = parseFloat($("#" + txtInsCharges).val() == "" ? 0 : $("#" + txtInsCharges).val()).toFixed(2);
        PatCharges = parseFloat($("#" + txtPatCharges).val() == "" ? 0 : $("#" + txtPatCharges).val()).toFixed(2);
        var CopayCharges = Number(0).toFixed(globalAppdata.DecimalPlaces);
        if (EncounterChargeCapture.IsSelfPay == "False" && EncounterChargeCapture.SelectedInsurancePlan) {
            CopayCharges = parseFloat($("#" + txtCopayCharges).val() == "" ? 0 : $("#" + txtCopayCharges).val()).toFixed(2);
        }

        if (currentCtrl.toLowerCase().indexOf("insurance") > -1) {
            if (InsCharges <= Fee) {
                if (EncounterChargeCapture.IsSelfPay == "False" && BillToPatient == false) {
                    $("#" + txtPatCharges).val(Number(Fee - InsCharges - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                }
                else {
                    $("#" + txtPatCharges).val(Number(Fee - InsCharges - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                    $("#" + txtInsCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
                }
            }
            else {
                if (EncounterChargeCapture.IsSelfPay == "False" && BillToPatient == false) {
                    $("#" + txtInsCharges).val(Number(Fee - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                    $("#" + txtPatCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
                }
                else {
                    $("#" + txtPatCharges).val(Number(Fee - InsCharges - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                    $("#" + txtInsCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
                }
            }
        }
        else if (currentCtrl.toLowerCase().indexOf("patient") > -1) {
            if (PatCharges <= Fee) {
                if (EncounterChargeCapture.IsSelfPay == "False" && BillToPatient == false) {
                    $("#" + txtInsCharges).val(Number(Fee - (Number(PatCharges) + Number(CopayCharges))).toFixed(globalAppdata.DecimalPlaces));
                }
                else {
                    $("#" + txtPatCharges).val(Number(Fee - InsCharges - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                    $("#" + txtInsCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
                }
            }
            else {
                $("#" + txtPatCharges).val(Number(Fee).toFixed(globalAppdata.DecimalPlaces));
                $("#" + txtInsCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
            }
        }
        else if (currentCtrl.toLowerCase().indexOf("fee") > -1 && IsInusranceAttached == true) {
            if (IsAssignedBenefits == true) {
                if (PatCharges <= Fee) {
                    if (EncounterChargeCapture.IsSelfPay == "False" && BillToPatient == false) {
                        $("#" + txtInsCharges).val(Number(Fee - (Number(PatCharges) + Number(CopayCharges))).toFixed(globalAppdata.DecimalPlaces));
                    }
                    else {
                        $("#" + txtPatCharges).val(Number(Fee - InsCharges - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                        //fix bug-PMS-1973
                        if (InsCharges > 0) {
                            $("#" + txtInsCharges).val(InsCharges);
                        }
                        else {
                            $("#" + txtInsCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
                        }
                    }
                }
                else {
                    if (EncounterChargeCapture.IsSelfPay == "False" && BillToPatient == false) {
                        $("#" + txtInsCharges).val(Number(Fee - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                        $("#" + txtPatCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
                    }
                    else {
                        $("#" + txtPatCharges).val(Number(Fee - InsCharges - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                        $("#" + txtInsCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
                    }
                }
            }
            else if (IsAssignedBenefits == false) {
                if (InsCharges <= Fee) {
                    $("#" + txtPatCharges).val(Number(Fee - InsCharges - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                }
                else if (InsCharges > Fee) {
                    $("#" + txtInsCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
                    $("#" + txtPatCharges).val(Number(Fee).toFixed(globalAppdata.DecimalPlaces));
                }
            }
        }

        //EncounterChargeCapture.UpdateVisitsDetails(self);
    },

    ValidateCharges: function (txtUnit, txtFee, txtInsCharges, txtPatCharges, txtCopayCharges, currentCtrl) {

        if ($("#" + txtFee).val() == "" || isNaN($("#" + txtFee).val()))
            $("#" + txtFee).val(0);

        var Unit = parseFloat(($("#" + txtUnit).val() == "" || $("#" + txtUnit).val() == 0 || isNaN($("#" + txtUnit).val())) ? 1 : $("#" + txtUnit).val()).toFixed(globalAppdata.DecimalPlaces);
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var Fee = parseFloat($("#" + txtFee).val());//* Unit;
        var InsCharges = parseFloat($("#" + txtInsCharges).val() == "" ? 0 : $("#" + txtInsCharges).val()).toFixed(globalAppdata.DecimalPlaces);

        var PatCharges = parseFloat($("#" + txtPatCharges).val() == "" ? 0 : $("#" + txtPatCharges).val()).toFixed(globalAppdata.DecimalPlaces);
        var CopayCharges = parseFloat($("#" + txtCopayCharges).val() == "" ? 0 : $("#" + txtCopayCharges).val()).toFixed(globalAppdata.DecimalPlaces);
        var isInsurancePlanAttached = false;
        // var IsInsuranceChecked = self.find("#dgvPatientInsurances tbody").find("input[type='checkbox']").prop("checked");
        if (EncounterChargeCapture.SelectedInsurancePlan) {
            isInsurancePlanAttached = true;
        }
        var isAssignedBenefits = $("#" + EncounterChargeCapture.params["PanelID"] + " #chkAssgBenefits").is(":checked");
        if (EncounterChargeCapture.IsSelfPay == "False" && EncounterChargeCapture.SelectedInsurancePlan) {
            CopayCharges = parseFloat($("#" + txtCopayCharges).val() == "" ? 0 : $("#" + txtCopayCharges).val()).toFixed(2);
        }
        else if (EncounterChargeCapture.IsSelfPay == "True") {
            $("#" + txtCopayCharges).val(Number(0).toFixed(globalAppdata.DecimalPlaces));
        }

        //fix bug#PMS-1526
        if (Fee != 0) {

            if (isInsurancePlanAttached) {
                EncounterChargeCapture.validateAmount(Fee, InsCharges, PatCharges, txtInsCharges, txtPatCharges, txtCopayCharges, currentCtrl, isInsurancePlanAttached, isAssignedBenefits);
            }
            else {
                if ($('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture").find("#chkBillToPatient").is(':checked') && EncounterChargeCapture.params.mode == "Edit") {
                    var strPatientCharge = Number(Fee) - (Number(InsCharges) + Number(CopayCharges));
                    $("#" + txtPatCharges).val(strPatientCharge.toFixed(globalAppdata.DecimalPlaces));
                    $("#" + txtInsCharges).val(InsCharges);
                }
                else {
                    $("#" + txtPatCharges).val((Number(Fee) - Number(CopayCharges)).toFixed(globalAppdata.DecimalPlaces));
                    $("#" + txtInsCharges).val(0);
                }
            }
            // validate charge amount against specified fee
            if (isInsurancePlanAttached && (PatCharges > 0 || InsCharges > 0)) {
                EncounterChargeCapture.validateAmount(Fee, InsCharges, PatCharges, txtInsCharges, txtPatCharges, txtCopayCharges, currentCtrl, isInsurancePlanAttached, isAssignedBenefits);
            }
        }

        else {
            $("#" + txtInsCharges).val(0);
            $("#" + txtPatCharges).val(0);
        }

        EncounterChargeCapture.RecalculateCPTTotal();
    },

    CalculateUnitFee: function (txtUnit, txtFee, hfFee, txtINSCharges, txtPATCharges, txtCopayCharges, currentCtrl, hfCurrentChargeUnits, txtExpectedFee, hfExpectedFee) {
        var CurrentChargeUnits = $("#" + hfCurrentChargeUnits).val();
        var CurrentUnits = $("#" + txtUnit).val();
        if (CurrentUnits == "" || CurrentUnits <= 0) {
            $("#" + txtUnit).val(1);
        }
        if ($("#" + hfFee).val() == "") {
            $("#" + hfFee).val(0);
        }
        var UnitVal = parseFloat($("#" + txtUnit).val());
        var FeeVal = parseFloat($("#" + hfFee).val());
        var ExpectedFeeVal = parseFloat($("#" + hfExpectedFee).val());
        var TotalFee = Number(UnitVal * FeeVal).toFixed(globalAppdata.DecimalPlaces);
        $("#" + txtFee).val(TotalFee);
        $("#" + txtExpectedFee).val(Number(UnitVal * ExpectedFeeVal).toFixed(globalAppdata.DecimalPlaces));

        EncounterChargeCapture.ValidateCharges(txtUnit, txtFee, txtINSCharges, txtPATCharges, txtCopayCharges, currentCtrl)

    },

    validateHoldDays: function (objHoldDays, currentId, childrow) {

        var self = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture");
        if (childrow)
            self = childrow;

        var HoldDays = $(self).find(objHoldDays).val();
        if (HoldDays.indexOf('_') >= 0) {
            $(self).find(objHoldDays).val("");
        }

    },

    OpenSearchPopup: function (SearchType, Ctrl, HiddenCtrl) {
        var controlToLoad = "";
        if (SearchType == "ICD") {
            controlToLoad = "Admin_IMOICD";
        }
        else if (SearchType == "CPT") {
            controlToLoad = "Admin_IMOCPT";
        }
        else if (SearchType == "Modifier") {
            controlToLoad = "Admin_Modifier";
        }
        var params = [];
        params["FromAdmin"] = "0";
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView" || EncounterChargeCapture.params.TabID == "Clinical_NotesView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
        if (Ctrl != null) {
            params["RefCtrl"] = Ctrl;
        }
        if (HiddenCtrl != null) {
            params["RefHiddenCtrl"] = HiddenCtrl;
        }
        if (controlToLoad != "") {
            LoadActionPan(controlToLoad, params);
        }

    },

    ChargeSave: function (ChargeData, ChargeId, objTable, VisitId, CurrentRow) {
        if (ChargeId && ChargeId > 0) {
            //Update Charge.
            EncounterChargeCapture.UpdateCharge(ChargeData, ChargeId).done(function (response) {
                if (response.status != false) {
                    utility.DisplayMessages(response.Message, 1);
                    /*update Payments Details Grid*/
                    EncounterChargeCapture.ReloadClaimPayments();
                    $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        else {
            // Save Charge.
            EncounterChargeCapture.SaveCharge(ChargeData, VisitId, ChargeId).done(function (response) {
                if (response.status != false) {
                    /*update Payments Details Grid*/
                    EncounterChargeCapture.ReloadClaimPayments();
                    utility.DisplayMessages(response.Message, 1);
                    ChargeId = response.ChargeId;
                    if (CurrentRow) {
                        EncounterChargeCapture.updateChargeRowId(CurrentRow, ChargeId);
                    }
                    $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
        return ChargeId;
    },

    ChargeDelete: function (chargeId, $row, obj) {
        EncounterChargeCapture.DeleteCharge(chargeId).done(function (response) {
            if (response.status == true) {
                if ($row != null && obj != null) {
                    var _self = obj;
                    _self.datatable.row($row.get(0)).remove().draw();
                }
                EncounterChargeCapture.RecalculateCPTTotal();
                utility.DisplayMessages(response.Message, 1);
                Patient_Demographic.UpdateBalancesInBanner();
                //Added by Azeem Raza Tayyab on 11-May-2016 to fix Bug#:PMS-5160
                $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
            }
            else {
                if (response.Message.indexOf("sufficient privileges") > 0)
                    utility.DisplayMessages(response.Message, 3);
                else
                    utility.DisplayMessages("Can not delete, CPT Code " + $row.find('input[id*="txtCPT"]').val() + " has payments", 3);
            }
        });
    },
    ChargeLoad: function (response) {
        var PanelChargeGrid = "#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result";
        var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
        var ChargesCount = response.VisitChargesCount;
        var ttlFee = 0;
        var ttlUnit = 0;
        var ttlBilled = 0;
        var ttlInsBalance = 0;
        var ttlPatBalance = 0;
        var ttlCopay = 0;
        var $FooterRow ="";
        $("#" + EncounterChargeCapture.params["PanelID"] + " #pnlVisitCharge_Result #dgvVisitCharge tfoot").html("");
        if (ChargesCount > 0) {
            //if ($.fn.dataTable.isDataTable(ChargeGridId))
            //    $(ChargeGridId).dataTable().fnDestroy();
            //$(ChargeGridId + " tbody").find("tr").remove();
             $FooterRow = $('<tr class="bold black bg-default" style="text-align: right" />');
            EncounterChargeCapture.EditableGrid.datatable.clear().draw();
            var isSubmitted = false;
            var VisitCharges_detail = JSON.parse(response.VisitsChargesLoad_JSON);
            EncounterChargeCapture.cptArray = [];
            $.each(VisitCharges_detail, function (i, item) {
                var ChargeId = item.hfChargeId;
                var CurrentRow = EncounterChargeCapture.AddNewChargeRow(ChargeId, "Edit");
                var VisitChargesTable = $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge");

                utility.bindMyJSON(true, item, false, VisitChargesTable).done(function () {

                    var CPTDescription = $(CurrentRow).find('input[id*="hfCPTDescription"]').val();

                    //CPT TOOLTIP
                    $(CurrentRow).find('div[id*="divtxtCPT"]').attr('title', CPTDescription)
                    $(CurrentRow).find('div[id*="divtxtCPT"]').attr('data-toggle', "tooltip");
                    $(CurrentRow).find('div[id*="divtxtCPT"]').attr('data-placement', "right");
                    $(CurrentRow).find('div[id*="divtxtCPT"]').attr('data-originalval', CPTDescription)
                });

              

                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

                $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("disabled", "disabled");
                $(CurrentRow).find('input[id*="txtPrimaryFEE"]').attr("disabled", "disabled");

                var multiplier = 1;
                if (item["IsVNC" + ChargeId] == "False") {
                    multiplier = -1;
                }

                $(CurrentRow).find('input[id*="txtTotalFEE"]').val(($(CurrentRow).find('input[id*="txtFEE"]').val() * $(CurrentRow).find('input[id*="txtUnits"]').val() * multiplier).toFixed(globalAppdata.DecimalPlaces))
                $(CurrentRow).find('input[id*="txtExpectedFEE"]').val($(CurrentRow).find('input[id*="hfExpectedFEE"]').val() * $(CurrentRow).find('input[id*="txtUnits"]').val());
                var row = EncounterChargeCapture.EditableGrid.datatable.row(CurrentRow);

                var newChildRow = row.child();
                var curentDropDown = $(newChildRow).find('datalist[id*="ddlPriorAuthorization"]');
                var curentPANCtrl = $(newChildRow).find('input[id*="txtPriorAuthorization"]');
                var hfAuthorizedId = $(newChildRow).find('input[id*="hfAuthorizationId"]');
                if (item["txtPriorAuthorization" + ChargeId]) {
                    $(newChildRow).find("#txtPriorAuthorization" + ChargeId).val(item["txtPriorAuthorization" + ChargeId]);
                }

                //LoadPreAuthorizations function is also called on CPT enter so its seems like a extra call
                //if (curentPANCtrl.val() == "") {
                //    EncounterChargeCapture.LoadPreAuthorizations(CurrentRow, curentDropDown, curentPANCtrl, hfAuthorizedId);
                //}

                ttlFee = parseFloat(ttlFee) + parseFloat($(CurrentRow).find('input[id*="txtFEE"]').val());
                ttlUnit = parseFloat(ttlUnit) + parseFloat($(CurrentRow).find('input[id*="txtUnits"]').val());
                ttlBilled = parseFloat(ttlBilled) + parseFloat($(CurrentRow).find('input[id*="txtTotalFEE"]').val());
                ttlInsBalance = parseFloat(ttlInsBalance) + parseFloat($(CurrentRow).find('input[id*="txtINSCharges"]').val());
                ttlPatBalance = parseFloat(ttlPatBalance) + parseFloat($(CurrentRow).find('input[id*="txtPATCharges"]').val());
                ttlCopay = parseFloat(ttlCopay) + parseFloat($(CurrentRow).find('input[id*="txtCOPAY"]').val());
                var fee = item["txtFEE" + ChargeId];

                $(CurrentRow).find('input:hidden[id*="hfFee"]').val(fee);

                //set NDC Validation
                if (item["txtNDC" + ChargeId]) {
                    $(newChildRow).find("#txtNDC" + ChargeId).val(item["txtNDC" + ChargeId]);
                    EncounterChargeCapture.SetValidation($(newChildRow).find("#txtNDC" + ChargeId), ChargeId, $(newChildRow))
                }

                //its an extra calling and have no functionality in it.
                //chargeSearchDetail.GetLedgerBalanceOfCharge(ChargeId, 0, 0, 0).done(function (response) {
                //    if (response.status != false) {
                //        if (Number(response.Balance) != 0) {
                //            //bug#PMS-1156
                //            //$(CurrentRow).find("a.remove-row").addClass("disableAll");
                //        }
                //    }
                //    else {
                //    }
                //    $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
                //});

                var isEmergency = item["chkEMG" + ChargeId] == "True" ? true : false;

                $(CurrentRow).find('input:checkbox[id*="chkEMG"]').attr("checked", isEmergency);
                $(CurrentRow).attr("ChargeStatus", item["txtStatus" + ChargeId]);

                EncounterChargeCapture.enableUpAndDown($(CurrentRow));
                EncounterChargeCapture.enableRemoveRow($(CurrentRow));
                //row.child().loadDropDowns(true).done(function () {
                //    utility.bindMyJSON(true, item, false, $(newChildRow));
                //    if (item["ddlNDCMeasurement" + ChargeId]) {
                //        setTimeout(function () {
                //            $(newChildRow).find('select[id*="ddlNDCMeasurement"]').val(item["ddlNDCMeasurement" + ChargeId]);// For IE
                //            $(newChildRow).find('select[id*="ddlNDCMeasurement"] option[value=' + item["ddlNDCMeasurement" + ChargeId] + ']').prop("selected", "selected");//For Chrome
                //        }, 1500);
                //    }
                //    $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());

                //});

                if (item["txtStatus" + ChargeId] == 'Submitted') {
                    isSubmitted = true;
                    EncounterChargeCapture.DisableEditableCurrunRow(CurrentRow);
                }
                else {
                    EncounterChargeCapture.EnableEditableRow(CurrentRow);
                }
                EncounterChargeCapture.ValidateCharges($(CurrentRow).find('input[id*="txtUnit"]').attr("id"), $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("id"), $(CurrentRow).find('input[id*="txtINSCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtPATCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtCOPAY"]').attr("id"), 'fee');

                EncounterChargeCapture.MakeChargesArray(item);
            });

            $FooterRow.append('<td colspan="10">Total:</td>' +
                '<td>' + utility.convertToFigure(ttlFee, true) + '</td>' +
                '<td style="text-align:left;">' + ttlUnit + '</td>' +
                '<td>' + utility.convertToFigure(ttlBilled, true) + '</td>' +
                '<td>' + utility.convertToFigure(ttlInsBalance, true) + '</td>' +
                '<td>' + utility.convertToFigure(ttlPatBalance, true) + '</td>' +
                '<td>' + utility.convertToFigure(ttlCopay, true) + '</td>');


            if (isSubmitted == true) {
                $("#" + EncounterChargeCapture.params.PanelID + " #hfIsSubmitted").val(1);
                $("#" + EncounterChargeCapture.params.PanelID + " #aReSubmit").removeClass("disableAll");
                $("#" + EncounterChargeCapture.params.PanelID + " #aReCalculateFee").addClass("disableAll");
                $("#" + EncounterChargeCapture.params.PanelID + " #radPCP").prop("disabled", true);
                $("#" + EncounterChargeCapture.params.PanelID + " #radSpecialist").prop("disabled", true);
                $("#" + EncounterChargeCapture.params["PanelID"] + " #pnlVisitCharge_Result #dgvVisitCharge_wrapper").next().attr("disabled", "disabled");
            }

            $($(ChargeGridId).find("tr")[1]).find("td.actions a.up-row").hide();
            $(ChargeGridId).find("tr:last").prev().prev().find("td.actions a.down-row").hide();
            $(ChargeGridId).find("tr").find(".checked-row").removeClass('hidden');

            EncounterChargeCapture.LoadCopayAlert();
            $("#" + EncounterChargeCapture.params["PanelID"] + " #pnlVisitCharge_Result #dgvVisitCharge tfoot").html($FooterRow);
        }
        else {
            if (EncounterChargeCapture.params.mode && EncounterChargeCapture.params.mode.toLowerCase() == "edit") {
                //fix bug#1117
                EncounterChargeCapture.EditableGrid.datatable.clear().draw();
                EncounterChargeCapture.AddNewChargeRow(null, 'Add');
            }
        }


        EncounterChargeCapture.LockClaim(EncounterChargeCapture.params.IsLocked);
        var tableCount = '';
        if (EncounterChargeCapture.params.ParentCtrl != null) {
            var self = null;
            if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
            else
                self = $('#' + EncounterChargeCapture.params.PanelID);
            tableCount = self.find('div.table-responsive');
        }
        else {
            tableCount = $("#" + EncounterChargeCapture.params.PanelID + " div.table-responsive");
        }
        for (var i = 0 ; i < tableCount.length - 1 ; i++) {
            $(tableCount[i]).removeClass('Of-a');

        }
        $("#" + EncounterChargeCapture.params.PanelID + " #divChargeDetails #ddlClaimType").on("change", EncounterChargeCapture.EnableAnesthesiaDiv(true));//)  change(function (ev) {
        //  EncounterChargeCapture.EnableAnesthesiaDiv();
        //});
        $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
    },
    MakeChargesArray: function (chargeData) {
        if (chargeData) {
            let obj = {}
            obj.id = chargeData.hfChargeId;
            obj.code = chargeData["txtCPT" + chargeData.hfChargeId];
            obj.code = chargeData["txtCPT" + chargeData.hfChargeId];
            obj.expectedFee = chargeData["txtExpectedFee" + chargeData.hfChargeId];
            obj.fee = chargeData["txtFEE" + chargeData.hfChargeId];
            EncounterChargeCapture.cptArray.push(obj);
        }
    },
   RecalculateCPTTotal : function()
   {
         var ttlFee = 0;
        var ttlUnit = 0;
        var ttlBilled = 0;
        var ttlInsBalance = 0;
        var ttlPatBalance = 0;
        var ttlCopay = 0;
        var $FooterRow ="";
        $("#" + EncounterChargeCapture.params["PanelID"] + " #pnlVisitCharge_Result #dgvVisitCharge tfoot").html("");
        $FooterRow = $('<tr class="bold black bg-default" style="text-align: right" />');
       $("#" + EncounterChargeCapture.params["PanelID"] + " #pnlVisitCharge_Result #dgvVisitCharge").find('tr').each(function ()
       {
           if ($(this).find('input[id*="txtFEE"]').val())
            {
                ttlFee = parseFloat(ttlFee) + parseFloat($(this).find('input[id*="txtFEE"]').val());
            }

           if ($(this).find('input[id*="txtUnits"]').val()) {
               ttlUnit = parseFloat(ttlUnit) + parseFloat($(this).find('input[id*="txtUnits"]').val());
           }
           if ($(this).find('input[id*="txtTotalFEE"]').val())
            {
                ttlBilled = parseFloat(ttlBilled) + parseFloat($(this).find('input[id*="txtTotalFEE"]').val());
            }
            if ($(this).find('input[id*="txtINSCharges"]').val())
            {
                ttlInsBalance = parseFloat(ttlInsBalance) +parseFloat($(this).find('input[id*="txtINSCharges"]').val());
            }

            if ($(this).find('input[id*="txtPATCharges"]').val())
            {
                ttlPatBalance = parseFloat(ttlPatBalance) +parseFloat($(this).find('input[id*="txtPATCharges"]').val());
            }

            if ($(this).find('input[id*="txtCOPAY"]').val())
             {
                ttlCopay = parseFloat(ttlCopay) + parseFloat($(this).find('input[id*="txtCOPAY"]').val());
            }

       });
        $FooterRow.append('<td colspan="10">Total:</td>' +
                '<td>' +utility.convertToFigure(ttlFee, true) + '</td>' +
                '<td style="text-align:left;">' +ttlUnit + '</td>' +
                '<td>' +utility.convertToFigure(ttlBilled, true) + '</td>' +
                '<td>' + utility.convertToFigure(ttlInsBalance, true) + '</td>' +
                '<td>' +utility.convertToFigure(ttlPatBalance, true) + '</td>' +
                '<td>' +utility.convertToFigure(ttlCopay, true) + '</td>');

         $("#" + EncounterChargeCapture.params["PanelID"] + " #pnlVisitCharge_Result #dgvVisitCharge tfoot").html($FooterRow);
    },


    AddNewChargeRow: function (RowId, mode, CurrRef) {
        //var myRows= EncounterChargeCapture.EditableGrid;
        var MasterVisitId = $("#" + EncounterChargeCapture.params.PanelID + " #hfMasterVisitId").val();
        // Hide PrimaryFee Column if Visit is Primary
        var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
        if (MasterVisitId == "") {

            $(ChargeGridId + " th#ColumnPrimaryFee").css("display", "none");
        }
        else {

            $(ChargeGridId + " th#ColumnPrimaryFee").css("display", "");
        }
        var CurrentRow = null;
        if (RowId && RowId > 0) {
            if (EncounterChargeCapture.params.ParentCtrl != null) {
                CurrentRow = EncounterChargeCapture.EditableGrid.rowAdd(RowId, "");
            }
            else {
                CurrentRow = EncounterChargeCapture.EditableGrid.rowAdd(RowId, EncounterChargeCapture.params.VisitId);
            }
        }
        else {
            var TemplateRow = $("#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result #dgvVisitCharge tbody tr[id*=-]").last();
            var TemplateRowId = 0;
            if (TemplateRow.length > 0) {
                if (TemplateRow.attr("id").indexOf("Anesthesia") != "-1")
                    TemplateRowId = parseInt(TemplateRow.prev().prev().attr("id"));
                else
                    TemplateRowId = parseInt(TemplateRow.attr("id"));
            }

            if (EncounterChargeCapture.params.ParentCtrl != null) {
                CurrentRow = EncounterChargeCapture.EditableGrid.rowAdd(TemplateRowId - 1, "");
            }
            else {
                CurrentRow = EncounterChargeCapture.EditableGrid.rowAdd(TemplateRowId - 1, EncounterChargeCapture.params.VisitId);
            }
        }
        // Hide PrimaryFee Column if Visit is Primary
        if (MasterVisitId == "") {
            $(CurrentRow).find('input[id*="txtPrimaryFEE"]').parent().css("display", "none");
        }
        else {
            $(CurrentRow).find('input[id*="txtPrimaryFEE"]').parent().css("display", "");
        }

        // Start Copy Previous Row Data to New Row
        if ($(CurrentRow).attr("id") != null && parseInt($(CurrentRow).attr("id")) < 0) {

            var PreviousRow = "";
            //if previous row's child is opened then its a row hence we need to consider the prev row of child which is parent
            if ($(CurrentRow).prev().length > 0 && $(CurrentRow).prev().attr("id").indexOf('Child') > -1) {
                PreviousRow = $("#" + $(CurrentRow).prev().prev().attr("id"));
            } else {
                PreviousRow = $("#" + $(CurrentRow).prev().attr("id"));
            }
        }


        var row = EncounterChargeCapture.EditableGrid.datatable.row(CurrentRow);
        //row.child(EncounterChargeCapture.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        // if ($("#" + EncounterChargeCapture.params.PanelID + " #divChargeDetails #ddlClaimType").find('option:selected').text() == "Professional Anesthesia") {
        row.child([EncounterChargeCapture.buildRowChild(row.data(), CurrentRow.attr("id")), EncounterChargeCapture.buildRowAnesthesia(row.data(), CurrentRow.attr("id"))]).show();
        row.child().first().attr("id", "Child" + CurrentRow.attr("id"));
        row.child().last().attr("id", "Anesthesia" + CurrentRow.attr("id"));
        // }
        //else {
        //row.child(EncounterChargeCapture.buildRowChild(row.data(), CurrentRow.attr("id"))).show();
        //row.child().attr("id", "Child" + CurrentRow.attr("id"));
        //}
        //row.child().attr("id", "Child" + CurrentRow.attr("id"));


        $(CurrentRow).find('input:text[id*="txtTotalFEE"]').attr('disabled', true);
        //$(CurrentRow).find('input:text[id*="txtCPT"]').attr('maxlength', '5');
        $(CurrentRow).find('input:text[id*="txtCPT"]').attr('autocomplete', 'off');
        //row.child().loadDropDowns(true).done(function () {
        //});

        row.child().find('.anesthesiarow #txtStartTime' + CurrentRow.attr("id")).timepicker({
            timeFormat: 'HH:mm',
            showMeridian: false,
            defaultTime: false,
        });

        row.child().find('.anesthesiarow #txtEndTime' + CurrentRow.attr("id")).timepicker({
            timeFormat: 'HH:mm',
            showMeridian: false,
            defaultTime: false,
        });

        if ($('#radAnesthesiologist').is(':checked')) {
            row.child().find('.anesthesiarow #txtStartTime' + CurrentRow.attr("id")).timepicker('setTime', $('#' + EncounterChargeCapture.params.PanelID + ' #divToggleAnesthesiaDetail #txtAnesthesiologistStartTime').val());
            row.child().find('.anesthesiarow #txtEndTime' + CurrentRow.attr("id")).timepicker('setTime', $('#' + EncounterChargeCapture.params.PanelID + ' #divToggleAnesthesiaDetail #txtAnesthesiologistEndTime').val());

        } else {
            row.child().find('.anesthesiarow #txtStartTime' + CurrentRow.attr("id")).timepicker('setTime', $('#' + EncounterChargeCapture.params.PanelID + ' #divToggleAnesthesiaDetail #txtCRNAStartTime').val());
            row.child().find('.anesthesiarow #txtEndTime' + CurrentRow.attr("id")).timepicker('setTime', $('#' + EncounterChargeCapture.params.PanelID + ' #divToggleAnesthesiaDetail #txtCRNAEndTime').val());
        }

        row.child().find('.anesthesiarow #txtRiskUnits' + CurrentRow.attr("id")).val($('#' + EncounterChargeCapture.params.PanelID + ' #txtRiskUnits').val());

        row.child().find('.anesthesiarow #txtRiskUnits' + CurrentRow.attr("id")).trigger("change")

        row.child().find('.anesthesiarow #txtTimeUnits' + CurrentRow.attr("id")).prop("disabled", true);
        row.child().find('.anesthesiarow #txtTotalMinutes' + CurrentRow.attr("id")).prop("disabled", true);
        row.child().find('.anesthesiarow #txtTotalUnits' + CurrentRow.attr("id")).prop("disabled", true);

        //internalize date picker

        //row.child.hide();
        row.child().first().hide();
        $(CurrentRow).find('input[id*="dtpDOSFrom"]').val($('#' + EncounterChargeCapture.params.PanelID + " #dtpDOSFrom").val() != "" ? $('#' + EncounterChargeCapture.params.PanelID + " #dtpDOSFrom").val() : $('#' + EncounterChargeCapture.params.PanelID + " #dtpAppointmentDate").val());
        $(CurrentRow).find('input[id*="dtpDOSTo"]').val($('#' + EncounterChargeCapture.params.PanelID + " #dtpDOSTo").val() != "" ? $('#' + EncounterChargeCapture.params.PanelID + " #dtpDOSTo").val() : $('#' + EncounterChargeCapture.params.PanelID + " #dtpAppointmentDate").val());
        $(CurrentRow).find('input[id*="txtUnits"]').val(1);
        $(CurrentRow).find('input[id*="txtModifier1"]').val($('#' + EncounterChargeCapture.params.PanelID + " #ddlServiceType option:selected").attr("refvalue"));

        if ($(CurrentRow).find('input[id*="txtModifier1"]').val() == "" && $('#' + EncounterChargeCapture.params.PanelID + " #ddlASA option:selected").val() != "")
            $(CurrentRow).find('input[id*="txtModifier1"]').val($('#' + EncounterChargeCapture.params.PanelID + " #ddlASA option:selected").text());
        else if ($('#' + EncounterChargeCapture.params.PanelID + " #ddlASA option:selected").val() != "")
            $(CurrentRow).find('input[id*="txtModifier2"]').val($('#' + EncounterChargeCapture.params.PanelID + " #ddlASA option:selected").text());

        if ($(CurrentRow).prev().length <= 0) {
            if (EncounterChargeCapture.IsSelfPay == "False") {
                $(CurrentRow).find('input[id*="txtCOPAY"]').val($('#' + EncounterChargeCapture.params.PanelID + " #txtVisitCopayment").val());
            }
            else if (EncounterChargeCapture.IsSelfPay == "True") {
                $(CurrentRow).find('input[id*="txtCOPAY"]').val(Number(0).toFixed(globalAppdata.DecimalPlaces));
            }
        }
        $(CurrentRow).find('input[id*="txtFEE"]').val(0);

        $(CurrentRow).find('input[id*="txtFEE"]').on("blur", function () {

            EncounterChargeCapture.ValidateCharges($(CurrentRow).find('input[id*="txtUnit"]').attr("id"), $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("id"), $(CurrentRow).find('input[id*="txtINSCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtPATCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtCOPAY"]').attr("id"), 'fee');
        });
        $(CurrentRow).find('input[id*="txtINSCharges"]').val(0);
        $(CurrentRow).find('input[id*="txtINSCharges"]').on("blur", function () {

            EncounterChargeCapture.ValidateCharges($(CurrentRow).find('input[id*="txtUnit"]').attr("id"), $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("id"), $(CurrentRow).find('input[id*="txtINSCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtPATCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtCOPAY"]').attr("id"), 'insurance');
        });


        $(CurrentRow).find('input[id*="txtPATCharges"]').val(0);
        $(CurrentRow).find('input[id*="txtPATCharges"]').on("blur", function () {

            EncounterChargeCapture.ValidateCharges($(CurrentRow).find('input[id*="txtUnit"]').attr("id"), $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("id"), $(CurrentRow).find('input[id*="txtINSCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtPATCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
        });
        $(CurrentRow).find('input[id*="txtCOPAY"]').on("blur", function () {

            EncounterChargeCapture.ValidateCharges($(CurrentRow).find('input[id*="txtUnit"]').attr("id"), $(CurrentRow).find('input[id*="txtTotalFEE"]').attr("id"), $(CurrentRow).find('input[id*="txtINSCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtPATCharges"]').attr("id"), $(CurrentRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
        });



        if (mode == 'Add')
            EncounterChargeCapture.FillChargePOS('hfFacility', 'hfPOS', CurrentRow);
        $(CurrentRow).find('input[id*="txtPOS"]').val($('#hfPOS').val());


        
        $(CurrentRow).find('input[id*="txtCPT"]').on("blur", function () {
            
            var originalVal = $(this).parent().attr("data-originalval");
            var changedVal = $(this).parent().siblings().find("input[id*='hfCPTDescription']").val();
            if (originalVal != changedVal) {
                var CurrentCPT = $(this);
                var duplicateCPT = false;
                if (CurrentCPT.val() != "") {
                    duplicateCPT = EncounterChargeCapture.CheckDuplicateCPT(CurrentCPT);
                }
                else {
                    EncounterChargeCapture.FillCurrentCPTFee(currentCPTVal, CurrentRow);
                }
                if (duplicateCPT == true) {
                    utility.myConfirm('This code already exists, do you want to add this code again?', function () {
                        var CurrentCPT = $(CurrentRow).find('input[id*="txtCPT"]');
                        var newChildRow = EncounterChargeCapture.EditableGrid.datatable.row(CurrentRow).child();
                        var PANDropDown = $(newChildRow).find('datalist[id*="ddlPriorAuthorization"]');
                        var PANCtrl = $(newChildRow).find('input[id*="txtPriorAuthorization"]');
                        var PANCtrlHf = $(newChildRow).find('input[id*="hfAuthorizationId"]');
                        var currentChargeCPT = $(CurrentRow).find('input[id*="hfCurrentChargeCPT"]').val();
                        var currentCPTVal = $(CurrentCPT).val();
                        var currentChargeId = $(CurrentRow).attr("id");
                        var hfCurrentCPT = $(CurrentRow).find('input[id*="hfCPT' + currentChargeId + '"]');
                        var curntDesc = $(CurrentRow).find('input[id*="hfCPTDescription"]').val();
                        var value = $(CurrentCPT).val() + " - " + curntDesc;
                        //utility.ValidateCPTCode($(CurrentCPT), "CPT", null);
                        utility.ValidateCPTCode($(CurrentCPT), $(hfCurrentCPT), value);
                        $.when(EncounterChargeCapture.fillChargeChildRow(currentCPTVal, CurrentRow, newChildRow)).then(function () {
                            var unitsVal = $(CurrentRow).find('input[id*="txtUnits"]').val();
                            if (currentChargeCPT != currentCPTVal) {
                                $(CurrentRow).find('input[id*="hfCurrentChargeCPT"]').val("");
                                currentChargeCPT = "";
                            }
                            if (currentChargeId > 0 && currentChargeCPT == "") {

                                EncounterChargeCapture.FillCurrentCPTFee(currentCPTVal, CurrentRow);
                            }
                            else if (currentChargeId < 0) {
                                EncounterChargeCapture.FillCurrentCPTFee(currentCPTVal, CurrentRow);
                            }
                            EncounterChargeCapture.LoadPreAuthorizations(CurrentRow, PANDropDown, PANCtrl, PANCtrlHf);
                        });
                    }, function () {
                        $(CurrentCPT).val('');
                        $(CurrentCPT).focus();
                        return false;
                    }, 'Confirmation duplicate CPT');
                }
                else {
                    var newChildRow = EncounterChargeCapture.EditableGrid.datatable.row(CurrentRow).child();
                    var PANDropDown = $(newChildRow).find('datalist[id*="ddlPriorAuthorization"]');
                    var PANCtrl = $(newChildRow).find('input[id*="txtPriorAuthorization"]');
                    var PANCtrlHf = $(newChildRow).find('input[id*="hfAuthorizationId"]');
                    var currentChargeCPT = $(CurrentRow).find('input[id*="hfCurrentChargeCPT"]').val();
                    var currentCPTVal = $(this).val();
                    var currentChargeId = $(CurrentRow).attr("id");
                    var CurrentCPT = $(CurrentRow).find('input[id*="txtCPT"]');
                    var hfCurrentCPT = $(CurrentRow).find('input[id*="hfCPT' + currentChargeId + '"]');
                    var curntDesc = $(CurrentRow).find('input[id*="hfCPTDescription"]').val();
                    var value = $(CurrentCPT).val() + " - " + curntDesc;
                    //utility.ValidateCPTCode($(this), "CPT", null);
                    utility.ValidateCPTCode($(CurrentCPT), $(hfCurrentCPT), value);
                    $.when(EncounterChargeCapture.fillChargeChildRow(currentCPTVal, CurrentRow, newChildRow)).then(function () {
                        var unitsVal = $(CurrentRow).find('input[id*="txtUnits"]').val();
                        if (currentChargeCPT != currentCPTVal) {
                            $(CurrentRow).find('input[id*="hfCurrentChargeCPT"]').val("");
                            currentChargeCPT = "";
                        }
                        if (currentChargeId > 0 && currentChargeCPT == "") {

                            EncounterChargeCapture.FillCurrentCPTFee(currentCPTVal, CurrentRow);
                        }
                        else if (currentChargeId < 0) {
                            EncounterChargeCapture.FillCurrentCPTFee(currentCPTVal, CurrentRow);
                        }
                        EncounterChargeCapture.LoadPreAuthorizations(CurrentRow, PANDropDown, PANCtrl, PANCtrlHf);
                    });
                }
                $(this).parent().attr("data-originalval", changedVal);
            }
        });

        $(CurrentRow).find('input[id*="txtModifier"]').on("blur", function () {
            var strCurrentCPTVal = $(CurrentRow).find('input[id*="txtCPT"]').val();
            EncounterChargeCapture.FillCurrentCPTFee(strCurrentCPTVal, CurrentRow);
        });

        $(CurrentRow).find('input[id*="txtICDPointer"]').on("blur", function () {
            var icdRef = $(this).val();
            var self = null;
            if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
            else
                self = $('#' + EncounterChargeCapture.params.PanelID);
            var RefICD = self.find('#txtICD' + icdRef);
            if (RefICD && RefICD.length > 0 && $(this).val() != "" && RefICD.val() != "") {
                if ($(this).prev('input[id*=txtICDPointer]').length > 0 && $(this).prev('input[id*=txtICDPointer]').val() == "") {
                    isValid = false;
                    $(this).val('');
                    utility.DisplayMessages("Please enter previous ICD pointer.", 3);
                }
                else if ($(this).val() !== "") {
                    var selectedPointer = $(this).val();
                    var selectedPointerId = $(this).attr("id");
                    var isPointerExist = false;
                    $.each($(this).closest("tr").find('input[id*="txtICDPointer"]'), function (key, value) {
                        if (selectedPointer == $(this).val() && selectedPointerId != $(this).attr("id")) {
                            isPointerExist = true;
                            utility.DisplayMessages("ICD pointer already exists.", 3);
                        }
                    });

                    if (isPointerExist == true)
                    { $(this).val(''); }

                }
                    //else if ($(this).prev('input[id*=txtICDPointer]').length > 0 && $(this).prev('input[id*=txtICDPointer]').val() == $(this).val()) {
                    //    $(this).val('');
                    //    utility.DisplayMessages("ICD pointer already exists.", 3);
                    //}
                else {
                    if ($(this).val() != "" && icdRef != $(this).val()) {

                    }
                }
            }
            else {
                if ($(this).val() != "") {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid ICD pointer.", 3);
                }
            }
        });
        // We don't need Up/Down for newly added Row as Id is in Minus
        EncounterChargeCapture.enableRemoveRow($(CurrentRow));

        if ($(CurrentRow).find("input[id*=dtpDOSFrom]").attr('id').indexOf('dtpDOSFrom') > -1 || $(CurrentRow).find("input[id*=dtpDOSTo]").attr("id").indexOf('dtpDOSTo') > -1) {
            var dtDOSFromId = String($(CurrentRow).find("input[id*=dtpDOSFrom]").attr('id'));
            var dtDOSToId = String($(CurrentRow).find("input[id*=dtpDOSTo]").attr("id"));
            EncounterChargeCapture.ValidateFromToDate(EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture', dtDOSFromId, dtDOSToId, true, function () {

                //from date change callback

                var DOSTo = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpDOSTo').datepicker("getDate");
                $('#' + dtDOSToId).datepicker('setEndDate', DOSTo);
                if ($('#' + dtDOSToId).val() == "") {
                    $('#' + dtDOSToId).val($('#' + dtDOSFromId).val())
                }
                EncounterChargeCapture.validateDosFromTo('#' + dtDOSFromId, '#' + dtDOSToId);
            }, function () {

                //to date change callback


            });
            EncounterChargeCapture.validateDosFromTo('#' + dtDOSFromId, '#' + dtDOSToId);
        }
        if (RowId < 1) {
            EncounterChargeCapture.BindSelfPayToBillToPatient();
        }
        if (RowId == null) {
            var ICDPointers = $(CurrentRow).find("input[id*='txtICDPointer1']");
            if (ICDPointers && ICDPointers.length > 0) {
                $(ICDPointers[0]).val('1');
            }
        }
        //$("#" + EncounterChargeCapture.params.PanelID + " #divChargeDetails #ddlClaimType").trigger('change');
        EncounterChargeCapture.EnableAnesthesiaDiv(true);
        $(CurrentRow).find('input[id*="txtTotalFEE"]').addClass("text-right");
        $(CurrentRow).find('input[id*="txtPrimaryFEE"]').addClass("text-right");
        $(CurrentRow).find('input[id*="txtFEE"]').addClass("text-right");
        $(CurrentRow).find('input[id*="txtINSCharges"]').addClass("text-right");
        $(CurrentRow).find('input[id*="txtPATCharges"]').addClass("text-right");
        $(CurrentRow).find('input[id*="txtCOPAY"]').addClass("text-right");
        EncounterChargeCapture.BindNDCAutoComplete(CurrentRow.attr("id"))
        return CurrentRow;
    },
    GetNDC: function (searchNdc, parentId) {
        var AllNDC = [];
        var IsValid = false;
        
        if (searchNdc != null && searchNdc.length > 2)
            IsValid = true;
        else
            IsValid = false;
        var dfd = new $.Deferred();
        if (IsValid) {
            var CPTCode = $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvVisitCharge tr#" + parentId).find('input[id*="txtCPT"]').val();
            if (CPTCode != "") {
                // serach parameter , class name, command name of class
                EncounterChargeCapture.SearchNDCDetail(0, searchNdc, CPTCode).done(function (responseData) {
                    if (responseData.status != false) {
                        if (responseData.NDCCount > 0) {
                            var NDCData = responseData.NDCData;
                            $.each(NDCData, function (i, item) {
                                AllNDC.push({
                                    id: item.CPTNdcId, value: item.NDCCode + (item.NDCDescription != "" ? " - " +                    item.NDCDescription : ""), NDCCode: item.NDCCode,
                                    NDCDescription: item.NDCDescription,
                                    Unit: item.Unit,
                                    UnitPrice: item.UnitPrice,
                                    NDCMeasurementId: item.NDCMeasurementId,
                                    NDCCodeAndDesc:item.NDCCode+(item.NDCDescription!=""?" - "+item.NDCDescription:"")
                                });
                            });
                        }
                    }
                    dfd.resolve(AllNDC);
                });
            }
            else {
                dfd.resolve(AllNDC);
            }
        }
        else {
            if (searchNdc.length == 0) {
                var childRow = $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvVisitCharge #Child" + parentId);
                $(childRow).find('input[id*="txtNDCDescription"]').val("");
                $(childRow).find('input[id*="txtNDCUnit"]').val("");
                $(childRow).find('input[id*="txtNDCUnitPrice"]').val("");
                $.each($(childRow).find('select[id*="ddlNDCMeasurement"]').find('option'), function (i, item) {
                    if ($(item).text() == "- Select -") {
                        $(item).attr("selected", "selected");
                    }
                })
            }
            dfd.resolve(AllNDC);
        }

        return dfd.promise();
    },
    SelectNDC: function (e, parentId) {
        var childRow = $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvVisitCharge #Child" + parentId);
        $(childRow).find('input[id*="txtNDC"]').val(e.NDCCode);
        $(childRow).find('input[id*="txtNDCDescription"]').val(e.NDCDescription);
        $(childRow).find('input[id*="txtNDCUnit"]').val(e.Unit);
        $(childRow).find('input[id*="txtNDCUnitPrice"]').val(e.UnitPrice);
        $(childRow).find('select[id*="ddlNDCMeasurement"]').val(e.NDCMeasurementId).attr("selected", "selected");
    },
    SearchNDCDetail: function (ndcId, NDCCode, CPTCode) {
        var data = "CPTNdcId=" + ndcId + "&NDCCode=" + NDCCode + "&CPTCode=" + CPTCode;
        return MDVisionService.defaultService(data, "ADMIN_CPT_CODE_DETAIL", "LOAD_CPT_CODE_NDC");
    },
    //END Charge Related Code

    //START Service Methods Code
    FillVisit: function (PatientID, VisitId) {
        var data = "VisitID=" + VisitId + "&PatientID=" + PatientID;
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "FILL_VISIT");
    },
    VisitChargesLoad: function (VisitId) {
        var data = "VisitID=" + VisitId;
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "LOAD_VISIT_CHARGE");
    },
    FillVisitPlans: function (VisitId) {
        var data = "VisitID=" + VisitId;
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SEARCH_VISIT_PLANS");
    },

    SaveVisit: function (VisitData, PatientID) {
        var data = "";
        if (EncounterChargeCapture.IsSelfPay == "False") {
            data = "VisitData=" + VisitData + "&PatientID=" + PatientID + "&SelectedPatientInsurance=" + EncounterChargeCapture.GetSelectedInsurancePlan() + "&IsSelfPay=" + EncounterChargeCapture.IsSelfPay;      //PRD-968
        }
        else {
            data = "VisitData=" + VisitData + "&PatientID=" + PatientID + "&IsSelfPay=" + EncounterChargeCapture.IsSelfPay;     //PRD-968
        }
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "SAVE_VISIT");
    },

    LoadClaimErrors: function (VisitId) {

        var data = "VisitId=" + VisitId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "LOAD_CLAIM_ERRORS");
    },


    UpdateVisit: function (VisitData, VisitID, PatientID) {
        var data = "VisitData=" + VisitData + "&VisitID=" + VisitID + "&PatientID=" + PatientID + "&SelectedPatientInsurance=" + EncounterChargeCapture.GetSelectedInsurancePlan();
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "UPDATE_VISIT");
    },

    UpdateActiveInactiveVisits: function (VisitID, IsActive) {
        var data = "VisitID=" + VisitID + "&PatientID=" + EncounterChargeCapture.params.patientID + "&IsActive=" + IsActive;
        return MDVisionService.defaultService(data, "PATIENT_EncounterChargeCapture", "UPDATE_VISIT_ACTIVE_INACTIVE");
    },

    SaveCharge: function (ChargeData, VisitId, ChargeId) {
        var data = "ChargeData=" + ChargeData + "&VisitId=" + VisitId + "&ChargeId=" + ChargeId;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "SAVE_CHARGE_CAPTURE");
    },

    VoidAndReCreateClaim: function () {
        var data = "VisitId=" + EncounterChargeCapture.params.VisitId;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "VOID_AND_RECREATE_CLAIM");
    },

    SplitClaim: function () {
        var data = "VisitId=" + EncounterChargeCapture.params.VisitId;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "SPLIT_CLAIM");
    },

    UpdateCharge: function (ChargeData, ChargeId) {
        var data = "ChargeData=" + ChargeData + "&ChargeId=" + ChargeId;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "UPDATE_CHARGE_CAPTURE");
    },

    DeleteCharge: function (ChargeId) {
        var data = "ChargeId=" + ChargeId;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "DELETE_CHARGE_CAPTURE");
    },

    DeleteVisit: function (VisitID) {
        var data = "VisitID=" + VisitID;
        return MDVisionService.defaultService(data, "PATIENT_EncounterChargeCapture", "DELETE_VISITS");
    },

    //END Service Methods Code

    UnLoad: function () {


        /* On Close make sure ICD is Entered if Claim is Ready for submission by Arsalan Javed */
        /* Check Start */
        if (Bill_ChargeSearch && Bill_ChargeSearch.params && Bill_ChargeSearch.params["PanelID"] == 'pnlBillChargeSearch') {
            $("#" + Bill_ChargeSearch.params["PanelID"] + " #dgvBillCharge").css("pointer-events", "");
        }
        var self = null;
        var IsItok = true;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);


        if (EncounterChargeCapture.params["mode"] != "Add" && EncounterChargeCapture.params["IsLocked"] == false) {
            var CurrentsubmitStatus = self.find('#ddlSubmitStatus option:selected').text().toLowerCase();
            var ClaimSubmitStatus = EncounterChargeCapture.params["SubmitStatus"];
            if (CurrentsubmitStatus == "ready to submit electronic" || CurrentsubmitStatus == "ready to submit paper") {
                var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
                if (formValidation) {

                    if (self.find("#txtICD1").val() == "") {
                        IsItok = false;
                        formValidation.enableFieldValidators('ICD1', true);
                        self.find("#spnICD1Required").css("display", "inline");
                    }
                    else {
                        IsItok = true;
                        formValidation.enableFieldValidators('ICD1', false);
                        self.find("#spnICD1Required").css("display", "none");
                    }

                    $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', "ICD1");

                }
            }
            else {
                if (ClaimSubmitStatus && (ClaimSubmitStatus.toLowerCase() == "ready to submit electronic" || ClaimSubmitStatus == "ready to submit paper") && ClaimSubmitStatus.toLowerCase() != CurrentsubmitStatus.toLowerCase()) {

                    if (CurrentsubmitStatus == "patient" && $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture").find("#chkBillToPatient").is(':checked')) {
                        IsItok = true;
                    }
                    else {
                        IsItok = false;
                        utility.DisplayMessages("Please save changes first.", 2);
                    }
                }
                else
                    IsItok = true;

            }
        }

        /* Check End */

        if (IsItok) {

            UnLoadMethod();
        }


        function UnLoadMethod() {
            if (EncounterChargeCapture.params != null && EncounterChargeCapture.params.ParentCtrl != null) {
                if (EncounterChargeCapture.params.ParentCtrl == "Encounter_CreateClaim")        //PRD-635 TahreeMalik
                    $('#modal-from-dom.modal').removeClass('modal-backdrop');

                if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail") {
                    var parentCtrl = EncounterChargeCapture.params.ParentCtrl;
                    if (EncounterChargeCapture.params["hyperlink"] == true) {
                        UnloadActionPan(parentCtrl, 'EncounterChargeCapture');
                        var TotalLoadedChargeCapture = $('[id="pnlEncounterChargeCapture"]').length;
                        EncounterChargeCapture.RemoveEncounterTabFromStore(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl, TotalLoadedChargeCapture);
                    }
                    else {
                        if ($('#frmEncounterChargeCapture').serialize() != $('#frmEncounterChargeCapture').data('serialize')) {
                            utility.myConfirm('2', function () {
                                var TotalLoadedChargeCapture = $('[id="pnlEncounterChargeCapture"]').length;
                                UnloadActionPan(parentCtrl, 'EncounterChargeCapture', true);
                                EncounterChargeCapture.RemoveEncounterTabFromStore(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl, TotalLoadedChargeCapture);
                                //EncounterChargeCapture.params = null;
                                //UnloadActionPan(null, 'EncounterChargeCapture', true);
                                ChargeBatch_Viewer.ActiveBtns(true);
                            }, function () {
                            },
                                    '2'
                                );
                        }
                        else {
                            var TotalLoadedChargeCapture = $('[id="pnlEncounterChargeCapture"]').length;
                            UnloadActionPan(parentCtrl, 'EncounterChargeCapture', true);
                            EncounterChargeCapture.RemoveEncounterTabFromStore(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl, TotalLoadedChargeCapture);
                            //EncounterChargeCapture.params = null;
                            //UnloadActionPan(null, 'EncounterChargeCapture', true);
                            ChargeBatch_Viewer.ActiveBtns(true);
                        }
                    }
                }
                else if (EncounterChargeCapture.params.ParentCtrl == "Bill_PaymentPosting") {
                    // Existance of Encounter Charge Capture multiple times
                    var TotalLoadedChargeCapture = $('[id="pnlEncounterChargeCapture"]').length;
                    var occurance = $('body #' + EncounterChargeCapture.params.PanelID).length;
                    if (occurance > 1) {
                        var selectedTab = GetCurrentSelectedTab();
                        var parentPanelID = selectedTab.PanelID;
                        UnloadActionPan(EncounterChargeCapture.params.ParentCtrl, 'EncounterChargeCapture', null, parentPanelID);
                        EncounterChargeCapture.RemoveEncounterTabFromStore(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl, TotalLoadedChargeCapture);
                    }
                    else {
                        UnloadActionPan(EncounterChargeCapture.params.ParentCtrl, 'EncounterChargeCapture');
                        EncounterChargeCapture.RemoveEncounterTabFromStore(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl, TotalLoadedChargeCapture);

                    }
                }
                else if (EncounterChargeCapture.params.ParentCtrl == "Bill_ChargeSearch" || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.ParentCtrl == "billTabUnClaimedAppointment" || EncounterChargeCapture.params.ParentCtrl == "schTabCalendar" || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "schTabMultipleView" || EncounterChargeCapture.params.ParentCtrl == "EDIReviewReport") {

                    var deffered = $.Deferred();

                    var parentCtrl = EncounterChargeCapture.params.ParentCtrl;

                    if ($('#frmEncounterChargeCapture').serialize() != $('#frmEncounterChargeCapture').data('serialize')) {
                        utility.myConfirm('2', function () {

                            var res = {
                                Status: true,
                                ParentCtrl: EncounterChargeCapture.params.ParentCtrl,
                                ddlSubmitStatusonUnload: self.find('#ddlSubmitStatus option:selected').text().toLowerCase(),
                                ddlCurrentSubmitStatus: EncounterChargeCapture.ddlCurrentSubmitStatus
                            };

                            deffered.resolve(res);

                            var TotalLoadedChargeCapture = $('[id="pnlEncounterChargeCapture"]').length;
                            UnloadActionPan(parentCtrl, 'EncounterChargeCapture');
                            EncounterChargeCapture.RemoveEncounterTabFromStore(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl, TotalLoadedChargeCapture);
                            //EncounterChargeCapture.params = null;
                            //UnloadActionPan(null, 'EncounterChargeCapture');
                        }, function () {
                        },
                                '2'
                            );
                    }
                    else {
                        var res = {
                            Status: true,
                            ParentCtrl: EncounterChargeCapture.params.ParentCtrl,
                            ddlSubmitStatusonUnload: self.find('#ddlSubmitStatus option:selected').text().toLowerCase(),
                            ddlCurrentSubmitStatus: EncounterChargeCapture.ddlCurrentSubmitStatus
                        };
                        deffered.resolve(res);

                        var TotalLoadedChargeCapture = $('[id="pnlEncounterChargeCapture"]').length;
                        UnloadActionPan(parentCtrl, 'EncounterChargeCapture');
                        if (EncounterChargeCapture.params["ParentCtrl"] == "Patient_Case_Detail") {
                            Patient_Case_Detail.LoadClaims(EncounterChargeCapture.params["CaseId"]);
                        }
                        EncounterChargeCapture.RemoveEncounterTabFromStore(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl, TotalLoadedChargeCapture);
                        //EncounterChargeCapture.params = null;
                        //UnloadActionPan(null, 'EncounterChargeCapture');


                    }

                    deffered.done(function (res) {

                        if (res.Status == true) {
                            // faizan ameen  IMP-614
                            if (res.ParentCtrl == "Bill_ChargeSearch") {
                                if (res.ddlSubmitStatusonUnload != res.ddlCurrentSubmitStatus) {
                                    // Bill_ChargeSearch.bIsFirstLoad = true;
                                    if (EncounterChargeCapture.ValidateChargeSearchScreen() == false) {
                                        $("#pnlBillChargeSearch #frmChargeSearch #dpDOSfrm").datepicker('setDate', new Date());
                                        $("#pnlBillChargeSearch #frmChargeSearch #dpDOSto").datepicker('setDate', new Date());
                                    }
                                    Bill_ChargeSearch.BillChargeSearch(0, $("#pnlBillCharge_Result li.active").text(), 15);
                                }
                            }
                        }

                    });
                }
                else {
                    var TotalLoadedChargeCapture = $('[id="pnlEncounterChargeCapture"]').length;
                    // if (EncounterChargeCapture.params.TabID == "BillingInformation") {
                    //} else {
                    if (typeof DefaultMenuSelected != "undefined" && DefaultMenuSelected == "MDVisionBilling") {
                        if (EncounterChargeCapture.params.TabID != EncounterChargeCapture.params.ParentCtrl) {
                            UnloadActionPan(EncounterChargeCapture.params.TabID, 'EncounterChargeCapture');
                            UnloadActionPan(EncounterChargeCapture.params.ParentCtrl, 'EncounterChargeCapture');
                        } else {
                            UnloadActionPan(EncounterChargeCapture.params.TabID, 'EncounterChargeCapture');
                                //PMS-4791
                            if (EncounterChargeCapture.params['ParentCtrl'] == "ERADetail") {

                                if ($('#ERADetail #dgvLinkedPaidChargesDetail_wrapper .datatables-footer').hasClass('hidden')) {
                                    $('#ERADetail #dgvLinkedPaidChargesDetail_wrapper .datatables-footer').removeClass('hidden');
                                }
                                if ($('#ERADetail #dgvLinkedUnpaidChargessDetail_wrapper .datatables-footer').hasClass('hidden')) {
                                    $('#ERADetail #dgvLinkedUnpaidChargessDetail_wrapper .datatables-footer').removeClass('hidden');
                                    
                                }                             
                                if ($('#ERADetail #dgvLinkedRecoupmentChargesDetail_wrapper .datatables-footer').hasClass('hidden')) {
                                    $('#ERADetail #dgvLinkedRecoupmentChargesDetail_wrapper .datatables-footer').removeClass('hidden');

                                }
                                if ($('#ERADetail #dgvNotLinkedChargesDetail_wrapper .datatables-footer').hasClass('hidden')) {
                                    $('#ERADetail #dgvNotLinkedChargesDetail_wrapper .datatables-footer').removeClass('hidden');

                                }
                                
                            }
                        }
                    } else {
                        UnloadActionPan(EncounterChargeCapture.params.TabID, 'EncounterChargeCapture');
                        UnloadActionPan(EncounterChargeCapture.params.ParentCtrl, 'EncounterChargeCapture');
                    }
                    //  }
                    EncounterChargeCapture.RemoveEncounterTabFromStore(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl, TotalLoadedChargeCapture);
                    //EncounterChargeCapture.params = null;
                }
            }
            else {
                // adnan maqbool,PMS-3997 , 17-02-2016
                var TotalLoadedChargeCapture = $('[id="pnlEncounterChargeCapture"]').length;
                UnloadActionPan(null, 'EncounterChargeCapture');
                if (EncounterChargeCapture.params != null && EncounterChargeCapture.params.ParentCtrl != null)
                    EncounterChargeCapture.RemoveEncounterTabFromStore(EncounterChargeCapture.params.TabID, EncounterChargeCapture.params.ParentCtrl, TotalLoadedChargeCapture);
                //end
                //EncounterChargeCapture.params = null;
            }
        }

    },

    SetValidation: function (obj, Id, childrow) {
        var self = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture");
        if (childrow)
            self = childrow;

        if ($(obj).val() != "") {
            $(self).find("#txtNDCUnit" + Id).attr("isoptional", "0");
            $(self).find("#txtNDCUnitPrice" + Id).attr("isoptional", "0");
            $(self).find("#ddlNDCMeasurement" + Id).attr("isoptional", "0");
        }
        else {
            $(self).find("#txtNDCUnit" + Id).removeAttr("isoptional")
            $(self).find("#txtNDCUnitPrice" + Id).removeAttr("isoptional")
            $(self).find("#ddlNDCMeasurement" + Id).removeAttr("isoptional")
        }
    },

    //START Charge Editable Grid Code
    buildRowChild: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var ChildHTML = $("<div class='childrow'></div>");
        var PriorAuthorization = "<div class='col-xs-1'><label class='control-label'>PAN</label> <input class='form-control' type='text' list='ddlPriorAuthorization" + ParentRowId + "'  maxlength='35' name='PriorAuthorization" + ParentRowId + "' id='txtPriorAuthorization" + ParentRowId + "'/><input type='hidden' id='hfAuthorizationId" + ParentRowId + "'/> <datalist id='ddlPriorAuthorization" + ParentRowId + "' name='PriorAuthorization" + ParentRowId + "><option  value='- SELECT -'></option></datalist></div>";
        var expectedFee = "<div class='col-xs-1'><label class='control-label'>Expected Fee</label><input  class='form-control'type='text' id='txtExpectedFee" + ParentRowId + "' name='txtExpectedFee" + ParentRowId + "' disabled/><input type='hidden' id='hfExpectedFee" + ParentRowId + "'/></div>";
        var txtNDC = '<div class="col-xs-1"><label class="control-label">NDC</label><input class="form-control" id="txtNDC' + ParentRowId + '" customname="NDC Code"  name="txtNDC' + ParentRowId + '" type="text" oninput="EncounterChargeCapture.OnNDCInput();" maxlength="11"/></div>';
        var NDCDescription = "<div class='col-xs-2'><label class='control-label'>NDC Description</label><input class='form-control' id='txtNDCDescription" + ParentRowId + "' name='NDCDescription" + ParentRowId + "' type='text' maxlength='55'/></div>";
        var NDCUnit = "<div class='col-xs-1 size-max90'><label class='control-label'>NDC Unit</label><input class='form-control' id='txtNDCUnit" + ParentRowId + "' name='NDCUnit" + ParentRowId + "' type='text' onkeypress='utility.ValidateDecimal(event, 2);' /></div>";
        var NDCUnitPrice = "<div class='col-xs-1  size-min90'><label class='control-label'>NDC Unit Price</label><input class='form-control' id='txtNDCUnitPrice" + ParentRowId + "' name='txtNDCUnitPrice" + ParentRowId + "' type='text' onkeypress='utility.ValidateDecimal(event, 2);' /></div>";
        var ddlNDCMeasurement = "<div class='col-xs-1'><label class='control-label'>NDC Meas</label><select id='ddlNDCMeasurement" + ParentRowId + "' name='ddlNDCMeasurement" + ParentRowId + "' class='form-control' ><option value=''refvalue='' refname=''>- Select -</option><option value='1' refvalue='' refname=''>F2 (International Unit)</option><option value='2' refvalue='' refname=''>GR (Gram)</option><option value='3' refvalue='' refname=''>ME (Milligram)</option><option value='4' refvalue='' refname=''>ML (Milliliter)</option><option value='5' refvalue='' refname=''>UN (Unit)</option></select></div>";
        var LineNotes = "<div class='col-xs-2'><label class='control-label'>Line Notes</label><textarea class='form-control'spellcheck='true' rows='1' id='txtComments" + ParentRowId + "' maxlength='100' name='txtComments" + ParentRowId + "'></textarea></div>";
        var CLIA = "<div id='divCLIA" + ParentRowId + "' ><div class='col-xs-1 size-max120'><label class='control-label'>CLIA</label><input type='text' class='form-control size100'  id='txtCLIA" + ParentRowId + "' maxlength='10' name='CLIA" + ParentRowId + "''/></div>";
        var HPCSCost = "<div id='divHPCS" + ParentRowId + "' ><div class='col-xs-2 size-max120'><label class='control-label'>Drug Code Cost</label><div class='input-group'><span class='input-group-addon xxs-font pl-tiny pr-tiny'><i class='fa fa-dollar'></i></span><input type='text' class='form-control size100'   id='txtdrugCodeCost" + ParentRowId + "' maxlength='10' name='drugCodeCost" + ParentRowId + "' /></div></div></div>";
        var ChkCPTDesc = "<div class='col-xs-3'><div class='checkbox checkbox-custom'><input type='checkbox' id='chkReportCPTDesc" + ParentRowId + "' checked><label class='control-label pl-none' for='chkReportCPTDesc" + ParentRowId + "'>Report CPT Description</label></div></div>";
        var spacer = '<div class="spacer5"></div>';
        ChildHTML.append(PriorAuthorization, expectedFee, txtNDC,NDCDescription, NDCUnit, NDCUnitPrice, ddlNDCMeasurement, LineNotes, CLIA, HPCSCost, ChkCPTDesc, spacer);
        return ChildHTML;

    },

    buildRowAnesthesia: function (obj, ParentRowId) {
        if (!ParentRowId) {
            ParentRowId = "";
        }
        var AnesthesiaHTML = $("<div class='anesthesiarow'></div>");
        var StartTime = "<div class='col-xs-1'><label class='control-label'>Start Time</label> <input type='text' data-plugin-timepicker data-plugin-options='{ 'minuteStep': 1 }' class='form-control' name='StartTime" + ParentRowId + "' id='txtStartTime" + ParentRowId + "'  onchange=\" EncounterChargeCapture.AnesthesiaUnits('txtStartTime" + ParentRowId + "', 'txtEndTime" + ParentRowId + "', " + ParentRowId + "); EncounterChargeCapture.TotalMinutesAndUnits(" + ParentRowId + ");\" /><input type='hidden' id='hfStartTime" + ParentRowId + "'/> </div>";
        var EndTime = "<div class='col-xs-1'><label class='control-label'>End Time</label><input data-plugin-timepicker data-plugin-options='{ 'minuteStep': 1 }' class='form-control' type='text' onchange=\" EncounterChargeCapture.AnesthesiaUnits('txtStartTime" + ParentRowId + "', 'txtEndTime" + ParentRowId + "', " + ParentRowId + "); EncounterChargeCapture.TotalMinutesAndUnits(" + ParentRowId + ");\" id='txtEndTime" + ParentRowId + "' name='txtEndTime" + ParentRowId + "'/><input type='hidden' id='hfEndTime" + ParentRowId + "'/></div>";
        var TimeUnits = "<div class='col-xs-1 size-max90'><label class='control-label'>Time Units</label><input class='form-control' id='txtTimeUnits" + ParentRowId + "' name='txtTimeUnits" + ParentRowId + "' type='text' onchange=\"EncounterChargeCapture.TotalMinutesAndUnits(" + ParentRowId + ");\" disabled /></div>";
        var BaseUnits = "<div class='col-xs-1 size-max90'><label class='control-label'>Base Units</label><input class='form-control' id='txtBaseUnits" + ParentRowId + "' name='BaseUnits" + ParentRowId + "' type='number' min='0' onchange=\"EncounterChargeCapture.TotalMinutesAndUnits(" + ParentRowId + ");\" maxlength='15' '/></div>";
        var RiskUnits = "<div class='col-xs-1  size-min100'><label class='control-label'>Risk Units</label><input class='form-control' id='txtRiskUnits" + ParentRowId + "' name='txtRiskUnits" + ParentRowId + "' type='number' min='0' onchange=\"EncounterChargeCapture.TotalMinutesAndUnits(" + ParentRowId + "); \" maxlength='15' /></div>";
        var TotalMinutes = "<div class='col-xs-1'><label class='control-label'>Total Minutes</label><input type='text' id='txtTotalMinutes" + ParentRowId + "' name='TotalMinutes" + ParentRowId + "' class='form-control' onchange=\" \" disabled /></div>";
        var TotalUnits = "<div class='col-xs-1'><label class='control-label'>Total Units</label><input type='text' class='form-control' rows='1' id='txtTotalUnits" + ParentRowId + "' maxlength='100' name='txtTotalUnits" + ParentRowId + "' onchange=\"; \" disabled/></div>";
        var RoundBiuldUnits = "<div class='col-xs-2 size-max120'><label class='control-label'>Round Billed Units</label><select class='form-control size150'  id='txtRoundBiuldUnits" + ParentRowId + "' name='RoundBiuldUnits" + ParentRowId + "'' onchange=\"EncounterChargeCapture.AnesthesiaUnits('txtStartTime" + ParentRowId + "', 'txtEndTime" + ParentRowId + "', " + ParentRowId + "); EncounterChargeCapture.TotalMinutesAndUnits(" + ParentRowId + ");\" ><option value='0' id='default'>Default</option><option value='1' id='default'>Round Up</option><option value='2' id='default'>Round Down</option></select></div>";
        var spacer = '<div class="spacer5"></div>';
        AnesthesiaHTML.append(StartTime, EndTime, TimeUnits, BaseUnits, RiskUnits, TotalMinutes, TotalUnits, RoundBiuldUnits, spacer);
        return AnesthesiaHTML;

    },

    rowExpand: function ($row, obj) {
        var _self = obj,
            $actions,
            values = [];
        var row = _self.datatable.row($row);
        if (row.child().first().is(":visible")) {
            // This row is already open - close it
            $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
            row.child().first().hide();
        }
        else {
            $row.find("td:first .fa-plus-square").attr("class", "fa fa-minus-square");
            // Open this row
            row.child().first().show();
        }

    },

    rowSave: function ($row, obj) {
        //Begin edited by Azeem Raza Tayyab on 29-Apr-2016 to Fix Bug#: PMS-1657
        var IsPOSValid = EncounterChargeCapture.ValidatePOS($row);
        var IsCPTValid = EncounterChargeCapture.ValidateCPT($($row));
        if (IsCPTValid && IsPOSValid) {
            EncounterChargeCapture.SaveChargeRow($row, obj);
        }
        else {
            if (!IsCPTValid) {
                utility.myConfirm("Discharge date is required for this CPT, would you like to save without Discharge date?", function () {
                    if (IsPOSValid) {
                        EncounterChargeCapture.SaveChargeRow();
                    }
                    else {
                        utility.myConfirm("Admission date is required for POS 21, would you like to save without Admission date?", function () {
                        }, function () {
                            var self = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture");
                            var Addsection = self.find('#claimAdditionalInfoDiv section:first');
                            Addsection.addClass('active')
                            Addsection.find('.toggle-content:first').css('display', 'block');
                            self.find("#dtpAdmissionDate").focus();
                        }, "Admission date is required for POS 21");
                    }
                }, function () {
                    var self = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture");
                    var Addsection = self.find('#claimAdditionalInfoDiv section:first');
                    Addsection.addClass('active')
                    Addsection.find('.toggle-content:first').css('display', 'block');
                    self.find("#dtpDischargeDate").focus();
                }, "Discharge date is required for this CPT");

            }
            if (!IsPOSValid) {
                utility.myConfirm("Admission date is required for POS 21, would you like to save without Admission date?", function () {
                    /*Begin Added By Azeem Raza Tayyab on 28-Apr-2016 to fix bug#: PMS-1657*/
                    if (IsCPTValid) {
                        EncounterChargeCapture.SaveChargeRow($row, obj);
                    }
                    else {
                        if (!IsCPTValid) {
                            utility.myConfirm("Discharge date is required for this CPT, would you like to save without Discharge date?", function () {
                                EncounterChargeCapture.SaveChargeRow($row, obj);
                            }, function () {
                                var self = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture");
                                var Addsection = self.find('#claimAdditionalInfoDiv section:first');
                                Addsection.addClass('active')
                                Addsection.find('.toggle-content:first').css('display', 'block');
                                self.find("#dtpDischargeDate").focus();
                            }, "Discharge date is required for this CPT");

                        }
                    }
                }, function () {
                    var self = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture");
                    var Addsection = self.find('#claimAdditionalInfoDiv section:first');
                    Addsection.addClass('active')
                    Addsection.find('.toggle-content:first').css('display', 'block');
                    self.find("#dtpAdmissionDate").focus();
                }, "Admission date is required for POS 21");
            }

        }
        //End edited by Azeem Raza Tayyab on 29-Apr-2016 to Fix Bug#: PMS-1657
    },

    //Created by Azeem Raza Tayyab on 29-Apr-2016 to Fix Bug#: PMS-1657
    SaveChargeRow: function ($row, obj) {

        //Override Charge Validation
        EncounterChargeCapture.OverrideChargeValidation($row);
        var IsRowValid = obj.rowValidate($row);
        var IsValidICDPointer = EncounterChargeCapture.ValidateICDPinter($row);
        var IsModifiersValid = EncounterChargeCapture.validateModifiers($row);

        if (IsValidICDPointer && IsModifiersValid && IsRowValid) {

            var CurrentRow = $row;//$row.getMyJSON()
            var myJSON = CurrentRow.getMyJSON();

            var childRow = EncounterChargeCapture.EditableGrid.datatable.row($row).child();
            var childJSON = childRow.getMyJSON();
            var GrandJSON = utility.MergeJSON(myJSON, childJSON);
            var self = null;
            if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
            else
                self = $('#' + EncounterChargeCapture.params.PanelID);
            var pnlICDs = self.find('#pnlICDs');
            var ICDJSON = $(pnlICDs).getMyJSON();
            GrandJSON = utility.MergeJSON(GrandJSON, ICDJSON);
            var ChargeStatus = CurrentRow.attr("ChargeStatus");
            if (ChargeStatus == "Submitted") {
                var ChargeStatusId = String("txtStatus" + $row.attr("id"));
                var statusArray = {
                };
                statusArray[ChargeStatusId] = "Submitted";
                var statusJSON = JSON.stringify(statusArray);

                GrandJSON = utility.MergeJSON(statusJSON, GrandJSON);
            }
            else if (ChargeStatus == "ReSubmit") {
                var ChargeStatusId = String("txtStatus" + $row.attr("id"));
                var statusArray = {
                };

                statusArray[ChargeStatusId] = "ReSubmit";

                var statusJSON = JSON.stringify(statusArray);

                GrandJSON = utility.MergeJSON(statusJSON, GrandJSON);
            }
            var fee = utility.convertToFigure($(CurrentRow).find('input[id*="txtFEE"]').val());

            if (fee > 0) {
                if (EncounterChargeCapture.validateChargeAmounts($row)) {

                    //charge amounts validated
                }
                else {

                    return false;
                }
                if (EncounterChargeCapture.params["VisitId"] != null && EncounterChargeCapture.params["VisitId"] > 0) {
                    var ChargeId = EncounterChargeCapture.ChargeSave(GrandJSON, $row.attr("id"), obj, EncounterChargeCapture.params["VisitId"], CurrentRow);
                }
                else {
                    utility.DisplayMessages("Create Visit first, to save charge", 3);
                }
            }
            else {
                utility.myConfirm('Do you want to save <b>0</b> Fee', function () {
                    if (EncounterChargeCapture.validateChargeAmounts($row)) {
                    }
                    else {

                        return false;
                    }
                    // imp-621

                    if (EncounterChargeCapture.params["VisitId"] != null && EncounterChargeCapture.params["VisitId"] > 0) {
                        var ChargeId = EncounterChargeCapture.ChargeSave(GrandJSON, $row.attr("id"), obj, EncounterChargeCapture.params["VisitId"], CurrentRow);
                    }
                    else {
                        utility.DisplayMessages("Create Visit first, to save charge", 3);
                    }

                }, function () {
                    return false;

                }, 'Confirm <b>0</b> Fee Save');
            }
        }
    },

    rowUp: function ($row, obj) {
       var from_Id = $row.attr("Id");
       var to_Id = "";
       //if ($row.prev().prev().prev().prev().attr("Id")) {
       //    to_Id = $row.prev().prev().prev().prev().attr("Id").replace(/[^\d.]/g, '');
       //}
       //else {
       //    to_Id = $row.prev().attr("Id").replace(/[^\d.]/g, '');
       //}

       if ($row.prev().attr("Id").match(/[a-z]/i)) {
           to_Id = $row.prev().prev().prev().attr("Id");
       }
       else {
           to_Id = $row.prev().attr("Id");
       }

       if (from_Id && to_Id && from_Id != undefined && to_Id != undefined) {
           var data = "FromElementId=" + from_Id + "&ToElementId=" + to_Id;
           EncounterChargeCapture.SortPatientCharge(data).done(function (response) {
               if (response.status != false) {
                   utility.DisplayMessages(response.Message, 1);
                   /************* HIDING CHILD ROW *******************/
                   var row = obj.datatable.row($row);

                   if (row.child.isShown()) {
                       $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                       row.child.hide();
                   }
                   /*************************************************/
                   // Draw row on grid
                   var prevRow = "";

                   if ($row.prev().attr("Id").match(/[a-z]/i)) {
                       prevRow = $row.prev().prev().prev().get(0);
                   }
                   else {
                       prevRow = $row.prev().get(0);
                   }

                   EditableTable.rowSwap(prevRow, $row.get(0));
                   //need to review next line tommorow
                   EncounterChargeCapture.showHideUpDown($row, $row.next());
               }
               else {
                   utility.DisplayMessages(response.Message, 3);
               }
           });
       }
    },

    rowDown: function ($row, obj) {
                var from_Id = $row.attr("Id");
                var to_Id = "";

                if ($row.next().attr("Id").match(/[a-z]/i)) {
                    to_Id = $row.next().next().next().attr("Id");
                }
                else {
                    to_Id = $row.next().attr("Id");
                }

                if (from_Id && to_Id && from_Id != undefined && to_Id != undefined) {
                    var data = "FromElementId=" + from_Id + "&ToElementId=" + to_Id;
                    EncounterChargeCapture.SortPatientCharge(data).done(function (response) {
                        if (response.status != false) {
                            utility.DisplayMessages(response.Message, 1);
                            // Draw row on grid
                            /************* HIDING CHILD ROW *******************/
                            var row = obj.datatable.row($row);

                            if (row.child.isShown()) {
                                $row.find("td:first .fa-minus-square").attr("class", "fa fa-plus-square");
                                row.child.hide();
                            }
                            /*************************************************/

                            var NextRow = "";

                            if ($row.next().attr("Id").match(/[a-z]/i)) {
                                NextRow = $row.next().next().next().get(0);
                            }
                            else {
                                NextRow = $row.next().get(0);
                            }

                            EditableTable.rowSwap($row.get(0), NextRow);
                            EncounterChargeCapture.showHideUpDown($row, $row.prev());
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
    },

    rowDetail: function ($row, obj) {
        var ChargeCapId = Number($row.attr("id"));
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["ChargeCapId"] = ChargeCapId;
                params["IsLocked"] = EncounterChargeCapture.params.IsLocked;
                params["mode"] = "Edit";
                if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView" || EncounterChargeCapture.params.TabID == "mstrTabDashBoard")
                    params["ParentCtrl"] = 'EncounterChargeCapture';
                else
                    params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
                LoadActionPan('chargeSearchDetail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    rowRemove: function ($row, obj) {
        utility.myConfirm('1', function () {
            if ($row.hasClass('adding')) {
            }
            //var _self = obj;
            //_self.datatable.row($row.get(0)).remove().draw();
            if (parseInt($row.attr("id")) > 0) {
                EncounterChargeCapture.ChargeDelete($row.attr("id"), $row, obj);
            }
            else {
                var _self = obj;
                _self.datatable.row($row.get(0)).remove().draw();
                EncounterChargeCapture.RecalculateCPTTotal();
                utility.DisplayMessages("Successfully Deleted", 1);
            }
        }, function () {},'1');
    },

    updateChargeRowId: function (CurrentRow, ChargeId) {
        var CurrentRowOldId = CurrentRow.attr("id");
        var RowId = String(CurrentRow.attr("id")).replace(CurrentRowOldId, ChargeId);
        CurrentRow.attr("id", RowId);

        CurrentRow.find('[type=hidden],[type=text],[type=password],[type=checkbox], textarea, select, checkbox').each(function () {
            if ($(this).attr("id") && $(this).attr("id") != "") {
                var CtrlId = String($(this).attr("id")).replace(CurrentRowOldId, ChargeId);
                $(this).attr("id", CtrlId);
            }
        });
        var ChildRow = EncounterChargeCapture.EditableGrid.datatable.row(CurrentRow).child();
        if (ChildRow) {
            var ChildRowId = String(ChildRow.attr("id")).replace(CurrentRowOldId, ChargeId);
            ChildRow.attr("id", ChildRowId);
            ChildRow.find('[type=hidden],[type=text],[type=password],[type=checkbox], textarea, select, checkbox').each(function () {
                if ($(this).attr("id") && $(this).attr("id") != "") {
                    var CtrlId = String($(this).attr("id")).replace(CurrentRowOldId, ChargeId);
                    $(this).attr("id", CtrlId);
                }
            });
        }
    },

    //END Charge Editable Grid Code

    addCPTRow: function (CPTCode) {
        var insertedRow = EncounterChargeCapture.AddNewChargeRow('Add');
        var CurrentCPT = $(insertedRow).find('input[id*="txtCPT"]');
        CurrentCPT.val(CPTCode);
        var duplicateCPT = false;
        if (CurrentCPT.val() != "") {
            duplicateCPT = EncounterChargeCapture.CheckDuplicateCPT(CurrentCPT);
        }
        if (EncounterChargeCapture.IsSelfPay == "True") {
            $(insertedRow).find('input[id^="txtINSCharges"]').prop('disabled', true);
            $(insertedRow).find('input[id^="txtCOPAY"]').prop('disabled', true);
        }
        else {
            $(insertedRow).find('#chkBillToPatient').prop('checked', false);
            $(insertedRow).find('input[id^="txtCOPAY"]').prop('disabled', false);
        }
        if (duplicateCPT == true) {
            utility.myConfirm('This code already exists, do you want to add this code again?', function () {
                EncounterChargeCapture.FillCurrentCPTFee(CPTCode, insertedRow);

                $(insertedRow).find('input[id*="txtCPT"]').val(CPTCode);
                //$(insertedRow).find('input[id*="hfCPT"]').val(CPTCode);

                /*********************/
                var newChildRow = EncounterChargeCapture.EditableGrid.datatable.row(insertedRow).child();

                var PANDropDown = $(newChildRow).find('datalist[id*="ddlPriorAuthorization"]');
                var hfAuthorizedId = $(newChildRow).find('input[id*="hfAuthorizationId"]');
                var curentPANCtrl = $(newChildRow).find('input[id*="txtPriorAuthorization"]');
                /************************/

                EncounterChargeCapture.fillChargeChildRow(CPTCode, insertedRow, newChildRow);

                EncounterChargeCapture.LoadPreAuthorizations(insertedRow, PANDropDown, curentPANCtrl, hfAuthorizedId);
            }, function () {
                $(CurrentCPT).val('');
                $(CurrentCPT).focus();
                return false;
            }, 'Confirmation duplicate CPT');
            //End Added By Azeem Raza Tayyab on  07-Apr-2016 to Fix Bug#:PMS-1985
        }
        else {
            EncounterChargeCapture.FillCurrentCPTFee(CPTCode, insertedRow);

            $(insertedRow).find('input[id*="txtCPT"]').val(CPTCode);
            //$(insertedRow).find('input[id*="hfCPT"]').val(CPTCode);

            /*********************/
            var newChildRow = EncounterChargeCapture.EditableGrid.datatable.row(insertedRow).child();

            var PANDropDown = $(newChildRow).find('datalist[id*="ddlPriorAuthorization"]');
            var hfAuthorizedId = $(newChildRow).find('input[id*="hfAuthorizationId"]');
            var curentPANCtrl = $(newChildRow).find('input[id*="txtPriorAuthorization"]');
            /************************/

            EncounterChargeCapture.fillChargeChildRow(CPTCode, insertedRow, newChildRow);

            EncounterChargeCapture.LoadPreAuthorizations(insertedRow, PANDropDown, curentPANCtrl, hfAuthorizedId);
        }
    },

    setCurrentFee: function (response, insertedRow) {
        var CPTLoad_detail = JSON.parse(response.CPTLoad_JSON);
        $.each(CPTLoad_detail, function (i, item) {
            var deffered = $.Deferred();
            var CurrentFee = item.Fee == "" ? 0 : item.Fee;
            var CurrentExpectedFee = item.ExpectedFee == "" ? 0 : item.ExpectedFee;
            var fee = parseFloat(CurrentFee);
            var Expectedfee = parseFloat(CurrentExpectedFee);

            var units = parseFloat($(insertedRow).find('input:text[id*="txtUnits"]').val());

            $(insertedRow).find('input[id*="txtFEE"]').val(fee);
            $(insertedRow).find('input[id*="txtTotalFEE"]').val(fee * units);
            $(insertedRow).find('input[id*="txtUnits"]').trigger("blur");
            $(insertedRow).find('input[id*="hfFee"]').val(fee);
            $(insertedRow).find('input[id*="hfExpectedFee"]').val(Expectedfee);
            var childRow = EncounterChargeCapture.EditableGrid.datatable.row(insertedRow).child();
            //Edited by Azeem Raza Tayyab to fix bug#: PMS-4118
            $(childRow).find('input[id*="txtExpectedFee"]').val((units * Expectedfee).toFixed(2));

            $(insertedRow).find('input[id*="txtTotalFEE"]').val((units * fee).toFixed(2));
            //Begin Added by Azeem Raza Tayyab on 17-Mar-2016 to fix Bug#:PMS-2397
            if (item.Modifier1 && $(insertedRow).find('input[id*="txtModifier1"]').val() == "") {
                $(insertedRow).find('input[id*="txtModifier1"]').val(item.Modifier1);
            }
            if (item.Modifier2 && $(insertedRow).find('input[id*="txtModifier2"]').val() == "") {
                $(insertedRow).find('input[id*="txtModifier2"]').val(item.Modifier2);
            }
            if (item.Modifier3 && $(insertedRow).find('input[id*="txtModifier3"]').val() == "") {
                $(insertedRow).find('input[id*="txtModifier3"]').val(item.Modifier3);
            }
            if (item.Modifier4 && $(insertedRow).find('input[id*="txtModifier4"]').val() == "") {
                $(insertedRow).find('input[id*="txtModifier4"]').val(item.Modifier4);
            }
            //End Added by Azeem Raza Tayyab on 17-Mar-2016 to fix Bug#:PMS-2397


            // find its anesthesia row and fill BasicUnits into BasicUnits field.

            //bug#PMS-1510
            var originalVal = $(insertedRow).find('input[id*="txtCPT"]').parent().attr("data-originalval");
            self.find("#Anesthesia" + insertedRow.attr('id')).find("#txtBaseUnits" + insertedRow.attr('id')).val(item.BasicUnits);
            var changedVal = $(insertedRow).find('input[id*="txtCPT"]').parent().siblings().find("input[id*='hfCPTDescription']").val();
            if (originalVal != changedVal) {
                self.find("#Anesthesia" + insertedRow.attr('id')).find("#txtBaseUnits" + insertedRow.attr('id')).trigger('change');
            }

            deffered.resolve();
            return deffered;
        });
    },

    FillCurrentCPTFee: function (CPTCode, insertedRow) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        if (CPTCode != null && CPTCode != "") {
            var childRow = EncounterChargeCapture.EditableGrid.datatable.row(insertedRow).child();

            var self = null;
            if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
            else
                self = $('#' + EncounterChargeCapture.params.PanelID);

            var provider = self.find('#hfProvider').val();
            var facility = self.find('#hfFacility').val();
            var practice = self.find('#hfPractice').val();
            var patientInsuraceId = "0";
            if (EncounterChargeCapture.SelectedInsurancePlan) {
                patientInsuraceId = $(EncounterChargeCapture.SelectedInsurancePlan).attr('insuranceid');
            }
            var POSCode = $(insertedRow).find('input[id*="txtPOS"]').val() == "" ? 0 : $(insertedRow).find('input[id*="txtPOS"]').val();
            var Modifier1 = $(insertedRow).find('input[id*="txtModifier1"]').val();
            var Modifier2 = $(insertedRow).find('input[id*="txtModifier2"]').val();
            var Modifier3 = $(insertedRow).find('input[id*="txtModifier3"]').val();
            var Modifier4 = $(insertedRow).find('input[id*="txtModifier4"]').val();
            var ChargeDOS = $(insertedRow).find('input[id*="dtpDOSFrom"]').val();
            EncounterChargeCapture.FillCPTFee(EncounterChargeCapture.params.VisitId, CPTCode, provider, facility, practice, patientInsuraceId, POSCode, Modifier1, Modifier2, Modifier3, Modifier4, ChargeDOS).done(function (response) {
                if (response.status != false && response.CPTCount > 0) {

                    $.when(EncounterChargeCapture.setCurrentFee(response, insertedRow)).then(function () {
                        EncounterChargeCapture.ValidateCharges($(insertedRow).find('input[id*="txtUnit"]').attr('id'), $(insertedRow).find('input[id*="txtTotalFEE"]').attr('id'), $(insertedRow).find('input[id*="txtINSCharges"]').attr('id'), $(insertedRow).find('input[id*="txtPATCharges"]').attr('id'), $(insertedRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient')
                    });

                }
                else {
                    $(insertedRow).find('input[id*="hfFee"]').val(0);
                    $(insertedRow).find('input[id*="txtFEE"]').val(0);
                    $(insertedRow).find('input[id*="txtTotalFEE"]').val(0);
                    $(insertedRow).find('input[id*="hfExpectedFee"]').val(0);
                    $(childRow).find('input[id*="txtExpectedFee"]').val(0);
                    EncounterChargeCapture.ValidateCharges($(insertedRow).find('input[id*="txtUnit"]').attr('id'), $(insertedRow).find('input[id*="txtTotalFEE"]').attr('id'), $(insertedRow).find('input[id*="txtINSCharges"]').attr('id'), $(insertedRow).find('input[id*="txtPATCharges"]').attr('id'), $(insertedRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
                }
                 EncounterChargeCapture.RecalculateCPTTotal();
            });
        }
        else {
            $(insertedRow).find('input[id*="hfFee"]').val(0);
            $(insertedRow).find('input[id*="txtFEE"]').val(0);
            $(insertedRow).find('input[id*="txtTotalFEE"]').val(0);
            $(insertedRow).find('input[id*="hfExpectedFee"]').val(0);
            $(childRow).find('input[id*="txtExpectedFee"]').val(0);
            //  $(childRow).find('input[id*="txtUnit"]').val(1);
            self.find("#Anesthesia" + insertedRow.attr('id')).find("#txtBaseUnits" + insertedRow.attr('id')).val(0);
            self.find("#Anesthesia" + insertedRow.attr('id')).find("#txtBaseUnits" + insertedRow.attr('id')).trigger('change');
            EncounterChargeCapture.ValidateCharges($(insertedRow).find('input[id*="txtUnit"]').attr('id'), $(insertedRow).find('input[id*="txtTotalFEE"]').attr('id'), $(insertedRow).find('input[id*="txtINSCharges"]').attr('id'), $(insertedRow).find('input[id*="txtPATCharges"]').attr('id'), $(insertedRow).find('input[id*="txtCOPAY"]').attr("id"), 'patient');
        }

       
    },

    fillCodeInTable: function (code, codeType, replaceExisting, currRow, RowData) {

        var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
        var dataTable = $(ChargeGridId).DataTable();

        //node wise traversal
        var colIndex = "";

        if (codeType.toLowerCase() == "modifier") {
            colIndex = 5;
        }
        else if (codeType.toLowerCase() == "icd") {
            colIndex = 6;
        }

        else if (codeType.toLowerCase() == "pos") {
            colIndex = 7;
        }

        if (colIndex != "") {
            dataTable.column(colIndex).nodes().each(function (node, index) {
                if ((dataTable.column(colIndex).nodes().length - 1) == index)
                    EncounterChargeCapture.findAndFillWithCode(node, code, replaceExisting, currRow, codeType, RowData);
            });
        }
    },

    findAndFillWithCode: function (node, code, replaceExisting, currRow, codeType, RowData) {
        var alreadyExists = false;
        var availableIndex = -1;
        if (codeType.toLowerCase() == "pos" && currRow != null) {
            $(currRow).find('input[id*="txtPOS"]').val(code);
            return;
        }
        $(node).find('input:text').each(function (innerIndex, innerValue) {

            if ($(innerValue).val() == code) {
                alreadyExists = true;
                return false;
            }
            if (availableIndex < 0 && $(innerValue).val() == "")
                availableIndex = innerIndex;
        });

        if (replaceExisting != true && !alreadyExists && availableIndex >= 0) {
            $($(node).find('input:text')[availableIndex]).val(code);
            $($(node).find('input:hidden')[availableIndex]).val(code);

            var ContainerCtrl = $(node).find('input:text')[availableIndex].id.substring($(node).find('input:text')[availableIndex].id.indexOf("txtICD") + 6);

            var snomedCode = "", snomedDescription = "", icd10Code = "", icd10Description = "", icd9Code = "", icd9Description = "";
            if (RowData != null) {
                var ICD9 = RowData.substring(0, RowData.lastIndexOf("!"));
                var ICD10 = RowData.substring(RowData.lastIndexOf("!") + 1, RowData.lastIndexOf("@"));
                var SNOMED = RowData.substring(RowData.lastIndexOf("@"));

                icd9Code = ICD9.split("-")[0];
                icd9Description = ICD9.split("-")[1];

                icd10Code = ICD10.split("-")[0];
                icd10Description = ICD10.split("-")[1];

                snomedCode = SNOMED.split("-")[0];
                snomedDescription = SNOMED.split("-")[1];

                $("#txtICD" + ContainerCtrl).val(icd10Code);
                $("#hfICD" + ContainerCtrl).val(icd9Code);
                $("#hfICDDescription" + ContainerCtrl).val(icd9Description);
                $("#hfICD10" + ContainerCtrl).val(icd10Code);
                $("#hfICD10Description" + ContainerCtrl).val(icd10Description);
                $("#hfSNOMED" + ContainerCtrl).val(snomedCode);
                $("#hfSNOMEDDescription" + ContainerCtrl).val(snomedDescription);
                $("#txtICD" + ContainerCtrl).parent("div").attr("data-original-title", icd10Description).attr("data-toggle", "tooltip").attr("data-placement", "right");
                //Set ToolTip for Comments.
                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
            }
        }
        else {

            if (codeType.toLowerCase() != "icd" && codeType.toLowerCase() != "modifier")
                $($(node).find('input:text')).val(code);
        }
    },
    // END Fill Code(s) in Table

    LoadPreAuthorizations: function (insertedRow, PANDropDown, curentPANCtrl, hfAuthorizedId) {
        var patientInsuranceId = "";
        if (EncounterChargeCapture.SelectedInsurancePlan) {
            patientInsuranceId = $(EncounterChargeCapture.SelectedInsurancePlan).attr("insuranceid");
        }
        var DOSFrom = $(insertedRow).find('input[id*="dtpDOSFrom"]').val();
        var DOSTo = $(insertedRow).find('input[id*="dtpDOSTo"]').val();
        var CPTCode = $(insertedRow).find('input[id*="txtCPT"]').val();

        //if any of the parameter is missing don't call the service
        if (patientInsuranceId != "" && DOSFrom != "" && DOSTo != "" && CPTCode != "") {
                    var parameters = "PatientID=" + $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val() + "&patientInsuranceId=" + patientInsuranceId + "&CPTCode=" + CPTCode + "&DOSFrom=" + DOSFrom + "&DOSTo=" + DOSTo;
                    EncounterChargeCapture.PreAuthorizationLoad(parameters).done(function (response) {
                        if (response.status != false) {
                            var authorizationDetail = JSON.parse(response.PatientPreAuthorizationLoad_JSON);
                            EncounterChargeCapture.bindPANDropDown(authorizationDetail, PANDropDown, curentPANCtrl, hfAuthorizedId)
                        }
                        else {
                        }
                    });
        }
    },

    PreAuthorizationLoad: function (parameters) {
        return MDVisionService.defaultService(parameters, "PATIENT_PREAUTHORIZATION", "LOAD_CPT_PREAUTHORIZATION");
    },

    bindPANDropDown: function (authorizationDetail, PANDropDown, curentPANCtrl, hfAuthorizedId) {

        $(PANDropDown).empty();
        curentPANCtrl.val("");
        hfAuthorizedId.val("");

        $.each(authorizationDetail, function (i, item) {

            $(PANDropDown).append($("<option />").val(item.PAN).attr("authorizeId", item.AuthorizeId));

        });
        if (PANDropDown.length == 1) {
            $(curentPANCtrl).val($(PANDropDown).find('option:first').val());
            $(hfAuthorizedId).val($(PANDropDown).find('option:first').attr('authorizeId'));
        }
    },

    removeDialogClasses: function () {
        $('#' + Encounter_Visits.params.PanelID + ' .modal-header').hide();
        $('#' + Encounter_Visits.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Encounter_Visits.params.PanelID + ' .modal-dialog').removeAttr('class');
        $('#' + Encounter_Visits.params.PanelID + ' #containerModalDialog').removeClass('modal-dialog');
    },

    DisableEditableGrid: function (isDisabled, isResubmit) {
        var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
        var dataTable = $(ChargeGridId).DataTable();
        dataTable.row().nodes().each(function (parentRow, index) {
            if (isDisabled == true) {
                EncounterChargeCapture.DisableEditableCurrunRow(parentRow);
            }
            else {
                EncounterChargeCapture.EnableEditableRow(parentRow);
            }
        });
        if (isDisabled == false && isResubmit == true) {
            $("#" + EncounterChargeCapture.params.PanelID + " #aReSubmit").addClass("disableAll");
            $("#" + EncounterChargeCapture.params.PanelID + " #dtpSubmittedDate, #txtSubmittedBy").val('');
            $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr").each(function (i, j) {

                $(this).attr('chargestatus', 'Resubmit');
            });
            EncounterChargeCapture.VisitChargesResubmit(EncounterChargeCapture.params.VisitId);
            EncounterChargeCapture.LockClaim(EncounterChargeCapture.params.IsLocked);
            EncounterChargeCapture.LoadClaimDetails(EncounterChargeCapture.params.VisitId, EncounterChargeCapture.params.patientID);
        }
        else {
            EncounterChargeCapture.LockClaim(EncounterChargeCapture.params.IsLocked);
        }
    },

    PatientInsuranceLoad: function (PatientInsuranceId) {
                Patient_Insurance.FillPatientInsurance(PatientInsuranceId).done(function (response) {
                    if (response.status != false) {
                        var isnurance_detail = JSON.parse(response.InsuranceFill_JSON);
                        if (isnurance_detail.txtEmployer != "") {
                            $("#" + EncounterChargeCapture.params.PanelID + ' #RadEmploymentYes').trigger('click');
                        } 
                        else {
                            $("#" + EncounterChargeCapture.params.PanelID + ' #RadEmploymentNo').trigger('click');
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
    },
    enableUpAndDown: function (CurrentRow) {
        CurrentRow.find("td.actions a.up-row,a.down-row").each(function () {
            $(this).removeClass('hidden')
        });
    },

    enableRemoveRow: function (CurrentRow) {
        CurrentRow.find("td.actions .remove-row").removeClass("hidden");
    },

    SortPatientCharge: function (PatientChargeData) {
        var data = PatientChargeData;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "UPDATE_CHARGE_ORDER");
    },

    Re_CalculateCPTFee: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var data_ = [];
        $(self).find("#dgvVisitCharge tbody tr.adding").each(function (i, item) {

            var provider = self.find('#hfProvider').val();
            var facility = self.find('#hfFacility').val();
            var practice = self.find('#hfPractice').val();
            var patientInsuraceId = "0";
            if (EncounterChargeCapture.SelectedInsurancePlan) {
                patientInsuraceId = $(EncounterChargeCapture.SelectedInsurancePlan).attr('insuranceid')
            }
            //self.find("#ddlPatientInsurance option:selected").val() == "" ? "0" : self.find("#ddlPatientInsurance option:selected").val();
            var POSCode = $(item).find('input[id*="txtPOS"]').val() == "" ? 0 : $(item).find('input[id*="txtPOS"]').val();
            var CPT = $(item).find('input[id*="txtCPT"]').val();
            var Modifier1 = $(item).find('input[id*="txtModifier1"]').val();
            var Modifier2 = $(item).find('input[id*="txtModifier2"]').val();
            var Modifier3 = $(item).find('input[id*="txtModifier3"]').val();
            var Modifier4 = $(item).find('input[id*="txtModifier4"]').val();
            var ChargeDOS = $(item).find('input[id*="dtpDOSFrom"]').val();
            var ChargeId = $(item).attr('id');

            data_.push({
                Provider: provider,
                Facility: facility,
                Practice: practice,
                PatientInsuraceId: patientInsuraceId,
                POSCode: POSCode,
                CPT: CPT,
                Modifier1: Modifier1,
                Modifier2: Modifier2,
                Modifier3: Modifier3,
                Modifier4: Modifier4,
                ChargeDOS: ChargeDOS,
                ChargeId: ChargeId
            });

        });

        var jsonData = JSON.stringify(data_);
        EncounterChargeCapture.ReCalculateCPTFee(jsonData, EncounterChargeCapture.params.VisitId).done(function (response) {
            if (response.status != false) {

                var AllCPTJsonList = JSON.parse(response.AllCPTJsonList);
                $.each(AllCPTJsonList, function (i, item) {

                    var JItem = JSON.parse(item);
                    var $currentRow = $(self).find("#dgvVisitCharge tbody tr[id='" + JItem.ChargeId + "']");

                    if ($currentRow) {
                        $.when(EncounterChargeCapture.setCurrentFee(JItem, $currentRow)).then(function () {

                            EncounterChargeCapture.ValidateCharges(
                                $($currentRow).find('input[id*="txtUnit"]').attr('id'),
                                $($currentRow).find('input[id*="txtTotalFEE"]').attr('id'),
                                $($currentRow).find('input[id*="txtINSCharges"]').attr('id'),
                                $($currentRow).find('input[id*="txtPATCharges"]').attr('id'),
                                $($currentRow).find('input[id*="txtCOPAY"]').attr("id"),
                                'patient')
                        });
                    }
                });
            }
            else {
            }
        });


    },

    FillCPTFee: function (VisitId, CPTCode, ProviderId, FacilityId, PracticeId, patientInsuraceId, POSCode, Modifier1, Modifier2, Modifier3, Modifier4, ChargeDOS) {
        if (VisitId == null) {
            VisitId = 0;
        }
        if (CPTCode == null) {
            CPTCode = "";
        }
        if (FacilityId == null) {
            FacilityId = 0;
        }
        if (PracticeId == null) {
            PracticeId = 0;
        }
        if (patientInsuraceId == null) {
            patientInsuraceId = 0;
        }
        if (POSCode == null) {
            POSCode = "";
        }
        if (Modifier1 == null) {
            Modifier1 = "";
        }
        if (Modifier2 == null) {
            Modifier2 = "";
        }
        if (Modifier3 == null) {
            Modifier3 = "";
        }
        if (Modifier4 == null) {
            Modifier4 = "";
        }
        if (ChargeDOS == null) {
            ChargeDOS = "";
        }
        var data = "VisitId=" + VisitId + "&CPTCode=" + CPTCode + "&ProviderId=" + ProviderId + "&FacilityId=" + FacilityId + "&PracticeId=" + PracticeId
                    + "&patientInsuraceId=" + patientInsuraceId + "&POSCode=" + POSCode + "&Modifier1=" + Modifier1 + "&Modifier2=" + Modifier2
                    + "&Modifier3=" + Modifier3 + "&Modifier4=" + Modifier4 + "&ChargeDOS=" + ChargeDOS;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "FILL_CPT_FEE");
    },

    ReCalculateCPTFee: function (data, VisitId) {

        var Data = "CPTData=" + data + "&VisitId=" + VisitId;
        return MDVisionService.defaultService(Data, "PATIENT_CHARGE_CAPTURE", "FILL_ALL_CPT_FEE");
    },

    showHideUpDown: function ($row, $RowSwapedWith) {
        var totalArrayLength = $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr").length - 2;
        var rowIndex = $.inArray($row[0], $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr")) - 1;
        var RowSwapIndex = $.inArray($RowSwapedWith[0], $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr")) - 1;

        if (rowIndex == 0) {
            $row.find("td.actions a.up-row").hide();
            $row.find("td.actions a.down-row").show();
        }
        if (rowIndex > 0) {
            $row.find("td.actions a.up-row").show();
            $row.find("td.actions a.down-row").show();
        }
        if (rowIndex == totalArrayLength) {
            $row.find("td.actions a.up-row").show();
            $row.find("td.actions a.down-row").hide();
        }
        if (RowSwapIndex == 0) {
            $RowSwapedWith.find("td.actions a.up-row").hide();
            $RowSwapedWith.find("td.actions a.down-row").show();
        }
        if (RowSwapIndex > 0) {
            $RowSwapedWith.find("td.actions a.up-row").show();
            $RowSwapedWith.find("td.actions a.down-row").show();
        }
        if (RowSwapIndex == totalArrayLength) {
            $RowSwapedWith.find("td.actions a.up-row").show();
            $RowSwapedWith.find("td.actions a.down-row").hide();
        }
        if ($("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr").last().attr('Id').indexOf('Anesthesia') > -1) {
            if ($("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr").last().prev().attr('Id').indexOf('Child') > -1) {
                $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr").last().prev().prev().find("td.actions a.down-row").hide();
            }
            else {
                $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr").last().prev().find("td.actions a.down-row").hide();
            }
        } else {
            $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge tr").last().find("td.actions a.down-row").hide();
        }
    },


    setDosFromInCharges: function () {
        var claimFromdDate = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpDOSFrom').datepicker("getDate");
        var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
        var dataTable = $(ChargeGridId).DataTable();

        dataTable.row().nodes().each(function (row, index) {

            var chargeFromDate = $(row).find('input[id*="dtpDOSFrom"]').datepicker("getDate");

            if ((claimFromdDate - chargeFromDate) >= 0) {
                //claim from date is greater
                $(row).find('input[id*="dtpDOSFrom"]').val($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpDOSFrom').val());
            }
            else {
            }
            $(row).find('input[id*="dtpDOSFrom"]').datepicker('setStartDate', claimFromdDate);
            $(row).find('input[id*="dtpDOSTo"]').datepicker('setStartDate', claimFromdDate);
        });

    },

    setDosToInCharges: function () {
        var claimToDate = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpDOSTo').datepicker("getDate");
        var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
        var dataTable = $(ChargeGridId).DataTable();

        dataTable.row().nodes().each(function (row, index) {

            var chargeToDate = $(row).find('input[id*="dtpDOSTo"]').datepicker("getDate");
            var chargeFromDate = $(row).find('input[id*="dtpDOSFrom"]').datepicker("getDate");
            if ((claimToDate - chargeToDate) > 0) {

                //ok
                // alert('greater than zero');
            }
            else {

                //not ok
                if ((chargeToDate - chargeFromDate) > 0) {
                    $(row).find('input[id*="dtpDOSTo"]').val("");
                }
                else {
                    $(row).find('input[id*="dtpDOSTo"]').val($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpDOSTo').val());
                    $(row).find('input[id*="dtpDOSFrom"]').val($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpDOSTo').val());
                }
            }
            $(row).find('input[id*="dtpDOSFrom"]').datepicker('setEndDate', claimToDate);
            $(row).find('input[id*="dtpDOSTo"]').datepicker('setEndDate', claimToDate);
        });
    },

    isRequired: function (field) {
        var isAutoAccident = $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #RadAutoYes').prop('checked');
        var isOtherAccident = $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #RadOtherYes').prop('checked');

        if (field == 'injuryDate') {
            return (isAutoAccident || isOtherAccident);
        }
        else if (field == 'state') {
            return (isAutoAccident && !isOtherAccident);
        }
    },

    validateAdmissionDate: function () {

        //addmission date should not be later than current date and should not later than dos range

        var dtpAdmissionEndDate = '';
        var DOSTo = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpDOSTo').datepicker("getDate");
        var currentDate = new Date();

        var admissionDate = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #dtpAdmissionDate').datepicker("getDate");

        if (DOSTo < currentDate) {
            dtpAdmissionEndDate = DOSTo;
        }
        else {
            dtpAdmissionEndDate = currentDate;

        }
        if (admissionDate > dtpAdmissionEndDate)
            $('#' + EncounterChargeCapture.params.PanelID + ' #dtpAdmissionDate').val('');


        $('#' + EncounterChargeCapture.params.PanelID + ' #dtpAdmissionDate').datepicker('setEndDate', dtpAdmissionEndDate);
    },

    validateDosFromTo: function (objDosFrom, objDosTo) {
        $(objDosFrom).datepicker('setEndDate', new Date());
        $(objDosTo).datepicker('setEndDate', new Date());

    },


    VisitChargesResubmit: function (VisitId) {
        EncounterChargeCapture.ResubmitVisitCharges(VisitId).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ResubmitVisitCharges: function (VisitID) {
        var data = "VisitID=" + VisitID;
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "RESUBMIT");
    },

    ShowHideEditableGridRows: function (isShow) {
        var ChargeGridId = "#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge";
        var dataTable = $(ChargeGridId).DataTable();
        dataTable.row().nodes().each(function (parentRow, index) {
            var row = EncounterChargeCapture.EditableGrid.datatable.row(parentRow);
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

    ShowHistory: function (ParentControlId, IsNotesLog) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var ChargeCapIds = "";
        self.find("#frmEncounterChargeCapture #dgvVisitCharge tr").each(function () {
            if ($(this).attr("id") && $(this).attr("id") != null) {
                if (ChargeCapIds != "")
                    ChargeCapIds = ChargeCapIds + ",";
                ChargeCapIds = ChargeCapIds + $(this).attr("id");
            }
        });
        var params = [];
        params["PanelID"] = EncounterChargeCapture.params.PanelID;
        params["VisitId"] = EncounterChargeCapture.params["VisitId"];
        params["patientID"] = EncounterChargeCapture.params.patientID;
        params["ChargeCapId"] = ChargeCapIds == "" ? 0 : ChargeCapIds;
        if ("EncounterChargeCapture" + EncounterChargeCapture.params["VisitId"] == EncounterChargeCapture.params.TabID) {
            params['ParentCtrl'] = "EncounterChargeCapture" + EncounterChargeCapture.params["VisitId"];
        } else {
            params['ParentCtrl'] = "EncounterChargeCapture";
        }
        params["IsNotesLog"]= IsNotesLog;
        LoadActionPan('Activity_Log', params);
    },

    FillChargeBatchViewer: function () {
        var self = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture');
        var PatientName = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #txtPatientName').val() + " - " + $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #txtPatientFullName').val()
        ChargeBatch_Viewer.BindVisitAndClaimNummer(EncounterChargeCapture.params.VisitId, self.find('#txtClaimNumber').val(), PatientName, $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture #hfPatientId').val());
        ChargeBatch_Viewer.BatchChargeDocumentUpdate(false);
        ChargeBatch_Viewer.ActiveBtns(true);
        $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
    },

    ValidateFromToDate: function (FormId, CtrlFromDateId, CtrlToDateId, IsOptional, onFromDateChangeCallback, onToDateChangeCallback) {

        var CtrlForm = "#" + FormId;
        var CtrlFromDate = CtrlForm + " #" + CtrlFromDateId;
        var CtrlToDate = CtrlForm + " #" + CtrlToDateId;
        var CtrFromDateName = $(CtrlToDate).attr("name");
        var CtrToDateName = $(CtrlToDate).attr("name");

        var date_format = 'dd/mm/yyyy';
        //set default Date Formate
        if (globalAppdata['DateFormat'])
            date_format = globalAppdata['DateFormat'];

        $(CtrlToDate).attr('disabled', true);
        $(CtrlToDate).attr('maxlength', '10');
        $(CtrlFromDate).attr('maxlength', '10');
        $(CtrlFromDate).datepicker({
            todayHighlight: true,
            format: date_format,
            todayBtn: 'linked',
        }).change(function (e) {
            if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                var fromDate = new Date($(CtrlFromDate).val());
                var toDate = new Date($(CtrlToDate).val());

                if (fromDate <= toDate && fromDate != '') {
                    $(CtrlToDate).val($(CtrlToDate).val()).datepicker('update');
                } else {
                    $(this).val('');
                }
            }
            $(CtrlToDate).attr('disabled', false);
            if (!IsOptional) {
                if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                    $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);
                }

            }
            $(this).datepicker('hide');
            if (!IsOptional) {
                if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                    $(CtrlForm).bootstrapValidator('revalidateField', CtrFromDateName);
                }
            }

            var inputDate = $(CtrlFromDate).datepicker('getDate');
            var date_format = 'dd/mm/yyyy';
            if (globalAppdata['DateFormat'])
                date_format = globalAppdata['DateFormat'];
            if ($(this).val().length == date_format.length) {
                if (!utility.isValidDate($(this).val())) {
                    $(this).val('');
                    utility.DisplayMessages("Please enter valid date", 3);
                }
            }
            if (onFromDateChangeCallback != null && typeof (onFromDateChangeCallback) == 'function') {
                setTimeout(onFromDateChangeCallback, 50);
            }

        }).on('keypress', function (e) {
            utility.preventAlphabetsInDatePicker(e);
        });

        $(CtrlToDate).datepicker({
            todayHighlight: true,
            format: date_format,
        }).change(function (e) {

            $(this).datepicker('hide');
            if (!IsOptional) {
                if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
                    $(CtrlForm).bootstrapValidator('revalidateField', CtrToDateName);
                }
            }
            if (onToDateChangeCallback != null && typeof (onToDateChangeCallback) == 'function') {
                setTimeout(onToDateChangeCallback, 50);
            }
            var CurrentDatepicker = this;
            setTimeout(function () {
                if ($(CurrentDatepicker).val().length == date_format.length) {
                    if (!utility.isValidDate($(CurrentDatepicker).val())) {
                        $(CurrentDatepicker).val('');
                        utility.DisplayMessages("Please enter valid date", 3);
                    }
                    if ($(CtrlFromDate).val() != '' && $(CtrlToDate).val() != '') {
                        var fromDate = new Date($(CtrlFromDate).val());
                        var toDate = new Date($(CtrlToDate).val());
                        if (fromDate > toDate) {
                            $(CurrentDatepicker).val('');
                        }
                    }
                }
            }, 50);
        }).on('keypress', function (e) {
            utility.preventAlphabetsInDatePicker(e);
        });

        $(CtrlFromDate).on('blur', function (e) {
            setTimeout(
               function () {
                   if ($(CtrlFromDate).val() != '') {
                       utility.ValidateDate(CtrlFromDate);
                   }

               }, 100);
        });
        $(CtrlToDate).on('blur', function (e) {
            setTimeout(function () {
                if ($(CtrlToDate).val() != '') {
                    utility.ValidateDate(CtrlToDate);
                }
            }, 100);
        });
    },

    SearchReferrals: function (PatientInsuranceId, ProviderId, FacilityId, ReferralDate, IsActive) {
        if (PatientInsuranceId == null) {
            PatientInsuranceId = "";
        }
        if (ProviderId == null) {
            ProviderId = "";
        }
        if (FacilityId == null) {
            FacilityId = "";
        }
        if (ReferralDate == null) {
            ReferralDate = "";
        }
        if (IsActive == null) {
            IsActive = "";
        }
        var data = "PatientInsuranceID=" + PatientInsuranceId;
        return MDVisionService.defaultService(data, "PATIENT_REFERRAL", "SEARCH_REFERRAL");
    },

    ValidateICDCode: function (item) {
        var Id = $(item).attr('id').split("ICD")[1];
        var hfICD = $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD' + Id).val();
        var hfICDDescription = $("#" + EncounterChargeCapture.params.PanelID).find('#hfICDDescription' + Id).val();
        var hfICD10 = $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD10' + Id).val();
        var hfICD10Description = $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD10Description' + Id).val();
        var hfSNOMED = $("#" + EncounterChargeCapture.params.PanelID).find('#hfSNOMED' + Id).val();
        var hfSNOMEDDescription = $("#" + EncounterChargeCapture.params.PanelID).find('#hfSNOMEDDescription' + Id).val();

        if ($(item).val() != "") {
            Admin_IMOICD.SearchICD(null, $(item).val(), null, null).done(function (response) {
                var ICDCodes = JSON.parse(response.ICDLoad_JSON);
                if (ICDCodes.length > 0) {
                    var outputArr = ICDCodes.filter(function (obj) {
                        if (obj.ICD9 == $(item).val() || obj.ICD10 == $(item).val()) {
                            return true;
                        }
                    });
                    if (outputArr.length <= 0) {
                        $(item).val('');
                        $(item).parent("div").attr("data-original-title", "");
                        utility.DisplayMessages("Invalid ICD Code.", 3);
                    }
                    else {
                        // set first ICD Values if all fields are empty.
                        if (hfICD == "" && hfICDDescription == "" && hfICD10 == "" && hfICD10Description == "" && hfSNOMED == "" && hfSNOMEDDescription == "") {
                            $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD' + Id).val(outputArr[0].ICD9);
                            $("#" + EncounterChargeCapture.params.PanelID).find('#hfICDDescription' + Id).val(outputArr[0].Description);
                            $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD10' + Id).val(outputArr[0].ICD10);
                            $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD10Description' + Id).val(outputArr[0].ICD10Description);
                            $("#" + EncounterChargeCapture.params.PanelID).find('#hfSNOMED' + Id).val(outputArr[0].SNOMEDId);
                            $("#" + EncounterChargeCapture.params.PanelID).find('#hfSNOMEDDescription' + Id).val(outputArr[0].SNOMEDDescription);
                            $(item).parent("div").attr("data-original-title", outputArr[0].ICD10Description);
                            $(item).parent("div").attr("title", outputArr[0].ICD10Description);

                        }
                    }
                }
                else {
                    $(item).val('');
                    $(item).parent("div").attr("data-original-title", "");
                    utility.DisplayMessages("Invalid ICD Code.", 3)
                }
            });
        }
        else
            $(item).parent("div").attr("data-original-title", "");
    },

    validateICD: function ($row) {
        var isValid = true;
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var $childRow = EditableGrid.datatable.row($row).child();
        if (self.find('#ddlSubmitStatus option:selected').text().toLowerCase() == "pending")
            return isValid;

        // ICD's
        var ICDs = $($row).find("input[id*='txtICD']");
        ICDs.each(function (item) {
            if ($(this).val() != "") {
                for (var i = 0; i < ICDs.index($(this)) ; i++) {
                    if ($(ICDs[i]).val() == "") {
                        $(ICDs[i]).css("border", "1px solid red");
                        isValid = false;
                    }
                    else
                        $(ICDs[i]).css("border", "");
                }
            }
            else {
            }
        });

        return isValid;
    },

    ValidateICDPinter: function ($row) {
        var isValid = true;

        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var $childRow = EditableGrid.datatable.row($row).child();

        // ICD's
        var ICDPointers = $($row).find("input[id*='txtICDPointer']");
        ICDPointers.each(function (item) {
            var icdRef = $(this).val();
            var RefICD = self.find('#txtICD' + icdRef);
            if (RefICD && RefICD.length > 0 && $(this).val() != "" && RefICD.val() != "") {
                if ($(this).prev('input[id*=txtICDPointer]').length > 0 && $(this).prev('input[id*=txtICDPointer]').val() == "") {
                    isValid = false;
                    $(this).val('');
                    utility.DisplayMessages("Please enter previous ICD poineter.", 3);
                }
                else if ($(this).prev('input[id*=txtICDPointer]').length > 0 && $(this).prev('input[id*=txtICDPointer]').val() == $(this).val()) {
                    $(this).val('');
                    utility.DisplayMessages("ICD poineter already exists.", 3);
                }
                else {
                    if ($(this).val() != "" && icdRef != $(this).val()) {
                        $(this).css("border", "");
                    }
                }
            }
            else {
                if ($(this).val() != "" && RefICD.val() == "") {
                    $(this).val('');
                    /*$(this).css("border", "1px solid red");
                    isValid = false;
                    utility.DisplayMessages("Please enter valid ICD pointer.", 3);*/
                }
                else
                    if ($(ICDPointers[0]).val() == "" && self.find('#ddlSubmitStatus option:selected').text().toLowerCase() != "pending") {
                        $(this).css("border", "1px solid red");
                        isValid = false;
                        return false;
                    }
            }
        });
        return isValid;
    },

    validateModifiers: function ($row) {
        var isValid = true;
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var $childRow = EditableGrid.datatable.row($row).child();
        if (self.find('#ddlSubmitStatus option:selected').text().toLowerCase() == "pending")
            return isValid;
        // ICD's
        var Modifiers = $($row).find("input[id*='txtModifier']");
        Modifiers.each(function (item) {
            if ($(this).val() != "") {
                for (var i = 0; i < Modifiers.index($(this)) ; i++) {
                    if ($(Modifiers[i]).val() == "") {
                        $(Modifiers[i]).css("border", "1px solid red");
                        isValid = false;
                    }
                    else
                        $(Modifiers[i]).css("border", "");
                }
            }
            else {

            }
        });

        return isValid;
    },

    validateChargeAmounts: function (obj) {

        if ($(obj).find('input[id*="txtFEE"]').length > 0) {
            var fee = utility.convertToFigure($(obj).find('input[id*="txtFEE"]').val());
            var insCharges = utility.convertToFigure($(obj).find('input[id*="txtINSCharges"]').val());
            var patCharges = utility.convertToFigure($(obj).find('input[id*="txtPATCharges"]').val());

            if (fee > 0) {
                // return true;
                // EncounterChargeCapture.IsZeroFee = false;
            }
            else {
                //return false;
                EncounterChargeCapture.IsZeroFee = true;
            }
            if (insCharges >= 0) {
                //  return true
            }
            else {
                utility.DisplayMessages("insurance charges must be greater than Zero", 3);
                $(obj).focus();
                return false;
            }
            if (patCharges >= 0) {
                //return true;
            }
            else {
                utility.DisplayMessages("patient charges must be greater than Zero", 3);
                $(obj).focus();
                return false;
            }
        }
        return true;
    },


    OpenPatientReferral: function () {
        var params = [];
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];

        var self = "";
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }
        patientInsuranceID = "";
        if (EncounterChargeCapture.SelectedInsurancePlan) {
            patientInsuranceID = $(EncounterChargeCapture.SelectedInsurancePlan).attr('insuranceid')
        }
        var ProviderId = self.find("#hfProvider").val();
        var FacilityId = self.find("#hfFacility").val();
        var DOSFrom = self.find("#dtpDOSFrom").val();

        params["RefCtrlReferralNo"] = "txtReferralNumber";
        params["RefHiddenIdCtrl"] = "hfReferralNumerId";
        params["RefCtrlRefProvider"] = "toIgrogeRefProviderId";
        params["patientID"] = self.find("#frmEncounterChargeCapture #hfPatientId").val();
        params["patientInsuranceID"] = patientInsuranceID;
        params["ProviderId"] = ProviderId;
        params["FacilityId"] = FacilityId;
        params["ReferralDate"] = DOSFrom;

        LoadActionPan('patientReferralSearch', params);
    },

    OpenPatientReferralDetail: function (HiddenCtrl) {

        var params = [];
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];

        params["ReferralID"] = $('#' + EncounterChargeCapture.params["PanelID"] + ' #' + HiddenCtrl).val();
        params["patientID"] = $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfPatientId').val();
        params["mode"] = "Edit";
        LoadActionPan('Patient_Referral', params);

    },

    IsEncounterTabExist: function (TabID, ParentCtrl) {
        var breturn = false;
        if (LoadedEncounterTabs != null) {
            for (var i = 0; i < LoadedEncounterTabs.length; i++) {
                if (LoadedEncounterTabs[i].TabID == TabID && LoadedEncounterTabs[i].ParentCtrl == ParentCtrl) {
                    breturn = true;
                    break;
                }
            }
        }
        return breturn;
    },

    AddEncounterTabArray: function (TabID, ParentCtrl) {
        var EncounterArray = LoadedEncounterTabs;
        if (!EncounterChargeCapture.IsEncounterTabExist(TabID, ParentCtrl)) {
            var EncounterTab = new Object();
            EncounterTab["TabID"] = TabID;
            EncounterTab["ParentCtrl"] = ParentCtrl;
            EncounterTab["params"] = EncounterChargeCapture.params;
            EncounterArray.push(EncounterTab);
        }
        LoadedEncounterTabs = EncounterArray;
    },

    RemoveEncounterTabFromStore: function (TabID, ParentCtrl, TotalLoadedChargeCapture) {
        if (LoadedEncounterTabs != null) {
            var UpdatedArray = [];
            EncounterChargeCapture.SetChargeCaptureparmsFromLoadedTabs(TotalLoadedChargeCapture, TabID);
            UpdatedArray = jQuery.grep(LoadedEncounterTabs, function (value) {
                return value.TabID != TabID && value.ParentCtrl != ParentCtrl;
            });
            LoadedEncounterTabs = UpdatedArray;
        }
    },

    ValidatePOS: function ($row) {
        var isValid = true;
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        //Added by Azeem Raza Tayyab on 11-May-2016 to fix Bug#:PMS-5157
        var dtpAdmissionDate = self.find("#dtpAdmissionDate");
        if (self.find('#ddlSubmitStatus option:selected').text().toLowerCase() != "pending") {
            if ($row) {
                if ($($row).attr("id") && $($row).attr("id") != null) {
                    var posVal = $($row).find('input[id*="txtPOS"]').val();
                    if (posVal == "21") {
                        if (dtpAdmissionDate && dtpAdmissionDate.val() == "") {
                            isValid = false;
                        }
                        else {
                            isValid = true;
                        }
                    }
                }
            }
            else {
                self.find("#dgvVisitCharge tr").each(function () {
                    if ($(this).attr("id") && $(this).attr("id") != null) {
                        var posVal = $(this).find('input[id*="txtPOS"]').val();
                        if (posVal == "21") {
                            if (dtpAdmissionDate && dtpAdmissionDate.val() == "") {
                                isValid = false;
                            }
                            else {
                                isValid = true;
                            }
                        }
                    }
                });
            }
        }
        return isValid;
    },

    ValidateCPT: function ($row) {
        var isValid = true;
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        //Added by Azeem Raza Tayyab on 11-May-2016 to fix Bug#:PMS-5157
        var dtpDischargeDate = self.find("#dtpDischargeDate");
        if (self.find('#ddlSubmitStatus option:selected').text().toLowerCase() != "pending") {
            if ($row) {
                self.find("#dgvVisitCharge tr").each(function () {
                    if ($($row).attr("id") && $($row).attr("id") != null) {
                        var cptVal = $($row).find('input[id*="txtCPT"]').val();
                        if (cptVal == "99238" || cptVal == "99239") {
                            if (dtpDischargeDate && dtpDischargeDate.val() == "") {
                                isValid = false;
                            }
                            else {
                                isValid = true;
                            }
                        }
                    }
                });
            }
            else {
                self.find("#dgvVisitCharge tr").each(function () {
                    if ($(this).attr("id") && $(this).attr("id") != null) {
                        var cptVal = $(this).find('input[id*="txtCPT"]').val();
                        if (cptVal == "99238" || cptVal == "99239") {
                            if (dtpDischargeDate && dtpDischargeDate.val() == "") {
                                isValid = false;
                            }
                            else {
                                isValid = true;
                            }
                        }
                    }
                });
            }
        }

        return isValid;
    },

    DisableEditableCurrunRow: function (parentRow) {
        $(parentRow).find('input:text[id*="dtpDOSFrom"]').unbind("change");
        $(parentRow).find('button').prop('disabled', true);
        $(parentRow).find('input, textarea,  select').prop('disabled', true);
        var childRow = EncounterChargeCapture.EditableGrid.datatable.row(parentRow).child();
        $(childRow).find('button').prop('disabled', true);


        $(parentRow).find('a:eq(1),a:eq(4),a:eq(5),input, textarea,  select').prop('disabled', true);

        var childRow = EncounterChargeCapture.EditableGrid.datatable.row(parentRow).child();
        $(childRow).find('input, textarea,  select').prop('disabled', true);
        var TotalBalance = parseFloat($(parentRow).find('input:hidden[id*="hfTotalBalance"]').val());
        $(parentRow).find('input:text[id*="txtTotalFEE"]').attr('disabled', true);
        $(parentRow).find('input:text[id*="txtExpectedFEE"]').attr('disabled', true);
        if (TotalBalance != null && TotalBalance != "" && TotalBalance > 0) {

            var CPTCtrl = $(parentRow).find('input:text[id*="txtCPT"]');
            $(CPTCtrl).next().find('button').prop('disabled', true);

            $(CPTCtrl).prop('disabled', true);

            $(parentRow).find('input:text[id*="txtTotalFEE"]').attr('disabled', true);

            $(childRow).find('input[id*="txtPriorAuthorization"]').attr('disabled', true);
            $(childRow).find('input[id*="txtNDC"]').attr('disabled', true);
            $(childRow).find('input[id*="txtNDCUnit"]').attr('disabled', true);
            $(childRow).find('input[id*="txtNDCUnitPrice"]').attr('disabled', true);
            $(childRow).find('select[id*="ddlNDCMeasurement"]').attr('disabled', true);

        }
    },

    EnableEditableRow: function (parentRow) {
        $(parentRow).find('input:text[id*="dtpDOSFrom"]').bind("change");
        $(parentRow).find('button').prop('disabled', false);
        $(parentRow).find('input, textarea,  select').prop('disabled', false);
        var childRow = EncounterChargeCapture.EditableGrid.datatable.row(parentRow).child();
        $(childRow).find('button').prop('disabled', false);
        $(parentRow).find('a:eq(1),a:eq(4),a:eq(5),input, textarea,  select').prop('disabled', false);
        $(childRow).find('input, textarea,  select').prop('disabled', false);
        $(childRow).find('select[id*="ddlNDCMeasurement"]').attr('disabled', false);
        $(parentRow).find('input:text[id*="txtTotalFEE"]').attr('disabled', true);
    },

    BilledToPatient: function (obj) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var Msg = "";
        var BillToPat = $(obj).is(':checked');
        var ChargesIds;
        var arrRowId = [];
        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tr").each(function () {
            if ($(this).attr("id") && $(this).attr("id") != null && $(this).attr("id") > 0) {
                //to make sure that ids of child rows are not being pushed as we only need parent rows
                if ($(this).attr("id").indexOf('Child') < 0)
                    arrRowId.push($(this).attr("id"));
            }
        });
        var strRowIds = arrRowId.join(",");
        if (BillToPat) {
            Msg = "Do you want to charge bill to patient?";
            utility.myConfirm(Msg, function () {
                if (strRowIds) {
                    EncounterChargeCapture.BilledToPatientCharges(BillToPat, strRowIds).done(function (response) {
                        if (response.status != false) {
                            var visit_detail = JSON.parse(response.ChargesInsBalLoad_JSON);
                            $.each(visit_detail, function (i, item) {
                                var $row = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tr[id=" + item.ChargeCapId + "]");
                                var PatCharges = parseFloat(item.PatCharges);
                                var INSCharges = parseFloat(item.InsCharges);
                                var InsBalance = parseFloat(item.InsBalance);
                                var PatBalance = parseFloat(item.PatBalance);
                                if (BillToPat) {
                                    /* INSCharges = parseFloat($row.find('input[id*="txtINSCharges"]').val() != "" ? $row.find('input[id*="txtINSCharges"]').val() : "0");
                                     PatCharges = parseFloat($row.find('input[id*="txtPATCharges"]').val() != "" ? $row.find('input[id*="txtPATCharges"]').val() : "0");*/
                                    $row.find('input[id*="txtINSCharges"]').val(parseFloat(INSCharges - InsBalance).toFixed(globalAppdata.DecimalPlaces));
                                    var copayCharge = $row.find('input[id*="txtCOPAY"]').val();

                                    var totalCharge = Number(INSCharges) + Number(PatCharges) + Number(copayCharge);
                                    var fee = $row.find('input[id*="txtFEE"]').val();
                                    if (totalCharge > fee) {
                                        $row.find('input[id*="txtPATCharges"]').val(parseFloat((PatCharges + InsBalance) - copayCharge).toFixed(globalAppdata.DecimalPlaces));
                                    }
                                    else {
                                        $row.find('input[id*="txtPATCharges"]').val(parseFloat(PatCharges + InsBalance).toFixed(globalAppdata.DecimalPlaces));
                                    }

                                    self.find('#chkBillToPatient').prop('checked', true);
                                    self.find('#ddlSubmitStatus option').filter(function () {
                                        return $.trim($(this).text().toLowerCase()) == "patient"
                                    }).prop('selected', true);
                                }
                                else {
                                    $row.find('input[id*="txtINSCharges"]').val(parseFloat(INSCharges).toFixed(globalAppdata.DecimalPlaces));
                                    $row.find('input[id*="txtPATCharges"]').val(parseFloat(PatCharges).toFixed(globalAppdata.DecimalPlaces));
                                    if (EncounterChargeCapture.params["SubmitStatus"]) {
                                        //select submit status as actual status
                                        self.find('#ddlSubmitStatus option').filter(function () {
                                            return $.trim($(this).text().toLowerCase()) == EncounterChargeCapture.params["SubmitStatus"].toLowerCase()
                                        }).prop('selected', true);
                                    }
                                }
                            });
                            //  utility.DisplayMessages(response.Message, 1);
                        }
                        else {
                            if (response.Message && response.Message != "") {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        }
                    });
                }
                //For handling new added Line Item:
                $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tr").each(function () {
                    if ($(this).attr("id") && $(this).attr("id") != null && $(this).attr("id") < 0) {
                        var $row = $(this);
                        INSCharges = parseFloat($row.find('input[id*="txtINSCharges"]').val() != "" ? $row.find('input[id*="txtINSCharges"]').val() : "0");
                        PatCharges = parseFloat($row.find('input[id*="txtPATCharges"]').val() != "" ? $row.find('input[id*="txtPATCharges"]').val() : "0");
                        if (BillToPat) {
                            $row.find('input[id*="txtINSCharges"]').val(parseFloat(0).toFixed(globalAppdata.DecimalPlaces));
                            $row.find('input[id*="txtPATCharges"]').val(parseFloat(PatCharges + INSCharges).toFixed(globalAppdata.DecimalPlaces));
                        }
                        else {
                            $row.find('input[id*="txtINSCharges"]').val(parseFloat(INSCharges + PatCharges).toFixed(globalAppdata.DecimalPlaces));
                            $row.find('input[id*="txtPATCharges"]').val(parseFloat(0).toFixed(globalAppdata.DecimalPlaces));
                        }
                    }
                });

            }, function () {
                if (BillToPat) {
                    $(obj).prop('checked', false)
                    if (EncounterChargeCapture.SelectedInsurancePlan) {
                        EncounterChargeCapture.BindSelectedInsurancePlan($(EncounterChargeCapture.SelectedInsurancePlan).attr("insuranceid"));
                    }
                }
                else {
                    $(obj).prop('checked', true)
                }
                //EncounterChargeCapture.CheckUnChekInsPlans(obj);
                return false;
            }, 'Confirmation Alert');
        } else {
            $(obj).prop("checked", true);
            return;
            //Msg = "Do you want to charge bill to insurance";
        }
    },

    BilledToPatientCharges: function (IsBillToPatient, strRowIds) {
        var data = "ChargesIds=" + strRowIds + "&BillToPatient=" + IsBillToPatient;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "BILL_TO_PATIENT_CHARGES");
    },

    OverrideChargeValidation: function ($row) {

        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var $childRow = EditableGrid.datatable.row($row).child();

        if (self.find('#ddlSubmitStatus option:selected').text().toLowerCase() == "pending") {

            $row.find('input,select').each(function () {

                if ($(this).attr("isoptional") && $(this).attr("isoptional") == "0" && $(this).attr("id").indexOf("txtCPT") < 0)
                    $(this).attr("isoptional", "24");
            });

            if ($childRow)
                $childRow.find('input,select').each(function () {

                    if ($(this).attr("isoptional") && $(this).attr("isoptional") == "0" && $(this).attr("id").indexOf("txtCPT") < 0)
                        $(this).attr("isoptional", "24");
                });
        }
        else {

            $row.find('input,select').each(function () {

                if ($(this).attr("isoptional") && $(this).attr("isoptional") == "24" && $(this).attr("id").indexOf("txtCPT") < 0)
                    $(this).attr("isoptional", "0");
            });

            if ($childRow)
                $childRow.find('input,select').each(function () {

                    if ($(this).attr("isoptional") && $(this).attr("isoptional") == "24" && $(this).attr("id").indexOf("txtCPT") < 0)
                        $(this).attr("isoptional", "0");
                });
        }

    },

    InitilizeToggle: function () {
        var self;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        $(self.find($('[data-plugin-toggle]'))).each(function () {
            var $this = $(this),
                opts = {};
            var pluginOptions = $this.data('plugin-options');
            if (pluginOptions)
                opts = pluginOptions;
            $this.themePluginToggle(opts);
        });
    },

    BindSelfPayToBillToPatient: function (isfromVisitLoad) {

        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }
        if (EncounterChargeCapture.IsSelfPay == "True") {
            self.find('#chkBillToPatient').prop('checked', true);
            if (self.find('input[id^="txtINSCharges"]')) {
                self.find('input[id^="txtINSCharges"]').prop('disabled', true);
            }
            //self.find("#dgvPatientInsurances").find("input[type='checkbox']").prop('checked', false);
            //self.find("#dgvPatientInsurances").find("input[type='checkbox']").prop('disabled', true);
            self.find("#lstVisitPlans").find("input[type='checkbox']").prop('checked', false);
            self.find("#lstVisitPlans").find("input[type='checkbox']").prop('disabled', true);
            //self.find('#chkBillToPatient').prop('disabled', true);
            self.find('input[id^="txtCOPAY"]').prop('disabled', true);
        }
        else {
            //self.find("#dgvPatientInsurances").find("input[type='checkbox']").prop('disabled', false);
              self.find("#lstVisitPlans").find("input[type='checkbox']").prop('disabled', true);
            if (self.find("#chkBillToPatient").is(':checked')) {
                self.find('#chkBillToPatient').prop('checked', true);
                //commented line of code create problem from paymentposting flow.option not selected in that case -pms-1631
                //  $("#ddlSubmitStatus option:contains('Patient')").attr('selected', 'selected');
                if (!isfromVisitLoad) {
                    var selectedoption = $("#ddlSubmitStatus option:contains('Patient')").val();
                    $("#ddlSubmitStatus").val(selectedoption);
                }
            }
            else {
                self.find('#chkBillToPatient').prop('checked', false);
            }
            self.find('input[id^="txtCOPAY"]').prop('disabled', false);
            self.find('input[id^="txtINSCharges"]').prop('disabled', false);

        }

        //Lock Claim Fields
        EncounterChargeCapture.LockClaim();
        //EncounterChargeCapture.SelectPrimaryInsurance();
        //EncounterChargeCapture.FillReferralNumber();
    },

    CheckSelfPay: function (isfromVisitLoad) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var accountNo = self.find("#txtPatientName").val();
        //var AllPatients = utility.GetPatientArray(accountNo, 0).done(function (response) {
        // $.each(response, function (i, item) {
        //if (item.AccountNumber == accountNo) {
        var selfPay = $("#hfSelfPay").val();
        EncounterChargeCapture.IsSelfPay = selfPay == "" ? "False" : selfPay;
        EncounterChargeCapture.BindSelfPayToBillToPatient(isfromVisitLoad);
        if (EncounterChargeCapture.IsSelfPay == "True") {
            self.find("#divPatientInsurancePlan").parent('div').css('display', 'none');
            self.find('#chkBillToPatient').prop('checked', true);
            self.find('#txtVisitCopayment').val(Number(0).toFixed(globalAppdata.DecimalPlaces));
        }
        else {
            self.find("#divPatientInsurancePlan").parent('div').css('display', 'block');
            if (self.find("#chkBillToPatient").is(':checked')) {
                self.find('#chkBillToPatient').prop('checked', true);
                //  $("#ddlSubmitStatus option:contains('Patient')").attr('selected', 'selected');
            }
            else {
                self.find('#chkBillToPatient').prop('checked', false);
            }
        }
        // }
        //});
        // });
    },
    LockClaim: function (isLocked) {

        if ((isLocked == null || isLocked == undefined || isLocked == "") && EncounterChargeCapture.params["IsLocked"] != "")
            isLocked = EncounterChargeCapture.params.IsLocked;

        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find("input:text,input:checkbox,input:radio,select,button,textarea").prop('disabled', isLocked);
        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #lstVisitPlans").find("input:text,input:checkbox").prop('disabled', false);
        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #chkBillToPatient").prop('disabled', false);

        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #ddlTablist").prop("disabled", false);
        $("#" + EncounterChargeCapture.params.PanelID + " #dgvPatientOutstanding").find("input:text,textarea").prop('disabled', true);

        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tr").each(function (i, row) {
            if ($(this).attr("id") && $(this).attr("id") != null) {
                if ($(this).attr('chargestatus') && $(this).attr('chargestatus').toLowerCase() == 'submitted') {
                    var childRow = EncounterChargeCapture.EditableGrid.datatable.row(this).child();
                    if (this)
                        $(this).find('a:eq(1),a:eq(2),a:eq(4),a:eq(5),input, textarea,  select,button').prop('disabled', true);
                    $(this).find('input[id*="chkActive"]').prop('disabled', false);
                    if (childRow)
                        $(childRow).find('input, textarea,select,button').prop('disabled', true);
                }
                else {
                    var childRow = EncounterChargeCapture.EditableGrid.datatable.row(this).child();
                    if (this) {
                        $(this).find('a:eq(1),a:eq(2),a:eq(4),a:eq(5),input, textarea,  select,button').prop('disabled', isLocked);
                    }
                    if (childRow) {
                        $(childRow).find('input, textarea,select,button').prop('disabled', isLocked);
                    }
                    //Begin Changes done by Azeem Raza Tayyab on 5-May-2016 related to Mailed Changes
                    if (isLocked && EncounterChargeCapture.IsVNC == null) {
                        //Enable ICDCode and Modifier for locked charge when charge status is not submitted
                        $(this).find('input[id*="txtPOS"]').prop('disabled', false);
                        $(this).find('input[id*="txtModifier"]').prop('disabled', false);
                        $(this).find('input[id*="txtModifier"]').next().find('.btn').prop('disabled', false);
                        $(this).find('a:eq(1)').prop('disabled', false);
                        $(this).find('input[id*="chkActive"]').prop('disabled', false);
                        if (EncounterChargeCapture.params.SubmitStatus != "Submitted") {
                            $(this).find('input[id*="txtICDPointer"]').prop('disabled', false);
                            $(this).find('select[id*="ddlNDCMeasurement"]').prop('disabled', false);
                            $(this).find('input[id*="txtPriorAuthorization"]').prop('disabled', false);
                            $(this).find('input[id*="txtExpectedFee"]').prop('disabled', false);
                            $(this).find('textarea[id*="txtComments"]').prop('disabled', false);
                            $(this).find('input[id*="txtCLIA"]').prop('disabled', false);
                            $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find('input[id="txtReferralNumber"]').prop('disabled', false);
                            $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find('button[id="lnkReferralNumber"]').prop('disabled', false);
                            $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find('input[id="txtPriorAuthNumber"]').prop('disabled', false);
                            $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find('button[id="lnkPAN"]').prop('disabled', false);
                            $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv").find('input,select,textarea,button').prop('disabled', false)
                            $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find("input[id=txtICD1]").not("input[id*=txtICD10Description]").prop('disabled', false);
                        }
                        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find('input[id="dtpHoldTill"]').prop('disabled', false);
                        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find('input[id="chkIsCleanclaim"]').prop('disabled', false);
                        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find('input[id="chkInCollections"]').prop('disabled', false);
                    }
                    //End Changes done by Azeem Raza Tayyab on 5-May-2016 related to Mailed Changes
                }
            }
        });

        //disabling add new item link
        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #pnlVisitCharge_Result a").filter(function () {
            if ($(this).text().toLowerCase() === 'add line item') {
                if (isLocked)
                    $(this).addClass("disableAll")
                else {
                    $(this).removeClass("disableAll");
                    $(this).removeAttr("disabled");
                }

            }

        });

        //allow change submit status
        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find("#ddlSubmitStatus").prop('disabled', false);
        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #SaveVisit").prop('disabled', false);

        if (isLocked == false) {
           $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find(
                "#txtPatientFullName,#txtClaimNumber,#dtpAppointmentDate,#dtpEncounterSignOffDate,#dtpSubmittedDate,#txtSubmittedBy,#chkPaid,#dtpClaimDate,#txtDOB,#txtAge,#txtSex"
                ).prop('disabled', true);
           // $('#dgvVisitCharge_wrapper input[id*="txtdrugCodeCost"]').prop("disabled", true)
            $('#' + EncounterChargeCapture.params["PanelID"] + ' input[id*="txtTotalFEE"]').prop('disabled', true);
            if (EncounterChargeCapture.params.mode = 'Edit')
                $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture").find("#txtPatientName").prop('disabled', true);

        }
        //Enable Claim Document close button
        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #CloseCD").prop('disabled', false);

        //Begin Changes done by Azeem Raza Tayyab on 5-May-2016 related to Mailed Changes
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        if (isLocked && EncounterChargeCapture.IsVNC == null) {
            //Enable Referring Provider
            self.find("#frmEncounterChargeCapture #txtRefProvider").prop('disabled', false);
            self.find("#frmEncounterChargeCapture #lnkRefProvider").prop('disabled', false);
            //Enabale claim CommentsdtpLastSeenDate
            self.find("#frmEncounterChargeCapture #txtClaimComments").prop('disabled', false);
            self.find("#frmEncounterChargeCapture #dtpLastSeenDate").prop('disabled', false);
        }
        //End Changes done by Azeem Raza Tayyab on 5-May-2016 related to Mailed Changes
        if (isLocked == false) {
            self.find("#txtICD1").prop("disabled", false);
            self.find("#btnICD1").prop("disabled", false);
            EncounterChargeCapture.EnableDisableICDs();
        }
        else if (isLocked == true) {
            //self.find("input[id^=txtICD]").prop("disabled", true);
            //self.find("button[id^=btnICD]").prop("disabled", true);
        }
        self.find("input[id^=txtICD10Description]").prop("disabled", true);

        //console.log("calling..."+ isLocked);
        if (isLocked) {

            $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #insuranceandpayments").find("input:checkbox").prop('disabled', false);
            if (EncounterChargeCapture.params.SubmitStatus != "Submitted" && EncounterChargeCapture.params.SubmitStatus != "Clearinghouse Rejection") {
                $("#" + EncounterChargeCapture.params.PanelID + " #aReSubmit").addClass("disableAll");
                EncounterChargeCapture.EnableDisableICDs();
            }
            else {
                $("#" + EncounterChargeCapture.params.PanelID + " #aReSubmit").removeClass("disableAll");
            }
        }

        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #txtNoteComments").prop('disabled', false);
        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #txtClaimNumber").prop('disabled', false);
        $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #btnsync").prop('disabled', false);
        $('#' + EncounterChargeCapture.params.PanelID + ' #txtNoteCommentsLoad').attr("disabled", "disabled");
    },

    LoadClaimDetails: function (VisitId, PatientId) {
        EncounterChargeCapture.params["VisitId"] = VisitId;
        EncounterChargeCapture.params["patientID"] = PatientId;

        EncounterChargeCapture.params["IsReload"] = true;
        EncounterChargeCapture.Load(EncounterChargeCapture.params);
        $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #divChargeDetails').toggleClass("col-lg-10");
    },

    CopayTypeConfirmation: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var Option = null;
        if (EncounterChargeCapture.SelectedInsurancePlan) {
            Option = EncounterChargeCapture.SelectedInsurancePlan;
        }

        var ProviderId = self.find('#hfProvider').val();
        var ProviderId = ProviderId != "" ? parseInt(ProviderId) : 0;
        if (ProviderId > 0 && EncounterChargeCapture.PrevisosProvider != ProviderId) {
            if (EncounterChargeCapture.IsCallChange == true) {
                EncounterChargeCapture.PrevisosProvider = ProviderId;
                Admin_Provider.SearchProvider('', ProviderId).done(function (response) {
                    if (response.status != false) {
                        if (response.ProviderCount > 0) {
                            var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);


                            if (EncounterChargeCapture.params.mode.toLowerCase() == "edit") {
                                if ((self.find('#radSpecialist').is(':checked') && ProviderLoadJSONData[0].IsSpecialist == "False") || (self.find('#radPCP').is(':checked') && ProviderLoadJSONData[0].IsSpecialist == "True")) {
                                    utility.myConfirm('Are you sure you want to change the copay type ?', function () {
                                        if (ProviderLoadJSONData[0].IsSpecialist == "False") {
                                            if (Option && $(Option).attr("AmtCopay") && EncounterChargeCapture.IsSelfPay == "False")
                                                self.find('#txtVisitCopayment').val(utility.convertToFigure($(Option).attr("AmtCopay")));
                                            self.find('#radPCP').trigger("click");
                                        }
                                        else {
                                            if (Option && $(Option).attr("SpecialistCopay") && EncounterChargeCapture.IsSelfPay == "False")
                                                self.find('#txtVisitCopayment').val(utility.convertToFigure($(Option).attr("SpecialistCopay")));
                                            self.find('#radSpecialist').trigger("click");
                                        }
                                        EncounterChargeCapture.SetChargeVisitCopay();
                                    }, function () {
                                        if (self.find('#radPCP').is(':checked')) {
                                            if (Option && $(Option).attr("AmtCopay") && EncounterChargeCapture.IsSelfPay == "False") {
                                                self.find('#txtVisitCopayment').val(utility.convertToFigure($(Option).attr("AmtCopay")));
                                            }
                                        } else if (self.find('#radSpecialist').is(':checked') && EncounterChargeCapture.IsSelfPay == "False") {
                                            if (Option && $(Option).attr("SpecialistCopay"))
                                                self.find('#txtVisitCopayment').val(utility.convertToFigure($(Option).attr("SpecialistCopay")));
                                        }
                                        EncounterChargeCapture.SetChargeVisitCopay();
                                    }, 'Confirm Copay Type Change');
                                }
                            }
                        }
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else {
                EncounterChargeCapture.IsCallChange = true;
            }
        }
    },

    CheckChargesActiveStatus: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var IsActive = false;
        self.find("#dgvVisitCharge tr").each(function (i, row) {
            if ($(this).attr("id") && $(this).attr("id") != null) {
                //var childRow = EncounterChargeCapture.EditableGrid.datatable.row(this).child();
                var chkActive = $(this).find("input[id^=chkActive]"); {
                    if ($(chkActive).is(':checked')) {
                        IsActive = true;
                        return true;
                    }
                }
            }
        });
        return IsActive;
    },

    UpdateSubmitStatusParams: function (myJSON) {
        var visit_detail = JSON.parse(myJSON);
        if (visit_detail.ddlPatientInsurance == "") {
            $('#' + EncounterChargeCapture.params.PanelID + ' #aPrintClaim').addClass('disableAll');
        }
        else {
            $('#' + EncounterChargeCapture.params.PanelID + ' #aPrintClaim').removeClass('disableAll');
        }
        EncounterChargeCapture.params["SubmitStatus"] = visit_detail.ddlSubmitStatus_text;
    },

    CheckDuplicateCPT: function (CurrentCPT) {
        IsDuplicate = false;
        var CurrentRow = $(CurrentCPT).closest('tr');
        var CurrentDOSFrom = $(CurrentRow).find("input[id*=dtpDOSFrom]");
        var CurrentDOSTo = $(CurrentRow).find("input[id*=dtpDOSTo]");

        $('input[id^="txtCPT"]').each(function () {
            var thisRow = $(this).closest('tr');
            if ($(CurrentRow).attr("id") && $(thisRow).attr("id") && $(CurrentRow).attr("id") != $(thisRow).attr("id")) {
                var ThisDOSFrom = $(CurrentRow).find("input[id*=dtpDOSFrom]");
                var ThisDOSTo = $(CurrentRow).find("input[id*=dtpDOSTo]");
                if (ThisDOSFrom.val() == CurrentDOSFrom.val() && ThisDOSTo.val() == CurrentDOSTo.val() && $(this).val() == $(CurrentCPT).val()) {
                    IsDuplicate = true;
                }
            }
        });

        return IsDuplicate;
    },


    /* Begin Added By Azeem Raza Tayyab on 14-Apr-2016 for Changes Related to Bug#:PMS-1522*/
    ClaimDocumentsLoad: function () {
        var actionPan = $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #pnlClaimDocuments');
        $(actionPan).find('#docModelDialog').addClass('modal-dialog modal-dialog-lg');
        $(actionPan).find('#docModel').addClass('modal-content');
        $(docHeader).show();
        $(actionPan).modal({
            show: 'true',
            backdrop: 'static',
            keyboard: false

        }).on('hidden.bs.modal', function () {
            if ($('body').find('.modal-backdrop').length > 0) {
                $('body').addClass('modal-open');
            }
            else
                $('body').removeClass('modal-open');

        });
        EncounterChargeCapture.ClaimDocumentsSearch();
    },

    ClaimDocumentsSearch: function () {
        if (EncounterChargeCapture.params["VisitId"] && EncounterChargeCapture.params["VisitId"] > 0) {
            var VisitId = EncounterChargeCapture.params["VisitId"];
            EncounterChargeCapture.SearchClaimDocuments(EncounterChargeCapture.params["VisitId"]).done(function (response) {
                if (response.status != false) {
                    EncounterChargeCapture.DocumentGridLoad(response);
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    SearchClaimDocuments: function (VisitID) {
        var data = "VisitID=" + VisitID;
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "CLAIM_DOCUMENTS");
    },

    DocumentGridLoad: function (response) {
        $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #pnlClaimDocuments #ContainerClaimDocuments #dgvClaimDocuments').dataTable().fnDestroy();

        $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #pnlClaimDocuments #ContainerClaimDocuments #dgvClaimDocuments tbody').find("tr").remove();

        if (response.ClaimChargeDocumentCount > 0) {
            var DocumentLoad_JSONData = JSON.parse(response.ClaimChargeDocumentLoad_JSON);
            $.each(DocumentLoad_JSONData, function (i, item) {
                var DemographicsMethod = "utility.PatientDemographics('" + item.PatientId + "', 'EncounterChargeCapture', event);";
                var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + item.PatientId + "', 'EncounterChargeCapture', event, true);";
                var $row = $('<tr/>');
                $row.attr("onclick", "utility.SelectGridRow($('#dgvClaimDocuments_row" + item.BatchDocId + "'))");
                $row.attr("id", "dgvClaimDocuments_row" + item.BatchDocId);
                $row.attr("BatchDocId", item.BatchDocId);
                $row.attr("BatchId", item.BatchId);

                if (item.BatchId) {
                    var action = '<td><a class="btn  btn-xs" href="#" onclick="EncounterChargeCapture.BatchChargeDocumentDelete(' + item.BatchDocId + ');" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="EncounterChargeCapture.BatchChargeDocumentEdit(' + item.BatchDocId + ',' + item.BatchId + ',' + item.BatchNumber + ' );"   title="Edit Record"><i class="fa fa-edit black"></i></a></td>';
                    $row.append('' + action + '<td patientId="' + item.PatientId + '"><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + "</td><td>" + item.FolderName + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title=' + item.FilePath + ' >' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');
                }
                else {
                    var action = '<td><a class="btn  btn-xs" href="#" onclick="EncounterChargeCapture.PatientDocumentDelete(' + item.BatchDocId + ',event);" title="Delete Record"><i class="fa fa-close red"></i></a>&nbsp;<a class="btn btn-xs" href="#" onclick="EncounterChargeCapture.PatientDocumentEdit(' + item.PatientId + ',' + item.BatchDocId + ',event );"   title="Edit Record"><i class="fa fa-edit black"></i></a></td>';
                    $row.append('' + action + '<td patientId="' + item.PatientId + '"><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + item.AccountNumber + '</a></td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + "</td><td>" + item.FolderName + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title=' + item.FilePath + ' >' + item.FilePath + '</td><td>' + item.FileType + '</td><td>' + item.Pages + '</td><td class="ellip150" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');
                }


                $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #pnlClaimDocuments #ContainerClaimDocuments #dgvClaimDocuments tbody').last().append($row);

            });
        }
        else {
            $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #pnlClaimDocuments #ContainerClaimDocuments #dgvClaimDocuments').DataTable({
                "language": {
                    "emptyTable": "No Documents Found for this Claim "
                }, "autoWidth": false, "bLengthChange": false,"searching":false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });


        }
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

        if ($.fn.dataTable.isDataTable('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #pnlClaimDocuments #ContainerClaimDocuments #dgvClaimDocuments'));
        else
            $('#' + EncounterChargeCapture.params.PanelID + '  #frmEncounterChargeCapture #pnlClaimDocuments #ContainerClaimDocuments #dgvClaimDocuments').DataTable({ "bLengthChange": false, "autoWidth": false, "searching":false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

    },

    UnLoadClaimDocument: function () {
        if ($('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #pnlClaimDocuments #ContainerClaimDocuments #dgvClaimDocuments tbody > tr:eq(0)').children().length>1) {
            if (!$('#' + EncounterChargeCapture.params.PanelID + ' #aClaimDocuments #DocAttached').hasClass('fa-paperclip'))
            { $('#' + EncounterChargeCapture.params.PanelID + ' #aClaimDocuments #DocAttached').addClass('fa-paperclip'); }
        }
        else {
            if ($('#' + EncounterChargeCapture.params.PanelID + ' #aClaimDocuments #DocAttached').hasClass('fa-paperclip'))
            { $('#' + EncounterChargeCapture.params.PanelID + ' #aClaimDocuments #DocAttached').removeClass('fa-paperclip'); }
        }
        var actionPan = $('#' + EncounterChargeCapture.params.PanelID + ' #frmEncounterChargeCapture #pnlClaimDocuments');
        actionPan.modal('hide');
        
    },

    BatchChargeDocumentEdit: function (BatChDocId, BatchId, BatchNumber) {
        AppPrivileges.GetFormPrivileges("Charge Batch", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["BatchDocId"] = BatChDocId;
                params["BatchId"] = BatchId;
                params["BatchNumber"] = BatchNumber;
                params["ParentCtrl"] = "EncounterChargeCapture";
                LoadActionPan('ChargeBatch_Viewer', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    PatientDocumentEdit: function (PatientID, PatDocID, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Documents", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["PatientID"] = PatientID;
                params["PatDocID"] = PatDocID;
                params["mode"] = "Edit";
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = "EncounterChargeCapture";
                LoadActionPan('Document_Viewer', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    BatchChargeDocumentDelete: function (BatChDocId) {
        utility.myConfirm('1', function () {
            var selectedValue = BatChDocId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                        chargeBatchDetail.DeleteBatchChargeDocument(BatChDocId).done(function (response) {
                            if (response.status == true) {
                                EncounterChargeCapture.ClaimDocumentsSearch();
                                utility.DisplayMessages(response.Message, 1);
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
            }
        }, function () { });
    },

    DeletePatientDocument: function (DocumentID) {
        var data = "DocumentID=" + DocumentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_DOCUMENT", "DEATTACH_CLAIM_DOCUMENT");
    },

    PatientDocumentDelete: function (BatChDocId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        utility.myConfirm('1', function () {
            var selectedValue = BatChDocId;
            if (selectedValue == "" || selectedValue == "undefined") {
            }
            else {
                EncounterChargeCapture.DeletePatientDocument(BatChDocId).done(function (response) {
                    if (response.status == true) {
                        EncounterChargeCapture.ClaimDocumentsSearch();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            }
        }, function () { });
    },

    OpenEncounter: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        if (self.find('#frmEncounterChargeCapture #hfPatientId') && self.find('#txtPatientName') && self.find('#frmEncounterChargeCapture #hfPatientId').val() != "") {
            var params = [];
            params["FromAdmin"] = 0;
            params["ParentCtrl"] = 'EncounterChargeCapture';
            params["PatientAccountNo"] = self.find('#txtPatientName').val();
            params["patientID"] = self.find('#frmEncounterChargeCapture #hfPatientId').val();

            LoadActionPan('Encounter_Visits', params);
        }
    },

    ValidateReferralNumber: function ($obj) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }
        var isInitilized = $($obj).data('ui-autocomplete') != undefined;
        if ($($obj).val() != "" && isInitilized) {
            var sourceArr = $($obj).autocomplete("option", "source");
            var haveObject = sourceArr.filter(function (obj) {
                var IsValid = obj.value.toLowerCase() == $($obj).val().toLowerCase();
                return IsValid;
            });
            if (haveObject.length == 0) {
                self.find("#lnkReferralNumberEdit").addClass("hidden");
                self.find("#lblReferralNumber").removeClass("hidden");
                self.find("#hfReferralNumerId").val("");
            }
            else {
                self.find("#lnkReferralNumberEdit").removeClass('hidden');
                self.find("#lblReferralNumber").addClass("hidden");
                if (haveObject[0]["id"])
                    self.find("#hfReferralNumerId").val(haveObject[0]["id"]);
            }
        }
        else {
            self.find("#lnkReferralNumberEdit").addClass('hidden');
            self.find("#lblReferralNumber").removeClass('hidden');
            self.find("#hfReferralNumerId").val("");
        }
    },


    LoadInsuranceGrid: function (response, isFromSyncBtn) {
        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvPatientInsurances").dataTable().fnDestroy();
        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvPatientInsurances tbody").find("tr").remove();
        $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #lstVisitPlans').empty();
        $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #divVisitPlans').css("display", "");
        $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #chkIsCleanclaim').prop("disabled", false);
        
        if (!$("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #divChargeDetails').hasClass("col-lg-10")) {
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #divChargeDetails').addClass("col-lg-10")
        }
        if (response.PatientInsuranceCount > 0) {

            var PatientInsuranceJSONData = JSON.parse(response.PatientInsuranceLoad_JSON);
            EncounterChargeCapture.params.PlanResponse = response;
            $.each(PatientInsuranceJSONData, function (i, item) {
                var $row = $('<tr/>');
                $row.attr("PlanPriority", item.PlanPriority);
                $row.attr("AmtCopay", item.AmtCopay);
                $row.attr("SpecialistCopay", item.SpecialistCopay);
                $row.attr("AssignBenefits", item.AssignBenefits);
                $row.attr("Box24BShaded", item.Box24BShaded);
                $row.attr("Box24IJShaded", item.Box24IJShaded);
                $row.attr("IsActive", item.IsActive);
                if (item.IsActive != "True") {
                    $row.css("color", "red");
                }
                $row.attr("IsEmployed", item.EmployerName != "" ? "True" : "False");
                $row.attr("InsuranceId", item.InsuranceId);
                $row.attr("InsurancePlanId", item.InsurancePlanId);
                $row.attr("claimtypeid", item.ClaimTypeId);
                $row.attr("claimtypename", item.ClaimTypeName);
                $row.attr("ediclearinghouseid", item.EDIClearingHouseId);
                $row.attr("edisubmitinsuranceid", item.EDISubmitInsuranceId);
                $row.attr("electronicsubmit", item.ElectronicSubmit);
                $row.attr("InsurancePlanIsSplitted", item.InsurancePlanIsSplitted);
                $row.attr("SplittedInsurancePlanName", item.SplittedInsurancePlanName);
                $row.attr("SplittedInsurancePlanId", item.SplittedInsurancePlanId);
                $row.attr("IsReportNPI", item.IsReportNPI);
                var InsuredName = item.LastName + ', ' + item.FirstName + ' ' + item.MI
                var ShowSplittedInsuranceName = false;
                if (EncounterChargeCapture.IsSplittedClaim == true && item.InsurancePlanIsSplitted.toLowerCase() == 'true' && Number(item.SplittedInsurancePlanId) > 0) {
                    ShowSplittedInsuranceName = true;
                    $row.append('<td style="display:none;">' + item.InsuranceId + '</td><td style="display:none;"><input type="checkbox"  onclick="EncounterChargeCapture.CheckedInsurance(this,event)" name="checkbox" id="' + item.InsuranceId + '"></td><td style="display:none;">' + item.PlanPriority + '</td> <td>' + utility.getpreferenceByPriority(item.PlanPriority) /*item.PlanPriority*/ + '</td> <td><a href="#" onclick="EncounterChargeCapture.OpenInsurancePlan(' + item.SplittedInsurancePlanId + ',event);" title="View Visit Detail">' + item.SplittedInsurancePlanName + '</a></td><td>' + item.State + '</td><td>' + item.SubscriberId + '</td><td>' + item.RelationName + '</td><td>' + InsuredName + '</td><td>' + item.GroupId + '</td>');
                }
                else
                    $row.append('<td style="display:none;">' + item.InsuranceId + '</td><td style="display:none;"><input type="checkbox" onclick="EncounterChargeCapture.CheckedInsurance(this,event)" name="checkbox" id="' + item.InsuranceId + '"></td><td style="display:none;">' + item.PlanPriority + '</td>  <td>' + utility.getpreferenceByPriority(item.PlanPriority) /*item.PlanPriority*/ + '</td> <td><a href="#" onclick="EncounterChargeCapture.OpenInsurance(' + item.InsuranceId + ',event);" title="View Visit Detail">' + item.InsurancePlanName + '</a></td><td>' + item.State + '</td><td>' + item.SubscriberId + '</td><td>' + item.RelationName + '</td><td>' + InsuredName + '</td><td>' + item.GroupId + '</td>');

                $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvPatientInsurances tbody").last().append($row);
                var FirstVisitPlanId = "";
                if (item.InsurancePlanName != "" && item.IsActive == "True") {
                    var bgcolor = "";
                    if (FirstVisitPlanId == "") {
                        FirstVisitPlanId = item.PatientInsuranceId;
                    }
                    var LoadVisitClick = "";
                    var CurrentListText = "";

                    if (ShowSplittedInsuranceName == true && item.SplittedInsurancePlanName != "") {
                        bgcolor = "active-plan";
                        CurrentListText = item.SplittedInsurancePlanName;
                    }
                    else
                        CurrentListText = item.InsurancePlanName;

                   // var lstInsurance = '<li id="' + item.InsuranceId  + '" planpriority="' + item.PlanPriority + '"><div class="checkbox-custom"><input type="checkbox"><a href="#" class="' + bgcolor + '"><span class="ui-icon ui-icon-arrow-4-diag"></span><p class="white size-max160 ellipses" data-toggle="tooltip" data-placement="left" title="' + CurrentListText + '">' + CurrentListText + '</p></a></div></li>';
                    
                    var lstInsurance = '<li id="' + item.InsuranceId + '" class="pl-none mb-xs' + bgcolor + '" planpriority="' + item.PlanPriority
                        + '" AmtCopay="' + item.AmtCopay + '" SpecialistCopay="' + item.SpecialistCopay + '" AssignBenefits="' + item.AssignBenefits
                        + '" Box24BShaded="' + item.Box24BShaded + '" Box24IJShaded="' + item.Box24IJShaded + '" IsActive="' + item.IsActive
                        + '" InsuranceId="' + item.InsuranceId + '" InsurancePlanId="' + item.InsurancePlanId
                        + '" ClaimTypeId="' + item.ClaimTypeId + '" ClaimTypeName="' + item.ClaimTypeName + '" EDIClearingHouseId="' + item.EDIClearingHouseId
                        + '" EDISubmitInsuranceId="' + item.EDISubmitInsuranceId + '" ElectronicSubmit="' + item.ElectronicSubmit + '" InsurancePlanIsSplitted="' + item.InsurancePlanIsSplitted
                        + '" SplittedInsurancePlanName="' + item.SplittedInsurancePlanName + '" SplittedInsurancePlanId="' + item.SplittedInsurancePlanId + '" IsReportNPI="' + item.IsReportNPI
                        
                        + '"><div class="checkbox-custom"><input type="checkbox" onclick="EncounterChargeCapture.CheckedInsurance(this,event)" id="chk' + item.InsuranceId + '"/><label class="white size100per height20 p-xxs insurancelist ellipses mt-default" data-toggle="tooltip" data-placement="left" title="'
                        + CurrentListText + '">' + CurrentListText + '</label></div></li>';
                    $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #lstVisitPlans').last().append(lstInsurance);
                    
                }

            });
        }
        else {
            $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvPatientInsurances").DataTable({
                "language": {
                    "emptyTable": "No Insurance Plan Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
            EncounterChargeCapture.InsurancePlan = null;
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #chkBillToPatient').prop('checked', true);
        }
        //Set ToolTip for Comments.
        if ($.fn.dataTable.isDataTable("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvPatientInsurances"))
            ;
        else
            $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvPatientInsurances").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "order": [[2, "asc"]], "autoWidth": false, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown



        // in case of insurance selected and not checked in that case insurance plan selected
        if (EncounterChargeCapture.InsurancePlan) {
            EncounterChargeCapture.BindSelectedInsurancePlan(EncounterChargeCapture.InsurancePlan);
        }
        EncounterChargeCapture.objInsDeffered.resolve();
        if (isFromSyncBtn) {
            EncounterChargeCapture.objsyncInsurance.resolve();
        }
    },

    CheckedInsurance: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }
        EncounterChargeCapture.SelectedInsurancePlan = null;
        var IsChecked = $(obj).is(':checked');
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        self.find("#lstVisitPlans").find("input[type='checkbox']")
          .prop('checked', false);

        $(obj).prop('checked', IsChecked);
        if (IsChecked) {
            var selectedPlan; //= self.find("#dgvPatientInsurances tbody tr[insuranceid='" + $(obj).attr("id") + "']");
            var currentId = $(obj).attr("id").substring(3, $(obj).attr("id").length);
            selectedPlan = self.find("#lstVisitPlans #" + currentId);
            if (selectedPlan && selectedPlan.attr('IsActive').toLowerCase() == "true") {
                var BillToPatient = self.find("#chkBillToPatient").is(':checked');
                if (BillToPatient) {
                    utility.myConfirm("Do you want to charge bill to insurance?", function () {
                        self.find("#chkBillToPatient").prop('checked', false);
                        EncounterChargeCapture.CheckUnChekInsPlans(obj);
                        EncounterChargeCapture.SelectedInsurancePlan = selectedPlan;
                        EncounterChargeCapture.FillPlanCopayment(EncounterChargeCapture.SelectedInsurancePlan, undefined, 'InsurancePlan');
                        self.find("#frmEncounterChargeCapture #dgvVisitCharge tr").each(function (i, row) {
                            if ($(this).attr("id") && $(this).attr("id") != null) {
                                var $row = $(row);
                                if ($row.find('input[id*="txtPATCharges"]').val() != "" && $row.find('input[id*="txtPATCharges"]').val() != "0.00") {
                                    var PatCharges = Number($row.find('input[id*="txtPATCharges"]').val() == "" ? 0 : $row.find('input[id*="txtPATCharges"]').val());
                                    var INSCharges = Number($row.find('input[id*="txtINSCharges"]').val() == "" ? 0 : $row.find('input[id*="txtINSCharges"]').val());

                                    $row.find('input[id*="txtINSCharges"]').val(parseFloat(PatCharges + INSCharges).toFixed(globalAppdata.DecimalPlaces));
                                    $row.find('input[id*="txtPATCharges"]').val(parseFloat(0).toFixed(globalAppdata.DecimalPlaces));
                                }
                            }
                        });
                        if (EncounterChargeCapture.params["SubmitStatus"] != "Patient") {
                            self.find('#ddlSubmitStatus option').filter(function () {
                                return $.trim($(this).text().toLowerCase()) == EncounterChargeCapture.params["SubmitStatus"].toLowerCase()
                            }).prop('selected', true);
                        }
                    }, function () {
                        $(obj).prop('checked', false);
                        self.find("#chkBillToPatient").prop('checked', true);
                        return false;
                    }, 'Confirmation Alert');
                }
                else {
                    EncounterChargeCapture.CheckUnChekInsPlans(obj);
                    EncounterChargeCapture.SelectedInsurancePlan = selectedPlan;
                    EncounterChargeCapture.FillPlanCopayment(EncounterChargeCapture.SelectedInsurancePlan, undefined, 'InsurancePlan');
                    self.find("#frmEncounterChargeCapture #dgvVisitCharge tr").each(function (i, row) {
                        if ($(this).attr("id") && $(this).attr("id") != null) {
                            var $row = $(row);
                            if ($row.find('input[id*="txtPATCharges"]').val() != "" && $row.find('input[id*="txtPATCharges"]').val() != "0.00") {
                                var PatCharges = Number($row.find('input[id*="txtPATCharges"]').val() == "" ? 0 : $row.find('input[id*="txtPATCharges"]').val());
                                var INSCharges = Number($row.find('input[id*="txtINSCharges"]').val() == "" ? 0 : $row.find('input[id*="txtINSCharges"]').val());

                                $row.find('input[id*="txtINSCharges"]').val(parseFloat(PatCharges + INSCharges).toFixed(globalAppdata.DecimalPlaces));
                                $row.find('input[id*="txtPATCharges"]').val(parseFloat(0).toFixed(globalAppdata.DecimalPlaces));
                            }
                        }
                    });
                }
            }
            else {
                $(obj).prop('checked', false);
                /* if (EncounterChargeCapture.SelectedInsurancePlan) {
                     self.find("#dgvPatientInsurances tbody tr[insuranceid='" + $(EncounterChargeCapture.SelectedInsurancePlan).attr("insuranceid") + "']").find("input[type='checkbox']").prop('checked', true)
                 }*/
                utility.DisplayMessages("Please select active insurance plan.", 2);
            }
        }
        else {
            $(obj).prop('checked', true);
        }
    },

    CheckUnChekInsPlans: function (obj) {
        var BillToPat = $(obj).is(':checked');
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        if (BillToPat) {
            self.find("#lstVisitPlans").find("input[type='checkbox']").prop('checked', false);
            //self.find("#dgvPatientInsurances").find("input[type='checkbox']")
            //  .prop('disabled', true);
            $(obj).prop("checked", true)
            self.find('#aSplitClaim').addClass('disableAll');
            if ($(obj).parent().parent().attr('isreportnpi') == "False") {
                self.find('#tblEncounterChargeCapture #claimAdditionalInfoDiv #chkIsReportNPI').prop('checked', false);
            } else {
                self.find('#tblEncounterChargeCapture #claimAdditionalInfoDiv #chkIsReportNPI').prop('checked', true)
            }
        }
        else {
            $(obj).prop("checked", true)
            //self.find("#dgvPatientInsurances").find("input[type='checkbox']")
            //     .prop('disabled', false);
        }
    },

    ValidateICDCodeAndSetDesc: function (item) {

        var isvalid = true;
        var Id = $(item).attr('id').split("ICD")[1];
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var exists = EncounterChargeCapture.IsICDExists(Id, $(item).val());
        if (exists) {
            for (var i = 1; i < 13; i++) {
                if (i != Id && self.find('#txtICD' + Id).val() != "") {
                    if (self.find('#txtICD' + i).val() != self.find('#txtICD' + Id).val()) {
                        isvalid = true;
                    }
                    else if (self.find('#txtICD' + i).val() == self.find('#txtICD' + Id).val()) {
                        isvalid = false;
                        break;
                    }
                }
            }
            if (isvalid) {
                var hfICD = $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD' + Id).val();
                var hfICDDescription = $("#" + EncounterChargeCapture.params.PanelID).find('#hfICDDescription' + Id).val();
                var hfICD10 = $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD10' + Id).val();
                var hfICD10Description = $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD10Description' + Id).val();
                var hfSNOMED = $("#" + EncounterChargeCapture.params.PanelID).find('#hfSNOMED' + Id).val();
                var hfSNOMEDDescription = $("#" + EncounterChargeCapture.params.PanelID).find('#hfSNOMEDDescription' + Id).val();
                if ($(item).val() != "") {
                    Admin_IMOICD.SearchICD(null, $(item).val(), null, null).done(function (response) {
                        $("#" + EncounterChargeCapture.params.PanelID).find('#btnICD' + Id).prop("disabled", false);
                        $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD' + Id).prop("disabled", false);
                        if (item && Id && Number(Id) < 12 && $(item).val() != "") {
                            //Enable next ICD Code;
                            var nextIndex = (Number(Id) + 1);
                            $("#" + EncounterChargeCapture.params.PanelID).find('#btnICD' + nextIndex).prop("disabled", false);
                            $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD' + nextIndex).prop("disabled", false);
                        }
                        var ICDCodes = JSON.parse(response.ICDLoad_JSON);
                        if (ICDCodes.length > 0) {
                            var outputArr = ICDCodes.filter(function (obj) {
                                if (obj.ICD9 == $(item).val() || obj.ICD10 == $(item).val()) {
                                    return true;
                                }
                            });
                            if (outputArr.length <= 0) {
                                $(item).val('');
                                $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD10Description' + Id).val('');
                                $(item).parent("div").attr("data-original-title", "");
                                utility.DisplayMessages("Invalid ICD Code.", 3);
                            }
                            else {
                                //  if (hfICD10Description == "") {
                                $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD10Description' + Id).val(outputArr[0].ICD10Description);
                                $(item).parent("div").attr("data-original-title", outputArr[0].ICD10Description);
                                //  }
                                //  if (hfICD == "" && hfICDDescription == "" && hfICD10 == "" && hfSNOMED == "" && hfSNOMEDDescription == "") {
                                $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD' + Id).val(outputArr[0].ICD9);
                                $("#" + EncounterChargeCapture.params.PanelID).find('#hfICDDescription' + Id).val(outputArr[0].Description);
                                $("#" + EncounterChargeCapture.params.PanelID).find('#hfICD10' + Id).val(outputArr[0].ICD10);
                                $("#" + EncounterChargeCapture.params.PanelID).find('#hfSNOMED' + Id).val(outputArr[0].SNOMEDId);
                                $("#" + EncounterChargeCapture.params.PanelID).find('#hfSNOMEDDescription' + Id).val(outputArr[0].SNOMEDDescription);
                                EncounterChargeCapture.SetFirstICDPointer(self, Id);
                                //adnan maqbool , pms-134,description in tooltip
                                $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD10Description' + Id).parent("div").attr("title", outputArr[0].ICD10Description).attr("data-original-title", outputArr[0].ICD10Description).attr("data-toggle", "tooltip").attr("data-placement", "top");
                                //Set ToolTip for Comments.
                                $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });

                                //  }
                            }
                        }
                        else {
                            $(item).val('');
                            $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD10Description' + Id).val('');
                            $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD10Description' + Id).parent("div").attr("data-original-title", "");
                            $(item).parent("div").attr("data-original-title", "");
                            utility.DisplayMessages("Invalid ICD Code.", 3)
                        }
                    });
                }
                else
                    $(item).parent("div").attr("data-original-title", "");
                $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD10Description' + Id).parent("div").attr("data-original-title", "");
            }
            else {
                $(item).val('');
                $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD10Description' + Id).val('');
                $(item).parent("div").attr("data-original-title", "");
                $("#" + EncounterChargeCapture.params.PanelID).find('#txtICD10Description' + Id).parent("div").attr("data-original-title", "");
                $(item).parent("div").attr("title", "");
                utility.DisplayMessages("This ICD already exists.", 2);
            }
        }
        EncounterChargeCapture.EnableDisableICDs();
        EncounterChargeCapture.SetFocusedICD(item);
    },

    EnableDisableICDs: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        for (var Id = 2; Id <= 12; Id++) {
            var icd = self.find('#txtICD' + Id);
            if (icd && icd.val() == "" && self.find('#txtICD' + (Number(Id) - 1)).val() == "") {
                icd.prop("disabled", true);
                self.find('#btnICD' + Id).prop("disabled", true);
            }
            else if (icd) {
                icd.prop("disabled", false);
                self.find('#btnICD' + Id).prop("disabled", false);
            }
        }
        EncounterChargeCapture.BindICDNValues(self);
    },

    RemoveSelectedICD: function () {
        if (EncounterChargeCapture.FocusedICD && $(EncounterChargeCapture.FocusedICD).val() != "") {
            utility.myConfirm('1', function () {
                var FocusedICD = EncounterChargeCapture.FocusedICD;
                var Id = $(FocusedICD).attr('id').split("ICD")[1];
                var self = null;
                if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                    self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
                else
                    self = $('#' + EncounterChargeCapture.params.PanelID);
                EncounterChargeCapture.RemoveICDFromStore(Id, self.find('#txtICD' + Id).val(), self);
                EncounterChargeCapture.ICDDelete(Id, self);

            }, function () {
            },
                        '1'
        );
        }
        else {
            utility.DisplayMessages("Please select ICD to remove.", 2);
        }

    },

    RemoveSelectedICDOnBlur: function (obj) {
        if (EncounterChargeCapture.FocusedICD && $(EncounterChargeCapture.FocusedICD).val() == "") {
            var FocusedICD = EncounterChargeCapture.FocusedICD;
            var Id = $(FocusedICD).attr('id').split("ICD")[1];
            var self = null;
            if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
            else
                self = $('#' + EncounterChargeCapture.params.PanelID);
            EncounterChargeCapture.RemoveICDFromStore(Id, self.find('#txtICD' + Id).val(), self);
            //EncounterChargeCapture.ICDDelete(Id, self);
            var VisitId = EncounterChargeCapture.params.VisitId;
            if (VisitId && self.find('#hfPVICDId' + Id).val() != "") {
                EncounterChargeCapture.DeleteVisitICD(Id, VisitId).done(function (response) {
                    if (response.status == true) {
                        self.find('#txtICD' + Id).val('');
                        self.find('#hfICD' + Id).val('');
                        self.find('#hfICDDescription' + Id).val('');
                        self.find('#hfICD10' + Id).val('');
                        self.find('#txtICD10Description' + Id).val('');
                        self.find('#hfSNOMED' + Id).val('');
                        self.find('#hfSNOMEDDescription' + Id).val('');
                        self.find('#hfPVICDId' + Id).val('');
                        if (Id < 12) {
                            var next = parseInt(Id) + 1;
                            self.find('#txtICD' + next).val('');
                            self.find('#hfICD' + next).val('');
                            self.find('#hfICDDescription' + next).val('');
                            self.find('#hfICD10' + next).val('');
                            self.find('#txtICD10Description' + next).val('');
                            self.find('#hfSNOMED' + next).val('');
                            self.find('#hfSNOMEDDescription' + next).val('');
                            self.find('#hfPVICDId' + next).val('');
                        }
                        EncounterChargeCapture.UpdateICDPosition(self);
                        EncounterChargeCapture.UpdateICDPointersValue(self, response);
                        utility.DisplayMessages(response.Message, 1);
                        //$('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }

                });
            }
            else {
                self.find('#txtICD' + Id).val('');
                self.find('#hfICD' + Id).val('');
                self.find('#hfICDDescription' + Id).val('');
                self.find('#hfICD10' + Id).val('');
                self.find('#txtICD10Description' + Id).val('');
                self.find('#hfSNOMED' + Id).val('');
                self.find('#hfSNOMEDDescription' + Id).val('');
                self.find('#hfPVICDId' + Id).val('');
                if (Id < 12) {
                    var next = parseInt(Id) + 1;
                    self.find('#txtICD' + next).val('');
                    self.find('#hfICD' + next).val('');
                    self.find('#hfICDDescription' + next).val('');
                    self.find('#hfICD10' + next).val('');
                    self.find('#txtICD10Description' + next).val('');
                    self.find('#hfSNOMED' + next).val('');
                    self.find('#hfSNOMEDDescription' + next).val('');
                    self.find('#hfPVICDId' + next).val('');
                }
                EncounterChargeCapture.UpdateICDPosition(self);
                EncounterChargeCapture.UpdateICDPointers(self, Id);
            }
        }
        //else {
        //    utility.DisplayMessages("Please select ICD to remove.", 2);
        //}

    },
    MoveUpSelectedICD: function () {
        if (EncounterChargeCapture.FocusedICD && $(EncounterChargeCapture.FocusedICD).val() != "") {
            var FocusedICD = EncounterChargeCapture.FocusedICD;
            var Id = $(FocusedICD).attr('id').split("ICD")[1];
            if (Id > 1 && Id < 13) {
                var self = null;
                if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                    self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
                else
                    self = $('#' + EncounterChargeCapture.params.PanelID);
                var txtICD = self.find('#txtICD' + Id).val();
                var hfICD = self.find('#hfICD' + Id).val();
                var hfICDDescription = self.find('#hfICDDescription' + Id).val();
                var hfICD10 = self.find('#hfICD10' + Id).val();
                var txtICD10Description = self.find('#txtICD10Description' + Id).val();
                var hfSNOMED = self.find('#hfSNOMED' + Id).val();
                var hfSNOMEDDescription = self.find('#hfSNOMEDDescription' + Id).val();

                var prevId = (Number(Id) - 1);
                var tempprevtxtICD = self.find('#txtICD' + prevId).val();

                if (txtICD != "" && tempprevtxtICD != "") {
                    EncounterChargeCapture.FocusedICD = self.find('#txtICD' + prevId);

                    var tempprevhfICD = self.find('#hfICD' + prevId).val();
                    var tempprevhfICDDescription = self.find('#hfICDDescription' + prevId).val();
                    var tempprevhfICD10 = self.find('#hfICD10' + prevId).val();
                    var tempprevtxtICD10Description = self.find('#txtICD10Description' + prevId).val();
                    var tempprevhfSNOMED = self.find('#hfSNOMED' + prevId).val();
                    var tempprevhfSNOMEDDescription = self.find('#hfSNOMEDDescription' + prevId).val();
                    //Updating previous ICDS
                    self.find('#txtICD' + prevId).val(txtICD);
                    self.find('#hfICD' + prevId).val(hfICD);
                    self.find('#hfICDDescription' + prevId).val(hfICDDescription);
                    self.find('#hfICD10' + prevId).val(hfICD10);
                    self.find('#txtICD10Description' + prevId).val(txtICD10Description);
                    self.find('#hfSNOMED' + prevId).val(hfSNOMED);
                    self.find('#hfSNOMEDDescription' + prevId).val(hfSNOMEDDescription);
                    //Updating Selected ICDS
                    self.find('#txtICD' + Id).val(tempprevtxtICD);
                    self.find('#hfICD' + Id).val(tempprevhfICD);
                    self.find('#hfICDDescription' + Id).val(tempprevhfICDDescription);
                    self.find('#hfICD10' + Id).val(tempprevhfICD10);
                    self.find('#txtICD10Description' + Id).val(tempprevtxtICD10Description);
                    self.find('#hfSNOMED' + Id).val(tempprevhfSNOMED);
                    self.find('#hfSNOMEDDescription' + Id).val(tempprevhfSNOMEDDescription);

                    EncounterChargeCapture.EnableDisableICDs();

                    self.find('#txtICD' + prevId).prop("disabled", false);
                    self.find('#btnICD' + prevId).prop("disabled", false);
                    self.find('#txtICD' + Id).prop("disabled", false);
                    self.find('#btnICD' + Id).prop("disabled", false);
                    self.find('#txtICD' + (Number(Id) + 1)).prop("disabled", false);
                    self.find('#btnICD' + (Number(Id) + 1)).prop("disabled", false);
                }
            }
        }
    },

    MoveDownSelectedICD: function () {
        if (EncounterChargeCapture.FocusedICD && $(EncounterChargeCapture.FocusedICD).val() != "") {
            var FocusedICD = EncounterChargeCapture.FocusedICD;
            var Id = $(FocusedICD).attr('id').split("ICD")[1];
            if (Id > 0 && Id < 12) {
                var self = null;
                if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
                    self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
                else
                    self = $('#' + EncounterChargeCapture.params.PanelID);
                var txtICD = self.find('#txtICD' + Id).val();
                var hfICD = self.find('#hfICD' + Id).val();
                var hfICDDescription = self.find('#hfICDDescription' + Id).val();
                var hfICD10 = self.find('#hfICD10' + Id).val();
                var txtICD10Description = self.find('#txtICD10Description' + Id).val();
                var hfSNOMED = self.find('#hfSNOMED' + Id).val();
                var hfSNOMEDDescription = self.find('#hfSNOMEDDescription' + Id).val();

                var nextId = (Number(Id) + 1);
                var tempnexttxtICD = self.find('#txtICD' + nextId).val();

                if (txtICD != "" && tempnexttxtICD != "") {
                    EncounterChargeCapture.FocusedICD = self.find('#txtICD' + nextId);

                    var tempnexthfICD = self.find('#hfICD' + nextId).val();
                    var tempnexthfICDDescription = self.find('#hfICDDescription' + nextId).val();
                    var tempnexthfICD10 = self.find('#hfICD10' + nextId).val();
                    var tempnexttxtICD10Description = self.find('#txtICD10Description' + nextId).val();
                    var tempnexthfSNOMED = self.find('#hfSNOMED' + nextId).val();
                    var tempnexthfSNOMEDDescription = self.find('#hfSNOMEDDescription' + nextId).val();
                    //Updating Next ICDS
                    self.find('#txtICD' + nextId).val(txtICD);
                    self.find('#hfICD' + nextId).val(hfICD);
                    self.find('#hfICDDescription' + nextId).val(hfICDDescription);
                    self.find('#hfICD10' + nextId).val(hfICD10);
                    self.find('#txtICD10Description' + nextId).val(txtICD10Description);
                    self.find('#hfSNOMED' + nextId).val(hfSNOMED);
                    self.find('#hfSNOMEDDescription' + nextId).val(hfSNOMEDDescription);
                    //Updating Selected ICDS
                    self.find('#txtICD' + Id).val(tempnexttxtICD);
                    self.find('#hfICD' + Id).val(tempnexthfICD);
                    self.find('#hfICDDescription' + Id).val(tempnexthfICDDescription);
                    self.find('#hfICD10' + Id).val(tempnexthfICD10);
                    self.find('#txtICD10Description' + Id).val(tempnexttxtICD10Description);
                    self.find('#hfSNOMED' + Id).val(tempnexthfSNOMED);
                    self.find('#hfSNOMEDDescription' + Id).val(tempnexthfSNOMEDDescription);

                    EncounterChargeCapture.EnableDisableICDs();

                    self.find('#txtICD' + nextId).prop("disabled", false);
                    self.find('#btnICD' + nextId).prop("disabled", false);
                    if (nextId < 12) {
                        self.find('#txtICD' + (nextId + 1)).prop("disabled", false);
                        self.find('#btnICD' + (nextId + 1)).prop("disabled", false);
                    }
                }
            }
        }
    },

    GetSelectedInsurancePlan: function () {
        var InsurancePlan = null;
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        self.find("#lstVisitPlans").find("input[type='checkbox']").each(function (index) {
            if ($(this) && $(this).prop('checked'))
                InsurancePlan = $(this).attr("id").substring(3,$(this).attr("id").length);
        });
        return InsurancePlan;
    },

    UncheckInsurancePlans: function () {
        EncounterChargeCapture.SelectedInsurancePlan = null;
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        self.find("#lstVisitPlans").find("input[type='checkbox']").each(function (index) {
            $(this).prop('checked', false);
        });
    },

    BindSelectedInsurancePlan: function (InsurancePlan) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        self.find("#lstVisitPlans").find("input[type='checkbox']").each(function (index) {
            if ($(this).attr("id") && $(this).attr("id").substring(3, $(this).attr("id").length) > 0 && $(this).attr("id").substring(3, $(this).attr("id").length) == InsurancePlan) {
                $(this).prop('checked', true);
                EncounterChargeCapture.SelectedInsurancePlan =$(this).parent().parent("li"); // self.find("#dgvPatientInsurances tbody tr[insuranceid='" + $(this).attr("id") + "']");

                var InsurancePlanIsSplitted = $(this).parent().parent("li").attr('InsurancePlanIsSplitted'); //self.find("#dgvPatientInsurances tbody tr[insuranceid='" + $(this).attr("id") + "']").attr('InsurancePlanIsSplitted');
                var SplittedInsurancePlanId = $(this).parent().parent("li").attr('SplittedInsurancePlanId'); //self.find("#dgvPatientInsurances tbody tr[insuranceid='" + $(this).attr("id") + "']").attr('SplittedInsurancePlanId');
                if (EncounterChargeCapture.IsToHideSplittedClaimBtn == false && InsurancePlanIsSplitted.toLowerCase() == 'true' && Number(SplittedInsurancePlanId) > 0)
                    self.find('#aSplitClaim').removeClass('disableAll');
                else
                    self.find('#aSplitClaim').addClass('disableAll');
            } else {
                $(this).prop('checked', false);
            }
        });

        if (InsurancePlan != null) {
            if (EncounterChargeCapture.params.ParentCtrl == "BillingInformation") {
                if ($("#divVisitPlans #lstVisitPlans li").length > 0) {
                    $("#divVisitPlans #lstVisitPlans li:first").addClass("active-plan");
                    
                }
            } else {
                self.find("#divVisitPlans #lstVisitPlans li").each(function () {
                    if ($(this).attr("id") == InsurancePlan)
                    {
                        $(this).find(".insurancelist").addClass("active-plan");
                        $(this).find(".insurancelist").removeClass("insurancelist");

                    }
                    else
                    {
                        $(this).find(".active-plan").addClass("insurancelist");
                        $(this).find(".active-plan").removeClass("active-plan");
                        
                    }
                });
            }
        }
        else {
            if (EncounterChargeCapture.params.ParentCtrl == "BillingInformation") {
                if ($("#divVisitPlans #lstVisitPlans li").length > 0) {
                    $("#divVisitPlans #lstVisitPlans li:first").addClass("active-plan");
                }
            }
        }
    },

    rowChecked: function (checkBoxobj, event) {
        if (event != null) {
            event.stopPropagation();
        }
        EncounterChargeCapture.CheckedAllCharges(checkBoxobj, event);
    },

    CheckedAllCharges: function (obj, event) {

        if (event != null) {
            event.stopPropagation();
        }

        var self = null;
        if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail" || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.ParentCtrl == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.ParentCtrl == "billTabUnClaimedAppointment" || EncounterChargeCapture.params.ParentCtrl == "Bill_ChargeSearch" || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.ParentCtrl == "schTabCalendar" || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "billTabPaymentPosting" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            self = $("#" + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $("#" + EncounterChargeCapture.params.PanelID);


        if (!$(obj).prop('checked')) {
            self.find("#chkAllCharges").prop('checked', false);
            self.find("#chkAllCharges").attr('title', 'Select all');
        }
        else {

            var selected = [];
            self.find("#dgvVisitCharge tr").each(function (i, row) {
                if ($(this).attr("id") && $(this).attr("id") != null) {
                    var childRow = EncounterChargeCapture.EditableGrid.datatable.row(this).child();
                    var chkActive = $(childRow).find("input[id^=chkActive]"); {
                        if (!$(chkActive).is(":checked")) {
                            selected.push(this);
                        }
                    }
                }
            });

            if (selected.length > 0) {
                self.find("#chkAllCharges").prop('checked', false);
                self.find("#chkAllCharges").attr('title', 'Select all');
            }
            else {
                self.find("#chkAllCharges").prop('checked', true);
                self.find("#chkAllCharges").attr('title', 'Unselect all');
            }
        }
    },

    SetFocusedICD: function (obj) {

        EncounterChargeCapture.FocusedICD = obj;
    },

    sort_li: function (a, b) {
        return ($(b).attr('planpriority')) < ($(a).attr('planpriority')) ? 1 : -1;
    },

    OpenPAN: function () {
        var params = [];
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];

        var self = "";
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }
        var patientInsuranceID = "";
        if (EncounterChargeCapture.SelectedInsurancePlan) {
            patientInsuranceID = $(EncounterChargeCapture.SelectedInsurancePlan).attr('insuranceid')
        }
        var DOSFrom = self.find("#dtpDOSFrom").val();

        params["RefCtrlPAN"] = "txtPriorAuthNumber";
        params["RefHiddenIdCtrl"] = "hfAuthorizeId";
        params["patientID"] = self.find("#hfPatientId").val();
        params["patientInsuranceID"] = patientInsuranceID;
        params["ReferralDate"] = DOSFrom;

        LoadActionPan('Patient_PreAuthorization', params);
    },

    OpenPANDetail: function (HiddenCtrl) {
        var params = [];
        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Authorization", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                params["PreAuthorizationId"] = $('#' + EncounterChargeCapture.params["PanelID"] + ' #' + HiddenCtrl).val();;
                params["PatientID"] = $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #hfPatientId').val();
                params["mode"] = "Edit";
                params["PlanResponse"] = EncounterChargeCapture.params.PlanResponse;
                LoadActionPan('Patient_PreAuthorization_Detail', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    ValidatePAN: function ($obj) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }

        var sourceArr = $($obj).autocomplete("option", "source");
        var haveObject = sourceArr.filter(function (obj) {
            var IsValid = obj.value.toLowerCase() == $($obj).val().toLowerCase();
            return IsValid;
        });
        if (haveObject.length == 0) {
            self.find("#lnkPANEdit").addClass("hidden");
            self.find("#lblPAN").removeClass("hidden");
        }
        else {
            self.find("#lnkPANEdit").removeClass('hidden');
            self.find("#lblPAN").addClass("hidden");
        }
    },

    OpenInsurance: function (PatientInsuranceId, event) {
        if (event != null) {
            event.stopPropagation();
        }
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0) {
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        }
        else {
            self = $('#' + EncounterChargeCapture.params.PanelID);
        }
        var params = [];
        params["patientID"] = self.find('#frmEncounterChargeCapture #hfPatientId').val();
        params["PatBanner"] = null;
        params["PatientInsuranceId"] = PatientInsuranceId;
        params["RefCtrl"] = "frmEncounterChargeCapture";

        if (EncounterChargeCapture.params.TabID == 'billTabOutOfOfficeVisits' || EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "schTabMultipleView" || EncounterChargeCapture.params.TabID == "mstrTabReports")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];

        LoadActionPan('Patient_Insurance', params);

    },

    FillVisitsDetails: function (self) {
        $(self.find("#dgvPatientOutstanding")).find('input').each(function () {
            var actualval = $(this).val();
            $(this).attr("actualval", actualval);
            $(this).addClass("text-right");
        });
    },

    UpdateVisitsDetails: function (self) {
        var InsCharges = 0;
        var PatCharges = 0;
        //PMS-384, adnan maqbool
        self.find("#frmEncounterChargeCapture #dgvVisitCharge tr:nth-child(1)").each(function () {
            if ($(this).attr("id") && $(this).attr("id") != null) {
                var $row = $(this);
                INSCharges = parseFloat($row.find('input[id*="txtINSCharges"]').val() != "" ? $row.find('input[id*="txtINSCharges"]').val() : "0");
                PATCharges = parseFloat($row.find('input[id*="txtPATCharges"]').val() != "" ? $row.find('input[id*="txtPATCharges"]').val() : "0");
                PatCharges = PatCharges + PATCharges;
                InsCharges = InsCharges + INSCharges;
            }
        });
        setTimeout(function () {
            self.find("#txtInsCharges").val(parseFloat(InsCharges).toFixed(globalAppdata.DecimalPlaces));
            self.find("#txtPatCharges").val(parseFloat(PatCharges).toFixed(globalAppdata.DecimalPlaces));
            var totalCharges = "0.00";
            totalCharges = parseFloat(InsCharges + PatCharges).toFixed(globalAppdata.DecimalPlaces);
            self.find("#txtTotalCharges").val(totalCharges);
        }, 500);
    },

    OpenInsurancePlan: function (InsurancePlanId, event) {

        if (event != null) {
            event.stopPropagation();
        }

        var params = [];
        var PanelID = null;
        if (EncounterChargeCapture.params["PanelID"] != 'pnlEncounterChargeCapture')
            PanelID = EncounterChargeCapture.params["PanelID"];

        if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
            params["ParentCtrl"] = 'EncounterChargeCapture';
        else
            params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];

        params["InsurancePlanId"] = InsurancePlanId
        params["PlanAddressId"] = ""
        params["mode"] = "Edit";
        Admin_InsurancePlan.params["FromAdmin"] == "0";
        LoadActionPan('insurancePlanDetail', params, PanelID);

    },

    ClaimPaymentsGridLoad: function (response) {
        var gridId = "#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #pnlClaimPayments_Result #dgvPayments";
        if ($.fn.dataTable.isDataTable(gridId))
            $(gridId).dataTable().fnDestroy();
        $(gridId + " tbody").find("tr").remove();
        //if (response.ClaimPaymentsCount > 0) {
        //    $('#' + EncounterChargeCapture.params["PanelID"] + " #pnlClaimPayments_Result").css("display", "inline");
        //}
        //else {
        //    $('#' + EncounterChargeCapture.params["PanelID"] + " #pnlClaimPayments_Result").css("display", "none");
        //}
        if (response.ClaimPaymentsCount > 0) {

            var planPriorityClass = "";
            var claimTitle = "";

            var ClaimPaymentsLoadJSONData = JSON.parse(response.ClaimPaymentsLoad_JSON);
            $.each(ClaimPaymentsLoadJSONData, function (i, item) {
                var Paid = utility.convertToFigure(0, true);
                if (item.Paid < 0) {
                    Paid = utility.convertToFigure((item.Paid) * (-1), true, true);
                } else {
                    Paid = utility.convertToFigure(item.Paid, true)
                }
                var Adj = utility.convertToFigure(0, true);
                if (item.Adj < 0) {
                    Adj = utility.convertToFigure((item.Adj) * (-1), true, true);
                } else {
                    Adj = utility.convertToFigure(item.Adj, true)
                }
                var $row = $('<tr/>');
                // $row.append('<td>' + item.CheckNo + '</td><td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td> <td>' + item.InsuranceName + '</td> <td>' + utility.convertToFigure(item.Allowed, true) + '</td> <td>' + utility.convertToFigure(item.Deductables, true) + '</td><td>' + utility.convertToFigure(item.Coinsurance, true) + '</td><td>' + utility.convertToFigure(item.Copay, true) + '</td><td>' + Paid + '</td><td>' + Adj + '</td><td>' + utility.convertToFigure(item.PatientResponsibility, true) + '</td><td>' + item.Code + '</td>');
                if (item.PaidAmountCr != "" && item.Copay != "") {
                    $row.append('<td>' + item.CheckNo + '</td><td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td> <td>' + item.InsuranceName + '</td> <td class="text-right">' + utility.convertToFigure(item.Allowed, true) + '</td> <td class="text-right">' + utility.convertToFigure(item.Deductables, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Coinsurance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Copay, true, true) + '</td><td class="text-right">' + Paid + '</td><td class="text-right">' + Adj + '</td><td class="text-right">' + utility.convertToFigure(item.PatientResponsibility, true) + '</td><td>' + item.Code + '</td>');
                }
                else if (item.PaidAmountCr != "" && item.Allowed != "") {
                    $row.append('<td>' + item.CheckNo + '</td><td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td> <td>' + item.InsuranceName + '</td> <td class="text-right">' + utility.convertToFigure(item.Allowed, true, true) + '</td> <td class="text-right">' + utility.convertToFigure(item.Deductables, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Coinsurance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Copay, true) + '</td><td class="text-right">' + Paid + '</td><td class="text-right">' + Adj + '</td><td class="text-right">' + utility.convertToFigure(item.PatientResponsibility, true) + '</td><td>' + item.Code + '</td>');
                }
                else if (item.PaidAmountCr != "" && item.Deductables != "") {
                    $row.append('<td>' + item.CheckNo + '</td><td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td> <td>' + item.InsuranceName + '</td> <td class="text-right">' + utility.convertToFigure(item.Allowed, true) + '</td> <td class="text-right">' + utility.convertToFigure(item.Deductables, true, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Coinsurance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Copay, true) + '</td><td class="text-right">' + Paid + '</td><td class="text-right">' + Adj + '</td><td class="text-right">' + utility.convertToFigure(item.PatientResponsibility, true) + '</td><td>' + item.Code + '</td>');
                }
                else if (item.PaidAmountCr != "" && item.Coinsurance != "") {
                    $row.append('<td>' + item.CheckNo + '</td><td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td> <td>' + item.InsuranceName + '</td> <td class="text-right">' + utility.convertToFigure(item.Allowed, true) + '</td> <td class="text-right">' + utility.convertToFigure(item.Deductables, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Coinsurance, true, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Copay, true) + '</td><td class="text-right">' + Paid + '</td><td class="text-right">' + Adj + '</td><td class="text-right">' + utility.convertToFigure(item.PatientResponsibility, true) + '</td><td>' + item.Code + '</td>');
                }
                else if (item.PaidAmountCr != "" && item.PatientResponsibility != "") {
                    $row.append('<td>' + item.CheckNo + '</td><td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td> <td>' + item.InsuranceName + '</td> <td class="text-right">' + utility.convertToFigure(item.Allowed, true) + '</td> <td class="text-right">' + utility.convertToFigure(item.Deductables, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Coinsurance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Copay, true) + '</td><td class="text-right">' + Paid + '</td><td class="text-right">' + Adj + '</td><td class="text-right">' + utility.convertToFigure(item.PatientResponsibility, true, true) + '</td><td>' + item.Code + '</td>');
                }
                else {
                    $row.append('<td>' + item.CheckNo + '</td><td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td> <td>' + item.InsuranceName + '</td> <td class="text-right">' + utility.convertToFigure(item.Allowed, true) + '</td> <td class="text-right">' + utility.convertToFigure(item.Deductables, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Coinsurance, true) + '</td><td class="text-right">' + utility.convertToFigure(item.Copay, true) + '</td><td class="text-right">' + Paid + '</td><td class="text-right">' + Adj + '</td><td class="text-right">' + utility.convertToFigure(item.PatientResponsibility, true) + '</td><td>' + item.Code + '</td>');
                }
                $(gridId + " tbody").last().append($row);
            });
            if ($.fn.dataTable.isDataTable(gridId) || $(gridId).parent().parent().hasClass("dataTables_wrapper"))
                ;
            else
                $(gridId).DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "order": [[1, "desc"]], "autoWidth": false, "bFilter": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] });

        }
        else {

            $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #pnlClaimPayments_Result #dgvPayments").DataTable({
                bDestroy: true,
                "language": {
                    "emptyTable": "No Payments Found"
                }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
    },

    BindICDNValues: function (self) {
        EncounterChargeCapture.ICDArrays = [];
        for (var Id = 1; Id < 13; Id++) {
            if (self.find('#txtICD' + Id) && self.find('#txtICD' + Id).val() != "") {
                EncounterChargeCapture.BuildICDsArray(Id, self.find('#txtICD' + Id).val(), self);
                (self.find('#txtICD' + (Id + 1))).prop("disabled", false);
                (self.find('#btnICD' + (Id + 1))).prop("disabled", false);
            }
        }
    },

    IsICDExists: function (ICD, value) {
        var breturn = false;
        if (EncounterChargeCapture.ICDArrays) {
            for (var i = 0; i < EncounterChargeCapture.ICDArrays.length; i++) {
                if (EncounterChargeCapture.ICDArrays[i].ICD == ICD && EncounterChargeCapture.ICDArrays[i].value == value) {
                    breturn = true;
                    break;
                }
            }
        }
        return breturn;
    },

    BuildICDsArray: function (ICD, value, self) {
        var array = EncounterChargeCapture.ICDArrays;
        if (!EncounterChargeCapture.IsICDExists(ICD, value)) {
            var objICD = new Object();
            objICD["ICD"] = ICD;
            objICD["value"] = value;
            objICD["hfICD"] = self.find("#hfICD" + ICD).val();
            objICD["hfICD10"] = self.find("#hfICD10" + ICD).val();
            objICD["hfICDDescription"] = self.find("#hfICDDescription" + ICD).val();
            objICD["hfSNOMED"] = self.find("#hfSNOMED" + ICD).val();
            objICD["hfSNOMEDDescription"] = self.find("#hfSNOMEDDescription" + ICD).val();
            objICD["txtICD10Description"] = self.find("#txtICD10Description" + ICD).val();
            array.push(objICD);
        }
        EncounterChargeCapture.ICDArrays = array;
        self.find("#hfPVICDIdsLength").val(EncounterChargeCapture.ICDArrays.length);
    },

    RemoveICDFromStore: function (ICD, value, self) {
        if (EncounterChargeCapture.ICDArrays) {
            var UpdatedArray = [];
            UpdatedArray = jQuery.grep(EncounterChargeCapture.ICDArrays, function (obj) {
                return obj.ICD != ICD && obj.value != value;
            });
            EncounterChargeCapture.ICDArrays = UpdatedArray;
            self.find("#hfPVICDIdsLength").val(EncounterChargeCapture.ICDArrays.length);
        }
    },
    SetChargeCaptureparmsFromLoadedTabs: function (TotalLoadedChargeCapture, TabID) {
        var UpdatedArray = [];
        UpdatedArray = jQuery.grep(LoadedEncounterTabs, function (value) {
            return !value.TabID.indexOf("EncounterChargeCapture") == 0;
        });
        for (var i = 0; i < UpdatedArray.length; i++) {
            if (TotalLoadedChargeCapture && i == (TotalLoadedChargeCapture - 1)) {
                EncounterChargeCapture.params = null;
                if (i > 0) {
                    EncounterChargeCapture.params = UpdatedArray.ICDArrays[i - 1].params;
                }
            }
        }
    },
    ICDDelete: function (ICDIndex, self) {
        var VisitId = EncounterChargeCapture.params.VisitId;
        if (VisitId) {
            EncounterChargeCapture.DeleteVisitICD(ICDIndex, VisitId).done(function (response) {
                if (response.status == true) {
                    self.find('#txtICD' + ICDIndex).val('');
                    self.find('#hfICD' + ICDIndex).val('');
                    self.find('#hfICDDescription' + ICDIndex).val('');
                    self.find('#hfICD10' + ICDIndex).val('');
                    self.find('#txtICD10Description' + ICDIndex).val('');
                    self.find('#hfSNOMED' + ICDIndex).val('');
                    self.find('#hfSNOMEDDescription' + ICDIndex).val('');
                    if (ICDIndex < 12) {
                        var next = parseInt(ICDIndex) + 1;
                        self.find('#txtICD' + next).val('');
                        self.find('#hfICD' + next).val('');
                        self.find('#hfICDDescription' + next).val('');
                        self.find('#hfICD10' + next).val('');
                        self.find('#txtICD10Description' + next).val('');
                        self.find('#hfSNOMED' + next).val('');
                        self.find('#hfSNOMEDDescription' + next).val('');
                    }
                    EncounterChargeCapture.UpdateICDPosition(self);
                    EncounterChargeCapture.UpdateICDPointersValue(self, response);
                    utility.DisplayMessages(response.Message, 1);
                    //$('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }

            });
        }
        else {
            self.find('#txtICD' + ICDIndex).val('');
            self.find('#hfICD' + ICDIndex).val('');
            self.find('#hfICDDescription' + ICDIndex).val('');
            self.find('#hfICD10' + ICDIndex).val('');
            self.find('#txtICD10Description' + ICDIndex).val('');
            self.find('#hfSNOMED' + ICDIndex).val('');
            self.find('#hfSNOMEDDescription' + ICDIndex).val('');
            if (ICDIndex < 12) {
                var next = parseInt(ICDIndex) + 1;
                self.find('#txtICD' + next).val('');
                self.find('#hfICD' + next).val('');
                self.find('#hfICDDescription' + next).val('');
                self.find('#hfICD10' + next).val('');
                self.find('#txtICD10Description' + next).val('');
                self.find('#hfSNOMED' + next).val('');
                self.find('#hfSNOMEDDescription' + next).val('');
            }
            EncounterChargeCapture.UpdateICDPosition(self);
            EncounterChargeCapture.UpdateICDPointers(self, ICDIndex);
        }
    },
    UpdateICDPointersValue: function (self, response) {
        var VisitCharges_detail = JSON.parse(response.VisitsChargesLoad_JSON);
        $.each(VisitCharges_detail, function (i, item) {
            var ChargeId = item.hfChargeId;
            var VisitChargesTable = $("#" + EncounterChargeCapture.params.PanelID + " #dgvVisitCharge");
            utility.bindMyJSON(true, item, false, VisitChargesTable).done(function () {
                $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());
            });
        });
    },
    DeleteVisitICD: function (ICDIndex, VisitId) {
        var data = "VisitId=" + VisitId + "&ICDIndex=" + ICDIndex;
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "DELATE_VISIT_ICD");
    },

    UpdateICDPosition: function (self) {
        var array = EncounterChargeCapture.ICDArrays;
        //Reset All controls.
        for (var i = 1; i < 13; i++) {
            self.find("#txtICD" + i).val('');
            self.find("#hfICD" + i).val('');
            self.find("#hfICD10" + i).val('');
            self.find("#hfICDDescription" + i).val('');
            self.find("#hfSNOMED" + i).val('');
            self.find("#hfSNOMEDDescription" + i).val('');
            self.find("#txtICD10Description" + i).val('');
        }
        for (var i = 0; i < array.length; i++) {
            var IcdIndex = (parseInt(i) + 1);
            self.find("#txtICD" + IcdIndex).val(array[i].value);
            self.find("#hfICD" + IcdIndex).val(array[i].hfICD);
            self.find("#hfICD10" + IcdIndex).val(array[i].hfICD10);
            self.find("#hfICDDescription" + IcdIndex).val(array[i].hfICDDescription);
            self.find("#hfSNOMED" + IcdIndex).val(array[i].hfSNOMED);
            self.find("#hfSNOMEDDescription" + IcdIndex).val(array[i].hfSNOMEDDescription);
            self.find("#txtICD10Description" + IcdIndex).val(array[i].txtICD10Description);
        }
        EncounterChargeCapture.EnableDisableICDs();
    },
    UpdateICDPointers: function (self, ICDIndex) {
        var ICDPointers = $(self).find("input[id*='txtICDPointer']");
        ICDPointers.each(function (item) {
            var icdRef = $(this).val();
            var RefICD = self.find('#txtICD' + icdRef);
            if (RefICD && RefICD.length > 0 && $(this).val() != "" && RefICD.val() != "") {
                if ($(this).val() != "" && icdRef != $(this).val()) {
                }
            }
            else {
                var secondVal = "";
                var thirdVal = "";
                var fourthVal = "";
                var currentval = "";
                var totalICDs = EncounterChargeCapture.ICDArrays.length + 1;
                if ($(this).val() != "") {
                    if ($(this).next('input').next('input').next('input')) {
                        fourthVal = $(this).next('input').next('input').next('input').val() == "" ? 0 : $(this).next('input').next('input').next('input').val();
                        if (parseInt(fourthVal) > totalICDs)
                            fourthVal = parseInt(fourthVal) - 1;
                    }
                    if ($(this).next('input').next('input')) {
                        thirdVal = $(this).next('input').next('input').val() == "" ? 0 : $(this).next('input').next('input').val();
                        if (parseInt(thirdVal) > totalICDs)
                            thirdVal = parseInt(thirdVal) - 1;
                    }
                    if ($(this).next('input')) {
                        secondVal = $(this).next('input').val() == "" ? 0 : $(this).next('input').val();
                        if (parseInt(secondVal) > totalICDs)
                            secondVal = parseInt(secondVal) - 1;
                    }
                    if ($(this).val()) {
                        currentval = $(this).val() == "" ? 0 : $(this).val();
                        if (parseInt(currentval) > totalICDs) {
                            currentval = parseInt(currentval) - 1;
                        }
                        else {
                            currentval = $(this).val()
                        }
                        $(this).val(currentval == 0 ? "" : currentval);
                    }
                    if ($(this)) {
                        $(this).val('');
                        $(this).val(currentval == 0 ? "" : currentval);
                    }
                    if ($(this).next('input')) {
                        $(this).next('input').val('');
                        $(this).val(secondVal == 0 ? "" : secondVal);
                    }
                    if ($(this).next('input').next('input')) {
                        $(this).next('input').next('input').val('');
                        $(this).next('input').val(thirdVal == 0 ? "" : thirdVal);
                    }
                    if ($(this).next('input').next('input').next('input')) {
                        $(this).next('input').next('input').next('input').val('');
                        $(this).next('input').next('input').val(fourthVal == 0 ? "" : fourthVal);
                    }
                }
            }
        });
    },
    SetFirstICDPointer: function (self, Id) {
        var ICDPointers = $(self).find("input[id*='txtICDPointer1']");
        if (ICDPointers && ICDPointers.length > 0) {
            if (Id == "1") {
                $(ICDPointers[0]).val(Id);
            }
        }
    },
    SelectPrimaryInsurance: function (isFromSyncBtn) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        if (EncounterChargeCapture.params.mode && EncounterChargeCapture.IsSelfPay && EncounterChargeCapture.IsSelfPay == "False" && (EncounterChargeCapture.params.mode.toLowerCase() == "add" || isFromSyncBtn == true)) {

            var isInsuranceChecked = false;
            self.find("#lstVisitPlans input[type=checkbox]").each(function () {
                if ($(this).prop('checked') == true) {
                    isInsuranceChecked = true;
                }
            });
            if (isFromSyncBtn == true && isInsuranceChecked == true) {
                //if already checked then insurance will not select again
                return;
            }

           // var ActiveInsurances = self.find("#dgvPatientInsurances tbody tr[isactive='True']");
            var ActiveInsurances = self.find("#lstVisitPlans li[isactive='True']");
            // EncounterChargeCapture.CheckedInsurance(checkBoxobj, event);
            if (ActiveInsurances.length > 0) {
                EncounterChargeCapture.SelectedInsurancePlan = self.find("#dgvPatientInsurances tbody tr[isactive='True']:first");
                $(ActiveInsurances[0]).find("input[type='checkbox']").prop("checked", true);
                var SelectedInsurance = ActiveInsurances[0];
                if (SelectedInsurance && $(SelectedInsurance).attr("AssignBenefits") == "True")
                    self.find("#chkAssgBenefits").prop("checked", true);
                else
                    self.find("#chkAssgBenefits").prop("checked", false);


                self.find("#ddlBox24IJShaded").val($(SelectedInsurance).attr("Box24IJShaded"));
                self.find("#ddlBox24BShaded").val($(SelectedInsurance).attr("Box24BShaded"));

                if (SelectedInsurance && $(SelectedInsurance).attr("IsEmployed") == "True") {
                    self.find("#RadEmploymentYes").prop("checked", true);
                    self.find("#RadEmploymentNo").prop("checked", false);
                }
                else {
                    self.find("#RadEmploymentNo").prop("checked", true);
                    self.find("#RadEmploymentYes").prop("checked", false);
                }
                if (SelectedInsurance && $(SelectedInsurance).attr("IsReportNPI") == "True") {
                    self.find("#chkIsReportNPI").prop("checked", true);
                }
                var ProviderId = self.find('#hfProvider').val();
                var ProviderId = ProviderId != "" ? parseInt(ProviderId) : 0;
                if (ProviderId > 0) {
                    Admin_Provider.SearchProvider('', ProviderId).done(function (response) {
                        if (response.status != false) {
                            if (response.ProviderCount > 0) {
                                var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);

                                if (EncounterChargeCapture.params.mode.toLowerCase() == 'add') {
                                    if (ProviderLoadJSONData[0].IsSpecialist == "False") {
                                        if (SelectedInsurance && $(SelectedInsurance).attr("AmtCopay") && EncounterChargeCapture.IsSelfPay == "False") {
                                            self.find('#txtVisitCopayment').val(utility.convertToFigure($(SelectedInsurance).attr("AmtCopay")));
                                        }
                                        self.find('#radPCP').trigger("click");
                                    }
                                    else {
                                        if (SelectedInsurance && $(SelectedInsurance).attr("SpecialistCopay") && EncounterChargeCapture.IsSelfPay == "False") {
                                            self.find('#txtVisitCopayment').val(utility.convertToFigure($(SelectedInsurance).attr("SpecialistCopay")));
                                        }
                                        self.find('#radSpecialist').trigger("click");
                                    }
                                    EncounterChargeCapture.SetChargeVisitCopay();
                                    self.find("#hfProviderCLIA").val(ProviderLoadJSONData[0].CLIA)
                                }
                            }
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                    });
                }
                else {
                    if (SelectedInsurance && $(SelectedInsurance).attr("AmtCopay") && EncounterChargeCapture.IsSelfPay == "False") {
                        self.find('#txtVisitCopayment').val(utility.convertToFigure($(SelectedInsurance).attr("AmtCopay")));
                        EncounterChargeCapture.SetChargeVisitCopay();
                    }
                }
            }
            //self.find("#dgvPatientInsurances").find("input[type='checkbox']").each(function (index) {
            //    if ($(this) && $(this).prop('checked'))
            //        InsurancePlan = $(this).attr("id");
        }
        // selectedPlan = self.find("#dgvPatientInsurances tbody tr[insuranceid='" + $(obj).attr("id") + "']");
    },

    SetDefaultSettingByInsurance: function (self, isFromProvider) {
        var SelectedInsurance = EncounterChargeCapture.SelectedInsurancePlan;
        if (SelectedInsurance && $(SelectedInsurance).attr("AssignBenefits") == "True")
            self.find("#chkAssgBenefits").prop("checked", true);
        else
            self.find("#chkAssgBenefits").prop("checked", false);

        if (SelectedInsurance && $(SelectedInsurance).attr("IsEmployed") == "True") {
            self.find("#RadEmploymentYes").prop("checked", true);
            self.find("#RadEmploymentNo").prop("checked", false);
        }
        else {
            self.find("#RadEmploymentNo").prop("checked", true);
            self.find("#RadEmploymentYes").prop("checked", false);
        }

        // PMS-1763 - Commented as per discussion with salman gillani that visit status selection will not change.
        if (isFromProvider != true) {
            if (EncounterChargeCapture.IsSetSubmissionMode == true && SelectedInsurance && $(SelectedInsurance).attr("insuranceid")) {

                if ($(SelectedInsurance).attr("electronicsubmit") && $(SelectedInsurance).attr("electronicsubmit").toLowerCase() == "true") {

                    self.find('#ddlSubmitStatus option').filter(function () {
                        return $.trim($(this).text().toLowerCase()) == "ready to submit electronic";
                    }).attr('selected', true);
                }
                else if ($(SelectedInsurance).attr("electronicsubmit") && $(SelectedInsurance).attr("electronicsubmit").toLowerCase() == "false") {

                    self.find('#ddlSubmitStatus option').filter(function () {
                        return $.trim($(this).text().toLowerCase()) == "ready to submit paper";
                    }).attr('selected', true);

                }
                EncounterChargeCapture.IsSetSubmissionMode == false;
            }
            else {
                if (SelectedInsurance && $(SelectedInsurance).attr("insuranceid")) {
                    if ($(SelectedInsurance).attr("electronicsubmit") && $(SelectedInsurance).attr("electronicsubmit").toLowerCase() == "true") {
                        self.find('#ddlSubmitStatus option').filter(function () {
                            return $.trim($(this).text().toLowerCase()) == "ready to submit electronic";
                        }).attr('selected', true);
                    }
                    else if ($(SelectedInsurance).attr("electronicsubmit") && $(SelectedInsurance).attr("electronicsubmit").toLowerCase() == "false") {

                        self.find('#ddlSubmitStatus option').filter(function () {
                            return $.trim($(this).text().toLowerCase()) == "ready to submit paper";
                        }).attr('selected', true);
                    }

                }
            }
        }
    },
    ResetAllICDFields: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        self.find("input[id*=txtICD]").not("input[id*=txtICDPointer]").val('');
    },
    SetChargeVisitCopay: function () {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);
        var CurrentRow = $("#" + EncounterChargeCapture.params.PanelID + " #pnlVisitCharge_Result #dgvVisitCharge tbody tr[id*=-]:first");
        $(CurrentRow).find('input[id*="txtCOPAY"]').val(Number(0).toFixed(globalAppdata.DecimalPlaces));
        if (EncounterChargeCapture.IsSelfPay == "False" && EncounterChargeCapture.SelectedInsurancePlan && EncounterChargeCapture.params.mode.toLowerCase() == "add") {
            $(CurrentRow).find('input[id*="txtCOPAY"]').val($('#' + EncounterChargeCapture.params.PanelID + " #txtVisitCopayment").val());
        }
        else {
            self.find('#txtVisitCopayment').val(Number(0).toFixed(globalAppdata.DecimalPlaces));
        }
    },

    //Begin Added by Azeem Raza Tayyab to fix bug#:PMS-5762
    ReloadClaimPayments: function (isFromPaymentPosting) {
        if (EncounterChargeCapture.params["VisitId"]) {
            var VisitId = EncounterChargeCapture.params["VisitId"];
            EncounterChargeCapture.LoadClaimPaymentDetails(VisitId).done(function (response) {
                if (response.status != false) {
                    var ClaimPaymentsDetail = JSON.parse(response.ClaimPaymentsDetail_JSON);
                    var self = null;
                    if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail" || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.ParentCtrl == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.ParentCtrl == "billTabUnClaimedAppointment" || EncounterChargeCapture.params.ParentCtrl == "Bill_ChargeSearch" || EncounterChargeCapture.params.ParentCtrl == "BillingInformation" || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.ParentCtrl == "schTabCalendar" || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.ParentCtrl == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "billTabPaymentPosting" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
                        self = $("#" + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
                    else
                        self = $("#" + EncounterChargeCapture.params.PanelID);

                    utility.bindMyJSON(true, ClaimPaymentsDetail, false, self.find("#dgvPatientOutstanding")).done(function () {

                        EncounterChargeCapture.FillVisitsDetails(self);
                        //PMS-1537
                        if (isFromPaymentPosting) {
                            EncounterChargeCapture.UncheckInsurancePlans();
                            EncounterChargeCapture.LoadVisit($("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val(), VisitId);
                        }
                    });
                }
            });
        }
    },

    LoadClaimPaymentDetails: function (VisitId) {
        var data = "VisitID=" + VisitId;
        return MDVisionService.defaultService(data, "PATIENT_ENCOUNTER_VISITS", "CLAIM_PAYMENTS_DETAILS");
    },
    //End Added by Azeem Raza Tayyab to fix bug#:PMS-5762

    ValidateSubmitStatus: function (self) {
        var isValid = false;
        var submitStatus = self.find('#ddlSubmitStatus option:selected').text();
        var IsSubmitted = self.find("#hfIsSubmitted").val()
        if (submitStatus && ((submitStatus.toLowerCase() == "submitted" && IsSubmitted == "1") || (EncounterChargeCapture.params.mode.toLowerCase() == "add" && submitStatus.toLowerCase() != "submitted") ||
        EncounterChargeCapture.params.mode.toLowerCase() == "edit" && submitStatus.toLowerCase() != "submitted" && IsSubmitted == "")) {
            isValid = true;
        }
        else {
            isValid = false;
        }
        return isValid;
    },
    SaveClaim: function (strMessage, myJSON) {
        var FeeAlert = "";
        if ($('#ddlBox24IJShaded').val() == '2' || $('#ddlBox24BShaded').val() == '2' || $('#ddlBox24BShaded').val() == '3') {
            var ProviderId = self.find('#hfProvider').val();
            var ProviderId = ProviderId != "" ? parseInt(ProviderId) : 0;

            if (ProviderId > 0) {
                Admin_Provider.SearchProvider('', ProviderId).done(function (response) {
                    if (response.status != false) {
                        if (response.ProviderCount > 0) {
                            var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);
                            if (!ProviderLoadJSONData[0].TaxonomyCode) {
                                utility.myConfirm('Taxonomy code is missing in the provider detail screen are you sure you want to select "Print Taxonomy Code" ?', function () {
                                    EncounterChargeCapture.SavePatientClaim(strMessage, myJSON);
                                }, function () {
                                    //NO CALLBACK
                                }, 'Confirm Taxonomy Code');
                            }
                            else {
                                EncounterChargeCapture.SavePatientClaim(strMessage, myJSON);
                            }
                        }
                        else {
                            EncounterChargeCapture.SavePatientClaim(strMessage, myJSON);
                        }
                    }
                });
            }
            else {
                EncounterChargeCapture.SavePatientClaim(strMessage, myJSON);
            }
        }
        else {
            EncounterChargeCapture.SavePatientClaim(strMessage, myJSON);
        }
    },

    SavePatientClaim: function (strMessage, myJSON) {
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

            if (EncounterChargeCapture.IsZeroFee) {
                EncounterChargeCapture.IsZeroFee = false;

                utility.myConfirm('Do you want to save <b>0</b> Fee', function () {
                    if (EncounterChargeCapture.IsFeeGreaterThanHPCSCost) {
                        EncounterChargeCapture.IsFeeGreaterThanHPCSCost = false;
                        FeeAlert = "Cost of the <b>" + EncounterChargeCapture.HPCS + "</b> is <b>$ </b><b>" + EncounterChargeCapture.HPCSCost + "</b> less than the billed amount. whould you like to continue?";
                        utility.myConfirm(FeeAlert, function () {
                            EncounterChargeCapture.PatVisitSave(strMessage, myJSON);
                        }, function () {
                            return false;
                        }, 'Alert');
                    }
                    else {
                        EncounterChargeCapture.PatVisitSave(strMessage, myJSON);
                    }
                }, function () {
                    return false;
                }, 'Confirm <b>0</b> Fee Save');
            }
            else {
                if (EncounterChargeCapture.IsFeeGreaterThanHPCSCost) {
                    EncounterChargeCapture.IsFeeGreaterThanHPCSCost = false;
                    FeeAlert = "Cost of the <b>" + EncounterChargeCapture.HPCS + "</b> is <b>$ </b><b>" + EncounterChargeCapture.HPCSCost + "</b> less than the billed amount. whould you like to continue?";
                    utility.myConfirm(FeeAlert, function () {
                        EncounterChargeCapture.PatVisitSave(strMessage, myJSON);
                    }, function () {
                        return false;
                    }, 'Alert');
                }
                else {
                    EncounterChargeCapture.PatVisitSave(strMessage, myJSON);
                }
            }
    },

    //Start PRD-635 TahreeMalik   Duplicate Claim Alert
    PatVisitSave: function (strMessage, myJSON) {
        var self = null;
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

        var ClaimNumber = '';
        if (EncounterChargeCapture.params.mode == "Edit")
            ClaimNumber = self.find('#txtClaimNumber').val();

        if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail" || EncounterChargeCapture.params.mode == "Edit")
            EncounterChargeCapture.DuplicateClaimAlert(strMessage, myJSON, EncounterChargeCapture.params.mode, ClaimNumber, self);
        else
            EncounterChargeCapture.PatSaveUpdateClaim(strMessage, myJSON, EncounterChargeCapture.params.mode, self);
    },
    LoadVisitDetail: function (VisitId, PatientId, IsOkBtnOnly, event) {
        if (event != null) {
            event.stopPropagation();
        }
        if (IsOkBtnOnly)
            $('#modal-from-dom #btnClose').click();

        EncounterChargeCapture.params.VisitId = VisitId;
        EncounterChargeCapture.isloadFrom = 0;
        EncounterChargeCapture.isloadTo = 0;
        EncounterChargeCapture.FillVisitData(VisitId, PatientId);
    },
    DuplicateClaimAlert: function(strMessage, myJSON, mode, ClaimNumber, self){
        var objData = {};
        objData['DOSFrom'] = self.find('#dtpDOSFrom').val();
        objData['DOSTo'] = self.find('#dtpDOSTo').val();
        objData['PatientId'] = self.find('#hfPatientId').val();
        objData["providerId"] = self.find('#hfProvider').val();
        objData["facilityId"] = self.find('#hfFacility').val();
        objData["ClaimNumber"] = ClaimNumber;
        var myJson = JSON.stringify(objData);

        Encounter_CreateClaim.VerifyDuplicateClaim_DBCall(myJson).done(function (response) {
            if (response.status != false) {
                var ClaimJSON = JSON.parse(response.VisitsLoad_JSON);
                if (mode == 'Edit') {
                    EncounterChargeCapture.PatSaveUpdateClaim(strMessage, myJSON, mode, self);
                    utility.duplicateClaimAlert("The Claim# " + Encounter_CreateClaim.BindClaimNumbersWFunction('EncounterChargeCapture', ClaimJSON, true) + " already exist for this patient with same DOS, Facility & Provider.", null, null, 'Duplicate Claim Alert!', 'OK', '', true, true, false);
                }
                else if(mode == 'Add'){
                    utility.duplicateClaimAlert("The Claim# " + Encounter_CreateClaim.BindClaimNumbersWFunction('EncounterChargeCapture', ClaimJSON, true) + " already exist for this patient with same DOS, Facility & Provider. Do you want to create a new claim?", function () {
                        EncounterChargeCapture.PatSaveUpdateClaim(strMessage, myJSON, mode, self);
                    }, function () {
                        EncounterChargeCapture.UnLoad();
                    }, 'Duplicate Claim Alert!', 'Yes', 'No', false, true, true);
                }
            }
            else
                EncounterChargeCapture.PatSaveUpdateClaim(strMessage, myJSON, mode, self);
        });
    },
    PatSaveUpdateClaim: function (strMessage, myJSON, mode, self) {
        var PatientId = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val();
        var VisitId = EncounterChargeCapture.params.VisitId;
        if(mode == "Add"){          
            EncounterChargeCapture.SaveVisit(myJSON, PatientId).done(function (response) {
                EncounterChargeCapture.LoadVisitAfterSaveUpdate(myJSON, self, response, mode);
            });
        }
        else if (mode == "Edit") {
            EncounterChargeCapture.UpdateVisit(myJSON, VisitId, PatientId).done(function (response) {
                EncounterChargeCapture.LoadVisitAfterSaveUpdate(myJSON, self, response, mode);
            });
        }
    },
    LoadVisitAfterSaveUpdate: function (myJSON, self, response, mode) {
        var ClaimsubmitStatus = self.find('#ddlSubmitStatus option:selected').text().toLowerCase();
        if (response.status != false) {
            EncounterChargeCapture.UpdateSubmitStatusParams(myJSON);
            if (mode == 'Add') {
                EncounterChargeCapture.IsSaved = true;
                $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #txtClaimNumber").val(response.ClaimNumber);
                $('#' + EncounterChargeCapture.params.PanelID + ' #aPrintClaim').removeClass('disableAll');
            }
            EncounterChargeCapture.params['VisitId'] = response.visitId;
            EncounterChargeCapture.ReloadClaimPayments();

            var visitupdateDetail = JSON.parse(response.VisitFill_JSON);
            utility.bindMyJSON(true, visitupdateDetail, false, self);
            if (response.NoteComments) {
                $('#' + EncounterChargeCapture.params.PanelID + ' #txtNoteComments').val("");
                $('#' + EncounterChargeCapture.params.PanelID + ' #txtNoteCommentsLoad').val(response.NoteComments);
            }

            EncounterChargeCapture.params.mode = "Edit";
            EncounterChargeCapture.ShowChargeDetails(true);
            self.find('#aPaymentPosting').removeClass('disableAll');
            self.find("#followUpCommentlnk").removeClass('disableAll');
            self.find("#aClaimDocuments").removeClass('disableAll');
            var SelectedPatientInsurance = EncounterChargeCapture.GetSelectedInsurancePlan();

            if (mode == 'Edit') {
                EncounterChargeCapture.InsurancePlan = SelectedPatientInsurance;
                //handle billtopatient check box
                if ($('#' + EncounterChargeCapture.params.PanelID + ' #chkBillToPatient').prop('checked') == true) {
                    $('#' + EncounterChargeCapture.params.PanelID + ' #chkBillToPatient').prop('checked', true);
                }
                else {
                    $('#' + EncounterChargeCapture.params.PanelID + ' #chkBillToPatient').prop('checked', false);
                }
            }

            if (SelectedPatientInsurance != null) {
                if (EncounterChargeCapture.params.ParentCtrl == "BillingInformation") {
                    if ($("#divVisitPlans #lstVisitPlans li").length > 0) {
                        if (mode == 'Add')
                            $("#divVisitPlans #lstVisitPlans li:first").addClass("active-plan");
                        else if (mode == 'Edit'){
                            $("#divVisitPlans #lstVisitPlans li:first").find("label").addClass("active-plan");
                            $("#divVisitPlans #lstVisitPlans li:first").find("label").removeClass("insurancelist");
                        }
                    }
                }
                else {
                    self.find("#divVisitPlans #lstVisitPlans li").each(function () {
                        if ($(this).attr("id") == SelectedPatientInsurance) {
                            $(this).find("label").addClass("active-plan");
                            $(this).find("label").removeClass("insurancelist");
                        }
                        else {
                            $(this).find("label").addClass("insurancelist");
                            $(this).find("label").removeClass("active-plan");
                        }
                    });
                }
            } else {
                if (EncounterChargeCapture.params.ParentCtrl == "BillingInformation") {
                    if ($("#divVisitPlans #lstVisitPlans li").length > 0) {
                        $("#divVisitPlans #lstVisitPlans li:first").find("label").addClass("active-plan");
                        $("#divVisitPlans #lstVisitPlans li:first").find("label").removeClass("insurancelist");
                    }
                }
            }

            self.find("#lstVisitPlans").find("input[type='checkbox']").each(function (index) {
                if ($(this).prop("checked") == true) {
                    var InsurancePlanIsSplitted = $(this).parent().parent("li").attr('InsurancePlanIsSplitted');
                    var SplittedInsurancePlanId = $(this).parent().parent("li").attr('SplittedInsurancePlanId');
                    if (EncounterChargeCapture.IsToHideSplittedClaimBtn == false && InsurancePlanIsSplitted.toLowerCase() == 'true' && Number(SplittedInsurancePlanId) > 0)
                        self.find('#aSplitClaim').removeClass('disableAll');
                    else
                        self.find('#aSplitClaim').addClass('disableAll');
                }
            });

            if (EncounterChargeCapture.params.ParentCtrl == "chargeBatchDetail" && mode == "Edit") {
                EncounterChargeCapture.FillChargeBatchViewer();
            }
            else {
                if ($("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tbody tr:last").attr('id') && $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tbody tr:last").attr('id').indexOf('-') >= 0)
                    EncounterChargeCapture.LoadVisitCharges(EncounterChargeCapture.params["VisitId"]);
            }

            Patient_Demographic.UpdateBalancesInBanner();
            $('#frmEncounterChargeCapture').data('serialize', $('#frmEncounterChargeCapture').serialize());

            //Scrub_Claim
            if (ClaimsubmitStatus == "ready to submit electronic" || ClaimsubmitStatus == "ready to submit paper") {
                EncounterChargeCapture.Scrub_Claim(EncounterChargeCapture.params.VisitId, response.Message);
            }
            else {
                utility.DisplayMessages(response.Message, 1);
            }
        }
        else {
            utility.DisplayMessages(response.Message, 3);
        }
        EncounterChargeCapture.isloadFrom = 0;
        EncounterChargeCapture.isloadTo = 0;
    },
    //End PRD-635 TahreeMalik  Duplicate Claim Alert

    UpdateClaim: function (strMessage, myJSON) {
        if ($('#ddlBox24IJShaded').val() == '2' || $('#ddlBox24BShaded').val() == '2' || $('#ddlBox24BShaded').val() == '3') {
            var ProviderId = self.find('#hfProvider').val();
            var ProviderId = ProviderId != "" ? parseInt(ProviderId) : 0;

            if (ProviderId > 0) {
                Admin_Provider.SearchProvider('', ProviderId).done(function (response) {
                    if (response.status != false) {
                        if (response.ProviderCount > 0) {
                            var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);
                            if (!ProviderLoadJSONData[0].TaxonomyCode) {
                                utility.myConfirm('Taxonomy code is missing in the provider detail screen are you sure you want to select "Print Taxonomy Code" ?', function () {
                                    EncounterChargeCapture.UpdatePatientClaim(strMessage, myJSON);
                                }
                                    , function () {
                                        //NO CALLBACK
                                    }, 'Confirm Taxonomy Code');
                            }
                            else {
                                EncounterChargeCapture.UpdatePatientClaim(strMessage, myJSON);
                            }
                        }
                        else {
                            EncounterChargeCapture.UpdatePatientClaim(strMessage, myJSON);
                        }
                    }
                });
            }
            else {
                EncounterChargeCapture.UpdatePatientClaim(strMessage, myJSON);
            }
        }
        else {
            EncounterChargeCapture.UpdatePatientClaim(strMessage, myJSON);
        }
    },

    UpdatePatientClaim: function (strMessage, myJSON) {
        if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
            self = $('#' + EncounterChargeCapture.params.PanelID + " #pnlEncounterChargeCapture");
        else
            self = $('#' + EncounterChargeCapture.params.PanelID);

            //change paramater to hidden field because paramater was coming undefined
            if (EncounterChargeCapture.IsZeroFee) {
                EncounterChargeCapture.IsZeroFee = false;
                utility.myConfirm('Do you want to save <b>0</b> Fee', function () {
                    if (EncounterChargeCapture.IsFeeGreaterThanHPCSCost) {
                        EncounterChargeCapture.IsFeeGreaterThanHPCSCost = false;
                        FeeAlert = "Cost of the <b>" + EncounterChargeCapture.HPCS + "</b> is <b>$ </b><b>" + EncounterChargeCapture.HPCSCost + "</b> less than the billed amount. whould you like to continue?";
                        utility.myConfirm(FeeAlert, function () {
                            EncounterChargeCapture.PatVisitUpdate(strMessage, myJSON);
                        }, function () {
                            return false;
                        }, 'Alert');
                    }
                    else {
                        EncounterChargeCapture.PatVisitUpdate(strMessage, myJSON);
                    }
                }, function () {
                    return false;
                }, 'Confirm <b>"0"</b> Fee Save');
            }
            else {
                if (EncounterChargeCapture.IsFeeGreaterThanHPCSCost) {
                    EncounterChargeCapture.IsFeeGreaterThanHPCSCost = false;
                    FeeAlert = "Cost of the <b>" + EncounterChargeCapture.HPCS + "</b> is <b>$ </b><b>" + EncounterChargeCapture.HPCSCost + "</b> less than the billed amount. whould you like to continue?";
                    utility.myConfirm(FeeAlert, function () {
                        EncounterChargeCapture.PatVisitUpdate(strMessage, myJSON);
                    }, function () {
                        return false;
                    }, 'Alert');
                }
                else {
                    EncounterChargeCapture.PatVisitUpdate(strMessage, myJSON);
                }
            }
    },
    PatVisitUpdate: function (strMessage, myJSON) {
        var contractualAdjustments=EncounterChargeCapture.checkContractualAdjustment();
        if(contractualAdjustments.length>0){
            var text = "";
            var codeList = [];
            var oldCodeAndConAdjList = [];
            var newCodeAndConAdjList = [];
            $.each(contractualAdjustments, function (i, item) {
                codeList.push(item["oldCpt"] + " to " + item["newCpt"]);
                if (item.oldConAdjExists) {
                    //PMS-4719
                    oldCodeAndConAdjList.push(item["oldCpt"] + " $" + parseFloat(item["oldConAdj"]).toFixed(Number(globalAppdata.DecimalPlaces)));
                }
                if (item.newConAdjExists) {
                    //PMS-4719
                    newCodeAndConAdjList.push(item["newCpt"] + " $" + parseFloat(item["newConAdj"]).toFixed(Number(globalAppdata.DecimalPlaces)));
                }
            });

            text = (codeList.length>0?"CPT Code changed from " + codeList.join(', ') + ".":"")
             + (oldCodeAndConAdjList.length>0?"Auto Contractual Adj for " + oldCodeAndConAdjList.join(', ') + " will be refunded":"")
             + ((oldCodeAndConAdjList.length > 0 && newCodeAndConAdjList.length > 0) ? " & " : (newCodeAndConAdjList.length == 0?".":""))
            +(newCodeAndConAdjList.length>0? "Auto Contractual Adj for " + newCodeAndConAdjList.join(', ') + " will be inserted.":"")+("Do you want to Proceed?");

            utility.myConfirmDetail(text, function () {
                EncounterChargeCapture.PatVisitSave(strMessage, myJSON);
            }, function () {
            },"Alert");
        }
        else{
            EncounterChargeCapture.PatVisitSave(strMessage, myJSON);
        }
       
    },
    LoadVisitCharges: function (visitId) {
        EncounterChargeCapture.VisitChargesLoad(visitId).done(function (response) {
            EncounterChargeCapture.ChargeLoad(response);
        });
    },
setDefaultDataForAnesthesia: function () {

    var self = null;
    if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
        self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
    else
        self = $('#' + EncounterChargeCapture.params.PanelID);

    var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");

    if (self.find('#radAnesthesiologist').is(':checked')) {
        formValidation.enableFieldValidators('Anesthesiologist', true);
        formValidation.enableFieldValidators('AnesthesiologistStartTime', true);
        formValidation.enableFieldValidators('AnesthesiologistEndTime', true);

        formValidation.enableFieldValidators('CRNA', false);
        formValidation.enableFieldValidators('CRNAStartTime', false);
        formValidation.enableFieldValidators('CRNAEndTime', false);

        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Anesthesiologist');
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'AnesthesiologistStartTime');
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'AnesthesiologistEndTime');
        }

    }
    else if (self.find('#radAnesthesiologist').is(':checked')) {
        formValidation.enableFieldValidators('CRNA', true);
        formValidation.enableFieldValidators('CRNAStartTime', true);
        formValidation.enableFieldValidators('CRNAEndTime', true);

        formValidation.enableFieldValidators('Anesthesiologist', false);
        formValidation.enableFieldValidators('AnesthesiologistStartTime', false);
        formValidation.enableFieldValidators('AnesthesiologistEndTime', false);

        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'CRNA');
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'CRNAStartTime');
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'CRNAEndTime');
        }

    }
    //PMS-482, adnan maqbool
    $('#' + EncounterChargeCapture.params["PanelID"] + " #divRadAnesthesia").find("i").remove();
    formValidation.enableFieldValidators('RadAnesthesia', true);
    //

},

SetAnesthesiaValidation: function (obj) {
    var currentObject = $(obj).attr('id');
    var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
    if (currentObject == "radAnesthesiologist") {

        //show hide required stars
        $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').find('.ansreq').removeClass('hidden');
        $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').find('.crnareq').addClass('hidden');

        formValidation.enableFieldValidators('Anesthesiologist', true);
        formValidation.enableFieldValidators('AnesthesiologistStartTime', true);
        formValidation.enableFieldValidators('AnesthesiologistEndTime', true);

        formValidation.enableFieldValidators('CRNA', false);
        formValidation.enableFieldValidators('CRNAStartTime', false);
        formValidation.enableFieldValidators('CRNAEndTime', false);

        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'Anesthesiologist');
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'AnesthesiologistStartTime');
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'AnesthesiologistEndTime');
        }
    }
    else if (currentObject == "radCRNA") {

        //show hide required stars
        $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').find('.crnareq').removeClass('hidden');
        $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').find('.ansreq').addClass('hidden');

        formValidation.enableFieldValidators('CRNA', true);
        formValidation.enableFieldValidators('CRNAStartTime', true);
        formValidation.enableFieldValidators('CRNAEndTime', true);

        formValidation.enableFieldValidators('Anesthesiologist', false);
        formValidation.enableFieldValidators('AnesthesiologistStartTime', false);
        formValidation.enableFieldValidators('AnesthesiologistEndTime', false);

        if ($('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'CRNA');
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'CRNAStartTime');
            $('#' + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'CRNAEndTime');
        }
    }
    $('#' + EncounterChargeCapture.params["PanelID"] + " #divRadAnesthesia").find("i").remove();
},

InitializeAnesthesiaTimePicker: function () {
    var self = null;
    if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
        self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
    else
        self = $('#' + EncounterChargeCapture.params.PanelID);

    self.find('#txtAnesthesiologistStartTime').timepicker({
        timeFormat: 'HH:mm',
        showMeridian: false,
        defaultTime: false
    }).on('changeTime.timepicker', function (e) {
        if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'AnesthesiologistStartTime');
        }

    });
    self.find('#txtAnesthesiologistEndTime').timepicker({
        timeFormat: 'HH:mm',
        showMeridian: false,
        defaultTime: false
    }).on('changeTime.timepicker', function (e) {

        if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'AnesthesiologistEndTime');
        }
    });
    self.find('#txtCRNAStartTime').timepicker({
        timeFormat: 'HH:mm',
        showMeridian: false,
        defaultTime: false
    }).on('changeTime.timepicker', function (e) {
        if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'CRNAStartTime');
        }

    });
    self.find('#txtCRNAEndTime').timepicker({
        timeFormat: 'HH:mm',
        showMeridian: false,
        defaultTime: false
    }).on('changeTime.timepicker', function (e) {

        if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != null && typeof $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data('bootstrapValidator') != 'undefined') {
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').bootstrapValidator('revalidateField', 'CRNAEndTime');
        }
    });
},

AnesthesiaUnits: function (Starttime, Endtime, RowId) {
    var start = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #' + Starttime).val();
    var end = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #' + Endtime).val();
    s = start.split(':');
    e = end.split(':');
    min = e[1] - s[1];
    hour_carry = 0;
    if (min < 0) {
        min += 60;
        hour_carry += 1;
    }
    hour = e[0] - s[0] - hour_carry;
    hour = hour * 4;
    min = (min / 15);
    diff = min + hour;
    if (diff % 1 > 0.5) { //&& $('#txtRoundBiuldUnits' + RowId).val() == "2"
        diff = diff + 1;
    }
    else if (diff % 1 == 0.5) {// && $('#txtRoundBiuldUnits' + RowId).val() == "2") {
        //diff = diff + 1;
    }
    else if (diff % 1 >= 0.5) {// && $('#txtRoundBiuldUnits' + RowId).val() != "2") {
        diff = diff + 1;
    }
    units = isNaN(parseInt(diff)) ? 0 : parseInt(diff);
    $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtTimeUnits' + RowId).val(units);
    //$('#txtTotalUnits' + RowId).val(units);
    //EncounterChargeCapture.StartTmeEndTime(RowId);
},

TotalMinutesAndUnits: function (RowId) {
    var start = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtStartTime' + RowId).val();
    var end = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtEndTime' + RowId).val();
    s = start.split(':');
    e = end.split(':');
    min = e[1] - s[1];
    hour_carry = 0;
    if (min < 0) {
        min += 60;
        hour_carry += 1;
    }
    hour = e[0] - s[0] - hour_carry;
    hourUnits = hour * 4;
    hourintominutes = hour * 60;
    minutes = hourintominutes + min;
    minUnits = (min / 15);
    TimeUnit = minUnits + hourUnits;

    if (TimeUnit % 1 > 0.5 && $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtRoundBiuldUnits' + RowId).val() == "2") {
        TimeUnit = TimeUnit + 1;
    }
    else if (TimeUnit % 1 == 0.5 && $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtRoundBiuldUnits' + RowId).val() == "2") {
        //diff = diff + 1;
    }
    else if (TimeUnit % 1 >= 0.5 && $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtRoundBiuldUnits' + RowId).val() != "2") {
        TimeUnit = TimeUnit + 1;
    }
    if (TimeUnit < 0) {
        TimeUnit = 0;
    }
    units = isNaN(parseInt(TimeUnit)) ? 0 : parseInt(TimeUnit);
    var Base = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtBaseUnits' + RowId).val();
    var Risk = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtRiskUnits' + RowId).val();

    var BaseUnits = isNaN(parseInt($("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtBaseUnits' + RowId).val())) ? 0 : parseFloat(Base);
    var RiskUnits = isNaN(parseInt($("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtRiskUnits' + RowId).val())) ? 0 : parseFloat(Risk);

    TotalUnits = units + BaseUnits + RiskUnits;
    minutes = isNaN(minutes) ? 0 : minutes;
    if (minutes < 0) {
        minutes = 0;
    }
    TotalMinutes = minutes + (BaseUnits * 15) + (RiskUnits * 15);
    TUnits = (TotalMinutes / 15)

    if (TUnits.toFixed(2) % 1 > 0.5 && $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtRoundBiuldUnits' + RowId).val() == "2") {
        TUnits = TUnits + 1;
    }
    else if (TUnits.toFixed(2) % 1 == 0.5 && $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtRoundBiuldUnits' + RowId).val() == "2") {
        //diff = diff + 1;
    }
    else if (TUnits.toFixed(2) % 1 >= 0.5 && $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtRoundBiuldUnits' + RowId).val() != "2") {
        TUnits = TUnits + 1;
    }
    TUnits = isNaN(parseInt(TUnits)) ? 0 : parseInt(TUnits);
    TotalMinutes = isNaN(parseInt(TotalMinutes)) ? 0 : parseFloat(TotalMinutes).toFixed(0);

    $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtTotalUnits' + RowId).val(TUnits);
    $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtTotalMinutes' + RowId).val(TotalMinutes);

    $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #txtUnits' + RowId).val(TUnits);
    $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #txtUnits' + RowId).trigger('blur');

    //EncounterChargeCapture.StartTmeEndTime(RowId);
},

StartTmeEndTime: function (RowId) {
    //var start = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtStartTime' + RowId).val();
    //var end = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtEndTime' + RowId).val();
    //var TimeUnits = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtTimeUnits' + RowId).val();
    //var Tunits = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtTotalUnits' + RowId).val();
    //var Tminutes = $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtTotalMinutes' + RowId).val();
    //if (TimeUnits < 0 || Tminutes < 0) {
    //    $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtTimeUnits' + RowId).val("");
    //}
    //if (Tunits < 0) {
    //    $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtTotalUnits' + RowId).val("");
    //}
    //if (Tminutes < 0) {
    //    $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtTotalMinutes' + RowId).val("");
    //}
    //if (end != "" && start != "" && end < start) {
    //    utility.DisplayMessages("Start Time cannot be greater than End Time.", 3);
    //    $("#" + EncounterChargeCapture.params.PanelID + ' #dgvVisitCharge #Anesthesia' + RowId + ' #txtEndTime' + RowId).val(start);
    //}
},

CheckStartEndTime: function (obj) {
    //var CRNAStartTime = $("#" + EncounterChargeCapture.params.PanelID + ' #txtCRNAStartTime').val();
    //var CRNAEndTime = $("#" + EncounterChargeCapture.params.PanelID + ' #txtCRNAEndTime').val();
    //var AnesthesiologistStartTime = $("#" + EncounterChargeCapture.params.PanelID + ' #txtAnesthesiologistStartTime').val();
    //var AnesthesiologistEndTime = $("#" + EncounterChargeCapture.params.PanelID + ' #txtAnesthesiologistEndTime').val();

    //if (CRNAEndTime != "" && CRNAStartTime != "" && parseInt(CRNAEndTime) < parseInt(CRNAStartTime)) {
    //    utility.DisplayMessages("Start Time cannot be greater than End Time.", 3);
    //    if ($(obj).attr('id') == 'txtCRNAEndTime')
    //        $("#" + EncounterChargeCapture.params.PanelID + ' #txtCRNAEndTime').val(CRNAStartTime);
    //    if ($(obj).attr('id') == 'txtCRNAStartTime')
    //        $("#" + EncounterChargeCapture.params.PanelID + ' #txtCRNAStartTime').val(CRNAEndTime);
    //}

    //if (AnesthesiologistEndTime != "" && AnesthesiologistStartTime != "" && parseInt(AnesthesiologistEndTime) < parseInt(AnesthesiologistStartTime)) {
    //    utility.DisplayMessages("Start Time cannot be greater than End Time.", 3);
    //    if ($(obj).attr('id') == 'txtAnesthesiologistEndTime')
    //        $("#" + EncounterChargeCapture.params.PanelID + ' #txtAnesthesiologistEndTime').val(AnesthesiologistStartTime);
    //    if ($(obj).attr('id') == 'txtAnesthesiologistStartTime')
    //        $("#" + EncounterChargeCapture.params.PanelID + ' #txtAnesthesiologistStartTime').val(AnesthesiologistEndTime);
    //}
},

setfocus: function (obj) {
    $(obj).focus();
    $(obj).trigger();
},

OpenAnesthesiaProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
    var self = null;
    if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
        self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
    else
        self = $('#' + EncounterChargeCapture.params.PanelID);

    var params = [];
    var PanelID = EncounterChargeCapture.params["TabID"];
    if (self.find('#radAnesthesiologist').is(':checked')) {
        params["IsOptional"] = false;
    } else {
        params["IsOptional"] = true;
    }
    params["RefForm"] = 'frmEncounterChargeCapture';
    params["Title"] = Title;
    params["RefCtrl"] = RefCtrl;
    params["RefCtrlHidden"] = RefCtrlHidden;
    params["RefCtrlLabel"] = RefCtrlLabel;
    params["RefCtrlLink"] = RefCtrlLink;
    params["ProviderId"] = "-1";
    params["FromAdmin"] = "0";
    params["Specialty"] = "Anesthesiology";
    if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
        params["ParentCtrl"] = 'EncounterChargeCapture';
    else
        params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
    LoadActionPan('Admin_Provider', params);
},

OpenCRNAProvider: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
    var self = null;
    if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
        self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
    else
        self = $('#' + EncounterChargeCapture.params.PanelID);

    var params = [];
    var PanelID = EncounterChargeCapture.params["TabID"];
    if (self.find('#radCRNA').is(':checked')) {
        params["IsOptional"] = false;
    } else {
        params["IsOptional"] = true;
    }
    params["RefForm"] = 'frmEncounterChargeCapture';
    params["Title"] = Title;
    params["RefCtrl"] = RefCtrl;
    params["RefCtrlHidden"] = RefCtrlHidden;
    params["RefCtrlLabel"] = RefCtrlLabel;
    params["RefCtrlLink"] = RefCtrlLink;
    params["ProviderId"] = "-1";
    params["FromAdmin"] = "0";
    params["Specialty"] = "CRNA";
    if (EncounterChargeCapture.params.TabID == 'chargeBatchDetail' || EncounterChargeCapture.params.TabID == 'clinicalTabProgressNote' || EncounterChargeCapture.params.TabID == 'billTabUnClaimedAppointment' || EncounterChargeCapture.params.TabID == 'Bill_ChargeSearch' || EncounterChargeCapture.params.TabID == 'Patient_Case_Detail' || EncounterChargeCapture.params.TabID == 'schTabCalendar' || EncounterChargeCapture.params.TabID == 'batchTabEncounter' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpPatientAR_Detail' || EncounterChargeCapture.params.TabID == 'Bill_FollowUpInsuranceAR_Detail' || EncounterChargeCapture.params.TabID == "billTabClaimSubmission" || EncounterChargeCapture.params.TabID == "Bill_ClaimSubmissionErrorReport" || EncounterChargeCapture.params.TabID == "Bill_PaymentPosting" || EncounterChargeCapture.params.TabID == "EDIClaimViewDetail" || EncounterChargeCapture.params.TabID == "ERADetail" || EncounterChargeCapture.params.TabID == "Document_Viewer" || EncounterChargeCapture.params.TabID == "claimViewDetail" || EncounterChargeCapture.params.TabID == "chargesViewDetail" || EncounterChargeCapture.params.TabID == "Bill_PatientResponsibilityPayment" || EncounterChargeCapture.params.TabID == "billTabFollowUpPatientAR" || EncounterChargeCapture.params.TabID == "billTabFollowUpInsuranceAR" || EncounterChargeCapture.params.TabID == "Bill_ERA_Summary" || EncounterChargeCapture.params.TabID == "Patient_Ledger" || EncounterChargeCapture.params.TabID == "mstrTabReports" || EncounterChargeCapture.params.ParentCtrl == "ReportsViewer_Detail" || EncounterChargeCapture.params.TabID == "schTabMultipleView")
        params["ParentCtrl"] = 'EncounterChargeCapture';
    else
        params["ParentCtrl"] = EncounterChargeCapture.params["TabID"];
    LoadActionPan('Admin_Provider', params);
},

OnChangeRiskUnits: function (obj) {
    var self = null;
    if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
        self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
    else
        self = $('#' + EncounterChargeCapture.params.PanelID);

    var riskUnits = $(obj).val();
    self.find('#txtRiskUnits').val(riskUnits);
    self.find(".anesthesiarow").find('input[id*="txtRiskUnits"]').val(riskUnits);
},

OnChangeServiceType: function (obj) {
    var self = null;
    if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
        self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
    else
        self = $('#' + EncounterChargeCapture.params.PanelID);

    self.find("#dgvVisitCharge").find("tr").find('input[id*="txtModifier1"]').val($(obj).find("option:selected").attr("refvalue"));
    self.find("#dgvVisitCharge").find("tr").find('input[id*="txtModifier1"]').trigger('blur');
},

OnChangeASA: function (obj) {

    var self = null;
    if (EncounterChargeCapture.params.PanelID.indexOf("pnlEncounterChargeCapture") < 0)
        self = $('#' + EncounterChargeCapture.params.PanelID + ' #pnlEncounterChargeCapture');
    else
        self = $('#' + EncounterChargeCapture.params.PanelID);

    var riskUnits = $(obj).find("option:selected").attr("refvalue");
    self.find('#txtRiskUnits').val(riskUnits);
    self.find(".anesthesiarow").find('input[id*="txtRiskUnits"]').val(riskUnits);
    self.find(".anesthesiarow").find('input[id*="txtRiskUnits"]').trigger('change');
    if (self.find("#dgvVisitCharge").find("tr").find('input[id*="txtModifier1"]').val() == "" && $(obj).find("option:selected").val() != "") {
        self.find("#dgvVisitCharge").find("tr").find('input[id*="txtModifier1"]').val($(obj).find("option:selected").text());
        self.find("#dgvVisitCharge").find("tr").find('input[id*="txtModifier1"]').trigger('blur');
    }
    else if (self.find("#dgvVisitCharge").find("tr").find('input[id*="txtModifier1"]').val() != "" && $(obj).find("option:selected").val() != "") {
        self.find("#dgvVisitCharge").find("tr").find('input[id*="txtModifier2"]').val($(obj).find("option:selected").text());
        self.find("#dgvVisitCharge").find("tr").find('input[id*="txtModifier2"]').trigger('blur');
    }

},

EnableAnesthesiaDiv: function (Newrow) {

    if ($("#" + EncounterChargeCapture.params.PanelID + " #divChargeDetails #ddlClaimType").find('option:selected').text() == "Professional Anesthesia") {
        $('#' + EncounterChargeCapture.params.PanelID).find('#dgvVisitCharge .anesthesiarow').show();
        $('#' + EncounterChargeCapture.params["PanelID"] + " #divRadAnesthesia").find("i").remove();
        var formValidation = $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture').data("bootstrapValidator");
        formValidation.enableFieldValidators('RadAnesthesia', true);
        if (Newrow != true)
            $("#" + EncounterChargeCapture.params.PanelID + " #divChargeDetails #radAnesthesiologist").trigger("click");
    }
    else {
        $('#' + EncounterChargeCapture.params.PanelID).find('#dgvVisitCharge .anesthesiarow').hide();
    }
    $('#' + EncounterChargeCapture.params.PanelID).find('#dgvVisitCharge .anesthesiarow').find('input:text[id*="txtTimeUnits"]').prop('disabled', true);
    $('#' + EncounterChargeCapture.params.PanelID).find('#dgvVisitCharge .anesthesiarow').find('input:text[id*="txtTotalMinutes"]').prop('disabled', true);
    $('#' + EncounterChargeCapture.params.PanelID).find('#dgvVisitCharge .anesthesiarow').find('input:text[id*="txtTotalUnits"]').prop('disabled', true);

},
BuildICDPointersArray: function (self) {
    var array = EncounterChargeCapture.ICDPointersArray;
    self.find("input[id*='txtICDPointer']]").each(function () {
        if ($(this).attr("id")) {
            objICD["id"] = $(this).attr("id");
            objICD["value"] = $(this).val();
            array.push(objICD);
        }
    });
    EncounterChargeCapture.ICDPointersArray = array;
    var array = EncounterChargeCapture.ICDPointersArray;
    //Reset All controls.
    self.find("input[id*='txtICDPointer']]").val('');
    for (var i = 0; i < array.length; i++) {
        var IcdIndex = (parseInt(i) + 1);
        var nextVal = array[i]
        self.find("#" + IcdIndex).val(array[i].value);
        self.find("#hfICD" + IcdIndex).val(array[i].hfICD);
        self.find("#hfICD10" + IcdIndex).val(array[i].hfICD10);
        self.find("#hfICDDescription" + IcdIndex).val(array[i].hfICDDescription);
        self.find("#hfSNOMED" + IcdIndex).val(array[i].hfSNOMED);
        self.find("#hfSNOMEDDescription" + IcdIndex).val(array[i].hfSNOMEDDescription);
        self.find("#txtICD10Description" + IcdIndex).val(array[i].txtICD10Description);
    }

},
calculateAge: function (DateOfBirth) {
    // var $surgeryDateContext = $('#' + Clinical_SurgicalHx.params.PanelID + '  section#sectionSurgicalDetails div#SurgicalDetails #dtpSurgicalSurgeryDate');
    //  var surgeryDate = $surgeryDateContext.val();
    var TodayDate = new Date();
    var Totalyears = "";
    if (TodayDate != "") {
        var second = 1000;
        var minute = second * 60;
        var hour = minute * 60;
        var day = hour * 24;
        var week = day * 7;
        var birthday = new Date(DateOfBirth);

        TodayDate = new Date(TodayDate)
        var timediff = TodayDate - birthday;
        var years = TodayDate.getFullYear() - birthday.getFullYear();
        var months = (TodayDate.getFullYear() * 12 + TodayDate.getMonth()) - (birthday.getFullYear() * 12 + birthday.getMonth());
        var days = Math.floor(timediff / day);
        var hours = Math.floor(timediff / hour);
        var minutes = Math.floor(timediff / minute);
        var seconds = Math.floor(timediff / second);
        var weeks = Math.floor(timediff / week);
        var age = ~~((Date.now() - birthday) / (31557600000));

        diff = new Date(
        TodayDate.getFullYear() - birthday.getFullYear(),
        TodayDate.getMonth() - birthday.getMonth(),
        TodayDate.getDate() - birthday.getDate()
        );
        var ageInYears = diff.getYear();
        var ageInMonths = diff.getMonth();
        var ageInDays = diff.getDate();
        if (timediff == "0") {
            Totalyears += 1 + " day";
        }
        else {
            if (ageInYears > 0) {
                Totalyears = ageInYears + " years ";
            }
            if (ageInMonths > 0) {
                Totalyears += ageInMonths + " months ";
            }
            // uncomment below age in years
            // Faizan ameen Improvement EMR-1690
            // Dated: 25-oct-2016.

            if (ageInDays > 0) {
                Totalyears += ageInDays + " days";
            }
        }
        // Totalyears = (years - 1) + " years";
        var Age
        if (Totalyears.indexOf('years') > -1) {
            Age = Totalyears.substr(0, Totalyears.indexOf('ears'));
        }
        else if (Totalyears.indexOf('months') > -1) {
            Age = Totalyears.substr(0, Totalyears.indexOf('onths'));

        }
        else if (Totalyears.indexOf('days') > -1) {
            Age = Totalyears.substr(0, Totalyears.indexOf('ays'));

        }

    }
    if (TodayDate != "")



        $("#" + EncounterChargeCapture.params.PanelID + ' #txtAge').val(Age);

},
// imp-621
validateDrugCodeCostFee: function (obj) {

    if ($("#dgvVisitCharge_wrapper").find('input[id*="txtdrugCodeCost' + $(obj).attr("id") + '"]').val() > 0) {
        var fee = utility.convertToFigure($(obj).find('input[id*="txtTotalFEE"]').val());
        EncounterChargeCapture.HPCS = $(obj).find('input[id*="txtCPT"]').val();
        EncounterChargeCapture.HPCSCost = utility.convertToFigure($("#dgvVisitCharge_wrapper").find('input[id*="txtdrugCodeCost' + $(obj).attr("id") + '"]').val());
        //var HPCSCost = utility.convertToFigure($(obj).find('input[id*="txtdrugCodeCost708559"]').val());

        if (fee > EncounterChargeCapture.HPCSCost) {
            EncounterChargeCapture.IsFeeGreaterThanHPCSCost = true;
        }
        else {
            //return false;
            EncounterChargeCapture.IsFeeGreaterThanHPCSCost = false;
        }

        }
        return true;
    },
    LoadClaimSummary: function () {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'EncounterChargeCapture';
                params["VisitId"] = EncounterChargeCapture.params.VisitId;
                //params["PatientId"] = PatientId;
                LoadActionPan('EncounterClaimSummary', params);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },
    FillCaseDetail: function (CaseNumber) {
        EncounterChargeCapture.CaseDetailFill(CaseNumber).done(function (response) {
            if (response.status != false) {
                var CaseDetail = JSON.parse(response.CASELoad_JSON);
                if (CaseDetail.length == 0) {
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpUnableToWorkFrom").val("");
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpUnableToWorkTo").val("");
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpAdmissionDate").val("");
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpDischargeDate").val("");
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpInjuryDate").val("");
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpCurrentIllnessDate").val("");
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #txtState").val("");
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadOtherNo").prop("checked", true);
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadAutoNo").prop("checked", true);
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadEmploymentNo").prop("checked", true);
                }
                else {
                    $.each(CaseDetail, function (i, item) {
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpUnableToWorkFrom").val(utility.RemoveTimeFromDate(null, item.HCFAField16DateFrom));
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpUnableToWorkTo").val(utility.RemoveTimeFromDate(null, item.HCFAField16DateTo));
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpAdmissionDate").val(utility.RemoveTimeFromDate(null, item.HCFAField18DateFrom));
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpDischargeDate").val(utility.RemoveTimeFromDate(null, item.HCFAField18DateTo));
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpCurrentIllnessDate").val(utility.RemoveTimeFromDate(null, item.InjuryDate));

                    if (item.EmploymentRelate == "True") {
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadEmploymentYes").prop("checked", true);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadEmploymentNo").prop("checked", false);
                    }
                    else {
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadEmploymentYes").prop("checked", false);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadEmploymentNo").prop("checked", true);
                    }
                    if (item.Accident == "Auto") {
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadOtherYes").prop("checked", false);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadOtherNo").prop("checked", true);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadAutoYes").prop("checked", true);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpInjuryDate").val(utility.RemoveTimeFromDate(null, item.InjuryDate));
                    } else if (item.Accident == "Non Auto") {

                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadAutoYes").prop("checked", false);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadOtherYes").prop("checked", true);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadAutoNo").prop("checked", true);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpInjuryDate").val(utility.RemoveTimeFromDate(null, item.InjuryDate));
                    }
                    else {
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadOtherNo").prop("checked", true);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #RadAutoNo").prop("checked", true);
                        $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #dtpInjuryDate").val("");
                    }
                    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #claimAdditionalInfoDiv #txtState").val(item.State);
                });
            }
        }

    })
},
CaseDetailFill: function (CaseNumber) {
    var data = "CaseNumber=" + CaseNumber;
    return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "FILL_CASE_DETAIL");
},

    //UpdatePatientInsurances: function () {
    //    console.log('Upadte Button Working');
    //},
UpdatePatientInsurancesDetail: function () {
    utility.myConfirm('Do you want to Update Insurance Plan From Patient Demographics?', function () {
        EncounterChargeCapture.SyncClaimDetailWithDemographic();
    }, function () {
        return false;
    }, 'Update Insurance');
},

SyncClaimDetailWithDemographic: function ()
{
    EncounterChargeCapture.objsyncInsurance = $.Deferred();
    var PatientId = null;
    var VisitId = null;
    if (!EncounterChargeCapture.params["PatientId"]) {
        PatientId = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfPatientId").val();
    }
    else {
        PatientId = EncounterChargeCapture.params["PatientId"];
    }
    if (!EncounterChargeCapture.params["VisitId"]) {
        VisitId = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #hfVisitId").val();
    }
    else {
        VisitId = EncounterChargeCapture.params["VisitId"];
    }
    EncounterChargeCapture.UpdatePatientInsurances(PatientId, VisitId).done(function (response) {
        if (response.status != false) {
            utility.DisplayMessages(response.message, 1);
            EncounterChargeCapture.LoadPatientInsurances(PatientId, true);

            $.when(EncounterChargeCapture.objsyncInsurance).then(function () {
                var BillToPatient = self.find("#chkBillToPatient").is(':checked');

                $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tr").each(function () {
                    if ($(this).attr("id") && $(this).attr("id") != null) {
                        var $row = $(this);
                        INSCharges = parseFloat($row.find('input[id*="txtINSCharges"]').val() != "" ? $row.find('input[id*="txtINSCharges"]').val() : "0");
                        PatCharges = parseFloat($row.find('input[id*="txtPATCharges"]').val() != "" ? $row.find('input[id*="txtPATCharges"]').val() : "0");
                        if (BillToPatient) {
                            $row.find('input[id*="txtINSCharges"]').val(parseFloat(0).toFixed(globalAppdata.DecimalPlaces));
                            $row.find('input[id*="txtPATCharges"]').val(parseFloat(PatCharges + INSCharges).toFixed(globalAppdata.DecimalPlaces));
                        }
                        else {
                            $row.find('input[id*="txtINSCharges"]').val(parseFloat(INSCharges + PatCharges).toFixed(globalAppdata.DecimalPlaces));
                            $row.find('input[id*="txtPATCharges"]').val(parseFloat(0).toFixed(globalAppdata.DecimalPlaces));
                        }
                    }
                });
            });
        }
        else {
            utility.DisplayMessages(response.message, 3);
        }
    });
},
    CheckConflict: function (PatientId, VisitId) {
        var data = "PatientId=" + PatientId + "&VisitId=" + VisitId;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "CHECK_PATIENT_VISIT_INSURANCE");
    },
    UpdatePatientInsurances: function (PatientId, VisitId) {
        var data = "PatientId=" + PatientId + "&VisitId=" + VisitId;
        return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "UPDATE_PATIENT_VISIT_INSURANCE");
    },
    ShowHidInsuranceSyncButton: function () {
        if ($("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #lstVisitPlans li').length > 0) {
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #btnsync').show();
        } else {
            $("#" + EncounterChargeCapture.params["PanelID"] + ' #frmEncounterChargeCapture #btnsync').hide();
        }
    },
    BindCLIA: function (CLIA) {
        self.find("#hfProviderCLIA").val(CLIA);
    },
    ValidateChargeSearchScreen: function () {
        var CtrlId = "#pnlBillChargeSearch #frmChargeSearch";
        var isok = false;
        $(CtrlId).find('select,[type=text],[type=hidden]').each(function () {
            if ($(this).hasClass('ValidateCriteria') == true && $(this).val() != "") {
                if (this.tagName.toLowerCase() == "select" && ($(this).val().toLowerCase() == "all")) {
                    // ignore.
                }
                else {
                    isok = true;

            }
        }

    });
    return isok;
},
LoadCopayAlert: function () {
    EncounterChargeCapture.GetCopayAlert(EncounterChargeCapture.params["VisitId"]).done(function (response) {
        if (response.status == true && response.AlertCopayListCount > 0 && response.AlertCopayListInfo_JSON[0].RemainingBalance > 0) {
            var markUp = '<div id="modal-from-dom" class="modal fade ">' +
                         '<div class="modal-dialog modal-dialog-md modal-top-adjust">' +
                         '<div class="modal-content">' +
                         '<div class="modal-header">' +
                            '<label class="noWordBreak" style="font-size:14px;"><strong><span style="font-size:14px;color:#ff0000;">Alert! </span>Collect Copay</strong></label>' +
                         '</div>' +
                         '<div class="modal-body bg-white Of-a">' +
                          '<table id="PatientFutureAppointment" class="table table-bordered table-striped table-condensed table-hover mb-none no-footer dataTable">' +
                          '<thead>' +
                          '<tr>' +
                          '<th>Copayment</th>' +
                          '<th>Date</th>' +
                          '<th>Time</th>' +
                          '<th class="noWordBreak">Insurance Plan</th>' +
                          '<th>Provider</th>' +
                          '<th>Facility</th>' +
                          '</tr>' +
                          '</thead>' +
                          '<body>';
            $(response.AlertCopayListInfo_JSON).each(function (i, item) {
                markUp += '<tr>';
                markUp += '<td style=text-align:right;>' +utility.convertToFigure(item.RemainingBalance, true) + '</td>';
                markUp += '<td>' + utility.DateTemplate(item.AppointmentDate) + '</td>';
                markUp += '<td>' + item.AppointmentTime + '</td>';
                markUp += '<td>' + item.InsurancePlan + '</td>';
                markUp += '<td class="noWordBreak">' + item.ProviderName + '</td>';
                markUp += '<td class="noWordBreak">' + item.FacilityName + '</td>';
                markUp += '</tr>';
            });
            markUp += '</body>';
            markUp += '</table>';
            markUp += '</div>';
            markUp += '<div class="modal-footer">';
            markUp += '<a href="#" id="btnUpdate"   data-dismiss="modal" class="btn btn-success" onclick="EncounterChargeCapture.OpenPaymentPosting(null, true)" >Collect Copay</a>';
            markUp += '<a href="#" id="btnCancel"  data-dismiss="modal" class="btn btn-danger" onclick="" >Cancel</a>';
            markUp += '</div>';
            markUp += '</div> <div></div>';


            $(markUp).modal({
                show: 'true',
                backdrop: 'static',
                keyboard: false
            }).on('shown.bs.modal', function () {
                $('#btnCancel', this).focus();

            }).on('hidden.bs.modal', function () {
                //$('body').css('overflow', 'auto !important');
                if ($('body').find('.modal-backdrop').length > 0) {
                    if ($('body').css('overflow').toLowerCase() != "scroll") {
                        $('body').addClass('modal-open');
                    }
                    else {
                        $('body').addClass('modal-open');
                    }

                }
            });
        }
    });
},
GetCopayAlert: function (VisitId) {
    var data = "VisitId=" + VisitId;
    return MDVisionService.defaultService(data, "PATIENT_CHARGE_CAPTURE", "VISIT_APPOINTMENT_COPAY_ALERT");
},

OpenMultipleNDCSelection: function (NDCData, CtrlId) {
    var params = [];
    params["FromAdmin"] = "0";
    params["ParentCtrl"] = 'EncounterChargeCapture';
    params["NDCDetail"] = NDCData;
    params["CtrlId"] = CtrlId;
    LoadActionPan('Encounter_NDCSelection', params);
},
OnCptSelect: function (cptCode, CtrlId) {
    EncounterChargeCapture.SearchNDCDetail(0, "", cptCode).done(function (responseData) {
        if (responseData.status != false) {
            if (responseData.NDCCount > 1) {
                EncounterChargeCapture.OpenMultipleNDCSelection(responseData.NDCData, CtrlId);
            }
            else if (responseData.NDCCount == 1) {
                var childRow = $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvVisitCharge #Child" + CtrlId);
                EncounterChargeCapture.setNDCData(responseData, childRow);
            }
        }
    });
},
OnNDCInput:function(){

},
BindNDCAutoComplete: function (id) {
    var valid = false;
    var Ctrl = $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvVisitCharge #txtNDC" + id);
    var hfCtrl = null;
    var onChange = function () {
        var id_;
        var value_;
        var link = $(Ctrl).parent().parent().prev('a');
        var data = this.dataSource.data();
        var haveObject = data.filter(function (obj) {
            if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.NDCCode && obj.NDCCode.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                id_ = obj.id;
                value_ = obj.NDCCode;
                return true;
            }
            else { return false; }
        });
        if (haveObject.length > 0) {
            if (hfCtrl)
                $(hfCtrl).val(id_);
            this.value(value_);
            $(link).show();
            $(link).prev().hide();
        }

        //else {
        //    if (hfCtrl)
        //        $(hfCtrl).val('');
        //    this.value('');
        //    $(link).hide();
        //    $(link).prev().show();
        //    if ($(Ctrl).val() != "") {
        //        utility.DisplayMessages($(Ctrl).attr('customName') + " is not Valid", 2);
        //    }
        //}
    };
    var onSelect = function (e) {
        var dataItem = this.dataItem(e.item.index());
        Ctrl.val(dataItem.NDCCode);
        EncounterChargeCapture.SelectNDC(dataItem, id);
    }
    $(Ctrl).kendoAutoComplete({
        dataTextField: 'value',
        filter: 'contains',
        minLength: 3,
        select: onSelect,
        change: onChange,
        dataSource: {
            serverFiltering: true,
            transport: {
                read: function (e) {
                    EncounterChargeCapture.GetNDC(Ctrl.val(), id).done(function (response) {
                        e.success(response);
                    });
                },
            }
        },
    });
},
///// PRD-275 start
OpenPatientLedger: function () {
    var params = [];
    params["patientID"] = EncounterChargeCapture.params["patientID"];
    //if (EncounterChargeCapture.params["ParentCtrl"] == "billTabUnClaimedAppointment") {
    //    params["patientID"] = EncounterChargeCapture.params["PatientId"];
    //}
   
    if (!params["patientID"]) {
        params["patientID"] = EncounterChargeCapture.params["PatientId"];
    }
    

    params["PatientOutstanding"] = 1;
    params["IsFromCollection"] = false;
    params["ParentCtrl"] = "EncounterChargeCapture";
    LoadActionPan('Patient_Ledger', params);
},
//PRD-212
Scrub_Claims: function () {
    AppPrivileges.GetFormPrivileges("Encounter", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        if (strMessage == "") {
            var VisitId;
            if (EncounterChargeCapture.params["VisitId"])
            VisitId = EncounterChargeCapture.params["VisitId"];     
                Bill_ClaimSubmission.ScrubClaims(VisitId).done(function (response) {
                    if (response.status != false) {
                        EncounterChargeCapture.Load_ClaimErrors(EncounterChargeCapture.params["VisitId"]);
                   }
                    else {
                        utility.DisplayMessages(response.Message, 2);
                    }
                });
           
        } else {
            utility.DisplayMessages(strMessage, 2);
        }
    });
},
    ///// PRD-275 end
ExpandCollapseTabsByDropdown: function () {
    var selectedTab = $("#" + EncounterChargeCapture.params.PanelID + " #frmEncounterChargeCapture #ddlTablist option:selected").val();
    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture" + " #" + selectedTab).find("section").addClass("active");
    $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture" + " #" + selectedTab).find("section div:first").css("display", "block");
    document.getElementById(selectedTab).scrollIntoView();
},

OpenFollowUpComments: function (isEdit)
{
        var params = [];
        params["VisitId"] = EncounterChargeCapture.params["VisitId"];
        params["mode"] = "Add";
        params["IsFromClaim"] = true;
        params["ParentCtrl"] = "EncounterChargeCapture";
        params["FromAdmin"] = "0";
        LoadActionPan('Bill_FollowUpARComments', params);
},
FollowUpCommentLoad:function(VisitId){
    Bill_FollowUpARComments.FollowUpCommentsSearch(0, VisitId).done(function (response) {
        EncounterChargeCapture.FollowUpCommentGridLoad(response);
    });
},
FollowUpCommentGridLoad: function (response) {

       if ($.fn.dataTable.isDataTable("#" +EncounterChargeCapture.params["PanelID"]+ " #dgvClaimFollowUpComments")) {
            $("#" +EncounterChargeCapture.params["PanelID"]+ " #dgvClaimFollowUpComments").dataTable().fnClearTable();
            $("#" +EncounterChargeCapture.params["PanelID"]+ " #dgvClaimFollowUpComments").dataTable().fnDestroy();
        }
        $("#" +EncounterChargeCapture.params["PanelID"]+ " #dgvClaimFollowUpComments tbody").empty();
        if (response.FollowUpCommentInfoCount > 0) {
            $.each(response.FollowUpCommentInfo, function (i, item) {
                var MethodMode = "EncounterChargeCapture.FillFollowUpComment(" + item.Id + ")";
                var $row = $('<tr/>');          
                 $row.attr("Id", item.Id);
                 //$row.attr("onclick", MethodMode);
                 $row.append('<td><a class="btn  btn-xs" href="#" onclick="EncounterChargeCapture.DeleteFollowUpComment(\'' + item.Id + '\',event);" title="Delete Record"> <i class="fa fa-close red"></i></a><a class="btn  btn-xs" href="#"  onclick="EncounterChargeCapture.FillFollowUpComment(' + item.Id + ');"></a><a class="btn btn-xs" onclick="' + MethodMode + '" href="#" title="Edit Record"><i class="fa fa-edit black"></i></a></td><td>' + item.followUpComments + '</td><td>' + item.ModifiedOn + '</td><td>' + item.ModifiedBy + '</td>');
                 $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvClaimFollowUpComments tbody").last().append($row);
            });
        }
        else {
           
            $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvClaimFollowUpComments").DataTable({
                "language": {
                    "emptyTable": "No Comment Found"
                }, "autoWidth": false, "bLengthChange": false, "bInfo": false,"bPaginate": false,  "searching": false, "bFilter":false ,"aoColumnDefs": [{ "bSortable": false, "bPaginate": false, "aTargets": [1] }]
            });
        }
        
        if ($.fn.dataTable.isDataTable($("#" + EncounterChargeCapture.params["PanelID"] + " #dgvClaimFollowUpComments")))
            ;
        else
            $("#" + EncounterChargeCapture.params["PanelID"] + " #dgvClaimFollowUpComments").DataTable({ "bInfo": false, "searching": false,"bPaginate": false,"bFilter":false ,"bLengthChange": false, "autoWidth": false, "order": [[0, "desc"]], "aoColumnDefs": [{ "bSortable": true, "bPaginate": false, "aTargets": [1] }] }); // to remove records per page dropdown
},
FillFollowUpComment: function (FollowUpId) {
    var params = [];
    params["mode"] = "Edit";
    params["FollowUpCommentId"] = FollowUpId;
    params["VisitId"] = EncounterChargeCapture.params["VisitId"];
    params["ParentCtrl"] = "EncounterChargeCapture";
    LoadActionPan("Bill_FollowUpARComments",params);
},
DeleteFollowUpComment: function (Id, event) {
    event.stopPropagation();
    Bill_FollowUpARComments.params.VisitId = EncounterChargeCapture.params["VisitId"];
    Bill_FollowUpARComments.params.ParentCtrl = "EncounterChargeCapture";
        Bill_FollowUpARComments.DeleteFollowUpComments(Id);
    
},
///// PRD-275 end
OpenPatientDocument: function () {
    var params = [];
    params["ParentCtrl"] = "EncounterChargeCapture";
    params["PatientId"] = EncounterChargeCapture.params.patientID;
    params["FromAdmin"] = "0";
    params["PatientFullName"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #txtPatientFullName").val();
    params["dtpDOSFrom"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dtpDOSFrom").val();
    params["dtpDOSTo"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dtpDOSTo").val();
    params["AccountNumber"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #txtPatientName").val();
    params["ClaimNumber"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #txtClaimNumber").val();
    params["VisitId"] = EncounterChargeCapture.params.VisitId;
    LoadActionPan("Patient_Document", params);
},

ChargesRowsSortingOnDOS: function () {
    var IsDOSDifferent = 0;
    var rowObjectArr=[];
    EncounterChargeCapture.CPTCodeSortedArr = [];
    var DOS = $($("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tbody tr")[0]).find('input[id*=dtpDOSFrom]').val();
    var rowLength = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tbody").find('.adding').length;
    
    //to check DOS of all charges is different or same
    for (var i = 0; i < rowLength; i++) {
        var nextDOS = $($("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tbody").find('.adding')[i]).find('input[id*=dtpDOSFrom]').val();
        if (DOS != nextDOS) {
            IsDOSDifferent = 1;
            break;
        }
    }

    //maintain array of rows object
    for (var j = 0; j < rowLength; j++) {
        var objData={};
        rowChargeId = $($("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tbody").find('.adding')[j]).attr('id')
        objData["chargeRow"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tbody").find('.adding')[j];
        objData["childRow"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tbody").find('tr[id*=Child' + rowChargeId +']')[0];
        objData["anesthesiaRow"] = $("#" + EncounterChargeCapture.params["PanelID"] + " #frmEncounterChargeCapture #dgvVisitCharge tbody").find('tr[id*=Anesthesia' + rowChargeId +']')[0];
        rowObjectArr.push(objData);
    }

    if (IsDOSDifferent == 1) {//if DOS is different, then rows will sort in ascending order on the basis of DOS
        var sortedDOS = rowObjectArr.sort(function (PrevNextDate, currDate)
        {
            var dateCurr = new Date($(currDate.chargeRow).find('input[id*=dtpDOSFrom]').val()).getTime();
            var datePrevNext = new Date($(PrevNextDate.chargeRow).find('input[id*=dtpDOSFrom]').val()).getTime();
            return datePrevNext > dateCurr ? 1 : -1;
        });
        EncounterChargeCapture.CPTCodeSortedArr = sortedDOS;
    }
    else {//if DOS is same, E&M codes will show on top of all records and other cpts will show in descending order on the basis of billed column
        var EMCodesArr = $.grep(rowObjectArr, function (objData) {
            var CPTCode=$(objData.chargeRow).find('input[id*=txtCPT]').val();//.split(' ')[0];
            return ((parseInt(CPTCode) >= 99201 && parseInt(CPTCode) <= 99205) || (parseInt(CPTCode) >= 99211 && parseInt(CPTCode) <= 99215) || (parseInt(CPTCode) >= 99221 && parseInt(CPTCode) <= 99223));
        }).sort(function(PrevNextObj, CurrObj){
            var CurrBilledAmount = $(CurrObj.chargeRow).find('input[id*=txtTotalFEE]').val();
            var PrevNextBilledAmount = $(PrevNextObj.chargeRow).find('input[id*=txtTotalFEE]').val();
            return CurrBilledAmount - PrevNextBilledAmount;//CurrBilledAmount > PrevNextBilledAmount ? 1 : -1;
        });
        EncounterChargeCapture.CPTCodeSortedArr = EMCodesArr;

        var CPTCodesArr = $.grep(rowObjectArr, function (objData) {
            var CPTCode=$(objData.chargeRow).find('input[id*=txtCPT]').val();//.split(' ')[0];
            return !((parseInt(CPTCode) >= 99201 && parseInt(CPTCode) <= 99205) || (parseInt(CPTCode) >= 99211 && parseInt(CPTCode) <= 99215) || (parseInt(CPTCode) >= 99221 && parseInt(CPTCode) <= 99223));
        }).sort(function(PrevNextObj, CurrObj){
            var CurrBilledAmount = $(CurrObj.chargeRow).find('input[id*=txtTotalFEE]').val();
            var PrevNextBilledAmount = $(PrevNextObj.chargeRow).find('input[id*=txtTotalFEE]').val();
            return CurrBilledAmount - PrevNextBilledAmount;//CurrBilledAmount > PrevNextBilledAmount ? 1 : -1;
        });

        for (var i = 0; i < CPTCodesArr.length; i++)
            EncounterChargeCapture.CPTCodeSortedArr.push(CPTCodesArr[i]);
    }

    //set charge order in rows according to CPT sorting rule
    var chargeOrder = 1;
    for (var ind = 0; ind < EncounterChargeCapture.CPTCodeSortedArr.length; ind++) {
        $(EncounterChargeCapture.CPTCodeSortedArr[ind].chargeRow).find('input[id*=txtChargeOrder]').val(chargeOrder);
        chargeOrder++;
    }
},
}