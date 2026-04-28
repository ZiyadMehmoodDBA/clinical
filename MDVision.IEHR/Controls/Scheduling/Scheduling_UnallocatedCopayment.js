Scheduling_UnallocatedCopayment = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        Scheduling_UnallocatedCopayment.params = params;

        if (Scheduling_UnallocatedCopayment.bIsFirstLoad) {
            Scheduling_UnallocatedCopayment.bIsFirstLoad = false;
        }
        //initialization of date-pickers.
        Scheduling_UnallocatedCopayment.InitializeControl();
        var self = $('#unallocatedCopaymentPanel');

        self.loadDropDowns(true).done(function () {

            $('#pnlUnallocatedCopayment #hfAppointmentId').val(Scheduling_UnallocatedCopayment.params.AppointmentId);
            $('#pnlUnallocatedCopayment #hfPatientId').val(Scheduling_UnallocatedCopayment.params.PatientId);
            $('#pnlUnallocatedCopayment #hfProviderId').val(Scheduling_UnallocatedCopayment.params.ProviderId);
            $('#pnlUnallocatedCopayment #hfFacilityId').val(Scheduling_UnallocatedCopayment.params.FacilityId);
            $('select[id=ddlPaymentType] option:eq(0)').prop('selected', 'selected');
            Scheduling_UnallocatedCopayment.FillPaidAC().done(function () {

                Scheduling_UnallocatedCopayment.BindAdvancePayment(Scheduling_UnallocatedCopayment.params.PatientId);
                if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked") || (Scheduling_UnallocatedCopayment.params["ParentCtrl"] == "schTabMultipleView" && Scheduling_UnallocatedCopayment.params["ResourceId"])) {
                    schcheckin.FillPatientData(Scheduling_UnallocatedCopayment.params.PatientId, Scheduling_UnallocatedCopayment.params.AppointmentId).done(function (response) {
                        if (response.status != false) {
                            var patientappointments = JSON.parse(response.PatientAppointment_JSON);
                            $('#frmUnallocatedCopayment #hfProviderId').val(patientappointments[0].ProviderId);
                        }
                        Scheduling_UnallocatedCopayment.LoadPatientCopayment();
                    });
                } else {
                    Scheduling_UnallocatedCopayment.LoadPatientCopayment();
                }
                Scheduling_UnallocatedCopayment.SelectPatientPayments(Scheduling_UnallocatedCopayment.params.AppointmentId);
            });
        });
    },

    InitializeControl: function () {

        //initialization of date-pickers.
        utility.creditCardExpiryDate('frmUnallocatedCopayment #dpExpiryDate', function () {

            // on-change callback method 
            $('#frmUnallocatedCopayment').bootstrapValidator('revalidateField', 'ExpiryDate');
        });
    },

    FillPaidAC: function () {
        var dfd = new $.Deferred();
        CacheManager.BindDropDownsByTwoIDs('#pnlUnallocatedCopayment #ddlPaidLedger', 'GetLedgerAccount', true, 1, 3).done(function () {

            $('select[id=ddlPaidLedger] option:eq(1)').prop('selected', 'selected');
            dfd.resolve(true);
        });

        return dfd.promise();
    },

    BindAdvancePayment: function (PatientId) {
        PatientId = parseInt(PatientId);
        var totalBalance = 0;
        var jsonArray = { "hfPatientId": PatientId, "hfFacility": "-1", "dtpPaidFrom": "", "dtpPaidTo": "", "ddlPaymentType": "" };
        Patient_AdvancePayment.AdvancePaymentSearch(JSON.stringify(jsonArray)).done(function (response) {
            if (response.status != false && response.AdvancePaymentLoad_JSON != null) {

                var AdvancePaymentLoad_JSONData = JSON.parse(response.AdvancePaymentLoad_JSON);
                Scheduling_UnallocatedCopayment.AllAdvancePayments = [];

                $.each(AdvancePaymentLoad_JSONData, function (i, item) {
                    if (parseFloat(item.Balance) > 0)
                        Scheduling_UnallocatedCopayment.AllAdvancePayments.push({ id: item.AdvPaymentId, value: utility.RemoveTimeFromDate(null, item.PaymentDate) + " - " + item.Balance, Balance: item.Balance });
                });

                $("#pnlUnallocatedCopayment #txtAdvancePayment").autocomplete({
                    autoFocus: true,
                    source: Scheduling_UnallocatedCopayment.AllAdvancePayments,
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#pnlUnallocatedCopayment #hfAdvancePaymentId").val(ui.item.id);

                        }, 100);
                    }
                });
            }
        });
    },

    LoadPatientCopayment: function () {
        Scheduling_UnallocatedCopayment.ValidatePatientCopayment();
    },

    ValidatePatientCopayment: function () {
        $('#frmUnallocatedCopayment')
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
                   ReceiptDate: {
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
        .on('success.form.bv', function (e) {
            e.preventDefault();
            if (e.type == "success") {
                var $form = $(e.target);
                var $button = $form.data('bootstrapValidator').getSubmitButton();
                switch ($button.attr('id')) {
                    case 'btnSave':
                        Scheduling_UnallocatedCopayment.PatientCopaymentSave();
                        break;
                    case 'btnSavePrint':
                        Scheduling_UnallocatedCopayment.PatientCopaymentSave(true);
                        break;

                    default:
                        Scheduling_UnallocatedCopayment.PatientCopaymentSave();
                        break;
                }
            }
            e.type = "";
        });
    },

    PatientCopaymentSave: function (isPrint) {
        var strMessage = "";
        var self = $("#pnlUnallocatedCopayment");
        var myJSON = self.getMyJSON();
        AppPrivileges.GetFormPrivileges("Copay Receipt", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {

                Scheduling_UnallocatedCopayment.SavePatientCopayment(myJSON).done(function (response) {
                    if (response.status != false) {

                        utility.DisplayMessages(response.Message, 1);
                        $('#pnlUnallocatedCopayment #txtReceiptNumber').val(response.ReceiptNumber);

                        Scheduling_UnallocatedCopayment.SelectPatientPayments(Scheduling_UnallocatedCopayment.params.AppointmentId);

                        if (isPrint) {
                            Scheduling_UnallocatedCopayment.PrintPreviewOfCopayment(response.UnAllocatedCopayId, isPrint);
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

    SavePatientCopayment: function (PatientCopaymentData) {
        var data = "PatientCopaymentData=" + PatientCopaymentData;
        return MDVisionService.defaultService(data, "SCHEDULING_UNALLOCATEDCOPAYMENT", "SAVE_UNALLOCATED_COPAYMENT");
    },

    SelectPatPayments: function (AppointmentID) {
        var data = "AppointmentID=" + AppointmentID;
        return MDVisionService.defaultService(data, "SCHEDULING_UNALLOCATEDCOPAYMENT", "GET_UNALLOCATED_COPAYMENT_SCHEDULING");
    },

    SelectPatientPayments: function (AppointmentID) {
        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Copay Receipt", "VIEW", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if ($("#pnlUnallocatedCopayment #pnlCopayment_Result").css("display") == "none") {
                    $("#pnlUnallocatedCopayment #pnlCopayment_Result").show();
                }

                Scheduling_UnallocatedCopayment.SelectPatPayments(AppointmentID).done(function (response) {
                    if (response.status != false) {
                        if (response.UnAllocatedCopayListRecordCount > 0) {
                            var patcopayment = response.UnAllocatedCopayInfo_JSON;
                            var CopayAmount = 0;
                            var TotalUnallocated = 0;
                            var isDeleted = 0;
                            $.each(patcopayment, function (i, item) {
                                if (patcopayment[i].CopayAmount && patcopayment[i].IsDeleted == 'False') {
                                    isDeleted = 1;
                                    CopayAmount = parseFloat(patcopayment[i].CopayAmount);
                                    if (patcopayment[i].Status == 'Unallocated') {
                                        TotalUnallocated = parseFloat(patcopayment[i].CopayAmount);
                                    }
                                }
                            });
                            $("#pnlUnallocatedCopayment #txtComments").val("");
                            if (isDeleted == 1) {
                                $("#pnlUnallocatedCopayment #lblTotalPaid").text(CopayAmount.toFixed(Number(globalAppdata.DecimalPlaces)));
                                $("#pnlUnallocatedCopayment #lblTotalUnallocatedAmount").text(TotalUnallocated.toFixed(Number(globalAppdata.DecimalPlaces)));
                                $("#pnlUnallocatedCopayment #btnsave").addClass('disableAll');
                                $("#pnlUnallocatedCopayment #btnSavePrint").addClass('disableAll');
                                $('#frmUnallocatedCopayment').data('bootstrapValidator').enableFieldValidators('PaidAC', true);
                            }
                            else {
                                $("#pnlUnallocatedCopayment #lblTotalPaid").text(CopayAmount.toFixed(Number(globalAppdata.DecimalPlaces)));
                                $("#pnlUnallocatedCopayment #lblTotalUnallocatedAmount").text(TotalUnallocated.toFixed(Number(globalAppdata.DecimalPlaces)));
                                $("#pnlUnallocatedCopayment #txtReceiptNumber").val("");
                                $("#pnlUnallocatedCopayment #txtPaid").focus();
                                $("#pnlUnallocatedCopayment #btnsave").removeClass('disableAll');
                                $("#pnlUnallocatedCopayment #btnSavePrint").removeClass('disableAll');
                                $('#frmUnallocatedCopayment').data('bootstrapValidator').enableFieldValidators('PaidAC', true);
                            }
                        }
                        else {
                            $("#pnlUnallocatedCopayment #txtPaid").focus();
                        }
                        Scheduling_UnallocatedCopayment.PatientCopaymentGridLoad(response);
                    }
                    else {
                        utility.DisplayMessages(response.Message, 3);
                        Scheduling_UnallocatedCopayment.PatientCopaymentGridLoad(response);
                    }
                });
                $('#pnlUnallocatedCopayment #dpDatePaid').val($.datepicker.formatDate('mm/dd/yy', new Date()));
            }
            else
                utility.DisplayMessages(strMessage, 2);

        });
    },

    PatientCopaymentGridLoad: function (response) {
        $("#dgvCopayment").dataTable().fnDestroy();
        $("#pnlCopayment_Result #dgvCopayment tbody").find("tr").remove();
        if (response.UnAllocatedCopayListRecordCount > 0) {
            var totalamtpaid = 0;
            var totalamtdis = 0;

            var PatientPaymentsJSONData = response.UnAllocatedCopayInfo_JSON;
            if (PatientPaymentsJSONData[0].AccountNumber != "") {

                $.each(PatientPaymentsJSONData, function (i, item) {

                    var $row = $('<tr/>');
                    $row.attr("onclick", "utility.SelectGridRow($('#dgvCopayment_row" + item.AppointmentId + "'))");
                    $row.attr("id", "dgvCopayment_row" + item.UnAllocatedCopayId);
                    $row.attr("UnAllocatedCopayId", item.UnAllocatedCopayId);

                    if (PatientPaymentsJSONData[i].IsDeleted == "True") {
                        $row.append('<td style="display:none; border:solid 1px green!important;">' + item.UnAllocatedCopayId + '</td><td style="border:solid 1px green!important;"></td><td style="border:solid 1px green!important;">' + (item.ReceiptNumber == null ? "" : item.ReceiptNumber) + '</td><td style="border:solid 1px green!important;">' + (item.ReceiptDate == null ? "" : item.ReceiptDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')) + '</td><td style="border:solid 1px green!important;">' + globalAppdata.DefaultCurrency + parseFloat(item.CopayAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="border:solid 1px green!important;">' + (item.PaymentType == null ? "" : item.PaymentType) + '</td><td style="border:solid 1px green!important;">' + (item.CheckNo == null ? "" : item.CheckNo) + '</td><td style="border:solid 1px green!important;">' + (item.PaidAccountType == null ? "" : item.PaidAccountType) + '</td > <td style="border:solid 1px green!important;">' + (item.CreatedOn == null ? "" : item.CreatedOn) + ' </td> <td style="border:solid 1px green!important;">' + (item.CreatedByName == null ? "" : item.CreatedByName) + ' </td><td style="border:solid 1px green!important;">' + (item.Status == null ? "" : item.Status) + ' </td>');
                        $("#pnlCopayment_Result #dgvCopayment tbody").last().append($row);

                        $row = $('<tr/>');
                        $row.attr("onclick", "utility.SelectGridRow($('#dgvCopayment_row" + item.AppointmentId + "'))");
                        $row.attr("id", "dgvCopayment_row" + item.UnAllocatedCopayId);
                        $row.attr("UnAllocatedCopayId", item.UnAllocatedCopayId);
                        $row.append('<td style="display:none; border:solid 1px red!important;">' + item.UnAllocatedCopayId + '</td><td style="border:solid 1px red!important;"></td><td style="border:solid 1px red!important;">' + (item.ReceiptNumber == null ? "" : item.ReceiptNumber) + '</td><td style="border:solid 1px red!important;">' + (item.ReceiptDate == null ? "" : item.ReceiptDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')) + '</td><td style="border:solid 1px red!important; color:red;font-weight:bold">(' + globalAppdata.DefaultCurrency + parseFloat(item.CopayAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + ')</td><td style="border:solid 1px red!important;">' + (item.PaymentType == null ? "" : item.PaymentType) + '</td><td style="border:solid 1px red!important;">' + (item.CheckNo == null ? "" : item.CheckNo) + '</td><td style="border:solid 1px red!important;">' + (item.PaidAccountType == null ? "" : item.PaidAccountType) + '</td > <td style="border:solid 1px red!important;">' + (item.CreatedOn == null ? "" : item.CreatedOn) + ' </td> <td style="border:solid 1px red!important;">' + (item.CreatedByName == null ? "" : item.CreatedByName) + ' </td><td style="border:solid 1px red!important;">' + "N/A" + ' </td>');

                    }

                    else {
                        var appointmentId = null;
                        if (item.AppointmentId)
                            appointmentId = item.AppointmentId;

                        $row.append('<td style="display:none; border:solid 1px green!important;">' + item.UnAllocatedCopayId + '</td><td style="border:solid 1px green!important;"><a class="btn  btn-xs' + (item.Status == "Allocated" ? " disableAll" : "") + '" href="#" onclick="Scheduling_UnallocatedCopayment.PatientCopaymentRefund(' + item.UnAllocatedCopayId + ',' + appointmentId + ');" title="Refund"><i class="fa fa-money"></i></a><a class="btn btn-xs" href="#" onclick="Scheduling_UnallocatedCopayment.PrintPreviewOfCopayment(' + item.UnAllocatedCopayId + ',false);" title="Print Record"><i class="fa fa-print"></i></a></td><td style="border:solid 1px green!important;">' + (item.ReceiptNumber == null ? "" : item.ReceiptNumber) + '</td><td style="border:solid 1px green!important;">' + (item.ReceiptDate == null ? "" : item.ReceiptDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, '')) + '</td><td style="border:solid 1px green!important;">' + globalAppdata.DefaultCurrency + parseFloat(item.CopayAmount).toFixed(Number(globalAppdata.DecimalPlaces)) + '</td><td style="border:solid 1px green!important;">' + (item.PaymentType == null ? "" : item.PaymentType) + '</td><td style="border:solid 1px green!important;">' + (item.CheckNo == null ? "" : item.CheckNo) + '</td><td style="border:solid 1px green!important;">' + (item.PaidAccountType == null ? "" : item.PaidAccountType) + '</td > <td style="border:solid 1px green!important;">' + (item.CreatedOn == null ? "" : item.CreatedOn) + ' </td> <td style="border:solid 1px green!important;">' + (item.CreatedByName == null ? "" : item.CreatedByName) + ' </td><td style="border:solid 1px green!important;">' + (item.Status == null ? "" : item.Status) + ' </td>');
                    }

                    $("#pnlCopayment_Result #dgvCopayment tbody").last().append($row);
                });
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

        $("#pnlUnallocatedCopayment").find('.Of-a').addClass('height-max200');
    },

    OpenAdvancePayment: function () {
        var params = [];
        params["patientID"] = Scheduling_UnallocatedCopayment.params.PatientId;
        params["ParentCtrl"] = 'Scheduling_UnallocatedCopayment';
        LoadActionPan('Patient_AdvancePayment', params);
    },

    PatientCopaymentRefund: function (UnAllocatedCopayId, AppointmentId) {

        var strMessage = "";
        AppPrivileges.GetFormPrivileges("Copay Receipt", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                utility.myConfirm('Are you sure want to refund?', function () {
                    var selectedValue = UnAllocatedCopayId;
                    if (selectedValue == "" || selectedValue == "undefined") {
                    }
                    else {
                        Scheduling_UnallocatedCopayment.PatCopaymentRefund(UnAllocatedCopayId).done(function (response) {
                            if (response.status != false) {
                                Scheduling_UnallocatedCopayment.SelectPatientPayments(AppointmentId);
                                utility.DisplayMessages("Refunded Successfully", 1);
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

    PatCopaymentRefund: function (UnAllocatedCopayId) {
        var data = "UnAllocatedCopayId=" + UnAllocatedCopayId;
        return MDVisionService.defaultService(data, "SCHEDULING_UNALLOCATEDCOPAYMENT", "REFUND_UNALLOCATED_COPAYMENT");
    },

    PrintPreviewOfCopayment: function (CopayReceiptID, isSaveReceiptDoc) {

        if (isSaveReceiptDoc == false)
            isSaveReceiptDoc = false;
        else
            isSaveReceiptDoc = true;

        var params = {};
        params["UnallocatedCopaymentId"] = CopayReceiptID;
        params["ParentCtrl"] = "Scheduling_UnallocatedCopayment";
        params["isSaveReceiptDoc"] = isSaveReceiptDoc;
        params["VisitId"] = Scheduling_UnallocatedCopayment.params["VisitId"] ? Scheduling_UnallocatedCopayment.params["VisitId"] : 0;;
        LoadActionPan('Unallocated_CopaymentView', params);
    },

    FillAdvancePayment: function (AdvancePaymentId, datePaid, Balance, event) {
        if (event != null) {
            event.stopPropagation();
        }
        $("#pnlUnallocatedCopayment #txtAdvancePayment").val(datePaid + " - " + Balance);
        $("#pnlUnallocatedCopayment #hfAdvancePaymentId").val(AdvancePaymentId);
        UnloadActionPan("Scheduling_UnallocatedCopayment");
    },

    UnLoad: function () {
        if (Scheduling_UnallocatedCopayment.params.PanelID == "schcheckin") {
            Scheduling_UnallocatedCopayment.params.ParentCtrl = "schcheckin"
        }
        UnloadActionPan(Scheduling_UnallocatedCopayment.params["ParentCtrl"], "actionPanUnallocatedCopayment");
    },

    SaveCopayReceiptInFolder: function (VisitId, dtoS, FileName, PatientID, pdfFile) {

        var data = new FormData();
        data.append("pdfFile", pdfFile);
        data.append("VisitId", VisitId);
        data.append("PatientID", PatientID);
        data.append("FileName", FileName);
        data.append("dtoS", dtoS);
        return MDVisionService.fileService(data, "SCHEDULING_UNALLOCATEDCOPAYMENT", "SAVE_PATIENT_UNALLOCATED_COPAY_RECEIPT_DOCUMENT");
    },
}