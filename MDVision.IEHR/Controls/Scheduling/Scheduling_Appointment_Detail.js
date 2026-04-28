appointmentDetail = {
    bIsFirstLoad: true,
    params: [],
    PatientInsuranceId: '',
    ReferralId: '',
    LastSctBaseSearch: '',
    CancellationReason: '',
    Load: function (params) {

        appointmentDetail.CancellationReason = "";
        BackgroundLoaderShow(true);

        appointmentDetail.params = params;

        if (appointmentDetail.bIsFirstLoad) {
            appointmentDetail.bIsFirstLoad = false;
        }

        if (appointmentDetail.params.mode == "Edit") {

            $('#appointmentDetail #headingTitle').html('Edit Appointment');
            $('#appointmentDetail #btnsave').html('Save & Exit');
            $('#appointmentDetail #btnAppointmentHistory').attr("disabled", false);
        }
        else {
            $('#appointmentDetail #btnUnallocatedCopayment').attr("disabled", "disabled");
            $('#appointmentDetail #btnAppointmentHistory').attr("disabled", true);
        }

        var self = $('#appointmentpanel');
        self.loadDropDowns(true).done(function () {

            $('#appointmentDetail #hfSlottimeid').val(appointmentDetail.params.SlotId);
            $('#appointmentDetail #hfSlottimedtlid').val(appointmentDetail.params.SlotdetailId);

            $('#appointmentDetail #txtFromTime').val(appointmentDetail.params.Time);
            $('#appointmentDetail #txtToTime').val(appointmentDetail.params.ToTime);

            $('#appointmentDetail #hfProviderId').val(appointmentDetail.params.ProviderId);
            $('#appointmentDetail #hfProviderName').val(appointmentDetail.params.ProviderName);
            $('#appointmentDetail #hfFacilityId').val(appointmentDetail.params.FacilityId);
            $('#appointmentDetail #hfFacilityName').val(appointmentDetail.params.FacilityName);
            $('#appointmentDetail #hfResourceId').val(appointmentDetail.params.ResourceId);
            $('#appointmentDetail #hfResourceName').val(appointmentDetail.params.ResourceName);
            $('#appointmentDetail #hfSchReasonId').val(appointmentDetail.params.ScheduleReasonId);
            $('#appointmentDetail #hfSlotDuration').val(appointmentDetail.params.SlotMinutes);

            $('#' + appointmentDetail.params.PanelID + ' #txtFromTime').timepicker().on('changeTime.timepicker', function (e) {
                disableFocus: false
            });
            if (appointmentDetail.params.PanelID == "schEditSlot" || appointmentDetail.params.PanelID == "pnlEncounterChargeCapture") {
                $("#actionPanSchEditSlot #frmappointmentDetail #ddlStatus").attr("disabled", "disabled");
            }
            if (appointmentDetail.params.ProviderId)
            { appointmentDetail.VisitTypeDropdownLoad(); }
            appointmentDetail.ProviderSearch(appointmentDetail.params.ProviderId);
            //appointmentDetail.LoadAllAutocomplete();
           
            appointmentDetail.ValidateAppointment();
            if (appointmentDetail.params.mode == "Add") {
                appointmentDetail.FillDDLAppointmentStatus();
                //$('#' + appointmentDetail.params.PanelID + ' #frmappointmentDetail #lnkFacility').prop('disabled', 'disabled')
            }
            $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
        });
        if (appointmentDetail.params["ParentCtrl"] == "schTabCalendar") {
            if (!appointmentDetail.params.ResourceId) {
                //$('#appointmentDetail #frmappointmentDetail #txtProvider').attr("disabled", true);
                //$('#appointmentDetail #frmappointmentDetail  #lnkProviderEdit').attr("disabled", true);
            }
            else {
                $('#appointmentDetail #frmappointmentDetail #txtProvider').attr("disabled", false);
                $('#appointmentDetail #frmappointmentDetail  #lnkProviderEdit').attr("disabled", false);
            }
        }
        if (appointmentDetail.params["ParentCtrl"] == "schTabMultipleView" && appointmentDetail.params["ResourceId"]) {

            $('#appointmentDetail #frmappointmentDetail #txtProvider').attr("disabled", false);
            $('#appointmentDetail #frmappointmentDetail  #lnkProviderEdit').attr("disabled", false);
        }
        else if (appointmentDetail.params["ParentCtrl"] == "schTabMultipleView") {
            $('#appointmentDetail #frmappointmentDetail #txtProvider').attr("disabled", true);
            $('#appointmentDetail #frmappointmentDetail  #lnkProviderEdit').attr("disabled", true);
        }
        $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
        appointmentDetail.BindPatientName();
        appointmentDetail.BindPatientAccount();
        appointmentDetail.BindProvider();
        appointmentDetail.BindFacility();
        appointmentDetail.BindRefProvider();
    },

    LoadSlotDetail: function () {

        $('#appointmentDetail #txtProvider').val(appointmentDetail.params.ProviderName);
        utility.SetKendoAutoCompleteSourceforValidate($('#appointmentDetail #txtProvider'), $('#appointmentDetail #txtProvider').val(), $('#appointmentDetail #hfProviderId'), $('#appointmentDetail #hfProviderId').val());
        //$('#appointmentDetail #txtFacility').val(appointmentDetail.params.FacilityName);
        if (appointmentDetail.params.ResourceName != 'null')
            $('#appointmentDetail #txtResource').val(appointmentDetail.params.ResourceName);

        //if (appointmentDetail.params.ScheduleReasonId != "")
        //    $("#appointmentDetail #ddlReason option[value=" + appointmentDetail.params.ScheduleReasonId + "]").attr('selected', 'selected');
        //if (appointmentDetail.params.ScheduleReason != 'undefined') {
        //    $('#appointmentDetail #txtSchReason').val(appointmentDetail.params.ScheduleReason);
        //}
        //if (appointmentDetail.params.ScheduleReasonId != null) {
        //    $('#appointmentDetail #hfSchReasonId').val(appointmentDetail.params.ScheduleReasonId);
        //}
        if (appointmentDetail.params.ScheduleReason != 'undefined') {
            $('#appointmentDetail #txtReasonComments').val(appointmentDetail.params.ScheduleReason);
        }

    },
    changeICDField: function () {

        if ($('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #radioFreetext").prop("checked") == true) {
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtComplaintsICD").addClass('hidden');
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #btnSearchCPT").addClass('hidden');
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtReasonComments").removeClass('hidden');
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtReasonComments").val('');
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtComplaintsICD").val('');
        } else {
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtComplaintsICD").removeClass('hidden');
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #btnSearchCPT").removeClass('hidden');
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtReasonComments").addClass('hidden');
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtReasonComments").val('');
            $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtComplaintsICD").val('');
        }
    },
    OpenSearchPopup: function (ctrl, HiddenCtrl) {
        var params = [];
        params["FromAdmin"] = "0";
        params["RefCtrl"] = ctrl;
        params['fromIcon'] = true;
        params["RefHiddenCtrl"] = HiddenCtrl;
        params["ParentCtrl"] = "appointmentDetail";
        LoadActionPan('Admin_IMOICD', params);
    },
    bindICD9AutoComplete: function (element) {
        var ctrl = $(element);
        if ($(element).val().length > 3)
            if ($(element).val().substring(0, 3).toLowerCase() == "sct")
                appointmentDetail.LastSctBaseSearch = $(element).val().substring(3, $(element).val().length);
            else
                appointmentDetail.LastSctBaseSearch = "";
        else
            appointmentDetail.LastSctBaseSearch = "";
        utility.BindIMOAutoCompleteText(element, 'COMMON_IMO_CODE', 'GET_IMO_ICDCODE', ctrl, null, true, -1, 'ICD', true, 'appointmentDetail', null, false);
    },
    LoadAllAutocomplete: function () {

        //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
        //    $("input#txtRefProvider").autocomplete({
        //        autoFocus: true,
        //        minLength: 3,  //added min length, Abdur Rehman Latif - PMS 2986
        //        source: RefProviders, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                $("#appointmentDetail #hfRefProvider").val(ui.item.id);
        //                if ($("#appointmentDetail #lnkRefProviderEdit").css("display") == "none") {
        //                    $("#appointmentDetail #lnkRefProviderEdit").css("display", "inline");
        //                    $("#appointmentDetail #lblRefProvider").css("display", "none");
        //                }
        //            }, 100);
        //        }
        //    });
        //});
        //CacheManager.BindCodes('GetProvider', false).done(function (result) {
        //    $("#appointmentDetail #frmappointmentDetail input#txtProvider").autocomplete({
        //        autoFocus: true,
        //        source: Providers, // pass an array
        //        select: function (event, ui) {
        //            setTimeout(function () {
        //                //if ($("#frmappointmentDetail #lnkProviderEdit").css("display") == "none") {
        //                //    $("#frmappointmentDetail #lnkProviderEdit").css("display", "");
        //                //    $("#frmappointmentDetail #lblProvider").css("display", "none");
        //                //}
        //                $("#frmappointmentDetail #txtProvider").attr("ProviderId", ui.item.id); // add the selected id
        //                $("#frmappointmentDetail #hfProviderId").val(ui.item.id);
        //                if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {
        //                    appointmentDetail.OpenPatReferralSearch($('#appointmentDetail #ddlInsurancePlan').val());
        //                }
        //            }, 100);
        //        }
        //    });
        //});

    },

    LoadAppointment: function () {
        BackgroundLoaderShow(true);
        if (appointmentDetail.params.mode == "Add") {


            if (appointmentDetail.params.ParentCtrl == "schwaitlistdetail") {

                $('#appointmentDetail #txtAccountNo').attr("disabled", "disabled");
                $('#appointmentDetail #txtFullName').attr("disabled", "disabled");
                $('#appointmentDetail #lnkPatientAccount').attr("disabled", "disabled");
                $('#appointmentDetail #lnkPatientName').attr("disabled", "disabled");
                //$('#appointmentDetail #ddlReason').attr("disabled", "disabled");
                //$('#appointmentDetail #Duration').attr("disabled", "disabled");
                $('#appointmentDetail #hfWaitListId').val(appointmentDetail.params.WaitListId);

                $('.recpatt').addClass('disableAll');
               // appointmentDetail.ProviderSearch(appointmentDetail.params.ProviderId);
                appointmentDetail.FillPatientAccount(appointmentDetail.params.PatientId);
            }
            if (appointmentDetail.params.ParentCtrl == "schcheckout")
                appointmentDetail.FillPatientAccount(appointmentDetail.params.PatientId);


          //  appointmentDetail.ProviderSearch(appointmentDetail.params.ProviderId);
            var AccountNo = $("#PatientProfile #hfAccountNo").val();


            if (appointmentDetail.params.ParentCtrl == "mstrTabDashBoard") {
                AccountNo = appointmentDetail.params.AccountNumber;
                appointmentDetail.FillPatientAccount(appointmentDetail.params.PatientId);
                $('#appointmentDetail #txtComments').val(appointmentDetail.params.CommentBox);
                $('#appointmentDetail #isRequestFromPortal').val("1");
                $('#appointmentDetail #PortalAppRequestId').val(appointmentDetail.params.PortalAppRequestId);
                $('#appointmentDetail #hfPracticeId').val(appointmentDetail.params.PracticeId);
                $('#appointmentDetail #hfPortalSchDate').val(appointmentDetail.params.PortalSchDate);
                $('#appointmentDetail #divPatNameAccount').addClass('disableAll');
                $('#appointmentDetail #hfPatientFirstLastName').val(appointmentDetail.params.PatientName);
            } else {
                if (AccountNo.length > 0 && appointmentDetail.params.ParentCtrl != 'schwaitlistdetail') {
                    $('#appointmentDetail #txtAccountNo').val(AccountNo);
                    appointmentDetail.FillPatientAccount($("#PatientProfile #hfPatientId").val());
                }
            }

            $('#appointmentDetail #headingTitle').html('Add Appointment');
            appointmentDetail.FillDuration('#frmappointmentDetail #hfSchReasonId', 0);
            appointmentDetail.LoadSlotDetail();

            //$("#appointmentDetail #ddlStatus option:selected").text('Schedule');


            $('#appointmentDetail #btnCopayment').attr("disabled", true);


            $('#appointmentDetail #txtScheduleDate').val(appointmentDetail.params.ScheduleDate.replace(/ [0-9]*[0-9]:[0-9][0-9]:[0-9][0-9] A*P*M/, ''));

            //$('#appointmentDetail #btnCopayment').hide();
            //$("#appointmentDetail #btnCopayment").toggleClass("hide");


            if (Scheduling_Calendar.disableAppointmentDate || appointmentDetail.params.ParentCtrl == "schwaitlistdetail" || appointmentDetail.params.ParentCtrl == "schcheckout") {
                $('#appointmentDetail #txtScheduleDate').attr("disabled", true);
            } else {
                if (!(appointmentDetail.params.ParentCtrl == "schwaitlistdetail")) {
                    utility.CreateDatePicker("appointmentDetail #frmappointmentDetail #txtScheduleDate", function () {
                    }, true);
                }
            }
            appointmentDetail.AddTime('#appointmentDetail #Duration');
            //serialize Data.
            $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
        }
        else if (appointmentDetail.params.mode == "Edit") {

            $('#appointmentDetail #txtAccountNo').attr("disabled", "disabled");
            $('#appointmentDetail #txtFullName').attr("disabled", "disabled");
            $('#appointmentDetail #lnkPatientAccount').attr("disabled", "disabled");
            $('#appointmentDetail #lnkPatientName').attr("disabled", "disabled");
            if (appointmentDetail.params.ParentCtrl == "schwaitlistdetail") {

                $('.recpatt').addClass('disableAll');
            }
            // BackgroundLoaderShow(true);
            appointmentDetail.FillAppointment(appointmentDetail.params.AppointmentId).done(function (response) {
                if (response.status != false) {

                    //$('#appointmentDetail #headingTitle').html('Edit Appointment');
                    var appointment_detail = JSON.parse(response.AppointmentFill_JSON);



                    if (appointment_detail.txtFromTime == "") {
                        $('#appointmentDetail #totimefrmtime').css("display", "none");
                    }

                    var appInsuranceId = appointment_detail.ddlInsurancePlan;
                    $('#appointmentDetail #hfSlottimeid').val(appointmentDetail.params.SlotId);
                    $('#appointmentDetail #hfSlottimedtlid').val(appointmentDetail.params.SlotdetailId);

                    $('#appointmentDetail #hfProviderId').val(appointmentDetail.params.ProviderId);
                    $('#appointmentDetail #hfProviderName').val(appointmentDetail.params.ProviderName);
                    $('#appointmentDetail #hfFacilityId').val(appointmentDetail.params.FacilityId);
                    $('#appointmentDetail #hfFacilityName').val(appointmentDetail.params.FacilityName);
                    $('#appointmentDetail #hfResourceId').val(appointmentDetail.params.ResourceId);
                    $('#appointmentDetail #hfResourceName').val(appointmentDetail.params.ResourceName);
                    $('#appointmentDetail #hfSchReasonId').val(appointmentDetail.params.ScheduleReasonId);
                    $('#appointmentDetail #hfReferralId').val(appointment_detail.ReferralId);
                    $("#appointmentDetail #hfAppointmentDate").val(appointment_detail.hfAppointmentDate);
                    $("#appointmentDetail #txtScheduleDate").val(appointment_detail.txtScheduleDate);

                    //$('#appointmentDetail #hfSlotDuration').val(appointment_detail.Duration);

                    if (appointment_detail.ReferralId != undefined && appointment_detail.ReferralId != null && appointment_detail.ReferralId != '') {
                        $('#appointmentDetail #btnAddReferral').text('View Referral');
                    }

                    $('#appointmentDetail #btnCopayment').attr("disabled", false);
                    //$('#appointmentDetail #btnCopayment').show();
                    //$("#appointmentDetail #btnCopayment").toggleClass("show");

                    if (appointmentDetail.params.ResourceId) {

                        $("#appointmentDetail #btnCopayment").attr("disabled", true);
                    }

                    if (appointmentDetail.params.checkin == 1) {
                        //$('#appointmentDetail #btnsave').hide();
                        //$("#appointmentDetail #btnsave").toggleClass("hide");
                        //$('#appointmentDetail #btnsave').attr("disabled", true);
                        //$("#appointmentpanel :input").attr("disabled", "disabled");
                    }
                    if (appointmentDetail.params.checkin == 2) {

                        //$('#appointmentDetail #btnsave').hide();
                        //$('#appointmentDetail #headingTitle').html('View Appointment');
                        //$("#appointmentDetail #btnsave").toggleClass("hide");
                        $('#appointmentDetail #btnsave').attr("disabled", true);
                        $("#appointmentpanel :input").attr("disabled", "disabled");
                    }
                    if (appointment_detail.rdSpecialist == 'True')
                        $('#appointmentDetail #rdSpecialist').attr("checked", "checked");
                    else
                        $('#appointmentDetail #rdPCP').attr("checked", "checked");

                    if (appointment_detail.ddlPatientType != "") {
                        $("#appointmentDetail #ddlPatientType option[value=" + appointment_detail.ddlPatientType + "]").attr('selected', 'selected').trigger('change');;
                        //appointmentDetail.LoadVisitType(appointment_detail.ddlPatientType);
                    }
                    if (appointment_detail.ddlVisitType != "") {
                        $("#appointmentDetail #ddlVisitType option[value=" + appointment_detail.ddlVisitType + "]").attr('selected', 'selected');
                    }



                    appointmentDetail.FillPatientAccount(appointment_detail.hfpatientid, appInsuranceId).done(function (results) {
                        if (appointmentDetail.params.checkin == 2 || appointmentDetail.params.checkin == 1) {

                            if (appointment_detail.AppointmentStatus.toUpperCase() == "CHECK OUT") {
                                $("#appointmentpanel :input").attr("disabled", "disabled");
                                $("#appointmentpanel #txtReferralNo").attr("disabled", "disabled");
                                $("#appointmentDetail #lnkInsurancePlanEdit").hide();
                                $("#appointmentDetail #lblInsurancePlan").show();
                            } else if (appointment_detail.AppointmentStatus.toUpperCase() == "CHECK IN" && appointmentDetail.params.isNoteCreated == false) {
                                $("#appointmentpanel :input:not(#txtComments,#btnsave,#ddlPatientType,#ddlVisitType,#txtReasonComments)").attr("disabled", "disabled");
                                $("#appointmentpanel #CoPayment").attr("disabled", "disabled");
                                $("#appointmentpanel #txtReferralNo").attr("disabled", "disabled");
                            } else if (appointment_detail.AppointmentStatus.toUpperCase() == "CHECK IN" && appointmentDetail.params.isNoteCreated == true) {
                                $("#appointmentpanel :input:not(#txtComments,#btnsave)").attr("disabled", "disabled");
                                $("#appointmentpanel #CoPayment").attr("disabled", "disabled");
                                $("#appointmentpanel #txtReferralNo").attr("disabled", "disabled");
                            }

                            $("#appointmentDetail #ddlStatus option").filter(function () {
                                return $(this).text().trim().toUpperCase() == 'CHECK IN';
                            }).prop('disabled', false);

                            $("#appointmentDetail #ddlStatus option").filter(function () {
                                return $(this).text().trim().toUpperCase() == 'CHECK OUT';
                            }).prop('disabled', false);
                        }


                        var self = $("#appointmentDetail");
                        if (appointmentDetail.params.ParentCtrl == "Scheduling_RescheduleSearch") {

                            if (appointmentDetail.params.isResource) {
                                appointment_detail.hfResourceId = appointmentDetail.params.ResourceId;
                                appointment_detail.txtResource = appointmentDetail.params.ResourceName;
                            }
                            else {
                                appointment_detail.hfProviderId = appointmentDetail.params.ProviderId;
                                appointment_detail.txtProvider = appointmentDetail.params.ProviderName;

                            }

                            appointment_detail.hfFacilityId = appointmentDetail.params.FacilityId;
                            appointment_detail.txtFacility = appointmentDetail.params.FacilityName;

                        }
                        utility.bindMyJSON(true, appointment_detail, false, self).done(function () {

                            if (appointment_detail.ReasonCommentType == "ICD") {
                                $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #radioProblem").prop("checked", true);
                                $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #radioProblem").trigger("change");
                                $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtReasonComments").val('');
                                $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtComplaintsICD").val(appointment_detail.txtReasonComments);
                            }
                            else {
                                $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #radioFreetext").prop("checked", true);
                                $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #radioFreetext").trigger("change");
                                $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtReasonComments").val(appointment_detail.txtReasonComments);
                                $('#' + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtComplaintsICD").val("");
                            }
                            if (appointment_detail.AppointmentStatus.toUpperCase() == "CANCEL") {
                                $('#appointmentDetail #CancelReason').show();
                                $('#appointmentDetail #CancelReason').find('a').attr('onclick', "appointmentDetail.LoadCancellationReason(" + appointmentDetail.params.AppointmentId + "," + appointment_detail.ddlStatus + ", true);");
                                $('#appointmentDetail #txtCancellationReason').val(appointment_detail.CancellationReason);
                            }
                            if (appointment_detail.hfRefProvider != "") {
                                if ($("#appointmentDetail #lnkRefProviderEdit").css("display") == "none") {
                                    $("#appointmentDetail #lnkRefProviderEdit").css("display", "inline");
                                    $("#appointmentDetail #lblRefProvider").css("display", "none");
                                }
                            }
                            //  BackgroundLoaderShow(false);

                            utility.SetKendoAutoCompleteSourceforValidate($('#appointmentDetail #txtProvider'), $('#appointmentDetail #txtProvider').val(), $('#appointmentDetail #hfProviderId'), $('#appointmentDetail #hfProviderId').val());
                            utility.SetKendoAutoCompleteSourceforValidate($("#appointmentDetail #txtFacility"), $("#appointmentDetail #txtFacility").val(), $("#appointmentDetail #hfFacilityId"), $("#appointmentDetail #hfFacilityId").val());
                            //internalize Referral Number
                            appointmentDetail.FillReferralNumber();
                            //serialize Data.
                            if ($("#frmappointmentDetail #ddlInsurancePlan").val() == "") {
                                $('#CoPayment').prop("disabled", true);
                            } else {
                                $('#CoPayment').prop("disabled", false);
                            }
                            if (appointmentDetail.params.checkin == "1") {
                                $('#CoPayment').prop("disabled", true);
                            }

                            // Start Reschedule Appointment

                            if (appointmentDetail.params.PanelID == "pnlRescheduleSearch") {
                                $('#appointmentDetail #txtFromTime').val(appointmentDetail.params.Time);
                                $('#appointmentDetail #txtToTime').val(appointmentDetail.params.ToTime);
                                $('#appointmentDetail #txtScheduleDate').val(appointmentDetail.params.ScheduleDate);
                                //$("#appointmentDetail #ddlStatus").removeAttr("selected");
                                //$("#appointmentDetail #ddlStatus option").filter(function () {
                                //    return $(this).text().trim().toUpperCase() == 'SCHEDULED';
                                //}).attr('selected', 'selected');
                                var value = $("option").filter(function () {
                                    return $(this).text() === 'Scheduled';
                                }).first().attr("value");
                                $("#appointmentDetail #ddlStatus").val(value);
                                $('#appointmentDetail #Duration').trigger('blur');
                            }

                            // End Reschedule Appointment



                            $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                        });
                        $('#appointmentDetail #btnUnallocatedCopayment').attr("disabled", false);
                        if (appointmentDetail.params.PanelID && appointmentDetail.params.PanelID == "pnlEncounterChargeCapture") {
                            $('#appointmentDetail #btnCopayment').attr("disabled", false);
                            $('#appointmentDetail #btnAppointmentHistory').attr("disabled", false);
                        }
                        appointmentDetail.AddTime('#appointmentDetail #Duration');
                        //serialize Data.
                        $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                        if (
                            $('#appointmentDetail #frmappointmentDetail #chkcopayalert').is(":checked")
                            ||
                            ($("#appointmentDetail #frmappointmentDetail #CoPayment").val() && parseFloat($("#appointmentDetail #frmappointmentDetail #CoPayment").val()) > 0)
                                                   ) {
                            $('#appointmentDetail #frmappointmentDetail #chkcopayalert').removeAttr("disabled", "disabled");
                        }
                    });
                    $('#appointmentDetail #txtScheduleDate').attr("disabled", true);
                    //serialize Data.
                    $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                }

                else {
                    utility.DisplayMessages(response.Message, 3);
                    BackgroundLoaderShow(false);
                }

            });

        }
        $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
    },
    BindPatientAccount: function () {
        var valid = false;
        var Ctrl = $('#appointmentDetail #txtAccountNo');
        var hfCtrl = $("#appointmentDetail #frmappointmentDetail #hfpatientid");
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
            }

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
            }
            appointmentDetail.FillReferralNumber();
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            Ctrl.val(dataItem.AccountNumber);
            appointmentDetail.FillPatientAccount(dataItem.id);
            utility.InsertRecentPatient(dataItem.id);
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
                        utility.GetPatientArray(Ctrl.val(), 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },

    PatientAccountOnInput: function () {
        var AccountNo = $('#appointmentDetail #txtAccountNo').val();
        if (AccountNo == "") {
            $('#appointmentDetail #frmappointmentDetail #txtFullName,#dtpDOB,#txtPatientBalance,#txtInsuranceBalance,#txtAdvanceBalance,#txtAdvanceBalance,#txtRefProvider,#txtPriority,#txtReferralNo,#CoPayment').val('');
            $('#appointmentDetail #frmappointmentDetail #txtReferralNo').val('');
            $('#appointmentDetail #frmappointmentDetail #ddlInsurancePlan option').remove();
            if ($("#appointmentDetail #lnkRefProviderEdit").css("display") == "inline") {
                $("#appointmentDetail #lnkRefProviderEdit").css("display", "none");
                $("#appointmentDetail #lblRefProvider").css("display", "inline");
            }
            if ($("#appointmentDetail #lnkReferralNumberEdit").css("display") == "inline") {
                $("#appointmentDetail #lnkReferralNumberEdit").css("display", "none");
                $("#appointmentDetail #lblReferralNumber").css("display", "inline");
            }

            $("#appointmentDetail #btnAddReferral").addClass("disableAll");
        }
    },

    BindPatientName: function () {
        var valid = false;
        var Ctrl = $("#appointmentDetail #txtFullName");
        var hfCtrl = $("#appointmentDetail #frmappointmentDetail #hfpatientid");
        var onChange = function () {
            var id_;
            var value_;
            var link = $(Ctrl).parent().parent().prev('a');
            var data = this.dataSource.data();
            var haveObject = data.filter(function (obj) {
                if ((obj.value && obj.value.toLowerCase() == $(Ctrl).val().toLowerCase()) || (obj.FullName && obj.FullName.toLowerCase() == $(Ctrl).val().toLowerCase())) {
                    id_ = obj.id;
                    value_ = obj.FullName;
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

            else {
                if (hfCtrl)
                    $(hfCtrl).val('');
                this.value('');
                $(link).hide();
                $(link).prev().show();
            }
            appointmentDetail.FillReferralPatientName();
        };
        var onSelect = function (e) {
            var dataItem = this.dataItem(e.item.index());
            Ctrl.val(dataItem.FullName);
            appointmentDetail.FillPatientAccount(dataItem.id);
            utility.InsertRecentPatient(dataItem.id);
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
                        utility.GetPatientArrayByName(Ctrl.val(), 1, 0, 1).done(function (response) {
                            e.success(response);
                        });
                    },
                }
            },
        });
    },

    PatientNameOnInput: function () {
        var AccountNo = $('#appointmentDetail #txtFullName').val();
        if (AccountNo == "") {
            $('#appointmentDetail #frmappointmentDetail #txtFullName,#dtpDOB,#txtPatientBalance,#txtInsuranceBalance,#txtAdvanceBalance,#txtAdvanceBalance,#txtRefProvider,#txtPriority,#txtReferralNo,#CoPayment').val('');
            $('#appointmentDetail #frmappointmentDetail #txtReferralNo').val('');
            $('#appointmentDetail #frmappointmentDetail #ddlInsurancePlan option').remove();
            if ($("#appointmentDetail #lnkRefProviderEdit").css("display") == "inline") {
                $("#appointmentDetail #lnkRefProviderEdit").css("display", "none");
                $("#appointmentDetail #lblRefProvider").css("display", "inline");
            }
            if ($("#appointmentDetail #lnkReferralNumberEdit").css("display") == "inline") {
                $("#appointmentDetail #lnkReferralNumberEdit").css("display", "none");
                $("#appointmentDetail #lblReferralNumber").css("display", "inline");
            }

            $("#appointmentDetail #btnAddReferral").addClass("disableAll");
        }
    },

    FillDDLAppointmentStatus: function () {


        //CacheManager.BindCodes('GetAppointmentStatus', false).done(function (result) {
        //    for (var i = 0; i < result.length; i++) {
        //        if (result[i].Name == "Scheduled" || result[i].Name == "SCHEDULED") {

        //            $('#appointmentDetail #ddlStatus').val(result[i].Value);
        //        }

        //        $("#appointmentDetail #ddlStatus option").filter(function () {
        //            return $(this).text().trim().toUpperCase() == 'CHECK IN';
        //        }).prop('disabled', true);

        //        $("#appointmentDetail #ddlStatus option").filter(function () {
        //            return $(this).text().trim().toUpperCase() == 'CHECK OUT';
        //        }).prop('disabled', true);

        //    }

        //    //serialize Data.
        //    $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
        //});

        $("#appointmentDetail #ddlStatus option").filter(function () {
            return $(this).text().trim().toUpperCase() == 'CHECK IN';
        }).prop('disabled', true);

        $("#appointmentDetail #ddlStatus option").filter(function () {
            return $(this).text().trim().toUpperCase() == 'CHECK OUT';
        }).prop('disabled', true);

        $("#appointmentDetail #ddlStatus option").filter(function () {
            return $(this).text().trim().toUpperCase() == 'SCHEDULED';
        }).attr('selected', 'selected');

        $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());

    },

    LoadInsurancePlans: function () {
        var PatientId = $("#appointmentDetail #frmappointmentDetail #hfpatientid").val();
        appointmentDetail.searchPatientInsurance(PatientId).done(function (response) {
            if (response.status != false) {
                var appInsuranceId = $('#appointmentDetail #ddlInsurancePlan').val();
                if (appointmentDetail.params.InsurancePlanLinkedId > 0) {
                    appInsuranceId = appointmentDetail.params.InsurancePlanLinkedId;
                    appointmentDetail.params.InsurancePlanLinkedId = 0;
                }
                appointmentDetail.BindInsurancePlans(response.PatientInsuranceLoad_JSON, appInsuranceId);
            } else {
                utility.DisplayMessages(response.Message, 3);
            }
        })
    },

    BindInsurancePlans: function (InsuranceJSON, appInsuranceId) {
        //var InsuranceJSON_detail = JSON.parse(InsuranceJSON);
        $("#appointmentDetail #lnkInsurancePlanEdit").show();
        $("#appointmentDetail #lblInsurancePlan").hide();
        var InsuranceJSON_detail = $.grep(JSON.parse(InsuranceJSON), function (obj) {
            if (obj.IsActive.toLowerCase() == 'true') {
                return obj;
            }
        });
        //var PatientDetailJSON_detail = JSON.parse(PatientDetailJSON);
        $("#frmappointmentDetail #ddlInsurancePlan").empty();
        $("#frmappointmentDetail #ddlInsurancePlan").append($('<option/>', {
            value: "",
            html: "- SELECT -",
            priority: "",
            coPayment: "",
            specialistCopay: ""
        }));
        $.each(InsuranceJSON_detail, function (i, item) {
            if (item.IsActive == "True") {
                $("#frmappointmentDetail #ddlInsurancePlan").append(
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
                if (appointmentDetail.params.mode == "Add") {
                    if ($('#appointmentDetail #rdPCP').is(':checked')) {
                        //if (InsuranceJSON_detail[i].InsuranceTypeName == "Primary") {
                        //    $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                        //    $('#appointmentDetail #txtPriority').val(InsuranceJSON_detail[i].PlanPriority);
                        //    $('#frmappointmentDetail #CoPayment').val(InsuranceJSON_detail[i].AmtCopay);

                        //}
                        //else if (InsuranceJSON_detail[i].PlanPriority == "1") {
                        if (appInsuranceId) {
                            if (appInsuranceId == InsuranceJSON_detail[i].InsuranceId) {
                                $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                                $('#appointmentDetail #txtPriority').val(InsuranceJSON_detail[i].PlanPriority);
                                if (InsuranceJSON_detail[i].AmtCopay != "")
                                    $('#frmappointmentDetail #CoPayment').val(parseFloat(InsuranceJSON_detail[i].AmtCopay).toFixed(2));
                            }
                        }
                        else {
                            $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[0].InsuranceId);
                            $('#appointmentDetail #txtPriority').val(InsuranceJSON_detail[0].PlanPriority);
                            if (InsuranceJSON_detail[0].AmtCopay != "")
                                $('#frmappointmentDetail #CoPayment').val(parseFloat(InsuranceJSON_detail[0].AmtCopay).toFixed(2));
                        }

                        if (parseFloat($("#frmappointmentDetail #CoPayment").val()) > 0) {
                            //Enabled Check Box 
                            $('#appointmentDetail #frmappointmentDetail #chkcopayalert').removeAttr("disabled", "disabled");
                        }

                        // }
                    }
                    else if ($('#appointmentDetail #rdSpecialist').is(':checked')) {
                        //if (InsuranceJSON_detail[i].InsuranceTypeName == "Primary") {
                        //    $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                        //    $('#appointmentDetail #txtPriority').val(InsuranceJSON_detail[i].PlanPriority);
                        //    $('#frmappointmentDetail #CoPayment').val(InsuranceJSON_detail[i].SpecialistCopay);

                        //}
                        //else if (InsuranceJSON_detail[i].PlanPriority == "1") {
                        if (appInsuranceId) {
                            if (appInsuranceId == InsuranceJSON_detail[i].InsuranceId) {
                                $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                                $('#appointmentDetail #txtPriority').val(InsuranceJSON_detail[i].PlanPriority);
                                if (InsuranceJSON_detail[i].SpecialistCopay != "") {
                                    $('#frmappointmentDetail #CoPayment').val(parseFloat(InsuranceJSON_detail[i].SpecialistCopay).toFixed(2));
                                }
                            }
                        }
                        else {
                            $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[0].InsuranceId);
                            $('#appointmentDetail #txtPriority').val(InsuranceJSON_detail[0].PlanPriority);
                            if (InsuranceJSON_detail[0].SpecialistCopay != "") {
                                $('#frmappointmentDetail #CoPayment').val(parseFloat(InsuranceJSON_detail[0].SpecialistCopay).toFixed(2));
                            }
                        }
                        //}

                        // Enabled Check Box 
                        if (parseFloat($("#frmappointmentDetail #CoPayment").val()) > 0) {
                            //Enabled Check Box 
                            $('#appointmentDetail #frmappointmentDetail #chkcopayalert').removeAttr("disabled", "disabled");
                        }

                    }
                    else {
                        if (appInsuranceId) {
                            if (appInsuranceId == InsuranceJSON_detail[i].InsuranceId) {
                                $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                            }
                        }
                        else {
                            $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[0].InsuranceId);
                        }
                    }
                    //appointmentDetail.OpenPatReferralSearch(InsuranceJSON_detail[0].InsuranceId);
                }
                if (appointmentDetail.params.mode == "Edit") {

                    if (appInsuranceId == InsuranceJSON_detail[i].InsuranceId) {

                        if ($('#appointmentDetail #rdSpecialist').is(':checked')) {
                            $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                            $('#appointmentDetail #txtPriority').val(InsuranceJSON_detail[i].PlanPriority);
                            if (InsuranceJSON_detail[i].SpecialistCopay != "")
                                $('#frmappointmentDetail #CoPayment').val(parseFloat(InsuranceJSON_detail[i].SpecialistCopay).toFixed(2));

                            if (parseFloat($("#frmappointmentDetail #CoPayment").val()) > 0) {
                                //Enabled Check Box 
                                $('#appointmentDetail #frmappointmentDetail #chkcopayalert').removeAttr("disabled", "disabled");
                            }
                        }

                        else if ($('#appointmentDetail #rdPCP').is(':checked')) {
                            $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                            $('#appointmentDetail #txtPriority').val(InsuranceJSON_detail[i].PlanPriority);
                            if (InsuranceJSON_detail[i].AmtCopay != "")
                                $('#frmappointmentDetail #CoPayment').val(parseFloat(InsuranceJSON_detail[i].AmtCopay).toFixed(2));

                            if (parseFloat($("#frmappointmentDetail #CoPayment").val()) > 0) {
                                //Enabled Check Box 
                                $('#appointmentDetail #frmappointmentDetail #chkcopayalert').removeAttr("disabled", "disabled");
                            }
                        }
                        else {
                            $('#appointmentDetail #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                        }

                    }

                }

            }
            if (appointmentDetail.params.checkin != "1") {
                appointmentDetail.OpenPatReferralSearch($('#appointmentDetail #ddlInsurancePlan').val());
            }

            if ($("#frmappointmentDetail #ddlInsurancePlan").val() == "") {
                $('#CoPayment').prop("disabled", true);
            } else {
                $('#CoPayment').prop("disabled", false);
            }

        } else {
            if ($("#frmappointmentDetail #ddlInsurancePlan").val() == "") {
                $('#CoPayment').prop("disabled", true);
            } else {
                $('#CoPayment').prop("disabled", false);
            }
            $('#frmappointmentDetail #CoPayment').val("");
            $('#appointmentDetail #txtPriority').val("");
        }


        //if (appointmentDetail.params.ScheduleReasonId != "")
        //    $("#appointmentDetail #ddlReason option[value=" + appointmentDetail.params.ScheduleReasonId + "]").attr('selected', 'selected');
        //if (appointmentDetail.params.ScheduleReason != 'undefined') {
        //    $('#appointmentDetail #txtSchReason').val(appointmentDetail.params.ScheduleReason);
        //}
        //if (appointmentDetail.params.ScheduleReasonId != null) {
        //    $('#appointmentDetail #hfSchReasonId').val(appointmentDetail.params.ScheduleReasonId);
        //}

        appointmentDetail.FillDDLAppointmentStatus();
        $('#frmappointmentDetail').data('bootstrapValidator').enableFieldValidators('Duration', true);
    },

    OpenCoPayment: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["AppointmentId"] = appointmentDetail.params.AppointmentId;
        params["PatientVisitId"] = appointmentDetail.params.PatientVisitId;

        params["ProviderId"] = appointmentDetail.params.ProviderId;
        params["FacilityId"] = appointmentDetail.params.FacilityId;

        params["PatientId"] = $("#appointmentDetail #hfpatientid").val();
        params["ParentCtrl"] = 'appointmentDetail';
        LoadActionPan('schcopayment', params);

    },

    CheckifCancelledAppointmentExists: function () {
        var strMessage = "";
        var self = $("#appointmentDetail");
        var myJSON = self.getMyJSON();
        var object = JSON.parse(myJSON);
        myJSON = JSON.stringify(object);
        return appointmentDetail.CheckCancelledAppointmentExists(myJSON);
    },
    SavePatientAppointmentDetail: function () {
        var strMessage = "";
        var self = $("#appointmentDetail");
        var myJSON = self.getMyJSON();
        var object = JSON.parse(myJSON);
        if ($('#appointmentDetail #ddlStatus option:selected').text().trim() == "Cancel") {
            if (appointmentDetail.CancellationReason == "" || appointmentDetail.CancellationReason == null) {
                appointmentDetail.LoadCancellationReason();
                return;
            }
            else {
                object["CancellationReason"] = appointmentDetail.CancellationReason;
            }
        }
        if (appointmentDetail.params.ParentCtrl == "Scheduling_RescheduleSearch") {
            object["IsReschedule"] = true;
        }
        if ($('#appointmentDetail #radioProblem').is(":checked")) {
            object["txtReasonComments"] = $('#appointmentDetail #txtComplaintsICD').val();
            object["ReasonCommentType"] = "ICD";
        }
        else if ($('#appointmentDetail #radioFreetext').is(":checked")) {
            object["txtReasonComments"] = $('#appointmentDetail #txtReasonComments').val();
            object["ReasonCommentType"] = "FreeText";
        }
        myJSON = JSON.stringify(object);
        var patternJSON;
        var $tab = $('#appointmentDetail #myTabContent'), $active = $tab.find('.tab-pane.active'), text = $active.find('p:hidden').text();
        //alert(text)
        if (text == 'Daily') {
            var self = $("#appointmentDetail #Daily");
            patternJSON = self.getMyJSON();
            var json2 = { "pattern": "Daily" };
            patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
        }
        else if (text == 'Weekly') {
            var self = $("#appointmentDetail #Weekly");
            patternJSON = self.getMyJSON();
            var json2 = { "pattern": "Weekly" };
            patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
            //appointmentDetail.ValidateAppointment();
        }
        else if (text == 'Monthly') {
            var self = $("#appointmentDetail #Monthly");
            patternJSON = self.getMyJSON();
            var json2 = { "pattern": "Monthly" };
            patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
        }

        patternJSON = patternJSON

        var j = { "name": "Daily" };
        JSON.stringify(j);
        if (appointmentDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Appointment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var result;
                    result = appointmentDetail.ValidateValues();
                    if (result != false) {
                        if ($('#hfIsPatientActive').val() == 1) {
                            appointmentDetail.SavePatientAppointment(myJSON, patternJSON).done(function (response) {
                                if (response.status != false) {
                                    utility.DisplayMessages(response.message, 1);
                                    appointmentDetail.LoadCurrentAppointmentInScheduler("Add", response.AppointmentId, response.AppointmentDetail, myJSON);
                                    UnloadActionPan(appointmentDetail.params["ParentCtrl"], "actionPanAppointmentDetail");
                                    if (appointmentDetail.params.PanelID == "schwaitlistdetail") {
                                        UnloadActionPan();
                                        Scheduling_WaitList.WaitListSearch(appointmentDetail.params.WaitListId);
                                    }

                                    if (appointmentDetail.params.ParentCtrl == "schcheckout") {
                                        var scheduler = $("#schcheckout #dvSlots").data("kendoScheduler");
                                        scheduler.dataSource.read();
                                        setTimeout(function () {
                                            UnloadActionPan();
                                        }, 200);
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
        else if (appointmentDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var result;
                    result = appointmentDetail.ValidateValues();
                    if (result != false) {
                        appointmentDetail.UpdatePatientAppointment(myJSON, appointmentDetail.params.AppointmentId).done(function (response) {
                            if (response.status != false) {
                                utility.DisplayMessages(response.message, 1);
                                if (appointmentDetail.params.PanelID != "pnlEncounterChargeCapture")
                                    appointmentDetail.LoadCurrentAppointmentInScheduler(appointmentDetail.params.mode, appointmentDetail.params.AppointmentId, response.AppointmentDetail, myJSON);
                                UnloadActionPan(appointmentDetail.params["ParentCtrl"], "actionPanAppointmentDetail");
                                setTimeout(function () {
                                    if (appointmentDetail.params.ParentCtrl == "Scheduling_RescheduleSearch") {
                                        Scheduling_RescheduleSearch.UnLoad();
                                    }
                                }, 200);
                                //var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                ////expression for week range
                                //var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                                ////Month Regular Expression
                                //var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
                                //if (appointmentDetail.params.PanelID == "pnlScheduleMuliView") {
                                //    var providerid = appointmentDetail.params.ProviderId;
                                //    var facilityid = appointmentDetail.params.FacilityId;
                                //    var resourceid = appointmentDetail.params.ResourceId;
                                //    var date = appointmentDetail.params.DayDate;
                                //    var dateid = appointmentDetail.params.DateId;
                                //    Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);
                                //}
                                //else if (appointmentDetail.params.PanelID == "schEditSlot") {
                                //    schEditSlot.SelectSlotDetail(appointmentDetail.params.TimeslotDetailid, appointmentDetail.params.ProviderId, appointmentDetail.params.ResourceId);
                                //    if (appointmentDetail.params.MultipleView == 1) {
                                //        var providerid = appointmentDetail.params.ProviderId;
                                //        var facilityid = appointmentDetail.params.FacilityId;
                                //        var resourceid = appointmentDetail.params.ResourceId;
                                //        var date = appointmentDetail.params.DayDate;
                                //        var dateid = appointmentDetail.params.DateId;
                                //        Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);
                                //    }
                                //    else if (appointmentDetail.params.DayDate.match(weekrg) && appointmentDetail.params.DayDate.length > 15) {
                                //        var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                                //        Scheduling_Calendar.ClearTable();
                                //        var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                                //        var week = Scheduling_Calendar.GetWeek(date1);
                                //        $('#pnlScheduleCalendar #daydate span').html(week);
                                //        var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                                //        $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                                //    }
                                //    else {
                                //        var statusslots = Scheduling_Calendar.FilterCriteria();
                                //        Scheduling_Calendar.DayCalendar(appointmentDetail.params.ProviderId, appointmentDetail.params.ResourceId, appointmentDetail.params.FacilityId, appointmentDetail.params.DayDate, statusslots);
                                //        if (appointmentDetail.params.ProviderId != "")
                                //            $("#pnlScheduleCalendar #Provider option[value=" + appointmentDetail.params.ProviderId + "]").attr('selected', 'selected');
                                //        if (appointmentDetail.params.ResourceId != "")
                                //            $("#pnlScheduleCalendar #Resource option[value=" + appointmentDetail.params.ResourceId + "]").attr('selected', 'selected');
                                //        if (appointmentDetail.params.FacilityId != "")
                                //            $("#pnlScheduleCalendar #Facility option[value=" + appointmentDetail.params.FacilityId + "]").attr('selected', 'selected');
                                //        if (appointmentDetail.params.DayDate != "")
                                //            $('#pnlScheduleCalendar #daydate span').html(appointmentDetail.params.DayDate);
                                //    }
                                //} else if (appointmentDetail.params.ParentCtrl == "mstrTabDashBoard") {
                                //    DashBoard.DashBoardAppRequestSearch();
                                //} else if (appointmentDetail.params.ParentCtrl == "Scheduling_RescheduleSearch") {
                                //    Scheduling_RescheduleSearch.ScheduleSearch();
                                //}
                                //    //-------------------------- Start Reschedule Appointment
                                //else if (appointmentDetail.params.PanelID == "pnlRescheduleSearch") {
                                //    Scheduling_RescheduleAppointment.MoveAppointment(appointmentDetail.params.AppointmentId, appointmentDetail.params.SlotdetailId).done(function (response) {
                                //        if (response.status != false) {
                                //            Scheduling_RescheduleSearch.ScheduleSearch();
                                //            if (appointmentDetail.params.MultipleView == 1) {
                                //                var providerid = appointmentDetail.params.ProviderId;
                                //                var facilityid = appointmentDetail.params.FacilityId;
                                //                var resourceid = appointmentDetail.params.ResourceId;
                                //                var date = appointmentDetail.params.DayDate;
                                //                var dateid = appointmentDetail.params.DateId;
                                //                Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);
                                //            }
                                //            else if (appointmentDetail.params.DayDate.match(weekrg) && appointmentDetail.params.DayDate.length > 15) {
                                //                var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                                //                Scheduling_Calendar.ClearTable();
                                //                var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                                //                var week = Scheduling_Calendar.GetWeek(date1);
                                //                $('#pnlScheduleCalendar #daydate span').html(week);
                                //                var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                                //                $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                                //            }
                                //            else {
                                //                var statusslots = Scheduling_Calendar.FilterCriteria();
                                //                Scheduling_Calendar.DayCalendar(appointmentDetail.params.ProviderId, appointmentDetail.params.ResourceId, $("#pnlScheduleCalendar #Facility").val(), appointmentDetail.params.DayDate, statusslots);
                                //            }
                                //        } else {
                                //        }
                                //    });
                                //}
                                //    //-------------------------- End Reschedule Appointment
                                //else if (appointmentDetail.params.DayDate.match(weekrg) && appointmentDetail.params.DayDate.length > 15) {
                                //    var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                                //    Scheduling_Calendar.ClearTable();
                                //    var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                                //    var week = Scheduling_Calendar.GetWeek(date1);
                                //    $('#pnlScheduleCalendar #daydate span').html(week);
                                //    var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                                //    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                                //}
                                //else {
                                //    var statusslots = Scheduling_Calendar.FilterCriteria();
                                //    Scheduling_Calendar.DayCalendar(appointmentDetail.params.ProviderId, appointmentDetail.params.ResourceId, appointmentDetail.params.FacilityId, appointmentDetail.params.DayDate, statusslots);
                                //    if (appointmentDetail.params.ProviderId != "")
                                //        $("#pnlScheduleCalendar #Provider option[value=" + appointmentDetail.params.ProviderId + "]").attr('selected', 'selected');
                                //    if (appointmentDetail.params.ResourceId != "")
                                //        $("#pnlScheduleCalendar #Resource option[value=" + appointmentDetail.params.ResourceId + "]").attr('selected', 'selected');
                                //    if (appointmentDetail.params.FacilityId != "")
                                //        $("#pnlScheduleCalendar #Facility option[value=" + appointmentDetail.params.FacilityId + "]").attr('selected', 'selected');
                                //    if (appointmentDetail.params.DayDate != "")
                                //        $('#pnlScheduleCalendar #daydate span').html(appointmentDetail.params.DayDate);
                                //}
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
    LoadCancellationReason: function (appid, statusid, IsEdit) {
        var params = [];
        params["AppointmentId"] = appid;
        params["StatusId"] = statusid;
        params["IsEdit"] = IsEdit;
        params["ParentCtrl"] = "appointmentDetail";
        LoadActionPan('PMSScheduler_AppointmentCancellation', params);
    },
    LoadCurrentAppointmentInScheduler: function (mode, appointmentId, AppointmentDetail, myJSON) {
        myJSON = JSON.parse(myJSON);
        if (AppointmentDetail)
        { AppointmentDetail = JSON.parse(AppointmentDetail); }
        var facility_obj = $("#pnlPMSScheduler #facilityMultiselect").data("kendoMultiSelect").dataSource.data();
        var status_obj = $("#pnlPMSScheduler #statusSingleselect").data("kendoMultiSelect").dataSource.data();
        // var facility = facility_obj.filter(f => f.id == myJSON.hfFacilityId);
        var facility = facility_obj.filter(function (f) {
            return f.id == myJSON.hfFacilityId;
        });
        var status = "";
        if (status_obj) {
            //status = status_obj.filter(f => f.id == myJSON.ddlStatus)[0].color;
            var status = status_obj.filter(function (f) {
                return f.id == myJSON.ddlStatus;
            })[0].color;
        }

        if (mode == "Add") {
            var scheduler = $("#scheduler").data("kendoScheduler");
            scheduler.dataSource.read();
        } else {
            if (PMSScheduler.PreviousViewType == null || PMSScheduler.PreviousViewType != "month") {
                var scheduler = $("#scheduler").data("kendoScheduler");
                scheduler.dataSource.read();
                //var dataSourceApp = PMSScheduler.CanScheduler.dataSource;
                //var ap = dataSourceApp._data.filter(f => f.AppointmentId == appointmentId)[0];
                //ap.AmtCopay = myJSON.CoPayment;
                //ap.AppointmentDate = new Date(AppointmentDetail[0].AppointmentDate).toISOString().replace(".000Z", '');
                //ap.AppointmentDateFrom = new Date(AppointmentDetail[0].AppointmentDate + " " + AppointmentDetail[0].TimeFrom).toISOString().replace(".000Z", '');
                //ap.AppointmentDateTo = new Date(AppointmentDetail[0].AppointmentDate + " " + AppointmentDetail[0].TimeTo).toISOString().replace(".000Z", '');
                //ap.AppointmentId = appointmentId;
                //ap.AppointmentStatus = myJSON.ddlStatus_text;
                //ap.AppointmentStatusIds = myJSON.ddlStatus;
                //ap.CommandType = null;
                //ap.Comments = AppointmentDetail[0].Comments;
                ////ap.CopayBal= 0;
                ////ap.CopayClass= "Green";
                ////ap.EligibilityStatus= "";
                ////ap.EndDate=null;
                //ap.FacilityColor = facility[0].color;
                //ap.FacilityId = myJSON.hfFacilityId;
                ////ap.FacilityIds= null;
                //ap.FacilityName = AppointmentDetail[0].FacilityName;
                //ap.IsNonBilable = myJSON.chkNonBilable;
                ////ap.IsResourceSch= false;
                //ap.Month = new Date(AppointmentDetail[0].AppointmentDate).getMonth() + 1;
                //ap.PatientId = myJSON.hfpatientid;
                //ap.PatientName = AppointmentDetail[0].PatientName;
                //ap.PatientType = myJSON.ddlPatientType_text;
                //ap.PatientTypeId = myJSON.ddlPatientType;
                //ap.ProviderId = myJSON.hfProviderId;
                ////ap.ProviderIds =  null;
                //ap.ProviderName = AppointmentDetail[0].ProviderName;
                //ap.ReasonComments = "Reason Comments";
                ////ap.ResourceIds =  null;
                ////ap.ResurceIds =  null;
                //ap.SchStatusId = myJSON.ddlStatus;
                ////ap.SchViewType =  null;
                ////ap.StartDate =  null;
                //ap.StatusColor = status;
                //ap.TimeFrom = AppointmentDetail[0].TimeFrom;
                //ap.TimeTo = AppointmentDetail[0].TimeTo;
                ////ap.View =  null;
                //ap.VisitTypeId = myJSON.ddlVisitType;
                ////ap.VisitTypeIds =  null;
                //ap.VisitTypeName = myJSON.ddlVisitType_text;
                //ap.Year = new Date(AppointmentDetail[0].AppointmentDate).getFullYear();
                //ap.description = undefined;
                ////ap.dirty =  false;
                //ap.end = new Date(AppointmentDetail[0].AppointmentDate + " " + AppointmentDetail[0].TimeTo);
                ////ap.endTimezone =  undefined;
                ////ap.id =  appointmentId;
                ////ap.isAllDay =  undefined;
                ////ap.ownerId =  0;
                ////ap.recurrenceException =  undefined;
                ////ap.recurrenceRule =  undefined;
                //ap.start = new Date(AppointmentDetail[0].AppointmentDate + " " + AppointmentDetail[0].TimeFrom);
                ////ap.startTimezone =  undefined;
                ////ap.title = undefined;
                //PMSScheduler.CanScheduler.dataSource.pushUpdate(ap);
            }
        }
    },

    DeleteAppointmentRow: function (appointmentId) {
        var dataSourceApp = PMSScheduler.CanScheduler.dataSource;
        // var ap = dataSourceApp._data.filter(f => f.AppointmentId == appointmentId)[0];
        var ap = dataSourceApp._data.filter(function (f) {
            return f.AppointmentId == appointmentId;
        })[0];

        PMSScheduler.CanScheduler.dataSource.remove(ap);
        PMSScheduler.CanScheduler.dataSource.sync();
        PMSScheduler.ResetAppointmentCount();
    },

    SaveAppointmentDetail: function () {
        var strMessage = "";
        var self = $("#appointmentDetail");
        var myJSON = self.getMyJSON();
        var object = JSON.parse(myJSON);
        if ($('#appointmentDetail #radioProblem').is(":checked")) {
            object["txtReasonComments"] = $('#appointmentDetail #txtComplaintsICD').val();
            object["ReasonCommentType"] = "ICD";
        }
        else if ($('#appointmentDetail #radioFreetext').is(":checked")) {
            object["txtReasonComments"] = $('#appointmentDetail #txtReasonComments').val();
            object["ReasonCommentType"] = "FreeText";
        }
        myJSON = JSON.stringify(object);
        var patternJSON;
        var $tab = $('#appointmentDetail #myTabContent'), $active = $tab.find('.tab-pane.active'), text = $active.find('p:hidden').text();
        //alert(text)
        if (text == 'Daily') {
            var self = $("#appointmentDetail #Daily");
            patternJSON = self.getMyJSON();
            var json2 = { "pattern": "Daily" };
            patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
        }
        else if (text == 'Weekly') {
            var self = $("#appointmentDetail #Weekly");
            patternJSON = self.getMyJSON();
            var json2 = { "pattern": "Weekly" };
            patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
            //appointmentDetail.ValidateAppointment();
        }
        else if (text == 'Monthly') {
            var self = $("#appointmentDetail #Monthly");
            patternJSON = self.getMyJSON();
            var json2 = { "pattern": "Monthly" };
            patternJSON = JSON.stringify($.extend(false, {}, JSON.parse(patternJSON), json2));
        }

        patternJSON = patternJSON

        var j = { "name": "Daily" };
        JSON.stringify(j);
        if (appointmentDetail.params.mode == "Add") {
            AppPrivileges.GetFormPrivileges("Appointment", "ADD", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var result;
                    result = appointmentDetail.ValidateValues();
                    if (result != false) {
                        if ($('#hfIsPatientActive').val() == 1) {
                            appointmentDetail.SaveAppointment(myJSON, patternJSON).done(function (response) {
                                if (response.status != false) {
                                    //Admin_ProviderSchedule.ProviderScheduleSearch(response.ScheduleId);
                                    utility.DisplayMessages(response.message, 1);
                                    UnloadActionPan(appointmentDetail.params["ParentCtrl"], "actionPanAppointmentDetail");

                                    var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                    //expression for week range
                                    var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                                    //Month Regular Expression
                                    var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;


                                    if (appointmentDetail.params.PanelID == "schwaitlistdetail") {

                                        UnloadActionPan();
                                        Scheduling_WaitList.WaitListSearch(appointmentDetail.params.WaitListId);

                                    }

                                    else if (appointmentDetail.params.PanelID == "pnlScheduleMuliView") {

                                        var providerid = appointmentDetail.params.ProviderId;
                                        var facilityid = appointmentDetail.params.FacilityId;
                                        var resourceid = appointmentDetail.params.ResourceId;
                                        var date = appointmentDetail.params.DayDate;
                                        var dateid = appointmentDetail.params.DateId;

                                        Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);


                                    } else if (appointmentDetail.params.ParentCtrl == "mstrTabDashBoard") {
                                        DashBoard.DashBoardAppRequestSearch();
                                    }

                                    else if (appointmentDetail.params.DayDate.match(weekrg) && appointmentDetail.params.DayDate.length > 15) {
                                        var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                                        Scheduling_Calendar.ClearTable();

                                        var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                                        var week = Scheduling_Calendar.GetWeek(date1);
                                        $('#pnlScheduleCalendar #daydate span').html(week);
                                        //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", $('#daydate span').html(week));
                                        var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                                        $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                                    }
                                    else {
                                        var statusslots = Scheduling_Calendar.FilterCriteria();
                                        Scheduling_Calendar.DayCalendarbystatus(appointmentDetail.params.ProviderId, appointmentDetail.params.ResourceId, $('#pnlScheduleCalendar #Facility').val(), appointmentDetail.params.DayDate, appointmentDetail.params.Time);
                                        if (appointmentDetail.params.ProviderId != "")
                                            $("#pnlScheduleCalendar #Provider option[value=" + appointmentDetail.params.ProviderId + "]").attr('selected', 'selected');
                                        if (appointmentDetail.params.ResourceId != "")
                                            $("#pnlScheduleCalendar #Resource option[value=" + appointmentDetail.params.ResourceId + "]").attr('selected', 'selected');
                                        //if (appointmentDetail.params.FacilityId != "")
                                        //    $("#pnlScheduleCalendar #Facility option[value=" + appointmentDetail.params.FacilityId + "]").attr('selected', 'selected');
                                        if (appointmentDetail.params.DayDate != "")
                                            $('#pnlScheduleCalendar #daydate span').html(appointmentDetail.params.DayDate);

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
        else if (appointmentDetail.params.mode == "Edit") {
            AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
                if (strMessage == "") {
                    var result;
                    result = appointmentDetail.ValidateValues();
                    if (result != false) {
                        appointmentDetail.UpdateAppointment(myJSON, appointmentDetail.params.AppointmentId).done(function (response) {
                            if (response.status != false) {
                                //Admin_ProviderSchedule.ProviderScheduleSearch(appointmentDetail.params.AppointmentId);
                                utility.DisplayMessages(response.message, 1);
                                UnloadActionPan(appointmentDetail.params["ParentCtrl"], "actionPanAppointmentDetail");

                                var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                                //expression for week range
                                var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                                //Month Regular Expression
                                var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;
                                if (appointmentDetail.params.PanelID == "pnlScheduleMuliView") {

                                    var providerid = appointmentDetail.params.ProviderId;
                                    var facilityid = appointmentDetail.params.FacilityId;
                                    var resourceid = appointmentDetail.params.ResourceId;
                                    var date = appointmentDetail.params.DayDate;
                                    var dateid = appointmentDetail.params.DateId;

                                    Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);

                                }
                                else if (appointmentDetail.params.PanelID == "schEditSlot") {
                                    schEditSlot.SelectSlotDetail(appointmentDetail.params.TimeslotDetailid, appointmentDetail.params.ProviderId, appointmentDetail.params.ResourceId);

                                    if (appointmentDetail.params.MultipleView == 1) {

                                        var providerid = appointmentDetail.params.ProviderId;
                                        var facilityid = appointmentDetail.params.FacilityId;
                                        var resourceid = appointmentDetail.params.ResourceId;
                                        var date = appointmentDetail.params.DayDate;
                                        var dateid = appointmentDetail.params.DateId;

                                        Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);

                                    }

                                    else if (appointmentDetail.params.DayDate.match(weekrg) && appointmentDetail.params.DayDate.length > 15) {
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
                                        Scheduling_Calendar.DayCalendar(appointmentDetail.params.ProviderId, appointmentDetail.params.ResourceId, appointmentDetail.params.FacilityId, appointmentDetail.params.DayDate, statusslots);
                                        if (appointmentDetail.params.ProviderId != "")
                                            $("#pnlScheduleCalendar #Provider option[value=" + appointmentDetail.params.ProviderId + "]").attr('selected', 'selected');
                                        if (appointmentDetail.params.ResourceId != "")
                                            $("#pnlScheduleCalendar #Resource option[value=" + appointmentDetail.params.ResourceId + "]").attr('selected', 'selected');
                                        if (appointmentDetail.params.FacilityId != "")
                                            $("#pnlScheduleCalendar #Facility option[value=" + appointmentDetail.params.FacilityId + "]").attr('selected', 'selected');
                                        if (appointmentDetail.params.DayDate != "")
                                            $('#pnlScheduleCalendar #daydate span').html(appointmentDetail.params.DayDate);
                                    }
                                } else if (appointmentDetail.params.ParentCtrl == "mstrTabDashBoard") {
                                    DashBoard.DashBoardAppRequestSearch();
                                } else if (appointmentDetail.params.ParentCtrl == "Scheduling_RescheduleSearch") {
                                    Scheduling_RescheduleSearch.ScheduleSearch();
                                }
                                    //-------------------------- Start Reschedule Appointment

                                else if (appointmentDetail.params.PanelID == "pnlRescheduleSearch") {

                                    Scheduling_RescheduleAppointment.MoveAppointment(appointmentDetail.params.AppointmentId, appointmentDetail.params.SlotdetailId).done(function (response) {
                                        if (response.status != false) {
                                            Scheduling_RescheduleSearch.ScheduleSearch();
                                            if (appointmentDetail.params.MultipleView == 1) {

                                                var providerid = appointmentDetail.params.ProviderId;
                                                var facilityid = appointmentDetail.params.FacilityId;
                                                var resourceid = appointmentDetail.params.ResourceId;
                                                var date = appointmentDetail.params.DayDate;
                                                var dateid = appointmentDetail.params.DateId;

                                                Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);

                                            }
                                            else if (appointmentDetail.params.DayDate.match(weekrg) && appointmentDetail.params.DayDate.length > 15) {
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
                                                Scheduling_Calendar.DayCalendar(appointmentDetail.params.ProviderId, appointmentDetail.params.ResourceId, $("#pnlScheduleCalendar #Facility").val(), appointmentDetail.params.DayDate, statusslots);
                                                //if (appointmentDetail.params.ProviderId != "")
                                                //    $("#pnlScheduleCalendar #Provider option[value=" + appointmentDetail.params.ProviderId + "]").attr('selected', 'selected');
                                                //if (appointmentDetail.params.ResourceId != "")
                                                //    $("#pnlScheduleCalendar #Resource option[value=" + appointmentDetail.params.ResourceId + "]").attr('selected', 'selected');
                                                //if (appointmentDetail.params.FacilityId != "")
                                                //    $("#pnlScheduleCalendar #Facility option[value=" + appointmentDetail.params.FacilityId + "]").attr('selected', 'selected');
                                                //if (appointmentDetail.params.DayDate != "")
                                                //    $('#pnlScheduleCalendar #daydate span').html(appointmentDetail.params.DayDate);
                                            }
                                        } else {

                                        }
                                    });
                                }

                                    //-------------------------- End Reschedule Appointment
                                else if (appointmentDetail.params.DayDate.match(weekrg) && appointmentDetail.params.DayDate.length > 15) {
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
                                    Scheduling_Calendar.DayCalendar(appointmentDetail.params.ProviderId, appointmentDetail.params.ResourceId, appointmentDetail.params.FacilityId, appointmentDetail.params.DayDate, statusslots);
                                    if (appointmentDetail.params.ProviderId != "")
                                        $("#pnlScheduleCalendar #Provider option[value=" + appointmentDetail.params.ProviderId + "]").attr('selected', 'selected');
                                    if (appointmentDetail.params.ResourceId != "")
                                        $("#pnlScheduleCalendar #Resource option[value=" + appointmentDetail.params.ResourceId + "]").attr('selected', 'selected');
                                    if (appointmentDetail.params.FacilityId != "")
                                        $("#pnlScheduleCalendar #Facility option[value=" + appointmentDetail.params.FacilityId + "]").attr('selected', 'selected');
                                    if (appointmentDetail.params.DayDate != "")
                                        $('#pnlScheduleCalendar #daydate span').html(appointmentDetail.params.DayDate);
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
        var slot = appointmentDetail.params.SlotdetailId;
        var pro = appointmentDetail.params.ProviderId;
        var res = appointmentDetail.params.ResourceId;

        if (addCase == 0) {
            //schEditSlot.SelectSchSlotDetail(slot, pro, res).done(function (response) {

            //    if (response.status != false) {
            //var editslotdetail = JSON.parse(response.SchSlotDetailFill_JSON);
            //var SchSlot_JSON = JSON.parse(response.SchSlot_JSON);
            //var minutes = editslotdetail.txtMinutes;
            $('#appointmentDetail #Duration').val(appointmentDetail.params.SlotMinutes);
            if ($('#frmappointmentDetail').data('bootstrapValidator') != null && typeof $('#frmappointmentDetail').data('bootstrapValidator') != 'undefined') {
                $('#frmappointmentDetail').bootstrapValidator('revalidateField', 'Duration');
            }

            $('#appointmentDetail #Duration').trigger('change');

            $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
            //if (SchSlot_JSON != null) {
            //    $('#appointmentDetail #txtFromTime').val(SchSlot_JSON[0].FromTimeSlots);
            //    $('#appointmentDetail #txtToTime').val(SchSlot_JSON[0].ToTimeSlots);
            //}
            // }
            //    else {
            //        //utility.DisplayMessages(response.Message, 3);
            //    }
            //});
        }
        else if (addCase == 1) {
            appointmentDetail.FillScheduleReasonDuration(ScheduleReasonID).done(function (response) {
                if (response.status != false) {
                    var reasonduration = JSON.parse(response.ProviderScheduleFill_JSON);
                    $('#appointmentDetail #Duration').val(reasonduration.Duration);
                    if ($('#frmappointmentDetail').data('bootstrapValidator') != null && typeof $('#frmappointmentDetail').data('bootstrapValidator') != 'undefined') {
                        $('#frmappointmentDetail').bootstrapValidator('revalidateField', 'Duration');
                    }

                    $('#appointmentDetail #Duration').trigger('change');
                    //serialize Data.
                    $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());


                }
                else {

                }
            });
        }

    },

    FillInsuranceData: function (control, controlToChange) {

        var priority = $("#frmappointmentDetail #ddlInsurancePlan option:selected").attr("priority");
        $("#frmappointmentDetail #txtPriority").val(priority);

        //$("#appointmentDetail #txtReferralNo").val("");

        var specialistCopay = $("#frmappointmentDetail #ddlInsurancePlan option:selected").attr("specialistCopay");
        var coPayment = $("#frmappointmentDetail #ddlInsurancePlan option:selected").attr("coPayment");


        if ($('#appointmentDetail #rdPCP').is(':checked')) {

            if (coPayment != NaN && coPayment != "" && typeof (coPayment) != 'undefined') {

                // enable checkbox               
                $('#appointmentDetail #frmappointmentDetail #chkcopayalert').removeAttr("disabled", "disabled");
                $("#frmappointmentDetail #CoPayment").val(parseFloat(coPayment).toFixed(2));
                if (parseFloat($("#frmappointmentDetail #CoPayment").val()) == 0) {
                    //disable checkebox
                    $('#appointmentDetail #frmappointmentDetail #chkcopayalert').prop("checked", false);
                    $('#appointmentDetail #frmappointmentDetail #chkcopayalert').attr("disabled", "disabled");
                }
            }
            else
                $("#frmappointmentDetail #CoPayment").val('');
        }
        else if ($('#appointmentDetail #rdSpecialist').is(':checked')) {
            if (specialistCopay != NaN && specialistCopay != "" && typeof (specialistCopay) != 'undefined') {
                //  enable checkbox
                $('#appointmentDetail #frmappointmentDetail #chkcopayalert').removeAttr("disabled", "disabled");
                $("#frmappointmentDetail #CoPayment").val(parseFloat(specialistCopay).toFixed(2));
                if (parseFloat($("#frmappointmentDetail #CoPayment").val()) == 0) {
                    //disable checkebox
                    $('#appointmentDetail #frmappointmentDetail #chkcopayalert').prop("checked", false);
                    $('#appointmentDetail #frmappointmentDetail #chkcopayalert').attr("disabled", "disabled");
                }
            }
            else
                $("#frmappointmentDetail #CoPayment").val('');
        }
        if ($("#frmappointmentDetail #ddlInsurancePlan").val() == "") {
            $('#CoPayment').prop("disabled", true);
        } else {
            $('#CoPayment').prop("disabled", false);
        }

    },

    FillPatientInfoFromSearch: function (PatientId, event) {
        if (Patient_Search.params && Patient_Search.params.ParentCtrl && Patient_Search.params.ParentCtrl == "appointmentDetail") {
            if ($('#appointmentDetail #SchedulingFormDiv').hasClass("disableAll")) {
                $('#appointmentDetail #SchedulingFormDiv').removeClass("disableAll")
            }
        }
        if (event != null) {
            event.stopPropagation();
            if (event.target.type == "checkbox") {
                $(':checkbox', this).trigger('click');
                return;
            }
        }
        //EMR - 6446  by:MAHMAD
        utility.callbackAfterAllDOMLoaded(function () {
            UnloadActionPan("appointmentDetail", "Patient_Search");
        });
        //EMR - 6446  by:MAHMAD
        if ($("#appointmentDetail #hfpatientid").val() != "" || PatientId != null) {
            if (PatientId != null) {
                setTimeout(function () { appointmentDetail.FillPatientAccount(PatientId); }, 200);
                utility.InsertRecentPatient(PatientId);
            }
        }
        else {
            utility.DisplayMessages("No patient selected", 2);
        }
    },

    FillPatientAccount: function (PatientId, appInsuranceId) {

        var dfd = new $.Deferred();
        appointmentDetail.PatientInsuranceId = appInsuranceId;
        appointmentDetail.FillPatient(PatientId).done(function (response) {
            if (response.status != false) {
                Patient_Demographic.FillPatientAlertsCount('1', PatientId, 'appointmentDetail').done(function () {
                    if (PatientId != $('#PatientProfile #hfPatientId').val()) {
                        Patient_Demographic.isFinanicialAlert = true;
                    }
                    $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                });
                $('#appointmentDetail #txtRefProvider').val("");
                $('#appointmentDetail #txtReferralNo').val("");
                $("#appointmentDetail #lnkReferralNumberEdit").css("display", "none");
                $("#appointmentDetail #lblReferralNumber").css("display", "inline");

                var patient_detail = JSON.parse(response.PatientFill_JSON);
                var patient_balance = JSON.parse(response.PatientBalance_JSON);
                var patient = JSON.parse(response.Patient_JSON);
                var patient_insurance = JSON.parse(response.PatientInsurance_JSON);
                //var self = $("#appointmentDetail");
                //utility.bindMyJSON(true, patient_detail, false, self);
                if (patient_balance.length > 0) {
                    $('#appointmentDetail #txtPatientBalance').val(parseFloat(Number(patient_balance[0].PatientBalance)).toFixed(2));
                    $('#appointmentDetail #txtAdvanceBalance').val(parseFloat(Number(patient_balance[0].AdvanceBalance)).toFixed(2));
                    $('#appointmentDetail #txtInsuranceBalance').val(parseFloat(Number(patient_balance[0].InsuranceBalance)).toFixed(2));
                }

                $('#appointmentDetail #txtAccountNo').val(patient_detail.txtAccountNo);
                $('#appointmentDetail #txtFullName').val(patient_detail.txtFullName);
                $('#appointmentDetail #dtpDOB').val(patient_detail.dtpDOB);
                $('#appointmentDetail #hfpatientid').val(patient_detail.hfpatientid);
                $('#appointmentDetail #hfIsPatientActive').val(patient_detail.hfIsPatientActive);
                utility.SetKendoAutoCompleteSourceforValidate($('#appointmentDetail #txtFullName'), patient_detail.txtFullName, $('#appointmentDetail #hfpatientid'), patient_detail.hfpatientid, "FullName");
                utility.SetKendoAutoCompleteSourceforValidate($('#appointmentDetail #txtAccountNo'), patient_detail.txtAccountNo, $('#appointmentDetail #hfpatientid'), patient_detail.hfpatientid, "AccountNumber");

                $("#appointmentDetail #btnAddReferral").removeClass("disableAll");

                appointmentDetail.params.PatientLastName = patient[0].LastName;
                appointmentDetail.params.PatientFirstName = patient[0].FirstName;

                $('#appointmentDetail #txtRefProvider').val(patient[0].ReferringProviderName);
                $('#appointmentDetail #hfRefProvider').val(patient[0].ReferringProviderId);

                utility.SetKendoAutoCompleteSourceforValidate($('#appointmentDetail #txtRefProvider'), patient[0].ReferringProviderName, $('#appointmentDetail #hfRefProvider'), patient[0].ReferringProviderId);
                //this code stuck screen to bind 9876 records.no need to do this
                //CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
                //    for (var i = 0; i < result.length; i++) {
                //        if (result[i].Value == patient[0].ReferringProviderId && patient[0].ReferringProviderId != "") {

                //            if ($("#appointmentDetail #lnkRefProviderEdit").css("display") == "none") {
                //                $("#appointmentDetail #lnkRefProviderEdit").css("display", "inline");
                //                $("#appointmentDetail #lblRefProvider").css("display", "none");
                //            }

                //            $('#appointmentDetail #txtRefProvider').val(patient[0].ReferringProviderName);
                //            $('#appointmentDetail #hfRefProvider').val(patient[0].ReferringProviderId);
                //            appointmentDetail.BindRefProvider();
                //            return false;
                //        } else {
                //            $("#appointmentDetail #lnkRefProviderEdit").css("display", "none");
                //            $("#appointmentDetail #lblRefProvider").css("display", "inline");
                //        }
                //    }
                //});



                if (appointmentDetail.params.mode == "Add") {
                    if (globalAppdata.DefaultFacilityId && globalAppdata.DefaultFacilityId != "") {
                        facilityDetail.FillFacility(globalAppdata.DefaultFacilityId).done(function (responseFacility) {
                            var facility_detail = JSON.parse(responseFacility.FacilityFill_JSON);
                            if (appointmentDetail.params.ParentCtrl == "schcheckout") {
                                $("#appointmentDetail #frmappointmentDetail #txtFacility").val(appointmentDetail.params.FacilityName);
                                $("#appointmentDetail #frmappointmentDetail #hfFacilityId").val(appointmentDetail.params.FacilityId);
                            }
                            else {
                                $("#appointmentDetail #frmappointmentDetail #txtFacility").val(facility_detail.txtDescription);
                                $("#appointmentDetail #frmappointmentDetail #hfFacilityId").val(globalAppdata.DefaultFacilityId);
                            }
                            utility.SetKendoAutoCompleteSourceforValidate($("#appointmentDetail #txtFacility"), $("#appointmentDetail #txtFacility").val(), $("#appointmentDetail #hfFacilityId"), $("#appointmentDetail #hfFacilityId").val());
                            appointmentDetail.BindInsurancePlans(response.PatientInsurance_JSON, appInsuranceId);
                            if ($('#frmappointmentDetail').data('bootstrapValidator') != null && typeof $('#frmappointmentDetail').data('bootstrapValidator') != 'undefined') {
                                $('#frmappointmentDetail').bootstrapValidator('revalidateField', 'FullName');
                            }
                            $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                            dfd.resolve('ok');
                        });
                    }
                    else {
                        appointmentDetail.BindInsurancePlans(response.PatientInsurance_JSON, appInsuranceId);
                        //appointmentDetail.ValidateAppointment();
                        if ($('#frmappointmentDetail').data('bootstrapValidator') != null && typeof $('#frmappointmentDetail').data('bootstrapValidator') != 'undefined') {
                            $('#frmappointmentDetail').bootstrapValidator('revalidateField', 'FullName');
                        }
                        $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                        dfd.resolve('ok');
                    }
                }
                else {
                    appointmentDetail.BindInsurancePlans(response.PatientInsurance_JSON, appInsuranceId);
                    //appointmentDetail.ValidateAppointment();
                    if ($('#frmappointmentDetail').data('bootstrapValidator') != null && typeof $('#frmappointmentDetail').data('bootstrapValidator') != 'undefined') {
                        $('#frmappointmentDetail').bootstrapValidator('revalidateField', 'FullName');
                    }
                    $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                    dfd.resolve('ok');
                }
                $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });
        return dfd.promise();
    },

    ValidateAppointment: function () {
        $('#frmappointmentDetail')
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
                   Facility: {
                       group: '.customvalidate',
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
            if ($('#appointmentDetail #ddlStatus option:selected').text().trim() == "Cancel"
                && (appointmentDetail.CancellationReason == null || appointmentDetail.CancellationReason == "")) {
                appointmentDetail.LoadCancellationReason();
            }
            else {
                // Check if a cancelled appointment already exists 
                appointmentDetail.CheckifCancelledAppointmentExists().done(function (response) {
                    //  var response = JSON.parse(response);
                    if (response.status) {
                        if (response.CancelledAppointmentExists) {
                            utility.myConfirm("61", function () {
                                appointmentDetail.SavePatientAppointmentDetail();
                            }
                                , function () {
                                    appointmentDetail.UnLoad();
                                })
                        }
                        else {
                            appointmentDetail.SavePatientAppointmentDetail();
                        }
                    }

                })

            }
        });
    },

    ResetHiddenValue: function () {
        $('#appointmentDetail #hfpatientid').val("-1");
    },

    ResetRefProvider: function () {

        $('#appointmentDetail #hfRefProvider').val(null);

    },

    ValidateValues: function () {
        if ($('#appointmentDetail #hfpatientid').val() == "-1") {
            utility.DisplayMessages("Patient not Valid", 2);
            return false;
        }
        else
            return true;

    },

    CheckFacility: function (ThisCTRL) {
        if ($(ThisCTRL).val() == "") {
            $("#appointmentDetail #frmappointmentDetail #hfFacilityId").val("");
        }
    },

    SavePatientAppointment: function (AppointmentData, AppointmentPatternData) {
        var referralId = appointmentDetail.ReferralId;
        if ($('#appointmentDetail #btnAddReferral').text() != 'View Referral') {
            referralId = "";
        }
        var data = "AppointmentData=" + AppointmentData + "&AppointmentPatternData=" + AppointmentPatternData + "&ReferralId=" + referralId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "SAVE_PATIENT_APPOINTMENT");
    },

    SaveAppointment: function (AppointmentData, AppointmentPatternData) {
        var referralId = appointmentDetail.ReferralId;
        if ($('#appointmentDetail #btnAddReferral').text() != 'View Referral') {
            referralId = "";
        }
        var data = "AppointmentData=" + AppointmentData + "&AppointmentPatternData=" + AppointmentPatternData + "&ReferralId=" + referralId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "SAVE_APPOINTMENT");
    },

    FillPatient: function (PatientID) {
        var data = "PatientID=" + PatientID;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_PATIENT");
    },

    searchPatientInsurance: function (patientID) {
        var data = "PatientID=" + patientID + "&PatientInsuranceId=" + 0;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "PATIENT_INSURANCE", "SEARCH_PATIENT_INSURANCE_FROMLAB");
    },

    FillRefProviderName: function (RefProviderId, RefProviderName) {
        $('#appointmentDetail #txtRefProvider').val(RefProviderName);
        $('#appointmentDetail #hfRefProvider').val(RefProviderId);
        UnloadActionPan(appointmentDetail.params["ParentCtrl"]);
    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";

        params["ParentCtrl"] = "appointmentDetail";
        LoadActionPan('Admin_ReferringProvider', params);
    },

    OpenPatientAccount: function () {
        if (!$('#appointmentDetail #SchedulingFormDiv').hasClass("disableAll")) {
            $('#appointmentDetail #SchedulingFormDiv').addClass("disableAll")
        }
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = 'appointmentDetail';
        LoadActionPan('Patient_Search', params);
    },

    OpenPatientEligibility: function () {

        var PatientInsurancePlanId = $("#appointmentDetail #ddlInsurancePlan").val();
        var PatientId = $("#appointmentDetail #hfpatientid").val();

        if (PatientId && PatientInsurancePlanId) {
            var params = [];
            params["FromAdmin"] = "0";
            params["patientID"] = PatientId;
            params["patientAccount"] = $('#appointmentDetail #txtAccountNo').val();
            params["patientLastName"] = appointmentDetail.params.PatientLastName;
            params["patientFirstName"] = appointmentDetail.params.PatientFirstName;
            params["patientInsurancePlanId"] = PatientInsurancePlanId;
            params["Provider"] = $('#appointmentDetail #txtProvider').val();
            params["ProviderId"] = $('#appointmentDetail #hfProviderId').val();
            params["ParentCtrl"] = 'appointmentDetail';
            LoadActionPan('Patient_Eligibility', params);
        }
        else {
            if (PatientId == "")
                utility.DisplayMessages("Please select Patient.", 3);
            else
                utility.DisplayMessages("Please select Patient insurance plan.", 3);
        }


    },

    FillReferralNumber: function () {
        // Start 29/01/2016 Muhammad Irfan for Bug # 3738
        var AccountNo = $('#appointmentDetail #frmappointmentDetail #txtAccountNo').val();
        if (AccountNo == "") {
            $('#appointmentDetail #frmappointmentDetail #txtFullName,#dtpDOB,#txtPatientBalance,#txtInsuranceBalance,#txtAdvanceBalance,#txtAdvanceBalance,#txtRefProvider,#txtPriority,#txtReferralNo,#CoPayment').val('');
            $('#appointmentDetail #frmappointmentDetail #ddlInsurancePlan option').remove();
            if ($("#appointmentDetail #lnkRefProviderEdit").css("display") == "inline") {
                $("#appointmentDetail #lnkRefProviderEdit").css("display", "none");
                $("#appointmentDetail #lblRefProvider").css("display", "inline");
            }

            $("#appointmentDetail #btnAddReferral").addClass("disableAll");
        }


        // End 29/01/2016 Muhammad Irfan for Bug # 3738
        var ParentCtrl = "appointmentDetail";
        var self = $("#appointmentDetail");

        var patientID = self.find("#hfpatientid").val();
        var selectedInsurancePlan = self.find("#ddlInsurancePlan option:selected").val() != null ? self.find("#ddlInsurancePlan option:selected").val() : "";
        var objReferralNumber = self.find("#txtReferralNo");
        var ProviderId = self.find("#hfProviderId").val();
        var FacilityId = self.find("#hfFacilityId").val();
        var VisitDate = self.find("#dtpDOB").val();
        var appStatus = $("#frmappointmentDetail #ddlStatus > option:selected").length > 0 ? $("#frmappointmentDetail #ddlStatus > option:selected").text().trim() : "";
        if (selectedInsurancePlan == "") {
            objReferralNumber.attr("disabled", "disabled");
        }
        else if (appStatus == "Check In") {
            objReferralNumber.attr("disabled", "disabled");
        }
        else {
            objReferralNumber.removeAttr("disabled");
        }
        $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
        if (selectedInsurancePlan != "" && ProviderId != "" && FacilityId != "" && VisitDate != "") {
            patientReferralSearch.SearchReferral("Incoming", selectedInsurancePlan, ProviderId, FacilityId, VisitDate, "1").done(function (response) {
                if (response.status != false) {
                    var PatientReferral = [];
                    if (response.ReferralCount > 0) {
                        var ReferralJSONData = JSON.parse(response.ReferralLoad_JSON);
                        //$.each(ReferralJSONData, function (i, item) {
                        //    PatientReferral.push({ id: item.PatientReferralId, value: item.ReferralAuthNo });
                        //});

                        //self.find("#txtReferralNo").autocomplete({
                        //    source: PatientReferral,
                        //    autoFocus: true,
                        //    select: function (event, ui) {

                        //        setTimeout(function () {
                        //            self.find("#txtReferralNo").val(ui.item.value);
                        //            self.find("#hfReferralNumerId").val(ui.item.id);
                        //            if (self.find("#lnkReferralNumberEdit").css("display") == "none") {
                        //                self.find("#lnkReferralNumberEdit").css("display", "inline");
                        //                self.find("#lblReferralNumber").css("display", "none");
                        //            }
                        //        }, 100);

                        //    }
                        //});

                        //self.find("#txtReferralNo").autocomplete("search");
                    }
                    $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                }
            });

        }
    },

    FillReferralPatientName: function () {
        // Start 29/01/2016 Muhammad Irfan for Bug # 3738
        var AccountNo = $('#appointmentDetail #frmappointmentDetail #txtFullName').val();
        if (AccountNo == "") {
            $('#appointmentDetail #frmappointmentDetail #txtFullName,#dtpDOB,#txtPatientBalance,#txtInsuranceBalance,#txtAdvanceBalance,#txtAdvanceBalance,#txtRefProvider,#txtPriority,#txtReferralNo,#CoPayment').val('');
            $('#appointmentDetail #frmappointmentDetail #ddlInsurancePlan option').remove();
            if ($("#appointmentDetail #lnkRefProviderEdit").css("display") == "inline") {
                $("#appointmentDetail #lnkRefProviderEdit").css("display", "none");
                $("#appointmentDetail #lblRefProvider").css("display", "inline");
            }

            $("#appointmentDetail #btnAddReferral").addClass("disableAll");
        }


        // End 29/01/2016 Muhammad Irfan for Bug # 3738
        var ParentCtrl = "appointmentDetail";
        var self = $("#appointmentDetail");

        var patientID = self.find("#hfpatientid").val();
        var selectedInsurancePlan = self.find("#ddlInsurancePlan option:selected").val() != null ? self.find("#ddlInsurancePlan option:selected").val() : "";
        var objReferralNumber = self.find("#txtReferralNo");
        var ProviderId = self.find("#hfProviderId").val();
        var FacilityId = self.find("#hfFacilityId").val();
        var VisitDate = self.find("#dtpDOB").val();
        var appStatus = $("#frmappointmentDetail #ddlStatus > option:selected").length > 0 ? $("#frmappointmentDetail #ddlStatus > option:selected").text().trim() : "";
        if (selectedInsurancePlan == "") {
            objReferralNumber.attr("disabled", "disabled");
        }
        else if (appStatus == "Check In") {
            objReferralNumber.attr("disabled", "disabled");
        }
        else {
            objReferralNumber.removeAttr("disabled");
        }
        $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
        if (selectedInsurancePlan != "" && ProviderId != "" && FacilityId != "" && VisitDate != "") {
            patientReferralSearch.SearchReferral("Incoming", selectedInsurancePlan, ProviderId, FacilityId, VisitDate, "1").done(function (response) {
                if (response.status != false) {
                    var PatientReferral = [];
                    if (response.ReferralCount > 0) {
                        var ReferralJSONData = JSON.parse(response.ReferralLoad_JSON);
                        //$.each(ReferralJSONData, function (i, item) {
                        //    PatientReferral.push({ id: item.PatientReferralId, value: item.ReferralAuthNo });
                        //});

                        //self.find("#txtReferralNo").autocomplete({
                        //    source: PatientReferral,
                        //    autoFocus: true,
                        //    select: function (event, ui) {

                        //        setTimeout(function () {
                        //            self.find("#txtReferralNo").val(ui.item.value);
                        //            self.find("#hfReferralNumerId").val(ui.item.id);
                        //            if (self.find("#lnkReferralNumberEdit").css("display") == "none") {
                        //                self.find("#lnkReferralNumberEdit").css("display", "inline");
                        //                self.find("#lblReferralNumber").css("display", "none");
                        //            }
                        //        }, 100);

                        //    }
                        //});

                        //self.find("#txtReferralNo").autocomplete("search");
                    }
                    $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                }
            });

        }
    },

    OpenPatientReferral: function () {
        var self = $("#appointmentDetail");
        var patientInsuranceID = self.find("#ddlInsurancePlan option:selected").val();

        if (patientInsuranceID != "") {
            var params = [];
            params["ParentCtrl"] = 'appointmentDetail';
            var ProviderId = self.find("#hfProviderId").val();
            var FacilityId = self.find("#hfFacilityId").val();
            var DOSFrom = appointmentDetail.params.DayDate;
            params["RefCtrlReferralNo"] = "txtReferralNo";
            params["RefHiddenIdCtrl"] = "hfReferralNumerId";
            params["RefCtrlRefProvider"] = "txtRefProvider";
            params["RefProviderHiddenIdCtrl"] = "hfRefProvider";
            params["patientID"] = self.find("#hfpatientid").val();
            params["patientInsuranceID"] = patientInsuranceID;
            params["ProviderId"] = ProviderId;
            params["FacilityId"] = null;
            params["ReferralDate"] = DOSFrom;
            params["FacilityToId"] = FacilityId;

            LoadActionPan('patientReferralSearch', params);
        } else {
            utility.DisplayMessages("Please select insurance plan first", 2);
        }


    },

    OpenPatientReferralDetail: function (HiddenCtrl) {

        var params = [];
        params["ParentCtrl"] = "appointmentDetail";
        params["ReferralID"] = $('#appointmentDetail #' + HiddenCtrl).val();
        params["patientID"] = $('#appointmentDetail #hfpatientid').val();
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

    UpdatePatientAppointment: function (AppointmentData, AppointmentID) {

        var data = "AppointmentData=" + AppointmentData + "&AppointmentID=" + AppointmentID + "&ReferralId=" + appointmentDetail.ReferralId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "UPDATE_PATIENT_APPOINTMENT");
    },
    CheckCancelledAppointmentExists: function (AppointmentData) {

        var data = "AppointmentData=" + AppointmentData;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "CHECK_CANCELLED_APPOINTMENT");
    },
    UpdateAppointment: function (AppointmentData, AppointmentID) {

        var data = "AppointmentData=" + AppointmentData + "&AppointmentID=" + AppointmentID + "&ReferralId=" + appointmentDetail.ReferralId;
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
        if (ProviderId) {
            appointmentDetail.SearchProvider("", ProviderId).done(function (response) {
                if (response.status != false) {
                    if (response.ProviderCount > 0) {
                        var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);
                        if (ProviderLoadJSONData[0].IsSpecialist == "True") {
                            $('#appointmentDetail #rdSpecialist').attr("checked", "checked");
                        } else {
                            $('#appointmentDetail #rdPCP').attr("checked", "checked");
                        }

                    }
                    appointmentDetail.LoadAppointment();
                }
            });
        }
        else {
            appointmentDetail.LoadAppointment();
        }
    },

    SearchProvider: function (ProviderData, ProviderId) {
        var data = "ProviderData=" + ProviderData + "&ProviderID=" + ProviderId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER", "SEARCH_PROVIDER");
    },

    UnLoad: function (isConfirmationHide) {
        if (appointmentDetail.params["ParentCtrl"] == null) {
            appointmentDetail.params["ParentCtrl"] = 'schwaitlistdetail';
        }
        if (isConfirmationHide) {
            UnloadActionPan(appointmentDetail.params["ParentCtrl"], "actionPanAppointmentDetail");
        }
        else {
            utility.UnLoadDialog('appointmentDetail #frmappointmentDetail', function () {
                UnloadActionPan(appointmentDetail.params["ParentCtrl"], "actionPanAppointmentDetail");
            }, function () {
                UnloadActionPan(appointmentDetail.params["ParentCtrl"], "actionPanAppointmentDetail");
            });
        }


    },

    OpenRefProviderDetail: function () {
        var params = [];
        params["ReferringProviderId"] = $('#appointmentDetail #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "appointmentDetail";

        LoadActionPan('referringproviderDetail', params);
    },

    //FillPatientBalance: function (AppointmentID) {
    //    var data = "AppointmentID=" + AppointmentID;
    //    // serach parameter , class name, command name of class
    //    return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_PATIENT_BALANCE");
    //},

    //PatientBalanceFill: function () {

    //    appointmentDetail.FillPatientBalance(AppointmentID).done(function (response) {

    //        if (response.status != false) {

    //            var patient_balance = JSON.parse(response.PatientBalanceFill_JSON);

    //            $('#appointmentDetail #txtPatientBalance').val(patient_balance.txtPatientBalance);

    //        }


    //    });

    //},


    OpenPatReferralSearch: function (PatientInsuranceId) {

        // Start 29/01/2016 Muhammad Irfan Bug # PMS-3739
        $('#appointmentDetail #frmappointmentDetail #txtReferralNo').val('');
        // End 29/01/2016 Muhammad Irfan Bug # PMS-3739

        var strMessage = "";
        var params = [];
        params["RefCtrlReferralNo"] = "txtReferralNo";
        params["RefCtrlRefProvider"] = "txtRefProvider";
        var PatientInsuranceId = $('#appointmentDetail #ddlInsurancePlan').val();
        PatientInsuranceId = PatientInsuranceId != null ? PatientInsuranceId : "";
        var ProviderId = $("#" + appointmentDetail.params.PanelID + " #frmappointmentDetail #hfProviderId").val();
        var FacilityId = $("#" + appointmentDetail.params.PanelID + " #frmappointmentDetail #hfFacilityId").val();
        var ReferralType = "Incoming";
        var appointmentDate = $('#appointmentDetail #txtScheduleDate').val();

        if (appointmentDetail.params.ResourceId) {
            ProviderId = $("#frmappointmentDetail #hfProviderId").val();
        }

        if (appointmentDate) {
            VisitDate = appointmentDate;
        }
        else {
            var d = new Date();
            VisitDate = d.toISOString().substring(0, 10);
        }

        var ParentCtrl = "appointmentDetail";
        var objReferralNumber = $("#appointmentDetail #txtReferralNo");
        var objRefProvider = $("#appointmentDetail #txtRefProvider");
        var objRefProviderId = $("#appointmentDetail #hfRefProvider");
        var patientID = $("#appointmentDetail #hfpatientid").val();

        if (PatientInsuranceId && ProviderId && VisitDate) {
            // if (FacilityId) {
            appointmentDetail.GetReferralNo(PatientInsuranceId, ProviderId, FacilityId, VisitDate).done(function (response) {
                if (response.status != false) {
                    if (response.ReferralCount == 1) {
                        var ReferralLoadJSONData = JSON.parse(response.ReferralLoad_JSON);
                        // set PatientRefrral Id 
                        if (ReferralLoadJSONData[0].PatientReferralId) {
                            $("#appointmentDetail #hfReferralNumerId").val(ReferralLoadJSONData[0].PatientReferralId);
                        }
                        objReferralNumber.val(ReferralLoadJSONData[0].ReferralAuthNo);
                        appointmentDetail.BindRefProviderJSON(ReferralLoadJSONData);
                    }
                    else if (response.ReferralCount > 1) {
                        // 
                        if (ProviderId && !$('#appointmentDetail #frmappointmentDetail #txtReferralNo').val()) {
                            patientReferralSearch.SearchReferral(ReferralType, PatientInsuranceId, ProviderId, 0, VisitDate, "1", true).done(function (response) {
                                if (response.status != false) {
                                    EncounterChargeCapture.OpenReferralSearch(PatientInsuranceId, objReferralNumber.attr("id"), objRefProvider.attr("id"), response, ParentCtrl, patientID, "Incoming", ProviderId, 0, VisitDate, "1", FacilityId);
                                    $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                                }
                                else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        }
                    }
                    $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                }
            });
            //}
            //else {
            //    patientReferralSearch.SearchReferral(ReferralType, PatientInsuranceId, ProviderId, 0, VisitDate, "1").done(function (response) {
            //        if (response.status != false) {
            //            if (response.ReferralCount > 1) {
            //                EncounterChargeCapture.OpenReferralSearch(PatientInsuranceId, objReferralNumber.attr("id"), objRefProvider.attr("id"), response, ParentCtrl, patientID, "Incoming", ProviderId, 0, VisitDate, "1");
            //            }
            //            else if (response.ReferralCount == 1) {
            //                var ReferralLoadJSONData = JSON.parse(response.ReferralLoad_JSON);
            //                appointmentDetail.BindRefProviderJSON(ReferralLoadJSONData);
            //            }
            //            $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
            //        }
            //        else {
            //            utility.DisplayMessages(response.Message, 3);
            //        }
            //    });
            //}

        }

    },

    BindRefProviderJSON: function (ReferralLoadJSONData) {

        var objRefProvider = $("#appointmentDetail #txtRefProvider");
        var objRefProviderId = $("#appointmentDetail #hfRefProvider");

        objRefProvider.val(ReferralLoadJSONData[0].ReferringFromName);
        objRefProviderId.val(ReferralLoadJSONData[0].ReferringFromId);
        objRefProvider.focus();
        utility.SetKendoAutoCompleteSourceforValidate($('#appointmentDetail #txtRefProvider'), $('#appointmentDetail #txtRefProvider').val(), $('#appointmentDetail #hfRefProvider'), $('#appointmentDetail #hfRefProvider').val());
        if ($("#appointmentDetail #lnkRefProviderEdit").css("display") == "none") {
            $("#appointmentDetail #lnkRefProviderEdit").css("display", "inline");
            $("#appointmentDetail #lblRefProvider").css("display", "none");
        }
    },



    AddTime: function (obj) {

        //var toTime = $('#appointmentDetail #txtToTime').val();

        //var toHours = Number($('#appointmentDetail #txtToTime').val().split(':')[0]);
        //var toMin = Number($('#appointmentDetail #txtToTime').val().split(':')[1].split(' ')[0]);
        //var timeZone = $('#appointmentDetail #txtToTime').val().split(':')[1].split(' ')[1];

        //var timeStart = new Date("01/01/2007 " + $('#txtFromTime').val()).getMinutes();
        //var timeEnd = new Date("01/01/2007 " + $('#txtToTime').val()).getMinutes();

        //var hourDiff = timeEnd - timeStart;

        //var remainder = "";
        //var hRemainder = "";
        //if (Number($('#Duration').val()) + toMin >= 60) {
        //    remainder = Number($('#Duration').val()) % 60;
        //    hRemainder = Math.round(Number($('#Duration').val()) / 60);
        //}
        //else {
        //    remainder = $('#Duration').val();

        //}
        //var newTime = "";
        //if (Number($('#Duration').val()) > hourDiff)
        //    newTime = (toHours + Number(hRemainder)) + ":" + (toMin + (Number($('#Duration').val()) - hourDiff)) + " " + timeZone;
        //else
        //    newTime = (toHours + Number(hRemainder)) + ":" + (toMin - (hourDiff - Number($('#Duration').val()))) + " " + timeZone;


        // var a = obj.value;
        // var c = $("#txtToTime").val();

        // var Min = c.substring(3, 5);

        // var hours = c.substring(0, 2);

        // var ampm = c.substring(6, 8);

        // var newMin = parseInt(a) + parseInt(Min);

        // if (newMin >= 60)
        //    var newhours = parseInt(hours) + 1;

        // if(hours < 12)
        //     if(newhours >= 12)
        //         if(ampm == "AM")
        //             ampm = "PM"


        //var nmin  = parseInt(newMin) - parseInt(a);



        //$("#txtToTime").val( newhours + ':' + nmin + ' ' + ampm)
        //$("#txtToTime").val(newTime);


        var fromTimeObj = $("#txtFromTime");
        var toTimeObj = $("#txtToTime")

        var timeFrom = new Date("01/01/2007 " + $('#txtFromTime').val());
        timeFrom.setMinutes(timeFrom.getMinutes() + Number($('#Duration').val()));
        $("#txtToTime").timepicker({ ampm: true });
        $("#txtToTime").timepicker("setTime", timeFrom);
        $("#txtToTime").val(appointmentDetail.formatAMPM(new Date("01/01/2007 " + $('#txtToTime').val())));
        //var timeEnd = new Date("01/01/2007 " + $('#txtToTime').val()).getMinutes();

    },
    formatAMPM: function (date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'PM' : 'AM';
        hours = hours % 12;
        hours = hours ? hours : 12; // the hour '0' should be '12'
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return strTime;
    },
    VisitTypeDropdownLoad: function () {
        appointmentDetail.LoadVisittypeDropdown(appointmentDetail.params.ProviderId).done(function (response) {
            $('#appointmentDetail #ddlVisitType').empty();
            if (response.length > 0) {
                $.each(response, function (j, result) {
                    $('#appointmentDetail #ddlVisitType').append($("<option />").val(result.Value).text(result.Name).attr("RefValue", result.RefValue).attr("Duration", result.RefName));
                });
            } else {
                $('#appointmentDetail #ddlVisitType').append('<option value="" refvalue="" duration="" style="display: block;">- Select -</option>');
            }
            appointmentDetail.BindVisitType($("#appointmentDetail #ddlPatientType"));
            $('#appointmentDetail #frmappointmentDetail').data('serialize', $('#appointmentDetail #frmappointmentDetail').serialize());
        });
    },

    BindVisitType: function (control) {
        var selectedPateintType = $(control).val();
        $("#appointmentDetail #ddlVisitType option:selected").prop("selected", false);
        $("#appointmentDetail #ddlPatientType option").each(function (i, item) {
            if (item.value == selectedPateintType) {
                appointmentDetail.showDropdownOptions(item.value, true);
            }
            else if (item.value == "") {
                appointmentDetail.showDropdownOptions('', true);
            }
            else
                appointmentDetail.showDropdownOptions(item.value, false);
        });

        if (selectedPateintType == "") {
            $('#appointmentDetail #ddlVisitType').val("");
            $('#appointmentDetail #ddlVisitType').attr("disabled", true);
        }
        else {
            $('#appointmentDetail #ddlVisitType').removeAttr("disabled");
        }
    },

    showDropdownOptions: function (value, canShowOption) {
        $('#appointmentDetail #ddlVisitType').find('option[Duration="0"]').remove()
        $('#appointmentDetail #ddlVisitType').find('option[refvalue="' + value + '"]').map(function () {
            return $(this).parent('span').length === 0 ? this : null;
        }).wrap('<span>').hide();

        if (canShowOption) {
            //$('#appointmentDetail #ddlVisitType').find('option[refvalue="' + value + '"]').show();
            ////$('#appointmentDetail #ddlVisitType').find('option:not([refvalue="' + value + '"])').hide();
            $('#appointmentDetail #ddlVisitType').find('option[refvalue="' + value + '"]').unwrap().show();
        }
        else
            $('#appointmentDetail #ddlVisitType').find('option[refvalue="' + value + '"]').hide();

    },

    LoadVisittypeDropdown: function (ProviderId) {
        var data = "ProviderId=" + ProviderId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "LOAD_VISIT_TYPE_DROPDOWN");
    },
    /* start 03/02/2016 Muhammad Irfan bug # PMS-3827,3828 */
    changeFunction: function () {
        var objDuration = $('#appointmentDetail #Duration');
        if (objDuration.val() == 0) {
            objDuration.val('');
            if ($('#frmappointmentDetail').data('bootstrapValidator') != null && typeof $('#frmappointmentDetail').data('bootstrapValidator') != 'undefined') {
                $('#frmappointmentDetail').bootstrapValidator('revalidateField', 'Duration');
            }
        } else {

        }
    },

    /* start 03/02/2016 Muhammad Irfan bug # PMS-3827,3828 */

    FillScheduleReason: function (ScheduleReasonId, ShortName, Duration, event) {

        if (event != null) {
            event.stopPropagation();
        }

        UnloadActionPan("appointmentDetail");

        $('#appointmentDetail #txtSchReason').val(ShortName);
        $('#appointmentDetail #hfSchReasonId').val(ScheduleReasonId);
        $('#appointmentDetail #Duration').val(Duration);

        if ($('#frmappointmentDetail').data('bootstrapValidator') != null && typeof $('#frmappointmentDetail').data('bootstrapValidator') != 'undefined') {
            $('#frmappointmentDetail').bootstrapValidator('revalidateField', 'Duration');
        }


    },

    BindScheduleReasons: function () {
        var SchReason = $('#appointmentDetail #txtSchReason').val();
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

            $('#appointmentDetail #txtSchReason').autocomplete({
                autoFocus: true,
                source: AllSchReasons,
                open: function (event, ui) { disable = true },
                close: function (event, ui) {
                    disable = false; $(this).focus();
                },
                select: function (event, ui) {
                    setTimeout(function () {
                        $('#appointmentDetail #txtSchReason').val(ui.item.value);
                        $('#appointmentDetail #hfSchReasonId').val(ui.item.id);
                        $('#appointmentDetail #Duration').val(ui.item.Duration)
                    }, 100);

                }
            }).blur(function () {
                setTimeout(function () {
                    utility.ValidateAutoComplete($('#appointmentDetail #txtSchReason'), 'appointmentDetail #hfSchReasonId', false, null, null, null);
                }, 200);
            });
            $('#appointmentDetail #txtSchReason').autocomplete("search");

        });

        //--------------------
    },

    OpenScheduleReason: function () {

        var params = [];
        params["ScheduleReasonId"] = "-1";
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "appointmentDetail";
        LoadActionPan('Admin_ScheduleReason', params);

    },

    //----------------------------------

    BindRefProvider: function () {
        var Ctrl = $('#appointmentDetail #txtRefProvider');
        var func = function () { return utility.GetRefProviderArray(Ctrl.val()) };
        var hfCtrl = $("#appointmentDetail #hfRefProvider")
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl);
    },

    BindProvider: function () {
        var Ctrl = $("#" + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtProvider");
        var func = function () { return utility.GetProviderArray(Ctrl.val()) };
        var hfCtrl = $("#" + appointmentDetail.params.PanelID + " #frmappointmentDetail #hfProviderId");
        var onSelect = function (e) {
            $("#" + appointmentDetail.params.PanelID + " #frmappointmentDetail #hfProviderId").val(e.id);
            appointmentDetail.OpenPatReferralSearch();
        };
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },

    BindFacility: function () {
        var Ctrl = $("#" + appointmentDetail.params.PanelID + " #frmappointmentDetail #txtFacility");
        var func = function () { return utility.GetFacilityDescriptionArray(Ctrl.val()) };
        var hfCtrl = $("#" + appointmentDetail.params.PanelID + " #frmappointmentDetail #hfFacilityId");
        var onSelect = function (e) {
            $("#" + appointmentDetail.params.PanelID + " #frmappointmentDetail #hfFacilityId").val(e.id);
            appointmentDetail.OpenPatReferralSearch();
        }
        utility.BindKendoAutoComplete(Ctrl, 2, "value", "contains", null, func, hfCtrl, onSelect);
    },
    //Function Name: addReferral
    //Author Name: Humaira Yousaf
    //Created Date: 22-08-2016
    //Description: Adds Referral
    addReferral: function () {
        var params = [];
        params["PatientId"] = $('#appointmentDetail #hfpatientid').val();
        params["ReferralTo"] = $('#appointmentDetail #txtProvider').val();
        params["ReferralToId"] = $('#appointmentDetail #hfProviderId').val();
        params["InsurancePlan"] = appointmentDetail.PatientInsuranceId != null ? appointmentDetail.PatientInsuranceId : $('#appointmentDetail #ddlInsurancePlan').val();
        // params["Reason"] = appointmentDetail.params.ScheduleReason;
        params["Reason"] = $('#appointmentDetail #txtSchReason').val();
        params["Status"] = 1; // Not Started    
        params["ParentCtrl"] = "appointmentDetail";

        if ($('#appointmentDetail #btnAddReferral').text() == 'Add Referral') {
            params["mode"] = "Add";
        }
        else {
            params["mode"] = "Edit";
            appointmentDetail.ReferralId = $('#appointmentDetail #hfReferralId').val();
            params["ReferralId"] = appointmentDetail.ReferralId;
        }
        LoadActionPan('Patient_Referrals_Incoming_Detail', params);
    },

    OpenQuickAddPatient: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "appointmentDetail";
        LoadActionPan('Patient_DemographicQuick', params)
    },


    OpenFacility: function () {
        if ($("#appointmentDetail #frmappointmentDetail #txtFacility").val() != "") {
            utility.myConfirm('Are you sure you want to change the Facility?', function () {
                var params = [];
                params["FacilityId"] = "-1";
                params["FromAdmin"] = "0";
                params["RefHiddenIdCtrl"] = "hfFacilityId";
                params["ParentCtrl"] = "appointmentDetail";
                LoadActionPan('Admin_Facility', params);
            }, function () { },
                        'Confirm Change'
                    );
        } else {
            var params = [];
            params["FacilityId"] = "-1";
            params["FromAdmin"] = "0";
            params["RefHiddenIdCtrl"] = "hfFacilityId";
            params["ParentCtrl"] = "appointmentDetail";
            LoadActionPan('Admin_Facility', params);
        }
    },


    changeIsNonBilable: function () {

        if ($("#appointmentDetail #chkNonBilable").is(':checked')) {
            utility.myConfirm('Are you sure you want to make the Visit as Non Billable?', function () {
                //
            }, function () {
                $('#appointmentDetail #chkNonBilable').prop('checked', false);
            },
                   'Confirm Non Billable'
               );
        } else {
            //
        }



    },

    setDurationOnVisitType: function (obj) {
        var txtDuration = $('#appointmentDetail #Duration');
        if (appointmentDetail.params.ProviderId && appointmentDetail.params.ProviderId != "-1" && $('#appointmentDetail #ddlVisitType option:selected').val() != "") {
            appointmentDetail.FillDurationOnVisitType(appointmentDetail.params.ProviderId, $('#appointmentDetail #ddlVisitType option:selected').val()).done(function (response) {
                if (response.status != false) {
                    txtDuration.val(response.Duration);
                    $('#appointmentDetail #Duration').trigger('blur');
                }
                else
                    appointmentDetail.DefaultConfigurationForVisitType();
            });
        } else
            appointmentDetail.DefaultConfigurationForVisitType();
    },
    DefaultConfigurationForVisitType: function () {
        var slotDuration = $('#appointmentDetail #hfSlotDuration').val();
        var txtDuration = $('#appointmentDetail #Duration');
        var selectedVisitType = $('#appointmentDetail #ddlVisitType option:selected').text().trim(); // Visit Type

        var selectedPatientType = $('#appointmentDetail #ddlPatientType option:selected').text().trim(); // Patient Type

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
        $('#appointmentDetail #Duration').trigger('blur');
    },
    FillDurationOnVisitType: function (ProviderId, PatientVisitTypeId) {
        var data = "ProviderID=" + ProviderId + "&PatientVisitTypeID=" + PatientVisitTypeId;
        // serach parameter , class name, command name of class
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "FILL_DURATION_ON_VISIT_TYPE");
    },
    OpenProviderDetail: function (Title, RefCtrl, RefCtrlHidden, RefCtrlLabel, RefCtrlLink) {
        if ($("#appointmentDetail #frmappointmentDetail #txtProvider").val() == "") {
            var params = [];
            params["Title"] = Title;
            params["RefCtrl"] = RefCtrl;
            params["RefCtrlHidden"] = RefCtrlHidden;
            params["RefCtrlLabel"] = RefCtrlLabel;
            params["RefCtrlLink"] = RefCtrlLink;
            params["IsOptional"] = false;
            params["RefForm"] = "frmappointmentDetail";
            params["ProviderId"] = "-1";
            params["FromAdmin"] = "0";
            params["ParentCtrl"] = 'appointmentDetail';
            LoadActionPan('Admin_Provider', params);
        } else {
            utility.myConfirm('Are you sure you want to change the Provider?', function () {
                var params = [];
                params["Title"] = Title;
                params["RefCtrl"] = RefCtrl;
                params["RefCtrlHidden"] = RefCtrlHidden;
                params["RefCtrlLabel"] = RefCtrlLabel;
                params["RefCtrlLink"] = RefCtrlLink;
                params["IsOptional"] = false;
                params["RefForm"] = "frmappointmentDetail";
                params["ProviderId"] = "-1";
                params["FromAdmin"] = "0";
                params["ParentCtrl"] = 'appointmentDetail';
                LoadActionPan('Admin_Provider', params);
            }, function () { },
                                    'Confirm Change'
                                    );
        }
    },
    OpenUnallocatedCopayment: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["AppointmentId"] = appointmentDetail.params.AppointmentId;
        params["VisitId"] = appointmentDetail.params.PatientVisitId ? appointmentDetail.params.PatientVisitId : 0;
        params["ProviderId"] = appointmentDetail.params.ProviderId;
        params["FacilityId"] = $("#appointmentDetail #hfFacilityId").val();
        params["PatientId"] = $("#appointmentDetail #hfpatientid").val();
        params["ParentCtrl"] = 'appointmentDetail';
        LoadActionPan('Scheduling_UnallocatedCopayment', params);
    },

    OpenAppointmentHistory: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["AppointmentId"] = appointmentDetail.params.AppointmentId;;
        params["PatientId"] = appointmentDetail.params.PatientId;
        params["ParentCtrl"] = 'appointmentDetail';
        LoadActionPan('appointmentHistory', params);

    },
    OpenInsurance: function () {
        var params = [];
        params["patientID"] = $("#appointmentDetail #frmappointmentDetail #hfpatientid").val();
        params["PatientInsuranceId"] = $('#appointmentDetail #ddlInsurancePlan').val();
        params["ParentCtrl"] = "appointmentDetail";

        LoadActionPan('Patient_Insurance', params);

    },
    GetReferralNo: function (PatientInsuranceId, ProviderId, FacilityId, AppointmentDate) {
        var PatientId = $("#appointmentDetail #hfpatientid").val();
        var data = "PatientInsuranceId=" + PatientInsuranceId + "&ProviderId=" + ProviderId + "&FacilityId=" + FacilityId + "&AppointmentDate=" + AppointmentDate + "&PatientId=" + PatientId;
        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "GET_REFERRAL_NO");
    },
    EnableDisableCopayAlert: function (obj) {
        if (parseFloat($(obj).val()) <= 0 || !$(obj).val()) {
            $('#appointmentDetail #frmappointmentDetail #chkcopayalert').prop("checked", false);
            $('#appointmentDetail #frmappointmentDetail #chkcopayalert').attr("disabled", "disabled");
        }

        else {
            $('#appointmentDetail #frmappointmentDetail #chkcopayalert').removeAttr("disabled", "disabled");
        }


    },
}