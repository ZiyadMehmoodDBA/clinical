Bill_PatientResponsibilityPayment = {
    params: [],
    AllAdvancePayments: [],
    Load: function (params) {

        Bill_PatientResponsibilityPayment.params = params;
        if (Bill_PatientResponsibilityPayment.params["PanelID"] != 'pnlBillPatientResponsibilityPayment')
            self = $('#' + Bill_PatientResponsibilityPayment.params["PanelID"] + ' #pnlBillPatientResponsibilityPayment');
        else
            self = $('#pnlBillPatientResponsibilityPayment');
        //var self = $('#' + Bill_PatientResponsibilityPayment.params.PanelID);

        self.loadDropDowns(true).done(function () {
            Bill_PatientResponsibilityPayment.LoadCharges();
            self.find('#ddlPaymentType').trigger('onchange');
            Bill_PatientResponsibilityPayment.ValidatePaymentPosting();
            utility.CreateDatePicker(Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment #dtpCheckDate,#dtpDatePaid,#dtpExpiryDate', function () {
                $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment').bootstrapValidator('revalidateField', 'DatePaid');
            });
            Bill_PatientResponsibilityPayment.BindAdvancePayment();
            Bill_PatientResponsibilityPayment.FillPatientLedgerAccount();
        });
    },
    LoadCharges: function () {
        $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment #btnSavePaymentPosting').attr('disabled', 'disabled');
        var infoArr = new Array();
        var dummy = new Object();
        dummy['DOSTo'] = Bill_PatientResponsibilityPayment.params.DOSTo;
        dummy['DOSFrom'] = Bill_PatientResponsibilityPayment.params.DOSFrom;
        dummy['PatientId'] = Bill_PatientResponsibilityPayment.params.patientID;
        dummy['FacilityId'] = Bill_PatientResponsibilityPayment.params.facilityId;
        infoArr.push(dummy);

        Bill_PatientStatement.PrintPatientStatements(JSON.stringify(infoArr)).done(function (response) {
            if (response.status) {
                var allCharges = JSON.parse(JSON.parse(response.Statement_JSON)[0].StatementDetail);
                var statementHeader = JSON.parse(response.Statement_JSON)[0].StatementHeader;
                //start syed zia 18-02-2016, resolve parsor exception in case of table html 
                var headerInfo = "";
                if (statementHeader.indexOf("<table") != -1) {
                    var replaceText = statementHeader.substring(statementHeader.indexOf("<table"), statementHeader.lastIndexOf("table>")).replace(/"/g, "'");
                    headerInfo = JSON.parse(statementHeader.substring(0, statementHeader.indexOf("<table")) + replaceText + statementHeader.substring(statementHeader.lastIndexOf("table>")))[0];
                }
                else {
                    headerInfo = JSON.parse(JSON.parse(response.Statement_JSON)[0].StatementHeader)[0];
                }

                //end syed zia 18-02-2016, resolve parsor exception in case of table html 
                $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #StatementCharges #dgvStatementCharges").dataTable().fnDestroy();
                $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #StatementCharges #dgvStatementCharges tbody").find("tr").remove();
                if (allCharges.length > 0) {//response.ChargeCount
                    $.each(allCharges, function (i, item) {
                        if (item.Procedure != "") {
                            var DemographicsMethod = "utility.PatientDemographics('" + headerInfo.PatientId + "', 'Bill_PatientResponsibilityPayment', event);";
                            var VisitDetail = "utility.LoadVisitDetail('" + item.VisitId + "', '" + headerInfo.PatientId + "', 'Bill_PatientResponsibilityPayment', event);";
                            var $row = $('<tr/>');
                            $row.attr("onclick", "utility.SelectGridRow($('#gvStatementCharge_row" + i + "'));");
                            $row.attr("id", "gvStatementCharge_row" + i);
                            $row.attr("ChargeID", item.ChargeCapId);

                            $row.append('<td style="display:none;">' + item.ChargeCapId + '</td><td><input type="checkbox" onclick="Bill_PatientResponsibilityPayment.checkPayment(this);" chargeCapId = ' + item.ChargeCapId + ' balance=' + item.PatBalance + ' visitId = ' + item.VisitId + ' facilityId=' + item.ChargeFacilityId + ' providerId=' + item.ChargeProviderId + ' patientId=' + headerInfo.PatientId + ' DOS=' + utility.RemoveTimeFromDate(null, item.Date) + ' id="chkChargeCapID' + item.ChargeCapId + '" /></td><td><a href="#" onclick="' + DemographicsMethod + '"  title="View Patient">' + headerInfo.AccountNumber + '</td><td>' + utility.RemoveTimeFromDate(null, item.Date) + '</td><td><a href="#" onclick="' + VisitDetail + '"  title="View Claim Detail">' + item.ClaimNumber + '</td><td>' + item.Procedure + '</td><td>' + item.Units + '</td><td>' + globalAppdata.DefaultCurrency + Number(item.Charges).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td>' + globalAppdata.DefaultCurrency + Number(item.PatBalance).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td>');
                            $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #StatementCharges #dgvStatementCharges tbody").last().append($row);
                        }
                    });
                }
                else {
                    $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #divStatementPaging").css("display", "none");
                    $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #StatementCharges #dgvStatementCharges").DataTable({
                        "language": {
                            "emptyTable": "No Charges Found"
                        }, "autoWidth": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
                    });
                }
                if ($.fn.dataTable.isDataTable("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #StatementCharges #dgvStatementCharges"))
                    ;
                else
                    $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #StatementCharges #dgvStatementCharges").DataTable({ "bInfo": false, "bPaginate": false, "bLengthChange": false, "autoWidth": false, "bFilter": true, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown
            }
        });
    },
    checkUncheckAll: function (obj) {
        $('#' + Bill_PatientResponsibilityPayment.params["PanelID"] + ' #StatementCharges #dgvStatementCharges input[id*="chkChargeCapID"]').prop("checked", $(obj).prop("checked"));
        Bill_PatientResponsibilityPayment.checkPayment();
    },
    ShowHidePaymentTypeDiv: function (value) {

        $('#' + Bill_PatientResponsibilityPayment.params.PanelID + " #frmBillPatientResponsibilityPayment div[id*='PaymentType']").each(function () {
            if (value == 2 && $(this).attr("id") == "PaymentTypeCheck") {
                $(this).css("display", "inline");
                $('#' + Bill_PatientResponsibilityPayment.params.PanelID + " #frmBillPatientResponsibilityPayment #divAdvancePayment").hide();
            }
            else if (value == 3 && $(this).attr("id") == "PaymentTypeCreditCard") {
                $(this).css("display", "inline");
                $('#' + Bill_PatientResponsibilityPayment.params.PanelID + " #frmBillPatientResponsibilityPayment #divAdvancePayment").hide();
            }
            else if (value == 4) {
                $(this).css("display", "none");
                $('#' + Bill_PatientResponsibilityPayment.params.PanelID + " #frmBillPatientResponsibilityPayment #divAdvancePayment").show();
            }
            else {
                $(this).css("display", "none");
                $('#' + Bill_PatientResponsibilityPayment.params.PanelID + " #frmBillPatientResponsibilityPayment #divAdvancePayment").hide();
            }
        });
    },
    OpenAdvancePayment: function () {
        var params = [];
        params["patientID"] = Bill_PatientResponsibilityPayment.params.patientID;
        params["ParentCtrl"] = "Bill_PatientResponsibilityPayment";

        LoadActionPan('Patient_AdvancePayment', params, "pnlBillPatientResponsibilityPayment");
    },
    checkPayment: function (ev) {
        var totalBalance = '00.00';
        var selectedObj = $('#' + Bill_PatientResponsibilityPayment.params["PanelID"] + ' input[id*="chkChargeCapID"]:checked').map(function (i, item) {
            totalBalance = Number(totalBalance) + Number($(item).attr('balance'));
            return this;
        }).get();

        if (selectedObj.length > 0) {
            $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #paymentContainer').removeClass('disableAll');
            $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment #btnSavePaymentPosting').removeAttr('disabled');
        }
        else {
            $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #paymentContainer').addClass('disableAll');
            $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment #btnSavePaymentPosting').attr('disabled', 'disabled');
        }
        $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #divTotalBalance #lblTotalBalance').text(utility.convertToFigure(totalBalance,true));
    },
    ValidatePaymentPosting: function () {
        utility.ClearFormValidation('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment');
        $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment')
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
                   
                   PatientPaidAccount: {
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
                   }
               }


           }).on('success.form.bv', function (e) {
               e.preventDefault();
               Bill_PatientResponsibilityPayment.PatientResponsibilityPaymentSave();
           });
    },
    PatientResponsibilityPaymentSave: function () {
        AppPrivileges.GetFormPrivileges("Payment Posting", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                   
                if (Number($('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment #txtPatientPaid').val()) > Number($('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment #lblTotalBalance').text().replace('$', ''))) {
                    utility.myConfirm("Paid amount is greater than the patient balance. Would you like to pay  " + $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment #lblTotalBalance').text() + " ?", function () {
                        Bill_PatientResponsibilityPayment.SavePayment();
                    },
                     function () {
                         return;
                     }, "<b>Confirm submission</b>");
                }
                else {
                    Bill_PatientResponsibilityPayment.SavePayment();
                }
            }
            else {
                utility.DisplayMessages(strMessage, 2);


            }

        });

    },
   
    SavePayment: function()
    {
        var amount = Number($('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #frmBillPatientResponsibilityPayment #txtPatientPaid').val());
        if (amount > 0) {
            var allCharges = new Array();
            var selectedObj = $('#' + Bill_PatientResponsibilityPayment.params["PanelID"] + ' input[id*="chkChargeCapID"]:checked').map(function (i, item) {
                var objCharge = new Object();
                objCharge['PatientID'] = $(item).attr('patientid');
                objCharge['DOS'] = $(item).attr('DOS');
                objCharge['FacilityID'] = $(item).attr('facilityid');
                objCharge['ChargeCapID'] = $(item).attr('chargeCapId');
                objCharge['VisitID'] = $(item).attr('visitid');
                objCharge['Balance'] = $(item).attr('balance');
                objCharge['ProviderID'] = $(item).attr('providerid');
                allCharges.push(objCharge);
                return this;
            }).get();
            if (selectedObj.length > 0) {
                allCharges = allCharges.sort(function (a, b) {
                    return a.DOS > b.DOS;
                });
                var self = null;
                if (Bill_PatientResponsibilityPayment.params["PanelID"] != 'pnlBillPatientResponsibilityPayment')
                    self = $('#' + Bill_PatientResponsibilityPayment.params["PanelID"] + ' #pnlBillPatientResponsibilityPayment');
                else
                    self = $('#pnlBillPatientResponsibilityPayment');
                // self = $('#' + Bill_PatientResponsibilityPayment.params.PanelID);
                var myJSON = self.getMyJSON();

                Bill_PatientResponsibilityPayment.SavePatientResponsibilityPayment(myJSON, JSON.stringify(allCharges)).done(function (response) {
                    if (response.status != false) {
                        Bill_PatientResponsibilityPayment.LoadCharges();
                        utility.DisplayMessages(response.Message, 1);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }


                });
            }
            else {
                utility.DisplayMessages("Please select charge(s)", 3);
            }
        }
        else {
            utility.DisplayMessages("Please specify valid amount", 3);
        }
    },
    BindAdvancePayment: function () {

        var totalBalance = 0;
        var jsonArray = { "hfPatientId": Bill_PatientResponsibilityPayment.params.patientID, "hfFacility": "-1", "dtpPaidFrom": "", "dtpPaidTo": "", "ddlPaymentType": "" };
        Patient_AdvancePayment.AdvancePaymentSearch(JSON.stringify(jsonArray)).done(function (response) {
            if (response.status != false && response.AdvancePaymentLoad_JSON != null) {

                var AdvancePaymentLoad_JSONData = JSON.parse(response.AdvancePaymentLoad_JSON);
                Bill_PatientResponsibilityPayment.AllAdvancePayments = [];

                $.each(AdvancePaymentLoad_JSONData, function (i, item) {

                    //AllAdvancePayments.push({ Date: item.PaymentDate, value: item.Balance, id: item.AdvPaymentId });
                    totalBalance += parseFloat(item.Balance);

                    if (parseFloat(item.Balance) > 0)
                        Bill_PatientResponsibilityPayment.AllAdvancePayments.push({ id: item.AdvPaymentId, value: utility.RemoveTimeFromDate(null, item.PaymentDate) + " - " + item.Balance, Balance: item.Balance });


                });

                Bill_PatientResponsibilityPayment.setAdvanceBalance(totalBalance);
                $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #txtAdvancePayment").autocomplete({

                    autoFocus: true,
                    source: Bill_PatientResponsibilityPayment.AllAdvancePayments,
                    select: function (event, ui) {


                        setTimeout(function () {
                            $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #hfAdvancePaymentId").val(ui.item.id);
                            $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #txtPatientPaid").val(ui.item.Balance);

                            Bill_PatientResponsibilityPayment.setAdvanceBalance(ui.item.Balance);

                        }, 100);

                        //$("#hfpatientid").val(ui.item.id);
                    }
                });
            }
        });
    },
    setAdvanceBalance: function (AdvanceBalance) {

        $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #frmBillPatientResponsibilityPayment #hfAdvanceBalance").val(0);
        $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #frmBillPatientResponsibilityPayment #lblAdvanceBalance").text(utility.convertToFigure(0, true));

        $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #frmBillPatientResponsibilityPayment #hfAdvanceBalance").val(AdvanceBalance);

        if (AdvanceBalance >= 0) {
            $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #frmBillPatientResponsibilityPayment #lblAdvanceBalance").removeClass('red');
        } else {
            $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #frmBillPatientResponsibilityPayment #lblAdvanceBalance").addClass('red');
        }
        $("#" + Bill_PatientResponsibilityPayment.params["PanelID"] + " #frmBillPatientResponsibilityPayment #lblAdvanceBalance").text(utility.convertToFigure(AdvanceBalance, true));


    },
    FillPatientLedgerAccount: function () {
        $.when(CacheManager.BindDropDownsByTwoIDs('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #ddlPatientPaid', 'GetLedgerAccount', true, 1, 1).done(function () {

            $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #ddlPatientPaid option').each(function () {
                if ($(this).attr("RefValue") != null && $(this).attr("RefValue").toLowerCase() == "true") {
                    $(this).attr("selected", "selected");
                    $('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #ddlPatientPaid').trigger('change');
                }
                else {
                    $(this).removeAttr("selected");
                }
            });

        }));

    },
    SavePatientResponsibilityPayment: function (PaymentData, ChargeData) {
        var data = "PaymentData=" + PaymentData + "&ChargeData=" + ChargeData + "&PmtBatchId=" + Bill_PatientResponsibilityPayment.params.BatchId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "BILLING_PAYMENT_POSTING", "SAVE_PAT_RES_PAYMENT");
    },
    validateTotalAmount: function (ev) {
        var totalAmount = Number($('#' + Bill_PatientResponsibilityPayment.params.PanelID + ' #divTotalBalance #lblTotalBalance').text());
        var enteredAmount = Number($(ev).val());

        if (enteredAmount > totalAmount) {
            $(ev).val('');
            utility.DisplayMessages("Amount shouldn't be greater than balance", 3);

        }
    },
    UnLoad: function () {
        UnloadActionPan(Bill_PatientResponsibilityPayment.params.ParentCtrl, 'Bill_PatientResponsibilityPayment', null, Bill_PatientResponsibilityPayment.params.PanelID);
    }

}