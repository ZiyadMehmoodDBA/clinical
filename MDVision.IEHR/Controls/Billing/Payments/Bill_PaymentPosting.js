Bill_PaymentPosting = {
    bIsFirstLoad: true,
    //bVisitFirst: true,
    strErrorMessage: "",
    showAlertMessage: true,
    // PMS-5483 -for insurance plan dropdown selection
    isFirstVisit: true,
    previousSelectCharge: "",
    previousVisit: "",
    selectedInsurancePlan: "",
    IsFromSave: false,
    //end -- PMS-5483
    params: [],
    AllNextResponsibilities: [],
    AllAdvancePayments: [],
    AllLedgerAccounts: [],
    zeroBilledAmount: false,

    Load: function (params) {
        Bill_PaymentPosting.zeroBilledAmount = false;
        Bill_PaymentPosting.showAlertMessage = true;
        Bill_PaymentPosting.AllNextResponsibilities = [];
        Bill_PaymentPosting.params = params;
        if (Bill_PaymentPosting.bIsFirstLoad) {
            //PMS-5483
            Bill_PaymentPosting.isFirstVisit = true;
            Bill_PaymentPosting.previousSelectCharge = "";
            Bill_PaymentPosting.previousVisit = "";
            Bill_PaymentPosting.selectedInsurancePlan = "";
            Bill_PaymentPosting.IsFromSave = false;
            //PMS-5483
            Bill_PaymentPosting.bIsFirstLoad = false;

            if (Bill_PaymentPosting.params.ParentCtrl != "billTabPaymentBatchSearch" && Bill_PaymentPosting.params.ParentCtrl != "Bill_ChargeSearch" && Bill_PaymentPosting.params.ParentCtrl != "billTabChargeSearch" && Bill_PaymentPosting.params.ParentCtrl != "paymentBatchDetail" && Bill_PaymentPosting.params.ParentCtrl != "ChargeBatch_Viewer" && Bill_PaymentPosting.params.ParentCtrl != "Bill_FollowUpPatientAR_Detail" && Bill_PaymentPosting.params.ParentCtrl != "Bill_FollowUpInsuranceAR_Detail" && Bill_PaymentPosting.params.ParentCtrl != "ERADetail" && Bill_PaymentPosting.params.ParentCtrl != "Bill_ERA_Summary" && Bill_PaymentPosting.params.ParentCtrl != "Bill_InsurancePaymentByBatch" && Bill_PaymentPosting.params.ParentCtrl != "Bill_Insurance_Payment_Detail" && (Bill_PaymentPosting.params.TabID && Bill_PaymentPosting.params.TabID.indexOf("EncounterChargeCapture") && (Bill_PaymentPosting.params.ParentCtrl && Bill_PaymentPosting.params.ParentCtrl.indexOf("Bill_ERA_ElectronicEOB")) != 0)) {
                Bill_PaymentPosting.removeDialogClasses();
            }

            if (Bill_PaymentPosting.params.PanelID != "pnlBillPaymentPosting") {
                Bill_PaymentPosting.params.PanelID += " #pnlBillPaymentPosting";
            }

            if (Bill_PaymentPosting.params.ParentCtrl && (Bill_PaymentPosting.params.ParentCtrl.indexOf("EncounterChargeCapture") > -1 || Bill_PaymentPosting.params.ParentCtrl == "Bill_ChargeSearch" || Bill_PaymentPosting.params.ParentCtrl == "billTabChargeSearch" || Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpInsuranceAR_Detail" || Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpPatientAR_Detail" || Bill_PaymentPosting.params.ParentCtrl == "Bill_ERA_ElectronicEOB" || Bill_PaymentPosting.params.ParentCtrl == "Bill_Insurance_Payment_Detail") && Bill_PaymentPosting.params.VisitId != null) {
                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #divSearchCharges').css("display", "none");
                var ChargeId = Bill_PaymentPosting.params["ChargeId"];

                utility.ValidateFromToDate('' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting', 'dpDOSFrom', 'dpDOSTo', true);
                if ($('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #divSearchCharges').css('display') != "none") {
                    utility.AddDaysFromToDate('frmBillPaymentPosting', 'dpDOSFrom', 'dpDOSTo', 0, 0);
                }

                Bill_PaymentPosting.LoadCharges(Bill_PaymentPosting.params.VisitId);

            }
            else if (Bill_PaymentPosting.params.ParentCtrl == "billTabPaymentBatchSearch" || Bill_PaymentPosting.params.ParentCtrl == "paymentBatchDetail" || Bill_PaymentPosting.params.ParentCtrl == "ChargeBatch_Viewer") {
                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #divSearchCharges').css("display", "");
                $('#' + Bill_PaymentPosting.params.PanelID + ' #txtPaymentBatch').val(Bill_PaymentPosting.params.BatchNumber);
                $('#' + Bill_PaymentPosting.params.PanelID + ' #txtCheckNumber').val(Bill_PaymentPosting.params.CheckNumber);
                $('#' + Bill_PaymentPosting.params.PanelID + ' #dtpCheckDate').val(Bill_PaymentPosting.params.CheckDate);

                $('#' + Bill_PaymentPosting.params.PanelID + ' #hfPaymentBatch').val(Bill_PaymentPosting.params.BatchId);
                $('#' + Bill_PaymentPosting.params.PanelID + ' #divPaymentBatch').addClass('disableAll');

                utility.ValidateFromToDate('' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting', 'dpDOSFrom', 'dpDOSTo', true);
                if ($('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #divSearchCharges').css('display') != "none") {
                    utility.AddDaysFromToDate('frmBillPaymentPosting', 'dpDOSFrom', 'dpDOSTo', 0, 0);
                }
                Bill_PaymentPosting.LoadCharges();

            }
            else {

                // ERA Detail,Bill_ERA_Summary only have Charge Id so that's why handled separately.
                if (Bill_PaymentPosting.params["IsSearchCharges"] != true && Bill_PaymentPosting.params.ParentCtrl == "ERADetail" || Bill_PaymentPosting.params.ParentCtrl == "Bill_ERA_Summary")
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #divSearchCharges').css("display", "none");
                else
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #divSearchCharges').css("display", "");

                if (Bill_PaymentPosting.params["IsSearchCharges"] == true) {
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #txtClaimNumber').val(Bill_PaymentPosting.params["ClaimNumber"]);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #txtCheckNumber').val(Bill_PaymentPosting.params["CheckNumber"]);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #dtpCheckDate').val(Bill_PaymentPosting.params["CheckDate"]);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #dtpDatePaid').val(Bill_PaymentPosting.params["PaidDate"]);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #txtInsurancePaid').val(Bill_PaymentPosting.params["PaidAmount"]);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #txtInsuranceComments').val(Bill_PaymentPosting.params["Comments"]);
                }
                utility.ValidateFromToDate('' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting', 'dpDOSFrom', 'dpDOSTo', true);
                if ($('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #divSearchCharges').css('display') != "none") {
                    utility.AddDaysFromToDate('frmBillPaymentPosting', 'dpDOSFrom', 'dpDOSTo', 0, 0);
                }
                Bill_PaymentPosting.LoadCharges();
            }

            var self = $('#' + Bill_PaymentPosting.params.PanelID);

            //end syed zia 11-02-2016, bug #PMS-3900
            self.loadDropDowns(true).done(function () {

                utility.CreateDatePicker(Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #dtpCheckDate', function () {

                    // on-change callback method
                    if (Bill_PaymentPosting.params)
                        $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'checkDate');

                });

                utility.creditCardExpiryDate(Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #dtpExpiryDate', function () {

                    // on-change callback method
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'expiryDate');
                });

                // setting cash for payment type
                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(2).attr("selected", "selected");//.trigger('change');

                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').trigger('change');


                //$('#' + Bill_PaymentPosting.params.PanelID + ' #dtpDatePaid').datepicker('setEndDate', new Date());

                Bill_PaymentPosting.setInsurancePriority($('#' + Bill_PaymentPosting.params.PanelID + ' #ddlNextInsuranceInsurancePlan'));

                Bill_PaymentPosting.setInsurancePriority($('#' + Bill_PaymentPosting.params.PanelID + ' #ddlRecoupmentNextInsuranceInsurancePlan'));

                //**** start syed zia, 30-01-2016 bug #PMS-3734  **//

                if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #divInsurancePayment ").css('display') == "block") {

                    $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType option").each(function () {
                        if ($(this).text() == "Advance Payment") {
                            $(this).remove();

                        }
                    });

                }

                //**** end syed zia, 30-01-2016 bug #PMS-3734  **//
            });


            utility.CreateDatePicker(Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #dtpDatePaid', function () {

                // on-change callback method
                if (Bill_PaymentPosting.params)
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'DatePaid');

            }, true);


            //FIXME
            ////start syed zia 15-02-2016, bug #3727
            //utility.ValidateFromToDate('' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting', 'dpDOSFrom', 'dpDOSTo', true);
            ////end syed zia 15-02-2016, bug #3727


            //if ($('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #divSearchCharges').css('display') != "none") {
            //    utility.AddDaysFromToDate('frmBillPaymentPosting', 'dpDOSFrom', 'dpDOSTo', 0, 0);
            //}

            Bill_PaymentPosting.ValidatePaymentPosting();

            Bill_PaymentPosting.LoadPaymentBatch();
            Bill_PaymentPosting.LoadPatientCase();
            Bill_PaymentPosting.BindPatientAccount();

            if (Bill_PaymentPosting.params.ParentCtrl && (Bill_PaymentPosting.params.ParentCtrl.indexOf("EncounterChargeCapture") > -1 || Bill_PaymentPosting.params.ParentCtrl.indexOf("Bill_ChargeSearch") > -1)) {
                if (Bill_PaymentPosting.params.IsERAAttached == true) {
                    $('#' + Bill_PaymentPosting.params.PanelID + " #btnEOB").show();
                    $('#' + Bill_PaymentPosting.params.PanelID + " #btnEOB").removeClass("disableAll");
                }
                else {
                    $('#' + Bill_PaymentPosting.params.PanelID + " #btnEOB").show();
                    $('#' + Bill_PaymentPosting.params.PanelID + " #btnEOB").addClass("disableAll");
                }
            }
            else if (!Bill_PaymentPosting.params.ParentCtrl) {
                $('#' + Bill_PaymentPosting.params.PanelID +" #btnEOB").hide();
            }
        }
    },

    ValidateSearchCriteria: function (CallBack) {
        Bill_PaymentPosting.IsFromSave = false;
        Bill_PaymentPosting.selectedInsurancePlan = "";
        utility.ValidateSearchCriteria(Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting", function () {
            Bill_PaymentPosting.LoadCharges();
            if (Bill_PaymentPosting.params["IsFromCollectCopay"] == true) {
                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').find("#radPatient").trigger("click");
            }
        });
    },

    removeDialogClasses: function () {
        $('#' + Bill_PaymentPosting.params.PanelID + ' .modal-header').hide();
        // $('#' + Encounter_Visits.params.PanelID + ' #modalbody').removeClass('panel-body');
        $('#' + Bill_PaymentPosting.params.PanelID + ' .modal-content').removeClass('modal-content');
        $('#' + Bill_PaymentPosting.params.PanelID + ' .modal-dialog-full').removeAttr('class');
    },

    FillInsuranceLedgerAccount: function () {
        return false;
        if ($('#' + Bill_PaymentPosting.params.PanelID + ' #divInsurancePayment #ddlInsurancePaid option').length <= 0) {

            $.when(CacheManager.BindDropDownsByTwoIDs('#' + Bill_PaymentPosting.params.PanelID + ' #divInsurancePayment #ddlInsurancePaid', 'GetLedgerAccount', true, 2, 2).done(function () {

                $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentPaid').html($('#' + Bill_PaymentPosting.params.PanelID + ' #divInsurancePayment #ddlInsurancePaid').html());
                // $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentPaid').trigger('change');

                $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentPaid option').each(function () {

                    if ($(this).attr("RefValue") != null && $(this).attr("RefValue").toLowerCase() == "true") {
                        $(this).prop('selected', true);//.trigger('change');
                        $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentPaid').trigger('change');
                    }
                    else {
                        $(this).removeAttr("selected");
                    }
                });

            })).done(function () {
                CacheManager.BindDropDownsByTwoIDs('#' + Bill_PaymentPosting.params.PanelID + ' #divInsurancePayment #ddlInsuranceWriteoff', 'GetLedgerAccount', true, 3, 2).done(function () {

                    $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentWriteoff').html($('#' + Bill_PaymentPosting.params.PanelID + ' #divInsurancePayment #ddlInsuranceWriteoff').html());
                    //$('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentWriteoff').trigger('change');

                    $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentWriteoff option').each(function () {
                        if ($(this).attr("RefValue") != null && $(this).attr("RefValue").toLowerCase() == "true") {
                            $(this).prop('selected', true);
                            $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentWriteoff').trigger('change');
                        }
                        else {
                            $(this).removeAttr("selected");
                        }
                    });

                });
            });
        }


        $.when(CacheManager.BindDropDownsByTwoIDs('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentPaid', 'GetLedgerAccount', true, 2, 2).done(function () {


            $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentPaid option').each(function () {

                if ($(this).attr("RefValue") != null && $(this).attr("RefValue").toLowerCase() == "true") {
                    $(this).prop('selected', true);//.trigger('change');
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentPaid').trigger('change');
                }
                else {
                    $(this).removeAttr("selected");
                }
            });

        })).done(function () {
            CacheManager.BindDropDownsByTwoIDs('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentWriteoff', 'GetLedgerAccount', true, 3, 2).done(function () {


                $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentWriteoff option').each(function () {
                    if ($(this).attr("RefValue") != null && $(this).attr("RefValue").toLowerCase() == "true") {
                        $(this).prop('selected', true);
                        $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentWriteoff').trigger('change');
                    }
                    else {
                        $(this).removeAttr("selected");
                    }
                });

            });
        });

        //-----------------------------------
    },

    FillPatientLedgerAccount: function () {


        if ($('#' + Bill_PaymentPosting.params.PanelID + ' #divPatientPayment #ddlPatientPaid option').length <= 0) {
            return false;
            $.when(CacheManager.BindDropDownsByTwoIDs('#' + Bill_PaymentPosting.params.PanelID + ' #divPatientPayment #ddlPatientPaid', 'GetLedgerAccountForPatient', true, 1).done(function () {

                $('#' + Bill_PaymentPosting.params.PanelID + ' #divPatientPayment #ddlPatientPaid option').each(function () {
                    if ($(this).attr("RefValue") != null && $(this).attr("RefValue").toLowerCase() == "true" && $(this).val() == 3) {
                        $(this).prop('selected', true);
                    }
                    else {
                        $(this).removeAttr("selected");
                    }
                });

            })).done(function () {
                CacheManager.BindDropDownsByTwoIDs('#' + Bill_PaymentPosting.params.PanelID + ' #divPatientPayment #ddlPatientDiscount', 'GetLedgerAccountForPatient', true, 4).done(function () {

                    //$('#' + Bill_PaymentPosting.params.PanelID + ' #divPatientPayment #ddlPatientDiscount option').each(function () {
                    //    if ($(this).attr("RefValue") != null && $(this).attr("RefValue").toLowerCase() == "true") {
                    //        $(this).prop('selected', true);
                    //        $('#' + Bill_PaymentPosting.params.PanelID + ' #divPatientPayment #ddlPatientDiscount').trigger('change');
                    //    }
                    //    else {
                    //        $(this).removeAttr("selected");
                    //    }
                    //});

                });
            });
        }

        else {
            $('#' + Bill_PaymentPosting.params.PanelID + ' #divPatientPayment #ddlPatientPaid').val(3);
            $('#' + Bill_PaymentPosting.params.PanelID + ' #divPatientPayment #ddlPatientDiscount').val(6);

        }
    },



    FillInsuranceNextInsurancePlans: function (VisitId, InsuranceId, InsurancePlanName, IsTransferred) {
        // Bind Next Responsibility Drop Down for Insurance Payment
        //IMP-813

        CacheManager.BindPatientData('GetPatientVisitInsurance', true, VisitId, 1).done(function (result) {
            result = JSON.parse(result.GetPatientVisitInsurance);
            // Find Info of Insurance attached to selected Charge
            var found_names = $.grep(result, function (v) {
                return v.Name === InsurancePlanName && v.Value === InsuranceId;
            });

            var arrResult = result.sort(Bill_PaymentPosting.SortByPriority);
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #divInsurancePayment #ddlNextInsuranceInsurancePlan").empty();
            //-----------------
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #divRecoupmentPayment #ddlRecoupmentNextInsuranceInsurancePlan").empty();
            //-----------------

            //------Patient Insurance Plan-----------
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPatientInsurancePlan").empty();
            //-----------------
            var array = [];
            $.each(result, function (i, item) {

                if (item.Value != "") {
                    var objICD = new Object();
                    objICD["value"] = item.Value;
                    objICD["priority"] = item.RefValue;
                    objICD["copayment"] = item.RefName;
                    objICD["InsurancePlan"] = item.Name;
                    array.push(objICD);
                }
                // if ((found_names.length <= 0) || (found_names[0] && (found_names[0].RefValue < item.RefValue))) {
                if (item.RefValue != "") {
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #divInsurancePayment #ddlNextInsuranceInsurancePlan").append(
                    $('<option />', {
                        value: item.Value,
                        html: item.Name,
                        priority: item.RefValue,
                        coPayment: item.RefName
                    }));
                }
                // }
                Bill_PaymentPosting.AllNextResponsibilities = array;
                //---------------------
                if ((found_names.length <= 0) || (found_names[0] && (found_names[0].RefValue < item.RefValue))) {
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #divRecoupmentPayment #ddlRecoupmentNextInsuranceInsurancePlan").append(
                    $('<option />', {
                        value: item.Value,
                        html: item.Name,
                        priority: item.RefValue,
                        coPayment: item.RefName
                    }));
                }
                //---------------------

                //------Patient Insurance Plan-----------

                var HtmlText = "";
                if (item.RefValue != "") {
                    if (item.RefValue == "1") {
                        HtmlText = "P - " + item.Name;
                    }
                    else if (item.RefValue == "2") {
                        HtmlText = "S - " + item.Name;
                    }
                    else if (item.RefValue == "3") {
                        HtmlText = "T - " + item.Name;
                    }
                    else {
                        HtmlText = "SP - " + item.Name;
                    }
                }
                else {
                    HtmlText = item.Name;
                }

                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPatientInsurancePlan").append(
                $('<option />', {
                    value: item.Value,
                    html: HtmlText,
                    priority: item.RefValue,
                    coPayment: item.RefName,
                    insurancePlanId: item.ExValue
                }));
                //---------------------

            });


            var VisitInsurancePlan = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanId").val();

            if (Bill_PaymentPosting.IsFromSave == false && Bill_PaymentPosting.isFirstVisit == true || Bill_PaymentPosting.selectedInsurancePlan == "") {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPatientInsurancePlan option").each(function () {
                    if ($(this).attr("insurancePlanId") == VisitInsurancePlan) {
                        $(this).prop('selected', true);
                        var formValidation = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data("bootstrapValidator");
                        if (formValidation) {
                            formValidation.enableFieldValidators('PatientInsurancePlan', false);
                            $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'PatientInsurancePlan');
                        }
                    }

                    else {
                        $(this).removeAttr("selected");
                    }

                });
            }
            else {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPatientInsurancePlan option").each(function () {
                    if ($(this).attr("insuranceplanid") == Bill_PaymentPosting.selectedInsurancePlan) {
                        $(this).prop('selected', true);
                    }
                    else {
                        $(this).removeAttr("selected");
                    }
                });

                // $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPatientInsurancePlan").val(Bill_PaymentPosting.selectedInsurancePlan);
                var formValidation = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data("bootstrapValidator");
                if (formValidation) {
                    formValidation.enableFieldValidators('PatientInsurancePlan', false);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'PatientInsurancePlan');
                }
            }

            setTimeout(function () {
                var insuranceplan = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPatientInsurancePlan");
                Bill_PaymentPosting.SetPatientInsuranceInfo(insuranceplan);
                var strInsurancePlan = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlNextInsuranceInsurancePlan");
                var InsurancePlanPriority = $('option:selected', strInsurancePlan).attr('priority');
                $('#' + Bill_PaymentPosting.params.PanelID + ' #txtPlanPriority').val(InsurancePlanPriority);

            }, 100);


            if (IsTransferred != null && IsTransferred == true) {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #divInsurancePayment #ddlNextInsuranceInsurancePlan").attr("disabled", true);
                //------------------
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #divRecoupmentPayment #ddlRecoupmentNextInsuranceInsurancePlan").attr("disabled", true);
                //------------------
            } else {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #divInsurancePayment #ddlNextInsuranceInsurancePlan").attr("disabled", false);
                //------------------
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #divRecoupmentPayment #ddlRecoupmentNextInsuranceInsurancePlan").attr("disabled", false);
                //------------------
            }

            // when there is no next insurance plan available select Patient as next responsibility. http://192.168.0.16:8080/browse/PMS-2855
            //******start syed zia, 30-01-2016, bug #PMS-3722 ***/
            if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #divInsurancePayment ").css('display') == "block") {

                if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #divInsurancePayment #ddlNextInsuranceInsurancePlan").find("option").length <= 0)
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #divInsurancePayment #RadInsuranceNextRespPatient").click();
                else
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #divInsurancePayment #RadInsuranceNextRespInsurance").click();
            }
            else if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #divPatientPayment ").css('display') == "block") {
                Bill_PaymentPosting.calculateBalance("Patient");
            }
            //else if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #divCopaymentPayment ").css('display') == "block") {
            //    Bill_PaymentPosting.calculateBalance("Copay");
            //}
            //******end syed zia, 30-01-2016, bug #PMS-3722 ***/



            //******start irfan ***/
            if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #divRecoupmentPayment ").css('display') == "block") {

                if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #divRecoupmentPayment #ddlRecoupmentNextInsuranceInsurancePlan").find("option").length <= 0)
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #divRecoupmentPayment #RadRecoupmentNextRespPatient").click();
                else
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #divRecoupmentPayment #RadRecoupmentNextRespInsurance").click();
            }
            else if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #divPatientPayment ").css('display') == "block") {
                Bill_PaymentPosting.calculateBalance("Patient");
            }
            //else if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #divCopaymentPayment ").css('display') == "block") {
            //    Bill_PaymentPosting.calculateBalance("Copay");
            //}
            //******end irfan ***/

            var string = "";



        });
    },

    FillPatientNextResponsibilityPlan: function (InsuranceId, InsurancePlanName) {
        // Bind Next Responsibility Drop Down for Patient Payment
        var ddlNextPatientRespddl = $("#" + Bill_PaymentPosting.params["PanelID"] + " #divPatientPayment #ddlNextPatientInsurancePlan");
        ddlNextPatientRespddl.empty();
        ddlNextPatientRespddl.attr("disabled", "disabled");
        ddlNextPatientRespddl.append($('<option/>', {
            value: InsuranceId,
            html: InsurancePlanName
        }));
    },

    ControlBaseLoadCharges: function (controlName) {
        var currentCtrlVal = $("#frmBillPaymentPosting  #" + controlName).val();
        if (currentCtrlVal != "") {
            Bill_PaymentPosting.LoadCharges();
        }
    },

    OpenElectronicEOB: function () {

        var params = [];
        params["VisitId"] = Bill_PaymentPosting.params.VisitId;
        if (Bill_PaymentPosting.params.patientID) {
            params["patientID"] = Bill_PaymentPosting.params.patientID;
        }
       // params["ChargeCapId"] = ChargeCapIds == "" ? 0 : ChargeCapIds;
        //AST - 525
        params["ParentCtrl"] = "Bill_PaymentPosting";

        LoadActionPan('Bill_ERA_ElectronicEOB', params);

    },

    LoadCharges: function (VisitId, PageNo, rpp, RowId) {
        var strMessage = "";
        //fixme
        //AppPrivileges.GetFormPrivileges("Payment Posting", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {

        if (!PageNo) {
            Bill_PaymentPosting.params.CurrentPageNo = 1;
        }
        /****** commented by ali (review it again)
                        // Begin 06-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3120

                        if (Bill_PaymentPosting.params.PanelID != "pnlBillPaymentPosting")
                            return; //Bill_PaymentPosting.params.PanelID = "pnlBillPaymentPosting";

                        // End 06-Jan-2015  Added By Azeem Raza Tayyab Bug # PMS-3120
        *****/
        if ($('#' + Bill_PaymentPosting.params.PanelID + " #pnlPostingCharge_Result").css("display") == "none") {
            $('#' + Bill_PaymentPosting.params.PanelID + " #pnlPostingCharge_Result").show();
        }
        if (Bill_PaymentPosting.params.BatchId != null) {
            var BatchId = Bill_PaymentPosting.params.BatchId;

        }


        var self = $("#" + Bill_PaymentPosting.params["PanelID"]);
        var myJSON = self.getMyJSONByName();

        // ERA Detail,Bill_ERA_Summary only have Charge Id so that's why handled separately.
        var ChargeId = 0;

        if (Bill_PaymentPosting.params.ParentCtrl == "ERADetail" || Bill_PaymentPosting.params.ParentCtrl == "Bill_ERA_Summary") {
            ChargeId = Bill_PaymentPosting.params.ChargeId;
        }

        Bill_PaymentPosting.SearchChargeModel(myJSON, VisitId, ChargeId, PageNo, rpp).done(function (response) {
            if (response.status != false) {
                if (response.ChargeCount > 0) {
                    $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #txtInsuranceTransfer").trigger("keyup");
                    $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #txtPatientTransfer").trigger("keyup");

                    //------------Pagination-----------
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #divPostingChargePaging").css("display", "inline");
                    //Showing 1 to 15 of 15 entries
                    var RecordsPerPage = rpp != null ? rpp : 5;
                    var CurrentPage = PageNo != null ? PageNo : 1;

                    //params["myJSON"] = myJSON;

                    var PagesToShow = Math.ceil(response.iTotalDisplayRecords / RecordsPerPage);
                    var totalPagesToDisplay = PagesToShow > 0 ? PagesToShow : 1;
                    if (PageNo == null) {
                        if (Bill_PaymentPosting.params.PanelID != "pnlBillPaymentPosting") {
                            utility.GetCustomPaging(Bill_PaymentPosting.params.PanelID + " #divPostingChargePaging", response.iTotalDisplayRecords, 5, "Bill_PaymentPosting", CurrentPage, RecordsPerPage);
                        } else {
                            utility.GetCustomPaging("divPostingChargePaging", response.iTotalDisplayRecords, 5, "Bill_PaymentPosting", CurrentPage, RecordsPerPage);
                        }

                    }
                    var toRecords = (parseInt(CurrentPage) * parseInt(RecordsPerPage)) < response.iTotalDisplayRecords ? (parseInt(CurrentPage) * parseInt(RecordsPerPage)) : response.iTotalDisplayRecords;
                    var showingText = "Showing " + ((parseInt(CurrentPage) - 1) * parseInt(RecordsPerPage) + 1) + " to " + toRecords + " of " + response.iTotalDisplayRecords + " Record(s)";
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #divPostingChargePaging #divShowingEntries").text(showingText);
                    // Change Background Color to Black for selected page
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " li").each(function () {
                        if ($(this).text() == CurrentPage) {
                            $(this).attr("class", "active");
                        }
                        else

                            $(this).removeAttr("class");
                    });
                    //------------End Pagination-------
                }
                else {
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #divPostingChargePaging").css("display", "none");
                }
                Bill_PaymentPosting.ChargesGridLoad(response, RowId);
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });


        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    ChargesGridLoad: function (response, RowId) {
        var tableCount = $("#" + Bill_PaymentPosting.params.PanelID + " div.table-responsive");

        for (var i = 0 ; i < tableCount.length - 1 ; i++) {
            $(tableCount[i]).removeClass('Of-a');
        }


        //utility.ClearFormValidation('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting');
        if (Bill_PaymentPosting.params.mode == null || Bill_PaymentPosting.params.mode == 'add')
            utility.ClearFormControls('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting div#ParentDiv');
        $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting div#ParentDiv").addClass("disableAll");
        $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #sectionChargeDetail,#sectionChargePaidPaymentDetail").removeClass("active");
        $("#" + Bill_PaymentPosting.params["PanelID"] + ' #frmBillPaymentPosting #sectionChargeDetail div:eq(0)').css({ 'display': "none" });
        $("#" + Bill_PaymentPosting.params["PanelID"] + ' #frmBillPaymentPosting #chargePaidDetailDiv div:eq(0)').css({ 'display': "none" });

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge tbody").empty();
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge").dataTable().fnDestroy();
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge tbody").find("tr").remove();
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge tfoot").find("tr").remove();


        if (response.ChargeCount > 0) {//response.ChargeCount
            var ChargeLoadJSONData = JSON.parse(response.ChargeLoad_JSON);
            Bill_PaymentPosting.loadAllLedgerAccountDropDowns(response);
            Bill_PaymentPosting.loadRemittanceCodesDropDowns(response);
            var claimTypeClass = "";
            var claimTitle = "";
            var $FooterRow = $('<tr class="bold black bg-default" style="text-align: right" />');
            var TtlBilledAmt = 0; var TtlExpectedFee = 0; var TtlUnits = 0; var TtlInsPaid = 0; var TtlInsWriteOff = 0; var TtlInsBalance = 0; var TtlPatPaid = 0; var TtlPatDiscount = 0; var TtlPatBalance = 0; var TtlBalance = 0;

            $.each(ChargeLoadJSONData, function (i, item) {
                //start bug #PMS - 5483
                if (i == 0) {
                    Bill_PaymentPosting.previousSelectCharge = item.ChargeCapId;
                    Bill_PaymentPosting.previousVisit = item.VisitId;

                }
                //end bug #PMS - 5483
                var $row = $('<tr />');
                $row.attr("onclick", "utility.SelectGridRow($('#gvPaymentPostingCharge_row" + item.ChargeCapId + "')); Bill_PaymentPosting.ChargePaymentAdd(" + item.ChargeCapId.trim() + ",'Add','" + item.PatientId + "','" + item.PatientInsuranceId + "','" + item.InsurancePlanId + "','" + item.InsurancePlanName + "','" + item.PlanPriority + "','" + item.VisitId + "','" + item.FacilityId + "','" + item.FacilityName + "','" + item.ProviderId + "','" + item.ProviderName + "','" + item.TotalBal + "','" + item.InsCharges + "','" + item.PatCharges + "','" + item.Copay + "','" + item.InsBalance + "','" + item.PatBalance + "','" + item.CopayBalance + "','" + parseFloat(item.Billed) + "','" + utility.RemoveTimeFromDate(null, item.DOSFrom) + "','" + item.IsTransferred + "','" + item.IsVoided + "','" + item.IsVNC + "');");
                $row.attr("id", "gvPaymentPostingCharge_row" + item.ChargeCapId);
                $row.attr("ChargeId", item.ChargeCapId);

                //if (item.IsActive == "True") {
                //    isactive = 0;
                //    activeRecord = "Active Record";
                //    tglclass = "fa fa-toggle-on green";
                //}
                //else {
                //    isactive = 1;
                //    activeRecord = "Inactive Record";
                //    tglclass = "fa fa-toggle-on red";
                //}

                //comment if condition against bug no # 895
                //if (item.IsPrimary == "False") {
                claimTypeClass = "bg-info";
                claimTitle = "Primary Claim";
                //}
                /* else if (item.IsPrimary == "1") {
                planPriorityClass = "bg-success";
                claimTitle = "Secondary Claim";
            }
            else if (item.IsPrimary == "2") {
                planPriorityClass = "";
                claimTitle = "Tertiary Claim";
            }
            else if (item.IsPrimary == "3") {
                planPriorityClass = "";
                claimTitle = "Supplementary claim";
            }*/
                //else {
                //    claimTypeClass = "";
                //    claimTitle = "Non Primary Claim";
                //}

                //var MethodMode = "";
                //var ActionBit = false;

                //var EditMethod = "Bill_PaymentPosting.ChargePaymentAdd(" + item.ChargeCapId.trim() + ",'Add','" + item.PatientId + "','" + item.PatientInsuranceId + "','" + item.InsurancePlanName + "','" + item.VisitId + "','" + item.FacilityId + "','" + item.FacilityName + "','" + item.ProviderId + "','" + item.ProviderName + "','" + item.TotalBal + "','" + item.InsCharges + "','" + item.PatCharges + "','" + item.Copay + "','" + item.InsBalance + "','" + item.PatBalance + "','" + item.CopayBalance + "','" + item.Fee + "');";

                //var ActiveInacvtiveMethod = "";//"Patient_Search.ActiveInactivePatient(" + item.PatientId.trim() + "," + isactive + ");";
                //var strAction = "";
                //strAction = '<a class="btn btn-xs" href="#sectionChargeDetail" onclick="' + EditMethod + '"  title="Edit Record"><i class="fa fa-edit black"></i></a><a class="btn  btn-xs mr-xs" href="#sectionChargeDetail" onclick="' + ActiveInacvtiveMethod + '" title="' + activeRecord + '"><i class="' + tglclass + '"></i></a>';

                // $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + strAction + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td>' + item.ClaimNumber + '</td><td>' + item.CPTCode + '</td><td>' + item.InsurancePlanName + '</td><td>' + item.Fee + '</td><td>' + item.TotalBal + '</td><td>0</td><td>0</td><td>0</td><td>' + item.PatChargeAmt + '</td><td>0</td><td>0</td><td>0</td><td>' + item.Copay + '</td><td>0</td><td>0</td><td>0</td><td></td>');
                var BilledAmount = parseFloat(item.Billed);
                TtlBilledAmt = parseFloat(TtlBilledAmt) + parseFloat(item.Billed);
                TtlExpectedFee = parseFloat(TtlExpectedFee) + parseFloat(item.ExpectedFee);
                TtlUnits = parseFloat(TtlUnits) + parseFloat(item.Units);
                TtlInsPaid = parseFloat(TtlInsPaid) + parseFloat(item.InsPaid);
                TtlInsWriteOff = parseFloat(TtlInsWriteOff) + parseFloat(item.InsWriteOff);
                TtlInsBalance = parseFloat(TtlInsBalance) + parseFloat(item.InsBalance);
                TtlPatPaid = parseFloat(TtlPatPaid) + (parseFloat(item.PatPaid) + parseFloat(item.CopayPaid));
                TtlPatDiscount = parseFloat(TtlPatDiscount) + (parseFloat(item.PatDiscount) + parseFloat(item.CopayDiscount));
                TtlPatBalance = parseFloat(TtlPatBalance) + (parseFloat(item.PatBalance) + parseFloat(item.CopayBalance));
                TtlBalance = parseFloat(TtlBalance) + parseFloat(item.TotalBal);
                //var BilledAmount = parseFloat(item.Fee) * parseFloat(item.Units);
                /**PMS-4546   instead of showing Fee show the Billed amount that was billed to the primary insu
                rance. (ALI AWAN) */

                $row.addClass(claimTypeClass);
                //<td>' + utility.convertToFigure(((parseFloat(item.Units) * parseFloat(item.Fee)) + parseFloat(item.Copay)), true) + '</td>
                //start syed zia, bug #PMS-1741
                // $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td title="' + claimTitle + '"><a href="#" onclick="Bill_PaymentPosting.LoadVisitDetail(' + item.VisitId.trim() + ',' + item.PatientId.trim() + ',event);"  title="View Claim Detail">' + item.ClaimNumber + '</a></td><td title="' + item.CPTDescription + '" data-toggle="tooltip" data-placement="right">' + item.CPTCode + '</td><td>' + utility.convertToFigure(BilledAmount, true) + '</td><td>' + utility.convertToFigure(item.ExpectedFee, true) + '</td><td>' + utility.convertToFigure(item.Units) + '</td><td>' + utility.convertToFigure(item.TotalBal, true) + '</td><td>' + utility.convertToFigure(item.InsCharges, true) + '</td><td>' + utility.convertToFigure(item.InsPaid, true) + '</td><td>' + utility.convertToFigure(item.InsWriteOff, true) + '</td><td>' + utility.convertToFigure(item.InsBalance, true) + '</td><td>' + utility.convertToFigure(item.PatCharges, true) + '</td><td>' + utility.convertToFigure(item.PatPaid, true) + '</td><td>' + utility.convertToFigure(item.PatDiscount, true) + '</td><td>' + utility.convertToFigure(item.PatBalance, true) + '</td><td>' + utility.convertToFigure(item.Copay, true) + '</td><td>' + utility.convertToFigure(item.CopayPaid, true) + '</td><td>' + utility.convertToFigure(item.CopayDiscount, true) + '</td><td>' + utility.convertToFigure(item.CopayBalance, true) + '</td>');
                var OpenClaimNumber = "";
                if (EMRUtility.CheckPnlOpen("pnlEncounterChargeCapture")) {
                    OpenClaimNumber = item.ClaimNumber;
                } else {
                    OpenClaimNumber = '<a href="#" onclick="Bill_PaymentPosting.LoadVisitDetail(' + item.VisitId.trim() + ',' + item.PatientId.trim() + ',event);"  title="View Claim Detail">' + item.ClaimNumber + '</a>';
                }

                $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td>' + utility.RemoveTimeFromDate(null, item.DOSFrom) + '</td><td title="' + claimTitle + '">' + OpenClaimNumber + '</td><td title="' + item.CPTDescription + '" data-toggle="tooltip" data-placement="right">' + item.CPTCode + '</td><td class="text-right">' + utility.convertToFigure(BilledAmount, true) + '</td><td class="text-right">' + utility.convertToFigure(item.ExpectedFee, true) + '</td><td>' + utility.convertToFigure(item.Units) + '</td><td class="text-right">' + utility.convertToFigure(item.InsPaid, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsWriteOff, true) + '</td><td class="text-right">' + utility.convertToFigure(item.InsBalance, true) + '</td><td class="text-right">' + utility.convertToFigure(Number(item.PatPaid) + Number(item.CopayPaid), true) + '</td><td class="text-right">' + utility.convertToFigure(Number(item.PatDiscount) + Number(item.CopayDiscount), true) + '</td><td class="text-right">' + utility.convertToFigure(Number(item.PatBalance) + Number(item.CopayBalance), true) + '</td><td class="text-right">' + utility.convertToFigure(item.TotalBal, true) + '</td>');


                //end syed zia, bug #PMS-1741

                if (item.HasError.toLowerCase() == "true") {
                    $($row).removeClass("active").removeClass("bg-info");
                    $($row).css("background", "#ff9999");
                }

                $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge tbody").last().append($row);
            });

            $FooterRow.append('<td colspan="3">Total:</td>' +
                '<td>' + utility.convertToFigure(TtlBilledAmt, true) + '</td>' +
                '<td>' + utility.convertToFigure(TtlExpectedFee, true) + '</td>' +
                '<td style="text-align: left;">' + utility.convertToFigure(TtlUnits, false) + '</td>' +
                '<td>' + utility.convertToFigure(TtlInsPaid, true) + '</td>' +
                '<td>' + utility.convertToFigure(TtlInsWriteOff, true) + '</td>' +
                '<td>' + utility.convertToFigure(TtlInsBalance, true) + '</td>' +
                '<td>' + utility.convertToFigure(TtlPatPaid, true) + '</td>' +
                '<td>' + utility.convertToFigure(TtlPatDiscount, true) + '</td>' +
                '<td>' + utility.convertToFigure(TtlPatBalance, true) + '</td>' +
                '<td>' + utility.convertToFigure(TtlBalance, true) + '</td>');

            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge tfoot").last().append($FooterRow);
        }
        else {

            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge tbody").empty();
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge").dataTable().fnDestroy();
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge tbody").find("tr").remove();

            $("#" + Bill_PaymentPosting.params["PanelID"] + " #divPostingChargePaging").css("display", "none");
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge").DataTable({
                "language": {
                    "emptyTable": "No Charges Found"
                }, "autoWidth": false, "bLengthChange": false, "bFilter": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            })
        }
        if ($.fn.dataTable.isDataTable("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge"))
            ;
        else

            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bSort": false, "iDisplayLength": 5, "bFilter": false, "aaSorting": [[1, "desc"]], "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        if (Bill_PaymentPosting.params.ChargeId != null) {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge #gvPaymentPostingCharge_row" + Bill_PaymentPosting.params.ChargeId).trigger("click");
        }
        else if (RowId != null) {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge tbody tr#" + RowId).trigger("click");
        }
        else {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingCharge_Result #dgvPostingCharge tbody tr:eq(0) ").click();
        }

        //// setting cash for payment type
        //$('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(1);

        //utility.CreateDatePicker(Bill_PaymentPosting.params.PanelID + ' #dtpDatePaid', function () {

        //    // on-change callback method
        //}, true);


    },

    ChargePaymentAdd: function (ChargeId, mode, PatientId, InsuranceId, InsurancePlanId, InsurancePlanName, PlanPriority, VisitId, FacilityId, FacilityName, ProviderId, ProviderName, TotalBalance, InsCharge, PatCharge, CopayCharge, InsBalance, PatBalance, CopayBalance, ChargeFee, DOS, IsTransferred, IsVoided, IsVNC) {

        Bill_PaymentPosting.zeroBilledAmount = false;
        if (ChargeFee == "0") {
            Bill_PaymentPosting.zeroBilledAmount = true;
        }
        //utility.ClearFormValidation('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting',false);
        if (IsVoided == "1" || IsVNC.toLowerCase() == "false") {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #btnSavePaymentPosting").addClass("disableAll");
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #btnDenialPaymentPosting").addClass("disableAll");
        }
        else {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #btnSavePaymentPosting").removeClass("disableAll");
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #btnDenialPaymentPosting").removeClass("disableAll");
        }
        if (Bill_PaymentPosting.previousVisit == VisitId) {
            if (Bill_PaymentPosting.previousSelectCharge == ChargeId) {
                Bill_PaymentPosting.isFirstVisit = true;
            }
            else if (Bill_PaymentPosting.previousSelectCharge != ChargeId) {

                Bill_PaymentPosting.isFirstVisit = false;
            }
        }
        else {
            Bill_PaymentPosting.previousVisit = VisitId;
            Bill_PaymentPosting.isFirstVisit = true;
            //In case of different visit reset the flag,its basically using for insurance plan.after save we will not change the dropdown value
            Bill_PaymentPosting.IsFromSave = false;
        }

        //start syed zia, bug #4376,bug #PMS-3900
        //in case of insurance balance > 0 or opening payment posting from patient AR patient radio button will be selected
        if ((parseInt(InsBalance) == 0 && parseInt(PatBalance) > 0) || Bill_PaymentPosting.params["ParentCtrl"] == "Bill_FollowUpPatientAR_Detail") {
            //  $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #radPatient').prop('checked', true);
            $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').find("#radPatient").trigger("click");
        }
        else {
            $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').find("#radInsurance").trigger("click");
        }
        //end syed zia, bug #4376,bug #PMS-3900
        $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting  #divInsurancePayment,#divPatientPayment").each(function () {
            $(this).find('[type=text], textarea').each(function () {
                $(this).val('');
            });
        });

        if (Bill_PaymentPosting.params.mode == null || Bill_PaymentPosting.params.mode == 'add')
            utility.ClearFormControls('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting div#ParentDiv');
        $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting div#ParentDiv").removeClass("disableAll");



        $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #sectionChargeDetail,#sectionChargePaidPaymentDetail").addClass("active");
        $("#" + Bill_PaymentPosting.params["PanelID"] + ' #frmBillPaymentPosting #sectionChargeDetail div:eq(0)').css({ 'display': "block" });
        $("#" + Bill_PaymentPosting.params["PanelID"] + ' #frmBillPaymentPosting #chargePaidDetailDiv div:eq(0)').css({ 'display': "block" });

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientId").val(PatientId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfChargeId").val(ChargeId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanPriority").val(PlanPriority);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfVisitPlanPriority").val(PlanPriority);

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfChargeTotalBalance").val(TotalBalance);

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsCharge").val(InsBalance);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatCharge").val(Number(PatBalance) + Number(CopayBalance));
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfCopayCharge").val(CopayBalance);

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsBalance").val(InsBalance);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatBalance").val(Number(PatBalance) + Number(CopayBalance));
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfCopayBalance").val(CopayBalance);

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfVisitId").val(VisitId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfFacilityId").val(FacilityId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtFacility").val(FacilityName);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfProviderId").val(ProviderId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtProvider").val(ProviderName);

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfChargeFee").val(ChargeFee);

        /***********/
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientInsurancePlanId").val(InsuranceId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanId").val(InsurancePlanId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanName").val(InsurancePlanName);

        if ((InsuranceId != null && InsuranceId > 0) && (parseInt(InsBalance) > 0)) {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #btnDenialPaymentPosting").attr("disabled", false);
        }
        else {

            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #btnDenialPaymentPosting").attr("disabled", true);
        }

        /*****start Syed zia 29-01-2016,bug PMS-3616 and PMS-9494****/
        if (InsurancePlanName == "") {

            // $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divCopaymentTransfer").addClass('disableAll');
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divPatientTransfers").addClass('disableAll');

        }
        else {
            //   $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divCopaymentTransfer").removeClass('disableAll');
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divPatientTransfers").removeClass('disableAll');

        }

        /*****end Syed zia 29-01-2016, bug PMS-3616 and PMS-9494****/
        // $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #radInsurance").trigger("click");

        Bill_PaymentPosting.params["mode"] = mode;
        // Show Charge Details
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #chargeDetailDiv").css("display", "");
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #chargePaidDetailDiv").css("display", "");
        //Bill_PaymentPosting.showAlertMessage = true;
        Bill_PaymentPosting.LoadChargesPaidPayment(ChargeId, IsVoided, IsVNC);

        //imp-813
        Bill_PaymentPosting.FillInsuranceNextInsurancePlans(VisitId, InsuranceId, InsurancePlanName, IsTransferred);
        // In case of Patient transfer, patientNextResponsibility needs to be null
        //therefore dropdown will be having no option and then get's hidden
        //Bill_PaymentPosting.FillPatientNextResponsibilityPlan(InsuranceId, InsurancePlanName);
        //  Bill_PaymentPosting.FillPatientNextResponsibilityPlan("", "");

        // setting cash for payment type


        if ($('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #radInsurance').prop('checked') == true) {

            $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(2).prop('selected', true).trigger('change');
        } else {
            $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(1).prop('selected', true).trigger('change');
        }



        $('#' + Bill_PaymentPosting.params.PanelID + ' #dtpDatePaid').datepicker('setStartDate', DOS);
        $('#' + Bill_PaymentPosting.params.PanelID + ' #dtpDatePaid').datepicker("setDate", $.datepicker.formatDate(date_format.replace('yy', ''), new Date()));
        if (Bill_PaymentPosting.params["IsFromCollectCopay"] == true) {
            $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').find("#radPatient").trigger("click");
        }
        //  Bill_PaymentPosting.BindAdvancePayment(PatientId);
    },

    SetPatientInsuranceInfo: function (obj) {

        var $obj = $("#" + $(obj).attr('id') + " option:selected");
        if ($obj.val() != "") {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientInsurancePlanId").val($obj.val());
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanId").val($obj.attr("insuranceplanid"));
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanName").val($obj.text());
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanPriority").val($obj.attr("priority"));
        }
        else {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientInsurancePlanId").val("");
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanId").val("");
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanName").val("");
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanPriority").val("");
            var formValidation = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data("bootstrapValidator");
            if (formValidation) {
                formValidation.enableFieldValidators('PatientInsurancePlan', true);
                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'PatientInsurancePlan');
            }
        }
        var priority = $obj.attr("priority");
        var VisitPlanPriority = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfVisitPlanPriority").val();
        Bill_PaymentPosting.ShowHideNextResponsibilities(priority, VisitPlanPriority);
    },

    ShowHideNextResponsibilities: function (SelectedPlanPriority, VisitPlanPriority) {


        if ($('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #radInsurance').prop('checked') == true) {
            var ddlPatientInsurance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlNextInsuranceInsurancePlan");
            var array = Bill_PaymentPosting.AllNextResponsibilities;
            if (array.length > 0 && SelectedPlanPriority && SelectedPlanPriority != array[array.length - 1].priority) {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #RadInsuranceNextRespInsurance").trigger("click");

                //if (SelectedPlanPriority == VisitPlanPriority) {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlNextInsuranceInsurancePlan option").each(function () {
                    if ($(this).attr("priority") == Number(SelectedPlanPriority) + 1) {
                        $(this).prop('selected', true);
                    }
                    else {
                        $(this).removeAttr("selected");
                    }

                });

                // var strInsurancePlan = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPatientInsurancePlan");
                // $('option:selected', strInsurancePlan).attr('priority');
                //// ddlPatientInsurance.find("option").remove();
                // $.each(array, function (i, item) {
                //     if (item.priority > SelectedPlanPriority && item.priority != SelectedPlanPriority) {
                //         var option = $("<option />");
                //         if (item.priority == (Number(SelectedPlanPriority) + 1)) {
                //             option.attr("selected", "selected");
                //         }
                //         option.attr("priority", item.priority);
                //         option.attr("copayment", item.copayment);
                //         option.attr("value", item.value);
                //         option.text($('<div/>').html(item.InsurancePlan).text());
                //         ddlPatientInsurance.append(option);
                //     }
                // });
                // }
            }
            else if (array.length > 0 && SelectedPlanPriority && SelectedPlanPriority == array[array.length - 1].priority) {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #RadInsuranceNextRespPatient").trigger("click");
            }
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfRecoupmentNextInsurancePlan").val("");
        }
        else if ($('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #radRecoupment').prop('checked') == true) {
            var array = Bill_PaymentPosting.AllNextResponsibilities;
            if (array.length > 0 && SelectedPlanPriority && SelectedPlanPriority != array[array.length - 1].priority) {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #RadRecoupmentNextRespInsurance").trigger("click");
                Bill_PaymentPosting.RecoupmentNextResponsabilityHfField();
                if (SelectedPlanPriority == VisitPlanPriority) {
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlNextInsuranceInsurancePlan option").each(function () {
                        if ($(this).attr("priority") == Number(SelectedPlanPriority) + 1) {
                            $(this).prop('selected', true);
                        }
                        else {
                            $(this).removeAttr("selected");
                        }

                    });

                }
            }
            else if (array.length > 0 && SelectedPlanPriority && SelectedPlanPriority == array[array.length - 1].priority) {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #RadRecoupmentNextRespPatient").trigger("click");
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfRecoupmentNextInsurancePlan").val("");
            }

        }

    },
    OpenPatientAlerts: function (patientID) {
        var params = [];
        var parentPanelId = null;
        params["patientID"] = patientID;

        //if (Bill_PaymentPosting.params.TabID == 'billTabPaymentBatchSearch' || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail' || Bill_PaymentPosting.params.TabID == 'Bill_ChargeSearch') {
        //    params["ParentCtrl"] = 'Bill_PaymentPosting';
        //} else {
        //    params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        //}
        if (Bill_PaymentPosting.params.TabID != null) {
            params["ParentCtrl"] = 'Bill_PaymentPosting';
            params["PaymentRef"] = Bill_PaymentPosting.params.PaymentRef;

            if (Bill_PaymentPosting.params["ParentCtrl"] != null)
                parentPanelId = GetTab(Bill_PaymentPosting.params["ParentCtrl"]).PanelID;
            // parentPanelId = "pnlBillChargeSearch";
        }

        else {
            params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        }

        //params["ParentCtrl"] = "patTabDemographic";
        LoadActionPan('Patient_MessageAlert', params, parentPanelId)
    },

    ValidateAllowedAmount: function (BalanceType) {
        var ChargeFee = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfChargeFee").val();
        ChargeFee = ChargeFee == "" ? 0 : ChargeFee;
        var InsBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsBalance").val();
        InsBalance = InsBalance == "" ? 0 : InsBalance;
        var ObjWriteOff = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceWriteoff");
        var ObjAllowed = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceAllowed");
        var AllowedAmount = ObjAllowed.val();
        AllowedAmount = AllowedAmount == "" ? 0 : AllowedAmount;
        AllowedAmount = parseFloat(AllowedAmount);
        if (AllowedAmount > 0 && InsBalance > 0) {
            if (AllowedAmount <= InsBalance) {
                ObjWriteOff.val(parseFloat(InsBalance - AllowedAmount).toFixed(Number(globalAppdata.DecimalPlaces)));
            }
            else {
                ObjAllowed.val("");
                ObjWriteOff.val("");
            }
        }
        else {
            ObjWriteOff.val("");
        }
    },

    ValidatePaymentBalance: function (BalanceType) {

        var InsCharge = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsCharge").val();
        InsCharge = InsCharge == "" ? 0 : InsCharge;
        var InsBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsBalance").val();
        InsBalance = InsBalance == "" ? 0 : InsBalance;

        var PatCharge = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatCharge").val();
        PatCharge = PatCharge == "" ? 0 : PatCharge;
        var PatBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatBalance").val();
        PatBalance = PatBalance == "" ? 0 : PatBalance;

        //var CopayCharge = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfCopayCharge").val();
        //CopayCharge = CopayCharge == "" ? 0 : CopayCharge;
        //var CopayBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfCopayBalance").val();
        //CopayBalance = CopayBalance == "" ? 0 : CopayBalance;


        if (BalanceType.toLowerCase() == "insurance") {

            var ObjPaid = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsurancePaid");
            var ObjWriteOff = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceWriteoff");
            var ObjCopay = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceCopay");

            var ObjcoInsurance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceCoinsurance");
            var ObjDeductables = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceDeductables");
            var ObjPatientResponsibility = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsurancePatientResponsibility");

            var ObjAllowed = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceAllowed");
            var AllowedAmount = ObjAllowed.val();
            var isValid = Bill_PaymentPosting.ValidateBalance(ObjPaid, ObjWriteOff, InsBalance, ObjCopay, "insurance", ObjcoInsurance, ObjDeductables, ObjPatientResponsibility);
            $.when(Bill_PaymentPosting.ValidateBalance(ObjPaid, ObjWriteOff, InsBalance, ObjCopay, "insurance", ObjcoInsurance, ObjDeductables, ObjPatientResponsibility)).done(function () {
                var PaidAmount = ObjPaid.val();
                PaidAmount = PaidAmount == "" ? 0 : PaidAmount;
                var WriteOffAmount = ObjWriteOff.val();
                WriteOffAmount = WriteOffAmount == "" ? 0 : WriteOffAmount;

                var CoInsuranceAmount = ObjcoInsurance.val();
                CoInsuranceAmount = CoInsuranceAmount == "" ? 0 : CoInsuranceAmount;

                var DeductablesAmount = ObjDeductables.val();
                DeductablesAmount = DeductablesAmount == "" ? 0 : DeductablesAmount;

                var PatientResponsibilityAmount = ObjPatientResponsibility.val();
                PatientResponsibilityAmount = PatientResponsibilityAmount == "" ? 0 : PatientResponsibilityAmount;




                var formValidation = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data("bootstrapValidator");
                if (WriteOffAmount > 0) {

                    //ObjWriteOff.focus();
                    //Bill_PaymentPosting.ValidatePaymentPosting();
                    //ObjWriteOff.blur(function () {
                    //    ObjWriteOff.focus();
                    //    //utility.ClearFormValidation('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting');
                    //    //formValidation.enableFieldValidators('InsuranceWriteoffAccount', true);
                    //    //Bill_PaymentPosting.ValidatePaymentPosting();
                    //}).blur();
                    ////alert("1111")
                }
                //else
                //    formValidation.enableFieldValidators('InsuranceWriteoffAccount', false);
                var CopayAmount = ObjCopay.val();
                CopayAmount = CopayAmount == "" ? 0 : CopayAmount;
                PaidAmount = parseFloat(PaidAmount);//(parseFloat(WriteOffAmount) + parseFloat(CopayAmount)) - parseFloat(PaidAmount)
                if (PaidAmount < 0) {
                    PaidAmount = 0 - (PaidAmount);
                }
                var CurrentInsBalance = parseFloat(InsBalance) - (parseFloat(PaidAmount) + parseFloat(WriteOffAmount) + parseFloat(CopayAmount) + parseFloat(CoInsuranceAmount) + parseFloat(DeductablesAmount) + parseFloat(PatientResponsibilityAmount));

                CurrentInsBalance = parseFloat(CurrentInsBalance).toFixed(Number(globalAppdata.DecimalPlaces));


                if (CurrentInsBalance < 0) {

                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceBalance").css('color', 'red');
                } else {
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceBalance").css('color', '#000');
                }

                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceBalance").val(CurrentInsBalance);
            });


            return isValid;
        }
        else if (BalanceType.toLowerCase() == "patient") {
            var ObjPaid = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientPaid");
            var ObjDiscount = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientDiscount");

            var objCopay = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopay");
            var ObjcoInsurance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientcoinsurance");
            var ObjDeductables = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientdeductables");
            var ObjPatientResponsibility = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientPatientResponsibility");

            var isValid = Bill_PaymentPosting.ValidateBalance(ObjPaid, ObjDiscount, PatBalance, objCopay, "patient", ObjcoInsurance, ObjDeductables, ObjPatientResponsibility);
            $.when(Bill_PaymentPosting.ValidateBalance(ObjPaid, ObjDiscount, PatBalance, objCopay, "patient", ObjcoInsurance, ObjDeductables, ObjPatientResponsibility)).done(function () {
                var PaidAmount = ObjPaid.val();
                PaidAmount = PaidAmount == "" ? 0 : PaidAmount;

                var DiscountAmount = ObjDiscount.val();
                DiscountAmount = DiscountAmount == "" ? 0 : DiscountAmount;


                var CopayAmount = objCopay.val();
                CopayAmount = CopayAmount == "" ? 0 : CopayAmount;

                var CoInsuranceAmount = ObjcoInsurance.val();
                CoInsuranceAmount = CoInsuranceAmount == "" ? 0 : CoInsuranceAmount;

                var DeductablesAmount = ObjDeductables.val();
                DeductablesAmount = DeductablesAmount == "" ? 0 : DeductablesAmount;

                var PatientResponsibilityAmount = ObjPatientResponsibility.val();
                PatientResponsibilityAmount = PatientResponsibilityAmount == "" ? 0 : PatientResponsibilityAmount;




                var CurrentPatBalance = parseFloat(PatBalance) - (parseFloat(PaidAmount) + parseFloat(DiscountAmount) + parseFloat(CopayAmount) + parseFloat(CoInsuranceAmount) + parseFloat(DeductablesAmount) + parseFloat(PatientResponsibilityAmount));

                if (CurrentPatBalance < 0) {
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientBalance").css('color', 'red');
                } else {
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientBalance").css('color', '#000');
                }

                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientBalance").val(CurrentPatBalance.toFixed(2));

            });
            return isValid;
        }
        //else if (BalanceType.toLowerCase() == "copay") {
        //    var ObjPaid = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentPaid");
        //    var ObjDiscount = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentDiscount");
        //    var ObjCopay = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopay");
        //    var isValid = Bill_PaymentPosting.ValidateBalance(ObjPaid, ObjDiscount, CopayBalance, ObjCopay, BalanceType);
        //    $.when(Bill_PaymentPosting.ValidateBalance(ObjPaid, ObjDiscount, CopayBalance, BalanceType)).done(function () {
        //        var PaidAmount = ObjPaid.val();
        //        PaidAmount = PaidAmount == "" ? 0 : PaidAmount;
        //        var DiscountAmount = ObjDiscount.val();
        //        DiscountAmount = DiscountAmount == "" ? 0 : DiscountAmount;
        //        var CopayAmount = ObjCopay.val();
        //        CopayAmount = CopayAmount == "" ? 0 : CopayAmount;
        //        //  PaidAmount = parseFloat(PaidAmount) - parseFloat(DiscountAmount)
        //        //bug #PMS-4935
        //        PaidAmount = parseFloat(PaidAmount) + parseFloat(DiscountAmount) + parseFloat(CopayAmount);
        //        //bug #PMS-4935
        //        var CurrentCopayBalance = parseFloat(CopayBalance) - PaidAmount;

        //        if (CurrentCopayBalance < 0) {

        //            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentBalance").css('color', 'red');
        //        } else {
        //            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentBalance").css('color', '#000');
        //        }

        //        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentBalance").val(CurrentCopayBalance);
        //    });
        //    return isValid;
        //}
    },

    ValidateBalance: function (ObjPaid, ObjDiscount, totalCharge, ObjCopay, BalanceType, ObjcoInsurance, ObjDeductables, ObjPatientResponsibility) {
        var isValid = true;
        Bill_PaymentPosting.strErrorMessage = "";
        totalCharge = parseFloat(totalCharge).toFixed(Number(globalAppdata.DecimalPlaces));

        var ConInsuranceAmount = 0;
        var DeductablesAmount = 0;
        var PatientResponsibilityAmount = 0;

        var chargeCopayAmount;
        if (ObjPatientResponsibility != null) {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceCopay").val();
            PatientResponsibilityAmount = $(ObjPatientResponsibility).val();
            PatientResponsibilityAmount = PatientResponsibilityAmount == "" ? 0 : PatientResponsibilityAmount;
            PatientResponsibilityAmount = parseFloat(PatientResponsibilityAmount);
        }

        if (ObjcoInsurance != null) {
            ConInsuranceAmount = $(ObjcoInsurance).val();
            ConInsuranceAmount = ConInsuranceAmount == "" ? 0 : ConInsuranceAmount;
            ConInsuranceAmount = parseFloat(ConInsuranceAmount);
        }

        if (ObjDeductables != null) {
            DeductablesAmount = $(ObjDeductables).val();
            DeductablesAmount = DeductablesAmount == "" ? 0 : DeductablesAmount;
            DeductablesAmount = parseFloat(DeductablesAmount);
        }

        if (ObjPatientResponsibility != null) {
            PatientResponsibilityAmount = $(ObjPatientResponsibility).val();
            PatientResponsibilityAmount = PatientResponsibilityAmount == "" ? 0 : PatientResponsibilityAmount;
            PatientResponsibilityAmount = parseFloat(PatientResponsibilityAmount);
        }

        var PaidAmount = $(ObjPaid).val();
        PaidAmount = PaidAmount == "" ? 0 : PaidAmount;
        PaidAmount = parseFloat(PaidAmount);

        var DiscountAmount = $(ObjDiscount).val();
        DiscountAmount = DiscountAmount == "" ? 0 : DiscountAmount;
        DiscountAmount = parseFloat(DiscountAmount);

        var CopayAmount = 0;
        if (ObjCopay != null) {
            CopayAmount = $(ObjCopay).val();
            CopayAmount = CopayAmount == "" ? 0 : CopayAmount;
            CopayAmount = parseFloat(CopayAmount);
        }


        var totalPaidAmount = PaidAmount + DiscountAmount;
        var totalTransferAmount = CopayAmount + ConInsuranceAmount + DeductablesAmount + PatientResponsibilityAmount;
        // var TotalPaidTransferAmount = totalPaidAmount + totalTransferAmount;
        //start syed zia, bug #PMS-4746
        var TotalPaidTransferAmount = parseFloat(totalPaidAmount + totalTransferAmount).toFixed(Number(globalAppdata.DecimalPlaces));
        //end syed zia, bug #PMS-4746
        if (TotalPaidTransferAmount > 0) {
            var InsBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsBalance").val();
            InsBalance = parseFloat(InsBalance);
            InsBalance = InsBalance == "" ? 0 : InsBalance;
            //var PatBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatBalance").val();
            //PatBalance = PatBalance == "" ? 0 : parseFloat(PatBalance).toFixed(Number(globalAppdata.DecimalPlaces));

            var PatBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatBalance").val();
            PatBalance = PatBalance == "" ? 0 : parseFloat(PatBalance).toFixed(Number(globalAppdata.DecimalPlaces));

            var CopayBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfCopayBalance").val();
            CopayBalance = CopayBalance == "" ? 0 : parseFloat(CopayBalance).toFixed(Number(globalAppdata.DecimalPlaces));
            //if (BalanceType.toLowerCase() == "insurance" && TotalPaidTransferAmount > InsBalance) {
            //    Bill_PaymentPosting.strErrorMessage = "Total Paid/Transfer amount must be less than insurance balance";
            //    isValid = false;
            //}
            //else if (BalanceType.toLowerCase() == "patient" && TotalPaidTransferAmount > PatBalance) {
            //    Bill_PaymentPosting.strErrorMessage = "Total Paid/Transfer amount must be less than patient balance";
            //    isValid = false;
            //}
            if (totalPaidAmount > 0 && totalTransferAmount > 0) {
                if (BalanceType.toLowerCase() == "insurance") {

                    if (Number(parseFloat(TotalPaidTransferAmount).toFixed(Number(globalAppdata.DecimalPlaces))) > Number(parseFloat(InsBalance).toFixed(Number(globalAppdata.DecimalPlaces)))) {
                        Bill_PaymentPosting.strErrorMessage = "Paid/Writeoff amount must be less than insurance balance";
                        isValid = false;
                    }
                    else if (Number(parseFloat(TotalPaidTransferAmount).toFixed(Number(globalAppdata.DecimalPlaces))) > Number(parseFloat(InsBalance).toFixed(Number(globalAppdata.DecimalPlaces)))) {
                        Bill_PaymentPosting.strErrorMessage = "Transfer amount must be less than insurance balance";
                        isValid = false;
                    }
                    else {
                        isValid = true;
                    }
                }
                else if (BalanceType.toLowerCase() == "patient") {
                    ///bug#PMS-4970
                    if (Number(parseFloat(TotalPaidTransferAmount)) > Number(parseFloat(PatBalance))) {
                        ///bug#PMS-4970
                        Bill_PaymentPosting.strErrorMessage = "Paid/Discount amount must be less than patient balance";
                        isValid = false;
                    }
                    else if (Number(parseFloat(TotalPaidTransferAmount)) > Number(parseFloat(PatBalance))) {
                        Bill_PaymentPosting.strErrorMessage = "Transfer amount must be less than patient balance";
                        isValid = false;
                    }
                    else {
                        isValid = true;
                    }
                }

                else if (BalanceType.toLowerCase() == "copay") {
                    //bug#PMS-4970
                    if (Number(parseFloat(TotalPaidTransferAmount)) > Number(parseFloat(CopayBalance))) {
                        //bug#PMS-4970
                        Bill_PaymentPosting.strErrorMessage = "Transfer amount must be less than copay balance";
                        isValid = false;
                    }
                    else {
                        isValid = true;
                    }
                }
            }
            else if (totalTransferAmount > 0) {
                var TotalTransfer = CopayAmount + ConInsuranceAmount + DeductablesAmount + PatientResponsibilityAmount;

                if (BalanceType.toLowerCase() == "insurance") {

                    if (TotalTransfer > InsBalance) {
                        Bill_PaymentPosting.strErrorMessage = "Transfer amount must be less than insurance balance";
                        isValid = false;
                    }
                    else {
                        isValid = true;
                    }
                }
                else if (BalanceType.toLowerCase() == "patient") {

                    if (TotalTransfer > PatBalance) {
                        Bill_PaymentPosting.strErrorMessage = "Transfer amount must be less than patient balance";
                        isValid = false;
                    }
                    else {
                        isValid = true;
                    }
                }
                else if (BalanceType.toLowerCase() == "copay") {

                    if (TotalTransfer > CopayBalance) {
                        Bill_PaymentPosting.strErrorMessage = "Transfer amount must be less than copay balance";
                        isValid = false;
                    }
                    else {
                        isValid = true;
                    }
                }

            }
            else if (totalPaidAmount > 0) {
                var TotalPayment = PaidAmount + DiscountAmount;

                TotalPayment = parseFloat(utility.convertToFigure(TotalPayment));



                if (PaidAmount > totalCharge) {

                    Bill_PaymentPosting.strErrorMessage = "Paid amount must be less than balance";
                    $(ObjPaid).focus();
                    isValid = false;
                }


                else if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPaymentType").val() == 4) {

                    var advanceBalance = parseFloat($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfAdvanceBalance").val());
                    if (PaidAmount > advanceBalance) {

                        Bill_PaymentPosting.strErrorMessage = "Paid amount must be less than Advance Balance";
                        $(ObjPaid).focus();
                        isValid = false;
                    }
                }


                else if (DiscountAmount > totalCharge) {
                    if (BalanceType != null && BalanceType.toLowerCase() == "insurance") {
                        Bill_PaymentPosting.strErrorMessage = "WriteOff amount must be less than balance";
                    }
                    else
                        Bill_PaymentPosting.strErrorMessage = "Discount amount must be less than balance";

                    $(ObjDiscount).focus();
                    isValid = false;
                }
                else if (CopayAmount > 0 && CopayAmount > totalCharge) {
                    //$(ObjCopay).val("");
                    //utility.DisplayMessages("Must be less than balance", 2);
                    $(ObjCopay).focus();
                    isValid = false;
                }
                else if (TotalPayment > totalCharge) {
                    var GreaterVal = Bill_PaymentPosting.GetGreaterValue(PaidAmount, DiscountAmount);
                    if (ObjCopay != null) {
                        var GreaterVal2 = Bill_PaymentPosting.GetGreaterValue(CopayAmount, GreaterVal);
                        //utility.DisplayMessages("Must be less than balance", 2);
                        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting input[Value='" + GreaterVal2 + "']").focus();
                        isValid = false;
                    }
                    else {
                        //GreaterVal == PaidAmount ? $(ObjPaid).val("") : $(ObjDiscount).val("");

                        if (GreaterVal == PaidAmount) {
                            Bill_PaymentPosting.strErrorMessage = "Paid amount must be less than balance";
                            $(ObjPaid).focus();
                            isValid = false;
                        }
                        else {
                            if (BalanceType.toLowerCase() == "insurance") {
                                Bill_PaymentPosting.strErrorMessage = "WriteOff amount must be less than balance";
                            }
                            else
                                Bill_PaymentPosting.strErrorMessage = "Discount amount must be less than balance";
                            $(ObjDiscount).focus();
                            isValid = false;
                        }


                    }
                }
            }
            else {
                Bill_PaymentPosting.strErrorMessage = "Please specify payment amount";
                isValid = false;
            }
        }
        return isValid;
    },

    ValidateWriteOff: function () {
        var ObjAllowed = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceAllowed");
        var AllowedAmount = ObjAllowed.val();
        AllowedAmount = AllowedAmount == "" ? 0 : AllowedAmount;
        AllowedAmount = parseFloat(AllowedAmount);
        if (AllowedAmount > 0) {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceWriteoff").blur();
        }
    },

    GetGreaterValue: function (Val1, Val2) {
        if (Val1 > Val2) {
            return Val1;
        }
        else
            return Val2;
    },

    LoadChargesPaidPayment: function (ChargeId, IsVoided, IsVNC) {

        var strMessage = "";
        //AppPrivileges.GetFormPrivileges("Payment Posting", "SEARCH", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
        //    if (strMessage == "") {
        if ($('#' + Bill_PaymentPosting.params.PanelID + " #pnlPostingChargePaid_Result").css("display") == "none") {
            $('#' + Bill_PaymentPosting.params.PanelID + " #pnlPostingChargePaid_Result").show();
        }

        var self = $("#" + Bill_PaymentPosting.params["PanelID"]);
        var myJSON = self.getMyJSON();

        Bill_PaymentPosting.SearchChargePaidPayment(ChargeId).done(function (response) {
            if (response.status != false) {
                Bill_PaymentPosting.ChargePaidPaymentGridLoad(response, IsVoided, IsVNC);

                // Now getting Message count in Charge paid payment prodcedure thats why added code here .
                var PatMessageCount = JSON.parse(response.PatientMessageCount_JSON)[0].PatMessageCount;
                if (parseInt(PatMessageCount) > 0 && Bill_PaymentPosting.showAlertMessage == true) {

                    var dialogText = "Patient has <b>" + PatMessageCount + "</b> Financial Alerts";
                    var dialogTitle = "Patient Financial Alert";
                    CheckBoxLabel = "Don't show this Message Again";
                    var alertOKdontShowFunc = function () {

                        //$('#dontShowThis')
                        if ($(".dontShowThis" + ChargeId + "").last().prop('checked') == true) {
                            Bill_PaymentPosting.showAlertMessage = false;
                        }
                        else {
                            Bill_PaymentPosting.showAlertMessage = true;
                        }
                    };
                    var alertOKFunc = function () {

                        //OK FUNCTION


                    };
                    var alertShowScreenFunc = function () {
                        var patientId = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientId").val();
                        Bill_PaymentPosting.OpenPatientAlerts(patientId);
                    };

                    utility.alertDialog(dialogText, dialogTitle, CheckBoxLabel, alertOKdontShowFunc, alertOKFunc, alertShowScreenFunc, ChargeId);
                }

            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        //    }
        //    else
        //        utility.DisplayMessages(strMessage, 2);
        //});
    },

    ChargePaidPaymentGridLoad: function (response, IsVoided, IsVNC) {

        //var tableCount = $("#" + Bill_PaymentPosting.params.PanelID + " div.table-responsive");

        //for (var i = 0 ; i < tableCount.length - 1 ; i++) {
        //    $(tableCount[i]).removeClass('Of-a');
        //}

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid tbody").empty();
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid").dataTable().fnDestroy();
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid tbody").find("tr").remove();
        if (response.PatientPaymentsCount > 0) {
            var PatientPaymentsJSONData = JSON.parse(response.PatientPayments_JSON);
            if (PatientPaymentsJSONData[0].AccountNumber != "") {
                $.each(PatientPaymentsJSONData, function (i, item) {
                    var $row = $('<tr />');
                    $row.attr("onclick", "utility.SelectGridRow($('#dgvCopayment_row" + item.PaymentId + "'))");
                    $row.attr("id", "dgvCopayment_row" + item.PaymentId);
                    $row.attr("PaymentId", item.PaymentId);

                    var myAmount = item.PaidAmountCr != "" ? utility.convertToFigure(item.PaidAmountCr, true, true) : utility.convertToFigure(item.PaidAmountDr, true);

                    var refundMethod = "";

                    if (item.IsRefund.toLowerCase() == "true") {

                        refundMethod = '<i title="Amount Refunded" class="fa fa-money red ml-xs"></i>';
                    }
                    else if (item.IsRecoupment.toLowerCase() == "true") {
                        refundMethod = '<i title="Adjusted Amount" class="fa fa-money red ml-xs"></i>';
                    }
                    else if (utility.convertToFigure(item.PaidAmountDr) <= 0) {

                        refundMethod = '<i title="Zero Payment" class="fa fa-money red ml-xs"></i>';
                    }
                    else {
                        // AST - 467
                        if (IsVoided != "1" && IsVNC.toLowerCase() != "false") {
                            //bug# 679 - in case of transfer amount.ins to ins or ins to pat refund is not allowed.it will be recoup as per discussion with salman gillani
                            if (item.LedgerAccountId != 8 && item.LedgerAccountId != 9 && item.LedgerAccountId != 10 && item.LedgerAccountId != 14 && item.LedgerAccountId != 15 && item.LedgerAccountId != 16 && item.LedgerAccountId != 17 && item.LedgerAccountId != 18) {
                                refundMethod = '<a class="btn  btn-xs" href="#" onclick="Bill_PaymentPosting.RefundPayment(' + item.PaymentId + ');" title="Refund"><i class="fa fa-money"></i></a>';
                            }
                            else {
                                refundMethod = '<i class="fa fa-money" style="padding-left:3px"></i>';
                            }
                        }
                        else {
                            refundMethod = '<i class="fa fa-money" style="padding-left:3px"></i>';
                        }
                    }


                    // refund logic of transfer entry is same as rest of the entries
                    /******
                    if (item.Coinsurance != "" || item.Deductables != "" || item.PatientResponsibility != "") {

                            refundMethod = '<td><i class="fa fa-money green ml-xs" title="Amount Transfered"></i></td>';
                        }
                        else {

                        }

                        *******/

                    if (item.IsContractualAdj == "True") {
                        item.CreatedByName = 'Auto Contractual Adj';
                    }

                    $row.append('<td style="display:none;">' + item.PaymentId + '</td><td>' + refundMethod + '&nbsp;<a class="btn btn-xs" href="#" onclick="Bill_PaymentPosting.AccountLedgerEdit(' + item.PaymentId + ');"   title="Edit Record"><i class="fa fa-edit black"></i></a></td> <td>' + utility.RemoveTimeFromDate(null, item.CheckDate) + '</td><td class="text-right">' + myAmount + '</td><td>' + item.PmtTypeName + '</td><td>' + item.CheckNo + '</td><td>' + item.LedgerAccountName + '</td > <td>' + item.CreatedOn + ' </td> <td class="noWordBreak">' + item.CreatedByName + ' </td><td class="ellip150" data-toggle="tooltip" data-placement="left" title="' + item.Comments + '">' + item.Comments + '</td>');

                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid tbody").last().append($row);
                });
                // for show Primary Inurance first PMS-4430 
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid").DataTable({ "ordering": true, "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown               
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid #thAccount").click();

            }



        }
        else {

            // if (!$('#dgvPostingChargePaid').parent().parent().hasClass("dataTables_wrapper")) {
            if (!$("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid").parent().parent().hasClass("dataTables_wrapper")) {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid").DataTable({
                    "language": {
                        "emptyTable": "No Paid Payment(s) Found"
                    }, "autoWidth": false, "bLengthChange": false, "bPaginate": false, "bInfo": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                });
            }
            else {
                var $row = $('<tr/>');
                $row.append('<td colspan="12" class="center" > No Paid Payment(s) Found </td>');
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid tbody").last().append($row);
            }
        }
        //if ($.fn.dataTable.isDataTable("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid") || $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid").parent().parent().hasClass("dataTables_wrapper"))
        //    ;
        //else
        //    $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid").DataTable({ "bInfo": true, "bPaginate": true, "bLengthChange": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        //start To remove duplicate footer and filter #PMS-4600
        if ($('#' + Bill_PaymentPosting.params.PanelID + " #dgvPostingChargePaid_wrapper").find(".datatables-footer").length > 1) {
            $('#' + Bill_PaymentPosting.params.PanelID + " #dgvPostingChargePaid_wrapper").find(".datatables-footer").last().remove();
            $('#' + Bill_PaymentPosting.params.PanelID + " #dgvPostingChargePaid_wrapper").find(".Of-a").first().removeClass("Of-a");
        }

        if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid_wrapper").find("#dgvPostingChargePaid_filter").length > 1) {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #pnlPostingChargePaid_Result #dgvPostingChargePaid_wrapper").find("#dgvPostingChargePaid_filter").last().remove()
        }
        //end To remove duplicate footer and filter #PMS-4600
        //Set ToolTip for Comments.
        $('[data-toggle=tooltip],[rel=tooltip]').tooltip({ container: 'body' });
    },

    UpdateChargeAmount: function () {

    },

    AccountLedgerEdit: function (PaymentId) {
        var selectedValue = PaymentId;
        var parentPanelId = null;
        if (selectedValue == "" || selectedValue == "undefined") {
        }
        else {
            var params = [];
            params["PaymentId"] = selectedValue;
            params["mode"] = "Edit";

            if (Bill_PaymentPosting.params.TabID == "EncounterChargeCapture" || Bill_PaymentPosting.params.TabID == "billTabPaymentBatchSearch" || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail' || Bill_PaymentPosting.params.TabID == 'Bill_ChargeSearch' || Bill_PaymentPosting.params.TabID == "Bill_FollowUpPatientAR_Detail" || Bill_PaymentPosting.params.TabID == "ERADetail" || Bill_PaymentPosting.params.TabID == "Bill_ERA_Summary" || Bill_PaymentPosting.params.TabID == "Bill_FollowUpInsuranceAR_Detail") {
                params["ParentCtrl"] = 'Bill_PaymentPosting';
                params["PaymentRef"] = Bill_PaymentPosting.params.PaymentRef;

                if (Bill_PaymentPosting.params["ParentCtrl"] != null)
                    parentPanelId = GetTab(Bill_PaymentPosting.params["ParentCtrl"]).PanelID;
                // parentPanelId = "pnlBillChargeSearch";
            }

            else {
                params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
            }
            /*******/

            LoadActionPan('BillLedgerDetail', params, parentPanelId);
            //LoadActionPan('userDetail', params);
        }
    },

    SortByPriority: function (a, b) {
        var aPriority = a.RefValue;
        var bPriority = b.RefValue;
        return ((aPriority < bPriority) ? -1 : ((aPriority > bPriority) ? 1 : 0));
    },

    ValidateTransferAmount: function (obj, transferType) {
        var divId = "";

        var copay = 0;
        var coinsurance = 0;
        var deductables = 0
        var patientResponsibility = 0;

        if (transferType.toLowerCase() == "insurance") {
            coinsurance = Bill_PaymentPosting.ConvertToFloat($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceCoinsurance").val());
            deductables = Bill_PaymentPosting.ConvertToFloat($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceDeductables").val());
            patientResponsibility = Bill_PaymentPosting.ConvertToFloat($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsurancePatientResponsibility").val());

        }
        else if (transferType.toLowerCase() == "patient") {
            copay = Bill_PaymentPosting.ConvertToFloat($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopay").val());
            coinsurance = Bill_PaymentPosting.ConvertToFloat($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientcoinsurance").val());
            deductables = Bill_PaymentPosting.ConvertToFloat($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientdeductables").val());
            patientResponsibility = Bill_PaymentPosting.ConvertToFloat($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientPatientResponsibility").val());


        }


        //var TransferAmount = String(parseFloat($(obj).val()));
        var TransferAmount = copay + coinsurance + deductables + patientResponsibility;


        var InsBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsBalance").val();
        InsBalance = InsBalance == "" ? 0 : InsBalance;

        var PatBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatBalance").val();
        PatBalance = PatBalance == "" ? 0 : PatBalance;

        //var CopayBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfCopayBalance").val();
        //CopayBalance = CopayBalance == "" ? 0 : CopayBalance;

        if (transferType.toLowerCase() == "insurance") {
            divId = "InsuranceTransfer";
            var NextRespInsurance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #RadInsuranceNextRespInsurance").prop("checked");
            if (NextRespInsurance == false && parseFloat(TransferAmount) > parseFloat(PatBalance)) {
                //$(obj).val("");
            }
            else if (NextRespInsurance == true && parseFloat(TransferAmount) > parseFloat(InsBalance)) {

            }
        }
        else if (transferType.toLowerCase() == "patient") {
            divId = "PatientTransfer";
            var NextRespInsurance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #RadPatNextRespInsurance").prop("checked");
            if (NextRespInsurance == false && parseFloat(TransferAmount) > parseFloat(PatBalance)) {

            }
            else if (NextRespInsurance == true && parseFloat(TransferAmount) > parseFloat(InsBalance)) {
                //$(obj).val("");
            }
        }

        if (divId != "" && TransferAmount != "") {

            $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting div[divtype*='" + divId + "']").each(function () {
                if (TransferAmount > 0) {
                    //$(this).css("display", "");
                    $(this).find("#RadInsuranceNextRespPatient").prop("disabled", false);
                    $(this).find("#RadInsuranceNextRespInsurance").prop("disabled", false);
                    //  $(this).find("#ddlNextInsuranceInsurancePlan").prop("disabled", false);

                    $(this).find("#RadPatNextRespPatient").prop("disabled", true);
                    $(this).find("#RadPatNextRespInsurance").prop("disabled", false);
                    $(this).find("#ddlNextPatientInsurancePlan").prop("disabled", false);
                    $(this).find("#chkInsuranceCrossOver").prop("disabled", false);
                    //$(this).find("#ddlNextPatientInsurancePlan").val("");
                }
                else {
                    // Reset Insurance Plan Selection
                    //$(this).find("#ddlNextInsuranceInsurancePlan").val("");
                    //$(this).find("#ddlNextPatientInsurancePlan").val("");
                    $(this).find("#RadInsuranceNextRespPatient").prop("disabled", true);
                    $(this).find("#RadInsuranceNextRespInsurance").prop("disabled", true);
                    // $(this).find("#ddlNextInsuranceInsurancePlan").prop("disabled", true);
                    $(this).find("#chkInsuranceCrossOver").prop("disabled", true);

                    $(this).find("#RadPatNextRespPatient").prop("disabled", true);
                    $(this).find("#RadPatNextRespInsurance").prop("disabled", true);
                    $(this).find("#ddlNextPatientInsurancePlan").prop("disabled", true);
                    //$(obj).val("");
                }
            });
        }
        Bill_PaymentPosting.calculateBalance(transferType);
    },

    ValidatePaymentPosting: function (PaymentType) {
        utility.ClearFormValidation('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting');

        if ($('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data('bootstrapValidator') != null && typeof $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data('bootstrapValidator') != 'undefined') {
            $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data('bootstrapValidator').destroy();
        }

        $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   DatePaid: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },


                   PaymentType: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },


                   AdvancePayment: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   InsurancePaid: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   InsurancePaidAccount: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   InsuranceWriteoff: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   InsuranceWriteoffAccount: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PatientPaid: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PatientPaidAccount: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PatientDiscount: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PatientDiscountAccount: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CopaymentPaid: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CopaymentPaidAccount: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CopaymentDiscount: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CopaymentDiscountAccount: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   CrossOver1: {
                       group: '.col-sm-4',
                       enabled: true,
                       validators: {
                           choice: {
                               message: ''
                           }
                       }
                   },
                   InsuranceTransfer: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   NextInsuranceInsurancePlan: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PatientInsurancePlan: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   //checkNumber: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},

                   //checkDate: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},

                   //creditCardType: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},

                   //cardNumber: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},

                   //expiryDate: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},
               }
           })
        .on('blur', 'input#txtInsurancePaid,input#txtInsuranceWriteoff,input#txtPatientPaid,input#txtPatientDiscount,input#chkCrossOver', function (e) {
            var formValidation = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data("bootstrapValidator");
            switch ($(this).attr("name")) {
                case 'InsurancePaid':

                    var InsurancePaidVal = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting input#txtInsurancePaid').val();
                    if (InsurancePaidVal != "") {
                        //formValidation.enableFieldValidators('OccurrenceDate1', true);//.revalidateField('OccurrenceDate1');
                        formValidation.enableFieldValidators('InsurancePaidAccount', true);
                    }
                    else
                        formValidation.enableFieldValidators('InsurancePaidAccount', false);
                    break;
                case 'InsuranceWriteoff':
                    var InsuranceWriteOffVal = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting input#txtInsuranceWriteoff').val();
                    if (InsuranceWriteOffVal != "") {
                        formValidation.enableFieldValidators('InsuranceWriteoffAccount', true);
                    }
                    else
                        formValidation.enableFieldValidators('InsuranceWriteoffAccount', false);
                    break;
                case 'PatientPaid':
                    var PatientPaidVal = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting input#txtPatientPaid').val();
                    if (PatientPaidVal != "") {
                        //formValidation.enableFieldValidators('OccurrenceDate1', true);//.revalidateField('OccurrenceDate1');
                        formValidation.enableFieldValidators('PatientPaidAccount', true);
                    }
                    else
                        formValidation.enableFieldValidators('PatientPaidAccount', false);
                    break;
                case 'PatientDiscount':
                    var PatientDiscountVal = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting input#txtPatientDiscount').val();
                    if (PatientDiscountVal != "") {
                        formValidation.enableFieldValidators('PatientDiscountAccount', true);
                    }
                    else
                        formValidation.enableFieldValidators('PatientDiscountAccount', false);
                    break;
                    //case 'CopaymentPaid':
                    //    var PatientDiscountVal = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting input#txtCopaymentPaid').val();
                    //    if (PatientDiscountVal != "") {
                    //        formValidation.enableFieldValidators('CopaymentPaidAccount', true);
                    //    }
                    //    else
                    //        formValidation.enableFieldValidators('CopaymentPaidAccount', false);
                    //    break;
                    //case 'CopaymentDiscount':
                    //    var PatientDiscountVal = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting input#txtCopaymentDiscount').val();
                    //    if (PatientDiscountVal != "") {
                    //        formValidation.enableFieldValidators('CopaymentDiscountAccount', true);
                    //    }
                    //    else
                    //        formValidation.enableFieldValidators('CopaymentDiscountAccount', false);
                    //    break;
                    //case 'CrossOver':
                    //    var CrossOverVal = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting input#chkCrossOver').attr("checked");//.val();
                    //    if (CrossOverVal==true) {
                    //        formValidation.enableFieldValidators('InsuranceTransfer', true);
                    //        formValidation.enableFieldValidators('NextInsuranceInsurancePlan', true);
                    //    }
                    //    else {
                    //        formValidation.enableFieldValidators('InsuranceTransfer', false);
                    //        formValidation.enableFieldValidators('NextInsuranceInsurancePlan', false);
                    //    }

                    //    break;
                default:
                    break;

            }
        })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Bill_PaymentPosting.PaymentPostingSave(PaymentType);
        });
    },


    ValidatePatientPayment: function () {
        $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

               }
           })
        .on('keyup', 'input#txtPatientPaid,input#txtPatientDiscount', function (e) {
            var formValidation = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data("bootstrapValidator");
            switch ($(this).attr("name")) {

                default:
                    break;

            }
        }).on('success.form.bv', function (e) {
            e.preventDefault();
            Bill_PaymentPosting.PaymentPostingSave("Patient");
        });
    },

    //ValidateCopaymentPayment: function () {

    //    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting')
    //       .bootstrapValidator({
    //           live: 'disabled',
    //           message: 'This value is not valid',
    //           feedbackIcons: {
    //               valid: 'glyphicon glyphicon-ok',
    //               invalid: 'glyphicon glyphicon-remove',
    //               validating: 'glyphicon glyphicon-refresh'
    //           },
    //           fields: {

    //           }
    //       })
    //    .on('keyup', 'input#txtCopaymentPaid,input#txtCopaymentDiscount', function (e) {
    //        var formValidation = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data("bootstrapValidator");
    //        switch ($(this).attr("name")) {

    //            default:
    //                break;

    //        }
    //    }).on('success.form.bv', function (e) {
    //        e.preventDefault();
    //        Bill_PaymentPosting.PaymentPostingSave("Copayment");
    //    });
    //},

    PaymentPostingSave: function (PaymentType) {

        var isValid = true;

        var strInsurancePlan = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPatientInsurancePlan");
        var InsurancePlanPriority = $('option:selected', strInsurancePlan).attr('priority');
        var InsurancePlanId = $('option:selected', strInsurancePlan).attr('insuranceplanid');
        Bill_PaymentPosting.selectedInsurancePlan = InsurancePlanId;
        var strPatientInsurancePlanId = $('option:selected', strInsurancePlan).val();
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientInsurancePlanId").val(strPatientInsurancePlanId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanPriority").val(InsurancePlanPriority);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanId").val(InsurancePlanId);

        $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting div[id*='Payment']").each(function () {
            if ($(this).css("display") != "none") {
                var CurrentDivId = $(this).attr("id");
                if (CurrentDivId == "divInsurancePayment") {
                    PaymentType = "Insurance";
                    isValid = Bill_PaymentPosting.ValidatePaymentBalance('Insurance');
                }
                else if (CurrentDivId == "divPatientPayment") {
                    PaymentType = "Patient";
                    isValid = Bill_PaymentPosting.ValidatePaymentBalance('Patient');
                }
                    //else if (CurrentDivId == "divCopaymentPayment") {
                    //    PaymentType = "Copayment";
                    //    isValid = Bill_PaymentPosting.ValidatePaymentBalance('copay');
                    //}

                else if (CurrentDivId == "divZeroPayment") {
                    PaymentType = "ZeroPayment";
                    isValid = true;//Bill_PaymentPosting.ValidatePaymentBalance('copay');
                }
                else if (CurrentDivId == "divRecoupmentPayment") {
                    PaymentType = "Recoupment";
                    isValid = true;//Bill_PaymentPosting.ValidatePaymentBalance('copay');
                }

                return
            }
        });
        if (isValid == false) {

            /****** Confirm Dialog start*******/
            utility.myConfirm('Future Balance Will be <b>Negative</b>. Do you want to Continue ?', function () {
                //YES

                Bill_PaymentPosting.postPayment(PaymentType);

            }


            , function () {
                //NO CALLBACK
            },
     'Confirm Over Payment'

     //
);
            /****** Confirm Dialog end*******/
        }

        else {
            /*Begin Edited by Azeem Raza Tayyab on 01-Nov-2016 to Implement CR: PMS-864*/
            if (Bill_PaymentPosting.zeroBilledAmount == true) {
                utility.myConfirm('Billed Fee is Zero, Do You Want to Post The Payment?', function () {
                    Bill_PaymentPosting.postPayment(PaymentType);
                }
              , function () {
                  //NO CALLBACK
              }, 'Confirm Zero Billed Fee');
            }
            else {
                Bill_PaymentPosting.postPayment(PaymentType);
            }
            /*End Edited by Azeem Raza Tayyab on 01-Nov-2016 to Implement CR: PMS-864*/
        }
    },

    postPayment: function (PaymentType) {

        var PaymentMode = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPaymentType").val();
        var CheckNumber = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCheckNumber").val();
        var ChargeId = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfChargeId").val();

        /* to check if the payment exists against a checkNumber*/

        if (CheckNumber != null && CheckNumber != undefined && CheckNumber != "" && PaymentMode == "2") {
            Bill_PaymentPosting.IsCheckPosted(CheckNumber, ChargeId).done(function (response) {


                if (response.Message == "") {

                    Bill_PaymentPosting.PostValidPayment(PaymentType);

                }
                else {
                    // confirm here


                    utility.myConfirm(response.Message, function () {
                        //YES

                        Bill_PaymentPosting.PostValidPayment(PaymentType);

                    }


           , function () {
               //NO CALLBACK
           },
    'Confirm Duplicate Payment'
);
                }




            });
        } else {
            //if checknumber is empty then post straigh away
            Bill_PaymentPosting.PostValidPayment(PaymentType);
        }

    },


    PostValidPayment: function (PaymentType) {


        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfIsDenied").val("false");
        //alert("validation Successfull for " + PaymentType);
        var strMessage = "";
        var self = $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting");

        var myJSON = self.getMyJSONByName();
        if (Bill_PaymentPosting.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Payment Posting", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    Bill_PaymentPosting.SaveChargePayment(myJSON, PaymentType).done(function (response) {
                        Bill_PaymentPosting.IsFromSave = true;
                        if (response.status != false) {
                            Bill_PaymentPosting.params.ChargeId = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfChargeId").val();
                            if (Bill_PaymentPosting.params.VisitId != null) {

                                Bill_PaymentPosting.LoadCharges(Bill_PaymentPosting.params.VisitId, Bill_PaymentPosting.params.CurrentPageNo, 5);
                            }

                            else {
                                Bill_PaymentPosting.LoadCharges(null, Bill_PaymentPosting.params.CurrentPageNo, 5);
                            }

                            //update the patient balances in patient banner
                            Patient_Demographic.UpdateBalancesInBanner();

                            utility.DisplayMessages(response.message, 1);
                            //start syed zia 02-02-2016, bug #PMS-3792
                            //utility.ClearFormValidation('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting", true);
                            $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #chargeDetailDiv #divInsurancePayment,#divAdvancePayment,#divPatientPayment,#divZeroPayment,#divRecoupmentPayment").each(function () {
                                $(this).find('[type=text], textarea,[type=checkbox]').each(function () {
                                    if ($(this).prop('type') == "checkbox") {
                                        $(this).prop('checked', false);
                                    }
                                    else {
                                        $(this).val('');
                                    }
                                });
                            });
                            //end syed zia 02-02-2016, bug #PMS-3792
                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }
                        if ($('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #radInsurance').prop('checked') == true || $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #radZeroPayment').prop('checked') == true || $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #radRecoupment').prop('checked') == true) {
                            $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').find("#radInsurance").trigger("click");
                        }
                    });
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    ShowHideInsurancePlan: function (obj) {
        var CurrentObj = $(obj);
        if (CurrentObj.attr("name") == "RadInsuranceNextResponsibility") {
            if (CurrentObj.attr("id") == "RadInsuranceNextRespPatient") {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #ddlNextInsuranceInsurancePlan").prop("disabled", true);//.val("");
            }
            else
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #ddlNextInsuranceInsurancePlan").prop("disabled", false);
        } else if (CurrentObj.attr("name") == "RadRecoupmentNextResponsibility") {
            if (CurrentObj.attr("id") == "RadRecoupmentNextRespPatient") {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #ddlRecoupmentNextInsuranceInsurancePlan").prop("disabled", true);//.val("");
            }
            else
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #ddlRecoupmentNextInsuranceInsurancePlan").prop("disabled", false);
        }
        //else if (CurrentObj.attr("name") == "RadPatNextResponsibility") {
        //    if (CurrentObj.attr("id") == "RadInsuranceNextRespPatient") {
        //        $("#" + Bill_PaymentPosting.params["PanelID"] + " #ddlNextInsuranceInsurancePlan").css("display", "none");
        //    }
        //    else
        //        $("#" + Bill_PaymentPosting.params["PanelID"] + " #ddlNextInsuranceInsurancePlan").css("display", "");
        //}
    },

    OpenPatientSearch: function () {
        var params = [];
        params["FromAdmin"] = "0";
        if (Bill_PaymentPosting.params.TabID == "billTabPaymentBatchSearch" || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail') {
            params["ParentCtrl"] = 'Bill_PaymentPosting';
        }
        else {
            params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        }
        LoadActionPan('Patient_Search', params);
    },

    FillPatientInfoFromSearch: function (PatientId, patFullName, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfPatientId").val(PatientId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientName").val(patFullName.split(" - ")[0]);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientFullName").val(patFullName.split(" - ")[1]);
        Bill_PaymentPosting.LoadPatientCase(PatientId);
        var ParentCtrl = null;
        if (Bill_PaymentPosting.params.TabID == 'billTabPaymentBatchSearch' || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail')
            ParentCtrl = 'Bill_PaymentPosting';
        else
            ParentCtrl = Bill_PaymentPosting.params["TabID"];
        if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientName").data('kendoAutoComplete'))
            utility.SetKendoAutoCompleteSourceforValidate($("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientName"), patFullName.split(" - ")[0], $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfPatientId"), PatientId);
        UnloadActionPan(ParentCtrl);
        utility.InsertRecentPatient(PatientId);
    },

    BindPatientAccount: function () {
        var Ctrl = $("#" + Bill_PaymentPosting.params.PanelID + " #txtPatientName");
        var hfCtrl = $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfPatientId");
        var func = function () { return Bill_PaymentPosting.GetActivePatientsArray(Ctrl.val()); };
        var onSelect = function (e) {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientName").val(e.value.split(" ")[0]);
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientFullName").val(e.FullName);
            utility.InsertRecentPatient(e.id);
            Bill_PaymentPosting.LoadPatientCase(e.id);
            Bill_PaymentPosting.EnableClaimCaseFields();
        };
        var onChange = function (valid) { if (!valid) $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientFullName").val(''); };
        utility.BindKendoAutoComplete(Ctrl, 3, "value", "contains", null, func, hfCtrl, onSelect, onChange);
    },

    GetActivePatientsArray: function (name) {
        var AllPatients = [];
        var dfd = new $.Deferred();
        appointmentDetail.LoadActivePatients(name).done(function (responseData) {
            if (responseData.status != false) {
                if (responseData.PatientCount > 0) {
                    var PatientLoadJSONData = JSON.parse(responseData.PatientLoad_JSON);
                    $.each(PatientLoadJSONData, function (i, item) {
                        AllPatients.push({ id: item.PatientId, value: item.AccountNumber, AccountNumber: item.AccountNumber, FullName: item.FullName, RefferingProviderName: item.ReferringProviderName, RefferingProviderId: item.ReferringProviderId, ProviderID: item.ProviderId, Providername: item.ProviderName, FacilityID: item.FacilityId, Facilityname: item.FacilityName });
                    });
                }
            }
            dfd.resolve(AllPatients);
        });
        return dfd.promise();
    },

    EnableClaimCaseFields: function () {
        $("#" + Bill_PaymentPosting.params.PanelID + " #txtClaimNumber,#lnkClaimNumber,#txtCaseNumber,#lnkCaseNumber").prop("disabled", false);
    },

    OpenEncounter: function () {
        var params = [];
        params["FromAdmin"] = 0;
        if (Bill_PaymentPosting.params.TabID == 'billTabPaymentBatchSearch' || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail')
            params["ParentCtrl"] = 'Bill_PaymentPosting';
        else
            params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        if ($("#" + Bill_PaymentPosting.params.PanelID + " #txtPatientName").val().trim() == "")
            params["patientID"] = 0;
        else
            params["patientID"] = Number($('#' + Bill_PaymentPosting.params.PanelID + ' #hfPatientId').val());
        LoadActionPan('Encounter_Visits', params);
        //$('#CloseVisits').remove();
        // $('#OpenVisits').remove();
        //if (Bill_PaymentPosting.bVisitFirst) {
        //    $($('body #OpenVisits')[0]).attr('id', 'OpenVisits1')
        //    $($('body #CloseVisits')[0]).attr('id', 'CloseVisits1');
        //    Bill_PaymentPosting.bVisitFirst = false;
        //}
    },

    FillClaimNumberFromSearch: function (ClaimNumber, AccountNumber, PatientName, PatientId, DOSFrom, VisitId) {
        //$("#" + Bill_PaymentPosting.params.PanelID + " #txtClaimNumber").val(ClaimNumber + ' - ( ' + AccountNumber + ' - ' + PatientName + ' )');
        $("#" + Bill_PaymentPosting.params.PanelID + " #txtClaimNumber").val(ClaimNumber);
        $("#" + Bill_PaymentPosting.params.PanelID + " #dpDOSfrm").val(DOSFrom);
        //$("#" + Bill_PaymentPosting.params.PanelID + " #hfPatientId").val(PatientId);
        //$("#" + Bill_PaymentPosting.params.PanelID + " #txtPatientName").val(AccountNumber + ' - ' + PatientName);
        //$("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientFullName").val(PatientName);
        $("#" + Bill_PaymentPosting.params.PanelID + " #hfVisitId").val(VisitId);
        //Bill_PaymentPosting.LoadPatientCase(PatientId);


        //UnloadActionPan(Bill_PaymentPosting.params["TabID"]);
        Encounter_Visits.UnLoad();
    },

    OpenCaseDetail: function (HiddenCtrl) {
        var params = [];
        params["CaseId"] = parseInt($('#' + Bill_PaymentPosting.params["PanelID"] + ' #' + HiddenCtrl).val());
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";

        //  params["PatientId"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfPatientId").val();
        params["RefCtrl"] = "txtCaseNumber";
        if (Bill_PaymentPosting.params.TabID == 'billTabPaymentBatchSearch' || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail')
            params["ParentCtrl"] = 'Bill_PaymentPosting';
        else
            params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        LoadActionPan('Patient_Case_Detail', params);
    },

    OpenCase: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["CaseId"] = "-1";
        // params["patientID"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfPatientId").val();
        params["patientID"] = "";
        params["FromAdmin"] = "0";
        if (Bill_PaymentPosting.params.TabID == 'billTabPaymentBatchSearch' || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail')
            params["ParentCtrl"] = 'Bill_PaymentPosting';
        else
            params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        LoadActionPan('Patient_Case', params);
    },

    BindClaimNumber: function () {
        $("#" + Bill_PaymentPosting.params.PanelID + " #txtClaimNumber").autocomplete({
            autoFocus: true,
            source: function (request, response) {
                utility.Keyupdelay(function () {
                    var ClaimNumber = $('#' + Bill_PaymentPosting.params.PanelID + ' #txtClaimNumber').val();
                    if (ClaimNumber.length > 2) {
                        Bill_ClaimSubmission.LoadClaimNumers(ClaimNumber).done(function (responseData) {
                            if (responseData.status != false) {
                                if (responseData.ClaimsCount > 0) {
                                    var Claims = JSON.parse(responseData.ClaimsLoad_JSON);
                                    var AllClaimsVisits = [];
                                    $.each(Claims, function (i, item) {
                                        AllClaimsVisits.push({ id: item.VisitId, value: item.ClaimNumber + ' - ( ' + item.AccountNumber + ' - ' + item.PatientName + ' )', PatientId: item.PatientId, DOSFrom: item.DOSFrom, PatientName: item.AccountNumber + ' - ' + item.PatientName, ClaimNumber: item.ClaimNumber });
                                    });
                                    response(AllClaimsVisits);
                                }
                            }
                        });
                    }

                });
            },

            select: function (event, ui) {
                setTimeout(function () {
                    $("#" + Bill_PaymentPosting.params.PanelID + " #hfVisitId").val(ui.item.id);
                    $("#" + Bill_PaymentPosting.params.PanelID + " #dpDOSfrm").val(ui.item.DOSFrom);
                    //$("#" + Bill_PaymentPosting.params.PanelID + " #hfPatientId").val(ui.item.PatientId);
                    //$("#" + Bill_PaymentPosting.params.PanelID + " #txtPatientName").val(ui.item.PatientName);
                    $("#" + Bill_PaymentPosting.params.PanelID + " #txtClaimNumber").val(ui.item.ClaimNumber);
                    //Bill_PaymentPosting.LoadPatientCase(ui.item.PatientId);
                }, 100);

                //$("#hfpatientid").val(ui.item.id);
            }
        });
    },
    LoadPatientCase: function (PatientId) {
        $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting input#txtCaseNumber").val('');
        $("#" + Bill_PaymentPosting.params.PanelID + " #hfCaseId").val('');

        if (PatientId == null)
            PatientId = -1;

        CacheManager.BindPatientData('GetPatientCase', true, PatientId).done(function (result) {
            var Ctrl = $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting input#txtCaseNumber");
            var hfCtrl = $("#" + Bill_PaymentPosting.params.PanelID + " #hfCaseId");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", PatientCase, null, hfCtrl);
        });
    },

    ShowHidePaymentDiv: function (obj) {

        var radBtnId = $(obj).attr("id");
        var formValidation = $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').data("bootstrapValidator");

        $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting div[id*='Payment']").each(function () {

            if (radBtnId == "radInsurance" && $(this).attr("id") == "divInsurancePayment") {
                Bill_PaymentPosting.FillInsuranceLedgerAccount();
                $(this).css("display", "");
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPaymentBatch").css("display", "");
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPatientInsurancePlan").css("display", "");

                //comment line against bug number PMS-981
                // $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPatientInsurancePlan").val("").attr("selected", "selected");

                if (formValidation) {
                    formValidation.enableFieldValidators('PatientInsurancePlan', true);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'PatientInsurancePlan');
                }


                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType").prop("disabled", false);
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType option").each(function () {
                    if ($(this).text() == "Advance Payment") {
                        $(this).remove();

                    }
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(2).attr("selected", "selected");

                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').trigger('change');
                });

                Bill_PaymentPosting.calculateBalance('insurance');


                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").hide();
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfRecoupmentNextInsurancePlan").val("");
            }
            else if (radBtnId == "radPatient" && $(this).attr("id") == "divPatientPayment") {
                Bill_PaymentPosting.FillPatientLedgerAccount();

                $(this).css("display", "");
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPaymentBatch").css("display", "none");
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPatientInsurancePlan").css("display", "none");

                if (formValidation) {
                    formValidation.enableFieldValidators('PatientInsurancePlan', false);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'PatientInsurancePlan');
                }

                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType").prop("disabled", false);
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType option").each(function () {
                    if ($(this).text() == "Advance Payment") {
                        $(this).remove();

                    }

                });

                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType").append('<option value="4" refvalue="" refname="">Advance Payment</option>')
                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(1).attr("selected", "selected");

                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').trigger('change');


                Bill_PaymentPosting.calculateBalance('patient');


                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").show();

                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfRecoupmentNextInsurancePlan").val("");
            }
                //else if (radBtnId == "radCopayment" && $(this).attr("id") == "divCopaymentPayment") {
                //    Bill_PaymentPosting.FillCopayLedgerAccount();
                //    $(this).css("display", "");
                //    $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPaymentBatch").css("display", "none");
                //    $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPatientInsurancePlan").css("display", "none");

                //    if (formValidation) {
                //        formValidation.enableFieldValidators('PatientInsurancePlan', false);
                //        $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'PatientInsurancePlan');
                //    }

                //    $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType").prop("disabled", false);
                //    $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType option").each(function () {
                //        if ($(this).text() == "Advance Payment") {
                //            $(this).remove();

                //        }


                //    });

                //    $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType").append('<option value="4" refvalue="" refname="">Advance Payment</option>')
                //    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(1).attr("selected", "selected");

                //    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').trigger('change');

                //    Bill_PaymentPosting.calculateBalance('copay');

                //    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").show();
                //}

                //zero payment section start
            else if (radBtnId == "radZeroPayment" && $(this).attr("id") == "divZeroPayment") {
                //Bill_PaymentPosting.FillCopayLedgerAccount();
                $(this).css("display", "");

                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPaymentBatch").css("display", "none");
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPatientInsurancePlan").css("display", "");

                if (formValidation) {
                    formValidation.enableFieldValidators('PatientInsurancePlan', true);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'PatientInsurancePlan');
                }

                //$('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType option").each(function () {
                //    if ($(this).text() == "Advance Payment") {
                //        $(this).remove();

                //    }


                //});

                //$('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType").append('<option value="4" refvalue="" refname="">Advance Payment</option>')
                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(1).attr("selected", "selected");

                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').trigger('change');
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType").prop("disabled", true);
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfRecoupmentNextInsurancePlan").val("");
                //Bill_PaymentPosting.calculateBalance('copay');

                //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").show();
            }
                //zero payment section end

                // Recoupment section start

            else if (radBtnId == "radRecoupment" && $(this).attr("id") == "divRecoupmentPayment") {
                Bill_PaymentPosting.FillInsuranceLedgerAccount();
                $(this).css("display", "");
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPaymentBatch").css("display", "");
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divPatientInsurancePlan").css("display", "");

                if (formValidation) {
                    formValidation.enableFieldValidators('PatientInsurancePlan', true);
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'PatientInsurancePlan');
                }

                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType").prop("disabled", false);
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #ddlPaymentType option").each(function () {
                    if ($(this).text() == "Advance Payment") {
                        $(this).remove();

                    }
                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').val(2).attr("selected", "selected");

                    $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPaymentType').trigger('change');
                });

                Bill_PaymentPosting.calculateBalance('insurance');


                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").hide();
                Bill_PaymentPosting.RecoupmentNextResponsabilityHfField();


            }
                // Recoupment section end

            else if (radBtnId == "radTransfer" && $(this).attr("id") == "divTransferPayment") {
                $(this).css("display", "");
            }
            else
                $(this).css("display", "none");
        });
        if (Bill_PaymentPosting.params["IsFromCollectCopay"] == true && radBtnId == "radPatient") {
            $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPatientPaid').find('option:contains(Copay Payment)').prop("selected", true);
        }
    },

    ShowHidePaymentTypeDiv: function (value) {

        $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting div[id*='PaymentType']").each(function () {
            if (value == 2 && $(this).attr("id") == "PaymentTypeCheck") {
                $(this).css("display", "inline");
                //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").hide();
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divAdvancePayment").hide();
            }
            else if (value == 3 && $(this).attr("id") == "PaymentTypeCreditCard") {
                $(this).css("display", "inline");

                //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").hide();
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divAdvancePayment").hide();
            }
            else if (value == 4) {
                $(this).css("display", "none");
                //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").show();
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divAdvancePayment").show();
                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'AdvancePayment');
                var patientId = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientId").val();
                if (patientId && patientId > 0)
                    Bill_PaymentPosting.BindAdvancePayment(patientId);
            }

            else {
                $(this).css("display", "none");
                //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").hide();
                $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divAdvancePayment").hide();
            }
        });
    },

    AdvancePaymentValidation: function () {
        $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').bootstrapValidator('revalidateField', 'AdvancePayment');
    },

    SaveChargePayment: function (PaymentData, PaymentType) {
        var objData = JSON.parse(PaymentData);

        objData["PaymentType"] = PaymentType;
        objData["CommandType"] = "Save";

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Payments", "PaymentPosting");
    },

    SearchCharges: function (ChargeData, VisitId, ChargeId, PageNumber, RowsPerPage) {


        if (VisitId == null) {
            VisitId = 0;
        }

        if (ChargeId == null) {
            ChargeId = 0;
        }
        if (PageNumber == null) {
            PageNumber = 1;
        }
        if (RowsPerPage == null) {
            RowsPerPage = 5;
        }


        Bill_PaymentPosting.params.CurrentPageNo = PageNumber;
        var data = "ChargeData=" + ChargeData + "&VisitId=" + VisitId + "&ChargeId=" + ChargeId + "&PageNumber=" + PageNumber + "&RowsPerPage=" + RowsPerPage;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_POSTING", "SEARCH_CHARGE");
    },

    SearchChargeModel: function (ChargeData, VisitId, ChargeId, PageNumber, RowsPerPage) {

        PageNumber = PageNumber == null || PageNumber == "" ? 1 : PageNumber;
        RowsPerPage = RowsPerPage == null || RowsPerPage == "" ? 5 : RowsPerPage;

        var objData = JSON.parse(ChargeData);

        objData["ChargeId"] = ChargeId;
        objData["VisitId"] = VisitId;
        objData["PageNumber"] = PageNumber;
        objData["CommandType"] = "Search";
        //objData["CreatedBy_Text"] = $('#ddlCreatedBy option:selected').text();
        //objData["Paid_Text"] = $('#ddlPaid option:selected').text();
        objData["RowsPerPage"] = RowsPerPage;

        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Payments", "PaymentPosting");

    },


    SearchChargePaidPayment: function (ChargeId) {
        if (ChargeId == null) {
            ChargeId = 0;
        }
        var data = "PaymentId=0&AppointmentID=0&VisitId=0&ChargeId=" + ChargeId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_COPAYMENT", "GET_PATIENT_COPAYMENT_BY_CHRGID");
    },

    UnLoadTab: function (Tab) {
        Bill_PaymentPosting.IsFromSave = false;
        Bill_PaymentPosting.selectedInsurancePlan = "";
        //Bill_PaymentPosting.removeDialogClasses();
        if (Bill_PaymentPosting.params != null && Bill_PaymentPosting.params.ParentCtrl != null) {
            if (Bill_PaymentPosting.params.ParentCtrl == "paymentBatchDetail") {
                var parentCtrl = Bill_PaymentPosting.params.ParentCtrl;
                if (Bill_PaymentPosting.params.OpenFrom == "paymentBatchDetail")
                    UnloadActionPan(Bill_PaymentPosting.params.ParentCtrl, 'Bill_PaymentPosting', null, "pnlPaymentBatchDetail");
                else
                    UnloadActionPan(parentCtrl, 'Bill_PaymentPosting', true);
                Bill_PaymentPosting.params = null;
                ChargeBatch_Viewer.PaymentBatchActiveBtns(true);
                paymentBatchDetail.paymentBatchFill(paymentBatchDetail.params.BatchId)
            }
            else {
                //Begin Added by Azeem Raza Tayyab to fix bug#:PMS-5762
                if (Bill_PaymentPosting.params.ParentCtrl && Bill_PaymentPosting.params.ParentCtrl.indexOf("EncounterChargeCapture") > -1) {
                    EncounterChargeCapture.ReloadClaimPayments(true);
                }
                //End Added by Azeem Raza Tayyab to fix bug#:PMS-5762
                //RemoveAdminTab(GetTab(Bill_PaymentPosting.params["TabID"]));
                //RemoveTab(GetTab(Bill_PaymentPosting.params["TabID"]));
                UnloadActionPan(Bill_PaymentPosting.params.ParentCtrl, "Bill_PaymentPosting");
                //UnloadActionPan(Bill_PaymentPosting.params.ParentCtrl);
            }
        }
        else
            RemoveAdminTab(Tab);//UnloadActionPan(null, Bill_PaymentPosting.params["TabID"]);
        Bill_PaymentPosting.params = null;


    },

    RefundPayment: function (PaymentId) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Payment Posting", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Are you sure you want to refund?', function () {
                    var selectedValue = PaymentId;
                    if (selectedValue != "" && selectedValue != "undefined") {


                        Bill_PaymentPosting.PaymentRefund(selectedValue).done(function (response) {
                            if (response.status != false) {

                                //RELOAD GRID
                                //Bill_PaymentPosting.showAlertMessage = false;
                                Bill_PaymentPosting.LoadChargesPaidPayment($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfChargeId").val());

                                if (Bill_PaymentPosting.params.VisitId != null) {

                                    Bill_PaymentPosting.LoadCharges(Bill_PaymentPosting.params.VisitId, Bill_PaymentPosting.params.CurrentPageNo, 5);

                                }
                                else {
                                    Bill_PaymentPosting.LoadCharges(null, Bill_PaymentPosting.params.CurrentPageNo, 5);
                                }

                                //update the patient balances in patient banner
                                Patient_Demographic.UpdateBalancesInBanner();

                                utility.DisplayMessages(response.Message, 1);



                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }
                        });
                    }
                }, function () { },
                    'Refund confirm'
                );
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });

    },

    PaymentRefund: function (PaymentId) {
        var data = "PaymentId=" + PaymentId;
        // serach parameter , class name, command name of class

        return MDVisionService.defaultService(data, "BILLING_PAYMENT_POSTING", "REFUND_PATIENT_PAYMENT");
    },

    setLedgerAccountIdInHiddentCtrl: function (hiddentCtrl, methodName, reload, Id1, Id2) {

        var data = "ID=" + Id1 + "&ID2=" + Id2
        MDVisionService.lookups(methodName, reload, data).done(function (result) {
            result = JSON.parse(result[methodName]);
            if (result[1] != null && result[1].Value != null)
                $(hiddentCtrl).val(result[1].Value);

            //var transferLedgerAccountId = {};

            //transferLedgerAccountId["TransferLedgerAccountId"] = result[1].Value;

            // transferLedgerAccountJson = JSON.stringify(transferLedgerAccountId);

        });

    },

    calculateBalance: function (BalanceType) {


        var InsBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsBalance").val();
        InsBalance = utility.convertToFigure(InsBalance);// InsBalance == "" ? 0 : InsBalance;

        var PatBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatBalance").val();
        PatBalance = utility.convertToFigure(PatBalance);// PatBalance == "" ? 0 : PatBalance;

        //var CopayBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfCopayBalance").val();
        //CopayBalance = utility.convertToFigure(CopayBalance);// CopayBalance == "" ? 0 : CopayBalance;


        if (BalanceType.toLowerCase() == "insurance") {

            var ObjPaid = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsurancePaid");
            var ObjWriteOff = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceWriteoff");

            //transfers
            var ObjCoinsurance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceCoinsurance");
            var ObjDeductables = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceDeductables");
            var ObjPatientResponsibility = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsurancePatientResponsibility");

            //Copay
            var ObjCopay = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceCopay");

            var PaidAmount = ObjPaid.val();
            PaidAmount = PaidAmount == "" ? 0 : PaidAmount;

            var WriteOffAmount = ObjWriteOff.val();
            WriteOffAmount = WriteOffAmount == "" ? 0 : parseFloat(WriteOffAmount);


            PaidAmount = parseFloat(PaidAmount);
            if (PaidAmount < 0) {
                PaidAmount = 0 - (PaidAmount);
            }


            //transfer amounts
            var coinsuranceAmount = ObjCoinsurance.val();
            coinsuranceAmount = coinsuranceAmount == "" ? 0 : parseFloat(coinsuranceAmount);

            var deductableAmount = ObjDeductables.val();
            deductableAmount = deductableAmount == "" ? 0 : parseFloat(deductableAmount);

            var patientResponsibilityAmount = ObjPatientResponsibility.val();
            patientResponsibilityAmount = patientResponsibilityAmount == "" ? 0 : parseFloat(patientResponsibilityAmount);


            //Copay Amount
            var CopayAmount = 0;

            if (ObjCopay.length > 0)
                CopayAmount = ObjCopay.val();

            CopayAmount = CopayAmount == "" ? 0 : parseFloat(CopayAmount);

            var CurrentInsBalance = (InsBalance - (PaidAmount + WriteOffAmount + coinsuranceAmount + deductableAmount + patientResponsibilityAmount + CopayAmount));

            CurrentInsBalance = utility.convertToFigure(CurrentInsBalance);// parseFloat(CurrentInsBalance).toFixed(Number(globalAppdata.DecimalPlaces));

            if (CurrentInsBalance < 0) {

                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceBalance").css('color', 'red');
            } else {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceBalance").css('color', '#000');
            }

            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtInsuranceBalance").val(CurrentInsBalance);

        }
        else if (BalanceType.toLowerCase() == "patient") {
            var ObjPaid = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientPaid");
            var ObjDiscount = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientDiscount");


            //transfers
            var ObjCopay = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopay");
            var ObjCoinsurance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientcoinsurance");
            var ObjDeductables = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientdeductables");
            var ObjPatientResponsibility = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientPatientResponsibility");


            var PaidAmount = ObjPaid.val();
            PaidAmount = PaidAmount == "" ? 0 : PaidAmount;

            var DiscountAmount = ObjDiscount.val();
            DiscountAmount = DiscountAmount == "" ? 0 : DiscountAmount;

            //transfer amounts
            var copayAmount = ObjCopay.val();
            copayAmount = copayAmount == "" ? 0 : parseFloat(copayAmount);

            var coinsuranceAmount = ObjCoinsurance.val();
            coinsuranceAmount = coinsuranceAmount == "" ? 0 : parseFloat(coinsuranceAmount);

            var deductableAmount = ObjDeductables.val();
            deductableAmount = deductableAmount == "" ? 0 : parseFloat(deductableAmount);

            var patientResponsibilityAmount = ObjPatientResponsibility.val();
            patientResponsibilityAmount = patientResponsibilityAmount == "" ? 0 : parseFloat(patientResponsibilityAmount);


            var CurrentPatBalance = parseFloat(PatBalance) - (parseFloat(PaidAmount) + parseFloat(DiscountAmount) + parseFloat(copayAmount) + parseFloat(coinsuranceAmount) + parseFloat(deductableAmount) + parseFloat(patientResponsibilityAmount));

            CurrentPatBalance = parseFloat(utility.convertToFigure(CurrentPatBalance));

            if (CurrentPatBalance < 0) {

                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientBalance").css('color', 'red');
            } else {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientBalance").css('color', '#000');
            }

            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtPatientBalance").val(parseFloat(CurrentPatBalance.toFixed(2)));


            //ADVANCE BALANCE CALCULATE

            if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPaymentType").val() == 4) {

                var advanceBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfAdvanceBalance").val() == "" ? 0 : $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfAdvanceBalance").val();

                var remainingAdvanceBalance = parseFloat(advanceBalance) - parseFloat(PaidAmount);

                if (remainingAdvanceBalance < 0) {

                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").addClass('red')
                } else {
                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").removeClass('red');
                }

                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").text(utility.convertToFigure(remainingAdvanceBalance, true));


            }

        }
        //else if (BalanceType.toLowerCase() == "copay") {
        //    var ObjPaid = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentPaid");
        //    var ObjDiscount = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentDiscount");
        //    var ObjCopayTransfer = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopay");

        //    var PaidAmount = ObjPaid.val();
        //    PaidAmount = PaidAmount == "" ? 0 : PaidAmount;

        //    var DiscountAmount = ObjDiscount.val();
        //    DiscountAmount = DiscountAmount == "" ? 0 : DiscountAmount;

        //    var CopayTransferAmount = ObjCopayTransfer.val();
        //    CopayTransferAmount = CopayTransferAmount == "" ? 0 : CopayTransferAmount;


        //    var CurrentCopayBalance = parseFloat(CopayBalance) - (parseFloat(PaidAmount) + parseFloat(DiscountAmount) + parseFloat(CopayTransferAmount));

        //    CurrentCopayBalance = parseFloat(utility.convertToFigure(CurrentCopayBalance));

        //    if (CurrentCopayBalance < 0) {

        //        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentBalance").css('color', 'red');
        //    } else {
        //        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentBalance").css('color', '#000');
        //    }

        //    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtCopaymentBalance").val(CurrentCopayBalance);


        //    if ($("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #ddlPaymentType").val() == 4) {

        //        var advanceBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfAdvanceBalance").val() == "" ? 0 : $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfAdvanceBalance").val();

        //        var remainingAdvanceBalance = parseFloat(advanceBalance) - parseFloat(PaidAmount);

        //        if (remainingAdvanceBalance < 0) {

        //            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").addClass('red')
        //        } else {
        //            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").removeClass('red');
        //        }

        //        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").text(utility.convertToFigure(remainingAdvanceBalance, true));


        //    }



        //}

    },

    // MA-727 Optimizations

    LoadPaymentBatch: function () {
       // CacheManager.BindCodes('GetPaymentBatch', false).done(function (result) {
            var Ctrl = $('#' + Bill_PaymentPosting.params.PanelID + " input#txtPaymentBatch");
            var hfCtrl = $('#' + Bill_PaymentPosting.params.PanelID + " #hfPaymentBatch");
            var func = function () {
                return Bill_PaymentPosting.GetPaymentBatch(Ctrl.val())
            };
            var onSelect = function (e) {
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtCheckNumber").val(e.CheckNumber);
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #dtpCheckDate").val(utility.RemoveTimeFromDate(null, e.CheckDate));
            };
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", null, func, hfCtrl);
       // });

    },

    GetPaymentBatch: function (BatchNumber) {
        var AllPaymentBatches = [];
        var dfd = new $.Deferred();
        if (BatchNumber) {
            Bill_PaymentPosting.LoadGetPaymentBatch_DBCall(BatchNumber).done(function (responseData) {
                if (responseData.length > 0) {
                    $.each(responseData, function (i, item) {
                        AllPaymentBatches.push({ id: item.Value, value: item.Name });
                    });
                }

                dfd.resolve(AllPaymentBatches);
            });
        }
        else {
            dfd.resolve(AllPaymentBatches);
        }

        return dfd.promise();
    },

    LoadGetPaymentBatch_DBCall: function (BatchNumber) {
        return MDVisionService.PMSAPIService(BatchNumber, "Payments", "GetPaymentBatch");
    },

    OpenPaymentBatch: function () {
        var params = [];
        var parentPanelId = null;
        params["BatchId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPaymentBatch";
        if (Bill_PaymentPosting.params.TabID != null) {
            params["ParentCtrl"] = 'Bill_PaymentPosting';
            params["PaymentRef"] = Bill_PaymentPosting.params.PaymentRef;

            if (Bill_PaymentPosting.params["ParentCtrl"] != null)
                parentPanelId = GetTab(Bill_PaymentPosting.params["ParentCtrl"]).PanelID;
            // parentPanelId = "pnlBillChargeSearch";
        }

        else {
            params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        }
        LoadActionPan('Bill_PaymentBatchSearch', params, parentPanelId);
    },

    OpenPaymentBatchDetail: function () {

        var params = [];
        params["BatchId"] = $('#pnlBillPaymentPosting #hfPaymentBatch').val();
        params["mode"] = "Edit";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtPaymentBatch";
        //Begin: Tab id Changed Author: Abdur Rehman Latif. March 10th,2015
        if (Bill_PaymentPosting.params.TabID == 'Bill_ChargeSearch') {
            //End: Tab id Changed Author: Abdur Rehman Latif. March 10th,2015
            params["ParentCtrl"] = 'Bill_PaymentPosting';
        }
        else {
            params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        }
        LoadActionPan('paymentBatchDetail', params);
    },

    fillPaymentBatch: function (PaymentBatchNumber, PaymentBatchId, CheckNumber, CheckDate) {

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPaymentBatch").val(PaymentBatchNumber);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfPaymentBatch").val(PaymentBatchId);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtCheckNumber").val(CheckNumber);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #dtpCheckDate").val(CheckDate);

        if (PaymentBatchNumber != "" && PaymentBatchId != "") {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #lnkPaymentBatchEdit").css("display", "inline");
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #lblPaymentBatch").css("display", "none");
        }

        // UnloadActionPan("Bill_PaymentPosting");
    },

    setAdvanceBalance: function (AdvanceBalance) {

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfAdvanceBalance").val(0);
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").text(utility.convertToFigure(0, true));

        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfAdvanceBalance").val(AdvanceBalance);

        if (AdvanceBalance >= 0) {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").removeClass('red');
        } else {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").addClass('red');
        }
        $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #lblAdvanceBalance").text(utility.convertToFigure(AdvanceBalance, true));


    },

    //---------------Pagination functions-----
    SelectedPageClick: function (PageNo, objPage) {
        // Change Background Color to Black for selected page
        $("#pnlPostingCharge_Result li").each(function () {
            if ($(this).text() == PageNo) {
                $(this).attr("class", "active");
            }
            else
                $(this).removeAttr("class");
        });
        Bill_PaymentPosting.params.CurrentPageNo = PageNo;
        if ((Bill_PaymentPosting.params.ParentCtrl == "Bill_ChargeSearch" || Bill_PaymentPosting.params.ParentCtrl == "billTabChargeSearch" || Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpInsuranceAR_Detail" || Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpPatientAR_Detail" || Bill_PaymentPosting.params.ParentCtrl == "EncounterChargeCapture") && Bill_PaymentPosting.params.VisitId != null)
            Bill_PaymentPosting.LoadCharges(Bill_PaymentPosting.params.VisitId, PageNo, 5);
        else
            Bill_PaymentPosting.LoadCharges(0, PageNo, 5);
    },

    PreviousPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlPostingCharge_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }

        });
        currentPageNo = currentPageNo != "" ? currentPageNo - 1 : "";

        Bill_PaymentPosting.params.CurrentPageNo = currentPageNo;
        if (currentPageNo != "" && currentPageNo > 0) {

            if ((Bill_PaymentPosting.params.ParentCtrl == "Bill_ChargeSearch" || Bill_PaymentPosting.params.ParentCtrl == "billTabChargeSearch" || Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpInsuranceAR_Detail" || Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpPatientAR_Detail" || Bill_PaymentPosting.params.ParentCtrl == "EncounterChargeCapture") && Bill_PaymentPosting.params.VisitId != null)
                Bill_PaymentPosting.LoadCharges(Bill_PaymentPosting.params.VisitId, currentPageNo, 5);
            else
                Bill_PaymentPosting.LoadCharges(0, currentPageNo, 5);


        }
    },

    NextPageClick: function (TotalPages, DisplayPages, pagingDivId) {
        var currentPageNo = "";
        $("#pnlPostingCharge_Result li").each(function () {
            if ($(this).attr("class") == "active") {
                $(this).removeAttr("class");
                currentPageNo = parseInt($(this).text());
            }
        });
        currentPageNo = currentPageNo != "" ? currentPageNo + 1 : "";
        Bill_PaymentPosting.params.CurrentPageNo = currentPageNo;
        if (currentPageNo != "" && currentPageNo > 0) {


            if ((Bill_PaymentPosting.params.ParentCtrl == "Bill_ChargeSearch" || Bill_PaymentPosting.params.ParentCtrl == "billTabChargeSearch" || Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpInsuranceAR_Detail" || Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpPatientAR_Detail" || Bill_PaymentPosting.params.ParentCtrl == "EncounterChargeCapture") && Bill_PaymentPosting.params.VisitId != null)
                Bill_PaymentPosting.LoadCharges(Bill_PaymentPosting.params.VisitId, currentPageNo, 5);
            else
                Bill_PaymentPosting.LoadCharges(0, currentPageNo, 5);

        }
    },


    showHideAdvancePaymentAutoComplete: function (obj) {

        if (obj == "divInsurancePayment") {

            $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divAdvancePayment").hide();
        }
        else if (obj == "divPatientPayment") {

            $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divAdvancePayment").show();
        }

        else if (obj == "divCopaymentPayment") {

            $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting #divAdvancePayment").hide();

        }

    },

    OpenAdvancePayment: function () {
        var params = [];
        var parentPanelId = null;
        //  params["PatientAdvancePaymentSearchId"] = "-1";
        // params["FromAdmin"] = "0";
        params["patientID"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientId").val();


        if (Bill_PaymentPosting.params.TabID == 'billTabChargeSearch') {
            params["ParentCtrl"] = 'Bill_PaymentPosting';
            params["PaymentRef"] = Bill_PaymentPosting.params.PaymentRef;
            parentPanelId = params["PaymentRef"];// "pnlBillChargeSearch";
        }

        else if (Bill_PaymentPosting.params.TabID == 'EncounterChargeCapture' || Bill_PaymentPosting.params.TabID == 'billTabPaymentBatchSearch' || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail') {
            params["ParentCtrl"] = 'Bill_PaymentPosting';
        }
            // Start 22/02/2016 Muhammad Irfan for bug # PMS-4074
        else if (Bill_PaymentPosting.params.TabID == 'Bill_ChargeSearch') {
            params["ParentCtrl"] = 'Bill_PaymentPosting';
            params["PaymentRef"] = Bill_PaymentPosting.params.PaymentRef;
            parentPanelId = params["PaymentRef"];// "pnlBillChargeSearch";
        }
            // End 22/02/2016 Muhammad Irfan for bug # PMS-4074

        else {
            params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        }
        LoadActionPan('Patient_AdvancePayment', params, parentPanelId);

        //if (Bill_PaymentPosting.params.TabID == 'paymentBatchDetail')
        //    params["ParentCtrl"] = 'Bill_PaymentPosting';
        //else
        //    params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        //LoadActionPan('Patient_AdvancePayment', params);
    },



    BindAdvancePayment: function (PatientId) {

        var totalBalance = 0;
        var jsonArray = { "hfPatientId": PatientId, "hfFacility": "-1", "dtpPaidFrom": "", "dtpPaidTo": "", "ddlPaymentType": "" };
        Patient_AdvancePayment.AdvancePaymentSearch(JSON.stringify(jsonArray)).done(function (response) {
            if (response.status != false && response.AdvancePaymentLoad_JSON != null) {

                var AdvancePaymentLoad_JSONData = JSON.parse(response.AdvancePaymentLoad_JSON);
                Bill_PaymentPosting.AllAdvancePayments = [];

                $.each(AdvancePaymentLoad_JSONData, function (i, item) {

                    //AllAdvancePayments.push({ Date: item.PaymentDate, value: item.Balance, id: item.AdvPaymentId });
                    totalBalance += parseFloat(item.Balance);

                    if (parseFloat(item.Balance) > 0)
                        Bill_PaymentPosting.AllAdvancePayments.push({ id: item.AdvPaymentId, value: utility.RemoveTimeFromDate(null, item.PaymentDate) + " - " + item.Balance, Balance: item.Balance });


                });

                Bill_PaymentPosting.setAdvanceBalance(totalBalance);
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtAdvancePayment").autocomplete({

                    autoFocus: true,
                    source: Bill_PaymentPosting.AllAdvancePayments,
                    select: function (event, ui) {


                        setTimeout(function () {
                            $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfAdvancePaymentId").val(ui.item.id);
                            //start syed zia 02-02-2016, bug #PMS-3792
                            // $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientPaid").val(ui.item.Balance);

                            //end syed zia 02-02-2016, bug #PMS-3792

                            // $("#" + Bill_PaymentPosting.params["PanelID"] + " #txtPatientFullName").val(ui.item.value.split(" ")[2]);
                            Bill_PaymentPosting.setAdvanceBalance(ui.item.Balance);

                        }, 100);

                        //$("#hfpatientid").val(ui.item.id);
                    }
                });
            }
        });
    },


    ConvertToFloat: function (value) {

        return value == "" ? 0 : parseFloat(value);

    },
    setInsurancePriority: function (ev) {
        $('#' + Bill_PaymentPosting.params.PanelID + ' #txtPlanPriority').val($(ev).find('option:selected').attr('priority'));
    },



    BillPaymentPostingReset: function () {

        //$('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting').find('[data-plugin-datepicker]').each(function () {
        //    $(this).datepicker('setDate', new Date());
        //});

        $('#' + Bill_PaymentPosting.params.PanelID + ' #divSearchCharges').resetAllControls();

        $("#" + Bill_PaymentPosting.params.PanelID + " #lblCaseNumber").css("display", "inline");
        $("#" + Bill_PaymentPosting.params.PanelID + " #lnkCaseNumberEdit").css("display", "none");

        //$("#" + Bill_PaymentPosting.params.PanelID + " #hfPatientId").val("");
        //$("#" + Bill_PaymentPosting.params.PanelID + " #hfVisitId").val("");
        //$("#" + Bill_PaymentPosting.params.PanelID + " #hfCaseId").val("");
        //Begin: Feb 17th, 2016, Abdur Rehman Latif, PMS-3992
        //Bill_PaymentPosting.LoadCharges();
        //End: Feb 17th, 2016, Abdur Rehman Latif, PMS-3992

    },

    OpenPatientStatement: function () {
        var params = [];
        // params["mode"] = "Add";
        params["FromAdmin"] = "0";
        var tabid = Bill_PaymentPosting.params.TabID;
        if (Bill_PaymentPosting.params.BatchId) {
            params["BatchId"] = Bill_PaymentPosting.params.BatchId;
        }
        else {
            params["BatchId"] = "";
        }
        params["PatientID"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientId").val();
        //   params["PatientId"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #hfPatientId").val();
        //AST-295
        if (Bill_PaymentPosting.params.TabID == 'billTabPaymentBatchSearch' || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail' || Bill_PaymentPosting.params.TabID == 'Bill_ChargeSearch' || Bill_PaymentPosting.params.TabID == "Bill_FollowUpInsuranceAR_Detail" || Bill_PaymentPosting.params.TabID == "Bill_FollowUpPatientAR_Detail" || Bill_PaymentPosting.params.TabID == "ERADetail" || Bill_PaymentPosting.params.TabID == "Bill_ERA_Summary" || Bill_PaymentPosting.params.TabID == "EncounterChargeCapture" || Bill_PaymentPosting.params.TabID == "Bill_ERA_ElectronicEOB" || Bill_PaymentPosting.params.TabID == "mstrTabDashBoard") {
            params["ParentCtrl"] = 'Bill_PaymentPosting';
        } else {
            params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        }

        if (tabid && tabid.includes("EncounterChargeCapture")) {
            params["ParentCtrl"] = 'Bill_PaymentPosting';
        }
        LoadActionPan('Bill_PatientStatement', params, Bill_PaymentPosting.params.PanelID);

    },

    LoadVisitDetail: function (VisitId, PatientId, event) {
        var strMessage = "";
        if (event != null) {
            event.stopPropagation();
        }
        AppPrivileges.GetFormPrivileges("Charges", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                var params = [];
                params["FromAdmin"] = 0;
                params["ParentCtrl"] = 'Bill_PaymentPosting';

                params["VisitId"] = VisitId;
                params["patientID"] = PatientId;
                if (Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpPatientAR_Detail")
                    LoadActionPan('EncounterChargeCapture', params, "pnlBillFollowUpPatientARDetail");

                else if (Bill_PaymentPosting.params.ParentCtrl == "Bill_FollowUpInsuranceAR_Detail")
                    LoadActionPan('EncounterChargeCapture', params, "pnlBillFollowUpInsuranceARDetail");

                else
                    LoadActionPan('EncounterChargeCapture', params, Bill_PaymentPosting.params.PanelID);
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },

    DenialPayment: function () {
        AppPrivileges.GetFormPrivileges("Payment Posting", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                /****************/
                utility.myConfirm('Are you sure you want to post the Denial ?', function () {

                    $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfIsDenied").val("true");

                    var self = $('#' + Bill_PaymentPosting.params.PanelID + " #frmBillPaymentPosting");
                    var myJSON = self.getMyJSONByName();

                    Bill_PaymentPosting.SaveChargePayment(myJSON, "Insurance").done(function (response) {


                        if (response.status != false) {

                            var params = [];

                            params["mode"] = "edit";
                            params["VisitId"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfVisitId").val();

                            // information to be passed to eligibility
                            params["patientID"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientId").val();
                            //params["patientAccount"] = AccountNumber;
                            //params["patientLastName"] = LastName;
                            //params["patientFirstName"] = FirstName;
                            params["patientInsurancePlanId"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfPatientInsurancePlanId").val();
                            //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsurancePlanName").val(InsurancePlanName);

                            params["Provider"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtProvider").val();
                            params["ProviderId"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfProviderId").val();

                            params["FollowUpInsuranceARID"] = 0;

                            if (Bill_PaymentPosting.params.TabID == 'billTabPaymentBatchSearch' || Bill_PaymentPosting.params.TabID == "Bill_ChargeSearch" || Bill_PaymentPosting.params.TabID == 'paymentBatchDetail' || Bill_PaymentPosting.params.TabID == "Bill_FollowUpInsuranceAR_Detail" || Bill_PaymentPosting.params.TabID == "EncounterChargeCapture") {
                                params["ParentCtrl"] = 'Bill_PaymentPosting';

                            } else {
                                params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
                            }
                            //bug# 5012
                            params["ChargeId"] = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfChargeId").val();
                            //bug# 5012
                            //start syed zia PMS-4147, add parameter to handle payment posting screen panel id null issue in case of unload tab.
                            params["FromDenial"] = "true";
                            //end syed zia PMS-4147, add parameter to handle payment posting screen panel id null issue in case of unload tab.
                            LoadActionPan('Bill_FollowUpInsuranceAR_Detail', params);

                            //utility.DisplayMessages(response.message, 1);

                        }
                        else {
                            utility.DisplayMessages(response.Message, 3);
                        }


                    });

                }, function () {

                    //NO CASE

                },
                           '<b>Confirm Denial</b>'
                       );
                /***************/
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
    },



    //-----------------------------------

    ValidateAllowedAmountRecoupment: function (BalanceType) {
        var ChargeFee = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfChargeFee").val();
        ChargeFee = ChargeFee == "" ? 0 : ChargeFee;
        var InsBalance = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfInsBalance").val();
        InsBalance = InsBalance == "" ? 0 : InsBalance;
        var ObjWriteOff = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtRecoupmentWriteoff");
        var ObjAllowed = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtRecoupmentAllowed");
        var AllowedAmount = ObjAllowed.val();
        AllowedAmount = AllowedAmount == "" ? 0 : AllowedAmount;
        AllowedAmount = parseFloat(AllowedAmount);
        if (AllowedAmount > 0 && InsBalance > 0) {
            if (AllowedAmount <= InsBalance) {
                ObjWriteOff.val(parseFloat(InsBalance - AllowedAmount).toFixed(Number(globalAppdata.DecimalPlaces)));
            }
            else {
                ObjAllowed.val("");
                ObjWriteOff.val("");
            }
        }
    },

    ValidateWriteOffRecoupment: function () {
        var ObjAllowed = $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtRecoupmentAllowed");
        var AllowedAmount = ObjAllowed.val();
        AllowedAmount = AllowedAmount == "" ? 0 : AllowedAmount;
        AllowedAmount = parseFloat(AllowedAmount);
        if (AllowedAmount > 0) {
            $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #txtRecoupmentWriteoff").blur();
        }
    },

    IsCheckPosted: function (CheckNo, ChargeID) {
        if (ChargeID == null) {
            ChargeID = 0;
        }
        var objData = new JSON.constructor();
        objData["ChargeId"] = ChargeID;
        objData["checkNumber"] = CheckNo;
        objData["CommandType"] = "is_check_posted";
        var data = JSON.stringify(objData);
        return MDVisionService.PMSAPIService(data, "Payments", "PaymentPosting");
    },

    getLedgerAccount: function (Type, ApplyTo) {
        var ledgerAccount = $.grep(Bill_PaymentPosting.AllLedgerAccounts, function (v) {
            return v.Type == Type && (v.ApplyTo == ApplyTo || ApplyTo == null);
        });

        return ledgerAccount;
    },

    loadAllLedgerAccountDropDowns: function (response) {
        Bill_PaymentPosting.AllLedgerAccounts = response.AllLedgerAccounts_JSON;

        // CONTROLS
        var insPaidCtrl = $("#" + Bill_PaymentPosting.params.PanelID + " #divInsurancePayment #ddlInsurancePaid");
        var insWriteOffCtrl = $("#" + Bill_PaymentPosting.params.PanelID + " #divInsurancePayment #ddlInsuranceWriteoff");
        var recoupmentPaidCtrl = $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentPaid')
        var recoupmentWriteOffCtrl = $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentWriteoff');
        var patPaidCtrl = $('#' + Bill_PaymentPosting.params.PanelID + ' #divPatientPayment #ddlPatientPaid')
        var patDiscountCtrl = $("#" + Bill_PaymentPosting.params.PanelID + " #divPatientPayment #ddlPatientDiscount");

        //INSURANCE/RECOUPMENT PAYMENT LEDGER
        insPaidCtrl.empty().append($('<option />', { value: "", html: "- Select -", refvalue: "" }));
        recoupmentPaidCtrl.empty().append($('<option />', { value: "", html: "- Select -", refvalue: "" }));
        $.each(Bill_PaymentPosting.getLedgerAccount(2, 2), function (i, item) {
            insPaidCtrl.append($('<option />', { value: item.LedgerAccountId, html: item.ShortName, refvalue: item.IsSystem }));
            recoupmentPaidCtrl.append($('<option />', { value: item.LedgerAccountId, html: item.ShortName, refvalue: item.IsSystem }));
        });

        //INSURANCE/RECOUPMENT WRITEOFF LEDGER
        insWriteOffCtrl.empty().append($('<option />', { value: "", html: "- Select -", refvalue: "" }));
        recoupmentWriteOffCtrl.empty().append($('<option />', { value: "", html: "- Select -", refvalue: "" }));
        $.each(Bill_PaymentPosting.getLedgerAccount(3, 2), function (i, item) {
            insWriteOffCtrl.append($('<option />', { value: item.LedgerAccountId, html: item.ShortName + '- ' + item.Description, refvalue: item.IsSystem }));
            recoupmentWriteOffCtrl.append($('<option />', { value: item.LedgerAccountId, html: item.ShortName + '- ' + item.Description, refvalue: item.IsSystem }));
        });


        //PATENT PAYMENT LEDGER
        patPaidCtrl.empty().append($('<option />', { value: "", html: "- Select -", refvalue: "" }));
        $.each(Bill_PaymentPosting.getLedgerAccount(1), function (i, item) {
            patPaidCtrl.append($('<option />', { value: item.LedgerAccountId, html: item.ShortName, refvalue: item.IsSystem }));
        });


        //PATENT DISCOUNT LEDGER
        patDiscountCtrl.empty().append($('<option />', { value: "", html: "- Select -", refvalue: "" }));
        $.each(Bill_PaymentPosting.getLedgerAccount(4), function (i, item) {
            patDiscountCtrl.append($('<option />', { value: item.LedgerAccountId, html: item.ShortName, refvalue: item.IsSystem }));
        });

        setTimeout(function () {
            insPaidCtrl.val(1).prop("selected", true);
            insWriteOffCtrl.val(2).prop("selected", true);

            recoupmentPaidCtrl.val(1).prop("selected", true);
            recoupmentWriteOffCtrl.val(2).prop("selected", true);

            patPaidCtrl.val(3).prop("selected", true);
            patDiscountCtrl.val(6).prop("selected", true);
            if (Bill_PaymentPosting.params["IsFromCollectCopay"] == true) {
                $('#' + Bill_PaymentPosting.params.PanelID + ' #frmBillPaymentPosting #ddlPatientPaid').find('option:contains(Copay Payment)').prop("selected", true);
            }
        }, 500);



    },

    loadRemittanceCodesDropDowns: function (response) {
        var remitCodes = response.RemitCodes_JSON;

        var insRemitCodeCtrl = $("#" + Bill_PaymentPosting.params.PanelID + " #divInsurancePayment #ddlRemitCode");
        var recoupmentRemitCodeCtrl = $('#' + Bill_PaymentPosting.params.PanelID + ' #divRecoupmentPayment #ddlRecoupmentRemitCode')

        insRemitCodeCtrl.empty().append($('<option />', { value: "", html: "- Select -", refvalue: "" }));
        recoupmentRemitCodeCtrl.empty().append($('<option />', { value: "", html: "- Select -", refvalue: "" }));
        $.each(remitCodes, function (i, item) {
            insRemitCodeCtrl.append($('<option />', { value: item.Value, html: item.Name }));
            recoupmentRemitCodeCtrl.append($('<option />', { value: item.Value, html: item.Name }));
        });

    },
    RecoupmentNextResponsabilityHfField: function () {
        $("#frmBillPaymentPosting #ddlPatientInsurancePlan option").each(function (item, value) {
            if ($(this).is(":selected")) {
                var planId = $(this).next().attr("value");
                $("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #hfRecoupmentNextInsurancePlan").val(planId);
            }
        });
    },
}