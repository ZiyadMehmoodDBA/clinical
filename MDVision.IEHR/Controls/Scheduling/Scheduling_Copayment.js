var planAppCopay = 0;
//var planSpecialistCopay = 0;
var copaySum = 0;
schcopayment = {
    bIsFirstLoad: true,
    bIsExit: false,
    params: [],
    AllAdvancePayments: [],
    Load: function (params) {
        schcopayment.params = params;

        if (schcopayment.bIsFirstLoad) {
            schcopayment.bIsFirstLoad = false;
        }
        //initialization of date-pickers.
        schcopayment.InitializeControl();
        var self = $('#copaymentpanel');

        self.loadDropDowns(true).done(function () {
            planAppCopay = 0;
            //planSpecialistCopay = 0;
            copaySum = 0;

            $('#schcopayment #hfAppointmentId').val(schcopayment.params.AppointmentId);
            $('#schcopayment #hfPatientId').val(schcopayment.params.PatientId);
            $('#schcopayment #hfProviderId').val(schcopayment.params.ProviderId);
            $('#schcopayment #hfFacilityId').val(schcopayment.params.FacilityId);

            var visitid = schcopayment.params.PatientVisitId;
            var visitname = schcopayment.params.PatientVisitName;

            //schcopayment.ProviderSearch(schcopayment.params.ProviderId);
            $('select[id=ddlPaymentType] option:eq(0)').prop('selected', 'selected');
            schcopayment.FillDiscountAC().done(function () {
                schcopayment.FillPaidAC().done(function () {

                    schcopayment.BindAdvancePayment(schcopayment.params.PatientId);
                    if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked") || (schcopayment.params["ParentCtrl"] == "schTabMultipleView" && schcopayment.params["ResourceId"])) {
                        schcheckin.FillPatientData(schcopayment.params.PatientId, schcopayment.params.AppointmentId).done(function (response) {
                            if (response.status != false) {
                                var patientappointments = JSON.parse(response.PatientAppointment_JSON);
                                $('#frmCopayment #hfProviderId').val(patientappointments[0].ProviderId);
                            }
                            schcopayment.LoadPatientCopayment();
                            if (visitid == 0)
                                schcopayment.SelectPatientPayments(null, schcopayment.params.AppointmentId, null);
                            else if (visitid != 0)
                                schcopayment.SelectPatientPayments(null, schcopayment.params.AppointmentId, schcopayment.params.PatientVisitId);
                        });
                    } else {
                        schcopayment.LoadPatientCopayment();
                        if (visitid == 0) {
                            schcopayment.SelectPatientPayments(null, schcopayment.params.AppointmentId, null);
                        }
                        else if (visitid != 0) {
                            schcopayment.SelectPatientPayments(null, schcopayment.params.AppointmentId, schcopayment.params.PatientVisitId);
                        }
                    }
                    //schcopayment.LoadPatientCopayment();
                    //schcopayment.SelectPatientCopaymentByIDs(schcopayment.params.AppointmentId, null);

                });
            });

           
        });
    },

    InitializeControl: function () {


        //initialization of date-pickers.
        utility.creditCardExpiryDate('frmCopayment #dpExpiryDate', function () {

            // on-change callback method 
            $('#frmCopayment').bootstrapValidator('revalidateField', 'ExpiryDate');
        });
    },

    LoadPatientCopayment: function () {

        schcopayment.ValidatePatientCopayment();


    },

    FillDiscountAC: function () {

        var dfd = new $.Deferred();
        CacheManager.BindDropDownsByTwoIDs('#schcopayment #ddlDiscountLedger', 'GetLedgerAccount', true, 4, 3).done(function () {

            $('select[id=ddlDiscountLedger] option:eq(1)').prop('selected', 'selected');

            dfd.resolve(true);
        });

        return dfd.promise();
    },

    FillPaidAC: function () {
        // Begin: Author, Abdur Rehman, Changes for PMS-1538 - Reverted these changes as per the comment on the Task
        //var dfd = new $.Deferred();
        //CacheManager.BindDropDownsByTwoIDs('#schcopayment #ddlPaidLedger', 'GetLedgerAccount', true, 1, -1).done(function () {

        //    $("select[id=ddlPaidLedger] option:contains('Copay Payment')").prop('selected', 'selected');
        //    dfd.resolve(true);
        //});

        //return dfd.promise();
        //End 

        var dfd = new $.Deferred();
        CacheManager.BindDropDownsByTwoIDs('#schcopayment #ddlPaidLedger', 'GetLedgerAccount', true, 1, 3).done(function () {

            $('select[id=ddlPaidLedger] option:eq(1)').prop('selected', 'selected');
            dfd.resolve(true);
        });

        return dfd.promise();
    },


    //LoadPatientCoPayment: function (AppointmentID, VisitId) {       
    //    schcopayment.SelectPatCopaymentDetailIDs(AppointmentID, VisitId).done(function (response) {

    //        if (response.status != false) {
    //            var patcopayment = JSON.parse(response.PatientCoPayments_JSON);

    //            planAppCopay = parseFloat(patcopayment[0].Copay);
    //            return planAppCopay;

    //        }
    //        else {
    //            utility.DisplayMessages(response.Message, 3);
    //        }
    //    });
    //},

    SelectPatientPayments: function (PaymentId, AppointmentID, VisitId) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Copayment", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#schcopayment #pnlCopayment_Result").css("display") == "none") {
                    $("#schcopayment #pnlCopayment_Result").show();
                }

                schcopayment.SelectPatPayments(PaymentId, AppointmentID, VisitId).done(function (response) {
                    var plancopay = JSON.parse(response.PatientCoPayments_JSON);

                    copaySum = 0;

                    if (plancopay.length != '0') {
                        if (plancopay[0].IsSpecialist == "True") {
                            planAppCopay = plancopay[0].SpecialistCopay;
                        }
                        else if (plancopay[0].IsSpecialist == "False") {
                            planAppCopay = plancopay[0].Copay;
                        }
                        //planSpecialistCopay = plancopay[0].SpecialistCopay;
                    }

                    if (response.status != false) {
                        var patcopayment = JSON.parse(response.PatientPayments_JSON);


                        $.each(patcopayment, function (i, item) {
                            if (patcopayment[i].PaidAmountDr != "" && patcopayment[i].IsRefund != 'True') {

                                copaySum = parseFloat(copaySum + parseFloat(patcopayment[i].PaidAmountDr));
                            }
                        });
                        //var sum = planAppCopay - copaySum;
                        //if ($('#schcopayment #rdPCP').is(':checked')) {

                        var sum = planAppCopay - copaySum;

                        //}
                        // else if ($('#schcopayment #rdSpecialist').is(':checked')) {

                        //var sum = planSpecialistCopay - copaySum;

                        //}
                        if (sum <= 0) {

                            if (sum == 0) {
                                // $("#schcopayment #txtPaid").attr("disabled", "disabled");
                                $("#schcopayment #txtDiscount").attr("disabled", "disabled");
                                // $("#schcopayment #btnsave").addClass('disableAll');
                                $("#schcopayment #txtBalance").val(sum.toFixed(Number(globalAppdata.DecimalPlaces)));
                                $("#schcopayment #txtBalance").text(sum.toFixed(Number(globalAppdata.DecimalPlaces)));
                                $("#schcopayment #txtPaid").val(sum.toFixed(Number(globalAppdata.DecimalPlaces)));
                                $("#schcopayment #txtPaid").text(sum.toFixed(Number(globalAppdata.DecimalPlaces)));
                            }
                            else {
                                // $("#schcopayment #txtPaid").attr("disabled", "disabled");
                                $("#schcopayment #txtDiscount").attr("disabled", "disabled");
                                // $("#schcopayment #btnsave").addClass('disableAll');
                                $("#schcopayment #txtBalance").val('(' + sum.toString().replace('-', '') + ')');
                                $("#schcopayment #txtBalance").text('(' + sum.toString().replace('-', '') + ')');
                            }
                        }

                        else {
                            $("#schcopayment #txtBalance").val(sum.toFixed(Number(globalAppdata.DecimalPlaces)));
                            $("#schcopayment #txtBalance").text(sum.toFixed(Number(globalAppdata.DecimalPlaces)));
                            //Edit By Mohsin Bug# PMS-3267

                            //$("#schcopayment #txtPaid").val('0');
                            //$("#schcopayment #txtPaid").text('0');
                            $("#schcopayment #txtPaid").val(sum.toFixed(Number(globalAppdata.DecimalPlaces)));
                            $("#schcopayment #txtPaid").text(sum.toFixed(Number(globalAppdata.DecimalPlaces)));

                            //END Edit By Mohsin Bug# PMS-3267

                            // $("#schcopayment #btnsave").removeClass('disableAll');
                            $('#frmCopayment').data('bootstrapValidator').enableFieldValidators('DiscountAC', false);
                            $('#frmCopayment').data('bootstrapValidator').enableFieldValidators('Discount', false);
                            $('#frmCopayment').data('bootstrapValidator').enableFieldValidators('PaidAC', true);

                            if (sum > 0) {
                                //  $("#schcopayment #txtPaid").attr("disabled", false);
                                $("#schcopayment #txtDiscount").attr("disabled", false);
                            }
                        }
                        var self = $("#schcopayment");
                        schcopayment.PatientCopaymentGridLoad(response);

                        //serialize form
                        $('#frmCopayment').data('serialize', $('#frmCopayment').serialize());
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        schcopayment.PatientCopaymentGridLoad(response);

                        //serialize form
                        $('#frmCopayment').data('serialize', $('#frmCopayment').serialize());
                    }
                });
                $('#schcopayment #dpDatePaid').val($.datepicker.formatDate('mm/dd/yy', new Date()));
            }
            else
                utility.DisplayMessages(strMessage, 2);

          
            //serialize form
            $('#frmCopayment').data('serialize', $('#frmCopayment').serialize());
        });
    },

    PatientCopaymentGridLoad: function (response) {
        $("#dgvCopayment").dataTable().fnDestroy();
        $("#pnlCopayment_Result #dgvCopayment tbody").find("tr").remove();
        if (response.PatientPaymentsCount > 0) {
            var totalamtpaid = 0;
            var totalamtdis = 0;

            var t = 0;
            var PatientPaymentsJSONData = JSON.parse(response.PatientPayments_JSON);
            if (PatientPaymentsJSONData[0].AccountNumber != "") {

                $.each(PatientPaymentsJSONData, function (i, item) {
                    t++;
                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#dgvCopayment_row" + item.AppointmentId + "'))");
                    $row.attr("id", "dgvCopayment_row" + item.PaymentId);
                    $row.attr("PaymentId", item.PaymentId);

                    if (PatientPaymentsJSONData[i].IsRefund == "True") {
                        if (item.PaidAmountDr != "") {

                            $row.append('<td style="display:none; border:solid 1px green!important;">' + item.PaymentId + '</td><td style="border:solid 1px green!important;"></td><td style="border:solid 1px green!important;">' + item.PaymentDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td style="border:solid 1px green!important;">' + globalAppdata.DefaultCurrency + parseFloat(item.PaidAmountDr).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="border:solid 1px green!important;">' + item.PmtTypeName + '</td><td style="border:solid 1px green!important;">' + item.CheckNo + '</td><td style="border:solid 1px green!important;">' + item.LedgerAccountName + '</td > <td style="border:solid 1px green!important;">' + item.CreatedOn + ' </td> <td style="border:solid 1px green!important;">' + item.CreatedByName + ' </td>');
                        }
                        if (item.PaidAmountCr != "") {

                            $row.append('<td style="display:none; border:solid 1px red!important;">' + item.PaymentId + '</td><td style="border:solid 1px red!important;"></td><td style="border:solid 1px red!important;">' + item.PaymentDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td style="border:solid 1px red!important; color:red;font-weight:bold">(' + globalAppdata.DefaultCurrency + parseFloat(item.PaidAmountCr).toFixed(Number(globalAppdata.DecimalPlaces)) + ')</td><td style="border:solid 1px red!important;">' + item.PmtTypeName + '</td><td style="border:solid 1px red!important;">' + item.CheckNo + '</td><td style="border:solid 1px red!important;">' + item.LedgerAccountName + '</td > <td style="border:solid 1px red!important;">' + item.CreatedOn + ' </td> <td style="border:solid 1px red!important;">' + item.CreatedByName + ' </td>');
                        }


                    }

                    else {
                        //totalamtpaid += parseFloat(item.PaidAmountDr);

                        if (item.AccountType.toUpperCase() == "PATIENT")
                            totalamtpaid += parseFloat(item.PaidAmountDr);
                        if (item.AccountType.toUpperCase() == "DISCOUNT")
                            totalamtdis += parseFloat(item.PaidAmountDr);

                        var appointmentId = null;
                        if (item.AppointmentId)
                            appointmentId = item.AppointmentId;

                        $row.append('<td style="display:none; border:solid 1px green!important;">' + item.PaymentId + '</td><td style="border:solid 1px green!important;"><input type="checkbox" onclick="schcopayment.EnableDisablePrintBtn()"/><a class="btn  btn-xs" style="margin-bottom:5px;" href="#" onclick="schcopayment.PatientCopaymentRefund(' + item.PaymentId + ',' + appointmentId + ',' + parseFloat(item.PaidAmountDr).toFixed(Number(globalAppdata.DecimalPlaces)) + ');" title="Refund"><i class="fa fa-money"></i></a></td><td style="border:solid 1px green!important;">' + item.PaymentDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '') + '</td><td style="border:solid 1px green!important;">' + globalAppdata.DefaultCurrency + parseFloat(item.PaidAmountDr).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="border:solid 1px green!important;">' + item.PmtTypeName + '</td><td style="border:solid 1px green!important;">' + item.CheckNo + '</td><td style="border:solid 1px green!important;">' + item.LedgerAccountName + '</td > <td style="border:solid 1px green!important;">' + item.CreatedOn + ' </td> <td style="border:solid 1px green!important;">' + item.CreatedByName + ' </td>');
                    }


                    $("#pnlCopayment_Result #dgvCopayment tbody").last().append($row);
                    //if (PatientPaymentsJSONData.length == t) {
                    //    $row = $('<tr/>');
                    //    $row.append('<td style="display:none; border:none!important;"></td><td style="border:none!important;"></td><td style="border:none!important;font-weight:bold">Total Paid:</td><td style="border:none!important;font-weight:bold">$' + parseFloat(totalamtpaid).toFixed(2) + '</td><td style="border:none!important;font-weight:bold">Total Discount:</td><td style="border:none!important;font-weight:bold">$' + parseFloat(totalamtdis).toFixed(2) + '</td><td style="border:none!important;"></td > <td style="border:none!important;"></td> <td style="border:none!important;"></td>');
                    //    $("#pnlCopayment_Result #dgvCopayment tbody").last().append($row);

                    //}


                });


                $("#schcopayment #lblTotalPaid").text(parseFloat(totalamtpaid).toFixed(Number(globalAppdata.DecimalPlaces)));
                $("#schcopayment #lblTotalDiscount").text(parseFloat(totalamtdis).toFixed(Number(globalAppdata.DecimalPlaces)));



            }

        }
        else {
            $('#dgvCopayment').DataTable({
                "language": {
                    "emptyTable": "No Copayment Found"
                }, "autoWidth": false, "bPaginate": false, "bInfo": false, "bLengthChange": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }]
            });
        }
        if ($.fn.dataTable.isDataTable('#pnlCopayment_Result #dgvCopayment'))
            ;
        else
            $("#pnlCopayment_Result #dgvCopayment").DataTable({ "bLengthChange": false, "bPaginate": false, "bInfo": false, "autoWidth": false, "aoColumnDefs": [{ "bSortable": false, "aTargets": [1] }] }); // to remove records per page dropdown

        $("#schcopayment").find('.Of-a').addClass('height-max200');
        schcopayment.EnableDisablePrintBtn();
    },

    PatientCopaymentRefund: function (PaymentId, AppointmentId, PaidAmountDr) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Copayment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Are you sure want to refund?', function () {
                    var selectedValue = PaymentId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        schcopayment.PatCopaymentRefund(PaymentId, AppointmentId, $('#schcopayment #hfAdvancePaymentId').val()).done(function (response) {
                            if (response.status != false) {

                                // Start 30/01/2016 Muhammad Irfan for bug # PMS-3160
                                //if (schcopayment.params.ParentCtrl != 'mstrTabDashBoard')
                                //   schcopayment.ReLoadDashboard();
                                // Start 30/01/2016 Muhammad Irfan for bug # PMS-3160

                                // Check with irfan why you used this.
                                // var patcopaymentDetail = JSON.parse(response.PatientPayments_JSON);
                                var self = $("#schcopayment");
                                var myJSON = self.getMyJSON();
                                var copayBalance = JSON.parse(myJSON).txtBalance;
                                if (copayBalance.indexOf(')') > -1)
                                {
                                    copayBalance = ("-")+ copayBalance.replace(')', "").replace('(', "").trim();
                                }
                                var remaingbal = parseFloat(copayBalance) + parseFloat(PaidAmountDr);
                                schcopayment.SelectPatientPayments(null, AppointmentId, schcopayment.params.PatientVisitId);
                                
                                schcopayment.UpdateAppointmentOnScheduler(AppointmentId, remaingbal, true);
                                utility.DisplayMessages("Refunded Successfully", 1);
                                $('select[id=ddlDiscountLedger] option:eq(1)').prop('selected', 'selected');
                                $('select[id=ddlPaidLedger] option:eq(1)').prop('selected', 'selected');
                                $('select[id=ddlPaymentType] option:eq(0)').prop('selected', 'selected');
                                ////UnloadActionPan(schcopayment.params["ParentCtrl"], "actionPanCopayment");

                                //var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                //Resolved QAC2-417 
                                //if ($('#btnweek').attr('class') == 'btn btn-default btn-sm active') {
                                //    Scheduling_Calendar.WeekClick();
                                //} else if (schcopayment.params.PanelID == "pnlScheduleMuliView") {
                                //    Scheduling_MuliView.LoadMultipleViewCalendar();
                                //} else {
                                //    var statusslots = Scheduling_Calendar.FilterCriteria();
                                //    if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
                                //        Scheduling_Calendar.DayCalendar(schcopayment.params.ProviderId, null, schcopayment.params.FacilityId, schcopayment.params.DayDate, statusslots);
                                //    }
                                //    else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                                //        Scheduling_Calendar.DayCalendar(null, schcopayment.params.ResourceId, schcopayment.params.FacilityId, schcopayment.params.DayDate, statusslots);
                                //    }
                                //    //Scheduling_Calendar.DayCalendar(schcopayment.params.ProviderId, schcopayment.params.ResourceId, schcopayment.params.FacilityId, schcopayment.params.DayDate, statusslots);
                                //}
                                //if (schcopayment.params.ProviderId != "")
                                //    $("#pnlScheduleCalendar #Provider option[value=" + schcopayment.params.ProviderId + "]").attr('selected', 'selected');
                                //if (schcopayment.params.ResourceId != "")
                                //    $("#pnlScheduleCalendar #Resource option[value=" + schcopayment.params.ResourceId + "]").attr('selected', 'selected');
                                //if (schcopayment.params.FacilityId != "")
                                //    $("#pnlScheduleCalendar #Facility option[value=" + schcopayment.params.FacilityId + "]").attr('selected', 'selected');
                                //if (schcopayment.params.DayDate != "")
                                //    $('#pnlScheduleCalendar #daydate span').html(schcopayment.params.DayDate);
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
    SaveAndExit: function (e) {
        schcopayment.bIsExit = true;
      
    },
    PrintPreviewOfCopayment: function (PaymentId) {

        var params = {};
        params["PaymentId"] = PaymentId;
        params["ParentCtrl"] = "schcopayment";
        params["isSaveReceiptDoc"] = false;
        params["VisitId"] = schcopayment.params["PatientVisitId"] ? schcopayment.params["PatientVisitId"] : 0;;
        LoadActionPan('Scheduling_CopaymentView', params);
    },

    ValidatePatientCopayment: function () {
        $('#frmCopayment')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   PaymentType: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DatePaid: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                           date: {
                               format: date_format.toUpperCase(),
                               message: ' '
                           }
                       }
                   },
                   Paid: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PaidAC: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Discount: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DiscountAC: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   //CheckNumber: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //},

                   //CheckDate: {
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

                   CardNumber: {
                       group: '.col-sm-4',
                       validators: {
                           //notEmpty: {
                           //    message: 'The credit card number is required'
                           //},
                           creditCard: {
                               message: 'The credit card number is not valid'
                           }
                       }
                   },

                   ExpiryDate: {
                       group: '.col-sm-4',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   }
               }
           })
       .on('error.form.bv', function (e, data) {
          
           schcopayment.bIsExit = false;
       })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (schcopayment.bIsExit) {
                schcopayment.PatientCopaymentSave(true);
                schcopayment.bIsExit = false;
            }
            else
                schcopayment.PatientCopaymentSave(false);
        });
    },

    PatientCopaymentSave: function (IsUnload) {
        
        $("#schcopayment #txtPaid").trigger('change');
        var strMessage = "";
        var self = $("#schcopayment");
        var myJSON = self.getMyJSON();
        var copayBalance = JSON.parse(myJSON).txtBalance;
       
        AppPrivileges.GetFormPrivileges("Copayment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                schcopayment.SavePatientCopayment(myJSON).done(function (response) {
                    if (response.status != false) {

                        utility.DisplayMessages(response.message, 1);

                        // Start 30/01/2016 Muhammad Irfan for bug # PMS-3160
                        if (schcopayment.params.ParentCtrl != 'mstrTabDashBoard')
                            schcopayment.ReLoadDashboard();
                        // Start 30/01/2016 Muhammad Irfan for bug # PMS-3160


                        var PaymentId = response.PaymentId;

                        var AppointmentId = response.AppointmentId;

                        schcopayment.SelectPatientPayments(null, AppointmentId, schcopayment.params.PatientVisitId);
                        schcopayment.UpdateAppointmentOnScheduler(AppointmentId, copayBalance, false);
                        schcopayment.BindAdvancePayment(schcopayment.params.PatientId);
                        $('#frmCopayment').data('bootstrapValidator').enableFieldValidators('Paid', true);
                        $('#frmCopayment').data('bootstrapValidator').enableFieldValidators('PaidAC', true);
                        $('#frmCopayment').data('bootstrapValidator').enableFieldValidators('DiscountAC', true);
                        $('#frmCopayment').data('bootstrapValidator').enableFieldValidators('Discount', true);
                        utility.ClearFormValidation('#frmCopayment', true);
                        $('select[id=ddlDiscountLedger] option:eq(1)').prop('selected', 'selected');
                        $('select[id=ddlPaidLedger] option:eq(1)').prop('selected', 'selected');
                        $('select[id=ddlPaymentType] option:eq(0)').prop('selected', 'selected');
                        //$('#pnlScheduleCalendar select[id=ddlPaymentType]').trigger('change');
                        //var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                        ////Resolved QAC2-417 
                        //if ($('#btnweek').attr('class') == 'btn btn-default btn-sm active') {
                        //    Scheduling_Calendar.WeekClick();
                        //} else if (schcopayment.params.PanelID == "pnlScheduleMuliView") {
                        //    Scheduling_MuliView.LoadMultipleViewCalendar();
                        //}
                        //else {
                        //    var statusslots = Scheduling_Calendar.FilterCriteria();
                        //    if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
                        //        Scheduling_Calendar.DayCalendar(schcopayment.params.ProviderId, null, schcopayment.params.FacilityId, schcopayment.params.DayDate, statusslots);
                        //    }
                        //    else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                        //        Scheduling_Calendar.DayCalendar(null, schcopayment.params.ResourceId, schcopayment.params.FacilityId, schcopayment.params.DayDate, statusslots);
                        //    }
                        //    //Scheduling_Calendar.DayCalendar(schcopayment.params.ProviderId, schcopayment.params.ResourceId, schcopayment.params.FacilityId, schcopayment.params.DayDate, statusslots);
                        //}
                        ///
                        //if (schcopayment.params.ProviderId != "")
                        //    $("#pnlScheduleCalendar #Provider option[value=" + schcopayment.params.ProviderId + "]").attr('selected', 'selected');
                        //if (schcopayment.params.ResourceId != "")
                        //    $("#pnlScheduleCalendar #Resource option[value=" + schcopayment.params.ResourceId + "]").attr('selected', 'selected');
                        //if (schcopayment.params.FacilityId != "")
                        //    $("#pnlScheduleCalendar #Facility option[value=" + schcopayment.params.FacilityId + "]").attr('selected', 'selected');
                        //if (schcopayment.params.DayDate != "")
                        //    $('#pnlScheduleCalendar #daydate span').html(schcopayment.params.DayDate);
                        ///*Pending for testing
                        // Begin 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3270
                        Patient_Demographic.UpdateBalancesInBanner();
                        // End 18-Jan-2016  Edited By Azeem Raza Tayyab Bug # PMS-3270
                        //*/
                        //MDVisionService.reloadLookups = true;
                        if (IsUnload) {
                           
                            UnloadActionPan(schcopayment.params["ParentCtrl"], "actionPanCopayment");
                        }
                        
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                    }
                });
            }
            else
                utility.DisplayMessages(strMessage, 2);
        });
       
    },

    UpdateAppointmentOnScheduler: function(appointmentId, copayBalance, IsRefund) {

        var dataSourceApp = PMSScheduler.CanScheduler.dataSource;
        //var ap = dataSourceApp._data.filter(f => f.AppointmentId == appointmentId)[0];

        var ap = dataSourceApp._data.filter(function (f) {
            return f.AppointmentId == appointmentId;
        })[0];

        var copay = copayBalance;
        //if (IsRefund == true) {
        //    if (ap.CopayBal < 0)
        //        copay = parseFloat( ap.CopayBal) + copayBalance;
        //    else
        //        copay = parseFloat( ap.CopayBal) - copayBalance

        //}
        ap.CopayBal = copay;

        if (ap.CopayBal == 0 ) {
            ap.CopayClass = "Green";
        }
        if (ap.CopayBal > 0) {
            ap.CopayClass = "Red";
        }
     
        if (ap.CopayBal < 0) {
            ap.CopayClass = "Green";
        }
        if (ap.AmtCopay > 0 && ap.CopayBal == 0) {
            ap.CopayBal = ap.AmtCopay;
        }
        PMSScheduler.CanScheduler.dataSource.pushUpdate(ap);

    },

    SavePatientCopayment: function (PatientCopaymentData) {
        var data = "PatientCopaymentData=" + PatientCopaymentData + "&PatientVisitId=" + schcopayment.params.PatientVisitId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_COPAYMENT", "SAVE_PATIENT_COPAYMENT");
    },

    SelectPatPayments: function (PaymentId, AppointmentID, VisitId) {
        var data = "PaymentId=" + PaymentId + "&AppointmentID=" + AppointmentID + "&VisitId=" + VisitId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_COPAYMENT", "GET_PATIENT_COPAYMENT");
    },

    SelectPatCopaymentDetailIDs: function (AppointmentID, VisitId) {
        var data = "AppointmentID=" + AppointmentID + "&VisitId=" + VisitId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_COPAYMENT", "GET_PATIENT_COPAYMENTBYIDs");
    },

    PatCopaymentRefund: function (PaymentId, AppointmentID, AdvancePaymnetId) {
        var data = "PaymentId=" + PaymentId + "&AppointmentID=" + AppointmentID + '&AdvancePaymnetId=' + AdvancePaymnetId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_COPAYMENT", "REFUND_PATIENT_COPAYMENT");
    },

    //ProviderSearch: function (ProviderId) {

    //    schcopayment.SearchProvider("", ProviderId).done(function (response) {
    //        if (response.status != false) {
    //            if (response.ProviderCount > 0) {
    //                var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);
    //                if (ProviderLoadJSONData[0].IsSpecialist == "False") {
    //                    $('#schcopayment #rdPCP').attr("checked", "checked");
    //                }
    //                else
    //                    $('#schcopayment #rdSpecialist').attr("checked", "checked");
    //            }
    //        }
    //        else {
    //            utility.DisplayMessages(response.Message, 3);
    //        }
    //    });

    //},

    SearchProvider: function (ProviderData, ProviderId) {
        var data = "ProviderData=" + ProviderData + "&ProviderID=" + ProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER", "SEARCH_PROVIDER");
    },


    BindAdvancePayment: function (PatientId) {
        PatientId = parseInt(PatientId);
        var totalBalance = 0;
        var jsonArray = { "hfPatientId": PatientId, "hfFacility": "-1", "dtpPaidFrom": "", "dtpPaidTo": "", "ddlPaymentType": "" };
        Patient_AdvancePayment.AdvancePaymentSearch(JSON.stringify(jsonArray)).done(function (response) {
            if (response.status != false && response.AdvancePaymentLoad_JSON != null) {

                var AdvancePaymentLoad_JSONData = JSON.parse(response.AdvancePaymentLoad_JSON);
                schcopayment.AllAdvancePayments = [];

                $.each(AdvancePaymentLoad_JSONData, function (i, item) {

                    //AllAdvancePayments.push({ Date: item.PaymentDate, value: item.Balance, id: item.AdvPaymentId });
                    totalBalance += parseFloat(item.Balance);

                    if (parseFloat(item.Balance) > 0)
                        schcopayment.AllAdvancePayments.push({ id: item.AdvPaymentId, value: utility.RemoveTimeFromDate(null, item.PaymentDate) + " - " + item.Balance, Balance: item.Balance });


                });

                schcopayment.setAdvanceBalance(totalBalance);
                $("#schcopayment #txtAdvancePayment").autocomplete({
                    autoFocus: true,
                    source: schcopayment.AllAdvancePayments,
                    select: function (event, ui) {


                        setTimeout(function () {
                            $("#schcopayment #hfAdvancePaymentId").val(ui.item.id);
                            //$("#schcopayment #txtPaid").val(ui.item.Balance);
                            schcopayment.setAdvanceBalance(ui.item.Balance);

                        }, 100);

                    }
                });
            }
        });
    },

    setAdvanceBalance: function (AdvanceBalance) {

        $("#schcopayment  #hfAdvanceBalance").val(0);
        $("#schcopayment  #lblAdvanceBalance").text("0.0");

        $("#schcopayment  #hfAdvanceBalance").val(AdvanceBalance);

        if (AdvanceBalance >= 0) {
            $("#schcopayment #lblAdvanceBalance").removeClass('red');
        } else {
            $("#schcopayment #lblAdvanceBalance").addClass('red');
        }
        if (schcopayment.isFloat(AdvanceBalance))
        {
            AdvanceBalance = AdvanceBalance.toFixed(2);
        }
        else
        {
            AdvanceBalance = AdvanceBalance+".00";
        }

        $("#schcopayment #lblAdvanceBalance").text( AdvanceBalance);


    },
     isFloat:function(n){
return Number(n) === n && n % 1 !== 0;
},
    OpenAdvancePayment: function () {
        var params = [];

        params["patientID"] = schcopayment.params.PatientId;

        //if (schcopayment.params. .params.TabID == 'paymentBatchDetail')
        params["ParentCtrl"] = 'schcopayment';
        //else
        //    params["ParentCtrl"] = Bill_PaymentPosting.params["TabID"];
        LoadActionPan('Patient_AdvancePayment', params);
    },

    FillAdvancePayment: function (AdvancePaymentId, datePaid, Balance, event) {
        if (event != null) {
            event.stopPropagation();
        }
        /*
                var RefCtrl = " #txtAdvancePayment";
                var PaidCtrl = " #txtPatientPaid";
                var balanceCtrl = " #hfAdvanceBalance";
                var RefHiddenIdCtrl = " #hfAdvancePaymentId";
                if (schcopayment.params["RefCtrl"] != null) {
                    RefCtrl = " #" + schcopayment.params["RefCtrl"];
                }
                if (schcopayment.params["RefHiddenIdCtrl"] != null) {
                    RefHiddenIdCtrl = " #" + schcopayment.params["RefHiddenIdCtrl"];
                }*/
        $("#schcopayment #txtAdvancePayment").val(datePaid + " - " + Balance);
        $("#schcopayment #hfAdvancePaymentId").val(AdvancePaymentId);
        $("#schcopayment #lblAdvanceBalance").text(Balance);

        /* 
         //$('#' + schcopayment.params["PanelID"] + PaidCtrl).val(Balance);
         
         // $('#' + Patient_AdvancePayment.params["PanelID"] + ' #lblAdvancePayment').css("display", "none");
         // $('#' + Patient_AdvancePayment.params["PanelID"] + ' #lnkAdvancePaymentEdit').css("display", "inline");
         
         */


        UnloadActionPan("schcopayment");
        // $('#' + schcopayment.params["PanelID"] + RefCtrl).focus();
    },

    UnLoad: function () {
        if (schcopayment.params.PanelID == "schcheckin") {
            schcopayment.params.ParentCtrl = "schcheckin"
        }
        utility.UnLoadDialog('frmCopayment', function () {
            UnloadActionPan(schcopayment.params["ParentCtrl"], "actionPanCopayment");
        }, function () {
            UnloadActionPan(schcopayment.params["ParentCtrl"], "actionPanCopayment");
        });

    },

    ReLoadDashboard: function () {
        //DashBoard.bIsFirstLoad = true;
        //$('#pnlDashboard .slide-ul').unslick();
    },
    GetALLCheckedRecipts: function () {
        var GetALLPayments = [];
        $($('#' + schcopayment.params["PanelID"] + ' #pnlCopayment_Result #dgvCopayment input:checked')).each(function (k, v) {
            GetALLPayments.push(parseInt($(this).parent().prev().html()));
        });
        schcopayment.PrintPreviewOfCopayment(GetALLPayments);
    },
    EnableDisablePrintBtn: function () {
        if ($('#' + schcopayment.params["PanelID"] + ' #pnlCopayment_Result #dgvCopayment input:checked').length > 0) {
            $('#' + schcopayment.params["PanelID"] + ' #btnPrint').removeAttr("disabled", "disabled");
        }
        else {
            $('#' + schcopayment.params["PanelID"] + ' #btnPrint').attr("disabled", "disabled");
        }

    }
}