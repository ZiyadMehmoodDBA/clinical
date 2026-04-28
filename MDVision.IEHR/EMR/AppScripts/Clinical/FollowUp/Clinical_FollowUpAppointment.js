Clinical_FollowUpAppointment = {
    bIsFirstLoad: true,
    params: [],
    appDate: null,
    Load: function (params) {

        Clinical_FollowUpAppointment.params = params;
        var self = $('#pnlClinicalFollowUpAppointment');
        self.loadDropDowns(true).done(function () {


            if (Clinical_ProgressNote.params.AppointmentId != null && Clinical_ProgressNote.params.AppointmentId > 0 && Clinical_FollowUpAppointment.params.AppointmentTimeFrom != null) {
                $('#pnlClinicalFollowUpAppointment #txtFromTime').timepicker("setTime", Clinical_FollowUpAppointment.tConvert(Clinical_FollowUpAppointment.params.AppointmentTimeFrom));
                $('#pnlClinicalFollowUpAppointment #txtToTime').timepicker("setTime", Clinical_FollowUpAppointment.tConvert(Clinical_FollowUpAppointment.params.AppointmentTimeTo));
                Clinical_FollowUpAppointment.appDate = new Date($('#' + Clinical_ProgressNote.params.PanelID + ' #txtLinkedAppointment').val().split(" ")[0]);
            } else {
                var ntTime = $('#' + Clinical_ProgressNote.params.PanelID + ' #VisitTime').val();
                $('#pnlClinicalFollowUpAppointment #txtFromTime').timepicker("setTime", ntTime);
                try {
                    var toTime = Clinical_FollowUpAppointment.parseTime(ntTime);
                    toTime.setMinutes(toTime.getMinutes() + 15);
                    $('#pnlClinicalFollowUpAppointment #txtToTime').timepicker("setTime", toTime);

                } catch (e) {
                }

                Clinical_FollowUpAppointment.appDate = new Date($('#pnlClinicalProgressNote #frmClinicalProgressNote #dtpVisitDate').val());
                $("#pnlClinicalFollowUpAppointment #frmClinicalFollowUpAppointment #txtFromTime").on('changeTime.timepicker', function (e) {
                    Clinical_FollowUpAppointment.AddTime(this);
                });
            }


            $("#pnlClinicalFollowUpAppointment #fromDate").val($.datepicker.formatDate('mm/dd/yy', Clinical_FollowUpAppointment.appDate));
            $('#pnlClinicalFollowUpAppointment #Provider').val(Clinical_FollowUpAppointment.params.CurrentNotesProviderId);
            $('#pnlClinicalFollowUpAppointment #Facility').val(Clinical_FollowUpAppointment.params.CurrentNotesFacilityId);


        });

        utility.CreateDatePicker('pnlClinicalFollowUpAppointment #fromDate', function () {

            Clinical_FollowUpAppointment.ValidateVisitDate();

        }, true);

        $("#frmClinicalFollowUpAppointment #btnSave").click(function (e) {
            Clinical_FollowUpAppointment.FollowUpAppointmentSave();
        });

        utility.callbackAfterAllDOMLoaded(function () {

            //Serialization
            $('#' + Clinical_FollowUpAppointment.params.PanelID + ' #frmClinicalFollowUpAppointment').data('serialize', $('#' + Clinical_FollowUpAppointment.params.PanelID + ' #frmClinicalFollowUpAppointment').serialize());

        });
        //Followup Appointments Provider Schedule Slot Implementation IMP-923
        Clinical_FollowUpAppointment.SetUpFollowUpAppointmentEvents();

    },

    parseTime: function (t) {
        var d = new Date();
        var time = t.match(/(\d+)(?::(\d\d))?\s*(p?)/);
        d.setHours(parseInt(time[1]) + (time[3] ? 12 : 0));
        d.setMinutes(parseInt(time[2]) || 0);
        return d;
    },
    ValidateAppointment: function () {
        $('#frmClinicalFollowUpAppointment')
           .bootstrapValidator({
               live: 'disabled',
               message: 'This value is not valid',
               feedbackIcons: {
                   valid: 'glyphicon glyphicon-ok',
                   invalid: 'glyphicon glyphicon-remove',
                   validating: 'glyphicon glyphicon-refresh'
               },
               fields: {

                   FromTime: {
                       group: '.col-sm-2',
                       validators: {
                           notEmpty: {
                               message: ''
                           },
                       }
                   },
                   ToTime: {
                       group: '.col-sm-2',
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
            Clinical_FollowUpAppointment.FollowUpAppointmentSave();
        });
    },
    tConvert: function (time) {
        var hours = time.split(':')[0];
        var minutes = time.split(':')[1];
        time = hours + ":" + minutes;
        // Check correct time format and split into components
        time = time.toString().match(/^([01]\d|2[0-3])(:)([0-5]\d)(:[0-5]\d)?$/) || [time];

        if (time.length > 1) { // If time format correct
            time = time.slice(1); // Remove full string match value
            time[5] = +time[0] < 12 ? ' am' : ' pm'; // Set AM/PM
            time[0] = +time[0] % 12 || 12; // Adjust hours
        }
        return time.join(''); // return adjusted time or original string
    },
    AddTime: function (obj) {

        var fromTimeObj = $("#txtFromTime");
        var toTimeObj = $("#txtToTime")

        var timeFrom = new Date("01/01/2007 " + $('#txtFromTime').val());
        timeFrom.setMinutes(timeFrom.getMinutes());
        $("#txtToTime").timepicker({ ampm: true });
        $("#txtToTime").timepicker("setTime", timeFrom);
        //$("#txtToTime").val(Clinical_FollowUpAppointment.formatAMPM(new Date("01/01/2007 " + $('#txtToTime').val())));
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
    SetUpFollowUpAppointmentEvents: function () {

        $("#pnlClinicalFollowUpAppointment #pnlFollowupAppointments input[type='radio']").on("click", function () {

            var soaptextValue = "";
            var cval = $(this).attr("cval");
            if (cval == "prn") {
                soaptextValue = "As per need.";
                Clinical_FollowUpAppointment.params.ctype = "prn";
                Clinical_FollowUpAppointment.params.cval = "";
                $("#pnlClinicalFollowUpAppointment #chkCreateAppointment").attr("disabled", "disabled");
                $("#pnlClinicalFollowUpAppointment #chkCreateAppointment").prop('checked', false);
            }
            else {
                $("#pnlClinicalFollowUpAppointment #chkCreateAppointment").attr("disabled", false);
                var ctype = $(this).attr("ctype");
                var addDays = 0;
                if (Clinical_ProgressNote.params.AppointmentId != null && Clinical_ProgressNote.params.AppointmentId > 0 && Clinical_FollowUpAppointment.params.AppointmentTimeFrom != null) {
                    Clinical_FollowUpAppointment.appDate = new Date($('#' + Clinical_ProgressNote.params.PanelID + ' #txtLinkedAppointment').val().split(" ")[0]);
                } else {
                    Clinical_FollowUpAppointment.appDate = new Date($('#pnlClinicalProgressNote #frmClinicalProgressNote #dtpVisitDate').val());
                }
                var myDate = Clinical_FollowUpAppointment.appDate;


                switch (ctype) {

                    case "Ds":
                        ctype = "Days";
                        addDays = parseInt(cval);
                        myDate.setDate(myDate.getDate() + addDays);
                        $("#pnlClinicalFollowUpAppointment #fromDate").val($.datepicker.formatDate('mm/dd/yy', myDate));
                        break;
                    case "W":
                        parseInt(cval) > 1 ? ctype = "Weeks" : ctype = "Week";
                        addDays = parseInt(cval) * 7;
                        myDate.setDate(myDate.getDate() + addDays);
                        $("#pnlClinicalFollowUpAppointment #fromDate").val($.datepicker.formatDate('mm/dd/yy', myDate));
                        break;
                    case "M":
                        parseInt(cval) > 1 ? ctype = "Months" : ctype = "Month";

                        myDate.setMonth(myDate.getMonth() + parseInt(cval));
                        $("#pnlClinicalFollowUpAppointment #fromDate").val($.datepicker.formatDate('mm/dd/yy', myDate));
                        break;
                    case "Y":
                        parseInt(cval) > 1 ? ctype = "Years" : ctype = "Year";
                        myDate.setFullYear(myDate.getFullYear() + parseInt(cval));
                        $("#pnlClinicalFollowUpAppointment #fromDate").val($.datepicker.formatDate('mm/dd/yy', myDate));
                        break;
                }

                soaptextValue = "Patient needs to be seen again in " + cval + " " + ctype;
                Clinical_FollowUpAppointment.params.ctype = ctype;
                Clinical_FollowUpAppointment.params.cval = cval;
            }
            if (soaptextValue != "") {
                $("#pnlClinicalFollowUpAppointment #followUpText").html(soaptextValue);
                $("#pnlClinicalFollowUpAppointment #txtComments").attr("rows", "3");
            }
            else {
                $("#pnlClinicalFollowUpAppointment #followUpText").html("");
                $("#pnlClinicalFollowUpAppointment #txtComments").attr("rows", "4");
                Clinical_FollowUpAppointment.params.ctype = "";
                Clinical_FollowUpAppointment.params.cval = "";
            }
        });

    },

    FollowUpAppointmentSave: function () {

        var soapText = $("#pnlClinicalFollowUpAppointment #followUpText").html();
        var comments = $("#pnlClinicalFollowUpAppointment #txtComments").val();
        if ($('#' + Clinical_FollowUpAppointment.params.PanelID + ' #frmClinicalFollowUpAppointment input[name="r1"]:checked').length > 0) {
            //if ($("#pnlClinicalFollowUpAppointment #chkCreateAppointment").is(':checked') == true) {
            if ($("#" + Clinical_FollowUpAppointment.params.PanelID + " #frmClinicalFollowUpAppointment #Facility").val() == "" || $("#" + Clinical_FollowUpAppointment.params.PanelID + " #frmClinicalFollowUpAppointment #Provider").val() == "") {
                utility.DisplayMessages("Please Select Follow Up Appointment Facility/Provider", 2);
                return;
            }
            var valuestart = $('#pnlClinicalFollowUpAppointment #txtFromTime').val();
            var valuestop = $('#pnlClinicalFollowUpAppointment #txtToTime').val()
            var hourStart = new Date("01/01/2007 " + valuestart).getHours();
            var hourEnd = new Date("01/01/2007 " + valuestop).getHours();
            var minuteStart = new Date("01/01/2007 " + valuestart).getMinutes();
            var minuteEnd = new Date("01/01/2007 " + valuestop).getMinutes();
            var hourDiff = hourEnd - hourStart;
            hourDiff = hourDiff * 60;
            var minuteDiff = minuteEnd - minuteStart;
            minuteDiff = parseInt(hourDiff) > 0 ? parseInt(hourDiff) + parseInt(minuteDiff) : parseInt(minuteDiff);
            var duration = parseInt(minuteDiff);

            var objData = new Object();
            objData["Duration"] = duration.toString();
            objData["hfpatientid"] = $('#PatientProfile #hfPatientId').val();
            objData["hfProviderId"] = $('#pnlClinicalFollowUpAppointment #Provider').val();
            objData["hfFacilityId"] = $('#pnlClinicalFollowUpAppointment #Facility').val();
            objData["ddlStatus"] = '9';
            objData["txtComments"] = $("#pnlClinicalFollowUpAppointment #txtComments").val();
            // objData["ddlPatientType"] = '2';
            // objData["ddlVisitType"] = '32';
            objData["txtScheduleDate"] = $('#pnlClinicalFollowUpAppointment #fromDate').val();
            objData["txtFromTime"] = $('#pnlClinicalFollowUpAppointment #txtFromTime').val();
            objData["txtToTime"] = $('#pnlClinicalFollowUpAppointment #txtToTime').val();
            objData["FromFollowUp"] = "True";
            objData["FollowUpAppointmentNotesId"] = Clinical_ProgressNote.params.NotesId;


            var data = JSON.stringify(objData);

            if (($('#pnlClinicalFollowUpAppointment #txtFromTime').val() == "" && $('#pnlClinicalFollowUpAppointment #txtToTime').val() == "") || ($('#pnlClinicalFollowUpAppointment #txtFromTime').val() == "" && $('#pnlClinicalFollowUpAppointment #txtToTime').val() != "") || ($('#pnlClinicalFollowUpAppointment #txtFromTime').val() != "" && $('#pnlClinicalFollowUpAppointment #txtToTime').val() == "")) {
                utility.DisplayMessages("Please Select Follow Up Appointment TimeFrom/TimeTo.", 2);
                return;
            }

            if (soapText != "" || comments != "") {
                if (Clinical_FollowUpAppointment.params.mode == "Add") {
                    Clinical_FollowUpAppointment.CreateFollowUp_SOAP_TextProgressNote(soapText, comments);
                } else if (Clinical_FollowUpAppointment.params.mode == "Edit") {

                    Clinical_FollowUpAppointment.detach_ComponentsFollowUp('Follow Up', true, true, false).done(function () {
                        if ($("#pnlClinicalFollowUpAppointment #chkCreateAppointment").is(':checked') == true) {
                            Clinical_FollowUpAppointment.SaveFollowUpAppointment(data).done(function (response) {
                                if (response.status != false) {
                                    Clinical_FollowUpAppointment.CreateFollowUp_SOAP_TextProgressNote(soapText, comments);
                                } else {
                                    utility.DisplayMessages(response.Message, 3);
                                }
                            });
                        } else {
                            Clinical_FollowUpAppointment.CreateFollowUp_SOAP_TextProgressNote(soapText, comments);
                        }
                    });
                }
                UnloadActionPan(Clinical_FollowUpAppointment.params["ParentCtrl"], "pnlClinicalFollowUpAppointment");
            }
            else {
                utility.DisplayMessages("Please Select Follow Up Appointment/Comments.", 2);
            }

        }
        else
            utility.DisplayMessages("Please select a follow-up appointment period.", 3);

    },
    SaveFollowUpAppointment: function (AppointmentData) {

        var data = "AppointmentData=" + AppointmentData;

        return MDVisionService.defaultService(data, "SCHEDULING_APPOINTMENT_DETAIL", "SAVE_PATIENT_APPOINTMENT");
    },
    CreateFollowUp_SOAP_TextProgressNote: function (soapText, comments, hideAlertMessage, cval, ctype) {

        var noteHTMLCtrl = '#' + Clinical_ProgressNote.params["PanelID"] + ' #ProgressnoteHTML';


        $('#' + Clinical_ProgressNote.params["PanelID"] + " #ActionsInitialOfficeVisit button[title='Follow Up']").remove();
        if (Clinical_ProgressNote.params["TemplateName"])
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().addClass('hidden');
        else
            $('#' + Clinical_ProgressNote.params["PanelID"] + " #InitialOfficeVisit #ProgressnoteHTML clinical_followup").parent().parent().remove();
        $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().find('section[id*="Cli_FollowUp_Main"]').remove();

        Clinical_FollowUpAppointment.checkFollowUpExists();


        var $mainDivFollowUp = $(document.createElement('div'));
        var $sectionBodyFollowUp = $(document.createElement('section'));
        var $detailsDiv = $(document.createElement('div'));
        $sectionBodyFollowUp.attr('id', "Cli_FollowUp_Main" + Clinical_FollowUpAppointment.params.appid);
        var $listFollowUp = $(document.createElement('ul'));


        Clinical_FollowUpAppointment.params.ctype = Clinical_FollowUpAppointment.params.ctype ? Clinical_FollowUpAppointment.params.ctype : "";
        Clinical_FollowUpAppointment.params.cval = Clinical_FollowUpAppointment.params.cval ? Clinical_FollowUpAppointment.params.cval : "";
        if (typeof cval != typeof undefined && cval != null && cval != "" && typeof ctype != typeof undefined && ctype != null && ctype != "") {
            Clinical_FollowUpAppointment.params.ctype = ctype;
            Clinical_FollowUpAppointment.params.cval = cval;
        }

        if (soapText != "") {
            soapText = "<div class='haveText' ctype='" + Clinical_FollowUpAppointment.params.ctype + "' cval='" + Clinical_FollowUpAppointment.params.cval + "'> " + soapText + "</div>";
        }
        if (comments != "") {
            soapText += "<div class='haveText'> <strong>Comments: </strong> " + comments + "</div>";
        }

        var $followUpHTML = "<div title='Follow Up Appointment'  name='Follow Up'>" + soapText + "<div/>";


        $listFollowUp.append("<div>" + $followUpHTML + " </div>");

        $detailsDiv.append($listFollowUp);

        $sectionBodyFollowUp.append($detailsDiv);

        $mainDivFollowUp.append($sectionBodyFollowUp);

        $(noteHTMLCtrl + ' clinical_followup').parent().parent().addClass('initialVisitBody');

        $(noteHTMLCtrl + ' clinical_followup').parent().parent().append($mainDivFollowUp.html());

        Clinical_ProgressNote.saveComponentSOAPText('Follow Up', hideAlertMessage);

    },

    checkFollowUpExists: function () {
        if ($('#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML clinical_followup').length == 0) {
            var CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #PlanNoteComponentList';
            if (Clinical_ProgressNote.params["TemplateName"] != '')
                CompnentSelector = '#' + Clinical_ProgressNote.params["PanelID"] + ' #InitialOfficeVisit #ProgressnoteHTML #ProgressNoteComponentList';

            $(CompnentSelector).append(' <li class="FollowUpComponent" NoteComponentId="NCDummyId"> <header>' +
                        '<clinical_followup title="Follow Up"  id="' + Clinical_FollowUpAppointment.params.appid + '" class="NotesComponent">' +
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

        var visitDate = new Date(Clinical_FollowUpAppointment.params.CurrentNotesVisitDate);

        var selectedDate = new Date($('#pnlClinicalFollowUpAppointment #fromDate').val());

        if (selectedDate < visitDate) {
            $('#pnlClinicalFollowUpAppointment #fromDate').val('');
            utility.DisplayMessages('Please select date greater than visit date', 3);

        }
    },

    detach_ComponentsFollowUp: function (componentName, isUpdate, familyHxComponentRemove, isSuccessMessage) {

        var dfd = $.Deferred();

        if (isSuccessMessage != false)
            isSuccessMessage = true;

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
                    dfd.resolve();
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
                        var def1 = $.Deferred();
                        promise.push(def1);
                        def1.resolve();
                    }
                }
                try {
                    $.when.apply($, promise).done(function () {
                        if (Clinical_ProgressNote.params["TemplateName"] == "")
                            $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().remove();
                        Clinical_ProgressNote.ShowHideComponetsHeaders();
                        Clinical_ProgressNote.CreateNotesComponent_Buttons($("#pnlClinicalProgressNote #hfNotesId").val());
                        dfd.resolve();
                    });
                }
                catch (ex)
                {
                    console.log(ex);
                }
                if (isSuccessMessage)
                    utility.DisplayMessages('Successfully Deleted!', 1);
            }
        } else {
            $('#' + Clinical_ProgressNote.params["PanelID"] + ' clinical_followup').parent().parent().find('section[id*="Cli_FollowUp_Main"]').remove();
            dfd.resolve();
        }

        return dfd;
    },

    UnLoad: function () {

        utility.UnLoadDialog(("#" + Clinical_FollowUpAppointment.params.PanelID + " #frmClinicalFollowUpAppointment"),
            function () {
                UnloadActionPan(Clinical_FollowUpAppointment.params["ParentCtrl"], "pnlClinicalFollowUpAppointment");
            }, function () {
                UnloadActionPan(Clinical_FollowUpAppointment.params["ParentCtrl"], "pnlClinicalFollowUpAppointment");
            });
    },
}