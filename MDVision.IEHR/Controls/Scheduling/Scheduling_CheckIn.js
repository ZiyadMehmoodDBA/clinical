schcheckin = {
    bIsFirstLoad: true,
    params: [],
    Load: function (params) {
        pageNo: null;
        rpp: null;
        schcheckin.params = params;

        if (schcheckin.bIsFirstLoad) {
            schcheckin.bIsFirstLoad = false;

        }
        //schcheckin.ProviderSearch(schcheckin.params.ProviderId);
        var self = $('#schcheckin');
        self.loadDropDowns(true).done(function () {
            schcheckin.LoadAllAutocomplete();
            schcheckin.LoadAppointment();
            var providerId = params.ProviderId;
            $('input[type=radio][name=RadTemplate]').change(function () {
                if (this.value == 'Provider') {
                    schcheckin.SearchTemplate(0, providerId, null, null).done(function (response) {
                        if (response.status != false) {
                            if (response.TemplateCount > 0) {
                                schcheckin.BindClinicalTemplate(response.TemplateLoad_JSON);

                                //serialize Data after all controls loaded.
                                $('#schcheckin #frmcheckin').data('serialize', $('#schcheckin #frmcheckin').serialize());
                            }
                        }
                    });
                }
                else if (this.value == 'Speciality') {
                    providerDetail.FillProvider(providerId).done(function (response) {
                        if (response.status != false) {
                            var providerDetail = JSON.parse(response.ProviderFill_JSON);
                            var specialtyId = providerDetail.ddlSpecialty;
                            schcheckin.SearchTemplate(specialtyId, 0, null, null).done(function (response) {
                                if (response.status != false) {
                                    if (response.TemplateCount > 0) {
                                        schcheckin.BindClinicalTemplate(response.TemplateLoad_JSON);
                                    }
                                }
                                else {
                                    $("#frmcheckin #ddlClinicalTemplate").empty();
                                    $("#frmcheckin #ddlClinicalTemplate").append($('<option/>', {
                                        value: "",
                                        html: "- SELECT -"
                                    }));
                                }
                            });
                        }
                    });
                }
            });
            schcheckin.SearchTemplate(null, providerId).done(function (response) {
                if (response.status != false) {
                    if (response.TemplateCount > 0) {
                        schcheckin.BindClinicalTemplate(response.TemplateLoad_JSON);

                        //serialize Data after all controls loaded.
                        $('#schcheckin #frmcheckin').data('serialize', $('#schcheckin #frmcheckin').serialize());
                    }
                }
            });

            //serialize Data after all controls loaded.
            $('#schcheckin #frmcheckin').data('serialize', $('#schcheckin #frmcheckin').serialize());
        });
        if ($('#pnlScheduleCalendar #rdresource').is(':checked')) {

            $('#schcheckin #frmcheckin #selectedresouce').css("display", "inline");
            $('#schcheckin #frmcheckin #txtRecource').val($("#pnlScheduleCalendar #Resource option:selected").text());
        }
        else {
            $('#schcheckin #frmcheckin #selectedresouce').css("display", "none");

        }
    },

    SearchTemplate: function (specialityId, providerId, pageNo, rpp) {
        if (pageNo == null) {
            pageNo = 1;
        }
        if (rpp == null) {
            rpp = 15;
        }
        var data = "SpecialityId=" + specialityId + "&ProviderId=" + providerId + "&PageNo=" + pageNo + "&rpp=" + rpp;
        return MDVisionService.defaultService(data, "SCHEDULING_CHECKIN", "LOAD_TEMPLATE_LOOKUP");
    },

    LoadAllAutocomplete: function () {

        CacheManager.BindCodes('GetRefProviders', false).done(function (result) {
            var Ctrl = $("#schcheckin input#txtRefProvider");
            var hfCtrl = $("#schcheckin #hfRefProvider");
            utility.BindKendoAutoComplete(Ctrl, 1, "value", "contains", RefProviders, null, hfCtrl);
        });

    },

    LoadAppointment: function () {
        BackgroundLoaderShow(true);


        schcheckin.FillPatientData(schcheckin.params.PatientId, schcheckin.params.AppointmentId).done(function (response) {
            if (response.status != false) {


                var appointment_detail = JSON.parse(response.AppointmentFill_JSON);
                var patientappointments = JSON.parse(response.PatientAppointment_JSON);
                var patinsurance = JSON.parse(response.PatientInsuranceDetail_JSON);
                var copayments = JSON.parse(response.PatientCopayment_JSON);
                var patient_balance = JSON.parse(response.PatientBalance_JSON);
                if (patientappointments[0].PatientReferralId) {
                    var PatientRefrralId = patientappointments[0].PatientReferralId;
                    schcheckin.params["PatientReferralId"] = PatientRefrralId
                }
                 
               
                schcheckin.params.txtAccountNumber = appointment_detail.txtAccountNumber;
                schcheckin.params.txtFirstName = appointment_detail.txtFirstName;
                schcheckin.params.txtLastName = appointment_detail.txtLastName;
                schcheckin.params.txtGender = appointment_detail.txtGender;
                schcheckin.params.txtHomePhoneNo = appointment_detail.txtHomePhoneNo;
                schcheckin.params.txtDOB = appointment_detail.txtDOB;
                schcheckin.params.txtAge = appointment_detail.txtAge;

                if (patient_balance.length > 0) {
                    schcheckin.params.PracticeId = patient_balance[0].PracticeId;
                    $('#schcheckin #txtPatientBalance').val(parseFloat(Number(patient_balance[0].PatientBalance)).toFixed(2));
                    //$('#schcheckin #txtAdvanceBalance').val(patient_balance[0].AdvanceBalance);
                    $('#schcheckin #txtPlanBalance').val(parseFloat(Number(patient_balance[0].InsuranceBalance)).toFixed(2));
                    if (patient_balance[0].PatientBalance != "") {
                        $('#schcheckin #lblPatBalance').css("display", "none");
                        $('#schcheckin #divPatientLedgerChkIn').css("display", "inline");
                        var PatientBalanceMarkup = "<a href='#' onclick=schcheckin.OpenOutstandingVisit('" + schcheckin.params.PatientId + "','1');>Patient Balance</a>";
                        $("#schcheckin #divPatientLedgerChkIn").html(PatientBalanceMarkup);
                    }
                } else {
                    schcheckin.params.PracticeId = "-1";
                }

                if (copayments.length != '0' || copayments.length != 0) {

                    $.each(copayments, function (i, item) {
                        if (copayments[i].PaidAmountDr != "" && copayments[i].IsRefund != 'True') {

                            $('#schcheckin #chkPaid').attr('checked', true);

                            return false;
                        }
                    });
                }

                else if (copayments.length == '0' || copayments.length == 0) {

                    $('#schcheckin #chkPaid').attr('checked', false);
                }


                $('#schcheckin #hfRefProvider').val(patientappointments[0].RefProviderId);
                $('#schcheckin #txtRefProvider').val(patientappointments[0].RefProviderName);

                $('#schcheckin #hfProviderId').val(patientappointments[0].ProviderId);
                $('#schcheckin #txtProvider').val(patientappointments[0].ProviderName);

                $('#schcheckin #hfFacilityId').val(patientappointments[0].FacilityId);
                $('#schcheckin #txtFacility').val(patientappointments[0].FacilityName);

                //$('#schcheckin #hfSchReasonId').val(patientappointments[0].SchReasonId);
                //$('#schcheckin #txtReason').val(patientappointments[0].SchReasonId);

                $('#schcheckin #hfSchStatusId').val(patientappointments[0].SchStatusId);

                $('#schcheckin #txtReason').val(patientappointments[0].ReasonComments);

                if (patientappointments[0].IsSpecialist == 'True')
                    $('#schcheckin #rdSpecialist').attr("checked", "checked");
                else
                    $('#schcheckin #rdPCP').attr("checked", "checked");

                var insuranceid = patientappointments[0].PatientInsuranceId;
                //Start 22-08-2016 Humaira Yousaf for referral Id
                $('#schcheckin #hfReferralId').val(patientappointments[0].ReferralId);
                if (patientappointments[0].ReferralId != null && patientappointments[0].ReferralId != '') {
                    $('#schcheckin #btnAddReferral').text('View Referral');
                }
                else {
                    $('#schcheckin #btnAddReferral').text('Add Referral');
                }
                //End 22-08-2016 Humaira Yousaf for referral Id

                $('#schcheckin #txtCopayment').val(parseFloat(Number(patientappointments[0].Copayment)).toFixed(2));

                var self = $("#schcheckin");
                utility.bindMyJSON(true, appointment_detail, false, self).done(function () {
                    utility.AutoEnableAutoCompleteLink($('#schcheckin #frmcheckin'));
                    schcheckin.ValidatePatientCheckIn();
                    //serialize Data after all controls loaded.
                    $('#schcheckin #frmcheckin').data('serialize', $('#schcheckin #frmcheckin').serialize());

                });

                schcheckin.BindInsurancePlans(response.PatientInsuranceDetail_JSON, insuranceid);

            }

            else {
                utility.DisplayMessages(response.Message, 3);
                BackgroundLoaderShow(false);
            }

        });
        //}
    },

    BindClinicalTemplate: function (templateJson) {
        var templateJsonDetail = JSON.parse(templateJson);
        $("#frmcheckin #ddlClinicalTemplate").empty();
        $("#frmcheckin #ddlClinicalTemplate").append($('<option/>', {
            value: "",
            html: "- SELECT -"
        }));
        $.each(templateJsonDetail, function (i, item) {
            $("#frmcheckin #ddlClinicalTemplate").append(
                $('<option />', {
                    value: item.TemplateId,
                    html: item.ShortName
                })
            );
        });
    },

    BindInsurancePlans: function (InsuranceJSON, InsuranceID) {
        if (InsuranceID == "") {
            $("#copayy").css("display", "none");
            $("#txtCopayment").attr("disabled", "disabled");
        }
        //Resolved By Mohsin Nasir Bug # PMS-2879
        var InsuranceJSON_detail = $.grep(JSON.parse(InsuranceJSON), function (obj) {
            if (obj.IsActive.toLowerCase() == 'true') {
                return obj;
            }
        });
        //END Resolved By Mohsin Nasir Bug # PMS-2879
        $("#frmcheckin #ddlInsurancePlan").empty();
        $("#frmcheckin #ddlInsurancePlan").append($('<option/>', {
            value: "",
            html: "- SELECT -",
            priority: "",
            coPayment: "",
            specialistCopay: ""
        }));
        $.each(InsuranceJSON_detail, function (i, item) {
            $("#frmcheckin #ddlInsurancePlan").append(
                $('<option />', {
                    value: item.InsuranceId,
                    html: item.InsurancePlanName,
                    priority: item.PlanPriority,
                    coPayment: item.AmtCopay,
                    specialistCopay: item.SpecialistCopay
                })
            );

        });

        for (var i = 0; i < InsuranceJSON_detail.length; i++) {

            if (InsuranceID == InsuranceJSON_detail[i].InsuranceId) {

                $('#schcheckin #ddlInsurancePlan').val(InsuranceJSON_detail[i].InsuranceId);
                if ($('#schcheckin #rdPCP').is(':checked')) {
                    //$('#schcheckin #txtCopayment').val(parseFloat(Number(InsuranceJSON_detail[i].AmtCopay)).toFixed(2));
                }
                else if ($('#schcheckin #rdSpecialist').is(':checked')) {
                    //$('#schcheckin #txtCopayment').val(parseFloat(Number(InsuranceJSON_detail[i].SpecialistCopay)).toFixed(2));
                }

            }
        }

    },

    FillInsuranceData: function (control, controlToChange) {


        var coPayment = $("#frmcheckin #ddlInsurancePlan option:selected").attr("coPayment");
        var specialistCopay = $("#frmcheckin #ddlInsurancePlan option:selected").attr("specialistCopay");

        if ($('#schcheckin #rdPCP').is(':checked')) {
            $("#frmcheckin #txtCopayment").val(parseFloat(Number(coPayment)).toFixed(2));
        }
        else if ($('#schcheckin #rdSpecialist').is(':checked')) {
            $("#frmcheckin #txtCopayment").val(parseFloat(Number(specialistCopay)).toFixed(2));
        }

    },

    ValidatePatientCheckIn: function () {
        $('#frmcheckin')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   //name: {
                   //    group: '.col-sm-4',
                   //    validators: {
                   //        notEmpty: {
                   //            message: ''
                   //        }
                   //    }
                   //}
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();
            schcheckin.PatientCheckInSave();
        });
    },

    PatientCheckInSave: function () {
        var selectplanId = $("#frmcheckin #ddlInsurancePlan option:selected").val();
        var insuranceplanlength = $("#frmcheckin #ddlInsurancePlan option").length;
        //PRD-112
        if (schcheckin.params["PatientReferralId"]) {
            //schcheckin.LoadPatientRefrralInfo(schcheckin.params["PatientReferralId"]);
            var PatientRerralId = schcheckin.params["PatientReferralId"];
            Patient_Referral.FillReferral(PatientRerralId).done(function (response) {
                if (response.status != false) {
                    var ReferralDetail = JSON.parse(response.ReferralFill_JSON);
                    var visitAllowd = parseInt(ReferralDetail.txtVisitsAllowed);
                    var visitUsed = parseInt(ReferralDetail.txtVisitsUsed);
                    if (visitUsed >= visitAllowd) {
                        utility.myConfirm('Referral/Prior Auth allowed visits are used. Do you want to continue with same referral?', function () {
                            $("#frmcheckin #hfisAllVisitUsed").val(false);
                            schcheckin.InusranceAlert(selectplanId, insuranceplanlength);
                            // if yes then no need to detacch patient refrra 
                           
                        }
                    , function () {
                        // if user clicks on no then dettach Refrall number and PatientRefrral Id 
                        $("#frmcheckin #hfisAllVisitUsed").val(true);
                        schcheckin.InusranceAlert(selectplanId, insuranceplanlength);
                       
                        // Patient_Insurance.CheckEligiblity(InsuranceResponse);
                    },
                   'ALERT'
                       //
                   );
                    }
                    else {
                           $("#frmcheckin #hfisAllVisitUsed").val(false);
                        schcheckin.InusranceAlert(selectplanId, insuranceplanlength);
                      
                    }

                } // end responce.


            });
        }
        else {
            schcheckin.InusranceAlert(selectplanId, insuranceplanlength);
            $("#frmcheckin #hfisAllVisitUsed").val(false);
        }
       

    },
    InusranceAlert: function (selectplanId, insuranceplanlength) {
        if (!selectplanId && insuranceplanlength > 1) {
            /****** Confirm Dialog start*******/
            utility.myConfirm('No insurance has been associated with this appointment. Do you want to select?', function () {
                $("#frmcheckin #ddlInsurancePlan").prop("disabled", false);
            }
            , function () {
                schcheckin.OpenCheckinReason();
            },
                 'Confirm Check in'
            );
        }
        else {
            schcheckin.CheckinAppointment();
        }
    },
    OpenCheckinReason: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["ParentCtrl"] = "schcheckin";
        LoadActionPan('Scheduling_CheckInReason', params);
    },
    CheckinAppointment: function (reasonComments) {
        if (reasonComments) {
            $('#frmcheckin #hfCheckInReason').val(reasonComments);
        }
        var strMessage = "";
        var self = $("#schcheckin");
        var myJSON = self.getMyJSON();

        AppPrivileges.GetFormPrivileges("Appointment", "EDIT", "FORM_PRIVILEGE", "GET_FORM_PRIVILEGE", function (strMessage) {
            if (strMessage == "") {
                if (schcheckin.params["ParentCtrl"] == "schTabCalendar" && $("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                    var ResourceproviderId = $('#pnlScheduleCalendar #Resource option:selected').attr('refname').split('-')[0]
                } else {
                    var ResourceproviderId = null;
                }
                //PMS-1721 improvement
                //if (schcheckin.params["ParentCtrl"] == "schTabMultipleView" && schcheckin.params["ResourceId"]) {
                //    var ResourceproviderId = null;
                //    var resourceDetail = null;
                //    $('#pnlScheduleCalendar #Resource option').each(function (e) {
                //        if ($(this).val() == schcheckin.params["ResourceId"]) {
                //            resourceDetail = $(this).attr("refname");
                //            return false;
                //        }
                //    });
                //    if (resourceDetail != null) {
                //        ResourceproviderId = resourceDetail.split("-")[0];
                //    }
                //}
                schcheckin.SavePatientCheckIn(myJSON, ResourceproviderId).done(function (response) {
                    if (response.status != false) {
                        //Admin_AppointmentStatus.AppointmentStatusSearch(response.AppointmentId);
                        utility.DisplayMessages(response.message, 1);
                        //PMS-4770
                        UnloadActionPan(schcheckin.params["ParentCtrl"], "schcheckin");
                        //var dayrgx = /(\d{2})\/(\d{2})\/(\d{4})/;
                        ////expression for week range
                        //var weekrg = /(\d{2})\/(\d{2})\/(\d{4}) - (\d{2})\/(\d{2})\/(\d{4})/;
                        ////Month Regular Expression
                        //var monthreg = /(\January|February|March|April|May|June|July|August|September|October|November|December)\/(\d{4})/;


                        if (schcheckin.params.PanelID == "pnlBillUnClaimedAppointment") {

                            Bill_UnClaimedAppointment.UnClaimedAppSearch();

                        }
                        else {
                            schcheckin.UpdateAppointmentOnScheduler(response.VisitId);
                        }
                        //else if (schcheckin.params.PanelID == "schEditSlot") {
                        //    schEditSlot.SelectSlotDetail(schcheckin.params.TimeslotDetailid, schcheckin.params.ProviderId, schcheckin.params.ResourceId);

                        //    if (schcheckin.params.MultipleView == 1) {

                        //        var providerid = schcheckin.params.ProviderId;
                        //        var facilityid = schcheckin.params.FacilityId;
                        //        var resourceid = schcheckin.params.ResourceId;
                        //        var date = schcheckin.params.DayDate;
                        //        var dateid = schcheckin.params.DateId;

                        //        Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);

                        //    }

                        //    else if (schcheckin.params.DayDate.match(weekrg) && schcheckin.params.DayDate.length > 15) {
                        //        var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                        //        Scheduling_Calendar.ClearTable();

                        //        var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                        //        var week = Scheduling_Calendar.GetWeek(date1);
                        //        $('#pnlScheduleCalendar #daydate span').html(week);
                        //        //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                        //        var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                        //        $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                        //    }
                        //    else {
                        //        var statusslots = Scheduling_Calendar.FilterCriteria();
                        //        Scheduling_Calendar.DayCalendar(schcheckin.params.ProviderId, schcheckin.params.ResourceId, schcheckin.params.FacilityId, schcheckin.params.DayDate, statusslots);
                        //        if (schcheckin.params.ProviderId != "")
                        //            $("#pnlScheduleCalendar #Provider option[value=" + schcheckin.params.ProviderId + "]").attr('selected', 'selected');
                        //        if (schcheckin.params.ResourceId != "")
                        //            $("#pnlScheduleCalendar #Resource option[value=" + schcheckin.params.ResourceId + "]").attr('selected', 'selected');
                        //        if (schcheckin.params.FacilityId != "")
                        //            $("#pnlScheduleCalendar #Facility option[value=" + schcheckin.params.FacilityId + "]").attr('selected', 'selected');
                        //        if (schcheckin.params.DayDate != "")
                        //            $('#pnlScheduleCalendar #daydate span').html(schcheckin.params.DayDate);
                        //    }


                        //}
                        //else if (schcheckin.params.PanelID == "pnlScheduleMuliView") {

                        //    var providerid = schcheckin.params.ProviderId;
                        //    var facilityid = schcheckin.params.FacilityId;
                        //    var resourceid = schcheckin.params.ResourceId;
                        //    var date = schcheckin.params.DayDate;
                        //    var dateid = schcheckin.params.DateId;

                        //    Scheduling_MuliView.BackDate(dateid, providerid, resourceid, facilityid, 0);


                        //}

                        //else if (schcheckin.params.DayDate.match(weekrg) && schcheckin.params.DayDate.length > 15) {
                        //    var a = $('#pnlScheduleCalendar #fromdate').datepicker('getDate');
                        //    Scheduling_Calendar.ClearTable();

                        //    var date1 = $.datepicker.formatDate('yy/mm/dd', new Date(a));
                        //    var week = Scheduling_Calendar.GetWeek(date1);
                        //    $('#pnlScheduleCalendar #daydate span').html(week);
                        //    //$('#pnlScheduleCalendar #fromdate').datepicker("setDate", date1);
                        //    var date2 = $.datepicker.formatDate('mm/dd/yy', new Date(a));
                        //    $('#pnlScheduleCalendar #fromdate').datepicker("setDate", date2);
                        //}

                        //else {
                        //    var statusslots = Scheduling_Calendar.FilterCriteria();

                        //    //Handle resource provider case
                        //    if ($("#pnlScheduleCalendar input:radio[name='RadioProvider']").is(":checked")) {
                        //        Scheduling_Calendar.DayCalendar(schcheckin.params.ProviderId, null, schcheckin.params.FacilityId, schcheckin.params.DayDate, statusslots);
                        //    }
                        //    else if ($("#pnlScheduleCalendar input:radio[name='RadioResource']").is(":checked")) {
                        //        Scheduling_Calendar.DayCalendar(null, schcheckin.params.ResourceId, schcheckin.params.FacilityId, schcheckin.params.DayDate, statusslots);
                        //    }

                        //    // Scheduling_Calendar.DayCalendar(schcheckin.params.ProviderId, schcheckin.params.ResourceId, schcheckin.params.FacilityId, schcheckin.params.DayDate, statusslots);
                        //    if (schcheckin.params.ProviderId != "")
                        //        $("#Provider option[value=" + schcheckin.params.ProviderId + "]").attr('selected', 'selected');
                        //    if (schcheckin.params.ResourceId != "")
                        //        $("#Resource option[value=" + schcheckin.params.ResourceId + "]").attr('selected', 'selected');
                        //    if (schcheckin.params.FacilityId != "")
                        //        $("#Facility option[value=" + schcheckin.params.FacilityId + "]").attr('selected', 'selected');
                        //    if (schcheckin.params.DayDate != "")
                        //        $('#pnlScheduleCalendar #daydate span').html(schcheckin.params.DayDate);

                        //}
                        //MDVisionService.reloadLookups = true;
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

    UpdateAppointmentOnScheduler: function(visitId) {

        PMSScheduler.CanScheduler.dataSource.read();
        //var dataSourceApp = PMSScheduler.CanScheduler.dataSource;
        //var ap = dataSourceApp._data.filter(f => f.AppointmentId == schcheckin.params.AppointmentId)[0];
        //ap.VisitId = visitId;
        //ap.AppointmentStatus = "Check In";
        ////ap.SchStatusId = StatusId;
        ////ap.StatusColor = color;
        //PMSScheduler.CanScheduler.dataSource.pushUpdate(ap);

    },

    //------Referring Provider Region--------------
    ResetRefProvider: function () {

        $('#schcheckin #hfRefProvider').val(null);

    },

    OpenRefProvider: function () {
        var params = [];
        params["ReferringProviderId"] = "-1";
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";

        params["ParentCtrl"] = "schcheckin";
        LoadActionPan('Admin_ReferringProvider', params);
    },
    OpenRefProviderDetail: function () {
        //Admin_ReferringProvider.ReferringProviderEdit($('#pnlDemographic #hfRefProvider').val(), "patTabDemographic", "txtRefProvider");
        var params = [];
        params["ReferringProviderId"] = $('#schcheckin #hfRefProvider').val();
        params["FromAdmin"] = "0";
        params["RefCtrl"] = "txtRefProvider";
        params["mode"] = "Edit";
        params["ParentCtrl"] = "schcheckin";

        LoadActionPan('referringproviderDetail', params);
    },
    //---------End Region--------------------------

    OpenCoPayment: function () {

        var params = [];
        params["FromAdmin"] = "0";
        params["PatientId"] = schcheckin.params.PatientId;
        params["ProviderId"] = $("#frmcheckin #hfProviderId").val(); //schcheckin.params.ProviderId;
        params["FacilityId"] = $("#frmcheckin #hfFacilityId").val();//schcheckin.params.FacilityId;
        params["ResourceId"] = schcheckin.params.ResourceId;
        params["AppointmentId"] = schcheckin.params.AppointmentId;
        params["ParentCtrl"] = 'schcheckin';
        LoadActionPan('schcopayment', params);

    },

    FillPatientData: function (PatientId, AppointmentId) {
        var data = "PatientId=" + PatientId + "&AppointmentId=" + AppointmentId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_CHECKIN", "FILL_PATIENT_DATA");
    },

    SavePatientCheckIn: function (PatientCheckInData, ResourceproviderId) {
       
        var data = "PatientCheckInData=" + PatientCheckInData + "&ReferralId=" + $('#schcheckin #hfReferralId').val() + "&ResourceproviderId=" + ResourceproviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_CHECKIN", "SAVE_PATIENT_CHECKIN");
    },

    CancelPatCheckIn: function (VisitId, AppointmentId, statusId, status) {

        //var AppointmentId = schcheckin.params.AppointmentId;

        var data = "VisitId=" + VisitId + "&AppointmentId=" + AppointmentId + "&StatusId=" + statusId + "&Status=" + status;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "SCHEDULING_CHECKIN", "CANCEL_PATIENT_CHECKIN");
    },

    ProviderSearch: function (ProviderId) {

        schcheckin.SearchProvider("", ProviderId).done(function (response) {
            if (response.status != false) {
                if (response.ProviderCount > 0) {
                    var ProviderLoadJSONData = JSON.parse(response.ProviderLoad_JSON);
                    if (ProviderLoadJSONData[0].IsSpecialist == "False") {
                        $('#schcheckin #rdPCP').attr("checked", "checked");
                    }
                    else
                        $('#schcheckin #rdSpecialist').attr("checked", "checked");
                }
            }
            else {
                utility.DisplayMessages(response.Message, 3);
            }
        });

    },

    SearchProvider: function (ProviderData, ProviderId) {
        var data = "ProviderData=" + ProviderData + "&ProviderID=" + ProviderId;
        // serach parameter , class name, command name of class 
        return MDVisionService.defaultService(data, "ADMIN_PROVIDER", "SEARCH_PROVIDER");
    },

    UnLoad: function () {

        utility.UnLoadDialog('schcheckin #frmcheckin', function () {
            UnloadActionPan(schcheckin.params["ParentCtrl"], "actionPanCheckIn");
        }, function () {
            UnloadActionPan(schcheckin.params["ParentCtrl"], "actionPanCheckIn");
        });


    },

    GetPatientLedgerMarkUp: function () {
        var PatientBalanceMarkup = "<a href='#' onclick=Patient_Demographic.OpenOutstandingVisit('" + selectedPatientId + "','1');>" + PatBalance + "</a>";
        $("#PatientProfile #PatBalance").html("<strong>Pat. Bal:</strong> " + PatientBalanceMarkup);
    },

    OpenOutstandingVisit: function (patientID, PatientOutstanding) {
        var params = [];

        params["PatientOutstanding"] = PatientOutstanding;
        params["patientID"] = patientID;
        params["PatientAccountNo"] = schcheckin.params.txtAccountNumber;
        params["PatientFirstName"] = schcheckin.params.txtFirstName;
        params["PatientLastName"] = schcheckin.params.txtLastName;
        params["patientAge"] = schcheckin.params.txtAge;
        params["PatientSex"] = schcheckin.params.txtGender;
        params["PatientDOB"] = schcheckin.params.txtDOB;
        params["PatientHomeTel"] = schcheckin.params.txtHomePhoneNo;
        params["PatBanner"] = false;
        params["ParentCtrl"] = "schcheckin";
        params["PracticeId"] = schcheckin.params.PracticeId;

        LoadActionPan('Patient_Ledger', params)
    },
    //Function Name: addReferral
    //Author: Humaira Yousaf
    //Date :  22-08-2016
    //Description: Adds Referral
    addReferral: function () {
        var params = [];
        params["PatientId"] = schcheckin.params.PatientId;
        params["ReferralTo"] = $('#schcheckin #txtProvider').val();
        params["ReferralToId"] = schcheckin.params.ProviderId;
        params["Reason"] = $('#schcheckin #txtReason').val();
        params["Status"] = 1; // Not Started    
        params["ParentCtrl"] = "schcheckin";

        if ($('#schcheckin #btnAddReferral').text() == 'Add Referral') {
            params["mode"] = "Add";
        }
        else {
            params["mode"] = "Edit";
            params["ReferralId"] = $('#schcheckin #hfReferralId').val();
        }
        LoadActionPan('Patient_Referrals_Incoming_Detail', params);
    },
    OpenUnallocatedCopayment: function () {
        var params = [];
        params["FromAdmin"] = "0";
        params["PatientId"] = schcheckin.params.PatientId;
        params["ProviderId"] = schcheckin.params.ProviderId;
        params["FacilityId"] = $("#frmcheckin #hfFacilityId").val();
        params["ResourceId"] = schcheckin.params.ResourceId;
        params["AppointmentId"] = schcheckin.params.AppointmentId;
        params["VisitId"] = schcheckin.params.PatientVisitId ? schcheckin.params.PatientVisitId : 0;
        params["ParentCtrl"] = 'schcheckin';
        LoadActionPan('Scheduling_UnallocatedCopayment', params);
    }
}