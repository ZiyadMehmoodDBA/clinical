BillLedgerDetail = {
    bIsFirstLoad: true,
    params: [],

    Load: function (params) {

        BillLedgerDetail.params = params;

        if (BillLedgerDetail.params["PanelID"] != "pnlBillLedgerDetail") {
            BillLedgerDetail.params["PanelID"] = BillLedgerDetail.params["PanelID"] + " #pnlBillLedgerDetail";
        }

       


        var self = $('#' + BillLedgerDetail.params.PanelID);
        self.loadDropDowns(true).done(function () {

            utility.CreateDatePicker(BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #dtpCheckDate', function () {

                // on-change callback method 

                $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail').bootstrapValidator('revalidateField', 'checkDate');

            });

            utility.creditCardExpiryDate(BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #dtpExpiryDate', function () {
                // on-change callback method
                $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail').bootstrapValidator('revalidateField', 'expiryDate');
            });
            BillLedgerDetail.FillLedger(BillLedgerDetail.params.PaymentId);
        });
    },


    FillLedger: function (PaymentId) {

        if (BillLedgerDetail.params.mode == "Edit") {
            BillLedgerDetail.FillBillLedger(PaymentId).done(function (response) {
                if (response.status != false) {

                    var PatientPayments_detail = JSON.parse(response.PatientPayments_JSON);
                    var self = $("#pnlBillLedgerDetail");
                    //By default check and credit card fields are hidden
                    //Set Calander value                    
                    //prd-19 if payment post by Mirth.
                    if (PatientPayments_detail.txtEnteredBy == "Mirth") {
                        $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #btnHistoryPaymentPosting').removeClass('hidden');
                        $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #ddlPaymentType').attr('disabled', false);
                        if (PatientPayments_detail.ddlPaymentType == "2") {
                            $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #txtCheckNumber').attr('disabled', false);
                            $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #dtpCheckDate').attr('disabled', false);
                        }
                        if (PatientPayments_detail.ddlPaymentType == "3") {
                            $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #txtCardNumber').attr('disabled', false);
                            $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #ddlCreditCardType').attr('disabled', false);
                            $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #dtpExpiryDate').attr('disabled', false);
                        }
                        BillLedgerDetail.params['EnterdBy'] = 'Mirth';
                    }
                    else {
                        $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #btnHistoryPaymentPosting').addClass('hidden');
                    }
                    if (PatientPayments_detail.ddlPaymentType == 2) {
                        $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #PaymentTypeCheck').show();
                    }
                        //in case of credit card show credit card div  
              
                    else if (PatientPayments_detail.ddlPaymentType == 3) {
                        $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #PaymentTypeCreditCard').show();
                    }
                    else {
                        // in case of no selection or cash or advance payment hide credit card and check feilds
                        $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #PaymentTypeCheck').hide();
                        $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #PaymentTypeCreditCard').hide();
                    }

                    utility.bindMyJSON(true, PatientPayments_detail, false, self).done(function () {

                        $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail').data('serialize', $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail').serialize());

                    });
                    // prd-19 set expiry date.
                    if (PatientPayments_detail.dtpExpiryDate)
                        $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #dtpExpiryDate').val(PatientPayments_detail.dtpExpiryDate);
                   
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },

    BillLedgerUpdate: function () {
        var Comment = $('#' + BillLedgerDetail.params.PanelID + ' #txtInsuranceComments').val();        
        var ChkPrintonPatStmt = $('#chkShwPatientStmt').prop('checked');
        var PaymentTypeId = $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #ddlPaymentType').val();
        var CheckDate = $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #dtpCheckDate').val();
        var CardNumber = $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #txtCardNumber').val();
        var CreditCardType = $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #ddlCreditCardType').val();
        var CraditCardExpiryDate = $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #dtpExpiryDate').val();
        var checkNumber = $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail #txtCheckNumber').val();
        BillLedgerDetail.UpdateBillLedger(BillLedgerDetail.params.PaymentId, Comment, ChkPrintonPatStmt, PaymentTypeId, CheckDate, CardNumber, CreditCardType, CraditCardExpiryDate, checkNumber).done(function (response) {
            if (response.status != false) {
                utility.DisplayMessages(response.Message, 1);
                $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail').data('serialize', $('#' + BillLedgerDetail.params.PanelID + ' #frmBillLedgerDetail').serialize());
                BillLedgerDetail.UnLoad();
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
    },

    ShowHidePaymentTypeDiv: function (value) {

        $('#' + BillLedgerDetail.params.PanelID + " #frmBillLedgerDetail div[id*='PaymentType']").each(function () {
            if (value == 2 && $(this).attr("id") == "PaymentTypeCheck") {
                // prd19- payment post by Mirth user then
                if (BillLedgerDetail.params['EnterdBy'] && BillLedgerDetail.params['EnterdBy'] == 'Mirth') {
                    $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #txtCheckNumber').attr('disabled', false);
                    $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #dtpCheckDate').attr('disabled', false);
                }
                $(this).css("display", "inline");
                //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").hide();
                $('#' + BillLedgerDetail.params.PanelID + " #frmBillLedgerDetail #divAdvancePayment").hide();
            }
            else if (value == 3 && $(this).attr("id") == "PaymentTypeCreditCard") {
                // prd19- payment post by Mirth user then
                if (BillLedgerDetail.params['EnterdBy'] && BillLedgerDetail.params['EnterdBy'] == 'Mirth') {
                    $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #txtCardNumber').attr('disabled', false);
                    $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #ddlCreditCardType').attr('disabled', false);
                    $('#' + BillLedgerDetail.params.PanelID + '  #frmBillLedgerDetail #dtpExpiryDate').attr('disabled', false);
               
                }
                $(this).css("display", "inline");

                //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").hide();
                $('#' + BillLedgerDetail.params.PanelID + " #frmBillLedgerDetail #divAdvancePayment").hide();
            }
            else if (value == 4) {

                $(this).css("display", "none");
                //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").show();
                $('#' + BillLedgerDetail.params.PanelID + " #frmBillLedgerDetail #divAdvancePayment").show();
            }

            else {
                $(this).css("display", "none");
                //$("#" + Bill_PaymentPosting.params["PanelID"] + " #frmBillPaymentPosting #divAdvanceBalance").hide();
                $('#' + BillLedgerDetail.params.PanelID + " #frmBillLedgerDetail #divAdvancePayment").hide();
            }
        });
    },

    FillBillLedger: function (PaymentId) {

        var data = "PaymentID=" + PaymentId;
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_POSTING", "FILL_BILL_LEDGER");

    },

    UpdateBillLedger: function (PaymentId, Comment, ChkPrintonPatStmt, PaymentTypeId, CheckDate, CardNumber, CreditCardType, CraditCardExpiryDate, checkNumber) {
        var data;
        // prd-19 if user update patient check number.
        if (checkNumber) {
             data = "PaymentID=" + PaymentId + "&Comment=" + Comment + "&ChkPrintonPatStmt=" + ChkPrintonPatStmt + "&PaymentTypeId=" + PaymentTypeId +
            "&CheckDate=" + CheckDate + "&CardNumber=" + CardNumber + "&CreditCardTypeId=" + CreditCardType + "&CraditCardExpiryDate=" + CraditCardExpiryDate + "&checkNumber=" + checkNumber;

        }
        else if (CardNumber) {
            data = "PaymentID=" + PaymentId + "&Comment=" + Comment + "&ChkPrintonPatStmt=" + ChkPrintonPatStmt + "&PaymentTypeId=" + PaymentTypeId +
           "&CheckDate=" + CheckDate + "&CardNumber=" + CardNumber + "&CreditCardTypeId=" + CreditCardType + "&CraditCardExpiryDate=" + CraditCardExpiryDate + "&checkNumber=" + CardNumber;
        }
        else {
            data = "PaymentID=" + PaymentId + "&Comment=" + Comment + "&ChkPrintonPatStmt=" + ChkPrintonPatStmt + "&PaymentTypeId=" + PaymentTypeId +
           "&CheckDate=" + CheckDate + "&CardNumber=" + CardNumber + "&CreditCardTypeId=" + CreditCardType + "&CraditCardExpiryDate=" + CraditCardExpiryDate +"&checkNumber=" + checkNumber;;
        }
        
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_POSTING", "UPDATE_BILL_LEDGER");
    },

    UnLoad: function () {


        if (BillLedgerDetail.params["ParentCtrl"] == "Bill_PaymentPosting" && BillLedgerDetail.params.PaymentRef == "EncounterChargeCapture") {

            UnloadActionPan(BillLedgerDetail.params["ParentCtrl"]);
        }
        else
        if (BillLedgerDetail.params["ParentCtrl"] == "Bill_PaymentPosting" && BillLedgerDetail.params.PaymentRef != null) {
            UnloadActionPan(BillLedgerDetail.params.ParentCtrl, "BillLedgerDetail", null, BillLedgerDetail.params.PaymentRef);
        }
        else if (BillLedgerDetail.params["ParentCtrl"] != null) {
            UnloadActionPan(BillLedgerDetail.params["ParentCtrl"]);
        }
            else {
            UnloadActionPan();
            }
        //}

        

        //utility.UnLoadDialog("BillLedgerDetail", function () {
        //    UnloadActionPan();
        //}, function () {
        //    UnloadActionPan();
        //});
    },
    // prd-19 show history for patient payment.
    ShowHistory: function () {
        var PanelID = 'pnlBillLedgerDetail';
        var ParentCtrl = 'BillLedgerDetail';
        var ProfileName = 'Payment Posting';
        var DBTableName = 'PatientPayments';
        var ColumnKeyId = BillLedgerDetail.params.PaymentId;

        utility.ShowHistory(ParentCtrl, PanelID, ProfileName, DBTableName, ColumnKeyId);
    },
};




