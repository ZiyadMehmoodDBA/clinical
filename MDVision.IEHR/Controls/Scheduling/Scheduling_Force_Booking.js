Scheduling_Force_Booking = {
    bIsFirstLoad: true,
    params: [],
    PatientInsuranceId: '',
    LastSctBaseSearch: '',
    ReferralId: '',
    Load: function (params) {


        BackgroundLoaderShow(true);

        Scheduling_Force_Booking.params = params;

        if (Scheduling_Force_Booking.bIsFirstLoad) {
            Scheduling_Force_Booking.bIsFirstLoad = false;
            Scheduling_Force_Booking.params.PanelID = "PnlSchedulingForceBooking";
        }
        var self = $('#PnlSchedulingForceBooking');
        self.loadDropDowns(true).done(function () {
            // Scheduling_Force_Booking.VisitTypeDropdownLoad();
            Scheduling_Force_Booking.LoadAppointment();
            Scheduling_Force_Booking.ValidateAppointment();
        });
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtProvider').attr("disabled", true);
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking  #lnkProviderEdit').attr("disabled", true);
        utility.CreateDatePicker('PnlSchedulingForceBooking #frmSchedulingForceBooking #txtScheduleDate', function (ev) {
            if ($('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data("bootstrapValidator") != null) {
                $('#pnlRescheduleAppointment #frmRescheduleAppointment').bootstrapValidator('revalidateField', 'ScheduleDate');
            }
        }, true);
        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtProvider").val(Scheduling_Force_Booking.params["ProviderName"]);
        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfProviderId").val(Scheduling_Force_Booking.params["ProviderId"]);
        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFacility").val(Scheduling_Force_Booking.params["FacilityName"]);
        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfFacilityId").val(Scheduling_Force_Booking.params["FacilityId"]);

        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtScheduleDate").datepicker("setDate", new Date(Scheduling_Force_Booking.params["DayDate"]));

    },
    changeICDField: function () {

        if ($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #radioFreetext").prop("checked") == true) {
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtComplaintsICD").addClass('hidden');
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #btnSearchCPT").addClass('hidden');
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReasonComments").removeClass('hidden');
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReasonComments").val('');
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtComplaintsICD").val('');
        } else {
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtComplaintsICD").removeClass('hidden');
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #btnSearchCPT").removeClass('hidden');
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReasonComments").addClass('hidden');
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReasonComments").val('');
            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtComplaintsICD").val('');
        }
    },
    OpenSearchPopup: function (ctrl, HiddenCtrl) {
        var params = [];
        params["FromAdmin"] = "0";
        params["RefCtrl"] = ctrl;
        params['fromIcon'] = true;
        params["RefHiddenCtrl"] = HiddenCtrl;
        params["ParentCtrl"] = "Scheduling_Force_Booking";
        LoadActionPan('Admin_IMOICD', params);
    },
    bindICD9AutoComplete: function (element) {
        var ctrl = $(element);
        if ($(element).val().length > 3)
            if ($(element).val().substring(0, 3).toLowerCase() == "sct")
                Scheduling_Force_Booking.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            else
                Scheduling_Force_Booking.LastSctBaseSearch = "";
        else
            Scheduling_Force_Booking.LastSctBaseSearch = "";
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', ctrl, null, true, -1, 'ICD', true, 'Scheduling_Force_Booking', null, false);
    },
    LoadSlotDetail: function () {

        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtProvider').val(Scheduling_Force_Booking.params.ProviderName);
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFacility').val(Scheduling_Force_Booking.params.FacilityName);
        if (Scheduling_Force_Booking.params.ResourceName != 'null')
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtResource').val(Scheduling_Force_Booking.params.ResourceName);
        if (Scheduling_Force_Booking.params.ScheduleReason != 'undefined') {
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReasonComments').val(Scheduling_Force_Booking.params.ScheduleReason);
        }

    },

    LoadAllAutocomplete: function () {

        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
        //    $("input#txtRefProvider").autocomplete({
        //        autoFocus: true,
        //        minLength: 3,  //added min length, Abdur Rehman Latif - PMS 2986
        //        source: RefProviders, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $("#Scheduling_Force_Booking #hfRefProvider").val(ui.item.id);
        //                if ($("#Scheduling_Force_Booking #lnkRefProviderEdit").css("display") == "none") {
        //                    $("#Scheduling_Force_Booking #lnkRefProviderEdit").css("display", "inline");
        //                    $("#Scheduling_Force_Booking #lblRefProvider").css("display", "none");
        //                }
        //            }, 100);
        //        }
        //    });
        //});
        //CacheManager.BindCodes('GetProvider', false).done(function (result) {
        //    $("#Scheduling_Force_Booking #frmScheduling_Force_Booking input#txtProvider").autocomplete({
        //        autoFocus: true,
        //        source: Providers, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                //if ($("#frmScheduling_Force_Booking #lnkProviderEdit").css("display") == "none") {
        //                //    $("#frmScheduling_Force_Booking #lnkProviderEdit").css("display", "");
        //                //    $("#frmScheduling_Force_Booking #lblProvider").css("display", "none");
        //                //}
        //                $("#frmScheduling_Force_Booking #txtProvider").attr("ProviderId", ui.item.id); // add the selected id
        //                $("#frmScheduling_Force_Booking #hfProviderId").val(ui.item.id);
        //                if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {
        //                    Scheduling_Force_Booking.OpenPatReferralSearch($('#Scheduling_Force_Booking #ddlInsurancePlan').val());
        //                }
        //            }, 100);
        //        }
        //    });
        //});

    },

    LoadAppointment: function () {
        BackgroundLoaderShow(true);
        if (Scheduling_Force_Booking.params.mode == "Add") {


            if (Scheduling_Force_Booking.params.ParentCtrl == "schwaitlistdetail") {

                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtAccountNo').attr("disabled", "disabled");
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName').attr("disabled", "disabled");
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkPatientAccount').attr("disabled", "disabled");
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkPatientName').attr("disabled", "disabled");

                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfWaitListId').val(Scheduling_Force_Booking.params.WaitListId);

                $('.recpatt').addClass('disableAll');
                Scheduling_Force_Booking.ProviderSearch(Scheduling_Force_Booking.params.ProviderId);
                Scheduling_Force_Booking.FillPatientAccount(Scheduling_Force_Booking.params.PatientId);
            }


            Scheduling_Force_Booking.ProviderSearch(Scheduling_Force_Booking.params.ProviderId);
            var AccountNo = $("#PatientProfile #hfAccountNo").val();

            if (Scheduling_Force_Booking.params.PatientId) {
                Scheduling_Force_Booking.FillPatientAccount(Scheduling_Force_Booking.params.PatientId);
            }
            else {
                if (AccountNo.length > 0 && Scheduling_Force_Booking.params.ParentCtrl != 'schwaitlistdetail') {
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtAccountNo').val(AccountNo);
                    Scheduling_Force_Booking.FillPatientAccount($("#PatientProfile #hfPatientId").val());
                }
            }

            $('#PnlSchedulingForceBooking  #headingTitle').html('Add Appointment');
            Scheduling_Force_Booking.FillDuration('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfSchReasonId', 0);
            Scheduling_Force_Booking.LoadSlotDetail();

            //$("#Scheduling_Force_Booking #ddlStatus option:selected").text('Schedule');

            //serialize Data.
            $('#Scheduling_Force_Booking #frmScheduling_Force_Booking').data('serialize', $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').serialize());

            $('#Scheduling_Force_Booking #btnCopayment').attr("disabled", true);


            //   $('#Scheduling_Force_Booking #txtScheduleDate').val(Scheduling_Force_Booking.params.ScheduleDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, ''));
        }
        else if (Scheduling_Force_Booking.params.mode == "Edit") {

            $('#Scheduling_Force_Booking #txtAccountNo').attr("disabled", "disabled");
            $('#Scheduling_Force_Booking #txtFullName').attr("disabled", "disabled");
            $('#Scheduling_Force_Booking #lnkPatientAccount').attr("disabled", "disabled");
            $('#Scheduling_Force_Booking #lnkPatientName').attr("disabled", "disabled");
            if (Scheduling_Force_Booking.params.ParentCtrl == "schwaitlistdetail") {

                $('.recpatt').addClass('disableAll');
            }
            // BackgroundLoaderShow(true);
            Scheduling_Force_Booking.FillAppointment(Scheduling_Force_Booking.params.AppointmentId).done(function (response) {
                if (response.status != false) {

                    //$('#Scheduling_Force_Booking #headingTitle').html('Edit Appointment');
                    var appointment_detail = JSON.parse(response.AppointmentFill_JSON);



                    if (appointment_detail.txtFromTime == "") {
                        $('#Scheduling_Force_Booking #totimefrmtime').css("display", "none");
                    }

                    var appInsuranceId = appointment_detail.ddlInsurancePlan;
                    $('#Scheduling_Force_Booking #hfSlottimeid').val(Scheduling_Force_Booking.params.SlotId);
                    $('#Scheduling_Force_Booking #hfSlottimedtlid').val(Scheduling_Force_Booking.params.SlotdetailId);

                    $('#Scheduling_Force_Booking #hfProviderId').val(Scheduling_Force_Booking.params.ProviderId);
                    $('#Scheduling_Force_Booking #hfProviderName').val(Scheduling_Force_Booking.params.ProviderName);
                    $('#Scheduling_Force_Booking #hfFacilityId').val(Scheduling_Force_Booking.params.FacilityId);
                    $('#Scheduling_Force_Booking #hfFacilityName').val(Scheduling_Force_Booking.params.FacilityName);
                    $('#Scheduling_Force_Booking #hfResourceId').val(Scheduling_Force_Booking.params.ResourceId);
                    $('#Scheduling_Force_Booking #hfResourceName').val(Scheduling_Force_Booking.params.ResourceName);
                    $('#Scheduling_Force_Booking #hfSchReasonId').val(Scheduling_Force_Booking.params.ScheduleReasonId);
                    $('#Scheduling_Force_Booking #hfReferralId').val(appointment_detail.ReferralId);
                    //$('#Scheduling_Force_Booking #hfSlotDuration').val(appointment_detail.Duration);



                    $('#Scheduling_Force_Booking #btnCopayment').attr("disabled", false);
                    //$('#Scheduling_Force_Booking #btnCopayment').show();
                    //$("#Scheduling_Force_Booking #btnCopayment").toggleClass("show");

                    if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

                        $("#Scheduling_Force_Booking #btnCopayment").attr("disabled", true);
                    }

                    if (Scheduling_Force_Booking.params.checkin == 1) {
                        //$('#Scheduling_Force_Booking #btnsave').hide();
                        //$("#Scheduling_Force_Booking #btnsave").toggleClass("hide");
                        //$('#Scheduling_Force_Booking #btnsave').attr("disabled", true);
                        //$("#appointmentpanel :input").attr("disabled", "disabled");
                    }
                    if (Scheduling_Force_Booking.params.checkin == 2) {

                        //$('#Scheduling_Force_Booking #btnsave').hide();
                        //$('#Scheduling_Force_Booking #headingTitle').html('View Appointment');
                        //$("#Scheduling_Force_Booking #btnsave").toggleClass("hide");
                        $('#Scheduling_Force_Booking #btnsave').attr("disabled", true);
                        $("#appointmentpanel :input").attr("disabled", "disabled");
                    }
                    if (appointment_detail.rdSpecialist == 'True')
                        $('#Scheduling_Force_Booking #rdSpecialist').attr("checked", "checked");
                    else
                        $('#Scheduling_Force_Booking #rdPCP').attr("checked", "checked");

                    if (appointment_detail.ddlPatientType != "") {
                        $("#Scheduling_Force_Booking #ddlPatientType option[value=" + appointment_detail.ddlPatientType + "]").attr('selected', 'selected').trigger('change');;
                        //Scheduling_Force_Booking.LoadVisitType(appointment_detail.ddlPatientType);
                    }
                    if (appointment_detail.ddlVisitType != "") {
                        $("#Scheduling_Force_Booking #ddlVisitType option[value=" + appointment_detail.ddlVisitType + "]").attr('selected', 'selected');
                    }


                    Scheduling_Force_Booking.FillPatientAccount(appointment_detail.hfpatientid, appInsuranceId).done(function (results) {
                        if (Scheduling_Force_Booking.params.checkin == 2 || Scheduling_Force_Booking.params.checkin == 1) {

                            if (appointment_detail.AppointmentStatus.toUpperCase() == "CHECK OUT") {
                                $("#appointmentpanel :input").attr("disabled", "disabled");
                                $("#appointmentpanel #txtReferralNo").attr("disabled", "disabled");
                            } else if (appointment_detail.AppointmentStatus.toUpperCase() == "CHECK IN" && Scheduling_Force_Booking.params.isNoteCreated == false) {
                                $("#appointmentpanel :input:not(#txtComments,#btnsave,#ddlPatientType,#ddlVisitType,#txtReasonComments)").attr("disabled", "disabled");
                                $("#appointmentpanel #CoPayment").attr("disabled", "disabled");
                                $("#appointmentpanel #txtReferralNo").attr("disabled", "disabled");
                            } else if (appointment_detail.AppointmentStatus.toUpperCase() == "CHECK IN" && Scheduling_Force_Booking.params.isNoteCreated == true) {
                                $("#appointmentpanel :input:not(#txtComments,#btnsave)").attr("disabled", "disabled");
                                $("#appointmentpanel #CoPayment").attr("disabled", "disabled");
                                $("#appointmentpanel #txtReferralNo").attr("disabled", "disabled");
                            }

                            $("#Scheduling_Force_Booking #ddlStatus option").filter(function () {
                                return $(this).text().trim().toUpperCase() == 'CHECK IN';
                            }).prop('disabled', false);

                            $("#Scheduling_Force_Booking #ddlStatus option").filter(function () {
                                return $(this).text().trim().toUpperCase() == 'CHECK OUT';
                            }).prop('disabled', false);
                        }


                        var self = $("#Scheduling_Force_Booking");
                        utility.bindMyJSON(true, appointment_detail, false, self).done(function () {
                            if (appointment_detail.ReasonCommentType == "ICD") {
                                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #radioProblem").prop("checked", true);
                                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #radioProblem").trigger("change");
                                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReasonComments").val('');
                                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtComplaintsICD").val(appointment_detail.txtReasonComments);
                            }
                            else {
                                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #radioFreetext").prop("checked", true);
                                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #radioFreetext").trigger("change");
                                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReasonComments").val(appointment_detail.txtReasonComments);
                                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtComplaintsICD").val("");
                            }
                            if (appointment_detail.hfRefProvider != "") {
                                if ($("#Scheduling_Force_Booking #lnkRefProviderEdit").css("display") == "none") {
                                    $("#Scheduling_Force_Booking #lnkRefProviderEdit").css("display", "inline");
                                    $("#Scheduling_Force_Booking #lblRefProvider").css("display", "none");
                                }
                            }
                            //  BackgroundLoaderShow(false);

                            utility.SetAutoCompleteSource($('#Scheduling_Force_Booking #txtProvider'), $('#Scheduling_Force_Booking #hfProviderId'));
                            //internalize Referral Number
                            Scheduling_Force_Booking.FillReferralNumber();
                            Scheduling_Force_Booking.BindRefProvider();
                            //serialize Data.
                            if ($("#frmScheduling_Force_Booking #ddlInsurancePlan").val() == "") {
                                $('#CoPayment').prop("disabled", true);
                            } else {
                                $('#CoPayment').prop("disabled", false);
                            }
                            if (Scheduling_Force_Booking.params.checkin == "1") {
                                $('#CoPayment').prop("disabled", true);
                            }

                            // Start Reschedule Appointment

                            if (Scheduling_Force_Booking.params.PanelID == "pnlRescheduleSearch") {
                                $('#Scheduling_Force_Booking #txtFromTime').val(Scheduling_Force_Booking.params.Time);
                                $('#Scheduling_Force_Booking #txtToTime').val(Scheduling_Force_Booking.params.ToTime);
                                $('#Scheduling_Force_Booking #txtScheduleDate').val(Scheduling_Force_Booking.params.ScheduleDate);
                                //$("#Scheduling_Force_Booking #ddlStatus").removeAttr("selected");
                                //$("#Scheduling_Force_Booking #ddlStatus option").filter(function () {
                                //    return $(this).text().trim().toUpperCase() == 'SCHEDULED';
                                //}).attr('selected', 'selected');
                                var value = $("option").filter(function () {
                                    return $(this).text() === 'Scheduled';
                                }).first().attr("value");
                                $("#Scheduling_Force_Booking #ddlStatus").val(value);
                                $('#Scheduling_Force_Booking #Duration').trigger('blur');
                            }

                            // End Reschedule Appointment

                            $('#Scheduling_Force_Booking #frmScheduling_Force_Booking').data('serialize', $('#Scheduling_Force_Booking #frmScheduling_Force_Booking').serialize());
                        });
                        $('#Scheduling_Force_Booking #btnUnallocatedCopayment').attr("disabled", false);
                    });
                }

                else {
                    utility.DisplayMessages(response.Message, 3);
                    BackgroundLoaderShow(false);
                }

            });

        }
    },

    BindPatientAccount: function () {
        var AccountNo = $('#Scheduling_Force_Booking #txtAccountNo').val();
        // Start 08/03/2016 Muhammad Irfan for bug # PMS-4361
        utility.Keyupdelay(function () {
            var AllPatients = utility.GetPatientArray(AccountNo, 1).done(function (response) {

                // Start 29/01/2016 Muhammad Irfan for Bug # 3738
                if (AccountNo == "") {
                    $('#Scheduling_Force_Booking #frmScheduling_Force_Booking #txtFullName,#dtpDOB,#txtPatientBalance,#txtInsuranceBalance,#txtAdvanceBalance,#txtAdvanceBalance,#txtRefProvider,#txtPriority,#txtReferralNo,#CoPayment').val('');
                    $('#Scheduling_Force_Booking #frmScheduling_Force_Booking #txtReferralNo').val('');
                    $('#Scheduling_Force_Booking #frmScheduling_Force_Booking #ddlInsurancePlan option').remove();
                    if ($("#Scheduling_Force_Booking #lnkRefProviderEdit").css("display") == "inline") {
                        $("#Scheduling_Force_Booking #lnkRefProviderEdit").css("display", "none");
                        $("#Scheduling_Force_Booking #lblRefProvider").css("display", "inline");
                    }
                    if ($("#Scheduling_Force_Booking #lnkReferralNumberEdit").css("display") == "inline") {
                        $("#Scheduling_Force_Booking #lnkReferralNumberEdit").css("display", "none");
                        $("#Scheduling_Force_Booking #lblReferralNumber").css("display", "inline");
                    }


                }
                $("#Scheduling_Force_Booking #txtAccountNo").autocomplete({
                    autoFocus: true,
                    source: response,
                    open: function (event, ui) { disable = true },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#Scheduling_Force_Booking #txtAccountNo").val(ui.item.AccountNumber);
                            Scheduling_Force_Booking.FillPatientAccount(ui.item.id);
                            utility.InsertRecentPatient(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {

                    setTimeout(function () {
                        utility.ValidateAutoComplete($("#Scheduling_Force_Booking #txtAccountNo"), "frmScheduling_Force_Booking #hfpatientid", false, 1, null, null);
                    }, 200);

                });


                $("#Scheduling_Force_Booking #txtAccountNo").autocomplete("search");
            });
        });
    },
    BindPatientName: function () {
        var AccountNo = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName').val();
        utility.Keyupdelay(function () {
            var AllPatients = utility.GetPatientArrayByName(AccountNo, 1).done(function (response) {
                if (AccountNo == "") {
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName,#dtpDOB,#txtPatientBalance,#txtInsuranceBalance,#txtAdvanceBalance,#txtAdvanceBalance,#txtRefProvider,#txtPriority,#txtReferralNo,#CoPayment').val('');
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReferralNo').val('');
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlInsurancePlan option').remove();
                    if ($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display") == "inline") {
                        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display", "none");
                        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lblRefProvider").css("display", "inline");
                    }
                    if ($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkReferralNumberEdit").css("display") == "inline") {
                        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkReferralNumberEdit").css("display", "none");
                        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lblReferralNumber").css("display", "inline");
                    }


                }


                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName").autocomplete({

                    autoFocus: true,
                    source: response,
                    open: function (event, ui) { disable = true },
                    close: function (event, ui) {
                        disable = false; $(this).focus();
                    },
                    select: function (event, ui) {
                        setTimeout(function () {
                            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName").val(ui.item.FullName);
                            Scheduling_Force_Booking.FillPatientAccount(ui.item.id);
                            utility.InsertRecentPatient(ui.item.id);
                        }, 100);
                    }
                }).blur(function () {
                    setTimeout(function () {
                        utility.ValidateAutoCompletePatientName($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName"), "frmSchedulingForceBooking #hfpatientid", false, 1, null, null);
                    }, 200);
                });


                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName").autocomplete("search");
                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName").focus();
            });
        });

    },

    FillDDLAppointmentStatus: function () {
        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlStatus option").filter(function () {
            return $(this).text().trim().toUpperCase() == 'CHECK IN';
        }).prop('disabled', true);

        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlStatus option").filter(function () {
            return $(this).text().trim().toUpperCase() == 'CHECK OUT';
        }).prop('disabled', true);

        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlStatus option").filter(function () {
            return $(this).text().trim().toUpperCase() == 'SCHEDULED';
        }).attr('selected', 'selected');

        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('serialize', $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').serialize());

    },

    BindInsurancePlans: function (InsuranceJSON, appInsuranceId) {
        var InsuranceJSON_detail = $.grep(JSON.parse(InsuranceJSON), function (obj) {
            if (obj.IsActive.toLowerCase() == 'true') {
                return obj;
            }
        });
        $("#frmSchedulingForceBooking #ddlInsurancePlan").empty();
        $("#frmSchedulingForceBooking #ddlInsurancePlan").append($('<option/>', {
            value: "",
            html: "- SELECT -",
            priority: "",
            coPayment: "",
            specialistCopay: ""
        }));
        $.each(InsuranceJSON_detail, function (i, item) {
            if (item.IsActive == "True") {
                $("#frmSchedulingForceBooking #ddlInsurancePlan").append(
                    $('<option />', {
                        value: item.InsuranceId,
                        html: item.InsurancePlanName,
                        priority: item.PlanPriority,
                        coPayment: item.AmtCopay,
                        specialistCopay: item.SpecialistCopay
                    })
                );
            }

        });

        if (InsuranceJSON_detail.length > 0) {
            for (var i = 0; i < InsuranceJSON_detail.length; i++) {
                if (Scheduling_Force_Booking.params.mode == "Add") {
                    if ($('#frmSchedulingForceBooking #rdPCP').is(':checked')) {

                        $('#frmSchedulingForceBooking #ddlInsurancePlan').val(InsuranceJSON_detail[0].InsuranceId);
                        $('#frmSchedulingForceBooking #txtPriority').val(InsuranceJSON_detail[0].PlanPriority);
                        if (InsuranceJSON_detail[0].AmtCopay != "")
                            $('#frmSchedulingForceBooking #CoPayment').val(parseFloat(InsuranceJSON_detail[0].AmtCopay).toFixed(2));


                    }
                    else if ($('#frmSchedulingForceBooking #rdSpecialist').is(':checked')) {

                        $('#frmSchedulingForceBooking #ddlInsurancePlan').val(InsuranceJSON_detail[0].InsuranceId);
                        $('#frmSchedulingForceBooking #txtPriority').val(InsuranceJSON_detail[0].PlanPriority);
                        if (InsuranceJSON_detail[0].SpecialistCopay != "")
                            $('#frmSchedulingForceBooking #CoPayment').val(parseFloat(InsuranceJSON_detail[0].SpecialistCopay).toFixed(2));
                        //}
                    }


                }


            }
            if (Scheduling_Force_Booking.params.mode == "Add") {
                Scheduling_Force_Booking.OpenPatReferralSearch($('#frmSchedulingForceBooking #ddlInsurancePlan').val());
            }

            if ($("#frmSchedulingForceBooking #ddlInsurancePlan").val() == "") {
                $('#CoPayment').prop("disabled", true);
            } else {
                $('#CoPayment').prop("disabled", false);
            }

        } else {
            if ($("#frmSchedulingForceBooking #ddlInsurancePlan").val() == "") {
                $('#CoPayment').prop("disabled", true);
            } else {
                $('#CoPayment').prop("disabled", false);
            }
            $('#frmSchedulingForceBooking #CoPayment').val("");
            $('#frmSchedulingForceBooking #txtPriority').val("");
        }
        Scheduling_Force_Booking.FillDDLAppointmentStatus();
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('bootstrapValidator').enableFieldValidators('Duration', true);
    },
    SaveScheduling_Force_Booking: function () {
        var strMessage = "";
        var self = $("#PnlSchedulingForceBooking");
        var myJSON = self.getMyJSON();
        var object = JSON.parse(myJSON);
        if ($('#PnlSchedulingForceBooking #radioProblem').is(":checked")) {
            object["txtReasonComments"] = $('#PnlSchedulingForceBooking #txtComplaintsICD').val();
            object["ReasonCommentType"] = "ICD";
        }
        else if ($('#PnlSchedulingForceBooking #radioFreetext').is(":checked")) {
            object["txtReasonComments"] = $('#PnlSchedulingForceBooking #txtReasonComments').val();
            object["ReasonCommentType"] = "FreeText";
        }
        myJSON = JSON.stringify(object);
        var patternJSON;
        var $tab = $('#PnlSchedulingForceBooking #myTabContent'), $active = $tab.find('.tab-pane.active'), text = $active.find('p:hidden').text();
        //alert(text)
        if (text == 'Daily') {
            var self = $("#PnlSchedulingForceBooking #Daily");
            patternJSON = self.getMyJSON();
            var json2 = { "pattern": "Daily" };
            patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
        }
        else if (text == 'Weekly') {
            var self = $("#PnlSchedulingForceBooking #Weekly");
            patternJSON = self.getMyJSON();
            var json2 = { "pattern": "Weekly" };
            patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
            //Scheduling_Force_Booking.ValidateAppointment();
        }
        else if (text == 'Monthly') {
            var self = $("#PnlSchedulingForceBooking #Monthly");
            patternJSON = self.getMyJSON();
            var json2 = { "pattern": "Monthly" };
            patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
        }

        patternJSON = patternJSON

        var j = { "name": "Daily" };
        JSON.stringify(j);
        if (Scheduling_Force_Booking.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Appointment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var result;
                    result = Scheduling_Force_Booking.ValidateValues();
                    if (result != false) {
                        if ($('#hfIsPatientActive').val() == 1) {
                            Scheduling_Force_Booking.SaveAppointment(myJSON, patternJSON).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);


                                    UnloadActionPan(Scheduling_Force_Booking.params["ParentCtrl"], "actionPanSchedulingForceBooking");

                                    if (Scheduling_Force_Booking.params.ParentCtrl == "schTabCalendar") {
                                        var statusslots = Scheduling_Calendar.FilterCriteria();
                                        Scheduling_Calendar.DayCalendar(Scheduling_Force_Booking.params.ProviderId, null, Scheduling_Force_Booking.params.FacilityId, Scheduling_Force_Booking.params.DayDate, statusslots);
                                        if (Scheduling_Force_Booking.params.ProviderId != "")
                                            $("#pnlScheduleCalendar #Provider option[value=" + Scheduling_Force_Booking.params.ProviderId + "]").attr('selected', 'selected');
                                        if (Scheduling_Force_Booking.params.FacilityId != "")
                                            $("#pnlScheduleCalendar #Facility option[value=" + Scheduling_Force_Booking.params.FacilityId + "]").attr('selected', 'selected');
                                        if (Scheduling_Force_Booking.params.DayDate != "")
                                            $('#pnlScheduleCalendar #daydate span').html(Scheduling_Force_Booking.params.DayDate);
                                    }
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        } else {
                            utility.DisplayMessages("Patient is Inactive/deceased", 2);
                        }

                    }

                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
        else if (Scheduling_Force_Booking.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var result;
                    result = Scheduling_Force_Booking.ValidateValues();
                    if (result != false) {
                        Scheduling_Force_Booking.UpdateAppointment(myJSON, Scheduling_Force_Booking.params.AppointmentId).done(function (response) {
                            if (response.status != false) {
                                //Admin_ProviderSchedule.ProviderScheduleSearch(Scheduling_Force_Booking.params.AppointmentId);
                                utility.DisplayMessages(response.message, 1);
                                UnloadActionPan(Scheduling_Force_Booking.params["ParentCtrl"], "actionPanScheduling_Force_Booking");

                                var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                //expression for week range
                                var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                                //Month Regular Expression
                                var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
                                if (Scheduling_Force_Booking.params.PanelID == "pnlScheduleMuliView") {

                                    var providerid = Scheduling_Force_Booking.params.ProviderId;
                                    var facilityid = Scheduling_Force_Booking.params.FacilityId;
                                    var resourceid = Scheduling_Force_Booking.params.ResourceId;
                                    var date = Scheduling_Force_Booking.params.DayDate;
                                    var dateid = Scheduling_Force_Booking.params.DateId;

                                    Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);

                                }
                                else if (Scheduling_Force_Booking.params.PanelID == "schEditSlot") {
                                    schEditSlot.SelectSlotDetail(Scheduling_Force_Booking.params.TimeslotDetailid, Scheduling_Force_Booking.params.ProviderId, Scheduling_Force_Booking.params.ResourceId);

                                    if (Scheduling_Force_Booking.params.MultipleView == 1) {

                                        var providerid = Scheduling_Force_Booking.params.ProviderId;
                                        var facilityid = Scheduling_Force_Booking.params.FacilityId;
                                        var resourceid = Scheduling_Force_Booking.params.ResourceId;
                                        var date = Scheduling_Force_Booking.params.DayDate;
                                        var dateid = Scheduling_Force_Booking.params.DateId;

                                        Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);

                                    }

                                    else if (Scheduling_Force_Booking.params.DayDate.match(weekrg) && Scheduling_Force_Booking.params.DayDate.length > 15) {
                                        var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                                        Scheduling_Calendar.ClearTable();

                                        var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                                        var week = Scheduling_Calendar.GetWeek(date1);
                                        $('#pnlScheduleCalendar #daydate span').html(week);
                                        //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                                        var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                                        $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                                    }
                                    else {
                                        var statusslots = Scheduling_Calendar.FilterCriteria();
                                        Scheduling_Calendar.DayCalendar(Scheduling_Force_Booking.params.ProviderId, Scheduling_Force_Booking.params.ResourceId, Scheduling_Force_Booking.params.FacilityId, Scheduling_Force_Booking.params.DayDate, statusslots);
                                        if (Scheduling_Force_Booking.params.ProviderId != "")
                                            $("#pnlScheduleCalendar #Provider option[value=" + Scheduling_Force_Booking.params.ProviderId + "]").attr('selected', 'selected');
                                        if (Scheduling_Force_Booking.params.ResourceId != "")
                                            $("#pnlScheduleCalendar #Resource option[value=" + Scheduling_Force_Booking.params.ResourceId + "]").attr('selected', 'selected');
                                        if (Scheduling_Force_Booking.params.FacilityId != "")
                                            $("#pnlScheduleCalendar #Facility option[value=" + Scheduling_Force_Booking.params.FacilityId + "]").attr('selected', 'selected');
                                        if (Scheduling_Force_Booking.params.DayDate != "")
                                            $('#pnlScheduleCalendar #daydate span').html(Scheduling_Force_Booking.params.DayDate);
                                    }
                                } else if (Scheduling_Force_Booking.params.ParentCtrl == "mstrTabDashBoard") {
                                    DashBoard.DashBoardAppRequestSearch();
                                } else if (Scheduling_Force_Booking.params.ParentCtrl == "Scheduling_RescheduleSearch") {
                                    Scheduling_RescheduleSearch.ScheduleSearch();
                                }
                                    //-------------------------- Start Reschedule Appointment

                                else if (Scheduling_Force_Booking.params.PanelID == "pnlRescheduleSearch") {

                                    Scheduling_RescheduleAppointment.MoveAppointment(Scheduling_Force_Booking.params.AppointmentId, Scheduling_Force_Booking.params.SlotdetailId).done(function (response) {
                                        if (response.status != false) {
                                            Scheduling_RescheduleSearch.ScheduleSearch();
                                            if (Scheduling_Force_Booking.params.MultipleView == 1) {

                                                var providerid = Scheduling_Force_Booking.params.ProviderId;
                                                var facilityid = Scheduling_Force_Booking.params.FacilityId;
                                                var resourceid = Scheduling_Force_Booking.params.ResourceId;
                                                var date = Scheduling_Force_Booking.params.DayDate;
                                                var dateid = Scheduling_Force_Booking.params.DateId;

                                                Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);

                                            }
                                            else if (Scheduling_Force_Booking.params.DayDate.match(weekrg) && Scheduling_Force_Booking.params.DayDate.length > 15) {
                                                var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                                                Scheduling_Calendar.ClearTable();
                                                var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                                                var week = Scheduling_Calendar.GetWeek(date1);
                                                $('#pnlScheduleCalendar #daydate span').html(week);
                                                var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                                                $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                                            }
                                            else {
                                                var statusslots = Scheduling_Calendar.FilterCriteria();
                                                Scheduling_Calendar.DayCalendar(Scheduling_Force_Booking.params.ProviderId, Scheduling_Force_Booking.params.ResourceId, $("#pnlScheduleCalendar #Facility").val(), Scheduling_Force_Booking.params.DayDate, statusslots);
                                                //if (Scheduling_Force_Booking.params.ProviderId != "")
                                                //    $("#pnlScheduleCalendar #Provider option[value=" + Scheduling_Force_Booking.params.ProviderId + "]").attr('selected', 'selected');
                                                //if (Scheduling_Force_Booking.params.ResourceId != "")
                                                //    $("#pnlScheduleCalendar #Resource option[value=" + Scheduling_Force_Booking.params.ResourceId + "]").attr('selected', 'selected');
                                                //if (Scheduling_Force_Booking.params.FacilityId != "")
                                                //    $("#pnlScheduleCalendar #Facility option[value=" + Scheduling_Force_Booking.params.FacilityId + "]").attr('selected', 'selected');
                                                //if (Scheduling_Force_Booking.params.DayDate != "")
                                                //    $('#pnlScheduleCalendar #daydate span').html(Scheduling_Force_Booking.params.DayDate);
                                            }
                                        } else {

                                        }
                                    });
                                }

                                    //-------------------------- End Reschedule Appointment
                                else if (Scheduling_Force_Booking.params.DayDate.match(weekrg) && Scheduling_Force_Booking.params.DayDate.length > 15) {
                                    var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                                    Scheduling_Calendar.ClearTable();

                                    var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                                    var week = Scheduling_Calendar.GetWeek(date1);
                                    $('#pnlScheduleCalendar #daydate span').html(week);
                                    //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                                    var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                                    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                                }
                                else {
                                    var statusslots = Scheduling_Calendar.FilterCriteria();
                                    Scheduling_Calendar.DayCalendar(Scheduling_Force_Booking.params.ProviderId, Scheduling_Force_Booking.params.ResourceId, Scheduling_Force_Booking.params.FacilityId, Scheduling_Force_Booking.params.DayDate, statusslots);
                                    if (Scheduling_Force_Booking.params.ProviderId != "")
                                        $("#pnlScheduleCalendar #Provider option[value=" + Scheduling_Force_Booking.params.ProviderId + "]").attr('selected', 'selected');
                                    if (Scheduling_Force_Booking.params.ResourceId != "")
                                        $("#pnlScheduleCalendar #Resource option[value=" + Scheduling_Force_Booking.params.ResourceId + "]").attr('selected', 'selected');
                                    if (Scheduling_Force_Booking.params.FacilityId != "")
                                        $("#pnlScheduleCalendar #Facility option[value=" + Scheduling_Force_Booking.params.FacilityId + "]").attr('selected', 'selected');
                                    if (Scheduling_Force_Booking.params.DayDate != "")
                                        $('#pnlScheduleCalendar #daydate span').html(Scheduling_Force_Booking.params.DayDate);
                                }
                            }
                            else {
                                utility.DisplayMessages(response.Message, 3);
                            }

                        });
                    }
                }
                else
                    utility.DisplayMessages(strMessage, 2);
            });
        }
    },

    FillDuration: function (control, addCase) {

        var ScheduleReasonID = $(control).val();
        var slot = Scheduling_Force_Booking.params.SlotdetailId;
        var pro = Scheduling_Force_Booking.params.ProviderId;
        var res = Scheduling_Force_Booking.params.ResourceId;

        if (addCase == 0) {

            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration').val(Scheduling_Force_Booking.params.SlotMinutes);
            if ($('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('bootstrapValidator') != null && typeof $(' #frmSchedulingForceBooking').data('bootstrapValidator') != 'undefined') {
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').bootstrapValidator('revalidateField', 'Duration');
            }

            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration').trigger('change');

            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('serialize', $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').serialize());

        }
        else if (addCase == 1) {
            Scheduling_Force_Booking.FillScheduleReasonDuration(ScheduleReasonID).done(function (response) {
                if (response.status != false) {
                    var reasonduration = JSON.parse(response.ProviderScheduleFill_JSON);
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration').val(reasonduration.Duration);
                    if ($('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('bootstrapValidator') != null && typeof $('#frmSchedulingForceBooking').data('bootstrapValidator') != 'undefined') {
                        $('#frmSchedulingForceBooking').bootstrapValidator('revalidateField', 'Duration');
                    }

                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration').trigger('change');
                    //serialize Data.
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('serialize', $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').serialize());


                }
                else {

                }
            });
        }

    },

    FillInsuranceData: function (control, controlToChange) {

        var priority = $("#frmScheduling_Force_Booking #ddlInsurancePlan option:selected").attr("priority");
        $("#frmScheduling_Force_Booking #txtPriority").val(priority);

        //$("#Scheduling_Force_Booking #txtReferralNo").val("");

        var specialistCopay = $("#frmScheduling_Force_Booking #ddlInsurancePlan option:selected").attr("specialistCopay");
        var coPayment = $("#frmScheduling_Force_Booking #ddlInsurancePlan option:selected").attr("coPayment");

        if ($('#Scheduling_Force_Booking #rdPCP').is(':checked')) {

            if (coPayment != NaN && coPayment != "" && typeof (coPayment) != 'undefined')
                $("#frmScheduling_Force_Booking #CoPayment").val(parseFloat(coPayment).toFixed(2));
            else
                $("#frmScheduling_Force_Booking #CoPayment").val('');
        }
        else if ($('#Scheduling_Force_Booking #rdSpecialist').is(':checked')) {
            if (specialistCopay != NaN && specialistCopay != "" && typeof (specialistCopay) != 'undefined')
                $("#frmScheduling_Force_Booking #CoPayment").val(parseFloat(specialistCopay).toFixed(2));
            else
                $("#frmScheduling_Force_Booking #CoPayment").val('');
        }
        if ($("#frmScheduling_Force_Booking #ddlInsurancePlan").val() == "") {
            $('#CoPayment').prop("disabled", true);
        } else {
            $('#CoPayment').prop("disabled", false);
        }

    },

    FillPatientInfoFromSearch: function (PatientId, event) {
        if (event != null) {
            event.stopPropagation();
            if (event.target.type == "checkbox") {
                $(':checkbox', this).trigger('click');
                return;
            }
        }
        UnloadActionPan("Scheduling_Force_Booking", "Patient_Search");

        setTimeout(function () { Scheduling_Force_Booking.FillPatientAccount(PatientId); }, 200);


        if ($('#frmScheduling_Force_Booking').data('bootstrapValidator') != null && typeof $('#frmScheduling_Force_Booking').data('bootstrapValidator') != 'undefined') {
            $('#frmScheduling_Force_Booking').bootstrapValidator('revalidateField', 'patientAccount');
        }
        utility.InsertRecentPatient(PatientId);
    },

    FillPatientAccount: function (PatientId, appInsuranceId) {

        var dfd = new $.Deferred();
        Scheduling_Force_Booking.PatientInsuranceId = appInsuranceId;
        Scheduling_Force_Booking.FillPatient(PatientId).done(function (response) {
            if (response.status != false) {
                Patient_Demographic.FillPatientAlertsCount('1', PatientId, 'Scheduling_Force_Booking').done(function () {
                    if (PatientId != $('#PatientProfile #hfPatientId').val()) {
                        Patient_Demographic.isFinanicialAlert = true;
                    }
                });
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtRefProvider').val("");
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReferralNo').val("");
                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkReferralNumberEdit").css("display", "none");
                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lblReferralNumber").css("display", "inline");

                var patient_detail = JSON.parse(response.PatientFill_JSON);
                var patient_balance = JSON.parse(response.PatientBalance_JSON);
                var patient = JSON.parse(response.Patient_JSON);
                var patient_insurance = JSON.parse(response.PatientInsurance_JSON);

                if (patient_balance.length > 0) {
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtPatientBalance').val(parseFloat(Number(patient_balance[0].PatientBalance)).toFixed(2));
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtAdvanceBalance').val(parseFloat(Number(patient_balance[0].AdvanceBalance)).toFixed(2));
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtInsuranceBalance').val(parseFloat(Number(patient_balance[0].InsuranceBalance)).toFixed(2));
                }

                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtAccountNo').val(patient_detail.txtAccountNo);
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName').val(patient_detail.txtFullName);
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #dtpDOB').val(patient_detail.dtpDOB);
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfpatientid').val(patient_detail.hfpatientid);
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfIsPatientActive').val(patient_detail.hfIsPatientActive);



                Scheduling_Force_Booking.params.PatientLastName = patient[0].LastName;
                Scheduling_Force_Booking.params.PatientFirstName = patient[0].FirstName;

                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtRefProvider').val(patient[0].ReferringProviderName);
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfRefProvider').val(patient[0].ReferringProviderId);

                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtRefProvider').val(patient[0].ReferringProviderName);
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfRefProvider').val(patient[0].ReferringProviderId);

                Scheduling_Force_Booking.BindInsurancePlans(response.PatientInsurance_JSON, appInsuranceId);
                //Scheduling_Force_Booking.ValidateAppointment();
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('serialize', $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').serialize());
                if ($('#frmSchedulingForceBooking').data('bootstrapValidator') != null && typeof $('#frmSchedulingForceBooking').data('bootstrapValidator') != 'undefined') {
                    $('#frmSchedulingForceBooking').bootstrapValidator('revalidateField', 'FullName');
                }
                dfd.resolve('ok');
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd.promise();
    },

    ValidateAppointment: function () {
        $('#frmSchedulingForceBooking')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   FullName: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },

                   Duration: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Status: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PatientType: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   PatientVisitType: {
                       group: '.col-sm-3',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   // Begin 20-Jan-2016  Removed By Azeem Raza Tayyab Bug # PMS-3477
                   /*Reason: {
                       group: '.col-sm-7',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },*/
                   // End 20-Jan-2016  Removed By Azeem Raza Tayyab Bug # PMS-3477
                   DailyAppointment: {
                       group: '.col-md-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   txtactiveweek: {
                       group: '.col-md-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DailyEveryDays: {
                       group: '.col-md-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   DailyEndByDate: {
                       group: '.col-md-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   WeeklyEndByDate: {
                       group: '.col-md-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   WeeklyAppointment: {
                       group: '.col-md-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   MonthlyEndByDate: {
                       group: '.col-md-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   MonthlyAppointment: {
                       group: '.col-md-6',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   txtmntActive: {
                       group: '.col-md-8',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   txtmntofevry: {
                       group: '.col-md-8',
                       enabled: false,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   'themonths[]': {
                       group: '.themonthsVD',
                       validators: {
                           choice: {
                               min: 1,
                               message: 'Please atleast choose 1.'
                           }
                       }
                   },
                   'theweeksM[]': {
                       group: '.theweeksmVD',
                       validators: {
                           choice: {
                               min: 1,
                               message: 'Please atleast choose 1.'
                           }
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
                   FromTime: {
                       group: '.col-sm-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   ScheduleDate: {
                       group: '.col-sm-2',
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

               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            Scheduling_Force_Booking.SaveScheduling_Force_Booking();
        });
    },

    ResetHiddenValue: function () {
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfpatientid').val("-1");
    },

    ResetRefProvider: function () {

        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfRefProvider').val(null);

    },

    ValidateValues: function () {
        if ($('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfpatientid').val() == "-1") {
            utility.DisplayMessages("Patient not Valid", 2);
            return false;
        }
        else
            return true;

    },

    SaveAppointment: function (AppointmentData, AppointmentPatternData) {

        var data = "AppointmentData=" + AppointmentData + "&AppointmentPatternData=" + AppointmentPatternData
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "SAVE_APPOINTMENT");
    },

    FillPatient: function (PatientID) {
        var data = "PatientID=" + PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_PATIENT");
    },

    FillRefProviderName: function (RefProviderId, RefProviderName) {
        $('#Scheduling_Force_Booking #txtRefProvider').val(RefProviderName);
        $('#Scheduling_Force_Booking #hfRefProvider').val(RefProviderId);
        Scheduling_Force_Booking.BindRefProvider();
        UnloadActionPan(Scheduling_Force_Booking.params["ParentCtrl"]);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";

        params["ParentCtrl"] = "Scheduling_Force_Booking";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenPatientAccount: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Scheduling_Force_Booking';
        LoadActionPan('Patient_Search', params);
    },
    FillReferralNumber: function () {
        var AccountNo = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtAccountNo').val();
        if (AccountNo == "") {
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName,#dtpDOB,#txtPatientBalance,#txtInsuranceBalance,#txtAdvanceBalance,#txtAdvanceBalance,#txtRefProvider,#txtPriority,#txtReferralNo,#CoPayment').val('');
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlInsurancePlan option').remove();
            if ($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display") == "inline") {
                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display", "none");
                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lblRefProvider").css("display", "inline");
            }


        }


        // End 29/01/2016 Muhammad Irfan for Bug # 3738
        var ParentCtrl = "Scheduling_Force_Booking";
        var self = $("#PnlSchedulingForceBooking #frmSchedulingForceBooking");

        var patientID = self.find("#hfpatientid").val();
        var selectedInsurancePlan = self.find("#ddlInsurancePlan option:selected").val();
        var objReferralNumber = self.find("#txtReferralNo");
        var ProviderId = self.find("#hfProviderId").val();
        var FacilityId = self.find("#hfFacilityId").val();
        var VisitDate = self.find("#dtpDOB").val();
        if (selectedInsurancePlan == "") {
            objReferralNumber.attr("disabled", "disabled");
        }
        else {
            objReferralNumber.removeAttr("disabled");
        }

        if (selectedInsurancePlan != "" && ProviderId != "" && FacilityId != "" && VisitDate != "") {
            patientReferralSearch.SearchReferral("Incoming", selectedInsurancePlan, ProviderId, FacilityId, VisitDate, "1").done(function (response) {
                if (response.status != false) {
                    var PatientReferral = [];
                    if (response.ReferralCount > 0) {
                        var ReferralJSONData = JSON.parse(response.ReferralLoad_JSON);
                    }
                }
            });

        }
    },

    FillReferralPatientName: function () {

        var AccountNo = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName').val();
        if (AccountNo == "") {
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtFullName,#dtpDOB,#txtPatientBalance,#txtInsuranceBalance,#txtAdvanceBalance,#txtAdvanceBalance,#txtRefProvider,#txtPriority,#txtReferralNo,#CoPayment').val('');
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlInsurancePlan option').remove();
            if ($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display") == "inline") {
                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display", "none");
                $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lblRefProvider").css("display", "inline");
            }


        }



        var ParentCtrl = "Scheduling_Force_Booking";
        var self = $("#PnlSchedulingForceBooking #frmSchedulingForceBooking");

        var patientID = self.find("#hfpatientid").val();
        var selectedInsurancePlan = self.find("#ddlInsurancePlan option:selected").val();
        var objReferralNumber = self.find("#txtReferralNo");
        var ProviderId = self.find("#hfProviderId").val();
        var FacilityId = self.find("#hfFacilityId").val();
        var VisitDate = self.find("#dtpDOB").val();
        if (selectedInsurancePlan == "") {
            objReferralNumber.attr("disabled", "disabled");
        }
        else {
            objReferralNumber.removeAttr("disabled");
        }

        if (selectedInsurancePlan != "" && ProviderId != "" && FacilityId != "" && VisitDate != "") {
            patientReferralSearch.SearchReferral("Incoming", selectedInsurancePlan, ProviderId, FacilityId, VisitDate, "1").done(function (response) {
                if (response.status != false) {
                    var PatientReferral = [];
                    if (response.ReferralCount > 0) {
                        var ReferralJSONData = JSON.parse(response.ReferralLoad_JSON);
                    }
                }
            });

        }
    },

    OpenPatientReferral: function () {
        var self = $("#PnlSchedulingForceBooking #frmSchedulingForceBooking");
        var patientInsuranceID = self.find("#ddlInsurancePlan option:selected").val();

        if (patientInsuranceID != "") {
            var params = [];
            params["ParentCtrl"] = 'Scheduling_Force_Booking';
            var ProviderId = self.find("#hfProviderId").val();
            var FacilityId = self.find("#hfFacilityId").val();
            var DOSFrom = Scheduling_Force_Booking.params.DayDate;
            params["RefCtrlReferralNo"] = "txtReferralNo";
            params["RefHiddenIdCtrl"] = "hfReferralNumerId";
            params["RefCtrlRefProvider"] = "txtRefProvider";
            params["RefProviderHiddenIdCtrl"] = "hfRefProvider";
            params["patientID"] = self.find("#hfpatientid").val();
            params["patientInsuranceID"] = patientInsuranceID;
            params["ProviderId"] = ProviderId;
            params["FacilityId"] = null;
            params["ReferralDate"] = DOSFrom;

            LoadActionPan('patientReferralSearch', params);
        } else {
            utility.DisplayMessages("Please select insurance plan first", 2);
        }


    },

    OpenPatientReferralDetail: function (HiddenCtrl) {

        var params = [];
        params["ParentCtrl"] = "Scheduling_Force_Booking";
        params["ReferralID"] = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #' + HiddenCtrl).val();
        params["patientID"] = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfpatientid').val();
        params["mode"] = "Edit";
        LoadActionPan('Patient_Referral', params);

    },

    FillScheduleReasonDuration: function (ScheduleReasonID) {
        var data = "ScheduleReasonID=" + ScheduleReasonID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_SCHEDULEREASON_DURATION");
    },

    FillInsuranceCopayment: function (InsurancePlanID) {
        var data = "InsurancePlanID=" + InsurancePlanID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_COPAYMENT");
    },

    FillAppointment: function (AppointmentID) {
        var data = "AppointmentID=" + AppointmentID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_APPOINTMENT");
    },

    UpdateAppointment: function (AppointmentData, AppointmentID) {

        var data = "AppointmentData=" + AppointmentData + "&AppointmentID=" + AppointmentID + "&ReferralId=" + Scheduling_Force_Booking.ReferralId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "UPDATE_APPOINTMENT");
    },

    LoadActivePatients: function (AccountNo) {
        var data = "AccountNo=" + AccountNo;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "SEARCH_PATIENT");
    },

    LoadActivePatientsByName: function (Searchstring) {
        var data = "AccountNo=" + Searchstring;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "SEARCH_PATIENT_BY_NAME");
    },

    DeletePatientAppointment: function (AppointmentId) {
        var data = "AppointmentID=" + AppointmentId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "DELETE_PATIENT_APPOINTMENT");
    },

    ProviderSearch: function (ProviderId) {


        if (Scheduling_Force_Booking.params.IsSpecialist == "False") {
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #rdPCP').attr("checked", "checked");
        }
        else
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #rdSpecialist').attr("checked", "checked");



    },

    SearchProvider: function (ProviderData, ProviderId) {
        var data = "ProviderData=" + ProviderData + "&ProviderID=" + ProviderId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER", "SEARCH_PROVIDER");
    },

    UnLoad: function () {
        if (Scheduling_Force_Booking.params["ParentCtrl"] == null) {
            Scheduling_Force_Booking.params["ParentCtrl"] = 'schwaitlistdetail';
        }
        utility.UnLoadDialog('PnlSchedulingForceBooking #frmSchedulingForceBooking', function () {
            UnloadActionPan(Scheduling_Force_Booking.params["ParentCtrl"], "actionPanSchedulingForceBooking");
        }, function () {
            UnloadActionPan(Scheduling_Force_Booking.params["ParentCtrl"], "actionPanSchedulingForceBooking");
        });


    },

    OpenRefProviderDetail: function () {
        var params = [];
        params["ReferringProviderId"] = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "Scheduling_Force_Booking";

        LoadActionPan('referringproviderDetail', params);
    },


    OpenPatReferralSearch: function (PatientInsuranceId) {


        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReferralNo').val('');


        var strMessage = "";
        var params = [];

        params["RefCtrlReferralNo"] = "txtReferralNo";
        params["RefCtrlRefProvider"] = "txtRefProvider";

        PatientInsuranceId = PatientInsuranceId != null ? PatientInsuranceId : "";
        var ProviderId = Scheduling_Force_Booking.params.ProviderId != null ? Scheduling_Force_Booking.params.ProviderId : "";
        var FacilityId = Scheduling_Force_Booking.params.FacilityId != null ? Scheduling_Force_Booking.params.FacilityId : "";;
        var ReferralType = "Incoming";
        if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {
            ProviderId = $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfProviderId").val();
        }
        if (Scheduling_Force_Booking.params.mode == "Edit") {
            VisitDate = $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfAppointmentDate").val();
        }
        else {
            var d = new Date();
            VisitDate = d.toISOString().substring(0, 10);
        }

        var ParentCtrl = "Scheduling_Force_Booking";
        var objReferralNumber = $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtReferralNo");
        var objRefProvider = $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtRefProvider");
        var objRefProviderId = $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfRefProvider");
        var patientID = $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfpatientid").val();
        if (PatientInsuranceId != "" && ProviderId != "" && VisitDate != "") {
            patientReferralSearch.SearchReferral(ReferralType, PatientInsuranceId, ProviderId, 0, VisitDate, "1").done(function (response) {
                if (response.status != false) {
                    if (response.ReferralCount > 1) {
                        EncounterChargeCapture.OpenReferralSearch(PatientInsuranceId, objReferralNumber.attr("id"), objRefProvider.attr("id"), response, ParentCtrl, patientID, "Incoming", ProviderId, 0, VisitDate, "1");
                    }
                    else if (response.ReferralCount == 1) {
                        var ReferralLoadJSONData = JSON.parse(response.ReferralLoad_JSON);
                        objReferralNumber.val(ReferralLoadJSONData[0].ReferralAuthNo);
                        //if (objRefProvider.val() == "") {
                        objRefProvider.val(ReferralLoadJSONData[0].ReferringFromName);
                        objRefProviderId.val(ReferralLoadJSONData[0].ReferringFromId);
                        objRefProvider.focus();
                        Scheduling_Force_Booking.BindRefProvider();
                        //}
                        if ($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display") == "none") {
                            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display", "inline");
                            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lblRefProvider").css("display", "none");
                        }
                    }
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
        }
    },


    AddTime: function (obj) {




        var fromTimeObj = $("#txtFromTime");
        var toTimeObj = $("#txtToTime")

        var timeFrom = new Date("01/01/2007 " + $('#txtFromTime').val());
        timeFrom.setMinutes(timeFrom.getMinutes() + Number($('#Duration').val()));
        $("#txtToTime").timepicker("setTime", timeFrom);
        //var timeEnd = new Date("01/01/2007 " + $('#txtToTime').val()).getMinutes();
        var endTime = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtToTime').val();
        if (endTime.match("^0")) {
            console.log("replace..");
            var endTime = endTime.replace("0", "12");
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtToTime').val(endTime);
        }
    },

    VisitTypeDropdownLoad: function () {
        Scheduling_Force_Booking.LoadVisittypeDropdown(Scheduling_Force_Booking.params.ProviderId).done(function (response) {
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').empty();
            if (response.length > 0) {
                $.each(response, function (j, result) {
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("Duration", result.RefName));
                });
            } else {
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').append('<option value="" refvalue="" duration="" style="display: block;">- Select -</option>');
            }
            Scheduling_Force_Booking.BindVisitType($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlPatientType"));
        });
    },

    BindVisitType: function (control) {
        var selectedPateintType = $(control).val();
        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType option:selected").prop("selected", false);
        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlPatientType option").each(function (i, item) {
            if (item.value == selectedPateintType) {
                Scheduling_Force_Booking.showDropdownOptions(item.value, true);
            }
            else if (item.value == "") {
                Scheduling_Force_Booking.showDropdownOptions('', true);
            }
            else
                Scheduling_Force_Booking.showDropdownOptions(item.value, false);
        });

        if (selectedPateintType == "") {
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').val("");
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').attr("disabled", true);
        }
        else {
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').removeAttr("disabled");
        }
    },

    showDropdownOptions: function (value, canShowOption) {
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').find('option[Duration="0"]').remove()
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').find('option[refvalue="' + value + '"]').map(function () {
            return $(this).parent('span').length === 0 ? this : null;
        }).wrap('<span>').hide();

        if (canShowOption) {

            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').find('option[refvalue="' + value + '"]').unwrap().show();
        }
        else
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType').find('option[refvalue="' + value + '"]').hide();

    },

    LoadVisittypeDropdown: function (ProviderId) {
        var data = "ProviderId=" + ProviderId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "LOAD_VISIT_TYPE_DROPDOWN");
    },
    /* start 03/02/2016 Muhammad Irfan bug # PMS-3827,3828 */
    changeFunction: function () {
        var objDuration = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration');
        if (objDuration.val() == 0) {
            objDuration.val('');
            if ($('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('bootstrapValidator') != null && typeof $('#frmSchedulingForceBooking').data('bootstrapValidator') != 'undefined') {
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').bootstrapValidator('revalidateField', 'Duration');
            }
        } else {

        }
    },

    /* start 03/02/2016 Muhammad Irfan bug # PMS-3827,3828 */

    FillScheduleReason: function (ScheduleReasonId, ShortName, Duration, event) {

        if (event != null) {
            event.stopPropagation();
        }

        UnloadActionPan("Scheduling_Force_Booking");

        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtSchReason').val(ShortName);
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfSchReasonId').val(ScheduleReasonId);
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration').val(Duration);

        if ($('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('bootstrapValidator') != null && typeof $('#frmSchedulingForceBooking').data('bootstrapValidator') != 'undefined') {
            $('#frmSchedulingForceBooking').bootstrapValidator('revalidateField', 'Duration');
        }


    },

    BindScheduleReasons: function () {
        var SchReason = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtSchReason').val();
        var AllSchReasons = [];
        var dfd = new $.Deferred();
        if (SchReason.length > 2) {
            utility.Keyupdelay(function () {
                blckreasonDetail.LoadScheduleReasons(SchReason).done(function (responseData) {
                    if (responseData.status != false) {
                        if (responseData.SchReasonCount > 0) {
                            var ScheduleReasons = JSON.parse(responseData.ScheduleReasonsLoad_JSON);

                            $.each(ScheduleReasons, function (i, item) {
                                AllSchReasons.push({ id: item.ScheduleReasonId, value: item.ShortName, Duration: item.Duration });
                            });

                        }
                    }
                    dfd.resolve(AllSchReasons);
                });
            });
        } else {
            dfd.resolve(AllSchReasons);
        }

        //---------------
        dfd.then(function () {

            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtSchReason').autocomplete({
                autoFocus: true,
                source: AllSchReasons,
                open: function (event, ui) { disable = true },
                close: function (event, ui) {
                    disable = false; $(this).focus();
                },
                select: function (event, ui) {
                    setTimeout(function () {
                        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtSchReason').val(ui.item.value);
                        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfSchReasonId').val(ui.item.id);
                        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration').val(ui.item.Duration)
                    }, 100);

                }
            }).blur(function () {
                setTimeout(function () {
                    utility.ValidateAutoComplete($('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtSchReason'), 'PnlSchedulingForceBooking #frmSchedulingForceBooking #hfSchReasonId', false, null, null, null);
                }, 200);
            });
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtSchReason').autocomplete("search");

        });

        //--------------------
    },

    OpenScheduleReason: function () {

        var params = [];
        params["ScheduleReasonId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "Scheduling_Force_Booking";
        LoadActionPan('Admin_ScheduleReason', params);

    },

    //----------------------------------

    BindRefProvider: function () {

        var shortName = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtRefProvider').val();
        utility.GetRefProviderArray(shortName).done(function (response) {

            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #txtRefProvider').autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfRefProvider").val(ui.item.id);
                        if ($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display") == "none") {
                            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lnkRefProviderEdit").css("display", "inline");
                            $("#PnlSchedulingForceBooking #frmSchedulingForceBooking #lblRefProvider").css("display", "none");
                        }
                    }, 100);

                }
            });

            BackgroundLoaderShow(false);
        });


    },

    BindProvider: function (isFullName, shortName) {


        if (!shortName)
            shortName = $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #txtProvider").val();

        if (isFullName)
            shortName = shortName.split(',')[0];

        utility.GetProviderArray(shortName).done(function (response) {

            $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #txtProvider").autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #txtProvider").val(ui.item.value);
                        $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #hfProvider").val(ui.item.id);
                        if ($("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #lnkProviderEdit").css("display") == "none") {
                            $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #lnkProviderEdit").css("display", "inline");
                            $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #lblProvider").css("display", "none");
                        }

                    }, 100);

                }
            });

        });
    },

    OpenFacility: function () {

        var params = [];
        params["FacilityId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefHiddenIdCtrl"] = "hfFacilityId";
        params["ParentCtrl"] = "Scheduling_Force_Booking";
        LoadActionPan('Admin_Facility', params);
    },


    changeIsNonBilable: function () {

        if ($("#PnlSchedulingForceBooking #frmSchedulingForceBooking #chkNonBilable").is(':checked')) {
            utility.myConfirm('Are you sure you want to make the Visit as Non Billable?', function () {
                //
            }, function () {
                $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #chkNonBilable').prop('checked', false);
            },
                   'Confirm Non Billable'
               );
        } else {
            //
        }
    },

    setDurationOnVisitType: function (obj) {
        var txtDuration = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration');
        if (Scheduling_Force_Booking.params.ProviderId && Scheduling_Force_Booking.params.ProviderId != "-1" && $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType option:selected').val() != "") {
            Scheduling_Force_Booking.FillDurationOnVisitType(Scheduling_Force_Booking.params.ProviderId, $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType option:selected').val()).done(function (response) {
                if (response.status != false) {
                    txtDuration.val(response.Duration);
                    $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration').trigger('blur');
                }
                else
                    Scheduling_Force_Booking.DefaultConfigurationForVisitType();
            });
        } else
            Scheduling_Force_Booking.DefaultConfigurationForVisitType();
    },
    DefaultConfigurationForVisitType: function () {
        var slotDuration = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #hfSlotDuration').val();
        var txtDuration = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration');
        var selectedVisitType = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlVisitType option:selected').text().trim(); // Visit Type

        var selectedPatientType = $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #ddlPatientType option:selected').text().trim(); // Patient Type

        if (selectedPatientType == '- Select -' && selectedPatientType == '- Select -') {
            txtDuration.val(slotDuration);
        } else if ((selectedPatientType == 'New Patient' || selectedPatientType == 'Established Patient') && selectedVisitType == '- Select -') {
            txtDuration.val(slotDuration);
        } else if (selectedPatientType == 'New Patient' && selectedVisitType == 'Annual Visit/Physical - New Patient') {
            txtDuration.val(30);
        } else if (selectedPatientType == 'New Patient' && (selectedVisitType != 'Annual Visit/Physical - New Patient' && selectedVisitType != '- Select -')) {
            txtDuration.val(45);
        } else if (selectedPatientType == 'Established Patient' && selectedVisitType == 'Annual Visit/Physical - Established Patient') {
            txtDuration.val(30);
        } else if (selectedPatientType == 'Established Patient' && selectedVisitType == 'General Follow-Up/Lab/Test Results - Established Patient') {
            txtDuration.val(30);
        } else if (selectedPatientType == 'Established Patient' && selectedVisitType != '- Select -') {
            txtDuration.val(30);
        } else {
            txtDuration.val(slotDuration);
        }
        $('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration').trigger('blur');
    },
    FillDurationOnVisitType: function (ProviderId, PatientVisitTypeId) {
        var data = "ProviderID=" + ProviderId + "&PatientVisitTypeID=" + PatientVisitTypeId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_DURATION_ON_VISIT_TYPE");
    },
    OpenProviderDetail: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        var params = [];
        params["Title"] = Title;
        params["RefCtrl"] = RefCtrl;
        params["RefCtrlHidden"] = RefCtrlHidden;
        params["RefCtrlLabel"] = RefCtrlLabel;
        params["RefCtrlLink"] = RefCtrlLink;
        params["IsOptional"] = false;
        params["RefForm"] = "frmSchedulingForceBooking";
        params["ProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'Scheduling_Force_Booking';
        LoadActionPan('Admin_Provider', params);
    },
    BindFacility: function () {

        var shortName = $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #txtFacility").val();
        utility.GetFacilityArray(shortName).done(function (response) {

            $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #txtFacility").autocomplete({
                autoFocus: true,
                source: response,
                select: function (event, ui) {
                    setTimeout(function () {

                        $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #txtFacility").val(ui.item.value);
                        $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #hfFacilityId").val(ui.item.id);
                        if ($("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #lnkFacilityEdit").css("display") == "none") {
                            $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #lnkFacilityEdit").css("display", "inline");
                            $("#" + Scheduling_Force_Booking.params.PanelID + " #frmSchedulingForceBooking #lblFacility").css("display", "none");
                        }

                    }, 100);

                }
            });
            //$("#" + Patient_Demographic.params.PanelID + " #frmDemographic #txtFacility").autocomplete("search");
        });
    },
    RevalidateFieds: function () {
        if ($('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('bootstrapValidator') != null && typeof $(' #frmSchedulingForceBooking').data('bootstrapValidator') != 'undefined') {
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').bootstrapValidator('revalidateField', 'ScheduleDate');
            $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').bootstrapValidator('revalidateField', 'FromTime');
        }

        //$('#PnlSchedulingForceBooking #frmSchedulingForceBooking #Duration').trigger('change');

        //$('#PnlSchedulingForceBooking #frmSchedulingForceBooking').data('serialize', $('#PnlSchedulingForceBooking #frmSchedulingForceBooking').serialize());

    },

}