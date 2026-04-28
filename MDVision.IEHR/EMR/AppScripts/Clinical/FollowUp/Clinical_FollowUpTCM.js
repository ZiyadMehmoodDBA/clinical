Clinical_FollowUpTCM = {
    bIsFirstLoad: true,
    params: [],
    scheduledSlots: [],
    Load: function (params) {
        Clinical_FollowUpTCM.params = params;
        Clinical_FollowUpTCM.scheduledSlots = [];

        //VisitDateForFollowUp
        utility.CreateDatePicker('pnlClinicalFollowUpTCM #fromDate', function () {

            Clinical_FollowUpTCM.ValidateVisitDate();
            Clinical_FollowUpTCM.UserChangeDate();

        }, true);



        if ($("#PatientProfile #hfDischargeDate").val() != "" && $("#PatientProfile #hfDischargeDate").val() != null && $('#' + Clinical_ProgressNote.params["PanelID"] + " #frmClinicalProgressNote #lblNotesReason").text().toLowerCase() == "transitional care management" && Clinical_ProgressNote.params.IsPhoneEncounter == true && Clinical_ProgressNote.params.TemplateName.toLowerCase() == "phone encounter tcm") {
            Clinical_FollowUpTCM.params["IsTCM"] = true;
        } else {
            Clinical_FollowUpTCM.params["IsTCM"] = false;
        }
        if (Clinical_FollowUpTCM.params["IsTCM"] == false) {
            $('#pnlClinicalFollowUpTCM #fromTime').timepicker().on('changeTime.timepicker', function (e) {
                disableFocus: false
                $('#frmClinicalFollowUpTCM').bootstrapValidator('revalidateField', 'Time');
            });
        }


        //$('#pnlClinicalFollowUpTCM #fromDate').datepicker({
        //    //defaultTime: '12:00 AM'
        //});

        // set to empty on load.
        $('#pnlClinicalFollowUpTCM #fromTime').val('');
        $('#pnlClinicalFollowUpTCM #hfPatientId').val(Clinical_FollowUpTCM.params.patientID);


        var self = $('#pnlClinicalFollowUpTCM');
        self.loadDropDowns(true).done(function () {

            if (Clinical_FollowUpTCM.params.mode == "Edit") {

                Clinical_FollowUpTCM.AppointmentFill();
            } else {
                $('#pnlClinicalFollowUpTCM #Provider').val(Clinical_FollowUpTCM.params.CurrentNotesProviderId);
                $('#pnlClinicalFollowUpTCM #Facility').val(Clinical_FollowUpTCM.params.CurrentNotesFacilityId);
                Clinical_FollowUpTCM.AvailableAppointmentFill();
            }

            Clinical_FollowUpTCM.ValidateFollowUpAppointment();
        });

        //Followup Appointments Provider Schedule Slot Implementation IMP-923
        Clinical_FollowUpTCM.SetUpFollowUpAppointmentEvents();


        $("#frmClinicalFollowUpTCM .submitbtns").click(function (e) {
            Clinical_FollowUpTCM.params.ActionId = $(this).attr("id");
        });

        $("#frmClinicalFollowUpTCM #txtScheduleAfter").keyup(function (evt) {
            this.value = this.value.substring(0, 3);
            this.value = this.value.replace(/\D/g, '');
        });

        utility.callbackAfterAllDOMLoaded(function () {

            //Serialization
            $('#' + Clinical_FollowUpTCM.params.PanelID + ' #frmClinicalFollowUpTCM').data('serialize', $('#' + Clinical_FollowUpTCM.params.PanelID + ' #frmClinicalFollowUpTCM').serialize());

        });
    },

    SetUpFollowUpAppointmentEvents: function () {

        $("#pnlClinicalFollowUpTCM #fromTime").addClass("disableAll");
        $('#pnlClinicalFollowUpTCM #fromTime').prop('readonly', true);
        //if (Clinical_FollowUpTCM.params["IsTCM"] == true) {
        //    $("#pnlClinicalFollowUpTCM #Duration").addClass("disableAll");
        //    $("#pnlClinicalFollowUpTCM #fromTime").addClass("disableAll");
        //    $('#pnlClinicalFollowUpTCM #fromTime').prop('readonly', true);
        //    $("#pnlClinicalFollowUpTCM #Duration").prop('readonly', true);
        //}
        //else {
        //    $("#pnlClinicalFollowUpTCM #Duration").removeClass("disableAll");
        //    $("#pnlClinicalFollowUpTCM #fromDate").removeClass("disableAll");
        //    $("#pnlClinicalFollowUpTCM #fromTime").removeClass("disableAll");
        //    $('#pnlClinicalFollowUpTCM #fromTime').prop('readonly', false);
        //    $("#pnlClinicalFollowUpTCM #Duration").prop('readonly', false);
        //}

    },

    GetAppointmentSlotInfo_DBCall: function (FacilityId, ProviderId, Duration, ctype, cval) {

        var objData = {};
        objData["Facility"] = FacilityId;
        objData["Provider"] = ProviderId;
        objData["Duration"] = Duration;
        objData["SlotType"] = ctype;
        objData["SlotValue"] = cval;
        objData["commandType"] = "Get_Appointment_SlotInfo";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FOLLOWUP", "ClinicalFollowUp");
    },

    AppointmentFill: function () {
        Clinical_FollowUpTCM.FillAppointment().done(function (response) {

            var response = JSON.parse(response);
            if (response.status != false) {
                var self = $('#pnlClinicalFollowUpTCM');

                var fillData = JSON.parse(response.AppointmentFill_JSON);

                utility.bindMyJSONByName(true, fillData, false, self);

                $('#pnlClinicalFollowUpTCM #fromDate,#Provider,#Provider,#Facility').prop('disabled', true);
                $('#pnlClinicalFollowUpTCM #btnSchedule').text('Reschedule');
            }
            else {
                //utility.DisplayMessages(response.Message, 2);
            }

            Clinical_FollowUpTCM.AvailableAppointmentFill();

        })

    },

    AvailableAppointmentFill: function () {

        Clinical_FollowUpTCM.loadAvailableAppointments().done(function (response) {

            var response = JSON.parse(response);
            if (response.AvailableAppointmentsCount > 0) {
                var a = null;
                var b = null;
                var c = null;
                var AppointmentDetail = JSON.parse(response.AvailableAppointmentsLoad_JSON);
                $('#pnlClinicalFollowUpTCM #dvisPatientScheduled').removeClass('hidden');
                $("#pnlClinicalFollowUpTCM #dvisPatientScheduled").empty();
                $.each(AppointmentDetail, function (i, item) {
                    var labeltext = 'Patient is scheduled for ' + item.AppointmentDate + ", " + item.TimeFrom;
                    //if (labeltext.indexOf('Patient is scheduled for') > -1) {
                    //    labeltext += (item.Reason != "" ? ", " + item.Reason : "")
                    //}
                    a = item.ProviderId;
                    b = item.FacilityId;
                    c = item.Date;

                    var dt = item.Date; // AppointmentDetail[0].Date;
                    var year = dt.split('-')[0];
                    var month = dt.split('-')[1];
                    var day = dt.split('-')[2];
                    var finalDt = month + '/' + day + '/' + year;
                    //$("#pnlClinicalFollowUpTCM #txtReasonComments").val(item.Reason);

                    $("#pnlClinicalFollowUpTCM #dvisPatientScheduled").append('<div class="checkbox-custom checkboxTiny"><input type="checkbox" name="Slot" onclick="Clinical_FollowUpTCM.changeTime(this);" id="chkisPatientScheduled' + i + '"><label class="blue" id="lblisPatientScheduled">' + labeltext + '</label></div>');
                    $("#pnlClinicalFollowUpTCM #chkisPatientScheduled" + i).val(item.TimeFrom);
                    Clinical_FollowUpTCM.scheduledSlots.push(item.TimeFrom);

                });
                setTimeout(function () { Clinical_FollowUpTCM.AvailableSlotsFill(a, b, c); }, 500);
            } else {
                $('#pnlClinicalFollowUpTCM #dvisPatientScheduled').addClass('hidden');
                Clinical_FollowUpTCM.scheduledSlots = [];
                Clinical_FollowUpTCM.AvailableSlotsFill(null, null, null);
            }


        });

    },

    UserChangeDate: function () {

        //if (Clinical_FollowUpTCM.params["IsUserChangeDate"] == true) {

        var addDays = $("#pnlClinicalFollowUpTCM #txtScheduleAfter").val();
        var date_ = $("#pnlClinicalFollowUpTCM #fromDate").val();
        if (date_ != "") {

            var diff = utility.getDateDiff(date_, new Date(), "days");
            if (diff < 0)
                $("#pnlClinicalFollowUpTCM #txtScheduleAfter").val(Math.abs(diff));
            else
                $("#pnlClinicalFollowUpTCM #txtScheduleAfter").val(0);

            $("#pnlClinicalFollowUpTCM #txtScheduleAfter").trigger("change");
        }

        Clinical_FollowUpTCM.AvailableAppointmentFill();
        // }
        //else
        //    Clinical_FollowUpTCM.SetUserChangeFocus(false);
    },

    SetUserChangeFocus: function (IsUserChangeDate) {

        Clinical_FollowUpTCM.params["IsUserChangeDate"] = IsUserChangeDate;
    },

    SetValue: function ($obj) {

        var value_ = $("#pnlClinicalFollowUpTCM #txtFreetext").val();
        var obj_value = $($obj).val();
        obj_value = obj_value != "" ? obj_value : "0";
        var index_ = value_.indexOf(':');
        if (index_ > 0) {
            var sub_ = value_.substring(index_ + 1);
            $("#pnlClinicalFollowUpTCM #txtFreetext").val(obj_value + " Day(s):" + sub_);
        }
        else {
            $("#pnlClinicalFollowUpTCM #txtFreetext").val(obj_value + " Day(s): ");
        }

    },

    changeDate: function () {


        var addDays = $("#pnlClinicalFollowUpTCM #txtScheduleAfter").val();
        if (addDays != "" && addDays != null) {

            addDays = parseInt(addDays);
            var dat = new Date();
            dat.setDate(dat.getDate() + addDays);
            // Clinical_FollowUpTCM.SetUserChangeFocus(false);
            $("#pnlClinicalFollowUpTCM #fromDate").datepicker("setDate", dat);
            Clinical_FollowUpTCM.AvailableAppointmentFill();
        }
    },

    AvailableSlotsFill: function (providerId, facilityId, Date) {
        Clinical_FollowUpTCM.loadAvailableSlots(providerId, facilityId, Date).done(function (response) {

            var response = JSON.parse(response);
            $("#" + Clinical_FollowUpTCM.params.PanelID + " #ulSlots tbody tr").remove();
            if (response.AvailableSlotsCount > 0) {

                var SlotsDetail = JSON.parse(response.AvailableSlotsLoad_JSON);
                if (SlotsDetail[0].FreeSlots < 1) {
                    $("#pnlClinicalFollowUpTCM #lblError").css("display", "block");
                    $("#pnlClinicalFollowUpTCM #lblError").text('SLOTS NOT AVAILABLE');
                    $("#pnlClinicalFollowUpTCM #dvisPatientScheduled").addClass('hidden');
                    return;
                } else {
                    $("#pnlClinicalFollowUpTCM #lblError").css("display", "none");

                    var finalTr = '';
                    var counter = 2;

                    $.each(SlotsDetail, function (i, item) {
                        var isAlreadyScheduled = false;
                        $.each(Clinical_FollowUpTCM.scheduledSlots, function (ind, val) {

                            if (item.SlotTime == val) {
                                isAlreadyScheduled = true;
                            }

                        });

                        if (!isAlreadyScheduled) {

                            if ($(finalTr).find('input').length % 4 == 0) {
                                finalTr = finalTr + '<tr>';
                            }
                            finalTr = finalTr + '<td><div class="col-xs-6 p-xs"><div class="checkbox-custom">';
                            finalTr = finalTr + '<input  type="checkbox" onclick="Clinical_FollowUpTCM.changeTime(this);" name="Slot" value="' + item.SlotTime + '"  >';
                            finalTr = finalTr + '   <label class="control-label">' + item.SlotTime + '</label></div></div></td>';

                            if ($(finalTr).find('input').length % 4 == 0) {
                                finalTr = finalTr + '</tr>';
                            }

                            if ($("#pnlClinicalFollowUpTCM #Duration").val() == "")
                                $("#pnlClinicalFollowUpTCM #Duration").val(item.SlotMinutes);
                        }
                    });



                    $("#" + Clinical_FollowUpTCM.params.PanelID + " #ulSlots tbody").append(finalTr);

                }

            } else {

                $("#pnlClinicalFollowUpTCM #lblError").css("display", "block");
            }


        });

    },

    changeTime: function (obj) {
        $("#pnlClinicalFollowUpTCM #ChooseSchedule").addClass('hidden');
        $("#pnlClinicalFollowUpTCM #fromTime").val($(obj).val());
        $('#dvSlotsDetail input').each(function () {
            if ($(this).val() != $(obj).val()) {
                $(this).prop('checked', false);
            }

        });
    },

    FillAppointment: function () {

        var objData = {};
        objData["AppointmentID"] = Clinical_FollowUpTCM.params.FollowUpAppointmentId;
        objData["commandType"] = "fill_followup_appointment";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FOLLOWUP", "ClinicalFollowUp");

    },

    loadAvailableAppointments: function () {

        var objData = {};
        objData["Date"] = $("#pnlClinicalFollowUpTCM #fromDate").val();
        objData["PatientId"] = $('#PatientProfile #hfPatientId').val();
        objData["Provider"] = $("#pnlClinicalFollowUpTCM #Provider").val();
        objData["Facility"] = $("#pnlClinicalFollowUpTCM #Facility").val();
        objData["commandType"] = "load_patient_appointment";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FOLLOWUP", "ClinicalFollowUp");

    },

    loadAvailableSlots: function (providerId, facilityId, dt) {

        var objData = {};
        if (providerId != null) {
            objData["Provider"] = providerId;
        } else {
            objData["Provider"] = $("#pnlClinicalFollowUpTCM #Provider").val();
        }
        if (facilityId != null) {
            objData["Facility"] = facilityId;
        } else {
            objData["Facility"] = $("#pnlClinicalFollowUpTCM #Facility").val();
        }
        if (dt != null) {
            objData["Date"] = dt;
        } else {

            var dat = new Date();
            var frmDate = (dat.getMonth() + 1) + "/" + dat.getDate() + "/" + dat.getFullYear();
            objData["Date"] = $("#pnlClinicalFollowUpTCM #fromDate").val() == "" ? frmDate : $("#pnlClinicalFollowUpTCM #fromDate").val();
            if ($("#pnlClinicalFollowUpTCM #fromDate").val() == "") {
                // Clinical_FollowUpTCM.SetUserChangeFocus(false);
                $("#pnlClinicalFollowUpTCM #fromDate").datepicker('setDate', objData["Date"]);
            }
        }

        objData["commandType"] = "load_provider_available_slots";
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FOLLOWUP", "ClinicalFollowUp");

    },

    ValidateFollowUpAppointment: function () {
        $('#frmClinicalFollowUpTCM')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {
                   Date: {
                       group: '.col-sm-4',
                       enabled: true,
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Provider: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
                   Duration: {
                       group: '.col-sm-4',
                       validators: {
                           notEmpty: {
                               message: ''
                           }
                       }
                   },
               }
           })
        .on('success.form.bv', function (e) {
            e.preventDefault();

            if (Clinical_FollowUpTCM.params.ActionId == "btnSave")
                Clinical_FollowUpTCM.FollowUpAppointmentSave();
            else if (Clinical_FollowUpTCM.params.ActionId == "btnSchedule")
                Clinical_FollowUpTCM.FollowUpAppointmentSchedule();
            else
                utility.DisplayMessages('Unknow action.', 3);
        });
    },

    FollowUpAppointmentSave: function () {

        var value_ = $("#" + Clinical_ProgressNote.params["PanelID"] + " #txtFreetext").val();

        if (value_) {
            if (Clinical_FollowUpTCM.params.mode == "Add") {
                Clinical_FollowUpTCM.CreateFollowUp_SOAP_TextProgressNote(value_);
            } else if (Clinical_FollowUpTCM.params.mode == "Edit") {
                Clinical_FollowUpTCM.detach_ComponentsFollowUpNew('Follow Up', true, true);
                setTimeout(function () {
                    Clinical_FollowUpTCM.CreateFollowUp_SOAP_TextProgressNote(value_);
                }, 10);
            }

            Clinical_FollowUpTCM.UnLoad('saved');
        }
        else {
            Clinical_FollowUpTCM.UnLoad('saved');
        }


    },

    CreateFollowUp_SOAP_TextProgressNote: function (value_) {

        if (!Clinical_FollowUpTCM.params.appid)
            Clinical_FollowUpTCM.params.appid = "";

        Clinical_FollowUpTCM.checkFollowUpExists();

        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';

        var $mainDivFollowUp = $(document.createElement('div'));
        var $sectionBodyFollowUp = $(document.createElement('section'));
        $sectionBodyFollowUp.attr('id', "Cli_FollowUp_Main" + Clinical_FollowUpTCM.params.appid);
        $sectionBodyFollowUp.attr('isPrn', "true");
        var $listFollowUp = $(document.createElement('ul'));
        $listFollowUp.css("list-style", "none");

        var $followUpHTML = "<li id='follow_up_freetext' title='Follow Up Appointment'  name='Follow Up'>" + value_ + "<li/>";


        $listFollowUp.append($followUpHTML);

        $sectionBodyFollowUp.append($listFollowUp);

        $mainDivFollowUp.append($sectionBodyFollowUp);
        $mainDivFollowUp.html()

        $(noteHTMLCtrl + ' clinical_followup').parent().parent().addClass('initialVisitBody');

        $(noteHTMLCtrl + ' clinical_followup').parent().parent().append($mainDivFollowUp.html());

        Clinical_ProgressNote.saveComponentSOAPText('Follow Up');

    },

    FollowUpAppointmentSchedule: function () {

        if (Clinical_FollowUpTCM.validateSlotDetail()) {

            var dfd = $.Deferred();
            var self = $("#pnlClinicalFollowUpTCM #frmClinicalFollowUpTCM");
            var myJSON = self.getMyJSONByName();
            Clinical_FollowUpTCM.FollowUpAppointmentSchedule_DBCall(myJSON).done(function (response) {
                response = JSON.parse(response);

                if (response.status != false || response.Message == "This Patient already has an appointment on this slot.") {

                    if (response.AppointmentId) {
                        Clinical_FollowUpTCM.params.appid = response.AppointmentId;
                    }

                    if (Clinical_FollowUpTCM.params.mode == "Add") {
                        Clinical_FollowUpTCM.CreateHTMLProgressNoteFollowUp();
                    } else if (Clinical_FollowUpTCM.params.mode == "Edit") {
                        Clinical_FollowUpTCM.detach_ComponentsFollowUpNew('Follow Up', true, true);
                        setTimeout(function () {
                            Clinical_FollowUpTCM.CreateHTMLProgressNoteFollowUp();
                        }, 10);
                    }
                    Clinical_FollowUpTCM.UnLoad('saved');
                }
                else {
                    utility.DisplayMessages(response.Message, 3);
                    dfd.resolve();
                }
            });
            return dfd;

        }
    },

    validateSlotDetail: function () {

        var IsTrue = true;
        if (!$("#pnlClinicalFollowUpTCM #fromTime").val()) {
            utility.DisplayMessages("Time is required.", 3);
            IsTrue = false;
        }
        else if (!$("#pnlClinicalFollowUpTCM #Facility").val()) {
            utility.DisplayMessages("Facility is required.", 3);
            IsTrue = false;
        }
        else if ($('#dvSlotsDetail').find('input[name="Slot"]:checked').length > 0) {
            IsTrue = true;
            $("#pnlClinicalFollowUpTCM #ChooseSchedule").addClass('hidden');
        }
        else {
            IsTrue = false;
            $("#pnlClinicalFollowUpTCM #ChooseSchedule").removeClass('hidden');
        }
        return IsTrue;

    },

    updateVisit_DBCall: function (AppointmentId) {

        var objData = {};


        objData["commandType"] = "update_patient_visit";
        objData["AppointmentID"] = AppointmentId;
        objData["PatientId"] = Clinical_ProgressNote.params.patientID;
        objData["VisitId"] = Clinical_ProgressNote.params.VisitId


        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FOLLOWUP", "ClinicalFollowUp");

    },

    FollowUpAppointmentSchedule_DBCall: function (appointmentData) {

        //var objData = new Object();
        var objData = JSON.parse(appointmentData);
        if (Clinical_FollowUpTCM.params.mode == "Edit") {

            objData["commandType"] = "update_followup_appointment";
            objData["AppointmentID"] = Clinical_FollowUpTCM.params.FollowUpAppointmentId;
        } else {
            objData["commandType"] = "save_followup_appointment";
        }
        //FollowUp, ClinicalFollowUp,
        var data = JSON.stringify(objData);
        return MDVisionService.APIService(data, "FOLLOWUP", "ClinicalFollowUp");

    },

    UnLoad: function (status) {

        var objDeffered = $.Deferred();
        if (Clinical_FollowUpTCM.params.ParentCtrl == "clinicalTabProgressNote") {
            if (EMRUtility.compareFormDataWithSerialized(Clinical_FollowUpTCM.params.PanelID + ' #frmClinicalFollowUpTCM') && status != 'saved') {
                utility.myConfirmNote('1', function () {
                    $("#" + Clinical_FollowUpTCM.params.PanelID + " #frmClinicalFollowUpTCM").bootstrapValidator('revalidateField', 'Date');
                    $("#" + Clinical_FollowUpTCM.params.PanelID + " #frmClinicalFollowUpTCM").bootstrapValidator('revalidateField', 'Time');
                    $("#" + Clinical_FollowUpTCM.params.PanelID + " #frmClinicalFollowUpTCM").bootstrapValidator('revalidateField', 'Provider');
                    $("#" + Clinical_FollowUpTCM.params.PanelID + " #frmClinicalFollowUpTCM").bootstrapValidator('revalidateField', 'Facility');
                    $("#" + Clinical_FollowUpTCM.params.PanelID + " #frmClinicalFollowUpTCM").bootstrapValidator('revalidateField', 'Duration');
                    var HasError = false;
                    $.each($("#" + Clinical_FollowUpTCM.params.PanelID + " #frmClinicalFollowUpTCM div"), function (i, item) {
                        if ($(item).hasClass("has-error")) {
                            HasError = true;
                            return;
                        }
                    })
                    if (!HasError) {
                        $.when(Clinical_FollowUpTCM.FollowUpAppointmentSchedule()).then(function () {
                            objDeffered.resolve();
                            return objDeffered;
                        });
                    }

                }, "", function () {
                    UnloadActionPan(Clinical_FollowUpTCM.params["ParentCtrl"], "pnlClinicalFollowUpTCM");
                    objDeffered.resolve();
                    return objDeffered;
                },
                '1');
            } else {
                UnloadActionPan(Clinical_FollowUpTCM.params["ParentCtrl"], "pnlClinicalFollowUpTCM");
                objDeffered.resolve();
                return objDeffered;
            }
        }
        else {
            UnloadActionPan(Clinical_FollowUpTCM.params["ParentCtrl"], "pnlClinicalFollowUpTCM");
        }
    },

    createHTMLFollowUp: function (SoapText, AppointmentId, hideAlertMessage) {

        Clinical_FollowUpTCM.checkFollowUpExists();
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';

        var $mainDivFollowUp = $(document.createElement('div'));
        var $sectionBodyFollowUp = $(document.createElement('section'));
        var $detailsDiv = $(document.createElement('div'));
        $sectionBodyFollowUp.attr('id', "Cli_FollowUp_Main" + AppointmentId);
        var $listFollowUp = $(document.createElement('ul'));
        var txt = $.parseHTML(SoapText);
        $listFollowUp.append(txt[0].textContent);

        $detailsDiv.append($listFollowUp);

        $sectionBodyFollowUp.append($detailsDiv);
        $mainDivFollowUp.append($sectionBodyFollowUp);
        $mainDivFollowUp.html()

        $(noteHTMLCtrl + ' clinical_followup').parent().parent().addClass('initialVisitBody');

        $(noteHTMLCtrl + ' clinical_followup').parent().parent().append($mainDivFollowUp.html());

        Clinical_ProgressNote.saveComponentSOAPText('Follow Up', hideAlertMessage);

    },

    CreateHTMLProgressNoteFollowUp: function () {

        Clinical_FollowUpTCM.checkFollowUpExists();

        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';
        var date = $("#pnlClinicalFollowUpTCM #fromDate").val();
        var time = $("#pnlClinicalFollowUpTCM #fromTime").val();
        var duration = $("#pnlClinicalFollowUpTCM #Duration").val();
        var provider = $("#pnlClinicalFollowUpTCM #Provider option:selected").text();
        var facility = $("#pnlClinicalFollowUpTCM #Facility option:selected").text();
        var comments = $("#pnlClinicalFollowUpTCM #txtComments").val();
        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';

        var $mainDivFollowUp = $(document.createElement('div'));
        var $sectionBodyFollowUp = $(document.createElement('section'));
        var $detailsDiv = $(document.createElement('div'));
        $sectionBodyFollowUp.attr('id', "Cli_FollowUp_Main" + Clinical_FollowUpTCM.params.appid);
        $sectionBodyFollowUp.attr('id_', "" + Clinical_FollowUpTCM.params.appid + "");
        var $listFollowUp = $(document.createElement('ul'));

        if (comments == "") {
            var $followUpHTML = "<div title='Follow Up Appointment'  name='Follow Up'><strong>Appointment Date: </strong>" + date + "<br/> <strong>Appointment Time: </strong>" + time + "<br/> <strong>Duration: </strong>" + duration + "<br/> <strong>Provider: </strong>" + provider + "<br/> <strong>Facility: </strong>" + facility + "<br/>";
        } else {
            var $followUpHTML = "<div title='Follow Up Appointment'  name='Follow Up'><strong>Appointment Date: </strong>" + date + "<br/> <strong>Appointment Time: </strong>" + time + "<br/> <strong>Duration: </strong>" + duration + "<br/> <strong>Provider: </strong>" + provider + "<br/> <strong>Facility: </strong>" + facility + "<br/> <strong>Comments: </strong>" + comments + "<br/>";

        }


        $listFollowUp.append("<div>" + $followUpHTML + " </div>");

        $detailsDiv.append($listFollowUp);

        $sectionBodyFollowUp.append($detailsDiv);

        $mainDivFollowUp.append($sectionBodyFollowUp);
        $mainDivFollowUp.html()

        $(noteHTMLCtrl + ' clinical_followup').parent().parent().addClass('initialVisitBody');

        $(noteHTMLCtrl + ' clinical_followup').parent().parent().append($mainDivFollowUp.html());

        Clinical_ProgressNote.saveComponentSOAPText('Follow Up');

    },

    checkFollowUpExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_followup').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="FollowUpComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_followup title="Follow Up"  id="' + Clinical_FollowUpTCM.params.appid + '" class="NotesComponent">' +
                        '<a class="btn btn-link btn-xs" onclick="Clinical_ProgressNote.SelectNotesComponentTab(\'FollowUp\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" title="Follow Up">Follow Up</a> ' +
                                        '<a onclick="Clinical_ProgressNote.RemoveComponentTab(\'Follow Up\',\'-1\',' + Clinical_ProgressNote.params.NotesId + ');" class="btn btn-link btn-xs hidden closeBtn" title="Remove"><i class="fa fa-times"></i></a>' +
                                   '</clinical_followup> </header></li>');
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseenter", function (e) {
                $(this).find('.closeBtn').removeClass('hidden');
                $(this).css('background', '#EAF1F8');
            });

            $('#' + Clinical_ProgressNote.params["PanelID"] + ' .initialVisit li header').bind("mouseleave", function (e) {
                $(this).find('.closeBtn').addClass('hidden');
                $(this).css('background', '#fff');
            });
        }
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #InitialOfficeVisit #ProgressnoteHTML clinical_followup").parent().parent().removeClass('hidden');
        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
    },

    ValidateVisitDate: function () {

        var visitDate = new Date(Clinical_FollowUpTCM.params.CurrentNotesVisitDate);

        var selectedDate = new Date($('#pnlClinicalFollowUpTCM #fromDate').val());

        if (selectedDate < visitDate) {
            $('#pnlClinicalFollowUpTCM #fromDate').val('');
            utility.DisplayMessages('Please select date greater than visit date', 3);
            if ($('#frmClinicalFollowUpTCM').data('bootstrapValidator') != null && typeof $('#frmClinicalFollowUpTCM').data('bootstrapValidator') != 'undefined') {
                $('#frmClinicalFollowUpTCM').bootstrapValidator('revalidateField', 'Date');
            }
        } else {
            if ($('#frmClinicalFollowUpTCM').data('bootstrapValidator') != null && typeof $('#frmClinicalFollowUpTCM').data('bootstrapValidator') != 'undefined') {
                $('#frmClinicalFollowUpTCM').bootstrapValidator('revalidateField', 'Date');
            }
        }


    },

    detach_ComponentsFollowUp: function (componentName, isUpdate, familyHxComponentRemove) {
        var NoteComponentId = $('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML .FollowUpComponent').attr('NoteComponentId');
        if (familyHxComponentRemove) {
            if (NoteComponentId && NoteComponentId != "NCDummyId") {
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Follow Up', true))
                }
                else
                    promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId))
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                    $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Follow Up']").remove();
                });
            }
            else {
                $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Follow Up']").remove();
                var promise = [];
                if (Clinical_ProgressNote.params["TemplateName"]) {
                    $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().addClass('hidden');
                    promise.push(Clinical_ProgressNote.saveComponentSOAPText('Follow Up', true))
                }
                else {
                    if (NoteComponentId && NoteComponentId != "NCDummyId")
                        promise.push(Clinical_ProgressNote.removeComponentSOAPText_DBCall(NoteComponentId));
                    else {
                        var def = $.Deferred();
                        promise.push(def1);
                        def.resolve();
                    }
                }
                $.when.apply($, promise).done(function () {
                    if (Clinical_ProgressNote.params["TemplateName"] == "")
                        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().remove();
                    Clinical_ProgressNote.ShowHideComponetsHeaders();
                    Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                });
                utility.DisplayMessages('Successfully Deleted!', 1);
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().find('section[id*="Cli_FollowUp_Main"]').remove();
        }
    },

    detach_ComponentsFollowUpNew: function (componentName, isUpdate, familyHxComponentRemove) {

        if (familyHxComponentRemove) {
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #InitialOfficeVisit #ProgressnoteHTML").find("section[id*='FollowUp']").remove()
            Clinical_ProgressNote.saveComponentSOAPText('Follow Up', true);
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().find('section[id*="Cli_FollowUp_Main"]').remove();
        }
    },
}